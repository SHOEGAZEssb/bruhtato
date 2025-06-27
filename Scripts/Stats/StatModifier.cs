public enum ModifierType
{
  Flat,
  Percent
}

/// <summary>
/// Represents a single modifier to a stat, e.g., +5 or +10%.
/// </summary>
public struct StatModifier
{
  public ModifierType Type;
  public float Value;

  public StatModifier(float value, ModifierType type)
  {
    Value = value;
    Type = type;
  }
}
