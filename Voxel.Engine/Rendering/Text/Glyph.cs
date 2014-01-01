using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using OpenTK;

namespace Voxel.Engine.Rendering.Text
{
    public class Glyph
    {
        public Glyph(char c, Vector2 uvPosition, Vector2 uvSize, Vector2 size)
        {
            C = c;
            UvPosition = uvPosition;
            UvSize = uvSize;
            Size = size;
        }

        public Glyph()
        {

        }

        public override string ToString()
        {
            return string.Format("{0}", C);
        }

        public char C;
        public Vector2 UvPosition;
        public Vector2 UvSize;
        public Vector2 Size;
    }
}
