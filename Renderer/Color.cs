using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;

namespace Renderer
{
    public struct Color
    {
        public double R;
        public double G;
        public double B;

        public Color(string hex)
        {
            int rgb = Convert.ToInt32(Regex.Replace(hex, @"#", ""), 16);
            R = ((rgb >> 16) & 255) / 255.0;
            G = ((rgb >> 8) & 255) / 255.0;
            B = (rgb & 255) / 255.0;
        }

        public Color(double r, double g, double b)
        {
            R = Math.Max(0, Math.Min(1, r));
            G = Math.Max(0, Math.Min(1, g));
            B = Math.Max(0, Math.Min(1, b));
        }

        public double Luminance
        {
            get
            {
                return R * 0.2126 + G * 0.7152 + B * 0.0722;
            }
        }

        public Color Add(Color c)
        {
            return new Color(R + c.R, G + c.G, B + c.B);
        }

        public Color Add(double n)
        {
            return new Color(R + n, G + n, B + n);
        }

        public Color Multiply(double f)
        {
            return new Color(R * f, G * f, B * f);
        }

        public static Color Lerp(Color a, Color b, double factor)
        {
            var dr = b.R - a.R;
            var dg = b.G - a.G;
            var db = b.B - a.B;

            return new Color(a.R + (dr * factor), a.G + (dg * factor), a.B + (db*factor));
        }
    }
}
