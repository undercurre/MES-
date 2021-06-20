/**
*┌──────────────────────────────────────────────────────────────┐
*│　描    述：接口实现                                                    
*│　作    者：嘉志科技                                            
*│　版    本：2.0    模板代码自动生成                                                
*│　创建时间：2020-05-19 09:10:09                             
*└──────────────────────────────────────────────────────────────┘
*┌──────────────────────────────────────────────────────────────┐
*│　命名空间： JZ.IMS.Repository.Oracle                                  
*│　类    名： MesQualityInfoRepository                                      
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
using JZ.IMS.Core.Extensions;

namespace JZ.IMS.Repository.Oracle
{
	public class MesQualityInfoRepository : BaseRepository<MesQualityInfo, Decimal>, IMesQualityInfoRepository
	{
		public MesQualityInfoRepository(IOptionsSnapshot<DbOption> options)
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
			string sql = "SELECT ENABLED FROM MES_QUALITY_INFO WHERE ID=:ID";
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
			string sql = "UPDATE MES_QUALITY_INFO set ENABLED=:ENABLED,UPDATE_TIME=SYSDATE WHERE ID=:Id";
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
			string sql = "SELECT MES_QUALITY_INFO_SEQ.NEXTVAL MY_SEQ FROM DUAL";
			var result = await _dbConnection.ExecuteScalarAsync(sql);
			return (decimal)result;
		}

		/// <summary>
		/// 删除数据
		/// </summary>
		/// <param name="mstId"></param>
		/// <returns></returns>
		public async Task<BaseResult> DeleteData(decimal mstId)
		{
			var result = new BaseResult();
			string delSql = "DELETE MES_QUALITY_DETAIL WHERE MST_ID = :MST_ID";
			string delDetailSql = "DELETE MES_QUALITY_INFO WHERE ID = :MST_ID";

			ConnectionFactory.OpenConnection(_dbConnection);
			using (var tran = _dbConnection.BeginTransaction())
			{
				try
				{
					await _dbConnection.ExecuteAsync(delSql, new { MST_ID = mstId });
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

		public async Task<IEnumerable<MesQualityCheckDetailListModel>> GetDetailData(decimal mstId)
		{
			string sql = @"SELECT D.*, I.STATUS AS MST_STATUS
							FROM MES_QUALITY_DETAIL D
								 INNER JOIN MES_QUALITY_INFO I ON D.MST_ID = I.ID
						   WHERE MST_ID = :MST_ID";

			var result = await _dbConnection.QueryAsync<MesQualityCheckDetailListModel>(sql, new { MST_ID = mstId });
			return result;
		}

		public async Task<IEnumerable<MesFirstCheckRecordDetailListModel>> GetMesFirstCheckDetailData(decimal mstId)
		{
			string sql = @"SELECT D.*, I.STATUS AS MST_STATUS
                                                FROM MES_FIRST_CHECK_RECORD_DETAIL D
                                                        INNER JOIN MES_QUALITY_INFO I ON D.HID = I.ID
                                            WHERE D.HID = TO_CHAR( :MST_ID)";
			var result = await _dbConnection.QueryAsync<MesFirstCheckRecordDetailListModel>(sql, new { MST_ID = mstId });
			return result;
		}

		/// <summary>
		/// 新增/修改检验明细
		/// </summary>
		/// <param name="item"></param>
		/// <param name="RESULT_STATUS"></param>
		/// <param name="REMARK"></param>
		/// <param name="MST_ID"></param>
		/// <returns></returns>
		public async Task<BaseResult> AddOrModifyDetailSave(List<MesQualityDetailAddOrModifyModel> itemList, decimal result_status, string REMARK, decimal MST_ID)
		{
			var result = new BaseResult();
			string mainSql = @"UPDATE MES_QUALITY_INFO SET RESULT_STATUS=:RESULT_STATUS,RESULT_REMARK=:RESULT_REMARK,UPDATE_TIME=SYSDATE WHERE ID=:MST_ID";
			//string addItemSql = @"INSERT INTO MES_QUALITY_DETAIL(ID, MST_ID, ITEM_ID, RESULT_VALUE) 
   //                               VALUES(MES_QUALITY_DETAIL_SEQ.NEXTVAL,:MST_ID,:ITEM_ID,:RESULT_VALUE)";

			String upItemSql = @"MERGE INTO MES_QUALITY_DETAIL D USING DUAL ON(D.ITEM_ID =:ITEM_ID AND D.MST_ID =:MST_ID )
                                               WHEN MATCHED THEN
                                                   UPDATE SET RESULT_VALUE=:RESULT_VALUE,RESULT_TYPE=:RESULT_TYPE
                                               WHEN NOT MATCHED THEN
                                                   INSERT(ID, MST_ID, ITEM_ID, RESULT_VALUE, RESULT_TYPE) 
                                                   VALUES(MES_QUALITY_DETAIL_SEQ.NEXTVAL,:MST_ID,:ITEM_ID,:RESULT_VALUE,:RESULT_TYPE)";
			ConnectionFactory.OpenConnection(_dbConnection);
			using (var tran = _dbConnection.BeginTransaction())
			{
				try
                {
                    await _dbConnection.ExecuteAsync(mainSql, new { RESULT_STATUS = result_status, RESULT_REMARK = REMARK, MST_ID }, tran);

                    foreach (var item in itemList)
					{
                        if (item.ITEM_ID > 0)
                        {
                            if (String.IsNullOrEmpty(item.RESULT_TYPE)) { item.RESULT_TYPE = "N"; }
                            else if (item.RESULT_TYPE != "N" && item.RESULT_TYPE != "Y") { throw new Exception("检查结果类型错误！"); }
                            await _dbConnection.ExecuteAsync(upItemSql, new
                            {
                                MST_ID = MST_ID,
                                ITEM_ID = item.ITEM_ID,
                                RESULT_VALUE = item.RESULT_VALUE,
								RESULT_TYPE = item.RESULT_TYPE
							}, tran);
                        }
                    }

					////新增项目
					//var newList = itemList.Where(t => t.ID == 0).ToList();
					//await _dbConnection.ExecuteAsync(addItemSql, newList, tran);

					////修改项目
					//var editList = itemList.Where(t => t.ID > 0).ToList();
					//string editItemSql = @"Update MES_QUALITY_DETAIL SET RESULT_VALUE=:RESULT_VALUE WHERE ID =:ID AND MST_ID =:MST_ID";
					//await _dbConnection.ExecuteAsync(editItemSql, editList, tran);

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
        /// 新增/修改物料确认明细
        /// </summary>
        /// <param name="item"></param>
        /// <param name="RESULT_STATUS"></param>
        /// <param name="REMARK"></param>
        /// <param name="MST_ID"></param>
        /// <returns></returns>
        public async Task<BaseResult> AddOrModifyBOMDetailSave(List<MesFirstCheckRecordDetailAddOrModifyModel> itemList, decimal RESULT_STATUS, string REMARK, decimal MST_ID)
        {
            var result = new BaseResult();
            string mainSql = @"UPDATE MES_QUALITY_INFO SET RESULT_STATUS=:RESULT_STATUS,RESULT_REMARK=:RESULT_REMARK,UPDATE_TIME=SYSDATE WHERE ID=:MST_ID";

            //String updateDetailSql = @"MERGE INTO MES_FIRST_CHECK_RECORD_DETAIL MFRD USING DUAL ON(MFRD.PART_NO =:PART_NO AND MFRD.HID =:HID )
            //                                   WHEN MATCHED THEN
            //                                       UPDATE SET RESULT=:RESULT,TENSION_VALUE=:TENSION_VALUE,TEST_VALUE=:TEST_VALUE,BRAND_NAME=:BRAND_NAME,VENDOR_NAME=:VENDOR_NAME
            //                                   WHEN NOT MATCHED THEN
            //                                       INSERT(ID, HID, POSITION,PART_NO, RESULT, TENSION_VALUE,TEST_VALUE, BRAND_NAME, VENDOR_NAME) 
            //                                       VALUES(:ID,:HID,:POSITION,:PART_NO,:RESULT,:TENSION_VALUE,:TEST_VALUE,:BRAND_NAME,:VENDOR_NAME) ";

            String insertDetailSql = @"INSERT INTO MES_FIRST_CHECK_RECORD_DETAIL (ID, HID, POSITION,PART_NO, RESULT, TENSION_VALUE,TEST_VALUE, BRAND_NAME, VENDOR_NAME, PARENT_PART_NO) VALUES (:ID,:HID,:POSITION,:PART_NO,:RESULT,:TENSION_VALUE,:TEST_VALUE,:BRAND_NAME,:VENDOR_NAME,:PARENT_PART_NO)";

            String updateDetailSql = @"UPDATE MES_FIRST_CHECK_RECORD_DETAIL SET RESULT=:RESULT, TENSION_VALUE=:TENSION_VALUE, TEST_VALUE=:TEST_VALUE, BRAND_NAME=:BRAND_NAME, VENDOR_NAME=:VENDOR_NAME WHERE ID = :ID AND HID = :HID AND PART_NO = :PART_NO ";

            ConnectionFactory.OpenConnection(_dbConnection);
            using (var tran = _dbConnection.BeginTransaction())
            {
                try
                {

                    await _dbConnection.ExecuteAsync(mainSql, new { RESULT_STATUS, RESULT_REMARK = REMARK, MST_ID }, tran);

                    String executeSql = "";

                    foreach (var item in itemList)
                    {
						String id = "";
                        if (String.IsNullOrEmpty(item.PARENT_PART_NO))
                        {
							id = await _dbConnection.QueryFirstOrDefaultAsync<String>("SELECT ID FROM MES_FIRST_CHECK_RECORD_DETAIL WHERE PART_NO = :PART_NO AND HID= :HID AND (PARENT_PART_NO IS NULL OR PARENT_PART_NO = '') ", new { PART_NO = item.PART_NO, HID = MST_ID.ToString() });
                        }
                        else
						{
							id = await _dbConnection.QueryFirstOrDefaultAsync<String>("SELECT ID FROM MES_FIRST_CHECK_RECORD_DETAIL WHERE PART_NO = :PART_NO AND HID= :HID AND PARENT_PART_NO = :PARENT_PART_NO ", new { PART_NO = item.PART_NO, HID = MST_ID.ToString(), PARENT_PART_NO = item.PARENT_PART_NO });
						}

                        if (String.IsNullOrEmpty(id)) { id = System.Guid.NewGuid().ToString("N"); executeSql = insertDetailSql; }
                        else { executeSql = updateDetailSql; }

                        await _dbConnection.ExecuteAsync(executeSql, new
                        {
                            ID = id,
                            HID = MST_ID.ToString(),
                            POSITION = item.POSITION,
                            PART_NO = item.PART_NO,
                            RESULT = item.RESULT,
                            TENSION_VALUE = item.TENSION_VALUE,
                            TEST_VALUE = item.TEST_VALUE,
                            BRAND_NAME = item.BRAND_NAME,
                            VENDOR_NAME = item.VENDOR_NAME,
							PARENT_PART_NO = item.PARENT_PART_NO
						}, tran);
                    }

					////新增项目
					//var newList = itemList.Where(t => t.ID.IsNullOrEmpty()).ToList();
					//newList.ForEach(c=>c.ID= System.Guid.NewGuid().ToString("N"));
					//await _dbConnection.ExecuteAsync(addItemSql, newList, tran);

					////修改项目
					//var editList = itemList.Where(t => !t.ID.IsNullOrEmpty()).ToList();
					//string editItemSql = @"Update MES_FIRST_CHECK_RECORD_DETAIL SET RESULT=:RESULT,TENSION_VALUE=:TENSION_VALUE WHERE ID =:ID AND HID =:HID";
					//await _dbConnection.ExecuteAsync(editItemSql, editList, tran);

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

        public async Task<BaseResult> DeleteDetail(decimal id)
		{
			var result = new BaseResult();
			string delItemSql = "DELETE MES_QUALITY_DETAIL_ITEM WHERE DETAIL_ID =:DETAIL_ID";
			string delDetailSql = "DELETE MES_QUALITY_DETAIL WHERE ID =:DETAIL_ID";

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
		/// <param name="mst_id"></param>
		/// <returns></returns>
		public async Task<IEnumerable<dynamic>> GetDetailItemData(decimal mst_id)
		{
			string sql = @" SELECT D.ID, D.MST_ID, D.ITEM_ID, D.RESULT_VALUE, D.RESULT_TYPE, I.CHECK_ITEM,I.CHECK_DESC,I.REMARK,I.QUANTIZE_TYPE,I.ISEMPTY
						FROM MES_QUALITY_DETAIL D
							 INNER JOIN MES_QUALITY_ITEMS I ON D.ITEM_ID = I.ID
					   WHERE D.MST_ID = :MST_ID
					ORDER BY I.CHECK_TYPE, I.ORDER_NO ";

			var result = await _dbConnection.QueryAsync<dynamic>(sql, new { MST_ID = mst_id });
			return result;
		}

		/// <summary>
		/// 获取BOM明细项目信息
		/// </summary>
		/// <param name="mst_id"></param>
		/// <returns></returns>
		public async Task<IEnumerable<dynamic>> GetDetailBOMData(decimal mst_id,String parent_part_no_sql)
        {
            string sql = @" SELECT MFRD.POSITION,MFRD.PART_NO,MFRD.RESULT,MFRD.TENSION_VALUE,MFRD.TEST_VALUE,MFRD.BRAND_NAME,MFRD.VENDOR_NAME,IP.NAME AS PART_NAME,IP.DESCRIPTION AS PART_DESC FROM MES_FIRST_CHECK_RECORD_DETAIL MFRD LEFT JOIN IMS_PART IP ON MFRD.PART_NO = IP.CODE WHERE  HID=TO_CHAR(:HID) " + parent_part_no_sql + " ORDER BY MFRD.PART_NO ";

            var result = await _dbConnection.QueryAsync<dynamic>(sql, new { HID = mst_id });
			return result;
		}

		/// <summary>
		/// 审核数据
		/// </summary>
		/// <param name="item"></param>
		/// <returns></returns>
		public async Task<BaseResult> AuditData(MesQualityInfoAddOrModifyModel item)
		{
			var result = new BaseResult();

			try
			{
				var model = Get(item.ID);
				if (model == null)
				{
					result.ResultCode = ResultCodeAddMsgKeys.CommonExceptionCode;
					result.ResultMsg = "当前审核的检验信息不存在，请刷新后重试！";
					return result;
				}

				if (model.STATUS != 0)
				{
					result.ResultCode = ResultCodeAddMsgKeys.CommonExceptionCode;
					result.ResultMsg = "当前审核的检验不是未审核状态，无法做审核操作！";
					return result;
				}

				string sql = "UPDATE MES_QUALITY_INFO SET STATUS = 1,AUDIT_TIME=SYSDATE,AUDIT_USER=:AUDIT_USER,UPDATE_TIME=SYSDATE WHERE ID=:ID";

				await _dbConnection.ExecuteAsync(sql, new { item.ID, item.AUDIT_USER });

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
		/// 通过检验类别+日期+工单+板型获取信息
		/// </summary>
		/// <param name="item"></param>
		/// <returns></returns>
		public async Task<BaseResult> GetDetailCount(MesQualityInfoAddOrModifyModel item)
		{
			var result = new BaseResult();
			try
			{
				string sql = "SELECT COUNT (1) FROM MES_QUALITY_INFO WHERE CHECK_TYPE = :CHECK_TYPE AND BATCH_NO = :BATCH_NO AND PCB_SIDE=:PCB_SIDE AND PRODUCT_DATE=:PRODUCT_DATE";
				if (item.ID > 0)
					sql += " AND ID <> :ID";

				var i = await _dbConnection.ExecuteScalarAsync<int>(sql, new { item.CHECK_TYPE, item.BATCH_NO, item.PCB_SIDE, item.PRODUCT_DATE, item.ID });
				if (i > 0)
				{
					result.ResultCode = ResultCodeAddMsgKeys.CommonExceptionCode;
					result.ResultMsg = "当前检验类别【" + GetCheckTypeName(item.CHECK_TYPE) + "】工单【" + item.BATCH_NO + "】板型【" + GetPcbSideName(item.PCB_SIDE) + "】在日期【" + item.PRODUCT_DATE.ToString("yyyy-MM-dd") + "】已存在，不可重复添加！";
					return result;
				}
			}
			catch (Exception ex)
			{
				result.ResultCode = ResultCodeAddMsgKeys.CommonExceptionCode;
				result.ResultMsg = ex.Message;
				return result;
			}
			return result;
		}

		private string GetCheckTypeName(decimal checkType)
		{
			string sql = "SELECT MEANING FROM SFCS_PARAMETERS  WHERE LOOKUP_TYPE='MES_QUALITY_TYPE' and ENABLED = 'Y' AND LOOKUP_CODE = :LOOKUP_CODE";
			return _dbConnection.ExecuteScalar<string>(sql, new { LOOKUP_CODE = checkType });
		}

		/// <summary>
		/// 返回物料信息
		/// </summary>
		/// <param name="item"></param>
		/// <returns></returns>
		public async Task<BaseResult> GetBOMInformation(MesQualityInfoAddOrModifyModel item)
		{
			var result = new BaseResult();
			try
			{
				
			}
			catch (Exception ex)
			{
				result.ResultCode = ResultCodeAddMsgKeys.CommonExceptionCode;
				result.ResultMsg = ex.Message;
				return result;
			}
			return result;
		}

		private string GetPcbSideName(decimal? pcbSide)
		{
			switch (pcbSide)
			{
				case 1:
					return "板面";
				case 2:
					return "板底";
				case 3:
					return "板底+板面";
				default:
					return "";
			}
		}
	}
}