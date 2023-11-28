using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI_Scene : UI_Base {

    public override bool Initialize() {
        if (base.Initialize() == false) return false;

        Main.UI.SetCanvas(this.gameObject, false);

        return true;
    }

}