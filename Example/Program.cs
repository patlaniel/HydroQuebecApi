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
            await client.Login(args[1], args[2]);
            await client.SelectCustomerByIndex(0);


            var periodData = client.FetchPeriodData().Result;
            var monthlyData = client.FetchMonthlyData().Result;
            var dailyData = client.FetchDailyData(DateTime.Today.AddDays(-7), DateTime.Today.AddDays(-3)).Result;
            var hourlyData = client.FetchHourlyData(DateTime.Today.AddDays(-7)).Result;
        }
    }
}
