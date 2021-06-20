using JZ.IMS.Core;
using JZ.IMS.Core.Repository;
using JZ.IMS.Core.Utilities.Reflect;
using JZ.IMS.IRepository;
using JZ.IMS.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Text;
using System.Threading.Tasks;

namespace JZ.IMS.Job
{
    public class CheckRuncardStatus : IMesJob<SfcsRuncard, decimal>
    {
        /// <summary>
        /// 校验流水号状态
        /// </summary>
        /// <param name="propertyprovider"></param>
        /// <param name="repository"></param>
        /// <param name="transaction"></param>
        /// <returns></returns>
        public Task<KeyValuePair<bool, string>> GetTask(Propertyprovider propertyprovider, IBaseRepository<SfcsRuncard, decimal> repository, IDbTransaction transaction)
        {
            return Task.Run(() =>
            {

                try
                {
                    List<SfcsRuncard> sfcsRuncards = repository.QueryEx<SfcsRuncard>("SELECT  * FROM SFCS_RUNCARD WHERE SN = :SN", new
                    {
                        SN = propertyprovider.sfcsRuncard.SN
                    });

                    if (sfcsRuncards != null && sfcsRuncards.Count > 0)
                    {
                        SfcsRuncard sfcsRuncard = sfcsRuncards[0];
                        //Fail:2 ;RepairIn: 16
                        if (sfcsRuncard.STATUS == 2 || sfcsRuncard.STATUS == 16)
                        {
                            return new KeyValuePair<bool, string>(false, String.Format("流水号{0}已不良状态!", propertyprovider.sfcsRuncard.SN));
                        }
                        //Shipped:5
                        if (sfcsRuncard.STATUS == 5)
                        {
                            return new KeyValuePair<bool, string>(false, String.Format("流水号{0}已出货状态!不能直接返到线上,疑是重复SN，请联系QE确认是否重复SN。", propertyprovider.sfcsRuncard.SN));
                        }
                        //TurnIN:4
                        if (sfcsRuncard.STATUS == 4)
                        {
                            return new KeyValuePair<bool, string>(false, String.Format("流水号{0}已存仓状态!不能直接返到线上,疑是重复SN，请联系QE确认是否重复SN。", propertyprovider.sfcsRuncard.SN));
                        }
                        //WipScrapped:14
                        //Scrapped:7
                        //RepairScrapped:17
                        if (sfcsRuncard.STATUS == 14 || sfcsRuncard.STATUS == 7 || sfcsRuncard.STATUS == 17)
                        {
                            return new KeyValuePair<bool, string>(false, String.Format("流水号{0}已是报废状态,不能再刷!", propertyprovider.sfcsRuncard.SN));
                        }
                        //當為系統廠時，檢查當前SN工序是否需要刷Repair out
                        //Repaired:8
                        //pcbCode:2
                        //if (sfcsRuncard.STATUS == 8 || propertyprovider.sfcsOperationLines.PLANT_CODE == 2 )
                        //{
                        //}
                        //系統廠維修必須刷完Repair Out才可繼續作業
                        //if (PlantCode == GlobalVariables.pcCode)
                        //{
                        //    if (runcardRow.STATUS == GlobalVariables.Repaired ||
                        //        runcardRow.STATUS == GlobalVariables.RepairPICheck ||
                        //        runcardRow.STATUS == GlobalVariables.RepairOQACheck)
                        //    {
                        //        throw new MESException(Properties.Resource.Err_NoRepairOut, SerialNumber);
                        //    }
                        //}
                        //NeedClear:20
                        if (sfcsRuncard.STATUS == 20)
                        {
                            return new KeyValuePair<bool, string>(false, String.Format("流水号{0}维修时已经选择需要清洗PCB板，请清洗!", propertyprovider.sfcsRuncard.SN));

                        }
                    }
                    return new KeyValuePair<bool, string>(true, "");
                }
                catch (Exception ex)
                {

                    return new KeyValuePair<bool, string>(false, "CheckRuncardStatus:" + ex.Message);
                }
            });
        }
    }
}
