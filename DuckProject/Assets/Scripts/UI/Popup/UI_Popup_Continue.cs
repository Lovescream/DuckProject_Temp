using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UI_Popup_Continue : UI_Popup {

    #region Enums

    enum Texts {
        txtCountDown,
    }
    enum Buttons {
        btnEnd,
        btnContinue,
    }

    #endregion

    #region MonoBehaviours

    void OnEnable() {
        Initialize();
    }

    void Start() {
        StartCoroutine(CoCountDown());
    }

    #endregion

    public override bool Initialize() {
        if (!base.Initialize()) return false;

        BindButton(typeof(Buttons));
        BindText(typeof(Texts));

        GetButton((int)Buttons.btnEnd).gameObject.BindEvent(OnBtnEnd);
        GetButton((int)Buttons.btnContinue).gameObject.BindEvent(OnBtnContinue);

        return true;
    }

    #region OnButtons

    private void OnBtnEnd() {
        Main.UI.ClosePopup(this);
        Main.Game.GameOver();
    }
    private void OnBtnContinue() {
        // TODO:: ±¤°í -> Continue Ã³¸®.
    }

    #endregion

    private IEnumerator CoCountDown() {
        int count = 10;
        while (count > 0) {
            yield return new WaitForSecondsRealtime(1);
            count--;
            GetText((int)Texts.txtCountDown).text = count.ToString();
        }
        yield return new WaitForSecondsRealtime(1);
        OnBtnEnd();
    }
}