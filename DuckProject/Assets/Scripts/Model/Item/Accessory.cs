using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Accessory : ItemBase {

    public override void ActivateItem() {
        base.ActivateItem();

        Owner.AddAccessory(this);
    }

    public override void DeactivateItem() {
        base.DeactivateItem();
        Owner.RemoveAccessory(this);
    }

    public override void OnLevelUp() {
        Owner.RemoveAccessory(this);
        Level++;
        SetInfo(Owner, itemName);
    }

}