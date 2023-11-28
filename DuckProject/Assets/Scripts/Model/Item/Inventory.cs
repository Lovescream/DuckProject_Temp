using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;

public class Inventory : MonoBehaviour {

    #region Properties

    public List<ItemBase> Weapons => ItemList.Where(item => item.IsActivated && item.Data.type == ItemType.Weapon).ToList();
    public List<ItemBase> Accessories => ItemList.Where(item => item.IsActivated && item.Data.type == ItemType.Accessory).ToList();
    public List<ItemBase> ItemList => itemList;
    public List<ItemBase> ActivatedItems => ItemList.Where(s => s.IsActivated).ToList();

    #endregion

    #region Fields

    private List<ItemBase> itemList = new();

    #endregion
    

    public void Add(ItemName itemName) {
        if (Main.Data.Items[$"{itemName}_01"].type == ItemType.Weapon) {
            Type type = Type.GetType(itemName.ToString());
            ItemBase itemBase = this.gameObject.AddComponent(type) as ItemBase;
            itemBase.SetInfo(this.GetComponent<Creature>(), itemName);
            itemList.Add(itemBase);
        }
        else {
            Accessory accessory = this.gameObject.AddComponent<Accessory>();
            accessory.SetInfo(this.GetComponent<Creature>(), itemName);
            itemList.Add(accessory);
        }
    }

    public void LevelUp(ItemName itemType) {
        for (int i = 0; i < ItemList.Count; i++) {
            if (ItemList[i].itemName == itemType) {
                ItemList[i].OnLevelUp();
            }
        }
    }


    public List<ItemBase> GetRandomActivatedItem(int count = 3) {
        // #1. 선택할 수 있는 아이템 목록을 만든다.
        List<ItemBase> list = new();
        foreach (ItemBase item in ActivatedItems) {
            // 최대 레벨에 도달한 아이템은 제외한다.
            if (item.Level >= Main.Data.GetMaxLevel(item.itemName)) continue;

            list.Add(item);
        }

        // #2. 목록을 무작위로 섞어 개수만큼 리턴한다.
        list.Shuffle();
        return list.Take(count).ToList();
    }
    public List<ItemBase> RecommendItems(int count = 3) {
        // #1. 새 무기나 장신구를 포함할 수 있는지 검사한다.
        bool includeNewWeapon = ItemList.Count(item => item.IsActivated && item.Data.type == ItemType.Weapon) < 6;
        bool includeNewAccessory = ItemList.Count(item => item.IsActivated && item.Data.type == ItemType.Accessory) < 6;

        // #2. 선택할 수 있는 아이템 목록을 만든다.
        List<ItemBase> list = new();
        foreach (ItemBase item in ItemList) {
            // #2-A. 최대 레벨에 도달한 아이템은 제외한다.
            if (item.Level >= Main.Data.GetMaxLevel(item.itemName)) continue;
            // #2-B. 새 무기를 선택할 수 없으면 활성화되지 않은 무기는 제외한다.
            if (item.itemName < ItemName._ITEMTYPEWEAPON) {
                if (!includeNewWeapon && !item.IsActivated) continue;
            }
            // #2-C. 새 장신구를 선택할 수 없으면 활성화되지 않은 장신구는 제외한다.
            else if (item.itemName < ItemName._ITEMTYPEACCESSORY) {
                if (!includeNewAccessory && !item.IsActivated) continue;
            }

            list.Add(item);
        }

        // #3. 목록을 무작위로 섞어 개수만큼 리턴한다.
        list.Shuffle();
        return list.Take(count).ToList();
    }
}