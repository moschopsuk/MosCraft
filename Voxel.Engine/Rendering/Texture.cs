using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using OpenTK.Graphics.OpenGL;

namespace Voxel.Engine.Rendering
{
    public abstract class Texture
    {
        #region Fields
        protected int _gLNumber = -1;
        protected bool _mipmap = false;
        #endregion

        #region Properties
        /// <summary>OpenGL's ID for this Texture</summary>
        public int GLNumber
        {
            get { return _gLNumber; }
        }

        /// <summary> Get or Set a boolean value indicating whether to mipmap the image </summary>
        /// <remarks> 
        /// Note that using mipmapping hurts performance and effects quality. 
        /// Changing the value will cause the texture to reinitialize. 
        /// </remarks>
        public bool Mipmap
        {
            get { return _mipmap; }
            set { _mipmap = value; }
        }

        /// <summary>The dimentionality of the texture (2D, 3D, etc.)</summary>
        abstract public TextureTarget TextureType { get; }

        /// <summary>The TextureType cast as an Enabled Enum</summary>
        protected EnableCap TextureTypeEnabled
        {
            get { return (EnableCap)TextureType; }
        }
        #endregion

        #region Constructors
        /// <summary> Alocate an OpenGL texture index without using data </summary>
        public Texture() : this(true) { }
        /// <summary> Alocate an OpenGL texture index without using an image </summary>
        /// <param name="mipmap"> Indicate whether to mipmap </param>
        public Texture(bool mipmap)
        {
            this._mipmap = mipmap;
            Initialize();
        }

        ~Texture()
        {
            try
            {
                Delete();
            }
            catch { }
        }
        #endregion

        #region Methods
        /// <summary> Use this texture for rendering </summary>
        /// <remarks> Side Effect: This texture type is enabled. All others are disabled. </remarks>
        virtual public void Bind()
        {
            Bind(TextureUnit.Texture0);
        }
        /// <summary> Use this texture for rendering </summary>
        /// <remarks> Side Effect: This texture type is enabled. All others are disabled. </remarks>
        /// <param name="textureUnit"> Used when multiple textures are needed simultaneously </param>
        virtual public void Bind(TextureUnit textureUnit)
        {
            GL.Disable(EnableCap.Texture3DExt);
            GL.Disable(EnableCap.Texture2D);

            GL.Enable(TextureTypeEnabled);
            GL.ActiveTexture(textureUnit);
            GL.BindTexture(TextureType, GLNumber);
            GL.TexParameter(TextureType, TextureParameterName.TextureWrapS, (int)TextureWrapMode.Clamp);
            GL.TexParameter(TextureType, TextureParameterName.TextureWrapT, (int)TextureWrapMode.Clamp);

            if (_mipmap)
            {
                GL.TexParameter(TextureType, TextureParameterName.TextureMinFilter, (int)All.LinearMipmapLinear);
                GL.TexParameter(TextureType, TextureParameterName.TextureMagFilter, (int)All.LinearMipmapLinear);
            }
            else
            {
                GL.TexParameter(TextureType, TextureParameterName.TextureMinFilter, (int)All.Nearest);
                GL.TexParameter(TextureType, TextureParameterName.TextureMagFilter, (int)All.Nearest);
            }
        }

        /*/// <summary>Apply this texture to a shader's sampler</summary>
        /// <param name="shaderProgram">The <c>ShaderProgram</c> where the sampler is located</param>
        /// <param name="samplerName">The name of the sampler in the shader </param>
        public void BindToShaderSampler(ShaderProgram shaderProgram, string samplerName, TextureUnit textureUnit)
        {
            int samplerUniformLocation = GL.GetUniformLocation(shaderProgram.ProgramIndex, samplerName);
            Bind(textureUnit);
            GL.Uniform1(samplerUniformLocation, textureUnit - TextureUnit.Texture0);
        }*/

        /// <summary> Allocate an OpenGL texture index </summary>
        /// <remarks> Side Effect: This type of texture is enabled </remarks>
        public void Initialize()
        {
            if (GLNumber >= 0)
                return;

            //Gl.glPushAttrib(Gl.GL_TEXTURE_BIT);
            GL.Enable(TextureTypeEnabled);
            GL.GenTextures(1, out _gLNumber);
            Bind();
            //Gl.glPopAttrib();
        }

        abstract protected void SendToGL();

        /// <summary> Stop using this texture for rendering </summary>
        /// <remarks> Side Effect: 2D Textures are disabled </remarks>
        public void UnBind()
        {
            GL.Disable(TextureTypeEnabled);
        }

        /// <summary> Delete the texture on the GPU </summary>
        public void Delete()
        {
            if (_gLNumber <= 0)
                return;
            GL.PushAttrib(AttribMask.TextureBit | AttribMask.EnableBit);
            GL.Enable(TextureTypeEnabled);
            GL.DeleteTextures(1, ref _gLNumber);
            GL.PopAttrib();
        }
        #endregion
    }
}
