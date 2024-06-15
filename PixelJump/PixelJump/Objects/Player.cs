using System;
using System.Numerics;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Raylib_CsLo.Raylib;
using System.Runtime.InteropServices;

namespace PixelJump
{
    public class Player
    {
        int size = 50;
        Raylib_CsLo.Color color = RED;
        Vector2 position;
        double mass;
        double currentVelocity;
        double timeUsed = 0;

        public Player(int size, Vector2 position, Raylib_CsLo.Color color, double mass, double currentVelocity) //Generates a new player object everytime called
        {
            this.size = size;
            this.color = color;
            this.position = position;
            this.mass = mass;
            this.currentVelocity = currentVelocity;
        }

        public int Size { get => size; set => size = value; }
        public Raylib_CsLo.Color Color { get => color; set => color = value; }
        public Vector2 Position { get => position; set => position = value; }
        public double Mass { get => mass; set => mass = value; }
        public double CurrentVelocity { get => currentVelocity; set => currentVelocity = value; }

        public void DrawPlayer(int posx, int posy, int size, Raylib_CsLo.Color color) //Draws player when called => Easier to identify in code when player is drawn && easier to search for
        {
            DrawRectangle(posx, posy, size, size, color);
        }

        public double GravitationalDistancesCentre(double gravitationalAcceleration,ref double futureVelocity,ref double timeTaken, ref double fullDistance, ref double totalDistanceTravelled, List<Platform> platforms, Player player, ref bool oneTimeSet)
        {
            double distanceTravelledInFrame;

            if (!oneTimeSet)
            {
                fullDistance = DistancePlayerToNextPlatformBelow(player, platforms);
                timeTaken = TimeTakenToReachPlatform(timeTaken, fullDistance, gravitationalAcceleration, player);
                futureVelocity = player.CurrentVelocity + gravitationalAcceleration * (timeUsed + GetFrameTime());
                oneTimeSet = true;
            }

            distanceTravelledInFrame = GravitationalDistances(futureVelocity, gravitationalAcceleration, ref timeUsed);

            if (distanceTravelledInFrame < fullDistance + totalDistanceTravelled)
            {
                return fullDistance + totalDistanceTravelled;
            }
            else
            {
                totalDistanceTravelled = totalDistanceTravelled + distanceTravelledInFrame;
                return distanceTravelledInFrame;
            }
        }

        /// <summary>
        /// <c>DistancePlayerToNextPlatformBelow</c> uses a list of <paramref name="platforms"/> and the position of the <paramref name="player"/> to get the distance between him and the nearest vertical <paramref name="platforms"/> below him
        /// </summary>
        /// <param name="player"></param>
        /// <param name="platforms"></param>
        /// <returns>double minimumDistance</returns>
        public double DistancePlayerToNextPlatformBelow(Player player, List<Platform> platforms)
        {
            double minimumDistance = -GetScreenHeight() + Position.Y;

            foreach (Platform platform in platforms)
            {
                if (platform.Position.Y >= player.position.Y + player.size && (platform.Position.X - (platform.Width / 2)) <= player.position.X || player.position.X > (platform.Position.X + (platform.Width / 2)))
                {
                    if (minimumDistance <= -platform.Position.Y + Position.Y + player.size)
                    {
                        minimumDistance = -platform.Position.Y + Position.Y + player.size;
                    }
                }
            }

            return minimumDistance;
        }

        public double TimeTakenToReachPlatform(double timeTaken, double fullDistance, double gravitationalAcceleration, Player player)
        {
            double firstTimeCalculated = (-player.currentVelocity + Math.Sqrt(Math.Pow(currentVelocity, 2) -4 * gravitationalAcceleration * -fullDistance)) / gravitationalAcceleration;
            double secondTimeCalculated = (-player.currentVelocity - Math.Sqrt(Math.Pow(currentVelocity, 2) - 4 * gravitationalAcceleration * -fullDistance)) / gravitationalAcceleration;

            if(firstTimeCalculated > secondTimeCalculated)
            {
                timeTaken = firstTimeCalculated;
            }
            else
            {
                timeTaken = secondTimeCalculated;
            }

            return timeTaken;
        }

        public double GravitationalDistances(double futureVelocity, double gravitationalAcceleration, ref double timeUsed)
        {
            double firstRangeEnd = currentVelocity * timeUsed + 0.5 * gravitationalAcceleration * Math.Pow(timeUsed, 2);
            double secondRangeEnd = currentVelocity * (timeUsed + GetFrameTime()) + 0.5 * gravitationalAcceleration * Math.Pow(timeUsed + GetFrameTime(), 2);
            Console.WriteLine(timeUsed + GetFrameTime());
            timeUsed = timeUsed + GetFrameTime();
            return secondRangeEnd - firstRangeEnd;
        }

        public void ChangePosition(double currentDistance, Player player)
        {
            player.Position = new Vector2(player.Position.X, player.Position.Y - (float)currentDistance);
        }
    }
}