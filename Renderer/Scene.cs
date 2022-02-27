using System;
using System.Collections.Generic;
using System.Drawing;

namespace Renderer
{
    class Scene
    {
        public readonly List<Entity> entities;
        public Location camera;
        public LightSource light;

        public readonly Bitmap bitmap;

        public Scene(Location camera, LightSource light)
        {
            this.camera = camera;
            this.light = light;
            this.entities = new List<Entity>();

            bitmap = new Bitmap(Image.FromFile("Sky.jpg"));
        }

        public void AddEntity(Entity entity)
        {
            this.entities.Add(entity);
        }

        public Color GetSkyboxColor(Vector vec)
        {
            // TODO
            var u = 0.5 + Math.Atan2(vec.Z, vec.X) / (2.0 * Math.PI);
            var v = 0.5 - Math.Asin(vec.Y) / Math.PI;
            var x = (int)(u * ((double)bitmap.Width - 1));
            var y = (int)(v * ((double)bitmap.Height - 1));
            if (x < 0 || x >= bitmap.Width || y < 0 || y >= bitmap.Height)
            {
                System.Diagnostics.Debug.WriteLine("x: " + x + " y: " + y);
                return new Color(0, 0, 0);
            }
            var pixel = bitmap.GetPixel(x, y);
            return new Color(pixel.R / 255.0, pixel.G / 255.0, pixel.B / 255.0);
        }
    }
}
