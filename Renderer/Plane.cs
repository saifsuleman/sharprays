using System;
using System.Collections.Generic;
using System.Text;

namespace Renderer
{
    class Plane : Entity
    {
        private static readonly Color BLACK = new Color(0.01, 0.01, 0.01);
        private static readonly Color WHITE = new Color(0.2, 0.2, 0.2);

        public Plane(double reflectivity, double emission) 
            : base(new Location(new Vector(0, -5.75, 0), 0, 0), reflectivity, emission) {}

        public override Vector? CalculateIntersection(Ray ray)
        {
            var t = -(ray.origin.Y - location.position.Y) / ray.direction.Y;
            if (t <= 0 || t > 1000)
            {
                return null;
            }
            return ray.origin.Add(ray.direction.Multiply(t));
        }

        public override Color GetColor(Vector vec)
        {
            int x = (int) Math.Abs(Math.Floor(vec.X * 0.1));
            int z = (int) Math.Abs(Math.Floor(vec.Z * 0.1));
            return (x % 2) == (z % 2) ? BLACK : WHITE;
        }

        public override Vector GetNormalAt(Vector vec)
        {
            return new Vector(0, 1, 0);
        }
    }
}
