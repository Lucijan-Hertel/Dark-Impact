using System.Text;
using System.Threading.Tasks;
using static Raylib_CsLo.RayGui;
using static Raylib_CsLo.Raylib;
using Raylib_CsLo;

namespace PixelJump.Screens
{
    public class MainMenu : Screen
    {
        Rectangle buttonIambutton = new Rectangle(100, 100, 100, 100);

        public override void Draw()
        {
            throw new NotImplementedException();
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

