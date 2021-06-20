/*
*┌──────────────────────────────────────────────────────────────┐
*│　描    述：自动产生序列号记录表，使用在自动产生卡通号，自动产生栈板号等。                                                    
*│　作    者：嘉志科技                                              
*│　版    本：2.0   模板代码自动生成                                              
*│　创建时间：2020-10-08 15:38:28                           
*└──────────────────────────────────────────────────────────────┘
*┌──────────────────────────────────────────────────────────────┐
*│　命名空间： JZ.IMS.IRepository                                   
*│　接口名称： ISfcsContainerListRepository                                      
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
    public interface ISfcsContainerListRepository : IBaseRepository<SfcsContainerList, String>
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
        /// 保存数据
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        //Task<decimal> SaveDataByTrans(SfcsContainerListModel model);

        /// <summary>
        /// 根据SN替换箱号
        /// </summary>
        /// <param name="carton_no">箱号</param>
        /// <param name="sn_id">产品流水号ID</param>
        /// <returns></returns>
        Task<int> UpdateCartonNoBySN(string carton_no, string oldCARTON_NO, decimal sn_id);
        Task<int> UpdateCartonNoBySN(string carton_no, string sn);
        Task<int> UpdateCartonNoBySN(string carton_no, string sn, IDbTransaction tran);

        /// <summary>
        /// 删除卡通号的数据
        /// </summary>
        /// <param name="carton_no"></param>
        /// <param name="sn"></param>
        /// <param name="tran"></param>
        /// <returns></returns>
        Task<int> DelCartonNoBySN(string carton_no, string sn, IDbTransaction tran);
        /// <summary>
        /// 根据卡通号（箱号）获取相关信息
        /// </summary>
        /// <param name="model">箱号</param>
        /// <returns></returns>
        Task<CartonInfoListModel> GetCartonInfoByCartonNo(SfcsContainerListRequestModel model);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        Task<decimal> SetCatonFullByCaton(PackageFullRequestModel model);

        /// <summary>
        /// 返工业务
        /// </summary>
        /// <param name="_sfcsReworkRepository"></param>
        /// <param name="oldSN"></param>
        /// <param name="tran"></param>
        /// <returns></returns>
        Task<bool> ReworkProcessBySN(ISfcsReworkRepository _sfcsReworkRepository, string oldSN,string userName);
    }
}