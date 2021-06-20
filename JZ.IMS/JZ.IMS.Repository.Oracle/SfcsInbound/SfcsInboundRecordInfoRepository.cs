/*
*┌──────────────────────────────────────────────────────────────┐
*│　描    述：入库记录信息表接口实现                                                    
*│　作    者：嘉志科技                                            
*│　版    本：2.0    模板代码自动生成                                                
*│　创建时间：2021-04-27 17:00:47                             
*└──────────────────────────────────────────────────────────────┘
*┌──────────────────────────────────────────────────────────────┐
*│　命名空间： JZ.IMS.Repository.Oracle                                  
*│　类    名： SfcsInboundRecordInfoRepository                                      
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

namespace JZ.IMS.Repository.Oracle
{
    public class SfcsInboundRecordInfoRepository:BaseRepository<SfcsInboundRecordInfo,Decimal>, ISfcsInboundRecordInfoRepository
    {
        public SfcsInboundRecordInfoRepository(IOptionsSnapshot<DbOption> options)
        {
            _dbOption =options.Get("iWMS");
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
			string sql = "SELECT ENABLED FROM SFCS_INBOUND_RECORD_INFO WHERE ID=:ID";
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
			string sql = "UPDATE SFCS_INBOUND_RECORD_INFO set ENABLED=:ENABLED WHERE ID=:Id";
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
			string sql = "SELECT SFCS_INBOUND_RECORD_INFO_SEQ.NEXTVAL MY_SEQ FROM DUAL";
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
			string sql = "select count(0) from SFCS_INBOUND_RECORD_INFO where id = :id";
			object result = await _dbConnection.ExecuteScalarAsync(sql, new
			{
				id
			});

			return (Convert.ToInt32(result) > 0);
		}

        /// <summary>
        /// 保存数据
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public async Task<decimal> SaveDataByTrans(SfcsInboundRecordInfoModel model)
        {
            int result = 1;
            ConnectionFactory.OpenConnection(_dbConnection);
            using (var tran = _dbConnection.BeginTransaction())
            {
                try
                {
                    List<decimal> widlist = new List<decimal>();
                    //新增
                    string insertSql = @"INSERT INTO SFCS_INBOUND_RECORD_INFO (ID,WO_ID,INBOUND_NO,INBOUND_QTY,STATUS,CREATE_TIME,CREATE_BY) 
					VALUES (:ID,:WO_ID,:INBOUND_NO,:INBOUND_QTY,'0',SYSDATE,:CREATE_BY)";
                    if (model.InsertRecords != null && model.InsertRecords.Count > 0)
                    {
                        foreach (var item in model.InsertRecords)
                        {
                            var newid = await GetSEQID();
                            SfcsWo swModel = QueryEx<SfcsWo>("SELECT * FROM SFCS_WO WHERE WO_NO = :WO_NO", new { WO_NO = item.WO_NO })?.FirstOrDefault();
                            if (swModel == null) { throw new Exception("WO_NO_NOT_EXIST"); }
                            item.WO_ID = Convert.ToDecimal(swModel.ID);
                            if (!widlist.Exists(m => m == item.WO_ID)) { widlist.Add(item.WO_ID); }
                            if (String.IsNullOrEmpty(item.CREATE_BY)) { throw new Exception("USER_NULL"); }
                            if (item.INBOUND_QTY < 1) { throw new Exception("INBOUND_QTY_ERROR"); }
                            //將序列轉成36進制表示
                            string r = Core.Utilities.RadixConvertPublic.RadixConvert(newid.ToString(), ViewModels.GlobalVariables.DecRadix, ViewModels.GlobalVariables.Base36Redix);
                            //六位表示
                            string ReleasedSequence = r.PadLeft(6, '0');
                            string yymmdd = QueryEx<string>("SELECT TO_CHAR(SYSDATE,'YYMMDD') YYMMDD FROM DUAL ").FirstOrDefault();
                            item.INBOUND_NO = "BT" + yymmdd + ReleasedSequence;
                            var resdata = await _dbConnection.ExecuteAsync(insertSql, new
                            {
                                ID = newid,
                                item.WO_ID,
                                item.INBOUND_NO,
                                item.INBOUND_QTY,
                                item.CREATE_BY,

                            }, tran);
                        }
                    }

                    //更新
                    string updateSql = @"UPDATE SFCS_INBOUND_RECORD_INFO SET INBOUND_QTY=:INBOUND_QTY,UPDATE_TIME=SYSDATE,UPDATE_BY=:UPDATE_BY WHERE ID=:ID AND STATUS = '0' ";
                    if (model.UpdateRecords != null && model.UpdateRecords.Count > 0)
                    {
                        foreach (var item in model.UpdateRecords)
                        {
                            SfcsInboundRecordInfo siiModel = QueryEx<SfcsInboundRecordInfo>("SELECT * FROM SFCS_INBOUND_RECORD_INFO WHERE ID = :ID", new { ID = item.ID })?.FirstOrDefault();
                            if (siiModel == null) { throw new Exception("FINISHED_ERROR"); } else if (siiModel.STATUS.Trim() == "1") { throw new Exception("INBOUND_STATUS1_ERROR"); }
                            if (!widlist.Exists(m => m == item.WO_ID)) { widlist.Add(item.WO_ID); }
                            var resdata = await _dbConnection.ExecuteAsync(updateSql, new
                            {
                                item.ID,
                                item.INBOUND_QTY,
                                item.UPDATE_BY,

                            }, tran);
                        }
                    }

                    //删除
                    string deleteSql = @"DELETE FROM SFCS_INBOUND_RECORD_INFO WHERE ID=:ID ";
                    if (model.RemoveRecords != null && model.RemoveRecords.Count > 0)
                    {
                        foreach (var item in model.RemoveRecords)
                        {
                            SfcsInboundRecordInfo siiModel = QueryEx<SfcsInboundRecordInfo>("SELECT * FROM SFCS_INBOUND_RECORD_INFO WHERE ID = :ID", new { ID = item.ID })?.FirstOrDefault();
                            if (siiModel == null) { throw new Exception("FINISHED_ERROR"); } else if (siiModel.STATUS.Trim() == "1") { throw new Exception("INBOUND_STATUS1_ERROR"); }
                            var resdata = await _dbConnection.ExecuteAsync(deleteSql, new
                            {
                                item.ID
                            }, tran);
                        }
                    }

                    foreach (decimal wo_id in widlist)
                    {
                        SfcsWo swModel = QueryEx<SfcsWo>("SELECT * FROM SFCS_WO WHERE ID = :ID", new { ID = wo_id }).FirstOrDefault();
                        object inbound_qty = await _dbConnection.ExecuteScalarAsync("SELECT NVL(SUM(INBOUND_QTY),0) AS INBOUND_QTY FROM SFCS_INBOUND_RECORD_INFO WHERE WO_ID = :WO_ID", new { WO_ID = swModel.ID });
                        if (swModel.OUTPUT_QTY < Convert.ToDecimal(inbound_qty))
                        {
                            throw new Exception("I_QTY_ERROR");
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
        /// 获取完工入库工单列表
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public async Task<TableDataModel> LoadInboundWoList(SfcsWoRequestModel model)
        {
            //string sWhere = " WHERE SW.ACTUAL_START_DATE IS NOT NULL AND SW.OUTPUT_QTY > 0 ";
            string sWhere = " WHERE 1=1 AND SW.OUTPUT_QTY > 0 ";
            if (!model.PART_NO.IsNullOrEmpty())
            {
                sWhere += $"AND INSTR(SW.PART_NO, :PART_NO) > 0 ";
            }
            if (!model.WO_NO.IsNullOrEmpty())
            {
                sWhere += $"AND INSTR(SW.WO_NO, :WO_NO) > 0 ";
            }
            if (model.MODEL_ID > 0)
            {
                sWhere += $"AND INSTR(SW.MODEL_ID, :MODEL_ID) > 0 ";
            }

            string sQuery = @"SELECT ROWNUM AS ROWNO,SW.ID, SW.WO_NO, SW.PART_NO, SW.OEM_PN, ML.MODEL, SR.ROUTE_NAME, SW.TARGET_QTY, SW.INPUT_QTY, NVL(SW.OUTPUT_QTY,0) AS OUTPUT_QTY, SW.ACTUAL_START_DATE,NVL((SELECT SUM(SIRI.INBOUND_QTY) FROM SFCS_INBOUND_RECORD_INFO SIRI WHERE SIRI.WO_ID =SW.ID ),0) AS INBOUND_QTY FROM SFCS_WO SW LEFT JOIN SFCS_MODEL ML ON ML.ID=SW.MODEL_ID LEFT JOIN SFCS_ROUTES SR ON SW.ROUTE_ID = SR.ID ";
            string pagedSql = SQLBuilderClass.GetPagedSQL(sQuery, " SW.ACTUAL_START_DATE DESC", sWhere);
            var resdata = await _dbConnection.QueryAsync<object>(pagedSql, model);

            string sqlcnt = @"SELECT COUNT(0) FROM SFCS_WO SW LEFT JOIN SFCS_MODEL ML ON ML.ID=SW.MODEL_ID" + sWhere;

            int cnt = await _dbConnection.ExecuteScalarAsync<int>(sqlcnt, model);

            return new TableDataModel
            {
                count = cnt,
                data = resdata?.ToList(),
            };
        }

        /// <summary>
        /// 根据工单号获取完工入库的记录列表
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public async Task<TableDataModel> LoadInboundInfoListByWo(SfcsInboundRecordInfoRequestModel model)
        {
            string sWhere = " AND SIRI.WO_ID = :WO_ID ";

            if (!model.INBOUND_NO.IsNullOrEmpty())
            {
                sWhere += $"AND INSTR(SIRI.INBOUND_NO, :INBOUND_NO) > 0 ";
            }

            if (!model.FINISHED_NO.IsNullOrEmpty())
            {
                sWhere += $"AND INSTR(SIRI.FINISHED_NO, :FINISHED_NO) > 0 ";
            }

            string sql = @" SELECT ROWNUM AS ROWNO,T.*,(T.OUTPUT_QTY - T.I_QTY) AS MAX_QTY FROM (SELECT SIRI.*,SW.WO_NO,NVL(SW.OUTPUT_QTY,0) AS OUTPUT_QTY,NVL((SELECT SUM(SIRI.INBOUND_QTY) FROM SFCS_INBOUND_RECORD_INFO SIRI WHERE SIRI.WO_ID =SW.ID ),0) AS I_QTY FROM SFCS_INBOUND_RECORD_INFO SIRI,SFCS_WO SW WHERE SIRI.WO_ID = SW.ID {0})T ";
            string sQuery = String.Format(sql, sWhere);
            string pagedSql = SQLBuilderClass.GetPagedSQL(sQuery, " ID ASC", "");
            var resdata = await _dbConnection.QueryAsync<object>(pagedSql, model);

            string sqlcnt = @"SELECT COUNT(0) FROM SFCS_INBOUND_RECORD_INFO SIRI WHERE 1=1 " + sWhere;

            int cnt = await _dbConnection.ExecuteScalarAsync<int>(sqlcnt, model);

            return new TableDataModel
            {
                count = cnt,
                data = resdata?.ToList(),
            };
        }

    }
}