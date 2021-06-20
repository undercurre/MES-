/*
*┌──────────────────────────────────────────────────────────────┐
*│　描    述：                                                    
*│　作    者：嘉志科技                                              
*│　版    本：2.0   模板代码自动生成                                              
*│　创建时间：2020-09-14 11:46:24                           
*└──────────────────────────────────────────────────────────────┘
*┌──────────────────────────────────────────────────────────────┐
*│　命名空间： JZ.IMS.IRepository                                   
*│　接口名称： ISfcsReworkRepository                                      
*└──────────────────────────────────────────────────────────────┘
*/
using JZ.IMS.Core.Repository;
using JZ.IMS.Models;
using JZ.IMS.ViewModels;
using System;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Data;

namespace JZ.IMS.IRepository
{
    public interface ISfcsReworkRepository : IBaseRepository<SfcsRework, Decimal>
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
		Task<decimal> SaveDataByTrans(SfcsReworkModel model);

        /// <summary>
        /// 保存传事务进来
        /// </summary>
        /// <param name="model"></param>
        /// <param name="tran"></param>
        /// <returns></returns>
        Task<decimal> SaveDataByTrans(SfcsReworkModel model, IDbTransaction tran, IDbConnection dbConnection);

        /// <summary>
        /// 根据流水号查出返工作业数据信息
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        Task<SfcsReworkListModel> GetReworkDataBySN(SfcsReworkRequestModel model);

        /// <summary>
        /// 根据新工单号查询数据
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        Task<SfcsReworkListModel> GetNewReworkDataByNewNo(SfcsReworkRequestModel model);

        /// <summary>
        /// 查询新工单数据
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        Task<TableDataModel> GetNewWorkNoData(SfcsWoNeWorkRequestModel model);

        /// <summary>
        /// 获取旧工单的SN列表（分页）
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        Task<WoTransferListModel> GetSNDataByOldWoOrCartonNo(SfcsWoNeWorkRequestModel model);

        /// <summary>
        /// 保存工单转移数据
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        Task<int> SaveWoReplaceByType(SaveWoTransferListModel model,int t_type);

    }
}