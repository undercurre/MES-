using JZ.IMS.Core.DbHelper;
using JZ.IMS.Core.Options;
using JZ.IMS.Core.Repository;
using JZ.IMS.IRepository;
using JZ.IMS.Models;
using Dapper;
using Microsoft.Extensions.Options;
using System;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Linq;
using JZ.IMS.ViewModels;
using JZ.IMS.Core.Extensions;

namespace JZ.IMS.Repository.Oracle
{
    /// <summary>
    /// 数据字典
    /// </summary>
    public class SfcsParametersRepository : BaseRepository<SfcsParameters, decimal>, ISfcsParametersRepository
    {
        public SfcsParametersRepository(IOptionsSnapshot<DbOption> options)
        {
            _dbOption = options.Get("iWMS");
            if (_dbOption == null)
            {
                throw new ArgumentNullException(nameof(DbOption));
            }
            _dbConnection = ConnectionFactory.CreateConnection(_dbOption.DbType, _dbOption.ConnectionString);
        }

        /// <summary>
        /// 通过条件获取呼叫类型
        /// </summary>
        /// <returns>结果集</returns>
        public List<SfcsParameters> GetListByCondition()
        {
            string conditions = "WHERE LOOKUP_TYPE = 'ANDON_CALL_TYPE' AND ENABLED = 'Y'";
            return GetList(conditions).ToList();
        }

        /// <summary>
        /// 通过条件获取呼叫类型（条件为：CHINESE）
        /// </summary>
        /// <returns>结果集</returns>
        public List<SfcsParameters> GetListByChinese(string CHINESE)
        {
            string conditions = "WHERE LOOKUP_TYPE = 'ANDON_CALL_TYPE' AND ENABLED = 'Y' AND CHINESE=:CHINESE";
            return GetList(conditions, new { CHINESE }).ToList();
        }

        /// <summary>
        /// 获取全部线体的物理位置
        /// </summary>
        /// <returns>结果集</returns>
        public List<SfcsParameters> GetPhysicalLocationList()
        {
            string conditions = "WHERE LOOKUP_TYPE = 'PHYSICAL_LOCATION' AND ENABLED = 'Y'";
            return GetList(conditions).ToList();
        }

        /// <summary>
        /// 获取全部线体的厂别
        /// </summary>
        /// <returns>结果集</returns>
        public List<SfcsParameters> GetPlantCodeList()
        {
            string conditions = "WHERE LOOKUP_TYPE = 'PLANT_CODE' AND ENABLED = 'Y'";
            return GetList(conditions).ToList();
        }

        /// <summary>
        /// 获取全部工序类别
        /// </summary>
        /// <returns>结果集</returns>
        public List<SfcsParameters> GetOperationCategoryList()
        {
            string conditions = "WHERE LOOKUP_TYPE = 'OPERATION_CATEGORY' AND ENABLED = 'Y'";
            return GetList(conditions).ToList();
        }

        /// <summary>
        /// 获取全部设备种类
        /// </summary>
        /// <returns>结果集</returns>
        public List<SfcsParameters> GetEquipmentCategoryList()
        {
            string conditions = "WHERE LOOKUP_TYPE = 'EQUIPMENT_CATEGORY' AND ENABLED = 'Y'  order by CHINESE desc";
            return GetList(conditions).ToList();
        }

        /// <summary>
        /// 获取经营单位
        /// </summary>
        /// <returns>结果集</returns>
        public List<SfcsParameters> GetBusinessUnitsList()
        {
            string sql = "SELECT ID, CHINESE FROM SFCS_PARAMETERS WHERE LOOKUP_TYPE = 'SBU_CODE' AND ENABLED = 'Y'";
            return _dbConnection.Query<SfcsParameters>(sql).ToList();
        }

        /// <summary>
        /// 获取全部部门
        /// </summary>
        /// <returns>结果集</returns>
        public List<SfcsDepartment> GetDepartmentList()
        {
            string sql = @"SELECT A.ID, A.CHINESE, B.CHINESE AS SBU_CHINESE, B.ID AS SBU_ID FROM SFCS_LOOKUPS A 
				INNER JOIN SFCS_PARAMETERS B ON B.ID = A.KIND AND A.ENABLED = 'Y' AND B.LOOKUP_TYPE = 'SBU_CODE'
				AND B.ENABLED = 'Y'";
            return _dbConnection.Query<SfcsDepartment>(sql).ToList();
        }

        /// <summary>
        /// 根据类型获取字典信息
        /// </summary>
        /// <returns>结果集</returns>
        public List<SfcsParameters> GetListByType(string type)
        {
            string sql = @"SELECT ID,LOOKUP_TYPE,NAME,LOOKUP_CODE,MEANING,DESCRIPTION,CHINESE,ENABLED FROM SFCS_PARAMETERS  WHERE LOOKUP_TYPE=:LOOKUP_TYPE and ENABLED = 'Y'  order by CHINESE  ";
            return _dbConnection.Query<SfcsParameters>(sql, new { LOOKUP_TYPE = type }).ToList();
        }

        /// <summary>
		/// 根据全部板型
		/// </summary>
		/// <returns>结果集</returns>
		public List<SfcsParameters> GetSmtLookupListByType(string type)
        {
            string sql = @"SELECT CODE AS LOOKUP_CODE,CN_DESC AS MEANING FROM SMT_LOOKUP WHERE TYPE=:TYPE";
            return _dbConnection.Query<SfcsParameters>(sql, new { TYPE = type }).ToList();
        }

        /// <summary>
        /// 获取类型下拉表
        /// </summary>
        /// <returns></returns>
        public async Task<TableDataModel> GetDistinctList(SfcsParametersRequestModel model)
        {

            string condition = " WHERE 1=1 ";
            if (!model.Key.IsNullOrWhiteSpace())
            {
                condition += $" AND (INSTR(UPPer(LOOKUP_TYPE), UPPer(:Key))) > 0  OR (INSTR(NAME, :Key)) > 0 ";
            }

            string sql = @" SELECT ROWNUM AS ROWNO,S.* FROM (SELECT DISTINCT LOOKUP_TYPE,NAME  FROM SFCS_PARAMETERS " + condition + " ORDER BY NAME ASC) S ";
            string pagesql = SQLBuilderClass.GetPagedSQL(sql, "LOOKUP_TYPE", "");
            var resdata = await _dbConnection.QueryAsync(pagesql, model);

            string sqlcnt = @" SELECT COUNT(*) FROM (SELECT DISTINCT LOOKUP_TYPE,NAME FROM SFCS_PARAMETERS " + condition + " ) ";
            int cnt = await _dbConnection.ExecuteScalarAsync<int>(sqlcnt, model);
            return new TableDataModel
            {
                count = cnt,
                data = resdata?.ToList(),
            };
        }

        /// <summary>
        /// 保存数据
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public async Task<decimal> SaveDataByTrans(SfcsParametersModel model)
        {
            int result = 1;
            ConnectionFactory.OpenConnection(_dbConnection);
            using (var tran = _dbConnection.BeginTransaction())
            {
                try
                {
                    //新增
                    string insertSql = @"INSERT INTO SFCS_PARAMETERS 
					(ID,LOOKUP_TYPE,NAME,LOOKUP_CODE,MEANING,DESCRIPTION,CHINESE,ENABLED) 
					VALUES (:ID,:LOOKUP_TYPE,:NAME,:LOOKUP_CODE,:MEANING,:DESCRIPTION,:CHINESE,:ENABLED)";
                    if (model.insertRecords != null && model.insertRecords.Count > 0)
                    {
                        foreach (var item in model.insertRecords)
                        {
                            var newid = await Get_MES_SEQ_ID();
                            item.LOOKUP_TYPE = item.LOOKUP_TYPE.ToUpper();
                            var resdata = await _dbConnection.ExecuteAsync(insertSql, new
                            {
                                ID = newid,
                                item.LOOKUP_TYPE,
                                item.NAME,
                                item.LOOKUP_CODE,
                                item.MEANING,
                                item.DESCRIPTION,
                                item.CHINESE,
                                item.ENABLED,

                            }, tran);
                        }
                    }
                    //更新
                    string updateSql = @"Update SFCS_PARAMETERS set LOOKUP_TYPE=:LOOKUP_TYPE,NAME=:NAME,LOOKUP_CODE=:LOOKUP_CODE,MEANING=:MEANING,DESCRIPTION=:DESCRIPTION,CHINESE=:CHINESE,ENABLED=:ENABLED  
						where ID=:ID ";
                    if (model.updateRecords != null && model.updateRecords.Count > 0)
                    {
                        foreach (var item in model.updateRecords)
                        {
                            item.LOOKUP_TYPE = item.LOOKUP_TYPE.ToUpper();
                            var resdata = await _dbConnection.ExecuteAsync(updateSql, new
                            {
                                item.ID,
                                item.LOOKUP_TYPE,
                                item.NAME,
                                item.LOOKUP_CODE,
                                item.MEANING,
                                item.DESCRIPTION,
                                item.CHINESE,
                                item.ENABLED,
                            }, tran);
                        }
                    }
                    //删除
                    string deleteSql = @"Delete from SFCS_PARAMETERS where ID=:ID ";
                    if (model.removeRecords != null && model.removeRecords.Count > 0)
                    {
                        foreach (var item in model.removeRecords)
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
        public async Task<bool> SaveMachineByTrans(SaveMachineConfigModel model)
        {
            int result = 1;
            ConnectionFactory.OpenConnection(_dbConnection);
            using (var tran = _dbConnection.BeginTransaction())
            {
                try
                {
                    string selectConfigSql = "SELECT * FROM SFCS_SITE_MACHINE_CONFIG WHERE SITE_ID=:SITE_ID  AND ENABLE='Y'";
                    //string selectDeSql = "SELECT * FROM SFCS_SITE_MACHINE_DETAIL WHERE KEY=:KEY  AND MST_ID=:MST_ID AND ENABLE='Y'";
                    //新增
                    string insertConfigSql = @"INSERT INTO SFCS_SITE_MACHINE_CONFIG 
					(ID,SITE_ID,MACHINE_TYPE,LINKE_TYPE,CHREATE_TIME,CREATOR,ENABLE) 
					VALUES (:ID,:SITE_ID,:MACHINE_TYPE,:LINKE_TYPE,sysdate,:CREATOR,'Y')";

                    string insertDetailSql = @"INSERT INTO SFCS_SITE_MACHINE_DETAIL 
					(ID,MST_ID,KEY,VALUE,DESCRIPTION,CHREATE_TIME,CREATOR,ENABLE) 
					VALUES (SFCS_SITE_MACHINE_SEQ.NEXTVAL,:MST_ID,:KEY,:VALUE,:DESCRIPTION,sysdate,:CREATOR,'Y')";

                    string updateConfigSql = @"UPDATE SFCS_SITE_MACHINE_CONFIG 
					SET SITE_ID=:SITE_ID,MACHINE_TYPE=:MACHINE_TYPE,LINKE_TYPE=:LINKE_TYPE,CREATOR=:CREATOR
					WHERE ID=:ID";

                    string deleteDetailSql = @"DELETE SFCS_SITE_MACHINE_DETAIL WHERE MST_ID=:MST_ID ";

                    if (model != null)
                    {
                        decimal mstID = 0;

                        var siteMachineModel = (await _dbConnection.QueryAsync<SfcsSiteMachineConfig>(selectConfigSql, new
                        {
                            SITE_ID = model.SiteID
                        }))?.FirstOrDefault();

                        #region 处理设备主表

                        if (siteMachineModel == null)
                        {
                            string seqSql = @" SELECT SFCS_SITE_MACHINE_SEQ.NEXTVAL FROM DUAL";
                            mstID = await _dbConnection.ExecuteScalarAsync<decimal>(seqSql);
                            //新增
                            var resdata = await _dbConnection.ExecuteAsync(insertConfigSql, new
                            {
                                ID = mstID,
                                SITE_ID = model.SiteID,
                                MACHINE_TYPE = model?.MachineDevType ?? 0,
                                LINKE_TYPE = model.LinkeType,
                                CREATOR = model.UserName
                            }, tran);
                        }
                        else
                        {
                            mstID = siteMachineModel.ID;
                            //修改
                            var resdata = await _dbConnection.ExecuteAsync(updateConfigSql, new
                            {
                                ID = mstID,
                                SITE_ID = model.SiteID,
                                MACHINE_TYPE = model?.MachineDevType ?? 0,
                                LINKE_TYPE = model.LinkeType,
                                CREATOR = model.UserName
                            }
                                , tran);
                        }

                        #endregion

                        #region 处理设备明细表
                        if (model.ItemList != null && model.ItemList.Count > 0)
                        {
                            await _dbConnection.ExecuteAsync(deleteDetailSql, new
                            {
                                MST_ID = mstID
                            }, tran);
                            foreach (var item in model.ItemList)
                            {
                                var resdata = await _dbConnection.ExecuteAsync(insertDetailSql, new
                                {
                                    MST_ID = mstID,
                                    KEY = item.Key,
                                    VALUE = item.Value,
                                    DESCRIPTION = item.Description,
                                    CREATOR = model.UserName
                                });

                            }

                        }
                        #endregion

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
            return result > 0 ? true : false ;
        }

    }
}
