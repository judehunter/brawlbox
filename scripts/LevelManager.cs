using Godot;
using System;

public class LevelManager : Node2D
{
	[Export] readonly float left;
	[Export] readonly float right;
	[Export] readonly float top;
	[Export] readonly float bottom;

	readonly Random rng = new Random();
	[Export] public Vector2[] spawnPoints;

	public void WrapAroundBoundary(Node2D node, float spawnOffset = 0)
	{
		if (node.Position.x > right + spawnOffset) node.Position = new Vector2(right + spawnOffset, node.Position.y);
		if (node.Position.x < left - spawnOffset) node.Position = new Vector2(left - spawnOffset, node.Position.y);
		if (node.Position.y > bottom) node.Position = new Vector2(node.Position.x, top);
		if (node.Position.y < top) node.Position = new Vector2(node.Position.x, bottom);
	}

	public Vector2 GetRandSpawnPoint() => spawnPoints[rng.Next(0, spawnPoints.Length)];

	public override void _Ready()
	{
		
	}
}
