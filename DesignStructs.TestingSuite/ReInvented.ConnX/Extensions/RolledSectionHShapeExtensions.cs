using ReInvented.Sections.Domain.Models;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Media.Media3D;


namespace ReInvented.ConnX.Extensions
{
    public static class RolledSectionHShapeExtensions
    {
        public static List<Point> GetSectionProfile(this RolledSectionHShape section)
        {
            double bf = section.Bf; double h = section.H; double tf = section.Tf;
            double tw = section.Tw; double cy = section.Cy;

            // Shift the profile so that C.G. is at (0,0)
            List<Point> profile = new List<Point>()
            {
                new Point(0,0),
                new Point(bf, 0),
                new Point(bf, tf),
                new Point(bf - ((bf - tf) / 2), tf),
                new Point(bf - ((bf - tf) / 2), h - tf),
                new Point(bf, h - tf),
                //new Point(bf, h),
                //new Point(0, h),
                //new Point(0, h - tf),
                //new Point((bf - tf) / 2, h - tf),
                //new Point((bf - tf) / 2, tf),
                //new Point(0, tf),
                //new Point(0, 0)
            };

            return profile.Select(p => new Point(p.X, p.Y)).ToList();
        }
    }
}
