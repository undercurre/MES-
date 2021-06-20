/*
*┌──────────────────────────────────────────────────────────────┐
*│　描    述：接口实现                                                    
*│　作    者：嘉志科技                                            
*│　版    本：2.0    模板代码自动生成                                                
*│　创建时间：2020-09-09 11:49:39                             
*└──────────────────────────────────────────────────────────────┘
*┌──────────────────────────────────────────────────────────────┐
*│　命名空间： JZ.IMS.Repository.Oracle                                  
*│　类    名： SfcsCollectDefectsRepository                                      
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
using System.Data;
using JZ.IMS.Core.Utilities;
using Org.BouncyCastle.Asn1.Crmf;
using JZ.IMS.Core.Extensions;

namespace JZ.IMS.Repository.Oracle
{
    public class SfcsCollectDefectsRepository : BaseRepository<SfcsCollectDefects, Decimal>, ISfcsCollectDefectsRepository
    {
        public SfcsCollectDefectsRepository(IOptionsSnapshot<DbOption> options)
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
            string sql = "SELECT ENABLED FROM SFCS_COLLECT_DEFECTS WHERE ID=:ID";
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
            string sql = "UPDATE SFCS_COLLECT_DEFECTS set ENABLED=:ENABLED WHERE ID=:Id";
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
            string sql = "SELECT SFCS_COLLECT_DEFECTS_SEQ.NEXTVAL MY_SEQ FROM DUAL";
            var result = await _dbConnection.ExecuteScalarAsync(sql);
            return (decimal)result;
        }

        /// <summary>
        /// 获取表的序列
        /// </summary>
        /// <returns></returns>
        public async Task<decimal> GetRepairSEQID()
        {
            string sql = "SELECT SFCS_REPAIR_RECIPE_SEQ.NEXTVAL MY_SEQ FROM DUAL";
            var result = await _dbConnection.ExecuteScalarAsync(sql);
            return (decimal)result;
        }

        /// <summary>
        /// 获取表的序列
        /// </summary>
        /// <returns></returns>
        public async Task<decimal> GetCollectDefectSEQID()
        {
            string sql = "SELECT SFCS_OPERATION_SEQ.NEXTVAL MY_SEQ FROM DUAL";
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
            string sql = "select count(0) from SFCS_COLLECT_DEFECTS where id = :id";
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
        public async Task<decimal> SaveDataByTrans(SfcsCollectDefectsModel model)
        {
            int result = 1;
            ConnectionFactory.OpenConnection(_dbConnection);
            using (var tran = _dbConnection.BeginTransaction())
            {
                try
                {

                    #region

                    ////新增
                    //string insertSql = @"insert into SFCS_COLLECT_DEFECTS 
                    //(COLLECT_DEFECT_ID,COLLECT_DEFECT_DETAIL_ID,DEFECT_CODE,CUSTOMER_DEFECT_CODE,SN,WO_ID,SN_ID,DEFECT_OPERATION_ID,REPAIR_OPERATION_ID,DEFECT_SITE_ID,DEFECT_OPERATOR,REPAIR_SITE_ID,REPAIRER,VALIDATED,CHECK_OPERATOR,DEFECT_TIME,REPAIR_TIME,REPAIR_IN_TIME,REPAIR_IN_OPERATOR,REPAIR_OUT_TIME,REPAIR_OUT_OPERATOR,NDF_FLAG,REPAIR_FLAG,SPI_REPAIR_TYPE) 
                    //VALUES (:COLLECT_DEFECT_ID,:COLLECT_DEFECT_DETAIL_ID,:DEFECT_CODE,:CUSTOMER_DEFECT_CODE,:SN,:WO_ID,:SN_ID,:DEFECT_OPERATION_ID,:REPAIR_OPERATION_ID,:DEFECT_SITE_ID,:DEFECT_OPERATOR,:REPAIR_SITE_ID,:REPAIRER,:VALIDATED,:CHECK_OPERATOR,:DEFECT_TIME,:REPAIR_TIME,:REPAIR_IN_TIME,:REPAIR_IN_OPERATOR,:REPAIR_OUT_TIME,:REPAIR_OUT_OPERATOR,:NDF_FLAG,:REPAIR_FLAG,:SPI_REPAIR_TYPE)";
                    //if (model.InsertRecords != null && model.InsertRecords.Count > 0)
                    //{
                    //	foreach (var item in model.InsertRecords)
                    //	{
                    //		var newid = await GetSEQID();
                    //		var resdata = await _dbConnection.ExecuteAsync(insertSql, new
                    //		{
                    //			ID = newid,
                    //			item.COLLECT_DEFECT_ID,
                    //			item.COLLECT_DEFECT_DETAIL_ID,
                    //			item.DEFECT_CODE,
                    //			item.CUSTOMER_DEFECT_CODE,
                    //			item.SN,
                    //			item.WO_ID,
                    //			item.SN_ID,
                    //			item.DEFECT_OPERATION_ID,
                    //			item.REPAIR_OPERATION_ID,
                    //			item.DEFECT_SITE_ID,
                    //			item.DEFECT_OPERATOR,
                    //			item.REPAIR_SITE_ID,
                    //			item.REPAIRER,
                    //			item.VALIDATED,
                    //			item.CHECK_OPERATOR,
                    //			item.DEFECT_TIME,
                    //			item.REPAIR_TIME,
                    //			item.REPAIR_IN_TIME,
                    //			item.REPAIR_IN_OPERATOR,
                    //			item.REPAIR_OUT_TIME,
                    //			item.REPAIR_OUT_OPERATOR,
                    //			item.NDF_FLAG,
                    //			item.REPAIR_FLAG,
                    //			item.SPI_REPAIR_TYPE,

                    //		}, tran);
                    //	}
                    //}
                    ////更新
                    //string updateSql = @"Update SFCS_COLLECT_DEFECTS set COLLECT_DEFECT_ID=:COLLECT_DEFECT_ID,COLLECT_DEFECT_DETAIL_ID=:COLLECT_DEFECT_DETAIL_ID,DEFECT_CODE=:DEFECT_CODE,CUSTOMER_DEFECT_CODE=:CUSTOMER_DEFECT_CODE,SN=:SN,WO_ID=:WO_ID,SN_ID=:SN_ID,DEFECT_OPERATION_ID=:DEFECT_OPERATION_ID,REPAIR_OPERATION_ID=:REPAIR_OPERATION_ID,DEFECT_SITE_ID=:DEFECT_SITE_ID,DEFECT_OPERATOR=:DEFECT_OPERATOR,REPAIR_SITE_ID=:REPAIR_SITE_ID,REPAIRER=:REPAIRER,VALIDATED=:VALIDATED,CHECK_OPERATOR=:CHECK_OPERATOR,DEFECT_TIME=:DEFECT_TIME,REPAIR_TIME=:REPAIR_TIME,REPAIR_IN_TIME=:REPAIR_IN_TIME,REPAIR_IN_OPERATOR=:REPAIR_IN_OPERATOR,REPAIR_OUT_TIME=:REPAIR_OUT_TIME,REPAIR_OUT_OPERATOR=:REPAIR_OUT_OPERATOR,NDF_FLAG=:NDF_FLAG,REPAIR_FLAG=:REPAIR_FLAG,SPI_REPAIR_TYPE=:SPI_REPAIR_TYPE  
                    //	where ID=:ID ";
                    //if (model.UpdateRecords != null && model.UpdateRecords.Count > 0)
                    //{
                    //	foreach (var item in model.UpdateRecords)
                    //	{
                    //		var resdata = await _dbConnection.ExecuteAsync(updateSql, new
                    //		{
                    //			item.ID,
                    //			item.COLLECT_DEFECT_ID,
                    //			item.COLLECT_DEFECT_DETAIL_ID,
                    //			item.DEFECT_CODE,
                    //			item.CUSTOMER_DEFECT_CODE,
                    //			item.SN,
                    //			item.WO_ID,
                    //			item.SN_ID,
                    //			item.DEFECT_OPERATION_ID,
                    //			item.REPAIR_OPERATION_ID,
                    //			item.DEFECT_SITE_ID,
                    //			item.DEFECT_OPERATOR,
                    //			item.REPAIR_SITE_ID,
                    //			item.REPAIRER,
                    //			item.VALIDATED,
                    //			item.CHECK_OPERATOR,
                    //			item.DEFECT_TIME,
                    //			item.REPAIR_TIME,
                    //			item.REPAIR_IN_TIME,
                    //			item.REPAIR_IN_OPERATOR,
                    //			item.REPAIR_OUT_TIME,
                    //			item.REPAIR_OUT_OPERATOR,
                    //			item.NDF_FLAG,
                    //			item.REPAIR_FLAG,
                    //			item.SPI_REPAIR_TYPE,

                    //		}, tran);
                    //	}
                    //}
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

        /// <summary>
        /// 获取SN下拉框
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public async Task<TableDataModel> GetSnDataList(DropDownBoxRequestModel model)
        {
            try
            {
                string conditions = " WHERE 1=1  ";
                if (!model.Key.IsNullOrEmpty())
                {
                    conditions += $"and (instr(a.SN, :Key) > 0  )";
                }
                string sql = @"select rownum as rowno,a.SN from (                                                  
                                            SELECT   DISTINCT (SR.SN)
                                            FROM SFCS_COLLECT_DEFECTS SCD, SFCS_RUNCARD SR
                                            WHERE SR.ID = SCD.SN_ID
                                            AND SCD.REPAIR_TIME IS NULL
                                            AND SR.STATUS = 2 ) a ";

                string pagedSql = SQLBuilderClass.GetPagedSQL(sql, " a.SN desc ", conditions);
                var resdata = await _dbConnection.QueryAsync<object>(pagedSql, model);
                string sqlcnt = @" select count(0) from (                                                  
                                            SELECT   DISTINCT (SR.SN)
                                            FROM SFCS_COLLECT_DEFECTS SCD, SFCS_RUNCARD SR
                                            WHERE SR.ID = SCD.SN_ID
                                            AND SCD.REPAIR_TIME IS NULL
                                            AND SR.STATUS = 2 ) a " + conditions;
                int cnt = await _dbConnection.ExecuteScalarAsync<int>(sqlcnt, model);

                return new TableDataModel
                {
                    count = cnt,
                    data = resdata?.ToList(),
                };
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        /// <summary>
        /// 获取线体下拉框
        /// </summary>
        /// <returns></returns>

        public async Task<TableDataModel> GetLINENAMEList(DropDownBoxRequestModel model)
        {
            try
            {
                string conditions = " where  1=1  ";
                string strwhere = " and 1=1 ";
                if (!model.Key.IsNullOrEmpty())
                {
                    conditions += $"and (instr(u.LINE_NAME, :Key) > 0 )";
                }
                if (model.USERID > 0)
                {
                    strwhere += $" and man.ID=:USERID";
                }
                string sql = string.Format(@"select * from (
                                                select ROWNUM as ROWNO, u.* from (
                                                    select  temp.* from (
                                                     select DISTINCT * from ( select ID,LINE_NAME,ORGANIZE_ID from  SMT_LINES
                                                      union all    
                                                      select ID,OPERATION_LINE_NAME as LINE_NAME,ORGANIZE_ID from SFCS_OPERATION_LINES where ENABLED='Y')
                                                    )temp 
                                                    inner join (select distinct useror.ORGANIZE_ID from SYS_MANAGER man 
                                                        left join sys_user_organize useror on man.ID=useror.MANAGER_ID 
                                                        where man.ENABLED='Y' and  useror.STATUS='Y'{0}) organ on temp.ORGANIZE_ID=organ.ORGANIZE_ID
                                                        ) u {1}  order by u.ID desc                                                ) uu where  rowno BETWEEN ((:Page-1)*:Limit+1) AND (:Limit*:Page) ", strwhere, conditions);

                // string pagedSql = SQLBuilderClass.GetPagedSQL(sql, " temp.ID desc", conditions);
                var resdata = await _dbConnection.QueryAsync<object>(sql, model);
                string sqlcnt = string.Format(@"select count(0) from (
                                                select ROWNUM as ROWNO, u.* from (
                                                    select  temp.* from (
                                                     select DISTINCT * from (  select ID,LINE_NAME,ORGANIZE_ID from  SMT_LINES
                                                      union all    
                                                      select ID,OPERATION_LINE_NAME as LINE_NAME,ORGANIZE_ID from SFCS_OPERATION_LINES where ENABLED='Y')
                                                    )temp 
                                                    inner join (select distinct useror.ORGANIZE_ID from SYS_MANAGER man 
                                                        left join sys_user_organize useror on man.ID=useror.MANAGER_ID 
                                                        where man.ENABLED='Y' and  useror.STATUS='Y'{0}) organ on temp.ORGANIZE_ID=organ.ORGANIZE_ID
                                                        ) u {1}  order by u.ID desc                                                ) uu ", strwhere, conditions);
                int cnt = await _dbConnection.ExecuteScalarAsync<int>(sqlcnt, model);

                return new TableDataModel
                {
                    count = cnt,
                    data = resdata?.ToList(),
                };
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }




        /// <summary>
        /// 获取站点下拉框数据
        /// </summary>
        /// <returns></returns>
        public async Task<TableDataModel> GetSITENAMEList(SiteNameRequestModel model)
        {
            try
            {
                string conditions = " WHERE  a.ENABLED='Y'  ";
                if (!model.Key.IsNullOrEmpty())
                {
                    conditions += $"and (instr(a.OPERATION_SITE_NAME, :Key) > 0 )";
                }
                if (model.Line_ID > 0)
                {
                    conditions += $" and a.OPERATION_LINE_ID=:Line_ID";
                }
                string sql = @"select ROWNUM as rowno, a.ID ,a.OPERATION_SITE_NAME from Sfcs_Operation_Sites a ";

                string pagedSql = SQLBuilderClass.GetPagedSQL(sql, " a.ID desc ", conditions);
                var resdata = await _dbConnection.QueryAsync<object>(pagedSql, model);
                string sqlcnt = @" select count(0) from Sfcs_Operation_Sites a " + conditions;
                int cnt = await _dbConnection.ExecuteScalarAsync<int>(sqlcnt, model);

                return new TableDataModel
                {
                    count = cnt,
                    data = resdata?.ToList(),
                };
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// 获取工序下拉框数据
        /// </summary>
        /// <returns></returns>
        public async Task<TableDataModel> GetOPERNAMEList(DropDownBoxRequestModel model)
        {
            try
            {
                string conditions = " WHERE a.ENABLED='Y'  ";
                if (!model.Key.IsNullOrEmpty())
                {
                    conditions += $"and (instr(a.OPERATION_NAME, :Key) > 0  or instr(a.DESCRIPTION, :Key) > 0)";
                }
                string sql = @"select  ROWNUM as rowno, a.ID,a.DESCRIPTION as OPERATION_NAME   from Sfcs_Operations a 
                inner join SFCS_PARAMETERS p on a.OPERATION_CATEGORY=p.LOOKUP_CODE and p.LOOKUP_TYPE='OPERATION_CATEGORY' AND p.MEANING='REPAIR' ";

                string pagedSql = SQLBuilderClass.GetPagedSQL(sql, " a.ID desc ", conditions);
                var resdata = await _dbConnection.QueryAsync<object>(pagedSql, model);
                string sqlcnt = @" select  count(0)   from  Sfcs_Operations a
                inner join SFCS_PARAMETERS p on a.OPERATION_CATEGORY=p.LOOKUP_CODE and p.LOOKUP_TYPE='OPERATION_CATEGORY' AND p.MEANING='REPAIR' " + conditions;
                int cnt = await _dbConnection.ExecuteScalarAsync<int>(sqlcnt, model);

                return new TableDataModel
                {
                    count = cnt,
                    data = resdata?.ToList(),
                };
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        /// <summary>
        /// 获取原因代码下拉框数据
        /// </summary>
        /// <returns></returns>
        public async Task<TableDataModel> GetReasonCodeList(DropDownBoxRequestModel model)
        {
            try
            {
                string conditions = " WHERE temp.ENABLED = 'Y' ";
                if (!model.Key.IsNullOrEmpty())
                {
                    conditions += $"and (instr(temp.REASON_CODE, :Key) > 0  or instr(temp.CHINESE_DESCRIPTION, :Key) > 0)";
                }
                string sql = @"SELECT ROWNUM as rowno, temp.REASON_CODE, temp.CHINESE_DESCRIPTION
                                FROM SFCS_REASON_CONFIG temp   ";

                string pagedSql = SQLBuilderClass.GetPagedSQL(sql, " temp.REASON_CODE ", conditions);
                var resdata = await _dbConnection.QueryAsync<object>(pagedSql, model);
                string sqlcnt = @"select count(0) FROM SFCS_REASON_CONFIG temp  " + conditions;
                int cnt = await _dbConnection.ExecuteScalarAsync<int>(sqlcnt, model);

                return new TableDataModel
                {
                    count = cnt,
                    data = resdata?.ToList(),
                };
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }

        /// <summary>
        /// 获取排除故障下拉框数据
        /// </summary>
        /// <returns></returns>
        public async Task<TableDataModel> GetResponserList(DropDownBoxRequestModel model)
        {
            try
            {
                string conditions = " WHERE  1=1  ";
                if (!model.Key.IsNullOrEmpty())
                {
                    conditions += $" and (instr(temp.DESCRIPTION, :Key) > 0  or instr(temp.MEANING, :Key) > 0  or instr(temp.CODE, :Key) > 0)";
                }
                string sql = @"select ROWNUM as rowno,temp.* from (
                                SELECT SL.CODE, SL.DESCRIPTION, SL.CHINESE,SP.MEANING
                                FROM SFCS_LOOKUPS SL, SFCS_PARAMETERS SP
                               WHERE     SP.LOOKUP_TYPE = 'VARIOUS_DATA_TYPE'
                                     AND SP.LOOKUP_CODE = SL.KIND
                                     AND SP.ENABLED = 'Y'
                                     AND SL.ENABLED = 'Y' ) temp ";
                var objectlist = await _dbConnection.QueryAsync<dynamic>(sql);
                string pagedSql = SQLBuilderClass.GetPagedSQL(sql, " temp.CODE ", conditions);
                var resdata = await _dbConnection.QueryAsync<object>(pagedSql, model);
                string sqlcnt = @"select count(0) FROM (
                                SELECT SL.CODE, SL.DESCRIPTION, SL.CHINESE,SP.MEANING
                                FROM SFCS_LOOKUPS SL, SFCS_PARAMETERS SP
                               WHERE     SP.LOOKUP_TYPE = 'VARIOUS_DATA_TYPE'
                                     AND SP.LOOKUP_CODE = SL.KIND
                                     AND SP.ENABLED = 'Y'
                                     AND SL.ENABLED = 'Y') temp  " + conditions;
                int cnt = await _dbConnection.ExecuteScalarAsync<int>(sqlcnt, model);

                return new TableDataModel
                {
                    count = cnt,
                    data = resdata?.ToList(),
                };
            }
            catch (Exception ex)
            {
                return null;
            }
        }



        /// <summary>
        /// 根据线体、站位、工序获取（“维修”工序类）站位信息
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public async Task<TableDataModel> GetRepairSiteData(SfcsCollectRepairSiteRequestModel model)
        {
            string conditions = " where  1=1  ";
            if (model.LINE_ID > 0)
            {
                conditions += $" and temp.LINE_ID=:LINE_ID ";
            }
            //if (model.SITE_ID > 0)
            //{
            //    conditions += $" and temp.SITE_ID=:SITE_ID ";
            //}
            if (model.OPER_ID > 0)
            {
                conditions += $" and temp.OPER_ID=:OPER_ID ";
            }
            if (!model.Key.IsNullOrEmpty())
            {
                conditions += $"and (instr(temp.SITE_NAME, :Key) > 0 )";
            }

            string sql = @"select rownum as rowno ,temp.* from (   
							 select distinct line.LINE_NAME, line.ID as LINE_ID,oper.DESCRIPTION as OPER_NAME,oper.ID as OPER_ID, 
							 opsi.OPERATION_SITE_NAME as SITE_NAME ,opsi.ID as SITE_ID,opsi.DESCRIPTION from  sfcs_operation_sites opsi
								inner join (select ID,LINE_NAME,ORGANIZE_ID from  SMT_LINES
											union all    
											select ID,OPERATION_LINE_NAME as LINE_NAME,ORGANIZE_ID from SFCS_OPERATION_LINES where ENABLED='Y') line on line.ID=opsi.OPERATION_LINE_ID
								inner join Sfcs_Operations oper on opsi.OPERATION_ID=oper.ID
                                inner join SFCS_PARAMETERS para on oper.OPERATION_CATEGORY=para.LOOKUP_CODE and para.LOOKUP_TYPE='OPERATION_CATEGORY' AND para.MEANING='REPAIR') temp ";

            string pagedSql = SQLBuilderClass.GetPagedSQL(sql, " temp.SITE_ID desc", conditions);
            var resdata = await _dbConnection.QueryAsync<object>(pagedSql, model);

            string sqlcnt = @"select count(0) from (   
							 select distinct line.LINE_NAME, line.ID as LINE_ID,oper.DESCRIPTION as OPER_NAME,oper.ID as OPER_ID, 
							 opsi.OPERATION_SITE_NAME as SITE_NAME ,opsi.ID as SITE_ID,oper.DESCRIPTION from  sfcs_operation_sites opsi
								inner join (select ID,LINE_NAME,ORGANIZE_ID from  SMT_LINES
											union all    
											select ID,OPERATION_LINE_NAME as LINE_NAME,ORGANIZE_ID from SFCS_OPERATION_LINES where ENABLED='Y') line on line.ID=opsi.OPERATION_LINE_ID
								inner join Sfcs_Operations oper on opsi.OPERATION_ID=oper.ID
                                inner join SFCS_PARAMETERS para on oper.OPERATION_CATEGORY=para.LOOKUP_CODE and para.LOOKUP_TYPE='OPERATION_CATEGORY' AND para.MEANING='REPAIR' ) temp   " + conditions;
            int cnt = await _dbConnection.ExecuteScalarAsync<int>(sqlcnt, model);

            return new TableDataModel
            {
                count = cnt,
                data = resdata?.ToList(),
            };
        }


        /// <summary>
        /// 根据工序ID获取未维修数量
        /// </summary>
        /// <param name="OPER_ID"></param>
        /// <returns></returns>
        public async Task<decimal> GetRefreshUnrepairedQty(decimal? OPER_ID)
        {
            string strwhere = " and 1=1 ";
            if (OPER_ID > 0)
            {
                strwhere += string.Format(" and SR.WIP_OPERATION={0}", OPER_ID);
            }
            string sql = @"SELECT COUNT (DISTINCT (SR.SN)) QTY
								FROM SFCS_COLLECT_DEFECTS SCD, SFCS_RUNCARD SR
								WHERE SR.ID = SCD.SN_ID
								AND SCD.REPAIR_TIME IS NULL
								AND SR.STATUS = 2 " + strwhere;
            object result = await _dbConnection.ExecuteScalarAsync(sql);

            return Convert.ToDecimal(result);

        }

        /// <summary>
        /// 保存维修数据
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public async Task<string> SaveRepairData(SfcsCollectDefectsModel model)
        {
            string returnMsg = "";
            ConnectionFactory.OpenConnection(_dbConnection);
            using (var tran = _dbConnection.BeginTransaction())
            {
                try
                {
                    //新增Repair Recipe
                    string insertsql = @"INSERT INTO SFCS_REPAIR_RECIPE(RECIPE_ID, COLLECT_DEFECT_ID,
                                                     REASON_CODE, LOCATION, RESPONSER, BAD_PART_NO,  TTF,DEFECT_DESCRIPTION,  REMARK)
                                                     VALUES(:RECIPE_ID, :COLLECT_DEFECT_ID, :REASON_CODE,
                                                     :LOCATION, :RESPONSER, :BAD_PART_NO, :TTF, :DEFECT_DESCRIPTION, :REMARK ) ";
                    var newid = await GetRepairSEQID();
                    var resdata = await _dbConnection.ExecuteAsync(insertsql, new
                    {
                        RECIPE_ID = newid,
                        model.COLLECT_DEFECT_ID,
                        model.REASON_CODE,
                        LOCATION = "NA",
                        model.RESPONSER,
                        model.BAD_PART_NO,
                        TTF = "00:01",
                        DEFECT_DESCRIPTION = "N/A",
                        model.REMARK,

                    }, tran);

                    //修改Collect Defect
                    string updatesql = @"UPDATE SFCS_COLLECT_DEFECTS SET REPAIR_OPERATION_ID = :REPAIR_OPERATION_ID,
                                                      REPAIR_SITE_ID = :REPAIR_SITE_ID, REPAIRER = :REPAIRER, REPAIR_TIME = SYSDATE, REPAIR_FLAG = 'Y'
                                                      WHERE COLLECT_DEFECT_ID = :COLLECT_DEFECT_ID ";

                    var repairid = await GetCollectDefectSEQID();
                    var resupdatedata = await _dbConnection.ExecuteAsync(updatesql, new
                    {
                        model.COLLECT_DEFECT_ID,
                        REPAIR_OPERATION_ID = repairid,
                        REPAIR_SITE_ID = model.REPAIRSITEROW_ID,
                        model.REPAIRER,
                    }, tran);

                    // Repaired Rucanrd处理方法
                    returnMsg = await RepairedRucanrd(model, repairid);

                    tran.Commit();
                }
                catch (Exception ex)
                {
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
            return returnMsg;
        }

        /// <summary>
        /// 保存报废功能
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public async Task<bool> SaveScrappedData(String SN)
        {
            bool resultBool = false;
            ConnectionFactory.OpenConnection(_dbConnection);
            using (var tran = _dbConnection.BeginTransaction())
            {
                try
                {

                    // 修改runcard中 状态为:维修报废(17)
                    string updateRuncardStatus = " UPDATE SFCS_RUNCARD SET STATUS=17 WHERE SN=:SN ";
                    //工单报废数量加1
                    string updateWoScrapQTY = "UPDATE SFCS_WO SET SCRAP_QTY=NVL(SCRAP_QTY,0)+1 WHERE ID in (SELECT WO_ID FROM SFCS_RUNCARD WHERE SN=:SN)";

                    var result = await _dbConnection.ExecuteAsync(updateRuncardStatus, new { SN = SN }, tran);
                    if (result > 0)
                    {
                        result = await _dbConnection.ExecuteAsync(updateWoScrapQTY, new { SN = SN }, tran);
                        if (result<=0)
                        {
                            tran.Rollback();
                        }
                    }
                    resultBool = true;
                    tran.Commit();
                }
                catch (Exception ex)
                {
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
            return resultBool;
        }


        /// <summary>
        /// Repaired Rucanrd处理方法
        /// </summary>
        private async Task<string> RepairedRucanrd(SfcsCollectDefectsModel model, decimal repairOperationID)
        {
            try
            {
                //获取runcardTable数据
                //List<string> snList = await GetMultiRuncardList(model.SN);
                //List<SfcsRuncardListModel> runcardData = null;
                //if (snList.Count == 1)
                //{
                //    runcardData = await GetRuncardDataTable(0, model.SN);
                //}
                //else
                //{
                //    foreach (string item in snList)
                //    {
                //        runcardData = await GetRuncardDataTable(0, item);
                //    }
                //}
                List<SfcsRuncardListModel> runcardData = await GetRuncardDataTable(0, model.SN);
                decimal reworkOperationID = 0, operationOrderNO = 0;
                string reworkOperationName = "";
                var routeTable = await GetRouteConfigDataTable((decimal)runcardData[0].ROUTE_ID, 0);
                foreach (var item in routeTable)
                {
                    if (item.CURRENT_OPERATION_ID == model.FAILOPERATIONID)
                    {
                        reworkOperationID = item.REWORK_OPERATION_ID;
                        break;
                    }
                }
                //沒有找到返回工序或返回工序為No Route時必須報警錯誤
                if ((reworkOperationID == 0) || (reworkOperationID == GlobalVariables.NoRoute))
                {
                    throw new Exception("系统找不到不良维修后应该返工到哪个工位，请确认制程配置。");
                }
                //获取工序信息
                var reworkOperation = await GetOperationDataTable(reworkOperationID);
                if (reworkOperation != null && reworkOperation.Count > 0)
                {
                    reworkOperationName = reworkOperation[0].OPERATION_NAME;
                }
                //获取制程配置信息
                var operationOrder = await GetRouteConfigDataTable((decimal)runcardData[0].ROUTE_ID, reworkOperationID);
                if (operationOrder != null && operationOrder.Count > 0)
                {
                    operationOrderNO = operationOrder[0].ORDER_NO;
                }
                // 找出需要清除的已采集数据
                var collectObjectTable = await FindCollectObjectsInOperation(true, true, true, runcardData[0].SN, (decimal)runcardData[0].ROUTE_ID, operationOrderNO);

                // 返工清除已采集数据
                foreach (var item in runcardData)
                {
                    ClearObjects(collectObjectTable, runcardData[0], repairOperationID, operationOrderNO, model.REPAIRER);
                }

                string finishedMessage = "当前流水号: {0} 还有待维修，请继续维修。";
                // 获取SN的待维修不良
                var repairingDefectTable = await GetCollectDefectsNotEsistsSN((decimal)runcardData[0].ID, model.COLLECT_DEFECT_ID.Value);
                if (repairingDefectTable.Count <= 0)//同一SN，所有不良代码都维修完了才更新SN
                {
                    // 更新SN
                    bool updateRucard = await UpdateRuncardRoute((decimal)model.REPAIRSITEROW_ID, reworkOperationID, (decimal)runcardData[0].LAST_OPERATION, GlobalVariables.Repaired, runcardData[0].SN);

                    finishedMessage = "当前流水号: {0} 已经维修成功，已经返到 {1} 对应工位,请送回产线继续作业。";
                }
                // 插入SN维修历史纪录
                RecordOperationHistory(model, runcardData[0], repairOperationID);


                // 其余连板消NTF
                ProcessMultiRuncardDefect(model, runcardData[0], repairOperationID, operationOrderNO, reworkOperationID);

                return string.Format(finishedMessage, model.SN, reworkOperationName);

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        /// <summary>
        /// 处理连板消不良
        /// </summary>
        private async void ProcessMultiRuncardDefect(SfcsCollectDefectsModel model, SfcsRuncardListModel _runcardRow, decimal repairOperationID, decimal operationOrderNO, decimal reworkOperationID)
        {
            try
            {
                var multiRuncardTable = await GetMultiRuncardDataTableBySN(_runcardRow.SN);
                if (multiRuncardTable != null && multiRuncardTable.Count > 0)
                {
                    foreach (var item in multiRuncardTable)
                    {
                        if (item.STATUS != 1)
                        {
                            // 已拆板，按单板处理
                            break;
                        }
                        if (item.SN_ID == _runcardRow.ID)
                        {
                            // 当前SN已经处理过，不用再处理
                            continue;
                        }
                        var panelRuncardRow = await GetRuncardDataTable(item.SN_ID, "");
                        if (!panelRuncardRow.IsNullOrEmpty() || panelRuncardRow[0].STATUS != GlobalVariables.Fail)
                        {
                            // 不是不良的不用处理
                            continue;
                        }
                        // 找出SN的待维修不良
                        var notRepairDefectTable = await GetCollectDefects(item.SN_ID, GlobalVariables.EnableN);

                        if (!notRepairDefectTable.IsNullOrEmpty() && notRepairDefectTable.Count > 0)
                        {
                            // 消不良 NTF
                            foreach (var defectRow in notRepairDefectTable)
                            {
                                // Repair Collect Defect
                                bool updateRepair = await RepairCollectDefect(defectRow.COLLECT_DEFECT_ID, repairOperationID, (decimal)model.REPAIRSITEROW_ID, model.REPAIRER);
                                // Repair Recipe
                                decimal recipeID = await GetRepairSEQID();
                                // Insert Repair Recipe
                                bool addRepair = await InsertRepairRecipe(recipeID,
                                                defectRow.COLLECT_DEFECT_ID,
                                                "", // rootCauseCategory
                                                "NTF",//reasonCode
                                                "", // location
                                                "", // responser 
                                                "", // badPartCode
                                                "", // lotCode
                                                "", // actionCode
                                                "", // timeFail
                                                "", // badPartVendor 
                                                "", // replacePNDateCode
                                                "", // replacePNVendor 
                                                "", // replacePNDeviceValue 
                                                "", // reelID 
                                                "", // defectDescription
                                                "", // dateCode
                                                "", // revisionNo
                                                "", // verify
                                                "", // remark
                                                "", // assemblyKind
                                                "", // rmaRepairType
                                                "" // ReRepairMark
                                                );
                            }


                            // 清除数据
                            // 找出需要清除的已采集数据
                            var collectObjectTable = await FindCollectObjectsInOperation(true, true, true, panelRuncardRow[0].SN, (decimal)panelRuncardRow[0].ROUTE_ID, operationOrderNO);

                            /// 清除對象 根據用戶設置+IE設置自動刪除對象數據
                            ClearObjects(collectObjectTable, panelRuncardRow[0], repairOperationID, operationOrderNO, model.REPAIRER);

                            // 更新SN
                            bool updateRuncard = await UpdateRuncardRoute((decimal)model.REPAIRSITEROW_ID, reworkOperationID, (decimal)panelRuncardRow[0].LAST_OPERATION, GlobalVariables.Repaired, panelRuncardRow[0].SN);

                            // 插入SN维修历史纪录
                            bool addOperation = await InsertOperationHistory(repairOperationID, (decimal)panelRuncardRow[0].WO_ID,
                    panelRuncardRow[0].ID, (decimal)panelRuncardRow[0].ROUTE_ID, (decimal)model.OPERATION_ID, (decimal)model.REPAIRSITEROW_ID, model.REPAIRER, GlobalVariables.Repaired, null);
                        }


                    }
                }

            }
            catch (Exception ex)
            {

                throw ex;
            }



        }
        /// <summary>
        /// 根據SN找出所有連板
        /// </summary>
        /// <param name="SN"></param>
        /// <returns></returns>
        private async Task<List<SfcsCollectMultiRuncardListModel>> GetMultiRuncardDataTableBySN(string SN)
        {
            try
            {
                string sql = @"SELECT SCMR1.* FROM SFCS_COLLECT_MULTI_RUNCARD SCMR,SFCS_RUNCARD SR,SFCS_COLLECT_MULTI_RUNCARD SCMR1 WHERE SCMR.SN_ID=SR.ID
                                                         AND SR.SN=:SN AND SCMR.ID=SCMR1.ID";
                var list = await _dbConnection.QueryAsync<SfcsCollectMultiRuncardListModel>(sql, new { SN });
                return list.ToList();
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }

        /// <summary>
        /// 維修收集的不良
        /// </summary>
        /// <param name="collectDefectID"></param>
        /// <param name="repairOperationID"></param>
        /// <param name="repairSiteID"></param>
        /// <param name="repairer"></param>
        /// <returns></returns>
        private async Task<bool> RepairCollectDefect(decimal collectDefectID, decimal repairOperationID,
            decimal repairSiteID, string repairer)
        {
            try
            {
                string sql = @"UPDATE SFCS_COLLECT_DEFECTS SET REPAIR_OPERATION_ID = :REPAIR_OPERATION_ID,
                                                      REPAIR_SITE_ID = :REPAIR_SITE_ID, REPAIRER = :REPAIRER, REPAIR_TIME = SYSDATE, REPAIR_FLAG = 'Y'
                                                      WHERE COLLECT_DEFECT_ID = :COLLECT_DEFECT_ID";
                return await _dbConnection.ExecuteAsync(sql, new
                {
                    COLLECT_DEFECT_ID = collectDefectID,
                    REPAIR_OPERATION_ID = repairOperationID,
                    REPAIR_SITE_ID = repairSiteID,
                    REPAIRER = repairer
                }) > 0;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// 收集維修處方
        /// </summary>
        /// <param name="_recipeID"></param>
        /// <param name="collectDefectID"></param>
        /// <param name="rootCauseCategory"></param>
        /// <param name="reasonCode"></param>
        /// <param name="location"></param>
        /// <param name="responser"></param>
        /// <param name="badPartNo"></param>
        /// <param name="lotCode"></param>
        /// <param name="actionCode"></param>
        /// <param name="ttf"></param>
        /// <param name="badPartVendor"></param>
        /// <param name="replacedPNDateCode"></param>
        /// <param name="replacedPNVendor"></param>
        /// <param name="replacedPNDeviceValue"></param>
        /// <param name="reelID"></param>
        /// <param name="defectDesc"></param>
        /// <param name="dateCode"></param>
        /// <param name="revision"></param>
        /// <param name="verify"></param>
        /// <param name="remark"></param>
        /// <param name="assemblyKind"></param>
        /// <param name="rmaRepairType"></param>
        /// <param name="reRepairMark"></param>
        /// <returns></returns>
        private async Task<bool> InsertRepairRecipe(decimal? _recipeID, decimal collectDefectID,
            string rootCauseCategory, string reasonCode, string location,
            string responser, string badPartNo, string lotCode, string actionCode, string ttf,
            string badPartVendor, string replacedPNDateCode, string replacedPNVendor,
            string replacedPNDeviceValue, string reelID, string defectDesc, string dateCode,
            string revision, string verify, string remark, string assemblyKind, string rmaRepairType,
            string reRepairMark)
        {
            try
            {

                decimal recipeID = 0;
                if (_recipeID.IsNullOrEmpty())
                {
                    recipeID = await GetRepairSEQID();
                }
                else
                {
                    recipeID = (decimal)_recipeID;
                }
                string sql = @"INSERT INTO SFCS_REPAIR_RECIPE(RECIPE_ID, COLLECT_DEFECT_ID, ROOT_CAUSE_CATEGORY,
                                                     REASON_CODE, LOCATION, RESPONSER, BAD_PART_NO, LOT_CODE, ACTION_CODE, TTF, BAD_PART_VENDOR,
                                                     REPLACED_PN_DATE_CODE, REPLACED_PN_VENDOR, REPLACED_PN_DEVICE_VALUE, REEL_ID, DEFECT_DESCRIPTION,
                                                     DATE_CODE, REVISION_NO, VERIFY, REMARK, ASSEMBLY_KIND, RMA_REPAIR_TYPE,RE_REPAIR_MARK)
                                                     VALUES(:RECIPE_ID, :COLLECT_DEFECT_ID, :ROOT_CAUSE_CATEGORY, :REASON_CODE,
                                                     :LOCATION, :RESPONSER, :BAD_PART_NO, :LOT_CODE, :ACTION_CODE, :TTF, :BAD_PART_VENDOR,
                                                     :REPLACED_PN_DATE_CODE, :REPLACED_PN_VENDOR, :REPLACED_PN_DEVICE_VALUE, :REEL_ID, :DEFECT_DESCRIPTION,
                                                     :DATE_CODE, :REVISION_NO, :VERIFY, :REMARK, :ASSEMBLY_KIND, :RMA_REPAIR_TYPE,:RE_REPAIR_MARK) ";

                return await _dbConnection.ExecuteAsync(sql, new
                {
                    RECIPE_ID = recipeID,
                    COLLECT_DEFECT_ID = collectDefectID,
                    ROOT_CAUSE_CATEGORY = rootCauseCategory,
                    REASON_CODE = reasonCode,
                    LOCATION = location,
                    RESPONSER = responser,
                    BAD_PART_NO = badPartNo,
                    LOT_CODE = lotCode,
                    ACTION_CODE = actionCode,
                    TTF = ttf,
                    BAD_PART_VENDOR = badPartVendor,
                    REPLACED_PN_DATE_CODE = replacedPNDateCode,
                    REPLACED_PN_VENDOR = replacedPNVendor,
                    REPLACED_PN_DEVICE_VALUE = replacedPNDeviceValue,
                    REEL_ID = reelID,
                    DEFECT_DESCRIPTION = defectDesc,
                    DATE_CODE = dateCode,
                    REVISION_NO = revision,
                    VERIFY = verify,
                    REMARK = remark,
                    ASSEMBLY_KIND = assemblyKind,
                    RMA_REPAIR_TYPE = rmaRepairType,
                    RE_REPAIR_MARK = reRepairMark
                }) > 0;

            }
            catch (Exception ex)
            {

                throw ex;
            }

        }





        /// <summary>
        /// 清除對象
        /// 根據用戶設置+IE設置自動刪除對象數據
        /// </summary>
        private async void ClearObjects(List<SfcsCollectObjectsListMode> collectObjectTable, SfcsRuncardListModel _runcardRow, decimal repairOperationID, decimal _orderNO, string _repairer)
        {
            decimal originalRouteID = (decimal)_runcardRow.ROUTE_ID;
            if (_runcardRow.IsNullOrEmpty())
            {
                return;
            }
            foreach (var item in collectObjectTable)
            {
                if (item.REWORK_REMOVE_FLAG == GlobalVariables.EnableY)
                {
                    switch (item.OBJECT_KIND)
                    {
                        //清除零件
                        case "COMPONENT":
                            ClearComponents(await GetCollectComponentsByRoute(_runcardRow.ID, item.OBJECT_NAME, item.ODM_PN, item.CUSTOMER_PN, originalRouteID, _orderNO), repairOperationID, _repairer, (decimal)_runcardRow.CURRENT_SITE);
                            break;

                        //清除資源
                        case "RESOURCE":
                            ClearResources(await GetCollectResourceTableByRoute(_runcardRow.ID, item.OBJECT_NAME, originalRouteID, _orderNO), repairOperationID, _repairer, (decimal)_runcardRow.CURRENT_SITE);
                            break;

                        //清除UID
                        case "UID":
                            ClearUID(await GetCollectUIDByRoute(_runcardRow.ID, item.OBJECT_NAME, originalRouteID, _orderNO), repairOperationID, _repairer, (decimal)_runcardRow.CURRENT_SITE);
                            break;

                        default:
                            break;
                    }
                }
            }
        }

        /// <summary>
        /// 更新流水号制程
        /// </summary>
        /// <param name="currentSite"></param>
        /// <param name="wipRoute"></param>
        /// <param name="lastRoute"></param>
        /// <param name="status"></param>
        /// <param name="sn"></param>
        /// <returns></returns>
        private async Task<bool> UpdateRuncardRoute(decimal currentSite, decimal wipRoute, decimal lastRoute,
            decimal status, string sn)
        {
            try
            {
                string sql = @"UPDATE SFCS_RUNCARD
                               SET CURRENT_SITE = :CURRENT_SITE,
                                   WIP_OPERATION = :WIP_OPERATION,
                                   LAST_OPERATION = :LAST_OPERATION,
                                   STATUS = :STATUS,
                                   OPERATION_TIME = SYSDATE
                             WHERE SN = :SN ";

                return await _dbConnection.ExecuteAsync(sql, new
                {
                    CURRENT_SITE = currentSite,
                    WIP_OPERATION = wipRoute,
                    LAST_OPERATION = lastRoute,
                    STATUS = status,
                    SN = sn
                }) > 0;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        /// <summary>
        /// 根据SN获取不良维修等信息
        /// </summary>
        /// <param name="SN"></param>
        /// <returns></returns>
        public async Task<List<SfcsCollectBadDataListModel>> GetDefectDataBySN(SfcsCollectBadRequestModel model)
        {
            try
            {
                List<SfcsCollectBadDataListModel> badlist = new List<SfcsCollectBadDataListModel>();
                SfcsCollectBadDataListModel baddata = new SfcsCollectBadDataListModel();
                //List<string> snList = await GetMultiRuncardList(model.SN);
                List<SfcsRuncardListModel> runcardData = null;
                string productFamily = "";
                decimal workOrderID = 0;
                DateTime buildDate;
                //if (snList.Count == 1)
                //{
                //    baddata.SN = model.SN;
                //    runcardData = await GetRuncardDataTable(0, baddata.SN);
                //}
                //else
                //{
                //    foreach (string item in snList)
                //    {
                //        runcardData = await GetRuncardDataTable(0, item);
                //        if (runcardData.Count() == 0)
                //        {
                //            throw new Exception(string.Format("流水号{0}找不到。", item));
                //        }
                //        if (runcardData[0].STATUS == GlobalVariables.Fail)
                //        {
                //            baddata.SN = item;
                //            break;
                //        }
                //    }
                //}
                baddata.SN = model.SN;
                runcardData = await GetRuncardDataTable(0, baddata.SN);
                // 检查SN状态
                if (runcardData[0].STATUS != GlobalVariables.Fail)
                {
                    throw new Exception(string.Format("{0}产品流水号不是不良状态，不能进行维修。", baddata.SN));
                }
                // 获取产品信息
                baddata.ROUTE_ID = (decimal)runcardData[0].ROUTE_ID;
                baddata.RUNCARDROW_ID = (decimal)runcardData[0].ID;
                //baddata.LAST_OPERATION = runcardData[0].LAST_OPERATION;

                var wolist = await GetWorkOrderTable((decimal)runcardData[0].WO_ID);

                if (wolist != null && wolist.Count > 0)
                {
                    baddata.WO_WO = wolist[0].WO_NO;
                    workOrderID = wolist[0].ID;
                    baddata.PART_NO = wolist[0].PART_NO;
                    buildDate = (DateTime)(wolist[0].START_DATE == null ? System.DateTime.Now : wolist[0].START_DATE);

                    var partNoTable = await GetPartNumberDataTable(baddata.PART_NO);
                    if (partNoTable != null && partNoTable.Count > 0)
                    {
                        // 产品系列
                        var familyRow = await GetProductFamilyDataTable((decimal)partNoTable[0].FAMILY_ID);
                        if (familyRow != null && familyRow.Count > 0)
                        {
                            productFamily = familyRow[0].FAMILY_NAME;
                        }
                    }

                    // 机种
                    var modelRow = await GetModelDataTable(wolist[0].MODEL_ID);
                    if (modelRow != null && modelRow.Count > 0)
                    {
                        baddata.MODEL = modelRow[0].MODEL;
                    }
                    // 通過最後一次fail作業記錄找到Fail時的工序
                    var historyRow = await GetLastFailHistory(runcardData[0].ID);
                    if (historyRow == null || historyRow.Count <= 0)
                    {
                        throw new Exception(string.Format("流水号{0}找不到刷不良的记录。", baddata.SN));
                    }
                    baddata.FAILOPERATIONID = (decimal)historyRow[0].SITE_OPERATION_ID;

                    // 获取流水号最近作业站点信息
                    var operationSiteRow = await GetOperationSiteDataTable((decimal)historyRow[0].OPERATION_SITE_ID, 0, 0);
                    if (operationSiteRow != null && operationSiteRow.Count > 0)
                    {
                        baddata.DEFECTSITE = operationSiteRow[0].OPERATION_SITE_NAME;

                        // 取得維修站點
                        var RepairSiteRow = await GetRepairSiteRow(runcardData[0].ID, (decimal)runcardData[0].WIP_OPERATION, operationSiteRow[0], baddata.SN);
                        if (RepairSiteRow != null && RepairSiteRow.Count > 0)
                        {
                            baddata.REPAIRSITEROW_ID = RepairSiteRow[0].ID;
                            baddata.OPERATION_ID = RepairSiteRow[0].OPERATION_ID;
                        }
                    }
                    // 获取SN的待维修不良
                    var repairingDefectTable = await GetCollectDefects((decimal)runcardData[0].ID, GlobalVariables.EnableN);
                    baddata.DEFECT_CODE = new List<SfcsCollectBadDataCodeModel>();
                    baddata.DEFECT_DETAIL = new List<SfcsCollectBadDataDetailModel>();
                    //加载不良信息
                    foreach (SfcsCollectDefectsListModel item in repairingDefectTable)
                    {
                        //baddata.COLLECT_DEFECT_ID = item.COLLECT_DEFECT_ID;
                        var defectlist = await GetDefectsByCode(item.DEFECT_CODE);
                        string defectDesc = "";
                        string chineseDesc = "";
                        if (defectlist != null && defectlist.Count > 0)
                        {
                            defectDesc = defectlist.IsNull() ? string.Empty : defectlist[0].DEFECT_DESCRIPTION;
                            chineseDesc = defectlist.IsNull() ? string.Empty : defectlist[0].CHINESE_DESCRIPTION;
                        }

                        baddata.DEFECT_CODE.Add(new SfcsCollectBadDataCodeModel()
                        {
                            COLLECT_DEFECT_ID = item.COLLECT_DEFECT_ID,
                            DEFECT_CODE = item.DEFECT_CODE,
                            DEFECT_DESCRIPTION = defectDesc,
                            CHINESE_DESCRIPTION = chineseDesc
                        });

                        if (item.COLLECT_DEFECT_DETAIL_ID > 0)
                        {
                            var detaillist = await GetCollectDefectsDetail((decimal)item.COLLECT_DEFECT_DETAIL_ID);
                            detaillist?.ForEach(x =>
                            {
                                baddata.DEFECT_DETAIL.Add(new SfcsCollectBadDataDetailModel() { DEFECT_CODE = item.DEFECT_CODE, DEFECT_DETAIL = x.DEFECT_DETAIL });
                            });
                        }
                    }
                    //获取前15不良原因排名
                    if (repairingDefectTable != null && repairingDefectTable.Count > 0)
                    {
                        baddata.TOPROOTCAUSES = new List<Top15RootCausesListModel>();
                        baddata.TOPROOTCAUSES = await GetTop15RootCauses(repairingDefectTable[0].DEFECT_CODE);
                    }

                    // 获取SN的已维修不良
                    var repairedDefectTable = await GetCollectDefects((decimal)runcardData[0].ID, GlobalVariables.EnableY);
                    baddata.REPAIRED_DEFECTS = new List<string>();
                    baddata.REPAIRED_CODE = new List<string>();
                    //加载已维修不良
                    foreach (SfcsCollectDefectsListModel item in repairedDefectTable)
                    {
                        baddata.REPAIRED_DEFECTS.Add(item.DEFECT_CODE);

                        if (item.COLLECT_DEFECT_ID > 0)
                        {
                            var defectlist = await GetRepairRecipeData(item.COLLECT_DEFECT_ID);
                            defectlist?.ForEach(x =>
                            {
                                baddata.REPAIRED_CODE.Add(x.REASON_CODE);
                            });
                        }
                    }
                }
                badlist.Add(baddata);
                return badlist;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// 清除Component對象數據
        /// </summary>
        private async void ClearComponents(List<SfcsCollectComponentsListModel> componentTable, decimal repairOperationID, string repairer, decimal siteID)
        {
            foreach (var row in componentTable)
            {
                bool addBack = await BackupComponentReworkData(repairOperationID, repairer, siteID, row.COLLECT_COMPONENT_ID);
                bool deleteComponent = await DeleteComponent(row.COLLECT_COMPONENT_ID);

                if (row.DEVICE_FLAG == GlobalVariables.EnableY)
                {
                    bool updateDevice = await UpdateDeviceComponentWhenRework(4, row.CUSTOMER_COMPONENT_SN);
                }

                ClearAttachments(repairOperationID, repairer, siteID, row.COLLECT_COMPONENT_ID);

                if ((row.COMPONENT_NAME.IndexOf("MB") != -1) ||
                    (row.COMPONENT_NAME.IndexOf("MP") != -1))
                {
                    ClearMBsMAC(row.CUSTOMER_COMPONENT_SN, siteID, repairOperationID, repairer);
                }

            }

        }

        /// <summary>
        /// 清除資源對象數據
        /// </summary>
        /// <param name="resourceTable"></param>
        private async void ClearResources(List<SfcsCollectResourcesListModel> resourceTable, decimal repairOperationID, string repairer, decimal siteID)
        {
            foreach (var row in resourceTable)
            {
                bool addBack = await BackupResourceReworkData(repairOperationID, repairer, siteID, row.COLLECT_RESOURCE_ID);
                bool deleteResource = await DeleteResource(row.COLLECT_RESOURCE_ID);

                ClearAttachments(repairOperationID, repairer, siteID, row.COLLECT_RESOURCE_ID);
            }
        }

        /// <summary>
        /// 清除MB時把MB對應的MAC一齊清除
        /// </summary>
        /// <param name="mbSN"></param>
        private async void ClearMBsMAC(string mbSN, decimal siteID, decimal repairOperationID, string repairer)
        {
            List<string> macList = await GetMotherBoardMAC(mbSN);

            if (macList != null && macList.Count > 0)
            {
                foreach (string item in macList)
                {
                    var macTable = await GetCollectUIDDataTable(siteID, "", item);

                    if (macTable != null && macTable.Count > 0)
                    {
                        ClearUID(macTable, repairOperationID, repairer, siteID);
                    }
                }
            }


        }

        /// <summary>
        /// 通過主板sn獲取mac
        /// </summary>
        /// <param name="mbSerialNumber"></param>
        /// <returns></returns>
        private async Task<List<string>> GetMotherBoardMAC(string mbSerialNumber)
        {
            List<string> mbMacList = new List<string>();
            try
            {
                //里面的function中返回的数据是空的，并且不清楚这里需要的是哪个字段（CS端返回的是一个字典），所以先放空走下面的流程
                List<string> resultList = await GetMacListBySNandMAC(mbSerialNumber, "MAC");
                string mbMac = "";
                //mbMac = resultList["RETURN_VALUE"].ToString();

                if (!mbMac.IsNullOrEmpty())
                {
                    string[] macList = mbMac.Split(GlobalVariables.comma);
                    mbMacList = macList.ToList<string>();
                }
                else
                {
                    decimal? mbID = await GetNewSFCSMBKey(mbSerialNumber);
                    if (!mbID.IsNull())
                    {
                        var uidTables = await GetCollectUIDDataTable((decimal)mbID, "MAC", "");
                        if (!uidTables.IsNull())
                        {
                            foreach (var item in uidTables)
                            {
                                mbMacList.Add(item.UID_NUMBER);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                //throw;
            }
            return mbMacList;
        }

        /// <summary>
        /// 获取Oracle程序包里面的数据
        /// </summary>
        /// <param name="SN"></param>
        /// <param name="MAC"></param>
        /// <returns></returns>
        private async Task<List<string>> GetMacListBySNandMAC(string SN, string MAC)
        {
            try
            {
                List<string> list = new List<string>();
                string message = "";
                string trace = "";
                string retuens = "";
                var p = new DynamicParameters();
                p.Add(":SN", SN);
                p.Add(":UID_KIND", MAC);
                p.Add(":MESSAGE", message, DbType.String, ParameterDirection.Output);
                p.Add(":TRACE", trace, DbType.String, ParameterDirection.Output);
                p.Add(":RETURNMSG", retuens, DbType.String, ParameterDirection.Output);

                await _dbConnection.ExecuteScalarAsync<dynamic>("SFCS_Data_PKG_toGetUidBySN", p, commandType: CommandType.StoredProcedure);
                list.Add(p.Get<string>(":MESSAGE").ToString());
                list.Add(p.Get<string>(":TRACE").ToString());
                list.Add(p.Get<string>(":RETURNMSG").ToString());
                return list;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }



        /// <summary>
        /// 獲取new sfcs MB鍵值
        /// </summary>
        /// <param name="mbSerialNumber"></param>
        /// <returns></returns>
        private async Task<decimal?> GetNewSFCSMBKey(string mbSerialNumber)
        {
            try
            {
                var mbRuncadTable = await GetRuncardDataTable(0, mbSerialNumber);
                if (mbRuncadTable != null && mbRuncadTable.Count > 0)
                {
                    return mbRuncadTable[0].ID;
                }
                else
                {
                    var uidRow = await GetCollectUIDDataTable(0, "PSN", mbSerialNumber);
                    if (uidRow.IsNull())
                    {
                        return null;
                    }
                    else
                    {
                        var runcardRow = await GetRuncardDataTable((decimal)uidRow[0].SN_ID, "");
                        if (runcardRow.IsNull())
                        {
                            return null;
                        }
                        else
                            return runcardRow[0].ID;
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// 獲取收集的UID
        /// </summary>
        /// <param name="UID_NAME"></param>
        /// <param name="UID_NUMBER"></param>
        /// <returns></returns>
        private async Task<List<SfcsCollectUidsListModel>> GetCollectUIDDataTable(decimal SN_ID, string UID_NAME, string UID_NUMBER)
        {
            try
            {
                string strwhere = " where 1=1 ";
                if (SN_ID > 0)
                    strwhere += string.Format(" and SN_ID={0}", SN_ID);
                if (!UID_NAME.IsNullOrEmpty())
                    strwhere += string.Format(" and UID_NAME='{0}'", UID_NAME);
                if (!UID_NUMBER.IsNullOrEmpty())
                    strwhere += string.Format(" and UID_NUMBER='{0}'", UID_NUMBER);

                string sql = "SELECT * FROM SFCS_COLLECT_UIDS    " + strwhere;
                var list = await _dbConnection.QueryAsync<SfcsCollectUidsListModel>(sql);
                return list.ToList();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        /// <summary>
        /// 清除UID對象數據
        /// </summary>
        /// <param name="uidTable"></param>
        private async void ClearUID(List<SfcsCollectUidsListModel> uidTable, decimal repairOperationID, string repairer, decimal siteID)
        {
            foreach (var row in uidTable)
            {
                bool addBack = await BackupUIDReworkData(repairOperationID, repairer, siteID, row.COLLECT_UID_ID);
                bool deleteUid = await DeleteUID(row.COLLECT_UID_ID);

                ClearAttachments(repairOperationID, repairer, siteID, row.COLLECT_UID_ID);
            }
        }

        /// <summary>
        /// 備份UID返工數據   BSMT_LOG.SFCS_COLLECT_UIDS 
        /// </summary>
        /// <param name="reworkOperationID"></param>
        /// <param name="reworkOperator"></param>
        /// <param name="siteID"></param>
        /// <param name="collectUIDID"></param>
        /// <returns></returns>
        private async Task<bool> BackupUIDReworkData(decimal reworkOperationID, string reworkOperator,
           decimal siteID, decimal collectUIDID)
        {
            try
            {
                string sql = @"INSERT INTO BSMT_LOG.SFCS_COLLECT_UIDS 
                                                  SELECT SCU.*,SYSDATE,:REWORK_OPERATION_ID,:REWORK_OPERATOR,:OPERATION_SITE
                                                  FROM SFCS_COLLECT_UIDS SCU WHERE SCU.COLLECT_UID_ID=:COLLECT_UID_ID";

                return await _dbConnection.ExecuteAsync(sql, new
                {
                    REWORK_OPERATION_ID = reworkOperationID,
                    REWORK_OPERATOR = reworkOperator,
                    OPERATION_SITE = siteID,
                    COLLECT_UID_ID = collectUIDID,
                }) > 0;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }



        /// <summary>
        /// 備份組件返工數據   BSMT_LOG.SFCS_COLLECT_COMPONENTS
        /// </summary>
        /// <param name="reworkOperationID"></param>
        /// <param name="reworkOperator"></param>
        /// <param name="siteID"></param>
        /// <param name="collectComponentID"></param>
        private async Task<bool> BackupComponentReworkData(decimal reworkOperationID, string reworkOperator,
            decimal siteID, decimal collectComponentID)
        {
            try
            {
                string sql = @"INSERT INTO BSMT_LOG.SFCS_COLLECT_COMPONENTS
                                SELECT SCO.COLLECT_COMPONENT_ID,SCO.OPERATION_ID,SCO.SN_ID,SCO.WO_ID,SCO.PRODUCT_OPERATION_CODE,
                                SCO.COMPONENT_ID,SCO.COMPONENT_NAME,SCO.ODM_COMPONENT_SN,SCO.ODM_COMPONENT_PN,SCO.CUSTOMER_COMPONENT_SN,
                                SCO.CUSTOMER_COMPONENT_PN,SCO.COMPONENT_QTY,SCO.SERIALIZED,SCO.COLLECT_SITE,SCO.COLLECT_TIME,
                                SCO.COLLECT_BY,SCO.REWORK_REMOVE_FLAG,SCO.REPLACE_FLAG,SCO.EDI_FLAG,SCO.ATTRIBUTE1,SCO.ATTRIBUTE2,
                                SCO.ATTRIBUTE3,SCO.ATTRIBUTE4,SCO.ATTRIBUTE5, SYSDATE,:REWORK_OPERATION_ID,:REWORK_OPERATOR,:OPERATION_SITE,SCO.DEVICE_FLAG
                                FROM SFCS_COLLECT_COMPONENTS SCO WHERE COLLECT_COMPONENT_ID=:COLLECT_COMPONENT_ID";

                return await _dbConnection.ExecuteAsync(sql, new
                {
                    REWORK_OPERATION_ID = reworkOperationID,
                    REWORK_OPERATOR = reworkOperator,
                    OPERATION_SITE = siteID,
                    COLLECT_COMPONENT_ID = collectComponentID,
                }) > 0;
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }



        /// <summary>
        /// 備份資源返工數據 BSMT_LOG.SFCS_COLLECT_RESOURCES
        /// </summary>
        /// <param name="reworkOperationID"></param>
        /// <param name="reworkOperator"></param>
        /// <param name="siteID"></param>
        /// <param name="collectComponentID"></param>
        private async Task<bool> BackupResourceReworkData(decimal reworkOperationID, string reworkOperator,
            decimal siteID, decimal collectResourceID)
        {
            try
            {
                string sql = @"INSERT INTO BSMT_LOG.SFCS_COLLECT_RESOURCES
                                                       SELECT SCR.*,SYSDATE,:REWORK_OPERATION_ID,:REWORK_OPERATOR,:OPERATION_SITE
                                                       FROM SFCS_COLLECT_RESOURCES SCR WHERE SCR.COLLECT_RESOURCE_ID=:COLLECT_RESOURCE_ID";

                return await _dbConnection.ExecuteAsync(sql, new
                {
                    REWORK_OPERATION_ID = reworkOperationID,
                    REWORK_OPERATOR = reworkOperator,
                    OPERATION_SITE = siteID,
                    COLLECT_RESOURCE_ID = collectResourceID,
                }) > 0;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// 刪除Components
        /// </summary>
        /// <param name="COLLECT_COMPONENT_ID"></param>
        /// <returns></returns>
        private async Task<bool> DeleteComponent(decimal COLLECT_COMPONENT_ID)
        {
            try
            {
                string sql = @"DELETE FROM SFCS_COLLECT_COMPONENTS  WHERE COLLECT_COMPONENT_ID=:COLLECT_COMPONENT_ID";
                return await _dbConnection.ExecuteAsync(sql, new { COLLECT_COMPONENT_ID }) > 0;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        /// <summary>
        /// 刪除資源
        /// </summary>
        /// <param name="COLLECT_RESOURCE_ID"></param>
        /// <returns></returns>
        private async Task<bool> DeleteResource(decimal COLLECT_RESOURCE_ID)
        {
            try
            {
                string sql = @"DELETE FROM SFCS_COLLECT_RESOURCES  WHERE COLLECT_RESOURCE_ID=:COLLECT_COMPONENT_ID";
                return await _dbConnection.ExecuteAsync(sql, new { COLLECT_RESOURCE_ID }) > 0;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// 刪除資源
        /// </summary>
        /// <param name="COLLECT_UID_ID"></param>
        /// <returns></returns>
        private async Task<bool> DeleteUID(decimal COLLECT_UID_ID)
        {
            try
            {
                string sql = @"DELETE FROM SFCS_COLLECT_UIDS  WHERE COLLECT_UID_ID=:COLLECT_UID_ID";
                return await _dbConnection.ExecuteAsync(sql, new { COLLECT_UID_ID }) > 0;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }



        /// <summary>
        /// 返工時, 更新測試零件狀態為可用狀態,  --測試次數減1
        /// </summary>
        /// <param name="status"></param>
        /// <param name="componentSN"></param>
        /// <returns></returns>
        private async Task<bool> UpdateDeviceComponentWhenRework(decimal status, string componentSN)
        {
            try
            {
                string sql = @"UPDATE SFCS_DEVICE_COMPONENTS SET STATUS = :STATUS, UPDATE_TIME = SYSDATE  WHERE COMPONENT_SN = :COMPONENT_SN";
                return await _dbConnection.ExecuteAsync(sql, new { STATUS = status, COMPONENT_SN = componentSN }) > 0;
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }

        /// <summary>
        /// 清除附件對象數據
        /// </summary>
        private async void ClearAttachments(decimal reworkOperationID, string reworkOperator,
            decimal siteID, decimal collectObjectID)
        {
            bool addBack = await BackupAttachmentReworkData(reworkOperationID, reworkOperator, siteID, collectObjectID);

            bool delete = await DeleteAttachment(collectObjectID);
        }




        /// <summary>
        /// 備份附件數據 BSMT_LOG.SFCS_COLLECT_ATTACHMENTS 
        /// </summary>
        /// <param name="reworkOperationID"></param>
        /// <param name="reworkOperator"></param>
        /// <param name="siteID"></param>
        /// <param name="collectAttachmentID"></param>
        public async Task<bool> BackupAttachmentReworkData(decimal reworkOperationID, string reworkOperator,
            decimal siteID, decimal collectObjectID)
        {
            try
            {
                string sql = @"INSERT INTO BSMT_LOG.SFCS_COLLECT_ATTACHMENTS 
                                                         SELECT SCA.*, SYSDATE,:REWORK_OPERATION_ID,:REWORK_OPERATOR,:OPERATION_SITE FROM SFCS_COLLECT_ATTACHMENTS SCA
                                                         WHERE COLLECT_OBJECT_ID=:COLLECT_OBJECT_ID";
                return await _dbConnection.ExecuteAsync(sql, new
                {
                    REWORK_OPERATION_ID = reworkOperationID,
                    REWORK_OPERATOR = reworkOperator,
                    OPERATION_SITE = siteID,
                    COLLECT_OBJECT_ID = collectObjectID
                }) > 0;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }



        /// <summary>
        /// 刪除Components
        /// </summary>
        /// <param name="COLLECT_OBJECT_ID"></param>
        /// <returns></returns>
        private async Task<bool> DeleteAttachment(decimal COLLECT_OBJECT_ID)
        {
            try
            {
                string sql = @"DELETE FROM SFCS_COLLECT_ATTACHMENTS  WHERE COLLECT_OBJECT_ID=:COLLECT_OBJECT_ID";
                return await _dbConnection.ExecuteAsync(sql, new { COLLECT_OBJECT_ID }) > 0;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// Record Operation History
        /// </summary>
        private async void RecordOperationHistory(SfcsCollectDefectsModel model, SfcsRuncardListModel _runcardRow, decimal repairOperationID)
        {
            //CS端这个集合没看到有使用，先注释
            //var siteRow = await GetOperationSiteDataTable((decimal)_runcardRow.CURRENT_SITE,0,0);

            bool addOperation = await InsertOperationHistory(repairOperationID, (decimal)_runcardRow.WO_ID, _runcardRow.ID, (decimal)_runcardRow.ROUTE_ID, (decimal)model.OPERATION_ID, (decimal)model.REPAIRSITEROW_ID, model.REPAIRER, GlobalVariables.Repaired, null);

        }
        /// <summary>
        /// 生成作業記錄
        /// </summary>
        /// <param name="operationID"></param>
        /// <param name="woID"></param>
        /// <param name="snID"></param>
        /// <param name="routeID"></param>
        /// <param name="operationSiteID"></param>
        /// <param name="user"></param>
        /// <param name="status"></param>
        /// <param name="visitNumber"></param>
        private async Task<bool> InsertOperationHistory(decimal operationID, decimal woID, decimal snID,
            decimal routeID, decimal siteOperationID, decimal operationSiteID,
            string user, decimal status, decimal? visitNumber)
        {
            try
            {
                if (visitNumber.IsNull())
                {
                    visitNumber = await GetVisitNumber(snID);
                }
                string sql = @"INSERT INTO SFCS_OPERATION_HISTORY(SN_ID,OPERATION_ID,WO_ID,ROUTE_ID,SITE_OPERATION_ID,OPERATION_SITE_ID,
                                                         OPERATOR,OPERATION_STATUS,OPERATION_TIME,VISIT_NUMBER) VALUES(:SN_ID,:OPERATION_ID,:WO_ID,
                                                         :ROUTE_ID,:SITE_OPERATION_ID,:OPERATION_SITE_ID,:OPERATOR,:OPERATION_STATUS,SYSDATE,:VISIT_NUMBER)";
                return await _dbConnection.ExecuteAsync(sql, new
                {
                    OPERATION_ID = operationID,
                    WO_ID = woID,
                    SN_ID = snID,
                    ROUTE_ID = routeID,
                    SITE_OPERATION_ID = siteOperationID,
                    OPERATION_SITE_ID = operationSiteID,
                    OPERATOR = user,
                    OPERATION_STATUS = status,
                    VISIT_NUMBER = visitNumber
                }) > 0;
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }
        /// <summary>
        /// 获取最大的VISIT_NUMBER
        /// </summary>
        /// <param name="snID"></param>
        /// <returns></returns>
        private async Task<decimal> GetVisitNumber(decimal snID)
        {
            try
            {
                string sql = @"SELECT MAX(VISIT_NUMBER) FROM SFCS_OPERATION_HISTORY WHERE SN_ID=:SN_ID";
                var result = await _dbConnection.ExecuteScalarAsync(sql, new { SN_ID = snID });
                return (decimal)result;
            }
            catch (Exception ex)
            {
                throw;
            }
        }


        /// <summary>
        /// 通过路径获取零件
        /// </summary>
        /// <param name="snID"></param>
        /// <param name="compName"></param>
        /// <param name="odmPN"></param>
        /// <param name="customerPN"></param>
        /// <param name="routeID"></param>
        /// <param name="orderNo"></param>
        /// <returns></returns>
        private async Task<List<SfcsCollectComponentsListModel>> GetCollectComponentsByRoute(decimal snID, string compName, string odmPN, string customerPN, decimal routeID, decimal orderNo)
        {
            try
            {
                string sql = @"SELECT * FROM SFCS_COLLECT_COMPONENTS WHERE SN_ID=:snID AND COMPONENT_NAME=:compName
                                                       AND ODM_COMPONENT_PN=:odmPN AND CUSTOMER_COMPONENT_PN=:customerPN
                                                       AND PRODUCT_OPERATION_CODE IN ( SELECT DISTINCT PRODUCT_OPERATION_CODE FROM SFCS_ROUTE_CONFIG 
                                                       WHERE ROUTE_ID=:routeID AND ORDER_NO>=:orderNo )";
                var list = await _dbConnection.QueryAsync<SfcsCollectComponentsListModel>(sql, new { snID, compName, odmPN, customerPN, routeID, orderNo });
                return list.ToList();
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }

        /// <summary>
        /// 根據製程获取数据
        /// </summary>
        /// <param name="snID"></param>
        /// <param name="resourceName"></param>
        /// <param name="routeID"></param>
        /// <param name="orderNo"></param>
        /// <returns></returns>
        private async Task<List<SfcsCollectResourcesListModel>> GetCollectResourceTableByRoute(decimal snID, string resourceName, decimal routeID, decimal orderNo)
        {
            try
            {
                string sql = @"SELECT * FROM SFCS_COLLECT_RESOURCES
                                WHERE SN_ID=:snID AND RESOURCE_NAME=:resourceName
                                AND PRODUCT_OPERATION_CODE IN  ( SELECT DISTINCT PRODUCT_OPERATION_CODE
                                FROM SFCS_ROUTE_CONFIG  WHERE ROUTE_ID=:routeID AND ORDER_NO>=:orderNo )";
                var list = await _dbConnection.QueryAsync<SfcsCollectResourcesListModel>(sql, new
                {
                    snID,
                    resourceName,
                    routeID,
                    orderNo
                });
                return list.ToList();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// 通过路径获取uid
        /// </summary>
        /// <param name="snID"></param>
        /// <param name="uidName"></param>
        /// <param name="routeID"></param>
        /// <param name="orderNo"></param>
        /// <returns></returns>
        private async Task<List<SfcsCollectUidsListModel>> GetCollectUIDByRoute(decimal snID, string uidName, decimal routeID, decimal orderNo)
        {
            try
            {
                string sql = @"SELECT * FROM SFCS_COLLECT_UIDS WHERE SN_ID=:SN_ID AND UID_NAME=:UID_NAME 
                                                       AND PRODUCT_OPERATION_CODE IN ( SELECT DISTINCT PRODUCT_OPERATION_CODE FROM SFCS_ROUTE_CONFIG 
                                                       WHERE ROUTE_ID=:ROUTE_ID AND ORDER_NO>=:ORDER_NO )";
                var list = await _dbConnection.QueryAsync<SfcsCollectUidsListModel>(sql, new
                {
                    SN_ID = snID,
                    UID_NAME = uidName,
                    ROUTE_ID = routeID,
                    ORDER_NO = orderNo
                });
                return list.ToList();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        /// <summary>
        /// 根据SN获取信息
        /// </summary>
        /// <param name="sn"></param>
        /// <returns></returns>
        public async Task<List<string>> GetMultiRuncardList(string sn)
        {
            try
            {
                List<string> snList = new List<string>();
                string sql = @"SELECT RA.SN
							  FROM SFCS_COLLECT_MULTI_RUNCARD MR,
								   SFCS_RUNCARD R,
								   SFCS_COLLECT_MULTI_RUNCARD MRA,
								   SFCS_RUNCARD RA
							 WHERE     MR.SN_ID = R.ID
								   AND R.SN = :sn
								   AND MR.STATUS = 1
								   AND MR.ID = MRA.ID
								   AND MRA.SN_ID = RA.ID";
                // 先按连板关系找关联的SN
                var snTable = await _dbConnection.QueryAsync<string>(sql, new { sn });
                if (snTable != null && snTable.ToList().Count() > 0)
                {
                    foreach (string snRow in snTable.ToList())
                    {
                        snList.Add(snRow);
                    }
                }

                // 按离线连板找关联SN
                if (snList.Count == 0)
                {
                    var offlineSNTable = await GetOfflineMultiRuncard(sn);

                    if (offlineSNTable != null && offlineSNTable.Count > 0)
                    {
                        foreach (string offsn in offlineSNTable)
                        {
                            snList.Add(offsn);
                        }
                    }
                }

                // 都没有找到关联SN，则为单板或已拆板
                if (snList.Count == 0)
                {
                    snList.Add(sn);
                }

                return snList;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// 根据SN获取信息
        /// </summary>
        /// <param name="sn"></param>
        /// <returns></returns>
        private async Task<List<string>> GetOfflineMultiRuncard(string SN)
        {
            try
            {

                string isBreakSql = @"select SCMR.* from  SFCS_COLLECT_MULTI_RUNCARD  SCMR ,SFCS_RUNCARD SR WHERE SCMR.SN_ID = SR.ID AND SR.SN = :SN AND SCMR.STATUS = 2";
                var breakTable = await _dbConnection.QueryAsync<object>(isBreakSql, new { SN });
                if (breakTable != null && breakTable.ToList().Count() > 0)
                {
                    //如果分板后直接返回
                    return null;
                }
                string sql = @"
SELECT DISTINCT SN
  FROM SMT_MULTIPANEL_DETAIL
 WHERE MULT_HEADER_ID IN (SELECT DISTINCT MULT_HEADER_ID
                            FROM SMT_MULTIPANEL_DETAIL
                           WHERE SN = :SN)";

                var list = await _dbConnection.QueryAsync<string>(sql, new { SN });

                return list.ToList();
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }

        /// <summary>
        /// 获取RuncardDataTable
        /// </summary>
        /// <param name="SN"></param>
        /// <returns></returns>
        private async Task<List<SfcsRuncardListModel>> GetRuncardDataTable(decimal ID, string SN)
        {
            try
            {
                string strwhere = " where 1=1 ";
                if (ID > 0)
                    strwhere += string.Format(" and ID={0} ", ID);
                if (!SN.IsNullOrWhiteSpace())
                    strwhere += string.Format(" and SN='{0}'", SN);

                string sql = "SELECT * FROM SFCS_RUNCARD  " + strwhere;
                var list = await _dbConnection.QueryAsync<SfcsRuncardListModel>(sql);
                return list.ToList();
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }
        /// <summary>
        /// 獲取WorkOrderTable
        /// </summary>
        /// <param name="ID"></param>
        /// <returns></returns>
        private async Task<List<SfcsWoListModel>> GetWorkOrderTable(decimal ID)
        {
            try
            {
                string sql = "SELECT * FROM SFCS_WO where   ID=:ID";
                var list = await _dbConnection.QueryAsync<SfcsWoListModel>(sql, new { ID });
                return list.ToList();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// 获取PartNumberDataTable
        /// </summary>
        /// <param name="PART_NO"></param>
        /// <returns></returns>
        private async Task<List<SfcsPnListModel>> GetPartNumberDataTable(string PART_NO)
        {
            try
            {
                string sql = "SELECT * FROM SFCS_PN  where    PART_NO=:PART_NO ";
                var list = await _dbConnection.QueryAsync<SfcsPnListModel>(sql, new { PART_NO });
                return list.ToList();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// 获取ProductFamilyDataTable 
        /// </summary>
        /// <param name="ID"></param>
        /// <returns></returns>
        private async Task<List<SfcsProductFamilyListModel>> GetProductFamilyDataTable(decimal ID)
        {
            try
            {
                string sql = "SELECT * FROM SFCS_PRODUCT_FAMILY  where ENABLED='Y' and  ID=:ID ";
                var list = await _dbConnection.QueryAsync<SfcsProductFamilyListModel>(sql, new { ID });
                return list.ToList();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        /// <summary>
        /// 获取ModelDataTable
        /// </summary>
        /// <param name="ID"></param>
        /// <returns></returns>
        private async Task<List<SfcsModelListModel>> GetModelDataTable(decimal ID)
        {
            try
            {
                string sql = "SELECT * FROM SFCS_MODEL  where ENABLED='Y' and  ID=:ID ";
                var list = await _dbConnection.QueryAsync<SfcsModelListModel>(sql, new { ID });
                return list.ToList();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// 獲取最後Fail歷史記錄
        /// </summary>
        /// <param name="ID"></param>
        /// <returns></returns>
        private async Task<List<SfcsOperationHistoryListModel>> GetLastFailHistory(decimal snID)
        {
            try
            {
                string sql = "SELECT * FROM SFCS_OPERATION_HISTORY WHERE SN_ID=:snID AND OPERATION_STATUS=2 ORDER BY OPERATION_TIME DESC ";
                var list = await _dbConnection.QueryAsync<SfcsOperationHistoryListModel>(sql, new { snID });
                return list.ToList();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        /// <summary>
        /// 獲取站點基礎信息
        /// </summary>
        /// <param name="ID"></param>
        /// <returns></returns>
        private async Task<List<SfcsOperationSitesListModel>> GetOperationSiteDataTable(decimal ID, decimal OPERATION_ID, decimal OPERATION_LINE_ID)
        {
            try
            {
                string strwhere = " where ENABLED='Y' ";
                if (ID > 0)
                    strwhere += string.Format(" and ID={0}", ID);
                if (OPERATION_ID > 0)
                    strwhere += string.Format(" and OPERATION_ID={0} ", OPERATION_ID);
                if (OPERATION_LINE_ID > 0)
                    strwhere += string.Format(" and OPERATION_LINE_ID={0}", OPERATION_LINE_ID);

                string sql = "SELECT * FROM SFCS_OPERATION_SITES  " + strwhere;
                var list = await _dbConnection.QueryAsync<SfcsOperationSitesListModel>(sql);
                return list.ToList();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// 取得維修站ID
        /// </summary>
        /// <param name="serialNumberID"></param>
        /// <param name="repairOperationCode"></param>
        /// <param name="failSiteRow"></param>
        private async Task<List<SfcsOperationSitesListModel>> GetRepairSiteRow(decimal serialNumberID, decimal repairOperationCode, SfcsOperationSitesListModel failSiteRow, string serialNumber)
        {
            try
            {
                List<SfcsOperationSitesListModel> sitelist = null;
                if (failSiteRow != null)
                {
                    sitelist = await GetOperationSiteDataTable(0, repairOperationCode, failSiteRow.OPERATION_LINE_ID);
                }
                if (sitelist == null || sitelist.Count <= 0)
                {
                    sitelist = await GetOperationSiteDataTable(0, repairOperationCode, 0);
                }
                if (sitelist == null || sitelist.Count <= 0)
                {
                    throw new Exception(string.Format("产品流水号{0}找不到适用的维修站点，请确认。", serialNumber));
                }
                return sitelist;
            }
            catch (Exception ex)
            {
                throw ex;
            }


        }

        /// <summary>
        /// 獲取不良信息，分為待維修的和已維修的不良
        /// </summary>
        /// <param name="SN_ID"></param>
        /// <param name="REPAIR_FLAG"></param>
        /// <returns></returns>
        private async Task<List<SfcsCollectDefectsListModel>> GetCollectDefects(decimal SN_ID, string REPAIR_FLAG)
        {
            try
            {
                string sql = "SELECT * FROM SFCS_COLLECT_DEFECTS  where SN_ID=:SN_ID and  REPAIR_FLAG=:REPAIR_FLAG ";
                var list = await _dbConnection.QueryAsync<SfcsCollectDefectsListModel>(sql, new { SN_ID, REPAIR_FLAG });
                return list.ToList();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// 獲取SN除指定采集ID外的待維修的不良信息
        /// </summary>
        /// <param name="SN_ID"></param>
        /// <param name="COLLECT_DEFECT_ID"></param>
        /// <returns></returns>
        private async Task<List<SfcsCollectDefectsListModel>> GetCollectDefectsNotEsistsSN(decimal SN_ID, decimal COLLECT_DEFECT_ID)
        {
            try
            {
                string sql = "SELECT * FROM SFCS_COLLECT_DEFECTS  where SN_ID=:SN_ID and  REPAIR_FLAG='N' and COLLECT_DEFECT_ID<>:COLLECT_DEFECT_ID ";
                var list = await _dbConnection.QueryAsync<SfcsCollectDefectsListModel>(sql, new { SN_ID, COLLECT_DEFECT_ID });
                return list.ToList();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        /// <summary>
        /// 根据不良代码获取不良信息
        /// </summary>
        /// <param name="DEFECT_CODE"></param>
        /// <returns></returns>
        private async Task<List<SfcsDefectConfigListModel>> GetDefectsByCode(string DEFECT_CODE)
        {
            try
            {
                string sql = "SELECT * FROM SFCS_DEFECT_CONFIG  where ENABLED='Y' and  DEFECT_CODE=:DEFECT_CODE ";
                var list = await _dbConnection.QueryAsync<SfcsDefectConfigListModel>(sql, new { DEFECT_CODE });
                return list.ToList();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        /// <summary>
        /// 獲取產品收集不良詳細信息
        /// </summary>
        /// <param name="DETAIL_ID"></param>
        /// <returns></returns>
        private async Task<List<SfcsCollectDefectsDetailListModel>> GetCollectDefectsDetail(decimal DETAIL_ID)
        {
            try
            {
                string sql = "SELECT * FROM SFCS_COLLECT_DEFECTS_DETAIL  where   COLLECT_DEFECT_DETAIL_ID=:DETAIL_ID ";
                var list = await _dbConnection.QueryAsync<SfcsCollectDefectsDetailListModel>(sql, new { DETAIL_ID });
                return list.ToList();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// 獲取維修數據信息
        /// </summary>
        /// <param name="DEFECT_ID"></param>
        /// <returns></returns>
        private async Task<List<SfcsRepairRecipeListModel>> GetRepairRecipeData(decimal DEFECT_ID)
        {
            try
            {
                string sql = "SELECT * FROM SFCS_REPAIR_RECIPE  where   COLLECT_DEFECT_ID=:DEFECT_ID ";
                var list = await _dbConnection.QueryAsync<SfcsRepairRecipeListModel>(sql, new { DEFECT_ID });
                return list.ToList();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// 获取前14不良原因排名
        /// </summary>
        /// <param name="defectCode"></param>
        /// <returns></returns>
        private async Task<List<Top15RootCausesListModel>> GetTop15RootCauses(string DEFECT_CODE)
        {
            try
            {
                string sql = @"SELECT   A.*
                                    FROM   (  SELECT   SCD.DEFECT_CODE, SCR.REASON_CODE, COUNT ( * ) QTY
                                                FROM   SFCS_COLLECT_DEFECTS SCD, SFCS_REPAIR_RECIPE SCR
                                                WHERE   SCD.COLLECT_DEFECT_ID = SCR.COLLECT_DEFECT_ID
                                                        AND SCD.DEFECT_CODE = :DEFECT_CODE
                                            GROUP BY   SCD.DEFECT_CODE, SCR.REASON_CODE
                                            ORDER BY   QTY DESC) A
                                    WHERE    ROWNUM <= 15 ";

                var resdata = await _dbConnection.QueryAsync<Top15RootCausesListModel>(sql, new { DEFECT_CODE });
                return resdata.ToList();
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }


        /// <summary>
        /// 获取制程配置信息
        /// </summary>
        /// <returns></returns>
        private async Task<List<SfcsRouteConfigListModel>> GetRouteConfigDataTable(decimal ROUTE_ID, decimal CURRENT_OPERATION_ID)
        {
            try
            {
                string strwhere = " where 1=1 ";
                if (ROUTE_ID > 0)
                {
                    strwhere += string.Format(" and ROUTE_ID={0}", ROUTE_ID);
                }
                if (CURRENT_OPERATION_ID > 0)
                {
                    strwhere += string.Format(" and CURRENT_OPERATION_ID={0}", CURRENT_OPERATION_ID);
                }

                string sql = "SELECT * FROM SFCS_ROUTE_CONFIG   " + strwhere + " ORDER BY ORDER_NO ";
                var list = await _dbConnection.QueryAsync<SfcsRouteConfigListModel>(sql);
                return list.ToList();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        /// <summary>
        /// 根据ID获取工序信息
        /// </summary>
        /// <param name="ID"></param>
        /// <returns></returns>
        private async Task<List<SfcsOperationsListModel>> GetOperationDataTable(decimal ID)
        {
            try
            {
                string sql = "SELECT * FROM SFCS_OPERATIONS  where   ID=:ID ";
                var list = await _dbConnection.QueryAsync<SfcsOperationsListModel>(sql, new { ID });
                return list.ToList();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// 查找刪除對象
        /// </summary>
        /// <param name="deleteComponent"></param>
        /// <param name="deleteResource"></param>
        /// <param name="deleteUID"></param>
        /// <param name="serialNumber"></param>
        /// <param name="routeID"></param>
        /// <param name="orderNO"></param>
        /// <returns></returns>
        private async Task<List<SfcsCollectObjectsListMode>> FindCollectObjectsInOperation(bool deleteComponent, bool deleteResource, bool deleteUID,
            string serialNumber, decimal routeID, decimal orderNO)
        {
            try
            {
                List<SfcsCollectObjectsListMode> list = new List<SfcsCollectObjectsListMode>();
                if (deleteComponent)
                {
                    list = list.Concat(await GetCollectComponentInOperation(serialNumber, routeID, orderNO)).ToList();
                }
                if (deleteResource)
                {
                    list = list.Concat(await GetCollectResourceInOperation(serialNumber, routeID, orderNO)).ToList();
                }
                if (deleteUID)
                {
                    list = list.Concat(await GetCollectUIDInOperation(serialNumber, routeID, orderNO)).ToList();
                }

                return list;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }




        /// <summary>
        /// 根據選擇的工位順序獲取所有採集的零件對象
        /// </summary>
        /// <param name="INPUT_DATA"></param>
        /// <param name="ROUTE_ID"></param>
        /// <param name="ORDER_NO"></param>
        /// <returns></returns>
        private async Task<List<SfcsCollectObjectsListMode>> GetCollectComponentInOperation(string INPUT_DATA, decimal ROUTE_ID, decimal ORDER_NO)
        {
            try
            {
                string sql = @"SELECT SCC.COMPONENT_NAME OBJECT_NAME, SCC.ODM_COMPONENT_PN ODM_PN, 
                                                       SCC.CUSTOMER_COMPONENT_PN CUSTOMER_PN, COUNT(*) QTY,
                                                       SCC.REWORK_REMOVE_FLAG, 'COMPONENT' OBJECT_KIND FROM SFCS_COLLECT_COMPONENTS SCC 
                                                       WHERE SCC.SN_ID IN ( SELECT SR.ID FROM SFCS_RUNCARD SR WHERE (SN=:INPUT_DATA
                                                       OR CARTON_NO=:INPUT_DATA OR PALLET_NO=:INPUT_DATA) ) AND SCC.PRODUCT_OPERATION_CODE IN (
                                                       SELECT DISTINCT PRODUCT_OPERATION_CODE FROM SFCS_ROUTE_CONFIG 
                                                       WHERE ROUTE_ID=:ROUTE_ID AND ORDER_NO>=:ORDER_NO ) GROUP BY SCC.COMPONENT_NAME, 
                                                       SCC.ODM_COMPONENT_PN, SCC.CUSTOMER_COMPONENT_PN, SCC.REWORK_REMOVE_FLAG ";
                var list = await _dbConnection.QueryAsync<SfcsCollectObjectsListMode>(sql, new { INPUT_DATA, ROUTE_ID, ORDER_NO });
                return list.ToList();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// 根據選擇的工位順序獲取所有採集的資源對象
        /// </summary>
        /// <param name="INPUT_DATA"></param>
        /// <param name="ROUTE_ID"></param>
        /// <param name="ORDER_NO"></param>
        /// <returns></returns>
        private async Task<List<SfcsCollectObjectsListMode>> GetCollectResourceInOperation(string INPUT_DATA, decimal ROUTE_ID, decimal ORDER_NO)
        {
            try
            {
                string sql = @"SELECT SCR.RESOURCE_NAME OBJECT_NAME, '' ODM_PN,'' CUSTOMER_PN, 
                                                        COUNT(*) QTY,SCR.REWORK_REMOVE_FLAG, 
                                                        'RESOURCE' OBJECT_KIND FROM SFCS_COLLECT_RESOURCES SCR
                                                        WHERE SCR.SN_ID IN ( SELECT SR.ID FROM SFCS_RUNCARD SR WHERE (SN=:INPUT_DATA
                                                        OR CARTON_NO=:INPUT_DATA OR PALLET_NO=:INPUT_DATA) ) AND SCR.PRODUCT_OPERATION_CODE IN (
                                                        SELECT DISTINCT PRODUCT_OPERATION_CODE FROM SFCS_ROUTE_CONFIG
                                                        WHERE ROUTE_ID=:ROUTE_ID AND ORDER_NO>=:ORDER_NO ) 
                                                        GROUP BY SCR.RESOURCE_NAME, SCR.REWORK_REMOVE_FLAG ";
                var list = await _dbConnection.QueryAsync<SfcsCollectObjectsListMode>(sql, new { INPUT_DATA, ROUTE_ID, ORDER_NO });
                return list.ToList();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// 根據選擇的工位順序獲取所有採集的零件對象
        /// </summary>
        /// <param name="INPUT_DATA"></param>
        /// <param name="ROUTE_ID"></param>
        /// <param name="ORDER_NO"></param>
        /// <returns></returns>
        private async Task<List<SfcsCollectObjectsListMode>> GetCollectUIDInOperation(string INPUT_DATA, decimal ROUTE_ID, decimal ORDER_NO)
        {
            try
            {
                string sql = @"SELECT SCU.UID_NAME OBJECT_NAME, '' ODM_PN,'' CUSTOMER_PN,   
                                                   COUNT(*) QTY, SCU.REWORK_REMOVE_FLAG,'UID' OBJECT_KIND 
                                                   FROM SFCS_COLLECT_UIDS SCU WHERE SCU.SN_ID IN ( 
                                                   SELECT SR.ID FROM SFCS_RUNCARD SR WHERE (SN=:INPUT_DATA
                                                   OR CARTON_NO=:INPUT_DATA OR PALLET_NO=:INPUT_DATA) )
                                                   AND SCU.PRODUCT_OPERATION_CODE IN (
                                                   SELECT DISTINCT PRODUCT_OPERATION_CODE FROM SFCS_ROUTE_CONFIG 
                                                   WHERE ROUTE_ID=:ROUTE_ID AND ORDER_NO>=:ORDER_NO ) 
                                                   GROUP BY SCU.UID_NAME,SCU.REWORK_REMOVE_FLAG ";
                var list = await _dbConnection.QueryAsync<SfcsCollectObjectsListMode>(sql, new { INPUT_DATA, ROUTE_ID, ORDER_NO });
                return list.ToList();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }



        /// <summary>
        /// 不良代码是否需要维修
        /// </summary>
        /// <param name="COLLECT_DEFECT_ID">采集ID</param>
        /// <returns>T:需要维修 F:不需要维修</returns>
        public async Task<bool> CheckCollectDefectNeedRepair(decimal COLLECT_DEFECT_ID)
        {
            string sql = "select count(0) from SFCS_COLLECT_DEFECTS where COLLECT_DEFECT_ID = :COLLECT_DEFECT_ID and REPAIR_FLAG = 'N'";
            object result = await _dbConnection.ExecuteScalarAsync(sql, new
            {
                COLLECT_DEFECT_ID
            });

            return (Convert.ToInt32(result) > 0);
        }






    }
}