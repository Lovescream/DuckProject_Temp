using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : Thing {

    #region Properties

    public Creature Owner { get; private set; }
    public ItemBase Source { get; private set; }

    public int PenetrationCount { get; private set; }
    public float Duration { get; private set; }

    #endregion

    #region Fields

    private float rotateSpeed;

    // Components.
    protected Rigidbody2D rigid;

    // Callbacks.
    protected Action<int> cbOnPenetrate;

    #endregion

    #region MonoBehaviours

    protected virtual void FixedUpdate() {
        if (rotateSpeed != 0) {
            this.transform.Rotate(new(0, 0, rotateSpeed * Time.fixedDeltaTime));
        }
    }

    protected virtual void OnTriggerEnter2D(Collider2D collision) {
        Creature creature = collision.GetComponent<Creature>();
        if (!creature.IsValid() || !this.IsValid()) return;
        if (creature.State == CreatureState.Dead) return;
        if (creature == Owner) return;

        creature.OnHit(Owner, Source, projectile: this);

        // 관통 횟수가 남지 않으면 소멸.
        if (PenetrationCount > 0) {
            PenetrationCount--;
            cbOnPenetrate?.Invoke(PenetrationCount);
        }
        else {
            rigid.velocity = Vector2.zero;
            if (this.IsValid()) Main.Object.Despawn(this);
        }
    }

    void OnDisable() {
        StopAllCoroutines();
    }

    #endregion

    #region Initialize / Set

    public override bool Initialize() {
        base.Initialize();

        return true;
    }

    public virtual Projectile SetInfo(Creature owner, ItemBase source) {
        this.Owner = owner;
        this.Source = source;

        rigid = this.GetComponent<Rigidbody2D>();

        this.transform.localScale = Vector3.one * source.Data.scaleMultiplier * Owner.ProjectileScaleMultiplier;
        PenetrationCount = source.Data.penetrationCount;
        Duration = source.Data.duration * Owner.ProjectileDurationMultiplier;

        if (this.gameObject.activeInHierarchy) StartCoroutine(CoCheckDestroy());

        return this;
    }

    public void SetRotation(float rotateSpeed) {
        this.rotateSpeed = rotateSpeed;
    }

    public void SetRotation(Vector2 origin, Vector2 toRotate) {
        this.transform.rotation = Quaternion.FromToRotation(origin, toRotate);
    }

    public void SetVelocity(Vector2 velocity) {
        rigid.velocity = velocity;
    }

    #endregion

    private IEnumerator CoCheckDestroy() {
        yield return new WaitForSeconds(Duration);
        Main.Object.Despawn(this);
    }

}