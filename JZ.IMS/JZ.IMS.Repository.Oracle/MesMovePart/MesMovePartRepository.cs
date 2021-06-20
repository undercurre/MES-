/**
*┌──────────────────────────────────────────────────────────────┐
*│　描    述：产线挪料表接口实现                                                    
*│　作    者：嘉志科技                                            
*│　版    本：2.0    模板代码自动生成                                                
*│　创建时间：2020-01-09 17:26:19                             
*└──────────────────────────────────────────────────────────────┘
*┌──────────────────────────────────────────────────────────────┐
*│　命名空间： JZ.IMS.Repository.Oracle                                  
*│　类    名： MesMovePartRepository                                      
*└──────────────────────────────────────────────────────────────┘
*/
using JZ.IMS.Core.DbHelper;
using JZ.IMS.Core.Options;
using JZ.IMS.Core.Repository;
using JZ.IMS.IRepository;
using JZ.IMS.Models;
using Dapper;
using Microsoft.Extensions.Options;
using System;
using System.Threading.Tasks;
using JZ.IMS.ViewModels;
using System.Collections.Generic;

namespace JZ.IMS.Repository.Oracle
{
	public class MesMovePartRepository : BaseRepository<MesMovePart, Decimal>, IMesMovePartRepository
	{
		public MesMovePartRepository(IOptionsSnapshot<DbOption> options)
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
			string sql = "SELECT ENABLED FROM MES_MOVE_PART WHERE ID=:ID";
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
			string sql = "UPDATE MES_MOVE_PART set ENABLED=:ENABLED WHERE ID=:Id";
			return await _dbConnection.ExecuteAsync(sql, new
			{
				ENABLED = status ? 'Y' : 'N',
				Id = id,
			});
		}

		/// <summary>
		/// 审核
		/// </summary>
		/// <param name="item"></param>
		/// <returns></returns>
		public async Task<BaseResult> AuditData(decimal id, string user)
		{
			BaseResult result = new BaseResult();
			string sql = "UPDATE MES_MOVE_PART SET STATUS = 1,AUDIT_USER = :AUDIT_USER,AUDIT_DATE = SYSDATE WHERE ID = :ID";
			if (await _dbConnection.ExecuteAsync(sql, new { ID = id, AUDIT_USER = user }) > 0)
			{
				result.ResultCode = ResultCodeAddMsgKeys.CommonObjectSuccessCode;
				result.ResultMsg = "审核成功";
			}
			else
			{
				result.ResultCode = ResultCodeAddMsgKeys.CommonExceptionCode;
				result.ResultMsg = "审核失败，数据异常，请刷新后重试！";
			}

			return result;
		}

		// <summary>
		/// 获取表的序列
		/// </summary>
		/// <returns></returns>
		public async Task<decimal> GetSEQID()
		{
			string sql = "SELECT MES_MOVE_PART_SEQ.NEXTVAL MY_SEQ FROM DUAL";
			var result = await _dbConnection.ExecuteScalarAsync(sql);
			return (decimal)result;
		}

		/// <summary>
		/// 根据工单号获取成品品信息
		/// </summary>
		/// <param name="wo_no"></param>
		/// <returns></returns>
		public async Task<V_IMS_WO> GetPartByWoNo(string wo_no)
		{
			string sql = "SELECT WO_NO,PART_NO,MODEL AS PRODUCT_NAME,DESCRIPTION AS MODEL FROM SMT_WO WHERE WO_NO = :WO_NO";
			return await _dbConnection.QueryFirstOrDefaultAsync<V_IMS_WO>(sql, new { WO_NO = wo_no });
		}

		/// <summary>
		/// 判断零件料号是否存在
		/// </summary>
		/// <param name="part_no"></param>
		/// <returns></returns>
		public async Task<bool> IsExistPart(string part_no)
		{
			string sql = "SELECT COUNT(*) FROM IMS_PART WHERE CODE =:PART_NO";
			return (await _dbConnection.ExecuteScalarAsync<int>(sql, new { PART_NO = part_no })) > 0;
		}

		/// <summary>
		/// 判断零件料号是否存在于成品料号中
		/// </summary>
		/// <returns></returns>
		public async Task<bool> IsExistPart(string product_no, decimal operationId, string part_no)
		{
			string sql = @"SELECT COUNT (*)
						  FROM SOP_ROUTES sp
							   INNER JOIN SOP_OPERATIONS_ROUTES sor ON SP.ID = SOR.ROUTE_ID
							   INNER JOIN SOP_OPERATIONS_ROUTES_PART PART
								  ON SOR.ID = PART.OPERATIONS_ROUTES_ID
						 WHERE     SP.PART_NO = :PRODUCT_NO
							   AND SOR.CURRENT_OPERATION_ID = :OPERATION_ID
							   AND PART.PART_NO = :PART_NO";
			return (await _dbConnection.ExecuteScalarAsync<int>(sql, new { PRODUCT_NO = product_no, OPERATION_ID = operationId, PART_NO = part_no })) > 0;
		}

		/// <summary>
		/// 根据ID获取数据
		/// </summary>
		/// <param name="id"></param>
		/// <returns></returns>
		public async Task<MesMovePart> GetMovePartById(decimal id)
		{
			string sql = "SELECT * FROM MES_MOVE_PART WHERE ID=:ID";
			return await _dbConnection.QueryFirstOrDefaultAsync<MesMovePart>(sql, new { ID = id });
		}

		/// <summary>
		/// 获取审核/激活后上料结果
		/// </summary>
		/// <param name="wo_no"></param>
		/// <param name="product_no"></param>
		/// <returns></returns>
		public async Task<IEnumerable<MesMovePartResultModel>> GetAuditResultData(MesMovePartRequestModel model)
		{
			string where = "";
			if (model.OperationType == 1)
				where += " ID = :MOVE_ID AND ENABLED='Y'";
			else
				where += " ID = :MOVE_ID AND ENABLED='N' AND STATUS = 1";

			string sql = string.Format(@"SELECT TB1.*, SO.OPERATION_NAME, SO.DESCRIPTION
							FROM (  SELECT TB.PART_NO,
											SUM (TB.PART_QTY) AS PART_QTY,
											TB.CURRENT_OPERATION_ID
									FROM (SELECT SORP.PART_NO, SORP.PART_QTY, SOR.CURRENT_OPERATION_ID
											FROM SOP_OPERATIONS_ROUTES_PART SORP,
													SOP_ROUTES SR,
													SOP_OPERATIONS_ROUTES SOR
											WHERE     SORP.OPERATIONS_ROUTES_ID = SOR.ID
													AND SOR.ROUTE_ID = SR.ID
													AND SR.PART_NO = :PART_NO
													AND SORP.IS_SCAN = 'Y'
											UNION
											SELECT PART_NO,
													CASE MOVE_TYPE WHEN 1 THEN PART_QTY ELSE -PART_QTY END
													AS PART_QTY,
													OPERATION_ID AS CURRENT_OPERATION_ID
											FROM MES_MOVE_PART
											WHERE     WO_NO = :WO_NO
													AND STATUS = 1
													AND ENABLED = 'Y'
													AND ID != :MOVE_ID
											UNION
											SELECT PART_NO,
													CASE MOVE_TYPE WHEN 1 THEN PART_QTY ELSE -PART_QTY END
													AS PART_QTY,
													OPERATION_ID AS CURRENT_OPERATION_ID
											FROM MES_MOVE_PART
											WHERE {0}) TB
								GROUP BY TB.PART_NO, TB.CURRENT_OPERATION_ID) TB1
								LEFT JOIN SFCS_OPERATIONS SO ON TB1.CURRENT_OPERATION_ID = SO.ID
							WHERE TB1.PART_QTY > 0", where);

			return await _dbConnection.QueryAsync<MesMovePartResultModel>(sql, new { MOVE_ID = model.ID, WO_NO = model.WO_NO, PART_NO = model.PRODUCT_NO });
		}
	}
}