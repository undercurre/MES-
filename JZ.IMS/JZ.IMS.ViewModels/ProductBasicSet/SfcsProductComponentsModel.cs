/**
*┌──────────────────────────────────────────────────────────────┐
*│　描    述： 更新或者新增实体                                                    
*│　作    者：嘉志科技                                              
*│　版    本：2.0   模板代码自动生成                                              
*│　创建时间：2020-04-03 15:59:15                            
*└──────────────────────────────────────────────────────────────┘
*┌──────────────────────────────────────────────────────────────┐
*│　命名空间: JZ.IMS.ViewModels                                  
*│　类    名：SfcsProductComponents                                     
*└──────────────────────────────────────────────────────────────┘
*/
using System;
using System.Collections.Generic;
using System.Text;

namespace JZ.IMS.ViewModels
{
	/// <summary>
	/// 嘉志科技
	/// 2020-04-03 15:59:15
	///  批量保存实体
	/// </summary>
	public class SfcsProductComponentsModel
	{
		#region 零件组件
		/// <summary>
		/// 新增的记录
		/// </summary>
		public List<SfcsProductComponentsAddOrModifyModel> InsertComponents { get; set; }

		/// <summary>
		/// 更新的记录
		/// </summary>
		public List<SfcsProductComponentsAddOrModifyModel> UpdateComponents { get; set; }

        /// <summary>
        /// 删除的记录
        /// </summary>
        public List<SfcsProductComponentsAddOrModifyModel> RemoveRecords { get; set; }

        #endregion

        #region 附件
        /// <summary>
        /// 新增的记录
        /// </summary>
        public List<SfcsProductAttachmentsAddOrModifyModel> InsertAttachments { get; set; }

		/// <summary>
		/// 更新的记录
		/// </summary>
		public List<SfcsProductAttachmentsAddOrModifyModel> UpdateAttachments { get; set; }

		#endregion

		#region 替代料维护
		/// <summary>
		/// 新增的记录
		/// </summary>
		public List<SfcsSubstituteComponentsAddOrModifyModel> InsertSubstitute { get; set; }

		/// <summary>
		/// 更新的记录
		/// </summary>
		public List<SfcsSubstituteComponentsAddOrModifyModel> UpdateSubstitute { get; set; }

		#endregion
	}
}
