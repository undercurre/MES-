using JZ.IMS.Core.DbHelper;
using JZ.IMS.Core.Options;
using JZ.IMS.Core.Repository;
using JZ.IMS.IRepository;
using JZ.IMS.Models;
using JZ.IMS.ViewModels;
using Dapper;
using Microsoft.Extensions.Options;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace JZ.IMS.Repository.Oracle
{
    public class ErpU9Repository : BaseRepository<SmtFeeder, String>, IErpU9Repository
    {
        public ErpU9Repository(IOptionsSnapshot<DbOption> options)
        {
            _dbOption = options.Get("erpU9");
            if (_dbOption == null)
            {
                throw new ArgumentNullException(nameof(DbOption));
            }
            _dbConnection = ConnectionFactory.CreateConnection(_dbOption.DbType, _dbOption.ConnectionString);
        }
        
    }
}
