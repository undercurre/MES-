/*
*┌──────────────────────────────────────────────────────────────┐
*│　描    述：                                                    
*│　作    者：嘉志科技                                              
*│　版    本：2.0   模板代码自动生成                                              
*│　创建时间：2020-04-17 11:14:59                           
*└──────────────────────────────────────────────────────────────┘
*┌──────────────────────────────────────────────────────────────┐
*│　命名空间： JZ.IMS.IRepository                                   
*│　接口名称： ISfcsStoplineHistoryRepository                                      
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
    public interface ISfcsStoplineHistoryRepository : IBaseRepository<SfcsStoplineHistory, Decimal>
    {   
		/// <summary>
        /// 获取表的序列
        /// </summary>
        /// <returns></returns>
		Task<decimal> GetSEQID();

		/// <summary>
		/// 加载数据
		/// </summary>
		/// <returns></returns>
		Task<TableDataModel> LoadData(SfcsStoplineHistoryRequestModel model);

		/// <summary>
		/// 获取停线不良统计
		/// </summary>
		/// <param name="headerID"></param>
		/// <returns></returns>
		Task<List<dynamic>> GetStopLineDefectStatistics(decimal headerID);

		/// <summary>
		/// 获取停线所有流水号
		/// </summary>
		/// <param name="headerID"></param>
		/// <param name="classification">厂部</param>
		/// <returns></returns>
		Task<List<StopLineDefectDetail>> GetStopLineDefectDetail(decimal headerID, decimal classification);

		/// <summary>
		/// 保存数据
		/// </summary>
		/// <param name="model"></param>
		/// <param name="hasMaintainHistory"></param>
		/// <returns></returns>
		Task<decimal> SaveDataByTrans(SfcsStoplineMaintainHistoryAddOrModifyModel model, bool hasMaintainHistory);
	}
}