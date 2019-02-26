using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;

namespace XUnitCompareAPIs.Common
{
    class GetAppsettings
    {
       
        static class ConfigurationManager
        {
            public static IConfiguration AppSetting { get; }

            static ConfigurationManager()
            {
                AppSetting = new ConfigurationBuilder()
                        .SetBasePath(Directory.GetCurrentDirectory())
                        .AddJsonFile("appsettings.json")
                        .Build();
            }


        }

        public string ReturnDatabaseConnection()
        {
            var baseB = AppDomain.CurrentDomain.BaseDirectory;

            if (baseB.Contains("ConsoleCompare2ApisDB"))
            {
                baseB = baseB.Replace("ConsoleCompare2ApisDB", "XUnitCompare2APIsDB");
            }

            var baseD = Directory.GetParent(baseB).Parent.Parent.Parent.ToString();
            var appPathD = System.IO.Path.Combine(baseD, "Data");
         

            string conn = ConfigurationManager.AppSetting["ConnectionStrings:DefaultConnection"];
            if (conn.Contains("%CONTENTROOTPATH%"))
            {
                conn = conn.Replace("%CONTENTROOTPATH%", appPathD);
            }


            return conn;
        }

    }
}
