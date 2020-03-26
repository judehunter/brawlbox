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
		countdownLabel = GetTree().Root.GetNode<Label>("Game/UILayer/HUD/Countdown/Label");
		HUDAnimationPlayer = GetTree().Root.GetNode<AnimationPlayer>("Game/UILayer/HUD/HUDAnimationPlayer");
		waveDisplay = GetTree().Root.GetNode<VBoxContainer>("Game/UILayer/HUD/MarginContainer/Elements/CenterContainer/NumberDisplay");
		soundEffectPlayer = GetNode<AudioStreamPlayer>("MapSoundEffectPlayer");
		gm = GetTree().Root.GetNode<Node2D>("Game") as GameManager;
		player = GetNode<KinematicBody2D>("Player") as Player;
	}
	public override async void _Process(float delta)
	{
		//if (curWave == -1 && Input.IsActionJustPressed("spacebar"))
		//{
		//	// Hide label "Press space to start"
		//	curWave = 0;
		//	while(true) await NextWave();
		//}
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
	protected AudioStreamPlayer soundEffectPlayer;
	protected GameManager gm;
	protected Player player;
	public bool WaveInProgress = false;
	public Camera camera;
	public int enemiesKilled = 0;


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
	public int curWave = 0;
	int waveTimer = 0;
	PackedScene[] entityScenes;	

	int GenEnemyCount() => difficulty * (int)Math.Ceiling((Math.Log(curWave + 1) / Math.Log(1.5)));

	public void FirstWave()
	{
		NextWave();
	}

	public void KillAllEnemies()
	{
		foreach (var item in GetTree().GetNodesInGroup("enemy"))
		{
			(item as Enemy).QueueFree();
		}
	}

	public async Task NextWave()
	{
		if (!player.alive) return;

		soundEffectPlayer.Stream = ResourceLoader.Load<AudioStream>("res://assets/noise.wav");
		waveTimer = 3;
		
		while (waveTimer > 0)
		{
			countdownLabel.Text = waveTimer.ToString();
			HUDAnimationPlayer.Play("DropNumber");
			soundEffectPlayer.Play();
			countdownLabel.Visible = true;
			await Task.Delay(TimeSpan.FromSeconds(1));
			waveTimer--;
		}
		countdownLabel.Visible = false;

		curWave++;
		int enemyCount = GenEnemyCount();
		GD.Print(curWave);
		gm.StartLevelMusic();
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
		switch(curWave)
		{
			case 11:
				curWaveText = "11th";
				break;
			case 12:
				curWaveText = "12th";
				break;
			case 13:
				curWaveText = "13th";
				break;
		}

		waveDisplay.GetChild<Label>(0).Text = curWaveText;

		for (int i = 0; i < enemyCount; i++)
		{
			if(player.alive)
			{
				Node enemyNode = entityScenes[rng.Next(0, entityScenes.Length)].Instance();
				AddChild(enemyNode);

				int remaining = enemyCount - 1;

				await Task.Delay(TimeSpan.FromSeconds(2));
			}
		}
		
		while (GetTree().GetNodesInGroup("enemy").Count != 0)
		{
				await Task.Delay(TimeSpan.FromSeconds(1));
		}
		_ = NextWave();
	}
}
