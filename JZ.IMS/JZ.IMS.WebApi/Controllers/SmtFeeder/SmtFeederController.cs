/*
*┌──────────────────────────────────────────────────────────────┐
*│　描    述： 控制器                                                    
*│　作    者：嘉志科技                                              
*│　版    本：2.0   模板代码自动生成                                              
*│　创建时间：2020-03-11 10:19:01                           
*└──────────────────────────────────────────────────────────────┘
*┌──────────────────────────────────────────────────────────────┐
*│　命名空间： JZ.IMS.Admin.Controllers                                   
*│　接口名称： ISmtFeederController                                      
*└──────────────────────────────────────────────────────────────┘
*/

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using JZ.IMS.Core.Helper;
using JZ.IMS.IServices;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using JZ.IMS.ViewModels;
using FluentValidation.Results;
using JZ.IMS.WebApi.Validation;
using JZ.IMS.WebApi.Controllers;
using JZ.IMS.WebApi.Public;
using JZ.IMS.IRepository;
using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Localization;
using System.Reflection;
using JZ.IMS.Core.Extensions;
using JZ.IMS.Models;

namespace JZ.IMS.WebApi.Controllers
{
	/// <summary>
	/// 飞达注册控制器 
	/// </summary>
	[Route("api/[controller]/[action]")]
	[ApiController]
	public class SmtFeederController : BaseController
	{
		private readonly ISmtFeederRepository _repository;
		private readonly IMapper _mapper;
		private readonly IHttpContextAccessor _httpContextAccessor;
		private readonly IStringLocalizer<SmtFeederController> _localizer;
		
		public SmtFeederController(ISmtFeederRepository repository, IMapper mapper, IHttpContextAccessor httpContextAccessor,
			IStringLocalizer<SmtFeederController> localizer)
		{
			_repository = repository;
			_mapper = mapper;
			_httpContextAccessor = httpContextAccessor;
			_localizer = localizer;
		}

		public class IndexVM
		{
			/// <summary>
			/// 获取状态类型列表
			/// </summary>
			/// <returns></returns>
			public List<IDNAME> StatusList { get; set; }

			/// <summary>
			/// 飞达类型
			/// </summary>
			/// <returns></returns>
			public List<CodeName> FeederTypeList { get; set; }
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
			if (!ErrorInfo.Status) {
				try
				{
					#region 设置返回值
					if (!ErrorInfo.Status)
					{
						returnVM.Result = new IndexVM
						{
							StatusList = await _repository.GetStatus(),
							FeederTypeList = await _repository.GetFeederTypeList(),
						};
					}

					#endregion
				}
				catch (Exception ex)
				{

					ErrorInfo.Set(ex.Message,MethodBase.GetCurrentMethod(),EnumErrorType.Error);
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
		/// 字段说明:
		/// 料架编号:FEEDER 
		/// 供应商:SUPPLIER	    类型:FTYPE    尺寸:FSIZE    本体编码:FBODYMARK
		/// 状态:STATUS         描述:DESCRIPTION 
		/// </summary>
		/// <param name="model"></param>
		/// <returns></returns>
		[HttpGet]
		[Authorize]
		public async Task<ApiBaseReturn<string>> LoadData([FromQuery]SmtFeederRequestModel model)
		{

			ApiBaseReturn<string> returnVM = new ApiBaseReturn<string>();
			if (!ErrorInfo.Status)
			{
				try
				{
					#region 检查参数

					if (!ErrorInfo.Status && (model.USER_ID ?? 0) <= 0)
					{
						ErrorInfo.Set(_localizer["USER_ID_NOT_NULL"], MethodBase.GetCurrentMethod(), EnumErrorType.Warning);
					}

					#endregion

					#region 设置返回值

					if (!ErrorInfo.Status)
					{
						var res = await _repository.LoadData(model);

						returnVM.Result = JsonHelper.ObjectToJSONOfDate(res?.data);
						returnVM.TotalCount = res?.count ?? 0;
					}

					#endregion
				}
				catch (Exception ex )
				{

					ErrorInfo.Set(ex.Message,MethodBase.GetCurrentMethod(),EnumErrorType.Error);
				}
			}

			#region 如果出现错误，则写错误日志并返回错误内容

			WriteLog(ref returnVM);

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
		public async Task<ApiBaseReturn<List<dynamic>>> ExportData([FromQuery]SmtFeederRequestModel model)
		{

			ApiBaseReturn<List<dynamic>> returnVM = new ApiBaseReturn<List<dynamic>>();

			if (!ErrorInfo.Status)
			{
				try
				{
					#region 检查参数

					if (!ErrorInfo.Status && (model.USER_ID ?? 0) <= 0)
					{
						ErrorInfo.Set(_localizer["USER_ID_NOT_NULL"], MethodBase.GetCurrentMethod(), EnumErrorType.Warning);
					}

					#endregion

					#region 设置返回值
					var result = await _repository.GetExportData(model);
					returnVM.Result = result.data;
					returnVM.TotalCount = result.count;
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
		/// 通过ID删除记录
		/// </summary>
		/// <param name="Id">要删除的记录的ID</param>
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

					if (!ErrorInfo.Status)
					{
						if (id <= 0)
						{
							returnVM.Result = false;
							//通用提示类的本地化问题处理
							string resultMsg = GetLocalMessage(_httpContextAccessor, ResultCodeAddMsgKeys.CommonModelStateInvalidCode,
								ResultCodeAddMsgKeys.CommonModelStateInvalidMsg);
							ErrorInfo.Set(resultMsg, MethodBase.GetCurrentMethod(), EnumErrorType.Warning);
						}
						else
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
		/// 保存数据
		/// </summary>
		/// <param name="model"></param>
		/// <returns></returns>
		[HttpPost]
		[Authorize("Permission")]
		public async Task<ApiBaseReturn<bool>> SaveData([FromBody] SmtFeederModel model)
		{
			ApiBaseReturn<bool> returnVM = new ApiBaseReturn<bool>();
			if (!ErrorInfo.Status)
			{
				try
				{
					#region 检查参数

					if (!ErrorInfo.Status)
					{
						//检验FEEDER是否存在
						if (model.insertRecords != null||model.updateRecords !=null)
						{
							List<SmtFeederAddOrModifyModel> list = model.insertRecords !=null? model.insertRecords : model.updateRecords;
							  var isRepeat = list.GroupBy(x => x.FEEDER).Where(x => x.Count()>1).FirstOrDefault();
							if (isRepeat!=null)
							{
								SmtFeederAddOrModifyModel feeder = isRepeat.ElementAt(0);
								if (feeder != null)
								{
									string msg = string.Format(_localizer["repeatfeeder_no_error"], feeder.FEEDER);
									ErrorInfo.Set(msg, MethodBase.GetCurrentMethod(), EnumErrorType.Warning);
									returnVM.Result = false;
								}
							}
							if (!ErrorInfo.Status)
							{
								foreach (var item in list)
								{
									var tmpdata = await _repository.ItemIsByFeeder(item.FEEDER,item.ID);
									if (tmpdata!=null)
									{
										ErrorInfo.Set(_localizer["existfeeder_no_error"], MethodBase.GetCurrentMethod(), EnumErrorType.Warning);
										returnVM.Result = false;
									}
								}
							}
						}
					}
				
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
        /// 保存PDA飞达盘点数据
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        [Authorize]
        public async Task<ApiBaseReturn<List<SfcsFeederKeepDetail>>> SavePDAFeederCheckData([FromBody] SaveFeederCheckDataRequestModel model)
        {
            SmtFeeder feeder = null;
			SfcsFeederKeepHead head = null;
            SfcsFeederKeepDetail detail = null;
			List<GetFeederInfoListModel> fList = null;
            ApiBaseReturn<List<SfcsFeederKeepDetail>> returnVM = new ApiBaseReturn<List<SfcsFeederKeepDetail>>();

            if (!ErrorInfo.Status)
            {
                try
                {
                    if (!model.CHECK_CODE.IsNullOrEmpty() && model.FEEDER_BODYMARK.IsNullOrEmpty())
                    {
                        head = (await _repository.GetListByTableEX<SfcsFeederKeepHead>("*", "SFCS_FEEDER_KEEP_HEAD", " AND CHECK_CODE=:CHECK_CODE", new { CHECK_CODE = model.CHECK_CODE })).FirstOrDefault();
                        if (!head.IsNullOrWhiteSpace())
                        {
							returnVM.Result = await _repository.GetListByTableEX<SfcsFeederKeepDetail>("*", "SFCS_FEEDER_KEEP_DETAIL", " AND KEEP_HEAD_ID=:KEEP_HEAD_ID ", new { KEEP_HEAD_ID = head.ID });
						}
                        else
                        {
							returnVM.Result = null;
                        }
                    }
                    else
                    {
                        #region 获取所在组织架构
                        Sys_Manager sys_Manager = (await _repository.GetListByTableEX<Sys_Manager>("*", "SYS_MANAGER", " AND USER_NAME = :USER_NAME", new { USER_NAME = model.CHECK_USER })).FirstOrDefault();
                        if (sys_Manager == null) { throw new Exception("USER_INFO_EMPTY"); }
                        String organize_id = "";
                        List<String> idList = _repository.QueryEx<String>("SELECT ID FROM SYS_ORGANIZE T START WITH T.ID IN (SELECT ORGANIZE_ID FROM SYS_USER_ORGANIZE WHERE MANAGER_ID=:USER_ID) CONNECT BY PRIOR T.ID=T.PARENT_ORGANIZE_ID", new { USER_ID = sys_Manager.ID });
                        if (idList != null && idList.Count() > 0)
                        {
                            organize_id = String.Join(",", idList);
                        }
                        else
                        {
                            throw new Exception("ORGANIZE_INFO_EMPTY");
                        } 
                        #endregion

                        if (model.FEEDER_BODYMARK.IsNullOrEmpty() && !ErrorInfo.Status)
                        {
                            ErrorInfo.Set(_localizer["FEEDER_BODYMARK_NOT_NULL"], MethodBase.GetCurrentMethod(), EnumErrorType.Warning);
                        }
                        else
                        {
                            feeder = (await _repository.GetListByTableEX<SmtFeeder>("*", "SMT_FEEDER", " AND FEEDER=:FEEDER AND ORGANIZE_ID in ("+ organize_id + ")", new { FEEDER = model.FEEDER_BODYMARK })).FirstOrDefault();
                            if (feeder.IsNullOrWhiteSpace())
                            {
                                ErrorInfo.Set(_localizer["FEEDER_CODE_INFO_NULL"], MethodBase.GetCurrentMethod(), EnumErrorType.Warning);
                            }
                            else if (feeder.STATUS == 6)
                            {
                                ErrorInfo.Set(_localizer["FEEDER_STATUS_6_NOT_CHECK"], MethodBase.GetCurrentMethod(), EnumErrorType.Warning);
                            }
                            else if (feeder.STATUS == 7)
							{
								ErrorInfo.Set(_localizer["FEEDER_STATUS_7_NOT_CHECK"], MethodBase.GetCurrentMethod(), EnumErrorType.Warning);
							}
							else
							{
                                if (model.CHECK_CODE.IsNullOrEmpty())
                                {
                                    String check_code = _repository.QueryEx<String>("SELECT CHECK_CODE FROM SFCS_FEEDER_KEEP_HEAD WHERE CHECK_STATUS != 2 AND ORGANIZE_ID in (" + organize_id + ") ORDER BY ID ASC").FirstOrDefault();
                                    if (!String.IsNullOrEmpty(check_code)) { model.CHECK_CODE = check_code; }
                                    fList = await _repository.GetListByTableEX<GetFeederInfoListModel>("FTYPE FEEDER_TYPE,FSIZE FEEDER_SIZE,COUNT(0) FEEDER_QTY", "SMT_FEEDER", " AND STATUS NOT IN (6,7) AND ORGANIZE_ID in (" + organize_id + ") GROUP BY FTYPE,FSIZE");
                                }
                            }
						}
                        if (!model.CHECK_CODE.IsNullOrEmpty() && !ErrorInfo.Status)
                        {
                            //检查编号是否存在
                            head = (await _repository.GetListByTableEX<SfcsFeederKeepHead>("*", "SFCS_FEEDER_KEEP_HEAD", " AND CHECK_CODE=:CHECK_CODE", new { CHECK_CODE = model.CHECK_CODE })).FirstOrDefault();
                            if (head.IsNullOrWhiteSpace())
                            {
                                ErrorInfo.Set(_localizer["FEEDER_CODE_INFO_NULL"], MethodBase.GetCurrentMethod(), EnumErrorType.Warning);
                            }
                            else if (head.CHECK_STATUS != 0)
                            {
                                ErrorInfo.Set(_localizer["CHECK_STATUS_NOT_0"], MethodBase.GetCurrentMethod(), EnumErrorType.Warning);
                            }
                            else
                            {
                                detail = (await _repository.GetListByTableEX<SfcsFeederKeepDetail>("*", "SFCS_FEEDER_KEEP_DETAIL", " AND KEEP_HEAD_ID=:KEEP_HEAD_ID AND FEEDER_TYPE=:FEEDER_TYPE AND FEEDER_SIZE=:FEEDER_SIZE  ", new { KEEP_HEAD_ID = head.ID, FEEDER_TYPE = feeder.FTYPE, FEEDER_SIZE = feeder.FSIZE })).FirstOrDefault();
                                if (!detail.IsNullOrWhiteSpace())
                                {
                                    SfcsFeederKeepContent content = (await _repository.GetListByTableEX<SfcsFeederKeepContent>("*", "SFCS_FEEDER_KEEP_CONTENT", " AND KEEP_DETAIL_ID=:KEEP_DETAIL_ID AND FEEDER_ID=:FEEDER_ID ", new { KEEP_DETAIL_ID = detail.ID, FEEDER_ID = feeder.ID })).FirstOrDefault();
                                    if (!content.IsNullOrWhiteSpace())
                                    {
                                        ErrorInfo.Set(_localizer["FEEDER_CODE_INFO_REPEAT"], MethodBase.GetCurrentMethod(), EnumErrorType.Warning);
									}
                                    else if (detail.FEEDER_TYPE_TOTAL < (detail.CHECK_QTY + 1))
                                    {
                                        ErrorInfo.Set(_localizer["FEEDER_CHECK_QTY_ERROR"], MethodBase.GetCurrentMethod(), EnumErrorType.Warning);
                                    }
                                }
							}
						}

						int headId = 0;
						if (!ErrorInfo.Status)
						{
							headId = await _repository.SavePDAFeederCheckData(model, feeder, head, detail, fList);
						}
						if (head == null && !model.CHECK_CODE.IsNullOrEmpty())
						{
							head = (await _repository.GetListByTableEX<SfcsFeederKeepHead>("*", "SFCS_FEEDER_KEEP_HEAD", " AND CHECK_CODE=:CHECK_CODE", new { CHECK_CODE = model.CHECK_CODE })).FirstOrDefault();
						}
						if (head != null || headId > 1)
						{
							headId = headId > 1 ? headId : Convert.ToInt32(head.ID);
							returnVM.Result = await _repository.GetListByTableEX<SfcsFeederKeepDetail>("*", "SFCS_FEEDER_KEEP_DETAIL", " AND KEEP_HEAD_ID=:KEEP_HEAD_ID ", new { KEEP_HEAD_ID = headId });
						}
					}
                }
                catch (Exception ex)
                {
                    ErrorInfo.Set(_localizer[ex.Message], MethodBase.GetCurrentMethod(), EnumErrorType.Error);
                }
            }

            #region 如果出现错误，则写错误日志并返回错误内容

            WriteLog(ref returnVM);

            #endregion

            return returnVM;
        }

        /// <summary>
        /// 删除PDA飞达盘点数据记录
        /// </summary>
        /// <param name="check_code">飞达点检编号</param>
        /// <returns></returns>
        [HttpPost]
        [Authorize]
        public async Task<ApiBaseReturn<bool>> DeletePDAFeederCheckData(String check_code)
        {
            ApiBaseReturn<bool> returnVM = new ApiBaseReturn<bool>();

            if (!ErrorInfo.Status)
            {
                try
                {
                    if (String.IsNullOrEmpty(check_code))
                    {
                        throw new Exception("FEEDER_CODE_INFO_NULL");
                    }
                    else
                    {
                        //只能能删除新增和未审核状态下的盘点单
                        SfcsFeederKeepHead head = (await _repository.GetListByTableEX<SfcsFeederKeepHead>("*", "SFCS_FEEDER_KEEP_HEAD", " AND CHECK_CODE=:CHECK_CODE", new { CHECK_CODE = check_code })).FirstOrDefault();
                        if (head == null)
                        {
                            throw new Exception("FEEDER_CODE_INFO_NULL");
                        }
                        else if (head.CHECK_STATUS == 0 || head.CHECK_STATUS == 1)
                        {
                            returnVM.Result = await _repository.DeletePDAFeederCheckData(head.ID);
                        }
                        else
                        {
                            throw new Exception("CHECK_STATUS_NOT_INSERT");
                        }

                    }
                }
                catch (Exception ex)
                {
                    returnVM.Result = false;
                    ErrorInfo.Set(_localizer[ex.Message], MethodBase.GetCurrentMethod(), EnumErrorType.Error);
                }
            }

            #region 如果出现错误，则写错误日志并返回错误内容

            WriteLog(ref returnVM);

            #endregion

            return returnVM;
        }

        /// <summary>
        /// 确认PDA飞达盘点数据
        /// </summary>
        /// <param name="check_code">飞达点检编号</param>
        /// <returns></returns>
        [HttpPost]
		[Authorize]
		public async Task<ApiBaseReturn<bool>> AuditFeederCheckData([FromBody] AuditFeederCheckDataRequestModel model)
		{
			SfcsFeederKeepHead head = null;
			ApiBaseReturn<bool> returnVM = new ApiBaseReturn<bool>();
			if (!ErrorInfo.Status)
			{
				try
				{
					#region 检查参数
					if (model.ID > 0 && !ErrorInfo.Status)
					{
                        //检查编号是否存在
                        head = (await _repository.GetListByTableEX<SfcsFeederKeepHead>("*", "SFCS_FEEDER_KEEP_HEAD", " AND ID=:ID", new { ID = model.ID })).FirstOrDefault();
                        if (head.IsNullOrWhiteSpace())
                        {
                            ErrorInfo.Set(_localizer["FEEDER_CODE_INFO_NULL"], MethodBase.GetCurrentMethod(), EnumErrorType.Warning);
                        }
                    }
                    else
                    {
						ErrorInfo.Set(_localizer["FEEDER_CODE_INFO_NULL"], MethodBase.GetCurrentMethod(), EnumErrorType.Warning);
					}
                    if (model.STATUS == 1 && !ErrorInfo.Status)
                    {
                        if (head.CHECK_STATUS != 0)
                        {
                            ErrorInfo.Set(_localizer["CHECK_STATUS_NOT_0"], MethodBase.GetCurrentMethod(), EnumErrorType.Warning);
                        }
                    }
                    else if (model.STATUS == 2 && !ErrorInfo.Status)
                    {
                        if (head.CHECK_STATUS != 1)
                        {
                            ErrorInfo.Set(_localizer["CHECK_STATUS_NOT_1"], MethodBase.GetCurrentMethod(), EnumErrorType.Warning);
                        }
                    }
                    else
                    {
						ErrorInfo.Set(_localizer["CHECK_STATUS_NOT_INSERT"], MethodBase.GetCurrentMethod(), EnumErrorType.Warning);
					}
                    #endregion

                    #region 保存并返回

                    if (!ErrorInfo.Status)
					{
						returnVM.Result = await _repository.ConfirmPDAFeederCheckData(model);
					}

					#endregion
				}
				catch (Exception ex)
				{
					ErrorInfo.Set(_localizer[ex.Message], MethodBase.GetCurrentMethod(), EnumErrorType.Error);
				}
			}

			#region 如果出现错误，则写错误日志并返回错误内容

			WriteLog(ref returnVM);

			#endregion

			return returnVM;
		}

		/// <summary>
		/// PDA飞达盘点列表
		/// </summary>
		/// <param name="model"></param>
		/// <returns></returns>
		[HttpGet]
        [Authorize]
        public async Task<ApiBaseReturn<List<FeederCheckListModel>>> LoadPDAFeederCheckList([FromQuery] FeederCheckRequestModel model)
        {

            ApiBaseReturn<List<FeederCheckListModel>> returnVM = new ApiBaseReturn<List<FeederCheckListModel>>();
            if (!ErrorInfo.Status)
            {
                try
                {
                    #region 检查参数


                    #endregion

                    #region 设置返回值

                    if (!ErrorInfo.Status)
                    {
                        returnVM.Result = await _repository.LoadPDAFeederCheckList(model);
                        returnVM.TotalCount = await _repository.LoadPDAFeederCheckListCount(model);
                    }

                    #endregion
                }
                catch (Exception ex)
                {
					ErrorInfo.Set(_localizer[ex.Message], MethodBase.GetCurrentMethod(), EnumErrorType.Warning);
                }
            }

            #region 如果出现错误，则写错误日志并返回错误内容

            WriteLog(ref returnVM);

            #endregion

            return returnVM;
        }

		/// <summary>
		/// 获取飞达盘点明细数据
		/// </summary>
		/// <param name="model"></param>
		/// <returns></returns>
		[HttpGet]
        [Authorize]
        public async Task<ApiBaseReturn<List<LoadPDAFeederCheckDetailListModel>>> LoadPDAFeederCheckDetailList([FromQuery] LoadPDAFeederCheckDetailRequestModel model)
        {
            ApiBaseReturn<List<LoadPDAFeederCheckDetailListModel>> returnVM = new ApiBaseReturn<List<LoadPDAFeederCheckDetailListModel>>();
            if (!ErrorInfo.Status)
            {
                try
                {
                    #region 设置返回值
                    String sQuery = "";

                    if (model.ID == 0)
                    {
                        if (String.IsNullOrEmpty(model.USER_NAME)) { throw new Exception("USER_INFO_EMPTY"); }
                        Sys_Manager sys_Manager = (await _repository.GetListByTableEX<Sys_Manager>("*", "SYS_MANAGER", " AND USER_NAME = :USER_NAME", new { USER_NAME = model.USER_NAME })).FirstOrDefault();
						if (sys_Manager == null) { throw new Exception("USER_INFO_EMPTY"); }
						String organize_id = "";
						List<String> idList = _repository.QueryEx<String>("SELECT ID FROM SYS_ORGANIZE T START WITH T.ID IN (SELECT ORGANIZE_ID FROM SYS_USER_ORGANIZE WHERE MANAGER_ID=:USER_ID) CONNECT BY PRIOR T.ID=T.PARENT_ORGANIZE_ID", new { USER_ID = sys_Manager.ID });
						if (idList != null && idList.Count() > 0)
						{
							organize_id = String.Join(",", idList);
						}
						else
						{
							throw new Exception("ORGANIZE_INFO_EMPTY");
                        }
                        sQuery = "SELECT SF.FEEDER,FTYPE FEEDER_TYPE,FSIZE FEEDER_SIZE,STATUS FEEDER_STATUS,'" + model.USER_NAME + "' CHECK_USER,SYSDATE AS CHECK_TIME FROM SMT_FEEDER SF WHERE SF.STATUS NOT IN (6,7) AND SF.ORGANIZE_ID in (" + organize_id + ") ORDER BY FTYPE,FSIZE ";
                    }
                    else
                    {
                        sQuery = @"SELECT SF.FEEDER,SF.FBODYMARK,FD.FEEDER_TYPE,FD.FEEDER_SIZE,FC.FEEDER_STATUS,FC.CHECK_USER,FC.CHECK_TIME FROM SFCS_FEEDER_KEEP_DETAIL FD,SFCS_FEEDER_KEEP_CONTENT FC,SMT_FEEDER SF WHERE FD.ID = FC.KEEP_DETAIL_ID AND FC.FEEDER_ID = SF.ID AND FD.ID = :ID";
                    }
                    if (!model.FBODYMARK.IsNullOrEmpty())
                    {
                        sQuery += " AND SF.FEEDER = :FEEDER";
                    }
                    returnVM.Result = _repository.QueryEx<LoadPDAFeederCheckDetailListModel>(sQuery, new { ID = model.ID, FEEDER = model.FBODYMARK });

                    #endregion
                }
                catch (Exception ex)
                {
                    ErrorInfo.Set(_localizer[ex.Message], MethodBase.GetCurrentMethod(), EnumErrorType.Warning);
                }
            }

            #region 如果出现错误，则写错误日志并返回错误内容

            WriteLog(ref returnVM);

            #endregion

            return returnVM;

        }

        /// <summary>
        /// 获取需要点检的飞达类型列表
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpGet]
        [Authorize]
        public async Task<ApiBaseReturn<List<SfcsFeederKeepDetail>>> LoadPDAFeederCheckInfo([FromQuery] GetFeederInfoRequestModel model)
        {
            ApiBaseReturn<List<SfcsFeederKeepDetail>> returnVM = new ApiBaseReturn<List<SfcsFeederKeepDetail>>();
            if (!ErrorInfo.Status)
            {
                try
                {
                    #region 设置返回值
                    Sys_Manager sys_Manager = (await _repository.GetListByTableEX<Sys_Manager>("*", "SYS_MANAGER", " AND USER_NAME = :USER_NAME", new { USER_NAME = model.CHECK_USER })).FirstOrDefault();
                    if (sys_Manager == null) { throw new Exception("USER_INFO_EMPTY"); }
                    String organize_id = "";
                    List<String> idList = _repository.QueryEx<String>("SELECT ID FROM SYS_ORGANIZE T START WITH T.ID IN (SELECT ORGANIZE_ID FROM SYS_USER_ORGANIZE WHERE MANAGER_ID=:USER_ID) CONNECT BY PRIOR T.ID=T.PARENT_ORGANIZE_ID", new { USER_ID = sys_Manager.ID });
                    if (idList != null && idList.Count() > 0)
                    {
                        organize_id = String.Join(",", idList);
                    }
                    else
                    {
                        throw new Exception("ORGANIZE_INFO_EMPTY");
                    }

                    returnVM.Result = await _repository.GetListByTableEX<SfcsFeederKeepDetail>("'0' ID,'0' KEEP_HEAD_ID,FTYPE FEEDER_TYPE,FSIZE FEEDER_SIZE,COUNT(0) FEEDER_TYPE_TOTAL, '0' CHECK_QTY", "SMT_FEEDER", " AND STATUS NOT IN (6,7) AND ORGANIZE_ID in (" + organize_id + ") GROUP BY FTYPE,FSIZE");

                    #endregion
                }
                catch (Exception ex)
                {
                    ErrorInfo.Set(_localizer[ex.Message], MethodBase.GetCurrentMethod(), EnumErrorType.Warning);
                }
            }

            #region 如果出现错误，则写错误日志并返回错误内容

            WriteLog(ref returnVM);

            #endregion

            return returnVM;

        }

        /// <summary>
        /// 保存飞达次数区间数据
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
		[Authorize]
		public async Task<ApiBaseReturn<bool>> SaveFeederRegionData([FromBody] SmtFeederRegionModel model)
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
                        decimal resdata = await _repository.SaveFeederRegionDataByTrans(model);
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
		/// 获取飞达次数区间
		/// </summary>
		/// <param name="model"></param>
		/// <returns></returns>
        [HttpGet]
        [Authorize]
        public async Task<ApiBaseReturn<string>> LoadeFeederRegionData([FromQuery] SmtStencilRegionRequestModel model)
        {

            ApiBaseReturn<string> returnVM = new ApiBaseReturn<string>();
            if (!ErrorInfo.Status)
            {
                try
                {
                    #region 检查参数

                    #endregion

                    #region 设置返回值

                    if (!ErrorInfo.Status)
                    {
                        var res = await _repository.LoadeFeederRegionData(model);

                        returnVM.Result = JsonHelper.ObjectToJSONOfDate(res?.data);
                        returnVM.TotalCount = res?.count ?? 0;
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