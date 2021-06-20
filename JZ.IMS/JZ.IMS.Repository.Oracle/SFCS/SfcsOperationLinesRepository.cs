/**
*┌──────────────────────────────────────────────────────────────┐
*│　描    述：产线线体 接口实现                                                    
*│　作    者：嘉志科技                                            
*│　版    本：2.0    模板代码自动生成                                                
*│　创建时间：2019-09-23 10:14:20                             
*└──────────────────────────────────────────────────────────────┘
*┌──────────────────────────────────────────────────────────────┐
*│　命名空间： JZ.IMS.Repository.Oracle                                  
*│　类    名： SfcsOperationLinesRepository                                      
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
using System.Collections.Generic;

namespace JZ.IMS.Repository.Oracle
{
    public class SfcsOperationLinesRepository:BaseRepository<SfcsOperationLines,Decimal>, ISfcsOperationLinesRepository
    {
        public SfcsOperationLinesRepository(IOptionsSnapshot<DbOption> options)
        {
            _dbOption =options.Get("iWMS");
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
			string sql = "UPDATE SFCS_OPERATION_LINES SET ENABLED=:ENABLED WHERE ID=:ID";
			return await _dbConnection.ExecuteAsync(sql, new {
				ENABLED = status,
				ID = id,
			});
		}

        /// <summary>
        /// 查询列表
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public async Task<TableDataModel> LoadData(SfcsOperationLineRequestModel model)
		{
            string conditions = "WHERE m.ID > 0 ";
            if (!model.OPERATION_LINE_NAME.IsNullOrWhiteSpace())
            {
                conditions += $"and (instr(m.OPERATION_LINE_NAME, :OPERATION_LINE_NAME) > 0 )";
            }
            if (!model.PLANT_CODE.IsNullOrWhiteSpace())
            {
                conditions += $" and PLANT_CODE=:PLANT_CODE ";
            }

            string sql = @"SELECT ROWNUM AS ROWNO, M.ID, M.OPERATION_LINE_NAME, M.LINE_TYPE, M.PHYSICAL_LOCATION, M.LINE, M.PLANT_CODE, M.ENABLED, M.SUBINVENTORY_ID,
                               M.ORGANIZE_ID, OZ.ORGANIZE_NAME, PL.CHINESE AS LINE_NAME_INCN, PT.CHINESE PLANT_CODE_INCN  
                           FROM SFCS_OPERATION_LINES M  INNER JOIN (SELECT DISTINCT T.* FROM SYS_ORGANIZE T START WITH T.ID IN (SELECT ORGANIZE_ID FROM 
                             SYS_USER_ORGANIZE WHERE MANAGER_ID=:USER_ID) CONNECT BY PRIOR T.ID=T.PARENT_ORGANIZE_ID) OZ ON M.ORGANIZE_ID = OZ.ID 
                           LEFT JOIN SFCS_PARAMETERS PL ON M.PHYSICAL_LOCATION = PL.LOOKUP_CODE AND PL.LOOKUP_TYPE = 'PHYSICAL_LOCATION' AND PL.ENABLED = 'Y' 
                           LEFT JOIN SFCS_PARAMETERS PT ON M.PLANT_CODE = PT.LOOKUP_CODE AND PT.LOOKUP_TYPE = 'PLANT_CODE' AND PT.ENABLED = 'Y' ";

            string pagedSql = SQLBuilderClass.GetPagedSQL(sql, "m.id desc", conditions);
            var resdata = await _dbConnection.QueryAsync<dynamic>(pagedSql, model);
            string sqlcnt = @"SELECT COUNT(0) From SFCS_OPERATION_LINES m inner join (select distinct t.* from SYS_ORGANIZE t start with t.id in (select organize_id from 
                             sys_user_organize where manager_id=:USER_ID) connect by prior t.id=t.PARENT_ORGANIZE_ID) oz on m.organize_id = oz.ID " + conditions;

            int cnt = await _dbConnection.ExecuteScalarAsync<int>(sqlcnt, model);
            return new TableDataModel
            {
                count = cnt,
                data = resdata?.ToList(),
            };
        }

		/// <summary>
		/// 获取导出数据
		/// </summary>
		/// <param name="model"></param>
		/// <returns></returns>
		public async Task<TableDataModel> GetExportData(SfcsOperationLineRequestModel model)
        {
            string conditions = "WHERE m.ID > 0 ";
            if (!model.OPERATION_LINE_NAME.IsNullOrWhiteSpace())
            {
                conditions += $"and (instr(m.OPERATION_LINE_NAME, :OPERATION_LINE_NAME) > 0 )";
            }

            string sql = @"SELECT ROWNUM AS ROWNO, M.ID, M.OPERATION_LINE_NAME, M.LINE_TYPE, M.PHYSICAL_LOCATION, M.LINE, M.PLANT_CODE, M.ENABLED, M.SUBINVENTORY_ID,
                                OZ.ORGANIZE_NAME ORGANIZE_ID, PL.CHINESE AS LINE_NAME_INCN, PT.CHINESE PLANT_CODE_INCN  
                           FROM SFCS_OPERATION_LINES M  INNER JOIN (SELECT DISTINCT T.* FROM SYS_ORGANIZE T START WITH T.ID IN (SELECT ORGANIZE_ID FROM 
                             SYS_USER_ORGANIZE WHERE MANAGER_ID=:USER_ID) CONNECT BY PRIOR T.ID=T.PARENT_ORGANIZE_ID) OZ ON M.ORGANIZE_ID = OZ.ID 
                           LEFT JOIN SFCS_PARAMETERS PL ON M.PHYSICAL_LOCATION = PL.LOOKUP_CODE AND PL.LOOKUP_TYPE = 'PHYSICAL_LOCATION' AND PL.ENABLED = 'Y' 
                           LEFT JOIN SFCS_PARAMETERS PT ON M.PLANT_CODE = PT.LOOKUP_CODE AND PT.LOOKUP_TYPE = 'PLANT_CODE' AND PT.ENABLED = 'Y' ";

            string pagedSql = SQLBuilderClass.GetPagedSQL(sql, "m.id desc", conditions);
            var resdata = await _dbConnection.QueryAsync<dynamic>(pagedSql, model);
            string sqlcnt = @"SELECT COUNT(0) From SFCS_OPERATION_LINES M  INNER JOIN (SELECT DISTINCT T.* FROM SYS_ORGANIZE T START WITH T.ID IN (SELECT ORGANIZE_ID FROM 
                             SYS_USER_ORGANIZE WHERE MANAGER_ID=:USER_ID) CONNECT BY PRIOR T.ID=T.PARENT_ORGANIZE_ID) OZ ON M.ORGANIZE_ID = OZ.ID 
                           LEFT JOIN SFCS_PARAMETERS PL ON M.PHYSICAL_LOCATION = PL.LOOKUP_CODE AND PL.LOOKUP_TYPE = 'PHYSICAL_LOCATION' AND PL.ENABLED = 'Y' 
                           LEFT JOIN SFCS_PARAMETERS PT ON M.PLANT_CODE = PT.LOOKUP_CODE AND PT.LOOKUP_TYPE = 'PLANT_CODE' AND PT.ENABLED = 'Y'" + conditions;

            int cnt = await _dbConnection.ExecuteScalarAsync<int>(sqlcnt, model);
            return new TableDataModel
            {
                count = cnt,
                data = resdata?.ToList(),
            };
        }

        public List<AllLinesModel> GetLinesListByUser(String userName)
        {
            string sql = @" SELECT distinct L.*
							FROM V_MES_LINES L
							WHERE EXISTS
									(SELECT 1
										FROM (    SELECT ID
													FROM SYS_ORGANIZE
												START WITH ID IN (select SUO.ORGANIZE_ID from  SYS_USER_ORGANIZE SUO, SYS_MANAGER SM WHERE SUO.MANAGER_ID = SM.ID
						AND SM.USER_NAME = :USER_NAME)
												CONNECT BY PRIOR ID = PARENT_ORGANIZE_ID)
										WHERE ID = L.ORGANIZE_ID)";

            return (_dbConnection.Query<AllLinesModel>(sql, new { USER_NAME = userName })).ToList();
        }

        /// <summary>
		/// 根据组织ID获取所有线别信息
		/// </summary>
		/// <param name="organizeId">组织ID，默认为“1”（集团）</param>
		/// <returns></returns>
		public List<AllLinesModel> GetLinesList(string organizeId = "1", string lineType = "")
        {
            if (string.IsNullOrEmpty(organizeId)) organizeId = "1";
            string sql = @" SELECT L.*
							FROM V_MES_LINES L INNER JOIN SYS_ORGANIZE_LINE O ON L.LINE_ID = O.LINE_ID {0}
							WHERE EXISTS
									(SELECT 1
										FROM (    SELECT ID
													FROM SYS_ORGANIZE
												START WITH ID = :ORGANIZE_ID
												CONNECT BY PRIOR ID = PARENT_ORGANIZE_ID)
										WHERE ID = O.ORGANIZE_ID)
						ORDER BY O.ORGANIZE_ID, O.LINE_TYPE, O.ATTRIBUTE6, TO_NUMBER (O.ATTRIBUTE5)";

            string sSqlLineType = "";
            if (lineType == "SMT" || lineType == "PCBA")
            {
                sSqlLineType = " AND L.LINE_TYPE = '" + lineType + "' ";
            }

            return (_dbConnection.Query<AllLinesModel>(string.Format(sql, sSqlLineType), new { ORGANIZE_ID = organizeId })).ToList();
        }
    }
}