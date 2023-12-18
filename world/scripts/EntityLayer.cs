using Godot;
using System;
using System.Collections.Generic;

public partial class EntityLayer : TileMap
{
    public EntityLayer()
    {
        InstanceHolder.Register(this);
    }

    public IReadOnlyList<PlayerControl> Players => _players;

    private readonly List<PlayerControl> _players = new();
    public void RegisterPlayer(PlayerControl player) => _players.Add(player);
    public void UnregisterPlayer(PlayerControl player) => _players.Remove(player);
}