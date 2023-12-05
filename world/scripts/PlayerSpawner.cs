using Godot;
using System;
using Godot.Collections;

public partial class PlayerSpawner : MultiplayerSpawner
{
    [Export] public PackedScene PlayerPackedScene;

    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        if (!Multiplayer.IsServer())
            return;

        AddPlayer(Multiplayer.GetUniqueId());

        Multiplayer.PeerConnected += AddPlayer;
        Multiplayer.PeerDisconnected += RemovePlayer;
    }

    private Dictionary<int, PlayerControl> _players = new();

    private void AddPlayer(long longId)
    {
        var id = (int)longId;
        var player = PlayerPackedScene.Instantiate<PlayerControl>();
        player.Id = id;
        player.Name = id.ToString();
        _players.Add(id, player);
        Instance.Get<EntityLayer>().AddChild(player);
        if (longId != 1) player.Position += Vector2.Right * 50;
    }

    private void RemovePlayer(long longId)
    {
        var id = (int)longId;
        _players[id].QueueFree();
        _players.Remove(id);
    }
}