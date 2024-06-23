﻿namespace ServiceApi.Helpers;
public class Utilities
{
    public static string CalcAwayDuration(DateTime lastLoginTime)
    {
        var duration = DateTime.Now - lastLoginTime;

        if (duration.Days > 7)
            return "";

        if (duration.Days > 0)
            return $"{duration.Days}d";

        if (duration.Hours > 0)
            return $"{duration.Hours}h";

        if (duration.Minutes > 0)
            return $"{duration.Minutes}m";

        return "";
    }
}