/**
*┌──────────────────────────────────────────────────────────────┐
*│　描    述：接口实现                                                    
*│　作    者：嘉志科技                                            
*│　版    本：2.0    模板代码自动生成                                                
*│　创建时间：2019-11-06 16:59:33                             
*└──────────────────────────────────────────────────────────────┘
*┌──────────────────────────────────────────────────────────────┐
*│　命名空间： JZ.IMS.Repository.Oracle                                  
*│　类    名： SopSkillStandardRepository                                      
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
using System.Text;
using System.Collections;
using System.Collections.Generic;
using JZ.IMS.ViewModels;

namespace JZ.IMS.Repository.Oracle
{
	public class SopSkillStandardRepository : BaseRepository<SopSkillStandard, Decimal>, ISopSkillStandardRepository
	{
		public SopSkillStandardRepository(IOptionsSnapshot<DbOption> options)
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
			string sql = "SELECT ENABLED FROM SOP_SKILL_STANDARD WHERE ID=:ID AND IS_DELETE='N'";
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
			string sql = "UPDATE SOP_SKILL_STANDARD set ENABLED=:ENABLED where  Id=:Id";
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
			string sql = "SELECT SOP_SKILL_STANDARD_SEQ.NEXTVAL MY_SEQ FROM DUAL";
			var result = await _dbConnection.ExecuteScalarAsync(sql);
			return (decimal)result;
		}

		/// <summary>
		/// 获取工序数据
		/// </summary>
		/// <param name="operationName">工序名称</param>
		/// <param name="desc">工序描述</param>
		/// <returns></returns>
		public async Task<IEnumerable<SfcsOperationsListModel>> LoadOperationData(string operationName, string desc)
		{
			StringBuilder sql = new StringBuilder();
			sql.Append("SELECT TAB1.ID,TAB1.OPERATION_NAME,TAB1.DESCRIPTION,tab2.CHINESE as OPERATION_CLASS_NAME,tab3.CHINESE AS OPERATION_CATEGORY_NAME FROM SFCS_OPERATIONS tab1 ");
			sql.Append("LEFT JOIN  ( select LOOKUP_CODE,CHINESE from SFCS_PARAMETERS where lookup_type='OPERATION_CLASS') tab2 on tab1.OPERATION_CLASS = tab2.LOOKUP_CODE ");
			sql.Append("LEFT JOIN (select LOOKUP_CODE,CHINESE from SFCS_PARAMETERS where lookup_type='OPERATION_CATEGORY') tab3 on tab1.OPERATION_CATEGORY = tab3.LOOKUP_CODE ");

			string where = " where tab1.ENABLED='Y' ";
			if (!string.IsNullOrEmpty(operationName))
				where += " and instr(tab1.OPERATION_NAME,:OPERATION_NAME) > 0 ";
			if (!string.IsNullOrEmpty(desc))
				where += " and instr(tab1.DESCRIPTION,:DESCRIPTION) > 0 ";

			sql.Append(where);
			sql.Append(" order by tab1.OPERATION_NAME ");

			var result = await _dbConnection.QueryAsync<SfcsOperationsListModel>(sql.ToString(), new { OPERATION_NAME = operationName, DESCRIPTION = desc });
			return result;
		}

		/// <summary>
		/// 获取工序技能评判标准数据
		/// </summary>
		/// <param name="ID">工序ID</param>
		/// <returns></returns>
		public async Task<IEnumerable<SopSkillStandardListModel>> LoadSkillStandardData(decimal ID)
		{
			StringBuilder sql = new StringBuilder();
			sql.Append("select * from SOP_SKILL_STANDARD where OPERATION_ID = :ID");

			var result = await _dbConnection.QueryAsync<SopSkillStandardListModel>(sql.ToString(), new { ID });
			return result;
		}

		/// <summary>
		/// 获取技能名称数据
		/// </summary>
		/// <returns></returns>
		public IEnumerable<string> GetTrainData()
		{
			StringBuilder sql = new StringBuilder();
			sql.Append("select DISTINCT TRAIN_NAME from SYS_EMPLOYEE_TRAIN_GRADE where TRAIN_NAME is not null");
			var result = _dbConnection.Query<string>(sql.ToString(), new { });
			return result;
		}
	}
}