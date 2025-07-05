using Godot;

/// <summary>
/// Projectile that moves in a straight line, damages enemies, and disappears when out of bounds or after penetrating too many enemies.
/// </summary>
public partial class Projectile : Area2D
{
  [Export] public float Speed = 400f;
  [Export] public float Damage = 1f;
  [Export] public float Lifetime = 1f;
  [Export] public int MaxPenetrations = 1;

  private Vector2 _direction;
  private int _penetrationsRemaining;
  private Rect2 _arenaBounds;

  public void Initialize(Vector2 direction, float damage, float range, int penetrate, Rect2 arenaBounds)
  {
    _direction = direction.Normalized();
    Rotation = direction.Angle();
    Damage = damage;
    Lifetime = range / Speed;
    MaxPenetrations = penetrate;
    _penetrationsRemaining = penetrate;
    _arenaBounds = arenaBounds;

    var timer = new Timer
    {
      WaitTime = Lifetime,
      OneShot = true
    };
    AddChild(timer);
    timer.Timeout += QueueFree;
    // defer call because timer might not be in the scene tree yet
    timer.CallDeferred("start");
  }

  public override void _PhysicsProcess(double delta)
  {
    Position += _direction * Speed * (float)delta;

    if (!_arenaBounds.HasPoint(GlobalPosition))
      QueueFree();
  }

  public override void _Ready()
  {
    AreaEntered += OnAreaEntered;
  }

  private void OnAreaEntered(Area2D area)
  {
    if (area.GetParent() is EnemyBase enemy)
    {
      enemy.TakeDamage(Damage);
      _penetrationsRemaining--;

      if (_penetrationsRemaining <= 0)
        QueueFree();
    }
  }
}
