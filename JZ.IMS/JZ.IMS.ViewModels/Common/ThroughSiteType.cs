using System;
using System.Collections.Generic;
using System.Text;

namespace JZ.IMS.ViewModels
{
    public enum ThroughSiteType
    {
        Pass,
        Fail,
        RePass,
        ReFail
    }

    /// <summary>
    /// 输入输出類型枚舉
    /// </summary>
    public enum IOType
    {
        Input,
        Output
    }

    /// <summary>
    /// 標準對象的狀態類型枚舉
    /// </summary>
    public enum StandardObjectStatusType
    {
        Completed,
        Incompleted
    }
}
