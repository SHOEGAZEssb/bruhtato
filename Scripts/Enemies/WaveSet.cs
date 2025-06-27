using Godot;

/// <summary>
/// Holds a sequence of WaveData for a full run.
/// </summary>
[GlobalClass]
public partial class WaveSet : Resource
{
  [Export] public WaveData[] Waves;
}
