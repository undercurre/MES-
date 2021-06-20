/**
*┌──────────────────────────────────────────────────────────────┐
*│　描    述：接口实现                                                    
*│　作    者：嘉志科技                                            
*│　版    本：2.0    模板代码自动生成                                                
*│　创建时间：2020-03-30 10:44:46                             
*└──────────────────────────────────────────────────────────────┘
*┌──────────────────────────────────────────────────────────────┐
*│　命名空间： JZ.IMS.Repository.Oracle                                  
*│　类    名： SfcsMessageConfigRepository                                      
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
    public class SfcsMessageConfigRepository:BaseRepository<SfcsMessageConfig,Decimal>, ISfcsMessageConfigRepository
    {
        public SfcsMessageConfigRepository(IOptionsSnapshot<DbOption> options)
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
			string sql = "SELECT ENABLED FROM SFCS_MESSAGE_CONFIG WHERE ID=:ID";
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
			string sql = "UPDATE SFCS_MESSAGE_CONFIG set ENABLED=:ENABLED WHERE ID=:Id";
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
			string sql = "SELECT SFCS_MESSAGE_CONFIG_SEQ.NEXTVAL MY_SEQ FROM DUAL";
			var result = await _dbConnection.ExecuteScalarAsync(sql);
			return (decimal)result;
		}

        /// <summary>
		///ID是否已被使用 
		/// </summary>
		/// <param name="id">id</param>
		/// <returns></returns>
		public async Task<bool> ItemIsByUsed(decimal id)
		{
			string sql = "select count(0) from SFCS_MESSAGE_CONFIG where id = :id";
			object result = await _dbConnection.ExecuteScalarAsync(sql, new
			{
				id
			});

			return (Convert.ToInt32(result) > 0);
		}

		/// <summary>
		/// 获取种类状态
		/// </summary>
		/// <returns></returns>
		public async Task<List<object>> GetStatusList() {
			string sql = " SELECT LOOKUP_CODE, MEANING, DESCRIPTION FROM SFCS_PARAMETERS WHERE LOOKUP_TYPE = 'MESSAGE_CATEGORY' ";
			var result = await _dbConnection.QueryAsync(sql);
			return result?.ToList();
		}

		/// <summary>
		/// 获取导出数据
		/// </summary>
		/// <param name="model"></param>
		/// <returns></returns>
		public async Task<TableDataModel> GetExportData(SfcsMessageConfigRequestModel model)
		{
			string conditions = " WHERE m.ID > 0 ";
			if (!model.MESSAGE_NO.IsNullOrWhiteSpace())
			{
				conditions += $"and instr(m.MESSAGE_NO, :MESSAGE_NO) > 0 ";
			}
			if (!model.CHINESE_MESSAGE.IsNullOrWhiteSpace())
			{
				conditions += $"and instr(m.CHINESE_MESSAGE, :CHINESE_MESSAGE) > 0 ";
			}
			if (!model.ENGLISH_MESSAGE.IsNullOrWhiteSpace())
			{
				conditions += $"and instr(m.ENGLISH_MESSAGE, :ENGLISH_MESSAGE) > 0 ";
			}
			if (model.PARAMETERS_COUNT > 0)
			{
				conditions += $"and instr(m.PARAMETERS_COUNT, :PARAMETERS_COUNT) > 0 ";
			}
			if (!model.CATEGORY.IsNullOrWhiteSpace())
			{
				conditions += $"and instr(m.CATEGORY, :CATEGORY) > 0 ";
			}
			if (!model.BACKGROUND_FLAG.IsNullOrWhiteSpace())
			{
				conditions += $"and instr(m.BACKGROUND_FLAG, :BACKGROUND_FLAG) > 0 ";
			}
			if (!model.APPLICATION_NAME.IsNullOrWhiteSpace())
			{
				conditions += $"and instr(m.APPLICATION_NAME, :APPLICATION_NAME) > 0 ";
			}

			string sql = @"SELECT ROWNUM AS ROWNO, m.ID,m.MESSAGE_NO,m.CHINESE_MESSAGE,m.ENGLISH_MESSAGE,m.PARAMETERS_COUNT, pm.MEANING as CATEGORY,
							 m.BACKGROUND_FLAG,m.APPLICATION_NAME  
                           From SFCS_MESSAGE_CONFIG m  
                           INNER JOIN SFCS_PARAMETERS pm ON m.CATEGORY = pm.LOOKUP_CODE AND pm.LOOKUP_TYPE = 'MESSAGE_CATEGORY' ";

			string pagedSql = SQLBuilderClass.GetPagedSQL(sql, "m.id desc", conditions);
			var resdata = await _dbConnection.QueryAsync<dynamic>(pagedSql, model);
			string sqlcnt = @"SELECT COUNT(0) From SFCS_MESSAGE_CONFIG m  
                              INNER JOIN SFCS_PARAMETERS pm ON m.CATEGORY = pm.LOOKUP_CODE AND pm.LOOKUP_TYPE = 'MESSAGE_CATEGORY' " + conditions;

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
		public async Task<decimal> SaveDataByTrans(SfcsMessageConfigModel model)
		{
			int result = 1;
			ConnectionFactory.OpenConnection(_dbConnection);
			using (var tran = _dbConnection.BeginTransaction())
			{
				try
				{
					//新增
					string insertSql = @"insert into SFCS_MESSAGE_CONFIG 
					(ID,MESSAGE_NO,CHINESE_MESSAGE,ENGLISH_MESSAGE,PARAMETERS_COUNT,CATEGORY,BACKGROUND_FLAG,APPLICATION_NAME) 
					VALUES (:ID,:MESSAGE_NO,:CHINESE_MESSAGE,:ENGLISH_MESSAGE,:PARAMETERS_COUNT,:CATEGORY,:BACKGROUND_FLAG,:APPLICATION_NAME)";
					if (model.insertRecords != null && model.insertRecords.Count > 0)
					{
						foreach (var item in model.insertRecords)
						{
							var newid = await Get_MES_SEQ_ID();
							var resdata = await _dbConnection.ExecuteAsync(insertSql, new
							{
								ID = newid,
								item.MESSAGE_NO,
								item.CHINESE_MESSAGE,
								item.ENGLISH_MESSAGE,
								item.PARAMETERS_COUNT,
								item.CATEGORY,
								item.BACKGROUND_FLAG,
								item.APPLICATION_NAME
							}, tran);
						}
					}
					//更新
					string updateSql = @"Update SFCS_MESSAGE_CONFIG set MESSAGE_NO=:MESSAGE_NO,CHINESE_MESSAGE=:CHINESE_MESSAGE,ENGLISH_MESSAGE=:ENGLISH_MESSAGE,PARAMETERS_COUNT=:PARAMETERS_COUNT,CATEGORY=:CATEGORY,BACKGROUND_FLAG=:BACKGROUND_FLAG,APPLICATION_NAME=:APPLICATION_NAME
						where ID=:ID ";
					if (model.updateRecords != null && model.updateRecords.Count > 0)
					{
						foreach (var item in model.updateRecords)
						{
							var resdata = await _dbConnection.ExecuteAsync(updateSql, new
							{
								item.ID,
								item.MESSAGE_NO,
								item.CHINESE_MESSAGE,
								item.ENGLISH_MESSAGE,
								item.PARAMETERS_COUNT,
								item.CATEGORY,
								item.BACKGROUND_FLAG,
								item.APPLICATION_NAME							}, tran);
						}
					}
					////删除
					string deleteSql = @"Delete from SFCS_MESSAGE_CONFIG where ID=:ID ";
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