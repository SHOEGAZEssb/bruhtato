using Godot;
using System.Collections.Generic;

/// <summary>
/// Stores equipped items and instantiates any effect scripts attached to them.
/// </summary>
public partial class InventoryComponent : Node
{
  [Signal] public delegate void ItemAddedEventHandler(ItemData item);

  private readonly List<ItemData> _items = new();
  private readonly List<Node> _activeItemEffects = new();

  /// <summary>
  /// Adds an item to the inventory, applies stat modifiers and attaches behavior.
  /// </summary>
  public void AddItem(ItemData item)
  {
    _items.Add(item);
    EmitSignal(SignalName.ItemAdded, item);

    // Apply stat modifiers (assumes PlayerStats is reachable)
    if (item.Modifiers != null && GameManager.Instance != null)
    {
      var stats = GameManager.Instance.RunStats;
      foreach (var mod in item.Modifiers)
      {
        stats.AddModifier(mod.StatType, new StatModifier(mod.Value, mod.Type));
      }
    }

    // Spawn item behavior if it has one
    if (item.EffectScript != null)
    {
      //var effect = (Node)item.EffectScript.New();
      //AddChild(effect);
      //_activeItemEffects.Add(effect);
    }
  }

  public IReadOnlyList<ItemData> GetAllItems() => _items.AsReadOnly();
}
