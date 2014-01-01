using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Text;
using OpenTK;
using Voxel.Engine.Rendering.Fonts;
using PixelFormat = System.Drawing.Imaging.PixelFormat;

namespace Voxel.Engine.Rendering.Text
{
    class BitmapFont
    {
        private Texture2D _texture;
        private readonly List<Glyph> _glyphs;
        private Font _font;

        public BitmapFont(string fileName, int size)
        {       
            _glyphs = new List<Glyph>();
            _font = new Font(LoadFont(fileName), size);

            GenerateSheet();
        }

        public Texture Texture
        {
            get { return _texture;  }
        }

        private FontFamily LoadFont(string fileName)
        {
            var fonts = new PrivateFontCollection();//here is where we assing memory space to myFonts
            fonts.AddFontFile(fileName);//we add the full path of the ttf file
            return fonts.Families[0];
        }

        private void GenerateSheet()
        {
            float x = 0, y = 0;
            const int padding = 4;
            int lineHeight = _font.Height;
            var bmp = new Bitmap(512, 512, PixelFormat.Format32bppArgb);
            var color = new SolidBrush(Color.White);

            using (var g = Graphics.FromImage(bmp))
            {
                g.SmoothingMode = SmoothingMode.AntiAlias;
			    g.InterpolationMode = InterpolationMode.HighQualityBilinear;
			    g.TextRenderingHint = TextRenderingHint.AntiAliasGridFit;
                g.Clear(Color.Transparent);

                for (int c = 0; c < 256; c++)
                {
                    var size = g.MeasureString(((char)c).ToString(), _font, new Size(int.MaxValue, int.MaxValue));

                    if (x + size.Width + padding >= bmp.Width)
                    {
                        y += lineHeight + padding;
                        x = 0;
                    }

                    g.DrawString(((char)c).ToString(), _font, color, new Point((int)x, (int)y));

                    var glyph = new Glyph((char)c, new Vector2(x, y) / 512, new Vector2(size.Width, size.Height) / 512, new Vector2(size.Width, size.Height));
                    _glyphs.Add(glyph);

                    x += size.Width + padding;
                }

                g.Flush();
                _texture = new Texture2D(bmp, false);
            }
        }
    }
}
