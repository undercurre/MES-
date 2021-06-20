/**
*┌──────────────────────────────────────────────────────────────┐
*│　描    述：                                                    
*│　作    者：嘉志科技                                              
*│　版    本：2.0   模板代码自动生成                                              
*│　创建时间：2020-04-03 11:58:55                           
*└──────────────────────────────────────────────────────────────┘
*┌──────────────────────────────────────────────────────────────┐
*│　命名空间： JZ.IMS.IRepository                                   
*│　接口名称： ISfcsProductMultiRuncardRepository                                      
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
    public interface ISfcsProductMultiRuncardRepository : IBaseRepository<SfcsProductMultiRuncard, Decimal>
    {   
		// <summary>
        /// 获取表的序列
        /// </summary>
        /// <returns></returns>
		Task<decimal> GetSEQID();

        /// <summary>
        /// 获取制程工序列表
        /// </summary>
        /// <param name="route_id">制程ID</param>
        /// <returns></returns>
        Task<List<SfcsOperations>> GetRouteConfigLists(decimal route_id);

        /// <summary>
        /// 获取连板配置
        /// </summary>
        /// <param name="part_no">料号</param>
        /// <param name="route_id">制程ID</param>
        /// <returns></returns>
        Task<SfcsProductMultiRuncard> GetSfcsProductMultiRuncard(string part_no, decimal route_id);

        /// <summary>
        /// 保存数据
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        Task<decimal> SaveDataByTrans(SfcsProductMultiRuncardAddOrModifyModel model);
	}
}