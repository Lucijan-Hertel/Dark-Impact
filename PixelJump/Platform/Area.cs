using System;
using PixelJump.Objects;
using System.Numerics;
using static Raylib_CsLo.Raylib;
using static Raylib_CsLo.RayGui;
using System.Runtime.InteropServices;

namespace PixelJump.Objects
{
    public class Area
    {
        private Vector2 position;
        private Vector2 size;
        private string information;
        bool platformPlaced;

        public Area(Vector2 position, Vector2 size, string information, bool platformPlaced)
        {
            this.position = position;
            this.size = size;
            this.information = information;
            this.platformPlaced = platformPlaced;
        }

        //--Platform Generation--//
        
        public Platform CreateNewPlatforms(List<Platform> platforms, Area area, Platform platform, MovingObjects player) // Change every Areas[1] to the opposite area side of the original area
        {
            Platform closestPlatform = platforms[0];
            Area closestArea = CheckForClosestArea(area, ref closestPlatform, platforms);
            Area leftArea = area;
            Area rightArea = closestArea;

            CheckWhichAreaIsOnWhichSide(ref leftArea, ref rightArea);

            if (closestArea.position != new Vector2(0, 0) && leftArea.Information.Contains("Spawn Area right") && rightArea.information.Contains("Spawn Area left"))
            {
                setBoolVariableForPlatformsInAreaToTrue(area, closestArea, platforms);
                return CreatePlatformWithTwoAreas(leftArea, rightArea, platforms, player);
            }
            else
            {
                //area.platformPlaced = true;
                return CreatePlatformWithSingleArea(area, platform, platforms, player);
            }

        }

        public void setBoolVariableForPlatformsInAreaToTrue(Area area, Area otherArea, List<Platform> platforms)
        {
            //new

            area.platformPlaced = true;
            otherArea.platformPlaced = true;

            for(int i = 0; i < platforms.Count; i++)
            {
                for(int j = 0; j < platforms[i].Areas.Count; j++)
                {
                    if (platforms[i].Areas[j].position == otherArea.position && platforms[i].Areas[j].size == otherArea.size)
                    {
                        platforms[i].Areas.RemoveAt(j);
                        platforms[i].Areas.Add(otherArea);
                    } 
                }
            }
        }

        public Area CheckForClosestArea(Area area, ref Platform closestPlatform, List<Platform> platforms)
        {
            float distanceBetweenAreas = 0;
            Area closestArea = new Area(new Vector2(0, 0), new Vector2(0, 0), "", false);

            foreach (Platform platform in platforms)
            {
                foreach (Area exampleArea in platform.Areas)
                {
                    if (area.information.Contains("Spawn Area left")
                        && exampleArea.Information.Contains("Spawn Area right")
                        && !exampleArea.platformPlaced
                        && (int)exampleArea.position.X != (int)(area.position.X + area.size.X + platform.Size.X)
                        && (area.position.Y <= exampleArea.position.Y
                        && area.position.Y + area.size.Y >= exampleArea.position.Y
                            || exampleArea.position.Y <= area.position.Y
                        && exampleArea.position.Y + exampleArea.size.Y >= area.position.Y)
                        && exampleArea.Position.X - area.Position.X < distanceBetweenAreas)
                    {
                        distanceBetweenAreas = exampleArea.Position.X - area.Position.X;
                        closestArea = exampleArea;
                        closestPlatform = platform;
                    } // here
                    else if (area.information.Contains("Spawn Area right")
                        && exampleArea.Information.Contains("Spawn Area left")
                        && !exampleArea.platformPlaced
                        && (int)exampleArea.position.X != (int)(area.position.X - platform.Size.X - exampleArea.size.X)
                        && exampleArea.position != platforms[0].Areas[1].Position
                        && (area.position.Y <= exampleArea.position.Y
                        && area.position.Y + area.size.Y >= exampleArea.position.Y
                            || exampleArea.position.Y <= area.position.Y
                        && exampleArea.position.Y + exampleArea.size.Y >= area.position.Y)
                        && area.position.X - exampleArea.Position.X <= distanceBetweenAreas)
                    {
                        distanceBetweenAreas = exampleArea.position.X - area.Position.X;
                        closestArea = exampleArea;
                        closestPlatform = platform;
                    } // here
                }
            }

            return closestArea;
        }

        public void CheckWhichAreaIsOnWhichSide(ref Area leftArea, ref Area rightArea)
        {
            if (leftArea.position.X > rightArea.position.X)
            {
                Area temporaryArea = rightArea;
                rightArea = leftArea;
                leftArea = temporaryArea;
            }
        }

        public Platform CreatePlatformWithTwoAreas(Area leftArea, Area rightArea, List<Platform> platforms, MovingObjects player)
        {
            Random rand = new Random();
            leftArea.platformPlaced = true;
            rightArea.platformPlaced = true;
            Vector2 minimumPosition;
            Vector2 maximumPosition;
            Vector2 position;
            Vector2 size;


            if (leftArea.position.Y < rightArea.position.Y)
            {
                minimumPosition.Y = rightArea.position.Y;
                maximumPosition.Y = leftArea.position.Y + leftArea.size.Y;
            }
            else
            {
                minimumPosition.Y = leftArea.position.Y;
                maximumPosition.Y = rightArea.position.Y + rightArea.size.Y;
            }

            minimumPosition.X = leftArea.position.X;
            maximumPosition.X = leftArea.position.X + leftArea.size.Y;

            position.Y = rand.Next((int)minimumPosition.Y, (int)maximumPosition.Y);
            position.X = rand.Next((int)minimumPosition.X, (int)maximumPosition.X);
            size.Y = 50;
            size.X = rand.Next((int)(rightArea.position.X - position.X), (int)(rightArea.position.X + rightArea.size.X - position.X));

            for (int i = 0; i < platforms.Count; i++)
            {
                while ((platforms[i].Areas[0].position.Y <= position.Y + size.Y
                        && platforms[i].Areas[0].position.Y + platforms[i].Areas[0].size.Y >= position.Y + size.Y
                        && platforms[i].Areas[0].position.X <= position.X + size.X
                        && platforms[i].Areas[0].position.X + platforms[i].Areas[0].size.X >= position.X + size.X)
                    || (platforms[i].Areas[0].position.Y <= position.Y + size.Y
                        && platforms[i].Areas[0].position.Y + platforms[i].Areas[0].size.Y >= position.Y + size.Y
                        && platforms[i].Areas[0].position.X <= position.X
                        && platforms[i].Areas[0].position.X + platforms[i].Areas[0].size.X >= position.X))
                {
                    position.Y = rand.Next((int)minimumPosition.Y, (int)maximumPosition.Y);
                    position.X = rand.Next((int)minimumPosition.X, (int)maximumPosition.X);
                    size.Y = 50;
                    size.X = rand.Next((int)(rightArea.position.X - position.X), (int)(rightArea.position.X + rightArea.size.X - position.X));
                }
            }

            leftArea.PlatformPlaced = true;
            rightArea.PlatformPlaced = true;

            return new Platform(position, size, DARKGREEN, rand.Next(1, 4), "");
        }

        public Platform CreatePlatformWithSingleArea(Area area, Platform platform, List<Platform> platforms, MovingObjects player)
        {
            Random rand = new Random();
            Platform temporaryPlatform = CreateCoordinatesForPlatform(platform, area, player);

            int numberOfTimes = 0;//debug

            for (int i = 0; i < platforms.Count; i++)
            {
                while ((platforms[i].Areas[0].position.Y <= temporaryPlatform.Position.Y + temporaryPlatform.Size.Y
                        && platforms[i].Areas[0].position.Y + platforms[i].Areas[0].size.Y >= temporaryPlatform.Position.Y + temporaryPlatform.Size.Y
                        && platforms[i].Areas[0].position.X <= temporaryPlatform.Position.X + temporaryPlatform.Size.X
                        && platforms[i].Areas[0].position.X + platforms[i].Areas[0].size.X >= temporaryPlatform.Position.X + temporaryPlatform.Size.X)
                    || (platforms[i].Areas[0].position.Y <= temporaryPlatform.Position.Y
                        && platforms[i].Areas[0].position.Y + platforms[i].Areas[0].size.Y >= temporaryPlatform.Position.Y
                        && platforms[i].Areas[0].position.X <= temporaryPlatform.Position.X
                        && platforms[i].Areas[0].position.X + platforms[i].Areas[0].size.X >= temporaryPlatform.Position.X))
                {
                    temporaryPlatform = CreateCoordinatesForPlatform(platform, area, player);
                    i = 0;
                    numberOfTimes++;
                    if (numberOfTimes > 50)
                        break;
                }  //Defenetly not an error in this for loop

                if (numberOfTimes > 50)
                {
                    temporaryPlatform.Color = BLUE;
                    break;
                }
            }

            area.platformPlaced = true;

            return temporaryPlatform;
        }

        public Platform CreateCoordinatesForPlatform(Platform platform, Area area, MovingObjects player)
        {
            Random rand = new Random();
            Vector2 platformPosition;
            Vector2 platformSize = new Vector2(0, 0);

            do
            {
                platformPosition = new Vector2(rand.Next((int) area.position.X + 50, (int)(area.position.X + area.size.X)),
                                               rand.Next((int) area.position.Y, (int) (area.position.Y + area.size.Y - player.Size.Y))); // X coordinate changed so that size can not get below 50

                if(platformPosition.X < 0)
                {
                    platformPosition.X = 0;
                }
                else if(platformPosition.X > GetScreenWidth())
                {
                    platformPosition.X = area.position.X; //could be a mistake
                }


                if (area.information.Contains("Spawn Area left") && platformPosition.X <= GetScreenWidth() / 4) // no platforms in platform.Platforms
                {
                    platformSize = new Vector2(-platformPosition.X, 50);
                }
                else if (area.information.Contains("Spawn Area left")) // no platforms in platform.Platforms
                {
                    platformSize = new Vector2(-platformPosition.X / 2, 50);
                }
                else if (area.information.Contains("Spawn Area right") && platformPosition.X <= GetScreenWidth() - (GetScreenWidth() /4)) // no platforms in platform.Platforms)
                {
                    platformSize = new Vector2(rand.Next((int)(0.125 * (GetScreenWidth() - platformPosition.X)), (int) (0.5 *(GetScreenWidth() - platformPosition.X))), 50);
                }
                else if(area.information.Contains("Spawn Area right"))
                {
                    platformSize = new Vector2(rand.Next((int)(0.5 * (GetScreenWidth() - platformPosition.X)), (int)(GetScreenWidth() - platformPosition.X)), 50);
                }

                if (GetScreenWidth() - (platformPosition.X + platformSize.X) < 150)
                    platformSize.X = GetScreenWidth() - platformPosition.X;

            } while (platformPosition.Y + size.Y >= platform.Areas[0].position.Y // + size.X
                && (platformPosition.X + platformSize.X >= platform.Areas[0].position.X
                    && area.information.Contains("Spawn Area left")
                || (platformPosition.X <= platform.Areas[0].position.X + platform.Areas[0].size.X
                    && area.information.Contains("Spawn Area right"))));


            if (area.information.Contains("Spawn Area left"))
            {
                platformPosition.X = platformPosition.X + platformSize.X;
                platformSize.X = Math.Abs(platformSize.X);
            }

            return new Platform(platformPosition, platformSize, DARKGREEN, rand.Next(1, 4), "");

        }

        public Vector2 Position { get => position; set => position = value; }
        public Vector2 Size { get => size; set => size = value; }
        public string Information { get => information; set => information = value; }
        public bool PlatformPlaced { get => platformPlaced; set => platformPlaced = value; }
    }
}

/* 

-> 

 */

// «.»

