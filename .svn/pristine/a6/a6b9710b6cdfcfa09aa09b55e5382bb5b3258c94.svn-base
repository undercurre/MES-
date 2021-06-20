using JZ.IMS.Core.Utilities;
using JZ.IMS.WebApi.Controllers;
using Microsoft.Extensions.Localization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

/*----------------------------------------------------------------
// Copyright (C) 2019 佛山市嘉志信息科技有限公司
//
// 文件名：RadixConvertPublic.cs
// 文件功能描述：進制轉換
// 
// 創建標識：嘉志
//
// 修改標識：
// 修改描述：
//----------------------------------------------------------------*/
namespace JZ.IMS.WebApi.Common
{
    public static class RadixConvertPublic
    {
        private static char[] standardArray;
        private static string standardString;

        public static IStringLocalizer<SfcsRuncardRangerRulesController> _localizer;

        /// <summary>
        /// 初始化變量
        /// </summary>
        /// <param name="value">進制字符串</param>
        /// <returns></returns>
        private static void InitialParameters(string standard)
        {
            standard = standard.Trim().ToUpper();
            standardString = standard;
            standardArray = standard.ToCharArray();
        }

        /// <summary>
        /// 取得字符在字符數組的下標
        /// </summary>
        /// <param name="charArray">參考進制數組</param>
        /// <param name="value">待確認字符</param>
        /// <returns>字符在數組中的下標</returns>
        private static int GetIndexOfChar(char[] charArray, char value)
        {
            for (int i = 0; i < charArray.Length; i++)
            {
                if (charArray[i] == value)
                {
                    return i;
                }
            }
            return 0;
        }

        /// <summary>
        /// 將指定進制的String表示形式轉換為等效的64位有符號整數。
        /// BaseX->Long
        /// </summary>
        /// <param name="value">指定進制的String表示形式</param>
        /// <param name="fromBase">value對應的進制，必須為[2,36]</param>
        /// <returns>等效于value中的數字的64位有符號整數。如果value为null，則為零，0L代表Int64的零。</returns>
        private static long BaseXToBase10(string value, int fromBase)
        {
            long result = 0;
            value = value.Trim();
            if (string.IsNullOrEmpty(value))
            {
                return result;
            }

            value = value.ToUpper();
            for (int i = 0; i < value.Length; i++)
            {
                if (!standardString.Contains(value[i].ToString()))
                {
                    //字符 {0} 不在 {1} 进制的字符数组{2}中
                    throw new ArgumentException(string.Format(_localizer["Msg_CharNotInDigitalArray"], value[i], 
                        fromBase, standardString));
                }
                else
                {
                    try
                    {
                        result += (long)Math.Pow(fromBase, value.Length - i - 1) * GetIndexOfChar(standardArray, value[i]); 
                    }
                    catch
                    {
                        //进制转换运算溢出
                        throw new OverflowException(_localizer["Msg_Radix_Convert_Overflow"]);
                    }
                }
            }

            return result;
        }

        /// <summary>
        /// 將64位有符號整數轉換為等效的指定進制的String表示形式。
        /// Long->BaseX
        /// </summary>
        /// <param name="value">64位有符號整數</param>
        /// <param name="toBase">目標進制數，必須為[2,36]</param>
        /// <returns>轉換進制后的String表示形式</returns>
        private static string Base10ToBaseX(long value, int toBase)
        {
            int digitIndex = 0;
            long longPositive = Math.Abs(value);
            int radix = toBase;
            char[] outDigits = new char[64];

            if (longPositive == 0)
            {
                return "0";
            }

            for (digitIndex = 0; digitIndex < 64; digitIndex++)
            {
                if (longPositive == 0)
                {
                    break;
                }

                try
                {
                    // 取余運算
                    outDigits[outDigits.Length - digitIndex - 1] =
                        standardArray[longPositive % radix];

                    // 取整運算
                    longPositive /= radix;
                }
                catch
                {
                    //进制转换运算溢出
                    throw new OverflowException(_localizer["Msg_Radix_Convert_Overflow"]);
                }
            }

            return new string(outDigits, outDigits.Length - digitIndex, digitIndex);
        }

        /// <summary>
        /// 任意進制轉換,將fromBase進制表示的value轉換為toBase進制表示
        /// 核心處理流程：string（源头進制字符串）-> Int64 -> string(目标進制字符串)
        /// </summary>
        /// <param name="value">fromBase的String表示</param>
        /// <param name="sourceDigits">源頭進制參考字符串</param>
        /// <param name="targetDigits">目標進制參考字符串</param>
        /// <returns>目標進制的String表示形式</returns>
        public static string RadixConvert(string value, string sourceDigits, string targetDigits)
        {
            if (string.IsNullOrEmpty(value.Trim()))
            {
                return string.Empty;
            }

            sourceDigits = sourceDigits.Trim();
            targetDigits = targetDigits.Trim();
            int fromBase = sourceDigits.Length;
            int toBase = targetDigits.Length;

            if (fromBase < 2 || fromBase > 36)
            {
                //源头进制 {0} 不在定义范围[2..36]内
                throw new ArgumentException(String.Format(_localizer["Msg_SourceDigitalWrong"], fromBase));
            }

            if (toBase < 2 || toBase > 36)
            {
                //目标进制 {0}不在定义范围[2..36]内
                throw new ArgumentException(String.Format(_localizer["Msg_TargetDigitalWrong"], toBase));
            }

            InitialParameters(sourceDigits);
            long m = BaseXToBase10(value, fromBase);
            InitialParameters(targetDigits);
            string r = Base10ToBaseX(m, toBase);
            return r;
        }

        /// <summary>
        /// 進制累加方法
        /// </summary>
        /// <param name="value">累加基數值</param>
        /// <param name="standardDigits">進制字符串，進制轉換中的參考標準</param>
        /// <param name="quantity">累加數量</param>
        /// <param name="isAsc">true:加法,false:减法</param>
        /// <returns>累加后的進制字符串</returns>
        public static string RadixInc(string value, string standardDigits, int quantity,bool isAsc=true)
        {
            if(value.IsNullOrEmpty())
            {
                return string.Empty;
            }

            int vauleLength = value.Trim().Length;

            InitialParameters(standardDigits);
            int BaseX = standardDigits.Length;

            if (BaseX < 2 || BaseX > 36)
            {
                //进制 {0} 不在定义范围[2..36]内
                throw new ArgumentException(String.Format(_localizer["Msg_Radix_Scope_OverFlow"], BaseX));
            }

            long m =isAsc? BaseXToBase10(value, BaseX) + quantity: BaseXToBase10(value, BaseX) - quantity;
            string r = Base10ToBaseX(m, BaseX).PadLeft(vauleLength, standardArray[0]);
            return r;
        }

        /// <summary>
        /// 進制累減方法 
        /// </summary>
        /// <param name="value">累減基數值</param>
        /// <param name="standardDigits">進制字符串，進制轉換中的參考標準</param>
        /// <param name="quantity">累減數量</param>
        /// <returns>累減后的進制字符串</returns>
        public static string RadixDec(string value, string standardDigits, int quantity)
        {
            if (string.IsNullOrEmpty(value.Trim()))
            {
                return string.Empty;
            }

            int vauleLength = value.Trim().Length;

            InitialParameters(standardDigits);
            int BaseX = standardDigits.Length;

            if (BaseX < 2 || BaseX > 36)
            {
                ////进制 {0} 不在定义范围[2..36]内
                throw new ArgumentException(String.Format(_localizer["Msg_Radix_Scope_OverFlow"], BaseX));
            }

            long m = BaseXToBase10(value, BaseX);

            if (m - quantity < 0)
            {
                //递减数量超出范围
                throw new ArgumentException(_localizer["Msg_NumberOverFlow"]);
            }
            else
            {
                m -= quantity;
            }

            string r = Base10ToBaseX(m, BaseX).PadLeft(vauleLength, standardArray[0]);
            return r;
        }
    }
}
