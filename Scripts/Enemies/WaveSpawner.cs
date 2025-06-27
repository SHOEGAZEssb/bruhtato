using Godot;
using System;

/// <summary>
/// Spawns enemies during waves with a fixed duration and enters shop between waves.
/// </summary>
public partial class WaveSpawner : Node
{
  [Export] public WaveSet WaveConfig;
  [Export] public NodePath WaveStatusPath;

  [Signal]
  public delegate void WaveEndedEventHandler();

  public int CurrentWave { get; private set; } = 0;

  private Timer _spawnTimer;
  private Timer _waveDurationTimer;
  private WaveStatus _waveUI;
  private WaveData _currentWave;

  public override void _Ready()
  {
    _spawnTimer = new Timer { OneShot = false };
    _waveDurationTimer = new Timer { OneShot = true };

    AddChild(_spawnTimer);
    AddChild(_waveDurationTimer);

    _spawnTimer.Timeout += SpawnEnemy;
    _waveDurationTimer.Timeout += EndWave;

    _waveUI = GetNode<WaveStatus>(WaveStatusPath);

    StartWave(0);
  }

  public void StartWave(int index)
  {
    if (index >= WaveConfig.Waves.Length)
    {
      GD.Print("All waves completed.");
      return;
    }

    CurrentWave = index + 1;
    _currentWave = WaveConfig.Waves[index];

    _waveUI.SetWave(CurrentWave);
    _waveUI.StartCountdown(_currentWave.Duration);

    _spawnTimer.WaitTime = _currentWave.TimeBetweenSpawns;
    _spawnTimer.Start();

    _waveDurationTimer.Start(_currentWave.Duration);
  }

  private void SpawnEnemy()
  {
    var enemyScene = _currentWave.EnemyTypes[GD.Randi() % _currentWave.EnemyTypes.Length];
    var enemy = enemyScene.Instantiate<Node2D>();
    enemy.GlobalPosition = GetRandomSpawnPoint();
    GetTree().CurrentScene.AddChild(enemy);
  }

  private void EndWave()
  {
    _spawnTimer.Stop();
    GD.Print($"Wave {CurrentWave} ended.");

    EmitSignal(SignalName.WaveEnded);
  }

  private Vector2 GetRandomSpawnPoint()
  {
    var bounds = GameManager.Instance.ArenaBounds.Bounds;
    return new Vector2(
      (float)GD.RandRange(bounds.Position.X, bounds.End.X),
      (float)GD.RandRange(bounds.Position.Y, bounds.End.Y)
    );
  }
}
