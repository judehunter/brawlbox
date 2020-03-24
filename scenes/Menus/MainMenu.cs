using Godot;
using System;

public class MainMenu : MarginContainer
{
	AudioStreamPlayer select;

	public override void _Ready()
	{
		GetNode<HBoxContainer>("VBoxContainer/UI/Right/Multiplayer").Visible = false;
		select = GetNode<AudioStreamPlayer>("SelectSounds");
	}

	private void _on_Solo_pressed()
	{
		GD.Print("ee");
		select.Play();
	}

	private void _on_Duo_pressed()
	{
		GetNode<HBoxContainer>("VBoxContainer/UI/Right/Multiplayer").Visible = true;
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

}



