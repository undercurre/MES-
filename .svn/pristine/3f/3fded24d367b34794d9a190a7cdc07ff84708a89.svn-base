/**
*┌──────────────────────────────────────────────────────────────┐
*│　描    述：抽检报告主表                                                    
*│　作    者：嘉志科技                                              
*│　版    本：2.0   模板代码自动生成                                              
*│　创建时间：2019-11-26 10:07:59                           
*└──────────────────────────────────────────────────────────────┘
*┌──────────────────────────────────────────────────────────────┐
*│　命名空间： JZ.IMS.IRepository                                   
*│　接口名称： IMesSpotcheckHeaderRepository                                      
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
	public interface IMesSpotcheckHeaderRepository : IBaseRepository<MesSpotcheckHeader, String>
	{
		/// <summary>
		/// 获取检验数据条目数
		/// </summary>
		/// <param name="model"></param>
		/// <returns></returns>
		Task<int> GetHeaderDataCount(MesSpotcheckHeaderRequestModel model);
		/// <summary>
		/// 获取检验数据条目数扩展
		/// </summary>
		/// <param name="model"></param>
		/// <returns></returns>
		Task<int> GetHeaderDataCountEx(MesSpotcheckHeaderRequestModel model);
		/// <summary>
		/// 获取检验数据
		/// </summary>
		/// <param name="model"></param>
		/// <returns></returns>
		Task<IEnumerable<MesSpotcheckHeaderListModel>> GetHeaderDataList(MesSpotcheckHeaderRequestModel model);
		/// <summary>
		/// 获取检验数据条目数
		/// </summary>
		/// <param name="model"></param>
		/// <returns></returns>
		Task<int> GetHeaderDataCountTwo(MesSpotcheckHeaderRequestModel model);
		/// <summary>
		/// 获取检验数据
		/// </summary>
		/// <param name="model"></param>
		/// <returns></returns>
		Task<IEnumerable<MesSpotcheckHeaderListModel>> GetHeaderDataTwoList(MesSpotcheckHeaderRequestModel model);

		/// <summary>
		/// 根据抽检批次号获取抽检数据
		/// </summary>
		/// <param name="model"></param>
		/// <returns></returns>
		Task<SpotcheckDetailListModel> GetSpotcheckDetailByBatchNo(string batchNo);

		/// <summary>
		/// 根据抽检批次号获取抽检不良明细数据
		/// </summary>
		/// <param name="model"></param>
		/// <returns></returns>
		Task<IEnumerable<SpotcheckFailDetailRequestModel>> GetSpotcheckFailDetail(PageModel model);

		/// <summary>
		/// 根据抽检批次号获取抽检不良明细数据总数
		/// </summary>
		/// <param name="model"></param>
		/// <returns></returns>
		Task<int> GetFailDetailDataCount(PageModel model);

		/// <summary>
		/// 获取所有线别
		/// </summary>
		/// <returns></returns>
		List<LineModel> GetLineDataAll();

		/// <summary>
		/// 根据检验批次号获取抽检数据
		/// </summary>
		/// <param name="batch">批次号</param>
		/// <returns></returns>
		Task<IEnumerable<MesSpotcheckDetailModel>> GetDetailDataList(string batch);

		/// <summary>
		/// 根据工单号获取产品信息
		/// </summary>
		/// <param name="wo_no">工单号</param>
		/// <returns></returns>
		Task<PartModel> GetPartDataByWoNo(string wo_no);

		/// <summary>
		/// 获取当前线别在线工单
		/// </summary>
		/// <param name="line_id"></param>
		/// <returns></returns>
		Task<string> GetWoNoByLineId(decimal line_id);

		/// <summary>
		/// 确认抽检单
		/// </summary>
		/// <param name="batch">批次号</param>
		/// <param name="user">审核人</param>
		/// <returns></returns>
		Task<int> ConfirmSpotCheck(int result, string batch, string user);

		/// <summary>
		/// 审核抽检单
		/// </summary>
		/// <param name="batch">批次号</param>
		/// <param name="auditUser">审核人</param>
		/// <returns></returns>
		Task<int> AuditSpotCheck(int resultStatus, string batch, string auditUser);

		/// <summary>
		/// 审核抽检单
		/// </summary>
		/// <param name="batch">批次号</param>
		/// <param name="auditUser">审核人</param>
		/// <returns></returns>
		Task<int> VerifySpotcheckHeader(VerifySpotCheckRequestModel model);

		/// <summary>
		/// 更新过程检验单并修改完工检验单
		/// </summary>
		/// <param name="model"></param>
		/// <param name="header"></param>
		/// <param name="qcDoc"></param>
		/// <returns></returns>
		Task<int> UpdateSpotCheckData(VerifySpotCheckRequestModel model, MesSpotcheckHeader header, QcDocListModel qcDoc);

		/// <summary>
		/// 更新抽检项目数据
		/// </summary>
		/// <param name="model"></param>
		/// <returns></returns>
		Task<int> UpdateSpotCheckIteamsData(MesSpotcheckIteamsRequestModel model);

		/// <summary>
		/// 保存QC过程检验的抽检明细
		/// </summary>
		/// <param name="model"></param>
		/// <returns></returns>
		Task<int> SaveFailDetailData(FailDetailRequestModel model, Decimal? qc_type);

		/// <summary>
		/// 删除抽检单
		/// </summary>
		/// <param name="batch"></param>
		/// <returns></returns>
		Task<int> DeleteSpotCheck(string batch);

		/// <summary>
		/// 根据抽检批次号和SN删除质检明细数据
		/// </summary>
		/// <param name="batch_no">抽检批次号</param>
		/// <param name="sn">产品流水号</param>
		/// <returns></returns>
		Task<int> DeleteSpotCheckDetailByBatchNo(string batch_no, string sn);

        /// <summary>
        /// 判断当前批次号的抽检单是否存在
        /// </summary>
        /// <param name="batch">批次号</param>
        /// <returns></returns>
        bool IsExits(string batch);

		/// <summary>
		/// 根据批次号获取当前单据状态
		/// </summary>
		/// <param name="batch">批次号</param>
		/// <returns></returns>
		int GetStatusByBatch(string batch);

		/// <summary>
		/// 获取最大批次
		/// </summary>
		/// <param name="lineId"></param>
		/// <param name="wo_no"></param>
		/// <returns></returns>
		decimal GetOrderNo(decimal lineId, string wo_no);
		/// <summary>
		/// 获取抽检数是否大于明细数量
		/// </summary>
		/// <param name="batch"></param>
		/// <returns></returns>
		bool IsCheckQtyGTDetailQty(string batch);

		#region 明细
		/// <summary>
		/// 根据类型获取数据字典
		/// </summary>
		/// <param name="type"></param>
		/// <returns></returns>
		List<SfcsParameters> GetParametersByType(string type);

		/// <summary>
		/// 获取不良数据
		/// </summary>
		/// <param name="model"></param>
		/// <returns></returns>
		Task<IEnumerable<SfcsDefectConfigListModel>> LoadDefectData(SfcsDefectConfigRequestModel model);

		/// <summary>
		/// 新增或修改明细
		/// </summary>
		/// <param name="model"></param>
		/// <returns></returns>
		Task<int> AddOrEditDetail(MesSpotcheckDetailModel model);

		/// <summary>
		/// 删除明细数据
		/// </summary>
		/// <param name="id"></param>
		/// <returns></returns>
		Task<int> DeleteDetail(decimal id, string batch);
		#endregion
	}
}