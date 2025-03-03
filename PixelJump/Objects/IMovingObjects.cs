using System;
using System.Numerics;

namespace PixelJump.Objects
{
	public interface IMovingObjects
	{
		void MovementCalculation(List<Platform> platforms, MovingObjects movingObject, MovingObjects player);

		void UpdateMovingObjectPosition(List<Platform> platforms, Vector2 distance);

		Vector2 CalculatingDistance(Vector2 initialVelocity, Vector2 acceleration, float time);

		Vector2 CalculatingVelocity(Vector2 initialVelocity, Vector2 acceleration, List<Platform> platforms, MovingObjects movingObject, MovingObjects player);

		Vector2 SettingInitialVelocity(Vector2 velocity, List<Platform> platforms, MovingObjects movingObject);

		float CalculatingTimeTillMaximumJumpHeight(Vector2 initialVelocity, Vector2 maximumAcceleration);

		Vector2 SettingAcceleration(List<Platform> platforms, MovingObjects movingObject);

		List<string> CheckIfMovingObjectCollidesWithObject(List<Platform> platforms);

		void DrawMovingObject(MovingObjects movingObject);

		void HealthSystem(List<Platform> platforms);

		void ReducingHealth(int damage);

		void DisplayHealth(int damage);

		void InitilizePlatforms(List<Platform> platforms, List<MovingObjects> enemies);

		void Attack(MovingObjects attacker, List<MovingObjects> attackedObjects, List<MovingObjects> enemies);
    }
}

