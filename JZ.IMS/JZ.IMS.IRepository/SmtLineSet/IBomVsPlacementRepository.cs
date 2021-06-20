using JZ.IMS.Core.Repository;
using JZ.IMS.Core.Utilities;
using JZ.IMS.Models;
using JZ.IMS.ViewModels;
using JZ.IMS.ViewModels.BomVsPlacement;
using JZ.IMS.WebApi.Common;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace JZ.IMS.IRepository
{
    public interface IBomVsPlacementRepository : IBaseRepository<SmtBom1, Decimal>
    {
        /// <summary>
        /// 料单线别
        /// </summary>
        /// <returns></returns>
        Task<List<CodeName>> GetStationKind();

        /// <summary>
        /// 同步ERP的数据到SMT_BOM1、STM_BOM2
        /// </summary>
        /// <param name="ProductId"></param>
        /// <param name="Type"></param>
        /// <param name="userName"></param>
        /// <returns></returns>
        Task<string> SyncBomByProdectId(string ProductId, string Type, string userName);

        /// <summary>
        /// 获取BOM信息
        /// </summary>
        /// <param name="sheetNo"></param>
        /// <param name="bomtype"></param>
        /// <returns></returns>
        Task<List<BOMData>> ExploreBom2(string sheetNo, BomType bomtype);

        /// <summary>
        /// 获取机台的配置
        /// </summary>
        /// <param name="StationName"></param>
        /// <returns></returns>
        Task<SmtStationConfig> GetStationConfig(string StationName, List<string> stationIdArray);

    }
}