using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PoisonBottle : RepeatItem {

    protected override void DoAttack() {
        PoisonFieldProjectile field = CreateProjectile(this, Owner, "PoisonFieldProjectile", Owner.transform.position) as PoisonFieldProjectile;
        StartCoroutine(field.Play());
    }

}