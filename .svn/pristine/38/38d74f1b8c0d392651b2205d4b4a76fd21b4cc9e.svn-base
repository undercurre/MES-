/**
*┌──────────────────────────────────────────────────────────────┐
*│　描    述：看板-每小时产能记录表                                                    
*│　作    者：嘉志科技                                              
*│　版    本：2.0   模板代码自动生成                                              
*│　创建时间：2020-05-25 16:55:23                           
*└──────────────────────────────────────────────────────────────┘
*┌──────────────────────────────────────────────────────────────┐
*│　命名空间： JZ.IMS.IRepository                                   
*│　接口名称： IMesKanbanHourYidldRepository                                      
*└──────────────────────────────────────────────────────────────┘
*/
using JZ.IMS.Core.Repository;
using JZ.IMS.Models;
using JZ.IMS.ViewModels;
using JZ.IMS.ViewModels.MesKanbanHourYidld;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace JZ.IMS.IRepository
{
    public interface IMesKanbanHourYidldRepository : IBaseRepository<MesKanbanHourYidld, Decimal>
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
        /// 重写查询语句获取数据
        /// </summary>
        /// <param name="pageNumber"></param>
        /// <param name="rowsPerPage"></param>
        /// <param name="conditions"></param>
        /// <param name="orderby"></param>
        /// <param name="parameters"></param>
        /// <returns></returns>
        Task<IEnumerable<MesKanbanHourYidld>> GetDataPagedAsync(int pageNumber, int rowsPerPage, string conditions, string orderby, object parameters = null);

        /// <summary>
        /// 获取总数量
        /// </summary>
        /// <returns></returns>
        new Task<int> RecordCountAsync(string conditions, object parameters = null);

        /// <summary>
        /// 重写查询语句获取数据
        /// </summary>
        /// <param name="pageNumber"></param>
        /// <param name="rowsPerPage"></param>
        /// <param name="conditions"></param>
        /// <param name="orderby"></param>
        /// <param name="parameters"></param>
        /// <returns></returns>
        Task<IEnumerable<MesKanbanHourYidld>> GetReportListPagedAsync(int pageNumber, int rowsPerPage, string conditions, string orderby, object parameters = null);

        /// <summary>
        /// 获取总数量
        /// </summary>
        /// <returns></returns>
        Task<int> RecordReportCountAsync(string conditions, object parameters = null);
        /// <summary>
        /// 获取单价
        /// </summary>
        /// <returns></returns>
        List<PriceTable> GetPriceListAsync(int pageNumber, int rowsPerPage, string conditions, object parameters = null);
    }
}