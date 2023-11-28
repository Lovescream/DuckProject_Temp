using System;
using System.Collections.Generic;

[Serializable]
public class ItemData {
    public string key;
    public string prefabName;
    public ItemType type;

    public float hpMaxMultiplier;
    public float hpRegen;
    public float healBonusMultiplier;
    public float range;
    public float cooldown;
    public float damage;
    public float damageMultiplier;
    public float defense;
    public float defenseMultiplier;
    public float duration;
    public int projectileCount;
    public int penetrationCount;
    public float speed;
    public float moveSpeedMultiplier;
    public float scaleMultiplier;
    public float cooldownMultiplier;
    public float pickupMultiplier;
    public float expMultiplier;
}

[Serializable]
public class ItemDataLoader:ILoader<string , ItemData> {
    public List<ItemData> items = new();

    public Dictionary<string, ItemData> MakeDictionary() {
        Dictionary<string, ItemData> dictionary = new();
        foreach (ItemData item in items) dictionary.Add(item.key, item);
        return dictionary;
    }
}
