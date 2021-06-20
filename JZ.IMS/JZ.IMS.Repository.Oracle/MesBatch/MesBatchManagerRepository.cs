/*
*┌──────────────────────────────────────────────────────────────┐
*│　描    述：批次管理表接口实现                                                    
*│　作    者：嘉志科技                                            
*│　版    本：2.0    模板代码自动生成                                                
*│　创建时间：2020-08-17 15:48:19                             
*└──────────────────────────────────────────────────────────────┘
*┌──────────────────────────────────────────────────────────────┐
*│　命名空间： JZ.IMS.Repository.Oracle                                  
*│　类    名： MesBatchManagerRepository                                      
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
    public class MesBatchManagerRepository:BaseRepository<MesBatchManager,Decimal>, IMesBatchManagerRepository
    {
        public MesBatchManagerRepository(IOptionsSnapshot<DbOption> options)
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
			string sql = "SELECT ENABLED FROM MES_BATCH_MANAGER WHERE ID=:ID";
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
			string sql = "UPDATE MES_BATCH_MANAGER set ENABLED=:ENABLED WHERE ID=:Id";
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
			string sql = "SELECT MES_BATCH_MANAGER_SEQ.NEXTVAL MY_SEQ FROM DUAL";
			var result = await _dbConnection.ExecuteScalarAsync(sql);
			return (decimal)result;
		}

		/// <summary>
		/// 获取附件表的序列
		/// </summary>
		/// <returns></returns>
		public async Task<decimal> GetFileSEQID()
		{
			string sql = "SELECT MES_BATCH_RESOURCES_SEQ.NEXTVAL MY_SEQ FROM DUAL";
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
			string sql = "select count(0) from MES_BATCH_MANAGER where id = :id";
			object result = await _dbConnection.ExecuteScalarAsync(sql, new
			{
				id
			});

			return (Convert.ToInt32(result) > 0);
		}

		/// <summary>
		/// 根据批次号判断批次管理表中是否存在该批次号
		/// </summary>
		/// <param name="locno"></param>
		/// <returns></returns>
		public async Task<bool> JudgeLocNoIsExistByLocNo(string locno)
		{
			string sql = "select count(0) from MES_BATCH_MANAGER where   LOC_NO = :locno";
			object result = await _dbConnection.ExecuteScalarAsync(sql, new
			{
				locno
			});

			return (Convert.ToInt32(result) > 0);
		}



		/// <summary>
		/// 保存数据
		/// </summary>
		/// <param name="model"></param>
		/// <returns></returns>
		public async Task<decimal> SaveDataByTrans(MesBatchManagerModel model)
		{
			int result = 1;
			ConnectionFactory.OpenConnection(_dbConnection);
			using (var tran = _dbConnection.BeginTransaction())
			{
				try
				{
					//新增
					string insertSql = @"insert into MES_BATCH_MANAGER 
					(ID,LINE_ID,LINE_NAME,LOC_NO,WO_NO,PART_NO,DESCRIPTION,PRODUCTION_TIME,PRODUCTION_QTY) 
					VALUES (:ID,:LINE_ID,:LINE_NAME,:LOC_NO,:WO_NO,:PART_NO,:DESCRIPTION,:PRODUCTION_TIME,:PRODUCTION_QTY)";
					if (model.InsertRecords != null && model.InsertRecords.Count > 0)
					{
						foreach (var item in model.InsertRecords)
						{
							var newid = await GetSEQID();
							var resdata = await _dbConnection.ExecuteAsync(insertSql, new
							{
								ID = newid,
								item.LINE_ID,
								item.LINE_NAME,
								item.LOC_NO,
								item.WO_NO,
								item.PART_NO,
								item.DESCRIPTION,
								item.PRODUCTION_TIME,
								item.PRODUCTION_QTY,
							}, tran);

							//新增批次管理信息时添加附件信息（需要主表ID）
							string insertFileSql = @"insert into MES_BATCH_RESOURCES 
					(ID,BT_MANAGER_ID,LINE_NAME,RESOURCE_TYPE,RESOURCES_URL,RESOURCE_NAME,RESOURCE_SIZE,UPLOAD_USER,CREATE_TIME) 
					VALUES (:ID,:BT_MANAGER_ID,:LINE_NAME,:RESOURCE_TYPE,:RESOURCES_URL,:RESOURCE_NAME,:RESOURCE_SIZE,:UPLOAD_USER,:CREATE_TIME)";
							if (model.InsertFileRecords != null && model.InsertFileRecords.Count > 0)
							{
								foreach (var files in model.InsertFileRecords)
								{
									var fileid = await GetFileSEQID();
									var filedata = await _dbConnection.ExecuteAsync(insertFileSql, new
									{
										ID = fileid,
										BT_MANAGER_ID=newid,
										files.LINE_NAME,
										files.RESOURCE_TYPE,
										files.RESOURCES_URL,
										files.RESOURCE_NAME,
										files.RESOURCE_SIZE,
										files.UPLOAD_USER,
										files.CREATE_TIME,

									}, tran);
								}
							}


						}
					}
					//更新
					string updateSql = @"Update MES_BATCH_MANAGER set LINE_ID=:LINE_ID,LINE_NAME=:LINE_NAME,LOC_NO=:LOC_NO,WO_NO=:WO_NO,PART_NO=:PART_NO,DESCRIPTION=:DESCRIPTION,PRODUCTION_TIME=:PRODUCTION_TIME,PRODUCTION_QTY=:PRODUCTION_QTY  
						where ID=:ID ";
					if (model.UpdateRecords != null && model.UpdateRecords.Count > 0)
					{
						foreach (var item in model.UpdateRecords)
						{
							var resdata = await _dbConnection.ExecuteAsync(updateSql, new
							{
								item.ID,
								item.LINE_ID,
								item.LINE_NAME,
								item.LOC_NO,
								item.WO_NO,
								item.PART_NO,
								item.DESCRIPTION,
								item.PRODUCTION_TIME,
								item.PRODUCTION_QTY,
							}, tran);
						}
					}
					//编辑批次管理信息时新增附件信息
					string editinsertFileSql = @"insert into MES_BATCH_RESOURCES 
					(ID,BT_MANAGER_ID,LINE_NAME,RESOURCE_TYPE,RESOURCES_URL,RESOURCE_NAME,RESOURCE_SIZE,UPLOAD_USER,CREATE_TIME) 
					VALUES (:ID,:BT_MANAGER_ID,:LINE_NAME,:RESOURCE_TYPE,:RESOURCES_URL,:RESOURCE_NAME,:RESOURCE_SIZE,:UPLOAD_USER,:CREATE_TIME)";
					if (model.EditInsertFileRecords != null && model.EditInsertFileRecords.Count > 0)
					{
						foreach (var item in model.EditInsertFileRecords)
						{
							var newid = await GetFileSEQID();
							var resdata = await _dbConnection.ExecuteAsync(editinsertFileSql, new
							{
								ID = newid,
								item.BT_MANAGER_ID,
								item.LINE_NAME,
								item.RESOURCE_TYPE,
								item.RESOURCES_URL,
								item.RESOURCE_NAME,
								item.RESOURCE_SIZE,
								item.UPLOAD_USER,
								item.CREATE_TIME,
							}, tran);
						}
					}


					////删除
					//string deleteSql = @"Delete from MES_BATCH_MANAGER where ID=:ID ";
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



		/// <summary>
		/// 根据批次号获取新增的数据
		/// </summary>
		/// <param name="model"></param>
		/// <returns></returns>
		public async Task<TableDataModel> GetMesBatchDataByLOCNO(MesBatchManagerRequestModel model)
		{
			string conditions = "where 1=1 ";

			if (!model.LINE_ID.IsNullOrWhiteSpace())
			{
				conditions += $"and ( SP.LINE_ID=:LINE_ID)";
			}
			if (!model.StartDate.IsNullOrWhiteSpace())
			{
				conditions += string.Format(@"and (SP.START_TIME >= to_date('{0}','yyyy-MM-dd HH24:mi:ss'))", model.StartDate.ToString());
			}
			if (!model.EndDate.IsNullOrWhiteSpace())
			{
				conditions += string.Format(@"and (SP.START_TIME < to_date('{0}','yyyy-MM-dd HH24:mi:ss'))", model.EndDate.ToString());
			}
			if (!model.Key.IsNullOrWhiteSpace())
			{
				conditions += $"and (instr(SP.WO_NO, :Key) > 0 )";
			}
			string sql = string.Format(@" SELECT * FROM (
											SELECT ROWNUM AS ROWNO, TEMP.* FROM (
													SELECT T.*
													FROM (SELECT ROW_NUMBER()OVER(PARTITION BY TE.LOC_NO ORDER BY TE.ORDER_NO DESC)RN, TE.* FROM (SELECT SR.ORDER_NO,SP.LOC_NO,SP.LINE_ID, SL.LINE_NAME,SP.MODEL AS DESCRIPTION,SP.WO_NO,SP.PCB_PN AS PART_NO,SP.START_TIME AS PRODUCTION_TIME,OUTPUT_QTY
														FROM SMT_PRODUCTION SP
														INNER JOIN SMT_LINES SL ON SP.LINE_ID = SL.ID 
														INNER JOIN SMT_ROUTE SR ON SP.STATION_ID = SR.STATION_ID
														{0}
													) TE)T
												 WHERE T.RN=1
												) TEMP   
										) WHERE ROWNO BETWEEN ((:PAGE-1)*:LIMIT+1) AND (:LIMIT*:PAGE) ", conditions);

			var resdata = await _dbConnection.QueryAsync<object>(sql,model);
			string sqlcnt = string.Format(@" select count(0) from (
											SELECT T.*
													FROM (SELECT ROW_NUMBER()OVER(PARTITION BY TE.LOC_NO ORDER BY TE.ORDER_NO DESC)RN, TE.* FROM (SELECT SR.ORDER_NO,SP.LOC_NO,SP.LINE_ID, SL.LINE_NAME,SP.MODEL AS DESCRIPTION,SP.WO_NO,SP.PCB_PN AS PART_NO,SP.START_TIME AS PRODUCTION_TIME,OUTPUT_QTY
														FROM SMT_PRODUCTION SP
														INNER JOIN SMT_LINES SL ON SP.LINE_ID = SL.ID 
														INNER JOIN SMT_ROUTE SR ON SP.STATION_ID = SR.STATION_ID
														{0}
													) TE)T
												 WHERE T.RN=1
										 ) temp ", conditions);
			int cnt = await _dbConnection.ExecuteScalarAsync<int>(sqlcnt,model);
			return new TableDataModel
			{
				count = cnt,
				data = resdata?.ToList(),
			};
		}

		/// <summary>
		/// 根据主表批次号获取物料报表信息
		/// </summary>
		/// <param name="model"></param>
		/// <returns></returns>
		public async Task<TableDataModel> GetMesMaterialInfoList(MesMaterialInfoRequestModel model)
		{
			string conditions = "where 1=1 ";
			if (!model.LOC_NO.IsNullOrWhiteSpace())
			{
				conditions += $"and (PR.LOC_NO=:LOC_NO)";
			}
			string sql = string.Format(@"select * from (  
											select  ROWNUM as rowno, temp.* from (   
													select SRL.REEL_ID,PA.CODE,PA.DESCRIPTION,SRL.USED_QTY,SRL.SUPPLY_BY,SRL.SUPPLY_TIME,SRL.LOCATION STATION_ID,SRL.VENDOR_CODE,SRL.VENDOR_NAME
													,IRL.LOT_CODE from SMT_PRODUCTION PR
													left join SMT_REEL SRL on PR.BATCH_NO=SRL.BATCH_NO
													left join IMS_REEL IRL on IRL.CODE=SRL.REEL_ID
													left join IMS_PART PA on SRL.PART_NO=PA.CODE
													{0}
												union all    
													select HRL.REEL_ID,PA.CODE,PA.DESCRIPTION,HRL.ORG_QTY as USED_QTY,HRL.OPERTOR as SUPPLY_BY,HRL.CREATE_TIME as ,SOS.OPERATION_SITE_NAME as STATION_ID,
													VEN.CODE as VENDOR_CODE,VEN.NAME as VENDOR_NAME,IRL.LOT_CODE from SFCS_PRODUCTION PR    
													left join MES_HI_REEL HRL on PR.BATCH_NO=HRL.BATCH_NO
													left join SFCS_OPERATION_SITES SOS ON HRL.OPERATION_SITE_ID = SOS.ID
													left join IMS_REEL IRL on IRL.CODE=HRL.REEL_ID
													left join IMS_PART PA on HRL.PART_NO=PA.CODE
													left join IMS_VENDOR VEN on IRL.VENDOR_ID=VEN.ID
													{0}
												) temp) WHERE rowno BETWEEN ((:Page-1)*:Limit+1) AND (:Limit*:Page) ", conditions);

			var resdata = await _dbConnection.QueryAsync<object>(sql,model);

			string sqlcnt =string.Format(@"select count(0) from (   
													select SRL.REEL_ID,PA.CODE,PA.DESCRIPTION,SRL.USED_QTY,SRL.SUPPLY_BY,SRL.SUPPLY_TIME,SRL.STATION_ID,SRL.VENDOR_CODE,SRL.VENDOR_NAME
													,IRL.LOT_CODE from SMT_PRODUCTION PR
													left join SMT_REEL SRL on PR.BATCH_NO=SRL.BATCH_NO
													left join IMS_REEL IRL on IRL.CODE=SRL.REEL_ID
													left join IMS_PART PA on SRL.PART_NO=PA.CODE
													{0}
												union all    
													select HRL.REEL_ID,PA.CODE,PA.DESCRIPTION,HRL.ORG_QTY as USED_QTY,HRL.OPERTOR as SUPPLY_BY,HRL.CREATE_TIME as ,HRL.OPERATION_SITE_ID as STATION_ID,
													VEN.CODE as VENDOR_CODE,VEN.NAME as VENDOR_NAME,IRL.LOT_CODE from SFCS_PRODUCTION PR    
													left join MES_HI_REEL HRL on PR.BATCH_NO=HRL.BATCH_NO
													left join IMS_REEL IRL on IRL.CODE=HRL.REEL_ID
													left join IMS_PART PA on HRL.PART_NO=PA.CODE
													left join IMS_VENDOR VEN on IRL.VENDOR_ID=VEN.ID {0} )", conditions);

			int cnt = await _dbConnection.ExecuteScalarAsync<int>(sqlcnt, model);
			return new TableDataModel
			{
				count = cnt,
				data = resdata?.ToList(),
			};

		}

		/// <summary>
		/// 根据主表ID获取打印时自动带出的数据
		/// </summary>
		/// <param name="ID"></param>
		/// <returns></returns>
		public async Task<TableDataModel> GetMesBatchPring(decimal ID) 
		{
			string sql =string.Format( @"select  distinct PR.MODEL,MBM.* from MES_BATCH_MANAGER MBM 
								left join SMT_PRODUCTION PR on MBM.LINE_ID=PR.LINE_ID where MBM.ID={0} ",ID);
			var resdata = await _dbConnection.QueryAsync<object>(sql);
			 
			return new TableDataModel
			{
				count =Convert.ToInt32( resdata?.ToList().Count()),
				data = resdata?.ToList(),
			};
		}

		/// <summary>
		/// 获取标签打印上传文件表中周转箱条码最新的一条数据
		/// </summary>
		/// <param name="ID"></param>
		/// <returns></returns>
		public async Task<TableDataModel> GetPrintFilesToPage()
		{
			string sql = @"select * from (
							select ID,FILE_NAME,FILE_TYPE,ORIGINAL_FILE_NAME from SFCS_PRINT_FILES 
							where ENABLED='Y' and LABEL_TYPE=4  order by ID desc ) where  ROWNUM <2 ";
			var resdata = await _dbConnection.QueryAsync<object>(sql);

			return new TableDataModel
			{
				count = Convert.ToInt32(resdata?.ToList().Count()),
				data = resdata?.ToList(),
			};
		}

	}
}