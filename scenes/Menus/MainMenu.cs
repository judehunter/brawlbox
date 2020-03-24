using Godot;
using System;

public class MainMenu : MarginContainer
{
	AudioStreamPlayer select;
	bool multiplayerVisible = false;

	public override void _Ready()
	{
		GetNode<HBoxContainer>("VBoxContainer/UI/MarginContainer/Right/Multiplayer").Visible = multiplayerVisible;
		select = GetNode<AudioStreamPlayer>("SelectSounds");
	}

	private void _on_Solo_pressed()
	{
		GD.Print("ee");
		select.Play();
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
