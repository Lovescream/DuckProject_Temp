using Newtonsoft.Json;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using UnityEditor;
using UnityEngine;

public class DataTransformer : EditorWindow {
#if UNITY_EDITOR

    [MenuItem("Tools/ParseExcel")]
    public static void ParseExcel() {
        ParseCreatureData("Creature");
        ParseStageData("Stage");
        ParseItemData("Item");
        ParseLevelData("Level");
    }

    private static void ParseCreatureData(string fileName) {
        CreatureDataLoader loader = new();

        string[] lines = File.ReadAllText($"{Application.dataPath}/@Resources/Data/Excel/{fileName}Data.csv").Split("\n");

        for (int y=1;y<lines.Length;y++) {
            string[] row = lines[y].Replace("\r", "").Split(',');
            if (row.Length == 0 || string.IsNullOrEmpty(row[0])) continue;

            loader.creatures.Add(new() {
                key = row[0],
                prefabName = row[1],
                hpMax = ConvertValue<float>(row[2]),
                hpMaxIncresement = ConvertValue<float>(row[3]),
                hpRegen = ConvertValue<float>(row[4]),
                damage = ConvertValue<float>(row[5]),
                damageIncresement = ConvertValue<float>(row[6]),
                defense = ConvertValue<float>(row[7]),
                moveSpeed = ConvertValue<float>(row[8]),
                exp = ConvertValue<int>(row[9]),
                itemTypeList = ConvertList<string>(row[10]),
            });
        }

        string jsonStr = JsonConvert.SerializeObject(loader, Formatting.Indented);
        File.WriteAllText($"{Application.dataPath}/@Resources/Data/JsonData/{fileName}Data.json", jsonStr);
        AssetDatabase.Refresh();
    }
    private static Dictionary<int, List<WaveData>> ParseWaveData(string fileName) {
        Dictionary<int, List<WaveData>> waves = new();

        string[] lines = File.ReadAllText($"{Application.dataPath}/@Resources/Data/Excel/{fileName}Data.csv").Split("\n");

        for (int y = 1; y < lines.Length; y++) {
            string[] row = lines[y].Replace("\r", "").Split(',');
            if (row.Length == 0 || string.IsNullOrEmpty(row[0])) continue;

            WaveData waveData = new() {
                stage = ConvertValue<int>(row[0]),
                index = ConvertValue<int>(row[1]),
                spawnInterval = ConvertValue<float>(row[2]),
                onceSpawnCount = ConvertValue<int>(row[3]),
                time = ConvertValue<float>(row[4]),
                enemyKey = ConvertList<string>(row[5]),
                eliteKey = ConvertValue<string>(row[6]),
                spawnPositionType = ConvertValue<SpawnPositionType>(row[7]),
                spawnDistance = ConvertValue<float>(row[8]),
                gemRatio = ConvertValue<float>(row[9]),
            };

            if (waves.ContainsKey(waveData.stage) == false) waves.Add(waveData.stage, new());
            waves[waveData.stage].Add(waveData);
        }
        return waves;
    }
    private static void ParseStageData(string fileName) {
        Dictionary<int, List<WaveData>> waves = ParseWaveData("Wave");
        StageDataLoader loader = new();

        string[] lines = File.ReadAllText($"{Application.dataPath}/@Resources/Data/Excel/{fileName}Data.csv").Split("\n");

        for (int y = 1; y < lines.Length; y++) {
            string[] row = lines[y].Replace("\r", "").Split(',');
            if (row.Length == 0 || string.IsNullOrEmpty(row[0])) continue;

            StageData stageData = new() {
                index = ConvertValue<int>(row[0]),
                bossKey = ConvertValue<string>(row[1]),
            };
            waves.TryGetValue(stageData.index, out stageData.waves);
            loader.stages.Add(stageData);
        }

        string jsonStr = JsonConvert.SerializeObject(loader, Formatting.Indented);
        File.WriteAllText($"{Application.dataPath}/@Resources/Data/JsonData/{fileName}Data.json", jsonStr);
        AssetDatabase.Refresh();
    }
    private static void ParseItemData(string fileName) {
        ItemDataLoader loader = new();

        string[] lines = File.ReadAllText($"{Application.dataPath}/@Resources/Data/Excel/{fileName}Data.csv").Split("\n");

        for (int y = 1; y < lines.Length; y++) {
            string[] row = lines[y].Replace("\r", "").Split(',');
            if (row.Length == 0 || string.IsNullOrEmpty(row[0])) continue;

            loader.items.Add(new() {
                key = row[0],
                prefabName = row[1],
                type = ConvertValue<ItemType>(row[2]),

                hpMaxMultiplier = ConvertValue<float>(row[3]),
                hpRegen = ConvertValue<float>(row[4]),
                healBonusMultiplier = ConvertValue<float>(row[5]),
                range = ConvertValue<float>(row[6]),
                cooldown = ConvertValue<float>(row[7]),
                damage = ConvertValue<float>(row[8]),
                damageMultiplier = ConvertValue<float>(row[9]),
                defense = ConvertValue<float>(row[10]),
                defenseMultiplier = ConvertValue<float>(row[11]),
                duration = ConvertValue<float>(row[12]),
                projectileCount = ConvertValue<int>(row[13]),
                penetrationCount = ConvertValue<int>(row[14]),
                speed = ConvertValue<float>(row[15]),
                moveSpeedMultiplier = ConvertValue<float>(row[16]),
                scaleMultiplier = ConvertValue<float>(row[17]),
                cooldownMultiplier = ConvertValue<float>(row[18]),
                pickupMultiplier = ConvertValue<float>(row[19]),
                expMultiplier = ConvertValue<float>(row[20]),
            });
        }

        string jsonStr = JsonConvert.SerializeObject(loader, Formatting.Indented);
        File.WriteAllText($"{Application.dataPath}/@Resources/Data/JsonData/{fileName}Data.json", jsonStr);
        AssetDatabase.Refresh();
    }
    private static void ParseLevelData(string fileName) {
        LevelDataLoader loader = new();

        string[] lines = File.ReadAllText($"{Application.dataPath}/@Resources/Data/Excel/{fileName}Data.csv").Split("\n");

        for (int y = 1; y < lines.Length; y++) {
            string[] row = lines[y].Replace("\r", "").Split(',');
            if (row.Length == 0 || string.IsNullOrEmpty(row[0])) continue;

            loader.levels.Add(new() {
                level = ConvertValue<int>(row[0]),
                totalExp = ConvertValue<int>(row[1]),
            });
        }

        string jsonStr = JsonConvert.SerializeObject(loader, Formatting.Indented);
        File.WriteAllText($"{Application.dataPath}/@Resources/Data/JsonData/{fileName}Data.json", jsonStr);
        AssetDatabase.Refresh();
    }


    private static T ConvertValue<T>(string value) {
        if (string.IsNullOrEmpty(value)) return default;
        TypeConverter converter = TypeDescriptor.GetConverter(typeof(T));
        return (T)converter.ConvertFromString(value);
    }

    private static List<T> ConvertList<T>(string value) {
        if (string.IsNullOrEmpty(value)) return new();
        return value.Split('|').Select(x => ConvertValue<T>(x)).ToList();
    }

#endif
}