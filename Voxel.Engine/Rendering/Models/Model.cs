using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using OpenTK;

namespace Voxel.Engine.Rendering.Models
{
    public class Model
    {
        public List<Vector3> Vertices { get; private set; }
        public List<Vector3> Normals { get; private set; }
        public List<Vector2> TexCords { get; private set; }
        public List<Face> Faces { get; private set; }

        public Model()
        {
            Vertices = new List<Vector3>();
            Normals = new List<Vector3>();
            TexCords = new List<Vector2>();
            Faces = new List<Face>();
        }
    }
}
