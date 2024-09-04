using Dapper;
using Microsoft.Data.Sqlite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace armaoffline.Data
{
    public interface IDapperHelper
    {
        SqliteConnection GetConnection(bool OpenConnection = true);
        void CreateDatabase();
        int ExecuteNonQuery(string Sql, DynamicParameters DynamicParameters = null);
        int ExecuteNonQuery(string Sql, object DynamicParameters = null);
        int ExecuteNonQuery(SqliteConnection DbConnection, string Sql, object DynamicParameters = null);
        List<T> GetList<T>(string Sql);
        List<T> GetList<T>(SqliteConnection DbConnection, string Sql);
        List<T> GetList<T>(string Sql, DynamicParameters DynamicParameters);
        List<T> GetList<T>(SqliteConnection DbConnection, string Sql, DynamicParameters DynamicParameters);
        List<T> GetList<T>(string Sql, object DynamicParameters);
        List<T> GetList<T>(SqliteConnection DbConnection, string Sql, object DynamicParameters);
        T GetFirstOrDefault<T>(string Sql);
        T GetFirstOrDefault<T>(SqliteConnection DbConnection, string Sql);
        T GetFirstOrDefault<T>(string Sql, DynamicParameters DynamicParameters);
        T GetFirstOrDefault<T>(SqliteConnection DbConnection, string Sql, DynamicParameters DynamicParameters);
        T GetFirstOrDefault<T>(string Sql, object DynamicParameters);
        T GetFirstOrDefault<T>(SqliteConnection DbConnection, string Sql, object DynamicParameters);
    }
}
