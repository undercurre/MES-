/*
*┌──────────────────────────────────────────────────────────────┐
*│　描    述：接口实现                                                    
*│　作    者：嘉志科技                                            
*│　版    本：2.0    模板代码自动生成                                                
*│　创建时间：2020-04-22 09:40:14                             
*└──────────────────────────────────────────────────────────────┘
*┌──────────────────────────────────────────────────────────────┐
*│　命名空间： JZ.IMS.Repository.Oracle                                  
*│　类    名： SfcsHoldProductHeaderRepository                                      
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
using JZ.IMS.WebApi.Common;

namespace JZ.IMS.Repository.Oracle
{
    public class SfcsLockProductHeaderRepository : BaseRepository<SfcsHoldProductHeader, Decimal>, ISfcsLockProductHeaderRepository
    {
        public SfcsLockProductHeaderRepository() { }
        /// <summary>
        /// 管控動作
        /// </summary>
        public enum HoldOperation
        {
            HoldWIP,//不能流水生产作业
            HoldRework,//不能返工
            HoldRMA,//
            HoldAssemply,//不能组装
            HoldShip,//不能出货
            HoldTurnIn,//不能存仓
            HoldRepair//不能RMA收料
        }

        private string workOrder;
        public static string partNumber=null;
        private string model;
        private string carton;
        private string pallet;
        private string compPartNumber;
        public static decimal operationID=0;
        public static string BillID;

        public const string I_InsertHoldProductHeader = @"INSERT INTO SFCS_HOLD_PRODUCT_HEADER(ID,HOLD_NUMBER,MAIN_CONDITION,MAIN_VALUE,
                                                          SUBSIDIARY_CONDITION,SUBSIDIARY_VALUE,HOLD_ACTION,STATUS,HOLD_QTY,HOLD_CAUSE,
                                                          HOLD_EMPNO,ECN_NO) VALUES(:ID,:HOLD_NUMBER,:MAIN_CONDITION,:MAIN_VALUE,
                                                          :SUBSIDIARY_CONDITION,:SUBSIDIARY_VALUE,:HOLD_ACTION,:STATUS,:HOLD_QTY,:HOLD_CAUSE,:HOLD_EMPNO,:ECN_NO) ";
        public const string I_InsertHoldProductDetail = @"INSERT INTO SFCS_HOLD_PRODUCT_DETAIL(ID,HOLD_ID,SN_ID,COMPONENT_SN,
                                                          CUSTOMER_COMPONENT_PN,HOLD_ACTION,STATUS,PART_NO,WO_NO,OPERATION_LINE_ID,
                                                          OPERATION_SITE_ID,PRODUCT_OPERATION_CODE,CURRENT_OPERATION_ID,MODEL) 
                                                          VALUES (SFCS_HOLD_DETAIL_ID_SEQ.NEXTVAL,
                                                          :HOLD_ID,:SN_ID,:COMPONENT_SN,:CUSTOMER_COMPONENT_PN,:HOLD_ACTION,:STATUS,
                                                          :PART_NO,:WO_NO,:OPERATION_LINE_ID,:OPERATION_SITE_ID,:PRODUCT_OPERATION_CODE,
                                                          :CURRENT_OPERATION_ID,:MODEL) ";
        public SfcsLockProductHeaderRepository(IOptionsSnapshot<DbOption> options)
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
            string sql = "SELECT ENABLED FROM SFCS_HOLD_PRODUCT_HEADER WHERE ID=:ID";
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
            string sql = "UPDATE SFCS_HOLD_PRODUCT_HEADER set ENABLED=:ENABLED WHERE ID=:Id";
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
            string sql = "SELECT SFCS_HOLD_PRODUCT_HEADER_SEQ.NEXTVAL MY_SEQ FROM DUAL";
            var result = await _dbConnection.ExecuteScalarAsync(sql);
            return (decimal)result;
        }

        /// <summary>
		///项目是否已被使用 
		/// </summary>
		/// <param name="id">项目id</param>
		/// <returns></returns>
		public async Task<bool> ItemIsByUsed(decimal id)
        {
            string sql = "select count(0) from SFCS_HOLD_PRODUCT_HEADER where id = :id";
            object result = await _dbConnection.ExecuteScalarAsync(sql, new
            {
                id
            });

            return (Convert.ToInt32(result) > 0);
        }

        /// <summary>
        ///获取RuncardDataTable
        /// </summary>
        /// <param name="id">项目id</param>
        /// <returns></returns>
        public async Task<List<SfcsRuncard>> GetRuncardDataTable(string SN)
        {
            string sql = @"SELECT SR.* FROM SFCS_RUNCARD SR  where  SR.SN=:SN
							UNION
							SELECT SR.* FROM SFCS_RUNCARD SR,SFCS_RUNCARD_REPLACE SRR
							WHERE SR.ID = SRR.SN_ID  and SR.SN=:SN";
            var result = await _dbConnection.QueryAsync<SfcsRuncard>(sql, new
            {
                SN
            });

            return result?.ToList();
        }

        /// <summary>
        ///获取SfcsOperationHistory
        /// </summary>
        /// <param name="id">项目id</param>
        /// <returns></returns>
        public List<SfcsOperationHistory> GetLastBFTHistory(decimal? id)
        {
            string sql = @"SELECT SOH.*  FROM SFCS_OPERATION_HISTORY SOH, SFCS_OPERATIONS SO   where  SOH.SN_ID = :SN_ID AND SOH.SITE_OPERATION_ID = SO.ID
                                                                AND UPPER(SO.OPERATION_NAME) LIKE '%BFT%' ORDER BY SOH.OPERATION_TIME DESC ";
            var result = _dbConnection.Query<SfcsOperationHistory>(sql, new { SN_ID = id });
            return result?.ToList();
        }

        /// <summary>
        ///获取GetBatchHeaderDataTable
        /// </summary>
        /// <param name="id">项目id</param>
        /// <returns></returns>
        public List<SfcsTurninBatchHeader> GetBatchHeaderDataTable(string TURNIN_NO)
        {
            string sql = @"SELECT STH.* FROM SFCS_TURNIN_BATCH_HEADER STH where SUBINVENTORY_CODE='inventory' and BATCH_NO=:BATCH_NO ";
            var result = _dbConnection.Query<SfcsTurninBatchHeader>(sql, new { BATCH_NO = TURNIN_NO });
            return result?.ToList();
        }

        /// <summary>
        ///获取GetHoldBill
        /// </summary>
        /// <param name="id">项目id</param>
        /// <returns></returns>
        public async Task<string> GetHoldBill()
        {
            string sql = @"SELECT 'HB'||LPAD(SFCS_HOLD_HEADER_SEQ.NEXTVAL,15,'0') BATCH FROM DUAL ";
            var result = await _dbConnection.ExecuteScalarAsync(sql);
            return (string)result;
        }

        /// <summary>
        ///获取GetHoldID
        /// </summary>
        /// <param name="id">项目id</param>
        /// <returns></returns>
        public async Task<decimal> GetHoldID()
        {
            string sql = @"SELECT SFCS_HOLD_HEADER_ID_SEQ.NEXTVAL ID FROM DUAL ";
            var result = await _dbConnection.ExecuteScalarAsync(sql);
            return (decimal)result;
        }

        /// <summary>
        /// 辨識刷入數據
        /// </summary>
        /// <param name="data"></param>
        public async Task<List<SfcsRuncard>> IdentifyInputData(string data, List<SfcsRuncard> runcards, int actionSelectIndex)
        {
            this.carton = null;
            this.pallet = null;
            this.workOrder = null;
            this.model = null;
            this.compPartNumber = null;

            if (string.IsNullOrWhiteSpace(data))
            {
                return null;
            }

            runcards = await GetListByTableEX<SfcsRuncard>(" SR.* ", " SFCS_RUNCARD SR ", " And CARTON_NO=:CARTON_NO ", new { CARTON_NO = data });
            if (runcards.Count > 0)
            {
                this.carton = data;
                return runcards;
            }

            runcards = await GetListByTableEX<SfcsRuncard>(" SR.* ", " SFCS_RUNCARD SR ", " And PALLET_NO=:PALLET_NO ", new { PALLET_NO = data });
            if (runcards.Count > 0)
            {
                this.pallet = data;
                return runcards;
            }

            runcards = await GetListByTableEX<SfcsRuncard>(" DISTINCT SR.* ", " SFCS_RUNCARD SR, SFCS_WO SW, SFCS_MODEL SM ", " And SR.WO_ID = SW.ID AND SM.ID(+) = SW.MODEL_ID And SW.WO_NO=:WO_NO ", new { WO_NO = data });
            if (runcards.Count > 0)
            {
                this.workOrder = data;
                return runcards;
            }

            if (actionSelectIndex == (int)HoldOperation.HoldShip)
            {
                //不鎖定已出貨的SN  GlobalVariables.Shipped
                runcards = await GetListByTableEX<SfcsRuncard>(" DISTINCT SR.* ", " SFCS_RUNCARD SR, SFCS_WO SW, SFCS_MODEL SM ", " And SR.WO_ID = SW.ID AND SM.ID = SW.MODEL_ID and SW.PART_NO=:PART_NO AND SR.STATUS != 5 ", new { PART_NO = data });
                if (runcards.Count > 0)
                {
                    partNumber = data;
                    return runcards;
                }
            }
            else if (actionSelectIndex == (int)HoldOperation.HoldTurnIn)
            {
                //不鎖定已存倉和已出貨的SN
                runcards = await GetListByTableEX<SfcsRuncard>(" DISTINCT SR.* ", " SFCS_RUNCARD SR, SFCS_WO SW, SFCS_MODEL SM ", " And SR.WO_ID = SW.ID AND SM.ID = SW.MODEL_ID and SW.PART_NO=:PART_NO AND SR.STATUS != 5 AND SR.STATUS != 4 ", new { PART_NO = data });
                if (runcards.Count > 0)
                {
                    partNumber = data;
                    return runcards;
                }
            }
            else if (actionSelectIndex == (int)HoldOperation.HoldWIP)
            {
                //不鎖定已存倉和已出貨的SN
                runcards = await GetListByTableEX<SfcsRuncard>(" DISTINCT SR.* ", " SFCS_RUNCARD SR, SFCS_WO SW, SFCS_MODEL SM ", " And SR.WO_ID = SW.ID AND SM.ID = SW.MODEL_ID and SW.PART_NO=:PART_NO AND SR.STATUS != 5 AND SR.STATUS != 4 ", new { PART_NO = data });
                if (runcards.Count > 0)
                {
                    partNumber = data;
                    return runcards;
                }
            }
            else
            {
                runcards = await GetListByTableEX<SfcsRuncard>(" DISTINCT SR.* ", " SFCS_RUNCARD SR, SFCS_WO SW, SFCS_MODEL SM ", " And SR.WO_ID = SW.ID AND SM.ID = SW.MODEL_ID and SW.PART_NO=:PART_NO  ", new { PART_NO = data });

                if (runcards.Count > 0)
                {
                    partNumber = data;
                    return runcards;
                }
            }

            runcards = await GetListByTableEX<SfcsRuncard>(" DISTINCT SR.* ", " SFCS_RUNCARD SR, SFCS_WO SW, SFCS_MODEL SM ", " And SR.WO_ID = SW.ID AND SM.ID = SW.MODEL_ID and SM.MODEL=:MODEL  ", new { MODEL = data });
            if (runcards.Count > 0)
            {
                this.model = data;
                return runcards;
            }

            if (actionSelectIndex != (int)HoldOperation.HoldAssemply)
            {

                runcards = await GetListByTableEX<SfcsRuncard>("  SR.* ", " SFCS_RUNCARD SR, SFCS_COLLECT_COMPONENTS SCO ", " And SCO.SN_ID = SR.ID And SCO.CUSTOMER_COMPONENT_PN=:CUSTOMER_COMPONENT_PN  ", new { CUSTOMER_COMPONENT_PN = data });
                if (runcards.Count > 0)
                {
                    this.compPartNumber = data;
                    return runcards;
                }
            }

            return runcards;
        }

        /// <summary>
        /// 保存数据
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public async Task<decimal> SaveDataByTrans(SfcsHoldProductHeaderModel model)
        {
            int result = 1;
            ConnectionFactory.OpenConnection(_dbConnection);
            using (var tran = _dbConnection.BeginTransaction())
            {
                try
                {
                    //新增
                    string insertSql = @"insert into SFCS_HOLD_PRODUCT_HEADER 
					(ID,HOLD_NUMBER,MAIN_CONDITION,MAIN_VALUE,SUBSIDIARY_CONDITION,SUBSIDIARY_VALUE,HOLD_ACTION,STATUS,HOLD_QTY,HOLD_CAUSE,HOLD_EMPNO,RELEASE_CAUSE,RELEASE_EMPNO,HOLD_TIME,RELEASE_TIME,ECN_NO) 
					VALUES (:ID,:HOLD_NUMBER,:MAIN_CONDITION,:MAIN_VALUE,:SUBSIDIARY_CONDITION,:SUBSIDIARY_VALUE,:HOLD_ACTION,:STATUS,:HOLD_QTY,:HOLD_CAUSE,:HOLD_EMPNO,:RELEASE_CAUSE,:RELEASE_EMPNO,:HOLD_TIME,:RELEASE_TIME,:ECN_NO)";
                    if (model.InsertRecords != null && model.InsertRecords.Count > 0)
                    {
                        foreach (var item in model.InsertRecords)
                        {
                            var newid = await GetSEQID();
                            var resdata = await _dbConnection.ExecuteAsync(insertSql, new
                            {
                                ID = newid,
                                item.HOLD_NUMBER,
                                item.MAIN_CONDITION,
                                item.MAIN_VALUE,
                                item.SUBSIDIARY_CONDITION,
                                item.SUBSIDIARY_VALUE,
                                item.HOLD_ACTION,
                                item.STATUS,
                                item.HOLD_QTY,
                                item.HOLD_CAUSE,
                                item.HOLD_EMPNO,
                                item.RELEASE_CAUSE,
                                item.RELEASE_EMPNO,
                                item.HOLD_TIME,
                                item.RELEASE_TIME,
                                item.ECN_NO,

                            }, tran);
                        }
                    }
                    //更新
                    string updateSql = @"Update SFCS_HOLD_PRODUCT_HEADER set HOLD_NUMBER=:HOLD_NUMBER,MAIN_CONDITION=:MAIN_CONDITION,MAIN_VALUE=:MAIN_VALUE,SUBSIDIARY_CONDITION=:SUBSIDIARY_CONDITION,SUBSIDIARY_VALUE=:SUBSIDIARY_VALUE,HOLD_ACTION=:HOLD_ACTION,STATUS=:STATUS,HOLD_QTY=:HOLD_QTY,HOLD_CAUSE=:HOLD_CAUSE,HOLD_EMPNO=:HOLD_EMPNO,RELEASE_CAUSE=:RELEASE_CAUSE,RELEASE_EMPNO=:RELEASE_EMPNO,HOLD_TIME=:HOLD_TIME,RELEASE_TIME=:RELEASE_TIME,ECN_NO=:ECN_NO  
						where ID=:ID ";
                    if (model.UpdateRecords != null && model.UpdateRecords.Count > 0)
                    {
                        foreach (var item in model.UpdateRecords)
                        {
                            var resdata = await _dbConnection.ExecuteAsync(updateSql, new
                            {
                                item.ID,
                                item.HOLD_NUMBER,
                                item.MAIN_CONDITION,
                                item.MAIN_VALUE,
                                item.SUBSIDIARY_CONDITION,
                                item.SUBSIDIARY_VALUE,
                                item.HOLD_ACTION,
                                item.STATUS,
                                item.HOLD_QTY,
                                item.HOLD_CAUSE,
                                item.HOLD_EMPNO,
                                item.RELEASE_CAUSE,
                                item.RELEASE_EMPNO,
                                item.HOLD_TIME,
                                item.RELEASE_TIME,
                                item.ECN_NO,

                            }, tran);
                        }
                    }
                    //删除
                    string deleteSql = @"Delete from SFCS_HOLD_PRODUCT_HEADER where ID=:ID ";
                    if (model.RemoveRecords != null && model.RemoveRecords.Count > 0)
                    {
                        foreach (var item in model.RemoveRecords)
                        {
                            var resdata = await _dbConnection.ExecuteAsync(deleteSql, new
                            {
                                item.ID
                            }, tran);
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
        /// 保存 
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public async Task<List<string>> RuncardSave(List<SfcsRuncard> runcardTable, int mainConditionSelectIndex, string data, int subsidiaryConditionSelectIndex, string inventory, DateTime beginTime, DateTime endTime, int actionSelectIndex, string holdcause, string CurrentOperator)
        {
            int result = 1;
            List<string> newList = new List<string>();
            ConnectionFactory.OpenConnection(_dbConnection);
            using (var tran = _dbConnection.BeginTransaction())
            {
                try
                {
                    string billNumber = await GetHoldBill();
                    BillID = billNumber;
                    decimal holdID = await GetHoldID();
                    inventory = string.IsNullOrWhiteSpace(inventory) ? beginTime.ToString() + GlobalVariables.comma + endTime.ToString() : inventory;
                    var resdata = await _dbConnection.ExecuteAsync(I_InsertHoldProductHeader, new
                    {
                        ID = holdID,
                        HOLD_NUMBER = billNumber,
                        MAIN_CONDITION = mainConditionSelectIndex,
                        MAIN_VALUE = data,
                        SUBSIDIARY_CONDITION = subsidiaryConditionSelectIndex,
                        SUBSIDIARY_VALUE = inventory,
                        HOLD_ACTION = actionSelectIndex,
                        STATUS = GlobalVariables.EnableY,
                        HOLD_QTY = runcardTable.Count,
                        HOLD_CAUSE = holdcause,
                        HOLD_EMPNO = CurrentOperator,
                        ECN_NO = "",
                    }, tran);

                    foreach (var row in runcardTable)
                    {
                       var tempdata= await _dbConnection.ExecuteAsync(I_InsertHoldProductDetail, new
                        {
                            HOLD_ID = holdID,
                            SN_ID = row.ID,
                            COMPONENT_SN = string.Empty,
                            CUSTOMER_COMPONENT_PN = string.Empty,
                            HOLD_ACTION = actionSelectIndex,
                            STATUS = GlobalVariables.EnableY,
                            PART_NO = partNumber,
                            WO_NO = workOrder,
                            OPERATION_LINE_ID = -1,
                            OPERATION_SITE_ID = -1,
                            PRODUCT_OPERATION_CODE = -1,
                            CURRENT_OPERATION_ID = operationID,
                            MODEL = model
                        }, tran);
                        if (tempdata > 0 && resdata > 0)
                        {
                            newList.Add(row.SN.ToString());
                        }
                    }

                    tran.Commit();
                }
                catch (Exception ex)
                {
                    newList = null;
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
            return newList;
        }

        /// <summary>
        /// HOLD_QTY=1 save
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public async Task<decimal> HoldQTYSave(List<SfcsRuncard> runcardTable, int mainConditionSelectIndex, string data, int subsidiaryConditionSelectIndex, string inventory, DateTime beginTime, DateTime endTime, int actionSelectIndex, string holdcause, string CurrentOperator, int count)
        {
            int result = 1;
            ConnectionFactory.OpenConnection(_dbConnection);
            using (var tran = _dbConnection.BeginTransaction())
            {
                try
                {
                    string billNumber = await GetHoldBill();
                    BillID = billNumber;
                    decimal holdID = await GetHoldID();
                    inventory = string.IsNullOrWhiteSpace(inventory) ? beginTime.ToString() + GlobalVariables.comma + endTime.ToString() : inventory;
                    var resdata = await _dbConnection.ExecuteAsync(I_InsertHoldProductHeader, new
                    {
                        ID = holdID,
                        HOLD_NUMBER = billNumber,
                        MAIN_CONDITION = mainConditionSelectIndex,
                        MAIN_VALUE = data,
                        SUBSIDIARY_CONDITION = subsidiaryConditionSelectIndex,
                        SUBSIDIARY_VALUE = inventory,
                        HOLD_ACTION = actionSelectIndex,
                        STATUS = GlobalVariables.EnableY,
                        HOLD_QTY = count,
                        HOLD_CAUSE = holdcause,
                        HOLD_EMPNO = CurrentOperator,
                        ECN_NO = "",
                    }, tran);

                    var tempdata = await _dbConnection.ExecuteAsync(I_InsertHoldProductDetail, new
                    {
                        HOLD_ID = holdID,
                        SN_ID = -1,
                        COMPONENT_SN = string.Empty,
                        CUSTOMER_COMPONENT_PN = data,
                        HOLD_ACTION = actionSelectIndex,
                        STATUS = GlobalVariables.EnableY,
                        PART_NO = partNumber,
                        WO_NO = workOrder,
                        OPERATION_LINE_ID = -1,
                        OPERATION_SITE_ID = -1,
                        PRODUCT_OPERATION_CODE = -1,
                        CURRENT_OPERATION_ID = -1,
                        MODEL = model
                    }, tran);
                    //成功提示 {0} 锁定成功！row.SN
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
        /// //處理不能組裝此零件序號業務
        /// </summary>
        /// <param name="runcardTable"></param>
        /// <param name="mainConditionSelectIndex"></param>
        /// <param name="data"></param>
        /// <param name="subsidiaryConditionSelectIndex"></param>
        /// <param name="inventory"></param>
        /// <param name="beginTime"></param>
        /// <param name="endTime"></param>
        /// <param name="actionSelectIndex"></param>
        /// <param name="holdcause"></param>
        /// <param name="CurrentOperator"></param>
        /// <param name="count"></param>
        /// <returns></returns>
        public async Task<List<string>> HoldComponentQTYSave(List<SfcsRuncard> runcardTable, int mainConditionSelectIndex, string data, int subsidiaryConditionSelectIndex, string inventory, DateTime beginTime, DateTime endTime, int actionSelectIndex, string holdcause, string CurrentOperator, decimal count, List<string> compList)
        {
            int tempdata = 1;
            int resdata = 1;
            ConnectionFactory.OpenConnection(_dbConnection);
            List<string> newList = new List<string>();
            using (var tran = _dbConnection.BeginTransaction())
            {
                try
                {
                    string billNumber = await GetHoldBill();
                    BillID = billNumber;
                    decimal holdID = await GetHoldID();
                    inventory = string.IsNullOrWhiteSpace(inventory) ? beginTime.ToString() + GlobalVariables.comma + endTime.ToString() : inventory;
                    resdata = await _dbConnection.ExecuteAsync(I_InsertHoldProductHeader, new
                    {
                        ID = holdID,
                        HOLD_NUMBER = billNumber,
                        MAIN_CONDITION = mainConditionSelectIndex,
                        MAIN_VALUE = data,
                        SUBSIDIARY_CONDITION = subsidiaryConditionSelectIndex,
                        SUBSIDIARY_VALUE = inventory,
                        HOLD_ACTION = actionSelectIndex,
                        STATUS = GlobalVariables.EnableY,
                        HOLD_QTY = count,
                        HOLD_CAUSE = holdcause,
                        HOLD_EMPNO = CurrentOperator,
                        ECN_NO = "",
                    }, tran);

                    foreach (string item in compList)
                    {
                        tempdata = await _dbConnection.ExecuteAsync(I_InsertHoldProductDetail, new
                        {
                            HOLD_ID = holdID,
                            SN_ID = -1,
                            COMPONENT_SN = item,
                            CUSTOMER_COMPONENT_PN = string.Empty,
                            HOLD_ACTION = actionSelectIndex,
                            STATUS = GlobalVariables.EnableY,
                            PART_NO = partNumber,
                            WO_NO = workOrder,
                            OPERATION_LINE_ID = -1,
                            OPERATION_SITE_ID = -1,
                            PRODUCT_OPERATION_CODE = -1,
                            CURRENT_OPERATION_ID = -1,
                            MODEL = model
                        }, tran);
                        if (tempdata > 0 && resdata > 0)
                        {
                            newList.Add(item);
                        }
                    }

                    tran.Commit();
                }
                catch (Exception ex)
                {
                    newList = null;
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
            return newList;
        }

        /// <summary>
        /// 保存 ProductOperationSiteSave
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public async Task<decimal> ProductOperationSiteSave(List<SfcsRuncard> runcardTable, int mainConditionSelectIndex, string data, string operationSiteName, decimal? operationSiteID, int subsidiaryConditionSelectIndex, string inventory, DateTime? beginTime, DateTime? endTime, int actionSelectIndex, string holdcause, string CurrentOperator)
        {
            int result = 1;
            ConnectionFactory.OpenConnection(_dbConnection);
            using (var tran = _dbConnection.BeginTransaction())
            {
                try
                {
                    string billNumber = await GetHoldBill();
                    BillID = billNumber;
                    decimal holdID = await GetHoldID();
                    var resdata = await _dbConnection.ExecuteAsync(I_InsertHoldProductHeader, new
                    {
                        ID = holdID,
                        HOLD_NUMBER = billNumber,
                        MAIN_CONDITION = mainConditionSelectIndex,
                        MAIN_VALUE = data,
                        SUBSIDIARY_CONDITION = subsidiaryConditionSelectIndex,
                        SUBSIDIARY_VALUE = operationSiteName,
                        HOLD_ACTION = actionSelectIndex,
                        STATUS = GlobalVariables.EnableY,
                        HOLD_QTY = 1,
                        HOLD_CAUSE = holdcause,
                        HOLD_EMPNO = CurrentOperator,
                        ECN_NO = "",
                    }, tran);

                    var tempdata = await _dbConnection.ExecuteAsync(I_InsertHoldProductDetail, new
                    {
                        HOLD_ID = holdID,
                        SN_ID = -1,
                        COMPONENT_SN = string.Empty,
                        CUSTOMER_COMPONENT_PN = string.Empty,
                        HOLD_ACTION = actionSelectIndex,
                        STATUS = GlobalVariables.EnableY,
                        PART_NO = partNumber,
                        WO_NO = workOrder,
                        OPERATION_LINE_ID = -1,
                        OPERATION_SITE_ID = operationSiteID,
                        PRODUCT_OPERATION_CODE = 0,
                        CURRENT_OPERATION_ID = -1,
                        MODEL = model
                    }, tran);

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
    }
}