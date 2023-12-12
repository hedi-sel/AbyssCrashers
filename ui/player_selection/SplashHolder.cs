using Godot;
using System;

public partial class SplashHolder : TextureRect
{
    [Export] public Texture2D Warrior;
    [Export] public Texture2D Mage;
    [Export] public Texture2D Rogue;
    [Export] public Texture2D Huntress;

    private TextureRect _childRect;

    public override void _EnterTree()
    {
        base._EnterTree();
        _childRect = GetNode<TextureRect>(nameof(TextureRect));
    }

    public void ShowTexture(PlayerClass.Id option)
    {
        var newTexture = option switch
        {
            PlayerClass.Id.Warrior => Warrior,
            PlayerClass.Id.Mage => Mage,
            PlayerClass.Id.Rogue => Rogue,
            PlayerClass.Id.Huntress => Huntress,
            _ => throw new NotImplementedException("Class not implemented")
        };
        _childRect.Texture = this.Texture;
        _childRect.Modulate = Colors.White;
        this.Texture = newTexture;
        var tween = GetTree().CreateTween().BindNode(this);
        tween.TweenProperty(_childRect, "modulate", Colors.Transparent, 0.2);
        tween.Play();
    }
}