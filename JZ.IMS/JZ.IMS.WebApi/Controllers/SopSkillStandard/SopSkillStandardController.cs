/**
*┌──────────────────────────────────────────────────────────────┐
*│　描    述： 控制器                                                    
*│　作    者：嘉志科技                                              
*│　版    本：2.0   模板代码自动生成                                              
*│　创建时间：2020-03-04 15:39:22                           
*└──────────────────────────────────────────────────────────────┘
*┌──────────────────────────────────────────────────────────────┐
*│　命名空间： JZ.IMS.Admin.Controllers                                   
*│　接口名称： IImsReelController                                      
*└──────────────────────────────────────────────────────────────┘
*/

using FluentValidation.Results;
using JZ.IMS.Core.Extensions;
using JZ.IMS.Core.Helper;
using JZ.IMS.IRepository;
using JZ.IMS.IServices;
using JZ.IMS.ViewModels;
using JZ.IMS.WebApi.Common;
using JZ.IMS.WebApi.Public;
using JZ.IMS.WebApi.Validation;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace JZ.IMS.WebApi.Controllers
{
    /// <summary>
    /// 工序评判标准 控制器
    /// </summary>
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class SopSkillStandardController : BaseController
    {
        private readonly IImsReelRepository _repository;
        private readonly IMstBom2DetailQtyRepository _repositorymbdq;
        private readonly IStringLocalizer<SopSkillStandardController> _localizer;
		private readonly IStringLocalizer<SfcsEquipContentConfController> _equiplocalizer;
		private readonly ISopSkillStandardService _service;

		public SopSkillStandardController(IImsReelRepository repository, IMstBom2DetailQtyRepository repositorymbdq, IStringLocalizer<SopSkillStandardController> localizer, ISopSkillStandardService service, IStringLocalizer<SfcsEquipContentConfController> equiplocalizer)
        {
            _repository = repository;
            _repositorymbdq = repositorymbdq;
            _localizer = localizer;
			_service = service;
			_equiplocalizer = equiplocalizer;
		}

		/// <summary>
		/// 首页视图
		/// </summary>
		/// <returns></returns>
		[HttpGet]
		[Authorize("Permission")]
		public ApiBaseReturn<IndexVM> Index()
		{
			ApiBaseReturn<IndexVM> returnVM = new ApiBaseReturn<IndexVM>();
			returnVM.Result = new IndexVM();
			if (!ErrorInfo.Status)
			{
				try
				{
					#region 设置返回值

					if (!ErrorInfo.Status)
					{
						returnVM.Result.TrainList = _service.GetTrainData();
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
		/// 查询所有
		/// 搜索按钮对应的处理也是这个方法
		/// </summary>
		/// <param name="model"></param>
		/// <returns></returns>		
		[HttpGet]
		[Authorize]
		public async Task<string> LoadData([FromQuery] SopSkillStandardRequestModel model)
		{
			return JsonHelper.ObjectToJSON(await _service.LoadDataAsync(model));
		}

		/// <summary>
		/// 添加或修改的相关操作
		/// </summary>
		/// <param name="item">请求体中的数据的映射</param>
		/// <returns>JSON格式的响应结果</returns>
		[HttpPost]
		[Authorize]
		public async Task<string> AddOrModifySave([FromForm] SopSkillStandardAddOrModifyModel item)
		{
			BaseResult result = new BaseResult();
			result = await _service.AddOrModifyAsync(item);
			return JsonHelper.ObjectToJSON(result);
		}

		/// <summary>
		/// 通过ID删除记录
		/// </summary>
		/// <param name="Id">要删除的记录的ID</param>
		/// <returns>JSON格式的响应结果</returns>
		[HttpPost]
		[Authorize]
		public async Task<string> DeleteOneById(decimal Id)
		{
			BaseResult result = new BaseResult();
			result = await _service.DeleteAsync(Id);
			return JsonHelper.ObjectToJSON(result);
		}

		/// <summary>
		/// 通过ID更改激活状态
		/// </summary>
		/// <param name="item"></param>
		/// <returns></returns>
		[HttpPost]
		[Authorize]
		public async Task<string> ChangeEnabled([FromForm] ChangeStatusModel item)
		{
			var result = new BaseResult();
			ChangeStatusModelValidation validationRules = new ChangeStatusModelValidation(_equiplocalizer);
			ValidationResult results = validationRules.Validate(item);
			if (results.IsValid)
			{
				result = await _service.ChangeEnableStatusAsync(item);
			}
			else
			{
				result.ResultCode = ResultCodeAddMsgKeys.CommonModelStateInvalidCode;
				result.ResultMsg = results.ToString("||");
			}
			return JsonHelper.ObjectToJSON(result);
		}

		/// <summary>
		/// 获取工序数据
		/// </summary>
		/// <param name="model">查询对象</param>
		/// <returns></returns>
		[HttpGet]
		[Authorize]
		public async Task<string> LoadOperationData([FromQuery] SopSkillStandardRequestModel model)
		{
			return JsonHelper.ObjectToJSON(await _service.LoadOperationData(model));
		}

		/// <summary>
		/// 获取工序技能评判标准数据
		/// </summary>
		/// <param name="ID">工序ID</param>
		/// <returns></returns>
		[HttpGet]
		[Authorize]
		public async Task<string> LoadSkillStandardData(decimal ID)
		{
			return JsonHelper.ObjectToJSON(await _service.LoadSkillStandardData(ID));
		}

        
    }
	#region 内部类
	public class IndexVM
    {
		/// <summary>
		/// 获取技能名称数据
		/// </summary>
		public List<String> TrainList { get; set; }

	}
	#endregion
}