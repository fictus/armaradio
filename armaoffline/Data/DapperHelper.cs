using Dapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Data.Sqlite;

namespace armaoffline.Data
{
    public class DapperHelper : IDapperHelper
    {
        public DapperHelper()
        {
        }

        public SqliteConnection GetConnection(bool OpenConnection = true)
        {
            SqliteConnection returnItem = new SqliteConnection("Data Source=arma.db");

            if (OpenConnection)
            {
                returnItem.Open();
            }

            return returnItem;
        }

        public void CreateDatabase()
        {
            using (var db = GetConnection())
            {
                db.ExecuteAsync(
                    @"
                    create table if not exists
                    armausers
                    (
                        id integer not null primary key autoincrement,
                        username text not null
                    );
            
                    create table if not exists
                    playlists
                    (
                        id integer not null primary key autoincrement,
                        playlistid integer not null,
                        userid integer not null,
                        playlistname text
                    );

                    create table if not exists
                    usersongs
                    (
                        id integer not null primary key autoincrement,
                        songid integer not null,
                        playlistid integer not null,
                        userid integer not null,
                        videoid text not null,
                        songname text,
                        artist text
                    );

                    create table if not exists
                    localsongs
                    (
                        id integer not null primary key autoincrement,
                        videoid text not null,
                        filename text not null
                    );
                    ").Wait();
            }
        }

        public int ExecuteNonQuery(string Sql, DynamicParameters DynamicParameters = null)
        {
            int returnItem = 0;

            using (var db = GetConnection())
            {
                returnItem = db.Execute(Sql,
                    DynamicParameters);
            }

            return returnItem;
        }

        public int ExecuteNonQuery(string Sql, object DynamicParameters = null)
        {
            int returnItem = 0;

            using (var db = GetConnection())
            {
                returnItem = db.Execute(Sql,
                    DynamicParameters);
            }

            return returnItem;
        }

        public int ExecuteNonQuery(SqliteConnection DbConnection, string Sql, object DynamicParameters = null)
        {
            int returnItem = DbConnection.Execute(Sql,
                DynamicParameters);

            return returnItem;
        }

        public List<T> GetList<T>(string Sql)
        {
            List<T> returnItem = null;

            using (var db = GetConnection())
            {
                returnItem = db.Query<T>(Sql).ToList();
            }

            return returnItem;
        }

        public List<T> GetList<T>(SqliteConnection DbConnection, string Sql)
        {
            List<T> returnItem = null;

            returnItem = DbConnection.Query<T>(Sql).ToList();

            return returnItem;
        }

        public List<T> GetList<T>(string Sql, DynamicParameters DynamicParameters)
        {
            List<T> returnItem = null;

            using (var db = GetConnection())
            {
                returnItem = db.Query<T>(Sql,
                    DynamicParameters).ToList();
            }

            return returnItem;
        }

        public List<T> GetList<T>(SqliteConnection DbConnection, string Sql, DynamicParameters DynamicParameters)
        {
            List<T> returnItem = null;

            returnItem = DbConnection.Query<T>(Sql,
                DynamicParameters).ToList();

            return returnItem;
        }

        public List<T> GetList<T>(string Sql, object DynamicParameters)
        {
            List<T> returnItem = null;

            using (var db = GetConnection())
            {
                returnItem = db.Query<T>(Sql,
                    DynamicParameters).ToList();
            }

            return returnItem;
        }

        public List<T> GetList<T>(SqliteConnection DbConnection, string Sql, object DynamicParameters)
        {
            List<T> returnItem = null;

            returnItem = DbConnection.Query<T>(Sql,
                DynamicParameters).ToList();

            return returnItem;
        }

        public T GetFirstOrDefault<T>(string Sql)
        {
            T returnItem;

            using (var db = GetConnection())
            {
                returnItem = db.Query<T>(Sql).FirstOrDefault();
            }

            return returnItem;
        }

        public T GetFirstOrDefault<T>(SqliteConnection DbConnection, string Sql)
        {
            T returnItem;

            returnItem = DbConnection.Query<T>(Sql).FirstOrDefault();

            return returnItem;
        }

        public T GetFirstOrDefault<T>(string Sql, DynamicParameters DynamicParameters)
        {
            T returnItem;

            using (var db = GetConnection())
            {
                returnItem = db.Query<T>(Sql,
                    DynamicParameters).FirstOrDefault();
            }

            return returnItem;
        }

        public T GetFirstOrDefault<T>(SqliteConnection DbConnection, string Sql, DynamicParameters DynamicParameters)
        {
            T returnItem;

            returnItem = DbConnection.Query<T>(Sql,
                DynamicParameters).FirstOrDefault();

            return returnItem;
        }

        public T GetFirstOrDefault<T>(string Sql, object DynamicParameters)
        {
            T returnItem;

            using (var db = GetConnection())
            {
                returnItem = db.Query<T>(Sql,
                    DynamicParameters).FirstOrDefault();
            }

            return returnItem;
        }

        public T GetFirstOrDefault<T>(SqliteConnection DbConnection, string Sql, object DynamicParameters)
        {
            T returnItem;

            returnItem = DbConnection.Query<T>(Sql,
                DynamicParameters).FirstOrDefault();

            return returnItem;
        }
    }
}
