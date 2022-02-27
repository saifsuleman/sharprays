using System;
using System.Collections.Generic;
using System.Text;

namespace Renderer
{
    class Ray
    {
        public readonly Vector origin;
        public readonly Vector direction;

        public Ray(Vector origin, Vector direction)
        {
            this.origin = origin;
            this.direction = direction;
        }

        public RayHit Cast(Scene scene)
        {
            RayHit closest = null;
            double closestDistance = double.PositiveInfinity;

            foreach (var entity in scene.entities)
            {
                var intersection = entity.CalculateIntersection(this);

                if (intersection == null) {
                    continue;
                }

                var distance = this.origin.Distance((Vector)intersection);
                if (distance < closestDistance)
                {
                    closest = new RayHit(this, entity, (Vector)intersection, entity.GetNormalAt((Vector)intersection));
                    closestDistance = distance;
                }
            }

            return closest;
        }
    }

    class RayHit
    {
        public readonly Ray ray;
        public readonly Entity entity;
        public readonly Vector position;
        public readonly Vector normal;
        public readonly double distance;

        public RayHit(Ray ray, Entity entity, Vector position, Vector normal)
        {
            this.ray = ray;
            this.entity = entity;
            this.position = position;
            this.normal = normal;
            distance = ray.origin.Distance(position);
        }
    }
}
