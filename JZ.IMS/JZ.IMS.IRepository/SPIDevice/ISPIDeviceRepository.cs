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

namespace JZ.IMS.IRepository
{
    public interface ISPIDeviceRepository : IBaseRepository<SmtSpiSrcGen, Decimal>
    {   
        /// <summary>
        /// 通过SN找工单
        /// </summary>
        /// <param name="SN"></param>
        /// <returns></returns>
        Task<string> GetWONOBySN(string SN);

        /// <summary>
        /// 保存SPIGEN数据
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        Task<decimal> SaveGENDataByTrans(SmtSpiSrcGenModel model);

        /// <summary>
        /// 保存SPIDET数据
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        Task<decimal> SaveDETDataByTrans(SmtSpiSrcDetModel model);

        /// <summary>
        /// 获取AOI数据
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        Task<bool> GetAOIData(SPIModel model);

    }
}