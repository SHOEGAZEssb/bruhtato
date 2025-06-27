using Godot;

public partial class Arena : Node2D
{
  [Export] public NodePath WaveSpawnerPath;
  [Export] public NodePath ShopUIPath;

  private WaveSpawner _waveSpawner;
  private ShopUI _shop;

  public override void _Ready()
  {
    _waveSpawner = GetNode<WaveSpawner>(WaveSpawnerPath);
    _shop = GetNode<ShopUI>(ShopUIPath);

    _waveSpawner.WaveEnded += OnWaveEnded;
    _shop.ShopClosed += OnShopClosed;
  }

  private void OnWaveEnded()
  {
    _shop.OpenShop();
  }

  private void OnShopClosed()
  {
    _waveSpawner.StartWave(_waveSpawner.CurrentWave + 1);
  }
}
