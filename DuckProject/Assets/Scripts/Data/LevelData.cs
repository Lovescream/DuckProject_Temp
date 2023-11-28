using System;
using System.Collections.Generic;

[Serializable]
public class LevelData {
    public int level;
    public int totalExp;
}

[Serializable]
public class LevelDataLoader : ILoader<int, LevelData> {
    public List<LevelData> levels = new();
    public Dictionary<int, LevelData> MakeDictionary() {
        Dictionary<int, LevelData> dictionary = new();
        foreach (LevelData level in levels)
            dictionary.Add(level.level, level);
        return dictionary;
    }
}