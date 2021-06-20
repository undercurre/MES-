/*
*┌──────────────────────────────────────────────────────────────┐
*│　描    述：                                                    
*│　作    者：嘉志科技                                              
*│　版    本：2.0   模板代码自动生成                                              
*│　创建时间：2020-04-14 14:53:45                           
*└──────────────────────────────────────────────────────────────┘
*┌──────────────────────────────────────────────────────────────┐
*│　命名空间： JZ.IMS.IRepository                                   
*│　接口名称： ISfcsRuncardRangerRepository                                      
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
    public interface ISfcsRuncardRangerRepository : IBaseRepository<SfcsRuncardRanger, Decimal>
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
		Task<decimal> GetSEQID(String seq = "SFCS_RUNCARD_RANGER_SEQ");

        /// <summary>
		///项目是否已被使用 
		/// </summary>
		/// <param name="id">项目id</param>
		/// <returns></returns>
		Task<bool> ItemIsByUsed(decimal id);

        /// <summary>
		/// 加载数据
		/// </summary>
		/// <returns></returns>
		Task<TableDataModel> LoadData(SfcsRuncardRangerRequestModel model);

        /// <summary>
        /// 保存数据
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        Task<decimal> SaveDataByTrans(SfcsRuncardRanger model);

        /// <summary>
        /// 修改打印状态
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<decimal> UpdatePrint(decimal id);

        /// <summary>
        /// 修改打印状态
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<decimal> UpdatePrintImportRuncard(decimal id);

        /// <summary>
        /// 特殊SN记录
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        Task<decimal> UpdateCustomerSNData(ImportRuncardSnAddOrModifyModel model,decimal header_id);

        /// <summary>
        /// 特殊SN记录的主表
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        Task<decimal> AddImportRuncardHeader(ImportRuncardSnAddOrModifyModel model);

        /// <summary>
        /// 特殊SN记录的主表的SN导入数量
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        Task<decimal> upSNHeaderByQty(decimal id, int qty);

        /// <summary>
        /// 拼板单码打印
        /// </summary>
        /// <param name="printTasks"></param>
        /// <param name="headerList"></param>
        /// <param name="detailList"></param>
        /// <returns></returns>
        Task<bool> PrintPuzzleSingleCode(SfcsPrintTasks printTasks, List<SfcsWoRgMultiHeader> headerList, List<SfcsWoRgMultiDetail> detailList);

        /// <summary>
        /// 拼板余码打印
        /// </summary>
        /// <param name="printTasks"></param>
        /// <param name="headerModel"></param>
        /// <returns></returns>
        Task<bool> PrintPuzzleRemainingCodeBySN(SfcsPrintTasks printTasks, SfcsWoRgMultiHeader headerModel);

        #region 导入特殊SN相关

        /// <summary>
        /// 加载数据
        /// </summary>
        /// <returns></returns>
        Task<TableDataModel> LoadImportRuncardSnSummaryData(ImportRuncardSnRequestModel model);

        /// <summary>
        /// 加载数据
        /// </summary>
        /// <returns></returns>
        Task<TableDataModel> LoadImportRuncardHeaderData(ImportRuncardHeaderRequestModel model);

        /// <summary>
        /// 加载数据
        /// </summary>
        /// <returns></returns>
        Task<TableDataModel> LoadImportRuncardSnData(ImportRuncardSnRequestModel model);

        /// <summary>
        /// 保存数据
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        Task<decimal> SaveImportRuncardSnDataByTrans(ImportRuncardSnModel model);

        /// <summary>
        /// 保存数据
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        Task<decimal> SaveImportRuncardHeaderByTrans(ImportRuncardHeaderModel model);
        #endregion

        #region 镭雕任务下达

        /// <summary>
        ///项目是否已被使用 
        /// </summary>
        /// <param name="id">项目id</param>
        /// <returns></returns>
        Task<bool> ItemIsByLaserTask(Decimal type_id, String task_type);

        /// <summary>
        /// 镭雕任务下达数据列表
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        Task<TableDataModel> LoadLaserTaskData(SfcsLaserTaskRequestModel model);

        /// <summary>
        /// 保存镭雕任务下达数据
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        Task<decimal> SaveLaserTaskDataByTrans(SfcsLaserTaskModel model);

        /// <summary>
        /// 更新镭雕任务状态(批量)
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        Task<decimal> UpdateLaserTaskStatus(UpdateLaserTaskStatusRequestModel model);

        /// <summary>
        /// 判断打印下达任务ID
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<decimal> GetSfcsLaserTask(List<LaserTaskStatusRequestModel> laserTaskStatusRequestModels);
        /// <summary>
        /// 删除镭雕任务数据
        /// </summary>
        /// <param name="laserTaskStatusRequestModels"></param>
        /// <returns></returns>
        Task<decimal> DeleLaserTasks(List<LaserTaskStatusRequestModel> laserTaskStatusRequestModels);
        #endregion

    }
}