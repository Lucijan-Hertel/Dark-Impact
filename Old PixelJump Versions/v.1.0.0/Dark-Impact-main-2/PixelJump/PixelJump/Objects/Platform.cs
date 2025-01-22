using System;
using System.Numerics;
using static Raylib_CsLo.Raylib;

namespace PixelJump
{
	public class Platform
	{
        Vector2 position;
        Vector2 size;
        Raylib_CsLo.Color color;
        List<Platform> platforms = new List<Platform>();

        public Platform(Vector2 position, Vector2 size, Raylib_CsLo.Color color) //Generates a new platform object everytime called
        {
            this.position = position;
            this.size = size;
            this.color = color;
        }

        public Raylib_CsLo.Color Color { get => color; set => color = value; }
        public Vector2 Position { get => position; set => position = value; }
        public List<Platform> Platforms { get => platforms; set => platforms = value; }
        public Vector2 Size { get => size; set => size = value; }

        public void DrawPlatforms() //Draws platform when called => Easier to identify in code when platform is drawn && easier to search for
        {
            foreach (Platform platform in platforms)
            {
                DrawRectangle((int) platform.position.X, (int) platform.position.Y, (int) platform.size.X, (int) platform.size.Y, platform.color);
            }
        }
    }
}

