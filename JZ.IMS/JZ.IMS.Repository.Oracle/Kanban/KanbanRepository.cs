using JZ.IMS.Core.DbHelper;
using JZ.IMS.Core.Options;
using JZ.IMS.Core.Repository;
using JZ.IMS.IRepository;
using JZ.IMS.Models;
using Dapper;
using Microsoft.Extensions.Options;
using System;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Text;
using System.Data;
using JZ.IMS.ViewModels;
using System.Linq;
using JZ.IMS.Core.Extensions;

namespace JZ.IMS.Repository.Oracle
{
    public class KanbanRepository : IKanbanRepository
    {
        protected DbOption _dbOption;
        protected IDbConnection _dbConnection;

        public KanbanRepository(IOptionsSnapshot<DbOption> options)
        {
            _dbOption = options.Get("iWMS");
            if (_dbOption == null)
            {
                throw new ArgumentNullException(nameof(DbOption));
            }
            _dbConnection = ConnectionFactory.CreateConnection(_dbOption.DbType, _dbOption.ConnectionString);
        }

        #region PCBA/SMT共用
        /// <summary>
        /// 获取每小时产能数据
        /// </summary>
        /// <param name="lineId">产线ID</param>
        /// <param name="topCount">查询数据量</param>
        /// <param name="lineType">产线类型 PCBA/SMT </param>
        /// <returns></returns>
        public async Task<IEnumerable<HourYidldModel>> GetHourYidldDataAsync(int lineId, int topCount, string lineType)
        {
            DateTime date = DateTime.Now;

            string endTime;
            string beginTime;
            if (lineType == "SMT")
            {
                beginTime = date.AddHours(-topCount + 1).ToString("yyyy-MM-dd HH:00:00");
                endTime = date.ToString("yyyy-MM-dd HH:00:00");
            }
            else
            {
                if (date.Hour < 8)
                    date = DateTime.Parse((date.AddDays(-1)).ToString("yyyy-MM-dd 21:00:00"));

                beginTime = date.AddHours(-topCount + 1).ToString("yyyy-MM-dd HH:00:00");
                endTime = date.ToString("yyyy-MM-dd HH:00:00");
            }

            //beginTime = "2019-11-22 08:00:00";
            //endTime = "2019-11-22 16:00:00";

            string sql = @"SELECT * FROM V_MES_HOUR_YIDLD
                        WHERE LINE_TYPE=:LINE_TYPE AND LINE_ID = :OPERATION_LINE_ID AND WORK_TIME >= TO_DATE(:BEGINTIME,'yyyy-MM-dd HH24:mi:ss') AND WORK_TIME <= TO_DATE(:ENDTIME,'yyyy-MM-dd HH24:mi:ss')
                         ORDER BY WORK_TIME,PART_NO,DTL_START_MINUTE";

            return await _dbConnection.QueryAsync<HourYidldModel>(sql, new { OPERATION_LINE_ID = lineId, BEGINTIME = beginTime, ENDTIME = endTime, LINE_TYPE = lineType });
        }

        /// <summary>
        /// 获取抽检良率
        /// </summary>
        /// <param name="lineId">产线id</param>
        /// <param name="wo_no">工单号</param>
        /// <returns></returns>
        public async Task<KanBanSpotCheckModel> GetKanbanSpotCheckDataAsync(int lineId, string wo_no, int topCount)
        {
            //string sql = "SELECT * FROM V_SPOTCHECK_RATE WHERE LINE_ID= :LINE_ID AND WO_NO = :WO_NO";

            string mstSql = @" SELECT LINE_TYPE,LINE_ID,
										 WO_NO,
										 SUM (CHECK_QTY) AS CHECK_QTY,
										 SUM (FAIL_QTY) AS FAIL_QTY
									FROM MES_SPOTCHECK_HEADER
								   WHERE STATUS = 3 AND LINE_ID = :LINE_ID AND WO_NO = :WO_NO
								GROUP BY LINE_TYPE,LINE_ID, WO_NO";

            var result = (await _dbConnection.QueryAsync<KanBanSpotCheckModel>(mstSql, new { LINE_ID = lineId, WO_NO = wo_no })).FirstOrDefault();

            if (result == null)
                return new KanBanSpotCheckModel();

            //string sql = @"SELECT tab2.* FROM 
            //				   (  SELECT HEADER.LINE_ID,
            //							 HEADER.WO_NO,
            //							 FDETAIL.DEFECT_CODE,
            //							 FDETAIL.DEFECT_LOC,
            //							 FDETAIL.DEFECT_DESCRIPTION,
            //							 COUNT (1) AS DEFECTQTY
            //						FROM MES_SPOTCHECK_FAIL_DETAIL FDETAIL
            //							 INNER JOIN MES_SPOTCHECK_DETAIL DETAIL
            //								ON FDETAIL.SPOTCHECK_DETAIL_ID = DETAIL.ID
            //							 INNER JOIN MES_SPOTCHECK_HEADER HEADER
            //								ON DETAIL.BATCH_NO = HEADER.BATCH_NO
            //					   WHERE     HEADER.STATUS = 3
            //							 AND HEADER.LINE_ID = :LINE_ID
            //							 AND HEADER.WO_NO = :WO_NO
            //					GROUP BY HEADER.LINE_ID,
            //							 HEADER.WO_NO,
            //							 FDETAIL.DEFECT_CODE,
            //							 FDETAIL.DEFECT_LOC,
            //							 FDETAIL.DEFECT_DESCRIPTION
            //					) TAB2
            //					 WHERE ROWNUM <= :TOPCOUNT ORDER BY TAB2.DEFECTQTY DESC,TAB2.DEFECT_LOC";

            string sql = @"SELECT tab2.*
						  FROM (  SELECT MST.BATCH_NO,
										 MST.WO_NO,
										 MST.CHECK_QTY,
										 MST.CHECKER,
										 DETAIL.CREATOR,
										 DETAIL.CREATE_TIME,
										 MST.CREATE_DATE,
										 FAIL.DEFECT_LOC,
										 FAIL.DEFECT_DESCRIPTION
									FROM MES_SPOTCHECK_HEADER mst
										 LEFT JOIN MES_SPOTCHECK_DETAIL detail
											ON     mst.BATCH_NO = detail.BATCH_NO
											   AND detail.STATUS = 'PASS'
										 LEFT JOIN MES_SPOTCHECK_FAIL_DETAIL fail
											ON detail.id = FAIL.SPOTCHECK_DETAIL_ID
								   WHERE MST.STATUS = 3 AND MST.LINE_ID = :LINE_ID AND mst.WO_NO = :WO_NO
								ORDER BY MST.BATCH_NO DESC, DETAIL.CREATE_TIME DESC) TAB2
						 WHERE 1=1 ";

            if (topCount > 0)
                sql += " and ROWNUM <= :TOPCOUNT";

            result.DetailData = (await _dbConnection.QueryAsync<KanBanSpotCheckDefectDetail>(sql, new { LINE_ID = lineId, WO_NO = wo_no, TOPCOUNT = topCount })).ToList();

            return result;
        }

        public async Task<IEnumerable<dynamic>> GetKanbanPassRateDataAsync(int lineId, string lineType)
        {
            string sql = "";
            if (lineType == "SMT")
            {
                sql = "SELECT * FROM V_SMT_KANBAN_APASS_RATE WHERE OPERATION_LINE_ID = :LINE_ID";
            }
            else
            {
                sql = "SELECT '直通率' TYPE,OPERATION_LINE_ID,PASS,TOTAL,RATE FROM SFCS_KANBAN_PASS_RATE WHERE OPERATION_LINE_ID = :LINE_ID";
            }
            return (await _dbConnection.QueryAsync(sql, new { LINE_ID = lineId })).ToList();
        }
        #endregion

        #region 产线看板

        /// <summary>
        /// 检查线体是否存在
        /// </summary>
        /// <param name="lineId"></param>
        /// <returns></returns>
        public async Task<bool> CheckLineAsync(int lineId)
        {
            var result = await _dbConnection.ExecuteScalarAsync<object>("SELECT COUNT(ID) FROM SFCS_OPERATION_LINES where ENABLED = 'Y' AND ID = " + lineId);
            if (Convert.ToInt32(result) > 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        /// <summary>
        /// 获取产线的工单信息
        /// </summary>
        /// <param name="lineId">产线id</param>
        /// <returns></returns>
        public async Task<IEnumerable<KanbanWoModel>> GetKanbanWoDataAsync(int lineId)
        {
            var p = new DynamicParameters();
            p.Add(":V_LINE_ID", lineId);
            await _dbConnection.ExecuteAsync("SYNC_SFCS_KANBAN_WO", p, commandType: CommandType.StoredProcedure);
            return await _dbConnection.QueryAsync<KanbanWoModel>("SELECT * FROM SFCS_KANBAN_WO WHERE OPERATION_LINE_ID = " + lineId);
        }

        /// <summary>
        /// 获取产线的站点统计信息
        /// </summary>
        /// <param name="lineId">产线id</param>
        /// <returns></returns>
        public async Task<IEnumerable<KanbanSiteModel>> GetKanbanSiteDataAsync(int lineId, string where = "")
        {
            var p = new DynamicParameters();
            p.Add(":V_LINE_ID", lineId);
            await _dbConnection.ExecuteAsync("SYNC_SFCS_KANBAN_SITE", p, commandType: CommandType.StoredProcedure);
            return await _dbConnection.QueryAsync<KanbanSiteModel>("SELECT * FROM SFCS_KANBAN_SITE WHERE OPERATION_LINE_ID = " + lineId + where);
        }

        /// <summary>
        /// 获取产线FCT工序的合格率
        /// </summary>
        /// <param name="lineId">产线id</param>
        /// <returns></returns>
        public async Task<IEnumerable<KanbanPassRateModel>> GetKanbanPassRateDataAsync(int lineId)
        {
            var p = new DynamicParameters();
            p.Add(":V_LINE_ID", lineId);
            await _dbConnection.ExecuteAsync("SYNC_SFCS_KANBAN_PASS_RATE", p, commandType: CommandType.StoredProcedure);
            return await _dbConnection.QueryAsync<KanbanPassRateModel>("SELECT * FROM SFCS_KANBAN_PASS_RATE WHERE OPERATION_LINE_ID = " + lineId);
        }

        /// <summary>
        /// 获取产线的呼叫信息
        /// </summary>
        /// <param name="lineId">产线id</param>
        /// <param name="day">最近今天的呼叫数据</param>
        /// <param name="topCount">查询前X条</param>
        /// <returns></returns>
        public async Task<IEnumerable<AndonCallModel>> GetCallDataAsync(int lineId, int day = 3, int topCount = 5)
        {
            var p = new DynamicParameters();
            p.Add(":V_LINE_ID", lineId);
            p.Add(":V_DAY", day);
            p.Add(":V_TOP_COUNT", topCount);

            await _dbConnection.ExecuteAsync("SYNC_SFCS_KANBAN_CALL", p, commandType: CommandType.StoredProcedure);
            return await _dbConnection.QueryAsync<AndonCallModel>("SELECT * FROM SFCS_KANBAN_CALL WHERE OPERATION_LINE_ID = " + lineId + " ORDER BY STATUS ASC,ID DESC");
        }

        public async Task<IEnumerable<TopDefectModel>> GetTopDefectDataAsync(int lineId, int topCount = 5)
        {
            var p = new DynamicParameters();
            p.Add(":V_LINE_ID", lineId);
            p.Add(":V_TOP_COUNT", topCount);

            await _dbConnection.ExecuteAsync("SYNC_SFCS_KANBAN_TOP_DEFECT", p, commandType: CommandType.StoredProcedure);
            return await _dbConnection.QueryAsync<TopDefectModel>("SELECT * FROM SFCS_KANBAN_TOP_DEFECT WHERE OPERATION_LINE_ID = " + lineId + " ORDER BY RANK_ID ASC");
        }

        /// <summary>
        /// 获取产线最近X小时的每小时产能
        /// </summary>
        /// <param name="lineId"></param>
        /// <param name="topCount"></param>
        /// <returns></returns>
        public async Task<IEnumerable<HourYidldModel>> GetKanbanHourYidldDataAsync(int lineId, int topCount)
        {
            var p = new DynamicParameters();
            p.Add(":V_LINE_ID", lineId);
            p.Add(":V_TOP_COUNT", topCount);

            await _dbConnection.ExecuteAsync("SYNC_SFCS_KANBAN_HOUR_YIDLD", p, commandType: CommandType.StoredProcedure);
            string sql = @"SELECT OPERATION_LINE_ID,OPERATION_LINE_NAME,WO_ID,WO_NO,PART_NO,MODEL,WORK_TIME,OUTPUT_QTY,round(STANDARD_CAPACITY,2) STANDARD_CAPACITY,REPORT_ID,STATUS,REPORT_CONTENT,REASON,RATE,round(STANDARD_CAPACITY_WORK,2) STANDARD_CAPACITY_WORK
                           FROM SFCS_KANBAN_HOUR_YIDLD WHERE OPERATION_LINE_ID = " + lineId + " ORDER BY WORK_TIME ASC";
            return await _dbConnection.QueryAsync<HourYidldModel>(sql);
        }

        /// <summary>
        /// 获取产线最近X小时的每小时产能（优化）(无效)
        /// </summary>
        /// <param name="lineId"></param>
        /// <param name="topCount"></param>
        /// <param name="date"></param>
        /// <returns></returns>
        //public async Task<IEnumerable<HourYidldModel>> GetKanbanHourYidldDataNewAsync(int lineId, int topCount, string date)
        //{
        //	//string sql = @"SELECT * FROM (select HY.*,CASE HY.WORK_TIME WHEN  TO_DATE( TO_CHAR(SYSDATE,'yyyy-MM-dd HH24')||':00:00','yyyy-MM-dd HH24:mi:ss') THEN -2 ELSE NVL(MR.STATUS,-1) END AS STATUS,
        //	//                         MR.REPORT_CONTENT,MR.REASON from MES_KANBAN_HOUR_YIDLD hy 
        //	//                         LEFT JOIN MES_MONITORING_REPORT mr ON MR.LINE_TYPE='PCBA' AND REPORT_TYPE='HOUR_YIELD' AND hy.LINE_ID = MR.LINE_ID 
        //	//                         AND MR.WO_NO = HY.WO_NO AND HY.WORK_TIME = MR.WORK_TIME 
        //	//                     WHERE HY.LINE_TYPE='PCBA' and  HY.LINE_ID = :OPERATION_LINE_ID  and HY.WORK_TIME >= TO_DATE(:WORK_TIME,'yyyy-MM-dd HH24:mi:ss')
        //	//                     ORDER BY HY.WORK_TIME DESC,HY.PART_NO ASC) WHERE ROWNUM <= :TOPCOUNT ORDER BY WORK_TIME ASC";

        //	string sql = @"  SELECT *
        //					FROM (  SELECT HY.*,
        //								   CASE HY.WORK_TIME
        //									  WHEN TO_DATE (
        //											  TO_CHAR (SYSDATE, 'yyyy-MM-dd HH24') || ':00:00',
        //											  'yyyy-MM-dd HH24:mi:ss')
        //									  THEN
        //										 -2
        //									  ELSE
        //										 NVL (MR.STATUS, -1)
        //								   END
        //									  AS STATUS,
        //								   MR.REPORT_CONTENT,
        //								   MR.REASON,
        //								   ROUND (HY.OUTPUT_QTY / hy.STANDARD_CAPACITY_WORK * 100, 2)
        //									  AS RATE
        //							  FROM (  SELECT LINE_ID,
        //											 WORK_TIME,
        //											 SUM (NVL (OUTPUT_QTY, 0)) AS OUTPUT_QTY,
        //											 SUM (NVL (STANDARD_CAPACITY, 0)) AS STANDARD_CAPACITY,
        //											 SUM (NVL (STANDARD_CAPACITY_WORK, 0))
        //												AS STANDARD_CAPACITY_WORK,
        //											 MAX (REPORT_ID) AS REPORT_ID
        //										FROM MES_KANBAN_HOUR_YIDLD
        //									   WHERE     LINE_TYPE = 'PCBA'
        //											 AND LINE_ID = :OPERATION_LINE_ID
        //											 AND WORK_TIME >=
        //													TO_DATE ( :WORK_TIME, 'yyyy-MM-dd HH24:mi:ss')
        //									GROUP BY LINE_ID, WORK_TIME) hy
        //								   LEFT JOIN MES_MONITORING_REPORT mr ON mr.ID = hy.REPORT_ID
        //						  ORDER BY hy.WORK_TIME DESC)
        //				   WHERE ROWNUM <= :TOPCOUNT
        //				ORDER BY WORK_TIME ASC
        //			";

        //	return await _dbConnection.QueryAsync<HourYidldModel>(sql, new { OPERATION_LINE_ID = lineId, WORK_TIME = date, TOPCOUNT = topCount });
        //}

        /// <summary>
        /// 产线看板的排产的完成率
        /// </summary>
        /// <param name="lineId"></param>
        /// <returns></returns>
        public async Task<List<KanbanWorkingPassRateModel>> GetKanbanWorkingPassRateDataAsync(int lineId)
        {
            List<KanbanWorkingPassRateModel> result = new List<KanbanWorkingPassRateModel>();
            //var p = new DynamicParameters();
            //p.Add(":V_LINE_ID", lineId);
            //await _dbConnection.ExecuteAsync("SYNC_SFCS_KANBAN_WORKING_PASS", p, commandType: CommandType.StoredProcedure);
            string sql = "SELECT* FROM SFCS_KANBAN_WORKING_PASS_RATE WHERE OPERATION_LINE_ID = " + lineId;
            result = (await _dbConnection.QueryAsync<KanbanWorkingPassRateModel>(sql, new { LINE_ID = lineId }))?.ToList();


            return result;
        }

        /// <summary>
        /// 获取项目信息
        /// </summary>
        /// <param name="lineId"></param>
        /// <returns></returns>
        public async Task<SfcsParameters> GetObjectDataAsync()
        {
            return (await _dbConnection.QueryAsync<SfcsParameters>("SELECT DESCRIPTION,CHINESE FROM Sfcs_Parameters WHERE  LOOKUP_TYPE='PROJECT_NAME' AND ENABLED='Y'"))?.FirstOrDefault();
        }

        /// <summary>
        /// 产线看板的标准产能
        /// </summary>
        /// <param name="lineId"></param>
        /// <returns></returns>
        public async Task<IEnumerable<SysWorkShiftDetailModel>> GetWorkShiftDetailDataAsync(int lineId)
        {
            return await _dbConnection.QueryAsync<SysWorkShiftDetailModel>("SELECT * FROM SYS_WORK_SHIFT_DETAIL WHERE LINE_ID = " + lineId + " AND LINE_TYPE = 'PCBA'");
        }

        /// <summary>
        /// 产线看板不良品信息
        /// </summary>
        /// <param name="wo_no"></param>
        /// <returns></returns>
        public async Task<IEnumerable<KanbanDefectMessageModel>> GetKanbanDefectMsgDataAsync(string wo_no)
        {
            string sql = @" SELECT SCD.SN,                                                 -- 产品条码
							 SDC.CHINESE_DESCRIPTION,                                      -- 不良现象
							 SOS.OPERATION_SITE_NAME,                                       --不良位置
							 SCD.DEFECT_TIME,                                               --不良时间
							 SCD.REPAIRER,                                                  --维修人员
							 SCD.REPAIR_TIME,                                               --维修时间
							 SP.CHINESE AS STATUS,                                         -- 当前状态
							 SO.DESCRIPTION                                                --当前工序,
						FROM SFCS_COLLECT_DEFECTS SCD,
							 SFCS_WO SW,
							 SFCS_DEFECT_CONFIG SDC,
							 SFCS_RUNCARD SR,
							 SFCS_OPERATION_SITES SOS,
							 SFCS_OPERATIONS SO,
							 (SELECT *
								FROM SFCS_parameters
							   WHERE lookup_type = 'RUNCARD_STATUS') SP
					   WHERE     SCD.WO_ID = SW.ID
							 AND SDC.DEFECT_CODE = SCD.DEFECT_CODE
							 AND SR.ID = SCD.SN_ID
							 AND SOS.ID = SCD.DEFECT_SITE_ID
							 AND SO.ID = SR.WIP_OPERATION
							 AND SR.STATUS = SP.LOOKUP_CODE
							 AND SW.WO_NO = :WO_NO
                             AND SCD.DEFECT_TIME >= to_date(to_char(sysdate,'yyyy-MM-dd'),'yyyy-MM-dd HH24:mi:ss')
					ORDER BY DEFECT_TIME DESC";

            return await _dbConnection.QueryAsync<KanbanDefectMessageModel>(sql, new { WO_NO = wo_no });
        }
        #endregion

        #region 自动化线看板

        /// <summary>
        /// 检查自动化线体是否存在
        /// </summary>
        /// <param name="lineId"></param>
        /// <returns></returns>
        public async Task<bool> CheckSmtLineAsync(int lineId)
        {
            var result = await _dbConnection.ExecuteScalarAsync<object>("SELECT COUNT(LINE_ID) FROM V_LINES_STATION WHERE ENABLED_ROUTE = 'Y' AND LINE_ID = " + lineId);
            if (Convert.ToInt32(result) > 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        /// <summary>
        /// 获取产线的工单信息
        /// </summary>
        /// <param name="lineId">产线id</param>
        /// <returns></returns>
        public async Task<IEnumerable<SmtKanbanWoModel>> GetSmtKanbanWoDataAsync(int lineId)
        {
            var p = new DynamicParameters();
            p.Add(":V_LINE_ID", lineId);
            await _dbConnection.ExecuteAsync("SYNC_SMT_KANBAN_WO", p, commandType: CommandType.StoredProcedure);
            return await _dbConnection.QueryAsync<SmtKanbanWoModel>("SELECT * FROM SMT_KANBAN_WO WHERE OPERATION_LINE_ID = " + lineId);
        }

        /// <summary>
        /// 自动化线看板的AOI的直通率
        /// </summary>
        /// <param name="lineId"></param>
        /// <returns></returns>
        public async Task<IEnumerable<SmtKanbanAoiPassRateModel>> GetSmtKanbanAoiPassRateDataAsync(int lineId)
        {
            var p = new DynamicParameters();
            p.Add(":V_LINE_ID", lineId);

            await _dbConnection.ExecuteAsync("SYNC_SMT_KANBAN_AOI_PASS", p, commandType: CommandType.StoredProcedure);
            return await _dbConnection.QueryAsync<SmtKanbanAoiPassRateModel>("SELECT * FROM SMT_KANBAN_AOI_PASS_RATE WHERE OPERATION_LINE_ID = " + lineId);
        }

        /// <summary>
        /// 自动化线看板的首件的直通率
        /// </summary>
        /// <param name="lineId"></param>
        /// <returns></returns>
        public async Task<IEnumerable<SmtKanbanFirstPassRateModel>> GetSmtKanbanFirstPassRateDataAsync(int lineId)
        {
            var p = new DynamicParameters();
            p.Add(":V_LINE_ID", lineId);

            await _dbConnection.ExecuteAsync("SYNC_SMT_KANBAN_FIRST_PASS", p, commandType: CommandType.StoredProcedure);
            return await _dbConnection.QueryAsync<SmtKanbanFirstPassRateModel>("SELECT * FROM SMT_KANBAN_FIRST_PASS_RATE WHERE OPERATION_LINE_ID = " + lineId);
        }

        /// <summary>
        /// 自动化线看板-低水位预警
        /// </summary>
        /// <param name="lineId"></param>
        /// <returns></returns>
        public async Task<IEnumerable<SmtKanbanRestPcbModel>> GetSmtKanbanRestPcbDataAsync(int lineId, int topCount)
        {
            var p = new DynamicParameters();
            p.Add(":V_LINE_ID", lineId);
            p.Add(":V_TOP_COUNT", topCount);

            await _dbConnection.ExecuteAsync("SYNC_SMT_KANBAN_REST_PCB", p, commandType: CommandType.StoredProcedure);
            return await _dbConnection.QueryAsync<SmtKanbanRestPcbModel>("SELECT * FROM SMT_KANBAN_REST_PCB WHERE OPERATION_LINE_ID = " + lineId);
        }

        /// <summary>
        /// 自动化线看板的SPI的直通率
        /// </summary>
        /// <param name="lineId"></param>
        /// <returns></returns>
        public async Task<IEnumerable<SmtKanbanSpiPassRateModel>> GetSmtKanbanSpiPassRateDataAsync(int lineId)
        {
            var parameterModle = await GetObjectDataAsync();


            var p = new DynamicParameters();
            p.Add(":V_LINE_ID", lineId);

            await _dbConnection.ExecuteAsync("SYNC_SMT_KANBAN_SPI_PASS", p, commandType: CommandType.StoredProcedure);
            return await _dbConnection.QueryAsync<SmtKanbanSpiPassRateModel>("SELECT * FROM SMT_KANBAN_SPI_PASS_RATE WHERE OPERATION_LINE_ID = " + lineId);
        }

        /// <summary>
        /// 自动化线看板的排产的完成率
        /// </summary>
        /// <param name="lineId"></param>
        /// <returns></returns>
        public async Task<IEnumerable<SmtKanbanWorkingPassRateModel>> GetSmtKanbanWorkingPassRateDataAsync(int lineId)
        {
            var parameterModle = await GetObjectDataAsync();
            List<SmtKanbanWorkingPassRateModel> result = new List<SmtKanbanWorkingPassRateModel>();
            string sql = "";
            var p = new DynamicParameters();
            var projectName = parameterModle == null ? "" : parameterModle.CHINESE.ToUpper();
            switch (projectName)
            {
                case "创维":
                    SmtKanbanWorkingPassRateModel passRateModel = new SmtKanbanWorkingPassRateModel();
                    sql = "SELECT SUM(SPP.PLAN_QUANTITY) AS SUM  FROM SMT_PRODUCE_PLAN SPP WHERE SPP.LINE_ID=:LINE_ID AND TO_CHAR(SPP.PLAN_DATE,'yyyy-MM-dd')=TO_CHAR(sysdate,'yyyy-MM-dd')";
                    decimal? denoMinator = await _dbConnection.ExecuteScalarAsync<decimal>(sql, new { LINE_ID = lineId });
                    p.Add(":V_LINE_ID", lineId);
                    await _dbConnection.ExecuteAsync("SYNC_SMT_KANBAN_HOUR_YIDLD_EX", p, commandType: CommandType.StoredProcedure);
                    var hourYildList = await _dbConnection.QueryAsync<SmtKanbanHourYidldModel>("SELECT * FROM SMT_KANBAN_HOUR_YIDLD WHERE OPERATION_LINE_ID =:LINE_ID ORDER BY WORK_TIME ASC", new { LINE_ID = lineId });
                    decimal? molecular = hourYildList.Sum<SmtKanbanHourYidldModel>(c => c.OUTPUT_QTY);
                    passRateModel.OPERATION_LINE_ID = lineId;
                    passRateModel.PASS = molecular ?? 0;
                    passRateModel.TOTAL = denoMinator ?? 0;
                    passRateModel.RATE = passRateModel.PASS == 0 ? 0 : Math.Round(((passRateModel.PASS / passRateModel.TOTAL) * 100), 2);
                    result.Add(passRateModel);
                    break;
                default:
                    p.Add(":V_LINE_ID", lineId);
                    //await _dbConnection.ExecuteAsync("SYNC_SMT_KANBAN_WORKING_PASS", p, commandType: CommandType.StoredProcedure);
                    await _dbConnection.QueryAsync<SmtKanbanWorkingPassRateModel>("SELECT * FROM SMT_KANBAN_WORKING_PASS_RATE WHERE OPERATION_LINE_ID = " + lineId);
                    break;
            }
            return result;


        }

        /// <summary>
        /// 获取自动化线最近X小时的每小时产能
        /// </summary>
        /// <param name="lineId"></param>
        /// <param name="topCount"></param>
        /// <returns></returns>
        public async Task<IEnumerable<SmtKanbanHourYidldModel>> GetSmtKanbanHourYidldDataAsync(int lineId, int topCount)
        {
            var p = new DynamicParameters();
            p.Add(":V_LINE_ID", lineId);
            p.Add(":V_TOP_COUNT", topCount);

            await _dbConnection.ExecuteAsync("SYNC_SMT_KANBAN_HOUR_YIDLD", p, commandType: CommandType.StoredProcedure);
            //await _dbConnection.ExecuteAsync("SYNC_SMT_KANBAN_HOUR_YIDLD_EX", p, commandType: CommandType.StoredProcedure);
            return await _dbConnection.QueryAsync<SmtKanbanHourYidldModel>("SELECT * FROM SMT_KANBAN_HOUR_YIDLD WHERE OPERATION_LINE_ID = " + lineId + " ORDER BY WORK_TIME ASC");

        }

        /// <summary>
        /// 获取自动化线最近X小时的每小时产能（优化）
        /// </summary>
        /// <param name="lineId"></param>
        /// <param name="topCount"></param>
        /// <param name="date"></param>
        /// <returns></returns>
        //public async Task<IEnumerable<SmtKanbanHourYidldModel>> GetSmtKanbanHourYidldDataNewAsync(int lineId, int topCount, string date)
        //{
        //	string sql = @"SELECT * FROM (select HY.*,CASE HY.WORK_TIME WHEN  TO_DATE( TO_CHAR(SYSDATE,'yyyy-MM-dd HH24')||':00:00','yyyy-MM-dd HH24:mi:ss') THEN -2 ELSE NVL(MR.STATUS,-1) END AS STATUS,
        //                          MR.REPORT_CONTENT,MR.REASON from MES_KANBAN_HOUR_YIDLD hy 
        //                          LEFT JOIN MES_MONITORING_REPORT mr ON MR.LINE_TYPE='SMT' AND REPORT_TYPE='HOUR_YIELD' AND hy.LINE_ID = MR.LINE_ID 
        //                          AND MR.WO_NO = HY.WO_NO AND HY.WORK_TIME = MR.WORK_TIME 
        //                      WHERE HY.LINE_TYPE='SMT' and  HY.LINE_ID = :OPERATION_LINE_ID  and HY.WORK_TIME >= TO_DATE(:WORK_TIME,'yyyy-MM-dd HH24:mi:ss')
        //                      ORDER BY HY.WORK_TIME DESC,HY.PART_NO ASC) WHERE ROWNUM <= :TOPCOUNT ORDER BY WORK_TIME ASC";

        //	return await _dbConnection.QueryAsync<SmtKanbanHourYidldModel>(sql, new { OPERATION_LINE_ID = lineId, WORK_TIME = date, TOPCOUNT = topCount });
        //}


        /// <summary>
        /// 自动化线看板的标准产能
        /// </summary>
        /// <param name="lineId"></param>
        /// <returns></returns>
        public async Task<IEnumerable<SysWorkShiftDetailModel>> GetSmtWorkShiftDetailDataAsync(int lineId)
        {
            return await _dbConnection.QueryAsync<SysWorkShiftDetailModel>("SELECT * FROM SYS_WORK_SHIFT_DETAIL WHERE LINE_ID = " + lineId + " AND LINE_TYPE = 'SMT'");
        }

        /// <summary>
        /// 获取自动化看板换线时间数据
        /// </summary>
        /// <param name="lineId"></param>
        /// <param name="day"></param>
        /// <returns></returns>
        public async Task<IEnumerable<MesChangeLineRecord>> GetSmtChangeLineTimeDataAsync(int lineId, int day)
        {
            DateTime date = DateTime.Parse(DateTime.Now.AddDays(-day).ToString("yyyy-MM-dd"));
            string sql = @"SELECT * FROM MES_CHANGE_LINE_RECORD WHERE LINE_TYPE='SMT' AND LINE_ID = :LINE_ID 
							AND (PRE_START_TIME >= :TIME OR NEXT_START_TIME >= :TIME)
						ORDER BY PRE_END_TIME";

            return await _dbConnection.QueryAsync<MesChangeLineRecord>(sql, new { LINE_ID = lineId, TIME = date });
        }
        #endregion

        #region 设备点检看板
        /// <summary>
        /// 获取设备列表
        /// </summary>
        /// <returns></returns>
        public async Task<IEnumerable<SfcsEquipmentListModel>> GetEquipmentList(decimal LineId)
        {
            StringBuilder sql = new StringBuilder();
            sql.Append(@"select tb1.*,tb2.LINE_NAME AS STATION_NAME,TB3.CHINESE AS CATEGORY_NAME,tb4.CHINESE AS USER_PART_NAME,tb5.RESOURCE_NAME as ImgName,tb5.RESOURCE_SIZE as ImgSize,tb5.RESOURCE_URL as ImgUrl from SFCS_EQUIPMENT tb1
						inner join(select ID, LINE_NAME from SMT_LINES union select ID, OPERATION_LINE_NAME AS LINE_NAME from SFCS_OPERATION_LINES) tb2 on tb1.STATION_ID = tb2.ID 
						inner join(select LOOKUP_CODE, CHINESE from SFCS_PARAMETERS  where lookup_type = 'EQUIPMENT_CATEGORY' AND ENABLED = 'Y') tb3 on tb1.CATEGORY = tb3.LOOKUP_CODE 
						left join(SELECT A.ID,A.CHINESE,B.CHINESE AS SBU_CHINESE,B.ID AS SBU_ID FROM SFCS_LOOKUPS A INNER JOIN SFCS_PARAMETERS B ON B.ID = A.KIND AND A.ENABLED = 'Y' AND B.LOOKUP_TYPE = 'SBU_CODE' AND B.ENABLED = 'Y') tb4 on tb1.USER_PART = tb4.ID
						left join(select ID,MST_ID,RESOURCE_URL,RESOURCES_CATEGORY,RESOURCE_NAME,RESOURCE_SIZE from SOP_OPERATIONS_ROUTES_RESOURCE WHERE RESOURCES_CATEGORY = 4) tb5 on tb1.ID = tb5.MST_ID
						where tb1.ENABLE = 'Y' and tb1.STATION_ID = :STATION_ID
						order by  tb1.ID desc
					");

            return await _dbConnection.QueryAsync<SfcsEquipmentListModel>(sql.ToString(), new { STATION_ID = LineId });
        }

        /// <summary>
        /// 获取设备维修记录
        /// </summary>
        /// <param name="topCount"></param>
        /// <param name="equipId"></param>
        /// <returns></returns>
        public async Task<IEnumerable<SfcsEquipRepairHead>> GetEquipRepairHeadList(int topCount, decimal equipId)
        {
            string sql = "select * from SFCS_EQUIP_REPAIR_HEAD where EQUIP_ID =:EQUIP_ID and ROWNUM <= :TOPCODE ORDER BY ID DESC";

            return await _dbConnection.QueryAsync<SfcsEquipRepairHead>(sql, new { EQUIP_ID = equipId, TOPCODE = topCount });
        }

        /// <summary>
        /// 获取设备点检记录
        /// </summary>
        /// <param name="topCount"></param>
        /// <param name="equipId"></param>
        /// <param name="type"></param>
        /// <returns></returns>
        public async Task<IEnumerable<SfcsEquipKeepHead>> GetEquipKeepHeadList(int topCount, decimal equipId, int type)
        {
            string sql = "SELECT * FROM (select* from SFCS_EQUIP_KEEP_HEAD where EQUIP_ID = :EQUIP_ID and KEEP_TYPE =:KEEP_TYPE ORDER BY KEEP_TIME DESC) WHERE ROWNUM <= :TOPCODE";

            return await _dbConnection.QueryAsync<SfcsEquipKeepHead>(sql, new { EQUIP_ID = equipId, KEEP_TYPE = type, TOPCODE = topCount });
        }

        #endregion

        /// <summary>
        /// 看板的异常报告
        /// </summary>
        /// <param name="lineId"></param>
        /// <param name="lineType"></param>
        /// <returns></returns>
        public async Task<IEnumerable<MesMonitoringReportListModel>> GetMonitoringReportDataAsync(int lineId, string lineType, int topCount)
        {
            var p = new DynamicParameters();
            p.Add(":V_LINE_ID", lineId);
            p.Add(":V_LINE_TYPE", lineType);
            p.Add(":V_TOP_COUNT", topCount);

            string sSql = "SELECT * FROM(" +
                "SELECT * FROM V_MES_MONITORING_REPORT " +
                "WHERE LINE_TYPE = :V_LINE_TYPE AND LINE_ID = :V_LINE_ID AND STATUS >= 0 ORDER BY STATUS ASC,ID DESC)" +
                "WHERE ROWNUM <= :V_TOP_COUNT";

            return await _dbConnection.QueryAsync<MesMonitoringReportListModel>(sSql, p, commandType: CommandType.Text);
        }

        #region 美联生产看板

        /// <summary>
        /// 获取当前工单的时间完成数
        /// </summary>
        /// <param name="lineId"></param>
        /// <returns></returns> 
        public async Task<WOPassVM> GetWoPassTotal(int lineId)
        {
            WOPassVM woPassVM = new WOPassVM();
            string sql = @"select sum(SSS.PASS)  from 
						   	  SFCS_PRODUCTION SP, SFCS_WO SW ,
							  SFCS_SITE_STATISTICS SSS, SFCS_OPERATION_SITES SOS
						   Where SP.WO_NO = SW.WO_NO AND SP.FINISHED = 'N'
								AND SSS.WO_ID = SW.ID AND SSS.OPERATION_SITE_ID = SOS.ID AND SOS.OPERATION_ID = 200
								AND SP.LINE_ID = :LINE_ID";
            woPassVM.PassTotal = await _dbConnection.ExecuteScalarAsync<decimal>(sql, new { LINE_ID = lineId });

            sql = @"select TARGET_QTY from SFCS_WO SW, SFCS_PRODUCTION SP where SW.WO_NO = SP.WO_NO AND SP.FINISHED = 'N' 
                      AND SP.LINE_ID = :LINE_ID";
            woPassVM.TargetQty = await _dbConnection.ExecuteScalarAsync<decimal>(sql, new { LINE_ID = lineId });

            return woPassVM;
        }

        /// <summary>
        /// 获取每小时产能
        /// </summary>
        /// <param name="lineId"></param>
        /// <returns></returns>
        public async Task<List<dynamic>> GetWoHourPass(int lineId)
        {
            //每小时产能 
            string sql = @"select SUM(sss.PASS) PASS ,sss.WORK_TIME  --每小时产能
                            from SFCS_SITE_STATISTICS sss, SFCS_OPERATION_SITES SOS
                            WHERE  SSS.OPERATION_SITE_ID = SOS.ID 
                            AND SOS.OPERATION_ID = 200 
                            AND SOS.OPERATION_LINE_ID = :LINE_ID
                            AND sss.WORK_TIME >= to_date(to_char(sysdate,'yyyy/mm/dd'),'yyyy/mm/dd HH24:mi:ss')
                            AND  sss.WORK_TIME< to_date(to_char(sysdate+1,'yyyy/mm/dd'),'yyyy/mm/dd HH24:mi:ss')
                            GROUP BY sss.WORK_TIME ";
            var resdata = await _dbConnection.QueryAsync<dynamic>(sql, new { LINE_ID = lineId });

            return resdata?.ToList();
        }

        /// <summary>
        /// 获取今日工单完成
        /// </summary>
        /// <param name="lineId"></param>
        /// <returns></returns>
        public async Task<List<dynamic>> GetWoToDayPass(int lineId)
        {
            //完成数量， 工单号
            string sql = @"select  SUM(sss.PASS) AS PASS, SW.WO_NO 
                            from SFCS_SITE_STATISTICS sss, SFCS_OPERATION_SITES SOS ,
                            SFCS_WO SW
                            WHERE  SSS.OPERATION_SITE_ID = SOS.ID 
                            AND SOS.OPERATION_ID = 200 
                            AND SSS.WO_ID = SW.ID
                            AND SOS.OPERATION_LINE_ID = :LINE_ID
                            AND sss.WORK_TIME >= to_date(to_char(sysdate,'yyyy/mm/dd'),'yyyy/mm/dd HH24:mi:ss')
                            AND  sss.WORK_TIME< to_date(to_char(sysdate+1,'yyyy/mm/dd'),'yyyy/mm/dd HH24:mi:ss')
                            GROUP BY SW.WO_NO";
            var resdata = await _dbConnection.QueryAsync<dynamic>(sql, new { LINE_ID = lineId });

            return resdata?.ToList();
        }

        /// <summary>
        /// 获取今日的工单汇总
        /// </summary>
        /// <param name="lineId"></param>
        /// <returns></returns>
        public async Task<List<dynamic>> GetWoToDayALL(int lineId)
        {
            //目标数量，完成数量
            string sql = @"select t.WOID,sum(t.PASS) PASS,t.QTY from (select swo.wo_no WOID,
                           CASE WHEN ssite.OPERATION_ID = 200  THEN  sum(stitcs.PASS)   
                           ELSE 0 END PASS ,swo.TARGET_QTY QTY from SFCS_SITE_STATISTICS stitcs  
                           left join SFCS_WO swo on swo.id=stitcs.wo_id
                           inner join SFCS_PRODUCTION sfpn on sfpn.WO_NO=swo.wo_no 
                           inner join SFCS_OPERATION_SITES ssite on stitcs.OPERATION_SITE_ID=ssite.ID
                           where   sfpn.FINISHED!='O' and sfpn.LINE_ID=:LINE_ID
                           and stitcs.WORK_TIME>=trunc(sysdate) and trunc(sysdate)+1 >stitcs.WORK_TIME
                           GROUP BY swo.wo_no,ssite.OPERATION_ID,swo.TARGET_QTY
													 order by pass desc
													 ) t
													 GROUP BY t.WOID,t.QTY ";
            var resdata = await _dbConnection.QueryAsync<dynamic>(sql, new { LINE_ID = lineId });

            return resdata?.ToList();
        }

        /// <summary>
        /// 直通率
        /// </summary>
        /// <param name="lineId"></param>
        /// <returns></returns>
        public async Task<decimal> GetWoToRate(int lineId)
        {
            //不良数量
            string failSql = @"select NVL(sum(fail),0) fail from  SFCS_SITE_STATISTICS stitcs
                               left join SFCS_WO swo on swo.id=stitcs.wo_id
                               inner join SFCS_OPERATION_SITES ssite on stitcs.OPERATION_SITE_ID=ssite.ID
                               where  ssite.OPERATION_LINE_ID=:LINE_ID
                               and stitcs.WORK_TIME>=trunc(sysdate) and trunc(sysdate)+1 >stitcs.WORK_TIME ";

            //前外观工序的pass数量
            string succesSql = @"select NVL(sum(pass),0) pass from SFCS_SITE_STATISTICS stitcs
                                 left join SFCS_WO swo on swo.id=stitcs.wo_id
                                 inner join SFCS_PRODUCTION sfpn on sfpn.WO_NO=swo.wo_no
                                 inner join SFCS_OPERATION_SITES ssite on stitcs.OPERATION_SITE_ID=ssite.ID
                                 where  ssite.OPERATION_ID = 383682 and  ssite.OPERATION_LINE_ID=:LINE_ID
                                 and stitcs.WORK_TIME>=trunc(sysdate) and trunc(sysdate)+1 >stitcs.WORK_TIME";
            var failNumber = await _dbConnection.ExecuteScalarAsync<decimal>(failSql, new { LINE_ID = lineId });
            var succesNumber = await _dbConnection.ExecuteScalarAsync<decimal>(succesSql, new { LINE_ID = lineId });
            var result = ((failNumber == 0) || (succesNumber == 0)) ? 0 : decimal.Round((1 - (failNumber / succesNumber)), 3);
            return result;
        }

        /// <summary>
        ///站点统计
        /// </summary>
        /// <param name="lineId"></param>
        /// <returns></returns>
        public async Task<List<dynamic>> GetSiteStatistics(int lineId)
        {
            string Sql = @"select ssite.OPERATION_SITE_NAME,NVL(sum(pass),0) pass,NVL(sum(fail),0) fail from SFCS_SITE_STATISTICS stitcs
                                 left join SFCS_WO swo on swo.id=stitcs.wo_id
                                 inner join SFCS_PRODUCTION sfpn on sfpn.WO_NO=swo.wo_no
                                 inner join SFCS_OPERATION_SITES ssite on stitcs.OPERATION_SITE_ID=ssite.ID
                                 where sfpn.FINISHED='N'  and ssite.OPERATION_LINE_ID=:LINE_ID 
                                 and stitcs.WORK_TIME>=trunc(sysdate) and trunc(sysdate)+1 >stitcs.WORK_TIME
								 GROUP by stitcs.OPERATION_SITE_ID,ssite.OPERATION_SITE_NAME
                                 ORDER BY stitcs.OPERATION_SITE_ID ASC";
            var resutl = await _dbConnection.QueryAsync<dynamic>(Sql, new { LINE_ID = lineId });
            return resutl?.ToList();
        }

        /// <summary>
        ///最近5天的产能
        /// </summary>
        /// <param name="lineId"></param>
        /// <returns></returns>
        public async Task<List<dynamic>> Top5Prouduct(int lineId)
        {

            string Sql = @"select sum(t.pass),WORK_TIME from (
                           select  stitcs.pass pass,to_char(stitcs.WORK_TIME,'YYYY-MM-DD') WORK_TIME  from SFCS_SITE_STATISTICS stitcs --sum(stitcs.pass) pass
                           left join SFCS_WO swo on swo.id=stitcs.wo_id
                           inner join SFCS_PRODUCTION sfpn on sfpn.WO_NO=swo.wo_no 
                           inner join SFCS_OPERATION_SITES ssite on stitcs.OPERATION_SITE_ID=ssite.ID
                           where  ssite.OPERATION_ID = 200 and  ssite.OPERATION_LINE_ID=383680
                           			and to_date(to_char(stitcs.WORK_TIME,'yyyy/mm/dd'), 'yyyy-mm-dd')<=trunc(sysdate) and trunc(sysdate)-5 <to_date(to_char(stitcs.WORK_TIME,'yyyy/mm/dd'), 'yyyy-mm-dd')
                           ) t
                           GROUP BY t.WORK_TIME
                           ORDER BY WORK_TIME ASC";
            var resutl = await _dbConnection.QueryAsync<dynamic>(Sql, new { LINE_ID = lineId });
            return resutl?.ToList();
        }

        #endregion

        #region AOI/SPI集成看板

        /// <summary>
        /// 获取看板数据
        /// </summary>
        /// <returns></returns>
        public async Task<List<AoiAndSpiReportListModel>> GetAoiSpiDataAsync(string organizeId, string floor)
        {
            //得到AOI所有机台信息
            string getStationSql = "SELECT :ORGANIZE_ID AS ORGANIZE_ID,MEANING AS STATION_NAME,DESCRIPTION AS STATION_TYPE,ATTRIBUTE2 AS FLOOR " +
                "FROM SFCS_PARAMETERS WHERE LOOKUP_TYPE = 'AOI_SPI_STATION_NAME' AND ENABLED='Y' AND ORGANIZE_ID = :ORGANIZE_ID AND ATTRIBUTE2=:FLOOR";
            var data = (await _dbConnection.QueryAsync<AoiAndSpiReportListModel>(getStationSql, new { ORGANIZE_ID = organizeId, FLOOR = floor })).ToList();

            //获取SPI所有设备
            string getSpiStation = @"SELECT :ORGANIZE_ID AS ORGANIZE_ID,TYPE AS STATION_TYPE,MST.LINE_ID,LINE_NAME,ATTRIBUTE7 AS FLOOR 
                                     FROM SYS_MES_AOI_JOB_CONTROL MST 
									 INNER JOIN SYS_ORGANIZE_LINE OL ON MST.LINE_ID = OL.LINE_ID 
									 WHERE TYPE = 'SPI' AND ENABLED = 'Y' AND ORGANIZE_ID = :ORGANIZE_ID AND ATTRIBUTE7 = :FLOOR ";
            var spidata = (await _dbConnection.QueryAsync<AoiAndSpiReportListModel>(getSpiStation, new { ORGANIZE_ID = organizeId, FLOOR = floor })).ToList();

            data.AddRange(spidata);

            //循环机台获取数据
            foreach (var item in data)
            {
                if (item.STATION_TYPE == "AOI")
                {
                    GetAoiData(item);
                    item.LINE_NAME = "无";
                }
                else
                    GetSpiData(item);
            }

            return data;
        }

        /// <summary>
        /// 获取AOI信息
        /// </summary>
        /// <param name="item"></param>
        private void GetAoiData(AoiAndSpiReportListModel item)
        {
            //获取工单信息
            AoiAndSpiReportListModel woModel = _dbConnection.Query<AoiAndSpiReportListModel>(string.Format(@"SELECT WO_NO, PART_NO, DESCRIPTION AS PART_DESC
																			  FROM SMT_WO
																			 WHERE WO_NO = (SELECT WORK_ORDER
																							  FROM MES_OFFLINE_AOI_PCB_RECORD
																							 WHERE ID = (SELECT MAX (ID)
																										   FROM MES_OFFLINE_AOI_PCB_RECORD
																										  WHERE MACHINE = '{0}'))", item.STATION_NAME.Trim())).FirstOrDefault();
            if (woModel != null)
            {
                item.WO_NO = woModel.WO_NO;
                item.PART_NO = woModel.PART_NO;
                item.PART_DESC = woModel.PART_DESC;
            }
            else
            {
                item.WO_NO = "无";
                item.PART_NO = "无";
                item.PART_DESC = "无";
                return;
            }

            //获取直通率
            item.PASS_QTY = _dbConnection.ExecuteScalar<decimal>("SELECT COUNT(1) FROM MES_OFFLINE_AOI_PCB_RECORD WHERE WORK_ORDER='" + item.WO_NO + "' AND TEST_RESULT = 'Pass'");
            item.TOTAL_QTY = _dbConnection.ExecuteScalar<decimal>("SELECT COUNT(1) FROM MES_OFFLINE_AOI_PCB_RECORD WHERE WORK_ORDER='" + item.WO_NO + "'");

            if (item.TOTAL_QTY == 0)
                item.FIRST_PASS_YIELD = 100;
            else
            {
                item.FIRST_PASS_YIELD = Math.Round(item.PASS_QTY / item.TOTAL_QTY * 100, 2);
            }

            //获取异常列表
            item.DefectList = _dbConnection.Query<AoiAndSpiReportDetail>(string.Format(@"SELECT *
														  FROM (  SELECT NG_NAME AS DEFECT_NAME, COUNT (1) AS DEFECT_QTY
																	FROM MES_OFFLINE_AOI_PCB_DETAIL
																   WHERE MST_ID IN (SELECT ID
																					  FROM MES_OFFLINE_AOI_PCB_RECORD
																					 WHERE     WORK_ORDER = '{0}'
																						   AND TEST_RESULT = 'NG')
																GROUP BY NG_NAME
																ORDER BY COUNT (1) DESC)
														 WHERE ROWNUM <= 5", item.WO_NO)).ToList();
        }

        /// <summary>
        /// 获取SPI信息
        /// </summary>
        /// <param name="item"></param>
        private void GetSpiData(AoiAndSpiReportListModel item)
        {
            //获取工单信息
            AoiAndSpiReportListModel woModel = _dbConnection.Query<AoiAndSpiReportListModel>(@"
													SELECT S.WO_NO, PART_NO, DESCRIPTION AS PART_DESC
													  FROM SMT_WO w INNER JOIN SMT_PRODUCTION s ON w.WO_NO = s.WO_NO
													 WHERE S.FINISHED = 'N' AND LINE_ID = :LINE_ID
												", new { item.LINE_ID }).FirstOrDefault();

            if (woModel != null)
            {
                item.WO_NO = woModel.WO_NO;
                item.PART_NO = woModel.PART_NO;
                item.PART_DESC = woModel.PART_DESC;
            }
            else
            {
                item.WO_NO = "无";
                item.PART_NO = "无";
                item.PART_DESC = "无";
                return;
            }

            //获取直通率
            item.PASS_QTY = _dbConnection.ExecuteScalar<decimal>("SELECT COUNT(1) FROM SMT_SPI_SRC_GEN WHERE WO='" + item.WO_NO + "' AND REVISE_RESULT = 'OK'");
            item.TOTAL_QTY = _dbConnection.ExecuteScalar<decimal>("SELECT COUNT(1) FROM SMT_SPI_SRC_GEN WHERE WO='" + item.WO_NO + "'");

            if (item.TOTAL_QTY == 0)
                item.FIRST_PASS_YIELD = 100;
            else
            {
                item.FIRST_PASS_YIELD = Math.Round(item.PASS_QTY / item.TOTAL_QTY * 100, 2);
            }

            //获取异常列表
            item.DefectList = _dbConnection.Query<AoiAndSpiReportDetail>(string.Format(@"
										SELECT *
										  FROM (  SELECT MEANING AS DEFECT_NAME, COUNT (1) AS DEFECT_QTY
													FROM SMT_SPI_SRC_DET DET
														 INNER JOIN (SELECT LOOKUP_CODE, MEANING
																	   FROM SFCS_PARAMETERS
																	  WHERE LOOKUP_TYPE = 'SPI_DEFECT') P
															ON DET.ERRCODE = P.LOOKUP_CODE
												   WHERE GEN_ID IN (SELECT ID
																	  FROM SMT_SPI_SRC_GEN
																	 WHERE     WO = '{0}'
																		   AND REVISE_RESULT = 'NG')
												GROUP BY MEANING
												ORDER BY COUNT (1) DESC)
										 WHERE ROWNUM <= 5", item.WO_NO)).ToList();
        }
        #endregion

        #region AI&RI集成看板

        /// <summary>
        /// 获取AI&RI集成看板数据
        /// </summary>
        /// <param name="organizeId"></param>
        /// <param name="idList"></param>
        /// <returns></returns>
        public async Task<List<AiAndRiReportListModel>> GetAiRiDataAsync(string organizeId, string floor, List<decimal> idList = null)
        {
            //查询AIRI线别信息列表SQL
            string getLinesSql = @"SELECT LINE_ID FROM SYS_ORGANIZE_LINE WHERE (ATTRIBUTE6='AI' OR ATTRIBUTE6='RI') AND ATTRIBUTE7=:FLOOR AND EXISTS
                                  (SELECT 1
                                     FROM (  SELECT ID
                                                 FROM SYS_ORGANIZE
                                           START WITH ID = :ORGANIZE_ID
                                           CONNECT BY PRIOR ID = PARENT_ORGANIZE_ID)
                                    WHERE ID = ORGANIZE_ID) ORDER BY CREATE_TIME";

            //获取工单信息SQL（工单信息、排产达成率、首件合格率）
            string getWoSql = @"SELECT WO.OPERATION_LINE_ID AS LINE_ID,WO.OPERATION_LINE_NAME AS LINE_NAME,WO.WO_NO,WO.PART_NO,WO.MODEL AS PART_DESC,WO.TARGET_QTY AS WO_TOTAL_QTY,WO.OUTPUT_QTY AS WO_PASS_QTY,WO.YIELD AS WO_PASS_RATE,
								WP.PASS AS WORK_PASS_QTY,WP.TOTAL AS WORK_TOTAL_QTY,WP.RATE AS WORK_PASS_RATE,
								FP.PASS AS FIRST_PASS_QTY,FP.TOTAL AS FIRST_TOTAL_QTY,FP.RATE AS FIRST_PASS_RATE
								FROM SMT_KANBAN_WO WO 
								INNER JOIN SMT_KANBAN_WORKING_PASS_RATE WP ON WO.OPERATION_LINE_ID = WP.OPERATION_LINE_ID
								INNER JOIN SMT_KANBAN_FIRST_PASS_RATE FP ON WO.OPERATION_LINE_ID = FP.OPERATION_LINE_ID
								WHERE WO.OPERATION_LINE_ID = :LINE_ID";

            List<AiAndRiReportListModel> list = new List<AiAndRiReportListModel>();
            //获取到所有线别
            if (idList == null)
                idList = (await _dbConnection.QueryAsync<decimal>(getLinesSql, new { ORGANIZE_ID = organizeId, FLOOR = floor })).ToList();

            foreach (var id in idList)
            {
                var linsId = int.Parse(id.ToString());

                var p = new DynamicParameters();
                p.Add(":V_LINE_ID", id);
                await _dbConnection.ExecuteAsync("SYNC_SMT_KANBAN_WO", p, commandType: CommandType.StoredProcedure);//统计工单信息
                await _dbConnection.ExecuteAsync("SYNC_SMT_KANBAN_WORKING_PASS", p, commandType: CommandType.StoredProcedure);//统计工单排产达成信息
                await _dbConnection.ExecuteAsync("SYNC_SMT_KANBAN_FIRST_PASS", p, commandType: CommandType.StoredProcedure);//统计首件合格率信息

                //获取到工单信息、排产达成率、首件合格率
                AiAndRiReportListModel model = (await _dbConnection.QueryAsync<AiAndRiReportListModel>(getWoSql, new { LINE_ID = id })).FirstOrDefault();

                if (model != null)
                {
                    //抽检情况
                    model.CHECK_RESULT = await GetKanbanSpotCheckDataAsync(linsId, model.WO_NO, 5);
                    //每小时产能
                    model.HOUR_YIDLD_RESULT = (await GetSmtKanbanHourYidldDataAsync(linsId, 5)).ToList();

                    list.Add(model);
                }
            }

            return list;
        }
        #endregion

        #region  看板入口

        /// <summary>
        /// 根据用户ID，获取用户的所有组织
        /// </summary>
        /// <param name="organizeId"></param>
        /// <returns></returns>
        public async Task<IEnumerable<BoardEntryListModel>> GetOrganizeList(decimal user_id)
        {
            string sql = @"SELECT OZ.ID AS ORGANIZE_ID, OZ.ORGANIZE_NAME FROM 
                            (SELECT DISTINCT T.* FROM SYS_ORGANIZE T START WITH T.ID IN (SELECT ORGANIZE_ID FROM 
                              SYS_USER_ORGANIZE WHERE MANAGER_ID=:USER_ID) CONNECT BY PRIOR T.ID=T.PARENT_ORGANIZE_ID) OZ 
                              INNER JOIN SYS_ORGANIZE_Type OT ON OZ.ORGANIZE_Type_ID = OT.ID 
                            WHERE OT.ORGANIZE_Type_CODE =  'BRANCH' ORDER BY OZ.ID";

            return await _dbConnection.QueryAsync<BoardEntryListModel>(sql, new { USER_ID = user_id });
        }

        /// <summary>
        /// 根据组织ID，获取到隶属该组织的所有线别
        /// </summary>
        /// <param name="organizeId"></param>
        /// <returns></returns>
        public async Task<IEnumerable<BoardEntryListModel>> GetLines(string organizeId)
        {

            string sql = @"select * from 
                 (SELECT A.ID, 'SMT' AS LINE_TYPE, A.LINE_NAME, A.LOCATION, A.PLANT, A.ORGANIZE_ID, OG.ORGANIZE_NAME  
                  FROM SMT_LINES A INNER JOIN SYS_ORGANIZE OG ON A.ORGANIZE_ID = OG.ID
                  UNION ALL 
                  SELECT  A.ID, 'SFCS' AS LINE_TYPE, A.OPERATION_LINE_NAME, SP.CHINESE, PT.CHINESE, A.ORGANIZE_ID, OG.ORGANIZE_NAME  
                 FROM SFCS_OPERATION_LINES A INNER JOIN SYS_ORGANIZE OG ON A.ORGANIZE_ID = OG.ID 
                   INNER JOIN SFCS_PARAMETERS SP ON A.PHYSICAL_LOCATION = SP.LOOKUP_CODE AND SP.LOOKUP_TYPE = 'PHYSICAL_LOCATION' AND SP.ENABLED = 'Y' 
                   INNER JOIN SFCS_PARAMETERS PT ON A.PHYSICAL_LOCATION = PT.LOOKUP_CODE AND PT.LOOKUP_TYPE = 'PLANT_CODE' AND PT.ENABLED = 'Y' 
                 where A.ENABLED = 'Y' ) OL  
                 WHERE EXISTS
                        (SELECT 1
                           FROM (    SELECT ID
                                       FROM SYS_ORGANIZE
                                 START WITH ID = :ORGANIZE_ID
                                 CONNECT BY PRIOR ID = PARENT_ORGANIZE_ID)
                          WHERE ID = OL.ORGANIZE_ID) ";

            return await _dbConnection.QueryAsync<BoardEntryListModel>(sql, new { ORGANIZE_ID = organizeId });
        }

        /// <summary>
        /// 根据组织ID，获取到AOI/SPI的楼层信息
        /// </summary>
        /// <param name="organizeId"></param>
        /// <returns></returns>
        public async Task<IEnumerable<AoiAndSpiReportListModel>> GetAoiSpiFloorData(string organizeId)
        {
            string sql = "SELECT * FROM V_AOISPI_FLOOR WHERE ORGANIZE_ID = :ORGANIZE_ID";
            return await _dbConnection.QueryAsync<AoiAndSpiReportListModel>(sql, new { ORGANIZE_ID = organizeId });
        }

        #endregion

        #region IDisposable Support
        private bool disposedValue = false; // 要检测冗余调用

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    // TODO: 释放托管状态(托管对象)。
                    _dbConnection?.Dispose();
                }

                // TODO: 释放未托管的资源(未托管的对象)并在以下内容中替代终结器。
                // TODO: 将大型字段设置为 null。

                disposedValue = true;
            }
        }

        // TODO: 仅当以上 Dispose(bool disposing) 拥有用于释放未托管资源的代码时才替代终结器。
        // ~BaseRepository() {
        //   // 请勿更改此代码。将清理代码放入以上 Dispose(bool disposing) 中。
        //   Dispose(false);
        // }

        // 添加此代码以正确实现可处置模式。
        public void Dispose()
        {
            // 请勿更改此代码。将清理代码放入以上 Dispose(bool disposing) 中。
            Dispose(true);
            // TODO: 如果在以上内容中替代了终结器，则取消注释以下行。
            // GC.SuppressFinalize(this);
        }
        #endregion

        #region AOI品质柏拉图
        /// <summary>
        /// AOI品质柏拉图
        /// </summary>
        /// <param name="WO_NO"></param>
        /// <returns></returns>
        public async Task<TableDataModel> GetAoiReportData(AoiAndSpiReportListModel model)
        {

            var dataSql = @"SELECT A.*,B.WO_NO,B.PART_NO,MODEL FROM (
								SELECT COUNT(1) AS TOTAL,
								    COUNT(CASE WHEN UPPER(TEST_RESULT)='PASS' THEN 1 ELSE NULL END) AS PASS,
								    COUNT(CASE WHEN UPPER(TEST_RESULT)='PASS' THEN NULL ELSE 1 END) AS UNPASS 
								    FROM MES_OFFLINE_AOI_PCB_RECORD B 
								WHERE WORK_ORDER = :WO_NO ) A,
								SFCS_WO B LEFT JOIN SFCS_MODEL C ON B.MODEL_ID = C.ID 
								WHERE WO_NO = :WO_NO ";
            var data = await _dbConnection.QueryAsync<dynamic>(dataSql, model);

            var sqlPage = $@"SELECT ELEMENT_NAME,NG_NAME,PN,MACHINE,ELEMENT_LOC,COUNT(1) AS NUM FROM MES_OFFLINE_AOI_PCB_DETAIL A LEFT JOIN MES_OFFLINE_AOI_PCB_RECORD B ON A.MST_ID = B.ID 
							 WHERE WORK_ORDER = :WO_NO 
							 GROUP BY ELEMENT_NAME,NG_NAME,PN,MACHINE,ELEMENT_LOC  ORDER BY NUM DESC";
            var dtlData = await _dbConnection.QueryAsync<dynamic>(sqlPage, model);

            var errorCountSql = @"SELECT * FROM (SELECT A.*,ROWNUM AS row_num FROM (SELECT ELEMENT_NAME,ELEMENT_LOC,NG_NAME,COUNT(1) AS NUM FROM MES_OFFLINE_AOI_PCB_DETAIL A LEFT JOIN MES_OFFLINE_AOI_PCB_RECORD B ON A.MST_ID = B.ID 
								  WHERE WORK_ORDER = :WO_NO 
								  GROUP BY ELEMENT_NAME,ELEMENT_LOC,NG_NAME ORDER BY NUM DESC) A) WHERE row_num<11 ORDER BY NUM DESC";
            var resultData = await _dbConnection.QueryAsync<dynamic>(errorCountSql, model);

            return new TableDataModel() { data = new { data, dtlData, resultData }, count = 0 };
        }
        #endregion

        #region SPI品质柏拉图
        /// <summary>
        /// SPI品质柏拉图
        /// </summary>
        /// <param name="WO_NO"></param>
        /// <returns></returns>
        public async Task<TableDataModel> GetSpiReportData(AoiAndSpiReportListModel model)
        {

            var dataSql = @"SELECT A.*,B.WO_NO,B.PART_NO,MODEL FROM (
								SELECT COUNT(1) AS TOTAL,
								COUNT(CASE WHEN TEST_RESULT='OK' THEN 1 ELSE NULL END) AS PASS,
								COUNT(CASE WHEN TEST_RESULT='OK' THEN NULL ELSE 1 END) AS UNPASS 
								FROM SMT_SPI_SRC_GEN WHERE WO=:WO_NO) A,
								SFCS_WO B LEFT JOIN SFCS_MODEL C ON B.MODEL_ID = C.ID 
								WHERE WO_NO = :WO_NO";
            var data = await _dbConnection.QueryAsync<dynamic>(dataSql, model);

            var sqlPage = $@"SELECT LOCATION,ERRCODE,COUNT(1) AS NUM FROM SMT_SPI_SRC_DET A LEFT JOIN SMT_SPI_SRC_GEN B ON A.GEN_ID = B.ID WHERE WO = :WO_NO GROUP BY LOCATION,ERRCODE  ORDER BY NUM DESC";
            var dtlData = await _dbConnection.QueryAsync<dynamic>(sqlPage, model);

            var errorCountSql = @"SELECT * FROM (SELECT A.*,ROWNUM AS row_num FROM (SELECT LOCATION,ERRCODE,COUNT(1) AS NUM FROM SMT_SPI_SRC_DET A LEFT JOIN SMT_SPI_SRC_GEN B ON A.GEN_ID = B.ID
								   WHERE WO = :WO_NO
								  GROUP BY LOCATION,ERRCODE  ORDER BY NUM DESC) A) WHERE row_num<11 ORDER BY NUM DESC";
            var resultData = await _dbConnection.QueryAsync<dynamic>(errorCountSql, model);

            return new TableDataModel() { data = new { data, dtlData, resultData }, count = 0 };
        }
        #endregion

        #region AVI品质柏拉图

        /// <summary>
        /// AVI品质柏拉图
        /// </summary>
        /// <param name="WO_NO"></param>
        /// <returns></returns>
        public async Task<TableDataModel> GetAviReportData(AoiAndSpiReportListModel model)
        {
            var dataSql = @"SELECT A.*,B.WO_NO,B.PART_NO,MODEL FROM (
								SELECT COUNT(1) AS TOTAL,
								    COUNT(CASE WHEN UPPER(RESULT_CODE)='PASS' THEN 1 ELSE NULL END) AS PASS,
								    COUNT(CASE WHEN UPPER(RESULT_CODE)='PASS' THEN NULL ELSE 1 END) AS UNPASS 
								    FROM SFCS_EQUIP_RECORD_MST B 
								WHERE WO_NO = :WO_NO ) A,
								SFCS_WO B LEFT JOIN SFCS_MODEL C ON B.MODEL_ID = C.ID ";
            var data = await _dbConnection.QueryAsync<dynamic>(dataSql, model);

            var sqlPage = $@"SELECT EQUIP_CODE,CODE,POINT,DESCRIPTION,COUNT(1) AS NUM FROM (SELECT EQUIP_CODE,
									(SELECT VALUE FROM SFCS_EQUIP_RECORD_DTL WHERE GROUP_ID=E.GROUP_ID AND NAME='Code') as CODE,
									(SELECT VALUE FROM SFCS_EQUIP_RECORD_DTL WHERE GROUP_ID=E.GROUP_ID AND NAME='Description') as Description,
									(SELECT VALUE FROM SFCS_EQUIP_RECORD_DTL WHERE GROUP_ID=E.GROUP_ID AND NAME='Point') as Point 
							 FROM SFCS_EQUIP_RECORD_MST A  RIGHT JOIN
									(SELECT MST_ID,GROUP_ID FROM SFCS_EQUIP_RECORD_MST C 
									LEFT JOIN SFCS_EQUIP_RECORD_DTL D ON C.ID = D.MST_ID 
							 WHERE WO_NO = :WO_NO AND MST_ID IS NOT NULL AND TYPE='AVI' GROUP BY MST_ID,GROUP_ID) E
									ON A.ID = E.MST_ID) GROUP BY CODE,POINT,DESCRIPTION,EQUIP_CODE  ORDER BY NUM DESC";
            var dtlData = await _dbConnection.QueryAsync<dynamic>(sqlPage, model);

            var errorCountSql = @"SELECT * FROM (SELECT A.*,ROWNUM AS row_num FROM (SELECT CODE,POINT,DESCRIPTION,COUNT(1) AS NUM FROM (SELECT A.ID,
									(SELECT VALUE FROM SFCS_EQUIP_RECORD_DTL WHERE GROUP_ID=E.GROUP_ID AND NAME='Code') as CODE,
									(SELECT VALUE FROM SFCS_EQUIP_RECORD_DTL WHERE GROUP_ID=E.GROUP_ID AND NAME='Description') as Description,
									(SELECT VALUE FROM SFCS_EQUIP_RECORD_DTL WHERE GROUP_ID=E.GROUP_ID AND NAME='Point') as Point 
							 FROM SFCS_EQUIP_RECORD_MST A  RIGHT JOIN
									(SELECT MST_ID,GROUP_ID FROM SFCS_EQUIP_RECORD_MST C 
									LEFT JOIN SFCS_EQUIP_RECORD_DTL D ON C.ID = D.MST_ID 
							 WHERE WO_NO = :WO_NO AND MST_ID IS NOT NULL AND TYPE='AVI' GROUP BY MST_ID,GROUP_ID) E
									ON A.ID = E.MST_ID) GROUP BY CODE,POINT,DESCRIPTION  ORDER BY NUM DESC) A) WHERE row_num<11 ORDER BY NUM DESC";
            var resultData = await _dbConnection.QueryAsync<dynamic>(errorCountSql, model);

            return new TableDataModel() { data = new { data, dtlData, resultData }, count = 0 };
        }
        #endregion

        #region ICT报表

        /// <summary>
        /// ICT报表
        /// </summary>
        /// <param name="WO_NO"></param>
        /// <returns></returns>
        public async Task<TableDataModel> GetIctReport(string WO_NO)
        {
            var mstSql = $@"SELECT SUM(CASE WHEN TOTAL_RESULT='PASS' THEN TOTAL_PANES ELSE 0 END) AS PASS_TOTAL,
								SUM(CASE WHEN TOTAL_RESULT='PASS' THEN 0 ELSE TOTAL_PANES END) AS FAIL_TOTAL,
								SUM(TOTAL_PANES) AS TOTAL_PANES 
							FROM ICT_HEADER 
							WHERE WO_NO = :WO_NO AND TEST_TYPE='ICT'";

            var data = await _dbConnection.QueryAsync<dynamic>(mstSql, new { WO_NO });
            var dtlSql = $@"SELECT 
								COUNT(case when UPPER(PANE_RESULT1)='PASS' then 1 else null end) PANE_RESULT1_PASS,
								COUNT(case when UPPER(PANE_RESULT2)='PASS' then 1 else null end) PANE_RESULT2_PASS,
								COUNT(case when UPPER(PANE_RESULT3)='PASS' then 1 else null end) PANE_RESULT3_PASS,
								COUNT(case when UPPER(PANE_RESULT4)='PASS' then 1 else null end) PANE_RESULT4_PASS,
								COUNT(case when UPPER(PANE_RESULT5)='Pass' then 1 else null end) PANE_RESULT5_PASS,
								COUNT(case when UPPER(PANE_RESULT1)='FAIL' then 1 else null end) PANE_RESULT1_FAIL,
								COUNT(case when UPPER(PANE_RESULT2)='FAIL' then 1 else null end) PANE_RESULT2_FAIL,
								COUNT(case when UPPER(PANE_RESULT3)='FAIL' then 1 else null end) PANE_RESULT3_FAIL,
								COUNT(case when UPPER(PANE_RESULT4)='FAIL' then 1 else null end) PANE_RESULT4_FAIL,
								COUNT(case when UPPER(PANE_RESULT5)='FAIL' then 1 else null end) PANE_RESULT5_FAIL,
								SUM(TOTAL_COMPONENT_FAILS) AS TOTAL_COMPONENT_FAILS,C.LOCATION
							FROM ICT_PANE_DETAIL A LEFT JOIN ICT_HEADER B ON B.ID = A.MST_ID 
                            INNER JOIN ICT_PANE_COMPONENT_FAIL C ON A.ID = C.MST_ID
							WHERE WO_NO = :WO_NO AND TEST_TYPE='ICT' 
                            GROUP BY C.LOCATION";
            var dtlData = await _dbConnection.QueryAsync<dynamic>(dtlSql, new { WO_NO });

            var resultDataSql = $@"SELECT to_char(B.TEST_DATE,'yyyy-MM-dd') TEST_DATE,
									COUNT(CASE WHEN UPPER(A.TOTAL_RESULT)='PASS' then 1 else null end) PASS_TOTAL,
									COUNT(CASE WHEN UPPER(A.TOTAL_RESULT)='PASS' then null else 1 end) FAIL_TOTAL
							   FROM ICT_PANE_DETAIL A LEFT JOIN ICT_HEADER B ON A.MST_ID = B.ID 
								WHERE WO_NO = :WO_NO AND TEST_TYPE='ICT' GROUP BY to_char(B.TEST_DATE,'yyyy-MM-dd') order by to_char(B.TEST_DATE,'yyyy-MM-dd')
							   ";
            var resultData = await _dbConnection.QueryAsync<dynamic>(resultDataSql, new { WO_NO });
            return new TableDataModel() { data = new { data, dtlData, resultData }, count = 0 };
        }
        #endregion

        #region FCT报表

        /// <summary>
        /// FCT报表
        /// </summary>
        /// <param name="WO_NO"></param>
        /// <returns></returns>
        public async Task<TableDataModel> GetFctReport(string WO_NO)
        {
            var mstSql = $@"SELECT COUNT(CASE WHEN TOTAL_RESULT='PASS' THEN 1 ELSE null END) AS PASS_TOTAL,
								COUNT(CASE WHEN TOTAL_RESULT='PASS' THEN null ELSE 1 END) AS FAIL_TOTAL,
								SUM(TOTAL_PANES) AS TOTAL_PANES 
							FROM ICT_HEADER 
							WHERE WO_NO = :WO_NO AND TEST_TYPE='FCT'";
            var data = await _dbConnection.QueryAsync<dynamic>(mstSql, new { WO_NO });

            var dtlSql = $@"SELECT COUNT(DESCRIPTION) FAIL_QTY,DESCRIPTION FROM ICT_HEADER A
                            INNER JOIN ICT_PANE_DETAIL B ON A.ID = B.MST_ID
                            INNER JOIN ICT_PANE_OPEN_FAIL C ON B.ID = C.MST_ID
                            WHERE WO_NO = :WO_NO AND TEST_TYPE = 'FCT'
                            GROUP BY DESCRIPTION";
            var dtlData = await _dbConnection.QueryAsync<dynamic>(dtlSql, new { WO_NO });

            var resultDataSql = $@"SELECT to_char(B.TEST_DATE,'yyyy-MM-dd') TEST_DATE,
									COUNT(CASE WHEN B.TOTAL_RESULT='PASS' then 1 else null end) PASS_TOTAL,
									COUNT(CASE WHEN B.TOTAL_RESULT='PASS' then null else 1 end) FAIL_TOTAL
							   FROM ICT_HEADER B
								WHERE WO_NO = :WO_NO AND TEST_TYPE='FCT' GROUP BY to_char(B.TEST_DATE,'yyyy-MM-dd') order by to_char(B.TEST_DATE,'yyyy-MM-dd')
							   ";
            var resultData = await _dbConnection.QueryAsync<dynamic>(resultDataSql, new { WO_NO });


            return new TableDataModel() { data = new { data, dtlData, resultData }, count = 0 };
        }
        #endregion

        #region 红胶,锡膏作业报表
        /// <summary>
        /// 红胶,锡膏作业报表
        /// </summary>
        /// <param name="page"></param>
        /// <param name="resourceNo"></param>
        /// <param name="startTime"></param>
        /// <returns></returns>
        public async Task<TableDataModel> GetResourceRuncardReport(PageModel page, string resourceNo, string code, DateTime? startTime = null)
        {
            var condition = " WHERE 1=1 ";
            if (startTime != null)
            {
                condition += " AND WARM_BEGIN>:START_TIME ";
            }
            if (!resourceNo.IsNullOrEmpty())
            {
                condition += " AND RESOURCE_NO = :RESOURCE_NO ";
            }
            if (!code.IsNullOrEmpty())
            {
                condition += " AND CODE = :CODE ";
            }
            var sql = $@"SELECT * FROM (SELECT A.*,ROWNUM rowno FROM(SELECT Y.* ,
                            (SELECT MAX(END_OPERATION_TIME) FROM 
                                (select * from SMT_RESOURCE_RUNCARD
                                 UNION
                                select * from SMT_LOG_RESOURCE_RUNCARD)
                             WHERE (CURRENT_OPERATION = 2 OR CURRENT_OPERATION=(CASE WHEN RESOURCE_ID = 1 THEN 6 ELSE 5 END)) 
                                    AND RESOURCE_NO = Y.RESOURCE_NO 
                                    AND END_OPERATION_TIME>Y.WARM_BEGIN) WARM_END,
                            (SELECT MAX(END_OPERATION_TIME) FROM 
                                (select * from SMT_RESOURCE_RUNCARD 
                                 UNION
                                select * from SMT_LOG_RESOURCE_RUNCARD)
                            WHERE (CURRENT_OPERATION = (CASE WHEN RESOURCE_ID = 1 THEN 3 ELSE -1 END) OR CURRENT_OPERATION=(CASE WHEN RESOURCE_ID = 1 THEN 7 ELSE -1 END)) 
                                    AND RESOURCE_NO = Y.RESOURCE_NO 
                                    AND END_OPERATION_TIME>Y.WARM_BEGIN) MIX_END,
                            (SELECT MAX(BEGIN_OPERATION_TIME) FROM 
                                (select * from SMT_RESOURCE_RUNCARD
                                 UNION
                                select * from SMT_LOG_RESOURCE_RUNCARD)
                            WHERE CURRENT_OPERATION = (CASE WHEN RESOURCE_ID = 1 THEN 5 ELSE 4 END) 
                                    AND RESOURCE_NO = Y.RESOURCE_NO 
                                    AND END_OPERATION_TIME>Y.WARM_BEGIN) ICE_BEGIN
                            FROM (SELECT RESOURCE_NO,DESCRIPTION,E.NICK_NAME,F.CN_DESC STATUS,RE.CN_DESC RESOURCE_NAME,
                            (SELECT MAX(BEGIN_OPERATION_TIME) FROM 
                                (select * from SMT_RESOURCE_RUNCARD
                                 UNION
                                select * from SMT_LOG_RESOURCE_RUNCARD)
                            WHERE (CURRENT_OPERATION = 2 OR CURRENT_OPERATION=(CASE WHEN RESOURCE_ID = 1 THEN 6 ELSE 5 END)) 
                                    AND RESOURCE_NO = A.RESOURCE_NO) WARM_BEGIN,
                            (SELECT MAX(BEGIN_OPERATION_TIME) FROM 
                                (select * from SMT_RESOURCE_RUNCARD
                                 UNION
                                select * from SMT_LOG_RESOURCE_RUNCARD)
                            WHERE (CURRENT_OPERATION = (CASE WHEN RESOURCE_ID = 1 THEN 3 ELSE -1 END) OR CURRENT_OPERATION=(CASE WHEN RESOURCE_ID = 1 THEN 7 ELSE -1 END)) 
                                    AND RESOURCE_NO = A.RESOURCE_NO) MIX_BEGIN,F.CODE
                        FROM SMT_RESOURCE_RUNCARD A LEFT JOIN SMT_RESOURCE_CATEGORY B ON A.CATEGORY_ID = B.CATEGORY_ID
                            LEFT JOIN SYS_MANAGER E ON A.OPERATOR = E.USER_NAME
                            LEFT JOIN SMT_LOOKUP F ON A.STATUS = F.CODE AND F.TYPE = 'RESOURCE_STATUS'
                            LEFT JOIN SMT_LOOKUP RE ON A.RESOURCE_ID = RE.CODE AND RE.TYPE = 'RESOURCE_OBJECT'
                            LEFT JOIN SMT_RESOURCE_RULES SRU ON A.CATEGORY_ID = SRU.CATEGORY_ID  AND A.ROUTE_ID = SRU.ROUTE_OPERATION_ID
                            WHERE SRU.ENABLED = 'Y') Y
                         {condition} ORDER BY CODE DESC, WARM_BEGIN DESC) A)
                    ";
            string pagedSql = SQLBuilderClass.GetPagedSQL(sql);

            var data = await _dbConnection.QueryAsync<dynamic>(pagedSql, new { RESOURCE_NO = resourceNo, START_TIME = startTime, page.Limit, page.Page });
            var countSql = $@"SELECT count(1)
                            FROM (SELECT RESOURCE_NO,
                            (SELECT MAX(BEGIN_OPERATION_TIME) FROM 
                                (select * from SMT_RESOURCE_RUNCARD
                                 UNION
                                select * from SMT_LOG_RESOURCE_RUNCARD)
                            WHERE (CURRENT_OPERATION = 2 OR CURRENT_OPERATION=(CASE WHEN RESOURCE_ID = 1 THEN 6 ELSE 5 END)) 
                                    AND RESOURCE_NO = A.RESOURCE_NO) WARM_BEGIN
                        FROM SMT_RESOURCE_RUNCARD A LEFT JOIN SMT_RESOURCE_CATEGORY B ON A.CATEGORY_ID = B.CATEGORY_ID ) Y
                         {condition}";
            int cnt = await _dbConnection.ExecuteScalarAsync<int>(countSql, new { RESOURCE_NO = resourceNo, START_TIME = startTime, page.Limit, page.Page, CODE = code });
            return new TableDataModel() { count = cnt, data = data };

        }
        #endregion

        #region QC报表
        /// <summary>
        /// QC报表
        /// </summary>
        /// <param name="lineId"></param>
        /// <param name="WO_NO"></param>
        /// <param name="date"></param>
        /// <returns></returns>
        public async Task<dynamic> GetQCReport(int lineId, string WO_NO, DateTime? date)
        {
            string dateStr = null;
            var dateCondition = "";
            var dateCondition2 = "";
            if (date != null)
            {
                dateStr = date.Value.ToString("yyyy-MM-dd");
                dateCondition = " AND TO_CHAR(WORK_TIME,'YYYY-MM-DD') = :dateStr ";
                dateCondition2 = " AND TIME=:dateStr ";
            }

            var dataSql = $@"SELECT WO_NO, MODEL, TARGET_QTY,
                              (SELECT SUM(OUTPUT_QTY)
                                  FROM MES_KANBAN_HOUR_YIDLD
                                 WHERE WO_NO = A.WO_NO {dateCondition}
                                   AND LINE_ID = :lineId) TOTAL
                         FROM SFCS_WO A
                         LEFT JOIN SFCS_MODEL B
                           ON A.MODEL_ID = B.ID
                         WHERE WO_NO = :WO_NO";
            var data = await _dbConnection.QueryAsync<dynamic>(dataSql, new { lineId, WO_NO, dateStr });


            var dtlSql = $@"SELECT * FROM (
                            SELECT A.DEFECT_CODE,DEFECT_DESCRIPTION,TO_CHAR(DEFECT_TIME,'YYYY-MM-DD') AS TIME,TO_CHAR(DEFECT_TIME,'HH24') AS HOUR,OPERATION_LINE_ID,WO_NO,COUNT(1) AS COUNT FROM SFCS_COLLECT_DEFECTS A 
                                LEFT JOIN SFCS_COLLECT_DEFECTS_DETAIL B ON A.COLLECT_DEFECT_DETAIL_ID = B.COLLECT_DEFECT_DETAIL_ID
                                LEFT JOIN SFCS_OPERATION_SITES C ON B.OPERATION_SITE_ID = C.ID
                                INNER JOIN SFCS_OPERATIONS D ON D.ID = C.OPERATION_ID AND OPERATION_CATEGORY=2
                                LEFT JOIN SFCS_DEFECT_CONFIG E ON A.DEFECT_CODE = E.DEFECT_CODE
                                LEFT JOIN SFCS_WO F ON A.WO_ID = F.ID
                            GROUP BY A.DEFECT_CODE,DEFECT_DESCRIPTION,TO_CHAR(DEFECT_TIME,'YYYY-MM-DD'),TO_CHAR(DEFECT_TIME,'HH24'),OPERATION_LINE_ID,WO_NO
                            UNION 
                            SELECT A.DEFECT_CODE,DEFECT_DESCRIPTION,TO_CHAR(REPORT_TIME,'YYYY-MM-DD') TIME,TO_CHAR(REPORT_TIME,'HH24') AS HOUR,OPERATION_LINE_ID,WO_NO,SUM(QTY) AS COUNT FROM SFCS_DEFECT_REPORT_WORK A
                                INNER JOIN SFCS_OPERATION_SITES B ON A.OPERATION_SITE_ID = B.ID
                                INNER JOIN SFCS_OPERATIONS D ON D.ID = B.OPERATION_ID AND OPERATION_CATEGORY=2
                                LEFT JOIN SFCS_DEFECT_CONFIG E ON A.DEFECT_CODE = E.DEFECT_CODE
                                LEFT JOIN SFCS_WO F ON A.WO_ID = F.ID
                            GROUP BY A.DEFECT_CODE,DEFECT_DESCRIPTION,TO_CHAR(REPORT_TIME,'YYYY-MM-DD'),TO_CHAR(REPORT_TIME,'HH24'),OPERATION_LINE_ID,WO_NO)
                        WHERE DEFECT_DESCRIPTION IS NOT NULL AND OPERATION_LINE_ID = :lineId AND WO_NO = :WO_NO {dateCondition2} ORDER BY DEFECT_CODE DESC";
            var dtlData = await _dbConnection.QueryAsync<dynamic>(dtlSql, new { lineId, WO_NO, dateStr });

            var resultSql = $@"SELECT * FROM ( SELECT TO_CHAR(OPERATION_TIME, 'YYYY-MM-DD') AS TIME,
                                    TO_CHAR(OPERATION_TIME, 'HH24') AS HOUR, COUNT(1) AS COUNT
                               FROM SFCS_OPERATION_HISTORY A
                               LEFT JOIN SFCS_OPERATIONS C
                                 ON SITE_OPERATION_ID = C.ID
                               LEFT JOIN SFCS_WO B
                                 ON B.ID = A.WO_ID
                               LEFT JOIN SFCS_OPERATION_SITES D
                                 ON A.OPERATION_SITE_ID = D.ID
                              WHERE OPERATION_CATEGORY = 2
                                AND WO_NO = :WO_NO
                                AND OPERATION_LINE_ID = :lineId
                              GROUP BY TO_CHAR(OPERATION_TIME, 'YYYY-MM-DD'),
                                       TO_CHAR(OPERATION_TIME, 'HH24')
                              UNION 
                            SELECT TO_CHAR(REPORT_TIME,'YYYY-MM-DD') TIME,TO_CHAR(REPORT_TIME,'HH24') AS HOUR,SUM(QTY) AS COUNT FROM SFCS_DEFECT_REPORT_WORK A
                                INNER JOIN SFCS_OPERATION_SITES B ON A.OPERATION_SITE_ID = B.ID
                                INNER JOIN SFCS_OPERATIONS D ON D.ID = B.OPERATION_ID AND OPERATION_CATEGORY=2
                                LEFT JOIN SFCS_DEFECT_CONFIG E ON A.DEFECT_CODE = E.DEFECT_CODE
                                LEFT JOIN SFCS_WO F ON A.WO_ID = F.ID
                            GROUP BY TO_CHAR(REPORT_TIME,'YYYY-MM-DD'),TO_CHAR(REPORT_TIME,'HH24'))
                              WHERE 1=1 {dateCondition2}
                             ";
            var resultData = await _dbConnection.QueryAsync<dynamic>(resultSql, new { lineId, WO_NO, dateStr });
            return new { data, dtlData, resultData };
        }
        #endregion

        #region 流程卡报表

        #region QuerySql
        public const string S_SelectRuncardView = @"SELECT   SR.ID SN_ID,
         SR.SN,
         SR.PARENT_SN,
         SW.WO_NO,
         SW.PART_NO,
         SW.OEM_PN,
         SP.CUSTOMER_PN,
         SC.CUSTOMER,
         SRT.ROUTE_NAME,
         SM.MODEL,
         SOS.OPERATION_SITE_NAME CURRENT_SITE,
         SO.DESCRIPTION WIP_ROUTE,
         SP1.DESCRIPTION RUNCARD_STATUS,
         SR.TURNIN_NO,
         STBH.SUBINVENTORY_CODE,
         SR.TRACKING_NO,
         SR.CARTON_NO,
         SR.PALLET_NO,
         SR.GG_NO,
         SR.GG_ITEM,
         SR.SMT_TURNIN_NO,
         SR.RMA_COUNT,
         SR.INPUT_TIME,
         SR.TURNIN_TIME,
         SR.SHIP_TIME,
         SR.REPLACE_FLAG,
         DECODE(NVL(SR.SAMPLE_FLAG, 'A'), 'N', '待抽检(N)', 'I', '不需要抽检(I)', 'Y', '已经抽检(Y)', 'A', '沒有抽检标记') SAMPLE_FLAG,
         SR.ROUTE_ID,
         SPF.FAMILY_NAME
  FROM   SFCS_RUNCARD SR 
         INNER JOIN SFCS_WO SW ON SR.WO_ID = SW.ID AND SR.SN = :SN
         INNER JOIN SFCS_ROUTES SRT ON SR.ROUTE_ID = SRT.ID
         INNER JOIN SFCS_PN SP ON SW.PART_NO = SP.PART_NO
         INNER JOIN SFCS_PARAMETERS SP1 ON SP1.LOOKUP_TYPE = 'RUNCARD_STATUS' AND SR.STATUS = SP1.LOOKUP_CODE
         INNER JOIN SFCS_OPERATIONS SO ON SR.WIP_OPERATION = SO.ID
         INNER JOIN SFCS_OPERATION_SITES SOS ON SR.CURRENT_SITE = SOS.ID
         LEFT JOIN SFCS_CUSTOMERS SC ON SP.CUSTOMER_ID = SC.ID
         LEFT JOIN SFCS_MODEL SM ON SP.MODEL_ID = SM.ID
         LEFT JOIN SFCS_PRODUCT_FAMILY SPF ON SP.FAMILY_ID = SPF.ID
         LEFT JOIN SFCS_TURNIN_BATCH_HEADER STBH ON SR.TURNIN_NO = STBH.BATCH_NO";

        public const string s_SnByUID = @"SELECT SRD.SN FROM SFCS_COLLECT_UIDS SCU,SFCS_RUNCARD SRD WHERE UID_NUMBER=:UID_NUMBER AND SRD.ID=SCU.SN_ID";

        //作业记录
        public const string S_SelectOpHistoryView = @"SELECT SRC.SN,SW.WO_NO,
           SR.ROUTE_NAME,
           SO.OPERATION_NAME,
           SO.DESCRIPTION AS OPERATION_DESCRIPTION,
           SOS.OPERATION_SITE_NAME,
           SOH.OPERATOR,
           SP.DESCRIPTION,
           SOH.OPERATION_TIME,
           SOH.VISIT_NUMBER
    FROM   SFCS_OPERATION_HISTORY SOH,
           SFCS_OPERATION_SITES SOS,
           SFCS_ROUTES SR,
           SFCS_OPERATIONS SO,
           SFCS_PARAMETERS SP,
           SFCS_WO SW,
           SFCS_RUNCARD SRC
   WHERE   SN_ID = :SN_ID
           AND SOH.WO_ID = SW.ID
           AND SOH.ROUTE_ID = SR.ID(+)
           AND SOH.SITE_OPERATION_ID = SO.ID(+)
           AND SOH.OPERATION_SITE_ID = SOS.ID(+)
           AND SP.LOOKUP_TYPE = 'RUNCARD_STATUS'
           AND SOH.OPERATION_STATUS = SP.LOOKUP_CODE
           AND SN_ID = SRC.ID
ORDER BY   SOH.OPERATION_TIME, SOH.ROWID ";
        //不良
        public const string S_SelectDefectHistory = @"
  SELECT SCD.COLLECT_DEFECT_ID,
         SCD.COLLECT_DEFECT_DETAIL_ID,
         SW.WO_NO,
         SCD.DEFECT_CODE,
         SDC.DEFECT_DESCRIPTION,
         SOS1.OPERATION_SITE_NAME DEFECT_SITE,
         SCD.DEFECT_TIME,
         SCD.DEFECT_OPERATOR,
         SRR.ROOT_CAUSE_CATEGORY,
         SRR.REASON_CODE,
         SRC.REASON_DESCRIPTION,
         SRR.LOCATION,
         SOS2.OPERATION_SITE_NAME REPAIR_SITE,
         SCD.REPAIRER,
         SCD.CHECK_OPERATOR,
         SCD.REPAIR_TIME,
         SCD.REPAIR_IN_TIME,
         SCD.REPAIR_IN_OPERATOR,
         SCD.REPAIR_OUT_TIME,
         SCD.REPAIR_OUT_OPERATOR,
         SCD.NDF_FLAG,
         SCD.REPAIR_FLAG,
         SRR.RESPONSER,
         SRR.ACTION_CODE,
         SRR.TTF,
         SRR.BAD_PART_NO,
         SRR.BAD_PART_VENDOR,
         SRR.REEL_ID,
         SRR.DEFECT_DESCRIPTION DEFECT_DETAIL_DESCRIPTION,
         SCD.ATTRIBUTE1,
         SCD.ATTRIBUTE2,
         SCD.ATTRIBUTE3,
         SCD.ATTRIBUTE4,
         SCD.ATTRIBUTE5
  FROM   SFCS_COLLECT_DEFECTS SCD,
        (SELECT R.* FROM  SFCS_REASON_CONFIG R, SFCS_RUNCARD D, 
           SFCS_WO W WHERE D.ID=:SN_ID AND D.WO_ID=W.ID AND R.REASON_CLASS = W.PLANT_CODE) SRC,
        (SELECT D.* FROM  SFCS_DEFECT_CONFIG D, SFCS_RUNCARD R, 
           SFCS_WO W WHERE R.ID=:SN_ID AND R.WO_ID=W.ID AND D.DEFECT_CLASS = W.PLANT_CODE) SDC,
         SFCS_WO SW,
         SFCS_REPAIR_RECIPE SRR,
         SFCS_OPERATION_SITES SOS1,
         SFCS_OPERATION_SITES SOS2
 WHERE   SN_ID = :SN_ID
         AND SCD.DEFECT_CODE = SDC.DEFECT_CODE(+)
         AND SCD.DEFECT_SITE_ID = SOS1.ID(+)
         AND SCD.REPAIR_SITE_ID = SOS2.ID(+)
         AND SCD.WO_ID = SW.ID 
         AND SCD.COLLECT_DEFECT_ID = SRR.COLLECT_DEFECT_ID(+)
         AND SRR.REASON_CODE = SRC.REASON_CODE(+) ";
        //测试记录
        public const string S_SelectTestHistoryView = @"select SR.SN,
        SOH.OPERATOR TEST_OPERATOR,
        SOH.OPERATION_TIME CREATE_TIME,
        SOS.OPERATION_SITE_NAME OPERATION_SITE_NAME,
        MAHD.ITEAM,
        MAHD.VALUE,
        DECODE(MAHD.ATTRIBUTE2,'0','NO TEST','1','SKIP','2','PASS','FAIL') STATUS 
        from MES_ATE_HEAD_DETAIL MAHD,SFCS_OPERATION_HISTORY SOH, SFCS_OPERATION_SITES SOS,SFCS_RUNCARD SR 
        where SR.ID = SOH.SN_ID 
        AND SOH.OPERATION_ID = MAHD.MST_ID 
        AND  SOH.OPERATION_SITE_ID = SOS.ID 
        AND SN = :SN_ID ";
        //零件
        public const string S_SelectCollectCompView = @"SELECT SCC.COLLECT_COMPONENT_ID,
         SR.SN,
         SW.WO_NO,
         SO.OPERATION_NAME,
         SCC.COMPONENT_NAME,
         SCC.ODM_COMPONENT_SN,
         SCC.ODM_COMPONENT_PN,
         SCC.CUSTOMER_COMPONENT_SN,
         SCC.CUSTOMER_COMPONENT_PN,
         SCC.COMPONENT_QTY,
         SCC.SERIALIZED,
         SOS.OPERATION_SITE_NAME,
         SCC.COLLECT_TIME,
         SCC.COLLECT_BY,
         SCC.REWORK_REMOVE_FLAG,
         SCC.REPLACE_FLAG,
         SCC.EDI_FLAG
  FROM   SFCS_COLLECT_COMPONENTS SCC,
         SFCS_WO SW,
         SFCS_ROUTE_CONFIG SRC,
         SFCS_OPERATIONS SO,
         SFCS_RUNCARD SR,
         SFCS_OPERATION_SITES SOS
 WHERE   SR.SN = :SN
         AND SCC.SN_ID = SR.ID
         AND SCC.WO_ID = SW.ID
         AND SCC.PRODUCT_OPERATION_CODE = SRC.PRODUCT_OPERATION_CODE
         AND SRC.CURRENT_OPERATION_ID = SO.ID(+)
         AND SCC.COLLECT_SITE = SOS.ID(+) ";
        //替换零件
        public const string S_SelectCompReplaceView = @"SELECT SCR.COMPONENT_NAME, SCR.NEW_CUSTOMER_COMPONENT_SN,
        SCR.OLD_CUSTOMER_COMPONENT_SN, SOS.OPERATION_SITE_NAME, SCR.SERIALIZED, SCR.COMPONENT_QTY,
        SCR.NEW_CUSTOMER_COMPONENT_PN, SCR.OLD_CUSTOMER_COMPONENT_PN,
        SCR.NEW_ODM_COMPONENT_PN, SCR.OLD_ODM_COMPONENT_PN, SCR.REPLACE_BY, SCR.REPLACE_TIME
        FROM SFCS_COMPONENT_REPLACE SCR,SFCS_OPERATION_SITES SOS
        WHERE SN_ID=:SN_ID AND SCR.REPLACE_SITE_ID=SOS.ID(+) ORDER BY REPLACE_TIME ";
        //制程
        public const string S_SelectRouteView = @"SELECT SO1.DESCRIPTION CURRENT_OPERATION,
        SO2.DESCRIPTION REPAIR_OPERATION,
        SO3.DESCRIPTION REWORK_OPERATION
        FROM SFCS_ROUTE_CONFIG SRC, SFCS_OPERATIONS SO1,  
        SFCS_OPERATIONS SO2,  SFCS_OPERATIONS SO3
        WHERE SRC.ROUTE_ID = :ROUTE_ID 
        AND SRC.CURRENT_OPERATION_ID = SO1.ID(+)
        AND SRC.REPAIR_OPERATION_ID = SO2.ID(+) 
        AND SRC.REWORK_OPERATION_ID = SO3.ID(+)
        ORDER BY SRC.ORDER_NO ";
        //返工前零件
        public const string S_SelectReworkCompView = @"SELECT SCC.COLLECT_COMPONENT_ID,
         SR.SN,
         SW.WO_NO,
         SO.OPERATION_NAME,
         SCC.COMPONENT_NAME,
         SCC.ODM_COMPONENT_SN,
         SCC.ODM_COMPONENT_PN,
         SCC.CUSTOMER_COMPONENT_SN,
         SCC.CUSTOMER_COMPONENT_PN,
         SCC.COMPONENT_QTY,
         SCC.SERIALIZED,
         SOS.OPERATION_SITE_NAME,
         SCC.COLLECT_TIME,
         SCC.COLLECT_BY,
         SCC.REWORK_REMOVE_FLAG,
         SCC.REPLACE_FLAG,
         SCC.EDI_FLAG
  FROM   JZMES.SFCS_COLLECT_COMPONENTS SCC,
         SFCS_WO SW,
         SFCS_ROUTE_CONFIG SRC,
         SFCS_OPERATIONS SO,
         SFCS_RUNCARD SR,
         SFCS_OPERATION_SITES SOS
 WHERE   SR.SN = :SN
         AND SCC.SN_ID = SR.ID
         AND SCC.WO_ID = SW.ID
         AND SCC.PRODUCT_OPERATION_CODE = SRC.PRODUCT_OPERATION_CODE
         AND SRC.CURRENT_OPERATION_ID = SO.ID(+)
         AND SCC.COLLECT_SITE = SOS.ID(+) ";//BSMT_LOG.SFCS_COLLECT_COMPONENTS SCC,
        //UID
        public const string S_SelectUIDView = @"SELECT SCU.COLLECT_UID_ID,
         SR.SN,
         SW.WO_NO,
         SCU.UID_NAME,
         SCU.UID_NUMBER,
         SP1.MEANING PLANT,
         SCU.UID_QTY,
         SCU.ORDER_NO,
         SCU.SERIALIZED,
         SCU.REWORK_REMOVE_FLAG,
         SCU.REPLACE_FLAG,
         SCU.EDI_FLAG,
         SOS.OPERATION_SITE_NAME,
         SCU.COLLECT_BY,
         SCU.COLLECT_TIME
  FROM   SFCS_COLLECT_UIDS SCU,
         SFCS_RUNCARD SR,
         SFCS_WO SW,
         SFCS_PARAMETERS SP1,
         SFCS_OPERATION_SITES SOS
 WHERE   SR.SN = :SN
         AND SR.ID = SCU.SN_ID
         AND SCU.WO_ID = SW.ID
         AND SP1.LOOKUP_CODE(+) = SCU.PLANT_CODE
         AND SCU.COLLECT_SITE = SOS.ID(+)  ";
        //流水号替换记录
        public const string S_SelectReplaceRuncardView = @"SELECT SRR.REPLACE_SN_ID, SRR.REPLACE_OPERATION_ID, SRR.SN_ID,
        SRR.OLD_SN, SRR.NEW_SN, SOS.OPERATION_SITE_NAME, SRR.REPLACE_REASON, 
        SRR.REPLACE_REMARK, SRR.REPLACE_BY, SRR.REPLACE_TIME
        FROM SFCS_RUNCARD_REPLACE SRR,SFCS_OPERATION_SITES SOS
        WHERE SRR.SN_ID=:SN_ID AND SRR.REPLACE_SITE_ID = SOS.ID(+) ORDER BY REPLACE_TIME ";
        //资源
        public const string S_SelectResourceView = @"SELECT   SCR.COLLECT_RESOURCE_ID,
                       SR.SN,
                       SW.WO_NO,
                       SO.OPERATION_NAME,
                       SCR.RESOURCE_NAME,
                       SCR.RESOURCE_NO,
                       SCR.RESOURCE_QTY,
                       SCR.REWORK_REMOVE_FLAG,
                       SCR.REPLACE_FLAG,
                       SCR.EDI_FLAG,
                       SOS.OPERATION_SITE_NAME,
                       SCR.COLLECT_BY,
                       SCR.COLLECT_TIME
                FROM   SFCS_COLLECT_RESOURCES SCR,
                       SFCS_RUNCARD SR,
                       SFCS_WO SW,
                       SFCS_ROUTE_CONFIG SRC,
                       SFCS_OPERATIONS SO,
                       SFCS_OPERATION_SITES SOS
               WHERE       SR.SN = :SN
                       AND SR.ID = SCR.SN_ID
                       AND SCR.WO_ID = SW.ID
                       AND SCR.PRODUCT_OPERATION_CODE = SRC.PRODUCT_OPERATION_CODE
                       AND SRC.CURRENT_OPERATION_ID = SO.ID(+)
                       AND SCR.COLLECT_SITE = SOS.ID(+)
                       AND RESOURCE_ID <> 31413
               UNION
              SELECT   SCR.COLLECT_RESOURCE_ID,
                       SR.SN,
                       SW.WO_NO,
                       SO.OPERATION_NAME,
                       SCR.RESOURCE_NAME,
                       SCR.RESOURCE_NO
                       || ' | '
                       || SPV.VENDOR_NAME
                       || ' | '
                       || SPV.VENDOR_DESCRIPTION
                       RESOURCE_NO,
                       SCR.RESOURCE_QTY,
                       SCR.REWORK_REMOVE_FLAG,
                       SCR.REPLACE_FLAG,
                       SCR.EDI_FLAG,
                       SOS.OPERATION_SITE_NAME,
                       SCR.COLLECT_BY,
                       SCR.COLLECT_TIME
                FROM   SFCS_COLLECT_RESOURCES SCR,
                       SFCS_RUNCARD SR,
                       SFCS_WO SW,
                       SFCS_ROUTE_CONFIG SRC,
                       SFCS_OPERATIONS SO,
                       SFCS_OPERATION_SITES SOS,
                       SFCS_PRODUCT_VENDOR SPV
               WHERE       SR.SN = :SN
                       AND SR.ID = SCR.SN_ID
                       AND SCR.WO_ID = SW.ID
                       AND SCR.PRODUCT_OPERATION_CODE = SRC.PRODUCT_OPERATION_CODE
                       AND SRC.CURRENT_OPERATION_ID = SO.ID(+)
                       AND SCR.COLLECT_SITE = SOS.ID(+)
                       AND RESOURCE_ID = 31413
                       AND SCR.RESOURCE_NO = SPV.VENDOR_CODE";
        //RMA
        public const string S_SelectRmaRuncard = @"SELECT SRR.ID,SR.SN,SRR.COUNT,SRM.RMA_NO,SRR.RETURNED_SITE,SRR.RMA_TYPE,SRR.OWNER,SRR.MIC_RMA_NO,
            SRR.PCB_DATECODE,SRR.PCB_VENDER,SRR.CUSTOMER_SYMPTOM,SRR.ON_SITE_VERIFICATION,SRR.ODM_SYMPTOM,
            SRR.STATUS,SRR.LEVEL_FLAG,SRR.IS_WARRANTY,SRR.IS_DAMAGE,SRR.IS_CND,SRR.LAST_SHIP_TIME,
            SRR.COLLECT_TIME,SRR.COLLECT_BY,SRR.INPUT_TIME,SRR.PACKING_TIME, SRR.ENABLED,
            SRR.CUSTOMER_TEST_SITE,SRR.IS_DEPOSIT,SRR.TAKE_OUT_INVENTORY
            FROM SFCS_RMA_RECEIPT SRR, SFCS_RUNCARD SR, SFCS_RMA SRM
            WHERE SR.ID = SRR.SN_ID AND SRR.RMA_ID = SRM.ID AND SR.SN = :SN ORDER BY SRR.COUNT DESC";
        //SELECT SRR.ID, SR.SN, SRR.COUNT, SRM.RMA_NO, SRR.RETURNED_SITE, SRR.RMA_TYPE, SRR.OWNER, SRR.MIC_RMA_NO,
        //    SRR.PCB_DATECODE, SRR.PCB_VENDER, SRR.CUSTOMER_SYMPTOM, SRR.ON_SITE_VERIFICATION, SRR.ODM_SYMPTOM,
        //    SRR.STATUS, SRR.LEVEL_FLAG, SRR.IS_WARRANTY, SRR.IS_DAMAGE, SRR.IS_CND, SRR.LAST_SHIP_TIME,
        //    SRR.COLLECT_TIME, SRR.COLLECT_BY, SRR.INPUT_TIME, SRR.PACKING_TIME, SRR.ENABLED,
        //    SRR.CUSTOMER_TEST_SITE, SRR.IS_DEPOSIT, SRR.TAKE_OUT_INVENTORY
        //    FROM SFCS_RMA_RECEIPT SRR, SFCS_RUNCARD SR, SFCS_RMA SRM
        //    WHERE SR.ID = SRR.SN_ID AND SRR.RMA_ID = SRM.ID
        //    AND (/*?SN<SR.SN = :SN|1=1>*/ :SN IS NULL)
        //    AND(/*?RMA_NO<SRM.RMA_NO = :RMA_NO|1=1>*/ :RMA_NO IS NULL)
        //    ORDER BY SRR.COUNT DESC
        //工单记录
        public const string S_SelectWOReplaceView = @"SELECT SW1.WO_NO, SW1.PART_NO, SW2.WO_NO NEW_WO_NO, SW2.PART_NO NEW_PART_NO,
              SP.CHINESE WO_REPLACE_TYPE, SOS.OPERATION_SITE_NAME, 
              TO_CHAR(SWR.REPLACE_TIME,'yyyy/mm/dd HH24:mi:ss') REPLACE_TIME,
              '第' || SWR.REPLACE_ORDER_NO || '替換' REPLACE_TIMES FROM SFCS_WO_REPLACE SWR,
              SFCS_WO SW1, SFCS_WO SW2, SFCS_OPERATION_SITES SOS, SFCS_PARAMETERS SP
              WHERE SWR.OLD_WO_ID = SW1.ID AND SWR.NEW_WO_ID = SW2.ID AND SWR.REPLACE_SITE_ID = SOS.ID
              AND SP.LOOKUP_TYPE = 'WO_REPLACE_TYPE' AND SP.LOOKUP_CODE = SWR.REPLACE_TYPE
              AND SWR.SN_ID = :SN_ID ORDER BY REPLACE_ORDER_NO ";
        #endregion

        /// <summary>
        /// 根据SN获取产品流水信息
        /// </summary>
        /// <param name="sn">产品流水号</param>
        /// <param name="type">类型</param>
        /// <returns></returns>
        public async Task<RuncardReportListModel> GetRuncardInfoBySn(string sn, String type)
        {

            if ("UID".Equals(type))
            {
                sn = (await _dbConnection.QueryAsync<string>(s_SnByUID, new { UID_NUMBER = sn }))?.FirstOrDefault() ?? sn;
            }

            RuncardReportListModel model = (await _dbConnection.QueryAsync<RuncardReportListModel>(S_SelectRuncardView, new { SN = sn }))?.FirstOrDefault();
            if (model != null)
            {
                //根据SNID获取作业记录
                model.OPERATIONHISTORY = _dbConnection.QueryAsync<OperationHistoryListModel>(S_SelectOpHistoryView, new { SN_ID = model.SN_ID }).Result.ToList();

                //根据SNID获取不良维修记录
                model.DEFECTHISTORY = _dbConnection.QueryAsync<DefectHistoryListModel>(S_SelectDefectHistory, new { SN_ID = model.SN_ID }).Result.ToList();

                //根据SNID获取测试记录
                model.TESTHISTORY = _dbConnection.QueryAsync<TestHistoryListModel>(S_SelectTestHistoryView, new { SN_ID = model.SN_ID }).Result.ToList();

                //根据SN_ID替换零件记录  SFCS_COMPONENT_REPLACE不存在
                model.COMPREPLACEHISTORY = _dbConnection.QueryAsync<CompReplaceHistoryListModel>(S_SelectCompReplaceView, new { SN_ID = model.SN_ID }).Result.ToList();

                //根据ROUTE_ID获取制程
                model.ROUTEHISTORY = _dbConnection.QueryAsync<RouteHistoryListModel>(S_SelectRouteView, new { ROUTE_ID = model.ROUTE_ID }).Result.ToList();

                //流水号替换记录
                model.REPLACERUNCARDHISTORY = _dbConnection.QueryAsync<ReplaceRuncardHistoryListModel>(S_SelectReplaceRuncardView, new { SN_ID = model.SN_ID }).Result.ToList();

                //资源
                model.RESOURCEHISTORY = (_dbConnection.QueryAsync<ResourceHistoryListModel>(S_SelectResourceView, new { SN = sn })).Result.ToList();

                //RMA  SFCS_RMA_RECEIPT SFCS_RMA不存在
                model.RMAHISTORY = _dbConnection.QueryAsync<RmaHistoryListModel>(S_SelectRmaRuncard, new { SN = sn }).Result.ToList();

                //工单记录
                model.WOHISTORY = _dbConnection.QueryAsync<WOReplaceHistoryListModel>(S_SelectWOReplaceView, new { SN_ID = model.SN_ID }).Result.ToList();

                model.COLLECTCOMPONENTS = _dbConnection.QueryAsync<SfcsCollectComponentsListModel>("SELECT* FROM SFCS_COLLECT_COMPONENTS WHERE OPERATION_ID IN(SELECT OPERATION_ID FROM SFCS_OPERATION_HISTORY WHERE SN_ID = :SN_ID)", new { SN_ID = model.SN_ID }).Result.ToList();

                model.COLLECTRESOURCES = _dbConnection.QueryAsync<SfcsCollectResourcesListModel>("SELECT* FROM SFCS_COLLECT_RESOURCES WHERE OPERATION_ID IN(SELECT OPERATION_ID FROM SFCS_OPERATION_HISTORY WHERE SN_ID = :SN_ID)", new { SN_ID = model.SN_ID }).Result.ToList();

                model.COLLECTUIDS = _dbConnection.QueryAsync<SfcsCollectUidsListModel>("SELECT* FROM SFCS_COLLECT_UIDS WHERE OPERATION_ID IN(SELECT OPERATION_ID FROM SFCS_OPERATION_HISTORY WHERE SN_ID = :SN_ID)", new { SN_ID = model.SN_ID }).Result.ToList();

            }
            else
            {
                model = new RuncardReportListModel();
            }

            //辅料信息
            model.RESOURCE = _dbConnection.QueryAsync<ResourceListModel>("SELECT S.RESOURCE_NO,S.CREATE_TIME,S.CREATE_BY,L.OPERATION_LINE_NAME AS LINE_NAME FROM SMT_RESOURCE_SN SRS,SMT_RESOURCE_WO S,SFCS_OPERATION_LINES L WHERE SRS.RESOURCE_WO_ID = S.ID AND S.LINE_ID = L.ID AND SRS.SN = :SN ", new { SN = sn }).Result.ToList();

            //钢网信息
            model.STENCIL = _dbConnection.QueryAsync<StencilListModel>("SELECT S.STENCIL_NO,S.CREATE_TIME,S.CREATE_BY,L.OPERATION_LINE_NAME AS LINE_NAME FROM SMT_STENCIL_SN SSS,SMT_STENCIL_WO S,SFCS_OPERATION_LINES L WHERE SSS.STENCIL_WO_ID = S.ID AND S.LINE_ID = L.ID AND SSS.SN = :SN ", new { SN = sn }).Result.ToList();

            //刮刀信息
            model.SCRAPER = _dbConnection.QueryAsync<ScraperListModel>("SELECT S.SCRAPER_NO,S.CREATE_TIME,S.CREATE_BY,L.OPERATION_LINE_NAME AS LINE_NAME FROM SMT_SCRAPER_sn SSS,SMT_SCRAPER_WO S,SFCS_OPERATION_LINES L WHERE SSS.SCRAPER_WO_ID = S.ID AND S.LINE_ID = L.ID AND SSS.SN = :SN", new { SN = sn }).Result.ToList();

            //根据SN获取零件记录
            model.COMPONENTHISTORY = _dbConnection.QueryAsync<ComponentHistoryListModel>(S_SelectCollectCompView, new { SN = sn }).Result.ToList();

            //返工前零件
            model.REWORKCOMPHISTORY = _dbConnection.QueryAsync<ReworkCompHistoryListModel>(S_SelectReworkCompView, new { SN = sn }).Result.ToList();

            //UID
            model.UIDHISTORY = _dbConnection.QueryAsync<UidHistoryListModel>(S_SelectUIDView, new { SN = sn }).Result.ToList();

            //料卷信息
            model.SMTSNREEL = _dbConnection.QueryAsync<SNReelListModel>("SELECT SSR.SN, SSR.PART_NO, IP.NAME, IP.DESCRIPTION, SSR.FEEDER, SSR.LOCATION, SSR.TRACE_TIME, SRL.VENDOR_CODE,SRL.VENDOR_NAME,SRL.LOT_CODE FROM SMT_SN_REEL SSR, IMS_PART IP,SMT_REEL SRL WHERE SSR.PART_NO = IP.CODE AND SSR.REEL_ID = SRL.REEL_ID AND SSR.SN = :SN ORDER BY SSR.TRACE_TIME DESC", new { SN = sn }).Result.ToList();

            return model;
        }
        #endregion

        #region 站点统计报表
        /// <summary>
        /// 根据条件获取相应的查询列表
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public async Task<TableDataModel> GetSiteStatisticsConditionList(StatisticsConditionRequestModel model) 
        {
            int page = 0, limit = 0;
            page = model.Page * model.Limit - model.Limit + 1;
            limit = model.Page * model.Limit;
            model.Page = page;
            model.Limit = limit;

            WhereListModel wModel = GetWhereSqlByType(model.TYPE, model.Key);

            string sQuery = string.Format("SELECT * FROM (SELECT ROWNUM R, T.* FROM ( SELECT {0} FROM {1} WHERE 1=1 {2}) T) WHERE R BETWEEN :Page AND :Limit", wModel.FIELDNAME, wModel.VIEWNAME, wModel.WHERE);
            var resdata = await _dbConnection.QueryAsync<object>(sQuery, model);

            sQuery = string.Format("SELECT COUNT(1) FROM ( SELECT {0} FROM {1} WHERE 1=1 {2})", wModel.FIELDNAME, wModel.VIEWNAME, wModel.WHERE);

            int cnt = await _dbConnection.ExecuteScalarAsync<int>(sQuery, model);
            return new TableDataModel
            {
                count = cnt,
                data = resdata?.ToList(),
            };
        }

        private WhereListModel GetWhereSqlByType(int type, string key)
        {
            WhereListModel model = new WhereListModel();
            model.TYPE = type;
            switch (type)
            {
                case 1:
                    model.VIEWNAME = "SFCS_MODEL";
                    model.FIELDNAME = "MODEL AS NAME";
                    if (key.IsNullOrWhiteSpace())
                    {
                        model.WHERE = "AND ENABLED = 'Y' ";
                    }
                    else
                    {
                        model.WHERE = "AND ENABLED = 'Y' AND INSTR(MODEL,:Key)> 0";
                    }
                    model.WHERE += " GROUP BY MODEL ORDER BY MODEL ASC";
                    break;
                case 2:
                    model.VIEWNAME = "SFCS_PN SP LEFT JOIN IMS_PART IP ON SP.PART_NO = IP.CODE";
                    model.FIELDNAME = "SP.ID,SP.PART_NO AS NAME,IP.DESCRIPTION";
                    if (!key.IsNullOrWhiteSpace())
                    {
                        model.WHERE = "AND INSTR(SP.PART_NO,:Key)> 0";
                    }
                    model.WHERE += " ORDER BY SP.PART_NO ASC";
                    break;
                case 3:
                    model.VIEWNAME = "SFCS_WO SW LEFT JOIN IMS_PART IP ON SW.PART_NO = IP.CODE";
                    model.FIELDNAME = "SW.ID,SW.WO_NO AS NAME,SW.PART_NO,IP.DESCRIPTION";
                    if (!key.IsNullOrWhiteSpace())
                    {
                        model.WHERE = "AND INSTR(SW.WO_NO,:Key)> 0";
                    }
                    model.WHERE += " ORDER BY SW.WO_NO ASC";
                    break;
                case 4:
                    model.VIEWNAME = "SFCS_OPERATION_LINES";
                    model.FIELDNAME = "ID,OPERATION_LINE_NAME AS NAME";
                    if (key.IsNullOrWhiteSpace())
                    {
                        model.WHERE = "AND ENABLED = 'Y' ";
                    }
                    else
                    {
                        model.WHERE = "AND ENABLED = 'Y' AND INSTR(OPERATION_LINE_NAME,:Key)> 0";
                    }
                    model.WHERE += " ORDER BY OPERATION_LINE_NAME ASC";
                    break;
                case 5:
                    model.VIEWNAME = "SFCS_ROUTES";
                    model.FIELDNAME = "ID, ROUTE_NAME AS NAME";
                    if (key.IsNullOrWhiteSpace())
                    {
                        model.WHERE = "AND ENABLED = 'Y' ";
                    }
                    else
                    {
                        model.WHERE = "AND ENABLED = 'Y' AND (INSTR(ROUTE_NAME,:Key)> 0 OR INSTR(PART_NO,:Key)> 0) ";
                    }
                    model.WHERE += " ORDER BY ROUTE_NAME ASC";
                    break;
                case 6:
                    model.VIEWNAME = "SYS_USERS";
                    model.FIELDNAME = "EMPNO AS NAME";
                    if (key.IsNullOrWhiteSpace())
                    {
                        model.WHERE = "AND STATUS = 'Y' ";
                    }
                    else
                    {
                        model.WHERE = "AND STATUS = 'Y' AND INSTR(EMPNO,:Key)> 0";
                    }
                    model.WHERE += " GROUP BY EMPNO ORDER BY EMPNO ASC";
                    break;
                case 7:
                    model.VIEWNAME = "SFCS_RUNCARD";
                    model.FIELDNAME = "ID, SN AS NAME";
                    if (!key.IsNullOrWhiteSpace())
                    {
                        model.WHERE = "AND INSTR(SN,:Key)> 0 ";
                    }
                    model.WHERE += " ORDER BY SN ASC";
                    break;
                case 8:
                    model.VIEWNAME = "SFCS_PARAMETERS";
                    model.FIELDNAME = "LOOKUP_CODE AS ID,MEANING AS NAME";
                    if (key.IsNullOrWhiteSpace())
                    {
                        model.WHERE = "AND LOOKUP_TYPE= 'PHYSICAL_LOCATION' ";
                    }
                    else
                    {
                        model.WHERE = "AND LOOKUP_TYPE= 'PHYSICAL_LOCATION' AND INSTR(MEANING,:Key)> 0 ";
                    }
                    model.WHERE += " ORDER BY LOOKUP_CODE ASC";
                    break;
                default:
                    model = null;
                    break;
            }
            return model;
        }

        /// <summary>
        /// 获取站点统计数据表格
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public async Task<List<SiteStatisticsTableListModel>> GetSiteStatisticsDataTable(StatisticalReportRequestModel model)
        {
            string sQuery = S_SelectSiteStatisticsByAll1;

            if (!model.ALL)
            {
                if (model.ROUTE_ID != null && model.ROUTE_ID.Count() > 0)
                {
                    string route = SplicingDecilmal(model.ROUTE_ID);
                    sQuery += string.Format(S_SelectSiteStatisticsByRoute, route);
                }

                if (model.LINE_ID != null && model.LINE_ID.Count() > 0)
                {
                    string subline = SplicingDecilmal(model.LINE_ID);
                    sQuery += string.Format(S_SelectSiteStatisticsBySubline, subline);
                }
                if (model.MODEL != null && model.MODEL.Count > 0)
                {
                    //起始时间 结束时间 机种 线别id
                    string mo = SplicingStr(model.MODEL);
                    sQuery += string.Format(S_SelectSiteStatisticsByModel, mo);
                }
                if (model.PART_NO != null && model.PART_NO.Count > 0)
                {
                    string pn = SplicingStr(model.PART_NO);
                    sQuery += string.Format(S_SelectSiteStatisticsByPN, pn);
                }
                if (model.WO_NO != null && model.WO_NO.Count > 0)
                {
                    //起始时间 结束时间 工单 线别id
                    string wo = SplicingStr(model.WO_NO);
                    sQuery += string.Format(S_SelectSiteStatisticsByWO, wo);
                }
            }  
            
            sQuery += S_SelectSiteStatisticsEnd;

            return (await _dbConnection.QueryAsync<SiteStatisticsTableListModel>(sQuery, model)).ToList();
        }

        /// <summary>
        /// 获取站点统计明细数据
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public async Task<List<SiteStatisticsDetailListModel>> GetSiteStatisticsDetail(StatisticalReportRequestModel model)
        {
            string sQuery = S_SelectDetailStatisticsByAll;
            if (!model.ALL)
            {
                if (model.ROUTE_ID != null && model.ROUTE_ID.Count() > 0)
                {
                    string route = SplicingDecilmal(model.ROUTE_ID);
                    sQuery += string.Format(S_SelectSiteStatisticsByRoute, route);
                }

                if (model.LINE_ID != null && model.LINE_ID.Count() > 0)
                {
                    string subline = SplicingDecilmal(model.LINE_ID);
                    sQuery += string.Format(S_SelectSiteStatisticsBySubline, subline);
                }
                if (model.MODEL != null && model.MODEL.Count > 0)
                {
                    //起始时间 结束时间 机种 线别id
                    string mo = SplicingStr(model.MODEL);
                    sQuery += string.Format(S_SelectSiteStatisticsByModel, mo);
                }
                if (model.PART_NO != null && model.PART_NO.Count > 0)
                {
                    string pn = SplicingStr(model.PART_NO);
                    sQuery += string.Format(S_SelectSiteStatisticsByPN, pn);
                }
                if (model.WO_NO != null && model.WO_NO.Count > 0)
                {
                    //起始时间 结束时间 工单 线别id
                    string wo = SplicingStr(model.WO_NO);
                    sQuery += string.Format(S_SelectSiteStatisticsByWO, wo);
                }
            }
            sQuery += S_SelectDetailStatisticsEnd;
            return (await _dbConnection.QueryAsync<SiteStatisticsDetailListModel>(sQuery, model)).ToList();
        }

        /// <summary>
        /// 获取站点统计每小时明细数据
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public async Task<List<SiteStatisticsDetailListModel>> GetSiteStatisticsHourDetail(StatisticalReportRequestModel model)
        {
            string sQuery = S_SelectHourDetailStatisticsByAll;
            if (!model.ALL)
            {
                if (model.ROUTE_ID != null && model.ROUTE_ID.Count() > 0)
                {
                    string route = SplicingDecilmal(model.ROUTE_ID);
                    sQuery += string.Format(S_SelectSiteStatisticsByRoute, route);
                }

                if (model.LINE_ID != null && model.LINE_ID.Count() > 0)
                {
                    string subline = SplicingDecilmal(model.LINE_ID);
                    sQuery += string.Format(S_SelectSiteStatisticsBySubline, subline);
                }
                if (model.MODEL != null && model.MODEL.Count > 0)
                {
                    string mo = SplicingStr(model.MODEL);
                    sQuery += string.Format(S_SelectSiteStatisticsByModel, mo);
                }
                if (model.PART_NO != null && model.PART_NO.Count > 0)
                {
                    string pn = SplicingStr(model.PART_NO);
                    sQuery += string.Format(S_SelectSiteStatisticsByPN, pn);
                }
                if (model.WO_NO != null && model.WO_NO.Count > 0)
                {
                    //起始时间 结束时间 工单 线别id
                    string wo = SplicingStr(model.WO_NO);
                    sQuery += string.Format(S_SelectSiteStatisticsByWO, wo);
                }
            }
            sQuery += S_SelectHourDetailStatisticsEnd;
            return (await _dbConnection.QueryAsync<SiteStatisticsDetailListModel>(sQuery, model)).ToList();
        }

        private string SplicingStr(List<string> str)
        {
            string result = "";
            if (str.Count == 1)
            {
                result = str[0];
            }
            else if (str.Count > 0)
            {
                result = string.Join("','", str.ToArray());
            }
            result = string.Format("'{0}'", result);
            return result;
        }

        private string SplicingDecilmal(List<string> str)
        {
            String result = "";
            if (str.Count == 1)
            {
                result = str[0];
            }
            else if (str.Count > 0)
            {
                result = string.Join(",", str.ToArray());
            }
            result = string.Format("{0}", result);
            return result;
        }

        #region site statistics sql

        public static string S_SelectWithRMAWO = @" AND SW.WO_TYPE !=3 ";

        public static string S_SelectSiteStatisticsBySubline = @" AND SOS.OPERATION_LINE_ID IN ({0}) ";

        public static string S_SelectSiteStatisticsByRoute = @" AND SRC.ROUTE_ID IN ({0}) ";

        public static string S_SelectSiteStatisticsEnd = @" AND SW.ID = SSS.WO_ID  AND SSS.OPERATION_SITE_ID = SOS.ID AND SOS.OPERATION_ID = SO.ID
                       GROUP BY   SO.DESCRIPTION, SO.ID) A
                ORDER BY  A.ORDER_NO ";

        public static string S_SelectSiteStatisticsByAll1 = @" 
        SELECT  
           ROUTE_CODE AS OPERATION_ID,
           DESCRIPTION AS OPERATION_NAME,
           PASS,
           FAIL,
           REPASS,
           REFAIL,
           TRUNC (PASS / DECODE( (PASS + FAIL), 0, 1, (PASS + FAIL)), 4) YIELD,
           PASS+FAIL TOTAL
        FROM (  SELECT 
                        SUM(SSS.PASS) PASS,
                        SUM(SSS.FAIL) FAIL,
                        SUM(SSS.REPASS) REPASS,
                        SUM(SSS.REFAIL) REFAIL,
                       SO.DESCRIPTION,
                       SO.ID ROUTE_CODE,
                       MAX (SRC.ORDER_NO) ORDER_NO
                FROM   SFCS_WO SW, SFCS_OPERATIONS SO, SFCS_ROUTE_CONFIG SRC,SFCS_MODEL SM,SFCS_SITE_STATISTICS SSS,SFCS_OPERATION_SITES SOS
               WHERE   SW.ROUTE_ID = SRC.ROUTE_ID
                       AND SRC.CURRENT_OPERATION_ID = SO.ID
                       AND SW.MODEL_ID = SM.ID(+)
                       AND SO.ENABLED = 'Y'
                       AND SSS.WORK_TIME >=:BEGIN_TIME AND SSS.WORK_TIME <=:END_TIME";

        public static string S_SelectSiteStatisticsByAll2 = @" 
            GROUP BY   SO.DESCRIPTION, SO.ID) A,
           (  SELECT   
                    A.OPERATION_ID,
                       SUM (B.PASS) PASS,
                       SUM (B.FAIL) FAIL,
                       SUM (B.REPASS) REPASS,
                       SUM (B.REFAIL) REFAIL
                FROM   SFCS_OPERATION_SITES A, SFCS_SITE_STATISTICS B, SFCS_WO SW,SFCS_MODEL SM
               WHERE   SW.ID=B.WO_ID AND A.ID = B.OPERATION_SITE_ID
                       AND SW.MODEL_ID = SM.ID
                       AND B.WORK_TIME >=:BEGIN_TIME AND B.WORK_TIME <=:END_TIME ";


        public static string S_SelectSiteStatisticsByWO = @" AND  WO_NO IN ({0})";


        public static string S_SelectSiteStatisticsByPN = @" AND SW.PART_NO IN ({0}) ";


        public static string S_SelectSiteStatisticsByModel = @" AND SM.MODEL IN({0}) ";

        #endregion

        #region detail site statistics sql


        public static string S_SelectDetailStatisticsEnd = @" GROUP BY SO.DESCRIPTION, SO.ID ,SRC.ORDER_NO,SOS.OPERATION_SITE_NAME,
          TRUNC (SSS.WORK_TIME),
         SM.MODEL,
         SW.WO_NO
        ) A 
         ORDER BY A.ORDER_NO,A.WORK_TIME ";

        public const string S_SelectDetailStatisticsByAll = @" 
            SELECT  
                    A.OPERATION_NAME, 
                    A.WORK_TIME, 
                    A.OPERATION_SITE_NAME,
                    A.MODEL, 
                    A.WO_NO, 
                    A.PASS, 
                    A.FAIL, 
                    A.REPASS, 
                    A.REFAIL,
                    TRUNC (PASS /DECODE ( (PASS + FAIL), 0, 1, (PASS + FAIL)), 4) YIELD
                FROM ( 
                 SELECT  
                 SUM(SSS.PASS) PASS,
                 SUM(SSS.FAIL) FAIL,
                 SUM(SSS.REPASS) REPASS,
                 SUM(SSS.REFAIL) REFAIL,
                 SOS.OPERATION_SITE_NAME,
                 TRUNC (SSS.WORK_TIME) WORK_TIME,
                 SM.MODEL,
                 SW.WO_NO,
                 SO.DESCRIPTION AS OPERATION_NAME, 
                 SO.ID ROUTE_CODE,
                SRC.ORDER_NO ORDER_NO
                FROM SFCS_WO SW, SFCS_OPERATIONS SO, SFCS_ROUTE_CONFIG SRC, SFCS_MODEL SM,SFCS_SITE_STATISTICS SSS,SFCS_OPERATION_SITES SOS
                WHERE SW.ROUTE_ID = SRC.ROUTE_ID
                AND SRC.CURRENT_OPERATION_ID = SO.ID
                AND SW.MODEL_ID = SM.ID
                AND SO.ENABLED = 'Y'
                AND SSS.WO_ID = SW.ID
                AND SSS.OPERATION_SITE_ID = SOS.ID
                AND SOS.OPERATION_ID = SO.ID
                AND SSS.WORK_TIME >=:BEGIN_TIME AND SSS.WORK_TIME <=:END_TIME ";

        #endregion

        #region hour detail statistics sql

        public static string S_SelectHourDetailStatisticsEnd = @" GROUP BY SO.DESCRIPTION, SO.ID ,SRC.ORDER_NO,SOS.OPERATION_SITE_NAME,
 SSS.WORK_TIME,
 SM.MODEL,
 SW.WO_NO
) A 
 ORDER BY A.ORDER_NO,A.WORK_TIME ";

        public const string S_SelectHourDetailStatisticsByAll = @" 
           SELECT  
            A.OPERATION_NAME, 
            A.WORK_TIME, 
            A.OPERATION_SITE_NAME,
            A.MODEL, 
            A.WO_NO, 
            A.PASS, 
            A.FAIL, 
            A.REPASS, 
            A.REFAIL,
            TRUNC (PASS /DECODE ( (PASS + FAIL), 0, 1, (PASS + FAIL)), 4) YIELD
        FROM ( 
         SELECT  
         SUM(SSS.PASS) PASS,
         SUM(SSS.FAIL) FAIL,
         SUM(SSS.REPASS) REPASS,
         SUM(SSS.REFAIL) REFAIL,
         SOS.OPERATION_SITE_NAME,
         SSS.WORK_TIME,
         SM.MODEL,
         SW.WO_NO,
         SO.DESCRIPTION AS OPERATION_NAME, 
         SO.ID ROUTE_CODE,
        SRC.ORDER_NO ORDER_NO
        FROM SFCS_WO SW, SFCS_OPERATIONS SO, SFCS_ROUTE_CONFIG SRC, SFCS_MODEL SM,SFCS_SITE_STATISTICS SSS,SFCS_OPERATION_SITES SOS
        WHERE SW.ROUTE_ID = SRC.ROUTE_ID
        AND SRC.CURRENT_OPERATION_ID = SO.ID
        AND SW.MODEL_ID = SM.ID(+)
        AND SO.ENABLED = 'Y'
        AND SSS.WO_ID = SW.ID
        AND SSS.OPERATION_SITE_ID = SOS.ID
        AND SOS.OPERATION_ID = SO.ID
        AND SSS.WORK_TIME >=:BEGIN_TIME AND SSS.WORK_TIME <=:END_TIME";

        #endregion
        #endregion

        #region 站点统计报表获取detail report
        /// <summary>
        /// 站点统计报表获取detail report
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public async Task<List<SiteStatisticsReportDetailListModel>> GetSiteStatisticsReportDetail(ReportDetailRequestModel model)
        {
            String whereConition = "";
            if (!model.ALL)
            {

                if (model.LINE_ID != null && model.LINE_ID.Count() > 0)
                {
                    string subline = SplicingDecilmal(model.LINE_ID);
                    whereConition += string.Format(S_SelectSiteStatisticsBySubline, subline);
                }
                if (model.MODEL != null && model.MODEL.Count > 0)
                {
                    //起始时间 结束时间 机种 线别id
                    string mo = SplicingStr(model.MODEL);
                    whereConition += string.Format(S_SelectSiteStatisticsByModel, mo);
                }
                if (model.PART_NO != null && model.PART_NO.Count > 0)
                {
                    string pn = SplicingStr(model.PART_NO);
                    whereConition += string.Format(S_SelectSiteStatisticsByPN, pn);
                }
                if (model.WO_NO != null && model.WO_NO.Count > 0)
                {
                    //起始时间 结束时间 工单 线别id
                    string wo = SplicingStr(model.WO_NO);
                    whereConition += string.Format(S_SelectSiteStatisticsByWO, wo);
                }
            }
            //string sQuery = "", data = "", subline = (model.LINE_ID != null && model.LINE_ID.Count() > 0) ? SplicingStr(model.LINE_ID) : "";
            string sQuery = GetDetailReportByAll(model.CONDITION, whereConition);

            //if (model.ALL)
            //{
            //    sQuery = GetDetailReportByAll(model.CONDITION, subline);
            //}

            //if (model.MODEL != null && model.MODEL.Count > 0)
            //{
            //    data = SplicingStr(model.MODEL);
            //    sQuery = GetDetailReportByModel(model.CONDITION, data, subline);
            //}

            //if (model.PART_NO != null && model.PART_NO.Count > 0)
            //{
            //    data = SplicingStr(model.PART_NO);
            //    sQuery = GetDetailReportByPN(model.CONDITION, data, subline, model.WITHOUTRMAWO);
            //}

            //if (model.WO_NO != null && model.WO_NO.Count > 0)
            //{
            //    data = SplicingStr(model.WO_NO);
            //    sQuery = GetDetailReportByWO(model.CONDITION, data, subline);
            //}

            return (await _dbConnection.QueryAsync<SiteStatisticsReportDetailListModel>(sQuery, model)).ToList();
        }

        /// <summary>
        /// 根据all获取detail report
        /// </summary>
        /// <returns></returns>
        private string GetDetailReportByAll(int condition, string subline)
        {
            string sQuery = "";
            switch (condition)
            {
                case GlobalVariables.All:
                    sQuery = DetailGridViewAllDefault(subline);
                    break;
                case GlobalVariables.GroupByModel:
                    sQuery = DetailGridViewAllGroupByModel(subline);
                    break;
                case GlobalVariables.GroupByWo:
                    sQuery = DetailGridViewAllGroupByWO(subline);
                    break;
                case GlobalVariables.GroupByPN:
                    sQuery = DetailGridViewAllGroupByPartNO(subline);
                    break;
                case GlobalVariables.GroupByTime:
                    sQuery = DetailGridViewAllGroupByWorkTime(subline);
                    break;
                case GlobalVariables.GroupBySite:
                    sQuery = DetailGridViewAllGroupByOperationSite(subline);
                    break;
                case GlobalVariables.GroupByWoPN:
                    sQuery = DetailGridViewAllGroupByWoPN(subline);
                    break;
                case GlobalVariables.OrderByModel:
                    sQuery = DetailGridViewAllOrderByModel(subline);
                    break;
                case GlobalVariables.OrderByPN:
                    sQuery = DetailGridViewAllOrderByPN(subline);
                    break;
                case GlobalVariables.OrderByWo:
                    sQuery = DetailGridViewAllOrderByWO(subline);
                    break;
                case GlobalVariables.OrderBySite:
                    sQuery = DetailGridViewAllOrderBySite(subline);
                    break;
                case GlobalVariables.OrderByTime:
                    sQuery = DetailGridViewAllOrderByTime(subline);
                    break;
                default:
                    break;
            }
            return sQuery;
        }

        /// <summary>
        /// 根据model获取detail report
        /// </summary>
        /// <returns></returns>
        //private string GetDetailReportByModel(int condition, string model, string subline)
        //{
        //    string sQuery = "";
        //    switch (condition)
        //    {
        //        case GlobalVariables.All:
        //            sQuery = DetailGridViewModelDefault(model, subline);
        //            break;
        //        case GlobalVariables.GroupByModel:
        //            sQuery = DetailGridViewModelGroupByModel(model, subline);
        //            break;
        //        case GlobalVariables.GroupByWo:
        //            sQuery = DetailGridViewModelGroupByWO(model, subline);
        //            break;
        //        case GlobalVariables.GroupByPN:
        //            sQuery = DetailGridViewModelGroupByPartNO(model, subline);
        //            break;
        //        case GlobalVariables.GroupByTime:
        //            sQuery = DetailGridViewModelGroupByWorkTime(model, subline);
        //            break;
        //        case GlobalVariables.GroupBySite:
        //            sQuery = DetailGridViewModelGroupByOperationSite(model, subline);
        //            break;
        //        case GlobalVariables.GroupByWoPN:
        //            sQuery = DetailGridViewModelGroupByWoPN(model, subline);
        //            break;
        //        case GlobalVariables.OrderByModel:
        //            sQuery = DetailGridViewModelOrderByModel(model, subline);
        //            break;
        //        case GlobalVariables.OrderByPN:
        //            sQuery = DetailGridViewModelOrderByPN(model, subline);
        //            break;
        //        case GlobalVariables.OrderByWo:
        //            sQuery = DetailGridViewModelOrderByWO(model, subline);
        //            break;
        //        case GlobalVariables.OrderBySite:
        //            sQuery = DetailGridViewModelOrderBySite(model, subline);
        //            break;
        //        case GlobalVariables.OrderByTime:
        //            sQuery = DetailGridViewModelOrderByTime(model, subline);
        //            break;
        //        default:
        //            break;
        //    }
        //    return sQuery;
        //}

        /// <summary>
        /// 根据pn获取detail report
        /// </summary>
        /// <returns></returns>
        //private string GetDetailReportByPN(int condition, string part_no, string subline, bool withoutRMAWO)
        //{
        //    string sQuery = "";
        //    switch (condition)
        //    {
        //        case GlobalVariables.All:
        //            sQuery = DetailGridViewPartNODefault(part_no, subline, withoutRMAWO);
        //            break;
        //        case GlobalVariables.GroupByModel:
        //            sQuery = DetailGridViewPartNOGroupByModel(part_no, subline, withoutRMAWO);
        //            break;
        //        case GlobalVariables.GroupByWo:
        //            sQuery = DetailGridViewPartNOGroupByWO(part_no, subline, withoutRMAWO);
        //            break;
        //        case GlobalVariables.GroupByPN:
        //            sQuery = DetailGridViewPartNOGroupByPartNO(part_no, subline, withoutRMAWO);
        //            break;
        //        case GlobalVariables.GroupByTime:
        //            sQuery = DetailGridViewPartNOGroupByWorkTime(part_no, subline, withoutRMAWO);
        //            break;
        //        case GlobalVariables.GroupBySite:
        //            sQuery = DetailGridViewPartNOGroupByOperationSite(part_no, subline, withoutRMAWO);
        //            break;
        //        case GlobalVariables.GroupByWoPN:
        //            sQuery = DetailGridViewPartNOGroupByWoPN(part_no, subline, withoutRMAWO);
        //            break;
        //        case GlobalVariables.OrderByModel:
        //            sQuery = DetailGridViewPartNOOrderByModel(part_no, subline, withoutRMAWO);
        //            break;
        //        case GlobalVariables.OrderByPN:
        //            sQuery = DetailGridViewPartNOOrderByPN(part_no, subline, withoutRMAWO);
        //            break;
        //        case GlobalVariables.OrderByWo:
        //            sQuery = DetailGridViewPartNOOrderByWO(part_no, subline, withoutRMAWO);
        //            break;
        //        case GlobalVariables.OrderBySite:
        //            sQuery = DetailGridViewPartNOOrderBySite(part_no, subline, withoutRMAWO);
        //            break;
        //        case GlobalVariables.OrderByTime:
        //            sQuery = DetailGridViewPartNOOrderByTime(part_no, subline, withoutRMAWO);
        //            break;
        //        default:
        //            break;
        //    }
        //    return sQuery;
        //}

        /// <summary>
        /// 根据wo获取detail report
        /// </summary>
        /// <returns></returns>
        //private string GetDetailReportByWO(int condition, string wo_no, string subline)
        //{
        //    string sQuery = "";
        //    switch (condition)
        //    {
        //        case GlobalVariables.All:
        //            sQuery = DetailGridViewWODefault(wo_no, subline);
        //            break;
        //        case GlobalVariables.GroupByModel:
        //            sQuery = DetailGridViewWOGroupByModel(wo_no, subline);
        //            break;
        //        case GlobalVariables.GroupByWo:
        //            sQuery = DetailGridViewWOGroupByWO(wo_no, subline);
        //            break;
        //        case GlobalVariables.GroupByPN:
        //            sQuery = DetailGridViewWOGroupByPartNO(wo_no, subline);
        //            break;
        //        case GlobalVariables.GroupByTime:
        //            sQuery = DetailGridViewWOGroupByWorkTime(wo_no, subline);
        //            break;
        //        case GlobalVariables.GroupBySite:
        //            sQuery = DetailGridViewWOGroupByOperationSite(wo_no, subline);
        //            break;
        //        case GlobalVariables.GroupByWoPN:
        //            sQuery = DetailGridViewWOGroupByWoPN(wo_no, subline);
        //            break;
        //        case GlobalVariables.OrderByModel:
        //            sQuery = DetailGridViewWoOrderByModel(wo_no, subline);
        //            break;
        //        case GlobalVariables.OrderByPN:
        //            sQuery = DetailGridViewWoOrderByPN(wo_no, subline);
        //            break;
        //        case GlobalVariables.OrderByWo:
        //            sQuery = DetailGridViewWoOrderByWO(wo_no, subline);
        //            break;
        //        case GlobalVariables.OrderBySite:
        //            sQuery = DetailGridViewWoOrderBySite(wo_no, subline);
        //            break;
        //        case GlobalVariables.OrderByTime:
        //            sQuery = DetailGridViewWoOrderByTime(wo_no, subline);
        //            break;
        //        default:
        //            break;
        //    }
        //    return sQuery;
        //}

        #region detail Business Methods

        private static string CreateGroupBySQL(string subline)
        {
            string SQL = @"
           SUM (SSS.PASS) PASS,
           SUM (SSS.FAIL) FAIL,
           SUM (SSS.REPASS) REPASS,
           SUM (SSS.REFAIL) REFAIL,
           TRUNC (
              SUM (SSS.PASS)
              / DECODE ( (SUM (SSS.PASS) + SUM (SSS.FAIL)),
                        0, 1,
                        (SUM (SSS.PASS) + SUM (SSS.FAIL))),
              4 ) YIELD
           FROM  SFCS_WO SW, SFCS_MODEL SM, SFCS_OPERATIONS SO, 
           SFCS_OPERATION_SITES SOS, SFCS_SITE_STATISTICS SSS
           WHERE SSS.WO_ID = SW.ID
           AND SW.MODEL_ID = SM.ID
           AND SOS.OPERATION_ID = SO.ID
           AND SOS.ID = SSS.OPERATION_SITE_ID
           AND SOS.OPERATION_ID = SO.ID      
           AND SOS.OPERATION_ID = :OPERATION_ID
           AND SSS.WORK_TIME >= :BEGIN_TIME AND SSS.WORK_TIME <= :END_TIME ";

            if (!subline.IsNullOrEmpty())
            {
                SQL += subline;
            }

            return SQL;
        }

        #region By All

        public static string DetailGridViewAllDefault(string subline)
        {
            string SQL
                = @"SELECT   SM.MODEL, 
                            SW.WO_NO, 
                            SW.PART_NO, 
                            SOS.OPERATION_SITE_NAME,
                            SSS.WORK_TIME, 
                            SSS.PASS, 
                            SSS.FAIL, 
                            SSS.REPASS,  SSS.REFAIL,TRUNC (  SSS.PASS / DECODE ( (SSS.PASS + SSS.FAIL), 0, 1, (SSS.PASS + SSS.FAIL)),  4 ) YIELD 
                            FROM   SFCS_WO SW,  SFCS_MODEL SM, SFCS_OPERATIONS SO, SFCS_OPERATION_SITES SOS, SFCS_SITE_STATISTICS SSS  
                            WHERE    SSS.WO_ID = SW.ID  AND SW.MODEL_ID = SM.ID 
                              AND SOS.OPERATION_ID = SO.ID 
                              AND SOS.ID = SSS.OPERATION_SITE_ID  
                              AND SOS.OPERATION_ID = SO.ID AND SOS.OPERATION_ID = :OPERATION_ID
                              AND SSS.WORK_TIME >= :BEGIN_TIME AND SSS.WORK_TIME <= :END_TIME ";
            if (!subline.IsNullOrEmpty())
            {
                SQL += subline;            }

            return SQL;
        }

        public static string DetailGridViewAllGroupByModel(string subline)
        {
            string SQL = @"SELECT   SM.MODEL, " + CreateGroupBySQL(subline) +
                         @" GROUP BY   SM.MODEL ";
            return SQL;
        }

        public static string DetailGridViewAllGroupByWO(string subline)
        {
            string SQL = @"SELECT   SW.WO_NO, " + CreateGroupBySQL(subline) +
                         @" GROUP BY   SW.WO_NO ";
            return SQL;
        }

        public static string DetailGridViewAllGroupByPartNO(string subline)
        {
            string SQL = @"SELECT   SW.PART_NO, " + CreateGroupBySQL(subline) +
                         @" GROUP BY   SW.PART_NO ";
            return SQL;
        }

        public static string DetailGridViewAllGroupByWorkTime(string subline)
        {
            string SQL = @"SELECT   SSS.WORK_TIME, " + CreateGroupBySQL(subline) +
                         @" GROUP BY   SSS.WORK_TIME ORDER BY SSS.WORK_TIME ";
            return SQL;
        }

        public static string DetailGridViewAllGroupByOperationSite(string subline)
        {
            string SQL = @"SELECT  SOS.OPERATION_SITE_NAME, " + CreateGroupBySQL(subline) +
                         @" GROUP BY  SOS.OPERATION_SITE_NAME ";
            return SQL;
        }

        public static string DetailGridViewAllGroupByWoPN(string subline)
        {
            string SQL = @"SELECT  SW.WO_NO,SW.PART_NO," + CreateGroupBySQL(subline) +
                @" GROUP BY SW.WO_NO,SW.PART_NO ";
            return SQL;
        }

        public static string DetailGridViewAllOrderByModel(string subline)
        {
            string SQL = DetailGridViewAllDefault(subline) +
                @" ORDER BY SM.MODEL ";
            return SQL;
        }

        public static string DetailGridViewAllOrderByPN(string subline)
        {
            string SQL = DetailGridViewAllDefault(subline) +
               @" ORDER BY SW.PART_NO ";
            return SQL;
        }

        public static string DetailGridViewAllOrderByWO(string subline)
        {
            string SQL = DetailGridViewAllDefault(subline) +
                @" ORDER BY SW.WO_NO ";
            return SQL;
        }

        public static string DetailGridViewAllOrderBySite(string subline)
        {
            string SQL = DetailGridViewAllDefault(subline) +
               @" ORDER BY SOS.OPERATION_SITE_NAME ";
            return SQL;
        }

        public static string DetailGridViewAllOrderByTime(string subline)
        {
            string SQL = DetailGridViewAllDefault(subline) +
                @" ORDER BY SSS.WORK_TIME ";
            return SQL;
        }

        #endregion

        #region By Model

        //public static string DetailGridViewModelDefault(string Model, string subline)
        //{
        //    string SQL
        //         = "SELECT   SM.MODEL, " +
        //             "         SW.WO_NO, " +
        //             "         SW.PART_NO, " +
        //             "         SOS.OPERATION_SITE_NAME, " +
        //             "         SSS.WORK_TIME, " +
        //             "         SSS.PASS, " +
        //             "         SSS.FAIL, " +
        //             "         SSS.REPASS, " +
        //             "         SSS.REFAIL, " +
        //             "         TRUNC ( " +
        //             "            SSS.PASS " +
        //             "            / DECODE ( (SSS.PASS + SSS.FAIL), 0, 1, (SSS.PASS + SSS.FAIL)), " +
        //             "            4 " +
        //             "         ) " +
        //             "            YIELD " +
        //             "  FROM   SFCS_WO SW, " +
        //             "         SFCS_MODEL SM, " +
        //             "         SFCS_OPERATIONS SO, " +
        //             "         SFCS_OPERATION_SITES SOS, " +
        //             "         SFCS_SITE_STATISTICS SSS " +
        //             " WHERE       SW.ID = SSS.WO_ID " +
        //             "         AND SW.MODEL_ID = SM.ID " +
        //             "         AND SOS.OPERATION_ID = SO.ID " +
        //             "         AND SOS.ID = SSS.OPERATION_SITE_ID " +
        //             "         AND SOS.OPERATION_ID = SO.ID " +
        //             "         AND SO.OPERATION_NAME = :OPERATION_NAME " +
        //             "         AND SSS.WORK_TIME >= :BEGIN_TIME AND SSS.WORK_TIME <= :END_TIME " +
        //             (!string.IsNullOrEmpty(Model) ? " AND SM.MODEL IN (" + Model + ")  " : "");

        //    if (!subline.IsNullOrEmpty())
        //    {
        //        SQL += string.Format(@" AND SOS.OPERATION_LINE_ID IN ({0}) ", subline);
        //    }

        //    return SQL;
        //}

        //public static string DetailGridViewModelGroupByModel(string Model, string subline)
        //{
        //    string SQL = @"SELECT   SM.MODEL, " + CreateGroupBySQL(subline) +
        //                 (!string.IsNullOrEmpty(Model) ? "  AND SM.MODEL IN (" + Model + ")  " : "") +
        //                 @" GROUP BY   SM.MODEL ";
        //    return SQL;
        //}

        //public static string DetailGridViewModelGroupByWO(string Model, string subline)
        //{
        //    string SQL = @"SELECT   SW.WO_NO, " + CreateGroupBySQL(subline) +
        //                 (!string.IsNullOrEmpty(Model) ? " AND SM.MODEL IN (" + Model + ")  " : "") +
        //                 @" GROUP BY  SW.WO_NO ";
        //    return SQL;
        //}

        //public static string DetailGridViewModelGroupByPartNO(string Model, string subline)
        //{
        //    string SQL = @"SELECT   SW.PART_NO, " + CreateGroupBySQL(subline) +
        //                 (!string.IsNullOrEmpty(Model) ? " AND SM.MODEL IN (" + Model + ")  " : "") +
        //                 @" GROUP BY SW.PART_NO ";
        //    return SQL;
        //}

        //public static string DetailGridViewModelGroupByWorkTime(string Model, string subline)
        //{
        //    string SQL = @"SELECT   SSS.WORK_TIME, " + CreateGroupBySQL(subline) +
        //                 (!string.IsNullOrEmpty(Model) ? " AND SM.MODEL IN (" + Model + ")  " : "") +
        //                 @" GROUP BY SSS.WORK_TIME ORDER BY SSS.WORK_TIME ";
        //    return SQL;
        //}

        //public static string DetailGridViewModelGroupByOperationSite(string Model, string subline)
        //{
        //    string SQL = @"SELECT   SOS.OPERATION_SITE_NAME, " + CreateGroupBySQL(subline) +
        //                 (!string.IsNullOrEmpty(Model) ? " AND SM.MODEL IN (" + Model + ")  " : "") +
        //                 @" GROUP BY SOS.OPERATION_SITE_NAME ";
        //    return SQL;
        //}

        //public static string DetailGridViewModelGroupByWoPN(string Model, string subline)
        //{
        //    string SQL = @"SELECT SW.WO_NO,SW.PART_NO, " + CreateGroupBySQL(subline) +
        //        (!string.IsNullOrEmpty(Model) ? " AND SM.MODEL IN (" + Model + ")  " : "") +
        //        @" GROUP BY SW.WO_NO, SW.PART_NO ";
        //    return SQL;
        //}

        //public static string DetailGridViewModelOrderByModel(string Model, string subline)
        //{
        //    string SQL = DetailGridViewModelDefault(Model, subline) +
        //        @" ORDER BY SM.MODEL ";
        //    return SQL;
        //}

        //public static string DetailGridViewModelOrderByPN(string Model, string subline)
        //{
        //    string SQL = DetailGridViewModelDefault(Model, subline) +
        //       @" ORDER BY SW.PART_NO ";
        //    return SQL;
        //}

        //public static string DetailGridViewModelOrderByWO(string Model, string subline)
        //{
        //    string SQL = DetailGridViewModelDefault(Model, subline) +
        //        @" ORDER BY SW.WO_NO ";
        //    return SQL;
        //}

        //public static string DetailGridViewModelOrderBySite(string Model, string subline)
        //{
        //    string SQL = DetailGridViewModelDefault(Model, subline) +
        //       @" ORDER BY SOS.OPERATION_SITE_NAME ";
        //    return SQL;
        //}

        //public static string DetailGridViewModelOrderByTime(string Model, string subline)
        //{
        //    string SQL = DetailGridViewModelDefault(Model, subline) +
        //        @" ORDER BY SSS.WORK_TIME ";
        //    return SQL;
        //}

        #endregion

        #region By Part NO

        //public static string DetailGridViewPartNODefault(string PN, string subline, bool withoutRMAWO)
        //{
        //    string SQL
        //       = "SELECT   SM.MODEL, " +
        //           "         SW.WO_NO, " +
        //           "         SW.PART_NO, " +
        //           "         SOS.OPERATION_SITE_NAME, " +
        //           "         SSS.WORK_TIME, " +
        //           "         SSS.PASS, " +
        //           "         SSS.FAIL, " +
        //           "         SSS.REPASS, " +
        //           "         SSS.REFAIL, " +
        //           "         TRUNC ( " +
        //           "            SSS.PASS " +
        //           "            / DECODE ( (SSS.PASS + SSS.FAIL), 0, 1, (SSS.PASS + SSS.FAIL)), " +
        //           "            4 " +
        //           "         ) " +
        //           "            YIELD " +
        //           "  FROM   SFCS_WO SW, " +
        //           "         SFCS_MODEL SM, " +
        //           "         SFCS_OPERATIONS SO, " +
        //           "         SFCS_OPERATION_SITES SOS, " +
        //           "         SFCS_SITE_STATISTICS SSS " +
        //           " WHERE       SSS.WO_ID = SW.ID " +
        //           "         AND SW.MODEL_ID = SM.ID " +
        //           "         AND SOS.OPERATION_ID = SO.ID " +
        //           "         AND SOS.ID = SSS.OPERATION_SITE_ID " +
        //           "         AND SOS.OPERATION_ID = SO.ID " +
        //           "         AND SO.OPERATION_NAME = :OPERATION_NAME " +
        //           "         AND SSS.WORK_TIME >= :BEGIN_TIME AND SSS.WORK_TIME <= :END_TIME " +
        //           (!string.IsNullOrEmpty(PN) ? " AND SW.PART_NO IN (" + PN + ") " : "") +
        //           (withoutRMAWO ? S_SelectWithRMAWO : "");

        //    if (!subline.IsNullOrEmpty())
        //    {
        //        SQL += string.Format(@" AND SOS.OPERATION_LINE_ID IN ({0}) ", subline);
        //    }

        //    return SQL;
        //}

        //public static string DetailGridViewPartNOGroupByModel(string PN, string subline, bool whthoutRMAWO)
        //{
        //    string SQL = @"SELECT   SM.MODEL, " + CreateGroupBySQL(subline) +
        //                 (!string.IsNullOrEmpty(PN) ? " AND SW.PART_NO IN (" + PN + ") " : "") +
        //                 (whthoutRMAWO ? S_SelectWithRMAWO : "") +
        //                 @" GROUP BY  SM.MODEL ";
        //    return SQL;
        //}

        //public static string DetailGridViewPartNOGroupByWO(string PN, string subline, bool whthoutRMAWO)
        //{
        //    string SQL = @"SELECT   SW.WO_NO, " + CreateGroupBySQL(subline) +
        //                 (!string.IsNullOrEmpty(PN) ? " AND SW.PART_NO IN (" + PN + ") " : "") +
        //                 (whthoutRMAWO ? S_SelectWithRMAWO : "") +
        //                 @" GROUP BY  SW.WO_NO ";
        //    return SQL;
        //}

        //public static string DetailGridViewPartNOGroupByPartNO(string PN, string subline, bool whthoutRMAWO)
        //{
        //    string SQL = @"SELECT   SW.PART_NO, " + CreateGroupBySQL(subline) +
        //                 (!string.IsNullOrEmpty(PN) ? " AND SW.PART_NO IN (" + PN + ") " : "") +
        //                 (whthoutRMAWO ? S_SelectWithRMAWO : "") +
        //                 @" GROUP BY  SW.PART_NO ";
        //    return SQL;
        //}

        //public static string DetailGridViewPartNOGroupByWorkTime(string PN, string subline, bool whthoutRMAWO)
        //{
        //    string SQL = @"SELECT   SSS.WORK_TIME, " + CreateGroupBySQL(subline) +
        //                 (!string.IsNullOrEmpty(PN) ? " AND SW.PART_NO IN (" + PN + ") " : "") +
        //                 (whthoutRMAWO ? S_SelectWithRMAWO : "") +
        //                 @" GROUP BY  SSS.WORK_TIME ORDER BY SSS.WORK_TIME ";
        //    return SQL;
        //}

        //public static string DetailGridViewPartNOGroupByOperationSite(string PN, string subline, bool whthoutRMAWO)
        //{
        //    string SQL = @"SELECT   SOS.OPERATION_SITE_NAME, " + CreateGroupBySQL(subline) +
        //                 (!string.IsNullOrEmpty(PN) ? " AND SW.PART_NO IN (" + PN + ") " : "") +
        //                 (whthoutRMAWO ? S_SelectWithRMAWO : "") +
        //                 @" GROUP BY SOS.OPERATION_SITE_NAME ";
        //    return SQL;
        //}

        //public static string DetailGridViewPartNOGroupByWoPN(string PN, string subline, bool whthoutRMAWO)
        //{
        //    string SQL = @"SELECT SW.WO_NO, SW.PART_NO," + CreateGroupBySQL(subline) +
        //        (!string.IsNullOrEmpty(PN) ? " AND SW.PART_NO IN (" + PN + ") " : "") +
        //        (whthoutRMAWO ? S_SelectWithRMAWO : "") +
        //        @" GROUP BY SW.WO_NO, SW.PART_NO ";
        //    return SQL;
        //}

        //public static string DetailGridViewPartNOOrderByModel(string PN, string subline, bool withoutRMAWO)
        //{
        //    string SQL = DetailGridViewPartNODefault(PN, subline, withoutRMAWO) +
        //        @" ORDER BY SM.MODEL ";
        //    return SQL;
        //}

        //public static string DetailGridViewPartNOOrderByPN(string PN, string subline, bool withoutRMAWO)
        //{
        //    string SQL = DetailGridViewPartNODefault(PN, subline, withoutRMAWO) +
        //       @" ORDER BY SW.PART_NO ";
        //    return SQL;
        //}

        //public static string DetailGridViewPartNOOrderByWO(string PN, string subline, bool withoutRMAWO)
        //{
        //    string SQL = DetailGridViewPartNODefault(PN, subline, withoutRMAWO) +
        //        @" ORDER BY SW.WO_NO ";
        //    return SQL;
        //}

        //public static string DetailGridViewPartNOOrderBySite(string PN, string subline, bool withoutRMAWO)
        //{
        //    string SQL = DetailGridViewPartNODefault(PN, subline, withoutRMAWO) +
        //       @" ORDER BY SOS.OPERATION_SITE_NAME ";
        //    return SQL;
        //}

        //public static string DetailGridViewPartNOOrderByTime(string PN, string subline, bool withoutRMAWO)
        //{
        //    string SQL = DetailGridViewPartNODefault(PN, subline, withoutRMAWO) +
        //        @" ORDER BY SSS.WORK_TIME ";
        //    return SQL;
        //}

        #endregion

        #region By WO

        //public static string DetailGridViewWODefault(string WO, string subline)
        //{
        //    string SQL
        //        = "SELECT   SM.MODEL, " +
        //            "         SW.WO_NO, " +
        //            "         SW.PART_NO, " +
        //            "         SOS.OPERATION_SITE_NAME, " +
        //            "         SSS.WORK_TIME, " +
        //            "         SSS.PASS, " +
        //            "         SSS.FAIL, " +
        //            "         SSS.REPASS, " +
        //            "         SSS.REFAIL, " +
        //            "         TRUNC ( " +
        //            "            SSS.PASS " +
        //            "            / DECODE ( (SSS.PASS + SSS.FAIL), 0, 1, (SSS.PASS + SSS.FAIL)), " +
        //            "            4 " +
        //            "         ) " +
        //            "            YIELD " +
        //            "  FROM   SFCS_WO SW, " +
        //            "         SFCS_MODEL SM, " +
        //            "         SFCS_OPERATIONS SO, " +
        //            "         SFCS_OPERATION_SITES SOS, " +
        //            "         SFCS_SITE_STATISTICS SSS " +
        //            " WHERE       SSS.WO_ID = SW.ID " +
        //            "         AND SW.MODEL_ID = SM.ID " +
        //            "         AND SOS.OPERATION_ID = SO.ID " +
        //            "         AND SOS.ID = SSS.OPERATION_SITE_ID " +
        //            "         AND SOS.OPERATION_ID = SO.ID " +
        //            "         AND SO.DESCRIPTION = :OPERATION_NAME " +
        //            "         AND SSS.WORK_TIME >= :BEGIN_TIME AND SSS.WORK_TIME <= :END_TIME " +
        //            (!string.IsNullOrEmpty(WO) ? " AND SW.WO_NO IN (" + WO + ") " : "");

        //    if (!subline.IsNullOrEmpty())
        //    {
        //        SQL += string.Format(@" AND SOS.OPERATION_LINE_ID IN ({0}) ", subline);
        //    }

        //    return SQL;
        //}

        //public static string DetailGridViewWOGroupByModel(string WO, string subline)
        //{
        //    string SQL = @"SELECT   SM.MODEL, " + CreateGroupBySQL(subline) +
        //                 (!string.IsNullOrEmpty(WO) ? " AND SW.WO_NO IN (" + WO + ") " : "") +
        //                 @" GROUP BY  SM.MODEL ";
        //    return SQL;
        //}

        //public static string DetailGridViewWOGroupByWO(string WO, string subline)
        //{
        //    string SQL = @"SELECT   SW.WO_NO, " + CreateGroupBySQL(subline) +
        //                 (!string.IsNullOrEmpty(WO) ? " AND SW.WO_NO IN (" + WO + ") " : "") +
        //                 @" GROUP BY  SW.WO_NO ";
        //    return SQL;
        //}

        //public static string DetailGridViewWOGroupByPartNO(string WO, string subline)
        //{
        //    string SQL = @"SELECT   SW.PART_NO, " + CreateGroupBySQL(subline) +
        //                 (!string.IsNullOrEmpty(WO) ? " AND SW.WO_NO IN (" + WO + ") " : "") +
        //                 @" GROUP BY   SW.PART_NO ";
        //    return SQL;
        //}

        //public static string DetailGridViewWOGroupByWorkTime(string WO, string subline)
        //{
        //    string SQL = @"SELECT   SSS.WORK_TIME, " + CreateGroupBySQL(subline) +
        //                 (!string.IsNullOrEmpty(WO) ? " AND SW.WO_NO IN (" + WO + ") " : "") +
        //                 @" GROUP BY  SSS.WORK_TIME ORDER BY SSS.WORK_TIME ";
        //    return SQL;
        //}

        //public static string DetailGridViewWOGroupByOperationSite(string WO, string subline)
        //{
        //    string SQL = @"SELECT SOS.OPERATION_SITE_NAME, " + CreateGroupBySQL(subline) +
        //                 (!string.IsNullOrEmpty(WO) ? " AND SW.WO_NO IN (" + WO + ") " : "") +
        //                 @" GROUP BY  SOS.OPERATION_SITE_NAME ";
        //    return SQL;
        //}

        //public static string DetailGridViewWOGroupByWoPN(string WO, string subline)
        //{
        //    string SQL = @"SELECT SW.WO_NO,SW.PART_NO," + CreateGroupBySQL(subline) +
        //                 (!string.IsNullOrEmpty(WO) ? " AND SW.WO_NO IN (" + WO + ") " : "") +
        //                 @" GROUP BY SW.WO_NO,SW.PART_NO";
        //    return SQL;
        //}

        //public static string DetailGridViewWoOrderByModel(string WO, string subline)
        //{
        //    string SQL = DetailGridViewWODefault(WO, subline) +
        //        @" ORDER BY SM.MODEL ";
        //    return SQL;
        //}

        //public static string DetailGridViewWoOrderByPN(string WO, string subline)
        //{
        //    string SQL = DetailGridViewWODefault(WO, subline) +
        //       @" ORDER BY SW.PART_NO ";
        //    return SQL;
        //}

        //public static string DetailGridViewWoOrderByWO(string WO, string subline)
        //{
        //    string SQL = DetailGridViewWODefault(WO, subline) +
        //        @" ORDER BY SW.WO_NO ";
        //    return SQL;
        //}

        //public static string DetailGridViewWoOrderBySite(string WO, string subline)
        //{
        //    string SQL = DetailGridViewWODefault(WO, subline) +
        //       @" ORDER BY SOS.OPERATION_SITE_NAME ";
        //    return SQL;
        //}

        //public static string DetailGridViewWoOrderByTime(string WO, string subline)
        //{
        //    string SQL = DetailGridViewWODefault(WO, subline) +
        //        @" ORDER BY SSS.WORK_TIME ";
        //    return SQL;
        //}

        #endregion

        #endregion
        #endregion

        #region 在制品报表
        /// <summary>
        /// 获取在制品报表数据
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public async Task<WipStatisticsListModel> GetWipStatisticsList(StatisticalReportRequestModel model)
        {
            WipStatisticsListModel wModel = new WipStatisticsListModel();
            string data = string.Empty, sqlSearchType = string.Empty;
            string sQuery = string.Empty, sQueryProduct = string.Empty, sQueryDetails = string.Empty;
            string sqlLagTime = string.Empty;
            switch (model.LAGTIME)
            {
                case 0:
                    sqlLagTime = @" OPERATION_TIME < 24";
                    break;
                case 1:
                    sqlLagTime = @" OPERATION_TIME BETWEEN 24 AND 47.99";
                    break;
                case 2:
                    sqlLagTime = @" OPERATION_TIME BETWEEN 48 AND 71.99";
                    break;
                case 3:
                    sqlLagTime = @" OPERATION_TIME >= 72";
                    break;
                default:
                    break;
            }

            if (model.ALL)
            {
                sQuery = S_SelectWipStatisticsByAll;
            }
            if (model.MODEL != null && model.MODEL.Count > 0)
            {
                data = SplicingStr(model.MODEL);
                if (!model.ISLAGTIME)
                {
                    //数据表格
                    sQuery = string.Format(S_SelectWipStatisticsByModel, data);
                    //产品统计
                    sQueryProduct = string.Format(S_SelectWipStatisticsTableByProduct, " AND SM.MODEL IN(" + data + ") ");
                }
                else
                {
                    //数据表格
                    sqlSearchType = @" SM.MODEL IN (" + data + ") ";
                    sQuery = string.Format(S_SelectWipLagTimeStatistics, sqlSearchType, sqlLagTime);

                    //超时明细
                    sQueryDetails = string.Format(S_SelectChildLagTimeStatistics, sqlSearchType, sqlLagTime);
                }
            }
            if (model.PART_NO != null && model.PART_NO.Count > 0)
            {
                data = SplicingStr(model.PART_NO);
                if (!model.ISLAGTIME)
                {
                    //数据表格
                    sQuery = string.Format(S_SelectWipStatisticsByPN, data);
                    //产品统计
                    sQueryProduct = string.Format(S_SelectWipStatisticsTableByProduct, " AND SW.PART_NO IN (" + data + ") ");
                }
                else
                {
                    //数据表格
                    sqlSearchType = @" SW.PART_NO IN (" + data + ") ";
                    sQuery = string.Format(S_SelectWipLagTimeStatistics, sqlSearchType, sqlLagTime);

                    //超时明细
                    sQueryDetails = string.Format(S_SelectChildLagTimeStatistics, sqlSearchType, sqlLagTime);
                }
            }
            if (model.WO_NO != null && model.WO_NO.Count > 0)
            {
                data = SplicingStr(model.WO_NO);
                if (!model.ISLAGTIME)
                {
                    //数据表格
                    sQuery = string.Format(S_SelectWipStatisticsByWo, data);
                    //产品统计
                    sQueryProduct = string.Format(S_SelectWipStatisticsTableByProduct, " AND SW.WO_NO IN ( " + data + ") ");
                }
                else
                {
                    //数据表格
                    sqlSearchType = @" SW.WO_NO IN (" + data + ") ";
                    sQuery = string.Format(S_SelectWipLagTimeStatistics, sqlSearchType, sqlLagTime);

                    //超时明细
                    sQueryDetails = string.Format(S_SelectChildLagTimeStatistics, sqlSearchType, sqlLagTime);
                }
            }
            if (model.LINE_ID != null && model.LINE_ID.Count > 0)
            {
                data = SplicingStr(model.LINE_ID);
                //数据表格
                sQuery = string.Format(S_SelectWipStatisticsByOperationLine, data);
            }
            wModel.DATATABLE = sQuery.IsNullOrEmpty() ? null : (await _dbConnection.QueryAsync<WipStatisticsDataTableListModel>(sQuery, model)).ToList();//数据表格
            if (model.ISPRODUCT && !string.IsNullOrEmpty(sQueryProduct))
            {
                wModel.DATAPRODUCT = (await _dbConnection.QueryAsync<WipStatisticsProductListModel>(sQueryProduct, model)).ToList();//数据表格
            }
            if (model.ISLAGTIME && !string.IsNullOrEmpty(sQueryDetails))
            {
                wModel.DATADETAILS = (await _dbConnection.QueryAsync<WipStatisticsDetailListModel>(sQueryDetails, model)).ToList();//超时明细数据
            }
            return wModel;
        }

        #region wip statistics sql

        public const string S_SelectWipStatisticsByAll = @"  SELECT A.ROUTE_CODE OPERATION_ID, A.OPERATION_NAME, NVL (QTY, 0) QTY
    FROM   (  SELECT   DISTINCT
                       SO.DESCRIPTION AS OPERATION_NAME,
                       SO.ID ROUTE_CODE,
                       MAX (SRC.ORDER_NO) ORDER_NO
                FROM   SFCS_WO SW, SFCS_OPERATIONS SO, SFCS_ROUTE_CONFIG SRC, SFCS_MODEL SM
               WHERE   SW.MODEL_ID = SM.ID
                       AND SW.ROUTE_ID = SRC.ROUTE_ID
                       AND (SRC.CURRENT_OPERATION_ID = SO.ID
                            OR SRC.REPAIR_OPERATION_ID = SO.ID OR SO.ID IN (990,991,999))
                       AND SO.ENABLED = 'Y'
            GROUP BY   SO.DESCRIPTION, SO.ID) A,
           (  SELECT   WIP_OPERATION, COUNT ( * ) QTY
                FROM   SFCS_RUNCARD SR, SFCS_OPERATIONS SO, SFCS_WO SW, SFCS_MODEL SM
               WHERE   SR.WO_ID = SW.ID AND SW.MODEL_ID = SM.ID 
                       AND SR.WIP_OPERATION = SO.ID
                       AND SR.INPUT_TIME >=:BEGIN_TIME AND SR.INPUT_TIME <=:END_TIME
            GROUP BY   WIP_OPERATION) B
   WHERE   A.ROUTE_CODE = B.WIP_OPERATION(+) 
   ORDER BY   ORDER_NO, ROUTE_CODE  ";

        public const string S_SelectWipStatisticsByWo = @" SELECT A.ROUTE_CODE OPERATION_ID,  A.OPERATION_NAME, NVL (QTY, 0) QTY
    FROM  (  SELECT   DISTINCT
                       SO.DESCRIPTION AS OPERATION_NAME,
                       SO.ID ROUTE_CODE,
                       MAX (SRC.ORDER_NO) ORDER_NO
                FROM   SFCS_WO SW, SFCS_OPERATIONS SO, SFCS_ROUTE_CONFIG SRC, SFCS_MODEL SM
               WHERE   SW.MODEL_ID = SM.ID
                       AND SW.WO_NO IN ({0})
                       AND SW.ROUTE_ID = SRC.ROUTE_ID
                       AND (SRC.CURRENT_OPERATION_ID = SO.ID
                           OR SRC.REPAIR_OPERATION_ID = SO.ID OR SO.ID IN (990,991,999))
                       AND SO.ENABLED = 'Y'
            GROUP BY   SO.DESCRIPTION, SO.ID) A,
           (  SELECT   WIP_OPERATION, COUNT ( * ) QTY
                FROM   SFCS_RUNCARD SR, SFCS_OPERATIONS SO, SFCS_WO SW, SFCS_MODEL SM
               WHERE   SR.WO_ID = SW.ID AND SW.MODEL_ID = SM.ID 
                       AND SR.WIP_OPERATION = SO.ID
                       AND SW.WO_NO IN ({0})
                       AND SR.INPUT_TIME >=:BEGIN_TIME AND SR.INPUT_TIME <=:END_TIME
            GROUP BY   WIP_OPERATION) B
   WHERE  A.ROUTE_CODE = B.WIP_OPERATION(+) 
    ORDER BY   ORDER_NO, ROUTE_CODE ";

        public const string S_SelectWipStatisticsByModel = @"SELECT A.ROUTE_CODE OPERATION_ID,  A.OPERATION_NAME, NVL (QTY, 0) QTY
      FROM   (  SELECT   DISTINCT
                       SO.DESCRIPTION AS OPERATION_NAME,
                       SO.ID ROUTE_CODE,
                       MAX (SRC.ORDER_NO) ORDER_NO
                FROM   SFCS_WO SW, SFCS_OPERATIONS SO, SFCS_ROUTE_CONFIG SRC, SFCS_MODEL SM
               WHERE   SW.MODEL_ID = SM.ID
                       AND SM.MODEL IN ({0})
                       AND SW.ROUTE_ID = SRC.ROUTE_ID
                       AND (SRC.CURRENT_OPERATION_ID = SO.ID
                           OR SRC.REPAIR_OPERATION_ID = SO.ID OR SO.ID IN (990,991,999))
                       AND SO.ENABLED = 'Y'
            GROUP BY   SO.DESCRIPTION, SO.ID) A,
           (  SELECT   WIP_OPERATION, COUNT ( * ) QTY
                FROM   SFCS_RUNCARD SR, SFCS_OPERATIONS SO, SFCS_WO SW, SFCS_MODEL SM
               WHERE   SR.WO_ID = SW.ID AND SW.MODEL_ID = SM.ID 
                       AND SR.WIP_OPERATION = SO.ID
                       AND SM.MODEL IN ({0})
                       AND SR.INPUT_TIME >=:BEGIN_TIME AND SR.INPUT_TIME <=:END_TIME
            GROUP BY   WIP_OPERATION) B
   WHERE   A.ROUTE_CODE = B.WIP_OPERATION(+) 
     ORDER BY   ORDER_NO, ROUTE_CODE ";

        public const string S_SelectWipStatisticsByPN = @"SELECT A.ROUTE_CODE OPERATION_ID,  A.OPERATION_NAME, NVL (QTY, 0) QTY
    FROM   (  SELECT   DISTINCT
                       SO.DESCRIPTION AS OPERATION_NAME,
                       SO.ID ROUTE_CODE,
                       MAX (SRC.ORDER_NO) ORDER_NO
                FROM   SFCS_WO SW, SFCS_OPERATIONS SO, SFCS_ROUTE_CONFIG SRC, SFCS_MODEL SM
               WHERE   SW.MODEL_ID = SM.ID
                       AND SW.PART_NO IN ({0})
                       AND SW.ROUTE_ID = SRC.ROUTE_ID
                       AND (SRC.CURRENT_OPERATION_ID = SO.ID
                           OR SRC.REPAIR_OPERATION_ID = SO.ID OR SO.ID IN (990,991,999))
                       AND SO.ENABLED = 'Y'
            GROUP BY   SO.DESCRIPTION, SO.ID) A,
           (  SELECT   WIP_OPERATION, COUNT ( * ) QTY
                FROM   SFCS_RUNCARD SR, SFCS_OPERATIONS SO, SFCS_WO SW, SFCS_MODEL SM
               WHERE   SR.WO_ID = SW.ID AND SW.MODEL_ID = SM.ID 
                       AND SR.WIP_OPERATION = SO.ID
                       AND SW.PART_NO IN ({0})
                       AND SR.INPUT_TIME >=:BEGIN_TIME AND SR.INPUT_TIME <=:END_TIME
            GROUP BY   WIP_OPERATION) B
    WHERE   A.ROUTE_CODE = B.WIP_OPERATION(+) 
       ORDER BY   ORDER_NO, ROUTE_CODE ";

        public const string S_SelectWipStatisticsByOperationLine = @"SELECT A.ROUTE_CODE OPERATION_ID, A.OPERATION_NAME, NVL (QTY, 0) QTY
    FROM   (  SELECT   DISTINCT
                       SO.DESCRIPTION AS OPERATION_NAME,
                       SO.ID ROUTE_CODE,
                       MAX (SRC.ORDER_NO) ORDER_NO
                FROM   SFCS_WO SW, SFCS_OPERATIONS SO, SFCS_ROUTE_CONFIG SRC, SFCS_MODEL SM
               WHERE   SW.MODEL_ID = SM.ID
                       AND SW.ROUTE_ID = SRC.ROUTE_ID
                       AND (SRC.CURRENT_OPERATION_ID = SO.ID
                            OR SRC.REPAIR_OPERATION_ID = SO.ID OR SO.ID IN (990,991,999))
                       AND SO.ENABLED = 'Y'
            GROUP BY   SO.DESCRIPTION, SO.ID) A,
           (  SELECT   WIP_OPERATION, COUNT ( * ) QTY
                FROM   SFCS_RUNCARD SR, SFCS_OPERATIONS SO, SFCS_WO SW, SFCS_MODEL SM,SFCS_OPERATION_SITES SOS,SFCS_OPERATION_LINES SOL
               WHERE   SR.WO_ID = SW.ID AND SW.MODEL_ID = SM.ID 
                       AND SR.WIP_OPERATION = SO.ID
                       AND SR.CURRENT_SITE = SOS.ID
                       AND SOS.OPERATION_LINE_ID = SOL.ID
                       AND SOL.ID in ({0})
                       AND SR.INPUT_TIME >=:BEGIN_TIME AND SR.INPUT_TIME <=:END_TIME
            GROUP BY   WIP_OPERATION) B
   WHERE   A.ROUTE_CODE = B.WIP_OPERATION(+) 
   ORDER BY   ORDER_NO, ROUTE_CODE  ";

        #endregion

        #region lagTime sql

        public const string S_SelectWipLagTimeStatistics = @" SELECT A.ROUTE_CODE OPERATION_ID,  A.OPERATION_NAME, NVL (QTY, 0) QTY
                                                FROM   (  SELECT   DISTINCT
                                                                   SO.OPERATION_NAME,
                                                                   SO.ID ROUTE_CODE,
                                                                   MAX (SRC.ORDER_NO) ORDER_NO
                                                            FROM   SFCS_WO SW, SFCS_OPERATIONS SO, SFCS_ROUTE_CONFIG SRC, SFCS_MODEL SM
                                                           WHERE   SW.MODEL_ID = SM.ID
                                                                   AND {0}
                                                                   AND SW.ROUTE_ID = SRC.ROUTE_ID
                                                                   AND (SRC.CURRENT_OPERATION_ID = SO.ID
                                                                       OR SRC.REPAIR_OPERATION_ID = SO.ID OR SO.ID IN (990,991,999))
                                                                   AND SO.ENABLED = 'Y'
                                                        GROUP BY   SO.OPERATION_NAME, SO.ID) A,
                                                (SELECT WIP_OPERATION,COUNT(*) QTY FROM (
                                                SELECT SR.SN, SR.WIP_OPERATION, ROUND((SYSDATE - MAX(SOH.OPERATION_TIME))*24, 2) OPERATION_TIME
                                                FROM SFCS_RUNCARD SR, SFCS_WO SW, SFCS_MODEL SM, SFCS_OPERATION_HISTORY SOH,
                                                    SFCS_OPERATIONS SO
                                                    WHERE SR.WO_ID = SW.ID AND SW.MODEL_ID = SM.ID
                                                    AND SR.ID = SOH.SN_ID
                                                    AND SO.ID = SR.WIP_OPERATION
                                                    AND SR.STATUS NOT IN (4, 5, 7)
                                                    AND {0}
                                                    AND SR.INPUT_TIME >=:BEGIN_TIME AND SR.INPUT_TIME <=:END_TIME
                                            GROUP BY SR.SN, SR.WIP_OPERATION)
                                                    WHERE {1}
                                                    GROUP BY WIP_OPERATION) B
                                            WHERE   A.ROUTE_CODE = B.WIP_OPERATION(+) 
                                                ORDER BY   ORDER_NO, ROUTE_CODE";

        public const string S_SelectChildLagTimeStatistics = @"SELECT * FROM (
                                                SELECT SR.SN, SW.WO_NO, SW.PART_NO, SO.OPERATION_NAME, SM.MODEL, SR.CARTON_NO, SR.PALLET_NO,
                                                    ROUND((SYSDATE - MAX(SOH.OPERATION_TIME))*24, 2) OPERATION_TIME, SP.DESCRIPTION STATUS
                                                FROM SFCS_RUNCARD SR, SFCS_WO SW, SFCS_MODEL SM, SFCS_OPERATION_HISTORY SOH,
                                                    SFCS_OPERATIONS SO, SFCS_PARAMETERS SP
                                                    WHERE SR.WO_ID = SW.ID AND SW.MODEL_ID = SM.ID
                                                    AND SR.ID = SOH.SN_ID
                                                    AND SP.LOOKUP_TYPE = 'RUNCARD_STATUS'
                                                    AND SR.STATUS = SP.LOOKUP_CODE
                                                    AND SO.ID = SR.WIP_OPERATION
                                                    AND SR.STATUS NOT IN (4, 5, 7)
                                                    AND {0}
                                                    AND SR.INPUT_TIME >=:BEGIN_TIME AND SR.INPUT_TIME <=:END_TIME
                                            GROUP BY SR.SN, SW.WO_NO, SW.PART_NO, SO.OPERATION_NAME, SM.MODEL, SR.CARTON_NO, SR.PALLET_NO, SP.DESCRIPTION)
                                                WHERE {1}
                                            ORDER BY SN, WO_NO, OPERATION_NAME";

        #endregion

        #region dataByProduct sql 

        public const string S_SelectWipStatisticsTableByProduct = @"SELECT MODEL , PART_NO, OPERATION_NAME, NVL (QTY, 0) QTY
    FROM  (  SELECT DISTINCT
                       SO.DESCRIPTION AS OPERATION_NAME,
                       SO.ID ROUTE_CODE ,SM.MODEL, SW.PART_NO, COUNT(*) QTY
                FROM   SFCS_WO SW, SFCS_OPERATIONS SO, SFCS_MODEL SM, SFCS_RUNCARD SR
               WHERE   SW.MODEL_ID = SM.ID
                       AND SR.WO_ID = SW.ID
                        {0}
                       AND SR.WIP_OPERATION = SO.ID
                       AND SO.ENABLED = 'Y'
                       AND SR.INPUT_TIME >=:BEGIN_TIME AND SR.INPUT_TIME <=:END_TIME
            GROUP BY   SM.MODEL, SW.PART_NO, SO.DESCRIPTION, SO.ID)
    ORDER BY  PART_NO, MODEL";

        #endregion

        #region 在制品报表获取detail report
        /// <summary>
        /// 在制品报表获取detail report
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public async Task<List<WipStatisticsReportDetailListModel>> GetWipStatisticsReportDetail(ReportDetailRequestModel model)
        {
            string sQuery = "", data = "";

            if (model.ALL)
            {
                sQuery = GetChildWipStatisticsByAll(model.CONDITION);
            }

            if (model.MODEL != null && model.MODEL.Count > 0)
            {
                data = SplicingStr(model.MODEL);
                if (model.ISLAGTIME)
                {
                    sQuery = GetChildLagTimeWipStatistics(model.LAGTIME, data, GlobalVariables.MODEL);
                }
                else
                {
                    sQuery = GetChildWipStatisticsByModel(model.CONDITION, data);
                }
            }

            if (model.PART_NO != null && model.PART_NO.Count > 0)
            {
                data = SplicingStr(model.PART_NO);
                if (model.ISLAGTIME)
                {
                    sQuery = GetChildLagTimeWipStatistics(model.LAGTIME, data, GlobalVariables.PART_NO);
                }
                else
                {
                    sQuery = GetChildWipStatisticsByPN(model.CONDITION, data);
                }
            }

            if (model.WO_NO != null && model.WO_NO.Count > 0)
            {
                data = SplicingStr(model.WO_NO);
                if (model.ISLAGTIME)
                {
                    sQuery = GetChildLagTimeWipStatistics(model.LAGTIME, data, GlobalVariables.WO_NO);
                }
                else
                {
                    sQuery = GetChildWipStatisticsByWo(model.CONDITION, data);
                }
            }

            if (model.LINE_ID != null && model.LINE_ID.Count > 0)
            {
                data = SplicingStr(model.LINE_ID);
                sQuery = GetChildWipStatisticsByLine(model.CONDITION, data);
            }

            return (await _dbConnection.QueryAsync<WipStatisticsReportDetailListModel>(sQuery, model)).ToList();
        }

        /// <summary>
        /// 获取详细子报表
        /// </summary>
        /// <param name="condition"></param>
        /// <returns></returns>
        private string GetChildWipStatisticsByAll(int condition)
        {
            string sQuery = "";
            switch (condition)
            {
                case GlobalVariables.All:
                    sQuery = DetailGridViewAllDefault();
                    break;
                case GlobalVariables.GroupByModel:
                    sQuery = DetailGridViewAllGroupByModel();
                    break;
                case GlobalVariables.GroupByPN:
                    sQuery = DetailGridViewAllGroupByPN();
                    break;
                case GlobalVariables.GroupByWo:
                    sQuery = DetailGridViewAllGroupByWo();
                    break;
                case GlobalVariables.GroupByCarton:
                    sQuery = DetailGridViewAllGroupByCarton();
                    break;
                case GlobalVariables.GroupByPallet:
                    sQuery = DetailGridViewAllGroupByPallet();
                    break;
                default:
                    break;
            }
            return sQuery;
        }

        /// <summary>
        /// 获取超时详细子报表
        /// </summary>
        /// <param name="lagtime"></param>
        /// <param name="data"></param>
        /// <param name="searchType"></param>
        /// <returns></returns>
        private string GetChildLagTimeWipStatistics(int lagtime, string data, string searchType)
        {
            string sQuery = "";
            switch (lagtime)
            {
                case 0:
                    sQuery = GetChildLagTimeTable(searchType, data, 0);
                    break;
                case 1:
                    sQuery = GetChildLagTimeTable(searchType, data, 24);
                    break;
                case 2:
                    sQuery = GetChildLagTimeTable(searchType, data, 48);
                    break;
                case 3:
                    sQuery = GetChildLagTimeTable(searchType, data, 72);
                    break;
                default:
                    break;
            }
            return sQuery;
        }

        /// <summary>
        /// 获取超时明细
        /// </summary>
        /// <param name="searchType"></param>
        /// <param name="data"></param>
        /// <param name="lagTime"></param>
        /// <param name="beginTime"></param>
        /// <param name="endTime"></param>
        /// <returns></returns>
        public static string GetChildLagTimeTable(string searchType, string data, decimal lagTime)
        {
            string sqlSearchType = string.Empty;
            string sqlLagTime = string.Empty;

            if (searchType == GlobalVariables.WO_NO)
            {
                sqlSearchType = @" SW.WO_NO IN (" + data + ") AND SO.ID = :OPERATION_ID";
            }
            else if (searchType == GlobalVariables.PART_NO)
            {
                sqlSearchType = @" SW.PART_NO IN (" + data + ") AND SO.ID = :OPERATION_ID";
            }
            else if (searchType == GlobalVariables.MODEL)
            {
                sqlSearchType = @" SM.MODEL IN (" + data + ") AND SO.ID = :OPERATION_ID";
            }

            if (lagTime == 24)
            {
                sqlLagTime = @" OPERATION_TIME BETWEEN 24 AND 47.99 ";
            }
            else if (lagTime == 48)
            {
                sqlLagTime = @" OPERATION_TIME BETWEEN 48 AND 71.99 ";
            }
            else if (lagTime == 72)
            {
                sqlLagTime = @" OPERATION_TIME >= 72 ";
            }
            else if (lagTime == 0)
            {
                sqlLagTime = @" OPERATION_TIME < 24 ";
            }

            string sql = string.Format(S_SelectChildLagTimeStatistics, sqlSearchType, sqlLagTime);
            return sql;
        }

        /// <summary>
        /// 根据机种获取详细子报表
        /// </summary>
        /// <param name="condition"></param>
        /// <param name="model"></param>
        /// <returns></returns>
        private string GetChildWipStatisticsByModel(int condition, string model)
        {
            string sQuery = "";
            switch (condition)
            {
                case GlobalVariables.All:
                    sQuery = DetailGridViewModelDefault(model);
                    break;
                case GlobalVariables.GroupByModel:
                    sQuery = DetailGridViewModelGroupByModel(model);
                    break;
                case GlobalVariables.GroupByPN:
                    sQuery = DetailGridViewModelGroupByPartNO(model);
                    break;
                case GlobalVariables.GroupByWo:
                    sQuery = DetailGridViewModelGroupByCarton(model);
                    break;
                case GlobalVariables.GroupByCarton:
                    sQuery = DetailGridViewModelGroupByCarton(model);
                    break;
                case GlobalVariables.GroupByPallet:
                    sQuery = DetailGridViewModelGroupByPallet(model);
                    break;
                default:
                    break;
            }
            return sQuery;
        }

        /// <summary>
        /// 根据料号获取详细子报表
        /// </summary>
        /// <param name="condition"></param>
        /// <param name="pn"></param>
        /// <returns></returns>
        private string GetChildWipStatisticsByPN(int condition, string pn)
        {
            string sQuery = "";
            switch (condition)
            {
                case GlobalVariables.All:
                    sQuery = DetailGridViewPartNODefault(pn);
                    break;
                case GlobalVariables.GroupByModel:
                    sQuery = DetailGridViewPartNOGroupByModel(pn);
                    break;
                case GlobalVariables.GroupByPN:
                    sQuery = DetailGridViewPartNOGroupByPartNo(pn);
                    break;
                case GlobalVariables.GroupByWo:
                    sQuery = DetailGridViewPartNOGroupByWO(pn);
                    break;
                case GlobalVariables.GroupByCarton:
                    sQuery = DetailGridViewPartNOGroupByCarton(pn);
                    break;
                case GlobalVariables.GroupByPallet:
                    sQuery = DetailGridViewPartNOGroupByPallet(pn);
                    break;
                default:
                    break;
            }
            return sQuery;
        }

        /// <summary>
        /// 根据工单获取详细子报表
        /// </summary>
        /// <param name="condition"></param>
        /// <param name="wo"></param>
        /// <returns></returns>
        private string GetChildWipStatisticsByWo(int condition, string wo)
        {
            string sQuery = "";
            switch (condition)
            {
                case GlobalVariables.All:
                    sQuery = DetailGridViewWODefault(wo);
                    break;
                case GlobalVariables.GroupByModel:
                    sQuery = DetailGridViewWOGroupByModel(wo);
                    break;
                case GlobalVariables.GroupByPN:
                    sQuery = DetailGridViewWOGroupByPartNO(wo);
                    break;
                case GlobalVariables.GroupByWo:
                    sQuery = DetailGridViewWOGroupByWO(wo);
                    break;
                case GlobalVariables.GroupByCarton:
                    sQuery = DetailGridViewWOGroupByCarton(wo);
                    break;
                case GlobalVariables.GroupByPallet:
                    sQuery = DetailGridViewWOGroupByPallet(wo);
                    break;
                default:
                    break;
            }
            return sQuery;
        }

        /// <summary>
        /// 根据线体获取详细子报表
        /// </summary>
        /// <param name="condition"></param>
        /// <param name="line"></param>
        /// <returns></returns>
        private string GetChildWipStatisticsByLine(int condition, string line)
        {
            string sQuery = "";
            switch (condition)
            {
                case GlobalVariables.All:
                    sQuery = DetailGridViewLineDefault(line);
                    break;
                case GlobalVariables.GroupByModel:
                    sQuery = DetailGridViewLineGroupByModel(line);
                    break;
                case GlobalVariables.GroupByPN:
                    sQuery = DetailGridViewLineGroupByPartNO(line);
                    break;
                case GlobalVariables.GroupByWo:
                    sQuery = DetailGridViewLineGroupByWO(line);
                    break;
                case GlobalVariables.GroupByCarton:
                    sQuery = DetailGridViewLineGroupByCarton(line);
                    break;
                case GlobalVariables.GroupByPallet:
                    sQuery = DetailGridViewLineGroupByPallet(line);
                    break;
                default:
                    break;
            }
            return sQuery;
        }

        #region child detail sql

        private static string CreateGroupBySQL()
        {
            string sql = @" COUNT(SR.SN) QTY
  FROM   SFCS_RUNCARD SR, SFCS_OPERATIONS SO, SFCS_WO SW,
         SFCS_MODEL SM, SFCS_PARAMETERS SP
 WHERE   SR.WO_ID = SW.ID
         AND SW.MODEL_ID = SM.ID
         AND SR.WIP_OPERATION = SO.ID
         AND SR.STATUS = SP.LOOKUP_CODE
         AND SP.LOOKUP_TYPE = 'RUNCARD_STATUS'
         AND SO.ID = :OPERATION_ID
         AND SR.INPUT_TIME >=:BEGIN_TIME AND SR.INPUT_TIME <=:END_TIME";
            return sql;
        }

        private static string CreateLineGroupBySQL()
        {
            string sql = @" COUNT(SR.SN) QTY
  FROM   SFCS_RUNCARD SR, SFCS_OPERATIONS SO, SFCS_WO SW,
         SFCS_MODEL SM, SFCS_PARAMETERS SP,SFCS_OPERATION_SITES SOS,SFCS_OPERATION_LINES SOL
 WHERE   SR.WO_ID = SW.ID
         AND SW.MODEL_ID = SM.ID
         AND SR.WIP_OPERATION = SO.ID
         AND SR.CURRENT_SITE = SOS.ID
         AND SOS.OPERATION_LINE_ID = SOL.ID
         AND SR.STATUS = SP.LOOKUP_CODE
         AND SP.LOOKUP_TYPE = 'RUNCARD_STATUS'
         AND SO.ID = :OPERATION_ID
         AND SR.INPUT_TIME >=:BEGIN_TIME AND SR.INPUT_TIME <=:END_TIME ";
            return sql;
        }

        #region by all

        public static string DetailGridViewAllDefault()
        {
            string sql = @" 
 SELECT  SR.SN,SP.DESCRIPTION STATUS, SO.OPERATION_NAME, SR.CARTON_NO,
         SR.PALLET_NO ,SW.WO_NO, SW.PART_NO, SM.MODEL, SR.OPERATION_TIME WORK_TIME
  FROM   SFCS_RUNCARD SR, SFCS_OPERATIONS SO, SFCS_WO SW,
         SFCS_MODEL SM, SFCS_PARAMETERS SP
 WHERE   SR.WO_ID = SW.ID
         AND SW.MODEL_ID = SM.ID
         AND SR.WIP_OPERATION = SO.ID
         AND SR.STATUS = SP.LOOKUP_CODE
         AND SP.LOOKUP_TYPE = 'RUNCARD_STATUS'
         AND SO.ID = :OPERATION_ID
         AND SR.INPUT_TIME >=:BEGIN_TIME AND SR.INPUT_TIME <=:END_TIME ";
            return sql;
        }

        public static string DetailGridViewAllGroupByModel()
        {
            string SQL = @"SELECT  SM.MODEL, " + CreateGroupBySQL() +
                         @" GROUP BY  SM.MODEL ";
            return SQL;
        }

        public static string DetailGridViewAllGroupByPN()
        {
            string SQL = @"SELECT  SW.PART_NO, " + CreateGroupBySQL() +
                         @" GROUP BY  SW.PART_NO ";
            return SQL;
        }

        public static string DetailGridViewAllGroupByWo()
        {
            string SQL = @"SELECT  SW.WO_NO, " + CreateGroupBySQL() +
                         @" GROUP BY  SW.WO_NO ";
            return SQL;
        }

        public static string DetailGridViewAllGroupByCarton()
        {
            string SQL = @"SELECT  SR.CARTON_NO, " + CreateGroupBySQL() +
                         @" GROUP BY  SR.CARTON_NO ";
            return SQL;
        }

        public static string DetailGridViewAllGroupByPallet()
        {
            string SQL = @"SELECT  SR.PALLET_NO, " + CreateGroupBySQL() +
                         @" GROUP BY  SR.PALLET_NO ";
            return SQL;
        }

        #endregion

        #region by model 

        public static string DetailGridViewModelDefault(string Model)
        {
            string sql = @" 
 SELECT  SR.SN,SP.DESCRIPTION STATUS, SO.OPERATION_NAME, SR.CARTON_NO,
         SR.PALLET_NO ,SW.WO_NO, SW.PART_NO, SM.MODEL, SR.OPERATION_TIME WORK_TIME
  FROM   SFCS_RUNCARD SR, SFCS_OPERATIONS SO, SFCS_WO SW,
         SFCS_MODEL SM, SFCS_PARAMETERS SP
 WHERE   SR.WO_ID = SW.ID
         AND SM.MODEL IN ({0})
         AND SW.MODEL_ID = SM.ID
         AND SR.WIP_OPERATION = SO.ID
         AND SR.STATUS = SP.LOOKUP_CODE
         AND SP.LOOKUP_TYPE = 'RUNCARD_STATUS'
         AND SO.ID = :OPERATION_ID
         AND SR.INPUT_TIME >=:BEGIN_TIME AND SR.INPUT_TIME <=:END_TIME ";
            return string.Format(sql, Model);
        }

        public static string DetailGridViewModelGroupByModel(string Model)
        {
            string SQL = @"SELECT   SM.MODEL, " + CreateGroupBySQL() +
                         (!string.IsNullOrEmpty(Model) ? "  AND SM.MODEL IN (" + Model + ")  " : "") +
                         @" GROUP BY   SM.MODEL ";
            return SQL;
        }

        public static string DetailGridViewModelGroupByWO(string Model)
        {
            string SQL = @"SELECT   SW.WO_NO, " + CreateGroupBySQL() +
                         (!string.IsNullOrEmpty(Model) ? " AND SM.MODEL IN (" + Model + ")  " : "") +
                         @" GROUP BY  SW.WO_NO ";
            return SQL;
        }

        public static string DetailGridViewModelGroupByPartNO(string Model)
        {
            string SQL = @"SELECT   SW.PART_NO, " + CreateGroupBySQL() +
                         (!string.IsNullOrEmpty(Model) ? " AND SM.MODEL IN (" + Model + ")  " : "") +
                         @" GROUP BY SW.PART_NO ";
            return SQL;
        }

        public static string DetailGridViewModelGroupByCarton(string Model)
        {
            string SQL = @"SELECT  SR.CARTON_NO, " + CreateGroupBySQL() +
                (!string.IsNullOrEmpty(Model) ? " AND SM.MODEL IN (" + Model + ")  " : "") +
                @" GROUP BY  SR.CARTON_NO ";
            return SQL;
        }

        public static string DetailGridViewModelGroupByPallet(string Model)
        {
            string SQL = @"SELECT  SR.PALLET_NO, " + CreateGroupBySQL() +
                 (!string.IsNullOrEmpty(Model) ? " AND SM.MODEL IN (" + Model + ")  " : "") +
                 @" GROUP BY  SR.PALLET_NO ";
            return SQL;
        }

        #endregion

        #region by pn

        public static string DetailGridViewPartNODefault(string PN)
        {
            string sql = @"
 SELECT  SR.SN,SP.DESCRIPTION STATUS, SO.OPERATION_NAME, SR.CARTON_NO,
         SR.PALLET_NO ,SW.WO_NO, SW.PART_NO, SM.MODEL, SR.OPERATION_TIME WORK_TIME
  FROM   SFCS_RUNCARD SR, SFCS_OPERATIONS SO, SFCS_WO SW,
         SFCS_MODEL SM, SFCS_PARAMETERS SP
 WHERE   SR.WO_ID = SW.ID
         AND SW.PART_NO IN ({0})
         AND SW.MODEL_ID = SM.ID
         AND SR.WIP_OPERATION = SO.ID
         AND SR.STATUS = SP.LOOKUP_CODE
         AND SP.LOOKUP_TYPE = 'RUNCARD_STATUS'
         AND SO.ID = :OPERATION_ID
         AND SR.INPUT_TIME >=:BEGIN_TIME AND SR.INPUT_TIME <=:END_TIME ";
            return string.Format(sql, PN);
        }

        public static string DetailGridViewPartNOGroupByModel(string PN)
        {
            string SQL = @"SELECT   SM.MODEL, " + CreateGroupBySQL() +
              (!string.IsNullOrEmpty(PN) ? " AND SW.PART_NO IN (" + PN + ") " : "") +
              @" GROUP BY  SM.MODEL ";
            return SQL;
        }

        public static string DetailGridViewPartNOGroupByPartNo(string PN)
        {
            string SQL = @"SELECT   SW.PART_NO, " + CreateGroupBySQL() +
             (!string.IsNullOrEmpty(PN) ? " AND SW.PART_NO IN (" + PN + ") " : "") +
             @" GROUP BY  SW.PART_NO ";
            return SQL;
        }

        public static string DetailGridViewPartNOGroupByWO(string PN)
        {
            string SQL = @"SELECT   SW.WO_NO, " + CreateGroupBySQL() +
             (!string.IsNullOrEmpty(PN) ? " AND SW.PART_NO IN (" + PN + ") " : "") +
             @" GROUP BY  SW.WO_NO ";
            return SQL;
        }

        public static string DetailGridViewPartNOGroupByCarton(string PN)
        {
            string SQL = @"SELECT SR.CARTON_NO, " + CreateGroupBySQL() +
                (!string.IsNullOrEmpty(PN) ? " AND SW.PART_NO IN (" + PN + ") " : "") +
                @" GROUP BY  SR.CARTON_NO ";
            return SQL;
        }

        public static string DetailGridViewPartNOGroupByPallet(string PN)
        {
            string SQL = @"SELECT SR.PALLET_NO, " + CreateGroupBySQL() +
                (!string.IsNullOrEmpty(PN) ? " AND SW.PART_NO IN (" + PN + ") " : "") +
                @" GROUP BY  SR.PALLET_NO ";
            return SQL;
        }

        #endregion

        #region by wo

        public static string DetailGridViewWODefault(string WO)
        {
            string sql = @"
 SELECT  SR.SN,SP.DESCRIPTION STATUS, SO.OPERATION_NAME, SR.CARTON_NO,
         SR.PALLET_NO ,SW.WO_NO, SW.PART_NO, SM.MODEL, SR.OPERATION_TIME WORK_TIME
  FROM   SFCS_RUNCARD SR, SFCS_OPERATIONS SO, SFCS_WO SW,
         SFCS_MODEL SM, SFCS_PARAMETERS SP
 WHERE   SR.WO_ID = SW.ID
         AND SW.WO_NO IN ({0})
         AND SW.MODEL_ID = SM.ID
         AND SR.WIP_OPERATION = SO.ID
         AND SR.STATUS = SP.LOOKUP_CODE
         AND SP.LOOKUP_TYPE = 'RUNCARD_STATUS'
         AND SO.ID = :OPERATION_ID
         AND SR.INPUT_TIME >=:BEGIN_TIME AND SR.INPUT_TIME <=:END_TIME ";

            return string.Format(sql, WO);
        }

        public static string DetailGridViewWOGroupByModel(string WO)
        {
            string SQL = @"SELECT   SM.MODEL, " + CreateGroupBySQL() +
                         (!string.IsNullOrEmpty(WO) ? " AND SW.WO_NO IN (" + WO + ") " : "") +
                         @" GROUP BY  SM.MODEL ";
            return SQL;
        }

        public static string DetailGridViewWOGroupByWO(string WO)
        {
            string SQL = @"SELECT  SW.WO_NO, " + CreateGroupBySQL() +
                         (!string.IsNullOrEmpty(WO) ? " AND SW.WO_NO IN (" + WO + ") " : "") +
                         @" GROUP BY  SW.WO_NO ";
            return SQL;
        }

        public static string DetailGridViewWOGroupByPartNO(string WO)
        {
            string SQL = @"SELECT  SW.PART_NO, " + CreateGroupBySQL() +
                         (!string.IsNullOrEmpty(WO) ? " AND SW.WO_NO IN (" + WO + ") " : "") +
                         @" GROUP BY   SW.PART_NO ";
            return SQL;
        }

        public static string DetailGridViewWOGroupByCarton(string WO)
        {
            string SQL = @"SELECT  SR.CARTON_NO, " + CreateGroupBySQL() +
                         (!string.IsNullOrEmpty(WO) ? " AND SW.WO_NO IN (" + WO + ") " : "") +
                         @" GROUP BY  SR.CARTON_NO ";
            return SQL;
        }

        public static string DetailGridViewWOGroupByPallet(string WO)
        {
            string SQL = @"SELECT  SR.PALLET_NO, " + CreateGroupBySQL() +
                         (!string.IsNullOrEmpty(WO) ? " AND SW.WO_NO IN (" + WO + ") " : "") +
                         @" GROUP BY  SR.PALLET_NO ";
            return SQL;
        }

        #endregion

        #region by line

        public static string DetailGridViewLineDefault(string line)
        {
            string sql = @"
 SELECT  SR.SN,SP.DESCRIPTION STATUS, SO.OPERATION_NAME, SR.CARTON_NO,
         SR.PALLET_NO ,SW.WO_NO, SW.PART_NO, SM.MODEL, SR.OPERATION_TIME WORK_TIME
  FROM   SFCS_RUNCARD SR, SFCS_OPERATIONS SO, SFCS_WO SW,
         SFCS_MODEL SM, SFCS_PARAMETERS SP
 WHERE   SR.WO_ID = SW.ID
         AND SW.WO_NO IN ({0})
         AND SW.MODEL_ID = SM.ID
         AND SR.WIP_OPERATION = SO.ID
         AND SR.STATUS = SP.LOOKUP_CODE
         AND SP.LOOKUP_TYPE = 'RUNCARD_STATUS'
         AND SO.ID = :OPERATION_ID
         AND SR.INPUT_TIME >=:BEGIN_TIME AND SR.INPUT_TIME <=:END_TIME ";

            return string.Format(sql, line);
        }

        public static string DetailGridViewLineGroupByModel(string line)
        {
            string SQL = @"SELECT   SM.MODEL, " + CreateLineGroupBySQL() +
                         (!string.IsNullOrEmpty(line) ? " AND SOL.ID IN (" + line + ") " : "") +
                         @" GROUP BY  SM.MODEL ";
            return SQL;
        }

        public static string DetailGridViewLineGroupByWO(string line)
        {
            string SQL = @"SELECT  SW.WO_NO, " + CreateLineGroupBySQL() +
                         (!string.IsNullOrEmpty(line) ? " AND SOL.ID IN (" + line + ") " : "") +
                         @" GROUP BY  SW.WO_NO ";
            return SQL;
        }

        public static string DetailGridViewLineGroupByPartNO(string line)
        {
            string SQL = @"SELECT  SW.PART_NO, " + CreateLineGroupBySQL() +
                         (!string.IsNullOrEmpty(line) ? " AND SOL.ID IN (" + line + ") " : "") +
                         @" GROUP BY   SW.PART_NO ";
            return SQL;
        }

        public static string DetailGridViewLineGroupByCarton(string line)
        {
            string SQL = @"SELECT  SR.CARTON_NO, " + CreateLineGroupBySQL() +
                         (!string.IsNullOrEmpty(line) ? " AND SOL.ID IN (" + line + ") " : "") +
                         @" GROUP BY  SR.CARTON_NO ";
            return SQL;
        }

        public static string DetailGridViewLineGroupByPallet(string line)
        {
            string SQL = @"SELECT  SR.PALLET_NO, " + CreateLineGroupBySQL() +
                         (!string.IsNullOrEmpty(line) ? " AND SOL.ID IN (" + line + ") " : "") +
                         @" GROUP BY  SR.PALLET_NO ";
            return SQL;
        }

        #endregion

        #endregion
        #endregion
        #endregion

        #region 不良维修报表
        /// <summary>
        /// 获取不良维修报表数据
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public async Task<List<DefectReportListModel>> GetDefectReportList(DefectReportRequestModel model)
        {
            List<DefectReportListModel> list = new List<DefectReportListModel>();
            string data = string.Empty, sQuery = string.Empty, sQueryAll = "";

            if (model.ALL)
            {
                sQueryAll = S_SelectDefectReport;
            }
            else if (model.MODEL != null && model.MODEL.Count > 0)
            {
                data = SplicingStr(model.MODEL);

                sQuery = S_SelectDefectReport + string.Format(S_SelectDefectReportByModel, data);
            }
            else if (model.PART_NO != null && model.PART_NO.Count > 0)
            {
                data = SplicingStr(model.PART_NO);
                sQuery = S_SelectDefectReport + string.Format(S_SelectDefectReportByPN, data);
            }
            else if (model.WO_NO != null && model.WO_NO.Count > 0)
            {
                data = SplicingStr(model.WO_NO);
                sQuery = S_SelectDefectReport + string.Format(S_SelectDefectReportByWo, data);
            }
            else if (model.USERS != null && model.USERS.Count > 0)
            {
                data = SplicingStr(model.USERS);
                sQuery = S_SelectDefectReport + string.Format(S_SelectDefectReportByRepairer, data);
            }
            else if (model.SN != null && model.SN.Count > 0)
            {
                data = SplicingStr(model.SN);
                sQuery = S_SelectDefectReport + string.Format(S_SelectDefectReportBySN, data);
            }

            #region 都要
            if (model.REPORTTYPE == 1)
            {
                sQuery += sQueryAll + string.Format(S_SelectRepairFlag, 'N') + S_SelectFailWip + S_SelectDefectTime;
            }
            else if (model.REPORTTYPE == 2)
            {
                sQuery += sQueryAll + string.Format(S_SelectRepairFlag, 'Y') + S_SelectRepairTime;
            }
            else
            {
                sQuery += sQueryAll + S_SelectDefectTime;
            }
            if (model.PLANTTYPE == 1)
            {
                sQuery += string.Format(S_SelectPlant, GlobalVariables.pcbCode);
            }
            else if (model.PLANTTYPE == 2)
            {
                sQuery += string.Format(S_SelectPlant, GlobalVariables.pcCode);
            }
            if (model.FLOOR != null && model.FLOOR.Count > 0)
            {
                data = SplicingStr(model.FLOOR);
                sQuery += string.Format(S_SelectByLocation, data);
            }
            sQuery = S_SelectPSNHead + sQuery + S_SelectPSNEnd; 
            #endregion

            list = (await _dbConnection.QueryAsync<DefectReportListModel>(sQuery, model)).ToList();
            AddDefectDetailData(ref list);

            //坏料表没有海雄说先不要2020-11-07
            //if (model.REPORTTYPE == 3 && list != null)
            //{
            //    AddBadPartReportList(ref list);
            //}

            return list;
        }

        public void AddDefectDetailData(ref List<DefectReportListModel> list)
        {
            for (int i = 0; i < list.Count; i++)
            {
                decimal collectDetailID = 0;
                DefectReportListModel model = list[i];//repairRow

                if (model.COLLECT_DEFECT_DETAIL_ID.IsNullOrEmpty())
                {
                    collectDetailID = Convert.ToDecimal(model.COLLECT_DEFECT_DETAIL_ID);
                }
                if (collectDetailID > 0)
                {
                    var detail = _dbConnection.Query<SfcsCollectDefectsDetailListModel>(S_SelectCollectDefectsDetail, new { COLLECT_DEFECT_DETAIL_ID = collectDetailID });
                    string detailInfo = null;
                    foreach (var item in detail)
                    {
                        if (detailInfo.IsNullOrEmpty())
                        {
                            detailInfo = item.DEFECT_DETAIL;
                        }
                        else
                        {
                            detailInfo += GlobalVariables.comma + item.DEFECT_DETAIL;
                        }
                    }
                    if (!detailInfo.IsNullOrEmpty())
                    {
                        list[i].COLLECT_DEFECT_DETAIL = detailInfo;
                    }
                }
            }

        }

        /// <summary>
        /// 新增壞料表格
        /// </summary>
        /// <param name="table"></param>
        public void AddBadPartReportList(ref List<DefectReportListModel> list)
        {
            for (int i = 0; i < list.Count; i++)
            {
                DefectReportListModel model = list[i];
                string reelCode = string.Empty;
                string tempReelCode = string.Empty;
                string makerName = string.Empty;
                string dateCode = string.Empty;
                string lotCode = string.Empty;
                foreach (string location in model.LOCATION.Split(','))
                {
                    string badPartNo = model.BAD_PART_NO;
                    DefectOldMakerListModel oldMaker = null;
                    if (!badPartNo.IsNullOrEmpty())
                    {
                        oldMaker = GetOldMakerInfo(model.SN_ID, badPartNo, location);
                    }

                    if (!makerName.IsNullOrEmpty())
                    {
                        makerName += ",";
                    }

                    if (!dateCode.IsNullOrEmpty())
                    {
                        dateCode += ",";
                    }

                    if (!lotCode.IsNullOrEmpty())
                    {
                        lotCode += ",";
                    }

                    if (!reelCode.IsNullOrEmpty())
                    {
                        reelCode += ",";
                    }

                    if (oldMaker != null)
                    {
                        reelCode += oldMaker.CODE;
                        makerName += oldMaker.NAME;
                        dateCode += oldMaker.DATE_CODE;
                        lotCode += oldMaker.LOT_CODE;
                    }
                }
                list[i].OLD_REEL_CODE = reelCode;
                list[i].OLD_DATE_CODE = dateCode;
                list[i].OLD_LOT_CODE = lotCode;
                list[i].OLD_MAKER = makerName;
            }

        }
        /// <summary>
        /// 获取旧物料信息
        /// </summary>
        /// <param name="sn_id">流水号ID</param>
        /// <param name="badPartNo">坏料</param>
        /// <param name="location">不良位置</param>
        /// <returns></returns>
        private DefectOldMakerListModel GetOldMakerInfo(decimal sn_id, string badPartNo, string location)
        {
            return _dbConnection.QueryFirst<DefectOldMakerListModel>(S_SelectOldMakerInfo, new { SN_ID = sn_id, PART_NO = badPartNo, LOCATION = location });
        }

        #region product defect report sql

        public const string S_SelectRepairFlag = @" AND SCD.REPAIR_FLAG = '{0}' ";

        public const string S_SelectFailWip = @" AND SR.STATUS IN (2,16) ";

        //public const string S_SelectDefectTime = @" AND (/*?BEGIN_TIME<SCD.DEFECT_TIME >= :BEGIN_TIME|1=1>*/ :BEGIN_TIME IS NULL)
        // AND (/*?END_TIME<SCD.DEFECT_TIME  <= :END_TIME|1=1>*/ :END_TIME IS NULL) ";

        public const string S_SelectDefectTime = @"  AND SCD.DEFECT_TIME >=:BEGIN_TIME AND SCD.DEFECT_TIME <=:END_TIME ";
        public const string S_SelectRepairTime = @"  AND SCD.REPAIR_TIME >=:BEGIN_TIME AND SCD.REPAIR_TIME <=:END_TIME ";

        //public const string S_SelectRepairTime = @" AND (/*?BEGIN_TIME<SCD.REPAIR_TIME >= :BEGIN_TIME|1=1>*/ :BEGIN_TIME IS NULL)
        // AND (/*?END_TIME<SCD.REPAIR_TIME  <= :END_TIME|1=1>*/ :END_TIME IS NULL) ";

        public const string S_SelectPlant = @" AND SW.PLANT_CODE = {0} ";

        public const string S_SelectByLocation = @" AND SOS1.OPERATION_LINE_ID IN (SELECT ID FROM SFCS_OPERATION_LINES WHERE PHYSICAL_LOCATION IN ({0}) )";

        public const string S_SelectPSNHead = @"SELECT SRR.*, SFCS_SUBSTRUCTION_PKG.GET_UID_BY_SN(SRR.SN, 'PSN') PSN FROM ( ";

        public const string S_SelectPSNEnd = @" ) SRR";

        public const string S_SelectDefectReport = @"
SELECT DISTINCT
       SR.SN,
       SP1.CHINESE STATUS,
       SO.DESCRIPTION CURRENT_SITE,
       SW.WO_NO,
       SW.PART_NO,
       SM.MODEL,
       TO_CHAR (SR.INPUT_TIME, 'yyyy/mm/dd HH24:mi:ss') INPUT_TIME,
       SOS2.OPERATION_SITE_NAME REPAIR_SITE,
       SO2.DESCRIPTION REPAIR_OPERATION_NAME,
       SCD.DEFECT_CODE,
       (SELECT DEFECT_DESCRIPTION
          FROM SFCS_DEFECT_CONFIG
         WHERE DEFECT_CODE = SCD.DEFECT_CODE AND DEFECT_CLASS = SW.PLANT_CODE)
          DEFECT_EN_DESCRIPTION,
       (SELECT CHINESE_DESCRIPTION
          FROM SFCS_DEFECT_CONFIG
         WHERE DEFECT_CODE = SCD.DEFECT_CODE AND DEFECT_CLASS = SW.PLANT_CODE)
          DEFECT_CN_DESCRIPTION,
       SOS1.OPERATION_SITE_NAME DEFECT_SITE,
       SRR.REASON_CODE,
       (SELECT REASON_DESCRIPTION
          FROM SFCS_REASON_CONFIG
         WHERE     REASON_CODE = SRR.REASON_CODE
               AND REASON_CLASS = SW.PLANT_CODE
               AND REASON_CATEGORY = 1)
          REASON_EN_DESCRIPTION,
       (SELECT CHINESE_DESCRIPTION
          FROM SFCS_REASON_CONFIG
         WHERE     REASON_CODE = SRR.REASON_CODE
               AND REASON_CLASS = SW.PLANT_CODE
               AND REASON_CATEGORY = 1)
          REASON_CN_DESCRIPTION,
       TO_CHAR (SCD.DEFECT_TIME, 'yyyy/mm/dd HH24:mi:ss') DEFECT_TIME_TEXT,
       TO_CHAR (SCD.REPAIR_TIME, 'yyyy/mm/dd HH24:mi:ss') REPAIR_TIME_TEXT,
       TRUNC ( (NVL (SCD.REPAIR_TIME, SYSDATE) - SCD.DEFECT_TIME), 4)
          LEAD_TIME,
       SCD.REPAIRER,
       SCD.DEFECT_OPERATOR,
       SRR.DEFECT_DESCRIPTION,
       SRR.ROOT_CAUSE_CATEGORY,
       (SELECT DESCRIPTION
          FROM SFCS_LOOKUPS
         WHERE KIND = 1 AND CODE = SRR.ROOT_CAUSE_CATEGORY)
          CAUSE_CATEGORY_DESCRIPTION,
       SRR.RESPONSER,
       (SELECT DESCRIPTION
          FROM SFCS_LOOKUPS
         WHERE KIND = 2 AND CODE = SRR.RESPONSER)
          RESPONSER_DESCRIPTION,
       SRR.LOCATION,
       SRR.ASSEMBLY_KIND,
       SRR.REMARK,
       SRR.BAD_PART_NO,
       SRR.LOT_CODE,
       SRR.BAD_PART_VENDOR,
       SRR.REPLACED_PN_DATE_CODE,
       SRR.REPLACED_PN_VENDOR,
       SRR.REPLACED_PN_DEVICE_VALUE,
       SRR.REEL_ID,
       TO_CHAR (SCD.REPAIR_IN_TIME, 'yyyy/mm/dd HH24:mi:ss')
          REPAIR_IN_TIME_TEXT,
       SCD.REPAIR_IN_OPERATOR,
       TO_CHAR (SCD.REPAIR_OUT_TIME, 'yyyy/mm/dd HH24:mi:ss')
          REPAIR_OUT_TIME_TEXT,
       SCD.REPAIR_OUT_OPERATOR,
       SRR.ACTION_CODE,
       (SELECT DESCRIPTION
          FROM SFCS_LOOKUPS
         WHERE KIND = 3 AND CODE = SRR.ACTION_CODE)
          ACTION_EN_DESCRIPTION,
       SRR.TTF,
       SRR.DATE_CODE,
       SCD.COLLECT_DEFECT_DETAIL_ID,
       STH.FIXTURE,
       STH.MACHINE,
       TRUNC ( (SYSDATE - SW.ACTUAL_START_DATE), 4) WO_LEAD_TIME,
       SRR.RE_REPAIR_MARK,
       STH.TEST_TIME_COST,
       TO_CHAR (STH.CREATE_TIME, 'yyyy/mm/dd HH24:mi:ss') CREATE_TIME,
       SCR1.RESOURCE_NO PCB_DATECODE,
       (SELECT    SCR2.RESOURCE_NO
               || ' | '
               || SPV.VENDOR_NAME
               || ' | '
               || SPV.VENDOR_DESCRIPTION
          FROM SFCS_PRODUCT_VENDOR SPV
         WHERE SPV.VENDOR_CODE = SCR2.RESOURCE_NO)
          PCB_VENDOR,
       SCD.DEFECT_OPERATION_ID,
       SR.ID SN_ID,
       '' MAKER_NAME
  FROM SFCS_COLLECT_DEFECTS SCD,
       SFCS_REPAIR_RECIPE SRR,
       SFCS_WO SW,
       SFCS_MODEL SM,
       SFCS_OPERATION_SITES SOS1,
       SFCS_OPERATION_SITES SOS2,
       SFCS_DEFECT_CONFIG SDC,
       SFCS_REASON_CONFIG SRC,
       SFCS_RUNCARD SR,
       SFCS_PARAMETERS SP1,
       SFCS_OPERATIONS SO,
       SFCS_OPERATIONS SO2,
       (SELECT *
          FROM SFCS_COLLECT_RESOURCES
         WHERE RESOURCE_ID = 34336) SCR1,
       (SELECT *
          FROM SFCS_COLLECT_RESOURCES
         WHERE RESOURCE_ID = 31413) SCR2,
       (SELECT *
          FROM SFCS_TEST_HISTORY
         WHERE STATUS = 'FAIL') STH
 WHERE     SCD.WO_ID = SW.ID
       AND SW.MODEL_ID = SM.ID
       AND SCD.COLLECT_DEFECT_ID = SRR.COLLECT_DEFECT_ID(+)
       AND SOS2.OPERATION_ID = SO2.ID(+)
       AND SCD.DEFECT_SITE_ID = SOS1.ID(+)
       AND SCD.REPAIR_SITE_ID = SOS2.ID(+)
       AND SCD.DEFECT_CODE = SDC.DEFECT_CODE(+)
       AND SRR.REASON_CODE = SRC.REASON_CODE(+)
       --AND SRR.ROOT_CAUSE_CATEGORY = SL1.CODE(+)
       --AND SRR.RESPONSER = SL2.CODE(+)
       --AND SRR.ACTION_CODE = SL3.CODE(+)
       AND SCD.SN_ID = SR.ID
       AND SP1.LOOKUP_TYPE = 'RUNCARD_STATUS'
       AND SR.STATUS = SP1.LOOKUP_CODE
       AND SR.WIP_OPERATION = SO.ID
       AND SCD.DEFECT_OPERATION_ID = STH.OPERATION_ID(+)
       AND SR.ID = SCR1.SN_ID(+)
       AND SR.ID = SCR2.SN_ID(+) ";

        public const string S_SelectDefectReportByModel = @" AND SM.MODEL IN ({0}) ";

        public const string S_SelectDefectReportByPN = @" AND SW.PART_NO IN ({0}) ";

        public const string S_SelectDefectReportByWo = @" AND SW.WO_NO IN ({0})  ";

        public const string S_SelectDefectReportByRepairer = @" AND SCD.REPAIRER IN ({0})  ";

        public const string S_SelectDefectReportBySN = @" AND SR.SN IN ({0}) ";

        public const string S_SelectOldMakerInfo = @"SELECT IR.CODE, IR.LOT_CODE, IR.DATE_CODE, IM.NAME
                                        FROM  IMS_REEL IR, IMS_PART IP, IMS_MAKER_PART@DBLINK_IMS IMP, IMS_MAKER@DBLINK_IMS IM
                                        WHERE IM.ID = IMP.MAKER_ID
                                            AND IR.MAKER_PART_ID = IMP.ID
                                            AND IP.ID = IR.PART_ID
                                            AND IR.CODE IN (SELECT REEL_ID FROM (           
                                   SELECT REEL_ID,REGEXP_SUBSTR(REFDESIGNATOR,'[^,]+',1,L) AS LOCATION FROM (                 
                                  SELECT R.REEL_ID, DBMS_LOB.SUBSTR(GET_FUJI_SN_REEL_REFERENCE (R.ID)) REFDESIGNATOR
                                  FROM SMT_FUJI_SN_REEL_HEADER@DBLINK_FUJIPTS H,
                                       SMT_FUJI_SN_REEL@DBLINK_FUJIPTS R
                                 WHERE     H.ID = R.HEADER_ID
                                       AND H.SN_ID = :SN_ID                          
                                       AND R.PICKUP_STATUS = 0
                                       AND R.PART_NO = :PART_NO)  A,
                                       (SELECT LEVEL L FROM DUAL CONNECT BY LEVEL<=200) B
                                WHERE L <=LENGTH(A.REFDESIGNATOR) - LENGTH(REPLACE(REFDESIGNATOR,','))+1)
                                WHERE LOCATION= :LOCATION )";

        //public const string S_SelectCollectDefectsDetail = @"SELECT * FROM SFCS_COLLECT_DEFECTS_DETAIL SCDD
        //                                                     WHERE (/*?COLLECT_DEFECT_DETAIL_ID<COLLECT_DEFECT_DETAIL_ID = :COLLECT_DEFECT_DETAIL_ID|1=1>*/ :COLLECT_DEFECT_DETAIL_ID IS NULL) 
        //                                                     AND (/*?OPERATION_ID<OPERATION_ID = :OPERATION_ID|1=1>*/ :OPERATION_ID IS NULL)
        //                                                     AND (/*?SN<SN = :SN|1=1>*/ :SN IS NULL)
        //                                                     AND (/*?SN_ID<SN_ID = :SN_ID|1=1>*/ :SN_ID IS NULL) ";
        //public const string S_SelectCollectDefectsDetail = @"SELECT * FROM SFCS_COLLECT_DEFECTS_DETAIL SCDD WHERE COLLECT_DEFECT_DETAIL_ID = :COLLECT_DEFECT_DETAIL_ID AND OPERATION_ID = :OPERATION_ID AND SN = :SN  AND SN_ID = :SN_ID";
        public const string S_SelectCollectDefectsDetail = @"SELECT * FROM SFCS_COLLECT_DEFECTS_DETAIL SCDD WHERE COLLECT_DEFECT_DETAIL_ID = :COLLECT_DEFECT_DETAIL_ID";

        #endregion
        #endregion

        #region 小时产能报表
        /// <summary>
        /// 小时产能报表
        /// </summary>
        /// <param name="lineId"></param>
        /// <param name="WO_NO"></param>
        /// <param name="date"></param>
        /// <returns></returns>
        public async Task<dynamic> GetKanbanHourReport(int lineId, string WO_NO, DateTime? STARTDATE, DateTime? ENDDATE, PageModel pageModel)
        {
            var dateCondition = "";
            if (lineId != 0)
            {
                dateCondition += $" AND A.LINE_ID = {lineId} ";
            }
            if (STARTDATE != null)
            {
                dateCondition += " AND WORK_TIME >= :STARTDATE ";
            }
            if (ENDDATE != null)
            {
                dateCondition += " AND WORK_TIME <= :ENDDATE ";
            }
            if (WO_NO.IsNullOrEmpty() == false)
            {
                dateCondition += $" AND (WO_NO LIKE '%{WO_NO}%' OR PART_NO LIKE '%{WO_NO}%')  ";
            }
            var dataSql = $@"SELECT * FROM (SELECT U.*,ROWNUM AS NUM FROM (SELECT A.*,B.LINE_NAME FROM MES_KANBAN_HOUR_YIDLD A LEFT JOIN V_MES_LINES B ON A.LINE_ID=B.LINE_ID WHERE 
                                1=1 {dateCondition} ORDER BY ID DESC) u ) u
                WHERE u.NUM BETWEEN ((:Page-1) * :Limit + 1) AND (:Page * :Limit)";
            var data = await _dbConnection.QueryAsync<dynamic>(dataSql, new { STARTDATE, ENDDATE, pageModel.Limit, pageModel.Page });
            var countSql = $@"SELECT count(1) FROM MES_KANBAN_HOUR_YIDLD A WHERE 
                                1=1 {dateCondition}";
            var count = await _dbConnection.ExecuteScalarAsync<decimal>(countSql, new { STARTDATE, ENDDATE, pageModel.Limit, pageModel.Page });
            return new { data, count };
        }
        #endregion

        #region 生产报表
        /// <summary>
        /// 小时产能报表
        /// </summary>
        /// <returns></returns>
        public async Task<dynamic> GetProductionReport(int lineId, string WO_NO, int type, string lineType, DateTime? STARTDATE, DateTime? ENDDATE, PageModel pageModel)
        {
            var dateCondition = "";
            var dateType = "'YYYY-MM-dd'";
            if (lineId != 0)
            {
                dateCondition += $" AND A.LINE_ID = {lineId} ";
            }
            if (STARTDATE != null)
            {
                dateCondition += " AND WORK_TIME >= :STARTDATE ";
            }
            if (ENDDATE != null)
            {
                dateCondition += " AND WORK_TIME <= :ENDDATE ";
            }
            if (WO_NO.IsNullOrEmpty() == false)
            {
                dateCondition += $" AND (WO_NO LIKE '%{WO_NO}%' OR PART_NO LIKE '%{WO_NO}%')  ";
            }
            if (type == 1)
            {
                dateType = "'YYYY-MM'";
            }
            else if (type == 2)
            {
                dateType = "'YYYY'";
            }
            if (lineType == "PCBA")
            {
                var dataSql = $@"SELECT * FROM (SELECT A.*,ROWNUM num FROM (SELECT A.*,B.PART_NO,C.MODEL,D.LINE_NAME FROM (SELECT WO_NO,LINE_ID,TO_CHAR(WORK_TIME,{dateType}) WORK_TIME ,SUM(OUTPUT_QTY) OUTPUT_QTY,SUM(STANDARD_CAPACITY) STANDARD_CAPACITY
                            FROM MES_KANBAN_HOUR_YIDLD 
                            WHERE  LINE_TYPE='PCBA' {dateCondition}
                            GROUP BY TO_CHAR(WORK_TIME,{dateType}),WO_NO,LINE_ID) A 
                            LEFT JOIN SFCS_WO B ON A.WO_NO = B.WO_NO
                            LEFT JOIN SFCS_MODEL C ON B.MODEL_ID = C.ID
                            LEFT JOIN V_MES_LINES D ON A.LINE_ID = D. LINE_ID ORDER BY WORK_TIME DESC) A)
                WHERE NUM BETWEEN ((:Page-1) * :Limit + 1) AND (:Page * :Limit)";
                var data = await _dbConnection.QueryAsync<dynamic>(dataSql, new { STARTDATE, ENDDATE, pageModel.Limit, pageModel.Page });
                var countSql = $@"SELECT COUNT(1) FROM (SELECT WO_NO
                            FROM MES_KANBAN_HOUR_YIDLD 
                            WHERE  LINE_TYPE='PCBA' {dateCondition}
                            GROUP BY TO_CHAR(WORK_TIME,{dateType}),WO_NO,LINE_ID)";
                var count = await _dbConnection.ExecuteScalarAsync<decimal>(countSql, new { STARTDATE, ENDDATE, pageModel.Limit, pageModel.Page });
                return new { data, count };
            }
            else
            {
                var dataSql = $@"SELECT * FROM (SELECT A.*,ROWNUM num FROM (SELECT A.*,B.PART_NO,B.DESCRIPTION MODEL,D.LINE_NAME FROM (SELECT WO_NO,LINE_ID,TO_CHAR(WORK_TIME,{dateType}) WORK_TIME ,SUM(OUTPUT_QTY) OUTPUT_QTY,SUM(STANDARD_CAPACITY) STANDARD_CAPACITY
                            FROM MES_KANBAN_HOUR_YIDLD 
                            WHERE  LINE_TYPE='SMT' {dateCondition}
                            GROUP BY TO_CHAR(WORK_TIME,{dateType}),WO_NO,LINE_ID) A 
                            LEFT JOIN SMT_WO B ON A.WO_NO = B.WO_NO
                            LEFT JOIN V_MES_LINES D ON A.LINE_ID = D. LINE_ID ORDER BY WORK_TIME DESC) A)
                WHERE NUM BETWEEN ((:Page-1) * :Limit + 1) AND (:Page * :Limit)";
                var data = await _dbConnection.QueryAsync<dynamic>(dataSql, new { STARTDATE, ENDDATE, pageModel.Limit, pageModel.Page });
                var countSql = $@"SELECT COUNT(1) FROM (SELECT WO_NO
                            FROM MES_KANBAN_HOUR_YIDLD 
                            WHERE  LINE_TYPE='SMT' {dateCondition}
                            GROUP BY TO_CHAR(WORK_TIME,{dateType}),WO_NO,LINE_ID)";
                var count = await _dbConnection.ExecuteScalarAsync<decimal>(countSql, new { STARTDATE, ENDDATE, pageModel.Limit, pageModel.Page });
                return new { data, count };
            }

        }
        #endregion

        #region SMT产线看板

        /// <summary>
        /// 获取不良维修报表数据
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public async Task<List<ProducePlanInfoListModel>> TopDayProducePlan(ProducePlanInfoRequestModel mdoel)
        {
            List<ProducePlanInfoListModel> list = new List<ProducePlanInfoListModel>();
            //PLAN_QUANTITY: 
            //O_QTY: 根据排产工单号获取排产当日完成的数量
            //string sQuery = @"SELECT T.*, (CASE WHEN T.O_QTY >= T.PLAN_QUANTITY THEN 'Y' ELSE 'N' END) AS IS_OK 
            //                  FROM (SELECT P.WO_NO, TO_CHAR(P.PLAN_DATE, 'YYYY-MM-DD') PLAN_DATE,P.PLAN_QUANTITY,(SELECT COUNT(0) O_QTY
            //                  FROM SFCS_IO_STATISTICS SO  LEFT JOIN SFCS_WO SW ON SO.WO_ID = SW.ID 
            //                  WHERE SW.WO_NO = P.WO_NO AND SO.IO_TYPE = 'O' AND TO_CHAR(SO.WORK_TIME,'YYYY-MM-DD') = TO_CHAR(P.PLAN_DATE,'YYYY-MM-DD')) AS O_QTY
            //                  FROM SMT_PRODUCE_PLAN P WHERE TO_CHAR(P.PLAN_DATE,'YYYY-MM-DD') > TO_CHAR(SYSDATE - :DAY ,'YYYY-MM-DD') AND P.PLAN_TYPE = '0' AND P.LINE_ID = :LINE_ID ORDER BY  P.ID DESC) T";
            string sQuery = @"SELECT T.*, (CASE WHEN T.O_QTY >= T.PLAN_QUANTITY THEN 'Y' ELSE 'N' END) AS IS_OK 
                              FROM (SELECT P.WO_NO, TO_CHAR(P.PLAN_DATE, 'YYYY-MM-DD') PLAN_DATE,P.PLAN_QUANTITY,(SELECT NVL(SUM(SSS.PASS),0) O_QTY FROM SFCS_SITE_STATISTICS SSS  LEFT JOIN SFCS_WO SW ON SSS.WO_ID = SW.ID 
                              LEFT JOIN SFCS_OPERATION_SITES SOS ON SSS.OPERATION_SITE_ID = SOS.ID WHERE SW.WO_NO = P.WO_NO
                              AND SOS.OPERATION_ID = ( SELECT T.CURRENT_OPERATION_ID FROM (SELECT * FROM SFCS_ROUTE_CONFIG ORDER BY ORDER_NO DESC)T WHERE T.ROUTE_ID =SW.ROUTE_ID AND T.NEXT_OPERATION_ID = 999 AND ROWNUM = 1)
                              AND TO_CHAR(SSS.WORK_TIME,'YYYY-MM-DD') = TO_CHAR(P.PLAN_DATE,'YYYY-MM-DD')) AS O_QTY
                              FROM SMT_PRODUCE_PLAN P WHERE TO_CHAR(P.PLAN_DATE,'YYYY-MM-DD') = TO_CHAR(SYSDATE,'YYYY-MM-DD') AND P.PLAN_TYPE = '0' AND P.LINE_ID = :LINE_ID ORDER BY  P.ID DESC) T";

            list = (await _dbConnection.QueryAsync<ProducePlanInfoListModel>(sQuery, mdoel)).ToList();
            return list;
        }

        /// <summary>
        /// 获取不良维修报表数据
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public async Task<HourYieldListModel> GetRefershHourYield(HourYieldRequestModel mdoel)
        {
            HourYieldListModel hyModel = new HourYieldListModel();
            List<HourYieldDataListModel> list = new List<HourYieldDataListModel>();
            string sQuery = String.Format("SELECT STANDARD_CAPACITY FROM (SELECT ROWNUM R, H.STANDARD_CAPACITY FROM SMT_PRODUCTION SP INNER JOIN SMT_PLACEMENT_HEADER H ON SP.PLACEMENT_MST_ID = H.ID AND H.ENABLED = 'Y' INNER JOIN SFCS_WO SW ON SP.WO_NO = SW.WO_NO WHERE SP.LINE_ID = '{0}' AND SW.ID = '{1}') WHERE R = 1 ", mdoel.LINE_ID, mdoel.WO_ID);
            var result = await _dbConnection.ExecuteScalarAsync(sQuery);
            hyModel.STANDARD_CAPACITY = Convert.ToInt32(result);

            //var p = new DynamicParameters();
            //p.Add(":V_LINE_ID", mdoel.LINE_ID);
            //p.Add(":V_TOP_COUNT", 8);
            //await _dbConnection.ExecuteAsync("SYNC_SMT_KANBAN_HOUR_YIDLD", p, commandType: CommandType.StoredProcedure);

            //PLAN_QUANTITY: 
            //O_QTY: 根据排产工单号获取排产当日完成的数量
            sQuery = @"SELECT T.WORK_HOUR,SUM(T.PASS) AS OUTPUT_QTY,SUM(T.FAIL) AS AOI_FAIL  FROM ( SELECT TO_CHAR(SSS.WORK_TIME, 'DD') || '日' || TO_CHAR(SSS.WORK_TIME, 'HH24') || '时' AS WORK_HOUR, SSS.PASS,SSS.FAIL FROM SFCS_SITE_STATISTICS SSS, SFCS_OPERATION_SITES SOS WHERE SSS.OPERATION_SITE_ID = SOS.ID AND SOS.OPERATION_LINE_ID = :LINE_ID AND TO_CHAR(SSS.WORK_TIME, 'yyyy-mm-dd')= TO_CHAR(SYSDATE, 'yyyy-mm-dd') AND SSS.WO_ID = :WO_ID AND(FAIL + PASS) > 0 )T GROUP BY T.WORK_HOUR ORDER BY T.WORK_HOUR ASC";

            hyModel.YieldData = (await _dbConnection.QueryAsync<HourYieldDataListModel>(sQuery, mdoel)).ToList();

            return hyModel;
        }

        /// <summary>
        /// 获取不良维修报表数据
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public async Task<List<SmtAOIDefectInfoListModel>> SmtAOIDefectInfo(HourYieldRequestModel mdoel)
        {
            List<SmtAOIDefectInfoListModel> list = new List<SmtAOIDefectInfoListModel>();
            string sQuery = @"SELECT T.*,SDC.DEFECT_DESCRIPTION FROM
    (SELECT D.DEFECT_CODE,
        SUM(D.DEFECT_QTY) DEFECT_QTY FROM
        (SELECT RW.DEFECT_CODE,
        SUM(QTY) DEFECT_QTY
        FROM SFCS_DEFECT_REPORT_WORK RW,SFCS_OPERATION_SITES OS
        WHERE RW.OPERATION_SITE_ID = OS.ID
                AND RW.WO_ID = :WO_ID
                AND OS.OPERATION_LINE_ID = :LINE_ID
        GROUP BY  RW.DEFECT_CODE
        UNION
        SELECT CD.DEFECT_CODE,
        COUNT(0) DEFECT_QTY
        FROM SFCS_COLLECT_DEFECTS CD,SFCS_OPERATION_SITES OS
        WHERE CD.DEFECT_SITE_ID = OS.ID
                AND CD.WO_ID = :WO_ID
                AND OS.OPERATION_LINE_ID = :LINE_ID
        GROUP BY  CD.DEFECT_CODE )D
        GROUP BY  D.DEFECT_CODE )T LEFT JOIN SFCS_DEFECT_CONFIG SDC ON T.DEFECT_CODE = SDC.DEFECT_CODE
    ORDER BY  DEFECT_QTY DESC";

            list = (await _dbConnection.QueryAsync<SmtAOIDefectInfoListModel>(sQuery, mdoel)).ToList();

            return list;
        }

        #endregion

        #region SMT车间看板

        /// <summary>
        /// SMT周计划
        /// </summary>
        /// <param name="user_id"></param>
        /// <returns></returns>
        public async Task<List<ProducePlanInfoWeekListModel>> GetProducePlanInfoWeek(int user_id)
        {
            List<ProducePlanInfoWeekListModel> list = new List<ProducePlanInfoWeekListModel>();

            List<ProducePlanInfoWeekListModel> Linelist = await GetLineData(user_id);

            String monday = await _dbConnection.ExecuteScalarAsync<String>("SELECT TO_CHAR(TRUNC(SYSDATE,'IW'),'YYYY-MM-DD') FROM DUAL");//本星期一的日期

            //String sQuery = @"SELECT T.*, (CASE WHEN T.O_QTY >= T.PLAN_QUANTITY THEN 'Y' ELSE 'N' END) AS IS_OK 
            //                  FROM (SELECT P.WO_NO,P.LINE_ID,TO_CHAR(P.PLAN_DATE, 'YYYY-MM-DD') PLAN_DATE,P.PLAN_QUANTITY,(SELECT SUM(SSS.PASS) O_QTY FROM SFCS_SITE_STATISTICS SSS  LEFT JOIN SFCS_WO SW ON SSS.WO_ID = SW.ID 
            //                  WHERE SW.WO_NO = P.WO_NO  AND TO_CHAR(SSS.WORK_TIME,'YYYY-MM-DD') = TO_CHAR(P.PLAN_DATE,'YYYY-MM-DD')) AS O_QTY
            //                  FROM SMT_PRODUCE_PLAN P WHERE P.PLAN_DATE > (SYSDATE - (to_char(SYSDATE-1,'D'))) AND P.PLAN_DATE < (SYSDATE + (7 - to_char(SYSDATE - 1,'D'))) AND P.PLAN_TYPE = '0' ORDER BY P.PLAN_DATE ASC) T";
            String sQuery = @"SELECT T.*, (CASE WHEN T.O_QTY >= T.PLAN_QUANTITY THEN 'Y' ELSE 'N' END) AS IS_OK 
                              FROM (SELECT P.WO_NO,P.LINE_ID,TO_CHAR(P.PLAN_DATE, 'YYYY-MM-DD') PLAN_DATE,P.PLAN_QUANTITY,(SELECT NVL(SUM(SSS.PASS),0) O_QTY FROM SFCS_SITE_STATISTICS SSS  LEFT JOIN SFCS_WO SW ON SSS.WO_ID = SW.ID 
                              LEFT JOIN SFCS_OPERATION_SITES SOS ON SSS.OPERATION_SITE_ID = SOS.ID WHERE SW.WO_NO = P.WO_NO 
                              AND SOS.OPERATION_ID = ( SELECT T.CURRENT_OPERATION_ID FROM (SELECT * FROM SFCS_ROUTE_CONFIG ORDER BY ORDER_NO DESC)T WHERE T.ROUTE_ID =SW.ROUTE_ID AND T.NEXT_OPERATION_ID = 999 AND ROWNUM = 1)
                              AND TO_CHAR(SSS.WORK_TIME,'YYYY-MM-DD') = TO_CHAR(P.PLAN_DATE,'YYYY-MM-DD')) AS O_QTY
                              FROM SMT_PRODUCE_PLAN P WHERE P.PLAN_DATE >= TRUNC(NEXT_DAY(SYSDATE-8,1)+1) AND P.PLAN_DATE < TRUNC(NEXT_DAY(SYSDATE-8,1)+7)+1 AND P.PLAN_TYPE = '0' ORDER BY P.PLAN_DATE ASC) T";
            List<ProducePlanInfoListModel> Plist = (await _dbConnection.QueryAsync<ProducePlanInfoListModel>(sQuery)).ToList();//本星期所有线体的排产数据

            foreach (var item in Linelist)
            {
                ProducePlanInfoWeekListModel wModel = new ProducePlanInfoWeekListModel();
                wModel.LINE_NAME = item.LINE_NAME;
                for (int i = 0; i < 7; i++)
                {
                    WeekDataListModel model = new WeekDataListModel();
                    DateTime today = Convert.ToDateTime(monday).AddDays(i);
                    model.WEEK = GetWeekData(Convert.ToInt32(today.DayOfWeek));
                    var lineDatalist = Plist.Where(m => m.LINE_ID == item.LINE_ID && m.PLAN_DATE == today.ToString("yyyy-MM-dd")).ToList();
                    List<String> noList = lineDatalist.Select(m => m.WO_NO).Distinct().ToList();
                    foreach (String wo_no in noList)
                    {
                        WeekWoDataListModel wo = new WeekWoDataListModel();
                        wo.WO_NO = wo_no;
                        wo.O_QTY = lineDatalist.Where(m => m.WO_NO == wo_no).Sum(m => m.O_QTY);
                        wo.PLAN_QUANTITY = lineDatalist.Where(m => m.WO_NO == wo_no).Sum(m => m.PLAN_QUANTITY);
                        model.WEEK_WO_DATA.Add(wo);
                    }
                    wModel.LINE_WEEK_DATA.Add(model);
                }

                list.Add(wModel);
            }

            return list;
        }

        public String GetWeekData(int getDay)
        {
            String weekstr = "";
            switch (getDay)
            {
                case 1: weekstr = "周一"; break;
                case 2: weekstr = "周二"; break;
                case 3: weekstr = "周三"; break;
                case 4: weekstr = "周四"; break;
                case 5: weekstr = "周五"; break;
                case 6: weekstr = "周六"; break;
                case 0: weekstr = "周日"; break;
            }
            return weekstr;
        }

        public async Task<List<ProducePlanInfoWeekListModel>> GetLineData(int user_id)
        {
            String sQuery = @"SELECT M.ID AS LINE_ID, M.LINE_NAME FROM SMT_LINES M INNER JOIN (SELECT DISTINCT T.* FROM SYS_ORGANIZE T START WITH T.ID IN (SELECT ORGANIZE_ID FROM SYS_USER_ORGANIZE WHERE MANAGER_ID=:USER_ID) CONNECT BY PRIOR T.ID=T.PARENT_ORGANIZE_ID) OZ ON M.ORGANIZE_ID = OZ.ID ORDER BY M.LINE_NAME ASC";

            SmtLinesRequestModel model = new SmtLinesRequestModel();
            model.USER_ID = user_id;

            return (await _dbConnection.QueryAsync<ProducePlanInfoWeekListModel>(sQuery, model)).ToList();
        }

        /// <summary>
        /// 全部线体的自动化线看板的AOI的直通率
        /// </summary>
        /// <param name="user_id"></param>
        /// <returns></returns>
        public async Task<List<SmtKanbanAoiPassRateModel>> GetSmtKanbanAoiPassRateByAllLines(int user_id)
        {
            List<SmtKanbanAoiPassRateModel> list = new List<SmtKanbanAoiPassRateModel>();

            List<ProducePlanInfoWeekListModel> Linelist = await GetLineData(user_id);

            //String sQuery = @"SELECT (CASE WHEN T.PASS <= 0 THEN 0 ELSE ROUND((T.PASS / T.TOTAL) * 100,2) END) AS RATE,T.TOTAL,T.PASS FROM (SELECT SUM(SSS.PASS) AS PASS,(SUM(SSS.PASS) + SUM(SSS.FAIL)) AS TOTAL FROM SFCS_SITE_STATISTICS SSS,SFCS_OPERATION_SITES SOS WHERE SSS.OPERATION_SITE_ID = SOS.ID AND SOS.OPERATION_LINE_ID = :LINE_ID AND SSS.WORK_TIME >= TRUNC(SYSDATE,'IW') AND SSS.WORK_TIME < TRUNC(SYSDATE + 6,'IW'))T";
            String sQuery = @"SELECT (CASE WHEN T.PASS <= 0 THEN 0 ELSE ROUND((T.PASS / T.TOTAL) * 100,2) END) AS RATE,T.TOTAL,T.PASS FROM (SELECT SUM(SSS.PASS) AS PASS,(SUM(SSS.PASS) + SUM(SSS.FAIL)) AS TOTAL FROM SFCS_SITE_STATISTICS SSS,SFCS_OPERATION_SITES SOS,SFCS_WO SW WHERE SSS.OPERATION_SITE_ID = SOS.ID AND SSS.WO_ID = SW.ID AND SOS.OPERATION_ID = ( SELECT T.CURRENT_OPERATION_ID FROM (SELECT * FROM SFCS_ROUTE_CONFIG ORDER BY ORDER_NO DESC)T WHERE T.ROUTE_ID =SW.ROUTE_ID AND T.NEXT_OPERATION_ID = 999 AND ROWNUM = 1) AND SOS.OPERATION_LINE_ID = :LINE_ID AND SSS.WORK_TIME >= TRUNC(NEXT_DAY(SYSDATE-8,1)+1) AND SSS.WORK_TIME < TRUNC(NEXT_DAY(SYSDATE-8,1)+7)+1)T";
            foreach (var item in Linelist)
            {
                //var p = new DynamicParameters();
                //p.Add(":V_LINE_ID", item.LINE_ID);
                //await _dbConnection.ExecuteAsync("SYNC_SMT_KANBAN_AOI_PASS", p, commandType: CommandType.StoredProcedure);
                //SmtKanbanAoiPassRateModel model = await _dbConnection.QueryFirstOrDefaultAsync<SmtKanbanAoiPassRateModel>("SELECT * FROM SMT_KANBAN_AOI_PASS_RATE WHERE OPERATION_LINE_ID = :LINE_ID ", new { LINE_ID = item.LINE_ID });
                //model.OPERATION_LINE_NAME = item.LINE_NAME;
                SmtKanbanAoiPassRateModel model = await _dbConnection.QueryFirstOrDefaultAsync<SmtKanbanAoiPassRateModel>(sQuery, new { LINE_ID = item.LINE_ID });
                model.OPERATION_LINE_NAME = item.LINE_NAME;
                list.Add(model);
            }

            return list;
        }

        /// <summary>
        /// SMT今日计划达成率
        /// </summary>
        /// <param name="user_id"></param>
        /// <returns></returns>
        public async Task<List<PlanAchievementRateListModel>> GetPlanAchievementRate(int user_id)
        {
            List<PlanAchievementRateListModel> list = new List<PlanAchievementRateListModel>();

            List<ProducePlanInfoWeekListModel> Linelist = await GetLineData(user_id);

            String sQuery = @"SELECT WO_NO,SUM(PLAN_QUANTITY) AS PLAN_QUANTITY FROM SMT_PRODUCE_PLAN WHERE  TO_CHAR(PLAN_DATE,'yyyy-MM-dd') = :PLAN_DATE_BEGIN AND LINE_ID = :LINE_ID GROUP BY WO_NO";

            //String sQueryOQty = @"SELECT COUNT(0) O_QTY FROM SFCS_IO_STATISTICS SO LEFT JOIN SFCS_WO SW ON SO.WO_ID = SW.ID LEFT JOIN SFCS_OPERATION_SITES OS ON SO.OPERATION_SITE_ID = OS.ID WHERE SO.IO_TYPE = 'O' AND SW.WO_NO = :WO_NO AND OS.OPERATION_LINE_ID = :LINE_ID AND TO_CHAR(SO.WORK_TIME,'YYYY-MM-DD') = :PLAN_DATE ";
            String sQueryOQty = @"SELECT SUM(QTY) FROM SFCS_CAP_REPORT CR LEFT JOIN SFCS_WO SW ON CR.WO_ID = SW.ID LEFT JOIN SFCS_OPERATION_SITES OS ON CR.OPERATION_SITE_ID = OS.ID WHERE SW.WO_NO = :WO_NO AND TO_CHAR(CR.REPORT_TIME,'YYYY-MM-DD') = :PLAN_DATE AND OS.OPERATION_LINE_ID = :LINE_ID ";

            String toDay = await _dbConnection.ExecuteScalarAsync<String>("SELECT TO_CHAR(SYSDATE, 'YYYY-MM-DD') FROM DUAL");

            foreach (var item in Linelist)
            {
                PlanAchievementRateListModel model = new PlanAchievementRateListModel();
                model.LINE_NAME = item.LINE_NAME;

                List<ProducePlanInfoListModel> rateList = (await _dbConnection.QueryAsync<ProducePlanInfoListModel>(sQuery, new SmtProducePlanRequestModel() { LINE_ID = Convert.ToDecimal(item.LINE_ID), PLAN_DATE_BEGIN = toDay })).ToList();
                foreach (var rate in rateList)
                {
                    model.PLAN_QUANTITY += rate.PLAN_QUANTITY;
                    model.O_QTY += await _dbConnection.ExecuteScalarAsync<Decimal>(sQueryOQty, new ProducePlanInfoListModel() { WO_NO = rate.WO_NO.Trim(), LINE_ID = item.LINE_ID, PLAN_DATE = toDay });
                }

                list.Add(model);
            }

            return list;
        }

        /// <summary>
        /// SMT线体任务跟踪
        /// </summary>
        /// <param name="user_id"></param>
        /// <returns></returns>
        public async Task<List<TaskTrackBySMTLineListModel>> GetTaskTrackBySMTLine(int user_id)
        {
            List<TaskTrackBySMTLineListModel> list = new List<TaskTrackBySMTLineListModel>();

            List<ProducePlanInfoWeekListModel> Linelist = await GetLineData(user_id);

            String sQuery = @"SELECT SW.WO_NO,OS.OPERATION_LINE_ID AS LINE_ID,CR.QTY AS O_QTY,CR.REPORT_TIME 
                              FROM SFCS_CAP_REPORT CR LEFT JOIN SFCS_WO SW ON CR.WO_ID = SW.ID
                              LEFT JOIN SFCS_OPERATION_SITES OS ON CR.OPERATION_SITE_ID = OS.ID 
                              WHERE TO_CHAR(CR.REPORT_TIME,'yyyy-MM-dd') = :PLAN_DATE_BEGIN ";

            String toDay = await _dbConnection.ExecuteScalarAsync<String>("SELECT TO_CHAR(SYSDATE, 'YYYY-MM-DD') FROM DUAL");
            List<CapReportByLineListModel> reportList = (await _dbConnection.QueryAsync<CapReportByLineListModel>(sQuery, new SmtProducePlanRequestModel() { PLAN_DATE_BEGIN = toDay })).ToList();//今天的产能报工数据

            foreach (var item in Linelist)
            {
                TaskTrackBySMTLineListModel model = new TaskTrackBySMTLineListModel();
                List<TaskTracByWokListModel> taskWoList = new List<TaskTracByWokListModel>();
                model.LINE_NAME = item.LINE_NAME;
                var lineList = reportList.Where(m => m.LINE_ID == item.LINE_ID).ToList();
                foreach (var wo_no in lineList.Select(m => m.WO_NO).Distinct().ToList())
                {
                    var woList = lineList.Where(m => m.WO_NO == wo_no).ToList();
                    TaskTracByWokListModel taskWo = new TaskTracByWokListModel();
                    taskWo.WO_NO = wo_no;
                    taskWo.O_QTY = woList.Sum(m => m.O_QTY);
                    DateTime date_end = woList.Max(m => m.REPORT_TIME);
                    DateTime date_begin = woList.Min(m => m.REPORT_TIME);
                    taskWo.DATE_END = (date_end.Hour + 1).ToString();
                    taskWo.DATE_BEGIN = date_begin.Hour.ToString();
                    taskWoList.Add(taskWo);
                }
                taskWoList = taskWoList.OrderBy(m => m.DATE_BEGIN).ToList();
                model.TASK_WO = taskWoList;
                list.Add(model);
            }

            return list;
        }

        /// <summary>
        /// SMT线体效率对比
        /// </summary>
        /// <param name="user_id"></param>
        /// <returns></returns>
        public async Task<List<EfficiencyBySMTLineListModel>> GetEfficiencyBySMTLine(int user_id)
        {
            List<EfficiencyBySMTLineListModel> list = new List<EfficiencyBySMTLineListModel>();

            List<ProducePlanInfoWeekListModel> Linelist = await GetLineData(user_id);

            String sQuery = @"SELECT SSS.* FROM SFCS_SITE_STATISTICS SSS,SFCS_OPERATION_SITES SOS,SFCS_WO SW WHERE SSS.OPERATION_SITE_ID = SOS.ID AND SSS.WO_ID = SW.ID AND SSS.OPERATION_SITE_ID IN (SELECT ID FROM SFCS_OPERATION_SITES WHERE OPERATION_LINE_ID = :LINE_ID AND SOS.OPERATION_ID = ( SELECT T.CURRENT_OPERATION_ID FROM (SELECT * FROM SFCS_ROUTE_CONFIG ORDER BY ORDER_NO DESC)T WHERE T.ROUTE_ID =SW.ROUTE_ID AND T.NEXT_OPERATION_ID = 999 AND ROWNUM = 1)) AND SSS.WORK_TIME >= TRUNC(NEXT_DAY(SYSDATE-8,1)+1) AND SSS.WORK_TIME < TRUNC(NEXT_DAY(SYSDATE-8,1)+7)+1 AND SSS.PASS >0 ORDER BY SSS.WO_ID,SSS.WORK_TIME DESC";


            String S_StandardCapacity = @"SELECT PH.STANDARD_CAPACITY FROM SMT_PLACEMENT_HEADER PH 
                              INNER JOIN SFCS_PN SP ON PH.PART_NO = SP.PART_NO
                              INNER JOIN SFCS_WO SW ON SP.PART_NO = SW.PART_NO
                              WHERE SW.ID = :WO_ID AND PH.STATION_ID = (SELECT STATION_ID FROM (SELECT ROWNUM R, T.* FROM ( SELECT STATION_ID FROM SMT_ROUTE WHERE LINE_ID = :LINE_ID ORDER BY ORDER_NO DESC ) T) WHERE R = 1)
                              AND PH.ENABLED = 'Y' ORDER BY PH.CREATE_TIME DESC";

            foreach (var item in Linelist)
            {
                EfficiencyBySMTLineListModel model = new EfficiencyBySMTLineListModel();
                model.LINE_NAME = item.LINE_NAME;

                //本周的站点统计数据
                List<SfcsSiteStatistics> StatisticsList = (await _dbConnection.QueryAsync<SfcsSiteStatistics>(sQuery, new { LINE_ID = item.LINE_ID })).ToList();

                Decimal Efficiency = 0;
                Decimal StandardTotalWorkHours = 0;//标准总工时
                Decimal ActualTotalWorkHours = 0;//实际总工时
                foreach (var wo_id in StatisticsList.Select(m => m.WO_ID).Distinct().ToList())
                {
                    //标准产能
                    Decimal standardCapacity = await _dbConnection.ExecuteScalarAsync<Decimal>(S_StandardCapacity, new { WO_ID = wo_id, LINE_ID = item.LINE_ID });
                    if (standardCapacity == 0) { continue; }
                    Decimal StandardWorkHours = 1 / standardCapacity;//标准工时
                    var wsList = StatisticsList.Where(m => m.WO_ID == wo_id).ToList();
                    Decimal Capacity = Convert.ToDecimal(wsList.Sum(m => m.PASS));//按站点统计表获取到当天的产能
                    StandardTotalWorkHours += StandardWorkHours * Capacity;//标准总工时
                    ActualTotalWorkHours += wsList.Count();//实际投入工时
                }
                if (ActualTotalWorkHours != 0)
                {
                    Efficiency = StandardTotalWorkHours / ActualTotalWorkHours;
                }
                model.EFFICIENCY = Math.Round(Efficiency * 100, 2, MidpointRounding.AwayFromZero);

                list.Add(model);
            }

            return list;
        }

        #endregion

    }
}
