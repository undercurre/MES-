/*
*┌──────────────────────────────────────────────────────────────┐
*│　描    述：批次管理文件上传表接口实现                                                    
*│　作    者：嘉志科技                                            
*│　版    本：2.0    模板代码自动生成                                                
*│　创建时间：2020-08-17 17:11:24                             
*└──────────────────────────────────────────────────────────────┘
*┌──────────────────────────────────────────────────────────────┐
*│　命名空间： JZ.IMS.Repository.Oracle                                  
*│　类    名： MesBatchResourcesRepository                                      
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
    public class MesBatchResourcesRepository:BaseRepository<MesBatchResources,Decimal>, IMesBatchResourcesRepository
    {
        public MesBatchResourcesRepository(IOptionsSnapshot<DbOption> options)
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
			string sql = "SELECT ENABLED FROM MES_BATCH_RESOURCES WHERE ID=:ID";
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
			string sql = "UPDATE MES_BATCH_RESOURCES set ENABLED=:ENABLED WHERE ID=:Id";
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
			string sql = "SELECT MES_BATCH_RESOURCES_SEQ.NEXTVAL MY_SEQ FROM DUAL";
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
			string sql = "select count(0) from MES_BATCH_RESOURCES where id = :id";
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
		public async Task<decimal> SaveDataByTrans(MesBatchResourcesModel model)
		{
			int result = 1;
			ConnectionFactory.OpenConnection(_dbConnection);
			using (var tran = _dbConnection.BeginTransaction())
			{
				try
				{
					//新增
					string insertSql = @"insert into MES_BATCH_RESOURCES 
					(ID,BT_MANAGER_ID,LINE_NAME,RESOURCE_TYPE,RESOURCES_URL,RESOURCE_NAME,RESOURCE_SIZE,UPLOAD_USER,CREATE_TIME) 
					VALUES (:ID,:BT_MANAGER_ID,:LINE_NAME,:RESOURCE_TYPE,:RESOURCES_URL,:RESOURCE_NAME,:RESOURCE_SIZE,:UPLOAD_USER,:CREATE_TIME)";
					if (model.InsertRecords != null && model.InsertRecords.Count > 0)
					{
						foreach (var item in model.InsertRecords)
						{
							var newid = await GetSEQID();
							var resdata = await _dbConnection.ExecuteAsync(insertSql, new
							{
								ID = newid,
								item.BT_MANAGER_ID,
								item.LINE_NAME,
								item.RESOURCE_TYPE,
								item.RESOURCES_URL,
								item.RESOURCE_NAME,
								item.RESOURCE_SIZE,
								item.UPLOAD_USER,
								item.CREATE_TIME,
							}, tran);
						}
					}
					////更新
					//string updateSql = @"Update MES_BATCH_RESOURCES set BT_MANAGER_ID=:BT_MANAGER_ID,LINE_NAME=:LINE_NAME,RESOURCE_TYPE=:RESOURCE_TYPE,RESOURCES_URL=:RESOURCES_URL,RESOURCE_NAME=:RESOURCE_NAME,RESOURCE_SIZE=:RESOURCE_SIZE,UPLOAD_USER=:UPLOAD_USER,CREATE_TIME=:CREATE_TIME  
					//	where ID=:ID ";
					//if (model.UpdateRecords != null && model.UpdateRecords.Count > 0)
					//{
					//	foreach (var item in model.UpdateRecords)
					//	{
					//		var resdata = await _dbConnection.ExecuteAsync(updateSql, new
					//		{
					//			item.ID,
					//			item.BT_MANAGER_ID,
					//			item.LINE_NAME,
					//			item.RESOURCE_TYPE,
					//			item.RESOURCES_URL,
					//			item.RESOURCE_NAME,
					//			item.RESOURCE_SIZE,
					//			item.UPLOAD_USER,
					//			item.CREATE_TIME,
					//		}, tran);
					//	}
					//}
					////删除
					//string deleteSql = @"Delete from MES_BATCH_RESOURCES where ID=:ID ";
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