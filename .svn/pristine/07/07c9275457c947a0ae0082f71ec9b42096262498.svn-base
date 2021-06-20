/*
*┌──────────────────────────────────────────────────────────────┐
*│　描    述：镭雕记录接口实现                                                    
*│　作    者：嘉志科技                                            
*│　版    本：2.0    模板代码自动生成                                                
*│　创建时间：2020-11-23 09:15:48                             
*└──────────────────────────────────────────────────────────────┘
*┌──────────────────────────────────────────────────────────────┐
*│　命名空间： JZ.IMS.Repository.Oracle                                  
*│　类    名： SfcsLaserRecordRepository                                      
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
    public class SfcsLaserRecordRepository:BaseRepository<SfcsLaserRecord,Decimal>, ISfcsLaserRecordRepository
    {
        public SfcsLaserRecordRepository(IOptionsSnapshot<DbOption> options)
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
			string sql = "SELECT ENABLED FROM SFCS_LASER_RECORD WHERE ID=:ID";
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
			string sql = "UPDATE SFCS_LASER_RECORD set ENABLED=:ENABLED WHERE ID=:Id";
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
			string sql = "SELECT SFCS_LASER_RECORD_SEQ.NEXTVAL MY_SEQ FROM DUAL";
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
			string sql = "select count(0) from SFCS_LASER_RECORD where id = :id";
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
		public async Task<decimal> SaveDataByTrans(SfcsLaserRecordModel model)
		{
			int result = 1;
			ConnectionFactory.OpenConnection(_dbConnection);
			using (var tran = _dbConnection.BeginTransaction())
			{
				try
				{
                    //新增
                    string insertSql = @"insert into SFCS_LASER_RECORD_TEST 
					(ID,MACHINE_NO,LASER_TIME,LOT_NO,PANEL_ID,SN,CREATE_TIME,CREATE_USER,MULTI_NO,IS_INPUT,INPUT_TIME,IS_INVALID,INVALID_TIME,ATTRIBUTE1,ATTRIBUTE2,ATTRIBUTE3,ATTRIBUTE4,ATTRIBUTE5) 
					VALUES (:ID,:MACHINE_NO,:LASER_TIME,:LOT_NO,:PANEL_ID,:SN,SYSDATE,:CREATE_USER,:MULTI_NO,:IS_INPUT,:INPUT_TIME,:IS_INVALID,:INVALID_TIME,:ATTRIBUTE1,:ATTRIBUTE2,:ATTRIBUTE3,:ATTRIBUTE4,:ATTRIBUTE5)";
                    if (model.InsertRecords != null && model.InsertRecords.Count > 0)
					{
						var panel_id = await GetSEQID();
						foreach (var item in model.InsertRecords)
						{
							var newid = await GetSEQID();
							var resdata = await _dbConnection.ExecuteAsync(insertSql, new
							{
								ID = newid,
								item.MACHINE_NO,
								item.LASER_TIME,
								item.LOT_NO,
								PANEL_ID = panel_id,
								item.SN,
								//item.CREATE_TIME,
								item.CREATE_USER,
								item.MULTI_NO,
								item.IS_INPUT,
								item.INPUT_TIME,
								item.IS_INVALID,
								item.INVALID_TIME,
								item.ATTRIBUTE1,
								item.ATTRIBUTE2,
								item.ATTRIBUTE3,
								item.ATTRIBUTE4,
								item.ATTRIBUTE5,
							}, tran);
						}
					}
					//更新
					string updateSql = @"Update SFCS_LASER_RECORD_TEST set MACHINE_NO=:MACHINE_NO,LASER_TIME=:LASER_TIME,LOT_NO=:LOT_NO,PANEL_ID=:PANEL_ID,SN=:SN,CREATE_TIME=:CREATE_TIME,CREATE_USER=:CREATE_USER,MULTI_NO=:MULTI_NO,IS_INPUT=:IS_INPUT,INPUT_TIME=:INPUT_TIME,IS_INVALID=:IS_INVALID,INVALID_TIME=:INVALID_TIME,ATTRIBUTE1=:ATTRIBUTE1,ATTRIBUTE2=:ATTRIBUTE2,ATTRIBUTE3=:ATTRIBUTE3,ATTRIBUTE4=:ATTRIBUTE4,ATTRIBUTE5=:ATTRIBUTE5  
						where ID=:ID ";
					if (model.UpdateRecords != null && model.UpdateRecords.Count > 0)
					{
						foreach (var item in model.UpdateRecords)
						{
							var resdata = await _dbConnection.ExecuteAsync(updateSql, new
							{
								item.ID,
								item.MACHINE_NO,
								item.LASER_TIME,
								item.LOT_NO,
								item.PANEL_ID,
								item.SN,
								item.CREATE_TIME,
								item.CREATE_USER,
								item.MULTI_NO,
								item.IS_INPUT,
								item.INPUT_TIME,
								item.IS_INVALID,
								item.INVALID_TIME,
								item.ATTRIBUTE1,
								item.ATTRIBUTE2,
								item.ATTRIBUTE3,
								item.ATTRIBUTE4,
								item.ATTRIBUTE5,
							}, tran);
						}
					}
					//删除
					string deleteSql = @"Delete from SFCS_LASER_RECORD_TEST where ID=:ID ";
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