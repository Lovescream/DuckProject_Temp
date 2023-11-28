using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EliteEnemy : Enemy {

    public override void OnStateEntered_Dead() {
        base.OnStateEntered_Dead();

        Main.Object.Spawn<TreasureChest>("", this.transform.position);
    }

}