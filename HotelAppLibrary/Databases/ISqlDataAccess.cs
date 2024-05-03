using System.Collections.Generic;

namespace HotelAppLibrary.Databases
{
    public interface ISqlDataAccess
    {
        List<T> LoadData<T, U>(string sqlStatement, U paramemters, string connectionStringName, bool isStoreProcedure = false);
        void SaveData<T>(string sqlStatement, T parameters, string connectionStringName, bool isStoreProcedure = false);
    }
}