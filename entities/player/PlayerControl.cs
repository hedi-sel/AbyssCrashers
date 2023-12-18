using System;
using AbyssCrashers.world.scripts.helpers;
using Godot;

public partial class PlayerControl : EntityControl
{
    [Export] public int Id;
    [Export] public PlayerClass.Id PlayerClass;
    [Export] public int RunSpeed = 120;

    private PlayerInput _playerInput;
    private AdvancedAnimationPlayer _animationPlayer;
    private ProjectileLauncher _projectileLauncher;
    private AnimatedSprite2D _animationSprite;
    private HealthBar _healthBar;

    public int PlayerOwner => _playerInput.GetMultiplayerAuthority();

    public override void _EnterTree()
    {
        base._EnterTree();
        InstanceHolder.Get<EntityLayer>().RegisterPlayer(this);
    }

    public override void _Ready()
    {
        base._Ready();
   
        _playerInput = GetNode<PlayerInput>(nameof(PlayerInput));

        if(PlayerOwner != Multiplayer.GetUniqueId()) // Process enabled for the instance's player 
            SetProcess(false);
        
        if (Multiplayer.GetUniqueId() != 1)
        {
            SetPhysicsProcess(false);
            return;
        }

        HealthUpdate += UpdateHealthBar;

        _healthBar = InstanceHolder.Get<HealthBar>();

        _animationSprite = GetNode<AnimatedSprite2D>(nameof(AnimatedSprite2D));
        _animationPlayer = GetNode<AdvancedAnimationPlayer>(nameof(AnimationPlayer));

        _projectileLauncher = GetNode<ProjectileLauncher>(nameof(ProjectileLauncher));

        UpdateHealthBar(Health);
    }

    public void LoadPlayerClass(PlayerClass.Id playerClassId)
    {
        PlayerClass = playerClassId;
        this.LoadPlayerClass();
    }

    private void UpdateHealthBar(float val)
    {
        _healthBar.SetCurrentHeartsFor(val, PlayerOwner);
        _healthBar.SetMaxHeartsFor(MaxHealth, PlayerOwner);
    }

    public override void TakeDamage(Vector2 knockback, float damage)
    {
        base.TakeDamage(knockback, damage);
        Velocity = knockback * 300;
    }

    protected override void Flicker()
    {
        var tween = CreateTween();
        const float time = 0.06f;
        const int count = 3;
        for (int i = 0; i < count; i++)
        {
            tween.TweenProperty(this, "modulate:a", 0, time);
            tween.TweenProperty(this, "modulate:a", 1, time);
        }

        tween.Play();
    }

    public override void _PhysicsProcess(double delta)
    {
        bool? facingRight = null;
        var direction = _playerInput.Movement;
        if (direction != Vector2.Zero)
        {
            Velocity = direction * RunSpeed;
            if (Math.Abs(Velocity.X) > 0.2)
                facingRight = Velocity.X > 0;
            _animationPlayer.PlayIfNeeded("run");
        }
        else
        {
            Velocity = Velocity.MoveToward(Vector2.Zero, RunSpeed);
            _animationPlayer.PlayIfNeeded("idle");
        }

        MoveAndSlide();

        var attackInput = _playerInput.Attack;
        var attackDirection = attackInput.ToCardinalDirection(0.3);
        if (attackDirection != CardinalDirection.Null)
        {
            facingRight = attackDirection == CardinalDirection.Right ? true
                : attackDirection == CardinalDirection.Left ? false
                : facingRight;

            if (_projectileLauncher.KeepActive(attackDirection.ToVector()))
                _animationPlayer.PlayEvent("attack");
        }

        if (facingRight.HasValue) FaceDirection(facingRight.Value);
    }

    public override void _Process(double delta)
    {
        base._Process(delta);
        InstanceHolder.Get<CameraMovement>().FollowPlayerAt(GlobalPosition);
    }

    public override void _ExitTree()
    {
        base._EnterTree();
        InstanceHolder.Get<EntityLayer>().UnregisterPlayer(this);
    }

    protected override void Die()
    {
        GD.Print("Im dead " + PlayerOwner);
    }
}