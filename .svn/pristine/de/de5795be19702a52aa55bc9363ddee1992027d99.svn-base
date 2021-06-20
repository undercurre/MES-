/*
*┌──────────────────────────────────────────────────────────────┐
*│　描    述：自动产生序列号记录表，使用在自动产生卡通号，自动产生栈板号等。接口实现                                                    
*│　作    者：嘉志科技                                            
*│　版    本：2.0    模板代码自动生成                                                
*│　创建时间：2020-10-08 15:38:28                             
*└──────────────────────────────────────────────────────────────┘
*┌──────────────────────────────────────────────────────────────┐
*│　命名空间： JZ.IMS.Repository.Oracle                                  
*│　类    名： SfcsContainerListRepository                                      
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
using System.Text;
using System.Data;
using JZ.IMS.Core.Extensions;

namespace JZ.IMS.Repository.Oracle
{
    public class SfcsContainerListRepository : BaseRepository<SfcsContainerList, String>, ISfcsContainerListRepository
    {
        public SfcsContainerListRepository(IOptionsSnapshot<DbOption> options)
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
            string sql = "SELECT ENABLED FROM SFCS_CONTAINER_LIST WHERE ID=:ID";
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
            string sql = "UPDATE SFCS_CONTAINER_LIST set ENABLED=:ENABLED WHERE ID=:Id";
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
            string sql = "SELECT SFCS_CONTAINER_LIST_SEQ.NEXTVAL MY_SEQ FROM DUAL";
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
            string sql = "select count(0) from SFCS_CONTAINER_LIST where id = :id";
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
        //public async Task<decimal> SaveDataByTrans(SfcsContainerListModel model)
        //{
        //	int result = 1;
        //	ConnectionFactory.OpenConnection(_dbConnection);
        //	using (var tran = _dbConnection.BeginTransaction())
        //	{
        //		try
        //		{
        //			//新增
        //			string insertSql = @"insert into SFCS_CONTAINER_LIST 
        //			(DATA_TYPE,CONTAINER_SN,PART_NO,WO_ID,RUNCARD_SN,OPERATION_CODE,SITE_ID,INVOICE,CUSTOMER_PO,REVISION,QUANTITY,FULL_FLAG,SEQUENCE,CREATED_DATE,UPDATED_DATE) 
        //			VALUES (:DATA_TYPE,:CONTAINER_SN,:PART_NO,:WO_ID,:RUNCARD_SN,:OPERATION_CODE,:SITE_ID,:INVOICE,:CUSTOMER_PO,:REVISION,:QUANTITY,:FULL_FLAG,:SEQUENCE,:CREATED_DATE,:UPDATED_DATE)";
        //			if (model.InsertRecords != null && model.InsertRecords.Count > 0)
        //			{
        //				foreach (var item in model.InsertRecords)
        //				{
        //					var newid = await GetSEQID();
        //					var resdata = await _dbConnection.ExecuteAsync(insertSql, new
        //					{
        //						ID = newid,
        //						item.DATA_TYPE,
        //						item.CONTAINER_SN,
        //						item.PART_NO,
        //						item.WO_ID,
        //						item.RUNCARD_SN,
        //						item.OPERATION_CODE,
        //						item.SITE_ID,
        //						item.INVOICE,
        //						item.CUSTOMER_PO,
        //						item.REVISION,
        //						item.QUANTITY,
        //						item.FULL_FLAG,
        //						item.SEQUENCE,
        //						item.CREATED_DATE,
        //						item.UPDATED_DATE,

        //					}, tran);
        //				}
        //			}
        //			//更新
        //			string updateSql = @"Update SFCS_CONTAINER_LIST set DATA_TYPE=:DATA_TYPE,CONTAINER_SN=:CONTAINER_SN,PART_NO=:PART_NO,WO_ID=:WO_ID,RUNCARD_SN=:RUNCARD_SN,OPERATION_CODE=:OPERATION_CODE,SITE_ID=:SITE_ID,INVOICE=:INVOICE,CUSTOMER_PO=:CUSTOMER_PO,REVISION=:REVISION,QUANTITY=:QUANTITY,FULL_FLAG=:FULL_FLAG,SEQUENCE=:SEQUENCE,CREATED_DATE=:CREATED_DATE,UPDATED_DATE=:UPDATED_DATE  
        //				where ID=:ID ";
        //			if (model.UpdateRecords != null && model.UpdateRecords.Count > 0)
        //			{
        //				foreach (var item in model.UpdateRecords)
        //				{
        //					var resdata = await _dbConnection.ExecuteAsync(updateSql, new
        //					{
        //						item.ID,
        //						item.DATA_TYPE,
        //						item.CONTAINER_SN,
        //						item.PART_NO,
        //						item.WO_ID,
        //						item.RUNCARD_SN,
        //						item.OPERATION_CODE,
        //						item.SITE_ID,
        //						item.INVOICE,
        //						item.CUSTOMER_PO,
        //						item.REVISION,
        //						item.QUANTITY,
        //						item.FULL_FLAG,
        //						item.SEQUENCE,
        //						item.CREATED_DATE,
        //						item.UPDATED_DATE,

        //					}, tran);
        //				}
        //			}
        //			//删除
        //			string deleteSql = @"Delete from SFCS_CONTAINER_LIST where ID=:ID ";
        //			if (model.RemoveRecords != null && model.RemoveRecords.Count > 0)
        //			{
        //				foreach (var item in model.RemoveRecords)
        //				{
        //					var resdata = await _dbConnection.ExecuteAsync(deleteSql, new
        //					{
        //						item.ID
        //					}, tran);
        //				}
        //			}

        //			tran.Commit();
        //		}
        //		catch (Exception ex)
        //		{
        //			result = -1;
        //			tran.Rollback();
        //			throw ex;
        //		}
        //		finally
        //		{
        //			if (_dbConnection.State != System.Data.ConnectionState.Closed)
        //			{
        //				_dbConnection.Close();
        //			}
        //		}
        //	}
        //	return result;
        //}

        /// <summary>
        /// 根据SN更新箱号
        /// </summary>
        /// <param name="carton_no">箱号</param>
        /// <param name="sn_id">产品流水号id</param>
        /// <returns></returns>
        public async Task<int> UpdateCartonNoBySN(string carton_no, string oldCARTON_NO, decimal sn_id)
        {
            int result = 0;
            ConnectionFactory.OpenConnection(_dbConnection);
            using (var tran = _dbConnection.BeginTransaction())
            {
                try
                {
                    string updateSql = @"UPDATE SFCS_RUNCARD SET CARTON_NO=:CARTON_NO WHERE ID=:SN_ID";
                    result = await _dbConnection.ExecuteAsync(updateSql, new
                    {
                        CARTON_NO = carton_no,
                        SN_ID = sn_id
                    }, tran);
                    if (result <= 0) { throw new Exception("UPDATE_CARTONNO_FAILURE"); }

                    decimal qty = QueryEx<decimal>("SELECT COUNT(1) QTY FROM SFCS_RUNCARD WHERE CARTON_NO =:CARTON_NO", new { CARTON_NO = carton_no }).FirstOrDefault();
                    decimal oldqty = QueryEx<decimal>("SELECT COUNT(1) QTY FROM SFCS_RUNCARD WHERE CARTON_NO =:CARTON_NO", new { CARTON_NO = oldCARTON_NO }).FirstOrDefault();

                    String U_UpadateContainerListSeq = @"UPDATE SFCS_CONTAINER_LIST SET SEQUENCE = :SEQUENCE WHERE CONTAINER_SN=:CONTAINER_SN";
                    result = Execute(U_UpadateContainerListSeq, new
                    {
                        SEQUENCE = qty,
                        CONTAINER_SN = carton_no
                    }, tran);
                    if (result <= 0) { throw new Exception("UPDATE_CARTONNO_FAILURE"); }

                    result = Execute(U_UpadateContainerListSeq, new
                    {
                        SEQUENCE = oldqty,
                        CONTAINER_SN = oldCARTON_NO
                    }, tran);
                    if (result <= 0) { throw new Exception("UPDATE_CARTONNO_FAILURE"); }

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

        public async Task<int> UpdateCartonNoBySN(string carton_no, string sn)
        {
            int result = 0;
            ConnectionFactory.OpenConnection(_dbConnection);
            using (var tran = _dbConnection.BeginTransaction())
            {
                try
                {
                    string updateSql = @"UPDATE SFCS_RUNCARD SET CARTON_NO=:CARTON_NO WHERE SN=:SN";
                    result = await _dbConnection.ExecuteAsync(updateSql, new
                    {
                        CARTON_NO = carton_no,
                        SN = sn
                    }, tran);
                    if (result <= 0) { throw new Exception("UPDATE_CARTONNO_FAILURE"); }

                    decimal qty = QueryEx<decimal>("SELECT COUNT(1) QTY FROM SFCS_RUNCARD WHERE CARTON_NO =:CARTON_NO", new { CARTON_NO = carton_no }).FirstOrDefault();

                    String U_UpadateContainerListSeq = @"UPDATE SFCS_CONTAINER_LIST SET SEQUENCE = :SEQUENCE WHERE CONTAINER_SN=:CONTAINER_SN";
                    result = Execute(U_UpadateContainerListSeq, new
                    {
                        SEQUENCE = qty,
                        CONTAINER_SN = carton_no
                    }, tran);
                    if (result <= 0) { throw new Exception("UPDATE_CARTONNO_FAILURE"); }

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

        public async Task<int> UpdateCartonNoBySN(string carton_no, string sn, IDbTransaction tran)
        {
            int result = 0;
            ConnectionFactory.OpenConnection(_dbConnection);
            try
            {
                string updateSql = @"UPDATE SFCS_RUNCARD SET CARTON_NO=:CARTON_NO WHERE SN=:SN";
                result = await _dbConnection.ExecuteAsync(updateSql, new
                {
                    CARTON_NO = carton_no,
                    SN = sn
                }, tran);
                if (result <= 0) { throw new Exception("UPDATE_CARTONNO_FAILURE"); }

                decimal qty = QueryEx<decimal>("SELECT COUNT(1) QTY FROM SFCS_RUNCARD WHERE CARTON_NO =:CARTON_NO", new { CARTON_NO = carton_no }).FirstOrDefault();

                String U_UpadateContainerListSeq = @"UPDATE SFCS_CONTAINER_LIST SET SEQUENCE = :SEQUENCE WHERE CONTAINER_SN=:CONTAINER_SN";
                result = Execute(U_UpadateContainerListSeq, new
                {
                    SEQUENCE = qty,
                    CONTAINER_SN = carton_no
                }, tran);
                if (result <= 0) { throw new Exception("UPDATE_CARTONNO_FAILURE"); }


            }
            catch (Exception ex)
            {
                result = -1;
                throw ex;
            }
            finally
            {
                if (_dbConnection.State != System.Data.ConnectionState.Closed)
                {
                    _dbConnection.Close();
                }
            }

            return result;
        }

        /// <summary>
        /// 删除sn对应的卡通号
        /// </summary>
        /// <param name="carton_no"></param>
        /// <param name="sn"></param>
        /// <param name="tran"></param>
        /// <returns></returns>
        public async Task<int> DelCartonNoBySN(string carton_no, string sn, IDbTransaction tran)
        {
            int result = 0;
            ConnectionFactory.OpenConnection(_dbConnection);
            try
            {
                string updateSql = @"UPDATE SFCS_RUNCARD SET CARTON_NO=:CARTON_NO WHERE SN=:SN";
                result = await _dbConnection.ExecuteAsync(updateSql, new
                {
                    CARTON_NO = "",
                    SN = sn
                }, tran);
                if (result <= 0) { throw new Exception("UPDATE_CARTONNO_FAILURE"); }

                decimal qty = QueryEx<decimal>("SELECT COUNT(1) QTY FROM SFCS_RUNCARD WHERE CARTON_NO =:CARTON_NO", new { CARTON_NO = carton_no }).FirstOrDefault();

                String U_UpadateContainerListSeq = @"UPDATE SFCS_CONTAINER_LIST SET SEQUENCE = :SEQUENCE WHERE CONTAINER_SN=:CONTAINER_SN";
                result = Execute(U_UpadateContainerListSeq, new
                {
                    SEQUENCE = qty,
                    CONTAINER_SN = carton_no
                }, tran);
                if (result <= 0) { throw new Exception("UPDATE_CARTONNO_FAILURE"); }


            }
            catch (Exception ex)
            {
                result = -1;
                throw ex;
            }
            finally
            {
                if (_dbConnection.State != System.Data.ConnectionState.Closed)
                {
                    _dbConnection.Close();
                }
            }

            return result;
        }

        /// <summary>
        /// 根据卡通号（箱号）获取相关信息
        /// </summary>
        /// <param name="model">箱号</param>
        /// <returns></returns>
        public async Task<CartonInfoListModel> GetCartonInfoByCartonNo(SfcsContainerListRequestModel model)
        {
            string fieldName = "", viewName = "", sWhere = "", sQuery = "";
            CartonInfoListModel cartonInfo = new CartonInfoListModel();
            int page = 0, limit = 0;
            page = model.Page * model.Limit - model.Limit + 1;
            limit = model.Page * model.Limit;
            model.Page = page;
            model.Limit = limit;

            sQuery = "SELECT CONTAINER_SN AS CARTON_NO,QUANTITY AS DEFINEDQTY FROM SFCS_CONTAINER_LIST WHERE CONTAINER_SN = :CARTON_NO";
            var conditionSN = "";
            cartonInfo = await _dbConnection.QueryFirstOrDefaultAsync<CartonInfoListModel>(sQuery, model);
            if (!model.SN.IsNullOrEmpty())
            {
                conditionSN = " AND SR.SN=:SN";
            }

            if (cartonInfo != null)
            {
                //获取产品列表
                fieldName = "SR.SN,SR.CARTON_NO,SW.Wo_No,SW.PART_NO,P.DESCRIPTION";
                viewName = "SFCS_RUNCARD SR LEFT JOIN SFCS_WO SW ON SR.WO_ID = SW.ID LEFT JOIN IMS_PART P ON SW.PART_NO= P.CODE";
                sWhere = $"AND SR.CARTON_NO = :CARTON_NO {conditionSN} ORDER BY SN DESC";
                sQuery = string.Format("SELECT * FROM (SELECT ROWNUM R, T.* FROM ( SELECT {0} FROM {1} WHERE 1=1 {2}) T) WHERE R BETWEEN :Page AND :Limit", fieldName, viewName, sWhere);
                cartonInfo.PRODUCT = await _dbConnection.QueryAsync<ProductInfoListModel>(sQuery, model);
                //sQuery = string.Format("SELECT COUNT(1) FROM ( SELECT {0} FROM {1} WHERE 1=1 {2})", fieldName, viewName, sWhere);
                //cartonInfo.CURRENTQTY = await _dbConnection.ExecuteScalarAsync<Decimal>(sQuery, model);
                cartonInfo.CURRENTQTY = cartonInfo.PRODUCT.Count();
            }
            return cartonInfo;
        }

        /// <summary>
        /// 箱号置满包装
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public async Task<decimal> SetCatonFullByCaton(PackageFullRequestModel model)
        {
            decimal printTaskId = 0;
            ConnectionFactory.OpenConnection(_dbConnection);
            using (var tran = _dbConnection.BeginTransaction())
            {
                try
                {

                    String LineNameSql = @"SELECT OL.OPERATION_LINE_NAME FROM SFCS_CONTAINER_LIST CL LEFT JOIN SFCS_OPERATION_SITES OS ON CL.SITE_ID = OS.ID LEFT JOIN SFCS_OPERATION_LINES OL ON OS.OPERATION_LINE_ID = OL.ID WHERE CL.CONTAINER_SN = :CARTON_NO ";
                    String line_name = QueryEx<string>(LineNameSql, new { CARTON_NO = model.CARTON_NO }).FirstOrDefault();//线体名称

                    //箱号置满包装
                    String updateContainSql = @"update SFCS_CONTAINER_LIST SET FULL_FLAG = :FULL_FLAG WHERE CONTAINER_SN = :CONTAINER_SN";
                    int result = await _dbConnection.ExecuteAsync(updateContainSql, new
                    {
                        FULL_FLAG = "Y",
                        CONTAINER_SN = model.CARTON_NO
                    }, tran);
                    if (result <= 0) { throw new Exception("FULL_FAILURE"); }

                    //查询打印模板
                    String printMappSql = @"SELECT SPF.* FROM SFCS_PRINT_FILES_MAPPING SPFM, SFCS_PRINT_FILES  SPF 
                    WHERE SPFM.PRINT_FILE_ID = SPF.ID AND SPFM.ENABLED = 'Y' AND SPF.ENABLED = 'Y' AND SPF.LABEL_TYPE = 3";
                    String printMappSqlByPn = printMappSql + " AND SPFM.PART_NO = :PART_NO";
                    //SfcsPn sfcsPn = QueryEx<SfcsPn>("SELECT SP.* FROM SFCS_PN SP where SP.PART_NO =:PART_NO", new { PART_NO = part_no }).FirstOrDefault();
                    SfcsPn sfcsPn = QueryEx<SfcsPn>("SELECT * FROM SFCS_PN SP WHERE SP.PART_NO IN (SELECT SW.PART_NO FROM SFCS_RUNCARD SR LEFT JOIN SFCS_WO SW ON SR.WO_ID = SW.ID WHERE SR.CARTON_NO = :CARTON_NO)", new { CARTON_NO = model.CARTON_NO }).FirstOrDefault();

                    SfcsPrintFiles sfcsPrintFiles = null;
                    List<SfcsPrintFiles> sfcsPrintMapplist = null;
                    sfcsPrintMapplist = QueryEx<SfcsPrintFiles>(printMappSqlByPn,
                        new
                        {
                            PART_NO = sfcsPn.PART_NO
                        });

                    if (sfcsPrintMapplist == null || sfcsPrintMapplist.Count <= 0)
                    {
                        String printMappSqlByModel = printMappSql + " AND SPFM.MODEL_ID = :MODEL_ID";
                        sfcsPrintMapplist = QueryEx<SfcsPrintFiles>(printMappSqlByModel,
                        new
                        {
                            MODEL_ID = sfcsPn.MODEL_ID
                        });
                    }
                    if (sfcsPrintMapplist == null || sfcsPrintMapplist.Count <= 0)
                    {
                        String printMappSqlByFamilly = printMappSql + " AND SPFM.PRODUCT_FAMILY_ID = :PRODUCT_FAMILY_ID";
                        sfcsPrintMapplist = QueryEx<SfcsPrintFiles>(printMappSqlByFamilly,
                        new
                        {
                            PRODUCT_FAMILY_ID = sfcsPn.FAMILY_ID
                        });
                    }
                    if (sfcsPrintMapplist == null || sfcsPrintMapplist.Count <= 0)
                    {
                        String printMappSqlByCustor = printMappSql + " AND SPFM.CUSTOMER_ID = :CUSTOMER_ID";

                        sfcsPrintMapplist = QueryEx<SfcsPrintFiles>(printMappSqlByCustor,
                        new
                        {
                            CUSTOMER_ID = sfcsPn.CUSTOMER_ID
                        });
                    }
                    if (sfcsPrintMapplist == null || sfcsPrintMapplist.Count <= 0)
                    {
                        sfcsPrintMapplist = QueryEx<SfcsPrintFiles>(printMappSqlByPn,
                        new
                        {
                            PART_NO = "000000"
                        });
                    }
                    if (sfcsPrintMapplist != null && sfcsPrintMapplist.Count > 0)
                    {
                        sfcsPrintFiles = sfcsPrintMapplist.FirstOrDefault();
                    }
                    else
                    {
                        throw new Exception("PACKAGE_NOT_NULL");
                    }

                    String sql = @"SELECT SR.CARTON_NO,SP.PART_NO PN, SP.DESCRIPTION MODEL, SCL.SEQUENCE QTY,SR.CARTON_NO QR_NO,SW.WO_NO, SR.SN
                                FROM SFCS_RUNCARD SR, SFCS_WO SW, SFCS_PN SP ,SFCS_CONTAINER_LIST SCL
                                WHERE SR.WO_ID = SW.ID AND SW.PART_NO = SP.PART_NO
                                AND SCL.CONTAINER_SN = :CARTON_NO
                                AND SR.CARTON_NO = SCL.CONTAINER_SN";
                    var cartonNoResponModelList = QueryEx<CartonResoponeModel>(sql, new
                    {
                        CARTON_NO = model.CARTON_NO
                    });
                    if (cartonNoResponModelList == null | cartonNoResponModelList.Count() <= 0)
                    {
                        throw new Exception("CARTON_NO_NOT_INFO");
                    }
                    StringBuilder snheader = new StringBuilder();
                    StringBuilder snDetail = new StringBuilder();
                    for (int i = 0; i < cartonNoResponModelList.Count; i++)
                    {
                        string index = (i + 1).ToString();
                        snheader.Append(String.Format(",SN{0}", index));
                        snDetail.Append(String.Format(",{0}", cartonNoResponModelList[i].SN));
                    }
                    StringBuilder stringBuilder = new StringBuilder();
                    stringBuilder.AppendLine(String.Format("BOX_NO,PN,MODEL,LINE_NAME,PRODUCT_TIME,QTY,QR_NO,WO_NO{0}", snheader.ToString()));
                    stringBuilder.AppendLine(String.Format("{0},{1},{2},{3},{4},{5},{6},{7}{8}",
                        model.CARTON_NO, cartonNoResponModelList[0].PN, cartonNoResponModelList[0].MODEL,
                        line_name,
                        DateTime.Now,
                        cartonNoResponModelList.Count,
                        model.CARTON_NO,
                        cartonNoResponModelList[0].WO_NO,
                        snDetail.ToString()));

                    printTaskId = QueryEx<decimal>("SELECT SFCS_PRINT_TASKS_SEQ.NEXTVAL MY_SEQ FROM DUAL").FirstOrDefault();
                    DynamicParameters p = new DynamicParameters();
                    p.Add("ID", printTaskId, System.Data.DbType.Decimal);
                    p.Add("PRINT_FILE_ID", sfcsPrintFiles.ID, System.Data.DbType.Decimal);
                    p.Add("OPERATOR", model.USER_NAME, System.Data.DbType.String);
                    p.Add("PRINT_DATA", stringBuilder.ToString(), System.Data.DbType.String);
                    String insertPrintTaskSql = @"INSERT INTO SFCS_PRINT_TASKS(ID,PRINT_FILE_ID,OPERATOR,CREATE_TIME,PRINT_STATUS,PRINT_DATA)VALUES(
		:ID,:PRINT_FILE_ID,:OPERATOR,sysdate,0,:PRINT_DATA)";
                    result = Execute(insertPrintTaskSql, p, tran, commandType: CommandType.Text);
                    if (result <= 0) { throw new Exception("PRINTTASK_FAILURE"); }

                    #region 添加IMS_REEL表

                    //string BarcodeFormatter = "M{0:yyMMdd}{1:000000}";
                    //decimal reel_id = (await QueryAsyncEx<decimal>("SELECT SEQ_REEL.NEXTVAL FROM DUAL")).FirstOrDefault();
                    string reel_code = model.CARTON_NO;//string.Format(BarcodeFormatter, DateTime.Now, reel_id);//物料条码
                                                       //rpModel.REEL_CODE.Add(reel_code);

                    //SELECT IMS.IMS_REEL_SEQ.NEXTVAL FROM DUAL
                    //SELECT IMS_REEL_SEQ.NEXTVAL@WMS FROM DUAL
                    decimal reelId = (await QueryAsyncEx<decimal>("SELECT IMS_REEL_SEQ.NEXTVAL@WMS FROM DUAL")).FirstOrDefault();

                    //IMS_REEL@WMS IMS.IMS_REEL
                    string selectVendorIdSql = @"SELECT ID FROM IMS_VENDOR@WMS WHERE CODE = 'Pilot'";
                    string selectPartIdSql = @"SELECT * FROM IMS_PART@WMS WHERE CODE=:CODE";
                    //string selectCartonNoSql = @"SELECT COUNT(*) FROM SFCS_RUNCARD  WHERE CARTON_NO=:CARTON_NO ";
                    string selectImsReelSql = @"SELECT COUNT(*) FROM IMS_REEL@WMS WHERE CODE=:CODE ";
                    string insertImsReelSql = @"INSERT INTO IMS_REEL@WMS(ID,CODE,VENDOR_ID,PART_ID,MAKER_PART_ID,DATE_CODE,LOT_CODE,MSD_LEVEL,ESD_FLAG,CASE_QTY,IQC_FLAG,ORIGINAL_QUANTITY) VALUES(:ID,:CODE,:VENDOR_ID,:PART_ID,:MAKER_PART_ID,:DATE_CODE,:LOT_CODE,:MSD_LEVEL,:ESD_FLAG,:CASE_QTY,:IQC_FLAG,:ORIGINAL_QUANTITY)";

                    var vendorID = await QueryAsyncEx<decimal>(selectVendorIdSql);
                    var partID = (await QueryAsyncEx<ImsPart>(selectPartIdSql, new { CODE = cartonNoResponModelList[0].PN })).FirstOrDefault()?.ID ?? 0;
                    var count = await _dbConnection.ExecuteScalarAsync<int>(selectImsReelSql, new { CODE = reel_code });
                    if (count <= 0)
                    {
                        var effectNum = await ExecuteAsync(insertImsReelSql, new
                        {
                            ID = reelId,
                            CODE = reel_code,
                            VENDOR_ID = vendorID,
                            PART_ID = partID,
                            MAKER_PART_ID = -1,
                            DATE_CODE = DateTime.Now.Date,
                            LOT_CODE = "",
                            MSD_LEVEL = "1",
                            ESD_FLAG = "",
                            CASE_QTY = 1,
                            IQC_FLAG = "Y",
                            ORIGINAL_QUANTITY = cartonNoResponModelList.Count
                        }, tran);

                        if (effectNum <= 0)
                            throw new Exception("更新条码数据异常，请稍后再尝试。");
                    }



                    #endregion

                    tran.Commit();
                }
                catch (Exception ex)
                {
                    tran.Rollback();//回滚事务
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
            return printTaskId;
        }

        /// <summary>
        /// 返工业务
        /// </summary>
        /// <param name="_sfcsReworkRepository"></param>
        /// <param name="oldSN"></param>
        /// <param name="tran"></param>
        /// <returns></returns>
        public async Task<bool> ReworkProcessBySN(ISfcsReworkRepository _sfcsReworkRepository, string oldSN, string userName)
        {
            bool result = false;
            try
            {
                var model = new SfcsReworkRequestModel();
                model.RETYPE = 0;
                model.SN = oldSN;
                //查对应的制程
                var reworkRouteList = await _sfcsReworkRepository.GetReworkDataBySN(model);
                var isExist = false;
                decimal index = 0;
                //查有没有包装工序
                if (reworkRouteList.CURRENTOPERATIONLIST != null && reworkRouteList.CURRENTOPERATIONLIST.Count > 0)
                {
                    for (int i = 0; i < reworkRouteList.CURRENTOPERATIONLIST.Count; i++)
                    {
                        if (reworkRouteList.CURRENTOPERATIONLIST[i].Values.Contains(200))
                        {
                            isExist = true;
                            index = i;
                        }
                    }
                }
                if (!isExist)
                {
                    throw new Exception("ROUTE_NOT_ERROR");
                }
                //保存返工的对象
                SfcsReworkModel reworkMolde = new SfcsReworkModel();
                reworkMolde.SNLIST = reworkRouteList.SNLIST;
                reworkMolde.SaveRecords = new List<SfcsReworkAddOrModifyModel>();
                SfcsReworkAddOrModifyModel sfcsReworkAddOrModifyModel = new SfcsReworkAddOrModifyModel();
                sfcsReworkAddOrModifyModel.CHOOSEINDEX = index;
                sfcsReworkAddOrModifyModel.CHOOSEINDEXVALUE = 200;
                sfcsReworkAddOrModifyModel.CLASSIFICATION = 1;
                sfcsReworkAddOrModifyModel.ISDELRESOURE = true;
                sfcsReworkAddOrModifyModel.ISDELUID = true;
                sfcsReworkAddOrModifyModel.ORDERNOLIST = reworkRouteList.ORDERNOLIST;
                sfcsReworkAddOrModifyModel.ORIGINALORDERNOLIST = new List<decimal>();
                sfcsReworkAddOrModifyModel.ORIGINALROUTEID = reworkRouteList.ORIGINALROUTEID;
                sfcsReworkAddOrModifyModel.PLANT_CODE = reworkRouteList.PLANT_CODE;
                sfcsReworkAddOrModifyModel.REPAIRER = userName;
                sfcsReworkAddOrModifyModel.RETYPE = 0;
                sfcsReworkAddOrModifyModel.ROUTE_ID = reworkRouteList.ROUTE_ID;
                sfcsReworkAddOrModifyModel.SN = reworkRouteList.SN;
                sfcsReworkAddOrModifyModel.WORKORDERID = reworkRouteList.WORKORDERID;
                reworkMolde.SaveRecords.Add(sfcsReworkAddOrModifyModel);
                var saveResult = await _sfcsReworkRepository.SaveDataByTrans(reworkMolde);
                result = saveResult > 0 ? true : false;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return result;

        }
    }
}