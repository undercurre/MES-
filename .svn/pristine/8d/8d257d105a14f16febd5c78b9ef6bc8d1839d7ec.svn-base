/*
*┌──────────────────────────────────────────────────────────────┐
*│　描    述：离线备料表                                                    
*│　作    者：嘉志科技                                              
*│　版    本：2.0   模板代码自动生成                                              
*│　创建时间：2020-07-15 13:47:58                           
*└──────────────────────────────────────────────────────────────┘
*┌──────────────────────────────────────────────────────────────┐
*│　命名空间： JZ.IMS.IRepository                                   
*│　接口名称： IMesOffLineReelRepository                                      
*└──────────────────────────────────────────────────────────────┘
*/
using JZ.IMS.Core.Repository;
using JZ.IMS.Models;
using JZ.IMS.ViewModels;
using System;
using System.Threading.Tasks;
using System.Collections.Generic;
using JZ.IMS.ViewModels.OfflineMaterials;

namespace JZ.IMS.IRepository
{
    public interface IMesOffLineReelRepository : IBaseRepository<MesOffLineReel, Decimal>
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
		///项目是否已被使用 
		/// </summary>
		/// <param name="id">项目id</param>
		/// <returns></returns>
		Task<bool> ItemIsByUsed(decimal id);

        /// <summary>
        /// 检测配置事项
        /// </summary>
        /// <param name="lineType"></param>
        /// <returns></returns>
        Task<bool> IsCheckConfig(string lineType);

        /// <summary>
        /// 根据线别 、生产时间、判断状态 、产前确认状态来查找
        /// </summary>
        /// <param name="LINE_ID"></param>
        /// <returns></returns>
        Task<List<MesProductionPreMst>> GetProductPreMst(string LINE_ID);

        /// <summary>
        /// 检测子项是否有备料事项
        /// </summary>
        /// <returns></returns>
        Task<List<MesProductionPreDtl>> IsCheckProductPreDlt(decimal id);
        /// <summary>
        /// 通过条码获取物料
        /// </summary>
        /// <returns></returns>
        Task<List<ImsPart>> GetImsPart(string reel_id);

        /// <summary>
        /// 获取BOM2的数据
        /// Part_No:产品编号
        /// </summary>
        /// <returns></returns>
        Task<List<SmtBom2>> GetBom2(string Part_No);

        /// <summary>
        /// 获取所有的料号
        /// PRE_MST_NO:产前确认编号
        /// </summary>
        /// <returns></returns>
        Task<List<dynamic>> GetALLPartNo(string PRE_MST_NO);

        /// <summary>
        /// 保存数据
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        Task<decimal> SaveDataByTrans(MesOffLineReelModel model);

        /// <summary>
        /// 保存产前确认子项
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        Task<decimal> SaveDTLItme(MesProductionPreMstModel model);

        /// <summary>
        /// 离线备料
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        Task<decimal> SaveDataOfflineUnloading(OfflineUnloadingModel model);
    }
}