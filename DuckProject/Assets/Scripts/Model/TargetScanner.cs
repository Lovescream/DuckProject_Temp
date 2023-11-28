using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class TargetScanner : MonoBehaviour {

    public RaycastHit2D[] Targets { get; private set; }
    public Transform NearestTarget { get; private set; }

    private float range;
    private LayerMask layerMask;

    public void Set(float range, LayerMask layerMask) {
        this.range = range;
        this.layerMask = layerMask;
    }

    public Transform GetNearest() {
        Targets = Physics2D.CircleCastAll(this.transform.position, range, Vector2.zero, 0, layerMask);
        if (Targets.Length <= 0) return null;
        NearestTarget = Targets.OrderBy(x => Vector3.Distance(x.transform.position, this.transform.position)).First().transform;
        return NearestTarget;
    }

}