/**
*┌──────────────────────────────────────────────────────────────┐
*│　描    述：呼叫记录处理接口实现                                                    
*│　作    者：嘉志科技                                            
*│　版    本：2.0    模板代码自动生成                                                
*│　创建时间：2019-09-23 16:09:13                             
*└──────────────────────────────────────────────────────────────┘
*┌──────────────────────────────────────────────────────────────┐
*│　命名空间： JZ.IMS.Repository.Oracle                                  
*│　类    名： AndonCallRecordHandleRepository                                      
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
using System.Data;
using JZ.IMS.ViewModels;

namespace JZ.IMS.Repository.Oracle
{
    public class AndonCallRecordHandleRepository : BaseRepository<AndonCallRecordHandle, Decimal>, IAndonCallRecordHandleRepository
    {
        public AndonCallRecordHandleRepository(IOptionsSnapshot<DbOption> options)
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
            string sql = "SELECT ENABLED FROM ANDON_CALL_RECORD_HANDLE WHERE ID=:ID AND IS_DELETE='N'";
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
            string sql = "update ANDON_CALL_RECORD_HANDLE set ENABLED=:ENABLED where  Id=:Id";
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
            string sql = "SELECT ANDON_CALL_RECORD_HANDLE_SEQ.NEXTVAL MY_SEQ FROM DUAL";
            var result = await _dbConnection.ExecuteScalarAsync(sql);
            return (decimal)result;
        }

        /// <summary>
        /// 添加处理记录并更新呼叫主表的状态
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public async Task<BaseResult> InsertAndUpdateSatus(AndonCallRecordHandle model)
        {
            BaseResult result = new BaseResult();
            result.ResultCode = ResultCodeAddMsgKeys.CommonObjectSuccessCode;
            result.ResultMsg = ResultCodeAddMsgKeys.CommonObjectSuccessMsg;
            ConnectionFactory.OpenConnection(_dbConnection);
            IDbTransaction transaction = _dbConnection.BeginTransaction();
            try
            {
                if (await _dbConnection.InsertAsync(model, transaction) > 0)
                {
                    int status = 0;
                    if (model.HANDLE_STATUS == 1)//处理成功
                    {
                        status = 2;
                    }
                    else if (model.HANDLE_STATUS == 2)//处理失败
                    {
                        status = 3;
                    }

                    if (status > 0)
                    {
                        string sql = "UPDATE ANDON_CALL_RECORD SET STATUS = " + status + ", END_TIME = SYSDATE, END_OPERATOR = '" + model.HANDLER + "' WHERE ID = " + model.MST_ID + " AND STATUS = 1";
                        if (Convert.ToInt32(await _dbConnection.ExecuteAsync(sql, transaction)) > 0)
                        {
                            transaction.Commit();
                        }
                        else
                        {
                            result.ResultCode = ResultCodeAddMsgKeys.CommonExceptionCode;
                            result.ResultMsg = "操作失败，记录状态为“处理中”时，才可进行此操作！";
                            transaction.Rollback();
                        }
                    }
                    else
                    {
                        transaction.Commit();
                    }
                }
                else
                {
                    result.ResultCode = ResultCodeAddMsgKeys.CommonExceptionCode;
                    result.ResultMsg = ResultCodeAddMsgKeys.CommonExceptionMsg + "，数据写入失败！";
                    transaction.Rollback();
                }
            }
            catch
            {
                transaction.Rollback(); // 回滚事务
                throw;
            }
            finally
            {
                if (_dbConnection.State != ConnectionState.Closed)
                {
                    _dbConnection.Close();
                }
            }
            return result;
        }


        /// <summary>
        /// 保存数据(行编辑保存异常记录处理)
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public async Task<decimal> SaveDataByTrans(AndonCallRecordHandleAddOrModifyModel model)
        {
            int result = 1;
            ConnectionFactory.OpenConnection(_dbConnection);
            using (var tran = _dbConnection.BeginTransaction())
            {
                try
                {
                    //更新
                    string updateSql = @"Update ANDON_CALL_RECORD_HANDLE set MST_ID=:MST_ID,HANDLER=:HANDLER,HANDLE_TIME=:HANDLE_TIME,HANDLE_CONTENT=:HANDLE_CONTENT,HANDLE_STATUS=:HANDLE_STATUS,TO_USER=:TO_USER,SOLUTION=:SOLUTION  
						where ID=:ID ";
                    if (model != null && model.Id > 0)
                    {
                        var resdata = await _dbConnection.ExecuteAsync(updateSql, new
                        {
                            model.Id,
                            model.MST_ID,
                            model.HANDLER,
                            model.HANDLE_TIME,
                            model.HANDLE_CONTENT,
                            model.HANDLE_STATUS,
                            model.TO_USER,
                            model.SOLUTION,
                        }, tran);
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
        /// 新增异常记录处理数据
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>

        public async Task<decimal> InsertRecordHandle(AndonCallRecordHandleAddOrModifyModel model)
        {
            int result = 1;
            ConnectionFactory.OpenConnection(_dbConnection);
            using (var tran = _dbConnection.BeginTransaction())
            {
                try
                {
                    //新增
                    string insertSql = @"insert into ANDON_CALL_RECORD_HANDLE 
					(ID,MST_ID,HANDLER,HANDLE_TIME,HANDLE_CONTENT,HANDLE_STATUS,TO_USER,SOLUTION) 
					VALUES (:ID,:MST_ID,:HANDLER,:HANDLE_TIME,:HANDLE_CONTENT,:HANDLE_STATUS,:TO_USER,:SOLUTION)";
                    if (model != null)
                    {

                        var newid = await GetSEQID();
                        var resdata = await _dbConnection.ExecuteAsync(insertSql, new
                        {
                            ID=newid,
                            model.MST_ID,
                            model.HANDLER,
                            model.HANDLE_TIME,
                            model.HANDLE_CONTENT,
                            model.HANDLE_STATUS,
                            model.TO_USER,
                            model.SOLUTION,
                        }, tran);

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