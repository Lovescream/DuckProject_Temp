using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UI_Popup_GameOver : UI_Popup {

    #region Enums

    enum Texts {
        txtStage,
        txtWave,
        txtTime,
        txtKill,
    }
    enum Buttons {
        Background
    }

    #endregion

    #region MonoBehaviours

    void OnEnable() {
        Initialize();
    }

    #endregion

    public override bool Initialize() {
        if (!base.Initialize()) return false;

        BindButton(typeof(Buttons));
        BindText(typeof(Texts));

        GetButton((int)Buttons.Background).gameObject.BindEvent(OnBtnBackground);

        return true;
    }

    public void SetInfo() {
        float time = Main.Game.PlayTime;
        GetText((int)Texts.txtStage).text = $"Stage {Main.Game.CurrentStage.index}";
        GetText((int)Texts.txtTime).text = $"{(int)(time / 60)}:{(int)(time % 60):D2}";
        GetText((int)Texts.txtWave).text = $"{Main.Game.CurrentWaveIndex - 1}";
        GetText((int)Texts.txtKill).text = $"{Main.Object.Player.KillCount}";
    }

    private void OnBtnBackground() {
        if (Main.Game.StageClearInfo.TryGetValue(Main.Game.CurrentStage.index, out StageClearInfo info)) {
            if (Main.Game.CurrentWaveIndex - 1 > info.wave) {
                info.wave = Main.Game.CurrentWaveIndex;
                Main.Game.StageClearInfo[Main.Game.CurrentStage.index] = info;
            }
            if (Main.Game.PlayTime > info.time) {
                info.time = Main.Game.PlayTime;
                Main.Game.StageClearInfo[Main.Game.CurrentStage.index] = info;
            }
            if (Main.Object.Player.KillCount > info.kill) {
                info.kill = Main.Object.Player.KillCount;
                Main.Game.StageClearInfo[Main.Game.CurrentStage.index] = info;
            }
        }

        Main.Game.SaveGame();
        Main.UI.ClosePopup(this);
        Main.Scene.LoadScene("LobbyScene");
    }
}