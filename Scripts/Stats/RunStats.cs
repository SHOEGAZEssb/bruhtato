using Bruhtato.Scripts.Player;
using Godot;
using System.Collections.Generic;

public enum StatType
{
  Undefined,

  HP,
  MoveSpeed,
  Damage,
  RangedDamage,
  MeleeDamage,
  AttackSpeed,
  Armor,
  Luck,
  Harvesting,

  EnemyDamage,
  EnemyHealth,
  EnemySpeed,
  EnemyExpDrop
}

/// <summary>
/// Holds all stat values for the player, including base and modifiers.
/// </summary>
public partial class RunStats : Node
{
  [Signal] public delegate void StatsChangedEventHandler(int statType);

  [Export] public CharacterData CharacterPreset;

  public Dictionary<StatType, Stat> Stats => _stats;
  private readonly Dictionary<StatType, Stat> _stats = new();

  public override void _Ready()
  {
    GameManager.Instance.RunStats = this;

    if (CharacterPreset != null)
    {
      // main
      _stats[StatType.HP] = new Stat(CharacterPreset.HP);
      _stats[StatType.MoveSpeed] = new Stat(CharacterPreset.MoveSpeed);
      _stats[StatType.Damage] = new Stat(CharacterPreset.Damage);
      _stats[StatType.AttackSpeed] = new Stat(CharacterPreset.AttackSpeed);

      // enemies
      _stats[StatType.EnemyDamage] = new Stat(CharacterPreset.EnemyDamage);
      _stats[StatType.EnemyHealth] = new Stat(CharacterPreset.EnemyHealth);
    }
    else
      GD.PrintErr("No CharacterPreset defined");
  }

  public void AddModifier(StatType statType, StatModifier mod)
  {
    if (_stats.TryGetValue(statType, out var stat))
      stat.AddModifier(mod);

    EmitSignal(SignalName.StatsChanged, (int)statType);
  }

  public float GetStat(StatType statType)
  {
    return _stats.TryGetValue(statType, out var stat) ? stat.GetValue() : 0f;
  }
}
