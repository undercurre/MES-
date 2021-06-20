/*
*┌──────────────────────────────────────────────────────────────┐
*│　描    述：接口实现                                                    
*│　作    者：嘉志科技                                            
*│　版    本：2.0    模板代码自动生成                                                
*│　创建时间：2020-04-22 09:40:14                             
*└──────────────────────────────────────────────────────────────┘
*┌──────────────────────────────────────────────────────────────┐
*│　命名空间： JZ.IMS.Repository.Oracle                                  
*│　类    名： SfcsHoldProductHeaderRepository                                      
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
    public class SfcsHoldProductHeaderRepository : BaseRepository<SfcsHoldProductHeader, Decimal>, ISfcsHoldProductHeaderRepository
    {
        public SfcsHoldProductHeaderRepository(IOptionsSnapshot<DbOption> options)
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
            string sql = "SELECT SFCS_HOLD_PRODUCT_HEADER_SEQ.NEXTVAL MY_SEQ FROM DUAL";
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
            string sql = "select count(0) from SFCS_HOLD_PRODUCT_HEADER where id = :id";
            object result = await _dbConnection.ExecuteScalarAsync(sql, new
            {
                id
            });

            return (Convert.ToInt32(result) > 0);
        }

        /// <summary>
        /// 產品流水號 前鎖定的數據
        /// </summary>
        /// <param name="sn"></param>
        /// <returns></returns>
        public async Task<List<SfcsHoldProductDetailVListModel>> GetHoldDetailViewAddByOldSN(string sn)
        {
            string sql = @"SELECT SHPD.* FROM SFCS_HOLD_PRODUCT_DETAIL_V SHPD
                     Where HOLD_NUMBER NOT LIKE 'SL%' AND SN =:SN AND STATUS='Y' 
                     UNION
                     SELECT SHPDV.* FROM SFCS_RUNCARD_REPLACE SRR,SFCS_HOLD_PRODUCT_DETAIL_V SHPDV
                     WHERE SRR.SN_ID = SHPDV.SN_ID AND SN =:SN AND STATUS='Y' ";
            var redata = await _dbConnection.QueryAsync<SfcsHoldProductDetailVListModel>(sql, new { SN = sn});
            return redata?.ToList();
        }

        /// <summary>
        /// 保存数据
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public async Task<decimal> SaveDataByTrans(UnLockBillSaveModel model)
        {
            int result = 1;
            ConnectionFactory.OpenConnection(_dbConnection);
            using (var tran = _dbConnection.BeginTransaction())
            {
                try
                {
                    //更新主表
                    string updateHeader = @"UPDATE SFCS_HOLD_PRODUCT_HEADER SET RELEASE_CAUSE=:RELEASE_CAUSE, STATUS='N',
                                                RELEASE_EMPNO=:RELEASE_EMPNO, RELEASE_TIME=SYSDATE 
                                           WHERE HOLD_NUMBER=:HOLD_NUMBER ";
                    var resdata = await _dbConnection.ExecuteAsync(updateHeader, new
                    {
                        model.RELEASE_CAUSE,
                        RELEASE_EMPNO = model.User_Name,
                        model.HOLD_NUMBER,
                    }, tran);

                    //更新子表
                    string updateDetail = @"UPDATE SFCS_HOLD_PRODUCT_DETAIL SET STATUS='N', RELEASE_CAUSE = :RELEASE_CAUSE,
                                                RELEASE_EMPNO = :RELEASE_EMPNO, RELEASE_TIME=SYSDATE 
                                            WHERE HOLD_ID=:HOLD_ID AND STATUS='Y' ";
                    resdata = await _dbConnection.ExecuteAsync(updateDetail, new
                    {
                        model.RELEASE_CAUSE,
                        RELEASE_EMPNO = model.User_Name,
                        HOLD_ID = model.ID,
                    }, tran);
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
        /// 解锁产品之解除产品锁定
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public async Task<UnLockProductSaveReturn> UnLockProductSave(UnLockProductSaveModel model)
        {
            var returnVM = new UnLockProductSaveReturn();
            returnVM.ResultMsgList = new List<string>();
            returnVM.Result = true;
            const int ReleaseThroughComponentSN = 1;
            string operationHistory = string.Empty;
            ConnectionFactory.OpenConnection(_dbConnection);
            using (var tran = _dbConnection.BeginTransaction())
            {
                try
                {
                    SortedList<decimal, string> headerList = new SortedList<decimal, string>();

                    foreach (var item in model.HoldProductList)
                    {
                        decimal holdProductDetailID = item.ID;
                        decimal holdID = item.HOLD_ID;
                        string serialNumber = null;
                        serialNumber = (model.OperationType == ReleaseThroughComponentSN) ? item.COMPONENT_SN : item.SN;
                        var headerRow = await GetAsync(holdID);

                        string updateDetail = @"UPDATE SFCS_HOLD_PRODUCT_DETAIL SET STATUS='N' , RELEASE_CAUSE = :RELEASE_CAUSE, 
                                                    RELEASE_EMPNO = :RELEASE_EMPNO, RELEASE_TIME=SYSDATE 
                                                WHERE ID=:ID ";
                        var resdata = await _dbConnection.ExecuteAsync(updateDetail, new
                        {
                            model.RELEASE_CAUSE,
                            RELEASE_EMPNO = model.User_Name,
                            ID = holdProductDetailID,
                        }, tran);

                        if (headerList.Keys.IndexOf(holdID) < 0)
                        {
                            headerList.Add(holdID, headerRow.HOLD_NUMBER);
                        }

                        operationHistory = string.Format("序号{0}解锁成功", serialNumber);
                        returnVM.ResultMsgList.Add(operationHistory);
                    }

                    string conditions = string.Empty;
                    // 更新對應的Header狀態
                    for (int i = 0; i < headerList.Count; i++)
                    {
                        conditions = "Where HOLD_ID=:HOLD_ID AND STATUS='Y'";
                        var detailData = (await GetListAsyncEx<SfcsHoldProductDetail>(conditions, new { HOLD_ID = headerList.Keys[i] })).ToList();

                        // 如果沒有鎖定記錄，解除單據Header的鎖定
                        if (detailData?.Count == 0)
                        {
                            string updateHeader = @"UPDATE SFCS_HOLD_PRODUCT_HEADER SET RELEASE_CAUSE =:RELEASE_CAUSE, STATUS='N',
                                                      RELEASE_EMPNO =:RELEASE_EMPNO, RELEASE_TIME=SYSDATE 
                                                    WHERE HOLD_NUMBER=:HOLD_NUMBER ";
                            await _dbConnection.ExecuteAsync(updateHeader, new
                            {
                                HOLD_NUMBER = headerList.Values[i],
                                RELEASE_CAUSE = "单据中所含明细项已全部解除锁定，系统自动解除单据的锁定状态",
                                RELEASE_EMPNO = "SFCS",
                            }, tran);
                        }
                    }

                    tran.Commit();
                }
                catch (Exception ex)
                {
                    returnVM.Result = false;
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
            return returnVM;
        }

    

    }
}