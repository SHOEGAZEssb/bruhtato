using Godot;

/// <summary>
/// Displays wave number and countdown timer.
/// </summary>
public partial class WaveStatus : Control
{
  private Label _waveLabel;
  private Label _timerLabel;
  private float _timeRemaining;
  private bool _countingDown = false;

  public override void _Ready()
  {
    _waveLabel = GetNode<Label>("VBoxContainer/WaveLabel");
    _timerLabel = GetNode<Label>("VBoxContainer/TimerLabel");

    _waveLabel.Text = "Wave 0";
    _timerLabel.Text = "";
  }

  public void SetWave(int waveNumber)
  {
    _waveLabel.Text = $"Wave {waveNumber}";
  }

  public void StartCountdown(float duration)
  {
    _timeRemaining = duration;
    _countingDown = true;
  }

  public override void _Process(double delta)
  {
    if (_countingDown)
    {
      _timeRemaining -= (float)delta;
      _timerLabel.Text = $"Next wave in: {_timeRemaining:F1}s";

      if (_timeRemaining <= 0f)
      {
        _countingDown = false;
        _timerLabel.Text = "";
      }
    }
  }
}
