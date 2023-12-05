using Godot;
using System;

public partial class MultiplayerMenu : CanvasLayer
{
    private const int Port = 2461;

    [Export] public PackedScene World;

    [Export] public Button Host;
    [Export] public Button Join;
    [Export] public LineEdit Ip;

    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        // Automatically start the server in headless mode.
        if (DisplayServer.GetName() == "headless")
        {
            GD.Print("Automatically starting dedicated server.");
            OnHostPressed();
        }

        Host.Pressed += OnHostPressed;
        Join.Pressed += () => OnJoinPressed(Ip.Text);
    }

    private void OnHostPressed()
    {
        GD.Print("Starting server");
        // Start as server.
        var peer = new ENetMultiplayerPeer();
        peer.CreateServer(Port);
        if (peer.GetConnectionStatus() == MultiplayerPeer.ConnectionStatus.Disconnected)
        {
            OS.Alert("Failed to start multiplayer server.");
            return;
        }

        Multiplayer.MultiplayerPeer = peer;
        StartGame();
        GD.Print("Server started");
    }


    private void OnJoinPressed(string ip)
    {
        if (string.IsNullOrEmpty(ip))
        {
            OS.Alert("Need an ip address to connect to.");
            return;
        }

        GD.Print(ip);
        var peer = new ENetMultiplayerPeer();
        peer.CreateClient(ip, Port);
        if (peer.GetConnectionStatus() == MultiplayerPeer.ConnectionStatus.Disconnected)
        {
            OS.Alert("Failed to start multiplayer client.");
            return;
        }

        Multiplayer.MultiplayerPeer = peer;
        StartGame();
    }


    void StartGame()
    {
        Hide();
        var worldScene = World.Instantiate();
        GetParent().AddChild(worldScene);
    }
}