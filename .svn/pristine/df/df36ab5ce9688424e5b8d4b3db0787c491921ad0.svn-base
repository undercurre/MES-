/**
*┌──────────────────────────────────────────────────────────────┐
*│　描    述：后台管理员                                                    
*│　作    者：Admin                                              
*│　版    本：1.0   模板代码自动生成                                              
*│　创建时间：2019-03-07 16:50:56                            
*└──────────────────────────────────────────────────────────────┘
*┌──────────────────────────────────────────────────────────────┐
*│　命名空间: JZ.IMS.Models                                  
*│　类    名：Manager                                     
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
	/// 后台管理员
	/// </summary>
	public partial class Sys_Manager
	{
		/// <summary>
		/// 主键
		/// </summary>
		[Key]
		public decimal ID { get; set; }

		/// <summary>
		/// 角色ID
		/// </summary>
		[Required]
		[MaxLength(10)]
		public decimal ROLE_ID { get; set; }

		/// <summary>
		/// 用户名
		/// </summary>
		[Required]
		[MaxLength(50)]
		public String USER_NAME { get; set; }

		/// <summary>
		/// 姓名
		/// </summary>
		[Required]
		[MaxLength(50)]
		public String FULL_NAME { get; set; }

		/// <summary>
		/// 密码盐
		/// </summary>
		[Required]
		[MaxLength(50)]
		public String PASSWORD_SALT { get; set; }

		/// <summary>
		/// 密码
		/// </summary>
		[Required]
		[MaxLength(50)]
		public String PASSWORD { get; set; }

		/// <summary>
		/// 头像
		/// </summary>
		[MaxLength(250)]
		public String AVATAR { get; set; }

		/// <summary>
		/// 用户昵称
		/// </summary>
		[MaxLength(50)]
		public String NICK_NAME { get; set; }

		/// <summary>
		/// 手机号码
		/// </summary>
		[MaxLength(50)]
		public String MOBILE { get; set; }

		/// <summary>
		/// 邮箱地址
		/// </summary>
		[MaxLength(250)]
		public String EMAIL { get; set; }

		/// <summary>
		/// 企业微信ID
		/// </summary>
		[MaxLength(250)]
		public String WORK_WECHAT_ID { get; set; }

		/// <summary>
		/// 外部系统ID
		/// </summary>
		[MaxLength(50)]
		public String ATTRIBUTE1 { get; set; }

		/// <summary>
		/// 登录次数
		/// </summary>
		[MaxLength(10)]
		public decimal? LOGIN_COUNT { get; set; }

		/// <summary>
		/// 最后一次登录IP
		/// </summary>
		[MaxLength(64)]
		public String LOGIN_LAST_IP { get; set; }

		/// <summary>
		/// 最后一次登录时间
		/// </summary>
		[MaxLength(23)]
		public DateTime? LOGIN_LAST_TIME { get; set; }

		/// <summary>
		/// 添加人
		/// </summary>
		[Required]
		[MaxLength(10)]
		public decimal ADD_MANAGER_ID { get; set; }

		/// <summary>
		/// 添加时间
		/// </summary>
		[Required]
		[MaxLength(23)]
		public DateTime Add_Time { get; set; }

		/// <summary>
		/// 修改人
		/// </summary>
		[MaxLength(10)]
		public decimal? MODIFY_MANAGER_ID { get; set; }

		/// <summary>
		/// 修改时间
		/// </summary>
		[MaxLength(23)]
		public DateTime? MODIFY_TIME { get; set; }

		/// <summary>
		/// 是否锁定
		/// </summary>
		[Required]
		[MaxLength(1)]
		public string ENABLED { get; set; }

		/// <summary>
		/// 是否删除
		/// </summary>
		[Required]
		[MaxLength(1)]
		public String IS_DELETE { get; set; }

		/// <summary>
		/// 备注
		/// </summary>
		[MaxLength(500)]
		public String REMARK { get; set; }

		/// <summary>
		/// 组织架构
		/// </summary>
		[MaxLength(50)]
		public String ORGANIZE_ID { get; set; }
	}
}
