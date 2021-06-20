/**
*┌──────────────────────────────────────────────────────────────┐
*│　描    述：                                                    
*│　作    者：嘉志科技                                              
*│　版    本：2.0   模板代码自动生成                                              
*│　创建时间：2020-03-03 16:03:30                           
*└──────────────────────────────────────────────────────────────┘
*┌──────────────────────────────────────────────────────────────┐
*│　命名空间： JZ.IMS.IRepository                                   
*│　接口名称： ISmtSolderpasteBatchmappingRepository                                      
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
    public interface ISmtSolderpasteBatchmappingRepository : IBaseRepository<SmtSolderpasteBatchmapping, String>
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
        /// 获取最新批次号
        /// </summary>
        /// <returns></returns>
        Task<String> GetBatchNo();
        /// <summary>
        /// 根据冰箱的位置获取批次信息
        /// </summary>
        /// <param name="loc"></param>
        /// <returns></returns>
        Task<IEnumerable<String>> GetBatchByLoc(String loc);
        /// <summary>
        /// 根据查询条件查询冰箱物理位置
        /// </summary>
        /// <param name="para"></param>
        /// <returns></returns>
        Task<IEnumerable<object>> GetLoction(String para);
        /// <summary>
        ///  通过批次号获取辅料详细信息
        /// </summary>
        /// <param name="bathNo"></param>
        /// <returns></returns>
        Task<IEnumerable<object>> GetBatchDeatil(String bathNo);
        /// <summary>
        /// 新增冷藏辅料信息
        /// </summary>
        /// <param name="batchNo"></param>
        /// <param name="reel_no"></param>
        /// <param name="friDgeLoc"></param>
        /// <param name="user"></param>
        /// <param name="remark"></param>
        /// <returns></returns>
        Task<bool> AddResources(String batchNo, String reel_no, String friDgeLoc, string user, String remark);
        /// <summary>
        /// 获取辅料当前的作业
        /// </summary>
        /// <param name="reelCode"></param>
        /// <returns></returns>
        Task<SmtResourcesRuncardViewModel> GetResourceRuncardView(String reelCode);
        /// <summary>
        /// 获取辅料制程
        /// </summary>
        /// <param name="resourceId"></param>
        /// <returns></returns>
        Task<IEnumerable<SmtResourceRouteOperationViewModel>> GetResourceRouteOperationView(decimal resourceId);

        /// <summary>
        /// 获取辅料历史的作业记录
        /// </summary>
        /// <param name="reelCode"></param>
        /// <returns></returns>
        Task<IEnumerable<SmtResourcesRuncardViewModel>> GetResourceRuncardLogView(String reelCode);
        /// <summary>
        /// 处理辅料逻辑
        /// </summary>
        /// <param name="resourceNo"></param>
        /// <param name="nextOperationId"></param>
        /// <param name="user"></param>
        void ProcessResourceRuncard(string resourceNo, decimal nextOperationId, string user);
        /// <summary>
        /// 改变辅料状态
        /// </summary>
        /// <param name="resourceNo"></param>
        /// <param name="status"></param>
        void SetResourceRuncardStatus(string resourceNo, decimal status);
        /// <summary>
        /// 获取制程信息
        /// </summary>
        /// <param name="objectId"></param>
        /// <returns></returns>
        Task<IEnumerable<SmtResourceRoute>> GetResourceRoute(decimal objectId);
        /// <summary>
        /// 修改辅料的环节信息
        /// </summary>
        /// <param name="routeId"></param>
        /// <param name="currentOperationId"></param>
        /// <param name="resourceNo"></param>
        /// <returns></returns>
        Task<bool> UpdateResourceRuncard(decimal routeId, Decimal currentOperationId, String resourceNo, DateTime beginTime, DateTime endtime);

        void CheckResourcesQty(string resourceNo, decimal nextOperationId);
    }
}