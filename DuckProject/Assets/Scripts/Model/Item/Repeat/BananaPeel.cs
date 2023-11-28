using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BananaPeel : RepeatItem {

    protected override void DoAttack() {
        for (int i = 0; i < ProjectileCount; i++) {
            Vector2 position = Owner.transform.position;
            Vector2 targetPosition = Camera.main.GetComponent<CameraController>().GetRandomPositionInCamera();

            BananaPeelProjectile peel = CreateProjectile(this, Owner, "BananaPeelProjectile", position) as BananaPeelProjectile;
            peel.SetRotation(180f);
            peel.Play(targetPosition);
        }
    }
}