using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using OpenTK.Graphics.OpenGL;

namespace Voxel.Engine.Rendering
{
    public class Capabilities
    {
        public static string VideoVersion
        {
            get
            {
                return GL.GetString(StringName.Version);
            }
        }

        public static string VideoVendor
        {
            get
            {
                return GL.GetString(StringName.Vendor);
            }
        }

        public static string VideoRenderer
        {
            get
            {
                return GL.GetString(StringName.Renderer);
            }
        }

        public static string ShaderVersion
        {
            get
            {
                return GL.GetString(StringName.ShadingLanguageVersion);
            }
        }

        public static int MajorVersion
        {
            get
            {
                int value = 0;
                GL.GetInteger(GetPName.MajorVersion, out value);
                return value;
            }
        }

        public static int MinorVersion
        {
            get
            {
                int value = 0;
                GL.GetInteger(GetPName.MinorVersion, out value);
                return value;
            }
        }

        public static IEnumerable<String> Extensions
        {
            get
            {
                int count = 0;
                GL.GetInteger(GetPName.NumExtensions, out count);
                
                for (int id = 0; id < count; id++)
                {

                    yield return GL.GetString(StringNameIndexed.Extensions, id);
                }
            }
        }

        public static bool SupportVBO
        {
            get
            {
                return Extensions.Contains("GL_ARB_vertex_buffer_object");
            }
        }

        public static bool SupportFBO
        {
            get
            {
                return Extensions.Contains("GL_ARB_framebuffer_object");
            }
        }

        public static bool SupportNPOT
        {
            get
            {
                return Extensions.Contains("GL_ARB_texture_non_power_of_two");
            }
        }
    }
}
