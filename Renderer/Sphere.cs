using System;
using System.Collections.Generic;
using System.Text;

namespace Renderer
{
    class Sphere : Entity
    {
        private readonly double radius;
        private readonly Color color;

        public Sphere(Location location, double radius, Color color, double reflectivity, double emission) : base(location, reflectivity, emission)
        {
            this.radius = radius;
            this.color = color;
        }

        public override Color GetColor(Vector vec)
        {
            return this.color;
        }

        public override Vector? CalculateIntersection(Ray ray)
        {
            var t = Vector.DotVector(location.position.Subtract(ray.origin), ray.direction);
            var p = ray.origin.Add(ray.direction.Multiply(t));
            var y = location.position.Subtract(p).Length;

            if (y < radius)
            {
                var x = Math.Sqrt(radius * radius - y * y);
                var t1 = t - x;
                if (t1 > 0)
                {
                    return ray.origin.Add(ray.direction.Multiply(t1));
                }
            }

            return null;
        }

        public override Vector GetNormalAt(Vector vec)
        {
            return vec.Subtract(location.position).Normalize();
        }
    }
}
