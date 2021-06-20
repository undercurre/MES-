/**
*┌──────────────────────────────────────────────────────────────┐
*│　描    述：接口实现                                                    
*│　作    者：嘉志科技                                            
*│　版    本：2.0    模板代码自动生成                                                
*│　创建时间：2020-10-07 13:42:53                             
*└──────────────────────────────────────────────────────────────┘
*┌──────────────────────────────────────────────────────────────┐
*│　命名空间： JZ.IMS.Repository.Oracle                                  
*│　类    名： SmtDrivingRecordMstRepository                                      
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
using JZ.IMS.Core.Extensions;

namespace JZ.IMS.Repository.Oracle
{
    public class SfcsEquipWaveDefMstRepository : BaseRepository<SfcsEquipWaveDefMst, Decimal>, ISfcsEquipWaveDefMstRepository
    {
        public SfcsEquipWaveDefMstRepository(IOptionsSnapshot<DbOption> options)
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
			string sql = "SELECT ENABLED FROM SFCS_EQUIP_WAVE_DEF_MST WHERE ID=:ID";
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
			string sql = "UPDATE SFCS_EQUIP_WAVE_DEF_MST set ENABLED=:ENABLED WHERE ID=:Id";
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
			string sql = "SELECT SFCS_EQUIP_WAVE_DEF_MST_SEQ.NEXTVAL MY_SEQ FROM DUAL";
			var result = await _dbConnection.ExecuteScalarAsync(sql);
			return (decimal)result;
		}

        // <summary>
        /// 获取数据
        /// </summary>
        /// <returns></returns>
        public async Task<dynamic> LoadData(SfcsEquipWaveDefMstRequestModel model) {
            string sql = @"SELECT A.*,B.PART_NO,C.MODEL,ROWNUM AS rowno FROM SFCS_EQUIP_WAVE_DEF_MST A 
                            LEFT JOIN SFCS_WO B ON A.WO_NO = B.WO_NO 
                            LEFT JOIN SFCS_MODEL C ON B.MODEL_ID = C.ID";
            var conditions = "WHERE 1=1";
            //状态
            if (!model.STATUS.IsNullOrWhiteSpace())
            {
                conditions += " AND STATUS = :STATUS";
            }
            //工单
            if (!model.WO_NO.IsNullOrWhiteSpace())
            {
                conditions += " AND A.WO_NO = :WO_NO";
            }
            //线别
            if (model.LINE_ID > 0)
            {
                conditions += " AND LINE_ID = :LINE_ID";
            }
            conditions += " ORDER BY A.ID DESC ";
            string pagedSql = SQLBuilderClass.GetPagedSQL(sql, conditions);
            var resdata = await _dbConnection.QueryAsync<dynamic>(pagedSql, model);
            return resdata;
        }
    }
}