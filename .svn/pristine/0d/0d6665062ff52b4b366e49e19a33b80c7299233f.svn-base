/**
*┌──────────────────────────────────────────────────────────────┐
*│　描    述：设备点检维修零件表接口实现                                                    
*│　作    者：嘉志科技                                            
*│　版    本：2.0    模板代码自动生成                                                
*│　创建时间：2019-11-01 10:47:11                             
*└──────────────────────────────────────────────────────────────┘
*┌──────────────────────────────────────────────────────────────┐
*│　命名空间： JZ.IMS.Repository.Oracle                                  
*│　类    名： SfcsEquipRepairDetailRepository                                      
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
using System.Collections.Generic;

namespace JZ.IMS.Repository.Oracle
{
	public class SfcsEquipRepairDetailRepository : BaseRepository<SfcsEquipRepairDetail, Decimal>, ISfcsEquipRepairDetailRepository
	{
		public SfcsEquipRepairDetailRepository(IOptionsSnapshot<DbOption> options)
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
			string sql = "SELECT ENABLED FROM SFCS_EQUIP_REPAIR_DETAIL WHERE ID=:ID AND IS_DELETE='N'";
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
			string sql = "UPDATE SFCS_EQUIP_REPAIR_DETAIL set ENABLED=:ENABLED where  Id=:Id";
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
			string sql = "SELECT SFCS_EQUIP_REPAIR_DETAIL_SEQ.NEXTVAL MY_SEQ FROM DUAL";
			var result = await _dbConnection.ExecuteScalarAsync(sql);
			return (decimal)result;
		}

		/// <summary>
		/// 根据维修记录获取维修配件信息
		/// </summary>
		/// <param name="headId">维修记录ID</param>
		/// <returns></returns>
		public async Task<IEnumerable<SfcsEquipRepairDetailListModel>> GetEquipRepairDetail(decimal headId)
		{
			string sql = "select * from SFCS_EQUIP_REPAIR_DETAIL where EQUIP_REPAIR_HEAD_ID=:EQUIP_REPAIR_HEAD_ID";
			return await _dbConnection.QueryAsync<SfcsEquipRepairDetailListModel>(sql, new { EQUIP_REPAIR_HEAD_ID = headId });
		}
	}
}