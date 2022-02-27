using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;

namespace Renderer
{
    static class Renderer
    {
        private static readonly int MAX_BOUNCES = 5;
        private static readonly double GLOBAL_ILLUMINATION = 0.3;
        private static readonly double SKY_EMISSION = 0.5;

        public static Microsoft.Xna.Framework.Color[] Render(Scene scene, int width, int height, double resolution)
        {
            var buf = new Microsoft.Xna.Framework.Color[width * height];

            var blockSize = (int)(1.0 / resolution);

            for (int y = 0; y < height; y += blockSize)
            {
                for (int x = 0; x < width; x += blockSize)
                {
                    (var u, var v) = GetNormalizedScreenCoordinates(x, y, width, height);
                    var pixel = ComputePixel(scene, u, v);
                    var color = new Microsoft.Xna.Framework.Color((int)(pixel.color.R * 255.0), (int)(pixel.color.G * 255.0), (int)(pixel.color.B * 255.0));
                    for (int i = 0; i < blockSize; i++)
                    {
                        for (int j = 0; j < blockSize; j++)
                        {
                            buf[(x + i) + (y + j) * width] = color;
                        }
                    }
                }
            }

            return buf;
        }

        public static Bitmap RenderBitmap(Scene scene, int width, int height)
        {
            var bitmap = new Bitmap(width, height);
            int i = 0;
            for (int x = 0; x < width; x++)
            {
                for (int y = 0; y < height; y++)
                {
                    (var u, var v) = GetNormalizedScreenCoordinates(x, y, width, height);
                    var pixel = ComputePixel(scene, u, v);
                    var color = System.Drawing.Color.FromArgb((int)(pixel.color.R * 255), (int)(pixel.color.G * 255), (int)(pixel.color.B * 255));
                    bitmap.SetPixel(x, y, color);
                    System.Diagnostics.Debug.WriteLine("Pixel: " + ++i + "/" + (width * height));
                }
            }
            return bitmap;
        }

        private static (double, double) GetNormalizedScreenCoordinates(double x, double y, double width, double height)
        {
            if (width > height)
            {
                var u = (x - width / 2.0 + height / 2.0) / height * 2.0 - 1.0;
                var v = -(y / height * 2.0 - 1.0);
                return (u, v);
            }
            else
            {
                var u = x / width * 2.0 - 1.0;
                var v = -((y - height / 2 + width / 2) / width * 2 - 1);
                return (u, v);
            }
        }

        private static Pixel ComputePixel(Scene scene, double u, double v)
        {
            var eyePos = new Vector(0, 0, -1 / Math.Tan(20 * (Math.PI / 180)));
            var rayDirection = new Vector(u, v, 0).Subtract(eyePos).Normalize().RotateYP(scene.camera.yaw, scene.camera.pitch);
            var ray = new Ray(eyePos.Add(scene.camera.position), rayDirection);
            var hit = ray.Cast(scene);
            return hit != null ? ComputePixelHit(scene, hit, MAX_BOUNCES) : GetSkyboxPixel(scene, rayDirection);
        }

        private static Pixel GetSkyboxPixel(Scene scene, Vector vec)
        {
            var color = scene.GetSkyboxColor(vec.Normalize());
            var emission = color.Luminance * SKY_EMISSION;
            return new Pixel(color, emission);
        }

        private static Pixel ComputePixelHit(Scene scene, RayHit hit, int bounces)
        {
            var reflectionVector = hit.ray.direction.Subtract(hit.normal.Multiply(2 * Vector.DotVector(hit.ray.direction, hit.normal)));
            var reflectionOrigin = hit.position.Add(reflectionVector.Multiply(0.001));
            var reflectionRay = new Ray(reflectionOrigin, reflectionVector);
            var reflectionHit = bounces > 0 ? reflectionRay.Cast(scene) : null;
            var reflection = reflectionHit == null ? GetSkyboxPixel(scene, reflectionVector) : ComputePixelHit(scene, reflectionHit, bounces - 1);

            var specularBrightness = GetSpecularBrightness(scene, hit);
            var reflectivity = hit.entity.reflectivity;
            var emission = hit.entity.emission;
            var brightness = GetDiffuseBrightness(scene, hit);
            if (reflectionHit != null)
            {
                brightness += GetDiffuseBrightness(scene, reflectionHit) * reflectionHit.entity.reflectivity * reflection.color.Luminance;
                specularBrightness += GetSpecularBrightness(scene, reflectionHit) * reflectionHit.entity.reflectivity * reflection.color.Luminance;
            }

            var entityColor = hit.entity.GetColor(hit.position);
            var color = Color.Lerp(entityColor, reflection.color, reflectivity)
                .Multiply(brightness)
                .Add(specularBrightness)
                .Add(entityColor.Multiply(emission))
                .Add(reflection.color.Multiply(reflection.emission * reflectivity));

            return new Pixel(color, Math.Min(1, emission + reflection.emission * reflectivity + specularBrightness));
        }

        private static double GetDiffuseBrightness(Scene scene, RayHit hit)
        {
            var ray = new Ray(scene.light.position, hit.position.Subtract(scene.light.position).Normalize());
            var lightHit = ray.Cast(scene);
            return lightHit != null && lightHit.entity != hit.entity
                ? GLOBAL_ILLUMINATION
                : Math.Max(GLOBAL_ILLUMINATION, scene.light.intensity * Math.Min(1, Vector.DotVector(hit.normal, scene.light.position.Subtract(hit.position))));
        }

        private static double GetSpecularBrightness(Scene scene, RayHit hit)
        {
            var cameraDirection = scene.camera.position.Subtract(hit.position).Normalize();
            var lightDirection = hit.position.Subtract(scene.light.position).Normalize();
            var lightReflectionVector = lightDirection.Subtract(hit.normal.Multiply(2 * Vector.DotVector(lightDirection, hit.normal)));
            var specularFactor = Math.Max(0, Math.Min(1, Vector.DotVector(lightReflectionVector, cameraDirection)));
            return specularFactor * specularFactor * hit.entity.reflectivity * scene.light.intensity;
        }
    }
}
