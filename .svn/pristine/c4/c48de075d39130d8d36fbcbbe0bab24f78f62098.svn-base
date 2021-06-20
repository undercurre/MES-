/**
*┌──────────────────────────────────────────────────────────────┐
*│　描    述：手插件物料状态监听表                                                    
*│　作    者：嘉志科技                                              
*│　版    本：2.0   模板代码自动生成                                              
*│　创建时间：2019-11-19 20:26:22                           
*└──────────────────────────────────────────────────────────────┘
*┌──────────────────────────────────────────────────────────────┐
*│　命名空间： JZ.IMS.IRepository                                   
*│　接口名称： IMesHiMaterialListenRepository                                      
*└──────────────────────────────────────────────────────────────┘
*/
using JZ.IMS.Core.Repository;
using JZ.IMS.Models;
using JZ.IMS.ViewModels.HiPTS;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace JZ.IMS.IRepository
{
    public interface IMesHiMaterialListenRepository : IBaseRepository<MesHiMaterialListen, Decimal>
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
        /// 获取产线低水位数据
        /// </summary>
        /// <param name="lineId"></param>
        /// <returns></returns>
        Task<IEnumerable<MesHiMaterialListenReelsModel>> GetHiMateriaListenReelByLine(decimal lineId);
        /// <summary>
        /// 上料信息
        /// </summary>
        /// <param name="lineId"></param>
        /// <returns></returns>
        Task<IEnumerable<MesAddMaterialModel>> GetAddMaterialModel(decimal lineId);

        Task<decimal> GetWarnValueByLineId(decimal lineId);

    }
}