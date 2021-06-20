/*
*┌──────────────────────────────────────────────────────────────┐
*│　描    述：自动产生序列号记录表，使用在自动产生卡通号，自动产生栈板号等。 控制器                                                    
*│　作    者：嘉志科技                                              
*│　版    本：3.0   模板代码自动生成                                              
*│　创建时间：2020-11-10 10:26:22                           
*└──────────────────────────────────────────────────────────────┘
*┌──────────────────────────────────────────────────────────────┐
*│　命名空间： JZ.IMS.WebApi.Controllers                                   
*│　接口名称： ISfcsContainerListController                                      
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
using JZ.IMS.Core;
using System.Data;
using JZ.IMS.Core.DbHelper;

namespace JZ.IMS.WebApi.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class SfcsContainerListController : BaseController
    {
        private readonly ISfcsContainerListRepository _repository;
        private readonly ISfcsRuncardRangerRepository _sfcsRuncardRangerRepository;
        private readonly ISfcsOperationSitesRepository _sfcsOperationSitesRepository;
        private readonly ISfcsOperationLinesRepository _sfcsOperationLinesRepository;
        private readonly ISfcsRuncardRepository _sfcsRuncardRepository;
        private readonly ISfcsCollectUidsRepository _sfcsCollectUidsRepository;
        private readonly ISfcsWoRepository _sfcsWoRepository;
        private readonly ISfcsPnRepository _sfcsPnRepository;
        private readonly ISfcsProductConfigRepository _sfcsProductConfigRepository;
        private readonly IMesMiddleCodeRepository _repositoryMC;
        private readonly ISfcsReworkRepository _repositoryRework;
        private readonly IMapper _mapper;
        private readonly ISfcsReworkRepository _sfcsReworkRepository;
        private readonly ISfcsPrintTasksRepository _sfcsPrintTasksRepository;

        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IStringLocalizer<SfcsContainerListController> _localizer;
        private readonly IStringLocalizer<SfcsRuncardRangerRulesController> _sfcsRuncardRangerlocalizerLocalizer;
        private readonly IStringLocalizer<AssemblyOperationController> _assemblyLocalizer;
        private readonly IStringLocalizer<MesMiddleCodeController> _localizerMC;

        public SfcsContainerListController(ISfcsContainerListRepository repository,
            ISfcsRuncardRangerRepository sfcsRuncardRangerRepository,
            ISfcsRuncardRepository sfcsRuncardRepository,
            ISfcsOperationSitesRepository sfcsOperationSitesRepository,
            ISfcsOperationLinesRepository sfcsOperationLinesRepository,
            ISfcsCollectUidsRepository sfcsCollectUidsRepository,
            ISfcsWoRepository sfcsWoRepository,
            ISfcsPnRepository sfcsPnRepository,
            ISfcsProductConfigRepository sfcsProductConfigRepository,
            ISfcsReworkRepository repositoryRework,
            IMapper mapper, IHttpContextAccessor httpContextAccessor,
            ISfcsReworkRepository sfcsReworkRepository,
            ISfcsPrintTasksRepository sfcsPrintTasksRepository,
            IMesMiddleCodeRepository repositoryMC,
            IStringLocalizer<SfcsContainerListController> localizer,
            IStringLocalizer<SfcsRuncardRangerRulesController> sfcsRuncardRangerlocalizer,
            IStringLocalizer<AssemblyOperationController> assemblylocalizer,
            IStringLocalizer<MesMiddleCodeController> localizerMC

            )
        {
            _repository = repository;
            _sfcsRuncardRepository = sfcsRuncardRepository;
            _sfcsOperationSitesRepository = sfcsOperationSitesRepository;
            _sfcsOperationLinesRepository = sfcsOperationLinesRepository;
            _sfcsCollectUidsRepository = sfcsCollectUidsRepository;
            _sfcsWoRepository = sfcsWoRepository;
            _sfcsPnRepository = sfcsPnRepository;
            _sfcsProductConfigRepository = sfcsProductConfigRepository;
            _sfcsRuncardRangerRepository = sfcsRuncardRangerRepository;
            _mapper = mapper;
            _httpContextAccessor = httpContextAccessor;
            _localizer = localizer;
            _sfcsRuncardRangerlocalizerLocalizer = sfcsRuncardRangerlocalizer;
            _assemblyLocalizer = assemblylocalizer;
            _sfcsReworkRepository = sfcsReworkRepository;
            _repositoryMC = repositoryMC;
            _repositoryRework = repositoryRework;
            _localizerMC = localizerMC;
            _sfcsPrintTasksRepository = sfcsPrintTasksRepository;
        }

        /// <summary>
        /// 首页视图
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
        /// 查询数据
        /// 搜索按钮对应的处理也是这个方法
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>	
        [HttpGet]
        [Authorize]
        public async Task<ApiBaseReturn<List<SfcsContainerListListModel>>> LoadData([FromQuery] SfcsContainerListRequestModel model)
        {
            ApiBaseReturn<List<SfcsContainerListListModel>> returnVM = new ApiBaseReturn<List<SfcsContainerListListModel>>();
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
                    var viewList = new List<SfcsContainerListListModel>();
                    list?.ForEach(x =>
                    {
                        var item = _mapper.Map<SfcsContainerListListModel>(x);
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
        /// 根据SN替换箱号
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        [Authorize]
        public async Task<ApiBaseReturn<CartonInfoListModel>> UpdateCartonNoBySN([FromQuery] SfcsContainerListRequestModel model)
        {
            ApiBaseReturn<CartonInfoListModel> returnVM = new ApiBaseReturn<CartonInfoListModel>();
            if (!ErrorInfo.Status)
            {
                try
                {
                    #region 检查参数
                    if (model.SN.IsNullOrEmpty())
                    {
                        if (model.Date.IsNullOrEmpty() && !ErrorInfo.Status)
                        {
                            ErrorInfo.Set(_localizer["DATE_NOT_NULL"], MethodBase.GetCurrentMethod(), EnumErrorType.Warning);
                        }
                        else
                        {
                            //用SN查箱号信息
                            if (Convert.ToBoolean(model.Type.ToUpper()))
                            {
                                SfcsRuncard sfcsRuncard = _repository.QueryEx<SfcsRuncard>("SELECT * FROM SFCS_RUNCARD WHERE SN = :SN", new { SN = model.Date }).FirstOrDefault();
                                //if (sfcsRuncard == null || sfcsRuncard.CARTON_NO.IsNullOrEmpty())
                                //{
                                //    ErrorInfo.Set(_localizer["SN_NO_NOT_INFO"], MethodBase.GetCurrentMethod(), EnumErrorType.Warning);
                                //}
                                if (!ErrorInfo.Status)
                                {
                                    model.CARTON_NO = (sfcsRuncard == null || sfcsRuncard.CARTON_NO.IsNullOrEmpty()) ? "" :sfcsRuncard.CARTON_NO;
                                }
                            }
                            else
                            {
                                //用卡通号查箱号信息
                                //1.判断当前Date是箱号或者SN
                                SfcsContainerList container = _repository.QueryEx<SfcsContainerList>("SELECT * FROM SFCS_CONTAINER_LIST WHERE CONTAINER_SN = :CONTAINER_SN ", new { CONTAINER_SN = model.Date }).FirstOrDefault();
                                if (container == null)
                                {
                                    SfcsRuncard sfcsRuncard = _repository.QueryEx<SfcsRuncard>("SELECT * FROM SFCS_RUNCARD WHERE SN = :SN", new { SN = model.Date }).FirstOrDefault();
                                    if (sfcsRuncard == null || model.CARTON_NO.IsNullOrEmpty())
                                    {
                                        ErrorInfo.Set(_localizer["DATE_ERROR"], MethodBase.GetCurrentMethod(), EnumErrorType.Warning);
                                    }
                                    else
                                    {
                                        String oldCARTON_NO = "";
                                        SfcsContainerList sfcsContainerList = _repository.QueryEx<SfcsContainerList>("SELECT * FROM SFCS_CONTAINER_LIST WHERE CONTAINER_SN = :CONTAINER_SN ", new { CONTAINER_SN = model.CARTON_NO }).FirstOrDefault();// && sfcsContainerList.FULL_FLAG == "Y"
                                        if (sfcsContainerList == null || sfcsContainerList.CONTAINER_SN.IsNullOrEmpty())
                                        {
                                            ErrorInfo.Set(_localizer["CARTON_NO_ERROR"], MethodBase.GetCurrentMethod(), EnumErrorType.Warning);
                                        }
                                        else if (sfcsContainerList.QUANTITY > 0 && sfcsContainerList.QUANTITY == sfcsContainerList.SEQUENCE)
                                        {
                                            ErrorInfo.Set(_localizer["CARTON_NO_FULL"], MethodBase.GetCurrentMethod(), EnumErrorType.Warning);
                                        }
                                        else
                                        {
                                            //1.箱号和SN对比的产品料号是否相同 2.检查SN在SFCS_RUNCARD是否有箱号（有箱号代表已经包装过站）
                                            String sQuery = "SELECT SW.PART_NO,SR.CARTON_NO FROM SFCS_RUNCARD SR INNER JOIN SFCS_WO SW ON SR.WO_ID = SW.ID WHERE SR.SN = :SN ";
                                            SfcsCollectCartonsListModel cModel = _repository.QueryEx<SfcsCollectCartonsListModel>(sQuery, new { SN = model.Date }).FirstOrDefault();
                                            if (cModel == null || cModel.CARTON_NO.IsNullOrEmpty() || sfcsContainerList.PART_NO.IsNullOrEmpty() || sfcsContainerList.PART_NO != cModel.PART_NO)
                                            {
                                                ErrorInfo.Set(_localizer["PART_NO_ERROR"], MethodBase.GetCurrentMethod(), EnumErrorType.Warning);
                                            }
                                            else
                                            {
                                                oldCARTON_NO = cModel.CARTON_NO;
                                            }
                                        }
                                        if (!ErrorInfo.Status)
                                        {
                                            //替换箱号
                                            await _repository.UpdateCartonNoBySN(model.CARTON_NO, oldCARTON_NO, sfcsRuncard.ID);
                                        }
                                    }
                                }
                                else
                                {
                                    model.CARTON_NO = model.Date;
                                }
                            }
                        }
                    }
                    else
                    {
                        if (model.CARTON_NO.IsNullOrEmpty())
                        {
                            ErrorInfo.Set(_localizer["CARTON_NO_ERROR"], MethodBase.GetCurrentMethod(), EnumErrorType.Warning);
                        }
                    }
                    #endregion

                    #region 保存并返回

                    if (!ErrorInfo.Status)
                    {
                        returnVM.Result = await _repository.GetCartonInfoByCartonNo(model);
                        returnVM.TotalCount = Convert.ToInt32(returnVM.Result==null?0:returnVM.Result.CURRENTQTY);
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
        /// 箱号置满包装
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        [Authorize]
        public async Task<ApiBaseReturn<decimal>> SetCatonFullByCaton(PackageFullRequestModel model)
        {
            ApiBaseReturn<decimal> returnVM = new ApiBaseReturn<decimal>();
            if (!ErrorInfo.Status)
            {
                try
                {
                    #region 检查参数
                    if (model.CARTON_NO.IsNullOrEmpty() && !ErrorInfo.Status)
                    {
                        ErrorInfo.Set(_localizer["CARTON_NO_NOT_NULL"], MethodBase.GetCurrentMethod(), EnumErrorType.Warning);
                    }
                    if (model.USER_NAME.IsNullOrEmpty() && !ErrorInfo.Status)
                    {
                        ErrorInfo.Set(_localizer["USER_NAME_NOT_NULL"], MethodBase.GetCurrentMethod(), EnumErrorType.Warning);
                    }
                    #endregion

                    #region 保存并返回

                    if (!ErrorInfo.Status)
                    {
                        returnVM.Result = await _repository.SetCatonFullByCaton(model);
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
        /// 产品漏刷检查
        /// </summary>
        /// <param name="carton_no">卡通号（箱号）</param>
        /// <param name="sn">产品流水号</param>
        /// <returns></returns>
        [HttpPost]
        [Authorize]
        public async Task<ApiBaseReturn<bool>> CartonOmissionCheck(string carton_no, string sn)
        {
            ApiBaseReturn<bool> returnVM = new ApiBaseReturn<bool>();
            if (!ErrorInfo.Status)
            {
                try
                {
                    #region 检查参数并设置返回值
                    //产品漏刷包装检查PDA实现产品漏刷检查（扫描箱号，产品条码），下面有个信息区显示检查信息
                    if ((carton_no.IsNullOrEmpty() || sn.IsNullOrEmpty()) && !ErrorInfo.Status)
                    {
                        ErrorInfo.Set(_localizer["CARTON_SN_NOT_NULL"], MethodBase.GetCurrentMethod(), EnumErrorType.Warning);
                    }
                    else
                    {
                        //箱号信息
                        SfcsContainerList container = _repository.QueryEx<SfcsContainerList>("SELECT * FROM SFCS_CONTAINER_LIST WHERE CONTAINER_SN = :CONTAINER_SN ", new { CONTAINER_SN = carton_no }).FirstOrDefault();
                        if (container == null)//箱号不存在
                        {
                            ErrorInfo.Set(_localizer["CARTON_NO_ERROR"], MethodBase.GetCurrentMethod(), EnumErrorType.Warning);
                        }
                        else
                        {
                            //产品流水号信息
                            String sQuery = "SELECT SW.PART_NO,SR.CARTON_NO,SR.WIP_OPERATION,SR.LAST_OPERATION FROM SFCS_RUNCARD SR INNER JOIN SFCS_WO SW ON SR.WO_ID = SW.ID WHERE SR.SN = :SN ";
                            CartonOmissionCheckListModel cModel = _repository.QueryEx<CartonOmissionCheckListModel>(sQuery, new { SN = sn }).FirstOrDefault();
                            if (cModel == null)
                            {
                                ErrorInfo.Set(_localizer["SN_ERROR"], MethodBase.GetCurrentMethod(), EnumErrorType.Warning);
                            }
                            else if (!cModel.CARTON_NO.IsNullOrEmpty())
                            {
                                returnVM.Result = true;
                                returnVM.ResultInfo = "当前流水号已在箱号为:" + cModel.CARTON_NO + "的箱子中,校验失败！";
                            }
                            else
                            {
                                //验证当前SN 所属的箱号和输入的箱号是否一致
                                if (cModel == null || cModel.PART_NO.IsNullOrEmpty() || container.PART_NO.IsNullOrEmpty())
                                {
                                    returnVM.Result = true;
                                    returnVM.ResultInfo = "当前输入的产品流水号(" + sn + ")和箱号(" + carton_no + "),校验失败！";
                                }
                                else if (cModel.PART_NO != container.PART_NO)
                                {
                                    returnVM.Result = true;
                                    returnVM.ResultInfo = "当前产品流水号所属箱号和输入的箱号不一致,校验失败！";
                                }
                                else if (cModel.WIP_OPERATION != GlobalVariables.EndOperation || cModel.LAST_OPERATION != GlobalVariables.EndOperation)
                                {
                                    returnVM.Result = true;
                                    returnVM.ResultInfo = "当前产品流水号(" + sn + ")未过站结束,校验失败！";
                                }
                                else
                                {
                                    //更新箱号
                                    returnVM.Result = await _repository.UpdateCartonNoBySN(carton_no, sn) > 0 ? true : false;
                                    returnVM.ResultInfo = "当前输入的产品流水号(" + sn + ")和箱号(" + carton_no + "),校验成功！";
                                }
                            }
                        }
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
        /// 新增SN包装过站
        /// </summary>
        /// <param name="model">包装实体</param>
        /// <returns></returns>
        [HttpPost]
        [Authorize]
        public async Task<ApiBaseReturn<bool>> PackingDoProcess([FromBody] SfcsContainerProcessRequestMolde model)
        {
            ApiBaseReturn<bool> returnVM = new ApiBaseReturn<bool>();
            returnVM.Result = false;
            if (!ErrorInfo.Status)
            {
                try
                {
                    #region 参数检验

                    if (!ErrorInfo.Status && model.LineId <= 0) ErrorInfo.Set(_localizer["LINEID_NOT_NULL"], MethodBase.GetCurrentMethod(), EnumErrorType.Error);
                    if (!ErrorInfo.Status && model.SiteId <= 0) ErrorInfo.Set(_localizer["SITE_NOT_NULL"], MethodBase.GetCurrentMethod(), EnumErrorType.Error);
                    if (!ErrorInfo.Status && model.SN.IsNullOrEmpty()) ErrorInfo.Set(_localizer["SN_NOT_NULL"], MethodBase.GetCurrentMethod(), EnumErrorType.Error);
                    if (!ErrorInfo.Status && model.CartonNo.IsNullOrEmpty()) ErrorInfo.Set(_localizer["CARTON_SN_NOT_NULL"], MethodBase.GetCurrentMethod(), EnumErrorType.Error);

                    #endregion

                    #region 设置返回值
                    if (!ErrorInfo.Status)
                    {
                        var cartonMolde = (await _sfcsRuncardRepository.GetListByTableEX<SfcsContainerList>("*", "SFCS_CONTAINER_LIST", " AND CONTAINER_SN=:CONTAINER_SN AND FULL_FLAG='Y'", new { CONTAINER_SN = model.CartonNo }))?.FirstOrDefault();
                        if (cartonMolde == null)
                        {
                            returnVM.Result = (await PackingProcessAsync(model));
                        }
                        else
                        {
                            //箱子已经刷满，不能再刷了。
                            throw new Exception(_localizer["FULL_FAILURE"]);
                        }

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
        /// 修改SN包装
        /// </summary>
        /// <param name="model">包装实体</param>
        /// <returns></returns>
        [HttpPost]
        [Authorize]
        public async Task<ApiBaseReturn<bool>> PackingModifyBySN([FromBody] SfcsContainerProcessRequestMolde model)
        {
            ApiBaseReturn<bool> returnVM = new ApiBaseReturn<bool>();
            returnVM.Result = false;
            if (!ErrorInfo.Status)
            {
                try
                {
                    #region 参数检验

                    if (!ErrorInfo.Status && model.LineId <= 0) ErrorInfo.Set(_localizer["LINEID_NOT_NULL"], MethodBase.GetCurrentMethod(), EnumErrorType.Error);
                    if (!ErrorInfo.Status && model.SiteId <= 0) ErrorInfo.Set(_localizer["SITE_NOT_NULL"], MethodBase.GetCurrentMethod(), EnumErrorType.Error);
                    if (!ErrorInfo.Status && model.SN.IsNullOrEmpty()) ErrorInfo.Set(_localizer["NEWSN_NOT_NULL"], MethodBase.GetCurrentMethod(), EnumErrorType.Error);
                    if (!ErrorInfo.Status && model.OldSN.IsNullOrEmpty()) ErrorInfo.Set(_localizer["OldSN_NOT_NULL"], MethodBase.GetCurrentMethod(), EnumErrorType.Error);
                    if (!ErrorInfo.Status && model.CartonNo.IsNullOrEmpty()) ErrorInfo.Set(_localizer["CARTON_SN_NOT_NULL"], MethodBase.GetCurrentMethod(), EnumErrorType.Error);

                    #endregion

                    #region 设置返回值
                    if (!ErrorInfo.Status)
                    {
                        try
                        {
                            //1.将旧SN进行返工处理
                            var result = await _repository.ReworkProcessBySN(_sfcsReworkRepository, model.OldSN, model.UserName);
                            if (result)
                            {
                                //2.2将新SN做新增处理(进行a新增)
                                returnVM.Result = (await PackingProcessAsync(model));

                            }

                        }
                        catch (Exception ex)
                        {
                            throw ex;
                        }
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
        /// 删除SN包装
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [Authorize]
        public async Task<ApiBaseReturn<bool>> PackingDelBySN([FromBody] List<SfcsContainerProcessRequestMolde> modelList)
        {
            ApiBaseReturn<bool> returnVM = new ApiBaseReturn<bool>();
            returnVM.Result = false;
            if (!ErrorInfo.Status)
            {
                try
                {
                    #region 参数检验

                    if (!ErrorInfo.Status && modelList.Count(c => c.LineId <= 0) > 0) ErrorInfo.Set(_localizer["LINEID_NOT_NULL"], MethodBase.GetCurrentMethod(), EnumErrorType.Error);
                    if (!ErrorInfo.Status && modelList.Count(c => c.SiteId <= 0) > 0) ErrorInfo.Set(_localizer["SITE_NOT_NULL"], MethodBase.GetCurrentMethod(), EnumErrorType.Error);
                    if (!ErrorInfo.Status && modelList.Count(c => c.SN.IsNullOrEmpty()) > 0) ErrorInfo.Set(_localizer["NEWSN_NOT_NULL"], MethodBase.GetCurrentMethod(), EnumErrorType.Error);
                    if (!ErrorInfo.Status && modelList.Count(c => c.CartonNo.IsNullOrEmpty()) > 0) ErrorInfo.Set(_localizer["CARTON_SN_NOT_NULL"], MethodBase.GetCurrentMethod(), EnumErrorType.Error);

                    #endregion

                    #region 设置返回值
                    if (!ErrorInfo.Status)
                    {
                        IDbConnection dbConnection = await _sfcsRuncardRepository.GetConnection();
                        ConnectionFactory.OpenConnection(dbConnection);
                        using (var tran = dbConnection.BeginTransaction())
                        {
                            try
                            {
                                foreach (var model in modelList)
                                {
                                    //1.将旧SN进行返工处理
                                    var result = await _repository.ReworkProcessBySN(_sfcsReworkRepository, model.SN, model.UserName);
                                    if (result)
                                    {
                                        //3.修改SFCS_CONTAINER_LIST表中的sequence数量（通过runcard表中有箱号相同的行数）
                                        var dataresult = await _repository.DelCartonNoBySN(model.CartonNo, model.SN, tran);
                                        returnVM.Result = dataresult > 0 ? true : false;
                                    }
                                }
                                tran.Commit();
                            }
                            catch (Exception ex)
                            {
                                tran.Rollback();
                                throw ex;
                            }
                            finally
                            {
                                dbConnection.Close();
                            }

                        }
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

        #region 内部类
        public class SfcsContainerProcessRequestMolde
        {
            /// <summary>
            /// 线体ID
            /// </summary>
            public decimal LineId { get; set; }

            /// <summary>
            /// 站点ID
            /// </summary>
            public decimal SiteId { get; set; }

            /// <summary>
            /// 新SN条码
            /// </summary>
            public string SN { get; set; }

            /// <summary>
            /// 旧SN条码(新增时不用传,修改时候 传)
            /// </summary>
            public string OldSN { get; set; }

            /// <summary>
            /// 用户
            /// </summary>
            public string UserName { get; set; }

            /// <summary>
            /// 卡通号
            /// </summary>
            public string CartonNo { get; set; }
        }
        #endregion

        #region 内部方法
        /// <summary>
        /// 包装过站
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        private async Task<bool> PackingProcessAsync(SfcsContainerProcessRequestMolde model)
        {
            //AssemblyOperationController controller = new AssemblyOperationController(_sfcsRuncardRangerRepository, _sfcsRuncardRepository, _sfcsOperationSitesRepository, _sfcsOperationLinesRepository, _sfcsCollectUidsRepository, _sfcsWoRepository, _sfcsPnRepository, _sfcsProductConfigRepository, _sfcsPrintTasksRepository, _repositoryMC, _repositoryRework, _httpContextAccessor, _sfcsRuncardRangerlocalizerLocalizer, _assemblyLocalizer, _localizerMC);
            bool result = false;
            try
            {
                //IDbConnection dbConnection = await _sfcsRuncardRepository.GetConnection();
                //ConnectionFactory.OpenConnection(dbConnection);
                //using (var tran = dbConnection.BeginTransaction())
                //{
                //    try
                //    {
                //        //1.1处理过站实体对象
                //        #region 处理过站实体对象
                //        Sys_Manager sys_Manager = _repository.QueryEx<Sys_Manager>("select * from SYS_MANAGER where USER_NAME = :USER_NAME",
                //                       new
                //                       {
                //                           USER_NAME = model.UserName
                //                       }).FirstOrDefault();
                //        if (sys_Manager == null)
                //        {
                //            //"用户信息不存在!"
                //            throw new Exception(_sfcsRuncardRangerlocalizerLocalizer["Err_UserNotEixst"]);
                //        }
                //        sys_Manager.PASSWORD = "";
                //        sys_Manager.PASSWORD_SALT = "";
                //        SfcsOperationSites sfcsOperationSites = await _sfcsOperationSitesRepository.GetAsync(model.SiteId);
                //        if (sfcsOperationSites == null)
                //        {
                //            throw new Exception(_sfcsRuncardRangerlocalizerLocalizer["Err_SiteIsnotEixst"]);

                //        }
                //        SfcsOperationLines sfcsOperationLines = await _sfcsOperationLinesRepository.GetAsync((Decimal)model.LineId);
                //        if (sfcsOperationLines == null)
                //        {
                //            throw new Exception(_sfcsRuncardRangerlocalizerLocalizer["Err_OperationSiteLinesIsNotEixst"]);

                //        }
                //        Propertyprovider propertyprovider = new Propertyprovider();
                //        propertyprovider.data = model.SN;
                //        propertyprovider.sfcsOperationLines = sfcsOperationLines;
                //        propertyprovider.sfcsOperationSites = sfcsOperationSites;
                //        propertyprovider.sys_Manager = sys_Manager;

                //        #endregion

                //        //1.2将新增SN进行包装过站
                //        propertyprovider = await controller.DoProcessAsync(propertyprovider, tran);
                //        //2.修改SN的Runcard表中的Caton_no修改成当前箱号
                //        //3、修改SFCS_CONTAINER_LIST表中的sequence数量（通过runcard表中有箱号相同的行数）
                //        var dataResult = await _sfcsRuncardRepository.UpdateCartonNoBySNEx(model.CartonNo, model.SN, tran);
                //        result = dataResult > 0 ? true : false;
                //        tran.Commit();
                //    }
                //    catch (Exception ex)
                //    {
                //        tran.Rollback();
                //        //propertyprovider.result = 2;
                //        //propertyprovider.msg = ex.Message;
                //        // ErrorInfo.Set(ex.Message, MethodBase.GetCurrentMethod(), EnumErrorType.Error);
                //        throw ex;
                //    }
                //    finally
                //    {
                //        dbConnection.Close();
                //    }
                //}
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return result;
        }

        #endregion

    }
}