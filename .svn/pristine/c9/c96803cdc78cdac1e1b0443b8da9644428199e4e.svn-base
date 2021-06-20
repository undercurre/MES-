using JZ.IMS.Core;
using JZ.IMS.Core.Repository;
using JZ.IMS.Core.Utilities.Reflect;
using JZ.IMS.Models;
using JZ.IMS.ViewModels;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JZ.IMS.Job.SubModuleJobStorage
{
    public class AutoCreatePallect : IMesSubModuleJob<SfcsRuncard, decimal>
    {
        public Task<KeyValuePair<bool, string>> GetTask(Propertyprovider propertyprovider, IBaseRepository<SfcsRuncard, decimal> repository, IDbTransaction transaction)
        {
            return Task.Run(() =>
            {
                try
                {
                    //1、判断有没有配置需要自动产生包装
                    //1、判断有没有配置需要自动产生包装
                    if (propertyprovider.pallet != null)
                    {
                        if (propertyprovider.pallet._SfcsProductPallet != null)//采集栈板：不需要产生包装
                        {
                            return new KeyValuePair<bool, string>(true, "");
                        }
                        if (propertyprovider.pallet.DefinedQty <= 0)
                        {
                            if (propertyprovider.pallet._SfcsProductPallet == null)
                            {
                                throw new Exception("请您输入包装容量!");
                            }
                            else
                            {
                                propertyprovider.pallet.DefinedQty = propertyprovider.pallet._SfcsProductPallet.STANDARD_QUANTITY;
                            }
                        }
                        String pallect_NO = GetPallectAsync(propertyprovider, repository, transaction);
                        Decimal id = repository.QueryEx<Decimal>("SELECT SFCS_COLLECT_OBJECT_SEQ.NEXTVAL FROM DUAL").FirstOrDefault();
                        CollectData collectData = new CollectData();
                        collectData.CollectObjectID = id;
                        collectData.Data = pallect_NO;
                        collectData.CollectTime = DateTime.Now;
                        collectData.CollectBy = propertyprovider.sys_Manager.USER_NAME;
                        propertyprovider.pallet.Pallet_NO = pallect_NO;
                        if (propertyprovider.pallet.collectDataList == null)
                        {
                            propertyprovider.pallet.collectDataList = new List<CollectData>();
                        }
                        propertyprovider.pallet.collectDataList.Add(collectData);
                        propertyprovider.pallet.Status = StandardObjectStatusType.Completed;
                    }
                    return new KeyValuePair<bool, string>(true, "");
                }
                catch (Exception ex)
                {

                    return new KeyValuePair<bool, string>(false, "AutoLinkPallet:" + ex.Message);
                }
            });
        }

        /// <summary>
        /// 獲取普通
        /// </summary>
        /// <returns></returns>
        public string GetPallectAsync(Propertyprovider propertyprovider, IBaseRepository<SfcsRuncard, decimal> repository, IDbTransaction transaction)
        {
            decimal siteId = propertyprovider.sfcsOperationSites.ID;
            String containerSql = @"SELECT SCL.* FROM SFCS_CONTAINER_LIST SCL 
            WHERE  DATA_TYPE = :DATA_TYPE AND SITE_ID = :SITE_ID AND FULL_FLAG = 'N'";
            SfcsContainerList sfcsContainerList = repository.QueryEx<SfcsContainerList>(
                containerSql,
                new
                {
                    DATA_TYPE = GlobalVariables.PallectLabel,
                    SITE_ID = siteId
                }).FirstOrDefault();
            String pallectNo = null;
            if (sfcsContainerList == null)
            {
                SfcsContainerList container = new GetRangerNo().GetRangerNoByRuleType(propertyprovider, GlobalVariables.RangerPallectNo, repository);
                if (container == null)
                {
                    //使用SFCS_PACKING_CARTON_SEQ
                    decimal sequence = repository.QueryEx<decimal>("SELECT SFCS_PACKING_CARTON_SEQ.NEXTVAL FROM DUAL ").FirstOrDefault();

                    //將序列轉成36進制表示
                    string result = Core.Utilities.RadixConvertPublic.RadixConvert(sequence.ToString(), ViewModels.GlobalVariables.DecRadix, ViewModels.GlobalVariables.Base36Redix);

                    //六位表示
                    string ReleasedSequence = result.PadLeft(6, '0');
                    string yymmdd = repository.QueryEx<string>("SELECT TO_CHAR(SYSDATE,'YYMMDD') YYMMDD FROM DUAL ").FirstOrDefault();
                    pallectNo = "PT" + yymmdd + ReleasedSequence;

                    string I_InsertContainerList = @"INSERT INTO SFCS_CONTAINER_LIST (DATA_TYPE, CONTAINER_SN, SITE_ID, QUANTITY, FULL_FLAG, SEQUENCE)
                                                      VALUES (:DATA_TYPE, :CONTAINER_SN, :SITE_ID, :QUANTITY, :FULL_FLAG, :SEQUENCE) ";
                    repository.Execute(I_InsertContainerList, new
                    {
                        DATA_TYPE = GlobalVariables.PallectLabel,
                        CONTAINER_SN = pallectNo,
                        SITE_ID = propertyprovider.sfcsOperationSites.ID,
                        QUANTITY = propertyprovider.pallet.DefinedQty,
                        FULL_FLAG = propertyprovider.pallet.DefinedQty == 1 ? "Y" : "N",
                        SEQUENCE = 1
                    }, transaction);
                }
                else
                {
                    pallectNo = container.CONTAINER_SN;
                    string I_InsertContainerList = @"INSERT INTO SFCS_CONTAINER_LIST (DATA_TYPE, CONTAINER_SN, PART_NO, SITE_ID, QUANTITY, FULL_FLAG, SEQUENCE,RANGER_RULE_ID,DIGITAL,RANGE,FIX_HEADER,FIX_TAIL)
                                                      VALUES (:DATA_TYPE, :CONTAINER_SN, :PART_NO, :SITE_ID, :QUANTITY, :FULL_FLAG, :SEQUENCE,:RANGER_RULE_ID,:DIGITAL,:RANGE,:FIX_HEADER,:FIX_TAIL) ";
                    repository.Execute(I_InsertContainerList, new
                    {
                        DATA_TYPE = GlobalVariables.PallectLabel,
                        CONTAINER_SN = pallectNo,
                        SITE_ID = propertyprovider.sfcsOperationSites.ID,
                        QUANTITY = propertyprovider.pallet.DefinedQty,
                        FULL_FLAG = propertyprovider.pallet.DefinedQty == 1 ? "Y" : "N",
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
                pallectNo = sfcsContainerList.CONTAINER_SN;
                //String seqSql = @"select count( distinct CARTON_NO) from SFCS_RUNCARD where PALLET_NO = :PALLET_NO";

                //decimal seq = repository.QueryEx<decimal>(seqSql, new
                //{
                //    PALLET_NO = pallectNo
                //}).FirstOrDefault();
                ////卡通剛刷滿
                //if (propertyprovider.carton.DefinedQty == seq + 1)
                //{
                //    string U_UpdateContainerList = @"UPDATE SFCS_CONTAINER_LIST SET FULL_FLAG = 'Y', ATTRIBUTE1 = 'AUTO'
                //                                      WHERE DATA_TYPE = :DATA_TYPE
                //                                      AND CONTAINER_SN = :CONTAINER_SN
                //                                      AND SITE_ID = :SITE_ID ";
                //    repository.Execute(U_UpdateContainerList, new
                //    {
                //        DATA_TYPE = GlobalVariables.PallectLabel,
                //        CONTAINER_SN = pallectNo,
                //        SITE_ID = propertyprovider.sfcsOperationSites.ID
                //    }, transaction);
                //}
                //String U_UpadateContainerListSeq = @"UPDATE SFCS_CONTAINER_LIST SET SEQUENCE= :SEQUENCE+1 WHERE CONTAINER_SN=:CONTAINER_SN";
                //repository.Execute(U_UpadateContainerListSeq, new
                //{
                //    SEQUENCE = seq,
                //    CONTAINER_SN = pallectNo
                //}, transaction);
            }
            return pallectNo;
        }
    }
}
