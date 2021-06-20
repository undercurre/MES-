/**
*┌──────────────────────────────────────────────────────────────┐
*│　描    述：接口实现                                                    
*│　作    者：嘉志科技                                            
*│　版    本：2.0    模板代码自动生成                                                
*│　创建时间：2020-02-27 14:08:53                             
*└──────────────────────────────────────────────────────────────┘
*┌──────────────────────────────────────────────────────────────┐
*│　命名空间： JZ.IMS.Repository.Oracle                                  
*│　类    名： SmtResourceRouteRepository                                      
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
using JZ.IMS.Core.Extensions;

namespace JZ.IMS.Repository.Oracle
{
    public class SmtResourceRouteRepository:BaseRepository<SmtResourceRoute,Decimal>, ISmtResourceRouteRepository
    {
        public SmtResourceRouteRepository(IOptionsSnapshot<DbOption> options)
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
			string sql = "SELECT ENABLED FROM SMT_RESOURCE_ROUTE WHERE ID=:ID";
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
			string sql = "UPDATE SMT_RESOURCE_ROUTE set ENABLED=:ENABLED WHERE ID=:Id";
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
			int cnt = 0;
			object result = 0;
			//辅料作业表 
			string sql = "select count(0) from smt_resource_runcard where ROUTE_ID = :id";
			result = await _dbConnection.ExecuteScalarAsync(sql, new
			{
				id
			});
			cnt = Convert.ToInt32(result);
			if (cnt == 0)
			{
				//辅料管控规则 
				sql = "select count(0) from SMT_RESOURCE_RULES where ROUTE_OPERATION_ID = :id";
				result = await _dbConnection.ExecuteScalarAsync(sql, new
				{
					id
				});
				cnt = Convert.ToInt32(result);
			}
			return (cnt > 0);
		}

		/// <summary>
		/// 获取辅料名称
		/// </summary>
		/// <returns></returns>
		public async Task<List<IDNAME>> GetNameAsync()
		{
			List<IDNAME> result = null;

			string sql = @"SELECT CODE AS ID, EN_DESC NAME FROM SMT_LOOKUP WHERE TYPE = 'RESOURCE_OBJECT' AND ENABLED = 'Y'";
			var tmpdata = await _dbConnection.QueryAsync<IDNAME>(sql);

			if (tmpdata != null)
			{
				result = tmpdata.ToList();
			}
			return result;
		}

		/// <summary>
		/// 获取辅料工序列表
		/// </summary>
		/// <returns></returns>
		public async Task<List<IDNAME>> GetProcessAsync()
		{
			List<IDNAME> result = null;

			string sql = @"SELECT CODE AS ID, VALUE AS NAME FROM SMT_LOOKUP WHERE TYPE = 'RESOURCE_ROUTE' AND ENABLED = 'Y'";
			var tmpdata = await _dbConnection.QueryAsync<IDNAME>(sql);

			if (tmpdata != null)
			{
				result = tmpdata.ToList();
			}
			return result;
		}

		/// <summary>
		/// 获取辅料类型列表
		/// </summary>
		/// <returns></returns>
		public async Task<List<IDNAME>> GetCategoryAsync()
		{
			List<IDNAME> result = null;

			string sql = @"SELECT CATEGORY_ID AS ID, CATEGORY_NAME AS NAME FROM SMT_RESOURCE_CATEGORY WHERE ENABLED = 'Y'";
			var tmpdata = await _dbConnection.QueryAsync<IDNAME>(sql);

			if (tmpdata != null)
			{
				result = tmpdata.ToList();
			}
			return result;
		}

		/// <summary>
		/// 获取特性列表
		/// </summary>
		/// <returns></returns>
		public async Task<List<IDNAME>> GetPropertyAsync()
		{
			List<IDNAME> result = null;

			string sql = @"SELECT CODE AS ID, VALUE AS NAME FROM SMT_LOOKUP WHERE TYPE = 'RESOURCE_PROPERTY' AND ENABLED = 'Y'";
			var tmpdata = await _dbConnection.QueryAsync<IDNAME>(sql);

			if (tmpdata != null)
			{
				result = tmpdata.ToList();
			}
			return result;
		}

		/// <summary>
		/// 导出分页分页
		/// </summary>
		/// <param name="model"></param>
		/// <returns></returns>
		public async Task<TableDataModel> GetExportData(SmtResourceRouteRequestModel model)
		{
			string sql = @" SELECT ROW_NUMBER() OVER(ORDER BY SRR.ID DESC) AS ROWNO, SRR.ID, LP1.EN_DESC OBJECT_ID, LP2.VALUE CURRENT_OPERATION, LP3.VALUE NEXT_OPERATION, SRR.ORDER_NO FROM SMT_RESOURCE_ROUTE SRR
							LEFT JOIN SMT_LOOKUP LP1 ON LP1.CODE=SRR.OBJECT_ID AND LP1.TYPE = 'RESOURCE_OBJECT' AND LP1.ENABLED = 'Y'
							LEFT JOIN SMT_LOOKUP LP2 ON LP2.CODE=SRR.CURRENT_OPERATION AND LP2.TYPE = 'RESOURCE_ROUTE' AND LP2.ENABLED = 'Y'
							LEFT JOIN SMT_LOOKUP LP3 ON LP3.CODE=SRR.NEXT_OPERATION AND LP3.TYPE = 'RESOURCE_ROUTE' AND LP3.ENABLED = 'Y' ";

			string conditions = " WHERE SRR.ID > 0 ";

			if (!model.OBJECT_ID.IsNullOrWhiteSpace() && Convert.ToInt32(model.OBJECT_ID) > 0)
			{
				conditions += $"and (SRR.OBJECT_ID = :OBJECT_ID)";
			}


			string pagedSql = SQLBuilderClass.GetPagedSQL(sql, conditions+ " ORDER BY SRR.ORDER_NO ASC ");
			var resdata = await _dbConnection.QueryAsync<object>(pagedSql, model);
			string sqlcnt = @" SELECT COUNT(SRR.ID) FROM SMT_RESOURCE_ROUTE SRR
LEFT JOIN SMT_LOOKUP LP1 ON LP1.CODE=SRR.OBJECT_ID AND LP1.TYPE = 'RESOURCE_OBJECT' AND LP1.ENABLED = 'Y'
LEFT JOIN SMT_LOOKUP LP2 ON LP2.CODE=SRR.CURRENT_OPERATION AND LP2.TYPE = 'RESOURCE_ROUTE' AND LP2.ENABLED = 'Y'
LEFT JOIN SMT_LOOKUP LP3 ON LP3.CODE=SRR.NEXT_OPERATION AND LP3.TYPE = 'RESOURCE_ROUTE' AND LP3.ENABLED = 'Y' " + conditions;

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
		public async Task<decimal> SaveDataByTrans(SmtResourceRouteModel model)
		{
			int result = 1;
			ConnectionFactory.OpenConnection(_dbConnection);
			using (var tran = _dbConnection.BeginTransaction())
			{
				try
				{
                    //删除
                    string deleteSql = @"Delete from smt_resource_route where ID=:ID ";
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
                    //新增
                    string insertSql = @"INSERT INTO SMT_RESOURCE_ROUTE (ID, OBJECT_ID, CURRENT_OPERATION, NEXT_OPERATION, ORDER_NO) 
					VALUES (:ID,:OBJECT_ID,:CURRENT_OPERATION,:NEXT_OPERATION,:ORDER_NO)";
                    if (model.insertRecords != null && model.insertRecords.Count > 0)
                    {
                        foreach (var item in model.insertRecords)
                        {
                            var newid = await GetSEQ_ID();
                            var resdata = await _dbConnection.ExecuteAsync(insertSql, new
                            {
                                ID = newid,
                                item.OBJECT_ID,
                                item.CURRENT_OPERATION,
                                item.NEXT_OPERATION,
                                item.ORDER_NO,
                            }, tran);
                        }
                    }
                    //更新
                    string updateSql = @"Update smt_resource_route set OBJECT_ID =:OBJECT_ID, CURRENT_OPERATION =:CURRENT_OPERATION,
							NEXT_OPERATION =:NEXT_OPERATION, ORDER_NO =:ORDER_NO 
						where ID=:ID ";
                    if (model.updateRecords != null && model.updateRecords.Count > 0)
                    {
                        foreach (var item in model.updateRecords)
                        {
                            var resdata = await _dbConnection.ExecuteAsync(updateSql, new
                            {
                                item.ID,
                                item.OBJECT_ID,
                                item.CURRENT_OPERATION,
                                item.NEXT_OPERATION,
                                item.ORDER_NO,
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