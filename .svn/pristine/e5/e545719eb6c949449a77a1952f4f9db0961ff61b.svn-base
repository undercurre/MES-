/*
*┌──────────────────────────────────────────────────────────────┐
*│　描    述：呼叫规则配置表接口实现                                                    
*│　作    者：嘉志科技                                            
*│　版    本：2.0    模板代码自动生成                                                
*│　创建时间：2020-08-12 17:36:04                             
*└──────────────────────────────────────────────────────────────┘
*┌──────────────────────────────────────────────────────────────┐
*│　命名空间： JZ.IMS.Repository.Oracle                                  
*│　类    名： AndonCallRuleConfigRepository                                      
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
	public class AndonCallRuleConfigRepository : BaseRepository<AndonCallRuleConfig, Decimal>, IAndonCallRuleConfigRepository
	{
		public AndonCallRuleConfigRepository(IOptionsSnapshot<DbOption> options)
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
			string sql = "SELECT ENABLED FROM ANDON_CALL_RULE_CONFIG WHERE ID=:ID";
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
			string sql = "UPDATE ANDON_CALL_RULE_CONFIG set ENABLED=:ENABLED WHERE ID=:Id";
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
			string sql = "SELECT ANDON_CALL_RULE_CONFIG_SEQ.NEXTVAL MY_SEQ FROM DUAL";
			var result = await _dbConnection.ExecuteScalarAsync(sql);
			return (decimal)result;
		}

		/// <summary>
		/// 获取呼叫规则配置推送规则表的序列
		/// </summary>
		/// <returns></returns>
		public async Task<decimal> GetPersonRuleSEQID()
		{
			string sql = "SELECT ANDON_CALL_PERSON_RULE_SEQ.NEXTVAL MY_SEQ FROM DUAL";
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
			string sql = "select count(0) from ANDON_CALL_RULE_CONFIG where id = :id";
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
		public async Task<decimal> SaveDataByTrans(AndonCallRuleConfigModel model)
		{
			int result = 1;
			ConnectionFactory.OpenConnection(_dbConnection);
			using (var tran = _dbConnection.BeginTransaction())
			{
				try
				{
					//新增
					string insertSql = @"insert into ANDON_CALL_RULE_CONFIG 
					(ID,LINE_ID,CALL_CONTENT_ID,ENABLED,CREATOR,CREATE_TIME) 
					VALUES (:ID,:LINE_ID,:CALL_CONTENT_ID,:ENABLED,:CREATOR,sysdate)";
					if (model.InsertRecords != null && model.InsertRecords.Count > 0)
					{
						foreach (var item in model.InsertRecords)
						{
							var newid = await GetSEQID();
							var resdata = await _dbConnection.ExecuteAsync(insertSql, new
							{
								ID = newid,
								item.LINE_ID,
								item.CALL_CONTENT_ID,
								item.ENABLED,
								item.CREATOR
							}, tran);
							
							//新增呼叫规则配置的时候同时创建一级、二级、三级推送规则
							string sqlleve1 = string.Format(@" insert into ANDON_CALL_PERSON_RULE(ID,CALL_RULE_ID,CALL_LEVEL,CREATE_USER,CREATE_TIME)
														  values({0},{1},0,'{2}',sysdate)", await GetPersonRuleSEQID(), newid,item.CREATOR);
							var relevel1 = await _dbConnection.ExecuteAsync(sqlleve1,tran);
							string sqlleve2 = string.Format(@" insert into ANDON_CALL_PERSON_RULE(ID,CALL_RULE_ID,CALL_LEVEL,CREATE_USER,CREATE_TIME)
														  values({0},{1},1,'{2}',sysdate)", await GetPersonRuleSEQID(), newid, item.CREATOR);
							var relevel2 = await _dbConnection.ExecuteAsync(sqlleve2, tran);
							string sqlleve3 = string.Format(@" insert into ANDON_CALL_PERSON_RULE(ID,CALL_RULE_ID,CALL_LEVEL,CREATE_USER,CREATE_TIME)
														  values({0},{1},2,'{2}',sysdate)", await GetPersonRuleSEQID(), newid, item.CREATOR);
							var relevel3 = await _dbConnection.ExecuteAsync(sqlleve3, tran);


						}
					}
					//更新
					string updateSql = @"Update ANDON_CALL_RULE_CONFIG set LINE_ID=:LINE_ID,CALL_CONTENT_ID=:CALL_CONTENT_ID,ENABLED=:ENABLED,CREATOR=:CREATOR
						where ID=:ID ";
					if (model.UpdateRecords != null && model.UpdateRecords.Count > 0)
					{
						foreach (var item in model.UpdateRecords)
						{
							var resdata = await _dbConnection.ExecuteAsync(updateSql, new
							{
								item.ID,
								item.LINE_ID,
								item.CALL_CONTENT_ID,
								item.ENABLED,
								item.CREATOR,
							}, tran);
						}
					}
					//更新呼叫规则配置推送表的记录
					string updateRuleSql = @"Update ANDON_CALL_PERSON_RULE set CALL_RULE_ID=:CALL_RULE_ID,CALL_LEVEL=:CALL_LEVEL,RULE_TYPE=:RULE_TYPE,RULE=:RULE,CREATE_USER=:CREATE_USER,CREATE_TIME=:CREATE_TIME,UPDATE_USER=:UPDATE_USER,UPDATE_TIME=:UPDATE_TIME,RULE_UNIT=:RULE_UNIT  
						where ID=:ID ";
					if (model.UpdateRecordsRule != null && model.UpdateRecordsRule.Count > 0)
					{
						foreach (var item in model.UpdateRecordsRule)
						{
							var resdata = await _dbConnection.ExecuteAsync(updateRuleSql, new
							{
								item.ID,
								item.CALL_RULE_ID,
								item.CALL_LEVEL,
								item.RULE_TYPE,
								item.RULE,
								item.CREATE_USER,
								item.CREATE_TIME,
								item.UPDATE_USER,
								item.UPDATE_TIME,
								item.RULE_UNIT,
							}, tran);
						}
					}


					//删除
					string deleteSql = @"Delete from ANDON_CALL_RULE_CONFIG where ID=:ID ";
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
		/// 获取数据
		/// </summary>
		/// <param name="model"></param>
		/// <returns></returns>
		public async Task<TableDataModel> GetRuleConfigDate(AndonCallRuleConfigRequestModel model)
		{
			string conditions = " where  a.ID > 0 ";
			if (!model.ID.IsNullOrWhiteSpace())
			{
				conditions += $"and (a.ID =:ID)";
			}
			if (!model.LINE_ID.IsNullOrWhiteSpace())
			{
				conditions += $"and (a.LINE_ID =:LINE_ID)";
			}
			if (!model.CALL_CATEGORY_CODE.IsNullOrWhiteSpace())
			{
				conditions += $"and (b.CALL_CATEGORY_CODE =:CALL_CATEGORY_CODE)";
			}
			if (!model.CALL_TYPE_CODE.IsNullOrWhiteSpace())
			{
				conditions += $"and (b.CALL_TYPE_CODE =:CALL_TYPE_CODE)";
			}
			if (!model.Key.IsNullOrWhiteSpace())
			{
				conditions += $"and (instr(b.CALL_TITLE, :Key) > 0  )";
			}
			if (!model.ENABLED.IsNullOrWhiteSpace())
			{
				conditions += $"and (a.ENABLED =:ENABLED)";
			}

			//SMT_LINES、SFCS_OPERATION_LINES 都为线体表
			string sql = @"
							select rownum as rowno, case when c.id>0 then  c.operation_line_name 
									 when e.id>0 then  e.line_name 
									 else ''  end as line_name,b.call_category_code,sp1.description call_category_name,b.call_type_code,sp2.description call_type_name,b.call_title, a.* from andon_call_rule_config a
							left join andon_call_content_config b on a.call_content_id=b.id
							left join sfcs_operation_lines c on a.line_id=c.id
							left join smt_lines e on a.line_id=e.id
							inner join (select * from sfcs_parameters where lookup_type='ALARM_CATEGORY' and enabled='Y') sp1 on sp1.lookup_code = b.call_category_code  
							inner join (select * from sfcs_parameters where lookup_type='ALARM_TYPE' and enabled='Y') sp2 on sp2.lookup_code = b.call_type_code  ";

			string pagedSql = SQLBuilderClass.GetPagedSQL(sql, " a.id desc", conditions);
			var resdata = await _dbConnection.QueryAsync<object>(pagedSql, model);
			 
			string sqlcnt = @"select count(0) from ANDON_CALL_RULE_CONFIG a
							left join ANDON_CALL_CONTENT_CONFIG b on a.CALL_CONTENT_ID=b.ID
							left join SFCS_OPERATION_LINES c on a.LINE_ID=c.ID
							left join SMT_LINES e on a.LINE_ID=e.ID " + conditions;
			int cnt = await _dbConnection.ExecuteScalarAsync<int>(sqlcnt, model);

			return new TableDataModel
			{
				count = cnt,
				data = resdata?.ToList(),
			};
		}

		/// <summary>
		/// 获取线体下拉框
		/// </summary>
		/// <returns></returns>

		public async Task<List<dynamic>> GetLINENAME()
		{
			List<dynamic> result = null;
            try
            {
				string sql = @"select * from (
								select ID,LINE_NAME from SMT_LINES 
								union all
								select ID,OPERATION_LINE_NAME as LINE_NAME from SFCS_OPERATION_LINES where ENABLED='Y'
							  ) order by LINE_NAME desc";
				var objectlist = await _dbConnection.QueryAsync<dynamic>(sql);
				return objectlist?.ToList();
			}
            catch (Exception ex)
            {
				result = null;
			}
			return result;
		}
	}
}