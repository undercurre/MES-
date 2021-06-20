using Dapper;
using JZ.IMS.Core.DbHelper;
using JZ.IMS.Core.Options;
using JZ.IMS.Core.Repository;
using JZ.IMS.IRepository.MesTongs;
using JZ.IMS.Models;
using JZ.IMS.ViewModels.MesTongs;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JZ.IMS.Repository.Oracle.MesTongs
{
	public class MesTongsBoardRepository : BaseRepository<MesTongsInfo, Decimal>, IMesTongsBoardRepository
	{
		public MesTongsBoardRepository(IOptionsSnapshot<DbOption> options)
		{
			_dbOption = options.Get("iWMS");
			if (_dbOption == null)
			{
				throw new ArgumentNullException(nameof(DbOption));
			}
			_dbConnection = ConnectionFactory.CreateConnection(_dbOption.DbType, _dbOption.ConnectionString);
		}

		/// <summary>
		/// 获取夹具申请数量信息
		/// </summary>
		/// <returns></returns>
		public async Task<MesTongsBoardDataModel> GetApplyQty()
		{
			string sql = @"SELECT SUM (CASE WHEN STATUS = 0 THEN QTY ELSE 0 END) AS NOT_AUDIT_QTY,
							   SUM (CASE WHEN STATUS = 1 THEN SURPLUS_QTY ELSE 0 END)
								  AS NOT_STORE_QTY
						  FROM MES_TONGS_APPLY";

			return (await _dbConnection.QueryAsync<MesTongsBoardDataModel>(sql)).FirstOrDefault();
		}

		/// <summary>
		/// 获取夹具状态分布信息
		/// </summary>
		/// <returns></returns>
		public async Task<IEnumerable<MesTongsBoardDataModel>> GetTongsStatusDis()
		{
			string sql = @"SELECT (CASE STATUS
							   WHEN 0 THEN '待入库'
							   WHEN 1 THEN '存储中'
							   WHEN 2 THEN '借出'
							   WHEN 4 THEN '保养中'
							   WHEN 5 THEN '维修中'
							   WHEN 6 THEN '已报废'
							   WHEN 7 THEN '永久借出'
							   ELSE '其它'
							END)
							  AS STATUS_NAME,
						 (CASE STATUS
							 WHEN 0 THEN 'gray'
							 WHEN 1 THEN 'green'
							 WHEN 2 THEN 'purple'
							 WHEN 4 THEN 'blue'
							 WHEN 5 THEN 'orange'
							 WHEN 6 THEN 'red'
							 WHEN 7 THEN '#a15151'
							 ELSE '#fff'
						  END)
							AS COLOR,
						   QTY
					  FROM (  SELECT STATUS, COUNT (*) AS QTY
								FROM MES_TONGS_INFO
							GROUP BY STATUS) TAB ORDER BY STATUS";

			return await _dbConnection.QueryAsync<MesTongsBoardDataModel>(sql);
		}

		/// <summary>
		/// 获取申请信息列表
		/// </summary>
		/// <returns></returns>
		public async Task<IEnumerable<MesTongsBoardDataModel>> GetApplyList()
		{
			string sql = @"  SELECT P.PART_NO,
									 P.PART_NAME,
									 P.PART_DESC,
									 P.VERSION,
									 MST.ID,
									 MST.QTY,
									 MST.SURPLUS_QTY,
									 MST.TONGS_TYPE,
									 MST.STATUS,
									 MST.NEED_DATE,
									 MST.NEED_DATE - TO_DATE (TO_CHAR (SYSDATE, 'YYYY-MM-DD'), 'YYYY-MM-DD') AS DIFF_DAY
								FROM MES_TONGS_APPLY MST
									 LEFT JOIN MES_TONGS_APPLY_PART P ON P.TONGS_APPLY_ID = MST.ID
							   WHERE MST.NEED_DATE <=
										TO_DATE (TO_CHAR (SYSDATE + 15, 'YYYY-MM-DD'), 'YYYY-MM-DD')
							ORDER BY MST.NEED_DATE";

			return await _dbConnection.QueryAsync<MesTongsBoardDataModel>(sql);
		}

		/// <summary>
		/// 获取夹具借出信息列表
		/// </summary>
		/// <returns></returns>
		public async Task<IEnumerable<MesTongsBoardDataModel>> GetTongsBorrowList()
		{
			string sql = @"SELECT INFO.ID,
									CODE,
									TONGS_TYPE,
									UPDATE_DATE,
									USER_NAME,
									REMARK,
									PART_NO,
									PART_NAME,
									PART_DESC,
									VERSION
							FROM MES_TONGS_INFO INFO
									INNER JOIN MES_TONGS_PART P ON INFO.ID = P.TONGS_ID
									LEFT JOIN
									(SELECT ID,
											TONGS_ID,
											OPERATION_TYPE,
											SUBSTR (REMARK, 0,INSTR (REMARK,'|',1,1)- 1)
											AS USER_NAME,
											SUBSTR (REMARK,
													INSTR (REMARK,'|',1,1)+ 1,
													LENGTH (REMARK))
											AS REMARK,
											RANK () OVER (PARTITION BY TONGS_ID ORDER BY CREATE_DATE DESC)
											ROW_NO
									FROM MES_TONGS_OPERATION_HISTORY
									WHERE OPERATION_TYPE IN (4, 9)) T
									ON INFO.ID = T.TONGS_ID AND T.ROW_NO = 1
							WHERE INFO.STATUS IN (2, 7) AND INFO.UPDATE_DATE >= TO_DATE(TO_CHAR(SYSDATE-15,'YYYY-MM-DD'),'YYYY-MM-DD')
						ORDER BY INFO.UPDATE_DATE DESC";

			return await _dbConnection.QueryAsync<MesTongsBoardDataModel>(sql);
		}

	}
}
