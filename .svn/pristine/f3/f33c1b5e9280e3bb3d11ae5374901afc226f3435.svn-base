/**
*┌──────────────────────────────────────────────────────────────┐
*│　描    述：员工技能分值表                                                    
*│　作    者：Admin                                                                    
*└──────────────────────────────────────────────────────────────┘
*┌──────────────────────────────────────────────────────────────┐
*│　命名空间： JZ.IMS.IRepository                                   
*│　接口名称： ISysEmployeeTrainGradeRepository                                      
*└──────────────────────────────────────────────────────────────┘
*/
using JZ.IMS.Core.Repository;
using JZ.IMS.Models;
using JZ.IMS.ViewModels;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace JZ.IMS.IRepository
{
	public interface ISysEmployeeTrainGradeRepository : IBaseRepository<SysEmployeeTrainGrade, string>
	{
		
		/// <summary>
		///获取员工技能分值
		/// </summary>
		/// <param name="user_id">员工工号</param>
		/// <returns></returns>
		Task<IEnumerable<SysEmployeeTrainGradeListModel>> LoadDataAsync(string user_id);

	}
}