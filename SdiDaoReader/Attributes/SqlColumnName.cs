using System;

namespace SdiDaoReader.Attributes
{
    public class SqlColumnName : Attribute
    {
        public SqlColumnName(string name)
        {
            Name = name;
        }

        public string Name { get; }
    }
}