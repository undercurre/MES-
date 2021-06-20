/*
*┌──────────────────────────────────────────────────────────────┐
*│　描    述：接口实现                                                    
*│　作    者：嘉志科技                                            
*│　版    本：2.0    模板代码自动生成                                                
*│　创建时间：2020-03-11 10:19:01                             
*└──────────────────────────────────────────────────────────────┘
*┌──────────────────────────────────────────────────────────────┐
*│　命名空间： JZ.IMS.Repository.Oracle                                  
*│　类    名： SmtFeederRepository                                      
*└──────────────────────────────────────────────────────────────┘
*/
using JZ.IMS.Core.DbHelper;
using JZ.IMS.Core.Options;
using JZ.IMS.Core.Repository;
using JZ.IMS.IRepository;
using JZ.IMS.Models;
using Dapper;
using Microsoft.Extensions.Options;
using System;
using System.Threading.Tasks;
using System.Linq;
using System.Collections.Generic;
using JZ.IMS.ViewModels;
using JZ.IMS.Core.Extensions;

namespace JZ.IMS.Repository.Oracle
{
    public class SmtFeederRepository : BaseRepository<SmtFeeder, Decimal>, ISmtFeederRepository
    {
        public SmtFeederRepository(IOptionsSnapshot<DbOption> options)
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
            string sql = "SELECT ENABLED FROM SMT_FEEDER WHERE ID=:ID";
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
            string sql = "UPDATE SMT_FEEDER set ENABLED=:ENABLED WHERE ID=:Id";
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
            string sql = "SELECT SMT_FEEDER_SEQ.NEXTVAL MY_SEQ FROM DUAL";
            var result = await _dbConnection.ExecuteScalarAsync(sql);
            return (decimal)result;
        }

        /// <summary>
        ///获取状态类型列表 
        /// </summary>
        /// <param name="id">项目id</param>
        /// <returns></returns>
        public async Task<List<IDNAME>> GetStatus()
        {
            string sql = "select CODE as ID ,CN_DESC as Name from SMT_LOOKUP where TYPE = 'SMT_FEEDER_STATUS'";
            var result = await _dbConnection.QueryAsync<IDNAME>(sql);
            return result?.ToList();
        }

        /// <summary>
        /// 获取飞达类型集
        /// </summary>
        /// <returns></returns>
        public async Task<List<CodeName>> GetFeederTypeList()
        {
            List<CodeName> result = null;

            string sql = @"SELECT TO_NUMBER(CODE) AS CODE, VALUE as NAME  
                            FROM SMT_LOOKUP
                            WHERE TYPE = 'FEEDER_TYPE_NAME'
	                            AND ENABLED = 'Y'";
            var tmpdata = await _dbConnection.QueryAsync<CodeName>(sql);

            if (tmpdata != null)
            {
                result = tmpdata.ToList();
            }
            return result;
        }

        /// <summary>
        ///料架编号FEEDER 判断是否存在
        /// </summary>
        /// <param name="FEEDER">料架编号</param>
        /// <returns></returns>
        public async Task<SmtFeeder> ItemIsByFeeder(string FEEDER, decimal ID = 0)
        {
            string sql = "SELECT * FROM SMT_FEEDER WHERE FEEDER =:FEEDER  ";
            string condition = "";
            var sqlparams = new object();
            if (ID > 0)
            {
                condition += " AND ID!=:ID ";
                sql += condition;
                sqlparams = new { FEEDER = FEEDER, ID = ID };
            }
            else
            {
                sqlparams = new { FEEDER = FEEDER };
            }

            var result = await _dbConnection.QueryAsync<SmtFeeder>(sql, sqlparams);
            return result?.FirstOrDefault();
        }

        /// <summary>
		/// 查询列表
		/// </summary>
		/// <param name="model"></param>
		/// <returns></returns>
		public async Task<TableDataModel> LoadData(SmtFeederRequestModel model)
        {
            string conditions = " WHERE m.ID > 0 ";
            if (!model.FEEDER.IsNullOrWhiteSpace())
            {
                conditions += $" AND INSTR(m.FEEDER, :FEEDER) > 0 ";
            }
            if (!model.SUPPLIER.IsNullOrEmpty())
            {
                conditions += $" AND INSTR(m.SUPPLIER, :SUPPLIER) > 0 ";
            }
            if (!model.FTYPE.IsNullOrWhiteSpace())
            {
                conditions += $" AND INSTR(m.FTYPE, :FTYPE) > 0 ";
            }
            if (!model.FBODYMARK.IsNullOrWhiteSpace())
            {
                conditions += $" AND INSTR(m.FBODYMARK, :FBODYMARK) > 0 ";
            }
            if (!model.FSIZE.IsNullOrWhiteSpace())
            {
                conditions += $" AND INSTR(m.FSIZE, :FSIZE) > 0 ";
            }
            if (model.STATUS > 0)
            {
                conditions += $" AND m.Status=:Status ";
            }
            if (!model.DESCRIPTION.IsNullOrWhiteSpace())
            {
                conditions += $" AND INSTR(m.DESCRIPTION, :DESCRIPTION) > 0 ";
            }
            
            string sql = @"SELECT ROWNUM AS ROWNO, M.ID, M.FEEDER, M.SUPPLIER, M.FTYPE, M.FSIZE, M.FBODYMARK, M.STATUS, M.CHECK_USED_COUNT, M.EMEND_USED_COUNT, 
                             M.TOTAL_USED_COUNT, M.LAST_CHECK_TIME, M.LAST_EMEND_TIME, M.LAST_REPAIR_TIME, M.DESCRIPTION, M.CREATE_BY, M.CREATE_TIME, M.UPDATE_BY, 
                             M.UPDATE_TIME, M.ORGANIZE_ID, OZ.ORGANIZE_NAME  
				           FROM SMT_FEEDER M INNER JOIN (SELECT DISTINCT T.* FROM SYS_ORGANIZE T START WITH T.ID IN (SELECT ORGANIZE_ID FROM 
                             SYS_USER_ORGANIZE WHERE MANAGER_ID=:USER_ID) CONNECT BY PRIOR T.ID=T.PARENT_ORGANIZE_ID) OZ ON M.ORGANIZE_ID = OZ.ID ";
            string pagedSql = SQLBuilderClass.GetPagedSQL(sql, "m.id", conditions);
            var resdata = await _dbConnection.QueryAsync<SmtFeederListModel>(pagedSql, model);

            string sqlcnt = @" SELECT COUNT(0) FROM SMT_FEEDER M INNER JOIN (SELECT DISTINCT T.* FROM SYS_ORGANIZE T START WITH T.ID IN (SELECT ORGANIZE_ID FROM 
                             SYS_USER_ORGANIZE WHERE MANAGER_ID=:USER_ID) CONNECT BY PRIOR T.ID=T.PARENT_ORGANIZE_ID) OZ ON M.ORGANIZE_ID = OZ.ID " + conditions;
            int cnt = await _dbConnection.ExecuteScalarAsync<int>(sqlcnt, model);
            return new TableDataModel
            {
                count = cnt,
                data = resdata?.ToList(),
            };
        }

        /// <summary>
        /// 导出分页分页
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public async Task<TableDataModel> GetExportData(SmtFeederRequestModel model)
        {
            string conditions = " WHERE SFR.ID > 0 ";

            if (!model.FEEDER.IsNullOrWhiteSpace())
            {
                conditions += $" AND INSTR(SFR.FEEDER, :FEEDER) > 0  ";
            }
            if (!model.SUPPLIER.IsNullOrEmpty())
            {
                conditions += $" AND INSTR(SFR.SUPPLIER, :SUPPLIER) > 0  ";
            }
            if (!model.FTYPE.IsNullOrWhiteSpace())
            {
                conditions += $" AND INSTR(SFR.FTYPE, :FTYPE) > 0 ";
            }
            if (!model.FBODYMARK.IsNullOrWhiteSpace())
            {
                conditions += $" AND INSTR(SFR.FBODYMARK, :FBODYMARK) > 0 ";
            }
            if (!model.FSIZE.IsNullOrWhiteSpace())
            {
                conditions += $" AND INSTR(SFR.FSIZE, :FSIZE) > 0 ";
            }
            if (model.STATUS > 0)
            {
                conditions += $" AND INSTR(SFR.STATUS, :STATUS) > 0 ";
            }
            if (!model.DESCRIPTION.IsNullOrWhiteSpace())
            {
                conditions += $" AND INSTR(SFR.DESCRIPTION, :DESCRIPTION) > 0 ";
            }
            if (model.STATUS > 0)
            {
                conditions += $" AND SFR.Status=:Status ";
            }

            string sql = @"SELECT ROWNUM AS ROWNO, SFR.ID,SFR.SUPPLIER, SFR.FEEDER,SFR.FTYPE,SFR.FSIZE,SFR.FBODYMARK,LP.CN_DESC STATUS,SFR.DESCRIPTION,OZ.ORGANIZE_NAME  ORGANIZE_ID 
                           FROM SMT_FEEDER SFR inner join (select distinct t.* from SYS_ORGANIZE t start with t.id in (select organize_id from 
                             sys_user_organize where manager_id=:USER_ID) connect by prior t.id=t.PARENT_ORGANIZE_ID) oz on SFR.organize_id = oz.ID
                           LEFT JOIN SMT_LOOKUP LP ON SFR.STATUS=LP.CODE AND LP.TYPE = 'SMT_FEEDER_STATUS' ";
            string pagedSql = SQLBuilderClass.GetPagedSQL(sql, "SFR.ID DESC", conditions);
            var resdata = await _dbConnection.QueryAsync<object>(pagedSql, model);
            string sqlcnt = @"SELECT COUNT(SFR.ID) FROM SMT_FEEDER SFR inner join (select distinct t.* from SYS_ORGANIZE t start with t.id in (select organize_id from 
                                sys_user_organize where manager_id=:USER_ID) connect by prior t.id=t.PARENT_ORGANIZE_ID) oz on SFR.organize_id = oz.ID 
                              LEFT JOIN SMT_LOOKUP LP ON SFR.STATUS=LP.CODE AND LP.TYPE = 'SMT_FEEDER_STATUS' " + conditions;

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
        public async Task<decimal> SaveDataByTrans(SmtFeederModel model)
        {
            int result = 1;
            ConnectionFactory.OpenConnection(_dbConnection);
            using (var tran = _dbConnection.BeginTransaction())
            {
                try
                {
                    //新增
                    string insertSql = @"INSERT INTO SMT_FEEDER(ID, FEEDER, SUPPLIER, FTYPE, FSIZE, FBODYMARK, STATUS , DESCRIPTION, CREATE_BY, CREATE_TIME, 
                        UPDATE_BY, UPDATE_TIME, ORGANIZE_ID)
                    VALUES(:ID, :FEEDER, :SUPPLIER, :FTYPE, :FSIZE, :FBODYMARK, :STATUS, :DESCRIPTION, :CREATE_BY, SYSDATE, :UPDATE_BY, SYSDATE,:ORGANIZE_ID)
";
                    if (model.insertRecords != null && model.insertRecords.Count > 0)
                    {
                        foreach (var item in model.insertRecords)
                        {
                            var newid = await Get_MES_SEQ_ID();
                            var resdata = await _dbConnection.ExecuteAsync(insertSql, new
                            {
                                ID = newid,
                                item.FEEDER,
                                item.SUPPLIER,
                                item.FTYPE,
                                item.FSIZE,
                                item.FBODYMARK,
                                item.STATUS,
                                item.DESCRIPTION,
                                item.CREATE_BY,
                                item.UPDATE_BY,
                                item.ORGANIZE_ID,
                            }, tran);
                        }
                    }

                    //更新
                    string updateSql = @"UPDATE SMT_FEEDER SET 
                                               FEEDER = :FEEDER,
                                               SUPPLIER = :SUPPLIER,
                                               FTYPE = :FTYPE,
                                               FSIZE = :FSIZE,
                                               FBODYMARK = :FBODYMARK,
                                               STATUS = :STATUS,
                                               DESCRIPTION = :DESCRIPTION,
                                               CREATE_BY = :CREATE_BY,
                                               UPDATE_BY = :UPDATE_BY,
                                               UPDATE_TIME = SYSDATE,
                                               ORGANIZE_ID = :ORGANIZE_ID
                                         WHERE ID = :ID";
                    if (model.updateRecords != null && model.updateRecords.Count > 0)
                    {
                        foreach (var item in model.updateRecords)
                        {
                            var resdata = await _dbConnection.ExecuteAsync(updateSql, new
                            {
                                item.ID,
                                item.FEEDER,
                                item.SUPPLIER,
                                item.FTYPE,
                                item.FSIZE,
                                item.FBODYMARK,
                                item.STATUS,
                                item.DESCRIPTION,
                                item.CREATE_BY,
                                item.UPDATE_BY,
                                item.ORGANIZE_ID,
                            }, tran);
                        }
                    }
                        
                    //删除
                    string deleteSql = @"Delete from smt_feeder where ID=:ID ";
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
        /// 保存数据
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public async Task<decimal> SaveUpdateByTrans(SmtFeederModel model)
        {
            int result = 1;
            ConnectionFactory.OpenConnection(_dbConnection);
            using (var tran = _dbConnection.BeginTransaction())
            {
                try
                {
                    //更新
                    string updateSql = @"UPDATE SMT_FEEDER SET 
                                               FEEDER = :FEEDER
                                         WHERE ID = :ID";
                    if (model.updateRecords != null && model.updateRecords.Count > 0)
                    {
                        foreach (var item in model.updateRecords)
                        {
                            var resdata = await _dbConnection.ExecuteAsync(updateSql, new
                            {
                                item.ID,
                                item.FEEDER
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
        /// 保存PDA飞达盘点数据
        /// </summary>
        /// <param name="model"></param>
        /// <param name="feeder"></param>
        /// <param name="head"></param>
        /// <returns></returns>
        public async Task<int> SavePDAFeederCheckData(SaveFeederCheckDataRequestModel model, SmtFeeder feeder, SfcsFeederKeepHead head, SfcsFeederKeepDetail detail, List<GetFeederInfoListModel> fList)
        {
            int result = 0;
            int detailId = 0, headId = 0;
            ConnectionFactory.OpenConnection(_dbConnection);
            using (var tran = _dbConnection.BeginTransaction())
            {
                try
                {
                    if (model.CHECK_CODE.IsNullOrEmpty() && head.IsNullOrWhiteSpace() && detail.IsNullOrWhiteSpace())
                    {
                        if (fList.IsNullOrWhiteSpace()) { throw new Exception("FEEDER_CHECK_QTY_ERROR"); }

                        headId = QueryEx<int>("SELECT SFCS_FEEDER_KEEP_HEAD_SEQ.NEXTVAL MY_SEQ FROM DUAL").FirstOrDefault();

                        //將序列轉成36進制表示
                        string resultStr = Core.Utilities.RadixConvertPublic.RadixConvert(headId.ToString(), ViewModels.GlobalVariables.DecRadix, ViewModels.GlobalVariables.Base36Redix);

                        //六位表示
                        string ReleasedSequence = resultStr.PadLeft(6, '0');
                        string yymmdd = QueryEx<string>("SELECT TO_CHAR(SYSDATE,'YYMMDD') YYMMDD FROM DUAL ").FirstOrDefault();
                        String FeederNo = "FDDJ" + yymmdd + ReleasedSequence;//飞达点检编号

                        ////点检月/次
                        //int count = QueryEx<int>("SELECT COUNT(1) FROM SFCS_FEEDER_KEEP_HEAD WHERE TO_CHAR(CHECK_START_TIME,'yyyy-MM') = :CHECK_TIME ", new
                        //{
                        //    CHECK_TIME = DateTime.Now.ToString("yyyy-MM")
                        //}).FirstOrDefault();
                        //count += 1;

                        String insertHeadSql = @"INSERT INTO SFCS_FEEDER_KEEP_HEAD(ID, CHECK_CODE, CHECK_STATUS, CHECK_COUNT, CHECK_USER, CHECK_START_TIME, CHECK_REMARKS, ORGANIZE_ID) VALUES(:ID, :CHECK_CODE, 0, (SELECT COUNT(0) + 1 AS C  FROM SFCS_FEEDER_KEEP_HEAD WHERE TO_CHAR(CHECK_START_TIME,'yyyy-MM') = TO_CHAR(SYSDATE,'yyyy-MM')), :CHECK_USER, SYSDATE, :CHECK_REMARKS,:ORGANIZE_ID)";
                        result = await _dbConnection.ExecuteAsync(insertHeadSql, new
                        {
                            ID = headId,
                            CHECK_CODE = FeederNo,
                            CHECK_USER = model.CHECK_USER,
                            CHECK_REMARKS = model.CHECK_REMARKS,
                            ORGANIZE_ID = feeder.ORGANIZE_ID
                        }, tran);
                        if (result <= 0) { throw new Exception("DATA_ERROR"); }

                        String insertDetailSql = @"INSERT INTO SFCS_FEEDER_KEEP_DETAIL (ID, KEEP_HEAD_ID, FEEDER_TYPE, FEEDER_SIZE, FEEDER_TYPE_TOTAL, CHECK_QTY) VALUES (:ID, :KEEP_HEAD_ID, :FEEDER_TYPE, :FEEDER_SIZE,:FEEDER_TYPE_TOTAL, :CHECK_QTY)";
                        foreach (var item in fList)
                        {
                            if (item.FEEDER_TYPE == feeder.FTYPE && item.FEEDER_SIZE == feeder.FSIZE)
                            {
                                detailId = QueryEx<int>("SELECT SFCS_FEEDER_KEEP_DETAIL_SEQ.NEXTVAL MY_SEQ FROM DUAL").FirstOrDefault();
                                result = await _dbConnection.ExecuteAsync(insertDetailSql, new
                                {
                                    ID = detailId,
                                    KEEP_HEAD_ID = headId,
                                    FEEDER_TYPE = feeder.FTYPE,
                                    FEEDER_SIZE = feeder.FSIZE,
                                    FEEDER_TYPE_TOTAL = item.FEEDER_QTY,
                                    CHECK_QTY = 1
                                }, tran);
                            }
                            else
                            {
                                int newid = QueryEx<int>("SELECT SFCS_FEEDER_KEEP_DETAIL_SEQ.NEXTVAL MY_SEQ FROM DUAL").FirstOrDefault();
                                result = await _dbConnection.ExecuteAsync(insertDetailSql, new
                                {
                                    ID = newid,
                                    KEEP_HEAD_ID = headId,
                                    FEEDER_TYPE = item.FEEDER_TYPE,
                                    FEEDER_SIZE = item.FEEDER_SIZE,
                                    FEEDER_TYPE_TOTAL = item.FEEDER_QTY,
                                    CHECK_QTY = 0
                                }, tran);
                            }
                            if (result <= 0) { throw new Exception("DATA_ERROR"); }
                        }
                        ////获取同类型和尺寸并可用状态的飞达数量
                        //int feeder_qty = QueryEx<int>("SELECT COUNT(1) FROM SMT_FEEDER WHERE STATUS NOT IN (6,7) AND FTYPE = :FTYPE AND FSIZE = :FSIZE AND ORGANIZE_ID = :ORGANIZE_ID ", new
                        //{
                        //    FTYPE = feeder.FTYPE,
                        //    FSIZE = feeder.FSIZE,
                        //    ORGANIZE_ID = feeder.ORGANIZE_ID
                        //}).FirstOrDefault();
                        //if (feeder_qty <= 0) { throw new Exception("FEEDER_CHECK_QTY_ERROR"); }

                        //detailId = QueryEx<int>("SELECT SFCS_FEEDER_KEEP_DETAIL_SEQ.NEXTVAL MY_SEQ FROM DUAL").FirstOrDefault();
                        //result = await _dbConnection.ExecuteAsync(insertDetailSql, new
                        //{
                        //    ID = detailId,
                        //    KEEP_HEAD_ID = headId,
                        //    FEEDER_TYPE = feeder.FTYPE,
                        //    FEEDER_SIZE = feeder.FSIZE,
                        //    FEEDER_TYPE_TOTAL = feeder_qty
                        //}, tran);
                        //if (result <= 0) { throw new Exception("DATA_ERROR"); }
                    }
                    else
                    {
                        detailId = detail.ID;
                        String updatedetailSql = @"UPDATE SFCS_FEEDER_KEEP_DETAIL SET CHECK_QTY = CHECK_QTY + 1 WHERE ID = :ID ";
                        result = await _dbConnection.ExecuteAsync(updatedetailSql, new
                        {
                            ID = detail.ID,
                        }, tran);
                        if (result <= 0) { throw new Exception("DATA_ERROR"); }

                    }

                    int contentId = QueryEx<int>("SELECT SFCS_FEEDER_KEEP_CONTENT_SEQ.NEXTVAL MY_SEQ FROM DUAL").FirstOrDefault();
                    String insertContentSql = @"INSERT INTO SFCS_FEEDER_KEEP_CONTENT (ID, KEEP_DETAIL_ID, FEEDER_ID, FEEDER_STATUS, CHECK_USER, CHECK_TIME, CHECK_REMARKS) VALUES (:ID, :KEEP_DETAIL_ID, :FEEDER_ID, :FEEDER_STATUS, :CHECK_USER,SYSDATE,:CHECK_REMARKS)";
                    result = await _dbConnection.ExecuteAsync(insertContentSql, new
                    {
                        ID = contentId,
                        KEEP_DETAIL_ID = detailId,
                        FEEDER_ID = feeder.ID,
                        FEEDER_STATUS = feeder.STATUS,
                        CHECK_REMARKS = model.CHECK_REMARKS,
                        CHECK_USER = model.CHECK_USER
                    }, tran);
                    if (result <= 0) throw new Exception("DATA_ERROR");

                    tran.Commit();
                }
                catch (Exception ex)
                {
                    headId = 0;
                    detailId = -1;
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
            return headId;
        }

        /// <summary>
        /// 删除PDA飞达盘点数据记录
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<bool> DeletePDAFeederCheckData(Decimal id)
        {
            int result = 0;
            ConnectionFactory.OpenConnection(_dbConnection);
            using (var tran = _dbConnection.BeginTransaction())
            {
                try
                {
                    //飞达点检记录详细内容表
                    String deleteSql = @"DELETE FROM SFCS_FEEDER_KEEP_CONTENT WHERE KEEP_DETAIL_ID IN (SELECT ID FROM SFCS_FEEDER_KEEP_DETAIL WHERE KEEP_HEAD_ID = :ID ) ";
                    result = await _dbConnection.ExecuteAsync(deleteSql, new { ID = id }, tran);

                    //飞达点检记录详细表
                    deleteSql = @"DELETE FROM SFCS_FEEDER_KEEP_DETAIL WHERE KEEP_HEAD_ID = :ID ";
                    result = await _dbConnection.ExecuteAsync(deleteSql, new { ID = id }, tran);

                    //飞达点检记录主表
                    deleteSql = @"DELETE FROM SFCS_FEEDER_KEEP_HEAD WHERE ID = :ID ";
                    result = await _dbConnection.ExecuteAsync(deleteSql, new { ID = id }, tran);
                    if (result <= 0) throw new Exception("DATA_ERROR");

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
            return result > 0 ? true : false;
        }

        /// <summary>
        /// 确认PDA飞达盘点数据
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public async Task<bool> ConfirmPDAFeederCheckData(AuditFeederCheckDataRequestModel model)
        {
            int result = 0;
            ConnectionFactory.OpenConnection(_dbConnection);
            using (var tran = _dbConnection.BeginTransaction())
            {
                try
                {
                    if (model.STATUS == 1)
                    {
                        String updateHeadSql = @"UPDATE SFCS_FEEDER_KEEP_HEAD SET CHECK_STATUS = '1',CHECK_END_TIME = SYSDATE WHERE ID = :ID ";
                        result = await _dbConnection.ExecuteAsync(updateHeadSql, new
                        {
                            ID = model.ID
                        }, tran);
                        if (result <= 0) throw new Exception("DATA_ERROR");
                    }
                    if (model.STATUS == 2)
                    {
                        String updateHeadSql = @"UPDATE SFCS_FEEDER_KEEP_HEAD SET CHECK_STATUS = '2',AUDIT_USER = :AUDIT_USER,AUDIT_REMARKS = :AUDIT_REMARKS,AUDIT_TIME = SYSDATE WHERE ID = :ID ";
                        result = await _dbConnection.ExecuteAsync(updateHeadSql, new
                        {
                            AUDIT_USER = model.AUDIT_USER,
                            AUDIT_REMARKS = model.AUDIT_REMARKS,
                            ID = model.ID
                        }, tran);
                        if (result <= 0) throw new Exception("DATA_ERROR");
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
            return result > 0 ? true : false;
        }

        public async Task<List<FeederCheckListModel>> LoadPDAFeederCheckList(FeederCheckRequestModel model)
        {
            FeederCheckModel fModel = new FeederCheckModel();
            int page = 0, limit = 0;
            page = model.Page * model.Limit - model.Limit + 1;
            limit = model.Page * model.Limit;
            fModel.Page = page;
            fModel.Limit = limit;

            String orderBy = " ORDER BY CHECK_START_TIME DESC ";
            String whereStr = "";

            if (!model.START_TIME.IsNullOrEmpty())
            {
                fModel.START_TIME = Convert.ToDateTime(model.START_TIME.Trim() + " 00:00:00");
                whereStr += " AND CHECK_START_TIME >= :START_TIME ";

                if (!model.END_TIME.IsNullOrEmpty())
                {
                    fModel.END_TIME = Convert.ToDateTime(model.END_TIME.Trim() + " 23:59:59");
                    whereStr += " AND CHECK_START_TIME <= :END_TIME ";
                }
            }
            fModel.CHECK_STATUS = model.CHECK_STATUS;
            if (model.CHECK_STATUS >= 0)
            {
                whereStr += " AND CHECK_STATUS = :CHECK_STATUS ";
            }
            String sQuery = string.Format("SELECT * FROM (SELECT ROWNUM R, T.* FROM ( SELECT * FROM SFCS_FEEDER_KEEP_HEAD WHERE 1=1 {0} {1}) T) WHERE R BETWEEN :Page AND :Limit", whereStr, orderBy);

            var resdata = await _dbConnection.QueryAsync<FeederCheckListModel>(sQuery, fModel);

            List<FeederCheckListModel> list = resdata.ToList();
            foreach (var item in list)
            {
                item.CHECK_TIME = item.CHECK_START_TIME.ToString("yyyy-MM-dd");
                item.FEEDER_QTY = QueryEx<int>("SELECT SUM(FEEDER_TYPE_TOTAL) FROM SFCS_FEEDER_KEEP_DETAIL WHERE KEEP_HEAD_ID = :KEEP_HEAD_ID ", new
                {
                    KEEP_HEAD_ID = item.ID
                }).FirstOrDefault();

                item.CHECK_QTY = QueryEx<int>("SELECT SUM(CHECK_QTY) FROM SFCS_FEEDER_KEEP_DETAIL WHERE KEEP_HEAD_ID = :KEEP_HEAD_ID ", new
                {
                    KEEP_HEAD_ID = item.ID
                }).FirstOrDefault();

                item.CHECK_YEAR = item.CHECK_START_TIME.Year.ToString();
                item.CHECK_HEAD = item.CHECK_START_TIME.Month.ToString() + "月第" + item.CHECK_COUNT + "次盘点";
            }

            return list;
        }

        public async Task<int> LoadPDAFeederCheckListCount(FeederCheckRequestModel model)
        {
            FeederCheckModel fModel = new FeederCheckModel();

            String whereStr = "";

            if (!model.START_TIME.IsNullOrEmpty())
            {
                fModel.START_TIME = Convert.ToDateTime(model.START_TIME.Trim() + " 00:00:00");
                whereStr += " AND CHECK_START_TIME >= :START_TIME ";

                if (!model.END_TIME.IsNullOrEmpty())
                {
                    fModel.END_TIME = Convert.ToDateTime(model.END_TIME.Trim() + " 23:59:59");
                    whereStr += " AND CHECK_START_TIME <= :END_TIME ";
                }
            }
            fModel.CHECK_STATUS = model.CHECK_STATUS;
            if (model.CHECK_STATUS >= 0)
            {
                whereStr += " AND CHECK_STATUS = :CHECK_STATUS ";
            }
            String sQuery = string.Format("SELECT COUNT(1) FROM SFCS_FEEDER_KEEP_HEAD WHERE 1=1 {0}", whereStr);

            int count = await _dbConnection.ExecuteScalarAsync<int>(sQuery, fModel);
            return count;
        }

        /// <summary>
		/// 保存数据
		/// </summary>
		/// <param name="model"></param>
		/// <returns></returns>
		public async Task<decimal> SaveFeederRegionDataByTrans(SmtFeederRegionModel model)
        {
            int result = 1;
            ConnectionFactory.OpenConnection(_dbConnection);
            using (var tran = _dbConnection.BeginTransaction())
            {
                try
                {
                    //新增
                    string insertSql = @"insert into SMT_FEEDER_REGION 
					(ID,FEEDER_TYPE,BEGIN_COUNT,END_COUNT,BETWEEN_STATUS,OUTSIDE_STATUS,ORDER_NO,DESCRIPTION) 
					VALUES (SMT_FEEDER_REGION_SEQ.NEXTVAL,:FEEDER_TYPE,:BEGIN_COUNT,:END_COUNT,:BETWEEN_STATUS,:OUTSIDE_STATUS,:ORDER_NO,:DESCRIPTION)";
                    if (model.InsertRecords != null && model.InsertRecords.Count > 0)
                    {
                        foreach (var item in model.InsertRecords)
                        {
                            var resdata = await _dbConnection.ExecuteAsync(insertSql, new
                            {
                                item.FEEDER_TYPE,
                                item.BEGIN_COUNT,
                                item.END_COUNT,
                                item.BETWEEN_STATUS,
                                item.OUTSIDE_STATUS,
                                item.ORDER_NO,
                                item.DESCRIPTION,

                            }, tran);
                        }
                    }
                    //更新
                    string updateSql = @"Update SMT_FEEDER_REGION set FEEDER_TYPE=:FEEDER_TYPE,BEGIN_COUNT=:BEGIN_COUNT,END_COUNT=:END_COUNT,BETWEEN_STATUS=:BETWEEN_STATUS,OUTSIDE_STATUS=:OUTSIDE_STATUS,ORDER_NO=:ORDER_NO,DESCRIPTION=:DESCRIPTION  
						where ID=:ID ";
                    if (model.UpdateRecords != null && model.UpdateRecords.Count > 0)
                    {
                        foreach (var item in model.UpdateRecords)
                        {
                            var resdata = await _dbConnection.ExecuteAsync(updateSql, new
                            {
                                item.ID,
                                item.FEEDER_TYPE,
                                item.BEGIN_COUNT,
                                item.END_COUNT,
                                item.BETWEEN_STATUS,
                                item.OUTSIDE_STATUS,
                                item.ORDER_NO,
                                item.DESCRIPTION,

                            }, tran);
                        }
                    }
                    //删除
                    string deleteSql = @"Delete from SMT_FEEDER_REGION where ID=:ID ";
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
        /// 查询列表
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public async Task<TableDataModel> LoadeFeederRegionData(SmtStencilRegionRequestModel model)
        {
            int page = 0, limit = 0;
            page = model.Page * model.Limit - model.Limit + 1;
            limit = model.Page * model.Limit;
            model.Page = page;
            model.Limit = limit;

            List<String> sQuery = InquireSQLHelper<SmtStencilRegionRequestModel>(model, "SMT_FEEDER_REGION", 1);

            var resdata = await _dbConnection.QueryAsync<SmtFeederRegionListModel>(sQuery[0], model);

            int cnt = await _dbConnection.ExecuteScalarAsync<int>(sQuery[1], model);
            return new TableDataModel
            {
                count = cnt,
                data = resdata?.ToList(),
            };
        }

    }
}