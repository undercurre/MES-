/**
*┌──────────────────────────────────────────────────────────────┐
*│　描    述：设备点检记录主表接口实现                                                    
*│　作    者：嘉志科技                                            
*│　版    本：2.0    模板代码自动生成                                                
*│　创建时间：2019-10-31 09:31:24                             
*└──────────────────────────────────────────────────────────────┘
*┌──────────────────────────────────────────────────────────────┐
*│　命名空间： JZ.IMS.Repository.Oracle                                  
*│　类    名： SfcsEquipKeepHeadRepository                                      
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
using System.Collections.Generic;
using JZ.IMS.ViewModels;
using System.Linq;
using JZ.IMS.Core.Extensions;

namespace JZ.IMS.Repository.Oracle
{
	public class SfcsEquipKeepRepository : BaseRepository<SfcsEquipKeepHead, Decimal>, ISfcsEquipKeepRepository
	{
		public SfcsEquipKeepRepository(IOptionsSnapshot<DbOption> options)
		{
			_dbOption = options.Get("iWMS");
			if (_dbOption == null)
			{
				throw new ArgumentNullException(nameof(DbOption));
			}
			_dbConnection = ConnectionFactory.CreateConnection(_dbOption.DbType, _dbOption.ConnectionString);
		}


        /// <summary>
		/// 获取用户组织ID
		/// </summary>
		/// <param name="mst_id"></param>
		/// <returns></returns>
		public async Task<decimal> GetOrganizeID(String USER_NAME)
        {
            decimal result = -1;

            string sql = @"select ORGANIZE_ID from SYS_MANAGER WHERE USER_NAME=:USER_NAME";
            result = await _dbConnection.ExecuteScalarAsync<decimal>(sql, new { USER_NAME });

            return result;
        }


		/// <summary>
		/// 获取用户组织ID
		/// </summary>
		/// <param name="mst_id"></param>
		/// <returns></returns>
		public string GetUserNameAndEmpno(String USER_NAME)
        {
            string sql = @"SELECT EMPNO ||'('|| USER_NAME ||')' FROM SYS_USERS WHERE EMPNO = :USER_NAME";
            string result = _dbConnection.ExecuteScalar<string>(sql, new { USER_NAME });
            return result;
        }

        /// <summary>
        /// 查询列表
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public async Task<TableDataModel> LoadData(EquipKeepRequestModel model)
		{

            decimal OrganizeID = await GetOrganizeID(model.USER_NAME);
            model.ORGANIZE_ID = OrganizeID.ToString();


            DynamicParameters param = new DynamicParameters();
			string conditions = "where 1 = 1 ";

			if (!model.KEEP_USER.IsNullOrWhiteSpace())
			{
				param.Add("KEEP_USER", model.KEEP_USER);
				conditions += $" and (instr(KEEP_USER, :KEEP_USER) > 0) ";
			}

			if (model.EQUIP_ID != null)
			{
				param.Add("EQUIP_ID", model.EQUIP_ID);
				conditions += $" and (EQUIP_ID =:EQUIP_ID) ";
			}

			if (model.CATEGORY != null)
			{
				param.Add("CATEGORY", model.CATEGORY);
				conditions += $" and (b.CATEGORY =:CATEGORY) ";
			}

			if (model.STATION_ID != null)
			{
				param.Add("STATION_ID", model.STATION_ID);
				conditions += $" and (b.STATION_ID =:STATION_ID) ";
			}

			if (model.KEEP_TYPE != null)
			{
				param.Add("KEEP_TYPE", model.KEEP_TYPE);
				conditions += $" and (KEEP_TYPE =:KEEP_TYPE) ";
			}

			if (!model.KEEP_CHECK_STATUS.IsNullOrWhiteSpace())
			{
				param.Add("KEEP_CHECK_STATUS", model.KEEP_CHECK_STATUS);
				conditions += $" and (KEEP_CHECK_STATUS =:KEEP_CHECK_STATUS) ";
			}

			if (!model.Key.IsNullOrWhiteSpace())
			{
				param.Add("Key", model.Key);
				conditions += $" and (b.PRODUCT_NO =:Key) ";
			}

			DateTime tempDate;
			DateTime? begdate = null, enddate = null;
			if (!string.IsNullOrEmpty(model.create_begin) && DateTime.TryParse(model.create_begin, out tempDate))
			{
				begdate = tempDate;
			}
			if (!string.IsNullOrEmpty(model.create_end) && DateTime.TryParse(model.create_end, out tempDate))
			{
				enddate = tempDate.AddDays(1);
			}
			if (begdate != null && enddate != null)
			{
				param.Add("begdate", begdate);
				param.Add("enddate", enddate);

				conditions += @" and KEEP_TIME between :begdate and :enddate ";
			}

            if (!model.ORGANIZE_ID.IsNullOrWhiteSpace()&& Convert.ToDecimal(model.ORGANIZE_ID) > 0)
            {
                conditions += @" AND EXISTS
                          (SELECT 1
                          FROM (SELECT ID
                             FROM SYS_ORGANIZE
                             START WITH ID = :ORGANIZE_ID
                             CONNECT BY PRIOR ID = PARENT_ORGANIZE_ID) WHERE ID = a.ORGANIZE_ID)";
                param.Add("ORGANIZE_ID", model.ORGANIZE_ID);
            }

            string RecordCountSql = $"SELECT count(1) FROM SFCS_EQUIP_KEEP_HEAD a " +
				$" left join SFCS_EQUIPMENT b on a.EQUIP_ID = b.ID { conditions} ";
			int cnt = await _dbConnection.ExecuteScalarAsync<int>(RecordCountSql, param);

			string OrderBy = "a.Id desc";
			string PageNumber = model.Page.ToString();
			string RowsPerPage = model.Limit.ToString();
			string PagedSql = $"SELECT * FROM (SELECT ROW_NUMBER() OVER(ORDER BY {OrderBy}) AS PagedNumber, " +
				$" a.ID,a.KEEP_CODE,a.EQUIP_ID,a.KEEP_TYPE, b.NAME EQUIP_Name, b.STATION_ID, b.CATEGORY,b.PRODUCT_NO ,a.KEEP_TIME,a.KEEP_USER,a.KEEP_CHECK_TIME, a.KEEP_CHECKER,a.KEEP_CHECK_STATUS, " +
				$" a.EQUIP_STATUS,a.ORGANIZE_ID FROM SFCS_EQUIP_KEEP_HEAD a left join SFCS_EQUIPMENT b on a.EQUIP_ID = b.ID { conditions}) u " +
				$" WHERE PagedNumber BETWEEN (({PageNumber}-1) * {RowsPerPage} + 1) AND ({PageNumber} * {RowsPerPage})";
			var pagedata = _dbConnection.Query<EquipKeepListModel>(PagedSql, param, null, true);
			return new TableDataModel
			{
				count = cnt,
				data = pagedata.ToList(),
			};
		}

		/// <summary>
		/// 获取主表对象
		/// </summary>
		/// <param name="id"></param>
		/// <returns></returns>
		public async Task<SfcsEquipKeepHead> LoadMainAsync(decimal id)
		{
			string sql = @"select a.*, b.station_id,b.product_no,b.name from sfcs_equip_keep_head a left join sfcs_equipment b on a.equip_id=b.id where a.id =:id";
			var resdata = await _dbConnection.QueryAsync<SfcsEquipKeepHead>(sql, new { id });

			if (resdata == null)
			{
				return null;
			}
			return resdata.FirstOrDefault();
		}

		/// <summary>
		/// 获取明细列表
		/// </summary>
		/// <param name="m_id"></param>
		/// <returns></returns>
		public async Task<List<EquipDtlViewModel>> LoadDetailLine(decimal m_id)
		{
			List<EquipDtlViewModel> result = null;

			string sql = @"select b.id, b.keep_head_id, b.keep_content_id, b.status, b.message, a.order_no, a.keep_content, a.keep_tools,b.auditremarks 
				from sfcs_equip_keep_detail b inner join sfcs_equip_content_conf a on a.id = b.keep_content_id 
				where b.keep_head_id =:m_id  order by a.order_no asc ";
			var tmpdata = await _dbConnection.QueryAsync<EquipDtlViewModel>(sql, new { m_id });

			if (tmpdata != null)
			{
				result = tmpdata.ToList();
			}
			return result;
		}

		/// <summary>
		/// 获取配置列表(带组织架构)
		/// </summary>
		/// <param name="mst_id"></param>
		/// <returns></returns>
		public async Task<List<SfcsEquipContentConf>> GetKeepConfigLineByOrganize(decimal equip_id, decimal keep_type,string user_name)
		{
            decimal OrganizeID = await GetOrganizeID(user_name);
            string ORGANIZE_ID = OrganizeID.ToString();

            List<SfcsEquipContentConf> result = null;

			string sql = @"select a.id, a.keep_content, a.keep_tools, a.order_no    
				from sfcs_equip_content_conf a inner join sfcs_equipment b on a.category_id =b.category 
				where a.ENABLE = 'Y'  AND EXISTS
                          (SELECT 1
                          FROM (SELECT ID
                             FROM SYS_ORGANIZE
                             START WITH ID = :ORGANIZE_ID
                             CONNECT BY PRIOR ID = PARENT_ORGANIZE_ID) WHERE ID = a.ORGANIZE_ID) and a.keep_type =:keep_type and b.id =:equip_id order by order_no asc ";
			var tmpdata = await _dbConnection.QueryAsync<SfcsEquipContentConf>(sql, new { keep_type, equip_id, ORGANIZE_ID = ORGANIZE_ID });

			if (tmpdata != null)
			{
				result = tmpdata.ToList();
			}
			return result;
		}

		/// <summary>
		/// 获取配置列表
		/// </summary>
		/// <param name="mst_id"></param>
		/// <returns></returns>
		public async Task<List<SfcsEquipContentConf>> GetKeepConfigLine(decimal equip_id, decimal keep_type)
		{

			List<SfcsEquipContentConf> result = null;

			string sql = @"select a.id, a.keep_content, a.keep_tools, a.order_no    
				from sfcs_equip_content_conf a inner join sfcs_equipment b on a.category_id =b.category 
				where a.ENABLE = 'Y'  AND  a.keep_type =:keep_type and b.id =:equip_id order by order_no asc ";
			var tmpdata = await _dbConnection.QueryAsync<SfcsEquipContentConf>(sql, new { keep_type, equip_id });

			if (tmpdata != null)
			{
				result = tmpdata.ToList();
			}
			return result;
		}

		/// <summary>
		/// 获取点检作业图
		/// </summary>
		/// <param name="id"></param>
		/// <returns></returns>
		public async Task<List<SOP_OPERATIONS_ROUTES_RESOURCE>> LoadSOPDataync(decimal id)
		{
			List<SOP_OPERATIONS_ROUTES_RESOURCE> result = null;

			string sql = @"select * from sop_operations_routes_resource where resources_category=5 and mst_id =:id order by order_no asc ";
			var tmpdata = await _dbConnection.QueryAsync<SOP_OPERATIONS_ROUTES_RESOURCE>(sql, new { id });

			if (tmpdata != null)
			{
				result = tmpdata.ToList();
			}
			return result;
		}

		/// <summary>
		/// 获取设备列表
		/// </summary>
		/// <param name="mst_id"></param>
		/// <returns></returns>
		public async Task<List<SfcsEquipment>> GetEquipmentAsync()
		{
			List<SfcsEquipment> result = null;

			string sql = @"select ID, NAME, CATEGORY, PRODUCT_NO, STATION_ID,ONLY_CODE From SFCS_EQUIPMENT 
				where ENABLE = 'Y' order by ID asc";
			var tmpdata = await _dbConnection.QueryAsync<SfcsEquipment>(sql, null);

			if (tmpdata != null)
			{
				result = tmpdata.ToList();
			}
			return result;
		}

		/// <summary>
		/// 获取设备状态
		/// </summary>
		/// <param name="mst_id"></param>
		/// <returns></returns>
		public async Task<decimal> GetEquipmentStatusByIdAsync(decimal id)
		{
			decimal result = -1;

			string sql = @"select status from sfcs_equipment where id =:id";
			result = await _dbConnection.ExecuteScalarAsync<decimal>(sql, new { id });

			return result;
		}

		/// <summary>
		/// 获取单据状态
		/// </summary>
		/// <param name="id"></param>
		/// <returns></returns>
		public async Task<decimal> GetBillStatus(decimal id)
		{
			string sql = "select keep_check_status from sfcs_equip_keep_head where id=:id ";
			var result = await _dbConnection.QueryFirstOrDefaultAsync<decimal>(sql, new
			{
				id,
			});
			return result;
		}

		/// <summary>
		/// 删除单据
		/// </summary>
		/// <param name="id"></param>
		/// <returns></returns>
		public async Task<decimal> DeleteBill(decimal id)
		{
			var result = 0;
			ConnectionFactory.OpenConnection(_dbConnection);
			using (var tran = _dbConnection.BeginTransaction())
			{
				try
				{
					string sql = "delete from sfcs_equip_keep_head where Id =:cid";
					result = await _dbConnection.ExecuteAsync(sql, new { cid = id }, tran);
					if (result > 0)
					{
						sql = "delete from sfcs_equip_keep_detail where keep_head_id =:keep_head_id";
						await _dbConnection.ExecuteAsync(sql, new { keep_head_id = id }, tran);
					}
					tran.Commit();
				}
				catch (Exception ex)
				{
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
        /// 更新状态
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public async Task<decimal> UpdateStatusById(BillStatusModel model)
        {
            decimal result = 0;
            ConnectionFactory.OpenConnection(_dbConnection);
            using (var tran = _dbConnection.BeginTransaction())
            {
                try
                {
					string sql = "";

					if (model.EquipStatus.IsNullOrWhiteSpace())
                    {
                        sql = "UPDATE SFCS_EQUIP_KEEP_HEAD SET KEEP_CHECK_STATUS=:STATUS, KEEP_CHECKER =:KEEP_CHECKER,KEEP_CHECK_TIME=SYSDATE WHERE ID=:ID";
                        result = await _dbConnection.ExecuteAsync(sql, new
                        {
                            STATUS = model.NewStatus,
                            ID = model.ID,
                            KEEP_CHECKER = model.Operator
                        }, tran);
                    }
                    else
                    {

                        sql = "UPDATE SFCS_EQUIP_KEEP_HEAD SET KEEP_CHECK_STATUS=:STATUS, KEEP_CHECKER =:KEEP_CHECKER,KEEP_CHECK_TIME=SYSDATE,EQUIP_STATUS=:EQUIP_STATUS WHERE ID=:ID";
                        result = await _dbConnection.ExecuteAsync(sql, new
                        {
                            STATUS = model.NewStatus,
                            ID = model.ID,
                            KEEP_CHECKER = model.Operator,
                            EQUIP_STATUS = model.EquipStatus
                        }, tran);
                    }
                    if (result > 0 && model.NewStatus == 1)
                    {
                        sql = "SELECT EQUIP_ID, EQUIP_STATUS FROM SFCS_EQUIP_KEEP_HEAD WHERE ID =:ID";
                        var resdata = await _dbConnection.QueryAsync<SfcsEquipKeepHead>(sql, new { ID = model.ID });
                        if (resdata != null && resdata.Count() > 0)
                        {
                            var resobj = resdata.FirstOrDefault();
                            string updatesql = @"UPDATE SFCS_EQUIPMENT SET STATUS =:STATUS WHERE ID =:ID";
                            await _dbConnection.ExecuteAsync(updatesql, new
                            {
                                ID = resobj.EQUIP_ID,
                                STATUS = resobj.EQUIP_STATUS,
                            }, tran);
                        }

                        if (model.StatusList != null)
                        {
                            string updateAuditRemarksSql = "UPDATE SFCS_EQUIP_KEEP_DETAIL SET AUDITREMARKS=:AUDITREMARKS WHERE ID=:ID";
                            foreach (var item in model.StatusList)
                            {
                                await _dbConnection.ExecuteAsync(updateAuditRemarksSql, new
                                {
                                    AUDITREMARKS = item.AUDITREMARKS,
                                    ID = item.ID
                                }, tran);
                            }
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
        /// 获取表的序列
        /// </summary>
        /// <returns></returns>
        public async Task<decimal> GetSEQID()
		{
			string sql = "SELECT SFCS_EQUIP_KEEP_HEAD_SEQ.NEXTVAL MY_SEQ FROM DUAL";
			var result = await _dbConnection.ExecuteScalarAsync(sql);
			return (decimal)result;
		}

		/// <summary>
		/// 保存数据
		/// </summary>
		/// <param name="model"></param>
		/// <returns></returns>
		public async Task<decimal> SaveDataByTrans(EquipKeepAddOrModifyModel model)
		{
            decimal OrganizeID = await GetOrganizeID(model.KEEP_USER);
            string ORGANIZE_ID = OrganizeID.ToString();

            decimal result = 0;
			decimal billid = -1;
			ConnectionFactory.OpenConnection(_dbConnection);
			using (var tran = _dbConnection.BeginTransaction())
			{
				try
				{
					var result_val = -1;
					//新增
					string insertMSTSql = @"INSERT INTO SFCS_EQUIP_KEEP_HEAD   
					(id, keep_code, equip_id, keep_time, keep_user, keep_check_status, equip_status, keep_type ,ORGANIZE_ID) 
					VALUES (:ID, :KEEP_CODE, :EQUIP_ID, :KEEP_TIME, :KEEP_USER, :KEEP_CHECK_STATUS, :EQUIP_STATUS, :KEEP_TYPE, :ORGANIZE_ID)";

					if (model.ID == 0)
					{
						billid = await GetSEQID();
						result_val = await _dbConnection.ExecuteAsync(insertMSTSql, new
						{
							ID = billid,
							model.KEEP_CODE,
							model.EQUIP_ID,
                            KEEP_TIME = DateTime.Now,
                            KEEP_USER = GetUserNameAndEmpno(model.KEEP_USER),
							model.KEEP_CHECK_STATUS,
							model.EQUIP_STATUS,
							model.KEEP_TYPE,
                            ORGANIZE_ID = ORGANIZE_ID
                        }, tran);
					}
					else
					{
						billid = model.ID;
						//更新
						string updateMSTSql = @"Update SFCS_EQUIP_KEEP_HEAD set EQUIP_ID =:EQUIP_ID, KEEP_TIME =:KEEP_TIME,
							EQUIP_STATUS =:EQUIP_STATUS, KEEP_TYPE =:KEEP_TYPE 
						where ID=:ID ";
						result_val = await _dbConnection.ExecuteAsync(updateMSTSql, new
						{
                            ID = model.ID,
                            EQUIP_ID = model.EQUIP_ID,
                            KEEP_TIME = DateTime.Now,
                            EQUIP_STATUS = model.EQUIP_STATUS,
                            KEEP_TYPE = model.KEEP_TYPE,
						}, tran);
					}

					if (result_val > 0)
					{
						//新增
						string insertSql = @"INSERT INTO SFCS_EQUIP_KEEP_DETAIL    
					(ID, KEEP_HEAD_ID, KEEP_CONTENT_ID, STATUS, MESSAGE) 
					VALUES (SFCS_EQUIP_KEEP_DETAIL_SEQ.NEXTVAL, :KEEP_HEAD_ID, :KEEP_CONTENT_ID, :STATUS, :MESSAGE)";
						if (model.insertRecords != null && model.insertRecords.Count > 0)
						{
							foreach (var item in model.insertRecords)
							{
								var resdata = await _dbConnection.ExecuteAsync(insertSql, new
								{
									KEEP_HEAD_ID = billid,
                                    KEEP_CONTENT_ID = item.KEEP_CONTENT_ID,
									item.STATUS,
									item.MESSAGE,
								}, tran);
							}
						}
						//更新
						string updateSql = @"Update SFCS_EQUIP_KEEP_DETAIL set STATUS =:STATUS, MESSAGE =:MESSAGE,KEEP_CONTENT_ID=:KEEP_CONTENT_ID where ID=:ID ";
						if (model.updateRecords != null && model.updateRecords.Count > 0)
						{
							foreach (var item in model.updateRecords)
							{
								var resdata = await _dbConnection.ExecuteAsync(updateSql, new
								{
									item.ID,
									item.STATUS,
									item.MESSAGE,
									KEEP_CONTENT_ID=item.KEEP_CONTENT_ID
								}, tran);
							}
						}
					}

					tran.Commit();
					result = billid;
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
		/// 加载月数据
		/// </summary>
		/// <param name="model"></param>
		/// <returns></returns>
		public async Task<TableDataModel> LoadMonthDataAsync(EquipKeepRequestModel model)
		{
			var KEEP_TIME = model.KEEP_BEGDATE.Value.ToString("yyyy-MM");
			var sql = @"SELECT A.KEEP_TIME,D.KEEP_CONTENT,B.NAME,C.STATUS,C.MESSAGE FROM SFCS_EQUIP_KEEP_HEAD A
							left join SFCS_EQUIPMENT B on A.EQUIP_ID = B.ID
							left join SFCS_EQUIP_KEEP_DETAIL C on A.ID = C.KEEP_HEAD_ID
							INNER JOIN SFCS_EQUIP_CONTENT_CONF D ON D.ID = C.KEEP_CONTENT_ID
						where STATION_ID=:STATION_ID AND to_char(KEEP_TIME,'yyyy-mm')=:KEEP_TIME and CATEGORY=:CATEGORY AND KEEP_CHECK_STATUS=1
						ORDER BY NAME, KEEP_TIME,KEEP_CONTENT";
			var data = await _dbConnection.QueryAsync<dynamic>(sql, new { model.STATION_ID, model.CATEGORY, model.ORGANIZE_ID, KEEP_TIME });
			return new TableDataModel
			{
				count = data.ToList().Count(),
				data = data.ToList(),
			};
		}

		/// <summary>
		/// 加载日数据
		/// </summary>
		/// <param name="model"></param>
		/// <returns></returns>
		public async Task<TableDataModel> LoadDayDataAsync(EquipKeepRequestModel model)
		{
			var KEEP_TIME = model.KEEP_BEGDATE.Value.ToString("yyyy-MM-dd");
			var sql = @"SELECT A.KEEP_TIME,D.KEEP_CONTENT,B.NAME,C.STATUS,C.MESSAGE FROM SFCS_EQUIP_KEEP_HEAD A
							left join SFCS_EQUIPMENT B on A.EQUIP_ID = B.ID
							left join SFCS_EQUIP_KEEP_DETAIL C on A.ID = C.KEEP_HEAD_ID
							INNER JOIN SFCS_EQUIP_CONTENT_CONF D ON D.ID = C.KEEP_CONTENT_ID
						where STATION_ID=:STATION_ID AND to_char(KEEP_TIME,'yyyy-mm-dd')=:KEEP_TIME and CATEGORY=:CATEGORY AND KEEP_CHECK_STATUS=1
						ORDER BY NAME, KEEP_TIME,KEEP_CONTENT";
			var data = await _dbConnection.QueryAsync<dynamic>(sql, new { model.STATION_ID, model.CATEGORY, model.ORGANIZE_ID, KEEP_TIME });
			return new TableDataModel
			{
				count = data.ToList().Count(),
				data = data.ToList(),
			};
		}
	}
}