/*
*┌──────────────────────────────────────────────────────────────┐
*│　描    述：接口实现                                                    
*│　作    者：嘉志科技                                            
*│　版    本：2.0    模板代码自动生成                                                
*│　创建时间：2020-04-23 10:33:58                             
*└──────────────────────────────────────────────────────────────┘
*┌──────────────────────────────────────────────────────────────┐
*│　命名空间： JZ.IMS.Repository.Oracle                                  
*│　类    名： SfcsRuncardRepository                                      
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
using System.Data;

namespace JZ.IMS.Repository.Oracle
{
    public class SfcsRuncardRepository:BaseRepository<SfcsRuncard,Decimal>, ISfcsRuncardRepository
    {
        public SfcsRuncardRepository(IOptionsSnapshot<DbOption> options)
        {
            _dbOption =options.Get("iWMS");
            if (_dbOption == null)
            {
                throw new ArgumentNullException(nameof(DbOption));
            }
            _dbConnection = ConnectionFactory.CreateConnection(_dbOption.DbType, _dbOption.ConnectionString);
        }

       private string processMultiOperation = @"SFCS_COMMON_OPERATION_PKG.PROCESS_MULTI_OPERATION19";
        /// <summary>
        /// 获取当前的连接
        /// </summary>
        /// <returns></returns>
        public async Task<IDbConnection> GetConnection()
        {
            return  this._dbConnection;
        }

        public async Task<Decimal> GetSFCSOperationID()
        {
            String sql = @"SELECT SFCS_OPERATION_SEQ.NEXTVAL FROM DUAL";
            var result = await _dbConnection.ExecuteScalarAsync(sql);
            return (decimal)result;
        }

        /// <summary>
        /// 根据主键获取激活状态
        /// </summary>
        /// <param name="id">主键</param>
        /// <returns></returns>
		public async Task<Boolean> GetEnableStatus(decimal id)
		{
			string sql = "SELECT ENABLED FROM SFCS_RUNCARD WHERE ID=:ID";
			var result = await _dbConnection.QueryFirstOrDefaultAsync<string>(sql, new
			{
				ID = id,
			});

			return result == "Y" ? true : false;
		}
        /// <summary>
        /// 检查抽检
        /// </summary>
        /// <param name="sn"></param>
        /// <param name="status"></param>
        /// <param name="operationID"></param>
        /// <param name="siteID"></param>
        /// <param name="procedureMessage"></param>
        /// <param name="ratio"></param>
        /// <param name="deliverCount"></param>
        /// <param name="needSpotCheck"></param>
        /// <returns></returns>
        public  Decimal SpotCheckControl(string sn, string status, decimal operationID,
            decimal siteID, out string procedureMessage, out string ratio, out decimal deliverCount,
            out decimal needSpotCheck, IDbTransaction transaction)
        {
            var p = new DynamicParameters();
            p.Add(":P_SN_STATUS", status, DbType.String, ParameterDirection.Input);
            p.Add(":P_OPERATION_ID", operationID, DbType.Decimal, ParameterDirection.Input);
            p.Add(":P_OPERATION_SITE_ID", siteID, DbType.Decimal, ParameterDirection.Input);
            p.Add(":P_SN", sn, DbType.String, ParameterDirection.Input);
            p.Add(":P_MESSAGE", "", DbType.String, ParameterDirection.Output);
            p.Add(":P_RATIO", "", DbType.String, ParameterDirection.Output);
            p.Add(":P_DELIVER_COUNT", 0, DbType.Decimal, ParameterDirection.Output);
            p.Add(":P_NEED_TO_SPOTCHECK", 0, DbType.Decimal, ParameterDirection.Output);
            p.Add(":P_RESULT", 0, DbType.Decimal, ParameterDirection.Output);
            p.Add(":P_TRACE", "", DbType.String, ParameterDirection.Output);

            _dbConnection.Execute("SFCS_FUNCTIONS_PKG.SPOT_CHECK_CONTROLLER", p, transaction, commandType: CommandType.StoredProcedure);
            procedureMessage = p.Get<String>(":P_MESSAGE");
            ratio = p.Get<String>(":P_RATIO");
            deliverCount = p.Get<Decimal>(":P_DELIVER_COUNT");
            needSpotCheck = p.Get<Decimal>(":P_NEED_TO_SPOTCHECK");
            Decimal result = p.Get<Decimal>(":P_RESULT");
            return result;
        }



        /// <summary>
        /// 修改激活状态
        /// </summary>
        /// <param name="id">主键</param>
        /// <param name="status">更改后的状态</param>
        /// <returns></returns>
        public async Task<decimal> ChangeEnableStatus(decimal id, bool status)
		{
			string sql = "UPDATE SFCS_RUNCARD set ENABLED=:ENABLED WHERE ID=:Id";
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
			string sql = "SELECT SFCS_RUNCARD_SEQ.NEXTVAL MY_SEQ FROM DUAL";
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
			string sql = "select count(0) from SFCS_RUNCARD where id = :id";
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
		public async Task<decimal> SaveDataByTrans(SfcsRuncardModel model)
		{
			int result = 1;
			ConnectionFactory.OpenConnection(_dbConnection);
			using (var tran = _dbConnection.BeginTransaction())
			{
				try
				{
					//新增
					string insertSql = @"insert into SFCS_RUNCARD 
					(ID,SN,PARENT_SN,WO_ID,ROUTE_ID,CURRENT_SITE,WIP_OPERATION,LAST_OPERATION,LAST_OPERATION_COUNTER,STATUS,TURNIN_NO,TRACKING_NO,CARTON_NO,PALLET_NO,GG_NO,GG_ITEM,SMT_TURNIN_NO,RMA_COUNT,INPUT_TIME,OPERATION_TIME,TURNIN_TIME,SHIP_TIME,REPLACE_FLAG,SAMPLE_BATCH,SAMPLE_FLAG,WARRANTY) 
					VALUES (:ID,:SN,:PARENT_SN,:WO_ID,:ROUTE_ID,:CURRENT_SITE,:WIP_OPERATION,:LAST_OPERATION,:LAST_OPERATION_COUNTER,:STATUS,:TURNIN_NO,:TRACKING_NO,:CARTON_NO,:PALLET_NO,:GG_NO,:GG_ITEM,:SMT_TURNIN_NO,:RMA_COUNT,:INPUT_TIME,:OPERATION_TIME,:TURNIN_TIME,:SHIP_TIME,:REPLACE_FLAG,:SAMPLE_BATCH,:SAMPLE_FLAG,:WARRANTY)";
					if (model.InsertRecords != null && model.InsertRecords.Count > 0)
					{
						foreach (var item in model.InsertRecords)
						{
							var newid = await GetSEQID();
							var resdata = await _dbConnection.ExecuteAsync(insertSql, new
							{
								ID = newid,
								item.SN,
								item.PARENT_SN,
								item.WO_ID,
								item.ROUTE_ID,
								item.CURRENT_SITE,
								item.WIP_OPERATION,
								item.LAST_OPERATION,
								item.LAST_OPERATION_COUNTER,
								item.STATUS,
								item.TURNIN_NO,
								item.TRACKING_NO,
								item.CARTON_NO,
								item.PALLET_NO,
								item.GG_NO,
								item.GG_ITEM,
								item.SMT_TURNIN_NO,
								item.RMA_COUNT,
								item.INPUT_TIME,
								item.OPERATION_TIME,
								item.TURNIN_TIME,
								item.SHIP_TIME,
								item.REPLACE_FLAG,
								item.SAMPLE_BATCH,
								item.SAMPLE_FLAG,
								item.WARRANTY,
							}, tran);
						}
					}
					//更新
					string updateSql = @"Update SFCS_RUNCARD set SN=:SN,PARENT_SN=:PARENT_SN,WO_ID=:WO_ID,ROUTE_ID=:ROUTE_ID,CURRENT_SITE=:CURRENT_SITE,WIP_OPERATION=:WIP_OPERATION,LAST_OPERATION=:LAST_OPERATION,LAST_OPERATION_COUNTER=:LAST_OPERATION_COUNTER,STATUS=:STATUS,TURNIN_NO=:TURNIN_NO,TRACKING_NO=:TRACKING_NO,CARTON_NO=:CARTON_NO,PALLET_NO=:PALLET_NO,GG_NO=:GG_NO,GG_ITEM=:GG_ITEM,SMT_TURNIN_NO=:SMT_TURNIN_NO,RMA_COUNT=:RMA_COUNT,INPUT_TIME=:INPUT_TIME,OPERATION_TIME=:OPERATION_TIME,TURNIN_TIME=:TURNIN_TIME,SHIP_TIME=:SHIP_TIME,REPLACE_FLAG=:REPLACE_FLAG,SAMPLE_BATCH=:SAMPLE_BATCH,SAMPLE_FLAG=:SAMPLE_FLAG,WARRANTY=:WARRANTY  
						where ID=:ID ";
					if (model.UpdateRecords != null && model.UpdateRecords.Count > 0)
					{
						foreach (var item in model.UpdateRecords)
						{
							var resdata = await _dbConnection.ExecuteAsync(updateSql, new
							{
								item.ID,
								item.SN,
								item.PARENT_SN,
								item.WO_ID,
								item.ROUTE_ID,
								item.CURRENT_SITE,
								item.WIP_OPERATION,
								item.LAST_OPERATION,
								item.LAST_OPERATION_COUNTER,
								item.STATUS,
								item.TURNIN_NO,
								item.TRACKING_NO,
								item.CARTON_NO,
								item.PALLET_NO,
								item.GG_NO,
								item.GG_ITEM,
								item.SMT_TURNIN_NO,
								item.RMA_COUNT,
								item.INPUT_TIME,
								item.OPERATION_TIME,
								item.TURNIN_TIME,
								item.SHIP_TIME,
								item.REPLACE_FLAG,
								item.SAMPLE_BATCH,
								item.SAMPLE_FLAG,
								item.WARRANTY,
							}, tran);
						}
					}
					//删除
					string deleteSql = @"Delete from SFCS_RUNCARD where ID=:ID ";
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
		/// 根据SN修改卡通号
		/// </summary>
		/// <param name="carton_no"></param>
		/// <param name="sn"></param>
		/// <param name="tran"></param>
		/// <returns></returns>
		public async Task<int> UpdateCartonNoBySN(string carton_no, string sn, IDbTransaction tran)
		{
			int result = 0;
			ConnectionFactory.OpenConnection(_dbConnection);
			try
			{
				string updateSql = @"UPDATE SFCS_RUNCARD SET CARTON_NO=:CARTON_NO WHERE SN=:SN";
				result = await _dbConnection.ExecuteAsync(updateSql, new
				{
					CARTON_NO = carton_no,
					SN = sn
				}, tran);
				if (result <= 0) { throw new Exception("UPDATE_CARTONNO_FAILURE"); }

				decimal qty = QueryEx<decimal>("SELECT COUNT(1) QTY FROM SFCS_RUNCARD WHERE CARTON_NO =:CARTON_NO", new { CARTON_NO = carton_no }).FirstOrDefault();

				String U_UpadateContainerListSeq = @"UPDATE SFCS_CONTAINER_LIST SET SEQUENCE = :SEQUENCE WHERE CONTAINER_SN=:CONTAINER_SN";
				result = Execute(U_UpadateContainerListSeq, new
				{
					SEQUENCE = qty,
					CONTAINER_SN = carton_no
				}, tran);
				if (result <= 0) { throw new Exception("UPDATE_CARTONNO_FAILURE"); }


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

			return result;
		}

		/// <summary>
		/// 根据SN修改卡通号
		/// </summary>
		/// <param name="carton_no"></param>
		/// <param name="sn"></param>
		/// <param name="tran"></param>
		/// <returns></returns>
		public async Task<int> UpdateCartonNoBySNEx(string carton_no, string sn, IDbTransaction tran)
		{
			int result = 0;
			try
			{
				string updateSql = @"UPDATE SFCS_RUNCARD SET CARTON_NO=:CARTON_NO WHERE SN=:SN";
				result = await _dbConnection.ExecuteAsync(updateSql, new
				{
					CARTON_NO = carton_no,
					SN = sn
				}, tran);
				if (result <= 0) { throw new Exception("UPDATE_CARTONNO_FAILURE"); }

				decimal qty = QueryEx<decimal>("SELECT COUNT(1) QTY FROM SFCS_RUNCARD WHERE CARTON_NO =:CARTON_NO", new { CARTON_NO = carton_no }).FirstOrDefault();
				decimal containerListQuantity = QueryEx<decimal>("SELECT QUANTITY FROM SFCS_CONTAINER_LIST WHERE CONTAINER_SN=:CONTAINER_SN", new { CONTAINER_SN = carton_no }).FirstOrDefault();

				string conditions = "";
				String U_UpadateContainerListSeq = $@"UPDATE SFCS_CONTAINER_LIST SET SEQUENCE = :SEQUENCE{conditions} WHERE CONTAINER_SN=:CONTAINER_SN";
                if (qty==containerListQuantity)
                {
					conditions += ",FULL_FLAG=:FULL_FLAG";
                }
				result = Execute(U_UpadateContainerListSeq, new
				{
					SEQUENCE = qty,
					CONTAINER_SN = carton_no
				}, tran);
				if (result <= 0) { throw new Exception("UPDATE_CARTONNO_FAILURE"); }
			}
			catch (Exception ex)
			{
				result = -1;
				throw ex;
			}

			return result;
		}
	}
}