using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI_GameScene : UI_Scene {

    #region Enums
    enum Objects {
        Slider,
        BossInfoObject,
        BossHpSliderObject,
        Alarm,
    }
    enum Texts {
        txtWaveValue,
        txtKill,
        txtLevel,
        txtTime,
        txtBossName,
    }
    enum Images {
        imgKillIcon,
        WhiteScreen,
        BossAlarm,
    }
    enum Buttons {
        btnPause,
    }

    #endregion

    #region Fields

    private Player player;

    #endregion

    public override bool Initialize() {
        if (base.Initialize() == false) return false;

        BindObject(typeof(Objects));
        BindButton(typeof(Buttons));
        BindText(typeof(Texts));
        BindImage(typeof(Images));

        GetButton((int)Buttons.btnPause).gameObject.BindEvent(OnBtnPause);

        GetObject((int)Objects.BossInfoObject).gameObject.SetActive(false);
        GetImage((int)Images.BossAlarm).gameObject.SetActive(false);
        
        player = Main.Object.Player;
        player.cbOnPlayerLevelUp += OnPlayerLevelUp;
        player.cbOnPlayerDataUpdated += OnPlayerDataUpdated;
        Main.Game.cbOnSecondChange += OnSecondChange;

        Refresh();

        return true;
    }

    private void Refresh() {
        GetObject((int)Objects.Slider).GetComponent<Slider>().value = player.ExpRatio;
        GetText((int)Texts.txtKill).text = player.KillCount.ToString();
        GetText((int)Texts.txtLevel).text = player.Level.ToString();
    }


    #region Action

    private void OnPlayerLevelUp() {
        List<ItemBase> list = player.Items.RecommendItems();
        if (list.Count > 0) {
            Main.UI.ShowPopupUI<UI_Popup_ItemSelect>();
        }
        GetObject((int)Objects.Slider).GetComponent<Slider>().value = player.ExpRatio;
        GetText((int)Texts.txtLevel).text = player.Level.ToString();
    }
    

    #endregion

    #region OnButtons

    private void OnBtnPause() {
        Main.UI.ShowPopupUI<UI_Popup_Pause>();
    }

    #endregion

    #region Effect

    public void DoEffect_Flash() {
        Sequence sequence = DOTween.Sequence();
        sequence.Append(GetImage((int)Images.WhiteScreen).DOFade(1, 0.1f))
            .Append(GetImage((int)Images.WhiteScreen).DOFade(0, 0.2f));
    }

    #endregion

    #region Event

    public void OnBossInfoUpdate(Enemy enemy) {
        if (enemy.State != CreatureState.Dead) {
            GetObject((int)Objects.BossInfoObject).SetActive(true);
            GetObject((int)Objects.BossHpSliderObject).GetComponent<Slider>().value = enemy.Hp / enemy.HpMax;
            GetText((int)Texts.txtBossName).text = Main.Local.Get("Enemies", enemy.Data.key);
        }
        else {
            GetObject((int)Objects.BossInfoObject).SetActive(false);
        }
    }

    public void OnWaveStart(int wave) {
        GetText((int)Texts.txtWaveValue).text = wave.ToString();
    }
    public void OnSecondChange(int time) {
        if (time == 0) GetText((int)Texts.txtTime).text = "";
        else GetText((int)Texts.txtTime).text = time.ToString();
    }
    private void OnPlayerDataUpdated() {
        GetObject((int)Objects.Slider).GetComponent<Slider>().value = player.ExpRatio;
        GetText((int)Texts.txtKill).text = player.KillCount.ToString();
    }

    #endregion
}

