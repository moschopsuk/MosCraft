using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace Voxel.Engine.Rendering.Models
{
    class ModelImporter
    {
        static public Model LoadOBJ(string fileName)
        {
            var reader = new StreamReader(fileName);
            string line;
            char[] splitChars = { ' ' };
            while ((line = reader.ReadLine()) != null)
            {
                line = line.Trim(splitChars);
                line = line.Replace("  ", " ");
                string[] parameters = line.Split(splitChars);

                switch (parameters[0])
                {
                }
            }

        }
    }
}
