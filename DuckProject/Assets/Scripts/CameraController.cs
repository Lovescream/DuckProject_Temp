using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour {

    #region Properties

    public float CamWidth { get; private set; }
    public float CamHeight { get; private set; }
    public float ScreenRatio => (float)Screen.width / Screen.height;

    #endregion

    #region Fields

    private Transform target;

    #endregion

    #region MonoBehaviours

    void Awake() {
        Camera.main.orthographicSize = 10f;
        CamHeight = Camera.main.orthographicSize;
        CamWidth = CamHeight * ScreenRatio;
    }

    void LateUpdate() {
        Follow();
    }

    #endregion

    public void SetTarget(Transform target) {
        this.target = target;
    }

    public Vector2 GetRandomPositionInCamera() {
        Vector2 currentPosition = this.transform.position;

        Vector2 min = currentPosition - new Vector2(CamWidth, CamHeight);
        Vector2 max = currentPosition + new Vector2(CamWidth, CamHeight);

        Vector2 MapCenter = Main.Game.CurrentMap.Size / 2;
        float limitX = Main.Game.CurrentMap.Size.x * 0.5f - 1;
        float limitY = Main.Game.CurrentMap.Size.y * 0.5f - 1;

        float x = Mathf.Clamp(Random.Range(min.x, max.x), MapCenter.x - limitX, MapCenter.x + limitX);
        float y = Mathf.Clamp(Random.Range(min.y, max.y), MapCenter.y - limitY, MapCenter.y + limitY);

        return new(x, y);
    }

    private void Follow() {
        if (target == null || Main.Game.CurrentMap == null) return;

        Vector3 position = target.transform.position;
        Vector2 MapCenter = Main.Game.CurrentMap.Size / 2;

        float limitX = Main.Game.CurrentMap.Size.x * 0.5f - CamWidth;
        float limitY = Main.Game.CurrentMap.Size.y * 0.5f - CamHeight;
        float x = Mathf.Clamp(position.x, MapCenter.x - limitX, MapCenter.x + limitX);
        float y = Mathf.Clamp(position.y, MapCenter.y - limitY, MapCenter.y + limitY);
        float z = -10;

        this.transform.position = new(x, y, z);
    }
}