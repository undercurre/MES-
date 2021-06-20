/*
*┌──────────────────────────────────────────────────────────────┐
*│　描    述： 列表显示实体                                                   
*│　作    者：嘉志科技                                              
*│　版    本：2.0   模板代码自动生成                                              
*│　创建时间：2020-04-14 14:53:45                            
*└──────────────────────────────────────────────────────────────┘
*┌──────────────────────────────────────────────────────────────┐
*│　命名空间: JZ.IMS.ViewModels                                  
*│　类    名：SfcsRuncardRanger                                     
*└──────────────────────────────────────────────────────────────┘
*/
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace JZ.IMS.ViewModels
{
    /// <summary>
    /// 嘉志科技
    /// 2020-04-14 14:53:45
    ///  列表显示实体
    /// </summary>
    public class SfcsRuncardRangerListModel
    {
        /// <summary>
        /// 主键ID
        /// </summary>
        public Decimal ID { get; set; }

        /// <summary>
        /// 工单ID
        /// </summary>
        public Decimal WO_ID { get; set; }

        /// <summary>
        /// 工单号
        /// </summary>
        public string WO_NO { get; set; }

        /// <summary>
        /// 开始流水号
        /// </summary>
        public String SN_BEGIN { get; set; }

        /// <summary>
        /// 结束流水号
        /// </summary>
        public String SN_END { get; set; }

        /// <summary>
        /// 数量
        /// </summary>
        public Decimal QUANTITY { get; set; }

        /// <summary>
        /// 进制
        /// </summary>
        public Decimal DIGITAL { get; set; }

        /// <summary>
        /// 变化位数
        /// </summary>
        public Decimal? RANGE { get; set; }

        /// <summary>
        /// 固定头码
        /// </summary>
        public String FIX_HEADER { get; set; }

        /// <summary>
        /// 固定尾码
        /// </summary>
        public String FIX_TAIL { get; set; }

        /// <summary>
        /// 固定头位数
        /// </summary>
        public Decimal? HEADER_LENGTH { get; set; }

        /// <summary>
        /// 固定尾位数
        /// </summary>
        public Decimal? TAIL_LENGTH { get; set; }

        /// <summary>
        /// 是否已打印
        /// </summary>
        public String PRINTED { get; set; }

        /// <summary>
        /// 排除字符
        /// </summary>
        public String EXCLUSIVE_CHAR { get; set; }

        /// <summary>
        /// 状态(注: 不等于1时, 不允许编辑)
        /// </summary>
        public Decimal? STATUS { get; set; }

        /// <summary>
        /// 流水号范围规则ID
        /// </summary>
        public Decimal? RANGER_RULE_ID { get; set; }


    }

    /// <summary>
    /// 工单投入查询报表返回实体
    /// </summary>
    public class InputSNListModel
    {
        /// <summary>
        /// 工单号
        /// </summary>
        public String WO_NO { get; set; }

        /// <summary>
        /// 已投入
        /// </summary>
        public List<String> IsSNList { get; set; } = new List<String>();

        /// <summary>
        ///  未投入
        /// </summary>
        public List<String> IsNotSNList { get; set; } = new List<String>();

    }

}
