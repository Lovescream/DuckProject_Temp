using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Bow : RepeatItem {

    private IEnumerator Shot() {
        for (int i = 0; i < ProjectileCount; i++) {
            Enemy enemy = FindEnemy();
            Vector2 direction = enemy != null ? enemy.transform.position - Owner.transform.position : Owner.Direction;
            Projectile projectile = CreateProjectile(this, Owner, "ArrowProjectile", Owner.transform.position);
            projectile.SetRotation(Vector2.right, direction.normalized);
            projectile.SetVelocity(direction.normalized * Data.speed * Owner.ProjectileSpeedMultiplier);
            yield return new WaitForSeconds(1f / ProjectileCount);
        }
    }

    protected override void DoAttack() {
        StartCoroutine(Shot());
    }



    private Enemy FindEnemy() {
        Collider2D[] colliders = Physics2D.OverlapCircleAll(Owner.transform.position, Data.range);
        List<Enemy> enemies = new();
        foreach (Collider2D collider in colliders) {
            if (!collider.TryGetComponent<Enemy>(out Enemy enemy)) continue;
            enemies.Add(enemy);
        }
        if (enemies.Count <= 0) return null;
        return enemies.OrderBy(x => Vector2.Distance(x.transform.position, Main.Object.Player.transform.position)).First();
    }

}