/*
*┌──────────────────────────────────────────────────────────────┐
*│　描    述：产前确认配置表                                                    
*│　作    者：嘉志科技                                              
*│　版    本：2.0   模板代码自动生成                                              
*│　创建时间：2020-04-24 17:23:47                           
*└──────────────────────────────────────────────────────────────┘
*┌──────────────────────────────────────────────────────────────┐
*│　命名空间： JZ.IMS.IRepository                                   
*│　接口名称： IMesProductionPreConfRepository                                      
*└──────────────────────────────────────────────────────────────┘
*/
using JZ.IMS.Core.Repository;
using JZ.IMS.Models;
using JZ.IMS.ViewModels;
using System;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace JZ.IMS.IRepository
{
    public interface IMesProductionLinePreparationRepository : IBaseRepository<MesProductionPreConf, Decimal>
    {

        /// <summary>
        /// 获取当前时间
        /// </summary>
        /// <returns></returns>
        Task<String> GetCurrentTime();

        /// <summary>
        /// 插入产线数据
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        Task<decimal> InsertProductLine(SfcsProductionAddOrModifyModel model);

        

        /// <summary>
        /// 获取序号
        /// </summary>
        /// <returns></returns>
        Task<decimal> GetSequenceValue(string name);

        /// <summary>
        /// 插入监控表数据
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        Task<decimal> InsertMaterial(MesHiMaterialListenAddOrModifyModel model);

        /// <summary>
        /// 插入HIREEL
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        Task<decimal> InsertHIREEL(MesHiReelAddOrModifyModel model);

        /// <summary>
        /// 新工单上线，旧工单下线,旧工单物料自动转移
        /// </summary>
        /// <param name="oldBatchNo"></param>
        /// <param name="newBatchNo"></param>
        /// <param name="user"></param>
        /// <param name="keeyWo"></param>
        /// <returns></returns>
        Task productKepMaterial(String oldBatchNo, String newBatchNo, String user, bool keeyWo);

        /// <summary>
        /// 设置制程
        /// </summary>
        /// <param name="woNo"></param>
        /// <param name="reelId"></param>
        /// <param name="operationLineId"></param>
        /// <returns></returns>
       Task<decimal> SetRoute(decimal reelId, string woNO);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="reelId"></param>
        /// <returns></returns>
        Task<decimal> GetOnhandQty(string reelId);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="status"></param>
        /// <returns></returns>
        string CastReelStatus(int status);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="reelCode"></param>
        /// <returns></returns>
        Task<Reel> GetReel(string reelCode);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="ID"></param>
        /// <returns></returns>
        Task<decimal> Update_ExistReel(object ID);

        /// <summary>
        /// 查询备料
        /// </summary>
        /// <param name="reelId"></param>
        /// <returns></returns>
        Task<List<MesHiReel>> GetHiReelData(string reelId);

        /// <summary>
        /// 更新监控数据
        /// </summary>
        /// <param name="id"></param>
        /// <param name="preQty"></param>
        /// <param name="reelId"></param>
        /// <returns></returns>
        Task<decimal> UpdateListen(decimal id, decimal preQty, string reelId = "");


        /// <summary>
        /// 查监听表的已经备完料
        /// </summary>
        /// <param name="Batch_no"></param>
        /// <returns></returns>
        Task<decimal> SelectListenBYBatch(string Batch_no);

        /// <summary>
        /// 更新产线开工
        /// </summary>
        /// <param name="Batch_no"></param>
        /// <returns></returns>
        Task<decimal> UpdateProductionBYBatch(string Batch_no);

        /// <summary>
        /// 更新为下线
        /// </summary>
        /// <param name="user"></param>
        /// <param name="lineId"></param>
        /// <param name="woNo"></param>
        /// <returns></returns>
        Task<decimal> UpdateProductLine(String user, decimal lineId, String woNo);

        /// <summary>
        /// 关闭当前上线工单的手插件物料状态监听表
        /// </summary>
        /// <param name="batchNo"></param>
        /// <returns></returns>
        Task<decimal> UpdateCloseListen(string batchNo);

        /// <summary>
        /// 事务开启动
        /// </summary>
        /// <returns></returns>
        Task StartTransaction();

        /// <summary>
        /// 提交事务
        /// </summary>
        /// <returns></returns>
        Task CommitTransaction();

        /// <summary>
        /// 回滚事务
        /// </summary>
        /// <returns></returns>
        Task RollbackTransaction();

        /// <summary>
        /// 关闭事务
        /// </summary>
        /// <param name="tran"></param>
        /// <returns></returns>
        Task<bool> CloseTransaction();

        /// <summary>
        /// 查找E-BOM
        /// </summary>
        /// <param name="PART_NO"></param>
        /// <returns></returns>
        Task<List<dynamic>> Select_ebomMaterialListen(string PART_NO);

        /// <summary>
        /// 检查BOM
        /// </summary>
        /// <param name="Part_ID"></param>
        /// <returns></returns>
        Task<dynamic> CheckBOMByPartID(decimal Part_ID);

    }
}