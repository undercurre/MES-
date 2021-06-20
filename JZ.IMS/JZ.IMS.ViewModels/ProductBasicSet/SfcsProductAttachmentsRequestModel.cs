/**
*┌──────────────────────────────────────────────────────────────┐
*│　描    述： 查询实体
*│　作    者：嘉志科技
*│　版    本：2.0   模板代码自动生成                                              
*│　创建时间：2020-04-01 09:20:16                            
*└──────────────────────────────────────────────────────────────┘
*┌──────────────────────────────────────────────────────────────┐
*│　命名空间: JZ.IMS.ViewModels                                  
*│　类    名：SfcsProductAttachments                                     
*└──────────────────────────────────────────────────────────────┘
*/
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace JZ.IMS.ViewModels
{
    /// <summary>
    /// 嘉志科技
    /// 2020-04-01 09:20:16
    ///  查询实体
    /// </summary>
    public class SfcsProductAttachmentsRequestModel : PageModel
    {

        /// <summary>
        /// 产品零件ID(PRODUCT_OBJECT_ID)
        /// </summary>
        public string PRODUCT_OBJECT_ID { get; set; }
        public SfcsProductAttachmentsRequestModel()
        {
            Page = 1;
            Limit = 50;
        }
    }
}
