/**
*┌──────────────────────────────────────────────────────────────┐
*│　描    述：接口实现                                                    
*│　作    者：嘉志科技                                            
*│　版    本：2.0    模板代码自动生成                                                
*│　创建时间：2020-03-30 11:58:48                             
*└──────────────────────────────────────────────────────────────┘
*┌──────────────────────────────────────────────────────────────┐
*│　命名空间： JZ.IMS.Repository.Oracle                                  
*│　类    名： SfcsCustomersRepository                                      
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
    public class SfcsCustomersRepository : BaseRepository<SfcsCustomers, decimal>, ISfcsCustomersRepository
    {
        public SfcsCustomersRepository(IOptionsSnapshot<DbOption> options)
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
            string sql = "SELECT ENABLED FROM SFCS_CUSTOMERS WHERE ID=:ID";
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
            string sql = "UPDATE SFCS_CUSTOMERS set ENABLED=:ENABLED WHERE ID=:Id";
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
            string sql = "select count(0) from SFCS_PN where CUSTOMER_ID = :id";
            object result = await _dbConnection.ExecuteScalarAsync(sql, new
            {
                id
            });

            return (Convert.ToInt32(result) > 0);
        }

        ///// <summary>
        ///// 获取父阶客户
        ///// </summary>
        ///// <returns></returns>
        //public async Task<List<IDNAME>> GetParentCustom()
        //{
        //    string sql = @"SELECT DISTINCT SC.ID, SC.CUSTOMER as NAME 
        //                   FROM SFCS_CUSTOMERS SC 
        //                  WHERE PARENT_ID IS NULL AND ENABLED = 'Y'";
        //    return (await _dbConnection.QueryAsync<IDNAME>(sql))?.ToList();
        //}

        /// <summary>
        /// 获取导出数据
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public async Task<TableDataModel> GetParentCustom(SfcsCustomersRequestModel model)
        {
            string conditions = " WHERE 1=1 ";

            if (!model.NAME.IsNullOrWhiteSpace())
            {
                conditions += $"AND instr(T.NAME ,:NAME)>0 ";
            }
            //if (!model.Key.IsNullOrWhiteSpace())
            //{
            //    conditions += $"AND (instr(SC.CUSTOMER, :Key) > 0 or instr(SC.NATIONALITY, :Key) > 0 or instr(SC.TEL, :Key) > 0 or instr(SC.MOBILE, :Key) > 0 ) ";
            //}

            string sql = @"select ROWNUM AS ROWNO,T.ID,T.NAME from (
                           SELECT DISTINCT SC.ID, SC.CUSTOMER as NAME 
                           FROM SFCS_CUSTOMERS SC 
                           WHERE PARENT_ID IS NULL AND ENABLED = 'Y'ORDER BY SC.CUSTOMER ASC) T ";

            string pagedSql = SQLBuilderClass.GetPagedSQL(sql, " T.NAME ASC", conditions);
            var resdata = await _dbConnection.QueryAsync<dynamic>(pagedSql, model);
            string sqlcnt = @"select count(*) from (
                           SELECT DISTINCT SC.ID, SC.CUSTOMER as NAME 
                           FROM SFCS_CUSTOMERS SC 
                           WHERE PARENT_ID IS NULL AND ENABLED = 'Y'ORDER BY SC.CUSTOMER ASC) T " + conditions;

            int cnt = await _dbConnection.ExecuteScalarAsync<int>(sqlcnt, model);
            return new TableDataModel
            {
                count = cnt,
                data = resdata?.ToList(),
            };
        }

        /// <summary>
        /// 加载数据
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public async Task<TableDataModel> LoadData(SfcsCustomersRequestModel model)
        {
            string conditions = " WHERE cus.ID > 0 ";
             if (model.PARENT_ID != null && model.PARENT_ID > 0)
            {
                conditions += $"and (cus.PARENT_ID =:PARENT_ID) ";
            }
            if (!model.Key.IsNullOrWhiteSpace())
            {
                conditions += $"and (instr(cus.CUSTOMER, :Key) > 0 or instr(cus.NATIONALITY, :Key) > 0 or instr(cus.TEL, :Key) > 0 or instr(cus.MOBILE, :Key) > 0 ) ";
            }
            string sql = @" select ROWNUM as rowno, sc.CUSTOMER as CustomerName, cus.* from Sfcs_Customers cus
                                left join SFCS_CUSTOMERS sc  on cus.PARENT_ID=sc.ID and sc.PARENT_ID IS NULL and  sc.ENABLED = 'Y' ";

            string pagedSql = SQLBuilderClass.GetPagedSQL(sql, " cus.ID desc", conditions);
            var resdata = await _dbConnection.QueryAsync<dynamic>(pagedSql, model);
            string sqlcnt = @"select count(1) from Sfcs_Customers cus
                             left join SFCS_CUSTOMERS sc  on cus.PARENT_ID=sc.ID and sc.PARENT_ID IS NULL and  sc.ENABLED = 'Y' " + conditions;

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
		public async Task<TableDataModel> GetExportData(SfcsCustomersRequestModel model)
        {
            string conditions = " WHERE m.ID > 0 ";

            if (model.PARENT_ID != null && model.PARENT_ID > 0)
            {
                conditions += $"and (m.PARENT_ID =:PARENT_ID) ";
            }
            if (!model.Key.IsNullOrWhiteSpace())
            {
                conditions += $"and (instr(m.CUSTOMER, :Key) > 0 or instr(m.NATIONALITY, :Key) > 0 or instr(m.TEL, :Key) > 0 or instr(m.MOBILE, :Key) > 0 ) ";
            }

            string sql = @"SELECT ROWNUM AS ROWNO,m.ID,SC.CUSTOMER AS PARENT_ID,m.CUSTOMER,m.NATIONALITY,m.TEL,m.MOBILE,m.FAX,m.CONTACT,m.POSTAL_CODE,
                              m.CITY,m.STATE,m.ADDRESS,m.ENABLED  
                           From SFCS_CUSTOMERS m  
                           LEFT JOIN SFCS_CUSTOMERS SC ON m.PARENT_ID = SC.ID AND SC.PARENT_ID IS NULL AND SC.ENABLED = 'Y' ";

            string pagedSql = SQLBuilderClass.GetPagedSQL(sql, "m.id desc", conditions);
            var resdata = await _dbConnection.QueryAsync<dynamic>(pagedSql, model);
            string sqlcnt = @"SELECT COUNT(0) From SFCS_CUSTOMERS m  
                           LEFT JOIN SFCS_CUSTOMERS SC ON m.PARENT_ID = SC.ID AND SC.PARENT_ID IS NULL AND SC.ENABLED = 'Y' " + conditions;

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
        public async Task<decimal> SaveDataByTrans(SfcsCustomersModel model)
        {
            int result = 1;
            ConnectionFactory.OpenConnection(_dbConnection);
            using (var tran = _dbConnection.BeginTransaction())
            {
                try
                {
                    //新增
                    string insertSql = @"insert into SFCS_CUSTOMERS 
					(ID,PARENT_ID,CUSTOMER,NATIONALITY,TEL,MOBILE,FAX,CONTACT,POSTAL_CODE,CITY,STATE,ADDRESS,ENABLED) 
					VALUES (:ID,:PARENT_ID,:CUSTOMER,:NATIONALITY,:TEL,:MOBILE,:FAX,:CONTACT,:POSTAL_CODE,:CITY,:STATE,:ADDRESS,:ENABLED)";
                    if (model.insertRecords != null && model.insertRecords.Count > 0)
                    {
                        foreach (var item in model.insertRecords)
                        {
                            var newid = await GetSEQID();
                            var resdata = await _dbConnection.ExecuteAsync(insertSql, new
                            {
                                ID = newid,
                                item.PARENT_ID,
                                item.CUSTOMER,
                                item.NATIONALITY,
                                item.TEL,
                                item.MOBILE,
                                item.FAX,
                                item.CONTACT,
                                item.POSTAL_CODE,
                                item.CITY,
                                item.STATE,
                                item.ADDRESS,
                                item.ENABLED,
                            }, tran);
                        }
                    }
                    //更新
                    string updateSql = @"Update SFCS_CUSTOMERS set PARENT_ID=:PARENT_ID,CUSTOMER=:CUSTOMER,NATIONALITY=:NATIONALITY,TEL=:TEL,
                                MOBILE=:MOBILE,FAX=:FAX,CONTACT=:CONTACT,POSTAL_CODE=:POSTAL_CODE,CITY=:CITY,STATE=:STATE,ADDRESS=:ADDRESS,ENABLED=:ENABLED   
						where ID=:ID ";
                    if (model.updateRecords != null && model.updateRecords.Count > 0)
                    {
                        foreach (var item in model.updateRecords)
                        {
                            var resdata = await _dbConnection.ExecuteAsync(updateSql, new
                            {
                                item.ID,
                                item.PARENT_ID,
                                item.CUSTOMER,
                                item.NATIONALITY,
                                item.TEL,
                                item.MOBILE,
                                item.FAX,
                                item.CONTACT,
                                item.POSTAL_CODE,
                                item.CITY,
                                item.STATE,
                                item.ADDRESS,
                                item.ENABLED,
                            }, tran);
                        }
                    }
                    //删除
                    //string deleteSql = @"Delete from SFCS_CUSTOMERS where ID=:ID ";
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
    }
}