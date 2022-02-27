using System;
using System.Collections.Generic;
using System.Text;

namespace Renderer
{
    public struct Pixel
    {
        public Color color;
        public double emission;

        public Pixel(Color color, double emission)
        {
            this.color = color;
            this.emission = emission;
        }
    }
}
