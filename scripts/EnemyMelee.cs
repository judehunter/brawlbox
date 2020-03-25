using Godot;
using System;

public class EnemyMelee : Enemy
{
	[Export] readonly int reactionTime = 300;

	int dirH = 0;
	int nextDirH = 0;
	float nextDirHTime = 0;

	public override void _PhysicsProcess(float delta)
	{
		base._PhysicsProcess(delta);

		Vector2 nearestDir = Position.DirectionTo(nearest.Position);

		uint OSTime = OS.GetTicksMsec();

		if (nearestDir.x > 0 && nextDirH != 1)
		{
			nextDirH = 1;
			nextDirHTime = OSTime + reactionTime;
		}
		else if (nearestDir.x < 0 && nextDirH != -1)
		{
			nextDirH = -1;
			nextDirHTime = OSTime + reactionTime;
		}

		if (OSTime > nextDirHTime) dirH = nextDirH;

		ObstacleRayCast(dirH);
		GapRayCast(dirH);

		velocity.x = dirH * moveSpeed;

		ApplyGravityForce();

		CheckKnockback("knockbackable", 500, 200);
		ApplyKnockbackForce();

		velocity = MoveAndSlide(velocity, Vector2.Up);

		lvlMgr.WrapAroundBoundary(this);
	}
}
