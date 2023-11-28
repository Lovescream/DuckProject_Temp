using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneManagerEx {

    public BaseScene CurrentScene => GameObject.FindObjectOfType<BaseScene>();

    public void LoadScene(string sceneName) {
        SceneChangeEffect_In effect = Main.Resource.Instantiate("SceneChangeEffect_In.prefab").GetComponent<SceneChangeEffect_In>();
        Time.timeScale = 1;
        effect.SetInfo(() => {
            Main.Resource.Destroy(Main.UI.SceneUI.gameObject);
            Main.Clear();
            SceneManager.LoadScene(sceneName);
        });
    }
}