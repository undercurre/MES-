/*
*┌──────────────────────────────────────────────────────────────┐
*│　描    述：                                                    
*│　作    者：嘉志科技                                              
*│　版    本：2.0   模板代码自动生成                                              
*│　创建时间：2020-04-10 19:51:17                           
*└──────────────────────────────────────────────────────────────┘
*┌──────────────────────────────────────────────────────────────┐
*│　命名空间： JZ.IMS.IRepository                                   
*│　接口名称： ISfcsPrintFilesRepository                                      
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
    public interface ISfcsPrintFilesRepository : IBaseRepository<SfcsPrintFiles, Decimal>
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
        /// 文件名字是否 
        /// </summary>
        /// <param name="filename"></param>
        /// <returns></returns>
        Task<bool> IsExistFileNmae(string filename);

        /// <summary>
		/// 保存数据
		/// </summary>
		/// <param name="model"></param>
		/// <returns></returns>
		Task<decimal> SaveDataByTrans(SfcsPrintFilesModel model);
        /// <summary>
        /// 标签打印设计文件上传数据更新
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        Task<int> UpdateDataByTrans(SfcsPrintFilesAddOrModifyModel model, string type);
        /// <summary>
        /// 查询打印文件信息
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<dynamic> GetPrintFiles(string id);

        Task<int> SavePrintFilesDetail(SfcsPrintFilesDetailAddOrModifyModel model);

    }
}