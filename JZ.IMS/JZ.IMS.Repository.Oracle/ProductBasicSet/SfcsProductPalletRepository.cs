/**
*┌──────────────────────────────────────────────────────────────┐
*│　描    述：接口实现                                                    
*│　作    者：嘉志科技                                            
*│　版    本：2.0    模板代码自动生成                                                
*│　创建时间：2020-04-06 14:36:40                             
*└──────────────────────────────────────────────────────────────┘
*┌──────────────────────────────────────────────────────────────┐
*│　命名空间： JZ.IMS.Repository.Oracle                                  
*│　类    名： SfcsProductPalletRepository                                      
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

namespace JZ.IMS.Repository.Oracle
{
    public class SfcsProductPalletRepository:BaseRepository<SfcsProductPallet,Decimal>, ISfcsProductPalletRepository
    {
        public SfcsProductPalletRepository(IOptionsSnapshot<DbOption> options)
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
			string sql = "SELECT ENABLED FROM SFCS_PRODUCT_PALLET WHERE ID=:ID";
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
			string sql = "UPDATE SFCS_PRODUCT_PALLET set ENABLED=:ENABLED WHERE ID=:Id";
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
			string sql = "SELECT SFCS_PRODUCT_PALLET_SEQ.NEXTVAL MY_SEQ FROM DUAL";
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
			string sql = "select count(0) from SFCS_PRODUCT_PALLET where id = :id";
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
		public async Task<decimal> SaveDataByTrans(SfcsProductPalletModel model)
		{
			int result = 1;
			ConnectionFactory.OpenConnection(_dbConnection);
			using (var tran = _dbConnection.BeginTransaction())
			{
				try
				{
					#region 栈板维护设定
					//新增
					string insertSql = @"insert into SFCS_PRODUCT_PALLET 
					(ID,PART_NO,FORMAT,STANDARD_QUANTITY,MAX_QUANTITY,MIN_QUANTITY,STANDARD_WEIGHT,MAX_WEIGHT,MIN_WEIGHT,STANDARD_CUBAGE,MAX_CUBAGE,MIN_CUBAGE,LENGTH,WIDTH,HEIGHT,COLLECT_OPERATION_ID,ENABLED) 
					VALUES (:ID,:PART_NO,:FORMAT,:STANDARD_QUANTITY,:MAX_QUANTITY,:MIN_QUANTITY,:STANDARD_WEIGHT,:MAX_WEIGHT,:MIN_WEIGHT,:STANDARD_CUBAGE,:MAX_CUBAGE,:MIN_CUBAGE,:LENGTH,:WIDTH,:HEIGHT,:COLLECT_OPERATION_ID,:ENABLED)";
					if (model.InsertProductPallet != null && model.InsertProductPallet.Count > 0)
					{
						foreach (var item in model.InsertProductPallet)
						{
							var newid = await Get_MES_SEQ_ID();
							var resdata = await _dbConnection.ExecuteAsync(insertSql, new
							{
								ID = newid,
								item.PART_NO,
								item.FORMAT,
								item.STANDARD_QUANTITY,
								item.MAX_QUANTITY,
								item.MIN_QUANTITY,
								item.STANDARD_WEIGHT,
								item.MAX_WEIGHT,
								item.MIN_WEIGHT,
								item.STANDARD_CUBAGE,
								item.MAX_CUBAGE,
								item.MIN_CUBAGE,
								item.LENGTH,
								item.WIDTH,
								item.HEIGHT,
								item.COLLECT_OPERATION_ID,
								item.ENABLED,							}, tran);
						}
					}
					//更新
					string updateSql = @"Update SFCS_PRODUCT_PALLET set PART_NO=:PART_NO,FORMAT=:FORMAT,STANDARD_QUANTITY=:STANDARD_QUANTITY,MAX_QUANTITY=:MAX_QUANTITY,MIN_QUANTITY=:MIN_QUANTITY,STANDARD_WEIGHT=:STANDARD_WEIGHT,MAX_WEIGHT=:MAX_WEIGHT,MIN_WEIGHT=:MIN_WEIGHT,STANDARD_CUBAGE=:STANDARD_CUBAGE,MAX_CUBAGE=:MAX_CUBAGE,MIN_CUBAGE=:MIN_CUBAGE,LENGTH=:LENGTH,WIDTH=:WIDTH,HEIGHT=:HEIGHT,COLLECT_OPERATION_ID=:COLLECT_OPERATION_ID,ENABLED=:ENABLED  
						where ID=:ID ";
					if (model.UpdateProductPallet != null && model.UpdateProductPallet.Count > 0)
					{
						foreach (var item in model.UpdateProductPallet)
						{
							var resdata = await _dbConnection.ExecuteAsync(updateSql, new
							{
								item.ID,
								item.PART_NO,
								item.FORMAT,
								item.STANDARD_QUANTITY,
								item.MAX_QUANTITY,
								item.MIN_QUANTITY,
								item.STANDARD_WEIGHT,
								item.MAX_WEIGHT,
								item.MIN_WEIGHT,
								item.STANDARD_CUBAGE,
								item.MAX_CUBAGE,
								item.MIN_CUBAGE,
								item.LENGTH,
								item.WIDTH,
								item.HEIGHT,
								item.COLLECT_OPERATION_ID,
								item.ENABLED,
							}, tran);
						}
					}
					//删除
					string deleteSql = @"Delete from SFCS_PRODUCT_PALLET where ID=:ID ";
					string deleteattachmentbyobjectSql = @" DELETE FROM SFCS_PRODUCT_ATTACHMENTS WHERE PRODUCT_OBJECT_ID=:PRODUCT_OBJECT_ID ";
					if (model.RemoveRecords != null && model.RemoveRecords.Count > 0)
					{
						foreach (var item in model.RemoveRecords)
						{
							var resdata = await _dbConnection.ExecuteAsync(deleteSql, new
							{
								item.ID
							}, tran);
							var desdata = await _dbConnection.ExecuteAsync(deleteattachmentbyobjectSql, new
							{
								PRODUCT_OBJECT_ID = item.ID
							}, tran);
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
							var newid = await Get_MES_SEQ_ID();
							var resdata = await _dbConnection.ExecuteAsync(insertattachmentsSql, new
							{
								ID = newid,
								item.PRODUCT_OBJECT_ID,
								item.ATTACHMENT_ID,
								item.DATA_FORMAT,
								item.FIX_VALUE,
								item.ATTACHMENT_QTY,
								item.ENABLED
							}, tran);
						}
					}
					//更新
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
    }
}