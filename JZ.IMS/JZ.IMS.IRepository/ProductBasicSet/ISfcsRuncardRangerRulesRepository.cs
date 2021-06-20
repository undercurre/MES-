/*
*┌──────────────────────────────────────────────────────────────┐
*│　描    述：                                                    
*│　作    者：嘉志科技                                              
*│　版    本：2.0   模板代码自动生成                                              
*│　创建时间：2020-04-11 10:06:08                           
*└──────────────────────────────────────────────────────────────┘
*┌──────────────────────────────────────────────────────────────┐
*│　命名空间： JZ.IMS.IRepository                                   
*│　接口名称： ISfcsRuncardRangerRulesRepository                                      
*└──────────────────────────────────────────────────────────────┘
*/
using JZ.IMS.Core.Repository;
using JZ.IMS.Models;
using JZ.IMS.ViewModels;
using System;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace JZ.IMS.IRepository
{
    public interface ISfcsRuncardRangerRulesRepository : IBaseRepository<SfcsRuncardRangerRules, Decimal>
    {   
        /// <summary>
		///项目是否已被使用 
		/// </summary>
		/// <param name="id">项目id</param>
		/// <returns></returns>
		Task<bool> ItemIsByUsed(decimal id);

        /// <summary>
		/// 保存数据
		/// </summary>
		/// <param name="model"></param>
		/// <returns></returns>
		Task<decimal> SaveDataByTrans(SfcsRuncardRangerRulesAddOrModifyModel model);

		/// <summary>
		/// 獲取Ranger生成規則
		/// </summary>
		Task<SfcsRuncardRangerRules> GetRuncardRangerRulesByPN(string partNumber, string enable, int rule_type);

		/// <summary>
		/// 獲取Ranger生成規則
		/// </summary>
		Task<SfcsRuncardRangerRules> GetRuncardRangerRulesByPlatformId(decimal platformId, string enable, int rule_type);

		/// <summary>
		/// 獲取Ranger生成規則
		/// </summary>
		Task<SfcsRuncardRangerRules> GetRuncardRangerRulesByFamilyId(decimal family_id, string enable, int rule_type);

        /// <summary>
        /// 獲取Ranger生成規則
        /// </summary>
        Task<SfcsRuncardRangerRules> GetRuncardRangerRulesByCustomerId(decimal customerId, string enable, int rule_type);

        Task<IEnumerable<SfcsRuncardRangerRulesListModel>> LoadData(SfcsRuncardRangerRulesRequestModel model);

		Task<int> LoadDataCount(SfcsRuncardRangerRulesRequestModel model);

	}
}