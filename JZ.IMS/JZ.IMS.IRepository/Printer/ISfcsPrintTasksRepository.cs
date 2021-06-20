/*
*┌──────────────────────────────────────────────────────────────┐
*│　描    述：打印任务表                                                    
*│　作    者：嘉志科技                                              
*│　版    本：2.0   模板代码自动生成                                              
*│　创建时间：2020-09-29 11:49:59                           
*└──────────────────────────────────────────────────────────────┘
*┌──────────────────────────────────────────────────────────────┐
*│　命名空间： JZ.IMS.IRepository                                   
*│　接口名称： ISfcsPrintTasksRepository                                      
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
    public interface ISfcsPrintTasksRepository : IBaseRepository<SfcsPrintTasks, Decimal>
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
		Task<decimal> SaveDataByTrans(SfcsPrintTasksModel model);
        /// <summary>
        /// 新增打印
        /// </summary>
        /// <param name="ID"></param>
        /// <param name="printFileId"></param>
        /// <param name="user"></param>
        /// <param name="printData"></param>
        /// <param name="wo_no">工单号</param>
        /// <param name="part_no">料号</param>
        /// <returns></returns>
        Task<Boolean> InsertPrintTask(decimal ID, decimal printFileId, String user, String printData, String wo_no, String part_no);

        /// <summary>
        /// 获取打印任务列表数据
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        Task<TableDataModel> GetPrintTasksData(GetPrintTasksRequestModel model);

        /// <summary>
        /// 添加重复打印数据
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        Task<Boolean> RepeatPrintTasks(SfcsPrintTasks model);

        /// <summary>
        /// 无码报工打印标签
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        Task<Decimal> SavePrintTasksData(SfcsPrintTasks model, SfcsContainerListListModel sclModel);

    }
}