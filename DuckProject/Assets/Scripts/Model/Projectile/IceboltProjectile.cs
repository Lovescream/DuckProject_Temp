using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IceboltProjectile : Projectile {

    protected override void OnTriggerEnter2D(Collider2D collision) {
        Creature creature = collision.GetComponent<Creature>();
        if (!creature.IsValid() || !this.IsValid()) return;
        if (creature == Owner) return;
        creature.SlowTimer += 1f;
        if (creature.SlowTimer >= 3f) creature.FrozenTimer += 1f;
        base.OnTriggerEnter2D(collision);
    }

}
