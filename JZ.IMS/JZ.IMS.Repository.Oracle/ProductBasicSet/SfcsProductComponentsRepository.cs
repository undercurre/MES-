/**
*┌──────────────────────────────────────────────────────────────┐
*│　描    述：接口实现                                                    
*│　作    者：嘉志科技                                            
*│　版    本：2.0    模板代码自动生成                                                
*│　创建时间：2020-04-02 10:47:11                             
*└──────────────────────────────────────────────────────────────┘
*┌──────────────────────────────────────────────────────────────┐
*│　命名空间： JZ.IMS.Repository.Oracle                                  
*│　类    名： SfcsProductComponentsRepository                                      
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
    public class SfcsProductComponentsRepository : BaseRepository<SfcsProductComponents, Decimal>, ISfcsProductComponentsRepository
    {
        public SfcsProductComponentsRepository(IOptionsSnapshot<DbOption> options)
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
            string sql = "SELECT ENABLED FROM SFCS_PRODUCT_COMPONENTS WHERE ID=:ID";
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
            string sql = "UPDATE SFCS_PRODUCT_COMPONENTS set ENABLED=:ENABLED WHERE ID=:Id";
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
            string sql = "SELECT SFCS_PRODUCT_COMPONENTS_SEQ.NEXTVAL MY_SEQ FROM DUAL";
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
            string sql = "select count(0) from SFCS_PRODUCT_COMPONENTS where id = :id";
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
        public async Task<decimal> SaveDataByTrans(SfcsProductComponentsModel model)
        {
            int result = 1;
            ConnectionFactory.OpenConnection(_dbConnection);
            using (var tran = _dbConnection.BeginTransaction())
            {
                try
                {
                    #region 零件
                    //新增
                    string insertcomponentsSql = @"insert into SFCS_PRODUCT_COMPONENTS 
					(ID,PART_NO,COMPONENT_ID,ODM_COMPONENT_PN,CUSTOMER_COMPONENT_PN,DATA_FORMAT,COMPONENT_QTY,SERIALIZED,ENABLED,REWORK_REMOVE_FLAG,EDI_FLAG,DEVICE_FLAG,STANDARD_USE_COUNT,CHECK_DEFECT_FLAG,COMPONENT_LOCATIONS,COLLECT_OPERATION_ID,ATTRIBUTE1,ATTRIBUTE2) 
					VALUES (:ID,:PART_NO,:COMPONENT_ID,:ODM_COMPONENT_PN,:CUSTOMER_COMPONENT_PN,:DATA_FORMAT,:COMPONENT_QTY,:SERIALIZED,:ENABLED,:REWORK_REMOVE_FLAG,:EDI_FLAG,:DEVICE_FLAG,:STANDARD_USE_COUNT,:CHECK_DEFECT_FLAG,:COMPONENT_LOCATIONS,:COLLECT_OPERATION_ID,:ATTRIBUTE1,:ATTRIBUTE2)";
                    if (model.InsertComponents != null && model.InsertComponents.Count > 0)
                    {
                        foreach (var item in model.InsertComponents)
                        {
                            var newid = await Get_MES_SEQ_ID();
                            var resdata = await _dbConnection.ExecuteAsync(insertcomponentsSql, new
                            {
                                ID= newid,
                                item.PART_NO,
                                item.COMPONENT_ID,
                                item.ODM_COMPONENT_PN,
                                item.CUSTOMER_COMPONENT_PN,
                                item.DATA_FORMAT,
                                item.COMPONENT_QTY,
                                item.SERIALIZED,
                                item.ENABLED,
                                item.REWORK_REMOVE_FLAG,
                                item.EDI_FLAG,
                                item.DEVICE_FLAG,
                                item.STANDARD_USE_COUNT,
                                item.CHECK_DEFECT_FLAG,
                                item.COMPONENT_LOCATIONS,
                                item.COLLECT_OPERATION_ID,
                                item.ATTRIBUTE1,
                                item.ATTRIBUTE2

                            }, tran);
                        }
                    }
                    //更新
                    string updatecomponentsSql = @"Update SFCS_PRODUCT_COMPONENTS 
                                                    set PART_NO=:PART_NO,COMPONENT_ID=:COMPONENT_ID,ODM_COMPONENT_PN=:ODM_COMPONENT_PN,CUSTOMER_COMPONENT_PN=:CUSTOMER_COMPONENT_PN,DATA_FORMAT=:DATA_FORMAT,COMPONENT_QTY=:COMPONENT_QTY,SERIALIZED=:SERIALIZED,ENABLED=:ENABLED,REWORK_REMOVE_FLAG=:REWORK_REMOVE_FLAG,EDI_FLAG=:EDI_FLAG,DEVICE_FLAG=:DEVICE_FLAG,STANDARD_USE_COUNT=:STANDARD_USE_COUNT,CHECK_DEFECT_FLAG=:CHECK_DEFECT_FLAG,COMPONENT_LOCATIONS=:COMPONENT_LOCATIONS,COLLECT_OPERATION_ID=:COLLECT_OPERATION_ID,ATTRIBUTE1=:ATTRIBUTE1,ATTRIBUTE2=:ATTRIBUTE2  
						where ID=:ID ";
                    if (model.UpdateComponents != null && model.UpdateComponents.Count > 0)
                    {
                        foreach (var item in model.UpdateComponents)
                        {
                            var resdata = await _dbConnection.ExecuteAsync(updatecomponentsSql, new
                            {
                                item.ID,
                                item.PART_NO,
                                item.COMPONENT_ID,
                                item.ODM_COMPONENT_PN,
                                item.CUSTOMER_COMPONENT_PN,
                                item.DATA_FORMAT,
                                item.COMPONENT_QTY,
                                item.SERIALIZED,
                                item.ENABLED,
                                item.REWORK_REMOVE_FLAG,
                                item.EDI_FLAG,
                                item.DEVICE_FLAG,
                                item.STANDARD_USE_COUNT,
                                item.CHECK_DEFECT_FLAG,
                                item.COMPONENT_LOCATIONS,
                                item.COLLECT_OPERATION_ID,
                                item.ATTRIBUTE1,
                                item.ATTRIBUTE2
                            }, tran);
                        }
                    }

                    #region 删除
                    string deleteSql = @"Delete from SFCS_PRODUCT_COMPONENTS where ID=:ID ";
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
                    #endregion

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

                    #endregion

                    #region 替代料维护
                    //新增
                    string insertsubstituteSql = @" INSERT INTO SFCS_SUBSTITUTE_COMPONENTS
													(ID,PRODUCT_COMPONENT_ID, SUBSTITUTE_COMP_PN, CUSTOMER_COMPONENT_PN, DATA_FORMAT, COMPONENT_QTY, BEGIN_DATE, END_DATE, SERIALIZED, REWORK_REMOVE_FLAG, EDI_FLAG, ENABLED,  DEVICE_FLAG, STANDARD_USE_COUNT,ATTRIBUTE2) 
												    VALUES(:ID, :PRODUCT_COMPONENT_ID, :SUBSTITUTE_COMP_PN, :CUSTOMER_COMPONENT_PN, :DATA_FORMAT, :COMPONENT_QTY, :BEGIN_DATE, :END_DATE, :SERIALIZED, :REWORK_REMOVE_FLAG, :EDI_FLAG, :ENABLED,:DEVICE_FLAG, :STANDARD_USE_COUNT,:ATTRIBUTE2) ";
                    if (model.InsertSubstitute != null && model.InsertSubstitute.Count > 0)
                    {
                        foreach (var item in model.InsertSubstitute)
                        {
                            var newid = await Get_MES_SEQ_ID();
                            if (item.BEGIN_DATE == null || item.BEGIN_DATE.ToString().IsNullOrWhiteSpace())
                            {
                                item.BEGIN_DATE = DateTime.Now;
                            }
                            if (item.END_DATE == null || item.END_DATE.ToString().IsNullOrWhiteSpace())
                            {
                                item.END_DATE = DateTime.Now;
                            }
                            var resdata = await _dbConnection.ExecuteAsync(insertsubstituteSql, new
                            {
                                ID= newid,
                                item.PRODUCT_COMPONENT_ID,
                                item.SUBSTITUTE_COMP_PN,
                                item.CUSTOMER_COMPONENT_PN,
                                item.DATA_FORMAT,
                                item.COMPONENT_QTY,
                                item.BEGIN_DATE,
                                item.END_DATE,
                                item.SERIALIZED,
                                item.REWORK_REMOVE_FLAG,
                                item.EDI_FLAG,
                                item.ENABLED,
                                item.DEVICE_FLAG,
                                item.STANDARD_USE_COUNT,
                                item.ATTRIBUTE2
                            },tran);
                        }
                    }
                    string updatesubstituteSql = @" UPDATE SFCS_SUBSTITUTE_COMPONENTS
													SET 
													SUBSTITUTE_COMP_PN = :SUBSTITUTE_COMP_PN,
													CUSTOMER_COMPONENT_PN = :CUSTOMER_COMPONENT_PN,
													DATA_FORMAT = :DATA_FORMAT,
													COMPONENT_QTY = :COMPONENT_QTY,
													BEGIN_DATE = :BEGIN_DATE,
													END_DATE = :END_DATE,
													SERIALIZED = :SERIALIZED,
													REWORK_REMOVE_FLAG = :REWORK_REMOVE_FLAG,
													EDI_FLAG = :EDI_FLAG,
													ENABLED = :ENABLED,
													DEVICE_FLAG = :DEVICE_FLAG,
													STANDARD_USE_COUNT = :STANDARD_USE_COUNT ,
                                                    ATTRIBUTE2=:ATTRIBUTE2
                                                    WHERE ID=:ID ";
                    if (model.UpdateSubstitute != null && model.UpdateSubstitute.Count > 0)
                    {
                        foreach (var item in model.UpdateSubstitute)
                        {
                            var resdata = await _dbConnection.ExecuteAsync(updatesubstituteSql, new
                            {
                                item.ID,
                                item.SUBSTITUTE_COMP_PN,
                                item.CUSTOMER_COMPONENT_PN,
                                item.DATA_FORMAT,
                                item.COMPONENT_QTY,
                                item.BEGIN_DATE,
                                item.END_DATE,
                                item.SERIALIZED,
                                item.REWORK_REMOVE_FLAG,
                                item.EDI_FLAG,
                                item.ENABLED,
                                item.DEVICE_FLAG,
                                item.STANDARD_USE_COUNT,
                                item.ATTRIBUTE2
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