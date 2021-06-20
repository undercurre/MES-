/*
*┌──────────────────────────────────────────────────────────────┐
*│　描    述：接口实现                                                    
*│　作    者：嘉志科技                                            
*│　版    本：2.0    模板代码自动生成                                                
*│　创建时间：2020-04-14 14:53:45                             
*└──────────────────────────────────────────────────────────────┘
*┌──────────────────────────────────────────────────────────────┐
*│　命名空间： JZ.IMS.Repository.Oracle                                  
*│　类    名： SfcsRuncardRangerRepository                                      
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
using System.Linq;
using System.Collections.Generic;

namespace JZ.IMS.Repository.Oracle
{
    public class SfcsRuncardRangerRepository : BaseRepository<SfcsRuncardRanger, Decimal>, ISfcsRuncardRangerRepository
    {
        public SfcsRuncardRangerRepository(IOptionsSnapshot<DbOption> options)
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
            string sql = "SELECT ENABLED FROM SFCS_RUNCARD_RANGER WHERE ID=:ID";
            var result = await _dbConnection.QueryFirstOrDefaultAsync<string>(sql, new
            {
                ID = id,
            });

            return result == "Y" ? true : false;
        }
        /// <summary>
        /// 修改打印
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<decimal> UpdatePrint(decimal id)
        {
            string sql = "UPDATE SFCS_RUNCARD_RANGER set PRINTED=:PRINTED WHERE ID=:Id";
            return await _dbConnection.ExecuteAsync(sql, new
            {
                PRINTED = 'Y',
                Id = id,
            });
        }
        /// <summary>
        /// 修改打印
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<decimal> UpdatePrintImportRuncard(decimal id)
        {
            string sql = "UPDATE IMPORT_RUNCARD_HEADER SET PRINTED=:PRINTED WHERE ID=:ID";
            return await _dbConnection.ExecuteAsync(sql, new
            {
                PRINTED = 'Y',
                ID = id,
            });
        }

        /// <summary>
        /// 修改激活状态
        /// </summary>
        /// <param name="id">主键</param>
        /// <param name="status">更改后的状态</param>
        /// <returns></returns>
		public async Task<decimal> ChangeEnableStatus(decimal id, bool status)
        {
            string sql = "UPDATE SFCS_RUNCARD_RANGER set ENABLED=:ENABLED WHERE ID=:Id";
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
		public async Task<decimal> GetSEQID(String seq = "SFCS_RUNCARD_RANGER_SEQ")
        {
            string sql = String.Format("SELECT {0}.NEXTVAL MY_SEQ FROM DUAL", seq);
            var result = await _dbConnection.ExecuteScalarAsync(sql);
            return (decimal)result;
        }

        /// <summary>
        /// 加载数据
        /// </summary>
        /// <returns></returns>
        public async Task<TableDataModel> LoadData(SfcsRuncardRangerRequestModel model)
        {
            string condition = " ";
            if (!string.IsNullOrWhiteSpace(model.WO_NO))
            {
                condition += $" and INSTR(SO.WO_NO,:WO_NO)>0 ";
            }
            if (!string.IsNullOrWhiteSpace(model.SN_BEGIN))
            {
                condition += $" and INSTR(RR.SN_BEGIN,:SN_BEGIN)>0 ";
            }
            if (!string.IsNullOrWhiteSpace(model.SN_END))
            {
                condition += $" and INSTR(RR.SN_END,:SN_END)>0 ";
            }
            if (!string.IsNullOrWhiteSpace(model.PART_NO))
            {
                condition += $" and INSTR(SO.PART_NO,:PART_NO)>0 ";
            }
            if (!string.IsNullOrWhiteSpace(model.FIX_HEADER))
            {
                condition += $" and INSTR(RR.FIX_HEADER,:FIX_HEADER)>0 ";
            }
            if (!string.IsNullOrWhiteSpace(model.FIX_TAIL))
            {
                condition += $" and INSTR(RR.FIX_TAIL,:FIX_TAIL)>0 ";
            }

            string sql = @"SELECT ROWNUM AS ROWNO,SO.WO_NO,SO.PART_NO,IPT.DESCRIPTION,IPT.Name, RR.* FROM SFCS_RUNCARD_RANGER RR, SFCS_WO SO,IMS_PART IPT
						   WHERE RR.WO_ID = SO.ID and IPT.Code=SO.Part_No ";
            string pagedSql = SQLBuilderClass.GetPagedSQL(sql, " RR.ID DESC", condition);
            var resdata = await _dbConnection.QueryAsync<dynamic>(pagedSql, model);

            string sqlcnt = @"select count(0) FROM SFCS_RUNCARD_RANGER RR, SFCS_WO SO WHERE RR.WO_ID = SO.ID " + condition;
            int cnt = await _dbConnection.ExecuteScalarAsync<int>(sqlcnt, model);

            return new TableDataModel
            {
                count = cnt,
                data = resdata?.ToList(),
            };
        }

        /// <summary>
		///项目是否已被使用 
		/// </summary>
		/// <param name="id">项目id</param>
		/// <returns></returns>
		public async Task<bool> ItemIsByUsed(decimal id)
        {
            string sql = "select count(0) from SFCS_RUNCARD_RANGER where id = :id";
            object result = await _dbConnection.ExecuteScalarAsync(sql, new
            {
                id
            });

            return (Convert.ToInt32(result) > 0);
        }

        /// <summary>
        /// 保存数据 //LoadStencilExcelFile
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public async Task<decimal> SaveDataByTrans(SfcsRuncardRanger item)
        {
            int result = 1;
            ConnectionFactory.OpenConnection(_dbConnection);
            using (var tran = _dbConnection.BeginTransaction())
            {
                try
                {
                    if (item.ID == 0)
                    {
                        //新增
                        string insertSql = @"insert into SFCS_RUNCARD_RANGER 
					(ID,WO_ID,SN_BEGIN,SN_END,QUANTITY,DIGITAL,RANGE,FIX_HEADER,FIX_TAIL,HEADER_LENGTH,TAIL_LENGTH,PRINTED,EXCLUSIVE_CHAR,STATUS,RANGER_RULE_ID) 
					VALUES (:ID,:WO_ID,:SN_BEGIN,:SN_END,:QUANTITY,:DIGITAL,:RANGE,:FIX_HEADER,:FIX_TAIL,:HEADER_LENGTH,:TAIL_LENGTH,:PRINTED,:EXCLUSIVE_CHAR,:STATUS,
                            :RANGER_RULE_ID)";
                        var newid = await Get_MES_SEQ_ID();
                        var resdata = await _dbConnection.ExecuteAsync(insertSql, new
                        {
                            ID = newid,
                            item.WO_ID,
                            item.SN_BEGIN,
                            item.SN_END,
                            item.QUANTITY,
                            item.DIGITAL,
                            item.RANGE,
                            item.FIX_HEADER,
                            item.FIX_TAIL,
                            item.HEADER_LENGTH,
                            item.TAIL_LENGTH,
                            item.PRINTED,
                            item.EXCLUSIVE_CHAR,
                            item.STATUS,
                            item.RANGER_RULE_ID,

                        }, tran);
                    }
                    else
                    {
                        //更新
                        string updateSql = @"Update SFCS_RUNCARD_RANGER set WO_ID=:WO_ID,SN_BEGIN=:SN_BEGIN,SN_END=:SN_END,QUANTITY=:QUANTITY,DIGITAL=:DIGITAL,
                            RANGE=:RANGE,FIX_HEADER=:FIX_HEADER,FIX_TAIL=:FIX_TAIL,HEADER_LENGTH=:HEADER_LENGTH,TAIL_LENGTH=:TAIL_LENGTH,PRINTED=:PRINTED,
                            EXCLUSIVE_CHAR=:EXCLUSIVE_CHAR,STATUS=:STATUS,RANGER_RULE_ID=:RANGER_RULE_ID  
						where ID=:ID ";
                        var resdata = await _dbConnection.ExecuteAsync(updateSql, new
                        {
                            item.ID,
                            item.WO_ID,
                            item.SN_BEGIN,
                            item.SN_END,
                            item.QUANTITY,
                            item.DIGITAL,
                            item.RANGE,
                            item.FIX_HEADER,
                            item.FIX_TAIL,
                            item.HEADER_LENGTH,
                            item.TAIL_LENGTH,
                            item.PRINTED,
                            item.EXCLUSIVE_CHAR,
                            item.STATUS,
                            item.RANGER_RULE_ID,

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
        /// 更新SN数据
        /// </summary>
        /// <returns></returns>
        public async Task<decimal> UpdateCustomerSNData(ImportRuncardSnAddOrModifyModel model,decimal header_id)
        {
            decimal result = -1;
            try
            {
                String U_UpdateCustomerSN = @"MERGE INTO IMPORT_RUNCARD_SN IRS USING DUAL ON(IRS.SN = :SN AND IRS.HEADER_ID = :HEADER_ID) WHEN MATCHED THEN UPDATE SET UPDATE_BY = :CREATE_BY, UPDATE_TIME = SYSDATE, WO_NO = :WO_NO, MAIN_CARD_IMEI = :MAIN_CARD_IMEI, MINOR_CARD_IMEI = :MINOR_CARD_IMEI, BT = :BT, MAC = :MAC, MEID = :MEID,SN2=:SN2, PART_TYPE=:PART_TYPE, PART_PN=:PART_PN, PART_EAN=:PART_EAN, CUSTOMER=:CUSTOMER, CUSTOMER_MODEL=:CUSTOMER_MODEL, CUSTOMER_CODE=:CUSTOMER_CODE, SUPPLY_LOCATION=:SUPPLY_LOCATION, DELIVERY_LOCATION=:DELIVERY_LOCATION, HARDWARE_VERSION=:HARDWARE_VERSION, SOFTWARE_VERSION=:SOFTWARE_VERSION, COLOUR=:COLOUR, CUSTOMER_BATCH_NO=:CUSTOMER_BATCH_NO, OUTER_BOX_NET_WEIGHT=:OUTER_BOX_NET_WEIGHT, OUTER_BOX_GROSS_WEIGHT=:OUTER_BOX_GROSS_WEIGHT, OUTER_BOX_QTY=:OUTER_BOX_QTY WHEN NOT MATCHED THEN INSERT (ID, SN, WO_NO, ENABLE, CREATE_TIME, CREATE_BY, UPDATE_TIME, UPDATE_BY, MAIN_CARD_IMEI, MINOR_CARD_IMEI, BT, MAC, MEID, HEADER_ID, SN2, PART_TYPE, PART_PN, PART_EAN, CUSTOMER, CUSTOMER_MODEL, CUSTOMER_CODE, SUPPLY_LOCATION, DELIVERY_LOCATION, HARDWARE_VERSION, SOFTWARE_VERSION, COLOUR, CUSTOMER_BATCH_NO, OUTER_BOX_NET_WEIGHT, OUTER_BOX_GROSS_WEIGHT, OUTER_BOX_QTY) VALUES (GET_SEQUENCE('IMPORT_RUNCARD_SN_SEQ'), :SN, :WO_NO, 'Y', SYSDATE, :CREATE_BY, SYSDATE, :CREATE_BY, :MAIN_CARD_IMEI, :MINOR_CARD_IMEI, :BT, :MAC, :MEID, :HEADER_ID, :SN2, :PART_TYPE, :PART_PN, :PART_EAN, :CUSTOMER, :CUSTOMER_MODEL, :CUSTOMER_CODE, :SUPPLY_LOCATION, :DELIVERY_LOCATION, :HARDWARE_VERSION, :SOFTWARE_VERSION, :COLOUR, :CUSTOMER_BATCH_NO, :OUTER_BOX_NET_WEIGHT, :OUTER_BOX_GROSS_WEIGHT, :OUTER_BOX_QTY)";
                result = await _dbConnection.ExecuteAsync(U_UpdateCustomerSN, new
                {
                    SN = model.SN,
                    WO_NO = model.WO_NO,
                    CREATE_BY = model.CREATE_BY,
                    MAIN_CARD_IMEI = model.MAIN_CARD_IMEI,
                    MINOR_CARD_IMEI = model.MINOR_CARD_IMEI,
                    BT = model.BT,
                    MAC = model.MAC,
                    MEID = model.MEID,
                    HEADER_ID = header_id,
                    SN2 = model.SN2,
                    PART_TYPE = model.PART_TYPE,
                    PART_PN = model.PART_PN,
                    PART_EAN = model.PART_EAN,
                    CUSTOMER = model.CUSTOMER,
                    CUSTOMER_MODEL = model.CUSTOMER_MODEL,
                    CUSTOMER_CODE = model.CUSTOMER_CODE,
                    SUPPLY_LOCATION = model.SUPPLY_LOCATION,
                    DELIVERY_LOCATION = model.DELIVERY_LOCATION,
                    HARDWARE_VERSION = model.HARDWARE_VERSION,
                    SOFTWARE_VERSION = model.SOFTWARE_VERSION,
                    COLOUR = model.COLOUR,
                    CUSTOMER_BATCH_NO = model.CUSTOMER_BATCH_NO,
                    OUTER_BOX_NET_WEIGHT = model.OUTER_BOX_NET_WEIGHT,
                    OUTER_BOX_GROSS_WEIGHT = model.OUTER_BOX_GROSS_WEIGHT,
                    OUTER_BOX_QTY = model.OUTER_BOX_QTY
                }, tranBase);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            return result;
        }

        /// <summary>
        /// 更新SN数据
        /// </summary>
        /// <returns></returns>
        public async Task<decimal> AddImportRuncardHeader(ImportRuncardSnAddOrModifyModel model)
        {
            decimal result = -1;
            try
            {
                string insertHeaderSql = @"INSERT INTO IMPORT_RUNCARD_HEADER(ID,WO_NO,CREATE_TIME,CREATE_BY,ENABLE)VALUES (:ID,:WO_NO,SYSDATE,:CREATE_BY,'Y')";
                var newid = await GetSEQID("IMPORT_RUNCARD_HEADER_SEQ");
                result = await _dbConnection.ExecuteAsync(insertHeaderSql, new
                {
                    ID = newid,
                    WO_NO = model.WO_NO,
                    CREATE_BY = model.CREATE_BY
                }, tranBase);
                if (result > 0)
                {
                    result = newid;
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            return result;
        }

        /// <summary>
        /// 特殊SN记录的主表的SN导入数量
        /// </summary>
        /// <returns></returns>
        public async Task<decimal> upSNHeaderByQty(decimal header_id, int qty)
        {
            decimal result = -1;
            try
            {
                String upHeaderSql = @"UPDATE IMPORT_RUNCARD_HEADER SET SN_QTY=:SN_QTY WHERE ID=:ID ";
                result = await _dbConnection.ExecuteAsync(upHeaderSql, new { ID = header_id, SN_QTY = qty }, tranBase);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            return result;
        }

        /// <summary>
        /// 拼板单码打印
        /// </summary>
        /// <param name="printTasks"></param>
        /// <param name="headerList"></param>
        /// <param name="detailList"></param>
        /// <returns></returns>
        public async Task<bool> PrintPuzzleSingleCode(SfcsPrintTasks printTasks, List<SfcsWoRgMultiHeader> headerList, List<SfcsWoRgMultiDetail> detailList)
        {
            bool result = false;
            ConnectionFactory.OpenConnection(_dbConnection);
            using (var tran = _dbConnection.BeginTransaction())
            {
                try
                {
                    int resdata = 0;
                    String insertHeaderSql = @"INSERT INTO SFCS_WO_RG_MULTI_HEADER(ID,WO_RANGER_ID,WO_NO,STATUS,SPLICING_QTY,SN,CREATE_USER,CREATE_TIME) VALUES (:ID,:WO_RANGER_ID,:WO_NO,'0',:SPLICING_QTY,:SN,:CREATE_USER,SYSDATE) ";

                    String insertDetailSql = @"INSERT INTO SFCS_WO_RG_MULTI_DETAIL(ID,MST_ID,SN,CREATE_USER,CREATE_TIME) VALUES (:ID,:MST_ID,:SN,:CREATE_USER,SYSDATE) ";

                    String insertPrintTaskSql = @"INSERT INTO SFCS_PRINT_TASKS(ID,PRINT_FILE_ID,OPERATOR,CREATE_TIME,PRINT_STATUS,PRINT_DATA,PART_NO,WO_NO)VALUES(:ID,:PRINT_FILE_ID,:OPERATOR,SYSDATE,0,:PRINT_DATA,:PART_NO,:WO_NO)";

                    foreach (var item in headerList)
                    {
                        resdata = await _dbConnection.ExecuteAsync(insertHeaderSql, new
                        {
                            ID = item.ID,
                            WO_RANGER_ID = item.WO_RANGER_ID,
                            WO_NO = item.WO_NO,
                            SPLICING_QTY = item.SPLICING_QTY,
                            SN = item.SN,
                            CREATE_USER = item.CREATE_USER
                        }, tran);
                        if (resdata <= 0) { throw new Exception("PRINT_FAIL"); }
                    }
                    foreach (var item in detailList)
                    {
                        resdata = await _dbConnection.ExecuteAsync(insertDetailSql, new
                        {
                            ID = item.ID,
                            MST_ID = item.MST_ID,
                            SN = item.SN,
                            CREATE_USER = item.CREATE_USER
                        }, tran);
                        if (resdata <= 0) { throw new Exception("PRINT_FAIL"); }
                    }

                    resdata = await _dbConnection.ExecuteAsync(insertPrintTaskSql, new
                    {
                        ID = printTasks.ID,
                        PRINT_FILE_ID = printTasks.PRINT_FILE_ID,
                        OPERATOR = printTasks.OPERATOR,
                        PRINT_DATA = printTasks.PRINT_DATA,
                        PART_NO = printTasks.PART_NO,
                        WO_NO = printTasks.WO_NO
                    }, tran);
                    if (resdata <= 0) { throw new Exception("PRINT_FAIL"); }

                    tran.Commit();
                    result = true;
                }
                catch (Exception ex)
                {
                    result = false;
                    tran.Rollback();//回滚事务
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
        /// 拼板余码打印
        /// </summary>
        /// <param name="printTasks"></param>
        /// <param name="headerModel"></param>
        /// <returns></returns>
        public async Task<bool> PrintPuzzleRemainingCodeBySN(SfcsPrintTasks printTasks, SfcsWoRgMultiHeader headerModel)
        {
            bool result = false;
            ConnectionFactory.OpenConnection(_dbConnection);
            using (var tran = _dbConnection.BeginTransaction())
            {
                try
                {
                    int resdata = 0;
                    String updateHeaderSql = @"UPDATE SFCS_WO_RG_MULTI_HEADER SET STATUS = 1,UPDATE_USER=:UPDATE_USER,UPDATE_TIME=SYSDATE WHERE ID = :ID";

                    String updateDetailSql = @"UPDATE SFCS_WO_RG_MULTI_DETAIL SET UPDATE_USER=:UPDATE_USER,UPDATE_TIME=SYSDATE WHERE MST_ID = :MST_ID";

                    String insertPrintTaskSql = @"INSERT INTO SFCS_PRINT_TASKS(ID,PRINT_FILE_ID,OPERATOR,CREATE_TIME,PRINT_STATUS,PRINT_DATA,PART_NO,WO_NO)VALUES(:ID,:PRINT_FILE_ID,:OPERATOR,SYSDATE,0,:PRINT_DATA,:PART_NO,:WO_NO)";

                    resdata = await _dbConnection.ExecuteAsync(updateHeaderSql, new
                    {
                        ID = headerModel.ID,
                        UPDATE_USER = headerModel.UPDATE_USER
                    }, tran);
                    if (resdata <= 0) { throw new Exception("PRINT_FAIL"); }

                    resdata = await _dbConnection.ExecuteAsync(updateDetailSql, new
                    {
                        MST_ID = headerModel.ID,
                        UPDATE_USER = headerModel.UPDATE_USER
                    }, tran);
                    if (resdata <= 0) { throw new Exception("PRINT_FAIL"); }

                    resdata = await _dbConnection.ExecuteAsync(insertPrintTaskSql, new
                    {
                        ID = printTasks.ID,
                        PRINT_FILE_ID = printTasks.PRINT_FILE_ID,
                        OPERATOR = printTasks.OPERATOR,
                        PRINT_DATA = printTasks.PRINT_DATA,
                        PART_NO = printTasks.PART_NO,
                        WO_NO = printTasks.WO_NO
                    }, tran);
                    if (resdata <= 0) { throw new Exception("PRINT_FAIL"); }

                    tran.Commit();
                    result = true;
                }
                catch (Exception ex)
                {
                    result = false;
                    tran.Rollback();//回滚事务
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

        #region 导入特殊SN相关

        /// <summary>
        /// 加载数据
        /// </summary>
        /// <returns></returns>
        public async Task<TableDataModel> LoadImportRuncardSnSummaryData(ImportRuncardSnRequestModel model)
        {
            string sWhere = " ";
            //if (!string.IsNullOrEmpty(model.WO_NO))
            //{
            //    sWhere += $" AND INSTR(IRS.WO_NO,:WO_NO)>0 ";
            //}
            if (!string.IsNullOrEmpty(model.USER_NAME))
            {
                sWhere += $" AND INSTR(IRS.CREATE_BY,:USER_NAME)>0 AND INSTR(IRS.UPDATE_BY,:USER_NAME)>0 ";
            }
            if (model.BEGIN_TIME != null)
            {
                sWhere += $" AND IRS.CREATE_TIME >= :BEGIN_TIME ";
            }
            if (model.END_TIME != null)
            {
                sWhere += $" AND IRS.CREATE_TIME <= :END_TIME ";
            }

            string sQuery = string.Format("SELECT * FROM (SELECT ROWNUM R, T.* FROM (SELECT IRS.WO_NO,IRS.HEADER_ID,TO_CHAR(IRS.CREATE_TIME,'yyyy-MM-dd') AS CREATE_TIME,IRS.CREATE_BY,SUM(1) SUMMARY_QTY FROM IMPORT_RUNCARD_SN IRS WHERE IRS.WO_NO IS NOT NULL AND IRS.HEADER_ID = :HEADER_ID {0} GROUP BY IRS.WO_NO,TO_CHAR(IRS.CREATE_TIME,'yyyy-MM-dd'),IRS.CREATE_BY ORDER BY TO_CHAR(IRS.CREATE_TIME,'yyyy-MM-dd') DESC) T) WHERE R BETWEEN :Page AND :Limit", sWhere);
            var resdata = await _dbConnection.QueryAsync<dynamic>(sQuery, model);

            sQuery = @"SELECT COUNT(1) FROM IMPORT_RUNCARD_SN IRS WHERE IRS.WO_NO IS NOT NULL AND IRS.HEADER_ID = :HEADER_ID " + sWhere;
            int cnt = await _dbConnection.ExecuteScalarAsync<int>(sQuery, model);

            return new TableDataModel
            {
                count = cnt,
                data = resdata?.ToList(),
            };
        }

        /// <summary>
        /// 加载数据
        /// </summary>
        /// <returns></returns>
        public async Task<TableDataModel> LoadImportRuncardHeaderData(ImportRuncardHeaderRequestModel model)
        {
            string sWhere = " ";
            if (!string.IsNullOrEmpty(model.WO_NO))
            {
                sWhere += $" AND INSTR(IRS.WO_NO,:WO_NO)>0 ";
            }
            if (!string.IsNullOrEmpty(model.USER_NAME))
            {
                sWhere += $" AND INSTR(IRS.CREATE_BY,:USER_NAME)>0 AND INSTR(IRS.UPDATE_BY,:USER_NAME)>0 ";
            }
            if (model.BEGIN_TIME != null)
            {
                sWhere += $" AND IRS.CREATE_TIME >= :BEGIN_TIME ";
            }
            if (model.END_TIME != null)
            {
                sWhere += $" AND IRS.CREATE_TIME <= :END_TIME ";
            }

            string sQuery = string.Format("SELECT * FROM (SELECT ROWNUM R, T.* FROM (SELECT * FROM IMPORT_RUNCARD_HEADER IRS WHERE 1=1 {0} ORDER BY IRS.ID ASC) T) WHERE R BETWEEN :Page AND :Limit", sWhere);
            var resdata = await _dbConnection.QueryAsync<dynamic>(sQuery, model);

            sQuery = @"SELECT COUNT(1) FROM IMPORT_RUNCARD_HEADER IRS WHERE 1=1 " + sWhere;
            int cnt = await _dbConnection.ExecuteScalarAsync<int>(sQuery, model);

            return new TableDataModel
            {
                count = cnt,
                data = resdata?.ToList(),
            };
        }

        /// <summary>
        /// 加载数据
        /// </summary>
        /// <returns></returns>
        public async Task<TableDataModel> LoadImportRuncardSnData(ImportRuncardSnRequestModel model)
        {
            string sWhere = " ";
            if (!string.IsNullOrEmpty(model.SN))
            {
                sWhere += $" AND INSTR(IRS.SN,:SN)>0 ";
            }
            if (!string.IsNullOrEmpty(model.USER_NAME))
            {
                sWhere += $" AND INSTR(IRS.CREATE_BY,:USER_NAME)>0 AND INSTR(IRS.UPDATE_BY,:USER_NAME)>0 ";
            }
            if (model.BEGIN_TIME != null)
            {
                sWhere += $" AND IRS.CREATE_TIME >= :BEGIN_TIME ";
            }
            if (model.END_TIME != null)
            {
                sWhere += $" AND IRS.CREATE_TIME <= :END_TIME ";
            }

            string sQuery = string.Format("SELECT * FROM (SELECT ROWNUM R, T.* FROM (SELECT * FROM IMPORT_RUNCARD_SN IRS WHERE IRS.WO_NO IS NOT NULL AND IRS.HEADER_ID = :HEADER_ID {0} ORDER BY IRS.ID ASC) T) WHERE R BETWEEN :Page AND :Limit", sWhere);
            var resdata = await _dbConnection.QueryAsync<dynamic>(sQuery, model);

            sQuery = @"SELECT COUNT(1) FROM IMPORT_RUNCARD_SN IRS WHERE IRS.WO_NO IS NOT NULL AND IRS.HEADER_ID = :HEADER_ID " + sWhere;
            int cnt = await _dbConnection.ExecuteScalarAsync<int>(sQuery, model);

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
		public async Task<decimal> SaveImportRuncardSnDataByTrans(ImportRuncardSnModel model)
        {
            int result = 1;
            ConnectionFactory.OpenConnection(_dbConnection);
            using (var tran = _dbConnection.BeginTransaction())
            {
                try
                {
                    //新增
                    string insertSql = @"INSERT INTO IMPORT_RUNCARD_SN(ID,WO_NO,HEADER_ID,SN,ENABLE,CREATE_TIME,CREATE_BY,MAIN_CARD_IMEI,MINOR_CARD_IMEI,BT,MAC,MEID, SN2, PART_TYPE, PART_PN, PART_EAN, CUSTOMER, CUSTOMER_MODEL, CUSTOMER_CODE, SUPPLY_LOCATION, DELIVERY_LOCATION, HARDWARE_VERSION, SOFTWARE_VERSION, COLOUR, CUSTOMER_BATCH_NO, OUTER_BOX_NET_WEIGHT, OUTER_BOX_GROSS_WEIGHT, OUTER_BOX_QTY) VALUES(:ID,:WO_NO,:HEADER_ID,:SN,'Y',SYSDATE,:CREATE_BY,:MAIN_CARD_IMEI,:MINOR_CARD_IMEI,:BT,:MAC,:MEID, :SN2, :PART_TYPE, :PART_PN, :PART_EAN, :CUSTOMER, :CUSTOMER_MODEL, :CUSTOMER_CODE, :SUPPLY_LOCATION, :DELIVERY_LOCATION, :HARDWARE_VERSION, :SOFTWARE_VERSION, :COLOUR, :CUSTOMER_BATCH_NO, :OUTER_BOX_NET_WEIGHT, :OUTER_BOX_GROSS_WEIGHT, :OUTER_BOX_QTY)";
                    if (model.InsertRecords != null && model.InsertRecords.Count > 0)
                    {
                        #region 主表数据
                        if (String.IsNullOrEmpty(model.InsertRecords[0].WO_NO)) { throw new Exception("SAVE_IMPORTRUNCARDSN_HEADER_ERR"); }
                        ImportRuncardHeader headerModel = await GetAsyncEx<ImportRuncardHeader>("WHERE WO_NO =:WO_NO", new { WO_NO = model.InsertRecords[0].WO_NO });
                        decimal header_id = headerModel == null ? 0 : headerModel.ID;
                        if (header_id < 1)
                        {
                            string insertHeaderSql = @"INSERT INTO IMPORT_RUNCARD_HEADER(ID,WO_NO,CREATE_TIME,CREATE_BY,ENABLE)VALUES (:ID,:WO_NO,SYSDATE,:CREATE_BY,'Y')";
                            header_id = await GetSEQID("IMPORT_RUNCARD_HEADER_SEQ");
                            result = await _dbConnection.ExecuteAsync(insertHeaderSql, new
                            {
                                ID = header_id,
                                WO_NO = model.InsertRecords[0].WO_NO,
                                CREATE_BY = model.InsertRecords[0].CREATE_BY
                            }, tran);
                            if (result < 1) { throw new Exception("SAVE_IMPORTRUNCARDSN_HEADER_ERR"); }
                        }
                        #endregion

                        foreach (var item in model.InsertRecords)
                        {
                            if (!String.IsNullOrEmpty(item.SN) && header_id > 0)
                            {
                                var newid = await GetSEQID("IMPORT_RUNCARD_SN_SEQ");
                                var resdata = await _dbConnection.ExecuteAsync(insertSql, new
                                {
                                    ID = newid,
                                    model.InsertRecords[0].WO_NO,
                                    item.HEADER_ID,
                                    item.SN,
                                    item.CREATE_BY,
                                    item.MAIN_CARD_IMEI,
                                    item.MINOR_CARD_IMEI,
                                    item.BT,
                                    item.MAC,
                                    item.MEID,
                                    item.SN2,
                                    item.PART_TYPE,
                                    item.PART_PN,
                                    item.PART_EAN,
                                    item.CUSTOMER,
                                    item.CUSTOMER_MODEL,
                                    item.CUSTOMER_CODE,
                                    item.SUPPLY_LOCATION,
                                    item.DELIVERY_LOCATION,
                                    item.HARDWARE_VERSION,
                                    item.SOFTWARE_VERSION,
                                    item.COLOUR,
                                    item.CUSTOMER_BATCH_NO,
                                    item.OUTER_BOX_NET_WEIGHT,
                                    item.OUTER_BOX_GROSS_WEIGHT,
                                    item.OUTER_BOX_QTY
                                }, tran);
                            }
                        }

                        String upHeaderSql = @"UPDATE IMPORT_RUNCARD_HEADER SET SN_QTY=( SELECT COUNT(1) QTY FROM IMPORT_RUNCARD_SN WHERE HEADER_ID = :HEADER_ID) WHERE ID=:HEADER_ID ";
                        await _dbConnection.ExecuteAsync(upHeaderSql, new { HEADER_ID = header_id }, tran);
                    }
                    //更新
                    string updateSql = @"UPDATE IMPORT_RUNCARD_SN SET SN=:SN, UPDATE_TIME=SYSDATE, UPDATE_BY=:UPDATE_BY, MAIN_CARD_IMEI=:MAIN_CARD_IMEI, MINOR_CARD_IMEI=:MINOR_CARD_IMEI, BT=:BT, MAC=:MAC, MEID=:MEID, SN2=:SN2, PART_TYPE=:PART_TYPE, PART_PN=:PART_PN, PART_EAN=:PART_EAN, CUSTOMER=:CUSTOMER, CUSTOMER_MODEL=:CUSTOMER_MODEL, CUSTOMER_CODE=:CUSTOMER_CODE, SUPPLY_LOCATION=:SUPPLY_LOCATION, DELIVERY_LOCATION=:DELIVERY_LOCATION, HARDWARE_VERSION=:HARDWARE_VERSION, SOFTWARE_VERSION=:SOFTWARE_VERSION, COLOUR=:COLOUR, CUSTOMER_BATCH_NO=:CUSTOMER_BATCH_NO, OUTER_BOX_NET_WEIGHT=:OUTER_BOX_NET_WEIGHT, OUTER_BOX_GROSS_WEIGHT=:OUTER_BOX_GROSS_WEIGHT, OUTER_BOX_QTY=:OUTER_BOX_QTY WHERE ID=:ID ";
                    if (model.UpdateRecords != null && model.UpdateRecords.Count > 0)
                    {
                        foreach (var item in model.UpdateRecords)
                        {
                            var resdata = await _dbConnection.ExecuteAsync(updateSql, new
                            {
                                item.ID,
                                item.SN,
                                item.UPDATE_TIME,
                                item.UPDATE_BY,
                                item.MAIN_CARD_IMEI,
                                item.MINOR_CARD_IMEI,
                                item.BT,
                                item.MAC,
                                item.MEID,
                                item.SN2,
                                item.PART_TYPE,
                                item.PART_PN,
                                item.PART_EAN,
                                item.CUSTOMER,
                                item.CUSTOMER_MODEL,
                                item.CUSTOMER_CODE,
                                item.SUPPLY_LOCATION,
                                item.DELIVERY_LOCATION,
                                item.HARDWARE_VERSION,
                                item.SOFTWARE_VERSION,
                                item.COLOUR,
                                item.CUSTOMER_BATCH_NO,
                                item.OUTER_BOX_NET_WEIGHT,
                                item.OUTER_BOX_GROSS_WEIGHT,
                                item.OUTER_BOX_QTY
                            }, tran);
                        }
                    }
                    //删除
                    string deleteSql = @"DELETE FROM IMPORT_RUNCARD_SN WHERE ID=:ID ";
                    if (model.RemoveRecords != null && model.RemoveRecords.Count > 0 && model.RemoveRecords[0].HEADER_ID > 0)
                    {
                        foreach (var item in model.RemoveRecords)
                        {
                            var resdata = await _dbConnection.ExecuteAsync(deleteSql, new
                            {
                                item.ID
                            }, tran);
                        }

                        String upHeaderSql = @"UPDATE IMPORT_RUNCARD_HEADER SET SN_QTY=( SELECT COUNT(1) QTY FROM IMPORT_RUNCARD_SN WHERE HEADER_ID = :HEADER_ID) WHERE ID=:HEADER_ID ";
                        await _dbConnection.ExecuteAsync(upHeaderSql, new { HEADER_ID = model.RemoveRecords[0].HEADER_ID }, tran);
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
        public async Task<decimal> SaveImportRuncardHeaderByTrans(ImportRuncardHeaderModel model)
        {
            int result = 1;
            ConnectionFactory.OpenConnection(_dbConnection);
            using (var tran = _dbConnection.BeginTransaction())
            {
                try
                {
      //              //新增
      //              string insertSql = @"INSERT INTO IMPORT_RUNCARD_HEADER (ID,WO_NO,CREATE_TIME,CREATE_BY,ENABLE,SN_QTY) VALUES (:ID,:WO_NO,:CREATE_TIME,:CREATE_BY,:ENABLE,:SN_QTY)";
      //              if (model.InsertRecords != null && model.InsertRecords.Count > 0)
      //              {
      //                  foreach (var item in model.InsertRecords)
      //                  {
      //                      var newid = await GetSEQID();
      //                      var resdata = await _dbConnection.ExecuteAsync(insertSql, new
      //                      {
      //                          ID = newid,
      //                          item.WO_NO,
      //                          item.CREATE_TIME,
      //                          item.CREATE_BY,
      //                          item.ENABLE,
      //                          item.SN_QTY,

      //                      }, tran);
      //                  }
      //              }
      //              //更新
      //              string updateSql = @"UPDATE IMPORT_RUNCARD_HEADER SET WO_NO=:WO_NO,CREATE_TIME=:CREATE_TIME,CREATE_BY=:CREATE_BY,ENABLE=:ENABLE,SN_QTY=:SN_QTY  
						//WHERE ID=:ID ";
      //              if (model.UpdateRecords != null && model.UpdateRecords.Count > 0)
      //              {
      //                  foreach (var item in model.UpdateRecords)
      //                  {
      //                      var resdata = await _dbConnection.ExecuteAsync(updateSql, new
      //                      {
      //                          item.ID,
      //                          item.WO_NO,
      //                          item.CREATE_TIME,
      //                          item.CREATE_BY,
      //                          item.ENABLE,
      //                          item.SN_QTY,

      //                      }, tran);
      //                  }
      //              }
                    //删除
                    string deleteSql = @"DELETE FROM IMPORT_RUNCARD_HEADER WHERE ID=:ID ";
                    string deleteSNSql = @"DELETE FROM IMPORT_RUNCARD_SN WHERE HEADER_ID=:HEADER_ID ";
                    if (model.RemoveRecords != null && model.RemoveRecords.Count > 0)
                    {
                        foreach (var item in model.RemoveRecords)
                        {
                            await _dbConnection.ExecuteAsync(deleteSql, new { item.ID }, tran);
                            await _dbConnection.ExecuteAsync(deleteSNSql, new { HEADER_ID = item.ID }, tran);
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

        #endregion

        #region 镭雕任务下达

        /// <summary>
        ///项目是否已存在
        /// </summary>
        /// <returns></returns>
        public async Task<bool> ItemIsByLaserTask(Decimal type_id, String task_type)
        {
            string sql = "SELECT COUNT(0) FROM SFCS_LASER_TASK WHERE TYPE_ID =:TYPE_ID AND TASK_TYPE=:TASK_TYPE";
            object result = await _dbConnection.ExecuteScalarAsync(sql, new
            {
                TYPE_ID = type_id,
                TASK_TYPE = task_type
            });

            return (Convert.ToInt32(result) > 0);
        }

        /// <summary>
        /// 镭雕任务下达数据列表
        /// </summary>
        /// <returns></returns>
        public async Task<TableDataModel> LoadLaserTaskData(SfcsLaserTaskRequestModel model)
        {
            string sWhere = " ";
            if (!string.IsNullOrEmpty(model.WO_NO))
            {
                sWhere += $" AND INSTR(LT.WO_NO,:WO_NO)>0 ";
            }
            if (!string.IsNullOrEmpty(model.PART_NO))
            {
                sWhere += $" AND INSTR(LT.PART_NO,:PART_NO)>0 ";
            }
            if (!string.IsNullOrEmpty(model.PART_DESC))
            {
                sWhere += $" AND INSTR(LT.PART_DESC=:PART_DESC)>0 ";
            }
            if (!string.IsNullOrEmpty(model.PRINT_STATUS))
            {
                sWhere += $" AND LT.PRINT_STATUS = :PRINT_STATUS ";
            }
            if (!string.IsNullOrEmpty(model.TASK_TYPE))
            {
                sWhere += $" AND LT.TASK_TYPE = :TASK_TYPE ";
            }
            if (!string.IsNullOrEmpty(model.USER_NAME))
            {
                sWhere += $" AND INSTR(LT.CREATE_USER,:USER_NAME)>0 AND INSTR(LT.UPDATE_USER,:USER_NAME)>0 ";
            }
            if (!string.IsNullOrEmpty(model.ENABLED))
            {
                sWhere += $" AND LT.ENABLED = :ENABLED ";
            }
            if (model.BEGIN_TIME != null)
            {
                sWhere += $" AND LT.CREATE_TIME >= :BEGIN_TIME ";
            }
            if (model.END_TIME != null)
            {
                sWhere += $" AND LT.CREATE_TIME <= :END_TIME ";
            }
            if(model.MACHINE_CODE != null)
            {
                sWhere += $" AND LT.MACHINE_CODE = :MACHINE_CODE ";
            }
            if (!string.IsNullOrEmpty(model.Key))
            {
                sWhere += $" AND (INSTR(LT.WO_NO,:Key)>0 OR INSTR(LT.PART_NO,:Key)>0 OR INSTR(LT.PART_DESC=:Key)>0 OR INSTR(LT.CREATE_USER,:Key)>0 OR INSTR(LT.UPDATE_USER,:Key)>0)";
            }

            string sQuery = string.Format("SELECT * FROM (SELECT ROWNUM R, T.* FROM (SELECT * FROM SFCS_LASER_TASK LT WHERE LT.ID > 0 {0} ORDER BY LT.ID DESC) T) WHERE R BETWEEN :Page AND :Limit", sWhere);
            var resdata = await _dbConnection.QueryAsync<dynamic>(sQuery, model);

            sQuery = @"SELECT COUNT(1) FROM SFCS_LASER_TASK LT WHERE LT.ID > 0 " + sWhere;
            int cnt = await _dbConnection.ExecuteScalarAsync<int>(sQuery, model);

            return new TableDataModel
            {
                count = cnt,
                data = resdata?.ToList(),
            };
        }

        /// <summary>
        /// 保存镭雕任务下达数据
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public async Task<decimal> SaveLaserTaskDataByTrans(SfcsLaserTaskModel model)
        {
            int result = 1;
            ConnectionFactory.OpenConnection(_dbConnection);
            using (var tran = _dbConnection.BeginTransaction())
            {
                try
                {
                    //新增
                    string insertSql = @"INSERT INTO SFCS_LASER_TASK 
					(ID,TYPE_ID,WO_NO,PART_NO,PART_DESC,TASK_TYPE,PRINT_TOTAL,PRINT_QTY,PRINT_STATUS,CREATE_USER,CREATE_TIME,ENABLED,MACHINE_CODE) 
					VALUES (:ID,:TYPE_ID,:WO_NO,:PART_NO,:PART_DESC,:TASK_TYPE,:PRINT_TOTAL,:PRINT_QTY,'1',:CREATE_USER,SYSDATE,:ENABLED,:MACHINE_CODE)";
                    if (model.InsertRecords != null && model.InsertRecords.Count > 0)
                    {
                        foreach (var item in model.InsertRecords)
                        {
                            var newid = await GetSEQID("SFCS_LASER_TASK_SEQ");
                            result = await _dbConnection.ExecuteAsync(insertSql, new
                            {
                                ID = newid,
                                item.TYPE_ID,
                                item.WO_NO,
                                item.PART_NO,
                                item.PART_DESC,
                                item.TASK_TYPE,
                                item.PRINT_TOTAL,
                                item.PRINT_QTY,
                                item.CREATE_USER,
                                item.ENABLED,
                                item.MACHINE_CODE

                            }, tran);
                        }
                    }
                    //更新
                    string updateSql = @"UPDATE SFCS_LASER_TASK SET TYPE_ID=:TYPE_ID,WO_NO=:WO_NO,PART_NO=:PART_NO,PART_DESC=:PART_DESC,TASK_TYPE=:TASK_TYPE,PRINT_TOTAL=:PRINT_TOTAL,PRINT_QTY=:PRINT_QTY,PRINT_STATUS=:PRINT_STATUS,UPDATE_USER=:UPDATE_USER,UPDATE_TIME=SYSDATE,ENABLED=:ENABLED  
						WHERE ID=:ID ";
                    if (model.UpdateRecords != null && model.UpdateRecords.Count > 0)
                    {
                        foreach (var item in model.UpdateRecords)
                        {
                            result = await _dbConnection.ExecuteAsync(updateSql, new
                            {
                                item.ID,
                                item.TYPE_ID,
                                item.WO_NO,
                                item.PART_NO,
                                item.PART_DESC,
                                item.TASK_TYPE,
                                item.PRINT_TOTAL,
                                item.PRINT_QTY,
                                item.PRINT_STATUS,
                                item.UPDATE_USER,
                                item.ENABLED,
                                item.MACHINE_CODE

                            }, tran);
                        }
                    }
                    //删除
                    string deleteSql = @"DELETE FROM SFCS_LASER_TASK WHERE ID=:ID ";
                    if (model.RemoveRecords != null && model.RemoveRecords.Count > 0)
                    {
                        foreach (var item in model.RemoveRecords)
                        {
                            result = await _dbConnection.ExecuteAsync(deleteSql, new
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
        /// 更新镭雕任务状态(批量)
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public async Task<decimal> UpdateLaserTaskStatus(UpdateLaserTaskStatusRequestModel model)
        {
            int result = 1;
            ConnectionFactory.OpenConnection(_dbConnection);
            using (var tran = _dbConnection.BeginTransaction())
            {
                try
                {
                    //更新
                    string updateSql = @"UPDATE SFCS_LASER_TASK SET ENABLED=:ENABLED,UPDATE_USER=:UPDATE_USER,UPDATE_TIME=SYSDATE  WHERE ID=:ID ";
                    if (model.STATUS_LIST != null && model.STATUS_LIST.Count > 0)
                    {
                        foreach (var item in model.STATUS_LIST)
                        {
                            if (item.ENABLED != "Y" && item.ENABLED != "N") { throw new Exception("状态错误！"); }
                            await _dbConnection.ExecuteAsync(updateSql, new
                            {
                                ID = item.ID,
                                ENABLED = item.ENABLED,
                                UPDATE_USER = model.USER_NAME
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
        /// 根据ID获取
        /// </summary>
        /// <param name="ID"></param>
        /// <returns></returns>
        public async Task<decimal> GetSfcsLaserTask(List<LaserTaskStatusRequestModel> laserTaskStatusRequestModels)
        {
            String ids = String.Empty;

            String getSql = @"SELECT  * from  SFCS_LASER_TASK  WHERE ID IN ({0}) AND ENABLED = 'Y'";
            foreach(LaserTaskStatusRequestModel item in laserTaskStatusRequestModels)
            {
                ids = ids + (item.ID + ",");
            }
            ids = ids.TrimEnd(',');
            var result = await  _dbConnection.QueryAsync<SfcsLaserTaskAddOrModifyModel>(String.Format(getSql, ids));
            return result.Count();
        }


        /// <summary>
        /// 更新镭雕任务状态(批量)
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public async Task<decimal> DeleLaserTasks(List<LaserTaskStatusRequestModel> laserTaskStatusRequestModels)
        {
            int result = 1;
            ConnectionFactory.OpenConnection(_dbConnection);
            using (var tran = _dbConnection.BeginTransaction())
            {
                try
                {
                    //更新
                    string updateSql = @"DELETE FROM  SFCS_LASER_TASK  WHERE ID=:ID ";
                    foreach (var item in laserTaskStatusRequestModels)
                    {
                        if (item.ENABLED != "Y" && item.ENABLED != "N") { throw new Exception("状态错误！"); }
                        await _dbConnection.ExecuteAsync(updateSql, new
                        {
                            ID = item.ID
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
        #endregion

    }
}