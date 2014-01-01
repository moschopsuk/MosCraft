using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using OpenTK;
using Voxel.Engine.Utils;

namespace Voxel.Engine.Cameras
{
    public abstract class Camera
    {
        private readonly Viewport _viewPort;
        private Frustum _frustum;
        private Vector3 _position;

        private const float ViewAngle = MathHelper.PiOver4;
        private const float NearPlane = 0.01f;
        private const float FarPlane = 250.0f;

        protected Camera(Viewport viewPort)
        {
            _viewPort = viewPort;
            _position = Vector3.Zero;
            _frustum = new Frustum(View * Projection);
        }

        #region Methods

        protected virtual void CalculateView()
        {
        }

        protected virtual void CalculateProjection()
        {
            Projection = Matrix4.CreatePerspectiveFieldOfView(ViewAngle, _viewPort.AspectRatio, NearPlane, FarPlane);
        }

        public virtual void Update(double dt)
        {
        }

        #endregion

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
