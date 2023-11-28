using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class UI_Popup_SelectStage : UI_Popup {

    #region Enums

    enum Texts {
        txtStageIndex,
        txtStageName,
        txtSurvivedWave,
        txtSurvivedTime,
    }
    enum Images {
        imgStage,
    }
    enum Buttons {
        btnNextStage,
        btnPrevStage,
        btnStart,
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
        BindImage(typeof(Images));

        GetButton((int)Buttons.btnNextStage).gameObject.BindEvent(OnBtnNextStage);
        GetButton((int)Buttons.btnPrevStage).gameObject.BindEvent(OnBtnPrevStage);
        GetButton((int)Buttons.btnStart).gameObject.BindEvent(OnBtnStart);

        Refresh();

        return true;
    }

    private void Refresh() {
        if (Main.Game.CurrentStage == null) Main.Game.SetStage(1);
        int index = Main.Game.CurrentStage.index;

        GetText((int)Texts.txtStageIndex).text = $"Stage {index}";
        GetText((int)Texts.txtStageName).text = Main.Local.Get("Stage", $"Stage_{index:D2}_Name");

        StageClearInfo info = Main.Game.StageClearInfo[index];
        int survivedWave = info.wave;
        float survivedTime = info.time;
        int min = (int)(survivedTime / 60);
        int sec = (int)(survivedTime - 60 * min);

        GetText((int)Texts.txtSurvivedWave).text = $"{Main.Local.Get("UI", "txtSurvivedWave")}{survivedWave}";
        GetText((int)Texts.txtSurvivedTime).text = $"{Main.Local.Get("UI", "txtSurvivedTime")}{min:D2}{Main.Local.Get("UI", "txtMin")}{sec:D2}{Main.Local.Get("UI", "txtSec")}";

        GetImage((int)Images.imgStage).sprite = Main.Resource.Load<Sprite>($"Stage{index:D2}.sprite");
    }


    #region OnButtons

    private void OnBtnNextStage() {
        Main.Game.SetNextStage();
    }
    private void OnBtnPrevStage() {
        Main.Game.SetPrevStage();
    }
    private void OnBtnStart() {
        Main.Scene.LoadScene("GameScene");
    }

    #endregion

    


}