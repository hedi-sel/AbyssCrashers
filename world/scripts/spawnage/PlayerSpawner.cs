using Godot;
using System;
using System.Linq;
using Godot.Collections;

public partial class PlayerSpawner : FolderSpawner
{
    private PackedScene PlayerPackedScene => PackedScenes.First();
    [Export] public SummonStation SummonStation;


    private readonly object _playerLoginLock = new();

    private Dictionary<int, PlayerControl> _players = new();
    private int _currentPlayers = 0;

    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        var mainScene = GetParent().GetParent<MainScene>();
        mainScene.PlayerSelected += OnPlayerSelected;

        if (Multiplayer.IsServer())
            Multiplayer.PeerDisconnected += RemovePlayer;

        Spawned += OnPlayerSpawned;
    }

    void OnPlayerSpawned(Node node)
    {
        var player = (PlayerControl)node;
        player.LoadPlayerClass(player.PlayerClass);
    }

    void OnPlayerSelected(PlayerClass.Id selectedClass)
    {
        if (Multiplayer.IsServer())
            AddPlayer(Multiplayer.GetUniqueId(), selectedClass);
        else
            RpcId(1, nameof(AddPlayer), Multiplayer.GetUniqueId(), (int)selectedClass);
    }

    [Rpc(MultiplayerApi.RpcMode.AnyPeer)]
    private void AddPlayer(long longId, PlayerClass.Id selectedClass)
    {
        lock (_playerLoginLock)
        {
            _currentPlayers++;
            var id = (int)longId;
            var player = PlayerPackedScene.Instantiate<PlayerControl>();
            player.LoadPlayerClass(selectedClass);

            player.Id = id;
            player.Name = id.ToString();
            _players.Add(id, player);
            Instance.Get<EntityLayer>().AddChild(player);
            player.GlobalPosition = SummonStation.GetPlayerPosition(_currentPlayers);
        }
    }

    private void RemovePlayer(long longId)
    {
        lock (_playerLoginLock)
        {
            var id = (int)longId;
            _players[id].QueueFree();
            _players.Remove(id);
            _currentPlayers--;
        }
    }
}