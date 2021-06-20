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
    public class YamahaPlacementSetup
    {
        const int slotIDX = 0;
        const int pnIDX = 1;
        const int descIDX = 2;
        const int feederTypeIDX = 3;
        const int sizeIDX = 4;

        const int noIDX = 5;

        const int refDesignIDX11 = 11;
        const int unitQtyIDX12 = 12;

        const int refDesignIDX10 = 10;
        const int unitQtyIDX11 = 11;

        const int refDesignIDX09 = 9;
        const int unitQtyIDX10 = 10;

        private decimal pcbSide = 1;
        public decimal PcbSide
        {
            get { return this.pcbSide; }
            set { this.pcbSide = value; }
        }

        //private decimal standardCapacity = 0;
        //private decimal placementHeaderID = 0;

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

        private readonly IStringLocalizer<BomVsPlacementController> _localizer;
        public YamahaPlacementSetup(IStringLocalizer<BomVsPlacementController> localizer)
        {
            _localizer = localizer;
        }

        public List<SmtPlacementTemp> LoadYamahaCsvPlacement(string filePath, string source_filename)
        {
            this.PlacementName = Path.GetFileNameWithoutExtension(source_filename);
            string[] fileData = File.ReadAllLines(filePath, Encoding.Default);
            int count = fileData.AsEnumerable().Count(f => f.Contains("安装号码"));
            if (count <= 0 && fileData.Length <= 4)
            {
                //"请选择正确的Yamaha料单文件。"
                throw new Exception(_localizer["Yamaha_file_error"]);
            }
            count = 0;
            string version = "";
            string versionTag = "";
            string slot = "";
            string pn = "";
            string desc = "";
            string feederType = "";
            string size = "";
            string refDesign = "";
            //string no = "";
            decimal unitQty = 0;
            List<SmtPlacementTemp> yamahaPlacement = new List<SmtPlacementTemp>();

            foreach (string data in fileData)
            {
                count += 1;
                if (count == 1)
                {
                    version = data.Split(',')[1];
                    continue;
                }
                if (count == 2)
                {
                    versionTag = data.ToUpper();
                    continue;
                }
                if (count == 3)
                {
                    continue;
                }
                if (count == 4)
                {
                    continue;
                }

                try
                {
                    string[] placeData = data.Split(',');
                    slot = placeData[slotIDX];
                    if (string.IsNullOrWhiteSpace(slot)) continue;
                    pn = placeData[pnIDX];
                    pn = pn.Split('-')[0];
                    desc = placeData[descIDX];
                    feederType = placeData[feederTypeIDX];
                    size = placeData[sizeIDX];
                    //no = placeData[noIDX];
                    if (placeData.Length <= 11)
                    {
                        refDesign = placeData[refDesignIDX09];
                        unitQty = decimal.Parse(placeData[unitQtyIDX10]);
                    }
                    else if (placeData.Length == 12)
                    {
                        refDesign = placeData[refDesignIDX10];
                        unitQty = decimal.Parse(placeData[unitQtyIDX11]);
                    }
                    else
                    {
                        refDesign = placeData[refDesignIDX11];
                        unitQty = decimal.Parse(placeData[unitQtyIDX12]);
                    }
                    if (unitQty <= 0)
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

                SmtPlacementTemp newRow = new SmtPlacementTemp();
                newRow.SLOT = slot;
                newRow.PART_NO = pn;
                newRow.DESCRIPTION = desc;
                newRow.FEEDER_TYPE = feederType;
                newRow.SIZE = size;
                newRow.NO = slot;
                newRow.REFDESIGNATOR = refDesign;
                newRow.UNITQTY = unitQty;

                yamahaPlacement.Add(newRow);
            }
            return yamahaPlacement;
        }

    }
}
