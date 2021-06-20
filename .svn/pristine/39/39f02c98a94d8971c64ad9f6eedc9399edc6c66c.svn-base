using System;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using Dapper;
using Microsoft.Extensions.Options;
using JZ.IMS.Core.DbHelper;
using JZ.IMS.Core.Options;
using JZ.IMS.IRepository;
using JZ.IMS.ViewModels;

namespace JZ.IMS.Repository.Oracle
{
	class SfcsEquipmentLinesRepository : ISfcsEquipmentLinesRepository
	{
		private DbOption _dbOption;
		private IDbConnection _dbConnection;

		public SfcsEquipmentLinesRepository(IOptionsSnapshot<DbOption> options)
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
		public List<SfcsEquipmentLinesModel> GetLinesList()
		{
			string sql = "SELECT * FROM (select ID, LINE_NAME from SMT_LINES union select ID,OPERATION_LINE_NAME AS LINE_NAME from SFCS_OPERATION_LINES WHERE ENABLED = 'Y') ORDER BY LINE_NAME desc";
			return (_dbConnection.Query<SfcsEquipmentLinesModel>(sql)).ToList();
		}

		/// <summary>
		/// 根据组织ID获取所有线别信息
		/// </summary>
		/// <param name="organizeId">组织ID，默认为“1”（集团）</param>
		/// <returns></returns>
		public List<AllLinesModel> GetLinesList(string organizeId = "1", string lineType = "")
		{

			if (string.IsNullOrEmpty(organizeId)) organizeId = "1";
			string sql = @" SELECT L.*
							FROM V_MES_LINES L INNER JOIN SYS_ORGANIZE_LINE O ON L.LINE_ID = O.LINE_ID {0}
							WHERE EXISTS
									(SELECT 1
										FROM (    SELECT ID
													FROM SYS_ORGANIZE
												START WITH ID = :ORGANIZE_ID
												CONNECT BY PRIOR ID = PARENT_ORGANIZE_ID)
										WHERE ID = O.ORGANIZE_ID)
						ORDER BY O.ORGANIZE_ID, O.LINE_TYPE, O.ATTRIBUTE6, TO_NUMBER (O.ATTRIBUTE5)";

			string sSqlLineType = "";
			if (lineType == "SMT" || lineType == "PCBA")
			{
				sSqlLineType = " AND L.LINE_TYPE = '" + lineType + "' ";
			}

			return (_dbConnection.Query<AllLinesModel>(string.Format(sql, sSqlLineType), new { ORGANIZE_ID = organizeId })).ToList();
		}

		/// <summary>
		/// 获取产线列表
		/// </summary>
		/// <returns></returns>
		public List<SfcsEquipmentLinesModel> GetRoHSLinesList()
		{
			string sql = "select ID,OPERATION_LINE_NAME AS LINE_NAME from SFCS_OPERATION_LINES WHERE ENABLED = 'Y' ORDER BY OPERATION_LINE_NAME ";
			return (_dbConnection.Query<SfcsEquipmentLinesModel>(sql)).ToList();
		}

		/// <summary>
		/// 获取SMT线列表
		/// </summary>
		/// <returns></returns>
		public List<SfcsEquipmentLinesModel> GetSMTLinesList()
		{
			string sql = "select ID, LINE_NAME from SMT_LINES ORDER BY id";
			return (_dbConnection.Query<SfcsEquipmentLinesModel>(sql)).ToList();
		}

        /// <summary>
		/// 获取所有线体类型
		/// </summary>
		/// <returns></returns>
		public List<SfcsLineView> GetVMesLinesList()
        {
            string sql = "SELECT * FROM V_MES_LINES";
            return (_dbConnection.Query<SfcsLineView>(sql)).ToList();
        }
    }
}
