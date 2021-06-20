/*
*┌──────────────────────────────────────────────────────────────┐
*│　描    述：接口实现                                                    
*│　作    者：嘉志科技                                            
*│　版    本：2.0    模板代码自动生成                                                
*│　创建时间：2020-07-22 10:16:13                             
*└──────────────────────────────────────────────────────────────┘
*┌──────────────────────────────────────────────────────────────┐
*│　命名空间： JZ.IMS.Repository.Oracle                                  
*│　类    名： MesBurnFileApplyRepository                                      
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
using System.Data;
using OfficeOpenXml;
using JZ.IMS.Core.Extensions;
using JZ.IMS.ViewModels.BurnFile;

namespace JZ.IMS.Repository.Oracle
{
    public class AteDeviceRepository : BaseRepository<MesBurnFileApply, Decimal>, IAteDeviceRepository
    {
        public AteDeviceRepository(IOptionsSnapshot<DbOption> options)
        {
            _dbOption = options.Get("iWMS");
            if (_dbOption == null)
            {
                throw new ArgumentNullException(nameof(DbOption));
            }
            _dbConnection = ConnectionFactory.CreateConnection(_dbOption.DbType, _dbOption.ConnectionString);
        }

        /// <summary>
        /// 通过SN找工单
        /// </summary>
        /// <param name="SN"></param>
        /// <returns></returns>
        public async Task<string> GetWONOBySN(string SN)
        {
            try
            {
                var p = new DynamicParameters();
                string message = "";
                string trace = "";
                string v_wo_no = "";
                //p.Add(":P_SN", SN, DbType.String, ParameterDirection.Input, 80);
                //p.Add(":P_UPDATE_RANGER_INPUTTED", 'Y', DbType.String, ParameterDirection.Input, 20);
                //p.Add(":P_MESSAGE", message, DbType.String, ParameterDirection.Output, 200);//out
                //p.Add(":P_TRACE", trace, DbType.String, ParameterDirection.Output, 200);//out
                //p.Add(":P_LANGUAGE", "EN", DbType.String, ParameterDirection.Input, 20);
                p.Add(":SN", SN, DbType.String, ParameterDirection.Input, 80);
                p.Add(":WO_NO", v_wo_no, DbType.String, ParameterDirection.Output, 200);//out
                var obj =await _dbConnection.ExecuteAsync("GET_SFCS_WO_BY_SN_EX", p, commandType: CommandType.StoredProcedure);
                String wono = p.Get<String>(":WO_NO");
                return !wono.IsNullOrWhiteSpace() ? wono : string.Empty;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        /// <summary>
        /// 检查SN状态和制程是否漏刷
        /// </summary>
        /// <returns></returns>
        public async Task<TableDataModel> CheckSNAndRoute(string SN, decimal siteId)
        {

            TableDataModel model = new TableDataModel();
            try
            {
                var p = new DynamicParameters();
                p.Add(":P_SN", SN, DbType.String, ParameterDirection.Input, 80);
                p.Add(":P_SITEID", siteId, DbType.Decimal, ParameterDirection.Input, 20);
                p.Add(":P_RESULT", "", DbType.Decimal, ParameterDirection.Output, 80);
                p.Add(":P_MESSAGE", "", DbType.String, ParameterDirection.Output, 80);
                p.Add(":P_TRACE", "", DbType.String, ParameterDirection.Output, 80);
                p.Add(":P_LANGUAGE", "EN", DbType.String, ParameterDirection.Input, 80);
                await _dbConnection.ExecuteScalarAsync<dynamic>("PROCESS_TEST_REQUEST", p, commandType: CommandType.StoredProcedure);
                //p_result:
                //0: Process OK
                //1: Process Error
                decimal result = p.Get<decimal>(":P_RESULT");
                string message = p.Get<string>(":P_MESSAGE").ToString();
                string trace = p.Get<string>(":P_TRACE").ToString();
                model.msg = message;
                model.code =Convert.ToInt32(result);
            }
            catch (Exception ex)
            {
                model.code = 1;
                throw new Exception(ex.Message);
            }
            return model;
        }


        /// <summary>
        /// 普通过站
        /// </summary>
        /// <returns></returns>
        public async Task<TableDataModel> ProcessMultiOperation(string SN, decimal operationID,decimal siteId)
        {

            TableDataModel model = new TableDataModel();
            try
            {
                var p = new DynamicParameters();
                p.Add(":P_SN", SN, DbType.String, ParameterDirection.Input, 80);
                p.Add(":P_OPERATION_ID", operationID, DbType.Decimal, ParameterDirection.Input, 20);
                p.Add(":P_DEFECT_CODES", "", DbType.String, ParameterDirection.Input, 80);
                p.Add(":P_DEFECTS_DETAIL", "", DbType.String, ParameterDirection.Input, 80);
                p.Add(":P_OPERATOR","", DbType.String, ParameterDirection.Input, 80);
                p.Add(":P_OPERATION_SITE_ID", siteId, DbType.Decimal, ParameterDirection.Output, 80);
                p.Add(":p_language", "EN", DbType.String, ParameterDirection.Input, 80);
                p.Add(":p_result", 0, DbType.Decimal, ParameterDirection.Output, 20);
                p.Add(":p_ratio", "", DbType.String, ParameterDirection.Output, 80);
                p.Add(":p_deliver_count", 0, DbType.Decimal, ParameterDirection.Output, 20);
                p.Add(":p_need_to_spotcheck", 0, DbType.Decimal, ParameterDirection.Output, 20);
                p.Add(":p_message", "", DbType.String, ParameterDirection.Output, 80);
                p.Add(":p_trace", "", DbType.String, ParameterDirection.Output, 80);
                await _dbConnection.ExecuteScalarAsync<dynamic>("SFCS_COMMON_OPERATION_PKG.PROCESS_MULTI_OPERATION19", p, commandType: CommandType.StoredProcedure);
                //p_result:
                //0: Process OK
                //1: Process Error
                decimal result = p.Get<decimal>(":p_result");
                string message = p.Get<string>(":p_message").ToString();
                string trace = p.Get<string>(":p_trace").ToString();
                model.msg = message;
                model.code = Convert.ToInt32(result);
            }
            catch (Exception ex)
            {
                model.code = 1;
                throw new Exception(ex.Message);
            }
            return model;
        }
    }
}