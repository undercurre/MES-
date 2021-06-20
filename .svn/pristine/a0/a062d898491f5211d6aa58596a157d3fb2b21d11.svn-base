/*
*┌──────────────────────────────────────────────────────────────┐
*│　描    述：接口实现                                                    
*│　作    者：嘉志科技                                            
*│　版    本：2.0    模板代码自动生成                                                
*│　创建时间：2020-04-20 11:41:41                             
*└──────────────────────────────────────────────────────────────┘
*┌──────────────────────────────────────────────────────────────┐
*│　命名空间： JZ.IMS.Repository.Oracle                                  
*│　类    名： SfcsProductOperationMonitorRepository                                      
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
    public class SfcsProductOperationMonitorRepository:BaseRepository<SfcsProductOperationMonitor,String>, ISfcsProductOperationMonitorRepository
    {
        public SfcsProductOperationMonitorRepository(IOptionsSnapshot<DbOption> options)
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
			string sql = "SELECT ENABLED FROM SFCS_PRODUCT_OPERATION_MONITOR WHERE ID=:ID";
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
			string sql = "UPDATE SFCS_PRODUCT_OPERATION_MONITOR set ENABLED=:ENABLED WHERE ID=:Id";
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
			string sql = "SELECT SFCS_PRODUCT_OPERATION_MONITOR_SEQ.NEXTVAL MY_SEQ FROM DUAL";
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
			string sql = "select count(0) from SFCS_PRODUCT_OPERATION_MONITOR where id = :id";
			object result = await _dbConnection.ExecuteScalarAsync(sql, new
			{
				id
			});

			return (Convert.ToInt32(result) > 0);
		}

		/// <summary>
		/// 产品流程管控配置
		/// </summary>
		/// <param name="model"></param>
		/// <returns></returns>
		public async Task<TableDataModel> GetMonitorList(SfcsProductOperationMonitorRequestModel model)
		{
			string condition = " ";
			if (model.BEGIN_OPERATION_CODE > 0)
			{
				condition += $" and SPOM.BEGIN_OPERATION_CODE=:BEGIN_OPERATION_CODE ";
			}
			if (model.END_OPERATION_CODE > 0)
			{
				condition += $" and SPOM.END_OPERATION_CODE=:END_OPERATION_CODE ";
			}
			if (model.STOP_CRITERIA > 0)
			{
				condition += $" and SPOM.STOP_CRITERIA=:STOP_CRITERIA ";
			}
			if (model.ALARM_CRITERIA > 0)
			{
				condition += $" and SPOM.ALARM_CRITERIA=:ALARM_CRITERIA ";
			}
			if (model.CRITERIA_UNIT>0)
			{
				condition += $" and SPOM.CRITERIA_UNIT=:CRITERIA_UNIT ";
			}
			if (model.COMPARE_MODE > 0)
			{
				condition += $" and SPOM.COMPARE_MODE=:COMPARE_MODE ";
			}
			if (!model.STOP_AND_HOLD.IsNullOrWhiteSpace() )
			{
				condition += $" and SPOM.STOP_AND_HOLD=:STOP_AND_HOLD ";
			}
			if (!model.ENABLED.IsNullOrWhiteSpace())
			{
				condition += $" and SPOM.ENABLED=:ENABLED ";
			}
			string sql = @" SELECT ROWNUM AS ROWNO,SPOM.* FROM SFCS_PRODUCT_OPERATION_MONITOR SPOM
							WHERE SPOM.PART_NO = :PART_NO AND MONITOR_MODE = :MONITOR_MODE
							AND END_OPERATION_CODE IN (SELECT PRODUCT_OPERATION_CODE
							FROM SFCS_ROUTE_CONFIG WHERE ROUTE_ID = :ROUTE_ID)  ";
			string pagedSql = SQLBuilderClass.GetPagedSQL(sql, " SPOM.ID DESC ", condition);
			var resdata = await _dbConnection.QueryAsync<object>(pagedSql, model);

			string sqlcnt = @"select count(0) FROM SFCS_PRODUCT_OPERATION_MONITOR SPOM
							WHERE SPOM.PART_NO = :PART_NO AND MONITOR_MODE = :MONITOR_MODE
							AND END_OPERATION_CODE IN (SELECT PRODUCT_OPERATION_CODE
							FROM SFCS_ROUTE_CONFIG WHERE ROUTE_ID = :ROUTE_ID)   " + condition;

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
		public async Task<decimal> SaveDataByTrans(SfcsProductOperationMonitorModel model)
		{
			int result = 1;
			ConnectionFactory.OpenConnection(_dbConnection);
			using (var tran = _dbConnection.BeginTransaction())
			{
				try
				{
					//新增
					string insertSql = @"insert into SFCS_PRODUCT_OPERATION_MONITOR 
					(ID,PART_NO,MONITOR_MODE,BEGIN_OPERATION_CODE,END_OPERATION_CODE,ALARM_CRITERIA,STOP_CRITERIA,CRITERIA_UNIT,COMPARE_MODE,STOP_AND_HOLD,ENABLED) 
					VALUES (:ID,:PART_NO,:MONITOR_MODE,:BEGIN_OPERATION_CODE,:END_OPERATION_CODE,:ALARM_CRITERIA,:STOP_CRITERIA,:CRITERIA_UNIT,:COMPARE_MODE,:STOP_AND_HOLD,:ENABLED)";
					if (model.InsertRecords != null && model.InsertRecords.Count > 0)
					{
						foreach (var item in model.InsertRecords)
						{
							var newid = await Get_MES_SEQ_ID();
							var resdata = await _dbConnection.ExecuteAsync(insertSql, new
							{
								ID = newid,
								item.PART_NO,
								item.MONITOR_MODE,
								item.BEGIN_OPERATION_CODE,
								item.END_OPERATION_CODE,
								item.ALARM_CRITERIA,
								item.STOP_CRITERIA,
								item.CRITERIA_UNIT,
								item.COMPARE_MODE,
								item.STOP_AND_HOLD,
								item.ENABLED,
							}, tran);
						}
					}
					//更新
					string updateSql = @"Update SFCS_PRODUCT_OPERATION_MONITOR set PART_NO=:PART_NO,MONITOR_MODE=:MONITOR_MODE,BEGIN_OPERATION_CODE=:BEGIN_OPERATION_CODE,END_OPERATION_CODE=:END_OPERATION_CODE,ALARM_CRITERIA=:ALARM_CRITERIA,STOP_CRITERIA=:STOP_CRITERIA,CRITERIA_UNIT=:CRITERIA_UNIT,COMPARE_MODE=:COMPARE_MODE,STOP_AND_HOLD=:STOP_AND_HOLD,ENABLED=:ENABLED  
						where ID=:ID ";
					if (model.UpdateRecords != null && model.UpdateRecords.Count > 0)
					{
						foreach (var item in model.UpdateRecords)
						{
							var resdata = await _dbConnection.ExecuteAsync(updateSql, new
							{
								item.ID,
								item.PART_NO,
								item.MONITOR_MODE,
								item.BEGIN_OPERATION_CODE,
								item.END_OPERATION_CODE,
								item.ALARM_CRITERIA,
								item.STOP_CRITERIA,
								item.CRITERIA_UNIT,
								item.COMPARE_MODE,
								item.STOP_AND_HOLD,
								item.ENABLED,
							}, tran);
						}
					}
					//删除
					string deleteSql = @"Delete from SFCS_PRODUCT_OPERATION_MONITOR where ID=:ID ";
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