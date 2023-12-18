using Godot;
using System;
using AbyssCrashers.world.scripts.helpers;

public partial class CameraMovement : Camera2D
{
    [Export] public Vector2 RoomSize = new(19 * 16, 11 * 16);


    public CameraMovement()
    {
        InstanceHolder.Register(this);
    }

    private Vector2 _officialPosition;
    private Rect2 _roomBox;

    private Vector2 _sceneScale;
    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        _officialPosition = Position;
        _sceneScale = GetParent<Node2D>().Scale;

        var offset = GetViewportRect().Size / _sceneScale - RoomSize;
        _roomBox = new Rect2(offset / 2, RoomSize);
    }

    public void MoveToNextRoom(CardinalDirection dir)
    {
        var movement = RoomSize * dir.ToVector();
        var tween = GetTree().CreateTween();
        _officialPosition += movement;
        tween.TweenProperty(this, "position", _officialPosition, 0.3);
    }

    public void FollowPlayerAt(Vector2 playerPosition)
    {
        var relativePosition = (playerPosition - GlobalPosition) / _sceneScale;
        if (relativePosition.X < _roomBox.Position.X)
            MoveToNextRoom(CardinalDirection.Left);
        if (relativePosition.X > _roomBox.End.X)
            MoveToNextRoom(CardinalDirection.Right);
        if (relativePosition.Y < _roomBox.Position.Y)
            MoveToNextRoom(CardinalDirection.Up);
        if (relativePosition.Y > _roomBox.End.Y)
            MoveToNextRoom(CardinalDirection.Down);
    }
}