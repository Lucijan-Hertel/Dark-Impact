using System;
using System.Numerics;
using static Raylib_CsLo.Raylib;
using PixelJump.InterfaceObjects;

namespace PixelJump.Screens
{
	public class StartScreen : Screen
	{
        Button button = new Button(new Vector2(0, 0), new Vector2(0, 0), "", null, "", false);
        List<Button> buttons = new List<Button>();

        Screen gameScreen = new GameScreen();
        Screen leaderboardScreen = new Leaderboard();
        Screen settingsScreen = new SettingsScreen();

        bool firstRender = false;

        public virtual void Update()
        {
            if (!firstRender)
            {
                InitializeObjects();
                firstRender = true;
            }

            button.UpdateButton(buttons);
        }

        public virtual void Draw()
        {
            button.drawButton(buttons, "startScreen");
        }

        public void InitializeObjects()
        {
            button.CreateButton(buttons, new Vector2((float)0.5 * (GetScreenWidth() - 300), 500), new Vector2(300, 100), "Play", "startScreen", gameScreen, null);
            button.CreateButton(buttons, new Vector2((float)0.5 * (GetScreenWidth() - 300), 650), new Vector2(300, 100), "Settings", "startScreen", settingsScreen, null);
            button.CreateButton(buttons, new Vector2((float)0.5 * (GetScreenWidth() - 300), 800), new Vector2(300, 100), "Leaderboard", "startScreen", gameScreen, null);
            button.CreateButton(buttons, new Vector2((float)0.5 * (GetScreenWidth() - 300), 950), new Vector2(300, 100), "Quit", "startScreen", gameScreen, null);
        }

        public void drawHeading(Vector2 position, float fontSize, string headingText)
        {
            DrawText(headingText, position.X, position.Y, fontSize, BLACK);
        }
    }
}

