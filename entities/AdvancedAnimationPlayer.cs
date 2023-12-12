using Godot;
using System;

public partial class AdvancedAnimationPlayer : AnimationPlayer
{
    private string _playingEvent;

    private string _savedAnimation;
    private double _savedPosition;

    public void PlayIfNeeded(string name, bool backwards = false)
    {
        PlayIfNeeded_Internal(name, backwards);
        Rpc(nameof(PlayIfNeeded_Internal), name, backwards);
    }

    public void PlayEvent(string name, bool continueCurrentAnimation = true)
    {
        PlayEvent_Internal(name, continueCurrentAnimation);
        Rpc(nameof(PlayEvent_Internal), name, continueCurrentAnimation);
    }


    public void AdvanceRpc(double delta)
    {
        Advance(delta);
        Rpc(nameof(Advance), delta);
    }

    //Private

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
    private void PlayEvent_Internal(string name, bool continueCurrentAnimation)
    {
        if (continueCurrentAnimation && !string.IsNullOrEmpty(CurrentAnimation))
        {
            _savedAnimation = CurrentAnimation;
            _savedPosition = CurrentAnimationPosition;
        }

        if (!continueCurrentAnimation)
            _savedAnimation = null;

        _playingEvent = name;
        Play(name);
    }

    public override void _EnterTree()
    {
        base._EnterTree();
        AnimationFinished += OnAnimationFinished;
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
    }
}