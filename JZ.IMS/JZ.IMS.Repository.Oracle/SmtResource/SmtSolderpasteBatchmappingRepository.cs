/**
*┌──────────────────────────────────────────────────────────────┐
*│　描    述：接口实现                                                    
*│　作    者：嘉志科技                                            
*│　版    本：2.0    模板代码自动生成                                                
*│　创建时间：2020-03-03 16:03:30                             
*└──────────────────────────────────────────────────────────────┘
*┌──────────────────────────────────────────────────────────────┐
*│　命名空间： JZ.IMS.Repository.Oracle                                  
*│　类    名： SmtSolderpasteBatchmappingRepository                                      
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
using System.Collections.Generic;
using JZ.IMS.WebApi.Public;
using System.Data;
using JZ.IMS.ViewModels;
using System.Linq;

namespace JZ.IMS.Repository.Oracle
{
    public class SmtSolderpasteBatchmappingRepository:BaseRepository<SmtSolderpasteBatchmapping,String>, ISmtSolderpasteBatchmappingRepository
    {
        public SmtSolderpasteBatchmappingRepository(IOptionsSnapshot<DbOption> options)
        {
            _dbOption =options.Get("iWMS");
            if (_dbOption == null)
            {
                throw new ArgumentNullException(nameof(DbOption));
            }
            _dbConnection = ConnectionFactory.CreateConnection(_dbOption.DbType, _dbOption.ConnectionString);
        }
        //resource status
        public const decimal ResourceUsed = 1;
        public const decimal ResourceUseup = 2;
        public const decimal ResourceDisused = 3;
        public const decimal ResourceTransfer = 4;
        public const decimal ResourceIceBox = 5;
        public const decimal ResourceWarm = 6;
        public const decimal ResourceWhip = 7;
        public const decimal ResourceUsedOver = 10;
        private string validFlag;
        private string standardFlag;
        private string exposeFlag;

      
        private decimal standardTime; //分鍾

        public const string AutoUpdate = "System Update";
        public const string ManualUpdate = "Manual Update";
        /// <summary>
        /// 获取最新批次号
        /// </summary>
        /// <returns></returns>
        public async Task<String> GetBatchNo()
        {
            DateTime currentDate = await this.GetCurrentTime();
            return currentDate.ToString("yyyyMMddHHmmss");
        }

        public async Task<IEnumerable<String>> GetBatchByLoc(String loc)
        {
            String sql = "select distinct BATCH_NO from SMT_SOLDERPASTE_BATCHMAPPING  where FRIDGE_LOC =:FRIDGE_LOC order by BATCH_NO desc";
            var result = await _dbConnection.QueryAsync<string>(sql, new
            {
                FRIDGE_LOC = loc
            });
            return result;
        }
        /// <summary>
        /// 获取当前时间
        /// </summary>
        /// <returns></returns>
        public async Task<DateTime> GetCurrentTime()
        {
            String sql = "select sysdate from dual";
            DateTime currentDate = await _dbConnection.QueryFirstOrDefaultAsync<DateTime>(sql);
            return currentDate;
        }
        /// <summary>
        /// 获取冰箱的物料位置
        /// </summary>
        /// <param name="para"></param>
        /// <returns></returns>
        public async Task<IEnumerable<object>> GetLoction(String para)
        {
            string cmd_QueryByText = @"
            SELECT CODE,VALUE,CN_DESC FROM SMT_LOOKUP 
                WHERE TYPE = 'SOLDERPASTE_FRIDGE_LOC' 
                AND (CN_DESC LIKE '{0}%' OR VALUE LIKE '{0}%')";
            string sql = String.Format(cmd_QueryByText, para);

            var result = await _dbConnection.QueryAsync<object>(sql);
            return result;
        }

        /// <summary>
        /// 通过批次号获取辅料详细信息
        /// </summary>
        /// <param name="bathNo"></param>
        /// <returns></returns>
        public async Task<IEnumerable<object>> GetBatchDeatil(String bathNo)
        {
            string sql = @"
                SELECT SSB.*,SR.PART_NO,SR.PART_NAME,SR.PART_DESC FROM SMT_SOLDERPASTE_BATCHMAPPING SSB
                LEFT JOIN IMS_REEL_INFO_VIEW SR ON SR.CODE = SSB.REEL_NO
                WHERE SSB.BATCH_NO = :BATCH_NO
                ORDER BY BATCH_NO DESC
            ";
            var result = await _dbConnection.QueryAsync<object>(sql,new {
                BATCH_NO = bathNo
            });
            return result;
        }

        /// <summary>
        /// 新增辅料信息
        /// </summary>
        /// <param name="batchNo"></param>
        /// <param name="reel_no"></param>
        /// <param name="friDgeLoc"></param>
        /// <param name="user"></param>
        /// <param name="remark"></param>
        /// <returns></returns>
        public async Task<bool> AddResources(String batchNo, String reel_no , String friDgeLoc, string user, String remark)
        {
            String resourcesSql = @"SELECT ID FROM SMT_RESOURCE_RUNCARD WHERE RESOURCE_NO = :RESOURCE_NO";
            decimal resourcesCount = await _dbConnection.ExecuteScalarAsync<decimal>(resourcesSql, new
            {
                RESOURCE_NO = reel_no
            });
            if (resourcesCount > 0)
            {
                throw new Exception("传入的辅料已经冷藏过！");
            }
            ConnectionFactory.OpenConnection(_dbConnection);
            using(IDbTransaction transaction = _dbConnection.BeginTransaction())
            {
                try
                {
                    DateTime currentTime = _dbConnection.QueryFirstOrDefault<DateTime>("SELECT SYSDATE FROM DUAL");
                    SmtSolderpasteBatchmapping solderpasteBatchmapping = new SmtSolderpasteBatchmapping();
                    solderpasteBatchmapping.ID = Guid.NewGuid().ToString();
                    solderpasteBatchmapping.BATCH_NO = batchNo;
                    solderpasteBatchmapping.REEL_NO = reel_no;
                    solderpasteBatchmapping.FRIDGE_LOC = friDgeLoc;
                    solderpasteBatchmapping.OPERATOR = user;
                    solderpasteBatchmapping.REMARK = remark;
                    solderpasteBatchmapping.OPERATION_TIME = currentTime;
                    int x = await _dbConnection.InsertAsync<SmtSolderpasteBatchmapping>(solderpasteBatchmapping, transaction);
                    if(x <= 0)
                    {
                        throw new Exception("新增批次号异常!");
                    }
                    this.ProcessResourceRuncard(reel_no, -999, user);
                    transaction.Commit();
                    return true;
                }
                catch (Exception ex)
                {
                    transaction.Rollback();
                    throw ex;
                }
               
            }
            
        }

        /// <summary>
        /// 通过辅料条码获取辅料料号
        /// </summary>
        /// <param name="reelCode"></param>
        /// <returns></returns>
        public async Task<String> GetResourcesPart(String reelCode)
        {
            String sql = @"SELECT CODE FROM IMS_PART WHERE ID IN(SELECT PART_ID FROM IMS_REEL WHERE CODE = :CODE)";
            string result =  _dbConnection.QueryFirstOrDefault<string>(sql, new
            {
                CODE = reelCode
            });
            return result;
        }

        /// <summary>
        /// 获取辅料当前的作业
        /// </summary>
        /// <param name="reelCode"></param>
        /// <returns></returns>
        public async Task<SmtResourcesRuncardViewModel> GetResourceRuncardView(String reelCode)
        {
            string sql = @"SELECT DISTINCT SRR.ID,
                   SRR.RESOURCE_ID,
                   SRR.ROUTE_ID,
                   SAO.EN_DESC OBJECT_NAME,
                   SRR.RESOURCE_NO,
			             SRR.CURRENT_OPERATION,
                   SP1.VALUE CURRENT_ROUTE,
			             SRT.NEXT_OPERATION,
                   SP2.VALUE NEXT_ROUTE,
                   SRR.STATUS STATUS_CODE,
                   SRR.LOCATION,
                   SP3.CN_DESC STATUS,
                   SRR.BEGIN_OPERATION_TIME,
                   SRR.END_OPERATION_TIME,
                   SRR.EXPOSE_TIME,
                   SRR.OPERATOR,
                   SRU.STANDARD_TIME,
                   SRC.VALID_TIME,
                   SRC.EXPOSE_TIME STANDARD_EXPOSE_TIME,
                   SRU.STANDARD_FLAG,
                   SRU.VALID_FLAG,
                   SRU.EXPOSE_FLAG,
                   SRC.CATEGORY_ID,
                   SRC.CATEGORY_NAME,
                   SSB.BATCH_NO,
                   SSB.FRIDGE_LOC
              FROM       SMT_RESOURCE_RUNCARD SRR
                        LEFT JOIN
                            SMT_SOLDERPASTE_BATCHMAPPING SSB 
                        ON SSB.REEL_NO = SRR.RESOURCE_NO
                      LEFT JOIN
                         SMT_RESOURCE_RULES SRU
                      ON SRR.CATEGORY_ID = SRU.CATEGORY_ID
                         AND SRR.ROUTE_ID = SRU.ROUTE_OPERATION_ID
                   LEFT JOIN
                      SMT_RESOURCE_CATEGORY SRC
                   ON SRR.CATEGORY_ID = SRC.CATEGORY_ID,
                   SMT_RESOURCE_ROUTE SRT,
                   SMT_LOOKUP SP1,
                   SMT_LOOKUP SP2,
                   SMT_LOOKUP SAO,
                   SMT_LOOKUP SP3
             WHERE     SRR.RESOURCE_ID = SAO.CODE
                   AND SAO.TYPE = 'RESOURCE_OBJECT'
                   AND SAO.ENABLED = 'Y'
                   AND SRR.ROUTE_ID = SRT.ID
                   AND SRR.CURRENT_OPERATION = SRT.CURRENT_OPERATION
                   AND SRR.STATUS = SP3.CODE
                   AND SP3.TYPE = 'RESOURCE_STATUS'
                   AND SRR.CURRENT_OPERATION = SP1.CODE
                   AND SP1.TYPE = 'RESOURCE_ROUTE'
                   AND SRT.NEXT_OPERATION = SP2.CODE
                   AND SP2.TYPE = 'RESOURCE_ROUTE'
                   AND SRU.ENABLED = 'Y'
                   AND SRC.ENABLED = 'Y'
                   AND SRR.RESOURCE_NO = :RESOURCE_NO";

            var result = await _dbConnection.QueryFirstOrDefaultAsync<SmtResourcesRuncardViewModel>(sql, new
            {
                RESOURCE_NO = reelCode
            });

            return result;

        }
        /// <summary>
        /// 获取辅料制程
        /// </summary>
        /// <param name="resourceId"></param>
        /// <returns></returns>
        public async Task<IEnumerable<SmtResourceRouteOperationViewModel>> GetResourceRouteOperationView(decimal resourceId)
        {
            string sql = @"
              SELECT SRR.ID,
                   SRR.OBJECT_ID,
                   SRR.CURRENT_OPERATION,
                   SRR.NEXT_OPERATION,
                   SRR.ORDER_NO,
                   SP.VALUE MEANING,
                   SPT.VALUE NEXT_ROUTE,
                   SAO.CN_DESC OBJECT_NAME
              FROM SMT_RESOURCE_ROUTE SRR,
                   SMT_LOOKUP SP,
                   SMT_LOOKUP SAO,
                   SMT_LOOKUP SPT
             WHERE     SRR.CURRENT_OPERATION = SP.CODE
                   AND SP.TYPE = 'RESOURCE_ROUTE'
                   AND SRR.OBJECT_ID = SAO.CODE
                   AND SPT.TYPE = 'RESOURCE_ROUTE'
                   AND SAO.TYPE = 'RESOURCE_OBJECT'
                   AND SRR.NEXT_OPERATION = SPT.CODE
                   AND SRR.OBJECT_ID = :OBJECT_ID
                  ORDER BY SRR.ORDER_NO ASC ";

           var result = await  _dbConnection.QueryAsync<SmtResourceRouteOperationViewModel>(sql, new
            {
                OBJECT_ID = resourceId
            });
            return result;
        }
        

        /// <summary>
        /// 获取辅料历史的作业记录
        /// </summary>
        /// <param name="reelCode"></param>
        /// <returns></returns>
        public async Task<IEnumerable<SmtResourcesRuncardViewModel>> GetResourceRuncardLogView(String reelCode)
        {
            string sql = @"SELECT DISTINCT SRR.ID,
                   SRR.RESOURCE_ID,
                   SRR.ROUTE_ID,
                   SAO.EN_DESC OBJECT_NAME,
                   SRR.RESOURCE_NO,
			             SRR.CURRENT_OPERATION,
                   SP1.VALUE CURRENT_ROUTE,
			             SRT.NEXT_OPERATION,
                   SP2.VALUE NEXT_ROUTE,
                   SRR.STATUS STATUS_CODE,
                   SP3.CN_DESC STATUS,
                   SRR.BEGIN_OPERATION_TIME,
                   SRR.END_OPERATION_TIME,
                   SRR.EXPOSE_TIME,
                   SRR.OPERATOR,
                   SRU.STANDARD_TIME,
                   SRC.VALID_TIME,
                   SRC.EXPOSE_TIME STANDARD_EXPOSE_TIME,
                   SRU.STANDARD_FLAG,
                   SRU.VALID_FLAG,
                   SRU.EXPOSE_FLAG,
                   SRC.CATEGORY_ID,
                   SRC.CATEGORY_NAME,
                   SSB.BATCH_NO,
                   SSB.FRIDGE_LOC
              FROM    SMT_LOG_RESOURCE_RUNCARD SRR
                        LEFT JOIN
                            SMT_SOLDERPASTE_BATCHMAPPING SSB 
                        ON SSB.REEL_NO = SRR.RESOURCE_NO
                      LEFT JOIN
                         SMT_RESOURCE_RULES SRU
                      ON SRR.CATEGORY_ID = SRU.CATEGORY_ID
                         AND SRR.ROUTE_ID = SRU.ROUTE_OPERATION_ID
                   LEFT JOIN
                      SMT_RESOURCE_CATEGORY SRC
                   ON SRR.CATEGORY_ID = SRC.CATEGORY_ID,
                   SMT_RESOURCE_ROUTE SRT,
                   SMT_LOOKUP SP1,
                   SMT_LOOKUP SP2,
                   SMT_LOOKUP SAO,
                   SMT_LOOKUP SP3
             WHERE     SRR.RESOURCE_ID = SAO.CODE
                   AND SAO.TYPE = 'RESOURCE_OBJECT'
                   AND SAO.ENABLED = 'Y'
                   AND SRR.ROUTE_ID = SRT.ID
                   AND SRR.CURRENT_OPERATION = SRT.CURRENT_OPERATION
                   AND SRR.STATUS = SP3.CODE
                   AND SP3.TYPE = 'RESOURCE_STATUS'
                   AND SRR.CURRENT_OPERATION = SP1.CODE
                   AND SP1.TYPE = 'RESOURCE_ROUTE'
                   AND SRT.NEXT_OPERATION = SP2.CODE
                   AND SP2.TYPE = 'RESOURCE_ROUTE'
                   AND SRU.ENABLED = 'Y'
                   AND SRC.ENABLED = 'Y'
                   AND SRR.RESOURCE_NO = :RESOURCE_NO
                   ORDER BY SRR.BEGIN_OPERATION_TIME";

            var result = await _dbConnection.QueryAsync<SmtResourcesRuncardViewModel>(sql, new
            {
                RESOURCE_NO = reelCode
            });

            return result;

        }

        /// <summary>
        /// 通过条码CODE获取产品条码
        /// </summary>
        /// <param name="reelCode"></param>
        /// <returns></returns>
        public async Task<ReelInfoViewModel> GetReelInfoViewModel(String reelCode)
        {
            string sql = @"SELECT * FROM IMS_REEL_INFO_VIEW WHERE CODE = :CODE";
            var result = await _dbConnection.QueryFirstOrDefaultAsync<ReelInfoViewModel>(sql, new
            {
                CODE = reelCode
            });
            return result;
        }

        /// <summary>
        /// 新增作业记录
        /// </summary>
        /// <param name="resourceNo"></param>
        /// <param name="nextRouteName"></param>
        private void InsertResourceRuncard(string resourceNo, decimal resourceID,
           decimal categoryID, decimal routeID, decimal currentOperation,string user)
        {
            String I_InsertResourceRuncard = @"INSERT INTO SMT_RESOURCE_RUNCARD(ID,RESOURCE_NO,RESOURCE_ID,CATEGORY_ID,ROUTE_ID,
                                    CURRENT_OPERATION,STATUS,BEGIN_OPERATION_TIME,CURRENT_SITE,LOCATION,EXPOSE_TIME,OPERATOR)
                                    VALUES(:ID,:RESOURCE_NO,:RESOURCE_ID,:CATEGORY_ID,:ROUTE_ID,:CURRENT_OPERATION,
                                    :STATUS,:BEGIN_OPERATION_TIME,:CURRENT_SITE,:LOCATION,:EXPOSE_TIME,:OPERATOR) ";

            DateTime beginOperationTime =  this.GetCurrentTime().Result;

            Decimal resourceRuncardId = this.GetSeqByName("SMT_RESOURCE_RUNCARD_SEQ").Result;


           var result =  _dbConnection.Execute(I_InsertResourceRuncard, new
            {
                ID = resourceRuncardId,
                RESOURCE_NO = resourceNo,
                RESOURCE_ID = resourceID,
                CATEGORY_ID = categoryID,
                ROUTE_ID = routeID,
                CURRENT_OPERATION = currentOperation,
                STATUS = ResourceIceBox,
                BEGIN_OPERATION_TIME = beginOperationTime,
                CURRENT_SITE = 0,
                LOCATION  =0,
                EXPOSE_TIME = 0,
                OPERATOR = user
            });
        }
        /// <summary>
        /// 获取辅料类型数据
        /// </summary>
        /// <param name="partNo"></param>
        /// <returns></returns>
        public Task<SmtResourceCategory> GetResourceCategory(String partNo)
        {
            string sql = @"select * from SMT_RESOURCE_CATEGORY where CATEGORY_NAME = :PART_NO";
            var result = _dbConnection.QueryFirstOrDefaultAsync<SmtResourceCategory>(sql, new
            {
                PART_NO = partNo
            });
            return result;
        }
        /// <summary>
        /// 获取制程信息
        /// </summary>
        /// <param name="objectId"></param>
        /// <returns></returns>
        public Task<IEnumerable<SmtResourceRoute>> GetResourceRoute(decimal objectId)
        {
            string sql = @"select * from SMT_RESOURCE_ROUTE where OBJECT_ID = :OBJECT_ID order by ORDER_NO";
            var result = _dbConnection.QueryAsync<SmtResourceRoute>(sql, new
            {
                OBJECT_ID = objectId
            });
            return result;
        }
        /// <summary>
        /// 修改辅料的制程环节
        /// </summary>
        /// <param name="routeId"></param>
        /// <param name="currentOperationId"></param>
        /// <param name="resourceNo"></param>
        /// <returns></returns>
        public async Task<bool> UpdateResourceRuncard(decimal routeId, Decimal currentOperationId,
            String resourceNo, DateTime beginTime, DateTime endtime)
        {
            String sql = @"UPDATE SMT_RESOURCE_RUNCARD SET ROUTE_ID=:ROUTE_ID, CURRENT_OPERATION=:CURRENT_OPERATION,BEGIN_OPERATION_TIME = :BEGIN_OPERATION_TIME ,END_OPERATION_TIME = :END_OPERATION_TIME WHERE RESOURCE_NO = :RESOURCE_NO";
            decimal result = await _dbConnection.ExecuteAsync(sql, new
            {
                ROUTE_ID = routeId,
                CURRENT_OPERATION = currentOperationId,
                RESOURCE_NO = resourceNo,
                BEGIN_OPERATION_TIME = beginTime,
                END_OPERATION_TIME = endtime
            });
            if(result> 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        /// <summary>
        /// 获取辅料管控规则新增
        /// </summary>
        /// <param name="objectId"></param>
        /// <param name="categoryId"></param>
        /// <param name="routeOperationId"></param>
        /// <returns></returns>
        public Task<SmtResourceRules> GetResourceRule(decimal objectId, decimal categoryId, decimal routeOperationId)
        {
            string sql = @"select * from SMT_RESOURCE_RULES 
                where OBJECT_ID = :OBJECT_ID 
                AND CATEGORY_ID = :CATEGORY_ID 
                AND ROUTE_OPERATION_ID = :ROUTE_OPERATION_ID 
                AND ENABLED = 'Y'";
            var result = _dbConnection.QueryFirstOrDefaultAsync<SmtResourceRules>(sql, new
            {
                OBJECT_ID = objectId,
                CATEGORY_ID = categoryId,
                ROUTE_OPERATION_ID = routeOperationId
            });
            return result;

        }
        /// <summary>
        /// 处理辅料逻辑
        /// </summary>
        /// <param name="resourceNo"></param>
        /// <param name="nextRouteName"></param>
        /// <param name="partNumber"></param>
        public void ProcessResourceRuncard(string resourceNo, decimal nextOperationId, string user)
        {
            try
            {
                ReelInfoViewModel reelInfoView =  GetReelInfoViewModel(resourceNo).Result;
                if (reelInfoView == null)
                {
                    throw new Exception(String.Format("辅料条码数据{0}有误!", resourceNo));
                }

                String part_no = reelInfoView.PART_NO;

                SmtResourceCategory smtResourceCategory =  this.GetResourceCategory(part_no).Result;
                if (smtResourceCategory == null)
                {
                    throw new Exception(String.Format("辅料料号{0}没有进行辅料类型维护!", part_no));
                }

                List<SmtResourceRoute> smtResourceRoutes = this.GetResourceRoute((decimal)smtResourceCategory.OBJECT_ID).Result.AsList<SmtResourceRoute>();

                if (smtResourceRoutes == null)
                {
                    throw new Exception("该辅料未进行制程配置！");
                }

                SmtResourcesRuncardViewModel resourceRuncardTable =  this.GetResourceRuncardView(resourceNo).Result;

                //不存在就insert resource
                if (resourceRuncardTable == null)
                {
                    ///冷藏冰箱下一工序位-999默认
                    if(nextOperationId == -999)
                    {
                        nextOperationId = (decimal)smtResourceRoutes[0].CURRENT_OPERATION;
                    }
                    if (nextOperationId != smtResourceRoutes[0].CURRENT_OPERATION)
                    {
                        throw new Exception("选择的不是第一道作业工序！");
                    }
                    InsertResourceRuncard(resourceNo, (decimal)smtResourceCategory.OBJECT_ID,
                        (decimal)smtResourceCategory.CATEGORY_ID, smtResourceRoutes[0].ID, nextOperationId, user);
                    return;
                }

                //判斷狀態
                if (resourceRuncardTable.STATUS_CODE == ResourceUseup
                        || resourceRuncardTable.STATUS_CODE == ResourceDisused)
                {
                    throw new Exception("辅料状态已经报废或用完！");
                }

                //判斷下一道工序是否正確
                Decimal? nextOperationID = resourceRuncardTable.NEXT_OPERATION;
                if (nextOperationID == null)
                {
                    throw new Exception("辅料已经刷完所有动作，无需再进一步作业！");
                }
                if (nextOperationId != nextOperationID&& nextOperationId != 6)
                {
                    throw new Exception(string.Format("辅料下一道工序应该为{0}", resourceRuncardTable.NEXT_ROUTE));
                }
                decimal objceId = (decimal)resourceRuncardTable.RESOURCE_ID;
                decimal categoryId = (decimal)resourceRuncardTable.CATEGORY_ID;
                decimal routeId = (decimal)resourceRuncardTable.ROUTE_ID;
                string fridgeLoc = resourceRuncardTable.FRIDGE_LOC;
                DateTime operationTime = (DateTime)resourceRuncardTable.BEGIN_OPERATION_TIME;
                decimal status = (decimal)resourceRuncardTable.STATUS_CODE;
                decimal recordExposeTime = (decimal)resourceRuncardTable.EXPOSE_TIME;
                //檢查FIFO
                this.ConfirmResourceFIFO(resourceNo, (decimal)resourceRuncardTable.CURRENT_OPERATION, objceId
                    , categoryId, status, fridgeLoc);

                //檢查時間標準
                this.ConfirmResourceTimeRule(resourceNo, objceId, categoryId, routeId, operationTime);
                DateTime madeTime = this.GetResourceMadeTime(reelInfoView.DATE_CODE);
                //檢查資源是否超過有效期
                if (!this.ConfirmResourceValid(resourceNo, objceId, categoryId,
                    (decimal)smtResourceCategory.EXPOSE_TIME, (decimal)smtResourceCategory.VALID_TIME, 
                    madeTime
                    ))
                {
                    throw new Exception(string.Format("{0}已经超出使用期限，自动报废", resourceNo));
                }

                //錫膏開封時檢查攪拌是否已超24小時
                //if (objceId == 1 &&
                //    this.nextOperation == GlobalVariables.ResourceOpen)
                //{
                //    if (!this.ConfirmResourceStirTimeOver24Hours(resourceNo))
                //    {
                //        Messager(string.Format("锡膏条码{0} 搅拌超24小时，需重新搅拌再使用！", resourceNo));

                //        // 攪拌超過24小時，需重新攪拌，下一流程自動改為攪拌
                //        this.nextOperation = GlobalVariables.ResourceStir;
                //        this.routeID = ResourceMaintainManager.GetResourceRouteDataTable(
                //            new KeyValuePair<string, object>("CURRENT_OPERATION", this.nextOperation),
                //            new KeyValuePair<string, object>("OBJECT_ID", this.resourceID)).FirstOrDefault().ID;
                //    }
                //}

                //檢查暴露時間
                this.ConfirmResourceExposeTime(resourceNo, objceId, categoryId,status,
                    operationTime, recordExposeTime, (Decimal)smtResourceCategory.EXPOSE_TIME);
                if (nextOperationId == 6)
                {
                    //重新放入冰箱

                }
                //更新
                this.UpdateResourceRuncard(resourceNo, objceId, categoryId, user, nextOperationId);
                return;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            
        }

        private void UpdateResourceRuncard(string resourceNo, decimal objectID, decimal categoryID, string user,decimal nextOperationId)
        {

            
            UpdateResourceRuncardEndTime(resourceNo);
            InsertLogResourceRuncard(resourceNo);

            //this.GetResourceRuncardTable(new KeyValuePair<string, object>("RESOURCE_NO", resourceNo));
            SmtResourcesRuncardViewModel resourceRuncardTable =  this.GetResourceRuncardView(resourceNo).Result;

            List<SmtResourceRoute> smtResourceRoutes = this.GetResourceRoute(objectID).Result.AsList();

            

            DateTime beginTime = (DateTime)resourceRuncardTable.BEGIN_OPERATION_TIME;
            DateTime endTime = (DateTime)resourceRuncardTable.END_OPERATION_TIME;
            Decimal status = (Decimal)resourceRuncardTable.STATUS_CODE;
            //decimal nextOperation = (Decimal)resourceRuncardTable.NEXT_OPERATION;
            SmtResourceRoute nextSmtResourceRoute = smtResourceRoutes.Where(f => f.CURRENT_OPERATION == nextOperationId).FirstOrDefault();
            if(nextSmtResourceRoute == null && nextOperationId == ResourceUsedOver) {
                this.SetResourceRuncardStatus(resourceNo, ResourceUseup);
                return;
            }
            if(nextSmtResourceRoute == null)
            {
                throw new Exception(String.Format("辅料：{0} 制程配置错误!",resourceNo));
            }
            decimal routeId = nextSmtResourceRoute.ID;
            decimal eTime = 0;

            if (this.exposeFlag == "Y" && !status.Equals(ResourceIceBox))
            {
                eTime = (decimal)((endTime - beginTime).TotalDays * 1440);
            }

            decimal station =ResourceUsed;
            if (nextOperationId.Equals(1) || nextOperationId.Equals(6))
            {
                station = 5;
            }
            else if (nextOperationId.Equals(2) || nextOperationId.Equals(15)
                || nextOperationId.Equals(14))
            {
                station = 6;
            }
            else if (nextOperationId.Equals(3) || nextOperationId.Equals(16))
            {
                station = 7;
            }
            decimal location;
            List<decimal> locationList = SelectSoldPasteLoction(resourceNo);
            if (locationList != null && locationList.Count > 0)
            {
                location = locationList[0];
            }
            else
            {
                throw new Exception("存入冰箱的辅料未选择冰箱储位！");
            }

            UpdateResourceRuncardTable(resourceNo, routeId,
                nextOperationId, station, 0, location
                , eTime, user);

            if (nextOperationId == ResourceUsedOver)
            {
                this.SetResourceRuncardStatus(resourceNo, ResourceUseup);
            }
        }
        /// <summary>
        /// 改变辅料状态
        /// </summary>
        /// <param name="resourceNo"></param>
        /// <param name="status"></param>
        public  void SetResourceRuncardStatus(string resourceNo, decimal status)
        {
           
            this.UpdateResourceRuncardStatus(resourceNo, status);
            
            //if(status == 3)
            //{
            //    //将下一步改为报废
            //}
            if (status != ResourceUsed
                && status != ResourceIceBox
                && status != ResourceWarm
                && status != ResourceWhip)
            {
                this.UpdateResourceUnuseLog(resourceNo, ManualUpdate);
                this.UpdateResourceRuncardEndTime(resourceNo);
                this.InsertLogResourceRuncard(resourceNo);
            }
        }

        private bool UpdateResourceRuncardTable(string resourceNo, decimal routeID,
           decimal currentOperation, decimal status,
           decimal currentSite, decimal location, decimal exposeTime, string user)
        {
             string sql = @"UPDATE SMT_RESOURCE_RUNCARD SET ROUTE_ID=:ROUTE_ID, CURRENT_OPERATION=:CURRENT_OPERATION, OPERATOR=:OPERATOR,
                                    STATUS=:STATUS, CURRENT_SITE=:CURRENT_SITE, LOCATION=:LOCATION, EXPOSE_TIME=:EXPOSE_TIME,
                                    BEGIN_OPERATION_TIME=SYSDATE, END_OPERATION_TIME= NULL WHERE RESOURCE_NO=:RESOURCE_NO ";
            var result = _dbConnection.ExecuteScalarAsync<decimal>(sql, new
            {
                RESOURCE_NO = resourceNo,
                ROUTE_ID = routeID,
                CURRENT_OPERATION = currentOperation,
                OPERATOR = user,
                STATUS = status,
                CURRENT_SITE = currentSite,
                LOCATION = location,
                EXPOSE_TIME = exposeTime
            });
            if(result.Result > 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        /// <summary>
		/// 获取批量冷藏中的冰箱储位
		/// </summary>
		/// <param name="reel_no"></param>
		/// <returns></returns>
		private List<decimal>  SelectSoldPasteLoction(string reel_no)
        {
            string sql = @"select SL.CODE from SMT_SOLDERPASTE_BATCHMAPPING SSB , SMT_LOOKUP SL where SSB.FRIDGE_LOC = SL.VALUE  AND SSB.REEL_NO = :REEL_NO";
            List<decimal> result = _dbConnection.QueryAsync<decimal>(sql, new
            {
                REEL_NO = reel_no
            }).Result.AsList();

            return result;
        }

        private decimal ConfirmResourceExposeTime(string resourceNo, 
            decimal objectID, decimal categoryID , 
            decimal status, DateTime beginOperationTime,
            decimal recordExposeTime, decimal exposeTime)
        {

            if (this.exposeFlag == "Y" && status != 5)
            {
                DateTime now =  this.GetCurrentTime().Result;

                TimeSpan inteval = now - beginOperationTime;

                if ((recordExposeTime + (decimal)(inteval.TotalDays * 1440)) >= (exposeTime * 60))
                {
                    this.UpdateResourceRuncardStatus(resourceNo, ResourceDisused);
                    this.UpdateResourceRuncardEndTime(resourceNo);
                    this.UpdateResourceUnuseLog(resourceNo, AutoUpdate);
                    this.InsertLogResourceRuncard(resourceNo);
                    throw new Exception(string.Format("{0}暴露超时，系统自动报废", resourceNo));
                }
                // 剩余时间（分钟）
                decimal valiTime = (exposeTime * 60) - (recordExposeTime + (decimal)(inteval.TotalDays * 1440));
                return valiTime;

            }
            else
            {
                return 0;
            }
        }

        private bool ConfirmResourceValid(string resouceNo, decimal objectID, 
            decimal categoryID, decimal exposeTime, decimal validTime, DateTime madeDate)
        {

            if (this.validFlag == "Y")
            {
                DateTime now = this.GetCurrentTime().Result;
                DateTime lastTime = madeDate.AddDays((double)validTime);
                if (now > lastTime)
                {
                    this.UpdateResourceRuncardStatus(resouceNo, ResourceDisused);
                    this.UpdateResourceRuncardEndTime(resouceNo);
                    this.UpdateResourceUnuseLog(resouceNo, AutoUpdate);
                    this.InsertLogResourceRuncard(resouceNo);
                    return false;
                }
                else
                {
                    return true;
                }
            }
            else
            {
                return true;
            }
        }
        
        private bool  InsertLogResourceRuncard(string resourceNo)
        {
            string sql = @"INSERT INTO SMT_LOG_RESOURCE_RUNCARD SELECT * FROM SMT_RESOURCE_RUNCARD WHERE RESOURCE_NO=:RESOURCE_NO ";
            decimal result = _dbConnection.ExecuteScalar<decimal>(sql, new
            {
                RESOURCE_NO = resourceNo
            });
            if (result > 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        private bool UpdateResourceUnuseLog(string resourceNo, string desc)
        {
            string sql = "UPDATE SMT_RESOURCE_RUNCARD SET ATTRIBUTE1=:ATTRIBUTE1 WHERE RESOURCE_NO=:RESOURCE_NO ";
            decimal result = _dbConnection.ExecuteScalar<decimal>(sql, new
            {
                RESOURCE_NO = resourceNo,
                ATTRIBUTE1 = desc
            });
            if (result > 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        private  bool UpdateResourceRuncardEndTime(string resourceNo)
        {
            string sql = @"UPDATE SMT_RESOURCE_RUNCARD SET END_OPERATION_TIME=SYSDATE WHERE RESOURCE_NO=:RESOURCE_NO ";
            decimal result = _dbConnection.ExecuteScalar<decimal>(sql, new
            {
                RESOURCE_NO = resourceNo
            });
            if (result > 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        private  bool UpdateResourceRuncardStatus(string resourceNo, decimal status)
        {
            String sql = @"UPDATE SMT_RESOURCE_RUNCARD SET STATUS=:STATUS WHERE RESOURCE_NO=:RESOURCE_NO ";
           decimal result =  _dbConnection.ExecuteScalar<decimal>(sql, new
            {
                STATUS = status,
                RESOURCE_NO = resourceNo
            });
            if(result > 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        private DateTime GetResourceMadeTime(string dateCode)
        {
            if (dateCode.Length != 8) return DateTime.Now;

            string year = dateCode.Substring(0, 4);
            string month = dateCode.Substring(4, 2);
            string day = dateCode.Substring(6, 2); ;
            return DateTime.Parse((year + "-" + month + "-" + day));
        }

        private void ConfirmResourceTimeRule(string resourceNo, decimal objectID, 
            decimal categoryID, decimal routeId, DateTime beginOperationTime)
        {
            SmtResourceRules smtResourceRules =  GetResourceRule(objectID, categoryID, routeId).Result;

            if (smtResourceRules == null)
            {
                throw new Exception(string.Format("{0}找不到该辅料在当前工序上时间规则配置，请检查", resourceNo));
            }

            this.standardFlag = smtResourceRules.STANDARD_FLAG;
            this.validFlag = smtResourceRules.VALID_FLAG;
            this.exposeFlag = smtResourceRules.EXPOSE_FLAG;
            this.standardTime = (decimal)smtResourceRules.STANDARD_TIME;

            if (this.standardFlag == "Y")
            {
                DateTime now =  this.GetCurrentTime().Result;

                DateTime lastTime = beginOperationTime.AddMinutes((double)standardTime);

                //當前時間大於開始時間加上標準時間
                if (now <= lastTime)
                {
                    double lessMinus = (beginOperationTime.AddMinutes((double)standardTime) - now).TotalMinutes;
                    throw new Exception(
                        string.Format("{0}当前流程未结束，还需要等待{1}分钟，不能进行下一作业!", resourceNo, Math.Round(lessMinus, 2)));
                }
            }
        }

        private void ConfirmResourceFIFO(string resourceNo, decimal currentOperation,
           decimal objectID, decimal categoryID, decimal currentStatus, string firLoc)
        {
            if (currentStatus == 5)
            {
              //仓库条码先进先出规则
              this.CheckBatch(resourceNo, objectID, categoryID, firLoc);

            }
        }

        private void CheckBatch(string resourceNo, decimal objectID, decimal categoryID, string firLoc)
        {
            List<SmtSolderpasteBatchmapping> currentSmtSolderPastBatchmappingList = _dbConnection.GetList<SmtSolderpasteBatchmapping>("where REEL_NO = :REEL_NO", new
            {
                REEL_NO = resourceNo
            }).AsList();
            string sql = @"select SSB.* from 
                SMT_SOLDERPASTE_BATCHMAPPING SSB , 
                SMT_RESOURCE_RUNCARD SRR
                where SSB.REEL_NO = SRR.RESOURCE_NO 
                AND SRR.STATUS = 5 
                AND SRR.CATEGORY_ID = :CATEGORY_ID
                AND SRR.RESOURCE_ID = :RESOURCE_ID
                AND FRIDGE_LOC = :FRIDGE_LOC order by BATCH_NO ASC";
            SmtSolderpasteBatchmapping fistSmtSolderPastBatchmapping = 
                _dbConnection.QueryFirstOrDefaultAsync<SmtSolderpasteBatchmapping>(sql, new
                {
                    CATEGORY_ID = categoryID,
                    RESOURCE_ID = objectID,
                    FRIDGE_LOC = firLoc
                }).Result;
            if (currentSmtSolderPastBatchmappingList == null || currentSmtSolderPastBatchmappingList.Count <= 0
                || fistSmtSolderPastBatchmapping ==null)
            {
                throw new Exception("当前辅料未进行批量冷藏!");
            }
            String currentBatch = currentSmtSolderPastBatchmappingList[0].BATCH_NO;
            String firstBatch = fistSmtSolderPastBatchmapping.BATCH_NO;
            if (!currentBatch.Equals(firstBatch))
            {
                throw new Exception(string.Format("按FIFO先进先出规则,应该要先用批次号为：{0} 的辅料！", firstBatch
                            ));
            }

        }

        /// <summary>
        /// 根据主键获取激活状态
        /// </summary>
        /// <param name="id">主键</param>
        /// <returns></returns>
		public async Task<Boolean> GetEnableStatus(decimal id)
		{
			string sql = "SELECT ENABLED FROM SMT_SOLDERPASTE_BATCHMAPPING WHERE ID=:ID";
			var result = await _dbConnection.QueryFirstOrDefaultAsync<string>(sql, new
			{
				ID = id
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
			string sql = "UPDATE SMT_SOLDERPASTE_BATCHMAPPING set ENABLED=:ENABLED WHERE ID=:Id";
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
			string sql = "SELECT SMT_SOLDERPASTE_BATCHMAPPING_SEQ.NEXTVAL MY_SEQ FROM DUAL";
			var result = await _dbConnection.ExecuteScalarAsync(sql);
			return (decimal)result;
		}

        // <summary>
        /// 获取表的序列
        /// </summary>
        /// <returns></returns>
		public async Task<decimal> GetSeqByName(String name)
        {
            string sql = string.Format("SELECT {0}.NEXTVAL MY_SEQ FROM DUAL", name);
            var result = await _dbConnection.ExecuteScalarAsync(sql);
            return (decimal)result;
        }

        public void CheckResourcesQty(string resourceNo, decimal nextOperationId)
        {
            decimal location;
            List<decimal> locationList = SelectSoldPasteLoction(resourceNo);
            if (locationList != null && locationList.Count > 0)
            {
                location = locationList[0];
            }
            else
            {
                throw new Exception("LOCATION_NULL");
            }

            if (nextOperationId.Equals(1) || nextOperationId.Equals(6) || nextOperationId.Equals(2) || nextOperationId.Equals(15) || nextOperationId.Equals(14) || nextOperationId.Equals(3) || nextOperationId.Equals(16))
            {
            }
            else
            {
                //String sQuery = "SELECT COUNT(1) FROM SMT_RESOURCE_RUNCARD WHERE CATEGORY_ID IN (SELECT CATEGORY_ID FROM SMT_RESOURCE_RUNCARD WHERE RESOURCE_NO= :RESOURCE_NO) AND STATUS = 1  AND LOCATION = :LOCATION AND RESOURCE_NO != :RESOURCE_NO";
                //decimal result = _dbConnection.QueryFirstOrDefault<decimal>(sQuery, new { RESOURCE_NO = resourceNo, LOCATION = location });
                //if (result > 0)
                //{
                //    throw new Exception("RESOURCES_QTY_ERROR");
                //}
            }
        }
    }
}