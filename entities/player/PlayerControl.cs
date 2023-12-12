using System;
using AbyssCrashers.world.scripts.helpers;
using Godot;

public partial class PlayerControl : EntityControl
{
    [Export] public int Id;
    [Export] public PlayerClass.Id PlayerClass;

    private PlayerInput _playerInput;
    private AdvancedAnimationPlayer _animationPlayer;
    private ProjectileLauncher _projectileLauncher;
    private AnimatedSprite2D _animationSprite;

    public int RunSpeed = 120;

    public override void _EnterTree()
    {
        base._EnterTree();
        Instance.Get<EntityLayer>().RegisterPlayer(this);
    }

    public override void _Ready()
    {
        base._Ready();
        if (Multiplayer.GetUniqueId() != 1)
        {
            SetPhysicsProcess(false);
            return;
        }

        _playerInput = GetNode<PlayerInput>(nameof(PlayerInput));
        _animationPlayer = GetNode<AdvancedAnimationPlayer>(nameof(AnimationPlayer));
        _projectileLauncher = GetNode<ProjectileLauncher>(nameof(ProjectileLauncher));
        _animationSprite = GetNode<AnimatedSprite2D>(nameof(AnimatedSprite2D));
    }

    public void LoadPlayerClass(PlayerClass.Id playerClassId)
    {
        PlayerClass = playerClassId;
        this.LoadPlayerClass();
    }

    public override void TakeDamage(Vector2 origin, float damage)
    {
        throw new NotImplementedException();
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

    public override void _ExitTree()
    {
        base._EnterTree();
        Instance.Get<EntityLayer>().UnregisterPlayer(this);
    }
}