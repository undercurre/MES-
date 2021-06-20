using JZ.IMS.Core.Repository;
using JZ.IMS.Models.SfcsTinRecord;
using System;
using System.Threading.Tasks;
using JZ.IMS.Models.SfcsTinRecord;

namespace JZ.IMS.IRepository
{
    public interface ISfcsTinRecordRepository : IBaseRepository<SfcsTinRecord, Decimal>
    {
        /// <summary>
        /// 根据主键获取激活状态
        /// </summary>
        /// <param name="id">主键</param>
        /// <returns></returns>
        Task<Boolean> GetEnableStatus(decimal id);

        // <summary>
        /// 获取表的序列
        /// </summary>
        /// <returns></returns>
        Task<decimal> GetSEQID();

        Task<decimal> GetApsOutputAsync(Decimal LINE_ID, DateTime OUTPUT_DAY);
    }
}