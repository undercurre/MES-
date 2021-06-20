/*
*┌──────────────────────────────────────────────────────────────┐
*│　描    述：接口实现                                                    
*│　作    者：嘉志科技                                            
*│　版    本：2.0    模板代码自动生成                                                
*│　创建时间：2020-04-08 08:52:34                             
*└──────────────────────────────────────────────────────────────┘
*┌──────────────────────────────────────────────────────────────┐
*│　命名空间： JZ.IMS.Repository.Oracle                                  
*│　类    名： SfcsRoutesRepository                                      
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
using JZ.IMS.Core.Utilities;

namespace JZ.IMS.Repository.Oracle
{
    public class SfcsRoutesRepository : BaseRepository<SfcsRoutes, Decimal>, ISfcsRoutesRepository
    {
        public SfcsRoutesRepository(IOptionsSnapshot<DbOption> options)
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
            string sql = "SELECT ENABLED FROM SFCS_ROUTES WHERE ID=:ID";
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
            string sql = "UPDATE SFCS_ROUTES set ENABLED=:ENABLED WHERE ID=:Id";
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
            string sql = "SELECT SFCS_ROUTES_SEQ.NEXTVAL MY_SEQ FROM DUAL";
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
            string sql = "select count(0) from SFCS_ROUTES where id = :id";
            object result = await _dbConnection.ExecuteScalarAsync(sql, new
            {
                id
            });

            return (Convert.ToInt32(result) > 0);
        }

        /// <summary>
        /// 查工序
        /// </summary>
        /// <returns></returns>
        public async Task<SfcsOperationsListModel> GetOperationDataTable(decimal current_operation_id)
        {
            string sql = @"SELECT SO.* FROM SFCS_OPERATIONS SO WHERE ID=:ID";
            var result = await _dbConnection.QueryAsync<SfcsOperationsListModel>(sql, new
            {
                ID = current_operation_id
            });
            return result?.ToList().FirstOrDefault();
        }

        /// <summary>
        /// 查制程名称
        /// </summary>
        /// <returns></returns>
        public async Task<dynamic> GetRoutesList(decimal ID)
        {
            string sql = @"SELECT SR.* FROM SFCS_ROUTES SR WHERE ID=:ID";
            var result = await _dbConnection.QueryAsync(sql, new
            {
                ID = ID
            });
            return result?.ToList().FirstOrDefault();
        }

        /// <summary>
        /// 尋找刪除的當前工序在Runcard中是否存在WIP_OPERATION,LAST_OPERATION以及未維修的工序
        /// </summary>
        /// <param name="operationCode"></param>
        /// <param name="routeID"></param>
        /// <returns></returns>
        public async Task<bool> CheckWIPExisted(decimal operationCode, decimal routeID)
        {
            if (operationCode.IsNullOrEmpty() || routeID.IsNullOrEmpty())
            {
                return false;
            }

            #region WIP
            string wipsql = @" SELECT COUNT(0) FROM SFCS_RUNCARD SR WHERE WIP_OPERATION=:WIP_OPERATION AND ROUTE_ID=:ROUTEID ";
            var wipresult = await _dbConnection.ExecuteScalarAsync(wipsql, new
            {
                WIP_OPERATION = operationCode,
                ROUTEID = routeID
            });
            if (Convert.ToInt32(wipresult) > 0)
            {
                return true;
            }
            #endregion

            #region 不良
            string defectsql = @" SELECT COUNT(0) FROM SFCS_WO SW, SFCS_COLLECT_DEFECTS SCD, SFCS_OPERATION_SITES SOS 
                                  WHERE ROUTE_ID = :ROUTE_ID 
                                  AND SW.ID = SCD.WO_ID AND SCD.DEFECT_SITE_ID = SOS.ID
                                  AND OPERATION_ID = :OPERATION_ID AND SCD.REPAIR_FLAG ='N' ";
            var defectsult = await _dbConnection.ExecuteScalarAsync(defectsql, new
            {
                ROUTE_ID = routeID,
                OPERATION_ID = operationCode
            });
            if (Convert.ToInt32(defectsult) > 0)
            {
                return true;
            }
            #endregion

            #region 维修后重流中

            string Runcardsql = @" SELECT COUNT(0) FROM SFCS_RUNCARD SR WHERE LAST_OPERATION=:LAST_OPERATION AND ROUTE_ID=:ROUTEID ";
            var runcardresult = await _dbConnection.ExecuteScalarAsync(Runcardsql, new
            {
                LAST_OPERATION = operationCode,
                ROUTEID = routeID
            });
            if (Convert.ToInt32(runcardresult) > 0)
            {
                return true;
            }

            #endregion

            return false;

        }

        /// <summary>
        /// 制程名称保存数据
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public async Task<decimal> SaveDataByTrans(SfcsRoutesModel model)
        {
            int result = 1;
            ConnectionFactory.OpenConnection(_dbConnection);
            using (var tran = _dbConnection.BeginTransaction())
            {
                try
                {
                    //新增
                    string insertSql = @"insert into SFCS_ROUTES 
					(ID,PART_NO,ROUTE_NAME,ROUTE_CLASS,ROUTE_TYPE,DESCRIPTION,ENABLED) 
					VALUES (:ID,:PART_NO,:ROUTE_NAME,:ROUTE_CLASS,:ROUTE_TYPE,:DESCRIPTION,:ENABLED)";
                    if (model.InsertRecords != null && model.InsertRecords.Count > 0)
                    {
                        foreach (var item in model.InsertRecords)
                        {
                            var newid = await Get_MES_SEQ_ID();
                            var resdata = await _dbConnection.ExecuteAsync(insertSql, new
                            {
                                ID = newid,
                                item.PART_NO,
                                item.ROUTE_NAME,
                                item.ROUTE_CLASS,
                                item.ROUTE_TYPE,
                                item.DESCRIPTION,
                                item.ENABLED,

                            }, tran);
                        }
                    }
                    //更新
                    string updateSql = @"Update SFCS_ROUTES set PART_NO=:PART_NO,ROUTE_NAME=:ROUTE_NAME,ROUTE_CLASS=:ROUTE_CLASS,ROUTE_TYPE=:ROUTE_TYPE,DESCRIPTION=:DESCRIPTION,ENABLED=:ENABLED  
						where ID=:ID ";
                    if (model.UpdateRecords != null && model.UpdateRecords.Count > 0)
                    {
                        foreach (var item in model.UpdateRecords)
                        {
                            var resdata = await _dbConnection.ExecuteAsync(updateSql, new
                            {
                                item.ID,
                                item.PART_NO,
                                item.ROUTE_NAME,
                                item.ROUTE_CLASS,
                                item.ROUTE_TYPE,
                                item.DESCRIPTION,
                                item.ENABLED,

                            }, tran);
                        }
                    }
                    //删除
                    string deleteSql = @"Delete from SFCS_ROUTES where ID=:ID ";
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
        /// 制程设定保存数据
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public async Task<decimal> RoutesConfigSaveData(SfcsRouteConfigModel model)
        {
            int result = 1;
            ConnectionFactory.OpenConnection(_dbConnection);
            using (var tran = _dbConnection.BeginTransaction())
            {
                try
                {
                    #region 删除
                    
                    if (model.RemoveRecords != null && model.RemoveRecords.Count > 0)
                    {
                        foreach (var item in model.RemoveRecords)
                        {
                            string deleteSql = @" DELETE SFCS_ROUTE_CONFIG WHERE ID =:ID ";

                            var resdata = await _dbConnection.ExecuteAsync(deleteSql, new
                            {
                                item.ID
                            }, tran);
                        }
                    }
                    #endregion
                    //更新
                    string updateSql = @" UPDATE SFCS_ROUTE_CONFIG
                                          SET ROUTE_ID = :ROUTE_ID,
                                          CURRENT_OPERATION_ID = :CURRENT_OPERATION_ID,
                                          PREVIOUS_OPERATION_ID = :PREVIOUS_OPERATION_ID,
                                          NEXT_OPERATION_ID = :NEXT_OPERATION_ID,
                                          REPAIR_OPERATION_ID = :REPAIR_OPERATION_ID,
                                          REWORK_OPERATION_ID = :REWORK_OPERATION_ID,
                                          ORDER_NO = :ORDER_NO
                                          WHERE ID = :ID ";
                    if (model.UpdateRecords != null && model.UpdateRecords.Count > 0)
                    {
                        foreach (var item in model.UpdateRecords)
                        {
                            var resdata = await _dbConnection.ExecuteAsync(updateSql, new
                            {
                                item.ID,
                                item.ROUTE_ID,
                                item.CURRENT_OPERATION_ID,
                                item.PREVIOUS_OPERATION_ID,
                                item.NEXT_OPERATION_ID,
                                item.REPAIR_OPERATION_ID,
                                item.REWORK_OPERATION_ID,
                                item.ORDER_NO
                            }, tran);
                        }
                    }

                    //新增
                    string insertSql = @" INSERT INTO SFCS_ROUTE_CONFIG(ID,  ROUTE_ID, PRODUCT_OPERATION_CODE, CURRENT_OPERATION_ID, PREVIOUS_OPERATION_ID, NEXT_OPERATION_ID, REPAIR_OPERATION_ID, REWORK_OPERATION_ID, ORDER_NO)
                                          VALUES(:ID,:ROUTE_ID,SFCS_PRODUCT_OP_CODE_SEQ.NEXTVAL,:CURRENT_OPERATION_ID,:PREVIOUS_OPERATION_ID,:NEXT_OPERATION_ID,:REPAIR_OPERATION_ID,:REWORK_OPERATION_ID,:ORDER_NO)";
                    if (model.InsertRecords != null && model.InsertRecords.Count > 0)
                    {
                        foreach (var item in model.InsertRecords)
                        {
                            var newid = await Get_MES_SEQ_ID();

                            var resdata = await _dbConnection.ExecuteAsync(insertSql, new
                            {
                                ID = newid,
                                item.ROUTE_ID,
                                item.CURRENT_OPERATION_ID,
                                item.PREVIOUS_OPERATION_ID,
                                item.NEXT_OPERATION_ID,
                                item.REPAIR_OPERATION_ID,
                                item.REWORK_OPERATION_ID,
                                item.ORDER_NO,
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
        /// 制程设定保存数据
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public async Task<decimal> RoutesConfigSaveDataEx(SfcsRouteConfigModel model)
        {
            int result = 1;
            ConnectionFactory.OpenConnection(_dbConnection);
            using (var tran = _dbConnection.BeginTransaction())
            {
                try
                {
                    //新增
                    string deleteSql = @" DELETE SFCS_ROUTE_CONFIG WHERE ROUTE_ID =:ROUTE_ID ";
                    string insertSql = @" INSERT INTO SFCS_ROUTE_CONFIG(ID,  ROUTE_ID, PRODUCT_OPERATION_CODE, CURRENT_OPERATION_ID, PREVIOUS_OPERATION_ID, NEXT_OPERATION_ID, REPAIR_OPERATION_ID, REWORK_OPERATION_ID, ORDER_NO)
                                          VALUES(:ID,:ROUTE_ID,:PRODUCT_OPERATION_CODE,:CURRENT_OPERATION_ID,:PREVIOUS_OPERATION_ID,:NEXT_OPERATION_ID,:REPAIR_OPERATION_ID,:REWORK_OPERATION_ID,:ORDER_NO)";
                    if (model.InsertRecords != null && model.InsertRecords.Count > 0)
                    {
                        var resdata = await _dbConnection.ExecuteAsync(deleteSql, new
                        {
                            model.InsertRecords[0].ROUTE_ID
                        }, tran);

                        foreach (var item in model.InsertRecords)
                        {
                            var newid = await Get_MES_SEQ_ID();
                            if (item.PRODUCT_OPERATION_CODE<=0)
                            {
                                item.PRODUCT_OPERATION_CODE = await Get_MES_SEQ_ID("SFCS_PRODUCT_OP_CODE_SEQ");
                            }
                            var addResult = await _dbConnection.ExecuteAsync(insertSql, new
                            {
                                ID = newid,
                                PRODUCT_OPERATION_CODE= item.PRODUCT_OPERATION_CODE,
                                item.ROUTE_ID,
                                item.CURRENT_OPERATION_ID,
                                item.PREVIOUS_OPERATION_ID,
                                item.NEXT_OPERATION_ID,
                                item.REPAIR_OPERATION_ID,
                                item.REWORK_OPERATION_ID,
                                item.ORDER_NO,
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










