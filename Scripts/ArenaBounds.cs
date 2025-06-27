using Godot;

/// <summary>
/// Provides a globally accessible arena rectangle to constrain player and camera.
/// </summary>
public partial class ArenaBounds : Node2D
{
  [Export] public Rect2 Bounds = new Rect2(0, 0, 1920, 1080);

  public override void _Ready()
  {
    GameManager.Instance.ArenaBounds = this;

    var visual = GetNodeOrNull<NinePatchRect>("../ArenaBoundsVisual");
    if (visual != null)
    {
      visual.Position = Bounds.Position;
      visual.Size = Bounds.Size;
    }
  }

  /// <summary>
  /// Clamps a world-space position to be within the arena bounds.
  /// </summary>
  public Vector2 ClampPosition(Vector2 position)
  {
    var clamped = position;
    clamped.X = Mathf.Clamp(clamped.X, Bounds.Position.X, Bounds.End.X);
    clamped.Y = Mathf.Clamp(clamped.Y, Bounds.Position.Y, Bounds.End.Y);
    return clamped;
  }
}
