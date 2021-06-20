/*
*┌──────────────────────────────────────────────────────────────┐
*│　描    述：                                                    
*│　作    者：嘉志科技                                              
*│　版    本：2.0   模板代码自动生成                                              
*│　创建时间：2020-04-08 08:52:34                           
*└──────────────────────────────────────────────────────────────┘
*┌──────────────────────────────────────────────────────────────┐
*│　命名空间： JZ.IMS.IRepository                                   
*│　接口名称： ISfcsRoutesRepository                                      
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
    public interface ISfcsRoutesRepository : IBaseRepository<SfcsRoutes, Decimal>
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
        /// 获取表的序列
        /// </summary>
        /// <returns></returns>
		Task<decimal> GetSEQID();

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
		Task<decimal> SaveDataByTrans(SfcsRoutesModel model);

        /// <summary>
        /// 制程设定保存
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        Task<decimal> RoutesConfigSaveData(SfcsRouteConfigModel model);

        /// <summary>
        /// 查制程名称
        /// </summary>
        /// <param name="current_operation_id"></param>
        /// <returns></returns>
        Task<SfcsOperationsListModel> GetOperationDataTable(decimal current_operation_id);

        /// <summary>
        /// 尋找刪除的當前工序在Runcard中是否存在WIP_OPERATION,LAST_OPERATION以及未維修的工序
        /// </summary>
        /// <param name="operationCode"></param>
        /// <param name="routeID"></param>
        /// <returns></returns>
        Task<bool> CheckWIPExisted(decimal operationCode, decimal routeID);

        /// <summary>
        /// 查制程名称
        /// </summary>
        /// <param name="ID"></param>
        /// <returns></returns>
        Task<dynamic> GetRoutesList(decimal ID);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        Task<decimal> RoutesConfigSaveDataEx(SfcsRouteConfigModel model);
    }
}