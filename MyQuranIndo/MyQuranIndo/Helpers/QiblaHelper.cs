﻿using MyQuranIndo.Messages;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Xamarin.Essentials;
namespace MyQuranIndo.Helpers
{
    public static class DistanceCalculator
    {
        const double kDegreesToRadians = Math.PI / 180.0;
        const double kRadiansToDegrees = 180.0 / Math.PI;

        public static double Bearing(Location position, Location location)
        {
            double fromLong = position.Longitude * kDegreesToRadians;
            double toLong = location.Longitude * kDegreesToRadians;
            double fromLat = position.Latitude * kDegreesToRadians;
            double toLat = location.Latitude * kDegreesToRadians;

            double dlon = toLong - fromLong;
            double y = Math.Sin(dlon) * Math.Cos(toLat);
            double x = Math.Cos(fromLat) * Math.Sin(toLat) - Math.Sin(fromLat) * Math.Cos(toLat) * Math.Cos(dlon);

            double direction = Math.Atan2(y, x);

            // convert to degrees
            direction = direction * kRadiansToDegrees;
            // normalize
            double fraction = modf(direction + 360.0, direction);
            direction += fraction;

            if (direction > 360)
            {
                direction -= 360;
            }

            return direction;
        }

        private static double modf(double orig, double ipart)
        {
            return orig - (Math.Floor(orig));
        }
    }
}
