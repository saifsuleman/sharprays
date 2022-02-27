using System;
using System.Collections.Generic;
using System.Text;

namespace Renderer
{
    struct Location
    {
        public Vector position;
        public double yaw;
        public double pitch;

        public Location(Vector position, double yaw, double pitch)
        {
            this.position = position;
            this.yaw = yaw;
            this.pitch = pitch;
        }
    }
}
