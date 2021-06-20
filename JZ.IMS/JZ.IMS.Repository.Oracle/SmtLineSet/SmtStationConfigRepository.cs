/**
*┌──────────────────────────────────────────────────────────────┐
*│　描    述：接口实现                                                    
*│　作    者：嘉志科技                                            
*│　版    本：2.0    模板代码自动生成                                                
*│　创建时间：2020-03-18 16:12:05                             
*└──────────────────────────────────────────────────────────────┘
*┌──────────────────────────────────────────────────────────────┐
*│　命名空间： JZ.IMS.Repository.Oracle                                  
*│　类    名： SmtStationConfigRepository                                      
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
    public class SmtStationConfigRepository:BaseRepository<SmtStationConfig,Decimal>, ISmtStationConfigRepository
    {
        public SmtStationConfigRepository(IOptionsSnapshot<DbOption> options)
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
			string sql = "SELECT ENABLED FROM SMT_STATION_CONFIG WHERE ID=:ID";
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
			string sql = "UPDATE SMT_STATION_CONFIG set ENABLED=:ENABLED WHERE ID=:Id";
			return await _dbConnection.ExecuteAsync(sql, new
			{
				ENABLED = status ? 'Y' : 'N',
				Id = id,
			});
		}

        /// <summary>
		///项目是否已被使用 
		/// </summary>
		/// <param name="id">项目id</param>
		/// <returns></returns>
		public async Task<bool> ItemIsByUsed(decimal id)
		{
			string sql = "select count(0) from SMT_STATION_CONFIG where id = :id";
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
		public async Task<TableDataModel> GetExportData(SmtStationConfigRequestModel model)
		{
			string sql = @"  SELECT  ROW_NUMBER() OVER(ORDER BY SC.ID DESC) AS ROWNO,SC.ID,SN.SMT_NAME STATION_ID,LP.CN_DESC CONFIG_TYPE,SC.VALUE,SC.DESCRIPTION,SC.ENABLED,SC.MODULE FROM SMT_STATION_CONFIG SC
							 LEFT JOIN SMT_STATION SN ON SN.ID=SC.STATION_ID
							 LEFT JOIN SMT_LOOKUP LP ON LP.CODE=SC.CONFIG_TYPE AND  LP.TYPE='MACHINECONFIG' ";

			string conditions = " WHERE SC.ID > 0 ";

			if (model.STATION_ID != null && model.STATION_ID > 0)
			{
				conditions += $" AND SC.STATION_ID=:STATION_ID ";
			}
			if (model.CONFIG_TYPE != null && model.CONFIG_TYPE > 0)
			{
				conditions += $" AND SC.CONFIG_TYPE=:CONFIG_TYPE ";
			}
			if (!model.VALUE.IsNullOrWhiteSpace())
			{
				conditions += $" AND INSTR(SC.VALUE, :VALUE) > 0 ";
			}
			if (!model.DESCRIPTION.IsNullOrWhiteSpace())
			{
				conditions += $" AND INSTR(SC.DESCRIPTION, :DESCRIPTION) > 0 ";
			}
			if (!model.ENABLED.IsNullOrWhiteSpace())
			{
				conditions += $" AND SC.ENABLED=:ENABLED ";
			}

			string pagedSql = SQLBuilderClass.GetPagedSQL(sql, conditions);
			var resdata = await _dbConnection.QueryAsync<object>(pagedSql, model);
			string sqlcnt = @" SELECT COUNT(SC.ID) FROM SMT_STATION_CONFIG SC
							 LEFT JOIN SMT_STATION SN ON SN.ID=SC.STATION_ID
							 LEFT JOIN SMT_LOOKUP LP ON LP.CODE=SC.CONFIG_TYPE AND  LP.TYPE='MACHINECONFIG' " + conditions;

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
		public async Task<decimal> SaveDataByTrans(SmtStationConfigModel model)
		{
			int result = 1;
			ConnectionFactory.OpenConnection(_dbConnection);
			using (var tran = _dbConnection.BeginTransaction())
			{
				try
				{
					//新增
					string insertSql = @"INSERT INTO SMT_STATION_CONFIG 
					(ID,STATION_ID,CONFIG_TYPE,VALUE,DESCRIPTION,ENABLED,MODULE) 
					VALUES (:ID,:STATION_ID,:CONFIG_TYPE,:VALUE,:DESCRIPTION,:ENABLED,:MODULE)";
					if (model.insertRecords != null && model.insertRecords.Count > 0)
					{
						foreach (var item in model.insertRecords)
						{
							var newid = await GetID("SEQ_ID");
							var resdata = await _dbConnection.ExecuteAsync(insertSql, new
							{
								ID = newid,
								item.STATION_ID,
								item.CONFIG_TYPE,
								item.VALUE,
								item.DESCRIPTION,
								item.ENABLED,
								item.MODULE,							}, tran);
						}
					}
					//更新
					string updateSql = @"Update SMT_STATION_CONFIG set STATION_ID=:STATION_ID,CONFIG_TYPE=:CONFIG_TYPE,VALUE=:VALUE,DESCRIPTION=:DESCRIPTION,ENABLED=:ENABLED,MODULE=:MODULE  
						where ID=:ID ";
					if (model.updateRecords != null && model.updateRecords.Count > 0)
					{
						foreach (var item in model.updateRecords)
						{
							var resdata = await _dbConnection.ExecuteAsync(updateSql, new
							{
								item.ID,
								item.STATION_ID,
								item.CONFIG_TYPE,
								item.VALUE,
								item.DESCRIPTION,
								item.ENABLED,
								item.MODULE,							}, tran);
						}
					}
					//删除
					string deleteSql = @"Delete from SMT_STATION_CONFIG where ID=:ID ";
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
		///获取配置类型列表
		/// </summary>
		/// <returns></returns>
		public async Task<List<T>> GetStatus<T>()
		{
			string sql = " SELECT CODE, VALUE,CN_DESC FROM SMT_LOOKUP WHERE TYPE='MACHINECONFIG' order by CN_DESC ";
			var result = await _dbConnection.QueryAsync<T>(sql);
			return result?.ToList<T>();
		}



		/// <summary>
		///获取机台列表
		/// </summary>
		/// <returns></returns>
		public async Task<List<IDNAME>> GetMachineList()
		{
			string sql = " SELECT ID, SMT_NAME NAME FROM SMT_STATION order by SMT_NAME";
			var result = await _dbConnection.QueryAsync<IDNAME>(sql);
			return result?.ToList();
		}



	}
}