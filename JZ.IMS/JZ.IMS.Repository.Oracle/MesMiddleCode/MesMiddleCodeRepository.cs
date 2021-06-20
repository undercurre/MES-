/*
*┌──────────────────────────────────────────────────────────────┐
*│　描    述：中转码接口实现                                                    
*│　作    者：嘉志科技                                            
*│　版    本：2.0    模板代码自动生成                                                
*│　创建时间：2020-11-12 14:25:16                             
*└──────────────────────────────────────────────────────────────┘
*┌──────────────────────────────────────────────────────────────┐
*│　命名空间： JZ.IMS.Repository.Oracle                                  
*│　类    名： MesMiddleCodeRepository                                      
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
using System.Text;

namespace JZ.IMS.Repository.Oracle
{
    public class MesMiddleCodeRepository:BaseRepository<MesMiddleCode,Decimal>, IMesMiddleCodeRepository
    {
        public MesMiddleCodeRepository(IOptionsSnapshot<DbOption> options)
        {
            _dbOption =options.Get("iWMS");
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
			string sql = "SELECT ENABLED FROM MES_MIDDLE_CODE WHERE ID=:ID";
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
			string sql = "UPDATE MES_MIDDLE_CODE set ENABLED=:ENABLED WHERE ID=:Id";
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
			string sql = "SELECT MES_MIDDLE_CODE_SEQ.NEXTVAL MY_SEQ FROM DUAL";
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
			string sql = "select count(0) from MES_MIDDLE_CODE where id = :id";
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
		public async Task<decimal> SaveDataByTrans(MesMiddleCodeModel model)
		{
			int result = 1;
			ConnectionFactory.OpenConnection(_dbConnection);
			using (var tran = _dbConnection.BeginTransaction())
			{
				try
				{
					//新增
					string insertSql = @"insert into MES_MIDDLE_CODE 
					(ID,CODE,CHREATE_TIME,CREATOR,STATUS) 
					VALUES (:ID,:CODE,:CHREATE_TIME,:CREATOR,:STATUS)";
					if (model.InsertRecords != null && model.InsertRecords.Count > 0)
					{
						foreach (var item in model.InsertRecords)
						{
							var newid = await GetSEQID();
							var resdata = await _dbConnection.ExecuteAsync(insertSql, new
							{
								ID = newid,
								item.CODE,
								item.CHREATE_TIME,
								item.CREATOR,
								item.STATUS,
							}, tran);
						}
					}
					//更新
					string updateSql = @"Update MES_MIDDLE_CODE set CODE=:CODE,CHREATE_TIME=:CHREATE_TIME,CREATOR=:CREATOR,STATUS=:STATUS  
						where ID=:ID ";
					if (model.UpdateRecords != null && model.UpdateRecords.Count > 0)
					{
						foreach (var item in model.UpdateRecords)
						{
							var resdata = await _dbConnection.ExecuteAsync(updateSql, new
							{
								item.ID,
								item.CODE,
								item.CHREATE_TIME,
								item.CREATOR,
								item.STATUS,
							}, tran);
						}
					}
					//删除
					string deleteSql = @"Delete from MES_MIDDLE_CODE where ID=:ID ";
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
        /// 保存绑定中转码采集的数据
        /// </summary>
        /// <param name="model"></param>
        /// <param name="tran"></param>
        /// <returns></returns>
        public async Task<bool> SaveMiddleCodeData(CollectMiddleCodeDataRequestModel model, System.Data.IDbTransaction tran)
        {
            int resdata = 0; decimal newid = 0;
			//获取中转码数据
			if (model.MIDDLECODE == null)
			{
				newid = await GetSEQID();
				//生成中转码
				string result = Core.Utilities.RadixConvertPublic.RadixConvert(newid.ToString(), ViewModels.GlobalVariables.DecRadix, ViewModels.GlobalVariables.Base36Redix);
                string ReleasedSequence = result.PadLeft(6, '0');
                string yymmdd = QueryEx<string>("SELECT TO_CHAR(SYSDATE,'YYMMDD') YYMMDD FROM DUAL ").FirstOrDefault();
                String middleCode = "ZZ" + yymmdd + ReleasedSequence;

                //1.根据中转码生成规则生成中转码 2.记录到MES_MIDDLE_CODE  STATUS状态（0：待用；1：在 用；2:报废）
                string insertMiddleCodeSql = @"INSERT INTO MES_MIDDLE_CODE  (ID,CODE,CHREATE_TIME,CREATOR,STATUS) VALUES (:ID,:CODE,SYSDATE,:CREATOR,'1')";
                resdata = await _dbConnection.ExecuteAsync(insertMiddleCodeSql, new { ID = newid, CODE = middleCode, CREATOR = model.MANAGER.USER_NAME }, tran);
            }
            else
            {
				newid = model.MIDDLECODE.ID;
				string updateMiddleCodeSql = @"UPDATE MES_MIDDLE_CODE SET STATUS='1' WHERE ID=:ID ";
				resdata = await _dbConnection.ExecuteAsync(updateMiddleCodeSql, new { ID = newid }, tran);
			}
			model.MIDDLECODE = QueryEx<MesMiddleCode>("SELECT * FROM MES_MIDDLE_CODE WHERE ID = :ID  AND STATUS = '1'", new { ID = newid }, tran).FirstOrDefault();
			if (resdata <= 0 || model.MIDDLECODE == null) { throw new Exception("SAVE_MIDDLE_DATA_ERROR"); }

            //新增中转码使用日志 STATUS状态（0：未解绑；1：已解绑）
            string getIdsql = "SELECT MES_MIDDLE_CODE_LOG_SEQ.NEXTVAL MY_SEQ FROM DUAL";
            var logId = await _dbConnection.ExecuteScalarAsync(getIdsql);
            string insertMiddleCodeLogSql = @"INSERT INTO MES_MIDDLE_CODE_LOG (ID,CODE_ID,CODE,SN,COLLECT_QTY,WO_NO,LINK_SITE_ID,LINK_TIME,LINK_USER,STATUS) VALUES (:ID,:CODE_ID,:CODE,:SN,:COLLECT_QTY,:WO_NO,:LINK_SITE_ID,SYSDATE,:LINK_USER,'0')";
            resdata = await _dbConnection.ExecuteAsync(insertMiddleCodeLogSql, new
            {
                ID = logId,
                CODE_ID = model.MIDDLECODE.ID,
                CODE = model.MIDDLECODE.CODE,
                SN = model.SN,
                COLLECT_QTY = model.COLLECT_QTY,
                WO_NO = model.WO_NO,
                LINK_SITE_ID = model.OPERATIONSITES.ID,
                LINK_USER = model.MANAGER.USER_NAME
            }, tran);
            if (resdata <= 0) { throw new Exception("SAVE_MIDDLE_DATA_ERROR"); }

            //部件码绑定记录
            //CODE_LOG_ID 中转条码ID CODE 关键部件的条码 CODE_TYPE条码类型（0:物料条码；1：半成 品条码；2：周转箱条码）
            //部件条码目前有三种: 第一种是半成品在SFCS_RUNCARD 表中的SN :第二种是仓库条码IMS_REEL的CODE: 第三种是周转码MES_BATCH_PRING的CARTON_NO
            int partQty = 0;//已收集的零件数量
            string insertMiddleLogCollectSql = @"INSERT INTO MES_MIDDLE_LOG_COLLECT  (ID,CODE_LOG_ID,CODE,CODE_TYPE) VALUES (:ID,:CODE_LOG_ID,:CODE,:CODE_TYPE)";
            getIdsql = "SELECT MES_MIDDLE_LOG_COLLECT_SEQ.NEXTVAL MY_SEQ FROM DUAL";
            if (model.IMSREEL != null)
            {
                partQty++;
                var collectId = await _dbConnection.ExecuteScalarAsync(getIdsql);
                resdata = await _dbConnection.ExecuteAsync(insertMiddleLogCollectSql, new { ID = collectId, CODE_LOG_ID = logId, CODE = model.IMSREEL.CODE, CODE_TYPE = "0" }, tran);
                if (resdata <= 0) { throw new Exception("SAVE_MIDDLE_DATA_ERROR"); }
            }
            if (model.RUNCARD != null)
            {
                partQty++;
                var collectId = await _dbConnection.ExecuteScalarAsync(getIdsql);
                resdata = await _dbConnection.ExecuteAsync(insertMiddleLogCollectSql, new { ID = collectId, CODE_LOG_ID = logId, CODE = model.RUNCARD.SN, CODE_TYPE = "1" }, tran);
                if (resdata <= 0) { throw new Exception("SAVE_MIDDLE_DATA_ERROR"); }
            }
            if (model.BATCHPRING != null)
            {
                partQty++;
                var collectId = await _dbConnection.ExecuteScalarAsync(getIdsql);
                resdata = await _dbConnection.ExecuteAsync(insertMiddleLogCollectSql, new { ID = collectId, CODE_LOG_ID = logId, CODE = model.BATCHPRING.CARTON_NO, CODE_TYPE = "2" }, tran);
                if (resdata <= 0) { throw new Exception("SAVE_MIDDLE_DATA_ERROR"); }
            }
            //校验部件数和已收集数量是否相同
            if (model.COLLECT_QTY != partQty) { throw new Exception("COLLECT_QTY_NOT_OVER"); }

            return true;
        }

        /// <summary>
        /// 解绑中转码数据
        /// </summary>
        /// <param name="model"></param>
        /// <param name="isPrintSN"></param>
        /// <param name="tran"></param>
        /// <returns></returns>
        public async Task<int> UnbindMiddleCodeData(CollectMiddleCodeDataRequestModel model, bool isPrintSN, System.Data.IDbTransaction tran)
        {
            int resdata = 0;
			int PrintFileId = 0;
			int printTaskId = await _dbConnection.ExecuteScalarAsync<int>("SELECT SFCS_PRINT_TASKS_SEQ.NEXTVAL MY_SEQ FROM DUAL");
            if (isPrintSN)
            {
                //打印SN
                String printMappSql = @"SELECT SPF.* FROM SFCS_PRINT_FILES_MAPPING SPFM, SFCS_PRINT_FILES  SPF 
                        WHERE SPFM.PRINT_FILE_ID = SPF.ID AND SPFM.ENABLED = 'Y' AND SPF.ENABLED = 'Y' AND SPF.LABEL_TYPE = 1";
                String printMappSqlByPn = printMappSql + " AND SPFM.PART_NO = :PART_NO";
                SfcsPrintFiles sfcsPrintFiles = null;
                List<SfcsPrintFiles> sfcsPrintMapplist = null;
                sfcsPrintMapplist = await QueryAsyncEx<SfcsPrintFiles>(printMappSqlByPn, new { PART_NO = model.PN.PART_NO });

                if (sfcsPrintMapplist == null)
                {
                    String printMappSqlByModel = printMappSql + " AND SPFM.MODEL_ID = :MODEL_ID";
                    sfcsPrintMapplist = await QueryAsyncEx<SfcsPrintFiles>(printMappSqlByModel, new { MODEL_ID = model.PN.MODEL_ID });
                }
                if (sfcsPrintMapplist == null)
                {
                    String printMappSqlByFamilly = printMappSql + " AND SPFM.PRODUCT_FAMILY_ID = :PRODUCT_FAMILY_ID";
                    sfcsPrintMapplist = await QueryAsyncEx<SfcsPrintFiles>(printMappSqlByFamilly, new { PRODUCT_FAMILY_ID = model.PN.FAMILY_ID  });
                }
                if (sfcsPrintMapplist == null)
                {
                    String printMappSqlByCustor = printMappSql + " AND SPFM.CUSTOMER_ID = :CUSTOMER_ID";

                    sfcsPrintMapplist = await QueryAsyncEx<SfcsPrintFiles>(printMappSqlByCustor, new { CUSTOMER_ID = model.PN.CUSTOMER_ID  });
                }
                //默认产品条码模板
                if (sfcsPrintMapplist == null || sfcsPrintMapplist.Count <= 0)
                {
                    sfcsPrintMapplist = await QueryAsyncEx<SfcsPrintFiles>(printMappSqlByPn, new { PART_NO = "000000"  });
                }
                if (sfcsPrintMapplist != null && sfcsPrintMapplist.Count > 0)
                {
                    sfcsPrintFiles = sfcsPrintMapplist.FirstOrDefault();
                    if (sfcsPrintFiles != null) { PrintFileId = (int)sfcsPrintFiles.ID; }
                }
                else
                {
                    throw new Exception("ERR_SETPRODUCTPRINTFILE");
                }
            }
            else
            {
                //打印中转码
				PrintFileId = await _dbConnection.ExecuteScalarAsync<int>("SELECT ID FROM SFCS_PRINT_FILES WHERE FILE_NAME='中转条码标签' AND ENABLED = 'Y'");
            }
            if (PrintFileId < 1) { throw new Exception("ERR_SETPRODUCTPRINTFILE"); }

            StringBuilder printData = new StringBuilder();
            printData.AppendLine("SN,CODE");
            printData.AppendLine(String.Format("{0},{1}", model.SN, model.MIDDLECODE.CODE));
            string insertPrintTaskSql = @"INSERT INTO SFCS_PRINT_TASKS(ID,PRINT_FILE_ID,OPERATOR,CREATE_TIME,PRINT_STATUS,PRINT_DATA,PART_NO,WO_NO)VALUES(:ID,:PRINT_FILE_ID,:OPERATOR,SYSDATE,0,:PRINT_DATA,:PART_NO,:WO_NO)";
			resdata = await _dbConnection.ExecuteAsync(insertPrintTaskSql, new { 
				ID = printTaskId, 
				PRINT_FILE_ID = PrintFileId,
				OPERATOR = model.MANAGER.USER_NAME,
				PRINT_DATA = printData.ToString(),
                PART_NO = model.PN.PART_NO,
				WO_NO = model.WO_NO
			}, tran);
			if (resdata <= 0) { throw new Exception("UNBIND_MIDDLE_DATA_ERROR"); }

			MesMiddleCodeLog middleCodeLog = QueryEx<MesMiddleCodeLog>("SELECT * FROM MES_MIDDLE_CODE_LOG WHERE CODE_ID = :ID AND SN = :SN AND STATUS = '0'", new { CODE_ID = model.MIDDLECODE.ID, SN = model.SN }, tran).FirstOrDefault();
            //STATUS状态（0：待用；1：在 用；2:报废）
            string updateSql = @"UPDATE MES_MIDDLE_CODE SET STATUS='0' WHERE ID=:ID ";
            resdata = await _dbConnection.ExecuteAsync(updateSql, new { ID = model.MIDDLECODE.ID }, tran);
            if (resdata <= 0 || middleCodeLog == null) { throw new Exception("UNBIND_MIDDLE_DATA_ERROR"); }

            //STATUS状态（0：未解绑；1：已解绑）
            updateSql = @"UPDATE MES_MIDDLE_CODE_LOG SET STATUS='1' WHERE ID=:ID ";
            resdata = await _dbConnection.ExecuteAsync(updateSql, new { ID = middleCodeLog.ID }, tran);
            if (resdata <= 0) { throw new Exception("UNBIND_MIDDLE_DATA_ERROR"); }

			return printTaskId;
        }
    }
}