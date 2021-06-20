/*
*┌──────────────────────────────────────────────────────────────┐
*│　描    述：接口实现                                                    
*│　作    者：嘉志科技                                            
*│　版    本：2.0    模板代码自动生成                                                
*│　创建时间：2020-07-22 10:16:13                             
*└──────────────────────────────────────────────────────────────┘
*┌──────────────────────────────────────────────────────────────┐
*│　命名空间： JZ.IMS.Repository.Oracle                                  
*│　类    名： MesBurnFileApplyRepository                                      
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
using OfficeOpenXml;
using JZ.IMS.Core.Extensions;
using JZ.IMS.ViewModels.BurnFile;
using JZ.IMS.ViewModels.SPIDevice;
using JZ.IMS.Core.Utilities;

namespace JZ.IMS.Repository.Oracle
{
    public class SPIDeviceRepository : BaseRepository<SmtSpiSrcGen, Decimal>, ISPIDeviceRepository
    {
        public SPIDeviceRepository(IOptionsSnapshot<DbOption> options)
        {
            _dbOption = options.Get("iWMS");
            if (_dbOption == null)
            {
                throw new ArgumentNullException(nameof(DbOption));
            }
            _dbConnection = ConnectionFactory.CreateConnection(_dbOption.DbType, _dbOption.ConnectionString);
        }

        /// <summary>
        /// 通过SN找工单
        /// </summary>
        /// <param name="SN"></param>
        /// <returns></returns>
        public async Task<string> GetWONOBySN(string SN)
        {
            var p = new DynamicParameters();
            string message = string.Empty;
            string trace = string.Empty;
            p.Add(":p_sn", SN);
            p.Add("p_update_ranger_inputted", "Y");
            p.Add("p_message", message, DbType.String, ParameterDirection.Output, 80);//out
            p.Add("p_trace", trace, DbType.String, ParameterDirection.Output, 100);//out
            p.Add("p_language", "EN");
            await _dbConnection.ExecuteScalarAsync<dynamic>("sfcs_substruction_pkg.get_wo_by_ranger19", p, commandType: CommandType.StoredProcedure);
            string wono = p.Get<string>(":p_trace");
            return !wono.IsNullOrWhiteSpace() ? wono : string.Empty;
        }


        /// <summary>
        /// 获取AOI的数据
        /// </summary>
        /// <param name="SN"></param>
        /// <returns></returns>
        public async Task<bool> GetAOIData(SPIModel model)
        {
            bool isOk = false;
            try
            {
                string select_WONO = "select * from SMT_WO where WO_NO=:WO_NO";
                var productionModel = (await _dbConnection.QueryAsync<SmtWo>(select_WONO, new { WO_NO = model.WO_NO })).FirstOrDefault();
                if (productionModel != null)
                {
                    string strConn = $"Server={model.IPAddress};User={model.Account};Password={model.PWD};Database={model.DataBaseName};Connection Timeout=1000;Ss1Mode=None";
                    IDbConnection _mySqlConnection = ConnectionFactory.CreateConnection("MySQL", strConn);
                    string select_cardsql = $"select * from lm400_1_card where id=";//这里需要处理
                    string select_pcbsql = $"select * from lm400_1_pcb where id=";//这里需要处理
                    var cardList = (await _mySqlConnection.QueryAsync<lm4001cardModel>(select_cardsql))?.ToList() ;
                    var pcbModel = (await _mySqlConnection.QueryAsync<lm4001pcbModel>(select_pcbsql)).FirstOrDefault();
                    var spiGen = new SmtSpiSrcGenAddOrModifyModel();
                    var spiDET = new SmtSpiSrcDetAddOrModifyModel();
                    if (cardList != null&& cardList.Count>0)
                    {
                        foreach (var cardModel in cardList)
                        {
                            var ID = Guid.NewGuid().ToString();
                            spiGen.ID = ID;
                            spiDET.ID = Guid.NewGuid().ToString();
                            spiDET.GEN_ID = ID;
                            spiGen.LINE_NO = cardModel.lineid;
                            spiGen.WO = model.WO_NO;
                            spiGen.PART_NO = productionModel.PART_NO;
                            spiGen.SN = cardModel.barcode;
                            spiGen.P_SN = pcbModel.barcode;
                            //检测状态：0:无需判断,1:PASS,2：ng
                            spiGen.TEST_RESULT = cardModel.checkstatus.ToString();
                            spiGen.TEST_TIME = cardModel.starttime;
                            spiGen.DURATION = Convert.ToDecimal((cardModel.endtime - cardModel.starttime).TotalSeconds);
                            spiGen.PASTENUM = cardModel.Pad_Total_Count;
                            spiGen.NGPADCNT = cardModel.Pad_NG_Count;
                            spiGen.RESULTPATH = cardModel.jobfilepath;
                            spiGen.CREATE_TIME = DateTime.Now;
                            spiGen.INVALIDNUM = cardModel.Pad_Total_Count - cardModel.Pad_Ok_Count;
                            spiDET.X_OFFSET = pcbModel.POSITIONXOFFSET.ToString();
                            spiDET.Y_OFFSET = pcbModel.POSITIONYOFFSET.ToString();

                            SmtSpiSrcGenModel genModel = new SmtSpiSrcGenModel();
                            genModel.InsertRecords = new List<SmtSpiSrcGenAddOrModifyModel>();
                            genModel.InsertRecords.Add(spiGen);
                            var genResult = await SaveGENDataByTrans(genModel);

                            SmtSpiSrcDetModel detModel = new SmtSpiSrcDetModel();
                            detModel.InsertRecords = new List<SmtSpiSrcDetAddOrModifyModel>();
                            detModel.InsertRecords.Add(spiDET);
                            var detResult = await SaveDETDataByTrans(detModel);
                            if (genResult != -1 && detResult != -1)
                            {
                                isOk = true;
                            }
                        }
                    }
                  
                }
            }
            catch (Exception ex)
            {

                throw new Exception(ex.Message);
            }
            return isOk;
        }

        /// <summary>
        /// 保存SPIGEN数据
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public async Task<decimal> SaveGENDataByTrans(SmtSpiSrcGenModel model)
        {
            int result = 1;
            ConnectionFactory.OpenConnection(_dbConnection);
            using (var tran = _dbConnection.BeginTransaction())
            {
                try
                {
                    //新增
                    string insertSql = @"insert into SMT_SPI_SRC_GEN 
					(ID,LINE_NO,WO,MO,PART_NO,P_SN,SN,BOARD_SEQ,TEST_RESULT,REVISE_RESULT,PROGRAM_NAME,TEST_TIME,CREATE_TIME,DURATION,PASTENUM,INVALIDNUM,NGPADCNT,RESULTPATH,RESERVED1BYSHORT,RESERVED2BYSHORT,RESERVED1BYINT,RESERVED1BYCHAR,AVGVPER,AVGAPER,AVGHEIGHT,AVGSHIFTX,AVGSHIFTY) 
					VALUES (:ID,:LINE_NO,:WO,:MO,:PART_NO,:P_SN,:SN,:BOARD_SEQ,:TEST_RESULT,:REVISE_RESULT,:PROGRAM_NAME,:TEST_TIME,:CREATE_TIME,:DURATION,:PASTENUM,:INVALIDNUM,:NGPADCNT,:RESULTPATH,:RESERVED1BYSHORT,:RESERVED2BYSHORT,:RESERVED1BYINT,:RESERVED1BYCHAR,:AVGVPER,:AVGAPER,:AVGHEIGHT,:AVGSHIFTX,:AVGSHIFTY)";
                    if (model.InsertRecords != null && model.InsertRecords.Count > 0)
                    {
                        foreach (var item in model.InsertRecords)
                        {

                            var resdata = await _dbConnection.ExecuteAsync(insertSql, new
                            {
                                item.ID,
                                item.LINE_NO,
                                item.WO,
                                item.MO,
                                item.PART_NO,
                                item.P_SN,
                                item.SN,
                                item.BOARD_SEQ,
                                item.TEST_RESULT,
                                item.REVISE_RESULT,
                                item.PROGRAM_NAME,
                                item.TEST_TIME,
                                item.CREATE_TIME,
                                item.DURATION,
                                item.PASTENUM,
                                item.INVALIDNUM,
                                item.NGPADCNT,
                                item.RESULTPATH,
                                item.RESERVED1BYSHORT,
                                item.RESERVED2BYSHORT,
                                item.RESERVED1BYINT,
                                item.RESERVED1BYCHAR,
                                item.AVGVPER,
                                item.AVGAPER,
                                item.AVGHEIGHT,
                                item.AVGSHIFTX,
                                item.AVGSHIFTY,
                            }, tran);
                        }
                    }
                    //更新
                    string updateSql = @"Update SMT_SPI_SRC_GEN set LINE_NO=:LINE_NO,WO=:WO,MO=:MO,PART_NO=:PART_NO,P_SN=:P_SN,SN=:SN,BOARD_SEQ=:BOARD_SEQ,TEST_RESULT=:TEST_RESULT,REVISE_RESULT=:REVISE_RESULT,PROGRAM_NAME=:PROGRAM_NAME,TEST_TIME=:TEST_TIME,CREATE_TIME=:CREATE_TIME,DURATION=:DURATION,PASTENUM=:PASTENUM,INVALIDNUM=:INVALIDNUM,NGPADCNT=:NGPADCNT,RESULTPATH=:RESULTPATH,RESERVED1BYSHORT=:RESERVED1BYSHORT,RESERVED2BYSHORT=:RESERVED2BYSHORT,RESERVED1BYINT=:RESERVED1BYINT,RESERVED1BYCHAR=:RESERVED1BYCHAR,AVGVPER=:AVGVPER,AVGAPER=:AVGAPER,AVGHEIGHT=:AVGHEIGHT,AVGSHIFTX=:AVGSHIFTX,AVGSHIFTY=:AVGSHIFTY  
						where ID=:ID ";
                    if (model.UpdateRecords != null && model.UpdateRecords.Count > 0)
                    {
                        foreach (var item in model.UpdateRecords)
                        {
                            var resdata = await _dbConnection.ExecuteAsync(updateSql, new
                            {
                                item.ID,
                                item.LINE_NO,
                                item.WO,
                                item.MO,
                                item.PART_NO,
                                item.P_SN,
                                item.SN,
                                item.BOARD_SEQ,
                                item.TEST_RESULT,
                                item.REVISE_RESULT,
                                item.PROGRAM_NAME,
                                item.TEST_TIME,
                                item.CREATE_TIME,
                                item.DURATION,
                                item.PASTENUM,
                                item.INVALIDNUM,
                                item.NGPADCNT,
                                item.RESULTPATH,
                                item.RESERVED1BYSHORT,
                                item.RESERVED2BYSHORT,
                                item.RESERVED1BYINT,
                                item.RESERVED1BYCHAR,
                                item.AVGVPER,
                                item.AVGAPER,
                                item.AVGHEIGHT,
                                item.AVGSHIFTX,
                                item.AVGSHIFTY,
                            }, tran);
                        }
                    }
                    //删除
                    string deleteSql = @"Delete from SMT_SPI_SRC_GEN where ID=:ID ";
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
        /// 保存SPIDET数据
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public async Task<decimal> SaveDETDataByTrans(SmtSpiSrcDetModel model)
        {
            int result = 1;
            ConnectionFactory.OpenConnection(_dbConnection);
            using (var tran = _dbConnection.BeginTransaction())
            {
                try
                {
                    //新增
                    string insertSql = @"insert into SMT_SPI_SRC_DET 
					(ID,GEN_ID,SEQ,LOCATION,ITEM_NO,AREA,HEIGHT,VOLUME,AREA_PEC,HEIGHT_PEC,VOLUME_PEC,X_OFFSET,Y_OFFSET,X_PADSIZE,Y_PADSIZE,RESULT,ERRCODE,PIN_NUM) 
					VALUES (:ID,:GEN_ID,:SEQ,:LOCATION,:ITEM_NO,:AREA,:HEIGHT,:VOLUME,:AREA_PEC,:HEIGHT_PEC,:VOLUME_PEC,:X_OFFSET,:Y_OFFSET,:X_PADSIZE,:Y_PADSIZE,:RESULT,:ERRCODE,:PIN_NUM)";
                    if (model.InsertRecords != null && model.InsertRecords.Count > 0)
                    {
                        foreach (var item in model.InsertRecords)
                        {
                            //var ID = Guid.NewGuid().ToString();
                            var resdata = await _dbConnection.ExecuteAsync(insertSql, new
                            {
                                item.ID,
                                item.GEN_ID,
                                item.SEQ,
                                item.LOCATION,
                                item.ITEM_NO,
                                item.AREA,
                                item.HEIGHT,
                                item.VOLUME,
                                item.AREA_PEC,
                                item.HEIGHT_PEC,
                                item.VOLUME_PEC,
                                item.X_OFFSET,
                                item.Y_OFFSET,
                                item.X_PADSIZE,
                                item.Y_PADSIZE,
                                item.RESULT,
                                item.ERRCODE,
                                item.PIN_NUM,
                            }, tran);
                        }
                    }
                    //更新
                    string updateSql = @"Update SMT_SPI_SRC_DET set GEN_ID=:GEN_ID,SEQ=:SEQ,LOCATION=:LOCATION,ITEM_NO=:ITEM_NO,AREA=:AREA,HEIGHT=:HEIGHT,VOLUME=:VOLUME,AREA_PEC=:AREA_PEC,HEIGHT_PEC=:HEIGHT_PEC,VOLUME_PEC=:VOLUME_PEC,X_OFFSET=:X_OFFSET,Y_OFFSET=:Y_OFFSET,X_PADSIZE=:X_PADSIZE,Y_PADSIZE=:Y_PADSIZE,RESULT=:RESULT,ERRCODE=:ERRCODE,PIN_NUM=:PIN_NUM  
						where ID=:ID ";
                    if (model.UpdateRecords != null && model.UpdateRecords.Count > 0)
                    {
                        foreach (var item in model.UpdateRecords)
                        {
                            var resdata = await _dbConnection.ExecuteAsync(updateSql, new
                            {
                                item.ID,
                                item.GEN_ID,
                                item.SEQ,
                                item.LOCATION,
                                item.ITEM_NO,
                                item.AREA,
                                item.HEIGHT,
                                item.VOLUME,
                                item.AREA_PEC,
                                item.HEIGHT_PEC,
                                item.VOLUME_PEC,
                                item.X_OFFSET,
                                item.Y_OFFSET,
                                item.X_PADSIZE,
                                item.Y_PADSIZE,
                                item.RESULT,
                                item.ERRCODE,
                                item.PIN_NUM,
                            }, tran);
                        }
                    }
                    //删除
                    string deleteSql = @"Delete from SMT_SPI_SRC_DET where ID=:ID ";
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
    }
}