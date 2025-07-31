using Dapper;
using Microsoft.Data.SqlClient;

namespace armaradio.Repositories
{
    public class DapperHelper : IDapperHelper
    {
        private readonly IConfiguration _configuration;
        public DapperHelper(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public SqlConnection GetConnection(string ConnectionName, bool OpenConnection = true)
        {
            SqlConnection returnItem = new SqlConnection(_configuration.GetConnectionString(ConnectionName));

            if (OpenConnection)
            {
                returnItem.Open();
            }

            return returnItem;
        }

        public int ExecuteNonQuery(string ConnectionName, string StoredProcedure, DynamicParameters DynamicParameters = null)
        {
            int returnItem = 0;

            using (var db = GetConnection(ConnectionName))
            {
                returnItem = db.Execute(StoredProcedure,
                    DynamicParameters,
                    commandType: System.Data.CommandType.StoredProcedure);
            }

            return returnItem;
        }

        public int ExecuteNonQuery(string ConnectionName, string StoredProcedure, object DynamicParameters = null)
        {
            int returnItem = 0;

            using (var db = GetConnection(ConnectionName))
            {
                returnItem = db.Execute(StoredProcedure,
                    DynamicParameters,
                    commandType: System.Data.CommandType.StoredProcedure);
            }

            return returnItem;
        }

        public int ExecuteNonQuery(SqlConnection DbConnection, string StoredProcedure, object DynamicParameters = null)
        {
            int returnItem = DbConnection.Execute(StoredProcedure,
                DynamicParameters,
                commandType: System.Data.CommandType.StoredProcedure);

            return returnItem;
        }

        public List<T> GetList<T>(string ConnectionName, string StoredProcedure)
        {
            List<T> returnItem = null;

            using (var db = GetConnection(ConnectionName))
            {
                returnItem = db.Query<T>(StoredProcedure,
                    commandType: System.Data.CommandType.StoredProcedure).ToList();
            }

            return returnItem;
        }

        public List<T> GetList<T>(string ConnectionName, string StoredProcedure, DynamicParameters DynamicParameters)
        {
            List<T> returnItem = null;

            using (var db = GetConnection(ConnectionName))
            {
                returnItem = db.Query<T>(StoredProcedure,
                    DynamicParameters,
                    commandType: System.Data.CommandType.StoredProcedure).ToList();
            }

            return returnItem;
        }

        public List<T> GetList<T>(string ConnectionName, string StoredProcedure, DynamicParameters DynamicParameters, int Timeout)
        {
            List<T> returnItem = null;

            using (var db = GetConnection(ConnectionName))
            {
                returnItem = db.Query<T>(StoredProcedure,
                    DynamicParameters,
                    commandType: System.Data.CommandType.StoredProcedure,
                    commandTimeout: Timeout).ToList();
            }

            return returnItem;
        }

        public List<T> GetList<T>(string ConnectionName, string StoredProcedure, object DynamicParameters, int Timeout)
        {
            List<T> returnItem = null;

            using (var db = GetConnection(ConnectionName))
            {
                returnItem = db.Query<T>(StoredProcedure,
                    DynamicParameters,
                    commandType: System.Data.CommandType.StoredProcedure,
                    commandTimeout: Timeout).ToList();
            }

            return returnItem;
        }

        public List<T> GetList<T>(string ConnectionName, string StoredProcedure, object DynamicParameters)
        {
            List<T> returnItem = null;

            using (var db = GetConnection(ConnectionName))
            {
                returnItem = db.Query<T>(StoredProcedure,
                    DynamicParameters,
                    commandType: System.Data.CommandType.StoredProcedure).ToList();
            }

            return returnItem;
        }

        public List<T> GetList<T>(SqlConnection DbConnection, string StoredProcedure)
        {
            List<T> returnItem = null;

            returnItem = DbConnection.Query<T>(StoredProcedure,
                commandType: System.Data.CommandType.StoredProcedure).ToList();

            return returnItem;
        }

        public List<T> GetList<T>(SqlConnection DbConnection, string StoredProcedure, DynamicParameters DynamicParameters)
        {
            List<T> returnItem = null;

            returnItem = DbConnection.Query<T>(StoredProcedure,
                DynamicParameters,
                commandType: System.Data.CommandType.StoredProcedure).ToList();

            return returnItem;
        }

        public List<T> GetList<T>(SqlConnection DbConnection, string StoredProcedure, DynamicParameters DynamicParameters, int Timeout)
        {
            List<T> returnItem = null;

            returnItem = DbConnection.Query<T>(StoredProcedure,
                DynamicParameters,
                commandType: System.Data.CommandType.StoredProcedure,
                commandTimeout: Timeout).ToList();

            return returnItem;
        }

        public List<T> GetList<T>(SqlConnection DbConnection, string StoredProcedure, object DynamicParameters)
        {
            List<T> returnItem = null;

            returnItem = DbConnection.Query<T>(StoredProcedure,
                DynamicParameters,
                commandType: System.Data.CommandType.StoredProcedure).ToList();

            return returnItem;
        }

        public List<T> GetList<T>(SqlConnection DbConnection, string StoredProcedure, object DynamicParameters, int Timeout)
        {
            List<T> returnItem = null;

            returnItem = DbConnection.Query<T>(StoredProcedure,
                DynamicParameters,
                commandType: System.Data.CommandType.StoredProcedure,
                commandTimeout: Timeout).ToList();

            return returnItem;
        }

        public T GetFirstOrDefault<T>(string ConnectionName, string StoredProcedure)
        {
            T returnItem;

            using (var db = GetConnection(ConnectionName))
            {
                returnItem = db.Query<T>(StoredProcedure,
                    commandType: System.Data.CommandType.StoredProcedure).FirstOrDefault();
            }

            return returnItem;
        }

        public T GetFirstOrDefault<T>(SqlConnection DbConnection, string StoredProcedure)
        {
            T returnItem;

            returnItem = DbConnection.Query<T>(StoredProcedure,
                commandType: System.Data.CommandType.StoredProcedure).FirstOrDefault();

            return returnItem;
        }

        public T GetFirstOrDefault<T>(string ConnectionName, string StoredProcedure, DynamicParameters DynamicParameters)
        {
            T returnItem;

            using (var db = GetConnection(ConnectionName))
            {
                returnItem = db.Query<T>(StoredProcedure,
                    DynamicParameters,
                    commandType: System.Data.CommandType.StoredProcedure).FirstOrDefault();
            }

            return returnItem;
        }

        public T GetFirstOrDefault<T>(string ConnectionName, string StoredProcedure, DynamicParameters DynamicParameters, int Timeout)
        {
            T returnItem;

            using (var db = GetConnection(ConnectionName))
            {
                returnItem = db.Query<T>(StoredProcedure,
                    DynamicParameters,
                    commandType: System.Data.CommandType.StoredProcedure,
                commandTimeout: Timeout).FirstOrDefault();
            }

            return returnItem;
        }

        public T GetFirstOrDefault<T>(SqlConnection DbConnection, string StoredProcedure, DynamicParameters DynamicParameters)
        {
            T returnItem;

            returnItem = DbConnection.Query<T>(StoredProcedure,
                DynamicParameters,
                commandType: System.Data.CommandType.StoredProcedure).FirstOrDefault();

            return returnItem;
        }

        public T GetFirstOrDefault<T>(SqlConnection DbConnection, string StoredProcedure, DynamicParameters DynamicParameters, int Timeout)
        {
            T returnItem;

            returnItem = DbConnection.Query<T>(StoredProcedure,
                DynamicParameters,
                commandType: System.Data.CommandType.StoredProcedure,
                commandTimeout: Timeout).FirstOrDefault();

            return returnItem;
        }

        public T GetFirstOrDefault<T>(string ConnectionName, string StoredProcedure, object DynamicParameters)
        {
            T returnItem;

            using (var db = GetConnection(ConnectionName))
            {
                returnItem = db.Query<T>(StoredProcedure,
                    DynamicParameters,
                    commandType: System.Data.CommandType.StoredProcedure).FirstOrDefault();
            }

            return returnItem;
        }

        public T GetFirstOrDefault<T>(string ConnectionName, string StoredProcedure, object DynamicParameters, int Timeout)
        {
            T returnItem;

            using (var db = GetConnection(ConnectionName))
            {
                returnItem = db.Query<T>(StoredProcedure,
                    DynamicParameters,
                    commandType: System.Data.CommandType.StoredProcedure,
                commandTimeout: Timeout).FirstOrDefault();
            }

            return returnItem;
        }

        public T GetFirstOrDefault<T>(SqlConnection DbConnection, string StoredProcedure, object DynamicParameters)
        {
            T returnItem;

            returnItem = DbConnection.Query<T>(StoredProcedure,
                DynamicParameters,
                commandType: System.Data.CommandType.StoredProcedure).FirstOrDefault();

            return returnItem;
        }

        public T GetFirstOrDefault<T>(SqlConnection DbConnection, string StoredProcedure, object DynamicParameters, int Timeout)
        {
            T returnItem;

            returnItem = DbConnection.Query<T>(StoredProcedure,
                DynamicParameters,
                commandType: System.Data.CommandType.StoredProcedure,
                commandTimeout: Timeout).FirstOrDefault();

            return returnItem;
        }
    }
}
