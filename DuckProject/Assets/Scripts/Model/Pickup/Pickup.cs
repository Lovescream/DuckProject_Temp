using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pickup : Thing {

    #region Properties

    public PickupType Type { get; protected set; }
    public float PickDistance { get; private set; } = 4.0f;

    #endregion

    #region Fields

    protected Coroutine coCheckDistance;

    #endregion

    #region MonoBehaviours

    public virtual void OnDisable() {
        if (coCheckDistance != null) {
            StopCoroutine(coCheckDistance);
            coCheckDistance = null;
        }
    }

    #endregion

    #region Initialize

    public override bool Initialize() {
        base.Initialize();

        return true;
    }

    #endregion

    public virtual void GetPickup() {
        Main.Game.CurrentMap.Remove(this);

        if (coCheckDistance == null && this.IsValid()) {
            Vector2 direction = (this.transform.position - Main.Object.Player.transform.position).normalized;
            Vector2 destination = (Vector2)this.transform.position + direction * 1.5f;
            this.transform.DOMove(destination, 0.3f).SetEase(Ease.Linear).OnComplete(() => {
                coCheckDistance = StartCoroutine(CoCheckDistance());
            });
        }
    }

    public virtual void OnGetPickup() {

    }

    private IEnumerator CoCheckDistance() {
        while (this.IsValid() == true) {
            // Pickup을 Player를 향해 이동
            this.transform.position = Vector3.MoveTowards(this.transform.position, Main.Object.Player.transform.position, Time.deltaTime * 15f);

            // Pickup이 가까워지면, Pickup을 획득.
            if (Vector3.Distance(this.transform.position, Main.Object.Player.transform.position) < 1f) {
                OnGetPickup();
                yield break;
            }
            yield return new WaitForFixedUpdate();
        }
    }

}