/*
*┌──────────────────────────────────────────────────────────────┐
*│　描    述：接口实现                                                    
*│　作    者：嘉志科技                                            
*│　版    本：2.0    模板代码自动生成                                                
*│　创建时间：2020-09-23 11:43:12                             
*└──────────────────────────────────────────────────────────────┘
*┌──────────────────────────────────────────────────────────────┐
*│　命名空间： JZ.IMS.Repository.Oracle                                  
*│　类    名： SfcsEcndocRepository                                      
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
using JZ.IMS.Core.Extensions;

namespace JZ.IMS.Repository.Oracle
{
	public class SfcsEcndocRepository : BaseRepository<SfcsEcndoc, Decimal>, ISfcsEcndocRepository
	{
		public SfcsEcndocRepository(IOptionsSnapshot<DbOption> options)
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
			string sql = "SELECT ENABLED FROM SFCS_ECNDOC WHERE ID=:ID";
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
			string sql = "UPDATE SFCS_ECNDOC set ENABLED=:ENABLED WHERE ID=:Id";
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
			string sql = "SELECT SFCS_ECNDOC_SEQ.NEXTVAL MY_SEQ FROM DUAL";
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
			string sql = "select count(0) from SFCS_ECNDOC where id = :id";
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
		public async Task<decimal> SaveDataByTrans(SfcsEcndocModel model)
		{
			int result = 1;
			ConnectionFactory.OpenConnection(_dbConnection);
			using (var tran = _dbConnection.BeginTransaction())
			{
				try
				{
					//新增
					string insertSql = @"insert into SFCS_ECNDOC 
					(ID,ECNDOC,DOCNO,ECR,CHANGEREASON,PRIORITY,CHANGETYPE,WFCURRENTSTATE,WFORIGINALSTATE,ISCOSTROLLUP,ECNDOCTYPE,ISBATCHCHANGE,BATCHCHANGETYPE,BATCHCHANGEOBJ,PROPOSER,DEPARTMENT,PROPOSEDATE,ISUPDATEREALTIME,BUSINESSCREATEDBY,BUSINESSCREATEDON,APPROVEUSER,APPROVEDATE,STATUS,PLANUSER,PLANDATE,ISSUEUSER,ISSUEDATE,UNAPPROVEUSER,UNAPPROVEDATE,ISADDNEWVERSION,CREATEDBY,CREATEDDATE,PROPOSERNAME,CHANGEREASONCODE,CHANGEREASONDETAIL,CHANGETYPECODE,CHANGETYPEDETAIL,ECNDOCTYPECODE,ECNDOCTYPEDETAIL,DEPARTMENTNAME,BUSINESSCREATEDNAME,APPROVEUSERNAME,PLANUSERNAME,ISSUEUSERNAME,UNAPPROVEUSERNAME) 
					VALUES (:ID,:ECNDOC,:DOCNO,:ECR,:CHANGEREASON,:PRIORITY,:CHANGETYPE,:WFCURRENTSTATE,:WFORIGINALSTATE,:ISCOSTROLLUP,:ECNDOCTYPE,:ISBATCHCHANGE,:BATCHCHANGETYPE,:BATCHCHANGEOBJ,:PROPOSER,:DEPARTMENT,:PROPOSEDATE,:ISUPDATEREALTIME,:BUSINESSCREATEDBY,:BUSINESSCREATEDON,:APPROVEUSER,:APPROVEDATE,:STATUS,:PLANUSER,:PLANDATE,:ISSUEUSER,:ISSUEDATE,:UNAPPROVEUSER,:UNAPPROVEDATE,:ISADDNEWVERSION,:CREATEDBY,:CREATEDDATE,:PROPOSERNAME,:CHANGEREASONCODE,:CHANGEREASONDETAIL,:CHANGETYPECODE,:CHANGETYPEDETAIL,:ECNDOCTYPECODE,:ECNDOCTYPEDETAIL,:DEPARTMENTNAME,:BUSINESSCREATEDNAME,:APPROVEUSERNAME,:PLANUSERNAME,:ISSUEUSERNAME,:UNAPPROVEUSERNAME)";
					if (model.InsertRecords != null && model.InsertRecords.Count > 0)
					{
						foreach (var item in model.InsertRecords)
						{
							var newid = await GetSEQID();
							var resdata = await _dbConnection.ExecuteAsync(insertSql, new
							{
								ID = newid,
								item.ECNDOC,
								item.DOCNO,
								item.ECR,
								item.CHANGEREASON,
								item.PRIORITY,
								item.CHANGETYPE,
								item.WFCURRENTSTATE,
								item.WFORIGINALSTATE,
								item.ISCOSTROLLUP,
								item.ECNDOCTYPE,
								item.ISBATCHCHANGE,
								item.BATCHCHANGETYPE,
								item.BATCHCHANGEOBJ,
								item.PROPOSER,
								item.DEPARTMENT,
								item.PROPOSEDATE,
								item.ISUPDATEREALTIME,
								item.BUSINESSCREATEDBY,
								item.BUSINESSCREATEDON,
								item.APPROVEUSER,
								item.APPROVEDATE,
								item.STATUS,
								item.PLANUSER,
								item.PLANDATE,
								item.ISSUEUSER,
								item.ISSUEDATE,
								item.UNAPPROVEUSER,
								item.UNAPPROVEDATE,
								item.ISADDNEWVERSION,
								item.CREATEDBY,
								item.CREATEDDATE,
								item.PROPOSERNAME,
								item.CHANGEREASONCODE,
								item.CHANGEREASONDETAIL,
								item.CHANGETYPECODE,
								item.CHANGETYPEDETAIL,
								item.ECNDOCTYPECODE,
								item.ECNDOCTYPEDETAIL,
								item.DEPARTMENTNAME,
								item.BUSINESSCREATEDNAME,
								item.APPROVEUSERNAME,
								item.PLANUSERNAME,
								item.ISSUEUSERNAME,
								item.UNAPPROVEUSERNAME,
							}, tran);
						}
					}
					//更新
					string updateSql = @"Update SFCS_ECNDOC set ECNDOC=:ECNDOC,DOCNO=:DOCNO,ECR=:ECR,CHANGEREASON=:CHANGEREASON,PRIORITY=:PRIORITY,CHANGETYPE=:CHANGETYPE,WFCURRENTSTATE=:WFCURRENTSTATE,WFORIGINALSTATE=:WFORIGINALSTATE,ISCOSTROLLUP=:ISCOSTROLLUP,ECNDOCTYPE=:ECNDOCTYPE,ISBATCHCHANGE=:ISBATCHCHANGE,BATCHCHANGETYPE=:BATCHCHANGETYPE,BATCHCHANGEOBJ=:BATCHCHANGEOBJ,PROPOSER=:PROPOSER,DEPARTMENT=:DEPARTMENT,PROPOSEDATE=:PROPOSEDATE,ISUPDATEREALTIME=:ISUPDATEREALTIME,BUSINESSCREATEDBY=:BUSINESSCREATEDBY,BUSINESSCREATEDON=:BUSINESSCREATEDON,APPROVEUSER=:APPROVEUSER,APPROVEDATE=:APPROVEDATE,STATUS=:STATUS,PLANUSER=:PLANUSER,PLANDATE=:PLANDATE,ISSUEUSER=:ISSUEUSER,ISSUEDATE=:ISSUEDATE,UNAPPROVEUSER=:UNAPPROVEUSER,UNAPPROVEDATE=:UNAPPROVEDATE,ISADDNEWVERSION=:ISADDNEWVERSION,CREATEDBY=:CREATEDBY,CREATEDDATE=:CREATEDDATE,PROPOSERNAME=:PROPOSERNAME,CHANGEREASONCODE=:CHANGEREASONCODE,CHANGEREASONDETAIL=:CHANGEREASONDETAIL,CHANGETYPECODE=:CHANGETYPECODE,CHANGETYPEDETAIL=:CHANGETYPEDETAIL,ECNDOCTYPECODE=:ECNDOCTYPECODE,ECNDOCTYPEDETAIL=:ECNDOCTYPEDETAIL,DEPARTMENTNAME=:DEPARTMENTNAME,BUSINESSCREATEDNAME=:BUSINESSCREATEDNAME,APPROVEUSERNAME=:APPROVEUSERNAME,PLANUSERNAME=:PLANUSERNAME,ISSUEUSERNAME=:ISSUEUSERNAME,UNAPPROVEUSERNAME=:UNAPPROVEUSERNAME  
						where ID=:ID ";
					if (model.UpdateRecords != null && model.UpdateRecords.Count > 0)
					{
						foreach (var item in model.UpdateRecords)
						{
							var resdata = await _dbConnection.ExecuteAsync(updateSql, new
							{
								item.ID,
								item.ECNDOC,
								item.DOCNO,
								item.ECR,
								item.CHANGEREASON,
								item.PRIORITY,
								item.CHANGETYPE,
								item.WFCURRENTSTATE,
								item.WFORIGINALSTATE,
								item.ISCOSTROLLUP,
								item.ECNDOCTYPE,
								item.ISBATCHCHANGE,
								item.BATCHCHANGETYPE,
								item.BATCHCHANGEOBJ,
								item.PROPOSER,
								item.DEPARTMENT,
								item.PROPOSEDATE,
								item.ISUPDATEREALTIME,
								item.BUSINESSCREATEDBY,
								item.BUSINESSCREATEDON,
								item.APPROVEUSER,
								item.APPROVEDATE,
								item.STATUS,
								item.PLANUSER,
								item.PLANDATE,
								item.ISSUEUSER,
								item.ISSUEDATE,
								item.UNAPPROVEUSER,
								item.UNAPPROVEDATE,
								item.ISADDNEWVERSION,
								item.CREATEDBY,
								item.CREATEDDATE,
								item.PROPOSERNAME,
								item.CHANGEREASONCODE,
								item.CHANGEREASONDETAIL,
								item.CHANGETYPECODE,
								item.CHANGETYPEDETAIL,
								item.ECNDOCTYPECODE,
								item.ECNDOCTYPEDETAIL,
								item.DEPARTMENTNAME,
								item.BUSINESSCREATEDNAME,
								item.APPROVEUSERNAME,
								item.PLANUSERNAME,
								item.ISSUEUSERNAME,
								item.UNAPPROVEUSERNAME,
							}, tran);
						}
					}
					//删除
					string deleteSql = @"Delete from SFCS_ECNDOC where ID=:ID ";
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
		/// 获取单据类型下拉框
		/// </summary>
		/// <returns></returns>

		public async Task<List<dynamic>> GetECNDocTypeList()
		{
			List<dynamic> result = null;
			try
			{
				string sql = @"select '生产BOM变更'as Texts,'10' as Codes from dual
								union all
								select '工艺变更单'as Texts,'20' as Codes from dual
								union all
								select '研发BOM变更'as Texts,'30' as Codes from dual
								union all
								select '替代'as Texts,'40' as Codes from dual
								union all
								select '直接修改'as Texts,'41' as Codes from dual
								union all
								select '初样BOM变更'as Texts,'50' as Codes from dual";
				var objectlist = await _dbConnection.QueryAsync<dynamic>(sql);
				return objectlist?.ToList();
			}
			catch (Exception ex)
			{
				result = null;
			}
			return result;
		}

		/// <summary>
		/// 获取变更类型下拉框
		/// </summary>
		/// <returns></returns>

		public async Task<List<dynamic>> GetChangeTypeList()
		{
			List<dynamic> result = null;
			try
			{
				string sql = @"select '新增'as Texts,'10' as Codes from dual
								union all
								select '修改'as Texts,'20' as Codes from dual
								union all
								select '作废'as Texts,'30' as Codes from dual";
				var objectlist = await _dbConnection.QueryAsync<dynamic>(sql);
				return objectlist?.ToList();
			}
			catch (Exception ex)
			{
				result = null;
			}
			return result;
		}
		/// <summary>
		/// 获取变更原因下拉框
		/// </summary>
		/// <returns></returns>

		public async Task<List<dynamic>> GetChangeReasonList()
		{
			List<dynamic> result = null;
			try
			{
				string sql = @"select '新增'as Texts,'10' as Codes from dual
								union all
								select '升级'as Texts,'20' as Codes from dual
								union all
								select '作废'as Texts,'30' as Codes from dual";
				var objectlist = await _dbConnection.QueryAsync<dynamic>(sql);
				return objectlist?.ToList();
			}
			catch (Exception ex)
			{
				result = null;
			}
			return result;
		}
		/// <summary>
		/// 获取状态下拉框
		/// </summary>
		/// <returns></returns>

		public async Task<List<dynamic>> GetStatusList()
		{
			List<dynamic> result = null;
			try
			{
				string sql = @"select '开立'as Texts,'0' as Codes from dual
								union all
								select '核准中'as Texts,'1' as Codes from dual
								union all
								select '已核准'as Texts,'2' as Codes from dual
								union all
								select '已发行'as Texts,'3' as Codes from dual";
				var objectlist = await _dbConnection.QueryAsync<dynamic>(sql);
				return objectlist?.ToList();
			}
			catch (Exception ex)
			{
				result = null;
			}
			return result;
		}



		/// <summary>
		/// 获取ECN主表数据信息
		/// </summary>
		/// <param name="model"></param>
		/// <returns></returns>
		public async Task<TableDataModel> GetECNDataList(SfcsEcndocRequestModel model)
		{
			string conditions = "where 1=1  ";
			if (!model.ECNDOCTYPE.IsNullOrWhiteSpace())
			{
				conditions+=$" and ecn.ECNDOCTYPECODE=:ECNDOCTYPE";
			}
			if (!model.CHANGEREASON.IsNullOrWhiteSpace())
			{
				conditions += $" and ecn.CHANGEREASONCODE=:CHANGEREASON";
			}
			if (!model.CHANGETYPE.IsNullOrWhiteSpace())
			{
				conditions += $" and ecn.CHANGETYPECODE=:CHANGETYPE";
			}
			if (!model.STATUS.ToString().IsNullOrWhiteSpace())
			{
				conditions += $" and ecn.STATUS=:STATUS";
			}
			if (!model.Key.IsNullOrWhiteSpace())
			{
				conditions += $" AND INSTR(ecn.DOCNO, :Key) > 0 ";
			}
			string sql = @"select ROWNUM as ROWNO,ecn.ID,ecn.ECNDOC,
							ecn.DOCNO,ecn.CHANGEREASONDETAIL,ecn.PRIORITY,ecn.CHANGETYPEDETAIL,
							ecn.ISCOSTROLLUP,ecn.ECNDOCTYPEDETAIL,ecn.ISBATCHCHANGE,
							case when ecn.BATCHCHANGETYPE=0 then 'BOM'
								else '工艺' end as BATCHCHANGETYPEEXPLAIN,
							case when ecn.BATCHCHANGEOBJ=0 then 'BOM子项'
								when ecn.BATCHCHANGEOBJ=1 then '替代件'
								when ecn.BATCHCHANGEOBJ=2 then '插件位置'
								when ecn.BATCHCHANGEOBJ=3 then '元件厂牌'
								when ecn.BATCHCHANGEOBJ=4 then 'BOM产出'
								when ecn.BATCHCHANGEOBJ=5 then '工序'
								when ecn.BATCHCHANGEOBJ=6 then '工序资源'
								else '阶梯损耗' end as BATCHCHANGEOBJEXPLAIN,
							ecn.PROPOSERNAME,ecn.DEPARTMENTNAME,ecn.PROPOSEDATE,ecn.ISUPDATEREALTIME,
							ecn.BUSINESSCREATEDNAME,ecn.BUSINESSCREATEDON,ecn.APPROVEUSERNAME,
							ecn.APPROVEDATE,
							case when ecn.STATUS=0 then '开立'
								when ecn.STATUS=1 then '核准中'
								when ecn.STATUS=2 then '已核准'
								else '已发行' end as STATUSEXPLAIN,ecn.ISADDNEWVERSION
							from SFCS_ECNDoc ecn ";

			string pagedSql = SQLBuilderClass.GetPagedSQL(sql, "ecn.ID desc ", conditions);
			var resdata = await _dbConnection.QueryAsync<object>(pagedSql, model);
			string sqlcnt = @" select count(0) from SFCS_ECNDoc ecn   " + conditions;
			int cnt = await _dbConnection.ExecuteScalarAsync<int>(sqlcnt, model);
			return new TableDataModel
			{
				count = cnt,
				data = resdata?.ToList(),
			};

		}

		/// <summary>
		/// 获取明细数据
		/// </summary>
		/// <param name="model"></param>
		/// <returns></returns>
		public async Task<TableDataModel> GetEcnDetailDataList(SfcsEcndocDetailRequestModel model)
		{
			string conditions = " where 1=1 ";
			if (model.ECNDOC > 0)
			{
				conditions += $" and det.ECNDOC=:ECNDOC";
			}
			string sql = @"select ROWNUM as ROWNO, det.ID,det.ECNDOC,det.LINENUM, 
								case when det.ACDTYPE=0 then '新增'
									when det.ACDTYPE=1 then '修改'
									else '作废' end as ACDTYPEEXPLAIN,
								case when det.CHANGETYPE=0 then '物料清单'
									else '工艺路线' end as CHANGETYPEEXPLAIN,
								det.CHGOBJCODE,det.CHGOBJNAME,det.ITEMMASTER,det.ORIGINALVERSION,det.VERSION,det.OWNERORGNAME,  
								 case when det.ALTERNATETYPE=0 then '主制造'
									  else '委外' end as ALTERNATETYPEEXPLAIN, 
								 det.PRODUCTLINE,det.LOT,det.PRODUCTUOMNAME,det.DISABLEDATE,det.ORIGINALDISABLEDATE,
								 case when det.STATUS=0 then '开立'
									  when det.STATUS=1 then '核准中'
									  when det.STATUS=2 then '已核准'
									  when det.STATUS=3 then '计划'
									 else '已发行' end as STATUSEXPLAIN,
								det.AUDITORNAME,det.AUDITDATE,det.ISCANCELED,
								case when det.BOMTYPE =0 then '自制'
									else '委外' end as BOMTYPEEXPLAIN,
								det.ISCONSIDERMRP,det.DETAILCREATEDBY,det.DETIALCREATEDON,det.PLANRELEASEDATE,det.ACTUALRELEASEDATE
								from SFCS_ECNItemDetail det ";


			string pagedSql = SQLBuilderClass.GetPagedSQL(sql, " det.ID desc", conditions);
			var resdata = await _dbConnection.QueryAsync<object>(pagedSql, model);
			string sqlcnt = @"  select count(0) from SFCS_ECNItemDetail det " + conditions;
			int cnt = await _dbConnection.ExecuteScalarAsync<int>(sqlcnt, model);
			return new TableDataModel
			{
				count = cnt,
				data = resdata?.ToList(),
			};

		}











	}
}