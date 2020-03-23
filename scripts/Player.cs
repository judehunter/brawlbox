using Godot;
using System;

public class Player : KinematicBody2D
{
	[Export] readonly float moveSpeed;
	[Export] readonly float jumpStrength;
	[Export] readonly float gravityStrength = 1;
	[Export] readonly float gravityFallStrength = 2;
	[Export] readonly float knockbackDampening = 2;
	[Export] readonly float knockbackThreshold = .2f;

	bool isJump = false;
	Vector2 velocity;

	Vector2 knockback;

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

	public void Knockback(float strength, Vector2 dist)
	{
		knockback += strength * dist.Normalized();
	}

	public void ApplyKnockback()
	{
		velocity += knockback;

		knockback /= knockbackDampening;

		if (knockback.Length() < knockbackThreshold)
		{
			knockback = Vector2.Zero;
		}
	}
		

	public override void _PhysicsProcess(float delta)
	{
		GetInput();
		ApplyGravity();
		ApplyKnockback();
		velocity = MoveAndSlide(velocity, Vector2.Up);
	}
}
