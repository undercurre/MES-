using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace JZ.IMS.WebApi.Public
{
	/// <summary>
	/// Api 返回基类
	/// </summary>
	public class ApiBaseReturn<T>
	{
		/// <summary>
		/// 错误信息
		/// </summary>
		public ApiErrorClass ErrorInfo { get; set; } = new ApiErrorClass();

		/// <summary>
		/// 返回结果
		/// </summary>
		public T Result { get; set; }

		/// <summary>
		/// 总记录数
		/// </summary>
		public int TotalCount { get; set; }

		/// <summary>
		/// 返回信息
		/// </summary>
		public string ResultInfo { get; set; }
	}
}
