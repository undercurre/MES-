using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace JZ.IMS.WebApi.Common
{
    public enum BarcodeTypes
    {
        /// <summary>
        /// 物料条码标签
        /// </summary>
        ReelID = 0,
        /// <summary>
        /// 公司内部料号
        /// </summary>
        PartNo = 1,
        /// <summary>
        /// 制造商料号
        /// </summary>
        MakerPart = 2,
        /// <summary>
        /// 制造商名称
        /// </summary>
        Maker = 3,
        /// <summary>
        /// 数量
        /// </summary>
        Quantity = 4,
        /// <summary>
        /// 制造日期
        /// </summary>
        DateCode = 5,
        /// <summary>
        /// 原产国
        /// </summary>
        Coo = 6,
        /// <summary>
        /// 生产批次号
        /// </summary>
        LotCode = 7,
        /// <summary>
        /// 客户料号
        /// </summary>
        CustomerPn = 8,
        /// <summary>
        /// Reference信息
        /// </summary>
        Ref = 9,
        /// <summary>
        /// BOX
        /// </summary>
        Box = 10,
        /// <summary>
        /// ERP Batch No
        /// </summary>
        BatchNo = 11,
        /// <summary>
        /// 供应商代码
        /// </summary>
        VendorCode = 12,

        #region 2020-6-2 LJW ADD
        /// <summary>
        /// 类型 
        /// </summary>
        Type = 13,
        /// <summary>
        /// 工厂
        /// </summary>
        BU,
        /// <summary>
        /// 库位
        /// </summary>
        SIC,
        /// <summary>
        /// 采购订单编号
        /// </summary>
        POCode,
        /// <summary>
        /// 出厂日期
        /// </summary>
        ShippingDate,
        /// <summary>
        /// 品牌  
        /// </summary>
        Brand,
        /// <summary>
        /// 单位（数量/单位）
        /// </summary>
        Unit,
        /// <summary>
        /// 毛重
        /// </summary>
        GrossWeight,
        /// <summary>
        /// 净重
        /// </summary>
        NetWeight,
        /// <summary>
        /// 包装箱长
        /// </summary>
        CartonSize_L,
        /// <summary>
        /// 包装箱宽
        /// </summary>
        CartonSize_W,
        /// <summary>
        /// 包装箱高
        /// </summary>
        CartonSize_H,
        #endregion

        /// <summary>
        /// 生产订单编号
        /// </summary>
        OrderNumber,

        /// <summary>
        /// 销售项目编号
        /// </summary>
        SalesProjectNumber,

        /// <summary>
        /// 销售项目项次号
        /// </summary>
        SalesNumber,

        /// <summary>
        /// 生产箱号
        /// </summary>
        BoxNumber,

        /// <summary>
        /// 保税类型
        /// </summary>
        BondType,

        /// <summary>
        /// 发票号
        /// </summary>
        InvoiceNo,

        /// <summary>
        /// 其他
        /// </summary>
        Else = 99
    }
}
