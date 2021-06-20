/*
*┌──────────────────────────────────────────────────────────────┐
*│　描    述：产前确认配置表 控制器                                                    
*│　作    者：嘉志科技                                              
*│　版    本：3.0   模板代码自动生成                                              
*│　创建时间：2020-04-24 17:23:47                           
*└──────────────────────────────────────────────────────────────┘
*┌──────────────────────────────────────────────────────────────┐
*│　命名空间： JZ.IMS.WebApi.Controllers                                   
*│　接口名称： IMesProductionPreConfController                                      
*└──────────────────────────────────────────────────────────────┘
*/

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using JZ.IMS.Core.Helper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using JZ.IMS.ViewModels;
using FluentValidation.Results;
using JZ.IMS.IRepository;
using JZ.IMS.WebApi.Public;
using System.Reflection;
using AutoMapper;
using JZ.IMS.Core.Extensions;
using JZ.IMS.Models;
using Microsoft.AspNetCore.Http;
using JZ.IMS.WebApi.Validation;
using Microsoft.Extensions.Localization;
using JZ.IMS.ViewModels.ProductBasicSet;
using OfficeOpenXml.FormulaParsing.Excel.Functions.Math;
using Microsoft.Extensions.Configuration;
using System.IO;
using JZ.IMS.WebApi.Controllers.BomVsPlacement;
using JZ.IMS.WebApi.Common;
using JZ.IMS.ViewModels.BomVsPlacement;

namespace JZ.IMS.WebApi.Controllers
{
    /// <summary>
    /// 产线备料 控制器
    /// </summary>
	[Route("api/[controller]/[action]")]
    [ApiController]
    public class MesProductionLinePreparationController : BaseController
    {
        private readonly IMesProductionLinePreparationRepository _repository;
        private readonly IMapper _mapper;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IStringLocalizer<MesProductionLinePreparationController> _localizer;
        private readonly IBomVsPlacementRepository _bomVsPlacementRepository;
        private BomVsPlacementService _service;
        private readonly IStringLocalizer<BomVsPlacementController> _bomLocalizer;

        public MesProductionLinePreparationController(IMesProductionLinePreparationRepository repository, IMapper mapper, IHttpContextAccessor httpContextAccessor,
            IStringLocalizer<MesProductionLinePreparationController> localizer, IStringLocalizer<BomVsPlacementController> bomlocalizer, IBomVsPlacementRepository bomVsPlacementRepository)
        {
            _repository = repository;
            _mapper = mapper;
            _httpContextAccessor = httpContextAccessor;
            _localizer = localizer;
            _bomVsPlacementRepository = bomVsPlacementRepository;
            _bomLocalizer = bomlocalizer;

        }

        public class IndexVM
        {

        }

        /// <summary>
        /// 首页视图
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Authorize("Permission")]
        public async Task<ApiBaseReturn<IndexVM>> Index()
        {
            ApiBaseReturn<IndexVM> returnVM = new ApiBaseReturn<IndexVM>();
            if (!ErrorInfo.Status)
            {
                try
                {
                    #region 设置返回值

                    if (!ErrorInfo.Status)
                    {
                        returnVM.Result = new IndexVM
                        {
                            //ContentTypeList = await _repository.GetListByTableEX<dynamic>("LOOKUP_CODE,CHINESE", "SFCS_PARAMETERS", "AND LOOKUP_TYPE = 'PRODUCTION_PRE_TYPE'"),
                            //ClassTypeList = await _repository.GetListByTableEX<dynamic>("LOOKUP_CODE,CHINESE", "SFCS_PARAMETERS", "AND LOOKUP_TYPE = 'SBU_CODE'"),
                        };
                    }

                    #endregion
                }
                catch (Exception ex)
                {
                    ErrorInfo.Set(ex.Message, MethodBase.GetCurrentMethod(), EnumErrorType.Error);
                }
            }

            #region 如果出现错误，则写错误日志并返回错误内容

            WriteLog(ref returnVM);

            #endregion

            return returnVM;
        }

        /// <summary>
        /// 查询开工工单(WO_NO工单 BATCH_NO批次号)
        /// 查询使用工单号(WO_NO)
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Authorize]
        public async Task<ApiBaseReturn<List<SfcsProductionListModel>>> GetWONO([FromQuery] SfcsProductionRequestModel model)
        {
            ApiBaseReturn<List<SfcsProductionListModel>> returnVM = new ApiBaseReturn<List<SfcsProductionListModel>>();
            try
            {
                int count = 0;
                string conditions = " WHERE 1=1 AND FINISHED IN('O', 'N') ";
                if (!model.WO_NO.IsNullOrWhiteSpace())
                {
                    conditions += $" AND WO_NO=:WO_NO ";
                }
                if (!model.Key.IsNullOrEmpty())
                {
                    conditions += $" AND LINE_ID=:Key ";
                }
                var list = (await _repository.GetListPagedEx<SfcsProduction>(model.Page, model.Limit, conditions, " START_TIME desc", model)).ToList();
                var productList = _mapper.Map<List<SfcsProductionListModel>>(list);
                count = await _repository.RecordCountAsyncEx<SfcsProduction>(conditions, model);
                returnVM.Result = productList;
                returnVM.TotalCount = count;

            }
            catch (Exception ex)
            {

                ErrorInfo.Set(ex.Message, MethodBase.GetCurrentMethod(), EnumErrorType.Error);
                //return ex.ToModel("apiSMTController.ProducLineEnd", true);
            }

            return returnVM;
        }

        /// <summary>
        /// 产线开工
        /// </summary>
        /// <param name="plitem">产线开工参数模型</param>
        /// <returns></returns>
        [HttpPost]
        [Authorize]
        public async Task<ApiBaseReturn<bool>> ProducLineBegin([FromBody] ProducLineVM plitem)
        {
            ApiBaseReturn<bool> returnVM = new ApiBaseReturn<bool>();
            returnVM.Result = false;
            await _repository.StartTransaction();
            //判断工单是否已经上线
            //修改，新增上线记录
            try
            {
                //状态字母"O"：上线未备完料状态，状态"N":上完线备完料正常生产，状态"Y":生产结束

                var currentProduct = await _repository.GetListByTableEX<SfcsProduction>("*", "SFCS_PRODUCTION", " AND LINE_ID = :LINE_ID AND WO_NO=:WO_NO AND FINISHED IN('O', 'N')", new
                {
                    LINE_ID = plitem.lineId,
                    WO_NO = plitem.woNo
                });

                String oldBatchNo = String.Empty;
                if (currentProduct != null && currentProduct.Count > 0)
                {
                    //如果当前的线别有生产数据
                    //先修改当前线别生产的数据
                    //if (currentProduct.Where(f => f.WO_NO == plitem.woNo).Count() > 0)
                    //{
                    //已经在生产的工单无需上线
                    returnVM.Result = true;
                    //"当前产线正在开工，请先产线下线后再开工!"
                    throw new Exception(_localizer["PLEASE_PRODUCTION_OFFLINE"]);

                    //}

                    //oldBatchNo = currentProduct.FirstOrDefault().BATCH_NO;
                    //String U_UpdateProductLine = @"update SFCS_PRODUCTION SET END_TIME = SYSDATE, END_BY = :END_BY ,FINISHED = 'Y' WHERE LINE_ID = :LINE_ID";
                    //DBA.FromDb("MES").ExecuteScalarByKeyValuePairs(U_UpdateProductLine,
                    //	new KeyValuePair<string, object>("END_BY", plitem.user),
                    //	new KeyValuePair<string, object>("LINE_ID", plitem.lineId));
                    // 关闭当前上线工单的手插件物料状态监听表
                    //MES_HI_MATERIAL_LISTEN
                    //String U_UpdateMaterialListen = @"update MES_HI_MATERIAL_LISTEN SET STATUS =1 where BATCH_NO = :BATCH_NO";
                    //DBA.FromDb("MES").ExecuteScalarByKeyValuePairs(U_UpdateMaterialListen,
                    //	new KeyValuePair<string, object>("BATCH_NO", oldBatchNo));
                }
                //this.ChangerLineBegin(currentProduct, woNo, lineId);

                String batchNo = Guid.NewGuid().ToString();

                var productInfoModel = (await _repository.GetListByTableEX<SmtWo>("*", "SMT_WO", " AND WO_NO = :WO_NO", new { WO_NO = plitem.woNo })).FirstOrDefault();
                if (productInfoModel == null)
                {
                    //未获取到此工单信息! 
                    throw new Exception(_localizer["WOINFORMATION_NOT_OBTAINED"]);

                }
                String partNo = productInfoModel.PART_NO.ToString();
                //if (!ErrorInfo.Status&& plitem.keepWo)
                //{
                //    if (currentProduct==null || !currentProduct.FirstOrDefault().PCB_PN.Equals(partNo))
                //    {
                //        //上线工单的产品编号{0}与当前正在生产的工单产品不一致无法续借工单! 
                //        throw new Exception(String.Format("上线工单的产品编号{0}与当前正在生产的工单产品不一致无法续借工单!", partNo));
                //    }
                //}
                String curentDate = await _repository.GetCurrentTime();
                String locNo = "BT" + plitem.lineId + curentDate;
                String model = productInfoModel.MODEL.IsNullOrEmpty() ? "" : productInfoModel.MODEL.ToString();
                var productLine = new SfcsProductionAddOrModifyModel();
                productLine.BATCH_NO = batchNo;
                productLine.LINE_ID = plitem.lineId;
                productLine.WO_NO = plitem.woNo;
                productLine.PCB_PN = partNo;
                productLine.PCB_SIDE = 0;
                productLine.MODEL = model;
                productLine.START_BY = plitem.user;
                productLine.FINISHED = "N";
                productLine.STATION_ID = 0;
                productLine.OPERATION_TYPE = 1;
                productLine.MULTI_NO = plitem.multNo;
                productLine.LOC_NO = locNo;
                var productResult = await _repository.InsertProductLine(productLine);
                if (productResult == -1)
                {
                    //产线开工失败! The production line failed to start!
                    throw new Exception(_localizer["PRODUCTION_FAILED_START"]);
                }

                //设定工单制程
                await _repository.SetRoute(plitem.routeId, plitem.woNo);

                List<dynamic> ebomMaterialListen = new List<dynamic>();

                //测试工单:WORK163017
                //根据工单找投料单
                string selectFeedingBillSql = @"SELECT PART.CODE PART_CODE, SUM(DTL.QUANTITY - DTL.BALANCE_QUANTITY) AS ISSUE_QTY 
                                FROM IMS_INTERFACE_DTL@WMS DTL
                                JOIN IMS_INTERFACE_MST@WMS MST ON MST.ID = DTL.MST_ID
                                JOIN IMS_PART@WMS PART ON PART.ID = DTL.PART_ID
                                JOIN IMS_WO_MST@WMS WMST ON WMST.INTERFACE_MST_ID = MST.ID
                                 WHERE WMST.WORK_ORDER = :WORK_ORDER
                                GROUP BY PART.CODE
                                HAVING SUM(DTL.QUANTITY - DTL.BALANCE_QUANTITY)>0";
              var FeedingBillList= await _repository.QueryAsyncEx<dynamic>(selectFeedingBillSql,new { WORK_ORDER = plitem.woNo });
                
                if (FeedingBillList==null|| FeedingBillList.Count<=0)
                {
                    //未获取到新的BOM零件物料数据,请您工程确认配置零件物料数据!
                    //throw new Exception(_localizer["BOM_PART_NOT_DATA1"]);
                    //没有找到对应的投料单,请您联系工程确认配置物料数据!
                    throw new Exception(_localizer["FEEDING_NOT_FOUND"]);
                }
                //switch (parameterModle.CHINESE.ToUpper())
                //{
                //    case "创维":
                //        //同步BOM信息
                //        _service = new BomVsPlacementService(_bomVsPlacementRepository, _bomLocalizer);
                //        await _service.LoadBomEX(productInfoModel.PART_NO, BomType.西门子组件);
                //        if (_service.IsError)
                //        {
                //            //未获取到新的BOM零件物料数据,请您工程确认配置零件物料数据!
                //            throw new Exception(_localizer["BOM_PART_NOT_DATA1"]);
                //        }
                //        else
                //        {
                //            ebomMaterialListen = _service.BomInfo.ToList<dynamic>();
                //        }
                //        break;
                //    case "汇泰隆":
                //        //同步Bom信息
                //        _service = new BomVsPlacementService(_bomVsPlacementRepository, _bomLocalizer);
                //        await _service.LoadBomEXbyWONo(productInfoModel.PART_NO, plitem.woNo);
                //        if (_service.IsError)
                //        {
                //            //未获取到新的BOM零件物料数据,请您工程确认配置零件物料数据!
                //            throw new Exception(_localizer["BOM_PART_NOT_DATA1"]);
                //        }
                //        else
                //        {
                //            ebomMaterialListen = _service.BomInfo.ToList<dynamic>();
                //        }
                //        break;
                //    default:
                //        //创建手插件物料状态监听表
                //        //1、查找E-BOM
                //        ebomMaterialListen = await _repository.Select_ebomMaterialListen(productInfoModel.PART_NO);
                //        if (ebomMaterialListen == null|| ebomMaterialListen.Count<=0)
                //        {
                //            //未获取到新的SOP零件物料数据,请您工程确认配置零件物料数据!
                //            throw new Exception(_localizer["SOP_PART_NOT_DATA"]);
                //        }
                //        break;
                //}

                foreach (var item in FeedingBillList)
                {
                    //获取物料信息
                    Decimal MaterialId = await _repository.GetSequenceValue("MES_HI_REEL_SEQ");
                    String materialPn = item?.PART_CODE;
                    
                    MesHiMaterialListenAddOrModifyModel hiMateriaListenModify = new MesHiMaterialListenAddOrModifyModel();
                    hiMateriaListenModify.ID = MaterialId;
                    hiMateriaListenModify.BATCH_NO = batchNo;
                    hiMateriaListenModify.WO_NO = plitem.woNo;
                    hiMateriaListenModify.OPERATION_LINE_ID = plitem.lineId;
                    hiMateriaListenModify.UNITY_QTY = 0;
                    hiMateriaListenModify.PART_NO = materialPn;
                    await _repository.InsertMaterial(hiMateriaListenModify);
                }

                await _repository.productKepMaterial(oldBatchNo, batchNo, plitem.user, plitem.keepWo);

                //从ERP获取产品的位号信息
                //this.GetProductLocByErp(partNo, plitem.user); 
                await _repository.CommitTransaction();
                returnVM.Result = true;
            }
            catch (Exception ex)
            {
                await _repository.RollbackTransaction();
                ErrorInfo.Set(ex.Message, MethodBase.GetCurrentMethod(), EnumErrorType.Error);
            }

            #region 如果出现错误，则写错误日志并返回错误内容

            WriteLog(ref returnVM);

            #endregion

            await _repository.CloseTransaction();

            return returnVM;
        }

        /// <summary>
        /// 上料作业
        /// </summary>
        /// <param name="hiReelInfo"></param>
        /// <returns></returns>
        [HttpPost]
        [Authorize]
        public async Task<ApiBaseReturn<bool>> CheckHIReel([FromBody] HiReelInfoVM hiReelInfo)
        {
            ApiBaseReturn<bool> returnVM = new ApiBaseReturn<bool>();
            returnVM.Result = false;
            await _repository.StartTransaction();
            try
            {
                string woNo = hiReelInfo.WoNo;
                string operationLineId = hiReelInfo.OperationLineId;
                string reelId = hiReelInfo.ReelId;
                string user = hiReelInfo.User;
                //string mac = hiReelInfo.MAC;
                //String S_SelectSiteOperation = @"SELECT SOS.ID, SOS.OPERATION_ID , OPERATOR
                //           FROM MES_LOGIN_HISTORY MLH, 
                //           SFCS_OPERATION_SITES SOS where 
                //           MLH.SITE_ID = SOS.ID AND MAC = :MAC 
                //           AND STATUS = 'Y'";
                //DataTable operationTable = DBA.FromDb("MES").ExecuteGetDataByKeyValuePairs(S_SelectSiteOperation,
                //	new KeyValuePair<string, object>("MAC", mac));
                //if (operationTable.IsNullOrEmpty())
                //{
                //	throw new Exception("扫入的站位异常，没有登录MES系统，或者没有设定站点信息！");
                //}

                //decimal operationid = Decimal.Parse(operationTable.FirstRow()["OPERATION_ID"].ToString());
                //decimal siteId = Decimal.Parse(operationTable.FirstRow()["ID"].ToString());
                //string userAd = operationTable.FirstRow()["OPERATOR"].ToString();
                var currentProduct = await _repository.GetListByTableEX<SfcsProduction>("*", "SFCS_PRODUCTION", " AND LINE_ID = :LINE_ID AND WO_NO=:WO_NO AND FINISHED IN('O', 'N')", new
                {
                    LINE_ID = operationLineId,
                    WO_NO = woNo
                });
                if (currentProduct == null)
                {
                    //当前产线未进行产线开工 The current production line has not started production line CURRENT_PRODUCTION_NOT_START
                    throw new Exception(_localizer["CURRENT_PRODUCTION_NOT_START"]);
                }

                var tempValue = currentProduct.FirstOrDefault().BATCH_NO;
                var listenTable = await _repository.GetListByTableEX<MesHiMaterialListen>("*", "MES_HI_MATERIAL_LISTEN", " AND BATCH_NO = :BATCH_NO AND WO_NO = :WO_NO AND OPERATION_LINE_ID = :OPERATION_LINE_ID ", new
                {
                    BATCH_NO = tempValue,
                    WO_NO = woNo,
                    OPERATION_LINE_ID = operationLineId
                });

                if (listenTable == null || listenTable.Count <= 0)
                {
                    //当前产线开工出现异常! The current production line starts abnormally!  CURRENT_PRODUCTION_ERROR
                    throw new Exception(_localizer["CURRENT_PRODUCTION_ERROR"]);
                }
                Reel reel = await _repository.GetReel(reelId);
                if (reel == null)
                {
                    //输入的物料条码不存在! The entered material barcode does not exist! BARCODE_NOT_EXIST
                    throw new Exception(_localizer["BARCODE_NOT_EXIST"]);
                }

                ////检查BOM零件物料
                //if (await _repository.CheckBOMByPartID(reel.PartID) <= 0)
                //{
                //    throw new Exception(_localizer["BOM_PART_NOT_DATA1"]);
                //}

                // 判断当前物料是否在当前工单，当前线别备过料
                var mesHiReel = await _repository.GetHiReelData(reelId);
                MesHiReel exsitHiReel = null;
                //当前物料{0}为不可用状态，被其他工单占用，或已经使用完!
                if (mesHiReel.Count(f => f.STATUS == 1 || f.STATUS == 2) > 0)
                {
                    throw new Exception(String.Format(_localizer["CURRENTMATERIAL_STATUS_CURRENT_MATERIAL"], reelId));
                }
                if (mesHiReel != null && mesHiReel.Count > 0)
                {
                    if (mesHiReel.Where(f => f.STATUS == 3).Count() > 0)
                    {
                        exsitHiReel = mesHiReel.Where(f => f.STATUS == 3).ToArray().FirstOrDefault();
                    }
                    else
                    {
                        //当前物料{0}为不可用状态，被其他工单占用，或已经使用完!
                        throw new Exception(String.Format(_localizer["CURRENTMATERIAL_STATUS_CURRENT_MATERIAL"], reelId));
                    }
                }

                //判断物料条码是否存在
                if (exsitHiReel != null)
                {
                    //修改状态为4 表示挪料了
                    reel.Quantity = exsitHiReel.QTY;
                    await _repository.Update_ExistReel(exsitHiReel.ID);
                }

                //SFCSDataSet.MES_HI_MATERIAL_LISTENRow[] listenRows = listenTable.Where(f => f.PART_NO == reel.PART_NO && f.OPERATION_ID == operationid).ToArray();
                var listenRows = listenTable.Where(f => f.PART_NO == reel.PART_NO).ToList();
                if (listenRows == null || listenRows.Count <= 0)
                {
                    //判断替代料信息
                    var replaceTable = await _repository.GetListByTableEX<SmtReplacePn>("", "SMT_REPLACE_PN", " AND WO_NO = :WO_NO AND REPLACE_PN = :REPLACE_PN AND ENABLED = 'Y' ", new
                    {
                        WO_NO = woNo,
                        REPLACE_PN = reel.PART_NO
                    });
                    if (replaceTable == null || replaceTable.Count <= 0)
                    {
                        //当前工单{0}，当前物料{1},在当前工位没有需要上料信息 
                        throw new Exception(String.Format(_localizer["CURRENT_WORK_NO_LOAD"], woNo, reel.PART_NO));
                    }
                    else
                    {
                        String currentReelPartNo = reel.PART_NO;
                        reel.PART_NO = replaceTable.FirstOrDefault().COMPONENT_PN.ToString();
                        //listenRows = listenTable.Where(f => f.PART_NO == reel.PART_NO && f.OPERATION_ID == operationid).ToArray();
                        listenRows = listenTable.Where(f => f.PART_NO == reel.PART_NO).ToList();
                        if (listenRows == null)
                        {
                            //当前工单{0}，当前物料{1},在当前工位没有需要上料信息 
                            throw new Exception(String.Format(_localizer["CURRENT_WORK_NO_LOAD"], woNo, currentReelPartNo));
                        }
                    }

                }
                decimal id = listenRows.FirstOrDefault().ID;
                MesHiReelAddOrModifyModel hiReelModel = new MesHiReelAddOrModifyModel();
                if (listenRows.FirstOrDefault().CURR_REEL_ID.IsNullOrEmpty())
                {
                    decimal preQty = (listenRows.FirstOrDefault().PRE_QTY) == null ? 0 : (listenRows.FirstOrDefault().PRE_QTY ?? 0) + reel.Quantity;
                    await _repository.UpdateListen(id, preQty, reelId);

                    //记录备料数据,物料为1：在用状态
                    //SFCSDataManager.InsertHiReelData(woNo, reelId, operationLineId, reel.Quantity, user, listenRows.FirstOrDefault().BATCH_NO, reel.PART_NO, 1, operationid, siteId, userAd);
                    hiReelModel.ID = await _repository.GetSequenceValue("MES_HI_REEL_SEQ");
                    hiReelModel.WO_NO = woNo;
                    hiReelModel.REEL_ID = reelId;
                    hiReelModel.OPERATION_LINE_ID = operationLineId;
                    hiReelModel.QTY = reel.Quantity;
                    hiReelModel.OPERTOR = user;
                    hiReelModel.USED_QTY = 0;
                    hiReelModel.ORG_QTY = reel.Quantity;
                    hiReelModel.BATCH_NO = listenRows.FirstOrDefault().BATCH_NO;
                    hiReelModel.PART_NO = reel.PART_NO;
                    hiReelModel.STATUS = 1;
                    await _repository.InsertHIREEL(hiReelModel);
                }
                else
                {
                    decimal preQty = (listenRows.FirstOrDefault().PRE_QTY ?? 0) + reel.Quantity;
                    await _repository.UpdateListen(id, preQty);
                    //记录备料数据，物料为0：待用状态
                    //SFCSDataManager.InsertHiReelData(woNo, reelId, operationLineId, reel.Quantity, user, listenRows.FirstOrDefault().BATCH_NO, reel.PART_NO, 0, operationid, siteId, userAd);
                    hiReelModel.ID = await _repository.GetSequenceValue("MES_HI_REEL_SEQ");
                    hiReelModel.WO_NO = woNo;
                    hiReelModel.REEL_ID = reelId;
                    hiReelModel.OPERATION_LINE_ID = operationLineId;
                    hiReelModel.QTY = reel.Quantity;
                    hiReelModel.OPERTOR = user;
                    hiReelModel.USED_QTY = 0;
                    hiReelModel.ORG_QTY = reel.Quantity;
                    hiReelModel.BATCH_NO = listenRows.FirstOrDefault().BATCH_NO;
                    hiReelModel.PART_NO = reel.PART_NO;
                    hiReelModel.STATUS = 0;
                    await _repository.InsertHIREEL(hiReelModel);
                }


                decimal result = await _repository.SelectListenBYBatch(currentProduct.FirstOrDefault().BATCH_NO);
                if (result <= 0 && currentProduct.FirstOrDefault().FINISHED.Equals("O"))
                {
                    //备齐物料，当前工单正式上线
                    await _repository.UpdateProductionBYBatch(currentProduct.FirstOrDefault().BATCH_NO);

                    //备齐物料换线成功
                    //this.ChangeLineEnd(woNo, Decimal.Parse(operationLineId));
                    await _repository.CommitTransaction();
                    returnVM.Result = true;
                }
                await _repository.CommitTransaction();

                var selectProduct = await _repository.GetListByTableEX<SfcsProduction>("*", "SFCS_PRODUCTION", " AND LINE_ID = :LINE_ID AND WO_NO=:WO_NO AND FINISHED IN('O', 'N')", new
                {
                    LINE_ID = operationLineId,
                    WO_NO = woNo
                });
                if (selectProduct.FirstOrDefault().FINISHED.Equals("O"))
                {
                    returnVM.Result = false;
                }
                else
                {
                    returnVM.Result = true;
                }
            }
            catch (Exception ex)
            {
                await _repository.RollbackTransaction();
                //returnVM.Set(ex.Message);
                //return ex.ToModel("apiSMTController.checkHIReel", true);
                ErrorInfo.Set(ex.Message, MethodBase.GetCurrentMethod(), EnumErrorType.Error);
            }
            if (!await _repository.CloseTransaction())
            {
                returnVM.Result = false;
            }

            #region 如果出现错误，则写错误日志并返回错误内容

            if (ErrorInfo.Status)
            {
                returnVM.ErrorInfo.Set(ErrorInfo);
                if (ErrorInfo.ErrorType == EnumErrorType.Error)
                {
                    CreateErrorLog(ErrorInfo);
                }
                ErrorInfo.Clear();
            }

            #endregion

            return returnVM;
        }

        /// <summary>
        /// 产线下线作业
        /// </summary>
        /// <param name="lineId"></param>
        /// <param name="user"></param>
        /// <returns></returns>
        [HttpPost]
        [Authorize]
        public async Task<ApiBaseReturn<bool>> ProducLineEnd([FromBody] ProducLineEndLIst modelArry)
        {
            ApiBaseReturn<bool> returnVM = new ApiBaseReturn<bool>();
            returnVM.Result = false;
            await _repository.StartTransaction();
            try
            {
                if (modelArry != null && modelArry.ProducLineArray != null && modelArry.ProducLineArray.Count > 0)
                {
                    foreach (var model in modelArry.ProducLineArray)
                    {
                        var currentProduct = await _repository.GetListByTableEX<SfcsProduction>("*", "SFCS_PRODUCTION", " AND LINE_ID = :LINE_ID AND WO_NO=:WO_NO AND FINISHED IN('O', 'N')", new
                        {
                            LINE_ID = model.lineId,
                            WO_NO = model.woNo
                        });

                        String oldBatchNo = String.Empty;
                        if (currentProduct != null && currentProduct.Count > 0)
                        {
                            //如果当前的线别有生产数据
                            //先修改当前线别生产的数据
                            oldBatchNo = currentProduct.FirstOrDefault().BATCH_NO;

                            await _repository.UpdateProductLine(model.user, model.lineId, model.woNo);
                            // 关闭当前上线工单的手插件物料状态监听表
                            //MES_HI_MATERIAL_LISTEN
                            await _repository.UpdateCloseListen(oldBatchNo);

                            await _repository.productKepMaterial(oldBatchNo, null, model.user, false);
                        }
                    }
                }
                await _repository.CommitTransaction();
                returnVM.Result = true;
            }
            catch (Exception ex)
            {
                await _repository.RollbackTransaction();
                ErrorInfo.Set(ex.Message, MethodBase.GetCurrentMethod(), EnumErrorType.Error);
                //return ex.ToModel("apiSMTController.ProducLineEnd", true);
            }
            if (!await _repository.CloseTransaction())
            {
                returnVM.Result = false;
            }
            return returnVM;
        }

    }
}
