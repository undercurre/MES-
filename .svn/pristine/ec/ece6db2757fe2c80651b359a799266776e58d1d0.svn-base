/*
*┌──────────────────────────────────────────────────────────────┐
*│　描    述：接口实现                                                    
*│　作    者：嘉志科技                                            
*│　版    本：2.0    模板代码自动生成                                                
*│　创建时间：2021-01-27 11:50:30                             
*└──────────────────────────────────────────────────────────────┘
*┌──────────────────────────────────────────────────────────────┐
*│　命名空间： JZ.IMS.Repository.Oracle                                  
*│　类    名： MesPartShelfRepository                                      
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
    public class MesPartShelfRepository : BaseRepository<MesPartShelf, Decimal>, IMesPartShelfRepository
    {
        public MesPartShelfRepository(IOptionsSnapshot<DbOption> options)
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
            string sql = "SELECT ENABLED FROM MES_PART_SHELF WHERE ID=:ID";
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
            string sql = "UPDATE MES_PART_SHELF set ENABLED=:ENABLED WHERE ID=:Id";
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
            string sql = "SELECT MES_PART_SHELF_SEQ.NEXTVAL MY_SEQ FROM DUAL";
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
            string sql = "select count(0) from MES_PART_SHELF where id = :id";
            object result = await _dbConnection.ExecuteScalarAsync(sql, new
            {
                id
            });

            return (Convert.ToInt32(result) > 0);
        }

        /// <summary>
        ///工单是否存在
        /// </summary>
        /// <param name="id">项目id</param>
        /// <returns></returns>
        public async Task<bool> SfcsWoByUsed(String woNo)
        {
            string sql = " SELECT COUNT(*) FROM SFCS_WO WHERE WO_NO =:WO_NO";
            object result = await _dbConnection.ExecuteScalarAsync(sql, new
            {
                WO_NO = woNo
            });

            return (Convert.ToInt32(result) > 0);
        }

        /// <summary>
        ///条码是否存在
        /// </summary>
        /// <param name="reelCode">条码code</param>
        /// <returns></returns>
        public async Task<bool> ImsReelByUsed(String reelCode)
        {
            string sql = " SELECT COUNT(*) FROM IMS_REEL WHERE CODE =:CODE ";
            object result = await _dbConnection.ExecuteScalarAsync(sql, new
            {
                CODE = reelCode
            });

            return (Convert.ToInt32(result) > 0);
        }

        /// <summary>
        ///箱号是否存在
        /// </summary>
        /// <param name="reelCode"></param>
        /// <returns></returns>
        public async Task<bool> CartonNoByUsed(String reelCode)
        {
            string sql = " SELECT COUNT(*) FROM SFCS_CONTAINER_LIST WHERE DATA_TYPE =:DATA_TYPE AND CONTAINER_SN =:CODE ";
            object result = await _dbConnection.ExecuteScalarAsync(sql, new
            {
                CODE = reelCode,
                DATA_TYPE = GlobalVariables.CartonLable
            });

            return (Convert.ToInt32(result) > 0);
        }

        /// <summary>
        ///条码是否使用过
        /// </summary>
        /// <param name="reelCode">条码code</param>
        /// <returns></returns>
        public async Task<bool> PartDetailByUsed(String reelCode)
        {
            string sql = " SELECT COUNT(*) FROM MES_PART_CHECK_DETAIL WHERE REEL_CODE =:REEL_CODE ";
            object result = await _dbConnection.ExecuteScalarAsync(sql, new
            {
                REEL_CODE = reelCode
            });

            return (Convert.ToInt32(result) > 0);
        }

        /// <summary>
        ///条码是否使用过
        /// </summary>
        /// <param name="reelCode">条码code</param>
        /// <returns></returns>
        public async Task<bool> PartShelfByUsed(String reelCode)
        {
            string sql = " SELECT COUNT(*) FROM MES_PART_SHELF WHERE CODE =:REEL_CODE ";
            object result = await _dbConnection.ExecuteScalarAsync(sql, new
            {
                REEL_CODE = reelCode
            });

            return (Convert.ToInt32(result) > 0);
        }

        /// <summary>
		/// 获取领料清单
		/// </summary>
		/// <param name="WoNo">工单</param>
		/// <returns></returns>
		public async Task<TableDataModel> GetPickingListData(MesCheckMaterialRequestModel model)
        {
            TableDataModel tableDataModel = new TableDataModel();

            try
            {

                string sql = $@" 
                SELECT U.* FROM     ( SELECT ROWNUM RNUM,T.*       FROM ( 
                    SELECT PART.PART_CODE,PART.PART_NAME,PART.QTY,NVL(HEADER.QTY,0) CHECK_QTY,DECODE(NVL(HEADER.STATUS,0),0,'未核对',1,'已核对') STATUS,HEADER.ID HID from (SELECT IPT.ID AS PART_ID,
                        IPT.CODE AS PART_CODE,
                        IPT.NAME AS PART_NAME，
                        DTL.QUANTITY - DTL.BALANCE_QUANTITY AS QTY,
                    	IWM.ID AS WO_ID
                      FROM IMS_INTERFACE_DTL@WMS DTL
                      LEFT JOIN IMS_INTERFACE_MST@WMS MST ON MST.ID = DTL.MST_ID
                      LEFT JOIN IMS_PART@WMS IPT ON IPT.ID = DTL.PART_ID
                      LEFT JOIN IMS_WO_MST@WMS IWM ON IWM.CODE = MST.CODE
                      WHERE IWM.WORK_ORDER = :WoNo 
                     ) PART
                     LEFT JOIN MES_PART_CHECK_HEADER HEADER ON HEADER.PART_NO=PART.PART_CODE AND HEADER.WO_NO=:WoNo WHERE PART.QTY>0  ORDER BY HEADER.STATUS DESC
                ) T WHERE ROWNUM <= :Limit*:Page )U
                WHERE U.RNUM > (:Page-1)*:Limit ";
                String countsql = $@"
                               SELECT COUNT(*) FROM (SELECT DTL.QUANTITY - DTL.BALANCE_QUANTITY AS QTY
                                FROM IMS_INTERFACE_DTL@WMS DTL
                                LEFT JOIN IMS_INTERFACE_MST@WMS MST ON MST.ID = DTL.MST_ID
                                LEFT JOIN IMS_PART@WMS IPT ON IPT.ID = DTL.PART_ID
                                LEFT JOIN IMS_WO_MST@WMS IWM ON IWM.CODE = MST.CODE
                                WHERE IWM.WORK_ORDER = :WoNo) T WHERE T.QTY>0
                                 ";
                var resutlDataList = (await _dbConnection.QueryAsync<MesCheckMaterialResponseModel>(sql, model))?.ToList();
                tableDataModel.data = resutlDataList;
                var cnt = await _dbConnection.ExecuteScalarAsync<int>(countsql, model);
                tableDataModel.count = cnt;

            }
            catch (Exception ex)
            {
                tableDataModel.code = -1;
                tableDataModel.msg = ex.Message;
            }
            return tableDataModel;
        }

        /// <summary>
        /// 判断核料是否成功
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public async Task<TableDataModel> CheckPickingByReelCode(MesCheckMaterialRequestModel model, String isCode)
        {
            TableDataModel resultModel = new TableDataModel();
            resultModel.code = -1;
            ConnectionFactory.OpenConnection(_dbConnection);
            using (var tran = _dbConnection.BeginTransaction())
            {
                try
                {
                    String quertyImsReelSql = @"SELECT *  FROM IMS_REEL REEL WHERE REEL.CODE = :REEL_CODE ";

                    decimal M_QTY = 0;String M_PART_NO = "";
                    if (isCode == GlobalVariables.EnableY)
                    {
                        var reelModel = (await _dbConnection.QueryAsync<ImsReel>(quertyImsReelSql, new { REEL_CODE = model.REELCODE }))?.FirstOrDefault();
                        if (reelModel == null) { return resultModel; }
                        M_QTY = reelModel.ORIGINAL_QUANTITY ?? 0;

                        String quertyPartCodeSql = @"SELECT PART.CODE  FROM IMS_REEL REEL,IMS_PART PART WHERE REEL.CODE = :REEL_CODE AND PART.ID = REEL.PART_ID";
                        var partNoModel = (await _dbConnection.QueryAsync<ImsPart>(quertyPartCodeSql, new { REEL_CODE = model.REELCODE }))?.FirstOrDefault();
                        if (partNoModel == null) { return resultModel; }
                        M_PART_NO = partNoModel.CODE;
                    }
                    else
                    {
                        var cModel = (await _dbConnection.QueryAsync<SfcsContainerList>("SELECT * FROM SFCS_CONTAINER_LIST WHERE DATA_TYPE =:DATA_TYPE AND CONTAINER_SN =:CODE",
                            new { DATA_TYPE = GlobalVariables.CartonLable, REEL_CODE = model.REELCODE }))?.FirstOrDefault();
                        M_QTY = cModel?.SEQUENCE ?? 0;
                        M_PART_NO = cModel?.PART_NO ?? "";
                    }

                    //核对料号根据工单
                    String wmsPartCodeSql = @"SELECT 
                                                IPT.ID AS PART_ID,
                                                IPT.CODE AS PART_CODE,
                                                IPT.NAME AS PART_NAME，
                                                DTL.QUANTITY - DTL.BALANCE_QUANTITY AS QTY,
                                                IWM.ID AS WO_ID
                                              FROM IMS_INTERFACE_DTL@WMS DTL
                                              LEFT JOIN IMS_INTERFACE_MST@WMS MST ON MST.ID = DTL.MST_ID
                                              LEFT JOIN IMS_PART@WMS IPT ON IPT.ID = DTL.PART_ID
                                              LEFT JOIN IMS_WO_MST@WMS IWM ON IWM.CODE = MST.CODE
                                             WHERE IWM.WORK_ORDER =:WO_NO AND IPT.CODE=:CODE";
                    var wmsPartList = (await _dbConnection.QueryAsync<MesCheckMaterialResponseModel>(wmsPartCodeSql, new { CODE = M_PART_NO, WO_NO = model.WoNo }))?.ToList();
                    if (wmsPartList.Count <= 0)
                        return resultModel;

                    MesCheckMaterialRequestModel requestModel = new MesCheckMaterialRequestModel();
                    requestModel.WoNo = model.WoNo;
                    var pickingList = await GetPickingListData(requestModel);

                    //核对成功更新到数据库
                    String selectPartHeadSql = @" SELECT * FROM MES_PART_CHECK_HEADER WHERE WO_NO=:WO_NO AND PART_NO=:PART_NO ";
                    String insertPartHeadSql = @"INSERT INTO MES_PART_CHECK_HEADER(ID, WO_NO, PART_NO, STATUS, CREATE_TIME, CREATE_USER,QTY)
                                                 VALUES (:ID, :WO_NO, :PART_NO, 0, sysdate, :CREATE_USER,:QTY)";
                    String updatePartHeadSql = @"UPDATE　MES_PART_CHECK_HEADER SET QTY=:QTY,STATUS=:STATUS,UPDATE_TIMTE=SYSDATE,UPDATE_USER=:UPDATE_USER WHERE WO_NO=:WO_NO AND PART_NO=:PART_NO ";
                    String insertPartDetailSql = @"INSERT INTO MES_PART_CHECK_DETAIL(ID, REEL_CODE, PART_NO, HEADER_ID, CREATE_TIME, CREATE_USER, WO_NO,QTY)
                                                   VALUES(MES_PART_CHECK_HEADER_SEQ.NEXTVAL, :REEL_CODE, :PART_NO, :HEADER_ID, sysdate, :CREATE_USER, :WO_NO,:QTY) ";
                    var headerModel = (await _dbConnection.QueryAsync<MesPartCheckHeader>(selectPartHeadSql, new { WO_NO = model.WoNo, PART_NO = M_PART_NO }))?.FirstOrDefault();
                    if (headerModel != null && headerModel.ID > 0)
                    {
                        //1.插入条码数据
                        var effectNum = await _dbConnection.ExecuteAsync(insertPartDetailSql, new
                        {
                            REEL_CODE = model.REELCODE,
                            PART_NO = M_PART_NO,
                            HEADER_ID = headerModel.ID,
                            CREATE_USER = model.UserName,
                            WO_NO = model.WoNo,
                            QTY = M_QTY
                        });
                        var qty = 0;
                        var STATUS = 0;
                        //2.插入成功
                        if (effectNum > 0)
                        {
                            //3.更新核对数量和核对状态
                            //总数量
                            var wmsQty = wmsPartList.FirstOrDefault(c => M_PART_NO.Equals(c.PART_CODE)).QTY;

                            STATUS = wmsQty <= (headerModel.QTY + M_QTY) ? GlobalVariables.successCode : GlobalVariables.FailedCode;//1是成功 0是失败
                            qty = Convert.ToInt32(headerModel.QTY + M_QTY);
                            effectNum = await _dbConnection.ExecuteAsync(updatePartHeadSql, new
                            {
                                QTY = qty,
                                STATUS = STATUS,
                                UPDATE_USER = model.UserName,
                                WO_NO = model.WoNo,
                                PART_NO = M_PART_NO
                            });
                        }
                    }
                    else
                    {
                        //1.插入主表
                        var id = await Get_MES_SEQ_ID("MES_PART_CHECK_HEADER_SEQ");
                        var effectNum = await _dbConnection.ExecuteAsync(insertPartHeadSql, new
                        {
                            ID = id,
                            PART_NO = M_PART_NO,
                            CREATE_USER = model.UserName,
                            WO_NO = model.WoNo,
                            QTY = M_QTY
                        });
                        //2.插入次表
                        if (effectNum > 0)
                        {
                            effectNum = await _dbConnection.ExecuteAsync(insertPartDetailSql, new
                            {
                                REEL_CODE = model.REELCODE,
                                PART_NO = M_PART_NO,
                                HEADER_ID = id,
                                CREATE_USER = model.UserName,
                                WO_NO = model.WoNo,
                                QTY = M_QTY
                            });
                        }
                    }
                    tran.Commit();
                    resultModel.code = 0;
                }
                catch (Exception ex)
                {
                    tran.Rollback();
                    resultModel.msg = ex.Message;
                }
            }
            return resultModel;
        }

        /// <summary>
        /// 保存数据
        /// </summary>
        /// <param name="model"></param>
        /// <param name="isCode"></param>
        /// <returns></returns>
        public async Task<decimal> SaveDataByTrans(MesPartShelfModel model, String isCode)
        {
            int result = 1;
            ConnectionFactory.OpenConnection(_dbConnection);
            using (var tran = _dbConnection.BeginTransaction())
            {
                try
                {
                    //新增
                    string insertSql = @"insert into MES_PART_SHELF 
					(ID,CODE,SHELF_ID,CREATE_TIME,CREATE_USER,STATUS,PUT_SHELVES_USER,PUT_SHELVES_TIME,QTY,STORAGE,PART_NO,DESCRIPTION) 
					VALUES (:ID,:CODE,:SHELF_ID,sysdate,:CREATE_USER,:STATUS,:CREATE_USER,sysdate,:QTY,:STORAGE,:PART_NO,:DESCRIPTION)";
                    if (model.InsertRecords != null && model.InsertRecords.Count > 0)
                    {
                        foreach (var item in model.InsertRecords)
                        {
                            String quertyImsReelSql = @"SELECT * FROM IMS_REEL REEL WHERE REEL.CODE = :REEL_CODE ";

                            String quertyImsPartSql = @"SELECT * FROM IMS_PART PART WHERE PART.ID=:ID ";
                            if (isCode == GlobalVariables.EnableY)
                            {
                                var reelModel = (await _dbConnection.QueryAsync<ImsReel>(quertyImsReelSql, new { REEL_CODE = item.CODE }))?.FirstOrDefault();
                                item.QTY = reelModel?.ORIGINAL_QUANTITY ?? 0;

                                var partModel = (await _dbConnection.QueryAsync<ImsPart>(quertyImsPartSql, new { ID = reelModel.PART_ID }))?.FirstOrDefault();
                                item.PART_NO = partModel?.CODE ?? "";
                            }
                            else
                            {
                                var cModel = (await _dbConnection.QueryAsync<SfcsContainerList>("SELECT * FROM SFCS_CONTAINER_LIST WHERE DATA_TYPE =:DATA_TYPE AND CONTAINER_SN =:CODE",
                                    new { DATA_TYPE = GlobalVariables.CartonLable, REEL_CODE = item.CODE }))?.FirstOrDefault();
                                item.QTY = cModel?.SEQUENCE ?? 0;
                                item.PART_NO = cModel?.PART_NO ?? "";
                            }

                            var newid = await Get_MES_SEQ_ID("MES_PART_SHELF_SEQ");
                            var resdata = await _dbConnection.ExecuteAsync(insertSql, new
                            {
                                ID = newid,
                                item.CODE,
                                item.SHELF_ID,
                                CREATE_USER = item.CREATE_USER,
                                item.STATUS,
                                item.QTY,
                                item.STORAGE,
                                item.PART_NO,
                                item.DESCRIPTION
                            }, tran);
                        }
                    }

                    #region 更新上下架
                    //              string updateSql = @"Update MES_PART_SHELF set CODE=:CODE,QTY=:QTY,SHELF_ID=:SHELF_ID,UPDATE_TIME=:UPDATE_TIME,UPDATE_USER=:UPDATE_USER,STATUS=:STATUS{0}  
                    //where ID=:ID ";
                    //              if (model.UpdateRecords != null && model.UpdateRecords.Count > 0)
                    //              {
                    //                  foreach (var item in model.UpdateRecords)
                    //                  {
                    //                      string conditions = item.STATUS == 0 ? ",FALL_SHELVES_USER=:UPDATE_USER,FALL_SHELVES_TIME=:UPDATE_TIME" : ",PUT_SHELVES_USER=:UPDATE_USER,PUT_SHELVES_TIME=:UPDATE_TIME";
                    //                      var resdata = await _dbConnection.ExecuteAsync(String.Format(updateSql, conditions), new
                    //                      {
                    //                          item.ID,
                    //                          item.CODE,
                    //                          item.SHELF_ID,
                    //                          item.UPDATE_TIME,
                    //                          item.UPDATE_USER,
                    //                          item.STATUS,
                    //                          item.QTY
                    //                      }, tran);
                    //                  }
                    //              } 
                    #endregion

                    string updateSql = @"Update MES_PART_SHELF set CODE=:CODE,QTY=:QTY,SHELF_ID=:SHELF_ID,UPDATE_TIME=:UPDATE_TIME,UPDATE_USER=:UPDATE_USER,STATUS=:STATUS,STORAGE=:STORAGE  
                    where ID=:ID ";
                    if (model.UpdateRecords != null && model.UpdateRecords.Count > 0)
                    {
                        foreach (var item in model.UpdateRecords)
                        {
                            var resdata = await _dbConnection.ExecuteAsync(String.Format(updateSql), new
                            {
                                item.ID,
                                item.CODE,
                                item.SHELF_ID,
                                item.UPDATE_TIME,
                                item.UPDATE_USER,
                                item.STATUS,
                                item.QTY,
                                item.STORAGE
                            }, tran);
                        }
                    }

                    //删除
                    string deleteSql = @" Delete from MES_PART_SHELF where CODE=:CODE ";
                    string insertRecordsql = @" INSERT INTO MES_PART_SHELF_RECORD(ID, CODE, STORAGE, QTY, CREATE_TIME, CREATE_USER, TYPE, HID,UPDATE_TIME)
                                                SELECT MES_PART_SHELF_SEQ.NEXTVAL,CODE, STORAGE, QTY, CREATE_TIME, CREATE_USER, 1, ID,SYSDATE FROM MES_PART_SHELF WHERE CODE=:CODE ";

                    if (model.RemoveRecords != null && model.RemoveRecords.Count > 0)
                    {
                        foreach (var item in model.RemoveRecords)
                        {
                            var resdata = await _dbConnection.ExecuteAsync(insertRecordsql, new
                            {
                                item.CODE
                            }, tran);

                            if (resdata <= 0)
                                throw new Exception("Update Error");

                            resdata = await _dbConnection.ExecuteAsync(deleteSql, new
                            {
                                item.CODE
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
        /// 通过工单获取物料储位
        /// </summary>
        /// <param name="woNo">工单号</param>
        /// <returns></returns>
        public async Task<TableDataModel> GetShelfByWONO(MesPartShelfRequestModel model)
        {
            TableDataModel dataModel = new TableDataModel();
            string sql = @" 
                        SELECT U.* FROM   ( SELECT ROWNUM RNUM,T.*       FROM ( 
                         SELECT SHELF.ID,SHELF.PART_NO PART_CODE,SHELF.CODE REEL_CODE,SHELF.DESCRIPTION,SHELF.QTY,SHELF.STORAGE FROM SFCS_WO WO,MES_PART_SHELF SHELF
                         WHERE  WO.PART_NO = SHELF.PART_NO AND SHELF.STATUS=1 {0}
                         ) T WHERE ROWNUM <= :LIMIT*:PAGE )U
                        WHERE U.RNUM > (:PAGE-1)*:LIMIT";
            try
            {

                string conditions = "";

                if (!model.REEL_CODE.IsNullOrWhiteSpace())
                {
                    conditions += $" AND SHELF.CODE=:REEL_CODE ";
                }

                if (!model.WO_NO.IsNullOrWhiteSpace())
                {
                    conditions += $" AND WO.WO_NO =:WO_NO ";
                }
                
                dataModel.data = (await _dbConnection.QueryAsync<MesPartShelfResponseModel>(String.Format(sql,conditions), model))?.ToList();
            }
            catch (Exception ex)
            {
                dataModel.code = -1;
                dataModel.msg = ex.Message;
            }
            return dataModel;
        }

    }
}