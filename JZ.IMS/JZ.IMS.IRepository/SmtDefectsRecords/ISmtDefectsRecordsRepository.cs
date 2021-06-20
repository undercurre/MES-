/*
*┌──────────────────────────────────────────────────────────────┐
*│　描    述：                                                    
*│　作    者：嘉志科技                                              
*│　版    本：2.0   模板代码自动生成                                              
*│　创建时间：2020-10-15 10:41:26                           
*└──────────────────────────────────────────────────────────────┘
*┌──────────────────────────────────────────────────────────────┐
*│　命名空间： JZ.IMS.IRepository                                   
*│　接口名称： ISmtDefectsRecordsRepository                                      
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
    public interface ISmtDefectsRecordsRepository : IBaseRepository<SmtDefectsRecords, Decimal>
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
		Task<decimal> SaveDataByTrans(SmtDefectsRecordsModel model);

        /// <summary>
        /// 查询报表数据
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        Task<dynamic> GetReportDefectsRecordsList(string conditions, SmtDefectsRecordsRequestModel model);

        /// <summary>
        /// 保存明细数据
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        Task<decimal> SaveReportDefectsRecordDtl(SmtDefectsRecordDtlModel model);

        /// <summary>
        /// 获取明细数据
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        Task<dynamic> GetReportDefectsRecordDtlList(SmtDefectsRecordDtlRequestModel model);

        /// <summary>
        /// 取消审核
        /// </summary>
        /// <returns></returns>
        Task<decimal> CancelCheck(int id,string examineUser);

        /// <summary>
        /// 审核
        /// </summary>
        /// <returns></returns>
        Task<decimal> Check(int id, string examineUser);

        /// <summary>
        /// 获取工单信息
        /// </summary>
        /// <param name="LineId"></param>
        /// <returns></returns>
        Task<SmtWo> GetWoInfoByLine(string LineId);
    }
}