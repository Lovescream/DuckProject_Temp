using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class StageClearInfo {
    public int index = 1;
    public int wave = 0;
    public int kill = 0;
    public float time = 0;
    public bool isClear = false;
}