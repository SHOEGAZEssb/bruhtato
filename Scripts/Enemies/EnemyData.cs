using Godot;

[GlobalClass]
public partial class EnemyData : Resource
{
  [Export] public float MaxHP = 5f;
  [Export] public float Damage = 1f;
  [Export] public float MoveSpeed = 100f;
  [Export] public float MoneyDrop = 3f; // also counts as EXP
}