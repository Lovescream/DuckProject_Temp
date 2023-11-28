using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public static class Extension {
    public static T GetOrAddComponent<T>(this GameObject obj) where T : Component {
        return Utilities.GetOrAddComponent<T>(obj);
    }

    public static void BindEvent(this GameObject go, Action action = null, Action<BaseEventData> dragAction = null, UIEvent type = UIEvent.Click) {
        UI_Base.BindEvent(go, action, dragAction, type);
    }

    public static bool IsValid(this GameObject obj) {
        return obj != null && obj.activeSelf;
    }
    public static bool IsValid(this Thing thing) {
        return thing != null && thing.isActiveAndEnabled;
    }

    public static void DestroyChilds(this GameObject obj) {
        Transform[] children = new Transform[obj.transform.childCount];
        for (int i = 0; i < obj.transform.childCount; i++)
            children[i] = obj.transform.GetChild(i);
        foreach (Transform child in children)
            Main.Resource.Destroy(child.gameObject);
    }

    public static void Shuffle<T>(this IList<T> list) {
        int n = list.Count;
        while (n>1) {
            n--;
            int k = UnityEngine.Random.Range(0, n + 1);
            (list[n], list[k]) = (list[k], list[n]);
        }
    }
}