using System.Threading.Tasks;
using System.Numerics;
using PixelJump.Screens;
using Raylib_CsLo;

namespace NEA
{
    public static class Program
    {
        public static async Task Main(string[] args)
        {
            PixelJump.Player player = new PixelJump.Player(new Vector2(50, 50), new Vector2(200, 200), new Vector2(0, 0), new Vector2(0, -245), Raylib.BLUE, 85);
            List<PixelJump.Platform> platforms = new List<PixelJump.Platform>();
            MainGame maingame = new MainGame();

            bool alreadyUsed = false;
            bool oneTimeSet = false;

            Raylib.InitWindow(1280, 720, "PixelJump");
            //Raylib.ToggleFullscreen();
            Raylib.SetTargetFPS(1600);
            while (!Raylib.WindowShouldClose()) 
            {
                Raylib.BeginDrawing();
                Raylib.ClearBackground(Raylib.SKYBLUE);
                Raylib.DrawFPS(10, 10);

                try
                {
                    maingame.Update(ref alreadyUsed, ref oneTimeSet);
                    maingame.Draw();
                }
                catch (Exception e)
                {
                    Console.WriteLine(e);
                    oneTimeSet = false;
                }

                Raylib.EndDrawing();
            }
            Raylib.CloseWindow();
        }
    }
}