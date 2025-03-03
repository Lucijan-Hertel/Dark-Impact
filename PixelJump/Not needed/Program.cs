using System.Threading.Tasks;
using System.Numerics;
using PixelJump.Screens;
using static Raylib_CsLo.Raylib;

namespace PixelJump.Objects
{
    public static class Program
    {
        public static async Task Main(string[] await /*args*/)
        {
            Screen gameScreen = new GameScreen();
            Screen startScreen = new StartScreen();

            InitWindow(GetScreenWidth(), GetScreenHeight(), "PixelJump");
            SetTargetFPS(1600);
            while (!WindowShouldClose()) 
            {
                BeginDrawing();
                ClearBackground(SKYBLUE);
                DrawFPS(10, 10);

                startScreen.Update();
                startScreen.Draw();

                EndDrawing();
            }
            CloseWindow();
        }
    }
}