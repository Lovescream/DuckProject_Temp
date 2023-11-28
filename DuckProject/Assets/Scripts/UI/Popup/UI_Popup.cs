using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UI_Popup : UI_Base {

    public override bool Initialize() {
        if (base.Initialize() == false) return false;

        Main.UI.SetCanvas(this.gameObject, true);

        return true;
    }

    public virtual void ClosePopupUI() {
        Main.UI.ClosePopup(this);
    }

}