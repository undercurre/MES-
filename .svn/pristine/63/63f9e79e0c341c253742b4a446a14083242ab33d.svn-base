/*
*┌──────────────────────────────────────────────────────────────┐
*│　描    述：产品流水号操作记录表接口实现                                                    
*│　作    者：嘉志科技                                            
*│　版    本：2.0    模板代码自动生成                                                
*│　创建时间：2020-04-23 14:53:45                             
*└──────────────────────────────────────────────────────────────┘
*┌──────────────────────────────────────────────────────────────┐
*│　命名空间： JZ.IMS.Repository.Oracle                                  
*│　类    名： SfcsOperationHistoryRepository                                      
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

namespace JZ.IMS.Repository.Oracle
{
    public class SfcsOperationHistoryRepository:BaseRepository<SfcsOperationHistory,String>, ISfcsOperationHistoryRepository
    {
        public SfcsOperationHistoryRepository(IOptionsSnapshot<DbOption> options)
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
			string sql = "SELECT ENABLED FROM SFCS_OPERATION_HISTORY WHERE ID=:ID";
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
			string sql = "UPDATE SFCS_OPERATION_HISTORY set ENABLED=:ENABLED WHERE ID=:Id";
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
			string sql = "SELECT SFCS_OPERATION_HISTORY_SEQ.NEXTVAL MY_SEQ FROM DUAL";
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
			string sql = "select count(0) from SFCS_OPERATION_HISTORY where id = :id";
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
		public async Task<decimal> SaveDataByTrans(SfcsOperationHistoryModel model)
		{
			int result = 1;
			ConnectionFactory.OpenConnection(_dbConnection);
			using (var tran = _dbConnection.BeginTransaction())
			{
				try
				{
					//新增
					string insertSql = @"insert into SFCS_OPERATION_HISTORY 
					(SN_ID,OPERATION_ID,WO_ID,ROUTE_ID,SITE_OPERATION_ID,OPERATION_SITE_ID,OPERATOR,OPERATION_STATUS,OPERATION_TIME,VISIT_NUMBER) 
					VALUES (:SN_ID,:OPERATION_ID,:WO_ID,:ROUTE_ID,:SITE_OPERATION_ID,:OPERATION_SITE_ID,:OPERATOR,:OPERATION_STATUS,:OPERATION_TIME,:VISIT_NUMBER)";
					if (model.InsertRecords != null && model.InsertRecords.Count > 0)
					{
						foreach (var item in model.InsertRecords)
						{
							var newid = await GetSEQID();
							var resdata = await _dbConnection.ExecuteAsync(insertSql, new
							{
								ID = newid,
								item.SN_ID,
								item.OPERATION_ID,
								item.WO_ID,
								item.ROUTE_ID,
								item.SITE_OPERATION_ID,
								item.OPERATION_SITE_ID,
								item.OPERATOR,
								item.OPERATION_STATUS,
								item.OPERATION_TIME,
								item.VISIT_NUMBER,
							}, tran);
						}
					}
					//更新
					string updateSql = @"Update SFCS_OPERATION_HISTORY set SN_ID=:SN_ID,OPERATION_ID=:OPERATION_ID,WO_ID=:WO_ID,ROUTE_ID=:ROUTE_ID,SITE_OPERATION_ID=:SITE_OPERATION_ID,OPERATION_SITE_ID=:OPERATION_SITE_ID,OPERATOR=:OPERATOR,OPERATION_STATUS=:OPERATION_STATUS,OPERATION_TIME=:OPERATION_TIME,VISIT_NUMBER=:VISIT_NUMBER  
						where ID=:ID ";
					if (model.UpdateRecords != null && model.UpdateRecords.Count > 0)
					{
						foreach (var item in model.UpdateRecords)
						{
							var resdata = await _dbConnection.ExecuteAsync(updateSql, new
							{
								
								item.SN_ID,
								item.OPERATION_ID,
								item.WO_ID,
								item.ROUTE_ID,
								item.SITE_OPERATION_ID,
								item.OPERATION_SITE_ID,
								item.OPERATOR,
								item.OPERATION_STATUS,
								item.OPERATION_TIME,
								item.VISIT_NUMBER,
							}, tran);
						}
					}
					//删除
					//string deleteSql = @"Delete from SFCS_OPERATION_HISTORY where ID=:ID ";
					//if (model.RemoveRecords != null && model.RemoveRecords.Count > 0)
					//{
					//	foreach (var item in model.RemoveRecords)
					//	{
					//		var resdata = await _dbConnection.ExecuteAsync(deleteSql, new
					//		{
					//			item.ID
					//		}, tran);
					//	}
					//}

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