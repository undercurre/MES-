/*
*┌──────────────────────────────────────────────────────────────┐
*│　描    述： 更新或者新增实体                                                    
*│　作    者：嘉志科技                                              
*│　版    本：2.0   模板代码自动生成                                              
*│　创建时间：2020-09-03 14:33:55                            
*└──────────────────────────────────────────────────────────────┘
*┌──────────────────────────────────────────────────────────────┐
*│　命名空间: JZ.IMS.ViewModels                                  
*│　类    名：SmtReel                                     
*└──────────────────────────────────────────────────────────────┘
*/
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace JZ.IMS.ViewModels
{
	/// <summary>
	/// 嘉志科技
	/// 2020-09-03 14:33:55
	///  更新或者新增实体
	/// </summary>
	public class SmtReelAddOrModifyModel
	{
		public Decimal ID {get;set;}

		/// <summary>
		/// 批次号
		/// </summary>
		public String BATCH_NO {get;set;}

		/// <summary>
		/// 物料条码
		/// </summary>
		public String REEL_ID {get;set;}

		/// <summary>
		/// 工单
		/// </summary>
		public String WO_NO {get;set;}

		/// <summary>
		/// 线体ID
		/// </summary>
		public Decimal? SMT_LINE_ID {get;set;}

		/// <summary>
		/// 站位ID
		/// </summary>
		public Decimal? STATION_ID {get;set;}

		/// <summary>
		/// 板面
		/// </summary>
		public Decimal? PCB_SIDE {get;set;}

		public Decimal? TABLE_NO {get;set;}

		/// <summary>
		/// 物料清单DTLID
		/// </summary>
		public Decimal? PLACEMENT_DTL_ID {get;set;}

		/// <summary>
		/// 位置
		/// </summary>
		public String LOCATION {get;set;}

		/// <summary>
		/// 飞达
		/// </summary>
		public String FEEDER {get;set;}

		/// <summary>
		/// 料号
		/// </summary>
		public String PART_NO {get;set;}

		/// <summary>
		/// 目标数
		/// </summary>
		public Decimal ORIGINAL_QTY {get;set;}

		public Decimal PREPARED_QTY {get;set;}

		/// <summary>
		/// 使用数量
		/// </summary>
		public Decimal? USED_QTY {get;set;}

		public Decimal ONHAND_QTY {get;set;}

		/// <summary>
		/// 单位用量
		/// </summary>
		public Decimal? UNIT_QTY {get;set;}

		public Decimal? PCB_COUNT {get;set;}

		public String VENDOR_CODE {get;set;}

		public String VENDOR_NAME {get;set;}

		public String DATE_CODE {get;set;}

		public String LOT_CODE {get;set;}

		public String MAKER_CODE {get;set;}

		public String MAKER_NAME {get;set;}

		public String MAKER_PN {get;set;}

		/// <summary>
		/// 状态
		/// </summary>
		public Decimal STATUS {get;set;}

		public String OFFLINE_PREPARE_BY {get;set;}

		public DateTime? OFFLINE_PREPARE_TIME {get;set;}

		/// <summary>
		/// 上料人员
		/// </summary>
		public String SUPPLY_BY {get;set;}

		/// <summary>
		/// 上料时间
		/// </summary>
		public DateTime? SUPPLY_TIME {get;set;}

		public String ON_SMT_BY {get;set;}

		public DateTime? ON_SMT_TIME {get;set;}

		public String TAKEOFF_BY {get;set;}

		public DateTime? TAKEOFF_TIME {get;set;}

		public String VERIFY_BY {get;set;}

		public DateTime? VERIFY_TIME {get;set;}

		public String USEDUP_BY {get;set;}

		public DateTime? USEDUP_TIME {get;set;}

		public String BODYMARK {get;set;}

		public String LCR_VALUE {get;set;}

		public String LCR_UNIT {get;set;}

		public String MEASURE_BY {get;set;}

		public String MEASURE_FLAG {get;set;}

		public Decimal? OPERATION_TYPE {get;set;}

		public String REEL_SLD {get;set;}


	}
}
