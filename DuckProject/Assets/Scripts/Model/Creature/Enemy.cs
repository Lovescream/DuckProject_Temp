using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : Creature {

    #region Parameters

    private readonly float KNOCKBACK_TIME = 0.1f;
    private readonly float KNOCKBACK_SPEED = 10f;

    #endregion

    #region Properties

    // Inputs.
    public override Vector2 Direction => (target != null) ? (target.transform.position - this.transform.position).normalized : new Vector2(UnityEngine.Random.Range(-1f,1f), UnityEngine.Random.Range(-1f,1f)).normalized;

    #endregion

    #region Fields

    private Thing target;

    private Coroutine coHitPlayer;
    private Coroutine coKnockback;
    private Coroutine coDead;

    public event Action<Enemy> EnemyInfoUpdate;

    #endregion

    #region MonoBehaviours

    void FixedUpdate() {
        if (IsFrozen) return;
        if (IsSlipped) return;
        if (State == CreatureState.Move) {
            Vector2 delta = target.transform.position - this.transform.position;
            rigid.MovePosition(rigid.position + delta.normalized * MoveSpeed * Time.fixedDeltaTime);
            rigid.velocity = Vector2.zero;
        }
    }

    void LateUpdate() {
        if (IsFrozen) return;
        if (IsSlipped) return;
        if (State == CreatureState.Move)
            spriter.flipX = target.transform.position.x < rigid.position.x;
    }

    protected virtual void OnCollisionEnter2D(Collision2D collision) {
        Player player = collision.gameObject.GetComponent<Player>();
        if (!player.IsValid() || !this.IsValid()) return;

        if (coHitPlayer != null) StopCoroutine(coHitPlayer);
        coHitPlayer = StartCoroutine(CoHitPlayer(player));
    }

    protected virtual void OnCollisionExit2D(Collision2D collision) {
        Player player = collision.gameObject.GetComponent<Player>();
        if (!player.IsValid() || !this.IsValid()) return;

        if (coHitPlayer != null) StopCoroutine(coHitPlayer);
        coHitPlayer = null;
    }

    #endregion

    #region Initialize / Set

    public override bool Initialize() {
        base.Initialize();

        return true;
    }

    public override void SetInfo(string key) {
        base.SetInfo(key);

        // Animator 설정.
        animator.runtimeAnimatorController = Main.Resource.Load<RuntimeAnimatorController>($"{Data.key}.animController");

        // Component 초기화.
        enemyCollider.enabled = true;
        rigid.simulated = true;

        // State 설정.
        State = CreatureState.Move;
        target = Main.Object.Player;
        FrozenTimer = 0;
        SlowTimer = 0;
        SlipTimer = 0;

        // Animation 초기화.
        animator.SetBool("Dead", false);

        UpdateEnemyData();
    }

    #endregion

    #region State

    public override void OnStateEntered_Hit() {
        base.OnStateEntered_Hit();

        animator.SetTrigger("Hit");
    }
    public override void OnStateEntered_Dead() {
        base.OnStateEntered_Dead();

        enemyCollider.enabled = false;
        rigid.simulated = false;
        animator.SetBool("Dead", true);

        Main.Object.Player.KillCount++;

        Gem gem = Main.Object.Spawn<Gem>("", this.transform.position);
        gem.SetInfo(Data.exp);

        if (coDead != null) StopCoroutine(coDead);
        coDead = StartCoroutine(CoDead());

        UpdateEnemyData();
    }

    #endregion

    public override void OnHit(Creature attacker, ItemBase item, float damage = 0, Projectile projectile = null) {
        if (item.itemName == ItemName.FireWand && projectile != null) {
            damage = (Main.Object.Player.Damage + item.Data.damage * ((float)projectile.PenetrationCount / item.Data.penetrationCount)) * (Main.Object.Player.DamageMultiplier * item.Data.damageMultiplier);
        }
        else {
            damage = (Main.Object.Player.Damage + item.Data.damage) * (Main.Object.Player.DamageMultiplier * item.Data.damageMultiplier);
        }
        base.OnHit(attacker, item, damage);
        UpdateEnemyData();

        if (coKnockback == null) coKnockback = StartCoroutine(CoKnockback(item.transform.position));
    }

    public void UpdateEnemyData() {
        if (this.IsValid() && this.gameObject.IsValid()) {
            EnemyInfoUpdate?.Invoke(this);
        }
    }

    private IEnumerator CoHitPlayer(Player player) {
        while (player != null) {
            player.OnHit(this, damage: Damage);
            yield return new WaitForSeconds(1f); // TODO:: NO HARDCODING!
        }
    }

    private IEnumerator CoKnockback(Vector2 origin) {
        float elapsed = 0;
        while (elapsed < KNOCKBACK_TIME) {
            elapsed += Time.deltaTime;

            Vector2 direction = (Vector2)this.transform.position - origin;
            Vector2 deltaPosition = direction.normalized * KNOCKBACK_SPEED * Time.fixedDeltaTime;
            rigid.MovePosition(rigid.position + deltaPosition);

            yield return null;
        }
        OnKnockbackEnd();
        coKnockback = null;
        yield break;
    }

    private IEnumerator CoDead() {
        yield return new WaitForSeconds(1.11f);
        coDead = null;
        Main.Object.Despawn(this);
        StopAllCoroutines();
        yield break;
    }
    
    private void OnKnockbackEnd() {
        if (State != CreatureState.Dead)
            State = CreatureState.Move;
    }
}