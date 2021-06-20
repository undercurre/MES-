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
    public interface IAteDeviceRepository : IBaseRepository<MesBurnFileApply, Decimal>
    {   
        /// <summary>
        /// 通过SN找工单
        /// </summary>
        /// <param name="SN"></param>
        /// <returns></returns>
        Task<string> GetWONOBySN(string SN);

        /// <summary>
        /// 检查SN状态和制程是否漏刷
        /// </summary>
        /// <param name="SN"></param>
        /// <param name="siteId"></param>
        /// <returns></returns>
        Task<TableDataModel> CheckSNAndRoute(string SN, decimal siteId);


        /// <summary>
        /// 普通过站
        /// </summary>
        /// <param name="SN"></param>
        /// <param name="operationID"></param>
        /// <param name="siteId"></param>
        /// <returns></returns>
        Task<TableDataModel> ProcessMultiOperation(string SN, decimal operationID, decimal siteId);
    }
}