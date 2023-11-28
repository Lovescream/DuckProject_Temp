using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class RepeatItem : ItemBase {

    protected virtual float Interval => Data.cooldown * Owner.CooldownMultiplier;
    protected virtual int ProjectileCount => Data.projectileCount + Owner.ProjectileCountBonus;

    private Coroutine coAttack;

    public override void ActivateItem() {
        base.ActivateItem();
        this.gameObject.SetActive(true);
        if (coAttack != null) StopCoroutine(coAttack);
        coAttack = StartCoroutine(CoStartAttack());
    }

    public override void DeactivateItem() {
        base.DeactivateItem();
        if (coAttack != null) StopCoroutine(coAttack);
        coAttack = null;
    }

    protected abstract void DoAttack();

    protected virtual IEnumerator CoStartAttack() {
        WaitForSeconds wait = new(Interval);
        yield return wait;
        while (true) {
            DoAttack();
            yield return wait;
        }
    }

}