/*
*┌──────────────────────────────────────────────────────────────┐
*│　描    述：产前确认主表接口实现                                                    
*│　作    者：嘉志科技                                            
*│　版    本：2.0    模板代码自动生成                                                
*│　创建时间：2020-04-25 09:05:16                             
*└──────────────────────────────────────────────────────────────┘
*┌──────────────────────────────────────────────────────────────┐
*│　命名空间： JZ.IMS.Repository.Oracle                                  
*│　类    名： MesProductionPreMstRepository                                      
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
    public class MesProductionPreMstRepository : BaseRepository<MesProductionPreMst, Decimal>, IMesProductionPreMstRepository
    {
        public MesProductionPreMstRepository(IOptionsSnapshot<DbOption> options)
        {
            _dbOption = options.Get("iWMS");
            if (_dbOption == null)
            {
                throw new ArgumentNullException(nameof(DbOption));
            }
            _dbConnection = ConnectionFactory.CreateConnection(_dbOption.DbType, _dbOption.ConnectionString);
        }

        /// <summary>
        /// 获取表的序列
        /// </summary>
        /// <returns></returns>
		public async Task<decimal> GetSEQID()
        {
            string sql = "SELECT MES_PRODUCTION_PRE_MST_SEQ.NEXTVAL MY_SEQ FROM DUAL";
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
            string sql = "select count(0) from MES_PRODUCTION_PRE_MST where id = :id";
            object result = await _dbConnection.ExecuteScalarAsync(sql, new
            {
                id
            });

            return (Convert.ToInt32(result) > 0);
        }

        /// <summary>
        /// 线体列表
        /// </summary>
        /// <returns></returns>
        public async Task<List<IDNAME>> GetLineList()
        {
            string sql = @"select ID, OPERATION_LINE_NAME NAME from SFCS_OPERATION_LINES 
						   UNION 
						   select ID, LINE_NAME NAME from SMT_LINES ";
            var redata = await _dbConnection.QueryAsync<IDNAME>(sql);
            return redata?.ToList();
        }

        /// <summary>
        /// 明细列表
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<List<MesProductionPreDtlListModel>> GetProductionPreDtlList(decimal id)
        {
            string sql = @"select pd.id, pd.conf_id, pd.mst_id, pd.creator, pd.creatime, pd.result, pd.description, 
							   pc.CONTENT_TYPE, pc.content, pc.Confirm_Content,sp.CHINESE CONTENT_TYPE_Caption
						   from mes_production_pre_dtl pd 
							   inner join MES_PRODUCTION_PRE_CONF pc on pd.conf_id = pc.id
							   inner join SFCS_PARAMETERS sp on pc.content_type = sp.lookup_code and sp.lookup_type = 'PRODUCTION_PRE_TYPE'
						   where pd.mst_id =:id ";
            var redata = await _dbConnection.QueryAsync<MesProductionPreDtlListModel>(sql, new { id });
            return redata?.ToList();
        }

        /// <summary>
        /// 保存数据
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public async Task<decimal> SaveDataByTrans(MesProductionPreMstModel model)
        {
            int result = 1;
            decimal newid = 0;
            ConnectionFactory.OpenConnection(_dbConnection);
            using (var tran = _dbConnection.BeginTransaction())
            {
                try
                {
                    //新增
                    if (model.MainData != null && model.MainData.ID == 0)
                    {
                        string insertMainSql = @"insert into MES_PRODUCTION_PRE_MST 
					        (ID,WO_NO,PART_NO,MODEL_ID,CUSTOMER_ID,PART_NAME,MODEL_NAME,LINE_ID,ORAGE_ID,PRODUCTION_TIME,END_STATUS,MST_NO,CREATOR,CREATIME) 
					        VALUES (:ID,:WO_NO,:PART_NO,:MODEL_ID,:CUSTOMER_ID,:PART_NAME,:MODEL_NAME,:LINE_ID,:ORAGE_ID,:PRODUCTION_TIME,:END_STATUS,:MST_NO,
                            :CREATOR, SYSDate)";
                        newid = await GetSEQID();
                        string new_mst_no = string.Format("PRE{0}", DateTime.Now.ToString("yyyyMMddHHmmssfff"));
                        var resdata = await _dbConnection.ExecuteAsync(insertMainSql, new
                        {
                            ID = newid,
                            model.MainData.WO_NO,
                            model.MainData.PART_NO,
                            model.MainData.MODEL_ID,
                            model.MainData.CUSTOMER_ID,
                            model.MainData.PART_NAME,
                            model.MainData.MODEL_NAME,
                            model.MainData.LINE_ID,
                            model.MainData.ORAGE_ID,
                            model.MainData.PRODUCTION_TIME,
                            model.MainData.END_STATUS,
                            MST_NO = new_mst_no,
                            model.MainData.CREATOR,
                        }, tran);
                    }
                    else
                    {
                        //更新
                        string updateMainSql = @"Update MES_PRODUCTION_PRE_MST set WO_NO=:WO_NO,PART_NO=:PART_NO,MODEL_ID=:MODEL_ID,CUSTOMER_ID=:CUSTOMER_ID,
                            PART_NAME=:PART_NAME,MODEL_NAME=:MODEL_NAME,LINE_ID=:LINE_ID,ORAGE_ID=:ORAGE_ID,PRODUCTION_TIME=:PRODUCTION_TIME,
                            END_STATUS=:END_STATUS,MST_NO=:MST_NO,CREATOR=:CREATOR
						where ID=:ID ";
                        var resdata = await _dbConnection.ExecuteAsync(updateMainSql, new
                        {
                            model.MainData.ID,
                            model.MainData.WO_NO,
                            model.MainData.PART_NO,
                            model.MainData.MODEL_ID,
                            model.MainData.CUSTOMER_ID,
                            model.MainData.PART_NAME,
                            model.MainData.MODEL_NAME,
                            model.MainData.LINE_ID,
                            model.MainData.ORAGE_ID,
                            model.MainData.PRODUCTION_TIME,
                            model.MainData.END_STATUS,
                            model.MainData.MST_NO,
                            model.MainData.CREATOR,
                        }, tran);
                    }
                    //新增明细
                    string insertSql = @"insert into MES_PRODUCTION_PRE_DTL 
					    (ID,CONF_ID,MST_ID,CREATOR,CREATIME,RESULT,DESCRIPTION) 
					    VALUES (MES_PRODUCTION_PRE_DTL_SEQ.NEXTVAL,:CONF_ID,:MST_ID,:CREATOR,SYSDate,:RESULT,:DESCRIPTION)";
                    if (model.InsertRecords != null && model.InsertRecords.Count > 0)
                    {
                        foreach (var item in model.InsertRecords)
                        {
                            var resdata = await _dbConnection.ExecuteAsync(insertSql, new
                            {
                                item.CONF_ID,
                                MST_ID = newid,
                                model.MainData.CREATOR,
                                item.RESULT,
                                item.DESCRIPTION,

                            }, tran);
                        }
                    }
                    //更新明细
                    string updateSql = @"Update MES_PRODUCTION_PRE_DTL set CONF_ID=:CONF_ID,MST_ID=:MST_ID,
                            RESULT=:RESULT,DESCRIPTION=:DESCRIPTION  
						where ID=:ID ";
                    if (model.UpdateRecords != null && model.UpdateRecords.Count > 0)
                    {
                        foreach (var item in model.UpdateRecords)
                        {
                            var resdata = await _dbConnection.ExecuteAsync(updateSql, new
                            {
                                item.ID,
                                item.CONF_ID,
                                item.MST_ID,
                                item.RESULT,
                                item.DESCRIPTION,

                            }, tran);
                        }
                    }
                    //删除明细
                    string deleteSql = @"Delete from MES_PRODUCTION_PRE_DTL where ID=:ID ";
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
        ///删除 
        /// </summary>
        /// <param name="id">主表id</param>
        /// <returns></returns>
        public async Task<int> DeleteByTrans(decimal id)
        {
            int result = 1;
            ConnectionFactory.OpenConnection(_dbConnection);
            using (var tran = _dbConnection.BeginTransaction())
            {
                try
                {
                    string sql = "delete from mes_production_pre_mst where id =:id";
                    result = await _dbConnection.ExecuteAsync(sql, new { id }, tran);
                    if (result > 0)
                    {
                        sql = "delete from mes_production_pre_dtl where mst_id =:mst_id";
                        await _dbConnection.ExecuteAsync(sql, new { mst_id = id }, tran);
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