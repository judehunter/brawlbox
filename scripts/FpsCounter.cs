using Godot;
using System;

public class FpsCounter : CanvasLayer
{
	Label label;
	public override void _Ready()
	{
		label = new Label();
		AddChild(label);
		label.SetPosition(new Vector2(2, 2));
	}


	public override void _Process(float delta)
	{
		label.Text = "FPS: " + Engine.GetFramesPerSecond();
	}
}
