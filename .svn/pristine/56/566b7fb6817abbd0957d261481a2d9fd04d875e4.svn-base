/*
*┌──────────────────────────────────────────────────────────────┐
*│　描    述：接口实现                                                    
*│　作    者：嘉志科技                                            
*│　版    本：2.0    模板代码自动生成                                                
*│　创建时间：2020-04-14 10:41:48                             
*└──────────────────────────────────────────────────────────────┘
*┌──────────────────────────────────────────────────────────────┐
*│　命名空间： JZ.IMS.Repository.Oracle                                  
*│　类    名： SfcsPrintFilesMappingRepository                                      
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
    public class SfcsPrintFilesMappingRepository : BaseRepository<SfcsPrintFilesMapping, Decimal>, ISfcsPrintFilesMappingRepository
    {
        public SfcsPrintFilesMappingRepository(IOptionsSnapshot<DbOption> options)
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
            string sql = "SELECT ENABLED FROM SFCS_PRINT_FILES_MAPPING WHERE ID=:ID";
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
            string sql = "UPDATE SFCS_PRINT_FILES_MAPPING set ENABLED=:ENABLED WHERE ID=:Id";
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
            string sql = "SELECT SFCS_PRINT_FILES_MAPPING_SEQ.NEXTVAL MY_SEQ FROM DUAL";
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
            string sql = "select count(0) from SFCS_PRINT_FILES_MAPPING where id = :id";
            object result = await _dbConnection.ExecuteScalarAsync(sql, new
            {
                id
            });

            return (Convert.ToInt32(result) > 0);
        }

        /// <summary>
        /// 分页
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public async Task<TableDataModel> GetSfcsPrintFilesMappingList(SfcsPrintFilesMappingRequestModel model)
        {
            string condition = "  ";
            if (model.CUSTOMER_ID>0)
            {
                condition += $" and MA.CUSTOMER_ID=:CUSTOMER_ID ";
            }
            if (model.MODEL_ID > 0)
            {
                condition += $" and MA.MODEL_ID=:MODEL_ID ";
            }
            if (!string.IsNullOrWhiteSpace(model.PART_NO))
            {
                condition += $" and INSTR(MA.PART_NO,:PART_NO)>0 ";
            }
            if (model.PRINT_FILE_ID>0)
            {
                condition += $" and MA.PRINT_FILE_ID=:PRINT_FILE_ID ";
            }
            if (!string.IsNullOrWhiteSpace(model.FILE_NAME))
            {
                condition += $" and INSTR(PF.FILE_NAME,:FILE_NAME) >0  ";
            }
            if (model.SITE_OPERATION_ID>0)
            {
                condition += $" and MA.SITE_OPERATION_ID=:SITE_OPERATION_ID ";
            }
            if (model.PRODUCT_FAMILY_ID > 0)
            {
                condition += $" and MA.PRODUCT_FAMILY_ID=:PRODUCT_FAMILY_ID ";
            }
            string sql = @"  SELECT  ROWNUM AS ROWNO, MA.ID,MA.CUSTOMER_ID,MA.MODEL_ID,MA.PART_NO,SC.CUSTOMER,SM.MODEL,SM.ID MODELID,MA.ENABLED,MA.AUTO_PRINT_FLAG,MA.SITE_OPERATION_ID,PF.LABEL_TYPE,SP.MEANING, PF.DESCRIPTION LABEL_DESCRIPTION,PF.FILE_NAME,MA.Print_File_Id,MA.PRODUCT_FAMILY_ID,SPF.FAMILY_NAME
                            FROM  SFCS_PRINT_FILES_MAPPING MA
                            LEFT JOIN SFCS_PRINT_FILES PF ON  MA.PRINT_FILE_ID = PF.ID
                            LEFT JOIN SFCS_CUSTOMERS SC ON  (MA.CUSTOMER_ID = SC.ID AND  SC.ENABLED='Y')
                            LEFT JOIN SFCS_MODEL SM ON  MA.MODEL_ID=SM.ID
                            LEFT JOIN SFCS_PARAMETERS SP ON (PF.LABEL_TYPE=SP.LOOKUP_CODE AND  SP.LOOKUP_TYPE='LABEL_TYPE')
                            LEFT JOIN SFCS_PRODUCT_FAMILY SPF ON MA.PRODUCT_FAMILY_ID=SPF.ID
                            WHERE 1=1  ";
            string pagedSql = SQLBuilderClass.GetPagedSQL(sql, " MA.ID ASC", condition);
            var resdata = await _dbConnection.QueryAsync<object>(pagedSql, model);

            string sqlcnt = @"select count(0) FROM   SFCS_PRINT_FILES_MAPPING MA
                            LEFT JOIN SFCS_PRINT_FILES PF ON  MA.PRINT_FILE_ID = PF.ID
                            LEFT JOIN SFCS_CUSTOMERS SC ON  (MA.CUSTOMER_ID = SC.ID AND  SC.ENABLED='Y')
                            LEFT JOIN SFCS_MODEL SM ON  MA.MODEL_ID=SM.ID
                            LEFT JOIN SFCS_PARAMETERS SP ON (PF.LABEL_TYPE=SP.LOOKUP_CODE AND  SP.LOOKUP_TYPE='LABEL_TYPE')
                            WHERE 1=1   " + condition;

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
        public async Task<decimal> SaveDataByTrans(SfcsPrintFilesMappingModel model)
        {
            int result = 1;
            ConnectionFactory.OpenConnection(_dbConnection);
            using (var tran = _dbConnection.BeginTransaction())
            {
                try
                {
                    #region 新增
                    string insertSql = @"insert into SFCS_PRINT_FILES_MAPPING 
					(ID,CUSTOMER_ID,PRODUCT_FAMILY_ID,PLATFORM_ID,JOBCODE_ID,MODEL_ID,PART_NO,PRINT_FILE_ID,SITE_OPERATION_ID,AUTO_PRINT_FLAG,ENABLED) 
					VALUES (:ID,:CUSTOMER_ID,:PRODUCT_FAMILY_ID,:PLATFORM_ID,:JOBCODE_ID,:MODEL_ID,:PART_NO,:PRINT_FILE_ID,:SITE_OPERATION_ID,:AUTO_PRINT_FLAG,:ENABLED)";
                    if (model.InsertRecords != null && model.InsertRecords.Count > 0)
                    {
                        foreach (var item in model.InsertRecords)
                        {
                            var newid = await Get_MES_SEQ_ID();
                            var resdata = await _dbConnection.ExecuteAsync(insertSql, new
                            {
                                ID = newid,
                                item.CUSTOMER_ID,
                                item.PRODUCT_FAMILY_ID,
                                item.PLATFORM_ID,
                                item.JOBCODE_ID,
                                item.MODEL_ID,
                                item.PART_NO,
                                item.PRINT_FILE_ID,
                                item.SITE_OPERATION_ID,
                                item.AUTO_PRINT_FLAG,
                                item.ENABLED,

                            }, tran);
                        }
                    }
                    #endregion

                    //更新
                    string updateSql = @"Update SFCS_PRINT_FILES_MAPPING set CUSTOMER_ID=:CUSTOMER_ID,PRODUCT_FAMILY_ID=:PRODUCT_FAMILY_ID,PLATFORM_ID=:PLATFORM_ID,JOBCODE_ID=:JOBCODE_ID,MODEL_ID=:MODEL_ID,PART_NO=:PART_NO,PRINT_FILE_ID=:PRINT_FILE_ID,SITE_OPERATION_ID=:SITE_OPERATION_ID,AUTO_PRINT_FLAG=:AUTO_PRINT_FLAG,ENABLED=:ENABLED  
						where ID=:ID ";
                    if (model.UpdateRecords != null && model.UpdateRecords.Count > 0)
                    {
                        foreach (var item in model.UpdateRecords)
                        {
                            var resdata = await _dbConnection.ExecuteAsync(updateSql, new
                            {
                                item.ID,
                                item.CUSTOMER_ID,
                                item.PRODUCT_FAMILY_ID,
                                item.PLATFORM_ID,
                                item.JOBCODE_ID,
                                item.MODEL_ID,
                                item.PART_NO,
                                item.PRINT_FILE_ID,
                                item.SITE_OPERATION_ID,
                                item.AUTO_PRINT_FLAG,
                                item.ENABLED,

                            }, tran);
                        }
                    }
                    //删除
                    string deleteSql = @"Delete from SFCS_PRINT_FILES_MAPPING where ID=:ID ";
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
    }
}