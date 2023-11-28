using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaterGun : RepeatItem {

    private IEnumerator Shot() {
        for (int i=0;i<ProjectileCount;i++) {
            Vector2 direction = Owner.Direction;
            Vector2 position = Owner.transform.position;

            Projectile projectile = CreateProjectile(this, Owner, "WaterGunProjectile", position);
            projectile.SetRotation(Vector2.right, direction.normalized);
            projectile.SetVelocity(direction.normalized * Data.speed * Owner.ProjectileSpeedMultiplier);

            yield return new WaitForSeconds(2f / ProjectileCount);
        }
    }

    protected override void DoAttack() {
        StartCoroutine(Shot());
    }

}