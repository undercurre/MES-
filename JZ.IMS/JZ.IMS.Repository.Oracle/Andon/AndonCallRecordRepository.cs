/**
*┌──────────────────────────────────────────────────────────────┐
*│　描    述：呼叫记录接口实现                                                    
*│　作    者：嘉志科技                                            
*│　版    本：2.0    模板代码自动生成                                                
*│　创建时间：2019-09-22 23:39:21                             
*└──────────────────────────────────────────────────────────────┘
*┌──────────────────────────────────────────────────────────────┐
*│　命名空间： JZ.IMS.Repository.Oracle                                  
*│　类    名： AndonCallRecordRepository                                      
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
using System.Data.Common;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.IO;
using System.Text;
using System.Data;
using JZ.IMS.ViewModels;
using JZ.IMS.Core.Extensions;

namespace JZ.IMS.Repository.Oracle
{
    public class AndonCallRecordRepository : BaseRepository<AndonCallRecord, Decimal>, IAndonCallRecordRepository
    {
        public AndonCallRecordRepository(IOptionsSnapshot<DbOption> options)
        {
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
            string sql = "SELECT ENABLED FROM ANDON_CALL_RECORD WHERE ID=:ID AND IS_DELETE='N'";
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
            string sql = "update ANDON_CALL_RECORD set ENABLED=:ENABLED where  Id=:Id";
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
            string sql = "SELECT ANDON_CALL_RECORD_SEQ.NEXTVAL MY_SEQ FROM DUAL";
            var result = await _dbConnection.ExecuteScalarAsync(sql);
            return (decimal)result;
        }

        public static String I_InsertCallNotice = @"INSERT INTO ANDON_CALL_NOTICE(ID, MST_ID,NOTICE_TYPE,NOTICE_CONTENT,NOTICT_TIME,STATUS)
			VALUES(:ID, :MST_ID,'wechat', :NOTICE_CONTENT, sysdate,0)";
        public static String S_SelectSysManager = @"select SM.*  FROM  ANDON_CALL_CONFIG  ANCC,  ANDON_CALL_PERSON_CONFIG ACPC, SYS_MANAGER SM
			where ANCC.ID = ACPC.MST_ID AND ACPC.USER_ID = SM.ID AND ANCC.OPERATION_LINE_ID = :OPERATION_LINE_ID  
			AND ANCC.CALL_TYPE_CODE = :CALL_TYPE_CODE AND ANCC.OPERATION_SITE_ID = :OPERATION_SITE_ID ";

        public static String I_InsertNoticeReceiver = @"INSERT INTO ANDON_CALL_NOTICE_RECEIVER (ID,MST_ID,USER_ID,NOTICE_ACCOUNT,NOTICE_TYPE,USER_NAME)
			VALUES(:ID, :MST_ID, :USER_ID, :NOTICE_ACCOUNT, 'wechat', :USER_NAME)";
        public static String I_InsertCallData = @"INSERT INTO ANDON_CALL_DATA(ID, CALL_NO,CALL_CONTENT_ID,LINE_ID ,LINE_NAME ,OPERATION_ID,OPERATION_NAME ,OPERATION_SITE_ID ,  OPERATION_SITE_NAME ,OPERATOR,CREATE_TIME ,STATUS ) 
                                                VALUES (ANDON_CALL_DATA_SEQ.NEXTVAL,:CALL_NO,:CALL_CONTENT_ID,:LINE_ID ,:LINE_NAME ,:OPERATION_ID,:OPERATION_NAME ,:OPERATION_SITE_ID ,:OPERATION_SITE_NAME ,:OPERATOR,SYSDATE ,0)";


        public async Task<AndonCallRecord> AddCallRecord(Decimal operationLineId, String operationLineName,
            Decimal operationId, String operationName, Decimal operationSiteId, String operationSiteName,
            String user, String andonTye, String callCode, String callContent, Decimal? callContentId = null)
        {
            decimal callRecordId = await GetSeqByName("ANDON_CALL_RECORD_SEQ");
            decimal callNoticeId = await GetSeqByName("ANDON_CALL_NOTICE_SEQ");
            DateTime currentTime = (DateTime)await _dbConnection.ExecuteScalarAsync("select SYSDATE  FROM DUAL");
            String callNo = andonTye + callCode + currentTime.ToString("yyyyMMddHHmmss");
            ConnectionFactory.OpenConnection(_dbConnection);
            using (IDbTransaction transaction = _dbConnection.BeginTransaction())
            {
                try
                {
                    var replaceCallContent = callContent.Replace("{{LINE_NAME}}", operationLineName).Replace("{{OPERATION_SITE_NAME}}", operationSiteName);
                    var callRecord = new AndonCallRecord()
                    {
                        Id = callRecordId,
                        CALL_NO = callNo,
                        OPERATION_LINE_ID = operationLineId,
                        OPERATION_LINE_NAME = operationLineName,
                        OPERATION_ID = operationId,
                        OPERATION_NAME = operationName,
                        OPERATION_SITE_ID = operationSiteId,
                        OPERATION_SITE_NAME = operationSiteName,
                        OPERATOR = user,
                        CREATE_TIME = DateTime.Now,
                        CALL_TYPE_CODE = andonTye,
                        CALL_CONTENT = replaceCallContent,
                        STATUS = 0,
                        CALL_CODE = callCode,
                        NOTICE_COUNT = 0
                    };
                    this.Insert(callRecord);

                    #region 旧逻辑 直接推送消息
                    //await _dbConnection.ExecuteAsync(I_InsertCallNotice, new
                    //{
                    //    ID = callNoticeId,
                    //    MST_ID = callRecordId,
                    //    NOTICE_CONTENT = callContent
                    //});
                    //var managerModels = _dbConnection.Query<Sys_Manager>(S_SelectSysManager, new
                    //{
                    //    OPERATION_LINE_ID = operationLineId,
                    //    CALL_TYPE_CODE = andonTye,
                    //    OPERATION_SITE_ID = operationId
                    //});
                    //if (managerModels == null || managerModels.Count() <= 0)
                    //{
                    //    managerModels = _dbConnection.Query<Sys_Manager>(S_SelectSysManager, new
                    //    {
                    //        OPERATION_LINE_ID = operationLineId,
                    //        CALL_TYPE_CODE = andonTye,
                    //        OPERATION_SITE_ID = 0
                    //    });
                    //}
                    //if (managerModels != null && managerModels.Count() > 0)
                    //{
                    //    foreach (Sys_Manager sys_Manager in managerModels)
                    //    {
                    //        decimal noticeReiverId = await GetSeqByName("ANDON_CALL_NOTICE_RECEIVER_SEQ");
                    //        var mobile = sys_Manager.MOBILE.IsNullOrWhiteSpace() ? 0 : Convert.ToDecimal(sys_Manager.MOBILE);
                    //        await _dbConnection.ExecuteAsync(I_InsertNoticeReceiver, new
                    //        {
                    //            ID = noticeReiverId,
                    //            MST_ID = callNoticeId,
                    //            USER_ID = sys_Manager.ID,
                    //            NOTICE_ACCOUNT = mobile,
                    //            USER_NAME = sys_Manager.USER_NAME
                    //        });
                    //    }
                    //} 
                    #endregion

                    #region 添加ANDON_CALL_DATA数据
                    if (callContentId!=null&&callContentId > 0)
                    {
                        await _dbConnection.ExecuteAsync(I_InsertCallData, new
                        {
                            CALL_NO = callNo,
                            CALL_CONTENT_ID = callContentId,
                            LINE_ID = operationLineId,
                            LINE_NAME = operationLineName,
                            OPERATION_ID = operationId,
                            OPERATION_SITE_ID = operationSiteId,
                            OPERATION_SITE_NAME = operationSiteName,
                            OPERATION_NAME = operationName,
                            OPERATOR = user
                        });
                    }
                    
                    #endregion

                    transaction.Commit();
                    //try
                    //{
                    //    #if DEBUG
                    //    #else
                    //         this.SendMessageToWechat();
                    //    #endif
                    //}
                    //catch
                    //{

                    //}
                    return callRecord;
                }
                catch (Exception ex)
                {
                    transaction.Rollback();
                    throw ex;
                }
                finally
                {
                    if (_dbConnection.State != ConnectionState.Closed)
                    {
                        _dbConnection.Close();
                    }
                }
            }

        }

        public String parametersSql = @"SELECT SP.MEANING FROM SFCS_PARAMETERS SP where LOOKUP_TYPE = :LOOKUP_TYPE AND ENABLED = :ENABLED";

        private void SendMessageToWechat()
        {
            //呼叫内容不为空调用呼企业微信呼叫

            StreamReader sr = null;
            HttpWebResponse response = null;
            try
            {
                var para = _dbConnection.Query<String>(parametersSql, new
                {
                    LOOKUP_TYPE = "ANDON_URL",
                    ENABLED = "Y"
                });
                if (para == null || para.Count() <= 0)
                {
                    new Exception("ANDON呼叫企业微信地址");
                }
                String postUrl = para.FirstOrDefault();
                HttpWebRequest webReq = (HttpWebRequest)WebRequest.Create(new Uri(postUrl));
                webReq.Method = "POST";
                webReq.ContentType = "application/x-www-form-urlencoded";
                webReq.ContentLength = 0;
                response = (HttpWebResponse)webReq.GetResponse();
                sr = new StreamReader(response.GetResponseStream(), Encoding.Default);
                String result = sr.ReadToEnd();
                if (!result.Trim().Equals("true"))
                {
                    throw new Exception("企业微信通知出现异常! 异常详细：" + result);
                }

            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                if (sr != null)
                {
                    sr.Close();
                }
                if (response != null)
                {
                    response.Close();
                }

            }
        }


        public async Task<decimal> GetSeqByName(String seqName)
        {
            String sql = String.Format("SELECT {0}.NEXTVAL  FROM DUAL", seqName);
            var result = await _dbConnection.ExecuteScalarAsync(sql);
            return (decimal)result;
        }


        /// <summary>
        /// 查询数据(呼叫页面展示使用)
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>

        public async Task<TableDataModel> GetCallRecordData(AndonCallRecordCallPageRequestModel model)
        {
            string conditions = " where  a.ID > 0 ";

            if (!model.STATUS.IsNullOrWhiteSpace())
            {
                conditions += $"and (a.STATUS =:STATUS)";
            }
            if (!model.LINE_ID.IsNullOrWhiteSpace())
            {
                conditions += $"and (a.OPERATION_LINE_ID =:LINE_ID)";
            }
            if (!model.STARTDATE.IsNullOrWhiteSpace())
            {
                conditions += string.Format(@"and (a.CREATE_TIME >= to_date('{0}','yyyy-MM-dd HH24:mi:ss'))", model.STARTDATE.ToString());
            }
            if (!model.ENDDATE.IsNullOrWhiteSpace())
            {
                conditions += string.Format(@"and (a.CREATE_TIME < to_date('{0}','yyyy-MM-dd HH24:mi:ss'))", model.ENDDATE.ToString());
            }
            if (!model.CALL_TITLE.IsNullOrWhiteSpace())
            {
                conditions += $"and (a.CALL_TITLE =:CALL_TITLE)";
            }
            if (!model.Key.IsNullOrWhiteSpace())
            {
                conditions += $"and (instr(b.CALL_NO, :Key) > 0 or instr(d.OPERATION_SITE_NAME, :Key) > 0)";
            }
            string sql = @" select ROWNUM as rowno,  case when f.ID>0 then  f.OPERATION_LINE_NAME 
                                     when e.ID>0 then  e.LINE_NAME 
                                     else ''  end as LINE_NAME,d.OPERATION_SITE_NAME as SITE_NAME,h.ID as HANDLE_ID , a.* from ANDON_CALL_RECORD a
                             left join ANDON_CALL_Data b on a.CALL_NO=b.CALL_NO
                             left join SFCS_OPERATIONS c on b.OPERATION_ID=c.ID
                             left join SFCS_OPERATION_SITES d on c.ID=d.OPERATION_ID
                             left join SMT_LINES e on a.OPERATION_LINE_ID=e.ID
                             left join SFCS_OPERATION_LINES f on a.OPERATION_LINE_ID=f.ID 
                             left join ANDON_CALL_RECORD_HANDLE h on a.ID=h.MST_ID";

            string pagedSql = SQLBuilderClass.GetPagedSQL(sql, " a.id desc", conditions);
            var resdata = await _dbConnection.QueryAsync<object>(pagedSql, model);

            string sqlcnt = @"select count(0) from ANDON_CALL_RECORD a
                             inner join ANDON_CALL_Data b on a.CALL_NO=b.CALL_NO
                             left join SFCS_OPERATIONS c on b.OPERATION_ID=c.ID
                             left join SFCS_OPERATION_SITES d on c.ID=d.OPERATION_ID
                             left join SMT_LINES e on a.OPERATION_LINE_ID=e.ID
                             left join SFCS_OPERATION_LINES f on a.OPERATION_LINE_ID=f.ID 
                             left join ANDON_CALL_RECORD_HANDLE h on a.ID=h.MST_ID " + conditions;
            int cnt = await _dbConnection.ExecuteScalarAsync<int>(sqlcnt, model);

            return new TableDataModel
            {
                count = cnt,
                data = resdata?.ToList(),
            };
        }
        /// <summary>
        /// 查询数据(编辑页面展示使用)
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>

        public async Task<List<AndonCallRecordExtendListModel>> GetCallRecordToEditData(AndonCallRecordCallPageRequestModel model)
        {
            string conditions = "WHERE a.ID > 0 ";

            if (!model.ID.IsNullOrWhiteSpace())
            {
                conditions += $"and (a.ID=:ID)";
            }
            string sql = @" select ROWNUM as rowno,  case when f.ID>0 then  f.OPERATION_LINE_NAME 
                                     when e.ID>0 then  e.LINE_NAME 
                                     else ''  end as LINE_NAME,b.WO_NO,b.PART_NO,g.DESCRIPTION as Part_Size,h.TO_USER,h.SOLUTION,
                                    h.HANDLE_STATUS,h.HANDLE_CONTENT,h.ID as HANDLE_ID , a.* from ANDON_CALL_RECORD a
                             inner join ANDON_CALL_Data b on a.CALL_NO=b.CALL_NO
                             left join SMT_LINES e on a.OPERATION_LINE_ID=e.ID
                             left join SFCS_OPERATION_LINES f on a.OPERATION_LINE_ID=f.ID
                             left join IMS_PART g on b.PART_NO=g.CODE
                             left join ANDON_CALL_RECORD_HANDLE h on a.ID=h.MST_ID ";

            string pagedSql = SQLBuilderClass.GetPagedSQL(sql, " a.id desc", conditions);
            var resdata = await _dbConnection.QueryAsync<AndonCallRecordExtendListModel>(pagedSql, model);

            return resdata.ToList();
        }



        /// <summary>
		/// 查询首页预警信息数据
		/// </summary>
		/// <param name="model"></param>
		/// <returns></returns>
		public async Task<TableDataModel> GetEarlyWarningData(AndonCallRecordHomePageRequestModel model)
        {
            string conditions = "and  a.ID > 0 ";
            if (!model.User_ID.IsNullOrWhiteSpace())
            {
                conditions += $" and (c.MANAGER_ID=:User_ID)";
            }
            if (model.ID > 0)
            {
                conditions += $" and a.ID=:ID";
            }
            if (model.SITE_ID > 0)
            {
                conditions += $" and a.OPERATION_SITE_ID=:SITE_ID";
            }
            string sql = string.Format(@"select * from (
                                            select ROWNUM as rowno, temp.* from (
                                                select   a.*,REPLACE(REPLACE(REPLACE(a.CALL_CONTENT, '{{LINE_NAME}}', d.LINE_NAME),'{{OPERATION_SITE_NAME}}',d.OPERATION_SITE_NAME),'{{OPERATION_NAME}}',d.OPERATION_NAME) NEW_CONTENT from ANDON_CALL_RECORD a
                                                    left join SMT_LINES b on a.OPERATION_LINE_ID=b.ID
                                                    left join SYS_USER_ORGANIZE c on b.ORGANIZE_ID=c.ORGANIZE_ID
                                                    left join ANDON_CALL_DATA d on d.CALL_CONTENT_ID=a.id
													where 1=1 and a.STATUS=0 {0}
                                                union all
                                                select   a.*,REPLACE(REPLACE(REPLACE(a.CALL_CONTENT, '{{LINE_NAME}}', d.LINE_NAME),'{{OPERATION_SITE_NAME}}',d.OPERATION_SITE_NAME),'{{OPERATION_NAME}}',d.OPERATION_NAME) NEW_CONTENT from ANDON_CALL_RECORD a
                                                    left join SFCS_OPERATION_LINES b on a.OPERATION_LINE_ID=b.ID
                                                    left join SYS_USER_ORGANIZE c on b.ORGANIZE_ID=c.ORGANIZE_ID
													left join ANDON_CALL_DATA d on d.CALL_CONTENT_ID=a.id
													where 1=1 and a.STATUS=0 {0}
                                            ) temp 
                                        ) where  rowno BETWEEN ((:Page-1)*:Limit+1) AND (:Limit*:Page) order by id desc", conditions);

            //string pagedSql = SQLBuilderClass.GetPagedSQL(sql, " a.id desc", conditions);
            var resdata = await _dbConnection.QueryAsync<object>(sql, model);
            string sqlcnt = string.Format(@" select count(0) from (
                                                select   a.* from ANDON_CALL_RECORD a
                                                    left join SMT_LINES b on a.OPERATION_LINE_ID=b.ID
                                                    left join SYS_USER_ORGANIZE c on b.ORGANIZE_ID=c.ORGANIZE_ID
                                                    where 1=1 {0}
                                                union all
                                                select   a.* from ANDON_CALL_RECORD a
                                                    left join SFCS_OPERATION_LINES b on a.OPERATION_LINE_ID=b.ID
                                                    left join SYS_USER_ORGANIZE c on b.ORGANIZE_ID=c.ORGANIZE_ID
                                                    where 1=1 {0}
                                            ) ", conditions);
            int cnt = await _dbConnection.ExecuteScalarAsync<int>(sqlcnt, model);

            return new TableDataModel
            {
                count = cnt,
                data = resdata?.ToList(),
            };
        }

        /// <summary>
        /// 查询首页待办事项信息数据
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public async Task<TableDataModel> GetWaitTakeData(AndonCallRecordHomePageRequestModel model)
        {
            string conditions = "WHERE a.ID > 0 ";
            if (!model.User_ID.IsNullOrWhiteSpace())
            {
                conditions += $" and (a.USER_ID=:User_ID)";
            }
            if (model.ID > 0)
            {
                conditions += $" and a.ID=:ID";
            }
            string sql = @"select rownum as rowno, a.* from (
                            select   distinct c.USER_ID, a.* from ANDON_CALL_RECORD a
                            left join ANDON_CALL_Notice b on a.ID=b.MST_ID
                            left join ANDON_CALL_NOTICE_RECEIVER c on b.ID=c.MST_ID
                            where a.STATUS=0
                        ) a ";

            string pagedSql = SQLBuilderClass.GetPagedSQL(sql, " a.id desc", conditions);
            var resdata = await _dbConnection.QueryAsync<object>(pagedSql, model);

            string sqlcnt = @"select count(0) from (
                                select   distinct c.USER_ID, a.* from ANDON_CALL_RECORD a
                                left join ANDON_CALL_Notice b on a.ID=b.MST_ID
                                left join ANDON_CALL_NOTICE_RECEIVER c on b.ID=c.MST_ID
                                where a.STATUS=0
                            ) a " + conditions;
            int cnt = await _dbConnection.ExecuteScalarAsync<int>(sqlcnt, model);

            return new TableDataModel
            {
                count = cnt,
                data = resdata?.ToList(),
            };
        }

    }
}