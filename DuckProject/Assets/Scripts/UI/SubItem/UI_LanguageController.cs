using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UI_LanguageController : UI_Base {

    #region Enums

    enum Buttons {
        btnNext,
        btnPrev
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

        GetButton((int)Buttons.btnNext).gameObject.BindEvent(OnBtnNext);
        GetButton((int)Buttons.btnPrev).gameObject.BindEvent(OnBtnPrev);

        return true;
    }

    private void OnBtnNext() {
        Main.Local.ChangeNextLanguage();
    }
    private void OnBtnPrev() {
        Main.Local.ChangePrevLanguage();
    }


}