using System;
using System.Numerics;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Raylib_CsLo.Raylib;
using System.Runtime.InteropServices;

namespace PixelJump.Objects
{
    public class Player : Creature
    {
        //- Attributes Player -------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------//

        float mass;
        Vector2 size = new Vector2();
        Vector2 position = new Vector2();
        Vector2 velocity = new Vector2();
        Vector2 acceleration = new Vector2();
        Vector2 distance = new Vector2();
        Raylib_CsLo.Color color = RED;
        
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

        //- General Methods -----------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------//

        /// <summary>
        /// Draws Player when called => Easier to identify when Player is drawn in the code
        /// </summary>
        /// <param name="posx"></param>
        /// <param name="posy"></param>
        /// <param name="width"></param>
        /// <param name="height"></param>
        /// <param name="color"></param>
        public void DrawPlayer(int posx, int posy, int width, int height, Raylib_CsLo.Color color) //Draws player when called => Easier to identify in code when player is drawn && easier to search for
        {
            DrawRectangle(posx, posy, width, height, color);
        }

        //- Gravity -------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------//

        public Vector2 GravitationalDistancesCenter(ref Vector2 fullDistance, ref Vector2 timeUsed, float meter, List<Platform> platforms, Player player, ref bool oneTimeSet, ref bool distanceCalculated, ref bool distanceWas0)
        {
            float range;
            Vector2 currentDistance;

            if(fullDistance.Y != DistancePlayerToNextPlatformBelow(meter, oneTimeSet, player, platforms))
            {
                fullDistance.Y = DistancePlayerToNextPlatformBelow(meter, oneTimeSet, player, platforms);
                distance.Y = 0;
            }

            if (!distanceCalculated)
            {
                fullDistance.Y = DistancePlayerToNextPlatformBelow(meter, oneTimeSet, player, platforms);
                distance.Y = 0;
                velocity.Y = 0;
                timeUsed.Y = TimeTakenToReachPlatform(fullDistance.Y, player.Acceleration.Y, player);
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

            currentDistance = Distances(acceleration, meter, ref timeUsed, velocity);

            if (currentDistance.Y < fullDistance.Y - distance.Y)
            {
                range = fullDistance.Y - distance.Y;
                distance.Y = distance.Y + range;

                return new Vector2(currentDistance.X, range);
            }
            else
            {
                distance.Y = currentDistance.Y;
                distance.X = currentDistance.X;
                return distance;
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
                if (platform.Position.Y >= (int) player.position.Y + player.size.Y && platform.Position.X <= (int) player.position.X + (int) player.size.X && (int) player.position.X <= (platform.Position.X + platform.Size.X))
                {
                    if (minimumDistance <= (-platform.Position.Y + (int) Position.Y + player.size.Y) / meter)
                    {
                        minimumDistance = (-platform.Position.Y + (int) Position.Y + player.size.Y) / meter;
                    }
                }
            }

            return minimumDistance;
        }

        /// <summary>
        /// Calculates the time needed for the Player to reach the next platform
        /// </summary>
        /// <param name="fullDistance"></param>
        /// <param name="gravitationalAcceleration"></param>
        /// <param name="player"></param>
        /// <returns></returns>
        public float TimeTakenToReachPlatform(float fullDistance, float gravitationalAcceleration, Player player)
        {
            float firstTimeCalculated = (-player.Velocity.Y + (float) Math.Sqrt(Math.Pow(player.Velocity.Y, 2) - 4 * gravitationalAcceleration * -fullDistance)) / gravitationalAcceleration;
            float secondTimeCalculated = (-player.Velocity.Y - (float) Math.Sqrt(Math.Pow(player.Velocity.Y, 2) - 4 * gravitationalAcceleration * -fullDistance)) / gravitationalAcceleration;

            if(firstTimeCalculated > secondTimeCalculated)
            {
                return firstTimeCalculated;
            }
            else
            {
                return secondTimeCalculated;
            }
        }

        //- General Movement --------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------//

        public Vector2 DistancesCenter(ref Vector2 fullDistance, ref Vector2 timeUsed, float meter, List<Platform> platforms, Player player, ref bool oneTimeSet, ref bool distanceCalculated, ref bool distanceWas0)
        {

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

            Vector2 currentDistance = GravitationalDistancesCenter(ref fullDistance, ref timeUsed, meter, platforms, player, ref oneTimeSet, ref distanceCalculated, ref distanceWas0);

            if (currentDistance.Y == 0)
            {
                timeUsed.Y = 0;
            }

            if (currentDistance.X == 0)
            {
                timeUsed.X = 0;
            }

            return currentDistance;
        }

        /// <summary>
        /// Uses SUVAT equations to calculate the distance between the player and the wished end
        /// </summary>
        /// <param name="Acceleration"></param>
        /// <param name="meter"></param>
        /// <param name="timeUsed"></param>
        /// <param name="velocity"></param>
        /// <returns>float distance</returns>
        public Vector2 Distances(Vector2 acceleration, float meter, ref Vector2 timeUsed, Vector2 velocity)
        {
            float verticalDistance;
            float horizontalDistance;
            float firstRangeEnd;
            float secondRangeEnd;

            firstRangeEnd = velocity.Y * timeUsed.Y + (float)0.5 * acceleration.Y * (float)Math.Pow(timeUsed.Y, 2);
            secondRangeEnd = velocity.Y * (timeUsed.Y + GetFrameTime()) + (float)0.5 * acceleration.Y * (float)Math.Pow(timeUsed.Y + GetFrameTime(), 2);

            verticalDistance = (secondRangeEnd - firstRangeEnd) / meter;

            //-//

            firstRangeEnd = velocity.X * timeUsed.X + (float)0.5 * acceleration.X * (float)Math.Pow(timeUsed.X, 2);
            secondRangeEnd = velocity.X * (timeUsed.X + GetFrameTime()) + (float)0.5 * acceleration.X * (float)Math.Pow(timeUsed.X + GetFrameTime(), 2);

            horizontalDistance = (secondRangeEnd - firstRangeEnd) / meter;

            //-//

            timeUsed = new Vector2(timeUsed.X + GetFrameTime(), timeUsed.Y + GetFrameTime());

            return new Vector2(horizontalDistance, verticalDistance);
        }

        public void ChangePosition(Vector2 distanceToTravel, float meter, Player player)
        {
            distanceToTravel.X = distanceToTravel.X * meter;
            distanceToTravel.Y = distanceToTravel.Y * meter;
            player.Position = new Vector2(player.Position.X + distanceToTravel.X, player.Position.Y - distanceToTravel.Y);

            if (distanceToTravel.Y == 0)
            {
                player.Position = new Vector2(player.Position.X, (float)Math.Round(player.Position.Y));
            }
        }
    }
}