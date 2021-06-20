/*
*┌──────────────────────────────────────────────────────────────┐
*│　描    述：接口实现                                                    
*│　作    者：嘉志科技                                            
*│　版    本：2.0    模板代码自动生成                                                
*│　创建时间：2020-09-11 14:06:34                             
*└──────────────────────────────────────────────────────────────┘
*┌──────────────────────────────────────────────────────────────┐
*│　命名空间： JZ.IMS.Repository.Oracle                                  
*│　类    名： SfcsCollectUidsRepository                                      
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
    public class SfcsCollectUidsRepository:BaseRepository<SfcsCollectUids,Decimal>, ISfcsCollectUidsRepository
    {
        public SfcsCollectUidsRepository(IOptionsSnapshot<DbOption> options)
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
			string sql = "SELECT ENABLED FROM SFCS_COLLECT_UIDS WHERE ID=:ID";
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
			string sql = "UPDATE SFCS_COLLECT_UIDS set ENABLED=:ENABLED WHERE ID=:Id";
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
			string sql = "SELECT SFCS_COLLECT_UIDS_SEQ.NEXTVAL MY_SEQ FROM DUAL";
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
			string sql = "select count(0) from SFCS_COLLECT_UIDS where id = :id";
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
		//public async Task<decimal> SaveDataByTrans(SfcsCollectUidsModel model)
		//{
		//	int result = 1;
		//	ConnectionFactory.OpenConnection(_dbConnection);
		//	using (var tran = _dbConnection.BeginTransaction())
		//	{
		//		try
		//		{
		//			//新增
		//			string insertSql = @"insert into SFCS_COLLECT_UIDS 
		//			(COLLECT_UID_ID,OPERATION_ID,SN_ID,WO_ID,PRODUCT_OPERATION_CODE,UID_ID,UID_NAME,UID_NUMBER,PLANT_CODE,UID_QTY,ORDER_NO,SERIALIZED,REWORK_REMOVE_FLAG,REPLACE_FLAG,EDI_FLAG,COLLECT_SITE,COLLECT_BY,COLLECT_TIME) 
		//			VALUES (:COLLECT_UID_ID,:OPERATION_ID,:SN_ID,:WO_ID,:PRODUCT_OPERATION_CODE,:UID_ID,:UID_NAME,:UID_NUMBER,:PLANT_CODE,:UID_QTY,:ORDER_NO,:SERIALIZED,:REWORK_REMOVE_FLAG,:REPLACE_FLAG,:EDI_FLAG,:COLLECT_SITE,:COLLECT_BY,:COLLECT_TIME)";
		//			if (model.InsertRecords != null && model.InsertRecords.Count > 0)
		//			{
		//				foreach (var item in model.InsertRecords)
		//				{
		//					var newid = await GetSEQID();
		//					var resdata = await _dbConnection.ExecuteAsync(insertSql, new
		//					{
		//						ID = newid,
		//						item.COLLECT_UID_ID,
		//						item.OPERATION_ID,
		//						item.SN_ID,
		//						item.WO_ID,
		//						item.PRODUCT_OPERATION_CODE,
		//						item.UID_ID,
		//						item.UID_NAME,
		//						item.UID_NUMBER,
		//						item.PLANT_CODE,
		//						item.UID_QTY,
		//						item.ORDER_NO,
		//						item.SERIALIZED,
		//						item.REWORK_REMOVE_FLAG,
		//						item.REPLACE_FLAG,
		//						item.EDI_FLAG,
		//						item.COLLECT_SITE,
		//						item.COLLECT_BY,
		//						item.COLLECT_TIME,
		//					}, tran);
		//				}
		//			}
		//			//更新
		//			string updateSql = @"Update SFCS_COLLECT_UIDS set COLLECT_UID_ID=:COLLECT_UID_ID,OPERATION_ID=:OPERATION_ID,SN_ID=:SN_ID,WO_ID=:WO_ID,PRODUCT_OPERATION_CODE=:PRODUCT_OPERATION_CODE,UID_ID=:UID_ID,UID_NAME=:UID_NAME,UID_NUMBER=:UID_NUMBER,PLANT_CODE=:PLANT_CODE,UID_QTY=:UID_QTY,ORDER_NO=:ORDER_NO,SERIALIZED=:SERIALIZED,REWORK_REMOVE_FLAG=:REWORK_REMOVE_FLAG,REPLACE_FLAG=:REPLACE_FLAG,EDI_FLAG=:EDI_FLAG,COLLECT_SITE=:COLLECT_SITE,COLLECT_BY=:COLLECT_BY,COLLECT_TIME=:COLLECT_TIME  
		//				where ID=:ID ";
		//			if (model.UpdateRecords != null && model.UpdateRecords.Count > 0)
		//			{
		//				foreach (var item in model.UpdateRecords)
		//				{
		//					var resdata = await _dbConnection.ExecuteAsync(updateSql, new
		//					{
		//						item.COLLECT_UID_ID,
		//						item.OPERATION_ID,
		//						item.SN_ID,
		//						item.WO_ID,
		//						item.PRODUCT_OPERATION_CODE,
		//						item.UID_ID,
		//						item.UID_NAME,
		//						item.UID_NUMBER,
		//						item.PLANT_CODE,
		//						item.UID_QTY,
		//						item.ORDER_NO,
		//						item.SERIALIZED,
		//						item.REWORK_REMOVE_FLAG,
		//						item.REPLACE_FLAG,
		//						item.EDI_FLAG,
		//						item.COLLECT_SITE,
		//						item.COLLECT_BY,
		//						item.COLLECT_TIME,
		//					}, tran);
		//				}
		//			}
		//			//删除
		//			string deleteSql = @"Delete from SFCS_COLLECT_UIDS where ID=:ID ";
		//			if (model.RemoveRecords != null && model.RemoveRecords.Count > 0)
		//			{
		//				foreach (var item in model.RemoveRecords)
		//				{
		//					var resdata = await _dbConnection.ExecuteAsync(deleteSql, new
		//					{
		//						item.ID
		//					}, tran);
		//				}
		//			}

		//			tran.Commit();
		//		}
		//		catch (Exception ex)
		//		{
		//			result = -1;
		//			tran.Rollback();
		//			throw ex;
		//		}
		//		finally
		//		{
		//			if (_dbConnection.State != System.Data.ConnectionState.Closed)
		//			{
		//				_dbConnection.Close();
		//			}
		//		}
		//	}
		//	return result;
		//}
    }
}