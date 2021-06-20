/**
*┌──────────────────────────────────────────────────────────────┐
*│　描    述：接口实现                                                    
*│　作    者：嘉志科技                                            
*│　版    本：2.0    模板代码自动生成                                                
*│　创建时间：2020-03-18 11:07:21                             
*└──────────────────────────────────────────────────────────────┘
*┌──────────────────────────────────────────────────────────────┐
*│　命名空间： JZ.IMS.Repository.Oracle                                  
*│　类    名： SmtFeederRepairRepository                                      
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
using System.Text;

namespace JZ.IMS.Repository.Oracle
{
    public class SmtFeederRepairRepository : BaseRepository<SmtFeederRepair, Decimal>, ISmtFeederRepairRepository
    {
        public SmtFeederRepairRepository(IOptionsSnapshot<DbOption> options)
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
            string sql = "SELECT SMT_FEEDER_REPAIR_SEQ.NEXTVAL MY_SEQ FROM DUAL";
            var result = await _dbConnection.ExecuteScalarAsync(sql);
            return (decimal)result;
        }

        /// <summary>
        /// 获取报修飞达列表
        /// </summary>
        /// <param name="model">model</param>
        /// <returns></returns>
        public async Task<TableDataModel> GetFeeder2RepairList(SmtFeederRepairRequestModel model)
        {
            string condition = " ";
            if (!model.Key.IsNullOrWhiteSpace())
            {
                condition += $" and (instr(SF.FEEDER, :Key) > 0) ";
            }

            string sql = @"SELECT ROWNUM AS rowno, SF.ID, SF.FEEDER as NAME FROM SMT_FEEDER_REPAIR SFR, SMT_FEEDER SF 
                           WHERE SF.ID = SFR.FEEDER_ID AND(SFR.RESULT = 1 OR SFR.RESULT IS NULL) ";
            string pagedSql = SQLBuilderClass.GetPagedSQL(sql, "SF.FEEDER desc", condition);
            var resdata = await _dbConnection.QueryAsync<IDNAME>(pagedSql, model);

            string sqlcnt = @"select count(0) FROM SMT_FEEDER_REPAIR SFR, SMT_FEEDER SF 
                           WHERE SF.ID = SFR.FEEDER_ID AND(SFR.RESULT = 1 OR SFR.RESULT IS NULL) " + condition;

            int cnt = await _dbConnection.ExecuteScalarAsync<int>(sqlcnt, model);

            return new TableDataModel
            {
                count = cnt,
                data = resdata?.ToList(),
            };
        }

        /// <summary>
        /// 获取飞达注册信息
        /// </summary>
        /// <param name="feeder">飞达编号</param>
        /// <returns></returns>
        public async Task<SmtFeeder> GetFeederInfo(string feeder)
        {
            string sql = @"SELECT * FROM SMT_FEEDER WHERE FEEDER = :FEEDER ";
            var resdata = await _dbConnection.QueryAsync<SmtFeeder>(sql, new { FEEDER = feeder });
            return resdata?.FirstOrDefault();
        }

        /// <summary>
        /// 获取根本原因列表
        /// </summary>
        /// <param name="model">model</param>
        /// <returns></returns>
        public async Task<TableDataModel> GetReasonList(SmtFeederRepairRequestModel model)
        {
            string condition = " ";
            if (!model.Key.IsNullOrWhiteSpace())
            {
                condition += $" and (instr(DESCRIPTION, :Key) > 0) ";
            }

            string sql = @"SELECT ROWNUM AS rowno, CODE, DESCRIPTION as NAME FROM SMT_FEEDER_DEFECT_REASON WHERE ENABLED = 'Y' ";
            string pagedSql = SQLBuilderClass.GetPagedSQL(sql, "CODE", condition);
            var resdata = await _dbConnection.QueryAsync<CodeName>(pagedSql, model);

            string sqlcnt = @"select count(0) FROM SMT_FEEDER_DEFECT_REASON WHERE ENABLED = 'Y' " + condition;

            int cnt = await _dbConnection.ExecuteScalarAsync<int>(sqlcnt, model);

            return new TableDataModel
            {
                count = cnt,
                data = resdata?.ToList(),
            };
        }

        /// <summary>
        /// 获取损坏部件列表
        /// </summary>
        /// <param name="model">model</param>
        /// <returns></returns>
        public async Task<TableDataModel> GetDamagePartList(SmtFeederRepairRequestModel model)
        {
            string condition = " ";
            if (!model.Key.IsNullOrWhiteSpace())
            {
                condition += $" and (instr(DESCRIPTION, :Key) > 0 OR instr(CODE, :Key) > 0)  ";
            }

            string sql = @"SELECT ROWNUM AS rowno, CODE,DESCRIPTION as NAME FROM SMT_FEEDER_DAMAGE_PART WHERE  ENABLED='Y' ";
            string pagedSql = SQLBuilderClass.GetPagedSQL(sql, "CODE", condition);
            var resdata = await _dbConnection.QueryAsync<CodeName>(pagedSql, model);

            string sqlcnt = @"select count(0) FROM SMT_FEEDER_DAMAGE_PART WHERE ENABLED = 'Y' " + condition;

            int cnt = await _dbConnection.ExecuteScalarAsync<int>(sqlcnt, model);

            return new TableDataModel
            {
                count = cnt,
                data = resdata?.ToList(),
            };
        }

        /// <summary>
        /// 获取检查項目列表
        /// </summary>
        /// <param name="model">model</param>
        /// <returns></returns>
        public async Task<TableDataModel> GetRepairItemsList(SmtFeederRepairRequestModel model)
        {
            string condition = " ";
            if (!model.Key.IsNullOrWhiteSpace())
            {
                condition += $" and (instr(DESCRIPTION, :Key) > 0) ";
            }

            string sql = @"SELECT ROWNUM AS rowno, CODE,DESCRIPTION as NAME FROM SMT_FEEDER_REPAIR_ITEMS WHERE  ENABLED='Y' ";
            string pagedSql = SQLBuilderClass.GetPagedSQL(sql, "CODE", condition);
            var resdata = await _dbConnection.QueryAsync<CodeName>(pagedSql, model);

            string sqlcnt = @"select count(0) FROM SMT_FEEDER_REPAIR_ITEMS WHERE ENABLED = 'Y' " + condition;

            int cnt = await _dbConnection.ExecuteScalarAsync<int>(sqlcnt, model);

            return new TableDataModel
            {
                count = cnt,
                data = resdata?.ToList(),
            };
        }

        /// <summary>
        /// 获取维修结果列表
        /// </summary>
        /// <param name="model">model</param>
        /// <returns></returns>
        public async Task<List<IDNAME>> GetResultList()
        {
            string sql = "SELECT 2 AS ID, '已修好' as NAME from dual Union " +
                "SELECT 4 AS ID, '报废' as NAME from dual  ";
            var resdata = await _dbConnection.QueryAsync<IDNAME>(sql);

            return resdata?.ToList();
        }

        /// <summary>
        /// 获取不良记录列表
        /// </summary>
        /// <param name="feeder">飞达编号</param>
        /// <returns></returns>
        public async Task<List<CodeName>> GetDefectList(string feeder)
        {
            string sql = @"SELECT DISTINCT SFD.CODE, SFD.DESCRIPTION as NAME 
                           FROM SMT_FEEDER SF, SMT_FEEDER_REPAIR SFR, SMT_FEEDER_DEFECT SFD
                           WHERE SF.ID = SFR.FEEDER_ID
                               AND SFR.DEFECT_CODE = SFD.CODE
                               AND SFR.RESULT NOT IN('2', '4')
                               AND SFD.ENABLED = 'Y'
                               AND SF.FEEDER =:FEEDER";
            var resdata = await _dbConnection.QueryAsync<CodeName>(sql, new { FEEDER = feeder });
            return resdata?.ToList();
        }

        /// <summary>
        /// 获取本月维修次数
        /// </summary>
        /// <param name="feeder">飞达ID</param>
        /// <returns></returns>
        public async Task<decimal> GetRepairCountByMonth(decimal feeder_id)
        {
            string sql = @"SELECT COUNT (*)
                              FROM SMT_FEEDER_REPAIR
                             WHERE     FEEDER_ID = :FEEDER_ID
                                   AND TRUNC (REPAIR_TIME, 'MONTH') = TRUNC (SYSDATE, 'MONTH') ";
            return await _dbConnection.ExecuteScalarAsync<decimal>(sql, new { FEEDER_ID = feeder_id });
        }

        /// <summary>
        /// 获取总计维修次数
        /// </summary>
        /// <param name="feeder">飞达ID</param>
        /// <returns></returns>
        public async Task<decimal> GetRepairTotalCount(decimal feeder_id)
        {
            string sql = @"SELECT COUNT(*) FROM SMT_FEEDER_REPAIR WHERE FEEDER_ID=:FEEDER_ID AND REPAIR_TIME IS NOT NULL ";
            return await _dbConnection.ExecuteScalarAsync<decimal>(sql, new { FEEDER_ID = feeder_id });
        }

        /// <summary>
        /// 获取维修结果列表
        /// </summary>
        /// <param name="model">model</param>
        /// <returns></returns>
        public async Task<List<IDNAME>> GetRepairResultList()
        {
            string sql = "SELECT 1 AS ID, '待维修' as NAME from dual Union SELECT 2 AS ID, '已修好' as NAME from dual Union " +
                         "SELECT 3 AS ID, '未修好' as NAME from dual Union SELECT 4 AS ID, '报废' as NAME from dual  ";
            var resdata = await _dbConnection.QueryAsync<IDNAME>(sql);

            return resdata?.ToList();
        }

        /// <summary>
        /// 获取维修记录列表
        /// </summary>
        /// <param name="feeder_id">飞达ID</param>
        /// <returns></returns>
        public async Task<List<SmtFeederRepairListModel>> GetRepairList(decimal feeder_id)
        {
            string sql = @"SELECT SF.FEEDER, SFR.*, SL.LINE_NAME, SR.DESCRIPTION AS REASON_DESC, SD.DESCRIPTION AS DEFECT_DESC,
                                SP.DESCRIPTION AS DAMAGE_PART_DESC, SI.DESCRIPTION AS REPAIR_ITEM_DESC 
                           FROM SMT_FEEDER SF JOIN SMT_FEEDER_REPAIR SFR ON SF.ID = SFR.FEEDER_ID
	                               LEFT OUTER JOIN SMT_LINES SL ON SFR.FEEDER_LOCATION = SL.ID 
                                   LEFT OUTER JOIN SMT_FEEDER_DEFECT_REASON SR ON SFR.reason_code = SR.CODE  
                                   LEFT OUTER JOIN smt_feeder_defect SD ON SFR.defect_code = SD.CODE  
                                   LEFT OUTER JOIN SMT_FEEDER_DAMAGE_PART SP ON SFR.damage_part = SP.CODE 
                                   LEFT OUTER JOIN SMT_FEEDER_REPAIR_ITEMS SI ON SFR.repair_item = SI.CODE 
                            WHERE SFR.FEEDER_ID = :FEEDER_ID";
                            
            var resdata = await _dbConnection.QueryAsync<SmtFeederRepairListModel>(sql, new { FEEDER_ID = feeder_id });
            return resdata?.ToList();
        }

        /// <summary>
        /// 保存数据
        /// </summary>
        /// <param name="model"></param>
        /// <param name="feeder_id"></param>
        /// <returns></returns>
        public async Task<decimal> SaveDataByTrans(SmtFeederRepairAddOrModifyModel model, decimal feeder_id)
        {
            int result = 1;
            ConnectionFactory.OpenConnection(_dbConnection);
            using (var tran = _dbConnection.BeginTransaction())
            {
                try
                {
                    if (model.RESULT == (decimal)FeederRepairResultEnum.FEEDER_REPAIR_RESULT_SCRAP)
                    {
                        //更新维修表 
                        string updateSql = @"Update SMT_FEEDER_REPAIR set REASON_CODE=:REASON_CODE, DAMAGE_PART=:DAMAGE_PART, REPAIRER_BY=:REPAIRER_BY,
                                 REPAIR_ITEM=:REPAIR_ITEM, METHOD=:METHOD, REPAIR_TIME=SYSDATE, RESULT=:RESULT   
						where DEFECT_CODE =:DEFECT_CODE and RESULT != 2 and FEEDER_ID=:FEEDER_ID ";
                        var resdata = await _dbConnection.ExecuteAsync(updateSql, new
                        {
                            FEEDER_ID = feeder_id,
                            model.DEFECT_CODE,
                            model.REASON_CODE,
                            model.DAMAGE_PART,
                            REPAIRER_BY = model.UserName,
                            model.REPAIR_ITEM,
                            model.METHOD,
                            RESULT = (decimal)FeederRepairResultEnum.FEEDER_REPAIR_RESULT_SCRAP
                        }, tran);

                        //更新飞达
                        updateSql = @"Update SMT_FEEDER set UPDATE_TIME = SYSDATE, UPDATE_BY=:UPDATE_BY, STATUS=:STATUS, LAST_REPAIR_TIME = SYSDATE   
						              where ID=:ID ";
                        await _dbConnection.ExecuteAsync(updateSql, new
                        {
                            ID = feeder_id,
                            UPDATE_BY = model.UserName,
                            STATUS = (decimal)FeederStatusEnum.FEEDER_STATUS_SCRAPED
                        }, tran);
                    }
                    else
                    {
                        //更新维修表 
                        string updateSql = @"Update SMT_FEEDER_REPAIR set REASON_CODE=:REASON_CODE, DAMAGE_PART=:DAMAGE_PART, REPAIRER_BY=:REPAIRER_BY,
                                 REPAIR_ITEM=:REPAIR_ITEM, METHOD=:METHOD, REPAIR_TIME=SYSDATE, RESULT=:RESULT   
						where DEFECT_CODE =:DEFECT_CODE and RESULT != 2 and FEEDER_ID=:FEEDER_ID ";
                        var resdata = await _dbConnection.ExecuteAsync(updateSql, new
                        {
                            FEEDER_ID = feeder_id,
                            model.DEFECT_CODE,
                            model.REASON_CODE,
                            model.DAMAGE_PART,
                            REPAIRER_BY = model.UserName,
                            model.REPAIR_ITEM,
                            model.METHOD,
                            model.RESULT
                        }, tran);

                        //更新飞达
                        updateSql = @"Update SMT_FEEDER set UPDATE_TIME = SYSDATE, UPDATE_BY=:UPDATE_BY, STATUS=:STATUS, LAST_REPAIR_TIME = SYSDATE   
						              where ID=:ID ";
                        await _dbConnection.ExecuteAsync(updateSql, new
                        {
                            ID = feeder_id,
                            UPDATE_BY = model.UserName,
                            STATUS = (decimal)FeederStatusEnum.FEEDER_STATUS_USABLE
                        }, tran);

                        //更新大Feeder狀態
                        if (model.FEEDER.IndexOf("-") > 0)
                        {
                            //string subFeeder = string.Empty;
                            //subFeeder = model.FEEDER.Substring(0, model.FEEDER.IndexOf("-"));
                            //if (subFeeder.Length == 4)
                            //{
                            //    string bigFeeder = @"SELECT * FROM SMT_FEEDER WHERE FEEDER LIKE :SUBFEEDER AND FEEDER <>:SUBFEEDER AND STATUS=:STATUS";
                            //    var tmpdata = await _dbConnection.QueryAsync<SmtFeeder>(bigFeeder, new
                            //    {
                            //        SUBFEEDER = model.FEEDER,
                            //        STATUS = (decimal)FeederStatusEnum.FEEDER_STATUS_REJECT
                            //    });
                            //    if (tmpdata != null)
                            //    {
                            //        string selectBigFeederByStatus = @"SELECT * FROM SMT_FEEDER WHERE FEEDER LIKE :SUBFEEDER AND FEEDER <>:SUBFEEDER AND STATUS<>:STATUS";
                            //        var tmpres = await _dbConnection.QueryAsync<SmtFeeder>(selectBigFeederByStatus, new
                            //        {
                            //            SUBFEEDER = model.FEEDER,
                            //            STATUS = (decimal)FeederStatusEnum.FEEDER_STATUS_WAITEMEND
                            //        });
                            //        //this.feederDataSet.SMT_FEEDER.Merge(updateBigFeederTable);
                            //    }
                            //}
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
        /// 保存报废数据
        /// </summary>
        /// <param name="model"></param>
        /// <param name="feeder_id"></param>
        /// <returns></returns>
        public async Task<decimal> SaveFeederScrap(SmtFeederScrapModel model, decimal feeder_id)
        {
            int result = 1;
            ConnectionFactory.OpenConnection(_dbConnection);
            using (var tran = _dbConnection.BeginTransaction())
            {
                try
                {
                    //更新飞达
                    string updateSql = @"Update SMT_FEEDER set UPDATE_TIME = SYSDATE, UPDATE_BY=:UPDATE_BY, STATUS=:STATUS  
						              where ID=:ID ";
                    result = await _dbConnection.ExecuteAsync(updateSql, new
                    {
                        ID = feeder_id,
                        UPDATE_BY = model.UserName,
                        STATUS = (decimal)FeederStatusEnum.FEEDER_STATUS_SCRAPED
                    }, tran);
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