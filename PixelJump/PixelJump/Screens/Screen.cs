using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PixelJump.Screens
{
    public abstract class Screen
    {
        private ScreenManager screenManager = default!;
        public abstract void Update(ScreenManager screenManager);
        public abstract void Draw();
    }
}

