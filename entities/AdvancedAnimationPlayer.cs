using Godot;
using System;

public partial class AdvancedAnimationPlayer : AnimationPlayer
{
    private string _playingEvent;

    private string _savedAnimation;
    private double _savedPosition;

    public override void _EnterTree()
    {
        base._EnterTree();
        AnimationFinished += OnAnimationFinished;
    }

    public void AdvanceRpc(double delta)
    {
        Advance(delta);
        Rpc(nameof(Advance), delta);
    }

    public void PlayIfNeeded(string name, bool backwards = false)
    {
        PlayIfNeeded_Internal(name, backwards);
        Rpc(nameof(PlayIfNeeded_Internal), name, backwards);
    }

    public void PlayEvent(string name)
    {
        PlayEvent_Internal(name);
        Rpc(nameof(PlayEvent_Internal), name);
    }

    [Rpc]
    private void PlayIfNeeded_Internal(string name, bool backwards = false)
    {
        if (name == CurrentAnimation || _playingEvent != null) return;
        if (backwards)
            PlayBackwards(name);
        else
            Play(name);
    }

    [Rpc]
    public void PlayEvent_Internal(string name)
    {
        if (_playingEvent == null)
        {
            _savedAnimation = CurrentAnimation;
            _savedPosition = CurrentAnimationPosition;
        }

        _playingEvent = name;
        Play(name);
    }

    private void OnAnimationFinished(StringName name)
    {
        if (_playingEvent == name)
            _playingEvent = null;
        if (_savedAnimation != null)
        {
            Play(_savedAnimation);
            Advance(_savedPosition);
            _savedAnimation = null;
            _savedPosition = 0;
        }
        else
            Play("idle");
    }
}