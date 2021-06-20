/**
*┌──────────────────────────────────────────────────────────────┐
*│　描    述：后台管理员接口实现                                                    
*│　作    者：Admin                                            
*│　版    本：1.0    模板代码自动生成                                                
*│　创建时间：2018-12-18 13:28:43                             
*└──────────────────────────────────────────────────────────────┘
*┌──────────────────────────────────────────────────────────────┐
*│　命名空间： JZ.IMS.Repository.Oracle                                  
*│　类    名： ManagerRepository                                      
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
using System.Threading.Tasks;
using System.Linq;
using JZ.IMS.ViewModels;
using System.Collections.Generic;
using JZ.IMS.Core.Extensions;
using System.Text;
using JZ.IMS.Core.Models;
using JZ.IMS.Core.Helper;

namespace JZ.IMS.Repository.Oracle
{
    public class ManagerRepository : BaseRepository<Sys_Manager, decimal>, IManagerRepository
    {
        public ManagerRepository(IOptionsSnapshot<DbOption> options)
        {
            _dbOption = options.Get("iWMS");
            if (_dbOption == null)
            {
                throw new ArgumentNullException(nameof(DbOption));
            }
            _dbConnection = ConnectionFactory.CreateConnection(_dbOption.DbType, _dbOption.ConnectionString);
        }

        public decimal ChangeLockStatusById(decimal id, bool status)
        {
            string sql = "update SYS_Manager set ENABLED=:ENABLED where Id=:Id";
            return _dbConnection.Execute(sql, new
            {
                IsLock = status ? 'N' : 'Y',
                Id = id,
            });
        }

        public async Task<decimal> ChangeLockStatusByIdAsync(decimal id, bool status)
        {
            string sql = "update SYS_Manager set ENABLED=:ENABLED where  Id=:Id";
            return await _dbConnection.ExecuteAsync(sql, new
            {
                ENABLED = status ? 'Y' : 'N',
                Id = id,
            });
        }

        public decimal DeleteLogical(decimal ids)
        {
            string sql = "update SYS_Manager set Is_Delete='Y' where Id =:Ids";
            return _dbConnection.Execute(sql, new
            {
                Ids = ids
            });
        }

        public async Task<decimal> DeleteLogicalAsync(decimal ids)
        {
            string sql = "update SYS_Manager set Is_Delete='Y' where Id =:Ids";
            return await _dbConnection.ExecuteAsync(sql, new
            {
                Ids = ids
            });
        }

        /// <summary>
        /// 真删除
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        public async Task<decimal> DeleteLogicalAsyncEx(decimal ids)
        {
            string sql = " DELETE FROM SYS_MANAGER  WHERE ID=:IDS ";
            return await _dbConnection.ExecuteAsync(sql, new
            {
                IDS = ids
            });
        }

        public async Task<Boolean> IsExistsNameAsync(string Name)
        {
            string sql = "select Id from SYS_Manager where Is_Delete='N' and USER_NAME=:Name";
            var result = await _dbConnection.QueryAsync<decimal>(sql, new
            {
                Name,
            });
            if (result != null && result.Count() > 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public async Task<Boolean> IsExistsNameAsync(string Name, decimal Id)
        {
            string sql = "select Id from SYS_Manager where Is_Delete='N' and USER_NAME=:Name and Id <> :Id";
            var result = await _dbConnection.QueryAsync<decimal>(sql, new
            {
                Name,
                Id,
            });
            if (result != null && result.Count() > 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public bool GetLockStatusById(decimal id)
        {
            string sql = "select ENABLED from SYS_Manager where Id=@Id and Is_Delete='N'";
            var result = _dbConnection.QueryFirstOrDefault<string>(sql, new
            {
                Id = id,
            });

            if (result == "Y")
            {
                return false;
            }
            else
            {
                return true;
            }
        }

        public async Task<bool> GetLockStatusByIdAsync(decimal id)
        {
            string sql = "select ENABLED from SYS_Manager where Id=:Id and Is_Delete='N'";
            var result = await _dbConnection.QueryFirstOrDefaultAsync<string>(sql, new
            {
                Id = id,
            });

            if (result == "Y")
            {
                return false;
            }
            else
            {
                return true;
            }
        }

        public async Task<string> GetPasswordByIdAsync(decimal Id)
        {
            //用户登陆现在用的是SYS_USERS 
            //SELECT COUNT(1) FROM SYS_USERS WHERE EMPNO = :USER_NAME AND UPPER(USER_PASSWORD) = UPPER(:USER_PASSWORD) 

            string sql = "select Password from SYS_Manager where Id=:Id and Is_Delete='N'";
            return await _dbConnection.QueryFirstOrDefaultAsync<string>(sql, new
            {
                Id = Id,
            });
        }

        /// <summary>
        /// 修改密码
        /// </summary>
        /// <param name="Id"></param>
        /// <param name="Password"></param>
        /// <param name="user_name">用户名称</param>
        /// <returns></returns>
        public async Task<decimal> ChangePasswordAsync(decimal id, string password, string user_name)
        {
            int result = 1;
            ConnectionFactory.OpenConnection(_dbConnection);
            using (var tran = _dbConnection.BeginTransaction())
            {
                try
                {
                    //更新SYS_Manager
                    string sql = "update SYS_Manager set Password=:Password where Id = :Id";
                    await _dbConnection.ExecuteAsync(sql, new
                    {
                        Password = password,
                        Id = id
                    }, tran);

                    //更新SYS_USERS
                    sql = "update SYS_USERS set USER_PASSWORD=:Password WHERE EMPNO =:USER_NAME";
                    await _dbConnection.ExecuteAsync(sql, new
                    {
                        Password = password,
                        USER_NAME = user_name
                    }, tran);

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
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<Sys_Manager> GetManagerContainRoleNameByIdAsync(decimal id)
        {
            string sql = @"SELECT   mr.Role_Name, m.Id, m.Role_Id, m.User_Name, m.Password, m.Avatar, m.Nick_Name, m.Mobile, m.WORK_WECHAT_ID, m.Email, 
				  m.Login_Count, m.Login_Last_Ip, m.Login_Last_Time, m.Add_Manager_Id, m.Add_Time, m.Modify_Manager_Id, m.Modify_Time, 
				  (CASE m.ENABLED WHEN 'Y' THEN 'true' ELSE 'false' END) as ENABLED, m.Is_Delete, m.Remark
			   FROM SYS_Manager m INNER JOIN SYS_Manager_Role mr ON m.Role_Id = mr.Id where m.Id=:Id and m.Is_Delete='N' ";
            return await _dbConnection.QueryFirstOrDefaultAsync<Sys_Manager>(sql, new
            {
                Id = id
            });
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public async Task<decimal> GetSEQIDAsync()
        {
            string sql = "SELECT Sys_Manager_SEQ.NEXTVAL MY_SEQ FROM DUAL";
            var result = await _dbConnection.ExecuteScalarAsync(sql);
            return (decimal)result;
        }

        /// <summary>
        /// 保存数据
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public async Task<decimal> SaveOrganizeData(ManagerAddOrModifyModel model)
        {
            int result = 1;
            List<string> organizeList = null;
            if (!model.ORGANIZE_ID_STRING.IsNullOrWhiteSpace())
            {
                organizeList = model.ORGANIZE_ID_STRING.Split(",").ToList();
            }
            if (organizeList == null || organizeList.Count == 0)
            {
                return 0;
            }

            ConnectionFactory.OpenConnection(_dbConnection);
            using (var tran = _dbConnection.BeginTransaction())
            {
                try
                {
                    //删除
                    string deleteSql = @"Delete from sys_user_organize where manager_id=:manager_id ";
                    await _dbConnection.ExecuteAsync(deleteSql, new
                    {
                        manager_id = model.ID,
                    }, tran);
                    //新增
                    string insertSql = @"insert into sys_user_organize 
					(id,manager_id,organize_id,start_date,creator,end_date,status) 
					values (sys_organize_seq.nextval,:manager_id,:organize_id,sysdate,:creator,null,'Y')";
                    if (organizeList != null && organizeList.Count > 0)
                    {
                        foreach (var item in organizeList)
                        {
                            var resdata = await _dbConnection.ExecuteAsync(insertSql, new
                            {
                                manager_id = model.ID,
                                organize_id = item,
                                creator = string.Empty,
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
        /// 获取导出数据
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public async Task<TableDataModel> GetExportData(ManagerRequestModel model)
        {
            string conditions = " where m.Is_Delete='N' ";
            if (!model.Key.IsNullOrWhiteSpace())
            {
                conditions += $"and (instr(m.User_Name, :Key) > 0 or instr(m.Nick_Name, :Key) > 0 or instr(m.Mobile, :Key) > 0 )";
            }

            string sql = @"select ROWNUM as rowno, mr.Role_Name as ROLE_ID,m.USER_NAME,m.NICK_NAME,m.MOBILE,M.WORK_WECHAT_ID,mm.NICK_NAME as ADD_MANAGER_ID,
                                            m.ADD_TIME,m.ENABLED,m.IS_DELETE,m.REMARK,orga.ORGANIZE_NAME as ORGANIZE_ID
                                            from Sys_Manager m
                                            left join Sys_Manager_Role mr on m.Role_ID=mr.ID and mr.IS_DELETE='N'
                                            left join Sys_Manager mm on m.ADD_MANAGER_ID=mm.ID
                                            inner join ( select MANAGER_ID,  wmsys.wm_concat(ORGANIZE_NAME) as ORGANIZE_NAME from (
                                                            select distinct uo.MANAGER_ID, org.ORGANIZE_NAME from SYS_USER_ORGANIZE uo 
                                                                left join SYS_ORGANIZE org on uo.ORGANIZE_ID=org.ID   )
                                                                group by MANAGER_ID ) orga on orga.MANAGER_ID=m.ID ";
            string pagedSql = SQLBuilderClass.GetPagedSQL(sql, " m.ID desc", conditions);
            var resdata = await _dbConnection.QueryAsync<object>(pagedSql, model);
            string sqlcnt = @"select count(0)
                                            from Sys_Manager m
                                            left join Sys_Manager_Role mr on m.Role_ID=mr.ID and mr.IS_DELETE='N'
                                            left join Sys_Manager mm on m.ADD_MANAGER_ID=mm.ID
                                            inner join ( select MANAGER_ID,  wmsys.wm_concat(ORGANIZE_NAME) as ORGANIZE_NAME from (
                                                            select distinct uo.MANAGER_ID, org.ORGANIZE_NAME from SYS_USER_ORGANIZE uo 
                                                                left join SYS_ORGANIZE org on uo.ORGANIZE_ID=org.ID   )
                                                                group by MANAGER_ID ) orga on orga.MANAGER_ID=m.ID " + conditions;
            int cnt = await _dbConnection.ExecuteScalarAsync<int>(sqlcnt, model);
            return new TableDataModel
            {
                count = cnt,
                data = resdata?.ToList(),
            };


        }


        /// <summary>
        /// 获取表的序列
        /// </summary>
        /// <returns></returns>
        public async Task<decimal> GetUserSEQID()
        {
            string sql = "SELECT SYS_MANAGER_SEQ.NEXTVAL MY_SEQ FROM DUAL";
            var result = await _dbConnection.ExecuteScalarAsync(sql);
            return (decimal)result;
        }

        /// <summary>
        /// 获取角色表ID
        /// </summary>
        /// <returns></returns>
        public async Task<decimal> GetRoleIDByName(string name)
        {
            string sql = string.Format("select ID from Sys_Manager_Role where IS_DELETE='N' and ROLE_NAME='{0}'",name);
            var result = await _dbConnection.ExecuteScalarAsync(sql);
            return (decimal)result;
        }

        /// <summary>
        /// 根据选择的用户名获取ID
        /// </summary>
        /// <returns></returns>
        public async Task<decimal> GetUserIDByName(string name)
        {
            string sql = string.Format("select ID from Sys_Manager where ENABLED='Y' and NICK_NAME ='{0}'", name);
            var result = await _dbConnection.ExecuteScalarAsync(sql);
            return (decimal)result;
        }

        /// <summary>
        /// 根据选择的组织架构名获取ID
        /// </summary>
        /// <returns></returns>
        public async Task<decimal> GetOrgIDByName(string name)
        {
            string sql = string.Format("SELECT ID FROM SYS_ORGANIZE WHERE ORGANIZE_NAME='{0}'", name);
            var result = await _dbConnection.ExecuteScalarAsync(sql);
            return (decimal)result;
        }

         

        /// <summary>
		/// 保存数据
		/// </summary>
		/// <param name="model"></param>
		/// <returns></returns>
		public async Task<ImportResult> SaveDataByTrans(List<ImportExcelItem> model, List<ImportDtl> tplList)
        {
            ImportResult result = new ImportResult();
            result.Result = 1;
            ConnectionFactory.OpenConnection(_dbConnection);
            using (var tran = _dbConnection.BeginTransaction())
            {
                try
                {
                    //新增
                    //string insertSql = @"insert into SYS_MANAGER 
                    //(ID,ROLE_ID,USER_NAME,NICK_NAME,MOBILE,WORK_WECHAT_ID,ADD_MANAGER_ID,ADD_TIME,ENABLED,REMARK,ORGANIZE_ID) 
                    //VALUES (:ID,:ROLE_ID,:USER_NAME,:NICK_NAME,:MOBILE,:WORK_WECHAT_ID,:ADD_MANAGER_ID,:ADD_TIME,:ENABLED,:REMARK,:ORGANIZE_ID)";

                    string insertSql = @"insert into {TableName} 
                    (ID,PASSWORD,{ColumnNameList}) 
					VALUES (:ID,:PASSWORD,{ParaList})";
                    var columnList = new StringBuilder();
                    var paraList = new StringBuilder();
                    string password = MD5Helper.ToMD5("MES123").ToUpper();

                    foreach (var column in tplList)
                    {
                        columnList.Append(column.COLUMN_NAME + ",");
                        paraList.Append(":" + column.COLUMN_NAME + ",");
                    }
                    insertSql = insertSql.Replace("{TableName}", "SYS_MANAGER")
                             .Replace("{ColumnNameList}", columnList.ToString().Substring(0, columnList.ToString().Length - 1))
                             .Replace("{ParaList}", paraList.ToString().Substring(0, paraList.ToString().Length - 1));

                    var template = await GetTemplateInfo("SYS_MANAGER");
                    string old_param = string.Empty; string new_param = string.Empty;
                    foreach (var tpItem in template)
                    {
                        if (tpItem.COLUMN_TYPE.ToUpper() == "DATE")
                        {
                            old_param = ":" + tpItem.COLUMN_NAME;
                            new_param = $"to_date({old_param}, 'yyyy-mm-dd HH24:MI:SS')";
                            insertSql = insertSql.Replace(old_param, new_param);
                        }
                    }
                    DynamicParameters pars = new DynamicParameters();
                    if (model != null && model.Count > 0)
                    {
                        string param = string.Empty, excel_val = string.Empty, value = string.Empty,nick_name=string.Empty,user_name=string.Empty;
                        int j = 1;
                        var orgid = 0;
                        if (tplList.Where(x => x.COLUMN_NAME == "PASSWORD").Count() > 0)
                        {
                            insertSql = insertSql.Replace("PASSWORD,", "").Replace(":PASSWORD,", "");
                        }
                        else
                        {
                            pars.Add("PASSWORD", password);
                        }
                        foreach (var item in model)
                        {
                            var newid = await GetUserSEQID();
                            pars.Add("ID", newid);
                            for (int i = 0; i < tplList.Count; i++)
                            {
                                param = tplList[i].COLUMN_NAME;
                                excel_val = item.GetType().GetProperty($"Column{i + 1}").GetValue(item)?.ToString() ?? string.Empty;
                                value = await GetValueByKey(tplList[i].REFERENCE_SQL, excel_val.Trim());
                                if (value.IsNullOrWhiteSpace() && !excel_val.IsNullOrWhiteSpace())
                                {
                                    result.Result = -1;
                                    result.ErrInfo = new ImportErrInfo
                                    {
                                        Index = j + 1,
                                        COLUMN_CAPTION = tplList[i].COLUMN_CAPTION,
                                    };
                                    tran.Rollback();
                                    return result;
                                }
                               
                                if (param == "PASSWORD")
                                {
                                    value= MD5Helper.ToMD5(value).ToUpper();
                                }
                                if (param == "ORGANIZE_ID")
                                {
                                    orgid = Convert.ToInt32(value);
                                }
                                if (param=="NICK_NAME")
                                {
                                    nick_name = value;
                                }
                                if (param == "USER_NAME")
                                {
                                    user_name = value;
                                }
                                pars.Add(param, value.ToUpper());
                            }

                            var resdata = await _dbConnection.ExecuteAsync(insertSql, pars, tran);

                            #region

                            //var newid = await GetUserSEQID();
                            //var roleid = await GetRoleIDByName(item.Column2);
                            //var userid = await GetUserIDByName(item.Column6);
                            //var orgid = await GetOrgIDByName(item.Column8);
                            //var resdata = await _dbConnection.ExecuteAsync(insertSql, new
                            //{
                            //    ID = newid,
                            //    ROLE_ID=roleid,
                            //    USER_NAME=item.Column1,
                            //    NICK_NAME=item.Column3,
                            //    MOBILE=item.Column5,
                            //    WORK_WECHAT_ID=item.Column4,
                            //    ADD_MANAGER_ID= userid,
                            //    ADD_TIME=Convert.ToDateTime(item.Column7),
                            //    ENABLED=item.Column9,
                            //    REMARK=item.Column10,
                            //    ORGANIZE_ID= orgid,
                            //}, tran);
                            #endregion

                            string insertOrgSql = @"insert into sys_user_organize 
					(ID,MANAGER_ID,ORGANIZE_ID,START_DATE,CREATOR,END_DATE,STATUS) 
					values (sys_organize_seq.nextval,:MANAGER_ID,:ORGANIZE_ID,sysdate,:CREATOR,null,'Y')";
                            var resorgdata = await _dbConnection.ExecuteAsync(insertOrgSql, new
                            {
                                MANAGER_ID = newid,
                                ORGANIZE_ID = orgid,
                                CREATOR = string.Empty,
                            }, tran);

                            if (ExistsUserName(nick_name)==false)
                            {
                                AddUser(user_name,nick_name,password);
                            }
                        }
                    }

                    #region


                    //              //更新
                    //              string updateSql = @"Update SYS_MANAGER set ROLE_ID=:ROLE_ID,USER_NAME=:USER_NAME,FULL_NAME=:FULL_NAME,PASSWORD_SALT=:PASSWORD_SALT,PASSWORD=:PASSWORD,AVATAR=:AVATAR,NICK_NAME=:NICK_NAME,MOBILE=:MOBILE,EMAIL=:EMAIL,WORK_WECHAT_ID=:WORK_WECHAT_ID,LOGIN_COUNT=:LOGIN_COUNT,LOGIN_LAST_IP=:LOGIN_LAST_IP,LOGIN_LAST_TIME=:LOGIN_LAST_TIME,ADD_MANAGER_ID=:ADD_MANAGER_ID,ADD_TIME=:ADD_TIME,MODIFY_MANAGER_ID=:MODIFY_MANAGER_ID,MODIFY_TIME=:MODIFY_TIME,ENABLED=:ENABLED,IS_DELETE=:IS_DELETE,REMARK=:REMARK,ORGANIZE_ID=:ORGANIZE_ID  
                    //where ID=:ID ";
                    //              if (model.UpdateRecords != null && model.UpdateRecords.Count > 0)
                    //              {
                    //                  foreach (var item in model.UpdateRecords)
                    //                  {
                    //                      var resdata = await _dbConnection.ExecuteAsync(updateSql, new
                    //                      {
                    //                          item.ID,
                    //                          item.ROLE_ID,
                    //                          item.USER_NAME,
                    //                          item.FULL_NAME,
                    //                          item.PASSWORD_SALT,
                    //                          item.PASSWORD,
                    //                          item.AVATAR,
                    //                          item.NICK_NAME,
                    //                          item.MOBILE,
                    //                          item.EMAIL,
                    //                          item.WORK_WECHAT_ID,
                    //                          item.LOGIN_COUNT,
                    //                          item.LOGIN_LAST_IP,
                    //                          item.LOGIN_LAST_TIME,
                    //                          item.ADD_MANAGER_ID,
                    //                          item.ADD_TIME,
                    //                          item.MODIFY_MANAGER_ID,
                    //                          item.MODIFY_TIME,
                    //                          item.ENABLED,
                    //                          item.IS_DELETE,
                    //                          item.REMARK,
                    //                          item.ORGANIZE_ID,

                    //                      }, tran);
                    //                  }
                    //              }
                    //              //删除
                    //              string deleteSql = @"Delete from SYS_MANAGER where ID=:ID ";
                    //              if (model.RemoveRecords != null && model.RemoveRecords.Count > 0)
                    //              {
                    //                  foreach (var item in model.RemoveRecords)
                    //                  {
                    //                      var resdata = await _dbConnection.ExecuteAsync(deleteSql, new
                    //                      {
                    //                          item.ID
                    //                      }, tran);
                    //                  }
                    //              }

                    #endregion

                    tran.Commit();
                }
                catch (Exception ex)
                {
                    result.Result = -1;
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
        /// 获取表对应的导入字段
        /// </summary>
        /// <param name="tableName">表名</param>
        private async Task<List<ImportItemVM>> GetTemplateInfo(string tableName)
        {
            List<ImportItemVM> result = new List<ImportItemVM>();
            List<string> excludeItem = new List<string> { "ID", "VERSION", "ENABLE_BILL_ID", "DISABLE_BILL_ID",
                "ATTRIBUTE4", "ATTRIBUTE5"};
            string[] excelItem = new string[] { "A", "B", "C", "D", "E", "F", "G", "H", "I", "J", "K", "L", "M", "N", "O", "P", "Q", "R", "S", "T", "U",
                "V", "W", "X", "Y", "Z","AA", "AB", "AC", "AD", "AE", "AF", "AG", "AH", "AI", "AJ", "AK", "AL", "AM", "AN", "AO", "AP", "AQ", "AR", "AS",
                "AT", "AU","AV", "AW", "AX", "AY", "AZ", "BA", "BB", "BC", "BD", "BE", "BF", "BG", "BH" };
            DatabaseType dbType = ConnectionFactory.GetDataBaseType(_dbOption.DbType);
            List<DbTable> tables = new List<DbTable>();
            using (var dbConnection = ConnectionFactory.CreateConnection(dbType, _dbOption.ConnectionString))
            {
                tables = dbConnection.GetCurrentDatabaseTableList(dbType, tableName.ToUpper(), _dbOption.DbUser);
            }

            if (tables != null && tables.Any())
            {
                var curtable = tables.FirstOrDefault();
                int idx = 1;
                foreach (var column in curtable.Columns)
                {
                    //排除的字段 
                    if (excludeItem.IndexOf(column.ColName.ToUpper()) != -1)
                        continue;

                    result.Add(new ImportItemVM
                    {
                        COLUMN_NAME = column.ColName,
                        COLUMN_CAPTION = column.Comments ?? string.Empty,
                        IS_UNIQUE = (column.IsPrimaryKey) ? 1 : 0,
                        ISNULL_ABLE = (column.IsPrimaryKey) ? 0 : ((column.IsNullable) ? 1 : 0),
                        EXCEL_ITEM = (idx <= 60) ? excelItem[idx - 1] : string.Empty,
                        COLUMN_TYPE = column.ColumnType,
                    });
                    idx++;
                }
            }

            return result;
        }


        /// <summary>
		/// 检测用户名是否存在
		/// </summary>
		/// <param name="userName"></param>
		/// <returns></returns>
		private bool ExistsUserName(string userName)
        {
            string sql = "SELECT COUNT(1) FROM SYS_USERS WHERE EMPNO = :USER_NAME";
            object result = _dbConnection.ExecuteScalar(sql, new
            {
                USER_NAME = userName
            });

            if (Convert.ToInt32(result) > 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        /// <summary>
        /// 添加sys_user表数据
        /// </summary>
        /// <param name="userName"></param>
        /// <param name="empno"></param>
        /// <param name="password"></param>
        /// <returns></returns>
        private  bool AddUser(String userName, String empno, string password)
        {
            string sql = @"INSERT INTO SYS_USERS(ID, VERSION, USER_NAME, EMPNO, AD_NAME, USER_TYPE, USER_PASSWORD, STATUS)
                VAlUES(SYS_GUID(), 1, :USER_NAME, :EMPNO, :AD_NAME, '内部',:USER_PASSWORD, 'Y')";
            object result = _dbConnection.ExecuteScalar(sql, new
            {
                USER_NAME = userName,
                EMPNO = empno,
                AD_NAME = empno,
                USER_PASSWORD = password
            });
            return true;
        }


        /// <summary>
        /// 获取SQL对应的值
        /// </summary>
        /// <param name="sql"></param>
        /// <param name="key"></param>
        /// <returns></returns>
        public async Task<string> GetValueByKey(string sql, string key)
        {
            string result = string.Empty;
            if (sql != null && sql.Trim().Length > 0)
            {
                var obj = await _dbConnection.ExecuteScalarAsync<string>(sql, new { key });
                result = obj?.ToString() ?? string.Empty;
            }
            else
            {
                result = key;
            }
            return result;
        }


    }
}