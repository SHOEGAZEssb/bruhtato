using System.Collections.Generic;

/// <summary>
/// Holds a numeric stat with base value and modifiers.
/// </summary>
public class Stat
{
  public float BaseValue { get; set; }
  private readonly List<StatModifier> _modifiers = new();

  public Stat(float baseValue = 0f)
  {
    BaseValue = baseValue;
  }

  public void AddModifier(StatModifier modifier) => _modifiers.Add(modifier);

  public void RemoveModifier(StatModifier modifier) => _modifiers.Remove(modifier);

  public float GetValue()
  {
    float final = BaseValue;
    float percent = 0f;

    foreach (var mod in _modifiers)
    {
      if (mod.Type == ModifierType.Flat)
        final += mod.Value;
      else
        percent += mod.Value;
    }

    return final * (1f + percent);
  }
}
