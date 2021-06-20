/*
*┌──────────────────────────────────────────────────────────────┐
*│　描    述：离线备料表接口实现                                                    
*│　作    者：嘉志科技                                            
*│　版    本：2.0    模板代码自动生成                                                
*│　创建时间：2020-07-15 13:47:58                             
*└──────────────────────────────────────────────────────────────┘
*┌──────────────────────────────────────────────────────────────┐
*│　命名空间： JZ.IMS.Repository.Oracle                                  
*│　类    名： MesOffLineReelRepository                                      
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
using JZ.IMS.ViewModels.OfflineMaterials;
using JZ.IMS.Core.Extensions;

namespace JZ.IMS.Repository.Oracle
{
    public class MesOffLineReelRepository : BaseRepository<MesOffLineReel, Decimal>, IMesOffLineReelRepository
    {
        public MesOffLineReelRepository(IOptionsSnapshot<DbOption> options)
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
            string sql = "SELECT ENABLED FROM MES_OFF_LINE_REEL WHERE ID=:ID";
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
            string sql = "UPDATE MES_OFF_LINE_REEL set ENABLED=:ENABLED WHERE ID=:Id";
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
            string sql = "SELECT MES_OFF_LINE_REEL_SEQ.NEXTVAL MY_SEQ FROM DUAL";
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
            string sql = "select count(0) from MES_OFF_LINE_REEL where id = :id";
            object result = await _dbConnection.ExecuteScalarAsync(sql, new
            {
                id
            });

            return (Convert.ToInt32(result) > 0);
        }


        /// <summary>
        /// 查看配置表是否存在备物事项
        /// </summary>
        /// <param name="lineType"></param>
        /// <returns></returns>
        public async Task<bool> IsCheckConfig(string lineType)
        {
            string sql = @" SELECT count(0) from MES_PRODUCTION_PRE_CONF WHERE CLASS_TYPE=:CLASS_TYPE AND ENABLED='Y' AND   CONTENT_TYPE = 1 ";
            var result = await _dbConnection.ExecuteScalarAsync(sql, new { CLASS_TYPE = lineType });
            return Convert.ToInt32(result) > 0;
        }

        /// <summary>
        /// 查找主表数据
        /// 根据线别 、生产时间、判断状态 、产前确认状态来查找
        /// </summary>
        /// <param name="lineType"></param>
        /// <returns></returns>
        public async Task<List<MesProductionPreMst>> GetProductPreMst(string LINE_ID)
        {
            string sql = @" SELECT * from MES_PRODUCTION_PRE_MST 
                            WHERE LINE_ID=:LINE_ID AND END_STATUS is null AND STATUS=:STATUS  ORDER BY PRODUCTION_TIME asc ";
            var result = await _dbConnection.QueryAsync<MesProductionPreMst>(sql,
                new
                {
                    LINE_ID = Convert.ToDecimal(LINE_ID),
                    STATUS = 0,//0:待上线，1:已上线，3:取消
                });
            return result?.ToList();
        }

        /// <summary>
        /// 检测子项是否有备料事项
        /// </summary>
        /// <returns></returns>
        public async Task<List<MesProductionPreDtl>> IsCheckProductPreDlt(decimal id)
        {
            string sql = @"select *
						   from mes_production_pre_dtl pd 
							   inner join MES_PRODUCTION_PRE_CONF pc on pd.conf_id = pc.id and pc.ENABLED='Y'
							   inner join SFCS_PARAMETERS sp on pc.content_type = sp.lookup_code and sp.lookup_type = 'PRODUCTION_PRE_TYPE'
						   where pd.mst_id =:id and pc.content_type=1 ";
            var redata = await _dbConnection.QueryAsync<MesProductionPreDtl>(sql, new { id });
            return redata?.ToList();
        }

        /// <summary>
        /// 通过条码获取物料
        /// reel_id:条码
        /// </summary>
        /// <returns></returns>
        public async Task<List<ImsPart>> GetImsPart(string reel_id)
        {
            string sql = @"select IPT.* from IMS_PART IPT
				 inner join IMS_REEL IRL on IRL.PART_ID=IPT.id
				 where IRl.code=:code";
            var result = await _dbConnection.QueryAsync<ImsPart>(sql, new { code = reel_id });
            return result?.ToList();
        }

        /// <summary>
        /// 获取所有的料号
        /// PRE_MST_NO:产前确认编号
        /// </summary>
        /// <returns></returns>
        public async Task<List<dynamic>> GetALLPartNo(string PRE_MST_NO)
        {
            string sql = @"select DISTINCT  IPT.CODE from IMS_REEL IRL
				 inner join IMS_PART IPT on IPT.ID=IRL.PART_ID
				 inner join MES_OFF_LINE_REEL OLR on  IRL.code=OLR.REEL_ID
                 where OLR.PRE_MST_NO=:PRE_MST_NO ";
            var result = await _dbConnection.QueryAsync<dynamic>(sql, new { PRE_MST_NO = PRE_MST_NO });
            return result?.ToList();
        }

        /// <summary>
        /// 获取BOM2的数据
        /// Part_No:产品编号
        /// </summary>
        /// <returns></returns>
        public async Task<List<SmtBom2>> GetBom2(string Part_No)
        {
            string sql = @" select b2.*from SMT_BOM1 b1,SMT_BOM2 b2 where b1.PARTENT_CODE =:PARTENT_CODE
            and B1.BOM_ID = B2.BOM_ID and b2.UNIT_QTY >0 and COMPONENT_LOCATION is not null ";
            var result = await _dbConnection.QueryAsync<SmtBom2>(sql, new { PARTENT_CODE = Part_No });
            return result?.ToList();
        }

        /// <summary>
        /// 保存产前确认子项
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public async Task<decimal> SaveDTLItme(MesProductionPreMstModel model)
        {
            int result = 1;
            decimal newid = 0;
            ConnectionFactory.OpenConnection(_dbConnection);
            using (var tran = _dbConnection.BeginTransaction())
            {
                try
                {
                    string updateSql = @"Update MES_PRODUCTION_PRE_DTL set  RESULT='Y'
						where ID=:ID ";
                    if (model.UpdateRecords != null && model.UpdateRecords.Count > 0)
                    {
                        foreach (var item in model.UpdateRecords)
                        {
                            var resdata = await _dbConnection.ExecuteAsync(updateSql, new
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
        /// 保存数据
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public async Task<decimal> SaveDataByTrans(MesOffLineReelModel model)
        {
            int result = 1;
            ConnectionFactory.OpenConnection(_dbConnection);
            using (var tran = _dbConnection.BeginTransaction())
            {
                try
                {
                    //新增
                    string insertSql = @"insert into MES_OFF_LINE_REEL 
					(ID,LINE_ID,FEEDER,FEEDER_TYPE,REEL_ID,STATUS,PREPARE_USER,PREPARE_TIME,USE_USER,USE_TIME,PRE_MST_NO) 
					VALUES (:ID,:LINE_ID,:FEEDER,:FEEDER_TYPE,:REEL_ID,:STATUS,:PREPARE_USER,:PREPARE_TIME,:USE_USER,:USE_TIME,:PRE_MST_NO)";
                    if (model.InsertRecords != null && model.InsertRecords.Count > 0)
                    {
                        foreach (var item in model.InsertRecords)
                        {
                            var newid = await GetSEQID();
                            var resdata = await _dbConnection.ExecuteAsync(insertSql, new
                            {
                                ID = newid,
                                item.LINE_ID,
                                item.FEEDER,
                                item.FEEDER_TYPE,
                                item.REEL_ID,
                                item.STATUS,
                                item.PREPARE_USER,
                                item.PREPARE_TIME,
                                item.USE_USER,
                                item.USE_TIME,
                                item.PRE_MST_NO
                            }, tran);
                        }
                    }
                    //更新
                    string updateSql = @"Update MES_OFF_LINE_REEL set LINE_ID=:LINE_ID,FEEDER=:FEEDER,FEEDER_TYPE=:FEEDER_TYPE,REEL_ID=:REEL_ID,STATUS=:STATUS,PREPARE_USER=:PREPARE_USER,PREPARE_TIME=:PREPARE_TIME,USE_USER=:USE_USER,USE_TIME=:USE_TIME  
						where ID=:ID ";
                    if (model.UpdateRecords != null && model.UpdateRecords.Count > 0)
                    {
                        foreach (var item in model.UpdateRecords)
                        {
                            var resdata = await _dbConnection.ExecuteAsync(updateSql, new
                            {
                                item.ID,
                                item.LINE_ID,
                                item.FEEDER,
                                item.FEEDER_TYPE,
                                item.REEL_ID,
                                item.STATUS,
                                item.PREPARE_USER,
                                item.PREPARE_TIME,
                                item.USE_USER,
                                item.USE_TIME,

                            }, tran);
                        }
                    }
                    //删除
                    string deleteSql = @"Delete from MES_OFF_LINE_REEL where ID=:ID ";
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
        /// 保存离线卸料
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public async Task<decimal> SaveDataOfflineUnloading(OfflineUnloadingModel model)
        {
            int result = 1;
            ConnectionFactory.OpenConnection(_dbConnection);
            using (var tran = _dbConnection.BeginTransaction())
            {
                try
                {
                    string condition = " WHERE LINE_ID=:LINE_ID ";
                    if (!model.KEY.IsNullOrEmpty())
                    {
                        condition += " AND (FEEDER=:KEY OR REEL_ID=:KEY) AND STATUS!=3 ";
                    }
                    
                    string select_Sql = "select * from MES_OFF_LINE_REEL " + condition;
                    var reelList = (await _dbConnection.QueryAsync<MesOffLineReel>(select_Sql, new { LINE_ID = model.LINE_ID, KEY = model.KEY })).ToList();

                    if (reelList.Count > 0)
                    {
                        //更新
                        string updateSql = @"Update MES_OFF_LINE_REEL SET STATUS=3  " + condition;
                        var resdata = await _dbConnection.ExecuteAsync(updateSql, model, tran);
                        tran.Commit();
                    }
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