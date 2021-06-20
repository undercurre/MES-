/*
*┌──────────────────────────────────────────────────────────────┐
*│　描    述： 控制器                                                    
*│　作    者：嘉志科技                                              
*│　版    本：3.0   模板代码自动生成                                              
*│　创建时间：2020-06-20 10:43:03                           
*└──────────────────────────────────────────────────────────────┘
*┌──────────────────────────────────────────────────────────────┐
*│　命名空间： JZ.IMS.WebApi.Controllers                                   
*│　接口名称： ISmtMsdRuncardController                                      
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
using JZ.IMS.WebApi.Common;
using JZ.IMS.WebApi.BomVsPlacement;
using JZ.IMS.ViewModels.SmtMSD;
using JZ.IMS.Models.SmtMSD;

namespace JZ.IMS.WebApi.Controllers
{
    /// <summary>
    /// MSD管控作业
    /// </summary>
	[Route("api/[controller]/[action]")]
    [ApiController]
    public class SmtMsdRuncardController : BaseController
    {
        private readonly ISmtMsdRuncardRepository _repository;
        private readonly IMapper _mapper;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IStringLocalizer<SmtMsdRuncardController> _localizer;
        private readonly SmtMsdRuncardService _service;
        private List<SmtMsdRuncard> msdRuncardTable;
        private List<dynamic> msdR12Table;
        private dynamic reelInfoViewRow;
        private string reelCode;
        private string reelPartNO;
        private string reelMakerCode;
        private decimal? partThickness;
        private string partLevelCode;


        public SmtMsdRuncardController(ISmtMsdRuncardRepository repository, IMapper mapper, IHttpContextAccessor httpContextAccessor,
            IStringLocalizer<SmtMsdRuncardController> localizer)
        {
            _repository = repository;
            _mapper = mapper;
            _httpContextAccessor = httpContextAccessor;
            _localizer = localizer;
            _service = new SmtMsdRuncardService(_repository, _localizer);
        }

        public class IndexVM
        {
            /// <summary>
            /// 当前操作和执行动作  工序 --显示Msg_CN_DESC和Msg_MSD_Action 传code
            /// </summary>
            public List<dynamic> MSDAction { get; set; }

            /// <summary>
            /// 当前区域  --显示Msg_EN_DESC 传Msg_MSD_Area 
            /// </summary>
            public List<dynamic> MSDArea { get; set; }

            /// <summary>
            /// 作业区域 传CODE
            /// </summary>
            public List<object> MSDLOCAL { get; set; }

        }

        /// <summary>
        /// 首页视图 
        /// 烘烤列表使用烘烤标准维护的LoadData方法
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
                        returnVM.Result = new IndexVM()
                        {
                            MSDAction = await _repository.GetListByTable("CODE,CN_DESC MSG_CN_DESC,VALUE MSG_MSD_ACTION", "SMT_LOOKUP", " And TYPE ='MSD_ACTION'  ORDER BY TYPE"),
                            MSDArea = await _repository.GetListByTable("VALUE Msg_MSD_Area,EN_DESC Msg_EN_DESC", "SMT_LOOKUP", " And TYPE ='MSD_AREA'  AND ENABLED ='Y'"),
                            MSDLOCAL = await _repository.GetListByTable(" CODE AS ID,VALUE CODE,CN_DESC DESCRIPTION ", "SMT_LOOKUP", "AND TYPE = 'MSD_LOC' AND ENABLED ='Y'ORDER BY TYPE ")
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
        /// 查询数据
        /// 搜索按钮对应的处理也是这个方法
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>	
        [HttpGet]
        [Authorize]
        public async Task<ApiBaseReturn<List<SmtMsdRuncardListModel>>> LoadData([FromQuery]SmtMsdRuncardRequestModel model)
        {
            ApiBaseReturn<List<SmtMsdRuncardListModel>> returnVM = new ApiBaseReturn<List<SmtMsdRuncardListModel>>();
            if (!ErrorInfo.Status)
            {
                try
                {
                    #region 设置返回值

                    int count = 0;
                    string conditions = " WHERE ID > 0 ";
                    if (!model.Key.IsNullOrWhiteSpace())
                    {
                        //conditions += $"and (instr(User_Name, :Key) > 0 or instr(Nick_Name, :Key) > 0)";
                    }
                    var list = (await _repository.GetListPagedAsync(model.Page, model.Limit, conditions, "Id desc", model)).ToList();
                    var viewList = new List<SmtMsdRuncardListModel>();
                    list?.ForEach(x =>
                    {
                        var item = _mapper.Map<SmtMsdRuncardListModel>(x);
                        //item.ENABLED = (item.ENABLED == "Y");
                        viewList.Add(item);
                    });

                    count = await _repository.RecordCountAsync(conditions, model);

                    returnVM.Result = viewList;
                    returnVM.TotalCount = count;

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
        /// 輸入料卷编码 测试数据 M200516000043
        /// 获取成功就 料卷不能修改(只读),作业区和执行动作 确认按钮可以操作
        /// BCD_ID 条码ID
        /// BCD_KIT BCD_KIT
        /// BU 库别
        /// CUSTOMER_CODE 客户代码
        /// CUSTOMER_NAME 客户名称
        /// LOT_CODE 生产批号
        /// MAKER_CODE 制造商代码
        /// MAKER_NAME 制造商名称
        /// DATE_CODE 制造日期
        /// ORIGINAL_QUANTITY 原始数量
        /// ORIGINAL_REEL_CODE 原始条码
        /// PART_DESC 料号描述
        /// PART_NO 料号
        /// QVL_DESC QVL描述
        /// QVL_NO 
        /// REEL_STATUS 状态
        /// REVISION 版本
        /// SIC_CODE  库别编码
        /// VENDOR_CODE 供应商代码
        /// VENDOR_NAME 供应商名称
        /// BeginTime 開始時間
        /// CurrentAction 當前動作
        /// FloorLife 
        /// PartLevelCode 元件等級
        /// PartThickness 元件厚度
        /// PassedTime 當前動作經歷時間
        /// RatedControlTime 額定管控時間(Bake)
        /// ReelCode 元件料號
        /// ReelPartNO 元件料号
        /// TotalOpenTimeAfterBake 暴露時間
        /// </summary>
        /// <param name="ReelCode"></param>
        /// <returns></returns>
        [HttpGet]
        [Authorize]
        public async Task<ApiBaseReturn<dynamic>> GetMSDInfo([FromQuery]string ReelCode)
        {
            ApiBaseReturn<dynamic> returnVM = new ApiBaseReturn<dynamic>();
            if (!ErrorInfo.Status)
            {
                try
                {
                    #region 参数验证
                    if (ReelCode.IsNullOrEmpty())
                    {
                        ErrorInfo.Set("Parameter Cannot Be Empty", MethodBase.GetCurrentMethod(), EnumErrorType.Warning);
                    }
                    #endregion

                    #region 设置返回值

                    if (!ErrorInfo.Status)
                    {

                        // Reel去掩碼
                        ReelCode = BarcoderFilter.FormatBarcode(ReelCode);

                        //检查是否为MSD管控物料
                        if (!(await _repository.IsMsdReel(ReelCode)))
                        {
                            //该物料为非MSD管控物料！
                            ErrorInfo.Set(_localizer["MSD_NOT_ERROR"], MethodBase.GetCurrentMethod(), EnumErrorType.Warning);
                        }

                        #region IdentifyReel
                        if (!ErrorInfo.Status)
                        {

                            if (!(await _service.IdentifyReel(ReelCode)))
                            {
                                throw new Exception();
                            }
                        }
                        #endregion

                        #region 取得元件資訊 GetMSDInfo

                        if (!ErrorInfo.Status)
                        {
                            var viewModel = new MsdInfoProviderView();
                            viewModel.ReelCode = ReelCode;
                            var reelInfo = _service.GetMSDInfo(viewModel).Result;

                            returnVM.Result = new List<dynamic>() { reelInfo, viewModel };
                        }

                        #endregion

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
        /// 執行動作
        /// 没有报错会返回true
        /// 传如下参数
        /// ActionArea(作业区域传ID) 
        /// ActionAreaType(作业区域传CODE)
        /// NewAction(执行动作CODE)
        /// MSDBakeRuleID(烘烤标准ID)如果选择了烧烤的时候，下面的列表要选择
        /// ReelCode(料卷编号)
        /// UserName(用户名字)
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [Authorize("Permission")]
        public async Task<ApiBaseReturn<bool>> TakeAction([FromBody] TakeActionView model)
        {
            ApiBaseReturn<bool> returnVM = new ApiBaseReturn<bool>();
            if (!ErrorInfo.Status)
            {
                try
                {
                    #region 设置返回值

                    if (model.ActionArea.IsNullOrEmpty())
                    {
                        //请选择操作区域。
                        throw new Exception(_localizer["PLEASE_SELEC_AREA"]);
                    }

                    if (model.NewAction.IsNullOrEmpty())
                    {
                        //请选择需要执行的动作。
                        throw new Exception(_localizer["PLEASE_SELEC_ACTION"]);
                    }

                    _service.ActionArea = model.ActionArea;
                    _service.ActionAreaType = model.ActionAreaType;
                    _service.UserName = model.UserName;
                    _service.ReelCode = model.ReelCode;
                    if (Convert.ToDecimal(model.NewAction) == (decimal)MSDAction.Bake)
                    {
                        var selectBakeRuleRow = (await _repository.QueryAsyncEx<SmtMsdBakeRule>("SELECT * FROM SMT_MSD_BAKE_RULE WHERE ID=:BID", new { BID = model.MSDBakeRuleID })).FirstOrDefault();
                        if (selectBakeRuleRow == null)
                        {
                            //请选择烘烤规则。.
                            throw new Exception(_localizer["PLEASE_SELECT_BAKING"]);
                        }
                        _service.BakeTemperature = selectBakeRuleRow.BAKE_TEMPERATURE ?? 0;
                        _service.BakeHumidity = selectBakeRuleRow.BAKE_HUMIDITY ?? 0;
                        _service.StandardBakeTime = selectBakeRuleRow.BAKE_TIME ?? 0;
                    }
                    _service.NewAction = Convert.ToDecimal(model.NewAction);

                    string reelCode = model.ReelCode;
                   await _service.Process(reelCode);
                    //_service.HintProcessOK(reelCode);
                    returnVM.Result = true;
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
        /// 操作變更
        /// partThickness(元件厚度)
        /// partLevelCode(元件等級)
        /// reelCode(料卷条码)
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        [HttpGet]
        [Authorize]
        public async Task<ApiBaseReturn<List<SmtMsdBakeRule>>> MsdActionLookUpChanged([FromQuery] string newActionID, string partLevelCode, string partThickness,string reelcode)
        {
            ApiBaseReturn<List<SmtMsdBakeRule>> returnVM = new ApiBaseReturn<List<SmtMsdBakeRule>>();
            if (!ErrorInfo.Status)
            {
                try
                {
                    _service.PartLevelCode = partLevelCode;
                    _service.PartThickness = partThickness;
                    _service.ReelCode = reelcode;
                    await  _service.GetData(reelcode);
                    int newActionValue = newActionID.IsNullOrEmpty() ?
                0 : Convert.ToInt32(newActionID);

                    switch (newActionValue)
                    {
                        case (int)MSDAction.Bake:

                            var msdBakeRuleList = await _service.GetMSDBakeStandard();
                            returnVM.Result = msdBakeRuleList;
                            if (msdBakeRuleList.Count == 0)
                            {
                                //找不到对应的烘烤规则，请检查相关设定。
                                ErrorInfo.Set(_localizer["Cannot_Find_BakingRule"], MethodBase.GetCurrentMethod(), EnumErrorType.Warning);
                            }
                            break;
                        default:

                            break;
                    }
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
        /// 保存数据
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        [Authorize("Permission")]
        public async Task<ApiBaseReturn<bool>> SaveData([FromBody] SmtMsdRuncardModel model)
        {
            ApiBaseReturn<bool> returnVM = new ApiBaseReturn<bool>();
            if (!ErrorInfo.Status)
            {
                try
                {
                    #region 检查参数

                    #endregion

                    #region 保存并返回

                    if (!ErrorInfo.Status)
                    {
                        decimal resdata = await _repository.SaveDataByTrans(model);
                        if (resdata != -1)
                        {
                            returnVM.Result = true;
                        }
                        else
                        {
                            returnVM.Result = false;
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

            WriteLog(ref returnVM);

            #endregion

            return returnVM;
        }

        /// <summary>
        /// 添加或修改视图
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Authorize("Permission")]
        public ApiBaseReturn<bool> AddOrModify()
        {
            ApiBaseReturn<bool> returnVM = new ApiBaseReturn<bool>();
            if (!ErrorInfo.Status)
            {
                try
                {
                    #region 设置返回值

                    returnVM.Result = true;

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
        /// 删除
        /// </summary>
        /// <param name="id">要删除的记录的ID</param>
        /// <returns>JSON格式的响应结果</returns>
        [HttpPost]
        [Authorize("Permission")]
        public async Task<ApiBaseReturn<bool>> DeleteOneById(decimal id)
        {
            ApiBaseReturn<bool> returnVM = new ApiBaseReturn<bool>();
            if (!ErrorInfo.Status)
            {
                try
                {
                    #region 删除并返回

                    if (!ErrorInfo.Status && id <= 0)
                    {
                        returnVM.Result = false;
                        //通用提示类的本地化问题处理
                        string resultMsg = GetLocalMessage(_httpContextAccessor, ResultCodeAddMsgKeys.CommonModelStateInvalidCode,
                            ResultCodeAddMsgKeys.CommonModelStateInvalidMsg);
                        ErrorInfo.Set(resultMsg, MethodBase.GetCurrentMethod(), EnumErrorType.Warning);
                    }
                    if (!ErrorInfo.Status)
                    {
                        var count = await _repository.DeleteAsync(id.ToString());
                        if (count > 0)
                        {
                            returnVM.Result = true;
                        }
                        else
                        {
                            //失败
                            returnVM.Result = false;
                            //通用提示类的本地化问题处理
                            string resultMsg = GetLocalMessage(_httpContextAccessor, ResultCodeAddMsgKeys.CommonExceptionCode,
                                ResultCodeAddMsgKeys.CommonExceptionMsg);
                            ErrorInfo.Set(resultMsg, MethodBase.GetCurrentMethod(), EnumErrorType.Warning);
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

            WriteLog(ref returnVM);

            #endregion

            return returnVM;
        }

        #region 独立方法



        /// <summary>
        /// 檢查Reel的合法性
        /// </summary>
        /// <returns></returns>
        private async Task<bool> IdentifyReel(string _reelCode)
        {

            // 獲取MSD Runcard信息
            msdRuncardTable = await _repository.GetListByTableEX<SmtMsdRuncard>(" * ", "SMT_MSD_RUNCARD", " And REEL_ID=:REEL_ID ", new { REEL_ID = _reelCode });

            // 檢查Reel是否存在
            reelInfoViewRow = (await _repository.GetListByTableEX<dynamic>(" * ", "IMS_REEL_INFO_VIEW", " And CODE=:CODE ", new { CODE = _reelCode })).ToList<dynamic>().FirstOrDefault();
            if (reelInfoViewRow.IsNullOrEmpty())
            {
                // Reel在IMS中不存在
                //料卷{0}不存在，请检查。
                ErrorInfo.Set(string.Format(_localizer["REEL_NOT_EXIST_ERROR"], _reelCode), MethodBase.GetCurrentMethod(), EnumErrorType.Warning);
            }

            reelCode = _reelCode;
            reelPartNO = reelInfoViewRow.PART_NO;
            if (!reelInfoViewRow.IsMAKER_CODENull())
            {
                reelMakerCode = reelInfoViewRow.MAKER_CODE;
            }

            if (msdRuncardTable.Count == 0)
            {
                // Reel第一次管控MSD時，從R12獲取等級和厚度
                msdR12Table = await _repository.GetListByTableEX<dynamic>("*", "IMS_MSD_R12", @" AND PART_CODE=:PART_CODE
               AND MAKER_CODE =:MAKER_CODE", new { PART_CODE = reelPartNO, MAKER_CODE = reelMakerCode });

                if (msdR12Table.Count == 0)
                {
                    msdR12Table = await _repository.GetListByTableEX<dynamic>("*", "IMS_MSD_R12", @" AND PART_CODE=:PART_CODE
               ", new { PART_CODE = reelPartNO });
                }

                if (msdR12Table.Count > 0)
                {
                    partLevelCode = msdR12Table[0].LEVEL_CODE.ToString().ToLower();
                    partThickness = msdR12Table[0].PART_THICKNESS.IsNullOrEmpty() ? 0 : (decimal)msdR12Table[0].PART_THICKNESS;
                }
                else
                {
                    //"没有找到元件等级，请确认。"--No component grade found
                    throw new Exception(_localizer["COMPONENT_NO_FOUND"]);
                }
            }
            else
            {
                // 已有管控的，按録入時候的等級和厚度繼續管控
                partLevelCode = msdRuncardTable.FirstOrDefault().LEVEL_CODE;
                partThickness = msdRuncardTable.FirstOrDefault().THICKNESS;
            }

            // 只有Reel第一次管控MSD時，檢查等級并提示
            if (partLevelCode.IsNullOrEmpty() || (partLevelCode == "0") || (partLevelCode == "1"))
            {
                string hintMessage = string.Empty;
                if (this.partLevelCode.IsNullOrEmpty())
                {
                    //料卷{0}等级缺省，不需要进行管控(系统默认)。
                    hintMessage = string.Format(_localizer["MATERIAL_NO_CONTROL"], reelCode);
                }
                else
                {
                    //料卷{0}等级为<{1}>，不需要进行管控。MATERIAL_NO_CONTROLEX 
                    hintMessage = string.Format(_localizer["MATERIAL_NO_CONTROLEX"], reelCode, this.partLevelCode);
                }
                throw new Exception(hintMessage);
                // Messenger.Hint(hintMessage);
                // return false;
            }

            // R12沒有元件厚度時，獲取默認厚度
            if (partThickness == 0)
            {
                var lookupRow = (await _repository.GetListByTableEX<SmtLookup>("*", "SMT_LOOKUP", @" AND TYPE='MSD_DEFAULT_THICKNESS' and ENABLED='Y' 
               ORDER BY TYPE "))?.ToList<SmtLookup>().FirstOrDefault();

                if (lookupRow != null)
                {
                    partThickness = Convert.ToDecimal(lookupRow.VALUE);
                }
            }

            //if (this.msdRuncardTable.Rows.Count == 0 &&
            //    IQCManager.GetDMRReel(
            //        new KeyValuePair<string, object>(GlobalVariables.REEL_ID, this.reelInfoViewRow.ID)).Rows.Count > 0)
            //{
            //    Messenger.Hint(string.Format("料卷{0}是DMR特殊物料，有可能过期或破损，请务必先烘烤再使用。", this.reelCode));
            //}

            return true;

        }

        #endregion

    }


}