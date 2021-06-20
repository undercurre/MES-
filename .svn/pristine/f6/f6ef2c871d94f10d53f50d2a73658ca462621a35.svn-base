/*
*┌──────────────────────────────────────────────────────────────┐
*│　描    述：镭雕任务下达表接口实现                                                    
*│　作    者：嘉志科技                                            
*│　版    本：2.0    模板代码自动生成                                                
*│　创建时间：2021-01-28 17:10:46                             
*└──────────────────────────────────────────────────────────────┘
*┌──────────────────────────────────────────────────────────────┐
*│　命名空间： JZ.IMS.Repository.Oracle                                  
*│　类    名： SfcsLaserTaskRepository                                      
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
    public class SfcsLaserTaskRepository : BaseRepository<SfcsLaserTask, Decimal>, ISfcsLaserTaskRepository
    {
        public SfcsLaserTaskRepository(IOptionsSnapshot<DbOption> options)
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
            string sql = "SELECT ENABLED FROM SFCS_LASER_TASK WHERE ID=:ID";
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
            string sql = "UPDATE SFCS_LASER_TASK set ENABLED=:ENABLED WHERE ID=:Id";
            return await _dbConnection.ExecuteAsync(sql, new
            {
                ENABLED = status ? 'Y' : 'N',
                Id = id,
            });
        }

        /// <summary>
        /// 获取表的序列
        /// </summary>
        /// <returns></returns>
		public async Task<decimal> GetSEQID()
        {
            string sql = "SELECT SFCS_LASER_TASK_SEQ.NEXTVAL MY_SEQ FROM DUAL";
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
            string sql = "select count(0) from SFCS_LASER_TASK where id = :id";
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
        public async Task<decimal> SaveDataByTrans(SfcsLaserTaskModel model)
        {
            int result = 1;
            ConnectionFactory.OpenConnection(_dbConnection);
            using (var tran = _dbConnection.BeginTransaction())
            {
                try
                {
                    //新增
                    string insertSql = @"insert into SFCS_LASER_TASK 
					(ID,TYPE_ID,WO_NO,PART_NO,PART_DESC,TASK_TYPE,PRINT_TOTAL,PRINT_QTY,PRINT_STATUS,CREATE_USER,CREATE_TIME,UPDATE_USER,UPDATE_TIME,ENABLED,MACHINE_CODE) 
					VALUES (:ID,:TYPE_ID,:WO_NO,:PART_NO,:PART_DESC,:TASK_TYPE,:PRINT_TOTAL,:PRINT_QTY,:PRINT_STATUS,:CREATE_USER,:CREATE_TIME,:UPDATE_USER,:UPDATE_TIME,:ENABLED,:MACHINE_CODE)";
                    if (model.InsertRecords != null && model.InsertRecords.Count > 0)
                    {
                        foreach (var item in model.InsertRecords)
                        {
                            var newid = await GetSEQID();
                            var resdata = await _dbConnection.ExecuteAsync(insertSql, new
                            {
                                ID = newid,
                                item.TYPE_ID,
                                item.WO_NO,
                                item.PART_NO,
                                item.PART_DESC,
                                item.TASK_TYPE,
                                item.PRINT_TOTAL,
                                item.PRINT_QTY,
                                item.PRINT_STATUS,
                                item.CREATE_USER,
                                item.CREATE_TIME,
                                item.UPDATE_USER,
                                item.UPDATE_TIME,
                                item.ENABLED,
                                item.MACHINE_CODE,

                            }, tran);
                        }
                    }
                    //更新
                    string updateSql = @"Update SFCS_LASER_TASK set TYPE_ID=:TYPE_ID,WO_NO=:WO_NO,PART_NO=:PART_NO,PART_DESC=:PART_DESC,TASK_TYPE=:TASK_TYPE,PRINT_TOTAL=:PRINT_TOTAL,PRINT_QTY=:PRINT_QTY,PRINT_STATUS=:PRINT_STATUS,CREATE_USER=:CREATE_USER,CREATE_TIME=:CREATE_TIME,UPDATE_USER=:UPDATE_USER,UPDATE_TIME=:UPDATE_TIME,ENABLED=:ENABLED,MACHINE_CODE=:MACHINE_CODE  
						where ID=:ID ";
                    if (model.UpdateRecords != null && model.UpdateRecords.Count > 0)
                    {
                        foreach (var item in model.UpdateRecords)
                        {
                            var resdata = await _dbConnection.ExecuteAsync(updateSql, new
                            {
                                item.ID,
                                item.TYPE_ID,
                                item.WO_NO,
                                item.PART_NO,
                                item.PART_DESC,
                                item.TASK_TYPE,
                                item.PRINT_TOTAL,
                                item.PRINT_QTY,
                                item.PRINT_STATUS,
                                item.CREATE_USER,
                                item.CREATE_TIME,
                                item.UPDATE_USER,
                                item.UPDATE_TIME,
                                item.ENABLED,
                                item.MACHINE_CODE,

                            }, tran);
                        }
                    }
                    //删除
                    string deleteSql = @"Delete from SFCS_LASER_TASK where ID=:ID ";
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
        /// 
        /// </summary>
        /// <param name="laserTaskResult"></param>
        /// <returns></returns>
        public async Task<LaserTaskResultListResponseModelEx> UpdateSnPcbNo(LaserTaskResultRequestModel laserTaskResult)
        {
            LaserTaskResultListResponseModelEx resultList = new LaserTaskResultListResponseModelEx();
            ConnectionFactory.OpenConnection(_dbConnection);
            using (var tran = _dbConnection.BeginTransaction())
            {
                try
                {

                    //resultList.DATETIME = laserTaskResult.DATETIME;
                    //resultList.FUN_TYPE = laserTaskResult.FUN_TYPE;
                    ResultDataListModel resultData = new ResultDataListModel();

                    string insertHeaderSql = @"INSERT INTO SMT_MULTIPANEL_HEADER(ID,BATCH_NO,WO_NO,MULT_NUMBER,LASER_RESULT,LASER_EXCEPTION)VALUES (:ID,'0',:WO_NO,:MULT_NUMBER,:LASER_RESULT,:LASER_EXCEPTION)";
                    string insertDetailSql = @"INSERT INTO SMT_MULTIPANEL_DETAIL(ID,MULT_HEADER_ID,SN,CREATETIME,TASK_NO,TASK_STATUS,TASK_MSG,TASK_ID)VALUES (:ID,:MULT_HEADER_ID,:SN,SYSDATE,:TASK_NO,:TASK_STATUS,:TASK_MSG,:TASK_ID)";

                    int header_id = await _dbConnection.ExecuteScalarAsync<int>("SELECT MULTIPANEL_SEQ.NEXTVAL MY_SEQ FROM DUAL");

                    await _dbConnection.ExecuteAsync(insertHeaderSql, new
                    {
                        ID = header_id,
                        WO_NO = laserTaskResult.WORK_ORDER,
                        MULT_NUMBER = laserTaskResult.PCB_LIST.Count(),
                        LASER_RESULT = laserTaskResult.LASER_RESULT,
                        LASER_EXCEPTION = laserTaskResult.LASER_EXCEPTION
                    }, tran);

                    int snPassQty = 0;
                    //更新打印列表
                    string updateTaskSql = @"UPDATE SFCS_LASER_TASK SET PRINT_STATUS=:PRINT_STATUS,PRINT_QTY=:PRINT_QTY,UPDATE_USER= :UPDATE_USER,UPDATE_TIME=SYSDATE,FAIL_QTY=NVL(FAIL_QTY,0)+:FAIL_QTY,SUCCESS_QTY=NVL(SUCCESS_QTY,0)+:SUCCESS_QTY WHERE ID=:ID ";
                    //更新工单流水号范围
                    string updateRangeSql = @"UPDATE SFCS_RUNCARD_RANGER SET PRINTED='Y' WHERE ID=:ID ";
                    //查询是否镭雕过数据
                    string multsql = "SELECT * FROM SMT_MULTIPANEL_DETAIL WHERE SN = :SN AND TASK_ID = :TASK_ID AND TASK_STATUS= '1' ";

                    var sntable = (await _dbConnection.QueryAsync<SmtMultipanelDetail>(multsql, new
                    {
                        SN = laserTaskResult.PCB_LIST[0].UNIT_SN,
                        TASK_ID = laserTaskResult.PCB_LIST[0].TASKID
                    }))?.ToList();

                    if (!sntable.IsNullOrWhiteSpace() && sntable.Count > 0)
                    {
                        //镭雕机上传的镭雕数据已经镭雕过!
                        throw new Exception("laser_been_carved");
                    }
                    List<String> taskList = new List<string>();
                    //子表添加数据
                    foreach (var model in laserTaskResult.PCB_LIST)
                    {
                        SNListModel snList = new SNListModel();
                        snList.NO = model.NO;
                        snList.TASKID = model.TASKID;
                        if (!taskList.Contains(model.TASKID))
                        {
                            taskList.Add(model.TASKID);
                        }
                        snList.UNIT_SN = model.UNIT_SN;
                        snList.STATUS = model.STATUS;
                        decimal detail_id = await _dbConnection.ExecuteScalarAsync<int>("SELECT MULTIPANEL_SEQ.NEXTVAL MY_SEQ FROM DUAL");
                        await _dbConnection.ExecuteAsync(insertDetailSql, new
                        {
                            ID = detail_id,
                            MULT_HEADER_ID = header_id,
                            SN = model.UNIT_SN,
                            TASK_NO = model.NO,
                            TASK_STATUS = model.STATUS,
                            TASK_MSG = model.MSG,
                            TASK_ID = model.TASKID
                        }, tran);

                        resultData.SNLIST.Add(snList);
                    }
                    foreach (String taskId in taskList)
                    {
                        String printTaskSql = @"select count(*) from  SMT_MULTIPANEL_DETAIL  where  TASK_ID = :TASK_ID ";
                        String taskSql = @"select * from  SFCS_LASER_TASK  where   ID = :ID ";
                        Decimal printTaskCount = await _dbConnection.ExecuteScalarAsync<decimal>(printTaskSql, new { TASK_ID = taskId });
                        var taskTable = (await _dbConnection.QueryAsync<SfcsLaserTask>(taskSql, new { ID = Decimal.Parse(taskId) }))?.ToList();
                        if (taskTable.IsNullOrWhiteSpace() || taskTable.Count <= 0)
                        {
                            //打印任务数据不存在!
                            throw new Exception("laser_print_not_exist");
                        }
                        SfcsLaserTask row = taskTable.FirstOrDefault();

                        //失败数量
                        var failQty = resultData.SNLIST.Count(c=>c.STATUS== GlobalVariables.CODE_FAIL&&c.TASKID==taskId);
                        //成功数量
                        var successQty = resultData.SNLIST.Count(c => c.STATUS == GlobalVariables.CODE_PASS && c.TASKID == taskId);
                        if (row.PRINT_TOTAL.ToString() == printTaskCount.ToString())
                        {
                            var effectNum = await _dbConnection.ExecuteAsync(updateTaskSql, new
                            {
                                PRINT_STATUS = "3",
                                PRINT_QTY = printTaskCount,
                                UPDATE_USER = laserTaskResult.USER_NAME,
                                ID = Decimal.Parse(taskId),
                                FAIL_QTY= failQty,
                                SUCCESS_QTY = successQty,
                            }, tran);

                            if (effectNum <= 0)
                                //未打印成功，不进行状态更新！
                                throw new Exception("laser_failure_successfully");

                            await _dbConnection.ExecuteAsync(updateRangeSql, new
                            {
                                ID = row.TYPE_ID
                            });
                        }
                        else
                        {
                            await _dbConnection.ExecuteAsync(updateTaskSql, new
                            {
                                PRINT_STATUS = 2,
                                PRINT_QTY = printTaskCount,
                                UPDATE_USER = laserTaskResult.USER_NAME,
                                ID = Decimal.Parse(taskId),
                                FAIL_QTY = failQty,
                                SUCCESS_QTY = successQty,
                            }, tran);
                        }
                    }

                    resultList.CODE = GlobalVariables.CODE_PASS;
                    //resultList.DATA.Add(resultData);
                    tran.Commit();
                }
                catch (Exception ex)
                {
                    tran.Rollback();
                    resultList.CODE = GlobalVariables.CODE_FAIL;
                    resultList.MSG = ex.Message;
                }
            }
            return resultList;
        }

    }
}