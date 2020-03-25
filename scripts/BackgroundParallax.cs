using Godot;

public class BackgroundParallax : Sprite
{

	public override void _Input(InputEvent @event)
	{
		if (@event is InputEventMouseMotion eventMouseMotion)
		{
			Vector2 offset = new Vector2(320 - eventMouseMotion.Position.x, 180 - eventMouseMotion.Position.y);
			Position = new Vector2(320 + offset.x / 20, 180 + offset.y / 20);
		}
	}
}
