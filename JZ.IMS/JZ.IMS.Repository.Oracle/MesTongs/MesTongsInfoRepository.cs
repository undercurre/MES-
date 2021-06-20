/**
*┌──────────────────────────────────────────────────────────────┐
*│　描    述：夹具基本信息表接口实现                                                    
*│　作    者：嘉志科技                                            
*│　版    本：2.0    模板代码自动生成                                                
*│　创建时间：2019-12-20 17:39:29                             
*└──────────────────────────────────────────────────────────────┘
*┌──────────────────────────────────────────────────────────────┐
*│　命名空间： JZ.IMS.Repository.Oracle                                  
*│　类    名： MesTongsInfoRepository                                      
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
using JZ.IMS.ViewModels;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using JZ.IMS.IRepository.MesTongs;
using JZ.IMS.Core.Extensions;
using JZ.IMS.ViewModels.MesTongs;

namespace JZ.IMS.Repository.Oracle
{
    public class MesTongsInfoRepository : BaseRepository<MesTongsInfo, Decimal>, IMesTongsInfoRepository
    {

        private readonly IMesTongsApplyRepository _applyRepository;
        public MesTongsInfoRepository(IOptionsSnapshot<DbOption> options, IMesTongsApplyRepository applyRepository)
        {
            _applyRepository = applyRepository;
            _dbOption = options.Get("iWMS");
            if (_dbOption == null)
            {
                throw new ArgumentNullException(nameof(DbOption));
            }
            _dbConnection = ConnectionFactory.CreateConnection(_dbOption.DbType, _dbOption.ConnectionString);
        }

        #region 对外方法
        /// <summary>
        /// 获取夹具列表信息
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>		
        public async Task<IEnumerable<MesTongsInfoListModel>> GetTongsData(MesTongsInfoRequestModel model)
        {
            //string sql = @"select * from (select tt.*,ROWNUM AS rowno from (
            //		SELECT MST.*,PARA.CHINESE AS DEPARTMENT_NAME,STORE.CODE AS STORE_CODE,STORE.NAME AS STORE_NAME FROM MES_TONGS_INFO MST
            //		LEFT JOIN （SELECT A.ID, A.CHINESE, B.CHINESE AS SBU_CHINESE, B.ID AS SBU_ID FROM SFCS_LOOKUPS A 
            //						INNER JOIN SFCS_PARAMETERS B ON B.ID = A.KIND AND A.ENABLED = 'Y' AND B.LOOKUP_TYPE = 'SBU_CODE'
            //						AND B.ENABLED = 'Y'） PARA ON MST.DEPARTMENT = PARA.ID
            //                 LEFT JOIN MES_TONGS_STORE_CONFIG STORE ON MST.STORE_ID = STORE.ID
            //		";
            string sql = @"select * from (select tt.*,ROWNUM AS rowno from (
							SELECT MST.*,PARA.ORGANIZE_NAME,STORE.CODE AS STORE_CODE,STORE.NAME AS STORE_NAME,SPS1.MEANING MES_TONGS_TYPE FROM MES_TONGS_INFO MST
							LEFT JOIN （select ORGANIZE_NAME,ID from SYS_ORGANIZE） PARA ON MST.ORGANIZE_ID = PARA.ID
							LEFT JOIN MES_TONGS_STORE_CONFIG STORE ON MST.STORE_ID = STORE.ID
                            LEFT JOIN SFCS_PARAMETERS SPS1 ON SPS1.LOOKUP_TYPE = 'MES_TONGS_TYPE' AND SPS1.ENABLED = 'Y' AND SPS1.LOOKUP_CODE = MST.TONGS_TYPE
					";

            sql += GetWhereStr(model);
            sql += @" order by MST.ID DESC ) tt where ROWNUM <= :Limit*:Page) tt2 where tt2.rowno >= (:Page-1)*:Limit";

            return await _dbConnection.QueryAsync<MesTongsInfoListModel>(sql, model);
        }

        /// <summary>
        /// 导出夹具列表信息
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>		
        public async Task<IEnumerable<dynamic>> GetExportData(MesTongsInfoRequestModel model)
        {
            //string sql = @"select * from (select tt.*,ROWNUM AS rowno from (
            //		SELECT MST.*,PARA.CHINESE AS DEPARTMENT_NAME,STORE.CODE AS STORE_CODE,STORE.NAME AS STORE_NAME FROM MES_TONGS_INFO MST
            //		LEFT JOIN （SELECT A.ID, A.CHINESE, B.CHINESE AS SBU_CHINESE, B.ID AS SBU_ID FROM SFCS_LOOKUPS A 
            //						INNER JOIN SFCS_PARAMETERS B ON B.ID = A.KIND AND A.ENABLED = 'Y' AND B.LOOKUP_TYPE = 'SBU_CODE'
            //						AND B.ENABLED = 'Y'） PARA ON MST.DEPARTMENT = PARA.ID
            //                 LEFT JOIN MES_TONGS_STORE_CONFIG STORE ON MST.STORE_ID = STORE.ID
            //		";
            string sql = @"select * from (select tt.*,ROWNUM AS rowno from (
							SELECT
	MST.ID,
	MST.CODE,
	SPS1.MEANING TONGS_TYPE,
	SPS2.CHINESE DEPARTMENT,
	SPS3.NAME SOURCES,
	SPS4.NAME STATUS,
	MSC.NAME STORE_ID,
	MST.CREATE_USER,
	MST.CREATE_DATE,
	MST.UPDATE_USER,
	MST.UPDATE_DATE,
	MST.ACTIVE,
	MST.ENABLED,
	PARA.ORGANIZE_NAME ORGANIZE_ID,
	MST.APPLY_ID, 
	MST.PRINCIPAL
FROM
	MES_TONGS_INFO MST
	LEFT JOIN （ SELECT
	ORGANIZE_NAME,
	ID 
FROM
	SYS_ORGANIZE） PARA ON MST.ORGANIZE_ID = PARA.ID
	LEFT JOIN MES_TONGS_STORE_CONFIG STORE ON MST.STORE_ID = STORE.ID
	LEFT JOIN SFCS_PARAMETERS SPS1 ON SPS1.LOOKUP_TYPE = 'MES_TONGS_TYPE' 
	AND SPS1.ENABLED = 'Y' 
	AND SPS1.LOOKUP_CODE = MST.TONGS_TYPE
	LEFT JOIN (
	SELECT
		A.ID,
		A.CHINESE,
		B.CHINESE AS SBU_CHINESE,
		B.ID AS SBU_ID 
	FROM
		SFCS_LOOKUPS A
		INNER JOIN SFCS_PARAMETERS B ON B.ID = A.KIND 
		AND A.ENABLED = 'Y' 
		AND B.LOOKUP_TYPE = 'SBU_CODE' 
		AND B.ENABLED = 'Y' 
	) SPS2 ON SPS2.ID = MST.DEPARTMENT
	LEFT JOIN ( SELECT 0 ID, '自制' NAME FROM dual UNION SELECT 1 ID, '外购' NAME FROM dual UNION SELECT 2 ID, '转移' NAME FROM dual ) SPS3 ON SPS3.ID = MST.SOURCES
	LEFT JOIN (
	SELECT
		0 ID,
		'待入库' NAME 
	FROM
		dual UNION
	SELECT
		1 ID,
		'存储中' NAME 
	FROM
		dual UNION
	SELECT
		2 ID,
		'借出' NAME 
	FROM
		dual UNION
	SELECT
		3 ID,
		'使用中' NAME 
	FROM
		dual UNION
	SELECT
		4 ID,
		'保养中' NAME 
	FROM
		dual UNION
	SELECT
		5 ID,
		'维修中' NAME 
	FROM
		dual UNION
	SELECT
		6 ID,
		'已报废' NAME 
	FROM
		dual 
	) SPS4 ON SPS4.ID = MST.STATUS
	LEFT JOIN MES_TONGS_STORE_CONFIG MSC ON MSC.ID = MST.STORE_ID
					";

            sql += GetWhereStr(model);
            sql += @" order by MST.ID DESC ) tt where ROWNUM <= :Limit*:Page) tt2 where tt2.rowno >= (:Page-1)*:Limit";

            return await _dbConnection.QueryAsync<dynamic>(sql, model);
        }

        /// <summary>
        /// 获取夹具与工位绑定
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>		
        public async Task<TableDataModel> GetTongsSiteByCodeAsync(MesTongsSiteRequestModel model)
        {
            TableDataModel dataModel = new TableDataModel();
            try
            {
                string conditions = " WHERE 1=1";

                if (model.ID > 0)
                {
                    conditions += " AND MTS.ID=:ID ";
                }

                if (model.SiteID > 0)
                {
                    conditions += " AND MTS.SITE_ID=:SiteID ";
                }

                if (!model.Code.IsNullOrWhiteSpace())
                {
                    conditions += " AND MTI.CODE=:Code ";
                }

                var sql = $@"SELECT * FROM (SELECT TT.*,ROWNUM AS ROWNO FROM (
								SELECT MTS.ID,MTS.TONGS_ID,MTI.CODE,MTS.SITE_ID,SOS.OPERATION_SITE_NAME,LINES.OPERATION_LINE_NAME FROM MES_TONGS_SITE MTS
								LEFT JOIN MES_TONGS_INFO  MTI on MTS.TONGS_ID=MTI.id
								LEFT JOIN SFCS_OPERATION_SITES SOS on MTS.SITE_ID=SOS.id
								LEFT JOIN SFCS_OPERATION_LINES LINES on LINES.id=SOS.OPERATION_LINE_ID
								{conditions} 
							ORDER BY MTS.ID DESC ) TT WHERE ROWNUM <= :Limit*:Page) TT2 WHERE TT2.ROWNO >= (:Page-1)*:Limit
							";

                string cnt = $@"SELECT COUNT(*)
							   FROM MES_TONGS_SITE MTS
							   LEFT JOIN MES_TONGS_INFO  MTI on MTS.TONGS_ID=MTI.id
								LEFT JOIN SFCS_OPERATION_SITES SOS on MTS.SITE_ID=SOS.id
								LEFT JOIN SFCS_OPERATION_LINES LINES on LINES.id=SOS.OPERATION_LINE_ID
							{ conditions} ";


                dataModel.data = await _dbConnection.QueryAsync<QuertyTongsSiteViewModel>(sql, model);
                dataModel.count = await _dbConnection.ExecuteScalarAsync<int>(cnt, model);
            }
            catch (Exception ex)
            {
                dataModel.code = -1;
                dataModel.msg = ex.Message;
            }
            return dataModel;
        }

        /// <summary>
        /// 获取夹具信息数量
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public async Task<int> GetTongsDataCount(MesTongsInfoRequestModel model)
        {
            string whereStr = GetWhereStr(model);
            StringBuilder sql = new StringBuilder();
            sql.Append("select count(*) from MES_TONGS_INFO MST ");
            sql.Append(whereStr);

            return await _dbConnection.ExecuteScalarAsync<int>(sql.ToString(), model);
        }


        /// <summary>
        /// 判断夹具编码是否存在
        /// </summary>
        /// <param name="code"></param>
        /// <returns></returns>
        public async Task<bool> IsExistCode(string code)
        {
            string sql = "SELECT COUNT(1) FROM MES_TONGS_INFO WHERE CODE=:CODE";
            return await _dbConnection.ExecuteScalarAsync<int>(sql, new { CODE = code }) > 0;
        }

        /// <summary>
        /// 根据ID获取夹具信息
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<MesTongsInfoListModel> GetTongsById(decimal id)
        {
            string sql = @"SELECT * FROM MES_TONGS_INFO WHERE ID=:ID";
            var data = (await _dbConnection.QueryAsync<MesTongsInfoListModel>(sql, new { ID = id })).FirstOrDefault();
            data.PartList = await GetTongsPartData(id);
            data.FamilyList = await GetTongsProductFamilyByID(id);
            return data;
        }

        /// <summary>
        /// 申请入库（批量新增）
        /// </summary>
        /// <param name="list"></param>
        /// <returns></returns>
        public async Task<BaseResult> ApplyGoStore(List<MesTongsInfoListModel> list, string user, string organizeId)
        {
            var result = new BaseResult();
            if (list.Count() == 0)
            {
                result.ResultCode = ResultCodeAddMsgKeys.CommonExceptionCode;
                result.ResultMsg = $"没有需要申请入库的数据！";
                return result;
            }

            string addTongsSql = @"INSERT INTO MES_TONGS_INFO(ID,CODE,TONGS_TYPE,DEPARTMENT,SOURCES,STATUS,STORE_ID,CREATE_USER,CREATE_DATE,UPDATE_USER,UPDATE_DATE,ACTIVE,ENABLED,ORGANIZE_ID,APPLY_ID,PRINCIPAL) VALUES(
									:ID,:CODE,:TONGS_TYPE,:DEPARTMENT,:SOURCES,1,:STORE_ID,:CREATE_USER,SYSDATE,:CREATE_USER,SYSDATE,'N','Y',:ORGANIZE_ID,:APPLY_ID,:PRINCIPAL)";
            string addTongsPartSql = @"INSERT INTO MES_TONGS_PART(ID,TONGS_ID,PART_NO,PART_NAME,PART_DESC,VERSION,CREATE_USER,CREATE_DATE,ENABLED) VALUES(
									MES_TONGS_PART_SEQ.NEXTVAL,:TONGS_ID,:PART_NO,:PART_NAME,:PART_DESC,:VERSION,:CREATE_USER,SYSDATE,'Y')";

            var apply = await _applyRepository.GetAsync(list[0].APPLY_ID);
            var partList = await _applyRepository.GetTongsApplyPartData(list[0].APPLY_ID);

            if (apply.SURPLUS_QTY < list.Count())
            {
                result.ResultCode = ResultCodeAddMsgKeys.CommonExceptionCode;
                result.ResultMsg = $"当前夹具申请信息只能入库{apply.SURPLUS_QTY}个夹具，目前已经{list.Count()}个！";
                return result;
            }

            ConnectionFactory.OpenConnection(_dbConnection);
            using (var tran = _dbConnection.BeginTransaction())
            {
                try
                {
                    foreach (var item in list)
                    {
                        item.ID = await GetSEQID();
                        item.TONGS_TYPE = apply.TONGS_TYPE;
                        item.DEPARTMENT = apply.DEPARTMENT;
                        item.SOURCES = apply.SOURCES;
                        item.CREATE_USER = user;
                        item.ORGANIZE_ID = organizeId;

                        if (await _dbConnection.ExecuteAsync(addTongsSql, item) == 1)
                        {
                            if (partList != null)
                            {
                                foreach (var part in partList)
                                {
                                    part.TONGS_ID = item.ID;
                                    part.CREATE_USER = item.CREATE_USER;
                                    await _dbConnection.ExecuteAsync(addTongsPartSql, part);
                                }
                            }

                            //插入操作记录信息
                            MesTongsOperationHistory data = new MesTongsOperationHistory
                            {
                                CREATE_USER = item.CREATE_USER,
                                OPERATION_TYPE = 0,
                                TONGS_ID = item.ID,
                                LAST_STATUS = 1,
                                PRE_STATUS = -1,
                                STORE_ID = item.STORE_ID
                            };
                            await InsertOperationRecord(data);
                        }
                        else
                        {
                            tran.Rollback();
                            result.ResultCode = ResultCodeAddMsgKeys.CommonExceptionCode;
                            result.ResultMsg = "夹具申请入库失败，请刷新后重试！";
                            return result;
                        }
                    }

                    //回写夹具申请信息待入库数量
                    apply.SURPLUS_QTY = apply.SURPLUS_QTY - list.Count();
                    if (apply.SURPLUS_QTY == 0)
                        apply.STATUS = 2;

                    await _applyRepository.UpdateAsync(apply);

                    tran.Commit();
                    result.ResultCode = ResultCodeAddMsgKeys.CommonObjectSuccessCode;
                    result.ResultMsg = ResultCodeAddMsgKeys.CommonObjectSuccessMsg;
                    return result;
                }
                catch (Exception ex)
                {
                    tran.Rollback();
                    result.ResultCode = ResultCodeAddMsgKeys.CommonExceptionCode;
                    result.ResultMsg = ex.Message;
                    return result;
                }
                finally
                {
                    if (_dbConnection.State != System.Data.ConnectionState.Closed)
                    {
                        _dbConnection.Close();
                    }
                }
            }
        }

        /// <summary>
        /// 申请入库
        /// </summary>
        /// <returns></returns>
        public async Task<BaseResult> ApplyGoStoreByModel(MesTongsInfoListModel model, string user, string organizeId)
        {
            var result = new BaseResult();

            string addTongsSql = @"INSERT INTO MES_TONGS_INFO(ID,CODE,TONGS_TYPE,DEPARTMENT,SOURCES,STATUS,STORE_ID,CREATE_USER,CREATE_DATE,UPDATE_USER,UPDATE_DATE,ACTIVE,ENABLED,ORGANIZE_ID,APPLY_ID,TONGS_MODEL,PRINCIPAL) VALUES(
									:ID,:CODE,:TONGS_TYPE,:DEPARTMENT,:SOURCES,1,:STORE_ID,:CREATE_USER,SYSDATE,:CREATE_USER,SYSDATE,'N','Y',:ORGANIZE_ID,:APPLY_ID,:TONGS_MODEL,:PRINCIPAL)";
            string addTongsPartSql = @"INSERT INTO MES_TONGS_PART(ID,TONGS_ID,PART_NO,PART_NAME,PART_DESC,VERSION,CREATE_USER,CREATE_DATE,ENABLED) VALUES(
									MES_TONGS_PART_SEQ.NEXTVAL,:TONGS_ID,:PART_NO,:PART_NAME,:PART_DESC,:VERSION,:CREATE_USER,SYSDATE,'Y')";

            ConnectionFactory.OpenConnection(_dbConnection);
            using (var tran = _dbConnection.BeginTransaction())
            {
                try
                {
                    model.ID = await GetSEQID();
                    model.APPLY_ID = 0;//默认为零
                    model.CREATE_USER = user;
                    model.ORGANIZE_ID = organizeId;
                    if (await _dbConnection.ExecuteAsync(addTongsSql, model) == 1)
                    {
                        if (model.PartList != null)
                        {
                            foreach (var part in model.PartList)
                            {
                                part.TONGS_ID = model.ID;
                                part.CREATE_USER = model.CREATE_USER;
                                await _dbConnection.ExecuteAsync(addTongsPartSql, part);
                            }
                        }
                        //插入操作记录信息
                        MesTongsOperationHistory data = new MesTongsOperationHistory
                        {
                            CREATE_USER = model.CREATE_USER,
                            OPERATION_TYPE = 0,
                            TONGS_ID = model.ID,
                            LAST_STATUS = 1,
                            PRE_STATUS = -1,
                            STORE_ID = model.STORE_ID
                        };
                        await InsertOperationRecord(data);
                    }
                    else
                    {
                        tran.Rollback();
                        result.ResultCode = ResultCodeAddMsgKeys.CommonExceptionCode;
                        result.ResultMsg = "夹具申请入库失败，请刷新后重试！";
                        return result;
                    }

                    tran.Commit();
                    result.ResultCode = ResultCodeAddMsgKeys.CommonObjectSuccessCode;
                    result.ResultMsg = ResultCodeAddMsgKeys.CommonObjectSuccessMsg;
                    return result;
                }
                catch (Exception ex)
                {
                    tran.Rollback();
                    result.ResultCode = ResultCodeAddMsgKeys.CommonExceptionCode;
                    result.ResultMsg = ex.Message;
                    return result;
                }
                finally
                {
                    if (_dbConnection.State != System.Data.ConnectionState.Closed)
                    {
                        _dbConnection.Close();
                    }
                }
            }
        }

        /// <summary>
        /// 新增/修改夹具信息
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public async Task<BaseResult> AddOrModify(MesTongsInfoListModel model)
        {
            var result = new BaseResult();
            if (await IsExistByCode(model.CODE, model.ID))
            {
                result.ResultCode = ResultCodeAddMsgKeys.CommonExceptionCode;
                result.ResultMsg = $"夹具编码【{model.CODE}】已经存在！";
                return result;
            }

            string addTongsSql = @"INSERT INTO MES_TONGS_INFO(ID,CODE,TONGS_TYPE,DEPARTMENT,SOURCES,STATUS,CREATE_USER,CREATE_DATE,ACTIVE,ENABLED,TONGS_MODEL,PRINCIPAL) VALUES(
									:ID,:CODE,:TONGS_TYPE,:DEPARTMENT,:SOURCES,0,:CREATE_USER,SYSDATE,'N','Y',:TONGS_MODEL,:PRINCIPAL)";
            string editTongsSql = @"UPDATE MES_TONGS_INFO SET TONGS_TYPE=:TONGS_TYPE,DEPARTMENT=:DEPARTMENT,SOURCES=:SOURCES,UPDATE_USER=:UPDATE_USER,UPDATE_DATE=SYSDATE,ORGANIZE_ID=:ORGANIZE_ID,TONGS_MODEL=:TONGS_MODEL,PRINCIPAL=:PRINCIPAL,STATUS=:STATUS WHERE ID=:ID";
            string delTongsPartSql = @"DELETE MES_TONGS_PART WHERE TONGS_ID = :TONGS_ID";
            string delTongsProductFamilySql = @"DELETE MES_TONGS_PRODUCT_FAMILY WHERE TONGS_ID = :TONGS_ID";
            string addTongsPartSql = @"INSERT INTO MES_TONGS_PART(ID,TONGS_ID,PART_NO,PART_NAME,PART_DESC,VERSION,CREATE_USER,CREATE_DATE,ENABLED) VALUES(
									MES_TONGS_PART_SEQ.NEXTVAL,:TONGS_ID,:PART_NO,:PART_NAME,:PART_DESC,:VERSION,:CREATE_USER,SYSDATE,'Y')";
            string addTongsProductFamilySql = @"INSERT INTO MES_TONGS_PRODUCT_FAMILY(ID,TONGS_ID,PRODUCT_FAMILY_ID,CREATE_TIME,CREATE_USER,ENABLED) VALUES(
									MES_TONGS_PART_SEQ.NEXTVAL,:TONGS_ID,:PRODUCT_FAMILY_ID,SYSDATE,:CREATE_USER,'Y')";

            ConnectionFactory.OpenConnection(_dbConnection);
            using (var tran = _dbConnection.BeginTransaction())
            {
                try
                {
                    if (model.ID == 0)
                    {//新增
                        model.ID = await GetSEQID();
                        if (await _dbConnection.ExecuteAsync(addTongsSql, model, tran) == 1)
                        {
                            if (model.PartList != null)
                                foreach (var part in model.PartList)
                                {
                                    part.TONGS_ID = model.ID;
                                    part.CREATE_USER = model.CREATE_USER;
                                    await _dbConnection.ExecuteAsync(addTongsPartSql, part, tran);
                                }

                            if (model.FamilyList != null)
                                foreach (var family in model.FamilyList)
                                {
                                    await _dbConnection.ExecuteAsync(addTongsProductFamilySql, new
                                    {
                                        TONGS_ID = model.ID,
                                        PRODUCT_FAMILY_ID = family.PRODUCT_FAMILY_ID,
                                        CREATE_USER = model.CREATE_USER
                                    }, tran);
                                }

                            //插入操作记录信息
                            MesTongsOperationHistory data = new MesTongsOperationHistory
                            {
                                CREATE_USER = model.CREATE_USER,
                                OPERATION_TYPE = 0,
                                TONGS_ID = model.ID,
                                LAST_STATUS = 0,
                                PRE_STATUS = -1,
                                STORE_ID = model.STORE_ID
                            };
                            await InsertOperationRecord(data);
                        }
                        else
                        {
                            tran.Rollback();
                            result.ResultCode = ResultCodeAddMsgKeys.CommonExceptionCode;
                            result.ResultMsg = "注册夹具失败，请刷新后重试！";
                            return result;
                        }
                    }
                    else
                    {//修改
                        if (await _dbConnection.ExecuteAsync(editTongsSql, model, tran) == 1)
                        {
                            await _dbConnection.ExecuteAsync(delTongsPartSql, new { TONGS_ID = model.ID }, tran);

                            await _dbConnection.ExecuteAsync(delTongsProductFamilySql, new { TONGS_ID = model.ID }, tran);

                            if (model.PartList != null)
                                foreach (var part in model.PartList)
                                {
                                    part.TONGS_ID = model.ID;
                                    part.CREATE_USER = model.UPDATE_USER;
                                    await _dbConnection.ExecuteAsync(addTongsPartSql, part, tran);
                                }

                            if (model.FamilyList != null)
                                foreach (var family in model.FamilyList)
                                {
                                    await _dbConnection.ExecuteAsync(addTongsProductFamilySql, new
                                    {
                                        TONGS_ID = model.ID,
                                        PRODUCT_FAMILY_ID = family.PRODUCT_FAMILY_ID,
                                        CREATE_USER = model.CREATE_USER
                                    }, tran);
                                }
                        }
                        else
                        {
                            tran.Rollback();
                            result.ResultCode = ResultCodeAddMsgKeys.CommonExceptionCode;
                            result.ResultMsg = "更新夹具失败，请刷新后重试！";
                            return result;
                        }
                    }
                    tran.Commit();
                    result.ResultCode = 0;
                    result.ResultMsg = "操作成功！";
                    return result;
                }
                catch (Exception ex)
                {
                    tran.Rollback();
                    result.ResultCode = ResultCodeAddMsgKeys.CommonExceptionCode;
                    result.ResultMsg = ex.Message;
                    return result;
                }
                finally
                {
                    if (_dbConnection.State != System.Data.ConnectionState.Closed)
                    {
                        _dbConnection.Close();
                    }
                }
            }
        }

        /// <summary>
        /// 根据主键获取激活状态
        /// </summary>
        /// <param name="id">主键</param>
        /// <returns></returns>
        public async Task<Boolean> GetActiveStatus(decimal id)
        {
            string sql = "SELECT ACTIVE FROM MES_TONGS_INFO WHERE ID=:ID";
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
        public async Task<decimal> ChangeActiveStatus(decimal id, bool status, string user)
        {
            string sql = "UPDATE MES_TONGS_INFO set ACTIVE=:ACTIVE,UPDATE_USER=:UPDATE_USER,UPDATE_DATE = SYSDATE WHERE ID=:Id";
            return await _dbConnection.ExecuteAsync(sql, new
            {
                ACTIVE = status ? 'Y' : 'N',
                UPDATE_USER = user,
                Id = id,
            });
        }

        /// <summary>
        /// 获取表的序列
        /// </summary>
        /// <returns></returns>
        public async Task<decimal> GetSEQID()
        {
            string sql = "SELECT MES_TONGS_INFO_SEQ.NEXTVAL MY_SEQ FROM DUAL";
            var result = await _dbConnection.ExecuteScalarAsync(sql);
            return (decimal)result;
        }

        /// <summary>
        /// 根据夹具ID获取对应产品信息
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<List<MesTongsPartModel>> GetTongsPartData(decimal id)
        {
            string sql = "SELECT * FROM MES_TONGS_PART WHERE TONGS_ID = :TONGS_ID AND ENABLED = 'Y'";
            return (await _dbConnection.QueryAsync<MesTongsPartModel>(sql, new { TONGS_ID = id })).ToList();
        }

        /// <summary>
        /// 根据夹具ID获取对应产品系列信息
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<List<MesTongsProductFamilyViewModel>> GetTongsProductFamilyByID(decimal id)
        {
            String sql = @"SELECT
						 	MTPF.ID,
						 	MTPF.TONGS_ID,
						 	MTPF.PRODUCT_FAMILY_ID,
						 	MTPF.ENABLED,
						 	SPF.FAMILY_NAME,
						 	SPF.DESCRIPTION,
						 	SCS.CUSTOMER 
						 FROM
						 	MES_TONGS_PRODUCT_FAMILY MTPF,
						 	SFCS_PRODUCT_FAMILY SPF,
						 	SFCS_CUSTOMERS SCS 
						 WHERE
						 	MTPF.PRODUCT_FAMILY_ID = SPF.ID(+) 
						 	AND SCS.id(+) = SPF.CUSTOMER_ID 
							AND TONGS_ID = :TONGS_ID
						 	AND MTPF.ENABLED = 'Y' 
						 	AND SPF.ENABLED = 'Y'";
            return (await _dbConnection.QueryAsync<MesTongsProductFamilyViewModel>(sql, new { TONGS_ID = id })).ToList();
        }

        /// <summary>
        /// 根据夹具ID获取夹具操作记录
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<List<MesTongsOperationHistoryListModel>> GetTongsOperationData(decimal id)
        {
            string sql = @"SELECT MST.*,STORE.CODE AS STORE_CODE,STORE.NAME AS STORE_NAME FROM MES_TONGS_OPERATION_HISTORY MST
                    LEFT JOIN MES_TONGS_STORE_CONFIG STORE ON MST.STORE_ID = STORE.ID WHERE MST.TONGS_ID = :TONGS_ID ORDER BY MST.ID DESC";

            return (await _dbConnection.QueryAsync<MesTongsOperationHistoryListModel>(sql, new { TONGS_ID = id })).ToList();
        }

        /// <summary>
        /// 根据夹具ID获取保养/激活记录
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<List<MesTongsMaintainHistory>> GetMaintainData(decimal id)
        {
            string sql = @"SELECT * FROM MES_TONGS_MAINTAIN_HISTORY WHERE TONGS_ID=:TONGS_ID ORDER BY TYPE,ID DESC";
            return (await _dbConnection.QueryAsync<MesTongsMaintainHistory>(sql, new { TONGS_ID = id })).ToList();
        }

        /// <summary>
        /// 根据夹具ID获取维修记录
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<List<MesTongsRepair>> GetRepairData(decimal id)
        {
            string sql = @"SELECT * FROM MES_TONGS_REPAIR WHERE TONGS_ID=:TONGS_ID ORDER BY ID DESC";
            return (await _dbConnection.QueryAsync<MesTongsRepair>(sql, new { TONGS_ID = id })).ToList();
        }

        /// <summary>
        /// 根据保养主表ID获取保养明细数据
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<List<MesTongsMaintainDetail>> GetMaintainDetailData(decimal id)
        {
            string sql = @"SELECT DETAIL.*,ITEMS.ITEM_NAME,ITEMS.REMARK AS ITEM_DESC FROM MES_TONGS_MAINTAIN_DETAIL DETAIL 
						INNER JOIN MES_TONGS_MAINTAIN_ITEMS ITEMS ON DETAIL.ITEM_ID = ITEMS.ID
						WHERE MST_ID = :MST_ID ORDER BY DETAIL.ID";
            return (await _dbConnection.QueryAsync<MesTongsMaintainDetail>(sql, new { MST_ID = id })).ToList();
        }

        /// <summary>
        /// 根据料号获取产品信息
        /// </summary>
        /// <param name="partNo"></param>
        /// <returns></returns>
        public async Task<ImsPart> GetPartByPartNo(string partNo)
        {
            string sql = @"SELECT NAME,DESCRIPTION FROM IMS_PART WHERE CODE = :CODE";

            return (await _dbConnection.QueryAsync<ImsPart>(sql, new { CODE = partNo })).FirstOrDefault();
        }

        /// <summary>
        /// 夹具入库
        /// </summary>
        /// <param name="tongsId"></param>
        /// <param name="storeId"></param>
        /// <param name="user"></param>
        /// <returns></returns>
        public async Task<BaseResult> EnterStore(decimal tongsId, decimal storeId, string user)
        {
            var result = new BaseResult();
            try
            {
                var model = await GetTongsById(tongsId);
                if (model.STATUS != 0)
                {
                    result.ResultCode = 106;
                    result.ResultMsg = "当前夹具不是待入库状态，无法做入库操作！";
                    return result;
                }

                string sql = "UPDATE MES_TONGS_INFO SET STORE_ID = :STORE_ID,STATUS=1,UPDATE_USER=:UPDATE_USER,UPDATE_DATE = SYSDATE WHERE ID=:ID";
                if ((await _dbConnection.ExecuteAsync(sql, new { STORE_ID = storeId, ID = tongsId, UPDATE_USER = user })) > 0)
                {
                    //插入操作记录信息
                    MesTongsOperationHistory data = new MesTongsOperationHistory
                    {
                        CREATE_USER = user,
                        OPERATION_TYPE = 1,
                        TONGS_ID = model.ID,
                        LAST_STATUS = 1,
                        PRE_STATUS = model.STATUS,
                        STORE_ID = storeId
                    };
                    await InsertOperationRecord(data);

                    result.ResultCode = 0;
                    result.ResultMsg = "入库成功！";
                }
                else
                {
                    result.ResultCode = 106;
                    result.ResultMsg = "入库失败，请刷新后重试！";
                }
                return result;
            }
            catch (Exception ex)
            {
                result.ResultCode = 106;
                result.ResultMsg = ex.Message;
                return result;
            }
        }

        /// <summary>
        /// 变更储位
        /// </summary>
        /// <param name="tongsId"></param>
        /// <param name="storeId"></param>
        /// <param name="user"></param>
        /// <returns></returns>
        public async Task<BaseResult> ChangeStore(decimal tongsId, decimal storeId, string user)
        {
            var result = new BaseResult();
            try
            {
                var model = await GetTongsById(tongsId);
                if (model.STATUS != 1)
                {
                    result.ResultCode = 106;
                    result.ResultMsg = "当前夹具不是存储状态，无法做变更储位操作！";
                    return result;
                }

                string sql = "UPDATE MES_TONGS_INFO SET STORE_ID = :STORE_ID,UPDATE_USER=:UPDATE_USER,UPDATE_DATE = SYSDATE WHERE ID=:ID";
                if ((await _dbConnection.ExecuteAsync(sql, new { STORE_ID = storeId, ID = tongsId, UPDATE_USER = user })) > 0)
                {
                    //插入操作记录信息
                    MesTongsOperationHistory data = new MesTongsOperationHistory
                    {
                        CREATE_USER = user,
                        OPERATION_TYPE = 3,
                        TONGS_ID = model.ID,
                        LAST_STATUS = model.STATUS,
                        PRE_STATUS = model.STATUS,
                        STORE_ID = storeId
                    };
                    await InsertOperationRecord(data);

                    result.ResultCode = 0;
                    result.ResultMsg = "变更储位成功！";
                }
                else
                {
                    result.ResultCode = 106;
                    result.ResultMsg = "变更储位失败，请刷新后重试！";
                }
                return result;
            }
            catch (Exception ex)
            {
                result.ResultCode = 106;
                result.ResultMsg = ex.Message;
                return result;
            }
        }

        /// <summary>
        /// 根据类型获取事项内容
        /// </summary>
        /// <param name="type">事项类型</param>
        /// <returns></returns>
        public async Task<List<MesTongsMaintainItemsListModel>> GetMaintainItemsData(int type, int tongsType)
        {
            string sql = "SELECT * FROM MES_TONGS_MAINTAIN_ITEMS WHERE ITEM_TYPE=:ITEM_TYPE AND (TONGS_TYPE=:TONGS_TYPE OR TONGS_TYPE=-1) AND ENABLED='Y'";
            var list = (await _dbConnection.QueryAsync<MesTongsMaintainItemsListModel>(sql, new { ITEM_TYPE = type, TONGS_TYPE = tongsType })).ToList();
            foreach (var item in list)
            {
                item.ACTIVE = "N";
            }
            return list;
        }

        /// <summary>
        /// 开始激活
        /// </summary>
        /// <param name="tongsId"></param>
        /// <param name="user"></param>
        /// <returns></returns>
        public async Task<BaseResult> BeginActive(decimal tongsId, string user)
        {
            var result = new BaseResult();
            result.ResultData = "0";

            try
            {
                var history = await GetMaintainHistoryByTongsId(tongsId, 1);
                //已经存在开始激活动作
                if (history != null)
                {
                    result.ResultCode = 0;
                    result.ResultMsg = "操作成功！";
                    result.ResultData = history.ID.ToString();
                    return result;
                }

                MesTongsMaintainHistory model = new MesTongsMaintainHistory();
                model.ID = await GetMaintainSEQ();
                model.TONGS_ID = tongsId;
                model.TYPE = 1;//激活事项
                model.CREATE_USER = user;

                //新增激活动作记录
                if (await InsertMaintainRecord(model))
                {
                    result.ResultCode = 0;
                    result.ResultMsg = "操作成功！";
                    result.ResultData = model.ID.ToString();
                }
                else
                {
                    result.ResultCode = 106;
                    result.ResultMsg = "操作失败，请刷新后重试！";
                }

                return result;
            }
            catch (Exception ex)
            {
                result.ResultCode = 106;
                result.ResultMsg = ex.Message;
                return result;
            }
        }

        /// <summary>
        /// 结束激活
        /// </summary>
        /// <param name="tongsId"></param>
        /// <param name="user"></param>
        /// <returns></returns>
        public async Task<BaseResult> EndActive(MesTongsMaintainHistory model)
        {
            var result = new BaseResult();
            string editTongsSql = @"UPDATE MES_TONGS_INFO SET ACTIVE=:ACTIVE,UPDATE_USER=:UPDATE_USER,UPDATE_DATE=SYSDATE WHERE ID=:ID";
            string editHistorySql = @"UPDATE MES_TONGS_MAINTAIN_HISTORY SET OPERATION_ID=:OPERATION_ID,MAINTAIN_USER=:MAINTAIN_USER,
						END_DATE=SYSDATE,STATUS=:STATUS,REMARK=:REMARK WHERE ID=:ID";
            string addItemsSql = @"INSERT INTO MES_TONGS_MAINTAIN_DETAIL VALUES(MES_TONGS_MAINTAIN_DTL_SEQ.NEXTVAL,
									:MST_ID,:ITEM_ID,:ITEM_STATUS,:REMARK)";

            ConnectionFactory.OpenConnection(_dbConnection);
            using (var tran = _dbConnection.BeginTransaction())
            {
                try
                {
                    //获取一个新的操作记录ID
                    model.OPERATION_ID = await GetOperationSEQ();

                    //获取到夹具信息
                    var tongs = await GetTongsById(model.TONGS_ID);
                    if (tongs == null)
                    {
                        tran.Rollback();
                        result.ResultCode = 106;
                        result.ResultMsg = "找不到对应夹具，请刷新后重试！";
                        return result;
                    }

                    //插入操作记录信息
                    MesTongsOperationHistory data = new MesTongsOperationHistory
                    {
                        ID = model.OPERATION_ID,
                        CREATE_USER = model.MAINTAIN_USER,
                        OPERATION_TYPE = 2,
                        TONGS_ID = model.TONGS_ID,
                        STORE_ID = tongs.STORE_ID,
                        LAST_STATUS = tongs.STATUS,
                        PRE_STATUS = tongs.STATUS
                    };
                    await InsertOperationRecord(data);

                    //更新夹具状态
                    await _dbConnection.ExecuteAsync(editTongsSql, new { UPDATE_USER = model.MAINTAIN_USER, ID = model.TONGS_ID, ACTIVE = (model.STATUS == 1 ? "Y" : "N") });

                    //更新保养记录数据
                    await _dbConnection.ExecuteAsync(editHistorySql, model);

                    //插入保养明细数据
                    foreach (var item in model.DetailList)
                    {
                        item.MST_ID = model.ID;
                        await _dbConnection.ExecuteAsync(addItemsSql, item);
                    }

                    tran.Commit();
                    result.ResultCode = 0;
                    result.ResultMsg = "操作成功！";
                    return result;
                }
                catch (Exception ex)
                {
                    tran.Rollback();
                    result.ResultCode = 106;
                    result.ResultMsg = ex.Message;
                    return result;
                }
                finally
                {
                    if (_dbConnection.State != System.Data.ConnectionState.Closed)
                    {
                        _dbConnection.Close();
                    }
                }
            }

        }

        /// <summary>
        /// 领用夹具
        /// </summary>
        /// <param name="tongsId"></param>
        /// <param name="user"></param>
        /// <returns></returns>
        public async Task<BaseResult> BorrowTongs(decimal tongsId, string user)
        {
            var result = new BaseResult();
            string editTongsSql = @"UPDATE MES_TONGS_INFO SET STATUS = 2,STORE_ID = NULL,UPDATE_USER=:UPDATE_USER,UPDATE_DATE = SYSDATE WHERE ID=:ID";

            ConnectionFactory.OpenConnection(_dbConnection);
            using (var tran = _dbConnection.BeginTransaction())
            {
                try
                {
                    //获取到夹具信息
                    var tongs = await GetTongsById(tongsId);
                    if (tongs == null)
                    {
                        tran.Rollback();
                        result.ResultCode = 106;
                        result.ResultMsg = "找不到对应夹具，请刷新后重试！";
                        return result;
                    }

                    //插入操作记录信息
                    MesTongsOperationHistory data = new MesTongsOperationHistory
                    {
                        CREATE_USER = user,
                        OPERATION_TYPE = 4,
                        TONGS_ID = tongsId,
                        LAST_STATUS = 2,
                        PRE_STATUS = tongs.STATUS
                    };
                    await InsertOperationRecord(data);

                    //更新夹具状态
                    await _dbConnection.ExecuteAsync(editTongsSql, new { UPDATE_USER = user, ID = tongsId });

                    tran.Commit();
                    result.ResultCode = 0;
                    result.ResultMsg = "操作成功！";
                    return result;
                }
                catch (Exception ex)
                {
                    tran.Rollback();
                    result.ResultCode = 106;
                    result.ResultMsg = ex.Message;
                    return result;
                }
                finally
                {
                    if (_dbConnection.State != System.Data.ConnectionState.Closed)
                    {
                        _dbConnection.Close();
                    }
                }
            }
        }

        /// <summary>
        /// 永久借出夹具
        /// </summary>
        /// <param name="tongsId"></param>
        /// <param name="user"></param>
        /// <returns></returns>
        public async Task<BaseResult> PermanentLendTongs(decimal tongsId, string user, string remark)
        {
            var result = new BaseResult();
            string editTongsSql = @"UPDATE MES_TONGS_INFO SET STATUS = 7,STORE_ID = NULL,UPDATE_USER=:UPDATE_USER,UPDATE_DATE = SYSDATE WHERE ID=:ID";

            ConnectionFactory.OpenConnection(_dbConnection);
            using (var tran = _dbConnection.BeginTransaction())
            {
                try
                {
                    //获取到夹具信息
                    var tongs = await GetTongsById(tongsId);
                    if (tongs == null)
                    {
                        tran.Rollback();
                        result.ResultCode = 106;
                        result.ResultMsg = "找不到对应夹具，请刷新后重试！";
                        return result;
                    }

                    //插入操作记录信息
                    MesTongsOperationHistory data = new MesTongsOperationHistory
                    {
                        CREATE_USER = user,
                        OPERATION_TYPE = 9,
                        TONGS_ID = tongsId,
                        LAST_STATUS = 7,
                        PRE_STATUS = tongs.STATUS,
                        REMARK = remark
                    };
                    await InsertOperationRecord(data);

                    //更新夹具状态
                    await _dbConnection.ExecuteAsync(editTongsSql, new { UPDATE_USER = user, ID = tongsId });

                    tran.Commit();
                    result.ResultCode = 0;
                    result.ResultMsg = "操作成功！";
                    return result;
                }
                catch (Exception ex)
                {
                    tran.Rollback();
                    result.ResultCode = 106;
                    result.ResultMsg = ex.Message;
                    return result;
                }
                finally
                {
                    if (_dbConnection.State != System.Data.ConnectionState.Closed)
                    {
                        _dbConnection.Close();
                    }
                }
            }
        }

        /// <summary>
        /// 开始保养
        /// </summary>
        /// <param name="tongsId"></param>
        /// <param name="user"></param>
        /// <returns></returns>
        public async Task<BaseResult> BeginMaintain(decimal tongsId, string user)
        {
            var result = new BaseResult();
            result.ResultData = "0";

            string editTongsSql = @"UPDATE MES_TONGS_INFO SET STATUS=4,STORE_ID=NULL,UPDATE_USER=:UPDATE_USER,UPDATE_DATE=SYSDATE WHERE ID=:TONGS_ID";

            ConnectionFactory.OpenConnection(_dbConnection);
            using (var tran = _dbConnection.BeginTransaction())
            {
                try
                {
                    var history = await GetMaintainHistoryByTongsId(tongsId, 0);
                    //已经存在开始激活动作
                    if (history != null)
                    {
                        tran.Rollback();
                        result.ResultCode = 0;
                        result.ResultMsg = "操作成功！";
                        result.ResultData = history.ID.ToString();
                        return result;
                    }

                    //获取到夹具信息
                    var tongs = await GetTongsById(tongsId);
                    if (tongs == null)
                    {
                        tran.Rollback();
                        result.ResultCode = 106;
                        result.ResultMsg = "找不到对应夹具，请刷新后重试！";
                        return result;
                    }

                    //插入操作记录信息
                    MesTongsOperationHistory data = new MesTongsOperationHistory
                    {
                        CREATE_USER = user,
                        OPERATION_TYPE = 6,
                        TONGS_ID = tongsId,
                        STORE_ID = tongs.STORE_ID,
                        LAST_STATUS = 4,
                        PRE_STATUS = tongs.STATUS
                    };
                    await InsertOperationRecord(data);

                    //插入保养动作记录
                    MesTongsMaintainHistory model = new MesTongsMaintainHistory();
                    model.ID = await GetMaintainSEQ();
                    model.TONGS_ID = tongsId;
                    model.TYPE = 0;//保养事项
                    model.CREATE_USER = user;
                    if (await InsertMaintainRecord(model))
                    {
                        result.ResultCode = 0;
                        result.ResultMsg = "操作成功！";
                        result.ResultData = model.ID.ToString();
                    }
                    else
                    {
                        tran.Rollback();
                        result.ResultCode = 106;
                        result.ResultMsg = "操作失败，请刷新后重试！";
                        return result;
                    }

                    //更新夹具状态
                    await _dbConnection.ExecuteAsync(editTongsSql, new { UPDATE_USER = user, TONGS_ID = tongsId });

                    tran.Commit();
                    return result;
                }
                catch (Exception ex)
                {
                    tran.Rollback();
                    result.ResultCode = 106;
                    result.ResultMsg = ex.Message;
                    return result;
                }
                finally
                {
                    if (_dbConnection.State != System.Data.ConnectionState.Closed)
                    {
                        _dbConnection.Close();
                    }
                }
            }
        }

        /// <summary>
        /// 结束保养
        /// </summary>
        /// <param name="tongsId"></param>
        /// <param name="user"></param>
        /// <returns></returns>
        public async Task<BaseResult> EndMaintain(MesTongsMaintainHistory model)
        {
            var result = new BaseResult();
            string editTongsSql = @"UPDATE MES_TONGS_INFO SET STATUS=:STATUS,UPDATE_USER=:UPDATE_USER,UPDATE_DATE=SYSDATE WHERE ID=:ID";
            string editHistorySql = @"UPDATE MES_TONGS_MAINTAIN_HISTORY SET OPERATION_ID=:OPERATION_ID,MAINTAIN_USER=:MAINTAIN_USER,
						END_DATE=SYSDATE,STATUS=:STATUS,REMARK=:REMARK WHERE ID=:ID";
            string addItemsSql = @"INSERT INTO MES_TONGS_MAINTAIN_DETAIL VALUES(MES_TONGS_MAINTAIN_DTL_SEQ.NEXTVAL,
									:MST_ID,:ITEM_ID,:ITEM_STATUS,:REMARK)";

            string addRepairSql = @"INSERT INTO MES_TONGS_REPAIR(ID,TONGS_ID,BEGIN_TIME,CREATE_USER,REPAIRER,REMARK) VALUES(MES_TONGS_REPAIR_SEQ.NEXTVAL,
								:TONGS_ID,SYSDATE,:CREATE_USER,:REPAIRER,:REMARK)";

            ConnectionFactory.OpenConnection(_dbConnection);
            using (var tran = _dbConnection.BeginTransaction())
            {
                try
                {
                    //获取一个新的操作记录ID
                    model.OPERATION_ID = await GetOperationSEQ();

                    //获取到夹具信息
                    var tongs = await GetTongsById(model.TONGS_ID);
                    if (tongs == null)
                    {
                        tran.Rollback();
                        result.ResultCode = 106;
                        result.ResultMsg = "找不到对应夹具，请刷新后重试！";
                        return result;
                    }

                    //插入操作记录信息
                    MesTongsOperationHistory data = new MesTongsOperationHistory
                    {
                        ID = model.OPERATION_ID,
                        CREATE_USER = model.MAINTAIN_USER,
                        OPERATION_TYPE = 7,
                        TONGS_ID = model.TONGS_ID,
                        STORE_ID = tongs.STORE_ID,
                        LAST_STATUS = model.STATUS == 1 ? 0 : 5,
                        PRE_STATUS = tongs.STATUS
                    };
                    await InsertOperationRecord(data);

                    //更新夹具状态
                    await _dbConnection.ExecuteAsync(editTongsSql, new { UPDATE_USER = model.MAINTAIN_USER, ID = model.TONGS_ID, STATUS = (model.STATUS == 1 ? 0 : 5) });

                    //更新保养记录数据
                    await _dbConnection.ExecuteAsync(editHistorySql, model);

                    //插入保养明细数据
                    foreach (var item in model.DetailList)
                    {
                        item.MST_ID = model.ID;
                        await _dbConnection.ExecuteAsync(addItemsSql, new
                        {
                            MST_ID = item.MST_ID,
                            ITEM_ID = item.ID,
                            ITEM_STATUS = item.ITEM_STATUS,
                            REMARK = item.REMARK
                        });
                    }

                    //状态为异常时，插入维修记录
                    if (model.STATUS == 2)
                    {
                        MesTongsRepair repair = new MesTongsRepair
                        {
                            TONGS_ID = model.TONGS_ID,
                            CREATE_USER = model.MAINTAIN_USER,
                            REMARK = "保养异常，进入维修",
                            REPAIRER = model.MAINTAIN_USER
                        };
                        await _dbConnection.ExecuteAsync(addRepairSql, repair);
                    }

                    tran.Commit();
                    result.ResultCode = 0;
                    result.ResultMsg = "操作成功！";
                    return result;
                }
                catch (Exception ex)
                {
                    tran.Rollback();
                    result.ResultCode = 106;
                    result.ResultMsg = ex.Message;
                    return result;
                }
                finally
                {
                    if (_dbConnection.State != System.Data.ConnectionState.Closed)
                    {
                        _dbConnection.Close();
                    }
                }
            }
        }




        /// <summary>
        /// 维修夹具
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public async Task<BaseResult> RepairTongs(MesTongsRepair model)
        {
            var result = new BaseResult();
            string editTongsSql = @"UPDATE MES_TONGS_INFO SET STATUS=:STATUS,UPDATE_USER=:UPDATE_USER,UPDATE_DATE=SYSDATE WHERE ID=:ID";
            string editRepairSql = @"UPDATE MES_TONGS_REPAIR SET OPERATION_ID=:OPERATION_ID,REPAIR_RESULT=:REPAIR_RESULT,REPAIRER=:REPAIRER,
								END_TIME=SYSDATE,REMARK=:REMARK WHERE ID=:ID";

            ConnectionFactory.OpenConnection(_dbConnection);
            using (var tran = _dbConnection.BeginTransaction())
            {
                try
                {
                    //获取一个新的操作记录ID
                    decimal operationId = await GetOperationSEQ();

                    //获取到夹具信息
                    var tongs = await GetTongsById(model.TONGS_ID);
                    if (tongs == null)
                    {
                        tran.Rollback();
                        result.ResultCode = 106;
                        result.ResultMsg = "找不到对应夹具，请刷新后重试！";
                        return result;
                    }

                    //插入操作记录信息
                    MesTongsOperationHistory data = new MesTongsOperationHistory
                    {
                        ID = operationId,
                        CREATE_USER = model.REPAIRER,
                        OPERATION_TYPE = 8,
                        TONGS_ID = model.TONGS_ID,
                        STORE_ID = tongs.STORE_ID,
                        LAST_STATUS = model.REPAIR_RESULT == 1 ? 0 : 6,
                        PRE_STATUS = tongs.STATUS
                    };
                    await InsertOperationRecord(data);

                    //更新夹具状态
                    await _dbConnection.ExecuteAsync(editTongsSql, new { UPDATE_USER = model.REPAIRER, ID = model.TONGS_ID, STATUS = (model.REPAIR_RESULT == 1 ? 0 : 6) });

                    //更新维修记录
                    model.OPERATION_ID = operationId;
                    await _dbConnection.ExecuteAsync(editRepairSql, model);

                    tran.Commit();
                    result.ResultCode = 0;
                    result.ResultMsg = "操作成功！";
                    return result;
                }
                catch (Exception ex)
                {
                    tran.Rollback();
                    result.ResultCode = 106;
                    result.ResultMsg = ex.Message;
                    return result;
                }
                finally
                {
                    if (_dbConnection.State != System.Data.ConnectionState.Closed)
                    {
                        _dbConnection.Close();
                    }
                }
            }
        }
        #endregion

        #region 校验方法
        /// <summary>
        /// 判断当前夹具编码是否存在
        /// </summary>
        /// <param name="code">夹具编码</param>
        /// <param name="id">排除的ID</param>
        /// <returns></returns>
        public async Task<bool> IsExistByCode(string code, decimal id = 0)
        {
            if (string.IsNullOrEmpty(code))
                return false;

            string sql = @"SELECT COUNT(*) FROM MES_TONGS_INFO WHERE CODE=:CODE";
            if (id != 0)
                sql += " AND ID != :ID";

            return (await _dbConnection.ExecuteScalarAsync<int>(sql, new { ID = id, CODE = code })) > 0;
        }

        /// <summary>
        /// 获取夹具已经是维修状态但还未做维修动作的维修数据
        /// </summary>
        /// <param name="tongsId"></param>
        /// <returns></returns>
        public async Task<MesTongsRepair> GetRepairByTongsId(decimal tongsId)
        {
            string sql = "SELECT * FROM MES_TONGS_REPAIR WHERE TONGS_ID=:TONGS_ID AND REPAIR_RESULT IS NULL AND BEGIN_TIME IS NOT NULL AND END_TIME IS NULL";
            return (await _dbConnection.QueryAsync<MesTongsRepair>(sql, new { TONGS_ID = tongsId })).FirstOrDefault();
        }

        /// <summary>
        /// 判断夹具是否已经做过开始激活的动作且还未结束激活
        /// </summary>
        /// <param name="tongsId"></param>
        /// <returns></returns>
        private async Task<MesTongsMaintainHistory> GetMaintainHistoryByTongsId(decimal tongsId, decimal type)
        {
            string sql = @"SELECT * FROM MES_TONGS_MAINTAIN_HISTORY WHERE TONGS_ID=:TONGS_ID AND TYPE=:TYPE AND START_DATE IS NOT NULL AND END_DATE IS NULL ORDER BY ID DESC";
            return (await _dbConnection.QueryAsync<MesTongsMaintainHistory>(sql, new { TONGS_ID = tongsId, TYPE = type })).FirstOrDefault();
        }
        #endregion

        #region 对内方法
        /// <summary>
        /// 获取WHERE条件
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        private string GetWhereStr(MesTongsInfoRequestModel model)
        {
            StringBuilder whereStr = new StringBuilder();
            whereStr.Append("  WHERE MST.ENABLED = 'Y' ");

            if (!string.IsNullOrEmpty(model.CODE) && model.IS_LIKE == GlobalVariables.NotLike)
                whereStr.Append(" and MST.CODE=:CODE ");

            if (!string.IsNullOrEmpty(model.CODE) && model.IS_LIKE == GlobalVariables.IsLike)
                whereStr.Append(" and (INSTR(MST.CODE,:CODE)>0) ");

            if (model.TONGS_TYPE != null)
                whereStr.Append(" and MST.TONGS_TYPE=:TONGS_TYPE ");

            if (model.DEPARTMENT != null)
                whereStr.Append(" and MST.DEPARTMENT=:DEPARTMENT ");

            if (model.SOURCES != null)
                whereStr.Append(" and MST.SOURCES=:SOURCES ");

            if (model.STATUS != null)
                whereStr.Append(" and MST.STATUS=:STATUS ");

            if (!string.IsNullOrEmpty(model.ACTIVE))
                whereStr.Append(" and MST.ACTIVE=:ACTIVE ");

            return whereStr.ToString();
        }

        /// <summary>
        /// 获取操作记录表序列
        /// </summary>
        /// <returns></returns>
        private async Task<decimal> GetOperationSEQ()
        {
            string sql = "SELECT MES_TONGS_OPERATION_SEQ.NEXTVAL MY_SEQ FROM DUAL";
            var result = await _dbConnection.ExecuteScalarAsync(sql);
            return (decimal)result;
        }

        /// <summary>
        /// 插入操作记录
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        private async Task<bool> InsertOperationRecord(MesTongsOperationHistory model)
        {
            try
            {
                if (model.ID == 0)
                    model.ID = await GetOperationSEQ();

                string sql = @"INSERT INTO MES_TONGS_OPERATION_HISTORY VALUES(:ID,
						:TONGS_ID,:OPERATION_TYPE,:PRE_STATUS,:LAST_STATUS,:STORE_ID,:REMARK,:CREATE_USER,SYSDATE)";

                return (await _dbConnection.ExecuteAsync(sql, model)) > 0;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        /// <summary>
        /// 插入保养记录
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        private async Task<bool> InsertMaintainRecord(MesTongsMaintainHistory model)
        {
            if (model.ID == 0)
                model.ID = await GetMaintainSEQ();

            string sql = @"INSERT INTO MES_TONGS_MAINTAIN_HISTORY(ID,TONGS_ID,OPERATION_ID,TYPE,START_DATE,CREATE_USER,CREATE_DATE) 
						VALUES(:ID,:TONGS_ID,0,:TYPE,SYSDATE,:CREATE_USER,SYSDATE)";

            return (await _dbConnection.ExecuteAsync(sql, model)) > 0;
        }

        /// <summary>
        /// 获取保养记录表序列
        /// </summary>
        /// <returns></returns>
        private async Task<decimal> GetMaintainSEQ()
        {
            string sql = "SELECT MES_TONGS_MAINTAIN_SEQ.NEXTVAL MY_SEQ FROM DUAL";
            var result = await _dbConnection.ExecuteScalarAsync(sql);
            return (decimal)result;
        }

        public async Task<List<TongsStoreInfoModel>> GetTongsInfoByWoNo(string WoNo, string PartNo)
        {
            List<TongsStoreInfoModel> list = new List<TongsStoreInfoModel>();
            string str = "";
            string lookup_type = "MES_TONGS_TYPE";

            if (!PartNo.IsNullOrWhiteSpace())
            {
                str += " AND TP.PART_NO = '" + PartNo + "'";

            }
            if (!WoNo.IsNullOrWhiteSpace())
            {
                str += " AND W.WO_NO =  '" + WoNo + "'";
            }
            string sql = @"SELECT SC.NAME as StoreName,TONGSTYPE.Meaning as TongsType,TI.Code FROM MES_TONGS_INFO TI 
INNER JOIN ( SELECT TP.TONGS_ID,TP.PART_NO,W.WO_NO FROM MES_TONGS_PART TP LEFT JOIN SMT_WO W ON TP.PART_NO = W.PART_NO
WHERE TP.ENABLED = 'Y'  " + str + @" )TPW ON TI.ID = TPW.TONGS_ID
LEFT JOIN MES_TONGS_STORE_CONFIG SC ON TI.STORE_ID = SC.CODE
LEFT JOIN MES_TONGS_APPLY MST ON TI.APPLY_ID = MST.ID
LEFT JOIN SFCS_PARAMETERS TONGSTYPE ON TONGSTYPE.LOOKUP_CODE=MST.TONGS_TYPE
WHERE TONGSTYPE.LOOKUP_TYPE=:LOOKUP_TYPE  AND TONGSTYPE.ENABLED = 'Y'AND TI.ENABLED = 'Y' AND LENGTH(TI.STORE_ID) > 0 ";
            if (!str.IsNullOrWhiteSpace())
            {
                list = (await _dbConnection.QueryAsync<TongsStoreInfoModel>(sql, new { LOOKUP_TYPE = lookup_type })).ToList();
            }
            return list;
        }
        #endregion


        #region 工装盘点
        /// <summary>
        /// 保存PDA飞达盘点数据
        /// </summary>
        /// <param name="model"></param>
        /// <param name="Tongs"></param>
        /// <param name="head"></param>
        /// <returns></returns>
        public async Task<int> SavePDATongsCheckData(SaveTongsCheckDataRequestModel model, MesTongsInfo Tongs, MesTongsKeepHead head, MesTongsKeepDetail detail)
        {
            int result = 0, headId = 0;
            ConnectionFactory.OpenConnection(_dbConnection);
            using (var tran = _dbConnection.BeginTransaction())
            {
                try
                {
                    if (model.CHECK_CODE.IsNullOrEmpty() && head.IsNullOrWhiteSpace() && detail.IsNullOrWhiteSpace())
                    {
                        headId = QueryEx<int>("SELECT MES_TONGS_KEEP_HEAD_SEQ.NEXTVAL MY_SEQ FROM DUAL").FirstOrDefault();

                        //將序列轉成36進制表示
                        string resultStr = Core.Utilities.RadixConvertPublic.RadixConvert(headId.ToString(), ViewModels.GlobalVariables.DecRadix, ViewModels.GlobalVariables.Base36Redix);

                        //六位表示
                        string ReleasedSequence = resultStr.PadLeft(6, '0');
                        string yymmdd = QueryEx<string>("SELECT TO_CHAR(SYSDATE,'YYMMDD') YYMMDD FROM DUAL ").FirstOrDefault();
                        String TongsNo = "GZDJ" + yymmdd + ReleasedSequence;//飞达点检编号

                        ////点检月/次
                        //int count = QueryEx<int>("SELECT COUNT(1) FROM SFCS_Tongs_KEEP_HEAD WHERE TO_CHAR(CHECK_START_TIME,'yyyy-MM') = :CHECK_TIME ", new
                        //{
                        //    CHECK_TIME = DateTime.Now.ToString("yyyy-MM")
                        //}).FirstOrDefault();
                        //count += 1;

                        String insertHeadSql = @"INSERT INTO MES_TONGS_KEEP_HEAD(ID, CHECK_CODE, CHECK_STATUS, CHECK_COUNT, CHECK_USER, CHECK_START_TIME, CHECK_REMARKS, ORGANIZE_ID) VALUES(:ID, :CHECK_CODE, 0, (SELECT COUNT(0) + 1 AS C  FROM MES_TONGS_KEEP_HEAD WHERE TO_CHAR(CHECK_START_TIME,'yyyy-MM') = TO_CHAR(SYSDATE,'yyyy-MM')), :CHECK_USER, SYSDATE, :CHECK_REMARKS,:ORGANIZE_ID)";
                        result = await _dbConnection.ExecuteAsync(insertHeadSql, new
                        {
                            ID = headId,
                            CHECK_CODE = TongsNo,
                            CHECK_USER = model.CHECK_USER,
                            CHECK_REMARKS = model.CHECK_REMARKS,
                            ORGANIZE_ID = Tongs.ORGANIZE_ID
                        }, tran);
                        if (result <= 0) { throw new Exception("DATA_ERROR"); }

                        //String insertDetailSql = @"INSERT INTO MES_TONGS_KEEP_DETAIL (ID, KEEP_HEAD_ID, Tongs_TYPE, Tongs_SIZE, Tongs_TYPE_TOTAL, CHECK_QTY) VALUES (:ID, :KEEP_HEAD_ID, :Tongs_TYPE, :Tongs_SIZE,:Tongs_TYPE_TOTAL, :CHECK_QTY)";
                        //foreach (var item in fList)
                        //{
                        //    if (item.Tongs_TYPE == Tongs.FTYPE && item.Tongs_SIZE == Tongs.FSIZE)
                        //    {
                        //        detailId = QueryEx<int>("SELECT SFCS_Tongs_KEEP_DETAIL_SEQ.NEXTVAL MY_SEQ FROM DUAL").FirstOrDefault();
                        //        result = await _dbConnection.ExecuteAsync(insertDetailSql, new
                        //        {
                        //            ID = detailId,
                        //            KEEP_HEAD_ID = headId,
                        //            Tongs_TYPE = Tongs.FTYPE,
                        //            Tongs_SIZE = Tongs.FSIZE,
                        //            Tongs_TYPE_TOTAL = item.Tongs_QTY,
                        //            CHECK_QTY = 1
                        //        }, tran);
                        //    }
                        //    else
                        //    {
                        //        int newid = QueryEx<int>("SELECT SFCS_Tongs_KEEP_DETAIL_SEQ.NEXTVAL MY_SEQ FROM DUAL").FirstOrDefault();
                        //        result = await _dbConnection.ExecuteAsync(insertDetailSql, new
                        //        {
                        //            ID = newid,
                        //            KEEP_HEAD_ID = headId,
                        //            Tongs_TYPE = item.Tongs_TYPE,
                        //            Tongs_SIZE = item.Tongs_SIZE,
                        //            Tongs_TYPE_TOTAL = item.Tongs_QTY,
                        //            CHECK_QTY = 0
                        //        }, tran);
                        //    }
                        //    if (result <= 0) { throw new Exception("DATA_ERROR"); }
                        //}
                        ////获取同类型和尺寸并可用状态的飞达数量
                        //int Tongs_qty = QueryEx<int>("SELECT COUNT(1) FROM SMT_Tongs WHERE STATUS NOT IN (6,7) AND FTYPE = :FTYPE AND FSIZE = :FSIZE AND ORGANIZE_ID = :ORGANIZE_ID ", new
                        //{
                        //    FTYPE = Tongs.FTYPE,
                        //    FSIZE = Tongs.FSIZE,
                        //    ORGANIZE_ID = Tongs.ORGANIZE_ID
                        //}).FirstOrDefault();
                        //if (Tongs_qty <= 0) { throw new Exception("Tongs_CHECK_QTY_ERROR"); }

                        //detailId = QueryEx<int>("SELECT SFCS_Tongs_KEEP_DETAIL_SEQ.NEXTVAL MY_SEQ FROM DUAL").FirstOrDefault();
                        //result = await _dbConnection.ExecuteAsync(insertDetailSql, new
                        //{
                        //    ID = detailId,
                        //    KEEP_HEAD_ID = headId,
                        //    Tongs_TYPE = Tongs.FTYPE,
                        //    Tongs_SIZE = Tongs.FSIZE,
                        //    Tongs_TYPE_TOTAL = Tongs_qty
                        //}, tran);
                        //if (result <= 0) { throw new Exception("DATA_ERROR"); }
                    }

                    int contentId = QueryEx<int>("SELECT MES_TONGS_KEEP_DETAIL_SEQ.NEXTVAL MY_SEQ FROM DUAL").FirstOrDefault();
                    String insertContentSql = @"INSERT INTO MES_TONGS_KEEP_DETAIL (ID, KEEP_HEAD_ID, TONGS_ID, TONGS_STATUS, CHECK_USER, CHECK_TIME, CHECK_REMARKS) VALUES (:ID, :KEEP_HEAD_ID, :TONGS_ID, :TONGS_STATUS, :CHECK_USER,SYSDATE,:CHECK_REMARKS)";
                    var keepheadid = headId == 0 ? head.ID : headId;
                    result = await _dbConnection.ExecuteAsync(insertContentSql, new
                    {
                        ID = contentId,
                        KEEP_HEAD_ID = keepheadid,
                        TONGS_ID = Tongs.ID,
                        TONGS_STATUS = 1,
                        CHECK_REMARKS = model.CHECK_REMARKS,
                        CHECK_USER = model.CHECK_USER
                    }, tran);
                    if (result <= 0) throw new Exception("DATA_ERROR");

                    tran.Commit();
                }
                catch (Exception ex)
                {
                    headId = 0;
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
            return headId;
        }

        /// <summary>
        /// 获取盘点数据
        /// </summary>
        /// <param name="headid">KEEP_HEAD_ID</param>
        /// <returns></returns>
        public async Task<List<TongsInfoInnerKeepDetail>> GetPDATongsCheckDataByHeadID(string organizeId, Decimal headid = 0)
        {
            List<TongsInfoInnerKeepDetail> result = new List<TongsInfoInnerKeepDetail>();
            try
            {
                string selectTongsCheckData = @"SELECT
											    	INFO.ID INFO_ID,
											    	INFO.CODE,
											    	SPS1.MEANING TONGS_TYPE,
											    	STORE.NAME STORE_NAME,
											    	{0}
											    FROM
											    	MES_TONGS_INFO INFO,
											    	MES_TONGS_STORE_CONFIG STORE,
                                                    MES_TONGS_KEEP_DETAIL KDETAIL,
											    	( SELECT * FROM SFCS_PARAMETERS WHERE LOOKUP_TYPE = 'MES_TONGS_TYPE' AND ENABLED = 'Y' ) SPS1 
											    WHERE
											    	INFO.STORE_ID = STORE.id ( + ) 
											    	AND SPS1.LOOKUP_CODE ( + ) = INFO.TONGS_TYPE 
                                                    AND KDETAIL.TONGS_ID(+)=INFO.ID
											    	AND INFO.STATUS != 6 
											    	AND INFO.ORGANIZE_ID IN ({1})";
                if (headid == 0)
                {
                    string sql = String.Format(selectTongsCheckData, @"'' CHECK_REMARKS,'' CHECK_TIME,'' CHECK_USER,0 KDID,'未盘点' KDETAIL_STATUS ", organizeId);
                    result = (await _dbConnection.QueryAsync<TongsInfoInnerKeepDetail>(sql))?.ToList();
                }
                else
                {
                    selectTongsCheckData += " AND KDETAIL.KEEP_HEAD_ID(+)=:ID ";
                    string sql = String.Format(selectTongsCheckData, @"KDETAIL.CHECK_REMARKS,TO_CHAR(KDETAIL.CHECK_TIME,'yyyy-MM-dd HH24:mi:ss') CHECK_TIME,KDETAIL.CHECK_USER,NVL(KDETAIL.KEEP_HEAD_ID,:ID) KDID,DECODE(KDETAIL.TONGS_STATUS,1,'已盘点','未盘点') KDETAIL_STATUS", organizeId);
                    result = (await _dbConnection.QueryAsync<TongsInfoInnerKeepDetail>(sql, new { ID = headid }))?.ToList();
                }
            }
            catch (Exception ex)
            {
                result = null;
                throw ex;
            }
            return result;
        }

        /// <summary>
        /// 删除PDA工装盘点数据记录
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<bool> DeletePDATongsCheckData(Decimal id)
        {
            int result = 0;
            ConnectionFactory.OpenConnection(_dbConnection);
            using (var tran = _dbConnection.BeginTransaction())
            {
                try
                {
                    //工装点检记录详细表
                    string deleteSql = @"DELETE FROM MES_TONGS_KEEP_DETAIL WHERE KEEP_HEAD_ID = :ID ";
                    result = await _dbConnection.ExecuteAsync(deleteSql, new { ID = id }, tran);

                    //工装点检记录主表
                    deleteSql = @"DELETE FROM MES_TONGS_KEEP_HEAD WHERE ID = :ID ";
                    result = await _dbConnection.ExecuteAsync(deleteSql, new { ID = id }, tran);
                    if (result <= 0) throw new Exception("DATA_ERROR");

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
            return result > 0 ? true : false;
        }

        /// <summary>
        /// 确认PDA飞达盘点数据
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public async Task<bool> ConfirmPDATongsCheckData(AuditTongsCheckDataRequestModel model)
        {
            int result = 0;
            ConnectionFactory.OpenConnection(_dbConnection);
            using (var tran = _dbConnection.BeginTransaction())
            {
                try
                {
                    if (model.STATUS == 1)
                    {
                        String updateHeadSql = @"UPDATE MES_TONGS_KEEP_HEAD SET CHECK_STATUS = '1',CHECK_END_TIME = SYSDATE WHERE ID = :ID ";
                        result = await _dbConnection.ExecuteAsync(updateHeadSql, new
                        {
                            ID = model.ID
                        }, tran);
                        if (result <= 0) throw new Exception("DATA_ERROR");
                    }
                    if (model.STATUS == 2)
                    {
                        String updateHeadSql = @"UPDATE MES_TONGS_KEEP_HEAD SET CHECK_STATUS = '2',AUDIT_USER = :AUDIT_USER,AUDIT_REMARKS = :AUDIT_REMARKS,AUDIT_TIME = SYSDATE WHERE ID = :ID ";
                        result = await _dbConnection.ExecuteAsync(updateHeadSql, new
                        {
                            AUDIT_USER = model.AUDIT_USER,
                            AUDIT_REMARKS = model.AUDIT_REMARKS,
                            ID = model.ID
                        }, tran);
                        if (result <= 0) throw new Exception("DATA_ERROR");
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
            return result > 0 ? true : false;
        }

        public async Task<List<TongsCheckListModel>> LoadPDATongsCheckList(TongsCheckRequestModel model, string organize_id)
        {
            TongsCheckModel fModel = new TongsCheckModel();
            int page = 0, limit = 0;
            page = model.Page * model.Limit - model.Limit + 1;
            limit = model.Page * model.Limit;
            fModel.Page = page;
            fModel.Limit = limit;

            String orderBy = " ORDER BY CHECK_START_TIME DESC ";
            String whereStr = "";

            if (!model.START_TIME.IsNullOrEmpty())
            {
                fModel.START_TIME = Convert.ToDateTime(model.START_TIME.Trim() + " 00:00:00");
                whereStr += " AND CHECK_START_TIME >= :START_TIME ";

                if (!model.END_TIME.IsNullOrEmpty())
                {
                    fModel.END_TIME = Convert.ToDateTime(model.END_TIME.Trim() + " 23:59:59");
                    whereStr += " AND CHECK_START_TIME <= :END_TIME ";
                }
            }
            fModel.CHECK_STATUS = model.CHECK_STATUS;
            if (model.CHECK_STATUS >= 0)
            {
                whereStr += " AND CHECK_STATUS = :CHECK_STATUS ";
            }
            String sQuery = string.Format("SELECT * FROM (SELECT ROWNUM R, T.* FROM ( SELECT * FROM MES_TONGS_KEEP_HEAD WHERE 1=1 {0} {1}) T) WHERE R BETWEEN :Page AND :Limit", whereStr, orderBy);

            var resdata = await _dbConnection.QueryAsync<TongsCheckListModel>(sQuery, fModel);

            List<TongsCheckListModel> list = resdata.ToList();
            foreach (var item in list)
            {
                item.CHECK_TIME = item.CHECK_START_TIME.ToString("yyyy-MM-dd");
                item.TONGS_QTY = QueryEx<int>("SELECT SUM(KEEP_HEAD_ID) FROM MES_TONGS_KEEP_DETAIL WHERE KEEP_HEAD_ID = :KEEP_HEAD_ID ", new
                {
                    KEEP_HEAD_ID = item.ID
                }).FirstOrDefault();
                var TongsCheckList = await GetPDATongsCheckDataByHeadID(organize_id);
                item.CHECK_QTY = (TongsCheckList == null || TongsCheckList.Count <= 0) ? 0 : TongsCheckList.Count;

                item.CHECK_YEAR = item.CHECK_START_TIME.Year.ToString();
                item.CHECK_HEAD = item.CHECK_START_TIME.Month.ToString() + "月第" + item.CHECK_COUNT + "次盘点";
            }

            return list;
        }

        public async Task<int> LoadPDATongsCheckListCount(TongsCheckRequestModel model)
        {
            TongsCheckModel fModel = new TongsCheckModel();

            String whereStr = "";

            if (!model.START_TIME.IsNullOrEmpty())
            {
                fModel.START_TIME = Convert.ToDateTime(model.START_TIME.Trim() + " 00:00:00");
                whereStr += " AND CHECK_START_TIME >= :START_TIME ";

                if (!model.END_TIME.IsNullOrEmpty())
                {
                    fModel.END_TIME = Convert.ToDateTime(model.END_TIME.Trim() + " 23:59:59");
                    whereStr += " AND CHECK_START_TIME <= :END_TIME ";
                }
            }
            fModel.CHECK_STATUS = model.CHECK_STATUS;
            if (model.CHECK_STATUS >= 0)
            {
                whereStr += " AND CHECK_STATUS = :CHECK_STATUS ";
            }
            String sQuery = string.Format("SELECT COUNT(1) FROM MES_TONGS_KEEP_HEAD WHERE 1=1 {0}", whereStr);

            int count = await _dbConnection.ExecuteScalarAsync<int>(sQuery, fModel);
            return count;
        }

        #endregion

        #region 工装验证

        /// <summary>
        /// 保存PDA工装验证数据
        /// </summary>
        /// <param name="model"></param>
        /// <param name="Tongs"></param>
        /// <param name="head"></param>
        /// <returns></returns>
        public async Task<int> SavePDATongsValidationData(SaveTongsValidationDataRequestModel model, MesTongsInfo Tongs, MesTongsValidationHead head, MesTongsValidationDetail detail)
        {
            int result = 0, headId = 0;
            ConnectionFactory.OpenConnection(_dbConnection);
            using (var tran = _dbConnection.BeginTransaction())
            {
                try
                {
                    if (model.CHECK_CODE.IsNullOrEmpty() && head.IsNullOrWhiteSpace() && detail.IsNullOrWhiteSpace())
                    {
                        headId = QueryEx<int>("SELECT MES_TONGS_VALIDATION_HEAD_SEQ.NEXTVAL MY_SEQ FROM DUAL").FirstOrDefault();

                        //將序列轉成36進制表示
                        string resultStr = Core.Utilities.RadixConvertPublic.RadixConvert(headId.ToString(), ViewModels.GlobalVariables.DecRadix, ViewModels.GlobalVariables.Base36Redix);

                        //六位表示
                        string ReleasedSequence = resultStr.PadLeft(6, '0');
                        string yymmdd = QueryEx<string>("SELECT TO_CHAR(SYSDATE,'YYMMDD') YYMMDD FROM DUAL ").FirstOrDefault();
                        String TongsNo = "GZVL" + yymmdd + ReleasedSequence;//工装验证编号

                        ////点检月/次
                        //int count = QueryEx<int>("SELECT COUNT(1) FROM SFCS_Tongs_KEEP_HEAD WHERE TO_CHAR(CHECK_START_TIME,'yyyy-MM') = :CHECK_TIME ", new
                        //{
                        //    CHECK_TIME = DateTime.Now.ToString("yyyy-MM")
                        //}).FirstOrDefault();
                        //count += 1;

                        String insertHeadSql = @"INSERT INTO MES_TONGS_VALIDATION_HEAD(ID, CHECK_CODE, CHECK_STATUS, CHECK_COUNT, CHECK_USER, CHECK_START_TIME, CHECK_REMARKS, ORGANIZE_ID) VALUES(:ID, :CHECK_CODE, 0, (SELECT COUNT(0) + 1 AS C  FROM MES_TONGS_VALIDATION_HEAD WHERE TO_CHAR(CHECK_START_TIME,'yyyy-MM') = TO_CHAR(SYSDATE,'yyyy-MM')), :CHECK_USER, SYSDATE, :CHECK_REMARKS,:ORGANIZE_ID)";
                        result = await _dbConnection.ExecuteAsync(insertHeadSql, new
                        {
                            ID = headId,
                            CHECK_CODE = TongsNo,
                            CHECK_USER = model.CHECK_USER,
                            CHECK_REMARKS = model.CHECK_REMARKS,
                            ORGANIZE_ID = Tongs.ORGANIZE_ID
                        }, tran);
                        if (result <= 0) { throw new Exception("DATA_ERROR"); }

                        //String insertDetailSql = @"INSERT INTO MES_TONGS_KEEP_DETAIL (ID, KEEP_HEAD_ID, Tongs_TYPE, Tongs_SIZE, Tongs_TYPE_TOTAL, CHECK_QTY) VALUES (:ID, :KEEP_HEAD_ID, :Tongs_TYPE, :Tongs_SIZE,:Tongs_TYPE_TOTAL, :CHECK_QTY)";
                        //foreach (var item in fList)
                        //{
                        //    if (item.Tongs_TYPE == Tongs.FTYPE && item.Tongs_SIZE == Tongs.FSIZE)
                        //    {
                        //        detailId = QueryEx<int>("SELECT SFCS_Tongs_KEEP_DETAIL_SEQ.NEXTVAL MY_SEQ FROM DUAL").FirstOrDefault();
                        //        result = await _dbConnection.ExecuteAsync(insertDetailSql, new
                        //        {
                        //            ID = detailId,
                        //            KEEP_HEAD_ID = headId,
                        //            Tongs_TYPE = Tongs.FTYPE,
                        //            Tongs_SIZE = Tongs.FSIZE,
                        //            Tongs_TYPE_TOTAL = item.Tongs_QTY,
                        //            CHECK_QTY = 1
                        //        }, tran);
                        //    }
                        //    else
                        //    {
                        //        int newid = QueryEx<int>("SELECT SFCS_Tongs_KEEP_DETAIL_SEQ.NEXTVAL MY_SEQ FROM DUAL").FirstOrDefault();
                        //        result = await _dbConnection.ExecuteAsync(insertDetailSql, new
                        //        {
                        //            ID = newid,
                        //            KEEP_HEAD_ID = headId,
                        //            Tongs_TYPE = item.Tongs_TYPE,
                        //            Tongs_SIZE = item.Tongs_SIZE,
                        //            Tongs_TYPE_TOTAL = item.Tongs_QTY,
                        //            CHECK_QTY = 0
                        //        }, tran);
                        //    }
                        //    if (result <= 0) { throw new Exception("DATA_ERROR"); }
                        //}
                        ////获取同类型和尺寸并可用状态的飞达数量
                        //int Tongs_qty = QueryEx<int>("SELECT COUNT(1) FROM SMT_Tongs WHERE STATUS NOT IN (6,7) AND FTYPE = :FTYPE AND FSIZE = :FSIZE AND ORGANIZE_ID = :ORGANIZE_ID ", new
                        //{
                        //    FTYPE = Tongs.FTYPE,
                        //    FSIZE = Tongs.FSIZE,
                        //    ORGANIZE_ID = Tongs.ORGANIZE_ID
                        //}).FirstOrDefault();
                        //if (Tongs_qty <= 0) { throw new Exception("Tongs_CHECK_QTY_ERROR"); }

                        //detailId = QueryEx<int>("SELECT SFCS_Tongs_KEEP_DETAIL_SEQ.NEXTVAL MY_SEQ FROM DUAL").FirstOrDefault();
                        //result = await _dbConnection.ExecuteAsync(insertDetailSql, new
                        //{
                        //    ID = detailId,
                        //    KEEP_HEAD_ID = headId,
                        //    Tongs_TYPE = Tongs.FTYPE,
                        //    Tongs_SIZE = Tongs.FSIZE,
                        //    Tongs_TYPE_TOTAL = Tongs_qty
                        //}, tran);
                        //if (result <= 0) { throw new Exception("DATA_ERROR"); }
                    }
                    var tongsMaintainModel = (await _dbConnection.QueryAsync<MesTongsMaintainHistory>("SELECT * FROM MES_TONGS_MAINTAIN_HISTORY WHERE ID=:ID", new { ID = model.TONGS_MAINTAIN_ID }))?.FirstOrDefault();
                    //0:未验证,1:合格,2:不合格
                    var resutStatus = tongsMaintainModel == null ? 0 : (tongsMaintainModel.STATUS ?? 0);
                    int contentId = QueryEx<int>("SELECT MES_TONGS_VALI_DETAIL_SEQ.NEXTVAL MY_SEQ FROM DUAL").FirstOrDefault();
                    String insertContentSql = @"INSERT INTO MES_TONGS_VALIDATION_DETAIL (ID, VALIDATION_HEAD_ID, TONGS_ID, TONGS_STATUS, CHECK_USER, CHECK_TIME, CHECK_REMARKS,TONGS_MAINTAIN_ID) VALUES (:ID, :VALIDATION_HEAD_ID, :TONGS_ID, :TONGS_STATUS, :CHECK_USER,SYSDATE,:CHECK_REMARKS,:TONGS_MAINTAIN_ID)";
                    var keepheadid = headId == 0 ? head.ID : headId;
                    result = await _dbConnection.ExecuteAsync(insertContentSql, new
                    {
                        ID = contentId,
                        VALIDATION_HEAD_ID = keepheadid,
                        TONGS_ID = Tongs.ID,
                        TONGS_STATUS = resutStatus,
                        CHECK_REMARKS = model.CHECK_REMARKS,
                        CHECK_USER = model.CHECK_USER,
                        TONGS_MAINTAIN_ID = model.TONGS_MAINTAIN_ID
                    }, tran);
                    if (result <= 0) throw new Exception("DATA_ERROR");

                    tran.Commit();
                }
                catch (Exception ex)
                {
                    headId = 0;
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
            return headId;
        }

        public async Task<List<TongsCheckListModel>> LoadPDATongsValidationList(TongsCheckRequestModel model, string organize_id)
        {
            TongsCheckModel fModel = new TongsCheckModel();
            int page = 0, limit = 0;
            page = model.Page * model.Limit - model.Limit + 1;
            limit = model.Page * model.Limit;
            fModel.Page = page;
            fModel.Limit = limit;

            String orderBy = " ORDER BY CHECK_START_TIME DESC ";
            String whereStr = "";

            if (!model.START_TIME.IsNullOrEmpty())
            {
                fModel.START_TIME = Convert.ToDateTime(model.START_TIME.Trim() + " 00:00:00");
                whereStr += " AND CHECK_START_TIME >= :START_TIME ";

                if (!model.END_TIME.IsNullOrEmpty())
                {
                    fModel.END_TIME = Convert.ToDateTime(model.END_TIME.Trim() + " 23:59:59");
                    whereStr += " AND CHECK_START_TIME <= :END_TIME ";
                }
            }
            fModel.CHECK_STATUS = model.CHECK_STATUS;
            if (model.CHECK_STATUS >= 0)
            {
                whereStr += " AND CHECK_STATUS = :CHECK_STATUS ";
            }
            String sQuery = string.Format("SELECT * FROM (SELECT ROWNUM R, T.* FROM ( SELECT * FROM MES_TONGS_VALIDATION_HEAD WHERE 1=1 {0} {1}) T) WHERE R BETWEEN :Page AND :Limit", whereStr, orderBy);

            var resdata = await _dbConnection.QueryAsync<TongsCheckListModel>(sQuery, fModel);

            List<TongsCheckListModel> list = resdata.ToList();
            foreach (var item in list)
            {
                item.CHECK_TIME = item.CHECK_START_TIME.ToString("yyyy-MM-dd");
                item.TONGS_QTY = QueryEx<int>("SELECT SUM(VALIDATION_HEAD_ID) FROM MES_TONGS_VALIDATION_DETAIL WHERE VALIDATION_HEAD_ID = :VALIDATION_HEAD_ID ", new
                {
                    VALIDATION_HEAD_ID = item.ID
                }).FirstOrDefault();
                var TongsCheckList = await GetPDATongsValidationDataByHeadID(organize_id, pageInde: model.Page, pageSize: model.Limit);
                item.CHECK_QTY = (TongsCheckList == null || TongsCheckList.count <= 0) ? 0 : TongsCheckList.count;

                item.CHECK_YEAR = item.CHECK_START_TIME.Year.ToString();
                item.CHECK_HEAD = item.CHECK_START_TIME.Month.ToString() + "月第" + item.CHECK_COUNT + "次盘点";
            }

            return list;
        }

        public async Task<int> LoadPDATongsValidationListCount(TongsCheckRequestModel model)
        {
            TongsCheckModel fModel = new TongsCheckModel();

            String whereStr = "";

            if (!model.START_TIME.IsNullOrEmpty())
            {
                fModel.START_TIME = Convert.ToDateTime(model.START_TIME.Trim() + " 00:00:00");
                whereStr += " AND CHECK_START_TIME >= :START_TIME ";

                if (!model.END_TIME.IsNullOrEmpty())
                {
                    fModel.END_TIME = Convert.ToDateTime(model.END_TIME.Trim() + " 23:59:59");
                    whereStr += " AND CHECK_START_TIME <= :END_TIME ";
                }
            }
            fModel.CHECK_STATUS = model.CHECK_STATUS;
            if (model.CHECK_STATUS >= 0)
            {
                whereStr += " AND CHECK_STATUS = :CHECK_STATUS ";
            }
            String sQuery = string.Format("SELECT COUNT(1) FROM MES_TONGS_VALIDATION_HEAD WHERE 1=1 {0}", whereStr);

            int count = await _dbConnection.ExecuteScalarAsync<int>(sQuery, fModel);
            return count;
        }

        /// <summary>
        /// 获取验证数据
        /// </summary>
        /// <param name="headid">VALIDATION_HEAD_ID</param>
        /// <returns></returns>
        public async Task<TableDataModel> GetPDATongsValidationDataByHeadID(string organizeId, int pageInde = 1, int pageSize = 99999999, Decimal headid = 0)
        {
            TableDataModel dataModel = new TableDataModel();
            List<TongsInfoInnerValidationDetail> result = new List<TongsInfoInnerValidationDetail>();
            try
            {
                string selectTongsCheckData = @"
                                                SELECT
											    	INFO.ID INFO_ID,
											    	INFO.CODE,
											    	SPS1.MEANING TONGS_TYPE,
											    	STORE.NAME STORE_NAME,
											    	{0}
											    FROM
											    	MES_TONGS_INFO INFO,
											    	MES_TONGS_STORE_CONFIG STORE,
                                                    MES_TONGS_VALIDATION_HEAD VHEAD,
                                                    MES_TONGS_VALIDATION_DETAIL VDETAIL,
                                                    MES_TONGS_MAINTAIN_HISTORY HISTORY,
											    	( SELECT * FROM SFCS_PARAMETERS WHERE LOOKUP_TYPE = 'MES_TONGS_TYPE' AND ENABLED = 'Y' ) SPS1 
											    WHERE
											    	INFO.STORE_ID = STORE.id ( + ) 
											    	AND SPS1.LOOKUP_CODE ( + ) = INFO.TONGS_TYPE 
                                                    AND VHEAD.ID(+)=VDETAIL.VALIDATION_HEAD_ID
                                                    AND VDETAIL.TONGS_ID(+)=INFO.ID
                                                    AND VDETAIL.TONGS_MAINTAIN_ID=HISTORY.ID(+)
											    	AND INFO.STATUS != 6 
											    	AND INFO.ORGANIZE_ID IN ({1})
";
                string countTongsCheckData = @"
                                                SELECT
											    	COUNT(*)
											    FROM
											    	MES_TONGS_INFO INFO,
											    	MES_TONGS_STORE_CONFIG STORE,
                                                    MES_TONGS_VALIDATION_HEAD VHEAD,
                                                    MES_TONGS_VALIDATION_DETAIL VDETAIL,
                                                    MES_TONGS_MAINTAIN_HISTORY HISTORY,
											    	( SELECT * FROM SFCS_PARAMETERS WHERE LOOKUP_TYPE = 'MES_TONGS_TYPE' AND ENABLED = 'Y' ) SPS1 
											    WHERE
											    	INFO.STORE_ID = STORE.id ( + ) 
											    	AND SPS1.LOOKUP_CODE ( + ) = INFO.TONGS_TYPE 
                                                    AND VHEAD.ID(+)=VDETAIL.VALIDATION_HEAD_ID
                                                    AND VDETAIL.TONGS_ID(+)=INFO.ID
                                                    AND VDETAIL.TONGS_MAINTAIN_ID=HISTORY.ID(+)
											    	AND INFO.STATUS != 6 
											    	AND INFO.ORGANIZE_ID IN ({0})";

                if (headid == 0)
                {
                    string sql = String.Format(selectTongsCheckData, @"'' CHECK_REMARKS,'' CHECK_TIME,'' CHECK_USER,0 VDID,'未验证' VDETAIL_STATUS,'' CHECK_CODE ", organizeId);
                    string sqlByPage = $@"SELECT U.* FROM     ( SELECT ROWNUM RNUM,T.* FROM ( {sql} ) T WHERE ROWNUM <= :PAGESIZE*:PAGEINDEX )U
                                           WHERE U.RNUM > (: PAGEINDEX - 1) *:PAGESIZE";
                    result = (await _dbConnection.QueryAsync<TongsInfoInnerValidationDetail>(sqlByPage, new { PAGEINDEX = pageInde, PAGESIZE = pageSize }))?.ToList();
                }
                else
                {
                    selectTongsCheckData += " AND VDETAIL.VALIDATION_HEAD_ID(+)=:ID ";
                    string sql = String.Format(selectTongsCheckData, @"VDETAIL.CHECK_REMARKS,TO_CHAR(VDETAIL.CHECK_TIME,'yyyy-MM-dd HH24:mi:ss') CHECK_TIME,VDETAIL.CHECK_USER,NVL(VDETAIL.VALIDATION_HEAD_ID,:ID) VDID,DECODE(VDETAIL.TONGS_STATUS,1,'合格',2,'不合格','未验证') VDETAIL_STATUS,VDETAIL.TONGS_MAINTAIN_ID,VHEAD.CHECK_CODE", organizeId);
                    string sqlByPage = $@"SELECT U.* FROM     ( SELECT ROWNUM RNUM,T.* FROM ( {sql} ) T WHERE ROWNUM <= :PAGESIZE*:PAGEINDEX )U
                                           WHERE U.RNUM > (: PAGEINDEX - 1) *:PAGESIZE";
                    result = (await _dbConnection.QueryAsync<TongsInfoInnerValidationDetail>(sqlByPage, new { ID = headid, PAGEINDEX = pageInde, PAGESIZE = pageSize }))?.ToList();
                }
                dataModel.data = result;
                dataModel.count = (await _dbConnection.ExecuteScalarAsync<int>(String.Format(countTongsCheckData, organizeId)));

            }
            catch (Exception ex)
            {
                dataModel.code = -1;
                dataModel.msg = ex.Message;
            }
            return dataModel;
        }

        /// <summary>
        /// 确认PDA工装盘点数据
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public async Task<bool> ConfirmPDATongsValidationData(AuditTongsCheckDataRequestModel model)
        {
            int result = 0;
            ConnectionFactory.OpenConnection(_dbConnection);
            using (var tran = _dbConnection.BeginTransaction())
            {
                try
                {
                    if (model.STATUS == 1)
                    {
                        String updateHeadSql = @"UPDATE MES_TONGS_VALIDATION_HEAD SET CHECK_STATUS = '1',CHECK_END_TIME = SYSDATE WHERE ID = :ID ";
                        result = await _dbConnection.ExecuteAsync(updateHeadSql, new
                        {
                            ID = model.ID
                        }, tran);
                        if (result <= 0) throw new Exception("DATA_ERROR");
                    }
                    if (model.STATUS == 2)
                    {
                        String updateHeadSql = @"UPDATE MES_TONGS_VALIDATION_HEAD SET CHECK_STATUS = '2',AUDIT_USER = :AUDIT_USER,AUDIT_REMARKS = :AUDIT_REMARKS,AUDIT_TIME = SYSDATE WHERE ID = :ID ";
                        result = await _dbConnection.ExecuteAsync(updateHeadSql, new
                        {
                            AUDIT_USER = model.AUDIT_USER,
                            AUDIT_REMARKS = model.AUDIT_REMARKS,
                            ID = model.ID
                        }, tran);
                        if (result <= 0) throw new Exception("DATA_ERROR");
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
            return result > 0 ? true : false;
        }

        /// <summary>
        /// 删除PDA工装验证数据记录
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<bool> DeletePDATongsValidationData(Decimal id)
        {
            int result = 0;
            ConnectionFactory.OpenConnection(_dbConnection);
            using (var tran = _dbConnection.BeginTransaction())
            {
                try
                {
                    //工装点检记录详细表
                    string deleteSql = @"DELETE FROM MES_TONGS_VALIDATION_DETAIL WHERE VALIDATION_HEAD_ID = :ID ";
                    result = await _dbConnection.ExecuteAsync(deleteSql, new { ID = id }, tran);

                    //工装点检记录主表
                    deleteSql = @"DELETE FROM MES_TONGS_VALIDATION_HEAD WHERE ID = :ID ";
                    result = await _dbConnection.ExecuteAsync(deleteSql, new { ID = id }, tran);
                    if (result <= 0) throw new Exception("DATA_ERROR");

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
            return result > 0 ? true : false;
        }

        /// <summary>
        ///查是否保养过
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<bool> QueryPDATongsValidationBy(String check_code, decimal hid)
        {
            int result = 0;
            try
            {
                var querySql = @"SELECT COUNT(*) FROM MES_TONGS_VALIDATION_HEAD HEAD,MES_TONGS_VALIDATION_DETAIL DETAIL,MES_TONGS_INFO INFO
                                  WHERE HEAD.ID=DETAIL.VALIDATION_HEAD_ID AND DETAIL.TONGS_ID=INFO.ID AND INFO.CODE=:CODE AND HEAD.ID=:ID AND DETAIL.TONGS_STATUS!=0 ";
                result = await _dbConnection.ExecuteScalarAsync<int>(querySql, new { CODE = check_code, ID = hid });
            }
            catch (Exception ex)
            {
                result = -1;
                throw ex;
            }
            finally
            {
                if (_dbConnection.State != System.Data.ConnectionState.Closed)
                {
                    _dbConnection.Close();
                }

            }
            return result > 0 ? true : false;
        }

        /// <summary>
        /// PDA验证--保养保存
        /// </summary>
        /// <param name="tongsId"></param>
        /// <param name="user"></param>
        /// <returns></returns>
        public async Task<BaseResult> PDAMaintain(MesTongsMaintainHistory model)
        {
            var result = new BaseResult();
            string editHistorySql = @"UPDATE MES_TONGS_MAINTAIN_HISTORY SET OPERATION_ID=:OPERATION_ID,MAINTAIN_USER=:MAINTAIN_USER,
						END_DATE=SYSDATE,STATUS=:STATUS,REMARK=:REMARK WHERE ID=:ID";
            string addHistorySql = @"INSERT INTO MES_TONGS_MAINTAIN_HISTORY (ID,TONGS_ID, OPERATION_ID,START_DATE,CREATE_USER,CREATE_DATE,STATUS,REMARK,MAINTAIN_USER,TYPE) 
                                                                      VALUES(:ID,:TONGS_ID,:OPERATION_ID,SYSDATE,:CREATE_USER,SYSDATE,:STATUS,:REMARK,:MAINTAIN_USER,:TYPE) ";
            string addItemsSql = @"INSERT INTO MES_TONGS_MAINTAIN_DETAIL VALUES(MES_TONGS_MAINTAIN_DTL_SEQ.NEXTVAL,
									:MST_ID,:ITEM_ID,:ITEM_STATUS,:REMARK)";

            ConnectionFactory.OpenConnection(_dbConnection);
            using (var tran = _dbConnection.BeginTransaction())
            {
                try
                {
                    //获取一个新的操作记录ID
                    model.OPERATION_ID = await GetOperationSEQ();
                    model.ID = await Get_MES_SEQ_ID("MES_TONGS_MAINTAIN_SEQ");

                    //获取到夹具信息
                    var tongs = await GetTongsById(model.TONGS_ID);
                    if (tongs == null)
                    {
                        tran.Rollback();
                        result.ResultCode = 106;
                        result.ResultMsg = "找不到对应夹具，请刷新后重试！";
                        return result;
                    }

                    //插入操作记录信息
                    MesTongsOperationHistory data = new MesTongsOperationHistory
                    {
                        ID = model.OPERATION_ID,
                        CREATE_USER = model.MAINTAIN_USER,
                        OPERATION_TYPE = 7,
                        TONGS_ID = model.TONGS_ID,
                        STORE_ID = tongs.STORE_ID,
                        LAST_STATUS = model.STATUS == 1 ? 0 : 5,
                        PRE_STATUS = tongs.STATUS
                    };

                    await InsertOperationRecord(data);

                    //更新夹具状态
                    //await _dbConnection.ExecuteAsync(editTongsSql, new { UPDATE_USER = model.MAINTAIN_USER, ID = model.TONGS_ID, STATUS = (model.STATUS == 1 ? 0 : 5) });

                    //更新保养记录数据
                    await _dbConnection.ExecuteAsync(addHistorySql, model);

                    //插入保养明细数据
                    foreach (var item in model.DetailList)
                    {
                        item.MST_ID = model.ID;
                        await _dbConnection.ExecuteAsync(addItemsSql, new
                        {
                            MST_ID = item.MST_ID,
                            ITEM_ID = item.ITEM_ID,
                            ITEM_STATUS = item.ITEM_STATUS,
                            REMARK = item.REMARK
                        });
                    }

                    tran.Commit();
                    result.ResultCode = 0;
                    result.ResultMsg = "操作成功！";
                    result.ResultData = model.ID.ToString();
                    return result;
                }
                catch (Exception ex)
                {
                    tran.Rollback();
                    result.ResultCode = 106;
                    result.ResultMsg = ex.Message;
                    return result;
                }
                finally
                {
                    if (_dbConnection.State != System.Data.ConnectionState.Closed)
                    {
                        _dbConnection.Close();
                    }
                }
            }
        }

        #endregion
    }
}