using Godot;
using System;

/// <summary>
/// Defines configuration for a single enemy wave.
/// </summary>
[GlobalClass]
public partial class WaveData : Resource
{
  [Export] public PackedScene[] EnemyTypes;
  [Export] public int EnemyCount = 10; // optional cap, or unused
  [Export] public float TimeBetweenSpawns = 0.5f;
  [Export] public float Duration = 20f; // new
}

