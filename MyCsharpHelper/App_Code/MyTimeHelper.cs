using System;
using System.Web;


// RemoveSpecialChars fuction allow us to clear string



public static class MyTimeHelper
{
    private static TimeZoneInfo EASTERN_ZONE = TimeZoneInfo.FindSystemTimeZoneById("Eastern Standard Time");

    public static string FormatDate(object dateObject, string format)
    {
        if (dateObject.ToString().Equals(""))
            return "";
        string dateString = int.Parse(dateObject.ToString()).ToString("00000000");
        // input strings are in 'host' format.  yyyyMMdd and hhmm
        DateTime temp = DateTime.ParseExact(dateString, "yyyyMMdd", null);
        return temp.ToString(format);
    }


    public static DateTime GetCurrentDateTime()
    {
        DateTime EasternTime = TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow, EASTERN_ZONE);
        return EasternTime;
    }
    public static string TimeAgo(DateTime dt)
    {
        if (dt > GetCurrentDateTime())
            return "about sometime from now";

        TimeSpan span = GetCurrentDateTime() - dt;

        if (span.Days > 365)
        {
            int years = (span.Days / 365);
            if (span.Days % 365 != 0)
                years += 1;
            return String.Format("about {0} {1} ago", years, years == 1 ? "year" : "years");
        }

        if (span.Days > 30)
        {
            int months = (span.Days / 30);
            if (span.Days % 31 != 0)
                months += 1;
            return String.Format("about {0} {1} ago", months, months == 1 ? "month" : "months");
        }

        if (span.Days > 0)
            return String.Format("about {0} {1} ago", span.Days, span.Days == 1 ? "day" : "days");

        if (span.Hours > 0)
            return String.Format("about {0} {1} ago", span.Hours, span.Hours == 1 ? "hour" : "hours");

        if (span.Minutes > 0)
            return String.Format("about {0} {1} ago", span.Minutes, span.Minutes == 1 ? "minute" : "minutes");

        if (span.Seconds > 5)
            return String.Format("about {0} seconds ago", span.Seconds);

        if (span.Seconds <= 5)
            return "just now";

        return string.Empty;
    }
    public static string getTrGun(DateTime tarih)
    {
        string c = "";
        switch (tarih.DayOfWeek.ToString())
        {
            case "Sunday": c = "Pazar"; break;
            case "Monday": c = "Pazartesi"; break;
            case "Tuesday": c = "Salı"; break;
            case "Wednesday": c = "Çarşamba"; break;
            case "Thursday": c = "Perşembe"; break;
            case "Friday": c = "Cuma"; break;
            case "Saturday": c = "Cumartesi"; break;
            default: c = tarih.DayOfWeek.ToString(); break;
        }
        return c;
    }

    public static int CalculateAge(DateTime BirthDate)
    {
        int years = DateTime.Now.Year - BirthDate.Year;
       
        if (DateTime.Now.Month < BirthDate.Month ||
            (DateTime.Now.Month == BirthDate.Month &&
            DateTime.Now.Day < BirthDate.Day))
            years--;

        return years;
    }
    public static DateTime AddWeekDays(DateTime date, int daysToAdd)
    {
        int daysAdded = 0;
        while (daysAdded < daysToAdd)
        {
            if (!(date.DayOfWeek == DayOfWeek.Saturday || date.DayOfWeek == DayOfWeek.Sunday))
            {
                date = date.AddDays(1);
                daysAdded++;
            }
            else if (date.DayOfWeek == DayOfWeek.Saturday)
            {
                date = date.AddDays(2);
            }
            else if (date.DayOfWeek == DayOfWeek.Sunday)
            {
                date = date.AddDays(1);
            }
        }
        return date;
    }

    public static double DateDiff(string howtocompare, System.DateTime startDate, System.DateTime endDate)
    {
        double diff = 0;
        try
        {
            System.TimeSpan TS = new
                System.TimeSpan(startDate.Ticks - endDate.Ticks);
            #region converstion options
            switch (howtocompare.ToLower())
            {
                case "m":
                    diff = Convert.ToDouble(TS.TotalMinutes);
                    break;
                case "s":
                    diff = Convert.ToDouble(TS.TotalSeconds);
                    break;
                case "t":
                    diff = Convert.ToDouble(TS.Ticks);
                    break;
                case "mm":
                    diff = Convert.ToDouble(TS.TotalMilliseconds);
                    break;
                case "yyyy":
                    diff = Convert.ToDouble(TS.TotalDays / 365);
                    break;
                case "q":
                    diff = Convert.ToDouble((TS.TotalDays / 365) / 4);
                    break;
                default:
                    //d
                    diff = Convert.ToDouble(TS.TotalDays);
                    break;
            }
            #endregion
        }
        catch
        {
            diff = -1;
        }
        return diff;
    }

    public static String Language
    {
        get
        {
            if (HttpContext.Current.Session["Language"] == null || HttpContext.Current.Session["Language"].ToString() == "")
                return "tr-TR";
            else
                return HttpContext.Current.Session["Language"].ToString();
        }
        set
        {
            HttpContext.Current.Session["Language"] = value;
        }
    }

    public static string FormatDate(object dateObject, string format, object dayChange)
    {
        if (dayChange.ToString().Equals(""))
            return "";
        string dateString = int.Parse(dateObject.ToString()).ToString("00000000");
        // input strings are in 'host' format.  yyyyMMdd and hhmm
        DateTime temp = DateTime.ParseExact(dateString, "yyyyMMdd", null);
        if (dayChange.ToString() == "00")
            return "";
        else
            return "(Next Day)";
    }

    public static string FormatDateV2(object dateObject, string format, object dayChange)
    {
        string dateString = int.Parse(dateObject.ToString()).ToString("00000000");
        // input strings are in 'host' format.  yyyyMMdd and hhmm
        DateTime temp = DateTime.ParseExact(dateString, "yyyyMMdd", null);
        if (dayChange.ToString() == "00")
            return temp.ToString(format);
        else
            return temp.AddDays(1).ToString(format);
    }

    public static string FormatTime(object timeObject, string format)
    {
        string timeStr = timeObject.ToString();
        if (timeStr.Length == 7)
            timeStr = timeStr.Substring(3);
        timeStr = int.Parse(timeStr).ToString("0000");
        // input strings are in 'host' format.  yyyyMMdd and hhmm
        DateTime temp = DateTime.ParseExact(timeStr, "HHmm", null);
        return temp.ToString("HH:mm tt");
    }

    public static string TimeToSeries(object oUcusSure)
    {
        string TotalSn = oUcusSure.ToString();
        if (TotalSn.Equals(""))
            return "";

        string Language = MyCsharpHelper.Language;
        int saat = (int)(Convert.ToInt32(TotalSn) / 60);
        int dakika = Convert.ToInt32(TotalSn) % 60;
        string sDakika = dakika == 0 ? "" : dakika.ToString() + (Language.StartsWith("en") ? "min " : "dk ");
        return saat.ToString() + (Language.StartsWith("en") ? "h " : "sa ") + sDakika;
    }

    public static string TimeToSeries(object oUcusSure, string dil)
    {
        string TotalSn = oUcusSure.ToString();
        int saat = (int)(Convert.ToInt32(TotalSn) / 60);
        int dakika = Convert.ToInt32(TotalSn) % 60;
        string sDakika = dakika == 0 ? "" : dakika.ToString() + (Language.StartsWith("en") ? "min " : "dk ");
        return saat.ToString() + (dil.StartsWith("en") ? "h " : "sa ") + sDakika;
    }



}