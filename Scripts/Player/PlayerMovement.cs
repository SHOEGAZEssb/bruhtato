using Godot;

/// <summary>
/// Handles top-down movement logic for the player.
/// </summary>
public partial class PlayerMovement : Node
{
  private RunStats _stats;
  private CharacterBody2D _body;
  private float _clampMargin = 0f;

  /// <summary>
  /// Caches reference to parent body on ready.
  /// </summary>
  public override void _Ready()
  {
    _body = GetParent<CharacterBody2D>();
    if (_body == null)
      GD.PushError("PlayerMovement must be a child of a CharacterBody2D.");

    _stats = GameManager.Instance.RunStats;

    // attempt to find a Sprite2D to compute half-width as margin
    // todo: probably rework if sprite is not fully symmetrical in both width and height
    var sprite = _body.GetNodeOrNull<Sprite2D>("Sprite2D");
    if (sprite != null && sprite.Texture != null)
    {
      var textureSize = sprite.Texture.GetSize() * sprite.Scale;
      _clampMargin = Mathf.Max(textureSize.X, textureSize.Y) / 2f;
    }
  }

  /// <summary>
  /// Called every physics tick to handle movement input.
  /// </summary>
  public override void _PhysicsProcess(double delta)
  {
    var input = Vector2.Zero;

    input.X = Input.GetActionStrength("ui_right") - Input.GetActionStrength("ui_left");
    input.Y = Input.GetActionStrength("ui_down") - Input.GetActionStrength("ui_up");

    input = input.Normalized();

    _body.Velocity = input * _stats.GetStat("MoveSpeed");
    _body.MoveAndSlide();

    // clamp the position to arena bounds
    var bounds = GameManager.Instance.ArenaBounds.Bounds;

    var clamped = _body.Position;
    clamped.X = Mathf.Clamp(clamped.X, bounds.Position.X + _clampMargin, bounds.End.X - _clampMargin);
    clamped.Y = Mathf.Clamp(clamped.Y, bounds.Position.Y + _clampMargin, bounds.End.Y - _clampMargin);

    _body.Position = clamped;

  }
}
