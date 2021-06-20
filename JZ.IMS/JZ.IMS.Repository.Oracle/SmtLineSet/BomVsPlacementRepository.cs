/**
*┌──────────────────────────────────────────────────────────────┐
*│　描    述：接口实现                                                    
*│　作    者：嘉志科技                                            
*│　版    本：2.0    模板代码自动生成                                                
*│　创建时间：2020-03-17 16:44:25                             
*└──────────────────────────────────────────────────────────────┘
*┌──────────────────────────────────────────────────────────────┐
*│　命名空间： JZ.IMS.Repository.Oracle                                  
*│　类    名： SmtLineConfigRepository                                      
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
using JZ.IMS.ViewModels.BomVsPlacement;
using JZ.IMS.Core.Utilities;
using JZ.IMS.WebApi.Common;

namespace JZ.IMS.Repository.Oracle
{
    public class BomVsPlacementRepository : BaseRepository<SmtBom1, Decimal>, IBomVsPlacementRepository
    {
        public BomVsPlacementRepository(IOptionsSnapshot<DbOption> options)
        {
            _dbOption = options.Get("iWMS");
            if (_dbOption == null)
            {
                throw new ArgumentNullException(nameof(DbOption));
            }
            _dbConnection = ConnectionFactory.CreateConnection(_dbOption.DbType, _dbOption.ConnectionString);
        }

        /// <summary>
        /// 料单线别
        /// </summary>
        /// <returns></returns>
        public async Task<List<CodeName>> GetStationKind()
        {
            List<CodeName> result = null;

            string sql = @"SELECT VALUE as Code, CN_DESC as NAME FROM SMT_LOOKUP WHERE TYPE = 'STATION_KIND' 
                           AND VALUE IN('AIRI', 'RI', 'PANASONIC', 'PANASONIC_CM')";
            var tmpdata = await _dbConnection.QueryAsync<CodeName>(sql);

            if (tmpdata != null)
            {
                result = tmpdata.ToList();
            }
            return result;
        }

        /// <summary>
        /// 同步ERP的数据到SMT_BOM1、STM_BOM2
        /// </summary>
        /// <param name="ProductId"></param>
        /// <param name="Type"></param>
        /// <param name="userName"></param>
        /// <returns></returns>
        public async Task<string> SyncBomByProdectId(string ProductId, string Type, string userName)
        {
            List<ErpBom> dtErpBom = null;
            try
            {
                //通过ProductId获取ERP中的bom数据
                string sql = @"SELECT * FROM ""v_BOM_For_MES""@erp where ""ProductID"" = '{0}' and ""MaterialStatus"" = 'Y' and ""Type"" = '{1}'";
                dtErpBom = (await _dbConnection.QueryAsync<ErpBom>(string.Format(sql, ProductId, Type)))?.ToList();
            }
            catch (Exception)
            {
                dtErpBom = null;
            }

            if (dtErpBom != null)//获取ERP数据成功
            {
                if (dtErpBom.Count == 0)
                {
                    return "";
                }

                ConnectionFactory.OpenConnection(_dbConnection);
                using (var tran = _dbConnection.BeginTransaction())
                {
                    try
                    {
                        //1、清除原来有的BOM数据
                        String bom1SelectSql = "SELECT * FROM SMT_BOM1 WHERE PARTENT_CODE = :PARTENT_CODE AND BOM_TYPE = :BOM_TYPE";
                        var currentSmtBom1Table = await _dbConnection.QueryAsync<SmtBom1>(bom1SelectSql, new
                        {
                            PARTENT_CODE = ProductId,
                            BOM_TYPE = Type,
                        }, tran);
                        if (currentSmtBom1Table != null && currentSmtBom1Table.Count() > 0)
                        {
                            string currentBomId = currentSmtBom1Table.FirstOrDefault().BOM_ID;
                            string deleteBom1Sql = "DELETE FROM SMT_BOM1 WHERE BOM_ID = :BOM_ID";
                            string deleteBom2Sql = "DELETE FROM SMT_BOM2 WHERE BOM_ID = :BOM_ID";

                            await _dbConnection.ExecuteAsync(deleteBom1Sql, new { BOM_ID = currentBomId }, tran);
                            await _dbConnection.ExecuteAsync(deleteBom2Sql, new { BOM_ID = currentBomId }, tran);
                        }
                        //2、新增同步ERP过来的数据
                        string bomId = Guid.NewGuid().ToString();

                        string insertBom1Sql = @"INSERT INTO SMT_BOM1(SHEET_NO,SHEET_TYPE,PARTENT_CODE,CREATE_DATE,ALTER_DATE,SHEET_DATE,SHEET_STA,
                                      AUDIT_DATE,USER_NO,AUDIT_USER,BOM_ID,VERSION_TIMES,BOM_TYPE) 
                                      VALUES ('0','0',:PARTENT_CODE,sysdate,sysdate,'0','1',sysdate,:USER_NO,:AUDIT_USER,:BOM_ID,'000',:BOM_TYPE)";
                        await _dbConnection.ExecuteAsync(insertBom1Sql, new
                        {
                            PARTENT_CODE = ProductId,
                            BOM_ID = bomId,
                            USER_NO = userName,
                            AUDIT_USER = userName,
                            BOM_TYPE = Type,
                        }, tran);

                        string insertBom2Sql = @"INSERT INTO SMT_BOM2(SHEET_NO,SHEET_LOT,PART_CODE,UNIT_QTY,UNIT_CODE,SCRAP_RATE,PART_LOCATION,REPLACE_SW,
                                     BOM_D_ID,EFFECTIVE_DATE,EXPRITY_DATE,BOM_ID,COMPONENT_LOCATION,REMARK,PART_NAME,PART_MODEL) 
                                     VALUES ('0','0',:PART_CODE,:UNIT_QTY,:UNIT_CODE,0,0,0,:BOM_D_ID,:EFFECTIVE_DATE,:EXPRITY_DATE,:BOM_ID,:COMPONENT_LOCATION,'',:PART_NAME,:PART_MODEL)";
                        foreach (var item in dtErpBom)
                        {
                            await _dbConnection.ExecuteAsync(insertBom2Sql, new
                            {
                                PART_CODE = item.MaterialID,
                                UNIT_QTY = item.QTY,
                                UNIT_CODE = item.PCS,
                                BOM_D_ID = Guid.NewGuid().ToString(),
                                EFFECTIVE_DATE = new DateTime(9998, 12, 31),
                                EXPRITY_DATE = new DateTime(9998, 12, 31),
                                BOM_ID = bomId,
                                COMPONENT_LOCATION = Convert.ToString(item.ScreenPrint).ExpandRange(new char[] { ',', ' ', ';', '|', '.' }, new char[] { '-' }),
                                PART_NAME = item.MaterialName,
                                PART_MODEL = item.MaterialModel,
                            }, tran);
                        }

                        tran.Commit();
                        return bomId;
                    }
                    catch (Exception ex)
                    {
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
            }
            else//获取ERP数据失败
            {
                //查询MES系统的smt_bom1的bom_id
                string sql = @"SELECT BOM_ID FROM SMT_BOM1 WHERE PARTENT_CODE = '{0}' AND BOM_TYPE = '{1}'";
                Object objBomID = await _dbConnection.ExecuteScalarAsync(string.Format(sql, ProductId, Type));
                return Convert.ToString(objBomID ?? string.Empty);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="bom_id"></param>
        /// <param name="bomtype"></param>
        /// <returns></returns>
        public async Task<List<BOMData>> ExploreBom2(string bom_id, BomType bomtype)
        {
            List<BOMData> bom = null;
            string cmd = "SELECT * FROM SMT_BOM2 WHERE BOM_ID = '{0}' AND REPLACE_SW = '0'";
            try
            {
                bom = (await _dbConnection.QueryAsync<BOMData>(string.Format(cmd, bom_id)))?.ToList();
            }
            catch (Exception)
            {
            }
            return bom;
        }

      
        /// <summary>
        /// 获取机台的配置
        /// </summary>
        /// <param name="StationName">机台名</param>
        /// <returns></returns>
        public async Task<SmtStationConfig> GetStationConfig(string StationName,List<string> stationIdArray)
        {
            SmtStationConfig modelArrary = new SmtStationConfig();
            string arrary = string.Join(',',stationIdArray);
            string cmd = $@"select t.STATION_ID from 
                            (SELECT STATION_ID,VALUE FROM SMT_STATION_CONFIG SSC
                               WHERE SSC.ENABLED = 'Y' AND SSC.CONFIG_TYPE IN (
                               SELECT CODE FROM SMT_LOOKUP WHERE TYPE = 'MACHINECONFIG' AND VALUE = 'MACHINE NAME' AND ENABLED = 'Y') 
                            and ssc.STATION_ID in ({arrary}))t where UPPER(t.VALUE)=UPPER(:VALUE)";
            try
            {
                modelArrary=(await _dbConnection.QueryAsync<SmtStationConfig>(cmd, new
                {
                    VALUE = StationName
                }))?.ToList().FirstOrDefault();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            return modelArrary;
        }

    }
}