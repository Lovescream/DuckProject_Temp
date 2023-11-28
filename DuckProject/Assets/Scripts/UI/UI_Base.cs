using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Diagnostics;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UI_Base : MonoBehaviour {

    #region Fields

    private Dictionary<Type, UnityEngine.Object[]> objects = new();
    private bool initialized = false;

    #endregion

    #region MonoBehaviours

    void Start() {
        Initialize();
    }

    #endregion

    #region Initialize

    public virtual bool Initialize() {
        if (initialized) return false;
        initialized = true;
        return true;
    }

    #endregion

    #region Binding

    private void Bind<T>(Type type) where T : UnityEngine.Object {
        string[] names = Enum.GetNames(type);
        UnityEngine.Object[] objs = new UnityEngine.Object[names.Length];
        objects.Add(typeof(T), objs);

        for (int i = 0; i < names.Length; i++) {
            if (typeof(T) == typeof(GameObject)) objs[i] = Utilities.FindChild(this.gameObject, names[i], true);
            else objs[i] = Utilities.FindChild<T>(this.gameObject, names[i], true);
            if (objs[i] == null) Debug.LogError($"[UI_Base: {this.name}] Failed to bind: {names[i]}");
        }
    }

    protected void BindObject(Type type) => Bind<GameObject>(type);
    protected void BindImage(Type type) => Bind<Image>(type);
    protected void BindText(Type type) => Bind<TextMeshProUGUI>(type);
    protected void BindButton(Type type) => Bind<Button>(type);

    private T Get<T>(int index) where T : UnityEngine.Object {
        if (!objects.TryGetValue(typeof(T), out UnityEngine.Object[] objs)) return null;
        return objs[index] as T;
    }

    protected GameObject GetObject(int index) => Get<GameObject>(index);
    protected Image GetImage(int index) => Get<Image>(index);
    protected TextMeshProUGUI GetText(int index) => Get<TextMeshProUGUI>(index);
    protected Button GetButton(int index) => Get<Button>(index);

    
    public static void BindEvent(GameObject obj, Action action = null, Action<BaseEventData> dragAction = null, UIEvent type = UIEvent.Click) {
        UI_EventHandler eventHandler = Utilities.GetOrAddComponent<UI_EventHandler>(obj);

        switch (type) {
            case UIEvent.Click:
                eventHandler.OnClickHandler -= action;
                eventHandler.OnClickHandler += action;
                break;
            case UIEvent.Preseed:
                eventHandler.OnPressedHandler -= action;
                eventHandler.OnPressedHandler += action;
                break;
            case UIEvent.PointerDown:
                eventHandler.OnPointerDownHandler -= action;
                eventHandler.OnPointerDownHandler += action;
                break;
            case UIEvent.PointerUp:
                eventHandler.OnPointerUpHandler -= action;
                eventHandler.OnPointerUpHandler += action;
                break;
            case UIEvent.Drag:
                eventHandler.OnDragHandler -= dragAction;
                eventHandler.OnDragHandler += dragAction;
                break;
            case UIEvent.BeginDrag:
                eventHandler.OnBeginDragHandler -= dragAction;
                eventHandler.OnBeginDragHandler += dragAction;
                break;
            case UIEvent.EndDrag:
                eventHandler.OnEndDragHandler -= dragAction;
                eventHandler.OnEndDragHandler += dragAction;
                break;
        }
    }

    #endregion

}