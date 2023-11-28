using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PigeonProjectile : Projectile {

    #region Fields

    // Components.
    private Animator animator;
    private CircleCollider2D circleCollider;

    #endregion

    #region MonoBehaviours

    protected override void OnTriggerEnter2D(Collider2D collision) {
        Creature creature = collision.GetComponent<Creature>();
        if (!creature.IsValid() || !this.IsValid()) return;
        if (creature == Owner) return;

        Explosion();
    }

    #endregion

    #region Initialize / Set

    public override bool Initialize() {
        base.Initialize();

        animator = this.GetComponent<Animator>();
        circleCollider = this.GetComponent<CircleCollider2D>();

        return true;
    }

    public override Projectile SetInfo(Creature owner, ItemBase source) {
        animator.SetBool("Explosion", false);

        return base.SetInfo(owner, source);
    }

    #endregion

    public void OnAnimationEnd() {
        Main.Object.Despawn(this);
    }

    private void Explosion() {
        animator.SetBool("Explosion", true);

        float radius = circleCollider.radius * Source.Data.scaleMultiplier * Owner.ProjectileScaleMultiplier;
        RaycastHit2D[] targets = Physics2D.CircleCastAll(this.transform.position, radius, Vector2.zero, 0);
        foreach (RaycastHit2D target in targets) {
            Creature creature = target.transform.GetComponent<Creature>();
            if (!creature.IsValid() || !this.IsValid()) continue;
            if (creature == Owner) continue;

            creature.OnHit(Owner, Source, projectile: this);
        }
    }


}