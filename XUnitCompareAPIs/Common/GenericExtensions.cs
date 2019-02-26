using System;
using System.Collections.Generic;
using System.Text;

namespace XUnitCompareAPIs.Common
{
     class GenericExtensions
    {
        public static string GenerateString()
        {          
            return Guid.NewGuid().ToString().Replace("-", string.Empty).Substring(0, 4);
        }


    }
}
