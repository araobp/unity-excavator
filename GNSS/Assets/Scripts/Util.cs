using System;

public static class Util
{
    static double Deg2Rad(double deg)
    {
        return deg * Math.PI / 180;
    }

    public static float Distance(double lat1, double lon1, double lat2, double lon2)
    {
        double R = 6371;

        lat1 = Deg2Rad(lat1);
        lon1 = Deg2Rad(lon1);

        lat2 = Deg2Rad(lat2);
        lon2 = Deg2Rad(lon2);

        double dlat = (lat2 - lat1);
        double dlon = (lon2 - lon1);

        double a = Math.Pow(Math.Sin(dlat / 2), 2) + Math.Cos(lat1) * Math.Cos(lat2) * Math.Pow(Math.Sin(dlon / 2), 2);
        double c = 2 * Math.Atan2(Math.Sqrt(a), Math.Sqrt(1 - a));
        double d = c * R;

        return (float)d * 1000;
    }

    public static float DistanceX(double lat1, double lon1, double lon2)
    {
        return Distance(lat1, lon1, lat1, lon2);
    }

    public static float DistanceY(double lat1, double lon1, double lat2)
    {
        return Distance(lat1, lon1, lat2, lon1);
    }
}
