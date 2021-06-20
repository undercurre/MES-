/**
*┌──────────────────────────────────────────────────────────────┐
*│　描    述：接口实现                                                    
*│　作    者：嘉志科技                                            
*│　版    本：2.0    模板代码自动生成                                                
*│　创建时间：2020-10-08 15:21:14                             
*└──────────────────────────────────────────────────────────────┘
*┌──────────────────────────────────────────────────────────────┐
*│　命名空间： JZ.IMS.Repository.Oracle                                  
*│　类    名： SfcsCustomersComplaintRepository                                      
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
using JZ.IMS.Core.Extensions;
using System.Linq;
using System.Collections.Generic;
using System.Data;
using System.IO;

namespace JZ.IMS.Repository.Oracle
{
    public class SfcsCustomersComplaintRepository:BaseRepository<SfcsCustomersComplaint,Decimal>, ISfcsCustomersComplaintRepository
    {
        public SfcsCustomersComplaintRepository(IOptionsSnapshot<DbOption> options)
        {
            _dbOption =options.Get("iWMS");
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
			string sql = "SELECT ENABLED FROM SFCS_CUSTOMERS_COMPLAINT WHERE ID=:ID";
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
			string sql = "UPDATE SFCS_CUSTOMERS_COMPLAINT set ENABLED=:ENABLED WHERE ID=:Id";
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
			string sql = "SELECT SFCS_CUSTOMERS_COMPLAINT_SEQ.NEXTVAL MY_SEQ FROM DUAL";
			var result = await _dbConnection.ExecuteScalarAsync(sql);
			return (decimal)result;
		}

        /// <summary>
		/// 查询列表
		/// </summary>
		/// <param name="model"></param>
		/// <returns></returns>
		public async Task<TableDataModel> LoadData(SfcsCustomersComplaintRequestModel model)
        {
            string conditions = " WHERE A.ID > 0 ";
            if (!model.Key.IsNullOrWhiteSpace())
            {
                //conditions += $"and (instr(m.SCRAPER_NO, :Key) > 0 or instr(m.LOCATION, :Key) > 0 or instr(m.CREATOR, :Key) > 0 )";
            }
            if (model.LINE_ID > 0)
            {
                conditions += " AND A.LINE_ID=:LINE_ID";
            }
            if (!model.WO_NO.IsNullOrEmpty())
            {
                conditions += " AND A.WO_NO=:WO_NO";
            }
            if (!model.PART_NO.IsNullOrEmpty())
            {
                conditions += " AND A.PART_NO=:PART_NO";
            }
            string sql = @"SELECT ROWNUM AS ROWNO,A.*,B.CUSTOMER,C.LINE_NAME FROM SFCS_CUSTOMERS_COMPLAINT A
                            LEFT JOIN SFCS_CUSTOMERS B ON A.CUSTOMERS_ID = B.ID
                            LEFT JOIN V_MES_LINES C ON A.LINE_ID = C.LINE_ID";
            string pagedSql = SQLBuilderClass.GetPagedSQL(sql, "A.ID", conditions);
            var resdata = await _dbConnection.QueryAsync<object>(pagedSql, model);

            string sqlcnt = @"SELECT COUNT(*) FROM SFCS_CUSTOMERS_COMPLAINT A {0}";
            sqlcnt = string.Format(sqlcnt, conditions);
            int cnt = await _dbConnection.ExecuteScalarAsync<int>(sqlcnt, model);
            return new TableDataModel
            {
                count = cnt,
                data = resdata?.ToList(),
            };
        }

        /// <summary>
		/// 获取设备列表
		/// </summary>
		/// <param name="mst_id"></param>
		/// <returns></returns>
		public async Task<List<SfcsCustomers>> GetCustomerList()
        {
            List<SfcsCustomers> result = null;

            string sql = @"SELECT ID,CUSTOMER FROM SFCS_CUSTOMERS order by ID asc";
            var tmpdata = await _dbConnection.QueryAsync<SfcsCustomers>(sql, null);

            if (tmpdata != null)
            {
                result = tmpdata.ToList();
            }
            return result;
        }

        /// <summary>
		/// 获取设备列表
		/// </summary>
		/// <param name="mst_id"></param>
		/// <returns></returns>
		public async Task<string> GetPartDesc(string Code)
        {
            string sql = @"SELECT DESCRIPTION FROM IMS_PART WHERE CODE = :CODE";
            var tmpdata = await _dbConnection.QueryFirstOrDefaultAsync(sql, new { CODE = Code });

            if (tmpdata != null)
            {
                return tmpdata.DESCRIPTION;
            }
            return "";
        }

        // <summary>
        /// 获取表的序列
        /// </summary>
        /// <returns></returns>
        public async Task<decimal> GetPhotoSEQID()
        {
            string sql = "SELECT SFCS_COMPLAINT_PHOTO_SEQ.NEXTVAL MY_SEQ FROM DUAL";
            var result = await _dbConnection.ExecuteScalarAsync(sql);
            return (decimal)result;
        }
        /// <summary>
        /// 保存资源图片
        /// </summary>
        /// <param name="ID"></param>
        /// <param name="filename"></param>
        /// <returns></returns>
        public async Task<TableDataModel> SavePhoto(decimal ID, string filename,string name)
        {
            try
            {
                var photoId = await GetPhotoSEQID();
                string sql = "INSERT INTO SFCS_COMPLAINT_PHOTO(ID,MST_ID,PHOTO_URL,CREATE_TIME,CREATE_USER,ATTRIBUTE1)" +
                    " VALUES(:photoId,:ID,:PHOTO_URL,SYSDATE,'ADMIN',:ATTRIBUTE1)";
                 await _dbConnection.ExecuteAsync(sql, new { ID = ID, PHOTO_URL = filename, ATTRIBUTE1 = name, photoId });
                return await GetPhoto(photoId);
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        /// <summary>
        /// 获取图片
        /// </summary>
        /// <param name="ID"></param>
        /// <returns></returns>
        public async Task<TableDataModel> GetPhoto(decimal ID)
        {
            string sql = @"SELECT ID,ATTRIBUTE1 name,PHOTO_URL url FROM SFCS_COMPLAINT_PHOTO WHERE ID=:ID";
            var resdata = await _dbConnection.QueryAsync<ComplaintFile>(sql, new { ID = ID });
            //转base64
            foreach (var item in resdata)
            {
                item.url = FileToBase64(item.url);
            }
            return new TableDataModel
            {
                count = resdata.Count(),
                data = resdata?.ToList(),
            };
        }

        /// <summary>
        /// 获取图片
        /// </summary>
        /// <param name="ID"></param>
        /// <returns></returns>
        public async Task<TableDataModel> GetPhotoList(decimal ID)
        {
            string sql = @"SELECT ID,ATTRIBUTE1 name,PHOTO_URL url FROM SFCS_COMPLAINT_PHOTO WHERE MST_ID=:MST_ID";
            var resdata = await _dbConnection.QueryAsync<ComplaintFile>(sql,new { MST_ID = ID });
            //转base64
            foreach (var item in resdata)
            {
                item.url = FileToBase64(item.url);
            }
            return new TableDataModel
            {
                count = resdata.Count(),
                data = resdata?.ToList(),
            };
        }

        /// <summary>
        /// 删除资源图片
        /// </summary>
        /// <param name="ID"></param>
        /// <param name="filename"></param>
        /// <returns></returns>
        public int DeleteFile(decimal ID)
        {
            try
            {
                string sql = "DELETE FROM SFCS_COMPLAINT_PHOTO WHERE ID = "+ID;
                return  _dbConnection.Execute(sql);
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        private string FileToBase64(string path) {
            FileStream stream = new FileStream(path, FileMode.Open);
            byte[] bytes = new byte[stream.Length];

            stream.Read(bytes, 0, bytes.Length);

            // 设置当前流的位置为流的开始 

            stream.Seek(0, SeekOrigin.Begin);
            stream.Close();
            string pic = Convert.ToBase64String(bytes);
            return pic;
        }
    }
}