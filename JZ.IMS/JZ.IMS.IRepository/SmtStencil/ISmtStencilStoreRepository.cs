/**
*┌──────────────────────────────────────────────────────────────┐
*│　描    述：                                                    
*│　作    者：嘉志科技                                              
*│　版    本：2.0   模板代码自动生成                                              
*│　创建时间：2020-03-10 18:09:05                           
*└──────────────────────────────────────────────────────────────┘
*┌──────────────────────────────────────────────────────────────┐
*│　命名空间： JZ.IMS.IRepository                                   
*│　接口名称： ISmtStencilStoreRepository                                      
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
    /// <summary>
    /// 
    /// </summary>
    public interface ISmtStencilStoreRepository : IBaseRepository<SmtStencilStore, Decimal>
    {   
		// <summary>
        /// 获取表的序列
        /// </summary>
        /// <returns></returns>
		Task<decimal> GetSEQID();

        /// <summary>
		/// 根据条件获取列表数量
		/// </summary>
		/// <param name="model"></param>
		/// <returns></returns>
		Task<IEnumerable<SmtStencilStoreListModel>> Loadata(SmtStencilStoreRequestModel model);

		/// <summary>
		/// 根据条件获取总记录数
		/// </summary>
		/// <param name="model"></param>
		/// <returns></returns>
		Task<int> GetTotalCount(SmtStencilStoreRequestModel model);

		/// <summary>
		///获取钢网存储信息
		/// </summary>
		/// <param name="id">项目id</param>
		/// <returns></returns>
		Task<SmtStencilStore> GetLocationInfo(string location);

		/// <summary>
		///获取钢网注册信息 
		/// </summary>
		/// <param name="id">项目id</param>
		/// <returns></returns>
		Task<SmtStencilConfig> GetStencil(string stencil_no);

		/// <summary>
		///获取钢网注册信息 byID
		/// </summary>
		/// <param name="ID">项目id</param>
		/// <returns></returns>
		Task<SmtStencilConfig> GetStencilByID(decimal ID);

		/// <summary>
		/// 如果發現該儲位對應的網板信息已經被刪除,則自動清除儲位信息
		/// </summary>
		/// <param name="userName"></param>
		/// <param name="location"></param>
		/// <param name="STENCIL_ID"></param>
		/// <returns></returns>
		Task<decimal> DeleteStencilLocation(string userName, string location, decimal STENCIL_ID);

		/// <summary>
		/// 判斷網板是否有清洗過
		/// </summary>
		/// <param name="stencilNo"></param>
		/// <returns></returns>
		Task<bool> CheckStencilCleaned(string stencilNo);

		/// <summary>
		///获取钢网存储信息By STENCIL_ID 
		/// </summary>
		/// <param name="id">项目id</param>
		/// <returns></returns>
		Task<SmtStencilStore> GetStencilStoreBySTENCIL_ID(decimal STENCIL_ID);

		/// <summary>
		/// 保存数据
		/// </summary>
		/// <param name="model"></param>
		/// <param name="stencil_id"></param>
		/// <returns></returns>
		Task<decimal> SaveDataByTrans(SmtStencilStoreModel model, decimal stencil_id);

		/// <summary>
		/// 报废出柜
		/// </summary>
		/// <param name="stencil_id">钢网ID</param>
		/// <param name="location">钢网储位</param>
		/// <param name="userName">操作人</param>
		/// <returns></returns>
		Task<decimal> ScrapStencilStore(decimal stencil_id, string location, string userName);

		/// <summary>
		/// 变更储位保存
		/// </summary>
		/// <param name="stencil_id">钢网ID</param>
		/// <param name="new_location">新钢网储位</param>
		/// <param name="new_location">旧钢网储位</param>
		/// <param name="userName">操作人</param>
		/// <returns></returns>
		Task<decimal> ChangeLocationSave(decimal stencil_id, string new_location, string location, string userName);

		/// <summary>
		///获取可用钢网注册信息 
		/// </summary>
		/// <param name="id">项目id</param>
		/// <returns></returns>
		Task<SmtStencilConfig> GetStencil2Enabled(string stencil_no);

		/// <summary>
		///获取钢网运行时间 
		/// </summary>
		/// <param name="id">项目id</param>
		/// <returns></returns>
		Task<SmtStencilRuntime> GetStencilRuntime(string stencil_no);

		/// <summary>
		/// 获取钢网清洗记录
		/// </summary>
		/// <param name="stencil_no"></param>
		/// <returns></returns>
		Task<SmtStencilCleanHistory> GetStencilCleanHistory(string stencil_no);

		/// <summary>
		/// 钢网领用保存
		/// </summary>
		/// <param name="model"></param>
		/// <param name="stencilStore"></param>
		/// <returns></returns>
		Task<decimal> StencilTakeSave(SmtStencilTakeSaveModel model, CheckScrapResult stencilStore);

		/// <summary>
		/// 获取钢网储位
		/// </summary>
		/// <param name="stencilNo"></param>
		/// <returns></returns>
		Task<string> GetStencilLocation(string stencilNo);

		/// <summary>
		/// 获取钢网是否清洗
		/// </summary>
		/// <param name="stencil_id">钢网ID</param>
		/// <returns></returns>
		Task<bool> StencilIsCleanedBefore(decimal stencil_id);

		/// <summary>
		/// 钢网归还保存
		/// </summary>
		/// <param name="model"></param>
		/// <param name="stencilStore"></param>
		/// <returns></returns>
		Task<decimal> StencilReturnSave(SmtStencilReturnSaveModel model, CheckScrapResult stencilStore);

		/// <summary>
		///获取用户信息
		/// </summary>
		/// <param name="id">用户账号</param>
		/// <returns></returns>
		Task<Sys_Manager> GetSysManager(string User_Name);

		/// <summary>
		/// 钢网清洗保存
		/// </summary>
		/// <param name="model"></param>
		/// <returns></returns>
		Task<decimal> StencilCleanSave(SmtStencilCleanSaveModel model);

		/// <summary>
		/// 获取钢网清洗记录列表
		/// </summary>
		/// <param name="stencil_no"></param>
		/// <returns></returns>
		Task<List<SmtStencilCleanHistory>> GetStencilCleanHistoryList(string stencil_no);

		/// <summary>
		///获取状态列表 
		/// </summary>
		/// <returns></returns>
		Task<List<IDNAME>> GetStatus();

		/// <summary>
		/// 钢网保养保存
		/// </summary>
		/// <param name="model"></param>
		/// <param name="stencil_id"></param>
		/// <returns></returns>
		Task<decimal> StencilMaintainSave(SmtStencilMaintainModel model, decimal stencil_id);

		/// <summary>
		/// 获取钢网保养记录列表
		/// </summary>
		/// <param name="stencil_no"></param>
		/// <returns></returns>
		Task<List<SmtStencilMaintainHistory>> GetStencilMaintainHistoryList(string stencil_no);

		/// <summary>
		/// 钢网品质柏拉图
		/// </summary>
		/// <param name="STENCIL_NO"></param>
		/// <param name="pageModel"></param>
		/// <returns></returns>
		Task<TableDataModel> GetStencilUseData(string STENCIL_NO, PageModel pageModel);
	}
}