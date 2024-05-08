using Dapper;
using Microsoft.Data.Sqlite;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Input;

namespace HotelAppLibrary.Databases
{
    public class SqliteDataAccess : ISqliteDataAccess
    {
        private readonly IConfiguration _config;

        public SqliteDataAccess(IConfiguration config)
        {
            _config = config;
        }

        public List<T> LoadData<T, U>(string sqlStatement, U paramemters, string connectionStringName)
        {

            string connectionString = _config.GetConnectionString(connectionStringName);

            using (IDbConnection cnn = new SqliteConnection(connectionString))
            {
                List<T> rows = cnn.Query<T>(sqlStatement, paramemters).ToList();

                return rows;
            }


        }

        public void SaveData<T>(string sqlStatement, T parameters, string connectionStringName)
        {
            string connectionString = _config.GetConnectionString(connectionStringName);

            using (IDbConnection cnn = new SqliteConnection(connectionString))
            {
                cnn.Execute(sqlStatement, parameters);
            }
        }
    }
}
