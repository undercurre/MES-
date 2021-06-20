/*
*┌──────────────────────────────────────────────────────────────┐
*│　描    述：                                                    
*│　作    者：嘉志科技                                              
*│　版    本：2.0   模板代码自动生成                                              
*│　创建时间：2020-06-30 16:18:55                            
*└──────────────────────────────────────────────────────────────┘
*┌──────────────────────────────────────────────────────────────┐
*│　命名空间: JZ.IMS.Models                                  
*│　类    名：MesCpcecnItemline                                     
*└──────────────────────────────────────────────────────────────┘
*/
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace JZ.IMS.Models
{
	/// <summary>
	/// 嘉志科技
	/// 2020-06-30 16:18:55
	/// 
	/// </summary>
	[Table("MES_CPCECN_ITEMLINE")]
	public partial class MesCpcecnItemline
	{
		/// <summary>
		/// 主键
		/// </summary>
		[Key]
		public Decimal ID {get;set;}

		/// <summary>
		/// 物料编码
		/// </summary>
		[MaxLength(50)]
		public String ITEMCODE {get;set;}

		[MaxLength(50)]
		public String BCDRAWID {get;set;}

		/// <summary>
		/// 更改前名称
		/// </summary>
		[MaxLength(50)]
		public String DCNAME {get;set;}

		/// <summary>
		/// 更改前品号属性
		/// </summary>
		[MaxLength(50)]
		public String BCCLAS {get;set;}

		/// <summary>
		/// 更改前规格
		/// </summary>
		[MaxLength(50)]
		public String BCSPEC {get;set;}

		/// <summary>
		/// 更改前材质
		/// </summary>
		[MaxLength(50)]
		public String BCMATERIAL {get;set;}

		/// <summary>
		/// 更改前扩展属性
		/// </summary>
		[MaxLength(150)]
		public String BCEXPANDS {get;set;}

		/// <summary>
		/// 更改前安规认证
		/// </summary>
		[MaxLength(50)]
		public String BCSTRDEF15 {get;set;}

		/// <summary>
		/// 更改前尺寸
		/// </summary>
		[MaxLength(50)]
		public String BCSTRDEF18 {get;set;}

		/// <summary>
		/// 更改前工艺
		/// </summary>
		[MaxLength(50)]
		public String BCSTRDEF19 {get;set;}

		[MaxLength(150)]
		public String ACDRAWID {get;set;}

		/// <summary>
		/// 更改后名称
		/// </summary>
		[MaxLength(50)]
		public String ACNAME {get;set;}

		/// <summary>
		/// 更改后品号属性
		/// </summary>
		[MaxLength(150)]
		public String ACCLAS {get;set;}

		/// <summary>
		/// 更改后规格
		/// </summary>
		[MaxLength(50)]
		public String ACSPEC {get;set;}

		/// <summary>
		/// 更改后材质
		/// </summary>
		[MaxLength(50)]
		public String ACMATERIAL {get;set;}

		/// <summary>
		/// 更改后扩展属性
		/// </summary>
		[MaxLength(150)]
		public String ACEXPANDS {get;set;}

		/// <summary>
		/// 更改后安规认证
		/// </summary>
		[MaxLength(50)]
		public String ACSTRDEF15 {get;set;}

		/// <summary>
		/// 更改后尺寸
		/// </summary>
		[MaxLength(50)]
		public String ACSTRDEF18 {get;set;}

		/// <summary>
		/// 更改后工艺
		/// </summary>
		[MaxLength(50)]
		public String ACSTRDEF19 {get;set;}

		/// <summary>
		/// 主题
		/// </summary>
		[MaxLength(50)]
		public String PROJCODE {get;set;}

		/// <summary>
		/// 状态
		/// </summary>
		[MaxLength(10)]
		public String STAT {get;set;}

		/// <summary>
		/// 更改单编号
		/// </summary>
		[MaxLength(50)]
		public String NTCODE {get;set;}

		/// <summary>
		/// 更改类型
		/// </summary>
		[MaxLength(50)]
		public String CDATA {get;set;}

		/// <summary>
		/// 更改单名称
		/// </summary>
		[MaxLength(50)]
		public String ECNNAME {get;set;}

		[MaxLength(22)]
		public Decimal? ECNID {get;set;}


	}
}
