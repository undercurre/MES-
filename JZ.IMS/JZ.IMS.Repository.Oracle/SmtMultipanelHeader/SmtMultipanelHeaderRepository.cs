/*
*┌──────────────────────────────────────────────────────────────┐
*│　描    述：连板拆板头部信息接口实现                                                    
*│　作    者：嘉志科技                                            
*│　版    本：2.0    模板代码自动生成                                                
*│　创建时间：2020-11-23 09:58:09                             
*└──────────────────────────────────────────────────────────────┘
*┌──────────────────────────────────────────────────────────────┐
*│　命名空间： JZ.IMS.Repository.Oracle                                  
*│　类    名： SmtMultipanelHeaderRepository                                      
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
    public class SmtMultipanelHeaderRepository:BaseRepository<SmtMultipanelHeader,Decimal>, ISmtMultipanelHeaderRepository
    {
        public SmtMultipanelHeaderRepository(IOptionsSnapshot<DbOption> options)
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
			string sql = "SELECT ENABLED FROM SMT_MULTIPANEL_HEADER WHERE ID=:ID";
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
			string sql = "UPDATE SMT_MULTIPANEL_HEADER set ENABLED=:ENABLED WHERE ID=:Id";
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
			string sql = "SELECT MULTIPANEL_SEQ.NEXTVAL MY_SEQ FROM DUAL";
			var result = await _dbConnection.ExecuteScalarAsync(sql);
			return (decimal)result;
		}

		/// <summary>
		/// 获取明细表的序列
		/// </summary>
		/// <returns></returns>
		public async Task<decimal> GetDetailSEQID()
		{
			string sql = "SELECT MULTIPANEL_SEQ.NEXTVAL MY_SEQ FROM DUAL";
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
			string sql = "select count(0) from SMT_MULTIPANEL_HEADER where id = :id";
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
		public async Task<decimal> SaveDataByTrans(SmtMultipanelMstModel model)
		{
			int result = 1;
            decimal mst_id = 0;
			ConnectionFactory.OpenConnection(_dbConnection);
			using (var tran = _dbConnection.BeginTransaction())
			{
				try
				{
					//新增主表
					if (model.MainData != null && model.MainData.ID == 0)
					{
						string insertMstSql = @"insert into SMT_MULTIPANEL_HEADER_TEST 
					(ID,BATCH_NO,MULT_SITE_ID,MULT_SITE_NAME,MULT_OPERATOR,MULT_TIME,SPLIT_SITE_ID,SPLIT_SITE_NAME,SPLIT_OPERATOR,SPLIT_TIME,IS_SPLIT,MULT_NUMBER,WO_NO) 
					VALUES (:ID,:BATCH_NO,:MULT_SITE_ID,:MULT_SITE_NAME,:MULT_OPERATOR,SYSDATE,:SPLIT_SITE_ID,:SPLIT_SITE_NAME,:SPLIT_OPERATOR,:SPLIT_TIME,:IS_SPLIT,:MULT_NUMBER,:WO_NO)";
						if (model.InsertRecords != null && model.InsertRecords.Count > 0)
						{
							mst_id = await GetSEQID();
							var resdata = await _dbConnection.ExecuteAsync(insertMstSql, new
							{
								ID = mst_id,
								model.MainData.BATCH_NO,
								model.MainData.MULT_SITE_ID,
								model.MainData.MULT_SITE_NAME,
								model.MainData.MULT_OPERATOR,
								//model.MainData.MULT_TIME,
								model.MainData.SPLIT_SITE_ID,
								model.MainData.SPLIT_SITE_NAME,
								model.MainData.SPLIT_OPERATOR,
								model.MainData.SPLIT_TIME,
								model.MainData.IS_SPLIT,
								model.MainData.MULT_NUMBER,
								model.MainData.WO_NO,

							}, tran);
						}
					}
					else
					{
						mst_id = model.MainData.ID;
						//更新主表
						string updateMstSql = @"Update SMT_MULTIPANEL_HEADER_TEST set BATCH_NO=:BATCH_NO,MULT_SITE_ID=:MULT_SITE_ID,MULT_SITE_NAME=:MULT_SITE_NAME,MULT_OPERATOR=:MULT_OPERATOR,MULT_TIME=:MULT_TIME,SPLIT_SITE_ID=:SPLIT_SITE_ID,SPLIT_SITE_NAME=:SPLIT_SITE_NAME,SPLIT_OPERATOR=:SPLIT_OPERATOR,SPLIT_TIME=:SPLIT_TIME,IS_SPLIT=:IS_SPLIT,MULT_NUMBER=:MULT_NUMBER,WO_NO=:WO_NO  
						where ID=:ID ";

						var resdata = await _dbConnection.ExecuteAsync(updateMstSql, new
						{
							model.MainData.ID,
							model.MainData.BATCH_NO,
							model.MainData.MULT_SITE_ID,
							model.MainData.MULT_SITE_NAME,
							model.MainData.MULT_OPERATOR,
							model.MainData.MULT_TIME,
							model.MainData.SPLIT_SITE_ID,
							model.MainData.SPLIT_SITE_NAME,
							model.MainData.SPLIT_OPERATOR,
							model.MainData.SPLIT_TIME,
							model.MainData.IS_SPLIT,
							model.MainData.MULT_NUMBER,
							model.MainData.WO_NO,

						}, tran);
					}
					////删除
					//string deleteSql = @"Delete from SMT_MULTIPANEL_HEADER where ID=:ID ";
					//if (model.RemoveRecords != null && model.RemoveRecords.Count > 0)
					//{
					//	foreach (var item in model.RemoveRecords)
					//	{
					//		var resdata = await _dbConnection.ExecuteAsync(deleteSql, new
					//		{
					//			item.ID
					//		}, tran);
					//	}
					//}
					//新增明细
					string insertSql = @"insert into SMT_MULTIPANEL_DETAIL_TEST 
					(ID,MULT_HEADER_ID,SN,CREATETIME) 
					VALUES (:ID,:MULT_HEADER_ID,:SN,SYSDATE)";
					if (model.InsertRecords != null && model.InsertRecords.Count > 0)
					{
						foreach (var item in model.InsertRecords)
						{
							var newid = await GetDetailSEQID();
							var resdata = await _dbConnection.ExecuteAsync(insertSql, new
							{
								ID = newid,
								MULT_HEADER_ID = mst_id,
								item.SN,
								//item.CREATETIME,
							}, tran);
						}
					}
					//更新明细
					string updateSql = @"Update SMT_MULTIPANEL_DETAIL_TEST set MULT_HEADER_ID=:MULT_HEADER_ID,SN=:SN,CREATETIME=:CREATETIME  
						where ID=:ID ";
					if (model.UpdateRecords != null && model.UpdateRecords.Count > 0)
					{
						foreach (var item in model.UpdateRecords)
						{
							var resdata = await _dbConnection.ExecuteAsync(updateSql, new
							{
								item.ID,
								item.MULT_HEADER_ID,
								item.SN,
								item.CREATETIME,
							}, tran);
						}
					}
					//删除明细
					string deleteSql = @"Delete from SMT_MULTIPANEL_DETAIL_TEST where ID=:ID ";
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