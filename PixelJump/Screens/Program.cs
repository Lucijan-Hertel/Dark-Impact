using System.Threading.Tasks;
using System.Numerics;
using PixelJump.Screens;
using static Raylib_CsLo.Raylib;

namespace PixelJump.Objects
{
    public static class Program
    {
        public static async Task Main(string[] args)
        {
            MainGame maingame = new MainGame();

            InitWindow(900, GetScreenHeight(), "PixelJump");
            //ToggleFullscreen();
            SetTargetFPS(1600);
            while (!WindowShouldClose()) 
            {
                BeginDrawing();
                ClearBackground(SKYBLUE);
                DrawFPS(10, 10);

                maingame.Update();
                maingame.Draw();

                EndDrawing();
            }
            CloseWindow();
        }
    }
}