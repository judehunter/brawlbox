using Godot;
using System;

public class Entity : KinematicBody2D
{
    [Export] protected readonly float knockbackDamping = 1.2f;
    [Export] protected readonly float knockbackThreshold = .2f;
    [Export] protected readonly float maxKnockback;
    [Export] protected readonly float gravityStrength = 1;
    [Export] protected readonly float jumpStrength = 300;
    [Export] protected readonly float moveSpeed;
    [Export] protected readonly int maxSpeed;
    protected Vector2 velocity;
    protected Vector2 knockback;

    protected LevelManager lvlMgr;

    public override void _Ready()
    {
        lvlMgr = GetTree().Root.GetNode<LevelManager>("Game/Level");
    }

    public void Knockback(float strength, Vector2 dist)
    {
        knockback += strength * dist.Normalized() * new Vector2(1, .03f);
        if (knockback.Length() > maxKnockback) knockback = knockback.Normalized() * maxKnockback;
    }

    protected virtual void ApplyGravityForce()
    {
        velocity.y += 9.8f * gravityStrength;
        if (velocity.y > maxSpeed) velocity.y = maxSpeed;
    }

    protected void ApplyKnockbackForce()
    {
        velocity += knockback;

        knockback /= knockbackDamping;

        if (knockback.Length() < knockbackThreshold)
        {
            knockback = Vector2.Zero;
        }
    }

    protected void CheckKnockback(string group, float strength, float strengthOther)
    {
        for (int i = 0; i < GetSlideCount(); i++)
        {
            var coll = GetSlideCollision(i);
            if (!(coll.Collider is Node2D)) continue;
            var other = coll.Collider as Node2D;
            if (other.IsInGroup(group))
            {
                lvlMgr.camera.Shake(5);
                Vector2 dist = Position - other.Position;
                Knockback(strength, dist);
                if (other is Entity) (other as Entity).Knockback(strengthOther, -dist);
            }
        }
    }
}
