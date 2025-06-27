using Godot;

/// <summary>
/// Defines base stats for a weapon, used to instantiate and scale behavior.
/// </summary>
[GlobalClass]
public partial class WeaponData : Resource
{
  [Export] public string WeaponName;
  [Export] public int Level = 1;
  [Export] public float BaseDamage = 5f;
  [Export] public float FireRate = 1f; // shots per second
  [Export] public float Range = 250f;  // targeting or projectile range
  [Export] public int Penetrate = 1;
  [Export] public Texture2D Sprite;
}
