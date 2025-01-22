using System;
using System.Numerics;
using static Raylib_CsLo.Raylib;
using static Raylib_CsLo.RayGui;
using Raylib_CsLo;

namespace PixelJump.Objects
{
    public class MainGame
    {
        Player player = new Player(new Vector2(50, 50), new Vector2(600, 200), new Vector2(0, 0), new Vector2(0, -500), BLUE, 85);
        Platform platform = new Platform(new Vector2(-50, -50), new Vector2(-50, -50), RED);
        Vector2 fullDistance = new Vector2();
        float meter = 25;
        Vector2 timeUsed = new Vector2();
        Vector2 distanceToTravel = new Vector2();
        bool distanceCalculated = false;
        bool distanceWas0 = false;

        public void Update(ref bool alreadyUsed, ref bool oneTimeSet)
        {
            if (!alreadyUsed)
            {
                platform.Platforms.Add(new Platform(new Vector2(200, 700), new Vector2(60, 10), RED));
                platform.Platforms.Add(new Platform(new Vector2(200, 300), new Vector2(60, 10), RED));
                alreadyUsed = true;
            }

            distanceToTravel = player.DistancesCenter(ref fullDistance, ref timeUsed, meter, platform.Platforms, player, ref oneTimeSet, ref distanceCalculated, ref distanceWas0);
        }

        public void Draw()
        {
            player.ChangePosition(distanceToTravel, meter, player);
            player.DrawPlayer((int)player.Position.X, (int)player.Position.Y, (int) player.Size.X, (int) player.Size.Y, player.Color);
            platform.DrawPlatforms();
        }
    }
}

