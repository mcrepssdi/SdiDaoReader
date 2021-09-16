using System;
using SdiDaoReader.Attributes.AttributeEnums;

namespace SdiDaoReader.Attributes
{
    public class IgnoreProperty : Attribute
    {
        public IgnoreType Type { get; set; }

        public IgnoreProperty(IgnoreType type)
        {
            Type = type;
        }
    }
}