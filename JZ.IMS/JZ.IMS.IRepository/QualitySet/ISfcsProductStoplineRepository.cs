/*
*┌──────────────────────────────────────────────────────────────┐
*│　描    述：                                                    
*│　作    者：嘉志科技                                              
*│　版    本：2.0   模板代码自动生成                                              
*│　创建时间：2020-04-16 12:07:39                           
*└──────────────────────────────────────────────────────────────┘
*┌──────────────────────────────────────────────────────────────┐
*│　命名空间： JZ.IMS.IRepository                                   
*│　接口名称： ISfcsProductStoplineRepository                                      
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
    public interface ISfcsProductStoplineRepository : IBaseRepository<SfcsProductStopline, Decimal>
    {   
        /// <summary>
		///项目是否已被使用 
		/// </summary>
		/// <param name="id">项目id</param>
		/// <returns></returns>
		Task<bool> ItemIsByUsed(decimal id);

        /// <summary>
		/// 获取制程工序列表
		/// </summary>
		/// <param name="route_id">制程ID</param>
		/// <returns></returns>
		Task<List<SfcsOperations>> GetRouteConfigLists(decimal route_id);

        /// <summary>
		/// 获取产品停线管控规则配置列表
		/// </summary>
		/// <param name="part_no">料号</param>
		/// <param name="route_id">制程ID</param>
		/// <returns></returns>
		Task<List<SfcsProductStopline>> GetSfcsProductStoplineList(string part_no, decimal route_id);

        /// <summary>
        /// 保存数据
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        Task<decimal> SaveDataByTrans(SfcsProductStoplineModel model);


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
        /// 查询数据
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        Task<TableDataModel> GetSfcsProductStoplinesList(SfcsProductStoplineRequestModel model);



        /// <summary>
        /// 获取线体下拉框
        /// </summary>
        /// <returns></returns>

        Task<List<dynamic>> GetLINENAME();


        /// <summary>
        /// 获取工序下拉框
        /// </summary>
        /// <returns></returns>

        Task<List<dynamic>> GetOperation();


        /// <summary>
		/// 根据主表ID获取停线管控主表和呼叫子表数据
		/// </summary>
		/// <param name="id"></param>
		/// <returns></returns>
		Task<TableDataModel> GetStopLineAndCallDataByID(decimal id);
 

    }
}