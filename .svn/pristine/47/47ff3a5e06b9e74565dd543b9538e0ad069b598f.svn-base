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
    public class AutoCreateQcDoc : IMesSubModuleJob<SfcsRuncard, decimal>
    {
        public Task<KeyValuePair<bool, string>> GetTask(Propertyprovider propertyprovider, IBaseRepository<SfcsRuncard, decimal> repository, IDbTransaction transaction)
        {
            return Task.Run(() =>
            {
                try
                {
                    int opQty = repository.QueryEx<int>("SELECT COUNT(0) FROM SFCS_OPERATIONS WHERE ID = :ID AND OPERATION_CATEGORY = :OPERATION_CATEGORY ", new { ID = propertyprovider.sfcsOperationSites.OPERATION_ID, OPERATION_CATEGORY = GlobalVariables.QCOperation }).FirstOrDefault();//获取工序是过程检验的
                    //根据工序类型(过程检验)决定产生质检单号
                    if (propertyprovider.sfcsOperationSites != null && opQty > 0)
                    {
                        String qcDocNo = GetQcDocAsync(propertyprovider, repository, transaction);//产生质检单号
                        if (propertyprovider.spotCheck == null) { propertyprovider.spotCheck = new SpotCheck(); }
                        propertyprovider.spotCheck.qcDocNo = qcDocNo;
                        decimal qcQty = repository.QueryEx<decimal>("SELECT NVL(Q.SEQUENCE ,0) AS SEQUENCE FROM SFCS_CONTAINER_LIST Q WHERE Q.CONTAINER_SN=:CONTAINER_SN", new { CONTAINER_SN = qcDocNo }).FirstOrDefault();//获取质检数量
                        propertyprovider.spotCheck.qcQty = qcQty;
                    }
                    return new KeyValuePair<bool, string>(true, "");
                }
                catch (Exception ex)
                {

                    return new KeyValuePair<bool, string>(false, "AutoCreateQcDoc:" + ex.Message);
                }
            });
        }

        /// <summary>
        /// 獲取普通
        /// </summary>
        /// <returns></returns>
        public string GetQcDocAsync(Propertyprovider propertyprovider, IBaseRepository<SfcsRuncard, decimal> repository, IDbTransaction transaction)
        {
            decimal siteId = propertyprovider.sfcsOperationSites.ID;
            String containerSql = @"SELECT SCL.* FROM SFCS_CONTAINER_LIST SCL 
            WHERE PART_NO = :PART_NO AND DATA_TYPE = :DATA_TYPE AND SITE_ID = :SITE_ID AND FULL_FLAG = 'N'";
            SfcsContainerList sfcsContainerList = repository.QueryEx<SfcsContainerList>(
                containerSql,
                new
                {
                    PART_NO = propertyprovider.product.partNumber,
                    DATA_TYPE = GlobalVariables.QcLable,
                    SITE_ID = siteId
                }).FirstOrDefault();
            String qcQty = null;
            if (sfcsContainerList == null)
            {
                //使用SFCS_PACKING_CARTON_SEQ
                decimal sequence = repository.QueryEx<decimal>("SELECT SFCS_PACKING_CARTON_SEQ.NEXTVAL FROM DUAL ").FirstOrDefault();

                //將序列轉成36進制表示
                string result = Core.Utilities.RadixConvertPublic.RadixConvert(sequence.ToString(), ViewModels.GlobalVariables.DecRadix, ViewModels.GlobalVariables.Base36Redix);

                //六位表示
                string ReleasedSequence = result.PadLeft(6, '0');
                string yymmdd = repository.QueryEx<string>("SELECT TO_CHAR(SYSDATE,'YYMMDD') YYMMDD FROM DUAL ").FirstOrDefault();
                qcQty = "QC" + yymmdd + ReleasedSequence;//质检单号
                string I_InsertContainerList = @"INSERT INTO SFCS_CONTAINER_LIST (DATA_TYPE, CONTAINER_SN, PART_NO, SITE_ID, QUANTITY, FULL_FLAG, SEQUENCE)
                                                      VALUES (:DATA_TYPE, :CONTAINER_SN, :PART_NO, 
                                                      :SITE_ID, :QUANTITY, :FULL_FLAG, :SEQUENCE) ";
                repository.Execute(I_InsertContainerList, new
                {
                    DATA_TYPE = GlobalVariables.QcLable,
                    CONTAINER_SN = qcQty,
                    PART_NO = propertyprovider.product.partNumber,
                    SITE_ID = propertyprovider.sfcsOperationSites.ID,
                    QUANTITY = 1,
                    FULL_FLAG = "N",
                    SEQUENCE = 1
                }, transaction);
            }
            else
            {
                //扫一次SN  SEQUENCE就加1  点击“产生质检”按钮产生调接口实现 SEQUENCE+1 ,FULL_FLAG = 'Y'刷滿 结束本单质检后可开始下一单
                qcQty = sfcsContainerList.CONTAINER_SN;//质检单号
                String U_UpadateContainerListSeq = @"UPDATE SFCS_CONTAINER_LIST SET SEQUENCE = SEQUENCE+1 WHERE CONTAINER_SN=:CONTAINER_SN";
                repository.Execute(U_UpadateContainerListSeq, new
                {
                    CONTAINER_SN = qcQty
                }, transaction);
            }
            return qcQty;
        }
    }
}
