using System;
using static Raylib_CsLo.Raylib;

namespace PixelJump.Screens
{
	public class ScreenManager : Screen
	{
        //Initialize all screens
        Screen startScreen = new StartScreen();
        Screen gameScreen = new GameScreen();
        Screen leaderboardScreen = new Leaderboard();
        Screen settingsScreen = new SettingsScreen();
        Screen keybindingScreen = new keybindingScreen();
        Screen currentScreen;
        //---Constructer---//

        public ScreenManager()
        {
            currentScreen = startScreen;
        }

        //---Methods---//

        public virtual void Update()
        {

        }

        public virtual void Draw()
        {

        }
    }
}

