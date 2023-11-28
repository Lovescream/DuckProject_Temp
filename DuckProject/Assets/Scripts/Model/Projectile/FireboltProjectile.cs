using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireboltProjectile : Projectile {

    private float originScale;

    public override Projectile SetInfo(Creature owner, ItemBase source) {
        base.SetInfo(owner, source);

        originScale = this.transform.localScale.x;

        cbOnPenetrate -= OnPenetrate;
        cbOnPenetrate += OnPenetrate;

        return this;
    }

    private void OnPenetrate(int remainCount) {
        float size = (float)remainCount / Source.Data.penetrationCount;
        this.transform.localScale = Vector3.one * originScale * size;
    }

}