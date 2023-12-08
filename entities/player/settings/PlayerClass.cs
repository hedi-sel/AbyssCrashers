using Godot;
using System;

public partial class PlayerClass : Resource
{
    public enum Id
    {
        Warrior,
        Mage,
        Rogue,
        Huntress
    }

    [Export] public Id ClassId = Id.Warrior;
    [Export] public SpriteFrames Frames;

}