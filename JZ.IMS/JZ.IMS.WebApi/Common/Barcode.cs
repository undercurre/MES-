using JZ.IMS.ViewModels;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace JZ.IMS.WebApi.Common
{
    public static class BarcoderFilter
    {

        public const string Default_SplitChars = @"|\;";

        public const string Reel2DBarcodePattern = "S{0}|P{1}|1P{2}|Q{3}|9D{4}|1T{5}|4L{6}|M{7}|CP{8}|R{9}";


        private static string _2d_spliter = string.Empty;

        public static ReelInfoViewModel GetSkyworthReel(string reel2dCode)
        {
            ReelInfoViewModel reel = new ReelInfoViewModel();

            string[] splitArray = reel2dCode.Split(':');
            if (splitArray.Length < 2)
            {
                reel.CODE = BarcoderFilter.FormatBarcode(reel2dCode);
                return reel;
            }
            reel.CODE = BarcoderFilter.FilterBarcodeSkyworth(reel2dCode, BarcodeTypes.ReelID);
            reel.PART_NO = BarcoderFilter.FilterBarcodeSkyworth(reel2dCode, BarcodeTypes.PartNo);
            reel.MAKER_PART_NO = BarcoderFilter.FilterBarcodeSkyworth(reel2dCode, BarcodeTypes.MakerPart);
            reel.MAKER_NAME = BarcoderFilter.FilterBarcodeSkyworth(reel2dCode, BarcodeTypes.Maker);
            reel.ORIGINAL_QUANTITY = Decimal.Parse(BarcoderFilter.FilterBarcodeSkyworth(reel2dCode, BarcodeTypes.Quantity));
            //reel.DATE_CODE = Decimal.Parse(BarcoderFilter.FilterBarcodeSkyworth(reel2dCode, BarcodeTypes.DateCode));
            reel.DATE_CODE = BarcoderFilter.FilterBarcodeSkyworth(reel2dCode, BarcodeTypes.DateCode);
            reel.LOT_CODE = BarcoderFilter.FilterBarcodeSkyworth(reel2dCode, BarcodeTypes.LotCode);
            reel.COO = BarcoderFilter.FilterBarcodeSkyworth(reel2dCode, BarcodeTypes.SalesProjectNumber);
            reel.CUSTOMER_PN = BarcoderFilter.FilterBarcodeSkyworth(reel2dCode, BarcodeTypes.CustomerPn);
            reel.REFERENCE = BarcoderFilter.FilterBarcodeSkyworth(reel2dCode, BarcodeTypes.Ref);
            reel.VENDOR_CODE = BarcoderFilter.FilterBarcodeSkyworth(reel2dCode, BarcodeTypes.VendorCode);
            return reel;

        }

        public static string _2D_Spliter
        {
            get
            {
                try
                {
                    if (string.IsNullOrEmpty(_2d_spliter))
                        _2d_spliter = BarcoderFilter.GetSpliter();

                    if (string.IsNullOrEmpty(_2d_spliter))
                        _2d_spliter = Default_SplitChars;
                }
                catch
                {
                    _2d_spliter = Default_SplitChars; ;
                }
                return _2d_spliter;
            }
        }

        public static string GetSpliter()
        {
            string spliter = Default_SplitChars;

            #region mask
            /*
            try
            {
                spliter = LookupManager.GetLookupTable(
                    new KeyValuePair<string, object>(GlobalVariables.TYPE, "VENDOR_BCD_SPLITER")
                    ).FirstRow().Cast<BasisDataSet.IMS_LOOKUPRow>().VALUE;
            }
            catch
            {
            }*/
            #endregion

            return spliter;
        }


        //public static string To2DBarcodeString(this IMS_REEL_INFO_VIEWRow reelInfoRow)
        //{
        //    if (reelInfoRow == null)
        //    {
        //        return string.Empty;
        //    }
        //    else
        //    {
        //        return string.Format(Reel2DBarcodePattern,
        //                            reelInfoRow.CODE,
        //                            reelInfoRow.PART_NO,
        //                            reelInfoRow.IsMAKER_PART_NONull() ? string.Empty : reelInfoRow.MAKER_PART_NO,
        //                            reelInfoRow.ORIGINAL_QUANTITY,
        //                            reelInfoRow.IsDATE_CODENull() ? string.Empty : (reelInfoRow.DATE_CODE.ToString("yyyy") +
        //                                reelInfoRow.DATE_CODE.ToString("MM") + reelInfoRow.DATE_CODE.ToString("dd")),
        //                            reelInfoRow.IsLOT_CODENull() ? string.Empty : reelInfoRow.LOT_CODE,
        //                            reelInfoRow.IsCOONull() ? string.Empty : reelInfoRow.COO,
        //                            reelInfoRow.IsMAKER_CODENull() ? string.Empty : reelInfoRow.MAKER_CODE,
        //                            reelInfoRow.IsCUSTOMER_PNNull() ? string.Empty : reelInfoRow.CUSTOMER_PN,
        //                            reelInfoRow.IsREFERENCENull() ? string.Empty : reelInfoRow.REFERENCE);
        //    }
        //}

        public static string FormatBarcode(string srcText, string prefix)
        {
            if (srcText == null)
                return null;
            if (srcText.Length < prefix.Length)
                return null;
            if (srcText.ToUpper().StartsWith(prefix))
                return srcText.Substring(prefix.Length).ToUpper();

            return null;
        }

        public static string CheckBarcode(string srcText, string prefix)
        {
            if (srcText == null)
                return null;
            if (srcText.Length < prefix.Length)
                return null;
            if (srcText.StartsWith(prefix))
                return srcText.ToUpper();

            return null;
        }

        public static string FilterBarcode(string originalStr, BarcodeTypes barcodeType)
        {
            //if (originalStr.ToUpper().StartsWith("BX")) return originalStr;
            //if (originalStr.ToUpper().StartsWith("DP")) return originalStr;
            //if (originalStr.ToUpper().StartsWith("BX")) return originalStr;
            //if (originalStr.ToUpper().StartsWith("DP")) return originalStr;
            //if (originalStr.ToUpper().StartsWith("QC")) return originalStr;
            //if (originalStr.ToUpper().StartsWith("QD")) return originalStr;
            //if (originalStr.ToUpper().StartsWith("AF")) return originalStr;
            //if (originalStr.ToUpper().StartsWith("CAR")) return originalStr;
            //if (originalStr.ToUpper().StartsWith("FW")) return originalStr;
            //if (originalStr.ToUpper().StartsWith("R1")) return originalStr;
            //if (originalStr.ToUpper().StartsWith("BP")) return originalStr;
            //if (originalStr.ToUpper().StartsWith("B0201")) return originalStr;
            //if (originalStr.ToUpper().StartsWith("B0301")) return originalStr;
            //if (originalStr.ToUpper().StartsWith("B0302")) return originalStr;
            //if (originalStr.ToUpper().StartsWith("B0401")) return originalStr;
            //if (originalStr.ToUpper().StartsWith("B0402")) return originalStr;
            //if (originalStr.ToUpper().StartsWith("B0602")) return originalStr;
            //if (originalStr.ToUpper().StartsWith("B08")) return originalStr;
            //if (originalStr.ToUpper().StartsWith("BB00001")) return originalStr;
            //if (originalStr.ToUpper().StartsWith("BB00002")) return originalStr;
            //if (originalStr.ToUpper().StartsWith("BB00003")) return originalStr;
            //if (originalStr.ToUpper().StartsWith("BB00006")) return originalStr;
            //if (originalStr.ToUpper().StartsWith("B") && originalStr.Length < 9) return originalStr;
            //if (originalStr.ToUpper().StartsWith("S") && originalStr.Length < 9) return originalStr;

            string regExpressPatern = string.Empty;
            switch (barcodeType)
            {
                case BarcodeTypes.ReelID:
                    regExpressPatern = BarcodePrefixCode.C_Prefix_ReelID;
                    break;
                case BarcodeTypes.PartNo:
                    regExpressPatern = BarcodePrefixCode.C_Prefix_MitacPN;
                    break;
                case BarcodeTypes.MakerPart:
                    regExpressPatern = BarcodePrefixCode.C_Prefix_MakerPN;
                    break;
                case BarcodeTypes.Maker:
                    regExpressPatern = BarcodePrefixCode.C_Prefix_Maker;
                    break;
                case BarcodeTypes.Quantity:
                    regExpressPatern = BarcodePrefixCode.C_Prefix_Qty;
                    break;
                case BarcodeTypes.DateCode:
                    regExpressPatern = BarcodePrefixCode.C_Prefix_Date;
                    break;
                case BarcodeTypes.Coo:
                    regExpressPatern = BarcodePrefixCode.C_Prefix_CoO;
                    break;
                case BarcodeTypes.LotCode:
                    regExpressPatern = BarcodePrefixCode.C_Prefix_Lot;
                    break;
                case BarcodeTypes.CustomerPn:
                    regExpressPatern = BarcodePrefixCode.C_Prefix_CustomerPN;
                    break;
                case BarcodeTypes.Ref:
                    regExpressPatern = BarcodePrefixCode.C_Prefix_Ref;
                    break;
                case BarcodeTypes.Box:
                    regExpressPatern = BarcodePrefixCode.C_Prefix_Ref;
                    break;
                case BarcodeTypes.Else:
                    regExpressPatern = string.Empty;
                    break;
                default:
                    regExpressPatern = string.Empty;
                    break;
            }
            if (string.IsNullOrEmpty(regExpressPatern)) return originalStr;

            string[] bcds = originalStr.Split(_2D_Spliter.ToCharArray(), StringSplitOptions.RemoveEmptyEntries);

            foreach (string bcd in bcds)
            {
                if (Regex.IsMatch(bcd, regExpressPatern, RegexOptions.IgnoreCase | RegexOptions.IgnorePatternWhitespace))
                    return Regex.Replace(bcd, regExpressPatern, string.Empty, RegexOptions.IgnoreCase | RegexOptions.IgnorePatternWhitespace).ToUpper();
            }

            return originalStr;
        }

        public static string FilterBarcode(string originalStr, string c_str_patern)
        {
            if (Regex.IsMatch(originalStr, c_str_patern, RegexOptions.IgnoreCase | RegexOptions.IgnorePatternWhitespace))
                return Regex.Replace(originalStr, c_str_patern, string.Empty, RegexOptions.IgnoreCase | RegexOptions.IgnorePatternWhitespace);
            else
                return originalStr;
        }

        public static bool FilterBarcode(string originalStr, string c_str_patern, ref string finalBarcode)
        {
            if (Regex.IsMatch(originalStr, c_str_patern, RegexOptions.IgnoreCase | RegexOptions.IgnorePatternWhitespace))
            {
                finalBarcode = Regex.Replace(originalStr, c_str_patern, string.Empty, RegexOptions.IgnoreCase | RegexOptions.IgnorePatternWhitespace);
                return true;
            }
            else
            {
                return false;
            }
        }

        public static string GetBarcode(this string _2dBarcodeString, string pattern)
        {
            string[] bcds = _2dBarcodeString.Split(BarcoderFilter.Default_SplitChars.ToCharArray());
            string result = string.Empty;
            bool isMatch = false;
            foreach (string bcd in bcds)
            {
                if (string.IsNullOrEmpty(bcd))
                    continue;
                isMatch = BarcoderFilter.FilterBarcode(bcd, pattern, ref result);
                if (isMatch)
                {
                    return result;
                }
            }
            return _2dBarcodeString;
        }
        public static string[] Split2DBarcode(this string _2dbarcode)
        {
            string[] splitArray = _2dbarcode.Split(new string[] { "|" }, StringSplitOptions.RemoveEmptyEntries);

            if (splitArray.Length < 2)
            {
                splitArray = _2dbarcode.Split(new char[] { '\\' }, StringSplitOptions.RemoveEmptyEntries);
            }
            return splitArray;
        }
        public static bool Is2DBarcode(this string codeString)
        {
            string[] splitArray = codeString.Split2DBarcode();
            if (splitArray.Length > 1)
                return true;
            else
                return false;
        }

        public static string GeetReelCode(this string reel2dCode)
        {
            string[] splitArray = reel2dCode.Split2DBarcode();
            foreach (string item in splitArray)  //解析字符串
            {
                string temp = item.Trim();

                if (Regex.IsMatch(temp, BarcodePrefixCode.C_Prefix_ReelID
                    , RegexOptions.IgnoreCase | RegexOptions.IgnorePatternWhitespace))
                {
                    string CODE = Regex.Replace(temp, BarcodePrefixCode.C_Prefix_ReelID,
                        "", RegexOptions.IgnoreCase | RegexOptions.IgnorePatternWhitespace).ToUpper();
                    if (CODE == null || CODE.Trim()=="")
                    {
                        throw new Exception("条形码中没有流水号");
                    }
                    return CODE;
                }
            }
            return reel2dCode;
        }

        public static string FixDateCode(this string yyyy00)
        {
            string tmp = yyyy00;
            tmp = tmp.Replace("-", "");
            tmp = tmp.Replace("/", "");
            tmp = tmp.Replace("|", "");
            tmp = tmp.Replace("\\", "");
            if (tmp.Length == 4)
            {
                int.Parse(tmp);
                tmp = string.Format("20{0}", tmp);  //yyww
            }
            else if (tmp.Length == 3)
            {
                tmp = tmp.Substring(0, 2) + "0" + tmp.Substring(tmp.Length - 1);//yyw
                tmp = string.Format("20{0}", tmp);
            }
            else if (tmp.Length == 6)
            {
                int fullYear = DateTime.Now.Year;
                int yearPrePart = int.Parse(fullYear.ToString().Substring(0, 2));
                int yearEndPart = int.Parse(fullYear.ToString().Substring(2, 2));
                List<int> fullYearList = new List<int>();
                for (int i = fullYear; i >= fullYear - 20; i--)
                {
                    fullYearList.Add(i);
                }
                List<int> shortYearList = new List<int>();
                for (int i = yearEndPart; i >= yearEndPart - 20; i--)
                {
                    shortYearList.Add(i);
                }
                if (fullYearList.Count(f => f.ToString() == tmp.Substring(0, 4)) <= 0)
                {
                    tmp = DateTime.Now.Year.ToString().Substring(0, 2) + tmp; //yymmdd
                    int dateInt = int.Parse(tmp);
                    int year = int.Parse(tmp.Substring(0, 4));
                    int month = int.Parse(tmp.Substring(4, 2));
                    int day = int.Parse(tmp.Substring(6));
                    tmp = year.ToString() + GetWeekOfYear(new DateTime(year, month, day)).ToString();
                }
            }
            else if (tmp.Length == 8)
            {
                int year = int.Parse(tmp.Substring(0, 4));
                int month = int.Parse(tmp.Substring(4, 2));
                int day = int.Parse(tmp.Substring(6));
                tmp = year.ToString() + GetWeekOfYear(new DateTime(year, month, day)).ToString();
            }
            return tmp;
        }

        /// <summary>
        /// 获取指定日期，在为一年中为第几周
        /// </summary>
        /// <param name="dt">指定时间</param>
        /// <reutrn>返回第几周</reutrn>
        public static int GetWeekOfYear(DateTime dt)
        {
            GregorianCalendar gc = new GregorianCalendar();
            int weekOfYear = gc.GetWeekOfYear(dt, CalendarWeekRule.FirstDay, DayOfWeek.Monday);
            return weekOfYear;
        }


        /// <summary>
        /// 格式化條碼
        /// </summary>
        /// <param name="originalStr"></param>
        /// <returns></returns>
        public static string FormatBarcode(string originalStr)
        {
            string result = originalStr;
            //if (result.ToUpper().StartsWith("BX")) return result;
            //if (result.ToUpper().StartsWith("DP")) return result;
            //if (result.ToUpper().StartsWith("QC")) return result;
            //if (result.ToUpper().StartsWith("QD")) return result;
            //if (result.ToUpper().StartsWith("AF")) return result;
            //if (result.ToUpper().StartsWith("CAR")) return result;
            //if (result.ToUpper().StartsWith("FW")) return result;
            //if (result.ToUpper().StartsWith("R1")) return result;
            //if (result.ToUpper().StartsWith("BP")) return result;
            //if (result.ToUpper().StartsWith("B0201")) return result;
            //if (result.ToUpper().StartsWith("B0301")) return result;
            //if (result.ToUpper().StartsWith("B0302")) return result;
            //if (result.ToUpper().StartsWith("B0401")) return result;
            //if (result.ToUpper().StartsWith("B0402")) return result;
            //if (result.ToUpper().StartsWith("B0602")) return result;
            //if (result.ToUpper().StartsWith("B08")) return result;
            //if (result.ToUpper().StartsWith("BB00001")) return result;
            //if (result.ToUpper().StartsWith("BB00002")) return result;
            //if (result.ToUpper().StartsWith("BB00003")) return result;
            //if (result.ToUpper().StartsWith("BB00006")) return result;
            //if (result.ToUpper().StartsWith("B") && result.Length < 9) return result;
            //if (result.ToUpper().StartsWith("S") && result.Length < 9) return result;
            result = FilterBarcodeSkyworth(originalStr, BarcodeTypes.ReelID);
            if (result != originalStr) return result;
            result = originalStr.GetBarcode(BarcodePrefixCode.C_Prefix_3S);
            if (result != originalStr) return result;
            result = originalStr.GetBarcode(BarcodePrefixCode.C_Prefix_ReelID);
            if (result != originalStr) return result;
            result = originalStr.GetBarcode(BarcodePrefixCode.C_Prefix_MakerPN);
            if (result != originalStr) return result;
            result = originalStr.GetBarcode(BarcodePrefixCode.C_Prefix_MitacPN);
            if (result != originalStr) return result;
            result = originalStr.GetBarcode(BarcodePrefixCode.C_Prefix_Qty);
            if (result != originalStr) return result;
            result = originalStr.GetBarcode(BarcodePrefixCode.C_Prefix_Date);
            if (result != originalStr) return result;
            result = originalStr.GetBarcode(BarcodePrefixCode.C_Prefix_CoO);
            if (result != originalStr) return result;
            result = originalStr.GetBarcode(BarcodePrefixCode.C_Prefix_Lot);
            if (result != originalStr) return result;
            result = originalStr.GetBarcode(BarcodePrefixCode.C_Prefix_CustomerPN);
            if (result != originalStr) return result;
            result = originalStr.GetBarcode(BarcodePrefixCode.C_Prefix_Ref);
            if (result != originalStr) return result;
            result = originalStr.GetBarcode(BarcodePrefixCode.C_Prefix_BPPO);
            if (result != originalStr) return result;
            result = originalStr.GetBarcode(BarcodePrefixCode.C_Prefix_QVL);
            if (result != originalStr) return result;
            result = originalStr.GetBarcode(BarcodePrefixCode.C_Prefix_LOT_CODE_P);
            if (result != originalStr) return result;
            result = originalStr.GetBarcode(BarcodePrefixCode.C_Prefix_LOT_CODE_1T);
            if (result != originalStr) return result;
            result = originalStr.GetBarcode(BarcodePrefixCode.C_Prefix_DATE_CODE_D);
            if (result != originalStr) return result;
            result = originalStr.GetBarcode(BarcodePrefixCode.C_Prefix_BOX);
            if (result != originalStr) return result;
            result = originalStr.GetBarcode(BarcodePrefixCode.C_Prefix_RCV_SN);
            if (result != originalStr) return result;
            result = originalStr.GetBarcode(BarcodePrefixCode.C_Prefix_PL_NO);
            if (result != originalStr) return result;
            result = originalStr.GetBarcode(BarcodePrefixCode.C_Prefix_P2);
            if (result != originalStr) return result;
            result = originalStr.GetBarcode(BarcodePrefixCode.C_Prefix_LOCATOR);
            if (result != originalStr) return result;
            result = originalStr.GetBarcode(BarcodePrefixCode.C_Prefix_PALLET);
            if (result != originalStr) return result;
            return result;
        }

        /// <summary>
        /// 条形码解析   2020-6-2 LJW
        /// </summary>
        /// <param name="originalStr">条形码Str</param>
        /// <param name="barcodeType">获取条形码类型</param>
        /// <returns>解析后的字符串，解析失败为原来字符串</returns>
        static string FilterBarcodeSkyworth(string originalStr, BarcodeTypes barcodeType)
        {
            //0类型 1创维物料号 2供应商代码 3数量 4唯一码 5毛重 6净重 7工厂 8库位 9单位 10采购订单编号 11包装箱尺寸长 12宽 13高 14生成批次 15出厂日期 16生产日期 17制造商型号 18品牌   
            string[] bcds = originalStr.Split(':');
            string attribute = string.Empty;
            if (bcds.Length < 5) return originalStr;   //检查条形码是否解析成功

            string BCD_TYPE = bcds[0];

            if (BCD_TYPE == BarcodeEx.Y)
            {
                if (bcds.Length < 20)
                {
                    throw new Exception("国内材料标签二维码内容不符合创维标准: 内容不足");
                }
            }
            else if (BCD_TYPE == BarcodeEx.Z)
            {
                if (bcds.Length < 23)
                {
                    throw new Exception("自制标签二维码内容不符合标准: 内容不足");
                }
            }
            else if (BCD_TYPE == BarcodeEx.J)
            {
                if (bcds.Length < 21)
                {
                    throw new Exception("进口材料标签二维码内容不符合标准: 内容不足");
                }
            }
            else if (BCD_TYPE == BarcodeEx.S)
            {
                if (bcds.Length < 11)
                {
                    throw new Exception("内箱标签二维码内容不符合标准: 内容不足");
                }
            }
            else
            {
                throw new Exception("当前条码内容格式不符合创维的要求!");
            }

            switch (barcodeType)
            {
                case BarcodeTypes.ReelID:
                    if (BCD_TYPE == BarcodeEx.Y)
                    {
                        attribute = bcds[BarcodeEx.Y_IDX_BCD_CODE];
                    }
                    else if (BCD_TYPE == BarcodeEx.Z)
                    {
                        attribute = bcds[BarcodeEx.Z_IDX_BCD_CODE];
                    }
                    else if (BCD_TYPE == BarcodeEx.J)
                    {
                        attribute = bcds[BarcodeEx.J_IDX_BCD_CODE];
                    }
                    else
                    {
                        attribute = bcds[BarcodeEx.S_IDX_BCD_CODE];
                    }
                    break;
                case BarcodeTypes.Box:
                    if (BCD_TYPE == BarcodeEx.Y)
                    {
                        attribute = bcds[BarcodeEx.Y_IDX_BCD_CODE];
                    }
                    else if (BCD_TYPE == BarcodeEx.Z)
                    {
                        attribute = bcds[BarcodeEx.Z_IDX_BCD_CODE];
                    }
                    else if (BCD_TYPE == BarcodeEx.J)
                    {
                        attribute = bcds[BarcodeEx.J_IDX_BCD_CODE];
                    }
                    else
                    {
                        attribute = bcds[BarcodeEx.S_IDX_BCD_CODE];
                    }
                    break;
                case BarcodeTypes.PartNo:
                    if (BCD_TYPE == BarcodeEx.Y)
                    {
                        attribute = bcds[BarcodeEx.Y_IDX_BCD_PART];
                    }
                    else if (BCD_TYPE == BarcodeEx.Z)
                    {
                        attribute = bcds[BarcodeEx.Z_IDX_BCD_PART];
                    }
                    else if (BCD_TYPE == BarcodeEx.J)
                    {
                        attribute = bcds[BarcodeEx.J_IDX_BCD_PART];
                    }
                    else
                    {
                        attribute = bcds[BarcodeEx.S_IDX_BCD_PART];
                    }
                    break;
                case BarcodeTypes.Maker:
                    if (BCD_TYPE == BarcodeEx.Y)
                    {
                        attribute = bcds[BarcodeEx.Y_IDX_BCD_MAKER_NAME];
                    }
                    else if (BCD_TYPE == BarcodeEx.Z)
                    {
                        attribute = bcds[BarcodeEx.Z_IDX_BCD_MAKER_NAME];
                    }
                    else if (BCD_TYPE == BarcodeEx.J)
                    {
                        attribute = bcds[BarcodeEx.J_IDX_BCD_MAKER_NAME];
                    }
                    else
                    {
                        attribute = bcds[BarcodeEx.S_IDX_BCD_MAKER_NAME];
                    }
                    break;
                case BarcodeTypes.Quantity:
                    if (BCD_TYPE == BarcodeEx.Y)
                    {
                        attribute = bcds[BarcodeEx.Y_IDX_BCD_QTY];
                    }
                    else if (BCD_TYPE == BarcodeEx.Z)
                    {
                        attribute = bcds[BarcodeEx.Z_IDX_BCD_QTY];
                    }
                    else if (BCD_TYPE == BarcodeEx.J)
                    {
                        attribute = bcds[BarcodeEx.J_IDX_BCD_QTY];
                    }
                    else
                    {
                        attribute = bcds[BarcodeEx.S_IDX_BCD_QTY];
                    }
                    break;
                case BarcodeTypes.DateCode:
                    if (BCD_TYPE == BarcodeEx.Y)
                    {
                        attribute = bcds[BarcodeEx.Y_IDX_BCD_PRODUCTION_DATE];
                    }
                    else if (BCD_TYPE == BarcodeEx.Z)
                    {
                        attribute = bcds[BarcodeEx.Z_IDX_BCD_PRODUCTION_DATE];
                    }
                    else if (BCD_TYPE == BarcodeEx.J)
                    {
                        attribute = bcds[BarcodeEx.J_IDX_BCD_PRODUCTION_DATE];
                    }
                    else
                    {
                        attribute = bcds[BarcodeEx.S_IDX_BCD_PRODUCTION_DATE];
                    }
                    break;
                case BarcodeTypes.LotCode:
                    if (BCD_TYPE == BarcodeEx.Y)
                    {
                        attribute = bcds[BarcodeEx.Y_IDX_BCD_LOT];
                    }
                    else if (BCD_TYPE == BarcodeEx.Z)
                    {
                        attribute = bcds[BarcodeEx.Z_IDX_BCD_LOT];
                    }
                    else if (BCD_TYPE == BarcodeEx.J)
                    {
                        attribute = bcds[BarcodeEx.J_IDX_BCD_LOT];
                    }
                    else
                    {
                        attribute = bcds[BarcodeEx.S_IDX_BCD_LOT];
                    }
                    break;
                case BarcodeTypes.BatchNo:
                    if (BCD_TYPE == BarcodeEx.Y)
                    {
                        attribute = bcds[BarcodeEx.Y_IDX_BCD_LOT];
                    }
                    else if (BCD_TYPE == BarcodeEx.Z)
                    {
                        attribute = bcds[BarcodeEx.Z_IDX_BCD_LOT];
                    }
                    else if (BCD_TYPE == BarcodeEx.J)
                    {
                        attribute = bcds[BarcodeEx.J_IDX_BCD_LOT];
                    }
                    else
                    {
                        attribute = bcds[BarcodeEx.S_IDX_BCD_LOT];
                    }
                    break;
                case BarcodeTypes.VendorCode:
                    if (BCD_TYPE == BarcodeEx.Y)
                    {
                        attribute = bcds[BarcodeEx.Y_IDX_BCD_VENDOR_CODE];
                    }
                    else if (BCD_TYPE == BarcodeEx.Z)
                    {
                        attribute = bcds[BarcodeEx.Z_IDX_BCD_VENDOR_CODE];
                    }
                    else if (BCD_TYPE == BarcodeEx.J)
                    {
                        attribute = bcds[BarcodeEx.J_IDX_BCD_VENDOR_CODE];
                    }
                    else
                    {
                        attribute = bcds[BarcodeEx.S_IDX_BCD_VENDOR_CODE];
                    }
                    break;
                case BarcodeTypes.Type:
                    if (BCD_TYPE == BarcodeEx.Y)
                    {
                        attribute = bcds[BarcodeEx.Y_IDX_BCD_TYPE];
                    }
                    else if (BCD_TYPE == BarcodeEx.Z)
                    {
                        attribute = bcds[BarcodeEx.Z_IDX_BCD_TYPE];
                    }
                    else if (BCD_TYPE == BarcodeEx.J)
                    {
                        attribute = bcds[BarcodeEx.J_IDX_BCD_TYPE];
                    }
                    else
                    {
                        attribute = bcds[BarcodeEx.S_IDX_BCD_TYPE];
                    }
                    break;
                case BarcodeTypes.BU:
                    if (BCD_TYPE == BarcodeEx.Y)
                    {
                        attribute = bcds[BarcodeEx.Y_IDX_BCD_BU];
                    }
                    else if (BCD_TYPE == BarcodeEx.Z)
                    {
                        attribute = bcds[BarcodeEx.Z_IDX_BCD_BU];
                    }
                    else if (BCD_TYPE == BarcodeEx.J)
                    {
                        attribute = bcds[BarcodeEx.J_IDX_BCD_BU];
                    }
                    else
                    {
                        attribute = "";
                    }
                    break;
                case BarcodeTypes.SIC:
                    if (BCD_TYPE == BarcodeEx.Y)
                    {
                        attribute = bcds[BarcodeEx.Y_IDX_BCD_SIC];
                    }
                    else if (BCD_TYPE == BarcodeEx.Z)
                    {
                        attribute = bcds[BarcodeEx.Z_IDX_BCD_SIC];
                    }
                    else if (BCD_TYPE == BarcodeEx.J)
                    {
                        attribute = bcds[BarcodeEx.J_IDX_BCD_SIC];
                    }
                    else
                    {
                        attribute = "";
                    }
                    break;
                case BarcodeTypes.POCode:
                    if (BCD_TYPE == BarcodeEx.Y)
                    {
                        attribute = bcds[BarcodeEx.Y_IDX_BCD_PO];
                    }
                    else if (BCD_TYPE == BarcodeEx.Z)
                    {
                        attribute = bcds[BarcodeEx.Z_IDX_BCD_PO];
                    }
                    else if (BCD_TYPE == BarcodeEx.J)
                    {
                        attribute = bcds[BarcodeEx.J_IDX_BCD_PO];
                    }
                    else
                    {
                        attribute = "";
                    }
                    break;
                case BarcodeTypes.ShippingDate:
                    if (BCD_TYPE == BarcodeEx.Y)
                    {
                        attribute = bcds[BarcodeEx.Y_IDX_BCD_DELIVERY_DATE];
                    }
                    else if (BCD_TYPE == BarcodeEx.Z)
                    {
                        attribute = bcds[BarcodeEx.Z_IDX_BCD_DELIVERY_DATE];
                    }
                    else if (BCD_TYPE == BarcodeEx.J)
                    {
                        attribute = bcds[BarcodeEx.J_IDX_BCD_DELIVERY_DATE];
                    }
                    else
                    {
                        attribute = bcds[BarcodeEx.S_IDX_BCD_DELIVERY_DATE];
                    }
                    break;
                case BarcodeTypes.Brand:
                    if (BCD_TYPE == BarcodeEx.Y)
                    {
                        attribute = bcds[BarcodeEx.Y_IDX_BCD_BRAND];
                    }
                    else if (BCD_TYPE == BarcodeEx.Z)
                    {
                        attribute = bcds[BarcodeEx.Z_IDX_BCD_BRAND];
                    }
                    else if (BCD_TYPE == BarcodeEx.J)
                    {
                        attribute = bcds[BarcodeEx.J_IDX_BCD_BRAND];
                    }
                    else
                    {
                        attribute = bcds[BarcodeEx.S_IDX_BCD_BRAND];
                    }
                    break;
                case BarcodeTypes.MakerPart:
                    if (BCD_TYPE == BarcodeEx.Y)
                    {
                        attribute = bcds[BarcodeEx.Y_IDX_BCD_MAKER_PN];
                    }
                    else if (BCD_TYPE == BarcodeEx.Z)
                    {
                        attribute = bcds[BarcodeEx.Z_IDX_BCD_MAKER_PN];
                    }
                    else if (BCD_TYPE == BarcodeEx.J)
                    {
                        attribute = bcds[BarcodeEx.J_IDX_BCD_MAKER_PN];
                    }
                    else
                    {
                        attribute = bcds[BarcodeEx.S_IDX_BCD_MAKER_PN];
                    }
                    break;
                case BarcodeTypes.Unit:
                    if (BCD_TYPE == BarcodeEx.Y)
                    {
                        attribute = bcds[BarcodeEx.Y_IDX_BCD_UNIT];
                    }
                    else if (BCD_TYPE == BarcodeEx.Z)
                    {
                        attribute = bcds[BarcodeEx.Z_IDX_BCD_UNIT];
                    }
                    else if (BCD_TYPE == BarcodeEx.J)
                    {
                        attribute = bcds[BarcodeEx.J_IDX_BCD_UNIT];
                    }
                    else
                    {
                        attribute = "";
                    }
                    break;
                case BarcodeTypes.GrossWeight:
                    if (BCD_TYPE == BarcodeEx.Y)
                    {
                        attribute = bcds[BarcodeEx.Y_IDX_BCD_GW];
                    }
                    else if (BCD_TYPE == BarcodeEx.Z)
                    {
                        attribute = bcds[BarcodeEx.Z_IDX_BCD_GW];
                    }
                    else if (BCD_TYPE == BarcodeEx.J)
                    {
                        attribute = bcds[BarcodeEx.J_IDX_BCD_GW];
                    }
                    else
                    {
                        attribute = "";
                    }
                    break;
                case BarcodeTypes.NetWeight:
                    if (BCD_TYPE == BarcodeEx.Y)
                    {
                        attribute = bcds[BarcodeEx.Y_IDX_BCD_NW];
                    }
                    else if (BCD_TYPE == BarcodeEx.Z)
                    {
                        attribute = bcds[BarcodeEx.Z_IDX_BCD_NW];
                    }
                    else if (BCD_TYPE == BarcodeEx.J)
                    {
                        attribute = bcds[BarcodeEx.J_IDX_BCD_NW];
                    }
                    else
                    {
                        attribute = "";
                    }
                    break;
                case BarcodeTypes.CartonSize_L:
                    if (BCD_TYPE == BarcodeEx.Y)
                    {
                        attribute = bcds[BarcodeEx.Y_IDX_BCD_CARTON_SIZE_L];
                    }
                    else if (BCD_TYPE == BarcodeEx.Z)
                    {
                        attribute = bcds[BarcodeEx.Z_IDX_BCD_CARTON_SIZE_L];
                    }
                    else if (BCD_TYPE == BarcodeEx.J)
                    {
                        attribute = bcds[BarcodeEx.J_IDX_BCD_CARTON_SIZE_L];
                    }
                    else
                    {
                        attribute = "";
                    }
                    break;
                case BarcodeTypes.CartonSize_W:
                    if (BCD_TYPE == BarcodeEx.Y)
                    {
                        attribute = bcds[BarcodeEx.Y_IDX_BCD_CARTON_SIZE_W];
                    }
                    else if (BCD_TYPE == BarcodeEx.Z)
                    {
                        attribute = bcds[BarcodeEx.Z_IDX_BCD_CARTON_SIZE_W];
                    }
                    else if (BCD_TYPE == BarcodeEx.J)
                    {
                        attribute = bcds[BarcodeEx.J_IDX_BCD_CARTON_SIZE_W];
                    }
                    else
                    {
                        attribute = "";
                    }
                    break;
                case BarcodeTypes.CartonSize_H:
                    if (BCD_TYPE == BarcodeEx.Y)
                    {
                        attribute = bcds[BarcodeEx.Y_IDX_BCD_CARTON_SIZE_H];
                    }
                    else if (BCD_TYPE == BarcodeEx.Z)
                    {
                        attribute = bcds[BarcodeEx.Z_IDX_BCD_CARTON_SIZE_H];
                    }
                    else if (BCD_TYPE == BarcodeEx.J)
                    {
                        attribute = bcds[BarcodeEx.J_IDX_BCD_CARTON_SIZE_H];
                    }
                    else
                    {
                        attribute = "";
                    }
                    break;
                case BarcodeTypes.CustomerPn:
                    attribute = string.Empty;
                    break;
                case BarcodeTypes.Ref:
                    attribute = string.Empty;
                    break;
                case BarcodeTypes.Else:
                    attribute = string.Empty;
                    break;
                case BarcodeTypes.Coo:
                    if (BCD_TYPE == BarcodeEx.Y)
                    {
                        attribute = "";
                    }
                    else if (BCD_TYPE == BarcodeEx.Z)
                    {
                        attribute = "";
                    }
                    else if (BCD_TYPE == BarcodeEx.J)
                    {
                        attribute = "";
                    }
                    else
                    {
                        attribute = bcds[BarcodeEx.S_IDX_BCD_COO];
                    }
                    break;
                case BarcodeTypes.OrderNumber:
                    if (BCD_TYPE == BarcodeEx.Y)
                    {
                        attribute = "";
                    }
                    else if (BCD_TYPE == BarcodeEx.Z)
                    {
                        attribute = bcds[BarcodeEx.Z_IDX_BCD_ORDER_NUMBER];
                    }
                    else if (BCD_TYPE == BarcodeEx.J)
                    {
                        attribute = "";
                    }
                    else
                    {
                        attribute = "";
                    }
                    break;
                case BarcodeTypes.SalesNumber:
                    if (BCD_TYPE == BarcodeEx.Y)
                    {
                        attribute = "";
                    }
                    else if (BCD_TYPE == BarcodeEx.Z)
                    {
                        attribute = bcds[BarcodeEx.Z_IDX_BCD_SALES_NUMBER];
                    }
                    else if (BCD_TYPE == BarcodeEx.J)
                    {
                        attribute = "";
                    }
                    else
                    {
                        attribute = "";
                    }
                    break;
                case BarcodeTypes.SalesProjectNumber:
                    if (BCD_TYPE == BarcodeEx.Y)
                    {
                        attribute = "";
                    }
                    else if (BCD_TYPE == BarcodeEx.Z)
                    {
                        attribute = bcds[BarcodeEx.Z_IDX_BCD_SALES_PROJECT_NUMBER];
                    }
                    else if (BCD_TYPE == BarcodeEx.J)
                    {
                        attribute = "";
                    }
                    else
                    {
                        attribute = "";
                    }
                    break;
                case BarcodeTypes.BoxNumber:
                    if (BCD_TYPE == BarcodeEx.Y)
                    {
                        attribute = "";
                    }
                    else if (BCD_TYPE == BarcodeEx.Z)
                    {
                        attribute = bcds[BarcodeEx.Z_IDX_BCD_BOX_NUMBER];
                    }
                    else if (BCD_TYPE == BarcodeEx.J)
                    {
                        attribute = "";
                    }
                    else
                    {
                        attribute = "";
                    }
                    break;
                case BarcodeTypes.InvoiceNo:
                    if (BCD_TYPE == BarcodeEx.J)
                    {
                        try
                        {
                            attribute = bcds[BarcodeEx.J_IDX_BCD_INVOICE_NO];
                        }
                        catch
                        {
                            attribute = "";
                        }
                    }
                    else
                    {
                        attribute = "";
                    }
                    break;
                case BarcodeTypes.BondType:
                    if (BCD_TYPE == BarcodeEx.Y)
                    {
                        attribute = "";
                    }
                    else if (BCD_TYPE == BarcodeEx.Z)
                    {
                        attribute = "";
                    }
                    else if (BCD_TYPE == BarcodeEx.J)
                    {
                        attribute = bcds[BarcodeEx.J_IDX_BCD_BOND_TYPE];
                    }
                    else
                    {
                        attribute = "";
                    }
                    break;
                default:
                    attribute = string.Empty;
                    break;
            }
            return attribute.Trim().Replace(" ", "").ToUpper();
        }

    }
}
