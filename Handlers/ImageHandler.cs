using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace MenuCore
{
    public class ImageHandler
    {
       
        public static Texture2D LoadImageFromEmbeddedResource(string path, int width, int height)
        {
            Texture2D image = new Texture2D(width, height);

            var assembly = Assembly.GetExecutingAssembly();
            using (var stream = assembly.GetManifestResourceStream(path))
            {
                byte[] imageData = new byte[stream.Length];
                stream.Read(imageData, 0, (int)stream.Length);
                image.LoadImage(imageData);
                image.wrapMode = TextureWrapMode.Clamp;
            }

            return image;
        }
        public static Texture2D LoadImageFromEmbeddedResource(string path)
        {
            return LoadImageFromEmbeddedResource(path, 1, 1);
        }
    }
}
