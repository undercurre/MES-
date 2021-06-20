using System;
using System.Linq;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;

namespace JZ.IMS.Repository.Barcode
{
    public static class BarcodeFilter
    {
        public const string C_Prefix_3S = "^3S|^[(]3S[)]|^[[]3S[]]";
        public const string C_Prefix_ReelID = "^S|[(]S[)]|^[[]S[]]";
        public const string C_Prefix_MitacPN = "^P|^[(]P[)]|^[[]P[]]";
        public const string C_Prefix_Maker = "^M|^[(]M[)]|^[[]M[]]";
        public const string C_Prefix_MakerPN = "^1P|^[(]1P[)]|^[[]1P[]]";
        public const string C_Prefix_Qty = "^Q|^[(]Q[)]|^[[]Q[]]";
        public const string C_Prefix_Date = "^9D|^[(]9D[)]|^[[]9D[]]";
        public const string C_Prefix_CoO = "^4L|^[(]4L[)]|^[[]4L[]]";
        public const string C_Prefix_Lot = "^1T|^[(]1T[)]|^[[]1T[]]";
        public const string C_Prefix_CustomerPN = "^CP|^[(]CP[)]|^[[]CP[]]";
        public const string C_Prefix_Ref = "^R|^[(]R[)]|^[[]R[]]";
        public const string C_Prefix_BPPO = "^K|^[(]K[)]|^[[]K[]]";
        public const string C_Prefix_QVL = "^V|^[(]V[)]|^[[]V[]]";
        public const string C_Prefix_LOT_CODE_P = "^P|^[(]P[)]|^[[]P[]]";
        public const string C_Prefix_LOT_CODE_1T = "^1T|^[(]1T[)]|^[[]1T[]]";
        public const string C_Prefix_DATE_CODE_D = "^D|^[(]D[)]|^[[]D[]]";
        public const string C_Prefix_BOX = "^B|^[(]B[)]|^[[]B[]]";
        public const string C_Prefix_RCV_SN = "^N|^[(]N[)]|^[[]N[]]";
        //public const string C_Prefix_PL_NO = "^C|^[(]C[)]|^[[]C[]]";
        public const string C_Prefix_P2 = "^H|^[(]H[)]|^[[]H[]]";
        public const string C_Prefix_LOCATOR = "^L|^[(]L[)]|^[[]L[]]";
        public const string C_Prefix_PALLET = "^U|^[(]U[)]|^[[]U[]]";

        public const string C_P2QTY_PREFIX = "^X|^[(]X[)]|^[[]X[]]";


        public const string Reel2DBarcodePattern = "S{0}|P{1}|1P{2}|Q{3}|9D{4}|1T{5}|4L{6}|M{7}|CP{8}|R{9}|X{10}";

        public const string SplitChars = @"|\;";

        public static bool Is2DBarcodeString(this string originalString)
        {
            string[] bcds = originalString.Split(BarcodeFilter.SplitChars.ToCharArray());
            if (bcds.Length > 2)
                return true;
            else
                return false;
        }

        public static bool FilterBarcode(string originalStr, string c_str_patern, ref string finalBarcode)
        {
            if (Regex.IsMatch(originalStr, c_str_patern, RegexOptions.IgnoreCase | RegexOptions.IgnorePatternWhitespace))
            {
                finalBarcode = Regex.Replace(originalStr, c_str_patern, "", RegexOptions.IgnoreCase | RegexOptions.IgnorePatternWhitespace);
                return true;
            }
            else
            {
                return false;
            }
        }


        public static string FilterBarcode(string originalStr, string c_str_patern)
        {
            if (Regex.IsMatch(originalStr, c_str_patern, RegexOptions.IgnoreCase | RegexOptions.IgnorePatternWhitespace))
                return Regex.Replace(originalStr, c_str_patern, "", RegexOptions.IgnoreCase | RegexOptions.IgnorePatternWhitespace);
            else
                return originalStr;
        }

        public static string GetBarcode(this string _2dBarcodeString, string pattern)
        {
            string[] bcds = _2dBarcodeString.Split(BarcodeFilter.SplitChars.ToCharArray());
            string result = string.Empty;
            bool isMatch = false;
            foreach (string bcd in bcds)
            {
                if (string.IsNullOrEmpty(bcd))
                    continue;
                isMatch = BarcodeFilter.FilterBarcode(bcd, pattern,ref result);
                if (isMatch)
                {
                    return result;
                }
            }
            return _2dBarcodeString;
        }


        public static ReelElements Converto2dToReel(string _2Dstring)
        {
            ReelElements reel = new ReelElements();
            string[] bcds = _2Dstring.Split(SplitChars.ToCharArray());
            string temp = string.Empty;
            foreach (string bcd in bcds)
            {
                if (string.IsNullOrEmpty(bcd)) continue;

                temp = FilterBarcode(bcd, C_Prefix_CoO);
                if (temp != bcd)
                {
                    reel.Coo = temp;
                    break;
                }
                temp = FilterBarcode(bcd, C_Prefix_CustomerPN);
                if (temp != bcd)
                {
                    reel.CustomerPN = temp;
                    break;
                }
                temp = FilterBarcode(bcd, C_Prefix_Date);
                if (temp != bcd)
                {
                    reel.DateCode = temp;
                    break;
                }
                temp = FilterBarcode(bcd, C_Prefix_Lot);
                if (temp != bcd)
                {
                    reel.LotCode = temp;
                    break;
                }
                temp = FilterBarcode(bcd, C_Prefix_Maker);
                if (temp != bcd)
                {
                    reel.MakerCode = temp;
                    break;
                }
                temp = FilterBarcode(bcd, C_Prefix_MakerPN);
                if (temp != bcd)
                {
                    reel.MakerPN = temp;
                    break;
                }
                temp = FilterBarcode(bcd, C_Prefix_MitacPN);
                if (temp != bcd)
                {
                    reel.PartNo = temp;
                    break;
                }
                temp = FilterBarcode(bcd, C_Prefix_Qty);
                if (temp != bcd)
                {
                    reel.Qty = temp;
                    break;
                }
                temp = FilterBarcode(bcd, C_Prefix_ReelID);
                if (temp != bcd)
                {
                    reel.ReelCode = temp;
                    break;
                }
                temp = FilterBarcode(bcd, C_Prefix_Ref);
                if (temp != bcd)
                {
                    reel.Ref = temp;
                    break;
                }
            }

            return reel;

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
            //if (result.ToUpper().StartsWith("SMT")) return result;
            //if (result.ToUpper().StartsWith("S6") && result.Length < 10 ) return result;
            //if (result.ToUpper().StartsWith("S7") && result.Length < 10) return result;
            //if (result.ToUpper().StartsWith("S8") && result.Length < 10) return result;
            //if (result.ToUpper().StartsWith("S11") && result.Length < 10) return result;
            //if (result.ToUpper().StartsWith("S06") && result.Length < 10) return result;
            //if (result.ToUpper().StartsWith("S07") && result.Length < 10) return result;
            //if (result.ToUpper().StartsWith("S08") && result.Length < 10) return result;
            //if (result.ToUpper().StartsWith("B") && result.Length < 9) return result;
            //if (result.ToUpper().StartsWith("S") && result.Length < 9) return result;

            if (originalStr.StartsWith("SSY")) return originalStr;
            if (originalStr.StartsWith("SY")) return originalStr;

            result = originalStr.GetBarcode(C_Prefix_3S);
            if (result != originalStr) return result;
            result = originalStr.GetBarcode(C_Prefix_ReelID);
            if (result != originalStr) return result;
            result = originalStr.GetBarcode(C_Prefix_MitacPN);
            if (result != originalStr) return result;
            result = originalStr.GetBarcode(C_Prefix_MakerPN);
            if (result != originalStr) return result;
            result = originalStr.GetBarcode(C_Prefix_Qty);
            if (result != originalStr) return result;
            result = originalStr.GetBarcode(C_Prefix_Date);
            if (result != originalStr) return result;
            result = originalStr.GetBarcode(C_Prefix_CoO);
            if (result != originalStr) return result;
            result = originalStr.GetBarcode(C_Prefix_Lot);
            if (result != originalStr) return result;
            result = originalStr.GetBarcode(C_Prefix_CustomerPN);
            if (result != originalStr) return result;
            result = originalStr.GetBarcode(C_Prefix_Ref);
            if (result != originalStr) return result;
            //result = originalStr.GetBarcode(C_Prefix_BPPO);
            //if (result != originalStr) return result;
            result = originalStr.GetBarcode(C_Prefix_QVL);
            if (result != originalStr) return result;
            result = originalStr.GetBarcode(C_Prefix_LOT_CODE_P);
            if (result != originalStr) return result;
            result = originalStr.GetBarcode(C_Prefix_LOT_CODE_1T);
            if (result != originalStr) return result;
            result = originalStr.GetBarcode(C_Prefix_DATE_CODE_D);
            if (result != originalStr) return result;
            result = originalStr.GetBarcode(C_Prefix_BOX);
            if (result != originalStr) return result;
            result = originalStr.GetBarcode(C_Prefix_RCV_SN);
            if (result != originalStr) return result;
            //result = originalStr.GetBarcode(C_Prefix_PL_NO);
            //if (result != originalStr) return result;
            result = originalStr.GetBarcode(C_Prefix_P2);
            if (result != originalStr) return result;
            result = originalStr.GetBarcode(C_Prefix_LOCATOR);
            if (result != originalStr) return result;
            result = originalStr.GetBarcode(C_Prefix_PALLET);
            if (result != originalStr) return result;
            return result;
        }


    }
}
