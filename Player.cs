using Godot;
using System;

public class Player : KinematicBody2D
{
	[Export] readonly float moveSpeed;
	[Export] readonly float jumpStrength;
	[Export] readonly float gravityStrength = 1;
	[Export] readonly float gravityFallStrength = 2;

	bool isJump = false;
	Vector2 velocity;

	void GetInput()
	{
		velocity.x = (Input.GetActionStrength("move_right") - Input.GetActionStrength("move_left")) * moveSpeed;
		if (velocity.y == 0)
		{
			if (Input.IsActionPressed("move_up"))
			{
				isJump = true;
				velocity.y = -1 * jumpStrength;
			}
		}
		if (Input.IsActionJustReleased("move_up") || velocity.y > 0)
		{
			isJump = false;
		}
	}

	void ApplyGravity()
	{
		velocity.y += 9.8f * (isJump ? gravityStrength : gravityFallStrength);
	}
		

	public override void _PhysicsProcess(float delta)
	{
		GetInput();
		ApplyGravity();
		velocity = MoveAndSlide(velocity, Vector2.Up);
	}
}
