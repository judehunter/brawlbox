using Godot;
using System;

public class EnemyMelee : Enemy
{
	[Export] readonly float moveSpeed = 50;
	[Export] readonly float gravityStrength = 1;
	[Export] readonly int reactionTime = 300;
	[Export] readonly float jumpStrength = 300;
	static float maxSpeed = 700;

	Vector2 velocity;
	int dirH = 0;
	int nextDirH = 0;
	float nextDirHTime = 0;

	void ObstacleRayCast()
	{
		if (dirH == 0 || !IsOnFloor()) return;

		var spaceState = GetWorld2d().DirectSpaceState;

		var result = spaceState.IntersectRay(Position, Position + new Vector2(dirH * 1000000, 0), new Godot.Collections.Array { this });
		if (result.Count <= 0) return;

		if (!(result["collider"] as Node).IsInGroup("obstacles")) return;

		Vector2 pos = result["position"] as Vector2? ?? Vector2.Zero;
		GetNode<Node2D>("../Gizmo").Position = pos;
		if (pos.DistanceTo(Position) < 30)
			velocity.y = -jumpStrength;
	}

	void GapRayCast()
	{
		if (dirH == 0 || !IsOnFloor()) return;

		var from = Position + new Vector2(dirH * 8, 0);

		var spaceState = GetWorld2d().DirectSpaceState;

		var result = spaceState.IntersectRay(from, from + new Vector2(0, 30), new Godot.Collections.Array { this });
		if (result.Count > 0)
		{
			//GD.Print(result["collider"]);
			Vector2 pos = result["position"] as Vector2? ?? Vector2.Zero;
			GetNode<Node2D>("../Gizmo2").Position = pos;
			if (pos.DistanceTo(Position) < 40)
				return;
		}
		velocity.y = -jumpStrength;
	}

	public override void _PhysicsProcess(float delta)
	{
		UpdateNearest();

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

		ObstacleRayCast();
		GapRayCast();

		velocity.x = dirH * moveSpeed;

		velocity.y += 9.8f * gravityStrength;
		if (velocity.y > maxSpeed) velocity.y = maxSpeed;

		velocity = MoveAndSlide(velocity, Vector2.Up);

		lvlMgr.WrapAroundBoundary(this);
	}
}
