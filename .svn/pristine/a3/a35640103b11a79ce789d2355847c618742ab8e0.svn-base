/**
*┌──────────────────────────────────────────────────────────────┐
*│　描    述：接口实现                                                    
*│　作    者：嘉志科技                                            
*│　版    本：2.0    模板代码自动生成                                                
*│　创建时间：2020-03-10 18:09:05                             
*└──────────────────────────────────────────────────────────────┘
*┌──────────────────────────────────────────────────────────────┐
*│　命名空间： JZ.IMS.Repository.Oracle                                  
*│　类    名： SmtStencilStoreRepository                                      
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
using JZ.IMS.Core.Extensions;
using System.Text;
using System.Linq;

namespace JZ.IMS.Repository.Oracle
{
    public class SmtStencilStoreRepository : BaseRepository<SmtStencilStore, Decimal>, ISmtStencilStoreRepository
    {
        public SmtStencilStoreRepository(IOptionsSnapshot<DbOption> options)
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
            string sql = "SELECT SMT_STENCIL_STORE_SEQ.NEXTVAL MY_SEQ FROM DUAL";
            var result = await _dbConnection.ExecuteScalarAsync(sql);
            return (decimal)result;
        }

        /// <summary>
        /// 根据条件获取列表数量
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public async Task<IEnumerable<SmtStencilStoreListModel>> Loadata(SmtStencilStoreRequestModel model)
        {
            string whereStr = GetWhereStr(model);

            StringBuilder sql = new StringBuilder();
            sql.Append("select * from (");
            sql.Append(@"SELECT ROWNUM AS rowno, SC.STENCIL_NO, SS.ID, SS.STENCIL_ID, SS.LOCATION, SS.PCB_PART_NO, SS.PCB_REVISION,
						   P.CN_DESC STATUS, SS.MANUFACTURE_TIME, SS.REMARK, SS.CREATE_TIME,
						   SS.UPDATE_TIME
					FROM SMT_STENCIL_STORE SS inner join SMT_STENCIL_CONFIG SC on SC.ID = SS.STENCIL_ID 
						 inner join SMT_LOOKUP P ON P.CODE = SS.STATUS AND P.TYPE = 'STENCIL_STORE_STATUS' ");
            sql.Append(whereStr.ToString());
            sql.Append(" order by  SS.ID desc");
            sql.Append(") tt where rowno BETWEEN (:Page-1)*:Limit+1 AND :Limit*:Page ");

            return await _dbConnection.QueryAsync<SmtStencilStoreListModel>(sql.ToString(), model);
        }

        /// <summary>
        /// 根据条件获取总记录数
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public async Task<int> GetTotalCount(SmtStencilStoreRequestModel model)
        {
            string whereStr = GetWhereStr(model);
            StringBuilder sql = new StringBuilder();
            sql.Append(@"select count(0) FROM SMT_STENCIL_STORE SS inner join SMT_STENCIL_CONFIG SC on SC.ID = SS.STENCIL_ID 
                         inner join SMT_LOOKUP P ON P.CODE = SS.STATUS AND P.TYPE = 'STENCIL_STORE_STATUS' ");
            sql.Append(whereStr.ToString());

            return await _dbConnection.ExecuteScalarAsync<int>(sql.ToString(), model);
        }

        /// <summary>
        /// 获取WHERE条件
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        private string GetWhereStr(SmtStencilStoreRequestModel model)
        {
            StringBuilder whereStr = new StringBuilder();
            whereStr.Append(" where 1=1 ");

            if (!string.IsNullOrEmpty(model.STENCIL_NO))
                whereStr.Append(" and instr(Upper(SC.STENCIL_NO),Upper(:STENCIL_NO))>0 ");

            if (!string.IsNullOrEmpty(model.LOCATION))
                whereStr.Append(" and Upper(SS.LOCATION) =Upper(:LOCATION) ");

            return whereStr.ToString();
        }

        /// <summary>
        ///获取钢网注册信息 
        /// </summary>
        /// <param name="id">项目id</param>
        /// <returns></returns>
        public async Task<SmtStencilConfig> GetStencil(string stencil_no)
        {
            string sql = "select * from SMT_STENCIL_CONFIG where STENCIL_NO = :STENCIL_NO";
            var result = await _dbConnection.QueryAsync<SmtStencilConfig>(sql, new
            {
                STENCIL_NO = stencil_no
            });
            return result?.FirstOrDefault();
        }

        /// <summary>
        ///获取可用钢网注册信息 
        /// </summary>
        /// <param name="id">项目id</param>
        /// <returns></returns>
        public async Task<SmtStencilConfig> GetStencil2Enabled(string stencil_no)
        {
            string sql = "select * from SMT_STENCIL_CONFIG where ENABLED= 'Y' AND STENCIL_NO = :STENCIL_NO";
            var result = await _dbConnection.QueryAsync<SmtStencilConfig>(sql, new
            {
                STENCIL_NO = stencil_no
            });
            return result?.FirstOrDefault();
        }

        /// <summary>
        ///获取钢网运行时间 
        /// </summary>
        /// <param name="id">项目id</param>
        /// <returns></returns>
        public async Task<SmtStencilRuntime> GetStencilRuntime(string stencil_no)
        {
            string sql = "SELECT * FROM SMT_STENCIL_RUNTIME where STENCIL_NO = :STENCIL_NO";
            var result = await _dbConnection.QueryAsync<SmtStencilRuntime>(sql, new
            {
                STENCIL_NO = stencil_no
            });
            return result?.FirstOrDefault();
        }

        /// <summary>
        /// 获取钢网清洗记录
        /// </summary>
        /// <param name="stencil_no"></param>
        /// <returns></returns>
        public async Task<SmtStencilCleanHistory> GetStencilCleanHistory(string stencil_no)
        {
            string sql = "SELECT * FROM SMT_STENCIL_CLEAN_HISTORY where STENCIL_NO = :STENCIL_NO";
            var result = await _dbConnection.QueryAsync<SmtStencilCleanHistory>(sql, new
            {
                STENCIL_NO = stencil_no
            });
            return result?.FirstOrDefault();
        }

        /// <summary>
        /// 获取钢网清洗记录列表
        /// </summary>
        /// <param name="stencil_no"></param>
        /// <returns></returns>
        public async Task<List<SmtStencilCleanHistory>> GetStencilCleanHistoryList(string stencil_no)
        {
            string sql = "SELECT * FROM SMT_STENCIL_CLEAN_HISTORY where STENCIL_NO = :STENCIL_NO";
            var result = await _dbConnection.QueryAsync<SmtStencilCleanHistory>(sql, new
            {
                STENCIL_NO = stencil_no
            });

            return result?.ToList();
        }

        /// <summary>
        ///获取钢网注册信息 byID
        /// </summary>
        /// <param name="ID">项目id</param>
        /// <returns></returns>
        public async Task<SmtStencilConfig> GetStencilByID(decimal ID)
        {
            return await _dbConnection.GetAsync<SmtStencilConfig>(ID);

            //string sql = "select * from SMT_STENCIL_CONFIG where ID = :ID";
            //var result = await _dbConnection.QueryAsync<SmtStencilConfig>(sql, new
            //{
            //	ID
            //});
            //return result?.FirstOrDefault();
        }

        /// <summary>
        ///获取钢网存储信息
        /// </summary>
        /// <param name="id">项目id</param>
        /// <returns></returns>
        public async Task<SmtStencilStore> GetLocationInfo(string location)
        {
            string sql = "SELECT * FROM SMT_STENCIL_STORE WHERE LOCATION = :LOCATION ";
            var result = await _dbConnection.QueryAsync<SmtStencilStore>(sql, new
            {
                LOCATION = location
            });
            return result?.FirstOrDefault();
        }

        /// <summary>
        ///获取用户信息
        /// </summary>
        /// <param name="id">用户账号</param>
        /// <returns></returns>
        public async Task<Sys_Manager> GetSysManager(string User_Name)
        {
            string sql = "SELECT User_Name FROM SYS_MANAGER where User_Name =:User_Name ";
            var result = await _dbConnection.QueryAsync<Sys_Manager>(sql, new
            {
                User_Name
            });
            return result?.FirstOrDefault();
        }


        /// <summary>
        ///获取钢网存储信息By STENCIL_ID 
        /// </summary>
        /// <param name="id">项目id</param>
        /// <returns></returns>
        public async Task<SmtStencilStore> GetStencilStoreBySTENCIL_ID(decimal STENCIL_ID)
        {
            string sql = "SELECT * FROM SMT_STENCIL_STORE WHERE STENCIL_ID = :STENCIL_ID ";
            var result = await _dbConnection.QueryAsync<SmtStencilStore>(sql, new
            {
                STENCIL_ID
            });
            return result?.FirstOrDefault();
        }

        /// <summary>
        /// 判斷網板是否有清洗過
        /// </summary>
        /// <param name="stencilNo"></param>
        /// <returns></returns>
        public async Task<bool> CheckStencilCleaned(string stencilNo)
        {
            bool redata = false;
            string sql = "SELECT count(0) FROM SMT_STENCIL_CLEAN_HISTORY WHERE STENCIL_NO = :STENCIL_NO ORDER BY STENCIL_NO,CLEAN_TIME";
            var result = await _dbConnection.ExecuteScalarAsync(sql, new
            {
                STENCIL_NO = stencilNo
            });
            redata = (Convert.ToInt32(result) > 0);
            if (!redata)
            {
                sql = @"SELECT COUNT(0) FROM SMT_STENCIL_OPERATION_HISTORY H, SMT_STENCIL_CONFIG S 
                        WHERE S.ID = H.STENCIL_ID AND S.STENCIL_NO =:STENCIL_NO ";
                result = await _dbConnection.ExecuteScalarAsync(sql, new
                {
                    STENCIL_NO = stencilNo
                });
                redata = !(Convert.ToInt32(result) > 0);
            }
            return redata;
        }

        /// <summary>
        /// 如果發現該儲位對應的網板信息已經被刪除,則自動清除儲位信息
        /// </summary>
        /// <param name="userName"></param>
        /// <param name="location"></param>
        /// <param name="STENCIL_ID"></param>
        /// <returns></returns>
        public async Task<decimal> DeleteStencilLocation(string userName, string location, decimal STENCIL_ID)
        {
            int result = 1;
            ConnectionFactory.OpenConnection(_dbConnection);
            using (var tran = _dbConnection.BeginTransaction())
            {
                try
                {
                    //新增操作历史
                    string insertSql = @"INSERT INTO SMT_STENCIL_OPERATION_HISTORY
                          (STENCIL_ID, STATUS, OPERATION_TYPE, OPERATION_BY, OPERATION_TIME, CREATE_BY, LOCATION)
						  VALUES (:STENCIL_ID, :STATUS, :OPERATION_TYPE, :OPERATION_BY, SYSDATE, :CREATE_BY, :LOCATION)";
                    var resdata = await _dbConnection.ExecuteAsync(insertSql, new
                    {
                        STENCIL_ID,
                        STATUS = GlobalVariables.StencilScarpStore,
                        OPERATION_TYPE = GlobalVariables.STENCIL_SCRAP_STORE,
                        OPERATION_BY = userName,
                        CREATE_BY = userName,
                        LOCATION = location,
                    }, tran);

                    //删除
                    if (resdata > 0)
                    {
                        string deleteSql = "DELETE FROM SMT_STENCIL_STORE WHERE STENCIL_ID = :STENCIL_ID";
                        resdata = await _dbConnection.ExecuteAsync(deleteSql, new
                        {
                            STENCIL_ID
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
        /// 保存数据
        /// </summary>
        /// <param name="model"></param>
        /// <param name="stencil_id"></param>
        /// <returns></returns>
        public async Task<decimal> SaveDataByTrans(SmtStencilStoreModel model, decimal stencil_id)
        {
            int result = 1;
            ConnectionFactory.OpenConnection(_dbConnection);
            using (var tran = _dbConnection.BeginTransaction())
            {
                try
                {
                    var tmpdata = await GetStencilStoreBySTENCIL_ID(stencil_id);
                    if (tmpdata == null)
                    {
                        decimal newid = await GetSEQID();
                        //插入新紀錄
                        string insertSql = @"INSERT INTO SMT_STENCIL_STORE(ID, STENCIL_ID, LOCATION, STATUS, MANUFACTURE_TIME, CREATE_TIME, REMARK)
                          VALUES (:ID, :STENCIL_ID, :LOCATION, :STATUS, :MANUFACTURE_TIME, SYSDATE, :REMARK)";
                        var resdata = await _dbConnection.ExecuteAsync(insertSql, new
                        {
                            ID = newid,
                            STENCIL_ID = stencil_id,
                            STATUS = GlobalVariables.StencilStored,
                            model.LOCATION,
                            GlobalVariables.StencilStored,
                            model.MANUFACTURE_TIME,
                            model.REMARK,
                        }, tran);

                        insertSql = @"INSERT INTO SMT_STENCIL_OPERATION_HISTORY
                          (STENCIL_ID, STATUS, OPERATION_TYPE, OPERATION_BY, OPERATION_TIME, CREATE_BY, LOCATION)
                          VALUES (:STENCIL_ID, :STATUS, :OPERATION_TYPE, :OPERATION_BY, SYSDATE, :CREATE_BY, :LOCATION)";
                        resdata = await _dbConnection.ExecuteAsync(insertSql, new
                        {
                            STENCIL_ID = stencil_id,
                            STATUS = GlobalVariables.StencilStored,
                            OPERATION_TYPE = GlobalVariables.STORE_STENCIL,
                            OPERATION_BY = model.UserName,
                            CREATE_BY = model.UserName,
                            model.LOCATION,
                        }, tran);
                    }
                    else if (tmpdata.STATUS != GlobalVariables.StencilTaken)
                    {
                        //更新記錄
                        string updateSql = @"UPDATE SMT_STENCIL_STORE
                                             SET LOCATION = :LOCATION,
                                                 STATUS = :STATUS,
                                                 MANUFACTURE_TIME = :MANUFACTURE_TIME,
                                                 REMARK = :REMARK,
                                                 UPDATE_TIME = SYSDATE
                                           WHERE STENCIL_ID = :STENCIL_ID";
                        await _dbConnection.ExecuteAsync(updateSql, new
                        {
                            STENCIL_ID = stencil_id,
                            model.LOCATION,
                            tmpdata.STATUS,
                            model.MANUFACTURE_TIME,
                            model.REMARK,
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
        /// 报废出柜
        /// </summary>
        /// <param name="stencil_id">钢网ID</param>
        /// <param name="location">钢网储位</param>
        /// <param name="userName">操作人</param>
        /// <returns></returns>
        public async Task<decimal> ScrapStencilStore(decimal stencil_id, string location, string userName)
        {
            int result = 1;
            ConnectionFactory.OpenConnection(_dbConnection);
            using (var tran = _dbConnection.BeginTransaction())
            {
                try
                {
                    //插入操作历史
                    string insertSql = @"INSERT INTO SMT_STENCIL_OPERATION_HISTORY
                          (STENCIL_ID, STATUS, OPERATION_TYPE, OPERATION_BY, OPERATION_TIME, CREATE_BY, LOCATION)
                          VALUES (:STENCIL_ID, :STATUS, :OPERATION_TYPE, :OPERATION_BY, SYSDATE, :CREATE_BY, :LOCATION)";
                    var resdata = await _dbConnection.ExecuteAsync(insertSql, new
                    {
                        STENCIL_ID = stencil_id,
                        STATUS = GlobalVariables.StencilScarpStore,
                        OPERATION_TYPE = GlobalVariables.STENCIL_SCRAP_STORE,
                        OPERATION_BY = userName,
                        CREATE_BY = userName,
                        LOCATION = location,
                    }, tran);

                    //删除記錄
                    string updateSql = @"DELETE FROM SMT_STENCIL_STORE WHERE STENCIL_ID = :STENCIL_ID";
                    await _dbConnection.ExecuteAsync(updateSql, new
                    {
                        STENCIL_ID = stencil_id
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
        /// 变更储位保存
        /// </summary>
        /// <param name="stencil_id">钢网ID</param>
        /// <param name="new_location">新钢网储位</param>
        /// <param name="new_location">旧钢网储位</param>
        /// <param name="userName">操作人</param>
        /// <returns></returns>
        public async Task<decimal> ChangeLocationSave(decimal stencil_id, string new_location, string location, string userName)
        {
            int result = 1;
            ConnectionFactory.OpenConnection(_dbConnection);
            using (var tran = _dbConnection.BeginTransaction())
            {
                try
                {
                    //更新
                    string updateSql = @"UPDATE SMT_STENCIL_STORE SET LOCATION = :LOCATION, UPDATE_TIME = SYSDATE
                                         WHERE STENCIL_ID = :STENCIL_ID";
                    await _dbConnection.ExecuteAsync(updateSql, new
                    {
                        STENCIL_ID = stencil_id,
                        LOCATION = new_location
                    }, tran);

                    //插入操作历史
                    string insertSql = @"INSERT INTO SMT_STENCIL_OPERATION_HISTORY
                          (STENCIL_ID, STATUS, OPERATION_TYPE, OPERATION_BY, OPERATION_TIME, CREATE_BY, LOCATION)
                           VALUES (:STENCIL_ID, :STATUS, :OPERATION_TYPE, :OPERATION_BY, SYSDATE, :CREATE_BY, :LOCATION)";
                    var resdata = await _dbConnection.ExecuteAsync(insertSql, new
                    {
                        STENCIL_ID = stencil_id,
                        STATUS = GlobalVariables.StencilStored,
                        OPERATION_TYPE = GlobalVariables.CHANGE_LOCATION,
                        OPERATION_BY = userName,
                        CREATE_BY = userName,
                        LOCATION = location,
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

        // <summary>
        /// 获取表CleanCleanHistory的序列
        /// </summary>
        /// <returns></returns>
		public async Task<decimal> GetCleanCleanHistorySEQID()
        {
            string sql = "SELECT SMT_STENCIL_CLEAN_HIS_SEQ.NEXTVAL MY_SEQ FROM DUAL";
            var result = await _dbConnection.ExecuteScalarAsync(sql);
            return (decimal)result;
        }

        /// <summary>
        /// 钢网领用保存
        /// </summary>
        /// <param name="model"></param>
        /// <param name="stencilStore"></param>
        /// <returns></returns>
        public async Task<decimal> StencilTakeSave(SmtStencilTakeSaveModel model, CheckScrapResult stencilStore)
        {
            int result = 1;
            ConnectionFactory.OpenConnection(_dbConnection);
            using (var tran = _dbConnection.BeginTransaction())
            {
                try
                {
                    //記錄網板清潔數據
                    decimal newid = await GetCleanCleanHistorySEQID();
                    string insertStencilCleanHistory = @"INSERT INTO SMT_STENCIL_CLEAN_HISTORY(ID, STENCIL_NO, OPERATION_LINE_NAME,
                                PRINT_COUNT, CLEAN_USER, INSPECT_USER, INSPECT_RESULT, CLEAN_TIME, TENSION_A, TENSION_B, TENSION_C, TENSION_D, TENSION_E, STENCIL_TYPE)
                                VALUES (:ID, :STENCIL_NO, :OPERATION_LINE_NAME, :PRINT_COUNT, :CLEAN_USER,
                                :INSPECT_USER, :INSPECT_RESULT, SYSDATE, :TENSION_A, :TENSION_B, :TENSION_C, :TENSION_D, :TENSION_E, :STENCIL_TYPE) ";
                    var resdata = await _dbConnection.ExecuteAsync(insertStencilCleanHistory, new
                    {
                        ID = newid,
                        model.STENCIL_NO,
                        OPERATION_LINE_NAME = string.Empty,
                        PRINT_COUNT = model.PrintCount,
                        CLEAN_USER = model.WorkNo,
                        INSPECT_USER = model.WorkNo,
                        INSPECT_RESULT = model.InspectResult,
                        model.TENSION_A,
                        model.TENSION_B,
                        model.TENSION_C,
                        model.TENSION_D,
                        model.TENSION_E,
                        STENCIL_TYPE = 0
                    }, tran);

                    //更新網板最新清潔時間
                    string updateSql = @"UPDATE SMT_STENCIL_CONFIG SET LAST_CLEAN_TIME = SYSDATE
                                         WHERE STENCIL_NO = :STENCIL_NO";
                    await _dbConnection.ExecuteAsync(updateSql, new
                    {
                        model.STENCIL_NO
                    }, tran);

                    //更新網板狀態
                    updateSql = @"UPDATE SMT_STENCIL_RUNTIME SET STATUS=:STATUS WHERE STENCIL_NO=:STENCIL_NO ";
                    await _dbConnection.ExecuteAsync(updateSql, new
                    {
                        STATUS = GlobalVariables.StencilCleaned,
                        model.STENCIL_NO
                    }, tran);

                    //执行领取
                    updateSql = @"UPDATE SMT_STENCIL_STORE SET STATUS = :STATUS, UPDATE_TIME = SYSDATE WHERE STENCIL_ID = :STENCIL_ID";
                    await _dbConnection.ExecuteAsync(updateSql, new
                    {
                        STATUS = GlobalVariables.StencilTaken,
                        stencilStore.STENCIL_ID
                    }, tran);

                    string insertStencilOperationHistory = @"INSERT INTO SMT_STENCIL_OPERATION_HISTORY
                          (STENCIL_ID, STATUS, OPERATION_TYPE, OPERATION_BY, OPERATION_TIME, CREATE_BY, LOCATION)
                            VALUES (:STENCIL_ID, :STATUS, :OPERATION_TYPE, :OPERATION_BY, SYSDATE, :CREATE_BY, :LOCATION)";
                    await _dbConnection.ExecuteAsync(insertStencilOperationHistory, new
                    {
                        STENCIL_ID = stencilStore.STENCIL_ID,
                        STATUS = GlobalVariables.StencilTaken,
                        OPERATION_TYPE = GlobalVariables.TAKE_STENCIL,
                        OPERATION_BY = model.WorkNo,
                        CREATE_BY = model.UserName,
                        stencilStore.LOCATION,
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
        /// 获取钢网储位
        /// </summary>
        /// <param name="stencilNo"></param>
        /// <returns></returns>
        public async Task<string> GetStencilLocation(string stencilNo)
        {
            string sql = @"SELECT S.LOCATION FROM SMT_STENCIL_STORE S, SMT_STENCIL_CONFIG C 
                        WHERE C.ID = S.STENCIL_ID AND C.STENCIL_NO =:STENCIL_NO ";
            var result = await _dbConnection.ExecuteScalarAsync(sql, new { STENCIL_NO = stencilNo });

            return result?.ToString() ?? string.Empty;
        }

        /// <summary>
        /// 获取钢网是否清洗
        /// </summary>
        /// <param name="stencil_id">钢网ID</param>
        /// <returns></returns>
        public async Task<bool> StencilIsCleanedBefore(decimal stencil_id)
        {
            string selectCleanHisBefore = @"SELECT SC.STENCIL_NO  
                      FROM SMT_STENCIL_CLEAN_HISTORY C,
                           SMT_STENCIL_CONFIG SC,
                           SMT_STENCIL_STORE S
                     WHERE C.STENCIL_NO = SC.STENCIL_NO
                       AND C.CLEAN_TIME >= S.UPDATE_TIME
                       AND C.CLEAN_TIME <= SYSDATE
                       AND SC.ID = S.STENCIL_ID
                       AND S.STENCIL_ID = :STENCIL_ID
                       AND S.STATUS = 2";
            var result = await _dbConnection.ExecuteScalarAsync(selectCleanHisBefore, new { STENCIL_ID = stencil_id });

            return (result != null);
        }

        /// <summary>
        /// 钢网归还保存
        /// </summary>
        /// <param name="model"></param>
        /// <param name="stencilStore"></param>
        /// <returns></returns>
        public async Task<decimal> StencilReturnSave(SmtStencilReturnSaveModel model, CheckScrapResult stencilStore)
        {
            int result = 1;
            ConnectionFactory.OpenConnection(_dbConnection);
            using (var tran = _dbConnection.BeginTransaction())
            {
                try
                {
                    //更新状态
                    string updateSql = @"UPDATE SMT_STENCIL_STORE SET STATUS = :STATUS, LOCATION= :LOCATION, UPDATE_TIME = SYSDATE
                                        WHERE STENCIL_ID = :STENCIL_ID";
                    await _dbConnection.ExecuteAsync(updateSql, new
                    {
                        stencilStore.STENCIL_ID,
                        STATUS = GlobalVariables.StencilStored,
                        model.LOCATION
                    }, tran);

                    string updateWorkSql = @"update SMT_STENCIL_WO  set IS_USING = 'N' WHERE STENCIL_NO = :STENCIL_NO";

                    await _dbConnection.ExecuteAsync(updateWorkSql, new
                    {
                        STENCIL_NO = model.STENCIL_NO
                    }, tran);

                    //記錄網板清潔數據
                    string insertSql = @"INSERT INTO SMT_STENCIL_OPERATION_HISTORY
                          (STENCIL_ID, STATUS, OPERATION_TYPE, OPERATION_BY, OPERATION_TIME, CREATE_BY, LOCATION)
                          VALUES (:STENCIL_ID, :STATUS, :OPERATION_TYPE, :OPERATION_BY, SYSDATE, :CREATE_BY, :LOCATION)";
                    var resdata = await _dbConnection.ExecuteAsync(insertSql, new
                    {
                        stencilStore.STENCIL_ID,
                        STATUS = GlobalVariables.StencilStored,
                        OPERATION_TYPE = GlobalVariables.RETURN_STENCIL,
                        OPERATION_BY = model.WorkNo,
                        CREATE_BY = model.UserName,
                        model.LOCATION,
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
        /// 钢网清洗保存
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public async Task<decimal> StencilCleanSave(SmtStencilCleanSaveModel model)
        {
            int result = 1;
            ConnectionFactory.OpenConnection(_dbConnection);
            using (var tran = _dbConnection.BeginTransaction())
            {
                try
                {
                    //記錄網板清潔數據
                    decimal newid = await GetCleanCleanHistorySEQID();
                    string insertStencilCleanHistory = @"INSERT INTO SMT_STENCIL_CLEAN_HISTORY(ID, STENCIL_NO, OPERATION_LINE_NAME,
                                PRINT_COUNT, CLEAN_USER, INSPECT_USER, INSPECT_RESULT, CLEAN_TIME, TENSION_A, TENSION_B, TENSION_C, TENSION_D, TENSION_E, STENCIL_TYPE)
                                VALUES (:ID, :STENCIL_NO, :OPERATION_LINE_NAME, :PRINT_COUNT, :CLEAN_USER,
                                :INSPECT_USER, :INSPECT_RESULT, SYSDATE, :TENSION_A, :TENSION_B, :TENSION_C, :TENSION_D, :TENSION_E, :STENCIL_TYPE) ";
                    var resdata = await _dbConnection.ExecuteAsync(insertStencilCleanHistory, new
                    {
                        ID = newid,
                        model.STENCIL_NO,
                        OPERATION_LINE_NAME = string.Empty,
                        PRINT_COUNT = model.PrintCount,
                        CLEAN_USER = model.WorkNo,
                        INSPECT_USER = model.WorkNo,
                        INSPECT_RESULT = model.InspectResult,
                        model.TENSION_A,
                        model.TENSION_B,
                        model.TENSION_C,
                        model.TENSION_D,
                        model.TENSION_E,
                        STENCIL_TYPE = 0
                    }, tran);

                    //更新網板最新清潔時間
                    string updateSql = @"UPDATE SMT_STENCIL_CONFIG SET LAST_CLEAN_TIME = SYSDATE
                                         WHERE STENCIL_NO = :STENCIL_NO";
                    await _dbConnection.ExecuteAsync(updateSql, new
                    {
                        model.STENCIL_NO
                    }, tran);

                    //更新網板狀態
                    var curStatus = (model.InspectResult.IndexOf("张力异常") == -1) ? GlobalVariables.StencilCleaned : GlobalVariables.StencilStopUsed;

                    updateSql = @"UPDATE SMT_STENCIL_RUNTIME SET STATUS=:STATUS WHERE STENCIL_NO=:STENCIL_NO ";
                    await _dbConnection.ExecuteAsync(updateSql, new
                    {
                        STATUS = curStatus,
                        model.STENCIL_NO
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
        ///获取状态列表 
        /// </summary>
        /// <returns></returns>
        public async Task<List<IDNAME>> GetStatus()
        {
            string sql = "SELECT CODE as ID, CN_DESC AS NAME FROM SMT_LOOKUP where Type ='STENCIL_STATUS'";
            var result = await _dbConnection.QueryAsync<IDNAME>(sql);
            return result?.ToList();
        }

        /// <summary>
        /// 获取钢网保养记录列表
        /// </summary>
        /// <param name="stencil_no"></param>
        /// <returns></returns>
        public async Task<List<SmtStencilMaintainHistory>> GetStencilMaintainHistoryList(string stencil_no)
        {
            List<SmtStencilMaintainHistory> result = new List<SmtStencilMaintainHistory>();

            var tmpdata = await GetStencil(stencil_no);
            if (tmpdata!= null)
            {
                string sql = "SELECT * FROM SMT_STENCIL_MAINTAIN_HISTORY where STENCIL_ID = :STENCIL_ID";
                var redata = await _dbConnection.QueryAsync<SmtStencilMaintainHistory>(sql, new
                {
                    STENCIL_ID = tmpdata.ID
                });
                result = redata?.ToList();
            }
            return result;
        }

        // <summary>
        /// 获取表StencilMaintainHistory的序列
        /// </summary>
        /// <returns></returns>
		public async Task<decimal> GetStencilMaintainHistorySEQID()
        {
            string sql = "SELECT SMT_STENCIL_MAINTAIN_HIS_SEQ.NEXTVAL MY_SEQ FROM DUAL";
            var result = await _dbConnection.ExecuteScalarAsync(sql);
            return (decimal)result;
        }

        // <summary>
        /// 获取表SMT_STENCIL_RUNTIME的序列
        /// </summary>
        /// <returns></returns>
        public async Task<decimal> GetStencilRuntimeSEQID()
        {
            string sql = "SELECT SMT_STENCIL_RUNTIME_SEQ.NEXTVAL MY_SEQ FROM DUAL";
            var result = await _dbConnection.ExecuteScalarAsync(sql);
            return (decimal)result;
        }

        /// <summary>
        /// 钢网保养保存
        /// </summary>
        /// <param name="model"></param>
        /// <param name="stencil_id"></param>
        /// <returns></returns>
        public async Task<decimal> StencilMaintainSave(SmtStencilMaintainModel model, decimal stencil_id)
        {
            int result = 1;
            decimal maintainhistory_id = 0;
            decimal maintainStatus = (model.ResultStatus ==1)? GlobalVariables.StencilUsed : GlobalVariables.StencilScrapped;
            ConnectionFactory.OpenConnection(_dbConnection);
            using (var tran = _dbConnection.BeginTransaction())
            {
                try
                {
                    var runtime = await GetStencilRuntime(model.STENCIL_NO);
                    if (runtime == null)
                    {
                        // 更新Runtime狀態
                        decimal runtime_id = await GetStencilRuntimeSEQID();
                        string insertStencilRuntime = @"INSERT INTO SMT_STENCIL_RUNTIME(ID,STENCIL_NO,PRINT_COUNT,PRODUCT_PASS_COUNT,STATUS,
                                CURRENT_COUNT,OPERATION_TIME,OPERATION_SITE_ID,OPERATOR) VALUES (
                                :ID,:STENCIL_NO,:PRINT_COUNT,:PRODUCT_PASS_COUNT,:STATUS,:CURRENT_COUNT,SYSDATE,:OPERATION_SITE_ID,:OPERATOR) ";
                        await _dbConnection.ExecuteAsync(insertStencilRuntime, new
                        {
                            ID = runtime_id,
                            model.STENCIL_NO,
                            PRINT_COUNT = 0,
                            PRODUCT_PASS_COUNT = 0,
                            STATUS = maintainStatus,
                            CURRENT_COUNT = 0,
                            OPERATION_SITE_ID = 0,
                            OPERATOR = model.UserName,
                        }, tran);

                        // 記錄維護操作
                        maintainhistory_id = await GetStencilMaintainHistorySEQID();
                        string insertStencilMaintainHistory = @"INSERT INTO SMT_STENCIL_MAINTAIN_HISTORY (ID,
                              STENCIL_ID, PRINT_COUNT, PRODUCT_PASS_COUNT, CURRENT_STATUS,MAINTAIN_STATUS, OPERATION_TIME, OPERATOR, MAINTAIN_DESCRIPTION)
                              VALUES (:ID, :STENCIL_ID,:PRINT_COUNT, :PRODUCT_PASS_COUNT, :CURRENT_STATUS,
                              :MAINTAIN_STATUS, SYSDATE, :OPERATOR, :MAINTAIN_DESCRIPTION) ";
                        await _dbConnection.ExecuteAsync(insertStencilMaintainHistory, new
                        {
                            ID = maintainhistory_id,
                            STENCIL_ID = stencil_id,
                            PRINT_COUNT = 0,
                            PRODUCT_PASS_COUNT = 0,
                            CURRENT_STATUS = GlobalVariables.StencilUsed,
                            MAINTAIN_STATUS = maintainStatus,
                            OPERATOR = model.UserName,
                            MAINTAIN_DESCRIPTION = model.Remark
                        }, tran);
                    }
                    else
                    {
                        // 更新Runtime狀態
                        string updateStencilStatus = @"UPDATE SMT_STENCIL_RUNTIME SET STATUS=:STATUS WHERE STENCIL_NO=:STENCIL_NO ";
                        await _dbConnection.ExecuteAsync(updateStencilStatus, new
                        {
                            model.STENCIL_NO,
                            STATUS = maintainStatus,
                        }, tran);

                        // 記錄維護操作
                        maintainhistory_id = await GetStencilMaintainHistorySEQID();
                        string insertStencilMaintainHistory = @"INSERT INTO SMT_STENCIL_MAINTAIN_HISTORY (ID,
                              STENCIL_ID, PRINT_COUNT, PRODUCT_PASS_COUNT, CURRENT_STATUS,MAINTAIN_STATUS, OPERATION_TIME, OPERATOR, MAINTAIN_DESCRIPTION)
                              VALUES (:ID, :STENCIL_ID,:PRINT_COUNT, :PRODUCT_PASS_COUNT, :CURRENT_STATUS,
                              :MAINTAIN_STATUS, SYSDATE, :OPERATOR, :MAINTAIN_DESCRIPTION) ";
                        await _dbConnection.ExecuteAsync(insertStencilMaintainHistory, new
                        {
                            ID = maintainhistory_id,
                            STENCIL_ID = stencil_id,
                            runtime.PRINT_COUNT,
                            runtime.PRODUCT_PASS_COUNT,
                            CURRENT_STATUS = runtime.STATUS,
                            MAINTAIN_STATUS = maintainStatus,
                            OPERATOR = model.UserName,
                            MAINTAIN_DESCRIPTION = model.Remark
                        }, tran);
                    }

                    //如果是报废，写入操作记录
                    if (maintainStatus == GlobalVariables.StencilScrapped)
                    {
                        string insertStencilOperationHistory =@"INSERT INTO SMT_STENCIL_OPERATION_HISTORY
                          (STENCIL_ID, STATUS, OPERATION_TYPE, OPERATION_BY, OPERATION_TIME, CREATE_BY, LOCATION)
                          VALUES (:STENCIL_ID, :STATUS, :OPERATION_TYPE, :OPERATION_BY, SYSDATE, :CREATE_BY, :LOCATION)";
                        await _dbConnection.ExecuteAsync(insertStencilOperationHistory, new
                        {
                            STENCIL_ID = stencil_id,
                            STATUS = maintainStatus,
                            OPERATION_TYPE = GlobalVariables.STENCIL_MAINTAIN_SCRAP,
                            OPERATION_BY = model.UserName,
                            CREATE_BY = model.UserName,
                            LOCATION = string.Empty
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
        /// 钢网使用次数
        /// </summary>
        /// <param name="WO_NO"></param>
        /// <returns></returns>
        public async Task<TableDataModel> GetStencilUseData(string STENCIL_NO, PageModel pageModel)
        {
            var conditions = " ";
            if (!STENCIL_NO.IsNullOrEmpty())
            {
                conditions += " AND A.STENCIL_NO = :STENCIL_NO";
            }
            var sqlPage = $@"SELECT * FROM (select A.*,B.PART_NO,B.PN_MODEL,rownum num,C.NICK_NAME,D.CN_DESC from SMT_STENCIL_RUNTIME A 
					LEFT JOIN SMT_STENCIL_PART B ON A.STENCIL_NO = B.STENCIL_NO 
					LEFT JOIN SYS_MANAGER C ON A.OPERATOR = C.USER_NAME 
					LEFT JOIN SMT_LOOKUP D ON A.STATUS = D.CODE
					WHERE D.TYPE='STENCIL_STORE_STATUS' {conditions}) u 
					WHERE u.num BETWEEN ((:Page-1) * :Limit + 1) AND (:Page * :Limit)";

            var data = await _dbConnection.QueryAsync<dynamic>(sqlPage, new { STENCIL_NO, pageModel.Limit, pageModel.Page });

            var countSql = $@"select COUNT(1) from SMT_STENCIL_RUNTIME A LEFT JOIN SMT_STENCIL_PART B ON A.STENCIL_NO = B.STENCIL_NO WHERE 1=1 {conditions}";
            var count = await _dbConnection.ExecuteScalarAsync<int>(countSql, new { STENCIL_NO, pageModel.Limit, pageModel.Page });

            return new TableDataModel() { data = data, count = count };
        }
    }
}