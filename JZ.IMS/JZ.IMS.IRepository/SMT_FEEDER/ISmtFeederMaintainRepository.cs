/**
*┌──────────────────────────────────────────────────────────────┐
*│　描    述：                                                    
*│　作    者：嘉志科技                                              
*│　版    本：2.0   模板代码自动生成                                              
*│　创建时间：2020-03-17 08:55:48                           
*└──────────────────────────────────────────────────────────────┘
*┌──────────────────────────────────────────────────────────────┐
*│　命名空间： JZ.IMS.IRepository                                   
*│　接口名称： ISmtFeederMaintainRepository                                      
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
    public interface ISmtFeederMaintainRepository : IBaseRepository<SmtFeederMaintain, Decimal>
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
        /// 保存数据
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        Task<decimal> SaveDataByTrans(SmtFeederMaintainAddOrModifyModel model);


        /// <summary>
        ///获取维修明细
        /// </summary>
        /// <param name="id">料架编号</param>
        /// <returns></returns>
        Task<TableDataModel> GetFedderMaintainList(SmtFeederIDModel model);


        /// <summary>
        ///料架编号FEEDER 判断是否存在
        /// </summary>
        /// <param name="FEEDER">料架编号</param>
        /// <returns></returns>
        Task<SmtFeeder> ItemIsByFeeder(string FEEDER);


        /// <summary>
        ///获取飞达信息By ID
        /// </summary>
        /// <param name="id">料架编号ID</param>
        /// <returns></returns>
        Task<SmtFeeder> GetSmtFeederByFeederID(decimal? ID);
        


    }
}