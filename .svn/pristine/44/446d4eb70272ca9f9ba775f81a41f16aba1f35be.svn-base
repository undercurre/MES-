/*
*┌──────────────────────────────────────────────────────────────┐
*│　描    述：特殊SN表 查询实体
*│　作    者：嘉志科技
*│　版    本：2.0   模板代码自动生成                                              
*│　创建时间：2020-10-24 14:15:40                            
*└──────────────────────────────────────────────────────────────┘
*┌──────────────────────────────────────────────────────────────┐
*│　命名空间: JZ.IMS.ViewModels                                  
*│　类    名：ImportRuncardSn                                     
*└──────────────────────────────────────────────────────────────┘
*/
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace JZ.IMS.ViewModels
{
    /// <summary>
    /// 嘉志科技
    /// 2020-10-24 14:15:40
    /// 特殊SN表 查询实体
    /// </summary>
    public class ImportRuncardSnRequestModel : PageModel
    {
        /// <summary>
        /// 主表ID（必传）
        /// </summary>
        public int HEADER_ID { get; set; } = 0;

        ///// <summary>
        ///// 工单号
        ///// </summary>
        //public String WO_NO { get; set; }

        /// <summary>
        /// 用户
        /// </summary>
        public String USER_NAME { get; set; }

        /// <summary>
        /// SN
        /// </summary>
        public String SN { get; set; }

        /// <summary>
        /// 开始时间 格式：2020-12-14 00:00:00
        /// </summary>
        public DateTime? BEGIN_TIME { get; set; } = null;

        /// <summary>
        /// 结束时间 格式：2020-12-14 23:59:59
        /// </summary>
        public DateTime? END_TIME { get; set; } = null;

    }

    /// <summary>
    /// 
    /// </summary>
    public class ImportRuncardHeaderRequestModel : PageModel
    {
        /// <summary>
        /// 工单号
        /// </summary>
        public String WO_NO { get; set; }

        /// <summary>
        /// 用户
        /// </summary>
        public String USER_NAME { get; set; }

        /// <summary>
        /// 开始时间 格式：2020-12-14 00:00:00
        /// </summary>
        public DateTime? BEGIN_TIME { get; set; } = null;

        /// <summary>
        /// 结束时间 格式：2020-12-14 23:59:59
        /// </summary>
        public DateTime? END_TIME { get; set; } = null;
    }
}
