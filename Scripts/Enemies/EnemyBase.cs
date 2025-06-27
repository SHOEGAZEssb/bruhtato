using Godot;

/// <summary>
/// Base class for all enemies. Manages stats, damage, and money drops.
/// </summary>
public partial class EnemyBase : CharacterBody2D
{
  [Export] public EnemyData Data;

  [Export] public PackedScene MoneyPickupScene;

  public float ContactDamage => Data.Damage;
  public float MoveSpeed => Data.MoveSpeed;

  private float _currentHP;

  public override void _Ready()
  {
    if (Data == null)
    {
      GD.PushError($"EnemyData not assigned on {Name}.");
      QueueFree();
      return;
    }

    _currentHP = Data.MaxHP;
  }

  public void TakeDamage(float amount)
  {
    _currentHP -= amount;
    GD.Print($"{Name} took {amount} damage, HP now {_currentHP}");

    if (_currentHP <= 0f)
      Die();
  }

  private void Die()
  {
    DropMoney();
    QueueFree();
  }

  private void DropMoney()
  {
    if (MoneyPickupScene == null)
      return;

    var amount = Mathf.FloorToInt(Data.MoneyDrop);
    for (int i = 0; i < amount; i++)
    {
      var pickup = MoneyPickupScene.Instantiate<MoneyPickup>();
      pickup.Initialize(1); // each worth 1
      pickup.GlobalPosition = GlobalPosition;

      var angle = (float)GD.RandRange(0, Mathf.Tau); // 0 to 2π
      var force = (float)GD.RandRange(150, 300);      // tweak as needed
      var velocity = new Vector2(Mathf.Cos(angle), Mathf.Sin(angle)) * force;

      pickup.ApplyImpulse(velocity); // you'll add this method below

      GetTree().CurrentScene.CallDeferred("add_child", pickup);
    }
  }
}