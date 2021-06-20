/**
*┌──────────────────────────────────────────────────────────────┐
*│　描    述：接口实现                                                    
*│　作    者：Admin                                            
*│　版    本：1.0    模板代码自动生成                                                
*│　创建时间：2019-01-05 17:54:04                             
*└──────────────────────────────────────────────────────────────┘
*┌──────────────────────────────────────────────────────────────┐
*│　命名空间： JZ.IMS.Repository.Oracle                                  
*│　类    名： MenuRepository                                      
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
using JZ.IMS.ViewModels;
using JZ.IMS.Core.Extensions;
using JZ.IMS.ViewModels.SOPRoutes;
using JZ.IMS.Models.SOP;
using System.Text;
using System.Collections.Generic;
using System.Data;
using System.Net;
using System.IO;
using Newtonsoft.Json.Linq;
using System.Collections.Specialized;
using JZ.IMS.Core.Helper;

namespace JZ.IMS.Repository.Oracle
{
    public class SOP_ROUTESRepository : BaseRepository<SOP_ROUTES, decimal>, ISOP_ROUTESRepository
    {
        public SOP_ROUTESRepository(IOptionsSnapshot<DbOption> options)
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
        public async Task<TableDataModel> LoadData(SOPRoutesRequestModel model)
        {
            DynamicParameters pars = new DynamicParameters();
            string conditions = "where 1 = 1 ";

            if (!model.PART_NO.IsNullOrWhiteSpace())
            {
                pars.Add("PART_NO", model.PART_NO);
                conditions += $"and (instr(SRS.PART_NO, :PART_NO) > 0) ";
            }

            if (!model.ROUTE_NAME.IsNullOrWhiteSpace())
            {
                pars.Add("ROUTE_NAME", model.ROUTE_NAME);
                conditions += $"and (instr(SRS.ROUTE_NAME, :ROUTE_NAME) > 0) ";
            }

            if (!model.STATUS.IsNullOrWhiteSpace())
            {
                pars.Add("STATUS", model.STATUS);
                conditions += $"and (SRS.STATUS =:STATUS) ";
            }

            DateTime tempDate;
            DateTime? begdate = null, enddate = null;
            if (!string.IsNullOrEmpty(model.create_begin) && DateTime.TryParse(model.create_begin, out tempDate))
            {
                begdate = tempDate;
            }
            if (!string.IsNullOrEmpty(model.create_end) && DateTime.TryParse(model.create_end, out tempDate))
            {
                enddate = tempDate.AddDays(1); //.ToString("yyyy-MM-dd HH:mm:ss");
            }
            if (begdate != null && enddate != null)
            {
                pars.Add("begdate", begdate);
                pars.Add("enddate", enddate);

                conditions += @" and SRS.CREATE_TIME between :begdate and :enddate ";
            }
            if (!string.IsNullOrEmpty(model.ROUTE_NAME_ALIAS))
            {
                conditions += @" and (instr(T.ROUTE_NAME_ALIS,:ROUTE_NAME_ALIAS) >0) ";
            }
            if (!string.IsNullOrEmpty(model.DESCRIPTION))
            {
                conditions += @" and (instr(SRS.DESCRIPTION,:DESCRIPTION) >0) ";
            }
            if (!string.IsNullOrEmpty(model.CREATER))
            {
                conditions += @" and (instr(SRS.CREATER,:CREATER) >0) ";
            }
            String sql = $@"SELECT
                            	U.* 
                            FROM
                            	(
                            	SELECT ROWNUM
                            		rnum,
                            		S.* 
                            	FROM
                            		(
                            		SELECT
                            			SRS.*,
                            			T.ROUTE_NAME_ALIS 
                            		FROM
                            			SOP_ROUTES SRS
                            			LEFT JOIN (
                            			SELECT
                            				RS.ROUTE_NAME ROUTE_NAME_ALIS,
                            				PC.CONFIG_VALUE,
                                            PC.PART_NO
                            			FROM
                            				( SELECT NVL( CONFIG_VALUE, 0 ) CONFIG_VALUE,PART_NO FROM SFCS_PRODUCT_CONFIG WHERE CONFIG_TYPE = 147 ) PC
                            				LEFT JOIN SFCS_ROUTES RS ON RS.id = PC.CONFIG_VALUE 
                            			) T ON T.PART_NO = SRS.PART_NO {conditions} ORDER BY SRS.ID DESC
                            		) S 
                            	WHERE
                            		ROWNUM <= : Limit *: Page 
                            	) U 
                            WHERE
                            	U.rnum > ( : Page - 1 ) *: Limit";
            string sqlcnt = $@"SELECT
                                	COUNT( * ) 
                                FROM
                                	SOP_ROUTES SRS
                                	LEFT JOIN (
                                	SELECT
                                		RS.ROUTE_NAME ROUTE_NAME_ALIS,
                                		PC.CONFIG_VALUE,
                                		PC.PART_NO 
                                	FROM
                                		( SELECT NVL( CONFIG_VALUE, 0 ) CONFIG_VALUE, PART_NO FROM SFCS_PRODUCT_CONFIG WHERE CONFIG_TYPE = 147 ) PC
                                		LEFT JOIN SFCS_ROUTES RS ON RS.id = PC.CONFIG_VALUE 
                                	) T ON T.PART_NO = SRS.PART_NO {conditions} 
                                ORDER BY
                                	SRS.ID DESC";
            int cnt = await _dbConnection.ExecuteScalarAsync<int>(sqlcnt, model);

            return new TableDataModel
            {
                count = cnt,
                //data = this.GetListPaged(model.Page, model.Limit, conditions, "Id desc", pars).ToList(),
                data = await _dbConnection.QueryAsync<SOPRoutesViewListModel>(sql, model),
            };
        }

        public async Task<dynamic> LoadDtlData(decimal id)
        {
            string sql = @"select a.ROUTE_ID, a.ORDER_NO, a.STANDARD_HUMAN,a.STANDARD_WORK_FORCE,a.STANDARD_CAPACITY, a.ID, a.current_operation_id, b.description, b.operation_name  
				from sop_operations_routes a inner join SFCS_OPERATIONS b on a.current_operation_id = b.id 
				where a.ROUTE_ID =:ROUTE_ID order by a.ORDER_NO ";
            var resdata = await _dbConnection.QueryAsync<SOPOperationsView>(sql, new { ROUTE_ID = id });

            return new TableDataModel
            {
                count = resdata.Count(),
                data = resdata,
            };
        }

        //资源
        public async Task<SOP_OPERATIONS_ROUTES_RESOURCE> LoadResourceByID(decimal id)
        {
            string sql = @"select * from SOP_OPERATIONS_ROUTES_RESOURCE where id =:cid";
            var resdata = await _dbConnection.QueryAsync<SOP_OPERATIONS_ROUTES_RESOURCE>(sql, new { cid = id });
            if (resdata == null)
            {
                return null;
            }
            return resdata.ToList().FirstOrDefault();
        }

        //作业图:资源列表
        public async Task<dynamic> LoadResourceData(decimal parent_id)
        {
            string sql = @"select * from SOP_OPERATIONS_ROUTES_RESOURCE where RESOURCES_CATEGORY = 1 and MST_ID =:id ORDER BY ORDER_NO ASC";
            var resdata = await _dbConnection.QueryAsync<SOP_OPERATIONS_ROUTES_RESOURCE>(sql, new { id = parent_id });

            return new TableDataModel
            {
                count = resdata.Count(),
                data = resdata,
            };
        }

        /// <summary>
        /// 根据工序ID和资源名称获取作业图:资源列表
        /// </summary>
        /// <param name="parent_id"></param>
        /// <returns></returns>
        public async Task<dynamic> LoadResourceByIDandName(MenuRequestModel model)
        {
            string conditions = " and 1=1 ";
            if (!model.parentid.IsNullOrWhiteSpace())
            {
                conditions += string.Format(@" and a.MST_ID= {0}", model.parentid);
            }
            if (!model.Name.IsNullOrWhiteSpace())
            {
                conditions += string.Format(@" and (instr(a.RESOURCE_NAME, '{0}') > 0 )", model.Name);
            }

            string sql = @"select ROWNUM as rowno, a.* from SOP_OPERATIONS_ROUTES_RESOURCE a  where a.RESOURCES_CATEGORY = 1 ";
            string pagedSql = SQLBuilderClass.GetPagedSQL(sql, " a.id desc", conditions);
            var resdata = await _dbConnection.QueryAsync<object>(pagedSql, model);

            string sqlcnt = @"select count(0) from SOP_OPERATIONS_ROUTES_RESOURCE a where a.RESOURCES_CATEGORY = 1  " + conditions;
            int cnt = await _dbConnection.ExecuteScalarAsync<int>(sqlcnt, model);


            return new TableDataModel
            {
                count = cnt,
                data = resdata,
            };
        }

        /// <summary>
        /// 根据工序ID和资源名称获取作业图:资源列表
        /// </summary>
        /// <param name="parent_id"></param>
        /// <returns></returns>
        public async Task<bool> UpdateResourceByParentID(MESTransferProcess model)
        {
            var result = false;
            string sql = @" UPDATE SOP_OPERATIONS_ROUTES_RESOURCE SET MST_ID=:MST_ID WHERE ID =:ID AND RESOURCES_CATEGORY = 1 ";

            ConnectionFactory.OpenConnection(_dbConnection);
            using (var tran = _dbConnection.BeginTransaction())
            {
                try
                {
                    foreach (var id in model.ResourceId)
                    {
                        await _dbConnection.ExecuteAsync(sql,new { ID=id, MST_ID =model.ParentId});
                    }
                    tran.Commit();
                    result = true;
                }
                catch {
                    tran.Rollback();
                }
             }
            return result;
        }

        //零件图:资源列表
        public async Task<dynamic> LoadResourceCmpData(decimal parent_id)
        {
            string sql = @"select tab1.*,TAB2.PART_NO AS PART_NO,TAB3.NAME AS PART_NAME,TAB3.DESCRIPTION AS PART_DESC,TAB2.PART_QTY,TAB2.PART_LOCATION,TAB2.IS_SCAN 
			from SOP_OPERATIONS_ROUTES_RESOURCE tab1
			   left join SOP_OPERATIONS_ROUTES_PART tab2 on tab1.ID = TAB2.RESOURCE_ID
			   left join IMS_PART tab3 on tab2.PART_ID = tab3.ID
			where TAB1.RESOURCES_CATEGORY = 2 and TAB1.MST_ID =:id ORDER BY TAB1.ORDER_NO ASC";
            var resdata = await _dbConnection.QueryAsync<SOP_OPERATIONS_ROUTES_RESOURCE>(sql, new { id = parent_id });

            return new TableDataModel
            {
                count = resdata.Count(),
                data = resdata,
            };
        }

        //产品图:资源对象
        public async Task<SOP_OPERATIONS_ROUTES_RESOURCE> LoadResourceProductData(decimal parent_id)
        {
            string sql = @"select * from SOP_OPERATIONS_ROUTES_RESOURCE where RESOURCES_CATEGORY = 0 and MST_ID =:id ORDER BY ORDER_NO ASC";
            var resdata = await _dbConnection.QueryAsync<SOP_OPERATIONS_ROUTES_RESOURCE>(sql, new { id = parent_id });

            if (resdata == null)
            {
                return null;
            }
            return resdata.FirstOrDefault();
        }

        public async Task<decimal> AuditorByIdAsync(ChangeStatusModel model)
        {
            string sql = "update SOP_ROUTES set STATUS=:Status, AUDITOR =:Operator, AUDIT_TIME =:tDate where Id=:Id";
            return await _dbConnection.ExecuteAsync(sql, new
            {
                Status = model.Status ? 1 : 0,
                model.Id,
                model.Operator,
                tDate = model.OperatorDatetime,
            });
        }

        /// <summary>
        /// 判断明细是否存在标准产能为空的数据
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<bool> CheckAudit(decimal id)
        {
            string sql = @" SELECT COUNT(*) FROM SOP_ROUTES A
                      INNER JOIN SOP_OPERATIONS_ROUTES B ON A.ID = B.ROUTE_ID
						WHERE    A.ID=:ID and NVL(B.STANDARD_CAPACITY,0) = 0";

            return (await _dbConnection.ExecuteScalarAsync<int>(sql, new { ID = id })) > 0;
        }

        ////删除工艺路线
        public async Task<decimal> DeleteLogicalAsync(decimal id)
        {
            var result = 0;
            ConnectionFactory.OpenConnection(_dbConnection);
            using (var tran = _dbConnection.BeginTransaction())
            {
                try
                {
                    string sql = "delete from SOP_ROUTES where Id =:cid";
                    result = await _dbConnection.ExecuteAsync(sql, new { cid = id });
                    if (result > 0)
                    {
                        sql = @"delete from SOP_OPERATIONS_ROUTES_RESOURCE a where exists(select 1 from SOP_OPERATIONS_ROUTES b 
							where a.mst_id=b.id and b.route_id=:routeid)";
                        await _dbConnection.ExecuteAsync(sql, new { routeid = id }, tran);
                        sql = "delete from SOP_OPERATIONS_ROUTES where ROUTE_ID =:routeid";
                        await _dbConnection.ExecuteAsync(sql, new { routeid = id }, tran);
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

        //删除工序
        public async Task<decimal> DeleteSubAsync(decimal id)
        {
            var result = 0;
            ConnectionFactory.OpenConnection(_dbConnection);
            using (var tran = _dbConnection.BeginTransaction())
            {
                try
                {
                    string sql = "delete from SOP_OPERATIONS_ROUTES where id =:cid";
                    result = await _dbConnection.ExecuteAsync(sql, new { cid = id }, tran);
                    if (result > 0)
                    {
                        sql = "delete from SOP_OPERATIONS_ROUTES_RESOURCE where MST_ID =:cid";
                        await _dbConnection.ExecuteAsync(sql, new { cid = id }, tran);
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
        /// 删除资源
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<decimal> DeleteResource(decimal id)
        {
            string sql = "delete from SOP_OPERATIONS_ROUTES_RESOURCE where Id =:cid";
            return await _dbConnection.ExecuteAsync(sql, new
            {
                cid = id
            });
        }

        public async Task<Boolean> GetDisplayStatusByIdAsync(decimal id)
        {
            string sql = "select STATUS from sop_routes where Id=:Id ";
            var result = await _dbConnection.QueryFirstOrDefaultAsync<decimal>(sql, new
            {
                Id = id,
            });

            if (result == 1)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        /// <summary>
        /// 校验SOP 物料BOM
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<BaseResult> CheckBomMaterial(decimal id)
        {
            BaseResult result = new BaseResult();
            string sopRouteSql = @"select PART_NO from SOP_ROUTES  where Id =:Id";
            var partNo = _dbConnection.QueryFirstOrDefault<decimal>(sopRouteSql, new
            {
                Id = id,
            });

            String sopMaterialSql = @"select SORP.PART_NO ,sum(SORP.PART_QTY) QTY from  
                SOP_OPERATIONS_ROUTES_PART  SORP,
                SOP_ROUTES SR, 
                SOP_OPERATIONS_ROUTES SOR
                where 
                SORP.OPERATIONS_ROUTES_ID = SOR.ID AND 
                SOR.ROUTE_ID = SR.ID
                AND SR.PART_NO =:PART_NO
                group by SORP.PART_NO
                order by part_no desc";
            List<dynamic> sopMaterialList = _dbConnection.Query(sopMaterialSql, new
            {
                PART_NO = partNo
            }).ToList();

            String bomMaterialSql = @"select ""MaterialID"" as PART_NO , SUM(""QTY"") QTY 
            from ""v_BOM_For_MES""@ERP where ""ProductID"" =:PART_NO
            and ""ScreenPrint"" <> '' AND QTY > 0 group by ""MaterialID""
            order by part_no desc";

            List<dynamic> bomMaterialList = _dbConnection.Query(bomMaterialSql, new
            {
                PART_NO = partNo
            }).ToList();
            if (bomMaterialList.Count != sopMaterialList.Count)
            {
                result.ResultCode = ResultCodeAddMsgKeys.CommonBillisCheckedCode;
                result.ResultMsg = String.Format("BOM中有{0}种零件物料，而SOP中配置了{1}种零件物料!",
                    bomMaterialList.Count,
                    sopMaterialList.Count);
                return result;
            }
            bool flag = true;
            StringBuilder resultMsg = new StringBuilder();
            foreach (dynamic bomMaterial in bomMaterialList)
            {
                dynamic sopMaterial = sopMaterialList.Where(f => f.PART_NO == bomMaterial.PART_NO).FirstOrDefault();
                if (sopMaterial == null)
                {
                    flag = false;
                    resultMsg.Append(String.Format("BOM中的零件物料{0}在SOP中没有配置,", bomMaterial.PART_NO));
                }
                else if (sopMaterial.QTY != bomMaterial.QTY)
                {
                    flag = false;
                    resultMsg.Append(String.Format("BOM中的零件物料{0}的数量是{1}在SOP中配置的数量是{2},", bomMaterial.PART_NO, bomMaterial.QTY, sopMaterial.QTY));
                }
            }
            if (!flag)
            {
                result.ResultCode = ResultCodeAddMsgKeys.CommonBillisCheckedCode;
                result.ResultMsg = resultMsg.ToString().TrimEnd(',');
                return result;
            }
            else
            {
                result.ResultCode = ResultCodeAddMsgKeys.CommonObjectSuccessCode;
                return result;
            }
        }

        public bool GetDisplayStatusById(decimal id)
        {
            string sql = "select STATUS from sop_routes where Id=:Id ";
            var result = _dbConnection.QueryFirstOrDefault<decimal>(sql, new
            {
                Id = id,
            });

            if (result == 1)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public async Task<Boolean> IsExistsNameAsync(string Name)
        {
            string sql = "select Id from SOP_ROUTES where PART_NO=:Name";
            var result = await _dbConnection.QueryAsync<decimal>(sql, new
            {
                Name = Name,
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
            string sql = "select Id from SOP_ROUTES where PART_NO=:Name and Id <> :Id ";
            var result = await _dbConnection.QueryAsync<decimal>(sql, new
            {
                Name = Name,
                Id = Id,
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

        public async Task<decimal> GetSEQIDAsync()
        {
            string sql = "SELECT SOP_ROUTES_SEQ.NEXTVAL MY_SEQ FROM DUAL";
            var result = await _dbConnection.ExecuteScalarAsync(sql);
            return (decimal)result;
        }

        public decimal Get_Detail_SEQID()
        {
            string sql = "SELECT SOP_OPERATIONS_ROUTES_SEQ.NEXTVAL MY_SEQ FROM DUAL";
            var result = _dbConnection.ExecuteScalar(sql);
            return (decimal)result;
        }

        public decimal Get_Resource_SEQID()
        {
            string sql = "SELECT sop_operations_routes_res_seq.NEXTVAL MY_SEQ FROM DUAL";
            var result = _dbConnection.ExecuteScalar(sql);
            return (decimal)result;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public decimal InsertByTrans(SOP_ROUTES model)
        {
            ConnectionFactory.OpenConnection(_dbConnection);

            decimal rountId = 0;
            string insertSql = @"INSERT INTO SOP_OPERATIONS_ROUTES 
					(ID, ROUTE_ID, CURRENT_OPERATION_ID, PREVIOUS_OPERATION_ID, NEXT_OPERATION_ID, ORDER_NO, STANDARD_HUMAN, STANDARD_WORK_FORCE, STANDARD_CAPACITY) 
					VALUES (:ID, :ROUTE_ID, :CURRENT_OPERATION_ID,0, 0, :ORDER_NO, :STANDARD_HUMAN, :STANDARD_WORK_FORCE, :STANDARD_CAPACITY)";
            using (var tran = _dbConnection.BeginTransaction())
            {
                try
                {
                    rountId = _dbConnection.Insert<decimal, SOP_ROUTES>(model, tran);

                    if (rountId > 0 && model.OperationList?.Count() > 0)
                    {
                        foreach (var item in model.OperationList)
                        {
                            var curid = Get_Detail_SEQID();
                            _dbConnection.Execute(insertSql, new
                            {
                                ID = curid,
                                ROUTE_ID = rountId,
                                item.CURRENT_OPERATION_ID,
                                item.ORDER_NO,
                                item.STANDARD_HUMAN,
                                item.STANDARD_WORK_FORCE,
                                item.STANDARD_CAPACITY
                            }, tran);
                        }
                    }

                    if (model.Resource != null)
                    {
                        InsertResource(model.Resource);
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
            return rountId;
        }

        /// <summary>
        /// 更新资源说明
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<decimal> UnpdateResourceMsg(SOP_OPERATIONS_ROUTES_RESOURCE resource, SopOperationsRoutesPartAddOrModifyModel partInfo = null)
        {
            string sql1 = "update SOP_OPERATIONS_ROUTES_RESOURCE set RESOURCE_MSG =:msg where Id =:cid";
            string sql2 = "DELETE FROM SOP_OPERATIONS_ROUTES_PART WHERE RESOURCE_ID = :RESOURCE_ID";
            string sql3 = @"INSERT INTO SOP_OPERATIONS_ROUTES_PART(ID,OPERATIONS_ROUTES_ID,RESOURCE_ID,PART_ID,PART_NO,PART_QTY,PART_LOCATION,CREATEUSER,CREATEDATE,IS_SCAN) 
				SELECT SOP_OPERATIONS_ROUTES_PART_SEQ.NEXTVAL,:OPERATIONS_ROUTES_ID,:RESOURCE_ID,:PART_ID,:PART_NO,:PART_QTY,:PART_LOCATION,:CREATEUSER,:CREATEDATE,:IS_SCAN FROM DUAL";

            ConnectionFactory.OpenConnection(_dbConnection);
            using (var tran = _dbConnection.BeginTransaction())
            {
                try
                {
                    if (resource.ID == 0)
                    {
                        decimal res_id = Get_Resource_SEQID();
                        resource.ID = res_id;
                        resource.ORDER_NO = res_id;
                        resource.RESOURCES_CATEGORY = 2;
                        _dbConnection.Insert<decimal, SOP_OPERATIONS_ROUTES_RESOURCE>(resource, tran);
                    }
                    else
                    {
                        await _dbConnection.ExecuteAsync(sql1, new
                        {
                            cid = resource.ID,
                            msg = resource.RESOURCE_MSG,
                        }, tran);
                    }

                    await _dbConnection.ExecuteAsync(sql2, new { RESOURCE_ID = resource.ID }, tran);

                    if (partInfo != null && partInfo.PART_NO != null)
                    {
                        partInfo.RESOURCE_ID = resource.ID;
                        await _dbConnection.ExecuteAsync(sql3, partInfo, tran);
                    }

                    tran.Commit();
                    return 1;
                }
                catch
                {
                    tran.Rollback(); // 回滚事务
                    throw;
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

        public decimal InsertDetail(SOP_OPERATIONS_ROUTES model)
        {
            decimal result = 0;
            try
            {
                result = _dbConnection.Insert<decimal, SOP_OPERATIONS_ROUTES>(model);

            }
            catch (Exception ex)
            {
                throw ex;
            }
            return result;
        }

        public decimal InsertResource(SOP_OPERATIONS_ROUTES_RESOURCE model)
        {
            decimal result = 0;
            try
            {
                result = _dbConnection.Insert<decimal, SOP_OPERATIONS_ROUTES_RESOURCE>(model);

            }
            catch (Exception ex)
            {
                throw ex;
            }
            return result;
        }

        public decimal UpdateResourceByID(SOP_OPERATIONS_ROUTES_RESOURCE model)
        {
            string sql = @"update SOP_OPERATIONS_ROUTES_RESOURCE set RESOURCE_URL =:RESOURCE_URL, RESOURCE_NAME =:RESOURCE_NAME, 
							RESOURCE_SIZE =:RESOURCE_SIZE where Id =:id";
            return _dbConnection.Execute(sql, new
            {
                id = model.ID,
                model.RESOURCE_URL,
                model.RESOURCE_NAME,
                model.RESOURCE_SIZE,
            });
        }

        /// <summary>
        /// 对接文控系统
        /// </summary>
        /// <param name="token"></param>
        /// <param name="specs"></param>
        /// <returns></returns>
        public async Task<DocumentSystemViewModel> GetDocmentSystemData(string token = "", string specs = "")
        {
            var model = new DocumentSystemViewModel();
            try
            {
                //查找生产字典 找url和token
                string sql = "SELECT MEANING,DESCRIPTION  FROM SFCS_PARAMETERS WHERE LOOKUP_TYPE='MES_UT_DOCUMENT' AND ENABLED='Y'";
                var paramsModel = (await _dbConnection.QueryAsync<SfcsParameters>(sql))?.ToList();
                if (paramsModel.Count <= 0)
                {
                    return null;
                }
                var selectURLModel = paramsModel.Where(c => c.MEANING.Equals("url")).FirstOrDefault();
                var selectTokenModel = paramsModel.Where(c => c.MEANING.Equals("url")).FirstOrDefault();

                string urlStr = selectURLModel == null ? "" : selectURLModel.DESCRIPTION.Trim();
                string tokenStr = selectTokenModel == null ? "" : selectTokenModel.DESCRIPTION.Trim();

                //请求头设置
                HttpWebRequest req = (HttpWebRequest)HttpWebRequest.Create(urlStr);
                req.Method = "POST";
                req.ContentType = "application/x-www-form-urlencoded";
                string content = $"token={tokenStr}&specs={specs}";
                //提交字节
                byte[] bs = Encoding.UTF8.GetBytes(content);
                req.ContentLength = bs.Length;
                //提交请求数据
                Stream reqStream = req.GetRequestStream();
                reqStream.Write(bs, 0, bs.Length);
                reqStream.Close();
                //接收返回的页面，必须的，不能省略

                using (WebResponse wr = req.GetResponse())
                {
                    using (System.IO.Stream respStream = wr.GetResponseStream())
                    {

                        using (System.IO.StreamReader reader = new System.IO.StreamReader(respStream, System.Text.Encoding.GetEncoding("utf-8")))
                        {
                            string responStr = reader.ReadToEnd();
                            model = JsonHelper.JSONToObject<DocumentSystemViewModel>(responStr);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return model;
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public decimal UpdateByTrans(SOP_ROUTES model)
        {
            ConnectionFactory.OpenConnection(_dbConnection);
            int result = 0;
            string insertSql = @"INSERT INTO SOP_OPERATIONS_ROUTES 
					(ID, ROUTE_ID, CURRENT_OPERATION_ID, PREVIOUS_OPERATION_ID, NEXT_OPERATION_ID, ORDER_NO, STANDARD_HUMAN, STANDARD_WORK_FORCE, STANDARD_CAPACITY) 
					VALUES (:ID, :ROUTE_ID, :CURRENT_OPERATION_ID,0, 0, :ORDER_NO, :STANDARD_HUMAN, :STANDARD_WORK_FORCE, :STANDARD_CAPACITY)";
            using (var tran = _dbConnection.BeginTransaction())
            {
                try
                {
                    result = _dbConnection.Update(model, tran);
                    if (result > 0 && model.OperationList != null)
                    {
                        //新增
                        var adddata = model.OperationList.Where(t => t.ID == 0).ToList();
                        if (adddata != null && adddata.Count() > 0)
                        {
                            foreach (var item in adddata)
                            {
                                var curid = Get_Detail_SEQID();
                                _dbConnection.Execute(insertSql, new
                                {
                                    ID = curid,
                                    ROUTE_ID = model.ID,
                                    item.CURRENT_OPERATION_ID,
                                    item.ORDER_NO,
                                    item.STANDARD_HUMAN,
                                    item.STANDARD_WORK_FORCE,
                                    item.STANDARD_CAPACITY
                                }, tran);
                            }
                        }
                        //更新排序
                        var updata = model.OperationList.Where(t => t.ID > 0).ToList();
                        string updateSql = @"Update SOP_OPERATIONS_ROUTES set ORDER_NO =:ORDER_NO, STANDARD_HUMAN =:STANDARD_HUMAN,
							STANDARD_WORK_FORCE =:STANDARD_WORK_FORCE, STANDARD_CAPACITY =:STANDARD_CAPACITY where ID=:ID ";
                        if (updata != null && updata.Count() > 0)
                        {
                            foreach (var item in updata)
                            {
                                _dbConnection.Execute(updateSql, new
                                {
                                    item.ID,
                                    item.ORDER_NO,
                                    item.STANDARD_HUMAN,
                                    item.STANDARD_WORK_FORCE,
                                    item.STANDARD_CAPACITY
                                }, tran);
                            }
                        }
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
        /// 通过料号+工序查找制程的工序信息
        /// </summary>
        /// <param name="partNo"></param>
        /// <param name="operationId"></param>
        /// <returns></returns>
        public IEnumerable<SOP_Operations> getOperationsRoutes(string partNo, decimal operationId)
        {
            StringBuilder sql = new StringBuilder();
            sql.Append("SELECT B.* FROM SOP_ROUTES A ");
            sql.Append("INNER JOIN SOP_OPERATIONS_ROUTES B ON A.ID = B.ROUTE_ID ");
            sql.Append("WHERE A.PART_NO = :PART_NO AND CURRENT_OPERATION_ID = :OPERATION_ID AND A.STATUS = 1 ");

            return _dbConnection.Query<SOP_Operations>(sql.ToString(), new { PART_NO = partNo, OPERATION_ID = operationId });
        }

        public IEnumerable<SOP_Operations> getOperationsRoutesPreview(string partNo, decimal operationId)
        {
            StringBuilder sql = new StringBuilder();
            sql.Append("SELECT B.* FROM SOP_ROUTES A ");
            sql.Append("INNER JOIN SOP_OPERATIONS_ROUTES B ON A.ID = B.ROUTE_ID ");
            sql.Append("WHERE A.PART_NO = :PART_NO AND CURRENT_OPERATION_ID = :OPERATION_ID ");

            return _dbConnection.Query<SOP_Operations>(sql.ToString(), new { PART_NO = partNo, OPERATION_ID = operationId });
        }

        /// <summary>
        /// SOP复制 执行方法
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public async Task<int> SOPCopyAsync(SOPCopyRequestModel model)
        {
            var p = new DynamicParameters();
            p.Add(":P_SOP_ID", model.ID, DbType.Decimal, ParameterDirection.Input, 20);
            p.Add(":P_PART_NO", model.PART_NO_NEW, DbType.String, ParameterDirection.Input, 20);
            p.Add(":P_ROUTE_NAME", model.ROUTE_NAME_NEW, DbType.String, ParameterDirection.Input, 200);
            p.Add(":P_DESCRIPTION", model.DESCRIPTION_NEW, DbType.String, ParameterDirection.Input, 60);
            p.Add(":P_CREATER", model.CREATER, DbType.String, ParameterDirection.Input, 50);
            p.Add(":P_RESULT", 0, DbType.Decimal, ParameterDirection.Output, 20);
            p.Add(":P_MESSAGE", "", DbType.String, ParameterDirection.Output, 50);

            await _dbConnection.ExecuteAsync("PROCESS_SOP_ROUTES_COPY", p, commandType: CommandType.StoredProcedure);
            if (Convert.ToInt32(p.Get<Decimal>(":P_RESULT")) == 0)
            {
                return 1;
            }
            else
            {
                return 0;
            }
        }

        /// <summary>
        /// SOP复制 执行方法(新-2020-9-1)
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public async Task<int> SOPCopyAsyncNEW(SOPCopyRequestModel model)
        {
            var p = new DynamicParameters();
            p.Add(":P_SOP_ID", model.ID, DbType.Decimal, ParameterDirection.Input, 20);
            p.Add(":P_PART_NO", model.PART_NO_NEW, DbType.String, ParameterDirection.Input, 20);
            p.Add(":P_ROUTE_NAME", model.ROUTE_NAME_NEW, DbType.String, ParameterDirection.Input, 200);
            p.Add(":P_DESCRIPTION", model.DESCRIPTION_NEW, DbType.String, ParameterDirection.Input, 60);
            p.Add(":P_CREATER", model.CREATER, DbType.String, ParameterDirection.Input, 50);
            p.Add(":P_RESULT", 0, DbType.Decimal, ParameterDirection.Output, 20);
            p.Add(":P_MESSAGE", "", DbType.String, ParameterDirection.Output, 50);

            await _dbConnection.ExecuteAsync("PROCESS_SOP_ROUTES_COPY_NEW", p, commandType: CommandType.StoredProcedure);
            if (Convert.ToInt32(p.Get<Decimal>(":P_RESULT")) == 0)
            {
                return 1;
            }
            else
            {
                return 0;
            }
        }

        /// <summary>
        /// 获取设备图片
        /// </summary>
        /// <returns></returns>
        public List<SOP_OPERATIONS_ROUTES_RESOURCE> GetEquipmentRoutes()
        {
            string sql = "select ID,MST_ID,RESOURCE_URL,RESOURCES_CATEGORY,RESOURCE_NAME,RESOURCE_SIZE from SOP_OPERATIONS_ROUTES_RESOURCE WHERE RESOURCES_CATEGORY = 4";
            return _dbConnection.Query<SOP_OPERATIONS_ROUTES_RESOURCE>(sql).ToList();
        }

        public int DelRoutesByMatId(decimal mstId)
        {
            string sql = "delete SOP_OPERATIONS_ROUTES_RESOURCE where MST_ID=:MST_ID and RESOURCES_CATEGORY = 4";
            return _dbConnection.Execute(sql, new
            {
                MST_ID = mstId
            });
        }

        /// <summary>
        /// 获取工序评判标准
        /// </summary>
        /// <param name="site_id">站点ID</param>
        /// <returns></returns>
        public async Task<IEnumerable<SopSkillStandardListModel>> LoadSkillStandard(decimal site_id)
        {
            string sql = "select tb1.* from SOP_SKILL_STANDARD tb1 inner join SFCS_OPERATION_SITES tb2 on tb1.OPERATION_ID = tb2.OPERATION_ID and tb2.ID = :site_id";
            return await _dbConnection.QueryAsync<SopSkillStandardListModel>(sql, new
            {
                site_id
            });
        }

        /// <summary>
        /// 根据零件料号获取零件信息（包括PLM图片）
        /// </summary>
        /// <param name="partNo"></param>
        /// <returns></returns>
        public async Task<ImsPart> GetPartByPartNo(string partNo)
        {
            string sql = @"SELECT part.*,'' URL, '' DOCNAME
                              FROM IMS_PART part
                             WHERE code = :CODE";

            return (await _dbConnection.QueryAsync<ImsPart>(sql, new { CODE = partNo })).FirstOrDefault();
        }

        /// <summary>
        /// 根据零件图片ID获取零件信息
        /// </summary>
        /// <param name="sourceId"></param>
        /// <returns></returns>
        public async Task<SopOperationsRoutesPartListModel> GetSourcePartBySourceId(string sourceId)
        {
            string sql = "SELECT a.*, b.NAME,b.DESCRIPTION FROM SOP_OPERATIONS_ROUTES_PART a " +
                "left join IMS_PART b on a.PART_ID = b.ID  WHERE a.RESOURCE_ID = :RESOURCE_ID";

            return (await _dbConnection.QueryAsync<SopOperationsRoutesPartListModel>(sql, new { RESOURCE_ID = sourceId })).FirstOrDefault();
        }

        /// <summary>
        /// 根据零件图片ID删除零件信息
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        public async Task<int> DeleteResourcePart(decimal Id)
        {
            string sql = @"DELETE FROM SOP_OPERATIONS_ROUTES_PART WHERE  RESOURCE_ID = :RESOURCE_ID";

            return await _dbConnection.ExecuteAsync(sql, new { RESOURCE_ID = Id });
        }

        #region 设备点检事项

        /// <summary>
        /// 获取设备点检事项图片
        /// </summary>
        /// <param name="mstId"></param>
        /// <returns></returns>
        public async Task<IEnumerable<SOP_OPERATIONS_ROUTES_RESOURCE>> GetEquipContentConfResource(decimal mstId)
        {
            string sql = "SELECT * from SOP_OPERATIONS_ROUTES_RESOURCE WHERE MST_ID = :MST_ID AND RESOURCES_CATEGORY = 5 ORDER BY ORDER_NO ASC, ID ASC";
            return await _dbConnection.QueryAsync<SOP_OPERATIONS_ROUTES_RESOURCE>(sql, new
            {
                MST_ID = mstId
            });
        }

        /// <summary>
        /// 删除设备点检事项图片
        /// </summary>
        /// <param name="mstId"></param>
        /// <returns></returns>
        public async Task<int> DelEquipContentConfResource(decimal mstId)
        {
            string sql = "DELETE SOP_OPERATIONS_ROUTES_RESOURCE WHERE MST_ID = :MST_ID AND RESOURCES_CATEGORY = 5";
            return await _dbConnection.ExecuteAsync(sql, new
            {
                MST_ID = mstId
            });
        }

        #endregion

    }
}