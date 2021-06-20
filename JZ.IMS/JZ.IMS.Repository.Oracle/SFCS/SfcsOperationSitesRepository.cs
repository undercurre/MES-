/**
*┌──────────────────────────────────────────────────────────────┐
*│　描    述：接口实现                                                    
*│　作    者：嘉志科技                                            
*│　版    本：2.0    模板代码自动生成                                                
*│　创建时间：2019-09-29 17:59:32                             
*└──────────────────────────────────────────────────────────────┘
*┌──────────────────────────────────────────────────────────────┐
*│　命名空间： JZ.IMS.Repository.Oracle                                  
*│　类    名： SfcsOperationSitesRepository                                      
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
    public class SfcsOperationSitesRepository:BaseRepository<SfcsOperationSites,Decimal>, ISfcsOperationSitesRepository
    {
        public SfcsOperationSitesRepository(IOptionsSnapshot<DbOption> options)
        {
            _dbOption =options.Get("iWMS");
            if (_dbOption == null)
            {
                throw new ArgumentNullException(nameof(DbOption));
            }
            _dbConnection = ConnectionFactory.CreateConnection(_dbOption.DbType, _dbOption.ConnectionString);
        }       

        // <summary>
        /// 获取表的序列
        /// </summary>
        /// <returns></returns>
		public async Task<decimal> GetSEQIDAsync()
		{
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
			string sql = "UPDATE SFCS_OPERATION_SITES SET ENABLED=:ENABLED WHERE ID=:ID";
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
        public async Task<TableDataModel> GetExportData(SfcsOperationSitesRequestModel model)
        {
            string conditions = "WHERE m.ID > 0 ";
            if (!model.OPERATION_SITE_NAME.IsNullOrWhiteSpace())
            {
                conditions += $"and (instr(m.OPERATION_SITE_NAME, :OPERATION_SITE_NAME) > 0)";
            }

            string sql = @"SELECT ROWNUM AS ROWNO,m.ID,sl.OPERATION_LINE_NAME as OPERATION_LINE_ID, m.OPERATION_SITE_NAME, so.DESCRIPTION OPERATION_ID, m.DESCRIPTION, m.ENABLED   
                           From SFCS_OPERATION_SITES m  
                           LEFT JOIN SFCS_OPERATION_LINES sl ON m.OPERATION_LINE_ID = sl.ID 
                           LEFT JOIN SFCS_OPERATIONS so ON m.OPERATION_ID = so.ID  ";

            string pagedSql = SQLBuilderClass.GetPagedSQL(sql, "m.id desc", conditions);
            var resdata = await _dbConnection.QueryAsync<dynamic>(pagedSql, model);
            string sqlcnt = @"SELECT COUNT(0) From SFCS_OPERATION_SITES m " + conditions;

            int cnt = await _dbConnection.ExecuteScalarAsync<int>(sqlcnt, model);
            return new TableDataModel
            {
                count = cnt,
                data = resdata?.ToList(),
            };
        }
    }
}