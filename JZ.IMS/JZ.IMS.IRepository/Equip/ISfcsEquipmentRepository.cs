/**
*┌──────────────────────────────────────────────────────────────┐
*│　描    述：设备基本信息表                                                    
*│　作    者：嘉志科技                                              
*│　版    本：2.0   模板代码自动生成                                              
*│　创建时间：2019-10-28 17:48:27                           
*└──────────────────────────────────────────────────────────────┘
*┌──────────────────────────────────────────────────────────────┐
*│　命名空间： JZ.IMS.IRepository                                   
*│　接口名称： ISfcsEquipmentRepository                                      
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
    public interface ISfcsEquipmentRepository : IBaseRepository<SfcsEquipment, Decimal>
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
		/// 根据主键获取设备
		/// </summary>
		/// <param name="id">主键</param>
		/// <returns></returns>
		SfcsEquipmentListModel GetModelById(decimal id);

		/// <summary>
		/// 根据条件获取设备数量
		/// </summary>
		/// <param name="model">条件对象</param>
		/// <returns></returns>
		Task<int> GetEquipmentListCount(SfcsEquipmentRequestModel model);

		/// <summary>
		/// 根据条件获取设备数据
		/// </summary>
		/// <param name="model">条件对象</param>
		/// <returns></returns>
		Task<IEnumerable<SfcsEquipmentListModel>> GetEquipmentList(SfcsEquipmentRequestModel model);

        /// <summary>
		/// 根据唯一码获取设备数据
		/// </summary>
		/// <param name="model">条件对象</param>
		/// <returns></returns>
		SfcsEquipmentListModel GetEquipment(string ONLY_CODE);

        /// <summary>
        /// 根据ID修改设备状态（用于回写）
        /// </summary>
        /// <param name="id">设备ID</param>
        /// <param name="status">设备状态</param>
        /// <returns></returns>
        Task<int> EditEquipStatus(decimal id, decimal status);


		/// <summary>
		/// 获取导出数据
		/// </summary>
		/// <param name="model"></param>
		/// <returns></returns>
		Task<TableDataModel> GetExportData(SfcsEquipmentRequestModel model);

        #region 设备盘点

        /// <summary>
        /// 
        /// </summary>
        /// <param name="organizeId"></param>
        /// <param name="headid"></param>
        /// <returns></returns>
        Task<List<EquipmentInfoInnerKeepDetail>> GetPDAEquipmentCheckDataByHeadID(Decimal headid = 0);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="model"></param>
        /// <param name="organize_id"></param>
        /// <returns></returns>
        Task<List<EquipmentCheckListModel>> LoadPDAEquipmentCheckList(EquipmentCheckRequestModel model);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        Task<int> LoadPDAEquipmentCheckListCount(EquipmentCheckRequestModel model);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<bool> DeletePDAEquipmentCheckData(Decimal id);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="model"></param>
        /// <param name="Equipment"></param>
        /// <param name="head"></param>
        /// <param name="detail"></param>
        /// <returns></returns>
        Task<int> SavePDAEquipmentCheckData(SaveEquipmentCheckDataRequestModel model, SfcsEquipment Equipment, SfcsEquipmentKeepHead head, SfcsEquipmentKeepDetail detail);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        Task<bool> ConfirmPDAEquipmentCheckData(AuditEquipmentCheckDataRequestModel model);

        #endregion

        #region 设备验证
        /// <summary>
        /// 
        /// </summary>
        /// <param name="model"></param>
        /// <param name="Equipment"></param>
        /// <param name="head"></param>
        /// <param name="detail"></param>
        /// <returns></returns>
        Task<int> SavePDAEquipmentValidationData(SaveEquipmentValidationDataRequestModel model, SfcsEquipment Equipment, SfcsEquipmentValidationHead head, SfcsEquipValidationDetail detail);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        Task<List<EquipmentCheckListModel>> LoadPDAEquipmentValidationList(EquipmentCheckRequestModel model);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        Task<int> LoadPDAEquipmentValidationListCount(EquipmentCheckRequestModel model);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="headid"></param>
        /// <returns></returns>
        Task<List<EquipmentInfoInnerValidationDetail>> GetPDAEquipmentValidationDataByHeadID(Decimal headid = 0);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        Task<bool> ConfirmPDAEquipmentValidationData(AuditEquipmentCheckDataRequestModel model);

        /// <summary>
        /// /
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<bool> DeletePDAEquipmentValidationData(Decimal id);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="check_code"></param>
        /// <param name="hid"></param>
        /// <returns></returns>
        Task<bool> QueryPDAEquipmentValidationBy(String check_code, decimal hid);
        #endregion

    }
		
}