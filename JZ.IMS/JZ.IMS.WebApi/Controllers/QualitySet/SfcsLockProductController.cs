/*
*┌──────────────────────────────────────────────────────────────┐
*│　描    述： 控制器                                                    
*│　作    者：嘉志科技                                              
*│　版    本：3.0   模板代码自动生成                                              
*│　创建时间：2020-04-22 09:40:14                           
*└──────────────────────────────────────────────────────────────┘
*┌──────────────────────────────────────────────────────────────┐
*│　命名空间： JZ.IMS.WebApi.Controllers                                   
*│　接口名称： ISfcsHoldProductHeaderController                                      
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
using System.Collections;
using JZ.IMS.WebApi.Common;
using static JZ.IMS.WebApi.Common.HoldProduct;
using JZ.IMS.Repository.Oracle;
using System.Net.Http.Headers;
using System.IO;
using JZ.IMS.ViewModels.QualitySet;

namespace JZ.IMS.WebApi.Controllers
{
    /// <summary>
    /// 产品锁定 控制器
    /// </summary>
	[Route("api/[controller]/[action]")]
    [ApiController]
    public class SfcsLockProductController : BaseController
    {
        private readonly ISfcsLockProductHeaderRepository _repository;
        private readonly IMapper _mapper;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IStringLocalizer<SfcsLockProductController> _localizer;
        private DateTime beginTime;
        private DateTime endTime;
        private string inventory;

        private int mainConditionSelectIndex;
        private int subsidiaryConditionSelectIndex;
        private int actionSelectIndex;
        public string snFilePath;


        public enum HoldProductSubsidiaryCondition
        {
            TurnInTime,
            InputTime,
            LastBFTTime,
            Invertory,
            NoSubsidiaryCondition
        }
        public SfcsLockProductController(ISfcsLockProductHeaderRepository repository, IMapper mapper, IHttpContextAccessor httpContextAccessor,
            IStringLocalizer<SfcsLockProductController> localizer)
        {
            _repository = repository;
            _mapper = mapper;
            _httpContextAccessor = httpContextAccessor;
            _localizer = localizer;
        }

        public class IndexVM
        {
            /// <summary>
            /// 主管控字典
            /// </summary>
            public List<dynamic> MainCondition { get; set; }
            /// <summary>
            /// 辅助管控项目
            /// </summary>
            public List<dynamic> SubsidiaryCondition { get; set; }
            /// <summary>
            /// 管控动作
            /// </summary>
            public List<dynamic> ControlAction { get; set; }
            /// <summary>
            /// 管控站点operationSite
            /// </summary>
            public List<dynamic> OperationSite { get; set; }
            /// <summary>
            /// 獲取站點所屬線別信息
            /// </summary>
            public List<dynamic> OperationLine { get; set; }
            /// <summary>
            /// 管控工序
            /// </summary>
            public List<dynamic> OperationsList { get; set; }

        }

        public class ResultIndex
        {
            /// <summary>
            /// 前台显示 SN号/零件号
            /// </summary>
            public List<string> SNList { get; set; }
            
            /// <summary>
            /// 保存成功不？
            /// </summary>
            public bool IsOk { get; set ; }

            /// <summary>
            /// 单据号
            /// </summary>
            public string BillNumber { get; set; }
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
                        returnVM.Result = new IndexVM()
                        {
                            MainCondition = await _repository.GetListByTable(" SP.ID,SP.LOOKUP_CODE,SP.MEANING  ", " SFCS_PARAMETERS SP ", " And  LOOKUP_TYPE='MAIN_CONDITION'   ORDER BY LOOKUP_TYPE"),
                            SubsidiaryCondition = await _repository.GetListByTable(" SP.ID,SP.LOOKUP_CODE,SP.MEANING ", " SFCS_PARAMETERS SP ", " And LOOKUP_TYPE='SUBSIDIARY_CONDITION'   ORDER BY LOOKUP_TYPE"),
                            ControlAction = await _repository.GetListByTable(" SP.ID,SP.LOOKUP_CODE,SP.MEANING ", " SFCS_PARAMETERS SP ", " And LOOKUP_TYPE='CONTROL_ACTION'   ORDER BY LOOKUP_TYPE "),
                            OperationSite = await _repository.GetListByTable(" OS.ID,OS.OPERATION_SITE_NAME ", " SFCS_OPERATION_SITES OS ", "  ORDER BY OS.OPERATION_SITE_NAME "),
                            OperationLine = await _repository.GetListByTable(" OL.ID,OL.OPERATION_LINE_NAME ", " SFCS_OPERATION_LINES OL ", "  ORDER BY OPERATION_LINE_NAME "),
                            OperationsList = await _repository.GetListByTable(" SO.ID,SO.Operation_Name ", " SFCS_OPERATIONS SO ", " And SO.OPERATION_CATEGORY!=5 order by SO.Operation_Name "),
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
        /// 开发资料
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Authorize("Permission")]
        public async Task<ApiBaseReturn<string>> GetDocmentAPI()
        {
            ApiBaseReturn<string> returnVM = new ApiBaseReturn<string>();
            if (!ErrorInfo.Status)
            {
                try
                {
                    #region 设置返回值

                    if (!ErrorInfo.Status)
                    {
                        returnVM.Result = @"HoldBySerialNumber,//單筆/批量產品序號    产品序号 产品流水号
                                            HoldByCustomSerialNumber,//料號與自定義產品序號
                                            HoldByCartonOrPallet,//卡通/棧板
                                            HoldByWorkOrderOrPartNumberOrModel,//工單/料號/機種
                                            HoldByComponentCustomerPartNumber,//零件客戶料號
                                            HoldByComponentSerialNumber,//單筆/批量零件序號
                                            HoldByCustomComponentSerialNumber,//料號與自定義零件序號
                                            HoldSiteByPartNumberOrWorkOrder,//料號/工單與站點
                                            HoldSite,//站點
                                            HoldWipOperationBySerialNumber,//產品序號與工序
                                            HoldWIPOperationByPartNumberAndOperationLine = 21,
                                            HoldWIPOperationByPartNumber = 22
                                            
                                            --historyMemoEdit 就是操作记录 
                                            --mainConditionRadioGroup 主界面操作 
                                            --dataInputTextEdit 数据输入 
                                            --snButtonEdit 产品流水号
                                            --compSNButtonEdit 产品零件序号 
                                            --beginSpinEdit 始于第 位 至第 endSpinEdit 位序号为 snRangerTextEdit 
                                            --operationSiteLookUpEdit 管控站点 
                                            --operationLookUpEdit 管控工序 
                                            --actionRadioGroup 管控业务
                                            --subsidiaryRadioGroup辅助条件 
                                            --beginDateTimePicker开始时间 
                                            --endDateTimePicker结束时间 
                                            --inventoryButtonEdit 库别
                                            --causeMemoEdit 锁定原因
                                            --ecnTextEdit 标记ECNO
                                            
                                            
                                            --historyMemoEdit 就是操作记录 
                                            --mainConditionRadioGroup 主界面操作 
                                            --dataInputTextEdit 数据输入 
                                            --snButtonEdit 产品流水 
                                            --compSNButtonEdit 产品零件序号 
                                            --beginSpinEdit 始于第 位 至第 endSpinEdit 位序号为 snRangerTextEdit 
                                            --operationSiteLookUpEdit 管控站点 
                                            --operationLookUpEdit 管控工序 (OperationVale)
                                            --actionRadioGroup 管控业务
                                            --辅助条件 subsidiaryRadioGroup
                                            --开始时间 beginDateTimePicker
                                            --结束时间 endDateTimePicker
                                            --inventoryButtonEdit 库别
                                            --causeMemoEdit 完成锁定
                                            --causeMemoEdit 锁定原因
                                            --ecnTextEdit 标记ECNO
                                            --username用户名字
                                            
                                            --
                                            ProcessDefineSerialNumber
                                            partno dataInputTextEdit 
                                            data snRangerTextEdit
                                            beginSpinEdit startindex 
                                            endSpinEdit endindex 
                                            
                                            
                                            [验证数据]
                                            --验证
                                            [管控条件验证]
                                            主管条件
                                            1.//單筆/批量產品序號 {料号为空的时候，提示:请输入有效数据，{0} 对应的料号}
                                            2.//料號與自定義產品序號和//料號與自定義零件序號{开始位和结束位为0,分别提示请输入开始字符位置，请输入结束字符位置,如果序号为空，提示请输入 固定序号,如果开始序号大于结束序号提示:开始节点大于结束节点，如果(结束序号-开始序号+1)!=位序号的长度，提示:选择的序号长度与输入的序号固定值长度不一致，请确认。}
                                            3.//卡通/棧板和//工單/料號/機種和//零件客戶料號 (如果输入值为空，提示:请输入有效数据，{0}对应输入的内容，)
                                            4.//料號/工單與站點（如果料号为空和管控站点为空，提示:请输入有效数据，{0}对应输入的内容，）
                                            5.//站點（管控站点为空就提示:请输入有效数据，{0}对应输入的内容）
                                            6.//產品序號與工序(产品流水号和管控工序为空就提示:请输入有效数据，{0}对应输入的内容,)
                                            [管控业务验证]
                                            1.如果没有选择提示:请选择要管制的动作。
                                            2.选择不能组装时候 ，同时管控条件选择不是//零件客戶料號和//單筆/批量零件序號，提示:只有设定零件方面信息才能决定不能组装动作。
                                            3.如果管控条件选择//產品序號與工序同时管控业务选择不是 不能流水生产作业 就提示:选择锁定产品与工序，必须选择不能流水作业生产。
                                            [辅助条件验证]
                                            1.管控条件选择//料號與自定義產品序號 料号为空的时候，辅助条件不是存仓时间，同时也不是投产时间，提示自定义序号必须再加上投产时间或存仓时间作为约束，否则数据查询太慢。在上面这个条件里面，结束日期大于开始日期的100天，就提示:时间范围过长，不适合自定义序号来管控，请用其它管控办法。
                                            2.辅助条件为库别，如果库别为空就提示: 找不到相应的库别。 
                                            3.如果选择的是时间，就是结束时间要大于开始时间，不然提示
                                            4.时间没有问题，再在历史记录显示 时间";
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

        #region 1.單筆/批量產品序號 //10.產品序號與工序

        /// <summary>
        /// 1.單筆/批量產品序號 //10.產品序號與工序
        /// 保存数据 ，传snvalue(产品流水号)可以上传文件,OperationVale(管控工序)成功了会返回 SN列表需要显示，成功提示 {0} 锁定成功！SN
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        [Authorize("Permission")]
        public async Task<ApiBaseReturn<ResultIndex>> ProcessSingleOrMultiRuncardSave([FromForm] SfcsLockProductModel model)
        {

            List<SfcsRuncard> runcards = new List<SfcsRuncard>();
            ApiBaseReturn<ResultIndex> returnVM = new ApiBaseReturn<ResultIndex>();
            ResultIndex resultmsg = new ResultIndex() { IsOk = false };
            resultmsg.SNList = new List<string>();
            var Files = Request.Form.Files;
            IFormFile txtFile = null;
            if (Files != null && Files.Count > 0)
            { txtFile = Request.Form.Files[0]; }
            var filename = string.Empty;
            var extname = string.Empty;
            var newFileName = string.Empty;
            string conditions = string.Empty;
            decimal filesize = 0;

            this.beginTime = model.BeginDate.IsNullOrWhiteSpace()? DateTime.Now: Convert.ToDateTime(model.BeginDate);
            this.endTime = model.EndDate.IsNullOrWhiteSpace() ? DateTime.Now : Convert.ToDateTime(model.EndDate);
            this.inventory = model.InventoryVale;
            SfcsLockProductHeaderRepository.operationID = model.OperationVale ?? 0;
            this.subsidiaryConditionSelectIndex = model.SubsidiaryRadioGroup ?? 0;
            this.mainConditionSelectIndex = model.MainConditionRadioGroup ?? 0;
            this.actionSelectIndex = model.ActionRadioGroup ?? 0;
            string snvalue = model.SNvalue;
            decimal operationID = model.OperationVale ?? 0;

            if (!ErrorInfo.Status)
            {
                try
                {
                    if (txtFile != null)
                    {

                        #region 检查参数

                        if (!ErrorInfo.Status && (txtFile == null || txtFile.FileName.IsNullOrEmpty()))
                        {
                            //上传失败
                            ErrorInfo.Set(_localizer["upload_error"], MethodBase.GetCurrentMethod(), EnumErrorType.Warning);
                        }

                        if (!ErrorInfo.Status)
                        {
                            filename = ContentDispositionHeaderValue
                                            .Parse(txtFile.ContentDisposition)
                                            .FileName
                                            .Trim('"');
                            extname = filename.Substring(filename.LastIndexOf("."), filename.Length - filename.LastIndexOf("."));

                            #region 判断后缀

                            if (!extname.ToLower().Contains("txt"))
                            {
                                //msg = "只允许上传txt文件."
                                ErrorInfo.Set(_localizer["file_suffix_error"], MethodBase.GetCurrentMethod(), EnumErrorType.Warning);
                            }

                            #endregion

                            #region 判断大小

                            filesize = Convert.ToDecimal(Math.Round(txtFile.Length / 1024.00, 3));
                            long mb = txtFile.Length / 1024 / 1024; // MB
                            if (mb > 1)
                            {
                                //"只允许上传小于 1MB 的文件."
                                ErrorInfo.Set(_localizer["size_1m_error"], MethodBase.GetCurrentMethod(), EnumErrorType.Warning);
                            }

                            #endregion
                        }
                        #endregion

                        #region 解释txt数据

                        if (!ErrorInfo.Status)
                        {
                            newFileName = DateTime.Now.ToString("yyyyMMddHHmmssfff") + new Random().Next(1000, 9999) + extname;
                            var pathRoot = AppContext.BaseDirectory + @"upload\tmpdata\";
                            if (Directory.Exists(pathRoot) == false)
                            {
                                Directory.CreateDirectory(pathRoot);
                            }
                            filename = pathRoot + $"{newFileName}";
                            using (FileStream fs = System.IO.File.Create(filename))
                            {
                                txtFile.CopyTo(fs);
                                fs.Flush();
                            }

                            if (!System.IO.File.Exists(filename))
                            {
                                ErrorInfo.Set(_localizer["upload_error"], MethodBase.GetCurrentMethod(), EnumErrorType.Warning);
                            }
                        }

                        #endregion
                    }

                    #region 保存并返回

                    if (!ErrorInfo.Status)
                    {
                        #region IdentifyRuncardSN(string data)snvalue 
                        if (snvalue.IsNullOrWhiteSpace()&&!ErrorInfo.Status)
                        {
                            snvalue = filename;
                        }
                        //runcards = await new HoldProduct().IdentifyRuncard(_repository, _localizer, runcards, snvalue);
                        runcards = await _repository.GetRuncardDataTable(snvalue);

                        if (runcards.Count > 0)
                        {
                            // serialNumber = snvalue;
                        }
                        else if (System.IO.File.Exists(snvalue))
                        {
                            System.Collections.ArrayList list = FilePublic.GetSimpleFileContent(snvalue);
                            for (int i = 0; i < list.Count; i++)
                            {
                                string sn = list[i].ToString().Trim();
                                //序號為空，跳過
                                if (string.IsNullOrEmpty(sn))
                                {
                                    continue;
                                }
                                bool matchExist = false;
                                foreach (var row in runcards)
                                {
                                    if (row.SN == sn)
                                    {
                                        matchExist = true;
                                        break;
                                    }
                                }

                                //序號為空，跳過
                                if (matchExist)
                                {
                                    continue;
                                }

                                if ((await _repository.GetRuncardDataTable(sn)).Count == 0 && !ErrorInfo.Status)
                                {
                                    //resultmsg.MsgList.Add(string.Format(_localizer["Err_SerialNumberNotExist"], sn));
                                    ErrorInfo.Set(string.Format(_localizer["Err_SerialNumberNotExist"], sn), MethodBase.GetCurrentMethod(), EnumErrorType.Error);
                                }
                                else
                                {
                                    runcards.AddRange(await _repository.GetRuncardDataTable(sn));
                                }
                            }
                            snFilePath = snvalue;
                        }
                        else
                        {
                            if (!ErrorInfo.Status)
                            {
                                ErrorInfo.Set(string.Format(_localizer["Err_UnKnow"], snvalue), MethodBase.GetCurrentMethod(), EnumErrorType.Warning);
                            }
                            
                        }
                        #endregion

                        #region 中间业务
                        if (runcards.Count == 0 && !ErrorInfo.Status)
                        {
                            ErrorInfo.Set(string.Format(_localizer["Err_SerialNumberNotExist"], snvalue), MethodBase.GetCurrentMethod(), EnumErrorType.Warning);
                        }

                        HoldProduct holdproductobj = new HoldProduct();
                        runcards = holdproductobj.ExecuteRestraint(_repository, subsidiaryConditionSelectIndex, mainConditionSelectIndex, runcards, beginTime, endTime);

                        if (runcards.Count == 0 && !ErrorInfo.Status)
                        {
                            //throw new MESException(Operation.Bussiness.Properties.Resources.Err_NoRuncardMatch);
                            ErrorInfo.Set(string.Format(_localizer["Err_NoRuncardMatch"], snvalue), MethodBase.GetCurrentMethod(), EnumErrorType.Warning);
                        }

                        #endregion

                        #region RecordHoldProduct
                        
                        resultmsg.SNList.AddRange(await _repository.RuncardSave(runcards, mainConditionSelectIndex, snvalue, subsidiaryConditionSelectIndex, inventory, beginTime, endTime, actionSelectIndex, model.HoldCause, model.UserName));
                        
                        if (resultmsg.SNList!=null&& resultmsg.SNList.Count>0)
                        {
                            resultmsg.IsOk = true;
                            resultmsg.BillNumber = SfcsLockProductHeaderRepository.BillID;
                            returnVM.Result = resultmsg;
                        }
                        else
                        {
                            returnVM.Result = resultmsg;
                        }

                        #endregion
                    }

                    #endregion
                }
                catch (Exception ex)
                {
                    ErrorInfo.Set(ex.Message, MethodBase.GetCurrentMethod(), EnumErrorType.Error);
                }
                finally
                {
                    if (!filename.IsNullOrWhiteSpace())
                    {
                        if (System.IO.File.Exists(filename))
                        {
                            System.IO.File.Delete(filename);
                        }
                    }
                }
            }

            #region 如果出现错误，则写错误日志并返回错误内容

            WriteLog(ref returnVM);

            #endregion

            return returnVM;
        }

        #endregion

        #region 2.料號與自定義產品序號和7.料號與自定義零件序號

        /// <summary>
        /// 2.料號與自定義產品序號和7.料號與自定義零件序號
        /// 保存数据  传dataInputValue（料号）这个可以为空,snRangerValue(位序号),beginSpinValue(开始于),endSpinValue(结束于)
        /// </summary>
        /// <param name="holdcause">锁定原因</param>
        /// <param name="ecnNo">标记ecnNO</param>
        /// <returns></returns>
        [HttpPost]
        [Authorize("Permission")]
        public async Task<ApiBaseReturn<ResultIndex>> ProcessDefineSerialNumberSave([FromBody]SfcsLockProductModel model)
        {

            // 處理自定義序號功能
            List<SfcsRuncard> runcards = new List<SfcsRuncard>();
            ApiBaseReturn<ResultIndex> returnVM = new ApiBaseReturn<ResultIndex>();
            ResultIndex resultmsg = new ResultIndex() { IsOk = false };
            resultmsg.SNList = new List<string>();
            int subsidiaryConditionSelectIndex = model.SubsidiaryRadioGroup ?? 0;
            int mainConditionSelectIndex = model.MainConditionRadioGroup ?? 0;
            int actionSelectIndex = model.ActionRadioGroup ?? 0;
            decimal beginStringIndex = model.BeginSpinValue ?? 0;
            decimal endStringIndex = model.EndSpinValue ?? 0;
            SfcsLockProductHeaderRepository.partNumber = model.DataInputValue;
            string partNumber = model.DataInputValue;
            string defineString = model.SNRangerValue;
            string inventory = model.InventoryVale;
            this.beginTime = model.BeginDate.IsNullOrWhiteSpace() ? DateTime.Now : Convert.ToDateTime(model.BeginDate);
            this.endTime = model.EndDate.IsNullOrWhiteSpace() ? DateTime.Now : Convert.ToDateTime(model.EndDate);
            if (!ErrorInfo.Status)
            {
                try
                {
                    #region 检查参数
                    //處理成品料號+自定義runcad sn受管控
                    decimal END_POSITION = endStringIndex - beginStringIndex + 1;
                    if (mainConditionSelectIndex == (int)HoldProductMainCondition.HoldByCustomSerialNumber)
                    {
                        //成品料號不為空+沒有選擇輔助條件
                        if (subsidiaryConditionSelectIndex == 4)//这里cs为-1
                        {
                            var temp = await _repository.GetListByTableEX<SfcsRuncard>(" SR.* ", " SFCS_RUNCARD SR,SFCS_WO SW ", @" And SUBSTR(SR.SN, :START_POSITION, :END_POSITION) = :DATA AND SR.WO_ID = SW.ID
and SW.PART_NO=:PART_NO  ", new { PART_NO = partNumber, START_POSITION = beginStringIndex, END_POSITION = END_POSITION, DATA = defineString });
                            runcards.AddRange(temp);
                        }
                        if (subsidiaryConditionSelectIndex == (int)HoldProductSubsidiaryCondition.TurnInTime)
                        {
                            if (partNumber.IsNullOrEmpty())
                            {
                                //成品料號為空+選擇輔助條件(存倉時間)
                                var temp = await _repository.GetListByTableEX<SfcsRuncard>(" SR.* ", " SFCS_RUNCARD SR,SFCS_WO SW ", @" And SUBSTR(SR.SN, :START_POSITION, :END_POSITION) = :DATA AND SR.WO_ID = SW.ID
and SR.TURNIN_TIME >= :TURNIN_BEGIN_TIME and SR.TURNIN_TIME <= :TURNIN_END_TIME ", new { START_POSITION = beginStringIndex, END_POSITION = END_POSITION, DATA = defineString, TURNIN_BEGIN_TIME = beginTime, TURNIN_END_TIME = endTime });
                                runcards.AddRange(temp);
                            }
                            else
                            {
                                //成品料號不為空+選擇輔助條件(存倉時間)
                                var temp = await _repository.GetListByTableEX<SfcsRuncard>(" SR.* ", " SFCS_RUNCARD SR,SFCS_WO SW ", @" And SUBSTR(SR.SN, :START_POSITION, :END_POSITION) = :DATA AND SR.WO_ID = SW.ID
and SW.PART_NO=:PART_NO and SR.TURNIN_TIME >= :TURNIN_BEGIN_TIME and SR.TURNIN_TIME <= :TURNIN_END_TIME   ", new { PART_NO = partNumber, START_POSITION = beginStringIndex, END_POSITION = END_POSITION, DATA = defineString, TURNIN_BEGIN_TIME = beginTime, TURNIN_END_TIME = endTime });
                                runcards.AddRange(temp);
                            }
                        }
                        if (subsidiaryConditionSelectIndex == (int)HoldProductSubsidiaryCondition.InputTime)
                        {
                            if (partNumber.IsNullOrEmpty())
                            {
                                //成品料號為空+選擇輔助條件(投入時間)
                                var temp = await _repository.GetListByTableEX<SfcsRuncard>(" SR.* ", " SFCS_RUNCARD SR,SFCS_WO SW ", @" And SUBSTR(SR.SN, :START_POSITION, :END_POSITION) = :DATA AND SR.WO_ID = SW.ID
 and SR.INPUT_TIME >= :INPUT_BEGIN_TIME and SR.INPUT_TIME <= :INPUT_END_TIME    ", new { START_POSITION = beginStringIndex, END_POSITION = END_POSITION, DATA = defineString, INPUT_BEGIN_TIME = beginTime, INPUT_END_TIME = endTime });
                                runcards.AddRange(temp);
                            }
                            else
                            {
                                //成品料號不為空+選擇輔助條件(投入時間)
                                var temp = await _repository.GetListByTableEX<SfcsRuncard>(" SR.* ", " SFCS_RUNCARD SR,SFCS_WO SW ", @" And SUBSTR(SR.SN, :START_POSITION, :END_POSITION) = :DATA AND SR.WO_ID = SW.ID
 and SW.PART_NO=:PART_NO and SR.INPUT_TIME >= :INPUT_BEGIN_TIME and SR.INPUT_TIME <= :INPUT_END_TIME    ", new { PART_NO = partNumber, START_POSITION = beginStringIndex, END_POSITION = END_POSITION, DATA = defineString, INPUT_BEGIN_TIME = beginTime, INPUT_END_TIME = endTime });
                                runcards.AddRange(temp);
                            }
                        }
                    }

                    //處理成品料號+自定義component sn受管控
                    if (mainConditionSelectIndex == (int)HoldProductMainCondition.HoldByCustomComponentSerialNumber)
                    {
                        //成品料號不為空+沒有選擇輔助條件
                        if (subsidiaryConditionSelectIndex == -1)
                        {
                            var temp = await _repository.GetListByTableEX<SfcsRuncard>("  DISTINCT SR.* ", " SFCS_RUNCARD SR, SFCS_COLLECT_COMPONENTS SCC, SFCS_WO SW ", @" AND SCC.SN_ID = SR.ID AND SR.WO_ID = SW.ID And SUBSTR(SCC.CUSTOMER_COMPONENT_SN, :START_POSITION, :END_POSITION) = :DATA
 AND SW.PART_NO = :PART_NO    ", new { PART_NO = partNumber, START_POSITION = beginStringIndex, END_POSITION = END_POSITION, DATA = defineString });
                            runcards.AddRange(temp);
                        }
                        else
                        {
                            if (partNumber.IsNullOrEmpty())
                            {
                                //成品料號為空+選擇輔助條件(投入時間)
                                var temp = await _repository.GetListByTableEX<SfcsRuncard>(" DISTINCT SR.* ", " SFCS_RUNCARD SR, SFCS_COLLECT_COMPONENTS SCC, SFCS_WO SW  ", @" And  SCC.SN_ID = SR.ID AND SR.WO_ID = SW.ID And SUBSTR(SCC.CUSTOMER_COMPONENT_SN, :START_POSITION, :END_POSITION) = :DATA
AND SCC.COLLECT_TIME >= :INPUT_BEGIN_TIME AND SCC.COLLECT_TIME <= :INPUT_END_TIME ", new { START_POSITION = beginStringIndex, END_POSITION = END_POSITION, DATA = defineString, INPUT_BEGIN_TIME = beginTime, INPUT_END_TIME = endTime });
                                runcards.AddRange(temp);
                            }
                            else
                            {
                                //成品料號不為空+選擇輔助條件(投入時間)
                                var temp = await _repository.GetListByTableEX<SfcsRuncard>(" DISTINCT SR.* ", " SFCS_RUNCARD SR, SFCS_COLLECT_COMPONENTS SCC, SFCS_WO SW ", @" And  SCC.SN_ID = SR.ID AND SR.WO_ID = SW.ID And SUBSTR(SCC.CUSTOMER_COMPONENT_SN, :START_POSITION, :END_POSITION) = :DATA 
 AND SW.PART_NO = :PART_NO AND SCC.COLLECT_TIME >= :INPUT_BEGIN_TIME AND SCC.COLLECT_TIME <= :INPUT_END_TIME   ", new { PART_NO = partNumber, START_POSITION = beginStringIndex, END_POSITION = END_POSITION, DATA = defineString, INPUT_BEGIN_TIME = beginTime, INPUT_END_TIME = endTime });
                                runcards.AddRange(temp);
                            }
                        }
                    }

                    if (runcards.Count == 0)
                    {
                        //没有查找到符合规则的产品，请调整条件后再动作
                        ErrorInfo.Set(_localizer["Err_NoRuncardMatch"], MethodBase.GetCurrentMethod(), EnumErrorType.Warning);
                    }
                    #endregion

                    #region 保存并返回

                    if (!ErrorInfo.Status)
                    {
                        string data = (partNumber.IsNullOrEmpty() ? string.Empty : partNumber + GlobalVariables.comma) + beginStringIndex.ToString() + GlobalVariables.comma + endStringIndex.ToString() + GlobalVariables.comma + defineString;
                        resultmsg.SNList.AddRange(await _repository.RuncardSave(runcards, mainConditionSelectIndex, data, subsidiaryConditionSelectIndex, inventory, beginTime, endTime, actionSelectIndex, model.HoldCause, model.UserName));
                        if (resultmsg.SNList != null && resultmsg.SNList.Count > 0)
                        {

                            resultmsg.IsOk = true;
                            resultmsg.BillNumber = SfcsLockProductHeaderRepository.BillID;
                            returnVM.Result = resultmsg;

                        }
                        else
                        {
                            returnVM.Result = resultmsg;
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

        #endregion

        #region 卡通/棧板和工單/料號/機種和零件客戶料號

        /// <summary>
        /// 卡通/棧板和工單/料號/機種和零件客戶料號
        /// 保存数据  传dataInputValue 这个对应的(卡通/栈板号,工单/料号/机种,零件客户料号)
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        [Authorize("Permission")]
        public async Task<ApiBaseReturn<ResultIndex>> ProcessInputDataSave([FromBody] SfcsLockProductModel model)
        {
            List<SfcsRuncard> runcards = new List<SfcsRuncard>();
            ApiBaseReturn<ResultIndex> returnVM = new ApiBaseReturn<ResultIndex>();
            ResultIndex resultmsg = new ResultIndex() { IsOk = false };
            resultmsg.SNList = new List<string>();
            this.beginTime = model.BeginDate.IsNullOrWhiteSpace() ? DateTime.Now : Convert.ToDateTime(model.BeginDate);
            this.endTime = model.EndDate.IsNullOrWhiteSpace() ? DateTime.Now : Convert.ToDateTime(model.EndDate);
            string inventory = model.InventoryVale;
            int subsidiaryConditionSelectIndex = model.SubsidiaryRadioGroup ?? 0;
            int mainConditionSelectIndex = model.MainConditionRadioGroup ?? 0;
            int actionSelectIndex = model.ActionRadioGroup ?? 0;
            string data = model.DataInputValue;
            if (!ErrorInfo.Status)
            {
                try
                {
                    #region 检查参数
                    if (!ErrorInfo.Status)
                    {

                    }
                    #endregion

                    #region 保存并返回

                    if (!ErrorInfo.Status)
                    {

                        runcards = await _repository.IdentifyInputData(data, runcards, actionSelectIndex);
                        if (runcards.Count > 0 && !ErrorInfo.Status)
                        {
                            runcards = new HoldProduct().ExecuteRestraint(_repository, subsidiaryConditionSelectIndex, mainConditionSelectIndex, runcards, beginTime, endTime);
                            //如果鎖定為不能出貨,則將已出貨的流水號去掉
                            if ((actionSelectIndex == (int)HoldOperation.HoldShip) && !ErrorInfo.Status)
                            {
                                for (int i = runcards.Count - 1; i >= 0; i--)
                                {
                                    if (runcards[i].STATUS == GlobalVariables.Shipped)
                                    {
                                        runcards.Remove(runcards[i]);
                                    }
                                }
                            }
                        }
                        else
                        {
                            if (actionSelectIndex != (int)HoldOperation.HoldAssemply)
                            {
                                ErrorInfo.Set(string.Format(_localizer["Err_UnKnow"], data), MethodBase.GetCurrentMethod(), EnumErrorType.Warning);
                            }
                            else
                            {

                                //處理不能組裝此零件料號業務
                                var result = await _repository.HoldQTYSave(runcards, mainConditionSelectIndex, data, subsidiaryConditionSelectIndex, inventory, beginTime, endTime, actionSelectIndex, model.HoldCause, model.UserName, 1);
                                if (result != -1 && !ErrorInfo.Status)
                                {
                                    List<string> listdata = new List<string>();
                                    listdata.Add(data);
                                    //{0} 锁定成功！
                                    resultmsg.SNList.AddRange(listdata);
                                }
                            }
                        }
                        if (runcards.Count == 0 && !ErrorInfo.Status)
                        {
                            //没有查找到符合规则的产品，请调整条件后再动作
                            ErrorInfo.Set(_localizer["Err_NoRuncardMatch"], MethodBase.GetCurrentMethod(), EnumErrorType.Warning);
                        }
                        if (!ErrorInfo.Status)
                        {
                            resultmsg.SNList.AddRange(await _repository.RuncardSave(runcards, mainConditionSelectIndex, data, subsidiaryConditionSelectIndex, inventory, beginTime, endTime, actionSelectIndex, model.HoldCause, model.UserName));
                            if (resultmsg.SNList != null && resultmsg.SNList.Count > 0)
                            {
                                resultmsg.IsOk = true;
                                resultmsg.BillNumber = SfcsLockProductHeaderRepository.BillID;
                                returnVM.Result = resultmsg;

                            }
                            else
                            {
                                returnVM.Result = resultmsg;
                            }
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
        #endregion

        #region  單筆/批量零件序號
        /// <summary>
        ///  單筆/批量零件序號
        /// 保存数据 传 compSNButtonValue(产品零件序号)
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        [Authorize("Permission")]
        public async Task<ApiBaseReturn<ResultIndex>> ProcessSingleOrMultiComponentSave([FromForm] SfcsLockProductModel model)
        {
            ApiBaseReturn<ResultIndex> returnVM = new ApiBaseReturn<ResultIndex>();
            ResultIndex resultmsg = new ResultIndex() { IsOk = false};
            resultmsg.SNList = new List<string>();
            this.beginTime = model.BeginDate.IsNullOrWhiteSpace() ? DateTime.Now : Convert.ToDateTime(model.BeginDate);
            this.endTime = model.EndDate.IsNullOrWhiteSpace() ? DateTime.Now : Convert.ToDateTime(model.EndDate);
            this.inventory = model.InventoryVale;
            this.subsidiaryConditionSelectIndex = model.SubsidiaryRadioGroup ?? 0;
            this.mainConditionSelectIndex = model.MainConditionRadioGroup ?? 0;
            this.actionSelectIndex = model.ActionRadioGroup ?? 0;
            string data = model.CompSNButtonValue;
            string compSerialNumber = string.Empty;
            List<SfcsRuncard> runcards = new List<SfcsRuncard>();


            var Files = Request.Form.Files;
            IFormFile txtFile = null;
            if (Files != null && Files.Count > 0)
            { txtFile = Request.Form.Files[0]; }

            var filename = string.Empty;
            var extname = string.Empty;
            decimal filesize = 0;
            var newFileName = string.Empty;
            string conditions = string.Empty;

            if (!ErrorInfo.Status)
            {
                try
                {
                    if (txtFile != null)
                    {

                        #region 检查参数

                        if (!ErrorInfo.Status && (txtFile == null || txtFile.FileName.IsNullOrEmpty()))
                        {
                            //上传失败
                            ErrorInfo.Set(_localizer["upload_error"], MethodBase.GetCurrentMethod(), EnumErrorType.Warning);
                        }

                        if (!ErrorInfo.Status)
                        {
                            filename = ContentDispositionHeaderValue
                                            .Parse(txtFile.ContentDisposition)
                                            .FileName
                                            .Trim('"');
                            extname = filename.Substring(filename.LastIndexOf("."), filename.Length - filename.LastIndexOf("."));

                            #region 判断后缀

                            if (!extname.ToLower().Contains("txt"))
                            {
                                //msg = "只允许上传txt文件."
                                ErrorInfo.Set(_localizer["file_suffix_error"], MethodBase.GetCurrentMethod(), EnumErrorType.Warning);
                            }

                            #endregion

                            #region 判断大小

                            filesize = Convert.ToDecimal(Math.Round(txtFile.Length / 1024.00, 3));
                            long mb = txtFile.Length / 1024 / 1024; // MB
                            if (mb > 1)
                            {
                                //"只允许上传小于 1MB 的文件."
                                ErrorInfo.Set(_localizer["size_1m_error"], MethodBase.GetCurrentMethod(), EnumErrorType.Warning);
                            }

                            #endregion
                        }

                        #endregion

                        #region 解释txt数据

                        if (!ErrorInfo.Status)
                        {
                            newFileName = DateTime.Now.ToString("yyyyMMddHHmmssfff") + new Random().Next(1000, 9999) + extname;
                            var pathRoot = AppContext.BaseDirectory + @"upload\tmpdata\";
                            if (Directory.Exists(pathRoot) == false)
                            {
                                Directory.CreateDirectory(pathRoot);
                            }
                            filename = pathRoot + $"{newFileName}";
                            using (FileStream fs = System.IO.File.Create(filename))
                            {
                                txtFile.CopyTo(fs);
                                fs.Flush();
                            }

                            if (!System.IO.File.Exists(filename))
                            {
                                ErrorInfo.Set(_localizer["upload_error"], MethodBase.GetCurrentMethod(), EnumErrorType.Warning);
                            }
                        }

                        #endregion

                    }

                    #region 保存并返回

                    if (!ErrorInfo.Status)
                    {
                        if (data.IsNullOrWhiteSpace() && !ErrorInfo.Status)
                        {
                            data = filename;
                        }

                        if (actionSelectIndex == (int)HoldOperation.HoldAssemply)
                        {

                            List<string> compList = new List<string>();
                            if (System.IO.File.Exists(data))
                            {
                                ArrayList list = FilePublic.GetSimpleFileContent(data);
                                for (int i = 0; i < list.Count; i++)
                                {
                                    string compSN = list[i].ToString().Trim();
                                    if (compList.IndexOf(compSN) < 0)
                                    {
                                        compList.Add(compSN);
                                    }
                                }
                                //this.compSnFilePath = data;
                            }
                            else
                            {
                                compSerialNumber = data;
                                compList.Add(compSerialNumber);
                            }
                            if (compList.Count == 0)
                            {
                                ErrorInfo.Set(string.Format(_localizer["Err_UnKnow"], data), MethodBase.GetCurrentMethod(), EnumErrorType.Warning);
                            }

                            //處理不能組裝此零件序號業務
                       resultmsg.SNList.AddRange(await _repository.HoldComponentQTYSave(runcards, mainConditionSelectIndex, data, subsidiaryConditionSelectIndex, inventory, beginTime, endTime, actionSelectIndex, model.HoldCause, model.UserName, (decimal)compList.Count, compList));
                            if (resultmsg.SNList != null && resultmsg.SNList.Count > 0)
                            {
                                resultmsg.IsOk = true;
                                resultmsg.BillNumber = SfcsLockProductHeaderRepository.BillID;
                                returnVM.Result = resultmsg;

                            }
                            else
                            {
                                returnVM.Result = resultmsg;
                            }
                        }
                        else
                        {
                            if (System.IO.File.Exists(data))
                            {
                                List<string> compList = new List<string>();
                                ArrayList list = FilePublic.GetSimpleFileContent(data);
                                for (int i = 0; i < list.Count; i++)
                                {
                                    string compSN = list[i].ToString().Trim();
                                    if (compList.IndexOf(compSN) < 0)
                                    {
                                        compList.Add(compSN);
                                    }
                                }
                                foreach (string item in compList)
                                {

                                    var tempTable = await _repository.GetListByTableEX<SfcsRuncard>(" SR.* ", " SFCS_RUNCARD SR, SFCS_COLLECT_COMPONENTS SCO ", " And SCO.SN_ID = SR.ID and SCO.CUSTOMER_COMPONENT_SN=:CUSTOMER_COMPONENT_SN ", new { CUSTOMER_COMPONENT_SN = item });
                                    List<SfcsRuncard> newList = new List<SfcsRuncard>();

                                    foreach (var row in tempTable)
                                    {
                                        foreach (var runcardRow in runcards)
                                        {
                                            if (row.SN != runcardRow.SN)
                                            {
                                                newList.Add(row);
                                                break;
                                            }
                                        }
                                    }
                                    if (runcards == null || runcards.Count == 0)
                                    {
                                        newList = tempTable;
                                    }
                                    if (newList.Count > 0)
                                    {
                                        //this.runcardTable.Merge(tempTable);
                                        runcards =  newList;
                                    }
                                }
                            }
                            else
                            {
                                compSerialNumber = data;
                                runcards = await _repository.GetListByTableEX<SfcsRuncard>(" SR.* ", " SFCS_RUNCARD SR, SFCS_COLLECT_COMPONENTS SCO ", " And SCO.SN_ID = SR.ID and SCO.CUSTOMER_COMPONENT_SN=:CUSTOMER_COMPONENT_SN ", new { CUSTOMER_COMPONENT_SN = compSerialNumber });
                            }
                            if (runcards.Count == 0)
                            {
                                ErrorInfo.Set(_localizer["Err_NoRuncardMatch"], MethodBase.GetCurrentMethod(), EnumErrorType.Warning);
                            }

                            resultmsg.SNList.AddRange(await _repository.RuncardSave(runcards, mainConditionSelectIndex, data, subsidiaryConditionSelectIndex, inventory, beginTime, endTime, actionSelectIndex, model.HoldCause, model.UserName));

                            if (resultmsg.SNList != null && resultmsg.SNList.Count > 0)
                            {
                                resultmsg.IsOk = true;
                                resultmsg.BillNumber = SfcsLockProductHeaderRepository.BillID;
                                returnVM.Result = resultmsg;

                            }
                            else
                            {
                                returnVM.Result = resultmsg;
                            }
                        }
                    }

                    #endregion

                }
                catch (Exception ex)
                {

                    ErrorInfo.Set(ex.Message, MethodBase.GetCurrentMethod(), EnumErrorType.Error);
                }
                finally
                {
                    if (!filename.IsNullOrWhiteSpace())
                    {
                        if (System.IO.File.Exists(filename))
                        {
                            System.IO.File.Delete(filename);
                        }
                    }
                }
            }

            #region 如果出现错误，则写错误日志并返回错误内容

            WriteLog(ref returnVM);

            #endregion

            return returnVM;
        }

        #endregion

        #region 料號/工單與站點和站點  

        /// <summary>
        /// 料號/工單與站點和站點    
        /// 保存数据 传dataInputValue(料号),operationSiteName（管控站点），operationSiteID(管控站点ID)
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        [Authorize("Permission")]
        public async Task<ApiBaseReturn<ResultIndex>> ProcessProductOperationSiteSave([FromBody] SfcsLockProductModel model)
        {
            this.subsidiaryConditionSelectIndex = model.SubsidiaryRadioGroup ?? 0;
            this.mainConditionSelectIndex = model.MainConditionRadioGroup ?? 0;
            this.actionSelectIndex = model.ActionRadioGroup ?? 0;
            string data = model.DataInputValue;
            string operationSiteName = model.OperationSiteName;
            List<SfcsRuncard> runcards = new List<SfcsRuncard>();
            ApiBaseReturn<ResultIndex> returnVM = new ApiBaseReturn<ResultIndex>();
            ResultIndex resultmsg = new ResultIndex() { IsOk = false};
            resultmsg.SNList = new List<string>();

            if (!ErrorInfo.Status)
            {
                try
                {
                    #region 检查参数

                    #endregion

                    #region 保存并返回

                    if (!ErrorInfo.Status)
                    {
                        runcards = await _repository.IdentifyInputData(data, runcards, actionSelectIndex);

                        if (data.IsNullOrEmpty())
                        {
                            data = operationSiteName;
                            operationSiteName = string.Empty;
                        }

                        decimal resdata = await _repository.ProductOperationSiteSave(runcards, mainConditionSelectIndex, data, operationSiteName, model.OperationSiteID, subsidiaryConditionSelectIndex, "", null, null, actionSelectIndex, model.HoldCause, model.UserName);
                        if (resdata != -1)
                        {
                            resultmsg.IsOk = true;
                            resultmsg.BillNumber = SfcsLockProductHeaderRepository.BillID;
                            returnVM.Result = resultmsg;

                        }
                        else
                        {
                            returnVM.Result = resultmsg;
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
        #endregion

        #region 保存原方法
        /// <summary>
        /// 保存数据
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        [Authorize("Permission")]
        public async Task<ApiBaseReturn<bool>> SaveData([FromBody] SfcsHoldProductHeaderModel model)
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
        #endregion
    }
}