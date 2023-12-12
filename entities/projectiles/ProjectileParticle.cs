using System;
using Godot;

namespace AbyssCrashers.objects;

public partial class ProjectileParticle : Projectile
{
    protected override void Destroy()
    {
        var particleEmitter = GetNode<CpuParticles2D>("CPUParticles2D");
        particleEmitter.Emitting = false;

        GetNode<CpuParticles2D>("CPUParticles2D");
        SetDeferred(nameof(Monitoring), false);
        SetProcess(false);

        var aliveTimer = new Timer
        {
            OneShot = true,
            Autostart = true,
            WaitTime = particleEmitter.Lifetime,
        };
        aliveTimer.Timeout += QueueFree;

        AddChild(aliveTimer);
    }
}