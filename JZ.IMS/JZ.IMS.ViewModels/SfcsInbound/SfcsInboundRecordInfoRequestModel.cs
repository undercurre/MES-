/*
*┌──────────────────────────────────────────────────────────────┐
*│　描    述：入库记录信息表 查询实体
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
	/// 入库记录信息表 查询实体
	/// </summary>
	public class SfcsInboundRecordInfoRequestModel : PageModel
	{
		/// <summary>
		/// 工单ID（必传）
		/// </summary>
		public Decimal WO_ID { get; set; }

		/// <summary>
		/// MES入库单号
		/// </summary>
		public String INBOUND_NO { get; set; }

		/// <summary>
		/// ERP完工单号
		/// </summary>
		public String FINISHED_NO { get; set; }

	}




}
