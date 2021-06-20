/**
*┌──────────────────────────────────────────────────────────────┐
*│　描    述：员工信息表                                                    
*│　作    者：嘉志科技                                              
*│　版    本：2.0   模板代码自动生成                                              
*│　创建时间：2019-10-23 23:28:38                           
*└──────────────────────────────────────────────────────────────┘
*┌──────────────────────────────────────────────────────────────┐
*│　命名空间： JZ.IMS.IRepository                                   
*│　接口名称： ISysEmployeeRepository                                      
*└──────────────────────────────────────────────────────────────┘
*/
using JZ.IMS.Core.Repository;
using JZ.IMS.Models;
using JZ.IMS.ViewModels;
using System;
using System.Threading.Tasks;

namespace JZ.IMS.IRepository
{
	public interface ISysEmployeeRepository : IBaseRepository<SysEmployee, String>
	{
		/// <summary>
		///获取员工信息
		/// </summary>
		/// <param name="user_id">员工工号</param>
		/// <returns></returns>
		Task<SysEmployeeListModel> LoadDataAsync(string user_id);
	}
}