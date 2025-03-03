using System;
using System.Drawing;
using System.Numerics;
using Raylib_CsLo;
using static Raylib_CsLo.Raylib;

namespace PixelJump.Objects
{
    public abstract class MovingObjects : IMovingObjects
    {
        //---Variables---//

        //--Movement--//
        private Vector2 position = new Vector2();
        private Vector2 size = new Vector2();
        private Vector2 distance = new Vector2();
        private Vector2 velocity = new Vector2();
        private Vector2 initialVelocity = new Vector2();
        private Vector2 maximumVelocity = new Vector2();
        private Vector2 acceleration = new Vector2();
        private Vector2 maximumAcceleration = new Vector2();

        private bool spaceGotPressed;

        //--Health-System--//
        private int health = 100;

        //---Methods---//

        /// <summary>
        /// Initilizes and sets parameters for a new MovingObject related Object
        /// </summary>
        /// <param name="position"></param>
        /// <param name="size"></param>
        /// <param name="distance"></param>
        /// <param name="velocity"></param>
        /// <param name="initialVelocity"></param>
        /// <param name="maximumVelocity"></param>
        /// <param name="acceleration"></param>
        /// <param name="spaceGotPressed"></param>
        protected MovingObjects(Vector2 position, Vector2 size, Vector2 distance, Vector2 velocity, Vector2 initialVelocity, Vector2 maximumVelocity, Vector2 acceleration, Vector2 maximumAcceleration, bool spaceGotPressed)
        {
            this.position = position;
            this.size = size;
            this.distance = distance;
            this.velocity = velocity;
            this.initialVelocity = initialVelocity;
            this.maximumVelocity = maximumVelocity;
            this.acceleration = acceleration;
            this.maximumAcceleration = maximumAcceleration;
            this.spaceGotPressed = spaceGotPressed;
        }

        //--Movement--//

        public virtual void InitilizePlatforms(List<Platform> platforms, List<MovingObjects> enemies)
        {

        }

        /// <summary>
        /// The initial method, allowing the object which inherits from the abstract class <c> MovingObjects </c> to move with set parabeters. Depending on class, method has added features not included in base method.
        /// </summary>
        /// <param name="platform"></param>
        /// <param name="movingObject"></param>
        /// <param name="player"></param>
        public virtual void MovementCalculation(List<Platform> platforms, MovingObjects movingObject, MovingObjects enemy)
        {
            this.acceleration = SettingAcceleration(platforms, movingObject);
            this.velocity = CalculatingVelocity(initialVelocity, acceleration, platforms, movingObject, enemy);
            this.distance = CalculatingDistance(initialVelocity, acceleration, GetFrameTime());
            this.initialVelocity = SettingInitialVelocity(velocity, platforms, movingObject);


            UpdateMovingObjectPosition(platforms, distance);
            UpdateSpaceVariable(movingObject);
        }

        /// <summary>
        /// Setting the <c>acceleration</c> of the movingObject to a certain value depending on if the player is standing on the platform or not.
        /// </summary>
        /// <param name="platform"></param>
        /// <returns>Vector2 acceleration</returns>
        public virtual Vector2 SettingAcceleration(List<Platform> platforms, MovingObjects movingObject)
        {
            Vector2 acceleration;

            //-Vertical-//

            if (CheckIfMovingObjectCollidesWithObject(platforms).Contains("bottom") && !movingObject.SpaceGotPressed)
            {
                acceleration.Y = 0;
            }
            else
            {
                acceleration.Y = maximumAcceleration.Y;
            }

            //-Horizontal-//

            acceleration.X = 0;

            //-Return-//

            return acceleration;
        }

        /// <summary>
        /// Calculates the <c>velocity</c> depending on the <c>acceleration</c> of the object.
        /// </summary>
        /// <param name="initialVelocity"></param>
        /// <param name="acceleration"></param>
        /// <param name="platforms"></param>
        /// <param name="movingObject"></param>
        /// <param name="player"></param>
        /// <returns>Vector2 velocity</returns>
        public virtual Vector2 CalculatingVelocity(Vector2 initialVelocity, Vector2 acceleration, List<Platform> platforms, MovingObjects movingObject, MovingObjects enemy)
        {
            Vector2 velocity;

            //-Vertical-//

            if (acceleration.Y != 0)
            {
                velocity.Y = initialVelocity.Y + acceleration.Y * GetFrameTime();
            }
            else
            {
                velocity.Y = 0;
            }

            //-Horizontal-//

            velocity.X = initialVelocity.X;

            return velocity;
        }

        /// <summary>
        /// Calculates The <c>distance</c> the player should travel in the time, the last frame was loading. Depending on <c>initial velocity</c> and <c>acceleration</c>.
        /// </summary>
        /// <param name="initialVelocity"></param>
        /// <param name="acceleration"></param>
        /// <param name="time"></param>
        /// <returns>Vector2 distance</returns>
        public virtual Vector2 CalculatingDistance(Vector2 initialVelocity, Vector2 acceleration, float time)
        {
            Vector2 distance;

            distance.Y = initialVelocity.Y * time + (float)0.5 * acceleration.Y * (float)Math.Pow(time, 2);
            distance.X = initialVelocity.X * time;

            return distance;
        }

        /// <summary>
        /// Setting the vertical <c>initial velocity</c> of the object depending if the object has hit a platform with its upper edge and the horizontal by using the value of <c>velocity</c>. 
        /// </summary>
        /// <param name="velocity"></param>
        /// <param name="platforms"></param>
        /// <param name="movingObject"></param>
        /// <returns>Vector2 initialVelocity</returns>
        public virtual Vector2 SettingInitialVelocity(Vector2 velocity, List<Platform> platforms, MovingObjects movingObject)
        {
            Vector2 initialVelocity;

            if (CheckIfMovingObjectCollidesWithObject(platforms).Contains("top") && velocity.Y >= 0)
            {
                initialVelocity.Y = 0;
            }
            else
            {
                initialVelocity.Y = velocity.Y;
            }

            initialVelocity.X = velocity.X;

            return initialVelocity;
        }

        /// <summary>
        /// Uses the distance calculated before to change the <c>position</c> variable of the object by adding the distance onto it.
        /// </summary>
        /// <param name="platforms"></param>
        /// <param name="distance"></param>
        public virtual void UpdateMovingObjectPosition(List<Platform> platforms, Vector2 distance)
        {
            position.Y = position.Y - distance.Y;
            position.X = position.X - distance.X;
        }

        public void UpdateSpaceVariable(MovingObjects movingObject)
        {
            if (movingObject.Velocity.Y < 0)
            {
                movingObject.SpaceGotPressed = false;
            }
        }


        /// <summary>
        /// Calculates the <c>time</c> needed that the <c>moving Object</c> reaches maximum height given on <C
        /// </summary>
        /// <param name="initialVelocity"></param>
        /// <returns>float time</returns>
        public virtual float CalculatingTimeTillMaximumJumpHeight(Vector2 initialVelocity, Vector2 maximumAcceleration)
        {
            return (float)(-initialVelocity.Y / (maximumAcceleration.Y));
        }

        //--Collisions--//

        public virtual List<string> CheckIfMovingObjectCollidesWithObject(List<Platform> platforms)
        {
            List<string> Collisions = new List<string>();

            foreach (Platform platform in platforms)
            {
                if ((int)platform.Position.X <= (int)(position.X + size.X)
                    && (int)position.X <= (int)platform.Position.X + platform.Size.X
                    && (int)platform.Position.Y <= (int)(position.Y + size.Y) + 1
                    && (int)platform.Position.Y >= (int) position.Y + 1)
                {
                    position.Y = platform.Position.Y - size.Y - 1;
                    Collisions.Add("bottom");
                }
                if ((int)position.Y == (int)platform.Position.Y
                    && ((int)(position.X + size.X) == (int)platform.Position.X - 1
                    || (int)position.X == (int)platform.Position.X + (int)platform.Size.X + 1))
                {
                    Collisions.Add("bottom");
                }
                if (((int)(position.X + size.X) == (int)platform.Position.X - 1
                    && (int)position.Y <= platform.Position.Y + platform.Size.Y
                    && (int)position.Y + size.Y > platform.Position.Y - 1)
                    || (int)position.X + size.X == GetScreenWidth() - 1)
                {
                    Collisions.Add("right");
                }
                if (((int)position.X == (int)platform.Position.X + (int)platform.Size.X + 1
                    && (int)position.Y <= platform.Position.Y + platform.Size.Y
                    && (int)position.Y + size.Y > platform.Position.Y - 1)
                    || (int)position.X == 1)
                {
                    Collisions.Add("left");
                }
                if (platform.Position.X - 1 <= (int)position.X + size.X
                    && (int)position.X <= platform.Position.X + platform.Size.X + 1
                    && (int)(platform.Position.Y + platform.Size.Y) + 1 == (int)position.Y)
                {
                    Collisions.Add("top");
                }
            }

            return Collisions;
        }

        //--Health-Related--//

        public virtual void HealthSystem(List<Platform> platforms)
        {
            if (CheckIfMovingObjectCollidesWithObject(platforms).Contains("bottom"))
            {
                int fallDamage = 0; //Change Fall Damage to sum of Fall Damage and Hit Damage of Enemy/Player
                ReducingHealth(fallDamage);
                DisplayHealth(fallDamage);
            }
        }

        public virtual void ReducingHealth(int damage)
        {
            if (damage > 100)
            {
                health = 0;
            }
            else
            {
                health = health - damage;
            }
        }

        public virtual void DisplayHealth(int damage)
        {
            if (damage != 0)
            {
                Console.WriteLine(health);
            }
        }

        public virtual void removeDeadEnemies(List<MovingObjects> enemies) { }

        //--Interactions-between-different-Moving-Objects--//

        public virtual void Attack(MovingObjects attacker, List<MovingObjects> attackedObjects, List<MovingObjects> enemies)
        {
            Vector2 knockbackVelocity = new Vector2(-100, 100);

            foreach (MovingObjects attackedObject in attackedObjects)
            {
                if (attacker.position.X > attackedObject.position.X + attackedObject.size.X)
                    knockbackVelocity.X = 100;
                else if (attacker.position.X < attackedObject.position.X + attackedObject.size.X
                     && attacker.position.X + attacker.size.X > attackedObject.position.X)
                    knockbackVelocity.X = 0;

                attackedObject.health -= 20;
                attackedObject.initialVelocity = knockbackVelocity;
                attackedObject.spaceGotPressed = true;
            }
        }

        //--Drawing-Enemy--//

        public virtual void DrawMovingObject(MovingObjects movingObject)
        {
            DrawRectangle((int)position.X, (int)position.Y, (int)size.X, (int)size.Y, RED);
        }

        public virtual void DrawHealth(MovingObjects player) { }



        // public abstract bool LetsEverythingFreeze(MovingObjects movingObject, List<Vector2> initialSettings, bool escPressed);

        

        public Vector2 Position { get => position; set => position = value; }
        public Vector2 Distance { get => distance; set => distance = value; }
        public Vector2 Velocity { get => velocity; set => velocity = value; }
        public Vector2 InitialVelocity { get => initialVelocity; set => initialVelocity = value; }
        public Vector2 MaximumVelocity { get => maximumVelocity; set => maximumVelocity = value; }
        public Vector2 Acceleration { get => acceleration; set => acceleration = value; }
        public Vector2 Size { get => size; set => size = value; }
        public bool SpaceGotPressed { get => spaceGotPressed; set => spaceGotPressed = value; }
        public int Health { get => health; set => health = value; }
        public Vector2 MaximumAcceleration { get => maximumAcceleration; set => maximumAcceleration = value; }
    }
}

