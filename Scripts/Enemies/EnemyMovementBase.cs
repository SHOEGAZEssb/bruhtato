using Godot;

/// <summary>
/// Handles basic movement toward the player target.
/// </summary>
public partial class EnemyMovementBase : Node
{
  [Export] public float MoveSpeed = 150f;

  private CharacterBody2D _body;

  public override void _Ready()
  {
    _body = GetParent<CharacterBody2D>();
  }

  public override void _PhysicsProcess(double delta)
  {
    var direction = (GameManager.Instance.Player.GlobalPosition - _body.GlobalPosition).Normalized();
    _body.Velocity = direction * MoveSpeed;
    _body.MoveAndSlide();
  }
}
