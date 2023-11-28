using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI_HPBar : UI_Base {

    #region Enums

    enum GameObjects {
        HPBar,
    }

    #endregion

    #region Fields

    private Creature owner;
    private Slider slider;

    #endregion


    public override bool Initialize() {
        if (!base.Initialize()) return false;

        BindObject(typeof(GameObjects));
        slider = GetObject((int)GameObjects.HPBar).GetComponent<Slider>();
        owner = this.transform.parent.GetComponent<Creature>();

        return true;
    }

    void Update() {
        slider.value = Mathf.Clamp01(owner.Hp / owner.HpMax);
    }

}