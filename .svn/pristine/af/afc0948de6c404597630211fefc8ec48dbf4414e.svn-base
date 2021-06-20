/*
*┌──────────────────────────────────────────────────────────────┐
*│　描    述：接口实现                                                    
*│　作    者：嘉志科技                                            
*│　版    本：2.0    模板代码自动生成                                                
*│　创建时间：2020-04-16 13:37:10                             
*└──────────────────────────────────────────────────────────────┘
*┌──────────────────────────────────────────────────────────────┐
*│　命名空间： JZ.IMS.Repository.Oracle                                  
*│　类    名： SfcsProductSampleRepository                                      
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
    public class SfcsProductSampleRepository:BaseRepository<SfcsProductSample,String>, ISfcsProductSampleRepository
    {
        public SfcsProductSampleRepository(IOptionsSnapshot<DbOption> options)
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
			string sql = "SELECT ENABLED FROM SFCS_PRODUCT_SAMPLE WHERE ID=:ID";
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
			string sql = "UPDATE SFCS_PRODUCT_SAMPLE set ENABLED=:ENABLED WHERE ID=:Id";
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
			string sql = "SELECT SFCS_PRODUCT_SAMPLE_SEQ.NEXTVAL MY_SEQ FROM DUAL";
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
			string sql = "select count(0) from SFCS_PRODUCT_SAMPLE where id = :id";
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
		public async Task<TableDataModel> GetOperationList(SfcsProductSampleRequestModel model)
		{
			string condition = " ";
		
			string sql = @" SELECT ROWNUM AS ROWNO,SRC.Route_Id,SRC.Product_Operation_Code,SO.OPERATION_NAME,SO.DESCRIPTION
                                                        FROM SFCS_ROUTE_CONFIG SRC, SFCS_OPERATIONS SO
                                                        WHERE  SRC.CURRENT_OPERATION_ID=SO.ID
                                                        AND SO.ENABLED='Y' 
                                                        AND SRC.ROUTE_ID=:ROUTE_ID  ";
			string pagedSql = SQLBuilderClass.GetPagedSQL(sql, " SRC.ORDER_NO ASC ", condition);
			var resdata = await _dbConnection.QueryAsync<object>(pagedSql, model);

			string sqlcnt = @"select count(0) FROM SFCS_ROUTE_CONFIG SRC, SFCS_OPERATIONS SO
                                                        WHERE  SRC.CURRENT_OPERATION_ID=SO.ID
                                                        AND SO.ENABLED='Y' 
                                                        AND SRC.ROUTE_ID=:ROUTE_ID ORDER BY SRC.ORDER_NO  ";

			int cnt = await _dbConnection.ExecuteScalarAsync<int>(sqlcnt, model);

			return new TableDataModel
			{
				count = cnt,
				data = resdata?.ToList(),
			};
		}

		/// <summary>
		/// 产品抽检方案分页
		/// </summary>
		/// <param name="model"></param>
		/// <returns></returns>
		public async Task<TableDataModel> GetProductSampleList(SfcsProductSampleRequestModel model)
		{
			string condition = " ";
			if (!model.PART_NO.IsNullOrWhiteSpace())
			{
				condition += $" and SPS.PART_NO=:PART_NO ";
			}
			if (model.ROUTE_ID > 0)
			{
				condition += $" and ROUTE_ID=:ROUTE_ID ";
			}
			if (!model.ROUTE_NAME.IsNullOrWhiteSpace())
			{
				condition += $"  and INSTR(SR.ROUTE_NAME,:ROUTE_NAME) > 0 ";
			}
			if (model.SAMPLE_MODE > 0)
			{
				condition += $" and SPS.SAMPLE_MODE=:SAMPLE_MODE ";
			}
			if (model.PROJECT_ID > 0)
			{
				condition += $" and SPS.PROJECT_ID=:PROJECT_ID ";
			}
			if (model.DELIVER_OPERATION_CODE > 0)
			{
				condition += $" and SPS.DELIVER_OPERATION_CODE=:DELIVER_OPERATION_CODE ";
			}
			if (model.SAMPLE_OPERATION_CODE > 0)
			{
				condition += $" and SPS.SAMPLE_OPERATION_CODE=:SAMPLE_OPERATION_CODE ";
			}
			if (!model.ENABLED.IsNullOrWhiteSpace())
			{
				condition += $" and INSTR(SPS.ENABLED,:ENABLED) > 0 ";
			}
			string sql = @" SELECT ROWNUM AS ROWNO,SPS.*, SR.ROUTE_NAME, SR.ID ROUTE_ID 
                              FROM SFCS_PRODUCT_SAMPLE SPS, SFCS_ROUTE_CONFIG SRC, SFCS_ROUTES SR
                              WHERE SPS.DELIVER_OPERATION_CODE = SRC.PRODUCT_OPERATION_CODE
                              AND SRC.ROUTE_ID = SR.ID   ";
			string pagedSql = SQLBuilderClass.GetPagedSQL(sql, " SR.ID ASC ", condition);
			var resdata = await _dbConnection.QueryAsync<object>(pagedSql, model);

			string sqlcnt = @"select count(0) FROM SFCS_PRODUCT_SAMPLE SPS, SFCS_ROUTE_CONFIG SRC, SFCS_ROUTES SR
                              WHERE SPS.DELIVER_OPERATION_CODE = SRC.PRODUCT_OPERATION_CODE
                              AND SRC.ROUTE_ID = SR.ID   "+ condition;

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
		public async Task<decimal> SaveDataByTrans(SfcsProductSampleModel model)
		{
			int result = 1;
			ConnectionFactory.OpenConnection(_dbConnection);
			using (var tran = _dbConnection.BeginTransaction())
			{
				try
				{
					//新增
					string insertSql = @"insert into SFCS_PRODUCT_SAMPLE 
					(ID,PART_NO,SAMPLE_MODE,PROJECT_ID,DELIVER_OPERATION_CODE,SAMPLE_OPERATION_CODE,CURRENT_SAMPLE_RATIO,SAMPLE_OPERATION_COUNT,DELIVER_COUNT,SAMPLE_PASS_COUNT,SAMPLE_FAIL_COUNT,ENABLED,MUST_SIGN_WITH_FAIL) 
					VALUES (:ID,:PART_NO,:SAMPLE_MODE,:PROJECT_ID,:DELIVER_OPERATION_CODE,:SAMPLE_OPERATION_CODE,:CURRENT_SAMPLE_RATIO,:SAMPLE_OPERATION_COUNT,:DELIVER_COUNT,:SAMPLE_PASS_COUNT,:SAMPLE_FAIL_COUNT,:ENABLED,:MUST_SIGN_WITH_FAIL)";
					if (model.InsertRecords != null && model.InsertRecords.Count > 0)
					{
						foreach (var item in model.InsertRecords)
						{
							var newid = await Get_MES_SEQ_ID();
							var resdata = await _dbConnection.ExecuteAsync(insertSql, new
							{
								ID = newid,
								item.PART_NO,
								item.SAMPLE_MODE,
								item.PROJECT_ID,
								item.DELIVER_OPERATION_CODE,
								item.SAMPLE_OPERATION_CODE,
								item.CURRENT_SAMPLE_RATIO,
								item.SAMPLE_OPERATION_COUNT,
								item.DELIVER_COUNT,
								item.SAMPLE_PASS_COUNT,
								item.SAMPLE_FAIL_COUNT,
								item.ENABLED,
								item.MUST_SIGN_WITH_FAIL,
							}, tran);
						}
					}
					//更新
					string updateSql = @"Update SFCS_PRODUCT_SAMPLE set PART_NO=:PART_NO,SAMPLE_MODE=:SAMPLE_MODE,PROJECT_ID=:PROJECT_ID,DELIVER_OPERATION_CODE=:DELIVER_OPERATION_CODE,SAMPLE_OPERATION_CODE=:SAMPLE_OPERATION_CODE,CURRENT_SAMPLE_RATIO=:CURRENT_SAMPLE_RATIO,SAMPLE_OPERATION_COUNT=:SAMPLE_OPERATION_COUNT,DELIVER_COUNT=:DELIVER_COUNT,SAMPLE_PASS_COUNT=:SAMPLE_PASS_COUNT,SAMPLE_FAIL_COUNT=:SAMPLE_FAIL_COUNT,ENABLED=:ENABLED,MUST_SIGN_WITH_FAIL=:MUST_SIGN_WITH_FAIL  
						where ID=:ID ";
					if (model.UpdateRecords != null && model.UpdateRecords.Count > 0)
					{
						foreach (var item in model.UpdateRecords)
						{
							var resdata = await _dbConnection.ExecuteAsync(updateSql, new
							{
								item.ID,
								item.PART_NO,
								item.SAMPLE_MODE,
								item.PROJECT_ID,
								item.DELIVER_OPERATION_CODE,
								item.SAMPLE_OPERATION_CODE,
								item.CURRENT_SAMPLE_RATIO,
								item.SAMPLE_OPERATION_COUNT,
								item.DELIVER_COUNT,
								item.SAMPLE_PASS_COUNT,
								item.SAMPLE_FAIL_COUNT,
								item.ENABLED,
								item.MUST_SIGN_WITH_FAIL,
							}, tran);
						}
					}
					//删除
					string deleteSql = @"Delete from SFCS_PRODUCT_SAMPLE where ID=:ID ";
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