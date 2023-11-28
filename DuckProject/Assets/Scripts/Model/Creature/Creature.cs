using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Creature : Thing {

    #region Properties

    public CreatureData Data { get; private set; }

    // Inputs.
    public virtual Vector2 Direction => Vector2.zero;

    // Status.
    public float HpMax { get; protected set; }
    public float HpMaxIncresement { get; protected set; }
    public float HpMaxMultiplier { get; protected set; } = 1;
    public float HpRegen { get; protected set; }
    public float HealBonusMultiplier { get; protected set; } = 1;
    public float Damage { get; protected set; }
    public float DamageMultiplier { get; protected set; } = 1;
    public float Defense { get; protected set; }
    public float DefenseBonus { get; protected set; }
    public float DefenseMultiplier { get; protected set; } = 1;
    public float MoveSpeed { get; protected set; }
    public float MoveSpeedMultiplier { get; protected set; } = 1;

    public float CooldownMultiplier {
        get => cooldownMultiplier;
        set {
            cooldownMultiplier = Mathf.Clamp(value, 0.2f, 2f);
        }
    }
    public float ProjectileScaleMultiplier {
        get => projectileScaleMultiplier;
        set {
            projectileScaleMultiplier = Mathf.Clamp(value, 0.1f, 2.2f);
        }
    }
    public float ProjectileSpeedMultiplier {
        get => projectileSpeedMultiplier;
        set => projectileSpeedMultiplier = value;
    }
    public float ProjectileDurationMultiplier {
        get => projectileDurationMultiplier;
        set {
            projectileDurationMultiplier = Mathf.Clamp(value, 0.1f, 5f);
        } 
    }
    public int ProjectileCountBonus { get; set; } = 0;
    public float PickUpDistanceMultiplier { get; set; } = 1;
    public float ExpMultiplier { get; set; } = 1;

    public float Hp {
        get => hp;
        set {
            if (value > HpMax) hp = HpMax;
            else if (value <= 0) {
                hp = 0;
                if (State != CreatureState.Dead)
                    State = CreatureState.Dead;
            }
            else hp = value;
        }
    }

    // State.
    public CreatureState State {
        get => state;
        set {
            state = value;
            switch (State) {
                case CreatureState.Idle: OnStateEntered_Idle(); break;
                case CreatureState.Move: OnStateEntered_Move(); break;
                case CreatureState.Attack: OnStateEntered_Attack(); break;
                case CreatureState.Hit: OnStateEntered_Hit(); break;
                case CreatureState.Dead: OnStateEntered_Dead(); break;
            }
        }
    }
    public bool IsSlow { get; protected set; }
    public float SlowTimer {
        get => slowTimer;
        set {
            if (value <= 0) {
                slowTimer = 0;
                if (IsSlow) {
                    IsSlow = false;
                    MoveSpeedMultiplier += 0.75f;
                    SetStatus(false);
                }
            }
            else {
                slowTimer = value;
                if (!IsSlow) {
                    IsSlow = true;
                    MoveSpeedMultiplier -= 0.75f;
                    SetStatus(false);
                }
            }
        }
    }
    public bool IsFrozen { get; protected set; }
    public float FrozenTimer {
        get => frozenTimer;
        set {
            if (value <= 0) {
                frozenTimer = 0;
                if (IsFrozen) {
                    IsFrozen = false;
                    spriter.color = new(0.3f, 0.3f, 0.8f, 1f);
                }
            }
            else {
                frozenTimer = value;
                if (!IsFrozen) {
                    IsFrozen = true;
                    spriter.color = Color.white;
                }
            }
        }
    }
    public bool IsSlipped { get; protected set; }
    public float SlipTimer {
        get => slipTimer;
        set {
            if (value <= 0) {
                slipTimer = 0;
                if (IsSlipped) {
                    IsSlipped = false;
                    this.transform.DORotate(Vector3.zero, 0);
                }
            }
            else {
                slipTimer = value;
                if (!IsSlipped) {
                    IsSlipped = true;
                    this.transform.DORotate(new Vector3(0, 0, 180), 0.25f);
                }
            }
        }
    }

    // Skill.
    public Inventory Items { get; private set; }

    #endregion

    #region Fields

    private float cooldownMultiplier = 1;
    private float projectileScaleMultiplier = 1;
    private float projectileSpeedMultiplier = 1;
    private float projectileDurationMultiplier = 1;
    

    protected float hp;

    // State.
    private CreatureState state;
    private float slowTimer;
    private float frozenTimer;
    private float slipTimer;
    protected List<Accessory> accessories = new();

    // Components.
    protected SpriteRenderer spriter;
    protected Collider2D enemyCollider;
    protected Rigidbody2D rigid;
    protected Animator animator;

    #endregion

    #region MonoBehaviours

    protected virtual void Update() {
        SlowTimer -= Time.deltaTime;
        FrozenTimer -= Time.deltaTime;
        SlipTimer -= Time.deltaTime;

        if (State != CreatureState.Dead)
            Hp += HpRegen * HealBonusMultiplier * Time.deltaTime;
    }

    #endregion

    public override bool Initialize() {
        base.Initialize();

        spriter = this.GetComponent<SpriteRenderer>();
        enemyCollider = this.GetComponent<Collider2D>();
        rigid = this.GetComponent<Rigidbody2D>();
        animator = this.GetComponent<Animator>();

        Items = this.gameObject.GetOrAddComponent<Inventory>();

        return true;
    }

    public virtual void SetInfo(string key) {
        Initialize();


        Data = Main.Data.Creatures[key];

        SetStatus();

        SetItems();

        this.transform.rotation = Quaternion.Euler(0, 0, 0);
    }

    public virtual void SetStatus(bool isFullHp = true) {
        HpMax = (Data.hpMax + Data.hpMaxIncresement * (Main.Game.CurrentWave.index - 1)) * HpMaxMultiplier;
        Damage = (Data.damage + Data.damageIncresement * (Main.Game.CurrentWave.index - 1)) * DamageMultiplier;
        Defense = (Data.defense + DefenseBonus) * DefenseMultiplier;
        MoveSpeed = Data.moveSpeed * MoveSpeedMultiplier;

        if (isFullHp) Hp = HpMax;
    }

    public virtual void SetItems() {
        foreach (string itemKey in Data.itemTypeList) {
            ItemName type = (ItemName)Enum.Parse(typeof(ItemName), itemKey);
            if (type != ItemName.NONE && type != ItemName._ITEMTYPEACCESSORY && type != ItemName._ITEMTYPEWEAPON) Items.Add(type);
        }
    }

    public virtual void AddAccessory(Accessory item) {
        if (accessories.Contains(item)) return;
        accessories.Add(item);

        HpMaxMultiplier += item.Data.hpMaxMultiplier;
        HpRegen += item.Data.hpRegen;
        HealBonusMultiplier += item.Data.healBonusMultiplier;
        DamageMultiplier += item.Data.damageMultiplier;
        DefenseBonus += item.Data.defense;
        DefenseMultiplier += item.Data.defenseMultiplier;
        MoveSpeedMultiplier += item.Data.moveSpeedMultiplier;
        CooldownMultiplier += item.Data.cooldownMultiplier;
        ProjectileScaleMultiplier += item.Data.scaleMultiplier;
        ProjectileSpeedMultiplier += item.Data.speed;
        ProjectileDurationMultiplier += item.Data.duration;
        ProjectileCountBonus += item.Data.projectileCount;
        PickUpDistanceMultiplier += item.Data.pickupMultiplier;
        ExpMultiplier += item.Data.expMultiplier;

        SetStatus(false);
    }
    public virtual void RemoveAccessory(Accessory item) {
        if (!accessories.Contains(item)) return;
        accessories.Remove(item);

        HpMaxMultiplier -= item.Data.hpMaxMultiplier;
        HpRegen -= item.Data.hpRegen;
        HealBonusMultiplier -= item.Data.healBonusMultiplier;
        DamageMultiplier -= item.Data.damageMultiplier;
        DefenseBonus -= item.Data.defense;
        DefenseMultiplier -= item.Data.defenseMultiplier;
        MoveSpeedMultiplier -= item.Data.moveSpeedMultiplier;
        CooldownMultiplier -= item.Data.cooldownMultiplier;
        ProjectileScaleMultiplier -= item.Data.scaleMultiplier;
        ProjectileSpeedMultiplier -= item.Data.speed;
        ProjectileDurationMultiplier -= item.Data.duration;
        ProjectileCountBonus -= item.Data.projectileCount;
        PickUpDistanceMultiplier -= item.Data.pickupMultiplier;
        ExpMultiplier -= item.Data.expMultiplier;


        SetStatus(false);
    }


    #region State

    public virtual void OnStateEntered_Idle() { }
    public virtual void OnStateEntered_Move() { }
    public virtual void OnStateEntered_Attack() { }
    public virtual void OnStateEntered_Hit() { }
    public virtual void OnStateEntered_Dead() { }

    #endregion


    public virtual void OnHit(Creature attacker, ItemBase skill = null, float damage = 0, Projectile projectile = null) {
        Hp -= damage;
        Main.Object.ShowDamageText(this.transform.position, damage);
        if (State != CreatureState.Dead)
        State = CreatureState.Hit;
    }

}