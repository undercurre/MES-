/**
*┌──────────────────────────────────────────────────────────────┐
*│　描    述：巡检主表接口实现                                                    
*│　作    者：嘉志科技                                            
*│　版    本：2.0    模板代码自动生成                                                
*│　创建时间：2019-10-28 13:51:00                             
*└──────────────────────────────────────────────────────────────┘
*┌──────────────────────────────────────────────────────────────┐
*│　命名空间： JZ.IMS.Repository.Oracle                                  
*│　类    名： IpqaMstRepository                                      
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
using JZ.IMS.Core.Extensions;
using System.Linq;
using System.Collections.Generic;

namespace JZ.IMS.Repository.Oracle
{
    public class IpqaMstRepository : BaseRepository<IpqaMst, Decimal>, IIpqaMstRepository
    {
        public IpqaMstRepository(IOptionsSnapshot<DbOption> options)
        {
            _dbOption = options.Get("iWMS");
            if (_dbOption == null)
            {
                throw new ArgumentNullException(nameof(DbOption));
            }
            _dbConnection = ConnectionFactory.CreateConnection(_dbOption.DbType, _dbOption.ConnectionString);
        }

        /// <summary>
        /// 查询列表
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public async Task<TableDataModel> LoadData(IpqaMstRequestModel model)
        {
            DynamicParameters pars = new DynamicParameters();
            string conditions = "where 1 = 1 ";

            if (!model.BUSINESS_UNIT.IsNullOrWhiteSpace())
            {
                pars.Add("BUSINESS_UNIT", model.BUSINESS_UNIT);
                conditions += $"and (BUSINESS_UNIT_ID =:BUSINESS_UNIT) ";
            }

            if (!model.DEPARTMENT.IsNullOrWhiteSpace())
            {
                pars.Add("DEPARTMENT", model.DEPARTMENT);
                conditions += $"and (DEPARTMENT_ID =:DEPARTMENT) ";
            }

            if (!model.U_LINE.IsNullOrWhiteSpace())
            {
                pars.Add("U_LINE", model.U_LINE);
                conditions += $"and (U_LINE_ID =:U_LINE) ";
            }

            if (!model.PRODUCT_NAME.IsNullOrWhiteSpace())
            {
                pars.Add("PRODUCT_NAME", model.PRODUCT_NAME);
                conditions += $"and (instr(PRODUCT_NAME, :PRODUCT_NAME) > 0) ";
            }

            if (!model.IPQA_TYPE.IsNullOrWhiteSpace())
            {
                pars.Add("IPQA_TYPE", model.IPQA_TYPE);
                conditions += $"and (IPQA_TYPE =:IPQA_TYPE) ";
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
                pars.Add("begdate", begdate);
                pars.Add("enddate", enddate);

                conditions += @" and CreateDate between :begdate and :enddate ";
            }

            int cnt = await RecordCountAsync(conditions, pars);
            List<IpqaMst> tmplist = this.GetListPaged(model.Page, model.Limit, conditions, "Id desc", pars).ToList();
            return new TableDataModel
            {
                count = cnt,
                data = tmplist,
            };
        }

        /// <summary>
        /// 获取主表对象
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<IpqaMst> LoadMainAsync(decimal id)
        {
            string sql = @"select * from ipqa_mst where id =:id";
            var resdata = await _dbConnection.QueryAsync<IpqaMst>(sql, new { id });

            if (resdata == null)
            {
                return null;
            }
            return resdata.FirstOrDefault();
        }

        //获取明细列表
        public async Task<List<IpqaDtlViewModel>> LoadDetailAsync(decimal mst_id)
        {
            List<IpqaDtlViewModel> result = null;

            string sql = @"select b.id, b.mst_id, b.order_id, b.ipqa_config_id, b.ipqa_result, b.ipqa_explain, 
					a.category, a.item_name, a.process_require, a.reference_standard, a.quantize_type, a.quantize_val,
				(case when a.quantize_type = 0  then '无量化标准' when a.quantize_type = 1 then '有量化标准' end) quantize_type_caption 
				from ipqa_dtl b inner join ipqa_config a on a.id = b.ipqa_config_id 
				where b.mst_id =:mst_id  ORDER BY b.order_id ASC ";
            var tmpdata = await _dbConnection.QueryAsync<IpqaDtlViewModel>(sql, new { mst_id });

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
        public async Task<List<IpqaConfigVM>> GetIpqaConfigAsync(decimal ipqa_type)
        {
            List<IpqaConfigVM> result = null;

            string sql = @"select id, category, item_name, order_id, process_require, reference_standard, quantize_type,   
					   (case when quantize_type = 0 then '无量化标准' 
　　　　				when quantize_type = 1 then '有量化标准' end) quantize_type_caption, createtime, creator, quantize_val 
				from ipqa_config where enabled = 1 and ipqa_type=:ipqa_type order by order_id asc";
            var tmpdata = await _dbConnection.QueryAsync<IpqaConfigVM>(sql, new { ipqa_type });

            if (tmpdata != null)
            {
                result = tmpdata.ToList();
            }
            return result;
        }

        /// <summary>
        /// 获取获取工单信息列表(分页)
        /// </summary>
        /// <param name="wo_no">工单号</param>
        /// <returns></returns>
        public async Task<TableDataModel> GetWoList(PageModel model)
        {
            DynamicParameters pars = new DynamicParameters();
            string conditions = "where 1 = 1 ";

            if (!model.Key.IsNullOrWhiteSpace())
            {
                pars.Add("wo_no", model.Key);
                conditions += $"and (instr(wo_no, :wo_no) > 0) ";
            }

            //string sql = @"select wo_no, part_no, product_name, model, target_qty, start_date, due_date from v_ims_wo ";
            int cnt = await _dbConnection.RecordCountAsync<V_IMS_WO>(conditions, pars);
            List<V_IMS_WO> tmplist = _dbConnection.GetListPaged<V_IMS_WO>(model.Page, model.Limit, conditions, "wo_no desc", pars).ToList();

            return new TableDataModel
            {
                count = cnt,
                data = tmplist,
            };
        }

        /// <summary>
        /// 获取线别当前在线工单产品信息
        /// </summary>
        /// <param name="lineId"></param>
        /// <returns></returns>
        public async Task<V_IMS_WO> GetPartByLineId(decimal lineId)
        {
            string sql = @"SELECT WO_NO
						  FROM (SELECT LINE_ID, WO_NO
								  FROM SMT_PRODUCTION
								 WHERE FINISHED = 'N'
								UNION
								SELECT LINE_ID, WO_NO
								  FROM SFCS_PRODUCTION
								 WHERE FINISHED = 'N') TAB1
						 WHERE TAB1.LINE_ID = :LINE_ID";

            string wo_no = (await _dbConnection.ExecuteScalarAsync<string>(sql, new { LINE_ID = lineId }));
            if (string.IsNullOrEmpty(wo_no))
                return null;

            return (await _dbConnection.GetListPagedAsync<V_IMS_WO>(1, 10, " where wo_no=:wo_no ", "wo_no desc", new { wo_no = wo_no })).FirstOrDefault();
        }


        // <summary>
        /// 获取表的序列
        /// </summary>
        /// <returns></returns>
        public async Task<decimal> GetSEQID()
        {
            string sql = "SELECT IPQA_MST_SEQ.NEXTVAL MY_SEQ FROM DUAL";
            var result = await _dbConnection.ExecuteScalarAsync(sql);
            return (decimal)result;
        }

        /// <summary>
        /// 保存数据
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public async Task<decimal> SaveDataByTrans(IpqaMstModel model)
        {
            decimal result = 0;
            decimal billid = -1;
            ConnectionFactory.OpenConnection(_dbConnection);
            using (var tran = _dbConnection.BeginTransaction())
            {
                try
                {
                    var result_val = -1;
                    //新增
                    string insertMSTSql = @"INSERT INTO IPQA_MST  
					(ID, BILL_CODE, BUSINESS_UNIT_ID, DEPARTMENT_ID, U_LINE_ID, PRODUCT_NAME, PRODUCT_MODEL, PRODUCT_DATE, PRODUCT_BILLNO, PRODUCT_QTY, CREATEDATE, CREATOR, CREATETIME,IPQA_TYPE,CHECK_STATUS) 
					VALUES (:ID, :BILL_CODE, :BUSINESS_UNIT, :DEPARTMENT, :U_LINE, :PRODUCT_NAME, :PRODUCT_MODEL, :PRODUCT_DATE, :PRODUCT_BILLNO, :PRODUCT_QTY, :CREATEDATE, :CREATOR, :CREATETIME, :IPQA_TYPE,:CHECK_STATUS)";

                    if (model.ID == 0)
                    {
                        billid = await GetSEQID();
                        result_val = await _dbConnection.ExecuteAsync(insertMSTSql, new
                        {
                            ID = billid,
                            model.BILL_CODE,
                            model.BUSINESS_UNIT,
                            model.DEPARTMENT,
                            model.U_LINE,
                            model.PRODUCT_NAME,
                            model.PRODUCT_MODEL,
                            model.PRODUCT_DATE,
                            model.PRODUCT_BILLNO,
                            model.PRODUCT_QTY,
                            model.CREATEDATE,
                            model.CREATOR,
                            model.CREATETIME,
                            model.IPQA_TYPE,
                            model.CHECK_STATUS,
                        }, tran);
                    }
                    else
                    {
                        billid = model.ID;
                        //更新
                        string updateMSTSql = @"Update IPQA_MST set BUSINESS_UNIT_ID =:BUSINESS_UNIT, DEPARTMENT_ID =:DEPARTMENT,
							U_LINE_ID =:U_LINE, PRODUCT_NAME =:PRODUCT_NAME, PRODUCT_MODEL =:PRODUCT_MODEL,
							PRODUCT_DATE =:PRODUCT_DATE,PRODUCT_BILLNO =:PRODUCT_BILLNO, PRODUCT_QTY =:PRODUCT_QTY 
						where ID=:ID ";
                        result_val = await _dbConnection.ExecuteAsync(updateMSTSql, new
                        {
                            model.ID,
                            model.BUSINESS_UNIT,
                            model.DEPARTMENT,
                            model.U_LINE,
                            model.PRODUCT_NAME,
                            model.PRODUCT_MODEL,
                            model.PRODUCT_DATE,
                            model.PRODUCT_BILLNO,
                            model.PRODUCT_QTY
                        }, tran);
                    }

                    if (result_val > 0)
                    {
                        //新增
                        string insertSql = @"INSERT INTO IPQA_DTL   
					(ID, MST_ID, ORDER_ID, IPQA_CONFIG_ID, IPQA_RESULT, IPQA_EXPLAIN) 
					VALUES (IPQA_DTL_SEQ.NEXTVAL, :MST_ID, :ORDER_ID, :IPQA_CONFIG_ID, :IPQA_RESULT, :IPQA_EXPLAIN)";
                        if (model.insertRecords != null && model.insertRecords.Count > 0)
                        {
                            foreach (var item in model.insertRecords)
                            {
                                var resdata = await _dbConnection.ExecuteAsync(insertSql, new
                                {
                                    MST_ID = billid,
                                    item.ORDER_ID,
                                    item.IPQA_CONFIG_ID,
                                    item.IPQA_RESULT,
                                    item.IPQA_EXPLAIN,
                                }, tran);
                            }
                        }
                        //更新
                        string updateSql = @"Update IPQA_DTL set IPQA_RESULT =:IPQA_RESULT, IPQA_EXPLAIN =:IPQA_EXPLAIN where ID=:ID ";
                        if (model.updateRecords != null && model.updateRecords.Count > 0)
                        {
                            foreach (var item in model.updateRecords)
                            {
                                var resdata = await _dbConnection.ExecuteAsync(updateSql, new
                                {
                                    item.ID,
                                    item.IPQA_RESULT,
                                    item.IPQA_EXPLAIN,
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
        /// 获取单据状态
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<decimal> GetBillStatus(decimal id)
        {
            string sql = "select check_status from ipqa_mst where id=:id ";
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
                    string sql = "delete from ipqa_mst where Id =:id";
                    result = await _dbConnection.ExecuteAsync(sql, new { id }, tran);
                    if (result > 0)
                    {
                        sql = "delete from ipqa_dtl where mst_id =:mst_id";
                        await _dbConnection.ExecuteAsync(sql, new { mst_id = id }, tran);
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
            string sql = "update ipqa_mst set check_status=:status, checker =:operator,check_time=:check_time where id=:id";
            return await _dbConnection.ExecuteAsync(sql, new
            {
                status = model.NewStatus,
                model.ID,
                model.Operator,
                check_time = model.OperatorDatetime
            });
        }
    }
}