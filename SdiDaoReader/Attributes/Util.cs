namespace SdiDaoReader.Attributes
{
    public static class Util
    {        
        public static bool IsEmpty(this string str)
        {
            if (str == null) { return true; }
            if (str.Length == 0) { return true; }
            return false;
        }
        
        public static bool IsNotEmpty(this string s)
        {
            return !(IsEmpty(s));
        }
    }
}