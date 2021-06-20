/*
*┌──────────────────────────────────────────────────────────────┐
*│　描    述：停线管控产品表接口实现                                                    
*│　作    者：嘉志科技                                            
*│　版    本：2.0    模板代码自动生成                                                
*│　创建时间：2020-08-24 15:19:35                             
*└──────────────────────────────────────────────────────────────┘
*┌──────────────────────────────────────────────────────────────┐
*│　命名空间： JZ.IMS.Repository.Oracle                                  
*│　类    名： MesStoplinePnRepository                                      
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
    public class MesStoplinePnRepository:BaseRepository<MesStoplinePn,Decimal>, IMesStoplinePnRepository
    {
        public MesStoplinePnRepository(IOptionsSnapshot<DbOption> options)
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
			string sql = "SELECT ENABLED FROM MES_STOPLINE_PN WHERE ID=:ID";
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
			string sql = "UPDATE MES_STOPLINE_PN set ENABLED=:ENABLED WHERE ID=:Id";
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
			string sql = "SELECT MES_STOPLINE_PN_SEQ.NEXTVAL MY_SEQ FROM DUAL";
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
			string sql = "select count(0) from MES_STOPLINE_PN where id = :id";
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
		public async Task<decimal> SaveDataByTrans(MesStoplinePnModel model)
		{
			int result = 1;
			ConnectionFactory.OpenConnection(_dbConnection);
			using (var tran = _dbConnection.BeginTransaction())
			{
				try
				{
					//新增
					string insertSql = @"insert into MES_STOPLINE_PN 
					(ID,PRODUCT_STOPLINE_ID,PART_NO,MODEL,ENABLED) 
					VALUES (:ID,:PRODUCT_STOPLINE_ID,:PART_NO,:MODEL,:ENABLED)";
					if (model.InsertRecords != null && model.InsertRecords.Count > 0)
					{
						foreach (var item in model.InsertRecords)
						{
							var newid = await GetSEQID();
							var resdata = await _dbConnection.ExecuteAsync(insertSql, new
							{
								ID = newid,
								item.PRODUCT_STOPLINE_ID,
								item.PART_NO,
								item.MODEL,
								item.ENABLED,
							}, tran);
						}
					}
					//更新
					string updateSql = @"Update MES_STOPLINE_PN set PRODUCT_STOPLINE_ID=:PRODUCT_STOPLINE_ID,PART_NO=:PART_NO,MODEL=:MODEL,ENABLED=:ENABLED  
						where ID=:ID ";
					if (model.UpdateRecords != null && model.UpdateRecords.Count > 0)
					{
						foreach (var item in model.UpdateRecords)
						{
							var resdata = await _dbConnection.ExecuteAsync(updateSql, new
							{
								item.ID,
								item.PRODUCT_STOPLINE_ID,
								item.PART_NO,
								item.MODEL,
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
		public async Task<TableDataModel> LoadDataLinePN(MesStoplinePnRequestModel model)
		{
			string conditions = "where  a.ID > 0 ";
			if (!model.STOPLINE_ID.IsNullOrWhiteSpace())
			{
				conditions += $"and (a.PRODUCT_STOPLINE_ID =:STOPLINE_ID)";
			}
			if (!model.PART_NO.IsNullOrWhiteSpace())
			{
				conditions += $"and (a.PART_NO =:PART_NO)";
			}
			if (!model.Key.IsNullOrWhiteSpace())
			{
				conditions += $"and (instr(a.MODEL, :Key) > 0 )";
			}

			string sql =string.Format( @" select * from (
											select  ROWNUM as ROWNO, temp.* from (
												select   a.* from MES_STOPLINE_PN a {0} order by a.ID desc) temp)
										WHERE ROWNO BETWEEN ((:Page-1)*:Limit+1) AND (:Limit*:Page) order by ROWNO asc",conditions);

			//string pagedSql = SQLBuilderClass.GetPagedSQL(sql, " a.id desc", conditions);
			var resdata = await _dbConnection.QueryAsync<object>(sql, model);
			string sqlcnt = @"select count(0) from MES_STOPLINE_PN a " + conditions;

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
		public async Task<TableDataModel> GetLinePNToAdd(MesStoplinePnRequestModel model)
		{
			string conditions = " and  a.ID > 0 ";
			string strwhere = " 1=1 ";
			if (!model.STOPLINE_ID.IsNullOrWhiteSpace())
			{
				strwhere += $"and (b.PRODUCT_STOPLINE_ID=:STOPLINE_ID)";
			}
			if (!model.PART_NO.IsNullOrWhiteSpace())
			{
				conditions += $"and (a.CODE =:PART_NO)";
			}
			if (!model.Key.IsNullOrWhiteSpace())
			{
				conditions += $"and (instr(a.DESCRIPTION, :DESCRIPTION) > 0 )";
			}

			string sql = string.Format(@" select * from (
											select ROWNUM as ROWNO, temp.* from (
												select    a.* from IMS_PART a 
													left join MES_STOPLINE_PN b on {0} and a.CODE=b.PART_NO 
													where b.PART_NO is null  {1}  order by a.CODE ) temp 
										) where ROWNO BETWEEN ((:Page-1)*:Limit+1) AND (:Limit*:Page)", strwhere, conditions);
			//string pagedSql = SQLBuilderClass.GetPagedSQL(sql, " rowno asc ", strwhere,conditions);
			var resdata = await _dbConnection.QueryAsync<object>(sql, model);
			string sqlcnt =string.Format(@"select    count(0) from IMS_PART a 
											left join MES_STOPLINE_PN b on   {0} and a.CODE=b.PART_NO 
											where b.PART_NO is null {1} order by a.CODE", strwhere, conditions);

			int cnt = await _dbConnection.ExecuteScalarAsync<int>(sqlcnt, model);

			return new TableDataModel
			{
				count = cnt,
				data = resdata?.ToList(),
			};
		}

	}
}