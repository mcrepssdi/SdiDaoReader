using System.Collections.Generic;
using System.Data;

namespace SdiDaoReader
{
    public abstract class Dao
    {
        public abstract List<T> SelectItems<T>(CommandType commandType, string sql, string DbName = null, Dictionary<string, object> parameters = null, int timeout = 60);
        public abstract T SelectItem<T>(CommandType commandType, string sql, string DbName = null, Dictionary<string, object> parameters = null, int timeout = 60);
        public abstract int ExecuteNonQuery(CommandType commandType, string sql, string DbName = null, Dictionary<string, object> parameters = null, int timeout = 60);
        public abstract T Select<T>(CommandType commandType, string sql, string colname, string DbName = null, Dictionary<string, object> parameters = null, int timeout = 60);
        public abstract T ExecuteScalar<T>(CommandType commandType, string sql, string DbName = null, Dictionary<string, object> parameters = null, int timeout = 60);
    }
}