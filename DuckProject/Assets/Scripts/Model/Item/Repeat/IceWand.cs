using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IceWand : RepeatItem {
    private IEnumerator Shot() {
        for (int i = 0; i < ProjectileCount; i++) {
            Vector2 direction = Owner.Direction;
            Vector2 position = Owner.transform.position;

            IceboltProjectile bolt = CreateProjectile(this, Owner, "IceboltProjectile", position) as IceboltProjectile;
            bolt.SetVelocity(direction * Data.speed * Owner.ProjectileSpeedMultiplier);

            yield return new WaitForSeconds(1f / ProjectileCount);
        }
    }

    protected override void DoAttack() {
        StartCoroutine(Shot());
    }

}