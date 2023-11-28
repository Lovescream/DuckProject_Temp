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
        // #1. ������ �� �ִ� ������ ����� �����.
        List<ItemBase> list = new();
        foreach (ItemBase item in ActivatedItems) {
            // �ִ� ������ ������ �������� �����Ѵ�.
            if (item.Level >= Main.Data.GetMaxLevel(item.itemName)) continue;

            list.Add(item);
        }

        // #2. ����� �������� ���� ������ŭ �����Ѵ�.
        list.Shuffle();
        return list.Take(count).ToList();
    }
    public List<ItemBase> RecommendItems(int count = 3) {
        // #1. �� ���⳪ ��ű��� ������ �� �ִ��� �˻��Ѵ�.
        bool includeNewWeapon = ItemList.Count(item => item.IsActivated && item.Data.type == ItemType.Weapon) < 6;
        bool includeNewAccessory = ItemList.Count(item => item.IsActivated && item.Data.type == ItemType.Accessory) < 6;

        // #2. ������ �� �ִ� ������ ����� �����.
        List<ItemBase> list = new();
        foreach (ItemBase item in ItemList) {
            // #2-A. �ִ� ������ ������ �������� �����Ѵ�.
            if (item.Level >= Main.Data.GetMaxLevel(item.itemName)) continue;
            // #2-B. �� ���⸦ ������ �� ������ Ȱ��ȭ���� ���� ����� �����Ѵ�.
            if (item.itemName < ItemName._ITEMTYPEWEAPON) {
                if (!includeNewWeapon && !item.IsActivated) continue;
            }
            // #2-C. �� ��ű��� ������ �� ������ Ȱ��ȭ���� ���� ��ű��� �����Ѵ�.
            else if (item.itemName < ItemName._ITEMTYPEACCESSORY) {
                if (!includeNewAccessory && !item.IsActivated) continue;
            }

            list.Add(item);
        }

        // #3. ����� �������� ���� ������ŭ �����Ѵ�.
        list.Shuffle();
        return list.Take(count).ToList();
    }
}