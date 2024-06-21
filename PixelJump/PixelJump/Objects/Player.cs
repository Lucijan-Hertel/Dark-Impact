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
        Vector2 size = new Vector2();
        Raylib_CsLo.Color color = RED;
        float mass;
        Vector2 position = new Vector2();
        Vector2 velocity = new Vector2();
        Vector2 acceleration = new Vector2();

        public Player(Vector2 size, Vector2 position, Vector2 velocity, Vector2 acceleration, Raylib_CsLo.Color color, float mass) //Generates a new player object everytime called
        {
            this.size = size;
            this.position = position;
            this.velocity = velocity;
            this.acceleration = acceleration;
            this.mass = mass;
            this.color = color;
        }

        public Vector2 Size { get => size; set => size = value; }
        public Vector2 Position { get => position; set => position = value; }
        public Vector2 Velocity { get => velocity; set => velocity = value; }
        public Vector2 Acceleration { get => acceleration; set => acceleration = value; }
        public float Mass { get => mass; set => mass = value; }
        public Raylib_CsLo.Color Color { get => color; set => color = value; }

        public void DrawPlayer(int posx, int posy, int width, int height, Raylib_CsLo.Color color) //Draws player when called => Easier to identify in code when player is drawn && easier to search for
        {
            DrawRectangle(posx, posy, width, height, color);
        }

        public float GravitationalDistancesCenter(float timeTaken, ref float fullDistance, ref Vector2 timeUsed, ref float totalDistanceTravelled, float meter, List<Platform> platforms, Player player, ref bool oneTimeSet, ref bool distanceCalculated, ref bool distanceWas0)
        {
            float distanceTravelledInFrame = 0;
            float range;

            if (!oneTimeSet)
            {
                timeTaken = TimeTakenToReachPlatform(fullDistance, player.Acceleration.Y, player);
                oneTimeSet = true;
            }

            if (!distanceCalculated)
            {
                fullDistance = DistancePlayerToNextPlatformBelow(meter, oneTimeSet, player, platforms);
                totalDistanceTravelled = 0;
                distanceCalculated = true;
            }
            else if (DistancePlayerToNextPlatformBelow(meter, oneTimeSet, player, platforms) == 0) //-1/meter because there is a bug which lets the player draw 1 pixel over the platform
            {
                distanceWas0 = true;
            }
            else if (distanceWas0 && DistancePlayerToNextPlatformBelow(meter, oneTimeSet, player, platforms) != 0)
            {
                distanceCalculated = false;
                distanceWas0 = false;
            }

            distanceTravelledInFrame = Distances(player.Acceleration.Y, meter, ref timeUsed.Y, player.Velocity.Y);

            if (distanceTravelledInFrame < fullDistance - totalDistanceTravelled || distanceWas0)
            {
                range = fullDistance - totalDistanceTravelled;
                totalDistanceTravelled = totalDistanceTravelled + range;
                return range;
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
        /// <returns>float minimumDistance</returns>
        public float DistancePlayerToNextPlatformBelow(float meter, bool oneTimeSet, Player player, List<Platform> platforms)
        {
            float minimumDistance = (-GetScreenHeight() + Position.Y + player.size.Y) / meter;

            foreach (Platform platform in platforms)
            {
                if (platform.Position.Y >= (int) player.position.Y + player.size.Y && platform.Position.X <= (int) player.position.X + (int) player.size.X && (int) player.position.X <= (platform.Position.X + platform.Width))
                {
                    if (minimumDistance <= (-platform.Position.Y + (int) Position.Y + player.size.Y) / meter)
                    {
                        minimumDistance = (-platform.Position.Y + (int) Position.Y + player.size.Y) / meter;
                    }
                }
            }

            return minimumDistance;
        }

        public float TimeTakenToReachPlatform(float fullDistance, float gravitationalAcceleration, Player player)
        {
            float timeTaken;

            float firstTimeCalculated = (-player.Velocity.Y + (float) Math.Sqrt(Math.Pow(player.Velocity.Y, 2) - 4 * gravitationalAcceleration * -fullDistance)) / gravitationalAcceleration;
            float secondTimeCalculated = (-player.Velocity.Y - (float) Math.Sqrt(Math.Pow(player.Velocity.Y, 2) - 4 * gravitationalAcceleration * -fullDistance)) / gravitationalAcceleration;

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

        public float Distances(float Acceleration, float meter, ref float timeUsed, float velocity)
        {

            Console.WriteLine(Acceleration + ", " + meter + ", " + timeUsed + ", " + velocity);

            float firstRangeEnd = velocity * timeUsed + (float) 0.5 * Acceleration * (float)Math.Pow(timeUsed, 2);
            float secondRangeEnd = velocity * (timeUsed + GetFrameTime()) + (float) 0.5 * Acceleration * (float) Math.Pow(timeUsed + GetFrameTime(), 2);

            timeUsed = timeUsed + GetFrameTime();
            
            return (secondRangeEnd - firstRangeEnd) / meter; //The meter because in here we convert pixels in meters to make to model more realistic
        }

        public void ChangePosition(Vector2 distanceToTravel, float meter, Player player)
        {
            distanceToTravel.X = distanceToTravel.X * meter;
            distanceToTravel.Y = distanceToTravel.Y * meter;
            player.Position = new Vector2(player.Position.X + distanceToTravel.X, player.Position.Y - distanceToTravel.Y);

            if(distanceToTravel.Y == 0)
            {
                player.Position = new Vector2(player.Position.X, (float) Math.Round(player.Position.Y));
            }
        }

        //---------------------------------------------------------------------------------------------------------------------//

        public Vector2 DistancesCenter(ref float fullDistance, ref Vector2 timeUsed, ref float totalDistanceTravelled, float meter, List<Platform> platforms, Player player, ref bool oneTimeSet, ref bool distanceCalculated, ref bool distanceWas0)
        {
            float verticalDistance = GravitationalDistancesCenter(timeUsed.Y, ref fullDistance, ref timeUsed, ref totalDistanceTravelled, meter, platforms, player, ref oneTimeSet, ref distanceCalculated, ref distanceWas0);

            if(verticalDistance == 0)
            {
                timeUsed.Y = 0;
            }

            if (IsKeyDown(Raylib_CsLo.KeyboardKey.KEY_D)) 
            {
                player.acceleration.X = (float) 174*2;
            }
            else if (IsKeyDown(Raylib_CsLo.KeyboardKey.KEY_A))
            {
                player.acceleration.X = (float) -174*2;
            }
            else
            {
                player.acceleration.X = 0;
            }

            float horizontalDistance = Distances(player.Acceleration.X, meter, ref timeUsed.X, player.Velocity.X);

            if(horizontalDistance == 0)
            {
                timeUsed.X = 0;
            }

            return new Vector2((float) horizontalDistance, (float) verticalDistance);
        }
    }
}