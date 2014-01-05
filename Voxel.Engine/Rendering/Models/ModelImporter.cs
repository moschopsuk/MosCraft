using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using OpenTK;

namespace Voxel.Engine.Rendering.Models
{
    class ModelImporter
    {
        static public Model LoadObj(string fileName)
        {
            var model = new Model();
            var reader = new StreamReader(fileName);
            string line;

            while ((line = reader.ReadLine()) != null)
            {
                string[] p = line.Split(' ');
                float x, y, z, u, v;

                switch (p[0])
                {
                    //Vertex
                    case "v":
                        x = float.Parse(p[1]);
                        y = float.Parse(p[2]);
                        z = float.Parse(p[3]);
                        model.Vertices.Add(new Vector3(x, y, z));
                        break;
                    
                    //Vertex Normal
                    case "vn":
                        x = float.Parse(p[1]);
                        y = float.Parse(p[2]);
                        z = float.Parse(p[3]);
                        model.Normals.Add(new Vector3(x, y, z));
                        break;

                    //Vertex texture coord
                    case "vt":
                        u = float.Parse(p[1]);
                        v = float.Parse(p[2]);
                        model.TexCords.Add(new Vector2(u, v));
                        break;
                    
                    //Face
                    case "f":
                        for (int i = 1; i <= 3; i++)
                        {
                            string[] face = p[i].Split('/');
                            int vi = int.Parse(face[0]) - 1;
                            int ti = int.Parse(face[1]) - 1;
                            int ni = int.Parse(face[2]) - 1;
                            model.Faces.Add(new Face(vi, ti, ni));
                        }
                        break;

                    default:
                        continue;

                }
            }

            reader.Close();
            return model;
        }
    }
}
