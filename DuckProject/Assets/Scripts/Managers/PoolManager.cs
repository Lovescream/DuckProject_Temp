using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;

public class Pool {
    private GameObject prefab;
    private IObjectPool<GameObject> pool;
    private Transform root;
    private Transform Root {
        get {
            if (root == null) {
                GameObject obj = new() { name = $"[Pool_Root] {prefab.name}" };
                root = obj.transform;
            }
            return root;
        }
    }

    public Pool(GameObject prefab) {
        this.prefab = prefab;
        this.pool = new ObjectPool<GameObject>(OnCreate, OnGet, OnRelease, OnDestroy);
    }

    public GameObject Pop() {
        return pool.Get();
    }

    public void Push(GameObject obj) {
        pool.Release(obj);
    }
    #region Callbacks

    private GameObject OnCreate() {
        GameObject obj = GameObject.Instantiate(prefab);
        obj.transform.SetParent(Root);
        obj.name = prefab.name;
        return obj;
    }
    private void OnGet(GameObject obj) {
        obj.SetActive(true);
    }
    private void OnRelease(GameObject obj) {
        obj.SetActive(false);
    }
    private void OnDestroy(GameObject obj) {
        GameObject.Destroy(obj);
    }

    #endregion
}

public class PoolManager {

    private Dictionary<string, Pool> pools = new();

    public GameObject Pop(GameObject prefab) {
        // #1. 풀이 없으면 새로 만든다.
        if (pools.ContainsKey(prefab.name) == false) CreatePool(prefab);

        // #2. 해당 풀에서 하나를 가져온다.
        return pools[prefab.name].Pop();
    }

    public bool Push(GameObject obj) {
        // #1. 풀이 있는지 확인한다. (보통 있는 것이 정상)
        if (pools.ContainsKey(obj.name) == false) return false;

        // #2. 풀에 게임오브젝트를 돌려준다.
        pools[obj.name].Push(obj);

        return true;
    }

    private void CreatePool(GameObject prefab) {
        Pool pool = new(prefab);
        pools.Add(prefab.name, pool);
    }

    public void Clear() {
        pools.Clear();
    }

}