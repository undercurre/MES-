/**
*┌──────────────────────────────────────────────────────────────┐
*│　描    述：                                                    
*│　作    者：嘉志科技                                              
*│　版    本：2.0   模板代码自动生成                                              
*│　创建时间：2020-03-04 15:39:22                           
*└──────────────────────────────────────────────────────────────┘
*┌──────────────────────────────────────────────────────────────┐
*│　命名空间： JZ.IMS.IRepository                                   
*│　接口名称： IImsReelRepository                                      
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
    public interface IImsReelRepository : IBaseRepository<ImsReel, Decimal>
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
        /// 获取reel条码信息
        /// </summary>
        /// <param name="reelCode"></param>
        /// <returns></returns>
        Task<ReelInfoViewModel> GetReelInfoViewModel(String reelCode);
        /// <summary>
        /// 获取料号ID
        /// </summary>
        /// <param name="partNo"></param>
        /// <returns></returns>
        Task<decimal> GetPartId(String partNo);

        /// <summary>
        /// 获取供应商ID
        /// </summary>
        /// <param name="VendorCode"></param>
        /// <returns></returns>
        Task<decimal> GetVendorId(String VendorCode);
        /// <summary>
        /// 
        /// </summary>
        /// <param name="reelInfoViewModel"></param>
        /// <returns></returns>
        Task<bool> KeepVendorBarcode(ReelInfoViewModel reelInfoViewModel);
        /// <summary>
        /// 同步物料条码数据
        /// </summary>
        /// <param name="reelInfoViewModel"></param>
        /// <returns></returns>
        Task<bool> KeepVendorBarcodeInWMS(ReelInfoViewModel reelInfoViewModel);

        /// <summary>
        /// 获取供应商列表
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        Task<IEnumerable<VendorListModel>> GetVendorList(ImsPartRequestModel model);

        /// <summary>
		/// 获取供应商列表条数
		/// </summary>
		/// <param name="model"></param>
		/// <returns></returns>
		Task<int> GetVendorListCount(ImsPartRequestModel model);

        /// <summary>
        /// 获取物料信息列表
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        Task<IEnumerable<ImsPartListModel>> GetImsPartList(ImsPartRequestModel model);

        /// <summary>
		/// 获取物料信息列表条数
		/// </summary>
		/// <param name="model"></param>
		/// <returns></returns>
		Task<int> GetImsPartListCount(ImsPartRequestModel model);

        /// <summary>
        /// 保存物料条码信息并生成打印数据
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        Task<ReelPrintListModel> SaveReelPrintInfo(ReelPrintRequestModel model);

    }
}