using JZ.IMS.Core;
using JZ.IMS.Core.Repository;
using JZ.IMS.Core.Utilities.Reflect;
using JZ.IMS.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JZ.IMS.Job.RuncardJobStorage
{
    public class CheckPacking : IMesJob<SfcsRuncard, decimal>
    {
        public Task<KeyValuePair<bool, string>> GetTask(Propertyprovider propertyprovider, IBaseRepository<SfcsRuncard, decimal> repository, IDbTransaction transaction)
        {
            return Task.Run(() =>
            {
                try
                {
                    ///检查包装工序有没有配置包装配置
                    //if(propertyprovider.sfcsOperationSites.OPERATION_ID == 200)
                    //{
                    //    SfcsProductCarton sfcsProductCarton = repository.QueryEx<SfcsProductCarton>(
                    //        "select * from SFCS_PRODUCT_CARTON WHERE PART_NO = :PART_NO AND COLLECT_OPERATION_ID= :COLLECT_OPERATION_ID",
                    //        new
                    //        {
                    //            PART_NO = propertyprovider.product.partNumber,
                    //            COLLECT_OPERATION_ID = 200
                    //        }).FirstOrDefault();
                    //    if(sfcsProductCarton == null)
                    //    {
                    //        throw new Exception(String.Format("请工程人员维护好料号：{0}包装工序的的箱号配置!", propertyprovider.product.partNumber));
                    //    }
                    //}
                    return new KeyValuePair<bool, string>(true, "");
                }
                catch (Exception ex)
                {

                    return new KeyValuePair<bool, string>(false, "CheckRoute:" + ex.Message);
                }
            });
        }
    }
}
