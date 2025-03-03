using System;
using System.Numerics;
using System.Runtime.InteropServices;
using static Raylib_CsLo.Raylib;

namespace PixelJump.Objects
{
    public class Enemy : MovingObjects
    {
        //--Attributes--//

        private Vector2 position = new Vector2();
        private Vector2 resultantPosition = new Vector2();
        private Vector2 size = new Vector2();
        private Vector2 distance = new Vector2();
        private Vector2 velocity = new Vector2();
        private Vector2 initialVelocity = new Vector2();
        private Vector2 maximumVelocity = new Vector2();
        private Vector2 acceleration = new Vector2();
        private Vector2 maximumAcceleration = new Vector2();
        private bool spaceGotPressed;

        public Enemy(Vector2 position, Vector2 size, Vector2 distance, Vector2 velocity, Vector2 initialVelocity, Vector2 maximumVelocity, Vector2 acceleration, Vector2 maximumAcceleration, bool spaceGotPressed) : base(position, size, distance, velocity, initialVelocity, maximumVelocity, acceleration, maximumAcceleration, spaceGotPressed)
        {
            this.position = position;
            this.size = size;
            this.distance = distance;
            this.velocity = velocity;
            this.initialVelocity = initialVelocity;
            this.maximumVelocity = maximumVelocity;
            this.acceleration = acceleration;
            this.spaceGotPressed = spaceGotPressed;
        }

        //--Methods--//

        public override Vector2 CalculatingVelocity(Vector2 initialVelocity,
                                                    Vector2 acceleration,
                                                    List<Platform> platforms,
                                                    MovingObjects movingObject,
                                                    MovingObjects enemy)
        {
            if (velocity.X == 0 && CheckIfMovingObjectCollidesWithObject(platforms).Contains("bottom"))
                resultantPosition = setRandomPointOnPlatform(platforms, movingObject, enemy);
            else if (velocity.X == 0)
                resultantPosition = position;

            velocity = base.CalculatingVelocity(initialVelocity, acceleration, platforms, movingObject, enemy);

            List<string> collisions = CheckIfMovingObjectCollidesWithObject(platforms);

            if (resultantPosition.X < (int) movingObject.Position.X && collisions.Contains("bottom") && !movingObject.SpaceGotPressed)
            {
                velocity.X = maximumVelocity.X; 
            }
            else if(resultantPosition.X > (int) movingObject.Position.X && collisions.Contains("bottom") && !movingObject.SpaceGotPressed)
            {
                velocity.X = -maximumVelocity.X;
            }
            else if (!movingObject.SpaceGotPressed && collisions.Contains("bottom")) // should get knockback if hit and not on platform
            {
                velocity.X = 0;
            }

            return velocity;
        }

        public Vector2 setRandomPointOnPlatform(List<Platform> platforms, MovingObjects enemy, MovingObjects player)
        {
            Platform platformPlayerIsStandingOn = getPlatformPlayerIsStandingOn(platforms, enemy);
            Random rand = new Random();
            int positionEnemySHouldWalkTo = rand.Next((int)platformPlayerIsStandingOn.Position.X,
                (int)(platformPlayerIsStandingOn.Position.X + platformPlayerIsStandingOn.Size.X)); //Fix Platform size -> then -50

            Vector2 distanceBetweenEnemyAndPlayer = enemy.Position - player.Position - new Vector2(50, 0);

            if (distanceBetweenEnemyAndPlayer.X > -500 && distanceBetweenEnemyAndPlayer.X < 500
                && distanceBetweenEnemyAndPlayer.Y > -500 && distanceBetweenEnemyAndPlayer.Y < 500
                && (distanceBetweenEnemyAndPlayer.X > enemy.Position.X - platformPlayerIsStandingOn.Position.X))
            {
                positionEnemySHouldWalkTo = (int) platformPlayerIsStandingOn.Position.X;
            }
            else if(distanceBetweenEnemyAndPlayer.X > -500 && distanceBetweenEnemyAndPlayer.X < 500
                && distanceBetweenEnemyAndPlayer.Y > -500 && distanceBetweenEnemyAndPlayer.Y < 500
                && Math.Abs(distanceBetweenEnemyAndPlayer.X) > Math.Abs(enemy.Position.X - platformPlayerIsStandingOn.Position.X - platformPlayerIsStandingOn.Size.X))
            {
                positionEnemySHouldWalkTo = (int) (platformPlayerIsStandingOn.Position.X + platformPlayerIsStandingOn.Size.X - 50d);
            }
            else if(distanceBetweenEnemyAndPlayer.X > -500 && distanceBetweenEnemyAndPlayer.X < 500
                && distanceBetweenEnemyAndPlayer.Y > -500 && distanceBetweenEnemyAndPlayer.Y < 500)
            {
                positionEnemySHouldWalkTo = (int) player.Position.X;
            }

            return new Vector2(positionEnemySHouldWalkTo, enemy.Position.Y);
        }

        public Platform getPlatformPlayerIsStandingOn(List<Platform> platforms, MovingObjects enemy)
        {
            foreach (Platform platform in platforms)
            {
                if((int)platform.Position.X - 1 <= (int)(enemy.Position.X + enemy.Size.X)
                    && (int)enemy.Position.X <= (int)platform.Position.X + platform.Size.X + 1
                    && (int)platform.Position.Y - 1 == (int)enemy.Position.Y + enemy.Size.Y)
                {
                    return platform;
                }
            }

            return new Platform(new Vector2(0, 0), new Vector2(0, 0), MAGENTA, 0, "");
        }

        public override void InitilizePlatforms(List<Platform> platforms, List<MovingObjects> enemies)
        {
            Random rand = new Random();

            foreach(Platform platform in platforms)
            {
                if(!platform.Information.Contains("enemy initialisation finished"))
                {
                    platform.Information = platform.Information + "enemy initialisation finished";

                    if(rand.Next(3) == 1)
                    {
                        MovingObjects enemy = new Enemy(new Vector2(platform.Position.X, platform.Position.Y - 50 - 1),
                                              new Vector2(50, 50),
                                              new Vector2(0, 0),
                                              new Vector2(0, 0),
                                              new Vector2(0, 0),
                                              new Vector2(200, 53 * 18),
                                              new Vector2(0, 0),
                                              new Vector2(0, (float)-9.8 * 18),
                                              false);
                        enemies.Add(enemy);
                    }
                }
            }
        }

        public override void removeDeadEnemies(List<MovingObjects> enemies)
        {
            List<MovingObjects> enemiesToRemove = new List<MovingObjects>();

            foreach(MovingObjects enemy in enemies)
            {
                if(enemy.Health <= 0)
                {
                    enemiesToRemove.Add(enemy);
                }
            }

            foreach(MovingObjects enemy in enemiesToRemove)
            {
                enemies.Remove(enemy);
            }
        }

        public override void Attack(MovingObjects attacker, List<MovingObjects> attackedObjects, List<MovingObjects> enemies)
        {
            Random rand = new Random();
            try
            {
                if (Math.Abs(attacker.Position.X - attackedObjects[0].Position.X) < 100
                    && attacker.Position.Y - attackedObjects[0].Position.Y >= -1
                    && attacker.Position.Y - attackedObjects[0].Position.Y < 100
                    && rand.Next(1, GetFPS() * 3) == 1)
                {
                    base.Attack(attacker, attackedObjects, enemies);
                }
            }
            catch (Exception e)
            {

            }
        }
    }
}