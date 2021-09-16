using System;
using System.Collections.Generic;
using System.Data;
using NLog;
using Oracle.ManagedDataAccess.Client;
using SdiDaoReader.Attributes;

namespace SdiDaoReader
{
    internal class OracleDao : Dao
    {
        private static Logger _logger;
        private readonly string _connStr;
        
        public OracleDao(string connstr, Logger logger = null)
        {
            _connStr = connstr;
            _logger = logger;
        }
        
        public override List<T> SelectItems<T>(CommandType commandType, string sql, string DbName = null, Dictionary<string, object> parameters = null, int timeout = 60)
        {
            _logger?.Trace("Entering...");
            List<T> list = new();
            using OracleConnection conn = GetConnection();
            conn.Open();
            if (DbName.IsNotEmpty()) conn.ChangeDatabase(DbName);
            using OracleCommand cmd = new (sql, conn) {CommandType = commandType, CommandTimeout = timeout};
            if (parameters?.Count > 0)
            {
                foreach ((string key, object value) in parameters)
                {
                    cmd.Parameters.Add(new OracleParameter(key, value));
                }
            }
            using OracleDataReader sdr = cmd.ExecuteReader();
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
            using OracleConnection conn = GetConnection();
            conn.Open();
            if (DbName.IsNotEmpty()) conn.ChangeDatabase(DbName);
            using OracleCommand cmd = new (sql, conn) {CommandType = commandType, CommandTimeout = timeout};
            if (parameters?.Count > 0)
            {
                foreach ((string key, object value) in parameters)
                {
                    cmd.Parameters.Add(new OracleParameter(key, value));
                }
            }
            using OracleDataReader sdr = cmd.ExecuteReader();
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
            using OracleConnection conn = GetConnection();
            conn.Open();
            if (DbName.IsNotEmpty()) conn.ChangeDatabase(DbName);
            using OracleCommand cmd = new (sql, conn) {CommandType = commandType, CommandTimeout = timeout};
            if (!(parameters?.Count > 0)) return cmd.ExecuteNonQuery();
            
            foreach ((string key, object value) in parameters)
            {
                cmd.Parameters.Add(new OracleParameter(key, value));
            }
            return cmd.ExecuteNonQuery();
        }

        public override T Select<T>(CommandType commandType, string sql, string colname, string DbName = null, Dictionary<string, object> parameters = null, int timeout = 60)
        {
            _logger?.Trace("Entering...");
            if (DbName.IsEmpty()) throw new Exception("Default Database cannot be empty");
            T obj = default;
            using OracleConnection conn = GetConnection();
            conn.Open();
            conn.ChangeDatabase(DbName);
            using OracleCommand cmd = new (sql, conn) {CommandType = commandType, CommandTimeout = timeout};
            if (parameters is {Count: > 0})
            {
                foreach ((string key, object value) in parameters)
                {
                    cmd.Parameters.Add(new OracleParameter(key, value));
                }
            }
            using OracleDataReader sdr = cmd.ExecuteReader();
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
            using OracleConnection conn = GetConnection();
            conn.Open();
            if (DbName.IsNotEmpty()) conn.ChangeDatabase(DbName);
            using OracleCommand cmd = new (sql, conn) {CommandType = commandType, CommandTimeout = timeout};
            if (!(parameters?.Count > 0)) return (T)cmd.ExecuteScalar();
            
            foreach ((string key, object value) in parameters)
            {
                cmd.Parameters.Add(new OracleParameter(key, value));
            }
            return (T)cmd.ExecuteScalar();
        }
        
        private OracleConnection GetConnection()
        {
            OracleConnection  connection = null;
            try
            {
                connection = new OracleConnection (_connStr);
            }
            catch (Exception e)
            {
                _logger?.Error($"SQLException Error: {e.Message}");
            }
            return connection;
        }
    }
}