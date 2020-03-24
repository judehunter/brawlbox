using Godot;
using System;

public class LevelManager : Node2D
{
	[Export] readonly float left;
	[Export] readonly float right;
	[Export] readonly float top;
	[Export] readonly float bottom;

	public void WrapAroundBoundary(Node2D node)
	{
		if (node.Position.x > right) node.Position = new Vector2(left, node.Position.y);
		if (node.Position.x < left) node.Position = new Vector2(right, node.Position.y);
		if (node.Position.y > bottom) node.Position = new Vector2(node.Position.x, top);
		if (node.Position.y < top) node.Position = new Vector2(node.Position.x, bottom);
	}

	public override void _Ready()
	{
		
	}
}
