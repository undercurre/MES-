/**
*┌──────────────────────────────────────────────────────────────┐
*│　描    述：                                                    
*│　作    者：嘉志科技                                              
*│　版    本：2.0   模板代码自动生成                                              
*│　创建时间：2020-05-19 09:10:09                           
*└──────────────────────────────────────────────────────────────┘
*┌──────────────────────────────────────────────────────────────┐
*│　命名空间： JZ.IMS.IRepository                                   
*│　接口名称： IMesQualityInfoRepository                                      
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
    public interface IMesQualityInfoRepository : IBaseRepository<MesQualityInfo, Decimal>
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
		/// 删除数据
		/// </summary>
		/// <param name="mstId"></param>
		/// <returns></returns>
		Task<BaseResult> DeleteData(decimal mstId);

        /// <summary>
        /// 获取检验明细数据
        /// </summary>
        /// <param name="mstId"></param>
        /// <returns></returns>
        Task<IEnumerable<MesQualityCheckDetailListModel>> GetDetailData(decimal mstId);

        /// <summary>
        /// 新增修改明细数据
        /// </summary>
        /// <param name="item"></param>
        /// <returns></returns>
        Task<BaseResult> AddOrModifyDetailSave(List<MesQualityDetailAddOrModifyModel> item, decimal RESULT_STATUS, string REMARK, decimal MST_ID);

        /// <summary>
        /// 删除明细数据
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<BaseResult> DeleteDetail(decimal id);

        /// <summary>
        /// 获取检验明细项目信息
        /// </summary>
        /// <param name="detailId"></param>
        /// <returns></returns>
        Task<IEnumerable<dynamic>> GetDetailItemData(decimal detailId);

        /// <summary>
        /// 审核检验信息
        /// </summary>
        /// <param name="item"></param>
        /// <returns></returns>
        Task<BaseResult> AuditData(MesQualityInfoAddOrModifyModel item);

        /// <summary>
        /// 通过日期+批号+板型获取信息
        /// </summary>
        /// <param name="item"></param>
        /// <returns></returns>
        Task<BaseResult> GetDetailCount(MesQualityInfoAddOrModifyModel item);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="mstId"></param>
        /// <returns></returns>
        Task<IEnumerable<MesFirstCheckRecordDetailListModel>> GetMesFirstCheckDetailData(decimal mstId);

        /// <summary>
        /// 获取BOM明细项目信息
        /// </summary>
        /// <param name="mst_id"></param>
        /// <returns></returns>
        Task<IEnumerable<dynamic>> GetDetailBOMData(decimal mst_id, String parent_part_no_sql);

        /// <summary>
		/// 新增/修改物料确认明细
		/// </summary>
		/// <param name="item"></param>
		/// <param name="RESULT_STATUS"></param>
		/// <param name="REMARK"></param>
		/// <param name="MST_ID"></param>
		/// <returns></returns>
        Task<BaseResult> AddOrModifyBOMDetailSave(List<MesFirstCheckRecordDetailAddOrModifyModel> itemList, decimal RESULT_STATUS, string REMARK, decimal MST_ID);
    }

}