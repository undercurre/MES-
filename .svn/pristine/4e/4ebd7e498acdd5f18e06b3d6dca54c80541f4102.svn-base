/*
*┌──────────────────────────────────────────────────────────────┐
*│　描    述：客户SN表接口实现                                                    
*│　作    者：嘉志科技                                            
*│　版    本：2.0    模板代码自动生成                                                
*│　创建时间：2020-12-09 09:49:33                             
*└──────────────────────────────────────────────────────────────┘
*┌──────────────────────────────────────────────────────────────┐
*│　命名空间： JZ.IMS.Repository.Oracle                                  
*│　类    名： ImportRuncardSnRepository                                      
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
    public class ImportRuncardSnRepository:BaseRepository<ImportRuncardSn,Decimal>, IImportRuncardSnRepository
    {
        public ImportRuncardSnRepository(IOptionsSnapshot<DbOption> options)
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
			string sql = "SELECT ENABLED FROM IMPORT_RUNCARD_SN WHERE ID=:ID";
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
			string sql = "UPDATE IMPORT_RUNCARD_SN set ENABLED=:ENABLED WHERE ID=:Id";
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
			string sql = "SELECT IMPORT_RUNCARD_SN_SEQ.NEXTVAL MY_SEQ FROM DUAL";
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
			string sql = "select count(0) from IMPORT_RUNCARD_SN where id = :id";
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
		public async Task<decimal> SaveDataByTrans(ImportRuncardSnModel model)
		{
			int result = 1;
			ConnectionFactory.OpenConnection(_dbConnection);
			using (var tran = _dbConnection.BeginTransaction())
			{
				try
				{
					//新增
					string insertSql = @"insert into IMPORT_RUNCARD_SN 
					(ID,WO_NO,SN,PARENT_SN,ROUTE_ID,ENABLE,CREATE_TIME,CREATE_BY,UPDATE_TIME,UPDATE_BY,ATTRIBUTE1,ATTRIBUTE2,ATTRIBUTE3,ATTRIBUTE4,ATTRIBUTE5,MAIN_CARD_IMEI,MINOR_CARD_IMEI,BT,MAC,MEID) 
					VALUES (:ID,:WO_NO,:SN,:PARENT_SN,:ROUTE_ID,:ENABLE,SYSDATE,:CREATE_BY,SYSDATE,:UPDATE_BY,:ATTRIBUTE1,:ATTRIBUTE2,:ATTRIBUTE3,:ATTRIBUTE4,:ATTRIBUTE5,:MAIN_CARD_IMEI,:MINOR_CARD_IMEI,:BT,:MAC,:MEID)";
					if (model.InsertRecords != null && model.InsertRecords.Count > 0)
					{
						foreach (var item in model.InsertRecords)
						{
							var newid = await GetSEQID();
							var resdata = await _dbConnection.ExecuteAsync(insertSql, new
							{
								ID = newid,
								item.WO_NO,
								item.SN,
								item.PARENT_SN,
								item.ROUTE_ID,
								item.ENABLE,
								//item.CREATE_TIME,
								item.CREATE_BY,
								//item.UPDATE_TIME,
								item.UPDATE_BY,
								item.ATTRIBUTE1,
								item.ATTRIBUTE2,
								item.ATTRIBUTE3,
								item.ATTRIBUTE4,
								item.ATTRIBUTE5,
								item.MAIN_CARD_IMEI,
								item.MINOR_CARD_IMEI,
								item.BT,
								item.MAC,
								item.MEID,
							}, tran);
						}
					}
					//更新
					string updateSql = @"Update IMPORT_RUNCARD_SN set WO_NO=:WO_NO,SN=:SN,PARENT_SN=:PARENT_SN,ROUTE_ID=:ROUTE_ID,ENABLE=:ENABLE,CREATE_TIME=:CREATE_TIME,CREATE_BY=:CREATE_BY,UPDATE_TIME=SYSDATE,UPDATE_BY=:UPDATE_BY,ATTRIBUTE1=:ATTRIBUTE1,ATTRIBUTE2=:ATTRIBUTE2,ATTRIBUTE3=:ATTRIBUTE3,ATTRIBUTE4=:ATTRIBUTE4,ATTRIBUTE5=:ATTRIBUTE5,MAIN_CARD_IMEI=:MAIN_CARD_IMEI,MINOR_CARD_IMEI=:MINOR_CARD_IMEI,BT=:BT,MAC=:MAC,MEID=:MEID  
						where ID=:ID ";
					if (model.UpdateRecords != null && model.UpdateRecords.Count > 0)
					{
						foreach (var item in model.UpdateRecords)
						{
							var resdata = await _dbConnection.ExecuteAsync(updateSql, new
							{
								item.ID,
								item.WO_NO,
								item.SN,
								item.PARENT_SN,
								item.ROUTE_ID,
								item.ENABLE,
								item.CREATE_TIME,
								item.CREATE_BY,
								//item.UPDATE_TIME,
								item.UPDATE_BY,
								item.ATTRIBUTE1,
								item.ATTRIBUTE2,
								item.ATTRIBUTE3,
								item.ATTRIBUTE4,
								item.ATTRIBUTE5,
								item.MAIN_CARD_IMEI,
								item.MINOR_CARD_IMEI,
								item.BT,
								item.MAC,
								item.MEID,
							}, tran);
						}
					}
					//删除
					string deleteSql = @"Delete from IMPORT_RUNCARD_SN where ID=:ID ";
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

		public async Task<string> GetLastImportRuncardSn(string woNo)
		{
			string sql = @"select MAX(ATTRIBUTE1) AS ATTRIBUTE1 FROM IMPORT_RUNCARD_SN WHERE WO_NO = :WO_NO";
			var result = await _dbConnection.ExecuteScalarAsync(sql,
				new
				{
					WO_NO = woNo
				});
			return (string)result;
		}

		public async Task<decimal> GetTotalSnByWoNo(string woNo, string enable)
		{
			string sql = @"select COUNT(1) AS TOTAL FROM IMPORT_RUNCARD_SN WHERE WO_NO = :WO_NO AND ENABLE = :ENABLE";
			var result = await _dbConnection.ExecuteScalarAsync(sql,
				new
				{
					WO_NO = woNo,
					ENABLE = enable
				});
			return (decimal)result;
		}

	}
}