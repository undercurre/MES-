using JZ.IMS.Core.Repository;
using JZ.IMS.Models;
using JZ.IMS.ViewModels;
using JZ.IMS.ViewModels;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace JZ.IMS.IRepository
{
	public interface IMesPartTempWarehouseRepository : IBaseRepository<MesPartTempWarehouse, Decimal>
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
		/// 获取库存数据
		/// </summary>
		/// <param name="model"></param>
		/// <returns></returns>
		Task<IEnumerable<MesPartTempWarehouseListModel>> GetMstData(MesPartTempWarehouseRequestModel model);

		/// <summary>
		/// 获取库存信息数量
		/// </summary>
		/// <param name="model"></param>
		/// <returns></returns>
		Task<int> GetMstDataCount(MesPartTempWarehouseRequestModel model);

		/// <summary>
		/// 获取明细数据
		/// </summary>
		/// <param name="mstId">库存主表ID</param>
		/// <returns></returns>
		Task<IEnumerable<MesPartTempDetailListModel>> GetDetailData(decimal mstId);

		/// <summary>
		/// 获取操作记录数据
		/// </summary>
		/// <param name="mstId">库存主表ID</param>
		/// <returns></returns>
		Task<IEnumerable<MesPartTempRecordListModel>> GetRecordData(decimal mstId, decimal? detailId);

		/// <summary>
		/// 获取系统参数
		/// </summary>
		/// <param name="type"></param>
		/// <returns></returns>
		List<SfcsParameters> GetParametersByType(string type);

		/// <summary>
		/// 判断物料编码是否已经存在库存数据
		/// </summary>
		/// <param name="partNo"></param>
		/// <returns></returns>
		Task<bool> IsExistPartNo(string partNo);

		/// <summary>
		/// 判断物料条码是否在库存明细中存在
		/// </summary>
		/// <param name="reel_id"></param>
		/// <returns></returns>
		Task<bool> IsExistReelDetail(string reel_id);

		/// <summary>
		/// 根据物料编码获取下一个该出库的物料条码
		/// </summary>
		/// <param name="part_no"></param>
		/// <returns></returns>
		Task<string> GetNextReelId(string part_no);

		/// <summary>
		/// 根据物料唯一标识获取物料信息（入库）
		/// </summary>
		/// <param name="reel_id"></param>
		/// <returns></returns>
		Task<MesPartTempReelModel> GetReelDataInput(string reel_id);

        /// <summary>
		/// 根据物料唯一标识获取物料信息（入库）
		/// </summary>
		/// <param name="reel_id"></param>
		/// <returns></returns>
		Task<MesPartTempReelModel> GetReelDataInputByAndroid(string reel_id);

        /// <summary>
        /// 根据物料唯一标识获取物料信息（出库）
        /// </summary>
        /// <param name="reel_id"></param>
        /// <returns></returns>
        Task<MesPartTempReelModel> GetReelDataOutput(string reel_id);

		/// <summary>
		/// 获取所有线别列表
		/// </summary>
		/// <returns></returns>
		List<SfcsEquipmentLinesModel> GetLinesList();

		/// <summary>
		/// 入库操作
		/// </summary>
		/// <param name="item"></param>
		/// <returns></returns>
		Task<BaseResult> InputWarehouseData(MesPartTempWarehouseAddOrModifyModel item);

		/// <summary>
		/// 出库操作
		/// </summary>
		/// <param name="item"></param>
		/// <returns></returns>
		Task<BaseResult> OutputWarehouseData(MesPartTempWarehouseAddOrModifyModel item);

        /// <summary>
		/// 确认拆分并且返回
		/// </summary>
		/// <param name="item"></param>
		/// <returns></returns>
		Task<BaseResult> ReelCodeSplitAsync(MesPartTempReelModel item);

        /// <summary>
		/// 修改原条码数量
		/// </summary>
		/// <param name="item"></param>
		/// <returns></returns>
		Task<BaseResult> ReelCodeUpdate(MesPartTempReelModel item);

        /// <summary>
        /// 解析条码
        /// </summary>
        /// <param name="item"></param>
        /// <returns></returns>
        Task<Reel> GetReel(string reelCode);

		/// <summary>
		/// 确认拆分并且返回新条码 
		/// </summary>
		/// <param name="model"></param>
		/// <returns></returns>
		Task<TableDataModel> ReelCodeSplitAsyncEx(MesReelCodeSplitModel model);

	}
}
