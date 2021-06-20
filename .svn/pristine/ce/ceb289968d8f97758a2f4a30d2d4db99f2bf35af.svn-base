/*
*┌──────────────────────────────────────────────────────────────┐
*│　描    述：接口实现                                                    
*│　作    者：嘉志科技                                            
*│　版    本：2.0    模板代码自动生成                                                
*│　创建时间：2020-04-11 10:06:08                             
*└──────────────────────────────────────────────────────────────┘
*┌──────────────────────────────────────────────────────────────┐
*│　命名空间： JZ.IMS.Repository.Oracle                                  
*│　类    名： SfcsRuncardRangerRulesRepository                                      
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
    public class SfcsRuncardRangerRulesRepository : BaseRepository<SfcsRuncardRangerRules, Decimal>, ISfcsRuncardRangerRulesRepository
    {
        public SfcsRuncardRangerRulesRepository(IOptionsSnapshot<DbOption> options)
        {
            _dbOption = options.Get("iWMS");
            if (_dbOption == null)
            {
                throw new ArgumentNullException(nameof(DbOption));
            }
            _dbConnection = ConnectionFactory.CreateConnection(_dbOption.DbType, _dbOption.ConnectionString);
        }

        /// <summary>
		///项目是否已被使用 
		/// </summary>
		/// <param name="id">项目id</param>
		/// <returns></returns>
		public async Task<bool> ItemIsByUsed(decimal id)
        {
            string sql = "select count(0) from SFCS_RUNCARD_RANGER_RULES where id = :id";
            object result = await _dbConnection.ExecuteScalarAsync(sql, new
            {
                id
            });

            return (Convert.ToInt32(result) > 0);
        }

        /// <summary>
        /// 保存数据
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public async Task<decimal> SaveDataByTrans(SfcsRuncardRangerRulesAddOrModifyModel model)
        {
            int result = 1;
            ConnectionFactory.OpenConnection(_dbConnection);
            using (var tran = _dbConnection.BeginTransaction())
            {
                try
                {
                    //新增
                    if (model.ID == 0)
                    {
                        string insertSql = @"insert into SFCS_RUNCARD_RANGER_RULES 
					    (ID,CUSTOMER_ID,PRODUCT_FAMILY_ID,PLATFORM_ID,MODEL_ID,PART_NO,WO_NO,FIX_HEADER,FIX_TAIL,RANGE_LENGTH,RANGE_START_CODE,DIGITAL,EXCLUSIVE_CHAR,ENABLED,RULE_TYPE,SORT_TYPE) 
					    VALUES (:ID,:CUSTOMER_ID,:PRODUCT_FAMILY_ID,:PLATFORM_ID,:MODEL_ID,:PART_NO,:WO_NO,:FIX_HEADER,:FIX_TAIL,:RANGE_LENGTH,:RANGE_START_CODE,:DIGITAL,:EXCLUSIVE_CHAR,:ENABLED,:RULE_TYPE,:SORT_TYPE)";
                        var newid = await Get_MES_SEQ_ID();
                        var resdata = await _dbConnection.ExecuteAsync(insertSql, new
                        {
                            ID = newid,
                            model.CUSTOMER_ID,
                            model.PRODUCT_FAMILY_ID,
                            model.PLATFORM_ID,
                            model.MODEL_ID,
                            model.PART_NO,
                            model.WO_NO,
                            model.FIX_HEADER,
                            model.FIX_TAIL,
                            model.RANGE_LENGTH,
                            model.RANGE_START_CODE,
                            model.DIGITAL,
                            model.EXCLUSIVE_CHAR,
                            model.ENABLED,
                            model.RULE_TYPE,
                            model.SORT_TYPE

                        }, tran);
                    }
                    else
                    {
                        //更新
                        string updateSql = @"Update SFCS_RUNCARD_RANGER_RULES set CUSTOMER_ID=:CUSTOMER_ID,PRODUCT_FAMILY_ID=:PRODUCT_FAMILY_ID,PLATFORM_ID=:PLATFORM_ID,
                            MODEL_ID=:MODEL_ID,PART_NO=:PART_NO,WO_NO=:WO_NO,FIX_HEADER=:FIX_HEADER,FIX_TAIL=:FIX_TAIL,RANGE_LENGTH=:RANGE_LENGTH,RANGE_START_CODE=:RANGE_START_CODE,
                            DIGITAL=:DIGITAL,EXCLUSIVE_CHAR=:EXCLUSIVE_CHAR,ENABLED=:ENABLED,RULE_TYPE=:RULE_TYPE,SORT_TYPE=:SORT_TYPE  
						    where ID=:ID ";
                        var resdata = await _dbConnection.ExecuteAsync(updateSql, new
                        {
                            model.ID,
                            model.CUSTOMER_ID,
                            model.PRODUCT_FAMILY_ID,
                            model.PLATFORM_ID,
                            model.MODEL_ID,
                            model.PART_NO,
                            model.WO_NO,
                            model.FIX_HEADER,
                            model.FIX_TAIL,
                            model.RANGE_LENGTH,
                            model.RANGE_START_CODE,
                            model.DIGITAL,
                            model.EXCLUSIVE_CHAR,
                            model.ENABLED,
                            model.RULE_TYPE,
                            model.SORT_TYPE
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

        /// <summary>
		/// 獲取Ranger生成規則
		/// </summary>
		public async Task<SfcsRuncardRangerRules> GetRuncardRangerRulesByPN(string partNumber, string enable, int rule_type)
        {
            string sql = @"SELECT * FROM SFCS_RUNCARD_RANGER_RULES WHERE PART_NO = :PART_NO AND ENABLED = :ENABLED AND RULE_TYPE = :RULE_TYPE";
            var resdata = await _dbConnection.QueryAsync<SfcsRuncardRangerRules>(sql, 
                new { PART_NO = partNumber, ENABLED = enable, RULE_TYPE = rule_type });

            //string sqlcnt = @"select count(0) FROM SFCS_RUNCARD_RANGER_RULES WHERE PART_NO = :PART_NO AND ENABLED = :ENABLED";
            //int cnt = await _dbConnection.ExecuteScalarAsync<int>(sqlcnt,
            //    new { PART_NO = partNumber, ENABLED = enable });

            //return new TableDataModel
            //{
            //    count = cnt,
            //    data = resdata?.ToList(),
            //};

            return resdata?.FirstOrDefault();
        }

        /// <summary>
        /// 獲取Ranger生成規則
        /// </summary>
        public async Task<SfcsRuncardRangerRules> GetRuncardRangerRulesByPlatformId(decimal platformId, string enable, int rule_type)
        {
            string sql = @"SELECT * FROM SFCS_RUNCARD_RANGER_RULES WHERE PLATFORM_ID = :PLATFORM_ID AND ENABLED = :ENABLED AND RULE_TYPE = :RULE_TYPE";
            var resdata = await _dbConnection.QueryAsync<SfcsRuncardRangerRules>(sql,
                new { PLATFORM_ID = platformId, ENABLED = enable, RULE_TYPE = rule_type });

            //string sqlcnt = @"select count(0) FROM SFCS_RUNCARD_RANGER_RULES WHERE PLATFORM_ID = :PLATFORM_ID AND ENABLED = :ENABLED";
            //int cnt = await _dbConnection.ExecuteScalarAsync<int>(sqlcnt,
            //    new { PLATFORM_ID = platformId, ENABLED = enable });

            //return new TableDataModel
            //{
            //    count = cnt,
            //    data = resdata?.ToList(),
            //};

            return resdata?.FirstOrDefault();
        }

        /// <summary>
        /// 獲取Ranger生成規則
        /// </summary>
        public async Task<SfcsRuncardRangerRules> GetRuncardRangerRulesByFamilyId(decimal family_id, string enable, int rule_type)
        {
            string sql = @"SELECT * FROM SFCS_RUNCARD_RANGER_RULES WHERE PRODUCT_FAMILY_ID = :PRODUCT_FAMILY_ID AND ENABLED = :ENABLED AND RULE_TYPE = :RULE_TYPE";
            var resdata = await _dbConnection.QueryAsync<SfcsRuncardRangerRules>(sql,
                new { PRODUCT_FAMILY_ID = family_id, ENABLED = enable, RULE_TYPE = rule_type });

            return resdata?.FirstOrDefault();
        }

        /// <summary>
		/// 獲取Ranger生成規則
		/// </summary>
		public async Task<SfcsRuncardRangerRules> GetRuncardRangerRulesByCustomerId(decimal customerId, string enable, int rule_type)
        {
            string sql = @"SELECT * FROM SFCS_RUNCARD_RANGER_RULES WHERE CUSTOMER_ID = :CUSTOMER_ID AND ENABLED = :ENABLED AND RULE_TYPE = :RULE_TYPE";
            var resdata = await _dbConnection.QueryAsync<SfcsRuncardRangerRules>(sql,
                new { CUSTOMER_ID = customerId, ENABLED = enable, RULE_TYPE = rule_type });

            //string sqlcnt = @"select count(0) FROM SFCS_RUNCARD_RANGER_RULES WHERE CUSTOMER_ID = :CUSTOMER_ID AND ENABLED = :ENABLED";
            //int cnt = await _dbConnection.ExecuteScalarAsync<int>(sqlcnt,
            //    new { CUSTOMER_ID = customerId, ENABLED = enable });

            //return new TableDataModel
            //{
            //    count = cnt,
            //    data = resdata?.ToList(),
            //};

            return resdata?.FirstOrDefault();
        }

        public async Task<IEnumerable<SfcsRuncardRangerRulesListModel>> LoadData(SfcsRuncardRangerRulesRequestModel model)
        {
            int page = 0, limit = 0;
            page = model.Page * model.Limit - model.Limit + 1;
            limit = model.Page * model.Limit;
            model.Page = page;
            model.Limit = limit;

            string sWhere = GetWhereStr(model);

            string sQuery = string.Format("SELECT * FROM (SELECT ROWNUM R, T.* FROM ( SELECT RR.*,SPF.FAMILY_NAME FROM SFCS_RUNCARD_RANGER_RULES RR LEFT JOIN SFCS_PRODUCT_FAMILY SPF ON RR.PRODUCT_FAMILY_ID=SPF.ID {0} ORDER BY RR.ID DESC) T) WHERE R BETWEEN :Page AND :Limit", sWhere);

            return await _dbConnection.QueryAsync<SfcsRuncardRangerRulesListModel>(sQuery, model);
        }

        public async Task<int> LoadDataCount(SfcsRuncardRangerRulesRequestModel model)
        {
            string sWhere = GetWhereStr(model);
            string sQuery = string.Format("SELECT COUNT(1) FROM SFCS_RUNCARD_RANGER_RULES RR {0} ", sWhere);
            return await _dbConnection.ExecuteScalarAsync<int>(sQuery, model);
        }

        /// <summary>
        /// 获取WHERE条件
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        private string GetWhereStr(SfcsRuncardRangerRulesRequestModel model)
        {
            string sWhere = " WHERE RR.ID > 0 ";
            if (model.CUSTOMER_ID != null && model.CUSTOMER_ID > 0)
            {
                sWhere += $" AND (RR.CUSTOMER_ID =:CUSTOMER_ID) ";
            }
            if (model.PART_NO != null && model.PART_NO != "")
            {
                sWhere += $" AND (instr(RR.PART_NO, :PART_NO) > 0) ";
            }
            if (model.PRODUCT_FAMILY_ID != null && model.PRODUCT_FAMILY_ID >= 0)
            {
                sWhere += $" AND RR.PRODUCT_FAMILY_ID = :PRODUCT_FAMILY_ID ";
            }
            if (model.RULE_TYPE > -1 && model.RULE_TYPE < 4)
            {
                sWhere += $" AND RR.RULE_TYPE = :RULE_TYPE ";
            }
            return sWhere;
        }

    }
}