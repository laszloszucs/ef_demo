using System;
using System.Linq;

namespace ef_demo.Helpers
{
    public static class Randomizer
    {
        private static Random random = new Random();
        public static string RandomString(int length)
        {
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
            return new string(Enumerable.Repeat(chars, length)
              .Select(s => s[random.Next(s.Length)]).ToArray());
        }

        public static int RandomNumber(int from, int to)
        {
            return random.Next(from, to);
        }
    }
}
