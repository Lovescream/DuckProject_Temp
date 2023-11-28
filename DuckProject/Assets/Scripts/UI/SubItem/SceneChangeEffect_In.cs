using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SceneChangeEffect_In : UI_Popup {

    private List<SceneChangeEffect_In_Sub> list = new();

    private Action cb;

    public override bool Initialize() {
        if (!base.Initialize()) return false;

        float height = (float)Screen.height / Screen.width * 1080f;

        float y = height / 2f;
        while (y > -height / 2f) {
            GameObject obj = Main.Resource.Instantiate("SceneChangeEffect_In_Sub.prefab", this.transform, false);
            RectTransform rect = obj.GetComponent<RectTransform>();
            Vector2 position = rect.anchoredPosition;
            position.y = y;
            rect.anchoredPosition = position;
            y -= 100f;
            list.Add(obj.GetComponent<SceneChangeEffect_In_Sub>());
        }

        return true;
    }

    public void SetInfo(Action callback) {
        this.cb = callback;
        StartCoroutine(CoPlay());
    }

    public IEnumerator CoPlay() {
        Initialize();
        for (int i = 0; i < list.Count; i += 2) {
            list[i].Play();
        }
        yield return new WaitForSeconds(0.25f);
        for (int i = 1; i < list.Count; i += 2) {
            list[i].Play();
        }
        yield return new WaitForSeconds(1.5f);
        cb?.Invoke();
        yield break;
    }

}