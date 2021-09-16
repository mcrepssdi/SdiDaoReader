using NLog;

namespace SdiDaoReader
{
    public class DaoFactory
    {
        public enum DatabaseType
        {
            MsSql,
            MySql,
            Oracle
        }
        
        public static Dao GetDaoFactory(DatabaseType type, string connStr, Logger logger = null)
        {
            logger?.Debug("Entering...");
            return type switch
            {
                DatabaseType.MsSql => new SqlDao(connStr, logger),
                DatabaseType.MySql => new MySqlDao(connStr, logger),
                DatabaseType.Oracle => new OracleDao(connStr, logger)
            };
        }
    }
}