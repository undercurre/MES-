/**
*┌──────────────────────────────────────────────────────────────┐
*│　描    述：工序 接口实现                                                    
*│　作    者：嘉志科技                                            
*│　版    本：2.0    模板代码自动生成                                                
*│　创建时间：2019-09-23 11:12:27                             
*└──────────────────────────────────────────────────────────────┘
*┌──────────────────────────────────────────────────────────────┐
*│　命名空间： JZ.IMS.Repository.Oracle                                  
*│　类    名： SfcsOperationsRepository                                      
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
	public class SfcsOperationsRepository : BaseRepository<SfcsOperations, Decimal>, ISfcsOperationsRepository
	{
		public SfcsOperationsRepository(IOptionsSnapshot<DbOption> options)
		{
			_dbOption = options.Get("iWMS");
			if (_dbOption == null)
			{
				throw new ArgumentNullException(nameof(DbOption));
			}
			_dbConnection = ConnectionFactory.CreateConnection(_dbOption.DbType, _dbOption.ConnectionString);
		}

		/// <summary>
		/// 获取ID的序列
		/// </summary>
		/// <returns></returns>
		public async Task<decimal> GetSEQIDAsync() {
			string sql = "SELECT MES_SEQ_ID.NEXTVAL MY_SEQ FROM DUAL";
			var result = await _dbConnection.ExecuteScalarAsync(sql);
			return (decimal)result;
		}

		/// <summary>
		/// 通过ID更新对应的激活状态
		/// </summary>
		/// <param name="id">改变了状态的ID</param>
		/// <param name="status">改变后的状态</param>
		/// <returns>成功更新的条数</returns>
		public async Task<decimal> UpdateEnabledById(decimal id, string status) {
			string sql = "UPDATE SFCS_OPERATIONS SET ENABLED=:ENABLED WHERE ID=:ID";
			return await _dbConnection.ExecuteAsync(sql, new {
				ENABLED = status,
				ID = id,
			});
		}

		/// <summary>
		/// 获取导出数据
		/// </summary>
		/// <param name="model"></param>
		/// <returns></returns>
		public async Task<TableDataModel> GetExportData(SfcsOperationsRequestModel model)
		{
			string conditions = "WHERE m.id <>100 and m.id <>999 ";
			if (!model.OPERATION_NAME.IsNullOrWhiteSpace())
			{
				conditions += $"and (instr(m.OPERATION_NAME, :OPERATION_NAME) > 0 )";
			}

			string sql = @"SELECT ROWNUM AS ROWNO,m.ID,m.OPERATION_NAME,m.OPERATION_CLASS,pm.CHINESE as OPERATION_CATEGORY,m.DESCRIPTION,m.ENABLED,m.AUTO_LINK 
						   From SFCS_OPERATIONS m  
						   LEFT JOIN SFCS_PARAMETERS pm ON m.OPERATION_CATEGORY = pm.LOOKUP_CODE AND pm.LOOKUP_TYPE = 'OPERATION_CATEGORY' AND pm.ENABLED = 'Y' ";

			string pagedSql = SQLBuilderClass.GetPagedSQL(sql, "m.id desc", conditions);
			var resdata = await _dbConnection.QueryAsync<dynamic>(pagedSql, model);
			string sqlcnt = @"SELECT COUNT(0) From SFCS_OPERATIONS m  
                           LEFT JOIN SFCS_PARAMETERS pm ON m.OPERATION_CATEGORY = pm.LOOKUP_CODE AND pm.LOOKUP_TYPE = 'OPERATION_CATEGORY' AND pm.ENABLED = 'Y' " + conditions;

			int cnt = await _dbConnection.ExecuteScalarAsync<int>(sqlcnt, model);
			return new TableDataModel
			{
				count = cnt,
				data = resdata?.ToList(),
			};
		}

        /// <summary>
        /// 获取制程工序
        /// </summary>
        /// <param name="route_id"></param>
        /// <returns></returns>
        public async Task<dynamic> GetRouteOperationByRouteID(int route_id)
        {
            String sQyery = @"SELECT SRC.ID, SRC.PRODUCT_OPERATION_CODE, SRC.CURRENT_OPERATION_ID , SO.OPERATION_NAME,SO.DESCRIPTION 
								FROM SFCS_ROUTE_CONFIG SRC, SFCS_OPERATIONS  SO 
								WHERE ROUTE_ID = :ROUTE_ID AND SRC.CURRENT_OPERATION_ID = SO.ID 
								ORDER BY ORDER_NO ASC";
            var resdata = await _dbConnection.QueryAsync<dynamic>(sQyery, new { ROUTE_ID = route_id });
            return resdata;
        }

    }
}