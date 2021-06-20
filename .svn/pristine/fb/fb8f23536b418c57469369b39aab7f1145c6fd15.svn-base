/*
*┌──────────────────────────────────────────────────────────────┐
*│　描    述：周转箱打印表接口实现                                                    
*│　作    者：嘉志科技                                            
*│　版    本：2.0    模板代码自动生成                                                
*│　创建时间：2020-08-18 10:46:57                             
*└──────────────────────────────────────────────────────────────┘
*┌──────────────────────────────────────────────────────────────┐
*│　命名空间： JZ.IMS.Repository.Oracle                                  
*│　类    名： MesBatchPringRepository                                      
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
    public class MesBatchPringRepository:BaseRepository<MesBatchPring,Decimal>, IMesBatchPringRepository
    {
        public MesBatchPringRepository(IOptionsSnapshot<DbOption> options)
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
			string sql = "SELECT ENABLED FROM MES_BATCH_PRING WHERE ID=:ID";
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
			string sql = "UPDATE MES_BATCH_PRING set ENABLED=:ENABLED WHERE ID=:Id";
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
			string sql = "SELECT MES_BATCH_PRING_SEQ.NEXTVAL MY_SEQ FROM DUAL";
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
			string sql = "select count(0) from MES_BATCH_PRING where id = :id";
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
		public async Task<decimal> SaveDataByTrans(MesBatchPringModel model)
		{
			int result = 1;
			ConnectionFactory.OpenConnection(_dbConnection);
			using (var tran = _dbConnection.BeginTransaction())
			{
				try
				{
					StringBuilder stringBuilder = new StringBuilder();
					String PrintDataHead = "BOX_NO,LOC_NO,LINE_NAME,PRODUCT_TIME,WO_NO,TARGE_QTY,QTY,PN,MODEL,QR_NO";
					stringBuilder.AppendLine(PrintDataHead);
					//新增
					string insertSql = @"insert into MES_BATCH_PRING 
					(ID,BT_MANAGER_ID,CODE,QTY,CARTON_NO,CREATE_TIME,CREATOR) 
					VALUES (:ID,:BT_MANAGER_ID,:CODE,:QTY,:CARTON_NO,:CREATE_TIME,:CREATOR)";
					String selectPrintDataSql = @"select  MBM.LOC_NO ,MBP.CODE CARTON_NO, MBM.WO_NO, MBM.PART_NO PN, SW.TARGET_QTY TARGE_QTY,MBP.QTY, MBM.LINE_NAME, MBM.DESCRIPTION MODEL, 
						MBP.CODE QR_NO,MBM.PRODUCTION_TIME
						from MES_BATCH_PRING MBP, MES_BATCH_MANAGER MBM, SFCS_WO SW 
						where MBP.BT_MANAGER_ID = MBM.ID
						AND SW.WO_NO = MBM.WO_NO AND MBP.ID = :ID";
					if (model.InsertRecords != null && model.InsertRecords.Count > 0)
					{
						String user = "";
						foreach (var item in model.InsertRecords)
						{
							var newid = await GetSEQID();
							var resdata = await _dbConnection.ExecuteAsync(insertSql, new
							{
								ID = newid,
								item.BT_MANAGER_ID,
								item.CODE,
								item.QTY,
								item.CARTON_NO,
								item.CREATE_TIME,
								item.CREATOR,
							}, tran);
							user = item.CREATOR;
							var printDatas = await _dbConnection.QueryAsync(selectPrintDataSql,
								new { ID = newid }, tran);
							foreach(var printData in printDatas)
                            {
								stringBuilder.Append(String.Format("{0},{1},{2},{3},{4},{5},{6},{7},{8},{9}",
									printData.CARTON_NO, printData.LOC_NO, printData.LINE_NAME, printData.PRODUCTION_TIME, printData.WO_NO,
									printData.TARGE_QTY, printData.QTY, printData.PN, printData.MODEL, printData.QR_NO));
								stringBuilder.AppendLine();
							}
							stringBuilder.Remove(stringBuilder.Length - 1, 1);
						}
						String printDataBytes = stringBuilder.ToString();
						decimal printTaskId = await _dbConnection.ExecuteScalarAsync<decimal>("select SFCS_PRINT_TASKS_SEQ.NEXTVAL from dual");

						decimal printFileId = await _dbConnection.ExecuteScalarAsync<decimal>("select ID from SFCS_PRINT_FILES where LABEL_TYPE = 4 and ENABLED = 'Y' order by id desc");
						String insertPrintTaskSql = @"INSERT INTO SFCS_PRINT_TASKS(ID,PRINT_FILE_ID,OPERATOR,CREATE_TIME,PRINT_STATUS,PRINT_DATA)VALUES(
					:ID,:PRINT_FILE_ID,:OPERATOR,sysdate,0,:PRINT_DATA)";
					 await	_dbConnection.ExecuteAsync(insertPrintTaskSql, new
						{
							ID = printTaskId,
							PRINT_FILE_ID = printFileId,
							OPERATOR = user,
							PRINT_DATA = printDataBytes
						}, tran);
						result =(int)printTaskId;
					}
					////更新
					//string updateSql = @"Update MES_BATCH_PRING set BT_MANAGER_ID=:BT_MANAGER_ID,CODE=:CODE,QTY=:QTY,CARTON_NO=:CARTON_NO,CREATE_TIME=:CREATE_TIME,CREATOR=:CREATOR  
					//	where ID=:ID ";
					//if (model.UpdateRecords != null && model.UpdateRecords.Count > 0)
					//{
					//	foreach (var item in model.UpdateRecords)
					//	{
					//		var resdata = await _dbConnection.ExecuteAsync(updateSql, new
					//		{
					//			item.ID,
					//			item.BT_MANAGER_ID,
					//			item.CODE,
					//			item.QTY,
					//			item.CARTON_NO,
					//			item.CREATE_TIME,
					//			item.CREATOR,
					//		}, tran);
					//	}
					//}
					////删除
					//string deleteSql = @"Delete from MES_BATCH_PRING where ID=:ID ";
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