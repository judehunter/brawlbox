using Godot;
using System;
using System.Collections.Generic;
using System.Linq;

public class Enemy : Entity
{
	public static readonly List<Player> players = new List<Player>();

	protected Player nearest;

	protected void UpdateNearest()
	{
		float minDist = float.MaxValue;
		Player minDistPlayer = null;
		foreach (var player in players)
		{
			float dist = player.Position.DistanceTo(Position);
			if (dist < minDist)
			{
				minDist = dist;
				minDistPlayer = player;
			}
		}
		nearest = minDistPlayer;
	}

	protected void ObstacleRayCast(int dirH)
	{
		if (dirH == 0 || !IsOnFloor()) return;

		var spaceState = GetWorld2d().DirectSpaceState;

		var result = spaceState.IntersectRay(Position, Position + new Vector2(dirH * 1000000, 0), new Godot.Collections.Array { this });
		if (result.Count <= 0) return;

		if (!(result["collider"] as Node).IsInGroup("obstacle")) return;

		Vector2 pos = result["position"] as Vector2? ?? Vector2.Zero;
		GetNode<Node2D>("../Gizmo").Position = pos;
		if (pos.DistanceTo(Position) < 30)
			velocity.y = -jumpStrength;
	}

	protected void GapRayCast(int dirH)
	{
		if (dirH == 0 || !IsOnFloor()) return;

		var from = Position + new Vector2(dirH * 8, 0);

		var spaceState = GetWorld2d().DirectSpaceState;

		var result = spaceState.IntersectRay(from, from + new Vector2(0, 30), new Godot.Collections.Array { this });
		if (result.Count > 0)
		{
			Vector2 pos = result["position"] as Vector2? ?? Vector2.Zero;
			GetNode<Node2D>("../Gizmo2").Position = pos;
			if (pos.DistanceTo(Position) < 40)
				return;
		}
		velocity.y = -jumpStrength;
	}

	public override void Die(bool wasByGem)
	{
		var parts = ((PackedScene)ResourceLoader.Load("res://scenes/ParticlesLarge.tscn")).Instance() as Particles2D;
		parts.GlobalPosition = GlobalPosition;
		parts.Emitting = true;

		GetTree().Root.GetNode("Game").AddChild(parts);

		GD.Print(wasByGem);
		lvlMgr.enemiesKilled++;
		if (!wasByGem) lvlMgr.enemiesKilledWithoutGem++;
		if (lvlMgr.enemiesKilledWithoutGem % 15 == 0) {
			lvlMgr.gems++;
			lvlMgr.gemDisplay.Text = lvlMgr.gems.ToString();
		};
		QueueFree();
	}

	public override void _Ready()
	{
		base._Ready();
		lvlMgr = GetTree().Root.GetNode<LevelManager>("Game/Level");
		Position = lvlMgr.GetRandSpawnPoint();
	}

	public override void _PhysicsProcess(float delta)
	{
		base._PhysicsProcess(delta);
		UpdateNearest();
	}

	static public void AddPlayer(Player player)
	{
		players.Add(player);
	}
}
