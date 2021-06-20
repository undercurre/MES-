/**
*┌──────────────────────────────────────────────────────────────┐
*│　描    述：接口实现                                                    
*│　作    者：嘉志科技                                            
*│　版    本：2.0    模板代码自动生成                                                
*│　创建时间：2020-04-06 14:36:26                             
*└──────────────────────────────────────────────────────────────┘
*┌──────────────────────────────────────────────────────────────┐
*│　命名空间： JZ.IMS.Repository.Oracle                                  
*│　类    名： SfcsProductResourcesRepository                                      
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
using JZ.IMS.ViewModels.ProductBasicSet.ComponentReplace;
using JZ.IMS.Core.Utilities;
using JZ.IMS.Core.Extensions;

namespace JZ.IMS.Repository.Oracle
{
    public class SfcsProductResourcesRepository:BaseRepository<SfcsProductResources,Decimal>, ISfcsProductResourcesRepository
    {
        public SfcsProductResourcesRepository(IOptionsSnapshot<DbOption> options)
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
			string sql = "SELECT ENABLED FROM SFCS_PRODUCT_RESOURCES WHERE ID=:ID";
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
			string sql = "UPDATE SFCS_PRODUCT_RESOURCES set ENABLED=:ENABLED WHERE ID=:Id";
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
			string sql = "SELECT SFCS_PRODUCT_RESOURCES_SEQ.NEXTVAL MY_SEQ FROM DUAL";
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
			string sql = "select count(0) from SFCS_PRODUCT_RESOURCES where id = :id";
			object result = await _dbConnection.ExecuteScalarAsync(sql, new
			{
				id
			});

			return (Convert.ToInt32(result) > 0);
		}

		/// <summary>
		///料号是否存在
		/// </summary>
		/// <param name="partno">料号partno</param>
		/// <returns></returns>
		public async Task<bool> ItemIsByPartNo(string partno)
		{
			string sql = "select count(0) from SFCS_PN where PART_NO = :PART_NO";
			object result = await _dbConnection.ExecuteScalarAsync(sql, new
			{
				PART_NO = partno
			});

			return (Convert.ToInt32(result) > 0);
		}

		/// <summary>
		/// 保存数据
		/// </summary>
		/// <param name="model"></param>
		/// <returns></returns>
		public async Task<decimal> SaveDataByTrans(SfcsProductResourcesModel model)
		{
			int result = 1;
			ConnectionFactory.OpenConnection(_dbConnection);
			using (var tran = _dbConnection.BeginTransaction())
			{
				try
				{
					#region 添加产品资料维护
					//新增
					string insertproductSql = @"insert into SFCS_PRODUCT_RESOURCES 
					(ID,PART_NO,RESOURCE_ID,DATA_FORMAT,FIXED_VALUE,RESOURCE_QTY,ENABLED,REWORK_REMOVE_FLAG,EDI_FLAG,COLLECT_OPERATION_ID) 
					VALUES (:ID,:PART_NO,:RESOURCE_ID,:DATA_FORMAT,:FIXED_VALUE,:RESOURCE_QTY,:ENABLED,:REWORK_REMOVE_FLAG,:EDI_FLAG,:COLLECT_OPERATION_ID)";
					if (model.InsertRecords != null && model.InsertRecords.Count > 0)
					{
						foreach (var item in model.InsertRecords)
						{
							var newid = await Get_MES_SEQ_ID();
							var resdata = await _dbConnection.ExecuteAsync(insertproductSql, new
							{
								ID = newid,
								item.PART_NO,
								item.RESOURCE_ID,
								item.DATA_FORMAT,
								item.FIXED_VALUE,
								item.RESOURCE_QTY,
								item.ENABLED,
								item.REWORK_REMOVE_FLAG,
								item.EDI_FLAG,
								item.COLLECT_OPERATION_ID,							}, tran);
						}
					}
					//更新
					string updateproductSql = @"Update SFCS_PRODUCT_RESOURCES set PART_NO=:PART_NO,RESOURCE_ID=:RESOURCE_ID,DATA_FORMAT=:DATA_FORMAT,FIXED_VALUE=:FIXED_VALUE,RESOURCE_QTY=:RESOURCE_QTY,ENABLED=:ENABLED,REWORK_REMOVE_FLAG=:REWORK_REMOVE_FLAG,EDI_FLAG=:EDI_FLAG,COLLECT_OPERATION_ID=:COLLECT_OPERATION_ID  
						where ID=:ID ";
					if (model.UpdateRecords != null && model.UpdateRecords.Count > 0)
					{
						foreach (var item in model.UpdateRecords)
						{
							var resdata = await _dbConnection.ExecuteAsync(updateproductSql, new
							{
								item.ID,
								item.PART_NO,
								item.RESOURCE_ID,
								item.DATA_FORMAT,
								item.FIXED_VALUE,
								item.RESOURCE_QTY,
								item.ENABLED,
								item.REWORK_REMOVE_FLAG,
								item.EDI_FLAG,
								item.COLLECT_OPERATION_ID,							}, tran);
						}
					}
					//删除
					string deleteSql = @"DELETE FROM SFCS_PRODUCT_RESOURCES WHERE ID=:ID ";
					string deleteattachmentbyobjectSql = @" DELETE FROM SFCS_PRODUCT_ATTACHMENTS WHERE PRODUCT_OBJECT_ID=:PRODUCT_OBJECT_ID ";
					if (model.RemoveRecords != null && model.RemoveRecords.Count > 0)
					{
						foreach (var item in model.RemoveRecords)
						{
							var resdata = await _dbConnection.ExecuteAsync(deleteSql, new
							{
								item.ID
							}, tran);
							var desdata = await _dbConnection.ExecuteAsync(deleteattachmentbyobjectSql,new {
								PRODUCT_OBJECT_ID=item.ID
							},tran);
						}
					}

					#endregion

					#region 附件处理
					string insertattachmentsSql = @"INSERT INTO SFCS_PRODUCT_ATTACHMENTS(ID, PRODUCT_OBJECT_ID, ATTACHMENT_ID, DATA_FORMAT, FIX_VALUE, ATTACHMENT_QTY, ENABLED)
                                                    VALUES(:ID, :PRODUCT_OBJECT_ID, :ATTACHMENT_ID, :DATA_FORMAT, :FIX_VALUE, :ATTACHMENT_QTY, :ENABLED)";
					if (model.InsertAttachments != null && model.InsertAttachments.Count > 0)
					{
						foreach (var item in model.InsertAttachments)
						{
							var resdata = await _dbConnection.ExecuteAsync(insertattachmentsSql, new
							{
								ID = await GetID("MES_SEQ_ID"),
								item.PRODUCT_OBJECT_ID,
								item.ATTACHMENT_ID,
								item.DATA_FORMAT,
								item.FIX_VALUE,
								item.ATTACHMENT_QTY,
								item.ENABLED
							}, tran) ;
						}
					}
					string updateattachmentsSql = @" UPDATE SFCS_PRODUCT_ATTACHMENTS SET 
                                                 ATTACHMENT_ID = :ATTACHMENT_ID,
                                                 DATA_FORMAT = :DATA_FORMAT,
                                                 FIX_VALUE = :FIX_VALUE,
                                                 ATTACHMENT_QTY = :ATTACHMENT_QTY,
                                                 ENABLED = :ENABLED 
                                                 WHERE ID=:ID";
					if (model.UpdateAttachments != null && model.UpdateAttachments.Count > 0)
					{
						foreach (var item in model.UpdateAttachments)
						{
							var resdata = await _dbConnection.ExecuteAsync(updateattachmentsSql, new
							{
								item.ID,
								item.ATTACHMENT_ID,
								item.DATA_FORMAT,
								item.FIX_VALUE,
								item.ATTACHMENT_QTY,
								item.ENABLED
							}, tran);
						}
					}

					//删除
					string deleteattachmentSql = @" DELETE FROM SFCS_PRODUCT_ATTACHMENTS WHERE ID=:ID ";
					if (model.RemoveAttachments != null && model.RemoveAttachments.Count > 0)
					{
						foreach (var item in model.RemoveAttachments)
						{
							var resdata = await _dbConnection.ExecuteAsync(deleteattachmentSql, new
							{
								item.ID
							}, tran);
						}
					}

					#endregion

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
		/// 资源替代的保存
		/// </summary>
		/// <param name="model"></param>
		/// <returns></returns>
		public async Task<decimal> SaveDataByOldResources(SfcsReplaceModel<CommReplaceViewModel> model)
		{
			int result = 1;
			ConnectionFactory.OpenConnection(_dbConnection);
			using (var tran = _dbConnection.BeginTransaction())
			{
				
				try
				{
					string selectDataFormSql = @"SELECT * FROM SFCS_PRODUCT_RESOURCES WHERE PART_NO=:PART_NO AND COLLECT_OPERATION_ID=:COLLECT_OPERATION_ID";

					//更新
					string updateSql = @"Update SFCS_RESOURCES_REPLACE SET NEW_NO=:NEW_NO,OLD_NO=:OLD_NO ";
					if (model.UpdateRecords != null && model.UpdateRecords.Count > 0)
					{
						foreach (var item in model.UpdateRecords)
						{
							var oldPrmodel = (await _dbConnection.QueryAsync<SfcsProductUids>(selectDataFormSql, new
							{
								PART_NO=item.PartNo,
								COLLECT_OPERATION_ID=item.CollectOperationID
							}))?.FirstOrDefault();
							//格式验证
							if (oldPrmodel != null && !oldPrmodel.DATA_FORMAT.IsNullOrWhiteSpace() && !FormatChecker.FormatCheck(item.NewNo, oldPrmodel.DATA_FORMAT))
							{
								return result = -1;
							}
							
							var resdata = await _dbConnection.ExecuteAsync(updateSql, new
							{
								OLD_NO = item.OldNo,
								NEW_NO = item.NewNo
							}, tran);
						}
					}

					//新增
					string insertSql = @"INSERT INTO SFCS_RESOURCES_REPLACE(REPLACE_RESOURCES_ID,NEW_NO,OLD_NO,REPLACE_BY,REPLACE_TIME)
                                         VALUES(SFCS_RESOURCES_REPLACE_SEQ.NEXTVAL,:NEW_NO,:OLD_NO,:REPLACE_BY,SYSDATE)";
					if (model.InsertRecords != null && model.InsertRecords.Count > 0)
					{
						foreach (var item in model.InsertRecords)
						{

							var oldPrmodel = (await _dbConnection.QueryAsync<SfcsProductUids>(selectDataFormSql, new
							{
								PART_NO = item.PartNo,
								COLLECT_OPERATION_ID = item.CollectOperationID
							}))?.FirstOrDefault();
							//格式验证
							if (oldPrmodel != null && !oldPrmodel.DATA_FORMAT.IsNullOrWhiteSpace() && !FormatChecker.FormatCheck(item.NewNo, oldPrmodel.DATA_FORMAT))
							{
								return result = -1;
							}

							var resdata = await _dbConnection.ExecuteAsync(insertSql, new
							{
								OLD_NO = item.OldNo,
								NEW_NO = item.NewNo,
								REPLACE_BY = model.UserName
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