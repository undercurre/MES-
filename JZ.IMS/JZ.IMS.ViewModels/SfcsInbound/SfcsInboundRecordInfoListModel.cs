/*
*┌──────────────────────────────────────────────────────────────┐
*│　描    述：入库记录信息表 列表显示实体                                                   
*│　作    者：嘉志科技                                              
*│　版    本：2.0   模板代码自动生成                                              
*│　创建时间：2021-04-27 17:00:28                            
*└──────────────────────────────────────────────────────────────┘
*┌──────────────────────────────────────────────────────────────┐
*│　命名空间: JZ.IMS.ViewModels                                  
*│　类    名：SfcsInboundRecordInfo                                     
*└──────────────────────────────────────────────────────────────┘
*/
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace JZ.IMS.ViewModels
{
	/// <summary>
	/// 嘉志科技
	/// 2021-04-27 17:00:28
	/// 入库记录信息表 列表显示实体
	/// </summary>
	public class SfcsInboundRecordInfoListModel
	{
		/// <summary>
		/// 主键
		/// </summary>
		public Decimal ID {get;set;}

		/// <summary>
		/// 工单ID
		/// </summary>
		public Decimal WO_ID {get;set;}

		/// <summary>
		/// MES入库单号
		/// </summary>
		public String INBOUND_NO {get;set;}

		/// <summary>
		/// ERP完工单号
		/// </summary>
		public String FINISHED_NO {get;set;}

		/// <summary>
		/// 入库数量
		/// </summary>
		public Decimal INBOUND_QTY {get;set;}

		/// <summary>
		/// 状态：0 未处理 1 已处理
		/// </summary>
		public String STATUS {get;set;}

		/// <summary>
		/// 入库信息
		/// </summary>
		public String INBOUND_INFO {get;set;}

		/// <summary>
		/// 创建时间
		/// </summary>
		public DateTime CREATE_TIME {get;set;}

		/// <summary>
		/// 创建人
		/// </summary>
		public String CREATE_BY {get;set;}

        /// <summary>
        /// 修改时间
        /// </summary>
        public DateTime UPDATE_TIME { get; set; }

        /// <summary>
        /// 修改人
        /// </summary>
        public String UPDATE_BY { get; set; }

		public String ERP_BILL_NO { get; set; }
		public String AUTO_EDITOR { get; set; }

	}
}
