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

        public Platform CreateNewPlatforms(List<Platform> platforms, Area area, Platform platform, Player player) // Change every Areas[1] to the opposite area side of the original area
        {
            Platform closestPlatform = platforms[0];

            if (CheckForClosestArea(area, ref closestPlatform, platforms).position != new Vector2(0, 0))
            {
                Area closestArea = CheckForClosestArea(area, ref closestPlatform, platforms);
                area.platformPlaced = true;
                return CreatePlatformWithTwoAreas(area, platform, closestArea, closestPlatform, player);
            }
            else
            {
                area.platformPlaced = true;
                return CreatePlatformWithSingleArea(area, platform, platforms);
            }

        }

        public Platform CreatePlatformWithSingleArea(Area area, Platform platform, List<Platform> platforms)
        {
            Random rand = new Random();
            area.platformPlaced = true;
            Platform temporaryPlatform = CreateCoordinatesForPlatform(platform, area);

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
                    temporaryPlatform = CreateCoordinatesForPlatform(platform, area);
                    i = 0;
                }  //Defenetly not an error in this for loop
            }

            return new Platform(temporaryPlatform.Position, temporaryPlatform.Size, DARKGREEN, rand.Next(1, 3)); //Maybe Platform generation wrong because of rand.Next(1, 3) in multiple cases
        }

        public Platform CreateCoordinatesForPlatform(Platform platform, Area area)
        {
            Random rand = new Random();
            Vector2 platformPosition = new Vector2(0, 0);
            Vector2 platformSize = new Vector2(0, 0);

            do
            {
                int x = GetScreenWidth();
                platformPosition = new Vector2(rand.Next((int) area.position.X + 50, (int)(area.position.X + area.size.X)), rand.Next((int)area.position.Y, (int)(area.position.Y + area.size.Y))); // X coordinate changed so that size can not get below 50

                if (area.information.Contains("Spawn Area left") && area.position.X <= GetScreenWidth() / 2) // no platforms in platform.Platforms
                {
                    platformSize = new Vector2(-platformPosition.X, 50);
                }
                else if (area.information.Contains("Spawn Area left")) // no platforms in platform.Platforms
                {
                    platformSize = new Vector2(-platformPosition.X / 2, 50);
                }
                else if (area.information.Contains("Spawn Area right") && area.position.X <= GetScreenWidth() /2) // no platforms in platform.Platforms)
                {
                    platformSize = new Vector2(rand.Next((int)(0.125 * (GetScreenWidth() - platformPosition.X)), (int) (0.5 *(GetScreenWidth() - platformPosition.X))), 50);
                }
                else if(area.information.Contains("Spawn Area right"))
                {
                    platformSize = new Vector2(rand.Next((int)(0.5 * (GetScreenWidth() - platformPosition.X)), (int)(GetScreenWidth() - platformPosition.X)), 50);
                }

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

            return new Platform(platformPosition, platformSize, DARKGREEN, rand.Next(1, 3));

        }

        public string RemoveVectorsFromString(string inputString, char startingCharacter, char endingCharacter)
        {
            int positionOfStartingCharacter = 0;
            int positionOfEndingCharacter = 0;

            while (inputString.Contains(startingCharacter))
            {
                positionOfStartingCharacter = inputString.IndexOf(startingCharacter) - 2;
                positionOfEndingCharacter = inputString.IndexOf(endingCharacter) + 1;

                inputString = inputString.Remove(positionOfStartingCharacter, positionOfEndingCharacter - positionOfStartingCharacter);
            }

            return inputString;
        }

        public Platform CreatePlatformWithTwoAreas(Area leftArea, Platform leftPlatform, Area rightArea, Platform rightPlatform, Player player)
        {
            Platform platform;
            Random rand = new Random();
            leftArea.platformPlaced = true;
            rightArea.platformPlaced = true;
            Vector2 minimumPosition;
            Vector2 maximumPosition;
            Vector2 position;
            Vector2 size;

            if(leftArea.position.X > rightArea.position.X)
            {
                Area temporaryArea = rightArea;
                Platform temporaryPlatform = rightPlatform;
                rightArea = leftArea;
                leftArea = temporaryArea;
                rightPlatform = leftPlatform;
                leftPlatform = temporaryPlatform;
            }


            if(leftArea.position.Y < rightArea.position.Y)
            {
                minimumPosition.Y = leftArea.position.Y;
                maximumPosition.Y = rightArea.position.Y + rightArea.size.Y;
            }
            else
            {
                minimumPosition.Y = rightArea.position.Y;
                maximumPosition.Y = leftArea.position.Y + leftArea.size.Y;
            }

            minimumPosition.X = leftArea.position.X;
            maximumPosition.X = leftArea.position.X + leftArea.size.Y;

            do
            {
                position.Y = rand.Next((int)minimumPosition.Y, (int)maximumPosition.Y);
                position.X = rand.Next((int)minimumPosition.X, (int)maximumPosition.X);
                size.Y = 50;
                size.X = rand.Next((int)(rightArea.position.X - position.X), (int)(rightArea.position.X + rightArea.size.X - position.X));
            } while (position.Y < leftPlatform.Areas[0].Position.Y
                    && position.Y < rightPlatform.Areas[0].Position.Y
                    && position.X > leftPlatform.Areas[0].position.X + leftPlatform.Areas[0].size.X
                    && position.X + size.X < rightPlatform.Areas[0].position.X);

            leftArea.PlatformPlaced = true;
            rightArea.PlatformPlaced = true;

            return new Platform(position, size, PINK, rand.Next(1, 3));
        }

        public bool CheckIfPlatformsAreInBetweenTwoPlatforms(Area leftArea, Area rightArea, List<Platform> platforms, int distanceBetweenAreaAAndAreaB)
        {
            Area areaLowerToTheBottom;
            Area areaUpperToTheTop;

            if(distanceBetweenAreaAAndAreaB > 0)
            {
                areaLowerToTheBottom = leftArea;
                areaUpperToTheTop = rightArea;
            }
            else
            {
                areaLowerToTheBottom = rightArea;
                areaUpperToTheTop = leftArea;
            }

            if (leftArea.position.X > rightArea.position.X)
            {
                Area temporaryArea = leftArea;
                leftArea = rightArea;
                rightArea = temporaryArea;
            }

            foreach (Platform platform in platforms)
            {
                if (areaLowerToTheBottom.position.Y <= areaUpperToTheTop.position.Y + areaUpperToTheTop.size.Y && leftArea.position.X <= platform.Areas[0].position.X + platform.Areas[0].size.X && rightArea.position.X + rightArea.size.X >= platform.Areas[0].position.X && ((areaLowerToTheBottom.position.Y <= platform.Areas[0].position.Y && areaUpperToTheTop.position.Y + areaUpperToTheTop.size.X >= platform.Areas[0].position.Y) || (areaLowerToTheBottom.position.Y <= platform.Areas[0].position.Y + platform.Areas[0].size.Y && areaUpperToTheTop.position.Y + areaUpperToTheTop.size.X >= platform.Areas[0].position.Y + platform.Areas[0].size.Y)))
                {
                    platform.Areas[0].information = platform.Areas[0].information + ", " + leftArea.position + ", " + rightArea.position;
                    return true;
                }
            }

            return false;
        }

        public bool FitsPlatformInArea(Area area, Vector2 position, Vector2 size)
        {
            Vector2 positionsize = new Vector2(position.X + size.X, position.Y + size.Y);

            if (position.X > area.position.X && position.Y > area.position.Y && positionsize.X < area.position.X + area.size.X && positionsize.Y < area.position.Y + area.size.Y)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public List<int> QuickSortAreas(List<int> distanceBetweenAreas)
        {
            List<int> leftQuatile = new List<int>();
            List<int> rightQuatile = new List<int>();
            List<int> quickSortedList = new List<int>();

            for(int i = 0; i < distanceBetweenAreas.Count; i++)
            {
                if(i < distanceBetweenAreas.Count / 2 && i > 1)
                {
                    leftQuatile.Add(distanceBetweenAreas[i]);
                }
                else if(i >1)
                {
                    rightQuatile.Add(distanceBetweenAreas[i]);
                }
                else
                {
                    quickSortedList.Add(distanceBetweenAreas[0]);
                }
            }

            rightQuatile = QuickSortAreas(rightQuatile);
            leftQuatile = QuickSortAreas(leftQuatile);

            return quickSortedList = MergeSortDistances(leftQuatile, rightQuatile);

        }

        public List<int> MergeSortDistances(List<int> leftSide, List<int> rightSide)
        {
            List<int> mergedList = new List<int>();

            int indexLeft = 0;
            int indexRight = 0;

            while (indexLeft < leftSide.Count && indexRight < rightSide.Count)
            {
                if (leftSide[indexLeft] < rightSide[indexRight])
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

        public Area CheckForClosestArea(Area area, ref Platform closestPlatform,  List<Platform> platforms)
        {
            float distanceBetweenAreas = 0;
            Area closestArea = new Area(new Vector2(0, 0), new Vector2(0, 0), "", false);

            foreach(Platform platform in platforms)
            {
                foreach(Area exampleArea in platform.Areas)
                {
                    Vector2 x = new Vector2((int) (exampleArea.position.X + exampleArea.size.X), (int) (exampleArea.position.Y + exampleArea.size.Y));
                    Vector2 xx = new Vector2((int) (area.position.X + area.size.X), (int) (area.position.Y + area.size.Y));

                    if (area.information.Contains("Spawn Area left")
                        && exampleArea.Information.Contains("Spawn Area right") && !exampleArea.platformPlaced
                        && (int) exampleArea.position.X != (int) (area.position.X + area.size.X + platform.Size.X)
                        && (area.position.Y <= exampleArea.position.Y && area.position.Y + area.size.Y >= exampleArea.position.Y
                            || exampleArea.position.Y <= area.position.Y && exampleArea.position.Y + exampleArea.size.Y >= area.position.Y)
                        && area.position.X - exampleArea.Position.X < distanceBetweenAreas)
                    {
                        distanceBetweenAreas = area.position.X - exampleArea.Position.X;
                        closestArea = exampleArea;
                        closestPlatform = platform;
                    } // here
                    else if(area.information.Contains("Spawn Area right")
                        && exampleArea.Information.Contains("Spawn Area left") && !exampleArea.platformPlaced
                        && (int) area.position.X != (int) (exampleArea.position.X + exampleArea.size.X + platform.Size.X)
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

