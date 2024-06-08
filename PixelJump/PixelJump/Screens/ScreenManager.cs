using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PixelJump.Screens
{
    public class ScreenManager
    {
        Screen currentScreen;
        ScreenManager sm = default!;

        public ScreenManager(Screen currentScreen)
        {
            this.currentScreen = currentScreen;
        }

        public void SetScreen(Screen newScreen)
        {
            currentScreen = newScreen;
        }

        public void Update()
        {
            currentScreen.Update(sm);
        }

        public void Draw()
        {
            currentScreen.Draw();
        }
    }
}
