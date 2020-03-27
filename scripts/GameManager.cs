using Godot;
using System;
using System.Threading.Tasks;

public class GameManager : Node2D
{
	AudioStreamPlayer music;
	Tween tween;

	public enum GAME_STATE {MENU, INGAME_ALIVE, DEATH_SCREEN};
	public GAME_STATE state;

	public override void _Ready()
	{
		music = GetNode<AudioStreamPlayer>("MusicPlayer");
		music.Stream = ResourceLoader.Load<AudioStream>("res://assets/bg.ogg");
		music.Play();
		tween = GetNode<Tween>("Tween");
		state = GAME_STATE.MENU;
	}

	public void PlayerDied()
	{
		state = GAME_STATE.DEATH_SCREEN;
		music.Bus = "Highpass";
		Control HUD = GetNode<Control>("UILayer/HUD");
		LevelManager lv = GetNode<LevelManager>("Level");
		HUD.GetNode<MarginContainer>("MarginContainer").Visible = false;
		HUD.GetNode<Control>("DeathScreen").Visible = true;
		string statsText = "Waves survived: " + lv.curWave + "\nCrystals Collected: " + lv.gems + "\nEnemies killed: " + lv.enemiesKilled;
		HUD.GetNode<Label>("DeathScreen/Center/VBoxContainer/Stats").Text = statsText;
		GetNode<LevelManager>("Level").KillAllEnemies();
	}

	public void RestartGame()
	{
		PackedScene Level = ResourceLoader.Load<PackedScene>("res://scenes/levels/TestMap.tscn");
		Enemy.players.Clear();
		RemoveChild(GetNode<Node2D>("Level"));
		AddChild(Level.Instance());
		music.Bus = "Master";
		Control HUD = GetNode<Control>("UILayer/HUD");
		HUD.GetNode<MarginContainer>("MarginContainer").Visible = true;
		HUD.GetNode<Control>("DeathScreen").Visible = false;
		state = GAME_STATE.INGAME_ALIVE;
		GetNode<LevelManager>("Level").FirstWave();
	}

	public override void _Process(float delta)
	{
		if(Input.IsActionJustPressed("restart") && state == GAME_STATE.DEATH_SCREEN)
		{
			RestartGame();
		}

		if(Input.IsActionJustPressed("state_exit"))
		{
			if (state == GAME_STATE.INGAME_ALIVE || state == GAME_STATE.DEATH_SCREEN) GoToMenu();
			if (state == GAME_STATE.MENU) GetTree().Quit();
		}
	}

	public async void GoToMenu()
	{
		music.Disconnect("finished", this, nameof(musicWarmupFinished));
		music.Stop();
		PackedScene menu = ResourceLoader.Load<PackedScene>("res://scenes/MainMenu.tscn");
		Node level = GetNode<Node2D>("Level");
		Node HUD = GetNode<Control>("UILayer/HUD");
		RemoveChild(level);
		GetNode("UILayer").RemoveChild(HUD);
		AddChild(menu.Instance());
		music.Stream = ResourceLoader.Load<AudioStream>("res://assets/bg.ogg");
		music.Play();
		await Task.Delay(TimeSpan.FromSeconds(1));
		state = GAME_STATE.MENU;
	}

	public async void StopMusicWithReverb()
	{
		music.Bus = "ReverbBus";
;		await Task.Delay(TimeSpan.FromMilliseconds(30));
		music.Stop();
		await Task.Delay(TimeSpan.FromSeconds(3));
		music.Bus = "Master";
	}

	public void StartLevelMusic()
	{
		if (music.Playing) return;
		music.Stream = ResourceLoader.Load<AudioStream>("res://assets/rivals-warmup.ogg");
		music.Play();
		music.Connect("finished", this, nameof(musicWarmupFinished));
	}

	public void musicWarmupFinished()
	{
		music.Disconnect("finished", this, nameof(musicWarmupFinished));
		music.Stream = ResourceLoader.Load<AudioStream>("res://assets/rivals-loop.ogg");
		music.Play();
	}

	public async void StartSoloGame()
	{
		StopMusicWithReverb();
		PackedScene Level = ResourceLoader.Load<PackedScene>("res://scenes/levels/TestMap.tscn");
		PackedScene HUD = ResourceLoader.Load<PackedScene>("res://scenes/GameHUD.tscn");
		Node menu = GetNode<Control>("MainMenu");
		GetNode<ColorRect>("CRTShader/BlackOverlay").Visible = true;
		RemoveChild(menu);
		GetNode<CanvasLayer>("UILayer").AddChild(HUD.Instance());
		AddChild(Level.Instance());
		await Task.Delay(TimeSpan.FromSeconds(1));
		GetNode<ColorRect>("CRTShader/BlackOverlay").Visible = false;
		state = GAME_STATE.INGAME_ALIVE;
		await Task.Delay(TimeSpan.FromSeconds(1));
		GetNode<LevelManager>("Level").FirstWave();
	}
}
