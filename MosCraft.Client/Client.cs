using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using OpenTK;
using OpenTK.Graphics.OpenGL;
using Voxel.Engine;
using Voxel.Engine.Cameras;
using Voxel.Engine.Debug;
using Voxel.Engine.Rendering;
using Voxel.Engine.Rendering.Text;

namespace MosCraft.Client
{
    class Client : GameWindow
    {
        private FpsComponent _fps;
        private Viewport _view;
        private Camera _camera;

        #region Properties

        public Camera Camera
        {
            get { return _camera; }
        }

        #endregion

        public Client()
            : base(1280, 720)
        {
            _fps = new FpsComponent();
            _view = new Viewport(Width, Height);
            _camera = new FirstPersonCamera(_view);

            Console.WriteLine(Capabilities.VideoVendor);
            Console.WriteLine(Capabilities.VideoRenderer);
            Console.WriteLine(Capabilities.VideoVersion);
            Console.WriteLine("OpenGL:" + Capabilities.MajorVersion + "." + Capabilities.MinorVersion);

            //BitmapFont b = new BitmapFont(@"./../../../Res/8bitoperator.ttf", 13);
        }

        protected override void OnLoad(EventArgs e)
        {
            GL.ClearColor(Color.MidnightBlue);
            _fps.OnLoad();
        }

        protected override void OnResize(EventArgs e)
        {
            _view.SetSize(Width, Height);
        }

        protected override void OnUpdateFrame(FrameEventArgs e)
        {
        }

        protected override void OnRenderFrame(FrameEventArgs e)
        {
            GL.Clear(ClearBufferMask.ColorBufferBit);

            _fps.OnRender(RenderTime);

            GL.Begin(BeginMode.Triangles);

            GL.Color3(Color.MidnightBlue);
            GL.Vertex2(-1.0f, 1.0f);
            GL.Color3(Color.SpringGreen);
            GL.Vertex2(0.0f, -1.0f);
            GL.Color3(Color.Ivory);
            GL.Vertex2(1.0f, 1.0f);

            GL.End();

            SwapBuffers();
        }
    }
}
