using Godot;
using System.Collections.Generic;

/// <summary>
/// Detects enemy overlaps and applies periodic contact damage with invincibility.
/// </summary>
public partial class PlayerHurtbox : Area2D
{
  [Export] public float InvincibilityTime = 1.0f;
  [Export] public float ContactDamageInterval = 0.5f;

  private bool _isInvincible = false;
  private Timer _invincibilityTimer;
  private Timer _damageTimer;
  private HealthComponent _health;

  private readonly HashSet<Node2D> _overlappingEnemies = new();

  public override void _Ready()
  {
    _invincibilityTimer = new Timer
    {
      WaitTime = InvincibilityTime,
      OneShot = true
    };
    _invincibilityTimer.Timeout += () => _isInvincible = false;
    AddChild(_invincibilityTimer);

    _damageTimer = new Timer
    {
      WaitTime = ContactDamageInterval,
      OneShot = false,
      Autostart = true
    };
    _damageTimer.Timeout += ApplyContactDamage;
    AddChild(_damageTimer);

    _health = GetParent().GetNode<HealthComponent>("HealthComponent");

    BodyEntered += OnBodyEntered;
    BodyExited += OnBodyExited;
  }

  private void OnBodyEntered(Node2D body)
  {
    if (body is EnemyBase enemyBase)
    {
      _overlappingEnemies.Add(body);

      // immediate damage if not invincible and this is the first contact
      if (!_isInvincible)
      {
        _health.TakeDamage(enemyBase.ContactDamage);
        _isInvincible = true;
        _invincibilityTimer.Start();
      }
    }
  }

  private void OnBodyExited(Node2D body)
  {
    if (body is EnemyBase)
      _overlappingEnemies.Remove(body);
  }

  private void ApplyContactDamage()
  {
    if (_overlappingEnemies.Count == 0 || _isInvincible)
      return;

    float totalDamage = 0;

    foreach (var enemy in _overlappingEnemies)
    {
      if (enemy is EnemyBase enemyBase)
        totalDamage += enemyBase.ContactDamage;
    }

    if (totalDamage > 0)
    {
      _health.TakeDamage(totalDamage);
      _isInvincible = true;
      _invincibilityTimer.Start();
    }
  }
}
