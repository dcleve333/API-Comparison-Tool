using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using OfficeOpenXml;
using System.Reflection;
using OfficeOpenXml.Table;
using System.Diagnostics;

namespace ConsoleCompareApis
{
    class Program
    {
        static void Main(string[] args)
        {

            var readresults = CallTwoAPisAsync().GetAwaiter().GetResult();

            if (readresults.Count == 0)
            {
                Console.WriteLine("No issues Found");
                Console.ReadKey();

                //foreach (var xx in readresults)
                //{
                //    Console.WriteLine(xx.CallId);
                //    Console.WriteLine(xx.ResponseTimeOriginal);
                //    Console.WriteLine(xx.ResponseTimeNew);
                //}


                using (var pck = new ExcelPackage())
                {

                    var worksheet = pck.Workbook.Worksheets.Add("API Comparison Result");

                    worksheet.Cells["A1"].LoadFromCollection(readresults, true, TableStyles.Medium9);
                    pck.SaveAs(new FileInfo(Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), "APIComparisonResult.xlsx")));
                }
            }
            else
            {

                foreach (var xx in readresults)
                {
                    Console.WriteLine(xx.CallId);
                    Console.WriteLine(xx.ResponseTimeOriginal);
                    Console.WriteLine(xx.ResponseTimeNew);
                    Console.WriteLine(xx.ResponseOriginal);
                    Console.WriteLine(xx.ResponseNew);
                    
                }


                using (var pck = new ExcelPackage())
                {

                    var worksheet = pck.Workbook.Worksheets.Add("API Comparison Result");

                    worksheet.Cells["A1"].LoadFromCollection(readresults, true, TableStyles.Medium9);
                    pck.SaveAs(new FileInfo(Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), "APIComparisonResult.xlsx")));
                }
            }



        }
        

        private static async Task<List<ResultsofTest>> CallTwoAPisAsync()
        {
            var data = GetData();
            int timeOut = Convert.ToInt32(data[0]);
            var urlOriginal = data[1];
            var urlNew = data[2];
            var queryStringList = data.Skip(3);
            int callID = 0;

            var resultsList = new List<ResultsofTest>();

            foreach (var queryString in queryStringList)
            {
                callID++;

                using (HttpClient client = new HttpClient())
                {
                    client.Timeout = TimeSpan.FromMilliseconds(timeOut);         


                    var responseBodyOriginal = await CallOriginalService(client, urlOriginal, queryString);
                    var responseBodyNew = await CallNewService(client, urlNew, queryString);
            


                    if (responseBodyNew.Item1 != responseBodyOriginal.Item1)
                    {
                        resultsList.Add(new ResultsofTest
                        {
                            CallId = callID,
                            ResponseTimeOriginal = responseBodyOriginal.Item2,
                            ResponseTimeNew = responseBodyNew.Item2,
                            ResponseOriginal = responseBodyOriginal.Item1,
                            ResponseNew = responseBodyNew.Item1
                        }
                        );
                    }




                }

            }

            return resultsList;

        }

        private static async Task<Tuple<string, long>> CallOriginalService(HttpClient client, string urlOriginal, string queryString)
        {

            string responseBodyOriginal = string.Empty;
            long responseTimeOriginal = 0;

            try
            {
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));

                var stopWatch = Stopwatch.StartNew();
                var httpResponse = await client.GetAsync(urlOriginal + queryString);

                responseBodyOriginal = await httpResponse.Content.ReadAsStringAsync();
                stopWatch.Stop();
                responseTimeOriginal = stopWatch.ElapsedMilliseconds;


            }
            catch
            {
                // No logging is needed
                var exception = new HttpRequestException();
                string orgm = exception.Message;
                responseBodyOriginal = orgm;
            }
          
            return new Tuple<string, long>(responseBodyOriginal, responseTimeOriginal);

       

        }

        private static async Task<Tuple<string, long>> CallNewService(HttpClient client, string urlNew, string queryString)
        {
            string responseBodyNew = string.Empty;
            long responseTimeNew = 0;
            try
            {
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));

                var stopWatchnew = Stopwatch.StartNew();
                var httpResponse = await client.GetAsync(urlNew + queryString);

                responseBodyNew = await httpResponse.Content.ReadAsStringAsync();
                stopWatchnew.Stop();
                responseTimeNew = stopWatchnew.ElapsedMilliseconds;

            }
            catch
            {
                // No logging is needed
                var exception = new HttpRequestException();
                string orgm = exception.Message;
                responseBodyNew = orgm;
            }

            return new Tuple<string, long>(responseBodyNew, responseTimeNew);


        }


        private static string[] GetData()
        {
            var filePath = System.IO.Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "DataSource.txt");

            return File.ReadAllLines(filePath);
        }






    }

    public class ResultsofTest
    {
        public int CallId { get; set; }
        public long ResponseTimeOriginal { get; set; }
        public long ResponseTimeNew { get; set; }
        public string ResponseOriginal { get; set; }
        public string ResponseNew { get; set; }
        
    }


}
