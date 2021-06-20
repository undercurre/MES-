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
    public class CheckRoute : IMesJob<SfcsRuncard, decimal>
    {
        public Task<KeyValuePair<bool, string>> GetTask(Propertyprovider propertyprovider, IBaseRepository<SfcsRuncard, decimal> repository, IDbTransaction transaction)
        {
            return Task.Run(() =>
            {
                try
                {
                    SfcsIgnoreSiteHistory sfcsIgnoreSiteHistory =
                            repository.QueryEx<SfcsIgnoreSiteHistory>(
                                "SELECT * FROM SFCS_IGNORE_SITE_HISTORY WHERE SN_ID = :SN_ID AND ENABLED = :ENABLED",
                                new
                                {
                                    SN_ID = propertyprovider.sfcsRuncard.ID,
                                    ENABLED = "Y"
                                }
                                ).FirstOrDefault();
                    if (sfcsIgnoreSiteHistory != null)
                    {
                        return new KeyValuePair<bool, string>(true, "");
                    }
                    bool matchRoute = false;
                    foreach (SfcsRouteConfig sfcsRouteConfig in propertyprovider.route.sfcsRouteConfigs)
                    {
                        if (sfcsRouteConfig.CURRENT_OPERATION_ID == propertyprovider.sfcsOperationSites.OPERATION_ID)
                        {
                            matchRoute = true;
                            break;
                        }
                    }
                    if (!matchRoute)
                    {
                        return new KeyValuePair<bool, string>(false, "选择错误的站点!");
                    }
                    return new KeyValuePair<bool, string>(true, "");
                }
                catch (Exception ex)
                {

                    return new KeyValuePair<bool, string>(false, "CheckRoute:"+ ex.Message);
                }
            });
          }
    }
}
