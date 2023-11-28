using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UI_Popup_GetItem : UI_Popup {

    #region Enums

    enum GameObjects {
        UI_ItemCard,
    }
    enum Buttons {
        Background,
    }
    enum Texts {
        txtGetTitle,
        txtGetDescription,
    }


    #endregion

    #region MonoBehaviours

    void OnEnable() {
        Initialize();
    }

    #endregion

    public override bool Initialize() {
        if (!base.Initialize()) return false;

        BindObject(typeof(GameObjects));
        BindButton(typeof(Buttons));
        BindText(typeof(Texts));

        GetButton((int)Buttons.Background).gameObject.BindEvent(OnBtnBackground);

        return true;
    }

    public void SetInfo() {

        ItemBase item = Main.Object.Player.Items.GetRandomActivatedItem(1)[0];
        if (item != null) {
            GetObject((int)GameObjects.UI_ItemCard).GetComponent<UI_ItemCard>().SetInfo(item);
            Main.Object.Player.Items.LevelUp(item.itemName);
        }
        else {
            // TODO:: 더 이상 얻을 수 있는 아이템이 없다면?
            Main.UI.ClosePopup(this);
        }

    }

    private void OnBtnBackground() {
        Main.UI.ClosePopup(this);
    }
}