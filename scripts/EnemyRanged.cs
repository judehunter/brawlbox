using Godot;
using System;

public class EnemyRanged : Enemy
{
	[Export] readonly int minPlayerDist;
	[Export] readonly int maxPlayerDist;

	readonly Random rng = new Random();
	int playerDist;
	int dirH = 0;

	public override void _Ready()
	{
		base._Ready();
		playerDist = rng.Next(minPlayerDist, maxPlayerDist);
	}

	public override void _PhysicsProcess(float delta)
	{
		base._PhysicsProcess(delta);

		Vector2 nearestDir = Position.DirectionTo(nearest.Position);
		//float nearestDist = Position.DistanceTo(nearest.Position);

		float nearestXDist = Math.Abs(nearest.Position.x - Position.x);
		if (nearestXDist > playerDist || !IsOnFloor())
		{
			if (nearestXDist < 4) dirH = 0;
			else if (nearestDir.x > 0) dirH = 1;
			else if (nearestDir.x < 0) dirH = -1;
		}
		else dirH = 0;

		ObstacleRayCast(dirH);
		GapRayCast(dirH);

		velocity.x = dirH * moveSpeed;

		ApplyGravityForce();

		CheckKnockback("knockbackable", 500, 200);
		ApplyKnockbackForce();

		velocity = MoveAndSlide(velocity, Vector2.Up);

		lvlMgr.WrapAroundBoundary(this, 16);
	}
}
