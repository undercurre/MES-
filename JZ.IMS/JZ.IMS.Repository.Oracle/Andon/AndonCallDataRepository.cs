/*
*┌──────────────────────────────────────────────────────────────┐
*│　描    述：异常呼叫记录数据表接口实现                                                    
*│　作    者：嘉志科技                                            
*│　版    本：2.0    模板代码自动生成                                                
*│　创建时间：2020-08-20 10:24:27                             
*└──────────────────────────────────────────────────────────────┘
*┌──────────────────────────────────────────────────────────────┐
*│　命名空间： JZ.IMS.Repository.Oracle                                  
*│　类    名： AndonCallDataRepository                                      
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
    public class AndonCallDataRepository:BaseRepository<AndonCallData,Decimal>, IAndonCallDataRepository
    {
        public AndonCallDataRepository(IOptionsSnapshot<DbOption> options)
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
			string sql = "SELECT ENABLED FROM ANDON_CALL_DATA WHERE ID=:ID";
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
			string sql = "UPDATE ANDON_CALL_DATA set ENABLED=:ENABLED WHERE ID=:Id";
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
			string sql = "SELECT ANDON_CALL_DATA_SEQ.NEXTVAL MY_SEQ FROM DUAL";
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
			string sql = "select count(0) from ANDON_CALL_DATA where id = :id";
			object result = await _dbConnection.ExecuteScalarAsync(sql, new
			{
				id
			});

			return (Convert.ToInt32(result) > 0);
		}
        /// <summary>
        /// 检查呼叫数据是否存在
        /// </summary>
        /// <param name="callNo"></param>
        /// <returns></returns>
        public async Task<bool> ItemIsByCallNo(String callNo)
        {
            string sql = "select count(0) from ANDON_CALL_DATA where CALL_NO = :CALL_NO";
            object result = await _dbConnection.ExecuteScalarAsync(sql, new
            {
                CALL_NO = callNo
            });

            return (Convert.ToInt32(result) > 0);
        }

		/// <summary>
		/// 保存数据
		/// </summary>
		/// <param name="model"></param>
		/// <returns></returns>
		public async Task<decimal> SaveDataByTrans(AndonCallDataModel model)
		{
			int result = 1;
			ConnectionFactory.OpenConnection(_dbConnection);
			using (var tran = _dbConnection.BeginTransaction())
			{
				try
				{
					//新增
					string insertSql = @"insert into ANDON_CALL_DATA 
					(ID,CALL_NO,CALL_CONTENT_ID,LINE_ID,LINE_NAME,OPERATION_ID,OPERATION_NAME,OPERATION_SITE_ID,OPERATION_SITE_NAME,OPERATOR,CREATE_TIME,WO_NO,PART_NO,ROUTE_ID,ROUTE_NAME,STANDARD_DATA,MIN_STANDARD_DATA,MAX_STANDARD_DATA,ACTUAL_DATA,MIN_ACTUAL_DATA,MAX_ACTUAL_DATA,STATUS) 
					VALUES (:ID,:CALL_NO,:CALL_CONTENT_ID,:LINE_ID,:LINE_NAME,:OPERATION_ID,:OPERATION_NAME,:OPERATION_SITE_ID,:OPERATION_SITE_NAME,:OPERATOR,:CREATE_TIME,:WO_NO,:PART_NO,:ROUTE_ID,:ROUTE_NAME,:STANDARD_DATA,:MIN_STANDARD_DATA,:MAX_STANDARD_DATA,:ACTUAL_DATA,:MIN_ACTUAL_DATA,:MAX_ACTUAL_DATA,:STATUS)";
					if (model.InsertRecords != null && model.InsertRecords.Count > 0)
					{
						foreach (var item in model.InsertRecords)
						{
							var newid = await GetSEQID();
							var resdata = await _dbConnection.ExecuteAsync(insertSql, new
							{
								ID = newid,
								item.CALL_NO,
								item.CALL_CONTENT_ID,
								item.LINE_ID,
								item.LINE_NAME,
								item.OPERATION_ID,
								item.OPERATION_NAME,
								item.OPERATION_SITE_ID,
								item.OPERATION_SITE_NAME,
								item.OPERATOR,
								item.CREATE_TIME,
								item.WO_NO,
								item.PART_NO,
								item.ROUTE_ID,
								item.ROUTE_NAME,
								item.STANDARD_DATA,
								item.MIN_STANDARD_DATA,
								item.MAX_STANDARD_DATA,
								item.ACTUAL_DATA,
								item.MIN_ACTUAL_DATA,
								item.MAX_ACTUAL_DATA,
								item.STATUS,
							}, tran);
						}
					}
					//更新
					string updateSql = @"Update ANDON_CALL_DATA set CALL_NO=:CALL_NO,CALL_CONTENT_ID=:CALL_CONTENT_ID,LINE_ID=:LINE_ID,LINE_NAME=:LINE_NAME,OPERATION_ID=:OPERATION_ID,OPERATION_NAME=:OPERATION_NAME,OPERATION_SITE_ID=:OPERATION_SITE_ID,OPERATION_SITE_NAME=:OPERATION_SITE_NAME,OPERATOR=:OPERATOR,CREATE_TIME=:CREATE_TIME,WO_NO=:WO_NO,PART_NO=:PART_NO,ROUTE_ID=:ROUTE_ID,ROUTE_NAME=:ROUTE_NAME,STANDARD_DATA=:STANDARD_DATA,MIN_STANDARD_DATA=:MIN_STANDARD_DATA,MAX_STANDARD_DATA=:MAX_STANDARD_DATA,ACTUAL_DATA=:ACTUAL_DATA,MIN_ACTUAL_DATA=:MIN_ACTUAL_DATA,MAX_ACTUAL_DATA=:MAX_ACTUAL_DATA,STATUS=:STATUS  
						where ID=:ID ";
					if (model.UpdateRecords != null && model.UpdateRecords.Count > 0)
					{
						foreach (var item in model.UpdateRecords)
						{
							var resdata = await _dbConnection.ExecuteAsync(updateSql, new
							{
								item.ID,
								item.CALL_NO,
								item.CALL_CONTENT_ID,
								item.LINE_ID,
								item.LINE_NAME,
								item.OPERATION_ID,
								item.OPERATION_NAME,
								item.OPERATION_SITE_ID,
								item.OPERATION_SITE_NAME,
								item.OPERATOR,
								item.CREATE_TIME,
								item.WO_NO,
								item.PART_NO,
								item.ROUTE_ID,
								item.ROUTE_NAME,
								item.STANDARD_DATA,
								item.MIN_STANDARD_DATA,
								item.MAX_STANDARD_DATA,
								item.ACTUAL_DATA,
								item.MIN_ACTUAL_DATA,
								item.MAX_ACTUAL_DATA,
								item.STATUS,
							}, tran);
						}
					}
					//删除
					string deleteSql = @"Delete from ANDON_CALL_DATA where ID=:ID ";
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
    }
}