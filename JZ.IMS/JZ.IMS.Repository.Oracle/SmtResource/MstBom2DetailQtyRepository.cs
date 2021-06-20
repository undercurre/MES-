/*
*┌──────────────────────────────────────────────────────────────┐
*│　描    述：接口实现                                                    
*│　作    者：嘉志科技                                            
*│　版    本：2.0    模板代码自动生成                                                
*│　创建时间：2020-10-14 16:33:04                             
*└──────────────────────────────────────────────────────────────┘
*┌──────────────────────────────────────────────────────────────┐
*│　命名空间： JZ.IMS.Repository.Oracle                                  
*│　类    名： MstBom2DetailQtyRepository                                      
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
    public class MstBom2DetailQtyRepository:BaseRepository<MstBom2DetailQty,Decimal>, IMstBom2DetailQtyRepository
    {
        public MstBom2DetailQtyRepository(IOptionsSnapshot<DbOption> options)
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
			string sql = "SELECT ENABLED FROM MST_BOM2_DETAIL_QTY WHERE ID=:ID";
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
			string sql = "UPDATE MST_BOM2_DETAIL_QTY set ENABLED=:ENABLED WHERE ID=:Id";
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
			string sql = "SELECT MST_BOM2_DETAIL_QTY_SEQ.NEXTVAL MY_SEQ FROM DUAL";
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
			string sql = "select count(0) from MST_BOM2_DETAIL_QTY where id = :id";
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
		public async Task<decimal> SaveDataByTrans(MstBom2DetailQtyModel model)
		{
			int result = 1;
			ConnectionFactory.OpenConnection(_dbConnection);
			using (var tran = _dbConnection.BeginTransaction())
			{
				try
				{
					//新增
					string insertSql = @"insert into MST_BOM2_DETAIL_QTY 
					(ID,WO_NO,PART_NO,PART_CODE,SHORTTAGE_QTY,SENT_QTY) 
					VALUES (:ID,:WO_NO,:PART_NO,:PART_CODE,:SHORTTAGE_QTY,:SENT_QTY)";
					if (model.InsertRecords != null && model.InsertRecords.Count > 0)
					{
						foreach (var item in model.InsertRecords)
						{
							var newid = await GetSEQID();
							var resdata = await _dbConnection.ExecuteAsync(insertSql, new
							{
								ID = newid,
								item.WO_NO,
								item.PART_NO,
								item.PART_CODE,
								item.SHORTTAGE_QTY,
								item.SENT_QTY,
							}, tran);
						}
					}
					//更新
					string updateSql = @"Update MST_BOM2_DETAIL_QTY set WO_NO=:WO_NO,PART_NO=:PART_NO,PART_CODE=:PART_CODE,SHORTTAGE_QTY=:SHORTTAGE_QTY,SENT_QTY=:SENT_QTY  
						where ID=:ID ";
					if (model.UpdateRecords != null && model.UpdateRecords.Count > 0)
					{
						foreach (var item in model.UpdateRecords)
						{
							var resdata = await _dbConnection.ExecuteAsync(updateSql, new
							{
								item.ID,
								item.WO_NO,
								item.PART_NO,
								item.PART_CODE,
								item.SHORTTAGE_QTY,
								item.SENT_QTY,
							}, tran);
						}
					}
					//删除
					string deleteSql = @"Delete from MST_BOM2_DETAIL_QTY where ID=:ID ";
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
		/// 获取欠料情况报表数据
		/// </summary>
		/// <param name="model"></param>
		/// <returns></returns>
		public async Task<TableDataModel> GetLackMaterialsData(PageModel model)
        {
            int page = 0, limit = 0;
            page = model.Page * model.Limit - model.Limit + 1;
            limit = model.Page * model.Limit;
            model.Page = page;
            model.Limit = limit;

            string sWhere = "";

            if (!model.Key.IsNullOrWhiteSpace())
            {
                sWhere += $" AND WO.WO_NO LIKE '%" + model.Key + "%' ";
            }

            string sQuery = string.Format("SELECT * FROM (SELECT ROWNUM R, T.* FROM ( SELECT T.*,NVL(QTY.SHORTTAGE_QTY,0) SHORTTAGE_QTY,NVL(QTY.SENT_QTY,0) SENT_QTY,QTY.ID FROM (SELECT WO.WO_NO,WO.PART_NO,BOM2.PART_CODE,BOM2.PART_NAME,IP.DESCRIPTION,BOM2.COMPONENT_LOCATION,BOM2.REMARK FROM SFCS_WO WO,SMT_BOM1 BOM1,SMT_BOM2 BOM2,IMS_PART IP WHERE WO.PART_NO=BOM1.PARTENT_CODE AND BOM1.BOM_ID=BOM2.BOM_ID AND BOM2.PART_CODE=IP.CODE {0} ) T LEFT JOIN MST_BOM2_DETAIL_QTY QTY ON T.WO_NO=QTY.WO_NO AND T.PART_NO=QTY.PART_NO AND T.PART_CODE=QTY.PART_CODE ORDER BY T.WO_NO DESC) T) WHERE R BETWEEN :Page AND :Limit", sWhere);
            var resdata = await _dbConnection.QueryAsync<object>(sQuery, model);

            sQuery = string.Format("SELECT COUNT(1) FROM (SELECT WO.WO_NO,WO.PART_NO,BOM2.PART_CODE,BOM2.PART_NAME,IP.DESCRIPTION,BOM2.COMPONENT_LOCATION,BOM2.REMARK FROM SFCS_WO WO,SMT_BOM1 BOM1,SMT_BOM2 BOM2,IMS_PART IP WHERE WO.PART_NO=BOM1.PARTENT_CODE AND BOM1.BOM_ID=BOM2.BOM_ID AND BOM2.PART_CODE=IP.CODE {0} ) T LEFT JOIN MST_BOM2_DETAIL_QTY QTY ON T.WO_NO=QTY.WO_NO AND T.PART_NO=QTY.PART_NO AND T.PART_CODE=QTY.PART_CODE ORDER BY T.WO_NO DESC", sWhere);

            int cnt = await _dbConnection.ExecuteScalarAsync<int>(sQuery, model);
            return new TableDataModel
            {
                count = cnt,
                data = resdata?.ToList(),
            };
        }

        /// <summary>
        /// 保存数据
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public async Task<int> SaveDataByMstBom2Detail(MstBom2DetailQtyAddOrModifyModel model)
        {
            int result = 1;
            ConnectionFactory.OpenConnection(_dbConnection);
            using (var tran = _dbConnection.BeginTransaction())
            {
                try
                {
                    if (model.ID == 0)
                    {
                        //新增
                        string insertSql = @"INSERT INTO MST_BOM2_DETAIL_QTY (ID,WO_NO,PART_NO,PART_CODE,SHORTTAGE_QTY,SENT_QTY)  VALUES (:ID,:WO_NO,:PART_NO,:PART_CODE,:SHORTTAGE_QTY,:SENT_QTY)";
                        var newid = await GetSEQID();
                        var resdata = await _dbConnection.ExecuteAsync(insertSql, new
                        {
                            ID = newid,
                            model.WO_NO,
                            model.PART_NO,
                            model.PART_CODE,
                            model.SHORTTAGE_QTY,
                            model.SENT_QTY,

                        }, tran);
                        result = resdata;
                    }
                    else
                    {
                        //更新
                        string updateSql = @"UPDATE MST_BOM2_DETAIL_QTY SET WO_NO=:WO_NO,PART_NO=:PART_NO,PART_CODE=:PART_CODE,SHORTTAGE_QTY=:SHORTTAGE_QTY,SENT_QTY=:SENT_QTY WHERE ID=:ID ";
                        var resdata = await _dbConnection.ExecuteAsync(updateSql, new
                        {
                            model.ID,
                            model.WO_NO,
                            model.PART_NO,
                            model.PART_CODE,
                            model.SHORTTAGE_QTY,
                            model.SENT_QTY,
                        }, tran);
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
        /// 根据欠料表ID删除数据
        /// </summary>
        /// <param name="id">欠料表ID</param>
        /// <returns></returns>
        public async Task<decimal> DeleteMstBom2Detail(decimal id)
        {
            int result = 1;
            ConnectionFactory.OpenConnection(_dbConnection);
            using (var tran = _dbConnection.BeginTransaction())
            {
                try
                {
                    //删除
                    string deleteSql = @"DELETE FROM MST_BOM2_DETAIL_QTY WHERE ID=:ID ";
                    result = await _dbConnection.ExecuteAsync(deleteSql, new { id }, tran);
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
    }
}