using Godot;
using System;

public class ProverbDisplay : Label
{
	public override void _Ready()
	{
		
	}

	private void changeText(string text = "WHAT DOESN'T KILL YOU MAKES YOU STRONGER.")
	{

		if(GetNode<HBoxContainer>("../../UI/MarginContainer/Right/Multiplayer").Visible)
		{
			Text = "Many hands make light work.";
		} else
		{
			Text = text;
		}
	}

	private void _on_Solo_mouse_entered()
	{
		changeText("If you want something done right, you have to do it yourself.");
	}


	private void _on_Solo_mouse_exited()
	{
		changeText();
	}


	private void _on_Duo_mouse_entered()
	{
		changeText("Many hands make light work.");
	}


	private void _on_Duo_mouse_exited()
	{
		changeText();
	}

}


