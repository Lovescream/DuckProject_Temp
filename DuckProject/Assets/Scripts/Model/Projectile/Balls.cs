using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Balls : Thing {

    #region Properties

    public Creature Owner { get; private set; }
    public ItemBase Source { get; private set; }
    public bool IsCompleted { get; private set; }
    public int Count => Source.Data.projectileCount + Owner.ProjectileCountBonus;
    public float Radius => Source.Data.range;
    public float RotationSpeed => Source.Data.speed * Owner.ProjectileSpeedMultiplier;
    public float Duration => Source.Data.duration * Owner.ProjectileScaleMultiplier;
    public float AnimTime => Source.Data.cooldown / 5f;

    #endregion

    #region MonoBehaviours

    void Update() {
        this.transform.Rotate(0, 0, RotationSpeed * Time.deltaTime);
    }

    #endregion

    #region Initialize / Set

    public override bool Initialize() {
        base.Initialize();

        return true;
    }
    
    public void SetInfo(Creature owner, ItemBase source) {
        Owner = owner;
        Source = source;
        IsCompleted = false;
        StartCoroutine(CoAction());
    }

    #endregion

    private IEnumerator CoAction() {
        CreateBalls();
        yield return new WaitForSeconds(Duration - 2 * AnimTime);
        DestroyBalls();
        yield return new WaitForSeconds(AnimTime);
        IsCompleted = true;
        yield break;
    }

    private void CreateBalls() {
        float angle = 2 * Mathf.PI / Count;
        Debug.Log(Count);
        for (int i = 0; i < Count; i++) {
            float x = Mathf.Cos(i * angle) * Radius;
            float y = Mathf.Sin(i * angle) * Radius;
            Projectile ball = Main.Object.Spawn<Projectile>("BallProjectile.prefab", this.transform.position);
            ball.SetInfo(Owner, Source);
            ball.transform.SetParent(this.transform);
            ball.transform.DOLocalMove(new(x, y), AnimTime);
        }
    }
    private void DestroyBalls() {
        for (int i = 0; i < this.transform.childCount; i++) {
            Transform t = this.transform.GetChild(i);
            t.DOLocalMove(Vector2.zero, AnimTime).OnComplete(() => {
                if (t.gameObject.IsValid())
                    Main.Object.Despawn(t.GetComponent<Projectile>());
            });
        }
    }

}