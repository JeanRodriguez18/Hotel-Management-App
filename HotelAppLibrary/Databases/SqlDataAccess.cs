using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using Dapper;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
namespace HotelAppLibrary.Databases
{
    public class SqlDataAccess : ISqlDataAccess
    {
        private readonly IConfiguration _config;

        public SqlDataAccess(IConfiguration config)
        {
            _config = config;
        }


        public List<T> LoadData<T, U>(string sqlStatement, U paramemters, string connectionStringName, bool isStoreProcedure = false)
        {
            string connectionString = _config.GetConnectionString(connectionStringName);

            //This is the default for normal querys (select and so on)
            CommandType commandType = CommandType.Text;

            if (isStoreProcedure == true)
            {
                commandType = CommandType.StoredProcedure;
            }

            using (IDbConnection cnn = new SqlConnection(connectionString))
            {
                List<T> rows = cnn.Query<T>(sqlStatement, paramemters, commandType: commandType).ToList();
                return rows;
            }
        }


        public void SaveData<T>(string sqlStatement, T parameters, string connectionStringName, bool isStoreProcedure = false)
        {
            string connectionString = _config.GetConnectionString(connectionStringName);

            //This is the default for normal querys (select and so on)
            CommandType commandType = CommandType.Text;

            if (isStoreProcedure == true)
            {
                commandType = CommandType.StoredProcedure;
            }

            using (IDbConnection cnn = new SqlConnection(connectionString))
            {
                cnn.Execute(sqlStatement, parameters, commandType: commandType);
            }
        }


    }
}
