using System;
using System.Collections.Generic;
using System.Text;

namespace JZ.IMS.ViewModels
{
    /// <summary>
    /// 校验返回类
    /// </summary>
    public class CheckResult
    {
        /// <summary>
        /// 是否出错
        /// </summary>
        public bool Error { get; set; } = false;

        /// <summary>
        /// 错误信息
        /// </summary>
        public string ErrMsg { get; set; } = string.Empty;

        /// <summary>
        /// 
        /// </summary>
        public decimal ID { get; set; } =0;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="message"></param>
        public void Set(string message)
        {
            Error = true;
            ErrMsg = message;
        }
    }
}
