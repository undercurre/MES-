/*
*┌──────────────────────────────────────────────────────────────┐
*│　描    述：入库记录信息表 更新或者新增实体                                                    
*│　作    者：嘉志科技                                              
*│　版    本：2.0   模板代码自动生成                                              
*│　创建时间：2021-04-27 17:00:28                            
*└──────────────────────────────────────────────────────────────┘
*┌──────────────────────────────────────────────────────────────┐
*│　命名空间: JZ.IMS.ViewModels                                  
*│　类    名：SfcsInboundRecordInfo                                     
*└──────────────────────────────────────────────────────────────┘
*/
using System;
using System.Collections.Generic;
using System.Text;

namespace JZ.IMS.ViewModels
{
	/// <summary>
	/// 嘉志科技
	/// 2021-04-27 17:00:28
	/// 入库记录信息表 批量保存实体
	/// </summary>
	public class SfcsInboundRecordInfoModel
	{
		/// <summary>
		/// 新增的记录
		/// </summary>
		public List<SfcsInboundRecordInfoAddOrModifyModel> InsertRecords { get; set; }

		/// <summary>
		/// 更新的记录
		/// </summary>
		public List<SfcsInboundRecordInfoAddOrModifyModel> UpdateRecords { get; set; }

        /// <summary>
        /// 删除的记录
        /// </summary>
        public List<SfcsInboundRecordInfoAddOrModifyModel> RemoveRecords { get; set; }
    }
}
