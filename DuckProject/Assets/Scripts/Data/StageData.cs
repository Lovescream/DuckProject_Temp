using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class StageData {
    public int index;
    public string bossKey;
    public List<WaveData> waves;
}

[Serializable]
public class StageDataLoader : ILoader<int, StageData> {
    public List<StageData> stages = new();

    public Dictionary<int, StageData> MakeDictionary() {
        Dictionary<int, StageData> dictionary = new();
        foreach (StageData stage in stages) dictionary.Add(stage.index, stage);
        return dictionary;
    }
}