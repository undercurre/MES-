/*
*┌──────────────────────────────────────────────────────────────┐
*│　描    述：接口实现                                                    
*│　作    者：嘉志科技                                            
*│　版    本：2.0    模板代码自动生成                                                
*│　创建时间：2020-03-30 10:44:48                             
*└──────────────────────────────────────────────────────────────┘
*┌──────────────────────────────────────────────────────────────┐
*│　命名空间： JZ.IMS.Repository.Oracle                                  
*│　类    名： SfcsProductConfigRepository                                      
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
using JZ.IMS.ViewModels.ProductBasicSet.ComponentReplace;
using JZ.IMS.Core.Utilities;

namespace JZ.IMS.Repository.Oracle
{
    public class SfcsProductConfigRepository : BaseRepository<SfcsProductConfig, Decimal>, ISfcsProductConfigRepository
    {
        public SfcsProductConfigRepository(IOptionsSnapshot<DbOption> options)
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
            string sql = "SELECT ENABLED FROM SFCS_PRODUCT_CONFIG WHERE ID=:ID";
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
            string sql = "UPDATE SFCS_PRODUCT_CONFIG set ENABLED=:ENABLED WHERE ID=:Id";
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
            string sql = "SELECT SFCS_PRODUCT_CONFIG_SEQ.NEXTVAL MY_SEQ FROM DUAL";
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
            string sql = "select count(0) from SFCS_PRODUCT_CONFIG where id = :id";
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
        public async Task<TableDataModel> GetProductConfig(SfcsProductConfigRequestModel model)
        {
            string sql = @" SELECT ROW_NUMBER() OVER(ORDER BY PC.ID DESC) AS ROWNO , PC.ID,PC.PART_NO,PC.CONFIG_TYPE,SP.CHINESE,PC.CONFIG_VALUE,PC.DESCRIPTION,PC.ENABLED FROM SFCS_PRODUCT_CONFIG PC
LEFT JOIN SFCS_PARAMETERS SP ON PC.CONFIG_TYPE=SP.LOOKUP_CODE AND SP.LOOKUP_TYPE = 'PRODUCT_CONFIG_TYPE' AND SP.ENABLED = 'Y' ";
            string condition = " WHERE PC.ID > 0 ";

            if (!model.PART_NO.IsNullOrWhiteSpace())
            {
                condition += $"and PC.PART_NO=:PART_NO ";
            }
            if (model.CONFIG_TYPE > 0)
            {
                condition += $"and PC.CONFIG_TYPE =:CONFIG_TYPE ";
            }
            if (!model.CONFIG_VALUE.IsNullOrWhiteSpace())
            {
                condition += $"and instr(PC.CONFIG_VALUE,:CONFIG_VALUE) > 0 ";
            }
            if (!model.DESCRIPTION.IsNullOrWhiteSpace())
            {
                condition += $"and instr(PC.DESCRIPTION,:DESCRIPTION) > 0 ";
            }
            if (!model.ENABLED.IsNullOrWhiteSpace())
            {
                condition += $"and PC.ENABLED=:ENABLED ";
            }

            string pagedSql = SQLBuilderClass.GetPagedSQL(sql, " PC.ID DESC", condition);
            var resdata = await _dbConnection.QueryAsync<object>(pagedSql, model);
            string sqlcnt = @" SELECT COUNT(PC.ID) FROM SFCS_PRODUCT_CONFIG PC
LEFT JOIN SFCS_PARAMETERS SP ON PC.CONFIG_TYPE=SP.LOOKUP_CODE AND SP.LOOKUP_TYPE = 'PRODUCT_CONFIG_TYPE' AND SP.ENABLED = 'Y' " + condition;

            int cnt = await _dbConnection.ExecuteScalarAsync<int>(sqlcnt, model);
            return new TableDataModel
            {
                count = cnt,
                data = resdata?.ToList(),
            };
        }


        /// <summary>
        /// 根据料号判断是否存在重复的配置类型
        /// </summary>
        /// <param name="partno"></param>
        /// <returns></returns>
        public async Task<bool> ConfigTypeIsExistByPartNo(string partno, decimal? configtype)
        {
            string sql = "select count(0) from SFCS_PRODUCT_CONFIG where ENABLED='Y' and  PART_NO=:partno and  CONFIG_TYPE = :configtype ";
            object result = await _dbConnection.ExecuteScalarAsync(sql, new
            {
                partno,
                configtype
            });

            return (Convert.ToInt32(result) > 0);
        }

        /// <summary>
		/// 获取导出数据
		/// </summary>
		/// <param name="model"></param>
		/// <returns></returns>
		public async Task<TableDataModel> GetExportData(SfcsProductConfigRequestModel model)
        {
            string condition = " WHERE PC.ID > 0 ";

            if (!model.PART_NO.IsNullOrWhiteSpace())
            {
                condition += $"and PC.PART_NO=:PART_NO ";
            }
            if (model.CONFIG_TYPE > 0)
            {
                condition += $"and PC.CONFIG_TYPE =:CONFIG_TYPE ";
            }
            if (!model.CONFIG_VALUE.IsNullOrWhiteSpace())
            {
                condition += $"and instr(PC.CONFIG_VALUE,:CONFIG_VALUE) > 0 ";
            }
            if (!model.DESCRIPTION.IsNullOrWhiteSpace())
            {
                condition += $"and instr(PC.DESCRIPTION,:DESCRIPTION) > 0 ";
            }
            if (!model.ENABLED.IsNullOrWhiteSpace())
            {
                condition += $"and PC.ENABLED=:ENABLED ";
            }
            string sql = @"SELECT ROWNUM AS ROWNO, PC.ID,PC.PART_NO,SP.CHINESE as CONFIG_TYPE,PC.CONFIG_VALUE,PC.DESCRIPTION,PC.ENABLED 
                           FROM SFCS_PRODUCT_CONFIG PC
                           LEFT JOIN SFCS_PARAMETERS SP ON PC.CONFIG_TYPE=SP.LOOKUP_CODE AND SP.LOOKUP_TYPE = 'PRODUCT_CONFIG_TYPE' AND SP.ENABLED = 'Y' ";
            string pagedSql = SQLBuilderClass.GetPagedSQL(sql, " PC.ID DESC", condition);
            var resdata = await _dbConnection.QueryAsync<object>(pagedSql, model);
            string sqlcnt = @" SELECT COUNT(PC.ID) FROM SFCS_PRODUCT_CONFIG PC
                               LEFT JOIN SFCS_PARAMETERS SP ON PC.CONFIG_TYPE=SP.LOOKUP_CODE AND SP.LOOKUP_TYPE = 'PRODUCT_CONFIG_TYPE' AND SP.ENABLED = 'Y' " + condition;

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
        public async Task<decimal> SaveDataByTrans(SfcsProductConfigModel model)
        {
            int result = 1;
            ConnectionFactory.OpenConnection(_dbConnection);
            using (var tran = _dbConnection.BeginTransaction())
            {
                try
                {
                    //新增
                    string insertSql = @"INSERT INTO SFCS_PRODUCT_CONFIG 
					(ID,PART_NO,CONFIG_TYPE,CONFIG_VALUE,DESCRIPTION,ENABLED) 
					VALUES (:ID,:PART_NO,:CONFIG_TYPE,:CONFIG_VALUE,:DESCRIPTION,:ENABLED)";
                    if (model.insertRecords != null && model.insertRecords.Count > 0)
                    {
                        foreach (var item in model.insertRecords)
                        {
                            var newid = await Get_MES_SEQ_ID();
                            var resdata = await _dbConnection.ExecuteAsync(insertSql, new
                            {
                                ID = newid,
                                item.PART_NO,
                                item.CONFIG_TYPE,
                                item.CONFIG_VALUE,
                                item.DESCRIPTION,
                                item.ENABLED,
                            }, tran);
                        }
                    }
                    //更新
                    string updateSql = @"Update SFCS_PRODUCT_CONFIG set PART_NO=:PART_NO,CONFIG_TYPE=:CONFIG_TYPE,CONFIG_VALUE=:CONFIG_VALUE,DESCRIPTION=:DESCRIPTION,ENABLED=:ENABLED  
						where ID=:ID ";
                    if (model.updateRecords != null && model.updateRecords.Count > 0)
                    {
                        foreach (var item in model.updateRecords)
                        {
                            var resdata = await _dbConnection.ExecuteAsync(updateSql, new
                            {
                                item.ID,
                                item.PART_NO,
                                item.CONFIG_TYPE,
                                item.CONFIG_VALUE,
                                item.DESCRIPTION,
                                item.ENABLED,
                            }, tran);
                        }
                    }
                    ////删除
                    string deleteSql = @"Delete from SFCS_PRODUCT_CONFIG where ID=:ID ";
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
        /// 根据料号获取配置的制程id
        /// </summary>
        /// <param name="part_no"></param>
        /// <returns></returns>
        public decimal GetRouteIdByPartNo(string part_no)
        {
            decimal route_id = 0;
            try
            {
                //在工单制程没有配置的情况下 修改过站根据产品配置传制程ID
                //(SELECT LOOKUP_CODE FROM SFCS_PARAMETERS WHERE LOOKUP_TYPE = 'PRODUCT_CONFIG_TYPE'  AND ENABLED = 'Y' AND DESCRIPTION = '产品管控制程代码') = 147
                SfcsProductConfig pcModel = QueryEx<SfcsProductConfig>("SELECT * FROM SFCS_PRODUCT_CONFIG WHERE PART_NO = :PART_NO AND CONFIG_TYPE = 147 AND ENABLED = 'Y'", new { PART_NO = part_no }).FirstOrDefault();
                if (pcModel != null)
                {
                    route_id = Convert.ToDecimal(pcModel.CONFIG_VALUE);
                }
                return route_id;
            }
            catch (Exception)
            {
                return route_id;
            }
        }

        /// <summary>
        /// 获取零件信息
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public async Task<List<ImsPart>> GetDataByOldComponents(String partNo)
        {
            List<ImsPart> resultData = new List<ImsPart>();
            try
            {
                String sql = @"SELECT DESCRIPTION FROM IMS_PART WHERE CODE=:PARTNO";
                resultData = (await _dbConnection.QueryAsync<ImsPart>(sql, new {PARTNO=partNo }))?.ToList();

            }
            catch (Exception ex)
            {
                throw ex;
            }
            return resultData;
        }

        /// <summary>
        /// 零件替代的保存
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public async Task<decimal> SaveDataByOldComponents(SfcsReplaceModel<ComponentReplaceViewModel> model)
        {
            int result = 1;
            ConnectionFactory.OpenConnection(_dbConnection);
            using (var tran = _dbConnection.BeginTransaction())
            {
                try
                {
                    //更新
                    string updateSql = @"Update SFCS_COMPONENT_REPLACE set NEW_ODM_COMPONENT_PN=:NEW_ODM_COMPONENT_PN, OLD_ODM_COMPONENT_PN=:OLD_ODM_COMPONENT_PN, REPLACE_BY=:REPLACE_BY,OLD_ODM_COMPONENT_SN=:OLD_ODM_COMPONENT_SN,NEW_ODM_COMPONENT_SN=:NEW_ODM_COMPONENT_SN
						WHERE ID=:ID ";
                    if (model.UpdateRecords != null && model.UpdateRecords.Count > 0)
                    {
                        foreach (var item in model.UpdateRecords)
                        {
                            var oldPrmodel = (await _dbConnection.QueryAsync<SfcsProductComponents>("SELECT * FROM SFCS_PRODUCT_COMPONENTS WHERE ODM_COMPONENT_PN=:ODMCOMPONENTPN AND PART_NO=:PART_NO AND COLLECT_OPERATION_ID=:COLLECT_OPERATION_ID ", new
                            {
                                ODMCOMPONENTPN = item.OldODMComponentPn,
                                PART_NO=item.PartNo,
                                COLLECT_OPERATION_ID=item.CollectOperationID
                            }))?.FirstOrDefault();

                            //格式验证
                            if (oldPrmodel != null && !oldPrmodel.DATA_FORMAT.IsNullOrWhiteSpace() && !FormatChecker.FormatCheck(item.NewODMComponentSn, oldPrmodel.DATA_FORMAT))
                            {
                                return result = -1;
                            }
                            var resdata = await _dbConnection.ExecuteAsync(updateSql, new
                            {
                                OLD_ODM_COMPONENT_SN = item.OldODMComponentSn,
                                NEW_ODM_COMPONENT_SN = item.NewODMComponentSn,
                                NEW_ODM_COMPONENT_PN = item.NewODMComponentPn,
                                OLD_ODM_COMPONENT_PN = item.OldODMComponentPn,
                                REPLACE_BY = model.UserName
                            }, tran);
                        }
                    }

                    //新增
                    string insertSql = @"INSERT INTO SFCS_COMPONENT_REPLACE(REPLACE_COMPONENT_ID,NEW_ODM_COMPONENT_PN,OLD_ODM_COMPONENT_PN,REPLACE_BY, REPLACE_TIME,OLD_ODM_COMPONENT_SN,NEW_ODM_COMPONENT_SN)
                                         VALUES(SFCS_COMPONENT_REPLACE_SEQ.NEXTVAL,:NEW_ODM_COMPONENT_PN, :OLD_ODM_COMPONENT_PN, :REPLACE_BY,SYSDATE,:OLD_ODM_COMPONENT_SN,:NEW_ODM_COMPONENT_SN)";
                    if (model.InsertRecords != null && model.InsertRecords.Count > 0)
                    {
                        foreach (var item in model.InsertRecords)
                        {
                            var oldPrmodel = (await _dbConnection.QueryAsync<SfcsProductComponents>("SELECT * FROM SFCS_PRODUCT_COMPONENTS WHERE ODM_COMPONENT_PN=:ODMCOMPONENTPN AND PART_NO=:PART_NO AND COLLECT_OPERATION_ID=:COLLECT_OPERATION_ID ", new
                            {
                                ODMCOMPONENTPN = item.OldODMComponentPn,
                                PART_NO = item.PartNo,
                                COLLECT_OPERATION_ID = item.CollectOperationID
                            }))?.FirstOrDefault();

                            //格式验证
                            if (oldPrmodel != null&& !oldPrmodel.DATA_FORMAT.IsNullOrWhiteSpace() && !FormatChecker.FormatCheck(item.NewODMComponentSn, oldPrmodel.DATA_FORMAT))
                            {
                                return result = -1;
                            }
                            var resdata = await _dbConnection.ExecuteAsync(insertSql, new
                            {
                                OLD_ODM_COMPONENT_SN = item.OldODMComponentSn,
                                NEW_ODM_COMPONENT_SN = item.NewODMComponentSn,
                                NEW_ODM_COMPONENT_PN = item.NewODMComponentPn,
                                OLD_ODM_COMPONENT_PN = item.OldODMComponentPn,
                                REPLACE_BY = model.UserName
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