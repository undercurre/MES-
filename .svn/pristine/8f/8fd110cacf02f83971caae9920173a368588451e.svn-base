/**
*┌──────────────────────────────────────────────────────────────┐
*│　描    述：首五件检验主表接口实现                                                    
*│　作    者：嘉志科技                                            
*│　版    本：2.0    模板代码自动生成                                                
*│　创建时间：2020-05-13 09:51:01                             
*└──────────────────────────────────────────────────────────────┘
*┌──────────────────────────────────────────────────────────────┐
*│　命名空间： JZ.IMS.Repository.Oracle                                  
*│　类    名： MesFirstCheckInfoRepository                                      
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
using System.Linq;

namespace JZ.IMS.Repository.Oracle
{
	public class MesFirstCheckInfoRepository : BaseRepository<MesFirstCheckInfo, Decimal>, IMesFirstCheckInfoRepository
	{
		public MesFirstCheckInfoRepository(IOptionsSnapshot<DbOption> options)
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
			string sql = "SELECT ENABLED FROM MES_FIRST_CHECK_INFO WHERE ID=:ID";
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
			string sql = "UPDATE MES_FIRST_CHECK_INFO set ENABLED=:ENABLED WHERE ID=:Id";
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
			string sql = "SELECT MES_FIRST_CHECK_INFO_SEQ.NEXTVAL MY_SEQ FROM DUAL";
			var result = await _dbConnection.ExecuteScalarAsync(sql);
			return (decimal)result;
		}

		/// <summary>
		/// 根据主表ID删除明细数据
		/// </summary>
		/// <param name="mstId"></param>
		/// <returns></returns>
		public async Task<BaseResult> DeleteDetailByMstId(decimal mstId)
		{
			var result = new BaseResult();
			string delItemSql = "DELETE MES_FIRST_CHECK_DETAIL_ITEM WHERE DETAIL_ID IN (SELECT ID FROM MES_FIRST_CHECK_DETAIL WHERE MST_ID=:MST_ID)";
			string delDetailSql = "DELETE MES_FIRST_CHECK_DETAIL WHERE MST_ID =:MST_ID";

			ConnectionFactory.OpenConnection(_dbConnection);
			using (var tran = _dbConnection.BeginTransaction())
			{
				try
				{
					await _dbConnection.ExecuteAsync(delItemSql, new { MST_ID = mstId });
					await _dbConnection.ExecuteAsync(delDetailSql, new { MST_ID = mstId });

					tran.Commit();
					result.ResultCode = ResultCodeAddMsgKeys.CommonObjectSuccessCode;
					result.ResultMsg = ResultCodeAddMsgKeys.CommonObjectSuccessMsg;
					return result;
				}
				catch (Exception ex)
				{
					tran.Rollback(); // 回滚事务
					result.ResultCode = ResultCodeAddMsgKeys.CommonExceptionCode;
					result.ResultMsg = ex.Message;
					return result;
				}
				finally
				{
					if (_dbConnection.State != System.Data.ConnectionState.Closed)
					{
						_dbConnection.Close();
					}
				}
			}
		}

		/// <summary>
		/// 获取首五件检验明细数据
		/// </summary>
		/// <param name="mstId"></param>
		/// <returns></returns>
		public async Task<IEnumerable<MesFirstCheckDetailListModel>> GetDetailData(decimal mstId)
		{
			string sql = @"SELECT D.*, I.STATUS AS MST_STATUS
							FROM MES_FIRST_CHECK_DETAIL D
								 INNER JOIN MES_FIRST_CHECK_INFO I ON D.MST_ID = I.ID
						   WHERE MST_ID = :MST_ID
						ORDER BY CHECK_TIME DESC";

			var result = await _dbConnection.QueryAsync<MesFirstCheckDetailListModel>(sql, new { MST_ID = mstId });
			return result;
		}

		/// <summary>
		/// 新增修改首五件明细数据
		/// </summary>
		/// <param name="item"></param>
		/// <returns></returns>
		public async Task<BaseResult> AddOrModifyDetailSave(MesFirstCheckDetailAddOrModifyModel item)
		{
			var result = new BaseResult();
			if (IsPartSn(item.MST_ID, item.PART_SN, item.ID))
			{
				result.ResultCode = ResultCodeAddMsgKeys.CommonExceptionCode;
				result.ResultMsg = string.Format("当前条码【{0}】已经存在检验数据！", item.PART_SN);
				return result;
			}

			string addSql = @"INSERT INTO MES_FIRST_CHECK_DETAIL VALUES(:ID,:MST_ID,:PART_SN,:RESULT_STATUS,:REMARK,SYSDATE,:CHECK_USER)";
			string editSql = @"UPDATE MES_FIRST_CHECK_DETAIL SET PART_SN=:PART_SN,RESULT_STATUS=:RESULT_STATUS,REMARK=:REMARK WHERE ID=:ID";
			string addItemSql = @"INSERT INTO MES_FIRST_CHECK_DETAIL_ITEM VALUES(MES_FIRST_CHECK_DETAIL_ITEM_SE.NEXTVAL,:DETAIL_ID,:ITEM_ID,:RESULT_VALUE)";
			string editItemSql = @"UPDATE MES_FIRST_CHECK_DETAIL_ITEM SET RESULT_VALUE = :RESULT_VALUE WHERE ID=:ID";

			ConnectionFactory.OpenConnection(_dbConnection);
			using (var tran = _dbConnection.BeginTransaction())
			{
				try
				{
					if (item.ID == 0)
					{
						item.ID = GetDetailSeq();
						await _dbConnection.ExecuteAsync(addSql, item);
						foreach (var model in item.ItemList)
						{
							model.DETAIL_ID = item.ID;
							await _dbConnection.ExecuteAsync(addItemSql, model);
						}
					}
					else
					{
						await _dbConnection.ExecuteAsync(editSql, item);
						foreach (var model in item.ItemList)
						{
							await _dbConnection.ExecuteAsync(editItemSql, model);
						}
					}
					tran.Commit();
					result.ResultCode = ResultCodeAddMsgKeys.CommonObjectSuccessCode;
					result.ResultMsg = ResultCodeAddMsgKeys.CommonObjectSuccessMsg;
					return result;
				}
				catch (Exception ex)
				{
					tran.Rollback(); // 回滚事务
					result.ResultCode = ResultCodeAddMsgKeys.CommonExceptionCode;
					result.ResultMsg = ex.Message;
					return result;
				}
				finally
				{
					if (_dbConnection.State != System.Data.ConnectionState.Closed)
					{
						_dbConnection.Close();
					}
				}
			}
		}

		/// <summary>
		/// 删除首五件明细数据
		/// </summary>
		/// <param name="id"></param>
		/// <returns></returns>
		public async Task<BaseResult> DeleteDetail(decimal id)
		{
			var result = new BaseResult();
			string delItemSql = "DELETE MES_FIRST_CHECK_DETAIL_ITEM WHERE DETAIL_ID =:DETAIL_ID";
			string delDetailSql = "DELETE MES_FIRST_CHECK_DETAIL WHERE ID =:DETAIL_ID";

			ConnectionFactory.OpenConnection(_dbConnection);
			using (var tran = _dbConnection.BeginTransaction())
			{
				try
				{
					await _dbConnection.ExecuteAsync(delItemSql, new { DETAIL_ID = id });
					await _dbConnection.ExecuteAsync(delDetailSql, new { DETAIL_ID = id });

					tran.Commit();
					result.ResultCode = ResultCodeAddMsgKeys.CommonObjectSuccessCode;
					result.ResultMsg = ResultCodeAddMsgKeys.CommonObjectSuccessMsg;
					return result;
				}
				catch (Exception ex)
				{
					tran.Rollback(); // 回滚事务
					result.ResultCode = ResultCodeAddMsgKeys.CommonExceptionCode;
					result.ResultMsg = ex.Message;
					return result;
				}
				finally
				{
					if (_dbConnection.State != System.Data.ConnectionState.Closed)
					{
						_dbConnection.Close();
					}
				}
			}
		}

		/// <summary>
		/// 获取检验明细项目信息
		/// </summary>
		/// <param name="detailId"></param>
		/// <returns></returns>
		public async Task<IEnumerable<MesFirstCheckDetailItemListModel>> GetDetailItemData(decimal detailId)
		{
			string sql = @" SELECT D.*,
							 I.CHECK_ITEM,
							 I.CHECK_GIST,
							 P.MEANING AS CHECK_TYPE_NAME
						FROM MES_FIRST_CHECK_DETAIL_ITEM D
							 INNER JOIN MES_FIRST_CHECK_ITEMS I ON D.ITEM_ID = I.ID
							 INNER JOIN
							 (SELECT LOOKUP_CODE, MEANING
								FROM SFCS_PARAMETERS
							   WHERE LOOKUP_TYPE = 'FIRST_CHECK_ITEM_TYPE' AND ENABLED = 'Y') P
								ON I.CHECK_TYPE = P.LOOKUP_CODE
					   WHERE D.DETAIL_ID = :DETAIL_ID
					ORDER BY I.CHECK_TYPE, I.ORDER_NO";

			var result = await _dbConnection.QueryAsync<MesFirstCheckDetailItemListModel>(sql, new { DETAIL_ID = detailId });
			return result;
		}

		/// <summary>
		/// 审核首五件信息
		/// </summary>
		/// <param name="item"></param>
		/// <returns></returns>
		public async Task<BaseResult> AuditData(MesFirstCheckInfoAddOrModifyModel item)
		{
			var result = new BaseResult();

			try
			{
				var model = Get(item.ID);
				if (model == null)
				{
					result.ResultCode = ResultCodeAddMsgKeys.CommonExceptionCode;
					result.ResultMsg = "当前审核的首五件检验信息不存在，请刷新后重试！";
					return result;
				}

				if (model.STATUS != 0)
				{
					result.ResultCode = ResultCodeAddMsgKeys.CommonExceptionCode;
					result.ResultMsg = "当前审核的首五件检验不是未审核状态，无法做审核操作！";
					return result;
				}

				var count = GetDetailCount(item.ID);
				if (count < 5)
				{
					result.ResultCode = ResultCodeAddMsgKeys.CommonExceptionCode;
					result.ResultMsg = "当前首五件检验明细不够5条，请继续检验！";
					return result;
				}

				if (count > 5)
				{
					result.ResultCode = ResultCodeAddMsgKeys.CommonExceptionCode;
					result.ResultMsg = "当前首五件检验明细已超过5条，请核实！";
					return result;
				}

				string sql = "UPDATE MES_FIRST_CHECK_INFO SET STATUS=1,RESULT_STATUS=:RESULT_STATUS,RESULT_REMARK=:RESULT_REMARK,AUDIT_TIME=SYSDATE,AUDIT_USER=:AUDIT_USER WHERE ID=:ID";

				await _dbConnection.ExecuteAsync(sql, new { item.ID, item.RESULT_STATUS, item.RESULT_REMARK, item.AUDIT_USER });

				result.ResultCode = ResultCodeAddMsgKeys.CommonObjectSuccessCode;
				result.ResultMsg = "审核成功！";
				return result;
			}
			catch (Exception ex)
			{
				result.ResultCode = ResultCodeAddMsgKeys.CommonExceptionCode;
				result.ResultMsg = "审核失败，原因：" + ex.Message;
				return result;
			}
		}

		/// <summary>
		/// 获取检验明细序列
		/// </summary>
		/// <returns></returns>
		private decimal GetDetailSeq()
		{
			string sql = "SELECT MES_FIRST_CHECK_DETAIL_SEQ.NEXTVAL FROM DUAL";
			return _dbConnection.ExecuteScalar<decimal>(sql);
		}

		/// <summary>
		/// 判断条码是否已经存在检验数据
		/// </summary>
		/// <param name="mstId"></param>
		/// <param name="sn"></param>
		/// <param name="detailId"></param>
		/// <returns></returns>
		private bool IsPartSn(decimal mstId, string sn, decimal detailId)
		{
			string sql = "SELECT COUNT (1) FROM MES_FIRST_CHECK_DETAIL WHERE MST_ID = :MST_ID AND PART_SN = :PART_SN";
			if (detailId != 0)
				sql += " AND ID <> :DETAIL_ID";

			var i = _dbConnection.ExecuteScalar<int>(sql, new { MST_ID = mstId, PART_SN = sn, DETAIL_ID = detailId });
			return i > 0;
		}

		/// <summary>
		/// 获取明细数量
		/// </summary>
		/// <param name="mstId"></param>
		/// <returns></returns>
		private int GetDetailCount(decimal mstId)
		{
			string sql = "SELECT COUNT (1) FROM MES_FIRST_CHECK_DETAIL WHERE MST_ID = :MST_ID";
			var i = _dbConnection.ExecuteScalar<int>(sql, new { MST_ID = mstId });
			return i;
		}
	}
}