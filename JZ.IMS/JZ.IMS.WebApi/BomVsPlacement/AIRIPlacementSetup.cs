using Aspose.Cells;
using JZ.IMS.Core.Extensions;
using JZ.IMS.Core.Utilities;
using JZ.IMS.IRepository;
using JZ.IMS.Models;
using JZ.IMS.ViewModels.BomVsPlacement;
using Microsoft.Extensions.Localization;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace JZ.IMS.WebApi.Controllers.BomVsPlacement
{
    public class AIRIPlacementSetup
    {
        const int slotIDX = 0;
        const int pnIDX = 1;
        const int descIDX = 2;
        const int feederTypeIDX = 3;
        const int sizeIDX = 3;
        const int refDesignIDX = 6;
        const int unitQtyIDX = 4;

        const string KeyOfSlot = "序号"; //A4
        const string KeyOfDesc = "元件品名";//C4
        const string KeyOfPN = "元件品号"; //B4
        const string KeyOfFeederType = "元件规格";//D4
        const string KeyOfDirection = "备注"; //方向，H4
        const string KeyOfRefDesign = "插件位置";//G4
        const string KeyOfUnitQty = "单位用量"; //E4

        private decimal pcbSide = 1;
        public decimal PcbSide
        {
            get { return this.pcbSide; }
            set { this.pcbSide = value; }
        }
        //private decimal pcbRouteCode = 0;
        private decimal standardCapacity = 0;
        private decimal placementHeaderID = 0;
        private Worksheet excel;

        private string placementName;
        public string PlacementName
        {
            get
            {
                return this.placementName;
            }
            set
            {
                this.placementName = value;
            }
        }

        /// <summary>
        /// 料号
        /// </summary>
        public string Part_NO { get; set; }

        private string placementDescription;

        public string PlacementDescription
        {
            get
            {
                return this.placementDescription;
            }
            set
            {
                this.placementDescription = value;
            }
        }

        private string partNumber;
        public string PartNumber
        {
            get
            {
                return this.partNumber;
            }
            set
            {
                this.partNumber = value;
            }
        }

        private decimal smtStationID;
        public decimal SMTStationID
        {
            get
            {
                return this.smtStationID;
            }
            private set
            {
                this.smtStationID = value;
            }
        }

        #region constructor

        private readonly IStringLocalizer<BomVsPlacementController> _localizer;
        private readonly IBomVsPlacementRepository _repository;
        public AIRIPlacementSetup(Worksheet excelControl, IStringLocalizer<BomVsPlacementController> localizer, IBomVsPlacementRepository repository = null)
        {
            this.excel = excelControl;
            _localizer = localizer;
            _repository = repository;
        }

        #endregion

        /// <summary>
        /// 清除
        /// </summary>
        public void Clear()
        {
            this.placementName = string.Empty;
            this.partNumber = string.Empty;
            this.smtStationID = 0;
            this.placementHeaderID = 0;
        }

        public List<SmtPlacementTemp> LoadAIRIPlacement()
        {
            object tmpValue;

            string slot = "";
            string pn = "";
            string desc = "";
            string feederType = "";
            string size = "";
            string refDesign = "";
            string no = "";
            decimal unitQty = 0;
            string unitQtyStringValue = string.Empty;
            string location = "";

            string k_Slot = ""; //A4
            string k_Desc = "";//C4
            string k_PN = ""; //B4
            string k_FeederType = "";//D4
            string k_Direction = ""; //H4
            string k_RefDesign = "";//G4
            string k_UnitQty = ""; //E4

            Worksheet sheet = this.excel;
            var yamahaPlacement = new List<SmtPlacementTemp>();

            int startRow = -1;
            tmpValue = sheet.Cells["A4"].Value;
            if (tmpValue != null)
                k_Slot = tmpValue.ToString().Trim();
            tmpValue = sheet.Cells["C4"].Value;
            if (tmpValue != null)
                k_Desc = tmpValue.ToString().Trim();
            tmpValue = sheet.Cells["B4"].Value;
            if (tmpValue != null)
                k_PN = tmpValue.ToString().Trim();
            tmpValue = sheet.Cells["D4"].Value;
            if (tmpValue != null)
                k_FeederType = tmpValue.ToString().Trim();
            tmpValue = sheet.Cells["H4"].Value;
            if (tmpValue != null)
                k_Direction = tmpValue.ToString().Trim();
            tmpValue = sheet.Cells["G4"].Value;
            if (tmpValue != null)
                k_RefDesign = tmpValue.ToString().Trim();
            tmpValue = sheet.Cells["E4"].Value;
            if (tmpValue != null)
                k_UnitQty = tmpValue.ToString().Trim();

            if (k_Slot == KeyOfSlot &&
            k_Desc == KeyOfDesc &&
            k_PN == KeyOfPN &&
            k_FeederType == KeyOfFeederType &&
            k_Direction == KeyOfDirection &&
            k_UnitQty == KeyOfUnitQty &&
            k_RefDesign == KeyOfRefDesign
            )
            {
                startRow = 4;
                this.placementDescription = sheet.Cells["C2"].Value.ToString().Trim();
                this.placementName = sheet.Cells["C3"].Value.ToString().Trim();
                this.partNumber = sheet.Cells["C1"].Value.ToString().Trim();
                this.Part_NO = sheet.Cells["C1"].Value.ToString().Trim();
            }
            else
            {
                //Messenger.Error("请选择正确的AI/RI料单文件!");
                throw new Exception(_localizer["AIRI_file_error"]);
                return yamahaPlacement;
            }

            int continueNullRowCount = 0;

            for (int i = startRow; i < sheet.Cells.Rows.Count; i++)
            {
                tmpValue = sheet.Cells[i, slotIDX].Value;
                if (tmpValue != null)
                    slot = tmpValue.ToString().Trim();
                no = slot;
                tmpValue = sheet.Cells[i, descIDX].Value;
                if (tmpValue != null)
                    desc = tmpValue.ToString().Trim();
                tmpValue = sheet.Cells[i, pnIDX].Value;
                if (tmpValue != null)
                    pn = tmpValue.ToString().Trim();
                tmpValue = sheet.Cells[i, feederTypeIDX].Value;
                if (tmpValue != null)
                    feederType = tmpValue.ToString().Trim();
                size = feederType;
                tmpValue = sheet.Cells[i, refDesignIDX].Value;
                if (tmpValue != null)
                    refDesign = tmpValue.ToString().Trim();
                tmpValue = sheet.Cells[i, unitQtyIDX].Value;
                if (tmpValue != null)
                    unitQtyStringValue = tmpValue.ToString().Trim();

                tmpValue = sheet.Cells[i, 7].Value;
                if (tmpValue != null)
                    location = tmpValue.ToString().Trim();

                if (continueNullRowCount > 2) break;

                if (string.IsNullOrEmpty(slot))
                {
                    continueNullRowCount += 1;
                    continue;
                    //yamahaPlacement.Clear();
                    //Messenger.Error(string.Format("第{0}行的【站位】为空,料单导入失败!", i + 1));
                    //break;
                }
                if (string.IsNullOrEmpty(pn))
                {
                    continueNullRowCount += 1;
                    continue;
                    //yamahaPlacement.Clear();
                    //Messenger.Error(string.Format("第{0}行的【编码】为空,料单导入失败!", i + 1));
                    //break;
                }
                if (string.IsNullOrEmpty(refDesign))
                {
                    //yamahaPlacement.Clear();
                    //Messenger.Error(string.Format("第{0}行的【位置】为空,料单导入失败!", i + 1));
                    //break;
                    continue;
                }

                unitQty = Convert.ToDecimal(unitQtyStringValue);
                if (unitQty <= 0)
                {
                    yamahaPlacement.Clear();
                    //Messenger.Error(string.Format("第{0}行的【用量】小于等于0,料单导入失败!", i + 1));
                    string errmsg = string.Format(string.Format(_localizer["AIRI_file_import_error"], i + 1));
                    throw new Exception(errmsg);
                    break;
                }
                continueNullRowCount = 0;
                SmtPlacementTemp newRow = new SmtPlacementTemp();
                newRow.SLOT = slot;
                newRow.SUB_SLOT = " ";
                newRow.PART_NO = pn;
                newRow.DESCRIPTION = desc;
                newRow.FEEDER_TYPE = feederType;
                newRow.SIZE = size;
                newRow.NO = no;
                newRow.REFDESIGNATOR = refDesign;
                newRow.UNITQTY = unitQty;
                newRow.LOCATION = location;
                yamahaPlacement.Add(newRow);

                slot = string.Empty;
                pn = string.Empty;
                desc = string.Empty;
                feederType = string.Empty;
                size = string.Empty;
                no = string.Empty;
                refDesign = string.Empty;
            }
            return yamahaPlacement;
        }

        public List<SmtPlacementTemp> LoadRIPlacement()
        {
            object tmpValue;

            string slot = "";
            string pn = "";
            string desc = "";
            string feederType = "";
            string size = "";
            string refDesign = "";
            string no = "";
            decimal unitQty = 0;
            string unitQtyStringValue = string.Empty;
            string location = "";

            const int slotIndex = 9;
            const int pnIndex = 6;
            const int feederTypeIndex = 7;
            const int refDesignIndex = 1;
            Worksheet sheet = this.excel;
            var yamahaPlacement = new List<SmtPlacementTemp>();

            int startRow = 2;
            int continueNullRowCount = 0;

            for (int i = startRow; i < sheet.Cells.Rows.Count; i++)
            {
                tmpValue = sheet.Cells[i, slotIndex].Value;
                if (tmpValue != null)
                    slot = tmpValue.ToString().Trim();
                no = slot;
                tmpValue = sheet.Cells[i, pnIndex].Value;
                if (tmpValue != null)
                    //pn = tmpValue.ToString().Trim();
                    pn = Regex.Replace(tmpValue.ToString(), @"(.*\()(.*)(\).*)", "$2");
                tmpValue = sheet.Cells[i, feederTypeIndex].Value;
                if (tmpValue != null)
                    feederType = tmpValue.ToString().Trim();
                size = feederType;
                tmpValue = sheet.Cells[i, refDesignIndex].Value;
                if (tmpValue != null)
                    refDesign = tmpValue.ToString().Trim();


                continueNullRowCount = 0;
                SmtPlacementTemp newRow = new SmtPlacementTemp();
                newRow.SLOT = string.IsNullOrWhiteSpace(slot) ? null : slot;
                newRow.PART_NO = string.IsNullOrWhiteSpace(pn) ? null : pn;
                newRow.FEEDER_TYPE = string.IsNullOrWhiteSpace(feederType) ? null : feederType;
                newRow.REFDESIGNATOR = string.IsNullOrWhiteSpace(refDesign) ? null : refDesign;
                yamahaPlacement.Add(newRow);


                slot = string.Empty;
                pn = string.Empty;
                desc = string.Empty;
                feederType = string.Empty;
                size = string.Empty;
                no = string.Empty;
                refDesign = string.Empty;
            }
            var newPlacement = new List<SmtPlacementTemp>();
            if (yamahaPlacement != null && yamahaPlacement.Count > 0)
            {
                var PlacementGroup = yamahaPlacement.GroupBy(c => new { c.SLOT, c.PART_NO }).ToList();
                foreach (var groupitem in PlacementGroup)
                {
                    if (!string.IsNullOrEmpty(groupitem.ToList()[0].SLOT))
                    {
                        var temp = new SmtPlacementTemp();

                        var refNo = string.Join(",", groupitem.Select(c => c.REFDESIGNATOR).Distinct());
                        temp.REFDESIGNATOR = refNo;
                        temp.SLOT = groupitem.ToList()[0].SLOT;
                        temp.PART_NO = groupitem.ToList()[0].PART_NO;
                        temp.UNITQTY = groupitem.Count();
                        temp.FEEDER_TYPE = groupitem.ToList()[0].FEEDER_TYPE;
                        newPlacement.Add(temp);
                    }

                }
            }
            return newPlacement;
        }

        /// <summary>
        /// 处理三星TXT
        /// </summary>
        /// <param name="filePath"></param>
        /// <returns></returns>
        public List<SmtPlacementTemp> LoadGalaxyPlacement(string filePath)
        {
            var tmp = string.Empty;

            #region 读取文件
            string[] str = File.ReadAllLines(filePath, Encoding.UTF8);//需要编码
            ArrayList list = new ArrayList();
            List<string> temptitle = new List<string>();
            List<SmtPlacementTemp> yamahaPlacement = new List<SmtPlacementTemp>();
            for (int i = 0; i < str.Length; i++)
            {
                if (i == 0)
                {
                    continue;
                }
                if (i == 1)
                {
                    tmp = System.Text.RegularExpressions.Regex.Replace(str[i], @"\s+", "&&");
                    temptitle = tmp.Split(@"&&").ToList();
                    tmp = string.Empty;
                    continue;
                }
                tmp = System.Text.RegularExpressions.Regex.Replace(str[i], @"\s+", "&&");
                var contentlist = tmp.Split(@"&&").ToList();
                SmtPlacementTemp newRow = new SmtPlacementTemp();
                if (!string.IsNullOrWhiteSpace(str[i].ToString().Trim()))
                {
                    for (int j = 0; j < temptitle.Count; j++)
                    {
                        switch (temptitle[j])
                        {
                            case "No.":
                                newRow.SLOT = contentlist[j];
                                break;
                            case "TYPE":
                                newRow.TYPE = contentlist[j];
                                break;
                            case "PART.NAME":
                                newRow.PART_NO = contentlist[j];
                                break;
                            case "PART.GROUP":
                                newRow.FEEDER_TYPE = contentlist[j];
                                break;
                            case "REFERENCES":
                                newRow.REFDESIGNATOR = contentlist[j];
                                break;
                            case "TOTAL":
                                var unitQty = Convert.ToDecimal(contentlist[j]);
                                if (unitQty <= 0)
                                {
                                    //yamahaPlacement.Clear();
                                    //Messenger.Error(string.Format("第{0}行的【用量】小于等于0,料单导入失败!", i + 1));
                                    //string errmsg = string.Format(string.Format(_localizer["AIRI_file_import_error"], i + 1));
                                    // throw new Exception(errmsg);
                                    continue;
                                }
                                newRow.UNITQTY = unitQty;
                                break;
                            case "SKIP":
                                break;
                            default:
                                //Messenger.Error("请选择正确的料单文件!");
                                throw new Exception(_localizer["incorrectness_placement_file"]);
                                break;
                        }
                    }
                    yamahaPlacement.Add(newRow);
                    newRow = null;
                }
                else
                {
                    break;
                }
            }
            #endregion

            return yamahaPlacement;
        }
        /// <summary>
        /// Excel导入三星基本方式
        /// </summary>
        /// <returns></returns>
        public List<SmtPlacementTemp> BaseLoadGalaxyPlacementExcel(string filePath,int multiNo=1)
        {
            object tmpValue;
            const int slotIndex = 0;
            const int pnIndex = 2;
            const int feederTypeIndex = 1;
            const int refDesignIndex = 3;
            const int unitQtyIndex = 4;

            string slot = "";
            string pn = "";
            string feederType = "";
            string size = "";
            string refDesign = "";
            string no = "";
            decimal unitQty = 0;
            string unitQtyStringValue = string.Empty;

            Worksheet sheet = this.excel;
            var yamahaPlacement = new List<SmtPlacementTemp>();

            int startRow = 3;

            int continueNullRowCount = 0;

            for (int i = startRow; i < sheet.Cells.Rows.Count; i++)
            {
                tmpValue = sheet.Cells[i, slotIndex].Value;
                if (tmpValue != null)
                {
                    if (!tmpValue.ToString().StartsWith('F') && !tmpValue.ToString().StartsWith('R') && !tmpValue.ToString().StartsWith('T'))
                    {
                        continue;
                    }
                    else
                    {
                        if (tmpValue.ToString().Contains('('))
                        {
                            tmpValue = tmpValue.ToString().Split('(', StringSplitOptions.RemoveEmptyEntries)[0].ToString();
                        }
                        slot = tmpValue.ToString();
                    }
                }
                else
                {
                    break;
                }

                no = slot;
                tmpValue = sheet.Cells[i, pnIndex].Value;
                if (tmpValue != null)
                    pn = Regex.Replace(tmpValue.ToString().Trim(), @"(.*\()(.*)(\).*)", "$2");
                //pn = GetValue(tmpValue.ToString().Trim(),"(",")");
                tmpValue = sheet.Cells[i, feederTypeIndex].Value;
                if (tmpValue != null)
                    feederType = tmpValue.ToString().Trim();
                size = feederType;
                tmpValue = sheet.Cells[i, refDesignIndex].Value;
                if (tmpValue != null)
                    refDesign = tmpValue.ToString().Trim();
                tmpValue = sheet.Cells[i, unitQtyIndex].Value;
                if (tmpValue != null)
                {
                    if (tmpValue.ToString().Contains(':'))
                    {
                        tmpValue = tmpValue.ToString().Split(':', StringSplitOptions.RemoveEmptyEntries)[1].Split(')', StringSplitOptions.RemoveEmptyEntries)[0].ToString();
                    }
                    unitQtyStringValue = tmpValue.ToString().Trim();
                }


                if (continueNullRowCount > 2) break;

                if (string.IsNullOrEmpty(slot))
                {
                    continueNullRowCount += 1;
                    continue;
                    //yamahaPlacement.Clear();
                    //Messenger.Error(string.Format("第{0}行的【站位】为空,料单导入失败!", i + 1));
                    //break;
                }
                if (string.IsNullOrEmpty(pn))
                {
                    continueNullRowCount += 1;
                    continue;
                    //yamahaPlacement.Clear();
                    //Messenger.Error(string.Format("第{0}行的【编码】为空,料单导入失败!", i + 1));
                    //break;
                }
                if (string.IsNullOrEmpty(refDesign))
                {
                    //yamahaPlacement.Clear();
                    //Messenger.Error(string.Format("第{0}行的【位置】为空,料单导入失败!", i + 1));
                    //break;
                    continue;
                }

                unitQty = Convert.ToDecimal(unitQtyStringValue);
                if (unitQty <= 0)
                {
                    //yamahaPlacement.Clear();
                    //Messenger.Error(string.Format("第{0}行的【用量】小于等于0,料单导入失败!", i + 1));
                    // string errmsg = string.Format(string.Format(_localizer["AIRI_file_import_error"], i + 1));
                    // throw new Exception(errmsg);
                    continue;
                }
                continueNullRowCount = 0;
                SmtPlacementTemp newRow = new SmtPlacementTemp();
                newRow.SLOT = slot;
                newRow.PART_NO = pn;
                newRow.FEEDER_TYPE = feederType;
                newRow.REFDESIGNATOR = refDesign;
                newRow.UNITQTY = unitQty* multiNo;
                yamahaPlacement.Add(newRow);

                slot = string.Empty;
                pn = string.Empty;
                feederType = string.Empty;
                size = string.Empty;
                no = string.Empty;
                refDesign = string.Empty;
            }
            return yamahaPlacement;
        }

        /// <summary>
        /// 获取开始和结束字符中间的字符
        /// </summary>
        /// <param name="str">传入字符</param>
        /// <param name="s">开始字符</param>
        /// <param name="e">结束字符</param>
        /// <returns></returns>
        public string GetValue(string str, string s, string e)
        {
            Regex rg = new Regex("(?<=(" + s + "))[.\\s\\S]*?(?=(" + e + "))", RegexOptions.Multiline | RegexOptions.Singleline);
            return rg.Match(str).Value;
        }

        /// <summary>
        /// Excel导入三星
        /// </summary>
        /// <returns></returns>
        public List<SmtPlacementTemp> LoadGalaxyPlacementExcel(string filePath)
        {
            object tmpValue;
            const int slotIndex = 0;
            const int pnIndex = 2;
            const int feederTypeIndex = 3;
            const int refDesignIndex = 5;
            const int unitQtyIndex = 6;

            string slot = "";
            string pn = "";
            string feederType = "";
            string size = "";
            string refDesign = "";
            string no = "";
            decimal unitQty = 0;
            string unitQtyStringValue = string.Empty;


            Worksheet sheet = this.excel;
            var yamahaPlacement = new List<SmtPlacementTemp>();

            int startRow = 2;

            int continueNullRowCount = 0;

            for (int i = startRow; i < sheet.Cells.Rows.Count; i++)
            {

                tmpValue = sheet.Cells[i, slotIndex].Value;
                if (tmpValue != null)
                    slot = tmpValue.ToString().Trim();
                no = slot;
                tmpValue = sheet.Cells[i, pnIndex].Value;
                if (tmpValue != null)
                    pn = tmpValue.ToString().Trim();
                tmpValue = sheet.Cells[i, feederTypeIndex].Value;
                if (tmpValue != null)
                    feederType = tmpValue.ToString().Trim();
                size = feederType;
                tmpValue = sheet.Cells[i, refDesignIndex].Value;
                if (tmpValue != null)
                    refDesign = tmpValue.ToString().Trim();
                tmpValue = sheet.Cells[i, unitQtyIndex].Value;
                if (tmpValue != null)
                    unitQtyStringValue = tmpValue.ToString().Trim();

                if (continueNullRowCount > 2) break;

                if (string.IsNullOrEmpty(slot))
                {
                    continueNullRowCount += 1;
                    continue;
                    //yamahaPlacement.Clear();
                    //Messenger.Error(string.Format("第{0}行的【站位】为空,料单导入失败!", i + 1));
                    //break;
                }
                if (string.IsNullOrEmpty(pn))
                {
                    continueNullRowCount += 1;
                    continue;
                    //yamahaPlacement.Clear();
                    //Messenger.Error(string.Format("第{0}行的【编码】为空,料单导入失败!", i + 1));
                    //break;
                }
                if (string.IsNullOrEmpty(refDesign))
                {
                    //yamahaPlacement.Clear();
                    //Messenger.Error(string.Format("第{0}行的【位置】为空,料单导入失败!", i + 1));
                    //break;
                    continue;
                }

                unitQty = Convert.ToDecimal(unitQtyStringValue);
                if (unitQty <= 0)
                {
                    //yamahaPlacement.Clear();
                    //Messenger.Error(string.Format("第{0}行的【用量】小于等于0,料单导入失败!", i + 1));
                    // string errmsg = string.Format(string.Format(_localizer["AIRI_file_import_error"], i + 1));
                    // throw new Exception(errmsg);
                    continue;
                }
                continueNullRowCount = 0;
                SmtPlacementTemp newRow = new SmtPlacementTemp();
                newRow.SLOT = slot;
                newRow.PART_NO = pn;
                newRow.FEEDER_TYPE = feederType;
                newRow.REFDESIGNATOR = refDesign;
                newRow.UNITQTY = unitQty;
                yamahaPlacement.Add(newRow);

                slot = string.Empty;
                pn = string.Empty;
                feederType = string.Empty;
                size = string.Empty;
                no = string.Empty;
                refDesign = string.Empty;
            }
            return yamahaPlacement;
        }

        /// <summary>
        /// Excel导入西门子
        /// </summary>
        /// <returns></returns>
        public async Task<SmtPlacementTempByLine> LoadSiemensPlacementExcelAsync(string filePath,List<string> stationIdArray,int multiNo=1)
        {
            var linePlacement = new SmtPlacementTempByLine();
            linePlacement.LinePlacement = new List<SmtPlacementTempByStation>();

            try
            {
                object tmpValue;
                string stageNo = "B";//B20
                string typeName = "D";//D22 飞达类型 供料器类型
                string slotNo = "A";//A24 料槽
                string SubsoltNo = "L";//L24 子料槽 供料器类型
                string SubsoltNoEx = "M";//M24 子料槽 料盘
                string fPNNO = "M";//M24 飞达对应 零件料号
                string lpPNNO = "X";//X319 料盘的 零件料号
                string unitNo = "BC";//BC24 单位用量
                string stationNo = "B";//机台

                int type = 1;//1为供料器类型 2为料盘
                string slot = "";
                string stage = "";
                string subslot = "";
                string pn = "";
                string feederType = "";
                string size = "";
                string refDesign = "";
                string no = "";
                string stationID = "";
                decimal unitQty = 0;
                string unitQtyStringValue = string.Empty;
                Worksheet sheet = this.excel;

                int startRow = 19;
                int continueNullRowCount = 0;
                for (int i = startRow; i < sheet.Cells.Rows.Count; i++)
                {
                    //获取机台
                    tmpValue = sheet.Cells[stationNo + i].Value;
                    if (tmpValue != null && !tmpValue.ToString().Contains(":"))
                    {
                        tmpValue = tmpValue.ToString().Trim();
                        var station = await _repository.GetStationConfig(tmpValue.ToString().Trim(), stationIdArray);
                        if (station==null)
                        {
                            stationID = "";
                            continue;
                        }
                        stationID = station == null ? "" : station.STATION_ID.ToString();
                        var stationModel = new SmtPlacementTempByStation();
                        stationModel.StationID = stationID;
                        stationModel.PlacementDetail = new List<SmtPlacementTemp>();
                        if (linePlacement.LinePlacement.Count(c => c.StationID == stationModel.StationID) <= 0)
                        {
                            linePlacement.LinePlacement.Add(stationModel);
                        }
                        continue;
                    }

                    //获取模组
                    tmpValue = sheet.Cells[stageNo + i].Value;
                    if (tmpValue != null && tmpValue.ToString().Contains(":"))
                    {
                        stage = tmpValue.ToString().Split('-', StringSplitOptions.RemoveEmptyEntries)[0].Trim();
                        i++;
                        continue;
                    }

                    tmpValue = sheet.Cells[typeName + i].Value;
                    if (tmpValue != null && tmpValue.ToString().Trim().Contains("料盘"))
                    {
                        type = 2;
                        i++;
                        continue;
                    }
                    else if (tmpValue != null && tmpValue.ToString().Trim().Contains("供料器类型"))
                    {
                        type = 1;
                        i++;
                        continue;
                    }

                    //主料槽
                    tmpValue = sheet.Cells[slotNo + i].Value;
                    if (tmpValue != null)
                        slot = tmpValue.ToString().Trim();

                    //飞达类型
                    tmpValue = sheet.Cells[typeName + i].Value;
                    if (tmpValue != null)
                        feederType = tmpValue.ToString().Trim();

                    //子料槽
                    if (type == 1)//就是供料器类型
                    {
                        tmpValue = sheet.Cells[SubsoltNo + i].Value;
                    }
                    else
                    {
                        tmpValue = sheet.Cells[SubsoltNoEx + i].Value;
                    }
                    if (tmpValue != null)
                    {
                        subslot = tmpValue.ToString().Trim();
                    }


                    //元件料号
                    if (type == 1)//就是供料器类型
                    {
                        tmpValue = sheet.Cells[fPNNO + i].Value;
                    }
                    else
                    {
                        tmpValue = sheet.Cells[lpPNNO + i].Value;
                    }
                    if (tmpValue != null)
                    {
                        pn = tmpValue.ToString().Trim();
                    }
                    else
                    {
                        continue;
                    }


                    //单位
                    tmpValue = sheet.Cells[unitNo + i].Value;
                    if (tmpValue != null)
                        unitQtyStringValue = tmpValue.ToString().Trim();

                    #region 验证

                    //if (continueNullRowCount > 2) continue;

                    //if (string.IsNullOrEmpty(slot))
                    //{
                    //  continueNullRowCount += 1;
                    // continue;
                    //yamahaPlacement.Clear();
                    //Messenger.Error(string.Format("第{0}行的【站位】为空,料单导入失败!", i + 1));
                    //break;
                    //}
                    //if (string.IsNullOrEmpty(pn))
                    //{
                    //  continueNullRowCount += 1;
                    //  continue;
                    //yamahaPlacement.Clear();
                    //Messenger.Error(string.Format("第{0}行的【编码】为空,料单导入失败!", i + 1));
                    //break;
                    //}
                    //if (string.IsNullOrEmpty(refDesign))
                    //{
                    //yamahaPlacement.Clear();
                    //Messenger.Error(string.Format("第{0}行的【位置】为空,料单导入失败!", i + 1));
                    //break;
                    // continue;
                    //} 


                    if (string.IsNullOrEmpty(subslot))
                    {
                        continueNullRowCount += 1;
                        continue;
                        // yamahaPlacement.Clear();
                        //Messenger.Error(string.Format("第{0}行的【编码】为空,料单导入失败!", i + 1));
                        //break;
                    }
                    #endregion

                    unitQty = Convert.ToDecimal(unitQtyStringValue);
                    if (unitQty <= 0)
                    {
                        //yamahaPlacement.Clear();
                        //Messenger.Error(string.Format("第{0}行的【用量】小于等于0,料单导入失败!", i + 1));
                        // string errmsg = string.Format(string.Format(_localizer["AIRI_file_import_error"], i + 1));
                        // throw new Exception(errmsg);
                        continue;
                    }
                    continueNullRowCount = 0;
                    SmtPlacementTemp newRow = new SmtPlacementTemp();
                    newRow.SLOT = slot;
                    newRow.SUB_SLOT = subslot;
                    newRow.TABLE_NO = stage;
                    newRow.PART_NO = pn;
                    newRow.FEEDER_TYPE = feederType;
                    newRow.REFDESIGNATOR = refDesign;
                    newRow.UNITQTY = unitQty*multiNo;
                    foreach (var item in linePlacement.LinePlacement)
                    {
                        if (item.StationID == stationID)
                        {
                            item.PlacementDetail.Add(newRow);
                        }
                    }

                    pn = string.Empty;
                    //feederType = string.Empty;
                    subslot = string.Empty;
                    //slot = string.Empty;
                    unitQty = 0;
                    size = string.Empty;
                    no = string.Empty;
                    refDesign = string.Empty;
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message); ;
            }
            return linePlacement;
        }

    }
}
