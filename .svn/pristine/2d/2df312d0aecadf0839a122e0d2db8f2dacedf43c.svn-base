/**
*┌──────────────────────────────────────────────────────────────┐
*│　描    述：设备点检维修表接口实现                                                    
*│　作    者：嘉志科技                                            
*│　版    本：2.0    模板代码自动生成                                                
*│　创建时间：2019-10-31 10:50:09                             
*└──────────────────────────────────────────────────────────────┘
*┌──────────────────────────────────────────────────────────────┐
*│　命名空间： JZ.IMS.Repository.Oracle                                  
*│　类    名： SfcsEquipRepairHeadRepository                                      
*└──────────────────────────────────────────────────────────────┘
*/
using JZ.IMS.Core.DbHelper;
using JZ.IMS.Core.Options;
using JZ.IMS.Core.Repository;
using JZ.IMS.IRepository;
using JZ.IMS.Models;
using Dapper;
using Microsoft.Extensions.Options;
using System;
using System.Threading.Tasks;
using JZ.IMS.ViewModels;
using System.Collections.Generic;
using System.Linq;

namespace JZ.IMS.Repository.Oracle
{
	public class SfcsEquipRepairHeadRepository : BaseRepository<SfcsEquipRepairHead, decimal>, ISfcsEquipRepairHeadRepository
	{
		private readonly ISfcsEquipRepairDetailRepository _detailRepository;
		private readonly ISfcsEquipmentRepository _repositoryEquipment;

		public SfcsEquipRepairHeadRepository(IOptionsSnapshot<DbOption> options, ISfcsEquipRepairDetailRepository detailRepository,
			ISfcsEquipmentRepository repositoryEquipment)
		{
			_detailRepository = detailRepository;
			_repositoryEquipment = repositoryEquipment;

			_dbOption = options.Get("iWMS");
			if (_dbOption == null)
			{
				throw new ArgumentNullException(nameof(DbOption));
			}
			_dbConnection = ConnectionFactory.CreateConnection(_dbOption.DbType, _dbOption.ConnectionString);
		}

		/// <summary>
		/// 根据主键获取激活状态
		/// </summary>
		/// <param name="id">主键</param>
		/// <returns></returns>
		public async Task<Boolean> GetEnableStatus(decimal id)
		{
			string sql = "SELECT ENABLED FROM SFCS_EQUIP_REPAIR_HEAD WHERE ID=:ID AND IS_DELETE='N'";
			var result = await _dbConnection.QueryFirstOrDefaultAsync<string>(sql, new
			{
				ID = id,
			});

			return result == "Y" ? true : false;
		}

		/// <summary>
		/// 修改激活状态
		/// </summary>
		/// <param name="id">主键</param>
		/// <param name="status">更改后的状态</param>
		/// <returns></returns>
		public async Task<decimal> ChangeEnableStatus(decimal id, bool status)
		{
			string sql = "UPDATE SFCS_EQUIP_REPAIR_HEAD set ENABLED=:ENABLED where  Id=:Id";
			return await _dbConnection.ExecuteAsync(sql, new
			{
				ENABLED = status ? 'Y' : 'N',
				Id = id,
			});
		}

		// <summary>
		/// 获取表的序列
		/// </summary>
		/// <returns></returns>
		public async Task<decimal> GetSEQID()
		{
			string sql = "SELECT SFCS_EQUIP_REPAIR_HEAD_SEQ.NEXTVAL MY_SEQ FROM DUAL";
			var result = await _dbConnection.ExecuteScalarAsync(sql);
			return (decimal)result;
		}

		// <summary>
		/// 获取维修记录列表
		/// </summary>
		/// <returns></returns>
		public async Task<IEnumerable<SfcsEquipRepairHeadListModel>> GetLoadData(int pageNumber, int rowsPerPage, decimal equipId)
		{
			string whereStr = " where 1=1 ";
			whereStr += " and tb1.EQUIP_ID = :EQUIP_ID ";
			string sql = string.Format("select * from (select tt.*,ROWNUM AS rowno from (select tb1.*,tb2.NAME AS EQUIP_NAME from SFCS_EQUIP_REPAIR_HEAD tb1 inner join SFCS_EQUIPMENT tb2 on TB1.EQUIP_ID = TB2.ID {0} order by tb1.ID desc) tt where ROWNUM <= {1}) tt2 where tt2.rowno >= {2}", whereStr, rowsPerPage * pageNumber, (rowsPerPage - 1) * pageNumber);

			return await _dbConnection.QueryAsync<SfcsEquipRepairHeadListModel>(sql, new { EQUIP_ID = equipId });
		}

		/// <summary>
		/// 获取维修记录数
		/// </summary>
		/// <returns></returns>
		public async Task<int> GetLoadDataCnt(decimal equipId)
		{
			string whereStr = " where 1=1 ";
			whereStr += " and tb1.EQUIP_ID = :EQUIP_ID ";
			string sql = "select count(*) from SFCS_EQUIP_REPAIR_HEAD tb1 inner join SFCS_EQUIPMENT tb2 on TB1.EQUIP_ID = TB2.ID  " + whereStr;

			return await _dbConnection.ExecuteScalarAsync<int>(sql, new { EQUIP_ID = equipId });
		}

		/// <summary>
		/// 获取正在维修的记录ID
		/// </summary>
		/// <returns></returns>
		public decimal? GetRepairID(decimal equipId)
		{
			string sql = "select ID from SFCS_EQUIP_REPAIR_HEAD  where EQUIP_ID = :EQUIP_ID and REPAIR_STATUS = 3";

			return (_dbConnection.Query<decimal>(sql, new { EQUIP_ID = equipId })).FirstOrDefault();
		}

        /// <summary>
        /// 新增或更新设备维修记录并对设备以及维修配件信息做相应操作
        /// </summary>
        /// <param name="item"></param>
        /// <returns></returns>
        public async Task<BaseResult> AddOrModifyData(SfcsEquipRepairHeadAddOrModifyModel item)
        {
            BaseResult result = new BaseResult();
            result.ResultData = "-1";
            ConnectionFactory.OpenConnection(_dbConnection);
            using (var tran = _dbConnection.BeginTransaction())
            {
                try
                {
                    decimal detailID = 0;
                    if (item.ID <= 0) { detailID = await GetSEQID(); } else { detailID = item.ID; }
                    //新增维修配件记录
                    if (item.DetailList != null)
                    {
                        foreach (var detail in item.DetailList)
                        {
                            detail.EQUIP_REPAIR_HEAD_ID = detailID;
                            detail.ID = await _detailRepository.GetSEQID();
                            if (await _detailRepository.InsertAsync(detail) <= 0)
                            {
                                tran.Rollback();
                                result.ResultCode = ResultCodeAddMsgKeys.CommonExceptionCode;
                                result.ResultMsg = "添加维修配件信息异常！";
                                return result;
                            }
                        }
                    }

                    //更新设备状态
                    if (await _repositoryEquipment.EditEquipStatus(item.EQUIP_ID, item.REPAIR_STATUS) <= 0)
                    {
                        tran.Rollback();
                        result.ResultCode = ResultCodeAddMsgKeys.CommonExceptionCode;
                        result.ResultMsg = "更新设备状态异常！";
                        return result;
                    }

                    SfcsEquipRepairHead model;
                    if (item.ID == 0)
                    {
                        //新增
                        model = new SfcsEquipRepairHead
                        {
                            ID = detailID,
                            REPAIR_CODE = item.REPAIR_CODE,
                            BEGINTIME = item.BEGINTIME,
                            REPAIR_USER = item.REPAIR_USER,
                            EQUIP_ID = item.EQUIP_ID,
                            REPAIR_STATUS = item.REPAIR_STATUS.ToString()
                        };
                        if (await InsertAsync(model) <= 0)
                        {
                            tran.Rollback();
                            result.ResultCode = ResultCodeAddMsgKeys.CommonExceptionCode;
                            result.ResultMsg = "新增维修记录信息异常！";
                            return result;
                        }
                        result.ResultData = detailID.ToString();
                    }
                    else
                    {
                        //修改
                        model = Get(item.ID);
                        model.REPAIR_CONTENT = item.REPAIR_CONTENT;
                        model.REPAIR_STATUS = item.REPAIR_STATUS.ToString();
                        model.ENDTIME = item.ENDTIME;
                        if (await UpdateAsync(model) <= 0)
                        {
                            tran.Rollback();
                            result.ResultCode = ResultCodeAddMsgKeys.CommonExceptionCode;
                            result.ResultMsg = "更新维修记录信息异常！";
                            return result;
                        }
                        result.ResultData = model.ID.ToString();
                    }

                    tran.Commit();
                }
                catch
                {
                    tran.Rollback(); // 回滚事务
                    throw;
                }
                finally
                {
                    if (_dbConnection.State != System.Data.ConnectionState.Closed)
                    {
                        _dbConnection.Close();
                    }
                }
            }

            result.ResultCode = ResultCodeAddMsgKeys.CommonObjectSuccessCode;
            result.ResultMsg = ResultCodeAddMsgKeys.CommonObjectSuccessMsg; ;
            return result;
        }
    }
}