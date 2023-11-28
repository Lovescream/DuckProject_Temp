using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PigeonLauncher : RepeatItem {

    private IEnumerator Shot() {
        for (int i = 0; i < ProjectileCount; i++) {
            Vector2 direction = new Vector2(Random.Range(-1f, 1f), Random.Range(-1f, 1f)).normalized;
            Vector2 position = Owner.transform.position;

            PigeonProjectile pigeon = CreateProjectile(this, Owner, "PigeonProjectile", position) as PigeonProjectile;
            pigeon.SetRotation(Vector2.up, direction);
            pigeon.SetVelocity(direction * Data.speed * Owner.ProjectileSpeedMultiplier);

            yield return new WaitForSeconds(1f / ProjectileCount);
        }
    }

    protected override void DoAttack() {
        StartCoroutine(Shot());
    }

}