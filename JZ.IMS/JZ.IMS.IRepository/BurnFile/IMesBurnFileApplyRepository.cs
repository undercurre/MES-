/*
*┌──────────────────────────────────────────────────────────────┐
*│　描    述：                                                    
*│　作    者：嘉志科技                                              
*│　版    本：2.0   模板代码自动生成                                              
*│　创建时间：2020-07-22 10:16:13                           
*└──────────────────────────────────────────────────────────────┘
*┌──────────────────────────────────────────────────────────────┐
*│　命名空间： JZ.IMS.IRepository                                   
*│　接口名称： IMesBurnFileApplyRepository                                      
*└──────────────────────────────────────────────────────────────┘
*/
using JZ.IMS.Core.Repository;
using JZ.IMS.Models;
using JZ.IMS.ViewModels;
using System;
using System.Threading.Tasks;
using System.Collections.Generic;
using JZ.IMS.ViewModels.BurnFile;

namespace JZ.IMS.IRepository
{
    public interface IMesBurnFileApplyRepository : IBaseRepository<MesBurnFileApply, Decimal>
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
        /// 获取文件申请的序列
        /// </summary>
        /// <returns></returns>
		Task<decimal> GetFileApplySEQID();

        /// <summary>
        /// 获取文件表的序列
        /// </summary>
        /// <returns></returns>
        Task<decimal> GetFIleManagerSEQID();

        /// <summary>
        /// 获取烧录表的序列
        /// </summary>
        /// <returns></returns>
        Task<decimal> GetBurnFIleSEQID();

        /// <summary>
		///项目是否已被使用 
		/// </summary>
		/// <param name="id">项目id</param>
		/// <returns></returns>
		Task<bool> ItemIsByUsed(decimal id);

        /// <summary>
        /// 获取文件关联分页
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        Task<TableDataModel> GetApplyRelationLoadData(MesBurnApplyRelationRequestModel model);

        /// <summary>
        /// 绑定文件保存
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        Task<decimal> SaveApplyAndRelationDataByTrans(BurnFileApplyAndRelation model);

        /// <summary>
        /// 保存烧录文件
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        Task<TableDataModel> SaveFileManagerDataByTrans(MesBurnFileManagerModel model);

        /// <summary>
        /// 保存申请
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        Task<decimal> SaveFileApplyDataByTrans(MesBurnFileApplyModel model);

        /// <summary>
        ///  保存申请文件关联表
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        Task<decimal> SaveApplyRelationDataByTrans(MesBurnApplyRelationModel model);
        /// <summary>
        /// 保存文件下载
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        Task<TableDataModel> SaveFileDownDataByTrans(MesBurnFileDownModel model);

        /// <summary>
        /// 保存文件下载历史记录表
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        Task<decimal> SaveFileDownHistoryByTrans(MesBurnFileDownHistoryModel model);

        /// <summary>
        /// 保存SN数据
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        Task<decimal> SaveBurnSNByTrans(MesBurnSnDownModel model);

        /// <summary>
        /// 下载记录文件
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        Task<TableDataModel> GetDownLoadData(MesBurnFileDownRequestModel model);

        /// <summary>
        ///  获取烧录文件的具体信息
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        Task<List<MesBurnFileManager>> GetMesBurnManagerByNo(string WO_NO);

        /// <summary>
        /// 获取申请文件通过工单
        /// </summary>
        /// <param name="WO_NO"></param>
        /// <returns></returns>
        Task<List<MesBurnFileApply>> GetMesFileApplyByWONO(string WO_NO);

        /// <summary>
        /// 通过SN找工单
        /// </summary>
        /// <param name="SN"></param>
        /// <returns></returns>
        Task<TableDataModel> GetWONOBySN(string SN);

        /// <summary>
        /// 下载地址的保存
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        Task<TableDataModel> DownAddressByTrans(BurnFileaddressModel model);

        /// <summary>
        /// 烧录结果保存到数据库
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        Task<Boolean> InsertBurnResult(MesBurnFileResultRequestModel model);
    }
}