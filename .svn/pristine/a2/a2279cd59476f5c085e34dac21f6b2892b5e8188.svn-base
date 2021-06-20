/*
*┌──────────────────────────────────────────────────────────────┐
*│　描    述：接口实现                                                    
*│　作    者：嘉志科技                                            
*│　版    本：2.0    模板代码自动生成                                                
*│　创建时间：2020-09-14 09:32:34                             
*└──────────────────────────────────────────────────────────────┘
*┌──────────────────────────────────────────────────────────────┐
*│　命名空间： JZ.IMS.Repository.Oracle                                  
*│　类    名： SysProjApiMstRepository                                      
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
    public class SysProjApiMstRepository:BaseRepository<SysProjApiMst,Decimal>, ISysProjApiMstRepository
    {
        public SysProjApiMstRepository(IOptionsSnapshot<DbOption> options)
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
			string sql = "SELECT ENABLED FROM SYS_PROJ_API_MST WHERE ID=:ID";
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
			string sql = "UPDATE SYS_PROJ_API_MST set ENABLED=:ENABLED WHERE ID=:Id";
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
			string sql = "SELECT SYS_PROJ_API_MST_SEQ.NEXTVAL MY_SEQ FROM DUAL";
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
			string sql = "select count(0) from SYS_PROJ_API_MST where id = :id";
			object result = await _dbConnection.ExecuteScalarAsync(sql, new
			{
				id
			});

			return (Convert.ToInt32(result) > 0);
		}

		/// <summary>
		/// 保存参数数据，把旧的全删了，重新添加
		/// </summary>
		/// <param name="tran"></param>
		/// <param name="Mst_ID"></param>
		/// <param name="records">参数列表</param>
		/// <param name="addParam">保存参数</param>
		/// <returns></returns>
		public async Task<decimal> SaveParamsByTrans(System.Data.IDbTransaction tran, decimal MST_ID, List<SysProjApiParm> parms, bool addParams)
		{
			var result = 1;
			//删除旧数据
			string deleteSql = @"Delete from SYS_PROJ_API_PARM where MST_ID=:MST_ID ";
			var resdata = await _dbConnection.ExecuteAsync(deleteSql, new
			{
				MST_ID
			}, tran);
			//新增参数数据
			if (addParams)
			{
				var insertSql = @"insert into SYS_PROJ_API_PARM 
							(PARM_NAME,DESCRIPTION,MST_ID,REQUIRED)
							VALUES (:PARM_NAME,:DESCRIPTION,:MST_ID,REQUIRED)";
				foreach (var item in parms)
				{
					var resData = await _dbConnection.ExecuteAsync(insertSql, new
					{
						item.PARM_NAME,
						item.DESCRIPTION,
						item.REQUIRED,
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
		public async Task<List<decimal>> SaveDataByTrans(SysProjApiMstModel model)
		{
			List<decimal> ids = new List<decimal>();
			ConnectionFactory.OpenConnection(_dbConnection);
			using (var tran = _dbConnection.BeginTransaction())
			{
				try
				{
					//新增
					string insertSql = @"insert into SYS_PROJ_API_MST 
					(ID,RID,PROJ_VERTION,PROJ,ACTION,DESCRIPTION,URL,CREATE_USER,CREATE_TIME,UPDATE_USER,UPDATE_TIME,ENDABLED) 
					VALUES (:ID,:RID,:PROJ_VERTION,:PROJ,:ACTION,:DESCRIPTION,:URL,:CREATE_USER,:CREATE_TIME,:UPDATE_USER,:UPDATE_TIME,:ENDABLED)";
					if (model.InsertRecords != null && model.InsertRecords.Count > 0)
					{
						foreach (var item in model.InsertRecords)
						{
							var newid = await GetSEQID();
							var resdata = await _dbConnection.ExecuteAsync(insertSql, new
							{
								ID = newid,
								item.RID,
								item.PROJ_VERTION,
								item.PROJ,
								item.ACTION,
								item.DESCRIPTION,
								item.URL,
								item.CREATE_USER,
								item.CREATE_TIME,
								item.UPDATE_USER,
								item.UPDATE_TIME,								item.ENABLED,
							}, tran);
                            var parmData = await SaveParamsByTrans(tran, newid, item.parms, true);
							ids.Add(newid);
						}
					}
					//更新
					string updateSql = @"Update SYS_PROJ_API_MST set RID=:RID,PROJ_VERTION=:PROJ_VERTION,PROJ=:PROJ,ACTION=:ACTION,DESCRIPTION=:DESCRIPTION,URL=:URL,CREATE_USER=:CREATE_USER,CREATE_TIME=:CREATE_TIME,UPDATE_USER=:UPDATE_USER,UPDATE_TIME=:UPDATE_TIME,ENABLED=:ENABLED 
						where ID=:ID ";
					if (model.UpdateRecords != null && model.UpdateRecords.Count > 0)
					{
						foreach (var item in model.UpdateRecords)
						{
							var resdata = await _dbConnection.ExecuteAsync(updateSql, new
							{
								item.ID,
								item.RID,
								item.PROJ_VERTION,
								item.PROJ,
								item.ACTION,
								item.DESCRIPTION,
								item.URL,
								item.CREATE_USER,
								item.CREATE_TIME,
								item.UPDATE_USER,
								item.UPDATE_TIME,								item.ENABLED,
							}, tran);
                            var parmData = await SaveParamsByTrans(tran, item.ID, item.parms, true);
						}
						ids = model.UpdateRecords.Select(p => p.ID).ToList();
					}
					//删除
					string deleteSql = @"Delete from SYS_PROJ_API_MST where ID=:ID ";
					if (model.RemoveRecords != null && model.RemoveRecords.Count > 0)
					{
						foreach (var item in model.RemoveRecords)
						{
							var resdata = await _dbConnection.ExecuteAsync(deleteSql, new
							{
								item.ID
							}, tran);
                            var parmData = await SaveParamsByTrans(tran, item.ID, item.parms, true);
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
        /// 执行SQL
        /// </summary>
        /// <param name="sql">执行的sql</param>
        /// <returns></returns>
        public async Task<string> CreateUrl(SysProjApiMstExecuteModel model,bool addParm)
        {
            //获取ReportMST对象
            var mstItem = await GetMstItem(model.MST_RID);
			//获取参数对象并对应赋值，并筛选需用的参数
			var parmNames = model.parms.Where(n => !n.PARM_VALUE.IsNullOrEmpty()).Select(n => n.PARM_NAME).ToArray();
			var reportParms = (await GetParams(model.MST_RID)).ToList();
			reportParms = reportParms.Where(p => parmNames.Contains(p.PARM_NAME)).ToList();

			foreach (var item in reportParms)
            {
                var parm = model.parms.FirstOrDefault(p => p.PARM_NAME == item.PARM_NAME);
                if (parm != null) item.PARM_VALUE = parm.PARM_VALUE;
            }
			//加条件参数
			var url = mstItem.URL;
			if (addParm) {
				for (int i = 0; i < reportParms.Count; i++)
				{
					url += (i == 0 ? "?" : "&");
					url += $"{reportParms[i].PARM_NAME}={reportParms[i].PARM_VALUE}";
				}
			}

			return url;
        }

		/// <summary>
		/// 获取Report_MST的单个数据
		/// </summary>
		/// <param name="id"></param>
		/// <returns></returns>
		public async Task<SysProjApiMst> GetMstItemByUrl(string LOCAL_URL, string PROJ)
		{
			var sql = $"SELECT * FROM SYS_PROJ_API_MST WHERE upper(LOCAL_URL) = upper('{LOCAL_URL}') AND Proj= '{PROJ}'";
			var result = await _dbConnection.QueryFirstOrDefaultAsync<SysProjApiMst>(sql);
			return result;
		}

		/// <summary>
		/// 获取Report_MST的单个数据
		/// </summary>
		/// <param name="id"></param>
		/// <returns></returns>
		public async Task<SysProjApiMst> GetMstItem(decimal RID)
		{
			var sql = "SELECT * FROM SYS_PROJ_API_MST WHERE RID = " + RID;
			var result = await _dbConnection.QueryFirstAsync<SysProjApiMst>(sql);
			return result;
		}

		/// <summary>
		/// 获取参数列表
		/// </summary>
		/// <param name="MST_ID">REPORT_MST的id</param>
		/// <returns></returns>
		public async Task<List<SysProjApiParm>> GetParams(decimal MST_RID)
		{
			string sql = "SELECT * FROM SYS_PROJ_API_PARM WHERE MST_RID = " + MST_RID;
			var result = (await _dbConnection.QueryAsync<SysProjApiParm>(sql)).ToList();
			return result;
		}
	}
}