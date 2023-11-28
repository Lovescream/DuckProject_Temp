using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class UIManager {

    #region Properties

    public GameObject Root {
        get {
            GameObject root = GameObject.Find("@UI_Root");
            if (root == null) root = new("@UI_Root");
            return root;
        }
    }

    public UI_Scene SceneUI => sceneUI;

    #endregion

    #region Fields

    private int order = 10;

    private UI_Scene sceneUI;

    private Stack<UI_Popup> popups = new();

    // events.
    public event Action<int> OnTimeScaleChanged;

    #endregion

    public void SetCanvas(GameObject obj, bool sort = true, int sortOrder = 0) {
        Canvas canvas = obj.GetOrAddComponent<Canvas>();
        canvas.renderMode = RenderMode.ScreenSpaceOverlay;
        canvas.overrideSorting = true;

        CanvasScaler scaler = obj.GetOrAddComponent<CanvasScaler>();
        scaler.uiScaleMode = CanvasScaler.ScaleMode.ScaleWithScreenSize;
        scaler.referenceResolution = new(1080, 1920);

        obj.GetOrAddComponent<GraphicRaycaster>();

        if (sort) {
            canvas.sortingOrder = order;
            order++;
        }
        else {
            canvas.sortingOrder = sortOrder;
        }
    }

    #region SceneUI

    public T ShowSceneUI<T>(string name = null) where T : UI_Scene {
        if (string.IsNullOrEmpty(name)) name = typeof(T).Name;

        GameObject obj = Main.Resource.Instantiate($"{name}.prefab");
        obj.transform.SetParent(Root.transform);

        T sceneUI = obj.GetOrAddComponent<T>();
        this.sceneUI = sceneUI;

        return sceneUI;
    }

    #endregion

    #region Popup

    public T ShowPopupUI<T>(string name = null) where T : UI_Popup {
        if (string.IsNullOrEmpty(name)) name = typeof(T).Name;

        GameObject obj = Main.Resource.Instantiate(($"{name}.prefab"));
        obj.transform.SetParent(Root.transform);
        T popup = obj.GetOrAddComponent<T>();
        popups.Push(popup);

        RefreshTimeScale();

        return popup;
    }

    public void ClosePopup(UI_Popup popup) {
        if (popups.Count == 0) return;
        if (popups.Peek() != popup) {
            Debug.LogError($"[UIManager] ClosePopup({popup.name}): Close popup failed.");
            return;
        }
        // Managers.Sound.PlayPopupClose();
        ClosePopup();
    }

    public void ClosePopup() {
        if (popups.Count == 0) return;

        UI_Popup popup = popups.Pop();
        Main.Resource.Destroy(popup.gameObject);
        order--;

        RefreshTimeScale();
    }

    public void CloseAllPopup() {
        while (popups.Count > 0) ClosePopup();
    }

    public int GetPopupCount() => popups.Count;

    #endregion

    #region SubItem

    public T CreateSubItem<T>(Transform parent = null, string name = null, bool pooling = true) where T : UI_Base {
        if (string.IsNullOrEmpty(name)) name = typeof(T).Name;

        GameObject obj = Main.Resource.Instantiate($"{name}.prefab", parent, pooling);
        obj.transform.SetParent(parent);
        return Utilities.GetOrAddComponent<T>(obj);
    }

    #endregion

    public void Clear() {
        CloseAllPopup();
        Time.timeScale = 1;
        sceneUI = null;
    }

    public void RefreshTimeScale() {
        if (SceneManager.GetActiveScene().name != "GameScene") {
            Time.timeScale = 1;
            return;
        }
        if (popups.Count > 0) Time.timeScale = 0;
        else Time.timeScale = 1;
        OnTimeScaleChanged?.Invoke((int)Time.timeScale);
    }

}

public enum UIEvent {
    Click,
    Preseed,
    PointerDown,
    PointerUp,
    BeginDrag,
    Drag,
    EndDrag,
}