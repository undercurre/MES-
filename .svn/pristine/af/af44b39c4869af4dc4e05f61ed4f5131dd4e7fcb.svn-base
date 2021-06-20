/*
*┌──────────────────────────────────────────────────────────────┐
*│　描    述：接口实现                                                    
*│　作    者：嘉志科技                                            
*│　版    本：2.0    模板代码自动生成                                                
*│　创建时间：2020-04-23 16:57:11                             
*└──────────────────────────────────────────────────────────────┘
*┌──────────────────────────────────────────────────────────────┐
*│　命名空间： JZ.IMS.Repository.Oracle                                  
*│　类    名： SfcsTurninBatchHeaderRepository                                      
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
    public class SfcsTurninBatchHeaderRepository:BaseRepository<SfcsTurninBatchHeader,String>, ISfcsTurninBatchHeaderRepository
    {
        public SfcsTurninBatchHeaderRepository(IOptionsSnapshot<DbOption> options)
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
			string sql = "SELECT ENABLED FROM SFCS_TURNIN_BATCH_HEADER WHERE ID=:ID";
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
			string sql = "UPDATE SFCS_TURNIN_BATCH_HEADER set ENABLED=:ENABLED WHERE ID=:Id";
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
			string sql = "SELECT SFCS_TURNIN_BATCH_HEADER_SEQ.NEXTVAL MY_SEQ FROM DUAL";
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
			string sql = "select count(0) from SFCS_TURNIN_BATCH_HEADER where id = :id";
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
		public async Task<decimal> SaveDataByTrans(SfcsTurninBatchHeaderModel model)
		{
			int result = 1;
			ConnectionFactory.OpenConnection(_dbConnection);
			using (var tran = _dbConnection.BeginTransaction())
			{
				try
				{
					//新增
					string insertSql = @"insert into SFCS_TURNIN_BATCH_HEADER 
					(BATCH_NO,SUBINVENTORY_CODE,LOCATION,QUANTITY,BATCH_FLAG,PROCESS_FLAG,SLIP_NUMBER,FAIL_UNITS,CREATE_TIME,UPDATE_TIME,IO_FLAG,IO_REMARK,CREATED_BY) 
					VALUES (:BATCH_NO,:SUBINVENTORY_CODE,:LOCATION,:QUANTITY,:BATCH_FLAG,:PROCESS_FLAG,:SLIP_NUMBER,:FAIL_UNITS,:CREATE_TIME,:UPDATE_TIME,:IO_FLAG,:IO_REMARK,:CREATED_BY)";
					if (model.InsertRecords != null && model.InsertRecords.Count > 0)
					{
						foreach (var item in model.InsertRecords)
						{
							
							var resdata = await _dbConnection.ExecuteAsync(insertSql, new
							{
								
								item.BATCH_NO,
								item.SUBINVENTORY_CODE,
								item.LOCATION,
								item.QUANTITY,
								item.BATCH_FLAG,
								item.PROCESS_FLAG,
								item.SLIP_NUMBER,
								item.FAIL_UNITS,
								item.CREATE_TIME,
								item.UPDATE_TIME,
								item.IO_FLAG,
								item.IO_REMARK,
								item.CREATED_BY,
							}, tran);
						}
					}
					//更新
					string updateSql = @"Update SFCS_TURNIN_BATCH_HEADER set BATCH_NO=:BATCH_NO,SUBINVENTORY_CODE=:SUBINVENTORY_CODE,LOCATION=:LOCATION,QUANTITY=:QUANTITY,BATCH_FLAG=:BATCH_FLAG,PROCESS_FLAG=:PROCESS_FLAG,SLIP_NUMBER=:SLIP_NUMBER,FAIL_UNITS=:FAIL_UNITS,CREATE_TIME=:CREATE_TIME,UPDATE_TIME=:UPDATE_TIME,IO_FLAG=:IO_FLAG,IO_REMARK=:IO_REMARK,CREATED_BY=:CREATED_BY  
						where ID=:ID ";
					if (model.UpdateRecords != null && model.UpdateRecords.Count > 0)
					{
						foreach (var item in model.UpdateRecords)
						{
							var resdata = await _dbConnection.ExecuteAsync(updateSql, new
							{
								
								item.BATCH_NO,
								item.SUBINVENTORY_CODE,
								item.LOCATION,
								item.QUANTITY,
								item.BATCH_FLAG,
								item.PROCESS_FLAG,
								item.SLIP_NUMBER,
								item.FAIL_UNITS,
								item.CREATE_TIME,
								item.UPDATE_TIME,
								item.IO_FLAG,
								item.IO_REMARK,
								item.CREATED_BY,
							}, tran);
						}
					}
					//删除
					//string deleteSql = @"Delete from SFCS_TURNIN_BATCH_HEADER where ID=:ID ";
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