using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shovel : RepeatItem {
    
    protected override void DoAttack() {
        for (int i = 0; i < ProjectileCount; i++) {
            Vector2 direction = new Vector2(Random.Range(-1f, 1f), Random.Range(-1f, 1f)).normalized;
            Vector2 position = Owner.transform.position;

            Projectile projectile = CreateProjectile(this, Owner, "ShovelProjectile", position);
            projectile.SetRotation(180f);
            projectile.SetVelocity(direction * Data.speed * Owner.ProjectileSpeedMultiplier);
        }
    }
}
