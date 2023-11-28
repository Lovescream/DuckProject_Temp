using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UI_Joystick : UI_Scene {

    #region Fields

    private Vector2 dragBeginPosition;

    private float radius;

    // Components.
    private GameObject background;
    private GameObject handler;

    #endregion

    #region Enums

    enum GameObjects {
        Background,
        Handler,
    }

    #endregion

    #region MonoBehaviours

    void OnDestroy() {
        Main.UI.OnTimeScaleChanged -= OnTimeScaleChanged;
    }

    #endregion

    #region Initialize

    public override bool Initialize() {
        base.Initialize();

        BindObject(typeof(GameObjects));
        background = GetObject((int)GameObjects.Background);
        handler = GetObject((int)GameObjects.Handler);

        this.gameObject.BindEvent(OnPointerDown, null, type: UIEvent.PointerDown);
        this.gameObject.BindEvent(null, OnDrag, type:UIEvent.Drag);
        this.gameObject.BindEvent(OnPointerUp, null, type: UIEvent.PointerUp);
        Main.UI.OnTimeScaleChanged += OnTimeScaleChanged;

        radius = background.GetComponent<RectTransform>().sizeDelta.y / 5;

        Activate(false);

        return true;
    }

    #endregion

    #region Event

    private void OnPointerDown() {
        Activate(true);

        dragBeginPosition = Input.mousePosition;
        background.transform.position = dragBeginPosition;
        handler.transform.position = dragBeginPosition;
    }

    private void OnDrag(BaseEventData baseEventData) {
        PointerEventData pointerEventData = baseEventData as PointerEventData;
        Vector2 dragPosition = pointerEventData.position;

        Vector2 delta = dragPosition - dragBeginPosition;

        Vector2 newPosition = dragBeginPosition + delta.normalized * Mathf.Clamp(delta.sqrMagnitude, 0, radius);

        handler.transform.position = newPosition;


        Main.Object.Player.Input = delta.normalized;
    }

    private void OnPointerUp() {
        Main.Object.Player.Input = Vector2.zero;
        handler.transform.position = background.transform.position;
        Activate(false);
    }

    #endregion

    private void Activate(bool isActive) {
        background.GetComponent<Image>().DOFade(isActive ? 1 : 0, 0.25f);
        handler.GetComponent<Image>().DOFade(isActive ? 1 : 0, 0.25f);
    }

    private void OnTimeScaleChanged(int timeScale) {
        if (timeScale == 1) {
            this.gameObject.SetActive(true);
            OnPointerUp();
        }
        else {
            this.gameObject.SetActive(false);
        }
    }
}