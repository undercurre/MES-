/*
*┌──────────────────────────────────────────────────────────────┐
*│　描    述：接口实现                                                    
*│　作    者：嘉志科技                                            
*│　版    本：2.0    模板代码自动生成                                                
*│　创建时间：2020-07-22 10:16:13                             
*└──────────────────────────────────────────────────────────────┘
*┌──────────────────────────────────────────────────────────────┐
*│　命名空间： JZ.IMS.Repository.Oracle                                  
*│　类    名： MesBurnFileApplyRepository                                      
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
using System.Data;
using OfficeOpenXml;
using JZ.IMS.Core.Extensions;
using JZ.IMS.ViewModels.BurnFile;
using MySqlX.XDevAPI.Relational;
using System.IO;

namespace JZ.IMS.Repository.Oracle
{
    public class MesBurnFileApplyRepository : BaseRepository<MesBurnFileApply, Decimal>, IMesBurnFileApplyRepository
    {
        const int _cloudFile = 2;//云服务
        const int _customerFile = 1;//客户文件

        public MesBurnFileApplyRepository(IOptionsSnapshot<DbOption> options)
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
            string sql = "SELECT ENABLED FROM MES_BURN_FILE_APPLY WHERE ID=:ID";
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
            string sql = "UPDATE MES_BURN_FILE_APPLY set ENABLED=:ENABLED WHERE ID=:Id";
            return await _dbConnection.ExecuteAsync(sql, new
            {
                ENABLED = status ? 'Y' : 'N',
                Id = id,
            });
        }

        /// <summary>
        /// 获取文件申请的序列
        /// </summary>
        /// <returns></returns>
		public async Task<decimal> GetFileApplySEQID()
        {
            string sql = "SELECT MES_FILE_APPLY_SEQ.NEXTVAL FROM DUAL";
            var result = await _dbConnection.ExecuteScalarAsync(sql);
            return (decimal)result;
        }

        /// <summary>
        /// 获取文件的序列
        /// </summary>
        /// <returns></returns>
        public async Task<decimal> GetFIleManagerSEQID()
        {
            string sql = "SELECT MES_FILE_MANAGER_SEQ.NEXTVAL FROM DUAL";
            var result = await _dbConnection.ExecuteScalarAsync(sql);
            return (decimal)result;
        }

        /// <summary>
        /// 获取烧录的序列
        /// </summary>
        /// <returns></returns>
        public async Task<decimal> GetBurnFIleSEQID()
        {
            string sql = "SELECT MES_BURN_FILE_SEQ.NEXTVAL FROM DUAL";
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
            string sql = "select count(0) from MES_BURN_FILE_APPLY where id = :id";
            object result = await _dbConnection.ExecuteScalarAsync(sql, new
            {
                id
            });

            return (Convert.ToInt32(result) > 0);
        }

        /// <summary>
        /// 获取文件关联分页
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public async Task<TableDataModel> GetApplyRelationLoadData(MesBurnApplyRelationRequestModel model)
        {
            string sql = @" SELECT ROW_NUMBER() OVER(ORDER BY BAR.ID DESC) AS ROWNO,BFM.ID as BFMANAGER_ID,BFM.CODE,BFM.TYPE as TYPE,BFM.PATH,BFM.FILENAME,BFA.ID as BFAPPLY_ID ,BFA.APPLY_NO,BFA.PART_CODE,BFA.WO_NO,BAR.STATUS,BAR.CREATE_TIME,BFM.CREATE_TIME B_CREATE_TIME FROM MES_BURN_APPLY_RELATION BAR  
                            LEFT JOIN MES_BURN_FILE_MANAGER BFM ON BFM.ID=BAR.BURN_FILE_ID
                            LEFT JOIN MES_BURN_FILE_APPLY BFA ON BAR.APPLY_ID=BFA.ID";

            string conditions = " WHERE 1=1 AND BAR.STATUS=0 ";

            if (!model.CODE_ID.IsNullOrWhiteSpace())
            {
                conditions += $" and BFM.ID =:CODE_ID ";
            }
            if (!model.APPLY_ID.IsNullOrWhiteSpace())
            {
                conditions += $" and BFA.ID =:APPLY_ID ";
            }
            if (!model.CODE.IsNullOrWhiteSpace())
            {
                conditions += $" and BFM.CODE =:CODE ";
            }
            if (!model.APPLY_NO.IsNullOrWhiteSpace())
            {
                conditions += $" and BFA.APPLY_NO=:APPLY_NO ";
            }

            string pagedSql = SQLBuilderClass.GetPagedSQL(sql, conditions);
            var resdata = await _dbConnection.QueryAsync<object>(pagedSql, model);
            string sqlcnt = @"  SELECT COUNT(*) FROM MES_BURN_APPLY_RELATION BAR  
                            LEFT JOIN MES_BURN_FILE_MANAGER BFM ON BFM.ID=BAR.BURN_FILE_ID
                            LEFT JOIN MES_BURN_FILE_APPLY BFA ON BAR.APPLY_ID=BFA.ID " + conditions;

            int cnt = await _dbConnection.ExecuteScalarAsync<int>(sqlcnt, model);
            return new TableDataModel
            {
                count = cnt,
                data = resdata?.ToList(),
            };
        }

        /// <summary>
        /// 获取下载文件记录
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public async Task<TableDataModel> GetDownLoadData(MesBurnFileDownRequestModel model)
        {
            string sql = @" SELECT  ROWNUM as rowno,MBFD.ID,MBFD.DOWN_NO,MBFA.PART_CODE,MBFD.WO_NO,MBFA.APPLY_NO,MBFM.CODE MANAGER_NO,MBFD.CREATE_TIME FROM MES_BURN_FILE_DOWN MBFD 
                            LEFT JOIN MES_BURN_FILE_APPLY MBFA ON MBFD.APPLY_ID=MBFA.ID
                            INNER JOIN MES_BURN_APPLY_RELATION MBAR ON MBAR.APPLY_ID=MBFD.APPLY_ID
                            INNER JOIN MES_BURN_FILE_MANAGER MBFM ON MBAR.BURN_FILE_ID=MBFM.ID";

            string conditions = " WHERE 1=1 AND  MBAR.STATUS=0 ";

            if (model.ID > 0)
            {
                conditions += $"AND MBFD.ID=:ID ";
            }
            if (!model.WO_NO.IsNullOrWhiteSpace())
            {
                conditions += $"AND MBFD.WO_NO=:WO_NO ";
            }
            if (model.APPLY_ID > 0)
            {
                conditions += $"AND MBFD.APPLY_ID=:APPLY_ID ";
            }
            if ((model.StartTime != null && model.EndTime != null) && (model.StartTime <= model.EndTime))
            {
                conditions += $"AND MBFD.CREATE_TIME > =:StartTime AND MBFD.CREATE_TIME < =:EndTime ";
            }

            string pagedSql = SQLBuilderClass.GetPagedSQL(sql, conditions + " ORDER BY MBFD.CREATE_TIME DESC");
            var resdata = await _dbConnection.QueryAsync<object>(pagedSql, model);
            string sqlcnt = @"  SELECT COUNT(1) FROM MES_BURN_FILE_DOWN MBFD 
                            LEFT JOIN MES_BURN_FILE_APPLY MBFA ON MBFD.APPLY_ID=MBFA.ID
                            INNER JOIN MES_BURN_APPLY_RELATION MBAR ON MBAR.APPLY_ID=MBFD.APPLY_ID
                            INNER JOIN MES_BURN_FILE_MANAGER MBFM ON MBAR.BURN_FILE_ID=MBFM.ID" + conditions;

            int cnt = await _dbConnection.ExecuteScalarAsync<int>(sqlcnt, model);
            return new TableDataModel
            {
                count = cnt,
                data = resdata?.ToList(),
            };
        }

        /// <summary>
        /// 修改激活状态
        /// </summary>
        /// <param name="id">主键</param>
        /// <param name="status">更改后的状态</param>
        /// <returns></returns>
        public async Task<List<MesBurnFileApply>> GetMesFileApplyByWONO(string WO_NO)
        {
            string sql = " SELECT * FROM MES_BURN_FILE_APPLY WHERE WO_NO=:WO_NO ";
            return (await _dbConnection.QueryAsync<MesBurnFileApply>(sql, new
            {
                WO_NO = WO_NO
            }))?.ToList();
        }


        /// <summary>
        /// 通过SN找工单
        /// </summary>
        /// <param name="SN"></param>
        /// <returns></returns>
        public async Task<TableDataModel> GetWONOBySN(string SN)
        {
          
            string woNo = "";
            TableDataModel tableData = new TableDataModel();
            try
            {
                string sql = @" SELECT COUNT (*) FROM sfcs_runcard_ranger
                           WHERE :p_sn BETWEEN sn_begin AND sn_end
                           AND LENGTH(:p_sn) = LENGTH(sn_end)
                           AND(FIX_HEADER = SUBSTR(:p_sn, 1, HEADER_LENGTH)
                               OR FIX_HEADER IS NULL)
                           AND(FIX_TAIL = SUBSTR(:p_sn,LENGTH(:p_sn) - TAIL_LENGTH + 1,TAIL_LENGTH)
                               OR FIX_TAIL IS NULL) ";
                string sqlRanger = @"SELECT id,
                                   wo_id,
                                   exclusive_char,
                                   header_length,
                                   RANGE,
                                   STATUS
                                 FROM sfcs_runcard_ranger
                                 WHERE     :p_sn BETWEEN sn_begin AND sn_end
                                 AND LENGTH (:p_sn) = LENGTH (sn_end)
                                 AND (   FIX_HEADER = SUBSTR (:p_sn, 1, HEADER_LENGTH)
                                      OR FIX_HEADER IS NULL)
                                 AND (   FIX_TAIL =
                                            SUBSTR (:p_sn,
                                                    LENGTH (:p_sn) - TAIL_LENGTH + 1,
                                                    TAIL_LENGTH)
                                      OR FIX_TAIL IS NULL)";
                string selectWONO = @" SELECT WO_NO
                                        FROM SFCS_WO
                                       WHERE ID = :ID ";

                var count = 0;
                count = await _dbConnection.ExecuteScalarAsync<int>(sql, new { p_sn = SN });
                if (count == 1)
                {
                    var model = (await _dbConnection.QueryAsync<SfcsRuncardRanger>(sqlRanger, new
                    {
                        p_sn = SN
                    })).FirstOrDefault();

                    if (model != null)
                    {
                        var serial_number = "";
                        var v_exclusive_char = "";
                        serial_number = SN.Substring(Convert.ToInt32(model.HEADER_LENGTH ?? 0) , Convert.ToInt32(model.RANGE ?? 0));
                        while (model.EXCLUSIVE_CHAR.Length > 0)
                        {
                            v_exclusive_char = model.EXCLUSIVE_CHAR.Substring(1, 1);
                            model.EXCLUSIVE_CHAR = model.EXCLUSIVE_CHAR.Substring(2);
                            //SN Char Not Match
                            if (serial_number.Contains(v_exclusive_char))
                            {
                                tableData.code = -1;
                                throw new Exception();
                            }
                        }
                    }

                  var woModel= (await _dbConnection.QueryAsync<SfcsWo>(selectWONO,new {ID=model.WO_ID })).FirstOrDefault();
                    if (woModel != null) woNo = woModel.WO_NO;

                }
            }
            catch (Exception ex)
            {
                tableData.code = -1;
            }
            tableData.data = woNo;
            return tableData;

        }

        /// <summary>
        /// 获取烧录文件的具体信息
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public async Task<List<MesBurnFileManager>> GetMesBurnManagerByNo(string WO_NO)
        {
            TableDataModel tableData = new TableDataModel();
            ConnectionFactory.OpenConnection(_dbConnection);
            List<MesBurnFileManager> result = new List<MesBurnFileManager>();
            try
            {
                string sql = @"SELECT MBFM.* FROM MES_BURN_APPLY_RELATION MBAR 
                               INNER JOIN  MES_BURN_FILE_APPLY MBFA ON MBFA.ID = MBAR.APPLY_ID
                               INNER JOIN MES_BURN_FILE_MANAGER MBFM ON MBAR.BURN_FILE_ID = MBFM.ID
                               WHERE MBFA.WO_NO =:WO_NO AND MBAR.STATUS = 0 ";
                result = (await _dbConnection.QueryAsync<MesBurnFileManager>(sql, new { WO_NO = WO_NO })).ToList();
                return result?.ToList();
            }
            catch (Exception ex)
            {

                throw new Exception(ex.Message);
            }

            return result;
        }

        /// <summary>
        /// 保存烧录文件
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public async Task<TableDataModel> SaveFileManagerDataByTrans(MesBurnFileManagerModel model)
        {
            TableDataModel tableData = new TableDataModel();
            ConnectionFactory.OpenConnection(_dbConnection);
            List<string> listID = new List<string>();
            using (var tran = _dbConnection.BeginTransaction())
            {
                try
                {
                    //删除
                    string deleteSql = @"Delete from MES_BURN_FILE_MANAGER where ID=:ID ";
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

                    //更新
                    string updateSql = @"Update MES_BURN_FILE_MANAGER set CODE=:CODE,TYPE=:TYPE,PATH=:PATH,FILENAME=:FILENAME,REMARK=:REMARK
						where ID=:ID ";
                    if (model.UpdateRecords != null && model.UpdateRecords.Count > 0)
                    {
                        foreach (var item in model.UpdateRecords)
                        {
                            listID.Add(item.ID.ToString());
                            var resdata = await _dbConnection.ExecuteAsync(updateSql, new
                            {
                                item.ID,
                                item.CODE,
                                item.TYPE,
                                item.PATH,
                                item.FILENAME,
                                item.REMARK
                            }, tran);
                        }
                    }

                    //新增
                    string insertSql = @"insert into MES_BURN_FILE_MANAGER 
					(ID,CODE,TYPE,PATH,FILENAME,CREATE_TIME,REMARK) 
					VALUES (:ID,:CODE,:TYPE,:PATH,:FILENAME,:CREATE_TIME,:REMARK)";
                    if (model.InsertRecords != null && model.InsertRecords.Count > 0)
                    {
                        foreach (var item in model.InsertRecords)
                        {
                            //var ID = await GetFIleManagerSEQID();
                            listID.Add(item.ID.ToString());
                            //var code = string.Empty;
                            //code = ID > 10000 ? ID.ToString() : ID.ToString().PadLeft(6, '0');
                            //item.CODE = "BF-" + code; // 一共6位,位数不够时从左边开始用0补;
                            item.CREATE_TIME = DateTime.Now;
                            var resdata = await _dbConnection.ExecuteAsync(insertSql, new
                            {
                                item.ID,
                                item.CODE,
                                item.TYPE,
                                item.PATH,
                                item.FILENAME,
                                item.CREATE_TIME,
                                item.REMARK
                            }, tran);
                        }
                    }
                    tran.Commit();
                    tableData.data = listID;
                }
                catch (Exception ex)
                {
                    tableData.code = -1;
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
            return tableData;
        }

        /// <summary>
        /// 保存文件绑定的关系
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public async Task<decimal> SaveApplyAndRelationDataByTrans(BurnFileApplyAndRelation model)
        {
            int result = 1;
            ConnectionFactory.OpenConnection(_dbConnection);
            using (var tran = _dbConnection.BeginTransaction())
            {
                try
                {

                    #region 处理申请文件

                    #region 删除
                    string deleteSql = @"Delete from MES_BURN_FILE_APPLY where ID=:ID ";
                    if (model.ApplyModel.RemoveRecords != null && model.ApplyModel.RemoveRecords.Count > 0)
                    {
                        foreach (var item in model.ApplyModel.RemoveRecords)
                        {
                            var select_Apply = "SELECT * FROM MES_BURN_FILE_APPLY WHERE ID=:ID";
                            string insertHistorySql = @"insert into MES_BURN_FILE_APPLY_HISTORY 
					(ID,APPLY_NO,PART_CODE,WO_NO,USER_NAME,CREATE_TIME,LOG_TIME,LOG_USER,MST_ID) 
					VALUES (:ID,:APPLY_NO,:PART_CODE,:WO_NO,:USER_NAME,:CREATE_TIME,:LOG_TIME,:LOG_USER,:MST_ID)";

                            var data = (await _dbConnection.QueryAsync<MesBurnFileApply>(select_Apply, new { ID = item.ID })).FirstOrDefault();
                            if (data != null)
                            {
                                #region 更新到记录表
                                var ID = await GetBurnFIleSEQID();
                                var resdata = await _dbConnection.ExecuteAsync(insertHistorySql, new
                                {
                                    ID = ID,
                                    data.APPLY_NO,
                                    data.PART_CODE,
                                    data.WO_NO,
                                    data.USER_NAME,
                                    data.CREATE_TIME,
                                    MST_ID = data.ID,
                                    LOG_TIME = item.CREATE_TIME,
                                    LOG_USER = item.USER_NAME,
                                }, tran);
                                #endregion

                                var resDelData = await _dbConnection.ExecuteAsync(deleteSql, new
                                {
                                    item.ID
                                }, tran);

                                #region 删除绑定文件

                                //删除
                                string deleteRelationSql = @"Delete from MES_BURN_APPLY_RELATION where APPLY_ID=:ID ";
                                await _dbConnection.ExecuteAsync(deleteRelationSql, new
                                {
                                    item.ID
                                }, tran);

                                #endregion

                            }
                        }
                    }
                    #endregion

                    #region 更新模块
                    //string updateSql = @"Update MES_BURN_FILE_APPLY set APPLY_NO=:APPLY_NO,PART_CODE=:PART_CODE,WO_NO=:WO_NO,USER_NAME=:USER_NAME,CREATE_TIME=:CREATE_TIME,MODIFY_TIME=:MODIFY_TIME  
                    //where ID=:ID ";

                    //               if (model.ApplyModel.UpdateRecords != null && model.ApplyModel.UpdateRecords.Count > 0)
                    //               {
                    //                   foreach (var item in model.ApplyModel.UpdateRecords)
                    //                   {
                    //                       #region 文件申请更新方法
                    //                       //var resdata = await _dbConnection.ExecuteAsync(updateSql, new
                    //                       //{

                    //                       //    item.ID,
                    //                       //    item.APPLY_NO,
                    //                       //    item.PART_CODE,
                    //                       //    item.WO_NO,
                    //                       //    item.USER_NAME,
                    //                       //    item.CREATE_TIME,
                    //                       //    item.MODIFY_TIME,
                    //                       //    
                    //                       //}, tran);
                    //                       #endregion

                    //                       #region 更新操作
                    //                       var newid = await GetBurnFIleSEQID();
                    //                       var select_Apply = "SELECT * FROM MES_BURN_FILE_APPLY WHERE ID=:ID";
                    //                       string insertHistorySql = @"insert into MES_BURN_FILE_APPLY_HISTORY 
                    //(ID,APPLY_NO,PART_CODE,WO_NO,USER_NAME,CREATE_TIME,LOG_TIME,LOG_USER,MST_ID) 
                    //VALUES (:ID,:APPLY_NO,:PART_CODE,:WO_NO,:USER_NAME,:CREATE_TIME,:LOG_TIME,:LOG_USER,:MST_ID)";

                    //                       var data = (await _dbConnection.QueryAsync<MesBurnFileApply>(select_Apply, new { ID = item.ID })).FirstOrDefault();
                    //                       if (data != null)
                    //                       {
                    //                           #region 更新到记录表
                    //                           var resdata = await _dbConnection.ExecuteAsync(insertHistorySql, new
                    //                           {
                    //                               ID = newid,
                    //                               data.APPLY_NO,
                    //                               data.PART_CODE,
                    //                               data.WO_NO,
                    //                               data.USER_NAME,
                    //                               data.CREATE_TIME,
                    //                               MST_ID = data.ID,
                    //                               LOG_TIME = item.CREATE_TIME,
                    //                               LOG_USER = item.USER_NAME,
                    //                           }, tran);
                    //                           #endregion

                    //                           #region 删除
                    //                           string deleteApplySql = @"Delete from MES_BURN_FILE_APPLY where ID=:ID ";
                    //                           var resDelData = await _dbConnection.ExecuteAsync(deleteApplySql, new
                    //                           {
                    //                               item.ID
                    //                           }, tran);
                    //                           #endregion

                    //                           #region 新增

                    //                           string insertApplySql = @"insert into MES_BURN_FILE_APPLY 
                    //                                    (ID,APPLY_NO,PART_CODE,WO_NO,USER_NAME,CREATE_TIME) 
                    //                                    VALUES (:ID,:APPLY_NO,:PART_CODE,:WO_NO,:USER_NAME,to_date(:CREATE_TIME))";
                    //                           var ID = await GetFileApplySEQID();
                    //                           var code = string.Empty;
                    //                           code = ID > 10000 ? ID.ToString() : ID.ToString().PadLeft(6, '0');
                    //                           item.APPLY_NO = "AYN-" + code; // 一共6位,位数不够时从左边开始用0补;
                    //                           item.CREATE_TIME = DateTime.Now;
                    //                           var resApplydata = await _dbConnection.ExecuteAsync(insertApplySql, new
                    //                           {
                    //                               ID = ID,
                    //                               item.APPLY_NO,
                    //                               item.PART_CODE,
                    //                               item.WO_NO,
                    //                               item.USER_NAME,
                    //                               item.CREATE_TIME
                    //                           }, tran);

                    //                           #endregion
                    //                       }
                    //                       #endregion
                    //                   }
                    //               }
                    #endregion

                    #region 新增
                    string insertSql = @"insert into MES_BURN_FILE_APPLY 
					(ID,APPLY_NO,PART_CODE,WO_NO,USER_NAME,CREATE_TIME) 
					VALUES (:ID,:APPLY_NO,:PART_CODE,:WO_NO,:USER_NAME,:CREATE_TIME)";
                    if (model.ApplyModel.InsertRecords != null && model.ApplyModel.InsertRecords.Count > 0)
                    {
                        foreach (var item in model.ApplyModel.InsertRecords)
                        {
                            var newid = await GetBurnFIleSEQID();
                            string conditions = "";
                            if (!item.WO_NO.IsNullOrWhiteSpace())
                            {
                                conditions += " AND WO_NO =:WO_NO ";
                            }
                            if (!item.PART_CODE.IsNullOrWhiteSpace())
                            {
                                conditions += " AND PART_CODE =:PART_CODE ";
                            }
                            var select_Apply = "SELECT * FROM MES_BURN_FILE_APPLY WHERE 1=1 " + conditions;
                            string insertHistorySql = @"insert into MES_BURN_FILE_APPLY_HISTORY 
					(ID,APPLY_NO,PART_CODE,WO_NO,USER_NAME,CREATE_TIME,LOG_TIME,LOG_USER,MST_ID) 
					VALUES (:ID,:APPLY_NO,:PART_CODE,:WO_NO,:USER_NAME,:CREATE_TIME,:LOG_TIME,:LOG_USER,:MST_ID)";

                            var data = (await _dbConnection.QueryAsync<MesBurnFileApply>(select_Apply, new { PART_CODE = item.PART_CODE, WO_NO = item.WO_NO })).FirstOrDefault();
                            if (data != null)
                            {
                                #region 更新到记录表

                                var historyResult = await _dbConnection.ExecuteAsync(insertHistorySql, new
                                {
                                    ID = newid,
                                    data.APPLY_NO,
                                    data.PART_CODE,
                                    data.WO_NO,
                                    data.USER_NAME,
                                    data.CREATE_TIME,
                                    MST_ID = data.ID,
                                    LOG_TIME = item.CREATE_TIME,
                                    LOG_USER = item.USER_NAME,
                                }, tran);

                                #endregion

                                #region 删除
                                string deleteApplySql = @"Delete from MES_BURN_FILE_APPLY where ID=:ID ";
                                var resDelData = await _dbConnection.ExecuteAsync(deleteApplySql, new
                                {
                                    item.ID
                                }, tran);
                                #endregion

                                #region 新增


                                //var ID = await GetFileApplySEQID();
                                //var code = string.Empty;
                                //code = ID > 10000 ? ID.ToString() : ID.ToString().PadLeft(6, '0');
                                //item.APPLY_NO = "AYN-" + code; // 一共6位,位数不够时从左边开始用0补;
                                item.CREATE_TIME = DateTime.Now;
                                var resApplydata = await _dbConnection.ExecuteAsync(insertSql, new
                                {
                                    item.ID,
                                    item.APPLY_NO,
                                    item.PART_CODE,
                                    item.WO_NO,
                                    item.USER_NAME,
                                    item.CREATE_TIME
                                }, tran);

                                #endregion
                            }
                            else
                            {
                                item.CREATE_TIME = DateTime.Now;
                                var resApplydata = await _dbConnection.ExecuteAsync(insertSql, new
                                {
                                    item.ID,
                                    item.APPLY_NO,
                                    item.PART_CODE,
                                    item.WO_NO,
                                    item.USER_NAME,
                                    item.CREATE_TIME
                                }, tran);
                            }
                        }
                    }
                    #endregion

                    #endregion


                    #region 处理绑定文件
                    string insertRelationSql = @"insert into MES_BURN_APPLY_RELATION 
					(ID,APPLY_ID,BURN_FILE_ID,STATUS,USER_NAME,CREATE_TIME) 
					VALUES (:ID,:APPLY_ID,:BURN_FILE_ID,:STATUS,:USER_NAME,:CREATE_TIME)";

                    if (model.RelationArrary.Count > 0)
                    {
                        foreach (var relationModel in model.RelationArrary)
                        {
                            //更新
                            string select_sql = "SELECT * FROM MES_BURN_APPLY_RELATION WHERE APPLY_ID=:APPLY_ID";
                            string updateSql = @"Update MES_BURN_APPLY_RELATION set STATUS=:STATUS,MODIFY_TIME=:MODIFY_TIME  WHERE APPLY_ID=:APPLY_ID ";
                            string updateRelationSql = @"Update MES_BURN_APPLY_RELATION set STATUS=:STATUS,MODIFY_TIME=:MODIFY_TIME  WHERE APPLY_ID=:APPLY_ID AND BURN_FILE_ID=:BURN_FILE_ID ";

                            if (relationModel.UpdateRecords != null && relationModel.UpdateRecords.Count > 0)
                            {
                                //修改之前的文件为禁用的状态
                                var relationList = (await _dbConnection.QueryAsync<MesBurnApplyRelation>(select_sql, new { APPLY_ID = relationModel.UpdateRecords[0].APPLY_ID })).ToList();
                                if (relationList != null && relationList.Count > 0)
                                {
                                    var modifyTime = DateTime.Now;
                                    var resdata = await _dbConnection.ExecuteAsync(updateSql, new
                                    {
                                        MODIFY_TIME = modifyTime,
                                        STATUS = 1,
                                        APPLY_ID = relationModel.UpdateRecords[0].APPLY_ID
                                    });
                                }
                                //
                                foreach (var item in relationModel.UpdateRecords)
                                {
                                    //存在在修改状态为0 不存在就添加
                                    var nowTime = DateTime.Now;
                                    if (relationList.Count(c => c.BURN_FILE_ID == item.BURN_FILE_ID) > 0)
                                    {
                                        var resdata = await _dbConnection.ExecuteAsync(updateRelationSql, new
                                        {
                                            MODIFY_TIME = nowTime,
                                            STATUS = 0,
                                            item.APPLY_ID,
                                            item.BURN_FILE_ID
                                        });
                                    }
                                    else
                                    {
                                        var newid = await GetBurnFIleSEQID();
                                        item.CREATE_TIME = nowTime;
                                        var resdata = await _dbConnection.ExecuteAsync(insertRelationSql, new
                                        {
                                            ID = newid,
                                            item.APPLY_ID,
                                            item.BURN_FILE_ID,
                                            STATUS = 0,
                                            item.USER_NAME,
                                            item.CREATE_TIME
                                        }, tran);
                                    }
                                }
                            }

                            //新增


                            if (relationModel.InsertRecords != null && relationModel.InsertRecords.Count > 0)
                            {
                                foreach (var item in relationModel.InsertRecords)
                                {
                                    var newid = await GetBurnFIleSEQID();
                                    item.CREATE_TIME = DateTime.Now;
                                    var resdata = await _dbConnection.ExecuteAsync(insertRelationSql, new
                                    {
                                        ID = newid,
                                        item.APPLY_ID,
                                        item.BURN_FILE_ID,
                                        STATUS = 0,
                                        item.USER_NAME,
                                        item.CREATE_TIME
                                    }, tran);
                                }
                            }
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
        /// 保存文件申请
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public async Task<decimal> SaveFileApplyDataByTrans(MesBurnFileApplyModel model)
        {
            int result = 1;
            ConnectionFactory.OpenConnection(_dbConnection);
            using (var tran = _dbConnection.BeginTransaction())
            {
                try
                {

                    #region 删除
                    string deleteSql = @"Delete from MES_BURN_FILE_APPLY where ID=:ID ";
                    if (model.RemoveRecords != null && model.RemoveRecords.Count > 0)
                    {
                        foreach (var item in model.RemoveRecords)
                        {
                            var select_Apply = "SELECT * FROM MES_BURN_FILE_APPLY WHERE ID=:ID";
                            string insertHistorySql = @"insert into MES_BURN_FILE_APPLY_HISTORY 
					(ID,APPLY_NO,PART_CODE,WO_NO,USER_NAME,CREATE_TIME,LOG_TIME,LOG_USER,MST_ID) 
					VALUES (:ID,:APPLY_NO,:PART_CODE,:WO_NO,:USER_NAME,:CREATE_TIME,:LOG_TIME,:LOG_USER,:MST_ID)";

                            var data = (await _dbConnection.QueryAsync<MesBurnFileApply>(select_Apply, new { ID = item.ID })).FirstOrDefault();
                            if (data != null)
                            {
                                #region 更新到记录表
                                var ID = await GetBurnFIleSEQID();
                                var resdata = await _dbConnection.ExecuteAsync(insertHistorySql, new
                                {
                                    ID = ID,
                                    data.APPLY_NO,
                                    data.PART_CODE,
                                    data.WO_NO,
                                    data.USER_NAME,
                                    data.CREATE_TIME,
                                    MST_ID = data.ID,
                                    LOG_TIME = item.CREATE_TIME,
                                    LOG_USER = item.USER_NAME,
                                }, tran);
                                #endregion

                                var resDelData = await _dbConnection.ExecuteAsync(deleteSql, new
                                {
                                    item.ID
                                }, tran);
                            }
                        }
                    }
                    #endregion

                    #region 更新模块
                    //string updateSql = @"Update MES_BURN_FILE_APPLY set APPLY_NO=:APPLY_NO,PART_CODE=:PART_CODE,WO_NO=:WO_NO,USER_NAME=:USER_NAME,CREATE_TIME=:CREATE_TIME,MODIFY_TIME=:MODIFY_TIME  
                    //where ID=:ID ";

                    if (model.UpdateRecords != null && model.UpdateRecords.Count > 0)
                    {
                        foreach (var item in model.UpdateRecords)
                        {
                            #region 文件申请更新方法
                            //var resdata = await _dbConnection.ExecuteAsync(updateSql, new
                            //{

                            //    item.ID,
                            //    item.APPLY_NO,
                            //    item.PART_CODE,
                            //    item.WO_NO,
                            //    item.USER_NAME,
                            //    item.CREATE_TIME,
                            //    item.MODIFY_TIME,
                            //    
                            //}, tran);
                            #endregion

                            #region 更新操作
                            var newid = await GetBurnFIleSEQID();
                            var select_Apply = "SELECT * FROM MES_BURN_FILE_APPLY WHERE ID=:ID";
                            string insertHistorySql = @"insert into MES_BURN_FILE_APPLY_HISTORY 
					(ID,APPLY_NO,PART_CODE,WO_NO,USER_NAME,CREATE_TIME,LOG_TIME,LOG_USER,MST_ID) 
					VALUES (:ID,:APPLY_NO,:PART_CODE,:WO_NO,:USER_NAME,:CREATE_TIME,:LOG_TIME,:LOG_USER,:MST_ID)";

                            var data = (await _dbConnection.QueryAsync<MesBurnFileApply>(select_Apply, new { ID = item.ID })).FirstOrDefault();
                            if (data != null)
                            {
                                #region 更新到记录表
                                var resdata = await _dbConnection.ExecuteAsync(insertHistorySql, new
                                {
                                    ID = newid,
                                    data.APPLY_NO,
                                    data.PART_CODE,
                                    data.WO_NO,
                                    data.USER_NAME,
                                    data.CREATE_TIME,
                                    MST_ID = data.ID,
                                    LOG_TIME = item.CREATE_TIME,
                                    LOG_USER = item.USER_NAME,
                                }, tran);
                                #endregion

                                #region 删除
                                string deleteApplySql = @"Delete from MES_BURN_FILE_APPLY where ID=:ID ";
                                var resDelData = await _dbConnection.ExecuteAsync(deleteApplySql, new
                                {
                                    item.ID
                                }, tran);
                                #endregion

                                #region 新增

                                string insertApplySql = @"insert into MES_BURN_FILE_APPLY 
					                                    (ID,APPLY_NO,PART_CODE,WO_NO,USER_NAME,CREATE_TIME) 
					                                    VALUES (:ID,:APPLY_NO,:PART_CODE,:WO_NO,:USER_NAME,to_date(:CREATE_TIME))";
                                var ID = await GetFileApplySEQID();
                                var code = string.Empty;
                                code = ID > 10000 ? ID.ToString() : ID.ToString().PadLeft(6, '0');
                                item.APPLY_NO = "AYN-" + code; // 一共6位,位数不够时从左边开始用0补;
                                item.CREATE_TIME = DateTime.Now;
                                var resApplydata = await _dbConnection.ExecuteAsync(insertApplySql, new
                                {
                                    ID = ID,
                                    item.APPLY_NO,
                                    item.PART_CODE,
                                    item.WO_NO,
                                    item.USER_NAME,
                                    item.CREATE_TIME
                                }, tran);

                                #endregion
                            }
                            #endregion
                        }
                    }
                    #endregion

                    #region 新增
                    string insertSql = @"insert into MES_BURN_FILE_APPLY 
					(ID,APPLY_NO,PART_CODE,WO_NO,USER_NAME,CREATE_TIME) 
					VALUES (:ID,:APPLY_NO,:PART_CODE,:WO_NO,:USER_NAME,:CREATE_TIME)";
                    if (model.InsertRecords != null && model.InsertRecords.Count > 0)
                    {
                        foreach (var item in model.InsertRecords)
                        {
                            var newid = await GetBurnFIleSEQID();
                            string conditions = "";
                            if (!item.WO_NO.IsNullOrWhiteSpace())
                            {
                                conditions += " AND WO_NO =:WO_NO ";
                            }
                            if (!item.PART_CODE.IsNullOrWhiteSpace())
                            {
                                conditions += " AND PART_CODE =:PART_CODE ";
                            }
                            var select_Apply = "SELECT * FROM MES_BURN_FILE_APPLY WHERE 1=1 " + conditions;
                            string insertHistorySql = @"insert into MES_BURN_FILE_APPLY_HISTORY 
					(ID,APPLY_NO,PART_CODE,WO_NO,USER_NAME,CREATE_TIME,LOG_TIME,LOG_USER,MST_ID) 
					VALUES (:ID,:APPLY_NO,:PART_CODE,:WO_NO,:USER_NAME,:CREATE_TIME,:LOG_TIME,:LOG_USER,:MST_ID)";

                            var data = (await _dbConnection.QueryAsync<MesBurnFileApply>(select_Apply, new { PART_CODE = item.PART_CODE, WO_NO = item.WO_NO })).FirstOrDefault();
                            if (data != null)
                            {
                                #region 更新到记录表

                                var historyResult = await _dbConnection.ExecuteAsync(insertHistorySql, new
                                {
                                    ID = newid,
                                    data.APPLY_NO,
                                    data.PART_CODE,
                                    data.WO_NO,
                                    data.USER_NAME,
                                    data.CREATE_TIME,
                                    MST_ID = data.ID,
                                    LOG_TIME = item.CREATE_TIME,
                                    LOG_USER = item.USER_NAME,
                                }, tran);

                                #endregion

                                #region 删除
                                string deleteApplySql = @"Delete from MES_BURN_FILE_APPLY where ID=:ID ";
                                var resDelData = await _dbConnection.ExecuteAsync(deleteApplySql, new
                                {
                                    item.ID
                                }, tran);
                                #endregion

                                #region 新增

                                string insertApplySql = @"insert into MES_BURN_FILE_APPLY 
					                                    (ID,APPLY_NO,PART_CODE,WO_NO,USER_NAME,CREATE_TIME) 
					                                    VALUES (:ID,:APPLY_NO,:PART_CODE,:WO_NO,:USER_NAME,:CREATE_TIME)";
                                //var ID = await GetFileApplySEQID();
                                //var code = string.Empty;
                                //code = ID > 10000 ? ID.ToString() : ID.ToString().PadLeft(6, '0');
                                //item.APPLY_NO = "AYN-" + code; // 一共6位,位数不够时从左边开始用0补;
                                item.CREATE_TIME = DateTime.Now;
                                var resApplydata = await _dbConnection.ExecuteAsync(insertApplySql, new
                                {
                                    item.ID,
                                    item.APPLY_NO,
                                    item.PART_CODE,
                                    item.WO_NO,
                                    item.USER_NAME,
                                    item.CREATE_TIME
                                }, tran);

                                #endregion
                            }
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
        /// 下载地址的保存
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public async Task<TableDataModel> DownAddressByTrans(BurnFileaddressModel model)
        {
            TableDataModel tabledata = new TableDataModel();

            //新增
            string insertFileDownSql = @"insert into MES_BURN_FILE_DOWN 
					(ID,WO_NO,APPLY_ID,USER_NAME,CREATE_TIME,DOWN_NO) 
					VALUES (:ID,:WO_NO,:APPLY_ID,:USER_NAME,:CREATE_TIME,:DOWN_NO)";
            string insertDownHistorySql = @"insert into MES_BURN_FILE_DOWN_HISTORY 
					(ID,MST_ID,FILE_NAME,FILE_LEN,FILE_TYPE,FILE_TIME) 
					VALUES (:ID,:MST_ID,:FILE_NAME,:FILE_LEN,:FILE_TYPE,:FILE_TIME)";
            ConnectionFactory.OpenConnection(_dbConnection);

            using (var tran = _dbConnection.BeginTransaction())
            {
                try
                {
                    if (model != null && model.DownLoad.Count() > 0)
                    {
                        //插入数据库
                        MesBurnFileDownModel downModel = new MesBurnFileDownModel();
                        downModel.InsertRecords = new List<MesBurnFileDownAddOrModifyModel>();
                        MesBurnFileDownAddOrModifyModel downAddOrModifyModel = new MesBurnFileDownAddOrModifyModel();
                        var applymodel = (await GetListByTableEX<MesBurnFileApply>("*", "MES_BURN_FILE_APPLY", "AND ID=:ID", new { ID = model.APPLY_ID })).FirstOrDefault();
                        downAddOrModifyModel.APPLY_ID = model.APPLY_ID;
                        downAddOrModifyModel.WO_NO = applymodel != null ? applymodel.WO_NO : "";
                        downAddOrModifyModel.USER_NAME = model.USER_NAME;
                        downAddOrModifyModel.CREATE_TIME = DateTime.Now;
                        var MSTID = await GetBurnFIleSEQID();
                        var code = string.Empty;
                        code = MSTID > 10000 ? MSTID.ToString() : MSTID.ToString().PadLeft(6, '0');
                        downAddOrModifyModel.DOWN_NO = "DW-" + code; // 一共6位,位数不够时从左边开始用0补;
                        var result = await _dbConnection.ExecuteAsync(insertFileDownSql, new
                        {
                            ID = MSTID,
                            downAddOrModifyModel.WO_NO,
                            downAddOrModifyModel.APPLY_ID,
                            downAddOrModifyModel.USER_NAME,
                            downAddOrModifyModel.CREATE_TIME,
                            downAddOrModifyModel.DOWN_NO,
                        }, tran);
                        string dir = @"upload\sfcsBurnFiles\DownFile\"; //+ DateTime.Now.ToString("yyyyMMdd") + @"\";
                        string webpath = $"/upload/sfcsBurnFiles/DownFile/"; //+ DateTime.Now.ToString("yyyyMMdd")+"/";
                        var pathWebRoot = AppContext.BaseDirectory + dir;
                        if (Directory.Exists(pathWebRoot) == false)
                        {
                            Directory.CreateDirectory(pathWebRoot);
                        }
                        foreach (var item in model.DownLoad)
                        {

                            var pathList = new List<string>();
                            string fullPath = string.Empty;
                            string newPath = string.Empty;

                            if (!item.Path.IsNullOrWhiteSpace())
                            {
                                var extension = string.Empty;
                                var oldName = string.Empty;
                                if (item.Type == _customerFile)
                                {
                                    string[] fileArry = item.Path.Split('.');

                                    //解析扩展名
                                    extension = Path.GetExtension(item.Path);

                                    //解析旧文件名
                                    oldName = Path.GetFileNameWithoutExtension(item.Path);

                                    //获取上级的文件夹
                                    DirectoryInfo pathInfo = new DirectoryInfo(item.Path);
                                    string parentDirectory = pathInfo.Parent.FullName;

                                    //新的文件路径 需要处理的
                                    string newfile = item.BURN_FILE_ID + "_" + oldName + extension;
                                    newPath = Path.Combine(pathWebRoot, newfile);
                                    System.IO.File.Copy(item.Path, newPath, true);
                                    fullPath = Path.Combine(webpath, newfile);
                                    pathList.Add(fullPath);
                                }
                                else
                                {
                                    newPath = AppContext.BaseDirectory + item.Path;
                                    if (System.IO.File.Exists(newPath))
                                    {
                                        pathList.Add(item.Path);
                                    }
                                }

                                if (result != -1)
                                {
                                    //插入数据库记录表
                                    MesBurnFileDownHistoryModel downHistoryModel = new MesBurnFileDownHistoryModel();
                                    downHistoryModel.InsertRecords = new List<MesBurnFileDownHistoryAddOrModifyModel>();
                                    MesBurnFileDownHistoryAddOrModifyModel downHistoryAddOrModifyModel = new MesBurnFileDownHistoryAddOrModifyModel();
                                    downHistoryAddOrModifyModel.MST_ID = MSTID;
                                    if (item.Type == _customerFile)
                                    {
                                        downHistoryAddOrModifyModel.FILE_NAME = oldName;//item.BURN_FILE_ID + "_" + oldName;
                                    }
                                    else if (item.Type == _cloudFile)
                                    {
                                        downHistoryAddOrModifyModel.FILE_NAME = Path.GetFileNameWithoutExtension(item.Path);
                                    }
                                    FileInfo info = new FileInfo(newPath);
                                    downHistoryAddOrModifyModel.FILE_LEN = (info.Length / 1024 / 1024).ToString();//MB
                                    downHistoryAddOrModifyModel.FILE_TYPE = Path.GetExtension(newPath).Split('.')[1];
                                    downHistoryAddOrModifyModel.FILE_TIME = info.CreationTime;
                                    var newid = await GetBurnFIleSEQID();
                                    var resdata = await _dbConnection.ExecuteAsync(insertDownHistorySql, new
                                    {
                                        ID = newid,
                                        downHistoryAddOrModifyModel.MST_ID,
                                        downHistoryAddOrModifyModel.FILE_NAME,
                                        downHistoryAddOrModifyModel.FILE_LEN,
                                        downHistoryAddOrModifyModel.FILE_TYPE,
                                        downHistoryAddOrModifyModel.FILE_TIME,
                                    }, tran);

                                    var data = resdata;
                                    if (data != -1)
                                    {
                                        tabledata.data = pathList;
                                    }
                                    else
                                    {
                                        tabledata.code = -1;
                                        tran.Rollback();
                                    }
                                }
                            }
                        }

                    }
                    tran.Commit();
                }
                catch (Exception ex)
                {
                    tabledata.code = -1;
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
            return tabledata;
        }

        /// <summary>
        /// 文件下载记录
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public async Task<TableDataModel> SaveFileDownDataByTrans(MesBurnFileDownModel model)
        {
            TableDataModel tableData = new TableDataModel();
            ConnectionFactory.OpenConnection(_dbConnection);
            using (var tran = _dbConnection.BeginTransaction())
            {
                try
                {

                    //删除
                    string deleteSql = @"Delete from MES_BURN_FILE_DOWN where ID=:ID ";
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

                    //更新
                    string updateSql = @"Update MES_BURN_FILE_DOWN set WO_NO=:WO_NO,APPLY_ID=:APPLY_ID,USER_NAME=:USER_NAME,CREATE_TIME=:CREATE_TIME,ATTRIBUTE6=:ATTRIBUTE6,ATTRIBUTE7=:ATTRIBUTE7,ATTRIBUTE8=:ATTRIBUTE8,ATTRIBUTE9=:ATTRIBUTE9,ATTRIBUTE10=:ATTRIBUTE10,DOWN_NO=:DOWN_NO  
						where ID=:ID ";
                    if (model.UpdateRecords != null && model.UpdateRecords.Count > 0)
                    {
                        foreach (var item in model.UpdateRecords)
                        {
                            var resdata = await _dbConnection.ExecuteAsync(updateSql, new
                            {
                                item.ID,
                                item.WO_NO,
                                item.APPLY_ID,
                                item.USER_NAME,
                                item.CREATE_TIME,
                                item.ATTRIBUTE6,
                                item.ATTRIBUTE7,
                                item.ATTRIBUTE8,
                                item.ATTRIBUTE9,
                                item.ATTRIBUTE10,
                                item.DOWN_NO,
                            }, tran);
                        }
                    }

                    //新增
                    string insertSql = @"insert into MES_BURN_FILE_DOWN 
					(ID,WO_NO,APPLY_ID,USER_NAME,CREATE_TIME,ATTRIBUTE6,ATTRIBUTE7,ATTRIBUTE8,ATTRIBUTE9,ATTRIBUTE10,DOWN_NO) 
					VALUES (:ID,:WO_NO,:APPLY_ID,:USER_NAME,:CREATE_TIME,:ATTRIBUTE6,:ATTRIBUTE7,:ATTRIBUTE8,:ATTRIBUTE9,:ATTRIBUTE10,:DOWN_NO)";
                    if (model.InsertRecords != null && model.InsertRecords.Count > 0)
                    {
                        foreach (var item in model.InsertRecords)
                        {
                            var ID = await GetBurnFIleSEQID();
                            var code = string.Empty;
                            code = ID > 10000 ? ID.ToString() : ID.ToString().PadLeft(6, '0');
                            item.DOWN_NO = "DW-" + code; // 一共6位,位数不够时从左边开始用0补;
                            tableData.data = ID;
                            var resdata = await _dbConnection.ExecuteAsync(insertSql, new
                            {
                                ID = ID,
                                item.WO_NO,
                                item.APPLY_ID,
                                item.USER_NAME,
                                item.CREATE_TIME,
                                item.ATTRIBUTE6,
                                item.ATTRIBUTE7,
                                item.ATTRIBUTE8,
                                item.ATTRIBUTE9,
                                item.ATTRIBUTE10,
                                item.DOWN_NO,
                            }, tran);
                        }
                    }



                    tran.Commit();
                }
                catch (Exception ex)
                {
                    tableData.code = -1;
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
            return tableData;
        }

        /// <summary>
		/// 保存申请文件关联表
		/// </summary>
		/// <param name="model"></param>
		/// <returns></returns>
		public async Task<decimal> SaveApplyRelationDataByTrans(MesBurnApplyRelationModel model)
        {
            int result = 1;
            ConnectionFactory.OpenConnection(_dbConnection);
            using (var tran = _dbConnection.BeginTransaction())
            {
                try
                {

                    //删除
                    string deleteSql = @"Delete from MES_BURN_APPLY_RELATION where ID=:ID ";
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

                    //更新
                    string updateSql = @"Update MES_BURN_APPLY_RELATION set APPLY_ID=:APPLY_ID,BURN_FILE_ID=:BURN_FILE_ID,STATUS=:STATUS,USER_NAME=:USER_NAME,MODIFY_TIME=:MODIFY_TIME  
						where ID=:ID ";
                    if (model.UpdateRecords != null && model.UpdateRecords.Count > 0)
                    {
                        foreach (var item in model.UpdateRecords)
                        {
                            item.MODIFY_TIME = DateTime.Now;
                            var resdata = await _dbConnection.ExecuteAsync(updateSql, new
                            {
                                item.ID,
                                item.APPLY_ID,
                                item.BURN_FILE_ID,
                                item.STATUS,
                                item.USER_NAME,
                                item.MODIFY_TIME
                            }, tran);
                        }
                    }

                    //新增
                    string insertSql = @"insert into MES_BURN_APPLY_RELATION 
					(ID,APPLY_ID,BURN_FILE_ID,STATUS,USER_NAME,CREATE_TIME) 
					VALUES (:ID,:APPLY_ID,:BURN_FILE_ID,:STATUS,:USER_NAME,:CREATE_TIME)";

                    if (model.InsertRecords != null && model.InsertRecords.Count > 0)
                    {
                        foreach (var item in model.InsertRecords)
                        {
                            var newid = await GetBurnFIleSEQID();
                            item.CREATE_TIME = DateTime.Now;
                            var resdata = await _dbConnection.ExecuteAsync(insertSql, new
                            {
                                ID = newid,
                                item.APPLY_ID,
                                item.BURN_FILE_ID,
                                STATUS = 0,
                                item.USER_NAME,
                                item.CREATE_TIME
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
		/// 保存下载历史记录数据
		/// </summary>
		/// <param name="model"></param>
		/// <returns></returns>
		public async Task<decimal> SaveFileDownHistoryByTrans(MesBurnFileDownHistoryModel model)
        {
            int result = 1;
            ConnectionFactory.OpenConnection(_dbConnection);
            using (var tran = _dbConnection.BeginTransaction())
            {
                try
                {
                    //新增
                    string insertSql = @"insert into MES_BURN_FILE_DOWN_HISTORY 
					(ID,MST_ID,FILE_NAME,FILE_LEN,FILE_TYPE,FILE_TIME) 
					VALUES (:ID,:MST_ID,:FILE_NAME,:FILE_LEN,:FILE_TYPE,:FILE_TIME)";
                    if (model.InsertRecords != null && model.InsertRecords.Count > 0)
                    {
                        foreach (var item in model.InsertRecords)
                        {
                            var newid = await GetBurnFIleSEQID();
                            var resdata = await _dbConnection.ExecuteAsync(insertSql, new
                            {
                                ID = newid,
                                item.MST_ID,
                                item.FILE_NAME,
                                item.FILE_LEN,
                                item.FILE_TYPE,
                                item.FILE_TIME,
                            }, tran);
                        }
                    }
                    //更新
                    string updateSql = @"Update MES_BURN_FILE_DOWN_HISTORY set MST_ID=:MST_ID,FILE_NAME=:FILE_NAME,FILE_LEN=:FILE_LEN,FILE_TYPE=:FILE_TYPE,FILE_TIME=:FILE_TIME
						where ID=:ID ";
                    if (model.UpdateRecords != null && model.UpdateRecords.Count > 0)
                    {
                        foreach (var item in model.UpdateRecords)
                        {
                            var resdata = await _dbConnection.ExecuteAsync(updateSql, new
                            {
                                item.ID,
                                item.MST_ID,
                                item.FILE_NAME,
                                item.FILE_LEN,
                                item.FILE_TYPE,
                                item.FILE_TIME,

                            }, tran);
                        }
                    }
                    //删除
                    string deleteSql = @"Delete from MES_BURN_FILE_DOWN_HISTORY where ID=:ID ";
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
		/// 保存SN记录数据
		/// </summary>
		/// <param name="model"></param>
		/// <returns></returns>
		/// <summary>
		/// 保存数据
		/// </summary>
		/// <param name="model"></param>
		/// <returns></returns>
		public async Task<decimal> SaveBurnSNByTrans(MesBurnSnDownModel model)
        {
            int result = 1;
            ConnectionFactory.OpenConnection(_dbConnection);
            using (var tran = _dbConnection.BeginTransaction())
            {
                try
                {
                    //删除
                    string deleteSql = @"Delete from MES_BURN_SN_DOWN where ID=:ID ";
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

                    //更新
                    string updateSql = @"Update MES_BURN_SN_DOWN set DOWN_ID=:DOWN_ID,DOWN_NO=:DOWN_NO,SN=:SN,USER_NAME=:USER_NAME,MODIFY_TIME=:MODIFY_TIME,APPLY_NO=:APPLY_NO  
						where ID=:ID ";
                    if (model.UpdateRecords != null && model.UpdateRecords.Count > 0)
                    {
                        foreach (var item in model.UpdateRecords)
                        {
                            item.MODIFY_TIME = DateTime.Now;
                            var resdata = await _dbConnection.ExecuteAsync(updateSql, new
                            {
                                item.ID,
                                item.DOWN_ID,
                                item.DOWN_NO,
                                item.SN,
                                item.USER_NAME,
                                item.MODIFY_TIME,
                                item.APPLY_NO,
                            }, tran);
                        }
                    }

                    //新增
                    string insertSql = @"insert into MES_BURN_SN_DOWN 
					(ID,DOWN_ID,DOWN_NO,SN,USER_NAME,CREATE_TIME,APPLY_NO) 
					VALUES (:ID,:DOWN_ID,:DOWN_NO,:SN,:USER_NAME,:CREATE_TIME,:APPLY_NO)";
                    string selectSql = "SELECT * FROM MES_BURN_SN_DOWN WHERE SN=:SN";
                    string insertLogSql = @"
                        			 INSERT INTO JZMES_LOG.MES_BURN_SN_DOWN 
                                     SELECT * FROM MES_BURN_SN_DOWN WHERE SN =:SN and DOWN_NO!=:DOWN_NO and APPLY_NO!=:APPLY_NO
                        ";
                    string delSql = @" DELETE FROM MES_BURN_SN_DOWN WHERE SN=:SN";
                    if (model.InsertRecords != null && model.InsertRecords.Count > 0)
                    {
                        foreach (var item in model.InsertRecords)
                        {
                            if ((await _dbConnection.QueryAsync<MesBurnSnDown>(selectSql, new { SN = item.SN })).Any())
                            {
                                await _dbConnection.ExecuteAsync(insertLogSql, new { SN = item.SN, DOWN_NO = item.DOWN_NO, APPLY_NO = item.APPLY_NO });
                                await _dbConnection.ExecuteAsync(delSql, new { SN = item.SN });
                            }

                            var newid = await GetBurnFIleSEQID();
                            item.CREATE_TIME = DateTime.Now;
                            var resdata = await _dbConnection.ExecuteAsync(insertSql, new
                            {
                                ID = newid,
                                item.DOWN_ID,
                                item.DOWN_NO,
                                item.SN,
                                item.USER_NAME,
                                item.CREATE_TIME,
                                item.APPLY_NO,
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
        /// 烧录结果保存到数据库
        /// </summary>
        /// <returns></returns>
        public async Task<Boolean> InsertBurnResult(MesBurnFileResultRequestModel model)
        {
            var result = false;
            try
            {
                string sql = @"INSERT INTO MES_BURN_RESULT_HISTORY(ID,HANDLERID ,  HANDLERMFR ,  HANDLERTYPE ,  HANDLERINFO ,  PROGRAMMERTYPE ,  ICTOTAL ,  ICPASS ,  ICFail ,  UPH ,  SOCKETNUMBER ,  MAXSOCKETNUMBER ,  YIELD ,  ESTIMATEUPH ,  ESTIMATEENDTIME ,  CYCLETIMEEACHIC ,  ERRORMESSAGE ,  ERRORCODE ,  LOTNUMBER ,  LOTSIZE ,  ICMANUFACTURE ,  ICTYPE ,  CONFIG ,  LASERCONTENT ,  LOTSTARTTIME ,  OPERATOR ,  LOTENDTIME ,  PRODUCTIONDATE ,  PRODUCTIME ,  ERRORTIME ,  BULK ,  CHESKSUM ,  WRITERPROG ,  NEXTPROCESSSTATE ,  NGAUTHORIZER ) 
                                                            VALUES (MES_BURN_RESULT_HISTORY_SEQ.NEXTVAL, :HANDLERID, :HANDLERMFR, :HANDLERTYPE, :HANDLERINFO, :PROGRAMMERTYPE, :ICTOTAL, :ICPASS ,  :ICFail ,  :UPH ,  :SOCKETNUMBER , :MAXSOCKETNUMBER ,:YIELD ,:ESTIMATEUPH ,:ESTIMATEENDTIME ,:CYCLETIMEEACHIC ,:ERRORMESSAGE ,:ERRORCODE ,:LOTNUMBER ,:LOTSIZE ,:ICMANUFACTURE ,:ICTYPE ,:CONFIG ,:LASERCONTENT ,:LOTSTARTTIME ,:OPERATOR ,:LOTENDTIME ,:PRODUCTIONDATE ,:PRODUCTIME ,:ERRORTIME ,:BULK ,:CHESKSUM ,:WRITERPROG ,:NEXTPROCESSSTATE ,:NGAUTHORIZER )";
               var effectNum= await _dbConnection.ExecuteAsync(sql);
                result = effectNum > 0 ? true : false;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return result;
        }

    }
}