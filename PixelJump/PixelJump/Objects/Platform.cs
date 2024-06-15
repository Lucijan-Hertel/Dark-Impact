using System;
using System.Numerics;
using static Raylib_CsLo.Raylib;

namespace PixelJump
{
	public class Platform
	{
        Vector2 position;
		int width;
		int height;
        Raylib_CsLo.Color color;
        List<Platform> platforms = new List<Platform>();

        public Platform(Vector2 position, int width, int height, Raylib_CsLo.Color color) //Generates a new platform object everytime called
        {
            this.position = position;
            this.width = width;
            this.height = height;
            this.color = color;
        }

        public int Width { get => width; set => width = value; }
        public int Height { get => height; set => height = value; }
        public Raylib_CsLo.Color Color { get => color; set => color = value; }
        public Vector2 Position { get => position; set => position = value; }
        public List<Platform> Platforms { get => platforms; set => platforms = value; }

        public void DrawPlatform(int posx, int posy, int width, int height, Raylib_CsLo.Color color) //Draws platform when called => Easier to identify in code when platform is drawn && easier to search for
        {
            DrawRectangle(posx, posy, width, height, color);
        }
    }
}

