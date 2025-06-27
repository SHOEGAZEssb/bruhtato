using Godot;
using System.Collections.Generic;
using System.Reflection.Metadata;

/// <summary>
/// Basic shop UI that appears between waves.
/// </summary>
public partial class ShopUI : Control
{
  [Signal]
  public delegate void ShopClosedEventHandler();

  [Export] public NodePath CloseShopButtonPath;
  [Export] public PackedScene ShopItemUIScene;
  [Export] public GridContainer ShopGrid;
  [Export] public int ItemsToShow = 4;

  public override void _Ready()
  {
    Visible = false;

    GetNode<Button>(CloseShopButtonPath).Pressed += () =>
    {
      Visible = false;
      EmitSignal(SignalName.ShopClosed);
    };
  }

  /// <summary>
  /// Opens the shop and populates it with random items from the pool.
  /// </summary>
  public void OpenShop()
  {
    ClearShopGrid();

    var itemPool = GameManager.Instance.ItemPool;
    var items = itemPool.GetRandomItems(ItemsToShow);

    // test
    for (int i = 0; i < 4; i++)
    {
      var itemUI = ShopItemUIScene.Instantiate<ShopItemUi>();
      itemUI.Initialize(items[0]);
      ShopGrid.AddChild(itemUI);
    }

    //foreach (var item in items)
    //{
    //  var itemUI = ShopItemUIScene.Instantiate<ShopItemUi>();
    //  itemUI.Initialize(item);
    //  ShopGrid.AddChild(itemUI);
    //}

    Visible = true;
  }

  /// <summary>
  /// Removes any previously displayed items from the grid.
  /// </summary>
  private void ClearShopGrid()
  {
    foreach (var child in ShopGrid.GetChildren())
      child.QueueFree();
  }
}
