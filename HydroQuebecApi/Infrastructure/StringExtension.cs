using System;
using System.Linq;

namespace HydroQuebecApi.Infrastructure
{
    public static class StringExtension
    {
        private static Random random = new Random();
        public static string AddRandom(this string str, int length)
        {
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
            return str + new string(Enumerable.Repeat(chars, length).Select(s => s[random.Next(s.Length)]).ToArray());
        }
    }
}
