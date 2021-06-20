/**
*┌──────────────────────────────────────────────────────────────┐
*│　描    述：接口实现                                                    
*│　作    者：嘉志科技                                            
*│　版    本：2.0    模板代码自动生成                                                
*│　创建时间：2020-03-21 14:13:11                             
*└──────────────────────────────────────────────────────────────┘
*┌──────────────────────────────────────────────────────────────┐
*│　命名空间： JZ.IMS.Repository.Oracle                                  
*│　类    名： ImportDtlRepository                                      
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
using JZ.IMS.Core.Models;
using JZ.IMS.Core.Extensions;
using System.Text;

namespace JZ.IMS.Repository.Oracle
{
    public class ImportDtlRepository : BaseRepository<ImportDtl, Decimal>, IImportDtlRepository
    {
        public ImportDtlRepository(IOptionsSnapshot<DbOption> options)
        {
            _dbOption = options.Get("iWMS");
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
		public async Task<decimal> GetSEQID()
        {
            string sql = "SELECT IMPORT_DTL_SEQ.NEXTVAL MY_SEQ FROM DUAL";
            var result = await _dbConnection.ExecuteScalarAsync(sql);
            return (decimal)result;
        }

        /// <summary>
        /// 获取表的序列
        /// </summary>
        /// <returns></returns>
        public async Task<decimal> GetMainSEQID()
        {
            string sql = "SELECT IMPORT_MST_SEQ.NEXTVAL MY_SEQ FROM DUAL";
            var result = await _dbConnection.ExecuteScalarAsync(sql);
            return (decimal)result;
        }

        /// <summary>
        /// 获取所有导入主表集
        /// </summary>
        /// <returns></returns>
		public async Task<List<ImportMst>> GetImportMstList()
        {
            string sql = "SELECT * FROM import_mst order by id";
            var result = await _dbConnection.QueryAsync<ImportMst>(sql);

            return result?.ToList();
        }

        /// <summary>
        /// 获取导入主表信息
        /// </summary>
        /// <param name="table_name"></param>
        /// <returns></returns>
		public async Task<ImportMst> GetImportMst(string table_name)
        {
            string sql = "SELECT * FROM import_mst where totable_name = :table_name";
            var result = await _dbConnection.QueryAsync<ImportMst>(sql, new { table_name });

            return result?.FirstOrDefault();
        }

        /// <summary>
        /// 获取表对应的导入字段
        /// </summary>
        /// <param name="tableName">表名</param>
        public List<ImportItemVM> GetTemplateInfo(string tableName)
        {
            List<ImportItemVM> result = new List<ImportItemVM>();
            List<string> excludeItem = new List<string> { "ID", "VERSION", "ENABLE_BILL_ID", "DISABLE_BILL_ID",
                "ATTRIBUTE4", "ATTRIBUTE5"};
            string[] excelItem = new string[] { "A", "B", "C", "D", "E", "F", "G", "H", "I", "J", "K", "L", "M", "N", "O", "P", "Q", "R", "S", "T", "U",
                "V", "W", "X", "Y", "Z","AA", "AB", "AC", "AD", "AE", "AF", "AG", "AH", "AI", "AJ", "AK", "AL", "AM", "AN", "AO", "AP", "AQ", "AR", "AS",
                "AT", "AU","AV", "AW", "AX", "AY", "AZ", "BA", "BB", "BC", "BD", "BE", "BF", "BG", "BH" };
            DatabaseType dbType = ConnectionFactory.GetDataBaseType(_dbOption.DbType);
            List<DbTable> tables = new List<DbTable>();
            using (var dbConnection = ConnectionFactory.CreateConnection(dbType, _dbOption.ConnectionString))
            {
                tables = dbConnection.GetCurrentDatabaseTableList(dbType, tableName.ToUpper(), _dbOption.DbUser);
            }

            if (tables != null && tables.Any())
            {
                var curtable = tables.FirstOrDefault();
                int idx = 1;
                foreach (var column in curtable.Columns)
                {
                    //排除的字段 
                    if (excludeItem.IndexOf(column.ColName.ToUpper()) != -1)
                        continue;

                    result.Add(new ImportItemVM
                    {
                        COLUMN_NAME = column.ColName,
                        COLUMN_CAPTION = column.Comments ?? string.Empty,
                        IS_UNIQUE = (column.IsPrimaryKey) ? 1 : 0,
                        ISNULL_ABLE = (column.IsPrimaryKey) ? 0 : ((column.IsNullable) ? 1 : 0),
                        EXCEL_ITEM = (idx <= 60) ? excelItem[idx - 1] : string.Empty,
                        COLUMN_TYPE = column.ColumnType,
                    });
                    idx++;
                }
            }

            return result;
        }

        /// <summary>
        /// 保存表信息数据
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public async Task<decimal> SaveMainDataByTrans(ImportMstModel model)
        {
            int result = 1;
            ConnectionFactory.OpenConnection(_dbConnection);
            using (var tran = _dbConnection.BeginTransaction())
            {
                try
                {
                    //新增
                    string insertSql = @"insert into IMPORT_MST 
					(ID,TOTABLE_NAME,DESC_CN,DESC_EN,USED_SEQUENCE) 
					VALUES (:ID,:TOTABLE_NAME,:DESC_CN,:DESC_EN,:USED_SEQUENCE)";
                    if (model.InsertRecords != null && model.InsertRecords.Count > 0)
                    {
                        foreach (var item in model.InsertRecords)
                        {
                            var newid = await GetMainSEQID();
                            var resdata = await _dbConnection.ExecuteAsync(insertSql, new
                            {
                                ID = newid,
                                item.TOTABLE_NAME,
                                item.DESC_CN,
                                item.DESC_EN,
                                item.USED_SEQUENCE,

                            }, tran);
                        }
                    }
                    //更新
                    string updateSql = @"Update IMPORT_MST set TOTABLE_NAME=:TOTABLE_NAME,DESC_CN=:DESC_CN,DESC_EN=:DESC_EN,USED_SEQUENCE=:USED_SEQUENCE  
						where ID=:ID ";
                    if (model.UpdateRecords != null && model.UpdateRecords.Count > 0)
                    {
                        foreach (var item in model.UpdateRecords)
                        {
                            var resdata = await _dbConnection.ExecuteAsync(updateSql, new
                            {
                                item.ID,
                                item.TOTABLE_NAME,
                                item.DESC_CN,
                                item.DESC_EN,
                                item.USED_SEQUENCE,

                            }, tran);
                        }
                    }
                    //删除
                    string deleteSql = @"Delete from IMPORT_MST where ID=:ID ";
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
        /// 保存数据
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public async Task<decimal> SaveDataByTrans(ImportDtlModel model)
        {
            int result = 1;
            ConnectionFactory.OpenConnection(_dbConnection);
            using (var tran = _dbConnection.BeginTransaction())
            {
                try
                {
                    //新增
                    string insertSql = @"insert into IMPORT_DTL 
					(ID,MST_ID,COLUMN_NAME,COLUMN_CAPTION,IS_UNIQUE,ISNULL_ABLE,UPDATE_BY,UPDATE_TIME,EXCEL_ITEM,REFERENCE_SQL,LISTVALIDATION_SQL) 
					VALUES (:ID,:MST_ID,:COLUMN_NAME,:COLUMN_CAPTION,:IS_UNIQUE,:ISNULL_ABLE,:UPDATE_BY,SYSDATE,:EXCEL_ITEM,:REFERENCE_SQL,:LISTVALIDATION_SQL)";
                    if (model.insertRecords != null && model.insertRecords.Count > 0)
                    {
                        foreach (var item in model.insertRecords)
                        {
                            var newid = await GetSEQID();
                            var resdata = await _dbConnection.ExecuteAsync(insertSql, new
                            {
                                ID = newid,
                                MST_ID = model.ID,
                                item.COLUMN_NAME,
                                item.COLUMN_CAPTION,
                                item.IS_UNIQUE,
                                item.ISNULL_ABLE,
                                UPDATE_BY = model.UserName,
                                item.EXCEL_ITEM,
                                item.REFERENCE_SQL,
                                item.LISTVALIDATION_SQL,
                            }, tran);
                        }
                    }
                    //更新
                    string updateSql = @"Update IMPORT_DTL set MST_ID=:MST_ID,COLUMN_NAME=:COLUMN_NAME,COLUMN_CAPTION=:COLUMN_CAPTION,IS_UNIQUE=:IS_UNIQUE,
                                         ISNULL_ABLE=:ISNULL_ABLE,UPDATE_BY=:UPDATE_BY,UPDATE_TIME=SYSDATE,EXCEL_ITEM=:EXCEL_ITEM,REFERENCE_SQL=:REFERENCE_SQL,
                                         LISTVALIDATION_SQL =:LISTVALIDATION_SQL 
						                 Where ID=:ID ";
                    if (model.updateRecords != null && model.updateRecords.Count > 0)
                    {
                        foreach (var item in model.updateRecords)
                        {
                            var resdata = await _dbConnection.ExecuteAsync(updateSql, new
                            {
                                item.ID,
                                item.MST_ID,
                                item.COLUMN_NAME,
                                item.COLUMN_CAPTION,
                                item.IS_UNIQUE,
                                item.ISNULL_ABLE,
                                item.UPDATE_BY,
                                item.EXCEL_ITEM,
                                item.REFERENCE_SQL,
                                item.LISTVALIDATION_SQL,
                            }, tran);
                        }
                    }
                    //删除
                    string deleteSql = @"Delete from IMPORT_DTL where ID=:ID ";
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

        /// <summary>
        /// 项目是否已存在
        /// </summary>
        /// <param name="table_name">表名</param>
        /// <param name="field_name">字段名</param>
        /// <param name="field_val">字段值</param>
        /// <returns></returns>
        public new async Task<bool> ItemIsExist(string table_name, string field_name, string field_val)
        {
            string sql = $"select count(0) from {table_name} where {field_name} = :key";
            object result = await _dbConnection.ExecuteScalarAsync(sql, new
            {
                key = field_val
            });
            return (Convert.ToInt32(result) > 0);
        }

        // <summary>
        /// 获取表的序列
        /// </summary>
        /// <returns></returns>
        public async Task<string> GetImportSEQID(string seq_name)
        {
            string result = string.Empty;
            if (seq_name.ToLower() == "guid")
            {
                result = System.Guid.NewGuid().ToString("N");
            }
            else
            {
                string sql = $"SELECT {seq_name}.NEXTVAL MY_SEQ FROM DUAL";
                result = Convert.ToString(await _dbConnection.ExecuteScalarAsync(sql));
            }
            return result;
        }

        /// <summary>
        /// 获取SQL对应的值
        /// </summary>
        /// <param name="sql"></param>
        /// <param name="key"></param>
        /// <returns></returns>
        public async Task<string> GetValueByKey(string sql, string key)
        {
            string result = string.Empty;
            if (sql != null && sql.Trim().Length > 0)
            {
                var obj = await _dbConnection.ExecuteScalarAsync<string>(sql, new { key });
                result = obj?.ToString() ?? string.Empty;
            }
            else
            {
                result = key;
            }
            return result;
        }

        /// <summary>
        /// 保存导入的Excel数据数据
        /// </summary>
        /// <param name="excelItem">导入的Excel数据列表</param>
        /// <param name="tplList">导入模板数据列表</param>
        /// <param name="table_name">导入目标表名称</param>
        /// <returns></returns>
        public async Task<ImportResult> SaveImportExcelData(List<ImportExcelItem> excelItem, List<ImportDtl> tplList, string table_name)
        {
            ImportResult result = new ImportResult();
            result.Result = 1;
            var mst = await GetImportMst(table_name);
            string seq_name = mst.USED_SEQUENCE;
            ConnectionFactory.OpenConnection(_dbConnection);
            using (var tran = _dbConnection.BeginTransaction())
            {
                try
                {
                    //新增
                    string insertSql = @"insert into {TableName} 
                    (ID,{ColumnNameList}) 
					VALUES (:ID,{ParaList})";

                    var columnList = new StringBuilder();
                    var paraList = new StringBuilder();
                    foreach (var column in tplList)
                    {
                        columnList.Append(column.COLUMN_NAME + ",");
                        paraList.Append(":" + column.COLUMN_NAME + ",");
                    }
                    insertSql = insertSql.Replace("{TableName}", table_name)
                              .Replace("{ColumnNameList}", columnList.ToString().Substring(0, columnList.ToString().Length - 1))
                              .Replace("{ParaList}", paraList.ToString().Substring(0, paraList.ToString().Length - 1));

                    var template = GetTemplateInfo(table_name);
                    string old_param = string.Empty; string new_param = string.Empty;
                    foreach (var tpItem in template)
                    {
                        if (tpItem.COLUMN_TYPE.ToUpper() == "DATE")
                        {
                            old_param = ":" + tpItem.COLUMN_NAME;
                            new_param = $"to_date({old_param}, 'yyyy-mm-dd HH24:MI:SS')";
                            insertSql = insertSql.Replace(old_param, new_param);
                        }
                    }

                    DynamicParameters pars = new DynamicParameters();
                    if (excelItem != null && excelItem.Count > 0)
                    {
                        string param = string.Empty, excel_val = string.Empty, value = string.Empty;
                        int j = 1;
                        foreach (var item in excelItem)
                        {
                            //参数定义
                            var newid = await GetImportSEQID(seq_name);
                            pars.Add("ID", newid);

                            for (int i = 0; i < tplList.Count; i++)
                            {
                                param = tplList[i].COLUMN_NAME;
                                excel_val = item.GetType().GetProperty($"Column{i + 1}").GetValue(item)?.ToString() ?? string.Empty;
                                value = await GetValueByKey(tplList[i].REFERENCE_SQL, excel_val.Trim());
                                if (value.IsNullOrWhiteSpace() && !excel_val.IsNullOrWhiteSpace())
                                {
                                    result.Result = -1;
                                    result.ErrInfo = new ImportErrInfo
                                    {
                                        Index = j + 1,
                                        COLUMN_CAPTION = tplList[i].COLUMN_CAPTION,
                                    };
                                    tran.Rollback();
                                    return result;
                                }
                                pars.Add(param, value);
                            }
                            var resdata = await _dbConnection.ExecuteAsync(insertSql, pars, tran);
                            j++;
                        }
                    }
                    tran.Commit();
                }
                catch (Exception ex)
                {
                    result.Result = -1;
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