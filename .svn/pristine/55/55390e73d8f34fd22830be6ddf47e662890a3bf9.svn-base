/*
*┌──────────────────────────────────────────────────────────────┐
*│　描    述：接口实现                                                    
*│　作    者：嘉志科技                                            
*│　版    本：2.0    模板代码自动生成                                                
*│　创建时间：2020-04-06 16:48:13                             
*└──────────────────────────────────────────────────────────────┘
*┌──────────────────────────────────────────────────────────────┐
*│　命名空间： JZ.IMS.Repository.Oracle                                  
*│　类    名： SmtPlacementHeaderRepository                                      
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
using JZ.IMS.ViewModels.SmtLineSet;

namespace JZ.IMS.Repository.Oracle
{
    /// <summary>
    /// 料单管理
    /// </summary>
    public class SmtPlacementHeaderRepository : BaseRepository<SmtPlacementHeader, Decimal>, ISmtPlacementHeaderRepository
    {
        public SmtPlacementHeaderRepository(IOptionsSnapshot<DbOption> options)
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
            string sql = "SELECT SEQ_SMT_PLACEMENT_HEADER.NEXTVAL MY_SEQ FROM DUAL";
            var result = await _dbConnection.ExecuteScalarAsync(sql);
            return (decimal)result;
        }

        public async Task<decimal> GetSEQ_SMT_PLACEMENT_DETAIL()
        {
            string sql = "SELECT SEQ_SMT_PLACEMENT_DETAIL.NEXTVAL MY_SEQ FROM DUAL";
            var result = await _dbConnection.ExecuteScalarAsync(sql);
            return (decimal)result;
        }

        /// <summary>
        ///获取AI机台列表
        /// </summary>
        /// <returns></returns>
        public async Task<List<IDNAME>> GetStationList(string Type)
        {
            string sql = @"SELECT S.ID, smt_name NAME 
                             FROM SMT_STATION S, SMT_STATION_CONFIG C, SMT_LOOKUP L
                           WHERE S.ID = C.STATION_ID
                             AND C.CONFIG_TYPE = L.CODE
	                         AND L.TYPE ='MACHINECONFIG'
	                         AND L.VALUE ='MachineGroup'
	                         AND UPPER(C.VALUE)=:TYPE";
            var result = await _dbConnection.QueryAsync<IDNAME>(sql,new { TYPE = Type });
            return result?.ToList();
        }

        /// <summary>
        ///获取AI机台列表
        /// </summary>
        /// <returns></returns>
        public async Task<List<dynamic>> GetStationListBySIEMENS(string Type)
        {
            string sql = @"SELECT L.LINE_NAME,L.ID LINEID,S.SMT_NAME,S.ID STATIONID FROM SMT_LINES L,SMT_ROUTE SR,SMT_STATION S,SMT_STATION_CONFIG C,SMT_LOOKUP LP
                            WHERE L.ID=SR.LINE_ID AND SR.STATION_ID=S.ID  AND S.ID = C.STATION_ID
                             AND C.CONFIG_TYPE = LP.CODE   
							 AND LP.TYPE ='MACHINECONFIG'
                             AND LP.VALUE ='MachineGroup'
							 AND UPPER(C.VALUE)=:TYPE";
            var result = await _dbConnection.QueryAsync<dynamic>(sql, new { TYPE = Type });
            return result?.ToList();
        }

        /// <summary>
        /// 获取料单主表列表
        /// </summary>
        /// <param name="model">model</param>
        /// <returns></returns>
        public async Task<TableDataModel> GetSmtPlacementHeaderList(SmtPlacementHeaderRequestModel model)
        {
            string condition = " ";
            if (!model.PART_NO.IsNullOrWhiteSpace())
            {
                condition += $" and (SPH.PART_NO =:PART_NO) ";
            }
            if (!model.PLACEMENT.IsNullOrWhiteSpace())
            {
                condition += $" and (instr(SPH.PLACEMENT, :PLACEMENT) > 0) ";
            }
            if (model.STATION_ID != null && model.STATION_ID > 0)
            {
                condition += $" and (SS.ID =:STATION_ID) ";
            }

            string sql = @"SELECT ROW_NUMBER() OVER(ORDER BY SPH.UPDATE_TIME DESC) AS ROWNO, SPH.ID,
                                 SPH.PLACEMENT,
                                 SPH.PART_NO,
                                 L.CN_DESC PCB_SIDE,
                                 SPH.DESCRIPTION,
                                 SPH.ENABLED,
                                 SPH.CHECKED,
                                 SPH.CHECKED_BY,
                                 SPH.CHECKED_TIME,
                                 SPH.HI_OUTPUT_TIME,
                                 SPH.PCB_ROUTE_CODE,
                                 SPH.STANDARD_CAPACITY,
                                 SS.SMT_NAME,
                                 SPH.CREATE_BY,
                                 SPH.CREATE_TIME,
		                         SPH.UPDATE_BY,
		                         SPH.UPDATE_TIME
                          FROM   SMT_PLACEMENT_HEADER SPH,
                                 SMT_STATION SS,
                                 SMT_LOOKUP L 
                         WHERE   SS.ID = SPH.STATION_ID 
                                 AND L.TYPE = 'PCB_SIDE' 
                                 AND L.CODE = SPH.PCB_SIDE 
                                 AND SS.ENABLED = 'Y' 
                                 AND SPH.ENABLED ='Y' ";
            string pagedSql = SQLBuilderClass.GetPagedSQL(sql, "SPH.UPDATE_TIME DESC", condition);
            var resdata = await _dbConnection.QueryAsync<SmtPlacementHeaderListModel>(pagedSql, model);

            string sqlcnt = @"select count(0) FROM SMT_PLACEMENT_HEADER SPH,
                                     SMT_STATION SS,
                                     SMT_LOOKUP L 
                              where  SS.ID = SPH.STATION_ID 
                                     AND L.TYPE = 'PCB_SIDE' 
                                     AND L.CODE = SPH.PCB_SIDE 
                                     AND SS.ENABLED = 'Y' 
                                     AND SPH.ENABLED ='Y' " + condition;

            int cnt = await _dbConnection.ExecuteScalarAsync<int>(sqlcnt, model);

            return new TableDataModel
            {
                count = cnt,
                data = resdata?.ToList(),
            };
        }

        /// <summary>
        ///  料单编辑之保存数据
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public async Task<decimal> SaveDataByTrans(SmtPlacementSaveModel model)
        {
            int result = 1;
            ConnectionFactory.OpenConnection(_dbConnection);
            using (var tran = _dbConnection.BeginTransaction())
            {
                try
                {
                    //主表更新
                    DateTime currentTime = _dbConnection.QueryFirstOrDefault<DateTime>("SELECT SYSDATE FROM DUAL");
                    decimal? standardCapacity = Convert.ToDecimal(model.PlacementHeader.STANDARD_CAPACITY);
                    SmtPlacementHeader smtPlacementHeader = await _dbConnection.GetAsync<SmtPlacementHeader>(model.PlacementHeader.ID);
                    smtPlacementHeader.PLACEMENT = model.PlacementHeader.PLACEMENT;
                    smtPlacementHeader.PART_NO = model.PlacementHeader.PART_NO;
                    smtPlacementHeader.PCB_SIDE = model.PlacementHeader.PCB_SIDE;
                    smtPlacementHeader.DESCRIPTION = model.PlacementHeader.DESCRIPTION;
                    smtPlacementHeader.ENABLED = model.PlacementHeader.ENABLED;
                    smtPlacementHeader.CHECKED = model.PlacementHeader.CHECKED;
                    smtPlacementHeader.CHECKED_BY = model.PlacementHeader.CHECKED_BY;
                    smtPlacementHeader.CHECKED_TIME = model.PlacementHeader.CHECKED_TIME;
                    smtPlacementHeader.UPDATE_BY = model.PlacementHeader.UPDATE_BY;
                    smtPlacementHeader.UPDATE_TIME = currentTime;
                    smtPlacementHeader.STATION_ID = model.PlacementHeader.STATION_ID;
                    smtPlacementHeader.STANDARD_CAPACITY = standardCapacity;
                    await _dbConnection.UpdateAsync<SmtPlacementHeader>(smtPlacementHeader, tran);
                    //明细新增
                    string insertSql = @"insert into SMT_PLACEMENT_DETAIL 
					(ID,MST_ID,TABLENO,STAGE,SLOT,SUB_SLOT,LOCATION,PART_NO,UNITQTY,PNDESC,FEEDERTYPE,REFDESIGNATOR,ENABLED,SKIP,LOCATION_KEY,CREATE_TIME,CREATE_BY,UPDATE_TIME,UPDATE_BY) 
					VALUES (SEQ_SMT_PLACEMENT_DETAIL.NEXTVAL,:MST_ID,:TABLENO,:STAGE,:SLOT,:SUB_SLOT,:LOCATION,:PART_NO,:UNITQTY,:PNDESC,:FEEDERTYPE,
                        :REFDESIGNATOR,:ENABLED,:SKIP,:LOCATION_KEY,SYSDATE,:CREATE_BY,SYSDATE,:UPDATE_BY)";
                    if (model.PlacementDetail.InsertRecords != null && model.PlacementDetail.InsertRecords.Count > 0)
                    {
                        foreach (var item in model.PlacementDetail.InsertRecords)
                        {
                            string stationNo = null;
                            var stationConfigTable = await GetAsyncEx<SmtStationConfig>("where CONFIG_TYPE='113' AND STATION_ID=:STATION_ID", new { STATION_ID = model.PlacementHeader.STATION_ID });
                            if (stationConfigTable != null)
                            {
                                stationNo = stationConfigTable.VALUE;
                            }
                            else
                            {
                                throw new Exception("请先配置机台序列号！");
                            }
                            item.LOCATION = string.Format("{0}-{1}{2}", stationNo.PadLeft(3, '0'), item.SLOT, item.SUB_SLOT.IsNullOrEmpty()?"": item.SUB_SLOT.Trim());
                            var resdata = await _dbConnection.ExecuteAsync(insertSql, new
                            {
                                MST_ID = model.PlacementHeader.ID,
                                item.TABLENO,
                                item.STAGE,
                                item.SLOT,
                                item.SUB_SLOT,
                                item.LOCATION,
                                item.PART_NO,
                                item.UNITQTY,
                                item.PNDESC,
                                item.FEEDERTYPE,
                                item.REFDESIGNATOR,
                                item.ENABLED,
                                item.SKIP,
                                item.LOCATION_KEY,
                                CREATE_BY = model.PlacementHeader.UPDATE_BY,
                                model.PlacementHeader.UPDATE_BY,

                            }, tran);
                        }
                    }
                    //更新
                    string updateSql = @"Update SMT_PLACEMENT_DETAIL set TABLENO=:TABLENO,STAGE=:STAGE,SLOT=:SLOT,SUB_SLOT=:SUB_SLOT,
                            LOCATION=:LOCATION,PART_NO=:PART_NO,UNITQTY=:UNITQTY,PNDESC=:PNDESC,FEEDERTYPE=:FEEDERTYPE,REFDESIGNATOR=:REFDESIGNATOR,
                            ENABLED=:ENABLED,SKIP=:SKIP,LOCATION_KEY=:LOCATION_KEY,UPDATE_TIME=SYSDATE,UPDATE_BY=:UPDATE_BY  
						where ID=:ID ";
                    if (model.PlacementDetail.UpdateRecords != null && model.PlacementDetail.UpdateRecords.Count > 0)
                    {
                        foreach (var item in model.PlacementDetail.UpdateRecords)
                        {
                            string stationNo = null;
                            var stationConfigTable = await GetAsyncEx<SmtStationConfig>("where CONFIG_TYPE='113' AND STATION_ID=:STATION_ID", new { STATION_ID = model.PlacementHeader.STATION_ID });
                            if (stationConfigTable != null)
                            {
                                stationNo = stationConfigTable.VALUE;
                            }
                            else
                            {
                                throw new Exception("请先配置机台序列号！");
                            }
                            item.LOCATION = string.Format("{0}-{1}{2}", stationNo.PadLeft(3, '0'), item.SLOT, item.SUB_SLOT.IsNullOrEmpty() ? "" : item.SUB_SLOT.Trim());
                            var resdata = await _dbConnection.ExecuteAsync(updateSql, new
                            {
                                item.ID,
                                item.TABLENO,
                                item.STAGE,
                                item.SLOT,
                                item.SUB_SLOT,
                                item.LOCATION,
                                item.PART_NO,
                                item.UNITQTY,
                                item.PNDESC,
                                item.FEEDERTYPE,
                                item.REFDESIGNATOR,
                                item.ENABLED,
                                item.SKIP,
                                item.LOCATION_KEY,
                                model.PlacementHeader.UPDATE_BY,
                            }, tran);
                        }
                    }
                    //删除
                    string deleteSql = @"Delete from SMT_PLACEMENT_DETAIL where ID=:ID ";
                    if (model.PlacementDetail.RemoveRecords != null && model.PlacementDetail.RemoveRecords.Count > 0)
                    {
                        foreach (var item in model.PlacementDetail.RemoveRecords)
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
        ///  AI料单上传之保存数据
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public async Task<AIPlacementSaveResult> AIPlacementSave(PlacementSaveModel placementModel)
        {
            AIPlacementSaveResult result = new AIPlacementSaveResult();
            ConnectionFactory.OpenConnection(_dbConnection);
            using (var tran = _dbConnection.BeginTransaction())
            {
                try
                {
                    foreach (var model in placementModel.InsertRecords)
                    {
                        if (model.DataList == null || model.DataList.Count == 0)
                        {
                            result.result = -1;
                            continue;
                        }

                        foreach (var station in model.Stations)
                        {
                            //生成明细
                            List<SmtPlacementDetail> detailList = new List<SmtPlacementDetail>();
                            foreach (var item in model.DataList)
                            {
                                string stage = item.STAGE;
                                stage = stage.IsNullOrWhiteSpace() ? "1" : stage;
                                string slot = stage + item.SLOT.PadLeft(4, '0');
                                string subSlot = item.SUBSLOT.IsNullOrWhiteSpace() ? "" : item.SUBSLOT;
                                string stationNo = null;
                                var stationConfigTable = await GetAsyncEx<SmtStationConfig>("where CONFIG_TYPE='113' AND STATION_ID=:STATION_ID", new { STATION_ID = station });
                                if (stationConfigTable != null)
                                {
                                    stationNo = stationConfigTable.VALUE;
                                }
                                else
                                {
                                    result.result = -1;
                                    result.StationIsConfig = 0;
                                    tran.Rollback();
                                    return result;
                                    //throw new Exception("请先配置机台序列号！");
                                }
                                
                                SmtPlacementDetail placementDetail = new SmtPlacementDetail
                                {
                                    ID = await GetSEQ_SMT_PLACEMENT_DETAIL(),
                                    VERSION = 1,
                                    //MST_ID = newid,
                                    STAGE = stage,
                                    SLOT = slot,
                                    SUB_SLOT = subSlot,
                                    LOCATION = string.Format("{0}-{1}{2}", stationNo.PadLeft(3, '0'), slot, subSlot.Trim()),
                                    LOCATION_KEY = item.LOCATION_KEY,
                                    PART_NO = item.PART_NO.Trim(),
                                    UNITQTY = item.UNITQTY,
                                    PNDESC = item.DESCRIPTION,
                                    FEEDERTYPE = item.FEEDER_TYPE,
                                    REFDESIGNATOR = item.REFDESIGNATOR,
                                    ENABLED = GlobalVariables.EnableY,
                                    SKIP = GlobalVariables.EnableN,
                                    CREATE_BY = model.User_name,
                                    CREATE_TIME = DateTime.Now,
                                    UPDATE_TIME = DateTime.Now,
                                    UPDATE_BY = model.User_name,
                                };
                                detailList.Add(placementDetail);
                            }
                            //比较新旧料单
                            var res = await CompareNewAndOldPlacement(model, station, detailList);
                            if (res.PlacementHeaderID > 0)
                            {
                                if (res.UseReel != null)
                                {
                                    result.result = -1;
                                    result.UseReel = res.UseReel;
                                    tran.Rollback();
                                    return result;
                                }
                                else
                                {
                                    UpdatePlacement(res, model, detailList, station, tran);
                                }
                            }
                            else
                            {
                                //新增主表
                                decimal newid = await GetSEQID();
                                SmtPlacementHeader header = new SmtPlacementHeader
                                {
                                    ID = newid,
                                    PLACEMENT = model.Placement,
                                    STATION_ID = station,
                                    PART_NO = model.Part_NO,
                                    PCB_SIDE = model.PCB_SIDE,
                                    ENABLED = "Y",
                                    CREATE_BY = model.User_name,
                                    CREATE_TIME = DateTime.Now,
                                    HI_OUTPUT_TIME = 1,
                                    STANDARD_CAPACITY = 0,
                                    UPDATE_BY = model.User_name,
                                    UPDATE_TIME = DateTime.Now,
                                };
                                await _dbConnection.InsertAsync<SmtPlacementHeader>(header, tran);
                                //新增明细
                                foreach (var item in detailList)
                                {
                                    item.MST_ID = newid;
                                    await _dbConnection.InsertAsync<SmtPlacementDetail>(item, tran);
                                }
                            }
                        }

                    }

                    tran.Commit();
                }
                catch (Exception ex)
                {
                    result.result = -1;
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
        ///  Siemens料单上传之保存数据
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public async Task<AIPlacementSaveResult> SiemensPlacementSave(PlacementSaveModel placementModel)
        {
            AIPlacementSaveResult result = new AIPlacementSaveResult();
            ConnectionFactory.OpenConnection(_dbConnection);
            using (var tran = _dbConnection.BeginTransaction())
            {
                try
                {
                    foreach (var model in placementModel.InsertRecords)
                    {
                        if (model.DataList == null || model.DataList.Count == 0)
                        {
                            result.result = -1;
                            continue;
                        }

                        foreach (var station in model.Stations)
                        {
                            //生成明细
                            List<SmtPlacementDetail> detailList = new List<SmtPlacementDetail>();
                            foreach (var item in model.DataList)
                            {
                                if (item.StationID==station.ToString())
                                {
                                    string stage = item.STAGE;
                                    stage = stage.IsNullOrWhiteSpace() ? "1" : stage;
                                    string slot = stage + item.SLOT.PadLeft(4, '0');
                                    string subSlot = item.SUBSLOT.IsNullOrWhiteSpace() ? "" : item.SUBSLOT;
                                    string stationNo = null;
                                    var stationConfigTable = await GetAsyncEx<SmtStationConfig>("where CONFIG_TYPE='113' AND STATION_ID=:STATION_ID", new { STATION_ID = station });
                                    if (stationConfigTable != null)
                                    {
                                        stationNo = stationConfigTable.VALUE;
                                    }
                                    else
                                    {
                                        result.result = -1;
                                        result.StationIsConfig = 0;
                                        tran.Rollback();
                                        return result;
                                        //throw new Exception("请先配置机台序列号！");
                                    }

                                    SmtPlacementDetail placementDetail = new SmtPlacementDetail
                                    {
                                        //ID = await GetSEQ_SMT_PLACEMENT_DETAIL(),
                                        VERSION = 1,
                                        //MST_ID = newid,
                                        STAGE = stage,
                                        SLOT = slot,
                                        SUB_SLOT = subSlot,
                                        LOCATION = string.Format("{0}-{1}{2}", stationNo.PadLeft(3, '0'), slot, subSlot.Trim()),
                                        PART_NO = item.PART_NO.Trim(),
                                        UNITQTY = item.UNITQTY,
                                        PNDESC = item.DESCRIPTION,
                                        FEEDERTYPE = item.FEEDER_TYPE,
                                        REFDESIGNATOR = item.REFDESIGNATOR,
                                        ENABLED = GlobalVariables.EnableY,
                                        SKIP = GlobalVariables.EnableN,
                                        CREATE_BY = model.User_name,
                                        CREATE_TIME = DateTime.Now,
                                        UPDATE_TIME = DateTime.Now,
                                        UPDATE_BY = model.User_name,
                                    };
                                    detailList.Add(placementDetail);
                                }
                                
                            }
                            //比较新旧料单
                            var res = await CompareNewAndOldPlacement(model, station, detailList);
                            if (res.PlacementHeaderID > 0)
                            {
                                if (res.UseReel != null)
                                {
                                    result.result = -1;
                                    result.UseReel = res.UseReel;
                                    tran.Rollback();
                                    return result;
                                }
                                else
                                {
                                    UpdatePlacement(res, model, detailList, station, tran);
                                }
                            }
                            else
                            {
                                //新增主表
                                decimal newid = await GetSEQID();
                                SmtPlacementHeader header = new SmtPlacementHeader
                                {
                                    ID = newid,
                                    PLACEMENT = model.Placement,
                                    STATION_ID = station,
                                    VERSION = 1,
                                    PART_NO = model.Part_NO,
                                    PCB_SIDE = model.PCB_SIDE,
                                    ENABLED = "Y",
                                    CREATE_BY = model.User_name,
                                    CREATE_TIME = DateTime.Now,
                                    HI_OUTPUT_TIME = 1,
                                    STANDARD_CAPACITY = 0,
                                    UPDATE_BY = model.User_name,
                                    UPDATE_TIME = DateTime.Now,
                                };
                                await _dbConnection.InsertAsync<SmtPlacementHeader>(header, tran);
                                //新增明细
                                foreach (var item in detailList)
                                {
                                    item.ID = await GetSEQ_SMT_PLACEMENT_DETAIL();
                                    item.MST_ID = newid;
                                    item.VERSION = 1;
                                    await _dbConnection.InsertAsync<SmtPlacementDetail>(item, tran);
                                }
                            }
                        }

                    }

                    tran.Commit();
                }
                catch (Exception ex)
                {
                    result.result = -1;
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
        //同步跟新料单内容 chenhx 2020/11/24
        public async void UpdatePlacement(CompareResult res, AIPlacementSaveModel model,
            List<SmtPlacementDetail> detailList , Decimal station, System.Data.IDbTransaction tran)
        {
            //旧料单有的，新料单没有
            List<SmtPlacementDetail> deleteSmtPlacementDetails = new List<SmtPlacementDetail>();
            //旧料单没有的，新料单有
            List<SmtPlacementDetail> addSmtPlacementDetails = new List<SmtPlacementDetail>();
            //旧料单有的，新料单有的
            List<SmtPlacementDetail> updateSmtPlacementDetails = new List<SmtPlacementDetail>();
            //获取旧料单
            var olddetailList = (await GetListAsyncEx<SmtPlacementDetail>("Where MST_ID=:ID", new { ID = res.PlacementHeaderID }))?.ToList();
            if (olddetailList != null && olddetailList.Count() > 0)
            {
                foreach (SmtPlacementDetail oldPlacementDetail in olddetailList)
                {
                    var compareDetailRow = detailList.Where(f => f.STAGE == oldPlacementDetail.STAGE && f.SLOT == oldPlacementDetail.SLOT && f.SUB_SLOT == oldPlacementDetail.SUB_SLOT
                                         && f.PART_NO == oldPlacementDetail.PART_NO).FirstOrDefault();
                    if (compareDetailRow == null)
                    {
                        deleteSmtPlacementDetails.Add(oldPlacementDetail);
                    }
                    else
                    {
                        compareDetailRow.ID = oldPlacementDetail.ID;
                        compareDetailRow.VERSION = res.VERSION + 1;
                        compareDetailRow.MST_ID = oldPlacementDetail.MST_ID;
                        updateSmtPlacementDetails.Add(compareDetailRow);
                    }
                }
                var newDetailRows = detailList.Where(f => f.ID == 0 || f.MST_ID == null);
                if (newDetailRows != null && newDetailRows.Count() > 0)
                {
                    foreach (SmtPlacementDetail newPlacementDetail in newDetailRows)
                    {
                        newPlacementDetail.ID = await GetSEQ_SMT_PLACEMENT_DETAIL();
                        newPlacementDetail.MST_ID = res.PlacementHeaderID;
                        newPlacementDetail.VERSION = res.VERSION + 1;
                        addSmtPlacementDetails.Add(newPlacementDetail);
                    }
                }

            }
            else
            {
                foreach (SmtPlacementDetail smtPlacementDetail in detailList)
                {
                    smtPlacementDetail.ID = await GetSEQ_SMT_PLACEMENT_DETAIL();
                    smtPlacementDetail.MST_ID = res.PlacementHeaderID;
                    smtPlacementDetail.VERSION = res.VERSION + 1;
                    addSmtPlacementDetails.Add(smtPlacementDetail);
                }
            }


            //新增log记录
            string placementHeadLog = @"INSERT INTO JZMES_LOG.SMT_PLACEMENT_HEADER 
                                        (SELECT  SPH.* , sysdate as DELETE_TIME,  :DELETE_BY as DELETE_BY  from JZMES.SMT_PLACEMENT_HEADER SPH where SPH.ID = :ID)";
            await _dbConnection.ExecuteAsync(placementHeadLog, new
            {
                DELETE_BY = model.User_name,
                ID = res.PlacementHeaderID
            }, tran);
            string placementDetailLog = @"INSERT INTO JZMES_LOG.SMT_PLACEMENT_DETAIL 
                                        (select SPD.*,sysdate as DELETE_TIME,  :DELETE_BY as DELETE_BY from JZMES.SMT_PLACEMENT_DETAIL SPD  where SPD.MST_ID = :ID)";
            await _dbConnection.ExecuteAsync(placementDetailLog, new
            {
                DELETE_BY = model.User_name,
                ID = res.PlacementHeaderID
            }, tran);
            //更新料单主表
            SmtPlacementHeader header = new SmtPlacementHeader
            {
                ID = res.PlacementHeaderID,
                PLACEMENT = model.Placement,
                STATION_ID = station,
                VERSION = res.VERSION + 1,
                PART_NO = model.Part_NO,
                PCB_SIDE = model.PCB_SIDE,
                ENABLED = "Y",
                CREATE_BY = model.User_name,
                CREATE_TIME = DateTime.Now,
                HI_OUTPUT_TIME = 1,
                STANDARD_CAPACITY = 0,
                UPDATE_BY = model.User_name,
                UPDATE_TIME = DateTime.Now,
            };
            await _dbConnection.UpdateAsync<SmtPlacementHeader>(header, tran);
            //更新料单子表
            if (deleteSmtPlacementDetails != null && deleteSmtPlacementDetails.Count > 0)
            {
                foreach (SmtPlacementDetail deletePlacement in deleteSmtPlacementDetails)
                {
                    await _dbConnection.DeleteAsync<SmtPlacementDetail>(deletePlacement.ID, tran);
                }

            }
            if (addSmtPlacementDetails != null && addSmtPlacementDetails.Count > 0)
            {
                foreach (SmtPlacementDetail addPlacement in addSmtPlacementDetails)
                {
                    await _dbConnection.InsertAsync<SmtPlacementDetail>(addPlacement, tran);
                }
            }

            if (updateSmtPlacementDetails != null && updateSmtPlacementDetails.Count > 0)
            {
                foreach (SmtPlacementDetail updatePlacement in updateSmtPlacementDetails)
                {
                    await _dbConnection.UpdateAsync<SmtPlacementDetail>(updatePlacement, tran);
                }
            }
        }

        public class CompareResult
        {
            /// <summary>
            /// 找到的旧的主表ID
            /// </summary>
            public decimal PlacementHeaderID { get; set; }

            //版本
            public Decimal? VERSION { get; set; }

            /// <summary>
            /// 在使用的料单明细
            /// </summary>
            public UseReelInfo UseReel { get; set; }
        }

        /// <summary>
        /// 比較新旧料单
        /// </summary>
        /// <param name="model"></param>
        /// <param name="station">机台</param>
        /// <param name="detailList">新的料单明细</param>
        /// <returns></returns>
        private async Task<CompareResult> CompareNewAndOldPlacement(AIPlacementSaveModel model, decimal station, List<SmtPlacementDetail> detailList)
        {
            CompareResult result = new CompareResult();
            List<SmtPlacementDetail> onlyOldHaveList = new List<SmtPlacementDetail>();
            //获取旧料单Header
            string conditions = @" WHERE Enabled ='Y' AND PART_NO =:PART_NO AND PCB_SIDE =:PCB_SIDE AND STATION_ID =:STATION_ID";
            var smtPlacementHeader = await GetAsyncEx<SmtPlacementHeader>(conditions, new
            {
                PART_NO = model.Part_NO,
                model.PCB_SIDE,
                STATION_ID = station,
            });

            //上傳的是全新的料單，則直接將新料單的數據提交
            if (smtPlacementHeader == null)
            {
                result.PlacementHeaderID = -1;
            }
            else
            {
                result.PlacementHeaderID = smtPlacementHeader.ID;
                result.VERSION = smtPlacementHeader.VERSION==null ? 1: smtPlacementHeader.VERSION;
                //找到明细
                var olddetailList = (await GetListAsyncEx<SmtPlacementDetail>("Where MST_ID=:ID", new { ID = smtPlacementHeader.ID }))?.ToList();
                if (olddetailList != null && olddetailList.Count > 0)
                {
                    //以旧Detail為标准匹配新Detail  (model.DataList)
                    foreach (var old_item in olddetailList)
                    {
                        var newDetailRow = detailList.Where(f => f.STAGE == old_item.STAGE && f.SLOT == old_item.SLOT && f.SUB_SLOT == old_item.SUB_SLOT
                                                                 && f.PART_NO == old_item.PART_NO).FirstOrDefault();
                        if (newDetailRow == null)
                        {
                            ////如果旧的有，新的沒有，該行Detail加入onlyOldHaveList 
                            onlyOldHaveList.Add(old_item);
                        }
                    }
                }

                if (onlyOldHaveList.Count > 0)
                {
                    //检查当前的Detail是否可以已使用
                    foreach (var item in onlyOldHaveList)
                    {
                        var resdata = await CheckDetailIsUse(item);
                        if (resdata != null)
                        {
                            result.UseReel = resdata;
                            break;
                        }
                    }
                }
            }
            return result;
        }

        /// <summary>
        /// 料单的料号是否在使用
        /// </summary>
        /// <param name="detail"></param>
        /// <returns></returns>
        private async Task<UseReelInfo> CheckDetailIsUse(SmtPlacementDetail detail)
        {
            UseReelInfo result = null;
            string sql = @"SELECT * FROM SMT_REEL WHERE PLACEMENT_DTL_ID = :DTL_ID AND STATUS IN (3,6)";

            var smtReelTable = (await QueryAsyncEx<dynamic>(sql, new { DTL_ID = detail.ID }))?.ToList();
            if (smtReelTable != null && smtReelTable.Count > 0)
            {
                var smtReelRow = smtReelTable.FirstOrDefault();
                string stationName = null;

                var smtStationRow = await GetAsyncEx<SmtStation>("Where Enabled='Y' AND ID=:ID", new { ID = smtReelRow.STATION_ID });
                if (smtStationRow != null)
                {
                    stationName = smtStationRow.SMT_NAME;
                }
                result = new UseReelInfo()
                {
                    REEL_ID = smtReelRow.REEL_ID,
                    PART_NO = smtReelRow.PART_NO,
                    StationName = stationName,
                    LOCATION = smtReelRow.LOCATION,
                };
                //throw new Exception(string.Format(
                //    "{0}料号的料卷{1}正在机台{2}料站{3}制件,不能重传料单.请把该料卷卸下再重传料单",
                //    smtReelRow.REEL_ID, smtReelRow.PART_NO, stationName, smtReelRow.LOCATION));
            }
            return result;
        }

    }
}