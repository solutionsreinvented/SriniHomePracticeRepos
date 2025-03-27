using System;
using System.Collections.Generic;
using System.Linq;

using ReInvented.DroopModifier.Interfaces;
using ReInvented.StaadPro.Interop.Entities;


namespace ReInvented.DroopModifier.Extensions
{
    public static class IEnumerableExtensions
    {
        public static Node ModifyNode(this IEnumerable<IReading> readings, Node origin, Node target)
        {
            double atRadius = target.Radius(origin);
            double atAngle = Node.PlanAngleIn360DegreesOf(target, origin);

            IEnumerable<IReading> tops = readings.Tops(atAngle);
            IEnumerable<IReading> bottoms = readings.Bottoms(atAngle);

            IReading tl = tops.Left(atRadius);
            IReading tr = tops.Right(atRadius);
            IReading br = bottoms.Right(atRadius);
            IReading bl = bottoms.Left(atRadius);

            double sRadialDelta = tl == bl ? tl.DroopDelta : tl.DroopDelta + ((bl.DroopDelta - tl.DroopDelta) / (bl.Angle - tl.Angle) * (atAngle - tl.Angle));
            double eRadialDelta = tr == br ? tr.DroopDelta : tr.DroopDelta + ((br.DroopDelta - tr.DroopDelta) / (br.Angle - tr.Angle) * (atAngle - tr.Angle));

            double sAngularDelta = tl == tr ? tl.DroopDelta : tl.DroopDelta + ((tr.DroopDelta - tl.DroopDelta) / (tr.Radius - tl.Radius) * (atRadius - tl.Radius));
            double eAngularDelta = bl == br ? bl.DroopDelta : bl.DroopDelta + ((br.DroopDelta - bl.DroopDelta) / (br.Radius - bl.Radius) * (atRadius - bl.Radius));

            double radialDelta = tl == tr ? sRadialDelta : sRadialDelta + ((eRadialDelta - sRadialDelta) / (tr.Radius - tl.Radius) * (atRadius - tl.Radius));
            double angularDelta = bl == tl ? sAngularDelta : sAngularDelta + ((eAngularDelta - sAngularDelta) / (bl.Angle - tl.Angle) * (atAngle - tl.Angle));

            double avgDelta = (radialDelta + angularDelta) / 2;

            target.Y += avgDelta / 1000;

            return target;
        }

        private static IReading Left(this IEnumerable<IReading> topsOrBottoms, double atRadius)
        {
            IReading topLeft = topsOrBottoms.Where(r => Math.Round(r.Radius, 3) <= Math.Round(atRadius, 3)).OrderBy(r => atRadius - r.Radius).FirstOrDefault();

            return topLeft;
        }

        private static IReading Right(this IEnumerable<IReading> topsOrBottoms, double atRadius)
        {
            IReading topLeft = topsOrBottoms.Where(r => Math.Round(r.Radius, 3) >= Math.Round(atRadius, 3)).OrderBy(r => r.Radius - atRadius).FirstOrDefault();

            return topLeft;
        }


        private static IEnumerable<IReading> Tops(this IEnumerable<IReading> readings, double atAngle)
        {
            IEnumerable<IReading> tops = readings.Where(r => r.Angle <= atAngle).GroupBy(r => r.Angle - atAngle).LastOrDefault();

            if (tops == null) { tops = readings.GroupBy(r => r.Angle - atAngle).LastOrDefault(); }

            return tops;
        }

        private static IEnumerable<IReading> Bottoms(this IEnumerable<IReading> readings, double atAngle)
        {
            IEnumerable<IReading> bottoms = readings.Where(r => r.Angle >= atAngle).GroupBy(r => atAngle - r.Angle).FirstOrDefault();

            if (bottoms == null) { bottoms = readings.GroupBy(r => atAngle - r.Angle).FirstOrDefault(); }

            return bottoms;
        }
    }
}
