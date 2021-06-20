/*
*┌──────────────────────────────────────────────────────────────┐
*│　描    述：SPI测试主表数据                                                    
*│　作    者：嘉志科技                                              
*│　版    本：2.0   模板代码自动生成                                              
*│　创建时间：2020-08-10 15:34:32                            
*└──────────────────────────────────────────────────────────────┘
*┌──────────────────────────────────────────────────────────────┐
*│　命名空间: JZ.IMS.Models                                  
*│　类    名：SmtSpiSrcGen                                     
*└──────────────────────────────────────────────────────────────┘
*/
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace JZ.IMS.Models
{
	/// <summary>
	/// 嘉志科技
	/// 2020-08-10 15:34:32
	/// SPI测试主表数据
	/// </summary>
	[Table("SMT_SPI_SRC_GEN")]
	public partial class SmtSpiSrcGen
	{
		/// <summary>
		/// 唯一标识
		/// </summary>
		[Key]
		public String ID {get;set;}

		/// <summary>
		/// 线别编号
		/// </summary>
		[MaxLength(50)]
		public String LINE_NO {get;set;}

		/// <summary>
		/// 产品工单号
		/// </summary>
		[MaxLength(100)]
		public String WO {get;set;}

		/// <summary>
		/// 机种料号规格
		/// </summary>
		[MaxLength(50)]
		public String MO {get;set;}

		/// <summary>
		/// 产品料号
		/// </summary>
		[MaxLength(50)]
		public String PART_NO {get;set;}

		/// <summary>
		/// 大板条码
		/// </summary>
		[MaxLength(50)]
		public String P_SN {get;set;}

		/// <summary>
		/// 子产品条码
		/// </summary>
		[MaxLength(50)]
		public String SN {get;set;}

		/// <summary>
		/// 子板拼板好
		/// </summary>
		[MaxLength(10)]
		public String BOARD_SEQ {get;set;}

		/// <summary>
		/// 测试结果
		/// </summary>
		[MaxLength(10)]
		public String TEST_RESULT {get;set;}

		/// <summary>
		/// 人工判断结果
		/// </summary>
		[MaxLength(10)]
		public String REVISE_RESULT {get;set;}

		/// <summary>
		/// 程序名称
		/// </summary>
		[MaxLength(100)]
		public String PROGRAM_NAME {get;set;}

		/// <summary>
		/// 测试时间
		/// </summary>
		[MaxLength(7)]
		public DateTime? TEST_TIME {get;set;}

		/// <summary>
		/// 创建时间
		/// </summary>
		[MaxLength(7)]
		public DateTime? CREATE_TIME {get;set;}

		/// <summary>
		/// PCB板测试时间
		/// </summary>
		[MaxLength(22)]
		public Decimal? DURATION {get;set;}

		/// <summary>
		/// 焊盘数量
		/// </summary>
		[MaxLength(22)]
		public Decimal? PASTENUM {get;set;}

		/// <summary>
		/// 屏蔽的焊盘数量
		/// </summary>
		[MaxLength(22)]
		public Decimal? INVALIDNUM {get;set;}

		/// <summary>
		/// NG焊盘数量
		/// </summary>
		[MaxLength(22)]
		public Decimal? NGPADCNT {get;set;}

		/// <summary>
		/// 2D/3D数据保存目录
		/// </summary>
		[MaxLength(100)]
		public String RESULTPATH {get;set;}

		/// <summary>
		/// 拼板数量
		/// </summary>
		[MaxLength(22)]
		public Decimal? RESERVED1BYSHORT {get;set;}

		/// <summary>
		/// NG拼板数量
		/// </summary>
		[MaxLength(22)]
		public Decimal? RESERVED2BYSHORT {get;set;}

		/// <summary>
		/// PASS拼板数量
		/// </summary>
		[MaxLength(22)]
		public Decimal? RESERVED1BYINT {get;set;}

		/// <summary>
		/// 保存该记录的操作者
		/// </summary>
		[MaxLength(50)]
		public String RESERVED1BYCHAR {get;set;}

		/// <summary>
		/// 体积百分比平均值
		/// </summary>
		[MaxLength(22)]
		public Decimal? AVGVPER {get;set;}

		/// <summary>
		/// 面积百分比平均值
		/// </summary>
		[MaxLength(22)]
		public Decimal? AVGAPER {get;set;}

		/// <summary>
		/// 高度平均值
		/// </summary>
		[MaxLength(22)]
		public Decimal? AVGHEIGHT {get;set;}

		/// <summary>
		/// X偏移平均值
		/// </summary>
		[MaxLength(22)]
		public Decimal? AVGSHIFTX {get;set;}

		/// <summary>
		/// Y偏移平均值
		/// </summary>
		[MaxLength(22)]
		public Decimal? AVGSHIFTY {get;set;}


	}
}
