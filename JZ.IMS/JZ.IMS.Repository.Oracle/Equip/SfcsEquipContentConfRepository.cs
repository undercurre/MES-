/**
*┌──────────────────────────────────────────────────────────────┐
*│　描    述：设备点检内容配置表接口实现                                                    
*│　作    者：嘉志科技                                            
*│　版    本：2.0    模板代码自动生成                                                
*│　创建时间：2019-10-30 15:43:48                             
*└──────────────────────────────────────────────────────────────┘
*┌──────────────────────────────────────────────────────────────┐
*│　命名空间： JZ.IMS.Repository.Oracle                                  
*│　类    名： SfcsEquipContentConfRepository                                      
*└──────────────────────────────────────────────────────────────┘
*/
using JZ.IMS.Core.DbHelper;
using JZ.IMS.Core.Options;
using JZ.IMS.Core.Repository;
using JZ.IMS.IRepository;
using JZ.IMS.Models;
using Dapper;
using Microsoft.Extensions.Options;
using System;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Data;

namespace JZ.IMS.Repository.Oracle
{
	public class SfcsEquipContentConfRepository : BaseRepository<SfcsEquipContentConf, Decimal>, ISfcsEquipContentConfRepository
	{
		public SfcsEquipContentConfRepository(IOptionsSnapshot<DbOption> options)
		{
			_dbOption = options.Get("iWMS");
			if (_dbOption == null)
			{
				throw new ArgumentNullException(nameof(DbOption));
			}
			_dbConnection = ConnectionFactory.CreateConnection(_dbOption.DbType, _dbOption.ConnectionString);
		}

		public async Task<int> InsertAsync(SfcsEquipContentConf entity, List<SOP_OPERATIONS_ROUTES_RESOURCE> resourceList)
		{
			ConnectionFactory.OpenConnection(_dbConnection);
			using (IDbTransaction transaction = _dbConnection.BeginTransaction())
			{ // 创建事务	
				try
				{
					await _dbConnection.InsertAsync(entity, transaction);

					if (resourceList != null)
					{
						foreach (SOP_OPERATIONS_ROUTES_RESOURCE resource in resourceList)
						{
							resource.ID = Get_Resource_SEQID();
							resource.MST_ID = entity.ID;
							await _dbConnection.InsertAsync(resource, transaction);
						}
					}
					transaction.Commit(); // 提交事务
					return 1;
				}
				catch
				{
					transaction.Rollback(); // 回滚事务
					throw;
				}
				finally
				{
					if (_dbConnection.State != ConnectionState.Closed)
					{
						_dbConnection.Close();
					}
				}
			}
		}

		public async Task<int> UpdateAsync(SfcsEquipContentConf entity, List<SOP_OPERATIONS_ROUTES_RESOURCE> resourceList)
		{
			ConnectionFactory.OpenConnection(_dbConnection);
			using (IDbTransaction transaction = _dbConnection.BeginTransaction())
			{ // 创建事务	
				try
				{
					//更新主表数据
					await _dbConnection.UpdateAsync(entity, transaction);

					//删除资源表原有数据
					await DelEquipContentConfResource(entity.ID, transaction);

					if (resourceList != null)
					{
						//插入资源表新的数据
						foreach (SOP_OPERATIONS_ROUTES_RESOURCE resource in resourceList)
						{
							resource.ID = Get_Resource_SEQID();
							resource.MST_ID = entity.ID;
							await _dbConnection.InsertAsync(resource, transaction);
						}
					}
					transaction.Commit(); // 提交事务
					return 1;
				}
				catch
				{
					transaction.Rollback(); // 回滚事务
					throw;
				}
				finally
				{
					if (_dbConnection.State != ConnectionState.Closed)
					{
						_dbConnection.Close();
					}
				}
			}
		}

		/// <summary>
		/// 根据主键获取激活状态
		/// </summary>
		/// <param name="id">主键</param>
		/// <returns></returns>
		public async Task<Boolean> GetEnableStatus(decimal id)
		{
			string sql = "SELECT ENABLE FROM SFCS_EQUIP_CONTENT_CONF WHERE ID=:ID";
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
			string sql = "UPDATE SFCS_EQUIP_CONTENT_CONF set ENABLE = :ENABLE where Id=:Id";
			return await _dbConnection.ExecuteAsync(sql, new
			{
				ENABLE = status ? 'Y' : 'N',
				Id = id,
			});
		}

		// <summary>
		/// 获取表的序列
		/// </summary>
		/// <returns></returns>
		public async Task<decimal> GetSEQID()
		{
			string sql = "SELECT SFCS_EQUIP_CONTENT_CONF_SEQ.NEXTVAL MY_SEQ FROM DUAL";
			var result = await _dbConnection.ExecuteScalarAsync(sql);
			return (decimal)result;
		}

		public decimal Get_Resource_SEQID()
		{
			string sql = "SELECT sop_operations_routes_res_seq.NEXTVAL MY_SEQ FROM DUAL";
			var result = _dbConnection.ExecuteScalar(sql);
			return (decimal)result;
		}

		/// <summary>
		/// 删除设备点检事项图片
		/// </summary>
		/// <param name="mstId"></param>
		/// <returns></returns>
		public async Task<int> DelEquipContentConfResource(decimal mstId, IDbTransaction transaction)
		{
			string sql = "DELETE SOP_OPERATIONS_ROUTES_RESOURCE WHERE MST_ID = :MST_ID AND RESOURCES_CATEGORY = 5";
			return await _dbConnection.ExecuteAsync(sql, new
			{
				MST_ID = mstId
			}, transaction);
		}
	}
}