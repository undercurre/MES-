/**
*┌──────────────────────────────────────────────────────────────┐
*│　描    述：接口实现                                                    
*│　作    者：嘉志科技                                            
*│　版    本：2.0    模板代码自动生成                                                
*│　创建时间：2020-03-04 15:39:22                             
*└──────────────────────────────────────────────────────────────┘
*┌──────────────────────────────────────────────────────────────┐
*│　命名空间： JZ.IMS.Repository.Oracle                                  
*│　类    名： ImsReelRepository                                      
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
using System.Linq;
using System.Text;

namespace JZ.IMS.Repository.Oracle
{
    public class ImsReelRepository : BaseRepository<ImsReel, Decimal>, IImsReelRepository
    {
        public ImsReelRepository(IOptionsSnapshot<DbOption> options)
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
            string sql = "SELECT ENABLED FROM IMS_REEL WHERE ID=:ID";
            var result = await _dbConnection.QueryFirstOrDefaultAsync<string>(sql, new
            {
                ID = id,
            });

            return result == "Y" ? true : false;
        }
        /// <summary>
        /// 通过条码CODE获取产品条码
        /// </summary>
        /// <param name="reelCode"></param>
        /// <returns></returns>
        public async Task<ReelInfoViewModel> GetReelInfoViewModel(String reelCode)
        {
            string sql = @"SELECT* FROM IMS_REEL_INFO_VIEW WHERE CODE = :CODE";
            var result = await _dbConnection.QueryFirstOrDefaultAsync<ReelInfoViewModel>(sql, new
            {
                CODE = reelCode
            });
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
            string sql = "UPDATE IMS_REEL set ENABLED=:ENABLED WHERE ID=:Id";
            return await _dbConnection.ExecuteAsync(sql, new
            {
                ENABLED = status ? 'Y' : 'N',
                Id = id,
            });
        }
        /// <summary>
        /// 获取料号ID
        /// </summary>
        /// <param name="partNo"></param>
        /// <returns></returns>
        public async Task<decimal> GetPartId(String partNo)
        {
            String sql = "select ID from IMS_PART WHERE CODE = :CODE";
            decimal result = await _dbConnection.QueryFirstOrDefaultAsync<decimal>(sql, new
            {
                CODE = partNo
            });
            return result;
        }
        /// <summary>
        /// 获取供应商ID
        /// </summary>
        /// <param name="VendorCode"></param>
        /// <returns></returns>
        public async Task<decimal> GetVendorId(String VendorCode)
        {
            String sql = "select ID from IMS_VENDOR WHERE CODE = :CODE";
            decimal result = await _dbConnection.QueryFirstOrDefaultAsync<decimal>(sql, new
            {
                CODE = VendorCode
            });
            return result;
        }

        /// <summary>
        /// 在WMS中条码同步
        /// </summary>
        /// <param name="reelInfoViewModel"></param>
        /// <returns></returns>
        public async Task<bool> KeepVendorBarcodeInWMS(ReelInfoViewModel reelInfoViewModel)
        {
            ReelInfoViewModel existReelInfo = await GetReelInfoViewModel(reelInfoViewModel.CODE);
            if (existReelInfo != null)
            {
                return true;
            }
            else
            {
                ImsReel imsReel = new ImsReel();
                string seqSql = "SELECT IMS.IMS_REEL_SEQ.NEXTVAL MY_SEQ FROM DUAL";
                imsReel.ID = await _dbConnection.ExecuteScalarAsync<decimal>(seqSql);
                imsReel.VENDOR_ID = 1;
                imsReel.CODE = reelInfoViewModel.CODE;
                imsReel.BOX_ID = -1;
                imsReel.PART_ID = await GetPartId(reelInfoViewModel.PART_NO);
                imsReel.VENDOR_ID = await GetVendorId(reelInfoViewModel.VENDOR_CODE);
                imsReel.DATE_CODE = reelInfoViewModel.DATE_CODE;
                imsReel.LOT_CODE = reelInfoViewModel.LOT_CODE;
                if (reelInfoViewModel.CASE_QTY <= 0 || reelInfoViewModel.CASE_QTY == null)
                {
                    imsReel.CASE_QTY = reelInfoViewModel.ORIGINAL_QUANTITY;
                }
                else
                {
                    imsReel.CASE_QTY = reelInfoViewModel.CASE_QTY;
                }
                imsReel.ORIGINAL_QUANTITY = reelInfoViewModel.ORIGINAL_QUANTITY;
                imsReel.CUSTOMER_PN = reelInfoViewModel.CUSTOMER_PN;
                imsReel.REFERENCE = reelInfoViewModel.REFERENCE;
                imsReel.MSD_LEVEL = "1";
                imsReel.ESD_FLAG = "N";
                ImsPart imsPart = await _dbConnection.QueryFirstAsync<ImsPart>("select * from IMS_PART WHERE CODE = :CODE", new
                {
                    CODE = reelInfoViewModel.PART_NO
                });
                imsReel.DESCRIPTION = imsPart.DESCRIPTION;
                imsReel.TO_LOCATOR_ID = -1;
                imsReel.ORIGINAL_SIC_ID = -1;
                imsReel.MAKER_PART_CODE = reelInfoViewModel.MAKER_PART_NO;
                imsReel.ATTRIBUTE4 = reelInfoViewModel.VENDOR_CODE ?? "";//瓶号
                String insertImsReelSql = @"INSERT INTO IMS.IMS_REEL(ID,VENDOR_ID,CODE,BOX_ID,PART_ID,DATE_CODE,LOT_CODE,CASE_QTY,ORIGINAL_QUANTITY,CUSTOMER_PN,REFERENCE,MSD_LEVEL,ESD_FLAG,ATTRIBUTE4) 
VALUES(:ID,:VENDOR_ID,:CODE,:BOX_ID,:PART_ID,:DATE_CODE,:LOT_CODE,:CASE_QTY,:ORIGINAL_QUANTITY,:CUSTOMER_PN,:REFERENCE,:MSD_LEVEL,:ESD_FLAG,:ATTRIBUTE4)";

                DateTime dateCode = DateTime.ParseExact(imsReel.DATE_CODE.ToString(), "yyyyMMdd", System.Globalization.CultureInfo.CurrentCulture);
                decimal result = await _dbConnection.ExecuteAsync(insertImsReelSql, new
                {
                    ID = imsReel.ID,
                    VENDOR_ID = imsReel.VENDOR_ID,
                    CODE = imsReel.CODE,
                    BOX_ID = imsReel.BOX_ID,
                    PART_ID = imsReel.PART_ID,
                    DATE_CODE = dateCode,
                    LOT_CODE = imsReel.LOT_CODE,
                    CASE_QTY = imsReel.CASE_QTY,
                    ORIGINAL_QUANTITY = imsReel.ORIGINAL_QUANTITY,
                    CUSTOMER_PN = imsReel.CUSTOMER_PN,
                    REFERENCE = imsReel.REFERENCE,
                    MSD_LEVEL = imsReel.MSD_LEVEL,
                    ESD_FLAG = imsReel.ESD_FLAG,
                    ATTRIBUTE4 = imsReel.ATTRIBUTE4
                });
                if (result > 0)
                {
                    return true;
                }
                else
                {
                    return false;
                }

            }
        }

        /// <summary>
        ///同步物料条码信息
        /// </summary>
        /// <param name="reelInfoViewModel"></param>
        /// <returns></returns>
        public async Task<bool> KeepVendorBarcode(ReelInfoViewModel reelInfoViewModel)
        {
            ReelInfoViewModel existReelInfo = await GetReelInfoViewModel(reelInfoViewModel.CODE);
            if (existReelInfo != null)
            {
                return true;
            }
            else
            {
                int result = 0;
                ConnectionFactory.OpenConnection(_dbConnection);
                using (var tran = _dbConnection.BeginTransaction())
                {
                    try
                    {
                        ImsReel imsReel = new ImsReel();
                        //string seqSql = "SELECT IMS_REEL_SEQ.NEXTVAL MY_SEQ FROM DUAL";
                        string seqSql = "SELECT IMS_REEL_SEQ.NEXTVAL@WMS MY_SEQ FROM DUAL";
                        imsReel.ID = await _dbConnection.ExecuteScalarAsync<decimal>(seqSql);
                        imsReel.VENDOR_ID = 1;
                        imsReel.CODE = reelInfoViewModel.CODE;
                        imsReel.BOX_ID = -1;
                        imsReel.PART_ID = await GetPartId(reelInfoViewModel.PART_NO);
                        imsReel.VENDOR_ID = await GetVendorId(reelInfoViewModel.VENDOR_CODE);
                        imsReel.DATE_CODE = reelInfoViewModel.DATE_CODE;
                        imsReel.LOT_CODE = reelInfoViewModel.LOT_CODE;
                        if (reelInfoViewModel.CASE_QTY <= 0 || reelInfoViewModel.CASE_QTY == null)
                        {
                            imsReel.CASE_QTY = reelInfoViewModel.ORIGINAL_QUANTITY;
                        }
                        else
                        {
                            imsReel.CASE_QTY = reelInfoViewModel.CASE_QTY;
                        }
                        imsReel.ORIGINAL_QUANTITY = reelInfoViewModel.ORIGINAL_QUANTITY;
                        imsReel.CUSTOMER_PN = reelInfoViewModel.CUSTOMER_PN;
                        imsReel.REFERENCE = reelInfoViewModel.REFERENCE;
                        imsReel.MSD_LEVEL = "1";
                        imsReel.ESD_FLAG = "N";
                        ImsPart imsPart = await _dbConnection.QueryFirstAsync<ImsPart>("select * from IMS_PART WHERE CODE = :CODE", new
                        {
                            CODE = reelInfoViewModel.PART_NO
                        });
                        imsReel.DESCRIPTION = imsPart.DESCRIPTION;
                        imsReel.TO_LOCATOR_ID = -1;
                        imsReel.ORIGINAL_SIC_ID = -1;
                        imsReel.MAKER_PART_CODE = reelInfoViewModel.MAKER_PART_NO;
                        DateTime date_code = Convert.ToDateTime(imsReel.DATE_CODE);
                        String insertSql = @"INSERT INTO IMS_REEL@WMS (ID, VENDOR_ID, CODE, BOX_ID, PART_ID, DATE_CODE, LOT_CODE, CASE_QTY, ORIGINAL_QUANTITY, CUSTOMER_PN, REFERENCE, MSD_LEVEL, ESD_FLAG, DESCRIPTION, TO_LOCATOR_ID, ORIGINAL_SIC_ID, MAKER_PART_CODE) VALUES (:ID, :VENDOR_ID, :CODE, :BOX_ID, :PART_ID, :DATE_CODE, :LOT_CODE, :CASE_QTY, :ORIGINAL_QUANTITY, :CUSTOMER_PN, :REFERENCE, :MSD_LEVEL, :ESD_FLAG, :DESCRIPTION, :TO_LOCATOR_ID, :ORIGINAL_SIC_ID, :MAKER_PART_CODE) ";
                        result = await _dbConnection.ExecuteAsync(insertSql, new
                        {
                            imsReel.ID,
                            imsReel.VENDOR_ID,
                            imsReel.CODE,
                            imsReel.BOX_ID,
                            imsReel.PART_ID,
                            DATE_CODE = date_code,
                            imsReel.LOT_CODE,
                            imsReel.CASE_QTY,
                            imsReel.ORIGINAL_QUANTITY,
                            imsReel.CUSTOMER_PN,
                            imsReel.REFERENCE,
                            imsReel.MSD_LEVEL,
                            imsReel.ESD_FLAG,
                            imsReel.DESCRIPTION,
                            imsReel.TO_LOCATOR_ID,
                            imsReel.ORIGINAL_SIC_ID,
                            imsReel.MAKER_PART_CODE
                        }, tran);

                        tran.Commit();
                    }
                    catch (Exception)
                    {
                        result = 0;
                        tran.Rollback();
                    }
                    finally
                    {
                        if (_dbConnection.State != System.Data.ConnectionState.Closed)
                        {
                            _dbConnection.Close();
                        }
                    }
                }
                //decimal result = await _dbConnection.InsertAsync<ImsReel>(imsReel);
                if (result > 0)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
        }

        // <summary>
        /// 获取表的序列
        /// </summary>
        /// <returns></returns>
		public async Task<decimal> GetSEQID()
        {
            string sql = "SELECT IMS_REEL_SEQ.NEXTVAL MY_SEQ FROM DUAL";
            var result = await _dbConnection.ExecuteScalarAsync(sql);
            return (decimal)result;
        }

        /// <summary>
        /// 获取供应商列表
        /// </summary>
        /// <param name="model">Key : DESCRIPTION</param>
        /// <returns></returns>
        public async Task<IEnumerable<VendorListModel>> GetVendorList(ImsPartRequestModel model)
        {
            int page = 0, limit = 0;
            page = model.Page * model.Limit - model.Limit + 1;
            limit = model.Page * model.Limit;
            model.Page = page;
            model.Limit = limit;

            string sWhere = "";
            if (!string.IsNullOrEmpty(model.CODE))
            {
                sWhere += " AND INSTR(CODE,:CODE)> 0 ";
            }
            if (!string.IsNullOrEmpty(model.DESCRIPTION))
            {
                sWhere += " OR INSTR(DESCRIPTION,:DESCRIPTION)>0 ";
            }

            string sQuery = string.Format("SELECT * FROM (SELECT ROWNUM R, T.* FROM ( SELECT ID, CODE, DESCRIPTION , CODE || '-' || DESCRIPTION AS VENDOR_INFO FROM IMS_VENDOR WHERE ENABLED ='Y' {0} ORDER BY ID DESC) T) WHERE R BETWEEN :Page AND :Limit", sWhere);

            return await _dbConnection.QueryAsync<VendorListModel>(sQuery, model);
        }

        /// <summary>
        /// 获取供应商列表条数
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public async Task<int> GetVendorListCount(ImsPartRequestModel model)
        {
            string sQuery = "SELECT COUNT(1) FROM IMS_VENDOR WHERE ENABLED ='Y'";
            if (!string.IsNullOrEmpty(model.CODE))
            {
                sQuery += " AND INSTR(CODE,:CODE)> 0 ";
            }
            if (!string.IsNullOrEmpty(model.DESCRIPTION))
            {
                sQuery += " OR INSTR(DESCRIPTION,:DESCRIPTION)>0 ";
            }
            return await _dbConnection.ExecuteScalarAsync<int>(sQuery, model);
        }

        /// <summary>
        /// 获取物料信息列表
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public async Task<IEnumerable<ImsPartListModel>> GetImsPartList(ImsPartRequestModel model)
        {
            int page = 0, limit = 0;
            page = model.Page * model.Limit - model.Limit + 1;
            limit = model.Page * model.Limit;
            model.Page = page;
            model.Limit = limit;

            string sWhere = "";
            if (!string.IsNullOrEmpty(model.CODE))
            {
                sWhere += " AND INSTR(CODE,:CODE)> 0 ";
            }
            if (!string.IsNullOrEmpty(model.DESCRIPTION))
            {
                sWhere += " OR INSTR(DESCRIPTION,:DESCRIPTION)> 0 ";
            }
            if (!string.IsNullOrEmpty(sWhere))
            {
                sWhere = "WHERE 1=1 " + sWhere;
            }

            string sQuery = string.Format("SELECT * FROM (SELECT ROWNUM R, T.* FROM ( SELECT ID, CODE, NAME, DESCRIPTION,COMPANY_ID FROM IMS_PART {0} ORDER BY ID DESC) T) WHERE R BETWEEN :Page AND :Limit", sWhere);

            return await _dbConnection.QueryAsync<ImsPartListModel>(sQuery, model);
        }

        /// <summary>
        /// 获取物料信息列表条数
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public async Task<int> GetImsPartListCount(ImsPartRequestModel model)
        {
            string sQuery = "SELECT COUNT(1) FROM IMS_PART WHERE 1=1 ";
            if (!string.IsNullOrEmpty(model.CODE))
            {
                sQuery += " AND INSTR(CODE,:CODE)> 0 ";
            }
            if (!string.IsNullOrEmpty(model.DESCRIPTION))
            {
                sQuery += " OR INSTR(DESCRIPTION,:DESCRIPTION)> 0 ";
            }
            return await _dbConnection.ExecuteScalarAsync<int>(sQuery, model);
        }

        public async Task<ReelPrintListModel> SaveReelPrintInfo(ReelPrintRequestModel model)
        {
            ReelPrintListModel rpModel = new ReelPrintListModel();
            ConnectionFactory.OpenConnection(_dbConnection);
            using (var tran = _dbConnection.BeginTransaction())
            {
                try
                {
                    int result = 0;
                    var partModel = (await _dbConnection.QueryAsync<ImsPart>("SELECT NAME,DESCRIPTION  FROM IMS_PART WHERE ID =:ID", new { ID = model.PART_ID })).FirstOrDefault();

                    //添加物料条码数据
                    for (int i = 0; i < model.PRINT_QTY; i++)
                    {
                        string BarcodeFormatter = "M{0:yyMMdd}{1:000000}";
                        decimal reel_id = QueryEx<decimal>("SELECT SEQ_REEL.NEXTVAL FROM DUAL").FirstOrDefault();
                        string reel_code = string.Format(BarcodeFormatter, DateTime.Now, reel_id);//物料条码
                        rpModel.REEL_CODE.Add(reel_code);

                        //SELECT IMS.IMS_REEL_SEQ.NEXTVAL FROM DUAL
                        //SELECT IMS_REEL_SEQ.NEXTVAL@WMS FROM DUAL
                        decimal reelId = QueryEx<decimal>("SELECT IMS_REEL_SEQ.NEXTVAL@WMS FROM DUAL").FirstOrDefault();

                        //IMS_REEL@WMS IMS.IMS_REEL
                        string insertSql = @"INSERT INTO IMS_REEL@WMS(ID,CODE,VENDOR_ID,PART_ID,MAKER_PART_ID,DATE_CODE,LOT_CODE,MSD_LEVEL,ESD_FLAG,CASE_QTY,IQC_FLAG,ORIGINAL_QUANTITY) VALUES(:ID,:CODE,:VENDOR_ID,:PART_ID,:MAKER_PART_ID,:DATE_CODE,:LOT_CODE,:MSD_LEVEL,:ESD_FLAG,:CASE_QTY,:IQC_FLAG,:ORIGINAL_QUANTITY)";
                        result += await _dbConnection.ExecuteAsync(insertSql, new
                        {
                            ID = reelId,
                            CODE = reel_code,
                            VENDOR_ID = model.VENDOR_ID,
                            PART_ID = model.PART_ID,
                            MAKER_PART_ID = -1,
                            DATE_CODE = DateTime.ParseExact(model.DATE_CODE, "yyyyMMdd", System.Globalization.CultureInfo.CurrentCulture),
                            LOT_CODE = model.LOT_CODE,
                            MSD_LEVEL = "1",
                            ESD_FLAG = "",
                            CASE_QTY = 1,
                            IQC_FLAG = "Y",
                            ORIGINAL_QUANTITY = model.REEL_QTY
                        }, tran);

                    }

                    //添加打印数据
                    if (result == rpModel.REEL_CODE.Count)
                    {
                        StringBuilder print_data = new StringBuilder();
                        //SFCS_PRINT_TASKS 表中 PRINT_DATA打印数据（料号、数量、供应商、生产批次，生产日期，物料条码）
                        print_data.AppendLine("PART_CODE,QTY,VENDOR_NAME,LOT_CODE,DATE_CODE,REEL_CODE,PART_NAME,MODEL,QR_NO");
                        foreach (string reel_code in rpModel.REEL_CODE)
                        {
                            string qr_no = String.Format("S{0}|P{1}|Q{2}|9D{3}|1T{4}", reel_code,model.PART_CODE, model.REEL_QTY, model.DATE_CODE, model.LOT_CODE );
                            print_data.AppendLine(String.Format("{0},{1},{2},{3},{4},{5},{6},{7},{8}", model.PART_CODE, model.REEL_QTY, model.VENDOR_NAME, model.LOT_CODE, model.DATE_CODE, reel_code, partModel?.NAME??"",partModel?.DESCRIPTION??"", qr_no));
                        }
                        string insertPrintSql = @"INSERT INTO SFCS_PRINT_TASKS(ID,PRINT_FILE_ID,OPERATOR,CREATE_TIME,PRINT_STATUS,PRINT_DATA)VALUES(:ID,:PRINT_FILE_ID,:OPERATOR,sysdate,0,:PRINT_DATA)";
                        int printTaskId = QueryEx<int>("SELECT SFCS_PRINT_TASKS_SEQ.NEXTVAL MY_SEQ FROM DUAL").FirstOrDefault();
                        int PrintFileId =await _dbConnection.ExecuteScalarAsync<int>("SELECT ID FROM SFCS_PRINT_FILES WHERE LABEL_TYPE=5 AND ENABLED = 'Y'");
                        result = await _dbConnection.ExecuteAsync(insertPrintSql, new
                        {
                            ID = printTaskId,
                            PRINT_FILE_ID = PrintFileId,
                            OPERATOR = model.USER_NAME,
                            PRINT_DATA =print_data.ToString()
                        }, tran);
                        if (result > 0)
                        {
                            rpModel.PRINT_ID = printTaskId;
                        }
                        else
                        {
                            throw new Exception("SAVE_ERROR");
                        }
                    }
                    else
                    {
                        throw new Exception("SAVE_ERROR");
                    }

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
            return rpModel;
        }
    }
}