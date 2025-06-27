using Bruhtato.Scripts.Player;
using Godot;
using System.Collections.Generic;

public enum StatType
{
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
  [Signal] public delegate void StatsChangedEventHandler();

  [Export] public CharacterData CharacterPreset;

  public Dictionary<string, Stat> Stats => _stats;
  private readonly Dictionary<string, Stat> _stats = new();

  public override void _Ready()
  {
    GameManager.Instance.RunStats = this;

    if (CharacterPreset != null)
    {
      // main
      _stats["HP"] = new Stat(CharacterPreset.HP);
      _stats["MoveSpeed"] = new Stat(CharacterPreset.MoveSpeed);
      _stats["Damage"] = new Stat(CharacterPreset.Damage);
      _stats["AttackSpeed"] = new Stat(CharacterPreset.AttackSpeed);

      // enemies
      _stats["EnemyDamage"] = new Stat(CharacterPreset.EnemyDamage);
      _stats["EnemyHealth"] = new Stat(CharacterPreset.EnemyHealth);
    }
    else
      GD.PrintErr("No CharacterPreset defined");
  }

  public void AddModifier(string statName, StatModifier mod)
  {
    if (_stats.TryGetValue(statName, out var stat))
      stat.AddModifier(mod);

    EmitSignal(SignalName.StatsChanged);
  }

  public void AddModifiers(Dictionary<string, StatModifier> mods)
  {
    foreach (var kvp in mods)
    {
      if (_stats.TryGetValue(kvp.Key, out var stat))
        stat.AddModifier(kvp.Value);
    }

    EmitSignal(SignalName.StatsChanged);
  }

  public float GetStat(string statName)
  {
    return _stats.TryGetValue(statName, out var stat) ? stat.GetValue() : 0f;
  }
}
