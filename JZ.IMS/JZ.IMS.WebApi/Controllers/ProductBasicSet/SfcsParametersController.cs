/*
*┌──────────────────────────────────────────────────────────────┐
*│　描    述： 控制器                                                    
*│　作    者：嘉志科技                                              
*│　版    本：3.0   模板代码自动生成                                              
*│　创建时间：2020-03-30 10:44:46                           
*└──────────────────────────────────────────────────────────────┘
*┌──────────────────────────────────────────────────────────────┐
*│　命名空间： JZ.IMS.WebApi.Controllers                                   
*│　接口名称： ISfcsParametersController                                      
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

namespace JZ.IMS.WebApi.Controllers
{
    /// <summary>
    /// 生产字典管理控制器
    /// </summary>
	[Route("api/[controller]/[action]")]
    [ApiController]
    public class SfcsParametersController : BaseController
    {
        private readonly ISfcsParametersRepository _repository;
        private readonly IMapper _mapper;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IStringLocalizer<SfcsParametersController> _localizer;

        public SfcsParametersController(ISfcsParametersRepository repository, IMapper mapper, IHttpContextAccessor httpContextAccessor,
            IStringLocalizer<SfcsParametersController> localizer)
        {
            _repository = repository;
            _mapper = mapper;
            _httpContextAccessor = httpContextAccessor;
            _localizer = localizer;
        }

        /// <summary>
        /// 首页视图
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Authorize("Permission")]
        public async Task<ApiBaseReturn<bool>> Index()
        {
            ApiBaseReturn<bool> returnVM = new ApiBaseReturn<bool>();
            if (!ErrorInfo.Status)
            {
                try
                {
                    #region 设置返回值

                    if (!ErrorInfo.Status)
                    {
                        returnVM.Result = true;
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
        /// 获取类型下拉
        /// 查询请传一个key值即可
        /// 传其他没有用
        /// </summary>
        /// <param name="mode"></param>
        /// <returns></returns>
        [HttpGet]
        [Authorize]
        public async Task<ApiBaseReturn<List<object>>> GetStatusList([FromQuery] SfcsParametersRequestModel mode)
        {
            ApiBaseReturn<List<object>> returnVM = new ApiBaseReturn<List<object>>();
            returnVM.TotalCount = 0;
            if (!ErrorInfo.Status)
            {
                try
                {
                    var result = await _repository.GetDistinctList(mode);
                    returnVM.Result = result.data == null ? "" : result.data;
                    if (result.data != null)
                    {
                        returnVM.TotalCount = result.count == 0 ? 0 : result.count;
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
        /// 查询数据
        /// 搜索按钮对应的处理也是这个方法
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>	
        [HttpGet]
        [Authorize]
        public async Task<ApiBaseReturn<List<SfcsParametersListModel>>> LoadData([FromQuery] SfcsParametersRequestModel model)
        {
            ApiBaseReturn<List<SfcsParametersListModel>> returnVM = new ApiBaseReturn<List<SfcsParametersListModel>>();
            if (!ErrorInfo.Status)
            {
                try
                {
                    #region 设置返回值

                    int count = 0;
                    string conditions = " WHERE ID > 0 ";
                    if (!model.LOOKUP_TYPE.IsNullOrWhiteSpace())
                    {
                        conditions += $"and instr(UPPer(LOOKUP_TYPE), UPPer(:LOOKUP_TYPE)) > 0 ";
                    }
                    if (!model.NAME.IsNullOrWhiteSpace())
                    {
                        conditions += $"and instr(NAME, :NAME) > 0 ";
                    }
                    if (model.LOOKUP_CODE > 0)
                    {
                        conditions += $"and instr(LOOKUP_CODE, :LOOKUP_CODE) > 0 ";
                    }
                    if (!model.MEANING.IsNullOrWhiteSpace())
                    {
                        conditions += $"and instr(MEANING, :MEANING) > 0 ";
                    }
                    if (!model.DESCRIPTION.IsNullOrWhiteSpace())
                    {
                        conditions += $"and instr(DESCRIPTION, :DESCRIPTION) > 0 ";
                    }
                    if (!model.CHINESE.IsNullOrWhiteSpace())
                    {
                        conditions += $"and instr(CHINESE, :CHINESE) > 0 ";
                    }
                    if (!model.ENABLED.IsNullOrWhiteSpace())
                    {
                        conditions += $"and instr(ENABLED, :ENABLED) > 0 ";
                    }
                    var list = (await _repository.GetListPagedAsync(model.Page, model.Limit, conditions, "Id desc", model)).ToList();
                    var viewList = new List<SfcsParametersListModel>();
                    list?.ForEach(x =>
                    {
                        var item = _mapper.Map<SfcsParametersListModel>(x);
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
        /// 保存数据
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        [Authorize("Permission")]
        public async Task<ApiBaseReturn<bool>> SaveData([FromBody] SfcsParametersModel model)
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
                    if (ex.Message != null && ex.Message.IndexOf("SFCS_PARAMETERS_TYPE_CODE") != -1)
                    {
                        ErrorInfo.Set(_localizer["SFCS_PARAMETERS_TYPE_CODE"], MethodBase.GetCurrentMethod(), EnumErrorType.Error);
                    }
                    else if (ex.Message != null && ex.Message.IndexOf("SFCS_PARAMETERS_TYPE_MEANING") != -1)
                    {
                        ErrorInfo.Set(_localizer["SFCS_PARAMETERS_TYPE_MEANING"], MethodBase.GetCurrentMethod(), EnumErrorType.Error);
                    }
                    else { ErrorInfo.Set(ex.Message, MethodBase.GetCurrentMethod(), EnumErrorType.Error); }

                }
            }

            #region 如果出现错误，则写错误日志并返回错误内容

            WriteLog(ref returnVM);

            #endregion

            return returnVM;
        }

        /// <summary>
        /// 获取连接设备信息
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public async Task<ApiBaseReturn<MachineTypeClass>> GetMachineType()
        {
            ApiBaseReturn<MachineTypeClass> returnVM = new ApiBaseReturn<MachineTypeClass>();
            returnVM.Result = new MachineTypeClass();
            if (!ErrorInfo.Status)
            {
                try
                {
                    #region 设置返回值

                    if (!ErrorInfo.Status)
                    {
                        returnVM.Result.MachineDevType = await _repository.GetListByTableEX<dynamic>("LOOKUP_CODE,MEANING", "SFCS_PARAMETERS", " AND LOOKUP_TYPE='MES_MACHINE_DEV_TYPE' AND ENABLED='Y'");
                        returnVM.Result.LinkeType = await _repository.GetListByTableEX<dynamic>("LOOKUP_CODE,MEANING", "SFCS_PARAMETERS", " AND LOOKUP_TYPE='MES_LINKE_TYPE' AND ENABLED='Y'");
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
        /// 获取连接设备信息
        /// SITE_ID--机器设备
        /// MACHINE_TYPE--设备类型
        /// LINKE_TYPE--连接方式
        /// KEY--项
        /// VALUE--内容
        /// DESCRIPTION--描述
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public async Task<ApiBaseReturn<List<MachineDetailReturnModel>>> GetMachineDetailInfo([FromQuery] MachineDetailRquestModel model)
        {
            ApiBaseReturn<List<MachineDetailReturnModel>> returnVM = new ApiBaseReturn<List<MachineDetailReturnModel>>();
            returnVM.Result = new List<MachineDetailReturnModel>();
            if (!ErrorInfo.Status)
            {
                try
                {
                    #region 设置返回值
                    string conditions = "";
                    //if (model.MachineDevType > 0)
                    //{
                    //    conditions += " AND SMC.MACHINE_TYPE=:MACHINETYPE ";
                    //}
                    //if (model.LinkeType > 0)
                    //{
                    //    conditions += " AND SMC.LINKE_TYPE=:LINKETYPE";
                    //}
                    if (model.SiteID > 0)
                    {
                        conditions += " AND SMC.SITE_ID=:SITEID";
                    }
                    if (!ErrorInfo.Status)
                    {
                        var configViewModel = await _repository.GetListByTableEX<MachineDetailReturnModel>("SMC.ID MSTID,SMC.SITE_ID SiteID,SMC.MACHINE_TYPE MachineDevType,SMC.LINKE_TYPE LinkeType", "SFCS_SITE_MACHINE_CONFIG SMC", " " + conditions, new
                        {
                            SITEID = model.SiteID
                        });

                        if (configViewModel != null && configViewModel.Count > 0)
                        {
                            foreach (var item in configViewModel)
                            {
                                item.itemList = new List<MachineDetailReturnModelEx>();
                                item.itemList = await _repository.GetListByTableEX<MachineDetailReturnModelEx>("SMD.KEY,SMD.VALUE,SMD.DESCRIPTION", "SFCS_SITE_MACHINE_DETAIL SMD", " AND SMD.MST_ID=:MSTID ", new
                                {
                                    MSTID = item.MSTID
                                });
                            }
                        }
                        returnVM.Result = configViewModel;
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
        /// 获取连接设备信息
        /// MACHINE_TYPE--设备类型
        /// LINKE_TYPE--连接方式
        /// KEY--项
        /// VALUE--内容
        /// DESCRIPTION--描述
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        //[Authorize(GlobalVariables.Permission)]

        public async Task<ApiBaseReturn<bool>> SaveMachineDetailInfo([FromBody] SaveMachineConfigModel model)
        {
            ApiBaseReturn<bool> returnVM = new ApiBaseReturn<bool>();
            returnVM.Result = false;

            if (!ErrorInfo.Status)
            {
                try
                {
                    #region 设置返回值
                    if (model.MachineDevType <= 0 || model.LinkeType <= 0 || model.ItemList.Count <= 0)
                    {
                        return returnVM;
                    }

                    if (!ErrorInfo.Status)
                    {
                        returnVM.Result = await _repository.SaveMachineByTrans(model);
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

        #region 内部类
        public class MachineTypeClass
        {
            /// <summary>
            /// 设备
            /// </summary>
            public List<dynamic> MachineDevType { get; set; }
            /// <summary>
            /// 连接形式
            /// </summary>
            public List<dynamic> LinkeType { get; set; }
        }

        public class MachineDetailRquestModel
        {
            /// <summary>
            /// 
            /// </summary>
            public decimal SiteID { get; set; }
            /// <summary>
            /// 设备
            /// </summary>
            public decimal MachineDevType { get; set; }
            /// <summary>
            /// 连接类型
            /// </summary>
            public decimal LinkeType { get; set; }
        }

        public class MachineDetailReturnViewModel
        {
            /// <summary>
            /// 
            /// </summary>
            public decimal SITE_ID { get; set; }
            /// <summary>
            /// 设备
            /// </summary>
            public decimal MACHINE_TYPE { get; set; }
            /// <summary>
            /// 连接类型
            /// </summary>
            public decimal LINKE_TYPE { get; set; }
            /// <summary>
            /// 连接类型
            /// </summary>
            public decimal KEY { get; set; }
            /// <summary>
            /// 内容
            /// </summary>
            public string VALUE { get; set; }
            /// <summary>
            /// 描述
            /// </summary>
            public string DESCRIPTION { get; set; }

        }

        public class MachineDetailReturnModel
        {
            public decimal MSTID { get; set; }
            /// <summary>
            /// 
            /// </summary>
            public decimal SiteID { get; set; }
            /// <summary>
            /// 设备
            /// </summary>
            public decimal MachineDevType { get; set; }
            /// <summary>
            /// 连接类型
            /// </summary>
            public decimal LinkeType { get; set; }

            /// <summary>
            /// 各项
            /// </summary>
            public List<MachineDetailReturnModelEx> itemList { get; set; }
        }

        public class MachineDetailReturnModelEx
        {
            /// <summary>
            /// KEY--项
            /// </summary>
            public decimal KEY { get; set; }

            /// <summary>
            /// VALUE--内容
            /// </summary>
            public string VALUE { get; set; }

            /// <summary>
            /// DESCRIPTION--描述
            /// </summary>
            public string DESCRIPTION { get; set; }
        }

        #endregion

    }
}