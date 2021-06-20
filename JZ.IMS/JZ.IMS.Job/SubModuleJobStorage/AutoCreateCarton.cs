using JZ.IMS.Core;
using JZ.IMS.Core.Repository;
using JZ.IMS.Core.Utilities.Reflect;
using JZ.IMS.Models;
using JZ.IMS.ViewModels;
using System;
using System.Collections.Generic;
using System.ComponentModel.Design;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JZ.IMS.Job.SubModuleJobStorage
{
    public class AutoCreateCarton : IMesSubModuleJob<SfcsRuncard, decimal>
    {
        public Task<KeyValuePair<bool, string>> GetTask(Propertyprovider propertyprovider, IBaseRepository<SfcsRuncard, decimal> repository, IDbTransaction transaction)
        {
            return Task.Run(() =>
            {
                try
                {
                    //1、判断有没有配置需要自动产生包装
                    if (propertyprovider.carton != null )
                    {
                        if (propertyprovider.carton._SfcsProductCarton==null||(propertyprovider.carton._SfcsProductCarton != null&&propertyprovider.carton._SfcsProductCarton.AUTO_CREATE_FLAG == "Y"))
                        {
                            if (propertyprovider.carton.DefinedQty <= 0)
                            {
                                if(propertyprovider.carton._SfcsProductCarton == null)
                                {
                                    throw new Exception("请您输入包装容量!");
                                }
                                else
                                {
                                    propertyprovider.carton.DefinedQty = propertyprovider.carton._SfcsProductCarton.STANDARD_QUANTITY;
                                }
                            }
                            String Carton_NO = GetCartonAsync(propertyprovider, repository, transaction);
                            Decimal id = repository.QueryEx<Decimal>("SELECT SFCS_COLLECT_OBJECT_SEQ.NEXTVAL FROM DUAL").FirstOrDefault();
                            CollectData collectData = new CollectData();
                            collectData.CollectObjectID = id;
                            collectData.Data = Carton_NO;
                            collectData.CollectTime = DateTime.Now;
                            collectData.CollectBy = propertyprovider.sys_Manager.USER_NAME;
                            propertyprovider.carton.Carton_NO = Carton_NO;
                            if(propertyprovider.carton.collectDataList == null)
                            {
                                propertyprovider.carton.collectDataList = new List<CollectData>();
                            }
                            propertyprovider.carton.collectDataList.Add(collectData);
                            propertyprovider.carton.Status = StandardObjectStatusType.Completed;
                        }
                    }
                    return new KeyValuePair<bool, string>(true, "");
                }
                catch (Exception ex)
                {

                    return new KeyValuePair<bool, string>(false, "AutoPakage:" + ex.Message);
                }
            });
        }

        /// <summary>
        /// 獲取普通
        /// </summary>
        /// <returns></returns>
        public string GetCartonAsync(Propertyprovider propertyprovider, IBaseRepository<SfcsRuncard, decimal> repository, IDbTransaction transaction)
        {
            String SerialNumber = propertyprovider.sfcsRuncard.SN;
            decimal siteId = propertyprovider.sfcsOperationSites.ID;
            String containerSql = @"SELECT SCL.* FROM SFCS_CONTAINER_LIST SCL 
            WHERE PART_NO = :PART_NO AND DATA_TYPE = :DATA_TYPE AND SITE_ID = :SITE_ID AND FULL_FLAG = 'N'";
            SfcsContainerList sfcsContainerList = repository.QueryEx<SfcsContainerList>(
                containerSql,
                new
                {
                    PART_NO = propertyprovider.product.partNumber,
                    DATA_TYPE = GlobalVariables.CartonLable,
                    SITE_ID = siteId
                }).FirstOrDefault();
            String cartonNo = null;
            if (sfcsContainerList == null)
            {
                SfcsContainerList container = new GetRangerNo().GetRangerNoByRuleType(propertyprovider, GlobalVariables.RangerCartonNo, repository);
                if (container == null)
                {
                    //使用SFCS_PACKING_CARTON_SEQ
                    decimal sequence = repository.QueryEx<decimal>("SELECT SFCS_PACKING_CARTON_SEQ.NEXTVAL FROM DUAL ").FirstOrDefault();

                    //將序列轉成36進制表示
                    string result = Core.Utilities.RadixConvertPublic.RadixConvert(sequence.ToString(), ViewModels.GlobalVariables.DecRadix, ViewModels.GlobalVariables.Base36Redix);

                    //六位表示
                    string ReleasedSequence = result.PadLeft(6, '0');
                    string yymmdd = repository.QueryEx<string>("SELECT TO_CHAR(SYSDATE,'YYMMDD') YYMMDD FROM DUAL ").FirstOrDefault();
                    cartonNo = "BOX" + yymmdd + ReleasedSequence;

                    string I_InsertContainerList = @"INSERT INTO SFCS_CONTAINER_LIST (DATA_TYPE, CONTAINER_SN, PART_NO, SITE_ID, QUANTITY, FULL_FLAG, SEQUENCE)
                                                      VALUES (:DATA_TYPE, :CONTAINER_SN, :PART_NO, :SITE_ID, :QUANTITY, :FULL_FLAG, :SEQUENCE) ";
                    repository.Execute(I_InsertContainerList, new
                    {
                        DATA_TYPE = GlobalVariables.CartonLable,
                        CONTAINER_SN = cartonNo,
                        PART_NO = propertyprovider.product.partNumber,
                        SITE_ID = propertyprovider.sfcsOperationSites.ID,
                        QUANTITY = propertyprovider.carton.DefinedQty,
                        FULL_FLAG = propertyprovider.carton.DefinedQty == 1 ? "Y" : "N",
                        SEQUENCE = 1
                    }, transaction);
                }
                else
                {
                    cartonNo = container.CONTAINER_SN;

                    string I_InsertContainerList = @"INSERT INTO SFCS_CONTAINER_LIST (DATA_TYPE, CONTAINER_SN, PART_NO, SITE_ID, QUANTITY, FULL_FLAG, SEQUENCE,RANGER_RULE_ID,DIGITAL,RANGE,FIX_HEADER,FIX_TAIL)
                                                      VALUES (:DATA_TYPE, :CONTAINER_SN, :PART_NO, :SITE_ID, :QUANTITY, :FULL_FLAG, :SEQUENCE,:RANGER_RULE_ID,:DIGITAL,:RANGE,:FIX_HEADER,:FIX_TAIL) ";
                    repository.Execute(I_InsertContainerList, new
                    {
                        DATA_TYPE = GlobalVariables.CartonLable,
                        CONTAINER_SN = cartonNo,
                        PART_NO = propertyprovider.product.partNumber,
                        SITE_ID = propertyprovider.sfcsOperationSites.ID,
                        QUANTITY = propertyprovider.carton.DefinedQty,
                        FULL_FLAG = propertyprovider.carton.DefinedQty == 1 ? "Y" : "N",
                        SEQUENCE = 1,
                        RANGER_RULE_ID = container.RANGER_RULE_ID,
                        DIGITAL = container.DIGITAL,
                        RANGE = container.RANGE,
                        FIX_HEADER = container.FIX_HEADER,
                        FIX_TAIL = container.FIX_TAIL
                    }, transaction);

                }
            }
            else
            {
                cartonNo = sfcsContainerList.CONTAINER_SN;
                String seqSql = @"select count(*) from SFCS_RUNCARD where CARTON_NO = :CARTON_NO";
                
                decimal seq = repository.QueryEx<decimal>(seqSql, new
                {
                    CARTON_NO = cartonNo
                }).FirstOrDefault();
                if(propertyprovider.carton.DefinedQty  != sfcsContainerList.QUANTITY)
                {
                    if(propertyprovider.carton.DefinedQty < sfcsContainerList.QUANTITY 
                        && propertyprovider.carton.DefinedQty <= seq)
                    {
                        throw new Exception(string.Format("原有包装容量是{0},修改的包装容量{1}不能小于已包装数量{2}!", 
                            sfcsContainerList.QUANTITY, propertyprovider.carton.DefinedQty, seq));
                    }
                    else
                    {
                        String U_UpdateContainewQty = @"UPDATE  SFCS_CONTAINER_LIST SET QUANTITY = :QUANTITY,UPDATED_DATE = sysdate 
                            WHERE CONTAINER_SN = :CONTAINER_SN AND DATA_TYPE = :DATA_TYPE AND PART_NO = :PART_NO AND SITE_ID = :SITE_ID";
                        repository.Execute(U_UpdateContainewQty, new
                        {
                            QUANTITY = propertyprovider.carton.DefinedQty,
                            DATA_TYPE = GlobalVariables.CartonLable,
                            CONTAINER_SN = cartonNo,
                            PART_NO = propertyprovider.product.partNumber,
                            SITE_ID = propertyprovider.sfcsOperationSites.ID
                        }, transaction);
                    }
                }
                //卡通剛刷滿
                if (propertyprovider.carton.DefinedQty == seq + 1)
                {
                    string U_UpdateContainerList = @"UPDATE SFCS_CONTAINER_LIST SET FULL_FLAG = 'Y', ATTRIBUTE1 = 'AUTO'
                                                      WHERE DATA_TYPE = :DATA_TYPE
                                                      AND CONTAINER_SN = :CONTAINER_SN
                                                      AND PART_NO = :PART_NO
                                                      AND SITE_ID = :SITE_ID ";
                    repository.Execute(U_UpdateContainerList, new
                    {
                        DATA_TYPE = GlobalVariables.CartonLable,
                        CONTAINER_SN = cartonNo,
                        PART_NO = propertyprovider.product.partNumber,
                        SITE_ID = propertyprovider.sfcsOperationSites.ID
                    }, transaction);
                }
                else if(propertyprovider.carton.DefinedQty < seq + 1)
                {
                    throw new Exception("包装数量不能大于包装容量!");
                }
                String U_UpadateContainerListSeq = @"UPDATE SFCS_CONTAINER_LIST SET SEQUENCE= :SEQUENCE+1 WHERE CONTAINER_SN=:CONTAINER_SN";
                repository.Execute(U_UpadateContainerListSeq, new
                {
                    SEQUENCE = seq,
                    CONTAINER_SN = cartonNo
                }, transaction);
            }
            return cartonNo;
        }
    }
}
