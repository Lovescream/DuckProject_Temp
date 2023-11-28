using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class CreatureData {
    public string key;
    public string prefabName;
    public float hpMax;
    public float hpMaxIncresement;
    public float hpRegen;
    public float damage;
    public float damageIncresement;
    public float defense;
    public float moveSpeed;
    public int exp;
    public List<string> itemTypeList;
}

[Serializable]
public class CreatureDataLoader : ILoader<string, CreatureData> {
    public List<CreatureData> creatures = new();
    public Dictionary<string, CreatureData> MakeDictionary() {
        Dictionary<string, CreatureData> dictionary = new();
        foreach (CreatureData creature in creatures)
            dictionary.Add(creature.key, creature);
        return dictionary;
    }
}