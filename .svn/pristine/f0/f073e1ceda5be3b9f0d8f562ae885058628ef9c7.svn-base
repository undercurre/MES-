/*
*┌──────────────────────────────────────────────────────────────┐
*│　描    述： 控制器                                                    
*│　作    者：嘉志科技                                              
*│　版    本：3.0   模板代码自动生成                                              
*│　创建时间：2020-09-09 16:46:54                           
*└──────────────────────────────────────────────────────────────┘
*┌──────────────────────────────────────────────────────────────┐
*│　命名空间： JZ.IMS.WebApi.Controllers                                   
*│　接口名称： ISfcsCollectDefectsController                                      
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
    /// 不良维修 控制器
    /// </summary>
	[Route("api/[controller]/[action]")]
	[ApiController]
	public class SfcsCollectDefectsController : BaseController
	{
		private readonly ISfcsCollectDefectsRepository _repository;
        private readonly IMapper _mapper;
		private readonly IHttpContextAccessor _httpContextAccessor;
		private readonly IStringLocalizer<SfcsCollectDefectsController> _localizer;
		
		public SfcsCollectDefectsController(ISfcsCollectDefectsRepository repository, IMapper mapper, IHttpContextAccessor httpContextAccessor,
			IStringLocalizer<SfcsCollectDefectsController> localizer)
		{
			_repository = repository;
			_mapper = mapper;
			_httpContextAccessor = httpContextAccessor;
			_localizer = localizer;
		}


        #region

        //public class IndexVM
        //{
        //    /// <summary>
        //    /// 线体
        //    /// </summary>
        //    public List<dynamic> LINE_NAME { get; set; }

        //    /// <summary>
        //    /// 站点
        //    /// </summary>
        //    public List<dynamic> SITE_NAME { get; set; }

        //    /// <summary>
        //    /// 工序
        //    /// </summary>
        //    public List<dynamic> OPER_NAME { get; set; }
        //}


        //public class RepairVM {

        //    /// <summary>
        //    /// 原因代码
        //    /// </summary>
        //    public List<dynamic> REASON_CODE { get; set; }

        //    /// <summary>
        //    /// 排除故障
        //    /// </summary>
        //    public List<dynamic> RESPONSER { get; set; }
        //}

        

        ///// <summary>
        ///// 选择站点下拉框
        ///// </summary>
        ///// <returns></returns>
        //[HttpGet]
        //[Authorize("Permission")]
        //public async Task<ApiBaseReturn<IndexVM>> Index()
        //{
        //     ApiBaseReturn<IndexVM> returnVM = new ApiBaseReturn<IndexVM>();
        //    if (!ErrorInfo.Status)
        //    {
        //        try
        //        {
        //            #region 设置返回值

        //            if (!ErrorInfo.Status)
        //            {
        //                returnVM.Result = new IndexVM
        //                {
        //                    LINE_NAME = await _repository.GetLINENAME(),
        //                    SITE_NAME=await _repository.GetListByTable("ID,OPERATION_SITE_NAME", "Sfcs_Operation_Sites", "and ENABLED='Y' "),
        //                    OPER_NAME= await _repository.GetListByTable("ID,OPERATION_NAME", "Sfcs_Operations", "and ENABLED='Y' "),
        //                };
        //            }

        //            #endregion
        //        }
        //        catch (Exception ex)
        //        {
        //            ErrorInfo.Set(ex.Message, MethodBase.GetCurrentMethod(), EnumErrorType.Error);
        //        }
        //    }

        //    #region 如果出现错误，则写错误日志并返回错误内容

        //    WriteLog(ref returnVM);

        //    #endregion

        //    return returnVM;
        //}

        ///// <summary>
        ///// 维修时选择的下拉框
        ///// </summary>
        ///// <returns></returns>
        //[HttpGet]
        //[Authorize("Permission")]
        //public async Task<ApiBaseReturn<RepairVM>> GetRepairVM()
        //{
        //    ApiBaseReturn<RepairVM> returnVM = new ApiBaseReturn<RepairVM>();
        //    if (!ErrorInfo.Status)
        //    {
        //        try
        //        {
        //            #region 设置返回值

        //            if (!ErrorInfo.Status)
        //            {
        //                returnVM.Result = new RepairVM
        //                {
        //                    REASON_CODE = await _repository.GetReasonCodeList(),
        //                    RESPONSER=await _repository.GetResponserList(),
        //                };
        //            }

        //            #endregion
        //        }
        //        catch (Exception ex)
        //        {
        //            ErrorInfo.Set(ex.Message, MethodBase.GetCurrentMethod(), EnumErrorType.Error);
        //        }
        //    }

        //    #region 如果出现错误，则写错误日志并返回错误内容

        //    WriteLog(ref returnVM);

        //    #endregion

        //    return returnVM;
        //}

        #endregion


        /// <summary>
        /// 获取线体下拉框
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Authorize]
        public async Task<ApiBaseReturn<List<dynamic>>> GetLineNameList([FromQuery] DropDownBoxRequestModel model)
        {
            ApiBaseReturn<List<dynamic>> returnVM = new ApiBaseReturn<List<dynamic>>();
            if (!ErrorInfo.Status)
            {
                try
                {
                    #region 设置返回值

                    var lists = await _repository.GetLINENAMEList(model);
                    returnVM.Result = lists.data;
                    returnVM.TotalCount = lists.count;

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
        /// 获取站点
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Authorize]
        public async Task<ApiBaseReturn<List<dynamic>>> GetSiteNameList([FromQuery] SiteNameRequestModel model)
        {
            ApiBaseReturn<List<dynamic>> returnVM = new ApiBaseReturn<List<dynamic>>();
            if (!ErrorInfo.Status)
            {
                try
                {
                    #region 设置返回值

                    var lists = await _repository.GetSITENAMEList(model);
                    returnVM.Result = lists.data;
                    returnVM.TotalCount = lists.count;

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
        /// 获取维修工序
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Authorize]
        public async Task<ApiBaseReturn<List<dynamic>>> GetOperNameList([FromQuery] DropDownBoxRequestModel model)
        {
            ApiBaseReturn<List<dynamic>> returnVM = new ApiBaseReturn<List<dynamic>>();
            if (!ErrorInfo.Status)
            {
                try
                {
                    #region 设置返回值

                    var lists = await _repository.GetOPERNAMEList(model);
                    returnVM.Result = lists.data;
                    returnVM.TotalCount = lists.count;

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
        /// 获取SN数据
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Authorize]
        public async Task<ApiBaseReturn<List<dynamic>>> GetSnDataList([FromQuery] DropDownBoxRequestModel model)
        {
            ApiBaseReturn<List<dynamic>> returnVM = new ApiBaseReturn<List<dynamic>>();
            if (!ErrorInfo.Status)
            {
                try
                {
                    #region 设置返回值

                    var lists = await _repository.GetSnDataList(model);
                    returnVM.Result = lists.data;
                    returnVM.TotalCount = lists.count;

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
        /// 获取原因代码
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Authorize]
        public async Task<ApiBaseReturn<List<dynamic>>> GetReasonCodeList([FromQuery] DropDownBoxRequestModel model)
        {
            ApiBaseReturn<List<dynamic>> returnVM = new ApiBaseReturn<List<dynamic>>();
            if (!ErrorInfo.Status)
            {
                try
                {
                    #region 设置返回值

                    var lists = await _repository.GetReasonCodeList(model);
                    returnVM.Result = lists.data;
                    returnVM.TotalCount = lists.count;

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
        /// 获取排除故障
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Authorize]
        public async Task<ApiBaseReturn<List<dynamic>>> GetResponserList([FromQuery] DropDownBoxRequestModel model)
        {
            ApiBaseReturn<List<dynamic>> returnVM = new ApiBaseReturn<List<dynamic>>();
            if (!ErrorInfo.Status)
            {
                try
                {
                    #region 设置返回值

                    var lists = await _repository.GetResponserList(model);
                    returnVM.Result = lists.data;
                    returnVM.TotalCount = lists.count;

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
        ///  根据线体、站位、工序获取站位信息
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpGet]
        [Authorize]
        public async Task<ApiBaseReturn<List<dynamic>>> GetRepairSiteData([FromQuery] SfcsCollectRepairSiteRequestModel model)
        {
            ApiBaseReturn<List<dynamic>> returnVM = new ApiBaseReturn<List<dynamic>>();
            if (!ErrorInfo.Status)
            {
                try
                {
                    #region 设置返回值

                    var lists = await _repository.GetRepairSiteData(model);
                    returnVM.Result = lists.data;
                    returnVM.TotalCount = lists.count;

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
        /// 根据工序ID获取未维修数量
        /// </summary>
        /// <param name="OPER_ID">选择的工序ID</param>
        /// <returns></returns>
        [HttpGet]
        [Authorize]
        public async Task<ApiBaseReturn<decimal>> GetRefreshUnrepairedQty(decimal? OPER_ID)
        {
            ApiBaseReturn<decimal> returnVM = new ApiBaseReturn<decimal>();

            if (!ErrorInfo.Status)
            {
                try
                {
                    #region 设置返回值

                    if (!ErrorInfo.Status)
                    {

                        returnVM.Result = await _repository.GetRefreshUnrepairedQty(OPER_ID);
                        returnVM.TotalCount = 1;
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
        /// 根据流水号获取不良维修信息
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpGet]
        [Authorize]
        public async Task<ApiBaseReturn<List<SfcsCollectBadDataListModel>>> GetBadDataBySN([FromQuery] SfcsCollectBadRequestModel model)
        {
            ApiBaseReturn<List<SfcsCollectBadDataListModel>> returnVM = new ApiBaseReturn<List<SfcsCollectBadDataListModel>>();
            if (!ErrorInfo.Status)
            {
                try
                {
                    #region 设置返回值

                    var lists = await _repository.GetDefectDataBySN(model);
                    returnVM.Result = lists;
                    returnVM.TotalCount = lists.Count();

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
        public async Task<ApiBaseReturn<string>> SaveRepairData([FromBody] SfcsCollectDefectsModel model)
        {
            ApiBaseReturn<string> returnVM = new ApiBaseReturn<string>();
            if (!ErrorInfo.Status)
            {
                try
                {
                    #region 检查参数
                    if(!ErrorInfo.Status)
                    {
                        if(!model.COLLECT_DEFECT_ID.HasValue)
                        {
                            //采集ID不能为空!
                            ErrorInfo.Set(_localizer["SFCS_COLLECT_DEFECT_ID_NOTNULL"], MethodBase.GetCurrentMethod(), EnumErrorType.Error);
                        }
                        else
                        {
                            var needRepair = await _repository.CheckCollectDefectNeedRepair(model.COLLECT_DEFECT_ID.Value);
                            if (!needRepair)
                            {
                                //此不良代码已维修完，请勿重复操作!
                                ErrorInfo.Set(_localizer["SFCS_REPAIR_CHECK"], MethodBase.GetCurrentMethod(), EnumErrorType.Error);
                            }
                        }
                    }
                    #endregion

                    #region 保存并返回

                    if (!ErrorInfo.Status)
                    {
                        returnVM.Result = await _repository.SaveRepairData(model);
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
        /// 保存报废功能
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        [Authorize]
        public async Task<ApiBaseReturn<bool>> SaveScrappedData([FromBody] DefectsModel model)
        {
            ApiBaseReturn<bool> returnVM = new ApiBaseReturn<bool>();
            if (!ErrorInfo.Status)
            {
                try
                {
                    #region 检查参数
                    if (!ErrorInfo.Status)
                    {
                        if (model.SN.IsNullOrWhiteSpace())
                        {
                            //请输入SN信息。
                            throw new Exception(_localizer["PLEASE_SN_INFOMATION"]);
                        }
                      var modelList= (await _repository.GetListByTableEX<SfcsRuncard>("*", "SFCS_RUNCARD"," AND SN=:SN",new { SN=model.SN}));
                        if (modelList.Count<=0)
                        {
                            //请输入正确的SN 
                            throw new Exception(_localizer["PLEASE_CORRECT_SN"]);
                        }
                        else if(modelList.Count(c=>c.STATUS==17)>0)
                        {
                            //SN已经报废过,请输入正确的SN
                            throw new Exception(_localizer["ERROR_SCRAPPED_SN"]);
                        }
                    }
                    #endregion

                    #region 保存并返回

                    if (!ErrorInfo.Status)
                    {
                        returnVM.Result = await _repository.SaveScrappedData(model.SN);
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




        /*
         
        /// <summary>
        /// 查询数据
        /// 搜索按钮对应的处理也是这个方法
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>	
        [HttpGet]
        [Authorize]
        public async Task<ApiBaseReturn<List<SfcsCollectDefectsListModel>>> LoadData([FromQuery]SfcsCollectDefectsRequestModel model)
        {
            ApiBaseReturn<List<SfcsCollectDefectsListModel>> returnVM = new ApiBaseReturn<List<SfcsCollectDefectsListModel>>();
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
                    var viewList = new List<SfcsCollectDefectsListModel>();
                    list?.ForEach(x =>
                    {
                        var item = _mapper.Map<SfcsCollectDefectsListModel>(x);
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
        /// 当前ID是否已被使用 
        /// </summary>
        /// <param name="id">id</param>
        /// <returns></returns>
        [HttpGet]
        [Authorize]
        public async Task<ApiBaseReturn<bool>> ItemIsByUsed(decimal id)
        {
            ApiBaseReturn<bool> returnVM = new ApiBaseReturn<bool>();
            bool result = false;

            if (!ErrorInfo.Status)
            {
                try
                {
                    #region 设置返回值

                    if (!ErrorInfo.Status)
                    {
                        if (id > 0)
                        {
                            result = await _repository.ItemIsByUsed(id);
                        }
                        returnVM.Result = result;
                        returnVM.TotalCount = 1;
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
        /// 保存数据
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        [Authorize("Permission")]
        public async Task<ApiBaseReturn<bool>> SaveData([FromBody] SfcsCollectDefectsModel model)
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
                        var count = await _repository.DeleteAsync(id);
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




        **/

        #region 内部类
        /// <summary>
        /// 
        /// </summary>
        public class DefectsModel
        {
            public string SN { get; set; }
        }
        #endregion



    }
}