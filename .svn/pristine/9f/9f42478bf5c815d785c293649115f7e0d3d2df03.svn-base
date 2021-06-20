using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.IO;
using System.Linq;
using System.Net;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using JZ.IMS.Core;
using JZ.IMS.Core.DbHelper;
using JZ.IMS.Core.Extensions;
using JZ.IMS.Core.Utilities;
using JZ.IMS.IRepository;
using JZ.IMS.Models;
using JZ.IMS.ViewModels;
using JZ.IMS.WebApi.Common;
using JZ.IMS.WebApi.Public;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.Extensions.Localization;

namespace JZ.IMS.WebApi.Controllers
{
    /// <summary>
    /// 有码过站采集接口
    /// </summary>
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class AssemblyOperationController : BaseController
    {
        private readonly ISfcsRuncardRangerRepository _repository;

        private readonly ISfcsOperationSitesRepository _sfcsOperationSitesRepository;

        private readonly ISfcsOperationLinesRepository _sfcsOperationLinesRepository;

        private readonly ISfcsRuncardRepository _sfcsRuncardRepository;

        private readonly ISfcsCollectUidsRepository _sfcsCollectUidsRepository;

        private readonly ISfcsWoRepository _sfcsWoRepository;

        private readonly ISfcsPnRepository _sfcsPnRepository;

        private readonly ISfcsProductConfigRepository _sfcsProductConfigRepository;

        private readonly ISfcsPrintTasksRepository _sfcsPrintTasksRepository;

        private readonly IMesMiddleCodeRepository _repositoryMC;

        private readonly ISfcsReworkRepository _repositoryRework;
        private readonly IImportRuncardSnRepository _importRuncardSnRepository;
        private readonly ISfcsContainerListRepository _sfcsContainerListrepository;
        private readonly IHttpContextAccessor _httpContextAccessor;

        private readonly IStringLocalizer<SfcsRuncardRangerRulesController> _localizer;

        private readonly IStringLocalizer<AssemblyOperationController> _localizerA;

        private readonly IStringLocalizer<MesMiddleCodeController> _localizerMC;

        public AssemblyOperationController(ISfcsRuncardRangerRepository repository,
            ISfcsRuncardRepository sfcsRuncardRepository,
            ISfcsOperationSitesRepository sfcsOperationSitesRepository,
            ISfcsOperationLinesRepository sfcsOperationLinesRepository,
            ISfcsCollectUidsRepository sfcsCollectUidsRepository,
            ISfcsWoRepository sfcsWoRepository,
            ISfcsPnRepository sfcsPnRepository,
            ISfcsProductConfigRepository sfcsProductConfigRepository,
            ISfcsPrintTasksRepository sfcsPrintTasksRepository,
            IMesMiddleCodeRepository repositoryMC,
            IImportRuncardSnRepository importRuncardSnRepository,
            ISfcsReworkRepository repositoryRework,
            ISfcsContainerListRepository sfcsContainerListrepository,
            IHttpContextAccessor httpContextAccessor,
            IStringLocalizer<SfcsRuncardRangerRulesController> localizer,
            IStringLocalizer<AssemblyOperationController> localizerA,
            IStringLocalizer<MesMiddleCodeController> localizerMC)
        {
            _repository = repository;
            _sfcsRuncardRepository = sfcsRuncardRepository;
            _sfcsOperationSitesRepository = sfcsOperationSitesRepository;
            _sfcsOperationLinesRepository = sfcsOperationLinesRepository;
            _sfcsCollectUidsRepository = sfcsCollectUidsRepository;
            _sfcsWoRepository = sfcsWoRepository;
            _sfcsPnRepository = sfcsPnRepository;
            _sfcsProductConfigRepository = sfcsProductConfigRepository;
            _sfcsPrintTasksRepository = sfcsPrintTasksRepository;
            _repositoryMC = repositoryMC;
            _repositoryRework = repositoryRework;
            _importRuncardSnRepository = importRuncardSnRepository;
            _sfcsContainerListrepository = sfcsContainerListrepository;
            _httpContextAccessor = httpContextAccessor;
            _localizer = localizer;
            _localizerA = localizerA;
            _localizerMC = localizerMC;
        }

        public AssemblyOperationController()
        {
        }

        #region 公共接口方法
        /// <summary>
        /// 采集数据接口
        /// </summary>
        /// <param name="propertyprovider"></param>
        /// <returns></returns>
        [HttpPost]
        [Authorize]
        public async Task<ApiBaseReturn<Propertyprovider>> CollectData(Propertyprovider propertyprovider)
        {
            ApiBaseReturn<Propertyprovider> returnVM = new ApiBaseReturn<Propertyprovider>();
            if (!ErrorInfo.Status)
            {
                try
                {
                    #region 设置返回值
                    if (!ErrorInfo.Status)
                    {
                        //1、校验站点信息
                        if (propertyprovider.sfcsOperationSites == null || propertyprovider.sfcsOperationLines == null)
                        {
                            throw new Exception("请选择站点信息!");
                        }
                        IDbConnection dbConnection = await _sfcsRuncardRepository.GetConnection();
                        ConnectionFactory.OpenConnection(dbConnection);
                        using (var tran = dbConnection.BeginTransaction())
                        {
                            try
                            {
                                propertyprovider = await DoProcessAsync(propertyprovider, tran);
                                tran.Commit();
                            }
                            catch (Exception ex)
                            {
                                tran.Rollback();
                                propertyprovider.result = 2;
                                propertyprovider.msg = ex.Message;
                                ErrorInfo.Set(ex.Message, MethodBase.GetCurrentMethod(), EnumErrorType.Error);
                            }
                            finally
                            {
                                returnVM.Result = propertyprovider;
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
        /// lingk栈板作业
        /// </summary>
        /// <param name="propertyprovider"></param>
        /// <returns></returns>
        [HttpPost]
        [Authorize]
        public async Task<ApiBaseReturn<Propertyprovider>> CollectPalletData(Propertyprovider propertyprovider)
        {
            ApiBaseReturn<Propertyprovider> returnVM = new ApiBaseReturn<Propertyprovider>();
            if (!ErrorInfo.Status)
            {
                try
                {
                    #region 设置返回值
                    if (!ErrorInfo.Status)
                    {
                        //1、校验站点信息
                        if (propertyprovider.sfcsOperationSites == null || propertyprovider.sfcsOperationLines == null)
                        {
                            throw new Exception("请选择站点信息!");
                        }
                        IDbConnection dbConnection = await _sfcsRuncardRepository.GetConnection();
                        ConnectionFactory.OpenConnection(dbConnection);
                        using (var tran = dbConnection.BeginTransaction())
                        {
                            try
                            {
                                bool palletVerify = false;
                                //重置结果数据
                                propertyprovider.result = 0;
                                propertyprovider.msg = string.Empty;

                                List<SfcsRuncard> sfcsRuncardList = _sfcsRuncardRepository.GetListEx<SfcsRuncard>("WHERE CARTON_NO = :CARTON_NO ", new
                                {
                                    CARTON_NO = propertyprovider.data
                                }).ToList();
                                //检验输入的是否为采集栈板数据
                                if (propertyprovider.pallet._SfcsProductPallet != null && propertyprovider.product != null)
                                {
                                    palletVerify = this.VerifyPalletList(propertyprovider);
                                    if (palletVerify)
                                    {
                                        sfcsRuncardList = _sfcsRuncardRepository.GetListEx<SfcsRuncard>("WHERE CARTON_NO = :CARTON_NO ", new
                                        {
                                            CARTON_NO = propertyprovider.sfcsRuncard.CARTON_NO
                                        }).ToList();
                                        propertyprovider.pallet.Pallet_NO = propertyprovider.data;
                                    }
                                    else
                                    {
                                        throw new Exception(String.Format("输入的条码:{0}无效", propertyprovider.data));
                                    }
                                }
                                else
                                {
                                    if (sfcsRuncardList == null || sfcsRuncardList.Count <= 0)
                                    {
                                        throw new Exception("输入的箱号不存在!");
                                    }
                                }
                                foreach (SfcsRuncard sfcsRuncard in sfcsRuncardList)
                                {
                                    if(propertyprovider.sfcsRuncard != null) propertyprovider.sfcsRuncard = sfcsRuncard;
                                    propertyprovider.data = sfcsRuncard.SN;
                                    propertyprovider = await DoProcessAsync(propertyprovider, tran);
                                    if (propertyprovider.result == 1) break;
                                }
                                if (propertyprovider.pallet != null && propertyprovider.result == 0)//过站成功
                                {
                                    propertyprovider.pallet.CurrentQty = UpdatePalletCurrentQty(propertyprovider, tran, palletVerify);
                                    propertyprovider.pallet.Status = StandardObjectStatusType.Incompleted;
                                    propertyprovider.pallet._SfcsProductPallet = null;
                                    propertyprovider.product = null;
                                }
                                tran.Commit();
                            }
                            catch (Exception ex)
                            {
                                tran.Rollback();
                                propertyprovider.result = 2;
                                propertyprovider.msg = ex.Message;
                                ErrorInfo.Set(ex.Message, MethodBase.GetCurrentMethod(), EnumErrorType.Error);
                            }
                            finally
                            {
                                returnVM.Result = propertyprovider;
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
        /// 置满lingk栈板作业
        /// </summary>
        /// <param name="propertyprovider"></param>
        /// <returns></returns>
        [HttpPost]
        [Authorize]
        public async Task<ApiBaseReturn<Propertyprovider>> SetPalletFull(Propertyprovider propertyprovider)
        {
            ApiBaseReturn<Propertyprovider> returnVM = new ApiBaseReturn<Propertyprovider>();
            if (!ErrorInfo.Status)
            {
                try
                {
                    #region 设置返回值
                    if (!ErrorInfo.Status)
                    {
                        //1、校验站点信息
                        if (propertyprovider.sfcsOperationSites == null || propertyprovider.sfcsOperationLines == null)
                        {
                            throw new Exception("请选择站点信息!");
                        }
                        if (propertyprovider.pallet == null || propertyprovider.pallet.Pallet_NO == null)
                        {
                            throw new Exception("箱号不存在无法打印包装条码!");
                        }
                        else
                        {
                            IDbConnection dbConnection = await _sfcsRuncardRepository.GetConnection();
                            ConnectionFactory.OpenConnection(dbConnection);
                            using (var tran = dbConnection.BeginTransaction())
                            {
                                //执行校验的JOb（Finally RUN JOB）
                                try
                                {
                                    String UpdateContain = @"update SFCS_CONTAINER_LIST SET FULL_FLAG = :FULL_FLAG WHERE CONTAINER_SN = :CONTAINER_SN";
                                    _sfcsRuncardRepository.Execute(UpdateContain, new
                                    {
                                        FULL_FLAG = "Y",
                                        CONTAINER_SN = propertyprovider.pallet.Pallet_NO
                                    }, tran);
                                    Job.FinallyJobStorage.AutoLinkPallect autoLinkPallet = new Job.FinallyJobStorage.AutoLinkPallect();
                                    KeyValuePair<bool, string> result = await autoLinkPallet.GetTask(propertyprovider, _sfcsRuncardRepository, tran);
                                    if (!result.Key)
                                    {
                                        throw new Exception(result.Value);
                                    }
                                    tran.Commit();
                                }
                                catch (Exception ex)
                                {
                                    tran.Rollback();
                                    propertyprovider.result = 2;
                                    propertyprovider.msg = ex.Message;
                                    ErrorInfo.Set(ex.Message, MethodBase.GetCurrentMethod(), EnumErrorType.Error);
                                }
                                finally
                                {
                                    returnVM.Result = propertyprovider;
                                }
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
        /// 保存站点
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [Authorize]
        public async Task<ApiBaseReturn<Propertyprovider>> SaveSite(
            SaveSiteModelcs saveSiteModelcs)
        {

            Decimal siteId = saveSiteModelcs.SiteId;
            String userName = saveSiteModelcs.UserName;
            ApiBaseReturn<Propertyprovider> returnVM = new ApiBaseReturn<Propertyprovider>();
            if (!ErrorInfo.Status)
            {
                try
                {
                    #region 设置返回值

                    if (!ErrorInfo.Status)
                    {
                        Sys_Manager sys_Manager = _repository.QueryEx<Sys_Manager>("select * from SYS_MANAGER where USER_NAME = :USER_NAME",
                            new
                            {
                                USER_NAME = userName
                            }).FirstOrDefault();
                        if (sys_Manager == null)
                        {
                            ErrorInfo.Set(_localizerA["USER_NAME_NOT_EMPTY"], MethodBase.GetCurrentMethod(), EnumErrorType.Error);
                            return returnVM;
                        }
                        sys_Manager.PASSWORD = "";
                        sys_Manager.PASSWORD_SALT = "";
                        SfcsOperationSites sfcsOperationSites = await _sfcsOperationSitesRepository.GetAsync(siteId);
                        if (sfcsOperationSites == null)
                        {
                            ErrorInfo.Set(_localizer["Err_SiteIsnotEixst"], MethodBase.GetCurrentMethod(), EnumErrorType.Error);
                            return returnVM;
                        }
                        SfcsOperationLines sfcsOperationLines = await _sfcsOperationLinesRepository.GetAsync((Decimal)sfcsOperationSites.OPERATION_LINE_ID);
                        if (sfcsOperationLines == null)
                        {
                            ErrorInfo.Set(_localizer["Err_OperationSiteLinesIsNotEixst"], MethodBase.GetCurrentMethod(), EnumErrorType.Error);
                            return returnVM;
                        }
                        Propertyprovider propertyprovider = new Propertyprovider();
                        propertyprovider.sfcsOperationLines = sfcsOperationLines;
                        propertyprovider.sfcsOperationSites = sfcsOperationSites;
                        propertyprovider.sys_Manager = sys_Manager;
                        var operationCategory = _repository.ExecuteScalar(@"SELECT OPERATION_CATEGORY FROM SFCS_OPERATIONS WHERE ID = :OPERATION_ID",
                                                        new { OPERATION_ID = propertyprovider.sfcsOperationSites.OPERATION_ID });
                        //判断当前工序是否为包装类型
                        if (operationCategory == GlobalVariables.OPERATION_CATEGORY_PACKING)
                        {
                            propertyprovider.carton = new Carton();
                        }
                        //判断当前工序是否为栈板类型
                        if (operationCategory == GlobalVariables.OPERATION_CATEGORY_PALLET)
                        {
                            propertyprovider.pallet = new Pallet();
                        }
                        returnVM.Result = propertyprovider;
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
        /// 保存站点(用于采集过站周转箱打印)
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [Authorize]
        public async Task<ApiBaseReturn<Propertyprovider>> SaveSitePrint(SaveSiteModelcs saveSiteModelcs)
        {

            Decimal siteId = saveSiteModelcs.SiteId;
            String userName = saveSiteModelcs.UserName;
            ApiBaseReturn<Propertyprovider> returnVM = new ApiBaseReturn<Propertyprovider>();
            if (!ErrorInfo.Status)
            {
                try
                {
                    #region 设置返回值

                    if (!ErrorInfo.Status)
                    {
                        Sys_Manager sys_Manager = _repository.QueryEx<Sys_Manager>("select * from SYS_MANAGER where USER_NAME = :USER_NAME",
                            new
                            {
                                USER_NAME = userName
                            }).FirstOrDefault();
                        if (sys_Manager == null)
                        {
                            ErrorInfo.Set(_localizerA["USER_NAME_NOT_EMPTY"], MethodBase.GetCurrentMethod(), EnumErrorType.Error);
                            return returnVM;
                        }
                        sys_Manager.PASSWORD = "";
                        sys_Manager.PASSWORD_SALT = "";
                        SfcsOperationSites sfcsOperationSites = await _sfcsOperationSitesRepository.GetAsync(siteId);
                        if (sfcsOperationSites == null)
                        {
                            ErrorInfo.Set(_localizer["Err_SiteIsnotEixst"], MethodBase.GetCurrentMethod(), EnumErrorType.Error);
                            return returnVM;
                        }
                        SfcsOperationLines sfcsOperationLines = await _sfcsOperationLinesRepository.GetAsync((Decimal)sfcsOperationSites.OPERATION_LINE_ID);
                        if (sfcsOperationLines == null)
                        {
                            ErrorInfo.Set(_localizer["Err_OperationSiteLinesIsNotEixst"], MethodBase.GetCurrentMethod(), EnumErrorType.Error);
                            return returnVM;
                        }
                        Propertyprovider propertyprovider = new Propertyprovider();
                        propertyprovider.sfcsOperationLines = sfcsOperationLines;
                        propertyprovider.sfcsOperationSites = sfcsOperationSites;
                        propertyprovider.sys_Manager = sys_Manager;
                        propertyprovider.carton = new Carton();
                        if (propertyprovider.sfcsOperationSites.OPERATION_ID == GlobalVariables.LinkPalletOperation)
                        {
                            propertyprovider.pallet = new Pallet();
                        }
                        returnVM.Result = propertyprovider;
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
        /// 产生质检
        /// </summary>
        /// <param name="propertyprovider"></param>
        /// <returns></returns>
        [HttpPost]
        [Authorize]
        public async Task<ApiBaseReturn<Propertyprovider>> SetQcDocFull(Propertyprovider propertyprovider)
        {
            ApiBaseReturn<Propertyprovider> returnVM = new ApiBaseReturn<Propertyprovider>();
            if (!ErrorInfo.Status)
            {
                try
                {
                    #region 参数验证
                    if (propertyprovider.sfcsOperationSites == null || propertyprovider.sfcsOperationLines.IsNullOrWhiteSpace() && !ErrorInfo.Status)
                    {
                        ErrorInfo.Set(_localizerA["SITEID_ERROR"], MethodBase.GetCurrentMethod(), EnumErrorType.Warning);
                    }
                    if (propertyprovider.spotCheck == null || propertyprovider.spotCheck.qcDocNo == null)
                    {
                        ErrorInfo.Set(_localizerA["QCDOCNO_ERROR"], MethodBase.GetCurrentMethod(), EnumErrorType.Warning);
                    }
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
                                //点击产生质检只是修改状态，清空页面的质检数量
                                Propertyprovider newPropertyprovider = new Propertyprovider();
                                newPropertyprovider.sfcsOperationSites = propertyprovider.sfcsOperationSites;
                                newPropertyprovider.sfcsOperationLines = propertyprovider.sfcsOperationLines;
                                newPropertyprovider.defects = propertyprovider.defects;
                                newPropertyprovider.sys_Manager = propertyprovider.sys_Manager;
                                newPropertyprovider.product = new Product();
                                newPropertyprovider.spotCheck = propertyprovider.spotCheck;
                                newPropertyprovider.data = propertyprovider.data;
                                //1、校验SN
                                bool snCheck = await VerifySnData(newPropertyprovider, tran);
                                if (snCheck && newPropertyprovider.spotCheck != null && !String.IsNullOrEmpty(newPropertyprovider.spotCheck.qcDocNo))
                                {
                                    //点击“产生质检”按钮产生调接口实现 SEQUENCE+1 ,FULL_FLAG = 'Y'刷滿 
                                    String UpdateContain = @"UPDATE SFCS_CONTAINER_LIST SET FULL_FLAG = :FULL_FLAG WHERE CONTAINER_SN = :CONTAINER_SN";//, SEQUENCE = SEQUENCE+1
                                    int result = _repository.Execute(UpdateContain, new
                                    {
                                        FULL_FLAG = "Y",
                                        CONTAINER_SN = newPropertyprovider.spotCheck.qcDocNo
                                    }, tran);

                                    //20201223 1.更新过程检验报告为已审核

                                    MesSpotcheckHeader header = (await _repository.GetListByTableEX<MesSpotcheckHeader>("*", "MES_SPOTCHECK_HEADER", " AND BATCH_NO=:BATCH_NO", new { BATCH_NO = newPropertyprovider.spotCheck.qcDocNo })).FirstOrDefault();
                                    if (header == null)
                                    {
                                        ErrorInfo.Set(_localizer["BATCH_NO_INFO_NULL"], MethodBase.GetCurrentMethod(), EnumErrorType.Warning);
                                    }
                                    String updateSpotCheckSql = @"UPDATE MES_SPOTCHECK_HEADER SET STATUS = 3,RESULT = 0,AUDITOR =:AUDITOR,AUDIT_TIME = SYSDATE WHERE BATCH_NO = :BATCH_NO";
                                    result = _repository.Execute(updateSpotCheckSql, new
                                    {
                                        AUDITOR = propertyprovider.sys_Manager.USER_NAME,
                                        BATCH_NO = newPropertyprovider.spotCheck.qcDocNo
                                    }, tran);

                                    QcDocListModel qcDoc = new QcDocListModel();
                                    #region 获取U9检验方案相关数据
                                    try
                                    {
                                        SfcsContainerList sfcsContainerList = (await _repository.GetListByTableEX<SfcsContainerList>("*", "SFCS_CONTAINER_LIST", " AND CONTAINER_SN=:CONTAINER_SN", new { CONTAINER_SN = newPropertyprovider.spotCheck.qcDocNo })).FirstOrDefault();
                                        if (sfcsContainerList != null && !sfcsContainerList.PART_NO.IsNullOrEmpty())
                                        {
                                            string postUrl = _repository.QueryEx<string>("SELECT T.DESCRIPTION FROM SFCS_PARAMETERS T WHERE T.LOOKUP_TYPE = :LOOKUP_TYPE AND T.ENABLED = 'Y' AND T.LOOKUP_CODE = '1'", new { LOOKUP_TYPE = "QC_URL" }).FirstOrDefault();//获取完工检验url地址

                                            postUrl = postUrl + "?part_code=" + sfcsContainerList.PART_NO;//根据物料料号获取质检方案数据

                                            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(postUrl);
                                            request.Method = "POST";
                                            request.ContentType = "text/html;charset=UTF-8";
                                            request.ContentLength = 0;

                                            HttpWebResponse response = (HttpWebResponse)request.GetResponse();
                                            Stream myResponseStream = response.GetResponseStream();
                                            StreamReader myStreamReader = new StreamReader(myResponseStream, Encoding.UTF8);
                                            string retString = myStreamReader.ReadToEnd();
                                            myStreamReader.Close();
                                            myResponseStream.Close();
                                            Newtonsoft.Json.Linq.JObject jo = (Newtonsoft.Json.Linq.JObject)Newtonsoft.Json.JsonConvert.DeserializeObject(retString);
                                            if (jo["Code"].ToString() == "1")
                                            {
                                                qcDoc = Newtonsoft.Json.JsonConvert.DeserializeObject<QcDocListModel>(jo["Data"].ToString());
                                            }
                                            else
                                            {
                                                throw new Exception("GET_QC_CHECK_ITEM");

                                            }
                                        }
                                    }
                                    catch (Exception ex)
                                    {
                                        throw new Exception("GET_QC_CHECK_ITEM");
                                    }
                                    #endregion

                                    //2.产生终检报告

                                    #region 抽检报告主表添加数据

                                    String qcDocNo = null;//使用SFCS_PACKING_CARTON_SEQ
                                    String sequence = _sfcsRuncardRepository.QueryEx<String>("SELECT SFCS_PACKING_CARTON_SEQ.NEXTVAL FROM DUAL ")?.FirstOrDefault();
                                    if (String.IsNullOrEmpty(sequence)) { throw new Exception("SEQUENCE_ERROR"); }

                                    //將序列轉成36進制表示
                                    String resultStr = Core.Utilities.RadixConvertPublic.RadixConvert(sequence, ViewModels.GlobalVariables.DecRadix, ViewModels.GlobalVariables.Base36Redix);

                                    //六位表示
                                    String ReleasedSequence = resultStr.PadLeft(6, '0');
                                    String yymmdd = _sfcsRuncardRepository.GetYYMMDDEx();
                                    qcDocNo = "QC" + yymmdd + ReleasedSequence;//终检单号 完工检验

                                    string insertSpotcheckHeaderSql = @"INSERT INTO MES_SPOTCHECK_HEADER (BATCH_NO, LINE_ID, LINE_TYPE, WO_NO, ALL_QTY, CHECK_QTY, FAIL_QTY, SAMP_STANDART, SAMP_SIZE, STATUS, CHECKER, CONFIRM, AUDITOR, RESULT, CREATE_DATE, ORGANIZE_ID, WO_QTY, ORDER_NO, OUTER_CHECK_QTY, OUTER_FAIL_QTY, REMARK, WO_CLASS, QC_TYPE, PARENT_BATCH_NO, QCSCHEMAHEAD, QCSCHEMANAME, QCSCHEMAVERSION, OPERATION_SITE_ID) VALUES (:BATCH_NO, :LINE_ID, :LINE_TYPE, :WO_NO, :ALL_QTY, :CHECK_QTY, :FAIL_QTY, :SAMP_STANDART, :SAMP_SIZE, :STATUS, :CHECKER, null, null, null, SYSDATE, :ORGANIZE_ID, :WO_QTY, :ORDER_NO, :OUTER_CHECK_QTY, :OUTER_FAIL_QTY, null, null, :QC_TYPE, :PARENT_BATCH_NO, :QCSCHEMAHEAD, :QCSCHEMANAME, :QCSCHEMAVERSION, :OPERATION_SITE_ID)";
                                    _repository.Execute(insertSpotcheckHeaderSql, new
                                    {
                                        BATCH_NO = qcDocNo,
                                        LINE_ID = header.LINE_ID,
                                        LINE_TYPE = header.LINE_TYPE,
                                        WO_NO = header.WO_NO,
                                        ALL_QTY = header.ALL_QTY,
                                        CHECK_QTY = header.CHECK_QTY,
                                        FAIL_QTY = header.FAIL_QTY,
                                        SAMP_STANDART = header.SAMP_STANDART,
                                        SAMP_SIZE = header.SAMP_SIZE,
                                        STATUS = 0,
                                        CHECKER = propertyprovider.sys_Manager.USER_NAME,
                                        ORGANIZE_ID = header.ORGANIZE_ID,
                                        WO_QTY = header.WO_QTY,
                                        ORDER_NO = header.ORDER_NO,
                                        OUTER_CHECK_QTY = header.OUTER_CHECK_QTY,
                                        OUTER_FAIL_QTY = header.OUTER_FAIL_QTY,
                                        QC_TYPE = 1,
                                        PARENT_BATCH_NO = header.BATCH_NO,
                                        QCSCHEMAHEAD = qcDoc.QCSchemaHead,
                                        QCSCHEMANAME = qcDoc.QCSchemaName,
                                        QCSCHEMAVERSION = qcDoc.QCSchemaVersion,
                                        OPERATION_SITE_ID = header.OPERATION_SITE_ID
                                    }, tran);

                                    #endregion

                                    #region 抽检报告子表添加数据
                                    if (qcDoc.DetailsData != null)
                                    {
                                        String insertIteamsSql = @"INSERT INTO MES_SPOTCHECK_ITEAMS 
(ID, BATCH_NO, STEPID, ORDER_NO, ITEM, SUB_ORDER_NO, STANDARD, GUIDELINEVALUE1, INSPECT_METHOD, QCLEVEL, AQL, GUIDERANGER, CHECK_QTY, PASS, FAIL, RESULT) 
VALUES 
(:ID, :BATCH_NO, :STEPID, :ORDER_NO, :ITEM, :SUB_ORDER_NO, :STANDARD, :GUIDELINEVALUE1, :INSPECT_METHOD, :QCLEVEL, :AQL, '', :CHECK_QTY, 0, 0, 0)";
                                        foreach (var item in qcDoc.DetailsData)
                                        {
                                            decimal iteamsId = _sfcsRuncardRepository.QueryEx<decimal>("SELECT MES_SPOTCHECK_ITEAMS_SEQ.NEXTVAL MY_SEQ FROM DUAL").FirstOrDefault();

                                            _repository.Execute(insertIteamsSql, new
                                            {
                                                ID = iteamsId,
                                                BATCH_NO = qcDocNo,
                                                STEPID = item.STEPID,
                                                ORDER_NO = item.ORDER_NO,
                                                ITEM = item.ITEM,
                                                SUB_ORDER_NO = item.SUB_ORDER_NO,
                                                STANDARD = item.STANDARD,
                                                GUIDELINEVALUE1 = item.GuidelineValue1,
                                                INSPECT_METHOD = item.INSPECT_METHOD,
                                                QCLEVEL = item.QCLevel,
                                                AQL = item.AQL,
                                                CHECK_QTY = header.ALL_QTY
                                            }, tran);
                                        }
                                    }
                                    #endregion

                                }
                                else
                                {
                                    throw new Exception("SN_ERROR");
                                }
                                newPropertyprovider.data = string.Empty;
                                tran.Commit();
                            }
                            catch (Exception ex)
                            {
                                tran.Rollback();
                                propertyprovider.result = 2;
                                propertyprovider.msg = ex.Message;
                                ErrorInfo.Set(_localizerA[ex.Message], MethodBase.GetCurrentMethod(), EnumErrorType.Error);
                            }
                            finally
                            {
                                returnVM.Result = propertyprovider;
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
        /// 置满接口
        /// </summary>
        /// <param name="propertyprovider"></param>
        /// <returns></returns>
        [HttpPost]
        [Authorize]
        public async Task<ApiBaseReturn<Propertyprovider>> SetCatonFull(Propertyprovider propertyprovider)
        {
            ApiBaseReturn<Propertyprovider> returnVM = new ApiBaseReturn<Propertyprovider>();
            if (!ErrorInfo.Status)
            {
                try
                {
                    #region 设置返回值
                    if (!ErrorInfo.Status)
                    {
                        //1、校验站点信息
                        if (propertyprovider.sfcsOperationSites == null || propertyprovider.sfcsOperationLines == null)
                        {
                            throw new Exception("请选择站点信息!");
                        }
                        if (propertyprovider.carton == null || propertyprovider.carton.Carton_NO == null)
                        {
                            throw new Exception("箱号不存在无法打印包装条码!");
                        }
                        else
                        {
                            IDbConnection dbConnection = await _sfcsRuncardRepository.GetConnection();
                            ConnectionFactory.OpenConnection(dbConnection);
                            using (var tran = dbConnection.BeginTransaction())
                            {
                                //执行校验的JOb（Finally RUN JOB）
                                try
                                {
                                    Propertyprovider newPropertyprovider = new Propertyprovider();
                                    newPropertyprovider.sfcsOperationSites = propertyprovider.sfcsOperationSites;
                                    newPropertyprovider.sfcsOperationLines = propertyprovider.sfcsOperationLines;
                                    newPropertyprovider.defects = propertyprovider.defects;
                                    newPropertyprovider.sys_Manager = propertyprovider.sys_Manager;
                                    newPropertyprovider.product = new Product();
                                    newPropertyprovider.carton = propertyprovider.carton;
                                    newPropertyprovider.data = propertyprovider.data;
                                    newPropertyprovider.printer = propertyprovider.printer;
                                    //1、校验SN
                                    bool snCheck = await VerifySnData(newPropertyprovider, tran);
                                    if (snCheck)
                                    {
                                        String UpdateContain = @"update SFCS_CONTAINER_LIST SET FULL_FLAG = :FULL_FLAG WHERE CONTAINER_SN = :CONTAINER_SN";
                                       _sfcsRuncardRepository.Execute(UpdateContain, new
                                        {
                                            FULL_FLAG = "Y",
                                            CONTAINER_SN = newPropertyprovider.carton.Carton_NO
                                        }, tran);

                                        Job.FinallyJobStorage.AutoPakage autoPakage = new Job.FinallyJobStorage.AutoPakage();
                                        KeyValuePair<bool, string> result = await autoPakage.GetTask(newPropertyprovider, _sfcsRuncardRepository, tran);
                                        if (!result.Key)
                                        {
                                            throw new Exception(result.Value);
                                        }

                                    }
                                    else
                                    {
                                        throw new Exception(String.Format("输入的条码:{0}无效", propertyprovider.data));
                                    }
                                    newPropertyprovider.data = string.Empty;
                                    propertyprovider.printer = newPropertyprovider.printer;
                                    tran.Commit();
                                }
                                catch (Exception ex)
                                {
                                    tran.Rollback();
                                    propertyprovider.result = 2;
                                    propertyprovider.msg = ex.Message;
                                    ErrorInfo.Set(ex.Message, MethodBase.GetCurrentMethod(), EnumErrorType.Error);
                                }
                                finally
                                {
                                    returnVM.Result = propertyprovider;
                                }
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
        /// 调用箱号置满包装
        /// </summary>
        /// <param name="propertyprovider"></param>
        /// <returns></returns>
        [HttpPost]
        [Authorize]
        public async Task<ApiBaseReturn<Propertyprovider>> SetCatonFullByCaton(Propertyprovider propertyprovider)
        {
            ApiBaseReturn<Propertyprovider> returnVM = new ApiBaseReturn<Propertyprovider>();
            if (!ErrorInfo.Status)
            {
                try
                {
                    #region 设置返回值
                    if (!ErrorInfo.Status)
                    {
                        //1、校验站点信息
                        if (propertyprovider.sfcsOperationSites == null || propertyprovider.sfcsOperationLines == null)
                        {
                            throw new Exception("请选择站点信息!");
                        }
                        if (propertyprovider.carton == null || propertyprovider.carton.Carton_NO == null)
                        {
                            throw new Exception("箱号不存在无法打印包装条码!");
                        }
                        else
                        {
                            IDbConnection dbConnection = await _sfcsRuncardRepository.GetConnection();
                            ConnectionFactory.OpenConnection(dbConnection);
                            using (var tran = dbConnection.BeginTransaction())
                            {
                                //执行校验的JOb（Finally RUN JOB）
                                try
                                {
                                    String UpdateContain = @"update SFCS_CONTAINER_LIST SET FULL_FLAG = :FULL_FLAG WHERE CONTAINER_SN = :CONTAINER_SN";
                                    _sfcsRuncardRepository.Execute(UpdateContain, new
                                    {
                                        FULL_FLAG = "Y",
                                        CONTAINER_SN = propertyprovider.carton.Carton_NO
                                    }, tran);
                                    Job.FinallyJobStorage.AutoPakage autoPakage = new Job.FinallyJobStorage.AutoPakage();
                                    KeyValuePair<bool, string> result = await autoPakage.GetTask(propertyprovider, _sfcsRuncardRepository, tran);
                                    if (!result.Key)
                                    {
                                        throw new Exception(result.Value);
                                    }
                                    tran.Commit();
                                }
                                catch (Exception ex)
                                {
                                    tran.Rollback();
                                    propertyprovider.result = 2;
                                    propertyprovider.msg = ex.Message;
                                    ErrorInfo.Set(ex.Message, MethodBase.GetCurrentMethod(), EnumErrorType.Error);
                                }
                                finally
                                {
                                    returnVM.Result = propertyprovider;
                                }
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

        #region 中转码
        /// <summary>
        /// 保存采集中转码数据的站点
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [Authorize]
        public async Task<ApiBaseReturn<CollectMiddleCodeDataRequestModel>> SaveSiteByMiddleCode(SaveSiteModelcs siteModel)
        {
            Decimal siteId = siteModel.SiteId;
            String userName = siteModel.UserName;
            ApiBaseReturn<CollectMiddleCodeDataRequestModel> returnVM = new ApiBaseReturn<CollectMiddleCodeDataRequestModel>();
            if (!ErrorInfo.Status)
            {
                try
                {
                    #region 设置返回值
                    Sys_Manager sys_Manager = _repository.QueryEx<Sys_Manager>("SELECT * FROM SYS_MANAGER WHERE USER_NAME = :USER_NAME",
                            new
                            {
                                USER_NAME = userName
                            }).FirstOrDefault();
                    if (sys_Manager == null)
                    {
                        throw new Exception("USER_NOT_EIXST");
                    }
                    sys_Manager.PASSWORD = "";
                    sys_Manager.PASSWORD_SALT = "";
                    SfcsOperationSites site = await _sfcsOperationSitesRepository.GetAsync(siteId);
                    if (site == null)
                    {
                        throw new Exception("SITES_NOT_EIXST");
                    }
                    SfcsOperationLines line = await _sfcsOperationLinesRepository.GetAsync((Decimal)site.OPERATION_LINE_ID);
                    if (line == null)
                    {
                        throw new Exception("LINE_NOT_EIXST");
                    }
                    CollectMiddleCodeDataRequestModel model = new CollectMiddleCodeDataRequestModel();
                    model.OPERATIONLINES = line;
                    model.OPERATIONSITES = site;
                    model.MANAGER = sys_Manager;
                    returnVM.Result = model;
                    #endregion
                }
                catch (Exception ex)
                {
                    ErrorInfo.Set(_localizerMC[ex.Message], MethodBase.GetCurrentMethod(), EnumErrorType.Error);
                }
            }

            #region 如果出现错误，则写错误日志并返回错误内容

            WriteLog(ref returnVM);

            #endregion

            return returnVM;
        }

        /// <summary>
        /// 采集中转码数据并生成中转码
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        [Authorize]
        public async Task<ApiBaseReturn<CollectMiddleCodeDataRequestModel>> CollectMiddleCodeData([FromBody] CollectMiddleCodeDataRequestModel model)
        {
            ApiBaseReturn<CollectMiddleCodeDataRequestModel> returnVM = new ApiBaseReturn<CollectMiddleCodeDataRequestModel>();
            try
            {
                #region 检查参数

                int partQty = 0;//已收集的零件数量
                Boolean codeCheck = false;//校验是否部件码

                if (model.DATA.IsNullOrEmpty() && !ErrorInfo.Status)
                {
                    ErrorInfo.Set(_localizerMC["DATA_NOT_NULL"], MethodBase.GetCurrentMethod(), EnumErrorType.Warning);
                }
                else
                {
                    //1.校验部件码是否正确 需要收集的部件码的数量根据用户输入的来确定需要收集几个部件码
                    //部件条码目前有三种: 第一种是半成品在SFCS_RUNCARD 表中的SN :第二种是仓库条码IMS_REEL的CODE: 第三种是周转码MES_BATCH_PRING的CARTON_NO
                    if (model.IMSREEL == null && codeCheck == false)
                    {
                        //仓库条码
                        model.IMSREEL = (await _repository.GetListByTableEX<ImsReel>("*", "IMS_REEL", " AND CODE=:CODE", new { CODE = model.DATA })).FirstOrDefault();
                        codeCheck = model.IMSREEL != null ? true : false;
                    }
                    else if (model.IMSREEL != null && model.IMSREEL.CODE == model.DATA)
                    {
                        ErrorInfo.Set(_localizerMC["DATA_REPEAT"], MethodBase.GetCurrentMethod(), EnumErrorType.Warning);
                    }

                    if (model.IMSREEL != null) { partQty++; }

                    if (model.BATCHPRING == null && codeCheck == false)
                    {
                        //周转码
                        model.BATCHPRING = (await _repository.GetListByTableEX<MesBatchPring>("*", "MES_BATCH_PRING", " AND CARTON_NO = :CARTON_NO", new { CARTON_NO = model.DATA })).FirstOrDefault();
                        codeCheck = model.BATCHPRING != null ? true : false;
                    }
                    else if (model.BATCHPRING != null && model.BATCHPRING.CARTON_NO == model.DATA)
                    {
                        ErrorInfo.Set(_localizerMC["DATA_REPEAT"], MethodBase.GetCurrentMethod(), EnumErrorType.Warning);
                    }

                    if (model.BATCHPRING != null) { partQty++; }

                    if (model.RUNCARD == null && codeCheck == false)
                    {
                        //流水号
                        model.RUNCARD = (await _repository.GetListByTableEX<SfcsRuncard>("*", "SFCS_RUNCARD", " AND SN = :SN", new { SN = model.DATA })).FirstOrDefault();
                        codeCheck = model.RUNCARD != null ? true : false;
                    }
                    else if (model.RUNCARD != null && model.RUNCARD.SN == model.DATA)
                    {
                        ErrorInfo.Set(_localizerMC["DATA_REPEAT"], MethodBase.GetCurrentMethod(), EnumErrorType.Warning);
                    }

                    if (model.RUNCARD != null) { partQty++; }

                    //2. 先判断是否打印中转码  如果打印 1.codeCheck=false提示CODE_NOT_EIXST  2. codeCheck=true继续走流程（下一步校验是否刷满部件）
                    //                         不打印   2.codeCheck=false校验是否刷满部件  刷满 ->校验DATA是不是中转码   DATA是中转码 继续走流程（下一步生成SN以及中转码相关明细数据）；
                    //                                                                     未满 ->提示CODE_NOT_EIXST     DATA不是中转码 提示CODE_NOT_EIXST
                    if (!codeCheck && !ErrorInfo.Status)
                    {
                        if (model.PRINTCODE)
                        {
                            //不是部件码并且是自动打印中转码提示CODE_NOT_EIXST
                            ErrorInfo.Set(_localizerMC["CODE_NOT_EIXST"], MethodBase.GetCurrentMethod(), EnumErrorType.Warning);
                        }
                        else
                        {
                            if (model.COLLECT_QTY == partQty)
                            {
                                //STATUS = '0'当前中转码是待用状态
                                model.MIDDLECODE = (await _repository.GetListByTableEX<MesMiddleCode>("*", "MES_MIDDLE_CODE", " AND CODE=:CODE ", new { CODE = model.DATA })).FirstOrDefault();
                                if (model.MIDDLECODE == null) { ErrorInfo.Set(_localizerMC["CODE_NOT_EIXST"], MethodBase.GetCurrentMethod(), EnumErrorType.Warning); }
                                else if (model.MIDDLECODE != null && model.MIDDLECODE.STATUS == "1")
                                {
                                    model.MIDDLECODE = null;
                                    ErrorInfo.Set(_localizerMC["MIDDLE_CODE_STATUS_ERROR"], MethodBase.GetCurrentMethod(), EnumErrorType.Warning);
                                }
                            }
                            else { ErrorInfo.Set(_localizerMC["CODE_NOT_EIXST"], MethodBase.GetCurrentMethod(), EnumErrorType.Warning); }
                            //else if (model.COLLECT_QTY > partQty) { ErrorInfo.Set(_localizerMC["COLLECT_QTY_ERROR"], MethodBase.GetCurrentMethod(), EnumErrorType.Warning); }
                            //else if (model.COLLECT_QTY < partQty) { ErrorInfo.Set(_localizerMC["CODE_NOT_EIXST"], MethodBase.GetCurrentMethod(), EnumErrorType.Warning); }
                            //else { ErrorInfo.Set(_localizerMC["COLLECT_QTY_ERROR"], MethodBase.GetCurrentMethod(), EnumErrorType.Warning); }
                        }
                    }
                    model.COLLECTION_QTY = partQty;
                }
                if ((model.COLLECT_QTY < 0 || model.COLLECT_QTY > 3) && !ErrorInfo.Status)
                {
                    ErrorInfo.Set(_localizerMC["COLLECT_QTY_NOT_NULL"], MethodBase.GetCurrentMethod(), EnumErrorType.Warning);
                }
                if ((model.OPERATIONSITES == null || model.OPERATIONLINES == null || model.MANAGER == null) && !ErrorInfo.Status)
                {
                    ErrorInfo.Set(_localizerMC["SITES_NOT_NULL"], MethodBase.GetCurrentMethod(), EnumErrorType.Warning);
                }
                if (model.MANAGER == null && !ErrorInfo.Status)
                {
                    ErrorInfo.Set(_localizerMC["USER_NOT_EIXST"], MethodBase.GetCurrentMethod(), EnumErrorType.Warning);
                }
                if (model.WO_NO.IsNullOrEmpty() && !ErrorInfo.Status)
                {
                    ErrorInfo.Set(_localizerMC["WO_NOT_NULL"], MethodBase.GetCurrentMethod(), EnumErrorType.Warning);
                }
                else
                {
                    if (model.WO == null && !ErrorInfo.Status)
                    {
                        model.WO = (await _repository.GetListByTableEX<SfcsWo>("*", "SFCS_WO", " AND WO_NO=:WO_NO", new { WO_NO = model.WO_NO })).FirstOrDefault();
                    }
                    if (model.WO == null || model.WO.WO_NO != model.WO_NO && !ErrorInfo.Status)
                    {
                        ErrorInfo.Set(_localizerMC["WO_NOT_NULL"], MethodBase.GetCurrentMethod(), EnumErrorType.Warning);
                    }
                }
                if (model.PN == null && !ErrorInfo.Status)
                {
                    model.PN = (await _repository.GetListByTableEX<SfcsPn>("SP.*", "SFCS_PN SP, SFCS_WO SW", " AND SP.PART_NO = SW.PART_NO AND SW.ID = :ID", new { ID = model.WO.ID })).FirstOrDefault();
                    if (model.PN == null) { ErrorInfo.Set(_localizerMC["WO_NOT_NULL"], MethodBase.GetCurrentMethod(), EnumErrorType.Warning); }
                }
                model.RESULT = ErrorInfo.Status == true ? 1 : 0;
                model.MESSAGE = ErrorInfo.Status == true ? ErrorInfo.Message : model.MESSAGE;
                #endregion

                #region 保存并返回

                if (!ErrorInfo.Status)
                {
                    //校验是否采集完成
                    if (model.COLLECT_QTY == model.COLLECTION_QTY)
                    {
                        IDbConnection dbConnection = await _sfcsRuncardRepository.GetConnection();
                        ConnectionFactory.OpenConnection(dbConnection);
                        using (var tran = dbConnection.BeginTransaction())
                        {
                            try
                            {
                                model = await SaveMiddleCodeData(model, tran);
                                model.DATA = "";
                                model.SN = "";
                                tran.Commit();
                            }
                            catch (Exception ex)
                            {
                                tran.Rollback();
                                ErrorInfo.Set(_localizerMC[ex.Message], MethodBase.GetCurrentMethod(), EnumErrorType.Error);
                                model.DATA = "";
                                model.SN = "";
                                model.RESULT = 2;
                                model.MESSAGE = ErrorInfo.Message;
                            }
                            finally
                            {
                                if (dbConnection.State != System.Data.ConnectionState.Closed)
                                {
                                    dbConnection.Close();
                                }
                            }
                        }
                        model.PARTLIST = GetMiddleCodeData(model);
                    }
                    else
                    {
                        model.DATA = "";
                        model.SN = "";
                        model.RESULT = 0;
                        model.MESSAGE = "采集部件条码数据未完成，请继续完成采集！";
                    }
                }

                #endregion
            }
            catch (Exception ex)
            {
                ErrorInfo.Set(_localizerMC[ex.Message], MethodBase.GetCurrentMethod(), EnumErrorType.Error);
                model.DATA = "";
                model.SN = "";
                model.RESULT = 2;
                model.MESSAGE = ErrorInfo.Message;
                model.WO = null;
            }
            finally
            {
                returnVM.Result = model;
            }

            #region 如果出现错误，则写错误日志并返回错误内容

            WriteLog(ref returnVM);

            #endregion

            return returnVM;
        }

        /// <summary>
        /// 中转码过站处理
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        [Authorize]
        public async Task<ApiBaseReturn<CollectMiddleCodeDataRequestModel>> DoProcessMiddleCode([FromBody] CollectMiddleCodeDataRequestModel model)
        {
            ApiBaseReturn<CollectMiddleCodeDataRequestModel> returnVM = new ApiBaseReturn<CollectMiddleCodeDataRequestModel>();
            try
            {
                #region 检查参数

                if (model.DATA.IsNullOrEmpty() && !ErrorInfo.Status)
                {
                    ErrorInfo.Set(_localizerMC["DATA_NOT_NULL"], MethodBase.GetCurrentMethod(), EnumErrorType.Warning);
                }
                else
                {
                    //STATUS = '0'当前中转码是待用状态
                    model.MIDDLECODE = (await _repository.GetListByTableEX<MesMiddleCode>("*", "MES_MIDDLE_CODE", " AND CODE=:CODE ", new { CODE = model.DATA })).FirstOrDefault();
                    if (model.MIDDLECODE == null) { ErrorInfo.Set(_localizerMC["CODE_NOT_EIXST"], MethodBase.GetCurrentMethod(), EnumErrorType.Warning); }
                    else if (model.MIDDLECODE != null && model.MIDDLECODE.STATUS == "1")
                    {
                        model.MIDDLECODE = null;
                        ErrorInfo.Set(_localizerMC["MIDDLE_CODE_STATUS_ERROR"], MethodBase.GetCurrentMethod(), EnumErrorType.Warning);
                    }
                }
                if ((model.OPERATIONSITES == null || model.OPERATIONLINES == null || model.MANAGER == null) && !ErrorInfo.Status)
                {
                    ErrorInfo.Set(_localizerMC["SITES_NOT_NULL"], MethodBase.GetCurrentMethod(), EnumErrorType.Warning);
                }
                if (model.MANAGER == null && !ErrorInfo.Status)
                {
                    ErrorInfo.Set(_localizerMC["USER_NOT_EIXST"], MethodBase.GetCurrentMethod(), EnumErrorType.Warning);
                }
                if (model.WO_NO.IsNullOrEmpty() && !ErrorInfo.Status)
                {
                    ErrorInfo.Set(_localizerMC["WO_NOT_NULL"], MethodBase.GetCurrentMethod(), EnumErrorType.Warning);
                }
                else
                {
                    if (model.WO == null && !ErrorInfo.Status)
                    {
                        model.WO = (await _repository.GetListByTableEX<SfcsWo>("*", "SFCS_WO", " AND WO_NO=:WO_NO", new { WO_NO = model.WO_NO })).FirstOrDefault();
                    }
                    if (model.WO == null || model.WO.WO_NO != model.WO_NO && !ErrorInfo.Status)
                    {
                        ErrorInfo.Set(_localizerMC["WO_NOT_NULL"], MethodBase.GetCurrentMethod(), EnumErrorType.Warning);
                    }
                }
                if (model.PN == null && !ErrorInfo.Status)
                {
                    model.PN = (await _repository.GetListByTableEX<SfcsPn>("SP.*", "SFCS_PN SP, SFCS_WO SW", " AND SP.PART_NO = SW.PART_NO AND SW.ID = :ID", new { ID = model.WO.ID })).FirstOrDefault();
                    if (model.PN == null) { ErrorInfo.Set(_localizerMC["WO_NOT_NULL"], MethodBase.GetCurrentMethod(), EnumErrorType.Warning); }
                }
                model.RESULT = ErrorInfo.Status == true ? 1 : 0;
                model.MESSAGE = ErrorInfo.Status == true ? ErrorInfo.Message : model.MESSAGE;
                #endregion

                #region 保存并返回

                if (!ErrorInfo.Status)
                {
                    IDbConnection dbConnection = await _sfcsRuncardRepository.GetConnection();
                    ConnectionFactory.OpenConnection(dbConnection);
                    using (var tran = dbConnection.BeginTransaction())
                    {
                        try
                        {
                            model.SN = await GetSNByWoId(model.WO.ID);
                            model.IMSREEL = null; model.BATCHPRING = null; model.RUNCARD = null; model.COLLECT_QTY = 0;
                            bool result = this.SaveMiddleCodeList(model, tran);
                            if (result)
                            {
                                Propertyprovider propertyprovider = new Propertyprovider();
                                propertyprovider.data = model.SN;//根据流水号范围表获取到的SN
                                propertyprovider.sys_Manager = model.MANAGER;
                                propertyprovider.sfcsOperationLines = model.OPERATIONLINES;
                                propertyprovider.sfcsOperationSites = model.OPERATIONSITES;
                                propertyprovider.woNo = model.WO_NO;
                                propertyprovider = await DoProcessAsync(propertyprovider, tran);//过站处理
                                model.RESULT = (int)propertyprovider.result;
                                model.MESSAGE = propertyprovider.msg;
                                if (model.RESULT == 0 && model.PRINTCODE)
                                {
                                    //生成打印信息并解绑中转码信息
                                    model.PRINTTASKID = UnbindMiddleCodeData(model, true, tran);
                                }
                                else if (model.RESULT == 2)
                                {
                                    throw new Exception(model.MESSAGE);
                                }
                            }
                            else
                            {
                                throw new Exception("SAVE_MIDDLE_DATA_ERROR");
                            }
                            model.DATA = "";
                            model.SN = "";
                            tran.Commit();
                        }
                        catch (Exception ex)
                        {
                            tran.Rollback();
                            ErrorInfo.Set(_localizerMC[ex.Message], MethodBase.GetCurrentMethod(), EnumErrorType.Error);
                            model.DATA = "";
                            model.SN = "";
                            model.RESULT = 2;
                            model.MESSAGE = ErrorInfo.Message;
                        }
                    }

                    model.PARTLIST = GetMiddleCodeData(model);
                }

                #endregion
            }
            catch (Exception ex)
            {
                ErrorInfo.Set(_localizerMC[ex.Message], MethodBase.GetCurrentMethod(), EnumErrorType.Error);
                model.DATA = "";
                model.SN = "";
                model.RESULT = 2;
                model.MESSAGE = ErrorInfo.Message;
                model.WO = null;
            }
            finally
            {
                returnVM.Result = model;
            }

            #region 如果出现错误，则写错误日志并返回错误内容

            WriteLog(ref returnVM);

            #endregion

            return returnVM;
        }

        #endregion

        #region 完工检验单

        /// <summary>
        /// 审核完工检验单
        /// </summary>
        /// <param name="model">审核抽检模型</param>
        /// <returns></returns>
        [HttpPost]
        [Authorize]
        public async Task<ApiBaseReturn<bool>> AuditSpotCheck([FromBody] VerifySpotCheckRequestModel model)
        {
            MesSpotcheckHeader header = null;
            ApiBaseReturn<bool> returnVM = new ApiBaseReturn<bool>();
            if (!ErrorInfo.Status)
            {
                try
                {
                    #region 校验参数

                    if (model.USER_NAME.IsNullOrWhiteSpace() && !ErrorInfo.Status)
                    {
                        ErrorInfo.Set(_localizer["USER_NAME_NOT_EMPTY"], MethodBase.GetCurrentMethod(), EnumErrorType.Warning);
                    }

                    if (model.BATCH_NO.IsNullOrWhiteSpace() && !ErrorInfo.Status)
                    {
                        ErrorInfo.Set(_localizer["BATCH_NO_NOT_EMPTY"], MethodBase.GetCurrentMethod(), EnumErrorType.Warning);
                    }

                    header = (await _repository.GetListByTableEX<MesSpotcheckHeader>("*", "MES_SPOTCHECK_HEADER", " AND BATCH_NO=:BATCH_NO", new { BATCH_NO = model.BATCH_NO })).FirstOrDefault();
                    if (header == null)
                    {
                        ErrorInfo.Set(_localizer["BATCH_NO_INFO_NULL"], MethodBase.GetCurrentMethod(), EnumErrorType.Warning);
                    }
                    else if (header.QC_TYPE != 1 && header.QC_TYPE != 2)//完工检验和终检检验
                    {
                        //20201223 修改审核只审核完工检验和终检检验的数据
                        ErrorInfo.Set(_localizerA["QC_TYPE0_NOT"], MethodBase.GetCurrentMethod(), EnumErrorType.Warning);
                    }
                    #endregion

                    #region 审核并返回

                    if (!ErrorInfo.Status)
                    {
                        int resdata = 0;
                        if (model.STATUS == 0)
                        {
                            resdata = await CompleteAuditSpotCheck(model);//取消审核只改审核状态 
                        }
                        else if (model.STATUS == 3)
                        {
                            //20201225 终检进行抽检，抽检完进行提交（原有的审核按钮），只要出现不良品，直接返工（过程检验单）质检单中所有的SN
                            if (model.RESULT == 0)//审核合格 合格的对抽检的进行过站处理（过站传入的参数（站点，线别，用户））（在检验里面增加选择工作站点）
                            {
                                int fail_qty = (await _repository.GetListByTableEX<int>("COUNT(0) FAIL_QTY", "MES_SPOTCHECK_FAIL_DETAIL", " AND SPOTCHECK_DETAIL_ID IN (SELECT ID FROM MES_SPOTCHECK_DETAIL WHERE BATCH_NO = :BATCH_NO )", new { BATCH_NO = model.BATCH_NO })).FirstOrDefault();
                                if (fail_qty < 0)
                                {
                                    //返工当前检验单的父检验单中全部的SN
                                    resdata = await ReworkSNByBatchNo(header.PARENT_BATCH_NO, model.USER_NAME);
                                }
                                else
                                {
                                    //审核合格并过站处理
                                    resdata = await CompleteAuditSpotCheck(model);
                                }
                            }
                            else if (model.RESULT == 2)//返工 (完工检验单)检验单中送检的SN
                            {
                                //返工当前检验单中全部的SN
                                resdata = await ReworkSNByBatchNo(model.BATCH_NO, model.USER_NAME);
                            }
                            else
                            {

                                ErrorInfo.Set(_localizer["RESULT_TYPE_ERROR"], MethodBase.GetCurrentMethod(), EnumErrorType.Warning);
                            }
                        }
                        else
                        {
                            ErrorInfo.Set(_localizerA["AUDIT_STATUS_ERROR"], MethodBase.GetCurrentMethod(), EnumErrorType.Warning);
                        }

                        if (resdata == 0 && !ErrorInfo.Status)
                        {
                            returnVM.Result = false;
                            ErrorInfo.Set(_localizerA["AUDIT_FAIL"], MethodBase.GetCurrentMethod(), EnumErrorType.Warning);
                        }
                        else
                        {
                            returnVM.Result = true;
                        }
                    }

                    #endregion
                }
                catch (Exception ex)
                {
                    returnVM.Result = false;
                    ErrorInfo.Set(_localizerA[ex.Message], MethodBase.GetCurrentMethod(), EnumErrorType.Error);
                }
            }

            #region 如果出现错误，则写错误日志并返回错误内容

            WriteLog(ref returnVM);

            #endregion

            return returnVM;
        }

        /// <summary>
        /// 审核合格完工检验单 并过站处理
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        private async Task<int> CompleteAuditSpotCheck(VerifySpotCheckRequestModel model)
        {
            int result = 0;

            #region 保存并返回

            if (!ErrorInfo.Status)
            {
                IDbConnection dbConnection = await _sfcsRuncardRepository.GetConnection();
                ConnectionFactory.OpenConnection(dbConnection);
                using (var tran = dbConnection.BeginTransaction())
                {
                    try
                    {

                        int executeResult = _sfcsRuncardRepository.Execute("UPDATE MES_SPOTCHECK_HEADER SET STATUS = :STATUS,RESULT = :RESULT,AUDITOR =:AUDITOR,AUDIT_TIME = SYSDATE, REMARK = REMARK + :REMARK WHERE BATCH_NO = :BATCH_NO", new { STATUS = model.STATUS, RESULT = model.RESULT, AUDITOR = model.USER_NAME, REMARK = "," + model.REMARK, BATCH_NO = model.BATCH_NO }, tran);
                        if (executeResult < 1)
                        {
                            throw new Exception("AUDIT_FAIL");
                        }

                        if (model.STATUS == 3)
                        {
                            List<MesSpotcheckDetailListModel> detailList = await _repository.GetListByTableEX<MesSpotcheckDetailListModel>("*", "MES_SPOTCHECK_DETAIL", " AND BATCH_NO = :BATCH_NO ", new { BATCH_NO = model.BATCH_NO });
                            foreach (var item in detailList)
                            {
                                if (item.OPERATION_SITE_ID == null || item.OPERATION_SITE_ID < 1) { throw new Exception(string.Format(_localizerA["SN_SITE_INFO_NULL"], item.SN)); }

                                SaveSiteModelcs saveSite = new SaveSiteModelcs();
                                saveSite.SiteId = (decimal)item.OPERATION_SITE_ID;
                                saveSite.UserName = model.USER_NAME;

                                Propertyprovider propertyprovider = (await SaveSite(saveSite)).Result;
                                if (propertyprovider == null || propertyprovider.sys_Manager == null || propertyprovider.sfcsOperationSites == null || propertyprovider.sfcsOperationLines == null)
                                {
                                    throw new Exception(string.Format(_localizerA["SN_SITE_INFO_ERROR"], item.SN));
                                }

                                propertyprovider.data = item.SN;
                                propertyprovider = await DoProcessAsync(propertyprovider, tran);//过站处理
                                if (propertyprovider.result == 0)
                                {
                                    result = 1;
                                }
                                else
                                {
                                    throw new Exception(item.SN + ":" + propertyprovider.msg);
                                }
                            }
                        }

                        tran.Commit();
                    }
                    catch (Exception ex)
                    {
                        tran.Rollback();
                        result = 0;
                        throw ex;
                    }
                }

            }

            #endregion

            return result;
        }

        /// <summary>
        /// 根据完工检验单号返工SN
        /// </summary>
        /// <param name="batch_no"></param>
        /// <param name="user_name"></param>
        /// <returns></returns>
        private async Task<int> ReworkSNByBatchNo(String batch_no, String user_name)
        {
            int result = 0;

            #region 校验参数
            if (String.IsNullOrEmpty(batch_no))
            {
                throw new Exception("GET_REWORK_DATA_ERROR");
            }
            else
            {
                SfcsReworkModel reworkModel = new SfcsReworkModel();//返工实体
                reworkModel.SaveRecords = new List<SfcsReworkAddOrModifyModel>();
                reworkModel.SNLIST = await _repository.GetListByTableEX<String>("SN", "MES_SPOTCHECK_DETAIL", " AND BATCH_NO = :BATCH_NO ", new { BATCH_NO = batch_no });
                if (reworkModel.SNLIST != null && reworkModel.SNLIST.Count() > 0)
                {
                    foreach (var sn in reworkModel.SNLIST)
                    {
                        SfcsReworkRequestModel reworkRequestModel = new SfcsReworkRequestModel();
                        reworkRequestModel.RETYPE = 0;
                        reworkRequestModel.SN = sn;
                        SfcsReworkListModel reworkData = await _repositoryRework.GetReworkDataBySN(reworkRequestModel);

                        SfcsReworkAddOrModifyModel model = new SfcsReworkAddOrModifyModel();
                        model.SN = sn;
                        model.RETYPE = 0;
                        model.ISDELRESOURE = true;
                        model.ISDELUID = true;
                        SfcsRouteConfig route = (await _repository.GetListByTableEX<SfcsRouteConfig>("*", "SFCS_ROUTE_CONFIG", " AND ROUTE_ID=:ROUTE_ID AND CURRENT_OPERATION_ID IN (SELECT OPERATION_ID FROM SFCS_OPERATION_SITES WHERE ID IN (SELECT CURRENT_SITE FROM SFCS_RUNCARD WHERE SN =:SN))", new { ROUTE_ID = reworkData.ORIGINALROUTEID, SN = sn })).FirstOrDefault();
                        model.CHOOSEINDEX = Convert.ToDecimal(route.ORDER_NO);
                        model.CHOOSEINDEXVALUE = route.CURRENT_OPERATION_ID;
                        model.WORKORDERID = reworkData.WORKORDERID;
                        model.PLANT_CODE = reworkData.PLANT_CODE;
                        model.CLASSIFICATION = reworkData.CLASSIFICATION;
                        model.ROUTE_ID = reworkData.ROUTE_ID;
                        model.ORIGINALROUTEID = reworkData.ORIGINALROUTEID;
                        model.ORDERNOLIST = reworkData.ORDERNOLIST;
                        model.ORIGINALORDERNOLIST = reworkData.ORIGINALORDERNOLIST;
                        model.REPAIRER = user_name;
                        model.RUNCARDFORMAT = reworkData.RUNCARDFORMAT;
                        reworkModel.SaveRecords.Add(model);
                    }

                    decimal resdata = await _repositoryRework.SaveDataByTrans(reworkModel);
                    if (resdata != -1)
                    {
                        result = 1;
                    }
                    else
                    {
                        result = 0;
                    }
                }
            }
            #endregion
            return result;
        }
        #endregion

        #region 自动包装

        /// <summary>
        /// 新增SN包装过站
        /// </summary>
        /// <param name="model">包装实体</param>
        /// <returns></returns>
        [HttpPost]
        [Authorize]
        public async Task<ApiBaseReturn<Propertyprovider>> AddNewToPacking([FromBody] Propertyprovider model)
        {
            ApiBaseReturn<Propertyprovider> returnVM = new ApiBaseReturn<Propertyprovider>();
            returnVM.Result = model;
            if (!ErrorInfo.Status)
            {
                try
                {
                    #region 参数检验

                    if (!ErrorInfo.Status && model.sfcsOperationLines.Id <= 0) ErrorInfo.Set(_localizerA["LINEID_NOT_NULL"], MethodBase.GetCurrentMethod(), EnumErrorType.Error);
                    if (!ErrorInfo.Status && model.sfcsOperationSites.ID <= 0) ErrorInfo.Set(_localizerA["SITE_NOT_NULL"], MethodBase.GetCurrentMethod(), EnumErrorType.Error);
                    if (!ErrorInfo.Status && model.data.IsNullOrEmpty()) ErrorInfo.Set(_localizerA["SN_NOT_NULL"], MethodBase.GetCurrentMethod(), EnumErrorType.Error);
                    if (!ErrorInfo.Status && model.carton.Carton_NO.IsNullOrEmpty()) ErrorInfo.Set(_localizerA["CARTON_SN_NOT_NULL"], MethodBase.GetCurrentMethod(), EnumErrorType.Error);

                    #endregion

                    #region 设置返回值
                    if (!ErrorInfo.Status)
                    {
                        var cartonMolde = (await _sfcsRuncardRepository.GetListByTableEX<SfcsContainerList>("*", "SFCS_CONTAINER_LIST", " AND CONTAINER_SN=:CONTAINER_SN AND FULL_FLAG='Y'", new { CONTAINER_SN = model.carton.Carton_NO }))?.FirstOrDefault();
                        if (cartonMolde == null)
                        {
                            var boolResult = (await PackingProcessAsync(model));
                            model.result = boolResult ? 0 : 2;
                        }
                        else
                        {
                            //箱子已经刷满，不能再刷了。
                            throw new Exception(_localizerA["CARTON_IS_NULL"]);
                        }
                    }

                    #endregion
                }
                catch (Exception ex)
                {
                    ErrorInfo.Set(ex.Message, MethodBase.GetCurrentMethod(), EnumErrorType.Error);
                    model.result = 2;
                }
            }

            #region 如果出现错误，则写错误日志并返回错误内容

            WriteLog(ref returnVM);

            #endregion

            return returnVM;

        }

        /// <summary>
        /// 修改SN包装
        /// 新SN放到对象对应的data字段
        /// 旧SN放到对象对应的SN字段
        /// </summary>
        /// <param name="model">包装实体</param>
        /// <returns></returns>
        [HttpPost]
        [Authorize]
        public async Task<ApiBaseReturn<Propertyprovider>> ModifyPackingBySN([FromBody] Propertyprovider model)
        {
            ApiBaseReturn<Propertyprovider> returnVM = new ApiBaseReturn<Propertyprovider>();
            returnVM.Result = model;
            if (!ErrorInfo.Status)
            {
                try
                {
                    #region 参数检验

                    if (!ErrorInfo.Status && model.sfcsOperationLines.Id <= 0) ErrorInfo.Set(_localizerA["LINEID_NOT_NULL"], MethodBase.GetCurrentMethod(), EnumErrorType.Error);
                    if (!ErrorInfo.Status && model.sfcsOperationSites.ID <= 0) ErrorInfo.Set(_localizerA["SITE_NOT_NULL"], MethodBase.GetCurrentMethod(), EnumErrorType.Error);
                    if (!ErrorInfo.Status && model.data.IsNullOrEmpty()) ErrorInfo.Set(_localizerA["NEWSN_NOT_NULL"], MethodBase.GetCurrentMethod(), EnumErrorType.Error);
                    if (!ErrorInfo.Status && model.sn.IsNullOrEmpty()) ErrorInfo.Set(_localizerA["OldSN_NOT_NULL"], MethodBase.GetCurrentMethod(), EnumErrorType.Error);
                    if (!ErrorInfo.Status && model.carton.Carton_NO.IsNullOrEmpty()) ErrorInfo.Set(_localizerA["CARTON_SN_NOT_NULL"], MethodBase.GetCurrentMethod(), EnumErrorType.Error);

                    #endregion

                    #region 设置返回值
                    if (!ErrorInfo.Status)
                    {
                        try
                        {
                            //1.将旧SN进行返工处理
                            var result = await _sfcsContainerListrepository.ReworkProcessBySN(_repositoryRework, model.sn, model.sys_Manager.USER_NAME);
                            if (result)
                            {
                                //2.2将新SN做新增处理(进行新增)
                                var boolResult = (await PackingProcessAsync(model));
                                model.result = boolResult ? 0 : 2;
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
                    ErrorInfo.Set(ex.Message, MethodBase.GetCurrentMethod(), EnumErrorType.Error);
                    model.result = 2;
                }
            }

            #region 如果出现错误，则写错误日志并返回错误内容

            WriteLog(ref returnVM);

            #endregion

            return returnVM;

        }

        #endregion

        #region 老化管理

        /// <summary>
        /// 保存PDA老化记录
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        [Authorize]
        public async Task<ApiBaseReturn<Propertyprovider>> SaveOldRecordData([FromBody] Propertyprovider model)
        {
            ApiBaseReturn<Propertyprovider> returnVM = new ApiBaseReturn<Propertyprovider>();

            try
            {
                #region 参数检验

                if (String.IsNullOrEmpty(model.isEnd) || (model.isEnd != GlobalVariables.EnableY && model.isEnd != GlobalVariables.EnableN)) { ErrorInfo.Set(_localizerA["IS_END_NOT_NULL"], MethodBase.GetCurrentMethod(), EnumErrorType.Error); }
                if (!ErrorInfo.Status && model.sfcsOperationSites.ID <= 0) { ErrorInfo.Set(_localizerA["SITE_NOT_NULL"], MethodBase.GetCurrentMethod(), EnumErrorType.Error); }
                if (!ErrorInfo.Status && (model.sys_Manager == null || model.sys_Manager.USER_NAME.IsNullOrEmpty())) { ErrorInfo.Set(_localizerA["USER_NAME_NOT_EMPTY"], MethodBase.GetCurrentMethod(), EnumErrorType.Error); }
                SfcsRuncardListModel runcard = null; SfcsOldRecordListModel orModel = null;
                if (!ErrorInfo.Status && model.data.IsNullOrEmpty()) { ErrorInfo.Set(_localizerA["SN_NOT_NULL"], MethodBase.GetCurrentMethod(), EnumErrorType.Error); }
                runcard = (await _repository.GetListByTableEX<SfcsRuncardListModel>("*", "SFCS_RUNCARD", " And SN=:SN", new { SN = model.data }))?.FirstOrDefault();
                if (runcard == null)
                {
                    ErrorInfo.Set(_localizerA["SN_ERROR"], MethodBase.GetCurrentMethod(), EnumErrorType.Error);
                }
                else
                {
                    orModel = (await _sfcsRuncardRepository.GetListByTableEX<SfcsOldRecordListModel>("*", "SFCS_OLD_RECORD", " AND SN_ID = :SN_ID AND SN = :SN AND STAUTS = :STAUTS ",
                        new { SN_ID = runcard.ID, SN = runcard.SN, STAUTS = GlobalVariables.EnableN }))?.FirstOrDefault();
                }

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
                            // 1.开始老化添加老化记录 重复开始老化就进行数据更新
                            // 2.结束老化 先SN过站 2.1 过站成功 修改老化记录STAUTS为Y(结束); 2.2 过站失败提示失败信息
                            if (model.isEnd == GlobalVariables.EnableN)
                            {
                                model.result = 0;
                                if (orModel == null)
                                {
                                    orModel = new SfcsOldRecordListModel();
                                    orModel.SN_ID = runcard.ID;
                                    orModel.SN = runcard.SN;
                                }
                                orModel.BEGING_CREATOR = model.sys_Manager.USER_NAME;
                                orModel.STAUTS = GlobalVariables.EnableN;
                                model.msg = String.Format(_localizerA["OLD_BEGING_PASS"], model.data);
                            }
                            else if (model.isEnd == GlobalVariables.EnableY)
                            {
                                if (orModel == null) { throw new Exception(String.Format(_localizerA["SN_BEGING_OLD"], model.data)); }
                                model = await DoProcessAsync(model, tran);
                                orModel.END_CREATOR = model.sys_Manager.USER_NAME;
                                orModel.STAUTS = GlobalVariables.EnableY;
                            }

                            orModel.SITE_ID = model.sfcsOperationSites.ID;

                            if (model.result == 0)
                            {
                                String sql = orModel.ID > GlobalVariables.DecimalDefaults ? "UPDATE SFCS_OLD_RECORD SET SITE_ID = :SITE_ID,END_TIME = SYSDATE, END_CREATOR = :END_CREATOR, STAUTS = :STAUTS WHERE ID = :ID" : "INSERT INTO SFCS_OLD_RECORD (ID, SN_ID, SN, SITE_ID, BEGIN_TIME, BEGING_CREATOR, STAUTS) VALUES (SFCS_OLD_RECORD_SEQ.NEXTVAL, :SN_ID, :SN, :SITE_ID, SYSDATE, :BEGING_CREATOR, 'N')";
                                int result = _sfcsRuncardRepository.Execute(sql, new
                                {
                                    ID = orModel.ID,
                                    SN = orModel.SN,
                                    STAUTS = model.isEnd,
                                    SN_ID = orModel.SN_ID,
                                    SITE_ID = orModel.SITE_ID,
                                    END_CREATOR = orModel.END_CREATOR,
                                    BEGING_CREATOR = orModel.BEGING_CREATOR
                                }, tran);
                                if (result <= 0) { throw new Exception("SAVE_DATA_FAIL"); }
                            }
                            else if (model.result == 2)
                            {
                                throw new Exception(model.msg);
                            }

                            tran.Commit();
                        }
                        catch (Exception ex)
                        {
                            tran.Rollback();
                            model.result = 2;
                            model.msg = ex.Message;
                            ErrorInfo.Set(_localizerA[ex.Message], MethodBase.GetCurrentMethod(), EnumErrorType.Error);
                        }
                        finally
                        {
                            model.data = String.Empty;
                            model.printer = null;
                            returnVM.Result = model;
                        }
                    }
                }

                #endregion
            }
            catch (Exception ex)
            {
                ErrorInfo.Set(ex.Message, MethodBase.GetCurrentMethod(), EnumErrorType.Error);
            }

            #region 如果出现错误，则写错误日志并返回错误内容

            WriteLog(ref returnVM);

            #endregion

            return returnVM;
        }

        #endregion

        #endregion

        #region 私有方法

        /// <summary>
        /// 过站处理
        /// </summary>
        /// <param name="propertyprovider"></param>
        /// <param name="dbTransaction"></param>
        /// <returns></returns>
        internal async Task<Propertyprovider> DoProcessAsync(Propertyprovider propertyprovider, IDbTransaction dbTransaction)
        {
            bool multflag = false;
            List<String> dataList = new List<string>();
            dataList.Add(propertyprovider.data);
            //if (propertyprovider.product != null && propertyprovider.sfcsRuncard != null)
            //{
            //    dataList.Add(propertyprovider.data);
            //}
            //else
            //{
            //    dataList = this.GetListData(propertyprovider, out multflag);
            //}   
            for (int i = 0; i < dataList.Count; i++)
            {
                if (!multflag && i >= 1)
                {
                    break;
                }
                propertyprovider.data = dataList[i];
                if (propertyprovider.product != null && propertyprovider.sfcsRuncard != null)
                {
                    //产品条码已经存在
                    //识别采集数据
                    bool componentVerify = this.VerifyComponentList(propertyprovider);
                    bool resourceVerify = this.VerifyResourceList(propertyprovider);
                    bool cartonVerify = this.VerifyCartonList(propertyprovider);
                    bool palletVerify = this.VerifyPalletList(propertyprovider);
                    bool uidVerify = this.VerifyUidList(propertyprovider);
                    if (!componentVerify && !resourceVerify
                        && !cartonVerify
                        && !palletVerify
                        && !uidVerify)
                    {
                        throw new Exception(String.Format("输入的条码:{0}无效", propertyprovider.data));
                    }

                    //执行校验的JOb（SubModule JOB）
                    KeyValuePair<Boolean, String> subModelJobResult = await JobDirector<SfcsRuncard, Decimal>.ExecuteSubModuleJobAsync(propertyprovider, _sfcsRuncardRepository, dbTransaction);
                    if (!subModelJobResult.Key)
                    {
                        throw new Exception(subModelJobResult.Value);
                    }
                    //校验
                    if (this.ConfirmDataListAccomplish(propertyprovider))
                    {
                        //SN数据过站
                        await this.StoreSnDataAsync(propertyprovider, dbTransaction);

                        //过站结束新建交换对象
                        Propertyprovider temp = propertyprovider;
                        propertyprovider = new Propertyprovider();
                        propertyprovider.sfcsOperationSites = temp.sfcsOperationSites;
                        propertyprovider.sfcsOperationLines = temp.sfcsOperationLines;
                        propertyprovider.sys_Manager = temp.sys_Manager;
                        propertyprovider.woNo = temp.product.workOrder;
                        propertyprovider.spotCheck = temp.spotCheck;
                        propertyprovider.printer = temp.printer;
                        propertyprovider.result = 0;
                        propertyprovider.msg = "过站处理成功!";
                    }
                    else
                    {
                        propertyprovider.result = 1;
                        propertyprovider.msg = "数据采集成功，还有数据需要采集";
                    }
                }
                else
                {
                    //产品条码不存在
                    Propertyprovider newPropertyprovider = new Propertyprovider();
                    newPropertyprovider.sfcsOperationSites = propertyprovider.sfcsOperationSites;
                    newPropertyprovider.sfcsOperationLines = propertyprovider.sfcsOperationLines;
                    newPropertyprovider.defects = propertyprovider.defects;
                    newPropertyprovider.sys_Manager = propertyprovider.sys_Manager;
                    newPropertyprovider.product = new Product();
                    newPropertyprovider.data = propertyprovider.data;
                    newPropertyprovider.carton = propertyprovider.carton;
                    newPropertyprovider.pallet = propertyprovider.pallet;
                    newPropertyprovider.printer = propertyprovider.printer;
                    newPropertyprovider.woNo = propertyprovider.woNo;
                    //獲取作業唯一鍵值
                    newPropertyprovider.OperationId = await _sfcsRuncardRepository.GetSFCSOperationID();
                    //1、校验SN
                    bool snCheck = await VerifySnData(newPropertyprovider, dbTransaction);
                    if (snCheck)
                    {

                        //执行校验的JOb（MUST RUN JOB）
                        KeyValuePair<Boolean, String> jobResult = await JobDirector<SfcsRuncard, Decimal>.ExecuteMustRunJobAsync(newPropertyprovider, _sfcsRuncardRepository, dbTransaction);
                        if (!jobResult.Key)
                        {
                            throw new Exception(jobResult.Value);
                        }
                        //校验成功
                        //查看有没有采集不良
                        List<Defect> defect = newPropertyprovider.defects;
                        if (defect != null && defect.Count > 0)
                        {
                            //执行校验的JOb（SubModule JOB）
                            KeyValuePair<Boolean, String> subModelJobResult = await JobDirector<SfcsRuncard, Decimal>.ExecuteSubModuleJobAsync(newPropertyprovider, _sfcsRuncardRepository, dbTransaction);
                            if (!subModelJobResult.Key)
                            {
                                throw new Exception(subModelJobResult.Value);
                            }
                            //进行不良过站
                            await this.StoreSnDataAsync(newPropertyprovider, dbTransaction);
                            this.StoreDefectData(newPropertyprovider, dbTransaction);
                            //过站结束新建交换对象
                            propertyprovider = new Propertyprovider();
                            propertyprovider.sfcsOperationSites = newPropertyprovider.sfcsOperationSites;
                            propertyprovider.sfcsOperationLines = newPropertyprovider.sfcsOperationLines;
                            propertyprovider.sys_Manager = newPropertyprovider.sys_Manager;
                            propertyprovider.carton = newPropertyprovider.carton;
                            propertyprovider.printer = newPropertyprovider.printer;
                            propertyprovider.woNo = newPropertyprovider.product.workOrder;
                            propertyprovider.spotCheck = newPropertyprovider.spotCheck;
                            propertyprovider.result = 1;
                            propertyprovider.msg = "不良过站处理成功!";

                        }
                        else
                        {
                            //创建采集对象
                            this.BuildComponentList(newPropertyprovider);
                            this.BuildPalletList(newPropertyprovider);
                            this.BuildResourceList(newPropertyprovider);
                            this.BuildUidList(newPropertyprovider);
                            this.BuildCartonList(newPropertyprovider);
                            //执行校验的JOb（SubModule JOB）
                            KeyValuePair<Boolean, String> subModelJobResult = await JobDirector<SfcsRuncard, Decimal>.ExecuteSubModuleJobAsync(newPropertyprovider, _sfcsRuncardRepository, dbTransaction);
                            if (!subModelJobResult.Key)
                            {
                                throw new Exception(subModelJobResult.Value);
                            }
                            if (!this.ConfirmDataListAccomplish(newPropertyprovider))
                            {
                                multflag = false;
                                this.GetCollectDataView(newPropertyprovider);
                                propertyprovider = newPropertyprovider;
                                propertyprovider.woNo = newPropertyprovider.product.workOrder;
                                propertyprovider.result = 1;
                                propertyprovider.msg = "识别产品条码正确，请您完成采集数据!";
                            }
                            else
                            {
                                //没有采集数据直接进行过站
                                await this.StoreSnDataAsync(newPropertyprovider, dbTransaction);
                                //过站结束新建交换对象
                                propertyprovider = new Propertyprovider();
                                propertyprovider.sfcsOperationSites = newPropertyprovider.sfcsOperationSites;
                                propertyprovider.sfcsOperationLines = newPropertyprovider.sfcsOperationLines;
                                propertyprovider.sys_Manager = newPropertyprovider.sys_Manager;
                                propertyprovider.carton = newPropertyprovider.carton;
                                propertyprovider.printer = newPropertyprovider.printer;
                                propertyprovider.woNo = newPropertyprovider.product.workOrder;
                                propertyprovider.spotCheck = newPropertyprovider.spotCheck;
                                propertyprovider.result = 0;
                                propertyprovider.msg = "过站处理成功!";
                            }
                        }
                        dataList = this.GetListData(propertyprovider, out multflag);
                    }
                    else
                    {
                        //校验失败
                        //校验不良信息
                        SfcsDefectConfig sfcsDefectConfig = _repository.QueryEx<SfcsDefectConfig>(
                            "select * from SFCS_DEFECT_CONFIG WHERE DEFECT_CODE = :DEFECT_CODE AND ENABLED = :ENABLED",
                            new
                            {
                                DEFECT_CODE = propertyprovider.data,
                                ENABLED = "Y"
                            }
                            ).FirstOrDefault();
                        List<Defect> defects = newPropertyprovider.defects;
                        if (defects == null)
                        {
                            newPropertyprovider.defects = new List<Defect>();
                        }
                        if (sfcsDefectConfig != null)
                        {
                            Defect defect = new Defect();
                            if (defect.collectDataList == null)
                            {
                                defect.collectDataList = new List<CollectData>();
                            }
                            CollectData collect = new CollectData();
                            collect.Data = newPropertyprovider.data;
                            collect.CollectBy = newPropertyprovider.sys_Manager.USER_NAME;
                            collect.CollectTime = DateTime.Now;
                            defect.collectDataList.Add(collect);
                            newPropertyprovider.defects.Add(defect);
                            newPropertyprovider.msg = String.Format("成功采集{0}不良代码！", newPropertyprovider.data);
                        }
                        else if (newPropertyprovider.data.StartsWith("$FD"))
                        {
                            if (newPropertyprovider.defects == null || newPropertyprovider.defects.Count <= 0)
                            {
                                throw new Exception("请先采集不良代码信息!");
                            }
                            Defect defect = newPropertyprovider.defects.LastOrDefault();
                            if (defect.defectDetailList != null)
                            {
                                defect.defectDetailList.Add(newPropertyprovider.data.Replace("$FD", ""));
                            }
                            else
                            {
                                defect.defectDetailList = new List<string>();
                                defect.defectDetailList.Add(newPropertyprovider.data.Replace("$FD", ""));
                            }
                            newPropertyprovider.msg = String.Format("成功采集{0}不良描述!", newPropertyprovider.data.Replace("$FD", ""));
                        }
                        else
                        {
                            throw new Exception(String.Format("输入的条码:{0}无效", propertyprovider.data));
                        }
                        newPropertyprovider.data = string.Empty;
                        propertyprovider = newPropertyprovider;
                    }
                }
            }
            return propertyprovider;
        }

        /// <summary>
        /// 包装过站
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        private async Task<bool> PackingProcessAsync(Propertyprovider model)
        {
            bool result = false;
            try
            {
                IDbConnection dbConnection = await _sfcsRuncardRepository.GetConnection();
                ConnectionFactory.OpenConnection(dbConnection);
                using (var tran = dbConnection.BeginTransaction())
                {
                    try
                    {

                        //1将新增SN进行包装过站
                        var propertyprovider = await this.DoProcessAsync(model, tran);
                        //2.修改SN的Runcard表中的Caton_no修改成当前箱号
                        //3、修改SFCS_CONTAINER_LIST表中的sequence数量（通过runcard表中有箱号相同的行数）
                        var dataResult = await _sfcsRuncardRepository.UpdateCartonNoBySNEx(model.carton.Carton_NO, model.data, tran);
                        result = dataResult > 0 ? true : false;
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
            catch (Exception ex)
            {
                throw ex;
            }
            return result;
        }

        /// <summary>
        /// 检查拼板
        /// </summary>
        /// <param name="Propertyprovider"></param>
        /// <returns></returns>
        private List<String> GetListData(Propertyprovider propertyprovider, out bool multfalg)
        {
            multfalg = false;
            List<String> dataList = new List<string>();
            bool isDefect = false;
            if (propertyprovider.defects != null && propertyprovider.defects.Count > 0)
            {
                isDefect = true;
            }
            String multSql = @"select SMD1.*from SMT_MULTIPANEL_DETAIL SMD1,SMT_MULTIPANEL_DETAIL SMD2
                WHERE SMD1.MULT_HEADER_ID = SMD2.MULT_HEADER_ID AND SMD2.SN = :SN";
            var multRuncardList = _sfcsRuncardRepository.QueryEx<SmtMultipanelDetail>(multSql, new
            {
                SN = propertyprovider.data
            });
            if (multRuncardList == null || multRuncardList.Count <= 0)
            {
                multfalg = false;
            }
            else
            {
                String sql = @"select SCMR.* from SFCS_COLLECT_MULTI_RUNCARD  SCMR, SFCS_RUNCARD SR WHERE SCMR.SN_ID = SR.ID AND SR.SN = :SN";

                SfcsCollectMultiRuncard sfcsCollectMultiRuncard = _sfcsRuncardRepository.QueryEx<SfcsCollectMultiRuncard>(sql, new
                {
                    SN = propertyprovider.data
                }).FirstOrDefault();
                if (sfcsCollectMultiRuncard != null && sfcsCollectMultiRuncard.STATUS == 2)
                {
                    multfalg = false;
                }
                else
                {
                    multfalg = true;
                }
            }
            if (multfalg)
            {
                String multConfigSql = @"select  DISTINCT SRC.* from SFCS_ROUTE_CONFIG SRC, SMT_MULTIPANEL_DETAIL SMD1, SMT_MULTIPANEL_DETAIL SMD2 ,SFCS_RUNCARD SR
                         where SRC.ROUTE_ID = SR.ROUTE_ID AND SMD1.SN = :SN and SMD1.MULT_HEADER_ID = SMD2.MULT_HEADER_ID AND SR.SN = SMD2.SN
                         AND SRC.CURRENT_OPERATION_ID = SR.WIP_OPERATION ORDER by ORDER_NO ASC";
                var sfcsRouteConfigs = _sfcsRuncardRepository.QueryEx<SfcsRouteConfig>(multConfigSql, new
                {
                    SN = propertyprovider.data
                });

                if (sfcsRouteConfigs != null && sfcsRouteConfigs.Count > 0)
                {
                    String S_SelectRuncardByMultSn = @"select distinct SR.* from  SMT_MULTIPANEL_DETAIL SMD1, SMT_MULTIPANEL_DETAIL SMD2 ,SFCS_RUNCARD SR
                        where SMD1.SN = :SN and SMD1.MULT_HEADER_ID = SMD2.MULT_HEADER_ID AND SR.SN = SMD2.SN";
                    var currentRuncardTable = _sfcsRuncardRepository.QueryEx<SfcsRuncard>(S_SelectRuncardByMultSn, new
                    {
                        SN = propertyprovider.data
                    }
                        );
                    decimal currentMinWipOperation = sfcsRouteConfigs.FirstOrDefault().CURRENT_OPERATION_ID;

                    var statusList = from statusArray in currentRuncardTable
                                     where (new Decimal?[] { 1, 8, 10 }).Contains(statusArray.STATUS)
                                     select statusArray;
                    var wipList = from wipArray in currentRuncardTable
                                  group wipArray by wipArray.WIP_OPERATION into n
                                  select new
                                  {
                                      n.Key,
                                      count = n.Count()
                                  };
                    if (!isDefect)
                    {
                        //测试ok
                        //1、查询拼板中wip_opearation
                        //1.1 wip_operation相同并且status状态都在pass, repaired ,rework其中的状态，连扳过站
                        //1.2 wip_operation相同且状态不是pass, repaired ,rework其中的状态不允许过站
                        //1.3 wip_operation不相同且status状态都在pass, repaired ,rework其中的状态，按SFCS_ROUTE_CONFIG中ORDER_NO最小SN过站，其他不做处理
                        //1.4 wip_operation不相同且status状态不都是pass, repaired ,rework其中的状态，连板中有待维修，不允许过站
                        if (statusList.Count() == currentRuncardTable.Count
                            && wipList.Count() == 1)
                        {
                            foreach (SmtMultipanelDetail smtMultipanelDetail in multRuncardList)
                            {
                                dataList.Add(smtMultipanelDetail.SN);
                            }
                        }
                        else if (wipList.Count() == 1 && statusList.Count() != currentRuncardTable.Count)
                        {
                            throw new Exception("当前产品已经是不良请进入维修!");
                        }
                        else if (wipList.Count() > 1 && statusList.Count() == currentRuncardTable.Count)
                        {
                            //var currentRuncardTable.Where(f => f.WIP_OPERATION == currentMinWipOperation).ToArray();
                            var tempRuncardList = currentRuncardTable.Where(f => f.WIP_OPERATION == currentMinWipOperation);
                            foreach (SfcsRuncard tempSfcsRuncar in tempRuncardList)
                            {
                                dataList.Add(tempSfcsRuncar.SN);
                            }
                        }
                    }
                    else
                    { //测试fail
                      //2、查询拼板中wip_opearation
                      //2.1 wip_operation相同并且status状态都在pass, repaired ,rework其中的状态，单独对当前SN不良过站，其他SN不做处理。
                      //2.2 wip_operation相同且状态不是pass, repaired ,rework其中的状态不允许过站，进入维修状态。
                      //2.3 wip_operation不相同且status状态都在pass, repaired ,rework其中的状态，按SFCS_ROUTE_CONFIG中ORDER_NO最小SN,不良过站，记录实际不良SN，其他不做处理
                      //2.4 wip_operation不相同且status状态不都是pass, repaired ,rework其中的状态，
                      //2.4.1 当前SN在pass, repaired ,rework其中的状态 ，其他非pass,repaired,rework 状态的wip_operation 与当前sn 维修operation是否一致，不一致不允许不良过站，不一致允许不良进入维修过站
                      //2.4.2 当前SN不在pass, repaired ,rework其中的状态， 当前SN已经是不良不能重复不良过站
                        SmtMultipanelDetail currentMultSn = multRuncardList.Where(f => f.SN == propertyprovider.data).FirstOrDefault();
                        String defectDetail = String.Format("产品序号:{0},发生不良", currentMultSn.TASK_NO);
                        if (propertyprovider.defects.FirstOrDefault().defectDetailList == null)
                        {
                            propertyprovider.defects.FirstOrDefault().defectDetailList = new List<string>();
                            propertyprovider.defects.FirstOrDefault().defectDetailList.Add(defectDetail);
                        }
                        else
                        {
                            propertyprovider.defects.FirstOrDefault().defectDetailList.Add(defectDetail);
                        }
                        if (statusList.Count() == currentRuncardTable.Count
                                 && wipList.Count() == 1)
                        {
                            dataList.Add(propertyprovider.data);
                        }
                        else if (wipList.Count() == 1 && statusList.Count() != currentRuncardTable.Count)
                        {
                            throw new Exception(String.Format("当前产品{0}已经是不良请进入维修!", propertyprovider.data));
                        }
                        else if (wipList.Count() > 1 && statusList.Count() == currentRuncardTable.Count)
                        {
                            var tempRuncardList = currentRuncardTable.Where(f => f.WIP_OPERATION == currentMinWipOperation);
                            foreach (SfcsRuncard tempSfcsRuncar in tempRuncardList)
                            {
                                dataList.Add(tempSfcsRuncar.SN);
                            }
                        }
                        else
                        {
                            var currentSnRow = currentRuncardTable.Where(f => f.SN == propertyprovider.data).FirstOrDefault();

                            if (currentSnRow.STATUS.Equals(1) || currentSnRow.STATUS.Equals(8) || currentSnRow.STATUS.Equals(10))
                            {
                                decimal currentRepairOperation = sfcsRouteConfigs.Where(f => f.CURRENT_OPERATION_ID == currentSnRow.WIP_OPERATION).FirstOrDefault().REPAIR_OPERATION_ID;
                                if (currentRuncardTable.Where(f => f.WIP_OPERATION == currentRepairOperation).Count() > 0)
                                {
                                    dataList.Add(propertyprovider.data);
                                }
                                else
                                {
                                    throw new Exception(String.Format("当前产品{0}已经是不良请进入维修!", propertyprovider.data));
                                }
                            }
                            else
                            {
                                throw new Exception(String.Format("当前产品{0}已经是不良请进入维修!", propertyprovider.data));
                            }
                        }
                    }
                }
                else
                {
                    if (!isDefect)
                    {
                        foreach (SmtMultipanelDetail smtMultipanelDetail in multRuncardList)
                        {
                            dataList.Add(smtMultipanelDetail.SN);
                        }
                    }
                    else
                    {
                        SmtMultipanelDetail currentMultSn = multRuncardList.Where(f => f.SN == propertyprovider.data).FirstOrDefault();
                        String defectDetail = String.Format("产品序号:{0},发生不良", currentMultSn.TASK_NO);
                        if (propertyprovider.defects.FirstOrDefault().defectDetailList == null)
                        {
                            propertyprovider.defects.FirstOrDefault().defectDetailList = new List<string>();
                            propertyprovider.defects.FirstOrDefault().defectDetailList.Add(defectDetail);
                        }
                        dataList.Add(propertyprovider.data);
                    }
                }
            }
            else
            {
                dataList.Add(propertyprovider.data);
            }
            return dataList;
        }

        /// <summary>
        /// 初始化制程信息
        /// </summary>
        /// <param name="routeId"></param>
        /// <returns></returns>
        private Route InitialRoute(Decimal routeId)
        {
            Route route = new Route();
            SfcsRoutes sfcsRoutes = _repository.QueryEx<SfcsRoutes>(
                "select * from SFCS_ROUTES where ID = :ID",
                new
                {
                    ID = routeId
                }
                ).FirstOrDefault();
            if (sfcsRoutes == null)
            {
                throw new Exception(String.Format("系统找不到对应的制程：{0}", routeId));
            }
            route.routeName = sfcsRoutes.ROUTE_NAME;
            route.routeID = sfcsRoutes.ID;
            List<SfcsRouteConfig> sfcsRouteConfigs =
            _repository.QueryEx<SfcsRouteConfig>(
                "select * from SFCS_ROUTE_CONFIG where ROUTE_ID = :ROUTE_ID ORDER BY ORDER_NO",
                new
                {
                    ROUTE_ID = sfcsRoutes.ID
                }
                );
            if (sfcsRouteConfigs != null && sfcsRouteConfigs.Count > 0)
            {
                route.sfcsRouteConfigs = sfcsRouteConfigs;
            }
            else
            {
                throw new Exception(String.Format("制程：{0}没有配置工序信息!", sfcsRoutes.ROUTE_NAME));
            }
            return route;
        }

        /// <summary>
        /// 檢查組件作業是否完成
        /// </summary>
        /// <returns></returns>
        private Boolean ConfirmDataListAccomplish(Propertyprovider propertyprovider)
        {
            bool result = true;
            if (propertyprovider.carton != null
                && propertyprovider.carton.Status == StandardObjectStatusType.Incompleted)
            {
                result = false;
            }

            if (propertyprovider.pallet != null
                && propertyprovider.pallet.Status == StandardObjectStatusType.Incompleted)
            {
                result = false;
            }
            if (!propertyprovider.ComponentFinish)
            {
                bool flag = true;
                foreach (Component component in propertyprovider.components)
                {
                    if (component.Status == StandardObjectStatusType.Incompleted)
                    {
                        flag = false;
                    }
                }
                propertyprovider.ComponentFinish = flag;
                if (result && !flag)
                {
                    result = flag;
                }
            }
            if (!propertyprovider.ResourceFinish)
            {
                bool flag = true;
                foreach (Resource resource in propertyprovider.Resources)
                {
                    if (resource.Status == StandardObjectStatusType.Incompleted)
                    {
                        flag = false;
                    }
                }
                propertyprovider.ResourceFinish = flag;
                if (result && !flag)
                {
                    result = flag;
                }
            }
            if (!propertyprovider.UIDsFinish)
            {
                bool flag = true;
                foreach (UID uid in propertyprovider.UIDs)
                {
                    if (uid.Status == StandardObjectStatusType.Incompleted)
                    {
                        flag = false;
                    }
                }
                propertyprovider.UIDsFinish = flag;
                if (result && !flag)
                {
                    result = flag;
                }
            }

            return result;
        }
        /// <summary>
        /// 比较已经采集的数据
        /// </summary>
        /// <param name="data"></param>
        /// <param name="standardObject"></param>
        /// <returns></returns>
        private bool CompareData(String data, StandardObject standardObject)
        {
            foreach (CollectData collectData in standardObject.collectDataList)
            {
                if (collectData.Data == data)
                {
                    return true;
                }
            }
            return false;
        }
        /// <summary>
        /// 接收数据
        /// </summary>
        /// <param name="data"></param>
        /// <param name="standardObject"></param>
        private void AccepData(Decimal id, String data, StandardObject standardObject, Sys_Manager sysManager)
        {
            CollectData collectData = new CollectData();
            collectData.CollectObjectID = id;
            collectData.Data = data;
            collectData.CollectTime = DateTime.Now;
            collectData.CollectBy = sysManager.USER_NAME;
            standardObject.collectDataList.Add(collectData);
        }

        /// <summary>
        /// 生成零件采集数据
        /// </summary>
        private void BuildComponentList(Propertyprovider propertyprovider)
        {
            String componentProduction = @"SELECT SPC.ID OPERATION_OBJECT_ID,
               SPC.ID PRODUCT_COMPONENT_ID,
               SPC.PART_NO,
               SRC.PRODUCT_OPERATION_CODE,
               SPC.COMPONENT_ID,
               SAO.OBJECT_NAME,
               SAO.OBJECT_MARK,
               SP1.MEANING OBJECT_CATEGORY,
               SPC.ODM_COMPONENT_PN,
               SPC.CUSTOMER_COMPONENT_PN,
               SPC.DATA_FORMAT,
               SPC.COMPONENT_QTY,
               SPC.SERIALIZED,
               SPC.REWORK_REMOVE_FLAG,
               SPC.EDI_FLAG,
               SPC.ATTRIBUTE1 BOM_FLAG,
               SPC.COMPONENT_QTY,
               1 OBJECT_MODE,
               1 OPERATION_ORDER,
               SPC.DEVICE_FLAG,
               SPC.STANDARD_USE_COUNT,
               SPC.CHECK_DEFECT_FLAG
          FROM SFCS_PRODUCT_COMPONENTS SPC,
               SFCS_ROUTE_CONFIG SRC,
               SFCS_ALL_OBJECTS SAO,
               SFCS_PARAMETERS SP1
         WHERE     SPC.PART_NO = :PART_NO
               AND SPC.COLLECT_OPERATION_ID = :CURRENT_OPERATION_ID
               AND SRC.ROUTE_ID = :ROUTE_ID
               AND SRC.CURRENT_OPERATION_ID = SPC.COLLECT_OPERATION_ID
               AND SPC.ENABLED = 'Y'
               AND SPC.COMPONENT_ID = SAO.ID
               AND SP1.LOOKUP_TYPE = 'OBJECT_CATEGORY'
               AND SAO.OBJECT_CATEGORY = SP1.LOOKUP_CODE(+)
               AND SAO.ISACTIVE = 'Y'
               AND EXISTS(SELECT ID FROM SOP_ROUTES WHERE PART_NO = SPC.PART_NO AND STATUS = 1)";

            List<SfcsOperationComponents> sfcsOperationComponents = _sfcsRuncardRepository.QueryEx<SfcsOperationComponents>(
                componentProduction,
                new
                {
                    PART_NO = propertyprovider.product.partNumber,
                    CURRENT_OPERATION_ID = propertyprovider.sfcsRuncard.WIP_OPERATION,
                    ROUTE_ID = propertyprovider.route.routeID
                }
                );
            if (sfcsOperationComponents != null && sfcsOperationComponents.Count > 0)
            {
                List<Component> components = new List<Component>();
                foreach (SfcsOperationComponents sfcsOperationComponent in sfcsOperationComponents)
                {
                    Component component = new Component();
                    component._SfcsOperationComponents = sfcsOperationComponent;
                    component.OPERATION_OBJECT_ID = sfcsOperationComponent.COMPONENT_ID;
                    component.Format = sfcsOperationComponent.DATA_FORMAT;
                    component.collectDataList = new List<CollectData>();
                    components.Add(component);
                }
                propertyprovider.components = components;
                propertyprovider.ComponentFinish = false;
            }
        }

        /// <summary>
        /// 识别零件采集的数据
        /// </summary>
        /// <param name="propertyprovider"></param>
        private bool VerifyComponentList(Propertyprovider propertyprovider)
        {
            if (!propertyprovider.ComponentFinish)
            {
                foreach (Component component in propertyprovider.components)
                {
                    if (CompareData(propertyprovider.data, component))
                    {
                        throw new Exception("刷入的数据重复!");
                    }
                }
                foreach (Component component in propertyprovider.components)
                {
                    if (this.VerifyComponent(propertyprovider.data, component, propertyprovider.sys_Manager))
                    {
                        CollectDataView collectData = propertyprovider.collectDataViews.Where(f => f.OPERATION_OBJECT_ID == component.OPERATION_OBJECT_ID
                         && f.ODM_OBJECT_PN == component._SfcsOperationComponents.ODM_COMPONENT_PN).FirstOrDefault();
                        collectData.COLLECTED_QTY = component.collectDataList.Count;
                        return true;
                    }
                }

            }
            return false;
        }

        private bool VerifyComponent(String data, Component component, Sys_Manager sysManager)
        {
            if (component.Status != StandardObjectStatusType.Completed)
            {
                if (component.Format == null || component.Format.Trim() == "")
                {
                    //寻找runcard表，前置工序生成的零件

                    SfcsRuncard sfcsRuncard = _sfcsRuncardRepository.QueryEx<SfcsRuncard>(
                        "SELECT * FROM SFCS_RUNCARD WHERE SN = :SN",
                        new
                        {
                            SN = data
                        }
                        ).FirstOrDefault();
                    if (sfcsRuncard != null)
                    {
                        SfcsWo sfcsWo = _sfcsRuncardRepository.QueryEx<SfcsWo>(
                            "SELECT * FROM SFCS_WO WHERE ID = :ID",
                            new
                            {
                                ID = sfcsRuncard.WO_ID
                            }).FirstOrDefault();
                        if (component._SfcsOperationComponents.ODM_COMPONENT_PN == sfcsWo.PART_NO)
                        {
                            Decimal id = _sfcsRuncardRepository.QueryEx<Decimal>("SELECT SFCS_COLLECT_OBJECT_SEQ.NEXTVAL FROM DUAL").FirstOrDefault();
                            this.AccepData(id, data, component, sysManager);
                            if (component._SfcsOperationComponents.COMPONENT_QTY == component.collectDataList.Count)
                            {
                                component.Status = StandardObjectStatusType.Completed;
                            }
                            return true;
                        }
                        else
                        {
                            return false;
                        }
                    }
                    //解析条码
                    if (data.Is2DBarcode())
                    {
                        data = data.GeetReelCode();
                    }
                    //寻找仓库条码
                    ImsReel imsReel = _sfcsRuncardRepository.QueryEx<ImsReel>(
                        "select * from IMS_REEL WHERE CODE = :CODE ",
                        new
                        {
                            CODE = data
                        }).FirstOrDefault();
                    if (imsReel != null)
                    {
                        ImsPart imsPart = _sfcsRuncardRepository.QueryEx<ImsPart>(
                        "select * from IMS_PART where ID = :ID ",
                        new
                        {
                            ID = imsReel.PART_ID
                        }).FirstOrDefault();
                        if (component._SfcsOperationComponents.ODM_COMPONENT_PN == imsPart.CODE)
                        {
                            Decimal id = _sfcsRuncardRepository.QueryEx<Decimal>("SELECT SFCS_COLLECT_OBJECT_SEQ.NEXTVAL FROM DUAL").FirstOrDefault();
                            this.AccepData(id, data, component, sysManager);
                            if (component._SfcsOperationComponents.COMPONENT_QTY == component.collectDataList.Count)
                            {
                                component.Status = StandardObjectStatusType.Completed;
                            }
                            return true;
                        }
                        else
                        {
                            return false;
                        }
                    }
                }
                else
                {
                    if (FormatChecker.FormatCheck(data, component.Format))
                    {
                        Decimal id = _sfcsRuncardRepository.QueryEx<Decimal>("SELECT SFCS_COLLECT_OBJECT_SEQ.NEXTVAL FROM DUAL").FirstOrDefault();
                        this.AccepData(id, data, component, sysManager);
                        if (component._SfcsOperationComponents.COMPONENT_QTY == component.collectDataList.Count)
                        {
                            component.Status = StandardObjectStatusType.Completed;
                        }
                        return true;
                    }
                }
            }
            return false;

        }

        /// <summary>
        /// 保存采集的零件数据
        /// </summary>
        /// <param name="operationId"></param>
        /// <param name="propertyprovider"></param>
        /// <param name="transaction"></param>
        private void StoreComponentList(Decimal operationId, Propertyprovider propertyprovider, IDbTransaction transaction)
        {
            if (propertyprovider.components != null)
            {
                string I_InsertComponent = @"INSERT INTO SFCS_COLLECT_COMPONENTS(COLLECT_COMPONENT_ID,OPERATION_ID,SN_ID,WO_ID,
                                                  PRODUCT_OPERATION_CODE,COMPONENT_ID,COMPONENT_NAME,ODM_COMPONENT_PN,
                                                  CUSTOMER_COMPONENT_PN,ODM_COMPONENT_SN,COMPONENT_QTY,SERIALIZED,
                                                  COLLECT_SITE,COLLECT_TIME,COLLECT_BY,REWORK_REMOVE_FLAG,EDI_FLAG,ATTRIBUTE1,DEVICE_FLAG)
                                                  VALUES(:COLLECT_COMPONENT_ID,:OPERATION_ID,:SN_ID,:WO_ID,:PRODUCT_OPERATION_CODE,
                                                  :COMPONENT_ID,:COMPONENT_NAME,:ODM_COMPONENT_PN,
                                                  :CUSTOMER_COMPONENT_PN,:ODM_COMPONENT_SN,:COMPONENT_QTY,:SERIALIZED,
                                                  :COLLECT_SITE,:COLLECT_TIME,:COLLECT_BY,:REWORK_REMOVE_FLAG,:EDI_FLAG,:BOM_FLAG,:DEVICE_FLAG) ";
                foreach (Component component in propertyprovider.components)
                {
                    //存儲數據
                    foreach (CollectData collectData in component.collectDataList)
                    {
                        _sfcsRuncardRepository.Execute(
                            I_InsertComponent,
                            new
                            {
                                COLLECT_COMPONENT_ID = collectData.CollectObjectID,
                                OPERATION_ID = operationId,
                                SN_ID = propertyprovider.sfcsRuncard.ID,
                                WO_ID = propertyprovider.product.workOrderId,
                                PRODUCT_OPERATION_CODE = component._SfcsOperationComponents.PRODUCT_OPERATION_CODE,
                                COMPONENT_ID = component._SfcsOperationComponents.COMPONENT_ID,
                                COMPONENT_NAME = component._SfcsOperationComponents.OBJECT_NAME,
                                ODM_COMPONENT_PN = component._SfcsOperationComponents.ODM_COMPONENT_PN,
                                CUSTOMER_COMPONENT_PN = component._SfcsOperationComponents.CUSTOMER_COMPONENT_PN,
                                ODM_COMPONENT_SN = collectData.Data,
                                COMPONENT_QTY = component._SfcsOperationComponents.COMPONENT_QTY,
                                SERIALIZED = component._SfcsOperationComponents.SERIALIZED == null ? "N" : component._SfcsOperationComponents.SERIALIZED,
                                COLLECT_SITE = propertyprovider.sfcsOperationSites.ID,
                                COLLECT_TIME = collectData.CollectTime,
                                COLLECT_BY = collectData.CollectBy,
                                REWORK_REMOVE_FLAG = component._SfcsOperationComponents.REWORK_REMOVE_FLAG,
                                EDI_FLAG = component._SfcsOperationComponents.EDI_FLAG,
                                BOM_FLAG = component._SfcsOperationComponents.BOM_FLAG,
                                DEVICE_FLAG = component._SfcsOperationComponents.DEVICE_FLAG
                            },
                            transaction
                            );
                    }
                }

            }
        }
        /// <summary>
        /// 生成Pallet采集数据
        /// </summary>
        private void BuildPalletList(Propertyprovider propertyprovider)
        {
            String palletProduct = @"select * from SFCS_PRODUCT_PALLET SPP WHERE PART_NO = :PART_NO AND COLLECT_OPERATION_ID= :COLLECT_OPERATION_ID AND ENABLED = 'Y'
                                    AND EXISTS(SELECT ID FROM SOP_ROUTES WHERE PART_NO = SPP.PART_NO AND STATUS = 1)";
            SfcsProductPallet sfcsProductPallet = _sfcsRuncardRepository.QueryEx<SfcsProductPallet>(
                palletProduct,
                new
                {
                    PART_NO = propertyprovider.product.partNumber,
                    COLLECT_OPERATION_ID = propertyprovider.sfcsOperationSites.OPERATION_ID
                }
                ).FirstOrDefault();
            if (sfcsProductPallet != null)
            {
                Pallet pallet;
                if (propertyprovider.pallet != null)
                {
                    pallet = propertyprovider.pallet;
                }
                else
                {
                    pallet = new Pallet();
                }
                pallet._SfcsProductPallet = sfcsProductPallet;
                pallet.OPERATION_OBJECT_ID = 675;
                pallet.Format = sfcsProductPallet.FORMAT;
                pallet.collectDataList = new List<CollectData>();
                propertyprovider.pallet = pallet;
            }
        }
        /// <summary>
        /// 识别Pallet采集的数据
        /// </summary>
        /// <param name="propertyprovider"></param>
        private bool VerifyPalletList(Propertyprovider propertyprovider)
        {
            if (propertyprovider.pallet == null || propertyprovider.pallet._SfcsProductPallet == null)
            {
                return false;
            }
            if ((propertyprovider.pallet != null)
                && (propertyprovider.pallet.Status == StandardObjectStatusType.Completed))
            {
                return false;
            }
            else
            {
                return VerifyPallet(propertyprovider.data, propertyprovider.pallet, propertyprovider.sys_Manager);

            }
        }

        private bool VerifyPallet(String data, Pallet pallet, Sys_Manager sysManager)
        {
            if (FormatChecker.FormatCheck(data, pallet.Format))
            {
                var fullCnt = _repository.QueryEx<int>("SELECT COUNT(1) FROM SFCS_COLLECT_PALLETS WHERE PALLET_NO=:PALLET_NO AND PART_NO=:PART_NO AND STATUS=1", new
                {
                    PALLET_NO = data,
                    pallet._SfcsProductPallet.PART_NO
                }).FirstOrDefault();
                if (fullCnt > 0)
                {
                    throw new Exception("输入的栈板已满!");
                }

                Decimal ID = _repository.QueryEx<decimal>(
                    "SELECT SFCS_COLLECT_OBJECT_SEQ.NEXTVAL FROM DUAL").FirstOrDefault();
                this.AccepData(ID, data, pallet, sysManager);
                pallet.Status = StandardObjectStatusType.Completed;
                return true;
            }
            return false;
        }
        /// <summary>
        /// 保存采集的Pallet数据
        /// </summary>
        /// <param name="propertyprovider"></param>
        private void StorePalletList(Propertyprovider propertyprovider, IDbTransaction transaction)
        {
            if (propertyprovider.pallet != null)
            {
                CollectData collectData = propertyprovider.pallet.collectDataList.FirstOrDefault();
                List<SfcsCollectPalletsListModel> sfcsCollectPalletsListModels = _sfcsRuncardRepository.QueryEx<SfcsCollectPalletsListModel>(
                "SELECT SCP.* FROM SFCS_COLLECT_PALLETS SCP WHERE PALLET_NO = :PALLET_NO",
                new
                {
                    PALLET_NO = collectData.Data
                }
                );
                if (sfcsCollectPalletsListModels == null ||
                    sfcsCollectPalletsListModels.Count <= 0)
                {
                    //新增栈板记录,0表示卡通未满1表示栈板已满
                    decimal status = GlobalVariables.PalletInUsingStatus;
                    if (propertyprovider.pallet.DefinedQty > 0)
                    {
                        if (propertyprovider.pallet.DefinedQty == 1)
                        {
                            status = GlobalVariables.PalletFinishedStatus;
                        }
                    }
                    else if (propertyprovider.pallet._SfcsProductPallet.STANDARD_QUANTITY == 1)
                    {
                        status = GlobalVariables.PalletFinishedStatus;
                    }

                    string I_InsertPallet = @"INSERT INTO SFCS_COLLECT_PALLETS (COLLECT_PALLET_ID,PALLET_NO,PART_NO,LENGTH,WIDTH,HEIGHT,CUBAGE,QUANTITY,STATUS,COLLECT_SITE,COLLECT_BY,COLLECT_TIME)
                                               VALUES (:COLLECT_PALLET_ID,:PALLET_NO,:PART_NO,:LENGTH,:WIDTH,:HEIGHT,:CUBAGE,:QUANTITY,:STATUS,:COLLECT_SITE,:COLLECT_BY,:COLLECT_TIME) ";
                    String LENGTH = String.Empty;
                    String WIDTH = String.Empty;
                    String HEIGHT = String.Empty;
                    String CUBAGE = String.Empty;
                    if (propertyprovider.pallet._SfcsProductPallet != null)
                    {
                        LENGTH = propertyprovider.pallet._SfcsProductPallet.LENGTH;
                        WIDTH = propertyprovider.pallet._SfcsProductPallet.WIDTH;
                        HEIGHT = propertyprovider.pallet._SfcsProductPallet.HEIGHT;
                        CUBAGE = propertyprovider.pallet._SfcsProductPallet.STANDARD_CUBAGE;
                    }
                    _sfcsRuncardRepository.Execute(I_InsertPallet,
                        new
                        {
                            COLLECT_PALLET_ID = collectData.CollectObjectID,
                            PART_NO = propertyprovider.product.partNumber,
                            PALLET_NO = collectData.Data,
                            LENGTH = LENGTH,
                            WIDTH = WIDTH,
                            HEIGHT = HEIGHT,
                            CUBAGE = CUBAGE,
                            STATUS = status,
                            QUANTITY = 1,
                            COLLECT_SITE = propertyprovider.sfcsOperationSites.ID,
                            COLLECT_BY = collectData.CollectBy,
                            COLLECT_TIME = collectData.CollectTime,
                        }, transaction);
                }
                else
                {
                    decimal scannedQty;
                    decimal status = (Decimal)sfcsCollectPalletsListModels.FirstOrDefault().STATUS;
                    if (propertyprovider.pallet.DefinedQty > 0)
                    {

                        scannedQty = _sfcsRuncardRepository.QueryEx<decimal>(
                            "select count( distinct CARTON_NO) from SFCS_RUNCARD where PALLET_NO = :PALLET_NO",
                            new
                            {
                                PALLET_NO = collectData.Data
                            }
                            ).FirstOrDefault();
                        if ((scannedQty) == propertyprovider.pallet.DefinedQty)
                        {
                            //1表示栈板已满
                            status = GlobalVariables.PalletFinishedStatus;
                        }
                    }
                    else
                    {
                        scannedQty = (Decimal)sfcsCollectPalletsListModels.FirstOrDefault().QUANTITY;
                        if ((scannedQty) == (decimal)propertyprovider.pallet._SfcsProductPallet.STANDARD_QUANTITY)
                        {
                            //1表示栈板已满
                            status = GlobalVariables.PalletFinishedStatus;
                        }
                    }
                    string U_UpdatePallet = @"UPDATE SFCS_COLLECT_PALLETS SET QUANTITY=:QUANTITY, STATUS=:STATUS WHERE PALLET_NO=:PALLET_NO ";
                    //更新栈板狀態數量
                    _sfcsRuncardRepository.Execute(
                        U_UpdatePallet,
                        new
                        {
                            QUANTITY = scannedQty,//栈板存放的箱数
                            STATUS = status,
                            PALLET_NO = collectData.Data
                        },
                        transaction
                        );
                }
                //更新runcard carton
                string U_UpdateRuncardPallet = @"UPDATE SFCS_RUNCARD SET PALLET_NO=:PALLET_NO WHERE SN=:SN ";
                _sfcsRuncardRepository.Execute(U_UpdateRuncardPallet,
                    new
                    {
                        PALLET_NO = collectData.Data,
                        SN = propertyprovider.sfcsRuncard.SN
                    }, transaction);
            }

        }
        /// <summary>
        /// 生成Resource采集数据
        /// </summary>
        private void BuildResourceList(Propertyprovider propertyprovider)
        {
            String resourcesProduction = @"SELECT SPR.ID OPERATION_OBJECT_ID,
               SPR.ID PRODUCT_RESOURCE_ID,
               SPR.PART_NO,
               SRC.PRODUCT_OPERATION_CODE,
               SPR.RESOURCE_ID,
               SAO.OBJECT_NAME,
               SAO.OBJECT_MARK,
               SP1.MEANING OBJECT_CATEGORY,
               SPR.DATA_FORMAT,
               SPR.FIXED_VALUE,
               SPR.RESOURCE_QTY,
               SPR.BINDING_SITE,
               SPR.REPEATED,
               SPR.REWORK_REMOVE_FLAG,
               SPR.EDI_FLAG
          FROM SFCS_PRODUCT_RESOURCES SPR,
               SFCS_ROUTE_CONFIG SRC,
               SFCS_ALL_OBJECTS SAO,
               SFCS_PARAMETERS SP1
         WHERE     SPR.PART_NO = :PART_NO
               AND SPR.COLLECT_OPERATION_ID = :COLLECT_OPERATION_ID
               AND SRC.CURRENT_OPERATION_ID = SPR.COLLECT_OPERATION_ID
               AND SRC.ROUTE_ID = :ROUTE_ID
               AND SPR.RESOURCE_ID = SAO.ID
               AND SP1.LOOKUP_TYPE = 'OBJECT_CATEGORY'
               AND SAO.OBJECT_CATEGORY = SP1.LOOKUP_CODE(+)
               AND SAO.ISACTIVE = 'Y'
               AND SPR.ENABLED = 'Y'
               AND EXISTS(SELECT ID FROM SOP_ROUTES WHERE PART_NO = SPR.PART_NO AND STATUS = 1)";

            List<SfcsOperationResources> sfcsOperationResources = _sfcsRuncardRepository.QueryEx<SfcsOperationResources>(
                resourcesProduction,
                new
                {
                    PART_NO = propertyprovider.product.partNumber,
                    COLLECT_OPERATION_ID = propertyprovider.sfcsOperationSites.OPERATION_ID,
                    ROUTE_ID = propertyprovider.route.routeID
                }
                );
            if (sfcsOperationResources != null && sfcsOperationResources.Count > 0)
            {
                List<Resource> resources = new List<Resource>();
                foreach (SfcsOperationResources sfcsOperationResource in sfcsOperationResources)
                {
                    Resource resource = new Resource();
                    resource._SfcsOperationResources = sfcsOperationResource;
                    resource.OPERATION_OBJECT_ID = sfcsOperationResource.RESOURCE_ID;
                    resource.Format = sfcsOperationResource.DATA_FORMAT;
                    resource.collectDataList = new List<CollectData>();
                    resources.Add(resource);
                }
                propertyprovider.Resources = resources;
                propertyprovider.ResourceFinish = false;
            }
        }

        /// <summary>
        /// 识别Resource采集的数据
        /// </summary>
        /// <param name="propertyprovider"></param>
        private bool VerifyResourceList(Propertyprovider propertyprovider)
        {
            if (propertyprovider.ResourceFinish)
            {
                return false;
            }
            else
            {
                foreach (Resource resource in propertyprovider.Resources)
                {
                    if (CompareData(propertyprovider.data, resource))
                    {
                        throw new Exception("刷入的数据重复!");
                    }
                }
                foreach (Resource resource in propertyprovider.Resources)
                {
                    if (this.VerifyResource(propertyprovider.data, resource, propertyprovider.sys_Manager))
                    {
                        CollectDataView collectData = propertyprovider.collectDataViews.Where(f =>
                 f.OPERATION_OBJECT_ID == resource.OPERATION_OBJECT_ID).FirstOrDefault();
                        collectData.COLLECTED_QTY = resource.collectDataList.Count;
                        return true;
                    }
                }
            }
            return false;
        }
        /// <summary>
        /// 校验Resource
        /// </summary>
        /// <param name="data"></param>
        /// <param name="resource"></param>
        /// <param name="sysManager"></param>
        private bool VerifyResource(String data, Resource resource, Sys_Manager sysManager)
        {
            if (resource.Status == StandardObjectStatusType.Completed)
            {
                return false;
            }
            if (resource.Format == null || resource.Format.Trim() == "")
            {
                if (resource._SfcsOperationResources.FIXED_VALUE != null
                    && resource._SfcsOperationResources.FIXED_VALUE.Trim() != "")
                {
                    if (data == resource._SfcsOperationResources.FIXED_VALUE)
                    {
                        Decimal ID = _repository.QueryEx<decimal>(
                                                "SELECT SFCS_COLLECT_OBJECT_SEQ.NEXTVAL FROM DUAL").FirstOrDefault();
                        this.AccepData(ID, data, resource, sysManager);
                        if (resource.collectDataList.Count == resource._SfcsOperationResources.RESOURCE_QTY)
                        {
                            resource.Status = StandardObjectStatusType.Completed;
                        }
                        return true;
                    }
                }

            }
            else
            {
                if (FormatChecker.FormatCheck(data, resource.Format))
                {
                    Decimal ID = _repository.QueryEx<decimal>(
                        "SELECT SFCS_COLLECT_OBJECT_SEQ.NEXTVAL FROM DUAL").FirstOrDefault();
                    this.AccepData(ID, data, resource, sysManager);
                    if (resource.collectDataList.Count == resource._SfcsOperationResources.RESOURCE_QTY)
                    {
                        resource.Status = StandardObjectStatusType.Completed;
                    }
                    return true;
                }
            }
            return false;
        }
        /// <summary>
        /// 保存采集的Resource数据
        /// </summary>
        /// <param name="operationId"></param>
        /// <param name="propertyprovider"></param>
        /// <param name="transaction"></param>
        private void StoreResourceList(Decimal operationId, Propertyprovider propertyprovider, IDbTransaction transaction)
        {
            if (propertyprovider.ResourceFinish && propertyprovider.Resources != null)
            {
                string I_InsertResource = @"INSERT INTO SFCS_COLLECT_RESOURCES(COLLECT_RESOURCE_ID,OPERATION_ID,SN_ID,WO_ID,PRODUCT_OPERATION_CODE,
                                                 RESOURCE_ID,RESOURCE_NAME,RESOURCE_NO,RESOURCE_QTY,REWORK_REMOVE_FLAG,EDI_FLAG,COLLECT_SITE,COLLECT_BY,COLLECT_TIME)
                                                 VALUES(:COLLECT_RESOURCE_ID,:OPERATION_ID,:SN_ID,:WO_ID,:PRODUCT_OPERATION_CODE,:RESOURCE_ID,:RESOURCE_NAME,
                                                 :RESOURCE_NO,:RESOURCE_QTY,:REWORK_REMOVE_FLAG,:EDI_FLAG,:COLLECT_SITE,:COLLECT_BY,:COLLECT_TIME)";
                foreach (Resource resource in propertyprovider.Resources)
                {
                    foreach (CollectData collectData in resource.collectDataList)
                    {
                        _sfcsRuncardRepository.Execute(I_InsertResource, new
                        {
                            COLLECT_RESOURCE_ID = collectData.CollectObjectID,
                            OPERATION_ID = operationId,
                            SN_ID = propertyprovider.sfcsRuncard.ID,
                            WO_ID = propertyprovider.product.workOrderId,
                            PRODUCT_OPERATION_CODE = resource._SfcsOperationResources.PRODUCT_OPERATION_CODE,
                            RESOURCE_ID = resource._SfcsOperationResources.RESOURCE_ID,
                            RESOURCE_NAME = resource._SfcsOperationResources.OBJECT_NAME,
                            RESOURCE_NO = collectData.Data,
                            RESOURCE_QTY = resource._SfcsOperationResources.RESOURCE_QTY,
                            REWORK_REMOVE_FLAG = resource._SfcsOperationResources.REWORK_REMOVE_FLAG,
                            EDI_FLAG = resource._SfcsOperationResources.EDI_FLAG,
                            COLLECT_SITE = propertyprovider.sfcsOperationSites.ID,
                            COLLECT_BY = collectData.CollectBy,
                            COLLECT_TIME = collectData.CollectTime
                        }, transaction);
                    }
                }
            }
        }

        /// <summary>
        /// 組成UID鏈表
        /// </summary>
        private void BuildUidList(Propertyprovider propertyprovider)
        {
            String uidsProduction = @"SELECT SPU.ID  OPERATION_OBJECT_ID,
                    SPU.ID PRODUCT_UID_ID,
                    SPU.PART_NO,
                    SRC.PRODUCT_OPERATION_CODE,
                    SPU.UID_ID,
                    SAO.OBJECT_NAME,
                    SAO.OBJECT_MARK,
                    SP1.MEANING OBJECT_CATEGORY,
                    SPU.DATA_FORMAT,
                    SPU.UID_QTY,
                    SPU.SERIALIZED,
                    SPU.REWORK_REMOVE_FLAG,
                    SPU.EDI_FLAG
                    FROM SFCS_PRODUCT_UIDS SPU,
                    SFCS_ROUTE_CONFIG SRC,
                    SFCS_ALL_OBJECTS SAO,
                    SFCS_PARAMETERS SP1
                    WHERE SPU.PART_NO=:PART_NO AND SRC.ROUTE_ID =:ROUTE_ID
                    AND SPU.COLLECT_OPERATION_ID = :COLLECT_OPERATION_ID
                    AND SRC.CURRENT_OPERATION_ID = SPU.COLLECT_OPERATION_ID
                    AND SPU.UID_ID=SAO.ID AND SP1.LOOKUP_TYPE='OBJECT_CATEGORY' 
                    AND SAO.OBJECT_CATEGORY=SP1.LOOKUP_CODE(+) AND SAO.ISACTIVE='Y'
                    AND SPU.ENABLED='Y'
                    AND EXISTS(SELECT ID FROM SOP_ROUTES WHERE PART_NO = SPU.PART_NO AND STATUS = 1)";

            List<SfcsOperationUids> sfcsOperationUids = _sfcsRuncardRepository.QueryEx<SfcsOperationUids>(
                uidsProduction,
                new
                {
                    PART_NO = propertyprovider.product.partNumber,
                    COLLECT_OPERATION_ID = propertyprovider.sfcsOperationSites.OPERATION_ID,
                    ROUTE_ID = propertyprovider.route.routeID
                }
                );
            if (sfcsOperationUids != null && sfcsOperationUids.Count > 0)
            {
                List<UID> uIDs = new List<UID>();
                foreach (SfcsOperationUids sfcsOperationUid in sfcsOperationUids)
                {
                    UID uid = new UID();
                    uid._SfcsOperationUids = sfcsOperationUid;
                    uid.OPERATION_OBJECT_ID = sfcsOperationUid.UID_ID;
                    uid.Format = sfcsOperationUid.DATA_FORMAT;
                    uid.collectDataList = new List<CollectData>();
                    uIDs.Add(uid);
                }
                propertyprovider.UIDs = uIDs;
                propertyprovider.UIDsFinish = false;
            }
        }
        /// <summary>
        /// 识别Uid采集的数据
        /// </summary>
        /// <param name="propertyprovider"></param>
        private bool VerifyUidList(Propertyprovider propertyprovider)
        {
            if (propertyprovider.UIDsFinish)
            {
                return false;
            }
            foreach (UID uid in propertyprovider.UIDs)
            {
                if (CompareData(propertyprovider.data, uid))
                {
                    throw new Exception("刷入的数据重复!");
                }
            }
            foreach (UID uID in propertyprovider.UIDs)
            {
                if (this.VerifyUID(propertyprovider.data, uID, propertyprovider.sys_Manager))
                {
                    CollectDataView collectData = propertyprovider.collectDataViews.Where(f =>
                 f.OPERATION_OBJECT_ID == uID.OPERATION_OBJECT_ID).FirstOrDefault();
                    collectData.COLLECTED_QTY = uID.collectDataList.Count;
                    return true;
                }
            }
            return false;
        }
        /// <summary>
        /// 辨识UID
        /// </summary>
        /// <param name="data"></param>
        /// <param name="uid"></param>
        /// <param name="sysManager"></param>
        /// <returns></returns>
        private bool VerifyUID(String data, UID uid, Sys_Manager sysManager)
        {
            if (FormatChecker.FormatCheck(data, uid.Format))
            {
                List<SfcsCollectUids> sfcsCollectUids = _sfcsRuncardRepository.QueryEx<SfcsCollectUids>(
                    "select * from SFCS_COLLECT_UIDS WHERE UID_NUMBER = :UID_NUMBER",
                    new
                    {
                        UID_NUMBER = data
                    });
                if (sfcsCollectUids != null &&
                    sfcsCollectUids.Count > 0)
                {
                    throw new Exception(String.Format("系统中已存在唯一序号:{0}", data));
                }
                Decimal ID = _repository.QueryEx<decimal>(
                        "SELECT SFCS_COLLECT_OBJECT_SEQ.NEXTVAL FROM DUAL").FirstOrDefault();
                this.AccepData(ID, data, uid, sysManager);
                if (uid.collectDataList.Count == uid._SfcsOperationUids.UID_QTY)
                {
                    uid.Status = StandardObjectStatusType.Completed;
                }
                return true;

            }
            return false;
        }

        /// <summary>
        /// 保存采集的Uid数据
        /// </summary>
        /// <param name="operationId"></param>
        /// <param name="propertyprovider"></param>
        /// <param name="transaction"></param>
        private void StoreUidList(Decimal operationId, Propertyprovider propertyprovider, IDbTransaction transaction)
        {
            if (propertyprovider.UIDs != null)
            {
                string I_InsertUID = @"INSERT INTO SFCS_COLLECT_UIDS (COLLECT_UID_ID,OPERATION_ID,SN_ID,WO_ID,PRODUCT_OPERATION_CODE,UID_ID,UID_NAME,UID_NUMBER,PLANT_CODE,UID_QTY,ORDER_NO,
                                                        SERIALIZED,REWORK_REMOVE_FLAG,EDI_FLAG,COLLECT_SITE,COLLECT_BY,COLLECT_TIME)
                                                        VALUES (:COLLECT_UID_ID,:OPERATION_ID,:SN_ID,:WO_ID,:PRODUCT_OPERATION_CODE,:UID_ID,:UID_NAME,:UID_NUMBER,:PLANT_CODE,:UID_QTY,:ORDER_NO,:SERIALIZED,
                                                        :REWORK_REMOVE_FLAG,:EDI_FLAG,:COLLECT_SITE,:COLLECT_BY,:COLLECT_TIME)";
                foreach (UID uid in propertyprovider.UIDs)
                {
                    foreach (CollectData collectData in uid.collectDataList)
                    {
                        _sfcsRuncardRepository.Execute(I_InsertUID,
                            new
                            {
                                COLLECT_UID_ID = collectData.CollectObjectID,
                                OPERATION_ID = operationId,
                                SN_ID = propertyprovider.sfcsRuncard.ID,
                                WO_ID = propertyprovider.product.workOrderId,
                                PRODUCT_OPERATION_CODE = uid._SfcsOperationUids.PRODUCT_OPERATION_CODE,
                                UID_ID = uid._SfcsOperationUids.UID_ID,
                                UID_NAME = uid._SfcsOperationUids.OBJECT_NAME,
                                UID_NUMBER = collectData.Data,
                                PLANT_CODE = propertyprovider.product.plantCode,
                                UID_QTY = uid._SfcsOperationUids.UID_QTY,
                                ORDER_NO = 1,
                                SERIALIZED = uid._SfcsOperationUids.SERIALIZED,
                                REWORK_REMOVE_FLAG = uid._SfcsOperationUids.REWORK_REMOVE_FLAG,
                                EDI_FLAG = uid._SfcsOperationUids.EDI_FLAG,
                                COLLECT_SITE = propertyprovider.sfcsOperationSites.ID,
                                COLLECT_BY = collectData.CollectBy,
                                COLLECT_TIME = collectData.CollectTime
                            }, transaction); ;
                    }
                }
            }

        }
        /// <summary>
        /// 生成Carton采集数据
        /// </summary>
        private void BuildCartonList(Propertyprovider propertyprovider)
        {
            String cartonProduct = @"select * from SFCS_PRODUCT_CARTON SPC WHERE PART_NO = :PART_NO AND COLLECT_OPERATION_ID= :COLLECT_OPERATION_ID AND ENABLED = 'Y'
                                    AND EXISTS(SELECT ID FROM SOP_ROUTES WHERE PART_NO = SPC.PART_NO AND STATUS = 1)";
            SfcsProductCarton sfcsProductCarton = _sfcsRuncardRepository.QueryEx<SfcsProductCarton>(
                cartonProduct,
                new
                {
                    PART_NO = propertyprovider.product.partNumber,
                    COLLECT_OPERATION_ID = propertyprovider.sfcsOperationSites.OPERATION_ID
                }
                ).FirstOrDefault();
            if (sfcsProductCarton != null)
            {
                Carton carton;
                if (propertyprovider.carton != null)
                {
                    carton = propertyprovider.carton;
                }
                else
                {
                    carton = new Carton();
                }
                carton._SfcsProductCarton = sfcsProductCarton;
                carton.OPERATION_OBJECT_ID = 674;
                carton.Format = sfcsProductCarton.FORMAT;
                carton.collectDataList = new List<CollectData>();
                propertyprovider.carton = carton;
            }
        }
        /// <summary>
        /// 识别Carton采集的数据
        /// </summary>
        /// <param name="propertyprovider"></param>
        private bool VerifyCartonList(Propertyprovider propertyprovider)
        {
            if (propertyprovider.carton == null || propertyprovider.carton._SfcsProductCarton == null)
            {
                return false;
            }
            if ((propertyprovider.carton._SfcsProductCarton != null)
                 && (propertyprovider.carton.Status == StandardObjectStatusType.Completed))
            {
                return false;
            }
            else
            {
                return VerifyCarton(propertyprovider.data, propertyprovider.carton, propertyprovider.sys_Manager);
            }
        }
        /// <summary>
        /// 校验卡通号
        /// </summary>
        /// <returns></returns>
        private bool VerifyCarton(String data, Carton carton, Sys_Manager sysManager)
        {
            if (FormatChecker.FormatCheck(data, carton.Format))
            {
                var fullCnt = _repository.QueryEx<int>("SELECT COUNT(1) FROM SFCS_COLLECT_CARTONS WHERE CARTON_NO=:CARTON_NO AND PART_NO=:PART_NO AND STATUS=1", new
                {
                    CARTON_NO = data,
                    carton._SfcsProductCarton.PART_NO
                }).FirstOrDefault();
                if (fullCnt > 0)
                {
                    throw new Exception("输入的箱号已满箱!");
                }

                Decimal ID = _repository.QueryEx<decimal>(
                    "SELECT SFCS_COLLECT_OBJECT_SEQ.NEXTVAL FROM DUAL").FirstOrDefault();
                this.AccepData(ID, data, carton, sysManager);
                carton.Status = StandardObjectStatusType.Completed;
                return true;
            }
            return false;
        }
        /// <summary>
        /// 保存采集的Carton数据
        /// </summary>
        /// <param name="propertyprovider"></param>
        /// <param name="transaction"></param>
        private void StoreCartonList(Propertyprovider propertyprovider, IDbTransaction transaction)
        {
            if (propertyprovider.carton != null
                && propertyprovider.carton.Status == StandardObjectStatusType.Completed)
            {
                string S_SelectCollectCartons = @"SELECT SCC.* FROM SFCS_COLLECT_CARTONS SCC
                                                       WHERE CARTON_NO = :CARTON_NO";
                foreach (CollectData collectData in propertyprovider.carton.collectDataList)
                {
                    List<SfcsCollectCartonsListModel> sfcsCollectCartonsListModels =
                    _sfcsRuncardRepository.QueryEx<SfcsCollectCartonsListModel>(S_SelectCollectCartons,
                    new
                    {
                        CARTON_NO = collectData.Data

                    });

                    if (sfcsCollectCartonsListModels == null || sfcsCollectCartonsListModels.Count == 0)
                    {
                        decimal status = GlobalVariables.CartonInUsingStatus;

                        if (propertyprovider.carton.DefinedQty > 0)
                        {
                            if (propertyprovider.carton.DefinedQty == 1)
                            {
                                status = GlobalVariables.CartonFinishedStatus;
                            }
                        }
                        //新增卡通记录,0表示卡通未满1表示卡通已满
                        else if (propertyprovider.carton._SfcsProductCarton.STANDARD_QUANTITY == 1)
                        {
                            status = GlobalVariables.CartonFinishedStatus;
                        }
                        string I_InsertCarton = @"INSERT INTO SFCS_COLLECT_CARTONS(COLLECT_CARTON_ID,CARTON_NO,PART_NO,LENGTH,WIDTH,HEIGHT,CUBAGE,QUANTITY,STATUS,COLLECT_SITE,COLLECT_BY,COLLECT_TIME)
                                               VALUES (:COLLECT_CARTON_ID,:CARTON_NO,:PART_NO,:LENGTH,:WIDTH,:HEIGHT,:CUBAGE,:QUANTITY,:STATUS,:COLLECT_SITE,:COLLECT_BY,:COLLECT_TIME) ";
                        String LENGTH = String.Empty;
                        String WIDTH = String.Empty;
                        String HEIGHT = String.Empty;
                        String CUBAGE = String.Empty;
                        if (propertyprovider.carton._SfcsProductCarton != null)
                        {
                            LENGTH = propertyprovider.carton._SfcsProductCarton.LENGTH;
                            WIDTH = propertyprovider.carton._SfcsProductCarton.WIDTH;
                            HEIGHT = propertyprovider.carton._SfcsProductCarton.HEIGHT;
                            CUBAGE = propertyprovider.carton._SfcsProductCarton.CUBAGE;
                        }
                        //新增卡通
                        _sfcsRuncardRepository.Execute(I_InsertCarton,
                            new
                            {
                                COLLECT_CARTON_ID = collectData.CollectObjectID,
                                CARTON_NO = collectData.Data,
                                PART_NO = propertyprovider.product.partNumber,
                                LENGTH = LENGTH,
                                WIDTH = WIDTH,
                                HEIGHT = HEIGHT,
                                CUBAGE = CUBAGE,
                                QUANTITY = 1,
                                STATUS = status,
                                COLLECT_SITE = propertyprovider.sfcsOperationSites.ID,
                                COLLECT_BY = collectData.CollectBy,
                                COLLECT_TIME = collectData.CollectTime
                            }, transaction);
                    }
                    else
                    {
                        decimal status = (Decimal)sfcsCollectCartonsListModels.FirstOrDefault().STATUS;
                        String S_SelectRuncard = @"SELECT COUNT(*) FROM SFCS_RUNCARD SR WHERE SR.CARTON_NO = :CARTON_NO";
                        decimal scannedQty = _sfcsRuncardRepository.QueryEx<decimal>(S_SelectRuncard,
                            new
                            {
                                CARTON_NO = collectData.Data
                            }).FirstOrDefault();
                        if (propertyprovider.carton.DefinedQty > 0)
                        {

                            if ((++scannedQty) == propertyprovider.carton.DefinedQty)
                            {
                                //1表示卡通已满
                                status = GlobalVariables.CartonFinishedStatus;
                            }
                        }
                        else
                        {
                            if ((++scannedQty) == propertyprovider.carton._SfcsProductCarton.STANDARD_QUANTITY)
                            {
                                //1表示卡通已满
                                status = GlobalVariables.CartonFinishedStatus;
                            }
                        }
                        //更新卡通狀態數量
                        string U_UpdateCarton = @"UPDATE SFCS_COLLECT_CARTONS SET QUANTITY=:QUANTITY, STATUS=:STATUS WHERE CARTON_NO=:CARTON_NO ";
                        _sfcsRuncardRepository.Execute(U_UpdateCarton,
                            new
                            {
                                QUANTITY = scannedQty,
                                STATUS = status,
                                CARTON_NO = collectData.Data
                            },
                            transaction
                            );
                    }
                    string U_UpdateRuncardCarton = @"UPDATE SFCS_RUNCARD SET STATUS=3, CARTON_NO=:CARTON_NO WHERE SN=:SN ";
                    //更新runcard carton
                    _sfcsRuncardRepository.Execute(U_UpdateRuncardCarton,
                            new
                            {
                                SN = propertyprovider.sfcsRuncard.SN,
                                CARTON_NO = collectData.Data
                            },
                            transaction
                            );
                }
            }
        }
        /// <summary>
        /// 获取所有需要采集对象的视图
        /// </summary>
        /// <param name="propertyprovider"></param>
        private void GetCollectDataView(Propertyprovider propertyprovider)
        {
            String getCollectDataView = @"SELECT SPR.RESOURCE_ID OPERATION_OBJECT_ID,
               SPR.ID PRODUCT_OBJECT_ID,
               :PART_NO PART_NO,
               SRC.PRODUCT_OPERATION_CODE,
               SPR.RESOURCE_ID OBJECT_ID,
               SAO.OBJECT_NAME,
               '' ODM_OBJECT_PN,
               '' CUSTOMER_OBJECT_PN,
               SPR.DATA_FORMAT,
               '' FIXED_VALUE,
               '' SERIALIZED,
               SPR.RESOURCE_QTY NEED_ASSEMBLY_QTY,
               0 COLLECTED_QTY,
               '' REWORK_REMOVE_FLAG,
               '' EDI_FLAG,
               1 OBJECT_MODE,
               SP1.MEANING OBJECT_MODE_DESCRIPTION,
               1 OPERATION_ORDER,
               0 REMAINING_QTY,
               '' ATTRIBUTE1,
               '' ATTRIBUTE2,
               '' ATTRIBUTE3,
               '' ATTRIBUTE4,
               '' ATTRIBUTE5
          FROM SFCS_PRODUCT_RESOURCES SPR,
               SFCS_ROUTE_CONFIG SRC,
               SFCS_ALL_OBJECTS SAO,
               SFCS_PARAMETERS SP1
         WHERE     SPR.PART_NO = :PART_NO
               AND SRC.PRODUCT_OPERATION_CODE = :PRODUCT_OPERATION_CODE
               AND SRC.CURRENT_OPERATION_ID = SPR.COLLECT_OPERATION_ID
               AND SPR.RESOURCE_ID = SAO.ID
               AND SPR.ENABLED = 'Y'
               AND SP1.LOOKUP_TYPE = 'OBJECT_OPERATION_MODE'
               AND SP1.LOOKUP_CODE = '1'
        UNION ALL
        SELECT SPU.UID_ID OPERATION_OBJECT_ID,
               SPU.ID PRODUCT_OBJECT_ID,
               :PART_NO PART_NO,
               SRC.PRODUCT_OPERATION_CODE,
               SPU.UID_ID OBJECT_ID,
               SAO.OBJECT_NAME,
               '' ODM_OBJECT_PN,
               '' CUSTOMER_OBJECT_PN,
               SPU.DATA_FORMAT DATA_FORMAT,
               '' FIXED_VALUE,
               '' SERIALIZED,
               SPU.UID_QTY NEED_ASSEMBLY_QTY,
               0 COLLECTED_QTY,
               '' REWORK_REMOVE_FLAG,
               '' EDI_FLAG,
               1 OBJECT_MODE,
               SP1.MEANING OBJECT_MODE_DESCRIPTION,
               1 OPERATION_ORDER,
               0 REMAINING_QTY,
               '' ATTRBUTE1,
               '' ATTRIBUTE2,
               '' ATTRIBUTE3,
               '' ATTRIBUTE4,
               '' ATTRIBUTE5
          FROM SFCS_PRODUCT_UIDS SPU,
               SFCS_ROUTE_CONFIG SRC,
               SFCS_ALL_OBJECTS SAO,
               SFCS_PARAMETERS SP1
         WHERE     SPU.PART_NO = :PART_NO
               AND SRC.PRODUCT_OPERATION_CODE = :PRODUCT_OPERATION_CODE
               AND SRC.CURRENT_OPERATION_ID = SPU.COLLECT_OPERATION_ID
               AND SPU.UID_ID = SAO.ID
               AND SPU.ENABLED = 'Y'
               AND SP1.LOOKUP_TYPE = 'OBJECT_OPERATION_MODE'
               AND SP1.LOOKUP_CODE = '1'
        UNION ALL
        SELECT SPC.COMPONENT_ID OPERATION_OBJECT_ID,
               SPC.ID PRODUCT_OBJECT_ID,
               :PART_NO PART_NO,
               SRC.PRODUCT_OPERATION_CODE,
               SPC.COMPONENT_ID OBJECT_ID,
               SAO.OBJECT_NAME,
               SPC.ODM_COMPONENT_PN ODM_OBJECT_PN,
               SPC.CUSTOMER_COMPONENT_PN CUSTOMER_OBJECT_PN,
               SPC.DATA_FORMAT,
               '' FIXED_VALUE,
               '' SERIALIZED,
               SPC.COMPONENT_QTY NEED_ASSEMBLY_QTY,
               0 COLLECTED_QTY,
               '' REWORK_REMOVE_FLAG,
               '' EDI_FLAG,
               1 OBJECT_MODE,
               SP1.MEANING OBJECT_MODE_DESCRIPTION,
               1 OPERATION_ORDER,
               0 REMAINING_QTY,
               '' ATTRIBUTE1,
               '' ATTRIBUTE2,
               '' ATTRIBUTE3,
               '' ATTRIBUTE4,
               '' ATTRIBUTE5
          FROM SFCS_PRODUCT_COMPONENTS SPC,
               SFCS_ROUTE_CONFIG SRC,
               SFCS_ALL_OBJECTS SAO,
               SFCS_PARAMETERS SP1
         WHERE     SPC.PART_NO = :PART_NO
               AND SRC.PRODUCT_OPERATION_CODE = :PRODUCT_OPERATION_CODE
               AND SRC.CURRENT_OPERATION_ID = SPC.COLLECT_OPERATION_ID
               AND SPC.COMPONENT_ID = SAO.ID
               AND SPC.ENABLED = 'Y'
               AND SP1.LOOKUP_TYPE = 'OBJECT_OPERATION_MODE'
               AND SP1.LOOKUP_CODE = '1'
        UNION ALL
        SELECT 674 OPERATION_OBJECT_ID,
               SPC.ID PRODUCT_OBJECT_ID,
               :PART_NO PART_NO,
               SRC.PRODUCT_OPERATION_CODE,
               674 OBJECT_ID,
               SAO.OBJECT_NAME,
               '' ODM_OBJECT_PN,
               '' CUSTOMER_OBJECT_PN,
               SPC.FORMAT DATA_FORMAT,
               '' FIXED_VALUE,
               '' SERIALIZED,
               1 NEED_ASSEMBLY_QTY,
               0 COLLECTED_QTY,
               '' REWORK_REMOVE_FLAG,
               '' EDI_FLAG,
               1 OBJECT_MODE,
               SP1.MEANING OBJECT_MODE_DESCRIPTION,
               1 OPERATION_ORDER,
               0 REMAINING_QTY,
               '' ATTRIBUTE1,
               '' ATTRIBUTE2,
               '' ATTRIBUTE3,
               '' ATTRIBUTE4,
               '' ATTRIBUTE5
          FROM SFCS_PRODUCT_CARTON SPC,
               SFCS_ROUTE_CONFIG SRC,
               SFCS_ALL_OBJECTS SAO,
               SFCS_PARAMETERS SP1
         WHERE     SPC.PART_NO = :PART_NO
               AND SRC.PRODUCT_OPERATION_CODE = :PRODUCT_OPERATION_CODE
               AND SRC.CURRENT_OPERATION_ID = SPC.COLLECT_OPERATION_ID
               AND 674 = SAO.ID
               AND SPC.ENABLED = 'Y'
               AND SP1.LOOKUP_TYPE = 'OBJECT_OPERATION_MODE'
               AND SP1.LOOKUP_CODE = '1'
        UNION ALL
        SELECT 675 OPERATION_OBJECT_ID,
               SPP.ID PRODUCT_OBJECT_ID,
               :PART_NO PART_NO,
               SRC.PRODUCT_OPERATION_CODE,
               675 OBJECT_ID,
               SAO.OBJECT_NAME,
               '' ODM_OBJECT_PN,
               '' CUSTOMER_OBJECT_PN,
               SPP.FORMAT DATA_FORMAT,
               '' FIXED_VALUE,
               '' SERIALIZED,
               1 NEED_ASSEMBLY_QTY,
               0 COLLECTED_QTY,
               '' REWORK_REMOVE_FLAG,
               '' EDI_FLAG,
               1 OBJECT_MODE,
               SP1.MEANING OBJECT_MODE_DESCRIPTION,
               1 OPERATION_ORDER,
               0 REMAINING_QTY,
               '' ATTRIBUTE1,
               '' ATTRIBUTE2,
               '' ATTRIBUTE3,
               '' ATTRIBUTE4,
               '' ATTRIBUTE5
          FROM SFCS_PRODUCT_PALLET SPP,
               SFCS_ROUTE_CONFIG SRC,
               SFCS_ALL_OBJECTS SAO,
               SFCS_PARAMETERS SP1
         WHERE     SPP.PART_NO = :PART_NO
               AND SRC.PRODUCT_OPERATION_CODE = :PRODUCT_OPERATION_CODE
               AND SRC.CURRENT_OPERATION_ID = SPP.COLLECT_OPERATION_ID
               AND 675 = SAO.ID
               AND SPP.ENABLED = 'Y'
               AND SP1.LOOKUP_TYPE = 'OBJECT_OPERATION_MODE'
               AND SP1.LOOKUP_CODE = '1'";
            SfcsRouteConfig currentRouteconfig = propertyprovider.route.sfcsRouteConfigs.Where(
                f => f.CURRENT_OPERATION_ID == propertyprovider.sfcsOperationSites.OPERATION_ID
                ).FirstOrDefault();
            List<CollectDataView> collectDataViews = _sfcsRuncardRepository.QueryEx<CollectDataView>(
                getCollectDataView,
                new
                {
                    PART_NO = propertyprovider.product.partNumber,
                    PRODUCT_OPERATION_CODE = currentRouteconfig.PRODUCT_OPERATION_CODE
                }
                );
            if (collectDataViews != null && collectDataViews.Count > 0)
            {
                propertyprovider.collectDataViews = collectDataViews;
            }
        }

        /// <summary>
        /// 保存SN数据
        /// </summary>
        /// <param name ="operaionID"></param>
        /// <param name="propertyprovider"></param>
        ///<param name ="transaction"></param>
        private async Task StoreSnDataAsync(Propertyprovider propertyprovider, IDbTransaction transaction)
        {

            SfcsRouteConfig currentSfcsRouteConfig = propertyprovider.route.sfcsRouteConfigs.Where<SfcsRouteConfig>(f => f.CURRENT_OPERATION_ID == propertyprovider.sfcsOperationSites.OPERATION_ID).FirstOrDefault();
            if (currentSfcsRouteConfig == null)
            {
                throw new Exception("站点对应的工序不在此制程中");
            }
            //NoneRepair:748
            if (currentSfcsRouteConfig.REPAIR_OPERATION_ID == GlobalVariables.NoneRepair && propertyprovider.defects != null
                && propertyprovider.defects.Count > 0)
            {
                propertyprovider.defects = null;
                throw new Exception("当前工位不允许刷不良!");
            }

            //抽检控制
            // this.SpotCheck(operaionID, propertyprovider, transaction);

            if (!propertyprovider.product.runcardExist)
            {
                //流水号第一次投入，新增流水号
                InsertRuncard(propertyprovider, transaction);
            }
            else
            {
                //修改流水号
                //主板廠，產品fail時，要系統自動刪除卡通號
                //if (this.WorkOrderRow.PLANT_CODE == GlobalVariables.pcbCode && this.defectExist)
                //{
                //    this.ClearCartonAndPalletWhenDefectExist();
                //}
                this.UpdateRuncard(propertyprovider, transaction);
            }

            //this.RevisionControl();
            this.RuncardTransferThisStation(propertyprovider, transaction);
            this.InsertOperationHistory(propertyprovider, propertyprovider.OperationId, transaction);
            this.UpdateWorkOrderInputAndRanger(propertyprovider, transaction);
            //this.InsertDelayDefect(propertyprovider, transaction);
            //采集数据保存
            this.StoreComponentList(propertyprovider.OperationId, propertyprovider, transaction);
            this.StoreResourceList(propertyprovider.OperationId, propertyprovider, transaction);
            this.StoreUidList(propertyprovider.OperationId, propertyprovider, transaction);
            this.StoreCartonList(propertyprovider, transaction);
            this.StorePalletList(propertyprovider, transaction);
            //过站后校验处理
            //执行校验的JOb（Finally RUN JOB）
            KeyValuePair<Boolean, String> finallyJobResult = await JobDirector<SfcsRuncard, Decimal>.ExecuteFinallyRunJobAsync(propertyprovider, _sfcsRuncardRepository, transaction);
            if (!finallyJobResult.Key)
            {
                throw new Exception(finallyJobResult.Value);
            }
        }
        /// <summary>
        /// 保持不良信息
        /// </summary>
        /// <param name="operaionID"></param>
        /// <param name="propertyprovider"></param>
        /// <param name="transaction"></param>
        private void StoreDefectData(Propertyprovider propertyprovider, IDbTransaction transaction)
        {
            if (propertyprovider.defects == null || propertyprovider.defects.Count <= 0)
            {
                return;
            }
            string I_InsertCollectDefects = @"INSERT INTO SFCS_COLLECT_DEFECTS (COLLECT_DEFECT_ID,COLLECT_DEFECT_DETAIL_ID,DEFECT_CODE,CUSTOMER_DEFECT_CODE,SN,
                                                       WO_ID,SN_ID,DEFECT_OPERATION_ID,DEFECT_SITE_ID,DEFECT_OPERATOR,DEFECT_TIME,REPAIR_FLAG)
                                                       VALUES (:COLLECT_DEFECT_ID,:COLLECT_DEFECT_DETAIL_ID,:DEFECT_CODE,:CUSTOMER_DEFECT_CODE,:SN,:WO_ID,:SN_ID,
                                                       :DEFECT_OPERATION_ID,:DEFECT_SITE_ID,:DEFECT_OPERATOR,:DEFECT_TIME,'N') ";
            string I_InsertDefectDetails = @"INSERT INTO SFCS_COLLECT_DEFECTS_DETAIL (COLLECT_DEFECT_DETAIL_ID,OPERATION_ID,ORDER_NO,DEFECT_DETAIL,WO_ID,
                                                      SN_ID,SN,OPERATION_SITE_ID,OPERATOR,COLLECT_TIME)
                                                      VALUES(:COLLECT_DEFECT_DETAIL_ID,:OPERATION_ID,:ORDER_NO,:DEFECT_DETAIL,:WO_ID,
                                                      :SN_ID,:SN,:OPERATION_SITE_ID,:OPERATOR,SYSDATE)";
            foreach (Defect defect in propertyprovider.defects)
            {
                Decimal defectId = _sfcsRuncardRepository.QueryEx<decimal>(
                    "SELECT SFCS_COLLECT_DEFECTS_SEQ.NEXTVAL FROM DUAL").FirstOrDefault();
                //存儲Defect Detail
                decimal? defectDetailSeq = null;
                if (defect.defectDetailList != null && defect.defectDetailList.Count > 0)
                {
                    defectDetailSeq = _sfcsRuncardRepository.QueryEx<decimal>(
                    "SELECT SFCS_DEFECTS_DETAIL_SEQ.NEXTVAL FROM DUAL").FirstOrDefault();

                    decimal orderNo = 1;
                    foreach (string msg in defect.defectDetailList)
                    {
                        _sfcsRuncardRepository.Execute(I_InsertDefectDetails,
                            new
                            {
                                COLLECT_DEFECT_DETAIL_ID = defectDetailSeq,
                                OPERATION_ID = propertyprovider.OperationId,
                                ORDER_NO = orderNo,
                                DEFECT_DETAIL = msg,
                                WO_ID = propertyprovider.product.workOrderId,
                                SN_ID = propertyprovider.sfcsRuncard.ID,
                                SN = propertyprovider.sfcsRuncard.SN,
                                OPERATION_SITE_ID = propertyprovider.sfcsOperationSites.ID,
                                OPERATOR = propertyprovider.sys_Manager.USER_NAME
                            }, transaction);
                        orderNo++;
                    }
                }
                _sfcsRuncardRepository.Execute(I_InsertCollectDefects,
                    new
                    {
                        COLLECT_DEFECT_ID = defectId,
                        COLLECT_DEFECT_DETAIL_ID = defectDetailSeq,
                        DEFECT_CODE = defect.collectDataList.FirstOrDefault().Data,
                        CUSTOMER_DEFECT_CODE = "",
                        SN = propertyprovider.sfcsRuncard.SN,
                        WO_ID = propertyprovider.product.workOrderId,
                        SN_ID = propertyprovider.sfcsRuncard.ID,
                        DEFECT_OPERATION_ID = propertyprovider.sfcsOperationSites.OPERATION_ID,
                        DEFECT_SITE_ID = propertyprovider.sfcsOperationSites.ID,
                        DEFECT_OPERATOR = defect.collectDataList.FirstOrDefault().CollectBy,
                        DEFECT_TIME = defect.collectDataList.FirstOrDefault().CollectTime
                    });
            }
        }

        /// <summary>
        /// 更新工單和范圍信息 
        /// </summary>
        /// <param name="propertyprovider"></param>
        /// <param name="transaction"></param>
        private void UpdateWorkOrderInputAndRanger(Propertyprovider propertyprovider, IDbTransaction transaction)
        {
            if (!propertyprovider.product.runcardExist || propertyprovider.sfcsOperationSites.OPERATION_ID == GlobalVariables.TUOperation)
            {
                if (propertyprovider.product.sfcswo.INPUT_QTY + 1 > propertyprovider.product.sfcswo.TARGET_QTY)
                {
                    throw new Exception(String.Format("工单{0}已经刷满", propertyprovider.product.workOrder));
                }
                string U_UpdateWorkOrderStartDate = @"UPDATE SFCS_WO SET ACTUAL_START_DATE=SYSDATE WHERE ID=:ID";
                string U_UpdateWorkOrderStatus = @"UPDATE SFCS_WO SET WO_STATUS=:WO_STATUS WHERE ID=:ID";
                string U_UpdateWorkOrderInput = @"UPDATE SFCS_WO SET INPUT_QTY=INPUT_QTY+1 WHERE ID=:ID";
                string U_UpdateRuncardRangerStatus = @"UPDATE SFCS_RUNCARD_RANGER SET STATUS=:STATUS WHERE ID=:ID";
                if (propertyprovider.product.sfcswo.INPUT_QTY == 0)
                {
                    _sfcsRuncardRepository.Execute(U_UpdateWorkOrderStartDate,
                        new
                        {
                            ID = propertyprovider.product.workOrderId
                        }, transaction);
                    _sfcsRuncardRepository.Execute(U_UpdateWorkOrderStatus,
                        new
                        {
                            ID = propertyprovider.product.workOrderId,
                            WO_STATUS = GlobalVariables.WorkOrderInputStatus
                        }, transaction);
                }
                if (propertyprovider.product.sfcswo.INPUT_QTY + 1 == propertyprovider.product.sfcswo.TARGET_QTY)
                {
                    _sfcsRuncardRepository.Execute(U_UpdateWorkOrderStatus,
                        new
                        {
                            ID = propertyprovider.product.workOrderId,
                            WO_STATUS = GlobalVariables.WorkOrderCommitdStatus
                        }, transaction);
                }
                _sfcsRuncardRepository.Execute(U_UpdateWorkOrderInput,
                        new
                        {
                            ID = propertyprovider.product.workOrderId
                        }, transaction);
                if (propertyprovider.product.rangerStatus == 1)
                {
                    _sfcsRuncardRepository.Execute(U_UpdateRuncardRangerStatus,
                        new
                        {
                            ID = propertyprovider.product.rangerId,
                            STATUS = GlobalVariables.RangerInputStatus
                        }, transaction);
                }
            }

            int qty = _sfcsRuncardRepository.QueryEx<int>("SELECT COUNT(0) FROM SFCS_RUNCARD WHERE SN = :SN AND WIP_OPERATION = :OPERATION AND LAST_OPERATION =:OPERATION ", new { SN = propertyprovider.sfcsRuncard.SN, OPERATION = GlobalVariables.EndOperation }, transaction).FirstOrDefault();
            if (qty > 0)
            {
                string U_UpdateWO = @"UPDATE SFCS_WO SET OUTPUT_QTY=OUTPUT_QTY+1 WHERE ID=:ID";
                if (propertyprovider.product.sfcswo.INPUT_QTY + 1 == propertyprovider.product.sfcswo.TARGET_QTY)
                {
                    U_UpdateWO = @"UPDATE SFCS_WO SET OUTPUT_QTY=OUTPUT_QTY+1,ACTUAL_DUE_DATE = SYSDATE,WO_STATUS = '" + GlobalVariables.WorkOrderCommitdStatus + "' WHERE ID=:ID";
                }
                _sfcsRuncardRepository.Execute(U_UpdateWO, new { ID = propertyprovider.product.workOrderId }, transaction);

                String plan_date = _sfcsRuncardRepository.QueryEx<String>("SELECT TO_CHAR(SYSDATE,'YYYY-MM-DD') MITAC_DATE FROM DUAL").FirstOrDefault(); ;
                String pid = _sfcsRuncardRepository.QueryEx<String>("SELECT ID FROM SMT_PRODUCE_PLAN WHERE LINE_ID=:LINE_ID AND WO_NO=:WO_NO AND TO_CHAR(PLAN_DATE,'yyyy-MM-dd')=:PLAN_DATE",
                    new
                    {
                        LINE_ID = propertyprovider.sfcsOperationLines.Id,
                        WO_NO = propertyprovider.product.sfcswo.WO_NO,
                        PLAN_DATE = plan_date
                    })?.FirstOrDefault();
                if (!String.IsNullOrEmpty(pid))
                {
                    _sfcsRuncardRepository.Execute("UPDATE SMT_PRODUCE_PLAN SET OUTPUT_QTY=OUTPUT_QTY+1 WHERE ID=:ID", new { ID = pid }, transaction);
                }
            }
        }
        /// <summary>
        /// 記錄作業歷史記錄
        /// </summary>
        private void InsertOperationHistory(Propertyprovider propertyprovider, Decimal operaionID, IDbTransaction transaction)
        {
            string I_InsertOperationHistory = @"INSERT INTO SFCS_OPERATION_HISTORY(SN_ID,OPERATION_ID,WO_ID,ROUTE_ID,SITE_OPERATION_ID,OPERATION_SITE_ID,
                                                         OPERATOR,OPERATION_STATUS,OPERATION_TIME,VISIT_NUMBER) VALUES(:SN_ID,:OPERATION_ID,:WO_ID,
                                                         :ROUTE_ID,:SITE_OPERATION_ID,:OPERATION_SITE_ID,:OPERATOR,:OPERATION_STATUS,SYSDATE,:VISIT_NUMBER) ";
            if (propertyprovider.defects == null || propertyprovider.defects.Count <= 0)
            {
                _sfcsRuncardRepository.Execute(I_InsertOperationHistory, new
                {
                    SN_ID = propertyprovider.sfcsRuncard.ID,
                    OPERATION_ID = operaionID,
                    WO_ID = propertyprovider.product.workOrderId,
                    ROUTE_ID = propertyprovider.route.routeID,
                    SITE_OPERATION_ID = propertyprovider.sfcsOperationSites.OPERATION_ID,
                    OPERATION_SITE_ID = propertyprovider.sfcsOperationSites.ID,
                    OPERATOR = propertyprovider.sys_Manager.USER_NAME,
                    OPERATION_STATUS = GlobalVariables.Pass,
                    VISIT_NUMBER = 0
                });
            }
            else
            {
                decimal result = propertyprovider.sfcsOperationSites.OPERATION_ID == GlobalVariables.TUOperation ?
                    GlobalVariables.Pass : GlobalVariables.Fail;

                _sfcsRuncardRepository.Execute(I_InsertOperationHistory, new
                {
                    SN_ID = propertyprovider.sfcsRuncard.ID,
                    OPERATION_ID = operaionID,
                    WO_ID = propertyprovider.product.workOrderId,
                    ROUTE_ID = propertyprovider.route.routeID,
                    SITE_OPERATION_ID = propertyprovider.sfcsOperationSites.OPERATION_ID,
                    OPERATION_SITE_ID = propertyprovider.sfcsOperationSites.ID,
                    OPERATOR = propertyprovider.sys_Manager.USER_NAME,
                    OPERATION_STATUS = result,
                    VISIT_NUMBER = 0
                });
            }
        }

        /// <summary>
        /// 更新Runcard
        /// </summary>
        /// <param name="propertyprovider"></param>
        /// <param name="transaction"></param>
        private void UpdateRuncard(Propertyprovider propertyprovider, IDbTransaction transaction)
        {
            this.CheckRuncardRework(propertyprovider, transaction);

            SfcsRouteConfig currentSfcsRouteConfig = propertyprovider.route.sfcsRouteConfigs.
                Where(f => f.CURRENT_OPERATION_ID == propertyprovider.sfcsOperationSites.OPERATION_ID)
                .FirstOrDefault();
            decimal nextOperation = currentSfcsRouteConfig.NEXT_OPERATION_ID;
            decimal repairOperation = currentSfcsRouteConfig.REPAIR_OPERATION_ID;
            string U_UpdateRuncardRoute = @"UPDATE SFCS_RUNCARD SET WO_ID = :WO_ID,ROUTE_ID = :ROUTE_ID, CURRENT_SITE = :CURRENT_SITE,WIP_OPERATION = :WIP_OPERATION,
            LAST_OPERATION = :LAST_OPERATION,STATUS = :STATUS,OPERATION_TIME = SYSDATE WHERE SN = :SN ";
            if (propertyprovider.defects == null
                || propertyprovider.defects.Count <= 0)
            {

                _sfcsRuncardRepository.Execute(U_UpdateRuncardRoute, new
                {
                    WO_ID = propertyprovider.sfcsRuncard.WO_ID,
                    ROUTE_ID = propertyprovider.sfcsRuncard.ROUTE_ID,
                    CURRENT_SITE = propertyprovider.sfcsOperationSites.ID,
                    WIP_OPERATION = nextOperation,
                    LAST_OPERATION = propertyprovider.product.reworkFlag ?
                    propertyprovider.sfcsRuncard.LAST_OPERATION : nextOperation,
                    STATUS = GlobalVariables.Pass,
                    SN = propertyprovider.sfcsRuncard.SN
                }, transaction);
            }
            else
            {
                decimal result = propertyprovider.sfcsOperationSites.OPERATION_ID == GlobalVariables.TUOperation ?
                                    GlobalVariables.Pass : GlobalVariables.Fail;
                decimal wipRouteCode = propertyprovider.sfcsOperationSites.OPERATION_ID == GlobalVariables.TUOperation ?
                    nextOperation : repairOperation;
                _sfcsRuncardRepository.Execute(U_UpdateRuncardRoute, new
                {
                    WO_ID = propertyprovider.sfcsRuncard.WO_ID,
                    ROUTE_ID = propertyprovider.sfcsRuncard.ROUTE_ID,
                    CURRENT_SITE = propertyprovider.sfcsOperationSites.ID,
                    WIP_OPERATION = wipRouteCode,
                    LAST_OPERATION = propertyprovider.product.reworkFlag ?
                    propertyprovider.sfcsRuncard.LAST_OPERATION : nextOperation,
                    STATUS = GlobalVariables.Fail,
                    SN = propertyprovider.sfcsRuncard.SN
                }, transaction);
            }
        }
        /// <summary>
        /// 檢查流水號是否為返工
        /// </summary>
        /// <param name="propertyprovider"></param>
        /// <param name="transaction"></param>
        private void CheckRuncardRework(Propertyprovider propertyprovider, IDbTransaction transaction)
        {
            String IncRuncardLastCounterSql = @"UPDATE SFCS_RUNCARD SET LAST_OPERATION_COUNTER=NVL(LAST_OPERATION_COUNTER,0)+1 WHERE SN=:SN";
            String ClearRuncardLastCounterSql = @"UPDATE SFCS_RUNCARD SET LAST_OPERATION_COUNTER=0 WHERE SN=:SN";
            if (propertyprovider.sfcsRuncard.LAST_OPERATION == GlobalVariables.EndOperation)
            {
                propertyprovider.product.reworkFlag = true;

                this._sfcsRuncardRepository.Execute(IncRuncardLastCounterSql,
                    new
                    {
                        SN = propertyprovider.sfcsRuncard.SN
                    }, transaction);
                return;
            }
            decimal nextOperation = propertyprovider.route.sfcsRouteConfigs.
                Where(f => f.CURRENT_OPERATION_ID == propertyprovider.sfcsOperationSites.OPERATION_ID)
                .FirstOrDefault().NEXT_OPERATION_ID;
            if (nextOperation == GlobalVariables.EndOperation)
            {
                propertyprovider.product.reworkFlag = false;

                this._sfcsRuncardRepository.Execute(ClearRuncardLastCounterSql,
                    new
                    {
                        SN = propertyprovider.sfcsRuncard.SN
                    }, transaction);
                return;
            }
            SfcsRouteConfig lastRouteConfig = propertyprovider.route.sfcsRouteConfigs.
                Where(f => f.CURRENT_OPERATION_ID == propertyprovider.sfcsRuncard.LAST_OPERATION).FirstOrDefault();
            SfcsRouteConfig nextRouteConfig = propertyprovider.route.sfcsRouteConfigs.
                Where(f => f.CURRENT_OPERATION_ID == nextOperation).FirstOrDefault();

            if (lastRouteConfig.ORDER_NO >= nextRouteConfig.ORDER_NO)
            {
                propertyprovider.product.reworkFlag = true;
                this._sfcsRuncardRepository.Execute(IncRuncardLastCounterSql,
                    new
                    {
                        SN = propertyprovider.sfcsRuncard.SN
                    }, transaction);
            }
            else
            {
                propertyprovider.product.reworkFlag = false;
                this._sfcsRuncardRepository.Execute(ClearRuncardLastCounterSql,
                    new
                    {
                        SN = propertyprovider.sfcsRuncard.SN
                    }, transaction);
            }
        }

        /// <summary>
		/// Runcard通过此站点
		/// </summary>
		private void RuncardTransferThisStation(Propertyprovider propertyprovider, IDbTransaction transaction)
        {
            if (!propertyprovider.product.runcardExist)
            {
                if (propertyprovider.defects == null
                || propertyprovider.defects.Count <= 0)
                {
                    this.Pass(propertyprovider, transaction);
                }
                else
                {
                    this.Fail(propertyprovider, transaction);
                }
            }
            else
            {
                if (propertyprovider.defects == null
                || propertyprovider.defects.Count <= 0)
                {
                    if (propertyprovider.product.reworkFlag)
                    {
                        this.RePass(propertyprovider, transaction);
                    }
                    else
                    {
                        this.Pass(propertyprovider, transaction);
                    }
                }
                else
                {
                    if (propertyprovider.sfcsOperationSites.OPERATION_ID == GlobalVariables.TUOperation)
                    {
                        if (propertyprovider.product.reworkFlag)
                        {
                            this.RePass(propertyprovider, transaction);
                        }
                        else
                        {
                            this.Pass(propertyprovider, transaction);
                        }
                    }
                    else
                    {
                        if (propertyprovider.product.reworkFlag)
                        {
                            this.ReFail(propertyprovider, transaction);
                        }
                        else
                        {
                            this.Fail(propertyprovider, transaction);
                        }
                    }
                }
            }
        }

        /// <summary>
        /// 新增产品条码信息
        /// </summary>
        private void InsertRuncard(Propertyprovider propertyprovider, IDbTransaction transaction)
        {
            String insertSnSql = @"INSERT INTO SFCS_RUNCARD(ID, SN, WO_ID, ROUTE_ID, CURRENT_SITE, WIP_OPERATION,
                                                LAST_OPERATION, LAST_OPERATION_COUNTER, STATUS, INPUT_TIME, OPERATION_TIME)
                                                VALUES(:ID, :SN, :WO_ID, :ROUTE_ID, :CURRENT_SITE, :WIP_OPERATION,
                                               :LAST_OPERATION, :LAST_OPERATION_COUNTER, :STATUS, SYSDATE, SYSDATE)";
            SfcsRouteConfig currentRoueConfig = propertyprovider.route.sfcsRouteConfigs.Where(
                f => f.CURRENT_OPERATION_ID == propertyprovider.sfcsOperationSites.OPERATION_ID).FirstOrDefault();
            if (propertyprovider.defects == null
                || propertyprovider.defects.Count <= 0)
            {
                //Pass:1
                _sfcsRuncardRepository.Execute(insertSnSql, new
                {
                    ID = propertyprovider.sfcsRuncard.ID,
                    SN = propertyprovider.sfcsRuncard.SN,
                    WO_ID = propertyprovider.product.workOrderId,
                    ROUTE_ID = propertyprovider.route.routeID,
                    CURRENT_SITE = propertyprovider.sfcsOperationSites.ID,
                    WIP_OPERATION = currentRoueConfig.NEXT_OPERATION_ID,
                    LAST_OPERATION = propertyprovider.sfcsRuncard.LAST_OPERATION,
                    LAST_OPERATION_COUNTER = 0,
                    STATUS = GlobalVariables.Pass

                }, transaction); ;
            }
            else
            {
                //Fail:2
                _sfcsRuncardRepository.Execute(insertSnSql, new
                {
                    ID = propertyprovider.sfcsRuncard.ID,
                    SN = propertyprovider.sfcsRuncard.SN,
                    WO_ID = propertyprovider.product.workOrderId,
                    ROUTE_ID = propertyprovider.route.routeID,
                    CURRENT_SITE = propertyprovider.sfcsOperationSites.ID,
                    WIP_OPERATION = currentRoueConfig.REPAIR_OPERATION_ID,
                    LAST_OPERATION = propertyprovider.sfcsRuncard.LAST_OPERATION,
                    LAST_OPERATION_COUNTER = 0,
                    STATUS = GlobalVariables.Fail
                }, transaction);
            }
        }

        /// <summary>
		/// 抽檢
		/// </summary>
		private void SpotCheck(Decimal operaionID, Propertyprovider propertyprovider, IDbTransaction transaction)
        {
            if (propertyprovider.product != null && propertyprovider.product.runcardExist)
            {
                string message = null;
                string ratio = null;
                decimal deliverCount;
                decimal needSpotCheck;
                SfcsParameters sfcsParameters = _sfcsRuncardRepository.QueryEx<SfcsParameters>("SELECT * FROM SFCS_PARAMETERS WHERE LOOKUP_TYPE = :LOOKUP_TYPE AND LOOKUP_CODE = :LOOKUP_CODE",
                    new
                    {
                        LOOKUP_TYPE = "RUNCARD_STATUS",
                        LOOKUP_CODE = propertyprovider.sfcsRuncard.STATUS
                    }).FirstOrDefault();
                decimal result = _sfcsRuncardRepository.SpotCheckControl(propertyprovider.sfcsRuncard.SN, sfcsParameters.DESCRIPTION.ToUpper(),
                    operaionID, propertyprovider.sfcsOperationSites.ID, out message, out ratio, out deliverCount, out needSpotCheck, transaction);
                if (result == 1)
                {
                    throw new Exception(message);
                }
                if (needSpotCheck == 0)
                {
                    SpotCheck spotCheck = new SpotCheck();
                    spotCheck.needSpot = true;
                    spotCheck.ratio = ratio;
                    spotCheck.deliverCount = deliverCount;
                    propertyprovider.spotCheck = spotCheck;
                    propertyprovider.msg = message;
                }
                else
                {
                    if (propertyprovider.spotCheck != null)
                    {
                        propertyprovider.spotCheck.needSpot = false;
                        propertyprovider.spotCheck.ratio = ratio;
                        propertyprovider.spotCheck.deliverCount = deliverCount;
                        propertyprovider.msg = message;
                    }
                }
            }
        }

        /// <summary>
        /// Pass該站點
		/// 1. 当前站点的output加一
		/// 2. 当前站的下一站input加一
		/// 3. 如果当前站是第一站，则当前站的input加一 
        /// </summary>
        /// <param name="propertyprovider"></param>
        /// <param name="transaction"></param>
        private void Pass(Propertyprovider propertyprovider, IDbTransaction transaction)
        {
            SfcsRouteConfig currentSfcsRouteConfig = propertyprovider.route.sfcsRouteConfigs.
                Where(f => f.CURRENT_OPERATION_ID == propertyprovider.sfcsOperationSites.OPERATION_ID).FirstOrDefault();

            this.SiteStatistics(propertyprovider, (Decimal)propertyprovider.product.workOrderId,
                ThroughSiteType.Pass, transaction);
            this.InputOutputStatistics(propertyprovider, (Decimal)propertyprovider.product.workOrderId,
             (Decimal)propertyprovider.sfcsOperationSites.OPERATION_ID, IOType.Output, transaction);
            this.InputOutputStatistics(propertyprovider, (Decimal)propertyprovider.product.workOrderId,
                currentSfcsRouteConfig.NEXT_OPERATION_ID, IOType.Input, transaction);
            if (!propertyprovider.product.runcardExist)
            {
                this.InputOutputStatistics(propertyprovider, (Decimal)propertyprovider.product.workOrderId,
                   (Decimal)propertyprovider.sfcsOperationSites.OPERATION_ID, IOType.Input, transaction);
            }
        }
        /// <summary>
        /// Fail該站點
		/// 1. 如果当前站是第一站，则当前站的input加一 
        /// </summary>
        /// <param name="propertyprovider"></param>
        /// <param name="transaction"></param>
        private void Fail(Propertyprovider propertyprovider, IDbTransaction transaction)
        {
            this.SiteStatistics(propertyprovider, (Decimal)propertyprovider.product.workOrderId,
               ThroughSiteType.Fail, transaction);
            if (!propertyprovider.product.runcardExist)
            {
                this.InputOutputStatistics(propertyprovider, (Decimal)propertyprovider.product.workOrderId,
                                  (Decimal)propertyprovider.sfcsOperationSites.OPERATION_ID, IOType.Input, transaction);
            }
        }
        /// <summary>
        /// RePass该站点
		/// 1. 如果回到了Fail的站点时，当前站点的output加一
		/// 2. 当前站的下一站input加一 
        /// </summary>
        /// <param name="propertyprovider"></param>
        /// <param name="transaction"></param>
        private void RePass(Propertyprovider propertyprovider, IDbTransaction transaction)
        {
            this.SiteStatistics(propertyprovider, (Decimal)propertyprovider.product.workOrderId,
               ThroughSiteType.RePass, transaction);
            Decimal lastOperation = (Decimal)propertyprovider.sfcsRuncard.LAST_OPERATION;
            SfcsRouteConfig currentSfcsRouteConfig = propertyprovider.route.sfcsRouteConfigs.
                Where(f => f.CURRENT_OPERATION_ID == propertyprovider.sfcsOperationSites.OPERATION_ID).FirstOrDefault();

            Decimal nextOperation = currentSfcsRouteConfig.NEXT_OPERATION_ID;
            if (lastOperation == nextOperation)
            {
                String S_SelectOperationHistory = @"SELECT *FROM SFCS_OPERATION_HISTORY WHERE SN_ID = :SN_ID
AND WO_ID = :WO_ID AND SITE_OPERATION_ID = :SITE_OPERATION_ID AND OPERATION_STATUS = :OPERATION_STATUS";
                List<SfcsOperationHistory> sfcsOperationHistorys = _sfcsRuncardRepository.QueryEx<SfcsOperationHistory>(
                    S_SelectOperationHistory,
                    new
                    {
                        SN_ID = propertyprovider.sfcsRuncard.ID,
                        WO_ID = propertyprovider.product.workOrderId,
                        SITE_OPERATION_ID = propertyprovider.sfcsOperationSites.OPERATION_ID,
                        OPERATION_STATUS = GlobalVariables.Pass
                    });
                if (sfcsOperationHistorys != null && sfcsOperationHistorys.Count > 0)
                {
                    return;
                }
                //無PASS,即Fail站點需要統計I/O
                this.InputOutputStatistics(propertyprovider, (Decimal)propertyprovider.product.workOrderId,
            (Decimal)propertyprovider.sfcsOperationSites.OPERATION_ID, IOType.Output, transaction);
                this.InputOutputStatistics(propertyprovider, (Decimal)propertyprovider.product.workOrderId,
                    currentSfcsRouteConfig.NEXT_OPERATION_ID, IOType.Input, transaction);
            }
        }
        /// <summary>
        /// ReFail该站点
		/// 1. output和input都保持不变
        /// </summary>
        /// <param name="propertyprovider"></param>
        /// <param name="transaction"></param>
        private void ReFail(Propertyprovider propertyprovider, IDbTransaction transaction)
        {
            this.SiteStatistics(propertyprovider, (Decimal)propertyprovider.product.workOrderId,
               ThroughSiteType.ReFail, transaction);
        }
        /// <summary>
        /// 站点统计
        /// </summary>
        private void SiteStatistics(Propertyprovider propertyprovider, decimal wo_id, ThroughSiteType throughSiteType, IDbTransaction transaction)
        {

            DateTime currentTime = _sfcsRuncardRepository.QueryEx<DateTime>("SELECT SYSDATE FROM DUAL").FirstOrDefault();
            string timeString = string.Format("{0:yyyy/MM/dd HH:00:00}", currentTime);
            DateTime WorkTime = DateTime.Parse(timeString);

            String S_SiteStatistics = @"SELECT SI.*FROM SFCS_SITE_STATISTICS SI WHERE WO_ID = :WO_ID AND OPERATION_SITE_ID = :OPERATION_SITE_ID AND WORK_TIME = :WORK_TIME";
            String I_SiteStatistics = @"INSERT INTO SFCS_SITE_STATISTICS(WO_ID,OPERATION_SITE_ID,WORK_TIME,PASS,FAIL,REPASS,REFAIL)
                                                       VALUES(:WO_ID,:OPERATION_SITE_ID,:WORK_TIME,:PASS,:FAIL,:REPASS,:REFAIL)";
            String U_UpdatePassSiteStatistics = @"UPDATE SFCS_SITE_STATISTICS SET PASS=PASS+1 WHERE WO_ID=:WO_ID AND OPERATION_SITE_ID=:OPERATION_SITE_ID AND WORK_TIME=:WORK_TIME";
            String U_UpdateFailSiteStatistics = @"UPDATE SFCS_SITE_STATISTICS SET FAIL=FAIL+1 WHERE WO_ID=:WO_ID AND OPERATION_SITE_ID=:OPERATION_SITE_ID AND WORK_TIME=:WORK_TIME";
            String U_UpdateRepassSiteStatistics = @"UPDATE SFCS_SITE_STATISTICS SET REPASS=REPASS+1 WHERE WO_ID=:WO_ID AND OPERATION_SITE_ID=:OPERATION_SITE_ID AND WORK_TIME=:WORK_TIME";
            String U_UpdateRefailSiteStatistics = @"UPDATE SFCS_SITE_STATISTICS SET REFAIL=REFAIL+1 WHERE WO_ID=:WO_ID AND OPERATION_SITE_ID=:OPERATION_SITE_ID AND WORK_TIME=:WORK_TIME";
            List<SfcsSiteStatistics> sfcsSiteStatistics = _sfcsRuncardRepository.QueryEx<SfcsSiteStatistics>(
                S_SiteStatistics,
                new
                {
                    WO_ID = wo_id,
                    OPERATION_SITE_ID = propertyprovider.sfcsOperationSites.ID,
                    WORK_TIME = WorkTime
                }
                );
            switch (throughSiteType)
            {
                case ThroughSiteType.Pass:
                    if (sfcsSiteStatistics == null || sfcsSiteStatistics.Count <= 0)
                    {
                        _sfcsRuncardRepository.Execute(I_SiteStatistics,
                            new
                            {
                                WO_ID = wo_id,
                                OPERATION_SITE_ID = propertyprovider.sfcsOperationSites.ID,
                                WORK_TIME = WorkTime,
                                PASS = 1,
                                FAIL = 0,
                                REPASS = 0,
                                REFAIL = 0
                            }, transaction);
                    }
                    else
                    {
                        _sfcsRuncardRepository.Execute(U_UpdatePassSiteStatistics, new
                        {
                            WO_ID = wo_id,
                            OPERATION_SITE_ID = propertyprovider.sfcsOperationSites.ID,
                            WORK_TIME = WorkTime
                        });
                    }
                    break;
                case ThroughSiteType.Fail:
                    if (sfcsSiteStatistics == null || sfcsSiteStatistics.Count <= 0)
                    {
                        _sfcsRuncardRepository.Execute(I_SiteStatistics,
                            new
                            {
                                WO_ID = wo_id,
                                OPERATION_SITE_ID = propertyprovider.sfcsOperationSites.ID,
                                WORK_TIME = WorkTime,
                                PASS = 0,
                                FAIL = 1,
                                REPASS = 0,
                                REFAIL = 0
                            }, transaction);
                    }
                    else
                    {
                        _sfcsRuncardRepository.Execute(U_UpdateFailSiteStatistics, new
                        {
                            WO_ID = wo_id,
                            OPERATION_SITE_ID = propertyprovider.sfcsOperationSites.ID,
                            WORK_TIME = WorkTime
                        });
                    }
                    break;
                case ThroughSiteType.RePass:
                    if (sfcsSiteStatistics == null || sfcsSiteStatistics.Count <= 0)
                    {
                        _sfcsRuncardRepository.Execute(I_SiteStatistics,
                            new
                            {
                                WO_ID = wo_id,
                                OPERATION_SITE_ID = propertyprovider.sfcsOperationSites.ID,
                                WORK_TIME = WorkTime,
                                PASS = 0,
                                FAIL = 0,
                                REPASS = 1,
                                REFAIL = 0
                            }, transaction);
                    }
                    else
                    {
                        _sfcsRuncardRepository.Execute(U_UpdateRepassSiteStatistics, new
                        {
                            WO_ID = wo_id,
                            OPERATION_SITE_ID = propertyprovider.sfcsOperationSites.ID,
                            WORK_TIME = WorkTime
                        });
                    }
                    break;
                case ThroughSiteType.ReFail:
                    if (sfcsSiteStatistics == null || sfcsSiteStatistics.Count <= 0)
                    {
                        _sfcsRuncardRepository.Execute(I_SiteStatistics,
                            new
                            {
                                WO_ID = wo_id,
                                OPERATION_SITE_ID = propertyprovider.sfcsOperationSites.ID,
                                WORK_TIME = WorkTime,
                                PASS = 0,
                                FAIL = 0,
                                REPASS = 0,
                                REFAIL = 1
                            }, transaction);
                    }
                    else
                    {
                        _sfcsRuncardRepository.Execute(U_UpdateRefailSiteStatistics, new
                        {
                            WO_ID = wo_id,
                            OPERATION_SITE_ID = propertyprovider.sfcsOperationSites.ID,
                            WORK_TIME = WorkTime
                        });
                    }
                    break;
            }
        }
        /// <summary>
        /// 站点输入输出统计
        /// </summary>
        private void InputOutputStatistics(Propertyprovider propertyprovider, decimal wo_id, decimal operation_code, IOType ioType, IDbTransaction transaction)
        {
            DateTime currentTime = _sfcsRuncardRepository.QueryEx<DateTime>("SELECT SYSDATE FROM DUAL").FirstOrDefault();
            string timeString = string.Format("{0:yyyy/MM/dd HH:00:00}", currentTime);
            DateTime WorkTime = DateTime.Parse(timeString);
            char statisticsChar = (ioType == IOType.Input) ? 'I' : 'O';

            String S_SelectIOStatistics = @"SELECT SO.* FROM SFCS_IO_STATISTICS SO 
            WHERE OPERATION_SITE_ID = :OPERATION_SITE_ID 
            AND WO_ID = :WO_ID AND WORK_TIME = :WORK_TIME 
            AND OPERATION_ID = :OPERATION_ID AND IO_TYPE = :IO_TYPE";
            String I_InsertIOStatistics = @"INSERT INTO SFCS_IO_STATISTICS(OPERATION_SITE_ID,WO_ID,WORK_TIME,OPERATION_ID,IO_TYPE,QTY)
                                                     VALUES(:OPERATION_SITE_ID,:WO_ID,:WORK_TIME,:OPERATION_ID,:IO_TYPE,:QTY)";
            String U_UpdateIOStatistics = @"UPDATE SFCS_IO_STATISTICS SET QTY=QTY+1 WHERE WO_ID=:WO_ID AND OPERATION_SITE_ID=:OPERATION_SITE_ID AND WORK_TIME=:WORK_TIME AND OPERATION_ID=:OPERATION_ID AND IO_TYPE=:IO_TYPE";
            List<SfcsIoStatistics> sfcsIoStatistics = _sfcsRuncardRepository.QueryEx<SfcsIoStatistics>(S_SelectIOStatistics,
                new
                {
                    OPERATION_SITE_ID = propertyprovider.sfcsOperationSites.ID,
                    WO_ID = wo_id,
                    WORK_TIME = WorkTime,
                    OPERATION_ID = operation_code,
                    IO_TYPE = statisticsChar.ToString()
                });
            if (sfcsIoStatistics == null || sfcsIoStatistics.Count <= 0)
            {
                _sfcsRuncardRepository.Execute(I_InsertIOStatistics,
                    new
                    {
                        OPERATION_SITE_ID = propertyprovider.sfcsOperationSites.ID,
                        WO_ID = wo_id,
                        WORK_TIME = WorkTime,
                        OPERATION_ID = operation_code,
                        IO_TYPE = statisticsChar.ToString(),
                        QTY = 1
                    });
            }
            else
            {
                _sfcsRuncardRepository.Execute(U_UpdateIOStatistics,
                    new
                    {
                        OPERATION_SITE_ID = propertyprovider.sfcsOperationSites.ID,
                        WO_ID = wo_id,
                        WORK_TIME = WorkTime,
                        OPERATION_ID = operation_code,
                        IO_TYPE = statisticsChar.ToString()
                    });
            }
        }

        //校验SN
        private async Task<bool> VerifySnData(Propertyprovider propertyprovider, IDbTransaction trance)
        {
            //判断流水号在数据库是否存在
            if (this.IsRuncardExist(propertyprovider, trance))
            {
                return true;
            }
            else
            {
                // 流水号不存在，通过Runcard Ranger确定SN信息
                SfcsRuncardRanger sfcsRuncardRanger = this.FindWorkOrderRanger(propertyprovider.data);
                if (sfcsRuncardRanger != null)
                {
                    decimal wo_id = sfcsRuncardRanger.WO_ID;
                    Decimal rangerId = sfcsRuncardRanger.ID;
                    Decimal rangerStatus = sfcsRuncardRanger.STATUS;
                    //TU站转工单号
                    if (propertyprovider.sfcsOperationSites.OPERATION_ID == GlobalVariables.TUOperation
                    && !String.IsNullOrEmpty(propertyprovider.woNo))
                    {
                        SfcsWo sfcsWo = _sfcsRuncardRepository.QueryEx<SfcsWo>(
                            "SELECT * FROM  SFCS_WO WHERE WO_NO = :WO_NO",
                            new
                            {
                                WO_NO = propertyprovider.woNo
                            }
                            ).FirstOrDefault();
                        if (sfcsWo.ID != wo_id)
                        {
                            sfcsWo.ROUTE_ID = sfcsWo.ROUTE_ID > 0 ? sfcsWo.ROUTE_ID : _sfcsProductConfigRepository.GetRouteIdByPartNo(sfcsWo.PART_NO);
                            if (sfcsWo.ROUTE_ID <= 0)
                            {
                                throw new Exception(String.Format(String.Format("工单号{0}没有设置制作!", sfcsWo.WO_NO)));
                            }
                            wo_id = sfcsWo.ID;
                        }
                    }
                    Product product = InitializeProduction(wo_id);
                    //product.runcardExist = true;
                    propertyprovider.product = product;
                    propertyprovider.product.rangerId = rangerId;
                    propertyprovider.product.rangerStatus = rangerStatus;
                    String format = this.FindPartNumberConfig(product.sfcsProductConfigs, product.partNumber, "FORMAT").CONFIG_VALUE;
                    if (FormatChecker.FormatCheck(propertyprovider.data, format))
                    {
                        Route route = InitialRoute((Decimal)product.sfcswo.ROUTE_ID);
                        propertyprovider.route = route;
                        SfcsRuncard sfcsRuncard = new SfcsRuncard();
                        sfcsRuncard.ID = await _sfcsRuncardRepository.GetSEQID();
                        sfcsRuncard.SN = propertyprovider.data.ToUpper();//流水號強制大寫
                        sfcsRuncard.WO_ID = product.sfcswo.ID;
                        sfcsRuncard.ROUTE_ID = product.sfcswo.ROUTE_ID;
                        sfcsRuncard.CURRENT_SITE = propertyprovider.sfcsOperationSites.ID;
                        sfcsRuncard.WIP_OPERATION = route.sfcsRouteConfigs.FirstOrDefault().CURRENT_OPERATION_ID;
                        sfcsRuncard.LAST_OPERATION = route.sfcsRouteConfigs.FirstOrDefault().NEXT_OPERATION_ID;
                        propertyprovider.sfcsRuncard = sfcsRuncard;
                        return true;
                    }
                    else
                    {
                        throw new Exception(String.Format("流水号{0}格式不匹配", propertyprovider.data));
                    }
                }
                else
                {
                    var importSnlist = await _importRuncardSnRepository.GetListAsync(String.Format(" WHERE SN ='{0}' ", propertyprovider.data));
                    if (importSnlist != null && importSnlist.Count() > 0)
                    {
                        ImportRuncardSn importRuncardSn = importSnlist.FirstOrDefault();
                        var sfcswoList = await _sfcsWoRepository.GetListAsync(String.Format(" WHERE WO_NO ='{0}' ", importRuncardSn.WO_NO));
                        if (sfcswoList == null || sfcswoList.Count() <= 0)
                        {
                            throw new Exception(String.Format("系统找不到工单, 工单号:{0}", importRuncardSn.WO_NO));
                        }

                        decimal wo_id = sfcswoList.FirstOrDefault().ID;
                        //TU站转工单号
                        if (propertyprovider.sfcsOperationSites.OPERATION_ID == GlobalVariables.TUOperation
                        && !String.IsNullOrEmpty(propertyprovider.woNo))
                        {
                            SfcsWo sfcsWo = _sfcsRuncardRepository.QueryEx<SfcsWo>(
                                "SELECT * FROM  SFCS_WO WHERE WO_NO = :WO_NO",
                                new
                                {
                                    WO_NO = propertyprovider.woNo
                                }
                                ).FirstOrDefault();
                            if (sfcsWo.ID != wo_id)
                            {
                                sfcsWo.ROUTE_ID = sfcsWo.ROUTE_ID > 0 ? sfcsWo.ROUTE_ID : _sfcsProductConfigRepository.GetRouteIdByPartNo(sfcsWo.PART_NO);
                                if (sfcsWo.ROUTE_ID <= 0)
                                {
                                    throw new Exception(String.Format(String.Format("工单号{0}没有设置制作!", sfcsWo.WO_NO)));
                                }
                                wo_id = sfcsWo.ID;
                            }
                        }
                        Product product = InitializeProduction(wo_id);
                        propertyprovider.product = product;
                        String format = this.FindPartNumberConfig(product.sfcsProductConfigs, product.partNumber, "FORMAT").CONFIG_VALUE;
                        if (FormatChecker.FormatCheck(propertyprovider.data, format))
                        {
                            Route route = InitialRoute((Decimal)product.sfcswo.ROUTE_ID);
                            propertyprovider.route = route;
                            SfcsRuncard sfcsRuncard = new SfcsRuncard();
                            sfcsRuncard.ID = await _sfcsRuncardRepository.GetSEQID();
                            sfcsRuncard.SN = propertyprovider.data.ToUpper();//流水號強制大寫
                            sfcsRuncard.WO_ID = product.sfcswo.ID;
                            sfcsRuncard.ROUTE_ID = product.sfcswo.ROUTE_ID;
                            sfcsRuncard.CURRENT_SITE = propertyprovider.sfcsOperationSites.ID;
                            sfcsRuncard.WIP_OPERATION = route.sfcsRouteConfigs.FirstOrDefault().CURRENT_OPERATION_ID;
                            sfcsRuncard.LAST_OPERATION = route.sfcsRouteConfigs.FirstOrDefault().NEXT_OPERATION_ID;
                            propertyprovider.sfcsRuncard = sfcsRuncard;
                            return true;
                        }
                        else
                        {
                            throw new Exception(String.Format("流水号{0}格式不匹配", propertyprovider.data));
                        }

                    }
                    return false;
                }

            }
        }
        /// <summary>
        /// 查询产品配置
        /// </summary>
        /// <param name=""></param>
        /// <param name="partNumber"></param>
        /// <param name="configType"></param>
        /// <returns></returns>
        private SfcsProductConfig FindPartNumberConfig(List<SfcsProductConfig> sfcsProductConfigs, string partNumber, string configType)
        {
            SfcsParameters sfcsParameters = _repository.QueryEx<SfcsParameters>(
                "SELECT SP.* FROM SFCS_PARAMETERS SP WHERE SP.LOOKUP_TYPE = :LOOKUP_TYPE AND SP.MEANING = :MEANING",
                new
                {
                    LOOKUP_TYPE = "PRODUCT_CONFIG_TYPE",
                    MEANING = configType
                }).FirstOrDefault();
            if (sfcsParameters != null && sfcsParameters.ENABLED == "Y")
            {
                if (sfcsProductConfigs != null)
                {
                    SfcsProductConfig sfcsProductConfig = sfcsProductConfigs.Where(f => f.PART_NO == partNumber && f.CONFIG_TYPE == sfcsParameters.LOOKUP_CODE && f.ENABLED == "Y").FirstOrDefault();
                    if (sfcsProductConfig != null)
                    {
                        return sfcsProductConfig;
                    }
                    else
                    {
                        sfcsProductConfig = sfcsProductConfigs.Where(f => f.PART_NO == "000000" && f.CONFIG_TYPE == sfcsParameters.LOOKUP_CODE && f.ENABLED == "Y").FirstOrDefault();
                        if (sfcsProductConfig != null)
                        {
                            return sfcsProductConfig;
                        }
                    }
                }
            }

            throw new Exception(String.Format("料号：{0}没有做{1}设定!", partNumber, configType));
        }
        /// <summary>
        /// 根据sn获取工单流水号范围
        /// </summary>
        /// <param name="sn"></param>
        /// <returns></returns>
        private SfcsRuncardRanger FindWorkOrderRanger(string sn)
        {
            SfcsRuncardReplace sfcsRuncardReplace = _repository.QueryEx<SfcsRuncardReplace>(
                "SELECT * FROM SFCS_RUNCARD_REPLACE SRR WHERE OLD_SN = :OLD_SN",
                new
                {
                    OLD_SN = sn
                }
                ).FirstOrDefault();
            if (sfcsRuncardReplace != null)
            {
                throw new Exception(String.Format("{0}已经被替换使用，不能再投入使用!", sn));
            }

            var sfcsRuncardRangers = _repository.GetListEx<SfcsRuncardRanger>(
                 @"WHERE :SN BETWEEN SN_BEGIN AND SN_END
                  AND LENGTH(:SN) = LENGTH(SN_END)
                  AND (FIX_HEADER = SUBSTR(:SN,1,HEADER_LENGTH) OR FIX_HEADER IS NULL)
                  AND (FIX_TAIL = SUBSTR(:SN, LENGTH(:SN)-TAIL_LENGTH+1, TAIL_LENGTH) OR FIX_TAIL IS NULL)",
                 new
                 {
                     SN = sn
                 }
                 );

            if (sfcsRuncardRangers == null)
            {
                return null;
            }

            if (sfcsRuncardRangers.Count() > 1)
            {
                throw new Exception(String.Format("工单流水号范围设置重复,SN={0}", sn));
            }

            return sfcsRuncardRangers.FirstOrDefault();

        }

        private bool IsRuncardExist(Propertyprovider propertyprovider, IDbTransaction trance)
        {
            bool tranceWoflag = false;

            SfcsRuncard sfcsRuncard = null;
            SfcsCollectUids sfcsCollectUids = _sfcsCollectUidsRepository.QueryEx<SfcsCollectUids>(
                    "SELECT SCU.* FROM SFCS_COLLECT_UIDS SCU WHERE UID_NUMBER = :UID_NUMBER ",
                    new
                    {
                        UID_NUMBER = propertyprovider.data
                    }).FirstOrDefault();
            if (sfcsCollectUids != null)
            {
                //propertyprovider.product.runcardExist = true;
                sfcsRuncard = _sfcsRuncardRepository.QueryEx<SfcsRuncard>(
                    "SELECT * from SFCS_RUNCARD WHERE ID = :ID",
                    new
                    {
                        ID = sfcsCollectUids.SN_ID
                    }).FirstOrDefault();
            }
            if (sfcsRuncard == null)
            {
                sfcsRuncard = _sfcsRuncardRepository.QueryEx<SfcsRuncard>(
                    "SELECT * from SFCS_RUNCARD WHERE SN = :SN",
                    new
                    {
                        SN = propertyprovider.data
                    }).FirstOrDefault();
            }

            if (sfcsRuncard == null)
            {
                propertyprovider.product.runcardExist = false;
                return false;
            }
            //else
            //{
            //Product product = InitializeProduction((Decimal)sfcsRuncard.WO_ID);
            //product.runcardExist = true;
            //propertyprovider.product = product;
            //Route route = InitialRoute((Decimal)sfcsRuncard.ROUTE_ID);
            //propertyprovider.route = route;
            //propertyprovider.sfcsRuncard = sfcsRuncard;
            //return true;
            //}
            decimal wo_id = (Decimal)sfcsRuncard.WO_ID;
            decimal routeId = (Decimal)sfcsRuncard.ROUTE_ID;
            //TU站转工单号
            if (propertyprovider.sfcsOperationSites.OPERATION_ID == GlobalVariables.TUOperation
                && propertyprovider.woNo != "")
            {
                SfcsWo sfcsWo = _sfcsRuncardRepository.QueryEx<SfcsWo>(
                    "SELECT * FROM  SFCS_WO WHERE WO_NO = :WO_NO",
                    new
                    {
                        WO_NO = propertyprovider.woNo
                    }
                    ).FirstOrDefault();
                if (sfcsWo.ID != wo_id)
                {
                    sfcsWo.ROUTE_ID = sfcsWo.ROUTE_ID > 0 ? sfcsWo.ROUTE_ID : _sfcsProductConfigRepository.GetRouteIdByPartNo(sfcsWo.PART_NO);
                    if (sfcsWo.ROUTE_ID <= 0)
                    {
                        throw new Exception(String.Format(String.Format("工单号{0}没有设置制程!", sfcsWo.WO_NO)));
                    }

                    #region UT站验证
                    //1.转入的SN必须是已结束生产的状态
                    if (!sfcsRuncard.SN.IsNullOrWhiteSpace() && !GlobalVariables.EndOperation.Equals(sfcsRuncard.WIP_OPERATION) && !GlobalVariables.EndOperation.Equals(sfcsRuncard.LAST_OPERATION))
                        throw new Exception(String.Format(String.Format("转入的SN:{0},必须是已结束生产的状态!", sfcsRuncard.SN)));

                    //2.转入的SN所属的成品料号，必须在被转入工单的BOM中
                    //2.1查SN所属的成品料号
                    SfcsWo sfcsWoBySN = _sfcsRuncardRepository.QueryEx<SfcsWo>(
                    "SELECT * FROM  SFCS_WO WHERE ID = :ID",
                    new
                    {
                        ID = sfcsRuncard.WO_ID
                    }
                    ).FirstOrDefault();

                    if (sfcsWoBySN.IsNullOrWhiteSpace() || sfcsWoBySN.PART_NO.IsNullOrEmpty())
                        throw new Exception(String.Format(String.Format("SN:{0}异常,原因SN找不到对应的工单或料号!", sfcsRuncard.SN)));

                    //2.2判断是否存在转入的工单BOM中
                    string countBySql = @"SELECT COUNT(*) FROM SMT_BOM1 B1,SMT_BOM2 B2 
                                     WHERE B1.BOM_ID=B2.BOM_ID AND B1.PARTENT_CODE=:PARTENT_CODE AND B2.PART_CODE=:PART_CODE";
                    var count = _sfcsProductConfigRepository.ExecuteScalar(
                         countBySql,
                         new
                         {
                             PARTENT_CODE = sfcsWo.PART_NO,
                             PART_CODE = sfcsWoBySN.PART_NO
                         });
                    if(count<=0)
                        throw new Exception(String.Format(String.Format("当前SN:{0} 不属于被转工单{1},请注意检查!", sfcsRuncard.SN, sfcsWo.WO_NO)));
                    #endregion

                    //记录转工单数据
                    String woReplaceSql = @"INSERT INTO SFCS_WO_REPLACE (REPLACE_WO_ID,REPLACE_OPERATION_ID,SN_ID,OLD_WO_ID,NEW_WO_ID,PLANT_CODE,REPLACE_TYPE,REPLACE_SITE_ID,REPLACE_ORDER_NO,REPLACE_BY,REPLACE_TIME)
VALUES (MES_SEQ_ID.NEXTVAL,:REPLACE_OPERATION_ID,:SN_ID,:OLD_WO_ID,:NEW_WO_ID,:PLANT_CODE,:REPLACE_TYPE,:REPLACE_SITE_ID,:REPLACE_ORDER_NO,:REPLACE_BY,sysdate)";

                    _repository.ExecuteAsync(woReplaceSql, new
                    {
                        REPLACE_OPERATION_ID = propertyprovider.OperationId,
                        SN_ID = sfcsRuncard.ID,
                        OLD_WO_ID = sfcsRuncard.WO_ID,
                        NEW_WO_ID = sfcsWo.ID,
                        PLANT_CODE = 1,
                        REPLACE_TYPE = 3,
                        REPLACE_SITE_ID = propertyprovider.sfcsOperationSites.ID,
                        REPLACE_ORDER_NO = 0,
                        REPLACE_BY = propertyprovider.sys_Manager.USER_NAME
                    }, trance);

                    wo_id = sfcsWo.ID;
                    sfcsRuncard.WO_ID = wo_id;

                    routeId = sfcsWo.ROUTE_ID;
                    sfcsRuncard.ROUTE_ID = routeId;
                    tranceWoflag = true;
                }
            }
            Product product = InitializeProduction(wo_id);
            product.runcardExist = true;
            propertyprovider.product = product;
            Route route = InitialRoute(routeId);
            propertyprovider.route = route;
            if (tranceWoflag)
            {
                sfcsRuncard.CURRENT_SITE = propertyprovider.sfcsOperationSites.ID;
                sfcsRuncard.WIP_OPERATION = route.sfcsRouteConfigs.FirstOrDefault().CURRENT_OPERATION_ID;
                sfcsRuncard.LAST_OPERATION = route.sfcsRouteConfigs.FirstOrDefault().NEXT_OPERATION_ID;
            }
            propertyprovider.sfcsRuncard = sfcsRuncard;
            propertyprovider.product.runcardExist = true;
            return true;
        }

        private Product InitializeProduction(Decimal woId)
        {
            Product product = new Product();
            SfcsWo sfcswo = _sfcsWoRepository.Get((Decimal)woId);
            if (sfcswo == null)
            {
                throw new Exception(String.Format("系统找不到工单, 工单ID:{0}", woId));
            }
            if (sfcswo.OUTPUT_QTY >= sfcswo.TARGET_QTY && sfcswo.OUTPUT_QTY > 0)
            {
                throw new Exception(String.Format("工单号:{0}已经刷满", sfcswo.WO_NO));
            }
            product.workOrder = sfcswo.WO_NO;
            product.workOrderId = sfcswo.ID;
            SfcsPn sfcsPn = _sfcsPnRepository.QueryEx<SfcsPn>(
                "SELECT * FROM SFCS_PN WHERE PART_NO = :PART_NO",
                new
                {
                    PART_NO = sfcswo.PART_NO
                }).FirstOrDefault();
            product.partNumber = sfcsPn == null ? "" : sfcsPn.PART_NO;
            product.customerID = sfcsPn == null ? 0 : sfcsPn.CUSTOMER_ID;
            product.customerPartNumber = sfcsPn == null ? "" : sfcsPn.CUSTOMER_PN;
            product.familyID = sfcsPn == null ? 0 : sfcsPn.FAMILY_ID;
            product.sfcswo = sfcswo;
            SfcsModel sfcsModel = null;
            if (sfcswo.MODEL_ID != 0)
            {
                sfcsModel = _repository.QueryEx<SfcsModel>(
                               "select * from SFCS_MODEL where ID = :ID",
                               new
                               {
                                   ID = sfcswo.MODEL_ID
                               }
                               ).FirstOrDefault();
                product.model = sfcsModel.MODEL;
                product.modelID = sfcsModel.ID;
            }
            List<SfcsProductConfig> sfcsProductConfigs = _repository.QueryEx<SfcsProductConfig>(
                "SELECT SPC.* FROM SFCS_PRODUCT_CONFIG SPC WHERE SPC.PART_NO = :PART_NO AND ENABLED = :ENABLED",
                 new
                 {
                     PART_NO = sfcsPn.PART_NO,
                     ENABLED = "Y"
                 });
            //默认产品配置信息
            List<SfcsProductConfig> noSfcsProductConfigs = _repository.QueryEx<SfcsProductConfig>(
               "SELECT SPC.* FROM SFCS_PRODUCT_CONFIG SPC WHERE SPC.PART_NO = :PART_NO AND ENABLED = :ENABLED",
                new
                {
                    PART_NO = "000000",
                    ENABLED = "Y"
                });
            if (sfcsProductConfigs == null || sfcsProductConfigs.Count <= 0)
            {
                sfcsProductConfigs = noSfcsProductConfigs;
            }
            else
            {
                sfcsProductConfigs.AddRange(noSfcsProductConfigs);
            }
            if (sfcsProductConfigs != null && sfcsProductConfigs.Count > 0)
            {
                product.sfcsProductConfigs = sfcsProductConfigs;
            }
            if (product.sfcswo.ROUTE_ID <= 0)
            {
                product.sfcswo.ROUTE_ID = _sfcsProductConfigRepository.GetRouteIdByPartNo(sfcsPn.PART_NO);
                product.isPartRoute = true;
            }
            return product;
        }

        #region 中转码

        /// <summary>
        /// 保存中转码采集的数据并进行过站处理
        /// </summary>
        /// <param name="model"></param>
        /// <param name="tran"></param>
        /// <returns></returns>
        private async Task<CollectMiddleCodeDataRequestModel> SaveMiddleCodeData(CollectMiddleCodeDataRequestModel model, IDbTransaction tran)
        {
            CollectMiddleCodeDataRequestModel collectMiddleCode = model;
            //1.自动打印  是 产生中转码 -> 根据工单号去获取流水号范围表 根据流水号范围表获取SN 记录到中转码使用日志表 和 部件码绑定关系表  
            //            否 绑定中转码 -> 根据工单号去获取流水号范围表 根据流水号范围表获取SN 记录到中转码使用日志表 和 部件码绑定关系表  
            //2.记录完成调用 AssemblyOperation/CollectPalletData进行过站处理  过站成功 PRINTCODE=true 生成打印信息并解绑中转码
            collectMiddleCode.SN = await GetSNByWoId(model.WO.ID);
            if (collectMiddleCode.SN.IsNullOrEmpty()) { throw new Exception("SN_ERROR"); }

            bool result = this.SaveMiddleCodeList(collectMiddleCode, tran);
            if (result)
            {
                Propertyprovider propertyprovider = new Propertyprovider();
                propertyprovider.data = collectMiddleCode.SN;//根据流水号范围表获取到的SN
                propertyprovider.sys_Manager = collectMiddleCode.MANAGER;
                propertyprovider.sfcsOperationLines = collectMiddleCode.OPERATIONLINES;
                propertyprovider.sfcsOperationSites = collectMiddleCode.OPERATIONSITES;
                propertyprovider.woNo = collectMiddleCode.WO_NO;
                propertyprovider = await DoProcessAsync(propertyprovider, tran);//过站处理
                model.RESULT = (int)propertyprovider.result;
                model.MESSAGE = propertyprovider.msg;
                if (model.RESULT == 0 && model.PRINTCODE)
                {
                    //生成打印信息并解绑中转码信息
                    model.PRINTTASKID = UnbindMiddleCodeData(collectMiddleCode, false, tran);
                }
                else if (model.RESULT == 2)
                {
                    throw new Exception(model.MESSAGE);
                }
            }
            else
            {
                throw new Exception("SAVE_MIDDLE_DATA_ERROR");
            }
            return model;
        }

        private bool SaveMiddleCodeList(CollectMiddleCodeDataRequestModel model, IDbTransaction tran)
        {
            int resdata = 0; decimal newid = 0;
            //获取中转码数据
            if (model.MIDDLECODE == null)
            {
                newid = _sfcsRuncardRepository.QueryEx<Decimal>("SELECT MES_MIDDLE_CODE_SEQ.NEXTVAL MY_SEQ FROM DUAL").FirstOrDefault();
                //生成中转码
                string result = Core.Utilities.RadixConvertPublic.RadixConvert(newid.ToString(), ViewModels.GlobalVariables.DecRadix, ViewModels.GlobalVariables.Base36Redix);
                string ReleasedSequence = result.PadLeft(6, '0');
                string yymmdd = _sfcsRuncardRepository.QueryEx<string>("SELECT TO_CHAR(SYSDATE,'YYMMDD') YYMMDD FROM DUAL ").FirstOrDefault();
                String middleCode = "ZZ" + yymmdd + ReleasedSequence;

                //1.根据中转码生成规则生成中转码 2.记录到MES_MIDDLE_CODE  STATUS状态（0：待用；1：在 用；2:报废）
                string insertMiddleCodeSql = @"INSERT INTO MES_MIDDLE_CODE  (ID,CODE,CHREATE_TIME,CREATOR,STATUS) VALUES (:ID,:CODE,SYSDATE,:CREATOR,'1')";
                resdata = _sfcsRuncardRepository.Execute(insertMiddleCodeSql, new { ID = newid, CODE = middleCode, CREATOR = model.MANAGER.USER_NAME }, tran);
            }
            else
            {
                newid = model.MIDDLECODE.ID;
                string updateMiddleCodeSql = @"UPDATE MES_MIDDLE_CODE SET STATUS='1' WHERE ID=:ID ";
                resdata = _sfcsRuncardRepository.Execute(updateMiddleCodeSql, new { ID = newid }, tran);
            }
            model.MIDDLECODE = _sfcsRuncardRepository.QueryEx<MesMiddleCode>("SELECT * FROM MES_MIDDLE_CODE WHERE ID = :ID  AND STATUS = '1'", new { ID = newid }, tran).FirstOrDefault();
            if (resdata <= 0 || model.MIDDLECODE == null) { throw new Exception("SAVE_MIDDLE_DATA_ERROR"); }

            //新增中转码使用日志 STATUS状态（0：未解绑；1：已解绑）
            string getIdsql = "SELECT MES_MIDDLE_CODE_LOG_SEQ.NEXTVAL MY_SEQ FROM DUAL";
            int logId = _sfcsRuncardRepository.QueryEx<int>(getIdsql).FirstOrDefault();
            string insertMiddleCodeLogSql = @"INSERT INTO MES_MIDDLE_CODE_LOG (ID,CODE_ID,CODE,SN,COLLECT_QTY,WO_NO,LINK_SITE_ID,LINK_TIME,LINK_USER,STATUS) VALUES (:ID,:CODE_ID,:CODE,:SN,:COLLECT_QTY,:WO_NO,:LINK_SITE_ID,SYSDATE,:LINK_USER,'0')";
            resdata = _sfcsRuncardRepository.Execute(insertMiddleCodeLogSql, new
            {
                ID = logId,
                CODE_ID = model.MIDDLECODE.ID,
                CODE = model.MIDDLECODE.CODE,
                SN = model.SN,
                COLLECT_QTY = model.COLLECT_QTY,
                WO_NO = model.WO_NO,
                LINK_SITE_ID = model.OPERATIONSITES.ID,
                LINK_USER = model.MANAGER.USER_NAME
            }, tran);
            if (resdata <= 0) { throw new Exception("SAVE_MIDDLE_DATA_ERROR"); }

            //部件码绑定记录
            //CODE_LOG_ID 中转条码ID CODE 关键部件的条码 CODE_TYPE条码类型（0:物料条码；1：半成 品条码；2：周转箱条码）
            //部件条码目前有三种: 第一种是半成品在SFCS_RUNCARD 表中的SN :第二种是仓库条码IMS_REEL的CODE: 第三种是周转码MES_BATCH_PRING的CARTON_NO
            int partQty = 0;//已收集的零件数量
            string insertMiddleLogCollectSql = @"INSERT INTO MES_MIDDLE_LOG_COLLECT  (ID,CODE_LOG_ID,CODE,CODE_TYPE) VALUES (:ID,:CODE_LOG_ID,:CODE,:CODE_TYPE)";
            getIdsql = "SELECT MES_MIDDLE_LOG_COLLECT_SEQ.NEXTVAL MY_SEQ FROM DUAL";
            if (model.IMSREEL != null)
            {
                partQty++;
                int collectId = _sfcsRuncardRepository.QueryEx<int>(getIdsql).FirstOrDefault();
                resdata = _sfcsRuncardRepository.Execute(insertMiddleLogCollectSql, new { ID = collectId, CODE_LOG_ID = logId, CODE = model.IMSREEL.CODE, CODE_TYPE = "0" }, tran);
                if (resdata <= 0) { throw new Exception("SAVE_MIDDLE_DATA_ERROR"); }
            }
            if (model.RUNCARD != null)
            {
                partQty++;
                int collectId = _sfcsRuncardRepository.QueryEx<int>(getIdsql).FirstOrDefault();
                resdata = _sfcsRuncardRepository.Execute(insertMiddleLogCollectSql, new { ID = collectId, CODE_LOG_ID = logId, CODE = model.RUNCARD.SN, CODE_TYPE = "1" }, tran);
                if (resdata <= 0) { throw new Exception("SAVE_MIDDLE_DATA_ERROR"); }
            }
            if (model.BATCHPRING != null)
            {
                partQty++;
                int collectId = _sfcsRuncardRepository.QueryEx<int>(getIdsql).FirstOrDefault();
                resdata = _sfcsRuncardRepository.Execute(insertMiddleLogCollectSql, new { ID = collectId, CODE_LOG_ID = logId, CODE = model.BATCHPRING.CARTON_NO, CODE_TYPE = "2" }, tran);
                if (resdata <= 0) { throw new Exception("SAVE_MIDDLE_DATA_ERROR"); }
            }
            //校验部件数和已收集数量是否相同
            if (model.COLLECT_QTY != partQty) { throw new Exception("COLLECT_QTY_NOT_OVER"); }

            return true;
        }

        private int UnbindMiddleCodeData(CollectMiddleCodeDataRequestModel model, bool isPrintSN, IDbTransaction tran)
        {
            int resdata = 0;
            int PrintFileId = 0;
            StringBuilder printData = new StringBuilder();
            int printTaskId = _sfcsRuncardRepository.QueryEx<int>("SELECT SFCS_PRINT_TASKS_SEQ.NEXTVAL MY_SEQ FROM DUAL").FirstOrDefault();
            if (isPrintSN)
            {
                //打印SN
                String printMappSql = @"SELECT SPF.* FROM SFCS_PRINT_FILES_MAPPING SPFM, SFCS_PRINT_FILES  SPF 
                        WHERE SPFM.PRINT_FILE_ID = SPF.ID AND SPFM.ENABLED = 'Y' AND SPF.ENABLED = 'Y' AND SPF.LABEL_TYPE = 1";
                String printMappSqlByPn = printMappSql + " AND SPFM.PART_NO = :PART_NO";
                SfcsPrintFiles sfcsPrintFiles = null;
                List<SfcsPrintFiles> sfcsPrintMapplist = null;
                sfcsPrintMapplist = _sfcsRuncardRepository.QueryEx<SfcsPrintFiles>(printMappSqlByPn, new { PART_NO = model.PN.PART_NO });

                if (sfcsPrintMapplist == null)
                {
                    String printMappSqlByModel = printMappSql + " AND SPFM.MODEL_ID = :MODEL_ID";
                    sfcsPrintMapplist = _sfcsRuncardRepository.QueryEx<SfcsPrintFiles>(printMappSqlByModel, new { MODEL_ID = model.PN.MODEL_ID });
                }
                if (sfcsPrintMapplist == null)
                {
                    String printMappSqlByFamilly = printMappSql + " AND SPFM.PRODUCT_FAMILY_ID = :PRODUCT_FAMILY_ID";
                    sfcsPrintMapplist = _sfcsRuncardRepository.QueryEx<SfcsPrintFiles>(printMappSqlByFamilly, new { PRODUCT_FAMILY_ID = model.PN.FAMILY_ID });
                }
                if (sfcsPrintMapplist == null)
                {
                    String printMappSqlByCustor = printMappSql + " AND SPFM.CUSTOMER_ID = :CUSTOMER_ID";

                    sfcsPrintMapplist = _sfcsRuncardRepository.QueryEx<SfcsPrintFiles>(printMappSqlByCustor, new { CUSTOMER_ID = model.PN.CUSTOMER_ID });
                }
                //默认产品条码模板
                if (sfcsPrintMapplist == null || sfcsPrintMapplist.Count <= 0)
                {
                    sfcsPrintMapplist = _sfcsRuncardRepository.QueryEx<SfcsPrintFiles>(printMappSqlByPn, new { PART_NO = "000000" });
                }
                if (sfcsPrintMapplist != null && sfcsPrintMapplist.Count > 0)
                {
                    sfcsPrintFiles = sfcsPrintMapplist.FirstOrDefault();
                    if (sfcsPrintFiles != null) { PrintFileId = (int)sfcsPrintFiles.ID; }
                }
                else
                {
                    throw new Exception("ERR_SETPRODUCTPRINTFILE");
                }

                String detail = "", detailValue = "", header = "";
                detail = _repository.QueryEx<String>("SELECT CONFIG_VALUE FROM SFCS_PRODUCT_CONFIG WHERE PART_NO =:PART_NO AND CONFIG_TYPE =:CONFIG_TYPE AND ENABLED='Y'",
                            new { PART_NO = model.PN.PART_NO, CONFIG_TYPE = GlobalVariables.SNPrintData }).FirstOrDefault();
                if (!detail.IsNullOrEmpty())
                {
                    var detailArr = detail.Split("|");
                    for (int i = 0; i < detailArr.Length; i++)
                    {
                        header += "," + GlobalVariables.PrintHeader + (i + 1);
                        detailValue += "," + detailArr[i];
                    }
                }

                ImsPart imsPart = _repository.QueryEx<ImsPart>("SELECT * FROM IMS_PART WHERE CODE = :CODE",
                    new { CODE = model.PN.PART_NO }).FirstOrDefault();
                printData.AppendLine("PN,PN_NAME,MODEL,SN,QR_NO" + header);
                String qrNo = String.Format("{0}|{1}|{2}|{3}|{4}", imsPart.CODE, imsPart.NAME, imsPart.DESCRIPTION, model.SN, detail);
                printData.AppendLine(String.Format("{0},{1},{2},{3},{4}{5}", imsPart.CODE, imsPart.NAME, imsPart.DESCRIPTION, model.SN, qrNo, detailValue));
            }
            else
            {
                //打印中转码
                PrintFileId = _sfcsRuncardRepository.QueryEx<int>("SELECT ID FROM SFCS_PRINT_FILES WHERE FILE_NAME='中转条码标签' AND ENABLED = 'Y'").FirstOrDefault();
                printData.AppendLine("SN,CODE");
                printData.AppendLine(String.Format("{0},{1}", model.SN, model.MIDDLECODE.CODE));
            }
            if (PrintFileId < 1) { throw new Exception("ERR_SETPRODUCTPRINTFILE"); }

            string insertPrintTaskSql = @"INSERT INTO SFCS_PRINT_TASKS(ID,PRINT_FILE_ID,OPERATOR,CREATE_TIME,PRINT_STATUS,PRINT_DATA,PART_NO,WO_NO)VALUES(:ID,:PRINT_FILE_ID,:OPERATOR,SYSDATE,0,:PRINT_DATA,:PART_NO,:WO_NO)";
            resdata = _sfcsRuncardRepository.Execute(insertPrintTaskSql, new
            {
                ID = printTaskId,
                PRINT_FILE_ID = PrintFileId,
                OPERATOR = model.MANAGER.USER_NAME,
                PRINT_DATA = printData.ToString(),
                PART_NO = model.PN.PART_NO,
                WO_NO = model.WO_NO
            }, tran);
            if (resdata <= 0) { throw new Exception("UNBIND_MIDDLE_DATA_ERROR"); }

            MesMiddleCodeLog middleCodeLog = _sfcsRuncardRepository.QueryEx<MesMiddleCodeLog>("SELECT * FROM MES_MIDDLE_CODE_LOG WHERE CODE_ID = :CODE_ID AND SN = :SN ", new { CODE_ID = model.MIDDLECODE.ID, SN = model.SN }).FirstOrDefault();
            //STATUS状态（0：待用；1：在 用；2:报废）
            string updateSql = @"UPDATE MES_MIDDLE_CODE SET STATUS='0' WHERE ID=:ID ";
            resdata = _sfcsRuncardRepository.Execute(updateSql, new { ID = model.MIDDLECODE.ID }, tran);
            if (resdata <= 0 || middleCodeLog == null) { throw new Exception("UNBIND_MIDDLE_DATA_ERROR"); }

            //STATUS状态（0：未解绑；1：已解绑）
            updateSql = @"UPDATE MES_MIDDLE_CODE_LOG SET STATUS='1' WHERE ID=:ID ";
            resdata = _sfcsRuncardRepository.Execute(updateSql, new { ID = middleCodeLog.ID }, tran);
            if (resdata <= 0) { throw new Exception("UNBIND_MIDDLE_DATA_ERROR"); }

            return printTaskId;
        }

        /// <summary>
        /// 根据工单在流水号范围里获取一个SN
        /// </summary>
        /// <param name="wo_id"></param>
        /// <returns></returns>
        private async Task<String> GetSNByWoId(decimal wo_id)
        {
            String sn = "";
            List<String> snList = new List<string>();

            List<SfcsRuncardRanger> runcardRangerList = await _repository.GetListByTableEX<SfcsRuncardRanger>("RA.*", "SFCS_RUNCARD_RANGER RA,SFCS_RUNCARD_RANGER_RULES RU", " AND RA.RANGER_RULE_ID = RU.ID AND RU.RULE_TYPE = :RULE_TYPE AND RA.WO_ID = :WO_ID ORDER BY RA.ID DESC ", new { WO_ID = wo_id, RULE_TYPE = GlobalVariables.RangerSN });
            if (runcardRangerList.Count > 0)
            {
                foreach (var item in runcardRangerList)
                {
                    if (item.PRINTED != "Y")
                    {
                        List<String> runcardSNList = await _repository.GetListByTableEX<String>("SN", "SFCS_RUNCARD", " AND SN LIKE :SN ORDER BY SN ASC ", new { SN = item.FIX_HEADER + "%" + item.FIX_TAIL });
                        List<String> logSNList = await _repository.GetListByTableEX<String>("SN", "MES_MIDDLE_CODE_LOG", " AND SN LIKE :SN ORDER BY SN ASC ", new { SN = item.FIX_HEADER + "%" + item.FIX_TAIL });
                        if (runcardSNList.Count() < item.QUANTITY)
                        {
                            List<String> rangerSNList = (await GenerateRangerSN(item));
                            if (rangerSNList.Count > 0)
                            {
                                snList = rangerSNList.Except(runcardSNList).ToList();
                                //if (snList.Count() > 0) { break; }
                                foreach (var snitem in snList)
                                {
                                    if (logSNList.Where(m => m == snitem).FirstOrDefault().IsNullOrEmpty()) { sn = snitem; }
                                    if (!sn.IsNullOrEmpty()) { break; }
                                }
                                if (!sn.IsNullOrEmpty()) { break; }
                            }
                        }
                    }
                }
            }
            return sn;
        }

        /// <summary>
        /// 计算出流水号范围中的流水号信息
        /// </summary>
        /// <param name="sfcsRuncardRanger"></param>
        /// <returns></returns>
        private async Task<List<string>> GenerateRangerSN(SfcsRuncardRanger sfcsRuncardRanger)
        {
            List<string> SerialNumberList = new List<string>();
            var sfcsParameterslist = await _repository.QueryAsyncEx<SfcsParameters>("SELECT SP.* FROM SFCS_PARAMETERS SP WHERE LOOKUP_TYPE = :LOOKUP_TYPE AND LOOKUP_CODE = :LOOKUP_CODE",
                 new
                 {
                     LOOKUP_TYPE = "RADIX_TYPE",
                     LOOKUP_CODE = sfcsRuncardRanger.DIGITAL
                 });
            if (sfcsParameterslist == null || sfcsParameterslist.Count() <= 0)
            {
                return null;
            }
            string standardDigits = sfcsParameterslist.FirstOrDefault().DESCRIPTION;
            SerialNumberList.Add(sfcsRuncardRanger.SN_BEGIN);
            string snBeginRange = sfcsRuncardRanger.SN_BEGIN.Substring(
                (int)(sfcsRuncardRanger.HEADER_LENGTH), (int)sfcsRuncardRanger.RANGE);
            for (int i = 1; i < sfcsRuncardRanger.QUANTITY; i++)
            {
                // calculate sn from 2nd to the last
                string snRange = Core.Utilities.RadixConvertPublic.RadixInc(snBeginRange, standardDigits, i).PadLeft(snBeginRange.Length, '0').Trim();
                string sn = (sfcsRuncardRanger.FIX_HEADER.IsNullOrEmpty() ? "" : sfcsRuncardRanger.FIX_HEADER.ToString().Trim()) +
                    snRange + (sfcsRuncardRanger.FIX_TAIL.IsNullOrEmpty() ? "" : sfcsRuncardRanger.FIX_TAIL.ToString().Trim());

                // add new sn into list
                SerialNumberList.Add(sn);
            }
            return SerialNumberList;
        }

        /// <summary>
        /// 获取中转码采集记录
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        private List<CollectPartListModel> GetMiddleCodeData(CollectMiddleCodeDataRequestModel model)
        {
            String sQuery = @"SELECT (CASE WHEN MLC.CODE_TYPE = 0 THEN '物料条码' WHEN MLC.CODE_TYPE = 1 THEN '半成品条码' WHEN MLC.CODE_TYPE = 2 THEN '周转箱条码' ELSE '' END ) OBJECT_NAME, MCL.COLLECT_QTY NEED_ASSEMBLY_QTY,0 COLLECTED_QTY,'' DATA_FORMAT, SW.PART_NO,'' FIXED_VALUE FROM MES_MIDDLE_LOG_COLLECT MLC LEFT JOIN MES_MIDDLE_CODE_LOG MCL ON MLC.CODE_LOG_ID = MCL.ID LEFT JOIN SFCS_RUNCARD SR ON MCL.SN = SR.SN  LEFT JOIN SFCS_WO SW ON SR.WO_ID= SW.ID WHERE MCL.SN = :SN";

            return _repositoryMC.QueryEx<CollectPartListModel>(sQuery, new { SN = model.SN });
        }
        #endregion

        /// <summary>
        /// 更新栈板已扫箱
        /// </summary>
        private decimal UpdatePalletCurrentQty(Propertyprovider propertyprovider, IDbTransaction transaction, bool palletVerify)
        {
            if (palletVerify)
            {
                //采集栈板的没有SFCS_CONTAINER_LIST
                String collectPalletSql = @"SELECT QUANTITY FROM SFCS_COLLECT_PALLETS WHERE PALLET_NO=:PALLET_NO AND PART_NO=:PART_NO";
                var qty = _sfcsRuncardRepository.QueryEx<decimal>(collectPalletSql, new
                {
                    PALLET_NO = propertyprovider.pallet.collectDataList[0].Data,
                    PART_NO = propertyprovider.product.partNumber
                }).FirstOrDefault();
                return qty;
            }

            decimal siteId = propertyprovider.sfcsOperationSites.ID;
            String containerSql = @"SELECT SCL.* FROM SFCS_CONTAINER_LIST SCL 
            WHERE  DATA_TYPE = :DATA_TYPE AND SITE_ID = :SITE_ID AND FULL_FLAG = 'N'";
            SfcsContainerList sfcsContainerList = _sfcsRuncardRepository.QueryEx<SfcsContainerList>(
                containerSql,
                new
                {
                    DATA_TYPE = GlobalVariables.PallectLabel,
                    SITE_ID = siteId
                }, transaction).FirstOrDefault();
            String pallectNo = null;
            if (sfcsContainerList != null)
            {
                pallectNo = sfcsContainerList.CONTAINER_SN;
                String seqSql = @"select count( distinct CARTON_NO) from SFCS_RUNCARD where PALLET_NO = :PALLET_NO";

                decimal seq = _sfcsRuncardRepository.QueryEx<decimal>(seqSql, new
                {
                    PALLET_NO = pallectNo
                }, transaction).FirstOrDefault();
                //卡通剛刷滿
                if (propertyprovider.pallet.DefinedQty == seq + 1)
                {
                    string U_UpdateContainerList = @"UPDATE SFCS_CONTAINER_LIST SET FULL_FLAG = 'Y', ATTRIBUTE1 = 'AUTO'
                                                  WHERE DATA_TYPE = :DATA_TYPE
                                                  AND CONTAINER_SN = :CONTAINER_SN
                                                  AND SITE_ID = :SITE_ID ";
                    _sfcsRuncardRepository.Execute(U_UpdateContainerList, new
                    {
                        DATA_TYPE = GlobalVariables.PallectLabel,
                        CONTAINER_SN = pallectNo,
                        SITE_ID = propertyprovider.sfcsOperationSites.ID
                    }, transaction);
                }
                String U_UpadateContainerListSeq = @"UPDATE SFCS_CONTAINER_LIST SET SEQUENCE= :SEQUENCE+1 WHERE CONTAINER_SN=:CONTAINER_SN";
                _sfcsRuncardRepository.Execute(U_UpadateContainerListSeq, new
                {
                    SEQUENCE = seq,
                    CONTAINER_SN = pallectNo
                }, transaction);

                return seq++;
            }            
            return 1;
        }

        #endregion
    }
}