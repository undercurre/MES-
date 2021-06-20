
using JZ.IMS.Core.Extensions;
using JZ.IMS.Models;
using JZ.IMS.ViewModels;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace JZ.IMS.Repository.Barcode
{
    public static class LabelPublic
    {
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
                    if (CODE.IsNullOrEmpty())
                    {
                        throw new Exception("条形码中没有流水号");
                    }
                    return CODE;
                }
            }
            return reel2dCode;
        }

        /// <summary>
        /// 获取Reel 2d CODE值
        /// </summary>
        /// <param name="reel2dCode"></param>
        /// <returns></returns>
        public static Reel GetReel(string reel2dCode)
        {
            Reel reel = new Reel();
            string[] splitArray = reel2dCode.Split2DBarcode();
            if (splitArray.Length < 2)
            {
                reel.CODE = BarcodeFilter.FormatBarcode(reel2dCode);
                return reel;
            }

            foreach (string item in splitArray)  //解析字符串
            {
                string temp = item.Trim();

                if (Regex.IsMatch(temp, BarcodeFilter.C_Prefix_ReelID
                    , RegexOptions.IgnoreCase | RegexOptions.IgnorePatternWhitespace))
                {
                    reel.CODE = Regex.Replace(temp, BarcodeFilter.C_Prefix_ReelID,
                        "", RegexOptions.IgnoreCase | RegexOptions.IgnorePatternWhitespace).ToUpper();
                    if (reel.CODE.IsNullOrEmpty())
                    {
                        throw new Exception("条形码中没有流水号");
                    }
                    int len = reel.CODE.Length;
                    if (reel.CODE.StartsWith("M") == false && len >= 13)
                    {
                        //35911 170401 12345
                        string endStr = reel.CODE.Substring(len - 9);

                        reel.VendorCode = reel.CODE.Replace(endStr, "");
                        //bool exists = VendorExists(reel.VendorCode);
                        //if (exists == false)
                        //{
                        //    reel.VendorCode = reel.CODE.Substring(0, 8);
                        //}
                    }

                }
                else if (Regex.IsMatch(temp, BarcodeFilter.C_Prefix_BOX
                    , RegexOptions.IgnoreCase | RegexOptions.IgnorePatternWhitespace))
                {
                    reel.CODE = Regex.Replace(temp, BarcodeFilter.C_Prefix_BOX,
                        "", RegexOptions.IgnoreCase | RegexOptions.IgnorePatternWhitespace).ToUpper();
                    if (reel.CODE.IsNullOrEmpty())
                    {
                        throw new Exception("条形码中没有流水号");
                    }
                }
                else if (Regex.IsMatch(temp, BarcodeFilter.C_Prefix_MitacPN
                    , RegexOptions.IgnoreCase | RegexOptions.IgnorePatternWhitespace))
                {
                    reel.PART_NO = Regex.Replace(temp, BarcodeFilter.C_Prefix_MitacPN,
                        "", RegexOptions.IgnoreCase | RegexOptions.IgnorePatternWhitespace).ToUpper();
                    if (reel.PART_NO.StartsWith("P"))
                        reel.PART_NO = reel.PART_NO.Substring(1);
                    if (reel.PART_NO.IsNullOrEmpty())
                    {
                        throw new Exception("条形码中料号值为空");
                    }
                }
                else if (Regex.IsMatch(temp, BarcodeFilter.C_Prefix_MakerPN
                    , RegexOptions.IgnoreCase | RegexOptions.IgnorePatternWhitespace))
                {
                    reel.MakerPN = Regex.Replace(temp, BarcodeFilter.C_Prefix_MakerPN,
                        "", RegexOptions.IgnoreCase | RegexOptions.IgnorePatternWhitespace).ToUpper();
                    if (reel.MakerPN.IsNullOrEmpty())
                    {
                        throw new Exception("条形码中制造商料号值为空");
                    }
                }
                else if (Regex.IsMatch(temp, BarcodeFilter.C_Prefix_Maker
                    , RegexOptions.IgnoreCase | RegexOptions.IgnorePatternWhitespace))
                {
                    reel.MakerName = Regex.Replace(temp, BarcodeFilter.C_Prefix_Maker,
                        "", RegexOptions.IgnoreCase | RegexOptions.IgnorePatternWhitespace).ToUpper();
                    if (reel.MakerName.IsNullOrEmpty())
                    {
                        throw new Exception("条形码中制造商名称为空");
                    }
                }
                else if (Regex.IsMatch(temp, BarcodeFilter.C_Prefix_Qty
                    , RegexOptions.IgnoreCase | RegexOptions.IgnorePatternWhitespace))
                {
                    string qty = Regex.Replace(temp, BarcodeFilter.C_Prefix_Qty,
                        "", RegexOptions.IgnoreCase | RegexOptions.IgnorePatternWhitespace).ToUpper();
                    if (qty.IsNullOrEmpty())
                    {
                        throw new Exception("条形码中数量为空");
                    }
                    decimal reelQty = 0;
                    try
                    {
                        reelQty = decimal.Parse(qty);
                        reel.Quantity = reelQty;
                        reel.CaseQty = reelQty;
                    }
                    catch
                    {
                        throw new Exception("条形码中数量填写格式错误");
                    }
                }
                else if (Regex.IsMatch(temp, BarcodeFilter.C_Prefix_Date
                    , RegexOptions.IgnoreCase | RegexOptions.IgnorePatternWhitespace))
                {
                    string dateCode = Regex.Replace(temp, BarcodeFilter.C_Prefix_Date,
                        "", RegexOptions.IgnoreCase | RegexOptions.IgnorePatternWhitespace).ToUpper();
                    if (dateCode.IsNullOrEmpty())
                    {
                        throw new Exception("条形码中生产日期为空");
                    }
                    //dateCode = dateCode.FixDateCode();
                    reel.DateCode = decimal.Parse(dateCode);
                }
                else if (Regex.IsMatch(temp, BarcodeFilter.C_Prefix_CoO
                    , RegexOptions.IgnoreCase | RegexOptions.IgnorePatternWhitespace))
                {
                    reel.COO = Regex.Replace(temp, BarcodeFilter.C_Prefix_CoO,
                        "", RegexOptions.IgnoreCase | RegexOptions.IgnorePatternWhitespace).ToUpper();
                }
                else if (Regex.IsMatch(temp, BarcodeFilter.C_Prefix_Lot
                    , RegexOptions.IgnoreCase | RegexOptions.IgnorePatternWhitespace))
                {
                    reel.LotCode = Regex.Replace(temp, BarcodeFilter.C_Prefix_Lot,
                        "", RegexOptions.IgnoreCase | RegexOptions.IgnorePatternWhitespace).ToUpper();
                    if (reel.LotCode.IsNullOrEmpty())
                    {
                        throw new Exception("条形码中批次号为空");
                    }
                }
                else if (Regex.IsMatch(temp, BarcodeFilter.C_Prefix_CustomerPN
                    , RegexOptions.IgnoreCase | RegexOptions.IgnorePatternWhitespace))
                {
                    reel.CustomerPN = Regex.Replace(temp, BarcodeFilter.C_Prefix_CustomerPN,
                        "", RegexOptions.IgnoreCase | RegexOptions.IgnorePatternWhitespace).ToUpper();
                }
                else if (Regex.IsMatch(temp, BarcodeFilter.C_Prefix_Ref
                    , RegexOptions.IgnoreCase | RegexOptions.IgnorePatternWhitespace))
                {
                    reel.REF = Regex.Replace(temp, BarcodeFilter.C_Prefix_Ref,
                        "", RegexOptions.IgnoreCase | RegexOptions.IgnorePatternWhitespace).ToUpper();
                }
            }
            if (reel.CODE.IsNullOrEmpty())
            {
                if (splitArray.Length == 1)
                {
                    reel.CODE = splitArray[0];
                }
                else
                {
                    //条形码格式错误
                    throw new Exception(string.Format("料盘条形码{0}解析错误，请检查!", reel2dCode));
                }
            }
            return reel;
        }
    }
}
