using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Watermelon : RepeatItem {

    private IEnumerator Shot() {
        for (int i = 0; i < ProjectileCount; i++) {
            Vector2 targetPosition = Camera.main.GetComponent<CameraController>().GetRandomPositionInCamera();
            Vector2 spawnPosition = GetSpawnPosition(targetPosition);

            WatermelonProjectile watermelon = CreateProjectile(this, Owner, "WatermelonProjectile", spawnPosition) as WatermelonProjectile;

            watermelon.Play(targetPosition);
            yield return new WaitForSeconds(1f / ProjectileCount);
        }
    }

    protected override void DoAttack() {
        StartCoroutine(Shot());
    }

    private Vector2 GetSpawnPosition(Vector2 targetPosition) {
        float halfHeight = Camera.main.orthographicSize;
        float halfWidth = halfHeight * Camera.main.aspect;

        float x = targetPosition.x + (halfWidth + 1) * Mathf.Cos(60 * Mathf.Deg2Rad);
        float y = targetPosition.y + (halfHeight + 1) * Mathf.Sin(60 * Mathf.Deg2Rad);

        return new(x, y);
    }

}