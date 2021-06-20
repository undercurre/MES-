/**
*┌──────────────────────────────────────────────────────────────┐
*│　描    述：接口实现                                                    
*│　作    者：嘉志科技                                            
*│　版    本：2.0    模板代码自动生成                                                
*│　创建时间：2020-02-26 11:58:40                             
*└──────────────────────────────────────────────────────────────┘
*┌──────────────────────────────────────────────────────────────┐
*│　命名空间： JZ.IMS.Repository.Oracle                                  
*│　类    名： SmtResourceCategoryRepository                                      
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
    public class SmtResourceCategoryRepository : BaseRepository<SmtResourceCategory, Decimal>, ISmtResourceCategoryRepository
    {
        public SmtResourceCategoryRepository(IOptionsSnapshot<DbOption> options)
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
            string sql = "SELECT ENABLED FROM SMT_RESOURCE_CATEGORY WHERE ID=:ID";
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
            string sql = "UPDATE SMT_RESOURCE_CATEGORY set ENABLED=:ENABLED WHERE ID=:Id";
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
            string sql = "select count(0) from smt_resource_runcard where CATEGORY_ID = :id";
            object result = await _dbConnection.ExecuteScalarAsync(sql, new
            {
                id
            });

            return (Convert.ToInt32(result) > 0);
        }

        /// <summary>
        /// 导出分页分页
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public async  Task<TableDataModel> GetExportData(SmtResourceCategoryRequestModel model)
        {
            string sql = @"   SELECT ROW_NUMBER() OVER(ORDER BY SRC.ID DESC) AS ROWNO, SRC.ID, LP.EN_DESC OBJECT_ID, SRC.VENDOR_ID,SRC.CATEGORY_ID, SRC.CATEGORY_NAME, SRC.VALID_TIME, SRC.EXPOSE_TIME,LP2.VALUE PROPERTY_FLAG, SRC.ENABLED, SRC.DESCRIPTION FROM SMT_RESOURCE_CATEGORY SRC
                              LEFT JOIN SMT_LOOKUP LP ON LP.CODE=SRC.OBJECT_ID AND LP.TYPE = 'RESOURCE_OBJECT' AND LP.ENABLED = 'Y'
                              LEFT JOIN SMT_LOOKUP LP2 ON LP2.CODE=SRC.PROPERTY_FLAG AND LP2.TYPE = 'RESOURCE_PROPERTY' AND LP2.ENABLED = 'Y' ";

            string conditions = " WHERE SRC.ID > 0 ";

            if (model.OBJECT_ID != null && model.OBJECT_ID > 0)
            {
                conditions += $"and OBJECT_ID = :OBJECT_ID ";
            }
            if (!model.Key.IsNullOrWhiteSpace())
            {
                conditions += $"and (instr(CATEGORY_NAME, :Key) > 0 or instr(CATEGORY_ID, :Key) > 0)";
            }

            string pagedSql = SQLBuilderClass.GetPagedSQL(sql, conditions);
            var resdata = await _dbConnection.QueryAsync<object>(pagedSql, model);
            string sqlcnt = @" SELECT COUNT(SRC.ID) FROM SMT_RESOURCE_CATEGORY SRC
                              LEFT JOIN SMT_LOOKUP LP ON LP.CODE=SRC.OBJECT_ID AND LP.TYPE = 'RESOURCE_OBJECT' AND LP.ENABLED = 'Y'
                              LEFT JOIN SMT_LOOKUP LP2 ON LP2.CODE=SRC.PROPERTY_FLAG AND LP2.TYPE = 'RESOURCE_PROPERTY' AND LP2.ENABLED = 'Y' " + conditions;

            int cnt = await _dbConnection.ExecuteScalarAsync<int>(sqlcnt, model);
            return new TableDataModel
            {
                count = cnt,
                data = resdata?.ToList(),
            };
        }

        //// <summary>
        ///// 获取表的序列
        ///// </summary>
        ///// <returns></returns>
        //public async Task<decimal> GetSEQID()
        //{
        //	string sql = "SELECT SMT_RESOURCE_CATEGORY_SEQ.NEXTVAL MY_SEQ FROM DUAL";
        //	var result = await _dbConnection.ExecuteScalarAsync(sql);
        //	return (decimal)result;
        //}

        /// <summary>
        /// 保存数据
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public async Task<decimal> SaveDataByTrans(SmtResourceCategoryModel model)
        {
            int result = 1;
            ConnectionFactory.OpenConnection(_dbConnection);
            using (var tran = _dbConnection.BeginTransaction())
            {
                try
                {
                    //新增
                    string insertSql = @"INSERT INTO SMT_RESOURCE_CATEGORY 
					(ID, OBJECT_ID, VENDOR_ID, CATEGORY_ID, CATEGORY_NAME, VALID_TIME, EXPOSE_TIME, PROPERTY_FLAG, ENABLED, DESCRIPTION, BRAND, CATEGORY_MODEL) 
					VALUES (:ID, :OBJECT_ID, :VENDOR_ID, :CATEGORY_ID, :CATEGORY_NAME, :VALID_TIME, :EXPOSE_TIME, :PROPERTY_FLAG, :ENABLED, :DESCRIPTION, :BRAND, :CATEGORY_MODEL)";
                    if (model.insertRecords != null && model.insertRecords.Count > 0)
                    {
                        foreach (var item in model.insertRecords)
                        {
                            var newid = await GetSEQ_ID();
                            var resdata = await _dbConnection.ExecuteAsync(insertSql, new
                            {
                                ID = newid,
                                item.OBJECT_ID,
                                item.VENDOR_ID,
                                item.CATEGORY_ID,
                                item.CATEGORY_NAME,
                                item.VALID_TIME,
                                item.EXPOSE_TIME,
                                item.PROPERTY_FLAG,
                                item.ENABLED,
                                item.DESCRIPTION,
                                item.BRAND,
                                item.CATEGORY_MODEL
                            }, tran);
                        }
                    }
                    //更新
                    string updateSql = @"Update smt_resource_category set OBJECT_ID =:OBJECT_ID, VENDOR_ID =:VENDOR_ID, CATEGORY_ID =:CATEGORY_ID, 
							CATEGORY_NAME =:CATEGORY_NAME, VALID_TIME =:VALID_TIME, EXPOSE_TIME =:EXPOSE_TIME, PROPERTY_FLAG =:PROPERTY_FLAG,
							ENABLED =:ENABLED, DESCRIPTION =:DESCRIPTION, BRAND =:BRAND, CATEGORY_MODEL =:CATEGORY_MODEL  
						where ID=:ID ";
                    if (model.updateRecords != null && model.updateRecords.Count > 0)
                    {
                        foreach (var item in model.updateRecords)
                        {
                            var resdata = await _dbConnection.ExecuteAsync(updateSql, new
                            {
                                item.ID,
                                item.OBJECT_ID,
                                item.VENDOR_ID,
                                item.CATEGORY_ID,
                                item.CATEGORY_NAME,
                                item.VALID_TIME,
                                item.EXPOSE_TIME,
                                item.PROPERTY_FLAG,
                                item.ENABLED,
                                item.DESCRIPTION,
                                item.BRAND,
                                item.CATEGORY_MODEL
                            }, tran);
                        }
                    }
                    //删除
                    string deleteSql = @"Delete from smt_resource_category where ID=:ID ";
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