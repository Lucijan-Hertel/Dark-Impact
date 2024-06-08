using System;
using Raylib_CsLo;

namespace PixelJump
{
	public class Platform
	{
		int posx;
		int posy;
		int width;
		int height;
		Color color;

        public Platform(int posx, int posy, int width, int height, Color color) //Generates a new platform object everytime called
        {
            this.posx = posx;
            this.posy = posy;
            this.width = width;
            this.height = height;
            this.color = color;
        }

        public int Xpos { get => posx; set => posx = value; }
        public int Ypos { get => posy; set => posy = value; }
        public int Width { get => width; set => width = value; }
        public int Height { get => height; set => height = value; }
        public Color Color { get => color; set => color = value; }

        public static void DrawPlatform(int posx, int posy, int width, int height, Color color) //Draws platform when called => Easier to identify in code when platform is drawn && easier to search for
        {
            Raylib.DrawRectangle(posx, posy, width, height, color);
        }
    }
}

