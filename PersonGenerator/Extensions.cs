using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Extensions
{
    public static class ExtensionsClass
    {        
        public static string RandomElement(this string[] arr, Random rnd)
        {
            return arr[rnd.Next(arr.Length)];
        }
    }
}
