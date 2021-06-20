/**
*┌──────────────────────────────────────────────────────────────┐
*│　描    述：接口实现                                                    
*│　作    者：嘉志科技                                            
*│　版    本：2.0    模板代码自动生成                                                
*│　创建时间：2020-03-21 14:43:44                             
*└──────────────────────────────────────────────────────────────┘
*┌──────────────────────────────────────────────────────────────┐
*│　命名空间： JZ.IMS.Repository.Oracle                                  
*│　类    名： SmtWoRepository                                      
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
    public class SmtWoRepository:BaseRepository<SmtWo,Decimal>, ISmtWoRepository
    {
        public SmtWoRepository(IOptionsSnapshot<DbOption> options)
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
			string sql = "SELECT ENABLED FROM SMT_WO WHERE ID=:ID";
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
			string sql = "UPDATE SMT_WO set ENABLED=:ENABLED WHERE ID=:Id";
			return await _dbConnection.ExecuteAsync(sql, new
			{
				ENABLED = status ? 'Y' : 'N',
				Id = id,
			});
		}

        // <summary>
        /// 获取表的序列
        /// </summary>
        /// <returns></returns>
		public async Task<decimal> GetSEQID()
		{
			string sql = "SELECT SMT_WO_SEQ.NEXTVAL MY_SEQ FROM DUAL";
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
			string sql = "select count(0) from SMT_WO where id = :id";
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
		public async Task<decimal> SaveDataByTrans(SmtWoModel model)
		{
			int result = 1;
			ConnectionFactory.OpenConnection(_dbConnection);
			using (var tran = _dbConnection.BeginTransaction())
			{
				try
				{
					//新增
					string insertSql = @"insert into SMT_WO 
					(ID,WO_NO,PART_NO,MODEL,CLASSIFICATION,DESCRIPTION,ATTRIBUTE1,ATTRIBUTE2,ATTRIBUTE3,ATTRIBUTE4,ATTRIBUTE5,CREATE_TIME,UPDATE_TIME,PROJECT_NAME) 
					VALUES (:ID,:WO_NO,:PART_NO,:MODEL,:CLASSIFICATION,:DESCRIPTION,:ATTRIBUTE1,:ATTRIBUTE2,:ATTRIBUTE3,:ATTRIBUTE4,:ATTRIBUTE5,:CREATE_TIME,:UPDATE_TIME,:PROJECT_NAME)";
					if (model.insertRecords != null && model.insertRecords.Count > 0)
					{
						foreach (var item in model.insertRecords)
						{
							var newid = await GetID();
							var resdata = await _dbConnection.ExecuteAsync(insertSql, new
							{
								ID = newid,
								item.WO_NO,
item.PART_NO,
item.MODEL,
item.CLASSIFICATION,
item.DESCRIPTION,
item.ATTRIBUTE1,
item.ATTRIBUTE2,
item.ATTRIBUTE3,
item.ATTRIBUTE4,
item.ATTRIBUTE5,
item.CREATE_TIME,
item.UPDATE_TIME,item.PROJECT_NAME
							}, tran);
						}
					}
					//更新
					string updateSql = @"Update SMT_WO set WO_NO=:WO_NO,PART_NO=:PART_NO,MODEL=:MODEL,CLASSIFICATION=:CLASSIFICATION,DESCRIPTION=:DESCRIPTION,ATTRIBUTE1=:ATTRIBUTE1,ATTRIBUTE2=:ATTRIBUTE2,ATTRIBUTE3=:ATTRIBUTE3,ATTRIBUTE4=:ATTRIBUTE4,ATTRIBUTE5=:ATTRIBUTE5,CREATE_TIME=:CREATE_TIME,UPDATE_TIME=:UPDATE_TIME,PROJECT_NAME=:PROJECT_NAME  
						where ID=:ID ";
					if (model.updateRecords != null && model.updateRecords.Count > 0)
					{
						foreach (var item in model.updateRecords)
						{
							var resdata = await _dbConnection.ExecuteAsync(updateSql, new
							{
								item.ID,
								item.WO_NO,
item.PART_NO,
item.MODEL,
item.CLASSIFICATION,
item.DESCRIPTION,
item.ATTRIBUTE1,
item.ATTRIBUTE2,
item.ATTRIBUTE3,
item.ATTRIBUTE4,
item.ATTRIBUTE5,
item.CREATE_TIME,
item.UPDATE_TIME,item.PROJECT_NAME
							}, tran);
						}
					}
					//删除
					string deleteSql = @"Delete from SMT_WO where ID=:ID ";
					if (model.removeRecords != null && model.removeRecords.Count > 0)
					{
						foreach (var item in model.removeRecords)
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
    }
}