using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using OpenTK;

namespace Voxel.Engine.Cameras
{
    public abstract class Camera
    {
        private readonly Viewport _viewPort;
        private Vector3 _position;

        private const float NearPlane = 0.01f;
        private const float FarPlane = 250.0f;

        protected Camera(Viewport viewPort)
        {
            _viewPort = viewPort;
            _position = Vector3.Zero;
        }

        protected virtual void CalculateView()
        {
        }

        protected virtual void CalculateProjection()
        {
            Projection = Matrix4.CreatePerspectiveFieldOfView(MathHelper.PiOver4, _viewPort.AspectRatio, NearPlane, FarPlane);
        }

        #region Public Properties

        public Vector3 Position
        {
            get { return _position; }
            set
            {
                _position = value;
                CalculateView();
            }
        }

        public Matrix4 View { get; protected set; }

        public Matrix4 Projection { get; protected set; }

        #endregion
    }
}
