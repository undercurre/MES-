/**
*┌──────────────────────────────────────────────────────────────┐
*│　描    述： 列表显示实体                                                   
*│　作    者：嘉志科技                                              
*│　版    本：2.0   模板代码自动生成                                              
*│　创建时间：2020-04-02 10:58:33                            
*└──────────────────────────────────────────────────────────────┘
*┌──────────────────────────────────────────────────────────────┐
*│　命名空间: JZ.IMS.ViewModels                                  
*│　类    名：SfcsProductCarton                                     
*└──────────────────────────────────────────────────────────────┘
*/
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace JZ.IMS.ViewModels
{
	/// <summary>
	/// 嘉志科技
	/// 2020-04-02 10:58:33
	///  列表显示实体
	/// </summary>
	public class SfcsProductCartonListModel
	{
		/// <summary>
		/// 主键ID
		/// </summary>
		public Decimal ID {get;set;}

		/// <summary>
		/// 料号
		/// </summary>
		public String PART_NO {get;set;}

		/// <summary>
		/// 格式限定
		/// </summary>
		public String FORMAT {get;set;}

		/// <summary>
		/// 标准容量
		/// </summary>
		public Decimal STANDARD_QUANTITY {get;set;}

		/// <summary>
		/// 最大容量
		/// </summary>
		public Decimal? MAX_QUANTITY {get;set;}

		/// <summary>
		/// 最小容量
		/// </summary>
		public Decimal? MIN_QUANTITY {get;set;}

		/// <summary>
		/// 标准重量
		/// </summary>
		public String STANDARD_WEIGHT {get;set;}

		/// <summary>
		/// 最大重量
		/// </summary>
		public String MAX_WEIGHT {get;set;}

		/// <summary>
		/// 最小重量
		/// </summary>
		public String MIN_WEIGHT {get;set;}

		/// <summary>
		/// 标准体积
		/// </summary>
		public String CUBAGE {get;set;}

		/// <summary>
		/// 卡通长
		/// </summary>
		public String LENGTH {get;set;}

		/// <summary>
		/// 卡通宽
		/// </summary>
		public String WIDTH {get;set;}

		/// <summary>
		/// 卡通高
		/// </summary>
		public String HEIGHT {get;set;}

		/// <summary>
		/// 是否系统自动产生
		/// </summary>
		public String AUTO_CREATE_FLAG {get;set;}

		/// <summary>
		/// 采集工序
		/// </summary>
		public Decimal? COLLECT_OPERATION_ID {get;set;}

		/// <summary>
		/// 是否激活
		/// </summary>
		public String ENABLED {get;set;}


	}
}
