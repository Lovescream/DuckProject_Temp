using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
using static UnityEngine.GraphicsBuffer;

public class Player : Creature {

    #region Properties

    // Inputs.
    public override Vector2 Direction => (indicatorPoint.transform.position - this.transform.position).normalized;
    public Vector2 Input { get; set; }
    public Vector2 Velocity { get; private set; }

    public int ItemRefreshCount { get; set; }
    public int Level { get; private set; }
    public float Exp {
        get => exp;
        set {
            float delta = (value - exp) * ExpMultiplier;
            exp += delta;
            // LevelUp 체크.
            int level = Level;
            while (true) {
                // 다음 레벨이 없다면 break.
                if (!Main.Data.Levels.TryGetValue(level + 1, out _)) break;

                if (!Main.Data.Levels.TryGetValue(level, out LevelData currentLevelData)) break;
                if (Exp < currentLevelData.totalExp) break;
                level++;
            }
            // LevelUp 처리.
            if (level != Level) {
                Level = level;
                Main.Data.Levels.TryGetValue(level, out LevelData currentLevelData);
                RequireExp = currentLevelData.totalExp;
                cbOnPlayerLevelUp?.Invoke();
            }
            cbOnPlayerDataUpdated?.Invoke();
        }
    }
    public float RequireExp { get; private set; }
    public int KillCount {
        get => killCount;
        set {
            killCount = value;

            cbOnPlayerDataUpdated?.Invoke();
        }
    }
    public float ExpRatio {
        get {
            if (!Main.Data.Levels.TryGetValue(Level, out LevelData currentLevelData)) return 0;
            int currentLevelTotalExp = currentLevelData.totalExp;
            int prevLevelTotalExp = Main.Data.Levels.TryGetValue(Level - 1, out LevelData prevLevelData) ? prevLevelData.totalExp : 0;
            return (float)(Exp - prevLevelTotalExp) / (currentLevelTotalExp - prevLevelTotalExp);
        }
    }

    public float CollectDistance => 4.0f;

    #endregion

    #region Fields

    private float exp;
    private int killCount;

    private Transform indicator;
    private Transform indicatorPoint;

    // Callbacks.
    public Action cbOnPlayerLevelUp;
    public Action cbOnPlayerDataUpdated;

    #endregion

    #region MonoBehaviours

    void FixedUpdate() {
        if (IsFrozen) return;
        if (IsSlipped) return;

        Velocity = Input.normalized * MoveSpeed;

        rigid.MovePosition(rigid.position + Velocity * Time.fixedDeltaTime);

        if (Velocity.sqrMagnitude != 0) indicator.transform.eulerAngles = new Vector3(0, 0, Mathf.Atan2(-Velocity.x, Velocity.y) * 180 / Mathf.PI);
        if (Velocity.x != 0) spriter.flipX = Velocity.x < 0;

        animator.SetFloat("Speed", Velocity.magnitude);
        if (Velocity.magnitude != 0) {
            animator.SetFloat("X", Velocity.x);
            animator.SetFloat("Y", Velocity.y);
        }
    }

    protected override void Update() {
        base.Update();
        CollectPickup();
    }

    #endregion

    #region Initialize / Set

    public override bool Initialize() {
        base.Initialize();

        indicator = this.transform.Find("Indicator").transform;
        indicatorPoint = indicator.GetChild(0);

        FindObjectOfType<CameraController>().SetTarget(this.transform);

        return true;
    }

    public override void SetInfo(string key) {
        base.SetInfo(key);

        Level = 1;
        Exp = 0;
        ItemRefreshCount = 444;

        Items.LevelUp(ItemName.SoccerBall);
    }

    public override void SetItems() {
        base.SetItems();
    }

    #endregion

    public override void OnHit(Creature attacker, ItemBase item = null, float damage = 0, Projectile projectile = null) {
        if (item != null)
            damage = (attacker.Damage + item.Data.damage) * attacker.DamageMultiplier;
        base.OnHit(attacker, item, damage, projectile);
    }

    #region State

    public override void OnStateEntered_Dead() {
        base.OnStateEntered_Dead();

        Main.UI.ShowPopupUI<UI_Popup_Continue>();
    }

    #endregion

    private void CollectPickup() {
        List<Pickup> pickups = Main.Game.CurrentMap.GetPickups(this.transform.position, CollectDistance + 0.5f);

        foreach (Pickup pickup in pickups) {
            Vector3 delta = pickup.transform.position - this.transform.position;
            if (delta.sqrMagnitude <= (pickup.PickDistance * PickUpDistanceMultiplier) * (pickup.PickDistance * PickUpDistanceMultiplier)) pickup.GetPickup();
        }
    }

}