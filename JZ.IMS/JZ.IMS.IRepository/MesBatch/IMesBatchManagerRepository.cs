/*
*┌──────────────────────────────────────────────────────────────┐
*│　描    述：批次管理表                                                    
*│　作    者：嘉志科技                                              
*│　版    本：2.0   模板代码自动生成                                              
*│　创建时间：2020-08-17 15:48:19                           
*└──────────────────────────────────────────────────────────────┘
*┌──────────────────────────────────────────────────────────────┐
*│　命名空间： JZ.IMS.IRepository                                   
*│　接口名称： IMesBatchManagerRepository                                      
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
    public interface IMesBatchManagerRepository : IBaseRepository<MesBatchManager, Decimal>
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
		Task<decimal> SaveDataByTrans(MesBatchManagerModel model);

        /// <summary>
        /// 根据批次号获取新增的数据
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        Task<TableDataModel> GetMesBatchDataByLOCNO(MesBatchManagerRequestModel model);

        /// <summary>
        /// 根据批次号判断批次管理表中是否存在该批次号
        /// </summary>
        /// <param name="locno"></param>
        /// <returns></returns>
        Task<bool> JudgeLocNoIsExistByLocNo(string locno);

        /// <summary>
        /// 根据主表批次号获取物料报表信息
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        Task<TableDataModel> GetMesMaterialInfoList(MesMaterialInfoRequestModel model);

        /// <summary>
		/// 根据主表ID获取打印时自动带出的数据
		/// </summary>
		/// <param name="ID"></param>
		/// <returns></returns>
		Task<TableDataModel> GetMesBatchPring(decimal ID);


        /// <summary>
		/// 获取标签打印上传文件表中周转箱条码最新的一条数据
		/// </summary>
		/// <param name="ID"></param>
		/// <returns></returns>
		Task<TableDataModel> GetPrintFilesToPage();



    }
}