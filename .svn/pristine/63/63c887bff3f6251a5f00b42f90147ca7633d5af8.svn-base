/**
*┌──────────────────────────────────────────────────────────────┐
*│　描    述：夹具基本信息表                                                    
*│　作    者：嘉志科技                                              
*│　版    本：2.0   模板代码自动生成                                              
*│　创建时间：2019-12-20 17:39:29                           
*└──────────────────────────────────────────────────────────────┘
*┌──────────────────────────────────────────────────────────────┐
*│　命名空间： JZ.IMS.IRepository                                   
*│　接口名称： IMesTongsInfoRepository                                      
*└──────────────────────────────────────────────────────────────┘
*/
using JZ.IMS.Core.Repository;
using JZ.IMS.Models;
using JZ.IMS.ViewModels;
using JZ.IMS.ViewModels.MesTongs;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace JZ.IMS.IRepository
{
	public interface IMesTongsInfoRepository: IBaseRepository<MesTongsInfo, Decimal>
	{
		/// <summary>
		/// 获取夹具信息
		/// </summary>
		/// <param name="model"></param>
		/// <returns></returns>		
		Task<IEnumerable<MesTongsInfoListModel>> GetTongsData(MesTongsInfoRequestModel model);

		/// <summary>
		/// 导出功能
		/// </summary>
		/// <param name="model"></param>
		/// <returns></returns>
		Task<IEnumerable<dynamic>> GetExportData(MesTongsInfoRequestModel model);

		/// <summary>
		/// 获取夹具与工位绑定
		/// </summary>
		/// <param name="model"></param>
		/// <returns></returns>
		Task<TableDataModel> GetTongsSiteByCodeAsync(MesTongsSiteRequestModel model);

		/// <summary>
		/// 获取夹具信息数量
		/// </summary>
		/// <param name="model"></param>
		/// <returns></returns>
		Task<int> GetTongsDataCount(MesTongsInfoRequestModel model);

		/// <summary>
		/// 判断夹具编码是否存在
		/// </summary>
		/// <param name="code"></param>
		/// <returns></returns>
		Task<bool> IsExistCode(string code);

		/// <summary>
		/// 新增/修改夹具信息
		/// </summary>
		/// <param name="model"></param>
		/// <returns></returns>
		Task<BaseResult> AddOrModify(MesTongsInfoListModel model);


		/// <summary>
		///  申请入库（批量新增）
		/// </summary>
		/// <param name="list"></param>
		/// <param name="user"></param>
		/// <param name="organizeId"></param>
		/// <returns></returns>
		Task<BaseResult> ApplyGoStore(List<MesTongsInfoListModel> list, string user, string organizeId);

		/// <summary>
		/// 根据主键获取激活状态
		/// </summary>
		/// <param name="id">主键</param>
		/// <returns></returns>
		Task<Boolean> GetActiveStatus(decimal id);

		/// <summary>
		/// 修改激活状态
		/// </summary>
		/// <param name="id">主键</param>
		/// <param name="status">更改后的状态</param>
		/// <returns></returns>
		Task<decimal> ChangeActiveStatus(decimal id, bool status, string user);

		// <summary>
		/// 获取表的序列
		/// </summary>
		/// <returns></returns>
		Task<decimal> GetSEQID();

		/// <summary>
		/// 根据夹具ID获取对应产品信息
		/// </summary>
		/// <param name="id"></param>
		/// <returns></returns>
		Task<List<MesTongsPartModel>> GetTongsPartData(decimal id);

		/// <summary>
		/// 根据夹具ID获取夹具操作记录
		/// </summary>
		/// <param name="id"></param>
		/// <returns></returns>
		Task<List<MesTongsOperationHistoryListModel>> GetTongsOperationData(decimal id);

		/// <summary>
		/// 根据夹具ID获取保养/激活记录
		/// </summary>
		/// <param name="id"></param>
		/// <returns></returns>
		Task<List<MesTongsMaintainHistory>> GetMaintainData(decimal id);

		/// <summary>
		/// 根据夹具ID获取维修记录
		/// </summary>
		/// <param name="id"></param>
		/// <returns></returns>
		Task<List<MesTongsRepair>> GetRepairData(decimal id);

		/// <summary>
		/// 根据保养主表ID获取保养明细数据
		/// </summary>
		/// <param name="id"></param>
		/// <returns></returns>
		Task<List<MesTongsMaintainDetail>> GetMaintainDetailData(decimal id);

		/// <summary>
		/// 获取夹具已经是维修状态但还未做维修动作的维修数据
		/// </summary>
		/// <param name="tongsId"></param>
		/// <returns></returns>
		Task<MesTongsRepair> GetRepairByTongsId(decimal tongsId);

		/// <summary>
		/// 根据ID获取夹具信息
		/// </summary>
		/// <param name="id"></param>
		/// <returns></returns>
		Task<MesTongsInfoListModel> GetTongsById(decimal id);

		/// <summary>
		/// 根据料号获取产品信息
		/// </summary>
		/// <param name="partNo"></param>
		/// <returns></returns>
		Task<ImsPart> GetPartByPartNo(string partNo);

		/// <summary>
		/// 夹具入库
		/// </summary>
		/// <param name="tongsId"></param>
		/// <param name="storeId"></param>
		/// <param name="user"></param>
		/// <returns></returns>
		Task<BaseResult> EnterStore(decimal tongsId, decimal storeId, string user);

		/// <summary>
		/// 变更储位
		/// </summary>
		/// <param name="tongsId"></param>
		/// <param name="storeId"></param>
		/// <param name="user"></param>
		/// <returns></returns>
		Task<BaseResult> ChangeStore(decimal tongsId, decimal storeId, string user);

		/// <summary>
		/// 根据类型获取事项内容
		/// </summary>
		/// <param name="type">事项类型</param>
		/// <returns></returns>
		Task<List<MesTongsMaintainItemsListModel>> GetMaintainItemsData(int type, int tongsType);

		/// <summary>
		/// 开始激活
		/// </summary>
		/// <param name="tongsId"></param>
		/// <param name="user"></param>
		/// <returns></returns>
		Task<BaseResult> BeginActive(decimal tongsId, string user);

		/// <summary>
		/// 结束激活
		/// </summary>
		/// <param name="tongsId"></param>
		/// <param name="user"></param>
		/// <returns></returns>
		Task<BaseResult> EndActive(MesTongsMaintainHistory model);

		/// <summary>
		/// 领用夹具
		/// </summary>
		/// <param name="tongsId"></param>
		/// <param name="user"></param>
		/// <returns></returns>
		Task<BaseResult> BorrowTongs(decimal tongsId, string user);

		/// <summary>
		/// 永久借出夹具 
		/// </summary>
		/// <param name="tongsId"></param>
		/// <param name="user"></param>
		/// <param name="remark"></param>
		/// <returns></returns>
		Task<BaseResult> PermanentLendTongs(decimal tongsId, string user, string remark);

		/// <summary>
		/// 开始保养
		/// </summary>
		/// <param name="tongsId"></param>
		/// <param name="user"></param>
		/// <returns></returns>
		Task<BaseResult> BeginMaintain(decimal tongsId, string user);

		/// <summary>
		/// 结束保养
		/// </summary>
		/// <param name="tongsId"></param>
		/// <param name="user"></param>
		/// <returns></returns>
		Task<BaseResult> EndMaintain(MesTongsMaintainHistory model);

		/// <summary>
		/// 维修夹具
		/// </summary>
		/// <param name="model"></param>
		/// <returns></returns>
		Task<BaseResult> RepairTongs(MesTongsRepair model);

		/// <summary>
		/// 申请入库
		/// </summary>
		/// <param name="model"></param>
		/// <param name="user"></param>
		/// <param name="organizeId"></param>
		/// <returns></returns>
		Task<BaseResult> ApplyGoStoreByModel(MesTongsInfoListModel model, string user, string organizeId);

		/// <summary>
		/// 根据工单号或产品编号获取夹具信息
		/// </summary>
		/// <param name="WoNo">工单号</param>
		/// <param name="PartNo">产品编号</param>
		/// <returns></returns>
		Task<List<TongsStoreInfoModel>> GetTongsInfoByWoNo(string WoNo, string PartNo);

		/// <summary>
		/// 
		/// </summary>
		/// <param name="model"></param>
		/// <returns></returns>
		Task<List<TongsCheckListModel>> LoadPDATongsCheckList(TongsCheckRequestModel model, string organize_id);
		/// <summary>
		/// 
		/// </summary>
		/// <param name="model"></param>
		/// <returns></returns>
		Task<int> LoadPDATongsCheckListCount(TongsCheckRequestModel model);

		/// <summary>
		/// 
		/// </summary>
		/// <param name="model"></param>
		/// <param name="Tongs"></param>
		/// <param name="head"></param>
		/// <param name="detail"></param>
		/// <param name="fList"></param>
		/// <returns></returns>
		Task<int> SavePDATongsCheckData(SaveTongsCheckDataRequestModel model, MesTongsInfo Tongs, MesTongsKeepHead head, MesTongsKeepDetail detail);

		/// <summary>
		/// 获取盘点数据检验项
		/// </summary>
		/// <param name="organizeId"></param>
		/// <param name="headid"></param>
		/// <returns></returns>
		Task<List<TongsInfoInnerKeepDetail>> GetPDATongsCheckDataByHeadID(string organizeId, Decimal headid = 0);

		/// <summary>
		/// 
		/// </summary>
		/// <param name="id"></param>
		/// <returns></returns>
		Task<bool> DeletePDATongsCheckData(Decimal id);

		/// <summary>
		/// 
		/// </summary>
		/// <param name="model"></param>
		/// <returns></returns>
		Task<bool> ConfirmPDATongsCheckData(AuditTongsCheckDataRequestModel model);

		#region 工装验证

		/// <summary>
		/// 
		/// </summary>
		/// <param name="model"></param>
		/// <param name="organize_id"></param>
		/// <returns></returns>
		Task<List<TongsCheckListModel>> LoadPDATongsValidationList(TongsCheckRequestModel model, string organize_id);

		/// <summary>
		/// 
		/// </summary>
		/// <param name="model"></param>
		/// <returns></returns>
		Task<int> LoadPDATongsValidationListCount(TongsCheckRequestModel model);

		/// <summary>
		/// 
		/// </summary>
		/// <param name="organizeId"></param>
		/// <param name="headid"></param>
		/// <returns></returns>
		Task<TableDataModel> GetPDATongsValidationDataByHeadID(string organizeId, int pageInde = 1, int pageSize = 10, Decimal headid = 0);

		/// <summary>
		/// 
		/// </summary>
		/// <param name="model"></param>
		/// <returns></returns>
		Task<bool> ConfirmPDATongsValidationData(AuditTongsCheckDataRequestModel model);

		/// <summary>
		/// 
		/// </summary>
		/// <param name="model"></param>
		/// <param name="Tongs"></param>
		/// <param name="head"></param>
		/// <param name="detail"></param>
		/// <returns></returns>
		Task<int> SavePDATongsValidationData(SaveTongsValidationDataRequestModel model, MesTongsInfo Tongs, MesTongsValidationHead head, MesTongsValidationDetail detail);

		/// <summary>
		/// 
		/// </summary>
		/// <param name="model"></param>
		/// <returns></returns>
		Task<BaseResult> PDAMaintain(MesTongsMaintainHistory model);

		/// <summary>
		/// /
		/// </summary>
		/// <param name="id"></param>
		/// <returns></returns>
		Task<bool> DeletePDATongsValidationData(Decimal id);

		/// <summary>
		/// 
		/// </summary>
		/// <param name="check_code"></param>
		/// <returns></returns>
		Task<bool> QueryPDATongsValidationBy(String check_code, decimal hid);
		#endregion
	}
}