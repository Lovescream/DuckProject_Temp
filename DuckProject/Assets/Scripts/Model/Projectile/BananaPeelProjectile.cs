using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BananaPeelProjectile : Projectile {

    #region Fields

    private bool isActivated;

    #endregion

    #region MonoBehaviours

    protected override void OnTriggerEnter2D(Collider2D collision) {
        if (!isActivated) return;
        Creature creature = collision.GetComponent<Creature>();
        if (!creature.IsValid() || !this.IsValid()) return;
        if (creature == Owner) return;
        if (creature.transform.position.y <= this.transform.position.y) return;

        creature.SlipTimer += 3;
        creature.OnHit(Owner, Source, projectile: this);

        if (this.IsValid()) Main.Object.Despawn(this as Projectile);
    }

    #endregion

    #region Initialize / Set

    public override bool Initialize() {
        base.Initialize();
        return true;
    }

    public override Projectile SetInfo(Creature owner, ItemBase source) {
        isActivated = false;
        return base.SetInfo(owner, source);
    }

    #endregion

    public void Play(Vector2 targetPosition) {
        this.transform.DOMove(targetPosition, Source.Data.speed * Owner.ProjectileSpeedMultiplier).SetSpeedBased().SetEase(Ease.InQuad).OnComplete(Activate);
    }

    private void Activate() {
        isActivated = true;
        this.SetRotation(0);
    }
}