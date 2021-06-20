using JZ.IMS.Core;
using JZ.IMS.Core.Repository;
using JZ.IMS.Core.Utilities.Reflect;
using JZ.IMS.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Text;
using System.Threading.Tasks;
using System.Linq;

namespace JZ.IMS.Job.FinallyJobStorage
{
    public class BreakUid : IMesFinallyJob<SfcsRuncard, decimal>
    {
        public Task<KeyValuePair<bool, string>> GetTask(Propertyprovider propertyprovider, IBaseRepository<SfcsRuncard, decimal> repository, IDbTransaction transaction)
        {
            return Task.Run(() =>
            {
                try
                {
                    decimal operationId = (Decimal)propertyprovider.sfcsOperationSites.OPERATION_ID;
                    List<SfcsCollectUids> sfcsCollectUids = repository.QueryEx<SfcsCollectUids>("select * from SFCS_COLLECT_UIDS WHERE SN_ID = :SN_ID AND WO_ID = :WO_ID",
                        new
                        {
                            SN_ID = propertyprovider.sfcsRuncard.ID,
                            WO_ID = propertyprovider.sfcsRuncard.WO_ID
                        });
                    if (sfcsCollectUids != null)
                    {
                        foreach(SfcsCollectUids sfcsCollect in sfcsCollectUids)
                        {
                            String sql = @"select SPU.* from SFCS_COLLECT_UIDS SCU, SFCS_WO SW ,SFCS_PRODUCT_UIDS SPU ,SFCS_OPERATION_SITES SOS   
                            where SCU.WO_ID = SW.ID AND  SCU.UID_NUMBER = :UID_NUMBER AND SPU.PART_NO = SW.PART_NO AND SPU.UID_ID = SCU.UID_ID
                            AND SOS.ID = SCU.COLLECT_SITE AND SOS.OPERATION_ID = SPU.COLLECT_OPERATION_ID AND BREAK_OPERATION_ID = :BREAK_OPERATION_ID";
                            List<SfcsProductUids> sfcsProductUids = repository.QueryEx<SfcsProductUids>(sql, new
                            {
                                UID_NUMBER = sfcsCollect.UID_NUMBER,
                                BREAK_OPERATION_ID = operationId
                            });
                            if (sfcsProductUids != null && sfcsProductUids.Count > 0)
                            {
                                bool breakflag = false;

                                foreach (SfcsProductUids sfcsProduct in sfcsProductUids)
                                {
                                    if (Core.Utilities.FormatChecker.FormatCheck(sfcsCollect.UID_NUMBER, sfcsProduct.DATA_FORMAT))
                                    {
                                        breakflag = true;
                                        break;
                                    }
                                }
                                if (breakflag)
                                {
                                    string breaksql = @"INSERT INTO JZMES_LOG.SFCS_COLLECT_UIDS 
                                                  SELECT SCU.*,SYSDATE,:REWORK_OPERATION_ID,:REWORK_OPERATOR,:OPERATION_SITE
                                                  FROM SFCS_COLLECT_UIDS SCU WHERE SCU.COLLECT_UID_ID=:COLLECT_UID_ID";
                                    repository.Execute(breaksql, new
                                    {
                                        REWORK_OPERATION_ID = propertyprovider.sfcsOperationSites.OPERATION_ID,
                                        REWORK_OPERATOR = propertyprovider.sys_Manager.USER_NAME,
                                        OPERATION_SITE = propertyprovider.sfcsOperationSites.ID,
                                        COLLECT_UID_ID = sfcsCollect.COLLECT_UID_ID
                                    });
                                    string deletsql = @"DELETE FROM SFCS_COLLECT_UIDS  WHERE COLLECT_UID_ID=:COLLECT_UID_ID";
                                    repository.Execute(deletsql, new
                                    {
                                        COLLECT_UID_ID = sfcsCollect.COLLECT_UID_ID
                                    }
                                        );
                                }
                            }
                        }

                    }
                    return new KeyValuePair<bool, string>(true, "");
                }
                catch (Exception ex)
                {
                    return new KeyValuePair<bool, string>(false, "BreakUid:" + ex.Message);
                }
            });
        }
    }
}
