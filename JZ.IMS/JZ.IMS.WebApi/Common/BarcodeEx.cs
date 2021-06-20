using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace JZ.IMS.WebApi.Common
{
     public static class BarcodeEx
    {
        /// <summary>
        /// 国内原材料标签
        /// </summary>
        public const string Y = "Y";
        /// <summary>
        /// 自制件标签
        /// </summary>
        public const string Z = "Z";
        /// <summary>
        /// 内箱标签
        /// </summary>
        public const string S = "S";
        /// <summary>
        /// 进口标签
        /// </summary>
        public const string J = "J";

        /// <summary>
        /// 国内原材料标签
        /// </summary>
        public const int Y_IDX_BCD_TYPE = 0;
        public const int Y_IDX_BCD_PART = 1;
        public const int Y_IDX_BCD_VENDOR_CODE = 2;
        public const int Y_IDX_BCD_QTY = 3;
        public const int Y_IDX_BCD_CODE = 4;
        public const int Y_IDX_BCD_GW = 5;
        public const int Y_IDX_BCD_NW = 6;
        public const int Y_IDX_BCD_BU = 7;
        public const int Y_IDX_BCD_SIC = 8;
        public const int Y_IDX_BCD_UNIT = 9;
        public const int Y_IDX_BCD_PO = 10;
        public const int Y_IDX_BCD_CARTON_SIZE_L = 11;
        public const int Y_IDX_BCD_CARTON_SIZE_W = 12;
        public const int Y_IDX_BCD_CARTON_SIZE_H = 13;
        public const int Y_IDX_BCD_LOT = 14;
        public const int Y_IDX_BCD_DELIVERY_DATE = 15;
        public const int Y_IDX_BCD_PRODUCTION_DATE = 16;
        public const int Y_IDX_BCD_MAKER_PN = 17;
        public const int Y_IDX_BCD_BRAND = 18;
        public const int Y_IDX_BCD_MAKER_NAME = 19;


        /// <summary>
        /// 自制件标签
        /// </summary>
        public const int Z_IDX_BCD_TYPE = 0;
        public const int Z_IDX_BCD_PART = 1;
        public const int Z_IDX_BCD_VENDOR_CODE = 2;
        public const int Z_IDX_BCD_QTY = 3;
        public const int Z_IDX_BCD_CODE = 4;
        public const int Z_IDX_BCD_GW = 5;
        public const int Z_IDX_BCD_NW = 6;
        public const int Z_IDX_BCD_BU = 7;
        public const int Z_IDX_BCD_SIC = 8;
        public const int Z_IDX_BCD_UNIT = 9;
        public const int Z_IDX_BCD_PO = 10;
        public const int Z_IDX_BCD_CARTON_SIZE_L = 11;
        public const int Z_IDX_BCD_CARTON_SIZE_W = 12;
        public const int Z_IDX_BCD_CARTON_SIZE_H = 13;
        public const int Z_IDX_BCD_LOT = 14;
        public const int Z_IDX_BCD_DELIVERY_DATE = 15;
        public const int Z_IDX_BCD_PRODUCTION_DATE = 16;
        public const int Z_IDX_BCD_ORDER_NUMBER = 17;
        public const int Z_IDX_BCD_SALES_NUMBER = 18;
        public const int Z_IDX_BCD_SALES_PROJECT_NUMBER = 19;
        public const int Z_IDX_BCD_BOX_NUMBER = 20;
        public const int Z_IDX_BCD_BRAND = 21;
        public const int Z_IDX_BCD_MAKER_NAME = 22;
        public const int Z_IDX_BCD_MAKER_PN = 1;

        /// <summary>
        /// 内箱标签
        /// </summary>
        public const int S_IDX_BCD_TYPE = 0;
        public const int S_IDX_BCD_PART = 1;
        public const int S_IDX_BCD_CODE = 2;
        public const int S_IDX_BCD_QTY = 3;
        public const int S_IDX_BCD_BRAND = 4;
        public const int S_IDX_BCD_VENDOR_CODE = 5;
        public const int S_IDX_BCD_MAKER_NAME = 6;
        public const int S_IDX_BCD_COO = 7;
        public const int S_IDX_BCD_MAKER_PN = 8;
        public const int S_IDX_BCD_PRODUCTION_DATE = 9;
        public const int S_IDX_BCD_DELIVERY_DATE = 9;
        public const int S_IDX_BCD_LOT = 10;

        /// <summary>
        ///  进口材料标签
        /// </summary>
        public const int J_IDX_BCD_TYPE = 0;
        public const int J_IDX_BCD_PART = 1;
        public const int J_IDX_BCD_VENDOR_CODE = 2;
        public const int J_IDX_BCD_QTY = 3;
        public const int J_IDX_BCD_CODE = 4;
        public const int J_IDX_BCD_GW = 5;
        public const int J_IDX_BCD_NW = 6;
        public const int J_IDX_BCD_BU = 7;
        public const int J_IDX_BCD_SIC = 8;
        public const int J_IDX_BCD_UNIT = 9;
        public const int J_IDX_BCD_PO = 10;
        public const int J_IDX_BCD_CARTON_SIZE_L = 11;
        public const int J_IDX_BCD_CARTON_SIZE_W = 12;
        public const int J_IDX_BCD_CARTON_SIZE_H = 13;
        public const int J_IDX_BCD_LOT = 14;
        public const int J_IDX_BCD_DELIVERY_DATE = 15;
        public const int J_IDX_BCD_PRODUCTION_DATE = 16;
        public const int J_IDX_BCD_MAKER_PN = 17;
        public const int J_IDX_BCD_BRAND = 18;
        public const int J_IDX_BCD_MAKER_NAME = 19;
        public const int J_IDX_BCD_BOND_TYPE = 20;
        public const int J_IDX_BCD_INVOICE_NO = 21;

    }
}
