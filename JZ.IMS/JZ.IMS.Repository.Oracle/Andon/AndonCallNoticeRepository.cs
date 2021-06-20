/**
*┌──────────────────────────────────────────────────────────────┐
*│　描    述：呼叫通知内容接口实现                                                    
*│　作    者：嘉志科技                                            
*│　版    本：2.0    模板代码自动生成                                                
*│　创建时间：2019-09-23 22:15:35                             
*└──────────────────────────────────────────────────────────────┘
*┌──────────────────────────────────────────────────────────────┐
*│　命名空间： JZ.IMS.Repository.Oracle                                  
*│　类    名： AndonCallNoticeRepository                                      
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
using System.Collections.Generic;
using JZ.IMS.ViewModels;

namespace JZ.IMS.Repository.Oracle
{
	public class AndonCallNoticeRepository : BaseRepository<AndonCallNotice, Decimal>, IAndonCallNoticeRepository
	{
		public AndonCallNoticeRepository(IOptionsSnapshot<DbOption> options)
		{
			_dbOption = options.Get("iWMS");
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
			string sql = "SELECT ENABLED FROM ANDON_CALL_NOTICE WHERE ID=:ID AND IS_DELETE='N'";
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
			string sql = "update ANDON_CALL_NOTICE set ENABLED=:ENABLED where  Id=:Id";
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
			string sql = "SELECT ANDON_CALL_NOTICE_SEQ.NEXTVAL MY_SEQ FROM DUAL";
			var result = await _dbConnection.ExecuteScalarAsync(sql);
			return (decimal)result;
		}

		/// <summary>
		/// 获取通知列表
		/// </summary>
		/// <param name="id"></param>
		/// <returns></returns>
		public async Task<IEnumerable<AndonCallNoticeListModel>> GetListByIdAsync(decimal id)
		{
			//string sql = " SELECT A.*, B.NOTICE_RECEIVER " +
			//"FROM ANDON_CALL_NOTICE A " +
			//"INNER JOIN(SELECT MST_ID, wm_concat (NOTICE_RECEIVER) AS NOTICE_RECEIVER " +
			//"FROM (SELECT MST_ID, NVL(USER_NAME,'') || '[' || NVL(NOTICE_TYPE,'') || ':' || NVL(NOTICE_ACCOUNT,'') || ']' AS NOTICE_RECEIVER " +
			//"FROM ANDON_CALL_NOTICE_RECEIVER) T GROUP BY MST_ID) B  " +
			//"ON A.ID = B.MST_ID " +
			//" WHERE A.MST_ID = :Id ";
			string sql = @" SELECT A.*, B.NOTICE_RECEIVER FROM ANDON_CALL_NOTICE A INNER JOIN(
 SELECT MST_ID, NOTICE_RECEIVER AS NOTICE_RECEIVER FROM (
 SELECT MST_ID, NVL(USER_NAME,'') || '[' || NVL(NOTICE_TYPE,'') || ':' || NVL(NOTICE_ACCOUNT,'') || ']' AS NOTICE_RECEIVER 
FROM ANDON_CALL_NOTICE_RECEIVER) T GROUP BY MST_ID,NOTICE_RECEIVER) B  ON A.ID = B.MST_ID  WHERE A.MST_ID = :Id ";
			return await _dbConnection.QueryAsync<AndonCallNoticeListModel>(sql, new
			{
				Id = id,
			});
		}

		/// <summary>
		/// 获取通知列表
		/// </summary>
		/// <param name="AndonCallNoticeRequestModel"></param>
		/// <returns></returns>
		public async Task<IEnumerable<AndonCallNoticeListExModel>> GetListByModelAsync(AndonCallNoticeRequestModel model, string sqlWhere)
		{
			string sql = "SELECT ROW_NUMBER() OVER(ORDER BY A.ID DESC) AS PagedNumber," +
				"A.*,B.CALL_NO,B.OPERATION_LINE_ID,B.CALL_TYPE_CODE,B.OPERATION_LINE_NAME,'' AS CALL_TYPE_NAME " +
				"FROM ANDON_CALL_NOTICE A " +
				"INNER JOIN ANDON_CALL_RECORD B ON A.MST_ID = B.ID " +
				"WHERE A.ID > 0 " + sqlWhere;

			string sqlPage = "SELECT * FROM(" + sql + ") u " +
				"WHERE PagedNumber BETWEEN((" + model.Page + "-1) * " + model.Limit + "+1) AND(" + model.Page + "* " + model.Limit + ")";

			return await _dbConnection.QueryAsync<AndonCallNoticeListExModel>(sqlPage, model);
		}

		/// <summary>
		/// 获取通知列表
		/// </summary>
		/// <param name="AndonCallNoticeRequestModel"></param>
		/// <returns></returns>
		public async Task<int> GetCountByModelAsync(AndonCallNoticeRequestModel model, string sqlWhere)
		{
			string sql = "SELECT count(*) " +
				"FROM ANDON_CALL_NOTICE A " +
				"INNER JOIN ANDON_CALL_RECORD B ON A.MST_ID = B.ID " +
				"WHERE A.ID > 0 " + sqlWhere;

			return await _dbConnection.ExecuteScalarAsync<int>(sql, model);
		}
	}
}