using System;
using System.Collections.Generic;
using System.Text;

namespace JZ.IMS.ViewModels
{
    /// <summary>
    /// 
    /// </summary>
    public class BoolResult
    {
        /// <summary>
        /// 返回结果
        /// </summary>
        public bool Result { get; set; }

        /// <summary>
        /// 提示信息(不为空时, 要提示给用户看.)
        /// </summary>
        public string PromptMessage { get; set; } = string.Empty;
    }
}
