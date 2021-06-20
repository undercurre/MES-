/*
*┌──────────────────────────────────────────────────────────────┐
*│　描    述：接口实现                                                    
*│　作    者：嘉志科技                                            
*│　版    本：2.0    模板代码自动生成                                                
*│　创建时间：2020-03-17 11:59:41                             
*└──────────────────────────────────────────────────────────────┘
*┌──────────────────────────────────────────────────────────────┐
*│　命名空间： JZ.IMS.Repository.Oracle                                  
*│　类    名： SmtLinesRepository                                      
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
using System.Linq;
using JZ.IMS.Core.Extensions;

namespace JZ.IMS.Repository.Oracle
{
    public class SmtLinesRepository : BaseRepository<SmtLines, Decimal>, ISmtLinesRepository
    {
        public SmtLinesRepository(IOptionsSnapshot<DbOption> options)
        {
            _dbOption = options.Get("iWMS");
            if (_dbOption == null)
            {
                throw new ArgumentNullException(nameof(DbOption));
            }
            _dbConnection = ConnectionFactory.CreateConnection(_dbOption.DbType, _dbOption.ConnectionString);
        }
        /// <summary>
        /// 机台默认值配置
        /// </summary>
        public class Config
        {
            public string config_type { get; set; }
            public string value { get; set; }
            public string description { get; set; }
        }

        /// <summary>
        /// 新增机台默认插入类型
        /// </summary>
        List<Config> stationConfList = new List<Config>() {
            new Config { config_type = "105", value= "PANASONIC",description= "MachineGroup" },
            new Config { config_type = "111", value= "5",description= "等待轨道中最大容量板子数，超出会报警" },
            new Config { config_type = "112", value= "15",description= "机器轨道中最大容量板子数，超出会报警" },
            new Config { config_type = "113", value= "001",description= "机台站编号" },
            new Config { config_type = "119", value= "1",description= "多台机合并在一起时，用一套Sensor来管控板子进出" },
            new Config { config_type = "120", value= "2",description= "进入机台感应器的工作模式" },
            new Config { config_type = "121", value= "2",description= "出机台感应器的工作模式" },
            //new Config{ config_type="122",value="5",description="用於管控主板SMT制件时间，超出系统报警"}
        };

        /// <summary>
        /// 根据主键获取激活状态
        /// </summary>
        /// <param name="id">主键</param>
        /// <returns></returns>
		public async Task<Boolean> GetEnableStatus(decimal id)
        {
            string sql = "SELECT ENABLED FROM SMT_LINES WHERE ID=:ID";
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
            string sql = "UPDATE SMT_LINES set ENABLED=:ENABLED WHERE ID=:Id";
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
            string sql = "SELECT SMT_LINES_SEQ.NEXTVAL MY_SEQ FROM DUAL";
            var result = await _dbConnection.ExecuteScalarAsync(sql);
            return (decimal)result;
        }

        /// <summary>
        /// 获取机台的SEQ
        /// </summary>
        /// <returns></returns>
        public async Task<decimal> GetSationSEQID()
        {
            string sql = " SELECT SMT_STATION_SEQ.NEXTVAL MY_SEQ FROM DUAL";
            var result = await _dbConnection.ExecuteScalarAsync(sql);
            return (decimal)result;
        }

        /// <summary>
		///线体配置
		/// </summary>
		/// <param name="id">项目id</param>
		/// <returns></returns>
		public async Task<bool> ItemIsByUsed(decimal id)
        {
            string sql = "select count(0) from SMT_LINE_CONFIG where LINE_ID = :id";
            object result = await _dbConnection.ExecuteScalarAsync(sql, new
            {
                id
            });

            return (Convert.ToInt32(result) > 0);
        }

        /// <summary>
        ///线体配置扩展
        /// </summary>
        /// <param name="id">项目id</param>
        /// <returns></returns>
        public async Task<decimal> GetConfigCount(decimal id)
        {
            string sql = "select count(0) from SMT_LINE_CONFIG where LINE_ID = :id";
            object result = await _dbConnection.ExecuteScalarAsync(sql, new
            {
                id
            });

            return Convert.ToInt32(result);
        }

        /// <summary>
        /// 查询SMT线别数据
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public async Task<TableDataModel> LoadSMTLinesData(SmtLinesRequestModel model)
        {
            string conditions = " WHERE m.ID > 0 ";
            if (!model.Key.IsNullOrWhiteSpace())
            {
                conditions += $"and (instr(m.LINE_NAME, :Key) > 0 or instr(m.LOCATION, :Key) > 0 or instr(m.PLANT, :Key) > 0) ";
            }
            string sql = @"SELECT ROWNUM AS ROWNO, M.ID, M.LINE_NAME, M.LOCATION, M.PLANT, OZ.ORGANIZE_NAME ORGANIZE_ID,M.ORGANIZE_ID O_ID      
				           FROM SMT_LINES M INNER JOIN (SELECT DISTINCT T.* FROM SYS_ORGANIZE T START WITH T.ID IN (SELECT ORGANIZE_ID FROM 
                             SYS_USER_ORGANIZE WHERE MANAGER_ID=:USER_ID) CONNECT BY PRIOR T.ID=T.PARENT_ORGANIZE_ID) OZ ON M.ORGANIZE_ID = OZ.ID ";
            string pagedSql = SQLBuilderClass.GetPagedSQL(sql, "m.id", conditions);
            var resdata = await _dbConnection.QueryAsync<object>(pagedSql, model);

            string sqlcnt = @" select count(0) from smt_lines m inner join (select distinct t.* from SYS_ORGANIZE t start with t.id in (select organize_id from 
                             sys_user_organize where manager_id=:USER_ID) connect by prior t.id=t.PARENT_ORGANIZE_ID) oz on m.organize_id = oz.ID " + conditions;
            int cnt = await _dbConnection.ExecuteScalarAsync<int>(sqlcnt, model);
            return new TableDataModel
            {
                count = cnt,
                data = resdata?.ToList(),
            };
        }

        /// <summary>
        /// 获取线线别列表
        /// </summary>
        /// <returns></returns>
        public async Task<List<object>> GetList(string USER_ID=null)
        {
            string sql = @" SELECT L.ID,L.LINE_NAME,L.LOCATION,L.PLANT, OZ.ORGANIZE_NAME ORGANIZE_ID,L.ORGANIZE_ID O_ID      FROM SMT_LINES L
                            INNER JOIN (SELECT DISTINCT T.* FROM SYS_ORGANIZE T START WITH T.ID IN (SELECT ORGANIZE_ID FROM 
                            SYS_USER_ORGANIZE WHERE MANAGER_ID=:USER_ID) CONNECT BY PRIOR T.ID=T.PARENT_ORGANIZE_ID) OZ ON L.ORGANIZE_ID = OZ.ID
                            WHERE UPPER(LINE_NAME) NOT  LIKE '%ASSEMB%'
                            AND UPPER(LINE_NAME) NOT  LIKE '%PRESS%'
                            AND UPPER(LINE_NAME) NOT  LIKE '%ICT%' 
                            ORDER BY LINE_NAME ";
            var result = await _dbConnection.QueryAsync<object>(sql,new { USER_ID= USER_ID });
            return result?.ToList();
        }

        /// <summary>
        /// 读取机台信息
        /// </summary>
        /// <param name="lineId">线别ID</param>
        /// <returns></returns>
        public async Task<List<object>> GetRoutStation(string lineId)
        {
            string sql = @"SELECT S.*
                           FROM SMT_ROUTE R, SMT_STATION S
                           WHERE R.STATION_ID = S.ID
                           AND R.ENABLED = 'Y'
                           AND S.ENABLED = 'Y'
                           AND R.LINE_ID =:ID
                           ORDER BY R.ORDER_NO ASC ";
            var result = await _dbConnection.QueryAsync<object>(sql, new { ID = lineId });
            return result?.ToList();
        }


        /// <summary>
        /// 获取线别配 置
        /// </summary>
        /// <param name="lineId"></param>
        /// <returns></returns>
        public async Task<List<object>> GetLinesconfig(string lineId)
        {
            string sql = @" SELECT LC.ID,LC.CONFIG_TYPE,LC.VALUE,LC.DESCRIPTION,LP.CN_DESC 
                            FROM SMT_LINE_CONFIG LC ,SMT_LOOKUP LP
                            WHERE LC.CONFIG_TYPE=LP.CODE  AND LP.TYPE = 'LINECONFIG'
                            AND  LC.LINE_ID=:ID";
            var result = await _dbConnection.QueryAsync<object>(sql, new { ID = lineId });
            return result?.ToList();
        }

        /// <summary>
        /// 获取机台配置
        /// </summary>
        /// <param name="stationid"></param>
        /// <returns></returns>
        public async Task<List<object>> GetStationconfig(string stationid)
        {
            string sql = @" SELECT * FROM SMT_STATION_CONFIG
                            WHERE  STATION_ID=:ID  ";
            var result = await _dbConnection.QueryAsync<object>(sql, new { ID = stationid });
            return result?.ToList();
        }


        /// <summary>
        /// 获取机台配置
        /// </summary>
        /// <param name="stationid"></param>
        /// <returns></returns>
        public async Task<List<SmtStationConfig>> GetStationconfigMachineName()
        {
            string sql = @" SELECT SSC.STATION_ID,SSC.VALUE FROM SMT_STATION_CONFIG SSC
                           WHERE SSC.ENABLED = 'Y' AND SSC.CONFIG_TYPE IN (
                           SELECT CODE FROM SMT_LOOKUP WHERE TYPE = 'MACHINECONFIG' AND VALUE = 'MACHINE NAME' AND ENABLED = 'Y') ";
            var result = await _dbConnection.QueryAsync<SmtStationConfig>(sql);
            return result?.ToList();
        }

        /// <summary>
        /// 获取字典的默认值
        /// </summary>
        /// <returns></returns>
        public async Task<List<IDNAME>> GetLoopUpDefualt(string code = "105,111,112,113,119,120,121,122")
        {
            string sql = @" SELECT  LP.CODE ID,LP.CN_DESC Name  FROM SMT_LOOKUP LP WHERE TYPE='MACHINECONFIG'AND CODE IN(:pramas) ORDER BY TYPE ";
            var result = await _dbConnection.QueryAsync<IDNAME>(sql, new { code });
            return result?.ToList();
        }

        /// <summary>
        /// 保存数据
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public async Task<decimal> SaveDataByTrans(SmtLinesModel model)
        {
            int result = 1;
            ConnectionFactory.OpenConnection(_dbConnection);
            using (var tran = _dbConnection.BeginTransaction())
            {
                try
                {
                    //新增
                    string insertSql = @"insert into SMT_LINES 
					(ID, LINE_NAME, LOCATION, PLANT, ORGANIZE_ID) 
					VALUES (:ID, :LINE_NAME, :LOCATION, :PLANT, :ORGANIZE_ID)";
                    if (model.insertRecords != null && model.insertRecords.Count > 0)
                    {
                        foreach (var item in model.insertRecords)
                        {
                            var newid = await GetID();
                            var resdata = await _dbConnection.ExecuteAsync(insertSql, new
                            {
                                ID = newid,
                                item.LINE_NAME,
                                item.LOCATION,
                                item.PLANT,
                                item.ORGANIZE_ID,
                            }, tran);
                        }
                    }
                    //更新
                    string updateSql = @"Update SMT_LINES set LINE_NAME=:LINE_NAME, LOCATION=:LOCATION, PLANT=:PLANT, ORGANIZE_ID=:ORGANIZE_ID   
						where ID=:ID ";
                    if (model.updateRecords != null && model.updateRecords.Count > 0)
                    {
                        foreach (var item in model.updateRecords)
                        {
                            var resdata = await _dbConnection.ExecuteAsync(updateSql, new
                            {
                                item.ID,
                                item.LINE_NAME,
                                item.LOCATION,
                                item.PLANT,
                                item.ORGANIZE_ID,
                            }, tran);
                        }
                    }
                    //删除
                    string deleteSql = @"Delete from SMT_LINES where ID=:ID ";
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
        /// 保存线体和机台
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public async Task<decimal> SaveLineAndStation(PatchlineconfigModel model)
        {
            int result = 1;
            ConnectionFactory.OpenConnection(_dbConnection);
            using (var tran = _dbConnection.BeginTransaction())
            {
                try
                {
                    #region 线体
                    //新增
                    string insertLinesSql = @"insert into SMT_LINES 
					(ID, LINE_NAME, LOCATION, PLANT,ORGANIZE_ID) 
					VALUES (:ID, :LINE_NAME, :LOCATION, :PLANT, :ORGANIZE_ID)";
                    if (model.insertLines != null && model.insertLines.Count > 0)
                    {
                        foreach (var item in model.insertLines)
                        {
                            var newid = await GetID();
                            var resdata = await _dbConnection.ExecuteAsync(insertLinesSql, new
                            {
                                ID = newid,
                                item.LINE_NAME,
                                item.LOCATION,
                                item.PLANT,
                                item.ORGANIZE_ID,
                            }, tran);
                        }
                    }
                    //更新
                    string updateLinesSql = @"Update SMT_LINES set LINE_NAME=:LINE_NAME, LOCATION=:LOCATION, PLANT=:PLANT, ORGANIZE_ID=:ORGANIZE_ID   
						where ID=:ID ";
                    if (model.updateLines != null && model.updateLines.Count > 0)
                    {
                        foreach (var item in model.updateLines)
                        {
                            var resdata = await _dbConnection.ExecuteAsync(updateLinesSql, new
                            {
                                item.ID,
                                item.LINE_NAME,
                                item.LOCATION,
                                item.PLANT,
                                item.ORGANIZE_ID,
                            }, tran);
                        }
                    }
                    //删除
                    string deleteLinesSql = @"Delete from SMT_LINES where ID=:ID ";
                    if (model.removeLines != null && model.removeLines.Count > 0)
                    {
                        foreach (var item in model.removeLines)
                        {
                            var resdata = await _dbConnection.ExecuteAsync(deleteLinesSql, new
                            {
                                item.ID
                            }, tran);
                        }
                    }

                    #endregion 线体

                    #region 修改线体配置信息
                    string insertLinesConfigSql = @"insert into SMT_LINE_CONFIG 
					(ID,LINE_ID,CONFIG_TYPE,VALUE,DESCRIPTION,ENABLED) 
					VALUES (:ID,:LINE_ID,:CONFIG_TYPE,:VALUE,:DESCRIPTION,:ENABLED)";
                    string updateLinesConfigSql = @"UPDATE SMT_LINE_CONFIG SET  VALUE=:VALUE,DESCRIPTION=:DESCRIPTION 
						WHERE LINE_ID=:LINE_ID AND CONFIG_TYPE=:CONFIG_TYPE ";
                    string delLineConfigSql = @" DELETE FROM SMT_LINE_CONFIG  WHERE LINE_ID=:LINE_ID ";
                    if (model.updateLineConfigs != null && model.updateLineConfigs.Count > 0)
                    {
                        foreach (var item in model.updateLineConfigs)
                        {
                            #region 更新
                            if (item.VALUE.IsNullOrWhiteSpace())
                            {
                                item.VALUE = " ";
                            }
                            var resdata = await _dbConnection.ExecuteAsync(updateLinesConfigSql, new
                            {
                                item.LINE_ID,
                                item.CONFIG_TYPE,
                                VALUE = item.VALUE ,
                                item.DESCRIPTION,
                            }, tran);
                            #endregion

                            #region 没有记录就插入
                            if (resdata <= 0)
                            {
                                var newid = await GetID();
                                await _dbConnection.ExecuteAsync(insertLinesConfigSql, new
                                {
                                    ID = newid,
                                    item.LINE_ID,
                                    item.CONFIG_TYPE,
                                    VALUE = item.VALUE,
                                    item.DESCRIPTION,
                                    ENABLED = "Y",
                                }, tran);
                            }
                            #endregion
                        }
                    }
                    #endregion

                    #region 机台

                    //新增
                    string insertStationsSql = @"INSERT INTO SMT_STATION 
					                            (ID,SMT_NAME,TYPE,DESCRIPTION,ENABLED) 
					                            VALUES (:ID,:SMT_NAME,:TYPE,:DESCRIPTION,:ENABLED)";
                    string insertRouteSql = @" INSERT INTO SMT_ROUTE 
                                               (ID,LINE_ID, STATION_ID, ORDER_NO, ENABLED)
                                                VALUES(:ID,:LINE_ID,:STATION_ID,:ORDER_NO,:ENABLED) ";
                    string maxNum = @" SELECT MAX(ORDER_NO) FROM SMT_ROUTE WHERE LINE_ID=:LINE_ID ";
                    string insertStationConfigSql = @" INSERT INTO SMT_STATION_CONFIG(ID,STATION_ID, CONFIG_TYPE, VALUE,DESCRIPTION,ENABLED)
                                                    VALUES(:ID, :STATION_ID, :CONFIG_TYPE, :VALUE, :DESCRIPTION,:ENABLED)";
                    if (model.insertStations != null && model.insertStations.Count > 0)
                    {
                        foreach (var item in model.insertStations)
                        {
                            #region 增加机台
                            var newid = await GetID("SEQ_ID");
                            await _dbConnection.ExecuteAsync(insertStationsSql, new
                            {
                                ID = newid,
                                item.SMT_NAME,
                                item.TYPE,
                                item.DESCRIPTION,
                                item.ENABLED
                            }, tran);

                            #endregion

                            #region 增加路由线体和机台关系
                            //查询路由的机台Order_No
                            object resultno = await _dbConnection.ExecuteScalarAsync(maxNum, new
                            {
                                item.Line_ID
                            });
                            var routid = await GetSEQ_ID();
                            if (Convert.ToInt32(resultno) > 0 && resultno != null)
                            {
                                await _dbConnection.ExecuteAsync(insertRouteSql, new
                                {
                                    ID = routid,
                                    item.Line_ID,
                                    STATION_ID = newid,
                                    ORDER_NO = Convert.ToInt32(resultno) + 1,
                                    ENABLED = "Y"
                                }, tran);
                            }
                            else
                            {
                                await _dbConnection.ExecuteAsync(insertRouteSql, new
                                {
                                    ID = routid,
                                    Line_ID = item.Line_ID,
                                    STATION_ID = newid,
                                    ORDER_NO = 1,
                                    ENABLED = "Y"
                                }, tran);
                            }
                            #endregion

                            #region 插入机台配置字段
                            foreach (var scitem in stationConfList)
                            {
                                var configid = await GetSEQ_ID();
                                await _dbConnection.ExecuteAsync(insertStationConfigSql, new
                                {
                                    ID = configid,
                                    STATION_ID = newid,
                                    CONFIG_TYPE = scitem.config_type,
                                    VALUE = scitem.value,
                                    DESCRIPTION = scitem.description,
                                    ENABLED = "Y",
                                }, tran);
                            }
                            #endregion
                        }
                    }
                    //更新
                    string updateStationsSql = @"Update SMT_STATION SET SMT_NAME=:SMT_NAME,TYPE=:TYPE,DESCRIPTION=:DESCRIPTION,ENABLED=:ENABLED  
						WHERE ID=:ID ";
                    if (model.updateStations != null && model.updateStations.Count > 0)
                    {
                        foreach (var item in model.updateStations)
                        {
                            var resdata = await _dbConnection.ExecuteAsync(updateStationsSql, new
                            {
                                item.ID,
                                item.SMT_NAME,
                                item.TYPE,
                                item.DESCRIPTION,
                                item.ENABLED,
                            }, tran);
                        }
                    }
                    //删除
                    string deleteStationsSql = @"Delete from SMT_STATION where ID=:ID ";
                    if (model.removeStations != null && model.removeStations.Count > 0)
                    {
                        foreach (var item in model.removeStations)
                        {
                            var resdata = await _dbConnection.ExecuteAsync(deleteStationsSql, new
                            {
                                item.ID
                            }, tran);
                        }
                    }
                    #endregion

                    #region 更新机台配置信息

                    string deleteStationConfigSql = @" DELETE FROM SMT_STATION_CONFIG WHERE STATION_ID=:STATION_ID ";
                    string updateStationconfigSql = @" UPDATE SMT_STATION_CONFIG
                                                       SET VALUE =:VALUE,DESCRIPTION=:DESCRIPTION
                                                       WHERE STATION_ID = :STATION_ID AND CONFIG_TYPE = :CONFIG_TYPE ";
                    if (model.updateStationConfig != null && model.updateStationConfig.Count > 0)
                    {
                        foreach (var scitem in model.updateStationConfig)
                        {

                            #region 先更新
                            if (scitem.VALUE.IsNullOrWhiteSpace())
                            {
                                scitem.VALUE = " ";
                            }
                            var resdata = await _dbConnection.ExecuteAsync(updateStationconfigSql, new
                            {
                                scitem.STATION_ID,
                                scitem.CONFIG_TYPE,
                                VALUE = scitem.VALUE,
                                scitem.DESCRIPTION,
                            }, tran);
                            #endregion

                            #region 再插入
                            if (resdata <= 0)
                            {
                                var configid = await GetSEQ_ID();
                                await _dbConnection.ExecuteAsync(insertStationConfigSql, new
                                {
                                    ID = configid,
                                    STATION_ID = scitem.STATION_ID,
                                    CONFIG_TYPE = scitem.CONFIG_TYPE,
                                    VALUE = scitem.VALUE,
                                    scitem.DESCRIPTION,
                                    ENABLED = "Y",
                                }, tran);
                            }
                            #endregion
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