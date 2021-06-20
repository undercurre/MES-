/*
*┌──────────────────────────────────────────────────────────────┐
*│　描    述：接口实现                                                    
*│　作    者：嘉志科技                                            
*│　版    本：2.0    模板代码自动生成                                                
*│　创建时间：2020-06-15 10:42:16                             
*└──────────────────────────────────────────────────────────────┘
*┌──────────────────────────────────────────────────────────────┐
*│　命名空间： JZ.IMS.Repository.Oracle                                  
*│　类    名： ImsMsdR12Repository                                      
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
    public class ImsMsdR12Repository:BaseRepository<ImsMsdR12,Decimal>, IImsMsdR12Repository
    {
        public ImsMsdR12Repository(IOptionsSnapshot<DbOption> options)
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
			string sql = "SELECT ENABLED FROM IMS_MSD_R12 WHERE ID=:ID";
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
			string sql = "UPDATE IMS_MSD_R12 set ENABLED=:ENABLED WHERE ID=:Id";
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
			string sql = "SELECT IMS_MSD_R12_SEQ.NEXTVAL MY_SEQ FROM DUAL";
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
			string sql = "select count(0) from IMS_MSD_R12 where id = :id";
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
		public async Task<decimal> SaveDataByTrans(ImsMsdR12Model model)
		{
			int result = 1;
			ConnectionFactory.OpenConnection(_dbConnection);
			using (var tran = _dbConnection.BeginTransaction())
			{
				try
				{
					//新增
					string insertSql = @"insert into IMS_MSD_R12 
					(ID,ORGANIZATION_CODE,PART_CODE,DESCRIPTION,MAKER_CODE,LEVEL_CODE,PART_THICKNESS,MSD_REEL,MSD_SPECIAL,MSD_TIME,MSD_TRAY,CREATION_DATE,LAST_UPDATE_DATE,ENABLED) 
					VALUES (:ID,:ORGANIZATION_CODE,:PART_CODE,:DESCRIPTION,:MAKER_CODE,:LEVEL_CODE,:PART_THICKNESS,:MSD_REEL,:MSD_SPECIAL,:MSD_TIME,:MSD_TRAY,:CREATION_DATE,:LAST_UPDATE_DATE,:ENABLED)";
					if (model.InsertRecords != null && model.InsertRecords.Count > 0)
					{
						foreach (var item in model.InsertRecords)
						{
							var newid = await GetID();
							var resdata = await _dbConnection.ExecuteAsync(insertSql, new
							{
								ID = newid,
								item.ORGANIZATION_CODE,
								item.PART_CODE,
								item.DESCRIPTION,
								item.MAKER_CODE,
								item.LEVEL_CODE,
								item.PART_THICKNESS,
								item.MSD_REEL,
								item.MSD_SPECIAL,
								item.MSD_TIME,
								item.MSD_TRAY,
								item.CREATION_DATE,
								item.LAST_UPDATE_DATE,
								item.ENABLED,
							}, tran);
						}
					}
					//更新
					string updateSql = @"Update IMS_MSD_R12 set ORGANIZATION_CODE=:ORGANIZATION_CODE,PART_CODE=:PART_CODE,DESCRIPTION=:DESCRIPTION,MAKER_CODE=:MAKER_CODE,LEVEL_CODE=:LEVEL_CODE,PART_THICKNESS=:PART_THICKNESS,MSD_REEL=:MSD_REEL,MSD_SPECIAL=:MSD_SPECIAL,MSD_TIME=:MSD_TIME,MSD_TRAY=:MSD_TRAY,CREATION_DATE=:CREATION_DATE,LAST_UPDATE_DATE=:LAST_UPDATE_DATE,ENABLED=:ENABLED  
						where ID=:ID ";
					if (model.UpdateRecords != null && model.UpdateRecords.Count > 0)
					{
						foreach (var item in model.UpdateRecords)
						{
							var resdata = await _dbConnection.ExecuteAsync(updateSql, new
							{
								item.ID,
								item.ORGANIZATION_CODE,
								item.PART_CODE,
								item.DESCRIPTION,
								item.MAKER_CODE,
								item.LEVEL_CODE,
								item.PART_THICKNESS,
								item.MSD_REEL,
								item.MSD_SPECIAL,
								item.MSD_TIME,
								item.MSD_TRAY,
								item.CREATION_DATE,
								item.LAST_UPDATE_DATE,
								item.ENABLED,
							}, tran);
						}
					}
					//删除
					string deleteSql = @"Delete from IMS_MSD_R12 where ID=:ID ";
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