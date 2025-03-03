using System;
using System.Numerics;
using static Raylib_CsLo.Raylib;
using static Raylib_CsLo.RayGui;
using PixelJump.Objects;
using Raylib_CsLo;

namespace PixelJump.Screens
{
    public class GameScreen : Screen
    {
        //-New Instances-//
        MovingObjects player = new Player(new Vector2(200, 100 /*956*/), new Vector2(50, 50), new Vector2(0, 0), new Vector2(0, 0), new Vector2(0, 0), new Vector2(200, 53 * 18), new Vector2(0, 0), new Vector2(0, (float)-9.8 * 18), BLUE, false);
        MovingObjects enemy = new Enemy(new Vector2(400, 956), new Vector2(50, 50), new Vector2(0, 0), new Vector2(0, 0), new Vector2(0, 0), new Vector2(200, 53 * 18), new Vector2(0, 0), new Vector2(0, (float)-9.8 * 18), false);
        Platform platform = new Platform(new Vector2(), new Vector2(), RED, 0, "");
        Area area = new Area(new Vector2(0, 0), new Vector2(0, 0), "", false);
        Debug debug = new Debug();

        List<Vector2> initialSettings = new List<Vector2>();
        List<List<MovingObjects>> enemies = new List<List<MovingObjects>>();
        List<MovingObjects> players = new List<MovingObjects>();
        List<List<Platform>> platformScreens = new List<List<Platform>>();

        int numberOfCurrentScreen = 0;

        bool oneTimeInitialisation = false;

        public virtual void Update()
        {
            if (!oneTimeInitialisation) //at some point in a constructor
            {
                List<Platform> platformScreen1 = new List<Platform>();
                List<MovingObjects> enemyScreen1 = new List<MovingObjects>();
                platformScreens.Add(platformScreen1);
                enemies.Add(enemyScreen1);
                platformScreens[numberOfCurrentScreen].Add(new Platform(new Vector2(0, GetScreenHeight() - 100), new Vector2(GetScreenWidth(), 100), DARKGREEN, 2, ""));
                platform.AllocateAreasForPlatform(platformScreens[numberOfCurrentScreen][0], player);
                platformScreens[numberOfCurrentScreen] = platform.sortPlatforms(platformScreens[numberOfCurrentScreen]);
                players.Add(player);

                while (platformScreens[numberOfCurrentScreen][platformScreens[numberOfCurrentScreen].Count - 1].Position.Y > 100 /*platform.Platforms.Count < 12*/)
                {
                    platform.AllocateAreasAndPlacePlatformsForIt(platformScreens[numberOfCurrentScreen], area, player);
                    enemy.InitilizePlatforms(platformScreens[numberOfCurrentScreen], enemies[numberOfCurrentScreen]);  // Enemy size is 50 here, change in future
                    debug.WriteCoordinatesOfEveryNewPlatformInConsole(platformScreens[numberOfCurrentScreen]);
                }

                oneTimeInitialisation = true;
            }

            List<MovingObjects> attackedObjects = new List<MovingObjects>();

            player.Attack(player, attackedObjects, enemies[numberOfCurrentScreen]);
            player.HealthSystem(platformScreens[numberOfCurrentScreen]);
            player.MovementCalculation(platformScreens[numberOfCurrentScreen], player, null);

            enemy.removeDeadEnemies(enemies[numberOfCurrentScreen]);

            foreach (Enemy enemy in enemies[numberOfCurrentScreen])
            {
                enemy.Attack(enemy, players, enemies[numberOfCurrentScreen]);
                player = players[0];
                enemy.MovementCalculation(platformScreens[numberOfCurrentScreen], enemy, player);
            }

            platform.GenerateNewPlatformsAndSaveOldOnes(platformScreens, enemies, ref numberOfCurrentScreen, player, enemy, area);
        }

        public virtual void Draw()
        {
            //debug.DrawSpawnProtectionArea(platformScreens[numberOfCurrentScreen], BLACK);
            //debug.DrawAreasWithPatformsPlacedInIt(platformScreens[numberOfCurrentScreen], YELLOW);
            debug.WriteCoordinatesOfEveryNewPlatformInConsole(platformScreens[numberOfCurrentScreen]);
            player.DrawHealth(player);
            platform.DrawPlatforms(platformScreens[numberOfCurrentScreen]);
            foreach(Enemy enemy in enemies[numberOfCurrentScreen])
            {
                enemy.DrawMovingObject(enemy);
            }
            player.DrawMovingObject(player);
        }
    }
}

