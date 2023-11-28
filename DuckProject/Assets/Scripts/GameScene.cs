using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameScene : MonoBehaviour {

    #region Fields

    private bool resourceLoaded;

    private Boss boss;
    private bool isBossSpawned;

    // Components.
    private EnemySpawner enemySpawner;
    private UI_GameScene UI;

    // Callbacks.
    public Action<int> cbOnWaveStart;
    public Action cbOnWaveEnd; 

    #endregion

    void Awake() {
        SceneChangeEffect_Out effect = Main.Resource.Instantiate("SceneChangeEffect_Out.prefab").GetComponent<SceneChangeEffect_Out>();
        effect.SetInfo(null);
    }

    void Start() {
        if (Main.Resource.Loaded) {
            resourceLoaded = true;
            Main.Local.Initialize();
            Main.Data.Initialize();
            Main.Audio.Initialize();
            Main.Game.Initialize();
            InitializeGame();
        }
        else {
            Main.Resource.LoadAllAsync<UnityEngine.Object>("PreLoad", (key, count, totalCount) => {
                Debug.Log($"[GameScene] Load asset {key} ({count}/{totalCount})");
                if (count >= totalCount) {
                    Main.Resource.Loaded = true;
                    Main.Local.Initialize();
                    Main.Data.Initialize();
                    Main.Audio.Initialize();
                    Main.Game.Initialize();
                    resourceLoaded = true;
                    InitializeGame();
                }
            });
        }
    }

    void Update() {
        Main.Game.RemainTime -= Time.deltaTime;
        Main.Game.PlayTime += Time.deltaTime;
        if (Input.GetKeyDown(KeyCode.E)) {
            Main.Object.Player.Exp += 5;
        }
        else if (Input.GetKeyDown(KeyCode.T)) {
            Main.Object.Spawn<Bomb>("", new(10, 10));
        }
        else if (Input.GetKeyDown(KeyCode.F)) {
            Main.Game.RemainTime = 1.0f;
        }
    }

    private void InitializeGame() {
        // #1. 레벨 정보 초기화.
        if (enemySpawner == null) enemySpawner = this.gameObject.GetOrAddComponent<EnemySpawner>();
        enemySpawner.Initialize();
        Main.Game.InitializeLevel();

        // #2. Map.
        Main.Object.LoadMap($"Map_Stage{Main.Game.CurrentStage.index:D2}.prefab");

        // #3. Player 생성.
        Player player = Main.Object.Spawn<Player>("Player", Vector2.one * 12);
        player.Exp = 0;
        player.KillCount = 0;

        // #4. UI 보여주기.
        Main.UI.ShowSceneUI<UI_Joystick>();
        UI = Main.UI.ShowSceneUI<UI_GameScene>();
        UI.Initialize();
        cbOnWaveStart += UI.OnWaveStart;
        Main.Game.cbOnEndTime += EndWave;

        // #5. 웨이브 시작.
        StartWave();
    }



    private void StartWave() {
        Debug.Log($"StartWave: {Main.Game.CurrentWaveIndex}");
        Main.Game.RemainTime = Main.Game.CurrentWave.time;

        enemySpawner.StartSpawn();

        cbOnWaveStart?.Invoke(Main.Game.CurrentWaveIndex);

    }

    private void EndWave() {
        Debug.Log($"EndWave: {Main.Game.CurrentWaveIndex}");
        enemySpawner.EndSpawn();
        if (Main.Game.CurrentWaveIndex < Main.Game.CurrentStage.waves.Count) {
            Main.Game.CurrentWaveIndex++;
            StartWave();
        }
        else {
            if (!isBossSpawned) {
                Main.Game.RemainTime = 100f;
                SpawnBoss();
            }
            else {
                Main.Game.GameOver();
            }
        }
        cbOnWaveEnd?.Invoke();
    }

    private void SpawnBoss() {
        Vector2 spawnPosition = Main.Game.CurrentMap.GetRandomAroundPosition(Main.Object.Player.transform.position, 10);

        boss = Main.Object.Spawn<Boss>(Main.Game.CurrentStage.bossKey, spawnPosition);
        boss.EnemyInfoUpdate -= UI.OnBossInfoUpdate;
        boss.EnemyInfoUpdate += UI.OnBossInfoUpdate;
        boss.UpdateEnemyData();
        isBossSpawned = true;
    }
}