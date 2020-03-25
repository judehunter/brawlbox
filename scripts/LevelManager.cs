using Godot;
using System;
using System.Collections;
using System.Threading.Tasks;

public class LevelManager : Node2D
{
	public override async void _Ready()
	{
		camera = GetNode<Camera>("Camera2D");
		entityScenes = new PackedScene[] {
			(PackedScene)ResourceLoader.Load("res://scenes/EnemyMelee.tscn"),
			(PackedScene)ResourceLoader.Load("res://scenes/EnemyRanged.tscn")
		};
		await StartNextWave();
	}

	//Level boundaries and spawning
	[Export] readonly float left;
	[Export] readonly float right;
	[Export] readonly float top;
	[Export] readonly float bottom;
	[Export] public Vector2[] spawnPoints;

	readonly Random rng = new Random();
	public Camera camera;

	public void WrapAroundBoundary(Node2D node, float spawnOffset = 0)
	{
		if (node.Position.x > right + spawnOffset) node.Position = new Vector2(right + spawnOffset, node.Position.y);
		if (node.Position.x < left - spawnOffset) node.Position = new Vector2(left - spawnOffset, node.Position.y);
		if (node.Position.y > bottom) node.Position = new Vector2(node.Position.x, top);
		if (node.Position.y < top) node.Position = new Vector2(node.Position.x, bottom);
	}

	public Vector2 GetRandSpawnPoint() => spawnPoints[rng.Next(0, spawnPoints.Length)];

	//Waves
	[Export] readonly float difficulty = 1;
	int curWave = 0;
	Type[] entityTypes = { typeof(EnemyMelee), typeof(EnemyRanged) };
	PackedScene[] entityScenes;

	int GenEnemyCount() => 5 * (int)Math.Ceiling((Math.Log(curWave + 1) / Math.Log(1.5)));

	public async Task StartNextWave()
	{
		curWave++;
		// Display count down here
		// After count down

		for (int i = 0; i < GenEnemyCount(); i++)
		{
			Node enemyNode = entityScenes[rng.Next(0, entityTypes.Length)].Instance();

			AddChild(enemyNode);

			await Task.Delay(TimeSpan.FromSeconds(2));
		}
	}
}
