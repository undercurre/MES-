/**
*┌──────────────────────────────────────────────────────────────┐
*│　描    述：                                                    
*│　作    者：嘉志科技                                              
*│　版    本：2.0   模板代码自动生成                                              
*│　创建时间：2019-11-06 16:59:33                           
*└──────────────────────────────────────────────────────────────┘
*┌──────────────────────────────────────────────────────────────┐
*│　命名空间： JZ.IMS.IRepository                                   
*│　接口名称： ISopSkillStandardRepository                                      
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
    public interface ISopSkillStandardRepository : IBaseRepository<SopSkillStandard, Decimal>
    {   
        /// <summary>
        /// 根据主键获取激活状态
        /// </summary>
        /// <param name="id">主键</param>
        /// <returns></returns>
        Task<Boolean> GetEnableStatus(decimal id);

        /// <summary>
        /// 修改激活状态
        /// </summary>
        /// <param name="id">主键</param>
        /// <param name="status">更改后的状态</param>
        /// <returns></returns>
        Task<decimal> ChangeEnableStatus(decimal id, bool status);

		// <summary>
        /// 获取表的序列
        /// </summary>
        /// <returns></returns>
		Task<decimal> GetSEQID();

		/// <summary>
		/// 获取工序数据
		/// </summary>
		/// <param name="operationName">工序名称</param>
		/// <param name="desc">工序描述</param>
		/// <returns></returns>
		Task<IEnumerable<SfcsOperationsListModel>> LoadOperationData(string operationName, string desc);

		/// <summary>
		/// 获取工序技能评判标准数据
		/// </summary>
		/// <param name="ID">工序ID</param>
		/// <returns></returns>
		Task<IEnumerable<SopSkillStandardListModel>> LoadSkillStandardData(decimal ID);

		/// <summary>
		/// 获取技能名称数据
		/// </summary>
		/// <returns></returns>
		IEnumerable<string> GetTrainData();
	}
}