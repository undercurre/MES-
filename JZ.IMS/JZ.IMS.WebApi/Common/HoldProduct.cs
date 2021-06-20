using JZ.IMS.IRepository;
using JZ.IMS.Models;
using JZ.IMS.ViewModels;
using JZ.IMS.WebApi.Controllers;
using Microsoft.Extensions.Localization;
using MySqlX.XDevAPI.Common;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using static JZ.IMS.WebApi.Controllers.SfcsLockProductController;

namespace JZ.IMS.WebApi.Common
{
    /// <summary>
    /// 管控動作
    /// </summary>
    public enum HoldOperation
    {
        HoldWIP,
        HoldRework,
        HoldRMA,
        HoldAssemply,
        HoldShip,
        HoldTurnIn,
        HoldRepair
    }
    /// <summary>
    /// 輔助條件
    /// </summary>
    public enum HoldProductSubsidiaryCondition
    {
        TurnInTime,
        InputTime,
        LastBFTTime,
        Invertory,
        NoSubsidiaryCondition
    }
    public class HoldProduct
    {

        public string snFilePath = string.Empty;
      

  

        /// <summary>
        /// 主條件
        /// </summary>
        public enum HoldProductMainCondition
        {
            HoldBySerialNumber,//單筆/批量產品序號
            HoldByCustomSerialNumber,//料號與自定義產品序號
            HoldByCartonOrPallet,//卡通/棧板
            HoldByWorkOrderOrPartNumberOrModel,//工單/料號/機種
            HoldByComponentCustomerPartNumber,//零件客戶料號
            HoldByComponentSerialNumber,//單筆/批量零件序號
            HoldByCustomComponentSerialNumber,//料號與自定義零件序號
            HoldSiteByPartNumberOrWorkOrder,//料號/工單與站點
            HoldSite,//站點
            HoldWipOperationBySerialNumber,//產品序號與工序
            HoldWIPOperationByPartNumberAndOperationLine = 21,
            HoldWIPOperationByPartNumber = 22
        }


        /// <summary>
        /// 執行約束
        /// </summary>
        /// <param name="runcardDataTable"></param>
        public List<SfcsRuncard> ExecuteRestraint(ISfcsLockProductHeaderRepository repository, int subsidiaryConditionSelectIndex, int actionSelectIndex, List<SfcsRuncard> runcardTable, DateTime? beginTime, DateTime endTime)
        {
            //turnin time
            if (subsidiaryConditionSelectIndex == (int)HoldProductSubsidiaryCondition.TurnInTime)
            {
                return RestraintTurninTime(runcardTable, beginTime, endTime);
            }

            //input time
            if (subsidiaryConditionSelectIndex == (int)HoldProductSubsidiaryCondition.InputTime)
            {
                return RestraintInputTime(runcardTable, beginTime, endTime);
            }

            //last bft time
            if (subsidiaryConditionSelectIndex == (int)HoldProductSubsidiaryCondition.LastBFTTime)
            {
                return RestraintLastBftTime(repository, runcardTable, beginTime, endTime);
            }

            //inventory
            if (subsidiaryConditionSelectIndex == (int)HoldProductSubsidiaryCondition.Invertory)
            {
                return RestraintInventory(repository, runcardTable, beginTime, endTime);
            }

            //Shipped SN
            return RestraintShippedSN(runcardTable, actionSelectIndex);
        }

        /// <summary>
        /// 約束存倉時間
        /// </summary>
        /// <param name="runcardDataTable"></param>
        public List<SfcsRuncard> RestraintTurninTime(List<SfcsRuncard> runcardTable, DateTime? beginTime, DateTime? endTime)
        {
            for (int i = runcardTable.Count - 1; i >= 0; i--)
            {
                if (runcardTable[i].TURNIN_TIME == null ||
               runcardTable[i].STATUS == GlobalVariables.Shipped)
                {
                    runcardTable.Remove(runcardTable[i]);
                }
                else
                {
                    if (runcardTable[i].TURNIN_TIME < beginTime ||
                        runcardTable[i].TURNIN_TIME > endTime)
                    {
                        runcardTable.Remove(runcardTable[i]);
                    }
                }
            }
            return runcardTable;
        }

        /// <summary>
        /// 約束投產時間
        /// </summary>
        /// <param name="runcardDataTable"></param>
        public List<SfcsRuncard> RestraintInputTime(List<SfcsRuncard> runcardTable, DateTime? beginTime, DateTime endTime)
        {


            for (int i = runcardTable.Count - 1; i >= 0; i--)
            {
                if (runcardTable[i].INPUT_TIME != null)
                {
                    if (runcardTable[i].INPUT_TIME < beginTime ||
                        runcardTable[i].INPUT_TIME > endTime ||
                        runcardTable[i].STATUS == GlobalVariables.Shipped)
                    {
                        runcardTable.Remove(runcardTable[i]);
                    }
                }
            }
            return runcardTable;
        }

        /// <summary>
        /// 約束最後過bft時間
        /// </summary>
        /// <param name="runcardDataTable"></param>
        private List<SfcsRuncard> RestraintLastBftTime(ISfcsLockProductHeaderRepository repository, List<SfcsRuncard> runcardTable, DateTime? beginTime, DateTime endTime)
        {
            for (int i = runcardTable.Count - 1; i >= 0; i--)
            {
                if (runcardTable[i].STATUS == GlobalVariables.Shipped)
                {
                    runcardTable.Remove(runcardTable[i]);
                }
                else
                {

                    List<SfcsOperationHistory> bftHistoryTable = repository.GetLastBFTHistory(runcardTable[i].ID);

                    if (bftHistoryTable.Count > 0 && bftHistoryTable != null)
                    {
                        SfcsOperationHistory bftHistoryrow =
                            bftHistoryTable.FirstOrDefault();
                        if (bftHistoryrow.OPERATION_TIME < beginTime ||
                            bftHistoryrow.OPERATION_TIME > endTime)
                        {
                            runcardTable.Remove(runcardTable[i]);
                        }
                    }
                    else
                    {
                        runcardTable.Remove(runcardTable[i]);
                    }
                }
            }
            return runcardTable;
        }

        /// <summary>
        /// 約束庫別
        /// </summary>
        /// <param name="runcardDataTable"></param>
        private List<SfcsRuncard> RestraintInventory(ISfcsLockProductHeaderRepository repository, List<SfcsRuncard> runcardTable, DateTime? beginTime, DateTime endTime)
        {
            for (int i = runcardTable.Count - 1; i >= 0; i--)
            {
                if (runcardTable[i].TURNIN_NO == null ||
                    runcardTable[i].STATUS == GlobalVariables.Shipped)
                {
                    runcardTable.Remove(runcardTable[i]);
                }
                else
                {
                    List<SfcsTurninBatchHeader> bftHistoryTable = repository.GetBatchHeaderDataTable(runcardTable[i].TURNIN_NO);
                    if (bftHistoryTable != null && bftHistoryTable.Count == 0)
                    {
                        runcardTable.Remove(runcardTable[i]);
                    }
                }
            }
            return runcardTable;
        }

        /// <summary>
        /// 排除已出貨SN
        /// </summary>
        private List<SfcsRuncard> RestraintShippedSN(List<SfcsRuncard> runcardTable, int actionSelectIndex)
        {
            //如果鎖定為不能出貨,則將已出貨的流水號去掉
            if (actionSelectIndex == (int)HoldOperation.HoldShip)
            {
                for (int i = runcardTable.Count - 1; i >= 0; i--)
                {
                    if (runcardTable[i].STATUS == GlobalVariables.Shipped)
                    {
                        runcardTable.Remove(runcardTable[i]);
                    }
                }
            }
            return runcardTable;
        }

        public  async Task<List<SfcsRuncard>> IdentifyRuncard(ISfcsLockProductHeaderRepository repository, IStringLocalizer<SfcsUnLockProductController> localizer, List<SfcsRuncard> runcards, string data)
        {
            runcards = await repository.GetRuncardDataTable(data);

            if (runcards.Count > 0)
            {
               // serialNumber = snvalue;
            }
            else if (System.IO.File.Exists(data))
            {
                System.Collections.ArrayList list = FilePublic.GetSimpleFileContent(data);
                for (int i = 0; i < list.Count; i++)
                {
                    string sn = list[i].ToString().Trim();
                    //序號為空，跳過
                    if (string.IsNullOrEmpty(sn))
                    {
                        continue;
                    }
                    bool matchExist = false;
                    foreach (var row in runcards)
                    {
                        if (row.SN == sn)
                        {
                            matchExist = true;
                            break;
                        }
                    }

                    //序號為空，跳過
                    if (matchExist)
                    {
                        continue;
                    }

                    if ((await repository.GetRuncardDataTable(sn)).Count == 0)
                    {

                        throw new Exception(string.Format(localizer["Err_SerialNumberNotExist"], sn));
                    }
                    else
                    {
                        runcards.Union(await repository.GetRuncardDataTable(sn));
                    }
                }
                 snFilePath = data;
            }
            else
            {
                throw new Exception(localizer["Err_UnKnow"]);
            }

            return runcards;

        }

        

     

    }
}
