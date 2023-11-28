using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PoisonFieldProjectile : Projectile {

    public IEnumerator Play() {
        this.transform.localScale = Vector3.zero;
        this.transform.DOScale(Source.Data.scaleMultiplier * Owner.ProjectileScaleMultiplier, 0.25f);
        yield return new WaitForSeconds(0.25f);
        yield return new WaitForSeconds(Source.Data.duration * Owner.ProjectileScaleMultiplier - 0.5f);
        this.transform.DOScale(0, 0.25f).OnComplete(() => {
            if (this.IsValid()) Main.Object.Despawn(this);
        });
    }

}