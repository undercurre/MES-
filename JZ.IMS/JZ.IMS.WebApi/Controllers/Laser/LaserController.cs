using AutoMapper;
using JZ.IMS.Core.Extensions;
using JZ.IMS.Core.Utilities;
using JZ.IMS.IRepository;
using JZ.IMS.Models;
using JZ.IMS.ViewModels;
using JZ.IMS.WebApi.Common;
using JZ.IMS.WebApi.Public;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using RadixConvertPublic = JZ.IMS.WebApi.Common.RadixConvertPublic;

namespace JZ.IMS.WebApi.Controllers
{
    /// <summary>
    /// 镭雕机 控制器  
    /// </summary>
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class LaserController : BaseController
    {
        private readonly ISfcsWoRepository _woRepository;
        private readonly ISfcsPnRepository _pnRepository;
        private readonly ISfcsProductFamilyRepository _pfRepository;
        private readonly ISfcsCustomersRepository _customersRepository;
        private readonly ISfcsLaserRangerRepository _laserRangerRepository;
        private readonly ISfcsLaserExceptionLogRepository _laserExceptionLogRepository;
        private readonly ISfcsLaserRecordRepository _laserRecordRepository;
        private readonly ISfcsLaserTaskRepository _laserTaskRepository;
        private readonly ISfcsRuncardRangerRepository _runcardRangerRepository;
        private readonly ISfcsRuncardRangerRulesRepository _runcardRangerRulesRepository;
        private readonly ISfcsLaserRangerRulesRepository _laserRangerRulesRepository;
        private readonly ISfcsRuncardReplaceRepository _runcardReplaceRepository;
        private readonly ISfcsParametersRepository _parameRepository;
        private readonly ISfcsProductConfigRepository _productConfigRepository;
        private readonly ISmtMultipanelHeaderRepository _multipanelHeaderRepository;
        private readonly IImportRuncardSnRepository _importRuncardSnRepository;


        private readonly IMapper _mapper;
        private readonly IStringLocalizer<LaserController> _localizer;

        public LaserController(ISfcsWoRepository woRepository, ISfcsPnRepository pnRepository, ISfcsProductFamilyRepository pfRepository, ISfcsCustomersRepository customersRepository,
            ISfcsLaserRangerRepository laserRangerRepository, ISfcsLaserExceptionLogRepository laserExceptionLogRepository, ISfcsLaserRecordRepository laserRecordRepository,
            ISfcsRuncardRangerRepository runcardRangerRepository, ISfcsRuncardRangerRulesRepository runcardRangerRulesRepository, ISfcsRuncardReplaceRepository runcardReplaceRepository, ISfcsLaserRangerRulesRepository laserRangerRulesRepository,
            ISfcsParametersRepository parameRepository, ISfcsProductConfigRepository productConfigRepository,
            ISmtMultipanelHeaderRepository multipanelHeaderRepository, IImportRuncardSnRepository importRuncardSnRepository,
            ISfcsLaserTaskRepository laserTaskRepository,
            IMapper mapper,
            IStringLocalizer<LaserController> localizer)
        {
            _woRepository = woRepository;
            _pnRepository = pnRepository;
            _pfRepository = pfRepository;
            _customersRepository = customersRepository;
            _laserRangerRepository = laserRangerRepository;
            _laserExceptionLogRepository = laserExceptionLogRepository;
            _laserRecordRepository = laserRecordRepository;
            _runcardRangerRepository = runcardRangerRepository;
            _runcardRangerRulesRepository = runcardRangerRulesRepository;
            _laserRangerRulesRepository = laserRangerRulesRepository;
            _runcardReplaceRepository = runcardReplaceRepository;
            _parameRepository = parameRepository;
            _productConfigRepository = productConfigRepository;
            _multipanelHeaderRepository = multipanelHeaderRepository;
            _importRuncardSnRepository = importRuncardSnRepository;
            _laserTaskRepository = laserTaskRepository;
            _mapper = mapper;
            _localizer = localizer;
        }

        #region 获取新板子的镭雕数据接口
        /// <summary>
        /// 获取新板子的镭雕数据
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [AllowAnonymous]
        public async Task<LaserApiBaseReturn<NewLaserDataResultModel>> GetLaserData([FromBody] NewLaserDataRequestModel model)
        {
            LaserApiBaseReturn<NewLaserDataResultModel> returnVM = new LaserApiBaseReturn<NewLaserDataResultModel>();
            returnVM.header = new LaserApiBaseReturnHeader();
            returnVM.body = new NewLaserDataResultModel();

            if (!ErrorInfo.Status)
            {
                try
                {
                    #region 检查参数
                    if (!ErrorInfo.Status)
                    {
                        if (model == null)
                        {
                            //请求实体不能为空
                            ErrorInfo.Set(_localizer["model_notnull"], MethodBase.GetCurrentMethod(), EnumErrorType.Warning);
                        }
                        else if (model.EQP_ID.IsNullOrWhiteSpace())
                        {
                            //请求参数EQP_ID不能为空
                            ErrorInfo.Set(_localizer["eqp_id_notnull"], MethodBase.GetCurrentMethod(), EnumErrorType.Warning);
                        }
                        else if (model.CLIENT_TIME.IsNullOrWhiteSpace())
                        {
                            //请求参数CLIENT_TIME不能为空
                            ErrorInfo.Set(_localizer["client_time_notnull"], MethodBase.GetCurrentMethod(), EnumErrorType.Warning);
                        }
                        else if (model.WORK_ORDER.IsNullOrWhiteSpace())
                        {
                            //请求参数WORK_ORDER不能为空
                            ErrorInfo.Set(_localizer["work_order_notnull"], MethodBase.GetCurrentMethod(), EnumErrorType.Warning);
                        }
                        else if (model.UNIT_SN_COUNT <= 0)
                        {
                            //请求参数UNIT_SN_COUNT必须大于0
                            ErrorInfo.Set(_localizer["unit_sn_count_notnull"], MethodBase.GetCurrentMethod(), EnumErrorType.Warning);
                        }
                        else if (model.UNIT_MAC_LIST.Count > 0 && model.UNIT_SN_COUNT != model.UNIT_MAC_LIST.Count)
                        {
                            //需要返回的SN数量必须与小板MAC集合数量相等
                            ErrorInfo.Set(_localizer["unit_mac_list_err"], MethodBase.GetCurrentMethod(), EnumErrorType.Warning);
                        }
                    }
                    if (!ErrorInfo.Status)
                    {
                        bool existsWoNo = await _woRepository.ConfirmWorkOrderExisted(model.WORK_ORDER);
                        if (!existsWoNo)
                        {
                            //工单{0}不存在，请确认。
                            ErrorInfo.Set(_localizer["err_wo_not_found"], MethodBase.GetCurrentMethod(), EnumErrorType.Warning);
                        }
                    }
                    #endregion

                    #region 设置返回值
                    if (!ErrorInfo.Status)
                    {
                        #region 获得产品基本信息 GetProductInfo()
                        SfcsWo sfcsWo = await _woRepository.GetAsync("SELECT * FROM SFCS_WO WHERE WO_NO = :WO_NO", new { WO_NO = model.WORK_ORDER });
                        string partNumber = sfcsWo.PART_NO;
                        decimal woID = sfcsWo.ID;
                        //decimal plantCode = sfcsWo.PLANT_CODE;
                        decimal familyId = 0;
                        string familyName = "";

                        SfcsPn partNumberRow = await _pnRepository.GetAsync("SELECT * FROM SFCS_PN WHERE PART_NO = :PART_NO", new { PART_NO = partNumber });

                        SfcsProductFamily productFamilyRow = null;
                        if (!partNumberRow.IsNullOrWhiteSpace() && !partNumberRow.FAMILY_ID.IsNullOrEmpty() && partNumberRow.FAMILY_ID > 0)
                        {
                            productFamilyRow = await _pfRepository.GetAsync("SELECT * FROM SFCS_PRODUCT_FAMILY WHERE ID = :ID ", new { ID = partNumberRow.FAMILY_ID });
                            familyId = productFamilyRow.IsNullOrWhiteSpace() ? 0 : Convert.ToDecimal(productFamilyRow.ID);
                            familyName = productFamilyRow.IsNullOrWhiteSpace() ? "" : productFamilyRow.FAMILY_NAME;
                        }

                        SfcsCustomers customerRow = await _customersRepository.GetAsync(partNumberRow.CUSTOMER_ID);
                        #endregion

                        string snFormat = await this.GetSNFormat(partNumber, false);//取得SN Format

                        if (!ErrorInfo.Status)
                        {
                            #region 简单流程：使用工单流水号范围生成SN
                            // 自動創建流水號範圍
                            var laserRangerList = await this.AutoCreateRuncardRanger(woID, sfcsWo.WO_NO, snFormat, partNumber, partNumberRow.CUSTOMER_ID, customerRow.CUSTOMER, model.UNIT_SN_COUNT, familyId, familyName);

                            if (!ErrorInfo.Status && !laserRangerList.IsNullOrEmpty())
                            {
                                //生成镭雕机流水号
                                List<NewLaserDataResultItem> resultList = new List<NewLaserDataResultItem>();
                                foreach (var laserRanger in laserRangerList)
                                {
                                    List<NewLaserDataResultItem> serialNumberList = await this.GenerateRangerSN(laserRanger);
                                    foreach (var sn in serialNumberList)
                                    {
                                        resultList.Add(sn);
                                    }
                                }
                                returnVM.body.UNIT_SN_LIST = resultList;
                                returnVM.body.SERVER_TIME = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff");
                                returnVM.header.code = "200";
                                returnVM.header.desc = "";
                            }
                            #endregion
                            #region 复杂流程
                            else if (!ErrorInfo.Status)
                            {
                                var woRangerRule = await _laserRangerRulesRepository.GetAsync("SELECT * FROM SFCS_LASER_RANGER_RULES WHERE WO_NO = :WO_NO AND ENABLED = :ENABLED",
                                    new { WO_NO = sfcsWo.WO_NO, ENABLED = GlobalVariables.EnableY });
                                if (!woRangerRule.IsNullOrEmpty())
                                {
                                    //镭雕SN总数不能超过工单总数
                                    decimal unitSnCount = model.UNIT_SN_COUNT;
                                    var woSnCount = await _importRuncardSnRepository.GetTotalSnByWoNo(sfcsWo.WO_NO, GlobalVariables.EnableY);
                                    if (sfcsWo.TARGET_QTY.HasValue && sfcsWo.TARGET_QTY.Value - woSnCount < unitSnCount)
                                    {
                                        unitSnCount = sfcsWo.TARGET_QTY.Value - woSnCount;
                                    }
                                    if (unitSnCount <= 0)
                                    {
                                        //当前工单已没有可用的SN
                                        ErrorInfo.Set(_localizer["err_wo_not_sn"], MethodBase.GetCurrentMethod(), EnumErrorType.Warning);
                                    }

                                    if (!ErrorInfo.Status)
                                    {
                                        if ((!woRangerRule.FIX_HEADER.IsNullOrWhiteSpace() && woRangerRule.FIX_HEADER.Contains("<UNIT_KEY>")) || (!woRangerRule.FIX_TAIL.IsNullOrWhiteSpace() && woRangerRule.FIX_TAIL.Contains("<UNIT_KEY>")))
                                        {
                                            if (model.UNIT_MAC_LIST.Count <= 0)
                                            {
                                                //请求参数UNIT_MAC_LIST集合不能为空
                                                ErrorInfo.Set(_localizer["unit_mac_list_notnull"], MethodBase.GetCurrentMethod(), EnumErrorType.Warning);
                                            }
                                        }

                                        if (!ErrorInfo.Status)
                                        {
                                            List<NewLaserDataResultItem> resultList = new List<NewLaserDataResultItem>();
                                            List<NewLaserDataResultItemDataBase> resultList2 = new List<NewLaserDataResultItemDataBase>();

                                            //生成镭雕机流水号
                                            List<NewLaserDataResultItemDataBase> serialNumberList = await this.GenerateRangerSN2(woRangerRule, unitSnCount, model.UNIT_MAC_LIST);
                                            foreach (var sn in serialNumberList)
                                            {
                                                resultList.Add(new NewLaserDataResultItem() { NO = sn.NO, UNIT_SN = sn.UNIT_SN });
                                                resultList2.Add(sn);
                                            }

                                            if (resultList2.Count > 0)
                                            {
                                                #region 保存客户SN
                                                var importRuncardSnSave = new ImportRuncardSnModel();
                                                importRuncardSnSave.InsertRecords = new List<ImportRuncardSnAddOrModifyModel>();
                                                foreach (var item in resultList2)
                                                {
                                                    var checkWoSN = await _importRuncardSnRepository.GetAsync("SELECT * FROM IMPORT_RUNCARD_SN WHERE WO_NO = :WO_NO AND SN = :SN",
                                                        new { WO_NO = sfcsWo.WO_NO, SN = item.UNIT_SN });
                                                    if (checkWoSN.IsNullOrEmpty())
                                                    {
                                                        ImportRuncardSnAddOrModifyModel insertRecord = new ImportRuncardSnAddOrModifyModel();
                                                        insertRecord.WO_NO = sfcsWo.WO_NO;
                                                        insertRecord.SN = item.UNIT_SN;
                                                        insertRecord.ROUTE_ID = 0;
                                                        insertRecord.ENABLE = GlobalVariables.EnableN;
                                                        insertRecord.CREATE_BY = model.EQP_ID;
                                                        insertRecord.UPDATE_BY = model.EQP_ID;
                                                        insertRecord.ATTRIBUTE1 = item.UNIT_SN_RANGE;
                                                        importRuncardSnSave.InsertRecords.Add(insertRecord);
                                                    }
                                                }
                                                if (importRuncardSnSave.InsertRecords.Count > 0) await _importRuncardSnRepository.SaveDataByTrans(importRuncardSnSave);
                                                #endregion
                                            }

                                            returnVM.body.UNIT_SN_LIST = resultList;
                                            returnVM.body.SERVER_TIME = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff");
                                            returnVM.header.code = "200";
                                            returnVM.header.desc = "";
                                        }
                                    }
                                }
                                else
                                {
                                    // 没有找到自动生成规则，需手动设定范围
                                    ErrorInfo.Set(_localizer["err_rule_not_found"], MethodBase.GetCurrentMethod(), EnumErrorType.Warning);
                                }
                            }
                            #endregion
                        }
                    }
                    #endregion
                }
                catch (Exception ex)
                {
                    ErrorInfo.Set(ex.Message, MethodBase.GetCurrentMethod(), EnumErrorType.Error);
                }
            }

            #region 如果出现错误，则写错误日志并返回错误内容

            if (ErrorInfo.Status)
            {
                returnVM.header.code = "101";
                returnVM.header.desc = ErrorInfo.Message;
                if (ErrorInfo.ErrorType == EnumErrorType.Error)
                {
                    CreateErrorLog(ErrorInfo);
                }
                ErrorInfo.Clear();
            }

            #endregion

            return returnVM;
        }

        #region methods
        /// <summary>
        /// 取得SN Format
        /// </summary>
        private async Task<string> GetSNFormat(string partNumber, bool checkSNBeginFormat = true)
        {
            SfcsProductConfig configRow = await _productConfigRepository.GetAsync("SELECT * FROM SFCS_PRODUCT_CONFIG WHERE PART_NO = :PART_NO AND CONFIG_TYPE = :CONFIG_TYPE",
                new { PART_NO = partNumber, CONFIG_TYPE = GlobalVariables.SNFormat });
            if (configRow.IsNull())
            {
                configRow = await _productConfigRepository.GetAsync("SELECT * FROM SFCS_PRODUCT_CONFIG WHERE PART_NO = :PART_NO AND CONFIG_TYPE = :CONFIG_TYPE",
                new { PART_NO = "000000000000", CONFIG_TYPE = GlobalVariables.SNFormat });
            }
            if (configRow.IsNull() ||
                configRow.CONFIG_VALUE.IsNullOrEmpty())
            {
                //料号{0}没有设定流水号格式。
                ErrorInfo.Set(string.Format(_localizer["err_nofomat"], partNumber), MethodBase.GetCurrentMethod(), EnumErrorType.Warning);
            }
            if (!ErrorInfo.Status)
            {
                //if (checkSNBeginFormat)
                //{
                //	if (!FormatChecker.FormatCheck(
                //		this.snBeginTextEdit.Text, configRow.CONFIG_VALUE.Trim()))
                //	{
                //		throw new MESException(Properties.Resources.Err_SNFormat,
                //			this.snBeginTextEdit.Text, configRow.CONFIG_VALUE);
                //	}
                //}

                return configRow.CONFIG_VALUE.Trim();
            }
            return string.Empty;
        }
        /// <summary>
        /// 自動創建流水號範圍
        /// </summary>
        private async Task<List<SfcsLaserRanger>> AutoCreateRuncardRanger(decimal woID, string woNo, string snFormat, string partNumber, decimal customerId, string customerName, decimal quantity, decimal familyId, string familyName, decimal platformId = 0, string platform = "")
        {
            try
            {
                //先是成品料号再到产品系列最后客户
                string partNumberFound = string.Empty;
                string familyIDFound = string.Empty;
                string platformIDFound = string.Empty;
                string customerIDFound = string.Empty;
                string totalRuleFound = string.Empty;
                int foundCount = 0;

                SfcsRuncardRangerRules runcardRangerRuleRow = null;

                SfcsRuncardRangerRules runcardRangerRuleByPNRow = await _runcardRangerRulesRepository.GetRuncardRangerRulesByPN(partNumber, "Y", GlobalVariables.RangerSN);
                if (!runcardRangerRuleByPNRow.IsNullOrEmpty())
                {
                    partNumberFound = "PartNumber#" + partNumber + " ";
                    totalRuleFound = string.Concat(totalRuleFound, partNumberFound);
                    foundCount = ++foundCount;
                }

                SfcsRuncardRangerRules runcardRangerRuleByFamilyRow = null;
                if (familyId > 0)
                {
                    runcardRangerRuleByFamilyRow = await _runcardRangerRulesRepository.GetRuncardRangerRulesByFamilyId(platformId, "Y", GlobalVariables.RangerSN);
                    if (!runcardRangerRuleByFamilyRow.IsNullOrEmpty())
                    {
                        familyIDFound = "PlatformName#" + familyName + " ";
                        totalRuleFound = string.Concat(totalRuleFound, familyIDFound);
                        foundCount = ++foundCount;
                    }
                }

                SfcsRuncardRangerRules runcardRangerRuleByPlatformRow = null;
                if (platformId > 0)
                {
                    runcardRangerRuleByPlatformRow = await _runcardRangerRulesRepository.GetRuncardRangerRulesByPlatformId(platformId, "Y", GlobalVariables.RangerSN);
                    if (!runcardRangerRuleByPlatformRow.IsNullOrEmpty())
                    {
                        platformIDFound = "PlatformName#" + platform + " ";
                        totalRuleFound = string.Concat(totalRuleFound, platformIDFound);
                        foundCount = ++foundCount;
                    }
                }

                SfcsRuncardRangerRules runcardRangerRuleByCustomerRow = await _runcardRangerRulesRepository.GetRuncardRangerRulesByCustomerId(customerId, "Y", GlobalVariables.RangerSN);
                //if (runcardRangerRuleByCustomerRow.IsNullOrEmpty())
                //{
                //    runcardRangerRuleByCustomerRow = await _runcardRangerRulesRepository.GetRuncardRangerRulesByCustomerId(127537, "Y", GlobalVariables.RangerSN);
                //}
                if (!runcardRangerRuleByCustomerRow.IsNullOrEmpty())
                {
                    customerIDFound = "CustomerName#" + customerName + " ";
                    totalRuleFound = string.Concat(totalRuleFound, customerIDFound);
                    foundCount = ++foundCount;
                }

                // 范围选定遵循从细到广的原则
                if (!runcardRangerRuleByPNRow.IsNullOrEmpty())
                {
                    runcardRangerRuleRow = runcardRangerRuleByPNRow;
                }
                else if (!runcardRangerRuleByFamilyRow.IsNullOrEmpty())
                {
                    runcardRangerRuleRow = runcardRangerRuleByFamilyRow;
                }
                else if (!runcardRangerRuleByPlatformRow.IsNullOrEmpty())
                {
                    runcardRangerRuleRow = runcardRangerRuleByPlatformRow;
                }
                else if (!runcardRangerRuleByCustomerRow.IsNullOrEmpty())
                {
                    runcardRangerRuleRow = runcardRangerRuleByCustomerRow;
                }

                if (runcardRangerRuleRow == null)
                {
                    // 没有找到自动生成规则，需手动设定范围
                    //ErrorInfo.Set(_localizer["err_rule_not_found"], MethodBase.GetCurrentMethod(), EnumErrorType.Warning);
                }
                else
                {
                    // 辨析規則中的變化編碼
                    string fixHeader = await this.IdentitySpecialCodes(runcardRangerRuleRow.FIX_HEADER.IsNullOrWhiteSpace() ? "" : runcardRangerRuleRow.FIX_HEADER);
                    string fixTail = string.Empty;
                    if (!runcardRangerRuleRow.FIX_TAIL.IsNullOrWhiteSpace())
                    {
                        fixTail = await this.IdentitySpecialCodes(runcardRangerRuleRow.FIX_TAIL);
                    }
                    if (fixHeader.IsNullOrEmpty() && fixTail.IsNullOrEmpty())
                    {
                        //throw new MESException("固定头和固定尾不能同时为空!");
                        ErrorInfo.Set(_localizer["err_fixheader_fixtail"], MethodBase.GetCurrentMethod(), EnumErrorType.Warning);
                    }

                    if (!ErrorInfo.Status)
                    {
                        SfcsParameters row = await _parameRepository.GetAsync("SELECT * FROM SFCS_PARAMETERS WHERE LOOKUP_TYPE = :LOOKUP_TYPE AND LOOKUP_CODE = :LOOKUP_CODE",
                        new { LOOKUP_TYPE = "RADIX_TYPE", LOOKUP_CODE = runcardRangerRuleRow.DIGITAL });

                        if (row.IsNullOrWhiteSpace())
                        {
                            //throw new Exception("进制类型设定有误，请联系管理员处理。");
                            ErrorInfo.Set(_localizer["err_radix_type"], MethodBase.GetCurrentMethod(), EnumErrorType.Warning);
                        }

                        if (!ErrorInfo.Status)
                        {
                            string radixString = row.DESCRIPTION;

                            string snBegin = string.Empty;

                            // 从SFCS_RUNCARD_RANGER获取已配置的流水码
                            string rrCondictions = @" WHERE WO_ID = :WO_ID AND RANGER_RULE_ID = :RANGER_RULE_ID AND DIGITAL = :DIGITAL AND RANGE = :RANGE ";
                            if (!fixHeader.IsNullOrWhiteSpace())
                            {
                                rrCondictions += " AND FIX_HEADER = :FIX_HEADER";
                            }
                            if (!fixTail.IsNullOrWhiteSpace())
                            {
                                rrCondictions += " AND FIX_TAIL = :FIX_TAIL";
                            }
                            List<SfcsRuncardRanger> runcardRangerList = (await _runcardRangerRepository.GetListAsync(rrCondictions,
                                new
                                {
                                    WO_ID = woID,
                                    RANGER_RULE_ID = runcardRangerRuleRow.ID,
                                    DIGITAL = runcardRangerRuleRow.DIGITAL,
                                    RANGE = runcardRangerRuleRow.RANGE_LENGTH,
                                    FIX_HEADER = fixHeader,
                                    FIX_TAIL = fixTail
                                })).OrderBy(t => t.ID).ToList();
                            if (runcardRangerList.Count <= 0)
                            {
                                //工单流水号范围未配置，请联系管理员处理。
                                ErrorInfo.Set(_localizer["err_runcard_ranger"], MethodBase.GetCurrentMethod(), EnumErrorType.Warning);
                            }


                            if (!ErrorInfo.Status)
                            {
                                var laserRangerRow = await _laserRangerRepository.GetLastRangerByRule(
                                    runcardRangerRuleRow.ID, runcardRangerRuleRow.DIGITAL,
                                     runcardRangerRuleRow.RANGE_LENGTH, fixHeader, fixTail);
                                if (laserRangerRow.IsNullOrEmpty())
                                {
                                    // 第一段範圍，依照指定的開始碼產生SN Begin
                                    snBegin = (fixHeader + runcardRangerRuleRow.RANGE_START_CODE + fixTail).Trim();
                                }
                                else
                                {
                                    int rangeLength = (int)runcardRangerRuleRow.RANGE_LENGTH;
                                    string lastSNCode = laserRangerRow.SN_END.Substring(fixHeader.Length, rangeLength);

                                    string snBeginCode = Core.Utilities.RadixConvertPublic.RadixInc(lastSNCode, radixString, 1).PadLeft(rangeLength, '0');
                                    snBegin = fixHeader + snBeginCode + fixTail;
                                }

                                var returnVM = new List<SfcsLaserRanger>();
                                var insertRecords = new List<SfcsLaserRangerAddOrModifyModel>();
                                var updateRecords = new List<SfcsLaserRangerAddOrModifyModel>();
                                var updateRecord = _mapper.Map<SfcsLaserRangerAddOrModifyModel>(laserRangerRow);
                                bool hasUpdate = false;
                                string oldRuncardRangerId = "";
                                if (!laserRangerRow.IsNullOrEmpty()) oldRuncardRangerId = laserRangerRow.RUNCARD_RANGER_ID.ToString();
                                int curQuantity = 1;
                                string sn = snBegin;
                                foreach (SfcsRuncardRanger runcardRangerRow in runcardRangerList)//从工单流水号中开始产生SN
                                {
                                    string snRange = "";
                                    int rangeLength = runcardRangerRow.RANGE.HasValue ? (int)runcardRangerRow.RANGE.Value : 0;
                                    int headerLength = snBegin.Length - rangeLength - fixTail.Length;
                                    string fixHeaderTextEdit = "";
                                    if (!runcardRangerRow.FIX_HEADER.IsNullOrEmpty()) fixHeaderTextEdit = runcardRangerRow.FIX_HEADER;
                                    string fixTailTextEdit = "";
                                    if (!runcardRangerRow.FIX_TAIL.IsNullOrEmpty()) fixTailTextEdit = runcardRangerRow.FIX_TAIL;
                                    if (rangeLength > 0)
                                    {
                                        string runcardRangeRow_SnBeginRange = runcardRangerRow.SN_BEGIN.Substring(fixHeaderTextEdit.Length, rangeLength);//工单的开始流水号
                                        string runcardRangeRow_SnEndRange = runcardRangerRow.SN_END.Substring(fixHeaderTextEdit.Length, rangeLength);//工单的结束流水号
                                        bool gotoNext = false;
                                        while (curQuantity <= quantity)
                                        {
                                            if (rangeLength == sn.Length)
                                            {
                                                snRange = sn;
                                            }
                                            else
                                            {
                                                snRange = sn.Substring(headerLength, rangeLength);
                                            }
                                            if (!SFCSExpression.IsMatch(snRange, SFCSExpression.Alphanumeric)) continue;

                                            //    snEndRange = Core.Utilities.RadixConvertPublic.RadixInc(snBeginRange, radixString,
                                            //(int)quantity - 1).PadLeft(rangeLength, '0');

                                            //    string snEndTextEdit = runcardRangerRow.FIX_HEADER + snEndRange + runcardRangerRow.FIX_TAIL;//结束流水号

                                            //    if (!FormatChecker.FormatCheck(snEndTextEdit, snFormat)) continue;

                                            if (string.Compare(runcardRangeRow_SnBeginRange, snRange) <= 0
                                                && string.Compare(snRange, runcardRangeRow_SnEndRange) <= 0)//如果流水号在本范围内
                                            {
                                                if (oldRuncardRangerId != runcardRangerRow.ID.ToString())
                                                {
                                                    //新SFCS_LASER_RANGER记录
                                                    var _insertRecord = insertRecords.Find(t => t.RUNCARD_RANGER_ID == runcardRangerRow.ID);
                                                    if (_insertRecord.IsNullOrEmpty())
                                                    {
                                                        var insertRecord = _mapper.Map<SfcsLaserRangerAddOrModifyModel>(runcardRangerRow);
                                                        insertRecord.SN_BEGIN = runcardRangerRow.SN_BEGIN;
                                                        insertRecord.SN_END = $"{fixHeaderTextEdit}{snRange}{fixTailTextEdit}";//结束流水号
                                                        insertRecord.QUANTITY = 1;
                                                        insertRecord.RUNCARD_RANGER_ID = runcardRangerRow.ID;
                                                        insertRecords.Add(insertRecord);
                                                    }
                                                    else
                                                    {
                                                        //修改新添加的SFCS_LASER_RANGER记录结束流水号
                                                        _insertRecord.SN_END = $"{fixHeaderTextEdit}{snRange}{fixTailTextEdit}";//结束流水号
                                                        _insertRecord.QUANTITY = _insertRecord.QUANTITY + 1;
                                                    }
                                                }
                                                else
                                                {
                                                    hasUpdate = true;
                                                    //修改已有SFCS_LASER_RANGER的结束SN及数量
                                                    updateRecord.SN_END = $"{fixHeaderTextEdit}{snRange}{fixTailTextEdit}";//结束流水号
                                                    updateRecord.QUANTITY = updateRecord.QUANTITY + 1;
                                                }
                                            }
                                            else
                                            {
                                                gotoNext = true;
                                                break;
                                            }

                                            var _snRange = Core.Utilities.RadixConvertPublic.RadixInc(snRange, radixString, 1).PadLeft(rangeLength, '0');
                                            sn = $"{fixHeaderTextEdit}{_snRange}{fixTailTextEdit}";//下一流水号
                                            curQuantity++;
                                        }
                                        if (gotoNext) continue;

                                    }

                                    if (curQuantity >= quantity) break;
                                }


                                if (hasUpdate)
                                {
                                    updateRecords.Add(updateRecord);

                                    var _returnVM = _mapper.Map<SfcsLaserRanger>(updateRecord);
                                    _returnVM.SN_BEGIN = snBegin;
                                    _returnVM.QUANTITY = updateRecord.QUANTITY - laserRangerRow.QUANTITY;
                                    returnVM.Add(_returnVM);
                                }
                                foreach (var insert in insertRecords)
                                {
                                    var _returnVM = _mapper.Map<SfcsLaserRanger>(insert);
                                    returnVM.Add(_returnVM);
                                }

                                //保存流水号范围
                                var add = await _laserRangerRepository.SaveDataByTrans(new SfcsLaserRangerModel()
                                {
                                    InsertRecords = insertRecords,
                                    UpdateRecords = updateRecords
                                });


                                return returnVM;
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                ErrorInfo.Set(ex.Message, MethodBase.GetCurrentMethod(), EnumErrorType.Error);
            }
            return null;
        }

        /// <summary>
        /// 辨析特殊代码
        /// </summary>
        /// <param name="sourceString"></param>
        /// <returns></returns>
        private async Task<string> IdentitySpecialCodes(string sourceString)
        {
            string verifiedString = sourceString;

            // 獲取特殊字符並解析
            Regex regex = new Regex(@"(?<=\<)[^\<^\>]+(?=\>)");
            MatchCollection matchCollection = regex.Matches(sourceString);
            for (int i = 0; i < matchCollection.Count; i++)
            {
                switch (matchCollection[i].Value)
                {
                    case "YYWW":
                        string currentYYWW = await _laserRangerRepository.GetSysdateByFormat("YYWW");
                        verifiedString = verifiedString.Replace("<YYWW>", currentYYWW);
                        break;
                    case "YYIW":
                        string currentYYIW = await _laserRangerRepository.GetSysdateByFormat("IYIW");
                        verifiedString = verifiedString.Replace("<YYIW>", currentYYIW);
                        break;
                    case "IYIW":
                        // Oracle Work Week Format
                        string currentIYIW = await _laserRangerRepository.GetSysdateByFormat("IYIW");
                        verifiedString = verifiedString.Replace("<IYIW>", currentIYIW);
                        break;
                    case "YIW":
                        // Intel Work Week Format
                        /* Ues IYIW and adjust +1 or not manually
                        string currentYIW = DateManager.GetYIW();
                        decimal decimalVal = System.Convert.ToDecimal(currentYIW) + 1;
                        currentYIW = System.Convert.ToString(decimalVal);*/
                        string currentYIW = await _laserRangerRepository.Get_Intel_YIW();
                        verifiedString = verifiedString.Replace("<YIW>", currentYIW);
                        break;
                    case "IW":
                        // Oracle Work Week only Format
                        string currentIW = await _laserRangerRepository.GetSysdateByFormat("IW");
                        verifiedString = verifiedString.Replace("<IW>", currentIW);
                        break;
                    case "Y":
                        string currentY = await _laserRangerRepository.GetSysdateByFormat("Y");
                        verifiedString = verifiedString.Replace("<Y>", currentY);
                        break;
                    case "YY":
                        string currentYY = await _laserRangerRepository.GetSysdateByFormat("YY");
                        verifiedString = verifiedString.Replace("<YY>", currentYY);
                        break;
                    case "YYYY":
                        string currentYYYY = await _laserRangerRepository.GetSysdateByFormat("YYYY");
                        verifiedString = verifiedString.Replace("<YYYY>", currentYYYY);
                        break;
                    case "MM":
                        string currentMM = await _laserRangerRepository.GetSysdateByFormat("MM");
                        verifiedString = verifiedString.Replace("<MM>", currentMM);
                        break;
                    case "DD":
                        string currentDD = await _laserRangerRepository.GetSysdateByFormat("DD");
                        verifiedString = verifiedString.Replace("<DD>", currentDD);
                        break;
                    case "YYYYMMDD":
                        string currentYYYYMMDD = await _laserRangerRepository.GetSysdateByFormat("YYYYMMDD");
                        verifiedString = verifiedString.Replace("<YYYYMMDD>", currentYYYYMMDD);
                        break;
                    case "YYMMDD":
                        string currentYYMMDD = await _laserRangerRepository.GetSysdateByFormat("YYMMDD");
                        verifiedString = verifiedString.Replace("<YYMMDD>", currentYYMMDD);
                        break;
                    case "YYYYMM":
                        string currentYYYYMM = await _laserRangerRepository.GetSysdateByFormat("YYYYMM");
                        verifiedString = verifiedString.Replace("<YYYYMM>", currentYYYYMM);
                        break;
                    case "YYMM":
                        string currentYYMM = await _laserRangerRepository.GetSysdateByFormat("YYMM");
                        verifiedString = verifiedString.Replace("<YYMM>", currentYYMM);
                        break;
                    case "MMDD":
                        string currentMMDD = await _laserRangerRepository.GetSysdateByFormat("MMDD");
                        verifiedString = verifiedString.Replace("<MMDD>", currentMMDD);
                        break;
                    case "WW":
                        string currentWW = await _laserRangerRepository.GetSysdateByFormat("WW");
                        verifiedString = verifiedString.Replace("<WW>", currentWW);
                        break;

                    default:
                        break;
                }
            }

            return verifiedString;
        }

        /// <summary>
        /// 生成流水号（普通流程）
        /// </summary>
        /// <param name="woRuncardRangerRow"></param>
        private async Task<List<NewLaserDataResultItem>> GenerateRangerSN(SfcsLaserRanger woRuncardRangerRow)
        {
            List<NewLaserDataResultItem> result = new List<NewLaserDataResultItem>();
            result.Add(new NewLaserDataResultItem() { NO = 1, UNIT_SN = woRuncardRangerRow.SN_BEGIN });

            SfcsParameters radixRow = await _parameRepository.GetAsync("SELECT * FROM SFCS_PARAMETERS WHERE LOOKUP_TYPE = :LOOKUP_TYPE AND LOOKUP_CODE = :LOOKUP_CODE",
                       new { LOOKUP_TYPE = "RADIX_TYPE", LOOKUP_CODE = woRuncardRangerRow.DIGITAL });
            if (radixRow.IsNullOrWhiteSpace()) return result;
            string standardDigits = radixRow.DESCRIPTION;

            string snBeginRange = woRuncardRangerRow.SN_BEGIN.Substring(
                (int)(woRuncardRangerRow.HEADER_LENGTH), (int)woRuncardRangerRow.RANGE);
            for (int i = 1; i < woRuncardRangerRow.QUANTITY; i++)
            {
                // calculate sn from 2nd to the last
                string snRange = Core.Utilities.RadixConvertPublic.RadixInc(snBeginRange, standardDigits, i).PadLeft(snBeginRange.Length, '0').Trim();
                string fixHeader = "";
                if (!woRuncardRangerRow.FIX_HEADER.IsNullOrEmpty()) fixHeader = woRuncardRangerRow.FIX_HEADER.Trim();
                string fixTail = "";
                if (!woRuncardRangerRow.FIX_TAIL.IsNullOrEmpty()) fixTail = woRuncardRangerRow.FIX_TAIL.Trim();
                string sn = $"{fixHeader}{snRange}{fixTail}";

                // add new sn into list
                result.Add(new NewLaserDataResultItem() { NO = i + 1, UNIT_SN = sn });
            }

            return result;
        }

        /// <summary>
        /// 生成流水号（复杂流程）
        /// </summary>
        private async Task<List<NewLaserDataResultItemDataBase>> GenerateRangerSN2(SfcsLaserRangerRules woRuncardRangerRow, decimal snCount, List<LaserMacPcbNoPCBItem> macList)
        {
            List<NewLaserDataResultItemDataBase> result = new List<NewLaserDataResultItemDataBase>();

            string standardDigits = "";
            if (woRuncardRangerRow.RANGE_LENGTH > 0)//使用流水号
            {
                SfcsParameters radixRow = await _parameRepository.GetAsync("SELECT * FROM SFCS_PARAMETERS WHERE LOOKUP_TYPE = :LOOKUP_TYPE AND LOOKUP_CODE = :LOOKUP_CODE",
                      new { LOOKUP_TYPE = "RADIX_TYPE", LOOKUP_CODE = woRuncardRangerRow.DIGITAL });
                if (radixRow.IsNullOrWhiteSpace()) return result;
                standardDigits = radixRow.DESCRIPTION;
            }

            decimal iSnCount = snCount + 1;
            //获取上一次最后生成的流水号
            string snBeginRange = await _importRuncardSnRepository.GetLastImportRuncardSn(woRuncardRangerRow.WO_NO);
            if (snBeginRange.IsNullOrWhiteSpace())
            {
                snBeginRange = woRuncardRangerRow.RANGE_START_CODE;

                string snRange = snBeginRange;
                string fixHeader = "";
                if (!woRuncardRangerRow.FIX_HEADER.IsNullOrEmpty()) fixHeader = await this.IdentitySpecialCodes(woRuncardRangerRow.FIX_HEADER.Trim());
                string fixTail = "";
                if (!woRuncardRangerRow.FIX_TAIL.IsNullOrEmpty()) fixTail = await this.IdentitySpecialCodes(woRuncardRangerRow.FIX_TAIL.Trim());
                string sn = $"{fixHeader}{snRange}{fixTail}";

                //替换MAC
                string unitMacString = "";
                var unitMac = macList.FirstOrDefault(t => t.NO == 1);
                if (!unitMac.IsNullOrEmpty())
                {
                    unitMacString = unitMac.UNIT_MAC;
                }
                sn = sn.Replace("<UNIT_KEY>", unitMacString);

                result.Add(new NewLaserDataResultItemDataBase() { NO = 1, UNIT_SN = sn, UNIT_SN_RANGE = snRange });
                iSnCount = snCount;
            }

            for (int i = 1; i < iSnCount; i++)
            {
                string snRange = "";
                string fixHeader = "";
                if (!woRuncardRangerRow.FIX_HEADER.IsNullOrEmpty()) fixHeader = await this.IdentitySpecialCodes(woRuncardRangerRow.FIX_HEADER.Trim());
                string fixTail = "";
                if (!woRuncardRangerRow.FIX_TAIL.IsNullOrEmpty()) fixTail = await this.IdentitySpecialCodes(woRuncardRangerRow.FIX_TAIL.Trim());
                string sn = "";

                if (woRuncardRangerRow.RANGE_LENGTH > 0)//使用流水号
                {
                    // calculate sn from 2nd to the last
                    snRange = Core.Utilities.RadixConvertPublic.RadixInc(snBeginRange, standardDigits, i).PadLeft(snBeginRange.Length, '0').Trim();
                    sn = $"{fixHeader}{snRange}{fixTail}";
                }
                else
                {
                    sn = $"{fixHeader}{fixTail}";
                }

                //替换MAC
                string unitMacString = "";
                var unitMac = macList.FirstOrDefault(t => t.NO == result.Count + 1);
                if (!unitMac.IsNullOrEmpty())
                {
                    unitMacString = unitMac.UNIT_MAC;
                    sn = sn.Replace("<UNIT_KEY>", unitMacString);
                }

                // add new sn into list
                result.Add(new NewLaserDataResultItemDataBase() { NO = result.Count + 1, UNIT_SN = sn, UNIT_SN_RANGE = snRange });
            }

            return result;

        }

        #endregion

        #endregion

        /// <summary>
        /// 镭雕完成接口
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [AllowAnonymous]
        public async Task<LaserApiBaseReturn<LaserSnPcbNoResultModel>> UpdateSnPcbNo([FromBody] LaserSnPcbNoRequestModel model)
        {
            LaserApiBaseReturn<LaserSnPcbNoResultModel> returnVM = new LaserApiBaseReturn<LaserSnPcbNoResultModel>();
            returnVM.header = new LaserApiBaseReturnHeader();
            returnVM.body = new LaserSnPcbNoResultModel();

            if (!ErrorInfo.Status)
            {
                try
                {
                    #region 检查参数
                    if (!ErrorInfo.Status)
                    {
                        if (model == null)
                        {
                            //请求实体不能为空
                            ErrorInfo.Set(_localizer["model_notnull"], MethodBase.GetCurrentMethod(), EnumErrorType.Warning);
                        }
                        else if (model.EQP_ID.IsNullOrWhiteSpace())
                        {
                            //请求参数EQP_ID不能为空
                            ErrorInfo.Set(_localizer["eqp_id_notnull"], MethodBase.GetCurrentMethod(), EnumErrorType.Warning);
                        }
                        else if (model.CLIENT_TIME.IsNullOrWhiteSpace())
                        {
                            //请求参数CLIENT_TIME不能为空
                            ErrorInfo.Set(_localizer["client_time_notnull"], MethodBase.GetCurrentMethod(), EnumErrorType.Warning);
                        }
                        //else if (model.UNIT_SN.IsNullOrWhiteSpace())
                        //{
                        //    //请求参数UNIT_SN不能为空
                        //    ErrorInfo.Set(_localizer["unit_sn_notnull"], MethodBase.GetCurrentMethod(), EnumErrorType.Warning);
                        //}
                        else if (model.WORK_ORDER.IsNullOrWhiteSpace())
                        {
                            //请求参数WORK_ORDER不能为空
                            ErrorInfo.Set(_localizer["work_order_notnull"], MethodBase.GetCurrentMethod(), EnumErrorType.Warning);
                        }
                        else if (model.PCB_LIST.IsNullOrEmpty() || model.PCB_LIST.Count <= 0)
                        {
                            //请求参数PCB_LIST不能为空
                            ErrorInfo.Set(_localizer["pcb_list_notnull"], MethodBase.GetCurrentMethod(), EnumErrorType.Warning);
                        }
                        else if (!(model.NEED_RETROACTIVE == 0 || model.NEED_RETROACTIVE == 1))
                        {
                            //请求参数NEED_RETROACTIVE只能为0或1
                            ErrorInfo.Set(_localizer["need_retroactive_err"], MethodBase.GetCurrentMethod(), EnumErrorType.Warning);
                        }
                        else if (!(model.LASER_RESULT == 1 || model.LASER_RESULT == 2))
                        {
                            //请求参数LASER_RESULT只能为1或2
                            ErrorInfo.Set(_localizer["laser_result_err"], MethodBase.GetCurrentMethod(), EnumErrorType.Warning);
                        }
                    }

                    #endregion

                    #region 设置返回值

                    if (!ErrorInfo.Status)
                    {
                        bool isUpdateLastSn = false;//更新最后一次镭雕流水号

                        SfcsWo sfcsWo = await _woRepository.GetAsync("SELECT * FROM SFCS_WO WHERE WO_NO = :WO_NO", new { WO_NO = model.WORK_ORDER });
                        string partNumber = sfcsWo.PART_NO;
                        decimal woID = sfcsWo.ID;

                        var currBeginSN = model.PCB_LIST.OrderBy(t => t.UNIT_SN).FirstOrDefault().UNIT_SN;//开始流水号
                        var currEndSN = model.PCB_LIST.OrderByDescending(t => t.UNIT_SN).FirstOrDefault().UNIT_SN;//结束流水号

                        if (model.LASER_RESULT == 1)//镭雕成功
                        {
                            #region 防呆
                            List<SfcsLaserRanger> laserRangerTable = await _laserRangerRepository.GetLaserRangerBySN(woID, currBeginSN);

                            //获取最后一次镭雕完成的SN
                            SfcsLaserRanger lastLaserRangerRow = laserRangerTable.Where(t => t.LAST_LASER_SN.IsNullOrWhiteSpace() == false).OrderByDescending(t => t.LAST_LASER_SN).FirstOrDefault();
                            if (!lastLaserRangerRow.IsNullOrEmpty())
                            {
                                #region 普通流程
                                string lastLaserSN = lastLaserRangerRow.LAST_LASER_SN;
                                string lastSnEnd = "";//推算上一个结束流水号

                                SfcsParameters radixRow = await _parameRepository.GetAsync("SELECT * FROM SFCS_PARAMETERS WHERE LOOKUP_TYPE = :LOOKUP_TYPE AND LOOKUP_CODE = :LOOKUP_CODE",
                           new { LOOKUP_TYPE = "RADIX_TYPE", LOOKUP_CODE = lastLaserRangerRow.DIGITAL });
                                if (!radixRow.IsNullOrWhiteSpace())
                                {
                                    string radixString = radixRow.DESCRIPTION;

                                    string lastLaserSNRange = "";//最后一次镭雕的SN
                                    string lastSnEndRange = "";//推算的上一镭完的SN

                                    string fixHeader = "";
                                    if (!lastLaserRangerRow.FIX_HEADER.IsNullOrEmpty()) fixHeader = lastLaserRangerRow.FIX_HEADER;
                                    string fixTail = "";
                                    if (!lastLaserRangerRow.FIX_TAIL.IsNullOrEmpty()) fixTail = lastLaserRangerRow.FIX_TAIL;
                                    string snBegin = lastLaserRangerRow.SN_BEGIN;
                                    int rangeLength = lastLaserRangerRow.RANGE.HasValue ? (int)lastLaserRangerRow.RANGE.Value : 0;
                                    int headerLength = snBegin.Length - rangeLength - fixTail.Length;
                                    if (rangeLength > 0)
                                    {
                                        if (rangeLength == lastLaserSN.Length)
                                        {
                                            lastLaserSNRange = lastLaserSN;
                                        }
                                        else
                                        {
                                            lastLaserSNRange = lastLaserSN.Substring(headerLength, rangeLength);
                                        }

                                        string currEndSNRange = "";
                                        if (rangeLength == currEndSN.Length)
                                        {
                                            currEndSNRange = currEndSN;
                                        }
                                        else
                                        {
                                            currEndSNRange = currEndSN.Substring(headerLength, rangeLength);
                                        }

                                        string _lastSnEndRange = "";
                                        if (rangeLength == currBeginSN.Length)
                                        {
                                            _lastSnEndRange = currBeginSN;
                                        }
                                        else
                                        {
                                            _lastSnEndRange = currBeginSN.Substring(headerLength, rangeLength);
                                        }
                                        lastSnEndRange = Core.Utilities.RadixConvertPublic.RadixInc(_lastSnEndRange, radixString, -1).PadLeft(rangeLength, '0');
                                        lastSnEnd = $"{fixHeader}{lastSnEndRange}{fixTail}";//推算上一个结束流水号

                                        var lastSnBeginRange = Core.Utilities.RadixConvertPublic.RadixInc(_lastSnEndRange, radixString, -1 * model.PCB_LIST.Count).PadLeft(rangeLength, '0');//推算上一个开始流水号

                                        if (lastSnEnd != lastLaserSN)//推算的结束流水号 ≠ 上一实际结束流水号
                                        {
                                            if (string.Compare(lastLaserSNRange, lastSnEndRange) <= 0)
                                            {
                                                /*
                                                 * 1-4镭完：此时记录结束SN是4
                                                 * 5-8失败（断电）
                                                 * 9-12镭完：此时要检查上次的结束SN应是8，却此时是4=>【记录异常5-8】=>更新结束SN是12
                                                 */
                                                isUpdateLastSn = true;

                                                var exlog = await _laserExceptionLogRepository.GetAsync("SELECT * FROM SFCS_LASER_EXCEPTION_LOG WHERE SN_BEGIN = :SN_BEGIN AND SN_END = :SN_END AND IS_INVALID = 'N'",
                                                    new
                                                    {
                                                        SN_BEGIN = $"{fixHeader}{lastSnBeginRange}{fixTail}",
                                                        SN_END = lastSnEnd
                                                    });
                                                if (exlog.IsNullOrEmpty())
                                                {
                                                    //记录镭雕异常日志
                                                    SfcsLaserExceptionLogModel logInsertRecords = new SfcsLaserExceptionLogModel();
                                                    logInsertRecords.InsertRecords = new List<SfcsLaserExceptionLogAddOrModifyModel>();
                                                    var log = new SfcsLaserExceptionLogAddOrModifyModel();
                                                    log.LASER_RANGER_ID = lastLaserRangerRow.ID;
                                                    log.SN_BEGIN = $"{fixHeader}{lastSnBeginRange}{fixTail}";
                                                    log.SN_END = lastSnEnd;
                                                    log.IS_INVALID = GlobalVariables.EnableN;
                                                    logInsertRecords.InsertRecords.Add(log);
                                                    await _laserExceptionLogRepository.SaveDataByTrans(logInsertRecords);
                                                }
                                            }
                                            else if (string.Compare(lastLaserSNRange, currEndSNRange) > 0)
                                            {
                                                /*
                                                 * 1-4镭完：此时记录结束SN是4
                                                 * 5-8未镭完
                                                 * 9-12镭完：此时要检查上次的结束SN应是8，却此时是4=>记录异常5-8=>更新结束SN是12
                                                 * 5-8才镭完：此时发现上次的结束SN是12大于当前结束SN是8=>【把异常5-8设为失效】
                                                 */
                                                var log = await _laserExceptionLogRepository.GetAsync("SELECT * FROM SFCS_LASER_EXCEPTION_LOG WHERE SN_BEGIN = :SN_BEGIN AND SN_END = :SN_END AND IS_INVALID = 'N'",
                                                    new
                                                    {
                                                        SN_BEGIN = currBeginSN,
                                                        SN_END = currEndSN
                                                    });
                                                if (!log.IsNullOrEmpty())
                                                {
                                                    log.IS_INVALID = GlobalVariables.EnableY;
                                                    log.INVALID_TIME = DateTime.Now;
                                                    await _laserExceptionLogRepository.UpdateAsync(log);
                                                }
                                            }
                                        }
                                    }

                                }
                                #endregion
                            }
                            else
                            {
                                var woRangerRule = await _laserRangerRulesRepository.GetAsync("SELECT * FROM SFCS_LASER_RANGER_RULES WHERE WO_NO = :WO_NO AND ENABLED = :ENABLED",
                                    new { WO_NO = sfcsWo.WO_NO, ENABLED = GlobalVariables.EnableY });
                                if (!woRangerRule.IsNullOrEmpty())
                                {
                                    #region 复杂流程
                                    //记录客户SN为有效
                                    ImportRuncardSnModel saveModel = new ImportRuncardSnModel();
                                    saveModel.UpdateRecords = new List<ImportRuncardSnAddOrModifyModel>();

                                    foreach (var item in model.PCB_LIST)
                                    {
                                        var importRuncardSn = await _importRuncardSnRepository.GetAsync("SELECT * FROM IMPORT_RUNCARD_SN WHERE WO_NO = :WO_NO AND SN = :SN AND ENABLE = :ENABLE",
                                            new { WO_NO = sfcsWo.WO_NO, SN = item.UNIT_SN, ENABLE = GlobalVariables.EnableN });
                                        if (!importRuncardSn.IsNullOrEmpty())
                                        {
                                            var updateRecord = _mapper.Map<ImportRuncardSnAddOrModifyModel>(importRuncardSn);
                                            updateRecord.ENABLE = GlobalVariables.EnableY;
                                            updateRecord.UPDATE_BY = model.EQP_ID;
                                            saveModel.UpdateRecords.Add(updateRecord);
                                        }
                                    }
                                    if (saveModel.UpdateRecords.Count > 0) await _importRuncardSnRepository.SaveDataByTrans(saveModel);

                                    //处理异常日志
                                    var log = await _laserExceptionLogRepository.GetAsync("SELECT * FROM SFCS_LASER_EXCEPTION_LOG WHERE SN_BEGIN = :SN_BEGIN AND SN_END = :SN_END AND IS_INVALID = 'N'",
                                                    new
                                                    {
                                                        SN_BEGIN = currBeginSN,
                                                        SN_END = currEndSN
                                                    });
                                    if (!log.IsNullOrEmpty())
                                    {
                                        log.IS_INVALID = GlobalVariables.EnableY;
                                        log.INVALID_TIME = DateTime.Now;
                                        await _laserExceptionLogRepository.UpdateAsync(log);
                                    }

                                    #endregion
                                }
                                else
                                {
                                    isUpdateLastSn = true;
                                }
                            }

                            #endregion

                            #region 更新最后一次镭雕完成的SN
                            if (isUpdateLastSn)
                            {
                                //记录最后一次镭雕完成的SN
                                string sn = model.PCB_LIST.OrderByDescending(t => t.UNIT_SN).FirstOrDefault().UNIT_SN;//本次结束流水号
                                foreach (var laserRangerRow in laserRangerTable)
                                {
                                    string snRange = "";
                                    string fixHeader = "";
                                    if (!laserRangerRow.FIX_HEADER.IsNullOrEmpty()) fixHeader = laserRangerRow.FIX_HEADER;
                                    string fixTail = "";
                                    if (!laserRangerRow.FIX_TAIL.IsNullOrEmpty()) fixTail = laserRangerRow.FIX_TAIL;
                                    string snBegin = laserRangerRow.SN_BEGIN;
                                    int rangeLength = laserRangerRow.RANGE.HasValue ? (int)laserRangerRow.RANGE.Value : 0;
                                    int headerLength = snBegin.Length - rangeLength - fixTail.Length;
                                    if (rangeLength > 0)
                                    {
                                        if (rangeLength == sn.Length)
                                        {
                                            snRange = sn;
                                        }
                                        else
                                        {
                                            snRange = sn.Substring(headerLength, rangeLength);
                                        }

                                        string runcardRangeRow_SnBeginRange = laserRangerRow.SN_BEGIN.Substring(fixHeader.Length, rangeLength);//范围开始流水号
                                        string runcardRangeRow_SnEndRange = laserRangerRow.SN_END.Substring(fixHeader.Length, rangeLength);//范围结束流水号

                                        if (string.Compare(runcardRangeRow_SnBeginRange, snRange) <= 0
                                            && string.Compare(snRange, runcardRangeRow_SnEndRange) < 0)//如果流水号在本范围内
                                        {
                                            await _laserRangerRepository.UpdateLastLaserSN(laserRangerRow.ID, sn);
                                        }
                                        else if (string.Compare(runcardRangeRow_SnEndRange, snRange) <= 0)//如果流水号超过本范围结束流水号
                                        {
                                            //设置本范围最后一次镭雕完成的SN为本范围结束流水号
                                            await _laserRangerRepository.UpdateLastLaserSN(laserRangerRow.ID, laserRangerRow.SN_END);
                                        }

                                    }

                                }
                            }
                            #endregion

                            #region 写入SFCS_LASER_RECORD
                            if (!ErrorInfo.Status)
                            {
                                SfcsLaserRecordModel saveLaserRecodeModel = new SfcsLaserRecordModel();
                                saveLaserRecodeModel.InsertRecords = new List<SfcsLaserRecordAddOrModifyModel>();

                                foreach (var item in model.PCB_LIST.OrderBy(t => t.NO))
                                {
                                    SfcsLaserRecordAddOrModifyModel insertRecord = new SfcsLaserRecordAddOrModifyModel();
                                    insertRecord.MACHINE_NO = model.EQP_ID;
                                    insertRecord.LASER_TIME = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff");
                                    insertRecord.LOT_NO = "";
                                    insertRecord.SN = item.UNIT_SN;
                                    insertRecord.CREATE_USER = "镭雕机";
                                    insertRecord.MULTI_NO = model.PCB_LIST.Count;
                                    insertRecord.IS_INPUT = GlobalVariables.EnableN;
                                    insertRecord.IS_INVALID = GlobalVariables.EnableN;
                                    saveLaserRecodeModel.InsertRecords.Add(insertRecord);
                                }

                                var saveRecord = await _laserRecordRepository.SaveDataByTrans(saveLaserRecodeModel);

                            }
                            #endregion

                            #region SMT_MULTIPANEL_HEADER、SMT_MULTIPANEL_DETAIL
                            if (!ErrorInfo.Status)
                            {
                                if (model.NEED_RETROACTIVE == 0)//不需要追溯：则写入SMT_MULTIPANEL_HEADER、SMT_MULTIPANEL_DETAIL
                                {
                                    SmtMultipanelMstModel addMulMst = new SmtMultipanelMstModel();
                                    addMulMst.MainData = new SmtMultipanelHeaderAddOrModifyModel();
                                    addMulMst.MainData.BATCH_NO = DateTime.Now.ToString("yyyyMMddHHmmss");
                                    addMulMst.MainData.MULT_SITE_ID = "0";
                                    addMulMst.MainData.MULT_SITE_NAME = "镭雕机";
                                    addMulMst.MainData.MULT_OPERATOR = "镭雕机";
                                    addMulMst.MainData.IS_SPLIT = GlobalVariables.EnableN;
                                    addMulMst.MainData.MULT_NUMBER = model.PCB_LIST.Count;
                                    addMulMst.MainData.WO_NO = sfcsWo.WO_NO;

                                    addMulMst.InsertRecords = new List<SmtMultipanelDetailAddOrModifyModel>();
                                    foreach (var item in model.PCB_LIST)
                                    {
                                        SmtMultipanelDetailAddOrModifyModel insertRecord = new SmtMultipanelDetailAddOrModifyModel();
                                        insertRecord.SN = item.UNIT_SN;
                                        addMulMst.InsertRecords.Add(insertRecord);
                                    }

                                    var saveMultipanel = await _multipanelHeaderRepository.SaveDataByTrans(addMulMst);
                                }
                            }

                            #endregion
                        }
                        else//镭雕失败
                        {
                            #region 记录镭雕异常日志
                            SfcsLaserExceptionLogModel logInsertRecords = new SfcsLaserExceptionLogModel();
                            logInsertRecords.InsertRecords = new List<SfcsLaserExceptionLogAddOrModifyModel>();
                            var log = new SfcsLaserExceptionLogAddOrModifyModel();

                            var runcardRangerRow = (await _laserRangerRepository.GetListAsync(@"
				             WHERE ((:SN_BEGIN BETWEEN SN_BEGIN AND SN_END) OR (:SN_END BETWEEN SN_BEGIN AND SN_END))
                            AND (LENGTH(:SN_BEGIN) = LENGTH(SN_END)) 
                            AND (FIX_HEADER = SUBSTR(:SN_BEGIN,1,HEADER_LENGTH) OR FIX_HEADER IS NULL)
                            AND (FIX_TAIL = SUBSTR(:SN_BEGIN, LENGTH(:SN_BEGIN)-TAIL_LENGTH+1, TAIL_LENGTH) OR FIX_TAIL IS NULL)",
                            new { SN_BEGIN = currBeginSN, SN_END = currEndSN })).FirstOrDefault();
                            if (!runcardRangerRow.IsNullOrEmpty())
                            {
                                log.LASER_RANGER_ID = runcardRangerRow.ID;
                            }
                            else
                            {
                                log.LASER_RANGER_ID = 0;
                            }

                            log.SN_BEGIN = currBeginSN;
                            log.SN_END = currEndSN;
                            log.IS_INVALID = GlobalVariables.EnableN;
                            log.ATTRIBUTE1 = model.LASER_EXCEPTION;
                            logInsertRecords.InsertRecords.Add(log);
                            await _laserExceptionLogRepository.SaveDataByTrans(logInsertRecords);
                            #endregion
                        }

                        if (!ErrorInfo.Status)
                        {
                            returnVM.body.SERVER_TIME = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff");
                            returnVM.header.code = "200";
                            returnVM.header.desc = "";
                        }
                    }

                    #endregion

                }
                catch (Exception ex)
                {
                    ErrorInfo.Set(ex.Message, MethodBase.GetCurrentMethod(), EnumErrorType.Error);
                }
            }

            #region 如果出现错误，则写错误日志并返回错误内容

            if (ErrorInfo.Status)
            {
                returnVM.header.code = "101";
                returnVM.header.desc = ErrorInfo.Message;
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
        /// 镭雕机下载数据
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [AllowAnonymous]
        public async Task<LaserTaskDataListResponseModel> GetLaserDataEx([FromQuery] GetLaserDataModel model)
        {
            ApiBaseReturn<LaserTaskDataListResponseModel> returnVM = new ApiBaseReturn<LaserTaskDataListResponseModel>();
            SfcsWo sfcsWo = null;
            string errorMsg = String.Empty;
            SfcsParameters sfcsParameter = null;
            LaserTaskDataListResponseModel listModel = new LaserTaskDataListResponseModel();
            if (!ErrorInfo.Status)
            {
                try
                {
                    #region 参数验证
                    if (!ErrorInfo.Status)
                    {
                        if (model.WORK_ORDER.IsNullOrEmpty())
                        {
                            //请录入工单号。
                            errorMsg = _localizer["err_wo_orderno"];
                            ErrorInfo.Set(errorMsg, MethodBase.GetCurrentMethod(), EnumErrorType.Warning);
                        }
                        else
                        {
                            string conditions = "WHERE WO_NO =:WO_NO";
                            sfcsWo = await _woRepository.GetAsyncEx<SfcsWo>(conditions, new { WO_NO = model.WORK_ORDER });
                            if (sfcsWo == null)
                            {
                                //工单{0}不存在，请确认。
                                errorMsg = string.Format(_localizer["err_wo_not_found"], model.WORK_ORDER);
                                ErrorInfo.Set(errorMsg, MethodBase.GetCurrentMethod(), EnumErrorType.Warning);
                            }
                        }
                    }

                    if (!ErrorInfo.Status)
                    {
                        if (model.MACHINE_NO.IsNullOrEmpty())
                        {
                            errorMsg = _localizer["err_machine_no_not_found"];
                            ErrorInfo.Set(errorMsg, MethodBase.GetCurrentMethod(), EnumErrorType.Error);
                        }
                        else
                        {
                            string conditions = " WHERE LOOKUP_TYPE='PRINT_MACHINE_CODE' AND ENABLED='Y'AND MEANING=:MEANING ";
                            sfcsParameter = await _woRepository.GetAsyncEx<SfcsParameters>(conditions, new { MEANING = model.MACHINE_NO });

                            if (sfcsParameter == null)
                            {
                                //设备编号不存在，请确认。
                                errorMsg = string.Format(_localizer["err_machine_no_not_found"]);
                                ErrorInfo.Set(errorMsg, MethodBase.GetCurrentMethod(), EnumErrorType.Warning);
                            }
                        }
                    }

                    if (!ErrorInfo.Status && model.SN_COUNT <= 0)
                    {
                        //获取SN数量有误，请注意检查!
                        errorMsg = _localizer["err_sn_count_message"];
                        ErrorInfo.Set(errorMsg, MethodBase.GetCurrentMethod(), EnumErrorType.Error);
                    }
                    #endregion

                    #region 设置返回值

                    if (!ErrorInfo.Status)
                    {
                        //获取镭雕任务数
                        var CurrentLaserTask = (await _laserTaskRepository.GetListByTableEX<SfcsLaserTask>("*", "SFCS_LASER_TASK", " AND PRINT_STATUS!=3 AND MACHINE_CODE=:MACHINE_CODE AND WO_NO=:WO_NO AND ENABLED='Y'  ORDER BY ID ASC ", new
                        {
                            MACHINE_CODE = sfcsParameter.MEANING,
                            WO_NO = model.WORK_ORDER
                        }))?.ToList();


                        //按请求的SN数量获取镭雕数据
                        //已获取到的SN数量
                        int snCount = 0;
                        if (CurrentLaserTask == null || CurrentLaserTask.Count <= 0)
                        {
                            //获取任务数据失败！
                            throw new Exception(_localizer["err_get_laserdata"]);
                        }

                        LaserTaskListModel dataModel = new LaserTaskListModel();
                        for (int i = 0; i < CurrentLaserTask.Count; i++)
                        {
                            //还需要添加的sn数量
                            int getSNCount = 0;
                            if (snCount == model.SN_COUNT) { break; } else { getSNCount = model.SN_COUNT - snCount; }

                            //镭雕打印任务id
                            String task_id = CurrentLaserTask[i].ID.ToString().Trim();
                            int type_id = Convert.ToInt32(CurrentLaserTask[i].TYPE_ID);

                            //任务类型（1：工单流水号范围;2:导入SN;）
                            switch (CurrentLaserTask[i].TASK_TYPE.ToString().Trim())
                            {
                                case "1":
                                    dataModel = await GetSNListByRuncardRanger(CurrentLaserTask[i], getSNCount, snCount, task_id);
                                    break;
                                case "2":
                                    dataModel = await GetSNListByImportRuncard(type_id, getSNCount, snCount, task_id);
                                    break;
                                default:
                                    break;
                            }
                            if (dataModel != null)
                            {
                                snCount += dataModel.PCB_LIST.Count();
                                foreach (SNListModel snListMdoel in dataModel.PCB_LIST)
                                {
                                    listModel.DATA.PCB_LIST.Add(snListMdoel);
                                }
                            }
                        }
                        if (listModel.DATA.PCB_LIST.Count() < 1)
                        {
                            //获取SN数据失败！
                            throw new Exception(_localizer["err_get_data_failed"]);
                        }

                        listModel.CODE = GlobalVariables.CODE_PASS;
                    }

                    #endregion
                }
                catch (Exception ex)
                {
                    listModel.CODE = GlobalVariables.CODE_FAIL;
                    errorMsg = ex.Message;

                    ErrorInfo.Set(ex.Message, MethodBase.GetCurrentMethod(), EnumErrorType.Error);
                }
            }

            #region 如果出现错误，则写错误日志并返回错误内容

            WriteLog(ref returnVM);

            #endregion
            listModel.MSG = errorMsg;
            return listModel;
        }

        /// <summary>
        /// 镭雕结束数据上传
        /// LASER_RESULT:镭雕结果成功或失败：1表示镭雕成功，2表示镭雕失败
        /// STATUS:小板镭雕状态码1:成功， 0：失败
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        [AllowAnonymous]
        public async Task<LaserTaskResultListResponseModelEx> UpdateSnPcbNoEx(LaserTaskResultRequestModel model)
        {
            ApiBaseReturn<LaserTaskResultListResponseModelEx> returnVM = new ApiBaseReturn<LaserTaskResultListResponseModelEx>();
            LaserTaskResultListResponseModelEx resultList = new LaserTaskResultListResponseModelEx();
            try
            {
                #region 参数验证
                //1表示镭雕成功，2表示镭雕失败
                //if (!ErrorInfo.Status && !model.LASER_RESULT.Equals(1))
                //{
                //     resultList.MSG = model.LASER_EXCEPTION;
                //    return resultList;
                //}

                if (!ErrorInfo.Status && model.PCB_LIST.IsNullOrWhiteSpace() || model.PCB_LIST.Count <= 0)
                {
                    //镭雕机上传的镭雕数据不能为空
                    resultList.MSG = _localizer["err_no_data"];
                    ErrorInfo.Set(resultList.MSG, MethodBase.GetCurrentMethod(), EnumErrorType.Warning);
                }
                if (!ErrorInfo.Status && model.WORK_ORDER.IsNullOrWhiteSpace())
                {
                    //工单号不能为空
                    resultList.MSG = _localizer["err_no_wo_data"];
                    ErrorInfo.Set(resultList.MSG, MethodBase.GetCurrentMethod(), EnumErrorType.Warning);
                }
                #endregion
                #region 处理业务
                if (!ErrorInfo.Status)
                {
                    resultList = await _laserTaskRepository.UpdateSnPcbNo(model);
                    if (resultList.CODE==0)//失败
                    {
                        resultList.MSG = _localizer[resultList.MSG];
                        ErrorInfo.Set(resultList.MSG, MethodBase.GetCurrentMethod(), EnumErrorType.Warning);
                    }
                } 
                #endregion

            }
            catch (Exception ex)
            {
                resultList.CODE = GlobalVariables.CODE_FAIL;
                resultList.MSG =ex.Message;

                ErrorInfo.Set(ex.Message, MethodBase.GetCurrentMethod(), EnumErrorType.Error);
            }

            #region 如果出现错误，则写错误日志并返回错误内容

            WriteLog(ref returnVM);

            #endregion

            return resultList;
        }

        #region 内部方法

        /// <summary>
        /// 工单流水号范围取SN
        /// </summary>
        /// <param name="laserTaskModel">任务实体</param>
        /// <param name="getSNCount">需要的数量</param>
        /// <param name="sn_count">已获取的数量</param>
        /// <param name="task_id">任务ID</param>
        /// <returns></returns>
        private async Task<LaserTaskListModel> GetSNListByRuncardRanger(SfcsLaserTask laserTaskModel, int getSNCount, int sn_count, string task_id)
        {
            int count = sn_count;
            LaserTaskListModel mdoel = new LaserTaskListModel();
            int print_qty = Convert.ToInt32(laserTaskModel.PRINT_QTY);
            int type_id = Convert.ToInt32(laserTaskModel.TYPE_ID);

            var dtRuncardRanger = (await _laserExceptionLogRepository.GetListByTableEX<SfcsRuncardRanger>("*", "SFCS_RUNCARD_RANGER", " AND ID = :ID ", new { ID = type_id }))?.FirstOrDefault();
            if (dtRuncardRanger == null) { return mdoel = null; }
            List<String> rangerSNList = await GenerateRangerSN(dtRuncardRanger);
            if (print_qty == 0)
            {
                foreach (String SN in rangerSNList)
                {
                    SNListModel PCBList = new SNListModel();
                    PCBList.NO = ++count;
                    if (PCBList.NO > getSNCount) { break; }
                    PCBList.UNIT_SN = SN;
                    PCBList.TASKID = task_id;
                    mdoel.PCB_LIST.Add(PCBList);
                }
            }
            else
            {

                var dtSN = (await _laserExceptionLogRepository.GetListByTableEX<SmtMultipanelDetail>("SN", "SMT_MULTIPANEL_DETAIL", " AND TASK_ID= :TASK_ID AND TASK_STATUS= '1' ", new { TASK_ID = task_id }))?.ToList();

                //已经镭雕打印成功的SN
                List<String> snList = new List<String>();
                if (!dtSN.IsNullOrEmpty())
                {
                    foreach (var item in dtSN)
                    {
                        snList.Add(item.SN.ToString());
                    }
                }
                foreach (String SN in rangerSNList)
                {
                    if (mdoel.PCB_LIST.Count >= getSNCount) { break; }
                    if (!snList.Contains(SN))
                    {
                        SNListModel PCBList = new SNListModel();
                        PCBList.NO = ++count;
                        PCBList.UNIT_SN = SN;
                        PCBList.TASKID = task_id;
                        mdoel.PCB_LIST.Add(PCBList);
                    }
                }
            }
            return mdoel;
        }

        /// <summary>
		/// 计算出流水号范围中的流水号信息
		/// </summary>
		/// <param name="sfcsRuncardRanger"></param>
		/// <returns></returns>
		private async Task<List<string>> GenerateRangerSN(SfcsRuncardRanger sfcsRuncardRanger)
        {
            List<string> SerialNumberList = new List<string>();
            var sfcsParameterslist = await _laserExceptionLogRepository.GetListByTableEX<SfcsParameters>("*", "SFCS_PARAMETERS ", " AND LOOKUP_TYPE = :LOOKUP_TYPE AND LOOKUP_CODE = :LOOKUP_CODE",
                 new
                 {
                     LOOKUP_TYPE = "RADIX_TYPE",
                     LOOKUP_CODE = sfcsRuncardRanger.DIGITAL
                 });
            if (sfcsParameterslist == null || sfcsParameterslist.Count() <= 0)
            {
                return null;
            }
            string standardDigits = sfcsParameterslist.FirstOrDefault().DESCRIPTION;
            SerialNumberList.Add(sfcsRuncardRanger.SN_BEGIN);
            string snBeginRange = sfcsRuncardRanger.SN_BEGIN.Substring(
                (int)(sfcsRuncardRanger.HEADER_LENGTH), (int)sfcsRuncardRanger.RANGE);
            for (int i = 1; i < sfcsRuncardRanger.QUANTITY; i++)
            {
                // calculate sn from 2nd to the last
                string snRange = RadixConvertPublic.RadixInc(snBeginRange, standardDigits, i).PadLeft(snBeginRange.Length, '0').Trim();
                string sn = (sfcsRuncardRanger.FIX_HEADER.IsNullOrEmpty() ? "" : sfcsRuncardRanger.FIX_HEADER.ToString().Trim()) +
                    snRange + (sfcsRuncardRanger.FIX_TAIL.IsNullOrEmpty() ? "" : sfcsRuncardRanger.FIX_TAIL.ToString().Trim());

                // add new sn into list
                SerialNumberList.Add(sn);
            }
            return SerialNumberList;
        }

        /// <summary>
        /// 取导入SN
        /// </summary>
        /// <param name="id">导入SN的主表id</param>
        /// <param name="getSNCount">需要SN的数量</param>
        /// <param name="sn_count">已获取的数量</param>
        /// <returns></returns>
        private async Task<LaserTaskListModel> GetSNListByImportRuncard(int id, int getSNCount, int sn_count, String task_id)
        {
            int count = sn_count;
            LaserTaskListModel mdoel = new LaserTaskListModel();

            var dtHeader = (await _laserExceptionLogRepository.GetListByTableEX<ImportRuncardHeader>("*", "IMPORT_RUNCARD_HEADER", " AND ID = :ID ", new { ID = id }))?.ToList();
            if (dtHeader == null || dtHeader.Count < 1) { mdoel = null; return mdoel; }
            var dtRuncardSn = (await _laserExceptionLogRepository.GetListByTableEX<ImportRuncardSn>("*", "IMPORT_RUNCARD_SN", " AND  HEADER_ID = :HEADER_ID ", new { HEADER_ID = id }))?.ToList();
            if (dtRuncardSn == null || dtRuncardSn.Count < 1) { mdoel = null; return mdoel; }

            var dtSN = (await _laserExceptionLogRepository.GetListByTableEX<SmtMultipanelDetail>("*", "SMT_MULTIPANEL_DETAIL", " AND  MD.TASK_ID= :TASK_ID AND MD.TASK_STATUS= '1'  ", new { TASK_ID = task_id }))?.ToList();
            // List<String> snList = DataTableToModelList<String>(dtSN);//已经镭雕打印成功的SN
            List<String> snList = new List<String>();
            if (!dtSN.IsNullOrEmpty())
            {
                foreach (var item in dtSN)
                {
                    snList.Add(item.SN.ToString());
                }
            }
            for (int i = 0; i < dtRuncardSn.Count; i++)
            {

                if (mdoel.PCB_LIST.Count >= getSNCount) { break; }

                if (!snList.Contains(dtRuncardSn[i].SN.ToString()))
                {
                    SNListModel PCBList = new SNListModel();
                    PCBList.NO = ++count;
                    PCBList.UNIT_SN = dtRuncardSn[i].SN.ToString();
                    PCBList.TASKID = task_id;
                    mdoel.PCB_LIST.Add(PCBList);
                }
            }
            return mdoel;
        }
        #endregion
    }
}