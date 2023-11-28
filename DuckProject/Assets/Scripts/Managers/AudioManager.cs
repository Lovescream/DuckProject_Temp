using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager {

    #region Fields

    private Dictionary<AudioType, bool> mute = new();
    private Dictionary<AudioType, float> volume = new();

    #endregion

    public void Initialize() {
        SetMute(AudioType.BGM, false);
        SetMute(AudioType.SFX, false);
        SetVolume(AudioType.BGM, 1);
        SetVolume(AudioType.SFX, 1);
    }

    public bool IsMute(AudioType type) => mute[type];
    public float GetVolume(AudioType type) => volume[type];
    public void SetMute(AudioType type, bool isMute) {
        this.mute[type] = isMute;
    }
    public void SetVolume(AudioType type, float volume) {
        this.volume[type] = Mathf.Clamp01(volume);
    }

    public void Clear() {
        // TODO:: 모든 AudioSource에 대해 Stop();
    }
}

public enum AudioType {
    BGM,
    SFX,
}