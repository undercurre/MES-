/**
*┌──────────────────────────────────────────────────────────────┐
*│　描    述：                                                    
*│　作    者：嘉志科技                                              
*│　版    本：2.0   模板代码自动生成                                              
*│　创建时间：2020-04-06 16:48:13                           
*└──────────────────────────────────────────────────────────────┘
*┌──────────────────────────────────────────────────────────────┐
*│　命名空间： JZ.IMS.IRepository                                   
*│　接口名称： ISmtPlacementHeaderRepository                                      
*└──────────────────────────────────────────────────────────────┘
*/
using JZ.IMS.Core.Repository;
using JZ.IMS.Models;
using JZ.IMS.ViewModels;
using System;
using System.Threading.Tasks;
using System.Collections.Generic;
using JZ.IMS.ViewModels.SmtLineSet;

namespace JZ.IMS.IRepository
{
    public interface ISmtPlacementHeaderRepository : IBaseRepository<SmtPlacementHeader, Decimal>
    {   
		// <summary>
        /// 获取表的序列
        /// </summary>
        /// <returns></returns>
		Task<decimal> GetSEQID();

        /// <summary>
        ///获取AI机台列表
        /// </summary>
        /// <returns></returns>
        Task<List<IDNAME>> GetStationList(string Type);


        /// <summary>
        /// 获取西门子机台列表
        /// </summary>
        /// <param name="Type"></param>
        /// <returns></returns>
        Task<List<dynamic>> GetStationListBySIEMENS(string Type);

        /// <summary>
        /// 获取料单主表列表
        /// </summary>
        /// <param name="model">model</param>
        /// <returns></returns>
        Task<TableDataModel> GetSmtPlacementHeaderList(SmtPlacementHeaderRequestModel model);

        /// <summary>
        ///  料单编辑之保存数据
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        Task<decimal> SaveDataByTrans(SmtPlacementSaveModel model);

        /// <summary>
        ///  AI料单上传之保存数据
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        Task<AIPlacementSaveResult> AIPlacementSave(PlacementSaveModel model);

        /// <summary>
        /// 西门子
        /// </summary>
        /// <param name="placementModel"></param>
        /// <returns></returns>
        Task<AIPlacementSaveResult> SiemensPlacementSave(PlacementSaveModel placementModel);
    }
}