using Godot;
using System;

public class Ball : KinematicBody2D
{
	[Export] float speed;
	public Vector2 dir;
	public bool isPlayer;
	LevelManager lvlMgr;

	public override void _Ready()
	{
		Rotation = dir.Angle();
		lvlMgr = GetTree().Root.GetNode<LevelManager>("Game/Level");
	}
	public override void _PhysicsProcess(float delta)
	{
		Rotation = dir.Angle();
		var collision = MoveAndCollide(dir.Normalized() * speed);

		if (collision != null)
		{
			GD.Print(collision.Collider);
			if (collision.Collider is Entity e) e.Harm(500, e.Position - Position);
			if (collision.Collider is Player) lvlMgr.camera.Shake(5);

			var parts = ((PackedScene)ResourceLoader.Load("res://scenes/ParticlesSmall.tscn")).Instance() as Particles2D;
			parts.GlobalPosition = collision.Position;
			parts.Emitting = true;

			GetTree().Root.AddChild(parts);
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
