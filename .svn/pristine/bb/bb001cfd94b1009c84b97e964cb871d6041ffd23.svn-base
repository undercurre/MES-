/*
*┌──────────────────────────────────────────────────────────────┐
*│　描    述：接口实现                                                    
*│　作    者：嘉志科技                                            
*│　版    本：2.0    模板代码自动生成                                                
*│　创建时间：2020-09-14 11:46:24                             
*└──────────────────────────────────────────────────────────────┘
*┌──────────────────────────────────────────────────────────────┐
*│　命名空间： JZ.IMS.Repository.Oracle                                  
*│　类    名： SfcsReworkRepository                                      
*└──────────────────────────────────────────────────────────────┘
*/
using JZ.IMS.Core.DbHelper;
using JZ.IMS.Core.Options;
using JZ.IMS.Core.Repository;
using JZ.IMS.IRepository;
using JZ.IMS.Models;
using JZ.IMS.ViewModels;
using Dapper;
using Microsoft.Extensions.Options;
using System;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Linq;
using JZ.IMS.Core.Extensions;
using JZ.IMS.Core.Utilities;
using Org.BouncyCastle.Asn1.Utilities;
using System.Data;

namespace JZ.IMS.Repository.Oracle
{
    public class SfcsReworkRepository : BaseRepository<SfcsRework, Decimal>, ISfcsReworkRepository
    {
        private static List<decimal> wipOperationList = new List<decimal>();
        private static List<SfcsWoListModel> newWorkOrderRow = new List<SfcsWoListModel>();

        public SfcsReworkRepository(IOptionsSnapshot<DbOption> options)
        {
            _dbOption = options.Get("iWMS");
            if (_dbOption == null)
            {
                throw new ArgumentNullException(nameof(DbOption));
            }
            _dbConnection = ConnectionFactory.CreateConnection(_dbOption.DbType, _dbOption.ConnectionString);
        }

        /// <summary>
        /// 根据主键获取激活状态
        /// </summary>
        /// <param name="id">主键</param>
        /// <returns></returns>
		public async Task<Boolean> GetEnableStatus(decimal id)
        {
            string sql = "SELECT ENABLED FROM SFCS_REWORK WHERE ID=:ID";
            var result = await _dbConnection.QueryFirstOrDefaultAsync<string>(sql, new
            {
                ID = id,
            });

            return result == "Y" ? true : false;
        }

        /// <summary>
        /// 修改激活状态
        /// </summary>
        /// <param name="id">主键</param>
        /// <param name="status">更改后的状态</param>
        /// <returns></returns>
		public async Task<decimal> ChangeEnableStatus(decimal id, bool status)
        {
            string sql = "UPDATE SFCS_REWORK set ENABLED=:ENABLED WHERE ID=:Id";
            return await _dbConnection.ExecuteAsync(sql, new
            {
                ENABLED = status ? 'Y' : 'N',
                Id = id,
            });
        }

        /// <summary>
        /// 获取表的序列
        /// </summary>
        /// <returns></returns>
		public async Task<decimal> GetSEQID()
        {
            string sql = "SELECT SFCS_REWORK_SEQ.NEXTVAL MY_SEQ FROM DUAL";
            var result = await _dbConnection.ExecuteScalarAsync(sql);
            return (decimal)result;
        }

        /// <summary>
        /// 获取表的序列
        /// </summary>
        /// <returns></returns>
        public async Task<decimal> GetCollectDefectSEQID()
        {
            string sql = "SELECT SFCS_OPERATION_SEQ.NEXTVAL MY_SEQ FROM DUAL";
            var result = await _dbConnection.ExecuteScalarAsync(sql);
            return (decimal)result;
        }
        /// <summary>
        /// 获取最大的VISIT_NUMBER
        /// </summary>
        /// <param name="snID"></param>
        /// <returns></returns>
        private async Task<decimal> GetVisitNumber(decimal snID)
        {
            try
            {
                string sql = @"SELECT MAX(VISIT_NUMBER) FROM SFCS_OPERATION_HISTORY WHERE SN_ID=:SN_ID";
                var result = await _dbConnection.ExecuteScalarAsync(sql, new { SN_ID = snID });
                return (decimal)result;
            }
            catch (Exception ex)
            {
                throw;
            }
        }


        /// <summary>
        /// get max visit number
        /// </summary>
        /// <param name="snID"></param>
        /// <returns></returns>
        private async Task<decimal> GetMaxOpertionHistoryVisitNumber(decimal snID)
        {
            try
            {
                string sql = @"SELECT MAX(VISIT_NUMBER) FROM SFCS_OPERATION_HISTORY WHERE SN_ID=:SN_ID";
                var result = await _dbConnection.ExecuteScalarAsync(sql, new { SN_ID = snID });
                return (decimal)result;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }



        /// <summary>
		///项目是否已被使用 
		/// </summary>
		/// <param name="id">项目id</param>
		/// <returns></returns>
		public async Task<bool> ItemIsByUsed(decimal id)
        {
            string sql = "select count(0) from SFCS_REWORK where id = :id";
            object result = await _dbConnection.ExecuteScalarAsync(sql, new
            {
                id
            });

            return (Convert.ToInt32(result) > 0);
        }




        /// <summary>
        /// 查询新工单数据
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public async Task<TableDataModel> GetNewWorkNoData(SfcsWoNeWorkRequestModel model)
        {
            try
            {
                string conditions = "WHERE a.ID > 0 ";

                if (!model.Key.IsNullOrWhiteSpace())
                {
                    conditions += $"and (instr(a.WO_NO, :Key) > 0 or instr(a.PART_NO, :Key) > 0 )";
                }
                string sql = "SELECT ROWNUM as rowno, a.ID,a.WO_NO,a.PART_NO,a.INPUT_QTY,a.OUTPUT_QTY  FROM SFCS_WO  a  ";
                string pagedSql = SQLBuilderClass.GetPagedSQL(sql, " a.ID desc", conditions);
                var resdata = await _dbConnection.QueryAsync<object>(pagedSql, model);
                string sqlcnt = @"select count(0) from SFCS_WO  a " + conditions;
                int cnt = await _dbConnection.ExecuteScalarAsync<int>(sqlcnt, model);
                return new TableDataModel
                {
                    count = cnt,
                    data = resdata?.ToList(),
                };
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }

        /// <summary>
        /// 根据流水号查出返工作业数据信息
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public async Task<SfcsReworkListModel> GetReworkDataBySN(SfcsReworkRequestModel model)
        {
            try
            {
                SfcsReworkListModel reworklist = new SfcsReworkListModel();
                reworklist.SNLIST = new List<string>();
                reworklist.CURRENTOPERATIONLIST = new List<SortedList<string, decimal>>();
                // reworklist.OLDCURRENTOPERATIONLIST = new SortedList<string, decimal>();
                reworklist.ORIGINALORDERNOLIST = new List<decimal>();
                reworklist.ORDERNOLIST = new List<decimal>();
                reworklist.SN = model.SN;
                if (model.RETYPE > 0)
                {
                }
                //辨識輸入數據
                var runcardTable = await IdentifyDataInput(model, reworklist);

                SetOldRouteControlVisible(model, reworklist);

                if (model.RETYPE == 0)
                {
                    //焦點定位到當前作業工序
                    if (!runcardTable.IsNull() && runcardTable.Count > 0)
                    {
                        var row = runcardTable[0];

                        reworklist.CHOOSEINDEX = wipOperationList.IndexOf((decimal)row.WIP_OPERATION);
                        //if (reworklist.ORDERNOLIST.Count > 0)
                        //{
                        //    reworklist.CHOOSEOPERATIONORDER = reworklist.ORDERNOLIST[(int)reworklist.CHOOSEINDEX];
                        //}
                    }
                }
                else
                {
                    if (reworklist.CURRENTOPERATIONLIST.Count > 0)
                    {
                        for (int i = 0; i < reworklist.CURRENTOPERATIONLIST.Count; i++)
                        {
                            if (reworklist.CURRENTOPERATIONLIST[i].Values[0].ToString() == model.CHOOSEINDEXVALUE.ToString())
                            {
                                reworklist.CHOOSEINDEX = i;
                            }
                        }
                    }
                }

                // 顯示操作歷史記錄
                ShowOperationHistoryMessage(reworklist, model.SN);

                return reworklist;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        /// <summary>
        /// 根据新工单号查询数据
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public async Task<SfcsReworkListModel> GetNewReworkDataByNewNo(SfcsReworkRequestModel model)
        {
            try
            {
                SfcsReworkListModel newworklist = new SfcsReworkListModel();
                // newworklist.OLDCURRENTOPERATIONLIST = new SortedList<string, decimal>();
                newworklist.ORDERNOLIST = new List<decimal>();
                newworklist.CURRENTOPERATIONLIST = new List<SortedList<string, decimal>>();
                FindRouteConfig(model, newworklist);
                FindRuncardFormat(newworklist);
                ShowOperationHistoryMessage(newworklist, model.NEW_WORKNO);

                return newworklist;

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }



        /// <summary>
        /// 保存数据
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public async Task<decimal> SaveDataByTrans(SfcsReworkModel model)
        {
            int result = 1;

            ConnectionFactory.OpenConnection(_dbConnection);
            using (var tran = _dbConnection.BeginTransaction())
            {
                try
                {
                    if (model.SNLIST.IsNull() || model.SNLIST.Count == 0)
                    {
                        throw new Exception("请输入有效的数据");
                    }
                    if (model.SaveRecords != null && model.SaveRecords.Count > 0)
                    {
                        foreach (var item in model.SaveRecords)
                        {
                            item.CHOOSEOPERATIONORDER = item.ORDERNOLIST[(int)item.CHOOSEINDEX];
                            if (item.OLDCHOOSEINDEX > 0 && item.ORIGINALORDERNOLIST.Count > 0)
                            {
                                item.OLDCHOOSEOPERATIONORDER = item.ORIGINALORDERNOLIST[(int)item.OLDCHOOSEINDEX];
                            }

                            //提交前重新获取最新Runcard Table數據
                            var runcardTable = await GetTempRuncardTable(item.SN);

                            //檢查runcard返工規則
                            var reusltData = (await CheckRuncardRule(runcardTable, item));
                            if (!reusltData)
                            {
                                return 0;
                            }
                            //檢查該返工類型業務規則
                            reusltData=(await CheckReworkRule(runcardTable, item, model.SNLIST.Count));
                            if (!reusltData)
                            {
                                return 0;
                            }
                            //获取刪除對象
                            var collectObjectTable = await FindCollectObjectsInOperation(runcardTable, item);

                            decimal reworkOperationID = await GetCollectDefectSEQID();
                            //清除對象數據
                            ProcessObjectDatas(collectObjectTable, runcardTable, item, reworkOperationID);
                            //返工的製程發生變化時，將未清除的對象修改product operation code
                            UpdateProductOperationCode(runcardTable, item);
                            //清除ak(勾选框被注释，所以可不用执行这步操作)

                            //修改流水號
                            UpdateRuncard(runcardTable, item, reworkOperationID);

                            //記錄作業歷史記錄
                            RecordOperationHistory(runcardTable, item, reworkOperationID);
                            if (item.RETYPE != 0)
                            {
                                //// 記錄工單替換歷史記錄
                                RecordWoReplaceHistory(runcardTable, item, reworkOperationID);
                                // 更新工單實際投入時間
                                UpdateWoActualStartDate(item);
                                // 更新版本信息
                                UpdateRuncardRevision(runcardTable, item, reworkOperationID);
                            }
                        }
                    }
                    tran.Commit();
                }
                catch (Exception ex)
                {
                    result = -1;
                    tran.Rollback();
                    throw ex;
                }
                finally
                {
                    if (_dbConnection.State != System.Data.ConnectionState.Closed)
                    {
                        _dbConnection.Close();
                    }
                }
            }
            return result;
        }

        /// <summary>
        /// 保存数据(分布式事务)
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public async Task<decimal> SaveDataByTrans(SfcsReworkModel model, IDbTransaction tran,IDbConnection dbConnection)
        {
            int result = 1;
            try
            {
                if (model.SNLIST.IsNull() || model.SNLIST.Count == 0)
                {
                    throw new Exception("请输入有效的数据");
                }
                if (model.SaveRecords != null && model.SaveRecords.Count > 0)
                {
                    foreach (var item in model.SaveRecords)
                    {
                        item.CHOOSEOPERATIONORDER = item.ORDERNOLIST[(int)item.CHOOSEINDEX];
                        if (item.OLDCHOOSEINDEX > 0 && item.ORIGINALORDERNOLIST.Count > 0)
                        {
                            item.OLDCHOOSEOPERATIONORDER = item.ORIGINALORDERNOLIST[(int)item.OLDCHOOSEINDEX];
                        }

                        //提交前重新获取最新Runcard Table數據
                        var runcardTable = await GetTempRuncardTable(item.SN);

                        //檢查runcard返工規則
                        if (!await CheckRuncardRule(runcardTable, item))
                        {
                            return 0;
                        }
                        //檢查該返工類型業務規則
                        if (!await CheckReworkRule(runcardTable, item, model.SNLIST.Count))
                        {
                            return 0;
                        }
                        //获取刪除對象
                        var collectObjectTable = await FindCollectObjectsInOperation(runcardTable, item);

                        decimal reworkOperationID = await GetCollectDefectSEQID();
                        //清除對象數據
                        ProcessObjectDatas(collectObjectTable, runcardTable, item, reworkOperationID);
                        //返工的製程發生變化時，將未清除的對象修改product operation code
                        UpdateProductOperationCode(runcardTable, item);
                        //清除ak(勾选框被注释，所以可不用执行这步操作)

                        //修改流水號
                        UpdateRuncard(runcardTable, item, reworkOperationID);

                        //記錄作業歷史記錄
                        RecordOperationHistory(runcardTable, item, reworkOperationID);
                        if (item.RETYPE != 0)
                        {
                            //// 記錄工單替換歷史記錄
                            RecordWoReplaceHistory(runcardTable, item, reworkOperationID);
                            // 更新工單實際投入時間
                            UpdateWoActualStartDate(item);
                            // 更新版本信息
                            UpdateRuncardRevision(runcardTable, item, reworkOperationID);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                result = -1;
                tran.Rollback();
                throw ex;
            }

            return result;
        }

        /// <summary>
        /// 記錄工單替換歷史作業
        /// </summary>
        private async void RecordWoReplaceHistory(List<SfcsRuncardListModel> runlist, SfcsReworkAddOrModifyModel remodel, decimal reworkOperationID)
        {
            var NewWorkOrderRows = await GetWorkOrderTable(0, remodel.NEW_WORKNO);
            foreach (var row in runlist)
            {
                if (!NewWorkOrderRows.IsNull() && NewWorkOrderRows.Count > 0)
                {
                    InsertWorkOrderReplaceHistory(reworkOperationID, row.ID, (decimal)row.WO_ID, NewWorkOrderRows[0].ID, NewWorkOrderRows[0].PLANT_CODE, 2, (decimal)row.CURRENT_SITE, remodel.REPAIRER);
                }
            }
        }

        /// <summary>
        /// 更新工單實際投產日期
        /// </summary>
        private async void UpdateWoActualStartDate(SfcsReworkAddOrModifyModel remodel)
        {
            if (!remodel.NEW_WORKNO.IsNullOrEmpty())
            {
                var workOrderRow = await GetWorkOrderTable(0, remodel.NEW_WORKNO);

                if (!workOrderRow.IsNullOrEmpty() && workOrderRow.Count > 0 && workOrderRow[0].ACTUAL_START_DATE.IsNullOrEmpty())
                {
                    UpdateWorkOrderStartDate(workOrderRow[0].ID);
                }

            }

        }

        /// <summary>
        /// 更新Runcard版本信息
        /// 1新工單沒有原有SN的版本，保留原有的版本信息
        /// 2.新工單和SN均有的版本，更新SN的版本(以工單版本作為最新版本)
        /// 3.新工單有而SN沒有的版本，壓入新工單的版本
        /// </summary>
        private async void UpdateRuncardRevision(List<SfcsRuncardListModel> runlist, SfcsReworkAddOrModifyModel remodel, decimal reworkOperationID)
        {
            var NewWorkOrderRows = await GetWorkOrderTable(0, remodel.NEW_WORKNO);
            if (!NewWorkOrderRows.IsNull() && NewWorkOrderRows.Count > 0)
            {
                var ruleConfigRow = await GetRuleConfig(414055, NewWorkOrderRows[0].PART_NO, GlobalVariables.EnableY);
                if (!ruleConfigRow.IsNullOrEmpty() && ruleConfigRow.Count > 0)
                {
                    var workOrderRevisionRow = await GetWoRevisionDataTable(remodel.NEW_WORKNO, GlobalVariables.EnableY);
                    if (!workOrderRevisionRow.IsNullOrEmpty() && workOrderRevisionRow.Count > 0)
                    {
                        foreach (var runcardRow in runlist)
                        {
                            UpdateRuncardRevision(workOrderRevisionRow[0].ODM_VERSION,
                                             workOrderRevisionRow[0].RMA_VERSION,
                                             workOrderRevisionRow[0].BIOS_VERSION,
                                             workOrderRevisionRow[0].CUSTOMER_VERSION,
                                             workOrderRevisionRow[0].SHIPPING_VERSION,
                                             workOrderRevisionRow[0].PCBA_VERSION,
                                             workOrderRevisionRow[0].FG_VERSION,
                                             workOrderRevisionRow[0].BMC_VERSION,
                                             reworkOperationID, runcardRow.ID, runcardRow.SN);
                        }
                    }
                }
            }
        }

        /// <summary>
        /// 選擇性更新Rucard版本信息
        /// 傳入的版本信息為Null時不Update
        /// </summary>
        private async void UpdateRuncardRevision(
            string odmRevision, string rmaRevision, string biosRevision,
            string customerRevision, string shippingRevision,
            string pcbaRevision, string fgRevision, string bmcRevision,
            decimal replaceOperationID, decimal serialNumberID, string serialNumber)
        {
            string sqlstr = null;
            var runcardRevisionRow = await GetRuncardRevision(serialNumberID);
            if (runcardRevisionRow.IsNullOrEmpty() || runcardRevisionRow.Count <= 0)
            {
                return;
            }
            // OEM REV
            if (!odmRevision.IsNullOrEmpty())
            {
                RecordRevisionReplaceHistory(replaceOperationID, serialNumberID, serialNumber, runcardRevisionRow[0].ODM_VERSION, odmRevision, 1, "SFCS Auto Update", "SFCS");
                sqlstr = sqlstr.IsNullOrEmpty() ? string.Format(" ODM_VERSION = '{0}' ", odmRevision) :
                   sqlstr + GlobalVariables.comma + string.Format(" ODM_VERSION = '{0}' ", odmRevision);
            }
            // RMA REV
            if (!rmaRevision.IsNullOrEmpty())
            {
                RecordRevisionReplaceHistory(replaceOperationID, serialNumberID, serialNumber, runcardRevisionRow[0].RMA_VERSION, rmaRevision, 2, "SFCS Auto Update", "SFCS");
                sqlstr = sqlstr.IsNullOrEmpty() ? string.Format(" RMA_VERSION = '{0}' ", rmaRevision) :
                   sqlstr + GlobalVariables.comma + string.Format(" RMA_VERSION = '{0}' ", rmaRevision);
            }
            // BIOS REV
            if (!biosRevision.IsNullOrEmpty())
            {
                RecordRevisionReplaceHistory(replaceOperationID, serialNumberID, serialNumber, runcardRevisionRow[0].BIOS_VERSION, biosRevision, 3, "SFCS Auto Update", "SFCS");
                sqlstr = sqlstr.IsNullOrEmpty() ? string.Format(" BIOS_VERSION =  '{0}' ", biosRevision) :
                   sqlstr + GlobalVariables.comma + string.Format(" BIOS_VERSION = '{0}' ", biosRevision);
            }
            // CUSTOMER REV
            if (!customerRevision.IsNullOrEmpty())
            {
                RecordRevisionReplaceHistory(replaceOperationID, serialNumberID, serialNumber, runcardRevisionRow[0].CUSTOMER_VERSION, customerRevision, 4, "SFCS Auto Update", "SFCS");
                sqlstr = sqlstr.IsNullOrEmpty() ? string.Format(" CUSTOMER_VERSION =  '{0}' ", customerRevision) :
                   sqlstr + GlobalVariables.comma + string.Format(" CUSTOMER_VERSION =  '{0}' ", customerRevision);
            }
            // SHIPPING REV
            if (!shippingRevision.IsNullOrEmpty())
            {
                RecordRevisionReplaceHistory(replaceOperationID, serialNumberID, serialNumber, runcardRevisionRow[0].SHIPPING_VERSION, shippingRevision, 5, "SFCS Auto Update", "SFCS");
                sqlstr = sqlstr.IsNullOrEmpty() ? string.Format(" SHIPPING_VERSION =  '{0}' ", shippingRevision) :
                   sqlstr + GlobalVariables.comma + string.Format(" SHIPPING_VERSION =  '{0}' ", shippingRevision);
            }
            // PCBA REV
            if (!pcbaRevision.IsNullOrEmpty())
            {
                RecordRevisionReplaceHistory(replaceOperationID, serialNumberID, serialNumber, runcardRevisionRow[0].PCBA_VERSION, pcbaRevision, 6, "SFCS Auto Update", "SFCS");
                sqlstr = sqlstr.IsNullOrEmpty() ? string.Format(" PCBA_VERSION = '{0}' ", pcbaRevision) :
                   sqlstr + GlobalVariables.comma + string.Format(" PCBA_VERSION =  '{0}' ", pcbaRevision);
            }
            // FG REV
            if (!fgRevision.IsNullOrEmpty())
            {
                RecordRevisionReplaceHistory(replaceOperationID, serialNumberID, serialNumber, runcardRevisionRow[0].FG_VERSION, fgRevision, 7, "SFCS Auto Update", "SFCS");
                sqlstr = sqlstr.IsNullOrEmpty() ? string.Format(" FG_VERSION =  '{0}' ", fgRevision) :
                   sqlstr + GlobalVariables.comma + string.Format(" FG_VERSION =  '{0}' ", fgRevision);
            }
            // BMC REV
            if (!bmcRevision.IsNullOrEmpty())
            {
                RecordRevisionReplaceHistory(replaceOperationID, serialNumberID, serialNumber, runcardRevisionRow[0].BMC_VERSION, bmcRevision, 8, "SFCS Auto Update", "SFCS");
                sqlstr = sqlstr.IsNullOrEmpty() ? string.Format(" BMC_VERSION =  '{0}' ", bmcRevision) :
                   sqlstr + GlobalVariables.comma + string.Format(" BMC_VERSION =  '{0}' ", bmcRevision);
            }
            string sql = " UPDATE SFCS_RUNCARD_REVISIONS SET  " + sqlstr + string.Format(" WHERE SN_ID = {0}", serialNumberID);
            await _dbConnection.ExecuteAsync(sql);
        }

        /// <summary>
        /// 獲取流水號的版本
        /// </summary>
        /// <param name="SNID"></param>
        /// <returns></returns>
        private async Task<List<SfcsRuncardRevisionsListModel>> GetRuncardRevision(decimal SN_ID)
        {
            try
            {
                string sql = @"select * from SFCS_RUNCARD_REVISIONS where SN_ID=:SN_ID ";
                var list = await _dbConnection.QueryAsync<SfcsRuncardRevisionsListModel>(sql, new { SN_ID });
                return list.ToList();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }



        /// <summary>
        /// 保存版本替換歷史信息
        /// </summary>
        /// <param name="replaceRevisionID"></param>
        /// <param name="replaceOperationID"></param>
        /// <param name="snID"></param>
        /// <param name="sn"></param>
        /// <param name="oldRevision"></param>
        /// <param name="newRevision"></param>
        /// <param name="recisionType"></param>
        /// <param name="revisionReason"></param>
        /// <param name="replaceBy"></param>
        private async void RecordRevisionReplaceHistory(decimal replaceOperationID, decimal snID, string sn,
            string oldRevision, string newRevision, decimal revisionType, string revisionReason, string replaceBy)
        {
            try
            {
                string sql = @"INSERT INTO SFCS_REVISION_REPLACE
                                                                     (REPLACE_REVISION_ID, REPLACE_OPERATION_ID, SN_ID, SN,
                                                                      OLD_REVISION, NEW_REVISION, REVISION_TYPE, REPLACE_REASON,
                                                                      REPLACE_BY
                                                                     )
                                                              VALUES (SFCS_REVISION_REPLACE_SEQ.NEXTVAL, :REPLACE_OPERATION_ID, :SN_ID, :SN,
                                                                      :OLD_REVISION, :NEW_REVISION, :REVISION_TYPE, :REPLACE_REASON,
                                                                      :REPLACE_BY
                                                                     ) ";
                await _dbConnection.ExecuteAsync(sql, new
                {
                    REPLACE_OPERATION_ID = replaceOperationID,
                    SN_ID = snID,
                    SN = sn,
                    OLD_REVISION = oldRevision,
                    NEW_REVISION = newRevision,
                    REVISION_TYPE = revisionType,
                    REPLACE_REASON = revisionReason,
                    REPLACE_BY = replaceBy
                });

            }
            catch (Exception ex)
            {
                throw ex;
            }

        }

        /// <summary>
        /// 獲取開關業務數據
        /// </summary>
        /// <returns></returns>
        private async Task<List<SfcsRuleConfigListModel>> GetRuleConfig(decimal RULE_ID, string PART_NO, string Enabled)
        {
            try
            {
                string sql = @"select * from SFCS_RULE_CONFIG where RULE_ID=:RULE_ID and PART_NO=:PART_NO and Enabled=:Enabled";
                var list = await _dbConnection.QueryAsync<SfcsRuleConfigListModel>(sql, new { RULE_ID, PART_NO, Enabled });
                return list.ToList();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// 獲取版本
        /// </summary>
        /// <param name="WO_NO"></param>
        /// <param name="Enabled"></param>
        /// <returns></returns>
        private async Task<List<SfcsWorkorderRevisionsListModel>> GetWoRevisionDataTable(string WO_NO, string Enabled)
        {
            try
            {
                string sql = @"select * from SFCS_WORKORDER_REVISIONS  where WO_NO=:WO_NO  and Enabled=:Enabled";
                var list = await _dbConnection.QueryAsync<SfcsWorkorderRevisionsListModel>(sql, new { WO_NO, Enabled });
                return list.ToList();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        /// <summary>
        /// 更新工單開始日期
        /// </summary>
        /// <param name="workOrderID"></param>
        private async void UpdateWorkOrderStartDate(decimal workOrderID)
        {
            try
            {
                string sql = @"UPDATE SFCS_WO SET ACTUAL_START_DATE=SYSDATE WHERE ID=:ID";
                await _dbConnection.ExecuteAsync(sql, new { ID = workOrderID });
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// 記錄工單替換歷史記錄(SFCS_WO_REPLACE表不存在)
        /// </summary>
        /// <param name="replaceOperationID"></param>
        /// <param name="snID"></param>
        /// <param name="oldWoID"></param>
        /// <param name="newWoID"></param>
        /// <param name="plantCode"></param>
        /// <param name="replaceType"></param>
        /// <param name="replaceSite"></param>
        /// <param name="replaceBy"></param>
        private async void InsertWorkOrderReplaceHistory(decimal replaceOperationID, decimal snID, decimal oldWoID,
            decimal newWoID, decimal plantCode, decimal replaceType, decimal replaceSite, string replaceBy)
        {
            try
            {
                decimal count = await GetWoReplaceHistoryDataTable(snID);
                string sql = @"INSERT INTO SFCS_WO_REPLACE(REPLACE_WO_ID,REPLACE_OPERATION_ID,SN_ID,OLD_WO_ID,NEW_WO_ID,
                                                         PLANT_CODE,REPLACE_TYPE,REPLACE_SITE_ID,REPLACE_ORDER_NO,REPLACE_BY,REPLACE_TIME)
                                                         VALUES(SFCS_WO_REPLACE_SEQ.NEXTVAL,:REPLACE_OPERATION_ID,:SN_ID,:OLD_WO_ID,:NEW_WO_ID,
                                                         :PLANT_CODE,:REPLACE_TYPE,:REPLACE_SITE_ID,:REPLACE_ORDER_NO,:REPLACE_BY,SYSDATE)";
                await _dbConnection.ExecuteAsync(sql, new
                {
                    REPLACE_OPERATION_ID = replaceOperationID,
                    SN_ID = snID,
                    OLD_WO_ID = oldWoID,
                    NEW_WO_ID = newWoID,
                    PLANT_CODE = plantCode,
                    REPLACE_TYPE = replaceType,
                    REPLACE_SITE_ID = replaceSite,
                    REPLACE_ORDER_NO = count + 1,
                    REPLACE_BY = replaceBy
                });
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// 獲取工單替換歷史記錄数(SFCS_WO_REPLACE表不存在)
        /// </summary>
        /// <param name="SN_ID"></param>
        /// <returns></returns>
        private async Task<decimal> GetWoReplaceHistoryDataTable(decimal SN_ID)
        {
            try
            {
                string sql = @"select count(0) from  SFCS_WO_REPLACE where SN_ID=:SN_ID ";
                var result = await _dbConnection.ExecuteScalarAsync(sql, new { SN_ID });
                return (decimal)result;
            }
            catch (Exception ex)
            {
                return 0;
            }
        }

        /// <summary>
        /// 更新Runcard信息
        /// </summary>
        /// <param name=""></param>
        private async void UpdateRuncard(List<SfcsRuncardListModel> runlist, SfcsReworkAddOrModifyModel remodel, decimal reworkOperationID)
        {
            // 2.更新流水號信息
            foreach (var row in runlist)
            {
                bool addRuncard = await BackupRuncardReworkData(reworkOperationID, remodel.REPAIRER, (decimal)row.CURRENT_SITE, row.SN);

                // 1.更新棧板/卡通信息
                ClearCartonAndPallet(row, remodel, reworkOperationID);

                deleteCarton = true;//包装返工作业要清SFCS_RUNCARD的箱号
                if (remodel.RETYPE == 0)
                {
                    bool updateRuncard = await UpdateRuncardWipRework(deleteCarton, deletePallet, row.ID, remodel.CHOOSEINDEXVALUE);
                }
                else
                {
                    var NewWorkOrderRows = await GetWorkOrderTable(0, remodel.NEW_WORKNO);
                    if (!NewWorkOrderRows.IsNull() && NewWorkOrderRows.Count > 0)
                    {
                        UpdateRuncardTurninRework(deleteCarton, deletePallet, row.ID, NewWorkOrderRows[0].ID, remodel.CHOOSEINDEXVALUE, (decimal)remodel.ROUTE_ID);

                        UpdateWorkOrderInput(NewWorkOrderRows[0].ID);
                        // 將標記為T（待處理）的SN對應的抽檢標記更新為N（待抽檢）
                        if (!row.SAMPLE_FLAG.IsNullOrEmpty() && row.SAMPLE_FLAG.ToString().ToUpper() == "T")
                        {
                            UpdateRuncardSampleFlag(GlobalVariables.EnableN, row.ID);
                        }
                        // 將標記為TI（不抽檢）的SN對應的抽檢標記更新為I（不過抽檢）
                        if (!row.SAMPLE_FLAG.IsNullOrEmpty() && row.SAMPLE_FLAG.ToString().ToUpper() == "TI")
                        {
                            UpdateRuncardSampleFlag("I", row.ID);
                        }
                    }
                }
            }
        }

        private static bool deleteCarton = false;
        private static bool deletePallet = false;

        /// <summary>
        /// 清除卡通棧板
        /// </summary>
        /// 若SN已Link栈板，但IE修改制程取消Link Pallet 工序，则清除卡时同时清除Pallet，避免返工后Pallet仍然存在
        /// 先清除栈板，再清除卡通，避免卡通再清除一次Pallet导致异常。 shanki.f remark 20160204
        private async void ClearCartonAndPallet(SfcsRuncardListModel row, SfcsReworkAddOrModifyModel remodel, decimal reworkOperationID)
        {
            var workorder = await GetWorkOrderTable((decimal)row.WO_ID, "");
            if (!workorder.IsNull() && workorder.Count > 0)
            {
                string partNo = workorder[0].PART_NO;
                //看是否返到採集棧板前面站點
                if (remodel.RETYPE == 0 || remodel.ROUTE_ID == remodel.ORIGINALROUTEID)
                {
                    var tempOperation = await GetSetupOperationObjectDataTable(partNo, 675, (decimal)row.ROUTE_ID, (decimal)remodel.CHOOSEOPERATIONORDER, 1);
                    if (!tempOperation.IsNull() && tempOperation.Count == 1)
                    {
                        deletePallet = true;
                        if (!row.PALLET_NO.ToString().IsNullOrEmpty())
                        {
                            UpdatePalletRework(row.PALLET_NO, reworkOperationID, remodel.REPAIRER, (decimal)row.CURRENT_SITE);
                        }
                    }
                }
                else
                {
                    var tempOperation = await GetSetupOperationObjectDataTable(partNo, 675, (decimal)remodel.ORIGINALROUTEID, (decimal)remodel.OLDCHOOSEOPERATIONORDER, 1);
                    if (!tempOperation.IsNull() && tempOperation.Count == 1)
                    {
                        deletePallet = true;
                        if (!row.PALLET_NO.ToString().IsNullOrEmpty())
                        {
                            UpdatePalletRework(row.PALLET_NO, reworkOperationID, remodel.REPAIRER, (decimal)row.CURRENT_SITE);
                        }
                    }
                }

                //看是否返到採集卡通前面站點
                if (remodel.RETYPE == 0 || remodel.ROUTE_ID == remodel.ORIGINALROUTEID)
                {
                    var tempOperation = await GetSetupOperationObjectDataTable(partNo, 674, (decimal)row.ROUTE_ID, (decimal)remodel.CHOOSEOPERATIONORDER, 1);
                    if (!tempOperation.IsNull() && tempOperation.Count == 1)
                    {
                        deleteCarton = true;
                        if (!row.CARTON_NO.ToString().IsNullOrEmpty())
                        {
                            UpdateCartonRework(row.CARTON_NO, reworkOperationID, remodel.REPAIRER, (decimal)row.CURRENT_SITE);
                        }
                        //若制程中无Link Pallet 工序则deletePallet=false，此时SN若有栈板则同时清除栈板
                        if (!deletePallet && !row.PALLET_NO.ToString().IsNullOrEmpty())
                        {
                            deletePallet = true;
                            UpdatePalletRework(row.PALLET_NO, reworkOperationID, remodel.REPAIRER, (decimal)row.CURRENT_SITE);
                        }
                    }
                }
                else
                {
                    var tempOperation = await GetSetupOperationObjectDataTable(partNo, 674, (decimal)remodel.ORIGINALROUTEID, (decimal)remodel.OLDCHOOSEOPERATIONORDER, 1);
                    if (!tempOperation.IsNull() && tempOperation.Count == 1)
                    {
                        deleteCarton = true;
                        if (!row.CARTON_NO.ToString().IsNullOrEmpty())
                        {
                            UpdateCartonRework(row.CARTON_NO, reworkOperationID, remodel.REPAIRER, (decimal)row.CURRENT_SITE);
                        }
                        //若制程中无Link Pallet 工序则deletePallet=false，此时SN若有栈板则同时清除栈板
                        if (!deletePallet && !row.PALLET_NO.ToString().IsNullOrEmpty())
                        {
                            deletePallet = true;
                            UpdatePalletRework(row.PALLET_NO, reworkOperationID, remodel.REPAIRER, (decimal)row.CURRENT_SITE);
                        }
                    }
                }
            }
        }

        /// <summary>
        /// 更新存倉後返工
        /// </summary>
        /// <param name="deleteCarton"></param>
        /// <param name="deletePallet"></param>
        /// <param name="snID"></param>
        /// <param name="operationID"></param>
        /// <param name="woID"></param>
        /// <param name="routeID"></param>
        private async void UpdateRuncardTurninRework(Boolean deleteCarton, Boolean deletePallet,
             decimal snID, decimal operationID, decimal woID, decimal routeID)
        {
            string sqlstr = @"UPDATE SFCS_RUNCARD SET WIP_OPERATION=:WIP_OPERATION, LAST_OPERATION=:WIP_OPERATION,
                                                            STATUS=10, TURNIN_NO=NULL, SAMPLE_FLAG=NULL, TURNIN_TIME=NULL ";
            if (deleteCarton)
            {
                sqlstr += " , CARTON_NO=NULL ";
            }
            if (deletePallet)
            {
                sqlstr += " , PALLET_NO=NULL ";
            }
            string sql = sqlstr + " , WO_ID=:WO_ID, ROUTE_ID=:ROUTE_ID   WHERE ID=:ID ";

            await _dbConnection.ExecuteAsync(sql, new
            {
                WIP_OPERATION = operationID,
                ID = snID,
                WO_ID = woID,
                ROUTE_ID = routeID
            });

        }

        /// <summary>
        /// 更新工單投入
        /// </summary>
        /// <param name="workOrder"></param>
        private async void UpdateWorkOrderInput(decimal workOrderID)
        {
            try
            {
                string sql = @"UPDATE SFCS_WO SET INPUT_QTY=INPUT_QTY+1 WHERE ID=:ID";
                await _dbConnection.ExecuteAsync(sql, new { ID = workOrderID });
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// 更新runcard抽檢狀態
        /// </summary>
        /// <param name="sampleFlag"></param>
        /// <param name="snID"></param>
        private async void UpdateRuncardSampleFlag(string sampleFlag, decimal snID)
        {
            try
            {
                string sql = @"UPDATE SFCS_RUNCARD SET SAMPLE_FLAG=:SAMPLE_FLAG WHERE ID=:ID ";
                await _dbConnection.ExecuteAsync(sql, new { SAMPLE_FLAG = sampleFlag, ID = snID });
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }




        /// <summary>
        /// 獲取設定製程中對象
        /// </summary>
        /// <param name="partNumber"></param>
        /// <param name="objectID"></param>
        /// <param name="routeID"></param>
        /// <param name="orderNo"></param>
        /// <param name="objectMode"></param>
        /// <returns></returns>
        public async Task<List<SetupOperationObjectListModel>> GetSetupOperationObjectDataTable(
            string partNumber, decimal objectID, decimal routeID, decimal orderNo, decimal objectMode)
        {
            try
            {
                string sql = @"SELECT *
  FROM (SELECT ID,
               PART_NO,
               RESOURCE_ID OBJECT_ID,
               REWORK_REMOVE_FLAG,
               COLLECT_OPERATION_ID,
               ENABLED
          FROM SFCS_PRODUCT_RESOURCES
         WHERE PART_NO = :PART_NO AND RESOURCE_ID = :OBJECT_ID
        UNION
        SELECT ID,
               PART_NO,
               COMPONENT_ID OBJECT_ID,
               REWORK_REMOVE_FLAG,
               COLLECT_OPERATION_ID,
               ENABLED
          FROM SFCS_PRODUCT_COMPONENTS
         WHERE PART_NO = :PART_NO AND COMPONENT_ID = :OBJECT_ID
        UNION
        SELECT ID,
               PART_NO,
               UID_ID OBJECT_ID,
               REWORK_REMOVE_FLAG,
               COLLECT_OPERATION_ID,
               ENABLED
          FROM SFCS_PRODUCT_UIDS
         WHERE PART_NO = :PART_NO AND UID_ID = :OBJECT_ID
        UNION
        SELECT ID,
               PART_NO,
               674 OBJECT_ID,
               'Y' REWORK_REMOVE_FLAG,
               COLLECT_OPERATION_ID,
               ENABLED
          FROM SFCS_PRODUCT_CARTON
         WHERE PART_NO = :PART_NO
        UNION
        SELECT ID,
               PART_NO,
               675 OBJECT_ID,
               'Y' REWORK_REMOVE_FLAG,
               COLLECT_OPERATION_ID,
               ENABLED
          FROM SFCS_PRODUCT_PALLET
         WHERE PART_NO = :PART_NO) SCC
 WHERE     SCC.COLLECT_OPERATION_ID IN (SELECT CURRENT_OPERATION_ID
                                          FROM SFCS_ROUTE_CONFIG
                                         WHERE     ROUTE_ID = :ROUTE_ID
                                               AND ORDER_NO >= :ORDER_NO)
       AND SCC.ENABLED = 'Y'";
                var list = await _dbConnection.QueryAsync<SetupOperationObjectListModel>(sql, new
                {
                    PART_NO = partNumber,
                    OBJECT_ID = objectID,
                    ROUTE_ID = routeID,
                    ORDER_NO = orderNo,
                    OBJECT_MODE = objectMode
                });
                return list.ToList();
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }

        /// <summary>
        /// 更新棧板返工
        /// </summary>
        /// <param name="palletNumber"></param>
        /// <param name="data"></param>
        private async void UpdatePalletRework(string palletNumber,
            decimal reworkOperationID, string reworkOperator, decimal siteID)
        {
            var tempRun = await GetRuncardDataTable("PALLET_NO", palletNumber);
            decimal quantity = tempRun.Count;
            if (quantity == 0)
            {
                return;
            }
            var palletRow = await GetCollectPallet(palletNumber);
            if (palletRow.IsNull() || palletRow.Count <= 0)
            {
                return;
            }
            if (await GetLogPalletBackupHistory(palletRow[0].COLLECT_PALLET_ID) == 0)
            {
                BackupPalletReworkData(reworkOperationID, reworkOperator,
                    siteID, palletRow[0].COLLECT_PALLET_ID);
            }
            if (quantity == 1)
            {
                DeletePallet(palletRow[0].COLLECT_PALLET_ID);

                bool addAtt = await BackupAttachmentReworkData(reworkOperationID, reworkOperator, siteID, palletRow[0].COLLECT_PALLET_ID);

                bool deleteAtt = await DeleteAttachment(palletRow[0].COLLECT_PALLET_ID);
                return;
            }
            UpdatePallet(palletNumber, 0, quantity - 1);

        }


        /// <summary>
        /// 更新返工卡通信息
        /// </summary>
        /// <param name="cartonNumber"></param>
        /// <param name="data"></param>
        /// <param name="reworkOperationID"></param>
        /// <param name="reworkOperator"></param>
        /// <param name="siteID"></param>
        private async void UpdateCartonRework(string cartonNumber,
             decimal reworkOperationID, string reworkOperator, decimal siteID)
        {
            var tempRun = await GetRuncardDataTable("CARTON_NO", cartonNumber);
            decimal quantity = tempRun.Count;
            if (quantity == 0)
            {
                return;
            }
            var cartonRow = await GetCollectCarton(cartonNumber);
            if (cartonRow.IsNull() || cartonRow.Count <= 0)
            {
                return;
            }
            if (await GetLogCartonBackupHistory(cartonRow[0].COLLECT_CARTON_ID) == 0)
            {
                BackupCartonReworkData(reworkOperationID, reworkOperator,
                       siteID, cartonRow[0].COLLECT_CARTON_ID);
            }
            if (quantity == 1)
            {
                DeleteCarton(cartonRow[0].COLLECT_CARTON_ID);
                bool addAtt = await BackupAttachmentReworkData(reworkOperationID, reworkOperator, siteID, cartonRow[0].COLLECT_CARTON_ID);

                bool deleteAtt = await DeleteAttachment(cartonRow[0].COLLECT_CARTON_ID);
                return;
            }
            UpdateCarton(cartonNumber, 0, quantity - 1);
        }


        /// <summary>
        ///  获取收集的栈板信息
        /// </summary>
        /// <param name="PALLET_NO"></param>
        /// <returns></returns>
        private async Task<List<SfcsCollectPalletsListModel>> GetCollectPallet(string PALLET_NO)
        {
            try
            {
                string sql = @"SELECT  * FROM SFCS_COLLECT_PALLETS    WHERE  PALLET_NO=:PALLET_NO ";
                var list = await _dbConnection.QueryAsync<SfcsCollectPalletsListModel>(sql, new { PALLET_NO });
                return list.ToList();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        ///  获取收集的栈板信息
        /// </summary>
        /// <param name="PALLET_NO"></param>
        /// <returns></returns>
        private async Task<List<SfcsCollectCartonsListModel>> GetCollectCarton(string CARTON_NO)
        {
            try
            {
                string sql = @"SELECT  * FROM SFCS_COLLECT_CARTONS    WHERE  CARTON_NO=:CARTON_NO ";
                var list = await _dbConnection.QueryAsync<SfcsCollectCartonsListModel>(sql, new { CARTON_NO });
                return list.ToList();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        /// <summary>
        /// 獲取備份棧板歷史記錄
        /// </summary>
        /// <param name="collectPalletID"></param>
        /// <returns></returns>
        private async Task<decimal> GetLogPalletBackupHistory(decimal collectPalletID)
        {
            try
            {
                string sql = @"SELECT COUNT(*) QTY FROM JZMES_LOG.SFCS_COLLECT_PALLETS SCP
                                                     WHERE COLLECT_PALLET_ID=:COLLECT_PALLET_ID";

                var result = await _dbConnection.ExecuteScalarAsync(sql, new { COLLECT_PALLET_ID = collectPalletID });
                return (decimal)result;
            }
            catch (Exception ex)
            {
                return 0;
            }
        }

        /// <summary>
        /// 獲取備份卡通歷史表
        /// </summary>
        /// <param name="collectPalletID"></param>
        /// <returns></returns>
        private async Task<decimal> GetLogCartonBackupHistory(decimal COLLECT_CARTON_ID)
        {
            try
            {
                string sql = @"SELECT COUNT(*) QTY FROM JZMES_LOG.SFCS_COLLECT_CARTONS
                                                     WHERE COLLECT_CARTON_ID=:COLLECT_CARTON_ID";

                var result = await _dbConnection.ExecuteScalarAsync(sql, new { COLLECT_CARTON_ID });
                return (decimal)result;
            }
            catch (Exception ex)
            {
                return 0;
            }
        }

        /// <summary>
        /// 删除栈板数据信息
        /// </summary>
        private async void DeletePallet(decimal COLLECT_PALLET_ID)
        {
            try
            {
                string sql = @"DELETE FROM SFCS_COLLECT_PALLETS WHERE COLLECT_PALLET_ID=:COLLECT_PALLET_ID";
                await _dbConnection.ExecuteAsync(sql, new { COLLECT_PALLET_ID });
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        /// <summary>
        /// 刪除卡通数据信息
        /// </summary>
        private async void DeleteCarton(decimal COLLECT_CARTON_ID)
        {
            try
            {
                string sql = @"DELETE FROM SFCS_COLLECT_CARTONS WHERE COLLECT_CARTON_ID=:COLLECT_CARTON_ID";
                await _dbConnection.ExecuteAsync(sql, new { COLLECT_CARTON_ID });
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        /// <summary>
        /// 更新棧板
        /// </summary>
        /// <param name="pallet"></param>
        /// <param name="status"></param>
        /// <param name="qty"></param>
        private async void UpdatePallet(string pallet, decimal status, decimal qty)
        {
            try
            {
                string sql = @"UPDATE SFCS_COLLECT_PALLETS SET QUANTITY=:QUANTITY, STATUS=:STATUS WHERE PALLET_NO=:PALLET_NO ";
                await _dbConnection.ExecuteAsync(sql, new
                {
                    PALLET_NO = pallet,
                    STATUS = status,
                    QUANTITY = qty
                });
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        /// <summary>
        /// 更新卡通
        /// </summary>
        /// <param name="carton"></param>
        /// <param name="status"></param>
        /// <param name="qty"></param>
        private async void UpdateCarton(string carton, decimal status, decimal qty)
        {
            try
            {
                string sql = @"UPDATE SFCS_COLLECT_CARTONS SET QUANTITY=:QUANTITY, STATUS=:STATUS WHERE CARTON_NO=:CARTON_NO ";
                await _dbConnection.ExecuteAsync(sql, new
                {
                    CARTON_NO = carton,
                    STATUS = status,
                    QUANTITY = qty
                });
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }



        /// <summary>
        /// 備份Runcard返工數據
        /// </summary>
        /// <param name="reworkOperationID"></param>
        /// <param name="reworkOperator"></param>
        /// <param name="siteID"></param>
        /// <param name="data"></param>
        /// <returns></returns>
        private async Task<bool> BackupRuncardReworkData(decimal reworkOperationID, string reworkOperator,
            decimal siteID, string data)
        {
            try
            {
                string sql = @"INSERT INTO JZMES_LOG.SFCS_RUNCARD SELECT SR.*,SYSDATE, 
                                                      :REWORK_OPERATION_ID,:REWORK_OPERATOR,:OPERATION_SITE FROM SFCS_RUNCARD SR
                                                      WHERE SN=:DATA OR CARTON_NO=:DATA OR PALLET_NO=:DATA";
                return await _dbConnection.ExecuteAsync(sql, new
                {
                    REWORK_OPERATION_ID = reworkOperationID,
                    REWORK_OPERATOR = reworkOperator,
                    OPERATION_SITE = siteID,
                    DATA = data
                }) > 0;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }



        /// <summary>
        /// 備份棧板返工數據    JZMES_LOG.
        /// </summary>
        /// <param name="reworkOperationID"></param>
        /// <param name="reworkOperator"></param>
        /// <param name="siteID"></param>
        /// <param name="collectPalletID"></param>
        public async void BackupPalletReworkData(decimal reworkOperationID, string reworkOperator,
           decimal siteID, decimal collectPalletID)
        {
            try
            {
                string sql = @"INSERT INTO SFCS_COLLECT_PALLETS 
                                                     SELECT SCP.*,SYSDATE,:REWORK_OPERATION_ID,:REWORK_OPERATOR,:OPERATION_SITE
                                                     FROM SFCS_COLLECT_PALLETS SCP WHERE SCP.COLLECT_PALLET_ID=:COLLECT_PALLET_ID";
                await _dbConnection.ExecuteAsync(sql, new
                {
                    REWORK_OPERATION_ID = reworkOperationID,
                    REWORK_OPERATOR = reworkOperator,
                    OPERATION_SITE = siteID,
                    COLLECT_PALLET_ID = collectPalletID
                });
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }

        /// <summary>
        /// 備份卡通返工數據    JZMES_LOG.
        /// </summary>
        /// <param name="reworkOperationID"></param>
        /// <param name="reworkOperator"></param>
        /// <param name="siteID"></param>
        /// <param name="collectPalletID"></param>
        public async void BackupCartonReworkData(decimal reworkOperationID, string reworkOperator,
           decimal siteID, decimal collectCartonID)
        {
            try
            {
                string sql = @"INSERT INTO SFCS_COLLECT_CARTONS 
                                                     SELECT SCC.*,SYSDATE,:REWORK_OPERATION_ID,:REWORK_OPERATOR,:OPERATION_SITE
                                                     FROM SFCS_COLLECT_CARTONS SCC WHERE SCC.COLLECT_CARTON_ID=:COLLECT_CARTON_ID";
                await _dbConnection.ExecuteAsync(sql, new
                {
                    REWORK_OPERATION_ID = reworkOperationID,
                    REWORK_OPERATOR = reworkOperator,
                    OPERATION_SITE = siteID,
                    COLLECT_CARTON_ID = collectCartonID
                });
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }


        /// <summary>
        /// 更新Runcard WIP返工
        /// </summary>
        /// <param name="deleteCarton"></param>
        /// <param name="deletePallet"></param>
        /// <param name="snID"></param>
        /// <param name="operationID"></param>
        /// <returns></returns>
        private async Task<bool> UpdateRuncardWipRework(Boolean deleteCarton,
            Boolean deletePallet, decimal snID, decimal operationID)
        {
            try
            {
                string sql = @"UPDATE SFCS_RUNCARD SET WIP_OPERATION=:WIP_OPERATION, STATUS=10, SAMPLE_FLAG=NULL ";

                if (deleteCarton)
                    sql += ", CARTON_NO=NULL ";
                if (deletePallet)
                    sql += ", PALLET_NO=NULL ";
                sql += " WHERE ID=:ID ";
                return await _dbConnection.ExecuteAsync(sql, new
                {
                    WIP_OPERATION = operationID,
                    ID = snID
                }) > 0;
            }
            catch (Exception ex)
            {
                throw ex;
            }


        }

        /// <summary>
        /// 記錄歷史作業
        /// </summary>
        private async void RecordOperationHistory(List<SfcsRuncardListModel> runlist, SfcsReworkAddOrModifyModel remodel, decimal reworkOperationID)
        {
            foreach (var row in runlist)
            {
                var siteOperation = await GetOperationSiteDataTable((decimal)row.CURRENT_SITE, 0, 0);
                if (!siteOperation.IsNull() && siteOperation.Count > 0)
                {
                    decimal siteOperationID = siteOperation[0].OPERATION_ID;
                    if (remodel.RETYPE == 0)
                    {
                        decimal visitNumber = 1;
                        if (siteOperationID == GlobalVariables.FAOperation ||
                            siteOperationID == GlobalVariables.PAOperation)
                        {
                            //系統hp在fa/pa返工時必須得保持最大的visit number
                            visitNumber = await GetMaxOpertionHistoryVisitNumber(row.ID);
                        }
                        bool addhistory = await InsertOperationHistory(reworkOperationID, (decimal)remodel.WORKORDERID, row.ID, (decimal)remodel.ROUTE_ID, siteOperationID, (decimal)row.CURRENT_SITE, remodel.REPAIRER, GlobalVariables.Rework, visitNumber);
                    }
                    else
                    {
                        var NewWorkOrderRows = await GetWorkOrderTable(0, remodel.NEW_WORKNO);
                        if (!NewWorkOrderRows.IsNull() && NewWorkOrderRows.Count > 0)
                        {
                            bool addhistory = await InsertOperationHistory(reworkOperationID, NewWorkOrderRows[0].ID, row.ID, (decimal)remodel.ROUTE_ID, siteOperationID, (decimal)row.CURRENT_SITE, remodel.REPAIRER, GlobalVariables.Rework, 1);
                        }
                    }
                }
            }
        }




        /// <summary>
        /// 修改對象operation code值
        /// </summary>
        private async void UpdateProductOperationCode(List<SfcsRuncardListModel> runlist, SfcsReworkAddOrModifyModel remodel)
        {
            if (remodel.RETYPE != 0)
            {
                if (remodel.ROUTE_ID != remodel.ORIGINALROUTEID)
                {
                    foreach (var row in runlist)
                    {
                        UpdateObjectProductOperationCode(row.ID, (decimal)remodel.ROUTE_ID);
                    }
                }
            }
        }

        /// <summary>
        /// 同步Product Operation Code
        /// 使用範圍
        /// 1.返工（除維修返工）
        /// 2.Fail時強制存倉再返到維修站，消不良出站時使用
        /// </summary>
        private async void UpdateObjectProductOperationCode(decimal serialNumberID, decimal newRouteID)
        {
            //將未刪除的零件Product Operation Code更新過來
            var componentTable = await GetComponentProductOperationCode(serialNumberID);
            foreach (var operationRow in componentTable)
            {
                decimal oldProductOperationCode = (decimal)operationRow.PRODUCT_OPERATION_CODE;
                decimal objectID = (decimal)operationRow.COMPONENT_ID;
                decimal? newProductOperationCode = await GetNewProductOperationCode(newRouteID, objectID, oldProductOperationCode);
                if (!newProductOperationCode.IsNull())
                {
                    bool updateComp = await UpdateComponentProductOperationCode(serialNumberID,
                         (decimal)newProductOperationCode, oldProductOperationCode, objectID);
                }
            }

            //將未刪除的Product Operation Code更新過來
            var uidTable = await GetUIDProductOperationCode(serialNumberID);
            foreach (var operationRow in uidTable)
            {
                decimal oldProductOperationCode = (decimal)operationRow.PRODUCT_OPERATION_CODE;
                decimal objectID = (decimal)operationRow.UID_ID;
                decimal? newProductOperationCode = await GetNewProductOperationCode(newRouteID, objectID, oldProductOperationCode);
                if (!newProductOperationCode.IsNull())
                {
                    bool updateUid = await UpdateUidProductOperationCode(serialNumberID,
                        (decimal)newProductOperationCode, oldProductOperationCode, objectID);
                }
            }
            //將未刪除的資源Product Operation Code更新過來
            var resourceTable = await GetResourceProductOperationCode(serialNumberID);
            foreach (var operationRow in resourceTable)
            {

                decimal oldProductOperationCode = (decimal)operationRow.PRODUCT_OPERATION_CODE;
                decimal objectID = (decimal)operationRow.RESOURCE_ID;
                decimal? newProductOperationCode = await GetNewProductOperationCode(newRouteID, objectID, oldProductOperationCode);
                if (!newProductOperationCode.IsNull())
                {
                    bool updateResource = await UpdateResourceProductOperationCode(serialNumberID,
                        (decimal)newProductOperationCode, oldProductOperationCode, objectID);
                }
            }
        }

        /// <summary>
        /// 獲取零件產品製程工序鍵值
        /// </summary>
        /// <param name="snID"></param>
        /// <returns></returns>
        private async Task<List<SfcsCollectComponentsListModel>> GetComponentProductOperationCode(decimal SN_ID)
        {
            try
            {
                string sql = @"SELECT DISTINCT COMPONENT_ID, PRODUCT_OPERATION_CODE FROM SFCS_COLLECT_COMPONENTS WHERE SN_ID = :SN_ID";
                var list = await _dbConnection.QueryAsync<SfcsCollectComponentsListModel>(sql, new { SN_ID });
                return list.ToList();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        /// <summary>
        /// 獲取uid產品製程工序值
        /// </summary>
        /// <param name="snID"></param>
        /// <returns></returns>
        private async Task<List<SfcsCollectUidsListModel>> GetUIDProductOperationCode(decimal SN_ID)
        {
            try
            {
                string sql = @"SELECT  DISTINCT UID_ID, PRODUCT_OPERATION_CODE FROM SFCS_COLLECT_UIDS WHERE SN_ID = :SN_ID";
                var list = await _dbConnection.QueryAsync<SfcsCollectUidsListModel>(sql, new { SN_ID });
                return list.ToList();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// 獲取資源pcode
        /// </summary>
        /// <param name="SN_ID"></param>
        /// <returns></returns>
        private async Task<List<SfcsCollectResourcesListModel>> GetResourceProductOperationCode(decimal SN_ID)
        {
            try
            {
                string sql = @"SELECT DISTINCT RESOURCE_ID, PRODUCT_OPERATION_CODE FROM SFCS_COLLECT_RESOURCES WHERE SN_ID = :SN_ID";
                var list = await _dbConnection.QueryAsync<SfcsCollectResourcesListModel>(sql, new { SN_ID });
                return list.ToList();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// 獲取站點基礎信息
        /// </summary>
        /// <param name="ID"></param>
        /// <returns></returns>
        private async Task<List<SfcsOperationSitesListModel>> GetOperationSiteDataTable(decimal ID, decimal OPERATION_ID, decimal OPERATION_LINE_ID)
        {
            try
            {
                string strwhere = " where ENABLED='Y' ";
                if (ID > 0)
                    strwhere += string.Format(" and ID={0}", ID);
                if (OPERATION_ID > 0)
                    strwhere += string.Format(" and OPERATION_ID={0} ", OPERATION_ID);
                if (OPERATION_LINE_ID > 0)
                    strwhere += string.Format(" and OPERATION_LINE_ID={0}", OPERATION_LINE_ID);

                string sql = "SELECT * FROM SFCS_OPERATION_SITES  " + strwhere;
                var list = await _dbConnection.QueryAsync<SfcsOperationSitesListModel>(sql);
                return list.ToList();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        /// <summary>
        /// 生成作業記錄
        /// </summary>
        /// <param name="operationID"></param>
        /// <param name="woID"></param>
        /// <param name="snID"></param>
        /// <param name="routeID"></param>
        /// <param name="operationSiteID"></param>
        /// <param name="user"></param>
        /// <param name="status"></param>
        /// <param name="visitNumber"></param>
        private async Task<bool> InsertOperationHistory(decimal operationID, decimal woID, decimal snID,
            decimal routeID, decimal siteOperationID, decimal operationSiteID,
            string user, decimal status, decimal? visitNumber)
        {
            try
            {
                if (visitNumber.IsNull())
                {
                    visitNumber = await GetVisitNumber(snID);
                }
                string sql = @"INSERT INTO SFCS_OPERATION_HISTORY(SN_ID,OPERATION_ID,WO_ID,ROUTE_ID,SITE_OPERATION_ID,OPERATION_SITE_ID,
                                                         OPERATOR,OPERATION_STATUS,OPERATION_TIME,VISIT_NUMBER) VALUES(:SN_ID,:OPERATION_ID,:WO_ID,
                                                         :ROUTE_ID,:SITE_OPERATION_ID,:OPERATION_SITE_ID,:OPERATOR,:OPERATION_STATUS,SYSDATE,:VISIT_NUMBER)";
                return await _dbConnection.ExecuteAsync(sql, new
                {
                    OPERATION_ID = operationID,
                    WO_ID = woID,
                    SN_ID = snID,
                    ROUTE_ID = routeID,
                    SITE_OPERATION_ID = siteOperationID,
                    OPERATION_SITE_ID = operationSiteID,
                    OPERATOR = user,
                    OPERATION_STATUS = status,
                    VISIT_NUMBER = visitNumber
                }) > 0;
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }

        /// <summary>
        /// 修改零件對應的鍵值
        /// </summary>
        /// <param name="snID"></param>
        /// <param name="newCode"></param>
        /// <param name="oldCode"></param>
        private async Task<bool> UpdateComponentProductOperationCode(decimal snID, decimal newCode, decimal oldCode, decimal objectID)
        {
            try
            {
                string sql = @"UPDATE SFCS_COLLECT_COMPONENTS SET PRODUCT_OPERATION_CODE=:NEW_PRODUCT_OPERATION_CODE
                                   WHERE SN_ID = :SN_ID AND PRODUCT_OPERATION_CODE=:OLD_PRODUCT_OPERATION_CODE AND COMPONENT_ID = :COMPONENT_ID";
                return await _dbConnection.ExecuteAsync(sql, new
                {
                    SN_ID = snID,
                    OLD_PRODUCT_OPERATION_CODE = oldCode,
                    NEW_PRODUCT_OPERATION_CODE = newCode,
                    COMPONENT_ID = objectID
                }) > 0;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// 修改uid
        /// </summary>
        /// <param name="snID"></param>
        /// <param name="newCode"></param>
        /// <param name="oldCode"></param>
        /// <param name="objectID"></param>
        /// <returns></returns>
        private async Task<bool> UpdateUidProductOperationCode(decimal snID, decimal newCode, decimal oldCode, decimal objectID)
        {
            try
            {
                string sql = @"UPDATE SFCS_COLLECT_UIDS SET PRODUCT_OPERATION_CODE=:NEW_PRODUCT_OPERATION_CODE 
                                    WHERE SN_ID = :SN_ID AND PRODUCT_OPERATION_CODE = :OLD_PRODUCT_OPERATION_CODE AND UID_ID = :UID_ID";
                return await _dbConnection.ExecuteAsync(sql, new
                {
                    SN_ID = snID,
                    OLD_PRODUCT_OPERATION_CODE = oldCode,
                    NEW_PRODUCT_OPERATION_CODE = newCode,
                    COMPONENT_ID = objectID
                }) > 0;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// 更新資源pcode
        /// </summary>
        /// <param name="snID"></param>
        /// <param name="newCode"></param>
        /// <param name="oldCode"></param>
        /// <param name="objectID"></param>
        /// <returns></returns>
        private async Task<bool> UpdateResourceProductOperationCode(decimal snID, decimal newCode, decimal oldCode, decimal objectID)
        {
            try
            {
                string sql = @"UPDATE SFCS_COLLECT_RESOURCES SET PRODUCT_OPERATION_CODE=:NEW_PRODUCT_OPERATION_CODE
                WHERE  SN_ID = :SN_ID AND PRODUCT_OPERATION_CODE=:OLD_PRODUCT_OPERATION_CODE AND RESOURCE_ID = :RESOURCE_ID";
                return await _dbConnection.ExecuteAsync(sql, new
                {
                    SN_ID = snID,
                    OLD_PRODUCT_OPERATION_CODE = oldCode,
                    NEW_PRODUCT_OPERATION_CODE = newCode,
                    COMPONENT_ID = objectID
                }) > 0;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// 根據舊製程中operation獲取新製程中對應的值
        /// </summary>
        ///<param name="objectID"></param>
        ///<param name="routeID"></param>
        ///<param name="oldProductOperationCode"></param>
        /// <returns></returns>
        private async Task<decimal?> GetNewProductOperationCode(decimal routeID, decimal objectID, decimal oldProductOperationCode)
        {
            var routeConfigTable = await GetRouteConfigDataTable(routeID, 0);
            //是不是Print(B)或Print(T)
            var oldRouteConfigRow = await GetLogRouteConfigDataTable(oldProductOperationCode);
            if (!routeConfigTable.IsNull() && routeConfigTable.Count > 0)
            {
                if (!oldRouteConfigRow.IsNull() && oldRouteConfigRow.Count > 0)
                {
                    foreach (var row in routeConfigTable)
                    {
                        if ((oldRouteConfigRow[0].CURRENT_OPERATION_ID == GlobalVariables.PrintBOperation
                               && row.CURRENT_OPERATION_ID != GlobalVariables.PrintBOperation)
                               || (oldRouteConfigRow[0].CURRENT_OPERATION_ID == GlobalVariables.PrintTOperation
                               && row.CURRENT_OPERATION_ID != GlobalVariables.PrintTOperation))
                        {
                            continue;
                        }
                        var newObjectRow = await GetOperationObjectDataTable(row.PRODUCT_OPERATION_CODE, objectID, GlobalVariables.EnableY);
                        if (!newObjectRow.IsNull() && newObjectRow.Count > 0)
                        {
                            return newObjectRow[0].PRODUCT_OPERATION_CODE;
                        }
                    }
                }
            }
            return null;
        }

        /// <summary>
        /// 獲取備份中製程配置
        /// </summary>
        /// <param name="productOperationCode"></param>
        /// <returns></returns>
        private async Task<List<SfcsRouteConfigListModel>> GetLogRouteConfigDataTable(decimal PRODUCT_OPERATION_CODE)
        {
            try
            {
                string sql = @"SELECT * FROM  SFCS_ROUTE_CONFIG WHERE PRODUCT_OPERATION_CODE=:PRODUCT_OPERATION_CODE AND ROWNUM=1 ORDER BY VERSION DESC ";
                var list = await _dbConnection.QueryAsync<SfcsRouteConfigListModel>(sql, new { PRODUCT_OPERATION_CODE });
                return list.ToList();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private async Task<List<SfcsOperationObjectsListModel>> GetOperationObjectDataTable(decimal PRODUCT_OPERATION_CODE, decimal OBJECT_ID, string ENABLED)
        {
            try
            {
                string sql = @"SELECT * FROM  SFCS_OPERATION_OBJECTS WHERE  PRODUCT_OPERATION_CODE=:PRODUCT_OPERATION_CODE and OBJECT_ID=:OBJECT_ID and ENABLED=:ENABLED ";
                var list = await _dbConnection.QueryAsync<SfcsOperationObjectsListModel>(sql, new { PRODUCT_OPERATION_CODE, OBJECT_ID, ENABLED });
                return list.ToList();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        /// <summary>
        /// 處理返工數據，清除相關對象信息并備份已收集對象信息
        /// </summary>
        private async void ProcessObjectDatas(List<SfcsCollectObjectsListMode> collectObjectTable, List<SfcsRuncardListModel> runcardTable, SfcsReworkAddOrModifyModel remodel, decimal reworkOperationID)
        {
            //不需要刪除任何對象數據
            if (collectObjectTable.Count == 0)
            {
                return;
            }
            if (remodel.RETYPE != 3)
            {
                decimal orlist = 0, orilist = 0;
                if (remodel.ORDERNOLIST != null && remodel.ORDERNOLIST.Count > 0)
                    orlist = remodel.ORDERNOLIST[Convert.ToInt32(remodel.CHOOSEINDEX)];
                if (remodel.ORIGINALORDERNOLIST != null && remodel.ORIGINALORDERNOLIST.Count > 0)
                    orilist = remodel.ORIGINALORDERNOLIST[Convert.ToInt32(remodel.OLDCHOOSEINDEX)];

                foreach (var runcardRow in runcardTable)
                {
                    foreach (var row in collectObjectTable)
                    {
                        if (row.REWORK_REMOVE_FLAG == null ||row.REWORK_REMOVE_FLAG == GlobalVariables.EnableY)
                        {
                            switch (row.OBJECT_KIND)
                            {
                                //清除零件
                                case "COMPONENT":
                                    if (remodel.RETYPE == 0 || remodel.ROUTE_ID == remodel.ORIGINALROUTEID)
                                    {
                                        ClearComponents(await GetCollectComponentsByRoute(runcardRow.ID,
                                              row.OBJECT_NAME, row.ODM_PN, row.CUSTOMER_PN, (decimal)remodel.ROUTE_ID, orlist), reworkOperationID, remodel.REPAIRER, (decimal)runcardRow.CURRENT_SITE);
                                    }
                                    else
                                    {
                                        ClearComponents(await GetCollectComponentsByRoute(runcardRow.ID,
                                                 row.OBJECT_NAME, row.ODM_PN, row.CUSTOMER_PN, (decimal)remodel.ROUTE_ID, orilist), reworkOperationID, remodel.REPAIRER, (decimal)runcardRow.CURRENT_SITE);
                                    }
                                    break;
                                //清除資源
                                case "RESOURCE":
                                    if (remodel.RETYPE == 0 || remodel.ROUTE_ID == remodel.ORIGINALROUTEID)
                                    {
                                        ClearResources(await GetCollectResourceTableByRoute(runcardRow.ID,
                                           row.OBJECT_NAME, (decimal)remodel.ROUTE_ID, orlist), reworkOperationID, remodel.REPAIRER, (decimal)runcardRow.CURRENT_SITE);
                                    }
                                    else
                                    {
                                        ClearResources(await GetCollectResourceTableByRoute(runcardRow.ID,
                                          row.OBJECT_NAME, (decimal)remodel.ROUTE_ID, orilist), reworkOperationID, remodel.REPAIRER, (decimal)runcardRow.CURRENT_SITE);
                                    }
                                    break;
                                //清除UID
                                case "UID":
                                    if (remodel.RETYPE == 0 || remodel.ROUTE_ID == remodel.ORIGINALROUTEID)
                                    {
                                        ClearUID(await GetCollectUIDByRoute(runcardRow.ID, row.OBJECT_NAME, (decimal)remodel.ROUTE_ID, orlist), reworkOperationID, remodel.REPAIRER, (decimal)runcardRow.CURRENT_SITE);
                                    }
                                    else
                                    {
                                        ClearUID(await GetCollectUIDByRoute(runcardRow.ID, row.OBJECT_NAME, (decimal)remodel.ROUTE_ID, orilist), reworkOperationID, remodel.REPAIRER, (decimal)runcardRow.CURRENT_SITE);
                                    }
                                    break;
                                default:
                                    break;

                            }
                        }
                    }
                }
            }
        }


        /// <summary>
        /// 清除Component對象數據
        /// </summary>
        private async void ClearComponents(List<SfcsCollectComponentsListModel> componentTable, decimal repairOperationID, string repairer, decimal siteID)
        {
            foreach (var row in componentTable)
            {
                bool addBack = await BackupComponentReworkData(repairOperationID, repairer, siteID, row.COLLECT_COMPONENT_ID);
                bool deleteComponent = await DeleteComponent(row.COLLECT_COMPONENT_ID);

                ClearAttachments(repairOperationID, repairer, siteID, row.COLLECT_COMPONENT_ID);

                if (row.DEVICE_FLAG == GlobalVariables.EnableY)
                {
                    bool updateDevice = await UpdateDeviceComponentWhenRework(4, row.CUSTOMER_COMPONENT_SN);
                }

            }

        }


        /// <summary>
        /// 清除資源對象數據
        /// </summary>
        /// <param name="resourceTable"></param>
        private async void ClearResources(List<SfcsCollectResourcesListModel> resourceTable, decimal repairOperationID, string repairer, decimal siteID)
        {
            foreach (var row in resourceTable)
            {
                bool addBack = await BackupResourceReworkData(repairOperationID, repairer, siteID, row.COLLECT_RESOURCE_ID);
                bool deleteResource = await DeleteResource(row.COLLECT_RESOURCE_ID);

                ClearAttachments(repairOperationID, repairer, siteID, row.COLLECT_RESOURCE_ID);
            }
        }

        /// <summary>
        /// 清除UID對象數據
        /// </summary>
        /// <param name="uidTable"></param>
        private async void ClearUID(List<SfcsCollectUidsListModel> uidTable, decimal repairOperationID, string repairer, decimal siteID)
        {
            foreach (var row in uidTable)
            {
                bool addBack = await BackupUIDReworkData(repairOperationID, repairer, siteID, row.COLLECT_UID_ID);
                bool deleteUid = await DeleteUID(row.COLLECT_UID_ID);

                ClearAttachments(repairOperationID, repairer, siteID, row.COLLECT_UID_ID);
            }
        }


        /// <summary>
        /// 備份UID返工數據   JZMES_LOG.SFCS_COLLECT_UIDS 
        /// </summary>
        /// <param name="reworkOperationID"></param>
        /// <param name="reworkOperator"></param>
        /// <param name="siteID"></param>
        /// <param name="collectUIDID"></param>
        /// <returns></returns>
        private async Task<bool> BackupUIDReworkData(decimal reworkOperationID, string reworkOperator,
           decimal siteID, decimal collectUIDID)
        {
            try
            {
                string sql = @"INSERT INTO JZMES_LOG.SFCS_COLLECT_UIDS 
                                                  SELECT SCU.*,SYSDATE,:REWORK_OPERATION_ID,:REWORK_OPERATOR,:OPERATION_SITE
                                                  FROM SFCS_COLLECT_UIDS SCU WHERE SCU.COLLECT_UID_ID=:COLLECT_UID_ID";

                return await _dbConnection.ExecuteAsync(sql, new
                {
                    REWORK_OPERATION_ID = reworkOperationID,
                    REWORK_OPERATOR = reworkOperator,
                    OPERATION_SITE = siteID,
                    COLLECT_UID_ID = collectUIDID,
                }) > 0;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        /// <summary>
        /// 刪除資源
        /// </summary>
        /// <param name="COLLECT_UID_ID"></param>
        /// <returns></returns>
        private async Task<bool> DeleteUID(decimal COLLECT_UID_ID)
        {
            try
            {
                string sql = @"DELETE FROM SFCS_COLLECT_UIDS  WHERE COLLECT_UID_ID=:COLLECT_UID_ID";
                return await _dbConnection.ExecuteAsync(sql, new { COLLECT_UID_ID }) > 0;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }



        /// <summary>
        /// 備份資源返工數據 JZMES_LOG.SFCS_COLLECT_RESOURCES
        /// </summary>
        /// <param name="reworkOperationID"></param>
        /// <param name="reworkOperator"></param>
        /// <param name="siteID"></param>
        /// <param name="collectComponentID"></param>
        private async Task<bool> BackupResourceReworkData(decimal reworkOperationID, string reworkOperator,
            decimal siteID, decimal collectResourceID)
        {
            try
            {
                string sql = @"INSERT INTO JZMES_LOG.SFCS_COLLECT_RESOURCES
                                                       SELECT SCR.*,SYSDATE,:REWORK_OPERATION_ID,:REWORK_OPERATOR,:OPERATION_SITE
                                                       FROM SFCS_COLLECT_RESOURCES SCR WHERE SCR.COLLECT_RESOURCE_ID=:COLLECT_RESOURCE_ID";

                return await _dbConnection.ExecuteAsync(sql, new
                {
                    REWORK_OPERATION_ID = reworkOperationID,
                    REWORK_OPERATOR = reworkOperator,
                    OPERATION_SITE = siteID,
                    COLLECT_RESOURCE_ID = collectResourceID,
                }) > 0;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// 刪除資源
        /// </summary>
        /// <param name="COLLECT_RESOURCE_ID"></param>
        /// <returns></returns>
        private async Task<bool> DeleteResource(decimal COLLECT_RESOURCE_ID)
        {
            try
            {
                string sql = @"DELETE FROM SFCS_COLLECT_RESOURCES  WHERE COLLECT_RESOURCE_ID=:COLLECT_COMPONENT_ID";
                return await _dbConnection.ExecuteAsync(sql, new { COLLECT_RESOURCE_ID }) > 0;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }




        /// <summary>
        /// 通过路径获取零件
        /// </summary>
        /// <param name="snID"></param>
        /// <param name="compName"></param>
        /// <param name="odmPN"></param>
        /// <param name="customerPN"></param>
        /// <param name="routeID"></param>
        /// <param name="orderNo"></param>
        /// <returns></returns>
        private async Task<List<SfcsCollectComponentsListModel>> GetCollectComponentsByRoute(decimal snID, string compName, string odmPN, string customerPN, decimal routeID, decimal orderNo)
        {
            try
            {
                string sql = @"SELECT * FROM SFCS_COLLECT_COMPONENTS WHERE SN_ID=:snID AND COMPONENT_NAME=:compName
                                                       AND ODM_COMPONENT_PN=:odmPN
                                                       AND PRODUCT_OPERATION_CODE IN ( SELECT DISTINCT PRODUCT_OPERATION_CODE FROM SFCS_ROUTE_CONFIG 
                                                       WHERE ROUTE_ID=:routeID AND ORDER_NO>=:orderNo )";
                var list = await _dbConnection.QueryAsync<SfcsCollectComponentsListModel>(sql, new { snID, compName, odmPN, routeID, orderNo });
                return list.ToList();
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }

        /// <summary>
        /// 根據製程获取数据
        /// </summary>
        /// <param name="snID"></param>
        /// <param name="resourceName"></param>
        /// <param name="routeID"></param>
        /// <param name="orderNo"></param>
        /// <returns></returns>
        private async Task<List<SfcsCollectResourcesListModel>> GetCollectResourceTableByRoute(decimal snID, string resourceName, decimal routeID, decimal orderNo)
        {
            try
            {
                string sql = @"SELECT * FROM SFCS_COLLECT_RESOURCES
                                WHERE SN_ID=:snID AND RESOURCE_NAME=:resourceName
                                AND PRODUCT_OPERATION_CODE IN  ( SELECT DISTINCT PRODUCT_OPERATION_CODE
                                FROM SFCS_ROUTE_CONFIG  WHERE ROUTE_ID=:routeID AND ORDER_NO>=:orderNo )";
                var list = await _dbConnection.QueryAsync<SfcsCollectResourcesListModel>(sql, new
                {
                    snID,
                    resourceName,
                    routeID,
                    orderNo
                });
                return list.ToList();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// 通过路径获取uid
        /// </summary>
        /// <param name="snID"></param>
        /// <param name="uidName"></param>
        /// <param name="routeID"></param>
        /// <param name="orderNo"></param>
        /// <returns></returns>
        private async Task<List<SfcsCollectUidsListModel>> GetCollectUIDByRoute(decimal snID, string uidName, decimal routeID, decimal orderNo)
        {
            try
            {
                string sql = @"SELECT * FROM SFCS_COLLECT_UIDS WHERE SN_ID=:SN_ID AND UID_NAME=:UID_NAME 
                                                       AND PRODUCT_OPERATION_CODE IN ( SELECT DISTINCT PRODUCT_OPERATION_CODE FROM SFCS_ROUTE_CONFIG 
                                                       WHERE ROUTE_ID=:ROUTE_ID AND ORDER_NO>=:ORDER_NO )";
                var list = await _dbConnection.QueryAsync<SfcsCollectUidsListModel>(sql, new
                {
                    SN_ID = snID,
                    UID_NAME = uidName,
                    ROUTE_ID = routeID,
                    ORDER_NO = orderNo
                });
                return list.ToList();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        /// <summary>
        /// 備份組件返工數據   JZMES_LOG.SFCS_COLLECT_COMPONENTS
        /// </summary>
        /// <param name="reworkOperationID"></param>
        /// <param name="reworkOperator"></param>
        /// <param name="siteID"></param>
        /// <param name="collectComponentID"></param>
        private async Task<bool> BackupComponentReworkData(decimal reworkOperationID, string reworkOperator,
            decimal siteID, decimal collectComponentID)
        {
            try
            {
                string sql = @"INSERT INTO JZMES_LOG.SFCS_COLLECT_COMPONENTS
                                SELECT SCO.COLLECT_COMPONENT_ID,SCO.OPERATION_ID,SCO.SN_ID,SCO.WO_ID,SCO.PRODUCT_OPERATION_CODE,
                                SCO.COMPONENT_ID,SCO.COMPONENT_NAME,SCO.ODM_COMPONENT_SN,SCO.ODM_COMPONENT_PN,SCO.CUSTOMER_COMPONENT_SN,
                                SCO.CUSTOMER_COMPONENT_PN,SCO.COMPONENT_QTY,SCO.SERIALIZED,SCO.COLLECT_SITE,SCO.COLLECT_TIME,
                                SCO.COLLECT_BY,SCO.REWORK_REMOVE_FLAG,SCO.REPLACE_FLAG,SCO.EDI_FLAG,SCO.ATTRIBUTE1,SCO.ATTRIBUTE2,
                                SCO.ATTRIBUTE3,SCO.ATTRIBUTE4,SCO.ATTRIBUTE5, SYSDATE,:REWORK_OPERATION_ID,:REWORK_OPERATOR,:OPERATION_SITE,SCO.DEVICE_FLAG,'' COMPONENT_LOCATION
                                FROM SFCS_COLLECT_COMPONENTS SCO WHERE COLLECT_COMPONENT_ID=:COLLECT_COMPONENT_ID";

                return await _dbConnection.ExecuteAsync(sql, new
                {
                    REWORK_OPERATION_ID = reworkOperationID,
                    REWORK_OPERATOR = reworkOperator,
                    OPERATION_SITE = siteID,
                    COLLECT_COMPONENT_ID = collectComponentID,
                }) > 0;
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }


        /// <summary>
        /// 清除附件對象數據
        /// </summary>
        private async void ClearAttachments(decimal reworkOperationID, string reworkOperator,
            decimal siteID, decimal collectObjectID)
        {
            bool addBack = await BackupAttachmentReworkData(reworkOperationID, reworkOperator, siteID, collectObjectID);

            bool delete = await DeleteAttachment(collectObjectID);
        }

        /// <summary>
        /// 返工時, 更新測試零件狀態為可用狀態,  --測試次數減1
        /// </summary>
        /// <param name="status"></param>
        /// <param name="componentSN"></param>
        /// <returns></returns>
        private async Task<bool> UpdateDeviceComponentWhenRework(decimal status, string componentSN)
        {
            try
            {
                string sql = @"UPDATE SFCS_DEVICE_COMPONENTS SET STATUS = :STATUS, UPDATE_TIME = SYSDATE  WHERE COMPONENT_SN = :COMPONENT_SN";
                return await _dbConnection.ExecuteAsync(sql, new { STATUS = status, COMPONENT_SN = componentSN }) > 0;
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }

        /// <summary>
        /// 備份附件數據 JZMES_LOG.SFCS_COLLECT_ATTACHMENTS 
        /// </summary>
        /// <param name="reworkOperationID"></param>
        /// <param name="reworkOperator"></param>
        /// <param name="siteID"></param>
        /// <param name="collectAttachmentID"></param>
        public async Task<bool> BackupAttachmentReworkData(decimal reworkOperationID, string reworkOperator,
            decimal siteID, decimal collectObjectID)
        {
            try
            {
                string sql = @"INSERT INTO JZMES_LOG.SFCS_COLLECT_ATTACHMENTS 
                                                         SELECT SCA.*, SYSDATE,:REWORK_OPERATION_ID,:REWORK_OPERATOR,:OPERATION_SITE FROM SFCS_COLLECT_ATTACHMENTS SCA
                                                         WHERE COLLECT_OBJECT_ID=:COLLECT_OBJECT_ID";
                return await _dbConnection.ExecuteAsync(sql, new
                {
                    REWORK_OPERATION_ID = reworkOperationID,
                    REWORK_OPERATOR = reworkOperator,
                    OPERATION_SITE = siteID,
                    COLLECT_OBJECT_ID = collectObjectID
                }) > 0;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        /// <summary>
        /// 刪除Components
        /// </summary>
        /// <param name="COLLECT_OBJECT_ID"></param>
        /// <returns></returns>
        private async Task<bool> DeleteAttachment(decimal COLLECT_OBJECT_ID)
        {
            try
            {
                string sql = @"DELETE FROM SFCS_COLLECT_ATTACHMENTS  WHERE COLLECT_OBJECT_ID=:COLLECT_OBJECT_ID";
                return await _dbConnection.ExecuteAsync(sql, new { COLLECT_OBJECT_ID }) > 0;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        /// <summary>
        /// 刪除Components
        /// </summary>
        /// <param name="COLLECT_COMPONENT_ID"></param>
        /// <returns></returns>
        private async Task<bool> DeleteComponent(decimal COLLECT_COMPONENT_ID)
        {
            try
            {
                string sql = @"DELETE FROM SFCS_COLLECT_COMPONENTS  WHERE COLLECT_COMPONENT_ID=:COLLECT_COMPONENT_ID";
                return await _dbConnection.ExecuteAsync(sql, new { COLLECT_COMPONENT_ID = COLLECT_COMPONENT_ID }) > 0;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }



        /// <summary>
        /// 刪除對象確認
        /// </summary>
        private async Task<List<SfcsCollectObjectsListMode>> FindCollectObjectsInOperation(List<SfcsRuncardListModel> runlist, SfcsReworkAddOrModifyModel remodel)
        {
            List<SfcsCollectObjectsListMode> list = new List<SfcsCollectObjectsListMode>();
            if (remodel.RETYPE != 3)
            {
                decimal orlist = 0, orilist = 0;
                if (remodel.ORDERNOLIST != null && remodel.ORDERNOLIST.Count > 0)
                    orlist = remodel.ORDERNOLIST[Convert.ToInt32(remodel.CHOOSEINDEX)];
                if (remodel.ORIGINALORDERNOLIST != null && remodel.ORIGINALORDERNOLIST.Count > 0)
                    orilist = remodel.ORIGINALORDERNOLIST[Convert.ToInt32(remodel.OLDCHOOSEINDEX)];
                //CS端是否删除资源没有显示，但默认是勾选的，所以要走这步操作
                if (true)
                {
                    if (remodel.RETYPE == 0 || remodel.ROUTE_ID == remodel.ORIGINALROUTEID)
                    {
                        foreach (var row in runlist)
                        {
                            list = list.Concat(await GetCollectComponentInOperation(row.SN, (decimal)remodel.ROUTE_ID, orlist)).ToList();
                        }
                    }
                    else
                    {
                        foreach (var row in runlist)
                        {
                            list = list.Concat(await GetCollectComponentInOperation(row.SN, (decimal)remodel.ORIGINALROUTEID, orilist)).ToList();
                        }
                    }
                }

                if (remodel.ISDELRESOURE)
                {
                    if (remodel.RETYPE == 0 || remodel.ROUTE_ID == remodel.ORIGINALROUTEID)
                    {
                        foreach (var row in runlist)
                        {
                            list = list.Concat(await GetCollectResourceInOperation(row.SN, (decimal)remodel.ROUTE_ID, orlist)).ToList();
                        }
                    }
                    else
                    {
                        foreach (var row in runlist)
                        {
                            list = list.Concat(await GetCollectResourceInOperation(row.SN, (decimal)remodel.ORIGINALROUTEID, orilist)).ToList();
                        }
                    }
                }
                if (remodel.ISDELUID)
                {
                    if (remodel.RETYPE == 0 || remodel.ROUTE_ID == remodel.ORIGINALROUTEID)
                    {
                        foreach (var row in runlist)
                        {
                            list = list.Concat(await GetCollectUIDInOperation(row.SN, (decimal)remodel.ROUTE_ID, orlist)).ToList();
                        }
                    }
                    else
                    {
                        foreach (var row in runlist)
                        {
                            list = list.Concat(await GetCollectUIDInOperation(row.SN, (decimal)remodel.ORIGINALROUTEID, orilist)).ToList();
                        }
                    }
                }
            }
            return list;
        }


        /// <summary>
        /// 獲取產品收集不良
        /// </summary>
        /// <param name="SN_ID"></param>
        /// <param name="REPAIR_FLAG"></param>
        /// <returns></returns>
        private async Task<List<SfcsCollectDefectsListModel>> GetCollectDefects(decimal SN_ID, string REPAIR_FLAG)
        {
            try
            {
                string sql = "SELECT * FROM SFCS_COLLECT_DEFECTS where  SN_ID=:SN_ID and REPAIR_FLAG=:REPAIR_FLAG ";
                var list = await _dbConnection.QueryAsync<SfcsCollectDefectsListModel>(sql, new { SN_ID, REPAIR_FLAG });
                return list.ToList();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// 确认Runcard TurnIN 返工规则
        /// </summary>
        /// <param name="remodel"></param>
        /// <param name="runlist"></param>
        /// <param name="RuncardFormat"></param>
        /// <returns></returns>
        private async Task<bool> CheckRuncardRule(List<SfcsRuncardListModel> runlist, SfcsReworkAddOrModifyModel remodel)
        {
            bool result = false;
            try
            {
                if (remodel.RETYPE == 0)
                {
                    foreach (var row in runlist)
                    {
                        if (row.STATUS == GlobalVariables.Shipped ||
                        !row.GG_NO.IsNullOrEmpty())
                        {
                            throw new Exception(string.Format(@"流水号{0}已经出货,不能WIP返工。", row.SN));
                        }
                        if (row.STATUS == GlobalVariables.TurnIN )//||row.WIP_OPERATION == GlobalVariables.EndOperation
                        {
                            throw new Exception(string.Format(@"流水号{0}已经存仓,不能WIP返工。", row.SN));
                        }
                        if (await CheckWorkOrderStatusCancel((decimal)row.WO_ID, ""))
                        {
                            throw new Exception(string.Format(@"工单在ERP中已经被取消或工单不存在。"));
                        }
                        if (row.STATUS == GlobalVariables.Fail)
                        {
                            throw new Exception(string.Format(@"流水号{0}已经Fail,不能WIP返工。", row.SN));
                        }
                        if (row.STATUS == GlobalVariables.Scrapped ||
                           row.STATUS == GlobalVariables.WipScrapped ||
                           row.STATUS == GlobalVariables.RepairScrapped)
                        {
                            throw new Exception(string.Format(@"流水号{0}已经报废，不能WIP返工。", row.SN));
                        }
                    }
                }
                else
                {
                    var workOrderRow = await GetWorkOrderTable(0, remodel.NEW_WORKNO);
                    if (!workOrderRow.IsNull() && workOrderRow.Count > 0)
                    {
                        if (remodel.RUNCARDFORMAT.IsNullOrEmpty())
                        {
                            throw new Exception(string.Format("料号{0}没有配置流水号格式，请找IE确认。", workOrderRow[0].PART_NO));
                        }
                        foreach (var row in runlist)
                        {
                            var tempWoRow = await GetWorkOrderTable((decimal)row.WO_ID, "");
                            if (!tempWoRow.IsNull() && tempWoRow.Count > 0)
                            {
                                if (tempWoRow[0].WO_NO == remodel.NEW_WORKNO)
                                {
                                    throw new Exception(string.Format(@"新返工单与流水号{0}旧工单一致,请使用另外一张返工单。", row.SN));
                                }
                                if (row.STATUS != GlobalVariables.TurnIN)
                                {
                                    throw new Exception(string.Format(@"非存仓后的产品{0},不能进行存仓返工。", row.SN));
                                }

                                if (!FormatChecker.FormatCheck(row.SN, remodel.RUNCARDFORMAT))
                                {
                                    throw new Exception(string.Format(@"流水号格式不匹配，请找IE确认。"));
                                }
                                //存倉後返工，當前sn不能有一筆待維修的不良
                                var collectDefects = await GetCollectDefects(row.ID, GlobalVariables.EnableN);
                                if (!collectDefects.IsNull() && collectDefects.Count > 0)
                                {
                                    throw new Exception(string.Format(@"流水号{0}上次强制存仓前是不良产品，请使用维修返工。", row.SN));
                                }
                            }
                        }
                    }
                    else
                    {
                        return false;
                    }
                }
                result = true;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return result;
        }

        /// <summary>
        /// 确认返工规则
        /// </summary>
        /// <returns></returns>
        private async Task<bool> CheckReworkRule(List<SfcsRuncardListModel> runlist, SfcsReworkAddOrModifyModel remodel, int SnListCount)
        {
            if (remodel.RETYPE == 0)
            {
               var resultData= (await CheckReworkRoute(runlist, remodel));
                if (!resultData)
                {
                    return false;
                }
                if (await CheckReworkToSMT(remodel))
                {
                    return false;
                }
            }
            else
            {
                var tempWork = await GetWorkOrderTable(0, remodel.NEW_WORKNO);
                if (!tempWork.IsNull())
                {
                    if (await CheckWorkOrderStatusCancel(tempWork[0].ID, ""))
                    {
                        throw new Exception("工单在ERP中已经被取消或工单不存在。");
                    }
                    var NewWorkOrderRow = await GetWorkOrderTable(0, remodel.NEW_WORKNO);
                    if (!NewWorkOrderRow.IsNull())
                    {
                        if (NewWorkOrderRow[0].INPUT_QTY + SnListCount > NewWorkOrderRow[0].TARGET_QTY)
                        {
                            throw new Exception(string.Format("工单{0}投入量已达目标产量,无法再投生产。", remodel.NEW_WORKNO));
                        }
                        //新工單不允許設定流水號
                        var rangerTable = await GetRuncardRangerTable(NewWorkOrderRow[0].ID);
                        if (!rangerTable.IsNull() && rangerTable.Count > 0)
                        {
                            throw new Exception(string.Format("工单{0}已设定流水号范围，不允许返入流水号。", NewWorkOrderRow[0].WO_NO));
                        }
                        if (await CheckReworkToSMT(remodel))
                        {
                            return false;
                        }
                    }
                }
            }
            return true;
        }

        /// <summary>
        /// 比對製程
        /// </summary>
        /// <returns></returns>
        private async Task<bool> CheckReworkRoute(List<SfcsRuncardListModel> runlist, SfcsReworkAddOrModifyModel remodel)
        {
            foreach (var row in runlist)
            {
                var configRouteRow = await GetRouteConfigDataTable((decimal)row.ROUTE_ID, (decimal)row.WIP_OPERATION);
                if (configRouteRow.IsNull()||configRouteRow.Count<=0)
                {
                    if (row.WIP_OPERATION == GlobalVariables.EndOperation)
                    {
                        return true;
                    }
                    throw new Exception(string.Format(@"产品当前路径在制程中不存在。"));
                }
                if (remodel.CHOOSEOPERATIONORDER > configRouteRow[0].ORDER_NO)
                {
                    throw new Exception(string.Format(@"流水号{0}返工制程选择错误，请检查。", row.SN));
                }
            }
            return true;
        }



        /// <summary>
        /// 檢查工單在WIP狀態
        /// </summary>
        /// <param name="ID"></param>
        /// <param name="WO_NO"></param>
        /// <returns></returns>
        private async Task<bool> CheckWorkOrderStatusCancel(decimal ID, string WO_NO)
        {
            try
            {
                string strwhere = " and 1=1 ";
                if (ID > 0)
                    strwhere += string.Format(@" and ID={0}", ID);
                if (!WO_NO.IsNullOrEmpty())
                    strwhere += string.Format(@" and WO_NO='{0}'", WO_NO);

                string sql = @"SELECT  * FROM SFCS_WO    WHERE   (WIP_STATUS_TYPE IN (3,4) OR WIP_STATUS_TYPE IS NULL OR TURNIN_TYPE='N')  " + strwhere;
                var list = await _dbConnection.QueryAsync<object>(sql);
                if (list.IsNull())
                    return true;
                else
                    return false;

            }
            catch (Exception ex)
            {
                throw ex;
            }

        }

        /// <summary>
        /// 檢查PCB工單是否返到T/U之前，即SMT段
        /// </summary>
        /// <returns></returns>
        private async Task<bool> CheckReworkToSMT(SfcsReworkAddOrModifyModel remodel)
        {
            if (remodel.PLANT_CODE == GlobalVariables.pcbCode)
            {
                if (remodel.CLASSIFICATION == GlobalVariables.PCBClassficationCode)
                {
                    var configRow = await GetRouteConfigDataTable((decimal)remodel.ROUTE_ID, GlobalVariables.TUOperation);
                    if (!configRow.IsNull() && configRow.Count > 0)
                    {
                        if (remodel.CHOOSEOPERATIONORDER <= configRow[0].ORDER_NO)
                        {
                            throw new Exception("后段工单的SN不能返工到SMT段作业。");
                        }
                    }
                }
            }
            return false;
        }

        /// <summary>
        /// 獲取RuncardRangerTable
        /// </summary>
        /// <param name="WO_ID"></param>
        /// <returns></returns>
        private async Task<List<SfcsRuncardRangerListModel>> GetRuncardRangerTable(decimal WO_ID)
        {
            try
            {
                string sql = @"SELECT  * FROM SFCS_RUNCARD_RANGER  WHERE  WO_ID=:WO_ID  ";
                var list = await _dbConnection.QueryAsync<SfcsRuncardRangerListModel>(sql, new { WO_ID });
                return list.ToList();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// 顯示操作歷史記錄
        /// </summary>
        /// <param name="message"></param>
        private async void ShowOperationHistoryMessage(SfcsReworkListModel model, string message)
        {
            model.OPERATIONHISTORY = string.Format("<{0:yyyy/MM/dd HH:mm:ss}<", DateTime.Now) + " " + message;
        }

        /// <summary>
        /// 辨識輸入數據
        /// </summary>
        /// <param name="inputData"></param>
        private async Task<List<SfcsRuncardListModel>> IdentifyDataInput(SfcsReworkRequestModel request, SfcsReworkListModel model)
        {
            try
            {
                var tempRuncardTable = await GetTempRuncardTable(request.SN);
                foreach (var row in tempRuncardTable)
                {
                    model.WORKORDERID = tempRuncardTable[0].WO_ID;

                    FindBasicInformation(request, model);

                    //if (request.SNLIST != null && request.SNLIST.IndexOf(row.SN) >= 0)
                    //{
                    //    throw new Exception("刷入重复数据");
                    //}
                    if (row.STATUS == GlobalVariables.WipScrapped ||
                    row.STATUS == GlobalVariables.Scrapped ||
                    row.STATUS == GlobalVariables.RepairScrapped ||
                    row.STATUS == GlobalVariables.Fail ||
                    row.STATUS == GlobalVariables.RepairIn)
                    {
                        throw new Exception(string.Format(" 当前流水号{0}状态不允许返工，请检查。", row.SN));
                    }
                    //檢查是否被hold不能返工
                    VerifyHoldProductRework(row.ID, row.SN);

                    model.SNLIST.Add(row.SN);
                }

                return tempRuncardTable;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// 公共获取tempRuncardTable方法
        /// </summary>
        /// <returns></returns>
        private async Task<List<SfcsRuncardListModel>> GetTempRuncardTable(string SN)
        {
            var tempRuncardTable = await GetRuncardDataTable("SN", SN);
            if (tempRuncardTable == null || tempRuncardTable.Count == 0)
            {
                //通過uid來辨識runcard sn
                tempRuncardTable = await GetRuncardDataTableByUID(SN, GlobalVariables.pcbCode);
            }
            if (tempRuncardTable.Count > 0)
            {
                var multiRuncardTable = await GetCollectMultiRuncardDataTable(SN, 1);

                if (!multiRuncardTable.IsNull() && multiRuncardTable.Count > 0)
                {
                    decimal collectMultiRuncardID = multiRuncardTable[0].ID;
                    tempRuncardTable = await GetMultiRuncardDataTable(collectMultiRuncardID, 1);
                }
            }
            else
            {
                tempRuncardTable = await GetRuncardDataTable("CARTON_NO", SN);
                if (tempRuncardTable == null || tempRuncardTable.Count == 0)
                {
                    tempRuncardTable = await GetRuncardDataTable("PALLET_NO", SN);
                    if (tempRuncardTable == null || tempRuncardTable.Count == 0)
                    {
                        throw new Exception(string.Format(@"无法辨识的数据:{0}", SN));
                    }
                }
            }
            return tempRuncardTable;
        }


        /// <summary>
        /// 獲取基本信息
        /// </summary>
        private async void FindBasicInformation(SfcsReworkRequestModel request, SfcsReworkListModel model)
        {
            var workOrderRow = await GetWorkOrderTable((decimal)model.WORKORDERID, "");
            if (!workOrderRow.IsNull() && workOrderRow.Count > 0)
            {
                model.PART_NO = workOrderRow[0].PART_NO;
                model.WO_NO = workOrderRow[0].WO_NO;
                model.ORIGINALROUTEID = workOrderRow[0].ROUTE_ID;
                model.PLANT_CODE = workOrderRow[0].PLANT_CODE;
                model.CLASSIFICATION = workOrderRow[0].CLASSIFICATION;

                var modelRow = await GetModelDataTable(workOrderRow[0].MODEL_ID);
                if (!modelRow.IsNull() && modelRow.Count > 0)
                {
                    model.MODEL = modelRow[0].MODEL;
                }
                var partNumberRow = await GetPartNumberDataTable(workOrderRow[0].PART_NO);

                var routeRow = await GetProductRouteDataTable(workOrderRow[0].ROUTE_ID);
                if (routeRow.IsNullOrEmpty() || routeRow.Count == 0)
                {
                    throw new Exception("返工SN的原制程已被删除,请联系IE确认!");

                }
                else
                {
                    model.ROUTENAME = routeRow[0].ROUTE_NAME;
                }
                if (request.RETYPE == 0)
                {
                    model.ROUTE_ID = workOrderRow[0].ROUTE_ID;
                    FindRouteConfig(request, model);
                }
            }
        }

        /// <summary>
        /// 顯示舊製程
        /// 當A返B時,製程發生變化,顯示舊製程中所有工序
        /// </summary>
        private void SetOldRouteControlVisible(SfcsReworkRequestModel request, SfcsReworkListModel model)
        {
            // 維修返工不需顯示這一部份，維修返工不執行數據清除
            //if (request.RETYPE == 3)
            //{
            //    return;
            //}
            if (model.ORIGINALROUTEID != model.ROUTE_ID)
            {
                FindOriginalRouteConfig(model);
            }
        }

        /// <summary>
        /// 查找舊製程工位列表
        /// </summary>
        private async void FindOriginalRouteConfig(SfcsReworkListModel model)
        {

            var RouteData = await GetProductRouteDataTable((decimal)model.ORIGINALROUTEID);
            if (!RouteData.IsNull() && RouteData.Count > 0)
            {
                model.ROUTENAME = RouteData[0].ROUTE_NAME;
            }
            var oldRouteTable = await GetRouteConfigDataTable((decimal)model.ORIGINALROUTEID, 0);
            if (oldRouteTable.IsNull() || oldRouteTable.Count == 0)
            {
                throw new Exception("旧制程无法找到配置。");
            }

            foreach (var row in oldRouteTable)
            {
                var currentOperationRow = await GetOperationDataTable(row.CURRENT_OPERATION_ID);
                if (!currentOperationRow.IsNull() && currentOperationRow.Count > 0)
                {
                    model.ORIGINALORDERNOLIST.Add(row.ORDER_NO);
                    SortedList<string, decimal> currentOperation = new SortedList<string, decimal>();
                    currentOperation.Add(currentOperationRow[0].OPERATION_NAME, currentOperationRow[0].ID);
                    model.CURRENTOPERATIONLIST.Add(currentOperation);
                }
            }
        }


        /// <summary>
        /// 查找製程配置
        /// </summary>
        private async void FindRouteConfig(SfcsReworkRequestModel request, SfcsReworkListModel model)
        {
            if (!request.NEW_WORKNO.IsNullOrEmpty())
            {
                var workOrderRow = await GetWorkOrderTable(0, request.NEW_WORKNO);
                if (!workOrderRow.IsNull())
                {
                    model.ROUTE_ID = workOrderRow[0].ROUTE_ID;
                    model.PART_NO = workOrderRow[0].PART_NO;
                    model.WO_NO = workOrderRow[0].WO_NO;
                    var modelRow = await GetModelDataTable(workOrderRow[0].MODEL_ID);
                    if (!modelRow.IsNull() && modelRow.Count > 0)
                    {
                        model.MODEL = modelRow[0].MODEL;
                    }
                    newWorkOrderRow = workOrderRow;
                }
                else
                {
                    throw new Exception(string.Format(@"系统不存在{0}工单。", request.NEW_WORKNO));
                }
            }
            if ((decimal)model.ROUTE_ID <= 0)
            {
                throw new Exception(string.Format(@"工单{0}对应的制程没有配置工序，请确认。", request.NEW_WORKNO));
            }
            var RouteData = await GetProductRouteDataTable((decimal)model.ROUTE_ID);
            if (!RouteData.IsNull() && RouteData.Count > 0)
            {
                model.ROUTENAME = RouteData[0].ROUTE_NAME;
            }

            var routeConfigTable = await GetRouteConfigDataTable((decimal)model.ROUTE_ID, 0);
            if (routeConfigTable.IsNull() || routeConfigTable.Count == 0)
            {
                throw new Exception(string.Format(@"工单{0}对应的制程没有配置工序，请确认。", request.NEW_WORKNO));
            }
            foreach (var item in routeConfigTable)
            {
                var currentOperationRow = await GetOperationDataTable(item.CURRENT_OPERATION_ID);
                if (!currentOperationRow.IsNull() && currentOperationRow.Count > 0)
                {
                    SortedList<string, decimal> currentOperation = new SortedList<string, decimal>();
                    currentOperation.Add(currentOperationRow[0].DESCRIPTION, currentOperationRow[0].ID);
                    model.CURRENTOPERATIONLIST.Add(currentOperation);

                    wipOperationList.Add(item.CURRENT_OPERATION_ID);
                }
                model.ORDERNOLIST.Add(item.ORDER_NO);
            }
        }

        /// <summary>
        /// 查找新工單對應料號的流水號格式
        /// </summary>
        private async void FindRuncardFormat(SfcsReworkListModel model)
        {
            var productConfigRow = await GetProductConfigDataTable(GlobalVariables.SNFormat, newWorkOrderRow[0].PART_NO);
            if (productConfigRow.IsNullOrEmpty())
            {
                throw new Exception(string.Format(@"料号{0}没有配置流水号格式，请找IE确认。", newWorkOrderRow[0].PART_NO));
            }
            model.RUNCARDFORMAT = productConfigRow[0].CONFIG_VALUE;
        }

        /// <summary>
        /// 獲取產品配置信息
        /// </summary>
        /// <param name="CONFIG_TYPE"></param>
        /// <param name="PART_NO"></param>
        /// <returns></returns>
        private async Task<List<SfcsProductConfigListModel>> GetProductConfigDataTable(decimal CONFIG_TYPE, string PART_NO)
        {
            try
            {
                string sql = "SELECT * FROM SFCS_PRODUCT_CONFIG where  CONFIG_TYPE=:CONFIG_TYPE and PART_NO=:PART_NO ";
                var list = await _dbConnection.QueryAsync<SfcsProductConfigListModel>(sql, new { CONFIG_TYPE, PART_NO });
                return list.ToList();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        /// <summary>
        /// 檢查hold產品不能返工
        /// </summary>
        /// <param name="serialNumber"></param>
        private async void VerifyHoldProductRework(decimal snID, string serialNumber)
        {
            var holdProductDetailDataTable = await GetHoldDetailTable(snID, 1, GlobalVariables.EnableY);
            if (!holdProductDetailDataTable.IsNull() && holdProductDetailDataTable.Count > 0)
            {

                string holdCause = await GetHoldCause(holdProductDetailDataTable);
                throw new Exception(string.Format(@"产品{0}已经被Hold住，不能返工", holdCause));
            }



        }

        /// <summary>
        /// 取得具體的鎖定原因
        /// </summary>
        /// <returns></returns>
        private async Task<string> GetHoldCause(List<SfcsHoldProductDetailListModel> holdProductDetailDataTable)
        {
            string holdCause = string.Empty;
            foreach (var item in holdProductDetailDataTable)
            {
                var holdProductHeaderRow = await GetAllHoldProductHeaderTable((decimal)item.HOLD_ID);
                if (!holdProductHeaderRow.IsNullOrEmpty() && holdProductHeaderRow.Count > 0)
                {
                    holdCause += "\r\n 单据号：" + holdProductHeaderRow[0].HOLD_NUMBER + ",锁定原因：" + holdProductHeaderRow[0].HOLD_CAUSE;
                }
            }
            return holdCause;
        }

        /// <summary>
        /// 獲取hold header表
        /// </summary>
        /// <param name="ID"></param>
        private async Task<List<SfcsHoldProductHeaderListModel>> GetAllHoldProductHeaderTable(decimal ID)
        {
            try
            {
                string sql = "SELECT * FROM SFCS_HOLD_PRODUCT_HEADER where  ID=:ID ";
                var list = await _dbConnection.QueryAsync<SfcsHoldProductHeaderListModel>(sql, new { ID });
                return list.ToList();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        /// <summary>
        /// 獲取hold detail表
        /// </summary>
        /// <param name="SN_ID"></param>
        /// <param name="HOLD_ACTION"></param>
        /// <param name="STATUS"></param>
        private async Task<List<SfcsHoldProductDetailListModel>> GetHoldDetailTable(decimal SN_ID, decimal HOLD_ACTION, string STATUS)
        {
            try
            {
                string sql = "SELECT * FROM SFCS_HOLD_PRODUCT_DETAIL where  SN_ID=:SN_ID and  HOLD_ACTION=:HOLD_ACTION and STATUS=:STATUS ";
                var list = await _dbConnection.QueryAsync<SfcsHoldProductDetailListModel>(sql, new { SN_ID, HOLD_ACTION, STATUS });
                return list.ToList();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// 獲取所有製程
        /// </summary>
        /// <param name="ID"></param>
        private async Task<List<SfcsRoutesListModel>> GetProductRouteDataTable(decimal ID)
        {
            try
            {
                string sql = "SELECT * FROM SFCS_ROUTES where   ID=:ID";
                var list = await _dbConnection.QueryAsync<SfcsRoutesListModel>(sql, new { ID });
                return list.ToList();
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }
        /// <summary>
        /// 根据ID获取工序信息
        /// </summary>
        /// <param name="ID"></param>
        /// <returns></returns>
        private async Task<List<SfcsOperationsListModel>> GetOperationDataTable(decimal ID)
        {
            try
            {
                string sql = "SELECT * FROM SFCS_OPERATIONS  where   ID=:ID ";
                var list = await _dbConnection.QueryAsync<SfcsOperationsListModel>(sql, new { ID });
                return list.ToList();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        /// <summary>
        /// 獲取WorkOrderTable
        /// </summary>
        /// <param name="ID"></param>
        /// <returns></returns>
        private async Task<List<SfcsWoListModel>> GetWorkOrderTable(decimal ID, string WO_NO)
        {
            try
            {
                string strwhere = " where 1=1 ";
                if (ID > 0)
                    strwhere += string.Format(@" and ID={0}", ID);
                if (!WO_NO.IsNullOrEmpty())
                    strwhere += string.Format(@" and WO_NO='{0}'", WO_NO);
                string sql = "SELECT * FROM SFCS_WO  " + strwhere;
                var list = await _dbConnection.QueryAsync<SfcsWoListModel>(sql);
                return list.ToList();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }



        /// <summary>
        /// 獲取連板RuncardDataTable
        /// </summary>
        /// <param name="collectID"></param>
        /// <param name="status"></param>
        private async Task<List<SfcsRuncardListModel>> GetMultiRuncardDataTable(decimal collectID, decimal status)
        {

            try
            {
                string sql = @" SELECT SR.* FROM SFCS_COLLECT_MULTI_RUNCARD SCM, SFCS_RUNCARD SR
                WHERE SCM.ID = :COLLECT_ID AND SCM.SN_ID = SR.ID AND SCM.STATUS = :STATUS ";
                var list = await _dbConnection.QueryAsync<SfcsRuncardListModel>(sql, new
                {
                    COLLECT_ID = collectID,
                    STATUS = status
                });
                return list.ToList();
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }



        /// <summary>
        /// 获取RuncardDataTable
        /// </summary>
        /// <param name="Field">列名</param>
        /// <param name="value">值</param>
        /// <returns></returns>
        private async Task<List<SfcsRuncardListModel>> GetRuncardDataTable(string Field, string Value)
        {
            try
            {
                string strwhere = " where 1=1 ";

                if (!Field.IsNullOrWhiteSpace() && !Value.IsNullOrWhiteSpace())
                    strwhere += string.Format(" and {0}='{1}'", Field, Value);
                else
                {
                    return null;
                }

                string sql = "SELECT * FROM SFCS_RUNCARD  " + strwhere;
                var list = await _dbConnection.QueryAsync<SfcsRuncardListModel>(sql);
                return list.ToList();
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        /// <summary>
        /// 根據UID獲取Runcard Table
        /// </summary>
        /// <param name="uidNumber"></param>
        /// <param name="plantCode"></param>
        private async Task<List<SfcsRuncardListModel>> GetRuncardDataTableByUID(string uidNumber, decimal plantCode)
        {
            //查找UID數據
            var collectUIDTable = await GetCollectUIDDataTable(0, "", uidNumber, plantCode);
            if (collectUIDTable == null || collectUIDTable.Count == 0)
            {
                //如果找不到UID數據則返回空
                return new List<SfcsRuncardListModel>();
            }
            else
            {
                //如果找到UID數據，根據UID Link的SN ID查找SN
                var collectUIDRow = collectUIDTable[0];
                decimal serialNumberID = (decimal)collectUIDRow.SN_ID;
                return await GetRuncardDataTable("ID", serialNumberID.ToString());
            }
        }



        /// <summary>
        /// 获取制程配置信息
        /// </summary>
        /// <returns></returns>
        private async Task<List<SfcsRouteConfigListModel>> GetRouteConfigDataTable(decimal ROUTE_ID, decimal CURRENT_OPERATION_ID)
        {
            try
            {
                string strwhere = " where 1=1 ";
                if (ROUTE_ID > 0)
                {
                    strwhere += string.Format(" and ROUTE_ID={0}", ROUTE_ID);
                }
                if (CURRENT_OPERATION_ID > 0)
                {
                    strwhere += string.Format(" and CURRENT_OPERATION_ID={0}", CURRENT_OPERATION_ID);
                }

                string sql = "SELECT * FROM SFCS_ROUTE_CONFIG   " + strwhere + " ORDER BY ORDER_NO "; ;
                var list = await _dbConnection.QueryAsync<SfcsRouteConfigListModel>(sql);
                return list.ToList();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        /// <summary>
        /// 獲取收集的UID
        /// </summary>
        /// <param name="UID_NAME"></param>
        /// <param name="UID_NUMBER"></param>
        /// <returns></returns>
        private async Task<List<SfcsCollectUidsListModel>> GetCollectUIDDataTable(decimal SN_ID, string UID_NAME, string UID_NUMBER, decimal PLANT_CODE)
        {
            try
            {
                string strwhere = " where 1=1 ";
                if (SN_ID > 0)
                    strwhere += string.Format(" and SN_ID={0}", SN_ID);
                if (!UID_NAME.IsNullOrEmpty())
                    strwhere += string.Format(" and UID_NAME='{0}'", UID_NAME);
                if (!UID_NUMBER.IsNullOrEmpty())
                    strwhere += string.Format(" and UID_NUMBER='{0}'", UID_NUMBER);
                if (PLANT_CODE > 0)
                    strwhere += string.Format(" and PLANT_CODE={0}", PLANT_CODE);

                string sql = "SELECT * FROM SFCS_COLLECT_UIDS    " + strwhere;
                var list = await _dbConnection.QueryAsync<SfcsCollectUidsListModel>(sql);
                return list.ToList();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// 獲取Collect Multi Runcard數據表
        /// </summary>
        /// <param name="SN_ID"></param>
        /// <param name="STATUS"></param>
        /// <returns></returns>
        private async Task<List<SfcsCollectMultiRuncardListModel>> GetCollectMultiRuncardDataTable(string SN, decimal STATUS)
        {
            try
            {
                string strwhere = " WHERE SCMR.SN_ID = SR.ID AND SCMR.WO_ID = SR.WO_ID ";
                if (!string.IsNullOrEmpty(SN))
                    strwhere += string.Format(" and SR.SN='{0}'", SN);
                if (STATUS > 0)
                    strwhere += string.Format(" and SCMR.STATUS={0}", STATUS);

                string sql = "select SCMR.* from SFCS_COLLECT_MULTI_RUNCARD SCMR, SFCS_RUNCARD SR    " + strwhere;
                var list = await _dbConnection.QueryAsync<SfcsCollectMultiRuncardListModel>(sql);
                return list.ToList();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// 获取ModelDataTable
        /// </summary>
        /// <param name="ID"></param>
        /// <returns></returns>
        private async Task<List<SfcsModelListModel>> GetModelDataTable(decimal ID)
        {
            try
            {
                string sql = "SELECT * FROM SFCS_MODEL  where ENABLED='Y' and  ID=:ID ";
                var list = await _dbConnection.QueryAsync<SfcsModelListModel>(sql, new { ID });
                return list.ToList();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// 获取PartNumberDataTable
        /// </summary>
        /// <param name="PART_NO"></param>
        /// <returns></returns>
        private async Task<List<SfcsPnListModel>> GetPartNumberDataTable(string PART_NO)
        {
            try
            {
                string sql = "SELECT * FROM SFCS_PN  where    PART_NO=:PART_NO ";
                var list = await _dbConnection.QueryAsync<SfcsPnListModel>(sql, new { PART_NO });
                return list.ToList();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }



        /// <summary>
        /// 根據選擇的工位順序獲取所有採集的資源對象
        /// </summary>
        /// <param name="INPUT_DATA"></param>
        /// <param name="ROUTE_ID"></param>
        /// <param name="ORDER_NO"></param>
        /// <returns></returns>
        private async Task<List<SfcsCollectObjectsListMode>> GetCollectResourceInOperation(string INPUT_DATA, decimal ROUTE_ID, decimal ORDER_NO)
        {
            try
            {
                string sql = @"SELECT SCR.RESOURCE_NAME OBJECT_NAME, '' ODM_PN,'' CUSTOMER_PN, 
                                                        COUNT(*) QTY,SCR.REWORK_REMOVE_FLAG, 
                                                        'RESOURCE' OBJECT_KIND FROM SFCS_COLLECT_RESOURCES SCR
                                                        WHERE SCR.SN_ID IN ( SELECT SR.ID FROM SFCS_RUNCARD SR WHERE (SN=:INPUT_DATA
                                                        OR CARTON_NO=:INPUT_DATA OR PALLET_NO=:INPUT_DATA) ) AND SCR.PRODUCT_OPERATION_CODE IN (
                                                        SELECT DISTINCT PRODUCT_OPERATION_CODE FROM SFCS_ROUTE_CONFIG
                                                        WHERE ROUTE_ID=:ROUTE_ID AND ORDER_NO>=:ORDER_NO ) 
                                                        GROUP BY SCR.RESOURCE_NAME, SCR.REWORK_REMOVE_FLAG ";
                var list = await _dbConnection.QueryAsync<SfcsCollectObjectsListMode>(sql, new { INPUT_DATA, ROUTE_ID, ORDER_NO });
                return list.ToList();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// 根據選擇的工位順序獲取所有採集的零件對象
        /// </summary>
        /// <param name="inputData"></param>
        /// <param name="routeID"></param>
        /// <param name="orderNo"></param>
        /// <returns></returns>
        private async Task<List<SfcsCollectObjectsListMode>> GetCollectComponentInOperation(string inputData, decimal routeID, decimal orderNo)
        {
            try
            {
                string sql = @"SELECT SCC.COMPONENT_NAME OBJECT_NAME, SCC.ODM_COMPONENT_PN ODM_PN, 
                                                       SCC.CUSTOMER_COMPONENT_PN CUSTOMER_PN, COUNT(*) QTY,
                                                       SCC.REWORK_REMOVE_FLAG, 'COMPONENT' OBJECT_KIND FROM SFCS_COLLECT_COMPONENTS SCC 
                                                       WHERE SCC.SN_ID IN ( SELECT SR.ID FROM SFCS_RUNCARD SR WHERE (SN=:INPUT_DATA
                                                       OR CARTON_NO=:INPUT_DATA OR PALLET_NO=:INPUT_DATA) ) AND SCC.PRODUCT_OPERATION_CODE IN (
                                                       SELECT DISTINCT PRODUCT_OPERATION_CODE FROM SFCS_ROUTE_CONFIG 
                                                       WHERE ROUTE_ID=:ROUTE_ID AND ORDER_NO>=:ORDER_NO ) GROUP BY SCC.COMPONENT_NAME, 
                                                       SCC.ODM_COMPONENT_PN, SCC.CUSTOMER_COMPONENT_PN, SCC.REWORK_REMOVE_FLAG";
                var list = await _dbConnection.QueryAsync<SfcsCollectObjectsListMode>(sql, new
                {
                    INPUT_DATA = inputData,
                    ROUTE_ID = routeID,
                    ORDER_NO = orderNo
                });
                return list.ToList();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        /// <summary>
        /// 根據選擇的工位順序獲取所有採集的零件對象
        /// </summary>
        /// <param name="INPUT_DATA"></param>
        /// <param name="ROUTE_ID"></param>
        /// <param name="ORDER_NO"></param>
        /// <returns></returns>
        private async Task<List<SfcsCollectObjectsListMode>> GetCollectUIDInOperation(string INPUT_DATA, decimal ROUTE_ID, decimal ORDER_NO)
        {
            try
            {
                string sql = @"SELECT SCU.UID_NAME OBJECT_NAME, '' ODM_PN,'' CUSTOMER_PN,   
                                                   COUNT(*) QTY, SCU.REWORK_REMOVE_FLAG,'UID' OBJECT_KIND 
                                                   FROM SFCS_COLLECT_UIDS SCU WHERE SCU.SN_ID IN ( 
                                                   SELECT SR.ID FROM SFCS_RUNCARD SR WHERE (SN=:INPUT_DATA
                                                   OR CARTON_NO=:INPUT_DATA OR PALLET_NO=:INPUT_DATA) )
                                                   AND SCU.PRODUCT_OPERATION_CODE IN (
                                                   SELECT DISTINCT PRODUCT_OPERATION_CODE FROM SFCS_ROUTE_CONFIG 
                                                   WHERE ROUTE_ID=:ROUTE_ID AND ORDER_NO>=:ORDER_NO ) 
                                                   GROUP BY SCU.UID_NAME,SCU.REWORK_REMOVE_FLAG ";
                var list = await _dbConnection.QueryAsync<SfcsCollectObjectsListMode>(sql, new { INPUT_DATA, ROUTE_ID, ORDER_NO });
                return list.ToList();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// 获取旧工单的SN列表（分页）
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public async Task<WoTransferListModel> GetSNDataByOldWoOrCartonNo(SfcsWoNeWorkRequestModel model)
        {
            int page = 0, limit = 0;
            page = model.Page * model.Limit - model.Limit + 1;
            limit = model.Page * model.Limit;
            model.Page = page;
            model.Limit = limit;

            WoTransferListModel rModel = new WoTransferListModel();
            String sWhere = "";
            if (model.WO_ID > GlobalVariables.DecimalDefaults)
            {
                sWhere += " AND WO_ID=:WO_ID ";
                rModel.OPERATIONHISTORY += string.Format("<{0:yyyy/MM/dd HH:mm:ss}<", DateTime.Now) + " " + model.Key + Environment.NewLine;
            }
            if (!String.IsNullOrEmpty(model.SN))
            {
                sWhere += " AND instr(SN, :SN) > 0";
                rModel.OPERATIONHISTORY += string.Format("<{0:yyyy/MM/dd HH:mm:ss}<", DateTime.Now) + " " + model.SN + Environment.NewLine;
            }
            if (!String.IsNullOrEmpty(model.CARTON_NO))
            {
                sWhere += " AND CARTON_NO=:CARTON_NO";
                rModel.OPERATIONHISTORY += string.Format("<{0:yyyy/MM/dd HH:mm:ss}<", DateTime.Now) + " " + model.CARTON_NO + Environment.NewLine;
            }

            String sQuery = String.Format("SELECT SN FROM (SELECT ROWNUM R, T.* FROM ( SELECT SN FROM SFCS_RUNCARD WHERE 1=1 {0} ORDER BY SN ASC) T) WHERE R BETWEEN :Page AND :Limit", sWhere);

            rModel.SNLIST = await _dbConnection.QueryAsync<String>(sQuery, model);
            sQuery = String.Format("SELECT COUNT(0) FROM SFCS_RUNCARD WHERE 1=1 {0}", sWhere);
            rModel.SNTotalCount = await _dbConnection.ExecuteScalarAsync<int>(sQuery, model);

            return rModel;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public async Task<int> SaveWoReplaceByType(SaveWoTransferListModel model, int t_type)
        {
            int pId = 0;
            ConnectionFactory.OpenConnection(_dbConnection);
            using (var tran = _dbConnection.BeginTransaction())
            {
                try
                {
                    String insertSql = @"INSERT INTO SFCS_WO_REPLACE (REPLACE_WO_ID,REPLACE_OPERATION_ID,SN_ID,OLD_WO_ID,NEW_WO_ID,PLANT_CODE,REPLACE_TYPE,REPLACE_SITE_ID,REPLACE_ORDER_NO,REPLACE_BY,REPLACE_TIME) VALUES (MES_SEQ_ID.NEXTVAL,SFCS_OPERATION_SEQ.NEXTVAL,:SN_ID,:OLD_WO_ID,:NEW_WO_ID,:PLANT_CODE,:REPLACE_TYPE,:REPLACE_SITE_ID,:REPLACE_ORDER_NO,:REPLACE_BY,SYSDATE)";

                    String updateRuncardSql = t_type == 1 ? "UPDATE SFCS_RUNCARD SET WO_ID=:WO_ID WHERE ID=:ID" : "UPDATE SFCS_RUNCARD SET WO_ID=:WO_ID,ROUTE_ID=:ROUTE_ID,WIP_OPERATION=:WIP_OPERATION,LAST_OPERATION=:LAST_OPERATION WHERE ID=:ID";

                    foreach (var item in model.WOREPLACELIST)
                    {
                        decimal count = await GetWoReplaceHistoryDataTable((decimal)item.SN_ID);
                        int r = await _dbConnection.ExecuteAsync(insertSql, new
                        {
                            SN_ID = item.SN_ID,
                            OLD_WO_ID = item.OLD_WO_ID,
                            NEW_WO_ID = item.NEW_WO_ID,
                            PLANT_CODE = item.PLANT_CODE,
                            REPLACE_TYPE = item.REPLACE_TYPE,
                            REPLACE_SITE_ID = item.REPLACE_SITE_ID,
                            REPLACE_ORDER_NO = count + 1,
                            REPLACE_BY = item.REPLACE_BY
                        }, tran);

                        if (r < 1) { throw new Exception("数据保存失败!"); }

                        r = await _dbConnection.ExecuteAsync(updateRuncardSql, new
                        {
                            ID = item.SN_ID,
                            WO_ID = item.NEW_WO_ID,
                            ROUTE_ID = model.ROUTE_ID,
                            WIP_OPERATION = model.WIP_OPERATION,
                            LAST_OPERATION = model.LAST_OPERATION
                        }, tran);
                        if (r < 1) { throw new Exception("数据保存失败!"); }
                    }

                    if (t_type == 1)
                    {
                        String insertPrintSql = @"INSERT INTO SFCS_PRINT_TASKS(ID,PRINT_FILE_ID,OPERATOR,CREATE_TIME,PRINT_STATUS,PRINT_DATA,CARTON_NO,PALLET_NO,PART_NO,WO_NO)VALUES(:ID,:PRINT_FILE_ID,:OPERATOR,SYSDATE,0,:PRINT_DATA,:CARTON_NO,:PALLET_NO,:PART_NO,:WO_NO)";
                        var id = await _dbConnection.ExecuteScalarAsync("SELECT SFCS_PRINT_TASKS_SEQ.NEXTVAL MY_SEQ FROM DUAL");
                        pId = Convert.ToInt32(id);

                        int r = await _dbConnection.ExecuteAsync(insertPrintSql, new
                        {
                            ID = pId,
                            PRINT_FILE_ID = model.PRINTTASKS.PRINT_FILE_ID,
                            WO_NO = model.PRINTTASKS.WO_NO,
                            PART_NO = model.PRINTTASKS.PART_NO,
                            CARTON_NO = model.PRINTTASKS.CARTON_NO,
                            PALLET_NO = model.PRINTTASKS.PALLET_NO,
                            PRINT_DATA = model.PRINT_DATA,
                            OPERATOR = model.PRINTTASKS.OPERATOR
                        }, tran);
                        if (r < 1) { throw new Exception("数据保存失败!"); }
                    }

                    tran.Commit();
                }
                catch (Exception ex)
                {
                    pId = -1;
                    tran.Rollback();
                    throw ex;
                }
                finally
                {
                    if (_dbConnection.State != System.Data.ConnectionState.Closed)
                    {
                        _dbConnection.Close();
                    }
                }
            }

            return pId;
        }
    }
}