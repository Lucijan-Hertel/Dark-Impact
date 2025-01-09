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
        string positionFromPlatform;
        bool twoPlatfoms = false;

        public Area(Vector2 position, Vector2 size, string information, bool platformPlaced)
        {
            this.position = position;
            this.size = size;
            this.information = information;
            this.platformPlaced = platformPlaced;
        }

        //--Platform Generation--//

        public Platform CreateNewPlatforms(Player player, List<Platform> platforms, Area area) // Change every Areas[1] to the opposite area side of the original area
        {
            List<Platform> selectedPlatforms = new List<Platform>();
            List<int> distanceBetweenSelectedAreas = new List<int>();
            Vector2 distanceBetweenAreaAAndAreaB = new Vector2();
            float smallestDistanceBetweenPlatforms = 1000000000;
            Platform closestPlatform = platforms[0];


            foreach (Platform examplePlatfrom in platforms) // filtering out which areas could be potential pairs
            {
                distanceBetweenAreaAAndAreaB.Y = Math.Abs((int) (area.position.Y - examplePlatfrom.Areas[1].position.Y));
                distanceBetweenAreaAAndAreaB.X = Math.Abs((int)(area.position.X - examplePlatfrom.Areas[1].position.X));
                float maximumJumpHeight = player.CalculatingDistance(new Vector2(0, 200), new Vector2(0, (float) -9.8*18), player.CalculatingTimeTillMaximumJumpHeight(new Vector2(0, 200))).Y;

                if ((distanceBetweenAreaAAndAreaB.Y >= -maximumJumpHeight || distanceBetweenAreaAAndAreaB.Y <= maximumJumpHeight) && !CheckIfPlatformsAreInBetweenTwoPlatforms(area, examplePlatfrom.Areas[1], platforms, (int) distanceBetweenAreaAAndAreaB.Y) && examplePlatfrom.Areas[1].position != area.position)
                {
                    if (smallestDistanceBetweenPlatforms > distanceBetweenAreaAAndAreaB.X)
                    {
                        smallestDistanceBetweenPlatforms = distanceBetweenAreaAAndAreaB.X;
                    }
                }
                else if(CheckIfPlatformsAreInBetweenTwoPlatforms(area, examplePlatfrom.Areas[1], platforms, (int)distanceBetweenAreaAAndAreaB.Y) && examplePlatfrom.Areas[0].position != area.position)
                {
                    // Code for Platformgeneration with only one Area here

                    return CreatePlatformWithSingleAreaWhereAreaIsInBetween(area, examplePlatfrom.Platforms);
                }
                else
                {
                    return CreatePlatformWithSingleArea(area);
                }
            }

            return CreatePlatformWithTwoAreas(area, closestPlatform.Areas[1], smallestDistanceBetweenPlatforms);
        }

        public Platform CreatePlatformWithSingleArea(Area area)
        {
            Random rand = new Random();
            Vector2 position = new Vector2(0, 0);
            Vector2 size = new Vector2(0, 0);

            position = new Vector2(rand.Next((int)area.position.X, (int)(area.position.X + area.size.X)), rand.Next((int)area.position.Y, (int)(area.position.Y + area.size.Y)));
            if (area.information.Contains("Spawn Area left"))
            {
                size = new Vector2(rand.Next(0, (int) position.X -25), 25);
            }
            else if (area.information.Contains("Spawn Area left"))
            {
                size = new Vector2(rand.Next((int)position.X + 25, GetScreenWidth()), 25);
            }

            return new Platform(position, size, GREEN, rand.Next(0, 2));
        }

        public Platform CreatePlatformWithSingleAreaWhereAreaIsInBetween(Area area, List<Platform> platfoms) // Could be that a platform spawns in another platform
        {
            Area protectionArea = new Area(new Vector2(0, 0), new Vector2(0, 0), "", false);
            Random rand = new Random();
            Vector2 position = new Vector2();
            Vector2 size = new Vector2();

            foreach (Platform platform in platfoms)
            {
                if (platform.Areas[0].information.Contains("blocking two Areas from linking"))
                {
                    protectionArea = platform.Areas[0];
                    // Remove text searched for above from information
                }
            }

            position = new Vector2(rand.Next((int)area.position.X, (int)(area.position.X + area.size.X)), rand.Next((int)area.position.Y, (int)(area.position.Y + area.size.Y)));
            if (area.position.X < protectionArea.position.X)
            {
                size = new Vector2(rand.Next((int) position.X + 25, (int) protectionArea.position.X), 25);
            }
            else
            {
                size = new Vector2(rand.Next((int) (protectionArea.position.X + protectionArea.size.X), (int) position.X), 25);
            }

            return new Platform(position, size, GREEN, rand.Next(0, 2));
        }

        public Platform CreatePlatformWithTwoAreas(Area leftArea, Area rightArea, float smallestDistanceBetweenPlatforms)
        {
            Platform platform;
            Random rand = new Random();
            Vector2 minimumPosition;
            Vector2 maximumPosition;
            Vector2 position;
            Vector2 size;

            if(leftArea.position.X > rightArea.position.X)
            {
                Area temporaryArea = rightArea;
                rightArea = leftArea;
                leftArea = temporaryArea;
            }


            if(leftArea.position.Y < rightArea.position.X)
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

            position.Y = rand.Next((int) minimumPosition.Y, (int) maximumPosition.Y);
            position.X = rand.Next((int) minimumPosition.X, (int) maximumPosition.X);
            size.Y = 25;
            size.X = rand.Next((int)(rightArea.position.X - position.X), (int)(rightArea.position.X + rightArea.size.X - position.X));

            return new Platform(position, size, GREEN, rand.Next(0, 2));
        }

        public bool CheckIfPlatformsAreInBetweenTwoPlatforms(Area leftArea, Area rightArea, List<Platform> platforms, int distanceBetweenAreaAAndAreaB)
        {
            Vector2 positionOfArealowerToTheBottom;
            Random rand = new Random();

            if(distanceBetweenAreaAAndAreaB > 0)
            {
                positionOfArealowerToTheBottom = leftArea.position;
            }
            else
            {
                positionOfArealowerToTheBottom = rightArea.position;
            }

            if (leftArea.position.X > rightArea.position.X)
            {
                Area temporaryArea = leftArea;
                leftArea = rightArea;
                rightArea = temporaryArea;
            }

            foreach (Platform platform in platforms)
            {
                if ((leftArea.position.X >= platform.Areas[0].position.X + platform.Areas[0].size.X && rightArea.position.X + rightArea.size.X <= platform.Areas[0].position.X) && (leftArea.position.X + leftArea.size.X >= platform.Areas[0].position.X || rightArea.position.X + rightArea.size.X >= platform.Areas[0].position.X))
                {
                    platform.Areas[0].information = platform.Areas[0].information + ", blocking two Areas from linking";
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

            return quickSortedList = mergeSortDistances(leftQuatile, rightQuatile);

        }

        public List<int> mergeSortDistances(List<int> leftSide, List<int> rightSide)
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

        public Vector2 Position { get => position; set => position = value; }
        public Vector2 Size { get => size; set => size = value; }
        public string Information { get => information; set => information = value; }
        public bool PlatformPlaced { get => platformPlaced; set => platformPlaced = value; }
    }
}


// ln. 27

