using System;
using AbyssCrashers.world.scripts.helpers;
using Godot;

public partial class PlayerControl : EntityControl
{
    [Export] public int Id;
    [Export] public PlayerClass.Id PlayerClass;
    [Export] public int RunSpeed = 120;
    [Export] public float KnockbackMultiplier = 1f;

    public static event Action<int, RoomId> PlayerMoveRoom;

    private PlayerInput _playerInput;
    private AdvancedAnimationPlayer _animationPlayer;
    private ProjectileLauncher _projectileLauncher;
    private AnimatedSprite2D _animationSprite;
    private HealthBar _healthBar;

    public int PlayerOwner => _playerInput.GetMultiplayerAuthority();

    public override void _EnterTree()
    {
        base._EnterTree();
        InstanceHolder.Get<IEntityLayer>().RegisterPlayer(this);
    }

    public override void _Ready()
    {
        base._Ready();

        _playerInput = GetNode<PlayerInput>(nameof(PlayerInput));

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

    public bool Invulnerable { get; private set; }

    private Vector2 _knockbackVelocity = Vector2.Zero;

    public override bool TakeDamage(Vector2 knockback, float damage)
    {
        if (Invulnerable) return false;

        base.TakeDamage(knockback, damage);
        knockback *= KnockbackMultiplier;

        var tween = GetTree().CreateTween();
        tween.TweenMethod(Callable.From<Vector2>(v => _knockbackVelocity = v),
                knockback.Normalized() * 400, Vector2.Zero, knockback.Length() * 0.15f)
            .SetTrans(Tween.TransitionType.Cubic)
            .SetEase(Tween.EaseType.In);
        tween.Play();
        return true;
    }

    private const float FlickerTime = 1f;

    protected override void Flicker()
    {
        Rpc(nameof(Flicker_Internal), false);
        Flicker_Internal(true);
    }

    [Rpc]
    private void Flicker_Internal(bool isServer)
    {
        if (isServer) Invulnerable = true;

        var tween = CreateTween();
        const int count = 10;
        const float dt = FlickerTime / count / 2;
        for (var i = 0; i < count; i++)
        {
            tween.TweenProperty(this, "modulate:a", 0, dt);
            tween.TweenProperty(this, "modulate:a", 1, dt);
        }

        if (Multiplayer.IsServer())
            tween.TweenCallback(Callable.From(() => Invulnerable = false)).SetDelay(0.3f);
        tween.Play();
    }

    public override void _PhysicsProcess(double delta)
    {
        var room = RoomId.FromPosition(Position);
        if (room != CurrentRoom)
            SetRoom(room);

        if (_knockbackVelocity != Vector2.Zero)
        {
            Velocity = _knockbackVelocity;
            MoveAndSlide();
            if (_knockbackVelocity.Y == 0 || Math.Abs(_knockbackVelocity.X / _knockbackVelocity.Y) > 0.5f)
                FaceDirection(_knockbackVelocity.X < 0);
            return;
        }

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

    private void SetRoom(RoomId room)
    {
        PlayerMoveRoom?.Invoke(PlayerOwner, room);

        CurrentRoom = room;
        if (PlayerOwner == Multiplayer.GetUniqueId())
            SetCamera(room.X, room.Y);
        else
            RpcId(PlayerOwner, nameof(SetCamera), room.X, room.Y);
    }

    [Rpc]
    private void SetCamera(int x, int y)
        => InstanceHolder.Get<CameraMovement>().MoveToRoom(new RoomId(x, y));

    public override void _ExitTree()
    {
        base._EnterTree();
        InstanceHolder.Get<IEntityLayer>().UnregisterPlayer(this);
    }

    protected override void Die()
    {
    }
}