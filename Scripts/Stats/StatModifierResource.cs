using Godot;

/// <summary>
/// Represents a single modifier to a stat, e.g., +5 or +10%.
/// </summary>
[GlobalClass]
public partial class StatModifierResource : Resource
{
  [Export] public ModifierType Type = ModifierType.Flat;
  [Export] public float Value = 0f;
  [Export] public StatType StatType = StatType.Undefined;
}
