/**
*┌──────────────────────────────────────────────────────────────┐
*│　描    述： 更新或者新增实体                                                    
*│　作    者：嘉志科技                                              
*│　版    本：2.0   模板代码自动生成                                              
*│　创建时间：2020-03-31 16:22:05                            
*└──────────────────────────────────────────────────────────────┘
*┌──────────────────────────────────────────────────────────────┐
*│　命名空间: JZ.IMS.ViewModels                                  
*│　类    名：SfcsAllObjects                                     
*└──────────────────────────────────────────────────────────────┘
*/
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace JZ.IMS.ViewModels
{
	/// <summary>
	/// 嘉志科技
	/// 2020-03-31 16:22:05
	///  更新或者新增实体
	/// </summary>
	public class SfcsAllObjectsAddOrModifyModel
	{
		/// <summary>
		/// 主键ID
		/// </summary>
		public Decimal ID {get;set;}

		/// <summary>
		/// 采集类型名称
		/// </summary>
		public String OBJECT_NAME {get;set;}

		/// <summary>
		/// 标记信息
		/// </summary>
		public String OBJECT_MARK {get;set;}

		/// <summary>
		/// 采集类型种类
		/// </summary>
		public Decimal? OBJECT_CATEGORY {get;set;}

		/// <summary>
		/// 是否启用
		/// </summary>
		public String ISACTIVE {get;set;}

		/// <summary>
		/// 描述
		/// </summary>
		public String DESCRIPTION {get;set;}

	}
}
