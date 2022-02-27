using System;
using System.Collections.Generic;
using System.Text;

namespace Renderer
{
    struct Vector
    {
        public double X { get; set; }
        public double Y { get; set; }
        public double Z { get; set; }

        public Vector(double x, double y, double z)
        {
            X = x;
            Y = y;
            Z = z;
        }

        public Vector Add(double x, double y, double z)
        {
            return new Vector(X + x, Y + y, Z + z);
        }

        public Vector Add(Vector v)
        {
            return new Vector(X + v.X, Y + v.Y, Z + v.Z);
        }

        public Vector Subtract(double x, double y, double z)
        {
            return new Vector(X - x, Y - y, Z - z);
        }

        public Vector Subtract(Vector v)
        {
            return new Vector(X - v.X, Y - v.Y, Z - v.Z);
        }

        public Vector Multiply(double n)
        {
            return new Vector(X * n, Y * n, Z * n);
        }

        public Vector Multiply(Vector v)
        {
            return new Vector(X * v.X, Y * v.Y, Z * v.Z);
        }

        public double Length
        {
            get
            {
                return Math.Sqrt(X * X + Y * Y + Z * Z);
            }
        }

        public Vector RotateYP(double yaw, double pitch)
        {
            var yawRads = yaw * (Math.PI / 180.0);
            var pitchRads = pitch * (Math.PI / 180.0);
            var y = Y * Math.Cos(pitchRads) - Z * Math.Sin(pitchRads);
            var z = Y * Math.Sin(pitchRads) + Z * Math.Cos(pitchRads);
            var x = X * Math.Cos(yawRads) + z * Math.Sin(yawRads);
            z = -X * Math.Sin(yawRads) + z * Math.Cos(yawRads);
            return new Vector(x, y, z);
        }

        public Vector Normalize()
        {
            var length = Length;
            return new Vector(X / length, Y / length, Z / length);
        }

        public double Distance(Vector vec)
        {
            var dx = X - vec.X;
            var dy = Y - vec.Y;
            var dz = Z - vec.Z;
            return Math.Sqrt(dx * dx + dy * dy + dz * dz);
        }

        public static double DotVector(Vector a, Vector b)
        {
            return a.X * b.X + a.Y * b.Y + a.Z * b.Z;
        }
    }
}
