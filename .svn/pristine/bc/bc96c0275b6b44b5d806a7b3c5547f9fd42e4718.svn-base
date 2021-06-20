/*
*┌──────────────────────────────────────────────────────────────┐
*│　描    述：制程品质异常停线通知单表                                                    
*│　作    者：嘉志科技                                              
*│　版    本：2.0   模板代码自动生成                                              
*│　创建时间：2020-11-02 11:09:31                           
*└──────────────────────────────────────────────────────────────┘
*┌──────────────────────────────────────────────────────────────┐
*│　命名空间： JZ.IMS.IRepository                                   
*│　接口名称： IMesIpqaStopNoticeRepository                                      
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
    public interface IMesIpqaStopNoticeRepository : IBaseRepository<MesIpqaStopNotice, Decimal>
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
		Task<decimal> SaveDataByTrans(MesIpqaStopNoticeModel model);

        /// <summary>
        /// 分页查询
        /// </summary>
        Task<TableDataModel> LoadDataPagedList(MesIpqaStopNoticeRequestModel model);

        /// <summary>
        /// 线体数据
        /// </summary>
        /// <param name="organizeId"></param>
        /// <param name="lineType">SMT</param>
        /// <returns></returns>
        Task<TableDataModel> GetLinesList(string organizeId = "1", string lineType = "");

        /// <summary>
		/// 获取单据状态
		/// </summary>
		Task<decimal> GetBillStatus(decimal id);

        /// <summary>
        ///根据工单号获取产品信息 
        /// </summary>
        Task<PartModel> GetPartDataByWoNo(string wo_no);


        /// <summary>
        ///获取当前线别在线工单 
        /// </summary>
        /// <param name="line_id">线别ID</param>
        /// <returns></returns>
        Task<string> GetWoNoByLineId(decimal line_id);

        /// <summary>
        /// 审核
        /// </summary>
        Task<decimal> AuditBill(MesIpqaStopNoticeAuditBillRequestModel model);

        /// <summary>
        /// 批准
        /// </summary>
        Task<decimal> ApprovalBill(MesIpqaStopNoticeApprovalBillRequestModel model);
    }
}