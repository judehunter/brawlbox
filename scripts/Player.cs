using Godot;
using System;

public class Player : Entity
{
	[Export] readonly float moveSpeed;
	[Export] readonly float jumpStrength;
	[Export] readonly float gravityFallStrength = 2;

	bool isJump = false;

	void GetInput()
	{
		velocity.x = (Input.GetActionStrength("move_right") - Input.GetActionStrength("move_left")) * moveSpeed;
		if (velocity.y == 0)
		{
			if (IsOnFloor() && Input.IsActionPressed("move_up"))
			{
				isJump = true;
				velocity.y = -1 * jumpStrength;
			}

			else isJump = false;
		}
		if (Input.IsActionJustReleased("move_up") || velocity.y > 0)
		{
			isJump = false;
		}
	}

	protected override void ApplyGravityForce()
	{
		velocity.y += 9.8f * (isJump ? gravityStrength : gravityFallStrength);
		if (velocity.y > maxSpeed) velocity.y = maxSpeed;
	}

	public override void _Ready()
	{
		base._Ready();
		Enemy.AddPlayer(this);
	}

	public override void _PhysicsProcess(float delta)
	{
		GetInput();
		ApplyGravityForce();
		CheckKnockback("knockback", 500, 200);
		ApplyKnockbackForce();
		velocity = MoveAndSlide(velocity, Vector2.Up);
		
		lvlMgr.WrapAroundBoundary(this);
	}
}
