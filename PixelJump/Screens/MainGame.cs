using System;
using System.Numerics;
using static Raylib_CsLo.Raylib;
using static Raylib_CsLo.RayGui;
using PixelJump.Objects;
using Raylib_CsLo;

namespace PixelJump.Screens
{
    public class MainGame
    {
        //-New Instances-//
        Player player = new Player(new Vector2(50, 50), new Vector2(200, 956), new Vector2(0, 0), new Vector2(0, 0), new Vector2(0, 0), new Vector2(0, 0), RED);
        Platform platform = new Platform(new Vector2(), new Vector2(), RED, 0);
        Area area = new Area(new Vector2(0, 0), new Vector2(0, 0), "", false);

        bool alreadyUsed = false;

        public void Update()
        {
            if (!alreadyUsed)
            {
                platform.Platforms.Add(new Platform(new Vector2(0, GetScreenHeight() - 100), new Vector2(GetScreenWidth(), 100), DARKGREEN, 2));
                platform.AllocateAreasForPlatform(platform.Platforms[0], player);
                platform.Platforms = platform.sortPlatforms(platform.Platforms);

                for (int i = 0; i < 10; i++)
                {
                    platform.AllocateAreasAndPlacePlatformsForIt(platform, area, player);
                }

                alreadyUsed = true;
            }

            for (int i = 0; i < platform.Platforms.Count; i++)
            {
                foreach(Area area in platform.Platforms[i].Areas)
                {
                    if(!area.Information.Contains("Spawn protection"))
                    DrawRectangle((int) area.Position.X, (int) area.Position.Y, (int) area.Size.X, (int) area.Size.Y, BLACK);
                }
            }

            player.HealthSystem(platform);
            player.MovementCalculation(platform);
        }

        public void Draw()
        {
            platform.DrawPlatforms();
            player.DrawPlayer();
        }
    }
}

