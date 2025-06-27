using Godot;

/// <summary>
/// Detects nearby money and attracts it toward the player.
/// </summary>
public partial class MoneyCollector : Area2D
{
  [Export] public float PullSpeed = 300f;
  private Player _player;

  public override void _Ready()
  {
    _player = GetParent<Player>();
    AreaEntered += OnAreaEntered;
  }

  private void OnAreaEntered(Area2D area)
  {
    if (area is MoneyPickup pickup)
    {
      pickup.FlyTo(_player);
    }
  }

  public void UpdateRadius(float radius)
  {
    if (GetNode<CollisionShape2D>("CollisionShape2D").Shape is CircleShape2D shape)
      shape.Radius = radius;
  }
}
