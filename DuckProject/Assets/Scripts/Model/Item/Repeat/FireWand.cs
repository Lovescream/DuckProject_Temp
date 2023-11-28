using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireWand : RepeatItem {
    private IEnumerator Shot() {
        for (int i = 0; i < ProjectileCount; i++) {
            Vector2 direction = Owner.Direction;
            Vector2 position = Owner.transform.position;

            FireboltProjectile bolt = CreateProjectile(this, Owner, "FireboltProjectile", position) as FireboltProjectile;
            bolt.SetVelocity(direction * Data.speed * Owner.ProjectileSpeedMultiplier);

            yield return new WaitForSeconds(1f / ProjectileCount);
        }
    }

    protected override void DoAttack() {
        StartCoroutine(Shot());
    }
}