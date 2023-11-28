using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour {

    private Coroutine coUpdate;

    private bool initialized;

    public void Initialize() {
        if (initialized) return;

        Main.Game.cbOnEndTime += SpawnEliteEnemy;
    }

    public void StartSpawn() {
        if (coUpdate == null) coUpdate = StartCoroutine(CoUpdate());
    }

    public void EndSpawn() {
        if (coUpdate != null) StopCoroutine(coUpdate);
        coUpdate = null;
    }

    IEnumerator CoUpdate() {
        yield return new WaitUntil(() => Main.Game.CurrentMap != null);
        while (true) {
            WaveData wave = Main.Game.CurrentWave;

            if (wave.enemyKey.Count == 1) {
                for (int i = 0; i < wave.onceSpawnCount; i++) {
                    Vector2 spawnPosition = wave.spawnPositionType switch {
                        SpawnPositionType.EdgeOfMap => Main.Game.CurrentMap.GetRandomEdgePosition(wave.spawnDistance),
                        SpawnPositionType.Around => Main.Game.CurrentMap.GetRandomAroundPosition(Main.Object.Player.transform.position, wave.spawnDistance),
                        _ => Vector2.zero
                    };
                    Main.Object.Spawn<Enemy>(wave.enemyKey[0], spawnPosition);
                }
            }
            else {
                for (int i = 0; i < wave.onceSpawnCount; i++) {
                    Vector2 spawnPosition = wave.spawnPositionType switch {
                        SpawnPositionType.EdgeOfMap => Main.Game.CurrentMap.GetRandomEdgePosition(wave.spawnDistance),
                        SpawnPositionType.Around => Main.Game.CurrentMap.GetRandomAroundPosition(Main.Object.Player.transform.position, wave.spawnDistance),
                        _ => Vector2.zero
                    };
                    string key = wave.enemyKey[Random.Range(0, wave.enemyKey.Count)];
                    Main.Object.Spawn<Enemy>(key, spawnPosition);
                }
            }
            yield return new WaitForSeconds(wave.spawnInterval);
        }
    }

    private void SpawnEliteEnemy() {
        WaveData wave = Main.Game.CurrentWave;
        string eliteKey = wave.eliteKey;
        if (string.IsNullOrEmpty(eliteKey)) return;

        Vector2 spawnPosition = wave.spawnPositionType switch {
            SpawnPositionType.EdgeOfMap => Main.Game.CurrentMap.GetRandomEdgePosition(wave.spawnDistance),
            SpawnPositionType.Around => Main.Game.CurrentMap.GetRandomAroundPosition(Main.Object.Player.transform.position, wave.spawnDistance),
            _ => Vector2.zero
        };

        Main.Object.Spawn<EliteEnemy>(eliteKey, spawnPosition);
    }
}



public enum SpawnPositionType {
    EdgeOfMap,
    Around,
}