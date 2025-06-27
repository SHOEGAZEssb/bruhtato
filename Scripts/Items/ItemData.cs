using Godot;

public enum RarityType
{
  Common,
  Uncommon,
  Rare,
  Legendary
}

[GlobalClass]
public partial class ItemData : Resource
{
  [Export] public string ItemName;
  [Export(PropertyHint.MultilineText)] public string Description;
  [Export] public Texture2D Icon;
  [Export] public int Cost = 10;
  [Export] public RarityType Rarity = RarityType.Common;
  [Export] public StatModifierResource[] Modifiers;
  [Export] public Script EffectScript;
}
