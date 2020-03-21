using Godot;
using System;

public class Player : KinematicBody2D
{
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
	}

// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(float delta)
	{
		var move = new Vector2();
		if (Input.IsActionPressed("ui_right")) move.x += 1;
		if (Input.IsActionPressed("ui_left")) move.x -= 1;
		if (Input.IsActionPressed("ui_up")) move.y -= 1;
		if (Input.IsActionPressed("ui_down")) move.y += 1;

		Position += move.Normalized();
	}
}
