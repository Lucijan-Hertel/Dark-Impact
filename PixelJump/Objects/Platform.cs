using System;
using PixelJump.Objects;
using System.Numerics;
using static Raylib_CsLo.Raylib;
using static Raylib_CsLo.RayGui;
using System.Runtime.InteropServices;

namespace PixelJump
{
	public class Platform
	{
        Vector2 position;
        Vector2 size;
        Raylib_CsLo.Color color;
        List<Platform> platforms = new List<Platform>();
        List<Area> areas = new List<Area>();
        int numberOfAreas = 0;

        public Platform(Vector2 position, Vector2 size, Raylib_CsLo.Color color, int numberOfAreas) //Generates a new platform object everytime called
        {
            this.position = position;
            this.size = size;
            this.color = color;
            this.numberOfAreas = numberOfAreas;
        }

        public int NumberOfAreas { get => numberOfAreas; set => numberOfAreas = value; }
        public Raylib_CsLo.Color Color { get => color; set => color = value; }
        public Vector2 Position { get => position; set => position = value; }
        public List<Platform> Platforms { get => platforms; set => platforms = value; }
        public Vector2 Size { get => size; set => size = value; }
        public List<Area> Areas { get => areas; set => areas = value; }

        public void DrawPlatforms() //Draws platform when called => Easier to identify in code when platform is drawn && easier to search for
        {
            foreach (Platform platform in platforms)
            {
                DrawRectangle((int) platform.position.X, (int) platform.position.Y, (int) platform.size.X, (int) platform.size.Y, platform.color);
            }
        }

        public List<Platform> sortPlatforms(List<Platform> platforms)
        {
            List<Platform> mergedList = new List<Platform>();

            if (platforms.Count > 1)
            {
                List<Platform> leftSide = new List<Platform>();
                List<Platform> rightSide = new List<Platform>();

                for(int i = 0; i < platforms.Count; i++)
                {
                    if (i < platforms.Count / 2)
                    {
                        leftSide.Add(platforms[i]);
                    }
                    else
                    {
                        rightSide.Add(platforms[i]);
                    }
                }


                leftSide = sortPlatforms(leftSide);
                rightSide = sortPlatforms(rightSide);

                mergedList = mergeSortPlatform(leftSide, rightSide);
            }

            else
            {
                mergedList = platforms;
            }

            return mergedList;
        }

        public List<Platform> mergeSortPlatform(List<Platform> leftSide, List<Platform> rightSide)
        {
            List<Platform> mergedList = new List<Platform>();

            int indexLeft = 0;
            int indexRight = 0;

            while (indexLeft < leftSide.Count && indexRight < rightSide.Count)
            {
                if (leftSide[indexLeft].position.Y < rightSide[indexRight].position.Y)
                {
                    mergedList.Add(leftSide[indexLeft]);
                    indexLeft++;
                }
                else
                {
                    mergedList.Add(rightSide[indexRight]);
                    indexRight++;
                }
            }

            while (indexLeft < leftSide.Count)
            {
                mergedList.Add(leftSide[indexLeft]);
                indexLeft++;
            }

            while (indexRight < rightSide.Count)
            {
                mergedList.Add(rightSide[indexRight]);
                indexRight++;
            }

            return mergedList;
        }

        //--Level-Generation--//

        public void AllocateAreasAndPlacePlatformsForIt(Platform platform, Area area, Player player)
        {
            int listSize = platform.platforms.Count - 1;

            for (int i = 0; i <= listSize; i++)
            {
                foreach (Area exampleArea in platform.platforms[i].areas)
                {
                    if (!exampleArea.PlatformPlaced && !exampleArea.Information.Contains("Spawn protection"))
                    {
                        platform.Platforms.Add(area.CreateNewPlatforms(platform.platforms, exampleArea, platform.platforms[i], player));
                        AllocateAreasForPlatform(platforms[platform.platforms.Count - 1], player);
                    }
                }
            }
        }

        public void AllocateAreasForPlatform(Platform platform, Player player)
        {
            float time = 0;
            Vector2 distance;
            Random rand = new Random();

            if (platform.size.X < GetScreenWidth() && platform.areas.Count == 0) // Spawn protection area
            {
                platform.Areas.Add(new Area(new Vector2(platform.position.X - (int)(1.5 * player.Size.X), platform.Position.Y - (int)(1.5 * player.Size.Y)), new Vector2(platform.size.X + (int) (3 * player.Size.X), platform.Size.Y + (int) (3 * player.Size.Y)), "Spawn protection", false));
            }

            if (platform.numberOfAreas == 2 && platform.areas.Count < 2 && platform.size.X != GetScreenWidth() && GetScreenWidth() - (platform.position.X + platform.size.X) <= 150)
            {
                AllocateAreaOnPlatformLeft(player, platform);
                platform.numberOfAreas = 1;
            }
            else if((platform.numberOfAreas == 2 && platform.areas.Count < 2 && platform.size.X != GetScreenWidth()) || platform.position.X <= 150)
            {
                AllocateAreaOnPlatformRight(player, platform);
                platform.numberOfAreas = 1;
            }
            else if (platform.numberOfAreas == 3 && platform.areas.Count < 3 && platform.size.X != GetScreenWidth())
            {
                AllocateAreaOnPlatformLeft(player, platform);
                AllocateAreaOnPlatformRight(player, platform);
            }
            else if(platform.numberOfAreas == 1 && platform.areas.Count < 2)
            {
                AllocateOneOfBothAreas(player, platform);
            }

            // Special Case

            if(platform.size.X == GetScreenWidth() && platform.areas.Count < 3) // Starting Platform
            {
                time = player.CalculatingTimeTillMaximumJumpHeight(new Vector2(player.InitialVelocity.X, 200));
                distance = player.CalculatingDistance(new Vector2(150, 200), new Vector2(0, (float)-9.8 * 18), time);
                platform.Areas.Add(new Area(new Vector2(0, platform.position.Y - (int) (player.Size.Y * 1.5)), new Vector2(GetScreenWidth(), distance.Y), "Spawn protection", false)); // Spawn protection area
                platform.Areas.Add(new Area(new Vector2(0, platform.Position.Y - distance.Y - player.Size.Y), new Vector2(distance.X, distance.Y), "Spawn Area left", false)); // Spawn area left
                platform.Areas.Add(new Area(new Vector2(platform.size.X - distance.X, platform.Position.Y - distance.Y - player.Size.Y), new Vector2(distance.X, distance.Y), "Spawn Area left", false)); // Spawn area right
            }

            if (platform.Areas[0].Position.X < 0)
            {
                platform.Areas[0].Position = new Vector2(0, platform.Areas[0].Position.Y);
            }
            else if (platform.Areas[0].Position.X > GetScreenWidth())
            {
                platform.Areas[0].Position = new Vector2(GetScreenWidth(), platform.Areas[0].Position.Y);
            }
        }

        private void AllocateAreaOnPlatformRight(Player player, Platform platform)
        {
            float time;
            Vector2 distance;

            if (platform.size.X < GetScreenWidth() && platform.position.X + platform.size.X < GetScreenWidth()) // Spawn Area area right
            {
                time = player.CalculatingTimeTillMaximumJumpHeight(new Vector2(player.InitialVelocity.X, 200));
                distance = player.CalculatingDistance(new Vector2(150, 200), new Vector2(0, (float)-9.8 * 18), time);
                platform.Areas.Add(new Area(new Vector2(platform.Position.X + platform.Size.X, platform.Position.Y - distance.Y - player.Size.Y), new Vector2(distance.X, distance.Y), "Spawn Area right", false));
            }
        }

        private void AllocateAreaOnPlatformLeft(Player player, Platform platform)
        {
            float time;
            Vector2 distance;
            Area temporaryArea = new Area(new Vector2(0, 0), new Vector2(0, 0), "", false);


            if (platform.size.X < GetScreenWidth() && platform.position.X > 0) // Spawn area left
            {
                time = player.CalculatingTimeTillMaximumJumpHeight(new Vector2(player.InitialVelocity.X, 200));
                distance = player.CalculatingDistance(new Vector2(150, 200), new Vector2(0, (float)-9.8 * 18), time);
                temporaryArea = new Area(new Vector2(platform.Position.X - distance.X, platform.Position.Y - distance.Y - player.Size.Y), new Vector2(distance.X, distance.Y), "Spawn Area left", false);
            }

            if (temporaryArea.Position.X < 0)
            {
                temporaryArea.Position = new Vector2(0, temporaryArea.Position.Y);
            }

            platform.areas.Add(temporaryArea);
        }

        private void AllocateOneOfBothAreas(Player player, Platform platform)
        {
            Random rand = new Random();
            int leftOrRight = 0;
            if((platform.position.X != 0) && (platform.position.X + platform.size.X) != GetScreenWidth())
            {
                leftOrRight = rand.Next(1, 3);
            }

            if(leftOrRight == 1 || platform.position.X + platform.size.X == GetScreenWidth() || (leftOrRight == 2 && GetScreenWidth() - (platform.position.X + platform.size.X) < 150))
            {
                AllocateAreaOnPlatformLeft(player, platform);
            }
            else if(leftOrRight == 2 || platform.position.X == 0 || platform.position.X < 150)
            {
                AllocateAreaOnPlatformRight(player, platform);
            }
        }
    }
}

/*



 */

