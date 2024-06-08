using PixelJump.Screens;
using Raylib_CsLo;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Raylib_CsLo.RayGui;
using static Raylib_CsLo.Raylib;

namespace PixelJump.Screens
{
    public class Splash : Screen
    {
        Rectangle buttonIambutton = new Rectangle(100, 100, 100, 100);

        public Splash()
        {
            // Initisile all parts of splash screen
        }

        public override void Draw()
        {
            DrawText("Splash, Splash", 200, 200, 50, RED);
        }

        public override void Update(ScreenManager sm)
        {
            if (GuiButton(buttonIambutton, "I am a Button"))
            {
                sm.SetScreen(new Splash());
            }
        }
    }
}

