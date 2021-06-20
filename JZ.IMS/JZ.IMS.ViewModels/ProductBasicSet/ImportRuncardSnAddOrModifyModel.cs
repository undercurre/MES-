/*
*┌──────────────────────────────────────────────────────────────┐
*│　描    述：特殊SN表 更新或者新增实体                                                    
*│　作    者：嘉志科技                                              
*│　版    本：2.0   模板代码自动生成                                              
*│　创建时间：2020-10-24 14:15:40                            
*└──────────────────────────────────────────────────────────────┘
*┌──────────────────────────────────────────────────────────────┐
*│　命名空间: JZ.IMS.ViewModels                                  
*│　类    名：ImportRuncardSn                                     
*└──────────────────────────────────────────────────────────────┘
*/
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace JZ.IMS.ViewModels
{
	/// <summary>
	/// 嘉志科技
	/// 2020-10-24 14:15:40
	/// 特殊SN表 更新或者新增实体
	/// </summary>
	public class ImportRuncardSnAddOrModifyModel
	{
		/// <summary>
		/// 主键
		/// </summary>
		public Decimal ID {get;set; }

		/// <summary>
		/// 主表id（不可修改）导入sn主表id IMPORT_RUNCARD_HEADER.ID
		/// </summary>
		public Decimal HEADER_ID { get; set; }

		/// <summary>
		/// 工单号
		/// </summary>
		public String WO_NO {get;set;}

		/// <summary>
		/// SN流水号
		/// </summary>
		public String SN {get;set;}

		/// <summary>
		/// 父级流水号 暂无使用
		/// </summary>
		public String PARENT_SN {get;set;}

		/// <summary>
		/// 制程ID SFCS_ROUTES.ID
		/// </summary>
		public Decimal? ROUTE_ID {get;set;}

		/// <summary>
		/// 是否启用
		/// </summary>
		public String ENABLE {get;set;}

		/// <summary>
		/// 创建时间
		/// </summary>
		public DateTime? CREATE_TIME {get;set;}

		/// <summary>
		/// 创建人
		/// </summary>
		public String CREATE_BY {get;set;}

		/// <summary>
		/// 更新时间
		/// </summary>
		public DateTime? UPDATE_TIME {get;set;}

		/// <summary>
		/// 更新人
		/// </summary>
		public String UPDATE_BY {get;set;}

		/// <summary>
		/// 备用字段
		/// </summary>
		public String ATTRIBUTE1 {get;set;}

		/// <summary>
		/// 备用字段
		/// </summary>
		public String ATTRIBUTE2 {get;set;}

		/// <summary>
		/// 备用字段
		/// </summary>
		public String ATTRIBUTE3 {get;set;}

		/// <summary>
		/// 备用字段
		/// </summary>
		public String ATTRIBUTE4 {get;set;}

		/// <summary>
		/// 备用字段
		/// </summary>
		public String ATTRIBUTE5 {get;set;}

		/// <summary>
		/// 主卡IMEI
		/// </summary>
        public String MAIN_CARD_IMEI { get; set; } = "";

        /// <summary>
        /// 副卡IMEI
        /// </summary>
        public String MINOR_CARD_IMEI { get; set; } = "";

        /// <summary>
        /// BT
        /// </summary>
        public String BT { get; set; } = "";

        /// <summary>
        /// MAC
        /// </summary>
        public String MAC { get; set; } = "";

        /// <summary>
        /// MEID
        /// </summary>
        public String MEID { get; set; } = "";

		/// <summary>
		/// 产品类别
		/// </summary>
		public String PART_TYPE { get; set; }

		/// <summary>
		/// 产品代码（PN）
		/// </summary>
		public String PART_PN { get; set; }

		/// <summary>
		/// 商品代码(EAN)
		/// </summary>
		public String PART_EAN { get; set; }

		/// <summary>
		/// 客户
		/// </summary>
		public String CUSTOMER { get; set; }

		/// <summary>
		/// 客户机型
		/// </summary>
		public String CUSTOMER_MODEL { get; set; }

		/// <summary>
		/// 客户代码
		/// </summary>
		public String CUSTOMER_CODE { get; set; }

		/// <summary>
		/// 发货地
		/// </summary>
		public String SUPPLY_LOCATION { get; set; }

		/// <summary>
		/// 交货地
		/// </summary>
		public String DELIVERY_LOCATION { get; set; }

		/// <summary>
		/// 硬件版本
		/// </summary>
		public String HARDWARE_VERSION { get; set; }

		/// <summary>
		/// 软件版本
		/// </summary>
		public String SOFTWARE_VERSION { get; set; }

		/// <summary>
		/// 颜色
		/// </summary>
		public String COLOUR { get; set; }

		/// <summary>
		/// 客户批次号
		/// </summary>
		public String CUSTOMER_BATCH_NO { get; set; }

		/// <summary>
		/// 净重（外箱）
		/// </summary>
		public String OUTER_BOX_NET_WEIGHT { get; set; }

		/// <summary>
		/// 毛重(外箱)
		/// </summary>
		public String OUTER_BOX_GROSS_WEIGHT { get; set; }

		/// <summary>
		/// 数量（外箱）
		/// </summary>
		public String OUTER_BOX_QTY { get; set; }

		/// <summary>
		/// SN2
		/// </summary>
		public String SN2 { get; set; }

	}
}
