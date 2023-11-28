using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TreasureChest : Pickup {

    public override bool Initialize() {
        base.Initialize();
        Type = PickupType.TreasureChest;
        return true;
    }

    public override void OnGetPickup() {
        base.OnGetPickup();

        UI_Popup_GetItem popup = Main.UI.ShowPopupUI<UI_Popup_GetItem>();
        popup.SetInfo();

        Main.Object.Despawn(this);
    }

}