/**
*┌──────────────────────────────────────────────────────────────┐
*│　描    述：接口实现                                                    
*│　作    者：嘉志科技                                            
*│　版    本：2.0    模板代码自动生成                                                
*│　创建时间：2020-03-30 17:24:04                             
*└──────────────────────────────────────────────────────────────┘
*┌──────────────────────────────────────────────────────────────┐
*│　命名空间： JZ.IMS.Repository.Oracle                                  
*│　类    名： SfcsDefectConfigRepository                                      
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
    public class SfcsDefectConfigRepository : BaseRepository<SfcsDefectConfig, Decimal>, ISfcsDefectConfigRepository
    {
        public SfcsDefectConfigRepository(IOptionsSnapshot<DbOption> options)
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
            string sql = "SELECT ENABLED FROM SFCS_DEFECT_CONFIG WHERE ID=:ID";
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
            string sql = "UPDATE SFCS_DEFECT_CONFIG set ENABLED=:ENABLED WHERE ID=:Id";
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
            string sql = "SELECT MES_SEQ_ID.NEXTVAL MY_SEQ FROM DUAL";
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
            string sql = "select count(0) from SFCS_DEFECT_CONFIG where id = :id";
            object result = await _dbConnection.ExecuteScalarAsync(sql, new
            {
                id
            });

            return (Convert.ToInt32(result) > 0);
        }

        /// <summary>
        /// 获取不良类型
        /// </summary>
        /// <returns></returns>
        public async Task<List<CodeName>> GetDefectType()
        {
            string sql = @"SELECT LOOKUP_CODE as Code, CHINESE as NAME 
                           FROM SFCS_PARAMETERS
                           WHERE LOOKUP_TYPE = 'DEFECT_TYPE' ";
            return (await _dbConnection.QueryAsync<CodeName>(sql))?.ToList();
        }

        /// <summary>
        /// 获取不良种类
        /// </summary>
        /// <returns></returns>
        public async Task<List<CodeName>> GetDefectClass()
        {
            string sql = @"SELECT LOOKUP_CODE as Code, CHINESE as NAME 
                           FROM SFCS_PARAMETERS
                           WHERE LOOKUP_TYPE = 'DEFECT_CLASS' ";
            return (await _dbConnection.QueryAsync<CodeName>(sql))?.ToList();
        }

        /// <summary>
        /// 获取不良等级
        /// </summary>
        /// <returns></returns>
        public async Task<List<CodeName>> GetDefectLevelCode()
        {
            string sql = @"SELECT LOOKUP_CODE as Code, CHINESE as NAME 
                           FROM SFCS_PARAMETERS
                           WHERE LOOKUP_TYPE = 'DEFECT_LEVEL_CODE' ";
            return (await _dbConnection.QueryAsync<CodeName>(sql))?.ToList();
        }

        /// <summary>
        /// 获取不良类别
        /// </summary>
        /// <returns></returns>
        public async Task<List<CodeName>> GetDefectCategory()
        {
            string sql = @"SELECT LOOKUP_CODE as Code, CHINESE as NAME 
                           FROM SFCS_PARAMETERS
                           WHERE LOOKUP_TYPE = 'DEFECT_CATEGORY' ";
            return (await _dbConnection.QueryAsync<CodeName>(sql))?.ToList();
        }

        /// <summary>
        /// 获取不良来源
        /// </summary>
        /// <returns></returns>
        public async Task<List<string>> GetDefectSource()
        {
            string sql = @"SELECT '内部原因' REASON FROM DUAL
                           UNION
                           SELECT '外部原因' REASON FROM DUAL ";
            return (await _dbConnection.QueryAsync<string>(sql))?.ToList();
        }

        /// <summary>
        /// 导出分页分页
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public async Task<TableDataModel> GetExportData(SfcsDefectConfigRequestModel model)
        {
            string sql = @"  SELECT ROW_NUMBER() OVER(ORDER BY SDC.ID DESC) AS ROWNO,SDC.ID,SDC.DEFECT_CODE,PS1.CHINESE DEFECT_TYPE,PS2.CHINESE DEFECT_CLASS,PS3.CHINESE DEFECT_CATEGORY,PS4.CHINESE LEVEL_CODE,SDC.SOURCE,SDC.DEFECT_DESCRIPTION,SDC.CHINESE_DESCRIPTION,SDC.ENABLED FROM SFCS_DEFECT_CONFIG SDC
                             LEFT JOIN SFCS_PARAMETERS PS1 ON PS1.LOOKUP_TYPE = 'DEFECT_TYPE' AND SDC.DEFECT_TYPE=PS1.LOOKUP_CODE
                             LEFT JOIN SFCS_PARAMETERS PS2 ON PS2.LOOKUP_TYPE = 'DEFECT_CLASS' AND  SDC.DEFECT_CLASS=PS2.LOOKUP_CODE
                             LEFT JOIN SFCS_PARAMETERS PS3 ON PS3.LOOKUP_TYPE = 'DEFECT_CATEGORY' AND SDC.DEFECT_CATEGORY=PS3.LOOKUP_CODE
                             LEFT JOIN SFCS_PARAMETERS PS4 ON PS4.LOOKUP_TYPE = 'DEFECT_LEVEL_CODE' AND SDC.LEVEL_CODE=PS4.LOOKUP_CODE ";

            string conditions = " WHERE SDC.ID > 0 ";

            if (model?.DEFECT_TYPE > 0)
                conditions += " AND SDC.DEFECT_TYPE = :DEFECT_TYPE ";

            if (model?.DEFECT_CLASS > 0)
                conditions += " AND SDC.DEFECT_CLASS = :DEFECT_CLASS ";

            if (model?.DEFECT_CATEGORY > 0)
                conditions += " AND SDC.DEFECT_CATEGORY = :DEFECT_CATEGORY ";

            if (model?.DEFECT_LEVEL_CODE > 0)
                conditions += " AND SDC.LEVEL_CODE = :DEFECT_LEVEL_CODE ";

            if (!model.DEFECT_SOURCE.IsNullOrWhiteSpace())
                conditions += " AND SDC.SOURCE = :DEFECT_SOURCE ";

            if (!model.Key.IsNullOrWhiteSpace())
            {
                conditions += $"and (instr(upper(SDC.DEFECT_CODE), upper(:Key)) > 0 or instr(SDC.DEFECT_DESCRIPTION, :Key) > 0 or instr(SDC.CHINESE_DESCRIPTION, :Key) > 0)";
            }


            string pagedSql = SQLBuilderClass.GetPagedSQL(sql,  conditions);
            var resdata = await _dbConnection.QueryAsync<object>(pagedSql, model);
            string sqlcnt = @" SELECT COUNT(SDC.ID) FROM SFCS_DEFECT_CONFIG SDC
                             LEFT JOIN SFCS_PARAMETERS PS1 ON PS1.LOOKUP_TYPE = 'DEFECT_TYPE' AND SDC.DEFECT_TYPE=PS1.LOOKUP_CODE
                             LEFT JOIN SFCS_PARAMETERS PS2 ON PS2.LOOKUP_TYPE = 'DEFECT_CLASS' AND  SDC.DEFECT_CLASS=PS2.LOOKUP_CODE
                             LEFT JOIN SFCS_PARAMETERS PS3 ON PS3.LOOKUP_TYPE = 'DEFECT_CATEGORY' AND SDC.DEFECT_CATEGORY=PS3.LOOKUP_CODE
                             LEFT JOIN SFCS_PARAMETERS PS4 ON PS4.LOOKUP_TYPE = 'DEFECT_LEVEL_CODE' AND SDC.LEVEL_CODE=PS4.LOOKUP_CODE " + conditions;

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
        public async Task<decimal> SaveDataByTrans(SfcsDefectConfigModel model)
        {
            int result = 1;
            ConnectionFactory.OpenConnection(_dbConnection);
            using (var tran = _dbConnection.BeginTransaction())
            {
                try
                {
                    //新增
                    string insertSql = @"insert into SFCS_DEFECT_CONFIG 
					(ID,DEFECT_CODE,DEFECT_TYPE,DEFECT_CLASS,DEFECT_CATEGORY,LEVEL_CODE,SOURCE,DEFECT_DESCRIPTION,CHINESE_DESCRIPTION,ENABLED) 
					VALUES (:ID,:DEFECT_CODE,:DEFECT_TYPE,:DEFECT_CLASS,:DEFECT_CATEGORY,:LEVEL_CODE,:SOURCE,:DEFECT_DESCRIPTION,:CHINESE_DESCRIPTION,:ENABLED)";
                    if (model.insertRecords != null && model.insertRecords.Count > 0)
                    {
                        foreach (var item in model.insertRecords)
                        {
                            var newid = await GetSEQID();
                            var resdata = await _dbConnection.ExecuteAsync(insertSql, new
                            {
                                ID = newid,
                                item.DEFECT_CODE,
                                item.DEFECT_TYPE,
                                item.DEFECT_CLASS,
                                item.DEFECT_CATEGORY,
                                item.LEVEL_CODE,
                                item.SOURCE,
                                item.DEFECT_DESCRIPTION,
                                item.CHINESE_DESCRIPTION,
                                item.ENABLED,
                            }, tran);
                        }
                    }
                    //更新
                    string updateSql = @"Update SFCS_DEFECT_CONFIG set DEFECT_CODE=:DEFECT_CODE,DEFECT_TYPE=:DEFECT_TYPE,DEFECT_CLASS=:DEFECT_CLASS,
                                            DEFECT_CATEGORY=:DEFECT_CATEGORY,LEVEL_CODE=:LEVEL_CODE,SOURCE=:SOURCE,DEFECT_DESCRIPTION=:DEFECT_DESCRIPTION,
                                            CHINESE_DESCRIPTION=:CHINESE_DESCRIPTION,ENABLED=:ENABLED   
						                where ID=:ID ";
                    if (model.updateRecords != null && model.updateRecords.Count > 0)
                    {
                        foreach (var item in model.updateRecords)
                        {
                            var resdata = await _dbConnection.ExecuteAsync(updateSql, new
                            {
                                item.ID,
                                item.DEFECT_CODE,
                                item.DEFECT_TYPE,
                                item.DEFECT_CLASS,
                                item.DEFECT_CATEGORY,
                                item.LEVEL_CODE,
                                item.SOURCE,
                                item.DEFECT_DESCRIPTION,
                                item.CHINESE_DESCRIPTION,
                                item.ENABLED,
                            }, tran);
                        }
                    }
                    //删除
                    //string deleteSql = @"Delete from SFCS_DEFECT_CONFIG where ID=:ID ";
                    //if (model.removeRecords != null && model.removeRecords.Count > 0)
                    //{
                    //    foreach (var item in model.removeRecords)
                    //    {
                    //        var resdata = await _dbConnection.ExecuteAsync(deleteSql, new
                    //        {
                    //            item.ID
                    //        }, tran);
                    //    }
                    //}

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
        /// 获取产线维修数据
        /// </summary>
        /// <param name="WO_NO"></param>
        /// <param name="pageModel"></param>
        /// <returns></returns>
        public async Task<TableDataModel> GetSfscEquipHeadCodeDataAsync(string WO_NO, PageModel pageModel)
        {
            var conditions = " ";
            if (!WO_NO.IsNullOrEmpty())
            {
                conditions += " AND WO_NO = :WO_NO";
            }
            var sqlPage = $@"SELECT * FROM (SELECT ROWNUM NUM,A.* FROM(SELECT COUNT(A.DEFECT_CODE) QTY, A.DEFECT_CODE,SOURCE,B.DEFECT_DESCRIPTION,R.LOCATION,L.LINE_NAME,WO_NO
                                    FROM SFCS_COLLECT_DEFECTS A
                                    LEFT JOIN SFCS_DEFECT_CONFIG B ON A.DEFECT_CODE = B.DEFECT_CODE
                                    LEFT JOIN SFCS_OPERATION_SITES C ON A.DEFECT_SITE_ID = C.ID
                                    LEFT JOIN BSMT.V_MES_LINES L ON C.OPERATION_LINE_ID = L.LINE_ID
                                    INNER JOIN SFCS_WO W ON A.WO_ID = W.ID
                                    LEFT JOIN SFCS_REPAIR_RECIPE R ON A.COLLECT_DEFECT_ID = R.COLLECT_DEFECT_ID
                                    WHERE 1=1 {conditions}
                                    GROUP BY A.DEFECT_CODE,A.WO_ID,SOURCE,B.DEFECT_DESCRIPTION ,L.LINE_NAME,WO_NO,R.LOCATION) A) u 
					WHERE u.NUM BETWEEN ((:Page-1) * :Limit + 1) AND (:Page * :Limit)";

            var data = await _dbConnection.QueryAsync<dynamic>(sqlPage, new { WO_NO, pageModel.Limit, pageModel.Page });

            var countSql = $@"SELECT COUNT(1) FROM (SELECT COUNT(A.DEFECT_CODE) QTY, A.DEFECT_CODE,SOURCE,L.LINE_NAME,WO_NO,R.LOCATION
                                    FROM SFCS_COLLECT_DEFECTS A
                                    LEFT JOIN SFCS_DEFECT_CONFIG B ON A.DEFECT_CODE = B.DEFECT_CODE
                                    LEFT JOIN SFCS_OPERATION_SITES C ON A.DEFECT_SITE_ID = C.ID
                                    LEFT JOIN BSMT.V_MES_LINES L ON C.OPERATION_LINE_ID = L.LINE_ID
                                    INNER JOIN SFCS_WO W ON A.WO_ID = W.ID
                                    LEFT JOIN SFCS_REPAIR_RECIPE R ON A.COLLECT_DEFECT_ID = R.COLLECT_DEFECT_ID
                                    WHERE 1=1 {conditions}
                                    GROUP BY A.DEFECT_CODE,A.WO_ID,SOURCE,L.LINE_NAME,WO_NO,R.LOCATION)";
            var count = await _dbConnection.ExecuteScalarAsync<int>(countSql, new { WO_NO, pageModel.Limit, pageModel.Page });

            return new TableDataModel() { data = data, count = count };
        }

        /// <summary>
        /// 获取产线维修数据
        /// </summary>
        /// <param name="WO_NO"></param>
        /// <param name="pageModel"></param>
        /// <returns></returns>
        public async Task<TableDataModel> GetSfscEquipHeadDataAsync(string WO_NO, PageModel pageModel)
        {
            var conditions = " ";
            if (!WO_NO.IsNullOrEmpty())
            {
                conditions += " AND WO_NO = :WO_NO";
            }
            var sqlPage = $@"SELECT * FROM (SELECT ROWNUM NUM,A.*,(CASE A.REPAIR_RESULT WHEN 0 THEN '可用' ELSE '报废' END) AS REPAIR_RESULT_NAME,B.OPERATION_LINE_NAME,C.OPERATION_SITE_NAME,
                                    D.LOC,E.DEFECT_CODE,E.DEFECT_NAME 
                                    FROM SFCS_NOCODE_REPAIR A
                                    LEFT JOIN SFCS_OPERATION_LINES B ON A.LINE_ID = B.ID
                                    LEFT JOIN SFCS_OPERATION_SITES C ON A.SITE_ID = C.ID
                                    LEFT JOIN SFCS_NOCODE_DEFECT_LOC D ON A.ID = D.MST_ID
                                    LEFT JOIN SFCS_NOCODE_DEFECT E ON A.ID = E.MST_ID WHERE 1=1 {conditions}) u 
					WHERE u.num BETWEEN ((:Page-1) * :Limit + 1) AND (:Page * :Limit)";

            var data = await _dbConnection.QueryAsync<dynamic>(sqlPage, new { WO_NO, pageModel.Limit, pageModel.Page });

            var countSql = $@"SELECT COUNT(1) FROM SFCS_NOCODE_REPAIR A
                                    LEFT JOIN SFCS_OPERATION_LINES B ON A.LINE_ID = B.ID
                                    LEFT JOIN SFCS_OPERATION_SITES C ON A.SITE_ID = C.ID
                                    LEFT JOIN SFCS_NOCODE_DEFECT_LOC D ON A.ID = D.MST_ID
                                    LEFT JOIN SFCS_NOCODE_DEFECT E ON A.ID = E.MST_ID WHERE 1=1 {conditions}";
            var count = await _dbConnection.ExecuteScalarAsync<int>(countSql, new { WO_NO, pageModel.Limit, pageModel.Page });

            return new TableDataModel() { data = data, count = count };
        }

        /// <summary>
        /// 获取不良代码信息
        /// </summary>
        /// <returns></returns>
        public List<SfcsDefectConfig> GetDefectConfig()
        {
            string sql = "SELECT DEFECT_CODE,DEFECT_DESCRIPTION FROM SFCS_DEFECT_CONFIG ORDER BY ID DESC";
            return (_dbConnection.Query<SfcsDefectConfig>(sql)).ToList();
        }
    }
}