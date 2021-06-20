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
    public class PanasonicPlacementSetup
    {
        const int machineNoIDX = 0;
        const int machineNameIDX = 1;
        const int tableNoIDX = 2;
        const int slotIDX = 3;
        const int subSlotIDX = 4;
        const int pnIDX = 5;
        const int descIDX = 9;
        const int vendorIDX = 6;
        const int qtyIDX = 7;
        const int replaceIDX = 10;
        const int typeIDX = 11;
        const int designIDX = 8;

        char[] splitChar = { ',', '，', '/', '\\', '、', '.', '．', ' ', '　', ';' };

        private decimal pcbSide = 1;
        public decimal PcbSide
        {
            get { return this.pcbSide; }
            set { this.pcbSide = value; }
        }

        //private decimal pcbRouteCode = 0;
        private decimal standardCapacity = 0;
        private decimal placementHeaderID = 0;

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

        private decimal smtRouteOrder;
        public decimal SMTRouteOrder
        {
            get
            {
                return this.smtRouteOrder;
            }
            private set
            {
                this.smtRouteOrder = value;
            }
        }

        private readonly IStringLocalizer<BomVsPlacementController> _localizer;
        public PanasonicPlacementSetup(IStringLocalizer<BomVsPlacementController> localizer)
        {
            _localizer = localizer;
        }

        /// <summary>
        /// 清除
        /// </summary>
        public void Clear()
        {
            this.placementName = string.Empty;
            this.partNumber = string.Empty;
            this.smtStationID = 0;
            this.smtRouteOrder = 0;
            this.placementHeaderID = 0;
        }

        public List<SmtPlacementTemp> LoadCsvPlacement(string filePath, string source_filename)
        {
            this.PlacementName = Path.GetFileNameWithoutExtension(source_filename);
            string[] fileData = File.ReadAllLines(filePath, Encoding.Default);
            int count = fileData.AsEnumerable().Count(f => f.Contains("供料器设置"));
            if (count <= 0 && fileData.Length <= 5)
            {
                //throw new Exception("请选择正确的Panasonic料单文件。");
                throw new Exception(_localizer["incorrectness_panasonic_file"]);
            }
            count = 0;
            string version = "";
            string versionTag = "";
            string slot = "";
            string subSlot = "";
            string pn = "";
            string desc = "";
            string refDesignator = "";
            string tableNo = "";
            string replace = "";
            string vendor = "";
            string type = "";
            string machineName = "";
            string machineNo = "";
            decimal totalQty = 0;
            decimal unitQty = 0;
            List<SmtPlacementTemp> smtPlacement = new List<SmtPlacementTemp>();
            foreach (string data in fileData)
            {
                count += 1;
                if (count == 2)
                {
                    // 产品: CM-MJ1L-DSP-D11,,,,,,,,,,,
                    version = data.Split(',')[0].Split(':')[1].Trim();
                    versionTag = data.ToUpper();
                    continue;
                }
                if (count <= 5)
                {
                    continue;
                }

                try
                {
                    //string splitData = data;

                    //// 机器编号,机器名,工作台,插槽,子插槽,元件,供应,贴装点,CM-MJ1L-DSP-D11参考编号,元件说明,替代元件,薄型单式
                    //if (data.Contains("\""))
                    //{
                    //    string location = data.Substring(data.IndexOf("\""), data.LastIndexOf("\"") - data.IndexOf("\"") + 1);

                    //    splitData = data.Replace(location, location.Replace(",", ";").Replace("\"", ""));
                    //}

                    //替换双引号内的字符的逗号为分号
                    string splitData = ReplaceCommaToSemicolon(data);
                    string[] placeData = splitData.Split(',');

                    if (placeData.Length >= 12)
                    {
                        pn = placeData[pnIDX];
                        if (string.IsNullOrWhiteSpace(pn)) continue;
                        if (Convert.ToDecimal(placeData[qtyIDX]) <= 0) continue;//使用量为0
                        slot = placeData[slotIDX];
                        subSlot = placeData[subSlotIDX];
                        desc = placeData[descIDX];
                        machineName = placeData[machineNameIDX];
                        machineNo = placeData[machineNoIDX];
                        refDesignator = placeData[designIDX];
                        tableNo = placeData[tableNoIDX];
                        replace = placeData[replaceIDX];
                        vendor = placeData[vendorIDX];
                        type = placeData[typeIDX];
                        totalQty = decimal.Parse(placeData[qtyIDX]);
                        unitQty = decimal.Parse(placeData[qtyIDX]);
                        //unitQty = refDesignator.Trim().Split(splitChar).Count();
                    }
                    else
                    {
                        continue;
                        //托盘读取
                    }

                    if (unitQty <= 0)
                    {
                        //throw new Exception(string.Format("请注意，料站:{0}, 料号:{1}的用量为0!",slot, pn));
                        string errmsg = string.Format(_localizer["place_useqty_is0"], slot, pn);
                        throw new Exception(errmsg);
                    }
                }
                catch (Exception ex)
                {
                    throw ex;
                }

                var newRow = new SmtPlacementTemp();
                newRow.SLOT = slot;
                newRow.SUB_SLOT = subSlot;
                newRow.PART_NO = pn;
                newRow.DESCRIPTION = desc;
                newRow.REPLACE = replace;
                newRow.REFDESIGNATOR = refDesignator;
                newRow.TABLE_NO = tableNo;
                newRow.MACHINE_NO = machineNo;
                newRow.MACHINE_NAME = machineName;
                newRow.TYPE = type;
                newRow.VENDOR = vendor;
                newRow.TOTAL_QTY = totalQty;
                newRow.UNITQTY = unitQty;

                smtPlacement.Add(newRow);
            }

            this.placementName = version;
            return smtPlacement;
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
