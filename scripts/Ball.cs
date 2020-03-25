using Godot;
using System;

public class Ball : KinematicBody2D
{
	public Vector2 dir;
	public bool isPlayer;
	[Export] float speed;

	// tilemap | enemies | p1 | p2
	// 1 + 9 + 16 = 26
	// 1 + 4 = 5
	public override void _Ready()
	{
		//GD.Print(isPlayer);
		//if (!isPlayer) SetCollisionMask(26);
		//else SetCollisionMask(5);
		Rotation = dir.Angle();
	}
	public override void _PhysicsProcess(float delta)
	{
		Rotation = dir.Angle();
		var collision = MoveAndCollide(dir.Normalized() * speed);

		if (collision != null)
		{
			GD.Print(collision.Collider);
			if (collision.Collider is Entity e) e.Harm(500, e.Position - Position);
			QueueFree();
		}

		/*for (int i = 0; i < GetSlideCount(); i++)
		{
			var coll = GetSlideCollision(i);
			if (!(coll.Collider is Node2D)) continue;
			var other = coll.Collider as Node2D;
			if ((other is Player && !isPlayer) || (other is Enemy && isPlayer))
			{
				(other as Entity).Harm(500, -other.Position - Position);
			}
		}

		if (GetSlideCount() > 0) ;*/
	}
}
