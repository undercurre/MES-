/**
*┌──────────────────────────────────────────────────────────────┐
*│　描    述：接口实现                                                    
*│　作    者：嘉志科技                                            
*│　版    本：2.0    模板代码自动生成                                                
*│　创建时间：2020-03-31 18:04:38                             
*└──────────────────────────────────────────────────────────────┘
*┌──────────────────────────────────────────────────────────────┐
*│　命名空间： JZ.IMS.Repository.Oracle                                  
*│　类    名： SfcsProductUidsRepository                                      
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
using JZ.IMS.Core.Utilities;
using JZ.IMS.Core.Extensions;

namespace JZ.IMS.Repository.Oracle
{
    public class SfcsProductUidsRepository : BaseRepository<SfcsProductUids, Decimal>, ISfcsProductUidsRepository
    {
        public SfcsProductUidsRepository(IOptionsSnapshot<DbOption> options)
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
            string sql = "SELECT ENABLED FROM SFCS_PRODUCT_UIDS WHERE ID=:ID";
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
            string sql = "UPDATE SFCS_PRODUCT_UIDS set ENABLED=:ENABLED WHERE ID=:Id";
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
            string sql = "SELECT MES_SEQ_ID.NEXTVAL MY_SEQ FROM DUAL";
            var result = await _dbConnection.ExecuteScalarAsync(sql);
            return (decimal)result;
        }

        // <summary>
        /// 获取附件的序列
        /// </summary>
        /// <returns></returns>
		public async Task<decimal> GetAttachmentSEQID()
        {
            string sql = "SELECT SFCS_PRODUCT_ATTACHMENTS_SEQ.NEXTVAL MY_SEQ FROM DUAL";
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
            return true;
            string sql = "select count(0) from SFCS_PRODUCT_UIDS where id = :id";
            object result = await _dbConnection.ExecuteScalarAsync(sql, new
            {
                id
            });

            return (Convert.ToInt32(result) > 0);
        }

        /// <summary>
        /// 保存数据
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public async Task<decimal> SaveDataByTrans(SfcsProductUidsModel model)
        {
            decimal result = 1;
            ConnectionFactory.OpenConnection(_dbConnection);
            using (var tran = _dbConnection.BeginTransaction())
            {
                try
                {
                    //新增
                    string insertSql = @"insert into SFCS_PRODUCT_UIDS 
					(ID,PART_NO,UID_ID,DATA_FORMAT,UID_QTY,SERIALIZED,ENABLED,REWORK_REMOVE_FLAG,ORDERED,EDI_FLAG,COLLECT_OPERATION_ID,BREAK_OPERATION_ID) 
					VALUES (:ID,:PART_NO,:UID_ID,:DATA_FORMAT,:UID_QTY,:SERIALIZED,:ENABLED,:REWORK_REMOVE_FLAG,:ORDERED,:EDI_FLAG,:COLLECT_OPERATION_ID,:BREAK_OPERATION_ID)";
                    if (model.insertRecords != null && model.insertRecords.Count > 0)
                    {
                        foreach (var item in model.insertRecords)
                        {
                            var newid = await GetSEQID();
                            var resdata = await _dbConnection.ExecuteAsync(insertSql, new
                            {
                                ID = newid,
                                item.PART_NO,
                                item.UID_ID,
                                item.DATA_FORMAT,
                                item.UID_QTY,
                                item.SERIALIZED,
                                item.ENABLED,
                                item.REWORK_REMOVE_FLAG,
                                item.ORDERED,
                                item.EDI_FLAG,
                                item.COLLECT_OPERATION_ID,
                                item.BREAK_OPERATION_ID

                            }, tran);
                            result = newid;
                        }
                    }
                    //更新
                    string updateSql = @"Update SFCS_PRODUCT_UIDS set PART_NO=:PART_NO,UID_ID=:UID_ID,DATA_FORMAT=:DATA_FORMAT,UID_QTY=:UID_QTY,SERIALIZED=:SERIALIZED,
                                         ENABLED=:ENABLED,REWORK_REMOVE_FLAG=:REWORK_REMOVE_FLAG,ORDERED=:ORDERED,EDI_FLAG=:EDI_FLAG,COLLECT_OPERATION_ID=:COLLECT_OPERATION_ID ,BREAK_OPERATION_ID = :BREAK_OPERATION_ID
						                 where ID=:ID ";
                    if (model.updateRecords != null && model.updateRecords.Count > 0)
                    {
                        foreach (var item in model.updateRecords)
                        {
                            var resdata = await _dbConnection.ExecuteAsync(updateSql, new
                            {
                                item.ID,
                                item.PART_NO,
                                item.UID_ID,
                                item.DATA_FORMAT,
                                item.UID_QTY,
                                item.SERIALIZED,
                                item.ENABLED,
                                item.REWORK_REMOVE_FLAG,
                                item.ORDERED,
                                item.EDI_FLAG,
                                item.COLLECT_OPERATION_ID,
                                item.BREAK_OPERATION_ID
                            }, tran);
                        }
                    }
                    //删除
                    string deleteSql = @"Delete from SFCS_PRODUCT_UIDS where ID=:ID ";
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
		/// 保存附件数据
		/// </summary>
		/// <param name="model"></param>
		/// <returns></returns>
		public async Task<decimal> SaveAttachmentData(SfcsProductAttachmentsModel model)
        {
            decimal result = 1;
            ConnectionFactory.OpenConnection(_dbConnection);
            using (var tran = _dbConnection.BeginTransaction())
            {
                try
                {
                    //新增
                    string insertSql = @"insert into SFCS_PRODUCT_ATTACHMENTS 
					(ID,PRODUCT_OBJECT_ID,ATTACHMENT_ID,DATA_FORMAT,FIX_VALUE,ATTACHMENT_QTY,ENABLED) 
					VALUES (:ID,:PRODUCT_OBJECT_ID,:ATTACHMENT_ID,:DATA_FORMAT,:FIX_VALUE,:ATTACHMENT_QTY,:ENABLED)";
                    if (model.InsertRecords != null && model.InsertRecords.Count > 0)
                    {
                        foreach (var item in model.InsertRecords)
                        {
                            var newid = await GetAttachmentSEQID();
                            var resdata = await _dbConnection.ExecuteAsync(insertSql, new
                            {
                                ID = newid,
                                item.PRODUCT_OBJECT_ID,
                                item.ATTACHMENT_ID,
                                item.DATA_FORMAT,
                                item.FIX_VALUE,
                                item.ATTACHMENT_QTY,
                                item.ENABLED,

                            }, tran);
                            result = newid;
                        }
                    }
                    //更新
                    string updateSql = @"Update SFCS_PRODUCT_ATTACHMENTS set PRODUCT_OBJECT_ID=:PRODUCT_OBJECT_ID,ATTACHMENT_ID=:ATTACHMENT_ID,
                                         DATA_FORMAT=:DATA_FORMAT,FIX_VALUE=:FIX_VALUE,ATTACHMENT_QTY=:ATTACHMENT_QTY,ENABLED=:ENABLED  
						                 where ID=:ID ";
                    if (model.UpdateRecords != null && model.UpdateRecords.Count > 0)
                    {
                        foreach (var item in model.UpdateRecords)
                        {
                            var resdata = await _dbConnection.ExecuteAsync(updateSql, new
                            {
                                item.ID,
                                item.PRODUCT_OBJECT_ID,
                                item.ATTACHMENT_ID,
                                item.DATA_FORMAT,
                                item.FIX_VALUE,
                                item.ATTACHMENT_QTY,
                                item.ENABLED,

                            }, tran);
                        }
                    }
                    //删除
                    string deleteSql = @"Delete from SFCS_PRODUCT_ATTACHMENTS where ID=:ID ";
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

        /// <summary>
		/// 资源替代的保存
		/// </summary>
		/// <param name="model"></param>
		/// <returns></returns>
		public async Task<decimal> SaveDataByOldUIDs(SfcsReplaceModel<CommReplaceViewModel> model)
        {
            int result = 1;
            ConnectionFactory.OpenConnection(_dbConnection);
            using (var tran = _dbConnection.BeginTransaction())
            {
                string selectDataFormSql = @"SELECT * FROM SFCS_PRODUCT_UIDS WHERE PART_NO=:PART_NO AND COLLECT_OPERATION_ID=:COLLECT_OPERATION_ID";
                try
                {
                    //更新
                    string updateSql = @"UPDATE SFCS_UIDS_REPLACE SET NEW_NO=:NEW_NO,OLD_NO=:OLD_NO ";
                    if (model.UpdateRecords != null && model.UpdateRecords.Count > 0)
                    {
                        foreach (var item in model.UpdateRecords)
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
                            var resdata = await _dbConnection.ExecuteAsync(updateSql, new
                            {
                                OLD_NO = item.OldNo,
                                NEW_NO = item.NewNo
                            }, tran);
                        }
                    }

                    //新增
                    string insertSql = @"INSERT INTO SFCS_UIDS_REPLACE(REPLACE_UIDS_ID,NEW_NO,OLD_NO,REPLACE_BY,REPLACE_TIME)
                                         VALUES(SFCS_UIDS_REPLACE_SEQ.NEXTVAL,:NEW_NO,:OLD_NO,:REPLACE_BY,SYSDATE)";
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