/**
*┌──────────────────────────────────────────────────────────────┐
*│　描    述：                                                    
*│　作    者：嘉志科技                                              
*│　版    本：2.0   模板代码自动生成                                              
*│　创建时间：2020-02-27 14:15:50                           
*└──────────────────────────────────────────────────────────────┘
*┌──────────────────────────────────────────────────────────────┐
*│　命名空间： JZ.IMS.IRepository                                   
*│　接口名称： ISmtResourceRulesRepository                                      
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
    public interface ISmtResourceRulesRepository : IBaseRepository<SmtResourceRules, Decimal>
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

        /// <summary>
		/// 获取辅料规则工序选择列表
		/// </summary>
		/// <returns></returns>
		Task<List<ResourceRulesProcess>> GetProcessAsync();

        /// <summary>
        /// 保存数据
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        Task<decimal> SaveDataByTrans(SmtResourceRulesModel model);

        /// <summary>
        /// 分页
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        Task<TableDataModel> GetResourceRulesList(SmtResourceRulesRequestModel model);

        /// <summary>
        /// 导出数据
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        Task<TableDataModel> GetExportData(SmtResourceRulesRequestModel model);

        /// <summary>
        /// 获取辅料回温情况
        /// </summary>
        /// <returns></returns>
        Task<List<SmtResourceWarmVM>> GetSmtResourceWarm();

        /// <summary>
        /// 获取辅料使用情况
        /// </summary>
        /// <returns></returns>
        Task<List<SmtResourceUseVM>> GetSmtResourceUse();

    }
}