/*
*┌──────────────────────────────────────────────────────────────┐
*│　描    述：接口实现                                                    
*│　作    者：嘉志科技                                            
*│　版    本：2.0    模板代码自动生成                                                
*│　创建时间：2020-06-30 16:18:14                             
*└──────────────────────────────────────────────────────────────┘
*┌──────────────────────────────────────────────────────────────┐
*│　命名空间： JZ.IMS.Repository.Oracle                                  
*│　类    名： MesCpcecnBomlineRepository                                      
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
    public class MesCpcecnBomlineRepository:BaseRepository<MesCpcecnBomline,Decimal>, IMesCpcecnBomlineRepository
    {
        public MesCpcecnBomlineRepository(IOptionsSnapshot<DbOption> options)
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
			string sql = "SELECT ENABLED FROM MES_CPCECN_BOMLINE WHERE ID=:ID";
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
			string sql = "UPDATE MES_CPCECN_BOMLINE set ENABLED=:ENABLED WHERE ID=:Id";
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
			string sql = "SELECT MES_CPCECN_BOMLINE_SEQ.NEXTVAL MY_SEQ FROM DUAL";
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
			string sql = "select count(0) from MES_CPCECN_BOMLINE where id = :id";
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
		public async Task<decimal> SaveDataByTrans(MesCpcecnBomlineModel model)
		{
			int result = 1;
			ConnectionFactory.OpenConnection(_dbConnection);
			using (var tran = _dbConnection.BeginTransaction())
			{
				try
				{
					//新增
					string insertSql = @"insert into MES_CPCECN_BOMLINE 
					(ID,SEQ,SUPERCODE,SUPERDESC,BOMOPERATION,BCITEMCODE,BCLOCATION,BCNAME,BCSPEC,BCQTY,BCQTY_BASE,BCUNIT,BCBOMDDEF1,BCBOMDDEF4,BCNOTE,ACITEMCODE,ACLOCATION,ACNAME,ACSPEC,ACQTY,ACQTY_BASE,ACUNIT,ACBOMDDEF1,ACBOMDDEF4,ACNOTE,PROJCODE,STAT,NTCODE,CDATA,ECNNAME,ECNID) 
					VALUES (:ID,:SEQ,:SUPERCODE,:SUPERDESC,:BOMOPERATION,:BCITEMCODE,:BCLOCATION,:BCNAME,:BCSPEC,:BCQTY,:BCQTY_BASE,:BCUNIT,:BCBOMDDEF1,:BCBOMDDEF4,:BCNOTE,:ACITEMCODE,:ACLOCATION,:ACNAME,:ACSPEC,:ACQTY,:ACQTY_BASE,:ACUNIT,:ACBOMDDEF1,:ACBOMDDEF4,:ACNOTE,:PROJCODE,:STAT,:NTCODE,:CDATA,:ECNNAME,:ECNID)";
					if (model.InsertRecords != null && model.InsertRecords.Count > 0)
					{
						foreach (var item in model.InsertRecords)
						{
							var newid = await GetSEQID();
							var resdata = await _dbConnection.ExecuteAsync(insertSql, new
							{
								ID = newid,
								item.SEQ,
								item.SUPERCODE,
								item.SUPERDESC,
								item.BOMOPERATION,
								item.BCITEMCODE,
								item.BCLOCATION,
								item.BCNAME,
								item.BCSPEC,
								item.BCQTY,
								item.BCQTY_BASE,
								item.BCUNIT,
								item.BCBOMDDEF1,
								item.BCBOMDDEF4,
								item.BCNOTE,
								item.ACITEMCODE,
								item.ACLOCATION,
								item.ACNAME,
								item.ACSPEC,
								item.ACQTY,
								item.ACQTY_BASE,
								item.ACUNIT,
								item.ACBOMDDEF1,
								item.ACBOMDDEF4,
								item.ACNOTE,
								item.PROJCODE,
								item.STAT,
								item.NTCODE,
								item.CDATA,
								item.ECNNAME,
								item.ECNID,
							}, tran);
						}
					}
					//更新
					string updateSql = @"Update MES_CPCECN_BOMLINE set SEQ=:SEQ,SUPERCODE=:SUPERCODE,SUPERDESC=:SUPERDESC,BOMOPERATION=:BOMOPERATION,BCITEMCODE=:BCITEMCODE,BCLOCATION=:BCLOCATION,BCNAME=:BCNAME,BCSPEC=:BCSPEC,BCQTY=:BCQTY,BCQTY_BASE=:BCQTY_BASE,BCUNIT=:BCUNIT,BCBOMDDEF1=:BCBOMDDEF1,BCBOMDDEF4=:BCBOMDDEF4,BCNOTE=:BCNOTE,ACITEMCODE=:ACITEMCODE,ACLOCATION=:ACLOCATION,ACNAME=:ACNAME,ACSPEC=:ACSPEC,ACQTY=:ACQTY,ACQTY_BASE=:ACQTY_BASE,ACUNIT=:ACUNIT,ACBOMDDEF1=:ACBOMDDEF1,ACBOMDDEF4=:ACBOMDDEF4,ACNOTE=:ACNOTE,PROJCODE=:PROJCODE,STAT=:STAT,NTCODE=:NTCODE,CDATA=:CDATA,ECNNAME=:ECNNAME,ECNID=:ECNID  
						where ID=:ID ";
					if (model.UpdateRecords != null && model.UpdateRecords.Count > 0)
					{
						foreach (var item in model.UpdateRecords)
						{
							var resdata = await _dbConnection.ExecuteAsync(updateSql, new
							{
								item.ID,
								item.SEQ,
								item.SUPERCODE,
								item.SUPERDESC,
								item.BOMOPERATION,
								item.BCITEMCODE,
								item.BCLOCATION,
								item.BCNAME,
								item.BCSPEC,
								item.BCQTY,
								item.BCQTY_BASE,
								item.BCUNIT,
								item.BCBOMDDEF1,
								item.BCBOMDDEF4,
								item.BCNOTE,
								item.ACITEMCODE,
								item.ACLOCATION,
								item.ACNAME,
								item.ACSPEC,
								item.ACQTY,
								item.ACQTY_BASE,
								item.ACUNIT,
								item.ACBOMDDEF1,
								item.ACBOMDDEF4,
								item.ACNOTE,
								item.PROJCODE,
								item.STAT,
								item.NTCODE,
								item.CDATA,
								item.ECNNAME,
								item.ECNID,
							}, tran);
						}
					}
					//删除
					string deleteSql = @"Delete from MES_CPCECN_BOMLINE where ID=:ID ";
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
    }
}