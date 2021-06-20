/*
*┌──────────────────────────────────────────────────────────────┐
*│　描    述：接口实现                                                    
*│　作    者：嘉志科技                                            
*│　版    本：2.0    模板代码自动生成                                                
*│　创建时间：2020-03-30 10:44:48                             
*└──────────────────────────────────────────────────────────────┘
*┌──────────────────────────────────────────────────────────────┐
*│　命名空间： JZ.IMS.Repository.Oracle                                  
*│　类    名： SfcsPnRepository                                      
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
    public class SfcsPnRepository : BaseRepository<SfcsPn, Decimal>, ISfcsPnRepository
    {
        public SfcsPnRepository(IOptionsSnapshot<DbOption> options)
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
            string sql = "SELECT ENABLED FROM SFCS_PN WHERE ID=:ID";
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
            string sql = "UPDATE SFCS_PN set ENABLED=:ENABLED WHERE ID=:Id";
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
            string sql = "SELECT SFCS_PN_SEQ.NEXTVAL MY_SEQ FROM DUAL";
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
            string sql = "select count(0) from SFCS_PN where id = :id";
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
        public async Task<TableDataModel> GetPNCode(SfcsPnRequestModel model)
        {
            string sql = @" SELECT ROW_NUMBER() OVER(ORDER BY PN.ID DESC) AS ROWNO ,PN.ID,PN.PART_NO,PN.CUSTOMER_ID,C.CUSTOMER,PN.FAMILY_ID,PF.Family_Name,PN.MODEL_ID,M.MODEL,PN.CUSTOMER_PN,PN.BU_CODE,P.MEANING BU_MEANING,PN.CLASSIFICATION,SP.MEANING PLANTCODE,PN.PRODUCT_KIND,BP.MEANING PRODUCT_KIND_MEANING,PN.STAGE_CODE,CP.MEANING STAGE_CODE_MEANING,PN.DOUBLE_SIDE,PN.LEAD_FLAG,PN.SHIP_FLAG,PN.EDI_FLAG,PN.WARRANTY_LIMIT,PN.PHASE_IN_DATE,PN.PHASE_OUT_DATE,PN.TURNIN_TYPE,PN.DESCRIPTION,PN.CATEGORY,DP.MEANING CATEGORY_MEANING FROM SFCS_PN PN
                            LEFT JOIN SFCS_CUSTOMERS C ON PN.CUSTOMER_ID=C.ID
                            LEFT JOIN SFCS_PRODUCT_FAMILY PF ON PF.ID=PN.FAMILY_ID 
                            LEFT JOIN SFCS_MODEL M ON M.ID=PN.MODEL_ID
                            LEFT JOIN SFCS_PARAMETERS P ON (P.LOOKUP_CODE=PN.BU_CODE AND P.LOOKUP_TYPE = 'BU_CODE')
                            LEFT JOIN SFCS_PARAMETERS SP ON (SP.LOOKUP_CODE=PN.CLASSIFICATION AND SP.LOOKUP_TYPE = 'PLANT_CODE')
                            LEFT JOIN SFCS_PARAMETERS BP ON (BP.LOOKUP_CODE=PN.PRODUCT_KIND AND BP.LOOKUP_TYPE = 'PRODUCT_KIND')
                            LEFT JOIN SFCS_PARAMETERS CP ON (CP.LOOKUP_CODE=PN.STAGE_CODE AND CP.LOOKUP_TYPE = 'PRODUCT_STAGE')
                            LEFT JOIN SFCS_PARAMETERS DP ON (DP.LOOKUP_CODE=PN.CATEGORY AND DP.LOOKUP_TYPE = 'WO_CLASSIFICATION' 	AND DP.LOOKUP_CODE NOT IN (4) ) ";
            string condition = " WHERE Pn.ID > 0 ";

            if (!model.PART_NO.IsNullOrWhiteSpace())
            {
                condition += $"and PN.PART_NO=:PART_NO ";
            }
            if (model.CUSTOMER_ID > 0)
            {
                condition += $"and PN.CUSTOMER_ID =:CUSTOMER_ID ";
            }
            if (model.FAMILY_ID > 0)
            {
                condition += $"and PN.FAMILY_ID =:FAMILY_ID ";
            }
            if (model.MODEL_ID > 0)
            {
                condition += $"and PN.MODEL_ID =:MODEL_ID ";
            }
            if (!model.CUSTOMER_PN.IsNullOrWhiteSpace())
            {
                condition += $"and instr(PN.CUSTOMER_PN, :CUSTOMER_PN) > 0 ";
            }
            if (model.BU_CODE > 0)
            {
                condition += $"and PN.BU_CODE =:BU_CODE ";
            }
            if (model.CLASSIFICATION > 0)
            {
                condition += $"and PN.CLASSIFICATION =:CLASSIFICATION ";
            }
            if (model.PRODUCT_KIND > 0)
            {
                condition += $"and PN.PRODUCT_KIND =:PRODUCT_KIND ";
            }
            if (model.STAGE_CODE > 0)
            {
                condition += $" and PN.STAGE_CODE =:STAGE_CODE ";
            }
            if (!model.DOUBLE_SIDE.IsNullOrWhiteSpace())
            {
                condition += $" and instr(PN.DOUBLE_SIDE ,:DOUBLE_SIDE) > 0 ";
            }

            string pagedSql = SQLBuilderClass.GetPagedSQL(sql, "PN.ID DESC", condition);
            var resdata = await _dbConnection.QueryAsync<object>(pagedSql, model);
            string sqlcnt = @" SELECT COUNT(PN.ID) FROM SFCS_PN PN
                            LEFT JOIN SFCS_CUSTOMERS C ON PN.CUSTOMER_ID=C.ID
                            LEFT JOIN SFCS_PRODUCT_FAMILY PF ON PF.ID=PN.FAMILY_ID 
                            LEFT JOIN SFCS_MODEL M ON M.ID=PN.MODEL_ID
                            LEFT JOIN SFCS_PARAMETERS P ON (P.LOOKUP_CODE=PN.BU_CODE AND P.LOOKUP_TYPE = 'BU_CODE')
                            LEFT JOIN SFCS_PARAMETERS SP ON (SP.LOOKUP_CODE=PN.CLASSIFICATION AND SP.LOOKUP_TYPE = 'PLANT_CODE')
                            LEFT JOIN SFCS_PARAMETERS BP ON (BP.LOOKUP_CODE=PN.PRODUCT_KIND AND BP.LOOKUP_TYPE = 'PRODUCT_KIND')
                            LEFT JOIN SFCS_PARAMETERS CP ON (CP.LOOKUP_CODE=PN.STAGE_CODE AND CP.LOOKUP_TYPE = 'PRODUCT_STAGE')
                            LEFT JOIN SFCS_PARAMETERS DP ON (DP.LOOKUP_CODE=PN.CATEGORY AND DP.LOOKUP_TYPE = 'WO_CLASSIFICATION' 	AND DP.LOOKUP_CODE NOT IN (4) )  " + condition;

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
		public async Task<TableDataModel> GetExportData(SfcsPnRequestModel model)
        {
            string condition = " WHERE Pn.ID > 0 ";

            if (!model.PART_NO.IsNullOrWhiteSpace())
            {
                condition += $"and PN.PART_NO=:PART_NO ";
            }
            if (model.CUSTOMER_ID > 0)
            {
                condition += $"and PN.CUSTOMER_ID =:CUSTOMER_ID ";
            }
            if (model.FAMILY_ID > 0)
            {
                condition += $"and PN.FAMILY_ID =:FAMILY_ID ";
            }
            if (model.MODEL_ID > 0)
            {
                condition += $"and PN.MODEL_ID =:MODEL_ID ";
            }
            if (!model.CUSTOMER_PN.IsNullOrWhiteSpace())
            {
                condition += $"and instr(PN.CUSTOMER_PN, :CUSTOMER_PN) > 0 ";
            }
            if (model.BU_CODE > 0)
            {
                condition += $"and PN.BU_CODE =:BU_CODE ";
            }
            if (model.CLASSIFICATION > 0)
            {
                condition += $"and PN.CLASSIFICATION =:CLASSIFICATION ";
            }
            if (model.PRODUCT_KIND > 0)
            {
                condition += $"and PN.PRODUCT_KIND =:PRODUCT_KIND ";
            }
            if (model.STAGE_CODE > 0)
            {
                condition += $" and PN.STAGE_CODE =:STAGE_CODE ";
            }
            if (!model.DOUBLE_SIDE.IsNullOrWhiteSpace())
            {
                condition += $" and instr(PN.DOUBLE_SIDE ,:DOUBLE_SIDE) > 0 ";
            }
            string sql = @"SELECT ROWNUM AS ROWNO ,PN.ID,PN.PART_NO,C.CUSTOMER as CUSTOMER_ID,PF.Family_Name as FAMILY_ID,M.MODEL as MODEL_ID,PN.CUSTOMER_PN,
                                P.MEANING as BU_CODE,SP.MEANING as CLASSIFICATION,BP.MEANING as PRODUCT_KIND, CP.MEANING as STAGE_CODE,
                                PN.DOUBLE_SIDE,PN.LEAD_FLAG,PN.SHIP_FLAG,PN.EDI_FLAG,PN.WARRANTY_LIMIT,PN.PHASE_IN_DATE,
                                PN.PHASE_OUT_DATE,PN.TURNIN_TYPE,PN.DESCRIPTION,DP.MEANING as CATEGORY 
                            FROM SFCS_PN PN 
                            LEFT JOIN SFCS_CUSTOMERS C ON PN.CUSTOMER_ID=C.ID
                            LEFT JOIN SFCS_PRODUCT_FAMILY PF ON PF.ID=PN.FAMILY_ID 
                            LEFT JOIN SFCS_MODEL M ON M.ID=PN.MODEL_ID
                            LEFT JOIN SFCS_PARAMETERS P ON (P.LOOKUP_CODE=PN.BU_CODE AND P.LOOKUP_TYPE = 'BU_CODE')
                            LEFT JOIN SFCS_PARAMETERS SP ON (SP.LOOKUP_CODE=PN.CLASSIFICATION AND SP.LOOKUP_TYPE = 'PLANT_CODE')
                            LEFT JOIN SFCS_PARAMETERS BP ON (BP.LOOKUP_CODE=PN.PRODUCT_KIND AND BP.LOOKUP_TYPE = 'PRODUCT_KIND')
                            LEFT JOIN SFCS_PARAMETERS CP ON (CP.LOOKUP_CODE=PN.STAGE_CODE AND CP.LOOKUP_TYPE = 'PRODUCT_STAGE')
                            LEFT JOIN SFCS_PARAMETERS DP ON (DP.LOOKUP_CODE=PN.CATEGORY AND DP.LOOKUP_TYPE = 'WO_CLASSIFICATION' AND DP.LOOKUP_CODE NOT IN (4) ) ";
            string pagedSql = SQLBuilderClass.GetPagedSQL(sql, "PN.ID DESC", condition);
            var resdata = await _dbConnection.QueryAsync<object>(pagedSql, model);
            string sqlcnt = @" SELECT COUNT(PN.ID) FROM SFCS_PN PN
                            LEFT JOIN SFCS_CUSTOMERS C ON PN.CUSTOMER_ID=C.ID
                            LEFT JOIN SFCS_PRODUCT_FAMILY PF ON PF.ID=PN.FAMILY_ID 
                            LEFT JOIN SFCS_MODEL M ON M.ID=PN.MODEL_ID
                            LEFT JOIN SFCS_PARAMETERS P ON (P.LOOKUP_CODE=PN.BU_CODE AND P.LOOKUP_TYPE = 'BU_CODE')
                            LEFT JOIN SFCS_PARAMETERS SP ON (SP.LOOKUP_CODE=PN.CLASSIFICATION AND SP.LOOKUP_TYPE = 'PLANT_CODE')
                            LEFT JOIN SFCS_PARAMETERS BP ON (BP.LOOKUP_CODE=PN.PRODUCT_KIND AND BP.LOOKUP_TYPE = 'PRODUCT_KIND')
                            LEFT JOIN SFCS_PARAMETERS CP ON (CP.LOOKUP_CODE=PN.STAGE_CODE AND CP.LOOKUP_TYPE = 'PRODUCT_STAGE')
                            LEFT JOIN SFCS_PARAMETERS DP ON (DP.LOOKUP_CODE=PN.CATEGORY AND DP.LOOKUP_TYPE = 'WO_CLASSIFICATION' AND DP.LOOKUP_CODE NOT IN (4) )  " + condition;

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
        public async Task<decimal> SaveDataByTrans(SfcsPnModel model)
        {
            int result = 1;
            ConnectionFactory.OpenConnection(_dbConnection);
            using (var tran = _dbConnection.BeginTransaction())
            {
                try
                {
                    //新增
                    string insertSql = @"insert into SFCS_PN 
					(ID,VERSION,ENABLE_BILL_ID,DISABLE_BILL_ID,PART_NO,CUSTOMER_ID,FAMILY_ID,MODEL_ID,CUSTOMER_PN,BU_CODE,CLASSIFICATION,PRODUCT_KIND,STAGE_CODE,DOUBLE_SIDE,LEAD_FLAG,SHIP_FLAG,EDI_FLAG,WARRANTY_LIMIT,PHASE_IN_DATE,PHASE_OUT_DATE,TURNIN_TYPE,DESCRIPTION,CATEGORY,ATTRIBUTE2,ATTRIBUTE3,ATTRIBUTE4,ATTRIBUTE5) 
					VALUES (:ID,:VERSION,:ENABLE_BILL_ID,:DISABLE_BILL_ID,:PART_NO,:CUSTOMER_ID,:FAMILY_ID,:MODEL_ID,:CUSTOMER_PN,:BU_CODE,:CLASSIFICATION,:PRODUCT_KIND,:STAGE_CODE,:DOUBLE_SIDE,:LEAD_FLAG,:SHIP_FLAG,:EDI_FLAG,:WARRANTY_LIMIT,:PHASE_IN_DATE,:PHASE_OUT_DATE,:TURNIN_TYPE,:DESCRIPTION,:CATEGORY,:ATTRIBUTE2,:ATTRIBUTE3,:ATTRIBUTE4,:ATTRIBUTE5)";
                    if (model.insertRecords != null && model.insertRecords.Count > 0)
                    {
                        foreach (var item in model.insertRecords)
                        {
                            int part_no_qty = QueryEx<int>("SELECT COUNT(1) FROM SFCS_PN WHERE PART_NO = :PART_NO ", new { PART_NO = item.PART_NO }).FirstOrDefault();
                            if (part_no_qty > 0) { throw new Exception("PART_NO_REPEAT"); }

                            var newid = await Get_MES_SEQ_ID();
                            var resdata = await _dbConnection.ExecuteAsync(insertSql, new
                            {
                                ID = newid,
                                item.VERSION,
                                item.ENABLE_BILL_ID,
                                item.DISABLE_BILL_ID,
                                item.PART_NO,
                                item.CUSTOMER_ID,
                                item.FAMILY_ID,
                                item.MODEL_ID,
                                item.CUSTOMER_PN,
                                item.BU_CODE,
                                item.CLASSIFICATION,
                                item.PRODUCT_KIND,
                                item.STAGE_CODE,
                                item.DOUBLE_SIDE,
                                item.LEAD_FLAG,
                                item.SHIP_FLAG,
                                item.EDI_FLAG,
                                item.WARRANTY_LIMIT,
                                item.PHASE_IN_DATE,
                                item.PHASE_OUT_DATE,
                                item.TURNIN_TYPE,
                                item.DESCRIPTION,
                                item.CATEGORY,
                                item.ATTRIBUTE2,
                                item.ATTRIBUTE3,
                                item.ATTRIBUTE4,
                                item.ATTRIBUTE5,

                            }, tran);
                        }
                    }
                    //更新
                    string updateSql = @"Update SFCS_PN set VERSION=:VERSION,ENABLE_BILL_ID=:ENABLE_BILL_ID,DISABLE_BILL_ID=:DISABLE_BILL_ID,PART_NO=:PART_NO,CUSTOMER_ID=:CUSTOMER_ID,FAMILY_ID=:FAMILY_ID,MODEL_ID=:MODEL_ID,CUSTOMER_PN=:CUSTOMER_PN,BU_CODE=:BU_CODE,CLASSIFICATION=:CLASSIFICATION,PRODUCT_KIND=:PRODUCT_KIND,STAGE_CODE=:STAGE_CODE,DOUBLE_SIDE=:DOUBLE_SIDE,LEAD_FLAG=:LEAD_FLAG,SHIP_FLAG=:SHIP_FLAG,EDI_FLAG=:EDI_FLAG,WARRANTY_LIMIT=:WARRANTY_LIMIT,PHASE_IN_DATE=:PHASE_IN_DATE,PHASE_OUT_DATE=:PHASE_OUT_DATE,TURNIN_TYPE=:TURNIN_TYPE,DESCRIPTION=:DESCRIPTION,CATEGORY=:CATEGORY,ATTRIBUTE2=:ATTRIBUTE2,ATTRIBUTE3=:ATTRIBUTE3,ATTRIBUTE4=:ATTRIBUTE4,ATTRIBUTE5=:ATTRIBUTE5  
						where ID=:ID ";
                    if (model.updateRecords != null && model.updateRecords.Count > 0)
                    {
                        foreach (var item in model.updateRecords)
                        {
                            int part_no_qty = QueryEx<int>("SELECT COUNT(1) FROM SFCS_PN WHERE PART_NO = :PART_NO AND ID != :ID ", new { FTYPE = item.PART_NO }).FirstOrDefault();
                            if (part_no_qty > 0) { throw new Exception("PART_NO_REPEAT"); }

                            var resdata = await _dbConnection.ExecuteAsync(updateSql, new
                            {
                                item.ID,
                                item.VERSION,
                                item.ENABLE_BILL_ID,
                                item.DISABLE_BILL_ID,
                                item.PART_NO,
                                item.CUSTOMER_ID,
                                item.FAMILY_ID,
                                item.MODEL_ID,
                                item.CUSTOMER_PN,
                                item.BU_CODE,
                                item.CLASSIFICATION,
                                item.PRODUCT_KIND,
                                item.STAGE_CODE,
                                item.DOUBLE_SIDE,
                                item.LEAD_FLAG,
                                item.SHIP_FLAG,
                                item.EDI_FLAG,
                                item.WARRANTY_LIMIT,
                                item.PHASE_IN_DATE,
                                item.PHASE_OUT_DATE,
                                item.TURNIN_TYPE,
                                item.DESCRIPTION,
                                item.CATEGORY,
                                item.ATTRIBUTE2,
                                item.ATTRIBUTE3,
                                item.ATTRIBUTE4,
                                item.ATTRIBUTE5,

                            }, tran);
                        }
                    }
                    ////删除
                    //string deleteSql = @"Delete from SFCS_PN where ID=:ID ";
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