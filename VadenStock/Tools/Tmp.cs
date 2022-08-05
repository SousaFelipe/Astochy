using System;

using VadenStock.Tools;



namespace VadenStock.Tools
{
    public class Tmp
    {
        public enum Date
        {

            Full,
            Day,
            Month,
            Year
        }



        public static DateOnly Today
        {
            get
            {
                DateTime date = DateTime.Now;
                return new DateOnly(date.Year, date.Month, date.Day);
            }
        }



        public static DateOnly FirstDayOfMonth(DateTime? subject = null)
        {
            DateTime date = (subject == null)
                ? DateTime.Now
                : subject.Value;

            return new DateOnly(date.Year, date.Month, 1);
        }



        public static DateOnly LastDayOfMonth(DateTime? subject = null)
        {
            DateTime? date = (subject == null)
                ? null
                : subject.Value;

            return FirstDayOfMonth(date).AddMonths(1).AddDays(-1);
        }



        public static string[] GetRange(DateTime? initial, DateTime? final, Date dateType = Date.Full)
        {
            if (initial != null)
            {
                int days = (int)((final ?? DateTime.Now) - initial).Value.TotalDays;

                string[] range = new string[days + 1];
                DateTime date = DateTime.Today;

                for (int i = 0; i < range.Length; i++)
                {
                    date = new(date.Year, date.Month, i + 1);
                    range[i] = date.ToString("dd/MM/yyyy");

                    if (dateType == Date.Day)
                        range[i] = range[i].Split("/")[0];
                    else if (dateType == Date.Month)
                        range[i] = range[i].Split("/")[1];
                    else if (dateType == Date.Year)
                        range[i] = range[i].Split("/")[2];
                }

                return range;
            };

            return Array.Empty<string>();
        }
    }
}
