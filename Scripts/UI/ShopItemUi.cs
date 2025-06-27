using Godot;

/// <summary>
/// Displays one item in the shop and allows purchasing it.
/// </summary>
public partial class ShopItemUi : Panel
{
  private ItemData _item;

  public void Initialize(ItemData item)
  {
    _item = item;
    GetNode<Label>("VBoxContainer/ItemNameLabel").Text = item.ItemName;
    GetNode<Label>("VBoxContainer/ItemPriceLabel").Text = $"${item.Cost}";
    GetNode<Label>("VBoxContainer/ItemDescriptionLabel").Text = item.Description;
    GetNode<TextureRect>("VBoxContainer/TextureRect").Texture = item.Icon;

    GetNode<Button>("VBoxContainer/BuyButton").Pressed += OnBuyPressed;
  }

  private void OnBuyPressed()
  {
    var inv = GameManager.Instance.Player.InventoryComponent;
    var moneyComponent = GameManager.Instance.Player.ExpMoneyComponent;

    if (moneyComponent.TrySpendMoney(_item.Cost))
    {
      inv.AddItem(_item);
      QueueFree(); // remove from shop
    }
    else
    {
      GD.Print("Not enough money!");
    }
  }
}
