using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Voxel.Engine.DebugTools
{
    public class FpsComponent : GameComponent
    {
        private int _starttime;
        private int _frames;
        private float _fps;

        public override void OnLoad()
        {
            _starttime = Environment.TickCount;
            _frames = 0;
            _fps = 0.0f;
        }

        public override void OnRender(double dt)
        {
            _frames++;
            if (Environment.TickCount - _starttime > 1000)
            {
                Console.WriteLine("FPS:" + _fps);
                _fps = _frames / ((Environment.TickCount - _starttime) / 1000f);
                _starttime = Environment.TickCount;
                _frames = 0;
            }
        }
    }
}
