using Godot;

/// <summary>
/// Singleton manager that provides runtime access to key game systems like PlayerStats.
/// </summary>
public partial class GameManager : Node
{
  public static GameManager Instance { get; private set; }

  public Player Player { get; set; }

  public RunStats RunStats { get; set; }

  public ItemPool ItemPool { get; set; }

  public ArenaBounds ArenaBounds { get; set; }

  public MoneyPool MoneyPool { get; set; }

  public WaveSpawner WaveSpawner { get; set; }

  public override void _EnterTree()
  {
    Instance = this;
  }
}
