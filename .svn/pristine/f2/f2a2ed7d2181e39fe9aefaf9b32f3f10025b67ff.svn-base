/*
*┌──────────────────────────────────────────────────────────────┐
*│　描    述：接口实现                                                    
*│　作    者：嘉志科技                                            
*│　版    本：2.0    模板代码自动生成                                                
*│　创建时间：2020-04-02 10:58:38                             
*└──────────────────────────────────────────────────────────────┘
*┌──────────────────────────────────────────────────────────────┐
*│　命名空间： JZ.IMS.Repository.Oracle                                  
*│　类    名： SfcsProductCartonRepository                                      
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
    public class SfcsProductCartonRepository : BaseRepository<SfcsProductCarton, Decimal>, ISfcsProductCartonRepository
    {
        public SfcsProductCartonRepository(IOptionsSnapshot<DbOption> options)
        {
            _dbOption = options.Get("iWMS");
            if (_dbOption == null)
            {
                throw new ArgumentNullException(nameof(DbOption));
            }
            _dbConnection = ConnectionFactory.CreateConnection(_dbOption.DbType, _dbOption.ConnectionString);
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
            string sql = "select count(0) from SFCS_PRODUCT_CARTON where id = :id";
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
        public async Task<decimal> SaveDataByTrans(SfcsProductCartonModel model)
        {
            decimal result = 1;
            ConnectionFactory.OpenConnection(_dbConnection);
            using (var tran = _dbConnection.BeginTransaction())
            {
                try
                {
                    //新增
                    string insertSql = @"insert into SFCS_PRODUCT_CARTON 
					(ID,PART_NO,FORMAT,STANDARD_QUANTITY,MAX_QUANTITY,MIN_QUANTITY,STANDARD_WEIGHT,MAX_WEIGHT,MIN_WEIGHT,CUBAGE,LENGTH,WIDTH,HEIGHT,AUTO_CREATE_FLAG,COLLECT_OPERATION_ID,ENABLED) 
					VALUES (:ID,:PART_NO,:FORMAT,:STANDARD_QUANTITY,:MAX_QUANTITY,:MIN_QUANTITY,:STANDARD_WEIGHT,:MAX_WEIGHT,:MIN_WEIGHT,:CUBAGE,:LENGTH,:WIDTH,:HEIGHT,:AUTO_CREATE_FLAG,:COLLECT_OPERATION_ID,:ENABLED)";
                    if (model.InsertRecords != null && model.InsertRecords.Count > 0)
                    {
                        foreach (var item in model.InsertRecords)
                        {
                            var newid = await GetSEQID();
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
                                item.CUBAGE,
                                item.LENGTH,
                                item.WIDTH,
                                item.HEIGHT,
                                item.AUTO_CREATE_FLAG,
                                item.COLLECT_OPERATION_ID,
                                item.ENABLED,

                            }, tran);
                            result = newid;
                        }
                    }
                    //更新
                    string updateSql = @"Update SFCS_PRODUCT_CARTON set PART_NO=:PART_NO,FORMAT=:FORMAT,STANDARD_QUANTITY=:STANDARD_QUANTITY,MAX_QUANTITY=:MAX_QUANTITY,MIN_QUANTITY=:MIN_QUANTITY,STANDARD_WEIGHT=:STANDARD_WEIGHT,MAX_WEIGHT=:MAX_WEIGHT,MIN_WEIGHT=:MIN_WEIGHT,CUBAGE=:CUBAGE,LENGTH=:LENGTH,WIDTH=:WIDTH,HEIGHT=:HEIGHT,AUTO_CREATE_FLAG=:AUTO_CREATE_FLAG,COLLECT_OPERATION_ID=:COLLECT_OPERATION_ID,ENABLED=:ENABLED  
						where ID=:ID ";
                    if (model.UpdateRecords != null && model.UpdateRecords.Count > 0)
                    {
                        foreach (var item in model.UpdateRecords)
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
                                item.CUBAGE,
                                item.LENGTH,
                                item.WIDTH,
                                item.HEIGHT,
                                item.AUTO_CREATE_FLAG,
                                item.COLLECT_OPERATION_ID,
                                item.ENABLED,

                            }, tran);
                        }
                    }
                    //删除
                    string deleteSql = @"Delete from SFCS_PRODUCT_CARTON where ID=:ID ";
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
    }
}