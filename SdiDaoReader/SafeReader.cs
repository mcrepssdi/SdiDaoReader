using System;
using System.Data;

namespace SdiDaoReader
{
    public class SafeReader
    {
        /// <summary>
        /// Returns a String or Empty String if Column is Null
        /// </summary>
        /// <param name="sdr">DateReader</param>
        /// <param name="colName">Column Name</param>
        /// <returns></returns>
        public static string GetSafeString(IDataReader sdr, string colName)
        {
            string result = string.Empty;
            if (!sdr.IsDBNull(sdr.GetOrdinal(colName)))
            {
                result = sdr.GetString(sdr.GetOrdinal(colName));
            }
            return result.Trim();
        }
        
        /// <summary>
        /// Returns an Byte or 0 if Column is Null
        /// </summary>
        /// <param name="sdr">DateReader</param>
        /// <param name="colName">Column Name</param>
        /// <returns></returns>
        public static short GetSafeByte(IDataReader sdr, string colName)
        {
            byte result = 0;
            if (!sdr.IsDBNull(sdr.GetOrdinal(colName)))
            {
                result = sdr.GetByte(sdr.GetOrdinal(colName));
            }
            return result;
        }

        /// <summary>
        /// Returns an Int16 or 0 if Column is Null
        /// </summary>
        /// <param name="sdr">DateReader</param>
        /// <param name="colName">Column Name</param>
        /// <returns></returns>
        public static short GetSafeInt16(IDataReader sdr, string colName)
        {
            short result = 0;
            if (!sdr.IsDBNull(sdr.GetOrdinal(colName)))
            {
                result = sdr.GetInt16(sdr.GetOrdinal(colName));
            }
            return result;
        }

        /// <summary>
        /// Returns an Int32 or 0 if Column is Null
        /// </summary>
        /// <param name="sdr">DateReader</param>
        /// <param name="colName">Column Name</param>
        /// <returns></returns>
        public static int GetSafeInt32(IDataReader sdr, string colName)
        {
            int result = 0;
            if (!sdr.IsDBNull(sdr.GetOrdinal(colName)))
            {
                result = sdr.GetInt32(sdr.GetOrdinal(colName));
            }
            return result;
        }

        /// <summary>
        /// Returns a long or 0 if Column is Null
        /// </summary>
        /// <param name="sdr">DateReader</param>
        /// <param name="colName">Column Name</param>
        /// <returns></returns>
        public static long GetSafeInt64(IDataReader sdr, string colName)
        {
            long result = 0;
            if (!sdr.IsDBNull(sdr.GetOrdinal(colName)))
            {
                result = sdr.GetInt64(sdr.GetOrdinal(colName));
            }
            return result;
        }

        /// <summary>
        /// Returns an Double or 0 if Column is Null
        /// </summary>
        /// <param name="sdr">DateReader</param>
        /// <param name="colName">Column Name</param>
        /// <returns></returns>
        public static double GetSafeDouble(IDataReader sdr, string colName)
        {
            double result = 0.0;
            if (!sdr.IsDBNull(sdr.GetOrdinal(colName)))
            {
                result = sdr.GetDouble(sdr.GetOrdinal(colName));
            }
            return result;
        }

        /// <summary>
        /// Returns an Decimal or Decimal.Zero if Column is Null
        /// </summary>
        /// <param name="sdr">DateReader</param>
        /// <param name="colName">Column Name</param>
        /// <returns></returns>
        public static decimal GetSafeDecimal(IDataReader sdr, string colName)
        {
            decimal result = decimal.Zero;
            if (!sdr.IsDBNull(sdr.GetOrdinal(colName)))
            {
                result = sdr.GetDecimal(sdr.GetOrdinal(colName));
            }
            return result;
        }

        /// <summary>
        /// Returns an Decimal or null if Column is Null
        /// </summary>
        /// <param name="sdr">DateReader</param>
        /// <param name="colName">Column Name</param>
        /// <returns></returns>
        public static DateTime? GetSafeDateTime(IDataReader sdr, string colName)
        {
            DateTime? result = null;
            if (!sdr.IsDBNull(sdr.GetOrdinal(colName)))
            {
                result = sdr.GetDateTime(sdr.GetOrdinal(colName));
            }
            return result;
        }

        /// <summary>
        ///  Returns an boolean, defaults to false
        /// </summary>
        /// <param name="sdr"></param>
        /// <param name="colName"></param>
        /// <returns></returns>
        public static bool GetSafeBoolean(IDataReader sdr, string colName)
        {
            bool result = false;
            if (!sdr.IsDBNull(sdr.GetOrdinal(colName)))
            {
                result = sdr.GetBoolean(sdr.GetOrdinal(colName));
            }
            return result;
        }
        
        /// <summary>
        /// Returns an float or 0 if Column is Null
        /// </summary>
        /// <param name="sdr">DateReader</param>
        /// <param name="colName">Column Name</param>
        /// <returns></returns>
        public static float GetSafeFloat(IDataReader sdr, string colName)
        {
            float result = 0;
            if (!sdr.IsDBNull(sdr.GetOrdinal(colName)))
            {
                result = sdr.GetFloat(sdr.GetOrdinal(colName));
            }
            return result;
        }

        /// <summary>
        /// Returns a byte[] or empty byte array if Column is Null
        /// </summary>
        /// <param name="sdr">DateReader</param>
        /// <param name="colName">Column Name</param>
        /// <returns></returns>
        public static byte[] GetSafeBytes(IDataReader sdr, string colName)
        {
            if (sdr.IsDBNull(sdr.GetOrdinal(colName))) return Array.Empty<byte>();

            int ordinalPosition = sdr.GetOrdinal(colName);
            long len = sdr.GetBytes(ordinalPosition, 0, null, 0, 0);
            byte[] buffer = new byte[len];
            sdr.GetBytes(ordinalPosition, 0, buffer, 0, (int)len);
            return buffer;
        }
        
    }
}
