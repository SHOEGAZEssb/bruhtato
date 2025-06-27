using Godot;

/// <summary>
/// Component that manages health and emits signals on change.
/// </summary>
public partial class HealthComponent : Node
{
  [Signal] public delegate void HealthChangedEventHandler(float current, float max);
  [Signal] public delegate void DiedEventHandler();

  public float MaxHP { get; private set; }
  public float CurrentHP { get; private set; }

  /// <summary>
  /// Initializes HP values.
  /// </summary>
  public void Initialize(float hp)
  {
    MaxHP = hp;
    CurrentHP = hp;
    EmitSignal(SignalName.HealthChanged, CurrentHP, MaxHP);
  }

  /// <summary>
  /// Updates MaxHP and optionally preserves the % of remaining HP.
  /// </summary>
  public void SetMaxHP(float newMax, bool preserveRatio = true)
  {
    float ratio = MaxHP > 0 ? CurrentHP / MaxHP : 1f;

    MaxHP = newMax;
    CurrentHP = preserveRatio ? Mathf.Clamp(newMax * ratio, 0f, newMax) : Mathf.Min(CurrentHP, newMax);

    EmitSignal(SignalName.HealthChanged, CurrentHP, MaxHP);
  }


  public void TakeDamage(float amount)
  {
    CurrentHP = Mathf.Clamp(CurrentHP - amount, 0f, MaxHP);
    EmitSignal(SignalName.HealthChanged, CurrentHP, MaxHP);
    if (CurrentHP <= 0f)
      EmitSignal(SignalName.Died);
  }

  public void Heal(float amount)
  {
    CurrentHP = Mathf.Clamp(CurrentHP + amount, 0f, MaxHP);
    EmitSignal(SignalName.HealthChanged, CurrentHP, MaxHP);
  }
}
