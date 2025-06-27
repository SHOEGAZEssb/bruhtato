using Godot;

/// <summary>
/// Sets camera limits to match the arena bounds.
/// </summary>
public partial class CameraLimiter : Camera2D
{
  public override void _Ready()
  {
    var bounds = GameManager.Instance.ArenaBounds.Bounds;

    LimitLeft = (int)bounds.Position.X;
    LimitTop = (int)bounds.Position.Y;
    LimitRight = (int)bounds.End.X;
    LimitBottom = (int)bounds.End.Y;


    Enabled = true;
  }
}
