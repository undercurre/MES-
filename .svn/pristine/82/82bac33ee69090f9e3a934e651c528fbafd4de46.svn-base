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

namespace JZ.IMS.Repository.Oracle
{
    public class SimpleSOP_ROUTESRepository : BaseRepository<SOP_ROUTES, decimal>, ISimpleSOP_ROUTESRepository
    {
        public SimpleSOP_ROUTESRepository(IOptionsSnapshot<DbOption> options)
        {
            _dbOption = options.Get("iWMS");
            if (_dbOption == null)
            {
                throw new ArgumentNullException(nameof(DbOption));
            }
            _dbConnection = ConnectionFactory.CreateConnection(_dbOption.DbType, _dbOption.ConnectionString);
        }

        /// <summary>
        /// 获取所有线别列表
        /// </summary>
        /// <returns></returns>
        public async Task<List<SfcsEquipmentLinesModel>> GetLinesList()
        {
            string sql = "SELECT * FROM (select ID, LINE_NAME from SMT_LINES union select ID,OPERATION_LINE_NAME AS LINE_NAME from SFCS_OPERATION_LINES WHERE ENABLED = 'Y') ORDER BY id";
            return (await _dbConnection.QueryAsync<SfcsEquipmentLinesModel>(sql)).ToList();
        }

        /// <summary>
        /// 获取产线列表
        /// </summary>
        /// <returns></returns>
        public async Task<List<SfcsEquipmentLinesModel>> GetRoHSLinesList()
        {
            string sql = "select ID,OPERATION_LINE_NAME AS LINE_NAME from SFCS_OPERATION_LINES WHERE ENABLED = 'Y' ORDER BY id";
            return (await _dbConnection.QueryAsync<SfcsEquipmentLinesModel>(sql)).ToList();
        }

        /// <summary>
        /// 获取SMT线列表
        /// </summary>
        /// <returns></returns>
        public async Task<List<SfcsEquipmentLinesModel>> GetSMTLinesList()
        {
            string sql = "select ID, LINE_NAME from SMT_LINES ORDER BY id";
            return (await _dbConnection.QueryAsync<SfcsEquipmentLinesModel>(sql)).ToList();
        }

        /// <summary>
        /// 获取激活的工序集合
        /// </summary>
        /// <returns>工序集合（List集合）</returns>
        public async Task<List<SfcsOperationsListModel>> GetEnabledLists()
        {
            string conditions = "WHERE ENABLED = 'Y' AND OPERATION_CLASS = '1' and id <>100 and id <>999 ";//激活的
            conditions += " ORDER BY DESCRIPTION ";
            return (await _dbConnection.GetListAsync<SfcsOperationsListModel>(conditions))?.ToList();
        }

        /// <summary>
        /// 产品图:资源对象
        /// </summary>
        /// <param name="parent_id"></param>
        /// <returns></returns>
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

        /// <summary>
        /// 零件图:资源列表
        /// </summary>
        /// <param name="parent_id"></param>
        /// <returns></returns>
        public async Task<TableDataModel> LoadResourceCmpData(decimal parent_id)
        {
            string sql = @"select tab1.*,TAB3.CODE AS PART_NO,TAB3.NAME AS PART_NAME,TAB3.DESCRIPTION AS PART_DESC,TAB2.PART_QTY,TAB2.PART_LOCATION from SOP_OPERATIONS_ROUTES_RESOURCE tab1
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

        /// <summary>
        /// 作业图:资源列表
        /// </summary>
        /// <param name="parent_id"></param>
        /// <returns></returns>
        public async Task<TableDataModel> LoadResourceData(decimal parent_id)
        {
            string sql = @"select * from SOP_OPERATIONS_ROUTES_RESOURCE where MST_ID =:id ORDER BY ORDER_NO ASC";
            var resdata = await _dbConnection.QueryAsync<SOP_OPERATIONS_ROUTES_RESOURCE>(sql, new { id = parent_id });

            return new TableDataModel
            {
                count = resdata.Count(),
                data = resdata,
            };
        }

        /// <summary>
        /// 获取未处理的安灯呼叫信息
        /// </summary>
        /// <param name="operation_id">工序</param>
        /// <param name="operation_site_id">站点</param>
        /// <returns></returns>
        public async Task<bool> GetCallRecord(decimal operation_id, decimal operation_site_id)
        {
            string sql = @"select count(1) from ANDON_CALL_RECORD where operation_site_id=:SITE_ID and operation_id = :OPERATION_ID and status = 0";

            int count = await _dbConnection.ExecuteScalarAsync<int>(sql, new { OPERATION_ID = operation_id, SITE_ID = operation_site_id });

            return count > 0;
        }

        /// <summary>
        /// 根据线体获取站点信息
        /// </summary>
        /// <param name="lineId"></param>
        /// <returns></returns>
        public async Task<List<SfcsOperationSitesListModel>> GetOperationSiteList(decimal lineId)
        {
            string sql = "select * from SFCS_OPERATION_SITES  where ENABLED = 'Y' AND OPERATION_LINE_ID = :LINE_ID";

            return (await _dbConnection.QueryAsync<SfcsOperationSitesListModel>(sql, new { LINE_ID = lineId })).ToList();
        }

        /// <summary>
        /// 获取激活的工序集合
        /// </summary>
        /// <returns>线体集合</returns>
        public async Task<IEnumerable<SfcsOperations>> GetEnabledListsync()
        {
            string conditions = "WHERE ENABLED = 'Y' AND OPERATION_CLASS = '1' and id <>100 and id <>999 ";//激活的
            conditions += " ORDER BY DESCRIPTION ";
            return await _dbConnection.GetListAsync<SfcsOperations>(conditions);
        }

        /// <summary>
        /// 根据料号获取SOP配置工序
        /// </summary>
        /// <param name="partNo"></param>
        /// <returns></returns>
        public async Task<List<SfcsOperations>> GetOperationsByPartNo(string partNo, string version)
        {
            string sql = @"SELECT O.ID AS ROUTE_OPERATION_ID,R.PART_NO, SO.ID AS Id, SO.OPERATION_NAME
						  FROM SOP_ROUTES R, SOP_OPERATIONS_ROUTES O, SFCS_OPERATIONS SO
						 WHERE R.STATUS = 1 AND R.PART_NO = :PART_NO AND R.ATTRIBUTE1 = :VERSION AND R.ID = O.ROUTE_ID AND O.CURRENT_OPERATION_ID = SO.ID ";

            var data = (await _dbConnection.QueryAsync<SfcsOperations>(sql, new { PART_NO = partNo, VERSION = version })).ToList();
            return data;
        }

        /// <summary>
        /// 根据料号获取对应可用版本号
        /// </summary>
        /// <param name="partNo"></param>
        /// <returns></returns>
        public async Task<List<string>> GetSopVersionsData(string partNo)
        {
            string sql = "SELECT ATTRIBUTE1 FROM SOP_ROUTES WHERE PART_NO = :PART_NO AND STATUS = 1 AND ATTRIBUTE4 = 'Y'";
            var data = (await _dbConnection.QueryAsync<string>(sql, new { PART_NO = partNo })).ToList();
            return data;
        }

        /// <summary>
        /// 根据料号获取对应NP号
        /// </summary>
        /// <param name="partNo"></param>
        /// <returns></returns>
        public async Task<string> GetNpNoByPartNo(string partNo)
        {
            string sql = "SELECT NP_NO FROM MES_PART_NP WHERE ENABLED = 'Y' AND PART_NO = :PART_NO";
            var data = await _dbConnection.ExecuteScalarAsync<string>(sql, new { PART_NO = partNo });
            return data;
        }

        /// <summary>
        /// 导出查询数据
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public async Task<IEnumerable<SOP_ROUTES>> ExportData(SOPRoutesRequestModel model)
        {
            DynamicParameters pars = new DynamicParameters();
            string conditions = "where 1 = 1 ";

            if (!model.PART_NO.IsNullOrWhiteSpace())
            {
                pars.Add("PART_NO", model.PART_NO);
                conditions += $"and (instr(PART_NO, :PART_NO) > 0) ";
            }

            if (!model.ROUTE_NAME.IsNullOrWhiteSpace())
            {
                pars.Add("ROUTE_NAME", model.ROUTE_NAME);
                conditions += $"and (instr(ROUTE_NAME, :ROUTE_NAME) > 0) ";
            }

            if (!model.STATUS.IsNullOrWhiteSpace())
            {
                pars.Add("STATUS", model.STATUS);
                conditions += $"and (STATUS =:STATUS) ";
            }

            if (!model.ATTRIBUTE4.IsNullOrWhiteSpace())
            {
                pars.Add("ATTRIBUTE4", model.ATTRIBUTE4);
                conditions += $"and ATTRIBUTE4=:ATTRIBUTE4 ";
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

                conditions += @" and CREATE_TIME between :begdate and :enddate ";
            }

            var data = await this.GetListAsync(conditions, pars);

            foreach (var item in data)
            {
                if (item.LAST_UPDATE_TIME == DateTime.Parse("0001-01-01 00:00:00"))
                    item.LAST_UPDATE_TIME = null;
                if (item.AUDIT_TIME == DateTime.Parse("0001-01-01 00:00:00"))
                    item.AUDIT_TIME = null;
                if (item.REAUDIT_TIME == DateTime.Parse("0001-01-01 00:00:00"))
                    item.REAUDIT_TIME = null;
            }

            return data;
        }

        /// <summary>
        /// 资源
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
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
        /// 获取激活的工序集合
        /// </summary>
        /// <returns>线体集合</returns>
        public async Task<TableDataModel> GetEnabledListsync(PageModel model)
        {
            string conditions = "WHERE ENABLED = 'Y' AND OPERATION_CLASS = '1' and id <>100 and id <>999 ";//激活的

            if (!model.Key.IsNullOrWhiteSpace())
            {
                conditions += " and instr(DESCRIPTION, :Key) > 0 ";
            }

            conditions += " ORDER BY DESCRIPTION ";
            var resdata = await _dbConnection.GetListAsync<SOP_ROUTES>(conditions, new { Key = model.Key });

            return new TableDataModel
            {
                count = resdata.Count(),
                data = resdata.ToList(),
            };
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
                conditions += $"and (instr(PART_NO, :PART_NO) > 0) ";
            }

            if (!model.ROUTE_NAME.IsNullOrWhiteSpace())
            {
                pars.Add("ROUTE_NAME", model.ROUTE_NAME);
                conditions += $"and (instr(ROUTE_NAME, :ROUTE_NAME) > 0) ";
            }

            if (!model.STATUS.IsNullOrWhiteSpace())
            {
                pars.Add("STATUS", model.STATUS);
                conditions += $"and (STATUS =:STATUS) ";
            }

            if (!model.ATTRIBUTE4.IsNullOrWhiteSpace())
            {
                pars.Add("ATTRIBUTE4", model.ATTRIBUTE4);
                conditions += $"and ATTRIBUTE4=:ATTRIBUTE4 ";
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

                conditions += @" and CREATE_TIME between :begdate and :enddate ";
            }

            int cnt = await RecordCountAsync(conditions, pars);

            var data = this.GetListPaged(model.Page, model.Limit, conditions, "Id desc", pars).ToList();

            foreach (var item in data)
            {
                if (item.LAST_UPDATE_TIME == DateTime.Parse("0001-01-01 00:00:00"))
                    item.LAST_UPDATE_TIME = null;
                if (item.AUDIT_TIME == DateTime.Parse("0001-01-01 00:00:00"))
                    item.AUDIT_TIME = null;
                if (item.REAUDIT_TIME == DateTime.Parse("0001-01-01 00:00:00"))
                    item.REAUDIT_TIME = null;
            }

            return new TableDataModel
            {
                count = cnt,
                data = data
            };
        }

        public async Task<dynamic> LoadDtlData(decimal id)
        {
            string sql = @"select a.ROUTE_ID, a.ORDER_NO, a.STANDARD_HUMAN,a.STANDARD_WORK_FORCE,a.STANDARD_CAPACITY, a.ID, a.current_operation_id, b.description 
				from sop_operations_routes a inner join SFCS_OPERATIONS b on a.current_operation_id = b.id 
				where a.ROUTE_ID =:ROUTE_ID order by a.ORDER_NO ";
            var resdata = await _dbConnection.QueryAsync<SOPOperationsView>(sql, new { ROUTE_ID = id });

            return new TableDataModel
            {
                count = resdata.Count(),
                data = resdata,
            };
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

        /// <summary>
        /// 批量删除资源
        /// </summary>
        /// <param name="bool">是否</param>
        /// <returns></returns>
        public async Task<bool> DeleteResourceBatch(List<decimal> idList)
        {
            var result = false;
            if (idList.Count <= 0)
                return result;

            ConnectionFactory.OpenConnection(_dbConnection);
            using (var tran = _dbConnection.BeginTransaction())
            {
                try
                {
                    foreach (var id in idList)
                    {
                        string partSql = @"DELETE FROM SOP_OPERATIONS_ROUTES_PART WHERE  RESOURCE_ID = :RESOURCE_ID";
                        await _dbConnection.ExecuteAsync(partSql, new { RESOURCE_ID = id }, tran);

                        string resourceSql = "DELETE FROM SOP_OPERATIONS_ROUTES_RESOURCE where Id =:cid";
                        await _dbConnection.ExecuteAsync(resourceSql, new { cid = id }, tran);
                    }
                    tran.Commit();
                    result = true;

                }
                catch (Exception ex)
                {
                    tran.Rollback();
                }
            }
            return result;
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

        public async Task<Boolean> IsExistsAsync(string Name, string attribute1, decimal Id = 0)
        {
            string sql = "select COUNT(*) from SOP_ROUTES where PART_NO=:Name AND ATTRIBUTE1=:ATTRIBUTE1 ";
            if (Id > 0)
                sql += " and Id <> :Id";

            var result = await _dbConnection.ExecuteScalarAsync<int>(sql, new
            {
                Name,
                Id,
                ATTRIBUTE1 = attribute1
            });
            return result > 0;
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
            string sql3 = @"INSERT INTO SOP_OPERATIONS_ROUTES_PART(ID,OPERATIONS_ROUTES_ID,RESOURCE_ID,PART_ID,PART_NO,PART_QTY,PART_LOCATION,CREATEUSER,CREATEDATE) 
				SELECT SOP_OPERATIONS_ROUTES_PART_SEQ.NEXTVAL,:OPERATIONS_ROUTES_ID,:RESOURCE_ID,:PART_ID,:PART_NO,:PART_QTY,:PART_LOCATION,:CREATEUSER,:CREATEDATE FROM DUAL";

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
                        InsertResource(resource);
                    }
                    else
                    {
                        await _dbConnection.ExecuteAsync(sql1, new
                        {
                            cid = resource.ID,
                            msg = resource.RESOURCE_MSG,
                        });
                    }

                    await _dbConnection.ExecuteAsync(sql2, new { RESOURCE_ID = resource.ID });

                    if (partInfo != null && partInfo.PART_NO != null)
                    {
                        partInfo.RESOURCE_ID = resource.ID;
                        await _dbConnection.ExecuteAsync(sql3, partInfo);
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
            sql.Append("WHERE A.PART_NO = :PART_NO AND CURRENT_OPERATION_ID = :OPERATION_ID AND A.STATUS = 1");

            return _dbConnection.Query<SOP_Operations>(sql.ToString(), new { PART_NO = partNo, OPERATION_ID = operationId });
        }

        /// <summary>
        /// 根据成品料号获取BOM料号
        /// </summary>
        /// <param name="partNo">成品料号</param>
        /// <returns></returns>
        public async Task<TableDataModel> GetBomPNByPartNo(SOPRoutesRequestModel model)
        {
            TableDataModel tableDataModel = new TableDataModel();
            string conditions = "";
            if (!model.PART_NO.IsNullOrWhiteSpace())
            {
                conditions += " AND BOM1.PARTENT_CODE = :PART_NO ";
            }
            string sql = $@"	SELECT * FROM
                            (
                            SELECT A.*, ROWNUM RN
                            FROM (SELECT
                            	BOM2.PART_CODE 
                            FROM
                            	SMT_BOM1 BOM1,
                            	SMT_BOM2 BOM2 
                            WHERE
                            	BOM1.BOM_ID = BOM2.BOM_ID 
                            	{conditions} ORDER BY BOM2.PART_CODE ) A
                            WHERE ROWNUM <= :Page*:Limit
                            )
                            WHERE RN > (:Page-1)*:Limit ";

            string cnt = $@"SELECT
                                COUNT(*) 
                            FROM
                                SMT_BOM1 BOM1,
                                SMT_BOM2 BOM2
                            WHERE
                                BOM1.BOM_ID = BOM2.BOM_ID
                                {conditions} ORDER BY BOM2.PART_CODE ";
            tableDataModel.data = (await _dbConnection.QueryAsync<String>(sql.ToString(), model))?.ToList();
            tableDataModel.count = await _dbConnection.ExecuteScalarAsync<int>(cnt, model);
            return tableDataModel;
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
            p.Add(":P_ATTRIBUTE1", model.ATTRIBUTE1, DbType.String, ParameterDirection.Input, 50);
            p.Add(":P_ATTRIBUTE2", model.ATTRIBUTE2, DbType.String, ParameterDirection.Input, 50);
            p.Add(":P_ATTRIBUTE3", model.ATTRIBUTE3, DbType.String, ParameterDirection.Input, 50);
            p.Add(":P_RESULT", 0, DbType.Decimal, ParameterDirection.Output, 20);
            p.Add(":P_MESSAGE", "", DbType.String, ParameterDirection.Output, 50);

            await _dbConnection.ExecuteAsync("PROCESS_SOP_ROUTES_COPY", p, commandType: CommandType.StoredProcedure);
            if (Convert.ToInt32(p.Get<Decimal>(":P_RESULT")) == 0)
            {
                SetStatusByTime();
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
            //string sql = @"SELECT part.*, plm.URL,plm.DOCNAME
            //				  FROM IMS_PART part
            //					   LEFT JOIN jpgdoc_view@PLM plm ON part.code = plm.itemcode
            //				 WHERE code = :CODE";
            string sql = @"SELECT part.*,'' URL, '' DOCNAME
                              FROM IMS_PART part
                             WHERE code = :CODE";
            return (await _dbConnection.QueryAsync<ImsPart>(sql, new { CODE = partNo })).FirstOrDefault();
        }

        /// <summary>
        /// 根据零件料号获取零件信息（包括PLM图片）分页
        /// </summary>
        /// <param name="partNo"></param>
        /// <returns></returns>
        public async Task<TableDataModel> GetPartByPartNo(int pageIndex, int pageSize, string partNo)
        {
            //string sql = @"SELECT part.*, plm.URL,plm.DOCNAME
            //				  FROM IMS_PART part
            //					   LEFT JOIN jpgdoc_view@PLM plm ON part.code = plm.itemcode
            //				 WHERE code = :CODE";
            TableDataModel model = new TableDataModel();
            try
            {
                string condition = "";
                if (!partNo.IsNullOrWhiteSpace())
                {
                    condition = " AND PART.CODE=:CODE ";
                }
                string sql = $@"SELECT U.* FROM     
							( SELECT ROWNUM RNUM,T.* FROM 
								(
                                    select distinct s.* from 
                                    (
                                         SELECT PART.*,'' URL, '' DOCNAME FROM IMS_PART PART
                                         WHERE  PART.CODE NOT IN (SELECT PART_NO FROM SOP_ROUTES) {condition} ORDER BY NAME,DESCRIPTION DESC
                                    ) S
								) T
							 WHERE ROWNUM <= :PAGESIZE*:PAGEINDEX 
							 )U 
                           WHERE U.RNUM > (:PAGEINDEX-1)*:PAGESIZE ";
                string countSql = @"SELECT COUNT(*) FROM IMS_PART PART
                                 WHERE  PART.CODE NOT IN (SELECT PART_NO FROM SOP_ROUTES)  ORDER BY NAME,DESCRIPTION DESC";
                model.data = (await _dbConnection.QueryAsync<ImsPart>(sql, new { PAGEINDEX = pageIndex, PAGESIZE = pageSize, CODE = partNo }))?.ToList();
                model.count = await _dbConnection.ExecuteScalarAsync<int>(countSql);
            }
            catch (Exception ex)
            {
                model.code = -1;
                throw ex;
            }
            return model;

        }

        /// <summary>
        /// 根据生效日期跟失效日期，设置SOP状态
        /// </summary>
        public async void SetStatusByTime()
        {
            try
            {
                await _dbConnection.ExecuteAsync("AUTO_EDIT_SOP_STATUS", null, commandType: CommandType.StoredProcedure);
            }
            catch (Exception ex)
            {
            }
        }

        ///// <summary>
        ///// 获取当前工序资源最大排序下标
        ///// </summary>
        ///// <param name="mst_id"></param>
        ///// <returns></returns>
        //public decimal GetMaxOrderNO(decimal mst_id)
        //{
        //	string sql = "SELECT (TRUNC(NVL(MAX(NVL(ORDER_NO,0)),0)/10)+1)*10 AS MAX_INDEX FROM SOP_OPERATIONS_ROUTES_RESOURCE WHERE MST_ID=:MST_ID";
        //	return _dbConnection.ExecuteScalar<int>(sql, new { MST_ID = mst_id });
        //}

        /// <summary>
        /// 保存资源排序
        /// </summary>
        /// <param name="list"></param>
        /// <returns></returns>
        public async Task<BaseResult> SaveRoutesOrderNo(List<SOP_OPERATIONS_ROUTES_RESOURCE> list)
        {
            BaseResult result = new BaseResult();

            ConnectionFactory.OpenConnection(_dbConnection);
            using (var tran = _dbConnection.BeginTransaction())
            {
                try
                {
                    foreach (var item in list)
                    {
                        await _dbConnection.ExecuteAsync("UPDATE SOP_OPERATIONS_ROUTES_RESOURCE SET ORDER_NO = :ORDER_NO WHERE ID=:ID", new { item.ID, item.ORDER_NO });
                    }
                    tran.Commit();

                    result.ResultCode = 0;
                    result.ResultMsg = "保存成功！";
                }
                catch (Exception ex)
                {
                    tran.Rollback();
                    result.ResultCode = 106;
                    result.ResultMsg = ex.Message;
                }
                finally
                {
                    if (_dbConnection.State != ConnectionState.Closed)
                    {
                        _dbConnection.Close();
                    }
                }
            }

            return result;
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