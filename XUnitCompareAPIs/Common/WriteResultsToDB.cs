using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;

namespace XUnitCompareAPIs.Common
{
   public class WriteResultsToDB
    {


        public void WriteResults(string testID, int callID,  DateTime originalDomainTimeCalled, DateTime newDomainTimeCalled, DateTime testDateTime, string resultoriginal = null, string resultnew = null)
        {

            var gete = new GetAppsettings();
            string strConn = gete.ReturnDatabaseConnection();

            using (SqlConnection Conn = new SqlConnection(strConn))
            {
                try
                {
                    Conn.Open();
                    Console.WriteLine("Connection Open\n");
                    string sql = "Insert Into DataReturns(TestID, CallID, OriginalDomainReturn, NewDomainReturn, OriginalDomainTimeCalled, NewDomainTimeCalled, TestDateTime) VALUES (@param1, @param2, @param3, @param4, @param5, @param6, @param7)";
                    SqlCommand sqlCommand1 = new SqlCommand(sql, Conn);
                    sqlCommand1.Parameters.Add("@param1", SqlDbType.NVarChar, 20).Value = testID;
                    sqlCommand1.Parameters.Add("@param2", SqlDbType.Int).Value = callID;
                    sqlCommand1.Parameters.Add("@param3", SqlDbType.NVarChar, -1).Value = resultoriginal;
                    sqlCommand1.Parameters.Add("@param4", SqlDbType.NVarChar, -1).Value = resultnew;
                    sqlCommand1.Parameters.Add("@param5", SqlDbType.DateTime).Value = originalDomainTimeCalled;
                    sqlCommand1.Parameters.Add("@param6", SqlDbType.DateTime).Value = newDomainTimeCalled;
                    sqlCommand1.Parameters.Add("@param7", SqlDbType.DateTime).Value = testDateTime;
                    sqlCommand1.CommandType = CommandType.Text;
                    sqlCommand1.ExecuteNonQuery();
                    Conn.Close();
                    Console.WriteLine("Connection Close");

                }

                catch (Exception e)
                {
                    Console.WriteLine(e.StackTrace);
                }

            }
        }
    }
}
