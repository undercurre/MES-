/**
*┌──────────────────────────────────────────────────────────────┐
*│　描    述：角色权限表接口实现                                                    
*│　作    者：Admin                                            
*│　版    本：1.0    模板代码自动生成                                                
*│　创建时间：2019-01-05 17:54:04                             
*└──────────────────────────────────────────────────────────────┘
*┌──────────────────────────────────────────────────────────────┐
*│　命名空间： JZ.IMS.Repository.Oracle                                  
*│　类    名： RolePermissionRepository                                      
*└──────────────────────────────────────────────────────────────┘
*/
using JZ.IMS.Core.DbHelper;
using JZ.IMS.Core.Options;
using JZ.IMS.Core.Repository;
using JZ.IMS.IRepository;
using JZ.IMS.Models;
using Dapper;
using Microsoft.Extensions.Options;
using System;
using System.Linq;
using System.Threading.Tasks;
using System.Collections.Generic;
using JZ.IMS.Core.Utilities;
using JZ.IMS.ViewModels;

namespace JZ.IMS.Repository.Oracle
{
    public class RolePermissionRepository : BaseRepository<Sys_Role_Permission, decimal>, IRolePermissionRepository
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="options"></param>
        public RolePermissionRepository(IOptionsSnapshot<DbOption> options)
        {
            _dbOption = options.Get("iWMS");
            if (_dbOption == null)
            {
                throw new ArgumentNullException(nameof(DbOption));
            }
            _dbConnection = ConnectionFactory.CreateConnection(_dbOption.DbType, _dbOption.ConnectionString);
        }

        public decimal DeleteLogical(decimal[] ids)
        {
            string sql = "update SYS_Role_Permission set Is_Delete='Y' where Id in :Ids";
            return _dbConnection.Execute(sql, new
            {
                Ids = ids
            });
        }

        public async Task<decimal> DeleteLogicalAsync(decimal[] ids)
        {
            string sql = "update SYS_Role_Permission set Is_Delete='Y' where Id in :Ids";
            return await _dbConnection.ExecuteAsync(sql, new
            {
                Ids = ids
            });
        }

        /// <summary>
        /// 通过角色主键获取菜单主键数组
        /// </summary>
        /// <param name="RoleId"></param>
        /// <returns></returns>
        public decimal[] GetIdsByRoleId(decimal RoleId)
        {
            string sql = "select Menu_Id from SYS_Role_Permission where Role_Id=:RoleId";
            return _dbConnection.Query<decimal>(sql, new
            {
                RoleId = RoleId
            })?.ToArray();
        }

        /// <summary>
        /// 通过角色主键获取有权限的菜单列表
        /// </summary>
        /// <param name="RoleId"></param>
        /// <returns></returns>
        public List<Sys_Menu> GetMenuByRoleId(decimal RoleId)
        {
            string sql = @"select a.* from sys_menu a inner join SYS_Role_Permission b on a.id = b.menu_id 
						   where b.role_id =:RoleId and a.enabled = 'Y' and a.is_delete = 'N'";
            return _dbConnection.Query<Sys_Menu>(sql, new { RoleId = RoleId })?.ToList();
        }

        /// <summary>
        /// 角色信息查询
        /// </summary>
        /// <param name="RoleId"></param>
        /// <returns></returns>
        public async Task<List<dynamic>> GetRole()
        {
            List<dynamic> result = new List<dynamic>();
            try
            {
                string sql = "select SR.RoleID,SR.RoleName,SR.\"Desc\",T.COUNT from (select ID RoleID,ROLE_NAME RoleName,NVL(REMARK,'') \"Desc\" from  sys_manager_role where  Is_Delete='N') SR "
                              + " inner join(select SM.ROLE_ID, count(sm.Role_id) COUNT from sys_manager sm, sys_manager_role sr"
                              + " where sm.ROLE_ID= SR.ID and sm.IS_DELETE='N' and sm.ENABLED='Y' GROUP BY SM.ROLE_ID ) T on T.ROLE_ID = SR.RoleID"
                              + " ORDER BY SR.RoleName ASC";
                result = (await _dbConnection.QueryAsync<dynamic>(sql))?.ToList();
            }
            catch (Exception ex)
            {

                throw new Exception(ex.Message);
            }
            return result;
        }

        /// <summary>
        /// 角色信息查询
        /// </summary>
        /// <param name="RoleId"></param>
        /// <returns></returns>
        public async Task<TableDataModel> GetUserRoleInfo(string Account, string Name, string RoleName, string PageIndex, string PageSize)
        {
            List<dynamic> data = new List<dynamic>();
            int recoredCount = 0;
            TableDataModel tableData = new TableDataModel();
            try
            {
                string condition = " where 1=1 ";

                if (!Account.IsNullOrEmpty())
                {
                    condition += " And instr(SM.NICK_NAME,:Account)>0 ";
                }
                if (!Name.IsNullOrEmpty())
                {
                    condition += " And instr(SM.USER_NAME,:Name)>0 ";
                }
                if (!RoleName.IsNullOrEmpty())
                {
                    condition += " And instr(SMR.ROLE_NAME,:RoleName)>0 ";
                }
                string sql = " select TP.ID,TP.Account,TP.\"NAME\",TP.RoleName,TP.ORGANIZE_NAME,decode(TP.ENABLED,'Y','启用','禁用') StatusName,'MES' as AppName from (select rownum rid,t.* from "
                                + "(select SM.ID,SM.NICK_NAME Account,SM.USER_NAME \"NAME\",SMR.ROLE_NAME RoleName,SOE.ORGANIZE_NAME,SM.ENABLED from SYS_MANAGER SM "
                                + " left join SYS_MANAGER_ROLE SMR on SM.ROLE_ID = SMR.ID"
                                + " left join SYS_ORGANIZE SOE on SM.ORGANIZE_ID = SOE.ID"
                                + condition + " and SM.IS_DELETE='N' and SM.ENABLED='Y' ORDER BY SMR.ROLE_NAME ASC ) t "
                                + " where rownum <=:PageSize) TP "
                                + " where id >= (1 -:PageIndex)*:PageSize";
                data = (await _dbConnection.QueryAsync<dynamic>(sql, new
                {
                    Account = Account,
                    Name = Name,
                    RoleName = RoleName,
                    PageIndex = PageIndex,
                    PageSize = PageSize
                }))?.ToList();

                string countSql = @"select count(*) from SYS_MANAGER SM
                                    left join SYS_MANAGER_ROLE SMR on SM.ROLE_ID=SMR.ID
                                    left join SYS_ORGANIZE SOE on SM.ORGANIZE_ID=SOE.ID
                                   " + condition + " and SM.IS_DELETE='N' and SM.ENABLED='Y' ORDER BY SMR.ROLE_NAME ASC ";

                recoredCount = await _dbConnection.ExecuteScalarAsync<int>(countSql,new
                {
                    Account = Account,
                    Name = Name,
                    RoleName = RoleName
                });
            }
            catch (Exception ex)
            {
                tableData.code = -1;
                throw new Exception(ex.Message);
            }
            tableData.data = data;
            tableData.count = recoredCount;
            return tableData;
        }

        /// <summary>
        /// 用户查询
        /// </summary>
        /// <param name="RoleId"></param>
        /// <returns></returns>
        public async Task<TableDataModel> GetUser(string Account, string Name, string OrganizeID, string OrganizeName, string PageIndex, string PageSize)
        {
            List<dynamic> data = new List<dynamic>();
            int recoredCount = 0;
            TableDataModel tableData = new TableDataModel();
            try
            {
                string condition = " where 1=1  ";

                if (!Account.IsNullOrEmpty())
                {
                    condition += " And instr(SM.NICK_NAME,:Account) > 0 ";
                }
                if (!Name.IsNullOrEmpty())
                {
                    condition += " And instr(SM.USER_NAME,:Name)> 0 ";
                }
                if (!OrganizeName.IsNullOrEmpty())
                {
                    condition += " And instr(SOE.ORGANIZE_NAME,:OrganizeName)> 0 ";
                }
                if (!OrganizeID.IsNullOrEmpty())
                {
                    condition += " And SOE.ID=:OrganizeID ";
                }
                string sql = "select TP.ID,TP.Account,TP.\"NAME\",TP.RoleName,TP.ORGANIZE_NAME,decode(TP.ENABLED,'Y','启用','禁用') StatusName,'MES' as AppName from (select rownum rid,t.* from "
                              + "(select SM.ID,SM.NICK_NAME Account,SM.USER_NAME \"NAME\",SMR.ROLE_NAME RoleName,SOE.ORGANIZE_NAME,SM.ENABLED from SYS_MANAGER SM"
                              + " left join SYS_MANAGER_ROLE SMR on SM.ROLE_ID = SMR.ID"
                              + " left join SYS_ORGANIZE SOE on SM.ORGANIZE_ID = SOE.ID " + condition + " and SM.IS_DELETE='N' and SM.ENABLED='Y' ORDER BY SMR.ROLE_NAME ASC) t "
                              + " where rownum <=:PageSize) TP "
                              + " where rid >= (1 -:PageIndex)*:PageSize";
                data = (await _dbConnection.QueryAsync<dynamic>(sql, new
                {
                    Account = Account,
                    Name = Name,
                    OrganizeName = OrganizeName,
                    OrganizeID = OrganizeID,
                    PageIndex = PageIndex,
                    PageSize = PageSize
                }))?.ToList();

                string countSql = @"select count(*) from SYS_MANAGER SM
                                    left join SYS_MANAGER_ROLE SMR on SM.ROLE_ID=SMR.ID
                                    left join SYS_ORGANIZE SOE on SM.ORGANIZE_ID=SOE.ID
                                   " + condition + " and SM.IS_DELETE='N' and SM.ENABLED='Y' ORDER BY SMR.ROLE_NAME ASC ";

                recoredCount = await _dbConnection.ExecuteScalarAsync<int>(countSql, new
                {
                    Account = Account,
                    Name = Name,
                    OrganizeName = OrganizeName,
                    OrganizeID = OrganizeID
                });

            }
            catch (Exception ex)
            {
                tableData.code = -1;
                throw new Exception(ex.Message);
            }
            tableData.data = data;
            tableData.count = recoredCount;
            return tableData;
        }

        /// <summary>
        /// 用户角色配置
        /// </summary>
        /// <param name="RoleId"></param>
        /// <returns></returns>
        public async Task<bool> SetUserRole(string UserID, string RoleID)
        {
            bool result = false;
            int data = -1;
            try
            {

                string sql = " UPDATE SYS_MANAGER set ROLE_ID=:ROLE_ID where id=:ID and IS_DELETE='N' and ENABLED='Y' ";
                data = await _dbConnection.ExecuteAsync(sql, new
                {
                    ROLE_ID = RoleID,
                    ID = UserID
                });
            }
            catch (Exception ex)
            {
                
                throw new Exception(ex.Message);
            }
            result = data > 0?true:false;
            return result;
        }

    }
}