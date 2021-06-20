using System;
using System.Collections.Generic;
using System.Text;

namespace JZ.IMS.Repository.Oracle
{
    /// <summary>
    ///SQL生成器
    /// </summary>
    public class SQLBuilderClass
    {
        /// <summary>
        /// 生成分页语句
        /// </summary>
        /// <param name="sql"></param>
        /// <param name="orderBy"></param>
        /// <param name="wheresql"></param>
        /// <returns></returns>
        public static string GetPagedSQL(string sql, string orderBy, string wheresql = "")
        {
            string pagedSql = "SELECT * FROM ({SelectSql} {WhereClause} order by {OrderBy}) u " +
                " WHERE rowno BETWEEN ((:Page-1)*:Limit+1) AND (:Limit*:Page)";

            pagedSql = pagedSql.Replace("{SelectSql}", sql);
            pagedSql = pagedSql.Replace("{WhereClause}", wheresql);
            pagedSql = pagedSql.Replace("{OrderBy}", orderBy);

            return pagedSql;
        }

        /// <summary>
        /// 生成分页语句
        /// </summary>
        /// <param name="sql"></param>
        /// <param name="wheresql"></param>
        /// <returns></returns>
        public static string GetPagedSQL(string sql, string wheresql = "")
        {
            string pagedSql = "SELECT * FROM ({SelectSql} {WhereClause}) u " +
                " WHERE rowno BETWEEN ((:Page-1)*:Limit+1) AND (:Limit*:Page)";

            pagedSql = pagedSql.Replace("{SelectSql}", sql);
            pagedSql = pagedSql.Replace("{WhereClause}", wheresql);

            return pagedSql;
        }
    }
}
