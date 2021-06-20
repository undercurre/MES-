/**
*┌──────────────────────────────────────────────────────────────┐
*│　描    述：接口实现                                                    
*│　作    者：嘉志科技                                            
*│　版    本：2.0    模板代码自动生成                                                
*│　创建时间：2020-03-31 10:24:29                             
*└──────────────────────────────────────────────────────────────┘
*┌──────────────────────────────────────────────────────────────┐
*│　命名空间： JZ.IMS.Repository.Oracle                                  
*│　类    名： SfcsReasonConfigRepository                                      
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
    public class SfcsReasonConfigRepository:BaseRepository<SfcsReasonConfig,Decimal>, ISfcsReasonConfigRepository
    {
        public SfcsReasonConfigRepository(IOptionsSnapshot<DbOption> options)
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
			string sql = "SELECT ENABLED FROM SFCS_REASON_CONFIG WHERE ID=:ID";
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
			string sql = "UPDATE SFCS_REASON_CONFIG set ENABLED=:ENABLED WHERE ID=:Id";
			return await _dbConnection.ExecuteAsync(sql, new
			{
				ENABLED = status ? 'Y' : 'N',
				Id = id,
			});
		}

        // <summary>
        /// 获取表的序列
        /// </summary>
        /// <returns></returns>
		public async Task<decimal> GetSEQID()
		{
			string sql = "SELECT SFCS_REASON_CONFIG_SEQ.NEXTVAL MY_SEQ FROM DUAL";
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
			string sql = "select count(0) from SFCS_REASON_CONFIG where id = :id";
			object result = await _dbConnection.ExecuteScalarAsync(sql, new
			{
				id
			});

			return (Convert.ToInt32(result) > 0);
		}

		/// <summary>
		/// 获取类型
		/// </summary>
		/// <returns></returns>
		public async Task<List<object>> GetLookUp(string type)
		{
			string sql = @" SELECT LOOKUP_CODE, CHINESE, ENABLED  FROM SFCS_PARAMETERS WHERE LOOKUP_TYPE = :LOOKUP_TYPE ";
			var result = await _dbConnection.QueryAsync(sql,new {
				LOOKUP_TYPE=type
			});
			return result?.ToList();
		}

		/// <summary>
		/// 不良原因来源
		/// </summary>
		/// <returns></returns>
		public async Task<List<Object>> GetSource()
		{
			string sql = " SELECT '内部原因' SOURCE FROM DUAL UNION SELECT '外部原因' SOURCE FROM DUAL ";
			var result = await _dbConnection.QueryAsync(sql);
			return result?.ToList();
		}

		/// <summary>
		/// 导出分页分页
		/// </summary>
		/// <param name="model"></param>
		/// <returns></returns>
		public async Task<TableDataModel> GetExportData(SfcsReasonConfigRequestModel model)
		{
			string sql = @"  SELECT ROW_NUMBER() OVER(ORDER BY SRC.ID DESC) AS ROWNO,SRC.ID,SRC.REASON_CODE,SP1.CHINESE REASON_TYPE,SP2.CHINESE REASON_CLASS,SP3.CHINESE REASON_CATEGORY,SP4.CHINESE LEVEL_CODE,SRC.SOURCE,SRC.REASON_DESCRIPTION,SRC.CHINESE_DESCRIPTION,SRC.ENABLED FROM SFCS_REASON_CONFIG SRC
							 LEFT JOIN SFCS_PARAMETERS SP1 ON SP1.LOOKUP_TYPE='REASON_TYPE' AND SRC.REASON_TYPE=SP1.LOOKUP_CODE
							 LEFT JOIN SFCS_PARAMETERS SP2 ON SP2.LOOKUP_TYPE='REASON_CLASS' AND SRC.REASON_CLASS=SP2.LOOKUP_CODE
							 LEFT JOIN SFCS_PARAMETERS SP3 ON SP3.LOOKUP_TYPE='REASON_CATEGORY' AND SRC.REASON_CATEGORY=SP3.LOOKUP_CODE
							 LEFT JOIN SFCS_PARAMETERS SP4 ON SP4.LOOKUP_TYPE='REASON_LEVEL_CODE'  AND SRC.LEVEL_CODE=SP4.LOOKUP_CODE  ";

			string conditions = " WHERE SRC.ID > 0 ";

			if (!model.REASON_CODE.IsNullOrWhiteSpace())
			{
				conditions += $"and instr(SRC.REASON_CODE,:REASON_CODE) > 0 ";
			}
			if (model.REASON_TYPE > 0)
			{
				conditions += $"and (SRC.REASON_TYPE=:REASON_TYPE) ";
			}
			if (model.REASON_CLASS > 0)
			{
				conditions += $"and (SRC.REASON_CLASS=:REASON_CLASS) ";
			}
			if (model.REASON_CATEGORY > 0)
			{
				conditions += $"and (SRC.REASON_CATEGORY=:REASON_CATEGORY) ";
			}
			if (model.LEVEL_CODE > 0)
			{
				conditions += $"and (SRC.LEVEL_CODE=:LEVEL_CODE) ";
			}
			if (!model.SOURCE.IsNullOrWhiteSpace())
			{
				conditions += $"and instr(SRC.SOURCE, :SOURCE) > 0 ";
			}
			if (!model.REASON_DESCRIPTION.IsNullOrWhiteSpace())
			{
				conditions += $"and instr(SRC.REASON_DESCRIPTION, :REASON_DESCRIPTION) > 0 ";
			}
			if (!model.CHINESE_DESCRIPTION.IsNullOrWhiteSpace())
			{
				conditions += $"and instr(SRC.CHINESE_DESCRIPTION, :CHINESE_DESCRIPTION) > 0 ";
			}
			if (!model.ENABLED.IsNullOrWhiteSpace())
			{
				conditions += $"and SRC.ENABLED=:ENABLED ";
			}

			string pagedSql = SQLBuilderClass.GetPagedSQL(sql, conditions);
			var resdata = await _dbConnection.QueryAsync<object>(pagedSql, model);
			string sqlcnt = @" SELECT COUNT(SRC.ID) FROM SFCS_REASON_CONFIG SRC
							   LEFT JOIN SFCS_PARAMETERS SP1 ON SP1.LOOKUP_TYPE='REASON_TYPE' AND SRC.REASON_TYPE=SP1.LOOKUP_CODE
							   LEFT JOIN SFCS_PARAMETERS SP2 ON SP2.LOOKUP_TYPE='REASON_CLASS' AND SRC.REASON_CLASS=SP2.LOOKUP_CODE
							   LEFT JOIN SFCS_PARAMETERS SP3 ON SP3.LOOKUP_TYPE='REASON_CATEGORY' AND SRC.REASON_CATEGORY=SP3.LOOKUP_CODE
							   LEFT JOIN SFCS_PARAMETERS SP4 ON SP4.LOOKUP_TYPE='REASON_LEVEL_CODE'  AND SRC.LEVEL_CODE=SP4.LOOKUP_CODE " + conditions;

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
		public async Task<decimal> SaveDataByTrans(SfcsReasonConfigModel model)
		{
			int result = 1;
			ConnectionFactory.OpenConnection(_dbConnection);
			using (var tran = _dbConnection.BeginTransaction())
			{
				try
				{
					//新增
					string insertSql = @"insert into SFCS_REASON_CONFIG 
					(ID,REASON_CODE,REASON_TYPE,REASON_CLASS,REASON_CATEGORY,LEVEL_CODE,SOURCE,REASON_DESCRIPTION,CHINESE_DESCRIPTION,ENABLED) 
					VALUES (:ID,:REASON_CODE,:REASON_TYPE,:REASON_CLASS,:REASON_CATEGORY,:LEVEL_CODE,:SOURCE,:REASON_DESCRIPTION,:CHINESE_DESCRIPTION,:ENABLED)";
					if (model.insertRecords != null && model.insertRecords.Count > 0)
					{
						foreach (var item in model.insertRecords)
						{
							var newid = await GetSEQID();
							var resdata = await _dbConnection.ExecuteAsync(insertSql, new
							{
								ID = newid,
								item.REASON_CODE,
								item.REASON_TYPE,
								item.REASON_CLASS,
								item.REASON_CATEGORY,
								item.LEVEL_CODE,
								item.SOURCE,
								item.REASON_DESCRIPTION,
								item.CHINESE_DESCRIPTION,
								item.ENABLED							}, tran);
						}
					}
					//更新
					string updateSql = @"Update SFCS_REASON_CONFIG set REASON_CODE=:REASON_CODE,REASON_TYPE=:REASON_TYPE,REASON_CLASS=:REASON_CLASS,REASON_CATEGORY=:REASON_CATEGORY,LEVEL_CODE=:LEVEL_CODE,SOURCE=:SOURCE,REASON_DESCRIPTION=:REASON_DESCRIPTION,CHINESE_DESCRIPTION=:CHINESE_DESCRIPTION,ENABLED=:ENABLED
						where ID=:ID ";
					if (model.updateRecords != null && model.updateRecords.Count > 0)
					{
						foreach (var item in model.updateRecords)
						{
							var resdata = await _dbConnection.ExecuteAsync(updateSql, new
							{
								item.ID,
								item.REASON_CODE,
								item.REASON_TYPE,
								item.REASON_CLASS,
								item.REASON_CATEGORY,
								item.LEVEL_CODE,
								item.SOURCE,
								item.REASON_DESCRIPTION,
								item.CHINESE_DESCRIPTION,
								item.ENABLED,
							}, tran);
						}
					}
					////删除
					//string deleteSql = @"Delete from SFCS_REASON_CONFIG where ID=:ID ";
					//if (model.removeRecords != null && model.removeRecords.Count > 0)
					//{
					//	foreach (var item in model.removeRecords)
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