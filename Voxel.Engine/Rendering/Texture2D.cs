using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.Linq;
using System.Text;
using OpenTK.Graphics.OpenGL;
using GLPixelFormat = OpenTK.Graphics.OpenGL.PixelFormat;

namespace Voxel.Engine.Rendering
{
    /// <summary> This class encapsulates an OpenGl 2D Texture using a .NET Bitmap </summary>
    public class Texture2D : Texture
    {
        #region Fields
        List<Bitmap> imageMipMapped = new List<Bitmap>();
        #endregion

        #region Properties
        /// <summary> Gets or Sets the <c>Bitmap</c> used by the texture. </summary>
        public Bitmap Picture
        {
            get
            {
                if (imageMipMapped.Count > 0)
                    return imageMipMapped[0];
                return null;
            }
            set
            {
                imageMipMapped.Clear();
                if (value == null)
                    return;
                var img = new Bitmap(value);
                imageMipMapped.Add(img);
                if (_mipmap)
                    for (int size = img.Width / 2; size > 0; size /= 2)
                    {
                        var smallImg = new Bitmap(size, size, System.Drawing.Imaging.PixelFormat.Format32bppArgb);
                        using (var g = Graphics.FromImage(smallImg))
                        {
                            g.DrawImage(img, 0, 0, size, size);
                        }
                        imageMipMapped.Add(smallImg);
                        //System.Diagnostics.Debug.WriteLine(smallImg.Bitmap.GetPixel(size / 4, size / 4).A);
                    }
                SendToGL();
            }
        }

        public override TextureTarget TextureType { get { return TextureTarget.Texture2D; } }
        #endregion

        #region Constructors
        /// <summary> Alocate an OpenGL texture index without using an image </summary>
        public Texture2D() : this(null, true) { }
        /// <summary> Alocate an OpenGL texture index without using an image </summary>
        /// <param name="mipmap"> Indicate whether to mipmap </param>
        public Texture2D(bool mipmap) : this(null, mipmap) { }
        /// <summary> Alocate an OpenGL texture index using the provided Bitmap </summary>
        /// <param name="picture">The Bitmap to convert to a texture</param>
        public Texture2D(Bitmap picture) : this(picture, true) { }
        /// <summary> Alocate an OpenGL texture index using the provided Bitmap </summary>
        /// <param name="picture">The Bitmap to convert to a texture</param>
        /// <param name="mipmap"> Indicate whether to mipmap </param>
        public Texture2D(Bitmap picture, bool mipmap)
            : base(mipmap)
        {
            Picture = picture;
        }
        #endregion

        #region Methods
        /// <summary> Turn 2D textures off </summary>
        public static void Disable2DTextures()
        {
            GL.ActiveTexture(TextureUnit.Texture0);
            GL.Disable(EnableCap.Texture2D);
        }

        /// <summary> Initializes a blank texture for memory alocation </summary>
        /// <remarks> Used for framebuffers or dynamically writing to various parts of the texture </remarks>
        public void InitializeBlank(int width, int height)
        {
            InitializeBlank(width, height, PixelInternalFormat.Rgba8, GLPixelFormat.Rgba, PixelType.UnsignedByte);
        }
        /// <summary> Initializes a blank texture for memory alocation </summary>
        /// <remarks> Used for framebuffers or dynamically writing to various parts of the texture </remarks>
        public void InitializeBlank(int width, int height, PixelInternalFormat pixelInternalFormat, GLPixelFormat pixelFormat, PixelType pixelType)
        {
            //Initialize();
            //GL.PushAttrib(AttribMask.TextureBit | AttribMask.EnableBit);
            //GL.Enable(TextureTypeEnabled);
            GL.TexImage2D(TextureType, 0, pixelInternalFormat, width, height, 0, pixelFormat, pixelType, IntPtr.Zero);
            //GL.PopAttrib();
        }

        /// <summary> Sends the Bitmap data to OpenGL </summary>
        /// <remarks> Side Effect: 2D Textures are enabled </remarks>
        protected override void SendToGL()
        {
            var pixelFormat = GLPixelFormat.Bgra;
            if (Picture != null && Picture.PixelFormat == System.Drawing.Imaging.PixelFormat.Format24bppRgb)
                pixelFormat = GLPixelFormat.Bgr;
            SendToGL(pixelFormat);
        }

        /// <summary> Sends the Bitmap data to OpenGL </summary>
        /// <remarks> Side Effect: 2D Textures are enabled </remarks>
        /// <param name="pixelFormat">BGRA or BGR (GDI flips the byte order for some reason)</param>
        protected void SendToGL(GLPixelFormat pixelFormat)
        {
            Initialize();
            GL.PushAttrib(AttribMask.TextureBit);
            GL.Enable(TextureTypeEnabled);
            if (_mipmap)
            {
                for (int i = 0; i < imageMipMapped.Count; i++)
                {
                    var rect = new Rectangle(0, 0, imageMipMapped[i].Width, imageMipMapped[i].Height);
                    var bitmapData = imageMipMapped[i].LockBits(rect, ImageLockMode.ReadOnly, System.Drawing.Imaging.PixelFormat.Format32bppArgb);

                    GL.TexImage2D(TextureType, i, PixelInternalFormat.Rgba, imageMipMapped[i].Width,
                        imageMipMapped[i].Height, 0, pixelFormat, PixelType.UnsignedByte, bitmapData.Scan0);
                }

                GL.TexParameter(TextureType, TextureParameterName.TextureMinFilter, (int)All.LinearMipmapLinear);
                GL.TexParameter(TextureType, TextureParameterName.TextureMagFilter, (int)All.LinearMipmapLinear);
            }
            else
            {
                GL.TexImage2D(TextureType, 0, PixelInternalFormat.Rgba, imageMipMapped[0].Width, imageMipMapped[0].Height, 0, pixelFormat, PixelType.UnsignedByte, imageMipMapped[0]);
                GL.TexParameter(TextureType, TextureParameterName.TextureMinFilter, (int)All.Nearest);
                GL.TexParameter(TextureType, TextureParameterName.TextureMagFilter, (int)All.Nearest);
            }
            GL.PopAttrib();
        }
        #endregion
    }
}
