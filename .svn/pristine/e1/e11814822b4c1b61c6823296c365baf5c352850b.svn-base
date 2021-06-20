/**
*┌──────────────────────────────────────────────────────────────┐
*│　描    述：接口实现                                                    
*│　作    者：嘉志科技                                            
*│　版    本：2.0    模板代码自动生成                                                
*│　创建时间：2020-03-18 15:41:41                             
*└──────────────────────────────────────────────────────────────┘
*┌──────────────────────────────────────────────────────────────┐
*│　命名空间： JZ.IMS.Repository.Oracle                                  
*│　类    名： SmtStationRepository                                      
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
    public class SmtStationRepository:BaseRepository<SmtStation,Decimal>, ISmtStationRepository
    {
        public SmtStationRepository(IOptionsSnapshot<DbOption> options)
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
			string sql = "SELECT ENABLED FROM SMT_STATION WHERE ID=:ID";
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
			string sql = "UPDATE SMT_STATION set ENABLED=:ENABLED WHERE ID=:Id";
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
			string sql = "SELECT SMT_STATION_SEQ.NEXTVAL MY_SEQ FROM DUAL";
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
			string sql = "select count(0) from SMT_STATION where id = :id";
			object result = await _dbConnection.ExecuteScalarAsync(sql, new
			{
				id
			});

			return (Convert.ToInt32(result) > 0);
		}

		/// <summary>
		/// 导出分页分页
		/// </summary>
		/// <param name="model"></param>
		/// <returns></returns>
		public async Task<TableDataModel> GetExportData(SmtStationRequestModel model)
		{
			string sql = @"  SELECT ROW_NUMBER() OVER(ORDER BY ST.ID DESC) AS ROWNO, ST.ID,SMT_NAME,LP.VALUE TYPE,ST.DESCRIPTION,ST.ENABLED FROM SMT_STATION ST
							 LEFT JOIN SMT_LOOKUP LP ON LP.CODE=ST.TYPE AND LP.TYPE = 'STATION_KIND' ";

			string conditions = " WHERE ST.ID > 0 ";

			if (!model.SMT_NAME.IsNullOrWhiteSpace())
			{
				conditions += $" AND INSTR(ST.SMT_NAME, :SMT_NAME) > 0";
			}
			if (!model.TYPE.IsNullOrWhiteSpace())
			{
				conditions += $" AND ST.TYPE=:TYPE";
			}
			if (!model.DESCRIPTION.IsNullOrWhiteSpace())
			{
				conditions += $" AND INSTR(ST.DESCRIPTION, :DESCRIPTION) > 0";
			}
			if (!model.ENABLED.IsNullOrWhiteSpace())
			{
				conditions += $" AND ST.ENABLED=:ENABLED";
			}

			string pagedSql = SQLBuilderClass.GetPagedSQL(sql, "ST.ID DESC", conditions);
			var resdata = await _dbConnection.QueryAsync<object>(pagedSql, model);
			string sqlcnt = @" SELECT COUNT(ST.ID) FROM SMT_STATION ST
							 LEFT JOIN SMT_LOOKUP LP ON LP.CODE=ST.TYPE AND LP.TYPE = 'STATION_KIND' " + conditions;

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
		public async Task<decimal> SaveDataByTrans(SmtStationModel model)
		{
			int result = 1;
			ConnectionFactory.OpenConnection(_dbConnection);
			using (var tran = _dbConnection.BeginTransaction())
			{
				try
				{
					//新增
					string insertSql = @"INSERT INTO SMT_STATION 
					(ID,SMT_NAME,TYPE,DESCRIPTION,ENABLED) 
					VALUES (:ID,:SMT_NAME,:TYPE,:DESCRIPTION,:ENABLED)";
					if (model.insertRecords != null && model.insertRecords.Count > 0)
					{
						foreach (var item in model.insertRecords)
						{
							var newid = await GetID("SEQ_ID");
							var resdata = await _dbConnection.ExecuteAsync(insertSql, new
							{
								ID = newid,
								item.SMT_NAME,
								item.TYPE,
								item.DESCRIPTION,
								item.ENABLED,							}, tran);
						}
					}
					//更新
					string updateSql = @"Update SMT_STATION set SMT_NAME=:SMT_NAME,TYPE=:TYPE,DESCRIPTION=:DESCRIPTION,ENABLED=:ENABLED  
						where ID=:ID ";
					if (model.updateRecords != null && model.updateRecords.Count > 0)
					{
						foreach (var item in model.updateRecords)
						{
							var resdata = await _dbConnection.ExecuteAsync(updateSql, new
							{
								item.ID,
								item.SMT_NAME,
								item.TYPE,
								item.DESCRIPTION,
								item.ENABLED,							}, tran);
						}
					}
					//删除
					string deleteSql = @"Delete from SMT_STATION where ID=:ID ";
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


		/// <summary>
		///获取机器类型
		/// </summary>
		/// <returns></returns>
		public async Task<List<IDNAME>> GetStatus()
		{
			string sql = "SELECT CODE ID,VALUE NAME FROM SMT_LOOKUP WHERE TYPE = 'STATION_KIND'  order by VALUE ";
			var result = await _dbConnection.QueryAsync<IDNAME>(sql);
			return result?.ToList();
		}

	}
}