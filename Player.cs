using Godot;
using System;

public class Player : KinematicBody2D
{
	[Export]
	readonly float moveSpeed;

	public override void _PhysicsProcess(float delta)
	{
		var move = new Vector2
		{
			x = Input.GetActionStrength("move_right") - Input.GetActionStrength("move_left"),
			y = Input.GetActionStrength("move_down") - Input.GetActionStrength("move_up")
		};

		Position += move.Normalized() * delta * moveSpeed;
	}
}
