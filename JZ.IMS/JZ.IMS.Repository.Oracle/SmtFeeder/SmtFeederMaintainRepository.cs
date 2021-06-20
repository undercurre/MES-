/**
*┌──────────────────────────────────────────────────────────────┐
*│　描    述：接口实现                                                    
*│　作    者：嘉志科技                                            
*│　版    本：2.0    模板代码自动生成                                                
*│　创建时间：2020-03-17 08:55:48                             
*└──────────────────────────────────────────────────────────────┘
*┌──────────────────────────────────────────────────────────────┐
*│　命名空间： JZ.IMS.Repository.Oracle                                  
*│　类    名： SmtFeederMaintainRepository                                      
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

namespace JZ.IMS.Repository.Oracle
{
    public class SmtFeederMaintainRepository : BaseRepository<SmtFeederMaintain, Decimal>, ISmtFeederMaintainRepository
    {
        public SmtFeederMaintainRepository(IOptionsSnapshot<DbOption> options)
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
            string sql = "SELECT ENABLED FROM SMT_FEEDER_MAINTAIN WHERE ID=:ID";
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
            string sql = "UPDATE SMT_FEEDER_MAINTAIN set ENABLED=:ENABLED WHERE ID=:Id";
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
            string sql = "SELECT SMT_FEEDER_MAINTAIN_SEQ.NEXTVAL MY_SEQ FROM DUAL";
            var result = await _dbConnection.ExecuteScalarAsync(sql);
            return (decimal)result;
        }

        /// <summary>
        ///料架编号FEEDER 判断是否存在
        /// </summary>
        /// <param name="FEEDER">料架编号</param>
        /// <returns></returns>
        public async Task<SmtFeeder> ItemIsByFeeder(string FEEDER)
        {
            string sql = "SELECT * FROM smt_feeder WHERE FEEDER =:FEEDER ";
            var result = await _dbConnection.QueryAsync<SmtFeeder>(sql, new
            {
                FEEDER = FEEDER
            });
            return result?.FirstOrDefault();
        }

        /// <summary>
        ///获取飞达信息By ID
        /// </summary>
        /// <param name="id">料架编号ID</param>
        /// <returns></returns>
        public async Task<SmtFeeder> GetSmtFeederByFeederID(decimal? ID)
        {
            string sql = "SELECT * FROM SMT_FEEDER WHERE ID = :ID ";
            var result = await _dbConnection.QueryAsync<SmtFeeder>(sql, new
            {
                ID
            });
            return result?.FirstOrDefault();
        }

        /// <summary>
        ///获取维修明细
        /// </summary>
        /// <param name="id">料架编号</param>
        /// <returns></returns>
        public async Task<TableDataModel> GetFedderMaintainList(SmtFeederIDModel model)
        {
            string sql = @"SELECT ROWNUM AS ROWNO, SF.FEEDER FEEDER_NAME,
						 SFM.ID,
						 SFM.USED_COUNT,
						 SFM.FEEDER_ID,
						 CASE SFM.MAINTAIN_KIND
						 WHEN 1 THEN '已保养'
						 WHEN 2 THEN '已校正'
						 WHEN 3 THEN '已保养+校正'
						 END AS MAINTAIN_KIND,
						 SFM.MAINTAIN_BY,
						 SFM.DESCRIPTION,
						 SFM.MAINTAIN_TIME
						 FROM SMT_FEEDER SF, SMT_FEEDER_MAINTAIN SFM
						 WHERE SF.ID = SFM.FEEDER_ID AND SFM.FEEDER_ID = :FEEDER_ID";
            string pagedSql = SQLBuilderClass.GetPagedSQL(sql, " SFM.ID DESC ", "");
            var resdata = await _dbConnection.QueryAsync<SmtFeederMaintainListModel>(pagedSql, model);


            string sqlcnt = @"SELECT COUNT(0) FROM SMT_FEEDER SF, SMT_FEEDER_MAINTAIN SFM
						    WHERE SF.ID = SFM.FEEDER_ID AND SFM.FEEDER_ID = :FEEDER_ID ";

            int cnt = await _dbConnection.ExecuteScalarAsync<int>(sqlcnt, new
            {
                model.FEEDER_ID
            });

            return new TableDataModel
            {
                count = cnt,
                data = resdata?.ToList(),
            };

  
        }


        //public async Task<TableDataModel> GetReasionList(SmtFeederMaintainListModel model) 
        //{
            
        //}


        /// <summary>
        /// 保存数据
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public async Task<decimal> SaveDataByTrans(SmtFeederMaintainAddOrModifyModel model)
        {
            int result = 1;
            ConnectionFactory.OpenConnection(_dbConnection);
            using (var tran = _dbConnection.BeginTransaction())
            {
                try
                {
                    var tempdata = await GetSmtFeederByFeederID(model.FEEDER_ID);
                    if (tempdata!=null)
                    {
                        //新增
                        string insertSql = @"insert into SMT_FEEDER_MAINTAIN 
					(ID,FEEDER_ID,USED_COUNT,MAINTAIN_KIND,MAINTAIN_BY,DESCRIPTION,MAINTAIN_TIME) 
					VALUES (:ID,:FEEDER_ID,:USED_COUNT,:MAINTAIN_KIND,:MAINTAIN_BY,:DESCRIPTION,SYSDATE)";
                        var newid = await GetID();
                        var resdata = await _dbConnection.ExecuteAsync(insertSql, new
                        {
                            ID = newid,
                            model.FEEDER_ID,
                            USED_COUNT=tempdata.TOTAL_USED_COUNT,
                            model.MAINTAIN_KIND,
                            model.MAINTAIN_BY,//对应用户名
                            model.DESCRIPTION,
                        }, tran);
                        //更新操作
                        string updateSql = @" Update smt_feeder set CHECK_USED_COUNT=:CHECK_USED_COUNT,EMEND_USED_COUNT=:EMEND_USED_COUNT,LAST_CHECK_TIME=SYSDATE
                                         where ID=:ID ";
                        await _dbConnection.ExecuteAsync(updateSql, new
                        {
                            ID=tempdata.ID,
                            CHECK_USED_COUNT = (model.MAINTAIN_KIND == 1 || model.MAINTAIN_KIND == 3) ? tempdata.CHECK_USED_COUNT + 1 : tempdata.CHECK_USED_COUNT,
                            EMEND_USED_COUNT = (model.MAINTAIN_KIND == 2 || model.MAINTAIN_KIND == 3) ? tempdata.EMEND_USED_COUNT + 1 : tempdata.CHECK_USED_COUNT,
                        }, tran);
                    }

                    //              //更新
                    //              string updateSql = @"Update SMT_FEEDER_MAINTAIN set FEEDER_ID=:FEEDER_ID,USED_COUNT=:USED_COUNT,MAINTAIN_KIND=:MAINTAIN_KIND,MAINTAIN_BY=:MAINTAIN_BY,DESCRIPTION=:DESCRIPTION,MAINTAIN_TIME=:MAINTAIN_TIME  
                    //where ID=:ID ";
                    //              if (model.updateRecords != null && model.updateRecords.Count > 0)
                    //              {
                    //                  foreach (var item in model.updateRecords)
                    //                  {
                    //                      var resdata = await _dbConnection.ExecuteAsync(updateSql, new
                    //                      {
                    //                          item.ID,
                    //                          item.FEEDER_ID,
                    //                          item.USED_COUNT,
                    //                          item.MAINTAIN_KIND,
                    //                          item.MAINTAIN_BY,
                    //                          item.DESCRIPTION,
                    //                          item.MAINTAIN_TIME,

                    //                      }, tran);
                    //                  }
                    //              }
                    //              //删除
                    //              string deleteSql = @"Delete from SMT_FEEDER_MAINTAIN where ID=:ID ";
                    //              if (model.removeRecords != null && model.removeRecords.Count > 0)
                    //              {
                    //                  foreach (var item in model.removeRecords)
                    //                  {
                    //                      var resdata = await _dbConnection.ExecuteAsync(deleteSql, new
                    //                      {
                    //                          item.ID
                    //                      }, tran);
                    //                  }
                    //              }

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