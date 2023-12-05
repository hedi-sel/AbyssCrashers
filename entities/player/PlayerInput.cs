using Godot;
using System;

public partial class PlayerInput : MultiplayerSynchronizer
{
    [Export] public Vector2 Movement = Vector2.Zero;
    [Export] public Vector2 Attack = Vector2.Zero;

    public override void _EnterTree()
    {
        base._EnterTree();
        SetMultiplayerAuthority(GetParent<PlayerControl>().Id);
    }

    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        SetProcess(GetMultiplayerAuthority() == Multiplayer.GetUniqueId());
        GetGodotPropertyList();
    }

    // Called every frame. 'delta' is the elapsed time since the previous frame.
    public override void _Process(double delta)
    {
        Movement = Input.GetVector("ui_left", "ui_right", "ui_up", "ui_down");
        Attack = Input.GetVector("attack_left", "attack_right", "attack_up", "attack_down");
    }
}