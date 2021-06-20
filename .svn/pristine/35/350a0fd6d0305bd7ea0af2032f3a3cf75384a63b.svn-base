using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace JZ.IMS.WebApi.Common
{
	/// <summary>
	/// 提示信息枚举
	/// </summary>
	public enum MessageCode
	{
		#region 通用100+

		/// <summary>
		/// 通用成功编码 : "操作成功"
		/// </summary>
		CommonObjectSuccessCode = 0,

		/// <summary>
		/// 通用Form验证失败错误码: "请求数据校验失败"
		/// </summary>
		CommonModelStateInvalidCode = 101,

		/// <summary>
		/// 数据为空的编码: "数据不存在" 
		/// </summary>
		CommonFailNoDataCode = 102,

		/// <summary>
		/// 数据状态发生变化的编码: "数据状态已发生变化，请刷新后再进行操作"
		/// </summary>
		CommonDataStatusChangeCode = 103,

		/// <summary>
		/// 用户账号在MES系统中不存在，请在MES系统创建该用户后重试！
		/// </summary>
		CommonDataStatusIsNoExistOfMES = 104,

		/// <summary>
		/// 通用失败，系统异常错误码: "系统异常";
		/// </summary>
		CommonExceptionCode = 106,

		/// <summary>
		///单据不是新增状态的错误码: "单据不是新增状态, 不允许删除."
		/// </summary>
		CommonBillNotIsNewCode = 113,

		/// <summary>
		///单据不是待审核状态的错误码: "单据不是待审核状态, 不允许审核,修改,删除."
		/// </summary>
		CommonBillisCheckedCode = 110,

		/// <summary>
		///单据不是审核状态的错误码: "单据不是审核状态, 不允许取消审核."
		/// </summary>
		CommonBillisNotCheckedCode = 111,

		/// <summary>
		///此角色已被使用,不能删除."
		/// </summary>
		CommonRoleIsUsedCode = 112,

		#endregion

		#region 用户登录200+

		/// <summary>
		/// 错误次数超过允许最大失败次数: "错误超过3次，请重新打开浏览器后再进行登录"
		/// </summary>
		SignInErrorTimesOverTimesCode = 201,

		/// <summary>
		/// 用户名或者密码错误: "对不起，您输入的用户名或者密码错误"
		/// </summary>
		SignInPasswordOrUserNameErrorCode = 202,

		/// <summary>
		/// 用户被锁定: "对不起，该账号已锁定，请联系管理员"
		/// </summary>
		SignInUserLockedCode = 203,

		/// <summary>
		/// 验证码错误: "验证码输入有误"
		/// </summary>
		SignInCaptchaCodeErrorCode = 204,

		/// <summary>
		/// 未分配角色: "暂未分配角色，不能进行登录，请联系管理员"
		/// </summary>
		SignInNoRoleErrorCode = 205,

		/// <summary>
		/// 旧密码输入错误: "旧密码输入错误"
		/// </summary>
		PasswordOldErrorCode = 206

		#endregion
	}
}
