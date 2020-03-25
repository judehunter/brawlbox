using Godot;
using System;

public class MainMenu : MarginContainer
{
	AudioStreamPlayer select;
	GameManager gm;
	bool multiplayerVisible = false;

	public override void _Ready()
	{
		GetNode<HBoxContainer>("VBoxContainer/UI/MarginContainer/Right/Multiplayer").Visible = multiplayerVisible;
		select = GetNode<AudioStreamPlayer>("SelectSounds");
		gm = GetTree().Root.GetNode<Node2D>("Game") as GameManager;
	}

	private void _on_Solo_pressed()
	{
		select.Play();
		gm.StartSoloGame();
	}

	private void _on_Duo_pressed()
	{
		multiplayerVisible = !multiplayerVisible;
		GetNode<HBoxContainer>("VBoxContainer/UI/MarginContainer/Right/Multiplayer").Visible = multiplayerVisible;
		select.Play();
	}

	private void _on_JoinServer_pressed()
	{
		GetNode<CenterContainer>("AddressBox").Visible = true;
		select.Play();
	}

	private void _on_AddressBox_gui_input(InputEvent @event)
	{
		if (@event.IsPressed())
		{
			GetNode<CenterContainer>("AddressBox").Visible = false;
		}
	}

	private void _on_Credits_pressed()
	{

		PopupDialog p = GetNode<PopupDialog>("Credits/PopupDialog");
		if(!p.Visible) p.PopupCentered();
		select.Play();
	}

}
