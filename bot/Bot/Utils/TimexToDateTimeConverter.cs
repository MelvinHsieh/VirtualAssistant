using System;
using System.Globalization;

namespace CoreBot.Utils
{
    public static class TimexToDateTimeConverter
    {
        private static string[] validTimeFormats = new string[]
        {
            "HH",
            "HH:mm",
            "HH:mm:ss"
        };

        public static DateTime TryTimexToDateTime(string timex)
        {
            if (!string.IsNullOrEmpty(timex))
            {
                var timexSplit = timex.Split('T', 2);
                string dateString = timexSplit[0];
                string timeString = timexSplit[1];



                DateTime time;
                if (!DateTime.TryParseExact(timeString, validTimeFormats, CultureInfo.InvariantCulture,
                                              DateTimeStyles.None, out time))
                {
                    switch (timeString)
                    {
                        case "MO":
                            time = new DateTime(0, 0, 0, 9, 0, 0);
                            break;
                        case "AF":
                            time = new DateTime(0, 0, 0, 12, 0, 0);
                            break;
                        default:
                            break;
                    }
                }


                DateTime date;
                if (!DateTime.TryParse(dateString, out date))
                {
                    date = DateTime.Today.Date;
                }

                return new DateTime(date.Year, date.Month, date.Day, time.TimeOfDay.Hours, time.TimeOfDay.Minutes, time.TimeOfDay.Seconds);
            }

            return DateTime.Now;
        }
    }
}
