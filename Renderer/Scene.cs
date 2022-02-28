using System;
using System.Collections.Generic;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.PixelFormats;
using SixLabors.ImageSharp.Processing;

namespace Renderer
{
    class Scene
    {
        public readonly List<Entity> entities;
        public Location camera;
        public LightSource light;

        private readonly Image<Rgba32> image;

        public Scene(Location camera, LightSource light)
        {
            this.camera = camera;
            this.light = light;
            this.entities = new List<Entity>();

            image = Image.Load<Rgba32>("Sky.jpg");
        }

        public void AddEntity(Entity entity)
        {
            this.entities.Add(entity);
        }

        public Color GetSkyboxColor(Vector vec)
        {
            var u = 0.5 + Math.Atan2(vec.Z, vec.X) / (2.0 * Math.PI);
            var v = 0.5 - Math.Asin(vec.Y) / Math.PI;
            var x = (int)(u * ((double)image.Width - 1));
            var y = (int)(v * ((double)image.Height - 1));
            if (x < 0 || x >= image.Width || y < 0 || y >= image.Height)
            {
                System.Diagnostics.Debug.WriteLine("x: " + x + " y: " + y);
                return new Color(0, 0, 0);
            }
            var pixel = image[x, y];
            return new Color(pixel.R / 255.0, pixel.G / 255.0, pixel.B / 255.0);
        }
    }
}
