using Godot;
using System;
using System.Collections;
using System.Threading.Tasks;

public class LevelManager : Node2D
{
	public override void _Ready()
	{
		camera = GetNode<Camera>("Camera2D");
		entityScenes = new PackedScene[] {
			(PackedScene)ResourceLoader.Load("res://scenes/EnemyMelee.tscn"),
			(PackedScene)ResourceLoader.Load("res://scenes/EnemyRanged.tscn")
		};
		countdownLabel = GetTree().Root.GetNode<Label>("Game/HUD/Countdown/Label");
		HUDAnimationPlayer = GetTree().Root.GetNode<AnimationPlayer>("Game/HUD/HUDAnimationPlayer");
		waveDisplay = GetTree().Root.GetNode<VBoxContainer>("Game/HUD/MarginContainer/Elements/CenterContainer/NumberDisplay");
	}
	public override async void _Process(float delta)
	{
		if (curWave == -1 && Input.IsActionJustPressed("spacebar"))
		{
			// Hide label "Press space to start"
			curWave = 0;
			while(true) await NextWave();
		}
	}

	//Level boundaries and spawning
	[Export] readonly float left;
	[Export] readonly float right;
	[Export] readonly float top;
	[Export] readonly float bottom;
	[Export] public Vector2[] spawnPoints;

	readonly Random rng = new Random();
	protected Label countdownLabel;
	protected AnimationPlayer HUDAnimationPlayer;
	protected VBoxContainer waveDisplay;
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
	[Export] readonly int difficulty = 3;
	int curWave = -1;
	int waveTimer = 0;
	PackedScene[] entityScenes;	

	int GenEnemyCount() => difficulty * (int)Math.Ceiling((Math.Log(curWave + 1) / Math.Log(1.5)));

	public async Task NextWave()
	{
		
		waveTimer = 3;
		
		while (waveTimer > 0)
		{
			countdownLabel.Text = waveTimer.ToString();
			HUDAnimationPlayer.Play("DropNumber");
			countdownLabel.Visible = true;
			await Task.Delay(TimeSpan.FromSeconds(1));
			waveTimer--;
		}
		countdownLabel.Visible = false;

		curWave++;
		int enemyCount = GenEnemyCount();
		GD.Print(curWave);
		waveDisplay.Visible = true;
		
		string curWaveText;
		int lastDigit = curWave % 10;
		switch (lastDigit)
		{
			case 1:
				curWaveText = curWave + "st";
				break;
			case 2:
				curWaveText = curWave + "nd";
				break;
			case 3:
				curWaveText = curWave + "rd";
				break;
			default:
				curWaveText = curWave + "th";
				break;
		}

		waveDisplay.GetChild<Label>(0).Text = curWaveText;

		for (int i = 0; i < enemyCount; i++)
		{
			Node enemyNode = entityScenes[rng.Next(0, entityScenes.Length)].Instance();
			AddChild(enemyNode);

			int remaining = enemyCount - 1;

			await Task.Delay(TimeSpan.FromSeconds(2));
		}
	}
}
