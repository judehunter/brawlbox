using Godot;
using System;
using System.Collections.Generic;
using System.Linq;

public class Enemy : KinematicBody2D
{
    static readonly List<Player> players = new List<Player>();

    protected Player nearest;

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
       // players.AddRange(GetTree().GetNodesInGroup("players").Cast<Player>());
    }

    static public void AddPlayer(Player player)
    {
        players.Add(player);
    }
}
