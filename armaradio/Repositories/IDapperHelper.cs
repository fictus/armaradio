using Dapper;
using Microsoft.Data.SqlClient;

namespace armaradio.Repositories
{
    public interface IDapperHelper
    {
        SqlConnection GetConnection(string ConnectionName, bool OpenConnection = true);
        int ExecuteNonQuery(string ConnectionName, string StoredProcedure, DynamicParameters DynamicParameters = null);
        int ExecuteNonQuery(string ConnectionName, string StoredProcedure, object DynamicParameters = null);
        int ExecuteNonQuery(SqlConnection DbConnection, string StoredProcedure, object DynamicParameters = null);
        List<T> GetList<T>(string ConnectionName, string StoredProcedure);
        List<T> GetList<T>(SqlConnection DbConnection, string StoredProcedure);
        List<T> GetList<T>(string ConnectionName, string StoredProcedure, DynamicParameters DynamicParameters);
        List<T> GetList<T>(SqlConnection DbConnection, string StoredProcedure, DynamicParameters DynamicParameters);
        List<T> GetList<T>(string ConnectionName, string StoredProcedure, object DynamicParameters);
        List<T> GetList<T>(SqlConnection DbConnection, string StoredProcedure, object DynamicParameters);
        T GetFirstOrDefault<T>(string ConnectionName, string StoredProcedure);
        T GetFirstOrDefault<T>(SqlConnection DbConnection, string StoredProcedure);
        T GetFirstOrDefault<T>(string ConnectionName, string StoredProcedure, DynamicParameters DynamicParameters);
        T GetFirstOrDefault<T>(SqlConnection DbConnection, string StoredProcedure, DynamicParameters DynamicParameters);
        T GetFirstOrDefault<T>(string ConnectionName, string StoredProcedure, object DynamicParameters);
        T GetFirstOrDefault<T>(SqlConnection DbConnection, string StoredProcedure, object DynamicParameters);
    }
}
