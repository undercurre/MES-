/**
*┌──────────────────────────────────────────────────────────────┐
*│　描    述：员工技能分值表 接口实现                                                    
*│　作    者：Admin                                            
*│　版    本：1.0    模板代码自动生成                                                
*│　创建时间：2019-01-05 17:54:04                             
*└──────────────────────────────────────────────────────────────┘
*┌──────────────────────────────────────────────────────────────┐
*│　命名空间： JZ.IMS.Repository.Oracle                                  
*│　类    名： SysEmployeeTrainGradeRepository                                      
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
using System.Linq;
using System.Threading.Tasks;
using JZ.IMS.ViewModels;
using JZ.IMS.Core.Extensions;
using JZ.IMS.ViewModels.SOPRoutes;
using JZ.IMS.Models.SOP;
using System.Text;
using System.Collections.Generic;

namespace JZ.IMS.Repository.Oracle
{
	public class SysEmployeeTrainGradeRepository : BaseRepository<SysEmployeeTrainGrade, string>, ISysEmployeeTrainGradeRepository
	{
		public SysEmployeeTrainGradeRepository(IOptionsSnapshot<DbOption> options)
		{
			_dbOption = options.Get("iWMS");
			if (_dbOption == null)
			{
				throw new ArgumentNullException(nameof(DbOption));
			}
			_dbConnection = ConnectionFactory.CreateConnection(_dbOption.DbType, _dbOption.ConnectionString);
		}

		/// <summary>
		///获取员工技能分值
		/// </summary>
		/// <param name="user_id">员工工号</param>
		/// <returns></returns>
		public async Task<IEnumerable<SysEmployeeTrainGradeListModel>> LoadDataAsync(string user_id)
		{
			string sql = @"SELECT * FROM SYS_EMPLOYEE_TRAIN_GRADE WHERE USER_ID = :USER_ID";
			return await _dbConnection.QueryAsync<SysEmployeeTrainGradeListModel>(sql, new { USER_ID = user_id });
		}
	}
}