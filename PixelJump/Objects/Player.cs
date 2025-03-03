using System;
using System.Numerics;
using System.Runtime.InteropServices;
using static Raylib_CsLo.Raylib;

namespace PixelJump.Objects
{
    public class Player : MovingObjects
    {
        //- Attributes Player -------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------//

        Raylib_CsLo.Color color = RED;

        //-SUVAT-//

        private Vector2 position = new Vector2();
        private Vector2 size = new Vector2();
        private Vector2 velocity = new Vector2();
        private Vector2 initialVelocity = new Vector2();
        private Vector2 acceleration = new Vector2();
        private Vector2 distance = new Vector2();
        private Vector2 maximumVelocity = new Vector2();
        private Vector2 maximumAcceleration = new Vector2();
        private bool spaceGotPressed;

        public Player(Vector2 position, Vector2 size, Vector2 distance, Vector2 velocity, Vector2 initialVelocity, Vector2 maximumVelocity, Vector2 acceleration, Vector2 maximumAcceleration, Raylib_CsLo.Color color, bool spaceGotPressed) : base(position, size, distance, velocity, initialVelocity, maximumVelocity, acceleration, maximumAcceleration, spaceGotPressed) //Generates a new player object everytime called
        {
            this.size = size;
            this.position = position;
            this.velocity = velocity;
            this.initialVelocity = initialVelocity;
            this.maximumVelocity = maximumVelocity;
            this.acceleration = acceleration;
            this.maximumAcceleration = maximumAcceleration;
            this.color = color;
            this.spaceGotPressed = spaceGotPressed;
        }

        public Raylib_CsLo.Color Color { get => color; set => color = value; }

        //- General Methods -----------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------//

        public override void DrawMovingObject(MovingObjects player)
        {
            DrawRectangle((int)player.Position.X, (int)player.Position.Y, (int)player.Size.X, (int)player.Size.Y, color);
        }

        //--General Movement--//

        //-SUVAT-//

        public override Vector2 SettingInitialVelocity(Vector2 velocity, List<Platform> platforms, MovingObjects player)
        {
            Vector2 initialVelocity = base.SettingInitialVelocity(velocity, platforms, player);

            if(CheckIfMovingObjectCollidesWithObject(platforms).Contains("bottom") && IsKeyPressed(Raylib_CsLo.KeyboardKey.KEY_SPACE))
            {
                initialVelocity.Y = 200;
                player.SpaceGotPressed = true;
            }

            return initialVelocity;
        }

        public override Vector2 CalculatingVelocity(Vector2 initialVelocity, Vector2 acceleration, List<Platform> platforms, MovingObjects movingObject, MovingObjects enemy)
        {
            velocity = base.CalculatingVelocity(initialVelocity, acceleration, platforms, movingObject, enemy);

            if (IsKeyDown(Raylib_CsLo.KeyboardKey.KEY_A) && !CheckIfMovingObjectCollidesWithObject(platforms).Contains("left"))
            {
                velocity.X = maximumVelocity.X;
            }
            else if (IsKeyDown(Raylib_CsLo.KeyboardKey.KEY_D) && !CheckIfMovingObjectCollidesWithObject(platforms).Contains("right"))
            {
                velocity.X = -maximumVelocity.X;
            }
            else
            {
                velocity.X = 0;
            }

            return velocity;
        }

        //-Health System and Fall damage-//

        //--Fall Damage--//

        public int CheckingIfVelocityIsOverLimitAndCalculatingFallDamage(int limit, float multipliator)
        {
            int fallDamage = 0;

            if(velocity.Y <= -limit)
            {
                fallDamage = (int)(Math.Pow(velocity.Y, 2) * multipliator);
            }

            return fallDamage;
        }

        //--Health and Attack--//

        public override void Attack(MovingObjects attacker, List<MovingObjects> attackedObjects, List<MovingObjects> enemies)
        {
            Vector2 velocity = new Vector2(0, 0);

            attackedObjects = EnemiesNearPlayer(enemies ,attacker);

            if (IsKeyPressed(Raylib_CsLo.KeyboardKey.KEY_ENTER))
            {
                foreach (MovingObjects attackedObject in attackedObjects)
                    enemies.Remove(attackedObject);

                base.Attack(attacker, attackedObjects, enemies);

                foreach (MovingObjects attackedObject in attackedObjects)
                    enemies.Add(attackedObject);

            }
        }

        public List<MovingObjects> EnemiesNearPlayer(List<MovingObjects> enemies, MovingObjects player)
        {
            List<MovingObjects> attackedObjects = new List<MovingObjects>();

            foreach(MovingObjects enemy in enemies)
            {
                if (Math.Abs(player.Position.X - enemy.Position.X) < 100
                    && Math.Abs(player.Position.Y - enemy.Position.Y) < 100) //20 is range. replace with different variable to make it edible in the game
                {
                    attackedObjects.Add(enemy);
                }
            }

            return attackedObjects;
        }

        public override void DrawHealth(MovingObjects player)
        {
            DrawText(player.Health.ToString(), 100, 100, 50, RED);
        }
    }
}

/* 

-> If a & d movement works, test if collision works defenetly | ✔
-> New System for the horizontal movement | ✔
-> New system for hanging on platforms if player Y position equals platform Y position | ✔
-> Health system with fall damage | ✔
-> Collision fixing | ✔
-> Implementing Enemy collision | ✔ 
-> Implementing extra Collision with blue wall | !

 */

// «.»