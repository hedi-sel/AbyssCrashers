using Godot;
using System;
using System.Collections.Generic;

public partial class EntityLayer : Instance
{
    public IReadOnlyList<PlayerControl> Players => _players;

    private readonly List<PlayerControl> _players = new();
    public void RegisterPlayer(PlayerControl player) => _players.Add(player);
}