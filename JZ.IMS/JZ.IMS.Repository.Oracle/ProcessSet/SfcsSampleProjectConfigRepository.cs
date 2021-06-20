/*
*┌──────────────────────────────────────────────────────────────┐
*│　描    述：接口实现                                                    
*│　作    者：嘉志科技                                            
*│　版    本：2.0    模板代码自动生成                                                
*│　创建时间：2020-04-15 13:39:33                             
*└──────────────────────────────────────────────────────────────┘
*┌──────────────────────────────────────────────────────────────┐
*│　命名空间： JZ.IMS.Repository.Oracle                                  
*│　类    名： SfcsSampleProjectConfigRepository                                      
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
    public class SfcsSampleProjectConfigRepository:BaseRepository<SfcsSampleProjectConfig,String>, ISfcsSampleProjectConfigRepository
    {
        public SfcsSampleProjectConfigRepository(IOptionsSnapshot<DbOption> options)
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
			string sql = "SELECT ENABLED FROM SFCS_SAMPLE_PROJECT_CONFIG WHERE ID=:ID";
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
			string sql = "UPDATE SFCS_SAMPLE_PROJECT_CONFIG set ENABLED=:ENABLED WHERE ID=:Id";
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
			string sql = "SELECT SFCS_SAMPLE_PROJECT_CONFIG_SEQ.NEXTVAL MY_SEQ FROM DUAL";
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
			string sql = "select count(0) from SFCS_SAMPLE_PROJECT_CONFIG where id = :id";
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
		public async Task<decimal> SaveDataByTrans(SfcsSampleProjectConfigModel model)
		{
			int result = 1;
			ConnectionFactory.OpenConnection(_dbConnection);
			using (var tran = _dbConnection.BeginTransaction())
			{
				try
				{
					//新增
					string insertSql = @"insert into SFCS_SAMPLE_PROJECT_CONFIG 
					(ID,PROJECT_ID,SAMPLE_RATIO,SAMPLE_RATIO_COUNT,UP_RATIO,UP_RATIO_LIMIT_COUNT,DOWN_RATIO,DOWN_RATIO_LIMIT_COUNT,ORDER_NO,SAMPLE_COUNT) 
					VALUES (:ID,:PROJECT_ID,:SAMPLE_RATIO,:SAMPLE_RATIO_COUNT,:UP_RATIO,:UP_RATIO_LIMIT_COUNT,:DOWN_RATIO,:DOWN_RATIO_LIMIT_COUNT,:ORDER_NO,:SAMPLE_COUNT)";
					if (model.InsertRecords != null && model.InsertRecords.Count > 0)
					{
						foreach (var item in model.InsertRecords)
						{
							var newid = await Get_MES_SEQ_ID();
							var resdata = await _dbConnection.ExecuteAsync(insertSql, new
							{
								ID = newid,
								item.PROJECT_ID,
								item.SAMPLE_RATIO,
								item.SAMPLE_RATIO_COUNT,
								item.UP_RATIO,
								item.UP_RATIO_LIMIT_COUNT,
								item.DOWN_RATIO,
								item.DOWN_RATIO_LIMIT_COUNT,
								item.ORDER_NO,
								item.SAMPLE_COUNT,							}, tran);
						}
					}
					//更新
					string updateSql = @"Update SFCS_SAMPLE_PROJECT_CONFIG set PROJECT_ID=:PROJECT_ID,SAMPLE_RATIO=:SAMPLE_RATIO,SAMPLE_RATIO_COUNT=:SAMPLE_RATIO_COUNT,UP_RATIO=:UP_RATIO,UP_RATIO_LIMIT_COUNT=:UP_RATIO_LIMIT_COUNT,DOWN_RATIO=:DOWN_RATIO,DOWN_RATIO_LIMIT_COUNT=:DOWN_RATIO_LIMIT_COUNT,ORDER_NO=:ORDER_NO,SAMPLE_COUNT=:SAMPLE_COUNT  
						where ID=:ID ";
					if (model.UpdateRecords != null && model.UpdateRecords.Count > 0)
					{
						foreach (var item in model.UpdateRecords)
						{
							var resdata = await _dbConnection.ExecuteAsync(updateSql, new
							{
								item.ID,
								item.PROJECT_ID,
								item.SAMPLE_RATIO,
								item.SAMPLE_RATIO_COUNT,
								item.UP_RATIO,
								item.UP_RATIO_LIMIT_COUNT,
								item.DOWN_RATIO,
								item.DOWN_RATIO_LIMIT_COUNT,
								item.ORDER_NO,
								item.SAMPLE_COUNT,							}, tran);
						}
					}
					//删除
					string deleteSql = @"Delete from SFCS_SAMPLE_PROJECT_CONFIG where ID=:ID ";
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