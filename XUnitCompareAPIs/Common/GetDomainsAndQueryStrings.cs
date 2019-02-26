using System;
using System.Collections.Generic;
using System.Data.SqlClient;

namespace XUnitCompareAPIs.Common
{
    public static class GetDomainsAndQueryStrings
    {
        public static (List<string> dataread, List<string> listquerystringlist) ReturnDomainsandQueryStrings()
        {
            var gete = new GetAppsettings();
            string strConn = gete.ReturnDatabaseConnection();

            var dataread = new List<string>();
            var listquerystringlist = new List<string>();

            using (SqlConnection Conn = new SqlConnection(strConn))
            {
                try
                {
                    Conn.Open();
                    Console.WriteLine("Connection Open\n");
                    SqlCommand sqlCommand1 = new SqlCommand("SELECT Domain FROM Domains", Conn);
                    var dataReader = sqlCommand1.ExecuteReader();

                    while (dataReader.Read())
                    {
                        dataread.Add(dataReader.GetString(0));
                    }

                    Conn.Close();
                    Conn.Open();

                    SqlCommand sqlCommand2 = new SqlCommand("SELECT QueryString FROM QueryStringList", Conn);
                    var dataReader2 = sqlCommand2.ExecuteReader();
                    while (dataReader2.Read())
                    {
                        listquerystringlist.Add(dataReader2.GetString(0));
                    }

                    Conn.Close();
                    Console.WriteLine("Connection Close");

                }
                catch (Exception e)
                {
                    Console.WriteLine(e.StackTrace);
                }

                return (dataread, listquerystringlist);
            }
        }

        public static List<string> ReturnDomainsOrQueryStringList(string domainOrQueryStringList)
        {
            var gete = new GetAppsettings();
            string strConn = gete.ReturnDatabaseConnection();

            var dataread = new List<string>();

            switch (domainOrQueryStringList)
            {
                case "Domain":

                    using (SqlConnection Conn = new SqlConnection(strConn))
                    {
                        try
                        {
                            Conn.Open();
                            Console.WriteLine("Connection Open\n");
                            SqlCommand sqlCommand1 = new SqlCommand("SELECT Domain FROM Domains", Conn);
                            var dataReader = sqlCommand1.ExecuteReader();

                            while (dataReader.Read())
                            {
                                dataread.Add(dataReader.GetString(0));
                            }

                            Conn.Close();

                            Console.WriteLine("Connection Close");

                        }
                        catch (Exception e)
                        {
                            Console.WriteLine(e.StackTrace);
                        }

                        break;                   
                    }
                   

                case "QueryStringList":

                    using (SqlConnection Conn = new SqlConnection(strConn))
                    {
                        try
                        {
                            Conn.Open();
                            Console.WriteLine("Connection Open\n");
                            var sqlCommand1 = new SqlCommand("SELECT QueryString FROM QueryStringList", Conn);
                            var dataReader = sqlCommand1.ExecuteReader();

                            while (dataReader.Read())
                            {
                                dataread.Add(dataReader.GetString(0));
                            }

                            Conn.Close();

                            Console.WriteLine("Connection Close");

                        }
                        catch (Exception e)
                        {
                            Console.WriteLine(e.StackTrace);
                        }

                        break;
                    }

            }

            return (dataread);

        }


    }

}

