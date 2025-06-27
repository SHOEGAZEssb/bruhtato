using Godot;

/// <summary>
/// Displays the player's HP and EXP bars.
/// </summary>
public partial class PlayerStatus : Control
{
  private ProgressBar _hpBar;
  private ProgressBar _expBar;
  private Label _hpLabel;
  private Label _moneyLabel;


  public override void _Ready()
  {
    _hpBar = GetNode<ProgressBar>("VBoxContainer/HPBar");
    _hpLabel = _hpBar.GetNodeOrNull<Label>("Label");
    _expBar = GetNode<ProgressBar>("VBoxContainer/EXPBar");
    _moneyLabel = GetNode<Label>("VBoxContainer/MoneyDisplay/MoneyLabel");
  }

  /// <summary>
  /// Sets the HP bar value.
  /// </summary>
  public void SetHP(float current, float max)
  {
    _hpBar.MaxValue = max;
    _hpBar.Value = current;

    _hpLabel.Text = $"{Mathf.Ceil(current)} / {Mathf.Ceil(max)}";
  }

  /// <summary>
  /// Sets the EXP bar using absolute values.
  /// </summary>
  public void SetEXP(int current, int max)
  {
    _expBar.MaxValue = max;
    _expBar.Value = current;
  }

  /// <summary>
  /// Sets the money label value.
  /// </summary>
  public void SetMoney(int amount)
  {
    _moneyLabel.Text = amount.ToString();
  }

}
