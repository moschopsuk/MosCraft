using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using OpenTK;

namespace Voxel.Engine.Cameras
{
    public class FirstPersonCamera : Camera
    {
        public FirstPersonCamera(Viewport view) : base(view)
        {
        }

        public void LookAt(Vector3 target)
        {
            // Doesn't take into account the rotated UP vector
            // Should calculate rotations here!
            View = Matrix4.LookAt(Position, target, Vector3.UnitY);
        }
    }
}
