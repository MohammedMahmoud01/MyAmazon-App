
using System;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Linq;
using System.Collections.Generic;
using System.Net.Http;
using System.Net;
using System.Net.Http.Headers;
using Xamarin.Forms;

namespace Amazon.Helpers
{
    public class ClsApiParamters
    {
        public string ParamterName { get; set; }
        public string ParamterValue { get; set; }
    }

    public static class Utility
    {
        static bool InWork = false;
        //#if DEBUG
        //        public static readonly string ServerUrl = "https://www.dt-works.com/amd/horse";
        //#else
        //        //public static readonly string ServerUrl = "https://www.psaiahf.com/horse";
        //#endif

        public static readonly string AliServerUrl = "http://souq.alishahin.net/";

        public static readonly string ServerUrl = "http://mohammedmahmoud-001-site1.htempurl.com";//http://mohammedmahmoud-001-site1.htempurl.com

        public static string StripHTML(string input)
        {
            return Regex.Replace(input, "<.*?>", String.Empty);
        }

        public static string FormatTextWithoutnewLin(string input)
        {
            return input.Replace("\r", " ").Replace("\n", ".");
        }


        /// <summary>
        /// Converts unix date to C# date time
        /// </summary>
        /// <param name="UnixDate"></param>
        /// <returns></returns>
        public static DateTime ConvertUnixToDateTime(string UnixDate)
        {

            if (string.IsNullOrEmpty(UnixDate))
                return new DateTime();

            // Example of a UNIX timestamp for 11-29-2013 4:58:30
            double timestamp = double.Parse(UnixDate);

            // Format our new DateTime object to start at the UNIX Epoch
            System.DateTime dateTime = new System.DateTime(1970, 1, 1, 0, 0, 0, 0);

            // Add the timestamp (number of seconds since the Epoch) to be converted
            dateTime = dateTime.AddSeconds(timestamp);

            return dateTime;

        }


        /// <summary>
        /// Return the HTTP client for the service address 
        /// </summary>
        /// <returns>HTTP Client Object </returns>
        public static HttpClient GetClient()
        {
            CookieContainer cookieContainer = new CookieContainer();
            Cookie cookie = new Cookie();
            Uri baseAddress = new Uri(Utility.ServerUrl);
            var handler = new HttpClientHandler() { CookieContainer = cookieContainer };
            var client = new HttpClient(handler) { BaseAddress = baseAddress };

            if (cookie != null && !string.IsNullOrWhiteSpace(cookie.Value))
            {
                cookieContainer.Add(baseAddress, cookie);
            }

            return client;
        }

        public static async Task<string> CallWebApi(string url)
        {

            //if (InWork)
            //    return "inwork";

            InWork = true;

            try
            {
                using (var client = GetClient())
                {
                    client.DefaultRequestHeaders.Accept.Clear();
                    client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                    //client.DefaultRequestHeaders.Add("authorization", "Basic YmtkcmR0NGl0OjZKTDJuRXlGNW04WWg=");

                    try
                    {

                        HttpResponseMessage loginResponse = await client.GetAsync(Utility.ServerUrl + url);
                        if (loginResponse.IsSuccessStatusCode)
                        {
                            using (var responseContent = loginResponse.Content)
                            {
                                var result = responseContent.ReadAsStringAsync().Result;
                                return result;
                            }
                        }

                        HttpResponseMessage loginResponse2 = await client.GetAsync(Utility.AliServerUrl + url);
                        if (loginResponse2.IsSuccessStatusCode)
                        {
                            using (var responseContent = loginResponse2.Content)
                            {
                                var result = responseContent.ReadAsStringAsync().Result;
                                return result;
                            }
                        }

                    }
                    catch (Exception ex)
                    {

                        throw;
                    }
                }
            }
            catch (WebException ex)
            {
                //await App.Current.MainPage.DisplayAlert("message from server", sResult, "");
                //Log.Report(ex);
                throw ex;
            }
            catch (Exception ex)
            {
                //await App.Current.MainPage.DisplayAlert("message from server", sResult, "");
                //Log.Report(ex);
            }
            finally
            {
                InWork = false;
            }
            return null;
        }

        public static async Task<string> CallWebApi(ClsApiParamters Pramater, string url)
        {
            InWork = true;

            var parameters = new List<KeyValuePair<string, string>>();
                parameters.Add(new KeyValuePair<string, string>(Pramater.ParamterName, Pramater.ParamterValue));

            HttpContent content = new FormUrlEncodedContent(parameters);
            try
            {
                using (var client = GetClient())
                {
                    client.DefaultRequestHeaders.Accept.Clear();
                    client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                    //client.DefaultRequestHeaders.Add("authorization", "Basic YmtkcmR0NGl0OjZKTDJuRXlGNW04WWg=");
                    try
                    {
                        string URL = (url[0] == 'h')? url : Utility.ServerUrl + url;
                        HttpResponseMessage loginResponse = await client.GetAsync(URL);
                        if (loginResponse.IsSuccessStatusCode)
                        {
                            using (var responseContent = loginResponse.Content)
                            {
                                var result = responseContent.ReadAsStringAsync().Result;
                                return result;
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        return "WebException";
                    }
                }
            }
            catch (WebException ex)
            {
                //await App.Current.MainPage.DisplayAlert("message from server", sResult, "");
                //Log.Report(ex);
                throw ex;
            }
            catch (Exception ex)
            {
                //await App.Current.MainPage.DisplayAlert("message from server", sResult, "");
                //Log.Report(ex);
            }
            finally
            {
                InWork = false;
            }
            return null;
        }

        public static async Task<string> PostData(string url, string content)
        {
            var httpLient = new HttpClient();
            httpLient.DefaultRequestHeaders.Accept.Clear();
            httpLient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

            try
            {
                HttpContent Mycontent = new StringContent(content);
                MediaTypeHeaderValue mValue = new MediaTypeHeaderValue("application/json");
                Mycontent.Headers.ContentType = mValue;
                var response = await httpLient.PostAsync(ServerUrl + url, Mycontent);
                var result = await response.Content.ReadAsStringAsync();
                if (response.IsSuccessStatusCode)
                {
                    var responseData = await response.Content.ReadAsStringAsync();
                    return responseData.ToString();
                }
                else
                    return "api not responding";
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
        }

        public static byte[] ReadToEnd(System.IO.Stream stream)
        {
            long originalPosition = 0;

            if (stream.CanSeek)
            {
                originalPosition = stream.Position;
                stream.Position = 0;
            }

            try
            {
                byte[] readBuffer = new byte[4096];

                int totalBytesRead = 0;
                int bytesRead;

                while ((bytesRead = stream.Read(readBuffer, totalBytesRead, readBuffer.Length - totalBytesRead)) > 0)
                {
                    totalBytesRead += bytesRead;

                    if (totalBytesRead == readBuffer.Length)
                    {
                        int nextByte = stream.ReadByte();
                        if (nextByte != -1)
                        {
                            byte[] temp = new byte[readBuffer.Length * 2];
                            Buffer.BlockCopy(readBuffer, 0, temp, 0, readBuffer.Length);
                            Buffer.SetByte(temp, totalBytesRead, (byte)nextByte);
                            readBuffer = temp;
                            totalBytesRead++;
                        }
                    }
                }

                byte[] buffer = readBuffer;
                if (readBuffer.Length != totalBytesRead)
                {
                    buffer = new byte[totalBytesRead];
                    Buffer.BlockCopy(readBuffer, 0, buffer, 0, totalBytesRead);
                }
                return buffer;
            }
            finally
            {
                if (stream.CanSeek)
                {
                    stream.Position = originalPosition;
                }
            }
        }

        public static async Task ShareData(string message, string link)
        {
            try
            {
                //var title = "PSAIAHF";

                //Plugin.Share.Abstractions.ShareMessage ShareMesssage = new Plugin.Share.Abstractions.ShareMessage();
                //ShareMesssage.Text = message;
                //ShareMesssage.Title = title;
                //ShareMesssage.Url = link;

                //await CrossShare.Current.Share(ShareMesssage);
            }
            catch (Exception ex)
            {

            }
        }

        public static void ClearNavigationStack()
        {
            var existingPages = Application.Current.MainPage.Navigation.NavigationStack.ToList();
            for (int i = 1; i < existingPages.Count; i++)
            {
                Application.Current.MainPage.Navigation.RemovePage(existingPages[i]);
            }
        }
    }
}
