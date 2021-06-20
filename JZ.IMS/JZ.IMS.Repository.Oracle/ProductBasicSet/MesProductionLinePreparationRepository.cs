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
using System.Net.Http.Headers;
using System.IO;
using JZ.IMS.Core.Extensions;

namespace JZ.IMS.Repository.Oracle.ProductBasicSet
{
    public class MesProductionLinePreparationRepository : BaseRepository<MesProductionPreConf, Decimal>, IMesProductionLinePreparationRepository
    {

        System.Data.IDbTransaction tran = null;

        public MesProductionLinePreparationRepository(IOptionsSnapshot<DbOption> options)
        {
            _dbOption = options.Get("iWMS");
            if (_dbOption == null)
            {
                throw new ArgumentNullException(nameof(DbOption));
            }
            _dbConnection = ConnectionFactory.CreateConnection(_dbOption.DbType, _dbOption.ConnectionString);
        }

        public enum ReelStatus
        {
            REEL_FREE = 0,
            REEL_PREPARE = 1,
            REEL_SUPPLY = 2,
            REEL_PLACE = 3,
            REEL_COMPLETE = 4,
            REEL_BACKUP = 5,
            REEL_CONNECT = 6
        }

        public string CastReelStatus(int status)
        {
            string ret = "未知";
            switch (status)
            {
                case (int)ReelStatus.REEL_FREE:
                    ret = "空闲";
                    break;
                case (int)ReelStatus.REEL_PREPARE:
                    ret = "备料";
                    break;
                case (int)ReelStatus.REEL_SUPPLY:
                    ret = "上料";
                    break;
                case (int)ReelStatus.REEL_PLACE:
                    ret = "置件中";
                    break;
                case (int)ReelStatus.REEL_COMPLETE:
                    ret = "使用完";
                    break;
                case (int)ReelStatus.REEL_BACKUP:
                    ret = "备线生产";
                    break;
                case (int)ReelStatus.REEL_CONNECT:
                    ret = "接料生产";
                    break;
            }
            return ret;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="String"></typeparam>
        /// <param name="name"></param>
        /// <returns></returns>
        public async Task<decimal> GetSequenceValue(string name)
        {
            String sql = $" SELECT {name}.NEXTVAL FROM DUAL ";
            var result = await _dbConnection.ExecuteScalarAsync(sql);
            return (decimal)result;

        }

        /// <summary>
        /// 获取当前时间
        /// </summary>
        /// <returns></returns>
        public async Task<String> GetCurrentTime()
        {
            String sql = "select sysdate from dual";
            DateTime currentDate = await _dbConnection.QueryFirstOrDefaultAsync<DateTime>(sql);
            return currentDate.ToString("yyyyMMddHHmmss");
        }

        /// <summary>
        /// 插入监控表数据
        /// </summary>
        /// <returns></returns>
        public async Task<decimal> InsertMaterial(MesHiMaterialListenAddOrModifyModel model)
        {
            decimal result = -1;
            String I_InsertMaterial = @"INSERT  INTO MES_HI_MATERIAL_LISTEN(ID,BATCH_NO,WO_NO,OPERATION_LINE_ID,PART_NO,UNITY_QTY,PRE_QTY,USED_QTY, STATUS)
                           VALUES(:ID,:BATCH_NO,:WO_NO,:OPERATION_LINE_ID,:PART_NO,:UNITY_QTY,0,0,0)";
            try
            {
                result = await _dbConnection.ExecuteAsync(I_InsertMaterial, new
                {
                    ID = model.ID,
                    BATCH_NO = model.BATCH_NO,
                    WO_NO = model.WO_NO,
                    OPERATION_LINE_ID = model.OPERATION_LINE_ID,
                    PART_NO = model.PART_NO,
                    UNITY_QTY = model.UNITY_QTY
                });
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            return result;
        }

        /// <summary>
        /// 查找E-BOM
        /// </summary>
        /// <returns></returns>
        public async Task<List<dynamic>> Select_ebomMaterialListen(string PART_NO)
        {
            List<dynamic> result = new List<dynamic>();
            String S_InsertMaterialListen = @"select SORP.PART_NO , SUM(SORP.PART_QTY) AS  UNIT_QTY ,SOR.CURRENT_OPERATION_ID from  
                            SOP_OPERATIONS_ROUTES_PART  SORP,
                            SOP_ROUTES SR, 
                            SOP_OPERATIONS_ROUTES SOR
                            where 
                            SORP.OPERATIONS_ROUTES_ID = SOR.ID AND 
                            SOR.ROUTE_ID = SR.ID
                            AND SR.PART_NO = :PART_NO
                            GROUP BY SORP.PART_NO , CURRENT_OPERATION_ID";
            try
            {
                result = (await _dbConnection.QueryAsync<dynamic>(S_InsertMaterialListen,new {
                    PART_NO=PART_NO
                }))?.ToList();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            return result;
        }

        /// <summary>
        /// 插入产线数据
        /// </summary>
        /// <returns></returns>
        public async Task<decimal> InsertProductLine(SfcsProductionAddOrModifyModel model)
        {
            decimal result = -1;
            String sql = @"INSERT INTO SFCS_PRODUCTION(BATCH_NO,LINE_ID ,WO_NO,PCB_PN,PCB_SIDE,MODEL,START_BY,FINISHED,STATION_ID,OPERATION_TYPE,MULTI_NO,LOC_NO)
        VALUES(:BATCH_NO, :LINE_ID,:WO_NO, :PCB_PN, :PCB_SIDE,:MODEL,:START_BY,:FINISHED,:STATION_ID,:OPERATION_TYPE,:MULTI_NO,:LOC_NO)";
            try
            {
                result = await _dbConnection.ExecuteAsync(sql, model);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            return result;
        }


        /// <summary>
        /// 插入HIREEl
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public async Task<decimal> InsertHIREEL(MesHiReelAddOrModifyModel model)
        {
            decimal result = -1;
            String InsertNewHiReel = @"INSERT INTO MES_HI_REEL(ID, WO_NO, REEL_ID, OPERATION_LINE_ID, QTY, OPERTOR, CREATE_TIME, USED_QTY, ORG_QTY,STATUS,BATCH_NO,PART_NO,OPERAITON_ID,OPERATION_SITE_ID,MES_USER)
                VALUES (:ID, :WO_NO, :REEL_ID, :OPERATION_LINE_ID, :QTY, :OPERTOR, sysdate, :USED_QTY, :ORG_QTY,:STATUS,:BATCH_NO,:PART_NO,:OPERAITON_ID,:OPERATION_SITE_ID,:MES_USER)";
                        
            try
            {
                result = await _dbConnection.ExecuteAsync(InsertNewHiReel, model,this.tran);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            return result;
        }

        /// <summary>
        /// 查监听表的已经备完料
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public async Task<decimal> SelectListenBYBatch(string Batch_no)
        {
            decimal result = -1;
            String S_MaterialStatus = @"select count(*) from MES_HI_MATERIAL_LISTEN where BATCH_NO = :BATCH_NO AND  PRE_QTY <=0";

            try
            {
                result = await _dbConnection.ExecuteScalarAsync<int>(S_MaterialStatus, new {
                    BATCH_NO=Batch_no
                });
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            return result;
        }

        /// <summary>
        /// 查监听表的已经备完料
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public async Task<decimal> UpdateProductionBYBatch(string Batch_no)
        {
            decimal result = -1;
            String U_UpdateProduct = @"UPDATE SFCS_PRODUCTION SET FINISHED = 'N' WHERE BATCH_NO= :BATCH_NO";
            try
            {
                result = await _dbConnection.ExecuteAsync(U_UpdateProduct, new
                {
                    BATCH_NO = Batch_no
                },this.tran);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            return result;
        }

        /// <summary>
        /// 新工单上线，旧工单下线,旧工单物料自动转移
        /// </summary>
        /// <param name="oldBatchNo"></param>
        /// <param name="newBatchNo"></param>
        /// <param name="user"></param>
        /// <param name="keeyWo"></param>
        /// <returns></returns>
        public async Task productKepMaterial(String oldBatchNo, String newBatchNo, String user, bool keeyWo)
        {
            if (oldBatchNo.IsNullOrEmpty())
            {
                return;
            }
            //卸掉旧工单的物料，并且修改物料状态
            //数量为0 的改为用完
            //数量大于零的为空闲
            String upDateOldMaterial = @"begin  
                UPDATE  MES_HI_REEL SET STATUS = 3  WHERE BATCH_NO = :OLD_BATCH_NO AND QTY >0 AND STATUS IN (0,1);
                UPDATE  MES_HI_REEL SET STATUS = 2  WHERE BATCH_NO = :OLD_BATCH_NO AND QTY =0 AND STATUS IN (0,1);
            end;";
            await _dbConnection.ExecuteScalarAsync(upDateOldMaterial, new { OLD_BATCH_NO = oldBatchNo },this.tran);

            if (keeyWo)
            {
                //找出旧工单中与新工单相同料号的未用状态的物料
                String S_SelectOldMaterial = @"select * from MES_HI_REEL  WHERE BATCH_NO = :OLD_BATCH_NO AND STATUS = 3 AND PART_NO IN 
                (select PART_NO from  MES_HI_MATERIAL_LISTEN where BATCH_NO = :NEW_BATCH_NO)";
                var hiReelData = (await _dbConnection.QueryAsync<MesHiReel>(S_SelectOldMaterial, new { OLD_BATCH_NO = oldBatchNo, NEW_BATCH_NO = newBatchNo })).ToList();
                if (hiReelData == null || hiReelData.Count <= 0)
                {
                    return;
                }
                String currentHiMaterialListenSql = @"select * from MES_HI_MATERIAL_LISTEN WHERE BATCH_NO = :BATCH_NO";
                var currentHiMaterialListenTable = (await _dbConnection.QueryAsync<MesHiMaterialListen>(currentHiMaterialListenSql, new { BATCH_NO = newBatchNo })).ToList();

                List<MesHiReelAddOrModifyModel> newHiReelTable = new List<MesHiReelAddOrModifyModel>();
                foreach (var reelRow in hiReelData)
                {
                    MesHiReelAddOrModifyModel newHiReel = new MesHiReelAddOrModifyModel();
                    newHiReel.ID = await GetSequenceValue("MES_HI_REEL_SEQ");
                    newHiReel.WO_NO = currentHiMaterialListenTable.FirstOrDefault().WO_NO;
                    newHiReel.REEL_ID = reelRow.REEL_ID;
                    newHiReel.OPERATION_LINE_ID = currentHiMaterialListenTable.FirstOrDefault().OPERATION_LINE_ID.ToString();
                    newHiReel.QTY = reelRow.QTY;
                    newHiReel.OPERTOR = user;
                    newHiReel.USED_QTY = 0;
                    newHiReel.ORG_QTY = reelRow.ORG_QTY;
                    newHiReel.BATCH_NO = newBatchNo;
                    newHiReel.PART_NO = reelRow.PART_NO;
                    newHiReel.OPERAITON_ID = reelRow.OPERAITON_ID;
                    newHiReel.OPERATION_SITE_ID = reelRow.OPERATION_SITE_ID;
                    newHiReel.MES_USER = reelRow.MES_USER;
                    var temp = currentHiMaterialListenTable.Where(f => f.PART_NO == reelRow.PART_NO).ToList();
                    if (temp.FirstOrDefault().CURR_REEL_ID.IsNullOrEmpty())
                    {
                        foreach (var listenRow in temp)
                        {
                            listenRow.CURR_REEL_ID = reelRow.REEL_ID;
                            String UpdateListen = @"UPDATE MES_HI_MATERIAL_LISTEN SET CURR_REEL_ID = :CURR_REEL_ID ,PRE_QTY = :PRE_QTY WHERE ID = :ID";
                            await _dbConnection.ExecuteAsync(UpdateListen, new { CURR_REEL_ID = reelRow.REEL_ID, PRE_QTY = reelRow.QTY, ID = listenRow.ID },this.tran);
                        }
                        //新增reel 为1:在用状态
                        newHiReel.STATUS = 1;

                    }
                    else
                    {
                        foreach (var listenRow in temp)
                        {
                            listenRow.CURR_REEL_ID = reelRow.REEL_ID;
                            String UpdateListen = @"UPDATE MES_HI_MATERIAL_LISTEN SET PRE_QTY = PRE_QTY + :PRE_QTY WHERE ID = :ID";
                            await _dbConnection.ExecuteAsync(UpdateListen, new { PRE_QTY = reelRow.QTY, ID = listenRow.ID },this.tran);
                        }
                        //新增reel 为0:待用状态
                        newHiReel.STATUS = 0;

                    }
                    newHiReelTable.Add(newHiReel);
                    // 修改旧reel 记录状态为4：挪料状态
                    String updateOldHiReel = @"UPDATE  MES_HI_REEL SET STATUS = 4 WHERE ID = :ID";
                    await _dbConnection.ExecuteAsync(updateOldHiReel, new { ID = reelRow.ID },this.tran);
                }

                //提交所有修改
                //newHiReelTable
                foreach (var newReelRow in newHiReelTable)
                {
                    String InsertNewHiReel = @"INSERT INTO MES_HI_REEL(ID, WO_NO, REEL_ID, OPERATION_LINE_ID, QTY, OPERTOR, CREATE_TIME, USED_QTY, ORG_QTY,STATUS,BATCH_NO,PART_NO,OPERAITON_ID,OPERATION_SITE_ID,MES_USER)
                VALUES (:ID, :WO_NO, :REEL_ID, :OPERATION_LINE_ID, :QTY, :OPERTOR, sysdate, :USED_QTY, :ORG_QTY,:STATUS,:BATCH_NO,:PART_NO,:OPERAITON_ID,:OPERATION_SITE_ID,:MES_USER)";
                    await InsertHIREEL(newReelRow);
                }
                if (currentHiMaterialListenTable.Where(f => f.CURR_REEL_ID == null).Count() <= 0)
                {
                    String U_UpdateProductLine = @"update SFCS_PRODUCTION SET FINISHED = 'N' WHERE BATCH_NO = :BATCH_NO";
                    await _dbConnection.ExecuteAsync(U_UpdateProductLine, new { BATCH_NO = newBatchNo },this.tran);

                }
            }

        }

        public async Task<decimal> GetOnhandQty(string reelId)
        {
            string sql = @"
                SELECT ONHAND_QTY FROM SMT_REEL WHERE REEL_ID = :REEL_ID
                UNION
                SELECT 0 FROM DUAL WHERE NOT EXISTS(SELECT 1 FROM SMT_REEL WHERE REEL_ID = :REEL_ID)
            ";
            return await _dbConnection.ExecuteScalarAsync<int>(sql, new
            {
                REEL_ID = reelId
            });
        }

        public async Task<Reel> GetReel(string reelCode)
        {
            //更新WMS的Reel数据到MES
            //UpdateIMSReel(new KeyValuePair<string, object>("CODE", reelCode));

            //ReelDataSet.IMS_REEL_INFO_VIEWDataTable reelTable = GetReelInfoView(new KeyValuePair<string, object>("CODE", reelCode));
            //if (reelTable == null || reelTable.Count <= 0)
            //{
            ////同步WMS数据到MES
            //DBA.FromDb("MES").ExecuteProcedure("SYNC_IMS_REEL_FROM_WMS");
            //}
            string S_SelectReelInfoView = @"SELECT * FROM IMS_REEL_INFO_VIEW WHERE CODE=:CODE ";
            string S_SelectSMTReel = @"SELECT * FROM SMT_REEL R WHERE R.REEL_ID = :P_REEL_ID ORDER BY ID DESC";
            var smtReelTable = (await _dbConnection.QueryAsync<SmtReel>(S_SelectSMTReel, new { P_REEL_ID = reelCode })).ToList()?.FirstOrDefault();
            var reel = new Reel();
            var reelTable = (await _dbConnection.QueryAsync<dynamic>(S_SelectReelInfoView, new { CODE = reelCode })).ToList();
            
            if (smtReelTable != null )
            {
                reel.STATUS = Convert.ToDecimal(smtReelTable.STATUS);
            }
            if (reelTable == null || reelTable.Count <= 0)
            {
                return null;
            }
            var reelRow = reelTable.FirstOrDefault();
            
            reel.CaseQty = reelRow.CaseQty==null? 0 : reelRow.CASE_QTY;
            reel.CODE = reelRow.CODE;
            reel.COO = reelRow.COO==null? string.Empty : reelRow.COO;
            reel.CustomerPN = reelRow.CustomerPN==null ? string.Empty : reelRow.CUSTOMER_PN;
            reel.DateCode = reelRow.DateCode==null? -1 : reelRow.DATE_CODE;
            reel.ID = reelRow.ID;
            reel.LotCode = reelRow.LotCode==null ? string.Empty : reelRow.LOT_CODE;
            reel.MakerID = 0;
            reel.MakerName = reelRow.MakerName==null ? string.Empty : reelRow.MAKER_NAME;
            reel.MakerPartID = reelRow.MakerPartID==null ? -1 : reelRow.MAKER_PART_ID;
            reel.MakerPN = reelRow.MakerPN==null ? string.Empty : reelRow.MAKER_PART_NO;
            reel.PartID = reelRow.PART_ID??0;
            reel.PART_NO = reelRow.PART_NO??"";
            reel.PART_DESC = reelRow.PART_DESC??"";
            reel.PART_NAME = reelRow.PART_NAME??"";
            reel.Quantity = reelRow.ORIGINAL_QUANTITY==null?0 : reelRow.ORIGINAL_QUANTITY;
            reel.REF = reelRow.REF==null ? string.Empty : reelRow.REFERENCE;
            reel.VendorName = reelRow.VENDOR_NAME??"";
            reel.VendorCode = reelRow.VENDOR_CODE??"";
            reel.VendorID = reelRow.VENDOR_ID==null?0:reelRow.VENDOR_ID;
            reel.OnhandQty = await GetOnhandQty(reelCode);
            reel.STATUS_NAME = CastReelStatus((int)reel.STATUS);
            return reel;
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="ID"></param>
        /// <returns></returns>
        public async Task<decimal> Update_ExistReel(object ID)
        {
            var result = -1;
            try
            {
                String Update_ExistReel = @"update  MES_HI_REEL set STATUS = 4 where ID = :ID";
                result = await _dbConnection.ExecuteAsync(Update_ExistReel, new { ID = ID },this.tran);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            return result;
        }

        /// <summary>
		/// 查询备料
		/// </summary>
		/// <param name="woNo"></param>
		/// <param name="reelId"></param>
		/// <param name="operationLineId"></param>
		/// <returns></returns>
		public async Task<List<MesHiReel>> GetHiReelData(string reelId)
        {
            var result = new List<MesHiReel>();
            try
            {
                String sql = @"select * from MES_HI_REEL where REEL_ID = :REEL_ID AND STATUS IS NOT NULL ";
                result = (await _dbConnection.QueryAsync<MesHiReel>(sql, new
                {
                    REEL_ID = reelId
                }))?.ToList();
            }
            catch (Exception ex)
            {

                throw new Exception(ex.Message);
            }
            return result;
        }

        /// <summary>
        /// 设置制程
        /// </summary>
        /// <param name="woNo"></param>
        /// <param name="reelId"></param>
        /// <param name="operationLineId"></param>
        /// <returns></returns>
        public async Task<decimal> SetRoute(decimal reelId,string woNO)
        {
            var result = -1;
            try
            {
                String U_UpdateWo = @"UPDATE  SFCS_WO SET ROUTE_ID = :ROUTE_ID  WHERE WO_NO = :WO_NO AND WO_STATUS = 1";
                result = await _dbConnection.ExecuteAsync(U_UpdateWo, new { ROUTE_ID = reelId, WO_NO = woNO}, this.tran);
            }
            catch (Exception ex)
            {

                throw new Exception(ex.Message);
            }
            return result;
        }

        /// <summary>
        /// 更新操作
        /// </summary>
        /// <returns></returns>
        public async Task<decimal> UpdateListen(decimal id,decimal preQty,string reelId="")
        {
            decimal result = -1;
            try
            {
                string condition = "";
                if (!reelId.IsNullOrEmpty())
                {
                    condition += " ,CURR_REEL_ID = :CURR_REEL_ID ";
                }
                String U_UpdateListen = $"UPDATE MES_HI_MATERIAL_LISTEN SET PRE_QTY = :PRE_QTY {condition} WHERE ID = :ID";
                result = await _dbConnection.ExecuteAsync(U_UpdateListen, new { ID = id, PRE_QTY= preQty, CURR_REEL_ID= reelId },this.tran);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            return result;
        }

        /// <summary>
        /// 关闭当前的上线工单
        /// </summary>
        /// <returns></returns>
        public async Task<decimal> UpdateCloseListen(string batchNo)
        {
            decimal result = -1;
            try
            {
                
                String U_UpdateMaterialListen = @"update MES_HI_MATERIAL_LISTEN SET STATUS =1 where BATCH_NO = :BATCH_NO";
                result = await _dbConnection.ExecuteAsync(U_UpdateMaterialListen, new { BATCH_NO= batchNo }, this.tran);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            return result;
        }

        /// <summary>
        /// 更新操作
        /// </summary>
        /// <returns></returns>
        public async Task<decimal> UpdateProductLine(String user,decimal lineId,String woNo)
        {
            decimal result = -1;
            try
            {
                String U_UpdateProductLine = @"update SFCS_PRODUCTION SET END_TIME = SYSDATE, END_BY = :END_BY ,FINISHED = 'Y' WHERE LINE_ID = :LINE_ID AND WO_NO=:WO_NO ";
                result = await _dbConnection.ExecuteAsync(U_UpdateProductLine, new { END_BY=user, LINE_ID=lineId, WO_NO= woNo }, this.tran);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            return result;
        }

        /// <summary>
        /// 开启事务
        /// </summary>
        /// <returns></returns>
        public async Task StartTransaction()
        {
            
            try
            {
                ConnectionFactory.OpenConnection(_dbConnection);
                tran = _dbConnection.BeginTransaction();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        /// <summary>
        /// 事务提交
        /// </summary>
        /// <returns></returns>
        public async Task CommitTransaction()
        {

            try
            {
                this.tran.Commit();
            }
            catch (Exception ex)
            {

                throw new Exception(ex.Message);
            }
        }

        /// <summary>
        /// 事务回滚事务
        /// </summary>
        /// <returns></returns>
        public async Task RollbackTransaction()
        {

            try
            {
                this.tran.Rollback();
            }
            catch (Exception ex)
            {

                throw new Exception(ex.Message);
            }
        }

        /// <summary>
        /// 关闭事务
        /// </summary>
        /// <returns></returns>
        public async Task<bool> CloseTransaction()
        {
            bool result = false;
            try
            {
                this.tran.Dispose();
                if (_dbConnection.State != System.Data.ConnectionState.Closed)
                {
                    _dbConnection.Close();
                    result = true;
                }
            }
            catch (Exception ex)
            {

                throw new Exception(ex.Message);
            }
            return result;
        }

        /// <summary>
        /// 检查BOM物料
        /// </summary>
        /// <param name="Part_ID"></param>
        /// <returns></returns>
        public async Task<dynamic> CheckBOMByPartID(decimal Part_ID)
        {
            dynamic result = -1;
            try
            {
                result = await _dbConnection.ExecuteScalarAsync<int>("SELECT COUNT(1) FROM SMT_BOM2 B2 LEFT JOIN SMT_BOM1 B1 ON B2.BOM_ID = B1.BOM_ID LEFT JOIN IMS_PART P  ON B1.PARTENT_CODE = P.CODE WHERE P.ID = :PARTID ", new
                {
                    PARTID = Part_ID
                });
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            return result;
        }
    }
}
