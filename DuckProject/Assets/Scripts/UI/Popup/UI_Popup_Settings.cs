using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UI_Popup_Settings : UI_Popup {

    #region Enums

    enum GameObjects {
        LanguageController,
        BGMController,
        SFXController,
    }

    enum Buttons {
        Background,
    }

    #endregion

    #region MonoBehaviours

    void OnEnable() {
        Initialize();
    }

    #endregion

    public override bool Initialize() {
        if (!base.Initialize()) return false;

        BindObject(typeof(GameObjects));
        BindButton(typeof(Buttons));

        GetButton((int)Buttons.Background).gameObject.BindEvent(OnBtnBackground);

        return true;
    }

    #region OnButtons

    private void OnBtnBackground() {
        Main.UI.ClosePopup(this);
    }

    #endregion
}