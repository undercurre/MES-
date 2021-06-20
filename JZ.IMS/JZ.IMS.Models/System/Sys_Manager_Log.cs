/**
*┌──────────────────────────────────────────────────────────────┐
*│　描    述：操作日志                                                    
*│　作    者：Admin                                              
*│　版    本：1.0   模板代码自动生成                                              
*│　创建时间：2019-03-07 16:50:56                            
*└──────────────────────────────────────────────────────────────┘
*┌──────────────────────────────────────────────────────────────┐
*│　命名空间: JZ.IMS.Models                                  
*│　类    名：ManagerLog                                     
*└──────────────────────────────────────────────────────────────┘
*/
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace JZ.IMS.Models
{
	/// <summary>
	/// Admin
	/// 2019-03-07 16:50:56
	/// 操作日志
	/// </summary>
	public partial class Sys_Manager_Log
	{
		/// <summary>
		///  
		/// </summary>
		[Key]
		public decimal Id {get;set;}

		/// <summary>
		/// 操作类型
		/// </summary>
		[MaxLength(32)]
		public String Action_Type {get;set;}

		/// <summary>
		/// 主键
		/// </summary>
		[Required]
		[MaxLength(10)]
		public decimal Add_Manager_Id {get;set;}

		/// <summary>
		/// 操作人名称
		/// </summary>
		[MaxLength(50)]
		public String Add_Manager_User_Name {get;set;}

		/// <summary>
		/// 操作时间
		/// </summary>
		[Required]
		[MaxLength(23)]
		public DateTime Add_Time {get;set;}

		/// <summary>
		/// 操作IP
		/// </summary>
		[MaxLength(64)]
		public string Add_Ip {get;set;}

		/// <summary>
		/// 备注
		/// </summary>
		[MaxLength(256)]
		public String Remark {get;set;}


	}
}
