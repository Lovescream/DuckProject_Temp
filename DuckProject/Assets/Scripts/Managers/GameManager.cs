using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class GameManager {

    #region Properties
    
    // Data
    public Dictionary<int, StageClearInfo> StageClearInfo {
        get => data.clearInfo;
        set {
            data.clearInfo = value;
            SaveGame();
        }
    }

    // Map
    public Map CurrentMap { get; set; }

    // Stage / Wave
    public StageData CurrentStage { get; private set; }
    public WaveData CurrentWave => CurrentStage.waves[CurrentWaveIndex - 1];
    public int CurrentWaveIndex { get; set; }

    // Time
    public float RemainTime {
        get => remainTime;
        set {
            remainTime = value;
            RemainSecond = (int)RemainTime;
            if (RemainTime < 0) {
                cbOnEndTime?.Invoke();
            }
        }
    }
    public int RemainSecond {
        get => remainSecond;
        private set {
            if (remainSecond != value) {
                remainSecond = value;
                cbOnSecondChange?.Invoke(remainSecond);
            }
        }
    }
    public float PlayTime { get; set; }

    #endregion

    #region Fields

    // GameData.
    private GameData data = new();
    private string path;

    // Time.
    private float remainTime;
    private int remainSecond;

    // Callbacks.
    public Action<int> cbOnSecondChange;
    public Action cbOnEndTime;

    #endregion

    public void Initialize() {
        path = Application.persistentDataPath + "/SaveData.json";
        if (LoadGame()) return;

        // ============== 로드 실패, 초기화 ==============

        foreach (StageData stage in Main.Data.Stages.Values) {
            StageClearInfo info = new() {
                index = stage.index,
                wave = 0,
                kill = 0,
                time = 0,
                isClear = false,
            };
            data.clearInfo.Add(stage.index, info);
        }

        // ===============================================
        SaveGame();
    }

    public void InitializeLevel() {
        CurrentStage = Main.Data.Stages[1];
        CurrentWaveIndex = 1;
    }

    public void GameOver() {
        Main.UI.ShowPopupUI<UI_Popup_GameOver>().SetInfo();
    }

    #region Stage
    
    public void SetStage(int index) {
        if (index < 0 || index > Main.Data.Stages.Count) return;
        CurrentStage = Main.Data.Stages[index];
    }

    public void SetPrevStage() {
        if (CurrentStage.index <= 1) return;
        CurrentStage = Main.Data.Stages[CurrentStage.index - 2];
    }

    public void SetNextStage() {
        if (CurrentStage.index >= Main.Data.Stages.Count) return;
        CurrentStage = Main.Data.Stages[CurrentStage.index + 0];
    }

    #endregion

    #region Save / Load

    public void SaveGame() {
        string jsonStr = JsonConvert.SerializeObject(data);
        File.WriteAllText(path, jsonStr);
    }

    public bool LoadGame() {
        if (!File.Exists(path)) return false;

        string file = File.ReadAllText(path);
        GameData data = JsonConvert.DeserializeObject<GameData>(file);
        if (data != null) this.data = data;

        return true;
    }

    #endregion

}