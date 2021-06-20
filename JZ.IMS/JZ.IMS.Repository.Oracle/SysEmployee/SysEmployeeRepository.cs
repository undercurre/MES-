/**
*┌──────────────────────────────────────────────────────────────┐
*│　描    述：员工信息表接口实现                                                    
*│　作    者：嘉志科技                                            
*│　版    本：2.0    模板代码自动生成                                                
*│　创建时间：2019-10-23 23:28:38                             
*└──────────────────────────────────────────────────────────────┘
*┌──────────────────────────────────────────────────────────────┐
*│　命名空间： JZ.IMS.Repository.Oracle                                  
*│　类    名： SysEmployeeRepository                                      
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
using System.Linq;
using System.Threading.Tasks;
using System.IO;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;

namespace JZ.IMS.Repository.Oracle
{
	public class SysEmployeeRepository : BaseRepository<SysEmployee, String>, ISysEmployeeRepository
	{
		public SysEmployeeRepository(IOptionsSnapshot<DbOption> options)
		{
			_dbOption = options.Get("iWMS");
			if (_dbOption == null)
			{
				throw new ArgumentNullException(nameof(DbOption));
			}
			_dbConnection = ConnectionFactory.CreateConnection(_dbOption.DbType, _dbOption.ConnectionString);
		}

		/// <summary>
		///获取员工信息
		/// </summary>
		/// <param name="user_id">员工工号</param>
		/// <returns></returns>
		public async Task<SysEmployeeListModel> LoadDataAsync(string user_id)
		{
			string sql = @"SELECT USER_ID,USER_NAME,USER_SEX,USER_AGE,EDUCATION,ENTRYDATE,WORKINGYEARS FROM SYS_EMPLOYEE WHERE USER_ID = :USER_ID";
			var resdata = await _dbConnection.QueryAsync<SysEmployeeListModel>(sql, new { USER_ID = user_id });
			if (resdata == null)
			{
				return null;
			}
			if (resdata.FirstOrDefault() != null)
			{
				try
				{
					byte[] buff = (byte[])await GetManagerFromHrByUserIdAsync(user_id);
					resdata.FirstOrDefault().PHOTO = buff;
					resdata.FirstOrDefault().PHOTO_BASE64 = "data:image/jpg;base64," + Convert.ToBase64String(buff);
				}
				catch
				{

				}
			}
			return resdata.FirstOrDefault();
		}

		public async Task<object> GetManagerFromHrByUserIdAsync(string userId)
		{
			string sql = @"SELECT PHOTO FROM V_EMPLOYEE_FROM_HR WHERE USER_ID = :USER_ID";
			return await _dbConnection.ExecuteScalarAsync(sql, new
			{
				USER_ID = userId
			});
		}
	}
}