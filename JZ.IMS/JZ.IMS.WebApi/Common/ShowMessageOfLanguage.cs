using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace JZ.IMS.WebApi.Common
{
    /// <summary>
    /// 本地化显示提示信息
    /// </summary>
    public class MessageOfLanguage
    {
        /// <summary>
        /// 中文提示字典
        /// </summary>
        private static Dictionary<string, string> dic_MessageOfzh = new Dictionary<string, string>();

        /// <summary>
        /// 英文提示字典
        /// </summary>
        private static Dictionary<string, string> dic_MessageOfen = new Dictionary<string, string>();

        public MessageOfLanguage()
        {
            SetMessageOfzh();
            SetMessageOfen();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="acceptLanguage">语言</param>
        /// <param name="resultCode">提示信息编号</param>
        /// <returns></returns>
        public static string GetMessageOfLanguage(string acceptLanguage, int resultCode)
        {
            string result = string.Empty;
            if (dic_MessageOfen.Count == 0)
            {
                SetMessageOfzh();
                SetMessageOfen();
            }
            if (Enum.IsDefined(typeof(MessageCode), resultCode))
            {
                string keyName = Enum.GetName(typeof(MessageCode), resultCode);
                switch (acceptLanguage)
                {
                    case "en-US,en;":
                        {
                            result = dic_MessageOfen[keyName];
                            break;
                        }
                    case "zh-CN,zh;":
                        {
                            result = dic_MessageOfzh[keyName];
                            break;
                        }
                    default:
                        {
                            result = dic_MessageOfzh[keyName];
                            break;
                        }
                }
            }
            return result;
        }

        /// <summary>
        /// 中文提示
        /// </summary>
        private static void SetMessageOfzh()
        {
            if (dic_MessageOfzh.Count > 0) return;

            if (!dic_MessageOfzh.ContainsKey("CommonObjectSuccessCode")) dic_MessageOfzh.Add("CommonObjectSuccessCode", "操作成功。");

            if (!dic_MessageOfzh.ContainsKey("CommonModelStateInvalidCode")) dic_MessageOfzh.Add("CommonModelStateInvalidCode", "请求数据校验失败。");

            if (!dic_MessageOfzh.ContainsKey("CommonFailNoDataCode")) dic_MessageOfzh.Add("CommonFailNoDataCode", "数据不存在。");

            if (!dic_MessageOfzh.ContainsKey("CommonDataStatusChangeCode")) dic_MessageOfzh.Add("CommonDataStatusChangeCode", "数据状态已发生变化，请刷新后再进行操作。");

            if (!dic_MessageOfzh.ContainsKey("CommonExceptionCode")) dic_MessageOfzh.Add("CommonExceptionCode", "系统异常。");

            if (!dic_MessageOfzh.ContainsKey("CommonBillNotIsNewCode")) dic_MessageOfzh.Add("CommonBillNotIsNewCode", "单据不是新增状态, 不允许删除。");

            if (!dic_MessageOfzh.ContainsKey("CommonBillisCheckedCode")) dic_MessageOfzh.Add("CommonBillisCheckedCode", "单据不是待审核状态, 不允许审核,修改,删除。");

            if (!dic_MessageOfzh.ContainsKey("CommonBillisNotCheckedCode")) dic_MessageOfzh.Add("CommonBillisNotCheckedCode", "单据不是审核状态, 不允许取消审核。");

            if (!dic_MessageOfzh.ContainsKey("CommonDataStatusIsNoExistOfMES")) dic_MessageOfzh.Add("CommonDataStatusIsNoExistOfMES", "用户账号在MES系统中不存在，请在MES系统创建该用户后重试。");

            if (!dic_MessageOfzh.ContainsKey("SignInErrorTimesOverTimesCode")) dic_MessageOfzh.Add("SignInErrorTimesOverTimesCode", "错误超过3次，请重新打开浏览器后再进行登录。");

            if (!dic_MessageOfzh.ContainsKey("SignInPasswordOrUserNameErrorCode")) dic_MessageOfzh.Add("SignInPasswordOrUserNameErrorCode", "对不起，您输入的用户名或者密码错误。");

            if (!dic_MessageOfzh.ContainsKey("SignInUserLockedCode")) dic_MessageOfzh.Add("SignInUserLockedCode", "对不起，该账号已锁定，请联系管理员。");

            if (!dic_MessageOfzh.ContainsKey("SignInCaptchaCodeErrorCode")) dic_MessageOfzh.Add("SignInCaptchaCodeErrorCode", "验证码输入有误。");

            if (!dic_MessageOfzh.ContainsKey("SignInNoRoleErrorCode")) dic_MessageOfzh.Add("SignInNoRoleErrorCode", "暂未分配角色，不能进行登录，请联系管理员。");

            if (!dic_MessageOfzh.ContainsKey("PasswordOldErrorCode")) dic_MessageOfzh.Add("PasswordOldErrorCode", "旧密码输入错误。");

            if (!dic_MessageOfzh.ContainsKey("CommonRoleIsUsedCode")) dic_MessageOfzh.Add("CommonRoleIsUsedCode", "此角色已被使用,不能删除。");

        }

        /// <summary>
        /// 英文提示
        /// </summary>
        private static void SetMessageOfen()
        {
            if (dic_MessageOfen.Count > 0) return;

            if (!dic_MessageOfzh.ContainsKey("CommonObjectSuccessCode")) dic_MessageOfen.Add("CommonObjectSuccessCode", "operate successfully。");

            if (!dic_MessageOfzh.ContainsKey("CommonModelStateInvalidCode")) dic_MessageOfen.Add("CommonModelStateInvalidCode", "Request data validation failed。");

            if (!dic_MessageOfzh.ContainsKey("CommonFailNoDataCode")) dic_MessageOfen.Add("CommonFailNoDataCode", "Data does not exist。");
            
            if (!dic_MessageOfzh.ContainsKey("CommonDataStatusChangeCode")) dic_MessageOfen.Add("CommonDataStatusChangeCode", "The data state has changed, please refresh before operation。");

            if (!dic_MessageOfzh.ContainsKey("CommonExceptionCode")) dic_MessageOfen.Add("CommonExceptionCode", "system exception。");

            if (!dic_MessageOfzh.ContainsKey("CommonBillNotIsNewCode")) dic_MessageOfen.Add("CommonBillNotIsNewCode", "Documents are not new and cannot be deleted。");

            if (!dic_MessageOfzh.ContainsKey("CommonBillisCheckedCode")) dic_MessageOfen.Add("CommonBillisCheckedCode", "Documents are not in the state to be examined, and are not allowed to be examined, modified or deleted。");

            if (!dic_MessageOfzh.ContainsKey("CommonBillisNotCheckedCode")) dic_MessageOfen.Add("CommonBillisNotCheckedCode", "Documents are not in the state of audit, and the audit is not allowed to be cancelled。");

            if (!dic_MessageOfzh.ContainsKey("CommonDataStatusIsNoExistOfMES")) dic_MessageOfen.Add("CommonDataStatusIsNoExistOfMES", "The user account does not exist in MES system, Please try again after the user is created in MES system。");

            if (!dic_MessageOfzh.ContainsKey("SignInErrorTimesOverTimesCode")) dic_MessageOfen.Add("SignInErrorTimesOverTimesCode", "Error more than 3 times, please re-open the browser before login。");

            if (!dic_MessageOfzh.ContainsKey("SignInPasswordOrUserNameErrorCode")) dic_MessageOfen.Add("SignInPasswordOrUserNameErrorCode", "Sorry, the user name or password you entered is wrong。");

            if (!dic_MessageOfzh.ContainsKey("SignInUserLockedCode")) dic_MessageOfen.Add("SignInUserLockedCode", "Sorry, the account is locked, please contact the administrator。");

            if (!dic_MessageOfzh.ContainsKey("SignInCaptchaCodeErrorCode")) dic_MessageOfen.Add("SignInCaptchaCodeErrorCode", "Verification code input error。");

            if (!dic_MessageOfzh.ContainsKey("SignInNoRoleErrorCode")) dic_MessageOfen.Add("SignInNoRoleErrorCode", "No roles assigned, unable to log in, please contact the administrator。");

            if (!dic_MessageOfzh.ContainsKey("PasswordOldErrorCode")) dic_MessageOfen.Add("PasswordOldErrorCode", "The old password was entered incorrectly。");

            if (!dic_MessageOfzh.ContainsKey("CommonRoleIsUsedCode")) dic_MessageOfzh.Add("CommonRoleIsUsedCode", "This role is already in use and cannot be deleted.");
        }
    }
}
