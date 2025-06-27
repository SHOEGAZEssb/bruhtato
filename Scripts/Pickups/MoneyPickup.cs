using Godot;

/// <summary>
/// Money pickup that grants money/exp when collected.
/// </summary>
public partial class MoneyPickup : Area2D
{
  public float Value { get; private set; }

  private Vector2 _velocity;
  private const float Friction = 800f;

  private Player _target;
  private bool _isFlying;

  public void Initialize(float value)
  {
    Value = value;
  }

  public override void _Ready()
  {
    BodyEntered += OnBodyEntered;
  }

  private void OnBodyEntered(Node body)
  {
    if (body is Player)
    {
      GameManager.Instance.Player.ExpMoneyComponent.GainMoneyAndExp((int)Value);
      QueueFree();
    }
  }

  public void FlyTo(Player player)
  {
    _target = player;
    _isFlying = true;
  }

  public void ApplyImpulse(Vector2 impulse)
  {
    _velocity = impulse;
  }

  public override void _PhysicsProcess(double delta)
  {
    if (_isFlying && IsInstanceValid(_target))
    {
      var dir = (_target.GlobalPosition - GlobalPosition).Normalized();
      Position += dir * 500f * (float)delta;
      return;
    }

    if (_velocity.LengthSquared() > 1f)
    {
      Position += _velocity * (float)delta;
      _velocity = _velocity.MoveToward(Vector2.Zero, Friction * (float)delta);
    }
  }
}
