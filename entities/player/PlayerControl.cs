using System;
using Godot;

public partial class PlayerControl : CharacterBody2D, IEntityControl
{
    [Export] public int Id;
    [Export] public PlayerClass.Id PlayerClass;

    private PlayerInput _playerInput;
    private AdvancedAnimationPlayer _animationPlayer;
    private ProjectileLauncher _projectileLauncher;
    private AnimatedSprite2D _animationSprite;

    public EntityType Type => EntityType.Player;
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

    public void TakeDamage(Vector2 origin, float damage)
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

        var attackDirection = _playerInput.Attack;
        if (attackDirection.LengthSquared() > 0.3)
        {
            if (Math.Abs(attackDirection.X) >= Math.Abs(attackDirection.Y))
            {
                attackDirection.Y = 0;
                facingRight = attackDirection.X > 0;
            }
            else
                attackDirection.X = 0;

            attackDirection = attackDirection.Normalized();
            if (_projectileLauncher.KeepActive(attackDirection))
                _animationPlayer.PlayEvent("attack");
        }

        if (facingRight.HasValue) FaceDirection(facingRight.Value);
    }

    private void FaceDirection(bool right)
    {
        _animationSprite.FlipH = !right;
    }

    public override void _ExitTree()
    {
        base._EnterTree();
        Instance.Get<EntityLayer>().UnregisterPlayer(this);
    }
}