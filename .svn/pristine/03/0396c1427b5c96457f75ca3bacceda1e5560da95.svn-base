/**
*┌──────────────────────────────────────────────────────────────┐
*│　描    述：接口实现                                                    
*│　作    者：嘉志科技                                            
*│　版    本：2.0    模板代码自动生成                                                
*│　创建时间：2020-04-03 11:58:55                             
*└──────────────────────────────────────────────────────────────┘
*┌──────────────────────────────────────────────────────────────┐
*│　命名空间： JZ.IMS.Repository.Oracle                                  
*│　类    名： SfcsProductMultiRuncardRepository                                      
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
    public class SfcsProductMultiRuncardRepository : BaseRepository<SfcsProductMultiRuncard, Decimal>, ISfcsProductMultiRuncardRepository
    {
        public SfcsProductMultiRuncardRepository(IOptionsSnapshot<DbOption> options)
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
            string sql = "SELECT MES_SEQ_ID.NEXTVAL MY_SEQ FROM DUAL";
            var result = await _dbConnection.ExecuteScalarAsync(sql);
            return (decimal)result;
        }

        /// <summary>
        /// 获取制程工序列表
        /// </summary>
        /// <param name="route_id">制程ID</param>
        /// <returns></returns>
        public async Task<List<SfcsOperations>> GetRouteConfigLists(decimal route_id)
        {
            string sql = @"SELECT SRC.PRODUCT_OPERATION_CODE as ID, OP.OPERATION_NAME, OP.DESCRIPTION 
                           FROM SFCS_ROUTE_CONFIG SRC 
                              INNER JOIN SFCS_OPERATIONS OP ON SRC.CURRENT_OPERATION_ID = OP.ID 
                           WHERE SRC.ROUTE_ID =:ROUTE_ID ORDER BY SRC.ORDER_NO ";
            var resdata = await _dbConnection.QueryAsync<SfcsOperations>(sql, new { ROUTE_ID = route_id });
            return resdata?.ToList();
        }

        /// <summary>
        /// 获取连板配置
        /// </summary>
        /// <param name="part_no">料号</param>
        /// <param name="route_id">制程ID</param>
        /// <returns></returns>
        public async Task<SfcsProductMultiRuncard> GetSfcsProductMultiRuncard(string part_no, decimal route_id)
        {
            string sql = @"SELECT SPMR.* FROM SFCS_PRODUCT_MULTI_RUNCARD SPMR 
                           WHERE BREAK_OPERATION_CODE IN (SELECT PRODUCT_OPERATION_CODE  
                           FROM SFCS_ROUTE_CONFIG WHERE ROUTE_ID = :ROUTE_ID)
                           AND PART_NO = :PART_NO ";
            var resdata = await _dbConnection.QueryAsync<SfcsProductMultiRuncard>(sql,
            new
            {
                ROUTE_ID = route_id,
                PART_NO = part_no,
            });
            return resdata?.FirstOrDefault();
        }

        /// <summary>
        /// 保存数据
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public async Task<decimal> SaveDataByTrans(SfcsProductMultiRuncardAddOrModifyModel model)
        {
            int result = 1;
            ConnectionFactory.OpenConnection(_dbConnection);
            using (var tran = _dbConnection.BeginTransaction())
            {
                try
                {
                    if (model.ID == 0)
                    {
                        //新增
                        string insertSql = @"insert into SFCS_PRODUCT_MULTI_RUNCARD 
					    (ID,PART_NO,PRODUCT_UNITAGE,LINK_OPERATION_CODE,BREAK_OPERATION_CODE,WHOLE_FAIL_FLAG,ENABLED) 
					    VALUES (:ID,:PART_NO,0,:LINK_OPERATION_CODE,:BREAK_OPERATION_CODE,'Y',:ENABLED)";
                        var newid = await GetSEQID();
                        var resdata = await _dbConnection.ExecuteAsync(insertSql, new
                        {
                            ID = newid,
                            model.PART_NO,
                            model.LINK_OPERATION_CODE,
                            model.BREAK_OPERATION_CODE,
                            model.ENABLED,
                        }, tran);
                    }
                    else
                    {
                        //更新
                        string updateSql = @"Update SFCS_PRODUCT_MULTI_RUNCARD set PART_NO=:PART_NO, LINK_OPERATION_CODE=:LINK_OPERATION_CODE,
                                               BREAK_OPERATION_CODE=:BREAK_OPERATION_CODE, ENABLED=:ENABLED  
						                     Where ID=:ID ";
                        var resdata = await _dbConnection.ExecuteAsync(updateSql, new
                        {
                            model.ID,
                            model.PART_NO,
                            model.LINK_OPERATION_CODE,
                            model.BREAK_OPERATION_CODE,
                            model.ENABLED,
                        }, tran);
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