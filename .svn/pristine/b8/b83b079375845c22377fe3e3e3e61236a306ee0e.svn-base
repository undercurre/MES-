using System;
using System.Linq;
using JZ.IMS.Core;
using JZ.IMS.Models;
using JZ.IMS.ViewModels;
using System.Reflection;
using JZ.IMS.Core.Utilities;
using JZ.IMS.Core.Repository;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace JZ.IMS.Job
{
    public class GetRangerNo
    {

        public SfcsContainerList GetRangerNoByRuleType(Propertyprovider propertyprovider, int rule_type, IBaseRepository<SfcsRuncard, decimal> repository)
        {
            SfcsContainerList containerList = null;
            try
            {
                //先是工单号再到成品料号再到产品系列最后客户
                string conditions = string.Empty;
                string fromSql = "SELECT * FROM SFCS_RUNCARD_RANGER_RULES ";
                string partNumberFound = string.Empty;
                string familyIDFound = string.Empty;
                string platformIDFound = string.Empty;
                string customerIDFound = string.Empty;
                string totalRuleFound = string.Empty;

                #region 获取设置的规则
                //先是成品料号再到产品系列最后客户 
                SfcsRuncardRangerRules runcardRangerRule = null;

                if (!propertyprovider.product.workOrder.IsNullOrEmpty())
                {
                    conditions = "WHERE WO_NO =:WO_NO AND Enabled ='Y' AND RULE_TYPE =:RULE_TYPE ";
                    SfcsRuncardRangerRules runcardRangerRuleByWo = repository.QueryEx<SfcsRuncardRangerRules>(fromSql + conditions, new { WO_NO = propertyprovider.product.workOrder, RULE_TYPE = rule_type }).FirstOrDefault();
                    if (runcardRangerRuleByWo != null)
                    {
                        runcardRangerRule = runcardRangerRuleByWo;
                    }
                }

                if (runcardRangerRule == null && !propertyprovider.product.partNumber.IsNullOrEmpty())
                {
                    conditions = "WHERE PART_NO =:PART_NO AND Enabled ='Y' AND RULE_TYPE =:RULE_TYPE ";
                    SfcsRuncardRangerRules runcardRangerRuleByPN = repository.QueryEx<SfcsRuncardRangerRules>(fromSql + conditions, new { PART_NO = propertyprovider.product.partNumber, RULE_TYPE = rule_type }).FirstOrDefault();
                    if (runcardRangerRuleByPN != null)
                    {
                        runcardRangerRule = runcardRangerRuleByPN;
                    }
                }

                if (runcardRangerRule == null && propertyprovider.product.familyID != null && propertyprovider.product.familyID > 0)
                {
                    SfcsRuncardRangerRules runcardRangerRuleByFamily = null;
                    conditions = "WHERE PLATFORM_ID =:PRODUCT_FAMILY_ID AND Enabled ='Y' AND RULE_TYPE =:RULE_TYPE ";
                    runcardRangerRuleByFamily = repository.QueryEx<SfcsRuncardRangerRules>(fromSql + conditions, new { PRODUCT_FAMILY_ID = propertyprovider.product.familyID, RULE_TYPE = rule_type }).FirstOrDefault();
                    if (runcardRangerRuleByFamily != null)
                    {
                        runcardRangerRule = runcardRangerRuleByFamily;
                    }
                }

                if (runcardRangerRule == null && propertyprovider.product.customerID != null && propertyprovider.product.customerID > 0)
                {
                    SfcsRuncardRangerRules runcardRangerRuleByCustomer = null;
                    conditions = "WHERE CUSTOMER_ID =:CUSTOMER_ID AND Enabled ='Y' AND RULE_TYPE =:RULE_TYPE ";
                    runcardRangerRuleByCustomer = repository.QueryEx<SfcsRuncardRangerRules>(fromSql + conditions, new { CUSTOMER_ID = propertyprovider.product.customerID, RULE_TYPE = rule_type }).FirstOrDefault();
                    if (runcardRangerRuleByCustomer == null)
                    {
                        conditions = "WHERE CUSTOMER_ID =:CUSTOMER_ID AND Enabled ='Y' AND RULE_TYPE =:RULE_TYPE ";
                        runcardRangerRuleByCustomer = repository.QueryEx<SfcsRuncardRangerRules>(fromSql + conditions, new { CUSTOMER_ID = 127537, RULE_TYPE = rule_type }).FirstOrDefault();

                    }
                    if (runcardRangerRuleByCustomer != null)
                    {
                        runcardRangerRule = runcardRangerRuleByCustomer;
                    }
                }

                #endregion

                #region 按规则生成号码

                if (runcardRangerRule != null)
                {
                    // 辨析規則中的變化編碼
                    string fixHeader = IdentitySpecialCodes(runcardRangerRule.FIX_HEADER.IsNullOrEmpty() ? "" : runcardRangerRule.FIX_HEADER, repository);
                    string fixTail = string.Empty;
                    if (!runcardRangerRule.FIX_TAIL.IsNullOrEmpty())
                    {
                        fixTail = IdentitySpecialCodes(runcardRangerRule.FIX_TAIL, repository);
                    }
                    if (fixHeader.IsNullOrEmpty() && fixTail.IsNullOrEmpty())
                    {
                        throw new Exception("固定头和固定尾不能同时为空!");
                    }

                    //检查进制
                    conditions = " SELECT * FROM SFCS_PARAMETERS WHERE LOOKUP_TYPE =:LOOKUP_TYPE AND LOOKUP_CODE =:LOOKUP_CODE ";
                    SfcsParameters row = repository.QueryEx<SfcsParameters>(conditions, new { LOOKUP_TYPE = "RADIX_TYPE", LOOKUP_CODE = runcardRangerRule.DIGITAL }).FirstOrDefault();
                    if (row == null)
                    {
                        throw new Exception("进制类型设定有误，请联系管理员处理。");
                    }
                    string radixString = row.DESCRIPTION;

                    conditions = @" SELECT * FROM SFCS_CONTAINER_LIST WHERE RANGER_RULE_ID = :RANGER_RULE_ID AND DIGITAL = :DIGITAL AND RANGE = :RANGE AND DATA_TYPE = :DATA_TYPE";

                    if (!fixHeader.IsNullOrEmpty())
                    {
                        conditions += " AND FIX_HEADER = :FIX_HEADER ";
                    }
                    if (!fixTail.IsNullOrEmpty())
                    {
                        conditions += " AND FIX_TAIL = :FIX_TAIL ";
                    }
                    decimal data_type = 0;
                    if (rule_type == GlobalVariables.RangerCartonNo)
                    {
                        data_type = GlobalVariables.CartonLable;
                    }
                    else if (rule_type == GlobalVariables.RangerPallectNo)
                    {
                        data_type = GlobalVariables.PallectLabel;
                    }
                    else
                    {
                        throw new Exception("规则类型错误！");
                    }
                    conditions += " ORDER BY CONTAINER_SN DESC ";

                    containerList = repository.QueryEx<SfcsContainerList>(conditions, new
                    {
                        RANGER_RULE_ID = runcardRangerRule.ID,
                        DIGITAL = runcardRangerRule.DIGITAL,
                        RANGE = runcardRangerRule.RANGE_LENGTH,
                        FIX_HEADER = fixHeader,
                        FIX_TAIL = fixTail,
                        DATA_TYPE = data_type
                    }).FirstOrDefault();

                    if (containerList == null)
                    {
                        containerList = new SfcsContainerList();
                        // 第一段範圍，依照指定的開始碼產生SN Begin
                        containerList.CONTAINER_SN = (fixHeader + runcardRangerRule.RANGE_START_CODE + fixTail).Trim();
                    }
                    else
                    {
                        int rangeLength = (int)runcardRangerRule.RANGE_LENGTH;
                        string lastSNCode = containerList.CONTAINER_SN.Substring(fixHeader.Length, rangeLength);
                        //RadixConvertPublic._localizer = _localizer;
                        string snBeginCode = RadixConvertPublic.RadixInc(lastSNCode, radixString, 1).PadLeft(rangeLength, '0');
                        containerList.CONTAINER_SN = fixHeader + snBeginCode + fixTail;
                    }

                    containerList.RANGER_RULE_ID = runcardRangerRule.ID;
                    containerList.DIGITAL = runcardRangerRule.DIGITAL ?? 0;
                    containerList.RANGE = runcardRangerRule.RANGE_LENGTH;
                    containerList.FIX_HEADER = fixHeader;
                    containerList.FIX_TAIL = fixTail;
                }

                #endregion
            }
            catch (Exception ex)
            {
                containerList = null;
            }
            return containerList;
        }

        /// <summary>
        /// 辨析特殊代码
        /// </summary>
        /// <param name="sourceString"></param>
        /// <returns></returns>
        private string IdentitySpecialCodes(string sourceString, IBaseRepository<SfcsRuncard, decimal> repository, Decimal? rangerRuleId = 0)
        {
            string verifiedString = sourceString;

            // 獲取特殊字符並解析
            Regex regex = new Regex(@"(?<=\<)[^\<^\>]+(?=\>)");
            MatchCollection matchCollection = regex.Matches(sourceString);
            for (int i = 0; i < matchCollection.Count; i++)
            {
                //处理成品料号的规则
                if (matchCollection[i].Value.Contains("PN"))
                {
                    var specialCodesSplitList = matchCollection[i].Value.Replace("PN", "").Split(new char[] { '(', ',', ')' }, StringSplitOptions.RemoveEmptyEntries);
                    if (specialCodesSplitList.Length == 2)
                    {
                        //查规则的成品料号
                        //var ruleModel = (await _repository.GetListByTableEX<SfcsRuncardRangerRules>("*", "SFCS_RUNCARD_RANGER_RULES", " AND ID=:ID", new { ID = rangerRuleId }))?.FirstOrDefault();
                        SfcsRuncardRangerRules ruleModel = repository.QueryEx<SfcsRuncardRangerRules>("SELECT * FROM SFCS_RUNCARD_RANGER_RULES WHERE ID=:ID ", new { ID = rangerRuleId }).FirstOrDefault();
                        if (ruleModel != null && !ruleModel.PART_NO.IsNullOrEmpty())
                        {
                            //规则处理:<PN(0,2)> 截取0位的2个长度
                            int startIndex = Convert.ToInt32(specialCodesSplitList[0]);
                            int length = Convert.ToInt32(specialCodesSplitList[1]);
                            verifiedString = verifiedString.Replace("<" + matchCollection[i].Value.ToString() + ">", "") + "" + ruleModel.PART_NO.Substring(startIndex, length);
                        }
                        else
                        {
                            verifiedString = verifiedString.Replace("<" + matchCollection[i].Value.ToString() + ">", "");
                        }
                    }
                    break;
                }

                //处理工单的规则
                if (matchCollection[i].Value.Contains("WO"))
                {
                    var specialCodesSplitList = matchCollection[i].Value.Replace("WO", "").Split(new char[] { '(', ',', ')' }, StringSplitOptions.RemoveEmptyEntries);
                    if (specialCodesSplitList.Length == 2)
                    {
                        //查规则的成品料号
                        //var ruleModel = (await _repository.GetListByTableEX<SfcsRuncardRangerRules>("*", "SFCS_RUNCARD_RANGER_RULES", " AND ID=:ID", new { ID = rangerRuleId }))?.FirstOrDefault();
                        SfcsRuncardRangerRules ruleModel = repository.QueryEx<SfcsRuncardRangerRules>("SELECT * FROM SFCS_RUNCARD_RANGER_RULES WHERE ID=:ID ", new { ID = rangerRuleId }).FirstOrDefault();
                        if (ruleModel != null && !ruleModel.WO_NO.IsNullOrEmpty())
                        {
                            //规则处理:<WO(0,2)> 截取0位的2个长度
                            int startIndex = Convert.ToInt32(specialCodesSplitList[0]);
                            int length = Convert.ToInt32(specialCodesSplitList[1]);
                            verifiedString = verifiedString.Replace("<" + matchCollection[i].Value.ToString() + ">", "") + "" + ruleModel.PART_NO.Substring(startIndex, length);
                        }
                        else
                        {
                            verifiedString = verifiedString.Replace("<" + matchCollection[i].Value.ToString() + ">", "");
                        }
                    }
                    break;
                }

                switch (matchCollection[i].Value)
                {
                    case "YYWW":
                        string currentYYWW = repository.GetYYWWEx();
                        verifiedString = verifiedString.Replace("<YYWW>", currentYYWW);
                        break;
                    case "YYIW":
                        string currentYYIW = repository.GetYYIWEx();
                        verifiedString = verifiedString.Replace("<YYIW>", currentYYIW);
                        break;
                    case "IYIW":
                        string currentIYIW = repository.GetIYIWEx();
                        verifiedString = verifiedString.Replace("<IYIW>", currentIYIW);
                        break;
                    case "YIW":
                        string currentYIW = repository.Get_Intel_YIWEx();
                        verifiedString = verifiedString.Replace("<YIW>", currentYIW);
                        break;
                    case "IW":
                        string currentIW = repository.GetIWEx();
                        verifiedString = verifiedString.Replace("<IW>", currentIW);
                        break;
                    case "Y":
                        string currentY = repository.GetYEx();
                        verifiedString = verifiedString.Replace("<Y>", currentY);
                        break;
                    case "YY":
                        string currentYY = repository.GetYYEx();
                        verifiedString = verifiedString.Replace("<YY>", currentYY);
                        break;
                    case "YYYY":
                        string currentYYYY = repository.GetYYYYEx();
                        verifiedString = verifiedString.Replace("<YYYY>", currentYYYY);
                        break;
                    case "MM":
                        string currentMM = repository.GetMMEx();
                        verifiedString = verifiedString.Replace("<MM>", currentMM);
                        break;
                    case "DD":
                        string currentDD = repository.GetDDEx();
                        verifiedString = verifiedString.Replace("<DD>", currentDD);
                        break;
                    case "YYYYMMDD":
                        string currentYYYYMMDD = repository.GetYYYYMMDDEx();
                        verifiedString = verifiedString.Replace("<YYYYMMDD>", currentYYYYMMDD);
                        break;
                    case "YYMMDD":
                        string currentYYMMDD = repository.GetYYMMDDEx();
                        verifiedString = verifiedString.Replace("<YYMMDD>", currentYYMMDD);
                        break;
                    case "YYYYMM":
                        string currentYYYYMM = repository.GetYYYYMMEx();
                        verifiedString = verifiedString.Replace("<YYYYMM>", currentYYYYMM);
                        break;
                    case "YYMM":
                        string currentYYMM = repository.GetYYMMEx();
                        verifiedString = verifiedString.Replace("<YYMM>", currentYYMM);
                        break;
                    case "MMDD":
                        string currentMMDD = repository.GetMMDDEx();
                        verifiedString = verifiedString.Replace("<MMDD>", currentMMDD);
                        break;
                    case "WW":
                        string currentWW = repository.GetWWEx();
                        verifiedString = verifiedString.Replace("<WW>", currentWW);
                        break;
                    case "WWD":
                        String currentWWD = repository.GetWWDEx();
                        Decimal current = Decimal.Parse(currentWWD) - 1;
                        if (current % 10 == 0)
                        {
                            current = current + 7;
                        }
                        verifiedString = verifiedString.Replace("<WWD>", current.ToString());
                        break;
                    default:
                        break;
                }
            }
            return verifiedString;
        }

        public Dictionary<string, dynamic> GetData<T>(T tEntiy)
        {
            var dicData = new Dictionary<string, dynamic>();

            Type objType = tEntiy.GetType();//获取model的类型
            foreach (PropertyInfo propInfo in objType.GetProperties())//打印特性及模型的属性
            {
                Console.WriteLine(propInfo.PropertyType.Namespace);
                if (propInfo.PropertyType.Namespace == "System")
                {

                    object[] objAttrs = propInfo.GetCustomAttributes(typeof(ParameterAttribute), true);
                    if (objAttrs.Length > 0)
                    {
                        ParameterAttribute attr = objAttrs[0] as ParameterAttribute;
                        if (attr != null)
                        {
                            string attributeColumn = attr.Name;//模型上的特性的名
                            object value = propInfo.GetValue(tEntiy, null);//模型上的属性值
                            dicData.Add(attributeColumn, value);
                        }
                    }
                }
                else if (propInfo.PropertyType.IsClass && propInfo.PropertyType.Name != "List`1")
                {
                    if (propInfo.GetValue(tEntiy, null) != null)
                    {

                        var dicData2 = GetData(propInfo.GetValue(tEntiy, null));
                        if (dicData2.Count > 0)
                        {
                            dicData = dicData.Concat(dicData2).ToDictionary(k => k.Key, v => v.Value);
                        }
                    }
                }

            }
            return dicData;
        }

    }
}
