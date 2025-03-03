using System;
using System.Numerics;
using System.Runtime.InteropServices;
using static Raylib_CsLo.Raylib;

namespace PixelJump.Objects
{
    public class Debug
    {
        //--Attributes--//

        private int initialNumberOfPlatforms = 0;

        //--Methods--//

        public void DrawSpawnProtectionArea(List<Platform> platforms, Raylib_CsLo.Color color)
        {
            foreach (Platform platform in platforms)
            {
                for(int i = 0; i < platform.Areas.Count; i++)
                {
                    if (platform.Areas[i].Information.Contains("Spawn protection") && platform.Color.Equals(BLUE))
                    {
                        Area areaToDraw = platform.Areas[i];
                        DrawRectangle((int)areaToDraw.Position.X, (int)areaToDraw.Position.Y, (int)areaToDraw.Size.X, (int)areaToDraw.Size.Y, color);
                    }
                }
            }
        }

        public void DrawAreasWithPatformsPlacedInIt(List<Platform> platforms, Raylib_CsLo.Color color)
        {
            foreach (Platform platform in platforms)
            {
                foreach(Area area in platform.Areas)
                {
                    if (area.PlatformPlaced)
                        DrawRectangle((int)area.Position.X, (int)area.Position.Y, (int)area.Size.X, (int)area.Size.Y, color);
                }
            }
        }

        public void DrawAreasWithNoPlatformsPlacedInIt(List<Platform> platforms, Raylib_CsLo.Color color)
        {
            foreach (Platform platform in platforms)
            {
                foreach (Area area in platform.Areas)
                {
                    if (!area.PlatformPlaced)
                        DrawRectangle((int)area.Position.X, (int)area.Position.Y, (int)area.Size.X, (int)area.Size.Y, color);
                }
            }
        }

        public void WriteCoordinatesOfEveryNewPlatformInConsole(List<Platform> platforms)
        {
            for (int i = initialNumberOfPlatforms; i < platforms.Count - 1; i++)
            {
                Console.WriteLine("Position: " + platforms[i].Position.Y + ", PosSize: " + platforms[i].Position.Y + platforms[i].Size.Y);
            }

            initialNumberOfPlatforms = platforms.Count - 1;
        }

        public void FreezeEveryMovingObject()
        {

        }
    }
}

// «.»