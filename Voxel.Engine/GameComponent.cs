using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Voxel.Engine
{
    public abstract class GameComponent
    {
        public virtual void OnLoad() { }

        public virtual void OnUpdate(double dt) { }

        public virtual void OnRender(double dt) { }
    }
}
