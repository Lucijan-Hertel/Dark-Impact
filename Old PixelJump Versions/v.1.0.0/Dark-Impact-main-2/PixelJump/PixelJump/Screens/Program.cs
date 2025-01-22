using System.Threading.Tasks;
using System.Numerics;
using static Raylib_CsLo.Raylib;

namespace PixelJump.Objects
{
    public static class Program
    {
        public static async Task Main(string[] args)
        {
            PixelJump.Objects.Player player = new PixelJump.Objects.Player(new Vector2(50, 50), new Vector2(200, 200), new Vector2(0, 0), new Vector2(0, -245), BLUE, 85);
            List<PixelJump.Platform> platforms = new List<PixelJump.Platform>();
            MainGame maingame = new MainGame();

            bool alreadyUsed = false;
            bool oneTimeSet = false;

            InitWindow(1000, 1000, "PixelJump");
            //ToggleFullscreen();
            SetTargetFPS(1600);
            while (!WindowShouldClose()) 
            {
                BeginDrawing();
                ClearBackground(SKYBLUE);
                DrawFPS(10, 10);

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

                EndDrawing();
            }
            CloseWindow();
        }
    }
}