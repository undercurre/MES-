/**
*┌──────────────────────────────────────────────────────────────┐
*│　描    述：设备巡检配置接口实现                                                    
*│　作    者：嘉志科技                                            
*│　版    本：2.0    模板代码自动生成                                                
*│　创建时间：2019-10-23 16:18:50                             
*└──────────────────────────────────────────────────────────────┘
*┌──────────────────────────────────────────────────────────────┐
*│　命名空间： JZ.IMS.Repository.Oracle                                  
*│　类    名： IpqaConfigRepository                                      
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
using JZ.IMS.ViewModels;
using JZ.IMS.Core.Extensions;
using System.Linq;

namespace JZ.IMS.Repository.Oracle
{
	public class IpqaConfigRepository : BaseRepository<IpqaConfig, Decimal>, IIpqaConfigRepository
	{
		public IpqaConfigRepository(IOptionsSnapshot<DbOption> options)
		{
			_dbOption = options.Get("iWMS");
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
			string sql = "SELECT ENABLED FROM IPQA_CONFIG WHERE ID=:ID";
			var result = await _dbConnection.QueryFirstOrDefaultAsync<string>(sql, new
			{
				ID = id,
			});

			return result == "1" ? true : false;
		}

		/// <summary>
		/// 修改激活状态
		/// </summary>
		/// <param name="id">主键</param>
		/// <param name="status">更改后的状态</param>
		/// <returns></returns>
		public async Task<decimal> ChangeEnableStatus(decimal id, bool status)
		{
			string sql = "update IPQA_CONFIG set ENABLED=:ENABLED where Id=:Id";
			return await _dbConnection.ExecuteAsync(sql, new
			{
				ENABLED = status ? '1' : '0',
				Id = id,
			});
		}

		/// <summary>
		/// 巡检项目是否已被使用 
		/// </summary>
		/// <param name="id">项目id</param>
		/// <returns></returns>
		public async Task<bool> IpqaConfigIsByUsed(decimal id)
		{
			string sql = "select count(0) from ipqa_dtl where ipqa_config_id = :id";
			object result = await _dbConnection.ExecuteScalarAsync(sql, new
			{
				id
			});

			return (Convert.ToInt32(result) > 0);
		}

		// <summary>
		/// 获取表的序列
		/// </summary>
		/// <returns></returns>
		public async Task<decimal> GetSEQID()
		{
			string sql = "SELECT IPQA_CONFIG_SEQ.NEXTVAL MY_SEQ FROM DUAL";
			var result = await _dbConnection.ExecuteScalarAsync(sql);
			return (decimal)result;
		}

		/// <summary>
		/// 导出分页分页
		/// </summary>
		/// <param name="model"></param>
		/// <returns></returns>
		public async Task<TableDataModel> GetExportData(IpqaConfigRequestModel model)
		{
			string sql = @" SELECT ROW_NUMBER() OVER(ORDER BY ICG.ORDER_ID ASC) AS ROWNO,  ICG.ID, ICG.CATEGORY, ICG.ITEM_NAME, ICG.ORDER_ID, ICG.PROCESS_REQUIRE, ICG.REFERENCE_STANDARD,TQUANTIZE.NAME QUANTIZE_TYPE,TENABLE.NAME ENABLED, ICG.CREATETIME, ICG.CREATOR, ICG.QUANTIZE_VAL,TTYPE.NAME IPQA_TYPE FROM IPQA_CONFIG ICG
							LEFT JOIN (SELECT '0' AS CODE,'SMT车间巡检' AS NAME FROM DUAL UNION  SELECT '1' AS CODE,'产线车间巡检' AS NAME FROM DUAL ) TTYPE  ON TTYPE.CODE=ICG.IPQA_TYPE
							LEFT JOIN (SELECT '0' AS CODE,'无量化标准' AS NAME FROM DUAL UNION  SELECT '1' AS CODE,'有量化标准' AS NAME FROM DUAL ) TQUANTIZE  ON TQUANTIZE.CODE=ICG.QUANTIZE_TYPE
							LEFT JOIN (SELECT '1' AS CODE,'可用' AS NAME FROM DUAL UNION  SELECT '0' AS CODE,'不可用' AS NAME FROM DUAL ) TENABLE  ON TENABLE.CODE=ICG.ENABLED ";

			string conditions = " where 1=1 ";

			if (!model.Key.IsNullOrWhiteSpace())
			{
				conditions += $" AND (INSTR(ICG.CATEGORY, :KEY) > 0 OR INSTR(ICG.ITEM_NAME, :KEY) > 0 OR INSTR(PROCESS_REQUIRE, :KEY) > 0 or instr(REFERENCE_STANDARD, :Key) > 0) ";
			}
			if (!model.IPQA_TYPE.IsNullOrWhiteSpace())
			{
				conditions += $" and ICG.IPQA_TYPE =:IPQA_TYPE ";
			}


			string pagedSql = SQLBuilderClass.GetPagedSQL(sql, conditions);
			var resdata = await _dbConnection.QueryAsync<object>(pagedSql, model);
			string sqlcnt = @" SELECT COUNT(ICG.ID)  FROM IPQA_CONFIG ICG
							LEFT JOIN (SELECT '0' AS CODE,'SMT车间巡检' AS NAME FROM DUAL UNION  SELECT '1' AS CODE,'产线车间巡检' AS NAME FROM DUAL ) TTYPE  ON TTYPE.CODE=ICG.IPQA_TYPE
							LEFT JOIN (SELECT '0' AS CODE,'无量化标准' AS NAME FROM DUAL UNION  SELECT '1' AS CODE,'有量化标准' AS NAME FROM DUAL ) TQUANTIZE  ON TQUANTIZE.CODE=ICG.QUANTIZE_TYPE
							LEFT JOIN (SELECT '1' AS CODE,'可用' AS NAME FROM DUAL UNION  SELECT '0' AS CODE,'不可用' AS NAME FROM DUAL ) TENABLE  ON TENABLE.CODE=ICG.ENABLED " + conditions;

			int cnt = await _dbConnection.ExecuteScalarAsync<int>(sqlcnt, model);
			return new TableDataModel
			{
				count = cnt,
				data = resdata?.ToList(),
			};
		}

		/// <summary>
		/// 保存数据
		/// </summary>
		/// <param name="model"></param>
		/// <returns></returns>
		public async Task<decimal> SaveDataByTrans(IpqaConfigModel model)
		{
			int result = 1;
			ConnectionFactory.OpenConnection(_dbConnection);
			using (var tran = _dbConnection.BeginTransaction())
			{
				try
				{
					//新增
					string insertSql = @"INSERT INTO IPQA_CONFIG  
					(ID, CATEGORY, ITEM_NAME, ORDER_ID, PROCESS_REQUIRE, REFERENCE_STANDARD, QUANTIZE_TYPE, ENABLED, CREATETIME, CREATOR, QUANTIZE_VAL,IPQA_TYPE) 
					VALUES (:ID,:CATEGORY,:ITEM_NAME,:ORDER_ID,:PROCESS_REQUIRE,:REFERENCE_STANDARD,:QUANTIZE_TYPE,:ENABLED,:CREATETIME,:CREATOR,:QUANTIZE_VAL,:IPQA_TYPE)";
					if (model.insertRecords != null && model.insertRecords.Count > 0)
					{
						foreach (var item in model.insertRecords)
						{
							var newid = await GetSEQID();
							var resdata = await _dbConnection.ExecuteAsync(insertSql, new
							{
								ID = newid,
								item.CATEGORY,
								item.ITEM_NAME,
								item.ORDER_ID,
								item.PROCESS_REQUIRE,
								item.REFERENCE_STANDARD,
								item.QUANTIZE_TYPE,
								item.ENABLED,
								CREATETIME = DateTime.Now,
								CREATOR = model.UserName,
								item.QUANTIZE_VAL,
								model.IPQA_TYPE,
							}, tran);
						}
					}
					//更新
					string updateSql = @"Update IPQA_CONFIG set CATEGORY =:CATEGORY, ITEM_NAME =:ITEM_NAME,
							ORDER_ID =:ORDER_ID, PROCESS_REQUIRE =:PROCESS_REQUIRE, REFERENCE_STANDARD =:REFERENCE_STANDARD,
							QUANTIZE_TYPE =:QUANTIZE_TYPE,ENABLED =:ENABLED, QUANTIZE_VAL =:QUANTIZE_VAL 
						where ID=:ID ";
					if (model.updateRecords != null && model.updateRecords.Count > 0)
					{
						foreach (var item in model.updateRecords)
						{
							var resdata = await _dbConnection.ExecuteAsync(updateSql, new
							{
								item.ID,
								item.CATEGORY,
								item.ITEM_NAME,
								item.ORDER_ID,
								item.PROCESS_REQUIRE,
								item.REFERENCE_STANDARD,
								item.QUANTIZE_TYPE,
								item.ENABLED,
								item.QUANTIZE_VAL,
							}, tran);
						}
					}
					//删除
					string deleteSql = @"Delete from IPQA_CONFIG where ID=:ID ";
					if (model.removeRecords != null && model.removeRecords.Count > 0)
					{
						foreach (var item in model.removeRecords)
						{
							var resdata = await _dbConnection.ExecuteAsync(deleteSql, new
							{
								item.ID
							}, tran);
						}
					}

					tran.Commit();
				} catch (Exception ex)
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