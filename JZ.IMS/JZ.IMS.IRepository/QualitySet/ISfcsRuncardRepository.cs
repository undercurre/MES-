/*
*┌──────────────────────────────────────────────────────────────┐
*│　描    述：                                                    
*│　作    者：嘉志科技                                              
*│　版    本：2.0   模板代码自动生成                                              
*│　创建时间：2020-04-23 10:33:57                           
*└──────────────────────────────────────────────────────────────┘
*┌──────────────────────────────────────────────────────────────┐
*│　命名空间： JZ.IMS.IRepository                                   
*│　接口名称： ISfcsRuncardRepository                                      
*└──────────────────────────────────────────────────────────────┘
*/
using JZ.IMS.Core.Repository;
using JZ.IMS.Models;
using JZ.IMS.ViewModels;
using System;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Data;

namespace JZ.IMS.IRepository
{
    public interface ISfcsRuncardRepository : IBaseRepository<SfcsRuncard, Decimal>
    {

        Task<IDbConnection> GetConnection();
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
		/// 保存数据
		/// </summary>
		/// <param name="model"></param>
		/// <returns></returns>
		Task<decimal> SaveDataByTrans(SfcsRuncardModel model);
        /// <summary>
        /// 获取操作ID
        /// </summary>
        /// <returns></returns>
        Task<Decimal> GetSFCSOperationID();

        /// <summary>
        /// 根据SN修改卡通号
        /// </summary>
        /// <param name="carton_no"></param>
        /// <param name="sn"></param>
        /// <param name="tran"></param>
        /// <returns></returns>
        Task<int> UpdateCartonNoBySN(string carton_no, string sn, IDbTransaction tran);

        /// <summary>
        /// 分布式事务
        /// </summary>
        /// <param name="carton_no"></param>
        /// <param name="sn"></param>
        /// <param name="tran"></param>
        /// <returns></returns>
        Task<int> UpdateCartonNoBySNEx(string carton_no, string sn, IDbTransaction tran);
        Decimal SpotCheckControl(string sn, string status, decimal operationID,
            decimal siteID, out string procedureMessage, out string ratio, out decimal deliverCount,
            out decimal needSpotCheck, IDbTransaction transaction);

        

    }
}