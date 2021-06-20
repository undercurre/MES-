/*
*┌──────────────────────────────────────────────────────────────┐
*│　描    述：烘烤标准表接口实现                                                    
*│　作    者：嘉志科技                                            
*│　版    本：2.0    模板代码自动生成                                                
*│　创建时间：2020-06-15 10:44:32                             
*└──────────────────────────────────────────────────────────────┘
*┌──────────────────────────────────────────────────────────────┐
*│　命名空间： JZ.IMS.Repository.Oracle                                  
*│　类    名： SmtMsdBakeRuleRepository                                      
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
    public class SmtMsdBakeRuleRepository:BaseRepository<SmtMsdBakeRule,Decimal>, ISmtMsdBakeRuleRepository
    {
        public SmtMsdBakeRuleRepository(IOptionsSnapshot<DbOption> options)
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
			string sql = "SELECT ENABLED FROM SMT_MSD_BAKE_RULE WHERE ID=:ID";
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
			string sql = "UPDATE SMT_MSD_BAKE_RULE set ENABLED=:ENABLED WHERE ID=:Id";
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
			string sql = "SELECT SMT_MSD_BAKE_RULE_SEQ.NEXTVAL MY_SEQ FROM DUAL";
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
			string sql = "select count(0) from SMT_MSD_BAKE_RULE where id = :id";
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
		public async Task<decimal> SaveDataByTrans(SmtMsdBakeRuleModel model)
		{
			int result = 1;
			ConnectionFactory.OpenConnection(_dbConnection);
			using (var tran = _dbConnection.BeginTransaction())
			{
				try
				{
					//新增
					string insertSql = @"insert into SMT_MSD_BAKE_RULE 
					(ID,LEVEL_CODE,MIN_THICKNESS,MAX_THICKNESS,MIN_OPEN_TEMPERATURE,MAX_OPEN_TEMPERATURE,MIN_OPEN_HUMIDITY,MAX_OPEN_HUMIDITY,BAKE_TEMPERATURE,BAKE_HUMIDITY,BAKE_TIME,ENABLED,CLEAR_OPEN_TIME,MIN_OVER_TIME,MAX_OVER_TIME) 
					VALUES (:ID,:LEVEL_CODE,:MIN_THICKNESS,:MAX_THICKNESS,:MIN_OPEN_TEMPERATURE,:MAX_OPEN_TEMPERATURE,:MIN_OPEN_HUMIDITY,:MAX_OPEN_HUMIDITY,:BAKE_TEMPERATURE,:BAKE_HUMIDITY,:BAKE_TIME,:ENABLED,:CLEAR_OPEN_TIME,:MIN_OVER_TIME,:MAX_OVER_TIME)";
					if (model.InsertRecords != null && model.InsertRecords.Count > 0)
					{
						foreach (var item in model.InsertRecords)
						{
							var newid = await GetSEQ_ID();
							var resdata = await _dbConnection.ExecuteAsync(insertSql, new
							{
								ID = newid,
								item.LEVEL_CODE,
								item.MIN_THICKNESS,
								item.MAX_THICKNESS,
								item.MIN_OPEN_TEMPERATURE,
								item.MAX_OPEN_TEMPERATURE,
								item.MIN_OPEN_HUMIDITY,
								item.MAX_OPEN_HUMIDITY,
								item.BAKE_TEMPERATURE,
								item.BAKE_HUMIDITY,
								item.BAKE_TIME,
								item.ENABLED,
								item.CLEAR_OPEN_TIME,
								item.MIN_OVER_TIME,
								item.MAX_OVER_TIME,
							}, tran);
						}
					}
					//更新
					string updateSql = @"Update SMT_MSD_BAKE_RULE set LEVEL_CODE=:LEVEL_CODE,MIN_THICKNESS=:MIN_THICKNESS,MAX_THICKNESS=:MAX_THICKNESS,MIN_OPEN_TEMPERATURE=:MIN_OPEN_TEMPERATURE,MAX_OPEN_TEMPERATURE=:MAX_OPEN_TEMPERATURE,MIN_OPEN_HUMIDITY=:MIN_OPEN_HUMIDITY,MAX_OPEN_HUMIDITY=:MAX_OPEN_HUMIDITY,BAKE_TEMPERATURE=:BAKE_TEMPERATURE,BAKE_HUMIDITY=:BAKE_HUMIDITY,BAKE_TIME=:BAKE_TIME,ENABLED=:ENABLED,CLEAR_OPEN_TIME=:CLEAR_OPEN_TIME,MIN_OVER_TIME=:MIN_OVER_TIME,MAX_OVER_TIME=:MAX_OVER_TIME  
						where ID=:ID ";
					if (model.UpdateRecords != null && model.UpdateRecords.Count > 0)
					{
						foreach (var item in model.UpdateRecords)
						{
							var resdata = await _dbConnection.ExecuteAsync(updateSql, new
							{
								item.ID,
								item.LEVEL_CODE,
								item.MIN_THICKNESS,
								item.MAX_THICKNESS,
								item.MIN_OPEN_TEMPERATURE,
								item.MAX_OPEN_TEMPERATURE,
								item.MIN_OPEN_HUMIDITY,
								item.MAX_OPEN_HUMIDITY,
								item.BAKE_TEMPERATURE,
								item.BAKE_HUMIDITY,
								item.BAKE_TIME,
								item.ENABLED,
								item.CLEAR_OPEN_TIME,
								item.MIN_OVER_TIME,
								item.MAX_OVER_TIME,
							}, tran);
						}
					}
					//删除
					string deleteSql = @"Delete from SMT_MSD_BAKE_RULE where ID=:ID ";
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