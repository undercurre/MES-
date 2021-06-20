/**
*┌──────────────────────────────────────────────────────────────┐
*│　描    述：接口实现                                                    
*│　作    者：嘉志科技                                            
*│　版    本：2.0    模板代码自动生成                                                
*│　创建时间：2020-05-18 17:29:33                             
*└──────────────────────────────────────────────────────────────┘
*┌──────────────────────────────────────────────────────────────┐
*│　命名空间： JZ.IMS.Repository.Oracle                                  
*│　类    名： MesQualityItemsRepository                                      
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
using System.Linq;
using System.Collections.Generic;

namespace JZ.IMS.Repository.Oracle
{
    public class MesQualityItemsRepository : BaseRepository<MesQualityItems, Decimal>, IMesQualityItemsRepository
    {
        public MesQualityItemsRepository(IOptionsSnapshot<DbOption> options)
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
            string sql = "SELECT ENABLED FROM MES_QUALITY_ITEMS WHERE ID=:ID";
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
            string sql = "UPDATE MES_QUALITY_ITEMS set ENABLED=:ENABLED WHERE ID=:Id";
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
            string sql = "SELECT MES_QUALITY_ITEMS_SEQ.NEXTVAL MY_SEQ FROM DUAL";
            var result = await _dbConnection.ExecuteScalarAsync(sql);
            return (decimal)result;
        }

        /// <summary>
        /// 获取可用的检验项目
        /// </summary>
        /// <param name="organizeId"></param>
        /// <param name="bill_type"></param>
        /// <returns></returns>
        public List<dynamic> GetItemsData(string organizeId, decimal bill_type, decimal mstId)
        {
            String sql = @"SELECT I.ID AS ITEM_ID,I.CHECK_ITEM,I.CHECK_DESC,I.QUANTIZE_TYPE,I.ISEMPTY,D.RESULT_VALUE,NVL(D.ID ,0) ID,D.RESULT_TYPE FROM MES_QUALITY_ITEMS I 
           LEFT JOIN MES_QUALITY_INFO QI ON I.CHECK_TYPE = QI.CHECK_TYPE 
           LEFT JOIN MES_QUALITY_DETAIL D ON QI.ID = D.MST_ID 
           WHERE I.ENABLED = 'Y' AND I.CHECK_TYPE =:CHECK_TYPE AND QI.ID = :MST_ID";
            List<dynamic> data = _dbConnection.Query<dynamic>(sql + " AND D.ITEM_ID = I.ID", new { ORGANIZE_ID = organizeId, CHECK_TYPE = bill_type, MST_ID = mstId }).ToList();
            if (data == null || data.Count() < 1)
            {
                data = _dbConnection.Query<dynamic>(sql, new { ORGANIZE_ID = organizeId, CHECK_TYPE = bill_type, MST_ID = mstId })?.ToList();
            }
            return data;
        }

        /// <summary>
        /// 获取BOM信息
        /// </summary>
        /// <param name="organizeId"></param>
        /// <param name="bill_type"></param>
        /// <returns></returns>
        public async Task<List<dynamic>> GetBOMData(string partNo,Decimal id)
        {
            List<dynamic> reuslt = new List<dynamic>();
            try
            {
                string sql = @" SELECT B2.COMPONENT_LOCATION,B2.PART_CODE,B2.PART_NAME,B2.BOM_D_ID ID,B2.UNIT_QTY,B2.PART_MODEL,MFRD.TENSION_VALUE,MFRD.TEST_VALUE,MFRD.BRAND_NAME,MFRD.VENDOR_NAME,MFRD.RESULT
                    FROM SMT_BOM1 B1,SMT_BOM2 B2 ,(SELECT * FROM  MES_FIRST_CHECK_RECORD_DETAIL MFRD WHERE  MFRD.HID=TO_CHAR( :HID)) MFRD
                    WHERE B1.PARTENT_CODE =:PARTENT_CODE AND B2.PART_CODE = MFRD.PART_NO(+) 
                    AND B1.BOM_ID=B2.BOM_ID AND B2.COMPONENT_LOCATION IS NOT NULL ORDER BY B2.PART_NAME,B2.PART_CODE";
             var   data = await _dbConnection.QueryAsync<dynamic>(sql, new { PARTENT_CODE = partNo, HID = id });
               reuslt= data?.ToList();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            return reuslt;
        }

    }
}