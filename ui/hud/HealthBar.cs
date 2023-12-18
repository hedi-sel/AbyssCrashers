using Godot;
using System;

public partial class HealthBar : Control
{
    [Export] public TextureRect CurrentHealth;
    [Export] public TextureRect MaxHealth;

    private int _singleHeartWidht;
    private int _halfHeartWidth;

    public HealthBar()
    {
        InstanceHolder.Register(this);
    }

    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        _singleHeartWidht = CurrentHealth.Texture.GetWidth();
        _halfHeartWidth = _singleHeartWidht / 2 + _singleHeartWidht % 2;
    }


    public void SetMaxHeartsFor(float hp, int id)
    {
        if (id == Multiplayer.GetUniqueId())
            SetMaxHearts_Internal(hp);
        else RpcId(id, nameof(SetMaxHearts_Internal), hp);
    }

    public void SetCurrentHeartsFor(float hp, int id)
    {
        if (id == Multiplayer.GetUniqueId())
            SetCurrentHearts_Internal(hp);
        else RpcId(id, nameof(SetCurrentHearts_Internal), hp);
    }

    int GetSize(float hp)
    {
        int hpFloor = (int)hp;
        return hpFloor * _singleHeartWidht + (hp > hpFloor ? _halfHeartWidth : 0);
    }

    [Rpc]
    private void SetMaxHearts_Internal(float hp)
    {
        MaxHealth.Size = new Vector2(GetSize(hp), MaxHealth.Size.Y);
    }

    [Rpc]
    private void SetCurrentHearts_Internal(float hp)
    {
        CurrentHealth.Size = new Vector2(GetSize(hp), CurrentHealth.Size.Y);
    }
}