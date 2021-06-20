using JZ.IMS.Core.DbHelper;
using JZ.IMS.Core.Options;
using JZ.IMS.Core.Repository;
using JZ.IMS.IRepository;
using JZ.IMS.Models;
using Dapper;
using Microsoft.Extensions.Options;
using System;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Linq;
using JZ.IMS.ViewModels;
using System.Data;
using System.Text;
using JZ.IMS.Repository.Barcode;
using JZ.IMS.Core.Extensions;

namespace JZ.IMS.Repository.Oracle
{
    public class MesPartTempWarehouseRepository : BaseRepository<MesPartTempWarehouse, Decimal>, IMesPartTempWarehouseRepository
    {
        public MesPartTempWarehouseRepository(IOptionsSnapshot<DbOption> options)
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
            string sql = "SELECT ENABLED FROM MES_PART_TEMP_WAREHOUSE WHERE ID=:ID";
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
            string sql = "UPDATE MES_PART_TEMP_WAREHOUSE set ENABLED=:ENABLED WHERE ID=:Id";
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
            string sql = "SELECT MES_PART_TEMP_WAREHOUSE_SEQ.NEXTVAL MY_SEQ FROM DUAL";
            var result = await _dbConnection.ExecuteScalarAsync(sql);
            return (decimal)result;
        }

        /// <summary>
        /// 获取明细表的序列
        /// </summary>
        /// <returns></returns>
        public async Task<decimal> GetSeqDetail()
        {
            string sql = "SELECT MES_PART_TEMP_DETAIL_SEQ.NEXTVAL MY_SEQ FROM DUAL";
            var result = await _dbConnection.ExecuteScalarAsync(sql);
            return (decimal)result;
        }

        /// <summary>
        /// 获取库存数据
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public async Task<IEnumerable<MesPartTempWarehouseListModel>> GetMstData(MesPartTempWarehouseRequestModel model)
        {
            string sql = @"select * from (select tt.*,ROWNUM AS rowno from (
						  SELECT w.*,
							   P.NAME AS PART_NAME,
							   P.DESCRIPTION AS PART_DESC,
							   S.CHINESE AS PART_TYPE_NAME
						  FROM MES_PART_TEMP_WAREHOUSE W
							   LEFT JOIN IMS_PART P ON W.PART_NO = P.CODE
							   LEFT JOIN (SELECT LOOKUP_CODE, CHINESE
											FROM SFCS_PARAMETERS
										   WHERE LOOKUP_TYPE = 'PART_TYPE') S
								  ON W.PART_TYPE = S.LOOKUP_CODE";
            sql += GetWhereStr(model);
            sql += @" order by W.ID DESC ) tt where ROWNUM <= :Limit*:Page) tt2 where tt2.rowno >= (:Page-1)*:Limit";

            return await _dbConnection.QueryAsync<MesPartTempWarehouseListModel>(sql, model);
        }

        /// <summary>
        /// 获取库存信息数量
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public async Task<int> GetMstDataCount(MesPartTempWarehouseRequestModel model)
        {
            string whereStr = GetWhereStr(model);
            StringBuilder sql = new StringBuilder();
            sql.Append("select count(*) from MES_PART_TEMP_WAREHOUSE W ");
            sql.Append(whereStr);

            return await _dbConnection.ExecuteScalarAsync<int>(sql.ToString(), model);
        }

        /// <summary>
        /// 获取WHERE条件
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        private string GetWhereStr(MesPartTempWarehouseRequestModel model)
        {
            StringBuilder whereStr = new StringBuilder();
            whereStr.Append("  WHERE 1 = 1 ");

            if (model.PART_TYPE != null)
                whereStr.Append(" and W.PART_TYPE=:PART_TYPE ");

            if (!string.IsNullOrEmpty(model.PART_NO))
                whereStr.Append(" and (INSTR(W.PART_NO,:PART_NO)>0) ");

            if (!string.IsNullOrEmpty(model.ENABLED))
                whereStr.Append(" and W.ENABLED=:ENABLED ");

            if (model.IsInventory)
                whereStr.Append(" and W.QTY > 0 ");

            return whereStr.ToString();
        }

        /// <summary>
        /// 获取明细数据
        /// </summary>
        /// <param name="mstId">库存主表ID</param>
        /// <returns></returns>
        public async Task<IEnumerable<MesPartTempDetailListModel>> GetDetailData(decimal mstId)
        {
            string sql = @"SELECT *
							FROM (  SELECT *
									FROM MES_PART_TEMP_DETAIL
									WHERE MST_ID = :MST_ID AND STATUS = 1 AND EXP_DATE <> 0
								ORDER BY DATE_CODE + EXP_DATE, ID ASC)
						UNION ALL
						SELECT *
							FROM (  SELECT *
									FROM MES_PART_TEMP_DETAIL
									WHERE MST_ID = :MST_ID AND STATUS = 1 AND EXP_DATE = 0
								ORDER BY DATE_CODE, ID ASC)
						UNION ALL
						SELECT *
							FROM (  SELECT *
									FROM MES_PART_TEMP_DETAIL
									WHERE MST_ID = :MST_ID AND STATUS <> 1
								ORDER BY ID ASC)";

            //string sql = "SELECT * FROM MES_PART_TEMP_DETAIL WHERE MST_ID = :MST_ID ORDER BY ID DESC";
            return await _dbConnection.QueryAsync<MesPartTempDetailListModel>(sql, new { MST_ID = mstId });
        }

        /// <summary>
        /// 获取操作记录数据
        /// </summary>
        /// <param name="mstId">库存主表ID</param>
        /// <param name="detailId">库存明细ID</param>
        /// <returns></returns>
        public async Task<IEnumerable<MesPartTempRecordListModel>> GetRecordData(decimal mstId, decimal? detailId)
        {
            string sql = @"SELECT R.*,D.REEL_ID,L.LINE_NAME FROM MES_PART_TEMP_RECORD R 
						LEFT JOIN MES_PART_TEMP_DETAIL D ON R.TEMP_DETAIL_ID = D.ID
						LEFT JOIN (SELECT ID,OPERATION_LINE_NAME AS LINE_NAME from SFCS_OPERATION_LINES WHERE ENABLED = 'Y') L ON L.ID = R.LINE_ID
						WHERE R.MST_ID = :MST_ID ORDER BY R.CREATE_TIME DESC";
            if (detailId != null)
                sql += " AND DETAIL_ID = :DETAIL_ID";
            return await _dbConnection.QueryAsync<MesPartTempRecordListModel>(sql, new { MST_ID = mstId, DETAIL_ID = detailId });
        }

        /// <summary>
        /// 获取系统参数
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        public List<SfcsParameters> GetParametersByType(string type)
        {
            string sql = "SELECT * FROM SFCS_PARAMETERS WHERE LOOKUP_TYPE=:LOOKUP_TYPE AND ENABLED = 'Y'";
            return _dbConnection.Query<SfcsParameters>(sql, new { LOOKUP_TYPE = type }).ToList();
        }

        /// <summary>
        /// 判断物料编码是否已经存在库存数据
        /// </summary>
        /// <param name="partNo"></param>
        /// <returns></returns>
        public async Task<bool> IsExistPartNo(string partNo)
        {
            string sql = "SELECT COUNT(1) FROM MES_PART_TEMP_WAREHOUSE WHERE PART_NO = :PART_NO AND ENABLED = 'Y'";
            int i = await _dbConnection.ExecuteScalarAsync<int>(sql, new { PART_NO = partNo });
            return i > 0;
        }

        /// <summary>
        /// 判断物料条码是否在库存明细中存在
        /// </summary>
        /// <param name="reel_id"></param>
        /// <returns></returns>
        public async Task<bool> IsExistReelDetail(string reel_id)
        {
            string sql = "SELECT COUNT(1) FROM MES_PART_TEMP_DETAIL WHERE REEL_ID = :REEL_ID";
            int i = await _dbConnection.ExecuteScalarAsync<int>(sql, new { REEL_ID = reel_id });
            return i > 0;
        }

        /// <summary>
        /// 根据物料编码获取下一个该出库的物料条码
        /// </summary>
        /// <param name="part_no"></param>
        /// <returns></returns>
        public async Task<string> GetNextReelId(string part_no)
        {
            string sql = @"SELECT *
							  FROM (SELECT *
									  FROM (  SELECT REEL_ID
												FROM MES_PART_TEMP_DETAIL
											   WHERE     PART_NO = :PART_NO
													 AND STATUS = 1
													 AND EXP_DATE <> 0
											ORDER BY DATE_CODE + EXP_DATE, ID ASC)
									UNION ALL
									SELECT *
									  FROM (  SELECT REEL_ID
												FROM MES_PART_TEMP_DETAIL
											   WHERE     PART_NO = :PART_NO
													 AND STATUS = 1
													 AND EXP_DATE = 0
											ORDER BY DATE_CODE, ID ASC))
							 WHERE ROWNUM = 1";

            string ReelId = await _dbConnection.ExecuteScalarAsync<string>(sql, new { PART_NO = part_no });

            return ReelId;
        }

        /// <summary>
        /// 根据物料唯一标识获取物料信息（入库）
        /// </summary>
        /// <param name="reel_id"></param>
        /// <returns></returns>
        public async Task<MesPartTempReelModel> GetReelDataInput(string reel_id)
        {
            string sql = @"SELECT REEL.CODE,
                            REEL.ORIGINAL_QUANTITY AS QTY,
                            PART.CODE AS PART_NO,
                            PART.NAME AS PART_NAME,
                            PART.DESCRIPTION AS PART_DESC,
                            PART.ATTRIBUTE1 AS UNIT,
                            W.PART_TYPE,
                            W.ID AS MST_ID,
                            W.ENABLED,
                            REEL.ID,
                            DATE_CODE AS DATE_SCODE,
                            LOT_CODE，V.NAME AS VENDOR_NAME
                        FROM IMS_REEL REEL 
                            INNER JOIN IMS_PART PART ON REEL.PART_ID = PART.ID
                            LEFT JOIN MES_PART_TEMP_WAREHOUSE W ON PART.CODE = W.PART_NO
                            LEFT JOIN IMS_VENDOR V ON REEL.VENDOR_ID = V.ID
							WHERE REEL.CODE = :REEL_ID";

            var data = (await _dbConnection.QueryAsync<MesPartTempReelModel>(sql, new { REEL_ID = reel_id })).FirstOrDefault();
            return data;
        }

        /// <summary>
		/// 根据物料唯一标识获取物料信息（入库Android调用）
		/// </summary>
		/// <param name="reel_id"></param>
		/// <returns></returns>
		public async Task<MesPartTempReelModel> GetReelDataInputByAndroid(string reel_id)
        {
            string sql = @"SELECT REEL.CODE,
                            REEL.ORIGINAL_QUANTITY AS QTY,
                            PART.CODE AS PART_NO,
                            PART.NAME AS PART_NAME,
                            PART.DESCRIPTION AS PART_DESC,
                            PART.ATTRIBUTE1 AS UNIT,
                            W.PART_TYPE,
                            W.ID AS MST_ID,
                            W.ENABLED,
                            REEL.ID,
                            DATE_CODE AS DATE_SCODE,
                            LOT_CODE，V.NAME AS VENDOR_NAME
                        FROM IMS_REEL REEL 
                            INNER JOIN IMS_PART PART ON REEL.PART_ID = PART.ID
                            LEFT JOIN MES_PART_TEMP_WAREHOUSE W ON PART.CODE = W.PART_NO
                            LEFT JOIN IMS_VENDOR V ON REEL.VENDOR_ID = V.ID
							WHERE REEL.CODE = :REEL_ID";
            //获取条码信息
            var data = (await _dbConnection.QueryAsync<MesPartTempReelModel>(sql, new { REEL_ID = reel_id })).FirstOrDefault();
            //判断条码供应商是否为空
            if (string.IsNullOrEmpty(data.VENDOR_NAME))
            {
                string wmsSql = "SELECT VENDOR_ID FROM IMS_REEL@wms WHERE CODE=:CODE";
                var imsData = (await _dbConnection.QueryAsync<ImsReel>(wmsSql, new { CODE = reel_id })).FirstOrDefault();
                if (imsData == null) return data;
                else
                {
                    //执行修改MES的条码供应商
                    string upSql = "UPDATE IMS_REEL SET VENDOR_ID=:VENDOR_ID WHERE CODE=:CODE";
                    await _dbConnection.ExecuteScalarAsync(upSql, new { VENDOR_ID = imsData.VENDOR_ID, CODE = reel_id });
                }
                //重新查询取到数据
                data = (await _dbConnection.QueryAsync<MesPartTempReelModel>(sql, new { REEL_ID = reel_id })).FirstOrDefault();
            }
            return data;
        }

        /// <summary>
        /// 根据物料唯一标识获取物料信息（出库）
        /// </summary>
        /// <param name="reel_id"></param>
        /// <returns></returns>
        public async Task<MesPartTempReelModel> GetReelDataOutput(string reel_id)
        {
            string sql = @"SELECT D.*,
							   D.ID AS DETAIL_ID,
							   P.NAME AS PART_NAME,
							   P.DESCRIPTION AS PART_DESC,
							   M.UNIT,
							   M.PART_TYPE,
							   M.ENABLED
						  FROM MES_PART_TEMP_DETAIL D
							   INNER JOIN IMS_PART P ON D.PART_NO = P.CODE
							   INNER JOIN MES_PART_TEMP_WAREHOUSE M ON M.ID = D.MST_ID
						 WHERE D.STATUS = 1 AND D.REEL_ID=:REEL_ID";

            var data = (await _dbConnection.QueryAsync<MesPartTempReelModel>(sql, new { REEL_ID = reel_id })).FirstOrDefault();
            return data;
        }

        /// <summary>
        /// 获取所有线别列表
        /// </summary>
        /// <returns></returns>
        public List<SfcsEquipmentLinesModel> GetLinesList()
        {
            string sql = "SELECT ID,OPERATION_LINE_NAME AS LINE_NAME from SFCS_OPERATION_LINES WHERE ENABLED = 'Y' ORDER BY ID";
            return (_dbConnection.Query<SfcsEquipmentLinesModel>(sql)).ToList();
        }

        /// <summary>
        /// 入库操作
        /// </summary>
        /// <param name="item"></param>
        /// <returns></returns>
        public async Task<BaseResult> InputWarehouseData(MesPartTempWarehouseAddOrModifyModel item)
        {
            var result = new BaseResult();

            ConnectionFactory.OpenConnection(_dbConnection);
            using (var tran = _dbConnection.BeginTransaction())
            {
                try
                {
                    if (item.MST_ID == 0)
                    {
                        item.MST_ID = await GetSEQID();
                        await InsertMst(item);
                    }
                    else
                        await UpdateMst(item);

                    item.DETAIL_ID = await GetSeqDetail();
                    await InsertDetail(item);

                    await InsertRecord(item);

                    tran.Commit();
                    result.ResultCode = 0;
                    result.ResultMsg = "入库成功！";
                    return result;
                }
                catch (Exception ex)
                {
                    tran.Rollback();
                    result.ResultCode = 106;
                    result.ResultMsg = "入库失败！原因：" + ex.Message;
                    return result;
                }
                finally
                {
                    if (_dbConnection.State != ConnectionState.Closed)
                    {
                        _dbConnection.Close();
                    }
                }
            }
        }

        /// <summary>
        /// 出库操作
        /// </summary>
        /// <param name="item"></param>
        /// <returns></returns>
        public async Task<BaseResult> OutputWarehouseData(MesPartTempWarehouseAddOrModifyModel item)
        {
            var result = new BaseResult();

            var qty = await _dbConnection.ExecuteScalarAsync<decimal>("SELECT USABLE_QTY FROM MES_PART_TEMP_DETAIL WHERE REEL_ID = :REEL_ID", new { REEL_ID = item.REEL_ID });
            if (qty < item.QTY)
            {
                result.ResultCode = 106;
                result.ResultMsg = "出库失败！库存剩余数量有变动，已不够出库，请刷新后重试！";
                return result;
            }

            ConnectionFactory.OpenConnection(_dbConnection);
            using (var tran = _dbConnection.BeginTransaction())
            {
                try
                {
                    await UpdateDetail(item);

                    await InsertRecord(item);

                    item.QTY = -item.QTY;
                    await UpdateMst(item);

                    tran.Commit();
                    result.ResultCode = 0;
                    result.ResultMsg = "出库成功！";
                    return result;
                }
                catch (Exception ex)
                {
                    tran.Rollback();
                    result.ResultCode = 106;
                    result.ResultMsg = "出库失败！原因：" + ex.Message;
                    return result;
                }
                finally
                {
                    if (_dbConnection.State != ConnectionState.Closed)
                    {
                        _dbConnection.Close();
                    }
                }
            }
        }

        /// <summary>
        /// 新增库存信息
        /// </summary>
        /// <param name="item"></param>
        /// <returns></returns>
        private async Task<bool> InsertMst(MesPartTempWarehouseAddOrModifyModel item)
        {
            string sql = @"INSERT INTO MES_PART_TEMP_WAREHOUSE VALUES(:MST_ID,NULL,:PART_TYPE,:PART_NO,:QTY,:UNIT,'Y',NULL,:CREATE_USER,SYSDATE)";

            return await _dbConnection.ExecuteAsync(sql, item) > 0;
        }

        /// <summary>
        /// 更新库存信息
        /// </summary>
        /// <param name="item"></param>
        /// <returns></returns>
        private async Task<bool> UpdateMst(MesPartTempWarehouseAddOrModifyModel item)
        {
            string sql = @"UPDATE MES_PART_TEMP_WAREHOUSE SET QTY = QTY + :QTY WHERE ID = :MST_ID";

            return await _dbConnection.ExecuteAsync(sql, new { item.QTY, item.MST_ID }) > 0;
        }

        /// <summary>
        /// 新增明细信息
        /// </summary>
        /// <param name="item"></param>
        /// <returns></returns>
        private async Task<bool> InsertDetail(MesPartTempWarehouseAddOrModifyModel item)
        {
            item.TOTAL_QTY = item.QTY;
            item.USE_QTY = 0;
            item.USABLE_QTY = item.QTY;
            string sql = @"INSERT INTO MES_PART_TEMP_DETAIL VALUES(:DETAIL_ID,:MST_ID,:REEL_ID,:PART_NO,:TOTAL_QTY,:USE_QTY,:USABLE_QTY,:DATE_CODE,:EXP_DATE,1,NULL)";

            return await _dbConnection.ExecuteAsync(sql, item) > 0;
        }

        /// <summary>
        /// 更新明细信息
        /// </summary>
        /// <param name="item"></param>
        /// <returns></returns>
        private async Task<bool> UpdateDetail(MesPartTempWarehouseAddOrModifyModel item)
        {
            string sql = @"UPDATE MES_PART_TEMP_DETAIL
						   SET USE_QTY = USE_QTY + :QTY,
							   USABLE_QTY = USABLE_QTY - :QTY,
							   STATUS = (CASE USABLE_QTY - :QTY WHEN 0 THEN 2 ELSE 1 END)
						 WHERE ID = :DETAIL_ID";

            return await _dbConnection.ExecuteAsync(sql, item) > 0;
        }

        /// <summary>
        /// 新增操作记录信息
        /// </summary>
        /// <param name="item"></param>
        /// <returns></returns>
        private async Task<bool> InsertRecord(MesPartTempWarehouseAddOrModifyModel item)
        {
            if (item.OPERATION_TYPE == 1)
            {
                item.BEFORE_QTY = 0;
                item.LATER_QTY = item.QTY;
            }
            else
            {
                item.BEFORE_QTY = item.USABLE_QTY;
                item.LATER_QTY = item.USABLE_QTY - item.QTY;
            }

            string sql = @"INSERT INTO MES_PART_TEMP_RECORD VALUES(MES_PART_TEMP_RECORD_SEQ.NEXTVAL,:MST_ID,:DETAIL_ID,:OPERATION_TYPE,:LINE_ID,:BEFORE_QTY,:QTY,:LATER_QTY,:REMARK,:CREATE_USER,SYSDATE)";

            return await _dbConnection.ExecuteAsync(sql, item) > 0;
        }

        /// <summary>
        /// 确认拆分并且返回新条码
        /// </summary>
        /// <param name="item"></param>
        /// <returns></returns>
        public async Task<BaseResult> ReelCodeSplitAsync(MesPartTempReelModel model)
        {

            var result = new BaseResult();
            var p = new DynamicParameters();

            p.Add(":P_REEL_ID", model.ID, DbType.Decimal, ParameterDirection.Input, 20);
            p.Add(":P_QTY", model.S_QTY, DbType.Decimal, ParameterDirection.Input, 20);
            p.Add(":F_QTY", model.F_QTY, DbType.Decimal, ParameterDirection.Input, 20);
            p.Add(":P_NEW_CODE", "", DbType.String, ParameterDirection.Output, 4000);

            await _dbConnection.ExecuteAsync("IMS_STOCK_MANAGER_PKG.CREATEP2_PRO", p, commandType: CommandType.StoredProcedure);
            String new_code = p.Get<String>(":P_NEW_CODE").ToString();
            if (new_code.Equals("-1"))
            {
                result.ResultCode = 106;
                result.ResultMsg = "拆分失败！数量有变动，已不够拆分！";
                return result;
            }
            result.ResultCode = 105;
            result.ResultData = new_code;
            return result;
        }

        /// <summary>
        /// 确认拆分并且返回新条码
        /// </summary>
        /// <param name="item"></param>
        /// <returns></returns>
        public async Task<TableDataModel> ReelCodeSplitAsyncEx(MesReelCodeSplitModel model)
        {
            var result = new TableDataModel();
            result.code = -1; 

            var p = new DynamicParameters();
            ConnectionFactory.OpenConnection(_dbConnection);
            using (var tran = _dbConnection.BeginTransaction())
            {
                try
                {
                    string sql = " SELECT * FROM IMS_REEL WHERE CODE =:CODE ";
                    var reelModel = await _dbConnection.QueryFirstOrDefaultAsync<ImsReel>(sql, new
                    {
                        CODE = model.CODE
                    });
                    if (reelModel == null)
                        return result;

                    p.Add(":P_REEL_ID", reelModel.ID, DbType.Decimal, ParameterDirection.Input, 20);
                    p.Add(":P_QTY", model.S_QTY, DbType.Decimal, ParameterDirection.Input, 20);
                    p.Add(":F_QTY", model.F_QTY, DbType.Decimal, ParameterDirection.Input, 20);
                    p.Add(":P_NEW_CODE", "", DbType.String, ParameterDirection.Output, 4000);

                    await _dbConnection.ExecuteAsync("IMS_STOCK_MANAGER_PKG.CREATEP2_PRO",param: p, commandType: CommandType.StoredProcedure,transaction:tran);
                    String new_code = p.Get<String>(":P_NEW_CODE").ToString();
                    if (new_code.Equals("-1"))
                    {
                        //拆分失败！数量有变动，已不够拆分!
                        throw new Exception("error_qty_split_failed");
                    }

                    string updateSql = @"UPDATE IMS_REEL
						   SET ORIGINAL_QUANTITY = ORIGINAL_QUANTITY - :S_QTY
						 WHERE ID = :ID";

                    int up = await _dbConnection.ExecuteAsync(updateSql, new { ID = reelModel.ID, S_QTY = model.S_QTY },tran);
                    if (up <= 0)
                    {
                        //条码切分失败!
                        throw new Exception("error_operation_split_failed");
                    }

                    #region 处理下架的逻辑

                    if (model.IsDown>0)
                    {
                        string updatePartShelfSql = " UPDATE MES_PART_SHELF SET QTY=NVL(QTY,0)-NVL(:QTY,0) WHERE CODE=:CODE  AND QTY>=:QTY ";
                        int effectNum = await _dbConnection.ExecuteAsync(updatePartShelfSql, new { QTY = model.S_QTY, CODE =model.CODE },tran);
                        if (effectNum <= 0)
                        {
                            //条码切分失败!
                            throw new Exception("error_operation_split_failed");
                        }
                    }

                    #endregion

                    //新条码
                    result.data = new_code;
                    tran.Commit();
                    result.code = 0;
                }
                catch (Exception ex)
                {
                    tran.Rollback();
                    result.msg = ex.Message;
                }
            }

            return result;
        }

        /// <summary>
        /// 更新明细信息
        /// </summary>
        /// <param name="item"></param>
        /// <returns></returns>
        public async Task<BaseResult> ReelCodeUpdate(MesPartTempReelModel item)
        {

            var result = new BaseResult();
            ConnectionFactory.OpenConnection(_dbConnection);
            using (var tran = _dbConnection.BeginTransaction())
            {
                item.S_QTY = item.S_QTY * item.F_QTY;
                try
                {
                    string sql = @"UPDATE IMS_REEL
						   SET ORIGINAL_QUANTITY = ORIGINAL_QUANTITY - :S_QTY
						 WHERE ID = :ID";

                    int up = await _dbConnection.ExecuteAsync(sql, item);
                    if (up > 0)
                    {
                        tran.Commit();
                        result.ResultCode = 105;
                        result.ResultMsg = "拆分成功!";
                    }
                }
                catch (Exception ex)
                {
                    tran.Rollback();
                    result.ResultCode = 106;
                    result.ResultMsg = "拆分失败！原因：" + ex.Message;
                }
                finally
                {
                    if (_dbConnection.State != ConnectionState.Closed)
                    {
                        _dbConnection.Close();
                    }
                }
                return result;
            }
        }

        /// <summary>
        /// 条码解析
        /// </summary>
        /// <param name="reelCode"></param>
        /// <returns></returns>
        public async Task<Reel> GetReel(string reelCode)
        {
            if (LabelPublic.Is2DBarcode(reelCode) == true)
            {
                return await KeepVendorBarcode(reelCode);
            }
            else
            {
                Reel reel = null;
                reelCode = BarcodeFilter.FormatBarcode(reelCode);
                string sql = @"SELECT * FROM IMS_REEL WHERE CODE = :CODE";
                IEnumerable<Reel> reelList = await _dbConnection.QueryAsync<Reel>(sql, new { CODE = reelCode });
                if (reelList.Count() == 0)
                {
                    //同步WMS数据到MES
                    var p = new DynamicParameters();
                    p.Add(":V_CODE", reelCode);
                    await _dbConnection.ExecuteAsync("BSMT.SYNC_IMS_REEL_CODE_FROM_WMS", p, commandType: CommandType.StoredProcedure);
                    reelList = await _dbConnection.QueryAsync<Reel>(sql, new { CODE = reelCode });
                }
                if (reelList.Count() > 0)
                {
                    reel = reelList.FirstOrDefault();
                }
                else
                {
                    reel = new Reel();
                    reel.CODE = reelCode;
                }
                return reel;
            }
        }

        public async Task<Reel> KeepVendorBarcode(string code)
        {
            var reel = LabelPublic.GetReel(code);
            var reelExists = await ReelExists(reel.CODE); ;
            if (reelExists) return reel;

            reel.VendorID = await GetVendorId(reel.VendorCode);
            reel.PartID = await GetPartId(reel.PART_NO);

            var result = await AddImsReel(reel);
            return reel;
        }


        public async Task<bool> ReelExists(string reelCode)
        {
            string sql = @" SELECT COUNT(1) FROM IMS_REEL WHERE CODE = :CODE ";
            var result = await _dbConnection.ExecuteScalarAsync<int>(sql, new
            {
                CODE = reelCode,
            });
            return result > 0;
        }

        public async Task<decimal> GetVendorId(string VendorCode)
        {
            var sql = "SELECT ID FROM IMS_VENDOR WHERE CODE = ':CODE'";
            return await _dbConnection.QueryFirstOrDefaultAsync<decimal>(sql, new { CODE = VendorCode });
        }


        public async Task<decimal> GetPartId(string PART_NO)
        {
            var sql = "SELECT ID FROM IMS_PART WHERE CODE =:CODE";
            return await _dbConnection.QueryFirstOrDefaultAsync<decimal>(sql, new { CODE = PART_NO });
        }

        public async Task<BaseResult> AddImsReel(Reel reel)
        {
            string sql = @"SELECT * FROM IMS_PART WHERE ID=:ID";                                            //获取料号描述
            var partRow = await _dbConnection.QueryAsync(sql, new { ID = reel.PartID });

            String insertImsReelSql = @"INSERT INTO IMS_REEL(ID,VERSION,CODE,BOX_ID,VENDOR_ID,PART_ID,DATE_CODE,
				LOT_CODE,CASE_QTY,ORIGINAL_QUANTITY,CUSTOMER_PN,
				REFERENCE,MSD_LEVEL,ESD_FLAG,DESCRIPTION,TO_LOCATOR_ID,ORIGINAL_SIC_ID) VALUES(
				IMS_REEL_SEQ.NEXTVAL,:VERSION,:CODE,:BOX_ID,:VENDOR_ID,:PART_ID,:DATE_CODE,:LOT_CODE,:CASE_QTY,:ORIGINAL_QUANTITY,:CUSTOMER_PN,
				:REFERENCE,:MSD_LEVEL,:ESD_FLAG,:DESCRIPTION,:TO_LOCATOR_ID,:ORIGINAL_SIC_ID)";
            ConnectionFactory.OpenConnection(_dbConnection);
            var result = new BaseResult();
            using (var tran = _dbConnection.BeginTransaction())
            {
                try
                {
                    await _dbConnection.ExecuteAsync(insertImsReelSql, new
                    {
                        VERSION = 1,
                        CODE = reel.CODE,
                        BOX_ID = -1,
                        VENDOR_ID = reel.VendorID,
                        PART_ID = reel.PartID,
                        DATE_CODE = reel.DateCode,
                        LOT_CODE = reel.LotCode.IsNullOrEmpty() ? reel.DateCode.ToString() : reel.LotCode,
                        CASE_QTY = reel.CaseQty <= 0 ? reel.Quantity : reel.CaseQty,
                        ORIGINAL_QUANTITY = reel.Quantity,
                        CUSTOMER_PN = reel.CustomerPN.IsNullOrEmpty() ? "" : reel.CustomerPN,
                        REFERENCE = reel.REF.IsNullOrEmpty() ? "" : reel.REF,
                        MSD_LEVEL = 1,
                        ESD_FLAG = "N",
                        DESCRIPTION = partRow == null ? "" : partRow.FirstOrDefault().DESCRIPTION,
                        TO_LOCATOR_ID = -1,
                        ORIGINAL_SIC_ID = -1
                    });
                    tran.Commit();
                    result.ResultCode = ResultCodeAddMsgKeys.CommonObjectSuccessCode;
                    result.ResultMsg = ResultCodeAddMsgKeys.CommonObjectSuccessMsg;
                    return result;
                }
                catch (Exception ex)
                {
                    tran.Rollback(); // 回滚事务
                    result.ResultCode = ResultCodeAddMsgKeys.CommonExceptionCode;
                    result.ResultMsg = ex.Message;
                    return result;
                }
                finally
                {
                    if (_dbConnection.State != System.Data.ConnectionState.Closed)
                    {
                        _dbConnection.Close();
                    }
                }
            }
        }
    }
}
