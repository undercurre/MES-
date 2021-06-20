/*
*┌──────────────────────────────────────────────────────────────┐
*│　描    述：停线管控线别表接口实现                                                    
*│　作    者：嘉志科技                                            
*│　版    本：2.0    模板代码自动生成                                                
*│　创建时间：2020-08-24 11:51:57                             
*└──────────────────────────────────────────────────────────────┘
*┌──────────────────────────────────────────────────────────────┐
*│　命名空间： JZ.IMS.Repository.Oracle                                  
*│　类    名： MesStoplineLinesRepository                                      
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
    public class MesStoplineLinesRepository:BaseRepository<MesStoplineLines,Decimal>, IMesStoplineLinesRepository
    {
        public MesStoplineLinesRepository(IOptionsSnapshot<DbOption> options)
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
			string sql = "SELECT ENABLED FROM MES_STOPLINE_LINES WHERE ID=:ID";
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
			string sql = "UPDATE MES_STOPLINE_LINES set ENABLED=:ENABLED WHERE ID=:Id";
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
			string sql = "SELECT MES_STOPLINE_LINES_SEQ.NEXTVAL MY_SEQ FROM DUAL";
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
			string sql = "select count(0) from MES_STOPLINE_LINES where id = :id";
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
		public async Task<decimal> SaveDataByTrans(MesStoplineLinesModel model)
		{
			int result = 1;
			ConnectionFactory.OpenConnection(_dbConnection);
			using (var tran = _dbConnection.BeginTransaction())
			{
				try
				{
					//新增
					string insertSql = @"insert into MES_STOPLINE_LINES 
					(ID,PRODUCT_STOPLINE_ID,LINE_ID,LINE_TYPE,LINE_NAME,ORGANIZE_ID,ENABLED) 
					VALUES (:ID,:PRODUCT_STOPLINE_ID,:LINE_ID,:LINE_TYPE,:LINE_NAME,:ORGANIZE_ID,:ENABLED)";
					if (model.InsertRecords != null && model.InsertRecords.Count > 0)
					{
						foreach (var item in model.InsertRecords)
						{
							var newid = await GetSEQID();
							var resdata = await _dbConnection.ExecuteAsync(insertSql, new
							{
								ID = newid,
								item.PRODUCT_STOPLINE_ID,
								item.LINE_ID,
								item.LINE_TYPE,
								item.LINE_NAME,
								item.ORGANIZE_ID,
								item.ENABLED,
							}, tran);
						}
					}
					//更新
					string updateSql = @"Update MES_STOPLINE_LINES set PRODUCT_STOPLINE_ID=:PRODUCT_STOPLINE_ID,LINE_ID=:LINE_ID,LINE_TYPE=:LINE_TYPE,LINE_NAME=:LINE_NAME,ORGANIZE_ID=:ORGANIZE_ID,ENABLED=:ENABLED  
						where ID=:ID ";
					if (model.UpdateRecords != null && model.UpdateRecords.Count > 0)
					{
						foreach (var item in model.UpdateRecords)
						{
							var resdata = await _dbConnection.ExecuteAsync(updateSql, new
							{
								item.ID,
								item.PRODUCT_STOPLINE_ID,
								item.LINE_ID,
								item.LINE_TYPE,
								item.LINE_NAME,
								item.ORGANIZE_ID,
								item.ENABLED,
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
		/// 查询停线管控线别表数据(页面下方展示)
		/// </summary>
		/// <param name="model"></param>
		/// <returns></returns>
		public async Task<TableDataModel> LoadDataStopLines(MesStoplineLinesRequestModel model)
		{
			string conditions = "where  a.ID > 0 ";

			if (!model.STOPLINE_ID.IsNullOrWhiteSpace())
			{
				conditions += $"and (a.PRODUCT_STOPLINE_ID=:STOPLINE_ID)";
			}
			if (!model.LINE_ID.IsNullOrWhiteSpace())
			{
				conditions += $"and (a.LINE_ID=:LINE_ID)";
			}
			if (!model.LINE_TYPE.IsNullOrWhiteSpace())
			{
				conditions += $"and (a.LINE_TYPE=:LINE_TYPE)";
			}
			if (!model.ORGANIZE_ID.IsNullOrWhiteSpace())
			{
				conditions += $"and (a.ORGANIZE_ID=:ORGANIZE_ID)";
			}

			string sql = string.Format(@" select * from (
											select  ROWNUM as ROWNO, temp.* from (
												select   c.ORGANIZE_NAME ,a.* from MES_STOPLINE_LINES a
													left join SYS_ORGANIZE c on a.ORGANIZE_ID=c.ID {0} order by a.ID desc) temp)
									      WHERE ROWNO BETWEEN ((:Page-1)*:Limit+1) AND (:Limit*:Page) order by ROWNO asc ", conditions);

			//string pagedSql = SQLBuilderClass.GetPagedSQL(sql, " a.ID desc", conditions);
			var resdata = await _dbConnection.QueryAsync<object>(sql, model);
			string sqlcnt = @"select count(0) from MES_STOPLINE_LINES a " + conditions;

			int cnt = await _dbConnection.ExecuteScalarAsync<int>(sqlcnt, model);
			return new TableDataModel
			{
				count = cnt,
				data = resdata?.ToList(),
			};
		}
		
		
		/// <summary>
		/// 查询数据(新增停线管控线别表页面时)
		/// </summary>
		/// <param name="model"></param>
		/// <returns></returns>
		public async Task<TableDataModel> GetStopLinesToAdd(MesStoplineLinesRequestModel model)
		{
			string conditions = "and a.ID > 0 ";
			string strwhere = " 1=1 ";
			if (!model.STOPLINE_ID.IsNullOrWhiteSpace())
			{
				strwhere += $"and (d.PRODUCT_STOPLINE_ID=:STOPLINE_ID)";
			}
			if (!model.LINE_ID.IsNullOrWhiteSpace())
			{
				conditions += $"and (a.ID=:LINE_ID)";
			}
			if (!model.LINE_TYPE.IsNullOrWhiteSpace())
			{
				conditions += $"and (b.LOOKUP_CODE=:LINE_TYPE)";
			}
			if (!model.ORGANIZE_ID.IsNullOrWhiteSpace())
			{
				conditions += $"and (c.ID=:ORGANIZE_ID)";
			}

			string sql = string.Format(@" select * from (
											select ROWNUM as rowno , temp.* from (
												select a.ID as LINE_ID,a.LINE_NAME,b.CHINESE, c.ID as ORGANIZE_ID, c.ORGANIZE_NAME  from SMT_LINES a 
												left join SFCS_PARAMETERS b on b.LOOKUP_TYPE = 'MES_LINE_TYPE' and b.ENABLED='Y' and a.PLANT=b.LOOKUP_CODE
												left join SYS_ORGANIZE c on a.ORGANIZE_ID=c.ID 
												left join MES_STOPLINE_LINES d on   {0} and a.ID=d.LINE_ID
												where d.LINE_ID is null  {1}
												union all
												select a.ID as LINE_ID,a.OPERATION_LINE_NAME,b.CHINESE, c.ID as ORGANIZE_ID,c.ORGANIZE_NAME  from SFCS_OPERATION_LINES a 
												left join SFCS_PARAMETERS b on b.LOOKUP_TYPE = 'MES_LINE_TYPE' and b.ENABLED='Y' and a.PLANT_CODE=b.LOOKUP_CODE
												left join SYS_ORGANIZE c on a.ORGANIZE_ID=c.ID  
												left join MES_STOPLINE_LINES d on   {0} and a.ID=d.LINE_ID
												where a.ENABLED='Y' and d.LINE_ID is null {1}
											 ) temp  ) WHERE rowno BETWEEN ((:Page-1)*:Limit+1) AND (:Limit*:Page)", strwhere, conditions);

			var resdata = await _dbConnection.QueryAsync<object>(sql,model);
			string sqlcnt = string.Format(@" select count(0) from (
												select a.ID as LINE_ID,a.LINE_NAME,b.CHINESE, c.ID as ORGANIZE_ID, c.ORGANIZE_NAME  from SMT_LINES a 
												left join SFCS_PARAMETERS b on b.LOOKUP_TYPE = 'MES_LINE_TYPE' and b.ENABLED='Y' and a.PLANT=b.LOOKUP_CODE
												left join SYS_ORGANIZE c on a.ORGANIZE_ID=c.ID 
												left join MES_STOPLINE_LINES d on   {0} and a.ID=d.LINE_ID
												where d.LINE_ID is null  {1}
												union all
												select a.ID as LINE_ID,a.OPERATION_LINE_NAME,b.CHINESE, c.ID as ORGANIZE_ID,c.ORGANIZE_NAME  from SFCS_OPERATION_LINES a 
												left join SFCS_PARAMETERS b on b.LOOKUP_TYPE = 'MES_LINE_TYPE' and b.ENABLED='Y' and a.PLANT_CODE=b.LOOKUP_CODE
												left join SYS_ORGANIZE c on a.ORGANIZE_ID=c.ID  
												left join MES_STOPLINE_LINES d on   {0} and a.ID=d.LINE_ID
												where a.ENABLED='Y' and d.LINE_ID is null {1}
											 ) temp   ", strwhere,conditions);

			int cnt = await _dbConnection.ExecuteScalarAsync<int>(sqlcnt, model);

			return new TableDataModel
			{
				count = cnt,
				data = resdata?.ToList(),
			};
		}


	}
}