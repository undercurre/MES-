using JZ.IMS.Core;
using JZ.IMS.Core.Utilities.Reflect;
using JZ.IMS.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Text;
using System.Threading.Tasks;

namespace JZ.IMS.Job.FinallyJobStorage
{
    public class AutoChekWoRoute : IMesFinallyJob<SfcsRuncard, decimal>
    {
        public Task<KeyValuePair<bool, string>> GetTask(Propertyprovider propertyprovider, Core.Repository.IBaseRepository<SfcsRuncard, decimal> repository, IDbTransaction transaction)
        {
            return Task.Run(() =>
            {
                try
                {
                    if(propertyprovider.product.isPartRoute)
                    {
                        repository.Execute("UPDATE SFCS_WO SET ROUTE_ID = :ROUTE_ID WHERE ID = :ID", new
                        {
                            ROUTE_ID = propertyprovider.route.routeID,
                            ID = propertyprovider.product.sfcswo.ID
                        },transaction);
                    }
                    return new KeyValuePair<bool, string>(true, "");
                }
                catch (Exception ex)
                {

                    return new KeyValuePair<bool, string>(false, "AutoChekWoRoute:" + ex.Message);
                }
            });

        }
    }
}
