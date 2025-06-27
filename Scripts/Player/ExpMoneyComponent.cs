using Godot;
using System;

/// <summary>
/// Manages the player's experience and money. Money is used for purchases; EXP is used for leveling.
/// </summary>
public partial class ExpMoneyComponent : Node
{
  public int CurrentMoney { get; private set; }
  public int CurrentExp { get; private set; }
  private int _expToLevel = 100; // TODO: scale with level later

  [Signal] public delegate void MoneyChangedEventHandler(int newAmount);
  [Signal] public delegate void ExpChangedEventHandler(int currentExp, int expToLevel);


  /// <summary>
  /// Initializes the component with starting values.
  /// </summary>
  /// <param name="startingMoney">Initial money amount.</param>
  /// <param name="startingExp">Initial experience amount.</param>
  public void Initialize(int startingMoney = 0, int startingExp = 0)
  {
    CurrentMoney = startingMoney;
    CurrentExp = startingExp;

    EmitSignal(SignalName.MoneyChanged, CurrentMoney);
    EmitSignal(SignalName.ExpChanged, CurrentExp);
  }

  /// <summary>
  /// Adds both money and experience (e.g., from pickups).
  /// </summary>
  /// <param name="amount">Amount of exp and money to add.</param>
  public void GainMoneyAndExp(int amount)
  {
    CurrentMoney += amount;
    CurrentExp += amount;

    EmitSignal(SignalName.MoneyChanged, CurrentMoney);
    EmitSignal(SignalName.ExpChanged, CurrentExp, _expToLevel);
  }

  /// <summary>
  /// Spends money if enough is available.
  /// </summary>
  /// <param name="amount">Amount to spend.</param>
  /// <returns>True if successful, false if not enough money.</returns>
  public bool TrySpendMoney(int amount)
  {
    if (CurrentMoney < amount)
      return false;

    CurrentMoney -= amount;
    EmitSignal(SignalName.MoneyChanged, CurrentMoney);
    return true;
  }

  /// <summary>
  /// Adds EXP only (e.g., in case of non-money EXP sources).
  /// </summary>
  public void GainExp(int amount)
  {
    CurrentExp += amount;
    EmitSignal(SignalName.ExpChanged, CurrentExp);
  }

  /// <summary>
  /// Adds money only (e.g., in case of passive income or interest).
  /// </summary>
  public void GainMoney(int amount)
  {
    CurrentMoney += amount;
    EmitSignal(SignalName.MoneyChanged, CurrentMoney);
  }
}
