/*
*┌──────────────────────────────────────────────────────────────┐
*│　描    述：烧录结果信息记录
                                                    
*│　作    者：嘉志科技                                              
*│　版    本：2.0   模板代码自动生成                                              
*│　创建时间：2021-01-29 16:03:59                            
*└──────────────────────────────────────────────────────────────┘
*┌──────────────────────────────────────────────────────────────┐
*│　命名空间: JZ.IMS.Models                                  
*│　类    名：MesBurnResultHistory                                     
*└──────────────────────────────────────────────────────────────┘
*/
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace JZ.IMS.Models
{
	/// <summary>
	/// 嘉志科技
	/// 2021-01-29 16:03:59
	/// 烧录结果信息记录

	/// </summary>
	[Table("MES_BURN_RESULT_HISTORY")]
	public partial class MesBurnResultHistory
	{
		/// <summary>
		/// 主键
		/// </summary>
		[Key]
		public Decimal ID {get;set;}

		/// <summary>
		/// 供应商名称
		/// </summary>
		[MaxLength(50)]
		public String HANDLERID {get;set;}

		/// <summary>
		/// 供应商简写
		/// </summary>
		[MaxLength(50)]
		public String HANDLERMFR {get;set;}

		/// <summary>
		/// 设备型号
		/// </summary>
		[MaxLength(50)]
		public String HANDLERTYPE {get;set;}

		/// <summary>
		/// 客户名称
		/// </summary>
		[MaxLength(50)]
		public String HANDLERINFO {get;set;}

		/// <summary>
		/// 烧录器型号
		/// </summary>
		[MaxLength(50)]
		public String PROGRAMMERTYPE {get;set;}

		/// <summary>
		/// Total烧录芯片数量
		/// </summary>
		[MaxLength(22)]
		public Decimal? ICTOTAL {get;set;}

		/// <summary>
		/// Total烧录PASS的芯片数量
		/// </summary>
		[MaxLength(22)]
		public Decimal? ICPASS {get;set;}

		/// <summary>
		/// Total烧录Fail的芯片数量
		/// </summary>
		[MaxLength(22)]
		public Decimal? ICFail {get;set;}

		/// <summary>
		/// UPH
		/// </summary>
		[MaxLength(22)]
		public Decimal? UPH {get;set;}

		/// <summary>
		/// 当前Socket数量
		/// </summary>
		[MaxLength(22)]
		public Decimal? SOCKETNUMBER {get;set;}

		/// <summary>
		/// 最大支持Socket数量
		/// </summary>
		[MaxLength(22)]
		public Decimal? MAXSOCKETNUMBER {get;set;}

		/// <summary>
		/// 良率
		/// </summary>
		[MaxLength(22)]
		public Decimal? YIELD {get;set;}

		/// <summary>
		/// 预估的UPH
		/// </summary>
		[MaxLength(22)]
		public Decimal? ESTIMATEUPH {get;set;}

		/// <summary>
		/// 预估结束时间
		/// </summary>
		[MaxLength(7)]
		public DateTime? ESTIMATEENDTIME {get;set;}

		/// <summary>
		/// 平均每颗芯片烧录时间
		/// </summary>
		[MaxLength(22)]
		public Decimal? CYCLETIMEEACHIC {get;set;}

		[MaxLength(100)]
		public String ERRORMESSAGE {get;set;}

		[MaxLength(100)]
		public String ERRORCODE {get;set;}

		/// <summary>
		/// 工单号
		/// </summary>
		[MaxLength(100)]
		public String LOTNUMBER {get;set;}

		/// <summary>
		/// 工单数量
		/// </summary>
		[MaxLength(22)]
		public Decimal? LOTSIZE {get;set;}

		/// <summary>
		/// IC厂商
		/// </summary>
		[MaxLength(50)]
		public String ICMANUFACTURE {get;set;}

		/// <summary>
		/// IC型号
		/// </summary>
		[MaxLength(22)]
		public Decimal? ICTYPE {get;set;}

		/// <summary>
		/// 工程名称
		/// </summary>
		[MaxLength(30)]
		public String CONFIG {get;set;}

		[MaxLength(255)]
		public String LASERCONTENT {get;set;}

		/// <summary>
		/// 工单开始时间
		/// </summary>
		[MaxLength(7)]
		public DateTime? LOTSTARTTIME {get;set;}

		/// <summary>
		/// 操作员工的ID
		/// </summary>
		[MaxLength(10)]
		public String OPERATOR {get;set;}

		/// <summary>
		/// 工单结束时间
		/// </summary>
		[MaxLength(7)]
		public DateTime? LOTENDTIME {get;set;}

		/// <summary>
		/// 量产的日期
		/// </summary>
		[MaxLength(7)]
		public DateTime? PRODUCTIONDATE {get;set;}

		/// <summary>
		/// 量产的时间
		/// </summary>
		[MaxLength(50)]
		public String PRODUCTIME {get;set;}

		/// <summary>
		/// 量产的错误时间
		/// </summary>
		[MaxLength(7)]
		public DateTime? ERRORTIME {get;set;}

		[MaxLength(50)]
		public String BULK {get;set;}

		/// <summary>
		/// 校验和
		/// </summary>
		[MaxLength(50)]
		public String CHESKSUM {get;set;}

		[MaxLength(50)]
		public String WRITERPROG {get;set;}

		[MaxLength(50)]
		public String NEXTPROCESSSTATE {get;set;}

		[MaxLength(50)]
		public String NGAUTHORIZER {get;set;}

		/// <summary>
		/// 创建时间
		/// </summary>
		[MaxLength(7)]
		public DateTime? CREATE_TIME {get;set;}


	}
}
