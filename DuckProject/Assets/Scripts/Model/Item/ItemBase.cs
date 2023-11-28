using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemBase : Thing {

    #region Properties
    
    public ItemData Data { get; protected set; }
    public ItemData Data_Next => Main.Data.Items.TryGetValue($"{itemName}_{Level + 1:D2}", out ItemData data) ? data : null;

    public Creature Owner { get; protected set; }

    public ItemName itemName { get; protected set; }

    public int Level { get; set; }

    public bool IsActivated => Level > 0;

    #endregion

    public void SetInfo(Creature owner, ItemName type) {
        Owner = owner;
        itemName = type;
        if (Level <= 0) {
            DeactivateItem();
        }
        else {
            if (!Main.Data.Items.TryGetValue($"{itemName}_{Level:D2}", out ItemData data)) return;
            Data = data;
            ActivateItem();
        }
        OnChangedItemData();
    }

    public virtual void OnChangedItemData() { }

    public virtual void ActivateItem() { }
    public virtual void DeactivateItem() { }

    public virtual void OnLevelUp() {
        Level++;
        SetInfo(Owner, itemName);
    }

    protected virtual Projectile CreateProjectile(ItemBase itemBase, Creature owner, string prefabName, Vector2 position) {
        return Main.Object.Spawn<Projectile>($"{prefabName}.prefab", position).SetInfo(Owner, this);
    }

}

public enum ItemName {
    NONE,
    Shovel,
    Popgun,
    Ball,
    Bow,
    Stick,
    PoisonBottle,
    Knife,
    FireWand,
    WaterGun,
    Watermelon,
    IceWand,
    PigeonLauncher,
    BananaPeel,
    SoccerBall,
    _ITEMTYPEWEAPON,
    IcedAmericano,
    GoldfishBag,
    RingOfWaves,
    Parasol,
    HeartNecklace,
    WaterdropEarrings,
    CloudBracelet,
    Feather,
    Acorn,
    WitchsBroom,
    Magnet,
    RubberDuck,
    _ITEMTYPEACCESSORY,
}