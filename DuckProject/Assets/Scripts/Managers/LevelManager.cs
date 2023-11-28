using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelManager {

    public WaveData CurrentWave { get; private set; }
    
    public void NewWave(WaveData wave) {
        CurrentWave = wave;
    }
}