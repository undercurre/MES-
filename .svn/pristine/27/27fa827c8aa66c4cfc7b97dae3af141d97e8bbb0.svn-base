/**
*┌──────────────────────────────────────────────────────────────┐
*│　描    述： 控制器                                                    
*│　作    者：嘉志科技                                              
*│　版    本：3.0   模板代码自动生成                                              
*│　创建时间：2020-03-17 11:59:41                           
*└──────────────────────────────────────────────────────────────┘
*┌──────────────────────────────────────────────────────────────┐
*│　命名空间： JZ.IMS.WebApi.Controllers                                   
*│　接口名称： ISmtLinesController                                      
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
using System.ComponentModel;

namespace JZ.IMS.WebApi.Controllers
{
    /// <summary>
    /// 贴片线体配置控制器
    /// </summary>
	[Route("api/[controller]/[action]")]
    [ApiController]
    public class PatchlineconfigController : BaseController
    {
        private readonly ISmtLinesRepository _repository;
        private readonly IMapper _mapper;
        private readonly IStringLocalizer<SmtLinesController> _localizer;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly ISmtStationRepository _stationRepository;


        /// <summary>
        /// DIO卡类型
        /// </summary>
        public enum DIOCartType
        {
            [Description("7230卡")]
            C_7230 = 6,
            [Description("7432卡")]
            C_7432 = 16
        }

        /// <summary>
        /// 开关
        /// </summary>
        public enum WS_SensorWorkMode
        {
            [Description("OnOff")]
            swmOnOff = 2,
            [Description("OffOn")]
            swmOffOn = 4
        }

        public PatchlineconfigController(ISmtLinesRepository repository, IMapper mapper, IHttpContextAccessor httpContextAccessor,
            IStringLocalizer<SmtLinesController> localizer, ISmtStationRepository stationRepository)
        {
            _repository = repository;
            _mapper = mapper;
            _httpContextAccessor = httpContextAccessor;
            _localizer = localizer;
            _stationRepository = stationRepository;
        }

        public class IndexVM
        {
            /// <summary>
            /// 获取线别列表
            /// </summary>
            public List<object> linesLists { get; set; }
            /// <summary>
            /// 开关类型
            /// </summary>
            public List<object> wsList { get; set; }
            /// <summary>
            /// DIO卡类型
            /// </summary>
            public List<object> dioList { get; set; }
            /// <summary>
            /// 贴片机组
            /// </summary>
            public List<string> machineList { get; set; }
            /// <summary>
            /// 机台序号
            /// </summary>
            public List<string> stationNolist { get; set; }
            /// <summary>
            /// 机器类型
            /// </summary>
            public List<IDNAME> StatusList { get; set; }
            /// <summary>
            /// Y/N
            /// </summary>
            public List<string> YNList { get; set; }
            /// <summary>
            /// 自动扫描枪端口
            /// </summary>
            public List<string> InputPcbScanner { get; set; }
            /// <summary>
            /// 机台配置默认值
            /// </summary>
            public List<object> StationDefualt { get; set; } 

        }

        /// <summary>
        /// 首页视图
        /// 返回线别表
        /// 返回来DIO卡类型
        /// 返回开关下拉表
        /// 返回贴片机组下拉表
        /// 返回机台序号
        /// 返回Y/N
        /// 返回自动扫描枪端口
        /// 返回机台配置默认值
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Authorize("Permission")]
        public async Task<ApiBaseReturn<IndexVM>> Index(string USER_ID)
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
                            linesLists = await _repository.GetList(USER_ID),
                            dioList = new List<object>(){
                                new { key = "7230卡", value = DIOCartType.C_7230 },
                                new { key = "7230卡", value = DIOCartType.C_7230 }
                            },
                            wsList = new List<object>(){
                                new { key = "OnOff", value = WS_SensorWorkMode.swmOnOff },
                                new { key = "OffOn", value = WS_SensorWorkMode.swmOffOn }
                            },
                            machineList = new List<string>() { "PANASONIC", "PANASONIC_CM", "AI", "RI", "HI", "YAMAHA", "SAMSUNG","SIEMENS" },
                            stationNolist = new List<string>() { "001", "002", "003", "004", "005", "006", "007", "008", "009", "010", "011", "012", "013", "014", "015" },
                            StatusList = await _stationRepository.GetStatus(),
                            YNList = new List<string>() { "Y", "N" },
                            InputPcbScanner = new List<string>() { "  ", "COM1", "COM2", "COM3", "COM4", "COM5", "COM6", "COM7", "COM8", "COM9", "COM10" },
                            StationDefualt =new List<object>() {
                                new {config_type ="105",value ="PANASONIC",description="贴片机组"},
                                new {config_type ="110",value ="",description="停机端口"},
                                new {config_type ="111",value ="5",description="轨道中允许停放板数"},
                                new {config_type ="112",value ="15",description="机器加允许停放板数"},
                                new {config_type ="113",value ="001",description="机台序号"},
                                new {config_type ="114",value ="",description="轨道控制端口"},
                                new {config_type ="115",value ="",description="主版流水感应端口"},
                                new {config_type ="117",value ="",description="自动扫描抢端口"},
                                new {config_type ="140",value ="",description="自动扫描抢串口配置"},
                                new {config_type ="119",value ="1",description="连机数"},
                                new {config_type ="107",value ="",description="入口传感器端口"},
                                new {config_type ="120",value ="2",description="入口传感器工作模式"},
                                new {config_type ="108",value ="",description="出口传感器端口"},
                                new {config_type ="121",value ="2",description="出口传感器工作模式"},
                                new {config_type ="139",value ="N",description="单独追朔"},
                                new {config_type ="142",value ="",description="机器名称"},

                            }
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
        /// 获取机台信息
        /// 通过LINE_ID
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpGet]
        [Authorize]
        public async Task<ApiBaseReturn<List<object>>> GetRoutStation(string LINE_ID)
        {
            ApiBaseReturn<List<object>> returnVM = new ApiBaseReturn<List<object>>();
            if (!ErrorInfo.Status)
            {
                try
                {
                    #region 设置返回值
                    int count = 0;
                    var result = await _repository.GetRoutStation(LINE_ID);
                    if (result != null)
                    {
                        returnVM.Result = result;
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
        /// 获取线别的配置
        /// 通过LINE_ID
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpGet]
        [Authorize]
        public async Task<ApiBaseReturn<List<object>>> GetLinesConfig(string lineid)
        {
            ApiBaseReturn<List<object>> returnVM = new ApiBaseReturn<List<object>>();
            returnVM.Result = null;
            if (!ErrorInfo.Status)
            {
                try
                {
                    #region 设置返回值
                    if (!ErrorInfo.Status && !lineid.IsNullOrWhiteSpace())
                    {
                        var sult = await _repository.GetLinesconfig(lineid);
                        if (sult != null)
                        {
                            returnVM.Result = sult;
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
        /// 获取机台的配置信息
        /// 通过stationid
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpGet]
        [Authorize]
        public async Task<ApiBaseReturn<List<object>>> GetStationConfig(string stationid)
        {
            ApiBaseReturn<List<object>> returnVM = new ApiBaseReturn<List<object>>();
            if (!ErrorInfo.Status)
            {
                try
                {
                    #region 设置返回值
                    if (!ErrorInfo.Status && !stationid.IsNullOrWhiteSpace())
                    {
                        var sult = await _repository.GetStationconfig(stationid);
                        if (sult != null)
                        {
                            returnVM.Result = sult;
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
        /// 查询是否已经存在机器名称TRUE为已经存在
        /// ID:机器ID
        /// Name:为机器名称
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpGet]
        [Authorize/*("Permission")*/]
        public async Task<ApiBaseReturn<bool>> IsExistMacheName([FromQuery]decimal ID,string Name)
        {
            ApiBaseReturn<bool> returnVM = new ApiBaseReturn<bool>();
            returnVM.Result = false;
            if (!ErrorInfo.Status)
            {
                try
                {
                    #region 检查参数
                    if (ID>0&&!Name.IsNullOrEmpty())
                    {
                      var listStationConfig= await _repository.GetStationconfigMachineName();
                        if (listStationConfig!=null&&listStationConfig.Count>0)
                        {
                            listStationConfig.ForEach(c => {
                                if (c.ID!=ID&&c.VALUE.ToUpper().Equals(Name.ToUpper()))
                                {
                                    returnVM.Result = true;
                                }   
                            });
                        }
                    }
                    #endregion

                    #region 保存并返回

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
        /// 保存数据
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        [Authorize/*("Permission")*/]
        public async Task<ApiBaseReturn<bool>> SaveData([FromBody] PatchlineconfigModel model)
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
                        if (model!=null)
                        {
                            decimal resdata = await _repository.SaveLineAndStation(model);
                            if (resdata != -1)
                            {
                                returnVM.Result = true;
                            }
                            else
                            {
                                returnVM.Result = false;
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
    }
}