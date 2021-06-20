/*
*┌──────────────────────────────────────────────────────────────┐
*│　描    述：接口实现                                                    
*│　作    者：嘉志科技                                            
*│　版    本：2.0    模板代码自动生成                                                
*│　创建时间：2020-08-24 14:19:22                             
*└──────────────────────────────────────────────────────────────┘
*┌──────────────────────────────────────────────────────────────┐
*│　命名空间： JZ.IMS.Repository.Oracle                                  
*│　类    名： MesStoplineCallRepository                                      
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
    public class MesStoplineCallRepository:BaseRepository<MesStoplineCall,Decimal>, IMesStoplineCallRepository
    {
        public MesStoplineCallRepository(IOptionsSnapshot<DbOption> options)
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
			string sql = "SELECT ENABLED FROM MES_STOPLINE_CALL WHERE ID=:ID";
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
			string sql = "UPDATE MES_STOPLINE_CALL set ENABLED=:ENABLED WHERE ID=:Id";
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
			string sql = "SELECT MES_STOPLINE_CALL_SEQ.NEXTVAL MY_SEQ FROM DUAL";
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
			string sql = "select count(0) from MES_STOPLINE_CALL where id = :id";
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
		public async Task<decimal> SaveDataByTrans(MesStoplineCallModel model)
		{
			int result = 1;
			ConnectionFactory.OpenConnection(_dbConnection);
			using (var tran = _dbConnection.BeginTransaction())
			{
				try
				{
					//新增
					string insertSql = @"insert into MES_STOPLINE_CALL 
					(ID,PRODUCT_STOPLINE_ID,ENABLED,CALL_TONTENT_ID) 
					VALUES (:ID,:PRODUCT_STOPLINE_ID,:ENABLED,:CALL_TONTENT_ID)";
					if (model.InsertRecords != null && model.InsertRecords.Count > 0)
					{
						foreach (var item in model.InsertRecords)
						{
							var newid = await GetSEQID();
							var resdata = await _dbConnection.ExecuteAsync(insertSql, new
							{
								ID = newid,
								item.PRODUCT_STOPLINE_ID,
								item.ENABLED,
								item.CALL_TONTENT_ID,
							}, tran);
						}
					}
					//更新
					string updateSql = @"Update MES_STOPLINE_CALL set PRODUCT_STOPLINE_ID=:PRODUCT_STOPLINE_ID,ENABLED=:ENABLED,CALL_TONTENT_ID=:CALL_TONTENT_ID  
						where ID=:ID ";
					if (model.UpdateRecords != null && model.UpdateRecords.Count > 0)
					{
						foreach (var item in model.UpdateRecords)
						{
							var resdata = await _dbConnection.ExecuteAsync(updateSql, new
							{
								item.ID,
								item.PRODUCT_STOPLINE_ID,
								item.ENABLED,
								item.CALL_TONTENT_ID,
							}, tran);
						}
					}
					//删除
					string deleteSql = @"Delete from MES_STOPLINE_CALL where ID=:ID ";
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



		/// <summary>
		/// 获取所有的异常标题
		/// </summary>
		/// <returns></returns>
		public async Task<List<dynamic>> GetCallTitle()
		{
			List<dynamic> result = null;
			try
			{
				string sql = @"select   CALL_TITLE   from ANDON_CALL_CONTENT_CONFIG a where ENABLED='Y' and CALL_TITLE is not null order by CALL_TITLE desc ";
				var objectlist = await _dbConnection.QueryAsync<dynamic>(sql);
				return objectlist?.ToList();
			}
			catch (Exception ex)
			{
				result = null;
			}
			return result;
		}

		/// <summary>
		/// 根据异常种类、类型和异常标题获取异常内容配置表的信息
		/// </summary>
		/// <param name="code">种类</param>
		/// <param name="typecode">类型</param>
		/// <param name="title">异常标题</param>
		/// <returns></returns>
		public async Task<TableDataModel> GetCallContentConfigByTypeCode(string code, string typecode, string title)
		{
			string conditions = "";
			if (!code.IsNullOrWhiteSpace())
			{
				conditions += string.Format(@" and CALL_CATEGORY_CODE='{0}' ",code);
			}
			if (!typecode.IsNullOrWhiteSpace())
			{
				conditions += string.Format(@" and CALL_TYPE_CODE='{0}' ", typecode);
			}
			if (!title.IsNullOrWhiteSpace())
			{
				conditions += string.Format(@"  and CALL_TITLE='{0}' ", title);
			}

			string sql = string.Format(@"select * from ANDON_CALL_CONTENT_CONFIG  where ENABLED='Y' {0} ",conditions);
			var resdata = await _dbConnection.QueryAsync<object>(sql);

			return new TableDataModel
			{
				count = Convert.ToInt32(resdata?.ToList().Count()),
				data = resdata?.ToList(),
			};
		}





	}
}