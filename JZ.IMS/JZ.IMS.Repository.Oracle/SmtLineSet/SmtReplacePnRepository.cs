/**
*┌──────────────────────────────────────────────────────────────┐
*│　描    述：接口实现                                                    
*│　作    者：嘉志科技                                            
*│　版    本：2.0    模板代码自动生成                                                
*│　创建时间：2020-03-23 09:03:32                             
*└──────────────────────────────────────────────────────────────┘
*┌──────────────────────────────────────────────────────────────┐
*│　命名空间： JZ.IMS.Repository.Oracle                                  
*│　类    名： SmtReplacePnRepository                                      
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
using JZ.IMS.ViewModels.SmtLineSet;
using JZ.IMS.Core.Extensions;

namespace JZ.IMS.Repository.Oracle
{
    public class SmtReplacePnRepository:BaseRepository<SmtReplacePn,Decimal>, ISmtReplacePnRepository
    {
        public SmtReplacePnRepository(IOptionsSnapshot<DbOption> options)
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
			string sql = "SELECT ENABLED FROM SMT_REPLACE_PN WHERE ID=:ID";
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
			string sql = "UPDATE SMT_REPLACE_PN set ENABLED=:ENABLED WHERE ID=:Id";
			return await _dbConnection.ExecuteAsync(sql, new
			{
				ENABLED = status ? 'Y' : 'N',
				Id = id,
			});
		}

        // <summary>
        /// 获取表的序列
        /// </summary>
        /// <returns></returns>
		public async Task<decimal> GetSEQID()
		{
			string sql = "SELECT SMT_REPLACE_PN_SEQ.NEXTVAL MY_SEQ FROM DUAL";
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
			string sql = "select count(0) from SMT_REPLACE_PN where id = :id";
			object result = await _dbConnection.ExecuteScalarAsync(sql, new
			{
				id
			});

			return (Convert.ToInt32(result) > 0);
		}

		/// <summary>
		/// 分页
		/// </summary>
		/// <param name="model"></param>
		/// <returns></returns>
		public async Task<TableDataModel> GetSmtReplaceList(SmtReplacePnRequestModel model)
		{
			string sql = @" SELECT ROW_NUMBER() OVER(ORDER BY RPN.ID DESC) AS ROWNO, RPN.ID,RPN.WO_NO,RPN.PCB_PN,RPN.COMPONENT_PN,
							RPN.REPLACE_PN,RPN.VENDOR_CODE,IVR.DESCRIPTION,RPN.MAKER_PN,RPN.ENABLED,BEGINTIME,ENDTIME FROM SMT_REPLACE_PN RPN
							LEFT JOIN IMS_VENDOR IVR ON IVR.CODE=RPN.VENDOR_CODE  ";

			string conditions = " WHERE RPN.ID > 0 ";
			if (!model.WO_NO.IsNullOrWhiteSpace())
			{
				conditions += $" AND RPN.WO_NO=:WO_NO ";
			}
			if (!model.COMPONENT_PN.IsNullOrWhiteSpace())
			{
				conditions += $" AND RPN.COMPONENT_PN=:COMPONENT_PN ";
			}
			if (!model.REPLACE_PN.IsNullOrWhiteSpace())
			{
				conditions += $" AND RPN.REPLACE_PN=:REPLACE_PN ";
			}
			if (!model.VENDOR_CODE.IsNullOrWhiteSpace())
			{
				conditions += $" AND RPN.VENDOR_CODE=:VENDOR_CODE ";
			}
			if (!model.MAKER_PN.IsNullOrWhiteSpace())
			{
				conditions += $" AND RPN.MAKER_PN=:MAKER_PN ";
			}
			if (!model.ENABLED.IsNullOrWhiteSpace())
			{
				conditions += $" AND RPN.ENABLED=:ENABLED ";
			}
			string pagedSql = SQLBuilderClass.GetPagedSQL(sql, "RPN.ID DESC", conditions);
			var resdata = await _dbConnection.QueryAsync<object>(pagedSql, model);
			string sqlcnt = @" SELECT COUNT(RPN.ID) FROM SMT_REPLACE_PN RPN
							LEFT JOIN IMS_VENDOR IVR ON IVR.CODE=RPN.VENDOR_CODE   " + conditions;

			int cnt = await _dbConnection.ExecuteScalarAsync<int>(sqlcnt, model);
			return new TableDataModel
			{
				count = cnt,
				data = resdata?.ToList(),
			};
		}

		/// <summary>
		/// 获取导出数据
		/// </summary>
		/// <param name="model"></param>
		/// <returns></returns>
		public async Task<TableDataModel> GetExportData(SmtReplacePnRequestModel model)
		{
			string conditions = " WHERE RPN.ID > 0 ";
			if (!model.WO_NO.IsNullOrWhiteSpace())
			{
				conditions += $" AND RPN.WO_NO=:WO_NO ";
			}
			if (!model.COMPONENT_PN.IsNullOrWhiteSpace())
			{
				conditions += $" AND RPN.COMPONENT_PN=:COMPONENT_PN ";
			}
			if (!model.REPLACE_PN.IsNullOrWhiteSpace())
			{
				conditions += $" AND RPN.REPLACE_PN=:REPLACE_PN ";
			}
			if (!model.VENDOR_CODE.IsNullOrWhiteSpace())
			{
				conditions += $" AND RPN.VENDOR_CODE=:VENDOR_CODE ";
			}
			if (!model.MAKER_PN.IsNullOrWhiteSpace())
			{
				conditions += $" AND RPN.MAKER_PN=:MAKER_PN ";
			}
			if (!model.ENABLED.IsNullOrWhiteSpace())
			{
				conditions += $" AND RPN.ENABLED=:ENABLED ";
			}

			string sql = @" SELECT ROWNUM AS ROWNO, RPN.ID,RPN.WO_NO,RPN.PCB_PN,RPN.COMPONENT_PN,
								RPN.REPLACE_PN,IVR.DESCRIPTION AS VENDOR_CODE,RPN.MAKER_PN,RPN.ENABLED,BEGINTIME,ENDTIME 
							FROM SMT_REPLACE_PN RPN
							LEFT JOIN IMS_VENDOR IVR ON IVR.CODE=RPN.VENDOR_CODE  ";
			string pagedSql = SQLBuilderClass.GetPagedSQL(sql, "RPN.ID DESC", conditions);
			var resdata = await _dbConnection.QueryAsync<object>(pagedSql, model);
			string sqlcnt = @" SELECT COUNT(RPN.ID) FROM SMT_REPLACE_PN RPN
							LEFT JOIN IMS_VENDOR IVR ON IVR.CODE=RPN.VENDOR_CODE   " + conditions;

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
		public async Task<decimal> SaveDataByTrans(SmtReplacePnModel model)
		{
			int result = 1;
			ConnectionFactory.OpenConnection(_dbConnection);
			using (var tran = _dbConnection.BeginTransaction())
			{
				try
				{
					//新增
					string insertSql = @"insert into SMT_REPLACE_PN 
					(ID,WO_NO,PCB_PN,COMPONENT_PN,REPLACE_PN,VENDOR_CODE,MAKER_PN,ENABLED,BEGINTIME,ENDTIME,COMPONENT_PN_QTY,REPLACE_PN_QTY) 
					VALUES (:ID,:WO_NO,:PCB_PN,:COMPONENT_PN,:REPLACE_PN,:VENDOR_CODE,:MAKER_PN,:ENABLED,:BEGINTIME,:ENDTIME,:COMPONENT_PN_QTY,:REPLACE_PN_QTY)";
					if (model.insertRecords != null && model.insertRecords.Count > 0)
					{
						foreach (var item in model.insertRecords)
						{
							var newid = await GetID();
							var resdata = await _dbConnection.ExecuteAsync(insertSql, new
							{
								ID = newid,
								item.WO_NO,
								item.PCB_PN,
								item.COMPONENT_PN,
								item.REPLACE_PN,
								item.VENDOR_CODE,
								item.MAKER_PN,
								item.ENABLED,
								item.BEGINTIME,
								item.ENDTIME,
								item.COMPONENT_PN_QTY,
								item.REPLACE_PN_QTY,							}, tran);
						}
					}
					//更新
					string updateSql = @"Update SMT_REPLACE_PN set WO_NO=:WO_NO,PCB_PN=:PCB_PN,COMPONENT_PN=:COMPONENT_PN,REPLACE_PN=:REPLACE_PN,VENDOR_CODE=:VENDOR_CODE,MAKER_PN=:MAKER_PN,ENABLED=:ENABLED,BEGINTIME=:BEGINTIME,ENDTIME=:ENDTIME,COMPONENT_PN_QTY=:COMPONENT_PN_QTY,REPLACE_PN_QTY=:REPLACE_PN_QTY  
						where ID=:ID ";
					if (model.updateRecords != null && model.updateRecords.Count > 0)
					{
						foreach (var item in model.updateRecords)
						{
							var resdata = await _dbConnection.ExecuteAsync(updateSql, new
							{
								item.ID,
								item.WO_NO,
								item.PCB_PN,
								item.COMPONENT_PN,
								item.REPLACE_PN,
								item.VENDOR_CODE,
								item.MAKER_PN,
								item.ENABLED,
								item.BEGINTIME,
								item.ENDTIME,
								item.COMPONENT_PN_QTY,
								item.REPLACE_PN_QTY,							}, tran);
						}
					}
					//删除
					string deleteSql = @"Delete from SMT_REPLACE_PN where ID=:ID ";
					if (model.removeRecords != null && model.removeRecords.Count > 0)
					{
						foreach (var item in model.removeRecords)
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
		/// 获取供应商下拉表
		/// </summary>
		/// <returns></returns>
		public async Task<TableDataModel> GetVendorCode(IDNameRequestModel model)
		{
			string sql = " SELECT  ROWNUM AS rowno,CODE VENDOR_CODE, DESCRIPTION AS VENDOR_NAME FROM IMS_VENDOR ";
			string condition = "  WHERE ID > 0 ";
			if (!model.Key.IsNullOrWhiteSpace())
			{
				condition +=$" AND (INSTR(CODE, :KEY)) > 0 OR (INSTR(DESCRIPTION, :KEY)) > 0 ";
			}
			
			string pagedSql =SQLBuilderClass.GetPagedSQL(sql, " DESCRIPTION DESC", condition);
			var resdata = await _dbConnection.QueryAsync<object>(pagedSql, model);
			string sqlcnt = @" SELECT COUNT(ID) FROM IMS_VENDOR " + condition;

			int cnt = await _dbConnection.ExecuteScalarAsync<int>(sqlcnt, model);
			return new TableDataModel
			{
				count = cnt,
				data = resdata?.ToList(),
			};
		}
	}
}