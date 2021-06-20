/*
*┌──────────────────────────────────────────────────────────────┐
*│　描    述：呼叫内容配置接口实现                                                    
*│　作    者：嘉志科技                                            
*│　版    本：2.0    模板代码自动生成                                                
*│　创建时间：2020-08-12 11:10:38                             
*└──────────────────────────────────────────────────────────────┘
*┌──────────────────────────────────────────────────────────────┐
*│　命名空间： JZ.IMS.Repository.Oracle                                  
*│　类    名： AndonCallContentConfigRepository                                      
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
    public class AndonCallContentConfigRepository:BaseRepository<AndonCallContentConfig,Decimal>, IAndonCallContentConfigRepository
    {
        public AndonCallContentConfigRepository(IOptionsSnapshot<DbOption> options)
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
			string sql = "SELECT ENABLED FROM ANDON_CALL_CONTENT_CONFIG WHERE ID=:ID";
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
			string sql = "UPDATE ANDON_CALL_CONTENT_CONFIG set ENABLED=:ENABLED WHERE ID=:Id";
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
			string sql = "SELECT ANDON_CALL_CONTENT_CONFIG_SEQ.NEXTVAL MY_SEQ FROM DUAL";
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
			string sql = "select count(0) from ANDON_CALL_CONTENT_CONFIG where id = :id";
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
		public async Task<decimal> SaveDataByTrans(AndonCallContentConfigModel model)
		{
			int result = 1;
			ConnectionFactory.OpenConnection(_dbConnection);
			using (var tran = _dbConnection.BeginTransaction())
			{
				try
				{
					//新增
					string insertSql = @"insert into ANDON_CALL_CONTENT_CONFIG 
					(ID,CALL_TYPE_CODE,DESCRIPTION,CHINESE,ENABLED,CALL_CODE,CALL_TITLE,CALL_CATEGORY_CODE,CREATOR,CREATE_TIME) 
					VALUES (:ID,:CALL_TYPE_CODE,:DESCRIPTION,:CHINESE,:ENABLED,:CALL_CODE,:CALL_TITLE,:CALL_CATEGORY_CODE,:CREATOR,:CREATE_TIME)";
					if (model.InsertRecords != null && model.InsertRecords.Count > 0)
					{
						foreach (var item in model.InsertRecords)
						{
							var newid = await GetSEQID();
							var resdata = await _dbConnection.ExecuteAsync(insertSql, new
							{
								ID = newid,
								item.CALL_TYPE_CODE,
								item.DESCRIPTION,
								item.CHINESE,
								item.ENABLED,
								item.CALL_CODE,
								item.CALL_TITLE,
								item.CALL_CATEGORY_CODE,
								item.CREATOR,
								item.CREATE_TIME,
							}, tran);
						}
					}
					//更新
					string updateSql = @"Update ANDON_CALL_CONTENT_CONFIG set CALL_TYPE_CODE=:CALL_TYPE_CODE,DESCRIPTION=:DESCRIPTION,CHINESE=:CHINESE,ENABLED=:ENABLED,CALL_CODE=:CALL_CODE,CALL_TITLE=:CALL_TITLE,CALL_CATEGORY_CODE=:CALL_CATEGORY_CODE,CREATOR=:CREATOR,CREATE_TIME=:CREATE_TIME  
						where ID=:ID ";
					if (model.UpdateRecords != null && model.UpdateRecords.Count > 0)
					{
						foreach (var item in model.UpdateRecords)
						{
							var resdata = await _dbConnection.ExecuteAsync(updateSql, new
							{
								item.ID,
								item.CALL_TYPE_CODE,
								item.DESCRIPTION,
								item.CHINESE,
								item.ENABLED,
								item.CALL_CODE,
								item.CALL_TITLE,
								item.CALL_CATEGORY_CODE,
								item.CREATOR,
								item.CREATE_TIME,
							}, tran);
						}
					}
					//删除
					string deleteSql = @"Delete from ANDON_CALL_CONTENT_CONFIG where ID=:ID ";
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

		/// <summary>
		/// 导出数据
		/// </summary>
		/// <param name="model"></param>
		/// <returns></returns>
        public async Task<TableDataModel> GetExportData(AndonCallContentConfigRequestModel model)
        {
			string conditions = "WHERE a.ID > 0 ";
			if (!model.ID.IsNullOrWhiteSpace())
			{
				conditions += $"and (a.ID =:ID)";
			}
			if (!model.CALL_CATEGORY_CODE.IsNullOrWhiteSpace())
			{
				conditions += $"and (a.CALL_CATEGORY_CODE =:CALL_CATEGORY_CODE)";
			}
			if (!model.CALL_TYPE_CODE.IsNullOrWhiteSpace())
			{
				conditions += $"and (a.CALL_TYPE_CODE =:CALL_TYPE_CODE)";
			}
			if (!model.Key.IsNullOrWhiteSpace())
			{
				conditions += $"and (instr(a.DESCRIPTION, :Key) > 0 or instr(a.CHINESE, :Key) > 0)";
			}

			string sql = @" SELECT ROWNUM as rowno, a.* from ANDON_CALL_CONTENT_CONFIG  a ";

			string pagedSql = SQLBuilderClass.GetPagedSQL(sql, " a.id desc", conditions);
			var resdata = await _dbConnection.QueryAsync<dynamic>(pagedSql, model);
			string sqlcnt = @"SELECT COUNT(0) From ANDON_CALL_CONTENT_CONFIG   a " + conditions;

			int cnt = await _dbConnection.ExecuteScalarAsync<int>(sqlcnt, model);
			return new TableDataModel
			{
				count = cnt,
				data = resdata?.ToList(),
			};
		}
    }
}