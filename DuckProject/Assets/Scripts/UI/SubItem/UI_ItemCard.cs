using System.Collections;
using System.Collections.Generic;
using System.Data;
using UnityEngine;

public class UI_ItemCard : UI_Base {

    #region Enums

    enum GameObjects {
        UI_ItemCard,
        NewItemIcon,
    }

    enum Texts {
        txtLevel,
        txtItemName,
        txtItemDescription,
    }

    enum Images {
        imgItemIcon
    }

    #endregion

    #region Fields

    private ItemBase item;

    #endregion

    #region MonoBehaviours

    void Awake() {
        Initialize();
    }

    #endregion

    #region Initialize / Set

    public override bool Initialize() {
        if (base.Initialize() == false) return false;

        // Binding.
        BindObject(typeof(GameObjects));
        BindText(typeof(Texts));
        BindImage(typeof(Images));
        this.gameObject.BindEvent(OnClicked);

        return true;
    }

    public void SetInfo(ItemBase item) {
        this.transform.localScale = Vector3.one;

        GetObject((int)GameObjects.NewItemIcon).SetActive(false);

        this.item = item;
        ItemData data = item.Data_Next;
        GetImage((int)Images.imgItemIcon).sprite = Main.Resource.Load<Sprite>($"Item_Icon_{data.key.Split('_')[0]}.sprite");
        GetText((int)Texts.txtItemName).text = Main.Local.Get("Item", $"Item_{data.key.Split('_')[0]}");
        GetText((int)Texts.txtItemDescription).text = Main.Local.Get("Item", $"Item_{data.key}_Description");
        GetText((int)Texts.txtLevel).text = $"Lv. {item.Level + 1}";
    }

    #endregion

    private void OnClicked() {
        Main.Object.Player.Items.LevelUp(item.itemName);
        Main.UI.ClosePopup();
    }
}