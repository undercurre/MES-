using JZ.IMS.Core.Extensions;
using JZ.IMS.Core.Helper;
using JZ.IMS.Core.Utilities;
using JZ.IMS.IRepository;
using JZ.IMS.IServices;
using JZ.IMS.Models;
using JZ.IMS.ViewModels;
using JZ.IMS.WebApi.Common;
using JZ.IMS.WebApi.Public;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.IO;
using System.Linq;
using System.Net.Http.Headers;
using System.Reflection;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace JZ.IMS.WebApi.Controllers
{
    /// <summary>
    /// 用户管理控制器
    /// </summary>
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class ManagerController : BaseController
    {
        private readonly IManagerService _service;
        private readonly IManagerRoleService _roleService;
        private readonly IStringLocalizer<ManagerController> _localizer;
        private readonly IHttpContextAccessor _httpContextAccessor;

        private readonly IImportDtlRepository _repository;
        private readonly IStringLocalizer<ImportExcelController> _localizerimport;

        public ManagerController(IStringLocalizer<ManagerController> localizer, IManagerService service, IManagerRoleService roleService,
            IHttpContextAccessor httpContextAccessor, IStringLocalizer<ImportExcelController> localizerimport, IImportDtlRepository repository)
        {
            _localizer = localizer;
            _service = service;
            _roleService = roleService;
            _httpContextAccessor = httpContextAccessor;
            _localizerimport = localizerimport;
            _repository = repository;
        }

        /// <summary>
        /// 获取用户列表首页
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Authorize("Permission")]
        public ApiBaseReturn<bool> Index()
        {
            ApiBaseReturn<bool> returnVM = new ApiBaseReturn<bool>();
            if (!ErrorInfo.Status)
            {
                try
                {
                    #region 检查参数

                    #endregion

                    #region 设置返回值

                    if (!ErrorInfo.Status)
                    {
                        returnVM.Result = true;
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
        /// 获取用户列表数据
        /// </summary>
        /// <param name="model">列表查询模型</param>
        /// <returns></returns>
        [HttpGet]
        [Authorize]
        public async Task<ApiBaseReturn<List<ManagerListModel>>> LoadData([FromQuery]ManagerRequestModel model)
        {
            ApiBaseReturn<List<ManagerListModel>> returnVM = new ApiBaseReturn<List<ManagerListModel>>();
            if (!ErrorInfo.Status)
            {
                try
                {
                    #region 检查参数

                    if (!ErrorInfo.Status && model != null && (model.Limit == 0 || model.Page == 0))
                    {
                        ErrorInfo.Set(_localizer["pageparam_error"], MethodBase.GetCurrentMethod(), EnumErrorType.Warning);
                    }

                    #endregion

                    #region 设置返回值

                    if (!ErrorInfo.Status)
                    {
                        var resultData = await _service.LoadDataAsync(model);
                        if (resultData != null)
                        {
                            foreach (ManagerListModel item in resultData.data)
                            {
                                item.IsEnabled = (item.ENABLED == "Y");
                            }
                            returnVM.Result = resultData.data;
                            returnVM.TotalCount = resultData.count;
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
        /// 获取用户新增或编辑数据
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Authorize("Permission")]
        public ApiBaseReturn<List<Sys_Manager_Role>> AddOrModify()
        {
            ApiBaseReturn<List<Sys_Manager_Role>> returnVM = new ApiBaseReturn<List<Sys_Manager_Role>>();
            if (!ErrorInfo.Status)
            {
                try
                {
                    #region 检查参数

                    #endregion

                    #region 设置返回值

                    if (!ErrorInfo.Status)
                    {
                        var roleList = _roleService.GetListByCondition(new ManagerRoleRequestModel
                        {
                            Key = null
                        });
                        if (roleList != null)
                        {
                            returnVM.Result = roleList;
                            returnVM.TotalCount = 1;
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
        /// 导出数据
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>	
        [HttpGet]
        [Authorize]
        public async Task<ApiBaseReturn<List<dynamic>>> ExportData([FromQuery] ManagerRequestModel model)
        {
            ApiBaseReturn<List<dynamic>> returnVM = new ApiBaseReturn<List<dynamic>>();
            if (!ErrorInfo.Status)
            {
                try
                {
                    #region 设置返回值

                    var res = await _service.GetExportData(model);
                    returnVM.Result = res.data;
                    returnVM.TotalCount = res.count;

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
        /// <param name="model">参数</param>
        /// <returns></returns>
        [HttpPost]
        [Authorize("Permission")]
        public async Task<ApiBaseReturn<bool>> AddOrModifySave([FromBody]ManagerAddOrModifyModel model)
        {
            ApiBaseReturn<bool> returnVM = new ApiBaseReturn<bool>();
            if (!ErrorInfo.Status)
            {
                try
                {
                    #region 检查参数

                    if (!ErrorInfo.Status && model != null && (string.IsNullOrEmpty(model.USER_NAME) || model.USER_NAME.Length > 32))
                    {
                        //用户账号不能为空并且长度不能超过32个字符。
                        ErrorInfo.Set(_localizer["user_name_error"], MethodBase.GetCurrentMethod(), EnumErrorType.Warning);
                    }

                    if (!ErrorInfo.Status && model != null && model.ROLE_ID == 0)
                    {
                        //用户所属角色不能为空。
                        ErrorInfo.Set(_localizer["role_id_error"], MethodBase.GetCurrentMethod(), EnumErrorType.Warning);
                    }

                    if (!ErrorInfo.Status && model != null && !string.IsNullOrEmpty(model.NICK_NAME) && model.NICK_NAME.Length > 32)
                    {
                        //用户昵称长度不能超过32个字符。
                        ErrorInfo.Set(_localizer["nick_name_error"], MethodBase.GetCurrentMethod(), EnumErrorType.Warning);
                    }

                    if (!ErrorInfo.Status && model != null && !string.IsNullOrEmpty(model.REMARK) && model.REMARK.Length > 128)
                    {
                        //备注信息的长度不能超过128个字符。
                        ErrorInfo.Set(_localizer["remark_error"], MethodBase.GetCurrentMethod(), EnumErrorType.Warning);
                    }

                    #endregion

                    #region 设置返回值

                    if (!ErrorInfo.Status)
                    {
                        var resultData = await _service.AddOrModifyAsync(model);
                        if (resultData != null && resultData.ResultCode == ResultCodeAddMsgKeys.CommonObjectSuccessCode)
                        {
                            returnVM.Result = true;
                        }
                        else if (resultData != null && resultData.ResultCode != ResultCodeAddMsgKeys.CommonObjectSuccessCode)
                        {
                            returnVM.Result = false;
                            //通用提示类的本地化问题处理
                            string resultMsg = GetLocalMessage(_httpContextAccessor, resultData.ResultCode, resultData.ResultMsg);
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

        /// <summary>
        /// 用户ID
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpPost]
        [Authorize("Permission")]
        public async Task<ApiBaseReturn<bool>> Delete(decimal id)
        {
            ApiBaseReturn<bool> returnVM = new ApiBaseReturn<bool>();
            if (!ErrorInfo.Status)
            {
                try
                {
                    #region 检查参数

                    //if (!ErrorInfo.Status && model != null && (string.IsNullOrEmpty(model.USER_NAME) || model.USER_NAME.Length > 32))
                    //{
                    //	//用户账号不能为空并且长度不能超过32个字符。
                    //	ErrorInfo.Set(_localizer["user_name_error"], MethodBase.GetCurrentMethod(), EnumErrorType.Warning);
                    //}

                    #endregion

                    #region 设置返回值

                    if (!ErrorInfo.Status)
                    {
                        var resultData = await _service.DeleteIdsAsync(id);
                        if (resultData != null && resultData.ResultCode == ResultCodeAddMsgKeys.CommonObjectSuccessCode)
                        {
                            returnVM.Result = true;
                        }
                        else if (resultData != null && resultData.ResultCode != ResultCodeAddMsgKeys.CommonObjectSuccessCode)
                        {
                            returnVM.Result = false;
                            //通用提示类的本地化问题处理
                            string resultMsg = GetLocalMessage(_httpContextAccessor, resultData.ResultCode, resultData.ResultMsg);
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
        /// 重置密码
        /// </summary>
        /// <param name="item"></param>
        /// <returns></returns>
        [HttpPost]
        [Authorize("Permission")]
        public async Task<ApiBaseReturn<bool>> ResetPassword([FromBody]ManagerAddOrModifyModel item){
            ApiBaseReturn<bool> returnVM = new ApiBaseReturn<bool>();
            if (!ErrorInfo.Status)
            {
                try
                {
                    #region 检查参数

                    #endregion

                    #region 设置返回值

                    if (!ErrorInfo.Status)
                    {
                        var resultData = await _service.IsExistsNameAsync(item);
                        if (resultData == null)
                        {
                            throw new Exception(_localizer["user_not_exist"]);
                        }
                        else
                        {
                            var restPassWordResult = await _service.RestPasswordAsync(item);
                           if( restPassWordResult.ResultCode == ResultCodeAddMsgKeys.CommonObjectSuccessCode)
                            {
                                returnVM.Result = true;
                            }
                            else
                            {
                                throw new Exception(_localizer["user_reset_error"]);
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
        /// 是否已存在此用户账号
        /// </summary>
        /// <param name="item"></param>
        /// <returns></returns>
        [HttpPost]
		[Authorize]
		public async Task<ApiBaseReturn<bool>> IsExistsName([FromBody]ManagerAddOrModifyModel item)
		{
			ApiBaseReturn<bool> returnVM = new ApiBaseReturn<bool>();
			if (!ErrorInfo.Status)
			{
				try
				{
					#region 检查参数

					//if (!ErrorInfo.Status && model != null && (string.IsNullOrEmpty(model.USER_NAME) || model.USER_NAME.Length > 32))
					//{
					//	//用户账号不能为空并且长度不能超过32个字符。
					//	ErrorInfo.Set(_localizer["user_name_error"], MethodBase.GetCurrentMethod(), EnumErrorType.Warning);
					//}

					#endregion

					#region 设置返回值

					if (!ErrorInfo.Status)
					{
						var resultData = await _service.IsExistsNameAsync(item);
						if (resultData != null)
						{
							returnVM.Result = resultData.Data;
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
		/// 改变用户状态(启用或停用用户)
		/// </summary>
		/// <param name="model">参数</param>
		/// <returns></returns>
		[HttpPost]
		[Authorize("Permission")]
		public async Task<ApiBaseReturn<bool>> ChangeLockStatus([FromBody]ChangeStatusModel model)
		{
			ApiBaseReturn<bool> returnVM = new ApiBaseReturn<bool>();
			if (!ErrorInfo.Status)
			{
				try
				{
					#region 检查参数

					if (!ErrorInfo.Status && model != null && model.Id <= 0)
					{
						//id不能为空。
						ErrorInfo.Set(_localizer["id_error"], MethodBase.GetCurrentMethod(), EnumErrorType.Warning);
					}

					#endregion

					#region 设置返回值

					if (!ErrorInfo.Status)
					{
						var resultData = await _service.ChangeLockStatusAsync(model);
						if (resultData != null && resultData.ResultCode == ResultCodeAddMsgKeys.CommonObjectSuccessCode)
						{
							returnVM.Result = true;
						}
						else if (resultData != null && resultData.ResultCode != ResultCodeAddMsgKeys.CommonObjectSuccessCode)
						{
							returnVM.Result = false;
							//通用提示类的本地化问题处理
							string resultMsg = GetLocalMessage(_httpContextAccessor, resultData.ResultCode, resultData.ResultMsg);
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
		/// 修改密码
		/// </summary>
		/// <param name="model">参数模型</param>
		/// <returns></returns>
		[HttpPost]
		[Authorize]
		public async Task<ApiBaseReturn<bool>> ChangePassword([FromBody]ChangePasswordModel model)
		{
			ApiBaseReturn<bool> returnVM = new ApiBaseReturn<bool>();
			if (!ErrorInfo.Status)
			{
				try
				{
					#region 检查参数

					if (!ErrorInfo.Status && model != null && (model.User_Name.IsNullOrWhiteSpace()))
					{
						//用户名称不能为空。
						ErrorInfo.Set(_localizer["user_name_error"], MethodBase.GetCurrentMethod(), EnumErrorType.Warning);
					}

					if (!ErrorInfo.Status && model != null
						&& (string.IsNullOrEmpty(model.OldPassword) || string.IsNullOrEmpty(model.NewPassword) || string.IsNullOrEmpty(model.NewPasswordRe)))
					{
						//用户密码不能为空。
						ErrorInfo.Set(_localizer["password_null_error"], MethodBase.GetCurrentMethod(), EnumErrorType.Warning);
					}

					if (!ErrorInfo.Status && model != null && (model.NewPassword != model.NewPasswordRe))
					{
						//用户新密码和确认密码不相同。
						ErrorInfo.Set(_localizer["password_check_error"], MethodBase.GetCurrentMethod(), EnumErrorType.Warning);
					}

					#endregion

					#region 设置返回值

					if (!ErrorInfo.Status)
					{
						var resultData = await _service.ChangePasswordAsync(model);
						if (resultData != null && resultData.ResultCode == ResultCodeAddMsgKeys.CommonObjectSuccessCode)
						{
							returnVM.Result = true;
						}
						else if (resultData != null && resultData.ResultCode != ResultCodeAddMsgKeys.CommonObjectSuccessCode)
						{
							returnVM.Result = false;
							//通用提示类的本地化问题处理
							string resultMsg = GetLocalMessage(_httpContextAccessor, resultData.ResultCode, resultData.ResultMsg);
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
		/// 获取用户个人信息
		/// </summary>
		/// <param name="id">用户id</param>
		/// <returns></returns>
		[HttpGet]
		[Authorize]
		public async Task<ApiBaseReturn<Sys_Manager>> ManagerInfo(decimal id)
		{
			ApiBaseReturn<Sys_Manager> returnVM = new ApiBaseReturn<Sys_Manager>();
			if (!ErrorInfo.Status)
			{
				try
				{
					#region 检查参数

					if (!ErrorInfo.Status && id <= 0)
					{
						//id不能为空。
						ErrorInfo.Set(_localizer["id_error"], MethodBase.GetCurrentMethod(), EnumErrorType.Warning);
					}

					#endregion

					#region 设置返回值

					if (!ErrorInfo.Status)
					{
						var model = await _service.GetManagerContainRoleNameByIdAsync(id);
						if (model != null)
						{
							model.AVATAR = model.AVATAR ?? string.Empty;
							returnVM.Result = model;
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
		/// 保存用户个人信息
		/// </summary>
		/// <param name="model"></param>
		/// <returns></returns>
		[HttpPost]
		[Authorize]
		public async Task<ApiBaseReturn<bool>> ManagerInfoSave([FromBody]ChangeInfoModel model)
		{
			ApiBaseReturn<bool> returnVM = new ApiBaseReturn<bool>();
			if (!ErrorInfo.Status)
			{
				try
				{
					#region 检查参数

					if (!ErrorInfo.Status && model != null && (string.IsNullOrEmpty(model.NICK_NAME)))
					{
						//请输入用户昵称。
						ErrorInfo.Set(_localizer["nick_name_error"], MethodBase.GetCurrentMethod(), EnumErrorType.Warning);
					}

					#endregion

					#region 设置返回值

					if (!ErrorInfo.Status && model != null)
					{
						model.MODIFY_TIME = DateTime.Now;
						var resultData = await _service.UpdateManagerInfoAsync(model);
						if (resultData != null && resultData.ResultCode == ResultCodeAddMsgKeys.CommonObjectSuccessCode)
						{
							returnVM.Result = true;
						}
						else if (resultData != null && resultData.ResultCode != ResultCodeAddMsgKeys.CommonObjectSuccessCode)
						{
							returnVM.Result = false;
							//通用提示类的本地化问题处理
							string resultMsg = GetLocalMessage(_httpContextAccessor, resultData.ResultCode, resultData.ResultMsg);
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
        /// 导入保存数据
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        [Authorize]
        public async Task<ApiBaseReturn<bool>> SaveExcelData([FromForm] string table_name)
        {
            ApiBaseReturn<bool> returnVM = new ApiBaseReturn<bool>();
            if (!ErrorInfo.Status)
            {
               // List<ImportExcelItem> excelItem = await GetExcelDataToList(table_name);
                try
                {
                    var excelFile = Request.Form.Files[0];
                    var filename = string.Empty;
                    var extname = string.Empty;
                    decimal filesize = 0;
                    var newFileName = string.Empty;
                    string errmsg = string.Empty;
                    //导入数据集合
                    List<ImportExcelItem> excelItem = null;
                    //导入模板数据列表
                    List<ImportDtl> importDtlList = null;
                    WebResponseContent content = null;
                    #region 检查参数

                    if (!ErrorInfo.Status && (excelFile == null || excelFile.FileName.IsNullOrEmpty()))
                    {
                        //上传失败
                        ErrorInfo.Set(_localizer["upload_error"], MethodBase.GetCurrentMethod(), EnumErrorType.Warning);
                    }

                    if (!ErrorInfo.Status)
                    {
                        filename = ContentDispositionHeaderValue
                                        .Parse(excelFile.ContentDisposition)
                                        .FileName
                                        .Trim('"');
                        extname = filename.Substring(filename.LastIndexOf("."), filename.Length - filename.LastIndexOf("."));

                        #region 判断后缀

                        if (!extname.ToLower().Contains("xlsx"))
                        {
                            //msg = "只允许上传xlsx格式的Excel文件."
                            ErrorInfo.Set(_localizer["file_suffix_error"], MethodBase.GetCurrentMethod(), EnumErrorType.Warning);
                        }

                        #endregion

                        #region 判断大小

                        filesize = Convert.ToDecimal(Math.Round(excelFile.Length / 1024.00, 3));
                        long mb = excelFile.Length / 1024 / 1024; // MB
                        if (mb > 10)
                        {
                            //"只允许上传小于 10MB 的文件."
                            ErrorInfo.Set(_localizer["size_10m_error"], MethodBase.GetCurrentMethod(), EnumErrorType.Warning);
                        }

                        #endregion
                    }

                    if (!ErrorInfo.Status && table_name.IsNullOrEmpty())
                    {
                        //throw new Exception("请传入表名称。");
                        ErrorInfo.Set(_localizer["table_name_error"], MethodBase.GetCurrentMethod(), EnumErrorType.Warning);
                    }

                    if (!ErrorInfo.Status)
                    {
                        var resdata = await _repository.GetImportMst(table_name);
                        if (resdata == null)
                        {
                            //string.Format("传入模板未定义, 请先定入需导入的模板主信息.")
                            ErrorInfo.Set(_localizer["import_mst_error"], MethodBase.GetCurrentMethod(), EnumErrorType.Warning);
                        }
                        else
                        {
                            importDtlList = (await _repository.GetListAsync("Where MST_ID =:MST_ID order by length(EXCEL_ITEM)，EXCEL_ITEM ", new { MST_ID = resdata.ID }))?.ToList();
                        }
                    }

                    #endregion

                    #region 解释Excel数据

                    if (!ErrorInfo.Status)
                    {
                        newFileName = DateTime.Now.ToString("yyyyMMddHHmmssfff") + new Random().Next(1000, 9999) + extname;
                        var pathRoot = AppContext.BaseDirectory + @"upload\exceldata\";
                        if (Directory.Exists(pathRoot) == false)
                        {
                            Directory.CreateDirectory(pathRoot);
                        }
                        filename = pathRoot + $"{newFileName}";
                        using (FileStream fs = System.IO.File.Create(filename))
                        {
                            excelFile.CopyTo(fs);
                            fs.Flush();
                        }

                        content = EPPlusHelper.WriteToDataList(filename, importDtlList, _localizerimport);
                        if (!content.Status)
                        {
                            ErrorInfo.Set(content.Message, MethodBase.GetCurrentMethod(), EnumErrorType.Warning);
                        }
                    }

                    #endregion

                    #region 校验Excel数据(唯一列,不为空列)

                    if (!ErrorInfo.Status && content.Data != null)
                    {
                        excelItem = content.Data;
                        //PropertyInfo[] propertyInfos = typeof(ImportExcelItem).GetProperties().ToArray();
                        //唯一列
                        var uniqueItems = importDtlList.Where(t => t.IS_UNIQUE == 1).ToList();
                        int index = -1; bool isExist = false;
                        foreach (var item in uniqueItems)
                        {
                            index = importDtlList.IndexOf(item);
                            string columnName = $"Column{index + 1}";

                            List<string> tmpList = new List<string>();
                            string val = string.Empty;
                            foreach (var exItem in excelItem)
                            {
                                val = exItem.GetType().GetProperty(columnName).GetValue(exItem)?.ToString() ?? string.Empty;
                                if (!string.IsNullOrWhiteSpace(val))
                                {
                                    tmpList.Add(val);
                                }
                                else
                                {
                                    //"列{0}的值不能为空."
                                    errmsg = string.Format(_localizer["col_not_nullable"], columnName);
                                    ErrorInfo.Set(errmsg, MethodBase.GetCurrentMethod(), EnumErrorType.Warning);
                                    break;
                                }
                            }
                            var groups = tmpList.GroupBy(t => t).Where(t => t.Count() > 1).FirstOrDefault();
                            if (groups != null && !string.IsNullOrWhiteSpace(groups.ElementAt(0)))
                            {
                                //$"列{0}的值[{1}]不唯一."
                                errmsg = string.Format(_localizer["col_not_unique"], item.COLUMN_CAPTION, groups.ElementAt(0));
                                ErrorInfo.Set(errmsg, MethodBase.GetCurrentMethod(), EnumErrorType.Warning);
                                break;
                            }
                        }

                        if (!ErrorInfo.Status)
                        {
                            foreach (var item in uniqueItems)
                            {
                                index = importDtlList.IndexOf(item);
                                string columnName = $"Column{index + 1}";

                                List<string> tmpList = new List<string>();
                                string val = string.Empty;
                                foreach (var exItem in excelItem)
                                {
                                    val = exItem.GetType().GetProperty(columnName).GetValue(exItem)?.ToString() ?? string.Empty;
                                    isExist = await _repository.ItemIsExist(table_name, item.COLUMN_NAME, val);
                                    if (isExist)
                                    {
                                        //$"列{0}的值[{1}]已在数据库存在, 不唯一."
                                        errmsg = string.Format(_localizer["data_col_not_unique"], item.COLUMN_CAPTION, val);
                                        ErrorInfo.Set(errmsg, MethodBase.GetCurrentMethod(), EnumErrorType.Warning);
                                        break;
                                    }
                                }
                            }
                        }

                        //不为空项目校验
                        if (!ErrorInfo.Status)
                        {
                            var noNullItems = importDtlList.Where(t => t.ISNULL_ABLE == 0).ToList();
                            index = -1;
                            foreach (var item in noNullItems)
                            {
                                index = importDtlList.IndexOf(item);
                                string columnName = $"Column{index + 1}";

                                List<string> tmpList = new List<string>();
                                string val = string.Empty;
                                foreach (var exItem in excelItem)
                                {
                                    val = exItem.GetType().GetProperty(columnName).GetValue(exItem)?.ToString() ?? string.Empty;
                                    if (string.IsNullOrWhiteSpace(val))
                                    {
                                        //"列{0}的值[{1)}]不能为空."
                                        errmsg = string.Format(_localizer["col_not_nullable"], item.COLUMN_CAPTION, val);
                                        ErrorInfo.Set(errmsg, MethodBase.GetCurrentMethod(), EnumErrorType.Warning);
                                        break;
                                    }
                                }
                            }
                        }
                    }

                    #endregion

                    #region 保存并返回

                    if (!ErrorInfo.Status)
                    {
                        ImportResult resdata = await _service.SaveDataByTrans(excelItem,importDtlList);
                        if (resdata.Result > 0)
                        {
                            returnVM.Result = resdata.Result > 0;
                        }
                        else if (resdata.Result <= 0 && resdata.ErrInfo != null)
                        {
                            //第{0}行，{1}没有找到相关信息。
                            errmsg = string.Format(_localizer["notfind_getval"], resdata.ErrInfo.Index, resdata.ErrInfo.COLUMN_CAPTION);
                            ErrorInfo.Set(errmsg, MethodBase.GetCurrentMethod(), EnumErrorType.Warning);
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
        /// 将导入的Excel解析成数据
        /// </summary>
        /// <param name="table_name"></param>
        /// <returns></returns>
        private async Task<List<ImportExcelItem>> GetExcelDataToList(string table_name)
        {
            List < ImportExcelItem > excelItem = null;
            try
            {

                var excelFile = Request.Form.Files[0];
                var filename = string.Empty;
                var extname = string.Empty;
                decimal filesize = 0;
                var newFileName = string.Empty;
                string errmsg = string.Empty;
                List<ImportDtl> importDtlList = null;
                WebResponseContent content = null;
                #region 检查参数

                if (!ErrorInfo.Status && (excelFile == null || excelFile.FileName.IsNullOrEmpty()))
                {
                    //上传失败
                    ErrorInfo.Set(_localizer["upload_error"], MethodBase.GetCurrentMethod(), EnumErrorType.Warning);
                }

                if (!ErrorInfo.Status)
                {
                    filename = ContentDispositionHeaderValue
                                    .Parse(excelFile.ContentDisposition)
                                    .FileName
                                    .Trim('"');
                    extname = filename.Substring(filename.LastIndexOf("."), filename.Length - filename.LastIndexOf("."));

                    #region 判断后缀

                    if (!extname.ToLower().Contains("xlsx"))
                    {
                        //msg = "只允许上传xlsx格式的Excel文件."
                        ErrorInfo.Set(_localizer["file_suffix_error"], MethodBase.GetCurrentMethod(), EnumErrorType.Warning);
                    }

                    #endregion

                    #region 判断大小

                    filesize = Convert.ToDecimal(Math.Round(excelFile.Length / 1024.00, 3));
                    long mb = excelFile.Length / 1024 / 1024; // MB
                    if (mb > 10)
                    {
                        //"只允许上传小于 10MB 的文件."
                        ErrorInfo.Set(_localizer["size_10m_error"], MethodBase.GetCurrentMethod(), EnumErrorType.Warning);
                    }

                    #endregion
                }

                if (!ErrorInfo.Status && table_name.IsNullOrEmpty())
                {
                    //throw new Exception("请传入表名称。");
                    ErrorInfo.Set(_localizer["table_name_error"], MethodBase.GetCurrentMethod(), EnumErrorType.Warning);
                }

                if (!ErrorInfo.Status)
                {
                    var resdata = await _repository.GetImportMst(table_name);
                    if (resdata == null)
                    {
                        //string.Format("传入模板未定义, 请先定入需导入的模板主信息.")
                        ErrorInfo.Set(_localizer["import_mst_error"], MethodBase.GetCurrentMethod(), EnumErrorType.Warning);
                    }
                    else
                    {
                        importDtlList = (await _repository.GetListAsync("Where MST_ID =:MST_ID order by length(EXCEL_ITEM)，EXCEL_ITEM ", new { MST_ID = resdata.ID }))?.ToList();
                    }
                }

                #endregion

                #region 解释Excel数据

                if (!ErrorInfo.Status)
                {
                    newFileName = DateTime.Now.ToString("yyyyMMddHHmmssfff") + new Random().Next(1000, 9999) + extname;
                    var pathRoot = AppContext.BaseDirectory + @"upload\exceldata\";
                    if (Directory.Exists(pathRoot) == false)
                    {
                        Directory.CreateDirectory(pathRoot);
                    }
                    filename = pathRoot + $"{newFileName}";
                    using (FileStream fs = System.IO.File.Create(filename))
                    {
                        excelFile.CopyTo(fs);
                        fs.Flush();
                    }

                    content = EPPlusHelper.WriteToDataList(filename, importDtlList, _localizerimport);
                    if (!content.Status)
                    {
                        ErrorInfo.Set(content.Message, MethodBase.GetCurrentMethod(), EnumErrorType.Warning);
                    }
                }

                #endregion

                #region 校验Excel数据(唯一列,不为空列)

                if (!ErrorInfo.Status && content.Data != null)
                {
                    excelItem = content.Data;
                    //PropertyInfo[] propertyInfos = typeof(ImportExcelItem).GetProperties().ToArray();
                    //唯一列
                    var uniqueItems = importDtlList.Where(t => t.IS_UNIQUE == 1).ToList();
                    int index = -1; bool isExist = false;
                    foreach (var item in uniqueItems)
                    {
                        index = importDtlList.IndexOf(item);
                        string columnName = $"Column{index + 1}";

                        List<string> tmpList = new List<string>();
                        string val = string.Empty;
                        foreach (var exItem in excelItem)
                        {
                            val = exItem.GetType().GetProperty(columnName).GetValue(exItem)?.ToString() ?? string.Empty;
                            if (!string.IsNullOrWhiteSpace(val))
                            {
                                tmpList.Add(val);
                            }
                            else
                            {
                                //"列{0}的值不能为空."
                                errmsg = string.Format(_localizer["col_not_nullable"], columnName);
                                ErrorInfo.Set(errmsg, MethodBase.GetCurrentMethod(), EnumErrorType.Warning);
                                break;
                            }
                        }
                        var groups = tmpList.GroupBy(t => t).Where(t => t.Count() > 1).FirstOrDefault();
                        if (groups != null && !string.IsNullOrWhiteSpace(groups.ElementAt(0)))
                        {
                            //$"列{0}的值[{1}]不唯一."
                            errmsg = string.Format(_localizer["col_not_unique"], item.COLUMN_CAPTION, groups.ElementAt(0));
                            ErrorInfo.Set(errmsg, MethodBase.GetCurrentMethod(), EnumErrorType.Warning);
                            break;
                        }
                    }

                    if (!ErrorInfo.Status)
                    {
                        foreach (var item in uniqueItems)
                        {
                            index = importDtlList.IndexOf(item);
                            string columnName = $"Column{index + 1}";

                            List<string> tmpList = new List<string>();
                            string val = string.Empty;
                            foreach (var exItem in excelItem)
                            {
                                val = exItem.GetType().GetProperty(columnName).GetValue(exItem)?.ToString() ?? string.Empty;
                                isExist = await _repository.ItemIsExist(table_name, item.COLUMN_NAME, val);
                                if (isExist)
                                {
                                    //$"列{0}的值[{1}]已在数据库存在, 不唯一."
                                    errmsg = string.Format(_localizer["data_col_not_unique"], item.COLUMN_CAPTION, val);
                                    ErrorInfo.Set(errmsg, MethodBase.GetCurrentMethod(), EnumErrorType.Warning);
                                    break;
                                }
                            }
                        }
                    }

                    //不为空项目校验
                    if (!ErrorInfo.Status)
                    {
                        var noNullItems = importDtlList.Where(t => t.ISNULL_ABLE == 0).ToList();
                        index = -1;
                        foreach (var item in noNullItems)
                        {
                            index = importDtlList.IndexOf(item);
                            string columnName = $"Column{index + 1}";

                            List<string> tmpList = new List<string>();
                            string val = string.Empty;
                            foreach (var exItem in excelItem)
                            {
                                val = exItem.GetType().GetProperty(columnName).GetValue(exItem)?.ToString() ?? string.Empty;
                                if (string.IsNullOrWhiteSpace(val))
                                {
                                    //"列{0}的值[{1)}]不能为空."
                                    errmsg = string.Format(_localizer["col_not_nullable"], item.COLUMN_CAPTION, val);
                                    ErrorInfo.Set(errmsg, MethodBase.GetCurrentMethod(), EnumErrorType.Warning);
                                    break;
                                }
                            }
                        }
                    }
                }

                #endregion




                return excelItem;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

    }
}