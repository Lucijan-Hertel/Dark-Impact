using System;
using System.Numerics;
using static Raylib_CsLo.Raylib;
using static Raylib_CsLo.RayGui;
using Raylib_CsLo;

namespace PixelJump.Screens
{
    public class MainGame
    {
        Player player = new Player(new Vector2(50, 50), new Vector2(200, 200), new Vector2(0, 0), new Vector2(0, -245), BLUE, 85);
        Platform singlePlatform = new Platform(new Vector2(-50, -50), -50, -50, RED);
        Random rand = new Random();
        float fullDistance = 0;
        float timeTaken = 0;
        float totalDistanceTravelled = 0;
        float meter = 25;
        Vector2 timeUsed = new Vector2();
        Vector2 distanceToTravel = new Vector2();
        bool distanceCalculated = false;
        bool distanceWas0 = false;

        public void Update(ref bool alreadyUsed, ref bool oneTimeSet)
        {
            if (!alreadyUsed)
            {
                singlePlatform.Platforms.Add(new Platform(new Vector2(200, 400), 60, 10, RED));
                singlePlatform.Platforms.Add(new Platform(new Vector2(600, 300), 60, 10, RED));
                alreadyUsed = true;
            }

            distanceToTravel = player.DistancesCenter(ref fullDistance, ref timeUsed, ref totalDistanceTravelled, meter, singlePlatform.Platforms, player, ref oneTimeSet, ref distanceCalculated, ref distanceWas0);
        }

        public void Draw()
        {
            player.ChangePosition(distanceToTravel, meter, player);

            foreach(Platform platform in singlePlatform.Platforms)
            {
                platform.DrawPlatform((int) platform.Position.X, (int) platform.Position.Y, platform.Width, platform.Height, platform.Color);
            }
            player.DrawPlayer((int)player.Position.X, (int)player.Position.Y, (int) player.Size.X, (int) player.Size.Y, player.Color);
        }
    }
}

