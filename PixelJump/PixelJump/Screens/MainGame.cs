using System;
using System.Numerics;
using static Raylib_CsLo.Raylib;
using static Raylib_CsLo.RayGui;
using Raylib_CsLo;

namespace PixelJump.Screens
{
    public class MainGame
    {
        Player player = new Player(50, new Vector2(200, 200), BLUE, 85, 0);
        Platform singlePlatform = new Platform(new Vector2(-50, -50), -50, -50, RED);
        Random rand = new Random();
        double currentDistance = 0;
        double fullDistance = 0;
        double gravitationalAcceleration = -245;
        double futureVelocity = 0;
        double timeTaken = 0;
        double totalDistanceTravelled = 0;
        double meter = 25;

        public void Update(ref bool alreadyUsed, ref bool oneTimeSet)
        {
            if (!alreadyUsed)
            {
                singlePlatform.Platforms.Add(new Platform(new Vector2(200, 400), 60, 10, RED));
                alreadyUsed = true;
                Console.WriteLine(fullDistance);
            }
            
            currentDistance = player.GravitationalDistancesCentre(gravitationalAcceleration, ref futureVelocity, ref timeTaken, ref fullDistance, ref totalDistanceTravelled, meter, singlePlatform.Platforms, player, ref oneTimeSet);
            //Console.WriteLine(currentDistance);
        }

        public void Draw()
        {
            player.ChangePosition(currentDistance, meter, player);

            foreach(Platform platform in singlePlatform.Platforms)
            {
                platform.DrawPlatform((int) platform.Position.X, (int) platform.Position.Y, platform.Width, platform.Height, platform.Color);
            }
            player.DrawPlayer((int)player.Position.X, (int)player.Position.Y, player.Size, player.Color);
        }
    }
}

