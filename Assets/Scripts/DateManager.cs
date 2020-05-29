using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Globalization;

public static class DateManager
{
    public static DateTime GetNow => DateTime.Now;

    public static int GetDifferenceDays(DateTime a, DateTime b)
    {
        var difference = a - b;
        float count = (int)difference.TotalSeconds / 86400f;
        int days = Mathf.Abs(Mathf.FloorToInt(count));
        return days;
    }

    public static DateTime GetDateWithPlusDays(DateTime date, int days)
    {
        return date.AddDays(days);
    }

    public static DateTime GetDateWithMinusDays(DateTime date, int days)
    {
        return date.AddDays(-days);
    }

    public static string GetStringFromPattern(DateTime date, string pattern)
    {
        return date.ToString(pattern, CultureInfo.InvariantCulture);
    }
}

public static class DateAdapter
{
    public static DateTime FromString(string dateString)
    {
        return DateTime.Parse(dateString, CultureInfo.InvariantCulture);
    }

    public static string ToString(DateTime date)
    {
        return date.ToString();
    }
}
