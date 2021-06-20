using JZ.IMS.ViewModels.BomVsPlacement;
using Microsoft.Extensions.Localization;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;

namespace JZ.IMS.WebApi.Controllers.BomVsPlacement
{
    public class CMPlacementSetup
    {
        const int tableNoIDX = 0;
        const int slotIDX = 1;
        const int subSlotIDX = 4;
        const int pnIDX = 2;
        const int descIDX = 3;
        const int feederTypeIDX = 5;
        const int refDesignIDX = 8;
        const int qtyIDX = 7;

        char[] splitChar = { ',', '，', '/', '\\', '、', '.', '．', ' ', '　', ';' };

        public string PlacementName { get; set; }

        private readonly IStringLocalizer<BomVsPlacementController> _localizer;
        public CMPlacementSetup(IStringLocalizer<BomVsPlacementController> localizer)
        {
            _localizer = localizer;
        }

        public string FormatLocation(string stage, string slot, string subSlot)
        {
            return string.Format("{0}-{1}{2}", stage.PadLeft(3, '0'), slot.Trim(), subSlot.Trim());
        }

        public List<SmtPlacementTemp> LoadPlacement(string filePath, string source_filename)
        {
            this.PlacementName = Path.GetFileNameWithoutExtension(source_filename);
            string[] fileData = File.ReadAllLines(filePath, Encoding.Default);
            int count = fileData.AsEnumerable().Count(f => f.Contains("供料器准备"));
            if (count <= 0 && fileData.Length <= 10)
            {
                //"请选择正确的料单文件。"
                throw new Exception(_localizer["incorrectness_placement_file"]);
            }
            count = 0;
            string version = "";
            string versionTag = "";
            string tableNo = "";
            string machineName = "";
            string slot = "";
            string subSlot = "";
            string pn = "";
            string desc = "";
            string feederType = "";
            string refDesign = "";
            decimal totalQty = 0;
            decimal unitQty = 0;
            var placement = new List<SmtPlacementTemp>();

            placement.Clear();
            bool stopFlag = true;
            int moduleNo = 0;
            foreach (string data in fileData)
            {
                count += 1;
                if (count == 1)
                {
                    // 批量名 :SN-OP300WSINO-D12-22
                    version = data.Split(',')[0].Split(':')[1].Trim();
                    versionTag = data.ToUpper();
                    continue;
                }
                if (count == 2)
                {
                    versionTag = data.Split(',')[0].Split(':')[1].Trim();
                    continue;
                }
                if (data.Contains("机器名 :"))
                {
                    machineName = data.Split(',')[0].Split(':')[1].Trim();
                    moduleNo++;
                    continue;
                }
                if (count == 4)
                {
                    continue;
                }

                try
                {
                    string splitData = this.ReplaceCommaToSemicolon(data);
                    //if (data.Contains("\""))
                    //{
                    //    string location = data.Substring(data.IndexOf("\""), data.LastIndexOf("\"") - data.IndexOf("\"") + 1);

                    //    splitData = data.Replace(location, location.Replace(",", ";").Replace("\"", ""));
                    //}

                    string[] placeData = splitData.Split(',');

                    if (splitData.Contains("部件名称"))
                    {
                        stopFlag = false;
                        continue;
                    }

                    if (placeData[0].Trim() == "托盘布置数据")
                    {
                        stopFlag = true;
                        continue;
                    }

                    if (stopFlag || string.IsNullOrWhiteSpace(placeData[pnIDX].Trim()))
                    {
                        continue;
                    }
                    if (placeData[pnIDX].Trim().Contains("***"))
                    {
                        tableNo = placeData[tableNoIDX];
                        slot = string.IsNullOrWhiteSpace(placeData[slotIDX]) && !string.IsNullOrWhiteSpace(placeData[0]) ? slot : string.Format("{0}{1}", tableNo, placeData[slotIDX].PadLeft(4, '0'));
                        continue;
                    }

                    if (Convert.ToDecimal(placeData[qtyIDX]) <= 0) continue;//使用量为0

                    tableNo = placeData[tableNoIDX];
                    slot = string.IsNullOrWhiteSpace(placeData[slotIDX]) && !string.IsNullOrWhiteSpace(placeData[0]) ? slot : string.Format("{0}{1}", tableNo, placeData[slotIDX].PadLeft(4, '0'));
                    subSlot = placeData[subSlotIDX].Trim().Replace("-", "");
                    pn = placeData[pnIDX];
                    desc = placeData[descIDX];
                    feederType = placeData[feederTypeIDX];
                    refDesign = placeData[refDesignIDX];
                    totalQty = Convert.ToDecimal(placeData[qtyIDX]);
                    unitQty = Convert.ToDecimal(placeData[qtyIDX]);
                    //unitQty = refDesign.Trim().Split(splitChar).Count();

                    if (unitQty <= 0 && !string.IsNullOrWhiteSpace(refDesign))
                    {
                        //"请注意，料站:{0}, 料号:{1}的单位用量为0!", slot, pn
                        string errmsg = string.Format(_localizer["place_unitQty_is0"], slot, pn);
                        throw new Exception(errmsg);
                    }
                }
                catch (Exception ex)
                {
                    throw ex;
                }

                SmtPlacementTemp item = new SmtPlacementTemp()
                {
                    TABLE_NO = tableNo,
                    MACHINE_NAME = machineName,
                    SLOT = slot,
                    SUB_SLOT = subSlot,
                    PART_NO = pn,
                    DESCRIPTION = desc,
                    FEEDER_TYPE = feederType,
                    REFDESIGNATOR = refDesign,
                    TOTAL_QTY = totalQty,
                    UNITQTY = unitQty,
                };

                placement.Add(item);
            }
            return placement;
        }

        public PANASONIC_CM_VM LoadSmtPlacementToBomSheet(List<SmtPlacementTemp> placement)
        {
            PANASONIC_CM_VM result = new PANASONIC_CM_VM();
            result.TitleList = new List<string>(){ "机器名" , "元件名" , "贴片位号" , "单位用量" , "插槽" , "子插槽" , "备注" , "模组编号" };
            result.PlacementName = this.PlacementName;

            result.DataList = new List<PANASONIC_CM>();
            foreach (var row in placement)
            {
                var item = new PANASONIC_CM()
                {
                    MACHINE_NAME = row.MACHINE_NAME,
                    PART_NO = row.PART_NO,
                    REFDESIGNATOR = row.REFDESIGNATOR == null ? string.Empty : row.REFDESIGNATOR,
                    UNITQTY = row.UNITQTY,
                    SLOT = row.SLOT,
                    SUB_SLOT = row.SUB_SLOT,
                    DESCRIPTION = row.DESCRIPTION == null ? string.Empty : row.DESCRIPTION,
                    TABLE_NO = row.TABLE_NO
                };
                result.DataList.Add(item);
            }
            return result;
        }

        /// <summary>
        /// 对读入的字符串，双引号内的字符中，逗号改成分号
        /// </summary>
        /// <param name="str">原字符串</param>
        /// <returns></returns>
        private string ReplaceCommaToSemicolon(string str)
        {
            bool flag = false;
            string res = "";

            for (int i = 0; i < str.Length; i++)
            {
                if (str[i] == '\"')
                {
                    flag = !flag;
                    continue;
                }
                else
                {
                    res += (str[i] == ',' && flag) ? ';' : str[i];
                }
            }
            return res;
        }

    }
}
