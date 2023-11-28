using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;

public class ResourceManager {

    public bool Loaded { get; set; }

    private Dictionary<string, UnityEngine.Object> resources = new();

    #region Addressable

    // key를 받아 리소스를 비동기 로드하고, 완료되면 콜백 호출.
    public void LoadAsync<T>(string key, Action<T> callback = null) where T : UnityEngine.Object {
        // 이미 로드된 리소스는 다시 로드하지 않고 콜백만 호출해준다.
        if (resources.TryGetValue(key, out UnityEngine.Object resource)) {
            callback?.Invoke(resource as T);
            return;
        }

        // key를 받아, 실제로 어떤 리소스를 로드할 것인지 정한다.
        // Ex. Sprite의 경우 key를 그대로 로드하면 Texture2D가 로드되므로, Sprite 정보가 담긴 키값을 따로 로드해야 한다.
        string loadKey = key;
        if (key.Contains(".sprite")) loadKey = $"{key}[{key.Replace(".sprite", "")}]";

        // 리소스 비동기 로드 시작.
        if (key.Contains(".sprite")) {
            var asyncOperation = Addressables.LoadAssetAsync<Sprite>(loadKey);
            asyncOperation.Completed += op => {
                resources.Add(key, op.Result);
                callback?.Invoke(op.Result as T);
            };
        }
        else {
            var asyncOperation = Addressables.LoadAssetAsync<T>(loadKey);
            asyncOperation.Completed += op => {
                resources.Add(key, op.Result);
                callback?.Invoke(op.Result);
            };
        }
    }

    // 해당 label을 가진 모든 리소스를 비동기 로드하고, 완료되면 콜백(key, 현재로드수, 전체로드수) 호출.
    public void LoadAllAsync<T>(string label, Action<string, int, int> callback) where T : UnityEngine.Object {
        var operation = Addressables.LoadResourceLocationsAsync(label, typeof(T));
        operation.Completed += op => {
            int loadCount = 0;
            int totalCount = op.Result.Count;

            foreach (var result in op.Result) {
                LoadAsync<T>(result.PrimaryKey, obj => {
                    loadCount++;
                    callback?.Invoke(result.PrimaryKey, loadCount, totalCount);
                });
            }
        };
    }

    #endregion


    public T Load<T>(string key) where T : UnityEngine.Object {
        if (!resources.TryGetValue(key, out UnityEngine.Object resource)) return null;
        return resource as T;
    }

    // 해당 key의 프리팹을 로드하여 풀에서 가져오거나 인스턴스화한다.
    public GameObject Instantiate(string key, Transform parent = null, bool pooling = false) {
        GameObject prefab = Load<GameObject>(key);
        if (prefab == null) {
            Debug.LogError($"[ResourceManager] Instantiate({key}): Failed to load prefab.");
            return null;
        }

        if (pooling) return Main.Pool.Pop(prefab);

        GameObject obj = UnityEngine.Object.Instantiate(prefab, parent);
        obj.name = prefab.name;
        return obj;
    }

    // 해당 오브젝트를 풀에 돌려놓거나 파괴한다.
    public void Destroy(GameObject obj) {
        if (obj == null) return;

        if (Main.Pool.Push(obj)) return;

        UnityEngine.Object.Destroy(obj);
    }


}