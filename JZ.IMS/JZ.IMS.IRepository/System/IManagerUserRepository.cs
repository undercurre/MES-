/**
*┌──────────────────────────────────────────────────────────────┐
*│　描    述：后台管理员 登录校验                                                   
*│　作    者：Admin                                              
*│　版    本：1.0   模板代码自动生成                                              
*│　创建时间：2018-12-18 13:28:43                           
*└──────────────────────────────────────────────────────────────┘
*┌──────────────────────────────────────────────────────────────┐
*│　命名空间： JZ.IMS.IRepository                                   
*│　接口名称： IManagerUserRepository                                      
*└──────────────────────────────────────────────────────────────┘
*/
using JZ.IMS.Core.Repository;
using JZ.IMS.Models;
using System;
using System.Threading.Tasks;

namespace JZ.IMS.IRepository
{
	public interface IManagerUserRepository : IBaseRepository<Sys_Manager, decimal>
	{
		/// <summary>
		/// 检测用户+密码是否正确
		/// </summary>
		/// <param name="userName"></param>
		/// <param name="userPassword"></param>
		/// <returns></returns>
		bool CheckLogin(string userName, string userPassword);

		/// <summary>
		/// 检测用户名是否存在
		/// </summary>
		/// <param name="userName"></param>
		/// <returns></returns>
		bool ExistsUserName(string userName);
        /// <summary>
        /// 新增用户
        /// </summary>
        /// <param name="userName"></param>
        /// <param name="empno"></param>
        /// <param name="password"></param>
        /// <returns></returns>
        bool AddUser(String userName, String empno, string password);

    }
}