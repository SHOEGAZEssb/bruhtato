using Godot;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bruhtato.Scripts.Player
{
  [GlobalClass]
  public partial class CharacterData : Resource
  {
    // main
    [Export] public float MoveSpeed = 300f;
    [Export] public float Damage = 10f;
    [Export] public float AttackSpeed = 1f;
    [Export] public float HP = 15f;
    [Export] public float Armor = 0f;

    // enemies
    [Export] public float EnemyDamage = 0f;
    [Export] public float EnemyHealth = 0f;
  }
}
