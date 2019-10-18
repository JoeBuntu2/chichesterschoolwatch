using System;
using System.Collections.Generic; 

namespace DataImporter.Text
{
    class CaseAndWhitespaceInsensitiveComparison : IEqualityComparer<string>
    {
        public   bool Equals(string x, string y)
        {
            if (x.Equals(y, StringComparison.OrdinalIgnoreCase))
                return true;

            if (x.Replace(" ", "").Equals(y.Replace(" ", ""), StringComparison.OrdinalIgnoreCase))
                return true;

            return false;
        }

        public int GetHashCode(string obj)
        {
            throw new NotImplementedException();
        }


        //public override int GetHashCode(string obj)
        //{
        //    var text = (string) obj;
        //    return  text.Replace(" ", "")
        //}
    }
}
