using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Stick : RepeatItem {

    private IEnumerator Shot() {
        Vector2 direction = Owner.Direction;
        Vector2 position = Owner.transform.position;

        Projectile stick = CreateProjectile(this, Owner, "StickProjectile", position);

        float rotation = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        stick.transform.rotation = Quaternion.Euler(0, 0, rotation - 90);
        yield return new WaitForSeconds(0.125f);
        if (stick.IsValid()) Main.Object.Despawn(stick);
    }

    protected override void DoAttack() {
        StartCoroutine(Shot());
    }
}