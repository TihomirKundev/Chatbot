using System;

namespace ChatBot.Extensions
{
    public static class DateExtensions
    {
        private static readonly DateTime EPOCH = new(1970, 1, 1, 0, 0, 0);

        /// <summary>
        /// Converts the value of the current DateTime object to the amount of seconds since 1970-01-01.
        /// </summary>
        public static long ToUnixTimestamp(this DateTime dateTime) => Convert.ToInt64((dateTime - EPOCH).TotalMilliseconds);

        public static DateTime FromUnixTimestamp(long timestamp) => EPOCH.AddMilliseconds(timestamp);
    }
}
