
using System;
using System.Collections.Generic;
using System.Data;
using System.Text;
using JZ.IMS.Core;
using JZ.IMS.Models;
using System.Threading.Tasks;
using JZ.IMS.Core.Repository;
using JZ.IMS.Core.Utilities.Reflect;
using System.Net;
using System.IO;
using System.Linq;
using Newtonsoft.Json.Linq;

namespace JZ.IMS.Job.FinallyJobStorage
{
    public class AutoCompleteRptDoc : IMesFinallyJob<SfcsRuncard, decimal>
    {
        public Task<KeyValuePair<bool, string>> GetTask(Propertyprovider propertyprovider, IBaseRepository<SfcsRuncard, decimal> repository, IDbTransaction transaction)
        {
            return Task.Run(() =>
            {
                try
                {
                    //工单状态为4的时候记录完工报告
                    if (propertyprovider.sys_Manager != null && propertyprovider.product != null && propertyprovider.product.sfcswo != null)
                    {
                        SfcsWo wo = repository.QueryEx<SfcsWo>("SELECT * FROM SFCS_WO WHERE ID = :ID ", new { ID = propertyprovider.product.sfcswo.ID }, transaction).FirstOrDefault();
                        String postUrl = repository.QueryEx<String>("SELECT T.DESCRIPTION FROM SFCS_PARAMETERS T WHERE T.LOOKUP_TYPE ='U9ERP_URL' AND T.ENABLED = 'Y' AND T.LOOKUP_CODE = '100'")?.FirstOrDefault();
                        if (wo != null && wo.WO_STATUS == 4 && !String.IsNullOrEmpty(postUrl))
                        {
                            JObject joStr = new JObject();
                            joStr["USER_NAME"] = propertyprovider.sys_Manager.USER_NAME;//入库人
                            joStr["OUTPUT_QTY"] = wo.OUTPUT_QTY;//完工数量
                            joStr["WO_NO"] = propertyprovider.product.sfcswo.WO_NO;//工单号
                            joStr["RCVWH"] = repository.QueryEx<String>("SELECT ATTRIBUTE3 FROM IMS_PART WHERE CODE = :CODE ", new { CODE = wo.PART_NO }, transaction).FirstOrDefault();//存储地点

                            String jsonStr = Newtonsoft.Json.JsonConvert.SerializeObject(joStr);
                            postUrl = postUrl + "?JsonStr=" + jsonStr;
                                                                     
                            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(postUrl);
                            request.Method = "POST";
                            request.ContentType = "text/html;charset=UTF-8";
                            request.ContentLength = 0;

                            HttpWebResponse response = (HttpWebResponse)request.GetResponse();
                            Stream myResponseStream = response.GetResponseStream();
                            StreamReader myStreamReader = new StreamReader(myResponseStream, Encoding.UTF8);
                            string retString = myStreamReader.ReadToEnd();
                            myStreamReader.Close();
                            myResponseStream.Close();
                            JObject jo = (JObject)Newtonsoft.Json.JsonConvert.DeserializeObject(retString);
                            if (jo["Code"].ToString() == "0") { throw new Exception(jo["Msg"].ToString()); }
                        }
                    }

                    return new KeyValuePair<bool, string>(true, "");
                }
                catch (Exception ex)
                {
                    return new KeyValuePair<bool, string>(false, "AutoCompleteRpt:" + ex.Message);
                }
            });
        }
    }
}
