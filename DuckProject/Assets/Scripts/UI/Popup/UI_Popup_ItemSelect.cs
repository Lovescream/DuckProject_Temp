using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

public class UI_Popup_ItemSelect : UI_Popup {

    #region Enums

    enum GameObjects {
        ItemCardSelectList,
    }

    enum Buttons {
        btnADRefresh,
        btnCardRefresh,
    }

    enum Texts {
        txtRefreshCount,
    }

    enum Images {
        OwnItemSlot_Icon_0,
        OwnItemSlot_Icon_1,
        OwnItemSlot_Icon_2,
        OwnItemSlot_Icon_3,
        OwnItemSlot_Icon_4,
        OwnItemSlot_Icon_5,
        OwnAccessorySlot_Icon_0,
        OwnAccessorySlot_Icon_1,
        OwnAccessorySlot_Icon_2,
        OwnAccessorySlot_Icon_3,
        OwnAccessorySlot_Icon_4,
        OwnAccessorySlot_Icon_5,
    }

    #endregion

    #region MonoBehaviours

    void OnEnable() {
        Initialize();
    }

    #endregion

    public override bool Initialize() {
        if (base.Initialize() == false) return false;

        BindObject(typeof(GameObjects));
        BindButton(typeof(Buttons));
        BindText(typeof(Texts));
        BindImage(typeof(Images));

        GetButton((int)Buttons.btnADRefresh).gameObject.BindEvent(OnBtnADRefresh);
        GetButton((int)Buttons.btnCardRefresh).gameObject.BindEvent(OnBtnCardRefresh);

        Refresh();

        SetItems();

        List<ItemBase> items = Main.Object.Player.Items.Weapons.Where(s => s.IsActivated).ToList();
        for (int i = 0; i < items.Count; i++) {
            GetImage(i).sprite = Main.Resource.Load<Sprite>($"Item_Icon_{items[i].Data.key.Split('_')[0]}.sprite");
            GetImage(i).enabled = true;
        }
        items = Main.Object.Player.Items.Accessories.Where(s => s.IsActivated).ToList();
        for (int i = 0; i < items.Count; i++) {
            GetImage(i + 6).sprite = Main.Resource.Load<Sprite>($"Item_Icon_{items[i].Data.key.Split('_')[0]}.sprite");
            GetImage(i + 6).enabled = true;
        }

        return true;
    }

    private void Refresh() {
        string content = Main.Local.Get("UI", "txtRefreshCount");
        if (Main.Object.Player.ItemRefreshCount > 0)
            GetText((int)Texts.txtRefreshCount).text = $"{content}{Main.Object.Player.ItemRefreshCount}";
        else
            GetText((int)Texts.txtRefreshCount).text = $"<color=red>{content}0</color>";
    }

    private void SetItems() {
        GameObject parent = GetObject((int)GameObjects.ItemCardSelectList);
        parent.DestroyChilds();

        List<ItemBase> list = Main.Object.Player.Items.RecommendItems();

        foreach (ItemBase item in list) {
            UI_ItemCard itemCard = Main.UI.CreateSubItem<UI_ItemCard>(parent.transform);
            itemCard.GetComponent<UI_ItemCard>().SetInfo(item);
        }

    }

    private void OnBtnADRefresh() {

    }
    private void OnBtnCardRefresh() {
        if (Main.Object.Player.ItemRefreshCount > 0) {
            SetItems();
            Main.Object.Player.ItemRefreshCount--;
        }
        Refresh();
    }
}