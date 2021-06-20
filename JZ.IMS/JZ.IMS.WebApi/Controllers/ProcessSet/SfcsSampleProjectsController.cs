/*
*┌──────────────────────────────────────────────────────────────┐
*│　描    述： 控制器                                                    
*│　作    者：嘉志科技                                              
*│　版    本：3.0   模板代码自动生成                                              
*│　创建时间：2020-04-15 11:59:54                           
*└──────────────────────────────────────────────────────────────┘
*┌──────────────────────────────────────────────────────────────┐
*│　命名空间： JZ.IMS.WebApi.Controllers                                   
*│　接口名称： ISfcsSampleProjectsController                                      
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
    /// 抽检方案名称维护和抽检方案配置控制器
    /// </summary>
	[Route("api/[controller]/[action]")]
	[ApiController]
	public class SfcsSampleProjectsController : BaseController
	{
		private readonly ISfcsSampleProjectsRepository _repository;
        private readonly ISfcsSampleProjectConfigRepository _configrepository;
        private readonly IMapper _mapper;
		private readonly IHttpContextAccessor _httpContextAccessor;
		private readonly IStringLocalizer<SfcsSampleProjectsController> _localizer;
		
		public SfcsSampleProjectsController(ISfcsSampleProjectsRepository repository, IMapper mapper, IHttpContextAccessor httpContextAccessor,
			IStringLocalizer<SfcsSampleProjectsController> localizer, ISfcsSampleProjectConfigRepository configrepository)
		{
			_repository = repository;
			_mapper = mapper;
			_httpContextAccessor = httpContextAccessor;
			_localizer = localizer;
            _configrepository = configrepository;

        }

        public class IndexVM
        {
            /// <summary>
            /// 抽检比例
            /// </summary>
            public List<dynamic> SampleRatioList { get; set; }
            /// <summary>
            /// 说明文档
            /// </summary>
            public List<string> DocumentList { get; set; }
        }

        /// <summary>
        /// 首页视图
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Authorize/*("Permission")*/]
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
                            SampleRatioList = await _repository.GetListByTable(" SP.id,SP.Meaning,SP.Lookup_Type,SP.Lookup_Code ", "SFCS_PARAMETERS SP ", " And LOOKUP_TYPE='SAMPLE_RATIO'   ORDER BY LOOKUP_TYPE "),
                            DocumentList=new List<string>() { @"1.当前抽检比例 sampleRatioCount是:后面， sampleCount是:前面，如果sampleRatioCount和sampleCount是为空的时候， 提醒 抽检比例参数设定不完整，请确认。
                                                                2. lookup_code 对应SAMPLE_RATIO(当前抽检比例)，UP_RATIO(连续Pass跳转比例)，DOWN_RATIO(连续Fail跳转比例)
                                                                3. orderno第一行为0 每增加一行添加10
                                                                4. 第一行的的当前抽检比例和连续fail跳转比例相同，最后一行的当前抽检比例和连续Pass跳转比例相同" },
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
        /// 抽验方案名称维护查询
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>	
        [HttpGet]
        [Authorize]
        public async Task<ApiBaseReturn<List<SfcsSampleProjectsListModel>>> LoadSampleProjectsData([FromQuery]SfcsSampleProjectsRequestModel model)
        {
            ApiBaseReturn<List<SfcsSampleProjectsListModel>> returnVM = new ApiBaseReturn<List<SfcsSampleProjectsListModel>>();
            if (!ErrorInfo.Status)
            {
                try
                {
                    #region 设置返回值

                    int count = 0;
                    string conditions = " WHERE ID > 0 ";
                    if (!model.PROJECT_NAME .IsNullOrWhiteSpace())
                    {
                        conditions += $"and instr(PROJECT_NAME, :PROJECT_NAME) > 0 ";
                    }
                    if (!model.DESCRIPTION.IsNullOrWhiteSpace())
                    {
                        conditions += $"and instr(DESCRIPTION, :DESCRIPTION) > 0 ";
                    }
                    if (!model.ENABLED.IsNullOrWhiteSpace())
                    {
                        conditions += $"and instr(ENABLED, :ENABLED) > 0 ";
                    }
                    var list = (await _repository.GetListPagedAsync(model.Page, model.Limit, conditions, "PROJECT_NAME desc", model)).ToList();
                    var viewList = new List<SfcsSampleProjectsListModel>();
                    list?.ForEach(x =>
                    {
                        var item = _mapper.Map<SfcsSampleProjectsListModel>(x);
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
        /// 查询方案配置的数据
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>	
        [HttpGet]
        [Authorize]
        public async Task<ApiBaseReturn<List<SfcsSampleProjectConfigListModel>>> LoadSampleProjectConfigData([FromQuery]SfcsSampleProjectConfigRequestModel model)
        {
            ApiBaseReturn<List<SfcsSampleProjectConfigListModel>> returnVM = new ApiBaseReturn<List<SfcsSampleProjectConfigListModel>>();
            if (!ErrorInfo.Status)
            {
                try
                {
                    #region 设置返回值

                    int count = 0;
                    string conditions = " WHERE ID > 0 ";
                    if (model.ID>0)
                    {
                        conditions += $"and instr(ID, :ID) > 0 ";
                    }
                    if (model.PROJECT_ID>0)
                    {
                        conditions += $"and instr(PROJECT_ID, :PROJECT_ID) > 0 ";
                    }
                    var list = (await _configrepository.GetListPagedAsync(model.Page, model.Limit, conditions, "Id desc", model)).ToList();
                    var viewList = new List<SfcsSampleProjectConfigListModel>();
                    list?.ForEach(x =>
                    {
                        var item = _mapper.Map<SfcsSampleProjectConfigListModel>(x);
                        //item.ENABLED = (item.ENABLED == "Y");
                        viewList.Add(item);
                    });

                    count = await _configrepository.RecordCountAsync(conditions, model);

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
        /// 保存抽检文字名称维护
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        [Authorize("Permission")]
        public async Task<ApiBaseReturn<bool>> SaveData([FromBody] SfcsSampleProjectsModel model)
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
        /// 保存产品抽检方案
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        [Authorize/*("Permission")*/]
        public async Task<ApiBaseReturn<bool>> SaveSampleProjectConfigData([FromBody] SfcsSampleProjectConfigModel model)
        {
            ApiBaseReturn<bool> returnVM = new ApiBaseReturn<bool>();
            if (!ErrorInfo.Status)
            {
                try
                {
                    #region 检查参数
                        List<List<SfcsSampleProjectConfigAddOrModifyModel>> sampleprojectconfig = null;
                    if (model.InsertRecords!=null||model.UpdateRecords!=null)
                    {
                        sampleprojectconfig = new List<List< SfcsSampleProjectConfigAddOrModifyModel >> ();
                    }
                    //插入
                    if (model.InsertRecords!=null&&!ErrorInfo.Status)
                    {
                        sampleprojectconfig.Add(model.InsertRecords);
                    }
                    //更新
                    if (model.UpdateRecords!=null&&!ErrorInfo.Status)
                    {
                        sampleprojectconfig.Add(model.UpdateRecords);
                    }
                    if (sampleprojectconfig!=null)
                    {
                        foreach (var itemlist in sampleprojectconfig)
                        {
                            foreach (var item in itemlist)
                            {
                                if (item.SAMPLE_RATIO == 0 || item.UP_RATIO == 0 || item.DOWN_RATIO == 0 || item.SAMPLE_RATIO_COUNT == 0 || item.SAMPLE_COUNT == 0 || item.DOWN_RATIO_LIMIT_COUNT == 0 || item.UP_RATIO_LIMIT_COUNT == 0)
                                {
                                    //抽检比例没有设定，请确认。
                                    ErrorInfo.Set(_localizer["Not_Setup_Sample_Ratio_Err"], MethodBase.GetCurrentMethod(), EnumErrorType.Warning);
                                }
                            }
                            //同一抽检方案中不可多次配置同一抽检比例，请确认。
                            if((itemlist.GroupBy(p=>p.SAMPLE_RATIO).Where(i=>i.Count()>1).Count()>0) && !ErrorInfo.Status)
                            { 
                                ErrorInfo.Set(_localizer["SameSampleRatioInSampleProject_Err"], MethodBase.GetCurrentMethod(), EnumErrorType.Warning); 
                            }
                        }
                    }
                    #endregion

                    #region 保存并返回

                    if (!ErrorInfo.Status)
                    {
                        decimal resdata = await _configrepository.SaveDataByTrans(model);
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
    }
}