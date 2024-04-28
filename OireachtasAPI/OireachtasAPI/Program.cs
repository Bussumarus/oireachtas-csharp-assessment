using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System;
using System.Diagnostics;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;

namespace OireachtasAPI
{
    public class Program
    {
        public static string LEGISLATION_DATASET = "legislation.json";
        public static string MEMBERS_DATASET = "members.json";
        public static string API_URL = "https://api.oireachtas.ie";

        static void Main(string[] args)
        {
            var milliseconds = Profiler(() => filterBillsSponsoredBy("HenryJJAbbott"));
            Console.WriteLine($"Local Duration: {milliseconds} milliseconds");
            milliseconds = Profiler(() => filterUrlBillsSponsoredBy("HenryJJAbbott"));
            Console.WriteLine($"API Duration: {milliseconds} milliseconds");
            Console.ReadKey();
        }

        //public static Func<string, dynamic> load = jfname => JsonConvert.DeserializeObject((new System.IO.StreamReader(jfname)).ReadToEnd());
        public static dynamic load(string jfname)
        {
            if(jfname.StartsWith(API_URL))
            {
                return LoadFromURL(jfname).GetAwaiter().GetResult();
            }
            
            return JsonConvert.DeserializeObject(new System.IO.StreamReader(jfname).ReadToEnd());
        }

        public static T loadTyped<T>(string jfname) where T : class
        {
            if (jfname.StartsWith(API_URL))
            {
                return LoadFromURLTyped<T>(jfname).GetAwaiter().GetResult();
            }

            return JsonConvert.DeserializeObject<T>(new System.IO.StreamReader(jfname).ReadToEnd());
        }

        private static async Task<dynamic> LoadFromURL(string url)
        {
            var client = new HttpClient();
            var response = await client.GetAsync(url);
            if(response.IsSuccessStatusCode)
            {
                return await response.Content.ReadAsAsync<dynamic>();
            }

            return null;
        }

        private static async Task<T> LoadFromURLTyped<T>(string url) where T: class
        {
            var client = new HttpClient();
            var response = await client.GetAsync(url);
            if (response.IsSuccessStatusCode)
            {
                return await response.Content.ReadAsAsync<T>();
            }

            return null;
        }

        /// <summary>
        /// Return bills sponsored by the member with the specified pId
        /// </summary>
        /// <param name="pId">The pId value for the member</param>
        /// <returns>List of bill records</returns>
        public static List<dynamic> filterBillsSponsoredBy(string pId)
        {
            dynamic leg = load(LEGISLATION_DATASET);
            dynamic mem = load(MEMBERS_DATASET);

            List<dynamic> ret = new List<dynamic>();

            foreach (dynamic res in leg["results"])
            {
                dynamic p = res["bill"]["sponsors"];
                foreach(dynamic i in p)
                {
                    string name = i["sponsor"]["by"]["showAs"];
                    foreach (dynamic result in mem["results"])
                    {
                        string fname = result["member"]["fullName"];
                        string rpId = result["member"]["pId"];
                        if (fname == name && rpId == pId){
                            ret.Add(res["bill"]);
                        }
                    }
                }               
            }
            return ret;
        }

        /// <summary>
        /// Return bills sponsored by the member with the specified pId
        /// </summary>
        /// <param name="pId">The pId value for the member</param>
        /// <returns>List of bill records</returns>
        public static List<dynamic> filterUrlBillsSponsoredBy(string pId)
        {
            dynamic leg = load("https://api.oireachtas.ie/v1/legislation");
            dynamic mem = load("https://api.oireachtas.ie/v1/members");

            List<dynamic> ret = new List<dynamic>();

            foreach (dynamic res in leg["results"])
            {
                dynamic p = res["bill"]["sponsors"];
                foreach (dynamic i in p)
                {
                    string name = i["sponsor"]["by"]["showAs"];
                    foreach (dynamic result in mem["results"])
                    {
                        string fname = result["member"]["fullName"];
                        string rpId = result["member"]["pId"];
                        if (fname == name && rpId == pId)
                        {
                            ret.Add(res["bill"]);
                        }
                    }
                }
            }
            return ret;
        }

        /// <summary>
        /// Return bills sponsored by the member with the specified pId
        /// </summary>
        /// <param name="pId">The pId value for the member</param>
        /// <returns>List of bill records</returns>
        public static List<dynamic> filterTyedUrlTypedBillsSponsoredBy(string pId)
        {
            dynamic leg = loadTyped<Bills>("https://api.oireachtas.ie/v1/legislation");
            dynamic mem = loadTyped<Members>("https://api.oireachtas.ie/v1/members");

            List<dynamic> ret = new List<dynamic>();

            foreach (dynamic res in leg["results"])
            {
                dynamic p = res["bill"]["sponsors"];
                foreach (dynamic i in p)
                {
                    string name = i["sponsor"]["by"]["showAs"];
                    foreach (dynamic result in mem["results"])
                    {
                        string fname = result["member"]["fullName"];
                        string rpId = result["member"]["pId"];
                        if (fname == name && rpId == pId)
                        {
                            ret.Add(res["bill"]);
                        }
                    }
                }
            }
            return ret;
        }

        /// <summary>
        /// Return bills sponsored by the member with the specified pId
        /// </summary>
        /// <param name="pId">The pId value for the member</param>
        /// <returns>List of bill records</returns>
        public static List<dynamic> filterUrlLinqBillsSponsoredBy(string pId)
        {
            dynamic leg = load("https://api.oireachtas.ie/v1/legislation");
            dynamic mem = load("https://api.oireachtas.ie/v1/members");

            List<dynamic> ret = new List<dynamic>();

            foreach (dynamic res in leg["results"])
            {
                dynamic p = res["bill"]["sponsors"];
                foreach (dynamic i in p)
                {
                    string name = i["sponsor"]["by"]["showAs"];
                    foreach (dynamic result in mem["results"])
                    {
                        string fname = result["member"]["fullName"];
                        string rpId = result["member"]["pId"];
                        if (fname == name && rpId == pId)
                        {
                            ret.Add(res["bill"]);
                        }
                    }
                }
            }
            return ret;
        }

        /// <summary>
        /// Return bills updated within the specified date range
        /// </summary>
        /// <param name="since">The lastUpdated value for the bill should be greater than or equal to this date</param>
        /// <param name="until">The lastUpdated value for the bill should be less than or equal to this date.If unspecified, until will default to today's date</param>
        /// <returns>List of bill records</returns>
        public static List<dynamic> filterBillsByLastUpdated(DateTime since, DateTime until)
        {
            throw new NotImplementedException();
        }

        private static double Profiler(Action func)
        {
            var watch = new Stopwatch();
            watch.Start();
            func();
            watch.Stop();

            // clean up
            GC.Collect();
            GC.WaitForPendingFinalizers();
            GC.Collect();
            return watch.Elapsed.TotalMilliseconds;
        }
    }
}
