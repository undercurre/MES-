/*
*┌──────────────────────────────────────────────────────────────┐
*│　描    述：呼叫规则配置推送人员表接口实现                                                    
*│　作    者：嘉志科技                                            
*│　版    本：2.0    模板代码自动生成                                                
*│　创建时间：2020-08-13 10:57:48                             
*└──────────────────────────────────────────────────────────────┘
*┌──────────────────────────────────────────────────────────────┐
*│　命名空间： JZ.IMS.Repository.Oracle                                  
*│　类    名： AndonCallGroupRuleReRepository                                      
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
    public class AndonCallGroupRuleReRepository:BaseRepository<AndonCallGroupRuleRe,Decimal>, IAndonCallGroupRuleReRepository
    {
        public AndonCallGroupRuleReRepository(IOptionsSnapshot<DbOption> options)
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
			string sql = "SELECT ENABLED FROM ANDON_CALL_GROUP_RULE_RE WHERE ID=:ID";
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
			string sql = "UPDATE ANDON_CALL_GROUP_RULE_RE set ENABLED=:ENABLED WHERE ID=:Id";
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
			string sql = "SELECT ANDON_CALL_GROUP_RULE_RE_SEQ.NEXTVAL MY_SEQ FROM DUAL";
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
			string sql = "select count(0) from ANDON_CALL_GROUP_RULE_RE where id = :id";
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
		public async Task<decimal> SaveDataByTrans(AndonCallGroupRuleReModel model)
		{
			int result = 1;
			ConnectionFactory.OpenConnection(_dbConnection);
			using (var tran = _dbConnection.BeginTransaction())
			{
				try
				{
					//新增
					string insertSql = @"insert into ANDON_CALL_GROUP_RULE_RE 
					(ID,PERSON_RULE_ID,GROUP_ID,CREATE_USER,CREATE_TIME,UPDATE_USER,UPDATE_TIME,ENABLED) 
					VALUES (:ID,:PERSON_RULE_ID,:GROUP_ID,:CREATE_USER,:CREATE_TIME,:UPDATE_USER,:UPDATE_TIME,:ENABLED)";
					if (model.InsertRecords != null && model.InsertRecords.Count > 0)
					{
						foreach (var item in model.InsertRecords)
						{
							var newid = await GetSEQID();
							var resdata = await _dbConnection.ExecuteAsync(insertSql, new
							{
								ID = newid,
								item.PERSON_RULE_ID,
								item.GROUP_ID,
								item.CREATE_USER,
								item.CREATE_TIME,
								item.UPDATE_USER,
								item.UPDATE_TIME,
								item.ENABLED,
							}, tran);
						}
					}
					//更新
					string updateSql = @"Update ANDON_CALL_GROUP_RULE_RE set PERSON_RULE_ID=:PERSON_RULE_ID,GROUP_ID=:GROUP_ID,CREATE_USER=:CREATE_USER,CREATE_TIME=:CREATE_TIME,UPDATE_USER=:UPDATE_USER,UPDATE_TIME=:UPDATE_TIME,ENABLED=:ENABLED  
						where ID=:ID ";
					if (model.UpdateRecords != null && model.UpdateRecords.Count > 0)
					{
						foreach (var item in model.UpdateRecords)
						{
							var resdata = await _dbConnection.ExecuteAsync(updateSql, new
							{
								item.ID,
								item.PERSON_RULE_ID,
								item.GROUP_ID,
								item.CREATE_USER,
								item.CREATE_TIME,
								item.UPDATE_USER,
								item.UPDATE_TIME,
								item.ENABLED,
							}, tran);
						}
					}
					//删除
					string deleteSql = @"Delete from ANDON_CALL_GROUP_RULE_RE where ID=:ID ";
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
		/// 查询数据
		/// </summary>
		/// <param name="model"></param>
		/// <returns></returns>
		public async Task<TableDataModel> GetGroupRuleReData(AndonCallGroupRuleReRequestModel model)
		{
			string conditions = "WHERE a.ID > 0 ";

			if (!model.PERSON_RULE_ID.IsNullOrWhiteSpace())
			{
				conditions += $" and (a.PERSON_RULE_ID =:PERSON_RULE_ID)";
			}
			string sql = @" select ROWNUM as rowno, c.Organize_Name,b.GROUP_NAME,b.DESCRIPTION,a.* from ANDON_CALL_GROUP_RULE_RE a
								left join MES_MESSAGE_RECEIVER_GROUP b on a.GROUP_ID=b.ID
								left join SYS_ORGANIZE c on b.ORGANIZE_ID=c.ID ";

			string pagedSql = SQLBuilderClass.GetPagedSQL(sql, " a.id desc", conditions);
			var resdata = await _dbConnection.QueryAsync<object>(pagedSql, model);
			
			string sqlcnt = @"select count(0) from ANDON_CALL_GROUP_RULE_RE a
								left join MES_MESSAGE_RECEIVER_GROUP b on a.GROUP_ID=b.ID
								left join SYS_ORGANIZE c on b.ORGANIZE_ID=c.ID " + conditions;
			int cnt = await _dbConnection.ExecuteScalarAsync<int>(sqlcnt, model);

			return new TableDataModel
			{
				count = cnt,
				data = resdata?.ToList(),
			};

		}


	}
}