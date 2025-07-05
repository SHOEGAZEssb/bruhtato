using Godot;

/// <summary>
/// Automatically fires projectiles at the nearest enemy when off cooldown.
/// </summary>
public partial class Weapon : Node2D
{
  [Export] public WeaponData Data;
  [Export] public PackedScene ProjectileScene;

  private Marker2D _shootPoint;
  private float _cooldownRemaining = 0f;

  public override void _Ready()
  {
    _shootPoint = GetNode<Marker2D>("ShootPoint");
    GetNode<Sprite2D>("Sprite2D").Texture = Data.Sprite;
  }

  public override void _Process(double delta)
  {
    _cooldownRemaining -= (float)delta;

    var target = FindNearestEnemy();
    if (target != null)
    {
      var dir = (target.GlobalPosition - GlobalPosition).Normalized();
      Rotation = dir.Angle();

      if (_cooldownRemaining <= 0f)
      {
        Fire(target);
        _cooldownRemaining = GetCooldown();
      }
    }
  }

  private void Fire(Node2D target)
  {
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

  private float GetCooldown()
  {
    var attackSpeed = GameManager.Instance.RunStats.GetStat(StatType.AttackSpeed);
    var multiplier = 1f + (attackSpeed / 100f);

    // prevent zero or negative cooldown (e.g., if attackSpeed = -100 or worse)
    multiplier = Mathf.Max(multiplier, 0.05f); // allows up to -95% speed

    return 1f / (Data.FireRate * multiplier);
  }


}
