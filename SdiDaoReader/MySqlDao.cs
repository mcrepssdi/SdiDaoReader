using System;
using System.Collections.Generic;
using System.Data;
using MySqlConnector;
using NLog;
using SdiDaoReader.Attributes;

namespace SdiDaoReader
{
    internal class MySqlDao : Dao
    {
        private static Logger _logger;
        private readonly string _connStr;
        
        public MySqlDao(string connstr, Logger logger = null)
        {
            _connStr = connstr;
            _logger = logger;
        }
        
        public override List<T> SelectItems<T>(CommandType commandType, string sql, string DbName = null, Dictionary<string, object> parameters = null, int timeout = 60)
        {
            _logger?.Trace("Entering...");
            List<T> list = new();
            using MySqlConnection conn = GetConnection();
            conn.Open();
            if (DbName.IsNotEmpty()) conn.ChangeDatabase(DbName);
            using MySqlCommand cmd = new (sql, conn) {CommandType = commandType, CommandTimeout = timeout};
            if (parameters?.Count > 0)
            {
                foreach ((string key, object value) in parameters)
                {
                    cmd.Parameters.Add(new MySqlParameter(key, value));
                }
            }
            using MySqlDataReader sdr = cmd.ExecuteReader();
            if (!sdr.HasRows) return list;
            while (sdr.Read())
            {
                T obj = (T)Activator.CreateInstance(typeof(T));
                DaoReader.Load(obj, sdr);
                list.Add(obj);
            }
            return list;
        }

        public override T SelectItem<T>(CommandType commandType, string sql, string DbName = null, Dictionary<string, object> parameters = null, int timeout = 60)
        {
            _logger?.Trace("Entering...");
            T obj = (T)Activator.CreateInstance(typeof(T));
            using MySqlConnection conn = GetConnection();
            conn.Open();
            if (DbName.IsNotEmpty()) conn.ChangeDatabase(DbName);
            using MySqlCommand cmd = new (sql, conn) {CommandType = commandType, CommandTimeout = timeout};
            if (parameters?.Count > 0)
            {
                foreach ((string key, object value) in parameters)
                {
                    cmd.Parameters.Add(new MySqlParameter(key, value));
                }
            }
            using MySqlDataReader sdr = cmd.ExecuteReader();
            if (!sdr.HasRows) return obj;
            while (sdr.Read())
            {
                DaoReader.Load(obj, sdr);
            }
            return obj;
        }

        public override int ExecuteNonQuery(CommandType commandType, string sql, string DbName = null, Dictionary<string, object> parameters = null, int timeout = 60)
        {
            _logger?.Trace("Entering...");
            using MySqlConnection conn = GetConnection();
            conn.Open();
            if (DbName.IsNotEmpty()) conn.ChangeDatabase(DbName);
            using MySqlCommand cmd = new (sql, conn) {CommandType = commandType, CommandTimeout = timeout};
            if (!(parameters?.Count > 0)) return cmd.ExecuteNonQuery();
            
            foreach ((string key, object value) in parameters)
            {
                cmd.Parameters.Add(new MySqlParameter(key, value));
            }
            return cmd.ExecuteNonQuery();
        }

        public override T Select<T>(CommandType commandType, string sql, string colname, string DbName = null, Dictionary<string, object> parameters = null, int timeout = 60)
        {
            _logger?.Trace("Entering...");
            if (DbName.IsEmpty()) throw new Exception("Default Database cannot be empty");
            T obj = default;
            using MySqlConnection conn = GetConnection();
            conn.Open();
            conn.ChangeDatabase(DbName);
            using MySqlCommand cmd = new (sql, conn) {CommandType = commandType, CommandTimeout = timeout};
            if (parameters is {Count: > 0})
            {
                foreach ((string key, object value) in parameters)
                {
                    cmd.Parameters.Add(new MySqlParameter(key, value));
                }
            }
            using MySqlDataReader sdr = cmd.ExecuteReader();
            if (!sdr.HasRows) return obj;
            while (sdr.Read())
            {
                obj = (T)DaoReader.GetData(typeof(T), sdr, colname);
            }
            return obj;
        }

        public override T ExecuteScalar<T>(CommandType commandType, string sql, string DbName = null, Dictionary<string, object> parameters = null, int timeout = 60)
        {
            _logger?.Trace("Entering...");
            using MySqlConnection conn = GetConnection();
            conn.Open();
            if (DbName.IsNotEmpty()) conn.ChangeDatabase(DbName);
            using MySqlCommand cmd = new (sql, conn) {CommandType = commandType, CommandTimeout = timeout};
            if (!(parameters?.Count > 0)) return (T)cmd.ExecuteScalar();
            
            foreach ((string key, object value) in parameters)
            {
                cmd.Parameters.Add(new MySqlParameter(key, value));
            }
            return (T)cmd.ExecuteScalar();
        }
        
        private MySqlConnection GetConnection()
        {
            MySqlConnection connection = null;
            try
            {
                connection = new MySqlConnection(_connStr);
            }
            catch (Exception e)
            {
                _logger?.Error($"SQLException Error: {e.Message}");
            }

            return connection;
        }
    }
}