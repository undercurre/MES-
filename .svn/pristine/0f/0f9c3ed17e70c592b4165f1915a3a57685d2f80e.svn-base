/**
*┌──────────────────────────────────────────────────────────────┐
*│　描    述：设备基本信息表接口实现                                                    
*│　作    者：嘉志科技                                            
*│　版    本：2.0    模板代码自动生成                                                
*│　创建时间：2019-10-28 17:48:27                             
*└──────────────────────────────────────────────────────────────┘
*┌──────────────────────────────────────────────────────────────┐
*│　命名空间： JZ.IMS.Repository.Oracle                                  
*│　类    名： SfcsEquipmentRepository                                      
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
using System.Linq;
using System.Text;
using JZ.IMS.Core.Extensions;

namespace JZ.IMS.Repository.Oracle
{
    public class SfcsEquipmentRepository : BaseRepository<SfcsEquipment, Decimal>, ISfcsEquipmentRepository
    {
        public SfcsEquipmentRepository(IOptionsSnapshot<DbOption> options)
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
            string sql = "SELECT ENABLE FROM SFCS_EQUIPMENT WHERE ID=:ID ";
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
            string sql = "update SFCS_EQUIPMENT set ENABLE=:ENABLE where  Id=:Id";
            return await _dbConnection.ExecuteAsync(sql, new
            {
                ENABLE = status ? 'Y' : 'N',
                Id = id,
            });
        }

        /// <summary>
        /// 获取表的序列
        /// </summary>
        /// <returns></returns>
        public async Task<decimal> GetSEQID()
        {
            string sql = "SELECT SFCS_EQUIPMENT_SEQ.NEXTVAL MY_SEQ FROM DUAL";
            var result = await _dbConnection.ExecuteScalarAsync(sql);
            return (decimal)result;
        }

        /// <summary>
        /// 根据主键获取设备
        /// </summary>
        /// <param name="id">主键</param>
        /// <returns></returns>
        public SfcsEquipmentListModel GetModelById(decimal id)
        {
            string sql = "SELECT * FROM SFCS_EQUIPMENT WHERE ID=:ID ";
            var result = _dbConnection.Query<SfcsEquipmentListModel>(sql, new
            {
                ID = id,
            }).FirstOrDefault();

            return result;
        }

        /// <summary>
        /// 根据ID修改设备状态（用于回写）
        /// </summary>
        /// <param name="id">设备ID</param>
        /// <param name="status">设备状态</param>
        /// <returns></returns>
        public async Task<int> EditEquipStatus(decimal id, decimal status)
        {
            string sql = "update SFCS_EQUIPMENT set STATUS=:STATUS where ID = :ID";
            return await _dbConnection.ExecuteAsync(sql, new { STATUS = status, ID = id });
        }

        /// <summary>
        /// 根据条件获取设备列表信息（分页）
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public async Task<int> GetEquipmentListCount(SfcsEquipmentRequestModel model)
        {
            string whereStr = GetWhereStr(model);
            StringBuilder sql = new StringBuilder();
            sql.Append("select count(*) from SFCS_EQUIPMENT tb1 ");
            sql.Append(whereStr.ToString());

            return await _dbConnection.ExecuteScalarAsync<int>(sql.ToString(), new { model.CATEGORY, model.STATION_ID, model.NAME, model.CATEGORY_NAME, model.STATION_NAME, model.STATUS, model.ENABLE, model.Limit, model.Page });
        }

        /// <summary>
        /// 根据条件获取设备列表信息（分页）
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public SfcsEquipmentListModel GetEquipment(string ONLY_CODE)
        {
            string sql = "SELECT * FROM SFCS_EQUIPMENT WHERE ONLY_CODE = :ONLY_CODE";
            var result = _dbConnection.Query<SfcsEquipmentListModel>(sql, new
            {
                ONLY_CODE = ONLY_CODE
            }).FirstOrDefault();

            return result;
        }

        /// <summary>
        /// 根据条件获取设备列表数量
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public async Task<IEnumerable<SfcsEquipmentListModel>> GetEquipmentList(SfcsEquipmentRequestModel model)
        {
            string whereStr = GetWhereStr(model);

            StringBuilder sql = new StringBuilder();
            sql.Append("select * from (select tt.*,ROWNUM AS rowno from (");
            sql.Append(@"select tb1.*,tb2.LINE_NAME AS STATION_NAME,TB3.CHINESE AS CATEGORY_NAME,tb4.ORGANIZE_NAME AS USER_PART_NAME from SFCS_EQUIPMENT tb1
						left join(select ID, LINE_NAME from SMT_LINES union select ID, OPERATION_LINE_NAME AS LINE_NAME from SFCS_OPERATION_LINES) tb2 on tb1.STATION_ID = tb2.ID 
						left join(select LOOKUP_CODE, CHINESE from SFCS_PARAMETERS  where lookup_type = 'EQUIPMENT_CATEGORY' AND ENABLED = 'Y') tb3 on tb1.CATEGORY = tb3.LOOKUP_CODE 
						left join(select ORGANIZE_NAME,ID from SYS_ORGANIZE) tb4 on tb1.USER_PART = tb4.ID");
            sql.Append(whereStr.ToString());
            sql.Append(" order by  tb1." + (string.IsNullOrEmpty(model.OrderStr) ? "ID desc" : model.OrderStr));
            sql.Append(") tt where ROWNUM <= :Limit*:Page) tt2 where tt2.rowno >= (:Page-1)*:Limit");

            return await _dbConnection.QueryAsync<SfcsEquipmentListModel>(sql.ToString(), new { model.CATEGORY, model.STATION_ID, model.NAME, model.CATEGORY_NAME, model.STATION_NAME, model.STATUS, model.ENABLE, model.Limit, model.Page });
        }

        /// <summary>
        /// 获取WHERE条件
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        private string GetWhereStr(SfcsEquipmentRequestModel model)
        {
            StringBuilder whereStr = new StringBuilder();
            whereStr.Append(" where 1=1 ");

            if (!string.IsNullOrEmpty(model.ENABLE))
                whereStr.Append(" and tb1.ENABLE=:ENABLE ");

            if (model.STATUS != null && model.STATUS != -1)
                whereStr.Append(" and tb1.STATUS=:STATUS ");

            if (model.CATEGORY != null && model.CATEGORY != 0)
                whereStr.Append(" and tb1.CATEGORY=:CATEGORY ");

            if (model.STATION_ID != null && model.STATION_ID != 0)
                whereStr.Append(" and tb1.STATION_ID=:STATION_ID ");

            if (!string.IsNullOrEmpty(model.NAME) && model.IS_LIKE == GlobalVariables.NotLike)
                whereStr.Append(" and tb1.NAME=:NAME ");

            if (!string.IsNullOrEmpty(model.NAME) && model.IS_LIKE == GlobalVariables.IsLike)
                whereStr.Append(" and instr(tb1.NAME,:NAME)>0 ");

            if (!string.IsNullOrEmpty(model.STATUS_S))
                whereStr.Append(" and tb1.STATUS in (" + model.STATUS_S + ") ");

            return whereStr.ToString();
        }

        /// <summary>
        /// 获取导出数据
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public async Task<TableDataModel> GetExportData(SfcsEquipmentRequestModel model)
        {
            string conditions = " where 1=1 ";
            if (!string.IsNullOrEmpty(model.ENABLE))
                conditions += @" and tb1.ENABLE=:ENABLE ";

            if (model.STATUS != null && model.STATUS != -1)
                conditions += @" and tb1.STATUS=:STATUS ";

            if (model.CATEGORY != null && model.CATEGORY != 0)
                conditions += @" and tb1.CATEGORY=:CATEGORY ";

            if (model.STATION_ID != null && model.STATION_ID != 0)
                conditions += @" and tb1.STATION_ID=:STATION_ID ";

            if (!string.IsNullOrEmpty(model.NAME))
                conditions += @" and instr(tb1.NAME,:NAME)>0 ";

            if (!string.IsNullOrEmpty(model.STATUS_S))
                conditions += @" and tb1.STATUS in (" + model.STATUS_S + ") ";

            string sql = @" select ROWNUM as rowno , tb1.NAME,TB3.CHINESE AS CATEGORY,tb1.PROPERTY_NO,tb1.PRODUCT_NO,tb1.MODEL,tb4.CHINESE AS USER_PART,
								tb2.LINE_NAME as STATION_ID, tb1.POWER,tb1.VENDOR,tb1.BUY_TIME,tb1.USER_AGE,tb1.END_TIME,tb1.STATUS,tb1.ENABLE  from SFCS_EQUIPMENT tb1
						 inner join(select ID, LINE_NAME from SMT_LINES union select ID, OPERATION_LINE_NAME AS LINE_NAME from SFCS_OPERATION_LINES) tb2 on tb1.STATION_ID = tb2.ID 
						 inner join(select LOOKUP_CODE, CHINESE from SFCS_PARAMETERS  where lookup_type = 'EQUIPMENT_CATEGORY' AND ENABLED = 'Y') tb3 on tb1.CATEGORY = tb3.LOOKUP_CODE 
						 left join(SELECT A.ID,A.CHINESE,B.CHINESE AS SBU_CHINESE,B.ID AS SBU_ID FROM SFCS_LOOKUPS A INNER JOIN SFCS_PARAMETERS B 
						 ON B.ID = A.KIND AND A.ENABLED = 'Y' AND B.LOOKUP_TYPE = 'SBU_CODE' AND B.ENABLED = 'Y') tb4 on tb1.USER_PART = tb4.ID   ";
            string pagedSql = SQLBuilderClass.GetPagedSQL(sql, " tb1.ID desc", conditions);
            var resdata = await _dbConnection.QueryAsync<object>(pagedSql, model);
            string sqlcnt = @"select count(0) from SFCS_EQUIPMENT tb1
						 inner join(select ID, LINE_NAME from SMT_LINES union select ID, OPERATION_LINE_NAME AS LINE_NAME from SFCS_OPERATION_LINES) tb2 on tb1.STATION_ID = tb2.ID 
						 inner join(select LOOKUP_CODE, CHINESE from SFCS_PARAMETERS  where lookup_type = 'EQUIPMENT_CATEGORY' AND ENABLED = 'Y') tb3 on tb1.CATEGORY = tb3.LOOKUP_CODE 
						 left join(SELECT A.ID,A.CHINESE,B.CHINESE AS SBU_CHINESE,B.ID AS SBU_ID FROM SFCS_LOOKUPS A INNER JOIN SFCS_PARAMETERS B 
						 ON B.ID = A.KIND AND A.ENABLED = 'Y' AND B.LOOKUP_TYPE = 'SBU_CODE' AND B.ENABLED = 'Y') tb4 on tb1.USER_PART = tb4.ID  " + conditions;
            int cnt = await _dbConnection.ExecuteScalarAsync<int>(sqlcnt, model);
            return new TableDataModel
            {
                count = cnt,
                data = resdata?.ToList(),
            };

        }

        #region 设备盘点
        /// <summary>
        /// 保存PDA设备盘点数据
        /// </summary>
        /// <param name="model"></param>
        /// <param name="Equipment"></param>
        /// <param name="head"></param>
        /// <returns></returns>
        public async Task<int> SavePDAEquipmentCheckData(SaveEquipmentCheckDataRequestModel model, SfcsEquipment Equipment, SfcsEquipmentKeepHead head, SfcsEquipmentKeepDetail detail)
        {
            int result = 0, headId = 0;
            ConnectionFactory.OpenConnection(_dbConnection);
            using (var tran = _dbConnection.BeginTransaction())
            {
                try
                {
                    if (model.CHECK_CODE.IsNullOrEmpty() && head.IsNullOrWhiteSpace() && detail.IsNullOrWhiteSpace())
                    {
                        headId = QueryEx<int>("SELECT SFCS_EQUIPMENT_KEEP_HEAD_SEQ.NEXTVAL MY_SEQ FROM DUAL").FirstOrDefault();

                        //將序列轉成36進制表示
                        string resultStr = Core.Utilities.RadixConvertPublic.RadixConvert(headId.ToString(), ViewModels.GlobalVariables.DecRadix, ViewModels.GlobalVariables.Base36Redix);

                        //六位表示
                        string ReleasedSequence = resultStr.PadLeft(6, '0');
                        string yymmdd = QueryEx<string>("SELECT TO_CHAR(SYSDATE,'YYMMDD') YYMMDD FROM DUAL ").FirstOrDefault();
                        String EquipmentNo = "EQDJ" + yymmdd + ReleasedSequence;//设备点检编号

                        ////点检月/次
                        //int count = QueryEx<int>("SELECT COUNT(1) FROM SFCS_Equipment_KEEP_HEAD WHERE TO_CHAR(CHECK_START_TIME,'yyyy-MM') = :CHECK_TIME ", new
                        //{
                        //    CHECK_TIME = DateTime.Now.ToString("yyyy-MM")
                        //}).FirstOrDefault();
                        //count += 1;

                        String insertHeadSql = @"INSERT INTO SFCS_EQUIPMENT_KEEP_HEAD(ID, CHECK_CODE, CHECK_STATUS, CHECK_COUNT, CHECK_USER, CHECK_START_TIME, CHECK_REMARKS) VALUES(:ID, :CHECK_CODE, 0, (SELECT COUNT(0) + 1 AS C  FROM SFCS_EQUIPMENT_KEEP_HEAD WHERE TO_CHAR(CHECK_START_TIME,'yyyy-MM') = TO_CHAR(SYSDATE,'yyyy-MM')), :CHECK_USER, SYSDATE, :CHECK_REMARKS)";
                        result = await _dbConnection.ExecuteAsync(insertHeadSql, new
                        {
                            ID = headId,
                            CHECK_CODE = EquipmentNo,
                            CHECK_USER = model.CHECK_USER,
                            CHECK_REMARKS = model.CHECK_REMARKS
                        }, tran);
                        if (result <= 0) { throw new Exception("DATA_ERROR"); }

                        int contentId = QueryEx<int>("SELECT SFCS_EQUIPMENT_KEEP_DETAIL_SEQ.NEXTVAL MY_SEQ FROM DUAL").FirstOrDefault();
                        String insertContentSql = @"INSERT INTO SFCS_EQUIPMENT_KEEP_DETAIL (ID, KEEP_HEAD_ID, EQUIPMENT_ID, EQUIPMENT_STATUS, CHECK_USER, CHECK_TIME, CHECK_REMARKS) VALUES (:ID, :KEEP_HEAD_ID, :EQUIPMENT_ID, :EQUIPMENT_STATUS, :CHECK_USER,SYSDATE,:CHECK_REMARKS)";
                        var keepheadid = headId == 0 ? head.ID : headId;
                        result = await _dbConnection.ExecuteAsync(insertContentSql, new
                        {
                            ID = contentId,
                            KEEP_HEAD_ID = keepheadid,
                            EQUIPMENT_ID = Equipment.ID,
                            EQUIPMENT_STATUS = 1,
                            CHECK_REMARKS = model.CHECK_REMARKS,
                            CHECK_USER = model.CHECK_USER
                        }, tran);
                        if (result <= 0) throw new Exception("DATA_ERROR");

                        tran.Commit();
                    }
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
        public async Task<List<EquipmentInfoInnerKeepDetail>> GetPDAEquipmentCheckDataByHeadID( Decimal headid = 0)
        {
            List<EquipmentInfoInnerKeepDetail> result = new List<EquipmentInfoInnerKeepDetail>();
            try
            {
                string selectEquipmentCheckData = @"SELECT
        			    	INFO.ID INFO_ID,
	                        INFO.NAME,
	                        INFO.CATEGORY,
        			    	{0}
        			    FROM
        			    	SFCS_EQUIPMENT INFO,
	                        SFCS_EQUIPMENT_KEEP_DETAIL KDETAIL 
        			    WHERE
        			    	KDETAIL.EQUIPMENT_ID(+) = INFO.ID 
	                        AND INFO.STATUS != 3 ";
                if (headid == 0)
                {
                    string sql = String.Format(selectEquipmentCheckData, @"'' CHECK_REMARKS,'' CHECK_TIME,'' CHECK_USER,0 KDID,'未盘点' KDETAIL_STATUS ");
                    result = (await _dbConnection.QueryAsync<EquipmentInfoInnerKeepDetail>(sql))?.ToList();
                }
                else
                {
                    selectEquipmentCheckData += " AND KDETAIL.KEEP_HEAD_ID(+)=:ID ";
                    string sql = String.Format(selectEquipmentCheckData, @"KDETAIL.CHECK_REMARKS,TO_CHAR(KDETAIL.CHECK_TIME,'yyyy-MM-dd HH24:mi:ss') CHECK_TIME,KDETAIL.CHECK_USER,NVL(KDETAIL.KEEP_HEAD_ID,:ID) KDID,DECODE(KDETAIL.EQUIPMENT_STATUS,1,'已盘点','未盘点') KDETAIL_STATUS");
                    result = (await _dbConnection.QueryAsync<EquipmentInfoInnerKeepDetail>(sql, new { ID = headid }))?.ToList();
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
        /// 删除PDA设备盘点数据记录
        /// </summary>
        /// <param name = "id" ></ param >
        /// < returns ></ returns >
        public async Task<bool> DeletePDAEquipmentCheckData(Decimal id)
        {
            int result = 0;
            ConnectionFactory.OpenConnection(_dbConnection);
            using (var tran = _dbConnection.BeginTransaction())
            {
                try
                {
                    ///设备点检记录详细表
                    string deleteSql = @"DELETE FROM SFCS_EQUIPMENT_KEEP_DETAIL WHERE KEEP_HEAD_ID = :ID ";
                    result = await _dbConnection.ExecuteAsync(deleteSql, new { ID = id }, tran);

                    ///设备点检记录主表
                    deleteSql = @"DELETE FROM SFCS_EQUIPMENT_KEEP_HEAD WHERE ID = :ID ";
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
        /// 确认PDA设备盘点数据
        /// </summary>
        /// <param name = "model" ></ param >
        /// < returns ></ returns >
        public async Task<bool> ConfirmPDAEquipmentCheckData(AuditEquipmentCheckDataRequestModel model)
        {
            int result = 0;
            ConnectionFactory.OpenConnection(_dbConnection);
            using (var tran = _dbConnection.BeginTransaction())
            {
                try
                {
                    if (model.STATUS == 1)
                    {
                        String updateHeadSql = @"UPDATE SFCS_EQUIPMENT_KEEP_HEAD SET CHECK_STATUS = '1',CHECK_END_TIME = SYSDATE WHERE ID = :ID ";
                        result = await _dbConnection.ExecuteAsync(updateHeadSql, new
                        {
                            ID = model.ID
                        }, tran);
                        if (result <= 0) throw new Exception("DATA_ERROR");
                    }
                    if (model.STATUS == 2)
                    {
                        String updateHeadSql = @"UPDATE SFCS_EQUIPMENT_KEEP_HEAD SET CHECK_STATUS = '2',AUDIT_USER = :AUDIT_USER,AUDIT_REMARKS = :AUDIT_REMARKS,AUDIT_TIME = SYSDATE WHERE ID = :ID ";
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

        public async Task<List<EquipmentCheckListModel>> LoadPDAEquipmentCheckList(EquipmentCheckRequestModel model)
        {
            EquipmentCheckModel fModel = new EquipmentCheckModel();
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
            String sQuery = string.Format("SELECT * FROM (SELECT ROWNUM R, T.* FROM ( SELECT * FROM SFCS_EQUIPMENT_KEEP_HEAD WHERE 1=1 {0} {1}) T) WHERE R BETWEEN :Page AND :Limit", whereStr, orderBy);

            var resdata = await _dbConnection.QueryAsync<EquipmentCheckListModel>(sQuery, fModel);

            List<EquipmentCheckListModel> list = resdata.ToList();
            foreach (var item in list)
            {
                item.CHECK_TIME = item.CHECK_START_TIME.ToString("yyyy-MM-dd");
                item.EQUIPMENT_QTY = QueryEx<int>("SELECT NVL(SUM(KEEP_HEAD_ID),0) FROM SFCS_EQUIPMENT_KEEP_DETAIL WHERE KEEP_HEAD_ID = :KEEP_HEAD_ID ", new
                {
                    KEEP_HEAD_ID = item.ID
                }).FirstOrDefault();
                var EquipmentCheckList = await GetPDAEquipmentCheckDataByHeadID();
                item.CHECK_QTY = (EquipmentCheckList == null || EquipmentCheckList.Count <= 0) ? 0 : EquipmentCheckList.Count;

                item.CHECK_YEAR = item.CHECK_START_TIME.Year.ToString();
                item.CHECK_HEAD = item.CHECK_START_TIME.Month.ToString() + "月第" + item.CHECK_COUNT + "次盘点";
            }

            return list;
        }

        public async Task<int> LoadPDAEquipmentCheckListCount(EquipmentCheckRequestModel model)
        {
            EquipmentCheckModel fModel = new EquipmentCheckModel();

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
            String sQuery = string.Format("SELECT COUNT(1) FROM SFCS_EQUIPMENT_KEEP_HEAD WHERE 1=1 {0}", whereStr);

            int count = await _dbConnection.ExecuteScalarAsync<int>(sQuery, fModel);
            return count;
        }

        #endregion

        #region 设备验证

        /// <summary>
        /// 保存PDA设备验证数据
        /// </summary>
        /// <param name="model"></param>
        /// <param name="Equipment"></param>
        /// <param name="head"></param>
        /// <returns></returns>
        public async Task<int> SavePDAEquipmentValidationData(SaveEquipmentValidationDataRequestModel model, SfcsEquipment Equipment, SfcsEquipmentValidationHead head, SfcsEquipValidationDetail detail)
        {
            int result = 0, headId = 0;
            ConnectionFactory.OpenConnection(_dbConnection);
            using (var tran = _dbConnection.BeginTransaction())
            {
                try
                {
                    if (model.CHECK_CODE.IsNullOrEmpty() && head.IsNullOrWhiteSpace() && detail.IsNullOrWhiteSpace())
                    {
                        headId = QueryEx<int>("SELECT EQUIPMENT_VALIDATION_HEAD_SEQ.NEXTVAL MY_SEQ FROM DUAL").FirstOrDefault();

                        //將序列轉成36進制表示
                        string resultStr = Core.Utilities.RadixConvertPublic.RadixConvert(headId.ToString(), ViewModels.GlobalVariables.DecRadix, ViewModels.GlobalVariables.Base36Redix);

                        //六位表示
                        string ReleasedSequence = resultStr.PadLeft(6, '0');
                        string yymmdd = QueryEx<string>("SELECT TO_CHAR(SYSDATE,'YYMMDD') YYMMDD FROM DUAL ").FirstOrDefault();
                        String EquipmentNo = "EQVL" + yymmdd + ReleasedSequence;//设备验证编号

                        ////点检月/次
                        //int count = QueryEx<int>("SELECT COUNT(1) FROM SFCS_Equipment_KEEP_HEAD WHERE TO_CHAR(CHECK_START_TIME,'yyyy-MM') = :CHECK_TIME ", new
                        //{
                        //    CHECK_TIME = DateTime.Now.ToString("yyyy-MM")
                        //}).FirstOrDefault();
                        //count += 1;

                        String insertHeadSql = @"INSERT INTO SFCS_EQUIPMENT_VALIDATION_HEAD(ID, CHECK_CODE, CHECK_STATUS, CHECK_COUNT, CHECK_USER, CHECK_START_TIME, CHECK_REMARKS) VALUES(:ID, :CHECK_CODE, 0, (SELECT COUNT(0) + 1 AS C  FROM SFCS_EQUIPMENT_VALIDATION_HEAD WHERE TO_CHAR(CHECK_START_TIME,'yyyy-MM') = TO_CHAR(SYSDATE,'yyyy-MM')), :CHECK_USER, SYSDATE, :CHECK_REMARKS)";
                        result = await _dbConnection.ExecuteAsync(insertHeadSql, new
                        {
                            ID = headId,
                            CHECK_CODE = EquipmentNo,
                            CHECK_USER = model.CHECK_USER,
                            CHECK_REMARKS = model.CHECK_REMARKS,
                            //ORGANIZE_ID = Equipment.ORGANIZE_ID
                        }, tran);
                        if (result <= 0) { throw new Exception("DATA_ERROR"); }

                        //String insertDetailSql = @"INSERT INTO SFCS_Equipment_KEEP_DETAIL (ID, KEEP_HEAD_ID, Equipment_TYPE, Equipment_SIZE, Equipment_TYPE_TOTAL, CHECK_QTY) VALUES (:ID, :KEEP_HEAD_ID, :Equipment_TYPE, :Equipment_SIZE,:Equipment_TYPE_TOTAL, :CHECK_QTY)";
                        //foreach (var item in fList)
                        //{
                        //    if (item.Equipment_TYPE == Equipment.FTYPE && item.Equipment_SIZE == Equipment.FSIZE)
                        //    {
                        //        detailId = QueryEx<int>("SELECT SFCS_Equipment_KEEP_DETAIL_SEQ.NEXTVAL MY_SEQ FROM DUAL").FirstOrDefault();
                        //        result = await _dbConnection.ExecuteAsync(insertDetailSql, new
                        //        {
                        //            ID = detailId,
                        //            KEEP_HEAD_ID = headId,
                        //            Equipment_TYPE = Equipment.FTYPE,
                        //            Equipment_SIZE = Equipment.FSIZE,
                        //            Equipment_TYPE_TOTAL = item.Equipment_QTY,
                        //            CHECK_QTY = 1
                        //        }, tran);
                        //    }
                        //    else
                        //    {
                        //        int newid = QueryEx<int>("SELECT SFCS_Equipment_KEEP_DETAIL_SEQ.NEXTVAL MY_SEQ FROM DUAL").FirstOrDefault();
                        //        result = await _dbConnection.ExecuteAsync(insertDetailSql, new
                        //        {
                        //            ID = newid,
                        //            KEEP_HEAD_ID = headId,
                        //            Equipment_TYPE = item.Equipment_TYPE,
                        //            Equipment_SIZE = item.Equipment_SIZE,
                        //            Equipment_TYPE_TOTAL = item.Equipment_QTY,
                        //            CHECK_QTY = 0
                        //        }, tran);
                        //    }
                        //    if (result <= 0) { throw new Exception("DATA_ERROR"); }
                        //}
                        ////获取同类型和尺寸并可用状态的飞达数量
                        //int Equipment_qty = QueryEx<int>("SELECT COUNT(1) FROM SMT_Equipment WHERE STATUS NOT IN (6,7) AND FTYPE = :FTYPE AND FSIZE = :FSIZE AND ORGANIZE_ID = :ORGANIZE_ID ", new
                        //{
                        //    FTYPE = Equipment.FTYPE,
                        //    FSIZE = Equipment.FSIZE,
                        //    ORGANIZE_ID = Equipment.ORGANIZE_ID
                        //}).FirstOrDefault();
                        //if (Equipment_qty <= 0) { throw new Exception("Equipment_CHECK_QTY_ERROR"); }

                        //detailId = QueryEx<int>("SELECT SFCS_Equipment_KEEP_DETAIL_SEQ.NEXTVAL MY_SEQ FROM DUAL").FirstOrDefault();
                        //result = await _dbConnection.ExecuteAsync(insertDetailSql, new
                        //{
                        //    ID = detailId,
                        //    KEEP_HEAD_ID = headId,
                        //    Equipment_TYPE = Equipment.FTYPE,
                        //    Equipment_SIZE = Equipment.FSIZE,
                        //    Equipment_TYPE_TOTAL = Equipment_qty
                        //}, tran);
                        //if (result <= 0) { throw new Exception("DATA_ERROR"); }
                    }
                    //var EquipmentMaintainModel = (await _dbConnection.QueryAsync<SFCSEquipmentMaintainHistory>("SELECT * FROM SFCS_Equipment_MAINTAIN_HISTORY WHERE ID=:ID", new { ID = model.Equipment_MAINTAIN_ID }))?.FirstOrDefault();
                    //0:未验证,1:合格,2:不合格
                    var resutStatus = 1; //EquipmentMaintainModel == null ? 0 : (EquipmentMaintainModel.STATUS ?? 0);
                    int contentId = QueryEx<int>("SELECT EQUIPMENT_VALI_DETAIL_SEQ.NEXTVAL MY_SEQ FROM DUAL").FirstOrDefault();
                    String insertContentSql = @"INSERT INTO SFCS_EQUIP_VALIDATION_DETAIL (ID, VALIDATION_HEAD_ID, EQUIPMENT_ID, EQUIPMENT_STATUS, CHECK_USER, CHECK_TIME, CHECK_REMARKS,EQUIPMENT_MAINTAIN_ID) VALUES (:ID, :VALIDATION_HEAD_ID, :EQUIPMENT_ID, :EQUIPMENT_STATUS, :CHECK_USER,SYSDATE,:CHECK_REMARKS,:EQUIPMENT_MAINTAIN_ID)";
                    var keepheadid = headId == 0 ? head.ID : headId;
                    result = await _dbConnection.ExecuteAsync(insertContentSql, new
                    {
                        ID = contentId,
                        VALIDATION_HEAD_ID = keepheadid,
                        EQUIPMENT_ID = Equipment.ID,
                        EQUIPMENT_STATUS = resutStatus,
                        CHECK_REMARKS = model.CHECK_REMARKS,
                        CHECK_USER = model.CHECK_USER,
                        EQUIPMENT_MAINTAIN_ID = model.EQUIPMENT_MAINTAIN_ID
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

        public async Task<List<EquipmentCheckListModel>> LoadPDAEquipmentValidationList(EquipmentCheckRequestModel model)
        {
            EquipmentCheckModel fModel = new EquipmentCheckModel();
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
            String sQuery = string.Format("SELECT * FROM (SELECT ROWNUM R, T.* FROM ( SELECT * FROM SFCS_EQUIPMENT_VALIDATION_HEAD WHERE 1=1 {0} {1}) T) WHERE R BETWEEN :Page AND :Limit", whereStr, orderBy);

            var resdata = await _dbConnection.QueryAsync<EquipmentCheckListModel>(sQuery, fModel);

            List<EquipmentCheckListModel> list = resdata.ToList();
            foreach (var item in list)
            {
                item.CHECK_TIME = item.CHECK_START_TIME.ToString("yyyy-MM-dd");
                item.EQUIPMENT_QTY = QueryEx<int>("SELECT  NVL(SUM(VALIDATION_HEAD_ID),0) FROM SFCS_EQUIP_VALIDATION_DETAIL WHERE VALIDATION_HEAD_ID = :VALIDATION_HEAD_ID ", new
                {
                    VALIDATION_HEAD_ID = item.ID
                }).FirstOrDefault();
                var EquipmentCheckList = await GetPDAEquipmentValidationDataByHeadID();
                item.CHECK_QTY = (EquipmentCheckList == null || EquipmentCheckList.Count <= 0) ? 0 : EquipmentCheckList.Count;

                item.CHECK_YEAR = item.CHECK_START_TIME.Year.ToString();
                item.CHECK_HEAD = item.CHECK_START_TIME.Month.ToString() + "月第" + item.CHECK_COUNT + "次盘点";
            }

            return list;
        }

        public async Task<int> LoadPDAEquipmentValidationListCount(EquipmentCheckRequestModel model)
        {
            EquipmentCheckModel fModel = new EquipmentCheckModel();

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
            String sQuery = string.Format("SELECT COUNT(1) FROM SFCS_EQUIPMENT_VALIDATION_HEAD WHERE 1=1 {0}", whereStr);

            int count = await _dbConnection.ExecuteScalarAsync<int>(sQuery, fModel);
            return count;
        }

        /// <summary>
        /// 获取验证数据
        /// </summary>
        /// <param name="headid">VALIDATION_HEAD_ID</param>
        /// <returns></returns>
        public async Task<List<EquipmentInfoInnerValidationDetail>> GetPDAEquipmentValidationDataByHeadID( Decimal headid = 0)
        {
            List<EquipmentInfoInnerValidationDetail> result = new List<EquipmentInfoInnerValidationDetail>();
            try
            {
                string selectEquipmentCheckData = @"SELECT
        			    	INFO.ID INFO_ID,
	                        INFO.NAME,
	                        INFO.CATEGORY,
        			    	{0}
        			    FROM
        			    	SFCS_EQUIPMENT INFO,
	                        SFCS_EQUIP_VALIDATION_DETAIL KDETAIL 
        			    WHERE
        			    	KDETAIL.EQUIPMENT_ID(+) = INFO.ID 
	                        AND INFO.STATUS != 3 ";
                if (headid == 0)
                {
                    string sql = String.Format(selectEquipmentCheckData, @"'' CHECK_REMARKS,'' CHECK_TIME,'' CHECK_USER,0 VDID,'未验证' VDETAIL_STATUS ");
                    result = (await _dbConnection.QueryAsync<EquipmentInfoInnerValidationDetail>(sql))?.ToList();
                }
                else
                {
                    selectEquipmentCheckData += " AND KDETAIL.VALIDATION_HEAD_ID(+)=:ID ";
                    //string sql = String.Format(selectEquipmentCheckData, @"KDETAIL.CHECK_REMARKS,TO_CHAR(KDETAIL.CHECK_TIME,'yyyy-MM-dd HH24:mi:ss') CHECK_TIME,KDETAIL.CHECK_USER,KDETAIL.VALIDATION_HEAD_ID VDID,DECODE(KDETAIL.EQUIPMENT_STATUS,1,'合格',2,'不合格','未验证') VDETAIL_STATUS");
                    string sql = String.Format(selectEquipmentCheckData, @"KDETAIL.CHECK_REMARKS,TO_CHAR(KDETAIL.CHECK_TIME,'yyyy-MM-dd HH24:mi:ss') CHECK_TIME,KDETAIL.CHECK_USER,NVL(KDETAIL.VALIDATION_HEAD_ID,:ID) VDID,DECODE(KDETAIL.EQUIPMENT_STATUS,1,'验证','未验证') VDETAIL_STATUS");
                    result = (await _dbConnection.QueryAsync<EquipmentInfoInnerValidationDetail>(sql, new { ID = headid }))?.ToList();
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
        /// 确认PDA设备盘点数据
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public async Task<bool> ConfirmPDAEquipmentValidationData(AuditEquipmentCheckDataRequestModel model)
        {
            int result = 0;
            ConnectionFactory.OpenConnection(_dbConnection);
            using (var tran = _dbConnection.BeginTransaction())
            {
                try
                {
                    if (model.STATUS == 1)
                    {
                        String updateHeadSql = @"UPDATE SFCS_EQUIPMENT_VALIDATION_HEAD SET CHECK_STATUS = '1',CHECK_END_TIME = SYSDATE WHERE ID = :ID ";
                        result = await _dbConnection.ExecuteAsync(updateHeadSql, new
                        {
                            ID = model.ID
                        }, tran);
                        if (result <= 0) throw new Exception("DATA_ERROR");
                    }
                    if (model.STATUS == 2)
                    {
                        String updateHeadSql = @"UPDATE sfcs_equipment_validation_head SET CHECK_STATUS = '2',AUDIT_USER = :AUDIT_USER,AUDIT_REMARKS = :AUDIT_REMARKS,AUDIT_TIME = SYSDATE WHERE ID = :ID ";
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
        /// 删除PDA设备验证数据记录
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<bool> DeletePDAEquipmentValidationData(Decimal id)
        {
            int result = 0;
            ConnectionFactory.OpenConnection(_dbConnection);
            using (var tran = _dbConnection.BeginTransaction())
            {
                try
                {
                    //设备点检记录详细表
                    string deleteSql = @"DELETE FROM SFCS_EQUIP_VALIDATION_DETAIL WHERE VALIDATION_HEAD_ID = :ID ";
                    result = await _dbConnection.ExecuteAsync(deleteSql, new { ID = id }, tran);

                    //设备点检记录主表
                    deleteSql = @"DELETE FROM SFCS_EQUIPMENT_VALIDATION_HEAD WHERE ID = :ID ";
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
        public async Task<bool> QueryPDAEquipmentValidationBy(String check_code, decimal hid)
        {
            int result = 0;
            try
            {
                var querySql = @"SELECT COUNT(*) FROM SFCS_EQUIPMENT_VALIDATION_HEAD HEAD,SFCS_EQUIP_VALIDATION_DETAIL DETAIL,SFCS_EQUIPMENT INFO
                                  WHERE HEAD.ID=DETAIL.VALIDATION_HEAD_ID AND DETAIL.Equipment_ID=INFO.ID AND INFO.NAME=:CODE AND HEAD.ID=:ID AND DETAIL.Equipment_STATUS!=0 ";
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
        /// <param name="EquipmentId"></param>
        /// <param name="user"></param>
        /// <returns></returns>
      //  public async Task<BaseResult> PDAMaintain(SFCSEquipmentMaintainHistory model)
      //  {
      //      var result = new BaseResult();
      //      string editHistorySql = @"UPDATE SFCS_Equipment_MAINTAIN_HISTORY SET OPERATION_ID=:OPERATION_ID,MAINTAIN_USER=:MAINTAIN_USER,
						//END_DATE=SYSDATE,STATUS=:STATUS,REMARK=:REMARK WHERE ID=:ID";
      //      string addHistorySql = @"INSERT INTO SFCS_Equipment_MAINTAIN_HISTORY (ID,Equipment_ID, OPERATION_ID,START_DATE,CREATE_USER,CREATE_DATE,STATUS,REMARK,MAINTAIN_USER,TYPE) 
      //                                                                VALUES(:ID,:Equipment_ID,:OPERATION_ID,SYSDATE,:CREATE_USER,SYSDATE,:STATUS,:REMARK,:MAINTAIN_USER,:TYPE) ";
      //      string addItemsSql = @"INSERT INTO SFCS_Equipment_MAINTAIN_DETAIL VALUES(SFCS_Equipment_MAINTAIN_DTL_SEQ.NEXTVAL,
						//			:MST_ID,:ITEM_ID,:ITEM_STATUS,:REMARK)";

      //      ConnectionFactory.OpenConnection(_dbConnection);
      //      using (var tran = _dbConnection.BeginTransaction())
      //      {
      //          try
      //          {
      //              //获取一个新的操作记录ID
      //              model.OPERATION_ID = await GetOperationSEQ();
      //              model.ID = await Get_SFCS_SEQ_ID("SFCS_Equipment_MAINTAIN_SEQ");

      //              //获取到夹具信息
      //              var Equipment = await GetEquipmentById(model.Equipment_ID);
      //              if (Equipment == null)
      //              {
      //                  tran.Rollback();
      //                  result.ResultCode = 106;
      //                  result.ResultMsg = "找不到对应夹具，请刷新后重试！";
      //                  return result;
      //              }

      //              //插入操作记录信息
      //              SFCSEquipmentOperationHistory data = new SFCSEquipmentOperationHistory
      //              {
      //                  ID = model.OPERATION_ID,
      //                  CREATE_USER = model.MAINTAIN_USER,
      //                  OPERATION_TYPE = 7,
      //                  Equipment_ID = model.Equipment_ID,
      //                  STORE_ID = Equipment.STORE_ID,
      //                  LAST_STATUS = model.STATUS == 1 ? 0 : 5,
      //                  PRE_STATUS = Equipment.STATUS
      //              };

      //              await InsertOperationRecord(data);

      //              //更新夹具状态
      //              //await _dbConnection.ExecuteAsync(editEquipmentSql, new { UPDATE_USER = model.MAINTAIN_USER, ID = model.Equipment_ID, STATUS = (model.STATUS == 1 ? 0 : 5) });

      //              //更新保养记录数据
      //              await _dbConnection.ExecuteAsync(addHistorySql, model);

      //              //插入保养明细数据
      //              foreach (var item in model.DetailList)
      //              {
      //                  item.MST_ID = model.ID;
      //                  await _dbConnection.ExecuteAsync(addItemsSql, new
      //                  {
      //                      MST_ID = item.MST_ID,
      //                      ITEM_ID = item.ITEM_ID,
      //                      ITEM_STATUS = item.ITEM_STATUS,
      //                      REMARK = item.REMARK
      //                  });
      //              }

      //              tran.Commit();
      //              result.ResultCode = 0;
      //              result.ResultMsg = "操作成功！";
      //              result.ResultData = model.ID.ToString();
      //              return result;
      //          }
      //          catch (Exception ex)
      //          {
      //              tran.Rollback();
      //              result.ResultCode = 106;
      //              result.ResultMsg = ex.SFCSsage;
      //              return result;
      //          }
      //          finally
      //          {
      //              if (_dbConnection.State != System.Data.ConnectionState.Closed)
      //              {
      //                  _dbConnection.Close();
      //              }
      //          }
      //      }
      //  }

        #endregion


    }
}