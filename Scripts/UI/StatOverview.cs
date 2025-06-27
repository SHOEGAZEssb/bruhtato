using Godot;

/// <summary>
/// Displays current stats from the player's RunStats.
/// </summary>
public partial class StatOverview : Panel
{
  [Export] public NodePath StatContainerPath;
  private VBoxContainer _container;

  public override void _Ready()
  {
    GameManager.Instance.RunStats.StatsChanged += () => UpdateStats();
    _container = GetNode<VBoxContainer>(StatContainerPath);
    UpdateStats();
  }

  public void UpdateStats()
  {
    foreach (var child in _container.GetChildren())
    {
      if (child is Node node)
        node.QueueFree();
    }

    var stats = GameManager.Instance.RunStats.Stats;
    foreach (var kvp in stats)
    {
      var statLabel = new Label();
      statLabel.Text = $"{kvp.Key}: {Mathf.Round(kvp.Value.GetValue() * 100f) / 100f}";
      _container.AddChild(statLabel);
    }
  }
}
