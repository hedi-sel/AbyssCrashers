using Godot;
using System;

public partial class MainScene : Node2D
{
    [Export] public PackedScene World;

    private CanvasLayer _multiplayerMenu;
    private CanvasLayer _playerSelectionMenu;
    private Node _world;

    public event Action<PlayerClass.Id> PlayerSelected;

    public override void _Ready()
    {
        base._Ready();
        _multiplayerMenu = GetNode<CanvasLayer>("MultiplayerUI");
        _playerSelectionMenu = GetNode<CanvasLayer>("PlayerSelectionMenu");
    }

    public void OnMultiplayerConnected()
    {
        _multiplayerMenu.Hide();
        _playerSelectionMenu.Show();
        LoadWorld();
        if (!Multiplayer.IsServer())
            StartGame();
    }

    public void OnPlayerSelected(PlayerClass.Id selectedClass)
    {
        _playerSelectionMenu.Hide();
        PlayerSelected?.Invoke(selectedClass);
        if (Multiplayer.IsServer())
            StartGame();
    }

    private void LoadWorld()
    {
        _world = World.Instantiate();
        GetTree().Paused = true;
        AddChild(_world);
        MoveChild(_world, 0);
    }

    private void StartGame()
    {
        GetTree().Paused = false;
    }
}