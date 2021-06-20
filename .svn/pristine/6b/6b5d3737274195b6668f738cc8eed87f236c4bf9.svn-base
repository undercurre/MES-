/*
*┌──────────────────────────────────────────────────────────────┐
*│　描    述： 查询实体
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
	///  查询实体
	/// </summary>
	public class SfcsRuncardRangerRequestModel : PageModel
	{
		/// <summary>
		/// 工单号
		/// </summary>
		public string WO_NO { get; set; }

		/// <summary>
		/// 开始流水号
		/// </summary>
		public string SN_BEGIN { get; set; }

        /// <summary>
        /// 结束流水号
        /// </summary>
        public string SN_END { get; set; }

        /// <summary>
        /// 成品料号
        /// </summary>
        public string PART_NO { get; set; }

        /// <summary>
        /// 固定头
        /// </summary>
        public string FIX_HEADER { get; set; }

        /// <summary>
        /// 固定尾
        /// </summary>
        public string FIX_TAIL { get; set; }
    }

    /// <summary>
    /// 嘉志科技
    /// 2020-11-25 14:00:00
    ///  查询实体
    /// </summary>
    public class PrintPuzzleCodeRequestModel
    {
        /// <summary>
        /// 流水号范围id
        /// </summary>
        public Decimal Id { get; set; }

        /// <summary>
        /// 拼版数
        /// </summary>
        public Decimal BoardQty { get; set; }

        /// <summary>
        /// 操作用户
        /// </summary>
        public string UserName { get; set; }
    }
    
    public class PrintReturnModel
    {
        /// <summary>
        /// 任务列
        /// </summary>
        public List<decimal> TaskIdList { get; set; }
        /// <summary>
        /// 消息列
        /// </summary>
        public List<String> MsgList { get; set; }
    }

    /// <summary>
    /// 导入SN请求实体
    /// </summary>
    public class SaveExcelDataRequestModel
    {
        /// <summary>
        /// 操作用户
        /// </summary>
        public String USER_NAME { get; set; } = "";

        /// <summary>
        /// 流水号校验配置值
        /// </summary>
        public String SN_CONFIG_VALUE { get; set; } = "";

        /// <summary>
        /// 流水号2校验配置值
        /// </summary>
        public String SN2_CONFIG_VALUE { get; set; } = "";

        /// <summary>
        /// IMEI1校验配置值
        /// </summary>
        public String IMEI1_CONFIG_VALUE { get; set; } = "";

        /// <summary>
        /// IMEI2校验配置值
        /// </summary>
        public String IMEI2_CONFIG_VALUE { get; set; } = "";

        /// <summary>
        /// MEID校验配置值
        /// </summary>
        public String MEID_CONFIG_VALUE { get; set; } = "";

        /// <summary>
        /// MAC校验配置值
        /// </summary>
        public String MAC_CONFIG_VALUE { get; set; } = "";

        /// <summary>
        /// BT校验配置值
        /// </summary>
        public String BT_CONFIG_VALUE { get; set; } = "";

    }

}
