using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SceneChangeEffect_In_Sub : UI_Base {

    public void Play() {
        this.GetComponent<RectTransform>().DOAnchorPosX(0, 1f);
    }

}