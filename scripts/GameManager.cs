using Godot;
using System;
using System.Threading.Tasks;

public class GameManager : Node2D
{
	AudioStreamPlayer music;
	Tween tween;
	public override void _Ready()
	{
		music = GetNode<AudioStreamPlayer>("MusicPlayer");
		music.Stream = ResourceLoader.Load<AudioStream>("res://assets/bg.ogg");
		music.Play();
		tween = GetNode<Tween>("Tween");
	}

	public async void StopMusicWithReverb()
	{
		music.Bus = "ReverbBus";
		await Task.Delay(TimeSpan.FromMilliseconds(30));
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
		RemoveChild(menu);
		GetNode<ColorRect>("CRTShader/BlackOverlay").Visible = true;
		await Task.Delay(TimeSpan.FromSeconds(1));
		GetNode<CanvasLayer>("UILayer").AddChild(HUD.Instance());
		AddChild(Level.Instance());
		GetNode<ColorRect>("CRTShader/BlackOverlay").Visible = false;
		await Task.Delay(TimeSpan.FromSeconds(1));
		GetNode<LevelManager>("Level").NextWave();
	}
}
