using Newtonsoft.Json;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class DataManager {

    public Dictionary<string, CreatureData> Creatures = new();
    public Dictionary<int, StageData> Stages = new();
    public Dictionary<string, ItemData> Items = new();
    public Dictionary<int, LevelData> Levels = new();

    public void Initialize() {
        Creatures = LoadJson<CreatureDataLoader, string, CreatureData>("CreatureData").MakeDictionary();
        Stages = LoadJson<StageDataLoader, int, StageData>("StageData").MakeDictionary();
        Items = LoadJson<ItemDataLoader, string, ItemData>("ItemData").MakeDictionary();
        Levels = LoadJson<LevelDataLoader, int, LevelData>("LevelData").MakeDictionary();
    }

    public int GetMaxLevel(ItemName type) {
        return Items.Values.Where(s => s.key.Split('_')[0] == type.ToString()).Count();
    }

    Loader LoadJson<Loader, Key, Value>(string path) where Loader:ILoader<Key, Value> {
        TextAsset textAsset = Main.Resource.Load<TextAsset>(path);
        return JsonConvert.DeserializeObject<Loader>(textAsset.text);
    }

}

public interface ILoader<Key, Value> {
    Dictionary<Key, Value> MakeDictionary();
}