using Godot;
using System.Collections.Generic;

public partial class WeaponManager : Node2D
{
  [Export] public float CircleRadius = 40f;
  private List<Weapon> _weapons = new();

  public void SetWeapons(List<Weapon> weapons)
  {
    _weapons = weapons;

    for (int i = 0; i < _weapons.Count; i++)
    {
      var angle = i * (Mathf.Tau / _weapons.Count);
      var offset = new Vector2(Mathf.Cos(angle), Mathf.Sin(angle)) * CircleRadius;

      _weapons[i].Position = offset;
    }
  }
}
