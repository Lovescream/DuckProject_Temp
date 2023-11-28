using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LobbyScene : MonoBehaviour {

    private bool resourceLoaded;

    private UI_LobbyScene UI;

    void Start() {
        if (Main.Resource.Loaded) {
            resourceLoaded = true;
            Main.Local.Initialize();
            Main.Data.Initialize();
            Main.Audio.Initialize();
            Main.Game.Initialize();
            resourceLoaded = true;
            InitializeLobby();
        }
        else {
            Main.Resource.LoadAllAsync<UnityEngine.Object>("PreLoad", (key, count, totalCount) => {
                Debug.Log($"[GameScene] Load asset {key} ({count}/{totalCount})");
                if (count >= totalCount) {
                    Main.Resource.Loaded = true;
                    resourceLoaded = true;
                    Main.Local.Initialize();
                    Main.Data.Initialize();
                    Main.Audio.Initialize();
                    Main.Game.Initialize();
                    resourceLoaded = true;
                    InitializeLobby();
                }
            });
        }
    }

    void Update() {
        if (Input.GetKeyDown(KeyCode.Space)) {
            SceneManager.LoadScene("GameScene");
        }
    }

    private void InitializeLobby() {
        UI = Main.UI.ShowSceneUI<UI_LobbyScene>();
        Main.UI.ShowPopupUI<UI_Popup_SelectStage>();
    }
}