using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Knife : RepeatItem {

    protected override void DoAttack() {
        for (int i = 0; i < ProjectileCount; i++) {
            Vector3 direction = Quaternion.AngleAxis(10 * (i - (ProjectileCount - 1) / 2f), Vector3.forward) * (Vector3)Owner.Direction;
            Vector2 position = Owner.transform.position;

            Projectile knife = CreateProjectile(this, Owner, "KnifeProjectile", position);

            knife.transform.rotation = Quaternion.FromToRotation(Vector2.right, direction);
            knife.SetVelocity(direction * Data.speed * Owner.ProjectileSpeedMultiplier);
        }
    }

}