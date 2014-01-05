using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Voxel.Engine.Rendering.Models
{
    public class Face
    {
        public int VertexIndex { get; private set; }
        public int NomralIndex { get; private set; }
        public int TextureIndex { get; private set; }

        public Face(int v, int n, int t)
        {
            VertexIndex = v;
            NomralIndex = n;
            TextureIndex = t;
        }
    }
}
