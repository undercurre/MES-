/*
*┌──────────────────────────────────────────────────────────────┐
*│　描    述：抽检报告主表接口实现                                                    
*│　作    者：嘉志科技                                            
*│　版    本：2.0    模板代码自动生成                                                
*│　创建时间：2019-11-26 10:07:59                             
*└──────────────────────────────────────────────────────────────┘
*┌──────────────────────────────────────────────────────────────┐
*│　命名空间： JZ.IMS.Repository.Oracle                                  
*│　类    名： MesSpotcheckHeaderRepository                                      
*└──────────────────────────────────────────────────────────────┘
*/
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
using JZ.IMS.ViewModels;
using System.Text;
using System.Linq;
using JZ.IMS.Core.Extensions;

namespace JZ.IMS.Repository.Oracle
{
	public class MesSpotcheckHeaderRepository : BaseRepository<MesSpotcheckHeader, String>, IMesSpotcheckHeaderRepository
	{
		public MesSpotcheckHeaderRepository(IOptionsSnapshot<DbOption> options)
		{
			_dbOption = options.Get("iWMS");
			if (_dbOption == null)
			{
				throw new ArgumentNullException(nameof(DbOption));
			}
			_dbConnection = ConnectionFactory.CreateConnection(_dbOption.DbType, _dbOption.ConnectionString);
		}

		/// <summary>
		/// 获取检验数据条目数
		/// </summary>
		/// <param name="model"></param>
		/// <returns></returns>
		public async Task<int> GetHeaderDataCount(MesSpotcheckHeaderRequestModel model)
		{
			string sql = @"SELECT COUNT(*) FROM MES_SPOTCHECK_HEADER TAB1 
							INNER JOIN (
										select ID, LINE_NAME,'SMT' AS LINE_TYPE from SMT_LINES
										union 
										select ID,OPERATION_LINE_NAME AS LINE_NAME,'PCBA' AS LINE_TYPE from SFCS_OPERATION_LINES WHERE ENABLED = 'Y'
								 ) TAB2 ON TAB1.LINE_ID = TAB2.ID
							INNER JOIN SMT_WO TAB3 ON TAB1.WO_NO = TAB3.WO_NO ";
			sql += GetWhereStr(model);
			return await _dbConnection.ExecuteScalarAsync<int>(sql, model);
		}

		/// <summary>
		/// 获取检验数据条目数
		/// </summary>
		/// <param name="model"></param>
		/// <returns></returns>
		public async Task<int> GetHeaderDataCountEx(MesSpotcheckHeaderRequestModel model)
		{
			string sql = "SELECT COUNT(*) FROM MES_SPOTCHECK_HEADER TAB1";
			sql += GetWhereStr(model);
			return await _dbConnection.ExecuteScalarAsync<int>(sql, model);
		}

		/// <summary>
		/// 获取检验数据
		/// </summary>
		/// <param name="model"></param>
		/// <returns></returns>
		public async Task<IEnumerable<MesSpotcheckHeaderListModel>> GetHeaderDataList(MesSpotcheckHeaderRequestModel model)
		{
			string whereStr = GetWhereStr(model);

			string sql = @"select * from (select tt.*,ROWNUM AS rowno from (
							SELECT TAB1.*,TAB2.LINE_NAME,TAB3.PART_NO,TAB3.MODEL AS PART_NAME,TAB3.DESCRIPTION AS PART_DESC 
							FROM MES_SPOTCHECK_HEADER TAB1 
							INNER JOIN (
										select ID, LINE_NAME,'SMT' AS LINE_TYPE from SMT_LINES
										union 
										select ID,OPERATION_LINE_NAME AS LINE_NAME,'PCBA' AS LINE_TYPE from SFCS_OPERATION_LINES WHERE ENABLED = 'Y'
								 ) TAB2 ON TAB1.LINE_ID = TAB2.ID
							INNER JOIN SMT_WO TAB3 ON TAB1.WO_NO = TAB3.WO_NO
							";
			sql += whereStr;
			sql += "ORDER BY BATCH_NO DESC";
			sql += ") tt where ROWNUM <= :Limit*:Page) tt2 where tt2.rowno >= (:Page-1)*:Limit";

			return await _dbConnection.QueryAsync<MesSpotcheckHeaderListModel>(sql, model);
		}

		/// <summary>
		/// 获取检验数据条目数
		/// </summary>
		/// <param name="model"></param>
		/// <returns></returns>
		public async Task<int> GetHeaderDataCountTwo(MesSpotcheckHeaderRequestModel model)
		{
			string sql = "SELECT COUNT(1) FROM MES_SPOTCHECK_HEADER SH LEFT JOIN SFCS_CONTAINER_LIST SL ON SH.BATCH_NO = SL.CONTAINER_SN LEFT JOIN SFCS_WO SW ON SH.WO_NO = SW.WO_NO LEFT JOIN IMS_PART SP ON SW.PART_NO = SP.CODE";
			sql += GetWhereStrTwo(model);
			return await _dbConnection.ExecuteScalarAsync<int>(sql, model);
        }

        /// <summary>
        /// 获取检验数据
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public async Task<IEnumerable<MesSpotcheckHeaderListModel>> GetHeaderDataTwoList(MesSpotcheckHeaderRequestModel model)
        {
            int page = 0, limit = 0;
            page = model.Page * model.Limit - model.Limit + 1;
            limit = model.Page * model.Limit;
            model.Page = page;
            model.Limit = limit;
            
			string sWhere = GetWhereStrTwo(model);

            string sQuery = string.Format("SELECT * FROM (SELECT ROWNUM R, T.* FROM ( SELECT SH.BATCH_NO,SH.WO_NO,SH.QC_TYPE,SH.LINE_ID,SH.STATUS,SL.PART_NO,SP.NAME AS PART_NAME,SP.DESCRIPTION AS PART_DESC ,SH.ALL_QTY,SH.CHECK_QTY,SH.FAIL_QTY,SH.CHECKER,SH.AUDITOR,SH.CREATE_DATE,SH.REMARK,SH.RESULT,SH.AUDIT_TIME FROM MES_SPOTCHECK_HEADER SH LEFT JOIN SFCS_CONTAINER_LIST SL ON SH.BATCH_NO = SL.CONTAINER_SN LEFT JOIN SFCS_WO SW ON SH.WO_NO = SW.WO_NO LEFT JOIN IMS_PART SP ON SW.PART_NO = SP.CODE {0} ORDER BY SH.CREATE_DATE DESC) T) WHERE R BETWEEN :Page AND :Limit", sWhere);

            return await _dbConnection.QueryAsync<MesSpotcheckHeaderListModel>(sQuery, model);
        }

        /// <summary>
        /// 根据抽检批次号获取抽检数据
        /// </summary>
        /// <param name="batchNo">抽检批次号</param>
        /// <returns></returns>
        public async Task<SpotcheckDetailListModel> GetSpotcheckDetailByBatchNo(string batchNo)
        {
            string sQuery = @"SELECT SH.BATCH_NO,SH.PARENT_BATCH_NO,SW.WO_NO,SW.PART_NO,SP.NAME AS PART_NAME,SP.DESCRIPTION AS PART_DESC,
SH.ALL_QTY,SH.CHECK_QTY,SH.STATUS,SH.RESULT,SH.REMARK,SH.QC_TYPE,SH.OPERATION_SITE_ID,SH.QCSCHEMAHEAD,SH.QCSCHEMANAME,SH.QCSCHEMAVERSION 
FROM MES_SPOTCHECK_HEADER SH LEFT JOIN SFCS_WO SW ON SH.WO_NO = SW.WO_NO LEFT JOIN IMS_PART SP ON SW.PART_NO = SP.CODE
WHERE SH.BATCH_NO = :BATCH_NO ";
            SpotcheckDetailListModel model = (await _dbConnection.QueryAsync<SpotcheckDetailListModel>(sQuery, new { BATCH_NO = batchNo })).FirstOrDefault();
            return model;
        }

        /// <summary>
        /// 根据抽检批次号获取抽检不良明细数据
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public async Task<IEnumerable<SpotcheckFailDetailRequestModel>> GetSpotcheckFailDetail(PageModel model)
		{
			int page = 0, limit = 0;
			page = model.Page * model.Limit - model.Limit + 1;
			limit = model.Page * model.Limit;
			model.Page = page;
			model.Limit = limit;

			string sQuery = @"SELECT * FROM (SELECT ROWNUM R, T.* FROM ( SELECT SD.SN,SD.STATUS,FD.DEFECT_CODE,FD.DEFECT_LOC,FD.DEFECT_DESCRIPTION,FD.DEFECT_MSG,SD.OPERATION_SITE_ID SITE_ID,OS.OPERATION_SITE_NAME SITE_NAME FROM MES_SPOTCHECK_DETAIL SD LEFT JOIN MES_SPOTCHECK_FAIL_DETAIL FD ON SD.ID = FD.SPOTCHECK_DETAIL_ID LEFT JOIN SFCS_OPERATION_SITES OS ON SD.OPERATION_SITE_ID = OS.ID WHERE SD.BATCH_NO =:Key ORDER BY SD.SN ASC) T) WHERE R BETWEEN :Page AND :Limit";

			return await _dbConnection.QueryAsync<SpotcheckFailDetailRequestModel>(sQuery, model);
		}

		/// <summary>
		/// 根据抽检批次号获取抽检不良明细数据总数
		/// </summary>
		/// <param name="batchNo">抽检批次号</param>
		/// <returns></returns>
		public async Task<int> GetFailDetailDataCount(PageModel model)
		{
			string sQuery = "SELECT COUNT(1) FROM MES_SPOTCHECK_DETAIL SD LEFT JOIN MES_SPOTCHECK_FAIL_DETAIL FD ON SD.ID = FD.SPOTCHECK_DETAIL_ID WHERE SD.BATCH_NO =:Key ORDER BY SD.SN ASC";
			return await _dbConnection.ExecuteScalarAsync<int>(sQuery, model);
		}

		/// <summary>
		/// 获取所有线别
		/// </summary>
		/// <returns></returns>
		public List<LineModel> GetLineDataAll()
		{
			string sql = @"select * from (select ID, LINE_NAME,'SMT' AS LINE_TYPE from SMT_LINES
						union 
						select ID,OPERATION_LINE_NAME AS LINE_NAME,'PCBA' AS LINE_TYPE from SFCS_OPERATION_LINES WHERE ENABLED = 'Y'
					) t order by line_type ";

			return _dbConnection.Query<LineModel>(sql).ToList();
		}

		/// <summary>
		/// 根据工单号获取产品信息
		/// </summary>
		/// <param name="wo_no">工单号</param>
		/// <returns></returns>
		public async Task<PartModel> GetPartDataByWoNo(string wo_no)
		{
			string sql = @"SELECT ID,WO_NO,PART_NO,MODEL AS PART_NAME,DESCRIPTION AS PART_DESC FROM SMT_WO WHERE WO_NO = :WONO";

			return (await _dbConnection.QueryAsync<PartModel>(sql, new { WONO = wo_no })).FirstOrDefault();
		}

		/// <summary>
		/// 获取当前线别在线工单
		/// </summary>
		/// <param name="line_id"></param>
		/// <returns></returns>
		public async Task<string> GetWoNoByLineId(decimal line_id)
		{
			string sql = @"SELECT WO_NO
						  FROM (SELECT LINE_ID, WO_NO
								  FROM SMT_PRODUCTION
								 WHERE FINISHED = 'N'
								UNION
								SELECT LINE_ID, WO_NO
								  FROM SFCS_PRODUCTION
								 WHERE FINISHED = 'N') TAB1
						 WHERE TAB1.LINE_ID = :LINE_ID";

			return (await _dbConnection.ExecuteScalarAsync<string>(sql, new { LINE_ID = line_id }));
		}

		/// <summary>
		/// 确认抽检单
		/// </summary>
		/// <param name="batch">批次号</param>
		/// <param name="user">审核人</param>
		/// <returns></returns>
		public async Task<int> ConfirmSpotCheck(int result, string batch, string user)
		{
			string sql = "UPDATE MES_SPOTCHECK_HEADER SET RESULT = :RESULT,CONFIRM =:CONFIRM WHERE BATCH_NO = :BATCH_NO";

			return await _dbConnection.ExecuteAsync(sql, new { USERNAME = user, BATCH_NO = batch });
		}

		/// <summary>
		/// 审核抽检单
		/// </summary>
		/// <param name="batch">批次号</param>
		/// <param name="auditUser">审核人</param>
		/// <returns></returns>
		public async Task<int> AuditSpotCheck(int resultStatus, string batch, string auditUser)
		{
			string sql = @"UPDATE MES_SPOTCHECK_HEADER SET STATUS = 3,RESULT = :RESULT,AUDITOR =:AUDITOR, 
						FAIL_QTY = (SELECT count(*) FROM MES_SPOTCHECK_DETAIL T1 
									INNER JOIN MES_SPOTCHECK_FAIL_DETAIL T2 ON T1.ID = T2.SPOTCHECK_DETAIL_ID
									where T1.BATCH_NO = :BATCH_NO)
						WHERE BATCH_NO = :BATCH_NO";

			return await _dbConnection.ExecuteAsync(sql, new { RESULT = resultStatus, AUDITOR = auditUser, BATCH_NO = batch });
		}

		/// <summary>
		/// 审核抽检单
		/// </summary>
		/// <returns></returns>
		public async Task<int> VerifySpotcheckHeader(VerifySpotCheckRequestModel model)
        {
            string sql = @"UPDATE MES_SPOTCHECK_HEADER SET STATUS = :STATUS,RESULT = :RESULT,AUDITOR =:AUDITOR,AUDIT_TIME = SYSDATE, REMARK = REMARK + :REMARK WHERE BATCH_NO = :BATCH_NO";

            return await _dbConnection.ExecuteAsync(sql, new { STATUS = model.STATUS, RESULT = model.RESULT, AUDITOR = model.USER_NAME, REMARK = "," + model.REMARK, BATCH_NO = model.BATCH_NO });
        }

		/// <summary>
		/// 更新过程检验单并修改完工检验单
		/// </summary>
		/// <param name="model"></param>
		/// <param name="header"></param>
		/// <param name="qcDoc"></param>
		/// <returns></returns>
		public async Task<int> UpdateSpotCheckData(VerifySpotCheckRequestModel model, MesSpotcheckHeader header,QcDocListModel qcDoc)
        {

			int result = 0;
			ConnectionFactory.OpenConnection(_dbConnection);
			using (var tran = _dbConnection.BeginTransaction())
			{
				try
				{
                    string updateSpotCheckSql = @"UPDATE MES_SPOTCHECK_HEADER SET STATUS = :STATUS,RESULT = :RESULT,AUDITOR =:AUDITOR,AUDIT_TIME = SYSDATE, REMARK = REMARK + :REMARK WHERE BATCH_NO = :BATCH_NO";
                    result = await _dbConnection.ExecuteAsync(updateSpotCheckSql, new { STATUS = model.STATUS, RESULT = model.RESULT, AUDITOR = model.USER_NAME, REMARK = "," + model.REMARK, BATCH_NO = model.BATCH_NO }, tran);
                    if (result <= 0) { throw new Exception("新增失败!"); }
                    if (model.STATUS == 3)
                    {
						//生成完工检验单

						#region 抽检报告主表添加数据

						String qcDocNo = null;//使用SFCS_PACKING_CARTON_SEQ
						decimal sequence = QueryEx<decimal>("SELECT SFCS_PACKING_CARTON_SEQ.NEXTVAL FROM DUAL ").FirstOrDefault();

						//將序列轉成36進制表示
						string resultStr = Core.Utilities.RadixConvertPublic.RadixConvert(sequence.ToString(), ViewModels.GlobalVariables.DecRadix, ViewModels.GlobalVariables.Base36Redix);

						//六位表示
						string ReleasedSequence = resultStr.PadLeft(6, '0');
						string yymmdd = QueryEx<string>("SELECT TO_CHAR(SYSDATE,'YYMMDD') YYMMDD FROM DUAL ").FirstOrDefault();
						qcDocNo = "QC" + yymmdd + ReleasedSequence;//质检单号

						string insertSpotcheckHeaderSql = @"INSERT INTO MES_SPOTCHECK_HEADER (BATCH_NO, LINE_ID, LINE_TYPE, WO_NO, ALL_QTY, CHECK_QTY, FAIL_QTY, SAMP_STANDART, SAMP_SIZE, STATUS, CHECKER, CONFIRM, AUDITOR, RESULT, CREATE_DATE, ORGANIZE_ID, WO_QTY, ORDER_NO, OUTER_CHECK_QTY, OUTER_FAIL_QTY, REMARK, WO_CLASS, QC_TYPE, PARENT_BATCH_NO, QCSCHEMAHEAD, QCSCHEMANAME, QCSCHEMAVERSION, OPERATION_SITE_ID) VALUES (:BATCH_NO, :LINE_ID, :LINE_TYPE, :WO_NO, :ALL_QTY, :CHECK_QTY, :FAIL_QTY, :SAMP_STANDART, :SAMP_SIZE, :STATUS, :CHECKER, null, null, null, SYSDATE, :ORGANIZE_ID, :WO_QTY, :ORDER_NO, :OUTER_CHECK_QTY, :OUTER_FAIL_QTY, null, null, :QC_TYPE, :PARENT_BATCH_NO, :QCSCHEMAHEAD, :QCSCHEMANAME, :QCSCHEMAVERSION, :OPERATION_SITE_ID)";
						await _dbConnection.ExecuteAsync(insertSpotcheckHeaderSql, new {
							BATCH_NO = qcDocNo,
							LINE_ID = header.LINE_ID,
							LINE_TYPE = header.LINE_TYPE,
							WO_NO = header.WO_NO,
							ALL_QTY = header.ALL_QTY,
							CHECK_QTY = header.CHECK_QTY,
							FAIL_QTY = header.FAIL_QTY,
							SAMP_STANDART = header.SAMP_STANDART,
							SAMP_SIZE = header.SAMP_SIZE,
							STATUS = 0,
							CHECKER = model.USER_NAME,
							ORGANIZE_ID = header.ORGANIZE_ID,
							WO_QTY = header.WO_QTY,
							ORDER_NO = header.ORDER_NO,
							OUTER_CHECK_QTY = header.OUTER_CHECK_QTY,
							OUTER_FAIL_QTY = header.OUTER_FAIL_QTY,
							QC_TYPE = 1,
							PARENT_BATCH_NO = header.BATCH_NO,
							QCSCHEMAHEAD = qcDoc.QCSchemaHead,
							QCSCHEMANAME = qcDoc.QCSchemaName,
							QCSCHEMAVERSION = qcDoc.QCSchemaVersion,
							OPERATION_SITE_ID = header.OPERATION_SITE_ID
						}, tran);
						
						#endregion

						#region 抽检报告子表添加数据
						if (qcDoc.DetailsData != null)
						{
							foreach (var item in qcDoc.DetailsData)
							{
								DynamicParameters parametersB = new DynamicParameters();
								decimal iteamsId = QueryEx<decimal>("SELECT MES_SPOTCHECK_ITEAMS_SEQ.NEXTVAL MY_SEQ FROM DUAL").FirstOrDefault();
								parametersB.Add("ID", iteamsId, System.Data.DbType.Decimal);//ID
								parametersB.Add("BATCH_NO", qcDocNo, System.Data.DbType.String);//主表质检编号
								parametersB.Add("STEPID", item.STEPID, System.Data.DbType.Decimal);//质检方案检验步骤ID
								parametersB.Add("ORDER_NO", item.ORDER_NO, System.Data.DbType.String);//序号
								parametersB.Add("ITEM", item.ITEM, System.Data.DbType.String);//质检名称
								parametersB.Add("SUB_ORDER_NO", item.SUB_ORDER_NO, System.Data.DbType.String);//子序号
								parametersB.Add("STANDARD", item.STANDARD, System.Data.DbType.String);//指标
								parametersB.Add("GUIDELINEVALUE1", item.GuidelineValue1, System.Data.DbType.String);//指标值
								parametersB.Add("INSPECT_METHOD", item.INSPECT_METHOD, System.Data.DbType.String);//检验方式
								parametersB.Add("QCLEVEL", item.QCLevel, System.Data.DbType.String);//检验水平
								parametersB.Add("AQL", item.AQL, System.Data.DbType.String);//
								parametersB.Add("GUIDERANGER", "", System.Data.DbType.String);//指标范围
								parametersB.Add("CHECK_QTY", header.ALL_QTY, System.Data.DbType.String);//检验数量（等于qcQty）
								parametersB.Add(GlobalVariables.PassStatus, 0, System.Data.DbType.Decimal);//合格数量（根据SN PASS 判断 FAIL 的个数判断）
								parametersB.Add(GlobalVariables.FailStatus, 0, System.Data.DbType.Decimal);//不合格数 （根据SN PASS 判断 FAIL 的个数判断）
								parametersB.Add("RESULT", 0, System.Data.DbType.Decimal);//是否合格（默认合格 0合格 1 不合格 2 不合格）
								string insertIteamsSql = @"INSERT INTO MES_SPOTCHECK_ITEAMS (ID, BATCH_NO, STEPID, ORDER_NO, ITEM, SUB_ORDER_NO, STANDARD, GUIDELINEVALUE1, INSPECT_METHOD, QCLEVEL, AQL, GUIDERANGER, CHECK_QTY, PASS, FAIL, RESULT) VALUES (:ID, :BATCH_NO, :STEPID, :ORDER_NO, :ITEM, :SUB_ORDER_NO, :STANDARD, :GUIDELINEVALUE1, :INSPECT_METHOD, :QCLEVEL, :AQL, :GUIDERANGER, :CHECK_QTY, :PASS, :FAIL, :RESULT)";
								await _dbConnection.ExecuteAsync(insertIteamsSql, parametersB, tran);
							}
						}
						#endregion
					}
                    else
                    {
                        //删除完工检验单
                        string batch_no = _dbConnection.Query<string>("SELECT BATCH_NO FROM MES_SPOTCHECK_HEADER WHERE PARENT_BATCH_NO = :BATCH_NO", new { BATCH_NO = model.BATCH_NO }).FirstOrDefault();
                        if (!String.IsNullOrEmpty(batch_no))
                        {
                            string deleteSql = @"DELETE FROM MES_SPOTCHECK_HEADER WHERE BATCH_NO = :BATCH_NO ";//主表
                            result = await _dbConnection.ExecuteAsync(deleteSql, new { BATCH_NO = batch_no }, tran);
                            deleteSql = @"DELETE FROM MES_SPOTCHECK_ITEAMS WHERE BATCH_NO = :BATCH_NO ";//子表
                            result = await _dbConnection.ExecuteAsync(deleteSql, new { BATCH_NO = batch_no }, tran);
                            deleteSql = @"DELETE FROM MES_SPOTCHECK_FAIL_DETAIL WHERE SPOTCHECK_DETAIL_ID in (SELECT ID FROM MES_SPOTCHECK_DETAIL WHERE BATCH_NO = :BATCH_NO)";//不良
                            result = await _dbConnection.ExecuteAsync(deleteSql, new { BATCH_NO = batch_no }, tran);
                            deleteSql = @"DELETE FROM MES_SPOTCHECK_DETAIL WHERE BATCH_NO = :BATCH_NO ";//抽检明细
                            result = await _dbConnection.ExecuteAsync(deleteSql, new { BATCH_NO = batch_no }, tran);
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
        /// 更新抽检项目数据
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public async Task<int> UpdateSpotCheckIteamsData(MesSpotcheckIteamsRequestModel model)
        {
            int result = 0;
            List<MesSpotcheckIteamsListModel> iList = await GetListByTableEX<MesSpotcheckIteamsListModel>("*", "MES_SPOTCHECK_ITEAMS", " And BATCH_NO =:BATCH_NO", new { BATCH_NO = model.BATCH_NO });
            if (iList != null && iList.Count > 0)
            {
                ConnectionFactory.OpenConnection(_dbConnection);
                using (var tran = _dbConnection.BeginTransaction())
                {
                    try
                    {
                        foreach (var item in model.ITEAMS)
                        {
                            if (item.ID <= 0)
                            {
                                throw new Exception("ID_NOT_EMPTY");
                            }
                            else
                            {
                                MesSpotcheckIteamsListModel iModel = iList.Where(m => m.ID == item.ID).FirstOrDefault();
                                if (iModel == null)
                                {
                                    throw new Exception("ID_NOT_EMPTY");
                                }
                                else
                                {
									decimal? pass_qty = 0;

									if (item.FAIL < 0 || item.FAIL > iModel.CHECK_QTY)
                                    {
                                        throw new Exception("FAIL_ERROR");
                                    }
                                    else
                                    {
										pass_qty = iModel.CHECK_QTY - item.FAIL;
									}

									if (item.RESULT == null || item.RESULT != 1 || item.RESULT != 2 || item.RESULT != 0)
                                    {
                                        item.RESULT = iModel.RESULT;
                                    }


                                    //更新抽检报告子表
                                    string updateIteamsSql = @"UPDATE MES_SPOTCHECK_ITEAMS SET RESULT= :RESULT,FAIL= :FAIL,PASS= :PASS WHERE ID=:ID";

                                    result += await _dbConnection.ExecuteAsync(updateIteamsSql, new
                                    {
                                        RESULT = item.RESULT,
                                        FAIL = item.FAIL,
										PASS = pass_qty,
                                        ID = item.ID
                                    }, tran);
                                }

                            }
                        }

                        tran.Commit();
                    }
                    catch (Exception ex)
                    {
                        result = -1;
                        tran.Rollback();//回滚事务
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
            }
            return result;
        }

        /// <summary>
        /// 保存QC过程检验的抽检明细
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public async Task<int> SaveFailDetailData(FailDetailRequestModel model, Decimal? qc_type)
		{
			int result = 0;
			ConnectionFactory.OpenConnection(_dbConnection);
			using (var tran = _dbConnection.BeginTransaction())
			{
				try
				{
					String sQuery = @"SELECT NVL(COUNT(1) ,0) FROM MES_SPOTCHECK_DETAIL WHERE BATCH_NO = :BATCH_NO AND STATUS ='PASS' ";
					decimal passQty = QueryEx<decimal>(sQuery, new { BATCH_NO = model.BATCH_NO }).FirstOrDefault();
					sQuery = @"SELECT NVL(COUNT(1) ,0) FROM MES_SPOTCHECK_DETAIL WHERE BATCH_NO = :BATCH_NO AND STATUS ='FAIL'";
					decimal failQty = QueryEx<decimal>(sQuery, new { BATCH_NO = model.BATCH_NO }).FirstOrDefault();
					if (model.STATUS == "PASS") { passQty += 1; } else { failQty += 1; }
					decimal qcQty = passQty + failQty;

					//更新抽检报告主表
					string updateHeaderSql = @"UPDATE MES_SPOTCHECK_HEADER SET ALL_QTY= :ALL_QTY,CHECK_QTY= :CHECK_QTY WHERE BATCH_NO=:BATCH_NO";
					await _dbConnection.ExecuteAsync(updateHeaderSql, new
					{
						ALL_QTY = qcQty,
						CHECK_QTY = qcQty,
						BATCH_NO = model.BATCH_NO
					}, tran);

                    if (qc_type == 1) //只有优特的完工检验才有检验项目
                    {
                        //更新抽检报告子表
                        string updateIteamsSql = @"UPDATE MES_SPOTCHECK_ITEAMS SET CHECK_QTY= :CHECK_QTY,PASS= :PASS,FAIL= :FAIL WHERE BATCH_NO=:BATCH_NO";

                        await _dbConnection.ExecuteAsync(updateIteamsSql, new
                        {
                            CHECK_QTY = qcQty,
                            PASS = passQty,
                            FAIL = failQty,
                            BATCH_NO = model.BATCH_NO
                        }, tran);
                    }

                    //根据SN更新质检明细
                    decimal detailId = 0;
                    string id = QueryEx<string>("SELECT T.ID FROM MES_SPOTCHECK_DETAIL T WHERE T.BATCH_NO = :BATCH_NO AND T.SN =:SN ", new { BATCH_NO = model.BATCH_NO, SN = model.SN }).FirstOrDefault();
                    if (id.IsNullOrWhiteSpace())
                    {
                        detailId = QueryEx<decimal>("SELECT MES_SPOTCHECK_DETAIL_SEQ.NEXTVAL MY_SEQ FROM DUAL").FirstOrDefault();
                        string insertSql = @"INSERT INTO MES_SPOTCHECK_DETAIL (ID, BATCH_NO, SN, STATUS, CREATE_TIME, CREATOR, OPERATION_SITE_ID) VALUES (:ID, :BATCH_NO, :SN, :STATUS, SYSDATE, :CREATOR, :SITE_ID)";
                        result += await _dbConnection.ExecuteAsync(insertSql, new
                        {
                            ID = detailId,
                            BATCH_NO = model.BATCH_NO,
                            SN = model.SN,
                            STATUS = model.STATUS,
                            CREATOR = model.USER_NAME,
							SITE_ID = model.SITE_ID
						}, tran);
                    }
                    else
                    {
                        detailId = Convert.ToDecimal(detailId);
                        string updateSql = @"UPDATE MES_SPOTCHECK_DETAIL SET STATUS= :STATUS,CREATOR= :CREATOR,CREATE_TIME= SYSDATE,OPERATION_SITE_ID= :SITE_ID WHERE BATCH_NO=:BATCH_NO AND SN =:SN";
                        result += await _dbConnection.ExecuteAsync(updateSql, new
                        {
                            STATUS = model.STATUS,
                            CREATOR = model.USER_NAME,
                            BATCH_NO = model.BATCH_NO,
                            SN = model.SN,
							SITE_ID = model.SITE_ID
						}, tran);
                    }
					//写入不良信息
                    if (result > 0 && model.STATUS == "FAIL")
                    {
						decimal failId = QueryEx<decimal>("SELECT MES_SPOTCHECK_FAIL_DETAIL_SEQ.NEXTVAL MY_SEQ FROM DUAL").FirstOrDefault();
						string insertSql = @"INSERT INTO MES_SPOTCHECK_FAIL_DETAIL (ID, SPOTCHECK_DETAIL_ID, DEFECT_CODE, DEFECT_LOC, DEFECT_DESCRIPTION, DEFECT_MSG) VALUES (:ID, :SPOTCHECK_DETAIL_ID, :DEFECT_CODE, :DEFECT_LOC, :DEFECT_DESCRIPTION, :DEFECT_MSG)";
						result += await _dbConnection.ExecuteAsync(insertSql, new
						{
							ID = failId,
							SPOTCHECK_DETAIL_ID = detailId,
							DEFECT_CODE = model.DEFECT_CODE,
							DEFECT_LOC = model.DEFECT_LOC,
							DEFECT_DESCRIPTION = model.DEFECT_DESCRIPTION,
							DEFECT_MSG = model.DEFECT_MSG
						}, tran);
					}

                    tran.Commit();
				}
				catch (Exception ex)
				{
					result = -1;
					tran.Rollback();//回滚事务
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
        /// 根据抽检批次号和SN删除质检明细数据
        /// </summary>
        /// <param name="batch_no">抽检批次号</param>
        /// <param name="sn">产品流水号</param>
        /// <returns></returns>
        public async Task<int> DeleteSpotCheckDetailByBatchNo(string batch_no, string sn)
        {
            int result = 0;
            ConnectionFactory.OpenConnection(_dbConnection);
            using (var tran = _dbConnection.BeginTransaction())
            {
                try
                {

                    //删除异常数据
                    string deleteSql = "DELETE MES_SPOTCHECK_FAIL_DETAIL WHERE SPOTCHECK_DETAIL_ID IN (SELECT ID FROM MES_SPOTCHECK_DETAIL WHERE BATCH_NO = :BATCH_NO AND SN = :SN) ";
                    await _dbConnection.ExecuteAsync(deleteSql, new { BATCH_NO = batch_no, SN = sn });

                    //删除明细数据
                    result = await _dbConnection.ExecuteAsync("DELETE MES_SPOTCHECK_DETAIL WHERE BATCH_NO = :BATCH_NO AND SN = :SN ", new { BATCH_NO = batch_no, SN = sn });

                    if (result > 0)
                    {
                        //删除一条明细就要减少抽检数量
                        String sQuery = @"SELECT NVL(COUNT(1) ,0) FROM MES_SPOTCHECK_DETAIL WHERE BATCH_NO = :BATCH_NO AND STATUS ='PASS' ";
                        decimal passQty = QueryEx<decimal>(sQuery, new { BATCH_NO = batch_no }).FirstOrDefault();
                        sQuery = @"SELECT NVL(COUNT(1) ,0) FROM MES_SPOTCHECK_DETAIL WHERE BATCH_NO = :BATCH_NO AND STATUS ='FAIL'";
                        decimal failQty = QueryEx<decimal>(sQuery, new { BATCH_NO = batch_no }).FirstOrDefault();
                        decimal qcQty = passQty + failQty;

                        //更新抽检报告主表
                        string updateHeaderSql = @"UPDATE MES_SPOTCHECK_HEADER SET ALL_QTY = :QCQTY,CHECK_QTY = :QCQTY WHERE BATCH_NO=:BATCH_NO";
                        await _dbConnection.ExecuteAsync(updateHeaderSql, new { QCQTY = qcQty, BATCH_NO = batch_no }, tran);

                        //更新抽检报告子表

                        string updateIteamsSql = @"UPDATE MES_SPOTCHECK_ITEAMS SET CHECK_QTY= :CHECK_QTY,PASS= :PASS,FAIL= :FAIL WHERE BATCH_NO=:BATCH_NO";
                        await _dbConnection.ExecuteAsync(updateIteamsSql, new
                        {
                            CHECK_QTY = qcQty,
                            PASS = passQty,
                            FAIL = failQty,
                            BATCH_NO = batch_no
                        }, tran);

                    }

                    tran.Commit();
                }
                catch (Exception ex)
                {
                    tran.Rollback(); // 回滚事务
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
        /// 删除抽检单
        /// </summary>
        /// <param name="batch"></param>
        /// <returns></returns>
        public async Task<int> DeleteSpotCheck(string batch)
		{
			ConnectionFactory.OpenConnection(_dbConnection);
			using (var tran = _dbConnection.BeginTransaction())
			{
				try
				{
					string sql = @"DELETE MES_SPOTCHECK_FAIL_DETAIL WHERE SPOTCHECK_DETAIL_ID IN (
								SELECT ID FROM MES_SPOTCHECK_DETAIL WHERE BATCH_NO = :BATCH_NO
							)";
					//删除异常数据
					await _dbConnection.ExecuteAsync(sql, new { BATCH_NO = batch });
					//删除明细数据
					await _dbConnection.ExecuteAsync("DELETE MES_SPOTCHECK_DETAIL WHERE BATCH_NO = :BATCH_NO", new { BATCH_NO = batch });
					//删除抽检单
					int i = await _dbConnection.ExecuteAsync("DELETE MES_SPOTCHECK_HEADER WHERE BATCH_NO = :BATCH_NO", new { BATCH_NO = batch });
					tran.Commit();
					return i;
				}
				catch
				{
					tran.Rollback(); // 回滚事务
					throw;
				}
				finally
				{
					if (_dbConnection.State != System.Data.ConnectionState.Closed)
					{
						_dbConnection.Close();
					}
				}
			}
		}

		/// <summary>
		/// 判断当前批次号的抽检单是否存在
		/// </summary>
		/// <param name="batch">批次号</param>
		/// <returns></returns>
		public bool IsExits(string batch)
		{
			string sql = "select count(*) from MES_SPOTCHECK_HEADER where BATCH_NO = :BATCH_NO";

			int? i = _dbConnection.Query<int>(sql, new { BATCH_NO = batch }).FirstOrDefault();
			if (i != null && i > 0)
				return true;
			return false;
		}

		/// <summary>
		/// 根据批次号获取当前单据状态
		/// </summary>
		/// <param name="batch">批次号</param>
		/// <returns></returns>
		public int GetStatusByBatch(string batch)
		{
			string sql = "SELECT STATUS FROM MES_SPOTCHECK_HEADER WHERE BATCH_NO = :BATCH_NO";
			int i = _dbConnection.Query<int>(sql, new { BATCH_NO = batch }).FirstOrDefault();
			return i;
		}

		/// <summary>
		/// 获取最大批次
		/// </summary>
		/// <param name="lineId"></param>
		/// <param name="wo_no"></param>
		/// <returns></returns>
		public decimal GetOrderNo(decimal lineId, string wo_no)
		{
			string sql = "SELECT NVL(MAX(ORDER_NO),0) AS ORDER_NO FROM MES_SPOTCHECK_HEADER WHERE LINE_ID = :LINE_ID AND WO_NO = :WO_NO";

			decimal orderNo = _dbConnection.ExecuteScalar<decimal>(sql, new { LINE_ID = lineId, WO_NO = wo_no });
			return orderNo + 1;
		}

		/// <summary>
		/// 获取抽检数是否大于明细数量
		/// </summary>
		/// <param name="batch"></param>
		/// <returns></returns>
		public bool IsCheckQtyGTDetailQty(string batch)
		{
			string sql = @"SELECT (CASE WHEN CHECK_QTY > (SELECT COUNT(*) FROM MES_SPOTCHECK_DETAIL WHERE BATCH_NO = :BATCH_NO)  THEN 1 ELSE 0 END) AS ISGT 
				FROM MES_SPOTCHECK_HEADER WHERE BATCH_NO = :BATCH_NO";

			int i = _dbConnection.Query<int>(sql, new { BATCH_NO = batch }).FirstOrDefault();
			return i == 1;
		}

		/// <summary>
		/// 获取WHERE条件
		/// </summary>
		/// <param name="model"></param>
		/// <returns></returns>
		private string GetWhereStr(MesSpotcheckHeaderRequestModel model)
		{
			StringBuilder whereStr = new StringBuilder();
			whereStr.Append(" WHERE TAB1.QC_TYPE IS NULL ");

			if (model.LINE_ID != 0)
				whereStr.Append(" and TAB1.LINE_ID=:LINE_ID ");

            if (!string.IsNullOrEmpty(model.PART_NO))
				whereStr.Append(" and  instr(TAB3.PART_NO,:PART_NO)>0 ");

			if (!string.IsNullOrEmpty(model.WO_NO))
				whereStr.Append(" and  instr(TAB1.WO_NO,:WO_NO)>0 ");

			if (model.STATUS > -1)
				whereStr.Append(" and TAB1.STATUS=:STATUS ");

			if (model.RESULT > -1)
				whereStr.Append(" and TAB1.RESULT=:RESULT ");

			if (model.BEGIN_DATE != null)
				whereStr.Append(" and TAB1.CREATE_DATE >= :BEGIN_DATE ");

			if (model.END_DATE != null)
				whereStr.Append(" and TAB1.CREATE_DATE >= :END_DATE ");

			return whereStr.ToString();
		}

		/// <summary>
		/// 获取WHERE条件
		/// </summary>
		/// <param name="model"></param>
		/// <returns></returns>
		private string GetWhereStrTwo(MesSpotcheckHeaderRequestModel model)
		{
			string sWhere = " WHERE 1=1 ";

			if (model.LINE_ID != 0)
			{
				sWhere += $" AND SH.LINE_ID = :LINE_ID ";
			}
			if (model.QC_TYPE > -1)
			{
				sWhere += $" AND SH.QC_TYPE=:QC_TYPE ";
            }
            else
            {
                sWhere += $" AND SH.QC_TYPE IN ( 0,1,2) ";
            }
			if (!string.IsNullOrEmpty(model.PART_NO))
			{
				sWhere += $" AND INSTR(SL.PART_NO,:PART_NO)>0 ";
			}
			if (!string.IsNullOrEmpty(model.BATCH_NO))
			{
				sWhere += $" AND INSTR(SH.BATCH_NO,:BATCH_NO)>0  ";
			}
			if (!string.IsNullOrEmpty(model.WO_NO))
			{
				sWhere += $" AND INSTR(SH.WO_NO,:WO_NO)>0  ";
			}
			if (model.STATUS > -1)
			{
				sWhere += $" AND SH.STATUS=:STATUS ";
			}
			if (model.BEGIN_DATE != null)
			{
				sWhere += $" AND SH.CREATE_DATE >= :BEGIN_DATE ";
			}
			if (model.END_DATE != null)
			{
				sWhere += $" AND SH.CREATE_DATE <= :END_DATE ";
			}

			return sWhere;
		}

		/// <summary>
		/// 设置不良数量
		/// </summary>
		/// <param name="batch"></param>
		private async Task<int> SetFailQty(string batch)
		{
			string sql = @"UPDATE MES_SPOTCHECK_HEADER
						   SET FAIL_QTY =
								  (SELECT COUNT(*)

									 FROM MES_SPOTCHECK_DETAIL T1
										  INNER JOIN MES_SPOTCHECK_FAIL_DETAIL T2
											 ON T1.ID = T2.SPOTCHECK_DETAIL_ID

									WHERE T1.BATCH_NO = :BATCH_NO)
						 WHERE BATCH_NO = :BATCH_NO";

			return await _dbConnection.ExecuteAsync(sql, new { BATCH_NO = batch });
		}

		#region 明细
		/// <summary>
		/// 根据检验批次号获取抽检数据
		/// </summary>
		/// <param name="batch">批次号</param>
		/// <returns></returns>
		public async Task<IEnumerable<MesSpotcheckDetailModel>> GetDetailDataList(string batch)
		{
			string sql = @"SELECT TAB1.*,TAB2.DEFECT_CODE,TAB2.DEFECT_LOC,TAB2.DEFECT_DESCRIPTION,TAB2.DEFECT_MSG FROM MES_SPOTCHECK_DETAIL TAB1 
						LEFT JOIN MES_SPOTCHECK_FAIL_DETAIL TAB2 ON TAB1.ID = TAB2.SPOTCHECK_DETAIL_ID
						WHERE TAB1.BATCH_NO = :BATCH_NO ORDER BY tab1.ID
					";

			return await _dbConnection.QueryAsync<MesSpotcheckDetailModel>(sql, new { BATCH_NO = batch });
		}

		/// <summary>
		/// 根据类型获取数据字典
		/// </summary>
		/// <param name="type"></param>
		/// <returns></returns>
		public List<SfcsParameters> GetParametersByType(string type)
		{
			string sql = @"SELECT * FROM SFCS_PARAMETERS WHERE LOOKUP_TYPE = :LOOKUP_TYPE AND ENABLED = 'Y'";

			return _dbConnection.Query<SfcsParameters>(sql, new { LOOKUP_TYPE = type }).ToList();
		}

		/// <summary>
		/// 获取不良数据
		/// </summary>
		/// <param name="model"></param>
		/// <returns></returns>
		public async Task<IEnumerable<SfcsDefectConfigListModel>> LoadDefectData(SfcsDefectConfigRequestModel model)
		{
			string sql = @"SELECT MST.*,
							   d_type.CHINESE AS DEFECT_TYPE_NAME,
							   d_class.CHINESE AS DEFECT_CLASS_NAME,
							   d_category.CHINESE AS DEFECT_CATEGORY_NAME,
							   d_level.CHINESE AS LEVEL_CODE_NAME
						  FROM SFCS_DEFECT_CONFIG mst
							   LEFT JOIN (SELECT LOOKUP_CODE, CHINESE
											FROM SFCS_PARAMETERS
										   WHERE LOOKUP_TYPE = 'DEFECT_TYPE' AND ENABLED = 'Y') d_type
								  ON d_type.LOOKUP_CODE = MST.DEFECT_TYPE
							   LEFT JOIN (SELECT LOOKUP_CODE, CHINESE
											FROM SFCS_PARAMETERS
										   WHERE lookup_type = 'DEFECT_CLASS' AND ENABLED = 'Y') d_class
								  ON d_class.LOOKUP_CODE = MST.DEFECT_CLASS
							   LEFT JOIN (SELECT LOOKUP_CODE, CHINESE
											FROM SFCS_PARAMETERS
										   WHERE lookup_type = 'DEFECT_CATEGORY' AND ENABLED = 'Y') d_category
								  ON d_category.LOOKUP_CODE = MST.DEFECT_CATEGORY
							   LEFT JOIN (SELECT LOOKUP_CODE, CHINESE
											FROM SFCS_PARAMETERS
										   WHERE lookup_type = 'DEFECT_LEVEL_CODE' AND ENABLED = 'Y') d_level
								  ON d_level.LOOKUP_CODE = MST.LEVEL_CODE";

			string whereStr = " where MST.ENABLED = 'Y' ";

			if (model.DEFECT_TYPE != 0)
				whereStr += " and mst.DEFECT_TYPE = :DEFECT_TYPE ";

			if (model.DEFECT_CLASS != 0)
				whereStr += " and mst.DEFECT_CLASS = :DEFECT_CLASS ";

			if (model.DEFECT_CATEGORY != 0)
				whereStr += " and mst.DEFECT_CATEGORY = :DEFECT_CATEGORY ";

			if (model.DEFECT_LEVEL_CODE != 0)
				whereStr += " and mst.LEVEL_CODE = :DEFECT_LEVEL_CODE ";

			if (!string.IsNullOrEmpty(model.DEFECT_CODE))
				whereStr += " and MST.DEFECT_CODE = :DEFECT_CODE ";

			sql += whereStr;
			sql += " ORDER BY MST.DEFECT_CODE";

			return await _dbConnection.QueryAsync<SfcsDefectConfigListModel>(sql, model);
		}

		/// <summary>
		/// 获取明细ID
		/// </summary>
		/// <returns></returns>
		private decimal GetDetailSeq()
		{
			string sql = "SELECT MES_SPOTCHECK_DETAIL_SEQ.NEXTVAL FROM DUAL";
			var result = _dbConnection.ExecuteScalar(sql);
			return (decimal)result;
		}

		/// <summary>
		/// 获取明细不良ID
		/// </summary>
		/// <returns></returns>
		private decimal GetDetailFailSeq()
		{
			string sql = "SELECT MES_SPOTCHECK_FAIL_DETAIL_SEQ.NEXTVAL FROM DUAL";
			var result = _dbConnection.ExecuteScalar(sql);
			return (decimal)result;
		}

		/// <summary>
		/// 新增或修改明细
		/// </summary>
		/// <param name="model"></param>
		/// <returns></returns>
		public async Task<int> AddOrEditDetail(MesSpotcheckDetailModel model)
		{
			MesSpotcheckFailDetail fail = new MesSpotcheckFailDetail();
			fail.DEFECT_CODE = model.DEFECT_CODE;
			fail.DEFECT_DESCRIPTION = model.DEFECT_DESCRIPTION;
			fail.DEFECT_LOC = model.DEFECT_LOC;
			fail.DEFECT_MSG = model.DEFECT_MSG;
			fail.ID = GetDetailFailSeq();
			string addFailSql = @"INSERT INTO MES_SPOTCHECK_FAIL_DETAIL VALUES(:ID,:SPOTCHECK_DETAIL_ID,:DEFECT_CODE,:DEFECT_LOC,:DEFECT_DESCRIPTION,:DEFECT_MSG)";

			int i = 0;
			ConnectionFactory.OpenConnection(_dbConnection);
			using (var tran = _dbConnection.BeginTransaction())
			{
				try
				{
					if (model.ID == 0)
					{
						//Add
						model.ID = GetDetailSeq();
						if (model.STATUS == "FAIL")
						{
							fail.SPOTCHECK_DETAIL_ID = model.ID;
							await _dbConnection.ExecuteAsync(addFailSql, fail);
						}
						string detailSql = @"INSERT INTO MES_SPOTCHECK_DETAIL(ID,BATCH_NO,SN,STATUS,CREATE_TIME,CREATOR) VALUES(:ID,:BATCH_NO,:SN,:STATUS,SYSDATE,:CREATOR)";
						i = await _dbConnection.ExecuteAsync(detailSql, model);
						tran.Commit();
					}
					else
					{
						//Edit
						fail.SPOTCHECK_DETAIL_ID = model.ID;
						string deleteFailSql = "DELETE FROM MES_SPOTCHECK_FAIL_DETAIL WHERE SPOTCHECK_DETAIL_ID = :SPOTCHECK_DETAIL_ID";
						await _dbConnection.ExecuteAsync(deleteFailSql, fail);

						if (model.STATUS == "FAIL")
						{
							await _dbConnection.ExecuteAsync(addFailSql, fail);
						}
						string editDetailSql = @"UPDATE MES_SPOTCHECK_DETAIL SET SN=:SN,STATUS=:STATUS WHERE ID=:ID";
						i = await _dbConnection.ExecuteAsync(editDetailSql, model);
						tran.Commit();
					}
				}
				catch
				{
					tran.Rollback(); // 回滚事务
					throw;
				}
				finally
				{
					if (_dbConnection.State != System.Data.ConnectionState.Closed)
					{
						_dbConnection.Close();
					}
				}

				await SetFailQty(model.BATCH_NO);
				return i;
			}
		}

		/// <summary>
		/// 删除明细数据
		/// </summary>
		/// <param name="id"></param>
		/// <returns></returns>
		public async Task<int> DeleteDetail(decimal id, string batch)
		{
			string delFailSql = "DELETE MES_SPOTCHECK_FAIL_DETAIL WHERE SPOTCHECK_DETAIL_ID=:ID ";
			string delDetailSql = "DELETE MES_SPOTCHECK_DETAIL WHERE ID=:ID";

			int i = 0;
			ConnectionFactory.OpenConnection(_dbConnection);
			using (var tran = _dbConnection.BeginTransaction())
			{
				try
				{
					await _dbConnection.ExecuteAsync(delFailSql, new { ID = id });
					i = await _dbConnection.ExecuteAsync(delDetailSql, new { ID = id });

					if (i > 0)
						tran.Commit();
					else
						tran.Rollback();
				}
				catch
				{
					tran.Rollback(); // 回滚事务
					throw;
				}
				finally
				{
					if (_dbConnection.State != System.Data.ConnectionState.Closed)
					{
						_dbConnection.Close();
					}
				}
			}

			await SetFailQty(batch);
			return i;
		}
        #endregion
    }
}