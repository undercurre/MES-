/*
*┌──────────────────────────────────────────────────────────────┐
*│　描    述：接口实现                                                    
*│　作    者：嘉志科技                                            
*│　版    本：2.0    模板代码自动生成                                                
*│　创建时间：2020-04-16 12:07:39                             
*└──────────────────────────────────────────────────────────────┘
*┌──────────────────────────────────────────────────────────────┐
*│　命名空间： JZ.IMS.Repository.Oracle                                  
*│　类    名： SfcsProductStoplineRepository                                      
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
    public class SfcsProductStoplineRepository:BaseRepository<SfcsProductStopline,Decimal>, ISfcsProductStoplineRepository
    {
        public SfcsProductStoplineRepository(IOptionsSnapshot<DbOption> options)
        {
            _dbOption =options.Get("iWMS");
            if (_dbOption == null)
            {
                throw new ArgumentNullException(nameof(DbOption));
            }
            _dbConnection = ConnectionFactory.CreateConnection(_dbOption.DbType, _dbOption.ConnectionString);
        }

        /// <summary>
		///项目是否已被使用 
		/// </summary>
		/// <param name="id">项目id</param>
		/// <returns></returns>
		public async Task<bool> ItemIsByUsed(decimal id)
		{
			string sql = "select count(0) from SFCS_PRODUCT_STOPLINE where id = :id";
			object result = await _dbConnection.ExecuteScalarAsync(sql, new
			{
				id
			});

			return (Convert.ToInt32(result) > 0);
		}

		/// <summary>
		/// 获取制程工序列表
		/// </summary>
		/// <param name="route_id">制程ID</param>
		/// <returns></returns>
		public async Task<List<SfcsOperations>> GetRouteConfigLists(decimal route_id)
		{
			string sql = @"SELECT SRC.PRODUCT_OPERATION_CODE as ID, OP.OPERATION_NAME, OP.DESCRIPTION 
                           FROM SFCS_ROUTE_CONFIG SRC 
                              INNER JOIN SFCS_OPERATIONS OP ON SRC.CURRENT_OPERATION_ID = OP.ID 
                           WHERE SRC.ROUTE_ID =:ROUTE_ID ORDER BY SRC.ORDER_NO ";
			var resdata = await _dbConnection.QueryAsync<SfcsOperations>(sql, new { ROUTE_ID = route_id });
			return resdata?.ToList();
		}

		/// <summary>
		/// 获取产品停线管控规则配置列表
		/// </summary>
		/// <param name="part_no">料号</param>
		/// <param name="route_id">制程ID</param>
		/// <returns></returns>
		public async Task<List<SfcsProductStopline>> GetSfcsProductStoplineList(string part_no, decimal route_id)
		{
			string sql = @"SELECT SPS.* FROM SFCS_PRODUCT_STOPLINE SPS,SFCS_ROUTE_CONFIG SRC 
						   WHERE SPS.STOP_OPERATION_CODE=SRC.PRODUCT_OPERATION_CODE 
                                 AND SPS.PART_NO=:PART_NO AND SRC.ROUTE_ID=:ROUTE_ID ";
			var resdata = await _dbConnection.QueryAsync<SfcsProductStopline>(sql,
			new
			{
				ROUTE_ID = route_id,
				PART_NO = part_no,
			});
			return resdata?.ToList();
		}

		/// <summary>
		/// 获取表的序列
		/// </summary>
		/// <returns></returns>
		public async Task<decimal> GetStopLineCallSEQID()
		{
			string sql = "SELECT MES_STOPLINE_CALL_SEQ.NEXTVAL MY_SEQ FROM DUAL";
			var result = await _dbConnection.ExecuteScalarAsync(sql);
			return (decimal)result;
		}



		/// <summary>
		/// 保存数据
		/// </summary>
		/// <param name="model"></param>
		/// <returns></returns>
		public async Task<decimal> SaveDataByTrans(SfcsProductStoplineModel model)
		{
			int result = 1;
			ConnectionFactory.OpenConnection(_dbConnection);
			using (var tran = _dbConnection.BeginTransaction())
			{
				try
				{
					//新增
					string insertSql = @"insert into SFCS_PRODUCT_STOPLINE 
					(ID,PART_NO,STOPLINE_MODE,STOP_OPERATION_CODE,ALARM_CRITERIA,STOP_CRITERIA,DIVISION_CRITERIA,DIVISION_START,DIVISION_UNIT,ALARM_INTERVAL,INPUT_CONTROL,INPUT_OPERATION_CODE,INPUT_CONTROL_CRITERIA,INCLUDE_NDF,ENABLED,STOP_OPERATION_ID) 
					VALUES (:ID,:PART_NO,:STOPLINE_MODE,:STOP_OPERATION_CODE,:ALARM_CRITERIA,:STOP_CRITERIA,:DIVISION_CRITERIA,:DIVISION_START,:DIVISION_UNIT,:ALARM_INTERVAL,:INPUT_CONTROL,:INPUT_OPERATION_CODE,:INPUT_CONTROL_CRITERIA,:INCLUDE_NDF,:ENABLED,:STOP_OPERATION_ID)";
					if (model.InsertRecords != null && model.InsertRecords.Count > 0)
					{
						foreach (var item in model.InsertRecords)
						{
							var newid = await Get_MES_SEQ_ID();
							var resdata = await _dbConnection.ExecuteAsync(insertSql, new
							{
								ID = newid,
								item.PART_NO,
								item.STOPLINE_MODE,
								item.STOP_OPERATION_CODE,
								item.ALARM_CRITERIA,
								item.STOP_CRITERIA,
								item.DIVISION_CRITERIA,
								item.DIVISION_START,
								item.DIVISION_UNIT,
								item.ALARM_INTERVAL,
								item.INPUT_CONTROL,
								item.INPUT_OPERATION_CODE,
								item.INPUT_CONTROL_CRITERIA,
								item.INCLUDE_NDF,
								item.ENABLED,								item.STOP_OPERATION_ID,
							}, tran);

							//新增停线管控线别表记录
							string insertcallSql = @"insert into MES_STOPLINE_CALL 
					(ID,PRODUCT_STOPLINE_ID,ENABLED,CALL_TONTENT_ID) 
					VALUES (:ID,:PRODUCT_STOPLINE_ID,:ENABLED,:CALL_TONTENT_ID)";
							if (model.InserStoplineCallRecords != null && model.InserStoplineCallRecords.Count > 0)
							{
								foreach (var callitem in model.InserStoplineCallRecords)
								{
									var callid = await GetStopLineCallSEQID();
									var calldata = await _dbConnection.ExecuteAsync(insertcallSql, new
									{
										ID = callid,
										PRODUCT_STOPLINE_ID= newid,
										callitem.ENABLED,
										callitem.CALL_TONTENT_ID,

									}, tran);
								}

							}
						}
					}
					//更新
					string updateSql = @"Update SFCS_PRODUCT_STOPLINE set PART_NO=:PART_NO,STOPLINE_MODE=:STOPLINE_MODE,STOP_OPERATION_CODE=:STOP_OPERATION_CODE,ALARM_CRITERIA=:ALARM_CRITERIA,STOP_CRITERIA=:STOP_CRITERIA,DIVISION_CRITERIA=:DIVISION_CRITERIA,DIVISION_START=:DIVISION_START,DIVISION_UNIT=:DIVISION_UNIT,ALARM_INTERVAL=:ALARM_INTERVAL,INPUT_CONTROL=:INPUT_CONTROL,
											INPUT_OPERATION_CODE=:INPUT_OPERATION_CODE,INPUT_CONTROL_CRITERIA=:INPUT_CONTROL_CRITERIA,INCLUDE_NDF=:INCLUDE_NDF,ENABLED=:ENABLED ,STOP_OPERATION_ID=:STOP_OPERATION_ID 
										where ID=:ID ";
					if (model.UpdateRecords != null && model.UpdateRecords.Count > 0)
					{
						foreach (var item in model.UpdateRecords)
						{
							var resdata = await _dbConnection.ExecuteAsync(updateSql, new
							{
								item.ID,
								item.PART_NO,
								item.STOPLINE_MODE,
								item.STOP_OPERATION_CODE,
								item.ALARM_CRITERIA,
								item.STOP_CRITERIA,
								item.DIVISION_CRITERIA,
								item.DIVISION_START,
								item.DIVISION_UNIT,
								item.ALARM_INTERVAL,
								item.INPUT_CONTROL,
								item.INPUT_OPERATION_CODE,
								item.INPUT_CONTROL_CRITERIA,
								item.INCLUDE_NDF,
								item.ENABLED,								item.STOP_OPERATION_ID														}, tran);
						}
					}

					

					//更新线管控线别表记录
					string updatecallSql = @"Update MES_STOPLINE_CALL set PRODUCT_STOPLINE_ID=:PRODUCT_STOPLINE_ID,ENABLED=:ENABLED,CALL_TONTENT_ID=:CALL_TONTENT_ID  
						where ID=:ID ";
					if (model.UpdateStoplineCallRecords != null && model.UpdateStoplineCallRecords.Count > 0)
					{
						foreach (var item in model.UpdateStoplineCallRecords)
						{
							var resdata = await _dbConnection.ExecuteAsync(updatecallSql, new
							{
								item.ID,
								item.PRODUCT_STOPLINE_ID,
								item.ENABLED,
								item.CALL_TONTENT_ID,
							}, tran);
						}
					}


					////删除
					//string deleteSql = @"Delete from SFCS_PRODUCT_STOPLINE where ID=:ID ";
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
		/// 根据主键获取激活状态
		/// </summary>
		/// <param name="id">主键</param>
		/// <returns></returns>
		public async Task<Boolean> GetEnableStatus(decimal id)
		{
			string sql = "SELECT ENABLED FROM SFCS_PRODUCT_STOPLINE WHERE ID=:ID";
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
			string sql = "UPDATE SFCS_PRODUCT_STOPLINE set ENABLED=:ENABLED WHERE ID=:Id";
			return await _dbConnection.ExecuteAsync(sql, new
			{
				ENABLED = status ? 'Y' : 'N',
				Id = id,
			});
		}


		/// <summary>
		/// 查询数据
		/// </summary>
		/// <param name="model"></param>
		/// <returns></returns>
		public async Task<TableDataModel> GetSfcsProductStoplinesList(SfcsProductStoplineRequestModel model)
		{
			string conditions = "where  a.ID > 0 ";
			 
			if (!model.LINE_ID.IsNullOrWhiteSpace())
			{
				conditions += $"and (b.LINE_ID =:LINE_ID)";
			}
			if (!model.STOPLINE_MODE.IsNullOrWhiteSpace())
			{
				conditions += $"and (a.STOPLINE_MODE =:STOPLINE_MODE)";
			}
			if (!model.STOP_OPERATION_ID.IsNullOrWhiteSpace())
			{
				conditions += $"and (a.STOP_OPERATION_ID =:STOP_OPERATION_ID)";
			}
			if (!model.Key.IsNullOrWhiteSpace())
			{
				conditions += $"and (instr(a.PART_NO, :Key) > 0  )";
			}
			string sql = string.Format(@"select * from (
											select ROWNUM as ROWNO, temp.* from (
													select distinct c.OPERATION_NAME,d.CHINESE as STOPLINE_NAME, a.* from SFCS_PRODUCT_STOPLINE a 
													left join MES_STOPLINE_LINES b on a.ID=b.PRODUCT_STOPLINE_ID 
													left join SFCS_OPERATIONS c on c.ID=a.STOP_OPERATION_ID
													left join SFCS_PARAMETERS d on d.LOOKUP_TYPE = 'STOPLINE_MODE' and d.ENABLED='Y' and a.STOPLINE_MODE= d.LOOKUP_CODE   {0} 
												  order by a.ID desc ) temp)
										WHERE ROWNO BETWEEN ((:Page-1)*:Limit+1) AND (:Limit*:Page) order by ROWNO asc", conditions);

			var resdata = await _dbConnection.QueryAsync<object>(sql, model);
			string sqlcnt = string.Format(@"select count(0) from (					
								select distinct a.ID from SFCS_PRODUCT_STOPLINE a 
								left join MES_STOPLINE_LINES b on a.ID=b.PRODUCT_STOPLINE_ID 
								left join SFCS_OPERATIONS c on c.ID=a.STOP_OPERATION_ID
								left join SFCS_PARAMETERS d on d.LOOKUP_TYPE = 'STOPLINE_MODE' and d.ENABLED='Y' and a.STOPLINE_MODE= d.LOOKUP_CODE {0} )",  conditions);

			int cnt = await _dbConnection.ExecuteScalarAsync<int>(sqlcnt, model);
			return new TableDataModel
			{
				count = cnt,
				data = resdata?.ToList(),
			};

			 
		}


		/// <summary>
		/// 获取线体下拉框
		/// </summary>
		/// <returns></returns>

		public async Task<List<dynamic>> GetLINENAME()
		{
			List<dynamic> result = null;
			try
			{
				string sql = @"select * from (
								select ID,LINE_NAME from SMT_LINES 
								union all
								select ID,OPERATION_LINE_NAME as LINE_NAME from SFCS_OPERATION_LINES where ENABLED='Y'
							  ) order by LINE_NAME desc";
				var objectlist = await _dbConnection.QueryAsync<dynamic>(sql);
				return objectlist?.ToList();
			}
			catch (Exception ex)
			{
				result = null;
			}
			return result;
		}

		/// <summary>
		/// 获取工序下拉框
		/// </summary>
		/// <returns></returns>

		public async Task<List<dynamic>> GetOperation()
		{
			List<dynamic> result = null;
			try
			{
				string sql = @"select ID,OPERATION_NAME from SFCS_OPERATIONS where ENABLED='Y' order by OPERATION_NAME  desc";
				var objectlist = await _dbConnection.QueryAsync<dynamic>(sql);
				return objectlist?.ToList();
			}
			catch (Exception ex)
			{
				result = null;
			}
			return result;
		}

		/// <summary>
		/// 根据主表ID获取停线管控主表和呼叫子表数据
		/// </summary>
		/// <param name="id"></param>
		/// <returns></returns>
		public async Task<TableDataModel> GetStopLineAndCallDataByID(decimal id)
		{
			string sql = string.Format(@"select b.ID as CallID,b.ENABLED as CallEnabled, c.CALL_CATEGORY_CODE,c.CALL_TYPE_CODE,c.CALL_TITLE,c.CALL_CODE,c.CHINESE,
											c.ID as CONTENTID, a.* from SFCS_PRODUCT_STOPLINE a
											left join MES_STOPLINE_CALL b on a.ID=b.PRODUCT_STOPLINE_ID
											left join ANDON_CALL_CONTENT_CONFIG c on b.CALL_TONTENT_ID=c.ID
											where a.ID={0}", id );
			var resdata = await _dbConnection.QueryAsync<object>(sql);

			return new TableDataModel
			{
				count = Convert.ToInt32(resdata?.ToList().Count()),
				data = resdata?.ToList(),
			};
		}


    }
}