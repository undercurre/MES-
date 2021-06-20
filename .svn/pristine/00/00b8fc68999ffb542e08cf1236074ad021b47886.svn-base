/**
*┌──────────────────────────────────────────────────────────────┐
*│　描    述：                                                    
*│　作    者：嘉志科技                                              
*│　版    本：2.0   模板代码自动生成                                              
*│　创建时间：2020-03-17 11:59:41                           
*└──────────────────────────────────────────────────────────────┘
*┌──────────────────────────────────────────────────────────────┐
*│　命名空间： JZ.IMS.IRepository                                   
*│　接口名称： ISmtLinesRepository                                      
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
    public interface ISmtLinesRepository : IBaseRepository<SmtLines, Decimal>
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
		///项目是否已被使用 
		/// </summary>
		/// <param name="id">项目id</param>
		/// <returns></returns>
		Task<bool> ItemIsByUsed(decimal id);

        /// <summary>
		/// 保存数据
		/// </summary>
		/// <param name="model"></param>
		/// <returns></returns>
		Task<decimal> SaveDataByTrans(SmtLinesModel model);

        /// <summary>
        /// 获取线列下拉表
        /// </summary>
        Task<List<object>> GetList(string USER_ID = null);

        /// <summary>
        /// 获取机台下拉表
        /// </summary>
        /// <returns></returns>
        Task<List<object>> GetRoutStation(string linesid);

        /// <summary>
        /// 获取线别配置信息
        /// </summary>
        /// <param name="lineId"></param>
        /// <returns></returns>
        Task<List<object>> GetLinesconfig(string lineId);

        /// 获取机台配置信息
        /// </summary>
        /// <param name="lineId"></param>
        /// <returns></returns>
        Task<List<object>> GetStationconfig(string stationid);

        /// <summary>
        /// 保存线体和机台
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        Task<decimal> SaveLineAndStation(PatchlineconfigModel model);

        /// <summary>
        /// 获取字典默认值
        /// </summary>
        /// <param name="code"></param>
        /// <returns></returns>
        Task<List<IDNAME>> GetLoopUpDefualt(string code);

        /// <summary>
        /// 查询SMT线别数据
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        Task<TableDataModel> LoadSMTLinesData(SmtLinesRequestModel model);

        /// <summary>
        /// 获取机台接口
        /// </summary>
        /// <returns></returns>
        Task<List<SmtStationConfig>> GetStationconfigMachineName();
    }
}