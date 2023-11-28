using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoccerBall : RepeatItem {

    protected override void DoAttack() {
        for (int i = 0; i < ProjectileCount; i++) {
            Vector2 direction = new Vector2(Random.Range(-1f, 1f), Random.Range(-1f, 1f)).normalized;
            Vector2 position = Owner.transform.position;

            SoccerBallProjectile ball = CreateProjectile(this, Owner, "SoccerBallProjectile", position) as SoccerBallProjectile;
            ball.SetRotation(180f);
            ball.SetVelocity(direction * Data.speed * Owner.ProjectileSpeedMultiplier);
        }
    }

}