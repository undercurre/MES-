/*
*┌──────────────────────────────────────────────────────────────┐
*│　描    述：接口实现                                                    
*│　作    者：嘉志科技                                            
*│　版    本：2.0    模板代码自动生成                                                
*│　创建时间：2020-10-16 10:01:53                             
*└──────────────────────────────────────────────────────────────┘
*┌──────────────────────────────────────────────────────────────┐
*│　命名空间： JZ.IMS.Repository.Oracle                                  
*│　类    名： SfcsEquipmentThrowRepository                                      
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

namespace JZ.IMS.Repository.Oracle
{
    public class SfcsEquipmentThrowRepository:BaseRepository<SfcsEquipmentThrow,Decimal>, ISfcsEquipmentThrowRepository
    {
        public SfcsEquipmentThrowRepository(IOptionsSnapshot<DbOption> options)
        {
            _dbOption =options.Get("iWMS");
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
			string sql = "SELECT ENABLED FROM SFCS_EQUIPMENT_THROW WHERE ID=:ID";
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
			string sql = "UPDATE SFCS_EQUIPMENT_THROW set ENABLED=:ENABLED WHERE ID=:Id";
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
			string sql = "SELECT SFCS_EQUIPMENT_THROW_SEQ.NEXTVAL MY_SEQ FROM DUAL";
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
			string sql = "select count(0) from SFCS_EQUIPMENT_THROW where id = :id";
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
		public async Task<decimal> SaveDataByTrans(SfcsEquipmentThrowModel model)
		{
			int result = 1;
			ConnectionFactory.OpenConnection(_dbConnection);
			using (var tran = _dbConnection.BeginTransaction())
			{
				try
				{
					//新增
					string insertSql = @"insert into SFCS_EQUIPMENT_THROW 
					(ID,ORGANIZE_ID,LINE_ID,THROW_DATE,START_TIME,END_TIME,PART_NO,ORDER_QTY,TARGET_QTY,SITE_ID,EQUIPMENT_ID,THROW_QTY,CREATE_TIME,CREATE_USER,WO_NO,MODEL) 
					VALUES (:ID,:ORGANIZE_ID,:LINE_ID,:THROW_DATE,:START_TIME,:END_TIME,:PART_NO,:ORDER_QTY,:TARGET_QTY,:SITE_ID,:EQUIPMENT_ID,:THROW_QTY,SYSDATE,:CREATE_USER,:WO_NO,:MODEL)";
					if (model.InsertRecords != null && model.InsertRecords.Count > 0)
					{
						foreach (var item in model.InsertRecords)
						{
							var newid = await GetSEQID();
							var resdata = await _dbConnection.ExecuteAsync(insertSql, new
							{
								ID = newid,
								item.ORGANIZE_ID,
                                item.LINE_ID,
								item.THROW_DATE,
								item.START_TIME,
								item.END_TIME,
								item.PART_NO,
								item.ORDER_QTY,
								item.TARGET_QTY,
								item.SITE_ID,
								item.EQUIPMENT_ID,
								item.THROW_QTY,
								item.CREATE_TIME,
								item.CREATE_USER,                                item.MODEL,                                item.WO_NO,
							}, tran);
						}
					}
                    //更新
                    string updateSql = @"Update SFCS_EQUIPMENT_THROW set LINE_ID=:LINE_ID,THROW_DATE=:THROW_DATE,START_TIME=:START_TIME,END_TIME=:END_TIME,WO_NO=:WO_NO,PART_NO=:PART_NO,MODEL=:MODEL,ORDER_QTY=:ORDER_QTY,TARGET_QTY=:TARGET_QTY,SITE_ID=:SITE_ID,EQUIPMENT_ID=:EQUIPMENT_ID,THROW_QTY=:THROW_QTY  
						where ID=:ID ";
                    if (model.UpdateRecords != null && model.UpdateRecords.Count > 0)
                    {
                        foreach (var item in model.UpdateRecords)
                        {
                            var resdata = await _dbConnection.ExecuteAsync(updateSql, new
                            {
                                item.ID,
                                item.LINE_ID,
                                item.THROW_DATE,
                                item.START_TIME,
                                item.END_TIME,
                                item.WO_NO,
                                item.PART_NO,
                                item.MODEL,
                                item.ORDER_QTY,
                                item.TARGET_QTY,
                                item.SITE_ID,
                                item.EQUIPMENT_ID,
                                item.THROW_QTY,

                            }, tran);
                        }
                    }
                    //删除
                    string deleteSql = @"Delete from SFCS_EQUIPMENT_THROW where ID=:ID ";
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
        /// 获取设备列表
        /// </summary>
        /// <returns></returns>
        public List<SfcsEquipmentList> GetSfcsEquipmentList()
        {
            string sql = @"SELECT ID,NAME FROM SFCS_EQUIPMENT";
            return (_dbConnection.Query<SfcsEquipmentList>(sql)).ToList();
        }

        /// <summary>
        /// 获取站点列表
        /// </summary>
        /// <returns></returns>
        public List<SfcsOperationSiteList> GetSfcsOperationSiteList()
        {
            string sql = @"SELECT ID,OPERATION_SITE_NAME,OPERATION_LINE_ID LINE_ID FROM SFCS_OPERATION_SITES";
            return (_dbConnection.Query<SfcsOperationSiteList>(sql)).ToList();
        }

        /// <summary>
        /// 获取物料信息
        /// </summary>
        /// <returns></returns>
        public List<ImsPart> GetImsParts()
        {
            string sql = @"SELECT CODE,DESCRIPTION FROM IMS_PART";
            return (_dbConnection.Query<ImsPart>(sql)).ToList();
        }

        /// <summary>
        /// 获取工单信息
        /// </summary>
        /// <param name="WO_NO"></param>
        /// <returns></returns>
        public ReturnPartAndModel GetModelAndPartByWoNo(string WO_NO)
        {
            string sql = @"SELECT WO_NO,PART_NO,MODEL FROM SFCS_WO A
                LEFT JOIN SFCS_MODEL B ON A.MODEL_ID = B.ID WHERE WO_NO=:WO_NO";
            return (_dbConnection.Query<ReturnPartAndModel>(sql,new { WO_NO })).FirstOrDefault();
        }

        /// <summary>
        /// 获取报表数据
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public List<ReturnReportData> GetReturnReportDatas(SfcsEquipmentThrowRequestModel model,string conditions)
        {
            string sql = @"SELECT LINE_NAME,NAME EQUIPMENT_NAME,SUM(TARGET_QTY) TARGET_QTY,SUM(THROW_QTY) THROW_QTY,
                            ROUND(SUM(THROW_QTY)/SUM(TARGET_QTY),2) * 100 THROW_RATE FROM SFCS_EQUIPMENT_THROW A
                            LEFT JOIN V_MES_LINES B ON A.LINE_ID = B.LINE_ID
                            LEFT JOIN SFCS_EQUIPMENT C ON A.EQUIPMENT_ID = C.ID {0}
                            GROUP BY LINE_NAME,NAME";
            sql = string.Format(sql, conditions);
            return (_dbConnection.Query<ReturnReportData>(sql, model)).ToList();
        }
    }
}