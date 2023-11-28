using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UI_AudioController : UI_Base {

    #region Enums

    enum Texts {
        txtAudioType,
        txtVolume,
    }
    enum Images {
        iconMuteOff,
        iconMuteOn,
    }
    enum Buttons {
        btnMuteToggler,
        btnVolumeUp,
        btnVolumeDown,
    }

    #endregion

    #region Fields

    private AudioType type;

    #endregion

    #region MonoBehaviours

    void OnEnable() {
        Initialize();
    }

    #endregion

    public override bool Initialize() {
        if (!base.Initialize()) return false;

        BindButton(typeof(Buttons));
        BindText(typeof(Texts));
        BindImage(typeof(Images));

        GetButton((int)Buttons.btnMuteToggler).gameObject.BindEvent(OnBtnMuteToggler);
        GetButton((int)Buttons.btnVolumeUp).gameObject.BindEvent(OnBtnVolumeUp);
        GetButton((int)Buttons.btnVolumeDown).gameObject.BindEvent(OnBtnVolumeDown);

        if (this.name.Contains("BGM")) type = AudioType.BGM;
        else type = AudioType.SFX;
        GetText((int)Texts.txtAudioType).text = type.ToString();

        Refresh();

        return true;
    }

    private void Refresh() {
        GetText((int)Texts.txtVolume).text = $"{100 * Main.Audio.GetVolume(type):F0}";
        GetImage((int)Images.iconMuteOff).gameObject.SetActive(!Main.Audio.IsMute(type));
        GetImage((int)Images.iconMuteOn).gameObject.SetActive(Main.Audio.IsMute(type));
        GetButton((int)Buttons.btnVolumeUp).gameObject.SetActive(Main.Audio.GetVolume(type) < 1);
        GetButton((int)Buttons.btnVolumeDown).gameObject.SetActive(Main.Audio.GetVolume(type) > 0);
    }

    private void OnBtnMuteToggler() {
        Main.Audio.SetMute(type, !Main.Audio.IsMute(type));
        Refresh();
    }
    private void OnBtnVolumeUp() {
        Main.Audio.SetVolume(type, Main.Audio.GetVolume(type) + 0.05f);
        Refresh();
    }
    private void OnBtnVolumeDown() {
        Main.Audio.SetVolume(type, Main.Audio.GetVolume(type) - 0.05f);
        Refresh();
    }

}