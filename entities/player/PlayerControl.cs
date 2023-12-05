using Godot;

public partial class PlayerControl : CharacterBody2D, IEntityControl
{
    [Export] public int Id;
    [Export] public float Speed = 100.0f;


    private PlayerInput _playerInput;
    private ProjectileLauncher _projectileLauncher;

    public EntityType Type => EntityType.Player;

    public override void _Ready()
    {
        base._Ready();
        _playerInput = GetNode<PlayerInput>("PlayerInput");
        _projectileLauncher = (this as IEntityControl).GetComponent<ProjectileLauncher>();
        Instance.Get<EntityLayer>().RegisterPlayer(this);
    }

    public void TakeDamage(Vector2 origin, float damage)
    {
        throw new System.NotImplementedException();
    }

    public override void _PhysicsProcess(double delta)
    {
        if (Multiplayer.GetUniqueId() != 1)
            return;
        var direction = _playerInput.Movement;
        if (direction != Vector2.Zero)
        {
            Velocity = direction * Speed;
        }
        else
        {
            Velocity = Velocity.MoveToward(Vector2.Zero, Speed);
        }

        MoveAndSlide();

        var attackDirection = _playerInput.Attack;
        if (attackDirection.LengthSquared() > 0.3)
        {
            attackDirection = attackDirection.Normalized();
            _projectileLauncher.KeepActive(attackDirection);
        }
    }
}