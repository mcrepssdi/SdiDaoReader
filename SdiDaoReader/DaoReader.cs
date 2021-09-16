#nullable enable
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Reflection;
using NLog;
using SdiDaoReader.Attributes;
using SdiDaoReader.Attributes.AttributeEnums;

namespace SdiDaoReader
{
    public static class DaoReader
    {
        private static readonly Logger Logger = LogManager.GetCurrentClassLogger();

        public static void Load(object target, IDataReader sdr)
        {
            Logger.Trace("Entering...");
            Type targetType = target.GetType();
            List<PropertyInfo> properties = targetType.GetProperties().ToList();
            foreach (PropertyInfo prop in properties)
            {
                string colname = prop.Name;
                List<Attribute> attrs = Attribute.GetCustomAttributes(prop).ToList();
                if (attrs.Count > 0)
                {
                    Attribute? ignoreProperty = (from s in attrs where s is IgnoreProperty select s).FirstOrDefault();
                    if (ignoreProperty != null)
                    {
                        IgnoreProperty t = (IgnoreProperty)ignoreProperty;
                        if (IgnoreType.ALL == t.Type) continue;
                        if (IgnoreType.SELECT_ONLY == t.Type) continue;
                        if (IgnoreType.BOTH_SELECT_AND_INSERT == t.Type) continue;
                    }

                    Attribute? displayName = (from s in attrs where s is SqlColumnName select s).FirstOrDefault();
                    if (displayName != null)
                    {
                        SqlColumnName t = (SqlColumnName)displayName;
                        colname = t.Name;
                    }
                }
                
                object? data;
                Type? nullable = Nullable.GetUnderlyingType(prop.PropertyType);
                if (nullable != null)
                {
                    data = GetData(nullable, sdr, colname);
                    prop.SetValue(target, data, null);
                }
                else
                {
                    data = GetData(prop.PropertyType, sdr, colname);
                    prop.SetValue(target, Convert.ChangeType(data, prop.PropertyType), null);                    
                }
            }
        }
        
        public static object? GetData(Type type, IDataReader sdr, string colName)
        {
            object? data = null;
            if (type == typeof(string))
                data = SafeReader.GetSafeString(sdr, colName);
            else if (type == typeof(byte))
                data = SafeReader.GetSafeByte(sdr, colName);
            else if (type == typeof(short))
                data = SafeReader.GetSafeInt16(sdr, colName);
            else if (type == typeof(int))
                data = SafeReader.GetSafeInt32(sdr, colName);
            else if (type == typeof(long))
                data = SafeReader.GetSafeInt64(sdr, colName);
            else if (type == typeof(decimal))
                data = SafeReader.GetSafeDecimal(sdr, colName);
            else if (type == typeof(double))
                data = SafeReader.GetSafeDouble(sdr, colName);
            else if (type == typeof(float))
                data = SafeReader.GetSafeFloat(sdr, colName);
            else if (type == typeof(DateTime))
                data = SafeReader.GetSafeDateTime(sdr, colName);
            else if (type == typeof(bool))
                data = SafeReader.GetSafeBoolean(sdr, colName);
            else if (type == typeof(byte[]))
                data = SafeReader.GetSafeBytes(sdr, colName);
            return data;
        }
    }
}