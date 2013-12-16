using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenTK.Graphics.OpenGL;

namespace Voxel.Engine
{
    public class Viewport
    {
        public Viewport(int width, int height)
        {
            SetSize(width, height);
        }

        public void SetSeize(Size size)
        {
            SetSize(size.Width, size.Height);    
        }

        public void SetSize(int width, int height)
        {
            Width = width;
            Height = height;
            AspectRatio = Width / (float)Height;

            GL.Viewport(0, 0, Width, Height);
        }

        public float AspectRatio { get; private set; }
        public int Width { get; private set; }
        public int Height { get; private set; }
    }
}
