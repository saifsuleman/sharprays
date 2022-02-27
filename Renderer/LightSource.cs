using System;
using System.Collections.Generic;
using System.Text;

namespace Renderer
{
    struct LightSource
    {
        public Vector position;
        public double intensity;

        public LightSource(Vector position, double intensity)
        {
            this.position = position;
            this.intensity = intensity;
        }
    }
}
