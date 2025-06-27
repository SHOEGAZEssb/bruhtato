using Godot;
using System;
using System.Collections.Generic;

public partial class Player : CharacterBody2D
{
  [Export] public NodePath PlayerStatusPath;
  [Export] public NodePath HealthComponentPath;
  [Export] public NodePath ExpMoneyComponentPath;
  [Export] public NodePath InventoryComponentPath;
  [Export] public NodePath WeaponManagerPath;

  [Export] public PackedScene WeaponScene;
  [Export] public WeaponData[] StartingWeapons;
  private WeaponManager _weaponManager;

  public HealthComponent HealthComponent;
  public ExpMoneyComponent ExpMoneyComponent;
  public InventoryComponent InventoryComponent;

  public override void _Ready()
  {
    GameManager.Instance.Player = this;

    HealthComponent = GetNode<HealthComponent>(HealthComponentPath);
    HealthComponent.Initialize(GameManager.Instance.RunStats.GetStat("HP"));

    ExpMoneyComponent = GetNode<ExpMoneyComponent>(ExpMoneyComponentPath);
    InventoryComponent = GetNode<InventoryComponent>(InventoryComponentPath);

    // Find the PlayerStatus UI and connect to it
    var ui = GetNode<PlayerStatus>(PlayerStatusPath);
    HealthComponent.HealthChanged += ui.SetHP;
    ExpMoneyComponent.ExpChanged += ui.SetEXP;
    ExpMoneyComponent.MoneyChanged += ui.SetMoney;

    // Set initial value
    ui.SetHP(HealthComponent.CurrentHP, HealthComponent.MaxHP);
    ui.SetMoney(ExpMoneyComponent.CurrentMoney);

    // weapons

    _weaponManager = GetNode<WeaponManager>("WeaponManager");

    var weaponList = new List<Weapon>();

    foreach (var data in StartingWeapons)
    {
      var weapon = WeaponScene.Instantiate<Weapon>();
      weapon.Data = data;
      //weapon.ProjectileScene = GD.Load<PackedScene>("res://Weapons/Projectile.tscn"); // assign projectile scene
      AddChild(weapon); // weapon must be a child of player or WeaponManager

      weaponList.Add(weapon);
    }

    _weaponManager.SetWeapons(weaponList);
  }
}
