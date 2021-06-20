/**
*┌──────────────────────────────────────────────────────────────┐
*│　描    述：接口实现                                                    
*│　作    者：嘉志科技                                            
*│　版    本：2.0    模板代码自动生成                                                
*│　创建时间：2020-03-03 10:54:23                             
*└──────────────────────────────────────────────────────────────┘
*┌──────────────────────────────────────────────────────────────┐
*│　命名空间： JZ.IMS.Repository.Oracle                                  
*│　类    名： SfcsScraperConfigRepository                                      
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
    public class SfcsScraperConfigRepository:BaseRepository<SfcsScraperConfig,decimal>, ISfcsScraperConfigRepository
    {
        public SfcsScraperConfigRepository(IOptionsSnapshot<DbOption> options)
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
			string sql = "SELECT ENABLED FROM SFCS_SCRAPER_CONFIG WHERE ID=:ID";
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
			string sql = "UPDATE SFCS_SCRAPER_CONFIG set ENABLED=:ENABLED WHERE ID=:Id";
			return await _dbConnection.ExecuteAsync(sql, new
			{
				ENABLED = status ? 'Y' : 'N',
				Id = id,
			});
		}

		/// <summary>
		/// 查询列表
		/// </summary>
		/// <param name="model"></param>
		/// <returns></returns>
		public async Task<TableDataModel> LoadData(SfcsScraperConfigRequestModel model)
		{
			string conditions = " WHERE m.ID > 0 ";
			if (!model.Key.IsNullOrWhiteSpace())
			{
				conditions += $"and (instr(m.SCRAPER_NO, :Key) > 0 or instr(m.LOCATION, :Key) > 0 or instr(m.CREATOR, :Key) > 0 )";
			}
			string sql = @"SELECT ROWNUM AS ROWNO, M.ID, M.SCRAPER_NO, M.LOCATION, M.CREATOR, M.CREATE_TIME, M.ENABLED, M.MAX_USE_FLAG, M.ALARM_HOURS, 
						      M.STOP_HOURS, M.INTERVAL, M.LAST_CLEAN_TIME, M.LAST_ALARM_TIME, OZ.ORGANIZE_NAME ORGANIZE_ID ,M.ORGANIZE_ID O_ID  
				           FROM SFCS_SCRAPER_CONFIG M INNER JOIN (SELECT DISTINCT T.* FROM SYS_ORGANIZE T START WITH T.ID IN (SELECT ORGANIZE_ID FROM 
                             SYS_USER_ORGANIZE WHERE MANAGER_ID=:USER_ID) CONNECT BY PRIOR T.ID=T.PARENT_ORGANIZE_ID) OZ ON M.ORGANIZE_ID = OZ.ID ";
			string pagedSql = SQLBuilderClass.GetPagedSQL(sql, "m.id", conditions);
			var resdata = await _dbConnection.QueryAsync<object>(pagedSql, model);

			string sqlcnt = @" select count(0) from sfcs_scraper_config m inner join (select distinct t.* from SYS_ORGANIZE t start with t.id in (select organize_id from 
                             sys_user_organize where manager_id=:USER_ID) connect by prior t.id=t.PARENT_ORGANIZE_ID) oz on m.organize_id = oz.ID " + conditions;
			int cnt = await _dbConnection.ExecuteScalarAsync<int>(sqlcnt, model);
			return new TableDataModel
			{
				count = cnt,
				data = resdata?.ToList(),
			};
		}

		/// <summary>
		///刮刀号是否已被使用 
		/// </summary>
		/// <param name="SCRAPER_NO">刮刀号</param>
		/// <returns></returns>
		public async Task<bool> ItemIsByUsed(string SCRAPER_NO)
		{
			int cnt = 0;
			object result = 0;
			//刮刀作业表 
			string sql = "select count(0) from sfcs_scraper_runcard where SCRAPER_NO = :SCRAPER_NO";
			result = await _dbConnection.ExecuteScalarAsync(sql, new
			{
				SCRAPER_NO
			});

			cnt = Convert.ToInt32(result);
			/*if (cnt == 0)
			{
				//其他
				sql = " = :id";
				result = await _dbConnection.ExecuteScalarAsync(sql, new
				{
					id
				});
				cnt = Convert.ToInt32(result);
			}*/
			return (cnt > 0);
		}

		/// <summary>
		/// 保存数据
		/// </summary>
		/// <param name="model"></param>
		/// <returns></returns>
		public async Task<decimal> SaveDataByTrans(SfcsScraperConfigModel model)
		{
			int result = 1;
			ConnectionFactory.OpenConnection(_dbConnection);
			using (var tran = _dbConnection.BeginTransaction())
			{
				try
				{
					//新增
					string insertSql = @"INSERT INTO SFCS_SCRAPER_CONFIG(ID, SCRAPER_NO, LOCATION, CREATOR, CREATE_TIME, ENABLED, ALARM_HOURS, STOP_HOURS, INTERVAL, ORGANIZE_ID)
					VALUES (:ID,:SCRAPER_NO, :LOCATION, :CREATOR, :CREATE_TIME, :ENABLED, :ALARM_HOURS, :STOP_HOURS, :INTERVAL, :ORGANIZE_ID)";
					if (model.insertRecords != null && model.insertRecords.Count > 0)
					{
						foreach (var item in model.insertRecords)
						{
							var newid = await GetSEQ_ID();
							var resdata = await _dbConnection.ExecuteAsync(insertSql, new
							{
								ID = newid,
								item.SCRAPER_NO,
								item.LOCATION,
								item.CREATOR,
								item.CREATE_TIME,
								item.ENABLED,
								item.ALARM_HOURS,
								item.STOP_HOURS,
								item.INTERVAL,
								item.ORGANIZE_ID,
							}, tran);
						}
					}
					//更新
					string updateSql = @"update sfcs_scraper_config set SCRAPER_NO =:SCRAPER_NO, LOCATION =:LOCATION,
							ENABLED =:ENABLED, ALARM_HOURS =:ALARM_HOURS, STOP_HOURS =:STOP_HOURS, INTERVAL =:INTERVAL, ORGANIZE_ID =:ORGANIZE_ID,CREATE_TIME=:CREATE_TIME 
						where ID=:ID ";
					if (model.updateRecords != null && model.updateRecords.Count > 0)
					{
						foreach (var item in model.updateRecords)
						{
							var resdata = await _dbConnection.ExecuteAsync(updateSql, new
							{
								item.ID,
								item.SCRAPER_NO,
								item.LOCATION,
								item.ENABLED,
								item.ALARM_HOURS,
								item.STOP_HOURS,
								item.INTERVAL,
								item.CREATE_TIME,
								item.ORGANIZE_ID,
							}, tran);
						}
					}
					//删除
					string deleteSql = @"delete from sfcs_scraper_config where ID=:ID ";
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