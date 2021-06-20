/**
*┌──────────────────────────────────────────────────────────────┐
*│　描    述：                                                    
*│　作    者：嘉志科技                                              
*│　版    本：2.0   模板代码自动生成                                              
*│　创建时间：2020-03-18 11:07:21                           
*└──────────────────────────────────────────────────────────────┘
*┌──────────────────────────────────────────────────────────────┐
*│　命名空间： JZ.IMS.IRepository                                   
*│　接口名称： ISmtFeederRepairRepository                                      
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
    public interface ISmtFeederRepairRepository : IBaseRepository<SmtFeederRepair, Decimal>
    {
        // <summary>
        /// 获取表的序列
        /// </summary>
        /// <returns></returns>
        Task<decimal> GetSEQID();

        /// <summary>
        /// 保存数据
        /// </summary>
        /// <param name="model"></param>
        /// <param name="feeder_id"></param>
        /// <returns></returns>
        Task<decimal> SaveDataByTrans(SmtFeederRepairAddOrModifyModel model, decimal feeder_id);

        /// <summary>
        /// 获取飞达注册信息
        /// </summary>
        /// <param name="feeder">飞达编号</param>
        /// <returns></returns>
        Task<SmtFeeder> GetFeederInfo(string feeder);

        /// <summary>
        /// 获取报修飞达列表
        /// </summary>
        /// <param name="model">model</param>
        /// <returns></returns>
        Task<TableDataModel> GetFeeder2RepairList(SmtFeederRepairRequestModel model);

        /// <summary>
        /// 获取根本原因列表
        /// </summary>
        /// <param name="model">model</param>
        /// <returns></returns>
        Task<TableDataModel> GetReasonList(SmtFeederRepairRequestModel model);

        /// <summary>
        /// 获取损坏部件列表
        /// </summary>
        /// <param name="model">model</param>
        /// <returns></returns>
        Task<TableDataModel> GetDamagePartList(SmtFeederRepairRequestModel model);

        /// <summary>
        /// 获取检查項目列表
        /// </summary>
        /// <param name="model">model</param>
        /// <returns></returns>
        Task<TableDataModel> GetRepairItemsList(SmtFeederRepairRequestModel model);

        /// <summary>
        /// 获取维修结果列表
        /// </summary>
        /// <param name="model">model</param>
        /// <returns></returns>
        Task<List<IDNAME>> GetResultList();

        /// <summary>
        /// 获取不良记录列表
        /// </summary>
        /// <param name="feeder">飞达编号</param>
        /// <returns></returns>
        Task<List<CodeName>> GetDefectList(string feeder);

        /// <summary>
        /// 获取维修记录列表
        /// </summary>
        /// <param name="feeder_id">飞达ID</param>
        /// <returns></returns>
        Task<List<SmtFeederRepairListModel>> GetRepairList(decimal feeder_id);

        /// <summary>
        /// 获取本月维修次数
        /// </summary>
        /// <param name="feeder">飞达ID</param>
        /// <returns></returns>
        Task<decimal> GetRepairCountByMonth(decimal feeder_id);

        /// <summary>
        /// 获取总计维修次数
        /// </summary>
        /// <param name="feeder">飞达ID</param>
        /// <returns></returns>
        Task<decimal> GetRepairTotalCount(decimal feeder_id);

        /// <summary>
        /// 保存报废数据
        /// </summary>
        /// <param name="model"></param>
        /// <param name="feeder_id"></param>
        /// <returns></returns>
        Task<decimal> SaveFeederScrap(SmtFeederScrapModel model, decimal feeder_id);

        /// <summary>
        /// 获取维修结果列表
        /// </summary>
        /// <param name="model">model</param>
        /// <returns></returns>
        Task<List<IDNAME>> GetRepairResultList();
    }
}