/*
*┌──────────────────────────────────────────────────────────────┐
*│　描    述：报表自定义SQL-语句表接口实现                                                    
*│　作    者：嘉志科技                                            
*│　版    本：2.0    模板代码自动生成                                                
*│　创建时间：2020-07-22 16:19:07                             
*└──────────────────────────────────────────────────────────────┘
*┌──────────────────────────────────────────────────────────────┐
*│　命名空间： JZ.IMS.Repository.Oracle                                  
*│　类    名： ReportMstRepository                                      
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
using System.Runtime.InteropServices;
using JZ.IMS.Core.Extensions;

namespace JZ.IMS.Repository.Oracle
{
    public class ReportMstRepository:BaseRepository<ReportMst,Decimal>, IReportMstRepository
    {
        public ReportMstRepository(IOptionsSnapshot<DbOption> options)
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
			string sql = "SELECT ENABLED FROM REPORT_MST WHERE ID=:ID";
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
			string sql = "UPDATE REPORT_MST set ENABLED=:ENABLED WHERE ID=:Id";
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
			string sql = "SELECT REPORT_MST_SEQ.NEXTVAL MY_SEQ FROM DUAL";
			var result = await _dbConnection.ExecuteScalarAsync(sql);
			return (decimal)result;
		}

		/// <summary>
		/// 保存参数数据，把旧的全删了，重新添加
		/// </summary>
		/// <param name="tran"></param>
		/// <param name="Mst_ID"></param>
		/// <param name="records">参数列表</param>
		/// <param name="addParam">保存参数</param>
		/// <returns></returns>
		public async Task<decimal> SaveParamsByTrans(System.Data.IDbTransaction tran, decimal MST_ID, List<ReportParam> parms,bool addParams) {
			var result = 1;
			//删除旧数据
			string deleteSql = @"Delete from REPORT_PARAM where MST_ID=:MST_ID ";
			var resdata = await _dbConnection.ExecuteAsync(deleteSql, new
			{
				MST_ID
			}, tran);
			//新增参数数据
			if (addParams) {
				var insertSql = @"insert into REPORT_PARAM 
							(PARAM_NAME,DATA_TYPE,PARAM_VALUE,REMARK,MST_ID)
							VALUES (:PARAM_NAME,:DATA_TYPE,:PARAM_VALUE,:REMARK,:MST_ID)";
				foreach (var item in parms)
				{
					var resData = await _dbConnection.ExecuteAsync(insertSql, new
					{
						item.PARAM_NAME,
						item.DATA_TYPE,
						item.PARAM_VALUE,
						item.REMARK,
						MST_ID,
					}, tran);

				}
			}
			
			return result;
		}

		/// <summary>
		/// 保存数据
		/// </summary>
		/// <param name="model"></param>
		/// <returns></returns>
		public async Task<List<decimal>> SaveDataByTrans(ReportMstModel model)
		{
			List<decimal> ids = new List<decimal>();
			ConnectionFactory.OpenConnection(_dbConnection);
			using (var tran = _dbConnection.BeginTransaction())
			{
				try
				{
					//新增
					string insertSql = @"insert into REPORT_MST 
					(ID,SQL,CREATE_DATE,ACTION_NAME,REMARK,ENABLED,COLUMN_CONFIG,TOOLBAR_CONFIG) 
					VALUES (:ID,:SQL,:CREATE_DATE,:ACTION_NAME,:REMARK,:ENABLED,:COLUMN_CONFIG,:TOOLBAR_CONFIG)";
					if (model.InsertRecords != null && model.InsertRecords.Count > 0)
					{
						foreach (var item in model.InsertRecords)
						{
							var newid = await GetSEQID();
							var resdata = await _dbConnection.ExecuteAsync(insertSql, new
							{
								ID = newid,
								item.SQL,
								CREATE_DATE = DateTime.Now,
								item.ACTION_NAME,
								item.REMARK,
								item.ENABLED,								item.COLUMN_CONFIG,								item.TOOLBAR_CONFIG
							}, tran);
							var parmData = await SaveParamsByTrans(tran,newid,item.parms,true);
							ids.Add(newid);
						}

					}
					//更新
					string updateSql = @"Update REPORT_MST set SQL=:SQL,COLUMN_CONFIG=:COLUMN_CONFIG,TOOLBAR_CONFIG=:TOOLBAR_CONFIG,LAST_EDIT_DATE=:LAST_EDIT_DATE,ACTION_NAME=:ACTION_NAME,REMARK=:REMARK,ENABLED=:ENABLED  
						where ID=:ID ";
					if (model.UpdateRecords != null && model.UpdateRecords.Count > 0)
					{
						foreach (var item in model.UpdateRecords)
						{
							var resdata = await _dbConnection.ExecuteAsync(updateSql, new
							{
								item.ID,
								item.SQL,
								LAST_EDIT_DATE = DateTime.Now,
								item.ACTION_NAME,
								item.REMARK,
								item.ENABLED,
								item.COLUMN_CONFIG,								item.TOOLBAR_CONFIG
							}, tran);
							var parmData = await SaveParamsByTrans(tran, item.ID, item.parms,true);
						}
						ids = model.UpdateRecords.Select(p => p.ID).ToList();
					}
					//删除
					string deleteSql = @"Delete from REPORT_MST where ID=:ID ";
					if (model.RemoveRecords != null && model.RemoveRecords.Count > 0)
					{
						foreach (var item in model.RemoveRecords)
						{
							var resdata = await _dbConnection.ExecuteAsync(deleteSql, new
							{
								item.ID
							}, tran);
							var parmData = await SaveParamsByTrans(tran, item.ID, item.parms,false);
						}
						ids = model.RemoveRecords.Select(p => p.ID).ToList();
					}

					tran.Commit();
				}
				catch (Exception ex)
				{
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
			return ids;
		}

		/// <summary>
		/// 获取参数列表
		/// </summary>
		/// <param name="MST_ID">REPORT_MST的id</param>
		/// <returns></returns>
		public async Task<List<ReportParam>> GetParams(decimal MST_ID)
        {
            string sql = "SELECT * FROM REPORT_PARAM WHERE MST_ID = " + MST_ID;
            var result = (await _dbConnection.QueryAsync<ReportParam>(sql)).ToList();
            return result;
        }

		/// <summary>
		/// 执行SQL
		/// </summary>
		/// <param name="sql">执行的sql</param>
		/// <returns></returns>
		public async Task<TableDataModel> QuerySql(ExecuteSqlModel model,ReportMst mst)
		{
			//获取ReportMST对象
			var reportMsgItem = mst;
			//获取参数对象并对应赋值，并筛选需用的参数
			var reportParms = (await GetParams(model.Mst_ID))
				.Where(p => model.parms.Where(n => !n.PARAM_VALUE.IsNullOrEmpty()).Select(n => n.PARAM_NAME).ToArray().Contains(p.PARAM_NAME)).ToList();
			foreach (var item in reportParms)
			{
				var parm = model.parms.FirstOrDefault(p => p.PARAM_NAME == item.PARAM_NAME && p.MST_ID == item.MST_ID);
				if (parm != null) item.PARAM_VALUE = parm.PARAM_VALUE;
			}
			//加条件参数
			var conditions = "";
			DynamicParameters pars = new DynamicParameters();
			pars.Add("Key",model.Key);
			pars.Add("Limit", model.Limit);
			pars.Add("Page", model.Page);
			for (int i = 0; i < reportParms.Count(); i++)
			{
				var newStr = reportParms[i].Action_Type == (decimal)Param_ActionType.Equal ? $" AND {reportParms[i].PARAM_NAME} = :PARAM_VALUE{i} " :
							 reportParms[i].Action_Type == (decimal)Param_ActionType.Larger ? $" AND {reportParms[i].PARAM_NAME} > :PARAM_VALUE{i} " :
							 reportParms[i].Action_Type == (decimal)Param_ActionType.Larger_Equal ? $" AND {reportParms[i].PARAM_NAME} >= :PARAM_VALUE{i} " :
							 reportParms[i].Action_Type == (decimal)Param_ActionType.Smaller ? $" AND {reportParms[i].PARAM_NAME} < :PARAM_VALUE{i} " :
							 reportParms[i].Action_Type == (decimal)Param_ActionType.Smaller_Equal ? $" AND {reportParms[i].PARAM_NAME} <= :PARAM_VALUE{i} " :
							 reportParms[i].Action_Type == (decimal)Param_ActionType.Like ? $" AND INSTR({reportParms[i].PARAM_NAME},:PARAM_VALUE{i}) > 0 " : "";

				if (!newStr.IsNullOrEmpty())
				{
					pars.Add($"PARAM_VALUE{i}", reportParms[i].PARAM_VALUE);
					conditions += newStr;
				}
			}
			//定义的SQL
			var sql = reportMsgItem.SQL.ReplaceWhitespace().ToUpper().Contains("ROWNUMASROWNO") ?
				reportMsgItem.SQL :
				$"SELECT ROWNUM AS ROWNO ,reportSQL.* FROM ({reportMsgItem.SQL}) reportSQL";
			sql += " WHERE 1=1 ";
			var pageSql = SQLBuilderClass.GetPagedSQL(sql,conditions);
			var resdata = await _dbConnection.QueryAsync<object>(pageSql, pars);

			var sqlcnt = $"SELECT COUNT(1) FROM ({reportMsgItem.SQL})";
			int cnt = await _dbConnection.ExecuteScalarAsync<int>(sqlcnt, model);

			return new TableDataModel
			{
				count = cnt,
				data = resdata?.ToList(),
			};
		}

		/// <summary>
		/// 获取Report_MST的单个数据
		/// </summary>
		/// <param name="id"></param>
		/// <returns></returns>
        public async Task<ReportMst> GetReportMstItem(decimal id)
        {
			var sql = "SELECT * FROM REPORT_MST WHERE ID = " + id;
			var result = await _dbConnection.QueryFirstAsync<ReportMst>(sql);
			return result;
        }

	}
}