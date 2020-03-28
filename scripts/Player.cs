using Godot;
using System;

public class Player : Entity
{
	[Export] readonly float gravityFallStrength = 2;
	[Export] readonly int attackCD;

	bool isJump = false;
	int nextAttackTime = 0;
	Node2D attackPoint;
	GameManager gm;
	Label healthDisplay;
	public bool alive = true;
	

	public override void Die(bool _)
	{
		gm.PlayerDied();
		alive = false;
		GD.Print("Player died!");
		/*Hide();*/
	}

	public override void Harm(float strength, Vector2 dist)
	{
		if (!alive) return;
		HP--;
		if (HP <= 0) Die(false);
		knockback += strength * dist.Normalized() * new Vector2(1, .03f);
		healthDisplay.Text = (HP*10).ToString();
		//if (knockback.Length() > maxKnockback) knockback = knockback.Normalized() * maxKnockback;
	}

	void GetInput()
	{
		velocity.x = (Input.GetActionStrength("move_right") - Input.GetActionStrength("move_left")) * moveSpeed;
		if (velocity.y == 0)
		{
			if (IsOnFloor() && Input.IsActionPressed("move_up"))
			{
				isJump = true;
				velocity.y = -1 * jumpStrength;
			}

			else isJump = false;
		}
		if (Input.IsActionJustReleased("move_up") || velocity.y > 0)
		{
			isJump = false;
		}
	}

	void Attack()
	{
		var OSTime = OS.GetTicksMsec();
		if (OSTime < nextAttackTime) return;
		if (!Input.IsActionJustPressed("attack")) return;
		nextAttackTime = (int)OSTime + attackCD;
		Vector2 dir = attackPoint.GlobalPosition.DirectionTo(GetViewport().GetMousePosition());
		var ball = ((PackedScene)ResourceLoader.Load("res://scenes/PoisonBall.tscn")).Instance() as Ball;
		ball.GlobalPosition = attackPoint.GlobalPosition;
		ball.dir = dir;
		ball.isPlayer = true;
		GetTree().Root.AddChild(ball);
	}

	protected override void ApplyGravityForce()
	{
		velocity.y += 9.8f * (isJump ? gravityStrength : gravityFallStrength);
		if (velocity.y > maxSpeed) velocity.y = maxSpeed;
	}

	void BadassCrystalPowerMove()
	{
		if (!Input.IsActionJustPressed("crystal")) return;
		if (lvlMgr.gems <= 0) return;
		var g = lvlMgr.gems;
		lvlMgr.gems--;
		lvlMgr.gemDisplay.Text = lvlMgr.gems.ToString();
		foreach (var item in GetTree().GetNodesInGroup("enemy"))
		{
			(item as Enemy).Die(true);
			lvlMgr.camera.Shake(25);
		}
		lvlMgr.gems = g - 1;
		lvlMgr.gemDisplay.Text = lvlMgr.gems.ToString();
	}

	public override void _Ready()
	{
		base._Ready();
		attackPoint = GetNode<Node2D>("AttackPoint");
		Enemy.AddPlayer(this);
		gm = GetTree().Root.GetNode<Node2D>("Game") as GameManager;
		healthDisplay = GetTree().Root.GetNode<Label>("Game/UILayer/HUD/MarginContainer/Elements/HP/Label");
		healthDisplay.Text = (HP*10).ToString();
	}

	public override void _Process(float delta)
	{
		base._Process(delta);
		BadassCrystalPowerMove();
	}

	public override void _PhysicsProcess(float delta)
	{
		base._PhysicsProcess(delta);
		GetInput();
		ApplyGravityForce();
		CheckKnockback("knockback", 500, 200);
		ApplyKnockbackForce();
		velocity = MoveAndSlide(velocity, Vector2.Up);
		lvlMgr.WrapAroundBoundary(this);

		anim.CurrentAnimation = velocity.x == 0 ? "idle" : "walk";
		sprite.Scale = new Vector2(spriteScaleX * (velocity.x < -.1 ? -1 : 1), sprite.Scale.y);

		Attack();
	}

}
