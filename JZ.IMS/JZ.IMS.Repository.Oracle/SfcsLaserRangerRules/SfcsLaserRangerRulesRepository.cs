/*
*┌──────────────────────────────────────────────────────────────┐
*│　描    述：瑞德镭雕机流水号规则表（复杂流程）接口实现                                                    
*│　作    者：嘉志科技                                            
*│　版    本：2.0    模板代码自动生成                                                
*│　创建时间：2020-12-08 18:46:06                             
*└──────────────────────────────────────────────────────────────┘
*┌──────────────────────────────────────────────────────────────┐
*│　命名空间： JZ.IMS.Repository.Oracle                                  
*│　类    名： SfcsLaserRangerRulesRepository                                      
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
    public class SfcsLaserRangerRulesRepository:BaseRepository<SfcsLaserRangerRules,Decimal>, ISfcsLaserRangerRulesRepository
    {
        public SfcsLaserRangerRulesRepository(IOptionsSnapshot<DbOption> options)
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
			string sql = "SELECT ENABLED FROM SFCS_LASER_RANGER_RULES WHERE ID=:ID";
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
			string sql = "UPDATE SFCS_LASER_RANGER_RULES set ENABLED=:ENABLED WHERE ID=:Id";
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
			string sql = "SELECT SFCS_LASER_RANGER_RULES_SEQ.NEXTVAL MY_SEQ FROM DUAL";
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
			string sql = "select count(0) from SFCS_LASER_RANGER_RULES where id = :id";
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
		public async Task<decimal> SaveDataByTrans(SfcsLaserRangerRulesModel model)
		{
			int result = 1;
			ConnectionFactory.OpenConnection(_dbConnection);
			using (var tran = _dbConnection.BeginTransaction())
			{
				try
				{
					//新增
					string insertSql = @"insert into SFCS_LASER_RANGER_RULES 
					(ID,WO_NO,FIX_HEADER,FIX_TAIL,RANGE_LENGTH,RANGE_START_CODE,DIGITAL,EXCLUSIVE_CHAR,ENABLED,CREATE_TIME,CREATE_BY,UPDATE_TIME,UPDATE_BY,ATTRIBUTE1,ATTRIBUTE2,ATTRIBUTE3,ATTRIBUTE4,ATTRIBUTE5) 
					VALUES (:ID,:WO_NO,:FIX_HEADER,:FIX_TAIL,:RANGE_LENGTH,:RANGE_START_CODE,:DIGITAL,:EXCLUSIVE_CHAR,:ENABLED,:CREATE_TIME,:CREATE_BY,:UPDATE_TIME,:UPDATE_BY,:ATTRIBUTE1,:ATTRIBUTE2,:ATTRIBUTE3,:ATTRIBUTE4,:ATTRIBUTE5)";
					if (model.InsertRecords != null && model.InsertRecords.Count > 0)
					{
						foreach (var item in model.InsertRecords)
						{
							var newid = await GetSEQID();
							var resdata = await _dbConnection.ExecuteAsync(insertSql, new
							{
								ID = newid,
								item.WO_NO,
								item.FIX_HEADER,
								item.FIX_TAIL,
								item.RANGE_LENGTH,
								item.RANGE_START_CODE,
								item.DIGITAL,
								item.EXCLUSIVE_CHAR,
								item.ENABLED,
								item.CREATE_TIME,
								item.CREATE_BY,
								item.UPDATE_TIME,
								item.UPDATE_BY,
								item.ATTRIBUTE1,
								item.ATTRIBUTE2,
								item.ATTRIBUTE3,
								item.ATTRIBUTE4,
								item.ATTRIBUTE5,
							}, tran);
						}
					}
					//更新
					string updateSql = @"Update SFCS_LASER_RANGER_RULES set WO_NO=:WO_NO,FIX_HEADER=:FIX_HEADER,FIX_TAIL=:FIX_TAIL,RANGE_LENGTH=:RANGE_LENGTH,RANGE_START_CODE=:RANGE_START_CODE,DIGITAL=:DIGITAL,EXCLUSIVE_CHAR=:EXCLUSIVE_CHAR,ENABLED=:ENABLED,CREATE_TIME=:CREATE_TIME,CREATE_BY=:CREATE_BY,UPDATE_TIME=:UPDATE_TIME,UPDATE_BY=:UPDATE_BY,ATTRIBUTE1=:ATTRIBUTE1,ATTRIBUTE2=:ATTRIBUTE2,ATTRIBUTE3=:ATTRIBUTE3,ATTRIBUTE4=:ATTRIBUTE4,ATTRIBUTE5=:ATTRIBUTE5  
						where ID=:ID ";
					if (model.UpdateRecords != null && model.UpdateRecords.Count > 0)
					{
						foreach (var item in model.UpdateRecords)
						{
							var resdata = await _dbConnection.ExecuteAsync(updateSql, new
							{
								item.ID,
								item.WO_NO,
								item.FIX_HEADER,
								item.FIX_TAIL,
								item.RANGE_LENGTH,
								item.RANGE_START_CODE,
								item.DIGITAL,
								item.EXCLUSIVE_CHAR,
								item.ENABLED,
								item.CREATE_TIME,
								item.CREATE_BY,
								item.UPDATE_TIME,
								item.UPDATE_BY,
								item.ATTRIBUTE1,
								item.ATTRIBUTE2,
								item.ATTRIBUTE3,
								item.ATTRIBUTE4,
								item.ATTRIBUTE5,
							}, tran);
						}
					}
					//删除
					string deleteSql = @"Delete from SFCS_LASER_RANGER_RULES where ID=:ID ";
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