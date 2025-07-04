using Godot;
using System;

/// <summary>
/// Spawns enemies during waves with a fixed duration and enters shop between waves.
/// </summary>
public partial class WaveSpawner : Node
{
  [Export] public WaveSet WaveConfig;
  [Export] public NodePath WaveStatusPath;

  [Signal] public delegate void WaveStartedEventHandler(int wave);

  [Signal] public delegate void WaveCleanupEventHandler();

  [Signal] public delegate void WaveEndedEventHandler();

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
    _waveDurationTimer.Timeout += StartWaveCleanup;

    _waveUI = GetNode<WaveStatus>(WaveStatusPath);

    GameManager.Instance.WaveSpawner = this;

    CallDeferred(nameof(SetupSignals));
    StartWave(0);
  }

  private void SetupSignals()
  {
    GameManager.Instance.MoneyPool.MoneyPoolPickupCompleted += () => EndWave();
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

    EmitSignal(SignalName.WaveStarted, index);
  }

  private void SpawnEnemy()
  {
    var enemyScene = _currentWave.EnemyTypes[GD.Randi() % _currentWave.EnemyTypes.Length];
    var enemy = enemyScene.Instantiate<Node2D>();
    enemy.GlobalPosition = GetRandomSpawnPoint();
    GetTree().CurrentScene.AddChild(enemy);
  }

  private void StartWaveCleanup()
  {
    _spawnTimer.Stop();
    RemoveAllEnemies();
    CallDeferred(nameof(CollectLeftoverMoney));
    GD.Print($"Wave {CurrentWave} cleanup.");

    EmitSignal(SignalName.WaveCleanup);
  }

  private void EndWave()
  {
    // todo: level up, picked up chests etc
    GD.Print($"Wave {CurrentWave} ended.");
    EmitSignal(SignalName.WaveEnded);
  }

  private void RemoveAllEnemies()
  {
    var tree = GetTree();
    foreach (var enemyObj in tree.GetNodesInGroup("Enemies"))
    {
      if (enemyObj is EnemyBase enemy)
      {
        enemy.ForceRemove();
      }
    }
  }

  private void CollectLeftoverMoney()
  {
    var tree = GetTree();
    int pickupCount = 0;
    foreach (var enemyObj in tree.GetNodesInGroup("Pickups"))
    {
      if (enemyObj is MoneyPickup m)
      {
        m.FlyTo(GameManager.Instance.MoneyPool, true);
        pickupCount++;
      }
    }

    GameManager.Instance.MoneyPool.PickupCount = pickupCount;
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
