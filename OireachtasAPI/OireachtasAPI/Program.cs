#region License Information

// **********************************************************************************************************************************
// Program.cs
// Last Modified: 2024/04/29 11:14
// Last Modified By: Brian Dunne
// Copyright Emydex Technology Ltd @2024
// **********************************************************************************************************************************

#endregion

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace OireachtasAPI
{
    public class Program
    {
        #region Static Fields and Constants

        public static string LEGISLATION_DATASET = "legislation.json";
        public static string MEMBERS_DATASET = "members.json";
        public static string API_URL = "https://api.oireachtas.ie";
        public static string LEGISLATION_ENDPOINT = $"{API_URL}/v1/legislation";
        public static string MEMBERS_ENDPOINT = $"{API_URL}/v1/members";

        #endregion

        #region Public Static Methods

        /// <summary>
        ///     Return bills updated within the specified date range
        /// </summary>
        /// <param name="since">The lastUpdated value for the bill should be greater than or equal to this date</param>
        /// <param name="until">
        ///     The lastUpdated value for the bill should be less than or equal to this date.If unspecified, until
        ///     will default to today's date
        /// </param>
        /// <param name="api">Specifies is using local data source or the API end point</param>
        /// <returns>Returns an IList of Bill objects</returns>
        public static IList<Bill> filterBillsByLastUpdated(DateTime since, DateTime until, bool api = false)
        {
            var leg = load<Bills>(api ? LEGISLATION_ENDPOINT : LEGISLATION_DATASET);
            return leg.results.Where(r => r.bill.lastUpdated.Date >= since && r.bill.lastUpdated.Date <= until)
                      .Select(r => r.bill)
                      .OrderBy(b => b.lastUpdated)
                      .ToList();
        }

        /// <summary>
        ///     Return bills sponsored by the member with the specified pId
        /// </summary>
        /// <param name="pId">The pId value for the member</param>
        /// <param name="api">Specifies is using local data source or the API end point</param>
        /// <returns>Returns an IList of Bill objects</returns>
        public static IList<Bill> filterBillsSponsoredBy(string pId, bool api = false)
        {
            var leg = load<Bills>(api ? LEGISLATION_ENDPOINT : LEGISLATION_DATASET);
            var mem = load<Members>(api ? MEMBERS_ENDPOINT : MEMBERS_DATASET);

            return filterBillsSponsoredBy(leg, mem, pId);
        }

        /// <summary>
        ///     Loads the type for the information from the source data
        /// </summary>
        /// <typeparam name="T">The type representing the data being loaded</typeparam>
        /// <param name="source">The path to the source information</param>
        /// <returns>Returns the data for the specified type</returns>
        public static T load<T>(string source) where T : class
        {
            if (source.StartsWith(API_URL))
            {
                return LoadFromEndpoint<T>(source)
                       .GetAwaiter()
                       .GetResult();
            }

            return JsonConvert.DeserializeObject<T>(new StreamReader(source).ReadToEnd());
        }

        public static void Main(string[] args)
        {
            IList<Bill> bills;
            var api = args.Any() && args.Contains("-online");
            if (!args.Any() || (args.Length == 1 && api))
            {
                bills = FilterMenu(api);
            }
            else
            {
                bills = ParseArgs(args, api);
            }

            if (bills?.Any() == true)
            {
                foreach (var b in bills)
                {
                    Console.WriteLine($"Bill: {b.billNo} - {b.longTitleEn}\r\n");
                }

                Console.WriteLine("Press any key to continue...");
            }
            else if (bills != null)
            {
                Console.WriteLine("No Bills found");
            }

            Console.ReadKey();
        }

        #endregion

        #region Private Methods

        /// <summary>
        ///     Creates the menu for user interaction
        /// </summary>
        /// <param name="api">Specifies is using local data source or the API end point</param>
        /// <returns>Returns an IList of Bill objects</returns>
        private static IList<Bill> FilterMenu(bool api)
        {
            Console.Write("Select a filter:\r\n" + "1. Bill Sponsor\r\n" + "2. Bill Last Updated\r\n" + "Any Other Key; Exit");
            var key = Console.ReadKey();
            switch (key.Key)
            {
                case ConsoleKey.D1:
                    Console.Write("\r\nEnter Sponsor's Id: ");
                    var name = Console.ReadLine();
                    if (name == string.Empty)
                    {
                        Console.WriteLine("You must enter a name");
                        return null;
                    }

                    return filterBillsSponsoredBy(name, api);
                case ConsoleKey.D2:
                    Console.Write("\r\nEnter From Date: ");
                    var from = Console.ReadLine();
                    if (from == string.Empty || !DateTime.TryParse(from, out var fromDate))
                    {
                        Console.WriteLine($"{from} is not a valid Date value (Format: {CultureInfo.CurrentCulture.DateTimeFormat.ShortDatePattern})");
                        return null;
                    }

                    Console.Write("Enter To Date: ");
                    var to = Console.ReadLine();
                    if (to == string.Empty || !DateTime.TryParse(to, out var toDate))
                    {
                        Console.WriteLine($"{to} is not a valid Date value (Format: {CultureInfo.CurrentCulture.DateTimeFormat.ShortDatePattern})");
                        return null;
                    }

                    return filterBillsByLastUpdated(fromDate, toDate, api);
            }

            return null;
        }

        /// <summary>
        ///     Parses the command line arguments to retreive the relavant data
        /// </summary>
        /// <param name="args">The command line arguments</param>
        /// <param name="api">Specifies is using local data source or the API end point</param>
        /// <returns>Returns an IList of Bill objects</returns>
        private static IList<Bill> ParseArgs(string[] args, bool api)
        {
            if (args.Length <= (1 + (api ? 1 : 0)))
            {
                Console.WriteLine("Invalid arguments\r\nPass no arguments or follow the following usage:\r\n" + $"{nameof(OireachtasAPI)}.exe [-Id MemberId] [-date from to] [-online]");
                return null;
            }

            if (args.Contains("-Id"))
            {
                var name = args[Array.FindIndex(args, row => row == "-Id") + 1];
                return filterBillsSponsoredBy(name, api);
            }

            if (args.Contains("-date"))
            {
                if (args.Length < 3)
                {
                    Console.WriteLine("Invalid arguments\r\nPass no arguments or follow the following usage:\r\n" + $"{nameof(OireachtasAPI)}.exe [-Id MemberId] [-date from to]  [-online]");
                    return null;
                }

                var dateIndex = Array.FindIndex(args, row => row == "-date");
                var from = args[dateIndex + 1];
                var to = args[dateIndex + 2];
                if (!DateTime.TryParse(from, out var fromDate))
                {
                    Console.WriteLine($"{from} is not a valid Date value (Format: {CultureInfo.CurrentCulture.DateTimeFormat.ShortDatePattern})");
                    return null;
                }

                if (!DateTime.TryParse(to, out var toDate))
                {
                    Console.WriteLine($"{to} is not a valid Date value (Format: {CultureInfo.CurrentCulture.DateTimeFormat.ShortDatePattern})");
                    return null;
                }

                return filterBillsByLastUpdated(fromDate, toDate, api);
            }

            Console.WriteLine("Invalid arguments\r\nPass no arguments or follow the following usage:\r\n" + $"{nameof(OireachtasAPI)}.exe [-Id MemberId] [-date From To]  [-online]");

            return null;
        }

        /// <summary>
        ///     Loads information from a HTTP endpoint
        /// </summary>
        /// <typeparam name="T">The type representing the data being loaded</typeparam>
        /// <param name="url">The url of the endpoint</param>
        /// <returns>
        ///     Returns a Task that will yield an object of the specified
        ///     <typeparam name="T">
        /// </returns>
        private static async Task<T> LoadFromEndpoint<T>(string url) where T : class
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
        ///     Return bills sponsored by the member with the specified PId
        /// </summary>
        /// <param name="bills">The list of available bills to search</param>
        /// <param name="members">The list of available members to search</param>
        /// <param name="pId">The pId value for the member</param>
        /// <returns>Returns an IList of Bill objects</returns>
        private static IList<Bill> filterBillsSponsoredBy(Bills bills, Members members, string pId)
        {
            var member = members.results.FirstOrDefault(m => m.member.pId == pId);
            if (member == null)
            {
                return null; //Note; assumption that pId is supposed to be a unique identifier
            }

            var sponsorGroup = bills.results.GroupBy(b => b.bill.sponsors)
                                    .Select(g => new
                                                 {
                                                         Sponors = string.Join(",", g.Key.Select(s => s.sponsor.by.showAs)),
                                                         Records = g.Select(b => b.bill)
                                                 });
            var groups = sponsorGroup.Where(g => g.Sponors.Contains(member.member.fullName));
            var ret = new List<Bill>();
            foreach (var g in groups)
            {
                ret.AddRange(g.Records);
            }

            return ret;
        }

        private static double Profiler(Action func)
        {
            Process.GetCurrentProcess()
                   .PriorityClass = ProcessPriorityClass.High;
            Thread.CurrentThread.Priority = ThreadPriority.Highest;

            func(); //Warm up call

            // clean up
            GC.Collect();
            GC.WaitForPendingFinalizers();
            GC.Collect();

            var watch = new Stopwatch();
            watch.Start();
            func();
            watch.Stop();
            return watch.Elapsed.TotalMilliseconds;
        }

        #endregion
    }
}