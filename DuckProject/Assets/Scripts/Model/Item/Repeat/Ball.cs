using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ball : RepeatItem {

    protected override float Interval => Data.cooldown + Data.duration * Owner.ProjectileScaleMultiplier;

    protected override void DoAttack() {
        Balls balls = Main.Object.Spawn<Balls>("", this.transform.position);
        balls.transform.SetParent(Owner.transform);
        balls.SetInfo(Owner, this);
    }
}