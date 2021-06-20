/**
*┌──────────────────────────────────────────────────────────────┐
*│　描    述：接口实现                                                    
*│　作    者：嘉志科技                                            
*│　版    本：2.0    模板代码自动生成                                                
*│　创建时间：2020-03-05 09:21:49                             
*└──────────────────────────────────────────────────────────────┘
*┌──────────────────────────────────────────────────────────────┐
*│　命名空间： JZ.IMS.Repository.Oracle                                  
*│　类    名： SmtStencilConfigRepository                                      
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
using JZ.IMS.Core.Extensions;
using System.Linq;
using System.Data;
using System.Collections.Generic;

namespace JZ.IMS.Repository.Oracle
{
    public class SmtStencilConfigRepository:BaseRepository<SmtStencilConfig,Decimal>, ISmtStencilConfigRepository
    {
        public SmtStencilConfigRepository(IOptionsSnapshot<DbOption> options)
        {
            _dbOption =options.Get("iWMS");
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
			string sql = "SELECT ENABLED FROM SMT_STENCIL_CONFIG WHERE ID=:ID";
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
			string sql = "UPDATE SMT_STENCIL_CONFIG set ENABLED=:ENABLED WHERE ID=:Id";
			return await _dbConnection.ExecuteAsync(sql, new
			{
				ENABLED = status ? 'Y' : 'N',
				Id = id,
			});
		}

		/// <summary>
		///钢网号是否已被使用 
		/// </summary>
		/// <param name="STENCIL_NO">钢网号</param>
		/// <param name="STENCIL_ID">钢网ID</param>
		/// <returns></returns>
		public async Task<bool> ItemIsByUsed(string STENCIL_NO, decimal STENCIL_ID)
		{
			int cnt = 0;
			object result = 0;
			//网板产品对照表 
			string sql = "select count(0) from SMT_STENCIL_PART where STENCIL_NO = :STENCIL_NO";
			result = await _dbConnection.ExecuteScalarAsync(sql, new
			{
				STENCIL_NO
			});

			cnt = Convert.ToInt32(result);
			if (cnt == 0)
			{
				//网板存储表
				sql = "SELECT COUNT(0) FROM SMT_STENCIL_STORE WHERE STENCIL_ID = :STENCIL_ID";
				result = await _dbConnection.ExecuteScalarAsync(sql, new
				{
					STENCIL_ID
				});
				cnt = Convert.ToInt32(result);
			}
			return (cnt > 0);
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="model"></param>
		/// <returns></returns>
		public async Task<TableDataModel> LoadData(SmtStencilConfigRequestModel model)
		{
			string conditions = " WHERE m.ID > 0 ";
			if (!model.Key.IsNullOrWhiteSpace())
			{
				conditions += $" AND (INSTR(M.STENCIL_NO, :KEY) > 0) ";
			}
			if (!model.CODE.IsNullOrWhiteSpace())
			{
				conditions += $" AND (INSTR(M.ATTRIBUTE4, :CODE) > 0) ";
			}
			if (!model.YEARS_CONTROL.IsNullOrWhiteSpace())
			{
				conditions += $" AND (INSTR(M.YEARS_CONTROL, :YEARS_CONTROL) > 0) ";
			}
			if (!model.OBJECT_ID.IsNullOrWhiteSpace())
			{
				conditions += $" AND M.ATTRIBUTE5=:OBJECT_ID ";
			}
			string sql = @" SELECT ROWNUM AS ROWNO, M.ID, M.STENCIL_NO, M.OPERATION_SITE_ID, M.PRODUCT_UNITAGE, M.ALARM_HOURS, M.STOP_HOURS, M.INTERVAL, 
							  M.MAX_USED_FLAG, M.DESCRIPTION, M.LAST_CLEAN_TIME, M.LAST_ALARM_TIME, M.ENABLED, M.CUSTOM_MAX_USED_COUNT, M.TENSION_CONTROL_FLAG, 
							  M.TENSION_CONTROL_VALUE,M.ATTRIBUTE1 LENGTH,M.ATTRIBUTE2 WIDTH,M.ATTRIBUTE3 THICHNESS,M.ATTRIBUTE4 CODE,M.ATTRIBUTE5 OBJECT_ID, LP.EN_DESC OBJECT_NAME,M.YEARS_CONTROL,OZ.ORGANIZE_NAME ORGANIZE_ID,M.ORGANIZE_ID O_ID     
				           FROM SMT_STENCIL_CONFIG M INNER JOIN (SELECT DISTINCT T.* FROM SYS_ORGANIZE T START WITH T.ID IN (SELECT ORGANIZE_ID FROM 
                             SYS_USER_ORGANIZE WHERE MANAGER_ID=:USER_ID) CONNECT BY PRIOR T.ID=T.PARENT_ORGANIZE_ID) OZ ON M.ORGANIZE_ID = OZ.ID
						   LEFT JOIN SMT_LOOKUP LP ON LP.TYPE = 'RESOURCE_OBJECT' AND LP.ENABLED = 'Y' AND M.ATTRIBUTE5=LP.CODE ";
			string pagedSql = SQLBuilderClass.GetPagedSQL(sql, "m.id", conditions);
			var resdata = await _dbConnection.QueryAsync<object>(pagedSql, model);

			string sqlcnt = @" SELECT COUNT(0) FROM SMT_STENCIL_CONFIG M INNER JOIN (SELECT DISTINCT T.* FROM SYS_ORGANIZE T START WITH T.ID IN (SELECT ORGANIZE_ID FROM 
                             SYS_USER_ORGANIZE WHERE MANAGER_ID=:USER_ID) CONNECT BY PRIOR T.ID=T.PARENT_ORGANIZE_ID) OZ ON M.ORGANIZE_ID = OZ.ID " + conditions;
			int cnt = await _dbConnection.ExecuteScalarAsync<int>(sqlcnt, model);
			return new TableDataModel
			{
				count = cnt,
				data = resdata?.ToList(),
			};
		}
		
		/// <summary>
		/// 产品编号
		/// STENCIL_NO--钢网编号
		///LOCATION  --钢网储位
		///PRINT_COUNT --印刷次数
		///PCB_SIDE	--板底/面
		/// </summary>
		/// <param name="Part_No"></param>
		/// <returns></returns>
		public async Task<TableDataModel> LoadDataPDA(SmtStencilPDARequestModel model)
        {
			string conditions = "WHERE 1=1";
			string sql = @"SELECT ROWNUM ROWNO,T.* FROM (SELECT STENCIL_NO,
							LOCATION , 
							sum(PRINT_COUNT) PRINT_COUNT,
							PCB_SIDE	
							 FROM(
							SELECT SSC.STENCIL_NO, SSS.LOCATION,NVL(SSR.PRINT_COUNT,0) PRINT_COUNT, 
							SSP.PCB_SIDE FROM 
							SMT_STENCIL_CONFIG SSC, 
							SMT_STENCIL_STORE SSS ,
							SMT_STENCIL_RUNTIME SSR,
							SMT_STENCIL_PART SSP
							WHERE SSC.ID = SSS.STENCIL_ID
							AND SSC.STENCIL_NO = SSR.STENCIL_NO(+)
							AND SSC.ENABLED = 'Y'
							AND SSC.STENCIL_NO = SSP.STENCIL_NO
							AND SSP.PART_NO =:PartNo )
							GROUP BY STENCIL_NO,LOCATION,PCB_SIDE) T";
			string pagedSql = SQLBuilderClass.GetPagedSQL(sql, "PRINT_COUNT", conditions);
			var resdata = await _dbConnection.QueryAsync<object>(pagedSql,model);
			string sqlcnt = @"SELECT COUNT(0) FROM (SELECT STENCIL_NO,
							LOCATION , 
							SUM(PRINT_COUNT) PRINT_COUNT, 
							PCB_SIDE	
							 FROM(
							SELECT SSC.STENCIL_NO, SSS.LOCATION,NVL(SSR.PRINT_COUNT,0) PRINT_COUNT, 
							SSP.PCB_SIDE FROM 
							SMT_STENCIL_CONFIG SSC, 
							SMT_STENCIL_STORE SSS ,
							SMT_STENCIL_RUNTIME SSR,
							SMT_STENCIL_PART SSP
							WHERE SSC.ID = SSS.STENCIL_ID
							AND SSC.STENCIL_NO = SSR.STENCIL_NO(+)
							AND SSC.ENABLED = 'Y'
							AND SSC.STENCIL_NO = SSP.STENCIL_NO
							AND SSP.PART_NO = :PARTNO )
							GROUP BY STENCIL_NO,LOCATION,PCB_SIDE) T " + conditions;
			int cnt = await _dbConnection.ExecuteScalarAsync<int>(sqlcnt, model);
			return new TableDataModel
			{
				count = cnt,
				data = resdata?.ToList(),
			};

		}

		/// <summary>
		/// 保存数据
		/// </summary>
		/// <param name="model"></param>
		/// <returns></returns>
		public async Task<decimal> SaveDataByTrans(SmtStencilConfigModel model)
		{
			int result = 1;
			ConnectionFactory.OpenConnection(_dbConnection);
			using (var tran = _dbConnection.BeginTransaction())
			{
				try
				{
					//新增
					string insertSql = @"INSERT INTO SMT_STENCIL_CONFIG
						(ID, STENCIL_NO, OPERATION_SITE_ID, PRODUCT_UNITAGE, ALARM_HOURS, STOP_HOURS, INTERVAL, MAX_USED_FLAG, DESCRIPTION, ENABLED, CUSTOM_MAX_USED_COUNT, 
						 TENSION_CONTROL_FLAG, TENSION_CONTROL_VALUE,ORGANIZE_ID,ATTRIBUTE1,ATTRIBUTE2,ATTRIBUTE3,ATTRIBUTE4,ATTRIBUTE5,YEARS_CONTROL) 
					    VALUES (:ID, :STENCIL_NO, :OPERATION_SITE_ID, :PRODUCT_UNITAGE, :ALARM_HOURS, :STOP_HOURS, :INTERVAL, :MAX_USED_FLAG, :DESCRIPTION, :ENABLED, 
						 :CUSTOM_MAX_USED_COUNT, :TENSION_CONTROL_FLAG, :TENSION_CONTROL_VALUE,:ORGANIZE_ID,:ATTRIBUTE1,:ATTRIBUTE2,:ATTRIBUTE3,:ATTRIBUTE4,:ATTRIBUTE5,:YEARS_CONTROL)";
					if (model.insertRecords != null && model.insertRecords.Count > 0)
					{
						foreach (var item in model.insertRecords)
						{
							var newid = await GetSEQ_ID();
							var resdata = await _dbConnection.ExecuteAsync(insertSql, new
							{
								ID = newid,
								item.STENCIL_NO,
								item.OPERATION_SITE_ID,
								item.PRODUCT_UNITAGE,
								item.ALARM_HOURS,
								item.STOP_HOURS,
								item.INTERVAL,
								item.MAX_USED_FLAG,
								item.DESCRIPTION,
								item.ENABLED,
								item.CUSTOM_MAX_USED_COUNT,
								item.TENSION_CONTROL_FLAG,
								item.TENSION_CONTROL_VALUE,
								item.ORGANIZE_ID,
								item.ATTRIBUTE1,
								item.ATTRIBUTE2,
								item.ATTRIBUTE3,
								item.ATTRIBUTE4,
								item.ATTRIBUTE5,
								item.YEARS_CONTROL
							}, tran);
						}
					}
					//更新
					string updateSql = @"update smt_stencil_config set STENCIL_NO =:STENCIL_NO, OPERATION_SITE_ID =:OPERATION_SITE_ID, PRODUCT_UNITAGE =:PRODUCT_UNITAGE,
							ALARM_HOURS =:ALARM_HOURS, STOP_HOURS =:STOP_HOURS, INTERVAL =:INTERVAL, MAX_USED_FLAG =:MAX_USED_FLAG,  DESCRIPTION =:DESCRIPTION, 
							ENABLED =:ENABLED, CUSTOM_MAX_USED_COUNT =:CUSTOM_MAX_USED_COUNT, TENSION_CONTROL_FLAG =:TENSION_CONTROL_FLAG,
							TENSION_CONTROL_VALUE =:TENSION_CONTROL_VALUE, ORGANIZE_ID=:ORGANIZE_ID,ATTRIBUTE1=:ATTRIBUTE1,ATTRIBUTE2=:ATTRIBUTE2,ATTRIBUTE3=:ATTRIBUTE3,ATTRIBUTE4=:ATTRIBUTE4,ATTRIBUTE5=:ATTRIBUTE5,YEARS_CONTROL=:YEARS_CONTROL  
						where ID=:ID ";
					if (model.updateRecords != null && model.updateRecords.Count > 0)
					{
						foreach (var item in model.updateRecords)
						{
							var resdata = await _dbConnection.ExecuteAsync(updateSql, new
							{
								item.ID,
								item.STENCIL_NO,
								item.OPERATION_SITE_ID,
								item.PRODUCT_UNITAGE,
								item.ALARM_HOURS,
								item.STOP_HOURS,
								item.INTERVAL,
								item.MAX_USED_FLAG,
								item.DESCRIPTION,
								item.ENABLED,
								item.CUSTOM_MAX_USED_COUNT,
								item.TENSION_CONTROL_FLAG,
								item.TENSION_CONTROL_VALUE,
								item.ORGANIZE_ID,
								item.ATTRIBUTE1,
								item.ATTRIBUTE2,
								item.ATTRIBUTE3,
								item.ATTRIBUTE4,
								item.ATTRIBUTE5,
								item.YEARS_CONTROL
							}, tran);
						}
					}
					//删除
					string deleteSql = @"delete from smt_stencil_config where ID=:ID ";
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
        /// 保存钢网资源数据
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public async Task<int> SaveStencilResourceInfo(SmtStencilResourceListModel model)
        {
            int result = 0;
            ConnectionFactory.OpenConnection(_dbConnection);
            using (var tran = _dbConnection.BeginTransaction())
            {
                try
                {
                    decimal resourceId = QueryEx<decimal>("SELECT SMT_STENCIL_RESOURCE_SEQ.NEXTVAL MY_SEQ FROM DUAL").FirstOrDefault();
                    string insertSql = @"INSERT INTO SMT_STENCIL_RESOURCE (ID, STENCIL_ID, RESOURCE_URL, RESOURCE_NAME, RESOURCE_SIZE,UPLOAD_OPER,CREATE_TIME ) VALUES (:ID, :STENCIL_ID, :RESOURCE_URL, :RESOURCE_NAME, :RESOURCE_SIZE, :UPLOAD_OPER, SYSDATE)";
                    result += await _dbConnection.ExecuteAsync(insertSql, new
                    {
                        ID = resourceId,
                        STENCIL_ID = model.STENCIL_ID,
                        RESOURCE_URL = model.RESOURCE_URL,
                        RESOURCE_NAME = model.RESOURCE_NAME,
                        RESOURCE_SIZE = model.RESOURCE_SIZE,
                        UPLOAD_OPER = model.UPLOAD_OPER
                    }, tran);

                    tran.Commit();
                }
                catch (Exception ex)
                {
                    result = -1;
                    tran.Rollback();//回滚事务
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
		/// 根据钢网的资源ID获取钢网资源数据
		/// </summary>
		/// <param name="resource_id">钢网的资源ID</param>
		/// <returns></returns>
		public async Task<SmtStencilResourceListModel> GetStencilResourceInfo(string resource_id)
        {
            string sQuery = @"SELECT * FROM SMT_STENCIL_RESOURCE WHERE ID=:ID ORDER BY CREATE_TIME DESC ";
            return (await _dbConnection.QueryAsync<SmtStencilResourceListModel>(sQuery, new { ID = resource_id })).FirstOrDefault();
        }

        /// <summary>
        /// 获取钢网资源数据信息列表
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public async Task<IEnumerable<SmtStencilResourceListModel>> GetStencilResourceList(SmtStencilResourceRequestModel model)
        {
            int page = 0, limit = 0;
            page = model.Page * model.Limit - model.Limit + 1;
            limit = model.Page * model.Limit;
            model.Page = page;
            model.Limit = limit;

            string sWhere = "";
            if (!string.IsNullOrEmpty(model.RESOURCE_NAME))
            {
                sWhere += " AND INSTR(RESOURCE_NAME,:RESOURCE_NAME)>0 ";
            }

            string sQuery = string.Format("SELECT * FROM (SELECT ROWNUM R, T.* FROM ( SELECT * FROM SMT_STENCIL_RESOURCE WHERE STENCIL_ID=:STENCIL_ID {0} ORDER BY CREATE_TIME DESC) T) WHERE R BETWEEN :Page AND :Limit", sWhere);

            return await _dbConnection.QueryAsync<SmtStencilResourceListModel>(sQuery, model);
        }

        /// <summary>
        /// 获取钢网资源数据信息列表条数
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public async Task<int> GetStencilResourceListCount(SmtStencilResourceRequestModel model)
        {
            string sWhere = "";
            if (!string.IsNullOrEmpty(model.RESOURCE_NAME))
            {
                sWhere += " AND INSTR(RESOURCE_NAME,:RESOURCE_NAME)>0 ";
            }

            string sQuery = string.Format("SELECT COUNT(1) FROM SMT_STENCIL_RESOURCE WHERE STENCIL_ID=:STENCIL_ID {0} ORDER BY CREATE_TIME DESC", sWhere);
            return await _dbConnection.ExecuteScalarAsync<int>(sQuery, model);
        }
    }
}