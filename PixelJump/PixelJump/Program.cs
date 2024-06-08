using System.Threading.Tasks;
using PixelJump.Screens;
using Raylib_CsLo;

namespace NEA
{
    public static class Program
    {
        public static async Task Main(string[] args)
        {
            PixelJump.Player player = new PixelJump.Player(50, Raylib.RED);

            Raylib.InitWindow(1280, 720, "PixelJump");
            Raylib.SetTargetFPS(500);
            Splash splashscreen = new Splash();
            ScreenManager screenmanager = new ScreenManager(new Splash());
            while (!Raylib.WindowShouldClose()) 
            {
                Raylib.BeginDrawing();
                Raylib.ClearBackground(Raylib.SKYBLUE);
                Raylib.DrawFPS(10, 10);

                screenmanager.Update();
                screenmanager.Draw();

                Raylib.EndDrawing();
            }
            Raylib.CloseWindow();
        }
    }
}