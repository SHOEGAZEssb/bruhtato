using Godot;

/// <summary>
/// Money pickup that grants money/exp when collected or flies to pool storage.
/// </summary>
public partial class MoneyPickup : Area2D
{
  public float Value { get; private set; }

  private Vector2 _velocity;
  private const float Friction = 800f;

  private Node2D _target;

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
    if ((_target == null || _target == body) && body is Player)
    {
      GameManager.Instance.Player.ExpMoneyComponent.GainMoneyAndExp((int)Value);
      QueueFree();
    }
  }

  public void FlyTo(Node2D target, bool pool = false)
  {
    if (_target == null || pool)
      _target = target;
  }

  public void ApplyImpulse(Vector2 impulse)
  {
    _velocity = impulse;
  }

  public override void _PhysicsProcess(double delta)
  {
    var dt = (float)delta;

    if (_target != null && IsInstanceValid(_target))
    {
      var dir = (_target.GlobalPosition - GlobalPosition).Normalized();
      Position += dir * 500f * dt;
      return;
    }

    if (_velocity.LengthSquared() > 1f)
    {
      Position += _velocity * dt;
      _velocity = _velocity.MoveToward(Vector2.Zero, Friction * dt);
    }
  }
}
