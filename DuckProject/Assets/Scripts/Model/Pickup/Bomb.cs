using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Bomb : Pickup {

    public override bool Initialize() {
        base.Initialize();
        Type = PickupType.Bomb;
        return true;
    }

    public override void OnGetPickup() {
        base.OnGetPickup();

        (Main.UI.SceneUI as UI_GameScene)?.DoEffect_Flash();
        foreach (Enemy enemy in Main.Object.Enemies.ToList()) {
            enemy.State = CreatureState.Dead;
        }

        Main.Object.Despawn(this);
    }
}