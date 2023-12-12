using Godot;
using System;

public partial class PlayerSelect : Button
{
    [Export] public PlayerClass.Id ClassId;

    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        Pressed += OnPressed;
        MouseEntered += OnFocus;
        FocusEntered += OnFocus;
    }

    private void OnFocus()
    {
        GetParent().GetParent().GetParent().GetNode<SplashHolder>(nameof(TextureRect))
            .ShowTexture(ClassId);
    }

    private void OnPressed()
    {
        GetParent().GetParent().GetParent().GetParent<MainScene>().OnPlayerSelected(ClassId);
    }
}