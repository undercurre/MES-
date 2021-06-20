/**
*┌──────────────────────────────────────────────────────────────┐
*│　描    述：后台管理员 登录校验 接口实现                                                    
*│　作    者：Admin                                            
*│　版    本：1.0    模板代码自动生成                                                
*│　创建时间：2018-12-18 13:28:43                             
*└──────────────────────────────────────────────────────────────┘
*┌──────────────────────────────────────────────────────────────┐
*│　命名空间： JZ.IMS.Repository.Oracle                                  
*│　类    名： ManagerUserRepository                                      
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

namespace JZ.IMS.Repository.Oracle
{
	public class ManagerUserRepository : BaseRepository<Sys_Manager, decimal>, IManagerUserRepository
	{
		public ManagerUserRepository(IOptionsSnapshot<DbOption> options)
		{
			_dbOption = options.Get("iWMS");
			if (_dbOption == null)
			{
				throw new ArgumentNullException(nameof(DbOption));
			}
			_dbConnection = ConnectionFactory.CreateConnection(_dbOption.DbType, _dbOption.ConnectionString);
		}

		/// <summary>
		/// 检测用户+密码是否正确
		/// </summary>
		/// <param name="userName"></param>
		/// <param name="userPassword"></param>
		/// <returns></returns>
		public bool CheckLogin(string userName, string userPassword)
		{
			//string sql = "SELECT COUNT(1) FROM SYS_USERS WHERE EMPNO = :USER_NAME AND UPPER(USER_PASSWORD) = UPPER(:USER_PASSWORD)";
			string sql = "SELECT COUNT(1) FROM Sys_Manager WHERE UPPER(USER_NAME) = UPPER(:USER_NAME) AND UPPER(PASSWORD) = UPPER(:USER_PASSWORD)";
			object result = _dbConnection.ExecuteScalar(sql, new
			{
				USER_NAME = userName,
				USER_PASSWORD = userPassword,
			});

			if (Convert.ToInt32(result) > 0)
			{
				return true;
			}
			else
			{
				return false;
			}
		}

		/// <summary>
		/// 检测用户名是否存在
		/// </summary>
		/// <param name="userName"></param>
		/// <returns></returns>
		public bool ExistsUserName(string userName)
		{
			string sql = "SELECT COUNT(1) FROM SYS_USERS WHERE EMPNO = :USER_NAME";
			object result = _dbConnection.ExecuteScalar(sql, new
			{
				USER_NAME = userName
			});

			if (Convert.ToInt32(result) > 0)
			{
				return true;
			}
			else
			{
				return false;
			}
		}

        public bool AddUser(String userName,String empno, string password)
        {
            string sql = @"INSERT INTO SYS_USERS(ID, VERSION, USER_NAME, EMPNO, AD_NAME, USER_TYPE, USER_PASSWORD, STATUS)
                VAlUES(SYS_GUID(), 1, :USER_NAME, :EMPNO, :AD_NAME, '内部',:USER_PASSWORD, 'Y')";
            object result = _dbConnection.ExecuteScalar(sql, new
            {
                USER_NAME = userName,
                EMPNO = empno,
                AD_NAME = empno,
                USER_PASSWORD = password
            });
            return true;
        }
	}
}