/**
*┌──────────────────────────────────────────────────────────────┐
*│　描    述：首五件检验事项接口实现                                                    
*│　作    者：嘉志科技                                            
*│　版    本：2.0    模板代码自动生成                                                
*│　创建时间：2020-05-11 14:51:23                             
*└──────────────────────────────────────────────────────────────┘
*┌──────────────────────────────────────────────────────────────┐
*│　命名空间： JZ.IMS.Repository.Oracle                                  
*│　类    名： MesFirstCheckItemsRepository                                      
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
using System.Collections.Generic;
using System.Linq;

namespace JZ.IMS.Repository.Oracle
{
	public class MesFirstCheckItemsRepository : BaseRepository<MesFirstCheckItems, Decimal>, IMesFirstCheckItemsRepository
	{
		public MesFirstCheckItemsRepository(IOptionsSnapshot<DbOption> options)
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
			string sql = "SELECT ENABLED FROM MES_FIRST_CHECK_ITEMS WHERE ID=:ID";
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
		public async Task<decimal> ChangeEnableStatus(decimal id, bool status, string user)
		{
			string sql = "UPDATE MES_FIRST_CHECK_ITEMS set ENABLED=:ENABLED,UPDATE_USER=:UPDATE_USER,UPDATE_TIME = SYSDATE WHERE ID=:Id";
			return await _dbConnection.ExecuteAsync(sql, new
			{
				UPDATE_USER = user,
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
			string sql = "SELECT MES_FIRST_CHECK_ITEMS_SEQ.NEXTVAL MY_SEQ FROM DUAL";
			var result = await _dbConnection.ExecuteScalarAsync(sql);
			return (decimal)result;
		}

		/// <summary>
		/// 获取可用的检验项目
		/// </summary>
		/// <param name="organizeId"></param>
		/// <returns></returns>
		public List<MesFirstCheckItemsListModel> GetItemsData(string organizeId)
		{
			string sql = @" SELECT I.ID AS ITEM_ID,I.CHECK_ITEM,I.CHECK_GIST, P.MEANING AS CHECK_TYPE_NAME
							FROM MES_FIRST_CHECK_ITEMS I
								 LEFT JOIN
								 (SELECT LOOKUP_CODE, MEANING
									FROM SFCS_PARAMETERS
								   WHERE LOOKUP_TYPE = 'FIRST_CHECK_ITEM_TYPE' AND ENABLED = 'Y') P
									ON I.CHECK_TYPE = P.LOOKUP_CODE
						ORDER BY I.CHECK_TYPE, I.ORDER_NO";
			var data = _dbConnection.Query<MesFirstCheckItemsListModel>(sql);
			return data.ToList();
            //WHERE I.ENABLED = 'Y'

            //                     AND EXISTS
            //                            (SELECT 1

            //                               FROM(SELECT ID

            //                                           FROM SYS_ORGANIZE

            //                                     START WITH ID = :ORGANIZE_ID

            //                                     CONNECT BY PRIOR ID = PARENT_ORGANIZE_ID)

            //                              WHERE ID = I.ORGANIZE_ID)
		}
	}
}