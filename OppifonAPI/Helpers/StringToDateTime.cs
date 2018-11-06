using System;

namespace OppifonAPI.Helpers
{
    public class StringToDateTime
    {
        public static DateTime Convert(string datetime)
        {
            var counter = 0;
            var day = datetime.Substring(counter, 2);
            if (day.Contains("/"))
            {
                day = day.Substring(0, 1);
                counter += 2;
            }
            else
                counter += 3;

            var month = datetime.Substring(counter, 2);
            if (month.Contains("/"))
            {

                month = month.Substring(0, 1);
                counter += 2;
            }
            else
                counter += 3;

            var year = datetime.Substring(counter, 4);
            counter += 5;

            var hour = datetime.Substring(counter, 2);
            if (hour.Contains("."))
            {
                hour = hour.Substring(0, 1);
                counter += 2;
            }
            else
                counter += 3;

            var minute = datetime.Substring(counter, 2);
            if (hour.Contains("."))
            {
                minute = minute.Substring(0, 1);
                counter += 2;
            }
            else
                counter += 3;

            var second = datetime.Substring(counter , 2);

            return new DateTime(int.Parse(year), int.Parse(month), int.Parse(day), int.Parse(hour), int.Parse(minute), int.Parse(second), 0);
        }
    }
}
