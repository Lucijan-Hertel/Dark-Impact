using System;
using System.Numerics;
using System.Runtime.InteropServices;
using static Raylib_CsLo.Raylib;

namespace PixelJump.Objects
{
    public class Player
    {
        //- Attributes Player -------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------//

        bool alreadyOnTop = false;
        bool spaceGotPressed = false;
        int health = 100;
        Vector2 size = new Vector2();
        Raylib_CsLo.Color color = RED;

        //-SUVAT-//

        private Vector2 position = new Vector2();
        private Vector2 velocity = new Vector2();
        private Vector2 initialVelocity = new Vector2();
        private Vector2 acceleration = new Vector2();
        private Vector2 distance = new Vector2();

        public Player(Vector2 size, Vector2 position, Vector2 distance, Vector2 velocity, Vector2 initialVelocity, Vector2 acceleration, Raylib_CsLo.Color color) //Generates a new player object everytime called
        {
            this.size = size;
            this.position = position;
            this.velocity = velocity;
            this.initialVelocity = initialVelocity;
            this.acceleration = acceleration;
            this.color = color;
        }

        public Vector2 Size { get => size; set => size = value; }
        public Vector2 Position { get => position; set => position = value; }
        public Vector2 Velocity { get => velocity; set => velocity = value; }
        public Vector2 Acceleration { get => acceleration; set => acceleration = value; }
        public Raylib_CsLo.Color Color { get => color; set => color = value; }
        public Vector2 InitialVelocity { get => initialVelocity; set => initialVelocity = value; }
        public Vector2 Distance { get => distance; set => distance = value; }
        public int Health { get => health; set => health = value; }

        //- General Methods -----------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------//

        public void DrawPlayer()
        {
            DrawRectangle((int) position.X, (int) position.Y, (int) size.X, (int) size.Y, color);
        }

        //--General Movement--//

        public void MovementCalculation(Platform platform)
        {
            try
            {
                this.acceleration = SettingAcceleration(platform);
                this.velocity = CalculatingVelocity(initialVelocity, acceleration, platform.Platforms);
                this.distance = CalculatingDistance(initialVelocity, acceleration, GetFrameTime());
                this.initialVelocity = SettingInitialVelocity(velocity, platform.Platforms, ref alreadyOnTop);
            }

            catch (Exception e) { }

            UpdatePlayerPosition(platform.Platforms, distance);
            UpdateSpaceVariable();
        }

        public void UpdateSpaceVariable()
        {
            if(velocity.Y < 0)
            {
                spaceGotPressed = false;
            }
        }

        public void UpdatePlayerPosition(List<Platform> platforms, Vector2 distance)
        {
            position.Y = position.Y - distance.Y;
            position.X = position.X - distance.X;
        }

        //-SUVAT-//

        public Vector2 CalculatingDistance(Vector2 initialVelocity, Vector2 acceleration, float time)
        {
            Vector2 distance;

            distance.Y = initialVelocity.Y * time + (float) 0.5 * acceleration.Y * (float) Math.Pow(time, 2);
            distance.X = initialVelocity.X * time + (float) 0.5 * acceleration.X * (float) Math.Pow(time, 2);

            return distance;
        }

        public Vector2 CalculatingVelocity(Vector2 initialVelocity, Vector2 acceleration, List<Platform> platforms)
        {
            Vector2 velocity;

            if(acceleration.Y != 0)
            {
                velocity.Y = initialVelocity.Y + acceleration.Y * GetFrameTime();
            }
            else
            {
                velocity.Y = 0;
            }

            if (IsKeyDown(Raylib_CsLo.KeyboardKey.KEY_A) && !CheckIfPlayerCollidesWithObject(platforms).Contains("left"))
            {
                velocity.X = (float)150;
            }
            else if (IsKeyDown(Raylib_CsLo.KeyboardKey.KEY_D) && !CheckIfPlayerCollidesWithObject(platforms).Contains("right"))
            {
                velocity.X = (float)-150;
            }
            else
            {
                velocity.X = 0;
            }

            return velocity;
        }

        public Vector2 SettingInitialVelocity(Vector2 velocity, List<Platform> platforms, ref bool alreadyontop)
        {
            Vector2 initialVelocity;

            if(CheckIfPlayerCollidesWithObject(platforms).Contains("bottom") && IsKeyPressed(Raylib_CsLo.KeyboardKey.KEY_SPACE))
            {
                initialVelocity.Y = 200;
                spaceGotPressed = true;
                alreadyontop = false;
            }
            else if (CheckIfPlayerCollidesWithObject(platforms).Contains("top") && !alreadyontop && velocity.Y >= 0)
            {
                initialVelocity.Y = -velocity.Y;
                alreadyontop = true;
            }
            else
            {
                initialVelocity.Y = velocity.Y;
            }

            initialVelocity.X = velocity.X;
            return initialVelocity;
        }

        public float CalculatingTimeTillMaximumJumpHeight(Vector2 initialVelocity)
        {
            return (float) (-2*initialVelocity.Y/(2*(-9.8*18)));
        }

        //-Acceleration-//

        public Vector2 SettingAcceleration(Platform platform)
        {
            Vector2 acceleration;

            //-Vertical-//

            if (CheckIfPlayerCollidesWithObject(platform.Platforms).Contains("bottom") && !spaceGotPressed)
            {
                acceleration.Y = 0;
            }
            else
            {
                acceleration.Y = (float) -9.8*18;
            }

            //-Horizontal-//

            acceleration.X = 0;

            //-Return-//

            return acceleration;
        }


        //--Collision Checking--//

        public List<string> CheckIfPlayerCollidesWithObject(List<Platform> platforms)
        {
            List<string> Collisions = new List<string>();

            foreach(Platform platform in platforms)
            {
                if (platform.Position.X -1 <= (int) position.X + size.X && (int) position.X <= platform.Position.X + platform.Size.X + 1 && platform.Position.Y - 1 == (int)position.Y + size.Y)
                {
                    Collisions.Add("bottom");
                }
                if((int) position.Y == platform.Position.Y && ((int) position.X + size.X == platform.Position.X -1 || (int) position.X == platform.Position.X + platform.Size.X + 1))
                {
                    Collisions.Add("bottom");
                }
                if (((int)Position.X + size.X == platform.Position.X - 1 && (int) position.Y <= platform.Position.Y + platform.Size.Y && (int) position.Y + size.Y > platform.Position.Y - 1 ) || (int) position.X + size.X == GetScreenWidth() -1)
                {
                    Collisions.Add("right");
                }
                if (((int)Position.X == platform.Position.X + platform.Size.X + 1 && (int) position.Y <= platform.Position.Y + platform.Size.Y && (int) position.Y + size.Y > platform.Position.Y - 1) || (int) position.X == 1)
                {
                    Collisions.Add("left");
                }
                if (platform.Position.X - 1 <= (int) position.X + size.X && (int) position.X <= platform.Position.X + platform.Size.X + 1 && platform.Position.Y + platform.Size.Y + 1 == (int) position.Y)
                {
                    Collisions.Add("top");
                }
            }

            return Collisions;
        }

        //--Level-Generation--//



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

        //--Health System--//

        public void HealthSystem(Platform platform)
        {
            if (CheckIfPlayerCollidesWithObject(platform.Platforms).Contains("bottom"))
            {
                int fallDamage = 0;
                fallDamage = CheckingIfVelocityIsOverLimitAndCalculatingFallDamage(500, (float)0.0002);
                ReducingHealth(fallDamage);
                DisplayHealth(fallDamage);
            }
        }

        public void ReducingHealth(int fallDamage)
        {
            if(fallDamage > 100)
            {
                health = 0;
            }
            else
            {
                health = health - fallDamage;
            }
        }

        public void DisplayHealth(int fallDamage)
        {
            if(fallDamage != 0)
            {
                Console.WriteLine(health);
            }
        }

    }
}

/* 

-> If a & d movement works, test if collision works defenetly | ✔
-> New System for the horizontal movement | ✔
-> New system for hanging on platforms if player Y position equals platform Y position | ✔
-> Health system with fall damage |

 */

// «.»