using Aspose.Cells;
using JZ.IMS.Core.Helper;
using JZ.IMS.Core.Utilities;
using JZ.IMS.IRepository;
using JZ.IMS.IServices;
using JZ.IMS.ViewModels;
using JZ.IMS.ViewModels.BomVsPlacement;
using JZ.IMS.WebApi.Common;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Localization;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace JZ.IMS.WebApi.Controllers.BomVsPlacement
{
    /// <summary>
    /// 
    /// </summary>
    public class BomVsPlacementService
    {
        /// <summary>
        /// 是否出错
        /// </summary>
        public bool IsError = false;

        /// <summary>
        /// 出错信息
        /// </summary>
        public string ErrMsg = string.Empty;

        private string SmtType = "PANASONIC";

        private string source_filename = string.Empty;
        private string userName = string.Empty;
        public List<BOMData> BomInfo = null;

        private readonly IBomVsPlacementRepository _repository;
        private readonly IStringLocalizer<BomVsPlacementController> _localizer;

        public BomVsPlacementService(IBomVsPlacementRepository repository, IStringLocalizer<BomVsPlacementController> localizer)
        {
            _repository = repository;
            _localizer = localizer;
        }

        public BomVsPlacementService(IStringLocalizer<BomVsPlacementController> localizer)
        {
            _localizer = localizer;
        }

        private void Set(string Message)
        {
            IsError = true;
            ErrMsg = Message;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="smtType">料单类型</param>
        /// <param name="product_no">成品料号</param>
        /// <param name="bomType"></param>
        /// <param name="fileName">上传后的文件名</param>
        /// <param name="source_file">上传的源文件名</param>
        /// <param name="user_name">用户名称</param>
        public async Task<BomVsPlacementVM> LoadImportFile(string smtType, string product_no, BomType bomType, string fileName, string source_file,
            string user_name)
        {
            BomVsPlacementVM result = new BomVsPlacementVM();

            SmtType = smtType;
            source_filename = source_file;
            userName = user_name;

            Func<string, dynamic> importAction = null;
            switch (bomType)
            {
                case BomType.SMT组件:
                    importAction = new Func<string, dynamic>(file => { return this.LoadSmtPlacement(file); });
                    break;
                case BomType.AI组件:
                    importAction = new Func<string, dynamic>(file => { return this.LoadAIRIPlacement(file); });
                    break;
                case BomType.RI组件:
                    importAction = new Func<string, dynamic>(file => { return this.LoadAIRIPlacement(file); });
                    break;
                case BomType.ALL:
                    break;
            }

            if (importAction == null) return result;

            try
            {
                if (await this.LoadBom(product_no, bomType))
                {
                    result.PlacementList = importAction(fileName);
                }
                result.BomList = BomInfo;
                result.IsError = this.IsError;
                result.ErrMsg = this.ErrMsg;

                return result;
            }
            finally
            {
                if (File.Exists(fileName))
                {
                    File.Delete(fileName);
                }
            }
        }

        private dynamic LoadSmtPlacement(string placementFilePath)
        {
            if (this.SmtType == "YAMAHA")
            {
                return LoadYamahaPlacement(placementFilePath);
            }
            else if (this.SmtType == "PANASONIC")
            {
                return LoadPanasonicPlacement(placementFilePath);
            }
            else if (this.SmtType == "PANASONIC_CM")
            {
                return LoadSMTPlacement(placementFilePath);
            }
            else
            {
                return null;
            }
        }

        private PANASONIC_CM_VM LoadSMTPlacement(string placementFilePath)
        {
            CMPlacementSetup setup = new CMPlacementSetup(_localizer);
            var placement = setup.LoadPlacement(placementFilePath, source_filename);

            return setup.LoadSmtPlacementToBomSheet(placement);
        }

        private YamahaPlacementVM LoadYamahaPlacement(string placementFilePath)
        {
            YamahaPlacementSetup setupPlacement = new YamahaPlacementSetup(_localizer);
            var placement = setupPlacement.LoadYamahaCsvPlacement(placementFilePath, source_filename);

            YamahaPlacementVM result = new YamahaPlacementVM();
            result.TitleList = new List<string>() { "安装号码", "元件名", "图样名", "总数", "备注", "送料器类型", "传送间距" };
            result.PlacementName = setupPlacement.PlacementName;
            result.DataList = new List<YamahaPlacement>();
            foreach (var row in placement)
            {
                YamahaPlacement item = new YamahaPlacement()
                {
                    SLOT = row.SLOT,
                    PART_NO = row.PART_NO,
                    REFDESIGNATOR = row.REFDESIGNATOR == null ? string.Empty : row.REFDESIGNATOR,
                    UNITQTY = row.UNITQTY,
                    DESCRIPTION = row.DESCRIPTION == null ? string.Empty : row.DESCRIPTION,
                    FEEDER_TYPE = row.FEEDER_TYPE == null ? string.Empty : row.FEEDER_TYPE,
                    SIZE = row.SIZE == null ? string.Empty : row.SIZE,
                };
                result.DataList.Add(item);
            }
            return result;
        }

        private PanasonicPlacementVM LoadPanasonicPlacement(string placementFilePath)
        {
            PanasonicPlacementSetup setupPlacement = new PanasonicPlacementSetup(_localizer);
            var placement = setupPlacement.LoadCsvPlacement(placementFilePath, source_filename);

            PanasonicPlacementVM result = new PanasonicPlacementVM();
            result.PlacementName = setupPlacement.PlacementName;
            result.TitleList = new List<string>() { "机器名", "元件名", "贴片位号", "单位用量", "插槽", "子插槽", "备注", "模组编号", "总用量" };
            result.DataList = new List<PanasonicPlacement>();

            foreach (var row in placement)
            {
                PanasonicPlacement item = new PanasonicPlacement()
                {
                    MACHINE_NAME = row.MACHINE_NAME,
                    PART_NO = row.PART_NO,
                    REFDESIGNATOR = row.REFDESIGNATOR == null ? string.Empty : row.REFDESIGNATOR,
                    UNITQTY = row.UNITQTY,
                    SLOT = row.SLOT,
                    SUB_SLOT = row.SUB_SLOT,
                    DESCRIPTION = row.DESCRIPTION == null ? string.Empty : row.DESCRIPTION,
                    TABLE_NO = row.TABLE_NO,
                    TOTAL_QTY = row.TOTAL_QTY,
                };
                result.DataList.Add(item);
            }

            return result;
        }

        public AIRIPlacementVM<List<AIRIPlacement>> LoadAIRIPlacement(string placementFilePath)
        {
            Workbook workbook = new Workbook(placementFilePath);
            Worksheet sheet = workbook.Worksheets[0];

            AIRIPlacementSetup setupPlacement = new AIRIPlacementSetup(sheet, _localizer);
            var placement = setupPlacement.LoadAIRIPlacement();

            AIRIPlacementVM<List<AIRIPlacement>> result = new AIRIPlacementVM<List<AIRIPlacement>>();
            result.TitleList = new List<string>() { "站位", "编码", "位置", "用量", "名称", "规格", "方向" };
            result.PlacementName = setupPlacement.PlacementName;
            result.Part_NO = setupPlacement.Part_NO;

            result.DataList = new List<AIRIPlacement>();
            foreach (var row in placement)
            {
                var item = new AIRIPlacement()
                {
                    SLOT = row.SLOT,
                    PART_NO = row.PART_NO,
                    STAGE = "1",
                    REFDESIGNATOR = row.REFDESIGNATOR ?? string.Empty,
                    UNITQTY = row.UNITQTY,
                    DESCRIPTION = row.DESCRIPTION ?? string.Empty,
                    FEEDER_TYPE = row.FEEDER_TYPE ?? string.Empty,
                    LOCATION = row.LOCATION ?? string.Empty,
                };
                result.DataList.Add(item);
            }
            return result;
        }

        public AIRIPlacementVM<List<AIRIPlacement>> LoadRIPlacement(string placementFilePath)
        {
            Workbook workbook = new Workbook(placementFilePath);
            Worksheet sheet = workbook.Worksheets[0];
            AIRIPlacementSetup setupPlacement = new AIRIPlacementSetup(sheet, _localizer);
            var placement = setupPlacement.LoadRIPlacement();
            AIRIPlacementVM<List<AIRIPlacement>> result = new AIRIPlacementVM<List<AIRIPlacement>>();
            result.TitleList = new List<string>() { "站位信息", "料号信息", "位号信息", "用量", "飞达规格" };
            result.PlacementName = setupPlacement.PlacementName;
            result.Part_NO = setupPlacement.Part_NO;

            result.DataList = new List<AIRIPlacement>();
            foreach (var row in placement)
            {
                var item = new AIRIPlacement()
                {
                    SLOT = row.SLOT,
                    STAGE = "1",
                    PART_NO = row.PART_NO,
                    REFDESIGNATOR = row.REFDESIGNATOR ?? string.Empty,
                    UNITQTY = row.UNITQTY,
                    FEEDER_TYPE = row.FEEDER_TYPE ?? string.Empty,
                };
                result.DataList.Add(item);
            }
            return result;
        }

        public AIRIPlacementVM<List<AIRIPlacement>> LoadGalaxyPlacement(string placementFilePath, string fileName, string Part_NO)
        {
            Workbook workbook = new Workbook(placementFilePath);
            Worksheet sheet = workbook.Worksheets[0];
            AIRIPlacementSetup setupPlacement = new AIRIPlacementSetup(sheet, _localizer);
            var placement = setupPlacement.LoadGalaxyPlacement(placementFilePath);
            AIRIPlacementVM<List<AIRIPlacement>> result = new AIRIPlacementVM<List<AIRIPlacement>>();
            result.TitleList = new List<string>() { "站位信息", "料号信息", "位号信息", "用量", "飞达规格" };
            result.PlacementName = fileName;
            result.Part_NO = Part_NO;

            result.DataList = new List<AIRIPlacement>();
            foreach (var row in placement)
            {
                var item = new AIRIPlacement()
                {
                    SLOT = row.SLOT,
                    PART_NO = row.PART_NO,
                    REFDESIGNATOR = row.REFDESIGNATOR ?? string.Empty,
                    UNITQTY = row.UNITQTY,
                    FEEDER_TYPE = row.FEEDER_TYPE ?? string.Empty,
                    STAGE = "1",
                };
                result.DataList.Add(item);
            }
            return result;
        }

        public AIRIPlacementVM<List<AIRIPlacement>> LoadGalaxyPlacementExcel(string placementFilePath, string fileName, string Part_NO, int MultiNo)
        {
            Workbook workbook = new Workbook(placementFilePath);
            #region 美联
            //Worksheet sheet = workbook.Worksheets[0];
            //AIRIPlacementSetup setupPlacement = new AIRIPlacementSetup(sheet, _localizer);
            //var placement = setupPlacement.LoadGalaxyPlacementExcel(placementFilePath);
            #endregion
            Worksheet sheet = workbook.Worksheets[1];
            AIRIPlacementSetup setupPlacement = new AIRIPlacementSetup(sheet, _localizer);
            var placement = setupPlacement.BaseLoadGalaxyPlacementExcel(placementFilePath, MultiNo);
            AIRIPlacementVM<List<AIRIPlacement>> result = new AIRIPlacementVM<List<AIRIPlacement>>();
            result.TitleList = new List<string>() { "站位信息", "料号信息", "位号信息", "用量", "飞达规格" };
            result.PlacementName = fileName;
            result.Part_NO = Part_NO;

            result.DataList = new List<AIRIPlacement>();
            foreach (var row in placement)
            {
                var item = new AIRIPlacement()
                {
                    SLOT = row.SLOT,
                    PART_NO = row.PART_NO,
                    REFDESIGNATOR = row.REFDESIGNATOR ?? string.Empty,
                    UNITQTY = row.UNITQTY,
                    FEEDER_TYPE = row.FEEDER_TYPE ?? string.Empty,

                };
                result.DataList.Add(item);
            }
            return result;
        }

        public AIRIPlacementVM<List<AIRIPlacement>> LoadGalaxyPlacementTxt(string placementFilePath, string fileName, string Part_NO)
        {
            AIRIPlacementVM<List<AIRIPlacement>> result = new AIRIPlacementVM<List<AIRIPlacement>>();
            result.TitleList = new List<string>() { "站位信息", "料号信息", "位号信息", "用量", "飞达规格" };
            result.PlacementName = fileName;
            result.Part_NO = Part_NO;
            result.DataList = new List<AIRIPlacement>();

            try
            {
                //Regex replaceSpace = new Regex(@"\t{1,}", RegexOptions.IgnoreCase);
                string[] fileData = File.ReadAllLines(placementFilePath, Encoding.Default);
                int count = 0;
                decimal multNo = 0;
                String fistLoc = "";
                decimal locCount = 0;
                String data = "", str = "", pn = "", refDesign = "";
                for (int i = 0; i < fileData.Length; i++)
                {
                    if (String.IsNullOrEmpty(fileData[i])) { continue; }
                    data = ""; str = ""; pn = ""; refDesign = "";
                    if (fileData[i].Contains("Number of Array PCB"))
                    {
                        String[] multNoStrlist = fileData[i].Split('\t');
                        String multNoStr = multNoStrlist[1].Trim();
                        multNo = Decimal.Parse(multNoStr);
                    }
                    if (count < 1 && fileData[i].IndexOf("Tape Ref. Info") > 0)
                    {
                        count++;
                        continue;
                    }
                    if (fileData[i].IndexOf("Stick Feeder Information") > 0)
                    {
                        count = 0;
                        break;
                    }
                    if (count == 1 && !fileData[i].Trim().IsNullOrEmpty() && fileData[i].IndexOf("******") < 0)
                    {
                        count++;
                        String temData = fileData[i];
                        String refdesignator = "";
                        while (fileData[i + 1].StartsWith('\t') && fileData[i].IndexOf("Stick Feeder Information") < 0)
                        {
                            refdesignator += fileData[i + 1];
                            i++;
                        }
                        //恢复上一行
                        i--;
                        data = temData;
                        //refDesign = fileData[i + 1];
                        //replaceSpace.Replace(data, "|").Trim();
                        //F38|SM12|(0.02.03.00.0102C)M7|Total:|15|Points
                        string[] strArr = data.Split('\t');
                        AIRIPlacement placement = new AIRIPlacement();
                        placement.SLOT = strArr[0];
                        placement.FEEDER_TYPE = strArr[1];
                        string[] pnarr = strArr[2].Split(')');
                        pn = pnarr[0].Replace("(", "");
                        placement.PART_NO = pn;
                        placement.REFDESIGNATOR = String.IsNullOrEmpty(refdesignator) ? null : refdesignator.Replace("\t", "");
                        string[] loclist = String.IsNullOrEmpty(placement.REFDESIGNATOR) ? null : placement.REFDESIGNATOR.Split(',');
                        if (fistLoc == "")
                        {

                            if (loclist != null && loclist.Count() > 0)
                            {
                                fistLoc = loclist[0].Trim().ToUpper();
                            }
                        }
                        if (loclist != null && loclist.Count() > 0 && fistLoc != "")
                        {
                            foreach (String loc in loclist)
                            {
                                if (loc.Trim().ToUpper().Equals(fistLoc))
                                {
                                    locCount += 1;
                                }
                            }
                        }

                        placement.UNITQTY = Convert.ToDecimal(strArr[4]);
                        result.DataList.Add(placement);
                    }
                    else if (count == 2)
                    {
                        count = 1;
                    }
                }
                if (locCount > 0 && multNo > 0 && locCount == multNo * 2)
                {
                    var prePlaceMentList = result.DataList.Where(f => f.SLOT.StartsWith('F'));
                    foreach (AIRIPlacement placement in prePlaceMentList)
                    {
                        placement.LOCATION_KEY = "1";
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception("FILE_CONTENT_PARSING_ERROR");
            }

            return result;
        }

        public AIRIPlacementVM<List<AIRIPlacement>> LoadYMHPlacementExcel(string placementFilePath, string fileName, string Part_NO, int MultiNo)
        {
            AIRIPlacementVM<List<AIRIPlacement>> result = new AIRIPlacementVM<List<AIRIPlacement>>();
            try
            {
                result.TitleList = new List<string>() { "站位信息", "料号信息", "位号信息", "用量", "名称","飞达规格" };
                result.PlacementName = fileName;
                result.Part_NO = Part_NO;
                List<AIRIPlacement> pList = new List<AIRIPlacement>();
                Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);
                Aspose.Cells.TxtLoadOptions lo = new TxtLoadOptions();
                lo.Encoding = Encoding.GetEncoding("GB2312");
                Workbook workbook = new Workbook(placementFilePath, lo);
                Cells cells = workbook.Worksheets[0].Cells;
                System.Data.DataTable table = cells.ExportDataTable(2, 0, cells.MaxDataRow + 1, cells.MaxColumn + 1, true);

                foreach (System.Data.DataRow row in table.Rows)
                {
                    //1、站位--安装位置
                    //2、料号--元件名称，料号(规格)
                    //3、单位用量-- 根据位号查询执行状态行数
                    //4、位号--图样明，多个数据用逗号隔开
                    //5、飞达规格
                    String slot = row["安装位置"].ToString().Trim();
                    String cName = row["元件名"].ToString().Trim();
                    String refdesignator = row["图样名"].ToString().Trim();
                    String feeder_type = row["送料器类型"].ToString().Trim();
                    if (!String.IsNullOrEmpty(slot))
                    {
                        AIRIPlacement model = pList.Where(m => m.SLOT == slot)?.FirstOrDefault();

                        String part_no = cName.Split('(')[0];
                        String description = cName.Replace(part_no, "").Replace("(", "").Replace(")", "").Replace("（", "").Replace("）", "");
                        if (model == null)
                        {
                            model = new AIRIPlacement();
                            model.SLOT = slot;
                            model.PART_NO = part_no;
                            model.REFDESIGNATOR = refdesignator;
                            model.UNITQTY = 1;
                            model.DESCRIPTION = description;
                            model.FEEDER_TYPE = feeder_type;
                            model.STAGE = "1";
                            pList.Add(model);
                        }
                        else
                        {
                            refdesignator = model.REFDESIGNATOR + "," + refdesignator;
                            pList.Where(m => m.SLOT == slot).Select(x => { x.UNITQTY = model.UNITQTY + 1; x.REFDESIGNATOR = refdesignator; return x; }).ToList();
                        }
                    }
                }

                pList = pList.OrderBy(m => m.SLOT).ToList();
                result.DataList = pList;
            }
            catch (Exception ex)
            {
                throw new Exception("FILE_CONTENT_PARSING_ERROR");
            }

            return result;
        }

        /// <summary>
        /// 西门子
        /// </summary>
        /// <param name="placementFilePath"></param>
        /// <param name="fileName"></param>
        /// <param name="Part_NO"></param>
        /// <returns></returns>
        public async Task<AIRIPlacementVM<SmtPlacementByLine>> LoadSiemensPlacementExcel(string placementFilePath, string fileName, string part_NO, List<string> stationIdArray, int multiNo)
        {
            Workbook workbook = new Workbook(placementFilePath);
            Worksheet sheet = workbook.Worksheets[0];
            AIRIPlacementSetup setupPlacement = new AIRIPlacementSetup(sheet, _localizer, _repository);
            var placement = await setupPlacement.LoadSiemensPlacementExcelAsync(placementFilePath, stationIdArray, multiNo);
            AIRIPlacementVM<SmtPlacementByLine> result = new AIRIPlacementVM<SmtPlacementByLine>();
            result.TitleList = new List<string>() { "主料槽", "料号信息", "位号信息", "用量", "飞达规格", "模组", "子料槽" };
            result.PlacementName = fileName;
            result.Part_NO = part_NO;

            result.DataList = new SmtPlacementByLine();
            result.DataList.LinePlacementArrary = new List<SmtPlacementByStation>();
            foreach (var stationModel in placement.LinePlacement)
            {
                var stationVM = new SmtPlacementByStation();
                stationVM.StationID = stationModel.StationID;
                stationVM.PlacementDetailArrary = new List<AIRIPlacement>();
                foreach (var detailModle in stationModel.PlacementDetail)
                {
                    var item = new AIRIPlacement()
                    {
                        SLOT = detailModle.SLOT,
                        PART_NO = detailModle.PART_NO,
                        REFDESIGNATOR = detailModle.REFDESIGNATOR ?? string.Empty,
                        UNITQTY = detailModle.UNITQTY,
                        FEEDER_TYPE = detailModle.FEEDER_TYPE ?? string.Empty,
                        STAGE = detailModle.TABLE_NO,
                        SUBSLOT = detailModle.SUB_SLOT,
                    };
                    stationVM.PlacementDetailArrary.Add(item);
                }
                result.DataList.LinePlacementArrary.Add(stationVM);
            }
            return result;
        }
        public async Task<bool> LoadBom(string product_no, BomType bomtype)
        {

            string bom_id = "";
            try
            {
                string Type = "";
                if (bomtype == BomType.SMT组件)
                {
                    Type = "SMT";
                }
                else if (bomtype == BomType.AI组件)
                {
                    Type = "AI";
                }
                else if (bomtype == BomType.RI组件)
                {
                    Type = "RI";
                }
                else if (bomtype == BomType.三星组件)
                {
                    Type = "SAMSUNG";
                }
                else if (bomtype == BomType.西门子组件)
                {
                    Type = "SIEMENS";
                }
                #region 旧方法
                //同步ERP的数据到SMT_BOM1、STM_BOM2
                //bom_id = await _repository.SyncBomByProdectId(product_no, Type, userName);
                //if (bom_id == "")
                //{
                //    //Messenger.Hint("未查询到当前成品料号的料单，请确定在ERP中是否存在!", true);
                //    Set(_localizer["nofind_product_no"]);
                //} 
                #endregion

                //读取配置
                var builder = new ConfigurationBuilder().SetBasePath(Directory.GetCurrentDirectory())
                                                                .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true);

                var config = builder.Build();

                string postUrl = config["ERPBOMService:Url"] + "?ProductId=" + product_no + "&Type=" + Type + "&userName=" + userName;
                HttpWebRequest webReq = (HttpWebRequest)WebRequest.Create(new Uri(postUrl));
                webReq.Method = "Get";
                webReq.ContentLength = 0;
                //webReq.ContentType = "application";
                WebResponse response = (HttpWebResponse)webReq.GetResponse();
                StreamReader sr = new StreamReader(response.GetResponseStream(), Encoding.Default);
                JObject result = JObject.Parse(sr.ReadToEnd());
                if (result.IsNullOrEmpty())
                {
                    Set("SyncBomByProdectId Not Data");
                }
                if (result.GetValue("Code").Value<int>() == 1)
                {
                    String data = result.GetValue("Data").Value<String>();
                    if (data.IsNullOrEmpty())
                    {
                        //Messenger.Hint("未查询到当前成品料号的料单，请确定在ERP中是否存在!", true);
                        Set(_localizer["nofind_product_no"]);
                    }
                    else
                    {
                        bom_id = data;
                    }
                    BomInfo = await _repository.ExploreBom2(bom_id, bomtype);

                    return true;
                }
                else
                {
                    throw new Exception();
                }
            }
            catch (Exception)
            {
                //Messenger.Hint("同步ERP的BOM到MES系统失败，请仔细核对料单的数据是否正确!", true);
                //Set(_localizer["sync_bom_fall"]);
            }

            return false;
        }

        public async Task<bool> LoadBomEX(string product_no, BomType bomtype)
        {

            string bom_id = "";
            try
            {
                string Type = "";
                if (bomtype == BomType.SMT组件)
                {
                    Type = "SMT";
                }
                else if (bomtype == BomType.AI组件)
                {
                    Type = "AI";
                }
                else if (bomtype == BomType.RI组件)
                {
                    Type = "RI";
                }
                else if (bomtype == BomType.三星组件)
                {
                    Type = "SAMSUNG";
                }
                else if (bomtype == BomType.西门子组件)
                {
                    Type = "SIEMENS";
                }
                else if (bomtype == BomType.雅码哈)
                {
                    Type = "YAMAHA";
                }

                //读取配置
                var builder = new ConfigurationBuilder().SetBasePath(Directory.GetCurrentDirectory())
                                                            .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true);
                var config = builder.Build();
                string postUrl = config["ERPBOMService:Url"] + "?ProductId=" + product_no + "&Type=" + Type + "&userName=" + userName;
                HttpWebRequest webReq = (HttpWebRequest)WebRequest.Create(new Uri(postUrl));
                webReq.Method = "Get";
                webReq.ContentLength = 0;
                WebResponse response = (HttpWebResponse)webReq.GetResponse();
                StreamReader sr = new StreamReader(response.GetResponseStream(), Encoding.Default);
                JObject result = JObject.Parse(sr.ReadToEnd());
                if (result.IsNullOrEmpty())
                {
                    Set("SyncBomByProdectId Not Data");
                }
                if (result.GetValue("Code").Value<int>() == 1)
                {
                    String data = result.GetValue("Data").Value<String>();
                    if (data.IsNullOrEmpty())
                    {
                        //Messenger.Hint("未查询到当前成品料号的料单，请确定在ERP中是否存在!", true);
                        Set(_localizer["nofind_product_no"]);
                    }
                    else
                    {
                        bom_id = data;
                    }
                    BomInfo = await _repository.ExploreBom2(bom_id, bomtype);

                    return true;
                }
                else
                {
                    String data = result.GetValue("Data").Value<String>();
                    throw new Exception(data);
                }
            }
            catch (Exception ex)
            {
                //Messenger.Hint("同步ERP的BOM到MES系统失败，请仔细核对料单的数据是否正确!", true);
                Set(ex.Message);
            }

            return false;
        }

        /// <summary>
        /// 同步制造BOM
        /// </summary>
        /// <param name="product_no">产品料号</param>
        /// <param name="wo_no">工单</param>
        /// <returns></returns>
        public async Task<bool> LoadBomEXbyWONo(string product_no, string wo_no)
        {

            string bom_id = "";
            try
            {

                //读取配置
                var builder = new ConfigurationBuilder().SetBasePath(Directory.GetCurrentDirectory())
                                                            .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true);
                var config = builder.Build();
                string postUrl = config["ERPMBOMService:MBOMUrl"] + "?productId=" + product_no + "&woNo=" + wo_no + "&userName=" + userName;
                HttpWebRequest webReq = (HttpWebRequest)WebRequest.Create(new Uri(postUrl));
                webReq.Method = "Get";
                webReq.ContentLength = 0;
                WebResponse response = (HttpWebResponse)webReq.GetResponse();
                StreamReader sr = new StreamReader(response.GetResponseStream(), Encoding.Default);
                JObject result = JObject.Parse(sr.ReadToEnd());
                if (result.IsNullOrEmpty())
                {
                    Set("SyncBomByProdectId Not Data");
                }
                if (result.GetValue("Code").Value<int>() == 1)
                {
                    String data = result.GetValue("Data").Value<String>();
                    if (data.IsNullOrEmpty())
                    {
                        //Messenger.Hint("未查询到当前成品料号的料单，请确定在ERP中是否存在!", true);
                        Set(_localizer["nofind_product_no"]);
                    }
                    else
                    {
                        bom_id = data;
                    }
                    BomInfo = await _repository.ExploreBom2(bom_id, BomType.ALL);
                    return true;
                }
                else
                {
                    String data = result.GetValue("Data").Value<String>();
                    throw new Exception(data);
                }
            }
            catch (Exception ex)
            {
                //Messenger.Hint("同步ERP的BOM到MES系统失败，请仔细核对料单的数据是否正确!", true);
                Set(ex.Message);
            }
            return false;
        }

        /// <summary>
        /// BOM料单比对
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public CompareResult CompareByBom(CompareByBomModel model)
        {
            CompareResult returnVM = new CompareResult();
            JArray json = model.PlacementList; //JArray.Parse(
            string msg_info = string.Empty;
            string partNo = string.Empty;
            string position = string.Empty;
            decimal unitQty = 0m;
            List<string> result = new List<string>();
            List<string> bomPosition = new List<string>();
            List<string> fePosition = new List<string>();
            if (model.BOMDataList != null)
            {
                foreach (var row in model.BOMDataList)
                {
                    bool pnFound = false;
                    if (row.COMPONENT_LOCATION == null || row.COMPONENT_LOCATION.IsNullOrEmpty())
                    {
                        row.IS_OK = true;
                        continue;
                    }
                    if (row.UNIT_CODE == null || row.UNIT_QTY == 0)
                    {
                        row.IS_OK = true;
                        continue;
                    }
                    foreach (JObject item in json)
                    {
                        JToken[] ojb = item.PropertyValues().ToArray();
                        if (ojb == null || ojb[0] == null) continue;

                        partNo = ojb[1].ToString();
                        position = ojb[2].ToString().ToUpper();
                        unitQty = decimal.Parse(ojb[3].ToString());
                        if (partNo != row.PART_CODE) continue;
                        pnFound = true;

                        bomPosition.Clear();
                        fePosition.Clear();
                        bomPosition.AddRange((row.COMPONENT_LOCATION == null ? "" : row.COMPONENT_LOCATION.ToUpper().Trim()).Split(BomExploreManager.splitChar).OrderBy(f => f));
                        fePosition.AddRange(position.Trim().Split(BomExploreManager.splitChar).OrderBy(f => f));
                        bool hasError = false;
                        if (bomPosition.Count != fePosition.Count || unitQty % row.UNIT_QTY != 0)
                        {
                            //"料号:{0}的单位用量与BOM中的不一致"
                            msg_info = string.Format(_localizer["unit_qty_disaccord"], partNo);
                            result.Add(msg_info);
                            hasError = true;
                            continue;
                        }

                        for (int d = 0; d < bomPosition.Count; d++)
                        {
                            if (bomPosition[d] != fePosition[d])
                            {
                                if (fePosition[d].Contains('-'))
                                {
                                    if (bomPosition[d] == fePosition[d].Split('-')[0])
                                    {
                                        continue;
                                    }
                                }

                                hasError = true;
                                break;
                            }
                        }

                        if (hasError)
                        {
                            //"料号:{0}的贴片位号与BOM中的贴片位号存在差异"
                            msg_info = string.Format(_localizer["part_code_disaccord"], row.PART_CODE);
                            result.Add(msg_info);
                            continue;
                        }
                        row.IS_OK = true;
                    }

                    if (pnFound == false)
                    {
                        //料号:{0}在FE料单中未找到
                        msg_info = string.Format(_localizer["part_code_nofind_FE"], row.PART_CODE);
                        result.Add(msg_info);
                    }
                }

                foreach (JObject item in json)
                {
                    JToken[] ojb = item.PropertyValues().ToArray();
                    if (ojb == null || ojb[0] == null) continue;
                    partNo = ojb[1].ToString();

                    if (model.BOMDataList.Count(f => f.PART_CODE == partNo) <= 0)
                    {
                        //"料号:{0}沒有出現在BOM中!"
                        msg_info = string.Format(_localizer["part_code_nofind_bom"], partNo);
                        result.Add(msg_info);
                    }
                }
            }

            if (result.Count > 0)
            {
                returnVM.Result = false;
                returnVM.ResultMsg = result;
            }
            else if (model.BOMDataList != null)
            {
                returnVM.Result = true;
            }
            else
            {
                returnVM.Result = false;
                returnVM.ResultMsg = new List<string> { "没有BOM." };
            }

            return returnVM;
        }


        /// <summary>
        /// BOM料单比对(不比对位号)
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public CompareResult CompareNoLocationByBom(CompareByBomModel model)
        {
            CompareResult returnVM = new CompareResult();
            JArray json = model.PlacementList; //JArray.Parse(
            string msg_info = string.Empty;
            string partNo = string.Empty;
            string position = string.Empty;
            decimal unitQty = 0m;
            List<string> result = new List<string>();

            if (model.BOMDataList != null)
            {
                if (json != null && json.Count > 0)
                {
                    foreach (JObject item in json)
                    {
                        JToken[] ojb = item.PropertyValues().ToArray();
                        if (ojb == null || ojb[0] == null) continue;

                        partNo = ojb[1].ToString();
                        //position = ojb[2].ToString().ToUpper();
                        unitQty = decimal.Parse(ojb[3].ToString());
                        bool pnFound = false;
                        foreach (var row in model.BOMDataList)
                        {
                            row.IS_OK = false;

                            if (partNo != row.PART_CODE) continue;
                            pnFound = true;

                            //bool hasError = false;
                            if (unitQty % row.UNIT_QTY != 0)
                            {
                                //"料号:{0}的单位用量与BOM中的不一致"
                                msg_info = string.Format(_localizer["unit_qty_disaccord"], partNo);
                                result.Add(msg_info);
                                // hasError = true;
                                continue;
                            }

                            //if (hasError)
                            //{
                            //    //"料号:{0}的贴片位号与BOM中的贴片位号存在差异"
                            //    msg_info = string.Format(_localizer["part_code_disaccord"], row.PART_CODE);
                            //    result.Add(msg_info);
                            //    continue;
                            //}
                            row.IS_OK = true;
                        }

                        if (pnFound == false)
                        {
                            //"料号:{0}沒有出現在BOM中!"
                            msg_info = string.Format(_localizer["part_code_nofind_bom"], partNo);
                            result.Add(msg_info);
                        }
                    }
                }

            }

            if (result.Count > 0)
            {
                returnVM.Result = false;
                returnVM.ResultMsg = result;
            }
            else if (model.BOMDataList != null)
            {
                returnVM.Result = true;
            }
            else
            {
                returnVM.Result = false;
                returnVM.ResultMsg = new List<string> { "没有BOM." };
            }

            return returnVM;
        }
    }
}
