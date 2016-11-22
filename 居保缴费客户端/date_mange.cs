using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace 居保缴费客户端
{
    class date_mange
    {
        public static string Get_ShortDate(DateTime dt)
        {
            string year = dt.Year.ToString();
            string monts = dt.Month.ToString();
            string days = dt.Day.ToString();
            string dates = year + "/" + monts + "/" + days;
            return dates;
        }

        public static string Get_LongDate(DateTime dt)
        {
            string year = dt.Year.ToString();
            string monts = dt.Month.ToString();
            string days = dt.Day.ToString();
            string hours = dt.Hour.ToString();
            string minutes = dt.Minute.ToString();
            string seconds = dt.Second.ToString();
            string dates = year + "/" + monts + "/" + days + " " + hours + ":" + minutes + ":" + seconds;
            return dates;
        }
    }
}
