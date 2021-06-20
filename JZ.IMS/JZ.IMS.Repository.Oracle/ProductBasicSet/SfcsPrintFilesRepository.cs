/*
*┌──────────────────────────────────────────────────────────────┐
*│　描    述：接口实现                                                    
*│　作    者：嘉志科技                                            
*│　版    本：2.0    模板代码自动生成                                                
*│　创建时间：2020-04-10 19:51:17                             
*└──────────────────────────────────────────────────────────────┘
*┌──────────────────────────────────────────────────────────────┐
*│　命名空间： JZ.IMS.Repository.Oracle                                  
*│　类    名： SfcsPrintFilesRepository                                      
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

namespace JZ.IMS.Repository.Oracle
{
    public class SfcsPrintFilesRepository : BaseRepository<SfcsPrintFiles, Decimal>, ISfcsPrintFilesRepository
    {
        public SfcsPrintFilesRepository(IOptionsSnapshot<DbOption> options)
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
            string sql = "SELECT ENABLED FROM SFCS_PRINT_FILES WHERE ID=:ID";
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
            string sql = "UPDATE SFCS_PRINT_FILES set ENABLED=:ENABLED WHERE ID=:Id";
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
            string sql = "SELECT SFCS_PRINT_FILES_SEQ.NEXTVAL MY_SEQ FROM DUAL";
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
            string sql = "select count(0) from SFCS_PRINT_FILES where id = :id";
            object result = await _dbConnection.ExecuteScalarAsync(sql, new
            {
                id
            });

            return (Convert.ToInt32(result) > 0);
        }

        /// <summary>
        ///查出对应的打印文件信息
        /// </summary>
        /// <param name="id">项目id</param>
        /// <returns></returns>
        public async Task<dynamic> GetPrintFiles(string id)
        {
            string sql = "SELECT * FROM SFCS_PRINT_FILES WHERE ID = :ID";
            var result = await _dbConnection.QueryAsync(sql, new
            {
                ID = id
            });
            return result?.FirstOrDefault();
        }

        /// <summary>
        ///文件名字是否 
        /// </summary>
        /// <param name="filename">文件名</param>
        /// <returns></returns>
        public async Task<bool> IsExistFileNmae(string filename)
        {
            string sql = "SELECT COUNT(0) FROM SFCS_PRINT_FILES WHERE  FILE_NAME=:FILE_NAME";
            object result = await _dbConnection.ExecuteScalarAsync(sql, new
            {
                FILE_NAME = filename
            });

            return (Convert.ToInt32(result) > 0);
        }

        /// <summary>
        /// 保存数据
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public async Task<decimal> SaveDataByTrans(SfcsPrintFilesModel model)
        {
            int result = 1;
            ConnectionFactory.OpenConnection(_dbConnection);
            using (var tran = _dbConnection.BeginTransaction())
            {
                try
                {
                    //新增
                    string insertSql = @"insert into SFCS_PRINT_FILES 
					(ID,FILE_NAME,FILE_CONTENT,LABEL_TYPE,LABEL_IMAGE,DESCRIPTION,ENABLED,FILE_TYPE,ORIGINAL_FILE_NAME,FILE_VERSION_DATE) 
					VALUES (:ID,:FILE_NAME,:FILE_CONTENT,:LABEL_TYPE,:LABEL_IMAGE,:DESCRIPTION,:ENABLED,:FILE_TYPE,:ORIGINAL_FILE_NAME,:FILE_VERSION_DATE)";
                    if (model.InsertRecords != null && model.InsertRecords.Count > 0)
                    {
                        foreach (var item in model.InsertRecords)
                        {
                            var newid = await Get_MES_SEQ_ID();
                            var resdata = await _dbConnection.ExecuteAsync(insertSql, new
                            {
                                ID = newid,
                                item.FILE_NAME,
                                item.FILE_CONTENT,
                                item.LABEL_TYPE,
                                item.LABEL_IMAGE,
                                item.DESCRIPTION,
                                item.ENABLED,
                                item.FILE_TYPE,
                                item.ORIGINAL_FILE_NAME,
                                item.FILE_VERSION_DATE,
                            }, tran);
                        }
                    }
                    //更新
                    string updateSql = @"Update SFCS_PRINT_FILES set FILE_NAME=:FILE_NAME,FILE_CONTENT=:FILE_CONTENT,LABEL_TYPE=:LABEL_TYPE,LABEL_IMAGE=:LABEL_IMAGE,DESCRIPTION=:DESCRIPTION,ENABLED=:ENABLED,FILE_TYPE=:FILE_TYPE,ORIGINAL_FILE_NAME=:ORIGINAL_FILE_NAME,FILE_VERSION_DATE=:FILE_VERSION_DATE  
						where ID=:ID ";
                    if (model.UpdateRecords != null && model.UpdateRecords.Count > 0)
                    {
                        foreach (var item in model.UpdateRecords)
                        {
                            var resdata = await _dbConnection.ExecuteAsync(updateSql, new
                            {
                                item.ID,
                                item.FILE_NAME,
                                item.FILE_CONTENT,
                                item.LABEL_TYPE,
                                item.LABEL_IMAGE,
                                item.DESCRIPTION,
                                item.ENABLED,
                                item.FILE_TYPE,
                                item.ORIGINAL_FILE_NAME,
                                item.FILE_VERSION_DATE,
                            }, tran);
                        }
                    }
                    //删除
                    string deleteSql = @"Delete from SFCS_PRINT_FILES where ID=:ID ";
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
        /// 标签打印设计文件上传数据更新
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public async Task<int> UpdateDataByTrans(SfcsPrintFilesAddOrModifyModel model,string type)
        {
            int result = 1;
            ConnectionFactory.OpenConnection(_dbConnection);
            using (var tran = _dbConnection.BeginTransaction())
            {
                try
                {
                    //更新 
                    //Update SFCS_PRINT_FILES  set FILE_NAME =:FILE_NAME,FILE_CONTENT =:FILE_CONTENT,LABEL_TYPE =:LABEL_TYPE,LABEL_IMAGE =:LABEL_IMAGE,DESCRIPTION =:DESCRIPTION,ENABLED =:ENABLED,FILE_TYPE =:FILE_TYPE,ORIGINAL_FILE_NAME =:ORIGINAL_FILE_NAME,FILE_VERSION_DATE =:FILE_VERSION_DATE
                    //WHERE ID =:ID
                    //FILE_NAME,FILE_CONTENT,LABEL_TYPE,LABEL_IMAGE,DESCRIPTION,ENABLED,FILE_TYPE,ORIGINAL_FILE_NAME,FILE_VERSION_DATE
                    //:FILE_NAME,:FILE_CONTENT,:LABEL_TYPE,:LABEL_IMAGE,:DESCRIPTION,:ENABLED,:FILE_TYPE,:ORIGINAL_FILE_NAME,:FILE_VERSION_DATE
                    string insertSql = @"INSERT INTO SFCS_PRINT_FILES (ID,{0}) VALUES (:ID,{1})";
                    string updateSql = @"Update SFCS_PRINT_FILES  set {0} WHERE ID=:ID ";
                    string sql = @"";
                    string key = @"";
                    string val = @"";
                    //新增加
                    if (model != null && type=="Insert")
                    {
                        object parmstemp = null;
                        if (model.FILE_CONTENT != null && model.LABEL_IMAGE != null)
                        {
                            key = @"FILE_NAME,FILE_CONTENT,LABEL_TYPE,LABEL_IMAGE,DESCRIPTION,ENABLED,FILE_TYPE,ORIGINAL_FILE_NAME,FILE_VERSION_DATE ";
                            val = @":FILE_NAME,:FILE_CONTENT,:LABEL_TYPE,:LABEL_IMAGE,:DESCRIPTION,:ENABLED,:FILE_TYPE,:ORIGINAL_FILE_NAME,:FILE_VERSION_DATE";
                            sql = string.Format(insertSql, key, val);
                            parmstemp = new
                            {
                                model.ID,
                                model.FILE_NAME,
                                model.FILE_CONTENT,
                                model.LABEL_TYPE,
                                model.LABEL_IMAGE,
                                model.DESCRIPTION,
                                model.ENABLED,
                                model.FILE_TYPE,
                                model.ORIGINAL_FILE_NAME,
                                model.FILE_VERSION_DATE,
                            };
                        }
                        else if (model.FILE_CONTENT != null && model.FILE_CONTENT.Length > 0)
                        {
                            key = @"FILE_NAME,FILE_CONTENT,LABEL_TYPE,DESCRIPTION,ENABLED,FILE_TYPE,ORIGINAL_FILE_NAME,FILE_VERSION_DATE";
                            val = @":FILE_NAME,:FILE_CONTENT,:LABEL_TYPE,:DESCRIPTION,:ENABLED,:FILE_TYPE,:ORIGINAL_FILE_NAME,:FILE_VERSION_DATE";
                            sql = string.Format(insertSql, key, val);
                            parmstemp = new
                            {
                                model.ID,
                                model.FILE_NAME,
                                model.FILE_CONTENT,
                                model.LABEL_TYPE,
                                model.DESCRIPTION,
                                model.ENABLED,
                                model.FILE_TYPE,
                                model.ORIGINAL_FILE_NAME,
                                model.FILE_VERSION_DATE,
                            };
                        }
                        else if (model.LABEL_IMAGE != null && model.LABEL_IMAGE.Length > 0)
                        {
                            key = @"FILE_NAME,LABEL_TYPE,LABEL_IMAGE,DESCRIPTION,ENABLED";
                            val = @":FILE_NAME,:LABEL_TYPE,:LABEL_IMAGE,:DESCRIPTION,:ENABLED";
                            sql = string.Format(insertSql, key, val);
                            parmstemp = new
                            {
                                model.ID,
                                model.FILE_NAME,
                                model.LABEL_TYPE,
                                model.LABEL_IMAGE,
                                model.DESCRIPTION,
                                model.ENABLED,
                            };
                        }
                        else {
                            key = @"FILE_NAME,LABEL_TYPE,DESCRIPTION,ENABLED";
                            val = @":FILE_NAME,:LABEL_TYPE,:DESCRIPTION,:ENABLED";
                            sql = string.Format(insertSql, key, val);
                            parmstemp = new
                            {
                                model.ID,
                                model.FILE_NAME,
                                model.LABEL_TYPE,
                                model.DESCRIPTION,
                                model.ENABLED,
                            };
                        }

                        int resdata =await _dbConnection.ExecuteAsync(sql, parmstemp, tran);

                    }

                    #region 更新

                    if (model != null && type == "Update")
                    {
                        object parmstemp = null;
                        if (model.FILE_CONTENT != null && model.LABEL_IMAGE != null)
                        {
                            sql = string.Format(updateSql, "FILE_NAME=:FILE_NAME,FILE_CONTENT=:FILE_CONTENT,LABEL_TYPE=:LABEL_TYPE,LABEL_IMAGE=:LABEL_IMAGE,DESCRIPTION=:DESCRIPTION,ENABLED=:ENABLED,FILE_TYPE=:FILE_TYPE,ORIGINAL_FILE_NAME =:ORIGINAL_FILE_NAME,FILE_VERSION_DATE =:FILE_VERSION_DATE");
                            parmstemp = new
                            {
                                model.ID,
                                model.FILE_NAME,
                                model.FILE_CONTENT,
                                model.LABEL_TYPE,
                                model.LABEL_IMAGE,
                                model.DESCRIPTION,
                                model.ENABLED,
                                model.FILE_TYPE,
                                model.ORIGINAL_FILE_NAME,
                                model.FILE_VERSION_DATE,
                            };
                        }
                        else if (model.FILE_CONTENT != null && model.FILE_CONTENT.Length > 0)
                        {
                            sql = string.Format(updateSql, " FILE_NAME=:FILE_NAME,FILE_CONTENT=:FILE_CONTENT,LABEL_TYPE=:LABEL_TYPE,DESCRIPTION=:DESCRIPTION,ENABLED=:ENABLED,FILE_TYPE=:FILE_TYPE,ORIGINAL_FILE_NAME=:ORIGINAL_FILE_NAME,FILE_VERSION_DATE=:FILE_VERSION_DATE ");
                            parmstemp = new
                            {
                                model.ID,
                                model.FILE_NAME,
                                model.FILE_CONTENT,
                                model.LABEL_TYPE,
                                model.DESCRIPTION,
                                model.ENABLED,
                                model.FILE_TYPE,
                                model.ORIGINAL_FILE_NAME,
                                model.FILE_VERSION_DATE,
                            };
                        }
                        else if (model.LABEL_IMAGE != null && model.LABEL_IMAGE.Length > 0)
                        {
                            sql = string.Format(updateSql, " FILE_NAME=:FILE_NAME,LABEL_TYPE=:LABEL_TYPE,LABEL_IMAGE=:LABEL_IMAGE,DESCRIPTION=:DESCRIPTION,ENABLED=:ENABLED ");
                            parmstemp = new
                            {
                                model.ID,
                                model.FILE_NAME,
                                model.LABEL_TYPE,
                                model.LABEL_IMAGE,
                                model.DESCRIPTION,
                                model.ENABLED,
                            };
                        }
                        else
                        {
                            sql = string.Format(updateSql, " FILE_NAME=:FILE_NAME,LABEL_TYPE=:LABEL_TYPE,DESCRIPTION=:DESCRIPTION,ENABLED=:ENABLED ");
                            parmstemp = new
                            {
                                model.ID,
                                model.FILE_NAME,
                                model.LABEL_TYPE,
                                model.DESCRIPTION,
                                model.ENABLED,
                            };
                        }

                        var resdata =await _dbConnection.ExecuteAsync(sql, parmstemp, tran);
                    }

                    #endregion
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

        public async Task<int> SavePrintFilesDetail(SfcsPrintFilesDetailAddOrModifyModel model)
        {
            int result = 1;
            ConnectionFactory.OpenConnection(_dbConnection);
            using (var tran = _dbConnection.BeginTransaction())
            {
                try
                {
                    String sql = "";
                    object parmstemp = null;
                    if (model.ID < 1)
                    {
                        sql = @"INSERT INTO SFCS_PRINT_FILES_DETAIL (ID,PRINT_FILES_ID,FILE_CONTENT,CREATE_USER,CREATE_TIME) VALUES (:ID,:PRINT_FILES_ID,:FILE_CONTENT,:CREATE_USER,SYSDATE)";
                        model.ID = await GetID("SFCS_PRINT_FILES_DETAIL_SEQ");
                        parmstemp = new
                        {
                            model.ID,
                            model.PRINT_FILES_ID,
                            model.FILE_CONTENT,
                            model.CREATE_USER,
                        };
                    }
                    else
                    {
                        sql = @"UPDATE SFCS_PRINT_FILES_DETAIL SET FILE_CONTENT = :FILE_CONTENT,CREATE_USER=:CREATE_USER,CREATE_TIME=SYSDATE WHERE ID=:ID ";
                        parmstemp = new
                        {
                            model.ID,
                            model.FILE_CONTENT,
                            model.CREATE_USER,
                        };

                    }

                    result = await _dbConnection.ExecuteAsync(sql, parmstemp, tran);
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
