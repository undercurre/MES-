/**
*┌──────────────────────────────────────────────────────────────┐
*│　描    述：手插件物料状态监听表 控制器                                                    
*│　作    者：嘉志科技                                              
*│　版    本：2.0   模板代码自动生成                                              
*│　创建时间：2019-11-19 20:26:22                           
*└──────────────────────────────────────────────────────────────┘
*┌──────────────────────────────────────────────────────────────┐
*│　命名空间： JZ.IMS.Admin.Controllers                                   
*│　接口名称： IMesHiMaterialListenController                                      
*└──────────────────────────────────────────────────────────────┘
*/

using FluentValidation.Results;
using JZ.IMS.Core.Helper;
using JZ.IMS.IRepository;
using JZ.IMS.IServices;
using JZ.IMS.ViewModels;
using JZ.IMS.WebApi.Public;
using JZ.IMS.WebApi.Validation;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;

namespace JZ.IMS.WebApi.Controllers
{
    /// <summary>
	/// 产线上料看板管理 控制器  
	/// </summary>
	[Route("api/[controller]/[action]")]
    [ApiController]
    public class MesHiMaterialListenController : BaseController
    {
		private readonly IMesHiMaterialListenService _service;
		private readonly IKanbanService _serviceK;

		public MesHiMaterialListenController(IMesHiMaterialListenService service, IKanbanService serviceK)
		{
			_service = service;
			_serviceK = serviceK;
		}

        /// <summary>
        /// 首页视图
        /// </summary>
        /// <returns></returns>
        //[AllowAnonymous]
        //public IActionResult Index()
        //{
        //	return View();
        //}

        /// <summary>
        /// 查询所有
        /// 搜索按钮对应的处理也是这个方法
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>		
        //[AllowAnonymous]
        //public async Task<string> LoadData([FromQuery]MesHiMaterialListenRequestModel model)
        //{
        //	return JsonHelper.ObjectToJSON(await _service.LoadDataAsync(model));
        //}
        /// <summary>
        /// 获取产线低水位信息
        /// </summary>
        /// <param name="lineId"></param>
        /// <returns></returns>
        [HttpGet]
        [Authorize]
		public async Task<ApiBaseReturn<string>> GetMesHiMaterialListenReelsModels(decimal lineId)
		{
            ApiBaseReturn<string> returnVM = new ApiBaseReturn<string>();
            if (!ErrorInfo.Status)
            {
                try
                {
                    #region 设置返回值

                    returnVM.Result = JsonHelper.ObjectToJSON(await _service.GetMesHiMaterialListenReelsModels(lineId));
                  
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
        /// 获取备料信息
        /// </summary>
        /// <param name="lineId"></param>
        /// <returns></returns>
        [HttpGet]
        [Authorize]
		public async Task<ApiBaseReturn<string>> GetMesAddMaterialListModels([FromQuery] decimal lineId)
		{

            ApiBaseReturn<string> returnVM = new ApiBaseReturn<string>();
            if (!ErrorInfo.Status)
            {
                try
                {
                    #region 设置返回值

                    returnVM.Result = JsonHelper.ObjectToJSON(await _service.GetMesAddMaterialModels(lineId));

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
        /// 根据线别ID获取到上料看板预警值
        /// </summary>
        /// <param name="lineId">线别ID</param>
        /// <returns></returns>
        [HttpGet]
        [Authorize]
        public async Task<ApiBaseReturn<decimal>> GetWarnValueByLineId(int lineId)
        {
            ApiBaseReturn<decimal> returnVM = new ApiBaseReturn<decimal>();
            if (!ErrorInfo.Status)
            {
                try
                {
                    #region 设置返回值

                    returnVM.Result = await _service.GetWarnValueByLineId(lineId);

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
        /// 看板预警线
        /// </summary>
        /// <param name="lineId"></param>
        /// <returns></returns>
        [HttpGet]
        [Authorize]
        public async Task<ApiBaseReturn<string>> CheckLine(int lineId)
		{
            ApiBaseReturn<string> returnVM = new ApiBaseReturn<string>();
            if (!ErrorInfo.Status)
            {
                try
                {
                    #region 设置返回值

                    returnVM.Result = JsonHelper.ObjectToJSON(await _serviceK.CheckLineAsync(lineId));

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

		//[AllowAnonymous]
		//public async Task<string> GetKanbanWoData(int lineId)
		//{
		//	return JsonHelper.ObjectToJSON(await _serviceK.GetKanbanWoDataAsync(lineId));
		//}

		/// <summary>
		/// 添加或修改视图
		/// </summary>
		/// <returns></returns>
		//[HttpGet]
		//[AllowAnonymous]
		//public IActionResult AddOrModify()
		//{
		//	return View();
		//}

		/// <summary>
		/// 添加或修改的相关操作
		/// </summary>
		/// <param name="item">请求体中的数据的映射</param>
		/// <returns>JSON格式的响应结果</returns>
		//[HttpPost]
		//[ValidateAntiForgeryToken]
		//[Route("/MesHiMaterialListen/AddOrModifySave")]
		//public async Task<string> AddOrModifyAsync([FromForm]MesHiMaterialListenAddOrModifyModel item)
		//{
		//	BaseResult result = new BaseResult();
		//	result = await _service.AddOrModifyAsync(item);

		//	return JsonHelper.ObjectToJSON(result);
		//}

		///// <summary>
		///// 通过ID删除记录
		///// </summary>
		///// <param name="Id">要删除的记录的ID</param>
		///// <returns>JSON格式的响应结果</returns>
		//[HttpPost]
		//[ValidateAntiForgeryToken]
		//[Route("/MesHiMaterialListen/DeleteOneById")]
		//public async Task<string> DeleteOneById(decimal Id)
		//{
		//	BaseResult result = new BaseResult();
		//	result = await _service.DeleteAsync(Id);
		//	return JsonHelper.ObjectToJSON(result);
		//}

		///// <summary>
		///// 通过ID更改激活状态
		///// </summary>
		///// <param name="item"></param>
		///// <returns></returns>
		//[HttpPost]
		//[ValidateAntiForgeryToken]
		//[Route("/MesHiMaterialListen/ChangeEnabled")]
		//public async Task<string> ChangeEnabledAsync([FromForm]ChangeStatusModel item)
		//{
		//	var result = new BaseResult();
		//	ChangeStatusModelValidation validationRules = new ChangeStatusModelValidation();
		//	ValidationResult results = validationRules.Validate(item);
		//	if (results.IsValid)
		//	{
		//		result = await _service.ChangeEnableStatusAsync(item);
		//	}
		//	else
		//	{
		//		result.ResultCode = ResultCodeAddMsgKeys.CommonModelStateInvalidCode;
		//		result.ResultMsg = results.ToString("||");
		//	}
		//	return JsonHelper.ObjectToJSON(result);
		//}
	}
}