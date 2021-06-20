/*
*┌──────────────────────────────────────────────────────────────┐
*│　描    述：产前确认详细表 更新或者新增实体                                                    
*│　作    者：嘉志科技                                              
*│　版    本：2.0   模板代码自动生成                                              
*│　创建时间：2020-04-25 10:28:35                            
*└──────────────────────────────────────────────────────────────┘
*┌──────────────────────────────────────────────────────────────┐
*│　命名空间: JZ.IMS.ViewModels                                  
*│　类    名：MesProductionPreDtl                                     
*└──────────────────────────────────────────────────────────────┘
*/
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace JZ.IMS.ViewModels
{
	/// <summary>
	/// 嘉志科技
	/// 2020-04-25 10:28:35
	/// 产前确认详细表 更新或者新增实体
	/// </summary>
	public class MesProductionPreDtlAddOrModifyModel
	{
		/// <summary>
		/// 唯一标识
		/// </summary>
		public Decimal ID {get;set;}

		/// <summary>
		/// 配置表ID
		/// </summary>
		public Decimal CONF_ID {get;set;}

		/// <summary>
		/// 主表ID
		/// </summary>
		public Decimal MST_ID {get;set;}

		///// <summary>
		///// 创建人
		///// </summary>
		//public String CREATOR {get;set;}

		///// <summary>
		///// 创建时间
		///// </summary>
		//public DateTime CREATIME {get;set;}

		/// <summary>
		/// 判断结果 Y：正确，N：错误
		/// </summary>
		public String RESULT {get;set;}

		/// <summary>
		/// 判断描述
		/// </summary>
		public String DESCRIPTION {get;set;}


	}
}
