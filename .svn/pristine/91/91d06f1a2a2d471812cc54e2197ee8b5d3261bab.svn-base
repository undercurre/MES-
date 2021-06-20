/*
*┌──────────────────────────────────────────────────────────────┐
*│　描    述： 查询实体
*│　作    者：嘉志科技
*│　版    本：2.0   模板代码自动生成                                              
*│　创建时间：2020-04-14 16:44:04                            
*└──────────────────────────────────────────────────────────────┘
*┌──────────────────────────────────────────────────────────────┐
*│　命名空间: JZ.IMS.ViewModels                                  
*│　类    名：SfcsWo                                     
*└──────────────────────────────────────────────────────────────┘
*/
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace JZ.IMS.ViewModels
{
    /// <summary>
    /// 嘉志科技
    /// 2020-04-14 16:44:04
    ///  查询实体
    /// </summary>
    public class SfcsWoRequestModel : PageModel
    {
        /// <summary>
        /// 工单号
        /// </summary>
        public String WO_NO { get; set; }

        /// <summary>
        /// 料号
        /// </summary>
        public String PART_NO { get; set; }
        /// <summary>
        /// 机种
        /// </summary>
        public Decimal MODEL_ID { get; set; }
        /// <summary>
        /// 制造群
        /// </summary>
        public Decimal BU_CODE { get; set; }
        /// <summary>
        /// 计划投产日期
        /// </summary>
        public string START_DATE { get; set; }

        /// <summary>
        /// 计划完成日期
        /// </summary>
        public string DUE_DATE { get; set; }
        public SfcsWoRequestModel()
        {
            Page = 1;
            Limit = 50;
        }
    }
}
