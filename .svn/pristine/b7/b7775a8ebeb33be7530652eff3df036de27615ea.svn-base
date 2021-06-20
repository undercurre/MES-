using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace JZ.IMS.WebApi.Public
{
	/// <summary>
	/// API 错误类
	/// </summary>
	public class ApiErrorClass
	{
		#region 公开属性

		/// <summary>
		/// 状态(是否出现错误)
		/// </summary>
		public bool Status { get; private set; } = false;

		/// <summary>
		/// 信息
		/// </summary>
		public string Message { get; private set; } = string.Empty;

		#endregion

		#region 公开方法

		/// <summary>
		/// 清空
		/// </summary>
		public virtual void Clear()
		{
			Status = false;
			Message = string.Empty;
		}

		/// <summary>
		/// 设置
		/// </summary>
		/// <param name="message">内容</param>
		public virtual void Set(string message)
		{
			Status = true;
			Message = message;
		}

		/// <summary>
		/// 设置
		/// </summary>
		/// <param name="errorInfo">错误信息对象</param>
		public virtual void Set(ErrorInfoClass errorInfo)
		{
			Set(errorInfo.Message);
		}

		#endregion
	}
}
