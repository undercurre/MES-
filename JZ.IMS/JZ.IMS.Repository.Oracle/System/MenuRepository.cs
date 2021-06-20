/**
*┌──────────────────────────────────────────────────────────────┐
*│　描    述：后台管理菜单接口实现                                                    
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

namespace JZ.IMS.Repository.Oracle
{
	public class MenuRepository : BaseRepository<Sys_Menu, decimal>, IMenuRepository
	{
		public MenuRepository(IOptionsSnapshot<DbOption> options)
		{
			_dbOption = options.Get("iWMS");
			if (_dbOption == null)
			{
				throw new ArgumentNullException(nameof(DbOption));
			}
			_dbConnection = ConnectionFactory.CreateConnection(_dbOption.DbType, _dbOption.ConnectionString);
		}

		public async Task<decimal> ChangeDisplayStatusByIdAsync(decimal id, bool status)
		{
			string sql = "update SYS_Menu set ENABLED=:IsDisplay where  Id=:Id";
			return await _dbConnection.ExecuteAsync(sql, new
			{
				IsDisplay = status ? "Y" : "N",
				Id = id,
			});
		}

		public decimal DeleteLogical(decimal[] ids)
		{
			string sql = "update SYS_Menu set Is_Delete='Y' where Id in :Ids";
			return _dbConnection.Execute(sql, new
			{
				Ids = ids
			});
		}

		public async Task<decimal> DeleteLogicalAsync(decimal[] ids)
		{
			string sql = "update SYS_Menu set Is_Delete='Y' where Id in :Ids";
			return await _dbConnection.ExecuteAsync(sql, new
			{
				Ids = ids
			});
		}

		public async Task<decimal> DeleteSubAsync(decimal id) {
			string sql = "delete from SYS_Menu where Id =:Ids";
			return await _dbConnection.ExecuteAsync(sql, new {
				Ids = id
			});
		}

		public async Task<Boolean> GetDisplayStatusByIdAsync(decimal id)
		{
			string sql = "select ENABLED from SYS_Menu where Id=:Id and Is_Delete='N'";
			var result = await _dbConnection.QueryFirstOrDefaultAsync<string>(sql, new
			{
				Id = id,
			});

			if (result == "Y")
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
			string sql = "select Id from SYS_Menu where Name=:Name and Is_Delete='N'";
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
			string sql = "select Id from SYS_Menu where Name=:Name and Id <> :Id and Is_Delete='N'";
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

		public async Task<Boolean> IsExistsLinkUrlAsync(string LinkUrl)
		{
			string sql = "select Id from SYS_Menu where LINK_URL=:LINK_URL and Is_Delete='N'";
			var result = await _dbConnection.QueryAsync<decimal>(sql, new
			{
				LINK_URL = LinkUrl,
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

		public async Task<Boolean> IsExistsLinkUrlAsync(string LinkUrl, decimal Id)
		{
			string sql = "select Id from SYS_Menu where LINK_URL=:LINK_URL and Id <> :Id and Is_Delete='N'";
			var result = await _dbConnection.QueryAsync<decimal>(sql, new
			{
				LINK_URL = LinkUrl,
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
			string sql = "SELECT Sys_Menu_SEQ.NEXTVAL MY_SEQ FROM DUAL";
			var result = await _dbConnection.ExecuteScalarAsync(sql);
			return (decimal)result;
		}
	}
}