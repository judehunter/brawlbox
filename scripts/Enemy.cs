using Godot;
using System;
using System.Collections.Generic;
using System.Linq;

public class Enemy : KinematicBody2D
{
    static readonly List<Player> players = new List<Player>();

    protected Player nearest;

    protected LevelManager lvlMgr;

    protected void UpdateNearest()
    {
        float minDist = float.MaxValue;
        Player minDistPlayer = null;
        foreach (var player in players)
        {
            float dist = player.Position.DistanceTo(Position);
            if (dist < minDist)
            {
                minDist = dist;
                minDistPlayer = player;
            }
        }
        nearest = minDistPlayer;
    }

    public override void _Ready()
    {
        lvlMgr = GetTree().Root.GetNode<LevelManager>("Level");
    }

    static public void AddPlayer(Player player)
    {
        players.Add(player);
    }
}
