using System;
using System.Collections.Generic;
using System.Text;

namespace Renderer
{
    abstract class Entity
    {
        public Location location;
        public double reflectivity;
        public double emission;

        public Entity(Location location, double reflectivity, double emission)
        {
            this.location = location;
            this.reflectivity = reflectivity;
            this.emission = emission;
        }

        public abstract Color GetColor(Vector vec);

        public abstract Vector? CalculateIntersection(Ray ray);

        public abstract Vector GetNormalAt(Vector vec);
    }
}
