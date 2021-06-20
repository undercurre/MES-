/*
*┌──────────────────────────────────────────────────────────────┐
*│　描    述：接口实现                                                    
*│　作    者：嘉志科技                                            
*│　版    本：2.0    模板代码自动生成                                                
*│　创建时间：2020-04-14 16:44:04                             
*└──────────────────────────────────────────────────────────────┘
*┌──────────────────────────────────────────────────────────────┐
*│　命名空间： JZ.IMS.Repository.Oracle                                  
*│　类    名： SfcsWoRepository                                      
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
    public class SfcsWoRepository : BaseRepository<SfcsWo, Decimal>, ISfcsWoRepository
    {
        public SfcsWoRepository(IOptionsSnapshot<DbOption> options)
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
            string sql = "SELECT ENABLED FROM SFCS_WO WHERE ID=:ID";
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
            string sql = "UPDATE SFCS_WO set ENABLED=:ENABLED WHERE ID=:Id";
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
            string sql = "SELECT SFCS_WO_SEQ.NEXTVAL MY_SEQ FROM DUAL";
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
            string sql = "select count(0) from SFCS_WO where id = :id";
            object result = await _dbConnection.ExecuteScalarAsync(sql, new
            {
                id
            });

            return (Convert.ToInt32(result) > 0);
        }

        /// <summary>
        /// 工单分页
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public async Task<TableDataModel> GetWOList(SfcsWoRequestModel model)
        {
            string conditions = " WHERE SW.ID > 0 ";
            if (!model.PART_NO.IsNullOrWhiteSpace())
            {
                conditions += $"and instr(SW.PART_NO, :PART_NO) > 0 ";
            }
            if (!model.WO_NO.IsNullOrWhiteSpace())
            {
                conditions += $"and instr(SW.WO_NO, :WO_NO) > 0 ";
            }
            if (model.MODEL_ID>0)
            {
                conditions += $"and instr(SW.MODEL_ID, :MODEL_ID) > 0 ";
            }
            if (model.BU_CODE>0)
            {
                conditions += $"and instr(SW.BU_CODE, :BU_CODE) > 0 ";
            }
            if (!string.IsNullOrEmpty(model.START_DATE) && !string.IsNullOrWhiteSpace(model.DUE_DATE))
            {
                //enddate = tempDate.AddDays(1);
                conditions += $" and  SW.START_DATE>= TO_DATE(:START_DATE,'yyyy-MM-dd') and  SW.DUE_DATE>= TO_DATE(:DUE_DATE,'yyyy-MM-dd') ";
            }

            string sql = @" SELECT ROWNUM AS ROWNO,SW.ID,SW.WO_NO,SW.PART_NO,SW.OEM_PN,SW.MODEL_ID,ML.MODEL,SW.SO_ID,SW.WO_STATUS,SW.WO_TYPE,SW.STAGE_CODE,SW.PLANT_CODE,SW.ROUTE_ID,SW.TARGET_QTY,SW.INPUT_QTY,SW.OUTPUT_QTY,
                            SW.TURNIN_TYPE,SW.BU_CODE,SW.CLASSIFICATION,SW.START_DATE,SW.DUE_DATE,SW.ACTUAL_START_DATE,SW.ACTUAL_DUE_DATE,SW.ATTRIBUTE1 SALE,SW.MANUFACTURE_TYPE FROM SFCS_WO SW
                            LEFT JOIN SFCS_MODEL ML ON ML.ID=SW.MODEL_ID   ";
            string pagedSql = SQLBuilderClass.GetPagedSQL(sql, " SW.ID ASC ", conditions);
            var resdata = await _dbConnection.QueryAsync<object>(pagedSql, model);

            string sqlcnt = @"select count(0) FROM SFCS_WO SW 
                              LEFT JOIN SFCS_MODEL ML ON ML.ID=SW.MODEL_ID   " + conditions;

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
        public async Task<decimal> SaveDataByTrans(SfcsWoModel model)
        {
            int result = 1;
            ConnectionFactory.OpenConnection(_dbConnection);
            using (var tran = _dbConnection.BeginTransaction())
            {
                try
                {

                    #region 新增
                    //string insertSql = @"insert into SFCS_WO 
                    //(ID,WO_NO,PART_NO,OEM_PN,MODEL_ID,SO_ID,WO_STATUS,WO_TYPE,STAGE_CODE,PLANT_CODE,ROUTE_ID,TARGET_QTY,INPUT_QTY,OUTPUT_QTY,SCRAP_QTY,TRANSFER_QTY,SHIPPED_QTY,TURNIN_TYPE,BU_CODE,CLASSIFICATION,START_DATE,DUE_DATE,ACTUAL_START_DATE,ACTUAL_DUE_DATE,SHIP_DATE,MATERIAL_START_DATE,MATERIAL_RELEASED_DATE,MANUFACTURE_TYPE,WIP_STATUS_TYPE) 
                    //VALUES (:ID,:WO_NO,:PART_NO,:OEM_PN,:MODEL_ID,:SO_ID,:WO_STATUS,:WO_TYPE,:STAGE_CODE,:PLANT_CODE,:ROUTE_ID,:TARGET_QTY,:INPUT_QTY,:OUTPUT_QTY,:SCRAP_QTY,:TRANSFER_QTY,:SHIPPED_QTY,:TURNIN_TYPE,:BU_CODE,:CLASSIFICATION,:START_DATE,:DUE_DATE,:ACTUAL_START_DATE,:ACTUAL_DUE_DATE,:SHIP_DATE,:MATERIAL_START_DATE,:MATERIAL_RELEASED_DATE,:MANUFACTURE_TYPE,:WIP_STATUS_TYPE)";
                    //if (model.InsertRecords != null && model.InsertRecords.Count > 0)
                    //{
                    //	foreach (var item in model.InsertRecords)
                    //	{
                    //		var newid = await GetSEQID();
                    //		var resdata = await _dbConnection.ExecuteAsync(insertSql, new
                    //		{
                    //			ID = newid,
                    //			item.WO_NO,
                    //			item.PART_NO,
                    //			item.OEM_PN,
                    //			item.MODEL_ID,
                    //			item.SO_ID,
                    //			item.WO_STATUS,
                    //			item.WO_TYPE,
                    //			item.STAGE_CODE,
                    //			item.PLANT_CODE,
                    //			item.ROUTE_ID,
                    //			item.TARGET_QTY,
                    //			item.INPUT_QTY,
                    //			item.OUTPUT_QTY,
                    //			item.SCRAP_QTY,
                    //			item.TRANSFER_QTY,
                    //			item.SHIPPED_QTY,
                    //			item.TURNIN_TYPE,
                    //			item.BU_CODE,
                    //			item.CLASSIFICATION,
                    //			item.START_DATE,
                    //			item.DUE_DATE,
                    //			item.ACTUAL_START_DATE,
                    //			item.ACTUAL_DUE_DATE,
                    //			item.SHIP_DATE,
                    //			item.MATERIAL_START_DATE,
                    //			item.MATERIAL_RELEASED_DATE,
                    //			item.MANUFACTURE_TYPE,
                    //			item.WIP_STATUS_TYPE,

                    //		}, tran);
                    //	}
                    //} 
                    #endregion

                    //更新
                    string updateSql = @"Update SFCS_WO set WO_NO=:WO_NO,PART_NO=:PART_NO,OEM_PN=:OEM_PN,MODEL_ID=:MODEL_ID,SO_ID=:SO_ID,WO_STATUS=:WO_STATUS,WO_TYPE=:WO_TYPE,STAGE_CODE=:STAGE_CODE,PLANT_CODE=:PLANT_CODE,ROUTE_ID=:ROUTE_ID,TURNIN_TYPE=:TURNIN_TYPE
						where ID=:ID ";
                    if (model.UpdateRecords != null && model.UpdateRecords.Count > 0)
                    {
                        foreach (var item in model.UpdateRecords)
                        {
                            var resdata = await _dbConnection.ExecuteAsync(updateSql, new
                            {
                                item.ID,
                                item.WO_NO,
                                item.PART_NO,
                                item.OEM_PN,
                                item.MODEL_ID,
                                item.SO_ID,
                                item.WO_STATUS,
                                item.WO_TYPE,
                                item.STAGE_CODE,
                                item.PLANT_CODE,
                                item.ROUTE_ID,
                                item.TARGET_QTY,
                                item.INPUT_QTY,
                                item.OUTPUT_QTY,
                                item.SCRAP_QTY,
                                item.SHIPPED_QTY,
                                item.TURNIN_TYPE,
                                item.BU_CODE,
                                item.CLASSIFICATION,
                                item.START_DATE,
                                item.DUE_DATE,
                                item.MANUFACTURE_TYPE,
                                item.WIP_STATUS_TYPE,
                            }, tran);
                        }
                    }

                    #region 删除
                    //string deleteSql = @"Delete from SFCS_WO where ID=:ID ";
                    //if (model.RemoveRecords != null && model.RemoveRecords.Count > 0)
                    //{
                    //	foreach (var item in model.RemoveRecords)
                    //	{
                    //		var resdata = await _dbConnection.ExecuteAsync(deleteSql, new
                    //		{
                    //			item.ID
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
        /// 判断工单是否存在
        /// </summary>
        public async Task<bool> ConfirmWorkOrderExisted(string woNo)
        {
            string sql = @"SELECT COUNT(ID) FROM SFCS_WO WHERE WO_NO = :WO_NO";
            int cnt = await _dbConnection.ExecuteScalarAsync<int>(sql, new { WO_NO = woNo });
            if (cnt == 0)
                return false;
            else
                return true;
        }

        /// <summary>
        /// 查询开工工单
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public async Task<TableDataModel> GetProductionWO(SfcsProductionRequestModel model)
        {
            string sWhere = "";
            if (!model.WO_NO.IsNullOrEmpty())
            {
                sWhere += $"WHERE instr(WO_NO, :WO_NO) > 0 ";
            }

            string sQuery = @" SELECT ROWNUM as rowno,WO_NO FROM (SELECT DISTINCT T.WO_NO FROM ( SELECT WO_NO FROM SFCS_PRODUCTION UNION SELECT WO_NO FROM SMT_PRODUCTION )T) ";

            string pagedSql = SQLBuilderClass.GetPagedSQL(sQuery, " WO_NO DESC ", sWhere);
            var resdata = await _dbConnection.QueryAsync<object>(pagedSql, model);

            sQuery = String.Format("SELECT COUNT(0) FROM (SELECT DISTINCT T.WO_NO FROM ( SELECT WO_NO FROM SFCS_PRODUCTION UNION SELECT WO_NO FROM SMT_PRODUCTION )T {0}) ", sWhere);

            int cnt = await _dbConnection.ExecuteScalarAsync<int>(sQuery, model);

            return new TableDataModel
            {
                count = cnt,
                data = resdata?.ToList(),
            };
        }
    }
}