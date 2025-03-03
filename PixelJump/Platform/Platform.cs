using System;
using PixelJump.Objects;
using System.Numerics;
using static Raylib_CsLo.Raylib;
using static Raylib_CsLo.RayGui;
using System.Runtime.InteropServices;
using System.Diagnostics;

namespace PixelJump
{
	public class Platform
	{
        Vector2 position;
        Vector2 size;
        Raylib_CsLo.Color color;
        List<Area> areas = new List<Area>();
        int numberOfAreas = 0;
        string information;

        public Platform(Vector2 position, Vector2 size, Raylib_CsLo.Color color, int numberOfAreas, string information) //Generates a new platform object everytime called
        {
            this.position = position;
            this.size = size;
            this.color = color;
            this.numberOfAreas = numberOfAreas;
            this.information = information;
        }

        public int NumberOfAreas { get => numberOfAreas; set => numberOfAreas = value; }
        public Raylib_CsLo.Color Color { get => color; set => color = value; }
        public Vector2 Position { get => position; set => position = value; }
        public Vector2 Size { get => size; set => size = value; }
        public List<Area> Areas { get => areas; set => areas = value; }
        public string Information { get => information; set => information = value; }

        //---Drawing Platforms---//

        public void DrawPlatforms(List<Platform> platforms) //Draws platform when called => Easier to identify in code when platform is drawn && easier to search for
        {
            foreach (Platform platform in platforms)
            {
                DrawRectangle((int) platform.position.X, (int) platform.position.Y, (int) platform.size.X, (int) platform.size.Y, platform.color);
            }
        }

        //---Sorting Platforms---//

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

        //---Level Generation---//

        //-Area Allocation-//

        public void AllocateAreasAndPlacePlatformsForIt(List<Platform> platforms, Area area, MovingObjects player)
        {
            int listSize = platforms.Count - 1;

            for (int i = 0; i <= listSize; i++)
            {
                foreach (Area exampleArea in platforms[i].areas)
                {
                    if (!exampleArea.PlatformPlaced && !exampleArea.Information.Contains("Spawn protection"))
                    {
                        platforms.Add(area.CreateNewPlatforms(platforms, exampleArea, platforms[i], player));
                        AllocateAreasForPlatform(platforms[platforms.Count - 1], player);
                    }
                }
            }
        }

        public void AllocateAreasForPlatform(Platform platform, MovingObjects player)
        {
            float time = player.CalculatingTimeTillMaximumJumpHeight(new Vector2(player.InitialVelocity.X, 200), player.MaximumAcceleration);
            Vector2 distance = player.CalculatingDistance(new Vector2(player.MaximumVelocity.X, 200), player.MaximumAcceleration, time);
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
            else if(((platform.numberOfAreas == 2 && platform.areas.Count < 2 && platform.size.X != GetScreenWidth()) || platform.position.X <= 150) && platform.size.X != GetScreenWidth())
            {
                AllocateAreaOnPlatformRight(player, platform);
                platform.numberOfAreas = 1;
            }
            else if (platform.numberOfAreas == 3 && platform.areas.Count < 3 && platform.size.X != GetScreenWidth())
            {
                AllocateAreaOnPlatformLeft(player, platform);
                AllocateAreaOnPlatformRight(player, platform);
            }
            else if(platform.numberOfAreas == 1 && platform.areas.Count < 2 && platform.size.X != GetScreenWidth())
            {
                AllocateOneOfBothAreas(player, platform);
            }

            // Special Case

            if(platform.size.X == GetScreenWidth() && platform.areas.Count < 3) // Starting Platform
            {
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

        private void AllocateAreaOnPlatformLeft(MovingObjects player, Platform platform)
        {
            float time;
            Vector2 distance;
            Area temporaryArea = new Area(new Vector2(0, 0), new Vector2(0, 0), "", false);


            if (platform.size.X < GetScreenWidth() && platform.position.X > 0) // Spawn area left
            {
                time = player.CalculatingTimeTillMaximumJumpHeight(new Vector2(player.InitialVelocity.X, 200), player.MaximumAcceleration);
                distance = player.CalculatingDistance(new Vector2(player.MaximumVelocity.X, 200), player.MaximumAcceleration, time);
                temporaryArea = new Area(new Vector2(platform.Position.X - distance.X, platform.Position.Y - distance.Y - player.Size.Y), new Vector2(distance.X, distance.Y), "Spawn Area left", false);
            }

            if (temporaryArea.Position.X < 0)
            {
                temporaryArea.Position = new Vector2(0, temporaryArea.Position.Y);
            }

            platform.areas.Add(temporaryArea);
        }

        private void AllocateAreaOnPlatformRight(MovingObjects player, Platform platform)
        {
            float time;
            Vector2 distance;

            if (platform.size.X < GetScreenWidth() && platform.position.X + platform.size.X < GetScreenWidth()) // Spawn Area area right
            {
                time = player.CalculatingTimeTillMaximumJumpHeight(new Vector2(player.InitialVelocity.X, 200), player.MaximumAcceleration);
                distance = player.CalculatingDistance(new Vector2(player.MaximumVelocity.X, 200), player.MaximumAcceleration, time);
                platform.Areas.Add(new Area(new Vector2(platform.Position.X + platform.Size.X, platform.Position.Y - distance.Y - player.Size.Y), new Vector2(distance.X, distance.Y), "Spawn Area right", false));
            }
        }

        private void AllocateOneOfBothAreas(MovingObjects player, Platform platform)
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

        //-End-of-Screen-Boarder--//

        public void GenerateNewPlatformsAndSaveOldOnes(List<List<Platform>> platformScreens, List<List<MovingObjects>> enemies, ref int numberOfCurrentScreen, MovingObjects player, MovingObjects enemy, Area area)
        {
            if(player.Position.Y < 100 && numberOfCurrentScreen == platformScreens.Count - 1)
            {
                Platform temporaryPlatform;

                List<Platform> platforms = new List<Platform>();
                platformScreens.Add(platforms);
                List<MovingObjects> enemyScreen = new List<MovingObjects>();
                enemies.Add(enemyScreen);

                //-Take-over-platforms-under-Yposition-100-//

                foreach (Platform platform in platformScreens[numberOfCurrentScreen])
                {
                    if (platform.position.Y < 100)
                    {
                        temporaryPlatform = new Platform(new Vector2(platform.position.X, platform.position.Y + GetScreenHeight()),
                                                        platform.size,
                                                        platform.color,
                                                        platform.numberOfAreas,
                                                        platform.information);

                        foreach(Area placedArea in platform.Areas)
                        {
                            temporaryPlatform.areas.Add(new Area(new Vector2(placedArea.Position.X,
                                                                 placedArea.Position.Y + GetScreenHeight()),
                                                                 placedArea.Size,
                                                                 placedArea.Information,
                                                                 placedArea.PlatformPlaced));
                        }

                        platformScreens[numberOfCurrentScreen + 1].Add(temporaryPlatform);
                    }
                }

                //-Add-new-Platforms-//

                while (platformScreens[numberOfCurrentScreen+1][platformScreens[numberOfCurrentScreen+1].Count - 1].Position.Y > 0 /*platform.Platforms.Count < 12*/)
                {
                    AllocateAreasAndPlacePlatformsForIt(platformScreens[numberOfCurrentScreen+1], area, player);
                    enemy.InitilizePlatforms(platformScreens[numberOfCurrentScreen+1], enemies[numberOfCurrentScreen+1]);  // Enemy size is 50 here, change in future
                }

                foreach(Platform platform in platformScreens[numberOfCurrentScreen + 1])
                {
                    if(platform.position.Y + platform.size.Y >= 0)
                    {
                        platformScreens[numberOfCurrentScreen].Add(new Platform(new Vector2(platform.position.X, platform.position.Y - GetScreenHeight()),
                                                                                platform.size,
                                                                                platform.color,
                                                                                platform.numberOfAreas,
                                                                                platform.information));
                    }
                }

                platformScreens[numberOfCurrentScreen] = sortPlatforms(platformScreens[numberOfCurrentScreen]);
            }

            int xyz = GetScreenHeight();
            if (player.Position.Y <= 0)
            {
                player.Position = new Vector2(player.Position.X, player.Position.Y + GetScreenHeight());
                numberOfCurrentScreen++;
            }
            else if(player.Position.Y > GetScreenHeight())
            {
                player.Position = new Vector2(player.Position.X, player.Position.Y - GetScreenHeight());
                numberOfCurrentScreen--;
            }
        }
    }
}

/*



 */

