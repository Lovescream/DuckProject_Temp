using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Main : MonoBehaviour {

    #region Singleton

    private static Main instance;
    private static bool initialized;
    public static Main Instance {
        get {
            if (!initialized) {
                initialized = true;

                GameObject obj = GameObject.Find("@Main");
                if (obj == null) {
                    obj = new() { name = @"Main" };
                    obj.AddComponent<Main>();
                    DontDestroyOnLoad(obj);
                    instance = obj.GetComponent<Main>();
                }
            }
            return instance;
        }
    }
    #endregion

    private PoolManager pool = new();
    private ResourceManager resource = new();
    private ObjectManager objects = new();
    private UIManager ui = new();
    private DataManager data = new();
    private GameManager game = new();
    private LocalizationManager local = new();
    private AudioManager audio = new();
    private SceneManagerEx scene = new();
    public static PoolManager Pool => Instance?.pool;
    public static ResourceManager Resource => Instance?.resource;
    public static ObjectManager Object => Instance?.objects;
    public static UIManager UI => Instance?.ui;
    public static DataManager Data => Instance?.data;
    public static GameManager Game => Instance?.game;
    public static LocalizationManager Local => Instance?.local;
    public static AudioManager Audio => Instance?.audio;
    public static SceneManagerEx Scene => Instance?.scene;

    public static void Clear() {
        Audio.Clear();
        UI.Clear();
        Pool.Clear();
        Object.Clear();
    }

    #region CoroutineHelper

    public new static Coroutine StartCoroutine(IEnumerator coroutine) => (Instance as MonoBehaviour).StartCoroutine(coroutine);
    public new static void StopCoroutine(Coroutine coroutine) => (Instance as MonoBehaviour).StopCoroutine(coroutine);

    #endregion

}

public enum CreatureState {
    Idle,
    Move,
    Attack,
    Hit,
    Dead,
}
public enum ItemType {
    Weapon,
    Accessory,
}