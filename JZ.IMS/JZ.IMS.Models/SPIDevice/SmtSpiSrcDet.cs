/*
*┌──────────────────────────────────────────────────────────────┐
*│　描    述：SPI记录字表数据                                                    
*│　作    者：嘉志科技                                              
*│　版    本：2.0   模板代码自动生成                                              
*│　创建时间：2020-08-10 15:40:02                            
*└──────────────────────────────────────────────────────────────┘
*┌──────────────────────────────────────────────────────────────┐
*│　命名空间: JZ.IMS.Models                                  
*│　类    名：SmtSpiSrcDet                                     
*└──────────────────────────────────────────────────────────────┘
*/
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace JZ.IMS.Models
{
	/// <summary>
	/// 嘉志科技
	/// 2020-08-10 15:40:02
	/// SPI记录字表数据
	/// </summary>
	[Table("SMT_SPI_SRC_DET")]
	public partial class SmtSpiSrcDet
	{
		/// <summary>
		/// 唯一标识符
		/// </summary>
		[Key]
		public String ID {get;set;}

		/// <summary>
		/// 主表关联ID
		/// </summary>
		[MaxLength(50)]
		public String GEN_ID {get;set;}

		[MaxLength(10)]
		public String SEQ {get;set;}

		/// <summary>
		/// 位置
		/// </summary>
		[MaxLength(50)]
		public String LOCATION {get;set;}

		/// <summary>
		/// 焊盘编号
		/// </summary>
		[MaxLength(50)]
		public String ITEM_NO {get;set;}

		[MaxLength(20)]
		public String AREA {get;set;}

		/// <summary>
		/// 高度
		/// </summary>
		[MaxLength(20)]
		public String HEIGHT {get;set;}

		[MaxLength(20)]
		public String VOLUME {get;set;}

		[MaxLength(20)]
		public String AREA_PEC {get;set;}

		[MaxLength(20)]
		public String HEIGHT_PEC {get;set;}

		[MaxLength(20)]
		public String VOLUME_PEC {get;set;}

		/// <summary>
		/// X偏移
		/// </summary>
		[MaxLength(20)]
		public String X_OFFSET {get;set;}

		/// <summary>
		/// Y偏移
		/// </summary>
		[MaxLength(20)]
		public String Y_OFFSET {get;set;}

		[MaxLength(20)]
		public String X_PADSIZE {get;set;}

		[MaxLength(20)]
		public String Y_PADSIZE {get;set;}

		/// <summary>
		/// 检测结果
		/// </summary>
		[MaxLength(10)]
		public String RESULT {get;set;}

		/// <summary>
		/// 不良代码
		/// </summary>
		[MaxLength(50)]
		public String ERRCODE {get;set;}

		[MaxLength(10)]
		public String PIN_NUM {get;set;}


	}
}
