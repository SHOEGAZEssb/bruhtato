using Godot;

/// <summary>
/// Automatically fires projectiles at the nearest enemy.
/// </summary>
public partial class Weapon : Node2D
{
  [Export] public WeaponData Data;
  [Export] public PackedScene ProjectileScene;

  private Marker2D _shootPoint;
  private Timer _fireTimer;

  public override void _Ready()
  {
    _shootPoint = GetNode<Marker2D>("ShootPoint");
    _fireTimer = GetNode<Timer>("FireTimer");

    _fireTimer.WaitTime = 1f / Data.FireRate;
    _fireTimer.Timeout += TryShoot;
    _fireTimer.Start();

    GetNode<Sprite2D>("Sprite2D").Texture = Data.Sprite;
  }

  public override void _Process(double delta)
  {
    var target = FindNearestEnemy(); // or cache this per timer if expensive
    if (target != null)
    {
      var dir = (target.GlobalPosition - GlobalPosition).Normalized();
      Rotation = dir.Angle();
    }
  }


  private void TryShoot()
  {
    var target = FindNearestEnemy();
    if (target == null)
      return;

    var direction = Transform.X.Normalized();

    var projectile = ProjectileScene.Instantiate<Projectile>();
    projectile.GlobalPosition = _shootPoint.GlobalPosition;
    projectile.Initialize(direction, Data.BaseDamage, Data.Range, Data.Penetrate, GameManager.Instance.ArenaBounds.Bounds);

    GetTree().CurrentScene.AddChild(projectile);
  }

  private Node2D FindNearestEnemy()
  {
    var enemies = GetTree().GetNodesInGroup("Enemies");
    Node2D nearest = null;
    float nearestDistance = Data.Range;

    foreach (var node in enemies)
    {
      if (node is Node2D enemy)
      {
        float dist = GlobalPosition.DistanceTo(enemy.GlobalPosition);
        if (dist < nearestDistance)
        {
          nearest = enemy;
          nearestDistance = dist;
        }
      }
    }

    return nearest;
  }
}
