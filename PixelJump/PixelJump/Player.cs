using System;
using Raylib_CsLo;

namespace PixelJump
{
	public class Player
	{
        int size = 50;
        Color color = Raylib.RED;

        public Player(int size, Color color) //Generates a new player object everytime called
        {
            this.size = size;
            this.color = color;
        }

        public int Size { get => size; set => size = value; }
        public Color Color { get => color; set => color = value; }

        private static void DrawPlayer(int posx, int posy, int size, Color color) //Draws player when called => Easier to identify in code when player is drawn && easier to search for
		{
			Raylib.DrawRectangle(posx, posy, size, size, color);
		}
	}
}

