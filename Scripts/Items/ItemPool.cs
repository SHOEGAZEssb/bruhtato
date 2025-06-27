using Godot;
using System;
using System.Collections.Generic;
using System.Linq;

/// <summary>
/// Loads and provides access to all available items from a folder.
/// </summary>
public partial class ItemPool : Node
{
  [Export] public string ItemFolderPath = "res://Resources/Items";

  private readonly List<ItemData> _allItems = new();

  public override void _Ready()
  {
    GameManager.Instance.ItemPool = this;

    LoadAllItems();
  }

  private void LoadAllItems()
  {
    var dir = DirAccess.Open(ItemFolderPath);
    if (dir == null)
    {
      GD.PrintErr($"[ItemPool] Could not open folder: {ItemFolderPath}");
      return;
    }

    dir.ListDirBegin();
    while (true)
    {
      var file = dir.GetNext();
      if (string.IsNullOrEmpty(file)) break;

      if (!dir.CurrentIsDir() && file.EndsWith(".tres"))
      {
        var fullPath = $"{ItemFolderPath}/{file}";
        var item = ResourceLoader.Load<ItemData>(fullPath);
        if (item != null)
          _allItems.Add(item);
      }
    }
    dir.ListDirEnd();

    GD.Print($"[ItemPool] Loaded {_allItems.Count} items from {ItemFolderPath}");
  }

  /// <summary>
  /// Returns a list of randomly selected items.
  /// </summary>
  public List<ItemData> GetRandomItems(int count)
  {
    var rng = new Random();
    return _allItems.OrderBy(_ => rng.Next()).Take(count).ToList();
  }
}
