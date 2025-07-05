using Godot;

/// <summary>
/// Collects money that is not immediately added to the player, but pooled for the next wave.
/// </summary>
public partial class MoneyPool : Area2D
{
  [Signal] public delegate void MoneyPoolPickupCompletedEventHandler();

  public int CollectedMoney
  {
    get => _collectedMoney;
    private set
    {
      _collectedMoney = value;
      _label.Text = CollectedMoney.ToString();
    }
  }
  private int _collectedMoney;

  public int PickupCount
  {
    get => _pickupCount;
    set
    {
      _pickupCount = value;
      
      if (PickupCount <= 0)
      {
        CallDeferred("set_monitoring", false);
        EmitSignal(SignalName.MoneyPoolPickupCompleted);
      }
    }
  }
  private int _pickupCount;

  [Export] public NodePath LabelPath;
  private Label _label;

  public override void _Ready()
  {
    GameManager.Instance.MoneyPool = this;
    AreaEntered += OnAreaEntered;
    _label = GetNode<Label>(LabelPath);
    CallDeferred(nameof(SetupSignals));
  }

  private void SetupSignals()
  {
    GameManager.Instance.WaveSpawner.WaveCleanup += () => CallDeferred("set_monitoring", true);
  }

  public int RemoveFromPool(int amount)
  {
    int returnedValue;
    if (CollectedMoney <= amount)
    {
      returnedValue = CollectedMoney;
      CollectedMoney = 0;
    }
    else
    {
      returnedValue = amount;
      CollectedMoney -= amount;
    }

    return returnedValue;
  }

  private void OnAreaEntered(Area2D area)
  {
    if (area is MoneyPickup pickup)
    {
      CollectedMoney += (int)pickup.Value;
      pickup.QueueFree(); // or return to pool later
      PickupCount--;
    }
  }
}