using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UI_Popup_Pause : UI_Popup {

    #region Enums

    enum Buttons {
        btnResume,
        btnHome,
        btnSettings,
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

        GetButton((int)Buttons.btnResume).gameObject.BindEvent(OnBtnResume);
        GetButton((int)Buttons.btnHome).gameObject.BindEvent(OnBtnHome);
        GetButton((int)Buttons.btnSettings).gameObject.BindEvent(OnBtnSettings);

        return true;
    }

    #region OnButtons

    private void OnBtnResume() {
        Main.UI.ClosePopup(this);
    }

    private void OnBtnHome() {

    }

    private void OnBtnSettings() {
        Main.UI.ShowPopupUI<UI_Popup_Settings>();
    }

    #endregion
}