using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using OpenTK;
using OpenTK.Graphics;
using Voxel.Engine.Rendering.Text;

namespace Voxel.Engine.Rendering
{
    class SpriteBatch
    {

        #region DrawText Methods

        public void DrawText(BitmapFont font, string s, Vector2 position)
        {
            DrawText(font, s, position, Color4.White);
        }

        public void DrawText(BitmapFont font, string s, Vector2 position, Color4 color)
        {
        }

        #endregion
    }
}
