using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class WaveData {
    public int stage;
    public int index;
    public float spawnInterval;
    public int onceSpawnCount;
    public float time;
    public List<string> enemyKey;
    public string eliteKey;
    public SpawnPositionType spawnPositionType;
    public float spawnDistance;
    public float gemRatio;
}

[Serializable]
public class WaveDataLoader:ILoader<int, WaveData> {
    public List<WaveData> waves = new();

    public Dictionary<int, WaveData> MakeDictionary() {
        Dictionary<int, WaveData> dictionary = new();
        foreach (WaveData wave in waves) dictionary.Add(wave.index, wave);
        return dictionary;
    }
}