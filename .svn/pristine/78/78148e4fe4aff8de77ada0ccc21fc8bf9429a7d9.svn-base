/*
*┌──────────────────────────────────────────────────────────────┐
*│　描    述：接口实现                                                    
*│　作    者：嘉志科技                                            
*│　版    本：2.0    模板代码自动生成                                                
*│　创建时间：2020-06-20 10:43:03                             
*└──────────────────────────────────────────────────────────────┘
*┌──────────────────────────────────────────────────────────────┐
*│　命名空间： JZ.IMS.Repository.Oracle                                  
*│　类    名： SmtMsdRuncardRepository                                      
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
using JZ.IMS.Models.SmtMSD;
using JZ.IMS.Core.Utilities;

namespace JZ.IMS.Repository.Oracle
{
    public class SmtMsdRuncardRepository : BaseRepository<SmtMsdRuncard, String>, ISmtMsdRuncardRepository
    {


        public SmtMsdRuncardRepository(IOptionsSnapshot<DbOption> options)
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
            string sql = "SELECT ENABLED FROM SMT_MSD_RUNCARD WHERE ID=:ID";
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
            string sql = "UPDATE SMT_MSD_RUNCARD set ENABLED=:ENABLED WHERE ID=:Id";
            return await _dbConnection.ExecuteAsync(sql, new
            {
                ENABLED = status ? 'Y' : 'N',
                Id = id,
            });
        }

        /// <summary>
        /// SMT_MSD_OPERAT_HISTORY_SEQ
        /// </summary>
        /// <returns></returns>
		public async Task<decimal> GetMSDHistorySEQID()
        {
            string sql = "SELECT SMT_MSD_OPERAT_HISTORY_SEQ.NEXTVAL MY_SEQ FROM DUAL";
            var result = await _dbConnection.ExecuteScalarAsync(sql);
            return (decimal)result;
        }

        /// <summary>
        /// 壓入標準耗時，標準結束時間
        /// </summary>
        /// <param name="id"></param>
        /// <param name="standardEndTime"></param>
        public async Task AddStandardEndTime(decimal id, decimal standardEndTime)
        {
            string U_UpdateMSDOperationHistoryStandardEndTime = @"
            UPDATE   SMT_MSD_OPERATION_HISTORY
               SET   STANDARD_END_TIME = BEGIN_TIME + (:STANDARD_END_TIME/24)
             WHERE   ID = :ID ";
            var result = await _dbConnection.ExecuteAsync(U_UpdateMSDOperationHistoryStandardEndTime,
                new { ID = id, STANDARD_END_TIME = standardEndTime });
        }

        /// <summary>
        /// 获取表的序列
        /// </summary>
        /// <returns></returns>
        public async Task<decimal> GetSEQID()
        {
            string sql = "SELECT SMT_MSD_RUNCARD_SEQ.NEXTVAL MY_SEQ FROM DUAL";
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
            string sql = "select count(0) from SMT_MSD_RUNCARD where id = :id";
            object result = await _dbConnection.ExecuteScalarAsync(sql, new
            {
                id
            });

            return (Convert.ToInt32(result) > 0);
        }


        /// <summary>
        /// 判断料卷编号
        /// </summary>
        /// <param name="reel_id"></param>
        /// <returns></returns>
        public async Task<bool> IsMsdReel(string reel_id)
        {
            string cmd = @"SELECT IMR.*
							FROM IMS_MSD_R12 IMR,
								 IMS_PART IP,
								 IMS_REEL IR
							WHERE IR.PART_ID = IP.ID
							  AND IP.CODE = IMR.PART_CODE
							  AND IR.CODE = :REEL_ID
							  AND IMR.ENABLED = 'Y'";
            var result = await _dbConnection.ExecuteScalarAsync(cmd, new { REEL_ID = reel_id });
            return (Convert.ToInt32(result) > 0);
        }

        /// <summary>
        /// 保存数据
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public async Task<decimal> SaveDataByTrans(SmtMsdRuncardModel model)
        {
            int result = 1;
            ConnectionFactory.OpenConnection(_dbConnection);
            using (var tran = _dbConnection.BeginTransaction())
            {
                try
                {
                    //新增
                    string insertSql = @"insert into SMT_MSD_RUNCARD 
					(REEL_ID,CURRENT_ACTION,TEMPERATURE,HUMIDITY,TOTAL_OPEN_TIME,OPERATOR_BY,BEGIN_TIME,FLOOR_LIFE_END_TIME,LEVEL_CODE,THICKNESS,AREA) 
					VALUES (:REEL_ID,:CURRENT_ACTION,:TEMPERATURE,:HUMIDITY,0,:OPERATOR_BY,SYSDATE,,:FLOOR_LIFE_END_TIME,:LEVEL_CODE,:THICKNESS,:AREA)";
                    if (model.InsertRecords != null && model.InsertRecords.Count > 0)
                    {
                        foreach (var item in model.InsertRecords)
                        {

                            var resdata = await _dbConnection.ExecuteAsync(insertSql, new
                            {
                                item.REEL_ID,
                                item.CURRENT_ACTION,
                                item.TEMPERATURE,
                                item.HUMIDITY,
                                item.OPERATOR_BY,
                                item.FLOOR_LIFE_END_TIME,
                                item.LEVEL_CODE,
                                item.THICKNESS,
                                item.AREA,

                            }, tran);
                        }
                    }
                    //更新
                    string updateSql = @"Update SMT_MSD_RUNCARD set REEL_ID=:REEL_ID,CURRENT_ACTION=:CURRENT_ACTION,TEMPERATURE=:TEMPERATURE,HUMIDITY=:HUMIDITY,TOTAL_OPEN_TIME=:TOTAL_OPEN_TIME,STATUS=:STATUS,OPERATOR_BY=:OPERATOR_BY,BEGIN_TIME=:BEGIN_TIME,END_TIME=:END_TIME,FLOOR_LIFE_END_TIME=:FLOOR_LIFE_END_TIME,LEVEL_CODE=:LEVEL_CODE,THICKNESS=:THICKNESS,AREA=:AREA  
						where ID=:ID ";
                    if (model.UpdateRecords != null && model.UpdateRecords.Count > 0)
                    {
                        foreach (var item in model.UpdateRecords)
                        {
                            var resdata = await _dbConnection.ExecuteAsync(updateSql, new
                            {

                                item.REEL_ID,
                                item.CURRENT_ACTION,
                                item.TEMPERATURE,
                                item.HUMIDITY,
                                item.TOTAL_OPEN_TIME,
                                item.STATUS,
                                item.OPERATOR_BY,
                                item.BEGIN_TIME,
                                item.END_TIME,
                                item.FLOOR_LIFE_END_TIME,
                                item.LEVEL_CODE,
                                item.THICKNESS,
                                item.AREA,

                            }, tran);
                        }
                    }
                    //删除
                    string deleteSql = @"Delete from SMT_MSD_RUNCARD where ID=:ID ";
                    if (model.RemoveRecords != null && model.RemoveRecords.Count > 0)
                    {
                        foreach (var item in model.RemoveRecords)
                        {
                            var resdata = await _dbConnection.ExecuteAsync(deleteSql, new
                            {
                                item.REEL_ID
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
        /// 获取系统时间
        /// </summary>
        /// <returns></returns>
        public DateTime GetSystemTime()
        {
            string cmd = @"SELECT SYSDATE FROM DUAL";
            var result = _dbConnection.ExecuteScalar<DateTime>(cmd);
            return result;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <param name="reelCode"></param>
        /// <returns></returns>
        public async Task<int> InsertMSDOperationHistory(decimal id, string reelCode)
        {
            string cmd = @"  INSERT INTO SMT_MSD_OPERATION_HISTORY (ID,
                                                       REEL_ID,
                                                       ACTION_CODE,
                                                       TEMPERATURE,
                                                       HUMIDITY,
                                                       TOTAL_OPEN_TIME,
                                                       STATUS,
                                                       BEGIN_BY,
                                                       BEGIN_TIME,
                                                       ACTION_LOCATION,
                                                       LEVEL_CODE,
                                                       THICKNESS)
               SELECT   :ID,
                        REEL_ID,
                        CURRENT_ACTION,
                        TEMPERATURE,
                        HUMIDITY,
                        0 TOTAL_OPEN_TIME,
                        1,
                        OPERATOR_BY,
                        BEGIN_TIME,
                        AREA,
                        LEVEL_CODE,
                        THICKNESS
                 FROM   SMT_MSD_RUNCARD
                WHERE   REEL_ID = :REEL_ID ";
            var resdata = await _dbConnection.ExecuteAsync(cmd, new { ID = id, REEL_ID = reelCode });
            return resdata;
        }

        /// <summary>
        ///  Update FLOOR_LIFE_END_TIME
        /// </summary>
        /// <param name="ReelCode"></param>
        /// <param name="FloorLife"></param>
        /// <returns></returns>
        public async Task<int> UpdateFloorLifeEndTime(string ReelCode, decimal FloorLife)
        {
            string cmd = @"UPDATE   SMT_MSD_RUNCARD
               SET   FLOOR_LIFE_END_TIME = BEGIN_TIME + :FLOOR_LIFE
             WHERE   REEL_ID = :REEL_ID";
            var resdata = await _dbConnection.ExecuteAsync(cmd, new { REEL_ID = ReelCode, FLOOR_LIFE = FloorLife });
            return resdata;
        }

        /// <summary>
        ///  Update DelayFloorLifeEndTime
        /// </summary>
        /// <param name="ReelCode"></param>
        /// <param name="FloorLife"></param>
        /// <returns></returns>
        public async Task<int> DelayFloorLifeEndTime(string reelCode, double actionTime)
        {
            string cmd = @" UPDATE   SMT_MSD_RUNCARD
               SET   FLOOR_LIFE_END_TIME = FLOOR_LIFE_END_TIME + :ACTION_TIME
             WHERE   REEL_ID = :REEL_ID ";
            var resdata = await _dbConnection.ExecuteAsync(cmd, new { REEL_ID = reelCode, ACTION_TIME = actionTime });
            return resdata;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="reelCode"></param>
        /// <param name="beginTime"></param>
        /// <param name="totalOpenTime"></param>
        /// <param name="floorLifeEndTime"></param>
        /// <returns></returns>
        public async Task<int> UpdateFloorLifeAndBeginTime(string reelCode, DateTime beginTime, double totalOpenTime, DateTime floorLifeEndTime)
        {
            string cmd = @" UPDATE   SMT_MSD_RUNCARD
               SET   BEGIN_TIME = :BEGIN_TIME, TOTAL_OPEN_TIME = :TOTAL_OPEN_TIME, FLOOR_LIFE_END_TIME = :FLOOR_LIFE_END_TIME
             WHERE   REEL_ID = :REEL_ID ";
            var resdata = await _dbConnection.ExecuteAsync(cmd, new { REEL_ID = reelCode, BEGIN_TIME = beginTime, TOTAL_OPEN_TIME = totalOpenTime, FLOOR_LIFE_END_TIME = floorLifeEndTime });
            return resdata;
        }

        /// <summary>
        /// 取得物料資訊
        /// </summary>
        /// <param name="reelCode"></param>
        /// <returns></returns>
        public async Task<dynamic> GetMSDReelInfo(string reelCode)
        {

            string cmd = @"SELECT   '00003' CUSTOMER_CODE,
                     'MSL' CUSTOMER_NAME,
                     IV.CODE VENDOR_CODE,
                     IV.DESCRIPTION VENDOR_NAME,
                     IB.NAME BU,
                     ISU.CODE SIC_CODE,
                     IPT.CODE PART_NO,
                     IPT.DESCRIPTION PART_DESC,
                     IMP.CODE QVL_NO,
                     IR.ORIGINAL_QUANTITY,
                     IR.CODE BCD_KIT,
                     IR.CODE BCD_ID,
                      TO_CHAR(IR.DATE_CODE),
                     '0' REEL_STATUS,
                     IR.LOT_CODE,
                     IMP.DESCRIPTION QVL_DESC,
                     IR.REVISION,
                     (SELECT   CODE
                        FROM   IMS_REEL
                       WHERE   ID = IR.PARENT_ID)
                        ORIGINAL_REEL_CODE,
                     IM.CODE MAKER_CODE,
                     IM.NAME MAKER_NAME
              FROM   IMS_REEL IR,
                     IMS_VENDOR IV,
                     IMS_BU IB,
                     IMS_SUBINVENTORY ISU,
                     IMS_PART IPT,
                     IMS_MAKER_PART IMP,
                     IMS_MAKER IM
             WHERE       IR.VENDOR_ID = IV.ID
                     AND IR.PART_ID = IPT.ID
                     AND IR.MAKER_PART_ID = IMP.ID(+)
                     AND IMP.MAKER_ID = IM.ID(+)
                     AND IR.ORIGINAL_SIC_ID = ISU.ID(+)
                     AND ISU.BU_ID = IB.ID(+) 
                     AND IR.CODE =:REEL_CODE";
            var result = (await _dbConnection.QueryAsync<dynamic>(cmd, new { REEL_CODE = reelCode }))?.FirstOrDefault();
            return result;
        }

        /// <summary>
        /// 獲取MSD Floor Life
        /// </summary>
        /// <param name="levelCode"></param>
        /// <param name="thickness"></param>
        /// <param name="temperature"></param>
        /// <param name="humidity"></param>
        /// <returns></returns>
        public List<SmtMsdLevelRule> GetMSDFloorLife(string levelCode, decimal thickness, decimal temperature, decimal humidity)
        {
            string cmd = @"SELECT   *
              FROM   SMT_MSD_LEVEL_RULE   
             WHERE       LEVEL_CODE = :LEVEL_CODE
                     AND:THICKNESS > MIN_THICKNESS
                     AND:THICKNESS <= MAX_THICKNESS
                     AND TEMPERATURE = :TEMPERATURE 
                     AND HUMIDITY = :HUMIDITY ";
            return _dbConnection.Query<SmtMsdLevelRule>(cmd, new { LEVEL_CODE = levelCode, THICKNESS = thickness, TEMPERATURE = temperature, HUMIDITY = humidity })?.ToList();
        }


        /// <summary>
        /// 獲取SMT_MSD_RUNCARD
        /// </summary>
        /// <param name="REEL_ID"></param>
        /// <returns></returns>
        public List<SmtMsdRuncard> GetMSDRuncardDataTable(string REEL_ID)
        {
            string cmd = @" SELECT * FROM SMT_MSD_RUNCARD where REEL_ID=:REEL_ID";
            return _dbConnection.Query<SmtMsdRuncard>(cmd, new { REEL_ID = REEL_ID })?.ToList();

        }

        /// <summary>
        /// 獲取MSD烘烤標準
        /// </summary>
        /// <param name="levelCode"></param>
        /// <param name="thickness"></param>
        /// <param name="openTemperature"></param>
        /// <param name="overTime"></param>
        /// <param name="openHumidity"></param>
        /// <returns></returns>
        public async Task<List<SmtMsdBakeRule>> GetMSDBakeStandard(string levelCode, decimal thickness, decimal openTemperature,
            decimal openHumidity, decimal overTime)
        {
            string cmd = @"
            SELECT   *
              FROM   SMT_MSD_BAKE_RULE
             WHERE   LEVEL_CODE = :LEVEL_CODE
                     AND (:THICKNESS > MIN_THICKNESS AND :THICKNESS <= MAX_THICKNESS)
                     AND (:OPEN_TEMPERATURE > MIN_OPEN_TEMPERATURE
                          AND :OPEN_TEMPERATURE <= MAX_OPEN_TEMPERATURE)
                     AND (:OPEN_HUMIDITY > MIN_OPEN_HUMIDITY
                          AND :OPEN_HUMIDITY <= MAX_OPEN_HUMIDITY)
                     AND (:OVER_TIME > MIN_OVER_TIME AND :OVER_TIME <= MAX_OVER_TIME)
                     AND ENABLED = :ENABLED ";

           return (await _dbConnection.QueryAsync<SmtMsdBakeRule>(cmd, new
            {
                LEVEL_CODE = levelCode,
                THICKNESS = thickness,
                OPEN_TEMPERATURE = openTemperature,
                OPEN_HUMIDITY = openHumidity,
                OVER_TIME = overTime,
                ENABLED = 'Y'
            }))?.ToList();
        }

        public async Task<List<ImsReelInfoView>> GetReelInfoView(string _reelCode)
        {
            string cmd = @" SELECT * FROM IMS_REEL_INFO_VIEW  where CODE=:CODE";
           var result= (await _dbConnection.QueryAsync<ImsReelInfoView>(cmd, new { CODE = _reelCode }))?.ToList();
            return result;
        }

        /// <summary>
        /// 获取操作记录
        /// </summary>
        /// <param name="REEL_ID"></param>
        /// <param name="ACTION_CODE"></param>
        /// <returns></returns>
        public List<SmtMsdOperationHistory> GetMSDOperationHistoryDataTable(string REEL_ID, decimal ACTION_CODE)
        {
            string cmd = @"SELECT * FROM SMT_MSD_OPERATION_HISTORY WHERE REEL_ID=:REEL_ID AND ACTION_CODE=:ACTION_CODE ORDER BY ID DESC ";
            return _dbConnection.Query<SmtMsdOperationHistory>(cmd, new { REEL_ID = REEL_ID, ACTION_CODE = ACTION_CODE })?.ToList();
        }
       

        /// <summary>
        /// 更新結束動作的歷史記錄
        /// </summary>
        /// <param name="reelCode"></param>
        /// <param name="operatorBy"></param>
        /// <param name="area"></param>
        public async Task LogFinishActionHistory(string reelCode, string operatorBy, decimal actionCode)
        {
            string cmd = "SELECT * FROM SMT_MSD_OPERATION_HISTORY WHERE REEL_ID=:REEL_ID";
            var historyTable = (await _dbConnection.QueryAsync<SmtMsdOperationHistory>(cmd, new { REEL_ID = reelCode }))?.ToList();
            if (historyTable.Count > 0)
            {
                string updateCmd = @"UPDATE   SMT_MSD_OPERATION_HISTORY
               SET   STATUS = 0,
                     END_BY = :END_BY,
                     ACTUAL_END_TIME = SYSDATE
             WHERE   ID = :ID ";
                var maxid = historyTable.Where(f => f.ACTION_CODE == actionCode).Max(f => f.ID);
                var result = await _dbConnection.ExecuteAsync(updateCmd, new { ID = maxid, END_BY = operatorBy });
            }
            else
            {
                // 從舊系統導入的Reel，因初始導入時沒有歷史記錄，所以第一次操作時需參考MSD_RUNCARD產生首筆歷史記錄
                string insercmd = @"INSERT INTO SMT_MSD_OPERATION_HISTORY (ID,
                                                       REEL_ID,
                                                       ACTION_CODE,
                                                       TEMPERATURE,
                                                       HUMIDITY,
                                                       TOTAL_OPEN_TIME,
                                                       STATUS,
                                                       BEGIN_BY,
                                                       BEGIN_TIME,
                                                       END_BY,
                                                       ACTUAL_END_TIME,
                                                       ACTION_LOCATION,
                                                       LEVEL_CODE,
                                                       THICKNESS)
               SELECT   SMT_MSD_OPERAT_HISTORY_SEQ.NEXTVAL,
                        REEL_ID,
                        CURRENT_ACTION,
                        TEMPERATURE,
                        HUMIDITY,
                        0 TOTAL_OPEN_TIME,
                        0,
                        OPERATOR_BY,
                        BEGIN_TIME,
                        :END_BY,
                        SYSDATE,
                        AREA,
                        LEVEL_CODE,
                        THICKNESS
                 FROM   SMT_MSD_RUNCARD
                WHERE   REEL_ID = :REEL_ID";
                await _dbConnection.ExecuteAsync(insercmd, new { ID = reelCode, END_BY = operatorBy });

            }
        }

        /// <summary>
        /// 清除開封時間
        /// </summary>
        /// <param name="reelCode"></param>
        public async Task ClearOpenTime(string reelCode)
        {
            string U_UpdateOpenTime = @"
            UPDATE   SMT_MSD_RUNCARD
               SET   TOTAL_OPEN_TIME = 0
             WHERE   REEL_ID = :REEL_ID ";
            await _dbConnection.ExecuteAsync(U_UpdateOpenTime, new { REEL_ID = reelCode });

        }

        /// <summary>
        /// 更新Total Open Time
        /// </summary>
        /// <param name="reelCode"></param>
        public async Task UpdateTotalOpenTime(string reelCode)
        {
            string U_UpdateMSDRuncardTotalOpenTime = @"
            UPDATE   SMT_MSD_RUNCARD
               SET   TOTAL_OPEN_TIME = ROUND(NVL (TOTAL_OPEN_TIME, 0) + (SYSDATE - BEGIN_TIME) * 24, 3)
             WHERE   REEL_ID = :REEL_ID AND CURRENT_ACTION = 100 ";

            string U_UpdateHistoryTotalOpenTime = @"
            UPDATE   SMT_MSD_OPERATION_HISTORY
               SET   TOTAL_OPEN_TIME =
                        ROUND ( (NVL (ACTUAL_END_TIME, SYSDATE) - BEGIN_TIME) * 24, 3)
             WHERE   ID = :ID ";
            string Q_HistoryTable = "SELECT * FROM SMT_MSD_OPERATION_HISTORY WHERE REEL_ID=:REEL_ID and ACTION_CODE=:ACTION_CODE";
            await _dbConnection.ExecuteAsync(U_UpdateMSDRuncardTotalOpenTime, new { REEL_ID = reelCode });

            var historyTable = (await _dbConnection.QueryAsync(Q_HistoryTable, new { REEL_ID = reelCode, ACTION_CODE = (decimal)MSDAction.Open }))?.ToList();
            if (historyTable.Count > 0)
            {
                var max = historyTable.Max(f => f.ID);
                await _dbConnection.ExecuteAsync(U_UpdateHistoryTotalOpenTime, new { ID = max });
            }
        }

        /// <summary>
        /// 通过子Reel获取父Reel的IMS_Reel Table
        /// </summary>
        /// <param name="childReelCode"></param>
        /// <returns></returns>
        public async Task<List<ImsReel>> GetParentReelByChild(string childReelCode)
        {
            string cmd = @"
            SELECT   R2.*
              FROM   IMS_REEL R1, IMS_REEL R2
             WHERE   R1.CODE = :CHILD_REEL_CODE AND R1.PARENT_ID = R2.ID ";
            var result = (await _dbConnection.QueryAsync<ImsReel>(cmd, new { CHILD_REEL_CODE = childReelCode }))?.ToList();
            return result;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="reelCode"></param>
        /// <param name="actualEndTime"></param>
        /// <param name="parentReelHistoryID"></param>
        /// <returns></returns>
        public async Task<bool> CopyHistoryFromParentReel(string reelCode, DateTime actualEndTime, decimal parentReelHistoryID)
        {
            string cmd = @"
INSERT INTO SMT_MSD_OPERATION_HISTORY (ID,
                                           REEL_ID,
                                           ACTION_CODE,
                                           TEMPERATURE,
                                           HUMIDITY,
                                           TOTAL_OPEN_TIME,
                                           STATUS,
                                           BEGIN_BY,
                                           BEGIN_TIME,
                                           END_BY,
                                           ACTUAL_END_TIME,
                                           STANDARD_END_TIME,
                                           ACTION_LOCATION,
                                           LEVEL_CODE,
                                           THICKNESS)
   SELECT SMT_MSD_OPERAT_HISTORY_SEQ.NEXTVAL,
          :REEL_ID,
          ACTION_CODE,
          TEMPERATURE,
          HUMIDITY,
          TOTAL_OPEN_TIME,
          2,  --STATUS
          BEGIN_BY,
          BEGIN_TIME,
          END_BY,
          :ACTUAL_END_TIME,
          STANDARD_END_TIME,
          ACTION_LOCATION,
          LEVEL_CODE,
          THICKNESS
     FROM SMT_MSD_OPERATION_HISTORY
    WHERE ID = :ID ";
           var result= (await _dbConnection.ExecuteAsync(cmd, new { REEL_ID = reelCode, ACTUAL_END_TIME = actualEndTime, ID = parentReelHistoryID }));
           return (result > 0);
        }


        /// <summary>
        /// 获取Reel的创建时间(切分Reel的创建时间相当于切分时间)
        /// </summary>
        /// <param name="reelCode"></param>
        /// <returns></returns>
        public async Task<DateTime> GetReelCreateTime(string reelCode)
        {
             string cmd = @"
            SELECT   B.OPERATING_TIME
              FROM   {0}_LOG.IMS_REEL R, SYS_BILL B
             WHERE       R.ENABLE_BILL_ID = B.ID
                     AND R.VERSION = 1
                     AND R.CODE = :CODE ".FormatWith("JZMES");

            var result = await _dbConnection.ExecuteScalarAsync<DateTime>(cmd, new { CODE = reelCode });
            return result;
        }

        public async Task CreateMSDRuncard(bool msdRuncardExist, string reelCode, decimal currentAction,
            decimal temperature, decimal humidity, string operatorBy, string levelCode, decimal thickness, string area)
        {
            if (!msdRuncardExist)
            {
                string I_InsertMSDRuncard = @"
            INSERT INTO SMT_MSD_RUNCARD (REEL_ID,
                                             CURRENT_ACTION,
                                             TEMPERATURE,
                                             HUMIDITY,
                                             OPERATOR_BY,
                                             BEGIN_TIME,
                                             TOTAL_OPEN_TIME,
                                             LEVEL_CODE,
                                             THICKNESS,
                                             AREA)
              VALUES   (:REEL_ID,
                        :CURRENT_ACTION,
                        :TEMPERATURE,
                        :HUMIDITY,
                        :OPERATOR_BY,
                        SYSDATE,
                        0,
                        :LEVEL_CODE,
                        :THICKNESS,
                        :AREA) ";
                // Insert
                await _dbConnection.ExecuteAsync(I_InsertMSDRuncard, new
                {
                    CURRENT_ACTION = currentAction,
                    TEMPERATURE = temperature,
                    HUMIDITY = humidity,
                    OPERATOR_BY = operatorBy,
                    AREA = area,
                    REEL_ID = reelCode,
                    LEVEL_CODE = levelCode,
                    THICKNESS = thickness
                });

            }
            else
            {
                string U_UpdateMSDRuncard = @"
            UPDATE   SMT_MSD_RUNCARD
               SET   CURRENT_ACTION = :CURRENT_ACTION,
                     TEMPERATURE = :TEMPERATURE,
                     HUMIDITY = :HUMIDITY,
                     OPERATOR_BY = :OPERATOR_BY,
                     BEGIN_TIME = SYSDATE,
                     END_TIME = NULL,
                     AREA = :AREA
             WHERE   REEL_ID = :REEL_ID ";
                // Update
                await _dbConnection.ExecuteAsync(U_UpdateMSDRuncard, new
                {
                    CURRENT_ACTION = currentAction,
                    TEMPERATURE = temperature,
                    HUMIDITY = humidity,
                    OPERATOR_BY = operatorBy,
                    AREA = area,
                    REEL_ID = reelCode
                });

            }
        }


        /// <summary>
        /// MSD結束管控
        /// </summary>
        /// <param name="reelCode"></param>
        /// <param name="operatorBy"></param>
        /// <param name="area"></param>
        /// <returns></returns>
        public async Task<int> MSDActionEnd(string reelCode, string operatorBy, string area) 
        {
            string U_UpdateActionEnd = @"
            UPDATE   SMT_MSD_RUNCARD
               SET   CURRENT_ACTION = 103,
                     BEGIN_TIME = SYSDATE,
                     END_TIME = SYSDATE,
                     FLOOR_LIFE_END_TIME = NULL,
                     OPERATOR_BY = :OPERATOR_BY,
                     AREA = :AREA
             WHERE   REEL_ID = :REEL_ID ";
          return await _dbConnection.ExecuteAsync(U_UpdateActionEnd, new { REEL_ID = reelCode, OPERATOR_BY = operatorBy, AREA = area });
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="reelCode"></param>
        /// <param name="temperature"></param>
        /// <param name="humidity"></param>
        /// <returns></returns>
        public async Task<int> LogActionEndHistory(string reelCode, decimal temperature, decimal humidity)
        {
            string I_InsertActionEndHistory = @"
            INSERT INTO SMT_MSD_OPERATION_HISTORY (ID,
                                                       REEL_ID,
                                                       ACTION_CODE,
                                                       TEMPERATURE,
                                                       HUMIDITY,
                                                       TOTAL_OPEN_TIME,
                                                       STATUS,
                                                       BEGIN_BY,
                                                       BEGIN_TIME,
                                                       END_BY,
                                                       ACTUAL_END_TIME,
                                                       ACTION_LOCATION,
                                                       LEVEL_CODE,
                                                       THICKNESS)
               SELECT   SMT_MSD_OPERAT_HISTORY_SEQ.NEXTVAL,
                        REEL_ID,
                        CURRENT_ACTION,
                        :TEMPERATURE,
                        :HUMIDITY,
                        0 TOTAL_OPEN_TIME,
                        0,
                        OPERATOR_BY,
                        END_TIME,
                        OPERATOR_BY,
                        END_TIME,
                        AREA,
                        LEVEL_CODE,
                        THICKNESS
                 FROM   SMT_MSD_RUNCARD
                WHERE   REEL_ID = :REEL_ID ";
          return  await _dbConnection.ExecuteAsync(I_InsertActionEndHistory,new { REEL_ID = reelCode , TEMPERATURE = temperature , HUMIDITY = humidity });
        }


        /// <summary>
        ///  更新MSD操作區域
        /// </summary>
        /// <param name="reelCode"></param>
        /// <param name="area"></param>
        /// <param name="operater"></param>
        /// <returns></returns>
        public async Task<int> UpdateMSDArea(string reelCode, string area, string operater)
        {
            string U_UpdateMSDArea = @"
            UPDATE   SMT_MSD_RUNCARD
               SET   AREA = :AREA, OPERATOR_BY = :OPERATOR_BY
             WHERE   REEL_ID = :REEL_ID ";

          return await _dbConnection.ExecuteAsync(U_UpdateMSDArea, new { REEL_ID= reelCode, AREA= area, OPERATOR_BY= operater });
                
        }

        public async Task<int> LogTransferAfterOpenHistory(string reelCode, decimal actionCode)
        {
            string I_InsertTransferAfterOpenHistory = @"
INSERT INTO SMT_MSD_OPERATION_HISTORY (ID,
                                           REEL_ID,
                                           ACTION_CODE,
                                           TEMPERATURE,
                                           HUMIDITY,
                                           TOTAL_OPEN_TIME,
                                           STATUS,
                                           BEGIN_BY,
                                           BEGIN_TIME,
                                           END_BY,
                                           ACTUAL_END_TIME,
                                           ACTION_LOCATION,
                                           LEVEL_CODE,
                                           THICKNESS)
   SELECT SMT_MSD_OPERAT_HISTORY_SEQ.NEXTVAL,
          REEL_ID,
          :CURRENT_ACTION,
          TEMPERATURE,
          HUMIDITY,
          0 TOTAL_OPEN_TIME,
          0,
          OPERATOR_BY,
          SYSDATE,
          OPERATOR_BY,
          SYSDATE,
          AREA,
          LEVEL_CODE,
          THICKNESS
     FROM SMT_MSD_RUNCARD
    WHERE REEL_ID = :REEL_ID ";
         return await  _dbConnection.ExecuteAsync(I_InsertTransferAfterOpenHistory,new { REEL_ID = reelCode , CURRENT_ACTION = actionCode });
        }
    }
}
