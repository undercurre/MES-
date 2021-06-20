/**
*┌──────────────────────────────────────────────────────────────┐
*│　描    述：手插件物料状态监听表接口实现                                                    
*│　作    者：嘉志科技                                            
*│　版    本：2.0    模板代码自动生成                                                
*│　创建时间：2019-11-19 20:26:22                             
*└──────────────────────────────────────────────────────────────┘
*┌──────────────────────────────────────────────────────────────┐
*│　命名空间： JZ.IMS.Repository.Oracle                                  
*│　类    名： MesHiMaterialListenRepository                                      
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
using JZ.IMS.ViewModels.HiPTS;
using System.Collections.Generic;

namespace JZ.IMS.Repository.Oracle
{
    public class MesHiMaterialListenRepository:BaseRepository<MesHiMaterialListen, Decimal>, IMesHiMaterialListenRepository
    {
        public MesHiMaterialListenRepository(IOptionsSnapshot<DbOption> options)
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
			string sql = "SELECT ENABLED FROM MES_HI_MATERIAL_LISTEN WHERE ID=:ID AND IS_DELETE='N'";
			var result = await _dbConnection.QueryFirstOrDefaultAsync<string>(sql, new
			{
				ID = id,
			});

			return result == "Y" ? true : false;
		}

        private async Task<string> GetLineWoBatchNoAsync(decimal lineId)
        {
            string batchSql = "select BATCH_NO from SFCS_PRODUCTION where FINISHED IN ( 'N','O') AND LINE_ID = :LINE_ID";
            String batchNo = await _dbConnection.QueryFirstOrDefaultAsync<String>(batchSql, new
            {
                LINE_ID = lineId
            });
            return batchNo;
        }

        /// <summary>
        /// 根据线别ID获取到上料看板预警值
        /// </summary>
        /// <param name="lineId"></param>
        /// <returns></returns>
        public async Task<decimal> GetWarnValueByLineId(decimal lineId)
        {
            string sql = "SELECT MEANING FROM SFCS_PARAMETERS WHERE LOOKUP_TYPE = 'HIPTS_ALARM' AND LOOKUP_CODE = :LINE_ID AND ENABLED='Y'";
            decimal value = await _dbConnection.ExecuteScalarAsync<decimal>(sql, new
            {
                LINE_ID = lineId
            });
            if (value == 0)
                value = 200;
            return value;
        }

        public async Task<IEnumerable<MesHiMaterialListenReelsModel>> GetHiMateriaListenReelByLine(decimal lineId)
        {
            String batchNo = await GetLineWoBatchNoAsync(lineId);
            if (String.IsNullOrEmpty(batchNo))
            {
                return null;

            }
            String materilListenReelSql = @"SELECT MHML.*,SO.DESCRIPTION AS OPERATION_NAME,IP.NAME PN_NAME, DECODE(SIGN(MHML.PRE_QTY- MHML.USED_QTY ),0,0,-1,0,TRUNC((MHML.PRE_QTY-MHML.USED_QTY)/MHML.UNITY_QTY)) PCB_QTY,
            MHR.* 
                FROM  MES_HI_MATERIAL_LISTEN MHML ,
                (select IR.DATE_CODE ,IR.LOT_CODE, IV.CODE ,IV.NAME, MHR.REEL_ID, MHR.PART_NO REEL_PN, MHR.OPERTOR, MHR.CREATE_TIME, MHR.MES_USER,MHR.QTY
                  from IMS_REEL IR , MES_HI_REEL MHR, IMS_VENDOR  IV 
                  WHERE IR.CODE = MHR.REEL_ID AND IR.VENDOR_ID = IV.ID(+)
                  AND MHR.BATCH_NO = :BATCH_NO ) MHR,
                  IMS_PART IP,
                  SFCS_OPERATIONS SO
                  WHERE BATCH_NO = :BATCH_NO
                  AND MHML.OPERATION_ID = SO.ID
                  AND MHML.BATCH_NO = :BATCH_NO AND MHML.CURR_REEL_ID = MHR.REEL_ID(+)
                  AND  MHML.PART_NO = IP.CODE(+)
                  ORDER BY SO.DESCRIPTION ASC";
            var result = await _dbConnection.QueryAsync<MesHiMaterialListenReelsModel>(materilListenReelSql,
                new
                {
                    BATCH_NO = batchNo
                }
                );
            return result;
        }

        public async Task<IEnumerable<MesAddMaterialModel>> GetAddMaterialModel(decimal lineId)
        {
            String batchNo = await GetLineWoBatchNoAsync(lineId);
            if (String.IsNullOrEmpty(batchNo))
            {
                return null;

            }
            String addMaterialSql = @"select SOS.OPERATION_SITE_NAME  ,
                     IPN.NAME PART_NAME, MHR.PART_NO,
                    MHR.REEL_ID , MHR.QTY, 
                    MHR.ORG_QTY, MHR.USED_QTY,
                     SP.CHINESE AS STATUS ,
                     MHR.CREATE_TIME,
                     MHR.OPERTOR,
                     MHR.MES_USER,
                     IR.LOT_CODE,
                     IR.DATE_CODE,
                     IV.NAME V_NAME
                  from  
                  MES_HI_REEL MHR, 
                  IMS_REEL IR, 
                  IMS_VENDOR IV ,
                  SFCS_PARAMETERS SP,
                  SFCS_OPERATION_SITES SOS,
                  IMS_PART IPN
                  where MHR.REEL_ID = IR.CODE 
                  AND IR.VENDOR_ID = IV.ID(+)
                  AND IR.PART_ID = IPN.ID
                  AND SP.LOOKUP_TYPE = 'HI_RESOURCES_STATUS'
                  AND SP.LOOKUP_CODE = MHR.STATUS
                  AND MHR.OPERATION_SITE_ID = SOS.ID
                  AND MHR.BATCH_NO = :BATCH_NO
                   ORDER BY  MHR.CREATE_TIME DESC";
            var result = await _dbConnection.QueryAsync<MesAddMaterialModel>(addMaterialSql,
                new
                {
                    BATCH_NO = batchNo
                }
                );
            return result;
        }

        /// <summary>
        /// 修改激活状态
        /// </summary>
        /// <param name="id">主键</param>
        /// <param name="status">更改后的状态</param>
        /// <returns></returns>
		public async Task<decimal> ChangeEnableStatus(decimal id, bool status)
		{
			string sql = "UPDATE MES_HI_MATERIAL_LISTEN set ENABLED=:ENABLED where  Id=:Id";
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
			string sql = "SELECT MES_HI_MATERIAL_LISTEN_SEQ.NEXTVAL MY_SEQ FROM DUAL";
			var result = await _dbConnection.ExecuteScalarAsync(sql);
			return (decimal)result;
		}
    }
}