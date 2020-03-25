using Godot;
using System;

public class Ball : Node2D
{
	public Vector2 dir;
	float speed = 5;

	public override void _PhysicsProcess(float delta)
	{
		Rotation = dir.Angle();
		Position += dir.Normalized() * speed;
	}
}
