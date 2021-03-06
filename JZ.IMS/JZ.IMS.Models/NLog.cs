/**
*┌──────────────────────────────────────────────────────────────┐
*│　描    述：                                                    
*│　作    者：Admin                                              
*│　版    本：1.0   模板代码自动生成                                              
*│　创建时间：2019-03-07 16:50:56                            
*└──────────────────────────────────────────────────────────────┘
*┌──────────────────────────────────────────────────────────────┐
*│　命名空间: JZ.IMS.Models                                  
*│　类    名：NLog                                     
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
	/// 
	/// </summary>
	public partial class NLog
	{
		/// <summary>
		///  
		/// </summary>
		[Key]
		public Int32 Id {get;set;}

		/// <summary>
		///  
		/// </summary>
		[MaxLength(50)]
		public String Application {get;set;}

		/// <summary>
		///  
		/// </summary>
		[MaxLength(23)]
		public DateTime? Logged {get;set;}

		/// <summary>
		///  
		/// </summary>
		[MaxLength(50)]
		public String Level {get;set;}

		/// <summary>
		///  
		/// </summary>
		[MaxLength(512)]
		public String Message {get;set;}

		/// <summary>
		///  
		/// </summary>
		[MaxLength(250)]
		public String Logger {get;set;}

		/// <summary>
		///  
		/// </summary>
		[MaxLength(512)]
		public String Callsite {get;set;}

		/// <summary>
		///  
		/// </summary>
		[MaxLength(512)]
		public String Exception {get;set;}


	}
}
