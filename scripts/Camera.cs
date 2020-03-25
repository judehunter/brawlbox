using Godot;
using System;

public class Camera : Camera2D
{
    [Export] readonly float shakeDamping;
    [Export] readonly float shakeThreshold;
    readonly Random rng = new Random();
    float shakeAmt = 0;
    protected ShaderMaterial CRTShader;


    //10 - big shake
    // 5 - small shake
    public override void _Ready()
    {
        CRTShader = GetTree().Root.GetNode<ColorRect>("Game/CRTShader/ColorRect").Material as ShaderMaterial;
    }

    public void Shake(float amt)
    {
        shakeAmt = amt;
        CRTShader.SetShaderParam("aberration_amount", Math.Abs(shakeAmt)*0.6 + 0.4);
        CRTShader.SetShaderParam("boost", Math.Abs(shakeAmt) * 0.05 + 1.38);
    }

    void ReduceShake(float delta)
    {
        shakeAmt = shakeAmt > shakeThreshold ? (shakeAmt - shakeAmt * shakeDamping * delta) : 0;
        CRTShader.SetShaderParam("aberration_amount", Math.Abs(shakeAmt)*0.6 + 0.4);
        CRTShader.SetShaderParam("boost", Math.Abs(shakeAmt) * 0.05 + 1.38);
    }

    public override void _Process(float delta)
    {
        Offset = new Vector2(rng.Next(-1, 1) * shakeAmt, rng.Next(-1, 1) * shakeAmt);
        ReduceShake(delta);
    }
}
