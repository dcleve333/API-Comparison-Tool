using Newtonsoft.Json;
using System;
using System.Net.Http;
using System.IO;
using System.Net;
using System.Threading.Tasks;
using System.Text;

namespace XUnitCompareAPIs.Common
{

    public class MakeCall
    {
        
        public async Task MakeCallsAsync()
        {
            var domain = GetDomainsAndQueryStrings.ReturnDomainsOrQueryStringList("Domain");
            var urlOriginal = domain[0];
            var urlNew = domain[1];
            var querystrings = GetDomainsAndQueryStrings.ReturnDomainsOrQueryStringList("QueryStringList");

            int callID = 0;
            string testID = DateTime.Now.ToString("yyyy-MM-dd") + GenericExtensions.GenerateString();
            var testDateTime = DateTime.Now;

            string responseBodyOriginal = string.Empty;
            string responseBodyNew = string.Empty;

            foreach (var queryString in querystrings)
            {
                callID++;
                var originalDomainTimeCalled = DateTime.Now;
                var newDomainTimeCalled = DateTime.Now;

                using (HttpClient client = new HttpClient())
                {
                    client.Timeout = TimeSpan.FromMilliseconds(10000);

                    try
                    {
                        originalDomainTimeCalled = DateTime.Now;
                        client.DefaultRequestHeaders.Accept.Clear();
                        client.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));

                        var httpResponse = await client.GetAsync(urlOriginal + queryString);

                        if (httpResponse.IsSuccessStatusCode)
                        {
                            responseBodyOriginal = await httpResponse.Content.ReadAsStringAsync();
                        }

                        else if (httpResponse.IsSuccessStatusCode == false)
                        {
                            responseBodyOriginal = httpResponse.StatusCode.ToString();
                        }

                    }
                    catch
                    {
                        // No logging is needed
                        var exception = new HttpRequestException();
                        string newm = exception.Message;
                        responseBodyOriginal = newm;
                    }

                    try
                    {
                        newDomainTimeCalled = DateTime.Now;
                        client.DefaultRequestHeaders.Accept.Clear();
                        client.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));

                        var httpResponse = await client.GetAsync(urlNew + queryString);


                        if (httpResponse.IsSuccessStatusCode)
                        {

                            responseBodyNew = await httpResponse.Content.ReadAsStringAsync();
                        }


                        else if (httpResponse.IsSuccessStatusCode == false)
                        {
                            responseBodyNew = httpResponse.StatusCode.ToString();
                        }




                    }
                    catch
                    {
                        // No logging is needed
                        var exception = new HttpRequestException();
                        string newm = exception.Message;
                        responseBodyNew = newm;
                    }


                    var writeResults = new WriteResultsToDB();
                    writeResults.WriteResults(testID, callID, originalDomainTimeCalled, newDomainTimeCalled, testDateTime, responseBodyOriginal, responseBodyNew);

                }


            }



        }


    }




}
