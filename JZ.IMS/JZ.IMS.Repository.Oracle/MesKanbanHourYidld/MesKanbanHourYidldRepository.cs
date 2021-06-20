/**
*┌──────────────────────────────────────────────────────────────┐
*│　描    述：看板-每小时产能记录表接口实现                                                    
*│　作    者：嘉志科技                                            
*│　版    本：2.0    模板代码自动生成                                                
*│　创建时间：2020-05-25 16:55:23                             
*└──────────────────────────────────────────────────────────────┘
*┌──────────────────────────────────────────────────────────────┐
*│　命名空间： JZ.IMS.Repository.Oracle                                  
*│　类    名： MesKanbanHourYidldRepository                                      
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
using JZ.IMS.ViewModels.MesKanbanHourYidld;
using System.Linq;

namespace JZ.IMS.Repository.Oracle
{
    public class MesKanbanHourYidldRepository:BaseRepository<MesKanbanHourYidld,Decimal>, IMesKanbanHourYidldRepository
    {

        string conditions = @" AND EXISTS
                          (SELECT 1
                          FROM (SELECT ID
                             FROM SYS_ORGANIZE
                             START WITH ID = :ORGANIZE_ID
                             CONNECT BY PRIOR ID = PARENT_ORGANIZE_ID) WHERE ID = ORGANIZE_ID)";

        public MesKanbanHourYidldRepository(IOptionsSnapshot<DbOption> options)
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
			string sql = "SELECT ENABLED FROM MES_KANBAN_HOUR_YIDLD WHERE ID=:ID";
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
			string sql = "UPDATE MES_KANBAN_HOUR_YIDLD set ENABLED=:ENABLED WHERE ID=:Id";
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
			string sql = "SELECT MES_KANBAN_HOUR_YIDLD_SEQ.NEXTVAL MY_SEQ FROM DUAL";
			var result = await _dbConnection.ExecuteScalarAsync(sql);
			return (decimal)result;
		}
        /// <summary>
        /// 重写方法获取数据
        /// </summary>
        /// <param name="pageNumber"></param>
        /// <param name="rowsPerPage"></param>
        /// <param name="conditions"></param>
        /// <param name="orderby"></param>
        /// <returns></returns>
        public async Task<IEnumerable<MesKanbanHourYidld>> GetDataPagedAsync(int pageNumber, int rowsPerPage, string conditions, string orderby, object parameters = null)
        {
            string sql = @"SELECT * FROM (SELECT ROW_NUMBER() OVER(ORDER BY A.Id desc) AS PagedNumber, 
                            A.*,B.LINE_NAME,C.CN_DESC,E.REPORT_CONTENT,ROUND((A.OUTPUT_QTY/A.STANDARD_CAPACITY)*100,2) || '%' AS OUTPUT_TIVITY 
                            from MES_KANBAN_HOUR_YIDLD A
                            INNER JOIN V_MES_LINES B ON A.LINE_ID = B.LINE_ID
                            LEFT JOIN SMT_LOOKUP C ON A.PCB_SIDE = C.CODE AND C.TYPE ='PCB_SIDE'
                            INNER JOIN SYS_ORGANIZE_LINE D ON A.LINE_ID = D.LINE_ID
                            LEFT JOIN MES_MONITORING_REPORT E ON A.REPORT_ID = E.ID
                            WHERE {2}) u 
                            WHERE PagedNumber BETWEEN (({0}-1) * {1} + 1) AND ({0} * {1})";
            var model = parameters as MesKanbanHourYidldRequestModel;
            sql = string.Format(sql, pageNumber, rowsPerPage, conditions);
            return await _dbConnection.QueryAsync<MesKanbanHourYidld>(sql, parameters);
        }

        /// <summary>
        /// 获取数据数量
        /// </summary>
        /// <returns></returns>
        public new async Task<int> RecordCountAsync(string conditions, object parameters = null)
        {
            string sql = @"SELECT COUNT(1) from MES_KANBAN_HOUR_YIDLD A
                            INNER JOIN V_MES_LINES B ON A.LINE_ID = B.LINE_ID
                            LEFT JOIN SMT_LOOKUP C ON A.PCB_SIDE = C.CODE AND C.TYPE ='PCB_SIDE'
                            INNER JOIN SYS_ORGANIZE_LINE D ON A.LINE_ID = D.LINE_ID
                            LEFT JOIN MES_MONITORING_REPORT E ON A.REPORT_ID = E.ID
                            WHERE {0}";
            var model = parameters as MesKanbanHourYidldRequestModel;
            sql = string.Format(sql, conditions);
            var result = await _dbConnection.ExecuteScalarAsync<int>(sql, parameters);
            return result;
        }

        /// <summary>
        /// 重写方法获取数据
        /// </summary>
        /// <param name="pageNumber"></param>
        /// <param name="rowsPerPage"></param>
        /// <param name="conditions"></param>
        /// <param name="orderby"></param>
        /// <returns></returns>
        public async Task<IEnumerable<MesKanbanHourYidld>> GetReportListPagedAsync(int pageNumber, int rowsPerPage, string conditions, string orderby, object parameters = null)
        {
            conditions += this.conditions;
            string sql = @"SELECT * FROM (SELECT ROW_NUMBER() OVER(ORDER BY A.WORK_TIME desc) AS PagedNumber,A.* FROM V_MES_KANBAN_HOUR_YIDLD_REPORT A {2}) u 
                            WHERE PagedNumber BETWEEN (({0}-1) * {1} + 1) AND ({0} * {1})";
            var model = parameters as MesKanbanHourYidldRequestModel;
            sql = string.Format(sql, pageNumber, rowsPerPage, conditions);
            return await _dbConnection.QueryAsync<MesKanbanHourYidld>(sql, parameters);
        }

        /// <summary>
        /// 获取数据数量
        /// </summary>
        /// <returns></returns>
        public async Task<int> RecordReportCountAsync(string conditions, object parameters = null)
        {
            conditions += this.conditions;
            string sql = @"SELECT COUNT(1) from V_MES_KANBAN_HOUR_YIDLD_REPORT {0}";
            var model = parameters as MesKanbanHourYidldRequestModel;
            sql = string.Format(sql, conditions);
            var result = await _dbConnection.ExecuteScalarAsync<int>(sql, parameters);
            return result;
        }

        public List<PriceTable> GetPriceListAsync(int pageNumber, int rowsPerPage, string conditions, object parameters = null)
        {
            string sql = @"SELECT ""Code"" AS Code, ""Price"" AS Price FROM ""V_MES_Material""@ERP WHERE ""Price"" > 0 AND EXISTS
                        (SELECT * FROM (SELECT ROW_NUMBER() OVER(ORDER BY A.WORK_TIME desc) AS PagedNumber,A.* FROM V_MES_KANBAN_HOUR_YIDLD_REPORT A {2}) u 
                        WHERE PagedNumber BETWEEN (({0}-1) * {1} + 1) AND ({0} * {1}) AND  ""Code"" = PART_NO)";
            sql = string.Format(sql, pageNumber, rowsPerPage, conditions);
            return (_dbConnection.Query<PriceTable>(sql, parameters)).ToList();
        }
    }
}