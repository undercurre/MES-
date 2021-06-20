/*
*┌──────────────────────────────────────────────────────────────┐
*│　描    述：打印任务表接口实现                                                    
*│　作    者：嘉志科技                                            
*│　版    本：2.0    模板代码自动生成                                                
*│　创建时间：2020-09-29 11:49:59                             
*└──────────────────────────────────────────────────────────────┘
*┌──────────────────────────────────────────────────────────────┐
*│　命名空间： JZ.IMS.Repository.Oracle                                  
*│　类    名： SfcsPrintTasksRepository                                      
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
using JZ.IMS.Core.Extensions;

namespace JZ.IMS.Repository.Oracle
{
    public class SfcsPrintTasksRepository : BaseRepository<SfcsPrintTasks, Decimal>, ISfcsPrintTasksRepository
    {
        public SfcsPrintTasksRepository(IOptionsSnapshot<DbOption> options)
        {
            _dbOption = options.Get("iWMS");
            if (_dbOption == null)
            {
                throw new ArgumentNullException(nameof(DbOption));
            }
            _dbConnection = ConnectionFactory.CreateConnection(_dbOption.DbType, _dbOption.ConnectionString);
        }

        /// <summary>
        /// 新增打印
        /// </summary>
        /// <param name="ID">打印id</param>
        /// <param name="printFileId">打印文件id</param>
        /// <param name="user">打印人</param>
        /// <param name="printData">打印内容</param>
        /// <param name="wo_no">工单号</param>
        /// <param name="part_no">料号</param>
        /// <returns></returns>
        public async Task<Boolean> InsertPrintTask(decimal ID, decimal printFileId, String user, String printData, String wo_no, String part_no)
        {
            DynamicParameters p = new DynamicParameters();
            p.Add("ID", ID, System.Data.DbType.Decimal);
            p.Add("PRINT_FILE_ID", printFileId, System.Data.DbType.Decimal);
            p.Add("OPERATOR", user, System.Data.DbType.String);
            p.Add("PRINT_DATA", printData, System.Data.DbType.String);
            p.Add("PART_NO", part_no, System.Data.DbType.String);
            p.Add("WO_NO", wo_no, System.Data.DbType.String);
            String insertPrintTaskSql = @"INSERT INTO SFCS_PRINT_TASKS(ID,PRINT_FILE_ID,OPERATOR,CREATE_TIME,PRINT_STATUS,PRINT_DATA,PART_NO,WO_NO)VALUES(
			:ID,:PRINT_FILE_ID,:OPERATOR,sysdate,0,:PRINT_DATA,:PART_NO,:WO_NO)";
            decimal result = await _dbConnection.ExecuteAsync(insertPrintTaskSql, p, commandType: CommandType.Text);
            if (result > 0)
            {
                return true;
            }
            else
            {
                return false;
            }

        }

        /// <summary>
        /// 根据主键获取激活状态
        /// </summary>
        /// <param name="id">主键</param>
        /// <returns></returns>
        public async Task<Boolean> GetEnableStatus(decimal id)
        {
            string sql = "SELECT ENABLED FROM SFCS_PRINT_TASKS WHERE ID=:ID";
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
            string sql = "UPDATE SFCS_PRINT_TASKS set ENABLED=:ENABLED WHERE ID=:Id";
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
            string sql = "SELECT SFCS_PRINT_TASKS_SEQ.NEXTVAL MY_SEQ FROM DUAL";
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
            string sql = "select count(0) from SFCS_PRINT_TASKS where id = :id";
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
        public async Task<decimal> SaveDataByTrans(SfcsPrintTasksModel model)
        {
            int result = 1;
            ConnectionFactory.OpenConnection(_dbConnection);
            using (var tran = _dbConnection.BeginTransaction())
            {
                try
                {
                    //新增
                    string insertSql = @"insert into SFCS_PRINT_TASKS 
					(ID,PRINT_FILE_ID,OPERATOR,CREATE_TIME,PRINT_STATUS,PRINT_TIME,PRINT_DATA,PART_NO,WO_NO) 
					VALUES (:ID,:PRINT_FILE_ID,:OPERATOR,:CREATE_TIME,:PRINT_STATUS,:PRINT_TIME,:PRINT_DATA,:PART_NO,:WO_NO)";
                    if (model.InsertRecords != null && model.InsertRecords.Count > 0)
                    {
                        foreach (var item in model.InsertRecords)
                        {
                            var newid = await GetSEQID();
                            var resdata = await _dbConnection.ExecuteAsync(insertSql, new
                            {
                                ID = newid,
                                item.PRINT_FILE_ID,
                                item.OPERATOR,
                                item.CREATE_TIME,
                                item.PRINT_STATUS,
                                item.PRINT_TIME,
                                item.PRINT_DATA,
                                item.PART_NO,
                                item.WO_NO,

                            }, tran);
                        }
                    }
                    //更新
                    string updateSql = @"Update SFCS_PRINT_TASKS set PRINT_FILE_ID=:PRINT_FILE_ID,OPERATOR=:OPERATOR,CREATE_TIME=:CREATE_TIME,PRINT_STATUS=:PRINT_STATUS,PRINT_TIME=:PRINT_TIME,PRINT_DATA=:PRINT_DATA,PART_NO=:PART_NO,WO_NO=:WO_NO  
						where ID=:ID ";
                    if (model.UpdateRecords != null && model.UpdateRecords.Count > 0)
                    {
                        foreach (var item in model.UpdateRecords)
                        {
                            var resdata = await _dbConnection.ExecuteAsync(updateSql, new
                            {
                                item.ID,
                                item.PRINT_FILE_ID,
                                item.OPERATOR,
                                item.CREATE_TIME,
                                item.PRINT_STATUS,
                                item.PRINT_TIME,
                                item.PRINT_DATA,
                                item.PART_NO,
                                item.WO_NO,

                            }, tran);
                        }
                    }
                    //删除
                    string deleteSql = @"Delete from SFCS_PRINT_TASKS where ID=:ID ";
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
        /// 获取打印任务列表数据
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public async Task<TableDataModel> GetPrintTasksData(GetPrintTasksRequestModel model)
        {
            int page = 0, limit = 0;
            page = model.Page * model.Limit - model.Limit + 1;
            limit = model.Page * model.Limit;
            model.Page = page;
            model.Limit = limit;

            string sWhere = " WHERE SPT.PRINT_FILE_ID = SPF.ID AND SP.LOOKUP_TYPE = 'LABEL_TYPE' AND SPF.LABEL_TYPE = SP.LOOKUP_CODE ";

            if (!model.CHINESE.IsNullOrWhiteSpace())
            {
                sWhere += $" AND SP.CHINESE = :CHINESE ";
            }
            if (model.ID > 0)
            {
                sWhere += $" AND SPT.ID =:ID ";
            }
            if (!model.PRINT_STATUS.IsNullOrWhiteSpace())
            {
                sWhere += $" AND SPT.PRINT_STATUS = :PRINT_STATUS ";
            }
            if (!model.OPERATOR.IsNullOrWhiteSpace())
            {
                sWhere += $" AND SPT.OPERATOR = :OPERATOR ";
            }
            if (model.START_TIME != null)
            {
                sWhere += $" AND SPT.CREATE_TIME >= :START_TIME ";
            }
            if (model.END_TIME != null)
            {
                sWhere += $" AND SPT.CREATE_TIME <= :END_TIME ";
            }
            if (!model.PART_NO.IsNullOrWhiteSpace())
            {
                sWhere += $" AND INSTR(SPT.PART_NO,:PART_NO)> 0 ";
            }
            if (!model.WO_NO.IsNullOrWhiteSpace())
            {
                sWhere += $" AND INSTR(SPT.WO_NO,:WO_NO)> 0 ";
            }
            if (!model.CARTON_NO.IsNullOrWhiteSpace())
            {
                sWhere += $" AND INSTR(SPT.CARTON_NO,:CARTON_NO)> 0 ";
            }
            if (!model.PALLET_NO.IsNullOrWhiteSpace())
            {
                sWhere += $" AND INSTR(SPT.PALLET_NO,:PALLET_NO)> 0 ";
            }
            if (model.PRINT_NO != null && model.PRINT_NO == 0)
            {
                sWhere += $" AND SPT.PRINT_NO IS NULL OR SPT.PRINT_NO = '0' ";
            }
            else if (model.PRINT_NO != null && model.PRINT_NO > 0)
            {

                sWhere += $" AND SPT.PRINT_NO = :PRINT_NO ";
            }

            //ID 打印任务表id, FILE_NAME 文件名, CHINESE 描述(CN), OPERATOR 操作人员, CREATE_TIME 创建时间, PRINT_STATUS 打印状态0：未打印；1：已打印；2：报废
            string sQuery = string.Format("SELECT * FROM (SELECT ROWNUM R, T.* FROM ( SELECT SPT.ID, SPF.FILE_NAME, SP.CHINESE, SPT.OPERATOR, SPT.PART_NO, SPT.WO_NO, SPT.CARTON_NO, SPT.PALLET_NO, NVL(SPT.PRINT_NO ,0) PRINT_NO, SPT.CREATE_TIME, SPT.PRINT_STATUS FROM SFCS_PRINT_TASKS SPT, SFCS_PRINT_FILES  SPF, SFCS_PARAMETERS SP {0} ORDER BY SPT.PRINT_STATUS ASC, SPT.ID DESC) T) WHERE R BETWEEN :Page AND :Limit", sWhere);
            var resdata = await _dbConnection.QueryAsync<object>(sQuery, model);

            sQuery = string.Format("SELECT COUNT(1) FROM SFCS_PRINT_TASKS SPT, SFCS_PRINT_FILES  SPF, SFCS_PARAMETERS SP {0} ", sWhere);

            int cnt = await _dbConnection.ExecuteScalarAsync<int>(sQuery, model);
            return new TableDataModel
            {
                count = cnt,
                data = resdata?.ToList(),
            };

        }

        /// <summary>
        /// 添加重复打印数据
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public async Task<Boolean> RepeatPrintTasks(SfcsPrintTasks model)
        {
            int result = 0;
            ConnectionFactory.OpenConnection(_dbConnection);
            using (var tran = _dbConnection.BeginTransaction())
            {
                try
                {

                    String insertSql = @"INSERT INTO SFCS_PRINT_TASKS_REPEAT (ID,PRINT_TASKS_ID,PRINT_USER,PRINT_TIME) VALUES (:ID,:PRINT_TASKS_ID,:PRINT_USER, SYSDATE)";
                    var newid = await _dbConnection.ExecuteScalarAsync("SELECT SFCS_PRINT_TASKS_REPEAT_SEQ.NEXTVAL MY_SEQ FROM DUAL");
                    result += await _dbConnection.ExecuteAsync(insertSql, new
                    {
                        ID = newid,
                        PRINT_TASKS_ID = model.ID,
                        PRINT_USER = model.PRINTER
                    }, tran);

                    int print_no = Convert.ToInt32(model.PRINT_NO) + 1;
                    String updateSql = @"UPDATE SFCS_PRINT_TASKS SET PRINT_NO=:PRINT_NO WHERE ID=:ID ";
                    result += await _dbConnection.ExecuteAsync(updateSql, new
                    {
                        PRINT_NO = print_no,
                        ID = model.ID
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
            return result > 0 ? true : false;
        }

        /// <summary>
        /// 无码报工打印标签
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public async Task<Decimal> SavePrintTasksData(SfcsPrintTasks model, SfcsContainerListListModel sclModel)
        {
            Decimal print_id = 0;
            ConnectionFactory.OpenConnection(_dbConnection);
            using (var tran = _dbConnection.BeginTransaction())
            {
                try
                {
                    #region 添加IMS_REEL表

                    //string BarcodeFormatter = "M{0:yyMMdd}{1:000000}";
                    //decimal reel_id = (await QueryAsyncEx<decimal>("SELECT SEQ_REEL.NEXTVAL FROM DUAL")).FirstOrDefault();
                    string reel_code = model.CARTON_NO;//string.Format(BarcodeFormatter, DateTime.Now, reel_id);//物料条码
                                                       //rpModel.REEL_CODE.Add(reel_code);

                    //SELECT IMS.IMS_REEL_SEQ.NEXTVAL FROM DUAL
                    //SELECT IMS_REEL_SEQ.NEXTVAL@WMS FROM DUAL
                    decimal reelId = (await QueryAsyncEx<decimal>("SELECT IMS_REEL_SEQ.NEXTVAL@WMS FROM DUAL")).FirstOrDefault();

                    //IMS_REEL@WMS IMS.IMS_REEL
                    string selectVendorIdSql = @"SELECT ID FROM IMS_VENDOR@WMS WHERE CODE = 'Pilot'";
                    string selectPartIdSql = @"SELECT * FROM IMS_PART@WMS WHERE CODE=:CODE";
                    string selectCartonNoSql = @"SELECT COUNT(*) FROM SFCS_RUNCARD  WHERE CARTON_NO=:CARTON_NO ";
                    string selectImsReelSql = @"SELECT COUNT(*) FROM IMS_REEL@WMS WHERE CODE=:CODE ";
                    string insertImsReelSql = @"INSERT INTO IMS_REEL@WMS(ID,CODE,VENDOR_ID,PART_ID,MAKER_PART_ID,DATE_CODE,LOT_CODE,MSD_LEVEL,ESD_FLAG,CASE_QTY,IQC_FLAG,ORIGINAL_QUANTITY) VALUES(:ID,:CODE,:VENDOR_ID,:PART_ID,:MAKER_PART_ID,:DATE_CODE,:LOT_CODE,:MSD_LEVEL,:ESD_FLAG,:CASE_QTY,:IQC_FLAG,:ORIGINAL_QUANTITY)";

                    var vendorID = await _dbConnection.ExecuteScalarAsync<decimal>(selectVendorIdSql);
                    var partID = (await _dbConnection.QueryAsync<ImsPart>(selectPartIdSql, new { CODE = model.PART_NO })).FirstOrDefault()?.ID ?? 0;
                    var countOfSn = await _dbConnection.ExecuteScalarAsync<decimal>(selectCartonNoSql, new { CARTON_NO = model.CARTON_NO });
                    var count = await _dbConnection.ExecuteScalarAsync<int>(selectImsReelSql, new { CODE = reel_code });

                    if (count <= 0)
                    {
                        var effectNum = await ExecuteAsync(insertImsReelSql, new
                        {
                            ID = reelId,
                            CODE = reel_code,
                            VENDOR_ID = vendorID,
                            PART_ID = partID,
                            MAKER_PART_ID = -1,
                            DATE_CODE = DateTime.Now.Date,
                            LOT_CODE = "",
                            MSD_LEVEL = "1",
                            ESD_FLAG = "",
                            CASE_QTY = 1,
                            IQC_FLAG = "Y",
                            ORIGINAL_QUANTITY = countOfSn
                        }, tran);

                        if (effectNum <= 0)
                            throw new Exception("更新条码数据异常，请稍后再尝试。");
                    }
                    

                    #endregion

                    int result = await _dbConnection.ExecuteAsync("INSERT INTO SFCS_CONTAINER_LIST (DATA_TYPE, CONTAINER_SN, PART_NO, SITE_ID, QUANTITY, FULL_FLAG, SEQUENCE) VALUES (:DATA_TYPE, :CONTAINER_SN, :PART_NO, :SITE_ID, :QUANTITY, :FULL_FLAG, :SEQUENCE) ", new
                    {
                        DATA_TYPE = sclModel.DATA_TYPE,
                        CONTAINER_SN = sclModel.CONTAINER_SN,
                        PART_NO = sclModel.PART_NO,
                        SITE_ID = sclModel.SITE_ID,
                        QUANTITY = sclModel.QUANTITY,
                        FULL_FLAG = sclModel.FULL_FLAG,
                        SEQUENCE = sclModel.SEQUENCE
                    }, tran);

                    if (result > 0)
                    {
                        String insertSql = @"INSERT INTO SFCS_PRINT_TASKS(ID,PRINT_FILE_ID,OPERATOR,CREATE_TIME,PRINT_STATUS,PRINT_DATA,CARTON_NO,PART_NO,WO_NO)VALUES(:ID,:PRINT_FILE_ID,:OPERATOR,SYSDATE,0,:PRINT_DATA,:CARTON_NO,:PART_NO,:WO_NO)";
                        print_id = await GetSEQID();
                        result = await _dbConnection.ExecuteAsync(insertSql, new
                        {
                            ID = print_id,
                            PRINT_FILE_ID = model.PRINT_FILE_ID,
                            WO_NO = model.WO_NO,
                            PART_NO = model.PART_NO,
                            CARTON_NO = model.CARTON_NO,
                            PRINT_DATA = model.PRINT_DATA,
                            OPERATOR = model.OPERATOR
                        }, tran);

                        if (result <= 0)
                        {
                            print_id = 0;
                        }
                        else
                        {

                            string insertInboundRecordSql = @"INSERT INTO SFCS_INBOUND_RECORD_INFO (ID,WO_ID,INBOUND_NO,INBOUND_QTY,STATUS,CREATE_TIME,CREATE_BY,AUTO_EDITOR) 
					                                           VALUES (:ID,:WO_ID,:INBOUND_NO,:INBOUND_QTY,'0',SYSDATE,:CREATE_BY,:AUTO_EDITOR)";
                            var newid = await GetSEQID();
                            SfcsWo swModel = QueryEx<SfcsWo>("SELECT * FROM SFCS_WO WHERE WO_NO = :WO_NO", new { WO_NO = model.WO_NO })?.FirstOrDefault();
                            if (swModel == null) { throw new Exception("WO_NO_NOT_EXIST"); }

                            //是否自动审核
                            var isAutoEditor = GlobalVariables.FailedCode;
                            if (swModel.TURNIN_TYPE.Equals("Y"))
                            {
                                isAutoEditor = GlobalVariables.successCode;
                            }
                            var WO_ID = Convert.ToDecimal(swModel.ID);

                            //將序列轉成36進制表示
                            string r = Core.Utilities.RadixConvertPublic.RadixConvert(newid.ToString(), ViewModels.GlobalVariables.DecRadix, ViewModels.GlobalVariables.Base36Redix);
                            //六位表示
                            string ReleasedSequence = r.PadLeft(6, '0');
                            string yymmdd = QueryEx<string>("SELECT TO_CHAR(SYSDATE,'YYMMDD') YYMMDD FROM DUAL ").FirstOrDefault();
                            var INBOUND_NO = "BT" + yymmdd + ReleasedSequence;
                            var resdata = await _dbConnection.ExecuteAsync(insertInboundRecordSql, new
                            {
                                ID = newid,
                                WO_ID = WO_ID,
                                INBOUND_NO = INBOUND_NO,
                                INBOUND_QTY = sclModel.QUANTITY,
                                CREATE_BY = model.OPERATOR,
                                AUTO_EDITOR = isAutoEditor
                            }, tran);
                        }
                    }

                    tran.Commit();
                }
                catch (Exception ex)
                {
                    print_id = 0;
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
            return print_id;
        }

    }
}