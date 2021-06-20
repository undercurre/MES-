/**
*┌──────────────────────────────────────────────────────────────┐
*│　描    述：工艺路线                                                    
*│　作    者：Admin                                                                    
*└──────────────────────────────────────────────────────────────┘
*┌──────────────────────────────────────────────────────────────┐
*│　命名空间： JZ.IMS.IRepository                                   
*│　接口名称： ISOP_ROUTESRepository                                      
*└──────────────────────────────────────────────────────────────┘
*/
using JZ.IMS.Core.Repository;
using JZ.IMS.Models;
using JZ.IMS.Models.SOP;
using JZ.IMS.ViewModels;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace JZ.IMS.IRepository
{
    public interface ISimpleSOP_ROUTESRepository : IBaseRepository<SOP_ROUTES, decimal>
    {

        /// <summary>
        /// 获取所有线别列表
        /// </summary>
        /// <returns></returns>
        Task<List<SfcsEquipmentLinesModel>> GetLinesList();

        /// <summary>
        /// 获取产线列表
        /// </summary>
        /// <returns></returns>
        Task<List<SfcsEquipmentLinesModel>> GetRoHSLinesList();

        /// <summary>
        /// 获取SMT线列表
        /// </summary>
        /// <returns></returns>
        Task<List<SfcsEquipmentLinesModel>> GetSMTLinesList();

        /// <summary>
        /// 获取激活的工序集合
        /// </summary>
        /// <returns></returns>
        Task<List<SfcsOperationsListModel>> GetEnabledLists();

        /// <summary>
        /// 产品图:资源对象
        /// </summary>
        /// <param name="parent_id"></param>
        /// <returns></returns>
        Task<SOP_OPERATIONS_ROUTES_RESOURCE> LoadResourceProductData(decimal parent_id);

        /// <summary>
        /// 零件图:资源列表
        /// </summary>
        /// <param name="parent_id"></param>
        /// <returns></returns>
        Task<TableDataModel> LoadResourceCmpData(decimal parent_id);

        /// <summary>
        /// 作业图:资源列表
        /// </summary>
        /// <param name="parent_id"></param>
        /// <returns></returns>
        Task<TableDataModel> LoadResourceData(decimal parent_id);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="operation_id"></param>
        /// <param name="operation_site_id"></param>
        /// <returns></returns>
        Task<bool> GetCallRecord(decimal operation_id, decimal operation_site_id);

        /// <summary>
        /// 根据线体获取站点信息
        /// </summary>
        /// <param name="lineId"></param>
        /// <returns></returns>
        Task<List<SfcsOperationSitesListModel>> GetOperationSiteList(decimal lineId);

        /// <summary>
        /// 获取激活的工序集合
        /// </summary>
        /// <returns></returns>
        Task<IEnumerable<SfcsOperations>> GetEnabledListsync();

        /// <summary>
        /// 根据料号获取SOP配置工序
        /// </summary>
        /// <param name="partNo"></param>
        /// <param name="version"></param>
        /// <returns></returns>
        Task<List<SfcsOperations>> GetOperationsByPartNo(string partNo, string version);

        /// <summary>
        /// 根据料号获取对应可用版本号
        /// </summary>
        /// <param name="partNo"></param>
        /// <returns></returns>
        Task<List<string>> GetSopVersionsData(string partNo);

        /// <summary>
        /// 根据料号获取对应NP号
        /// </summary>
        /// <param name="partNo"></param>
        /// <returns></returns>
        Task<string> GetNpNoByPartNo(string partNo);

        /// <summary>
        /// 导出查询数据
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        Task<IEnumerable<SOP_ROUTES>> ExportData(SOPRoutesRequestModel model);

        /// <summary>
        /// 资源
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<SOP_OPERATIONS_ROUTES_RESOURCE> LoadResourceByID(decimal id);

        /// <summary>
        ///  根据零件图片ID获取零件信息
        /// </summary>
        /// <param name="sourceId"></param>
        /// <returns></returns>
        Task<SopOperationsRoutesPartListModel> GetSourcePartBySourceId(string sourceId);

        /// <summary>
        /// 获取激活的工序集合
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        Task<TableDataModel> GetEnabledListsync(PageModel model);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        bool GetDisplayStatusById(decimal id);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        Task<int> DeleteResourcePart(decimal Id);

        /// <summary>
        /// 批量删除资源
        /// </summary>
        /// <param name="bool">是否</param>
        /// <returns></returns>
        Task<bool> DeleteResourceBatch(List<decimal> idList);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        Task<TableDataModel> LoadData(SOPRoutesRequestModel model);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<dynamic> LoadDtlData(decimal id);


        /// <summary>
        /// 更新资源说明
        /// </summary>
        /// <param name="id"></param>
        /// <param name="msgInfo"></param>
        /// <returns></returns>
        Task<decimal> UnpdateResourceMsg(SOP_OPERATIONS_ROUTES_RESOURCE resource, SopOperationsRoutesPartAddOrModifyModel partInfo = null);


        /// <summary>
        /// 更新产品图资源
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        decimal UpdateResourceByID(SOP_OPERATIONS_ROUTES_RESOURCE model);

        /// <summary>
        /// 逻辑删除资源返回影响的行数
        /// </summary>
        /// <param name="ids">需要删除的主键</param>
        /// <returns>影响的行数</returns>
        Task<decimal> DeleteResource(decimal id);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<decimal> DeleteSubAsync(decimal id);

        /// <summary>
        /// 逻辑删除返回影响的行数（异步操作）
        /// </summary>
        /// <param name="ids">需要删除的主键</param>
        /// <returns>影响的行数</returns>
        Task<decimal> DeleteLogicalAsync(decimal id);

        /// <summary>
        /// 根据主键获取显示状态
        /// </summary>
        /// <param name="id">主键</param>
        /// <returns></returns>
        Task<Boolean> GetDisplayStatusByIdAsync(decimal id);

        /// <summary>
        /// 事务新增,并保存关联表数据
        /// </summary>
        /// <param name="model">实体对象</param>
        /// <returns></returns>
        decimal InsertByTrans(SOP_ROUTES model);

        /// <summary>
        /// 事务修改，并保存关联表数据
        /// </summary>
        /// <param name="model">实体对象</param>
        /// <returns></returns>
        decimal UpdateByTrans(SOP_ROUTES model);

        /// <summary>
        /// 审核
        /// </summary>
        /// <param name="id">主键</param>
        /// <param name="status">更改后的状态</param>
        /// <returns></returns>
        Task<decimal> AuditorByIdAsync(ChangeStatusModel model);

        /// <summary>
        /// 是否存在
        /// </summary>
        /// <param name="Name">别名</param>
        /// <returns></returns>
        Task<Boolean> IsExistsNameAsync(string Name);

        /// <summary>
        /// 是否存在
        /// </summary>
        /// <param name="Name">别名</param>
        /// <param name="Id">主键</param>
        /// <returns></returns>
        Task<Boolean> IsExistsAsync(string Name, string attribute1, decimal Id = 0);

        Task<decimal> GetSEQIDAsync();

        decimal Get_Detail_SEQID();

        decimal Get_Resource_SEQID();

        decimal InsertDetail(SOP_OPERATIONS_ROUTES model);

        decimal InsertResource(SOP_OPERATIONS_ROUTES_RESOURCE model);



        /// <summary>
        /// 通过料号+工序查找制程的工序信息
        /// </summary>
        /// <param name="partNo"></param>
        /// <param name="operationId"></param>
        /// <returns></returns>
        IEnumerable<SOP_Operations> getOperationsRoutes(string partNo, decimal operationId);

        IEnumerable<SOP_Operations> getOperationsRoutesPreview(string partNo, decimal operationId);

        /// <summary>
        /// SOP复制 执行方法
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        Task<int> SOPCopyAsync(SOPCopyRequestModel model);

        /// <summary>
        /// 获取设备图片
        /// </summary>
        /// <returns></returns>
        List<SOP_OPERATIONS_ROUTES_RESOURCE> GetEquipmentRoutes();

        /// <summary>
        /// 获取设备图片
        /// </summary>
        /// <returns></returns>
        int DelRoutesByMatId(decimal mstId);

        #region 设备点检事项

        /// <summary>
        /// 获取设备点检事项图片
        /// </summary>
        /// <param name="mstId"></param>
        /// <returns></returns>
        Task<IEnumerable<SOP_OPERATIONS_ROUTES_RESOURCE>> GetEquipContentConfResource(decimal mstId);

        /// <summary>
        /// 删除设备点检事项图片
        /// </summary>
        /// <param name="mstId"></param>
        /// <returns></returns>
        Task<int> DelEquipContentConfResource(decimal mstId);

        #endregion

        /// <summary>
        /// 获取工序评判标准
        /// </summary>
        /// <param name="site_id">站点ID</param>
        /// <returns></returns>
        Task<IEnumerable<SopSkillStandardListModel>> LoadSkillStandard(decimal site_id);

        /// <summary>
        /// 根据零件料号获取零件信息
        /// </summary>
        /// <param name="partNo"></param>
        /// <returns></returns>
        Task<ImsPart> GetPartByPartNo(string partNo);

        /// <summary>
        /// 根据零件料号获取零件信息（包括PLM图片）分页
        /// </summary>
        /// <param name="partNo"></param>
        /// <returns></returns>
        Task<TableDataModel> GetPartByPartNo(int pageIndex, int pageSize, string partNo);

        /// <summary>
        /// 校验SOP 物料BOM
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<BaseResult> CheckBomMaterial(decimal id);

        // <summary>
        /// 根据生效日期跟失效日期，设置SOP状态
        /// </summary>
        void SetStatusByTime();

        /// <summary>
        /// 保存资源排序
        /// </summary>
        /// <param name="list"></param>
        /// <returns></returns>
        Task<BaseResult> SaveRoutesOrderNo(List<SOP_OPERATIONS_ROUTES_RESOURCE> list);

        /// <summary>
        /// 根据成品料号获取BOM料号
        /// </summary>
        /// <returns></returns>
        Task<TableDataModel> GetBomPNByPartNo(SOPRoutesRequestModel model);

    }
}