using Godot;
using System;
using AbyssCrashers.world.scripts.helpers;

public partial class CameraMovement : Camera2D
{
    public CameraMovement()
    {
        InstanceHolder.Register(this);
    }

    private Vector2 _cameraOriginPosition;

    private Vector2 _sceneScale;

    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        _sceneScale = GetParent<Node2D>().Scale;
        _cameraOriginPosition = Position;
    }

    public void MoveToRoom(RoomId room)
    {
        var tween = GetTree().CreateTween();
        tween.TweenProperty(this, "position", _cameraOriginPosition + room.ToPosition(), 0.3);
    }

    public override void _Process(double delta)
    {
        base._Process(delta);
        if (Input.IsKeyPressed(Key.Kp5))
            MoveToRoom(new RoomId(0, 0));
        if (Input.IsKeyPressed(Key.Kp6))
            MoveToRoom(new RoomId(1, 0));
        if (Input.IsKeyPressed(Key.Kp4))
            MoveToRoom(new RoomId(-1, 0));
        if (Input.IsKeyPressed(Key.Kp8))
            MoveToRoom(new RoomId(0, -1));
        if (Input.IsKeyPressed(Key.Kp2))
            MoveToRoom(new RoomId(0, 1));
    }
}