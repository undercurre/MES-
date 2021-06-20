/**
*┌──────────────────────────────────────────────────────────────┐
*│　描    述：后台管理员                                                    
*│　作    者：Admin                                              
*│　版    本：1.0   模板代码自动生成                                              
*│　创建时间：2018-12-18 13:28:43                           
*└──────────────────────────────────────────────────────────────┘
*┌──────────────────────────────────────────────────────────────┐
*│　命名空间： JZ.IMS.IRepository                                   
*│　接口名称： IManagerRepository                                      
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
    public interface IManagerRepository : IBaseRepository<Sys_Manager, decimal>
    {
		/// <summary>
		/// 逻辑删除返回影响的行数
		/// </summary>
		/// <param name="ids">需要删除的主键数组</param>
		/// <returns>影响的行数</returns>
		decimal DeleteLogical(decimal ids);
        /// <summary>
        /// 逻辑删除返回影响的行数（异步操作）
        /// </summary>
        /// <param name="ids">需要删除的主键数组</param>
        /// <returns>影响的行数</returns>
        Task<decimal> DeleteLogicalAsync(decimal ids);

        /// <summary>
        /// 真删除返回影响的行数（异步操作）扩展
        /// </summary>
        /// <param name="ids">需要删除的主键数组</param>
        /// <returns>影响的行数</returns>
        Task<decimal> DeleteLogicalAsyncEx(decimal ids);

        /// <summary>
        /// 根据主键获取锁定状态
        /// </summary>
        /// <param name="id">主键</param>
        /// <returns></returns>
        Boolean GetLockStatusById(decimal id);

        /// <summary>
        /// 根据主键获取锁定状态
        /// </summary>
        /// <param name="id">主键</param>
        /// <returns></returns>
        Task<Boolean> GetLockStatusByIdAsync(decimal id);

		/// <summary>
		/// 修改锁定状态
		/// </summary>
		/// <param name="id">主键</param>
		/// <param name="status">更改后的状态</param>
		/// <returns></returns>
		decimal ChangeLockStatusById(decimal id,bool status);

        /// <summary>
        /// 修改锁定状态
        /// </summary>
        /// <param name="id">主键</param>
        /// <param name="status">更改后的状态</param>
        /// <returns></returns>
        Task<decimal> ChangeLockStatusByIdAsync(decimal id, bool status);

        /// <summary>
        /// 通过主键获取密码
        /// </summary>
        /// <param name="Id">主键</param>
        /// <returns></returns>
        Task<string> GetPasswordByIdAsync(decimal Id);

        /// <summary>
        /// 修改密码
        /// </summary>
        /// <param name="id">用户ID</param>
        /// <param name="password">密码</param>
        /// <param name="user_name">用户名称</param>
        /// <returns></returns>
        Task<decimal> ChangePasswordAsync(decimal id,string password, string user_name);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<Sys_Manager> GetManagerContainRoleNameByIdAsync(decimal id);

		Task<decimal> GetSEQIDAsync();

		Task<Boolean> IsExistsNameAsync(string Name);

		Task<Boolean> IsExistsNameAsync(string Name, decimal Id);

        /// <summary>
        /// 保存数据
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        Task<decimal> SaveOrganizeData(ManagerAddOrModifyModel model);

        /// <summary>
        /// 获取导出数据
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        Task<TableDataModel> GetExportData(ManagerRequestModel model);

        /// <summary>
		/// 保存数据
		/// </summary>
		/// <param name="model"></param>
		/// <returns></returns>
		Task<ImportResult> SaveDataByTrans(List<ImportExcelItem> model, List<ImportDtl> importDtlList);

    }
}