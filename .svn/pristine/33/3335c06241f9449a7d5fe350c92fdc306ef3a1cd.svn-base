/**
*┌──────────────────────────────────────────────────────────────┐
*│　描    述：接口实现                                                    
*│　作    者：嘉志科技                                            
*│　版    本：2.0    模板代码自动生成                                                
*│　创建时间：2020-03-30 10:44:47                             
*└──────────────────────────────────────────────────────────────┘
*┌──────────────────────────────────────────────────────────────┐
*│　命名空间： JZ.IMS.Repository.Oracle                                  
*│　类    名： SfcsProductFamilyRepository                                      
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
    public class SfcsProductFamilyRepository:BaseRepository<SfcsProductFamily,String>, ISfcsProductFamilyRepository
    {
        public SfcsProductFamilyRepository(IOptionsSnapshot<DbOption> options)
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
			string sql = "SELECT ENABLED FROM SFCS_PRODUCT_FAMILY WHERE ID=:ID";
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
			string sql = "UPDATE SFCS_PRODUCT_FAMILY set ENABLED=:ENABLED WHERE ID=:Id";
			
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
			string sql = "SELECT SFCS_PRODUCT_FAMILY_SEQ.NEXTVAL MY_SEQ FROM DUAL";
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
			string sql = @"select count(0) from SFCS_PRODUCT_FAMILY where id = :id";
			object result = await _dbConnection.ExecuteScalarAsync(sql, new
			{
				id
			});

			return (Convert.ToInt32(result) > 0);
		}

		/// <summary>
		/// 获取客户列表
		/// </summary>
		/// <returns></returns>
		public async Task<List<object>> GetCumstomerList()
		{
			string sql = @" SELECT DISTINCT SC.ID,SC.CUSTOMER,SC.NATIONALITY FROM SFCS_CUSTOMERS SC WHERE PARENT_ID IS NULL	AND ENABLED = 'Y' ";
			var result = await _dbConnection.QueryAsync(sql);
			return result?.ToList();
		}

		/// <summary>
		///	通过客户名字 搜索和分页 
		/// </summary>
		/// <param name="key"></param>
		/// <returns></returns>
		public async Task<TableDataModel> GetCumstomerListByPage(SfcsProductFamilyRequestModel model)
		{
			string sql = @" select ROWNUM AS ROWNO, c.* from  (SELECT DISTINCT SC.ID,SC.CUSTOMER,SC.NATIONALITY FROM SFCS_CUSTOMERS SC WHERE PARENT_ID IS NULL	AND ENABLED = 'Y'  ) C  ";
			int count = 0;
			string conditions = " WHERE C.ID > 0 ";
			if (!model.Key.IsNullOrWhiteSpace())
			{
				conditions += $" and instr(c.customer, :key) > 0 ";
			}

			string pagedSql = SQLBuilderClass.GetPagedSQL(sql, " C.CUSTOMER ", conditions);
			var resdata = await _dbConnection.QueryAsync<object>(pagedSql, model);
			string sqlcnt = @" SELECT COUNT(C.ID) FROM (SELECT DISTINCT SC.ID,SC.CUSTOMER,SC.NATIONALITY FROM SFCS_CUSTOMERS SC WHERE PARENT_ID IS NULL	AND ENABLED = 'Y'  ) C   " + conditions;

			int cnt = await _dbConnection.ExecuteScalarAsync<int>(sqlcnt, model);
			return new TableDataModel
			{
				count = cnt,
				data = resdata?.ToList(),
			};

		}

		/// <summary>
		/// 分页
		/// </summary>
		/// <param name="model"></param>
		/// <returns></returns>
		public async Task<TableDataModel> GetProductFamilyList(SfcsProductFamilyRequestModel model)
		{
			string sql = @" SELECT ROW_NUMBER() OVER(ORDER BY PF.ID DESC) AS ROWNO,PF.ID, PF.CUSTOMER_ID,CS.CUSTOMER,PF.FAMILY_NAME,PF.DESCRIPTION,PF.ENABLED,PF.PHASE_IN_DATE,PF.PHASE_OUT_DATE  FROM SFCS_PRODUCT_FAMILY PF LEFT JOIN SFCS_CUSTOMERS CS ON PF.CUSTOMER_ID=CS.ID ";
			int count = 0;

			string conditions = " WHERE pf.ID > 0 ";
			if (model.CUSTOMER_ID > 0)
			{
				conditions += $"and PF.CUSTOMER_ID=:CUSTOMER_ID ";
			}
			if (!model.FAMILY_NAME.IsNullOrWhiteSpace()&&model.IS_LIKE==GlobalVariables.IsLike)
			{
				conditions += $"and instr(PF.FAMILY_NAME, :FAMILY_NAME) > 0 ";
			}

			if (!model.FAMILY_NAME.IsNullOrWhiteSpace() && model.IS_LIKE == GlobalVariables.NotLike)
			{
				conditions += $"and PF.FAMILY_NAME=:FAMILY_NAME ";
			}


			if (!model.DESCRIPTION.IsNullOrWhiteSpace())
			{
				conditions += $"and instr(PF.DESCRIPTION, :DESCRIPTION) > 0 ";
			}
			if (!model.ENABLED.IsNullOrWhiteSpace())
			{
				conditions += $"and PF.ENABLED=:ENABLED ";
			}
			if (!model.PHASE_IN_DATE.IsNullOrWhiteSpace())
			{
				conditions += $"and PF.PHASE_IN_DATE>= to_date(:PHASE_IN_DATE,'yyyy-mm-dd') ";
			}
			if (!model.PHASE_OUT_DATE.IsNullOrWhiteSpace())
			{
				conditions += $"and PF.PHASE_OUT_DATE<= to_date(:PHASE_OUT_DATE,'yyyy-mm-dd') ";
			}

			string pagedSql = SQLBuilderClass.GetPagedSQL(sql, "PF.ID DESC", conditions);
			var resdata = await _dbConnection.QueryAsync<object>(pagedSql, model);
			string sqlcnt = @" SELECT COUNT(PF.ID) FROM SFCS_PRODUCT_FAMILY PF LEFT JOIN SFCS_CUSTOMERS CS ON PF.CUSTOMER_ID=CS.ID  " + conditions;

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
		public async Task<TableDataModel> GetExportData(SfcsProductFamilyRequestModel model)
		{
			string conditions = " WHERE pf.ID > 0 ";
			if (model.CUSTOMER_ID > 0)
			{
				conditions += $"and PF.CUSTOMER_ID=:CUSTOMER_ID ";
			}
			if (!model.FAMILY_NAME.IsNullOrWhiteSpace())
			{
				conditions += $"and instr(PF.FAMILY_NAME, :FAMILY_NAME) > 0 ";
			}
			if (!model.DESCRIPTION.IsNullOrWhiteSpace())
			{
				conditions += $"and instr(PF.DESCRIPTION, :DESCRIPTION) > 0 ";
			}
			if (!model.ENABLED.IsNullOrWhiteSpace())
			{
				conditions += $"and PF.ENABLED=:ENABLED ";
			}
			if (!model.PHASE_IN_DATE.IsNullOrWhiteSpace())
			{
				conditions += $"and PF.PHASE_IN_DATE>= to_date(:PHASE_IN_DATE,'yyyy-mm-dd') ";
			}
			if (!model.PHASE_OUT_DATE.IsNullOrWhiteSpace())
			{
				conditions += $"and PF.PHASE_OUT_DATE<= to_date(:PHASE_OUT_DATE,'yyyy-mm-dd') ";
			}
			string sql = @"SELECT ROWNUM AS ROWNO,PF.ID, CS.CUSTOMER as CUSTOMER_ID,PF.FAMILY_NAME,PF.DESCRIPTION,PF.ENABLED,PF.PHASE_IN_DATE,PF.PHASE_OUT_DATE  
						   FROM SFCS_PRODUCT_FAMILY PF LEFT JOIN SFCS_CUSTOMERS CS ON PF.CUSTOMER_ID=CS.ID ";
			string pagedSql = SQLBuilderClass.GetPagedSQL(sql, "PF.ID DESC", conditions);
			var resdata = await _dbConnection.QueryAsync<object>(pagedSql, model);
			string sqlcnt = @" SELECT COUNT(PF.ID) FROM SFCS_PRODUCT_FAMILY PF LEFT JOIN SFCS_CUSTOMERS CS ON PF.CUSTOMER_ID=CS.ID  " + conditions;

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
		public async Task<decimal> SaveDataByTrans(SfcsProductFamilyModel model)
		{
			int result = 1;
			ConnectionFactory.OpenConnection(_dbConnection);
			using (var tran = _dbConnection.BeginTransaction())
			{
				try
				{
					//新增
					string insertSql = @"insert into SFCS_PRODUCT_FAMILY 
					(ID,CUSTOMER_ID,FAMILY_NAME,DESCRIPTION,ENABLED,PHASE_IN_DATE,PHASE_OUT_DATE) 
					VALUES (:ID,:CUSTOMER_ID,:FAMILY_NAME,:DESCRIPTION,:ENABLED,:PHASE_IN_DATE,:PHASE_OUT_DATE)";
					if (model.insertRecords != null && model.insertRecords.Count > 0)
					{
						foreach (var item in model.insertRecords)
						{
							var newid = await Get_MES_SEQ_ID();
							var resdata = await _dbConnection.ExecuteAsync(insertSql, new
							{
								ID = newid,
								item.CUSTOMER_ID,
								item.FAMILY_NAME,
								item.DESCRIPTION,
								item.ENABLED,
								item.PHASE_IN_DATE,
								item.PHASE_OUT_DATE
							}, tran);
						}
					}
					//更新
					string updateSql = @"Update SFCS_PRODUCT_FAMILY set CUSTOMER_ID=:CUSTOMER_ID,FAMILY_NAME=:FAMILY_NAME,DESCRIPTION=:DESCRIPTION,ENABLED=:ENABLED,PHASE_IN_DATE=:PHASE_IN_DATE,PHASE_OUT_DATE=:PHASE_OUT_DATE
						where ID=:ID ";
					if (model.updateRecords != null && model.updateRecords.Count > 0)
					{
						foreach (var item in model.updateRecords)
						{
							var resdata = await _dbConnection.ExecuteAsync(updateSql, new
							{
								item.ID,
								item.CUSTOMER_ID,
								item.FAMILY_NAME,
								item.DESCRIPTION,
								item.ENABLED,
								item.PHASE_IN_DATE,
								item.PHASE_OUT_DATE,							}, tran);
						}
					}
					//删除
					string deleteSql = @"Delete from SFCS_PRODUCT_FAMILY where ID=:ID ";
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
    }
}