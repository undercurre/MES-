/*
*┌──────────────────────────────────────────────────────────────┐
*│　描    述：平板菜单权限表接口实现                                                    
*│　作    者：嘉志科技                                            
*│　版    本：2.0    模板代码自动生成                                                
*│　创建时间：2020-10-30 09:43:45                             
*└──────────────────────────────────────────────────────────────┘
*┌──────────────────────────────────────────────────────────────┐
*│　命名空间： JZ.IMS.Repository.Oracle                                  
*│　类    名： MesPadClientMenusRepository                                      
*└──────────────────────────────────────────────────────────────┘
*/
using JZ.IMS.Core.DbHelper;
using JZ.IMS.Core.Options;
using JZ.IMS.Core.Repository;
using JZ.IMS.IRepository;
using JZ.IMS.Models;
using JZ.IMS.ViewModels;
using Dapper;
using Microsoft.Extensions.Options;
using System;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Linq;

namespace JZ.IMS.Repository.Oracle
{
    public class MesPadClientMenusRepository:BaseRepository<MesPadClientMenus,Decimal>, IMesPadClientMenusRepository
    {
        public MesPadClientMenusRepository(IOptionsSnapshot<DbOption> options)
        {
            _dbOption =options.Get("iWMS");
            if (_dbOption == null)
            {
                throw new ArgumentNullException(nameof(DbOption));
            }
            _dbConnection = ConnectionFactory.CreateConnection(_dbOption.DbType, _dbOption.ConnectionString);
        }

        /// <summary>
        /// 根据主键获取激活状态
        /// </summary>
        /// <param name="id">主键</param>
        /// <returns></returns>
		public async Task<Boolean> GetEnableStatus(decimal id)
		{
			string sql = "SELECT ENABLED FROM MES_PAD_CLIENT_MENUS WHERE ID=:ID";
			var result = await _dbConnection.QueryFirstOrDefaultAsync<string>(sql, new
			{
				ID = id,
			});

			return result == "Y" ? true : false;
		}

        /// <summary>
        /// 修改激活状态
        /// </summary>
        /// <param name="id">主键</param>
        /// <param name="status">更改后的状态</param>
        /// <returns></returns>
		public async Task<decimal> ChangeEnableStatus(decimal id, bool status)
		{
			string sql = "UPDATE MES_PAD_CLIENT_MENUS set ENABLED=:ENABLED WHERE ID=:Id";
			return await _dbConnection.ExecuteAsync(sql, new
			{
				ENABLED = status ? 'Y' : 'N',
				Id = id,
			});
		}

        /// <summary>
        /// 获取表的序列
        /// </summary>
        /// <returns></returns>
		public async Task<decimal> GetSEQID()
		{
			string sql = "SELECT MES_PAD_CLIENT_MENUS_SEQ.NEXTVAL MY_SEQ FROM DUAL";
			var result = await _dbConnection.ExecuteScalarAsync(sql);
			return (decimal)result;
		}

        /// <summary>
		///项目是否已被使用 
		/// </summary>
		/// <param name="id">项目id</param>
		/// <returns></returns>
		public async Task<bool> ItemIsByUsed(decimal id)
		{
			string sql = "select count(0) from MES_PAD_CLIENT_MENUS where id = :id";
			object result = await _dbConnection.ExecuteScalarAsync(sql, new
			{
				id
			});

			return (Convert.ToInt32(result) > 0);
		}

		/// <summary>
		/// 保存数据
		/// </summary>
		/// <param name="model"></param>
		/// <returns></returns>
		public async Task<decimal> SaveDataByTrans(MesPadClientMenusModel model)
		{
			int result = 1;
			ConnectionFactory.OpenConnection(_dbConnection);
			using (var tran = _dbConnection.BeginTransaction())
			{
				try
				{
					//新增
					string insertSql = @"INSERT INTO MES_PAD_CLIENT_MENUS 
					(ID,MENU_TYPE,P_ID,P_ID_ARR,PAGE_URL,LINK_URL,ICON_URL,MENU_CODE,MENU_NAME,MENU_EN,ENABLED,SORT,CREATE_USER,CREATE_TIME) 
					VALUES (:ID,:MENU_TYPE,:P_ID,:P_ID_ARR,:PAGE_URL,:LINK_URL,:ICON_URL,:MENU_CODE,:MENU_NAME,:MENU_EN,'Y',:SORT,:CREATE_USER,SYSDATE)";
					if (model.InsertRecords != null && model.InsertRecords.Count > 0)
					{
						foreach (var item in model.InsertRecords)
						{
							var newid = await GetSEQID();
							var resdata = await _dbConnection.ExecuteAsync(insertSql, new
							{
								ID = newid,
								item.MENU_TYPE,
								item.P_ID,
								item.P_ID_ARR,
								item.PAGE_URL,
								item.LINK_URL,
								item.ICON_URL,
								item.MENU_CODE,
								item.MENU_NAME,
								item.MENU_EN,
								item.SORT,
								item.CREATE_USER							}, tran);
						}
					}
					//更新
					string updateSql = @"UPDATE MES_PAD_CLIENT_MENUS SET MENU_TYPE=:MENU_TYPE,P_ID=:P_ID,P_ID_ARR=:P_ID_ARR,PAGE_URL=:PAGE_URL,LINK_URL=:LINK_URL,ICON_URL=:ICON_URL,MENU_CODE=:MENU_CODE,MENU_NAME=:MENU_NAME,MENU_EN=:MENU_EN,ENABLED=:ENABLED,SORT=:SORT,UPDATE_USER=:UPDATE_USER,UPDATE_TIME=SYSDATE  
						WHERE ID=:ID ";
					if (model.UpdateRecords != null && model.UpdateRecords.Count > 0)
					{
						foreach (var item in model.UpdateRecords)
						{
							var resdata = await _dbConnection.ExecuteAsync(updateSql, new
							{
								item.ID,
								item.MENU_TYPE,
								item.P_ID,
								item.P_ID_ARR,
								item.PAGE_URL,
								item.LINK_URL,
								item.ICON_URL,
								item.MENU_CODE,
								item.MENU_NAME,
								item.MENU_EN,
								item.ENABLED,
								item.SORT,
								item.UPDATE_USER							}, tran);
						}
					}
					//删除
					string deleteSql = @"DELETE FROM MES_PAD_CLIENT_MENUS WHERE ID=:ID ";
					if (model.RemoveRecords != null && model.RemoveRecords.Count > 0)
					{
						foreach (var item in model.RemoveRecords)
						{
							var resdata = await _dbConnection.ExecuteAsync(deleteSql, new
							{
								item.ID
							}, tran);
						}
					}

					tran.Commit();
				}
				catch (Exception ex)
				{
					result = -1;
					tran.Rollback();
					throw ex;
				}
				finally
				{
					if (_dbConnection.State != System.Data.ConnectionState.Closed)
					{
						_dbConnection.Close();
					}
				}
			}
			return result;
		}

        /// <summary>
        /// 保存数据
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public async Task<int> SavePadRoleData(SavePadRoleDataRequestModel model)
        {
            int result = 0;
            ConnectionFactory.OpenConnection(_dbConnection);
            using (var tran = _dbConnection.BeginTransaction())
            {
                try
                {
                    string deleteSql = @"DELETE FROM SYS_PAD_MENUS_ROLES WHERE ROLE_ID=:ROLE_ID ";
                    result = await _dbConnection.ExecuteAsync(deleteSql, new { ROLE_ID = model.ROLE_ID }, tran);
                    foreach (int pad_id in model.PAD_ID)
                    {
                        if (pad_id > 0)
                        {
                            string insertSql = @"INSERT INTO SYS_PAD_MENUS_ROLES (ID, PAD_ID,ROLE_ID,CREATE_TIME) VALUES (SYS_PAD_MENUS_ROLES_SEQ.NEXTVAL,:PAD_ID,:ROLE_ID,SYSDATE)";
                            result = await _dbConnection.ExecuteAsync(insertSql, new
                            {
                                PAD_ID = pad_id,
                                ROLE_ID = model.ROLE_ID
                            }, tran);
                            if (result <= 0) { throw new Exception("新增失败!"); }
                        }
                    }

                    tran.Commit();
                }
                catch (Exception ex)
                {
                    result = -1;
                    tran.Rollback();
                    throw ex;
                }
                finally
                {
                    if (_dbConnection.State != System.Data.ConnectionState.Closed)
                    {
                        _dbConnection.Close();
                    }
                }
            }
            return result;
        }

    }
}