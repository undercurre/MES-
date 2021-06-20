/**
*┌──────────────────────────────────────────────────────────────┐
*│　描    述：                                                    
*│　作    者：嘉志科技                                              
*│　版    本：2.0   模板代码自动生成                                              
*│　创建时间：2020-03-11 10:19:01                           
*└──────────────────────────────────────────────────────────────┘
*┌──────────────────────────────────────────────────────────────┐
*│　命名空间： JZ.IMS.IRepository                                   
*│　接口名称： ISmtFeederRepository                                      
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
    public interface ISmtFeederRepository : IBaseRepository<SmtFeeder, Decimal>
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
        ///获取飞达状态类型
        /// </summary>
        /// <returns></returns>
        Task<List<IDNAME>> GetStatus();

        /// <summary>
        /// 保存数据
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        Task<decimal> SaveDataByTrans(SmtFeederModel model);

        /// <summary>
		/// 查询列表
		/// </summary>
		/// <param name="model"></param>
		/// <returns></returns>
		Task<TableDataModel> LoadData(SmtFeederRequestModel model);

        /// <summary>
        /// 导出分页
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        Task<TableDataModel> GetExportData(SmtFeederRequestModel model);

        /// <summary>
        ///料架编号FEEDER 判断是否存在
        /// </summary>
        /// <param name="FEEDER">料架编号</param>
        /// <returns></returns>
        Task<SmtFeeder> ItemIsByFeeder(string FEEDER, decimal ID = 0);

        /// <summary>
        /// 更新条码替换
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        Task<decimal> SaveUpdateByTrans(SmtFeederModel model);

        /// <summary>
        /// 保存PDA飞达盘点数据
        /// </summary>
        /// <param name="model"></param>
        /// <param name="feeder"></param>
        /// <param name="head"></param>
        /// <returns></returns>
        Task<int> SavePDAFeederCheckData(SaveFeederCheckDataRequestModel model, SmtFeeder feeder, SfcsFeederKeepHead head, SfcsFeederKeepDetail detail, List<GetFeederInfoListModel> fList);

        /// <summary>
        /// 删除PDA飞达盘点数据记录
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<bool> DeletePDAFeederCheckData(Decimal id);

        Task<bool> ConfirmPDAFeederCheckData(AuditFeederCheckDataRequestModel model);

        Task<List<FeederCheckListModel>> LoadPDAFeederCheckList(FeederCheckRequestModel model);

        Task<int> LoadPDAFeederCheckListCount(FeederCheckRequestModel model);

        /// <summary>
        /// 获取飞达类型集
        /// </summary>
        /// <returns></returns>
        Task<List<CodeName>> GetFeederTypeList();

        /// <summary>
        /// 保存数据
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        Task<decimal> SaveFeederRegionDataByTrans(SmtFeederRegionModel model);

        /// <summary>
		/// 查询列表
		/// </summary>
		/// <param name="model"></param>
		/// <returns></returns>
		Task<TableDataModel> LoadeFeederRegionData(SmtStencilRegionRequestModel model);


    }
}