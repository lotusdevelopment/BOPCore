using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Configuration;

namespace Core.InnerLogic
{
    public class Connections
    {
        protected internal string GetResponse(string ip, byte[] byteArray)
        {
            try
            {
                var myRequest = (HttpWebRequest)WebRequest.Create(ip);
                myRequest.Method = WebRequestMethods.Http.Post;
                myRequest.ContentType = "application/json; charset=utf-8";
                myRequest.ContentLength = byteArray.Length;
                var dataStream = myRequest.GetRequestStream();
                dataStream.Write(byteArray, 0, byteArray.Length);
                dataStream.Close();
                using (var response = (HttpWebResponse)myRequest.GetResponse())
                using (var reader = new StreamReader(response.GetResponseStream()))
                {
                    return reader.ReadToEnd();
                }
            }
            catch (WebException e)
            {
                Console.WriteLine(e.Message);
                return string.Empty;
            }
        }

        protected internal string GetResponse(string ip, List<string> param)
        {
            try
            {
                var parameters = param.Aggregate("", (current, item) => current + (item + "/"));
                var byteArray = Encoding.UTF8.GetBytes(parameters);
                ip += "/" + parameters;
                ip = ip.TrimEnd('/');
                var myRequest = (HttpWebRequest)WebRequest.Create(ip);
                myRequest.Method = WebRequestMethods.Http.Post;
                myRequest.ContentType = "application/json; charset=utf-8";
                myRequest.ContentLength = byteArray.Length;
                var dataStream = myRequest.GetRequestStream();
                dataStream.Write(byteArray, 0, byteArray.Length);
                dataStream.Close();
                using (var response = (HttpWebResponse)myRequest.GetResponse())
                using (var reader = new StreamReader(response.GetResponseStream()))
                {
                    return reader.ReadToEnd();
                }
            }
            catch (WebException e)
            {
                Console.WriteLine(e.Message);
                return string.Empty;
            }
        }

        protected internal string GetResponse(string ip)
        {
            try
            {
                var myRequest = (HttpWebRequest)WebRequest.Create(ip);
                myRequest.Method = "GET";
                using (var response = (HttpWebResponse)myRequest.GetResponse())
                using (var reader = new StreamReader(response.GetResponseStream()))
                {
                    return reader.ReadToEnd();
                }
            }
            catch (WebException e)
            {
                Console.WriteLine(e.Message);
                return null;
            }
        }

        protected internal string GetResponseGet(string ip, List<Dictionary<string, string>> param)
        {
            try
            {
                var myRequest = (HttpWebRequest)WebRequest.Create(ip + "/");
                myRequest.Method = WebRequestMethods.Http.Get;
                myRequest.ContentType = "application/json; charset=utf-8";
                using (var response = (HttpWebResponse)myRequest.GetResponse())
                using (var reader = new StreamReader(response.GetResponseStream()))
                {
                    return reader.ReadToEnd();
                }
            }
            catch (WebException e)
            {
                Console.WriteLine(e.Message);
                return string.Empty;
            }
        }

        protected internal string GetResponseGet(string ip, List<string> param)
        {
            try
            {
                ip += "/";
                ip = param.Aggregate(ip, (current, item) => current + (item + "/"));
                ip = ip.TrimEnd('/');
                var myRequest = (HttpWebRequest)WebRequest.Create(ip);
                myRequest.Method = WebRequestMethods.Http.Get;
                myRequest.ContentType = "application/json; charset=utf-8";
                using (var response = (HttpWebResponse)myRequest.GetResponse())
                using (var reader = new StreamReader(response.GetResponseStream()))
                {
                    return reader.ReadToEnd();
                }
            }
            catch (WebException e)
            {
                Console.WriteLine(e.Message);
                return string.Empty;
            }
        }

        protected internal string GetResponseGet(string ip, string param)
        {
            try
            {
                var myRequest = (HttpWebRequest)WebRequest.Create(ip + "/" + param);
                myRequest.Method = WebRequestMethods.Http.Get;
                myRequest.ContentType = "application/json; charset=utf-8";
                using (var response = (HttpWebResponse)myRequest.GetResponse())
                using (var reader = new StreamReader(response.GetResponseStream()))
                {
                    return reader.ReadToEnd();
                }
            }
            catch (WebException e)
            {
                Console.WriteLine(e.Message);
                return string.Empty;
            }
        }

        protected internal string GetResponseGet(string ip)
        {
            try
            {
                var myRequest = (HttpWebRequest)WebRequest.Create(ip);
                myRequest.Method = WebRequestMethods.Http.Get;
                myRequest.ContentType = "application/json; charset=utf-8";
                using (var response = (HttpWebResponse)myRequest.GetResponse())
                using (var reader = new StreamReader(response.GetResponseStream()))
                {
                    return reader.ReadToEnd();
                }
            }
            catch (WebException e)
            {
                Console.WriteLine(e.Message);
                return string.Empty;
            }
        }

        protected internal string GetTemResponseGetAsync(Uri baseAddress, string urlComplement)
        {
            using (var httpClient = new HttpClient { BaseAddress = baseAddress })
            {
                httpClient.DefaultRequestHeaders.Add("apiaryauth", WebConfigurationManager.AppSettings["TemHash"]);
                using (var response = httpClient.GetAsync(urlComplement))
                {
                    return response.Result.Content.ReadAsStringAsync().Result;
                }
            }
        }

        protected internal string GetTemResponseGetAsync(Uri baseAddress, string urlComplement, List<string> parameters)
        {
            using (var httpClient = new HttpClient { BaseAddress = baseAddress })
            {
                httpClient.DefaultRequestHeaders.Add("apiaryauth", WebConfigurationManager.AppSettings["TemHash"]);
                if (!urlComplement.EndsWith("/"))
                    urlComplement += '/';
                foreach (var item in parameters)
                {
                    urlComplement += item + '/';
                }
                urlComplement = urlComplement.TrimEnd('/');
                using (var response = httpClient.GetAsync(urlComplement))
                {
                    return response.Result.Content.ReadAsStringAsync().Result;
                }
            }
        }
    }
}