using Godot;
using System;

public class MovingPlatform : KinematicBody2D
{
	[Export] readonly Vector2 pointA;
	[Export] readonly Vector2 pointB;
	[Export] readonly float speed;

	Vector2 velocity;

	public override void _Ready()
	{
		Position = pointA;
		velocity = (pointB - pointA).Normalized() * speed;
	}

	void Move()
	{
		if ((Position - pointA > pointB - pointA) || (Position - pointB < pointA - pointB))
		{
			velocity = -velocity;
		}
	}

	public override void _PhysicsProcess(float delta)
	{
		Move();
		MoveAndSlide(velocity, Vector2.Up);
	}
}
