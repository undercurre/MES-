/*
*┌──────────────────────────────────────────────────────────────┐
*│　描    述：                                                    
*│　作    者：嘉志科技                                              
*│　版    本：2.0   模板代码自动生成                                              
*│　创建时间：2021-01-27 11:50:30                           
*└──────────────────────────────────────────────────────────────┘
*┌──────────────────────────────────────────────────────────────┐
*│　命名空间： JZ.IMS.IRepository                                   
*│　接口名称： IMesPartShelfRepository                                      
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
    public interface IMesPartShelfRepository : IBaseRepository<MesPartShelf, Decimal>
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
		/// <param name="isCode"></param>
		/// <returns></returns>
		Task<decimal> SaveDataByTrans(MesPartShelfModel model, String isCode);

        /// <summary>
		///工单是否存在
		/// </summary>
		/// <param name="woNo">工单编号</param>
		/// <returns></returns>
        Task<bool> SfcsWoByUsed(String woNo);


        /// <summary>
        /// 获取领料清单
        /// </summary>
        /// <param name="WoNo">工单</param>
        /// <returns></returns>
        Task<TableDataModel> GetPickingListData(MesCheckMaterialRequestModel model);

        /// <summary>
		/// 条码是否存在
		/// </summary>
		/// <param name="reelCode">条码code</param>
		/// <returns></returns>
		Task<bool> ImsReelByUsed(String reelCode);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="reelCode"></param>
        /// <returns></returns>
		Task<bool> CartonNoByUsed(String reelCode);

        /// <summary>
        /// 判断核料是否成功
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        Task<TableDataModel> CheckPickingByReelCode(MesCheckMaterialRequestModel model, String isCode);

        /// <summary>
        ///条码是否使用过
        /// </summary>
        /// <param name="reelCode">条码code</param>
        /// <returns></returns>
        Task<bool> PartDetailByUsed(String reelCode);

        /// <summary>
        /// 通过工单获取物料储位
        /// </summary>
        /// <param name="woNo">工单号</param>
        /// <returns></returns>
        Task<TableDataModel> GetShelfByWONO(MesPartShelfRequestModel model);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="reelCode"></param>
        /// <returns></returns>
        Task<bool> PartShelfByUsed(String reelCode);
    }
}