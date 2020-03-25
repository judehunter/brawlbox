using Godot;
using System;

public class Camera : Camera2D
{
    [Export] readonly float shakeDamping;
    [Export] readonly float shakeThreshold;
    readonly Random rng = new Random();
    float shakeAmt = 0;


    //10 - big shake
    // 5 - small shake
    public void Shake(float amt)
    {
        shakeAmt = amt;
    }

    void ReduceShake(float delta)
    {
        shakeAmt = shakeAmt > shakeThreshold ? (shakeAmt - shakeAmt * shakeDamping * delta) : 0;
    }

    public override void _Process(float delta)
    {
        Offset = new Vector2(rng.Next(-1, 1) * shakeAmt, rng.Next(-1, 1) * shakeAmt);
        ReduceShake(delta);
    }
}
