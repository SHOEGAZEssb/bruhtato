using Godot;

/// <summary>
/// Collects money that is not immediately added to the player, but pooled for the next wave.
/// </summary>
public partial class MoneyPool : Area2D
{
  [Signal] public delegate void MoneyPoolPickupCompletedEventHandler();

  public int CollectedMoney { get; private set; }

  public int PickupCount
  {
    get => _pickupCount;
    set
    {
      _pickupCount = value;
      _label.Text = CollectedMoney.ToString();
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
    GameManager.Instance.WaveSpawner.WaveEnded += () => CallDeferred("set_monitoring", false);
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

  //public void ResetPool()
  //{
  //  PickupCount = 0;
  //  CollectedMoney = 0;
  //  _label.Text = "0";
  //}
}
