using Dapper;
using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace armaradio_ops.Data
{
    public class _SqlHelper
    {
        public static SqlConnection GetConnection(string ConnectionName, bool OpenConnection = true)
        {
            SqlConnection returnItem = new SqlConnection("Server=localhost;Database=armaradio;Trusted_Connection=True;TrustServerCertificate=True;Connection Timeout=0;");

            if (OpenConnection)
            {
                returnItem.Open();
            }

            return returnItem;
        }

        public static int ExecuteNonQuery(string ConnectionName, string StoredProcedure, DynamicParameters DynamicParameters = null)
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

        public static int ExecuteNonQuery(string ConnectionName, string StoredProcedure, object DynamicParameters = null)
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

        public static int ExecuteNonQuery(SqlConnection DbConnection, string StoredProcedure, object DynamicParameters = null)
        {
            int returnItem = DbConnection.Execute(StoredProcedure,
                DynamicParameters,
                commandType: System.Data.CommandType.StoredProcedure);

            return returnItem;
        }

        public static List<T> GetList<T>(string ConnectionName, string StoredProcedure)
        {
            List<T> returnItem = null;

            using (var db = GetConnection(ConnectionName))
            {
                returnItem = db.Query<T>(StoredProcedure,
                    commandType: System.Data.CommandType.StoredProcedure).ToList();
            }

            return returnItem;
        }

        public static List<T> GetList<T>(SqlConnection DbConnection, string StoredProcedure)
        {
            List<T> returnItem = null;

            returnItem = DbConnection.Query<T>(StoredProcedure,
                commandType: System.Data.CommandType.StoredProcedure).ToList();

            return returnItem;
        }

        public static List<T> GetList<T>(string ConnectionName, string StoredProcedure, DynamicParameters DynamicParameters)
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

        public static List<T> GetList<T>(SqlConnection DbConnection, string StoredProcedure, DynamicParameters DynamicParameters)
        {
            List<T> returnItem = null;

            returnItem = DbConnection.Query<T>(StoredProcedure,
                DynamicParameters,
                commandType: System.Data.CommandType.StoredProcedure).ToList();

            return returnItem;
        }

        public static List<T> GetList<T>(string ConnectionName, string StoredProcedure, object DynamicParameters)
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

        public static List<T> GetList<T>(SqlConnection DbConnection, string StoredProcedure, object DynamicParameters)
        {
            List<T> returnItem = null;

            returnItem = DbConnection.Query<T>(StoredProcedure,
                DynamicParameters,
                commandType: System.Data.CommandType.StoredProcedure).ToList();

            return returnItem;
        }

        public static T GetFirstOrDefault<T>(string ConnectionName, string StoredProcedure)
        {
            T returnItem;

            using (var db = GetConnection(ConnectionName))
            {
                returnItem = db.Query<T>(StoredProcedure,
                    commandType: System.Data.CommandType.StoredProcedure).FirstOrDefault();
            }

            return returnItem;
        }

        public static T GetFirstOrDefault<T>(SqlConnection DbConnection, string StoredProcedure)
        {
            T returnItem;

            returnItem = DbConnection.Query<T>(StoredProcedure,
                commandType: System.Data.CommandType.StoredProcedure).FirstOrDefault();

            return returnItem;
        }

        public static T GetFirstOrDefault<T>(string ConnectionName, string StoredProcedure, DynamicParameters DynamicParameters)
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

        public static T GetFirstOrDefault<T>(SqlConnection DbConnection, string StoredProcedure, DynamicParameters DynamicParameters)
        {
            T returnItem;

            returnItem = DbConnection.Query<T>(StoredProcedure,
                DynamicParameters,
                commandType: System.Data.CommandType.StoredProcedure).FirstOrDefault();

            return returnItem;
        }

        public static T GetFirstOrDefault<T>(string ConnectionName, string StoredProcedure, object DynamicParameters)
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

        public static T GetFirstOrDefault<T>(SqlConnection DbConnection, string StoredProcedure, object DynamicParameters)
        {
            T returnItem;

            returnItem = DbConnection.Query<T>(StoredProcedure,
                DynamicParameters,
                commandType: System.Data.CommandType.StoredProcedure).FirstOrDefault();

            return returnItem;
        }
    }
}
