using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DogGuard : Boss {

    public override void SetInfo(string key) {
        base.SetInfo(key);

        Items.LevelUp(ItemName.Popgun);
        Items.LevelUp(ItemName.Popgun);
        Items.LevelUp(ItemName.Popgun);
        Items.LevelUp(ItemName.Popgun);
    }

}