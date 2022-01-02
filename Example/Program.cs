using System;
using System.Threading.Tasks;

namespace HydroQuebecApi
{
    class Program
    {
        /// <summary>
        /// Command-line arguments:
        /// Example.exe [username] [password]
        /// </summary>
        /// <param name="args"></param>
        /// <returns></returns>
        static async Task Main(string[] args)
        {
            var client = new Client();
            await client.LoginAsync(args[1], args[2]);
            await client.SelectCustomerByIndexAsync(0);


            var periodData = client.FetchPeriodDataAsync().Result;
            var yearlyData = client.FetchYearlyDataAsync().Result;
            var monthlyData = client.FetchMonthlyDataAsync().Result;
            var dailyData = client.FetchDailyDataAsync(DateTime.Today.AddDays(-7), DateTime.Today.AddDays(-3)).Result;
            var hourlyData = client.FetchHourlyDataAsync(DateTime.Today.AddDays(-7)).Result;
        }
    }
}
