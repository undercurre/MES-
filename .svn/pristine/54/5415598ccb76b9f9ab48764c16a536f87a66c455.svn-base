/**
*┌──────────────────────────────────────────────────────────────┐
*│　描    述：接口实现                                                    
*│　作    者：嘉志科技                                            
*│　版    本：2.0    模板代码自动生成                                                
*│　创建时间：2020-03-25 15:16:15                             
*└──────────────────────────────────────────────────────────────┘
*┌──────────────────────────────────────────────────────────────┐
*│　命名空间： JZ.IMS.Repository.Oracle                                  
*│　类    名： MesFirstCheckRecordHeaderRepository                                      
*└──────────────────────────────────────────────────────────────┘
*/
using JZ.IMS.Core.DbHelper;
using JZ.IMS.Core.Options;
using JZ.IMS.Core.Repository;
using JZ.IMS.IRepository;
using JZ.IMS.Models;
using JZ.IMS.ViewModels;
using Dapper;
using Microsoft.Extensions.Options;
using System;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Linq;
using JZ.IMS.Core.Extensions;

namespace JZ.IMS.Repository.Oracle
{
    public class MesFirstCheckRecordHeaderRepository : BaseRepository<MesFirstCheckRecordHeader, String>, IMesFirstCheckRecordHeaderRepository
    {
        public MesFirstCheckRecordHeaderRepository(IOptionsSnapshot<DbOption> options)
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
            string sql = "SELECT ENABLED FROM MES_FIRST_CHECK_RECORD_HEADER WHERE ID=:ID";
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
            string sql = "UPDATE MES_FIRST_CHECK_RECORD_HEADER set ENABLED=:ENABLED WHERE ID=:Id";
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
            string sql = "SELECT MES_FIRST_CHECK_RECORD_HEADER_SEQ.NEXTVAL MY_SEQ FROM DUAL";
            var result = await _dbConnection.ExecuteScalarAsync(sql);
            return (decimal)result;
        }

        /// <summary>
		///项目是否已被使用 
		/// </summary>
		/// <param name="id">项目id</param>
		/// <returns></returns>
		public async Task<bool> ItemIsByUsed(decimal id)
        {
            string sql = "select count(0) from MES_FIRST_CHECK_RECORD_HEADER where id = :id";
            object result = await _dbConnection.ExecuteScalarAsync(sql, new
            {
                id
            });

            return (Convert.ToInt32(result) > 0);
        }

        /// <summary>
        /// 获取解锁状态(Y/N)和解锁报告
        /// 参数ID:首件测试记录ID
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<object> GetStatusContent(string id)
        {
            string sql = " SELECT O.ID,O.STATUS,O.CONTENT FROM MES_FIRST_CHEK_OPERATIONS O WHERE CHECK_HEADER_ID =:ID ";
            object result = await _dbConnection.QueryAsync<object>(sql, new
            {
                id
            });
            return result;
        }




        /// <summary>
        /// 保存操作
        /// </summary>
        /// <param name="headerId">首件测试记录</param>
        /// <param name="user">操作员</param>
        /// <param name="status">解锁</param>
        /// <param name="content">内容</param>
        /// <returns></returns>
        public async Task<decimal> SaveData(string headerId, string user, string status, string content)
        {

            int result =1;
            ConnectionFactory.OpenConnection(_dbConnection);
            try
            {

                String sql = @"BEGIN
								  INSERT INTO JZMES_LOG.MES_FIRST_CHEK_OPERATIONS SELECT * FROM  MES_FIRST_CHEK_OPERATIONS WHERE CHECK_HEADER_ID = :CHECK_HEADER_ID;
									
								  DELETE FROM MES_FIRST_CHEK_OPERATIONS WHERE CHECK_HEADER_ID = :CHECK_HEADER_ID;
									
							      INSERT INTO MES_FIRST_CHEK_OPERATIONS VALUES(FIRST_CHECK_OPERATION_SEQ.NEXTVAL, :CHECK_HEADER_ID, :UNLOCK_OPERATOR, :STATUS, :CONTENT, SYSDATE);
										
								  END;";
                object redata = await _dbConnection.ExecuteScalarAsync(sql, new
                {
                    CHECK_HEADER_ID = headerId,
                    UNLOCK_OPERATOR = user,
                    STATUS = status,
                    CONTENT = content
                });
                result = Convert.ToInt32(redata);
            }
            catch (Exception ex)
            {
                result = -1;
                
                throw ex;
            }
            finally
            {
                if (_dbConnection.State != System.Data.ConnectionState.Closed)
                {
                    _dbConnection.Close();
                }
            }
            
            return result;
        }

        /// <summary>
        /// 获取数据列表
        /// </summary>
        /// <param name="pageNumber"></param>
        /// <param name="rowsPerPage"></param>
        /// <param name="conditions"></param>
        /// <param name="orderby"></param>
        /// <param name="parameters"></param>
        /// <returns></returns>
        public async Task<IEnumerable<MesFirstCheckRecordHeader>> GetListPagedAsynclData(int pageNumber, int rowsPerPage, string conditions, string orderby, object parameters = null)
        {
            string sql = @"SELECT * FROM (SELECT ROW_NUMBER() OVER(ORDER BY A.Id desc) AS PagedNumber, 
                        A.*,B.CONTENT,B.UNLOCK_OPERATOR,B.STATUS,B.CREATE_TIME,C.LINE_NAME 
                        FROM MES_FIRST_CHECK_RECORD_HEADER A
                        INNER JOIN MES_FIRST_CHEK_OPERATIONS B ON A.ID = B.CHECK_HEADER_ID
                        LEFT JOIN V_MES_LINES C ON A.LINE_ID = C.LINE_ID {2} {3}) u 
                        WHERE PagedNumber BETWEEN (({0}-1) * {1} + 1) AND ({0} * {1})";
            sql = string.Format(sql, pageNumber, rowsPerPage, conditions, orderby);
            return await _dbConnection.QueryAsync<MesFirstCheckRecordHeader>(sql, parameters);
        }
        /// <summary>
        /// 获取数据数量
        /// </summary>
        /// <returns></returns>
        public new async Task<int> RecordCountAsync(string conditions, object parameters = null)
        {
            string sql = @"SELECT COUNT(1)
                            FROM MES_FIRST_CHECK_RECORD_HEADER A
                            INNER JOIN MES_FIRST_CHEK_OPERATIONS B ON A.ID = B.CHECK_HEADER_ID {0}";
            sql = string.Format(sql, conditions);
            var result = await _dbConnection.ExecuteScalarAsync<int>(sql, parameters);
            return result;
        }
    }
}