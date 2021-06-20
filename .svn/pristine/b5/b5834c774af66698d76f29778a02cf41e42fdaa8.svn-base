/*
*┌──────────────────────────────────────────────────────────────┐
*│　描    述：                                                    
*│　作    者：嘉志科技                                              
*│　版    本：2.0   模板代码自动生成                                              
*│　创建时间：2020-09-09 11:49:39                           
*└──────────────────────────────────────────────────────────────┘
*┌──────────────────────────────────────────────────────────────┐
*│　命名空间： JZ.IMS.IRepository                                   
*│　接口名称： ISfcsCollectDefectsRepository                                      
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
    public interface ISfcsCollectDefectsRepository : IBaseRepository<SfcsCollectDefects, Decimal>
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
		Task<decimal> SaveDataByTrans(SfcsCollectDefectsModel model);

        /// <summary>
        /// 保存维修数据
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        Task<string> SaveRepairData(SfcsCollectDefectsModel model);


        /// <summary>
        /// 获取线体下拉框数据
        /// </summary>
        /// <returns></returns>
        Task<TableDataModel> GetLINENAMEList(DropDownBoxRequestModel model);

        /// <summary>
        /// 根据线体、站位、工序获取站位信息
        /// </summary>
        /// <returns></returns>
        Task<TableDataModel> GetRepairSiteData(SfcsCollectRepairSiteRequestModel model);

        /// <summary>
        /// 获取原因代码下拉框数据
        /// </summary>
        /// <returns></returns>
        Task<TableDataModel> GetReasonCodeList(DropDownBoxRequestModel model);

        /// <summary>
        /// 获取排除故障下拉框数据
        /// </summary>
        /// <returns></returns>
        Task<TableDataModel> GetResponserList(DropDownBoxRequestModel model);

        /// <summary>
        /// 获取站点下拉框数据
        /// </summary>
        /// <returns></returns>
        Task<TableDataModel> GetSITENAMEList(SiteNameRequestModel model);

        /// <summary>
        /// 获取工序下拉框数据
        /// </summary>
        /// <returns></returns>
        Task<TableDataModel> GetOPERNAMEList(DropDownBoxRequestModel model);

        /// <summary>
        /// 获取SN下拉框
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        Task<TableDataModel> GetSnDataList(DropDownBoxRequestModel model);



        /// <summary>
        /// 根据工序ID获取未维修数量
        /// </summary>
        /// <param name="OPER_ID"></param>
        /// <returns></returns>
        Task<decimal> GetRefreshUnrepairedQty(decimal? OPER_ID);


        /// <summary>
        /// 根据SN获取不良维修等信息
        /// </summary>
        /// <param name="SN"></param>
        /// <returns></returns>
        Task<List<SfcsCollectBadDataListModel>> GetDefectDataBySN(SfcsCollectBadRequestModel model);

        /// <summary>
        /// 不良代码是否需要维修
        /// </summary>
        /// <param name="COLLECT_DEFECT_ID">采集ID</param>
        /// <returns>T:需要维修 F:不需要维修</returns>
        Task<bool> CheckCollectDefectNeedRepair(decimal COLLECT_DEFECT_ID);

        /// <summary>
        /// 报废功能
        /// </summary>
        /// <param name="SN"></param>
        /// <returns></returns>
        Task<bool> SaveScrappedData(String SN);
    }
}