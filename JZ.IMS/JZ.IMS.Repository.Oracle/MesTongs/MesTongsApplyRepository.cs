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

namespace JZ.IMS.Repository.Oracle.MesTongs
{
    public class MesTongsApplyRepository : BaseRepository<MesTongsApply, Decimal>, IMesTongsApplyRepository
    {
        public MesTongsApplyRepository(IOptionsSnapshot<DbOption> options)
        {
            _dbOption = options.Get("iWMS");
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
            string sql = "SELECT ENABLED FROM MES_TONGS_APPLY WHERE ID=:ID";
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
            string sql = "UPDATE MES_TONGS_APPLY set ENABLED=:ENABLED WHERE ID=:Id";
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
            string sql = "SELECT MES_TONGS_APPLY_SEQ.NEXTVAL MY_SEQ FROM DUAL";
            var result = await _dbConnection.ExecuteScalarAsync(sql);
            return (decimal)result;
        }

        /// <summary>
        /// 新增/修改夹具申请信息
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public async Task<BaseResult> AddOrModify(MesTongsApplyListModel model)
        {
            var result = new BaseResult();

            string addApplySql = @"INSERT INTO MES_TONGS_APPLY(ID,QTY,SURPLUS_QTY,TONGS_TYPE,DEPARTMENT,SOURCES,STATUS,CREATE_USER,CREATE_DATE,UPDATE_USER,UPDATE_DATE,NEED_DATE,REMARK,ORGANIZE_ID) VALUES(
									:ID,:QTY,:SURPLUS_QTY,:TONGS_TYPE,:DEPARTMENT,:SOURCES,0,:CREATE_USER,SYSDATE,:CREATE_USER,SYSDATE,:NEED_DATE,:REMARK,:ORGANIZE_ID)";
            string editTongsSql = @"UPDATE MES_TONGS_APPLY SET QTY=:QTY,SURPLUS_QTY=:SURPLUS_QTY,TONGS_TYPE=:TONGS_TYPE,DEPARTMENT=:DEPARTMENT,SOURCES=:SOURCES,UPDATE_USER=:UPDATE_USER,UPDATE_DATE=SYSDATE,NEED_DATE=:NEED_DATE,REMARK=:REMARK ,ORGANIZE_ID=:ORGANIZE_ID  WHERE ID=:ID";
            string delTongsPartSql = @"DELETE MES_TONGS_APPLY_PART WHERE TONGS_APPLY_ID = :TONGS_ID";
            string addTongsPartSql = @"INSERT INTO MES_TONGS_APPLY_PART(ID,TONGS_APPLY_ID,PART_NO,PART_NAME,PART_DESC,VERSION,CREATE_USER,CREATE_DATE,ENABLED) VALUES(
									MES_TONGS_APPLY_PART_SEQ.NEXTVAL,:TONGS_APPLY_ID,:PART_NO,:PART_NAME,:PART_DESC,:VERSION,:CREATE_USER,SYSDATE,'Y')";

            ConnectionFactory.OpenConnection(_dbConnection);
            using (var tran = _dbConnection.BeginTransaction())
            {
                try
                {
                    if (model.ID == 0)
                    {//新增
                        model.ID = await GetSEQID();
                        if (await _dbConnection.ExecuteAsync(addApplySql, model) == 1)
                        {
                            if (model.PartList != null)
                                foreach (var part in model.PartList)
                                {
                                    part.TONGS_APPLY_ID = model.ID;
                                    part.CREATE_USER = model.CREATE_USER;
                                    await _dbConnection.ExecuteAsync(addTongsPartSql, part);
                                }
                        }
                        else
                        {
                            tran.Rollback();
                            result.ResultCode = ResultCodeAddMsgKeys.CommonExceptionCode;
                            result.ResultMsg = "夹具申请失败，请刷新后重试！";
                            return result;
                        }
                    }
                    else
                    {//修改
                        if (await _dbConnection.ExecuteAsync(editTongsSql, model) == 1)
                        {
                            await _dbConnection.ExecuteAsync(delTongsPartSql, new { TONGS_ID = model.ID });

                            if (model.PartList != null)
                                foreach (var part in model.PartList)
                                {
                                    part.TONGS_APPLY_ID = model.ID;
                                    part.CREATE_USER = model.UPDATE_USER;
                                    await _dbConnection.ExecuteAsync(addTongsPartSql, part);
                                }
                        }
                        else
                        {
                            tran.Rollback();
                            result.ResultCode = ResultCodeAddMsgKeys.CommonExceptionCode;
                            result.ResultMsg = "更新夹具申请信息失败，请刷新后重试！";
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
        /// 获取夹具申请信息列表信息
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>	
        public async Task<IEnumerable<MesTongsApplyListModel>> GetTongsApplyData(MesTongsApplyRequestModel model)
        {
            string sql = @"SELECT * FROM (SELECT TT.*,ROWNUM AS ROWNO FROM (
					SELECT MST.*,TONGSTYPE.MEANING AS TYPE_NAME,PARA.CHINESE AS DEPARTMENT_NAME,TONGS.* FROM MES_TONGS_APPLY MST
					LEFT JOIN （SELECT A.ID, A.CHINESE, B.CHINESE AS SBU_CHINESE, B.ID AS SBU_ID FROM SFCS_LOOKUPS A 
									INNER JOIN SFCS_PARAMETERS B ON B.ID = A.KIND AND A.ENABLED = 'Y' AND B.LOOKUP_TYPE = 'SBU_CODE'
									AND B.ENABLED = 'Y'） PARA ON MST.DEPARTMENT = PARA.ID
					LEFT JOIN (SELECT APPLY_ID,
						SUM(CASE WHEN STATUS IN (0,1) AND ACTIVE = 'Y' THEN 1 ELSE 0 END) AS USABLE_QTY,
						SUM(CASE WHEN STATUS IN (2,7) THEN 1 ELSE 0 END) AS SUM_LOAN_QTY,
						SUM(CASE WHEN STATUS IN (7) THEN 1 ELSE 0 END) AS LASTING_LOAN_QTY,
						SUM(CASE WHEN ACTIVE = 'N' THEN 1 ELSE 0 END) AS NO_ACTIVE_QTY,
						SUM(CASE WHEN STATUS IN (4,5) THEN 1 ELSE 0 END) AS REPAIR_QTY,
						SUM(CASE WHEN STATUS IN (6) THEN 1 ELSE 0 END) AS INVALID_QTY
					 FROM MES_TONGS_INFO WHERE ENABLED='Y'
					 GROUP BY APPLY_ID) TONGS ON TONGS.APPLY_ID = MST.ID
				   LEFT JOIN (SELECT ID,LOOKUP_TYPE,NAME,LOOKUP_CODE,MEANING,DESCRIPTION,CHINESE,ENABLED 
				     FROM SFCS_PARAMETERS  WHERE LOOKUP_TYPE='MES_TONGS_TYPE' and ENABLED = 'Y') 
					 TONGSTYPE on TONGSTYPE.LOOKUP_CODE=MST.TONGS_TYPE
					";
            sql += GetWhereStr(model);
            sql += @" order by MST.ID DESC ) tt where ROWNUM <= :Limit*:Page) tt2 where tt2.rowno > (:Page-1)*:Limit";

            return await _dbConnection.QueryAsync<MesTongsApplyListModel>(sql, model);
        }

        /// <summary>
        /// 获取夹具申请信息数量
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public async Task<int> GetTongsApplyDataCount(MesTongsApplyRequestModel model)
        {
            string whereStr = GetWhereStr(model);
            StringBuilder sql = new StringBuilder();
            sql.Append("select count(*) from MES_TONGS_APPLY MST ");
            sql.Append(whereStr);

            return await _dbConnection.ExecuteScalarAsync<int>(sql.ToString(), model);
        }

        /// <summary>
        /// 获取夹具申请信息列表信息（包含对应产品信息）
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>	
        public async Task<List<dynamic>> GetTongsApplyAndPartData(MesTongsApplyRequestModel model)
        {
            string sql = @"select * from	
                           (select * from (SELECT ROWNUM as rowno,MST.*,
							 P.PART_NO,
							 P.PART_NAME,
							 P.PART_DESC,
							 a.SBU_CHINESE DEPARTMENT_NAME
						FROM MES_TONGS_APPLY MST
							 LEFT JOIN MES_TONGS_APPLY_PART P ON MST.ID = P.TONGS_APPLY_ID
							 LEFT JOIN (SELECT A.ID, A.CHINESE, B.CHINESE AS SBU_CHINESE, B.ID AS SBU_ID FROM SFCS_LOOKUPS A 
					    INNER JOIN SFCS_PARAMETERS B ON B.ID = A.KIND AND A.ENABLED = 'Y' AND B.LOOKUP_TYPE = 'SBU_CODE'
				         AND B.ENABLED = 'Y')  a on a.ID=MST.DEPARTMENT
					";
            sql += GetWhereStr(model);
            sql += @" ORDER BY MST.ID DESC ) tt where tt.rowno <= :Limit*:Page) tt2 where tt2.rowno > (:Page-1)*:Limit";
            var result = (await _dbConnection.QueryAsync<dynamic>(sql, model))?.ToList();
            return result;
        }

        /// <summary>
        /// 获取夹具申请信息数量
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public async Task<int> GetTongsApplyAndPartDataCount(MesTongsApplyRequestModel model)
        {
            string whereStr = GetWhereStr(model);
            StringBuilder sql = new StringBuilder();
            sql.Append("select count(*) from MES_TONGS_APPLY MST LEFT JOIN MES_TONGS_APPLY_PART P ON MST.ID = P.TONGS_APPLY_ID");
            sql.Append(whereStr);

            return await _dbConnection.ExecuteScalarAsync<int>(sql.ToString(), model);
        }

        /// <summary>
        /// 根据ID获取夹具申请信息
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<MesTongsApplyListModel> GetTongsApplyById(decimal id)
        {
            string sql = @"SELECT * FROM MES_TONGS_APPLY WHERE ID=:ID";
            var data = (await _dbConnection.QueryAsync<MesTongsApplyListModel>(sql, new { ID = id })).FirstOrDefault();
            data.PartList = await GetTongsApplyPartData(id);
            return data;
        }

        /// <summary>
        /// 根据夹具申请ID获取对应产品信息
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<List<MesTongsPartModel>> GetTongsApplyPartData(decimal id)
        {
            string sql = "SELECT * FROM MES_TONGS_APPLY_PART WHERE TONGS_APPLY_ID = :TONGS_APPLY_ID AND ENABLED = 'Y'";
            return (await _dbConnection.QueryAsync<MesTongsPartModel>(sql, new { TONGS_APPLY_ID = id })).ToList();
        }

        /// <summary>
        /// 根据夹具申请ID获取对应已入库夹具信息
        /// </summary>
        /// <param name="organizeId"></param>
        /// <param name="applyId"></param>
        /// <returns></returns>
        public async Task<List<MesTongsInfoListModel>> GetTongsDataByApplyId(string organizeId, decimal applyId)
        {
            string sql = @"SELECT MST.*,STORE.CODE AS STORE_CODE,STORE.NAME AS STORE_NAME FROM MES_TONGS_INFO MST
                    LEFT JOIN MES_TONGS_STORE_CONFIG STORE ON MST.STORE_ID = STORE.ID 
					WHERE EXISTS (SELECT 1
									 FROM (    SELECT ID
												 FROM SYS_ORGANIZE
										   START WITH ID = :ORGANIZE_ID
										   CONNECT BY PRIOR ID = PARENT_ORGANIZE_ID)
									WHERE ID = MST.ORGANIZE_ID) 
					AND MST.APPLY_ID=:APPLY_ID 
					order by MST.ID DESC
					";

            return (await _dbConnection.QueryAsync<MesTongsInfoListModel>(sql, new { ORGANIZE_ID = organizeId, APPLY_ID = applyId })).ToList();
        }

        /// <summary>
        /// 根据ID删除夹具申请信息
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<BaseResult> DeleteById(decimal id)
        {
            var result = new BaseResult();

            string delPartSql = "DELETE FROM MES_TONGS_APPLY_PART WHERE TONGS_APPLY_ID = :ID";
            string delSql = "DELETE FROM MES_TONGS_APPLY WHERE ID = :ID";

            var model = Get(id);
            if (model == null)
            {
                result.ResultCode = ResultCodeAddMsgKeys.CommonExceptionCode;
                result.ResultMsg = "数据已被删除或不存在，请刷新后重试！";
                return result;
            }

            if (model.STATUS != 0)
            {
                result.ResultCode = ResultCodeAddMsgKeys.CommonExceptionCode;
                result.ResultMsg = "当前夹具申请信息不是初始状态！无法删除！";
                return result;
            }

            ConnectionFactory.OpenConnection(_dbConnection);
            using (var tran = _dbConnection.BeginTransaction())
            {
                try
                {
                    if (await _dbConnection.ExecuteAsync(delSql, new { ID = id }) > 0)
                    {
                        await _dbConnection.ExecuteAsync(delPartSql, new { ID = id });
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
        /// 审核夹具申请信息
        /// </summary>
        /// <param name="id"></param>
        /// <param name="user"></param>
        /// <returns></returns>
        public async Task<BaseResult> AuditData(decimal id, string user)
        {
            var result = new BaseResult();
            var model = Get(id);
            if (model == null)
            {
                result.ResultCode = ResultCodeAddMsgKeys.CommonExceptionCode;
                result.ResultMsg = "数据已被删除或不存在，请刷新后重试！";
                return result;
            }

            if (model.STATUS != 0)
            {
                result.ResultCode = ResultCodeAddMsgKeys.CommonExceptionCode;
                result.ResultMsg = "当前夹具申请信息不是初始状态！无法审核！";
                return result;
            }
            string sql = "UPDATE MES_TONGS_APPLY SET UPDATE_USER = :USER_NAME,UPDATE_DATE=SYSDATE,AUDIT_USER=:USER_NAME,AUDIT_DATE=SYSDATE,STATUS = 1 WHERE ID=:ID";

            try
            {
                await _dbConnection.ExecuteAsync(sql, new { ID = id, USER_NAME = user });
                result.ResultCode = 0;
                result.ResultMsg = "操作成功！";
            }
            catch (Exception ex)
            {
                result.ResultCode = ResultCodeAddMsgKeys.CommonExceptionCode;
                result.ResultMsg = ex.Message;
            }

            return result;
        }

        /// <summary>
        /// 获取WHERE条件
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        private string GetWhereStr(MesTongsApplyRequestModel model)
        {
            StringBuilder whereStr = new StringBuilder();
            whereStr.Append("  WHERE MST.ID > 0 ");

            if (!string.IsNullOrEmpty(model.ORGANIZE_ID))
                whereStr.Append(@"  AND EXISTS
								  (SELECT 1
									 FROM (    SELECT ID
												 FROM SYS_ORGANIZE
										   START WITH ID = :ORGANIZE_ID
										   CONNECT BY PRIOR ID = PARENT_ORGANIZE_ID)
									WHERE ID = MST.ORGANIZE_ID) ");

            if (model.TONGS_TYPE != null)
                whereStr.Append(" and MST.TONGS_TYPE=:TONGS_TYPE ");

            if (model.DEPARTMENT != null)
                whereStr.Append(" and MST.DEPARTMENT=:DEPARTMENT ");

            if (model.SOURCES != null)
                whereStr.Append(" and MST.SOURCES=:SOURCES ");

            if (model.STATUS != null)
                whereStr.Append(" and MST.STATUS=:STATUS ");

            if (model.BEGIN_DATE != null)
                whereStr.Append(" and MST.NEED_DATE >= :BEGIN_DATE ");

            if (model.END_DATE != null)
                whereStr.Append(" and MST.NEED_DATE <= :END_DATE ");

            return whereStr.ToString();
        }
    }
}
