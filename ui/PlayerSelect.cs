using Godot;
using System;

public partial class PlayerSelect : Button
{
    [Export] public PlayerClass.Id ClassId;

    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        Pressed += OnPressed;
    }

    private void OnPressed()
    {
        GetParent().GetParent().GetParent<MainScene>().OnPlayerSelected(ClassId);
    }
}